using System.Diagnostics;

namespace CardGame.Abstractions;

[DebuggerDisplay("{Name}")]
public class Player<TCard>
{
	public string Name { get; init; } = default!;	
	public HashSet<TCard> Hand { get; init; } = [];
	public int Index { get; init; }

	public override bool Equals(object? obj) =>
		(obj is Player<TCard> player) && Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => Name.GetHashCode();
}
