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
}
