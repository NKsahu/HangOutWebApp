﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HangOut
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Inventory",
                url: "Inventory/{controller}/{action}/{id}",
                defaults: new { controller = "vw_HG_UsersDetails", action = "vw_HG_UsersDetails", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "vw_HG_UsersDetails", action = "vw_HG_UsersDetails", id = UrlParameter.Optional }
            );
        }
    }
}
