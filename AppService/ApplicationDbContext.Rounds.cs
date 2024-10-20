namespace AppService;

public partial class ApplicationDbContext
{
	public async Task CompleteRoundAsync(int gameInstanceId, string newRoundState)
	{
		var gameInstance = await GameInstances.FindAsync(gameInstanceId) ?? throw new Exception("game not found");
		gameInstance.Round++;

		Rounds.Add(new()
		{
			GameInstanceId = gameInstanceId,
			State = gameInstance.State,
			Number = gameInstance.Round
		});

		gameInstance.State = newRoundState;
		await SaveChangesAsync();
	}

	public async Task CompleteGameAsync(int gameInstanceId, string score)
	{
		var gameInstance = await GameInstances.FindAsync(gameInstanceId) ?? throw new Exception("game not found");

		gameInstance.Score = score;
		gameInstance.FinishedAtUtc = DateTime.UtcNow;
		await SaveChangesAsync();
	}
}
