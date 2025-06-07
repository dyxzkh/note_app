namespace note_webapi.DTOs;

public class CreateNoteDto
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
}