using System.Text.Json.Serialization;

namespace CardGame.Abstractions;

public abstract class GameState<TCard>
{
	public HashSet<Player<TCard>> Players { get; init; } = [];
	[JsonIgnore]
	public Dictionary<int, Player<TCard>> PlayersByIndex { get; set; } = [];
	[JsonIgnore]
	public Dictionary<string, Player<TCard>> PlayersByName { get; set; } = [];
	/// <summary>
	/// allow logged on player to impersonate all players
	/// </summary>
	public bool IsDevMode { get; init; }
	/// <summary>
	/// whose turn is it?
	/// </summary>
	public Player<TCard>? CurrentPlayer { get; set; }
	public Queue<TCard> DrawPile { get; set; } = [];
	[JsonIgnore]
	public Func<Task>? OnStateChanged { get; set; }
	public abstract bool IsFinished { get; }

	public abstract (bool IsValid, string? Message) ValidatePlay(string playerName, TCard card);

	public void PlayCard(TCard card)
	{
		ArgumentNullException.ThrowIfNull(CurrentPlayer, nameof(CurrentPlayer));

		var (valid, message) = ValidatePlay(CurrentPlayer.Name, card);
		if (!valid) throw new Exception(message);

		OnPlayCard(card);
	}

	protected abstract void OnPlayCard(TCard card);

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
