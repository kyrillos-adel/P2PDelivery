using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Domain.Entities;
using P2PDelivery.Infrastructure.Contexts;

namespace P2PDelivery.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var Item in entities)
            {
                Item.CreatedAt = DateTime.Now;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            SaveInclude(entity, nameof(entity.IsDeleted), nameof(entity.DeletedAt));
        }


        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var Item in entities)
            {
                Delete(Item);
            }
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id && !x.IsDeleted);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression= null)
        {
            var query = _dbSet.Where(x => !x.IsDeleted);
            if(expression!= null)
            {
                query = query.Where(expression);
            }
            
            return query;
        }

        public async Task<TEntity> GetByIDAsync(int id)
        {
            return await GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }


        // update 
        public void SaveInclude(TEntity entity, params string[] properties)
        {
            var local = _dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);

            EntityEntry entityEntry = null;
            if (local is null)
            {
                entityEntry = _appDbContext.Entry(entity);
            }
            else
            {
                entityEntry = _appDbContext.ChangeTracker
                    .Entries<TEntity>().FirstOrDefault(x => x.Entity.Id == entity.Id);
            }

            foreach (var property in entityEntry.Properties)
            {
                if (properties.Contains(property.Metadata.Name))
                {
                    property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name).GetValue(entity);
                    property.IsModified = true;
                }
                else if (property.Metadata.Name == nameof(entity.UpdatedAt))
                {
                    property.CurrentValue = DateTime.Now;
                    property.IsModified = true;
                }
                else if (property.Metadata.Name == nameof(entity.UpdatedBy))
                {
                    property.CurrentValue = 1111;
                    property.IsModified = true;
                }
            }
        }

        public  Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
