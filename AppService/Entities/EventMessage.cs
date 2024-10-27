﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppService.Entities;

/// <summary>
/// a message relayed to players of a game instance
/// </summary>
public class EventMessage
{
	public long Id { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	public SupportedGames? Game { get; set; }
	public int? GameInstanceId { get; set; }
	/// <summary>
	/// message is not relayed to this user because it's the event originator
	/// </summary>
	public string FromUser { get; set; } = default!;
	public string ToUser { get; set; } = default!;
	public string? Topic { get; set; }
	public string? Payload { get; set; }

	public const string GameStartedTopic = "GameStarted";
	public const string RoundEndedTopic = "RoundEnded";
	public const string GameEndedTopic = "GameEnded";
}

public class EventMessageConfiguration : IEntityTypeConfiguration<EventMessage>
{
	public void Configure(EntityTypeBuilder<EventMessage> builder)
	{
		builder.Property(e => e.FromUser)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(e => e.ToUser)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(e => e.Topic)
			.HasMaxLength(50);

		builder.HasOne<GameInstance>()
			.WithMany()
			.HasForeignKey(e => e.GameInstanceId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
