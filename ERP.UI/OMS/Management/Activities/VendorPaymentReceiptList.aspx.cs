/**********************************************************************************************************************************
 1.0      30-05-2023        2.0.38        Sanchita      ERP - Listing Page - Vendor Payment / Receipt. refer: 26660
***********************************************************************************************************************************/
using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using ERP.Models;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static ERP.OMS.Management.Master.Mobileaccessconfiguration;

namespace ERP.OMS.Management.Activities
{
    public partial class VendorPaymentReceiptList : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();

        // Rev 1.0
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
            Task PopulateStockTrialDataTask = new Task(() => GetVendorPaymentRecieptdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetVendorPaymentRecieptdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_VendorPaymentRecieptList", con);
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
                //cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.Parameters.AddWithValue("@ACTION", "ALL");
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
        // End of Rev 1.0
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ReceiptPayment_ID";

           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string ComponyId = Convert.ToString(Session["LastCompany"]);
            // Rev 1.0
            int userid = Convert.ToInt32(Session["UserID"]);
            // End of Rev 1.0

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                // Rev 1.0
                //if (strBranchID == "0")
                //{
                //    DateTime _fromDt = Convert.ToDateTime(strFromDate);
                //    DateTime _todate = Convert.ToDateTime(strToDate);
                //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //    var q = from d in dc.v_VendorPaymentRecieptLists
                //            where branchidlist.Contains(Convert.ToInt32(d.BranchID)) &&
                //            d.ReceiptPayment_TransactionDate >= _fromDt && d.ReceiptPayment_TransactionDate <= _todate
                //            && d.CompanyID == ComponyId
                //            orderby d.ReceiptPayment_ID descending
                //            select d;

                //    e.QueryableSource = q;
                //    //var cnt = q.Count();
                //}
                //else
                //{
                //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //    var q = from d in dc.v_VendorPaymentRecieptLists
                //            where d.ReceiptPayment_TransactionDate >= Convert.ToDateTime(strFromDate) && d.ReceiptPayment_TransactionDate <= Convert.ToDateTime(strToDate)
                //            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.CompanyID == ComponyId
                //            orderby d.ReceiptPayment_ID descending
                //            select d;
                //    e.QueryableSource = q;
                //}


                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.VendorPaymentRecieptLists
                        where  d.USERID == userid
                        orderby d.SEQ descending
                        select d;

                e.QueryableSource = q;
                // End of Rev 1.0
            }
            else
            {
                // Rev 1.0
                //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_VendorPaymentRecieptLists
                //        where d.BranchID==0
                //        orderby d.ReceiptPayment_ID descending
                //        select d;

                //e.QueryableSource = q;
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.VendorPaymentRecieptLists
                        where d.BranchID == 0
                        orderby d.SEQ descending
                        select d;

                e.QueryableSource = q;
                // End of Rev 1.0
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

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


            CommonBL cbl = new CommonBL();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_CustomerReceiptPayment.Columns[18].Visible = true;


                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_CustomerReceiptPayment.Columns[18].Visible = false;

                }
            }


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=14");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=14");
            DataTable dtposTimeEditReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=15");
            DataTable dtposTimeDeleteReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=15");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateeditCon.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateeditCon.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "Vendor Payemnt DATA is Freezed between   " + hdnLockFromDateeditCon.Value + " to " + hdnLockToDateeditCon.Value + " for Edit. ";

            }
            if (dtposTimeEditReceipt != null && dtposTimeEditReceipt.Rows.Count > 0)
            {
                hdnLockFromDateReceiptEdit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateReceiptedit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateReceiptEditCon.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateReceipteditCon.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["DataFreeze_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = (spnEditLock.InnerText) + "Vendor Receipt DATA is Freezed between   " + hdnLockFromDateReceiptEditCon.Value + " to " + hdnLockToDateReceipteditCon.Value + " for Edit.";
            }

            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                hdnLockFromDatedeleteCon.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDatedeleteCon.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Todate"]);
                spnDeleteLock.InnerText = "Vendor Payemnt DATA is Freezed between   " + hdnLockFromDatedeleteCon.Value + " to " + hdnLockToDatedeleteCon.Value + " for Delete. ";
            }

            if (dtposTimeDeleteReceipt != null && dtposTimeDeleteReceipt.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateReceiptdeleteCon.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateReceiptdeleteCon.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["DataFreeze_Todate"]);
                spnDeleteLock.InnerText = (spnDeleteLock.InnerText) + "Vendor Receipt DATA is Freezed between   " + hdnLockFromDateReceiptdeleteCon.Value + " to " + hdnLockToDateReceiptdeleteCon.Value + " for Delete.";
            }

            if (!IsPostBack)
            {

                

                Session["exportval"] = null;
                Session["SaveModeVPR"] = null;
                Session["VendorPaymentReceiptDetails"] = null;
                //................Cookies..................
                //Grid_CustomerReceiptPayment.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt');</script>");
                ////...........Cookies End............... 
                Session.Remove("VendorPayRecListingDetails");
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                GetAllDropDownDetailForCashBank();
            }
           // FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/VendorPaymentReceiptList.aspx");
        }
        public void GetAllDropDownDetailForCashBank()
        {
            DataSet dst = new DataSet();
            dst = AllDropDownDetailForCashBank();
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataSource = dst.Tables[0];
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;
                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }
        }
        public DataSet AllDropDownDetailForCashBank()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            //exporter.GridView = Grid_ContraVoucher;

            exporter.GridViewID = "Grid_CustomerReceiptPayment";
            string filename = "VendorPaymentReceipt";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Vendor Payment/Receipt";
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
        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            //DataTable dtdata = GetCustomerReceiptPaymentListGridData();


            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    Grid_CustomerReceiptPayment.DataSource = dtdata;
            //    Grid_CustomerReceiptPayment.DataBind();
            //}
            //else
            //{
            //    Grid_CustomerReceiptPayment.DataSource = null;
            //    Grid_CustomerReceiptPayment.DataBind();
            //}
        }
        public DataTable GetCustomerReceiptPaymentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        //Rev Rajdip
        //public DataTable GetDebitNotevalid(string Action, string NotelNo)
        //{
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        //    DataTable dt = objEngine.GetDataTable("SELECT IsNull(Sum(Actual_Amount),0) Actual_Amount,IsNull(Sum(UnPaidAmount),0) UnPaidAmount FROM  V_VendorPayment_Receipt custview inner join " +
        //                    " Trans_VendorReceiptPayment VendRp  on custview.Doc_Id = VendRp.ReceiptPayment_ID "+
        //                    " WHERE ReceiptPayment_ID='" + NotelNo + "'");
        //    return dt;
        //}
        public DataTable GetAdvancevalid(string Action, string NotelNo)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            //DataTable dt = objEngine.GetDataTable("SELECT IsNull(Sum(Actual_Amount),0) Actual_Amount,IsNull(Sum(UnPaidAmount),0) UnPaidAmount FROM  V_VendorPayment_Receipt custview inner join " +
            //                " Trans_VendorReceiptPayment VendRp  on custview.Doc_Id = VendRp.ReceiptPayment_ID " +
            //                " WHERE ReceiptPayment_ID='" + NotelNo + "'");
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentValidationOnTagging");
            proc.AddVarcharPara("@ReceiptPayment_ID", 500, NotelNo);        
            dt = proc.GetTable();
            return dt;
        }
        //END REV RAJDIP
        protected void Grid_CustomerReceiptPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string POID = null;
            int deletecnt = 0;
            Boolean IsExist = false;
            string type = "";
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    POID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }
            Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = null;
            if (Command == "Delete")
            {
                DataTable dt = new DataTable();
                //Rev Rajdip
                //DataTable Dtdebitnotevalidation = GetDebitNotevalid("VendorNoteDelete", POID.ToString());
                DataTable DtAdvancevalidation = GetAdvancevalid("VendorNoteDelete", POID.ToString());
                //decimal Actual_Amount = Convert.ToDecimal(Dtdebitnotevalidation.Rows[0]["Actual_Amount"].ToString());
                //decimal UnPaidAmount = Convert.ToDecimal(Dtdebitnotevalidation.Rows[0]["UnPaidAmount"].ToString());
                //if (Actual_Amount != UnPaidAmount)
                //{
                //    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "his Payment number is tagged in another Receipt. Cannot Delete.";
                //    return;
                //}
                int Validate = Convert.ToInt32(DtAdvancevalidation.Rows[0]["Validate"].ToString());
                if (Validate > 0)
                {
                    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Payment number is tagged in another Receipt or already reconciled. Cannot Delete.";
                    return;
                }
                //End Rev Rajdip
                dt = objCustomerVendorReceiptPaymentBL.DeleteVendorDependentOrder(POID);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }

                if (IsExist) //dependent record present
                {
                    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "This Payment number is tagged in another Receipt. Cannot Delete.";
                }
                else //dependent record not present
                {
                    deletecnt = objCustomerVendorReceiptPaymentBL.DeleteVendorPROrder(POID);

                    if (deletecnt == 1)
                    {
                        Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Deleted successfully";

                        //string BranchID = Convert.ToString(cmbBranchfilter.Value);
                        //string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
                        //string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");

                        //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                        //string finyear = Convert.ToString(Session["LastFinYear"]);
                        //DataTable dtdata = new DataTable();
                        //Session["VendorPayRecListingDetails"] = GetVendorPaymentReceiptGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                        //Grid_CustomerReceiptPayment.DataBind();

                    }
                    else
                    {
                        Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                    }
                }
            }
            //else if (Command == "FilterGridByDate")
            //{
            //    string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
            //    string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
            //    string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

            //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //    string finyear = Convert.ToString(Session["LastFinYear"]);

            //    DataTable dtdata = new DataTable();
            //    Session["VendorPayRecListingDetails"] = GetVendorPaymentReceiptGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            //    Grid_CustomerReceiptPayment.DataBind();

            //}
        }
        public DataTable GetVendorPaymentReceiptGridData(string userbranch, string lastCompany, string finyear, string BranchID, string FromDate, string ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@FinYear", 500, finyear);
            proc.AddVarcharPara("@CompanyId", 500, lastCompany);
            proc.AddVarcharPara("@userbranchlist", 4000, userbranch);
            proc.AddVarcharPara("@BranchId", 4000, BranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);

            dt = proc.GetTable();
            return dt;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\VendorRecPay\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\VendorRecPay\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");
                string RecPayType = Convert.ToString(HdRecPayType.Value);
                if (RecPayType == "Receipt")
                {
                    RecPayType = "Receipt";
                }
                else
                {
                    RecPayType = "Payment";
                }

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Contains(RecPayType))
                    {
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
                }
                CmbDesignName.SelectedIndex = 0;
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }

        protected void Grid_CustomerReceiptPayment_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void Grid_CustomerReceiptPayment_DataBinding(object sender, EventArgs e)
        {
            //DataTable dsdata = (DataTable)Session["VendorPayRecListingDetails"];
            //Grid_CustomerReceiptPayment.DataSource = dsdata;
        }
    }
}