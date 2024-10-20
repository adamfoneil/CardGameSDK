using CardGame.Abstractions;
using HashidsNet;

namespace Games.FoxInTheForest;

public class FoxInTheForestGameFactory(IHashids hashids) : GameFactory<FoxInTheForestState, PlayingCard>
{
	private readonly IHashids _hashids = hashids;

	public override string Name => "Fox in the Forest";

	public override uint MinPlayers => 2;

	public override uint MaxPlayers => 2;

	protected override uint CardsPerHand => 13;

	public override IEnumerable<PlayingCard> Deck => Suits
		.SelectMany(suit => Enumerable.Range(1, 11).Select(val => new PlayingCard(val, suit)));

	public override string[] TestModePlayerNames => throw new NotImplementedException();

	private static readonly string[] Suits =
	[
		"Moons",
		"Keys",
		"Bells"
	];	

	protected override FoxInTheForestState CreateGameState(
		bool testMode,
		HashSet<Player<PlayingCard>> players,
		Queue<PlayingCard> drawPile)
	{
		var startPlayer = players.ToArray()[Random.Shared.Next(1, players.Count)];

		return new()
		{
			IsTestMode = testMode,
			DecreeCard = drawPile.Dequeue(),
			DrawPile = drawPile,
			Players = players,
			CurrentPlayer = startPlayer
		};
	}

	public override string GetUrl(int gameInstanceId)
	{
		throw new NotImplementedException();
	}

	public override FoxInTheForestState StartNewRound(FoxInTheForestState state)
	{
		throw new NotImplementedException();
	}

	public override (bool Result, string? Winner, string? FinalScore) IsFinished(string[] roundScores)
	{
		throw new NotImplementedException();
	}
}
