using EmployeeMasterKadai.Data;
using EmployeeMasterKadai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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

        // GET: Schedules/Create ここにRetirementFlagがTrueのEmployeeがJoinPopleに含ませない処理を入れたい
        public IActionResult Create()
        {
            // RetirementFlagがFalseのEmployeeのみを取得して、その名前の配列を作成します
            var employeeNames = _context.Employees
                .Where(e => !e.RetirementFlag)
                .Select(e => e.Name)
                .ToArray();

            // Scheduleオブジェクトを作成し、JoinPeopleに社員名の配列を設定します
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
