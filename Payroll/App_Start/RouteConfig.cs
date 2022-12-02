using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Payroll
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();
            //routes.MapRoute(
            //name: "Action",
            //url: "payrollTableFormula/Index",
            //defaults: new { controller = "payrollTableFormula", action = "Index" }
            // );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "payLogin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
