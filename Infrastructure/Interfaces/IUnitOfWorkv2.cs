
using Infrastucture.Entities;

namespace Infrastructure.Interfaces
{
	public interface IUnitOfWorkv2 : IDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
		Task<int> Complete();
		IBusv2Repository Busv2Repository { get; }
	}
}