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
using System.Data.SqlClient;
using System.Threading.Tasks; 
namespace ERP.OMS.Management.Activities
{
    public partial class CustomerReceiptPaymentList : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();

        DataTable dtPartyTotal = null;
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ReceiptPayment_ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string companyid = Convert.ToString(Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    DateTime _fromDt = Convert.ToDateTime(strFromDate);
                    DateTime _todate = Convert.ToDateTime(strToDate);
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_CustomerReceiptPaymentLists
                    //        where  branchidlist.Contains(Convert.ToInt32(d.BranchID)) 
                    //        && d.ReceiptPayment_TransactionDate >= _fromDt && d.ReceiptPayment_TransactionDate <= _todate && d.CompanyID == companyid
                    //        orderby d.ReceiptPayment_ID descending
                    //        select d;

                    var q = from d in dc.CustomerReceiptPaymentLists
                            where d.USERID == User_id
                            orderby d.SEQ descending 
                            select d;

                    e.QueryableSource = q;
                     
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_CustomerReceiptPaymentLists
                    //        where d.ReceiptPayment_TransactionDate >= Convert.ToDateTime(strFromDate) && d.ReceiptPayment_TransactionDate <= Convert.ToDateTime(strToDate)
                    //        && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.CompanyID == companyid
                    //        orderby d.ReceiptPayment_ID descending
                    //        select d;
                    //e.QueryableSource = q;

                    var q = from d in dc.CustomerReceiptPaymentLists
                            where d.USERID == User_id
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_CustomerReceiptPaymentLists
                //        where d.BranchID==0
                //        orderby d.ReceiptPayment_ID descending
                //        select d; 
                //e.QueryableSource = q;

                var q = from d in dc.CustomerReceiptPaymentLists
                        where d.SEQ == 0     
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL cbl = new CommonBL();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_CustomerReceiptPayment.Columns[17].Visible = true;


                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_CustomerReceiptPayment.Columns[17].Visible = false;

                }
            }

            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=5");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=5");
            DataTable dtposTimeEditReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=4");
            DataTable dtposTimeDeleteReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=4");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "Customer Payment DATA is Freezed between   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + "for Edit. ";
                
            }
            if(dtposTimeEditReceipt !=null && dtposTimeEditReceipt.Rows.Count>0)
             {
                 hdnLockFromDateReceiptEdit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Fromdate"]);
                 hdnLockToDateReceiptedit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Todate"]);
                 
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = (spnEditLock.InnerText) + "Customer Receipt DATA is Freezed between   " + hdnLockFromDateReceiptEdit.Value + " to " + hdnLockToDateReceiptedit.Value + " for Edit.";
             }

            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);

                spnDeleteLock.InnerText = "Customer Payment DATA is Freezed between   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + " for Delete. ";
            }

            if (dtposTimeDeleteReceipt != null && dtposTimeDeleteReceipt.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Todate"]);

                spnDeleteLock.InnerText = (spnDeleteLock.InnerText) + "Customer Receipt DATA is Freezed between   " + hdnLockFromDateReceiptdelete.Value + " to " + hdnLockToDateReceiptdelete.Value + " for Delete.";
            }

           

            if (!IsPostBack)
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
                //................Cookies..................
                //Grid_CustomerReceiptPayment.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerReceiptPaymentCustRecPayList";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerReceiptPaymentCustRecPayList');</script>");
                //...........Cookies End............... 
                //Session["exportval"] = null;
                //Session["SaveModeCRP"] = null;
                //Session["ReceiptPaymentDetails"] = null;
                //Session.Remove("CustomerRecPayListingDetails");
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

               

                GetAllDropDownDetailForCashBank();
            }
           // FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerReceiptPaymentList.aspx");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
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

            exporter.GridViewID = "Grid_CustomerReceiptPayment";
            string filename = "CustomerReceiptPayment";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Customer Receipt/Payment";
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
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void Grid_CustomerReceiptPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string POID = null;
            int deletecnt = 0;
            int _depCnt = 0;
            Boolean IsExist = false;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    POID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (Command == "Delete")
            {
                if (!IsCRTTransactionExist(POID))
                {
                    DataTable dt = new DataTable();
                    dt = objCustomerVendorReceiptPaymentBL.DeleteCustomerDependentOrder(POID);
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                    {
                        IsExist = true;
                    }
                    if (IsExist)
                    {
                        Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "This Receipt number is tagged in another Payment or Already Reconciled. Cannot Delete.";
                    }
                    else  
                    {
                        deletecnt = objCustomerVendorReceiptPaymentBL.DeleteCRPOrder(POID);
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
                            //Session["CustomerRecPayListingDetails"] = GetCustomerReceiptPaymentGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                            //Grid_CustomerReceiptPayment.DataBind();
                        }
                        else
                        {
                            Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Try again.";
                        }
                    }
                }
                else
                {
                    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "This Customer Receipt Payment  is tagged in other modules. Cannot Delete.";
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
            //    Session["CustomerRecPayListingDetails"] = GetCustomerReceiptPaymentGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            //    Grid_CustomerReceiptPayment.DataBind();

            //}
        }
        public DataTable GetCustomerReceiptPaymentGridData(string userbranch, string lastCompany, string finyear, string BranchID, string FromDate, string ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@FinYear", 500, finyear);
            proc.AddVarcharPara("@CompanyId", 500, lastCompany);
            proc.AddVarcharPara("@userbranchlist", 4000, userbranch);
            proc.AddVarcharPara("@BranchId", 4000, BranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            
            dt = proc.GetTable();
            return dt;
        }
        private bool IsCRTTransactionExist(string CRTid)
        {
            bool IsExist = false;
            if (CRTid != "" && Convert.ToString(CRTid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objCustomerVendorReceiptPaymentBL.CheckCRTTraanaction(CRTid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
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
                    RecPayType = "Refund";
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
                //SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        protected void Grid_CustomerReceiptPayment_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void Grid_CustomerReceiptPayment_DataBinding(object sender, EventArgs e)
        {
            //DataTable dsdata = (DataTable)Session["CustomerRecPayListingDetails"];
            //Grid_CustomerReceiptPayment.DataSource = dsdata;
        }

        protected void doc_selectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["DocTaggedDetails"] = null;
            var param = e.Parameter.Split('~');
            if (Convert.ToString(param[0]) == "GetCountDoc")
            {
                string Receipt_ID = Convert.ToString(param[1]);
                DataTable dtCount = oDBEngine.GetDataTable("Select (select COUNT(*) from Trans_CustomerReceiptPaymentDetail where ReceiptDetail_DocumentNumber ='" + Receipt_ID + "' and ReceiptDetail_DocumentTypeID = 'AdvancePayment' and IsOpening<>'OP')+(select COUNT(*) from tbl_trans_advanceAdjustment where Adjusted_doc_id ='" + Receipt_ID + "'  )+(select COUNT(*) from tbl_trans_DrNoteAdvanceAdjustment where Adjusted_doc_id ='" + Receipt_ID + "')+(Select COUNT(*) from Trans_CustomerReceiptPaymentDetail where ReceiptDetail_VoucherID='" + Receipt_ID + "' and isnull(ReceiptPaymentDetails_BankValueDate,'')<>'')+(Select Count(*) from Tbl_trans_adjustment where Doc_Type='Advance' and Adjusted_Doc_No='"+Receipt_ID+"')");
                doc_selectPanel.JSProperties["cpresult"] = "GetCount~" + Convert.ToString(dtCount.Rows[0][0]) +"~"+Receipt_ID;
                docGrid.DataBind();
            }

            if (Convert.ToString(param[0]) == "BindGrid")
            {

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
                proc.AddVarcharPara("@Action", 500, "GetDocTaggedDetails");
                proc.AddVarcharPara("@Receipt_ID", 500, Convert.ToString(param[1])); 
                dt = proc.GetTable();

                Session["DocTaggedDetails"] = dt;
                docGrid.DataBind();

                doc_selectPanel.JSProperties["cpresult"] = "ShowDetails";

            }
        }

        protected void docGrid_DataBinding(object sender, EventArgs e)
        {
            docGrid.DataSource = (DataTable)Session["DocTaggedDetails"];
        }
        protected void EntityServerModeDataSourceCO_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            #region Block By Sudip
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            string strToDate = Convert.ToString(hddnAsOnDate.Value);
            e.KeyExpression = "SLNO";

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" && Convert.ToString(d.PARTYTYPE) == "C"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }


            #endregion
            CustomerOutstanding.ExpandAll();

        }
        protected void ShowGridCustOut_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,BAL_AMOUNT from PARTYOUTSTANDINGDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Gross Outstanding:' AND PARTYTYPE='C'");
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalBalAmt = dtPartyTotal.Rows[0][1].ToString();

            }
            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_BalAmt":
                        e.Text = PartyTotalBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }

        }
        protected void cCustomerOutstanding_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strAsOnDate = Convert.ToString(e.Parameters.Split('~')[3]);
            string strCustomerId = Convert.ToString(e.Parameters.Split('~')[1]);
            string BranchId = e.Parameters.Split('~')[2];
            string strCommand = Convert.ToString(e.Parameters.Split('~')[0]);
            DataTable dtOutStanding = new DataTable();
            if (strCommand == "BindOutStanding")
            {
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingRecords(strAsOnDate, strCustomerId, BranchId);

                //CustomerOutstanding.DataSource = dtOutStanding;
                //CustomerOutstanding.DataBind();
                CustomerOutstanding.JSProperties["cpOutStanding"] = "OutStanding";


            }
        }

        protected void ShowGridCustOut_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Doc. Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }
        protected void ShowGridCustOut_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue) == "Party Outstanding:" || Convert.ToString(e.CellValue) == "Total:")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
        }

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
            Task PopulateStockTrialDataTask = new Task(() => GetCustomerReceiptPaymentdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetCustomerReceiptPaymentdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CustomerReceiptPayment_List", con);
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
    }
}