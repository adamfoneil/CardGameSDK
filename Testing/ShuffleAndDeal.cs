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

            foreach (var stack in deal)
            {
                Debug.Print("Player " + stack.Key.ToString());
                foreach (var card in stack)
                {
                    Debug.Print("- " + card.ToString());
                }
            }

            Assert.IsTrue(!cards.Any());
        }
    }
}