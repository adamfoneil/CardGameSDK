using AppService.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

/// <summary>
/// signals that a player is ready to play a game. When there are enough players
/// for a game, the web UI enables its "start game" button.
/// </summary>
public class ReadyPlayer : BaseTable
{
	public SupportedGames Game { get; set; }
	public int UserId { get; set; }

	public ApplicationUser? User { get; set; }
}

public class ReadyPlayerConfiguration : IEntityTypeConfiguration<ReadyPlayer>
{
	public void Configure(EntityTypeBuilder<ReadyPlayer> builder)
	{
		builder.HasAlternateKey(nameof(ReadyPlayer.Game), nameof(ReadyPlayer.UserId));

		builder
			.HasOne(rp => rp.User)
			.WithMany(u => u.ReadyPlayers)
			.HasForeignKey(rp => rp.UserId)
			.HasPrincipalKey(u => u.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}