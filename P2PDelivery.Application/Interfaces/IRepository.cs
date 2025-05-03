using System.Linq.Expressions;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        // Retriving 
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression=null);
        //IQueryable<TEntity> GetAll(Specification<TEntity> specification=null);
        Task<TEntity> GetByIDAsync(int id);

        // Add
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);


        // Update 
        void SaveInclude(TEntity entity, params string[] properties);

        //Delete
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
       


        // IsExist
        Task<bool> IsExistAsync(int id);

        // SaveChanges
        Task SaveChangesAsync();
        Task DeleteAsync(TEntity entity);
    }
}
