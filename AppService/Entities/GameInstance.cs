using AppService.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

public class GameInstance : BaseTable
{
	public SupportedGames Game { get; set; }
	public bool IsTestMode { get; set; }
	public int Round { get; set; }
	public string Url { get; set; } = default!;
	public string? State { get; set; }
	public string? Score { get; set; }
	public string? Winner { get; set; }
	public DateTime? FinishedAtUtc { get; set; }
	public bool IsFinished => FinishedAtUtc.HasValue;

	public ICollection<GameInstancePlayer> Players { get; set; } = [];

	public int CurrentRound => Round + 1;
}

public class GameInstanceConfiguration : IEntityTypeConfiguration<GameInstance>
{
	public void Configure(EntityTypeBuilder<GameInstance> builder)
	{
		builder.Property(gi => gi.Url).HasMaxLength(100).IsRequired();		
		builder.Property(gi => gi.Winner).HasMaxLength(50);
	}
}