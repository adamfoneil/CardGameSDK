using System.Data;

namespace CardGame.Abstractions;

public interface IGameState<TCard>
{
	int Id { get; }
	HashSet<Player<TCard>> Players { get; init; }
	/// <summary>
	/// allow logged on player to impersonate all players
	/// </summary>
	bool IsDevMode { get; }
	/// <summary>
	/// whose turn is it?
	/// </summary>
	Player<TCard>? CurrentPlayer { get; }
	Queue<TCard> DrawPile { get; }
}

public class Player<TCard>
{
	public string Name { get; init; } = default!;
	public bool Resigned { get; set; }
	public HashSet<TCard> Hand { get; init; } = [];

	public override bool Equals(object? obj) =>
		(obj is Player<TCard> player) && Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => Name.GetHashCode();	
}