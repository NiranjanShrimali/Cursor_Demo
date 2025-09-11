using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cursor_Demo.Models
{
	public class Department
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(150)]
		public string Name { get; set; } = string.Empty;

		public Guid TenantId { get; set; }
		public Tenant? Tenant { get; set; }

		public Guid? ParentDepartmentId { get; set; }
		public Department? ParentDepartment { get; set; }

		public ICollection<Department> ChildDepartments { get; set; } = new List<Department>();
		public ICollection<Employee> Employees { get; set; } = new List<Employee>();
	}
}


