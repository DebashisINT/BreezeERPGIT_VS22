using BusinessLogicLayer.MenuBLS;
using EntityLayer.MenuHelperELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceManagement.Controllers
{
    public class CommonController : Controller
    {
        public PartialViewResult _PartialMenu()
        {
            MenuBL menuBL = new MenuBL();
            List<MenuListModel> ModelLsit = menuBL.GetUserMenuListByGroup();
            return PartialView(ModelLsit);
        }

        public ActionResult RedirectToLogin(string rurl)
        {
            string url = "/ServiceManagement/Login.aspx";

            if (!string.IsNullOrWhiteSpace(rurl))
            {
                url += "?rurl=" + rurl;
            }

            return Redirect(url);
        }
	}
}