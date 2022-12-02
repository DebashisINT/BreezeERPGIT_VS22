using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CutOff
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CutOffLogin", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default2",
            //    url: "YearEnding/{controller}/{action}/{id}",
            //    defaults: new { controller = "YearEnding", action = "YearEndingStepOne", id = UrlParameter.Optional }
            //);


        }
    }
}

