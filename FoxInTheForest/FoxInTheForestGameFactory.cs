using CardGame.Abstractions;
using HashidsNet;
using Microsoft.Extensions.Logging;

namespace Games.FoxInTheForest;

public class FoxInTheForestGameFactory(IHashids hashids, ILogger<FoxInTheForestState> logger) : GameFactory<FoxInTheForestState, PlayingCard>
{
	private readonly IHashids _hashids = hashids;
	private readonly ILogger<FoxInTheForestState> _logger = logger;

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

		FoxInTheForestState result = new()
		{
			IsTestMode = testMode,
			DecreeCard = drawPile.Dequeue(),
			DrawPile = drawPile,
			Players = players,
			CurrentPlayer = startPlayer
		};

		result.Logger = _logger;
		return result;
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
