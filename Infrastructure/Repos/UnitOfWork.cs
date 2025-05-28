using Infrastructure.Interfaces;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TransportContext _context;
		private Hashtable _repos;
		private IBusRepository _busRepository;

		public UnitOfWork(TransportContext context, IBusRepository busRepository)
		{
			_context = context;
			_busRepository = busRepository;
		}

		public IBusRepository BusRepository => _busRepository;

		public async Task<int> Complete()
			=> await _context.SaveChangesAsync();

		public void Dispose()
			=> _context.Dispose();
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			if (_repos == null)
				_repos = new Hashtable();
			var type = typeof(TEntity).Name;
			if (!_repos.ContainsKey(type))
			{
				// Handle specialized repositories
				if (typeof(TEntity) == typeof(Buses))
				{
					_repos.Add(type, _busRepository);
				}
				else
				{
					var repoType = typeof(GenericRepository<>);
					var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);
					_repos.Add(type, repoInstance);
				}
			}
			return (IGenericRepository<TEntity>)_repos[type];
		}
	}
}
