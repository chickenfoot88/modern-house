using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Interfaces;
using Core.Enums;
using Core.Identity.Entities;

namespace Core.Identity.Services
{
    /// <summary>
    /// Сервис работы с разрешениями
    /// </summary>
    public class PermissionService
    {
        private readonly IDataStore _dataStore;

        public PermissionService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Получение разрешений роли
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<List<Permission>> GetRolePermissions(string[] roles)
        {
            var permissions = _dataStore.GetAll<RolePermission>()
                .Where(x => roles.Contains(x.RoleId))
                .Select(x => x.Permission)
                .Distinct()
                .ToList();

            return Task.FromResult(permissions);
        }
    }
}
