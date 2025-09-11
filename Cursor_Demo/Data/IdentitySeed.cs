using System.Threading.Tasks;
using Cursor_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cursor_Demo.Data
{
	public static class IdentitySeed
	{
		public static async Task SeedAsync(IServiceProvider services)
		{
			using var scope = services.CreateScope();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			string[] roles = new[] { "Admin", "HR", "Manager", "Employee" };
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			var adminEmail = "admin@local.test";
			var admin = await userManager.FindByEmailAsync(adminEmail);
			if (admin == null)
			{
				admin = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true
				};
				await userManager.CreateAsync(admin, "Admin#12345");
			}

			if (!await userManager.IsInRoleAsync(admin, "Admin"))
			{
				await userManager.AddToRoleAsync(admin, "Admin");
			}
		}
	}
}


