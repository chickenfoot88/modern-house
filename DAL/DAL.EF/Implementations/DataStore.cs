using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.DataAccess.Interfaces;

namespace DAL.EF.Implementations
{
    public class DataStore : IDataStore
    {
        internal ApplicationDbContext Context;

        public DataStore(ApplicationDbContext context)
        {
            Context = context;
        }

        public TEntity Get<TEntity>(long key) where TEntity : class, IEntity<long>
        {
            return Context.Set<TEntity>().FirstOrDefault(x => x.Id == key);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class, IEntity
        {
            return Context.Set<TEntity>().AsQueryable();
        }
        
        public IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty>> path) where TEntity : class, IEntity
        {
            return queryable.Include(path);
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public Task SaveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Context.Set<TEntity>().Add(entity);
            return Context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }

        public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Context.Set<TEntity>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Dispose()
        {
        }
    }
}
