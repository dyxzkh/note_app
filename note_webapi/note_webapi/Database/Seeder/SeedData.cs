using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using note_webapi.Interfaces;

public class SeedData
{
    private readonly string? _connectionString;
    private readonly IBcryptService _bcrypt;

    public SeedData(IBcryptService bcrypt, IConfiguration configuration)
    {
        _bcrypt = bcrypt;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task SeedAsync()
    {
        using var connection = Connection;

        // Create Roles if not exists
        var roleExists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Roles WHERE RoleName IN ('Admin', 'User')");

        if (roleExists == 0)
        {
            await connection.ExecuteAsync(@"
                INSERT INTO Roles (RoleName, RoleDescription, CreatedAt)
                VALUES 
                ('Admin', 'Administrator role', GETDATE()),
                ('User', 'Regular user role', GETDATE())
            ");
        }

        // Get Admin Role ID
        var adminRoleId = await connection.ExecuteScalarAsync<int>(
            "SELECT TOP 1 Id FROM Roles WHERE RoleName = 'Admin'");

        // Check if default admin user exists
        var userExists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Users WHERE Email = 'admin@example.com'");

        if (userExists == 0)
        {
            var password = _bcrypt.HashPassword("admin123");

            await connection.ExecuteAsync(@"
                INSERT INTO Users (Fullname, Email, Password, RoleId, CreatedAt)
                VALUES (@Fullname, @Email, @Password, @RoleId, GETDATE())
            ", new
            {
                Fullname = "Admin",
                Email = "admin@example.com",
                Password = password,
                RoleId = adminRoleId
            });
        }
    }
}