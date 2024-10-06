﻿namespace CardGame.Abstractions;

public abstract class GameFactory<TState, TCard> : IGameDispatcher where TState : notnull
{
	public abstract string Name { get; }
	public abstract uint MinPlayers { get; }
	public abstract uint MaxPlayers { get; }
	protected abstract uint CardsPerHand { get; }

	public abstract IEnumerable<TCard> Deck { get; }

	protected abstract TState CreateGameState(
		bool devMode,
		HashSet<Player<TCard>> players,
		Dictionary<int, Player<TCard>> byIndex,
		Dictionary<string, Player<TCard>> byName,
		Queue<TCard> drawPile);

	public TState Start(bool devMode, string[] playerNames)
	{
		if (playerNames.Length < MinPlayers) throw new Exception("not enough players");
		if (playerNames.Length > MaxPlayers) throw new Exception("too many players");

		var cards = Shuffle();
		var hands = Deal(CardsPerHand, cards, playerNames);
		var (players, byIndex, byName) = BuildPlayers(playerNames, hands);

		return CreateGameState(devMode, players, byIndex, byName, cards);
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

	private static (
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
	public object CreateStateObject(bool devMode, string[] playerNames) => Start(devMode, playerNames);
}
