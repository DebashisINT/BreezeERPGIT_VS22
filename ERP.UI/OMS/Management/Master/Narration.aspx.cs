using System;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Narration : ERP.OMS.ViewState_class.VSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //Narration.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    Narration.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //Narration.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    Narration.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }

        }
        protected void GrdNarration_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                GrdNarration.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                GrdNarration.FilterExpression = string.Empty;
            }
        }
    }
}