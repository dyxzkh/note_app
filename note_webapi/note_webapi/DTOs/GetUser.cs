namespace note_webapi.DTOs
{
    public class GetUser
    {
        public int Id { get; set; }
        public required string Fullname { get; init; }
        public required string Email { get; init; }
        public int RoleId { get; init; }
        public required string RoleName { get; set; }
    }
}
