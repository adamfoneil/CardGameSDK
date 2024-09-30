using CardGame.Abstractions;
using CardGame.Abstractions.Games;
using System.Diagnostics;

namespace Testing;

[TestClass]
public class ShuffleAndDeal
{
    [TestMethod]
    public void HeartsDeal()
    {
        var hearts = new Hearts();
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

internal class MockRepository : IRepository<HeartsGameState>
{
	public Task<HeartsGameState> GetByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task SaveAsync(HeartsGameState data)
	{
		throw new NotImplementedException();
	}
}