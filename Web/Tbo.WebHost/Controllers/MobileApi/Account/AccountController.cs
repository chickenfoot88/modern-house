using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Identity.OAuth.Services;
using Domain.MobileApi.Interfaces;
using Domain.MobileApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.MobileApi.Account
{
    /// <summary>
    /// Аккаунт
    /// </summary>
    [RoutePrefix("api/mobile/account")]
    [Authorize(Roles = "Driver", Users = "")]
    public class AccountController : BaseApiController
    {
        private readonly IDriverUserService _driverUserService;

        /// <summary>
        /// Менеджер аутентификации
        /// </summary>
        public IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private ApplicationSignInManager _signInManager;
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public AccountController(IDriverUserService driverUserService)
        {
            _driverUserService = driverUserService;
        }


        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <returns></returns>
        [Route("current")]
        [HttpGet]
        [Attributes.ResponseType(typeof(DriverUserInfo))]
        public HttpResponseMessage Current()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Failure("Пользователь не авторизован", HttpStatusCode.Unauthorized);
;            }
            var id = User.Identity.GetUserId<long>();
            var user = _driverUserService.GetDriverUserInfo(id);
            return user == null
                ? Failure("Пользователь не найден", HttpStatusCode.NotFound)
                : Success(user);
        }



        /// <summary>
        /// авторизация
        /// </summary>
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Login(string login, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(login, password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Success();
                default:
                    return Failure("Неудачная попытка входа.");
            }
        }



        /// <summary>
        /// Выход
        /// </summary>
        [Route("logout")]
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Success();
        }

        /// <summary>
        /// Установить координаты
        /// </summary>
        [Route("position")]
        [HttpPost]
        public HttpResponseMessage SetPosition(decimal latitude, decimal longitude)
        {
            var id = User.Identity.GetUserId<long>();
            _driverUserService.UpdateCarPosition(id, latitude, longitude);
            return Success();
        }
    }
}
