namespace SchoolManagement.Domain.Dtos.Response;

public class StudentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
}
