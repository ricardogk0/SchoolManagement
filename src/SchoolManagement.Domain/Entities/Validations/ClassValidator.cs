using FluentValidation;

namespace SchoolManagement.Domain.Entities.Validations;

public class ClassValidator : AbstractValidator<ClassEntity>
{
    public void ConfigureRulesForCreate()
    {
        RuleFor(c => c.ClassName)
            .NotEmpty().WithMessage("Class name cannot be empty.")
            .MaximumLength(100).WithMessage("Class name cannot exceed 100 characters.")
            .MinimumLength(3).WithMessage("Class name must be at least 3 characters long.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");
    }

    public void ConfigureRulesForUpdate(ClassEntity existingClass)
    {
        RuleFor(c => c.ClassName)
            .NotEmpty().WithMessage("Class name cannot be empty.")
            .MaximumLength(100).WithMessage("Class name cannot exceed 100 characters.")
            .MinimumLength(3).WithMessage("Class name must be at least 3 characters long.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(250).WithMessage("Description cannot exceed 255 characters.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");
    }
}
