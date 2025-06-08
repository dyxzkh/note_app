namespace note_webapi.DTOs
{
    public class CreateUserDto
    {
        public required string Fullname { get; init; }
        public required string Email { get; init; }
        public required string Password { get; set; }
    }
}
