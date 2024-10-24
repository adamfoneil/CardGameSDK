using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace CardGame.Abstractions;

public abstract class GameState<TCard>
{
	[JsonIgnore]
	public ILogger<GameState<TCard>>? Logger { get; set; }

	/// <summary>
	/// for logging purposes during a game
	/// </summary>
	[JsonIgnore]
	public int? GameInstanceId { get; set; }

	public HashSet<Player<TCard>> Players { get; init; } = [];
	[JsonIgnore]
	public Dictionary<int, Player<TCard>> PlayersByIndex => Players.ToDictionary(player => player.Index);
	[JsonIgnore]
	public Dictionary<string, Player<TCard>> PlayersByName => Players.ToDictionary(player => player.Name);
	/// <summary>
	/// allow logged on player to impersonate all players
	/// </summary>
	public bool IsTestMode { get; init; }
	/// <summary>
	/// whose turn is it?
	/// </summary>
	public Player<TCard>? CurrentPlayer { get; set; }
	public Queue<TCard> DrawPile { get; set; } = [];
	public abstract bool IsRoundFinished { get; }

	public abstract (bool IsValid, string? Message) ValidatePlay(string playerName, TCard card);

	public void PlayCard(string playerName, TCard card)
	{
		var (valid, message) = ValidatePlay(playerName, card);
		if (!valid) throw new Exception(message);

		OnPlayCard(playerName, card);
	}

	protected abstract void OnPlayCard(string playerName, TCard card);

	/// <summary>
	/// for testing purposes, should generate a valid play so that state can be recorded, validated
	/// </summary>
	public abstract void AutoPlay();

	public abstract Dictionary<string, int> Score { get; }

	protected abstract int NextPlayerIndex(int currentIndex);

	protected Player<TCard> NextPlayer()
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		var index = PlayersByName[CurrentPlayer.Name].Index;

		Log(LogLevel.Information, "Current player is {CurrentPlayer}, index {index}", CurrentPlayer.Name, index);

		index = NextPlayerIndex(index);

		Log(LogLevel.Information, "Next player is {index}, {name}", index, PlayersByIndex[index].Name);

		return PlayersByIndex[index];
	}

	protected void Log(LogLevel level, string messageTemplate, params object?[] parameters)
	{
		if (Logger == null) return;
		using var scope = (GameInstanceId.HasValue) ? Logger.BeginScope("GameInstance {GameInstanceId}", GameInstanceId) : default;
		Logger.Log(level, messageTemplate, parameters);
	}

	public record Play(string PlayerName, TCard Card);
}
