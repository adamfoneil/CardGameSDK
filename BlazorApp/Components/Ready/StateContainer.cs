using AppService;

namespace BlazorApp.Components.Ready;

public class StateContainer
{
	public event Func<SupportedGames, Task>? OnPlayersUpdatedAsync;
	public event Action<SupportedGames, string>? OnGameStarted;

	public void NotifyPlayersUpdated(SupportedGames game) => OnPlayersUpdatedAsync?.Invoke(game);
	public void NotifyGameStarted(SupportedGames game, string url) => OnGameStarted?.Invoke(game, url);
}
