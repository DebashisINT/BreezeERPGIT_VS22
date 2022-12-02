using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class SrvMastEntityList : System.Web.UI.Page
    {
        CommonBL Entitycb = new CommonBL();
        public bool IsImport = false;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDbEngine oDBEngineMst = new BusinessLogicLayer.MasterDbEngine();
        BusinessLogicLayer.MasterDataCheckingBL delMasterData = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        Srv_MastEntityBL srvBL = new Srv_MastEntityBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string EntityImportInEntityMaster = Entitycb.GetSystemSettingsResult("EntityImportInEntityMaster");
            if (EntityImportInEntityMaster.ToUpper() == "YES")
            {
                IsImport = true;
            }

            if (Session["ViewMode"] != null)
            {
                if (Session["ViewMode"].ToString() == "view")
                {
                    HttpContext.Current.Session["UserRightSession/management/Master/SrvMastEntityList.aspx"] = null;
                    Session["ViewMode"] = "";
                }
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvMastEntityList.aspx");
            if (!IsPostBack)
            {
                Session["ContactType"] = "TM";
            }
            fillGrid();
        }

        public void fillGrid()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_FetchEntity");
            proc.AddVarcharPara("@Action", 500, "EntityReport");
            proc.AddIntegerPara("@USER_ID", Convert.ToInt32(Session["userid"]));
            DataTable dt = proc.GetTable();
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

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                gridFinancer.JSProperties["cpDelmsg"] = null;
                gridFinancer.JSProperties["cpImportModel"] = null;
                if (CallVal[0] == "Delete")
                {
                    string[,] ContactData;
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + CallVal[1], 1);
                    if (ContactData[0, 0] != "n")
                    {
                        int i = srvBL.DeleteEntityMaster(ContactData[0, 0]);
                        gridFinancer.JSProperties["cpDelmsg"] = "Deleted Successfully.";
                        fillGrid();
                    }
                }
                else if (CallVal[0] == "ImportModel")
                {
                    string ComanyDbName = Convert.ToString(CallVal[1]); 
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
                    int i = srvBL.ImportEntitylMaster(Convert.ToString(ComanyDbName));
                    //if (i > 0)
                    //{
                        gridFinancer.JSProperties["cpImportModel"] = "Success";
                    //}
                    //else
                    //{
                    //    gridFinancer.JSProperties["cpImportModel"] = "fail";
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        protected void AspxExecutiveGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //DataTable dt = oDBEngine.GetDataTable("select ExecutiveName,ExecutiveuserId,ExecutivePassword from tbl_master_FinancerExecutive where Fin_InternalId=( select cnt_internalId from tbl_master_contact  where cnt_id=" + Convert.ToString(e.Parameters) + ")");
            //AspxExecutiveGrid.DataSource = dt;
            //AspxExecutiveGrid.DataBind();
        }
        
        public void bindexport(int Filter)
        {
            //gridFinancer.Columns[3].Visible = false;
            string filename = "Entity";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Entity";
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

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.Entity_Reports
                    where d.LOGIN_ID == userid
                    orderby d.SEQ
                    select d;
            e.QueryableSource = q;
        }


        /*-------------------Tanmoy------------------*/
        MasterSettings masterbl = new MasterSettings();
        public bool STBBranchMap { get; set; }

        [WebMethod]
        public static List<ModuleList> GetModuleList(string EntityId)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            List<ModuleList> omodel = new List<ModuleList>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_ModuleEntityMAP");
            proc.AddPara("@EntityId", EntityId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                omodel = UtilityLayer.APIHelperMethods.ToModelList<ModuleList>(dt);
            }
            return omodel;
        }

        [WebMethod]
        public static bool GetModuleListSubmit(string EntityId, List<string> Modulelist)
        {
            Employee_BL objEmploye = new Employee_BL();
            string ModuleId = "";
            int i = 1;

            if (Modulelist != null && Modulelist.Count > 0)
            {
                foreach (string item in Modulelist)
                {
                    if (item == "0")
                    {
                        ModuleId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            ModuleId = ModuleId + "," + item;
                        else
                            ModuleId = item;
                        i++;
                    }
                }

            }

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBmoduleEntityMapInsertUpdate");
            proc.AddPara("@EntityId", EntityId);
            proc.AddPara("@ModuleId", ModuleId);
            proc.AddPara("@User_id", Convert.ToString(HttpContext.Current.Session["userid"]));
            dt = proc.GetTable();

            return true;
        }

        public class ModuleList
        {
            public long ModuleId { get; set; }
            public String ModuleName { get; set; }
            public bool IsChecked { get; set; }
            public string status { get; set; }
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
        /*-------------------Tanmoy------------------*/
    }
}