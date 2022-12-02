using System;
using System.Data;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_frmshowreminder1 : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] != null)
            {
                DataTable dtIPAddress = oDBEngine.GetDataTable("tbl_master_user", "user_status", " user_ID='" + Session["userid"].ToString() + "'");
                if (dtIPAddress.Rows[0][0].ToString().Trim() == "0")
                {
                    Session.Abandon();
                    Page.ClientScript.RegisterStartupScript(GetType(), "JSAlrt", "<script>alert('You have been Logged out as another User has Logged in from another location')</script>");
                   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
            }
        }
    }
}