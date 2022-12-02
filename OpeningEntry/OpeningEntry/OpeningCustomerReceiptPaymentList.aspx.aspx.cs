using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using OpeningEntry.OpeningEntry.DBML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.OpeningEntry
{
    public partial class OpeningCustomerReceiptPaymentList_aspx : System.Web.UI.Page
    {


        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();

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
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    DateTime _fromDt = Convert.ToDateTime(strFromDate);
                    DateTime _todate = Convert.ToDateTime(strToDate);
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_CustomerReceiptPaymentLists
                            where branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            && d.ReceiptPayment_TransactionDate >= _fromDt && d.ReceiptPayment_TransactionDate <= _todate && d.CompanyID == companyid
                            orderby d.ReceiptPayment_ID descending
                            select d;

                    e.QueryableSource = q;

                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_CustomerReceiptPaymentLists
                            where d.ReceiptPayment_TransactionDate >= Convert.ToDateTime(strFromDate) && d.ReceiptPayment_TransactionDate <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.CompanyID == companyid
                            orderby d.ReceiptPayment_ID descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                var q = from d in dc.v_CustomerReceiptPaymentLists
                        where d.BranchID == 0
                        orderby d.ReceiptPayment_ID descending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //................Cookies..................
                //Grid_CustomerReceiptPayment.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerReceiptPaymentCustRecPayList";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerReceiptPaymentCustRecPayList');</script>");
                //...........Cookies End............... 
                //Session["exportval"] = null;
                //Session["SaveModeCRP"] = null;
                //Session["ReceiptPaymentDetails"] = null;
                //Session.Remove("CustomerRecPayListingDetails");



                //FormDate.Date = DateTime.Now;
                //toDate.Date = DateTime.Now;

                DateTime fromDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]);
                fromDate = fromDate.AddDays(-1);


                toDate.Date = fromDate;
                FormDate.Date = fromDate;
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
        //protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));

        //    if (Filter != 0)
        //    {
        //        if (Session["exportval"] == null)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //        else if (Convert.ToInt32(Session["exportval"]) != Filter)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //    }

        //}
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
                        Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "This Receipt number is tagged in another Payment. Cannot Delete.";
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
                DataTable dtCount = oDBEngine.GetDataTable("Select (select COUNT(*) from Trans_CustomerReceiptPaymentDetail where ReceiptDetail_DocumentNumber ='" + Receipt_ID + "' and ReceiptDetail_DocumentTypeID = 'AdvancePayment' and IsOpening<>'OP')+(select COUNT(*) from tbl_trans_advanceAdjustment where Adjusted_doc_id ='" + Receipt_ID + "'  )+(select COUNT(*) from tbl_trans_DrNoteAdvanceAdjustment where Adjusted_doc_id ='" + Receipt_ID + "')+(Select COUNT(*) from Trans_CustomerReceiptPaymentDetail where ReceiptDetail_VoucherID='" + Receipt_ID + "' and isnull(ReceiptPaymentDetails_BankValueDate,'')<>'')+(Select Count(*) from Tbl_trans_adjustment where Doc_Type='Advance' and Adjusted_Doc_No='" + Receipt_ID + "')");
                doc_selectPanel.JSProperties["cpresult"] = "GetCount~" + Convert.ToString(dtCount.Rows[0][0]) + "~" + Receipt_ID;
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
    }
}