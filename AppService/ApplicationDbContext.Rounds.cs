using CardGame.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AppService;

public partial class ApplicationDbContext
{
	public async Task<bool> PlayNextRoundAsync<TState, TCard>(
		int gameInstanceId, string nextRoundState, 
		string priorRoundScore, 
		GameFactory<TState, TCard> gameFactory) where TState : notnull
	{
		var gameInstance = await GameInstances.FindAsync(gameInstanceId) ?? throw new Exception("game not found");
		gameInstance.Round++;

		Rounds.Add(new()
		{
			GameInstanceId = gameInstanceId,
			State = gameInstance.State!,
			Number = gameInstance.Round,
			Score = priorRoundScore
		});		

		gameInstance.State = nextRoundState;
		await SaveChangesAsync();

		var allScores = await Rounds.Where(r => r.GameInstanceId == gameInstanceId).Select(r => r.Score).ToArrayAsync();
		var (finished, winner, finalScore) = gameFactory.IsFinished(allScores);
		if (finished)
		{
			gameInstance.State = null!;
			gameInstance.Score = finalScore;
			gameInstance.FinishedAtUtc = DateTime.UtcNow;
			await SaveChangesAsync();
			return false;
		}

		return true;
	}
}
