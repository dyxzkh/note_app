using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using note_webapi.Interfaces;
using note_webapi.Models;

namespace note_webapi.Repositories;

public class UserRepository(IConfiguration config, IBcryptService bcrypt, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly string? _connectionString = config.GetConnectionString("DefaultConnection");
    private readonly IBcryptService _bcrypt = bcrypt;
    private readonly ILogger<UserRepository> _logger = logger;
    private IDbConnection Connection => new SqlConnection(_connectionString);
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var db = Connection;
        return await db.QueryAsync<User>($"SELECT * FROM Users");
    }
    
    public async Task<User?> GetUserWithRoleByEmailAsync(string email)
    {
        using var db = Connection;

        var sql = @"
        SELECT u.*, r.RoleName
        FROM Users u
        INNER JOIN Roles r ON u.RoleId = r.Id
        WHERE u.Email = @Email";

        var result = await db.QueryAsync<User, string, User>(
            sql,
            (user, roleName) =>
            {
                user.RoleName = roleName;
                return user;
            },
            new { Email = email },
            splitOn: "RoleName"
        );

        return result.FirstOrDefault();
    }

    public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry)
    {
        using var db = Connection;
        await db.ExecuteAsync(
            "UPDATE Users SET RefreshToken = @Token, RefreshTokenExpiry = @Expiry WHERE Id = @Id",
            new { Token = refreshToken, Expiry = expiry, Id = userId });
    }

    public async Task<User?> GetByRefreshTokenAsync(string token)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE RefreshToken = @RefreshToken", 
            new { RefreshToken = token });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id", 
            new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email = @Email", 
            new { Email = email });
    }

    public async Task<RepositoryResult<int>> CreateAsync(User user)
    {
        using var db = Connection;
        
        // Check if email already exists
        var existingUser = await GetByEmailAsync(user.Email);
        if (existingUser != null)
        {
            return new RepositoryResult<int>
            {
                Success = false,
                ErrorMessage = "Email address is already in use",
                Data = 0
            };
        }

        try
        {
            var hashedPassword = _bcrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.CreatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Users (Fullname, Email, Password, CreatedAt, RoleId) 
                VALUES (@Fullname, @Email, @Password, @CreatedAt, @RoleId); 
                SELECT CAST(SCOPE_IDENTITY() as int);";
                
            var newId = await db.ExecuteScalarAsync<int>(sql, user);
            
            return new RepositoryResult<int>
            {
                Success = true,
                Data = newId
            };
        }
        catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
        {
            _logger.LogError(ex, "Failed to create user - duplicate email");
            return new RepositoryResult<int>
            {
                Success = false,
                ErrorMessage = "Email address is already in use",
                Data = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user");
            return new RepositoryResult<int>
            {
                Success = false,
                ErrorMessage = "An error occurred while creating the user",
                Data = 0
            };
        }
    }

    public async Task<RepositoryResult<bool>> UpdateAsync(User user)
    {
        using var db = Connection;
        
        // Check if email is being changed to one that already exists
        if (!string.IsNullOrEmpty(user.Email))
        {
            var existingUser = await GetByEmailAsync(user.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return new RepositoryResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Email address is already in use by another account",
                    Data = false
                };
            }
        }

        try
        {
            var hashedPassword = _bcrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Users 
                SET 
                    Fullname = @Fullname, 
                    Email = @Email, 
                    Password = @Password,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
                
            var rowsAffected = await db.ExecuteAsync(sql, user);
            
            return new RepositoryResult<bool>
            {
                Success = rowsAffected > 0,
                Data = rowsAffected > 0
            };
        }
        catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
        {
            _logger.LogError(ex, "Failed to update user - duplicate email");
            return new RepositoryResult<bool>
            {
                Success = false,
                ErrorMessage = "Email address is already in use",
                Data = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user");
            return new RepositoryResult<bool>
            {
                Success = false,
                ErrorMessage = "An error occurred while updating the user",
                Data = false
            };
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var db = Connection;
        try
        {
            return await db.ExecuteAsync(
                "DELETE FROM Users WHERE Id = @Id", 
                new { Id = id }) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user");
            return false;
        }
    }
}