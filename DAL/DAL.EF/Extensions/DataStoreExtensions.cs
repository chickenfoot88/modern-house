using System;
using System.Data;
using Core.DataAccess.Interfaces;
using DAL.EF.Implementations;

namespace DAL.EF.Extensions
{
    public static class DataStoreExtensions
    {
        public static void InTransaction(this IDataStore dataStore, Action<IDataStore> action)
        {
            var dataStoreImpl = dataStore as DataStore;
            if (dataStoreImpl == null)
            {
                throw new NotImplementedException();
            }

            var transaction = dataStoreImpl.Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                action(dataStoreImpl);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
