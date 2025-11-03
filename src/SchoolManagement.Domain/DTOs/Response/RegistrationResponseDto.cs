namespace SchoolManagement.Domain.DTOs.Response;

public class RegistrationResponseDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; }
}