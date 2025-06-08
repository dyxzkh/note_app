using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using note_webapi.DTOs;
using note_webapi.Interfaces;
using note_webapi.Models;

namespace note_webapi.Repositories;

public class NoteRepository(IConfiguration config, ILogger<UserRepository> logger) : INoteRepository
{

    private readonly string? _connectionString = config.GetConnectionString("DefaultConnection");
    private readonly ILogger<UserRepository> _logger = logger;
    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        using var db = Connection;
        return await db.QueryAsync<Note>($"SELECT * FROM Notes");
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<Note>(
            "SELECT * FROM Notes WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<Note>> GetByUserIdAsync(int userId)
    {
        using var db = Connection;
        return await db.QueryAsync<Note>(
            "SELECT * FROM Notes WHERE CreatedBy = @UserId ORDER BY CreatedAt DESC",
            new { UserId = userId });
    }

    public async Task<RepositoryResult<int>> CreateAsync(CreateNoteDto dto)
    {
        using var db = Connection;
        try
        {
            var sql = @"
                INSERT INTO Notes (Title, Content, CreatedAt, CreatedBy) 
                VALUES (@Title, @Content, @CreatedAt, @CreatedBy); 
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await db.ExecuteScalarAsync<int>(sql, dto);

            return new RepositoryResult<int>
            {
                Success = true,
                Data = newId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create note");
            return new RepositoryResult<int>
            {
                Success = false,
                ErrorMessage = "An error occurred while creating the note",
                Data = 0
            };
        }
    }

    public async Task<RepositoryResult<bool>> UpdateAsync(int noteId, UpdateNoteDto dto)
    {
        using var db = Connection;
        try
        {
            var sql = @"
                UPDATE Notes 
                SET 
                    Title = @Title, 
                    Content = @Content, 
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

            var rowsAffected = await db.ExecuteAsync(sql, new
            {
                Title = dto.Title,
                Content = dto.Content,
                UpdatedAt = dto.UpdatedAt,
                Id = noteId,
            });

            return new RepositoryResult<bool>
            {
                Success = rowsAffected > 0,
                Data = rowsAffected > 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update note");
            return new RepositoryResult<bool>
            {
                Success = false,
                ErrorMessage = "An error occurred while updating the note",
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
                "DELETE FROM Notes WHERE Id = @Id",
                new { Id = id }) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete note");
            return false;
        }
    }
}