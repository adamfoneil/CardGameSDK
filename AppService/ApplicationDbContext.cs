using AppService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AppService;

public enum SupportedGames
{
	Hearts = 1,
	FoxInTheForest = 2
}

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	private static IConfiguration Config => new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false)
		.AddJsonFile("appsettings.Development.json")
		.Build();

	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var connectionName = (!args?.Any() ?? true) ? "DefaultConnection" : args![0];

		var connectionString = Config.GetConnectionString(connectionName);

		Console.WriteLine($"args = {string.Join(", ", args!)}");
		Console.WriteLine($"connection name = {connectionName}");
		Console.WriteLine($"connection string = {connectionString}");

		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseSqlServer(connectionString);
		return new ApplicationDbContext(builder.Options);
	}
}