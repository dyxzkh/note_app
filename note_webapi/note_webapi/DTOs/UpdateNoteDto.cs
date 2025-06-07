namespace note_webapi.DTOs;

public class UpdateNoteDto
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}