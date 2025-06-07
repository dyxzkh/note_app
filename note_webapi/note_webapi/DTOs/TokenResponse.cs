namespace note_webapi.DTOs;

public class TokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiry { get; init; }
}