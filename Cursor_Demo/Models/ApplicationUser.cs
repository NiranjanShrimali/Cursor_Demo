using Microsoft.AspNetCore.Identity;
using System;

namespace Cursor_Demo.Models
{
	public class ApplicationUser : IdentityUser
	{
		public Guid? TenantId { get; set; }
	}
}


