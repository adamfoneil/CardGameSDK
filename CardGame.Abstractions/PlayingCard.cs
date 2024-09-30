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

public static class NamedRanks
{
    public static int Jack => 11;
    public static int Queen => 12;
    public static int King => 13;
    public static int Ace => 14;
}

public class PlayingCard(int rank, Suit suit)
{
    public Suit Suit { get; init; } = suit;
    public int Rank { get; init; } = rank;
    public string? Name { get; init; }

    public override string ToString() => $"{Name ?? Rank.ToString()} {Suit.Name}";

	public override bool Equals(object? obj) => obj is PlayingCard c && Suit.Equals(c.Suit) && Rank == c.Rank;
	public override int GetHashCode() => HashCode.Combine(Suit, Rank);

	public static IEnumerable<PlayingCard> StandardDeck =>
        Suits.All.SelectMany(s => 
            Enumerable.Range(2, 8).Select(val => new PlayingCard(val, s)).Concat(
            [
                new PlayingCard(NamedRanks.Jack, s) { Name = nameof(NamedRanks.Jack) },
                new PlayingCard(NamedRanks.Queen, s) { Name = nameof(NamedRanks.Queen) },
                new PlayingCard(NamedRanks.King, s) { Name = nameof(NamedRanks.King) },
                new PlayingCard(NamedRanks.Ace, s) { Name = nameof(NamedRanks.Ace) }
            ]));
}
