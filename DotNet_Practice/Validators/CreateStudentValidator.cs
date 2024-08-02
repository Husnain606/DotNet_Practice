using DotNet_Practice.DTOs.NewFolder;
using DotNet_Practice.Repository;
using FluentValidation;

namespace DotNet_Practice.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDTO>
    {
        public CreateStudentValidator(SchoolContext context)
        {
            RuleFor(s => s.StudentFirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(30);

            RuleFor(s => s.StudentLastName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(s => s.StudentFatherName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(s => s.Age)
                .NotEmpty()
                .InclusiveBetween(5, 18);

            RuleFor(s => s.Class)
                .NotEmpty();

            RuleFor(s => s.Contact)
                .NotEmpty()
                .Length(11).WithMessage("Contact number must be 11 digits long.");

            RuleFor(s => s.Mail)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid Email");

            RuleFor(s => s.Pasword)
                .NotEmpty();

            RuleFor(s => s.ConfirmPasword)
                .NotEmpty()
                .Equal(s => s.Pasword).WithMessage("Passwords do not match");

            RuleFor(s => s.DepartmentId)
                .NotEmpty()
                .Must(departmentId => context.Department.Any(d => d.Id == departmentId))
                .WithMessage(departmentId => $"Department ID {departmentId} does not exist.");
        }
    }
}
