//========================================================== Revision History ============================================================================================
//   1.0   Priti V2.0.36  02-02-2023  0025263: Listing view upgradation required of Cash/Bank Voucher of Accounts & Finance
//========================================== End Revision History =======================================================================================================


using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using System.Linq;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DataAccessLayer;
using DevExpress.Web.Data;
using System.Threading.Tasks;
using ERP.Models;
using static ERP.OMS.Management.Master.Mobileaccessconfiguration;

namespace ERP.OMS.Management.DailyTask
{
    public partial class CashBankEntryList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        CommonBL cbl = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/dailytask/CashBankEntryList.aspx");
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");


            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    GvCBSearch.Columns[21].Visible = true;


                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GvCBSearch.Columns[21].Visible = false;

                }
            }


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=54");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=54");
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

                String finyear = "";
               finyear= Convert.ToString(Session["LastFinYear"]).Trim();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                    toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);


                GvCBSearch.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CashBankPageCashBank";
               this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CashBankPageCashBank');</script>");

               Session.Remove("CashBankDetails");
               Session.Remove("exportval");
               Session.Remove("IBRef");
               Session.Remove("CB_FinalTaxRecord");
               Session.Remove("CashBank_ID");
               Session.Remove("SaveModeCB");
               Session.Remove("VoucherTypeCB");
               Session.Remove("schemavalueCB");
               Session.Remove("SaveNewValues");
               
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                }

                //string LastCompany = HttpContext.Current.Session["LastCompany"].ToString();
                //string userid = HttpContext.Current.Session["userid"].ToString();
                //string LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                //string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);

                Task PopulateStockTrialDataTask = new Task(() => GetAllDropDownDetailForCashBank());
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }
        public void GetAllDropDownDetailForCashBank()
        {
            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now; 

            DataSet dst = new DataSet();
            dst = AllDropDownDetailForCashBank();
           
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataSource = dst.Tables[1];
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;
                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }         
            
        }
        public DataSet AllDropDownDetailForCashBank()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 5000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "CBID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            List<int> branchidlist;
            int userid = Convert.ToInt32(Session["UserID"]);  //---- REV 1.0
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //---- REV 1.0
                    //var q = from d in dc.v_CashBankLists
                    //        where 
                    //        d.CashBank_TransactionDate >= Convert.ToDateTime(strFromDate) && d.CashBank_TransactionDate <= Convert.ToDateTime(strToDate)
                    //        && 
                    //        (branchidlist.Contains(Convert.ToInt32(d.CashBank_EnteredBranchID)) ||branchidlist.Contains(Convert.ToInt32(d.CashBank_BranchID)))

                    //        && Convert.ToString(d.CashBank_FinYear) == Convert.ToString(FinYear)
                    //        && Convert.ToString(d.CashBank_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.CashBank_TransactionDate
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.CASHBANKLISTs
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //---- REV 1.0
                    //var q = from d in dc.v_CashBankLists
                    //        where
                    //        d.CashBank_TransactionDate >= Convert.ToDateTime(strFromDate) && d.CashBank_TransactionDate <= Convert.ToDateTime(strToDate) 
                    //        &&
                    //        (branchidlist.Contains(Convert.ToInt32(d.CashBank_EnteredBranchID)) ||branchidlist.Contains(Convert.ToInt32(d.CashBank_BranchID)))
                    //        && Convert.ToString(d.CashBank_FinYear) == Convert.ToString(FinYear)
                    //        && Convert.ToString(d.CashBank_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.CashBank_TransactionDate
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.CASHBANKLISTs
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //---- REV 1.0
                //var q = from d in dc.v_CashBankLists
                //        where (d.CashBank_EnteredBranchID == '0' || d.CashBank_BranchID=='0')
                //        orderby d.CashBank_TransactionDate
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.CASHBANKLISTs
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
                //----END REV 1.0
            }
        }

        #region Export
        public void bindexport(int Filter)
        {
            exporter.GridViewID = "GvCBSearch";
            string filename = "CashBankVoucher";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Cash/Bank Voucher";
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
                case 5:
                    exporter.WriteXlsxToResponse();
                    break;
            }
        }
        protected void drdCashBankExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdCashBank.SelectedItem.Value.ToString());
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

        #endregion
        public int GetCaskBankDeleteData()
        {
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankDeleteDetails");

            proc.AddVarcharPara("@IBRef", 200, Convert.ToString(Session["IBRef"]));
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public DataTable CashBankVoucherDetailsList(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, string FromDate, string ToDate, string filter)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankVoucherDetailsList");
            proc.AddVarcharPara("@FinYear", 500, Fiyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            proc.AddVarcharPara("@filter", 10, filter);
            dt = proc.GetTable();
            return dt;
        }
        protected void GvCBSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];
            string IBRef = e.Parameters.Split('~')[1];
            GvCBSearch.JSProperties["cpDelete"] = null;
            if (command == "Delete")
            {
               // int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                //string IBRef = GvCBSearch.GetRowValues(RowIndex, "IBRef").ToString();
                Session["IBRef"] = IBRef;
                int val = GetCaskBankDeleteData();
                if (val == 1)
                {
                    GvCBSearch.JSProperties["cpDelete"] = "Successfully Deleted";
                    //string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
                    //string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");
                    //string BranchID = Convert.ToString(cmbBranchfilter.Value);
                    //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    //string finyear = Convert.ToString(Session["LastFinYear"]);
                    //Session["CashBankListingDetails"] = CashBankVoucherDetailsList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "true");
                    //GvCBSearch.DataBind();
                }
                else
                {
                    GvCBSearch.JSProperties["cpDelete"] = "Voucher is Reconciled.Cannot Delete";
                }

            }
           
        }

        protected void GvCBSearch_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\CashBankVoucher\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CashBankVoucher\DocDesign\Designes";
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


        //REV 1.0

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(FormDate.Date);
            dtTo = Convert.ToDateTime(toDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Task PopulateStockTrialDataTask = new Task(() => GetCashBankEntrydata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetCashBankEntrydata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_CASHBANK_LIST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                if (BRANCH_ID == "0")
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", Convert.ToString(Session["userbranchHierarchy"]));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                }
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        //END REV 1.0
    }
}