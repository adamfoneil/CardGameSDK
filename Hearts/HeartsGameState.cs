using CardGame.Abstractions;
using System.Text.Json.Serialization;

namespace Games.Hearts;

public enum PlayerOrientation
{
	Self = 0,
	Left = 1,
	Right = 2,
	Across = 3	
}

public class HeartsGameState : GameState<PlayingCard>
{
	private bool _isRoundFinished = false;

	public Suit? LeadingSuit { get; set; }
	public bool IsHeartsBroken { get; set; }
	public string? MoonShotPlayer { get; set; }

	public override bool IsFinished => _isRoundFinished;
	
	public List<Play> CurrentTrick { get; set; } = [];
	public List<Trick> Tricks { get; set; } = [];

	public Dictionary<string, PlayingCard> CurrentPlaysByPlayer => CurrentTrick.ToDictionary(p => p.PlayerName, p => p.Card);

	public PlayingCard? GetCurrentCard(string playerName) => CurrentPlaysByPlayer.TryGetValue(playerName, out var card) ? card : null;

	public Playslot GetRelativePlayslot(string playerName, PlayerOrientation orientation)
	{
		var playerNames = Players.Select(p => p.Name).ToArray();
		var selfIndex = Array.IndexOf(playerNames, playerName);
		var targetIndex = orientation switch
		{
			PlayerOrientation.Self => selfIndex,
			PlayerOrientation.Left => selfIndex - 1,
			PlayerOrientation.Right => selfIndex + 1,
			PlayerOrientation.Across => selfIndex + 2,
			_ => throw new ArgumentOutOfRangeException(nameof(orientation))
		};

		// wraparound
		if (targetIndex < 0) targetIndex += Players.Count;
		if (targetIndex >= Players.Count) targetIndex -= Players.Count;

		var playsByName = CurrentTrick.ToDictionary(t => t.PlayerName);
		var playerAtIndex = playerNames[targetIndex];

		return playsByName.TryGetValue(playerAtIndex, out var play) ? 
			new Playslot() { Card = play.Card, PlayerName = playerAtIndex } : 
			new Playslot() { PlayerName = playerAtIndex };
	}

	public Trick[] MyTricks(string playerName) => Tricks.Where(t => t.Winner.Equals(playerName)).ToArray();

	public override Dictionary<string, int> Score
	{
		get
		{
			var results = Tricks
				.GroupBy(t => t.Winner)
				.ToDictionary(grp => grp.Key, grp => grp.Sum(t => t.Points));

			var zeroPointPlayers = Players.Select(p => p.Name).Except(results.Keys);

			foreach (var player in zeroPointPlayers) results.Add(player, 0);

			if (MoonShotPlayer != null)
			{
				// add 26 to everyone's score
				foreach (var kp in results) results[kp.Key] += 26;
				// but set the moon shooter to 0
				results[MoonShotPlayer] = 0;

			}

			return results ?? [];
		}
	}

	private static int PointValue(PlayingCard card) =>
		card.Suit.Equals(ClassicSuits.Hearts) ? 1 :
		card.Suit.Equals(ClassicSuits.Spades) && card.Rank == ClassicNamedRanks.Queen ? 13 :
		0;

	protected override void OnPlayCard(PlayingCard card)
	{
		if (CurrentTrick.Count == 0)
		{
			LeadingSuit = card.Suit;
		}

		CurrentTrick.Add(new(CurrentPlayer!.Name, card));
		CurrentPlayer.Hand.Remove(card);

		if (!IsHeartsBroken && card.Suit.Equals(ClassicSuits.Hearts))
		{
			// todo: special event architecture?
			IsHeartsBroken = true;
		}

		if (CurrentTrick.Count == 4)
		{
			var winner = CurrentTrick
				.Where(c => c.Card.Suit.Equals(LeadingSuit))
				.MaxBy(p => p.Card.Rank)!.PlayerName;

			Tricks.Add(new()
			{
				Plays = [.. CurrentTrick],
				Winner = winner,
				Points = CurrentTrick.Sum(play => PointValue(play.Card)),
				HeartsBroken = IsHeartsBroken
			});

			CurrentTrick.Clear();
			LeadingSuit = null;
			CurrentPlayer = PlayersByName[winner];
		}
		else
		{
			CurrentPlayer = NextPlayer();
		}

		// on the last trick...
		if (Tricks.Count == 12)
		{
			// did anyone get all the hearts (shoot the moon)?
			var playersWithHearts = Tricks
				.SelectMany(t => t.Plays.Where(p => p.Card.Suit.Equals(ClassicSuits.Hearts)).Select(p => p.PlayerName))
				.Distinct();

			// if exactly one player has hearts, they must have all by definition
			if (playersWithHearts.Count() == 1)
			{
				MoonShotPlayer = playersWithHearts.First();
			}

			_isRoundFinished = true;
		}

		OnStateChanged?.Invoke();
	}

	public override (bool IsValid, string? Message) ValidatePlay(string playerName, PlayingCard card)
	{
		if (Tricks.Count == 0)
		{
			if (card.Suit.Equals(ClassicSuits.Hearts))
			{
				if (!PlayersByName[playerName].Hand.All(c => c.Suit.Equals(ClassicSuits.Hearts)))
				{
					return (false, "Cannot break hearts on the first trick");
				}
			}

			if (card.Suit.Equals(ClassicSuits.Spades) && card.Rank == ClassicNamedRanks.Queen)
			{
				return (false, "Cannot play queen of spades on the first trick");
			}
		}

		if (!IsHeartsBroken && card.Suit.Equals(ClassicSuits.Hearts))
		{
			if (LeadingSuit is null)
			{
				return (false, "Cannot lead with hearts until broken");
			}

			// if you're void in leading suit, you can play hearts
			if (!PlayersByName[playerName].Hand.Any(c => c.Suit.Equals(LeadingSuit)))
			{
				return (true, default);
			}

			return (false, "Hearts not broken yet");
		}

		if (!card.Suit.Equals(LeadingSuit))
		{
			if (PlayersByName[playerName].Hand.Any(card => card.Suit.Equals(LeadingSuit)))
			{
				return (false, $"Must play leading suit {LeadingSuit!.Name} if you can");
			}
		}

		return (true, default);
	}

	public override void AutoPlay()
	{
		if (_isRoundFinished) return;

		if (CurrentPlayer is null) throw new Exception("must have current player");

		var firstHeart = CurrentPlayer.Hand.FirstOrDefault(c => c.Suit.Equals(ClassicSuits.Hearts));
		var firstOfLeadingSuit = CurrentPlayer.Hand.FirstOrDefault(c => c.Suit.Equals(LeadingSuit));
		var firstNonHeart = CurrentPlayer.Hand.FirstOrDefault(c => !c.Suit.Equals(ClassicSuits.Hearts));
		var firstOfAny = CurrentPlayer.Hand.First();	

		var card = IsHeartsBroken ?
			firstHeart ?? firstOfLeadingSuit ?? firstOfAny :
			firstOfLeadingSuit ?? firstNonHeart ?? firstOfAny;		

		OnPlayCard(card);
	}

	public class Trick
	{
		public required List<Play> Plays { get; init; } = [];
		public required string Winner { get; init; }
		public required int Points { get; init; }
		public bool HeartsBroken { get; init; }

		[JsonIgnore]
		public Dictionary<string, PlayingCard> PlaysByName => Plays.ToDictionary(p => p.PlayerName, p => p.Card);
		[JsonIgnore]
		public PlayingCard WinningCard => PlaysByName.TryGetValue(Winner, out var card) ? card : throw new Exception("no winning card");
	}

	public class Playslot
	{
		public string PlayerName { get; init; } = default!;
		public PlayingCard? Card { get; set; }
	}
}