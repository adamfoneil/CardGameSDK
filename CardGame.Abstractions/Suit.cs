using System.Diagnostics;

namespace CardGame.Abstractions;

[DebuggerDisplay("{Name}")]
public class Suit(string name, int rank)
{
	public string Name { get; init; } = name;
	/// <summary>
	/// ranks are intended for Poker, where you need the suit to act as a tie-breaker
	/// </summary>
	public int Rank { get; init; } = rank;

	public override bool Equals(object? obj) => obj is Suit s && Name.Equals(s.Name, StringComparison.OrdinalIgnoreCase);
	public override int GetHashCode() => Name.GetHashCode();

	public static implicit operator Suit(string name) => new(name, 0);
}

public static class ClassicSuits
{
	public static Suit Clubs => new("Clubs", 1);
	public static Suit Diamonds => new("Diamonds", 2);
	public static Suit Hearts => new("Hearts", 3);
	public static Suit Spades => new("Spades", 4);
	public static Suit[] All => [Clubs, Diamonds, Hearts, Spades];
}