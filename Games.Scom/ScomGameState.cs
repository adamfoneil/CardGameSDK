using CardGame.Abstractions;

namespace Games.Scom;

public class Piece
{
	public string Name { get; set; } = default!;
	/// <summary>
	/// how far can I move per turn?
	/// </summary>
	public int Range { get; set; }
	/// <summary>
	/// subtract from enemy attack roll
	/// </summary>
	public int Armor { get; set; }
	/// <summary>
	/// add to attack roll
	/// </summary>
	public int Attack { get; set; }	
}

public class ScomGameState : GameState<Piece>
{
	public const int BoardSize = 18;
	public const int MovesPerTurn = 5;

	/// <summary>
	/// player name + piece name -> location
	/// </summary>
	public Dictionary<(string, string), (int X, int Y)> Locations { get; set; } = [];

	public override bool IsRoundFinished => throw new NotImplementedException();

	public override Dictionary<string, int> Score => throw new NotImplementedException();

	public override void AutoPlay()
	{
		throw new NotImplementedException();
	}

	public override (bool IsValid, string? Message) ValidatePlay(string playerName, Piece card)
	{
		throw new NotImplementedException();
	}

	protected override int NextPlayerIndex(int currentIndex)
	{
		throw new NotImplementedException();
	}

	protected override void OnPlayCard(string playerName, Piece card)
	{
		throw new NotImplementedException();
	}
}
