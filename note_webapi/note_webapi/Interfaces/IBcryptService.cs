namespace note_webapi.Interfaces;

public interface IBcryptService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}