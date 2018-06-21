using System;
using Core.DataAccess.Interfaces;
using Core.Identity.Entities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Identity.Services
{
    public class UserStore : 
        IUserRoleStore<ApplicationUser, long>, 
        IUserPasswordStore<ApplicationUser, long>, 
        IUserClaimStore<ApplicationUser, long>,
        IUserLockoutStore<ApplicationUser, long>
    {
        public IDataStore DataStore { get; set; }

        public UserStore(IDataStore dataStore)
        {
            this.DataStore = dataStore;
        }

        public async Task CreateAsync(ApplicationUser user)
        {
            await DataStore.SaveAsync(user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            await DataStore.DeleteAsync(user);
        }

        public void Dispose()
        {
        }

        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            return Task.FromResult(DataStore.Get<ApplicationUser>(userId));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(DataStore.GetAll<ApplicationUser>().FirstOrDefault(x => x.Login == userName));
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await DataStore.SaveChangesAsync();
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            var exists = DataStore.GetAll<UserRole>()
                .Any(x => x.User.Id == user.Id && x.RoleId == roleName);

            if (exists)
            {
                return;
            }

            var userRole = new UserRole
            {
                User = user,
                RoleId = roleName
            };
            await DataStore.SaveAsync(userRole);
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            var existingRole = DataStore.GetAll<UserRole>()
                .FirstOrDefault(x => x.User.Id == user.Id && x.RoleId == roleName);

            if (existingRole == null)
            {
                return;
            }

            await DataStore.DeleteAsync(existingRole);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            IList<string> roles = DataStore.GetAll<UserRole>()
                .Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId)
                .ToList();

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            var exists = DataStore.GetAll<UserRole>()
                .Any(x => x.User.Id == user.Id && x.RoleId == roleName);

            return Task.FromResult(exists);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            IList<Claim> claims = DataStore.GetAll<UserClaim>()
                .Select(x => new { x.ClaimType, x.ClaimValue})
                .ToList()
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();

            return Task.FromResult(claims);
        }

        public async Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            var existingClaim = DataStore.GetAll<UserClaim>()
                .FirstOrDefault(x => x.User.Id == user.Id && x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            if (existingClaim == null)
            {
                return;
            }

            await DataStore.DeleteAsync(existingClaim);
        }

        public async Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            var existingClaim = DataStore.GetAll<UserClaim>()
                .FirstOrDefault(x => x.User.Id == user.Id && x.ClaimType == claim.Issuer && x.ClaimValue == claim.Value);

            if (existingClaim != null)
            {
                return;
            }

            var newClaim = new UserClaim
            {
                User = user,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };

            await DataStore.SaveAsync(newClaim);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEndDate.Value);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Locked);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.Locked = enabled;
            return Task.FromResult(0);
        }
    }
}
