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
	public class DepartmentsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DepartmentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var departments = await _context.Departments.Include(d => d.ParentDepartment).ToListAsync();
			return View(departments);
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Department department)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Departments = await _context.Departments.ToListAsync();
				return View(department);
			}
			_context.Add(department);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null) return NotFound();
			ViewBag.Departments = await _context.Departments.Where(d => d.Id != id).ToListAsync();
			return View(department);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, Department department)
		{
			if (id != department.Id) return BadRequest();
			if (!ModelState.IsValid)
			{
				ViewBag.Departments = await _context.Departments.Where(d => d.Id != id).ToListAsync();
				return View(department);
			}
			_context.Update(department);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(Guid id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null) return NotFound();
			return View(department);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department != null)
			{
				_context.Departments.Remove(department);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}


