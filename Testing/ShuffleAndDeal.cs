using CardGame.Abstractions;
using Games.FoxInTheForest;
using Games.Hearts;
using System.Diagnostics;
using System.Text.Json;

namespace Testing;

[TestClass]
public class ShuffleAndDeal
{
	[TestMethod]
	public void HeartsDeal()
	{
		var hearts = new HeartsGameFactory();
		var game = hearts.Start(false, ["Adam", "Andy", "Dad", "Becky"]);

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
		var hearts = new HeartsGameFactory();
		var game = hearts.Start(false, ["Adam", "Andy", "Dad", "Becky"]);

		while (!game.IsFinished) game.AutoPlay();

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
		var foxInTheForest = new FoxInTheForestFactory();
		var game = foxInTheForest.Start(false, ["Adam", "Becky"]);

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
