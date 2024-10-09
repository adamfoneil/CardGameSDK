using AppService.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

public class Round : BaseTable
{
	public int GameInstanceId { get; set; }
	public int Number { get; set; }
	public string State { get; set; } = default!;
}

public class RoundConfiguration : IEntityTypeConfiguration<Round>
{
	public void Configure(EntityTypeBuilder<Round> builder)
	{
		builder.HasAlternateKey(nameof(Round.GameInstanceId), nameof(Round.Number));
		builder.Property(r => r.State).IsRequired();
	}
}