﻿using AppService.Entities.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

public class GameInstance : BaseTable
{
	public SupportedGames Game { get; set; }
	public string? State { get; set; } = default!;
	public DateTime? FinishedAtUtc { get; set; }
	public bool IsFinished => FinishedAtUtc.HasValue;

	public ICollection<GameInstancePlayer> Players { get; set; } = [];
}

public class GameInstanceConfiguration : IEntityTypeConfiguration<GameInstance>
{
	public void Configure(EntityTypeBuilder<GameInstance> builder)
	{
	}
}