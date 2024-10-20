namespace CardGame.Abstractions;

public abstract class GameFactory<TState, TCard> : IGameDispatcher where TState : notnull
{
	public abstract string Name { get; }
	public abstract uint MinPlayers { get; }
	public abstract uint MaxPlayers { get; }
	protected abstract uint CardsPerHand { get; }

	public abstract IEnumerable<TCard> Deck { get; }

	public abstract string[] TestModePlayerNames { get; }

	protected abstract TState CreateGameState(
		bool testMode,
		HashSet<Player<TCard>> players,
		Queue<TCard> drawPile);

	public abstract TState StartNewRound(TState state);

	public TState Start(bool testMode, (string Name, bool IsTest)[] players)
	{
		if (players.Length < MinPlayers) throw new Exception("not enough players");
		if (players.Length > MaxPlayers) throw new Exception("too many players");

		var cards = Shuffle();
		var hands = Deal(CardsPerHand, cards, players.Select(p => p.Name).ToArray());
		var playerEntities = BuildPlayers(players, hands);

		return CreateGameState(testMode, playerEntities, cards);
	}

	/// <summary>
	/// returns a copy of the Deck in randomized order
	/// </summary>
	private Queue<TCard> Shuffle()
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

	private static HashSet<Player<TCard>> BuildPlayers((string Name, bool IsTest)[] playerNames, ILookup<string, TCard> hands)
	{
		List<Player<TCard>> result = [];

		int index = 0;
		foreach (var player in playerNames)
		{
			index++;
			result.Add(new Player<TCard>()
			{
				Name = player.Name,
				Index = index,
				Hand = hands[player.Name].ToHashSet(),
				IsTest = player.IsTest
			});
		}

		HashSet<Player<TCard>> hashSet = [.. result];

		return hashSet;
	}

	private static ILookup<string, TCard> Deal(uint cardsPerHand, Queue<TCard> cards, string[] playerNames)
	{
		List<(string PlayerName, TCard Card)> result = [];

		for (int card = 0; card < cardsPerHand; card++)
		{
			foreach (var player in playerNames)
			{
				result.Add((player, cards.Dequeue()));
			}
		}

		return result.ToLookup(card => card.PlayerName, card => card.Card);
	}

	/// <summary>
	/// this is used with dispatch/launcher pages that are not type-specific
	/// </summary>
	public object CreateStateObject(bool testMode, (string Name, bool IsTest)[] players) => Start(testMode, players);

	/// <summary>
	/// link to page for playing a particular instance of the game
	/// </summary>
	public abstract string GetUrl(int gameInstanceId);

	public abstract (bool Result, string? Winner, string? FinalScore) IsFinished(string[] roundScores);
}
