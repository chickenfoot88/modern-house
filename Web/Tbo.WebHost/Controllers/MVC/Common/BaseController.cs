using System.Web.Mvc;

namespace Tbo.WebHost.Controllers.MVC.Common
{
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    [Authorize]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Установка ControllerName у ViewBag при каждом запросе
        /// </summary>
        /// <param name="filterContext">контекст</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        }
    }
}