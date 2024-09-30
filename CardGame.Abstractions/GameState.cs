namespace CardGame.Abstractions;

public abstract class GameState<TCard>
{
	public int Id { get; set; }
	public HashSet<Player<TCard>> Players { get; init; } = [];
	public Dictionary<int, Player<TCard>> PlayersByIndex { get; set; } = [];
	public Dictionary<string, Player<TCard>> PlayersByName { get; set; } = [];
	/// <summary>
	/// allow logged on player to impersonate all players
	/// </summary>
	public bool IsDevMode { get; init; }
	/// <summary>
	/// whose turn is it?
	/// </summary>
	public Player<TCard>? CurrentPlayer { get; set; }
	public Queue<TCard> DrawPile { get; set; } = [];
	public Func<Task>? OnStateChanged { get; set; }

	protected Player<TCard> NextPlayer()
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		var index = PlayersByName[CurrentPlayer.Name].Index;

		index++;
		if (index > Players.Count) index = 0;

		return PlayersByIndex[index];
	}	
}
