using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess.Interfaces
{
    public interface IDataStore: IDisposable
    {
        TEntity Get<TEntity>(long key) where TEntity : class, IEntity<long>;
        
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class, IEntity;

        IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty>> path) where TEntity : class, IEntity;

        void Save<TEntity>(TEntity entity) where TEntity : class, IEntity;

        Task SaveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void SaveChanges();

        Task SaveChangesAsync();

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

        bool HasChanges();
    }
}
