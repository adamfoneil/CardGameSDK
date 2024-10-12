using AppService;
using AppService.Entities;
using BlazorApp.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorApp;

internal delegate Task ReadyStateChangedHandler(SupportedGames game, string toUser, string? topic = null, string? payload = null);
internal delegate Task GameStateChangedHandler(int gameInstanceId, string toUser, string? topic = null, string? payload = null);

internal class EventBackgroundService(
	IDbContextFactory<ApplicationDbContext> dbFactory,
	IOptions<ConnectionStrings> connectionStrings) : BackgroundService
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;
	private readonly ConnectionStrings _connectionStrings = connectionStrings.Value;

	/// <summary>
	/// player added or removed from ready state
	/// </summary>
	public event ReadyStateChangedHandler? ReadyStateChanged;
	/// <summary>
	/// someone made a valid play in an active game
	/// </summary>
	public event GameStateChangedHandler? GameStateChanged;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var cn = new SqlConnection(_connectionStrings.DefaultConnection);

		while (!stoppingToken.IsCancellationRequested)
		{
			var readyMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[Game] IS NOT NULL");
			if (readyMessage != null && ReadyStateChanged != null)
			{
				await ReadyStateChanged.Invoke(readyMessage.Game!.Value, readyMessage.ToUser, readyMessage.Topic, readyMessage.Payload);
			}

			var gameStateMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[GameInstanceId] IS NOT NULL");
			if (gameStateMessage != null && GameStateChanged != null)
			{
				await GameStateChanged.Invoke(gameStateMessage.GameInstanceId!.Value, gameStateMessage.ToUser, gameStateMessage.Topic, gameStateMessage.Payload);
			}

			await Task.Delay(500, stoppingToken);
		}
	}
}
