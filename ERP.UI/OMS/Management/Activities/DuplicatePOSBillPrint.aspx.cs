using System;
using System.Configuration;
using System.Data;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.IO;
using ERP.Models;
using System.Linq;
using System.Collections.Generic;

namespace ERP.OMS.Management.Activities
{
    public partial class DuplicatePOSBillPrint : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PosSalesInvoiceList.aspx");

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


                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    PopulateBranchByHierchy(userbranchHierachy);
                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
                    GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrdQuotationPosList";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrdQuotationPosList');</script>");
                }
            }
        }

        #region Grid Section Start
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_posLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_posLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_posLists
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void GrdQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            GrdQuotation.JSProperties["cpinsert"] = null;
            GrdQuotation.JSProperties["cpEdit"] = null;
            GrdQuotation.JSProperties["cpUpdate"] = null;
            GrdQuotation.JSProperties["cpDelete"] = null;
            GrdQuotation.JSProperties["cpExists"] = null;
            GrdQuotation.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Delete")
            {
                deletecnt = posSale.DeleteInvoice(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
            else if (WhichCall == "FilterGridByDate")
            {
                //string fromdate = e.Parameters.Split('~')[1];
                //string toDate = e.Parameters.Split('~')[2];
                //string branch = e.Parameters.Split('~')[3];
                //DataTable dtdata = new DataTable();
                //dtdata = posSale.GetDuplicateInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
                //if (dtdata != null)
                //{
                //    Session["PosDupSalesInvoiceList"] = dtdata;
                //    GrdQuotation.DataBind();
                //}
            }
            else if (WhichCall == "RefreshGrid")
            {
                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //GetQuotationListGridData(userbranchHierachy, lastCompany);
            }
            else if (WhichCall == "CancelAssignment")
            {
                posSale.CancelBranchAssignment(Convert.ToInt32(e.Parameters.Split('~')[1]));
                GrdQuotation.JSProperties["cpCancelAssignMent"] = "yes";
                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //GetQuotationListGridData(userbranchHierachy, lastCompany);
            }
        }
        public void GetQuotationListGridData(string userbranch, string lastCompany)
        {
            //DataTable dtdata = posSale.GetInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), FormDate.Date.ToString("yyyy-MM-dd"), toDate.Date.ToString("yyyy-MM-dd"), Convert.ToString(cmbBranchfilter.Value));
            /* Abhisek
            DataTable dtdata = posSale.GetDuplicateInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), FormDate.Date.ToString("yyyy-MM-dd"), toDate.Date.ToString("yyyy-MM-dd"), Convert.ToString(cmbBranchfilter.Value));
            
            if (dtdata != null)
            {
                Session["PosDupSalesInvoiceList"] = dtdata;
                GrdQuotation.DataBind();
            }
             */

        }
        /* Abhisek
        protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["PosDupSalesInvoiceList"] != null)
            {
                GrdQuotation.DataSource = (DataTable)Session["PosDupSalesInvoiceList"];
            }
            else
            {
                GrdQuotation.DataSource = null;
            }

        }
         */
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion

        #region generic method
        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            if (name.Length > 0)
            {
                branchName.Text = Convert.ToString(name[0]["branch_description"]);
            }


        }
        [WebMethod]
        public static string GetCurrentBankBalance(string BranchId, string fromDate, string todate)
        {
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string MainAccountValID = string.Empty;
            string strColor = string.Empty;
            DataTable DT = new DataTable();
            DBEngine objEngine = new DBEngine();
            if (BranchId.Trim() != "0")
            {
                //+ " and convert(varchar(10),AccountsLedger_TransactionDate,120) >= '" + fromDate + "' and convert(varchar(10),AccountsLedger_TransactionDate,120) <='" + todate + "'"
                DT = objEngine.GetDataTable("Select isnull(Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr),0) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID=(select top 1  MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Cash')  and AccountsLedger_BranchId=" + BranchId);
                if (DT.Rows.Count != 0)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                    {
                        MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                        strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                    }
                }
            }
            else
            {
                // and  convert(varchar(10),AccountsLedger_TransactionDate,120) >= '" + fromDate + "' and convert(varchar(10),AccountsLedger_TransactionDate,120) <='" + todate + "'
                DT = objEngine.GetDataTable("Select isnull(Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr),0) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID=(select top 1  MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Cash') and  AccountsLedger_BranchId in( " + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")");
                if (DT.Rows.Count != 0)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                    {
                        MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                        strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                    }
                }

            }

            return MainAccountValID + "~" + strColor;
        }
        #endregion
        
    }
}