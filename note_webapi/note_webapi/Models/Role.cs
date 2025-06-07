namespace note_webapi.Models;

public class Role : BaseClass
{
    public required string RoleName { get; init; }
    public required string RoleDescription { get; init; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
}