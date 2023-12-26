using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Persistence.Contexts;
using System.Linq.Expressions;


namespace ProniaOnion.Persistence.Implementations.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbSet<T> _table;
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _table = context.Set<T>();
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public IQueryable<T> GetAll(bool isTracking = true, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table;

            query = _addIncludes(query,includes);

            if (ignoreQuery) query = query.IgnoreQueryFilters();
            return isTracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAllOrderBy(Expression<Func<T, object>> expressionOrder, int skip = 0, int take = 0, bool ignoreQuery=false,bool isDescending = false, bool isTracking = false, params string[] includes)
        {
            var query = _table.AsQueryable();

            if (isDescending) query = query.OrderBy(expressionOrder);
            else query = query.OrderByDescending(expressionOrder);


            if (skip != 0) query = query.Skip(skip);
            if (take != 0) query = query.Take(take);

            if (includes is not null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }
            if(ignoreQuery) query = query.IgnoreQueryFilters();
            return isTracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 0, bool ignoreQuery = false, bool isTracking = false, params string[] includes)
        {
            var query = _table.AsQueryable();

            if (expression is not null) query = query.Where(expression);

            if (skip != 0) query = query.Skip(skip);
            if (take != 0) query = query.Take(take);

            query = _addIncludes(query);

            if (ignoreQuery) query = query.IgnoreQueryFilters();
            return isTracking ? query : query.AsNoTracking(); ;
        }

        public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = true, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(expression);

            query = _addIncludes(query);

            if (!isTracking) query = query.AsNoTracking();
            if (ignoreQuery) query = query.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync();
        }

        
        public async Task<T> GetByIdAsync(int id, bool isTracking = true, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(x=>x.Id == id);
            
            query = _addIncludes(query,includes);//bele yazmisdin

            if(!isTracking) query = query.AsNoTracking();
            if(ignoreQuery) query = query.IgnoreQueryFilters();


            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<T,bool>> expression, bool ignoreQuery = false)
        {
            return ignoreQuery ? await _table.AnyAsync(expression) : await _table.IgnoreQueryFilters().AnyAsync(expression);
        }


        public void Update(T entity)
        {
            _table.Update(entity);
        }

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            
        }
        public void ReverseSoftDelete(T entity)
        {
            entity.IsDeleted = false;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<T> _addIncludes(IQueryable<T> query, params string[] includes)
        {
            if(includes is not null)
            {
                for(int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
               
            }
            return query;
        }

        
    }
}
