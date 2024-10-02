using CardGame.Abstractions;

namespace Games.Hearts;

public class HeartsGameFactory : GameFactory<HeartsGameState, PlayingCard>
{
    public override uint MinPlayers => 4;

    public override uint MaxPlayers => 4;

    protected override uint CardsPerHand => 12;

	public override IEnumerable<PlayingCard> Deck => PlayingCard.ClassicDeck;

    public override string Name => "Hearts (4p)";

    protected override HeartsGameState CreateGameState(
        bool devMode, 
        HashSet<Player<PlayingCard>> players, 
        Dictionary<int, Player<PlayingCard>> byIndex, 
        Dictionary<string, Player<PlayingCard>> byName,
        Queue<PlayingCard> drawPile)
    {       
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
}
