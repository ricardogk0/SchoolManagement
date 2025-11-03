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

        RuleFor(s => s.DocumentNumber)
            .NotEmpty().WithMessage("Document number cannot be empty.")
            .Must(IsValidCpf).WithMessage("Invalid CPF.")
            .MustAsync(async (documentNumber, cancellation) =>
            {
                var existingStudent = await _unitOfWork.Students.Find(b => b.DocumentNumber == documentNumber);
                return !existingStudent.Any();
            }).WithMessage("A Student with this document already exists.")
            .MaximumLength(20).WithMessage("Document number cannot exceed 20 characters.");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(async (email, cancellation) =>
            {
                var existingStudent = await _unitOfWork.Students.Find(b => b.Email == email);
                return !existingStudent.Any();
            }).WithMessage("A Student with this email already exists.");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage("Password must include upper/lowercase, numbers and special characters.");
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

        RuleFor(s => s.DocumentNumber)
            .NotEmpty().WithMessage("Document number cannot be empty.")
            .Must(IsValidCpf).WithMessage("Invalid CPF.")
            .MustAsync(async (documentNumber, cancellation) =>
            {
                var existingStudents = await _unitOfWork.Students.Find(b => b.DocumentNumber == documentNumber && b.Id != existingStudent.Id);
                return !existingStudents.Any();
            }).WithMessage("A Student with this document already exists.")
            .MaximumLength(20).WithMessage("Document number cannot exceed 20 characters.");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(async (email, cancellation) =>
            {
                var existingStudent = await _unitOfWork.Students.Find(b => b.Email == email);
                return !existingStudent.Any();
            }).WithMessage("A Student with this email already exists.");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage("Password must include upper/lowercase, numbers and special characters.");
    }

    private static bool IsValidCpf(string? documentNumber)
    {
        if (string.IsNullOrWhiteSpace(documentNumber)) return false;

        var digitsOnly = new string(documentNumber.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length != 11) return false;

        if (digitsOnly.Distinct().Count() == 1) return false;

        int CalculateCheckDigit(ReadOnlySpan<int> digits, ReadOnlySpan<int> weights)
        {
            var sum = 0;
            for (var i = 0; i < weights.Length; i++)
            {
                sum += digits[i] * weights[i];
            }
            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        var nums = digitsOnly.Select(c => c - '0').ToArray();

        var firstWeights = Enumerable.Range(2, 9).Reverse().ToArray();
        var firstCheck = CalculateCheckDigit(nums.AsSpan(0, 9), firstWeights);
        if (nums[9] != firstCheck) return false;

        var secondWeights = Enumerable.Range(2, 10).Reverse().ToArray();
        var secondCheck = CalculateCheckDigit(nums.AsSpan(0, 10), secondWeights);
        if (nums[10] != secondCheck) return false;

        return true;
    }
}
