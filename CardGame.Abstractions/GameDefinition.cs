namespace CardGame.Abstractions;

public abstract class GameDefinition<TState, TCard> where TState : IGameState<TCard>
{	
	public abstract uint MinPlayers { get; }
    public abstract uint MaxPlayers { get; }    
    public abstract IEnumerable<TCard> Deck { get; }    

    public abstract TState InitializeGame(bool devMode, string[] playerNames);

    /// <summary>
    /// returns a copy of the Deck in randomized order
    /// </summary>
    protected Queue<TCard> Shuffle()
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
}
