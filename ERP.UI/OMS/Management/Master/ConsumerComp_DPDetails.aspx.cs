using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using DevExpress.Web;
////using DevExpress.Web.ASPxTabControl;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
//using DevExpress.Web.ASPxCallbackPanel;
//////using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
////using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ConsumerComp_DPDetails : ERP.OMS.ViewState_class.VSPage
    {
        //  DBEngine dbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {


            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            SelectDp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void DpDetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName");
            DPname.Attributes.Add("onkeyup", "CallList(this,'DPName',event)");
        }
        protected void DpDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
            e.NewValues["DPName"] = DPname.Text;
        }
        protected void DpDetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
            e.NewValues["DPName"] = DPname.Text;
        }
        protected void DpDetailsGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["Category"] == null)
            {
                e.RowError = "Please Select Category";
                return;
            }
            if (e.NewValues["DP"] == null)
            {
                e.RowError = "Please Enter DP Name";
                return;
            }
            if (e.NewValues["ClientId"] == null)
            {
                e.RowError = "Please Enter Client ID";
                return;
            }
            if (e.NewValues["POA"] == null)
            {
                e.RowError = "Please Select POA";
                return;
            }
            if (DpDetailsGrid.IsNewRowEditing)
            {
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_accountType", " dpd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and dpd_accountType='" + Category + "'", 1);
                if (Category1[0, 0] != "n")
                {
                    if (Category1[0, 0] == "Default")
                    {
                        e.RowError = "Default Category Already Exists!";
                        return;
                    }
                }
            }
            else
            {
                string KeyVal = e.Keys["Id"].ToString();
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_id", " dpd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and dpd_accountType='" + Category + "'", 1);
                if (Category1[0, 0] != "n")
                {
                    if (KeyVal != Category1[0, 0])
                    {
                        e.RowError = "Default Category Already Exists!";
                        return;
                    }
                }
            }
        }
    }
}