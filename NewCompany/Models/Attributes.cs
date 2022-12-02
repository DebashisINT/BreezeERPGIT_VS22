using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewCompany.Models
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["userid"] == null)
            {
                HttpContext.Current.Session.Abandon();
                //filterContext.Result = new RedirectResult("/OMS/Login.aspx");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}