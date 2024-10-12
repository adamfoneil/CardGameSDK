using AppService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AppService;

public partial class ApplicationDbContext
{	
	public async Task GameStateChangedAsync(int gameInstanceId, string fromUser, string? topic = null, string? payload = null)
	{
		var players = await ActivePlayers
			.Include(row => row.User)
			.Where(p => p.GameInstanceId == gameInstanceId && p.User!.UserName != fromUser && !p.IsResigned)
			.Select(row => row.User!.UserName)
			.ToListAsync();

		var messages = players.Select(player => new EventMessage()
		{
			GameInstanceId = gameInstanceId,
			FromUser = fromUser,
			ToUser = player!,
			Topic = topic,
			Payload = payload
		});

		Events.AddRange(messages);
		await SaveChangesAsync();
	}

	public async Task GameStartedAsync(SupportedGames game, string fromUser, string url) =>
		await ReadyStateChangedAsync(game, fromUser, EventMessage.GameStarted, url);

	public async Task ReadyStateChangedAsync(SupportedGames game, string fromUser, string? topic = null, string? payload = null)
	{
		var players = await ReadyPlayers
			.Include(row => row.User)
			.Where(p => p.Game == game && p.User!.UserName != fromUser)
			.Select(row => row.User!.UserName)
			.ToListAsync();

		var messages = players.Select(player => new EventMessage()
		{
			Game = game,
			FromUser = fromUser,
			ToUser = player!,
			Topic = topic,
			Payload = payload
		});

		Events.AddRange(messages);
		await SaveChangesAsync();
	}
}
