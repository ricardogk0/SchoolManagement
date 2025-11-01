namespace SchoolManagement.Domain.Entities;

public class RegistrationEntity : EntityBase
{
    public StudentEntity Student { get; set; }
    public ClassEntity Class { get; set; }
}
