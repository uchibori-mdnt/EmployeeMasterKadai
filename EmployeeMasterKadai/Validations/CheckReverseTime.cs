using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class CheckReverseTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var schedule = validationContext.ObjectInstance as Schedule;

            if (schedule != null && (schedule.StartDay > schedule.EndDay) && schedule.AllDay == false || schedule.AllDay == true && (schedule.StartDay > schedule.EndDay))
            {

                return new ValidationResult("サーバーサイド：開始時刻が終了時刻を超えることはできません");
            }

            return ValidationResult.Success;
        }
    }
}