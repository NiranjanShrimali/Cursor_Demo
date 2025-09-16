using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cursor_Demo.Models
{
	public class Tenant
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(200)]
		public string Name { get; set; } = string.Empty;

		[MaxLength(200)]
		public string? Domain { get; set; }

		[MaxLength(500)]
		public string? Address { get; set; }

		[MaxLength(100)]
		public string? ContactEmail { get; set; }

		public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

		public ICollection<Department> Departments { get; set; } = new List<Department>();
		public ICollection<Employee> Employees { get; set; } = new List<Employee>();
	}
}


