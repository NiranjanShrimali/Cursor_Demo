using System;
using System.Threading.Tasks;
using Cursor_Demo.Data;
using Microsoft.AspNetCore.Http;

namespace Cursor_Demo.Middleware
{
	public class TenantResolutionMiddleware
	{
		private readonly RequestDelegate _next;

		public TenantResolutionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
		{
			// Simple strategy for demo: read tenant from header or query, fallback to single-tenant (null allowed)
			Guid tenantId;
			var tenantHeader = context.Request.Headers["X-Tenant-Id"].ToString();
			var tenantQuery = context.Request.Query["tenant"].ToString();

			if (Guid.TryParse(tenantHeader, out tenantId) || Guid.TryParse(tenantQuery, out tenantId))
			{
				context.Items[TenantConstants.HttpContextTenantKey] = tenantId;
				// dbContext.CurrentTenantId will be set lazily when first accessed
			}

			await _next(context);
		}
	}

	public static class TenantResolutionMiddlewareExtensions
	{
		public static IApplicationBuilder UseTenantResolution(this IApplicationBuilder app)
		{
			return app.UseMiddleware<TenantResolutionMiddleware>();
		}
	}
}


