using Microsoft.EntityFrameworkCore;
using Infrastucture.DbContexts;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using System.Linq.Expressions;


namespace Infrastructure.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TransportContext _transportContext;

        public GenericRepository(TransportContext transportContext)
        {
            _transportContext = transportContext;
        }
        public async Task Add(T entity)

           => await _transportContext.Set<T>().AddAsync(entity);


        public void Delete(T entity)
            => _transportContext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _transportContext.Set<T>().ToListAsync();


        public async Task<T> GetByIdAsync(int? id)
            => await _transportContext.Set<T>().FindAsync(id);

        public void Update(T entity)
            => _transportContext.Set<T>().Update(entity);
        public async Task<IReadOnlyList<T>> FindAllAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _transportContext.Set<T>().Where(predicate);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }
        // New method: Get all with includes
        public async Task<IReadOnlyList<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _transportContext.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        // New method: Find all with predicate and includes
        public async Task<IReadOnlyList<T>> FindAllIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _transportContext.Set<T>().Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        // New method: Get first or default with predicate and includes
        public async Task<T?> GetFirstOrDefaultIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _transportContext.Set<T>().Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _transportContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public IQueryable<T> GetAll()
        {
            return _transportContext.Set<T>();
        }
        public IQueryable<T> GetQueryable()
        {
            return _transportContext.Set<T>().AsQueryable();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _transportContext.Set<T>().AddRangeAsync(entities); 
        }

    }
}