namespace CardGame.Abstractions;

public abstract class Card
{
    public required string Suit { get; init; }
    public int Rank { get; init; } 
    public string? Name { get; init; }
    public int? PointValue { get; init; }
}
