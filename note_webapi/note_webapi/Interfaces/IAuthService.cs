using note_webapi.DTOs;
using note_webapi.Models;

namespace note_webapi.Interfaces;

public interface IAuthService
{
    Task<TokenResponse?> AuthenticateAsync(LoginRequest request);
    Task<TokenResponse?> RefreshTokenAsync(string token);
}