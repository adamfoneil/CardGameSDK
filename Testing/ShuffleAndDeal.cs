using CardGame.Abstractions;
using CardGame.Abstractions.Games;
using System.Diagnostics;

namespace Testing;

[TestClass]
public class ShuffleAndDeal
{
    [TestMethod]
    public async Task HeartsDeal()
    {
        var hearts = new Hearts(new MockRepository());
        var game = await hearts.NewGameAsync(false, [ "Adam", "Andy", "Dad", "Becky" ]);

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