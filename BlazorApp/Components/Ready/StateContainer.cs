namespace BlazorApp.Components.Ready;

public class StateContainer
{
	public event Func<Task>? OnPlayersUpdatedAsync;
	public event Action<string>? OnGameStarted;

	public void NotifyPlayerReady() => OnPlayersUpdatedAsync?.Invoke();
	public void NotifyGameStarted(string url) => OnGameStarted?.Invoke(url);
}
