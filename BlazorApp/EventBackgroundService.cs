using AppService;
using AppService.Entities;
using BlazorApp.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorApp;

internal class EventBackgroundService(
	EventRelay eventRelay,
	IDbContextFactory<ApplicationDbContext> dbFactory,
	IOptions<ConnectionStrings> connectionStrings) : BackgroundService
{
	private readonly EventRelay _eventRelay = eventRelay;
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;
	private readonly ConnectionStrings _connectionStrings = connectionStrings.Value;	

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var cn = new SqlConnection(_connectionStrings.DefaultConnection);

		while (!stoppingToken.IsCancellationRequested)
		{
			var readyMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[Game] IS NOT NULL");
			if (readyMessage != null)
			{
				_eventRelay.CallReadyStateChanged(readyMessage.Game!.Value, readyMessage.ToUser, readyMessage.Topic, readyMessage.Payload);
			}

			var gameStateMessage = await cn.DequeueAsync<EventMessage>("[dbo].[Events]", "[GameInstanceId] IS NOT NULL");
			if (gameStateMessage != null)
			{
				_eventRelay.CallGameStateChanged(gameStateMessage.GameInstanceId!.Value, gameStateMessage.ToUser, gameStateMessage.Topic, gameStateMessage.Payload);
			}

			await Task.Delay(500, stoppingToken);
		}
	}
}
