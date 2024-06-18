using EmployeeMasterKadai.Data;
using EmployeeMasterKadai.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations
{
    public class SameSchedule : ValidationAttribute
    {
        private readonly EmployeeMasterKadaiContext _context;

        public SameSchedule(EmployeeMasterKadaiContext context)
        {
            _context = context;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var schedule = validationContext.ObjectInstance as Schedule;

            if (schedule != null && schedule.JoinPeople != null)
            {
                var employees = _context.Employees;

                foreach (var joinPeopleId in schedule.JoinPeople)
                {
                    var employee = employees.FirstOrDefault(x => x.Id == joinPeopleId);

                    if (employee != null)
                    {
                        foreach (var pickSchedule in _context.Schedules)
                        {
                            if (schedule.Id != pickSchedule.Id && pickSchedule.JoinPeople != null && pickSchedule.JoinPeople.Contains(joinPeopleId))
                            {
                                var startDate = pickSchedule.StartDay;
                                var endDate = pickSchedule.EndDay;

                                if (!schedule.AllDay && !pickSchedule.AllDay)
                                {
                                    if ((startDate <= schedule.StartDay && schedule.StartDay <= endDate) ||
                                        (startDate <= schedule.EndDay && schedule.EndDay <= endDate) ||
                                        (schedule.StartDay <= startDate && startDate <= schedule.EndDay) ||
                                        (schedule.StartDay <= endDate && endDate <= schedule.EndDay) ||
                                        (startDate <= schedule.StartDay && schedule.EndDay <= endDate) ||
                                        (schedule.StartDay <= startDate && endDate <= schedule.EndDay))
                                    {
                                        return new ValidationResult("スケジュールが重複している社員がいます。よろしいですか？");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
