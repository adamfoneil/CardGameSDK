using CardGame.Abstractions.Games;
using System.Diagnostics;

namespace Testing
{
    [TestClass]
    public class ShuffleAndDeal
    {
        [TestMethod]
        public void HeartsDeal()
        {
            var game = new Hearts();
            var cards = game.Shuffle();
            var deal = game.Deal(cards, 4);

            foreach (var hand in deal)
            {
                Debug.Print("Player " + hand.Key.ToString());
                int index = 0;
                foreach (var card in hand)
                {
                    index++;
                    Debug.Print("- " + card.ToString() + $" ({index})");
                }
            }

            Assert.IsTrue(!cards.Any());
        }
    }
}