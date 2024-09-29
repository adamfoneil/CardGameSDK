namespace CardGame.Abstractions;

public abstract class GameDefinition<TCard>
{
    public abstract uint MinPlayers { get; }
    public abstract uint MaxPlayers { get; }
    public abstract uint CardsPerHand { get; }

    public abstract IEnumerable<TCard> Deck { get; }

    /// <summary>
    /// returns a copy of the Deck in randomized order
    /// </summary>
    public Queue<TCard> Shuffle()
    {
        var shuffled = Deck
            .Select(card => new { Card = card, RandomValue = Random.Shared.Next(1000) })
            .OrderBy(indexed => indexed.RandomValue)
            .Select(indexedCard => indexedCard.Card)
            .ToList();

        Queue<TCard> result = new();

        shuffled.ForEach(result.Enqueue);

        return result;
    }

    public ILookup<int, TCard> Deal(Queue<TCard> cards, int playerCount)
    {
        List<(int PlayerIndex, TCard Card)> result = [];

        for (int card = 0; card < CardsPerHand; card++)
        {
            for (int player = 0; player < playerCount; player++)
            {
                result.Add((player, cards.Dequeue()));
            }
        }

        return result.ToLookup(card => card.PlayerIndex, card => card.Card);
    }
}
