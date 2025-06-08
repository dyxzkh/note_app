using note_webapi.DTOs;
using note_webapi.Models;

namespace note_webapi.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<GetUser?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<RepositoryResult<int>> CreateAsync(User user);
    Task<RepositoryResult<int>> CreateUserRoleAsync(CreateUserDto user);
    Task<RepositoryResult<bool>> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
    Task<User?> GetByRefreshTokenAsync(string token);
}