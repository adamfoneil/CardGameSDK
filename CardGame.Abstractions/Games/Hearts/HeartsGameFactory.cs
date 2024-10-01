namespace CardGame.Abstractions.Games.Hearts;

public class HeartsGameFactory : GameFactory<HeartsGameState, PlayingCard>
{
    public override uint MinPlayers => 4;

    public override uint MaxPlayers => 4;

    public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

    public override string Name => "Hearts (4p)";

    public override HeartsGameState InitializeGame(bool devMode, string[] playerNames)
    {
        if (playerNames.Length != 4) throw new Exception("Must have 4 players");

        var cards = Shuffle();
        var hands = Deal(cards, playerNames);
        var (players, byIndex, byName) = BuildPlayers(playerNames, hands);
        var startPlayer = players.Single(p => p.Hand.Contains(new PlayingCard(2, Suits.Clubs)));

        HeartsGameState result = new()
        {
            IsDevMode = devMode,
            Players = players,
            PlayersByIndex = byIndex,
            PlayersByName = byName,
            DrawPile = cards, // should be empty because all cards are dealt
            CurrentPlayer = startPlayer
        };

        result.PlayCard(new(2, Suits.Clubs));

        return result;
    }

    private static ILookup<string, PlayingCard> Deal(Queue<PlayingCard> cards, string[] playerNames)
    {
        const int CardsPerHand = 12;

        List<(string PlayerName, PlayingCard Card)> result = [];

        for (int card = 0; card < CardsPerHand; card++)
        {
            foreach (var player in playerNames)
            {
                result.Add((player, cards.Dequeue()));
            }
        }

        return result.ToLookup(card => card.PlayerName, card => card.Card);
    }
}
