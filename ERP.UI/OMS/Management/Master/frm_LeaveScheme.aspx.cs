using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_LeaveScheme : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
           
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_LeaveScheme.aspx");
           
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

            if (!IsPostBack)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
        }
        protected void GridLeave_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ls_TotalPrevilegeLeave"] = 0;
            e.NewValues["ls_PLapplicablefor"] = "F";
            e.NewValues["ls_PLCalculationBasis"] = "P";
            e.NewValues["ls_PLaccumulation_days"] = 0;
            e.NewValues["ls_PLaccumulation_M_Y"] = "M";
            e.NewValues["ls_PLentitlement"] = 0;
            e.NewValues["ls_PLencashed"] = "Y";
            e.NewValues["ls_PLencashedEligibility"] = 0;
            e.NewValues["ls_PLaccumulatedCFNextYear"] = "Y";
            e.NewValues["ls_PLaccumulatedMax"] = 0;
            e.NewValues["ls_PLinstallments"] = 0;
            e.NewValues["ls_PLMinDayPerInstallments"] = 0;
            e.NewValues["ls_PLcount_PSWO_PH"] = "O";
            e.NewValues["ls_CLtotal"] = 0;
            e.NewValues["ls_CLCalculationBasis"] = "P";
            e.NewValues["ls_CLapplicablefor"] = "F";
            e.NewValues["ls_PLaccountMindayForEncashment"] = 0;
            e.NewValues["ls_CLentitlement"] = 0;
            e.NewValues["ls_CLencashed"] = "N";
            e.NewValues["ls_CLencashedEligibility"] = 0;
            e.NewValues["ls_CLaccumulatedCFNextYear"] = "N";
            e.NewValues["ls_CLcount_PSWO_PH"] = "O";
            e.NewValues["ls_CLMaxDayPerInstallments"] = 0;
            e.NewValues["ls_CLaccumulatedMax"] = 0;
            e.NewValues["ls_SLtotal"] = 0;
            e.NewValues["ls_SLapplicablefor"] = "F";
            e.NewValues["ls_SLCalculationBasis"] = "P";
            e.NewValues["ls_SLentitlement"] = 0;
            e.NewValues["ls_SLencashed"] = "N";
            e.NewValues["ls_SLMaxDayPerInstallments"] = 0;
            e.NewValues["ls_SLaccumulatedMax"] = 0;
            e.NewValues["ls_SLaccumulatedCFNextYear"] = "Y";
            e.NewValues["ls_SLencashedEligibility"] = 0;
            e.NewValues["ls_SLcount_PSWO_PH"] = "O";
            e.NewValues["ls_MLtotalPre"] = 0;
            e.NewValues["ls_MLtotalPos"] = 0;
            e.NewValues["ls_MLeligibility"] = 0;
            e.NewValues["ls_MLisPreWpostAdjustment"] = "Y";
        }
        protected void GridLeave_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (GridLeave.IsNewRowEditing)
            {
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string name = e.NewValues["ls_name"].ToString();
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_LeaveScheme ", " ls_id ", " ls_name='" + name.Trim() + "'", 1);
                if (data[0, 0] != "n")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Jc", "<script  language='javascript'>alertmessage();</script>", true);
                    //this.Page.ClientScript.RegisterStartupScript(GetType(),"Validation","<script  language='javascript'>alertmessage();</script>");
                    e.RowError = "This Leave Scheme Name already Exist!";
                    return;
                }
            }
        }
        protected void GridLeave_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!GridLeave.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridLeave.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
            else
            {
                ASPxGridViewTemplateReplacement RT = GridLeave.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }
        protected void GridLeave_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < GridLeave.Columns.Count; i++)
                    if (GridLeave.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 36;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    //DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    DevExpress.Web.Rendering.GridViewCommandItemsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridViewCommandItemsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"] == "DelAdd" || Session["PageAccess"] == "Delete" || Session["PageAccess"] == "All")
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
        protected void GridLeave_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void GridLeave_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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