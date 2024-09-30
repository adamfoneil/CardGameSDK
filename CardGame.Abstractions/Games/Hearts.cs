using System.Reflection;

namespace CardGame.Abstractions.Games;

public class Hearts : GameDefinition<HeartsGameState, PlayingCard>
{
	public override uint MinPlayers => 4;

	public override uint MaxPlayers => 4;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

	public override string Name => "Hearts (4p)";
	
	public override HeartsGameState InitializeGame(bool devMode, string[] playerNames)
	{
		if (playerNames.Length != 4) throw new Exception("Must have 4 players");

		var cards = Shuffle();
		var hands = Deal(cards, playerNames);
		var (players, byIndex, byName) = BuildPlayers(playerNames, hands);				
		var startPlayer = players.Single(p => p.Hand.Contains(new PlayingCard(2, Suits.Clubs)));

		HeartsGameState result = new()
		{
			IsDevMode = devMode,
			Players = players,
			PlayersByIndex = byIndex,
			PlayersByName = byName,
			DrawPile = cards, // should be empty because all cards are dealt
			CurrentPlayer = startPlayer
		};

		result.PlayCard(new(2, Suits.Clubs));

		return result;
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

public class HeartsGameState : GameState<PlayingCard>
{
	public Suit? LeadingSuit { get; private set; }
	public bool IsHeartsBroken { get; private set; }

	public readonly List<Play> CurrentTrick = [];
	public readonly List<Trick> Tricks = [];

	private static int PointValue(PlayingCard card) =>
		card.Suit == Suits.Hearts ? 1 :
		card.Suit == Suits.Spades && card.Rank == NamedRanks.Queen ? 26 :
		0;
	
	public override void PlayCard(PlayingCard card)
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		if (CurrentTrick.Count == 0)
		{
			LeadingSuit = card.Suit;
		}

		CurrentTrick.Add(new(CurrentPlayer.Name, card));
		CurrentPlayer.Hand.Remove(card);

		if (!IsHeartsBroken && card.Suit == Suits.Hearts)
		{
			// todo: special event architecture?
			IsHeartsBroken = true;
		}

		if (CurrentTrick.Count == 4)
		{
			var winner = CurrentTrick.MaxBy(p => p.Card.Rank)!.PlayerName;			
			Tricks.Add(new()
			{
				Plays = CurrentTrick,
				Winner = winner,
				Points = CurrentTrick.Sum(play => PointValue(play.Card))
			});

			CurrentTrick.Clear();
			LeadingSuit = null;
			CurrentPlayer = PlayersByName[winner];
		}
		else
		{
			CurrentPlayer = NextPlayer();
		}

		OnStateChanged?.Invoke();
	}

	public override (bool IsValid, string? Message) ValidatePlay(string playerName, PlayingCard card)
	{
		if (Tricks.Count == 0 && card.Suit == Suits.Hearts) return (false, "Cannot break hearts on the first trick");

		if (!IsHeartsBroken && card.Suit == Suits.Hearts) return (false, "Hearts not broken yet");

		if (card.Suit != LeadingSuit)
		{
			if (PlayersByName[playerName].Hand.Any(card => card.Suit == LeadingSuit))
			{
				return (false, "Must play leading suit if you can");
			}
		}

		return (true, default);
	}

	public class Trick
	{
		public required List<Play> Plays { get; init; } = [];
		public required string Winner { get; init; }
		public required int Points { get; init; }
	}	
}
