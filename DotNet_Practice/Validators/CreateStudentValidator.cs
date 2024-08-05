﻿using DotNet_Practice.DTOs.NewFolder;
using FluentValidation;

namespace DotNet_Practice.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDTO>
    {
        public CreateStudentValidator()
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
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(s => s.ConfirmPasword)
                .NotEmpty()
                .Equal(s => s.Pasword).WithMessage("Passwords do not match");

            RuleFor(s => s.DepartmentId)
                .NotEmpty();
        }
    }
}
