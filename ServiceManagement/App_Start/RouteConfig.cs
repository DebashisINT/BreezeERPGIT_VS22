using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ServiceManagement
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Rev Sanchita
            routes.MapRoute(
                name: "CustomRoute",
                url: "Short/{id}",
                defaults: new { controller = "Short", action = "Index", id = UrlParameter.Optional }
            );
            // End of Rev Sanchita


            routes.MapRoute(
             name: "Default1",
             url: "{controller}/{action}/{Company_Code}",
             defaults: new { controller = "Login", action = "login", Company_Code = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}

