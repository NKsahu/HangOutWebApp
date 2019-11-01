using System.Web.Routing;
using System.Web.Mvc;

namespace HangOut.Models.Common
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["UserInfo"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "vw_HG_UsersDetails", action = "vw_HG_UsersDetails" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}