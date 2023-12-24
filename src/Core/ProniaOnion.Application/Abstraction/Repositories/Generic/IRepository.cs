using ProniaOnion.Domain.Entities;
using System.Linq.Expressions;

namespace ProniaOnion.Application.Abstraction.Repositories
{
    public interface IRepository<T> where T : BaseEntity,new()
    {
        IQueryable<T> GetAll(bool isTracking = true,bool ignoreQuery=false,params string[] incldues);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 0, bool ignoreQuery=false, bool isTracking = false, params string[] includes);
        IQueryable<T> GetAllOrderBy(Expression<Func<T, object>> expressionOrder, int skip = 0, int take = 0, bool ignoreQuery=false,bool isDescending = false, bool isTracking = false, params string[] includes);
        Task<T> GetByIdAsync(int id,bool isTracking = true, bool ignoreQuery = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T,bool>> expression, bool isTracking = true, bool ignoreQuery = false, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T,bool>> expression,bool ignoreQuery = false);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseSoftDelete(T entity);
        Task SaveChangesAsync();
        
    }
}
