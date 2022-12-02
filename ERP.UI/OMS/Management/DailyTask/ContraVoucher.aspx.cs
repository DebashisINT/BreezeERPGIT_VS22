using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections;
using System.Configuration;
using System.Data;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System.Threading.Tasks;
using ERP.Models;
namespace ERP.OMS.Management.DailyTask
{
    public partial class ContraVoucher : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        DataTable DtCurrentSegment;
        BusinessLogicLayer.GenericLogSystem oGenericLogSystem;
        FinancialAccounting oFinancialAccounting = new FinancialAccounting();
        BusinessLogicLayer.DBEngine oDbEngine;

        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        string[] lengthIndex;
        string JVNumStr = string.Empty;
        static string ForJournalDate = null;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Rev Debashis
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
            //End of Rev Debashis
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/dailytask/ContraVoucher.aspx");
            oGenericLogSystem = new BusinessLogicLayer.GenericLogSystem();

            CommonBL cbl = new CommonBL();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string SubledgerCashBankTypeCheck = cbl.GetSystemSettingsResult("SubledgerCashBankType");
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");

            string AllowProjectInDetailsLevel = cbl.GetSystemSettingsResult("AllowProjectInDetailsLevel");
            Session["Vouchwernumber"] = null;
            //grid.Columns[8].Width = 0;

            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");

            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";
                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";
                }
            }


            DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=53");

            if (dtposTime != null && dtposTime.Rows.Count > 0)
            {
                hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
            }


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=53");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=53");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "DATA is Freezed between   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + " for Edit. ";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                spnDeleteLock.InnerText = spnEditLock.InnerText + "DATA is Freezed between   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + "  for Delete.";
                spnEditLock.InnerText = "";
            }            
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
                {
                    if (ProjectSelectInEntryModule == "Yes")
                    {
                        hdnProjectSelectInEntryModule.Value = "1";
                        lookup_Project.ClientVisible = true;
                        lblProject.Visible = true;
                    }
                    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                        hdnProjectSelectInEntryModule.Value = "0";
                        lookup_Project.ClientVisible = false;
                        lblProject.Visible = false;                        
                    }
                }
                if (!String.IsNullOrEmpty(AllowProjectInDetailsLevel))
                {
                    if (AllowProjectInDetailsLevel.ToUpper().Trim() == "NO")
                    {
                        hdnAllowProjectInDetailsLevel.Value = "0";
                        grid.Columns[8].Width = 0;
                    }
                }
                if (!String.IsNullOrEmpty(SubledgerCashBankTypeCheck))
                {
                    if (SubledgerCashBankTypeCheck == "Yes")
                    {
                        hdnSubledgerCashBankType.Value = "1";
                    }
                    else if (SubledgerCashBankTypeCheck.ToUpper().Trim() == "NO")
                    {
                        grid.Columns[1].Width = 0;
                        grid.Columns[3].Width = 0;
                        hdnSubledgerCashBankType.Value = "0";
                    }
                }
                if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
                {
                    if (ProjectSelectInEntryModule == "Yes")
                    {
                        Grid_ContraVoucher.Columns[5].Visible = true;
                    }
                    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                        Grid_ContraVoucher.Columns[5].Visible = false;
                    }
                }
                //For Hierarchy Start Tanmoy
                if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
                {
                    if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                    {
                        ddlHierarchy.Visible = true;
                        lblHierarchy.Visible = true;
                        lookup_Project.Columns[3].Visible = true;
                    }
                    else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                        ddlHierarchy.Visible = false;
                        lblHierarchy.Visible = false;
                        lookup_Project.Columns[3].Visible = false;
                    }
                }
                //For Hierarchy End Tanmoy

                String finyear = "";
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                Session.Remove("exportval");
                //Session.Remove("ContraListingDetails");
                string[] FinYEnd = Session["FinYearEnd"].ToString().Split(' ');
                string FinYearEnd = FinYEnd[0];
                ViewState["LogID"] = oGenericLogSystem.GetLogID();
                string strdefaultBranch = "";
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                }               
                Task PopulateStockTrialDataTask = new Task(() => BindAllControlDataOnPageLoad(FinYearEnd, strdefaultBranch));
                PopulateStockTrialDataTask.RunSynchronously(); 
                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                //Rev Debashis
                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    btncross.Visible = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>zoomcontravoucher(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                }
                //End of Rev Debashis
            }
        }

        #region Others
        /*Mantis Issue:- 0024782 Swati  Rev work Copy feature Add 25.03.2022*/
        [WebMethod]
        public static object GetNumberingSchemeByType(string id, string Type)
        {
            //string Type = "Copy";
            string BranchID = "";
            if (Type == "Copy")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtBranch = oDBEngine.GetDataTable("SELECT  CashBank_BranchID from Trans_CashBankVouchers WHERE  CashBank_ID='" + id + "'");
                if (dtBranch.Rows.Count > 0)
                {
                    // HttpContext.Current.Session["BranchID"] = dtBranch.Rows[0]["CashBank_BranchID"].ToString();
                    BranchID = dtBranch.Rows[0]["CashBank_BranchID"].ToString();
                }
            }
            else
            {
                BranchID = HttpContext.Current.Session["userbranchHierarchy"].ToString();
            }
            DataSet Schemadt = NumberingSchemData(Type, BranchID);

            DataTable numberingschemeData = Schemadt.Tables[3];
            AllddlCustNote All = new AllddlCustNote();
            All.NumberingSchema = (from DataRow dr in numberingschemeData.Rows
                                   select new ddlNumberingSchema()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();

            return All;
        }
        public class ddlNumberingSchema
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public class AllddlCustNote
        {
            public List<ddlNumberingSchema> NumberingSchema { get; set; }            
        }
        public static DataSet NumberingSchemData(string Type, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@BranchList", 4000, BranchID);            
            ds = proc.GetDataSet();
            return ds;
        }
        /* Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022 */
        public void BindAllControlDataOnPageLoad(string FinYearEnd, string strdefaultBranch)
        {
            this.Page.ClientScript.RegisterStartupScript(GetType(), "CS", "<script>PageLoad();</script>");

            GetAllDropDownDetailForCashBank();
            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            // Bind_NumberingScheme();  
            //oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            oDbEngine = new BusinessLogicLayer.DBEngine();

            // DtCurrentSegment = oDbEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId),exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");

            hdn_CurrentSegment.Value = "1";
            tDate.EditFormatString = objConverter.GetDateFormat("Date");
            InstDate.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;
            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ForJournalDate = date3.ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;
            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToDateTime(ForJournalDate).Month.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Day.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Year.ToString();
            }
            else
            {
                fDate = Convert.ToDateTime(oDBEngine.GetDate().Date).Month.ToString() + "/" + Convert.ToDateTime(oDBEngine.GetDate().Date).Day.ToString() + "/" + Convert.ToDateTime(oDBEngine.GetDate().Date).Year.ToString();
            }
            tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            InstDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ddlBranch.SelectedValue = strdefaultBranch;
            // FillContraGrid();
            //BindBranchFrom();
        }
        public void GetAllDropDownDetailForCashBank()
        {
            DataSet dst = new DataSet();
            dst = AllDropDownDetailForCashBank();
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataTextField = "BANKBRANCH_NAME";
                ddlBranch.DataValueField = "BANKBRANCH_ID";
                ddlBranch.DataSource = dst.Tables[0];
                ddlBranch.DataBind();

                ddlBranchTo.DataTextField = "BANKBRANCH_NAME";
                ddlBranchTo.DataValueField = "BANKBRANCH_ID";
                ddlBranchTo.DataSource = dst.Tables[0];
                ddlBranchTo.DataBind();


            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataSource = dst.Tables[1];
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;
                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }
            //Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022
           /* if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "ID";
                CmbScheme.DataSource = dst.Tables[3];
                CmbScheme.DataBind();  
            }*/
            //Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022
        }
        public DataSet AllDropDownDetailForCashBank()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            //proc.AddVarcharPara("@FinYear", 50, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            // Rev Sanchita [ Above lines got commented by mistake ]
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            // End of Rev Sanchita
            //Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022
            //proc.AddVarcharPara("@BranchList", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            if (hdnType.Value == "Copy")
            {
                proc.AddVarcharPara("@BranchList", 4000, Convert.ToString(HttpContext.Current.Session["BranchID"]));
            }
            else
            {
                proc.AddVarcharPara("@BranchList", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            }
            //Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022
            ds = proc.GetDataSet();
            return ds;
        }
        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            DataTable branchtable = getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        public DataTable getBranchListByHierchy(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 4000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }
        public void Bind_NumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "6", "N");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "Id";
                CmbScheme.DataSource = Schemadt;
                CmbScheme.DataBind();
            }
        }
        public void FillContraGrid()
        {
            DataSet dsdata = Get_ContraData();
            if (dsdata != null && dsdata.Tables.Count > 0)
            {
                Grid_ContraVoucher.DataSource = dsdata.Tables[0];
                Grid_ContraVoucher.DataBind();
            }
            else
            {
                Grid_ContraVoucher.DataSource = null;
                Grid_ContraVoucher.DataBind();
            }
        }
        public DataSet Get_ContraData()
        {
            ViewState["WhichSegment"] = "1";
            string BranchID = Convert.ToString(cmbBranchfilter.Value);
            string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
            string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");

            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            string finyear = Convert.ToString(Session["LastFinYear"]);

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "ContraVoucherList");
            proc.AddVarcharPara("@FinYear", 500, finyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@BranchList", 4000, userbranch);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable Get_ContraData(string userbranchlist, string lastCompany, string finyear, string BranchID, string FromDate, string ToDate)
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "ContraVoucherList");
            proc.AddVarcharPara("@FinYear", 500, finyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@BranchList", 4000, userbranchlist);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            ds = proc.GetTable();
            return ds;
        }
        public void bindexport(int Filter)
        {
            Grid_ContraVoucher.Columns[8].Visible = false;
            exporter.GridViewID = "Grid_ContraVoucher";
            string filename = "ContraVoucher";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Contra Voucher";
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
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void EntityServerModeDataSourceProject_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

           
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
       


            //DataTable ds = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            //proc.AddVarcharPara("@Action", 100, "CashBankProjectEditDetails");
            //proc.AddVarcharPara("@CashBank_ID", 100, Convert.ToString(Session["CashBank_ID"]));
            //ds = proc.GetTable();

            //List<V_ProjectList> ProjectList = new List<V_ProjectList>();

            //ProjectList = (from DataRow row in ds.Rows

            //               select new V_ProjectList
            //       {
            //           Proj_Code = row["ProjectCode"].ToString(),
            //           Proj_Id = Convert.ToInt64(row["Proj_Id"]),
            //           Customer = row["Customer"].ToString(),
            //           IsInUse = false



            //       }).ToList();

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(hddn_BranchID.Value)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        private void BindWithdrawalForm(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetMainAccountByBranch(e.Parameter);
            currentCombo.DataBind();
        }
        private void BindDepositInto(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetDepositInto(e.Parameter);
            currentCombo.DataBind();
        }
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            // oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDbEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);


                    if (!suppressZero)
                    {

                        sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CashBank_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDbEngine.GetDataTable(sqlQuery);





                    }

                    else
                    {
                        int i = startNo.Length;
                        while (i < paddCounter)
                        {
                            

                            sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery+= " and LEN(tjv.CashBank_VoucherNumber)="+i;
                            }

                            dtC = oDBEngine.GetDataTable(sqlQuery);
                            if (dtC.Rows[0][0].ToString() == "")
                            {
                                break;
                            }
                            i++;
                        }
                        if (i != 1)
                        {
                            sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + (i - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.CashBank_VoucherNumber)=" + (i-1);
                            }
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                    }

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CashBank_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDbEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            if (!suppressZero)
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            else
                                paddedStr = EmpCode.ToString();
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        if (!suppressZero)
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                        else
                            paddedStr = startNo;
                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT CashBank_VoucherNumber FROM Trans_CashBankVouchers WHERE CashBank_VoucherNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDbEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }
        protected void btnnew_Click(object sender, EventArgs e)
        {
            if (hdnVoucherNo.Value != null)
            {
                txtVoucherNo.Text = Convert.ToString(hdnVoucherNo.Value).Trim();
            }


            if (hdn_Mode.Value == "EDIT")
            {
                grid.JSProperties["cpModify"] = null;
                Modify();
                grid.JSProperties["cpModify"] = "Modify Successfully";
                setrefreshModify();

                //Response.Redirect("ContraVoucher.aspx");
            }
            else
            {
                string validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(CmbScheme.SelectedItem.Value));
                if (validate == "outrange")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('outrange')</script>");
                    return;
                }
                else if (validate == "duplicate")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('duplicate')</script>");
                    return;
                }
                else
                {
                    txtVoucherNo.Text = JVNumStr;
                }
                Add();
                SetRefresh();
                hdn_Mode.Value = "";
                hdn_Mode.Value = "";

            }


        }
        protected void Add()
        {
            Int64 ProjectId = 0;
            string formMode = "Entry";
            string WDrawFrom = hdnWithDrawFrom.Value;
            string WithdrawlSubledger = hdnWithDrawlLedgerName.Value;
            string DInto = hdnDepositInto.Value;
            string DepositLedger=hdnDedpositLedgerName.Value;
            string Currency = hdnCurrency_ID.Value;
            if (hdnProjectSelectInEntryModule.Value == "1")
            {
                if (hdnInlineProjId.Value != "")
                {
                    ProjectId = Convert.ToInt64(hdnInlineProjId.Value);
                }
            }
            if (Currency == "" || Currency == "0")
            {
                string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                string[] basedCurrency = LocalCurrency.Split('~');
                Currency = basedCurrency[0];
            }
            string rate = hdnRate.Value;
            if (rate == "")
            {
                rate = "0.0";
            }
            string amount = hdnAmount.Value;
            if (amount == "")
            {
                amount = "0.0";
            }
            string AmountInHomeCurrency = hdnAmountInHomeCurrency.Value;
            string Remarks = hdnRemarks.Value;
            Int64 ProjId;
            if (lookup_Project.Text != "")
            {
                string projectCode = lookup_Project.Text;
                DataTable dt = oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
            }
          
           else
            {
                ProjId = 0;
            }
            string OldIBRef = string.Empty;
            OldIBRef = Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty;
            string ExchangeSegmentID = string.Empty;
            ExchangeSegmentID = hdn_CurrentSegment.Value;

            DataSet dsInst = new DataSet();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand cmd = new SqlCommand("prc_ContraVoucher", con);
            cmd.CommandType = CommandType.StoredProcedure;


            Int32 Branch;
            if (ddlBranch.SelectedValue != "")
            {

                Branch = Convert.ToInt32(hddn_BranchID.Value);
            }
            else
            {
                Branch = 0;
            }
            cmd.Parameters.AddWithValue("@FormMode", formMode);
            cmd.Parameters.AddWithValue("@VoucherNumber", txtVoucherNo.Text.Trim());
            cmd.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
            string com = HttpContext.Current.Session["LastCompany"].ToString();
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@TransactionDate", tDate.Date);
            cmd.Parameters.AddWithValue("@ExchangeSegmentID", ExchangeSegmentID);
            cmd.Parameters.AddWithValue("@TransactionType", 'C');
            cmd.Parameters.AddWithValue("@EntryUserProfile", Session["EntryProfileType"].ToString());
            cmd.Parameters.AddWithValue("@Narration", txtNarration.Text.Trim());

            cmd.Parameters.AddWithValue("@BranchID", Branch);
            string BranchTo = ddlBranchTo.SelectedValue;
            if (hdnBranchIdTo.Value == "")
            {
                hdnBranchIdTo.Value = BranchTo;
            }

            cmd.Parameters.AddWithValue("@BranchIDTo", Convert.ToInt32(hdnBranchIdTo.Value));

            cmd.Parameters.AddWithValue("@WithDrawFrom", WDrawFrom);

            cmd.Parameters.AddWithValue("@WithdrawlSubledger", WithdrawlSubledger);

            cmd.Parameters.AddWithValue("@DepositInto", DInto);
            cmd.Parameters.AddWithValue("@DepositLedger", DepositLedger);
            cmd.Parameters.AddWithValue("@AmountInHomeCurrency", Convert.ToDecimal(AmountInHomeCurrency));
            cmd.Parameters.AddWithValue("@CurrencyID", Convert.ToInt32(Currency));
            cmd.Parameters.AddWithValue("@InstrumentType", ComboInstType.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@InstrumentNumber", txtInstNo.Text.Trim());
            cmd.Parameters.AddWithValue("@InstrumentDate", InstDate.Date);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            //cmd.Parameters.AddWithValue("@CaskBank_NumberingScheme", Convert.ToInt32(CmbScheme.SelectedItem.Value));
            /* Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022*/
            //cmd.Parameters.AddWithValue("@CaskBank_NumberingScheme", Convert.ToInt32(CmbScheme.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@CaskBank_NumberingScheme", Convert.ToInt32(hdnScemeID.Value));
            /* Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022 */
            cmd.Parameters.AddWithValue("@CashBankDetail_Rate", Convert.ToDecimal(rate));
            cmd.Parameters.AddWithValue("@CashBankDetail_Amount", Convert.ToDecimal(amount));
            cmd.Parameters.AddWithValue("@Project_Id", ProjId);
            cmd.Parameters.AddWithValue("@InlineProjectId", ProjectId);
           
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

          

            cmd.Dispose();
            con.Dispose();
             grid.JSProperties["cprtnVoucherNo"] = "Voucher No. Generated- " + txtVoucherNo.Text.Trim();
             txtVoucherNo.Text = "";
            txtInstNo.Text = "";
             txtNarration.Text = "";
          

        }
        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForContra");

            dt = proc.GetTable();
            return dt;

        }
        protected void Modify()
        {
            Int64 ProjectId = 0;
            string formMode = "EDIT";
            string WDrawFrom = hdnWithDrawFrom.Value;
            string DInto = hdnDepositInto.Value;
            string WithdrawlSubledger = hdnWithDrawlLedgerName.Value;
          
            string DepositLedger = hdnDedpositLedgerName.Value;
            //if (hdnProjectSelectInEntryModule.Value == "1")
            // ProjectId = Convert.ToInt64(hdnInlineProjId.Value);
            if (hdnProjectSelectInEntryModule.Value == "1")
            {
                if (hdnInlineProjId.Value != "")
                {
                    ProjectId = Convert.ToInt64(hdnInlineProjId.Value);
                }
            }
            Session["Vouchwernumber"] = hdn_CashBankID.Value;
            string Currency;

            if (hdnCurrency_ID.Value != "0")
            {
                Currency = hdnCurrency_ID.Value;
            }
            else
            {
                Currency = hdnDbSaveCurrenct.Value;
            }
            string rate = hdnRate.Value;
            if (rate == "")
            {
                rate = "0.0";
            }
            string amount = hdnAmount.Value;
            if (amount == "")
            {
                amount = "0.0";
            }
            string AmountInHomeCurrency = hdnAmountInHomeCurrency.Value;
            string Remarks = hdnRemarks.Value;

            string OldIBRef = string.Empty;
            OldIBRef = Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty;
            string ExchangeSegmentID = string.Empty;
            ExchangeSegmentID = hdn_CurrentSegment.Value;

            DataSet dsInst = new DataSet();

            // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_ContraVoucherModify", con);
            cmd.CommandType = CommandType.StoredProcedure;

            Int32 Branch;
            if (ddlBranch.SelectedValue != "")
            {
                // Branch = Convert.ToInt32(ddlBranch.SelectedValue);
                Branch = Convert.ToInt32(hddn_BranchID.Value);
            }
            else
            {
                Branch = 0;
            }

             CommonBL cbl = new CommonBL();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");


           
            Int64 ProjId=0;
            if (lookup_Project.Text != "")
            {
                string projectCode = lookup_Project.Text;
                DataTable dt = GetProjectCode(projectCode);
                    //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
            }
            else if (lookup_Project.Text=="")
            {
                //if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                //{
                //    DataTable dtproj = GetProjectEditData(hdn_CashBankID.Value);
                //    //oDbEngine.GetDataTable("select Project_Id from Trans_transactionprojectMapping where cashbank_Id='1375'");
                //    if (dtproj != null)
                //    {
                //        ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                //    }
                //    else
                //    {
                //        ProjId = 0;
                //    }
                //}
                //else
                //{
                    ProjId = 0;
                //}
               
            }

            else
            {
                ProjId = 0;
            }


            cmd.Parameters.AddWithValue("@FormMode", formMode);
            cmd.Parameters.AddWithValue("@CashBank_ID", Convert.ToInt32(hdn_CashBankID.Value));
            cmd.Parameters.AddWithValue("@VoucherNumber", txtVoucherNo.Text.Trim());
            cmd.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            cmd.Parameters.AddWithValue("@FinYear", hdnCashBank_FinYear.Value);
            cmd.Parameters.AddWithValue("@CompanyID", hdnCashBank_CompanyID.Value);
            cmd.Parameters.AddWithValue("@TransactionDate", tDate.Date);
            cmd.Parameters.AddWithValue("@ExchangeSegmentID", hdnCashBank_ExchangeSegmentID.Value);
            cmd.Parameters.AddWithValue("@TransactionType", 'C');
            cmd.Parameters.AddWithValue("@EntryUserProfile", Session["EntryProfileType"].ToString());
            cmd.Parameters.AddWithValue("@Narration", txtNarration.Text.Trim());
            cmd.Parameters.AddWithValue("@BranchID", Branch);
            cmd.Parameters.AddWithValue("@BranchIDTo", Convert.ToInt32(hdnBranchIdTo.Value));
            cmd.Parameters.AddWithValue("@WithDrawFrom", WDrawFrom);
            cmd.Parameters.AddWithValue("@WithdrawlSubledger", WithdrawlSubledger);
            cmd.Parameters.AddWithValue("@DepositLedger", DepositLedger);
            cmd.Parameters.AddWithValue("@DepositInto", DInto);
            cmd.Parameters.AddWithValue("@AmountInHomeCurrency", Convert.ToDecimal(AmountInHomeCurrency));
            cmd.Parameters.AddWithValue("@CurrencyID", Convert.ToInt32(Currency));
            cmd.Parameters.AddWithValue("@InstrumentType", ComboInstType.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@InstrumentNumber", txtInstNo.Text.Trim());
            cmd.Parameters.AddWithValue("@InstrumentDate", InstDate.Date);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            // cmd.Parameters.AddWithValue("@CaskBank_NumberingScheme", CmbScheme.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@CashBank_IBRef", hdnCashBank_IBRef.Value);
            cmd.Parameters.AddWithValue("@CashBankDetail_Rate", Convert.ToDecimal(rate));
            cmd.Parameters.AddWithValue("@CashBankDetail_Amount", Convert.ToDecimal(amount));
            cmd.Parameters.AddWithValue("@Project_Id", ProjId);
            cmd.Parameters.AddWithValue("@InlineProjectId", ProjectId);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
        }
        protected void btnexit_Click(object sender, EventArgs e)
        {
            if (hdn_Mode.Value == "EDIT")
            {
                Modify();
                Response.Redirect("ContraVoucher.aspx");
            }
            else
            {
                string validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(CmbScheme.SelectedItem.Value));
                if (validate == "outrange")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('outrange')</script>");
                    return;
                }
                else if (validate == "duplicate")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('duplicate')</script>");
                    return;
                }
                else
                {
                    txtVoucherNo.Text = JVNumStr;
                }
                Add();
                Response.Redirect("ContraVoucher.aspx");
                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>OnAddButtonClick()</script>");
            }
            hdn_Mode.Value = "";
        }
        public void SetRefresh()
        {
            int scheme_type = 0;

            DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.SelectedItem.Value));
            scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);


            if (scheme_type == 1)
            {

            }
            else
            {
                CmbScheme.SelectedIndex = 0;
                txtVoucherNo.Text = "";
            }


            ddlBranch.SelectedValue = Convert.ToString(hddn_BranchID.Value);
            ComboInstType.SelectedIndex = 0;
            txtInstNo.Text = "";
            txtNarration.Text = "";

            grid.DataSource = null;
            grid.DataBind();

            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>AddNewRowGrid()</script>");
        }
        public void setrefreshModify()
        {
            CmbScheme.SelectedIndex = 0;
            txtVoucherNo.Text = "";


            ddlBranch.SelectedIndex = -1;
            ComboInstType.SelectedIndex = -1;
            txtInstNo.Text = "";
            txtNarration.Text = "";

            grid.DataSource = null;
            grid.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>AddNewRowGridModify()</script>");

        }
        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (hdnVoucherNo.Value != null)
            {
                txtVoucherNo.Text = Convert.ToString(hdnVoucherNo.Value).Trim();
            }

            if (hdn_Mode.Value == "EDIT")
            {
                Modify();
                Response.Redirect("ContraVoucher.aspx");
            }
            else
            {
                /*Mantis Issue:- 0024782 Swati  Rev work Copy feature Add 25.03.2022*/
                //string validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(CmbScheme.SelectedItem.Value));
                string validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(hdnScemeID.Value));
                /* Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022 */
                if (validate == "outrange")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('outrange')</script>");
                    return;
                }
                else if (validate == "duplicate")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>chkValidConta('duplicate')</script>");
                    return;
                }
                else
                {
                    txtVoucherNo.Text = JVNumStr;
                }
                string finalVNo = txtVoucherNo.Text;
                Add();
               TblSearch.Attributes.Add("style", "display:none");
               divAddNew.Attributes.Add("style", "display:block");
               btncross.Attributes.Add("style", "display:block");
                    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "jAlert('Contra Voucher No. " + finalVNo + " generated.');window.location='ContraVoucher.aspx';", true);

               ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "jAlert('Contra Voucher No. " + finalVNo + " generated.', 'Alert Dialog: [ContraVoucher]', function (r) {if (r == true) {  window.location.assign('ContraVoucher.aspx');} });", true);
              
            }
        }

        #endregion Others

        #region  Main Grid Related

        //protected void Grid_ContraVoucher_DataBinding(object sender, EventArgs e)
        //{            
        //    DataTable dsdata = (DataTable)Session["ContraListingDetails"];
        //    Grid_ContraVoucher.DataSource = dsdata;
        //}
        protected void Grid_ContraVoucher_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Grid_ContraVoucher.JSProperties["cpCBDelete"] = null;
            string[] command = e.Parameters.ToString().Split('~');
            if (command[0] == "Delete")
            {
                int RowUpdated = 0;
                Int32 RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string IBRef = e.Parameters.Split('~')[2];

                string strLogID = ViewState["LogID"].ToString();
                try
                {

                    RowUpdated = oFinancialAccounting.DeleteCB(IBRef, Convert.ToInt32(Session["userid"].ToString()));
                    if (RowUpdated > 0)
                    {
                        Grid_ContraVoucher.JSProperties["cpCBDelete"] = "Successfully Deleted";
                        string BranchID = Convert.ToString(cmbBranchfilter.Value);
                        string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");

                        string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                        string finyear = Convert.ToString(Session["LastFinYear"]);

                        //DataTable dtdata = new DataTable();
                        //Session["ContraListingDetails"] = Get_ContraData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                        //Grid_ContraVoucher.DataBind();

                        oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", BusinessLogicLayer.GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, BusinessLogicLayer.GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", BusinessLogicLayer.GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, BusinessLogicLayer.GenericLogSystem.LogType.CB);
                    }
                    else
                    {

                    }
                }
                catch
                {
                    oGenericLogSystem.CreateLog("", "", BusinessLogicLayer.GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, BusinessLogicLayer.GenericLogSystem.LogType.CB);
                    oGenericLogSystem.CreateLog("", "", BusinessLogicLayer.GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, BusinessLogicLayer.GenericLogSystem.LogType.CB);
                }
            }
            else if (command[0] == "FilterGridByDate")
            {
                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataTable dtdata = new DataTable();
                Session["ContraListingDetails"] = Get_ContraData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                Grid_ContraVoucher.DataBind();

            }


        }
        protected void Grid_ContraVoucher_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        }

        #endregion

        #region  Batch Grid Related
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "WithDrawFrom")
            {
                ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(BindWithdrawalForm);
            }
            if (e.Column.FieldName == "DepositInto")
            {
                ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(BindWithdrawalForm);
            }
            e.Editor.ReadOnly = false;

            if (e.Column.FieldName == "WithdrawlSubLedger")
            {
                e.Editor.ReadOnly = true;  
            }

            if (e.Column.FieldName == "DepositSubLedger")
            {
                e.Editor.ReadOnly = true;  
            }
            if (hdnProjectSelectInEntryModule.Value == "1")
            {
                if (e.Column.FieldName == "Project_Code")
                {
                    e.Editor.ReadOnly = true;
                }
            }
           
        }
        protected void grid_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
            string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
            string CurrencyId = Convert.ToString(basedCurrency[0]);
            e.NewValues["Currency_ID"] = CurrencyId;

        }
        protected void grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        }
        public DataTable GetBatchGridEditData(string contraid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "BatchGridEditList");
            proc.AddIntegerPara("@Contra_ID", Convert.ToInt32(contraid));
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetBatchGridEditDataprojectContra(string contraid)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "BatchGridEditContraList");
            proc.AddIntegerPara("@Contra_ID", Convert.ToInt32(contraid));
            dt = proc.GetDataSet();
            return dt;
        }

        public DataTable GetProjectEditData(string contraid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddIntegerPara("@Contra_ID", Convert.ToInt32(contraid));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpEdit"] = "";
            string[] CallVal = e.Parameters.ToString().Split('~');
            lengthIndex = e.Parameters.Split('~');
            if (lengthIndex[0].ToString() == "BEFORE_EDIT")
            {
                hdn_Mode.Value = "";
                hdn_Mode.Value = "Edit";
                string contraid = lengthIndex[1].ToString();
                DataSet dsgrid = new DataSet();
                dsgrid = GetBatchGridEditDataprojectContra(contraid);

                DataTable DT = new DataTable();
                DataTable dtt = new DataTable();
                DT.Rows.Clear();

                //string strQuery = "Select Convert(nvarchar(10),CashBank_TransactionDate,120) as CashBank_TransactionDate,CashBank_VoucherNumber,CashBank_Narration," +                  
                //    "(Select IsNull(Sum(CashBankDetail_PaymentAmount),0) from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID)as CashBankDetail_PaymentAmount," +
                //    "(SELECT RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where branch_id=tc.CashBank_BranchID)as CashBank_Name,CashBank_BranchID,CashBank_BranchIDTO," +
                //    "CashBank_Currency,"+
                //    "(Select  CashBankDetail_MainAccountID from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID and CashBankDetail_PaymentAmount>0) as WithDrawFrom," +
                //    "(Select CashBankDetail_MainAccountID from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID and CashBankDetail_ReceiptAmount>0) as DepositInto" +                
                //    ", (select top 1 CashBankDetail_InstrumentType from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID)as CashBankDetail_InstrumentType" +
                //    ",(select top 1 CashBankDetail_InstrumentNumber from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID) as CashBankDetail_InstrumentNumber" +
                //    ",(select top 1 Convert(Date,CashBankDetail_InstrumentDate) from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID)as CashBankDetail_InstrumentDate" +
                //    ",(select top 1 CashBankDetail_Narration from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID)as Remarks,CashBank_IBRef,CashBank_FinYear" +
                //    ",CashBank_CompanyID,CashBank_ExchangeSegmentID" +
                //    ",CaskBank_NumberingScheme,(select top 1 CashBankDetail_Rate from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID) as Rate," +
                //    " (select top 1 CashBankDetail_Amount from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID) as Amount" +
                //    " from Trans_CashBankVouchers tc  where CashBank_ID=" + contraid;                            

                //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
                //DT = oDbEngine.GetDataTable(strQuery);
                Int64 ProjId=0;

                //lookup_Project.ClientEnabled = false;
                //lookup_Project.ClearButton.Visibility = AutoBoolean.False;

                DT = dsgrid.Tables[0];
                DataTable dtProjectgrid = dsgrid.Tables[1];
                dtt = GetProjectEditData(contraid);
                if (dtt!=null)
                {
                    ProjId = Convert.ToInt64(dtt.Rows[0]["Proj_Id"]);

                }
                string CashBank_TransactionDate = Convert.ToString(DT.Rows[0]["CashBank_TransactionDate"]);
                string CashBank_VoucherNumber = Convert.ToString(DT.Rows[0]["CashBank_VoucherNumber"]).Trim();
                string CashBank_Narration = Convert.ToString(DT.Rows[0]["CashBank_Narration"]);
                string CashBankDetail_PaymentAmount = Convert.ToString(DT.Rows[0]["CashBankDetail_PaymentAmount"]);
                string CashBank_BranchID = Convert.ToString(DT.Rows[0]["CashBank_BranchID"]);
                string CashBank_Currency = Convert.ToString(DT.Rows[0]["CashBank_Currency"]);//5
                string CashBankDetail_InstrumentType = Convert.ToString(DT.Rows[0]["CashBankDetail_InstrumentType"]);//6
                string CashBankDetail_InstrumentNumber = Convert.ToString(DT.Rows[0]["CashBankDetail_InstrumentNumber"]);//7
                string CashBankDetail_InstrumentDate = Convert.ToString(DT.Rows[0]["CashBankDetail_InstrumentDate"]);//9                
                string Remarks = Convert.ToString(DT.Rows[0]["Remarks"]);//8
                if (dtt != null)
                {
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End
                }
                string CashBank_IBRef = Convert.ToString(DT.Rows[0]["CashBank_IBRef"]);//10
                string CashBank_FinYear = Convert.ToString(DT.Rows[0]["CashBank_FinYear"]);//11
                string CashBank_CompanyID = Convert.ToString(DT.Rows[0]["CashBank_CompanyID"]);//12
                string CashBank_ExchangeSegmentID = Convert.ToString(DT.Rows[0]["CashBank_ExchangeSegmentID"]);//13
                string CaskBank_NumberingScheme = Convert.ToString(DT.Rows[0]["CaskBank_NumberingScheme"]);//14
                string Rate = Convert.ToString(DT.Rows[0]["Rate"]);//15
                string Amount = Convert.ToString(DT.Rows[0]["Amount"]);//16   
                string CashBank_BranchIDTO = Convert.ToString(DT.Rows[0]["CashBank_BranchIDTO"]);//17

                //Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022
              /*  grid.JSProperties["cpEdit"] = CashBank_TransactionDate + "~" + CashBank_VoucherNumber + "~" + CashBank_Narration + "~" +
                  CashBankDetail_PaymentAmount + "~" + CashBank_BranchID + "~" + CashBank_Currency + "~" + CashBankDetail_InstrumentType +
                  "~" + CashBankDetail_InstrumentNumber.Trim() + "~" + Remarks + "~" + CashBankDetail_InstrumentDate + "~" + CashBank_IBRef + "~" + CashBank_FinYear + "~"
                  + CashBank_CompanyID + "~" + CashBank_ExchangeSegmentID + "~" + CaskBank_NumberingScheme + "~" + Rate + "~" + Amount + "~" + CashBank_BranchIDTO + "~" + ProjId;
                */
                if (hdnType.Value=="Copy")
                {
                     grid.JSProperties["cpEdit"] = CashBank_TransactionDate + "~" + CashBank_VoucherNumber + "~" + CashBank_Narration + "~" +
                  CashBankDetail_PaymentAmount + "~" + CashBank_BranchID + "~" + CashBank_Currency + "~" + CashBankDetail_InstrumentType +
                  "~" + CashBankDetail_InstrumentNumber.Trim() + "~" + Remarks + "~" + CashBankDetail_InstrumentDate + "~" + CashBank_IBRef + "~" + CashBank_FinYear + "~"
                  + CashBank_CompanyID + "~" + CashBank_ExchangeSegmentID + "~" + CaskBank_NumberingScheme + "~" + Rate + "~" + Amount + "~" + CashBank_BranchIDTO + "~" + ProjId + "~" + "Copy";
                }
                else
                {
                    grid.JSProperties["cpEdit"] = CashBank_TransactionDate + "~" + CashBank_VoucherNumber + "~" + CashBank_Narration + "~" +
                  CashBankDetail_PaymentAmount + "~" + CashBank_BranchID + "~" + CashBank_Currency + "~" + CashBankDetail_InstrumentType +
                  "~" + CashBankDetail_InstrumentNumber.Trim() + "~" + Remarks + "~" + CashBankDetail_InstrumentDate + "~" + CashBank_IBRef + "~" + CashBank_FinYear + "~"
                  + CashBank_CompanyID + "~" + CashBank_ExchangeSegmentID + "~" + CaskBank_NumberingScheme + "~" + Rate + "~" + Amount + "~" + CashBank_BranchIDTO + "~" + ProjId+"~"+"";
                }
                //Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022

                string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                string[] basedCurrency = LocalCurrency.Split('~');
                List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
                hdnDbSaveCurrenct.Value = "";
                foreach (DataRow dr in DT.Rows)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.WithDrawFrom = Convert.ToString(dr["WithDrawFrom"]);
                    Vouchers.DepositInto = Convert.ToString(dr["DepositInto"]);
                    Vouchers.Currency_ID = Convert.ToString(dr["CashBank_Currency"]);

                    Vouchers.WithdrawlSubLedger = Convert.ToString(dr["WithdrawlSubLedger"]);
                    Vouchers.WithdrawlSubledgerId = Convert.ToString(dr["WithdrawlSubledgerId"]);
                    Vouchers.DepositSubLedger = Convert.ToString(dr["DepositSubLedger"]);
                    Vouchers.DepositSubledgerId = Convert.ToString(dr["DepositSubledgerId"]);

                    Vouchers.CashBankDetail_PaymentAmount = Convert.ToString(dr["CashBankDetail_PaymentAmount"]);
                    Vouchers.Remarks = Convert.ToString(dr["Remarks"]);
                    Vouchers.Rate = Convert.ToString(dr["Rate"]);
                    Vouchers.Amount = Convert.ToString(dr["Amount"]);
                    if (dtProjectgrid.Rows.Count > 0)
                    {
                        foreach (DataRow drrw in dtProjectgrid.Rows)
                        {
                            Vouchers.ProjectId = Convert.ToInt64(drrw["ProjectId"]);
                            Vouchers.Project_Code = Convert.ToString(drrw["Project_Code"]);
                        }
                    }
                    else
                    {
                        Vouchers.ProjectId = 0;
                        Vouchers.Project_Code = "";
                    }
                    VoucherList.Add(Vouchers);
                }
                grid.DataSource = VoucherList;
                grid.DataBind();

            }
        }
        #endregion

        #region Classes & DataSource

        public class VOUCHERLIST
        {
            public string WithDrawFrom { get; set; }
            public string DepositInto { get; set; }
            public string Currency_ID { get; set; }
            public string CashBankDetail_PaymentAmount { get; set; }
            public string Remarks { get; set; }
            public string Rate { get; set; }
            public string Amount { get; set; }
            public Int64 ProjectId { get; set; }
            public string Project_Code { get; set; }
            public string WithdrawlSubLedger { get; set; }
            public string DepositSubLedger { get; set; }
            public string WithdrawlSubledgerId { get; set; }
            public string DepositSubledgerId { get; set; }

        }
        public class WithDrawFrom
        {
            public string AccountCode { get; set; }
            public string IntegrateMainAccount { get; set; }
        }
        public class Currency
        {
            public string Currency_ID { get; set; }
            public string Currency_AlphaCode { get; set; }
        }
        public IEnumerable GetMainAccount()
        {
            List<WithDrawFrom> MainAccountList = new List<WithDrawFrom>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            //string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
            string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+MainAccount_BankCashType as IntegrateMainAccount," +
                                    " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                    " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                    " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                    " and (MainAccount_BankCompany=\'" + Convert.ToString(Session["LastCompany"]) + "\' OR Isnull(MainAccount_BankCompany,'')='')" +
                                    " and (MainAccount_branchId=\'" + Convert.ToString(Session["userbranchID"]) + "\' Or IsNull(MainAccount_branchId,'')='0') )";

            string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
            string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;

            DataTable DT = objEngine.GetDataTable(strQuery_Table);
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                WithDrawFrom WithDrawFroms = new WithDrawFrom();
                WithDrawFroms.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_AccountCode"]);
                WithDrawFroms.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["IntegrateMainAccount"]);
                MainAccountList.Add(WithDrawFroms);
            }

            return MainAccountList;
        }
        public IEnumerable GetDepositInto(string branchId)
        {
            List<WithDrawFrom> MainAccountList = new List<WithDrawFrom>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            //string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
            string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+MainAccount_BankCashType as IntegrateMainAccount," +
                                    " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                    " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                    " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                    " and (MainAccount_BankCompany=\'" + Convert.ToString(Session["LastCompany"]) + "\' OR Isnull(MainAccount_BankCompany,'')='')" +
                                    " and (MainAccount_branchId=\'" + branchId + "\' Or IsNull(MainAccount_branchId,'')='0') )";

            string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
            string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;

            DataTable DT = objEngine.GetDataTable(strQuery_Table);
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                WithDrawFrom WithDrawFroms = new WithDrawFrom();
                WithDrawFroms.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_AccountCode"]);
                WithDrawFroms.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["IntegrateMainAccount"]);
                MainAccountList.Add(WithDrawFroms);
            }

            return MainAccountList;
        }
        public IEnumerable GetMainAccountByBranch(string branchId)
        {
            List<WithDrawFrom> MainAccountList = new List<WithDrawFrom>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            //string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
            //                        " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
            //                        " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
            //                         " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
            //                        " and (MainAccount_BankCompany=\'" + Convert.ToString(Session["LastCompany"]) + "\' OR Isnull(MainAccount_BankCompany,'')='')" +
            //                        " and (MainAccount_branchId=\'" + branchId + "\' Or IsNull(MainAccount_branchId,'')='0') )";
            string CombinedQuery = "(Select MainAccount_ReferenceID,MainAccount_AccountCode,MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+MainAccount_BankCashType as IntegrateMainAccount," +
                                    " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                    " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                     " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                    " and (MainAccount_BankCompany=\'" + Convert.ToString(Session["LastCompany"]) + "\' OR Isnull(MainAccount_BankCompany,'')='')" +
                                    " and (MainAccount_branchId=\'" + branchId + "\' Or IsNull(MainAccount_branchId,'')='0') )";
            string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
            string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;

            DataTable DT = objEngine.GetDataTable(strQuery_Table);
            DataTable restrictedDT = objEngine.GetDataTable("select branch_id,MainAccount_id from tbl_master_ledgerBranch_map");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DataRow[] restrictedTablerow = restrictedDT.Select("MainAccount_id=" + Convert.ToString(DT.Rows[i]["MainAccount_ReferenceID"]));

                if (restrictedTablerow.Length > 0)
                {
                    DataTable restrictedTable = restrictedTablerow.CopyToDataTable();
                    DataRow[] restrictedRow = restrictedTable.Select("branch_id=" + branchId);
                    if (restrictedRow.Length > 0)
                    {
                        WithDrawFrom WithDrawFroms = new WithDrawFrom();
                        WithDrawFroms.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_AccountCode"]);
                        WithDrawFroms.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["IntegrateMainAccount"]);
                        MainAccountList.Add(WithDrawFroms);
                    }


                }
                else
                {
                    WithDrawFrom WithDrawFroms = new WithDrawFrom();
                    WithDrawFroms.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_AccountCode"]);
                    WithDrawFroms.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["IntegrateMainAccount"]);
                    MainAccountList.Add(WithDrawFroms);
                }

            }

            return MainAccountList;
        }

        public IEnumerable GetCurrency()
        {
            string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
            string[] basedCurrency = LocalCurrency.Split('~');
            List<Currency> CurrencyList = new List<Currency>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            string CombinedQuery = "select Currency_ID,Currency_AlphaCode from Master_Currency ";


            DataTable DT = objEngine.GetDataTable(CombinedQuery);
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Currency Currencys = new Currency();
                Currencys.Currency_ID = Convert.ToString(DT.Rows[i]["Currency_ID"]);
                Currencys.Currency_AlphaCode = Convert.ToString(DT.Rows[i]["Currency_AlphaCode"]);

                CurrencyList.Add(Currencys);
            }

            return CurrencyList;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            batchgrid.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            ((GridViewDataComboBoxColumn)grid.Columns["WithDrawFrom"]).PropertiesComboBox.DataSource = GetMainAccount();
            ((GridViewDataComboBoxColumn)grid.Columns["DepositInto"]).PropertiesComboBox.DataSource = GetMainAccount();
            //   ((GridViewDataComboBoxColumn)grid.Columns["Currency_ID"]).PropertiesComboBox.DataSource = GetCurrency();
            GridViewDataComboBoxColumn combo = ((GridViewDataComboBoxColumn)grid.Columns["Currency_ID"]);
            combo.PropertiesComboBox.DataSource = GetCurrency();

            //if (!IsPostBack)
            //    grid.DataBind();
        }
        public IEnumerable GetVoucher()
        {
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //string strQuery = "Select (select Currency_AlphaCode from Master_Currency where Currency_ID=tc.CashBank_Currency)as CashBank_Currency"+
            //"from Trans_CashBankVouchers tc  where CashBank_ID="+;
            return VoucherList;
        }

        #endregion


        #region Print Contra Voucher

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\ContraVoucher\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\ContraVoucher\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        #endregion




        #region WebMethod

        [WebMethod]
        public static string GetAddLock(DateTime LockDate)
        {
            string rtrnvalue = "0";
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForContraStatus");
            proc.AddDateTimePara("@TransactionDate", LockDate);
            proc.AddVarcharPara("@ReturnValueID", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValueID"));
            return rtrnvalue;
        }



        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToString(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch + "~" + Valid_From + "~" + Valid_Upto;
            }
            return Convert.ToString(strschemavalue);
        }

        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(VoucherNo, "0", "ContraVoucher_Check");
            }
            return status;
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
            }

            return SalesRate;
        }
        public void BindBranchFrom()
        {
            dsBranch.SelectCommand = "select '0' AS BANKBRANCH_ID,'Select'  AS BANKBRANCH_NAME " +
            " Union  SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
            ddlBranchTo.DataBind();
        }

        [WebMethod]
        public static string GetCurrentBankBalance(string MainAccountID, string Branch)
        {
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string MainAccountValID = string.Empty;
            string strColor = string.Empty;
            DataTable DT = new DataTable();
            DBEngine objEngine = new DBEngine();

            DT = objEngine.GetDataTable("Select Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID='" + MainAccountID + "' and AccountsLedger_BranchId=" + Branch);
            if (DT.Rows.Count != 0)
            {

                if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                {
                    MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                    strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                }


            }

            return MainAccountValID + "~" + strColor;
        }


        [WebMethod(EnableSession = true)]

        public static object GetCustomer(string SearchKey, string AccountType)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                SearchKey = SearchKey.Replace("'", "''");

                DataTable dtable = oDBEngine.GetDataTable("select ISNULL(MainAccount_SubLedgerType,'') SubledgerType from Master_MainAccount where MainAccount_AccountCode='" + AccountType + "'");

                string Type = "";
                if (dtable.Rows.Count > 0 && Convert.ToString(dtable.Rows[0]["SubledgerType"]) != "None")
                {

                    if (Convert.ToString(dtable.Rows[0]["SubledgerType"]) == "Customers")
                    {
                        Type = "CL";
                    }
                    else if (Convert.ToString(dtable.Rows[0]["SubledgerType"]) == "Employees")
                    {
                        Type = "EM";
                    }
                    else if (Convert.ToString(dtable.Rows[0]["SubledgerType"]) == "Vendors")
                    {
                        Type = "DV";
                    }
                    else if (Convert.ToString(dtable.Rows[0]["SubledgerType"]) == "Agents")
                    {
                        Type = "AG";
                    }
                    else if (Convert.ToString(dtable.Rows[0]["SubledgerType"]) == "Driver/Transporter")
                    {
                        Type = "TR";
                    }


                    //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI


                    // Rev 0019246 Subhra 26-12-2018 
                    //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    // DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    //End of Rev
                    DataTable cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from V_SubledgerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.ContactType='" + Type + "' and pcd.Name like '%" + SearchKey + "%') as t order by t.Name ");

                    listCust = (from DataRow dr in cust.Rows
                                select new CustomerModel()
                                {
                                    id = dr["cnt_internalid"].ToString(),
                                    Na = dr["Name"].ToString(),
                                    UId = Convert.ToString(dr["uniquename"]),
                                    add = Convert.ToString(dr["Billing"])
                                }).ToList();
                }
            }
            return listCust;
        }



        #endregion

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "CashBank_ID";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ContraVoucherLists
                            where d.CashBank_TransactionDate >= Convert.ToDateTime(strFromDate) && d.CashBank_TransactionDate <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.CashBank_BranchID))
                            orderby d.CashBank_TransactionDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ContraVoucherLists
                            where
                            d.CashBank_TransactionDate >= Convert.ToDateTime(strFromDate) && d.CashBank_TransactionDate <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.CashBank_BranchID))
                            orderby d.CashBank_TransactionDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_ContraVoucherLists
                        where d.CashBank_BranchID == '0'
                        orderby d.CashBank_TransactionDate descending
                        select d;
                e.QueryableSource = q;
            }
        }


        [WebMethod]
        public static String getWithdrawlValue(string WithDrawValue)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string WithDrawl = "";
            DataTable dt2 = oDBEngine.GetDataTable("select isnull(MainAccount_SubLedgerType,'')MainAccount_SubLedgerType from Master_MainAccount where MainAccount_AccountCode='" + WithDrawValue + "'");
            if (dt2.Rows.Count > 0)
            {
                WithDrawl = Convert.ToString(dt2.Rows[0]["MainAccount_SubLedgerType"]);
                return WithDrawl;
            }
            else
            {
                WithDrawl = "";
                return WithDrawl;
            }
        }


        //Tanmoy Hierarchy
        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }
        //Tanmoy Hierarchy End

        //chinmoy added for inline project code start 10-12-2019
        protected void ProjectCodeCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocFroProject = new DataTable();
            string Projectid = e.Parameter.Split('~')[1];
            //string DocNo = e.Parameter.Split('~')[2];
            if (hdnProjectSelectInEntryModule.Value == "1")
            {

                if (Projectid != "0")
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument(Projectid);
                }
                else
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument();
                }
            }
            else
            {
                dtDocFroProject = GetProjectCodeDetailsOnDocument();
            }

            Session["VendorPayRecProjectCodefromDoc"] = dtDocFroProject;
            if (dtDocFroProject != null && dtDocFroProject.Rows.Count > 0)
            {
                lookupPopup_ProjectCode.DataSource = dtDocFroProject;
                lookupPopup_ProjectCode.DataBind();
            }
            else
            {
                lookupPopup_ProjectCode.DataSource = null;
                lookupPopup_ProjectCode.DataBind();
            }

        }

        //chinmoy added for projecvt code start 10-12-2019
        protected void lookup_ProjectCode_DataBinding(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDoc"];
            lookupPopup_ProjectCode.DataSource = dsdata;
        }

        //End







        //Tanmoy Hierarchy
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }
        //End Tanmoy Hierarchy


        public DataTable GetProjectCodeDetailsOnDocument()
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankProjectList");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectCodeDetailsOnDocument(string ProjectId)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankProjectListMappingProjectCode");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            proc.AddBigIntegerPara("@ProjectId", Convert.ToInt64(ProjectId));
            dt = proc.GetTable();
            return dt;
        }


    }


    public class CustomerModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }
   
}