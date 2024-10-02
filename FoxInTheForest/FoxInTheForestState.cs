using CardGame.Abstractions;

namespace FoxInTheForest;

public class FoxInTheForestState : GameState<PlayingCard>
{
	public override bool IsFinished => throw new NotImplementedException();

	public required PlayingCard DecreeCard { get; init; }

	public override Dictionary<string, int> Score => throw new NotImplementedException();

	public override void AutoPlay()
	{
		throw new NotImplementedException();
	}

	protected override void OnPlayCard(PlayingCard card)
	{
		throw new NotImplementedException();
	}

	public override (bool IsValid, string? Message) ValidatePlay(string playerName, PlayingCard card)
	{
		throw new NotImplementedException();
	}
}
