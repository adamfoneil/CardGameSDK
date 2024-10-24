using CardGame.Abstractions;
using Microsoft.Extensions.Logging;

namespace Games.FoxInTheForest;

public class FoxInTheForestState : GameState<PlayingCard>
{
	public override bool IsRoundFinished => throw new NotImplementedException();

	public required PlayingCard DecreeCard { get; init; }

	public override Dictionary<string, int> Score => throw new NotImplementedException();

	public Suit? LeadingSuit { get; set; }

	private readonly List<Play> _currentTrick = [];
	private readonly List<Trick> _tricks = [];

	public List<Trick> Tricks => _tricks;

	public override void AutoPlay()
	{
		throw new NotImplementedException();
	}

	protected override void OnPlayCard(string playerName, PlayingCard card)
	{
		_currentTrick.Add(new Play(playerName, card));

		if (_currentTrick.Count == 1)
		{
			LeadingSuit = card.Suit;
		}

		if (_currentTrick.Count == 2)
		{
			var winnerSuit = _currentTrick.Any(p => p.Card.Suit.Equals(DecreeCard.Suit)) ?
				DecreeCard.Suit :
				_currentTrick[0].Card.Suit;

			var winner = _currentTrick
				.Where(c => c.Card.Suit.Equals(winnerSuit))
				.MaxBy(p => p.Card.Rank)!.PlayerName;

			_tricks.Add(new()
			{
				Plays = [.. _currentTrick],
				Winner = winner,
			});

			_currentTrick.Clear();
			CurrentPlayer = PlayersByName[winner];
		}
		else
		{
			CurrentPlayer = NextPlayer();
		}
	}

	public override (bool IsValid, string? Message) ValidatePlay(string playerName, PlayingCard card)
	{
		if (LeadingSuit != null && !card.Suit.Equals(LeadingSuit))
		{
			if (PlayersByName[playerName].Hand.Any(card => card.Suit.Equals(LeadingSuit)))
			{
				return (false, "Must play leading suit if you can");
			}
		}

		return (true, default);
	}

	protected override int NextPlayerIndex(int currentIndex)
	{
		int result = currentIndex + 1;
		if (result > 2) result = 1;
		return result;
	}

	public class Trick
	{
		public required List<Play> Plays { get; init; } = [];
		public required string Winner { get; init; }
	}
}
