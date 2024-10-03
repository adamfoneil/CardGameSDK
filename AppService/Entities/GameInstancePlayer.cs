using AppService.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

public class GameInstancePlayer : BaseTable
{
	public int GameInstanceId { get; set; }
	public int UserId { get; set; } = default!;

	public GameInstance? Game { get; set; }
	public ApplicationUser? User { get; set; }
}

public class PlayerConfiguration : IEntityTypeConfiguration<GameInstancePlayer>
{
	public void Configure(EntityTypeBuilder<GameInstancePlayer> builder)
	{		
		builder.HasAlternateKey(nameof(GameInstancePlayer.GameInstanceId), nameof(GameInstancePlayer.UserId));

		builder
			.HasOne(gip => gip.User)
			.WithMany(u => u.GameInstances)
			.HasForeignKey(gip => gip.UserId)
			.HasPrincipalKey(u => u.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
