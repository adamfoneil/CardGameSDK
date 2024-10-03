using AppService.Entities.Conventions;

namespace AppService.Entities;

public class GameInstance : BaseTable
{
	public SupportedGames Game { get; set; }
	public string? State { get; set; } = default!;
	public DateTime? FinishedAtUtc { get; set; }
	public bool IsFinished => FinishedAtUtc.HasValue;

	public ICollection<Player> Players { get; set; } = [];
}
