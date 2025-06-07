namespace note_webapi.Models;

public class User : BaseClass
{
    public required string Fullname { get; init; }
    public required string Email { get; init; }
    public required string Password { get; set; }
    public string? RefreshToken { get; init; }
    public DateTime? RefreshTokenExpiry { get; init; }
    
    public int RoleId { get; init; }
    
    public string? RoleName { get; set; }
}