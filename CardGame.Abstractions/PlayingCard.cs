namespace CardGame.Abstractions;

public static class ClassicNamedRanks
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

	public string DisplayRank => Name ?? Rank.ToString();
	public override string ToString() => $"{DisplayRank} {Suit.Name}";

	public override bool Equals(object? obj) => obj is PlayingCard c && Suit.Equals(c.Suit) && Rank == c.Rank;
	public override int GetHashCode() => HashCode.Combine(Suit, Rank);

	public static IEnumerable<PlayingCard> ClassicDeck =>
		ClassicSuits.All.SelectMany(s =>
			Enumerable.Range(2, 8).Select(val => new PlayingCard(val, s)).Concat(
			[
				new PlayingCard(ClassicNamedRanks.Jack, s) { Name = nameof(ClassicNamedRanks.Jack) },
				new PlayingCard(ClassicNamedRanks.Queen, s) { Name = nameof(ClassicNamedRanks.Queen) },
				new PlayingCard(ClassicNamedRanks.King, s) { Name = nameof(ClassicNamedRanks.King) },
				new PlayingCard(ClassicNamedRanks.Ace, s) { Name = nameof(ClassicNamedRanks.Ace) }
			]));
}
