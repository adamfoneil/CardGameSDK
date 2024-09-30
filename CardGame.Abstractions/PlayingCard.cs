namespace CardGame.Abstractions;

public class Suit(string name, int rank)
{
    public string Name { get; init; } = name;
    public int Rank { get; init; } = rank;

	public override bool Equals(object? obj) => obj is Suit s && Name.Equals(s.Name, StringComparison.OrdinalIgnoreCase);	
    public override int GetHashCode() => Name.GetHashCode();
}

public static class Suits
{
    public static Suit Clubs => new("Clubs", 1);
    public static Suit Diamonds => new("Diamonds", 2);
    public static Suit Hearts => new("Hearts", 3);
    public static Suit Spades => new("Spades", 4);
    public static Suit[] All => [Clubs, Diamonds, Hearts, Spades];
}

public class PlayingCard
{
    public required Suit Suit { get; init; }
    public int Rank { get; init; }
    public string? Name { get; init; }

    public override string ToString() => $"{Name ?? Rank.ToString()} {Suit.Name}";    

    public static IEnumerable<PlayingCard> StandardDeck =>
        Suits.All.SelectMany(s => 
            Enumerable.Range(2, 8).Select(val => new PlayingCard
            {
                Suit = s,
                Rank = val
            }).Concat(
            [
                new PlayingCard() { Suit = s, Name = "Jack", Rank = 11 },
                new PlayingCard() { Suit = s, Name = "Queen", Rank = 12 },
                new PlayingCard() { Suit = s, Name = "King", Rank = 13 },
                new PlayingCard() { Suit = s, Name = "Ace", Rank = 14 }
            ]));
}
