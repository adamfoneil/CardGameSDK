namespace CardGame.Abstractions.Games;

public class Hearts : GameDefinition<HeartsGameState, PlayingCard>
{
	public override uint MinPlayers => 4;

	public override uint MaxPlayers => 4;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

	// first player = hand with 2 of clubs

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

		result.AddPlay(new(2, Suits.Clubs));

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

	public readonly List<Play> CurrentPlays = [];
	public readonly List<Trick> Tricks = [];
	
	public void AddPlay(PlayingCard card)
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		if (CurrentPlays.Count == 0)
		{
			LeadingSuit = card.Suit;
		}

		CurrentPlays.Add(new(CurrentPlayer.Name, card));		

		if (CurrentPlays.Count == 4)
		{
			Tricks.Add(new()
			{
				Plays = CurrentPlays,
				Winner = GetWinner(LeadingSuit, CurrentPlays)
			});

			CurrentPlays.Clear();
			LeadingSuit = null;
		}

		CurrentPlayer = NextPlayer();

		OnStateChanged?.Invoke();
	}

	private Play GetWinner(Suit? leadingSuit, List<Play> currentPlays)
	{
		throw new NotImplementedException();
	}

	public class Trick
	{
		public required List<Play> Plays { get; init; } = [];
		public required Play Winner { get; init; }
	}

	public record Play(string PlayerName, PlayingCard Card);	
}
