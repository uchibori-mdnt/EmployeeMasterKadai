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
            return View(await _context.Schedule.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {

            var employeeNames = _context.EmployeeList.Select(e => e.Name).ToArray();
            var model = new Schedule
            {
                // JoinPeople プロパティに社員名のリストを設定する
                JoinPeople = employeeNames
            };

            return View(model);
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
                return RedirectToAction(nameof(Index));
            }


            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var employeeNames = _context.EmployeeList.Select(e => e.Name).ToArray();
            schedule.JoinPeople = employeeNames;

            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople,CreateAt")] Schedule schedule)
        {

            var foundScheduleData = await _context.Schedule.FirstOrDefaultAsync(e => e.Id == id);

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

                    // 変更を保存
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
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
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
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(Guid id)
        {
            return _context.Schedule.Any(e => e.Id == id);
        }
    }
}
