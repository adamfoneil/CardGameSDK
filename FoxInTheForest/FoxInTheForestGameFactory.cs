﻿using CardGame.Abstractions;
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

	public override string[] DevModePlayerNames => throw new NotImplementedException();

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
		var startPlayer = byIndex[Random.Shared.Next(1, byIndex.Count)];

		return new()
		{
			IsDevMode = devMode,
			DecreeCard = drawPile.Dequeue(),
			DrawPile = drawPile,
			Players = players,
			PlayersByIndex = byIndex,
			PlayersByName = byName,
			CurrentPlayer = startPlayer
		};
	}

	public override string GetUrl(int gameInstanceId)
	{
		throw new NotImplementedException();
	}
}
