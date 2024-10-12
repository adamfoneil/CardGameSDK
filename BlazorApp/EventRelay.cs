using AppService;

namespace BlazorApp;

internal delegate void ReadyStateChangedHandler(SupportedGames game, string toUser, string? topic = null, string? payload = null);
internal delegate void GameStateChangedHandler(int gameInstanceId, string toUser, string? topic = null, string? payload = null);

internal class EventRelay
{
	/// <summary>
	/// player added or removed from ready state
	/// </summary>
	public event ReadyStateChangedHandler? ReadyStateChanged;
	/// <summary>
	/// someone made a valid play in an active game
	/// </summary>
	public event GameStateChangedHandler? GameStateChanged;

	public void CallReadyStateChanged(SupportedGames game, string toUser, string? topic = null, string? payload = null) =>
		ReadyStateChanged?.Invoke(game, toUser, topic, payload);

	public void CallGameStateChanged(int gameInstanceId, string toUser, string? topic = null, string? payload = null) =>
		GameStateChanged?.Invoke(gameInstanceId, toUser, topic, payload);
}
