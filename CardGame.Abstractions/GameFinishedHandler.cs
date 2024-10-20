namespace CardGame.Abstractions;

public delegate GameFinishedResult GameFinishedHandler(IEnumerable<string> scores);

public class GameFinishedResult
{
	public bool IsFinished { get; init; }
	public string? Winner { get; init; }
	public string? FinalScore { get; init; }
}