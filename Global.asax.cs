using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HangOut.Models.Common;
using System.Web.Mvc;
using System.Web.Routing;

namespace HangOut
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RamRefresh();
        }
        public void RamRefresh()
        {
            Cart.List = new System.Collections.Generic.List<Cart>();
        }
    }
}
