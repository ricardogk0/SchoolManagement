namespace SchoolManagement.Domain.DTOs.Request;

public class RegistrationCreateDto
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
}