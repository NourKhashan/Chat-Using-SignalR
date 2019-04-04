using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FaceBookChat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{messageFrom}/{messageTo}",
                defaults: new { controller = "Users", action = "Login", messageFrom = UrlParameter.Optional, messageTo = UrlParameter.Optional }
            );
        }
    }
}
