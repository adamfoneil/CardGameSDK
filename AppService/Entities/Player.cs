using AppService.Entities.Conventions;

namespace AppService.Entities;

public class Player : BaseTable
{
	public int GameInstanceId { get; set; }
	public string UserId { get; set; } = default!;

	public GameInstance? Game { get; set; }
	public ApplicationUser? User { get; set; }
}
