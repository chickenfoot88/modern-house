using System;
using Core.Identity.Entities;
using Core.Identity.OAuth.Providers;
using Core.Identity.OAuth.Services;
using Core.Identity.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleInjector;

namespace Core.Identity.OAuth.OWIN.Startup
{
    public static class AuthCofig
    {
        public static void ConfigureAuth(IAppBuilder app, Container container)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(() => container.GetInstance<ApplicationUserManager>());
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, long>(
                        validateInterval: TimeSpan.FromMinutes(1440),
                        regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        getUserIdCallback: identity => identity.GetUserId<long>())
                }
            });

            //var oAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/token"),
            //    Provider = new ApplicationOAuthProvider("self", container),
            //    AuthorizeEndpointPath = new PathString("/api/account/externalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    AllowInsecureHttp = true
            //};

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            //app.UseOAuthBearerTokens(oAuthOptions);
        }
    }
}
