using System.ComponentModel.DataAnnotations;

namespace DotNet_Practice.Validators
{
    public class ContactLengthValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contact = value as string;
            if (contact != null && contact.Length == 11)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Contact number must be 10 digits long.");
        }
    }
}
