namespace CardGame.Abstractions;

public interface IRepository<TData>
{
	Task<TData> GetByIdAsync(int id);
	Task SaveAsync(TData data);
}
