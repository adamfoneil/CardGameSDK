using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Extensions;

internal static class MigrateDb
{
	internal static void MigrateDatabase<T>(this IServiceCollection services) where T : DbContext
	{
		using var serviceProvider = services.BuildServiceProvider();
		using var scope = serviceProvider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<T>();
		context.Database.Migrate();
	}
}
