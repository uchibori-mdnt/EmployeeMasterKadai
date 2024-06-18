using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class DateInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime inputValue)
            {
                if (inputValue > DateTime.Today)
                {
                    return new ValidationResult("退職日は本日以前の日付を入力してください。");
                }
            }
            return ValidationResult.Success;
        }

    }
}
