using Infrastucture.Entities;

namespace Infrastructure.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
		Task<int> Complete();
		IBusRepository BusRepository { get; }

	}
}