

using CardGame.Abstractions.Extensions;

namespace CardGame.Abstractions.Games;

public class Hearts : GameDefinition<HeartsGameState, PlayingCard>
{
	public override uint MinPlayers => 4;

	public override uint MaxPlayers => 4;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

	// first player = hand with 2 of clubs

	public bool IsHeartsBroken { get; private set; }

	public override string Name => "Hearts (4p)";	

	public override HeartsGameState InitializeGame(bool devMode, string[] playerNames)
	{
		var cards = Shuffle();
		var hands = Deal(cards, playerNames);
		var players = BuildPlayers(playerNames, hands);		
		var playersByIndex = players.ToDictionary(p => p.Index);
		var playersByName = players.ToDictionary(p => p.Name);
		var startPlayer = players.Single(p => p.Hand.Contains(new PlayingCard(2, Suits.Clubs)));

		return new()
		{
			IsDevMode = devMode,
			Players = players,
			PlayersByIndex = playersByIndex,
			PlayersByName = playersByName,
			DrawPile = cards, // should be empty because all cards are dealt
			CurrentPlayer = startPlayer
		};
	}

	private static HashSet<Player<PlayingCard>> BuildPlayers(string[] playerNames, ILookup<string, PlayingCard> hands)
	{
		List<Player<PlayingCard>> result = [];

		int index = 0;
		foreach (var player in playerNames)
		{
			index++;
			result.Add(new Player<PlayingCard>()
			{
				Name = player,
				Index = index,
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
	public int Id { get; set; }
	public bool IsDevMode { get; init; }	
	public HashSet<Player<PlayingCard>> Players { get; init; } = [];
	public Player<PlayingCard>? CurrentPlayer { get; set; }
	public Queue<PlayingCard> DrawPile { get; init; } = [];
	public Dictionary<int, Player<PlayingCard>> PlayersByIndex { get; init; } = [];
	public Dictionary<string, Player<PlayingCard>> PlayersByName { get; init; } = [];

	private readonly List<Play> _currentPlays = [];	

	public void AddPlay(PlayingCard card)
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		_currentPlays.Add(new(CurrentPlayer.Name, card));
		CurrentPlayer = this.NextPlayer();

		if (_currentPlays.Count == 4)
		{
			// determine winner, start new trick
		}
	}

	public class Trick
	{
		public List<Play> Plays { get; init; } = [];
		public Play Winner { get; init; }
	}

	public record Play(string PlayerName, PlayingCard Card);	
}
