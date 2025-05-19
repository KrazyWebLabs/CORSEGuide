namespace ShareProject.Models.DTOs.InputDTO;

public class NoteInputDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserEmail { get; set; }
    public int CategoryId { get; set; }
    public bool Priority { get; set; }
}
