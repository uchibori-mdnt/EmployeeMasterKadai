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
            // 社員名をあいうえお順にソートしたリストを ViewBag に追加
            var employeeList = await _context.Employees.OrderBy(e => e.Name).ToListAsync();
            ViewBag.EmployeeContext = employeeList;

            // スケジュールを取得し、クライアント側でOrganizerの名前であいうえお順にソートする
            var schedules = await _context.Schedules.OrderBy(s => s.StartDay).ToListAsync();

            // クライアント側でOrganizerの名前であいうえお順にソートする
            var sortedSchedules = schedules.OrderBy(s =>
                employeeList.FirstOrDefault(e => e.Id == Guid.Parse(s.Organizer))?.Name ?? "")
                .ToList();

            return View(sortedSchedules);
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
            // 有効な従業員のIDと名前を取得
            var employees = _context.Employees
                .Where(e => !e.RetirementFlag)
                .OrderBy(e => e.Name)
                .Select(e => new { e.Id, e.Name })
                .ToList();

            // ViewBagに従業員のIDと名前を追加
            ViewBag.People = employees;

            var model = new Schedule();
            return PartialView(model);
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule, string[] selectedEmployeeIds)
        {
            if (ModelState.IsValid)
            {
                var selectedEmployeeGuids = selectedEmployeeIds.Select(id => Guid.Parse(id)).ToArray();
                schedule.JoinPeople = selectedEmployeeGuids;


                schedule.CreateDate = DateTime.Now;
                schedule.UpdatedDate = DateTime.Now;
                schedule.Id = Guid.NewGuid();
                _context.Add(schedule);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                // バリデーションエラー時にも従業員のIDと名前をViewBagに追加して返す
                var employees = _context.Employees
                    .Where(e => !e.RetirementFlag)
                    .Select(e => new { e.Id, e.Name })
                    .ToList();

                ViewBag.People = employees;
                return PartialView("Create", schedule);
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

            var employees = _context.Employees
                .Where(e => !e.RetirementFlag)
                .OrderBy(e => e.Name)
                .Select(e => new { e.Id, e.Name })
                .ToList();

            ViewBag.People = employees;

            return PartialView(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople,CreateAt")] Schedule schedule, Guid[] selectedEmployeeIds)
        {
            var foundScheduleData = await _context.Schedules.FirstOrDefaultAsync(e => e.Id == id);

            if (id != foundScheduleData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    schedule.JoinPeople = selectedEmployeeIds;

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
                ViewBag.People = _context.Employees
                   .Where(e => !e.RetirementFlag)
                   .Select(e => e.Name)
                   .ToList();
                return PartialView("Create", schedule);
            }
        }

        [HttpPost]
        public IActionResult SameSchedule([Bind("Id,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule, Guid[] selectedEmployeeIds)
        {
            if (selectedEmployeeIds != null)
            {
                foreach (var employeeId in selectedEmployeeIds)
                {
                    var pickEmployeeId = _context.Employees.FirstOrDefault(x => x.Id == employeeId);

                    foreach (var pickSchedule in _context.Schedules)
                    {
                        if (schedule.Id != pickSchedule.Id && pickSchedule.JoinPeople != null && pickSchedule.JoinPeople.Contains(pickEmployeeId.Id))
                        {
                            var startDate = pickSchedule.StartDay;
                            var endDate = pickSchedule.EndDay;

                            if (!schedule.AllDay && !pickSchedule.AllDay)
                            {
                                if ((startDate <= schedule.StartDay && schedule.StartDay <= endDate) || (startDate <= schedule.EndDay && schedule.EndDay <= endDate) ||
                                    (schedule.StartDay <= startDate && startDate <= schedule.EndDay) || (schedule.StartDay <= endDate && endDate <= schedule.EndDay) ||
                                    (startDate <= schedule.StartDay && schedule.EndDay <= endDate) || (schedule.StartDay <= startDate && endDate <= schedule.EndDay))
                                {
                                    return Json(new { warning = true, message = "スケジュールが重複している社員がいます。よろしいですか？" });
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { warning = false });
        }

        [HttpPost]
        public IActionResult ErrorCheck([Bind("Organizer,Title,TypeToDo,AllDay,StartDay,EndDay,JoinPeople")] Schedule schedule)
        {
            if (schedule.Organizer == null)
            {
                return Json(new { warning = true, message = "コントローラー：件名は必須です。" });
            }
            if (schedule.Title == null)
            {
                return Json(new { warning = true, message = "コントローラー：担当者は必須です。" });
            }
            if (schedule.StartDay != null && schedule.EndDay != null && schedule.StartDay == schedule.EndDay && schedule.AllDay == false && schedule.StartDay.Value.TimeOfDay != TimeSpan.Zero)
            {
                return Json(new { warning = true, message = "コントローラー：開始時刻と終了時刻が同じになっています。" });
            }
            if (schedule != null && (schedule.StartDay.Value.TimeOfDay == TimeSpan.Zero || schedule.EndDay.Value.TimeOfDay == TimeSpan.Zero) && schedule.AllDay == false)
            {
                return Json(new { warning = true, message = "コントローラー：開始時刻と終了時刻を入力してください。" });
            }
            if (schedule != null && (schedule.StartDay > schedule.EndDay))
            {
                return Json(new { warning = true, message = "コントローラー：開始時刻が終了時刻を超えることはできません。" });
            }

            return Json(new { warning = false });
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
            ViewBag.EmployeeContext = await _context.Employees.ToListAsync();
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
