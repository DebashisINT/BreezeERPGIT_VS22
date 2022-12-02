using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
//////using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using System.IO;
using System.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_AuthoSignatory : ERP.OMS.ViewState_class.VSPage
    {
        /* Tier Structure
         DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
         */
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string CheckEmp = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlSignatory.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlSignatory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlSignatory.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlSignatory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!Page.IsPostBack)
            {
                //Session["KeyVal"] = Request.QueryString["id"].ToString();
                Session["KeyVal_id"] = Request.QueryString["id"].ToString();
            }
        }

        protected void gridSignatory_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            TextBox TxtEmployeeClient = (TextBox)gridSignatory.FindEditFormTemplateControl("TxtEmployee");
            TxtEmployeeClient.Attributes.Add("onkeyup", "CallList(this,'SearchByEmployees',event)");
        }

        protected void gridSignatory_RowInserting1(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            HiddenField TxtEmployeeId = (HiddenField)gridSignatory.FindEditFormTemplateControl("TxtEmployee_hidden");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxTodate");
            string IdForCompExchange = Request.QueryString["id"].ToString();
            string[,] data = oDBEngine.GetFieldValue("tbl_master_companyExchange", "exch_compid,exch_segmentid", "exch_internalid='" + IdForCompExchange.ToString() + "'", 2);
            //Check database for duplicate entry
            string dStartDate = DStartDate.Date.ToString("yyyy-MM-dd");
            string dEndDate = DEndDate.Date.ToString("yyyy-MM-dd");
            string today = oDBEngine.GetDate().ToString("yyyy-MM-dd");

            string CheckData = oDBEngine.GetFieldValue("Master_AuthorizedSignatory", "AuthorizedSignatory_EmployeeID", "AuthorizedSignatory_CompanyID='" + data[0, 0].ToString() + "' and AuthorizedSignatory_SegmentID='" + IdForCompExchange + "'  and AuthorizedSignatory_EmployeeID='" + TxtEmployeeId.Value + "' and (AuthorizedSignatory_DateTo='1900-01-01 00:00:00.000' or AuthorizedSignatory_DateTo > '" + dStartDate + "')", 1)[0, 0];
            if (CheckData != "n")
            {
                CheckEmp = "Duplicate";

            }
            else
            {
                string Fields = "AuthorizedSignatory_CompanyID,AuthorizedSignatory_SegmentID,AuthorizedSignatory_EmployeeID,AuthorizedSignatory_DateFrom,AuthorizedSignatory_DateTo,AuthorizedSignatory_CreateUser,AuthorizedSignatory_CreateDateTime";
                oDBEngine.InsurtFieldValue("Master_AuthorizedSignatory", Fields, "'" + data[0, 0].ToString() + "'," + IdForCompExchange + ",'" + TxtEmployeeId.Value + "','" + dStartDate + "','" + dEndDate + "','" + Session["userid"] + "','" + today + "'");
            }
            gridSignatory.CancelEdit();
            e.Cancel = true;
        }

        //protected void gridSignatory_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        //{
        //    HiddenField TxtEmployeeId = (HiddenField)gridSignatory.FindEditFormTemplateControl("TxtEmployee_hidden");
        //    TextBox TxtEmployeeName = (TextBox)gridSignatory.FindEditFormTemplateControl("TxtEmployee");
        //    ASPxDateEdit DStartDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxStartDate");
        //    ASPxDateEdit DEndDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxTodate");
        //    string id = e.EditingKeyValue.ToString();
        //    HiddenField TxtId = (HiddenField)gridSignatory.FindEditFormTemplateControl("hdid");
        //    TxtId.Value = e.EditingKeyValue.ToString();
        //    DataTable Data = oDBEngine.GetDataTable("Master_AuthorizedSignatory", "AuthorizedSignatory_ID,AuthorizedSignatory_CompanyID,AuthorizedSignatory_SegmentID,AuthorizedSignatory_EmployeeID,AuthorizedSignatory_DateFrom,AuthorizedSignatory_DateTo", "AuthorizedSignatory_ID='" + id + "'");
        //    TxtEmployeeId.Value = Data.Rows[0]["AuthorizedSignatory_EmployeeID"].ToString();
        //    DStartDate.Value = Convert.ToDateTime(Data.Rows[0]["AuthorizedSignatory_DateFrom"].ToString());
        //    DEndDate.Value = Convert.ToDateTime(Data.Rows[0]["AuthorizedSignatory_DateTo"].ToString());
        //    string name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') as name", "cnt_internalid='" + Data.Rows[0]["AuthorizedSignatory_EmployeeID"].ToString() + "'", 1)[0, 0];
        //    TxtEmployeeName.Text = name.ToString();
        //}
        protected void gridSignatory_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            HiddenField TxtEmployeeId = (HiddenField)gridSignatory.FindEditFormTemplateControl("TxtEmployee_hidden");
            HiddenField TxtId = (HiddenField)gridSignatory.FindEditFormTemplateControl("hdid");
            TextBox TxtEmployeeName = (TextBox)gridSignatory.FindEditFormTemplateControl("TxtEmployee");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSignatory.FindEditFormTemplateControl("ASPxTodate");
            string dStartDate = DStartDate.Date.ToString("yyyy-MM-dd");
            string dEndDate = DEndDate.Date.ToString("yyyy-MM-dd");
            string today = oDBEngine.GetDate().ToString("yyyy-MM-dd");

            e.NewValues["AuthorizedSignatory_DateFrom"] = dStartDate;
            e.NewValues["AuthorizedSignatory_DateTo"] = dEndDate;
            e.NewValues["AuthorizedSignatory_ModifyDateTime"] = today;
            if (TxtEmployeeId.Value != "")
            {
                e.NewValues["AuthorizedSignatory_EmployeeID"] = TxtEmployeeId.Value;
            }
            // e.NewValues["AuthorizedSignatory_EmployeeID"] = TxtEmployeeId.Value;
            //oDBEngine.SetFieldValue("Master_AuthorizedSignatory", "AuthorizedSignatory_EmployeeID='" + TxtEmployeeId.Value + "',AuthorizedSignatory_DateFrom='" + DStartDate.Value + "',AuthorizedSignatory_DateTo='" + DEndDate.Value + "',AuthorizedSignatory_ModifyUser='" + Session["userid"].ToString() + "',AuthorizedSignatory_ModifyDateTime='" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "'", "AuthorizedSignatory_ID='" + TxtId.Value + "'");
            //gridSignatory.CancelEdit();
            //e.Cancel = true;
        }

        protected void gridSignatory_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string id = e.Keys[0].ToString();

            DataTable DT = oDBEngine.GetDataTable("Master_AuthorizedSignatory", "AuthorizedSignatory_ID,AuthorizedSignatory_CompanyID,AuthorizedSignatory_SegmentID,AuthorizedSignatory_EmployeeID,AuthorizedSignatory_DateFrom,AuthorizedSignatory_DateTo,AuthorizedSignatory_CreateUser,AuthorizedSignatory_CreateDateTime,AuthorizedSignatory_ModifyUser,AuthorizedSignatory_ModifyDateTime", "AuthorizedSignatory_ID='" + id + "'");
            oDBEngine.InsurtFieldValue("Master_AuthorizedSignatory_log", "AuthorizedSignatory_CompanyID,AuthorizedSignatory_SegmentID,AuthorizedSignatory_EmployeeID,AuthorizedSignatory_DateFrom,AuthorizedSignatory_DateTo,AuthorizedSignatory_CreateUser,AuthorizedSignatory_CreateDateTime,AuthorizedSignatory_ModifyUser,AuthorizedSignatory_ModifyDateTime,AuthorizedSignatory_DeleteUser,AuthorizedSignatory_DeleteDateTime,Status", "'" + DT.Rows[0]["AuthorizedSignatory_CompanyID"] + "','" + DT.Rows[0]["AuthorizedSignatory_SegmentID"] + "','" + DT.Rows[0]["AuthorizedSignatory_EmployeeID"] + "','" + DT.Rows[0]["AuthorizedSignatory_DateFrom"] + "','" + DT.Rows[0]["AuthorizedSignatory_DateTo"] + "','" + DT.Rows[0]["AuthorizedSignatory_CreateUser"] + "','" + DT.Rows[0]["AuthorizedSignatory_CreateDateTime"] + "','" + DT.Rows[0]["AuthorizedSignatory_ModifyUser"] + "','" + DT.Rows[0]["AuthorizedSignatory_ModifyDateTime"] + "','" + Session["userid"] + "','" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "','D'");
            oDBEngine.DeleteValue("Master_AuthorizedSignatory", " AuthorizedSignatory_ID='" + id + "'");
            e.Cancel = true;
        }
        protected void gridSignatory_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpValue"] = CheckEmp;
        }
    }
}