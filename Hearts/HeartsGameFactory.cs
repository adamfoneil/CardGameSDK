using CardGame.Abstractions;
using HashidsNet;
using System.Text.Json;

namespace Games.Hearts;

public class HeartsGameFactory(IHashids hashids) : GameFactory<HeartsGameState, PlayingCard>
{
	private readonly IHashids _hashids = hashids;

	public override uint MinPlayers => 4;

	public override uint MaxPlayers => 4;

	protected override uint CardsPerHand => 12;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.ClassicDeck;

	public override string Name => "Hearts (4p)";

	public override string[] TestModePlayerNames => ["player2", "player3", "player4"];

	protected override HeartsGameState CreateGameState(
		bool testMode,
		HashSet<Player<PlayingCard>> players,
		Queue<PlayingCard> drawPile)
	{       	
		HeartsGameState result = new()
		{
			IsTestMode = testMode,
			Players = players,
		};		

		return result;
	}

	public override string GetUrl(int gameInstanceId) => $"/Hearts/{_hashids.Encode(gameInstanceId)}";

	public override HeartsGameState StartNewRound(HeartsGameState state)
	{
		var newRound = Start(state.IsTestMode, state.Players.Select(player => (player.Name, player.IsTest)).ToArray());

		newRound.PassDirection = state.PassDirection switch
		{
			PlayerOrientation.Left => PlayerOrientation.Right,
			PlayerOrientation.Right => PlayerOrientation.Across,
			PlayerOrientation.Across => PlayerOrientation.Self,
			PlayerOrientation.Self => PlayerOrientation.Left,
			_ => throw new Exception("unknown pass direction")
		};

		if (newRound.PassDirection == PlayerOrientation.Self)
		{
			newRound.Phase = PlayPhase.Play;
			newRound.CurrentPlayer = newRound.Players.Single(p => p.Hand.Contains(new PlayingCard(2, ClassicSuits.Clubs)));
			newRound.PlayCard(newRound.CurrentPlayer.Name, new(2, ClassicSuits.Clubs));						
		}

		return newRound;
	}

	public override (bool Result, string? Winner, string? FinalScore) IsFinished(string[] roundScores)
	{
		var scores = roundScores
			.Select(score => JsonSerializer.Deserialize<Dictionary<string, int>>(score));

		var playerTotals = scores
			.SelectMany(score => score!)
			.GroupBy(score => score.Key)
			.Select(group => (Player: group.Key, Total: group.Sum(score => score.Value)));

		var reachedMax = playerTotals
			.Where(playerTotals => playerTotals.Total >= 100);

		if (reachedMax.Any())
		{
			var winner = playerTotals.MinBy(playerTotals => playerTotals.Total).Player;
			var finalScore = JsonSerializer.Serialize(playerTotals);
			return (true, winner, finalScore);
		}

		return (false, default, default);
	}
}
