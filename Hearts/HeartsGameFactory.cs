﻿using CardGame.Abstractions;
using HashidsNet;

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
}
