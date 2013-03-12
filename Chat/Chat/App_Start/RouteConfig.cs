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
                url: "Chat/Room/{chatId}",
                defaults: new { controller = "Chat", action = "JoinRoom", chatId = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Chat",
                url: "{controller}/{action}/{chatId}",
                defaults: new { controller = "Chat", action = "List", chatId = UrlParameter.Optional }
                );
        }
    }
}