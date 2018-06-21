using System.Linq;
using System.Threading.Tasks;
using Core.Identity.Entities;
using Microsoft.AspNet.Identity;

namespace Core.Identity.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser, long>
    {
        public ApplicationUserManager(UserStore store) : base(store)
        {
            UserValidator =
                new Microsoft.AspNet.Identity.UserValidator<ApplicationUser, long>(this)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = false
                };
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            var  user = (Store as UserStore)?.DataStore.GetAll<ApplicationUser>()
                .FirstOrDefault(x => x.Login == userName);
            if (user == null)
                return null;
            return await CheckPasswordAsync(user, password) ? user : null;
        }
    }
}
