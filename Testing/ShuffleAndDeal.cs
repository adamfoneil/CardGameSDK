using CardGame.Abstractions;
using CardGame.Abstractions.Games.Hearts;
using System.Diagnostics;

namespace Testing;

[TestClass]
public class ShuffleAndDeal
{
    [TestMethod]
    public void HeartsDeal()
    {
        var hearts = new HeartsGameFactory();
        var game = hearts.InitializeGame(false, [ "Adam", "Andy", "Dad", "Becky" ]);

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

        Debug.Print($"Current player: {game.CurrentPlayer?.Name ?? "<unknown>"}");
        Debug.Print($"Leading suit: {game.LeadingSuit.Name}");

        Assert.IsTrue(!game.DrawPile.Any());
    }
}
