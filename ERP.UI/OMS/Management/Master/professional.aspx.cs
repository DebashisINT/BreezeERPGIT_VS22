using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_professional : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //GenericMethod oGenericmethod;
        //BusinessLogicLayer.GenericMethod oGenericmethod;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/professional.aspx");

            //ProfQualGrid.JSProperties["cpEdit"] = null;
            //ProfQualGrid.JSProperties["cpinsert"] = null;
            //ProfQualGrid.JSProperties["cpUpdate"] = null;
            //ProfQualGrid.JSProperties["cpDelete"] = null;
            //ProfQualGrid.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            BindProfessionalGrid();
            //ProfQualGrid.JSProperties["cpDelete"] = null;
            ProfQualGrid.JSProperties["cpEdit"] = null;
            ProfQualGrid.JSProperties["cpinsert"] = null;
            ProfQualGrid.JSProperties["cpUpdate"] = null;
            ProfQualGrid.JSProperties["cpDelete"] = null;
            ProfQualGrid.JSProperties["cpExists"] = null;
        }

        protected void BindProfessionalGrid()
        {
            // oGenericmethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericmethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtProf = new DataTable();
            dtProf = oGenericmethod.GetDataTable("SELECT pro_id,pro_professionName FROM tbl_master_profession order by pro_professionName");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtProf.Rows.Count > 0)
            {
                ProfQualGrid.DataSource = dtProf;
                ProfQualGrid.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            string filename = "Professions";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Professions";
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
        protected void ProfQualGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < ProfQualGrid.Columns.Count; i++)
                    if (ProfQualGrid.Columns[i] is GridViewCommandColumn)
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
        protected void ProfQualGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!ProfQualGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = ProfQualGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void ProfQualGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // GenericMethod oGenericmethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericmethod = new BusinessLogicLayer.GenericMethod();
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
          
            string WhichType = null;
            if (e.Parameters.ToString().Contains("~"))
                WhichType = e.Parameters.ToString().Split('~')[1];
            string WhichCall = e.Parameters.ToString().Split('~')[0];
            if (e.Parameters == "s")
                ProfQualGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                ProfQualGrid.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {
                //PopupProfession.HeaderText = "Edit Profession Details";
                DataTable dtEdit = oGenericmethod.GetDataTable("tbl_master_profession", "pro_professionName", "pro_id='" + WhichType + "'");
                if (dtEdit.Rows.Count > 0)
                {
                    if (dtEdit != null)
                    {
                        string p_profession = string.Empty;
                        p_profession = dtEdit.Rows[0]["pro_professionName"].ToString().Trim();

                        ProfQualGrid.JSProperties["cpEdit"] = p_profession + "~" + WhichType;

                    }
                }
            }
            if (WhichCall == "SaveProfession")
            {
                //  oGenericmethod = new GenericMethod();

                BusinessLogicLayer.GenericMethod oGenericmethod1 = new BusinessLogicLayer.GenericMethod();

                string[,] countrecord = oGenericmethod1.GetFieldValue("tbl_master_profession", "pro_professionName", "pro_professionName='" + txtProfession.Text + "'", 1);
                if (countrecord[0, 0] != "n")
                {
                    ProfQualGrid.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    insertcount = oGenericmethod1.Insert_Table("tbl_master_profession", "pro_professionName", "'" + txtProfession.Text + "'");
                    if (insertcount > 0)
                    {
                        ProfQualGrid.JSProperties["cp_FilterApplied"] = "true";
                        ProfQualGrid.JSProperties["cpinsert"] = "Success";
                        BindProfessionalGrid();
                    }
                    else
                        ProfQualGrid.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "UpdateProfession")
            {
                updtcnt = oGenericmethod.Update_Table("tbl_master_profession", "pro_professionName='" + txtProfession.Text.Trim() + "'", "pro_id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    ProfQualGrid.JSProperties["cpUpdate"] = "Success";
                    BindProfessionalGrid();
                }
                else
                    ProfQualGrid.JSProperties["cpUpdate"] = "fail";
            }

            if (WhichCall == "Delete")
            {
                //oGenericmethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericmethod2 = new BusinessLogicLayer.GenericMethod();

                DataTable delete = oDBEngine.GetDataTable("select * from tbl_master_contact where cnt_profession =" + WhichType);
                if (delete.Rows.Count < 1)
                {
                    deletecnt = oGenericmethod2.Delete_Table("tbl_master_profession", "pro_id=" + WhichType + "");
                    if (deletecnt > 0)
                    {
                        ProfQualGrid.JSProperties["cpDelete"] = "Success";
                        BindProfessionalGrid();
                    }
                    else
                        ProfQualGrid.JSProperties["cpDelete"] = "Success";
                }
                else if (delete.Rows.Count > 0)
                {
                    ProfQualGrid.JSProperties["cpDelete"] = "datalinked";
                }

            }
        }

        protected void ProfQualGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }
    }
}