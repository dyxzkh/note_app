using note_webapi.DTOs;
using note_webapi.Models;

namespace note_webapi.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllAsync();
    Task<Note?> GetByIdAsync(int id);
    Task<IEnumerable<Note>> GetByUserIdAsync(int userId);
    Task<RepositoryResult<int>> CreateAsync(CreateNoteDto dto);
    Task<RepositoryResult<bool>> UpdateAsync(int noteId, UpdateNoteDto dto);
    Task<bool> DeleteAsync(int id);
}