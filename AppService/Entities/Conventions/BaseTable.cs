namespace AppService.Entities.Conventions;

public abstract class BaseTable
{
	public int Id { get; set; }
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime? ModifiedAtUtc { get; set; }
}
