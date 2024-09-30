

namespace CardGame.Abstractions.Games;

public class Hearts(IRepository<HeartsGameState> repository) : GameDefinition<HeartsGameState, PlayingCard>(repository)
{
	public override uint MinPlayers => 4;

	public override uint MaxPlayers => 4;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

	// first player = hand with 2 of clubs

	public bool IsHeartsBroken { get; private set; }

	protected override HeartsGameState InitializeGame(bool devMode, string[] playerNames)
	{
		var cards = Shuffle();
		var hands = Deal(cards, playerNames);
		var players = BuildPlayers(playerNames, hands);
		var startPlayer = players.Single(p => p.Hand.Contains(new PlayingCard() { Suit = Suits.Clubs, Rank = 2 }));

		return new()
		{
			IsDevMode = devMode,
			Players = players,
			DrawPile = cards, // should be empty because all cards are dealt
			CurrentPlayer = startPlayer
		};
	}

	private static HashSet<Player<PlayingCard>> BuildPlayers(string[] playerNames, ILookup<string, PlayingCard> hands)
	{
		List<Player<PlayingCard>> result = [];

		foreach (var player in playerNames)
		{
			result.Add(new Player<PlayingCard>()
			{
				Name = player,
				Hand = hands[player].ToHashSet()
			});
		}

		return [.. result];
	}

	private static ILookup<string, PlayingCard> Deal(Queue<PlayingCard> cards, string[] playerNames)
	{
		const int CardsPerHand = 12;

		List<(string PlayerName, PlayingCard Card)> result = [];

		for (int card = 0; card < CardsPerHand; card++)
		{
			foreach (var player in playerNames)
			{
				result.Add((player, cards.Dequeue()));
			}
		}

		return result.ToLookup(card => card.PlayerName, card => card.Card);
	}
}

public class HeartsGameState : IGameState<PlayingCard>
{
	public bool IsDevMode { get; init; }

	public int Id { get; set; }

	public HashSet<Player<PlayingCard>> Players { get; init; } = [];

	public Player<PlayingCard>? CurrentPlayer { get; init; }

	public Queue<PlayingCard> DrawPile { get; init; } = [];
}
