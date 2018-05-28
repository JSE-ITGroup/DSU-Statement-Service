using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace StatmentWarehouseService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          /*
            routes.MapRoute(
               name: "Default",
               url: "api/{controller}/{action}/{id}",
               defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional }
           );
           */

            // By default route the user to the Help area if accessing the base URI.
            routes.MapRoute(
                "Help Area",
                "",
                new { controller = "Help", action = "Index" }
            ).DataTokens = new RouteValueDictionary(new { area = "HelpPage" });
           
        }
    }
}