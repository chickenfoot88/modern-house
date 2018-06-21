using System.Linq;
using Core.DataAccess.Interfaces;

namespace Core.DataAccess.Extensions
{
    public static class DataStoreExtensions
    {
        public static T FindById<T>(this IDataStore dataStore, string id) where T : class, IEntity<long>
        {
            long longId;
            if (long.TryParse(id, out longId))
            {
                return dataStore.FindById<T>(longId);
            }
            return null;
        }

        public static T FindById<T>(this IDataStore dataStore, long id) where T:class,IEntity<long>
        {
            return dataStore.GetAll<T>().FirstOrDefault(x => x.Id == id);
        }
    }
}