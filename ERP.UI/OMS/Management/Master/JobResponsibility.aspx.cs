using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Services;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_JobResponsibility : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Purpose : Replace .ToString() with Convert.ToString(..)
                //Name : Sudip 
                // Dated : 22-12-2016

                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/JobResponsibility.aspx");

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    Session["exportval"] = null;
                }

                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {
                        //jobResponse.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                        jobResponse.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    }
                    else
                    {
                        //jobResponse.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        jobResponse.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}

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
            string filename = "Job Responsibilities List";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Job Responsibilities";
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
        protected void JobResponseGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < JobResponseGrid.Columns.Count; i++)
                    if (JobResponseGrid.Columns[i] is GridViewCommandColumn)
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
                        if (Convert.ToString(Session["PageAccess"]) == "DelAdd" || Convert.ToString(Session["PageAccess"]) == "Delete" || Convert.ToString(Session["PageAccess"]) == "All")
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
        protected void JobResponseGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            if (!JobResponseGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = JobResponseGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }
        protected void JobResponseGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                JobResponseGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                JobResponseGrid.FilterExpression = string.Empty;
            }
        }
        [WebMethod]
        public static bool CheckUniqueName(string ProductClassCode, int proclassid)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (Convert.ToString(proclassid) != "" && Convert.ToString(ProductClassCode).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ProductClassCode.Trim(), Convert.ToString(proclassid), "MasterJobResponsibilities");
            }
            return status;
        }
        protected void JobResponseGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            int id = Convert.ToInt32(Convert.ToString(e.EditingKeyValue));
            Session["id"] = id;
        }
        protected void JobResponseGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
            int id = Convert.ToInt32(e.Keys[0]);
            int i = masterdata.DeleteJobResponsibilities(Convert.ToString(id));
            if (i == 1)
            {
                JobResponseGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
            }
            else
            {
                JobResponseGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;
            }
        }
        //Purpose: Add Edit and delete rights to Job Responsibilities
        //Name: Debjyoti Dhar.
        protected void JobResponseGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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