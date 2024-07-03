using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class IsNullScheduleDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as Schedule;

            if (model.StartDay.HasValue)
            {
                if (model.StartDay.Value.TimeOfDay == TimeSpan.Zero && model.AllDay == false)
                {
                    return new ValidationResult("サーバーサイド：開始時刻と終了時刻を入力してください。");
                }
            }

            return ValidationResult.Success;
        }
    }
}

