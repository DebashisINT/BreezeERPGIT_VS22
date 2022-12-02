using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_DPDetails : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine dbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/HRrecruitmentagent_DPDetails.aspx"); string cnttype = Session["ContactType"].ToString();
            SelectDp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];multi
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                string[,] EmployeeNameID = dbEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
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
                string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_accountType", " dpd_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and dpd_accountType='" + Category + "'", 1);
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
                string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_id", " dpd_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and dpd_accountType='" + Category + "'", 1);
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