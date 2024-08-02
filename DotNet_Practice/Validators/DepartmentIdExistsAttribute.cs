using DotNet_Practice.Repository;
using System.ComponentModel.DataAnnotations;

namespace DotNet_Practice.Validators
{
    public class DepartmentIdExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Department ID is required.");
            }

            var context = (SchoolContext)validationContext.GetService(typeof(SchoolContext));
            var departmentId = (Guid)value;

            if (!context.Department.Any(d => d.Id == departmentId))
            {
                return new ValidationResult($"Department ID {departmentId} does not exist.");
            }

            return ValidationResult.Success;
        }
    }
}
