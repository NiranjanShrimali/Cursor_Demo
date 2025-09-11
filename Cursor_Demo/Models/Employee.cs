using System;
using System.ComponentModel.DataAnnotations;

namespace Cursor_Demo.Models
{
	public class Employee
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(150)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string LastName { get; set; } = string.Empty;

		[MaxLength(200)]
		public string? Email { get; set; }

		[MaxLength(50)]
		public string? EmployeeCode { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public Guid TenantId { get; set; }
		public Tenant? Tenant { get; set; }

		public Guid? DepartmentId { get; set; }
		public Department? Department { get; set; }

		public bool IsActive { get; set; } = true;
	}
}


