
namespace CardGame.Abstractions.Games;

public class Hearts : GameDefinition<PlayingCard>
{
    public override uint MinPlayers => 4;

    public override uint MaxPlayers => 4;

    public override IEnumerable<PlayingCard> Deck => PlayingCard.StandardDeck;

    public override uint CardsPerHand => 12;
}
