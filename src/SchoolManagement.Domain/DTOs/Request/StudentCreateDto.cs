namespace SchoolManagement.Domain.DTOs.Request;

public class StudentCreateDto
{
    public string Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
