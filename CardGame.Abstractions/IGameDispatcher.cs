
namespace CardGame.Abstractions;

/// <summary>
/// provides the minimum information needed to create a game state,
/// without explicit reference to game's state type
/// </summary>
public interface IGameDispatcher
{	
	uint MaxPlayers { get; }
	uint MinPlayers { get; }
	string Name { get; }
	string GetUrl(int gameInstanceId);
	string[] TestModePlayerNames { get; }

	object CreateStateObject(bool devMode, string[] playerNames);
}