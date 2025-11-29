using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.Attributes
{
    public class RequiredIfMaleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the Gender property from the model
            var genderProperty = validationContext.ObjectType.GetProperty("Gender");

            if (genderProperty == null)
            {
                return new ValidationResult("Gender property not found");
            }

            var genderValue = genderProperty.GetValue(validationContext.ObjectInstance);

            // If Gender is not Male, validation passes automatically
            if (genderValue is Gender gender && gender != Gender.Male)
            {
                return ValidationResult.Success;
            }

            // If Gender is Male, check if MilitaryStatus has a valid value
            if (value == null || (value is MilitaryStatus status && (int)status == 0))
            {
                return new ValidationResult(ErrorMessage ?? "Military Status is required for male applicants");
            }

            return ValidationResult.Success;
        }
    }
}
