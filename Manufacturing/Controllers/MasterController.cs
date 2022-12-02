using BusinessLogicLayer.MenuBLS;
using EntityLayer.MenuHelperELS;
using Manufacturing.Models;
using Manufacturing.Repostiory;
using Manufacturing.Repostiory.PartialHeaderMenu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Manufacturing.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master

        private IMaster obj;
        private IPartialHeaderLayout _headerLayout;
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn("UserName");
            dt.Columns.Add(col1);
            DataRow dr = dt.NewRow();
            dr["UserName"] = "arindam";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["UserName"] = "avishek";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["UserName"] = "rahul";
            
            dt.Rows.Add(dr2);
            ViewData["Products"] = dt;
            return View();
        }
        public PartialViewResult _customerGrid(string FilterData)
        {
            obj = new Master();
            var data = obj.GetCustomerQueryable(FilterData);
           // CustomerEF customerEF = new CustomerEF();
            return PartialView(data);
        }

        public PartialViewResult _PartialHeaderLayout()
        {
            _headerLayout = new PartialHeaderLayout();
           
            DataSet _layout = new DataSet();
            string return_msg = string.Empty;
            try
            {


                if (Session["LastCompanyName"] == null || Session["LocalCurrency"] == null || Session["CompanyBigLogoUrl"] == null || Session["CompanySmallLogoUrl"] == null)
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
    }
}