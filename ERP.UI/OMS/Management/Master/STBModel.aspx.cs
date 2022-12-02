using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class STBModel : System.Web.UI.Page
    {
        public bool IsImport = false;
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL Modelcb = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/STBModel.aspx");
            BindSTBModelGrid();
            if (!IsPostBack)
            {
                string ModelImportInModelMaster = Modelcb.GetSystemSettingsResult("ModelImportInModelMaster");
                if (ModelImportInModelMaster.ToUpper() == "YES")
                {
                    IsImport = true;
                }

                GridSTBModel.JSProperties["cpEdit"] = null;
                GridSTBModel.JSProperties["cpinsert"] = null;
                GridSTBModel.JSProperties["cpUpdate"] = null;
                GridSTBModel.JSProperties["cpDelete"] = null;
                GridSTBModel.JSProperties["cpExists"] = null;
                if (HttpContext.Current.Session["userid"] == null)
                {
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }

               

                Session["exportval"] = null;
            }
        }

        protected void BindSTBModelGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM SRV_master_STBModel order by STBModel_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridSTBModel.DataSource = dtFillGrid;
                GridSTBModel.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridSTBModel.Settings.ShowFilterRow = true;
        }

        protected void GridSTBModel_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridSTBModel.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridSTBModel.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void GridSTBModel_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridSTBModel.JSProperties["cpEdit"] = null;
            GridSTBModel.JSProperties["cpinsert"] = null;
            GridSTBModel.JSProperties["cpUpdate"] = null;
            GridSTBModel.JSProperties["cpDelete"] = null;
            GridSTBModel.JSProperties["cpExists"] = null;
            GridSTBModel.JSProperties["cpImportModel"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridSTBModel.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridSTBModel.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable("select STBModel_Id,STBModel_Name from SRV_master_STBModel where STBModel_Id=" + WhichType + "");
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string STBModel = Convert.ToString(dtEdit.Rows[0]["STBModel_Name"]);
                    int STBModel_Id = Convert.ToInt32(dtEdit.Rows[0]["STBModel_Id"]);
                    GridSTBModel.JSProperties["cpEdit"] = STBModel + "~" + STBModel_Id;
                }
            }

            if (WhichCall == "updateSTBModel")
            {
                updtcnt = oGenericMethod.Update_Table("SRV_master_STBModel", "STBModel_Name='" + txtSTBModelName.Text + "'", "STBModel_Id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    GridSTBModel.JSProperties["cpUpdate"] = "Success";
                    BindSTBModelGrid();
                }
                else
                    GridSTBModel.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "saveSTBModel")
            {
                string[,] countrecord = oGenericMethod.GetFieldValue("SRV_master_STBModel", "STBModel_Name", "STBModel_Name='" + txtSTBModelName.Text + "'", 1);
                if (countrecord[0, 0] != "n")
                {
                    GridSTBModel.JSProperties["cpExists"] = "Exists";
                }
                else
                {

                    insertcount = oGenericMethod.Insert_Table("SRV_master_STBModel", "STBModel_Name,CREATED_ON,CREATED_BY",
                       "'" + txtSTBModelName.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    if (insertcount > 0)
                    {
                        GridSTBModel.JSProperties["cpinsert"] = "Success";
                        BindSTBModelGrid();
                    }
                    else
                        GridSTBModel.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterdata.DeleteSTBModel(Convert.ToString(id));
                if (i == 1)
                {
                    GridSTBModel.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridSTBModel.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";

                }
               
            }
            else if (WhichCall == "ImportModel")
            {
                string ComanyDbName = Convert.ToString(e.Parameters).Split('~')[1];
                ProcedureExecute proc = new ProcedureExecute("PROC_ImportSTBModelMaster");
                proc.AddVarcharPara("@Action", 500, "IMPORT");
                proc.AddVarcharPara("@DBNAME", 200, ComanyDbName);
                int i = proc.RunActionQuery();
                if (i > 0)
                {
                    GridSTBModel.JSProperties["cpImportModel"] = "Success";
                }
                else
                {
                    GridSTBModel.JSProperties["cpImportModel"] = "fail";
                }
            }
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
            GridSTBModel.Columns[2].Visible = false;

            string filename = "STBModel";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "STBModel";
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

        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }
    }
}