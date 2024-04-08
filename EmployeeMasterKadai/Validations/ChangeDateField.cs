using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class ChangeDateField : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var schedule = validationContext.ObjectInstance as Schedule;

            if (schedule != null && (schedule.StartDay == null || schedule.EndDay == null) && schedule.AllDay == false)
            {
                return new ValidationResult("開始時刻と終了時刻を入力してください。");
            }

            return ValidationResult.Success;
        }
    }
}
