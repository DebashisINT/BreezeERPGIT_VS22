using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.App_Start
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var fooCookie = filterContext.HttpContext.Request.Cookies["foo"];
            // TODO: do something with the foo cookie
        }
    }
}