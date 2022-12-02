using BusinessLogicLayer.MenuBLS;
using CutOff.Models;
using CutOff.Repository.PartialHeaderMenu;
using EntityLayer.MenuHelperELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CutOff.Controllers
{
    [SessionTimeout]
    public class CuttoffMasterController : Controller
    {

            private IPartialHeader _headerLayout;

            public ActionResult Index()
            {
                return View();
            }
            public PartialViewResult _PartialHeaderLayout()
            {               
                return PartialView();
            }
            public PartialViewResult _PartialMenu()
            {
                //BusinessLogicLayer.MasterPageBL bl = new BusinessLogicLayer.MasterPageBL();
                MenuBL menuBL = new MenuBL();
                List<MenuListModel> ModelLsit = menuBL.GetUserMenuListByGroup();
                return PartialView(ModelLsit);
            }
            public void logout()
            {
                string logoutkey = ConfigurationManager.AppSettings["LogOutURL"].ToString();
                // Response.Redirect(logoutkey, true);
                Response.Redirect("/ERP/OMS/SignOff.aspx", true);
                //Response.Redirect(string.Format("localhost:55364/prjLauncher/CCPLauncher.aspx?{0}", ToQueryString(post))); 
            }
        
	}
}