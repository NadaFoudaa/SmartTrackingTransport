using Core.Entities;
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
	public class UnitOfWorkv2 : IUnitOfWorkv2
	{
		private readonly TransportContext _context;
		private Hashtable _repos;
		private IBusv2Repository _busRepository;

		public UnitOfWorkv2(TransportContext context, IBusv2Repository busv2Repository)
		{
			_context = context;
			_busRepository = busv2Repository;
		}

		public IBusv2Repository Busv2Repository => _busRepository;

		public async Task<int> Complete()
			=> await _context.SaveChangesAsync();

		public void Dispose()
			=> _context.Dispose();
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
		{
			if (_repos == null)
				_repos = new Hashtable();
			var type = typeof(TEntity).Name;
			if (!_repos.ContainsKey(type))
			{
				// Handle specialized repositories
				if (typeof(TEntity) == typeof(Bus))
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
