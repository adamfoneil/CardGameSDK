using CardGame.Abstractions;

namespace Games.Hearts;

public class HeartsGameFactory : GameFactory<HeartsGameState, PlayingCard>
{
    public override uint MinPlayers => 4;

    public override uint MaxPlayers => 4;

    public override IEnumerable<PlayingCard> Deck => PlayingCard.ClassicDeck;

    public override string Name => "Hearts (4p)";

    protected override HeartsGameState CreateGameState(
        bool devMode, 
        HashSet<Player<PlayingCard>> players, 
        Dictionary<int, Player<PlayingCard>> byIndex, 
        Dictionary<string, Player<PlayingCard>> byName,
        Queue<PlayingCard> drawPile)
    {
        if (players.Count != 4) throw new Exception("Must have 4 players");

		var startPlayer = players.Single(p => p.Hand.Contains(new PlayingCard(2, ClassicSuits.Clubs)));

		HeartsGameState result = new()
        {
            IsDevMode = devMode,
            Players = players,
            PlayersByIndex = byIndex,
            PlayersByName = byName,            
            CurrentPlayer = startPlayer
        };

        result.PlayCard(new(2, ClassicSuits.Clubs));

        return result;
    }

    protected override ILookup<string, PlayingCard> Deal(Queue<PlayingCard> cards, string[] playerNames)
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
