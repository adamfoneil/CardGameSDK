namespace CardGame.Abstractions;

public abstract class GameDefinition<TState, TCard> where TState : GameState<TCard>
{
	public abstract string Name { get; }
	public abstract uint MinPlayers { get; }
	public abstract uint MaxPlayers { get; }
	public abstract IEnumerable<TCard> Deck { get; }

	public abstract TState InitializeGame(bool devMode, string[] playerNames);

	/// <summary>
	/// returns a copy of the Deck in randomized order
	/// </summary>
	protected Queue<TCard> Shuffle()
	{
		var shuffled = Deck
			.Select(card => new { Card = card, RandomValue = Random.Shared.Next(1000) })
			.OrderBy(indexed => indexed.RandomValue)
			.Select(indexedCard => indexedCard.Card)
			.ToList();

		Queue<TCard> result = new();

		shuffled.ForEach(result.Enqueue);

		return result;
	}

	protected static (
		HashSet<Player<TCard>> HashSet,
		Dictionary<int, Player<TCard>> ByIndex,
		Dictionary<string, Player<TCard>> ByName
	) BuildPlayers(string[] playerNames, ILookup<string, TCard> hands)
	{
		List<Player<TCard>> result = [];

		int index = 0;
		foreach (var player in playerNames)
		{
			index++;
			result.Add(new Player<TCard>()
			{
				Name = player,
				Index = index,
				Hand = hands[player].ToHashSet()
			});
		}

		HashSet<Player<TCard>> hashSet = [.. result];

		return (hashSet, result.ToDictionary(p => p.Index), result.ToDictionary(p => p.Name));
	}
}
