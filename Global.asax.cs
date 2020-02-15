
using System.Web.Http;
using HangOut.Models.Common;
using System.Web.Mvc;
using System.Web.Routing;
using HangOut.Models.DynamicList;
using HangOut.Models;

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
            OrderType.List = new OrderType().OrgTypeList();
            OrgType.List = new OrgType().ListOrgTypeList();
            PymentPageOpen.ListPytmPgOpen = new System.Collections.Generic.List<PymentPageOpen>();
            JoinCls.List = new JoinCls().ListProductType();
            DiscntCharge.ListDiscntChrge = new System.Collections.Generic.List<OrdDiscntChrge>();
        }
    }
}
