using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class SameDayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is Schedule model && model.StartDay != null && model.EndDay != null && model.StartDay == model.EndDay)
            {
                return new ValidationResult("開始時刻と終了時刻が同じになっています。");
            }

            return ValidationResult.Success;
        }
    }
}
