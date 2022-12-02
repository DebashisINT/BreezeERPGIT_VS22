using BusinessLogicLayer.MenuBLS;
using Dashboard_React.Repositories.PartialHeaderMenu;
using EntityLayer.MenuHelperELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard_React.Controllers
{
    public class DashboardMasterController : Controller
    {
        private IPartialHeader _headerLayout;

        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _PartialHeaderLayout()
        {
            _headerLayout = new PartialHeader();

            DataSet _layout = new DataSet();
            string return_msg = string.Empty;
            try
            {


                if (Session["LastCompany"] == null || Session["LocalCurrency"] == null || Session["CompanyBigLogoUrl"] == null || Session["CompanySmallLogoUrl"] == null)
                {
                    ViewData["biglogo"] = "../assests/images/logo.png";
                    ViewData["minilogo"] = "../assests/images/logo-mini.png";

                    _layout = _headerLayout.ViewLayoutHeader(ref return_msg, Session["LastCompany"].ToString(), Session["userbranchID"].ToString(), Session["userid"].ToString());
                    if (return_msg == "true")
                    {
                        Session["LocalCurrency"] = Convert.ToString(_layout.Tables[0].Rows[0]["TradeCurrency"]);
                        Session["LastCompanyName"] = Convert.ToString(_layout.Tables[1].Rows[0]["cmp_Name"]) + "(" + Convert.ToString(_layout.Tables[2].Rows[0]["branch_description"]) + ")";
                        if (!string.IsNullOrEmpty(Convert.ToString(_layout.Tables[3].Rows[0]["cmp_bigLogo"])))
                        {
                            if (System.IO.File.Exists(Server.MapPath(Convert.ToString(_layout.Tables[3].Rows[0]["cmp_bigLogo"]))))
                            {

                                Session["CompanyBigLogoUrl"] = Convert.ToString(_layout.Tables[3].Rows[0]["cmp_bigLogo"]);
                                ViewData["biglogo"] = Convert.ToString(_layout.Tables[3].Rows[0]["cmp_bigLogo"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(_layout.Tables[3].Rows[0]["cmp_smallLogo"])))
                            {
                                if (System.IO.File.Exists(Server.MapPath(Convert.ToString(_layout.Tables[3].Rows[0]["cmp_smallLogo"]))))
                                {

                                    Session["CompanySmallLogoUrl"] = Convert.ToString(_layout.Tables[3].Rows[0]["cmp_smallLogo"]);
                                    ViewData["minilogo"] = Convert.ToString(_layout.Tables[3].Rows[0]["cmp_smallLogo"]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    ViewData["biglogo"] = Convert.ToString(Session["CompanyBigLogoUrl"]);
                    ViewData["minilogo"] = Convert.ToString(Session["CompanySmallLogoUrl"]);
                }
            }
            catch (Exception ex)
            {

            }
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