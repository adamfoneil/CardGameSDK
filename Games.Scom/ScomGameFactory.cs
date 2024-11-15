using CardGame.Abstractions;
using HashidsNet;
using Microsoft.Extensions.Logging;

namespace Games.Scom;

public class ScomGameFactory(IHashids hashids, ILogger<ScomGameFactory> logger) : GameFactory<ScomGameState, Piece>
{
	private readonly IHashids _hashids = hashids;
	private readonly ILogger<ScomGameFactory> _logger = logger;

	public override string Name => "Scom";

	public override uint MinPlayers => 2;

	public override uint MaxPlayers => 2;

	public override uint CardsPerHand => throw new NotImplementedException();

	public override IEnumerable<Piece> Deck => throw new NotImplementedException();

	public override string[] TestModePlayerNames => [ "player2" ];

	public override string GetUrl(int gameInstanceId)
	{
		throw new NotImplementedException();
	}

	public override (bool Result, string? Winner, string? FinalScore) IsFinished(string[] roundScores)
	{
		throw new NotImplementedException();
	}

	public override ScomGameState StartNewRound(ScomGameState state)
	{
		throw new NotImplementedException();
	}

	protected override ScomGameState CreateGameState(bool testMode, HashSet<Player<Piece>> players, Queue<Piece> drawPile)
	{
		throw new NotImplementedException();
	}
}
