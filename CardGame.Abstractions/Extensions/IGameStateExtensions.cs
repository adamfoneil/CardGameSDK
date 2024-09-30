namespace CardGame.Abstractions.Extensions;

internal static class IGameStateExtensions
{
	internal static Player<TCard> NextPlayer<TCard>(this IGameState<TCard> gameState)
	{
		ArgumentNullException.ThrowIfNull(gameState.CurrentPlayer, nameof(gameState.CurrentPlayer));

		var index = gameState.PlayersByName[gameState.CurrentPlayer.Name].Index;
		
		index++;
		if (index > gameState.Players.Count) index = 0;

		return gameState.PlayersByIndex[index];
	}
}
