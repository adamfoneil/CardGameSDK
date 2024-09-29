namespace CardGame.Abstractions;

public record Suit(string Name, int Rank);

public static class SuitHelper
{
    public static IEnumerable<Suit> Standard =>
    [
        new("Clubs", 1),
        new("Diamonds", 2),
        new("Hearts", 3),
        new("Spades", 4)
    ];
}

public class PlayingCard
{
    public required Suit Suit { get; init; }
    public int Rank { get; init; }
    public string? Name { get; init; }

    public override string ToString() => $"{Name ?? Rank.ToString()} {Suit.Name}";
    

    public static IEnumerable<PlayingCard> StandardDeck =>
        SuitHelper.Standard.SelectMany(s => 
            Enumerable.Range(2, 8).Select(val => new PlayingCard
            {
                Suit = s,
                Rank = val
            }).Concat(
            [
                new PlayingCard() { Suit = s, Name = "Jack", Rank = 11},
                new PlayingCard() { Suit = s, Name = "Queen", Rank = 12 },
                new PlayingCard() { Suit = s, Name = "King", Rank = 13 },
                new PlayingCard() { Suit = s, Name = "Ace", Rank = 14 }
            ]));
}
