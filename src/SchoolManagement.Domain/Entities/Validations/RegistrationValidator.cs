using FluentValidation;
using SchoolManagement.Domain.Interfaces.UoW;

namespace SchoolManagement.Domain.Entities.Validations;

public class RegistrationValidator : AbstractValidator<RegistrationEntity>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegistrationValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void ConfigureRulesForCreate()
    {
        RuleFor(r => r.Student)
            .NotEmpty().WithMessage("Student ID cannot be empty.")
            .MustAsync(async (registration, studentId, cancellation) =>
            {
                return !ExistsStudentInClass(studentId.Id, registration.Class.Id);
            }).WithMessage("A student is already registered in this class.");
        RuleFor(r => r.Class)
            .NotEmpty().WithMessage("Class ID cannot be empty.");
    }

    private bool ExistsStudentInClass(Guid studentId, Guid classId)
    {
        var registrations = _unitOfWork.Registrations.Find(r => r.Student.Id == studentId && r.Class.Id == classId && !r.IsDeleted).Result;
        return registrations.Any();
    }
}