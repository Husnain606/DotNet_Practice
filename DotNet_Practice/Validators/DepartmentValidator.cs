using DotNet_Practice.DTOs.RequestDTO;
using FluentValidation;

namespace DotNet_Practice.Validators
{
    public class DepartmentValidator : AbstractValidator<CreateDepartmentDTO>
    {
        public DepartmentValidator()
        {
            RuleFor(s => s.Id)
                 .NotEmpty().WithMessage("Department ID is required");
            RuleFor(s => s.DepartmentName)
                .NotEmpty().MaximumLength(50)
                .WithMessage("Department Name is required");
            RuleFor(s => s.DepartmenrDescription)
                .MaximumLength(200).WithMessage("Department Description must be less than 200 characters");
        }
    }
}
