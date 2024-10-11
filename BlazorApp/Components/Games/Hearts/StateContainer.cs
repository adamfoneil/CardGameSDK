namespace BlazorApp.Components.Games.Hearts;

public class StateContainer
{
    public event Func<string, Task>? OnChangeAsync;

    public void NotifyStateChanged(string userName) => OnChangeAsync?.Invoke(userName);
}
