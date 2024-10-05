using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
	public int UserId { get; set; }

	public ICollection<GameInstancePlayer> GameInstances { get; set; } = [];
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(p => p.UserId).ValueGeneratedOnAdd();
		builder.HasAlternateKey(p => p.UserId);
	}
}
