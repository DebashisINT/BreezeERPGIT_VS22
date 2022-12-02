using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_DocumentType : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Purpose : Replace .ToString() with Convert.ToString(..)
                //Name : Sudip 
                // Dated : 21-12-2016

                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
                Session["exportval"] = null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/DocumentType.aspx");
         
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DocumentType.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    DocumentType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DocumentType.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    DocumentType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            } 
        }
        public void bindexport(int Filter)
        {
            //DocumentGrid.Columns[5].Visible = false;
            string filename = "Document Types List";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Document Types";
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
        protected void DocumentGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = 1;
                for (int i = 0; i < DocumentGrid.Columns.Count; i++)
                    if (DocumentGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == 1)
                    return;
                //____Two colum has been hided so index of command column will be leass by 1 
                //commandColumnIndex = commandColumnIndex - 1;
                commandColumnIndex = commandColumnIndex - 2;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if ( Convert.ToString(Session["PageAccess"]) == "DelAdd" ||  Convert.ToString(Session["PageAccess"]) == "Delete" ||  Convert.ToString(Session["PageAccess"]) == "All")
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
        protected void DocumentGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            //if (!DocumentGrid.IsNewRowEditing)
            //{
            //    ASPxGridViewTemplateReplacement RT = DocumentGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
            //    if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
            //        RT.Visible = true;
            //    else
            //        RT.Visible = false;
            //}

        }
        protected void DocumentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                DocumentGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                DocumentGrid.FilterExpression = string.Empty;
            }
        }
        protected void DocumentGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
            int id = Convert.ToInt32(e.Keys[0]);
            int i = masterdata.DeleteMasterDocumentType(id);
            if (i == 1)
            {
                DocumentGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
            }
            else
            {
                DocumentGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;
            }


            //string Id = e.Keys[0].ToString();
            //int i = 0;
            //i = oDBEngine.DeleteValue("tbl_master_documentType", " dty_id='" + Id + "'");
            ////string KeyVal = e.Keys["Id"].ToString();

            ////string[,] acccode = oDBEngine.GetFieldValue("tbl_master_groupMaster,tbl_trans_group",
            ////  "grp_contactId", "gpm_id=grp_groupMaster and gpm_id='" + KeyVal + "'", 1);

            //if (i > 0)
            //{
            //    DocumentGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
            //}
            //else
            //{
            //    //AccountGroup.JSProperties["cpDelmsg"] = "Cannot Delete. This AccountGroup Code Is In Use";
            //    DocumentGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
            //    e.Cancel = true;
            //}

        }
        //Purpose: Add Edit and delete rights to document types
        //Name: Debjyoti Dhar.
        protected void DocumentGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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