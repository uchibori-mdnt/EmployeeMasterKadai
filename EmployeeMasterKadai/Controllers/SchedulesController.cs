using EmployeeMasterKadai.Data;
using EmployeeMasterKadai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;



namespace EmployeeMasterKadai.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly EmployeeMasterKadaiContext _context;

        public SchedulesController(EmployeeMasterKadaiContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schedules.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

       
        public IActionResult Create()
        {
            var employeeNames = _context.Employees
                .Where(e => !e.RetirementFlag)
                .Select(e => e.Name)
                .ToArray();

            var model = new Schedule
            {
                JoinPeople = employeeNames
            };

            return PartialView(model);
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                schedule.CreateDate = DateTime.Now;
                schedule.UpdatedDate = DateTime.Now;
                schedule.Id = Guid.NewGuid();
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                var employeeNames = _context.Employees.Select(e => e.Name).ToArray();
                var model = new Schedule
                {
                    JoinPeople = employeeNames
                };

                return PartialView(model);
            }
        }

        [HttpPost]
        public IActionResult SameSchedule([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            var joinPeoples = schedule.JoinPeople;
            if(joinPeoples.Any())
            {
                foreach (var joinPeople in joinPeoples)
                {
                    if(joinPeople == "")
                    {
                        continue;
                    }

                    foreach (var pickSchedule in _context.Schedules)
                    {
                        if (pickSchedule.JoinPeople != null && pickSchedule.JoinPeople.Contains(joinPeople))
                        {
                            var startDate = pickSchedule.StartDay;
                            var endDate = pickSchedule.EndDay;

                            if (!schedule.AllDay && !pickSchedule.AllDay)
                            {
                                if ((startDate <= schedule.StartDay && schedule.StartDay <= endDate) || (endDate >= schedule.EndDay && schedule.EndDay >= schedule.StartDay))
                                {
                                    return Json(new { warning = true, message = "スケジュールがかぶっています。" });
                                }
                            }
                            else
                            {
                                if ((schedule.AllDay || pickSchedule.AllDay) && schedule.StartDay == pickSchedule.StartDay && schedule.EndDay == pickSchedule.EndDay)
                                {
                                    return Json(new { warning = true, message = "スケジュールが重複している社員がいます。よろしいですか？" });
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { warning = false});
        }

        [HttpPost]
        public IActionResult SameDay([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            if (schedule.StartDay != null && schedule.EndDay != null && schedule.StartDay == schedule.EndDay && schedule.AllDay == false)
            {
                return Json(new { warning = true, message = "開始時刻と終了時刻が同じになっています。" });
            }

            return Json(new { warning = false });
        }



        [HttpPost]
        public IActionResult ChangeDate([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            if (schedule != null && (schedule.StartDay == null || schedule.EndDay == null) && schedule.AllDay == false)
            {
                return Json(new { warning = true, message = "開始時刻と終了時刻を入力してください。" });
            }

            return Json(new { warning = false });
        }

        public IActionResult OverStartTime([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            if (schedule != null && (schedule.StartDay > schedule.EndDay) && schedule.AllDay == false)
            {

                return Json(new { warning = true, message = "開始時刻が終了時刻を超えることはできません。" });
            }
            return Json(new { warning = false });
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var employeeNames = _context.Employees.Select(e => e.Name).ToArray();
            schedule.JoinPeople = employeeNames;

            return PartialView(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople,CreateAt")] Schedule schedule)
        {

            var foundScheduleData = await _context.Schedules.FirstOrDefaultAsync(e => e.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    foundScheduleData.Organizer = schedule.Organizer;
                    foundScheduleData.Title = schedule.Title;
                    foundScheduleData.TypeToDo = schedule.TypeToDo;
                    foundScheduleData.AllDay = schedule.AllDay;
                    foundScheduleData.StartDay = schedule.StartDay;
                    foundScheduleData.EndDay = schedule.EndDay;
                    foundScheduleData.JoinPeople = schedule.JoinPeople;
                    foundScheduleData.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
            }
            else
            {
                var employeeNames = _context.Employees.Select(e => e.Name).ToArray();
                var model = new Schedule
                {
                    JoinPeople = employeeNames
                };
                //return Json(new { success = true });
                return PartialView(model);

            }
            //return PartialView(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(Guid id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
