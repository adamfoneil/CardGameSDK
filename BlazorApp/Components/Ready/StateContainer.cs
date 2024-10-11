using AppService;

namespace BlazorApp.Components.Ready;

public class StateContainer
{
	public event Action<SupportedGames>? OnPlayersUpdated;
	public event Action<SupportedGames, string>? OnGameStarted;

	public void NotifyPlayersUpdated(SupportedGames game) => OnPlayersUpdated?.Invoke(game);
	public void NotifyGameStarted(SupportedGames game, string url) => OnGameStarted?.Invoke(game, url);
}
