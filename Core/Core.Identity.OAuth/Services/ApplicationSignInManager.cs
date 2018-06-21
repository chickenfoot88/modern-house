using System.Threading.Tasks;
using Core.Identity.Entities;
using Core.Identity.Services;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Core.Identity.OAuth.Services
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, long>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (this.UserManager == null)
                return SignInStatus.Failure;
            ApplicationUser user = await this.UserManager.FindByNameAsync(userName);
            if ((object)user == null)
                return SignInStatus.Failure;
            if (await this.UserManager.IsLockedOutAsync(user.Id))
                return SignInStatus.LockedOut;
            if (await this.UserManager.CheckPasswordAsync(user, password))
            {
                await this.UserManager.ResetAccessFailedCountAsync(user.Id);
                await this.SignInAsync(user, isPersistent, false);
                return SignInStatus.Success;
            }
            if (shouldLockout)
            {
                await this.UserManager.AccessFailedAsync(user.Id);
                if (await this.UserManager.IsLockedOutAsync(user.Id))
                    return SignInStatus.LockedOut;
            }
            return SignInStatus.Failure;
        }
    }
}
