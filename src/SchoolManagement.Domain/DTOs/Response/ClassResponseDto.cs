namespace SchoolManagement.Domain.Dtos.Response;

public class ClassResponseDto
{
    public Guid Id { get; set; }
    public string ClassName { get; set; }
    public string Description { get; set; }
    public int StudentCount { get; set; }
}