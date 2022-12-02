using System.Web.Http;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService
{
    public class EmployeeSelfServiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EmployeeSelfService";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute("EmployeeSelfService_WebApiRoute",
                          "EmployeeSelfService/Api/{controller}/{action}/{id}",
                          new { id = RouteParameter.Optional });
            context.MapRoute(
                "EmployeeSelfService_default",
                "EmployeeSelfService/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
