using System.Runtime.CompilerServices;

namespace CardGame.Abstractions;

public interface IGameStateLogger
{
	Task LogAsync(int gameInstanceId, string message, [CallerMemberName] string? methodName = null);
}
