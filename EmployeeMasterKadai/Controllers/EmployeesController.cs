﻿using EmployeeMasterKadai.Data;
using EmployeeMasterKadai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeMasterKadaiContext _context;

        public EmployeesController(EmployeeMasterKadaiContext context)
        {
            _context = context;
        }

        // GET: EmployeeLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: EmployeeLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.Employees
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
            return PartialView();
        }

        // POST: EmployeeLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Department,RetirementFlag,RetirementDay,CreatedAt,UpdatedAt")] Employee employeeList)
        {
            if (ModelState.IsValid)
            {
                employeeList.CreatedAt = DateTime.Now;
                employeeList.UpdatedAt = DateTime.Now;
                employeeList.Id = Guid.NewGuid();
                _context.Add(employeeList);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                return PartialView(employeeList);
            }
        }

        [HttpPost]
        public IActionResult ErrorCheck([Bind("Name,Department,RetirementFlag,RetirementDay,CreatedAt,UpdatedAt")] Employee employeeList)
        {
            if (employeeList.Name == null)
            {
                return Json(new { warning = true, message = "社員名を入力してください。" });
            }
            if (employeeList.Department == null)
            {
                return Json(new { warning = true, message = "部署を入力してください。" });
            }
            if (employeeList.RetirementDay != null)
            {
                if (employeeList.RetirementDay > DateTime.Today)
                {
                    return Json(new { warning = true, message = "退職日は本日以前の日付を入力してください。" });
                }
            }
            if (employeeList.RetirementFlag && employeeList.RetirementDay == null)
            {
                return Json(new { warning = true, message = "退職日を入力してください。" });
            }

            return Json(new { warning = false });
        }

        // GET: EmployeeLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.Employees.FindAsync(id);
            if (employeeList == null)
            {
                return NotFound();
            }
            return PartialView(employeeList);
        }

        // POST: EmployeeLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Department,RetirementFlag,RetirementDay,CreatedAt,UpdatedAt")] Employee employeeList)
        {

            var existingRecord = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

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
                return Json(new { success = true });
            }
            return PartialView(employeeList);
        }

        // GET: EmployeeLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeList = await _context.Employees
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
            var employeeList = await _context.Employees.FindAsync(id);
            if (employeeList != null)
            {
                _context.Employees.Remove(employeeList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeListExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}