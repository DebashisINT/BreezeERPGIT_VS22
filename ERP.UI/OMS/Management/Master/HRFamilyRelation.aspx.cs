using System;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRFamilyRelation : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string pageAccess = "";
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRFamilyRelation.aspx");
            
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //FamilySource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    FamilySource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //FamilySource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    FamilySource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }
            FamilyGrid.JSProperties["cpDelmsg"] = null;
            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            string filename = "Family Relationship";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Family Relationship";
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
        protected void FamilyGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < FamilyGrid.Columns.Count; i++)
                    if (FamilyGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 5;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    //DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    DevExpress.Web.Rendering.GridViewCommandItemsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridViewCommandItemsCell;

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
        protected void FamilyGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!FamilyGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = FamilyGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void FamilyGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                FamilyGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                FamilyGrid.FilterExpression = string.Empty;
            }
        }

        protected void FamilyGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string Id = e.Keys[0].ToString();
            string[,] acccode = oDBEngine.GetFieldValue("tbl_master_contactfamilyrelationship",
                           "femrel_relationId", "femrel_relationId=(select fam_id from tbl_master_familyRelationship where fam_id='" + Id + "')", 1);


            if (acccode[0, 0] != "n")
            {
                FamilyGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;
            }
            else
            {
                //oDBEngine.DeleteValue("tbl_master_education ", "cnt_education ='" + Id + "' and branch_id not in (select distinct cnt_branchid from tbl_master_contact)");
                FamilyGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

            }
        }

        //Purpose: Add Edit and delete rights to Family
        //Name: Debjyoti Dhar.
        protected void FamilyGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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