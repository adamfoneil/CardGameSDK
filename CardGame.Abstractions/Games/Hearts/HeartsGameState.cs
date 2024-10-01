﻿namespace CardGame.Abstractions.Games.Hearts;

public class HeartsGameState : GameState<PlayingCard>
{
	private bool _isRoundFinished = false;

	public Suit? LeadingSuit { get; private set; }
	public bool IsHeartsBroken { get; private set; }
	public string? MoonShotPlayer { get; private set; }

	public override bool IsFinished => _isRoundFinished;

	private readonly List<Play> _currentTrick = [];
	private readonly List<Trick> _tricks = [];

	public List<Trick> Tricks => _tricks;

	private static int PointValue(PlayingCard card) =>
		card.Suit == Suits.Hearts ? 1 :
		card.Suit == Suits.Spades && card.Rank == NamedRanks.Queen ? 13 :
		0;

	public override void PlayCard(PlayingCard card)
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		var (valid, message) = ValidatePlay(CurrentPlayer.Name, card);
		if (!valid) throw new Exception(message);

		if (_currentTrick.Count == 0)
		{
			LeadingSuit = card.Suit;
		}

		_currentTrick.Add(new(CurrentPlayer.Name, card));
		CurrentPlayer.Hand.Remove(card);

		if (!IsHeartsBroken && card.Suit == Suits.Hearts)
		{
			// todo: special event architecture?
			IsHeartsBroken = true;
		}

		if (_currentTrick.Count == 4)
		{
			var winner = _currentTrick
				.Where(c => c.Card.Suit == LeadingSuit)
				.MaxBy(p => p.Card.Rank)!.PlayerName;

			_tricks.Add(new()
			{
				Plays = _currentTrick,
				Winner = winner,
				Points = _currentTrick.Sum(play => PointValue(play.Card))
			});

			_currentTrick.Clear();
			LeadingSuit = null;
			CurrentPlayer = PlayersByName[winner];
		}
		else
		{
			CurrentPlayer = NextPlayer();
		}

		// on the last trick...
		if (_tricks.Count == 12)
		{
			// did anyone get all the hearts (shoot the moon)?
			var playersWithHearts = _tricks
				.SelectMany(t => t.Plays.Where(p => p.Card.Suit == Suits.Hearts).Select(p => p.PlayerName))
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
		if (_tricks.Count == 0 && card.Suit == Suits.Hearts) return (false, "Cannot break hearts on the first trick");

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

	public override Dictionary<string, int> GetScore()
	{
		var results = _tricks
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

	public override void AutoPlay()
	{
		if (_isRoundFinished) return;

		if (CurrentPlayer is null) throw new Exception("must have current player");

		var card = ((IsHeartsBroken ?
			CurrentPlayer.Hand.First(c => c.Suit == Suits.Hearts) :
			CurrentPlayer.Hand.FirstOrDefault(c => c.Suit == LeadingSuit,
			CurrentPlayer.Hand.First())));

		PlayCard(card);
	}

	public class Trick
	{
		public required List<Play> Plays { get; init; } = [];
		public required string Winner { get; init; }
		public required int Points { get; init; }
	}
}