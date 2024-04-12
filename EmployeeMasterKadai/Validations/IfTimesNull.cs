using System.ComponentModel.DataAnnotations;
using EmployeeMasterKadai.Models;

namespace EmployeeMasterKadai.Validations
{
    public class IfTimesNull : ValidationAttribute
    {　
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (Employee)validationContext.ObjectInstance;

            if(model.RetirementFlag && model.RetirementDay == null)
            {
                return new ValidationResult("退職日を入力してください。");
            }

            return ValidationResult.Success;
        }
    }
}
