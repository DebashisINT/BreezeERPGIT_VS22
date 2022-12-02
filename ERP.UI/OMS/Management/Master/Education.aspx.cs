using System;
using System.Web;
//using DevExpress.Web; 
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Education : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
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
           
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Education.aspx");
           
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    education.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                }
                else
                {
                    education.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                }
            }
            EducationGrid.JSProperties["cpDelmsg"] = null;

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            string filename = "Education";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Education";
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
        protected void EducationGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < EducationGrid.Columns.Count; i++)
                    if (EducationGrid.Columns[i] is GridViewCommandColumn)
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
        protected void EducationGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!EducationGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = EducationGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void EducationGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                EducationGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                EducationGrid.FilterExpression = string.Empty;
            }
        }

        protected void EducationGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // Code Added and Commented By Sam on 15112016 to use Stored Proc to Check Master Data used in Other Segment
            MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
            int id = Convert.ToInt32(e.Keys[0]);
            int i = masterdata.DeleteMasterEducation(id);
            if(i==1)
            {
                EducationGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
            }
            else
            {
                EducationGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;
            }

            
            //string Id = e.Keys[0].ToString();
            //string[,] acccode = oDBEngine.GetFieldValue("tbl_master_educationProfessional",
            //               "edu_degree", "edu_degree=(select edu_id from tbl_master_education where edu_id='" + Id + "')", 1);


            //if (acccode[0, 0] != "n")
            //{
            //    EducationGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
            //    e.Cancel = true;
            //}
            //else
            //{
            //    //oDBEngine.DeleteValue("tbl_master_education ", "cnt_education ='" + Id + "' and branch_id not in (select distinct cnt_branchid from tbl_master_contact)");
            //    EducationGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

            //}


        }
        //Purpose: Add Edit and delete rights to Education
        //Name: Debjyoti Dhar.
        protected void EducationGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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