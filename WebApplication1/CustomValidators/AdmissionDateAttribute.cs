using System.ComponentModel.DataAnnotations;

namespace WebApplication1.CustomValidators
{
    public class AdmissionDateAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? date = (DateTime?)value;
            if(date < DateTime.Today)
            {
                return new ValidationResult("Admission date should be greater than or equal to todays date");
            }
            return ValidationResult.Success;
        }
    }
}
