using System;
using System.Linq;
using System.Threading.Tasks;
using Cursor_Demo.Data;
using Cursor_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cursor_Demo.Controllers
{
	[Authorize]
	public class EmployeesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public EmployeesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var employees = await _context.Employees.Include(e => e.Department).ToListAsync();
			return View(employees);
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Employee employee)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Departments = await _context.Departments.ToListAsync();
				return View(employee);
			}
			_context.Add(employee);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee == null) return NotFound();
			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View(employee);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, Employee employee)
		{
			if (id != employee.Id) return BadRequest();
			if (!ModelState.IsValid)
			{
				ViewBag.Departments = await _context.Departments.ToListAsync();
				return View(employee);
			}
			_context.Update(employee);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(Guid id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee == null) return NotFound();
			return View(employee);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}


