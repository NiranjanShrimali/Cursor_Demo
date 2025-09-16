using System;
using System.Linq;
using System.Linq.Expressions;
using Cursor_Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cursor_Demo.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public Guid? CurrentTenantId { get; private set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public DbSet<Tenant> Tenants => Set<Tenant>();
		public DbSet<Department> Departments => Set<Department>();
		public DbSet<Employee> Employees => Set<Employee>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Department>()
				.HasOne(d => d.ParentDepartment)
				.WithMany(p => p.ChildDepartments)
				.HasForeignKey(d => d.ParentDepartmentId)
				.OnDelete(DeleteBehavior.Restrict);

			// Global query filters for multi-tenancy
			builder.Entity<Department>().HasQueryFilter(CreateTenantFilter<Department>());
			builder.Entity<Employee>().HasQueryFilter(CreateTenantFilter<Employee>());
		}

		public override int SaveChanges()
		{
			ApplyTenantIds();
			return base.SaveChanges();
		}

		public override async System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			ApplyTenantIds();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyTenantIds()
		{
			var tenantId = GetCurrentTenantId();
			if (tenantId == null) return;

			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.State == EntityState.Added)
				{
					if (entry.Entity is Department dept)
					{
						dept.TenantId = tenantId.Value;
					}
					else if (entry.Entity is Employee emp)
					{
						emp.TenantId = tenantId.Value;
					}
				}
			}
		}

		private Expression<Func<TEntity, bool>> CreateTenantFilter<TEntity>() where TEntity : class
		{
			var tenantId = GetCurrentTenantId();
			if (typeof(TEntity) == typeof(Department))
			{
				return (Expression<Func<TEntity, bool>>)(object)((Department e) => !GetCurrentTenantId().HasValue || e.TenantId == GetCurrentTenantId());
			}
			if (typeof(TEntity) == typeof(Employee))
			{
				return (Expression<Func<TEntity, bool>>)(object)((Employee e) => !GetCurrentTenantId().HasValue || e.TenantId == GetCurrentTenantId());
			}
			return entity => true;
		}

		private Guid? GetCurrentTenantId()
		{
			if (CurrentTenantId.HasValue) return CurrentTenantId;
			var context = _httpContextAccessor.HttpContext;
			if (context != null && context.Items.TryGetValue(TenantConstants.HttpContextTenantKey, out var value) && value is Guid id)
			{
				CurrentTenantId = id;
				return id;
			}
			return null;
		}
	}

	public static class TenantConstants
	{
		public const string HttpContextTenantKey = "__tenant_id";
	}
}


