﻿using EmployeeMasterKadai.Data;
using EmployeeMasterKadai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMasterKadai.Controllers
{
    public class EmployeeListsController : Controller
    {
        private readonly EmployeeMasterKadaiContext _context;

        public EmployeeListsController(EmployeeMasterKadaiContext context)
        {
            _context = context;
        }

        // GET: EmployeeLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmployeeList.ToListAsync());
        }

        // GET: EmployeeLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.EmployeeList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeList == null)
            {
                return NotFound();
            }

            return View(employeeList);
        }

        // GET: EmployeeLists/Create
        public IActionResult Create()
        {
            //※※※ajaxなどの部分ビューを返すときは"PartialView"で
            return PartialView();
        }

        // POST: EmployeeLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Department,RetirementFlag,RetirementDay,CreatedAt,UpdatedAt")] EmployeeList employeeList)
        {
            if (ModelState.IsValid)
            {
                employeeList.CreatedAt = DateTime.Now;
                employeeList.UpdatedAt = DateTime.Now;
                employeeList.Id = Guid.NewGuid();
                _context.Add(employeeList);
                await _context.SaveChangesAsync();

                //※※※こっちもJson
                //return RedirectToAction(nameof(Index));
                return Json(new { success =true });
            }
            //※※※ajaxなどの部分ビューを返すときは"PartialView"で
            return PartialView(employeeList);
        }

        // GET: EmployeeLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.EmployeeList.FindAsync(id);
            if (employeeList == null)
            {
                return NotFound();
            }
            return View(employeeList);
        }

        // POST: EmployeeLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Department,RetirementFlag,RetirementDay,CreatedAt")] EmployeeList employeeList)
        {

            var existingRecord = await _context.EmployeeList.FirstOrDefaultAsync(e => e.Id == id);

            if (id != existingRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingRecord.Name = employeeList.Name;
                    existingRecord.Department = employeeList.Department;
                    existingRecord.RetirementFlag = employeeList.RetirementFlag;
                    existingRecord.RetirementDay = employeeList.RetirementDay;
                    existingRecord.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeListExists(employeeList.Id))
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
            return View(employeeList);
        }

        // GET: EmployeeLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.EmployeeList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeList == null)
            {
                return NotFound();
            }

            return View(employeeList);
        }

        // POST: EmployeeLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employeeList = await _context.EmployeeList.FindAsync(id);
            if (employeeList != null)
            {
                _context.EmployeeList.Remove(employeeList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeListExists(Guid id)
        {
            return _context.EmployeeList.Any(e => e.Id == id);
        }
    }
}
