namespace note_webapi.Models;

public class Note : BaseClass
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public int CreatedBy { get; set; }
}