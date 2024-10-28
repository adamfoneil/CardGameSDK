using AppService.Entities;
using BlazorApp.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace BlazorApp;

internal class EventBackgroundService(
	EventRelay eventRelay,
	ILogger<EventBackgroundService> logger,
	IOptions<ConnectionStrings> connectionStrings) : BackgroundService
{
	private readonly EventRelay _eventRelay = eventRelay;
	private readonly ILogger<EventBackgroundService> _logger = logger;
	private readonly ConnectionStrings _connectionStrings = connectionStrings.Value;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var cn = new SqlConnection(_connectionStrings.DefaultConnection);

		while (!stoppingToken.IsCancellationRequested)
		{
			var readyMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[Game] IS NOT NULL");
			if (readyMessage != null)
			{
				_logger.LogDebug("Ready state changed: {@message}", readyMessage);
				_eventRelay.NotifyReadyStateChanged(readyMessage);
			}

			var gameStateMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[GameInstanceId] IS NOT NULL");
			if (gameStateMessage != null)
			{
				_logger.LogDebug("Game state changed: {@message}", gameStateMessage);
				_eventRelay.NotifyGameStateChanged(gameStateMessage);
			}

			await Task.Delay(500, stoppingToken);
		}
	}
}
