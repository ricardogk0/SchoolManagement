using FluentValidation;
using SchoolManagement.Domain.Interfaces.UoW;

namespace SchoolManagement.Domain.Entities.Validations;

public class StudentValidator : AbstractValidator<StudentEntity>
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void ConfigureRulesForCreate()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Student name cannot be empty.")
            .MaximumLength(100).WithMessage("Student name cannot exceed 100 characters.")
            .MinimumLength(3).WithMessage("Student name must be at least 3 characters long.");

        RuleFor(s => s.BirthDate)
            .NotEmpty().WithMessage("Birth date cannot be empty.")
            .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Birth date must be in the past.");

        //TODO Validar se é um CPF valido
        RuleFor(s => s.DocumentNumber)
            .NotEmpty().WithMessage("Document number cannot be empty.")
            .MustAsync(async (documentNumber, cancellation) =>
            {
                var existingStudent = await _unitOfWork.Students.Find(b => b.DocumentNumber == documentNumber);
                return !existingStudent.Any();
            }).WithMessage("A Student with this document already exists.")
            .MaximumLength(20).WithMessage("Document number cannot exceed 20 characters.");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }

    public void ConfigureRulesForUpdate(StudentEntity existingStudent)
    {
        RuleFor(s => s.Name)
             .NotEmpty().WithMessage("Student name cannot be empty.")
             .MaximumLength(100).WithMessage("Student name cannot exceed 100 characters.")
             .MinimumLength(3).WithMessage("Student name must be at least 3 characters long.");

        RuleFor(s => s.BirthDate)
            .NotEmpty().WithMessage("Birth date cannot be empty.")
            .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Birth date must be in the past.");

        //TODO Validar se é um CPF valido
        RuleFor(s => s.DocumentNumber)
            .NotEmpty().WithMessage("Document number cannot be empty.")
            .MustAsync(async (documentNumber, cancellation) =>
            {
                var existingStudents = await _unitOfWork.Students.Find(b => b.DocumentNumber == documentNumber && b.Id != existingStudent.Id);
                return !existingStudents.Any();
            }).WithMessage("A Student with this document already exists.")
            .MaximumLength(20).WithMessage("Document number cannot exceed 20 characters.");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
