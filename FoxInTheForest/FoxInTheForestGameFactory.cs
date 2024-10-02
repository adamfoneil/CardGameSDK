using CardGame.Abstractions;
using FoxInTheForest;

namespace Games.FoxInTheForest;

public class FoxInTheForestFactory : GameFactory<FoxInTheForestState, PlayingCard>
{
	public override string Name => "Fox in the Forest";

	public override uint MinPlayers => 2;

	public override uint MaxPlayers => 2;

	public override IEnumerable<PlayingCard> Deck => Suits
		.SelectMany(suit => Enumerable.Range(1, 10).Select(val => new PlayingCard(val, suit)));

	private static readonly string[] Suits =
	[
		"Moons",
		"Keys",
		"Bells"
	];

	protected override FoxInTheForestState CreateGameState(
		bool devMode, 
		HashSet<Player<PlayingCard>> players, 
		Dictionary<int, Player<PlayingCard>> byIndex, 
		Dictionary<string, Player<PlayingCard>> byName, 
		Queue<PlayingCard> drawPile)
	{
		throw new NotImplementedException();
	}

	protected override ILookup<string, PlayingCard> Deal(Queue<PlayingCard> cards, string[] playerNames)
	{
		throw new NotImplementedException();
	}
}
