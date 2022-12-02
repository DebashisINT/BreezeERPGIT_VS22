using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class STBSchemeMast : System.Web.UI.Page
    {
        public bool IsImport = false;
        STBSchemeMastBL STBSchemeBL = new STBSchemeMastBL();
        public string pageAccess = "";
        BusinessLogicLayer.MasterDbEngine oDBEngineMst = new BusinessLogicLayer.MasterDbEngine();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL STBSchemecb = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/STBSchemeMast.aspx");

            if (!IsPostBack)
            {
                string STBSchemeImportInSTBSchemeMaster = STBSchemecb.GetSystemSettingsResult("STBSchemeImportInSTBSchemeMaster");
                if (STBSchemeImportInSTBSchemeMaster.ToUpper() == "YES")
                {
                    IsImport = true;
                }

                GridSTBScheme.JSProperties["cpEdit"] = null;
                GridSTBScheme.JSProperties["cpinsert"] = null;
                GridSTBScheme.JSProperties["cpUpdate"] = null;
                GridSTBScheme.JSProperties["cpDelete"] = null;
                GridSTBScheme.JSProperties["cpExists"] = null;
                if (HttpContext.Current.Session["userid"] == null)
                {
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

                Session["exportval"] = null;
            }
            BindProbGrid();
        }

        protected void BindProbGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = STBSchemeBL.GetSTBSchemeList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridSTBScheme.DataSource = dtFillGrid;
                GridSTBScheme.DataBind();
            }
            else
            {
                GridSTBScheme.DataSource = null;
                GridSTBScheme.DataBind();
            }
        }

        protected void btnSearch(object sender, EventArgs e)
        {
            GridSTBScheme.Settings.ShowFilterRow = true;
        }

        protected void GridSTBScheme_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridSTBScheme.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridSTBScheme.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }

        protected void GridSTBScheme_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridSTBScheme.JSProperties["cpEdit"] = null;
            GridSTBScheme.JSProperties["cpinsert"] = null;
            GridSTBScheme.JSProperties["cpUpdate"] = null;
            GridSTBScheme.JSProperties["cpDelete"] = null;
            GridSTBScheme.JSProperties["cpExists"] = null;
            GridSTBScheme.JSProperties["cpImportSTBScheme"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridSTBScheme.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridSTBScheme.FilterExpression = string.Empty;
            }

            if (WhichCall == "Edit")
            {
                DataTable dtEdit = STBSchemeBL.GetSTBSchemeEdit(WhichType);
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string ProbDesc = Convert.ToString(dtEdit.Rows[0]["STBSchemeDesc"]);
                    int ProbId = Convert.ToInt32(dtEdit.Rows[0]["STBSchemeID"]);
                    GridSTBScheme.JSProperties["cpEdit"] = ProbDesc + "~" + ProbId;
                }
            }
            else if (WhichCall == "updateSTBScheme")
            {
                updtcnt = STBSchemeBL.UpdateSTBSchemeMaster(txtSTBSchemeDesc.Text, Session["userid"].ToString(), WhichType);
                if (updtcnt > 0)
                {
                    GridSTBScheme.JSProperties["cpUpdate"] = "Success";
                    BindProbGrid();
                }
                else
                {
                    GridSTBScheme.JSProperties["cpUpdate"] = "fail";
                }
            }
            else if (WhichCall == "saveSTBScheme")
            {
                DataTable dtCheck = STBSchemeBL.GetSTBSchemeChecking(txtSTBSchemeDesc.Text);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    GridSTBScheme.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    insertcount = STBSchemeBL.InsertSTBSchemeMaster(txtSTBSchemeDesc.Text.Trim(), Session["userid"].ToString());
                    if (insertcount > 0)
                    {
                        GridSTBScheme.JSProperties["cpinsert"] = "Success";
                        BindProbGrid();
                    }
                    else
                    {
                        GridSTBScheme.JSProperties["cpinsert"] = "fail";
                    }
                }
            }
            else if (WhichCall == "Delete")
            {
                int id = Convert.ToInt32(WhichType);
                DataTable dt = STBSchemeBL.DeleteSTBSchemeMaster(Convert.ToString(id));
                if (dt != null && dt.Rows.Count > 1)
                {
                    GridSTBScheme.JSProperties["cpDelete"] = dt.Rows[0]["msg"].ToString();
                }
                else
                {
                    GridSTBScheme.JSProperties["cpDelete"] = dt.Rows[0]["msg"].ToString();
                }
            }
            else if (WhichCall == "ImportSTBScheme")
            {
                string ComanyDbName = Convert.ToString(e.Parameters).Split('~')[1];
               
                int i = STBSchemeBL.ImportSTBSchemeMaster(Convert.ToString(ComanyDbName));
                if (i > 0)
                {
                    GridSTBScheme.JSProperties["cpImportSTBScheme"] = "Success";
                }
                else
                {
                    GridSTBScheme.JSProperties["cpImportSTBScheme"] = "fail";
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
            GridSTBScheme.Columns[2].Visible = false;

            string filename = "STBScheme";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "STBScheme";
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
        }

        [WebMethod]
        public static string CheckWorkingRoster(string module_ID)
        {
            CommonBL ComBL = new CommonBL();
            string STBTransactionsRestrictBeyondTheWorkingDays = ComBL.GetSystemSettingsResult("STBTransactionsRestrictBeyondTheWorkingDays");
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    if (STBTransactionsRestrictBeyondTheWorkingDays.ToUpper() == "YES")
                    {
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBModuleRosterStatus");
                        proc.AddPara("@ModuleId", module_ID);
                        DataSet ds = proc.GetDataSet();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "true")
                            {
                                output = "true";
                            }
                            else if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "false")
                            {

                                output = "false~" + ds.Tables[1].Rows[0]["BeginTime"].ToString() + "~" + ds.Tables[1].Rows[0]["EndTime"].ToString();
                            }
                        }
                        else
                        {
                            output = "false";
                        }
                    }
                    else
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
    }
}