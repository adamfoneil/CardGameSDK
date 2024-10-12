using AppService;
using AppService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp;

internal delegate void ReadyStateChangedHandler(SupportedGames game, string toUser, string? topic = null, string? payload = null);
internal delegate void GameStateChangedHandler(int gameInstanceId, string toUser, string? topic = null, string? payload = null);

internal class EventRelay(
	IDbContextFactory<ApplicationDbContext> dbFactory,
	ILogger<EventRelay> logger)
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;
	private readonly ILogger<EventRelay> _logger = logger;

	public async Task GameStateChangedAsync(int gameInstanceId, string fromUser, string? topic = null, string? payload = null)
	{
		using var db = _dbFactory.CreateDbContext();

		var players = await db.ActivePlayers
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

		db.Events.AddRange(messages);
		await db.SaveChangesAsync();
	}

	public async Task GameStartedAsync(SupportedGames game, string fromUser, string url) =>
		await ReadyStateChangedAsync(game, fromUser, EventMessage.GameStarted, url);

	public async Task ReadyStateChangedAsync(SupportedGames game, string fromUser, string? topic = null, string? payload = null)
	{
		_logger.LogDebug("ReadyStateChanged invoked: {game} {fromUser}, topic = {topic}, payload = {payload}",
			game, fromUser, topic, payload);

		using var db = _dbFactory.CreateDbContext();

		var players = await db.ReadyPlayers
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

		db.Events.AddRange(messages);
		await db.SaveChangesAsync();
	}

	/// <summary>
	/// player added or removed from ready state
	/// </summary>
	public event ReadyStateChangedHandler? ReadyStateChanged;
	/// <summary>
	/// someone made a valid play in an active game
	/// </summary>
	public event GameStateChangedHandler? GameStateChanged;

	/// <summary>
	/// called by background service when event message is dequeued
	/// </summary>
	public void NotifyReadyStateChanged(SupportedGames game, string toUser, string? topic = null, string? payload = null)
	{		
		ReadyStateChanged?.Invoke(game, toUser, topic, payload);
	}

	/// <summary>
	/// called by background service when event message is dequeued
	/// </summary>
	public void NotifyGameStateChanged(int gameInstanceId, string toUser, string? topic = null, string? payload = null)
	{		
		GameStateChanged?.Invoke(gameInstanceId, toUser, topic, payload);
	}		
}
