namespace BlazorApp;

public class StateContainer
{
    public event Func<Task>? OnChangeAsync;
}
