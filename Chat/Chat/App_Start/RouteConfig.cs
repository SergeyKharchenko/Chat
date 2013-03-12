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
                name: "Room",
                url: "Chat/Room/{roomId}",
                defaults: new { controller = "Room", action = "JoinRoom", roomId = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Chat",
                url: "Chat/{action}/{roomId}",
                defaults: new { controller = "Room", action = "List", roomId = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{roomId}",
                defaults: new { controller = "Room", action = "List", roomId = UrlParameter.Optional }
                );
        }
    }
}