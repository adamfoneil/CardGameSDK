using CardGame.Abstractions;
using Games.FoxInTheForest;
using Games.Hearts;
using HashidsNet;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Testing;

[TestClass]
public class ShuffleAndDeal
{
	private static readonly string[] testHeartsPlayerNames = ["Adam", "Andy", "Dad", "Becky"];
	private static readonly string[] testFitFPlayerNames = ["Adam", "Becky"];

	private static readonly (string Name, bool IsTest)[] testHeartsPlayers = testHeartsPlayerNames.Select(p => (p, true)).ToArray();
	private static readonly (string Name, bool Istest)[] testFitFPlayers = testFitFPlayerNames.Select(p => (p, true)).ToArray();

	private static ILogger<T> GetLogger<T>() => new LoggerFactory().CreateLogger<T>();

	[TestMethod]
	public void HeartsDeal()
	{
		var hashIds = new Hashids();
		var hearts = new HeartsGameFactory(hashIds, GetLogger<HeartsGameState>());
		var game = hearts.Start(false, testHeartsPlayers);

		PrintCards(game);

		Debug.Print($"Current player: {game.CurrentPlayer?.Name ?? "<unknown>"}");
		Debug.Print($"Leading suit: {game.LeadingSuit.Name}");

		Assert.IsTrue(!game.DrawPile.Any());
	}

	private static void PrintCards<TCard>(GameState<TCard> game)
	{
		foreach (var player in game.Players)
		{
			Debug.Print(player.Name);
			int index = 0;
			foreach (var card in player.Hand)
			{
				index++;
				Debug.Print("- " + card.ToString() + $" ({index})");
			}
		}
	}

	/// <summary>
	/// simulate play so we can see what state is generated
	/// </summary>
	[TestMethod]
	public void HeartsAutoPlay()
	{
		var hearts = new HeartsGameFactory(new Hashids(), GetLogger<HeartsGameState>());
		var game = hearts.Start(false, testHeartsPlayers);

		while (!game.IsRoundFinished) game.AutoPlay();

		var json = JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true });
		Debug.Print(json);

		// for now, I'm just manually inspecting the json output
	}

	[TestMethod]
	public void SuitEquality()
	{
		// I had some trouble with suit equality
		Assert.AreEqual(ClassicSuits.Clubs, new PlayingCard(2, ClassicSuits.Clubs).Suit);

		// remember to use the Equals method, not ==
		//Assert.IsTrue(Suits.Clubs == new Suit("Clubs", 1));
	}

	[TestMethod]
	public void FoxInTheForestDeal()
	{
		var foxInTheForest = new FoxInTheForestGameFactory(new Hashids(), GetLogger<FoxInTheForestState>());
		var game = foxInTheForest.Start(false, testFitFPlayers);

		PrintCards(game);

		Debug.Print("Decree card:");
		Debug.Print("- " + game.DecreeCard);

		Debug.Print("Draw Pile:");
		int index = 0;
		foreach (var card in game.DrawPile)
		{
			index++;
			Debug.Print("- " + card.ToString() + $" ({index})");
		}
	}
}
