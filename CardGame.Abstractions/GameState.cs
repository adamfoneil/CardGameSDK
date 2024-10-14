using System.Text.Json.Serialization;

namespace CardGame.Abstractions;

public abstract class GameState<TCard>
{
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
	public abstract bool IsGameFinished { get; }

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

	protected Player<TCard> NextPlayer()
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		var index = PlayersByName[CurrentPlayer.Name].Index;

		index++;
		if (index > Players.Count) index = 1;

		return PlayersByIndex[index];
	}

	public record Play(string PlayerName, TCard Card);	
}
