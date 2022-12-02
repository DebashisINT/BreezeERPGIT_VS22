using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERP.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public UserAuthorizeAttribute(string url = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if ((HttpContext.Current.Session["userid"] == null) || HttpContext.Current.Session["usergoup"] == null)
                {
                    HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + "/OMS" + url);
                }
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if ((HttpContext.Current.Session["userid"] == null) || HttpContext.Current.Session["usergoup"] == null)
            {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.RawUrl))
            {
                //filterContext.Result = new RedirectResult("/OMS/Login.aspx?rurl=" + filterContext.HttpContext.Request.RawUrl);
                filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                { "controller", "Common" },
                                { "action", "RedirectToLogin" },
                                {"rurl", filterContext.HttpContext.Request.RawUrl}
                            });
            }
            else
            {
                //filterContext.Result = new RedirectResult("/OMS/Login.aspx");
                filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                { "controller", "Common" },
                                { "action", "RedirectToLogin" }
                            });
            }
        }
    }
}