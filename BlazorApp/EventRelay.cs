using AppService;

namespace BlazorApp;

internal delegate void ReadyStateChangedHandler(SupportedGames game, string toUser, string? topic = null, string? payload = null);
internal delegate void GameStateChangedHandler(int gameInstanceId, string toUser, string? topic = null, string? payload = null);

internal class EventRelay(ILogger<EventRelay> logger)
{
	private readonly ILogger<EventRelay> _logger = logger;

	/// <summary>
	/// player added or removed from ready state
	/// </summary>
	public event ReadyStateChangedHandler? ReadyStateChanged;
	/// <summary>
	/// someone made a valid play in an active game
	/// </summary>
	public event GameStateChangedHandler? GameStateChanged;

	public void CallReadyStateChanged(SupportedGames game, string toUser, string? topic = null, string? payload = null)
	{
		_logger.LogDebug("Ready state changed: {game}, {toUser}, {topic}, {payload}", game, toUser, topic, payload);
		ReadyStateChanged?.Invoke(game, toUser, topic, payload);
	}
		

	public void CallGameStateChanged(int gameInstanceId, string toUser, string? topic = null, string? payload = null)
	{
		_logger.LogDebug("Game state changed: {gameInstanceId}, {toUser}, {topic}, {payload}", gameInstanceId, toUser, topic, payload);
		GameStateChanged?.Invoke(gameInstanceId, toUser, topic, payload);
	}		
}
