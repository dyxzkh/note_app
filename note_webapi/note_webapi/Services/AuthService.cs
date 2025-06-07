using note_webapi.DTOs;
using note_webapi.Interfaces;

namespace note_webapi.Services;

public class AuthService(IUserRepository repo, IConfiguration config,IBcryptService bcrypt) : IAuthService
{
    private readonly IBcryptService _bcrypt = bcrypt;
    public async Task<TokenResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await repo.GetByEmailAsync(request.Email);
        if (user is null || !_bcrypt.VerifyPassword(request.Password, user.Password))
            return null;

        var accessToken = JwtHelper.GenerateAccessToken(user, config);
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var expiry = DateTime.UtcNow.AddDays(int.Parse(config["JwtSettings:RefreshTokenExpirationDays"]!));

        await repo.UpdateRefreshTokenAsync(user.Id, refreshToken, expiry);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = expiry
        };
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string token)
    {
        var user = await repo.GetByRefreshTokenAsync(token);
        if (user is null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            return null;

        var newAccessToken = JwtHelper.GenerateAccessToken(user, config);
        var newRefreshToken = JwtHelper.GenerateRefreshToken();
        var expiry = DateTime.UtcNow.AddDays(int.Parse(config["JwtSettings:RefreshTokenExpirationDays"]!));

        await repo.UpdateRefreshTokenAsync(user.Id, newRefreshToken, expiry);

        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiry = expiry
        };
    }
}
