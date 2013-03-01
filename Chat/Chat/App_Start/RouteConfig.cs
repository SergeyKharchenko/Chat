using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Chat",
                url: "chat/{action}",
                defaults: new { controller = "Chat", action = "List" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{login}",
                defaults: new {controller = "Account", action = "Index", login = UrlParameter.Optional}
                );
        }
    }
}