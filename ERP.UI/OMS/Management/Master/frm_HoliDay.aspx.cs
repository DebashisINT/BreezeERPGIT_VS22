using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_HoliDay : ERP.OMS.ViewState_class.VSPage, System.Web.UI.ICallbackEventHandler
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        DataTable DT = new DataTable();
        string data;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

        
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
                
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_HoliDay.aspx");
            
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string mode = Session["mode"].ToString();
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            #region Edit
            if (idlist[0] == "Edit")
            {
                Session["KeyVal"] = idlist[1].ToString();

            }
            #endregion
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        protected void GridWorking_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string param = e.Parameters.ToString();
            string[] data = param.Split('~');
            string[] ids = data[2].Split(',');
            Session["KeyVal"] = data[0];
            if (data[2] != "")
            {
                string wherecondition = " wor_id in (" + data[2] + ")";

                DataTable dt_work = oDBEngine.GetDataTable(" tbl_master_workingHours", " wor_id,wor_scheduleName ", wherecondition);
                GridWorking.DataSource = dt_work.DefaultView;
                GridWorking.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                GridWorking.DataSource = dt.DefaultView;
                GridWorking.DataBind();
            }
        }
        protected void GridExchange_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string param = e.Parameters.ToString();
            string[] data = param.Split('~');
            string[] ids = data[1].Split(',');
            //Session["KeyVal"] = data[0];
            if (data[1] != "")
            {
                string wherecondition = " exh_id in (" + data[1] + ")";
                DataTable dt_Exch = oDBEngine.GetDataTable(" tbl_master_exchange ", " exh_id, exh_name ", wherecondition);
                GridExchange.DataSource = dt_Exch.DefaultView;
                GridExchange.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                GridExchange.DataSource = dt.DefaultView;
                GridExchange.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowGridsForUpdate();</script>");
        }
        protected void GridHoliday_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {


            if (e.NewValues["hol_IsBankHoliday"].ToString().Trim() == "True")
                e.NewValues["hol_IsBankHoliday"] = "Y";
            if (e.NewValues["hol_IsBankHoliday"].ToString().Trim() == "False")
                e.NewValues["hol_IsBankHoliday"] = "N";

            //if (e.NewValues["hol_IsDepositoryHoliday"].ToString().Trim() == "True")
            //    e.NewValues["hol_IsDepositoryHoliday"] = "Y";
            //if (e.NewValues["hol_IsDepositoryHoliday"].ToString().Trim() == "False")
            //    e.NewValues["hol_IsDepositoryHoliday"] = "N";
        }
        protected void GridHoliday_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            string status = e.NewValues["hol_IsBankHoliday"].ToString().Trim();
            if (e.NewValues["hol_IsBankHoliday"].ToString().Trim() == "True")
                e.NewValues["hol_IsBankHoliday"] = "Y";
            if (e.NewValues["hol_IsBankHoliday"].ToString().Trim() == "False")
                e.NewValues["hol_IsBankHoliday"] = "N";
            //status = e.NewValues["hol_IsDepositoryHoliday"].ToString().Trim();
            //if (e.NewValues["hol_IsDepositoryHoliday"].ToString().Trim() == "True")
            //    e.NewValues["hol_IsDepositoryHoliday"] = "Y";
            //if (e.NewValues["hol_IsDepositoryHoliday"].ToString().Trim() == "False")
            //    e.NewValues["hol_IsDepositoryHoliday"] = "N";
        }

        protected void GridHoliday_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["hol_IsHoliday"] = "Y";
            e.NewValues["hol_IsBankHoliday"] = "True";
            //e.NewValues["hol_IsDepositoryHoliday"] = "True";
        }

        protected void GridHoliday_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridHoliday.DataBind();
        }
        protected void GridHoliday_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string date = e.NewValues["hol_DateOfHoliday"].ToString(); ;
            if (GridHoliday.IsNewRowEditing)
            {
                string[,] isdateExist = oDBEngine.GetFieldValue(" tbl_master_holiday ", " hol_id ", " convert(varchar(10),hol_Dateofholiday,101)=convert(varchar(10),cast('" + date.Trim() + "' as datetime),101)", 1);
                if (isdateExist[0, 0] != "n")
                {
                    e.RowError = "This Date Already Exist. Please select another Date!";
                    return;
                }
            }
        }
        protected void GridHoliday_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < GridHoliday.Columns.Count; i++)
                    if (GridHoliday.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 1;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }


                }

            }
        }
        protected void GridHoliday_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            GridHoliday.SettingsText.PopupEditFormCaption = "Modify Holiday";
        }
   
        protected void GridHoliday_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridHoliday.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridHoliday.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
            else
            {
                ASPxGridViewTemplateReplacement RT = GridHoliday.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void GridExchange_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void GridHoliday_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void GridHoliday_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }
    }
}