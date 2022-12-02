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
    public partial class SrvModelMast : System.Web.UI.Page
    {
        public bool IsImport = false;
        SrvModelMastBL ModelBL = new SrvModelMastBL();
        public string pageAccess = "";
        BusinessLogicLayer.MasterDbEngine oDBEngineMst = new BusinessLogicLayer.MasterDbEngine();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL Modelcb = new CommonBL();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvProbMast.aspx");

            if (!IsPostBack)
            {
                string ModelImportInModelMaster = Modelcb.GetSystemSettingsResult("ModelImportInModelMaster");
                if (ModelImportInModelMaster.ToUpper() == "YES")
                {
                    IsImport = true;
                }

                GridModel.JSProperties["cpEdit"] = null;
                GridModel.JSProperties["cpinsert"] = null;
                GridModel.JSProperties["cpUpdate"] = null;
                GridModel.JSProperties["cpDelete"] = null;
                GridModel.JSProperties["cpExists"] = null;
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
            dtFillGrid = ModelBL.GetModelList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridModel.DataSource = dtFillGrid;
                GridModel.DataBind();
            }
            else
            {
                GridModel.DataSource = null;
                GridModel.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridModel.Settings.ShowFilterRow = true;
        }

        protected void GridModel_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridModel.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridModel.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }

        protected void GridModel_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridModel.JSProperties["cpEdit"] = null;
            GridModel.JSProperties["cpinsert"] = null;
            GridModel.JSProperties["cpUpdate"] = null;
            GridModel.JSProperties["cpDelete"] = null;
            GridModel.JSProperties["cpExists"] = null;
            GridModel.JSProperties["cpImportModel"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridModel.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridModel.FilterExpression = string.Empty;
            }

            if (WhichCall == "Edit")
            {
                DataTable dtEdit = ModelBL.GetModelEdit(WhichType);
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string ProbDesc = Convert.ToString(dtEdit.Rows[0]["ModelDesc"]);
                    int ProbId = Convert.ToInt32(dtEdit.Rows[0]["ModelID"]);
                    GridModel.JSProperties["cpEdit"] = ProbDesc + "~" + ProbId;
                }
            }
            else if (WhichCall == "updateModel")
            {
                updtcnt = ModelBL.UpdateModelMaster(txtModelDesc.Text, Session["userid"].ToString(), WhichType);
                if (updtcnt > 0)
                {
                    GridModel.JSProperties["cpUpdate"] = "Success";
                    BindProbGrid();
                }
                else
                {
                    GridModel.JSProperties["cpUpdate"] = "fail";
                }
            }
            else if (WhichCall == "saveModel")
            {
                DataTable dtCheck = ModelBL.GetModelChecking(txtModelDesc.Text);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    GridModel.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    insertcount = ModelBL.InsertModelMaster(txtModelDesc.Text.Trim(), Session["userid"].ToString());
                    if (insertcount > 0)
                    {
                        GridModel.JSProperties["cpinsert"] = "Success";
                        BindProbGrid();
                    }
                    else
                    {
                        GridModel.JSProperties["cpinsert"] = "fail";
                    }
                }
            }
            else if (WhichCall == "Delete")
            {
                int id = Convert.ToInt32(WhichType);
                DataTable dt = ModelBL.DeleteModelMaster(Convert.ToString(id));
                if (dt != null && dt.Rows.Count > 1)
                {
                    GridModel.JSProperties["cpDelete"] = dt.Rows[0]["msg"].ToString();
                }
                else
                {
                    GridModel.JSProperties["cpDelete"] = dt.Rows[0]["msg"].ToString();
                }
            }
            else if (WhichCall == "ImportModel")
            {
                string ComanyDbName = Convert.ToString(e.Parameters).Split('~')[1]; 
                //List<object> ComanyDbNameList = lookup_company.GridView.GetSelectedFieldValues("DbName");

                //foreach (object[] item in ComanyDbNameList)
                //{
                //    ComanyDbName = item[0].ToString();
                //}

                //string CompanyCode = Convert.ToString(lookup_company.Value);
                //DataTable dtCompany = oDBEngineMst.GetDataTable("select DbName DbName,Name Company_Name,Company_Code from ERP_Company_List where IsActive=1 and Company_Code='" + CompanyCode + "'");
                //if (dtCompany.Rows.Count > 0)
                //{
                //    ComanyDbName = Convert.ToString(dtCompany.Rows[0]["DbName"]);
                //}
                int i = ModelBL.ImportModelMaster(Convert.ToString(ComanyDbName));
                if (i > 0)
                {
                    GridModel.JSProperties["cpImportModel"] = "Success";
                }
                else
                {
                    GridModel.JSProperties["cpImportModel"] = "fail";
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
            GridModel.Columns[2].Visible = false;

            string filename = "Model";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Model";
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

        //protected void ComponentCompany_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.Split('~')[0] == "BindCompanyGrid")
        //    {
        //        DataTable dtCompany = oDBEngineMst.GetDataTable("select DbName DbName,Name Company_Name,Company_Code from ERP_Company_List where IsActive=1 and Company_Code!='" + Convert.ToString(Session["LastCompany"]) + "'");
        //        if (dtCompany.Rows.Count > 0)
        //        {
        //            Session["SI_ComponentData_Company"] = dtCompany;

        //            lookup_company.DataSource = dtCompany;
        //            lookup_company.DataBind();
        //        }
        //        else
        //        {
        //            Session["SI_ComponentData_Company"] = dtCompany;
        //            lookup_company.DataSource = null;
        //            lookup_company.DataBind();
        //        }
        //    }
        //}

        //protected void lookup_company_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData_Company"] != null)
        //    {
        //        lookup_company.DataSource = (DataTable)Session["SI_ComponentData_Company"];
        //    }
        //}

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