namespace BlazorApp;

public class StateContainer
{
    public event Func<Task>? OnChangeAsync;

    public void NotifyStateChanged() => OnChangeAsync?.Invoke();
}
