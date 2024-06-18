using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class IsNullTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (Employee)validationContext.ObjectInstance;

            if (model.RetirementFlag && model.RetirementDay == null)
            {
                return new ValidationResult("退職日を入力してください。");
            }

            return ValidationResult.Success;
        }
    }
}
