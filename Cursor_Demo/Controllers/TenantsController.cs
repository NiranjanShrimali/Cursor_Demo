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
	[Authorize(Roles = "Admin")]
	public class TenantsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TenantsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var tenants = await _context.Tenants.ToListAsync();
			return View(tenants);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Tenant tenant)
		{
			if (!ModelState.IsValid) return View(tenant);
			_context.Add(tenant);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			var tenant = await _context.Tenants.FindAsync(id);
			if (tenant == null) return NotFound();
			return View(tenant);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, Tenant tenant)
		{
			if (id != tenant.Id) return BadRequest();
			if (!ModelState.IsValid) return View(tenant);
			_context.Update(tenant);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(Guid id)
		{
			var tenant = await _context.Tenants.FindAsync(id);
			if (tenant == null) return NotFound();
			return View(tenant);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var tenant = await _context.Tenants.FindAsync(id);
			if (tenant != null)
			{
				_context.Tenants.Remove(tenant);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}


