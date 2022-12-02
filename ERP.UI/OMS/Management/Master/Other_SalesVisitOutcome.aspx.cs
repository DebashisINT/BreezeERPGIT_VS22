using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Other_SalesVisitOutcome : ERP.OMS.ViewState_class.VSPage
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
        protected void Page_Init(object sender, EventArgs e)
        {
            CategorySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SalesVisitOutcomes.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Other_SalesVisitOutcome.aspx");
            
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                   // SalesVisitOutcomes.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                }
                else
                {
                   // SalesVisitOutcomes.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            string filename = "Sales Visit Outcome";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Sales Visit Outcome";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
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
        protected void btnSearch(object sender, EventArgs e)
        {
            OutComeGrid.Settings.ShowFilterRow = true;
        }
        protected void OutComeGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
                //int commandColumnIndex = -1;
                //for (int i = 0; i < OutComeGrid.Columns.Count; i++)
                //    if (OutComeGrid.Columns[i] is GridViewCommandColumn)
                //    {
                //        commandColumnIndex = i;
                //        break;
                //    }
                //if (commandColumnIndex == -1)
                //    return;
                //____One colum has been hided so index of command column will be leass by 1 
                //commandColumnIndex = commandColumnIndex - 4;
                //DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                //for (int i = 0; i < cell.Controls.Count; i++)
                //{
                //    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                //    if (button == null) return;
                //    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                //    if (hyperlink.Text == "Delete")
                //    {
                //        if (Session["PageAccess"] == "DelAdd" || Session["PageAccess"] == "Delete" || Session["PageAccess"] == "All")
                //        {
                //            hyperlink.Enabled = true;
                //            continue;
                //        }
                //        else
                //        {
                //            hyperlink.Enabled = false;
                //            continue;
                //        }
                //    }


                //}

           // }

        }
        protected void OutComeGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!OutComeGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = OutComeGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Edit" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void OutComeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                OutComeGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                OutComeGrid.FilterExpression = string.Empty;
            }
        }
        
        //Purpose: Add Edit and delete rights to sales visit outcome
        //Name: Debjyoti Dhar.
        protected void OutComeGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //if (!rights.CanDelete)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Delete)
            //    {
            //        e.Visible = false;
            //    }
            //}


            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}

        }
        public bool GetChecked(string Access)
        {
            switch (Access)
            {
                case "True":
                    return true;
                case "":
                    return false;
                case "Null":
                    return false;
                default:
                    return false;
            }

        }

        protected void chkActivateBatch_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            string itemindex = Convert.ToString((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            //Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {   { grid.PerformCallback('{0}');  }  }", itemindex);
            Dcheckbox.ClientSideEvents.CheckedChanged = "function(s, e) {  ActiveCall(s,e," + itemindex + ");  }";
        }

     
        protected void OutComeGrid_CustomCallback1(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string status = e.Parameters;
            int i = 0;
            var s = status.Split('~');
            string id = s[0];
            string active = s[1];
            BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
            i = obl.EditSalesVisitOutcomeIsActiveStatus(id, active);
            if (i > 0)
            {
                SalesVisitOutcomes.SelectCommand = "SELECT sv.slv_Id AS ID, sv.slv_SalesVisitOutcome AS Outcome, sv.slv_Category AS Category, s.Description AS description FROM tbl_master_SalesVisitOutCome AS sv INNER JOIN tbl_Master_SalesVisitOutcomeCategory AS s ON sv.slv_Category = s.Int_id";
            }
        }

        protected void OutComeGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            //GridViewDataTextColumn col1 = (GridViewDataTextColumn)OutComeGrid.Columns["IsActive"];
            ((GridViewDataColumn)OutComeGrid.Columns["IsActive"]).EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;

          
        }
    }
}