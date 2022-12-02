using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.IO;
using ERP.Models;
using System.Globalization;
using System.Linq;
namespace ERP.OMS.Management.Activities
{
    public partial class RateDifferenceEntryVendorList : System.Web.UI.Page
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PurchaseReturnBL objPurchaseReturnBL = new PurchaseReturnBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/RateDifferenceEntryVendorList.aspx");



            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");



            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    GrdPurchaseReturn.Columns[5].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GrdPurchaseReturn.Columns[5].Visible = false;
                }
            }

            if (!IsPostBack)
            {
                // Session.Remove("PurchaseReturnListingDetails");
                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

                    PopulateBranchByHierchy(userbranch);

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    Session["RDEV_schemavalue"] = null;
                    //  GetFinacialYearBasedQouteDate();
                    //  string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
                    //  string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
                    //  GetPurchaseReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
                    GrdPurchaseReturn.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesRateDifferenceEntryVendorWithStockListDetailsGrid";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesRateDifferenceEntryVendorWithStockListDetailsGrid');</script>");

                }
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
       
        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
              
            }
        }
        public void bindexport(int Filter)
        {
            GrdPurchaseReturn.Columns[5].Visible = false;
            string filename = "Rate Difference Entry Vendor";
            exporter.FileName = filename;
            //  exporter.FileName = "PurchaseReturnWithStockAndAccount";

            exporter.PageHeader.Left = "Rate Difference Entry Vendor";
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

        #endregion

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }

        #region Export Grid Section Start

        protected void GrdPurchaseReturn_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            if (Command == "Delete")
            {
                string PurchaseReturnID = Convert.ToString(e.Parameters).Split('~')[1];
                int deletecnt = 0;
                deletecnt = objPurchaseReturnBL.DeletePReturn(PurchaseReturnID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userbranchID"]), "RDEV");

                string BranchID = Convert.ToString(cmbBranchfilter.Value);
                //   string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

                string FromDate = Convert.ToString(FormDate.Value);
                string ToDate = Convert.ToString(toDate.Value);

                DataTable dtdata = new DataTable();
                //  dtdata = objSalesReturnBL.GetIssueToCustomerReturnListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), BranchID, FromDate, ToDate, "SC");

                dtdata = objPurchaseReturnBL.GetPRListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), "RDEV", FromDate, ToDate, Convert.ToString(Session["LastFinYear"]), BranchID);

                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    //  Session["PurchaseReturnListingDetails"] = dtdata;
                    GrdPurchaseReturn.DataBind();
                }



                GrdPurchaseReturn.JSProperties["cpDelete"] = "Deleted successfully.";
                //if (deletecnt>0)
                //{ GrdSalesReturn.JSProperties["cpDelete"] = "Deleted successfully."; }
                //else { GrdSalesReturn.JSProperties["cpDelete"] = "Please try again."; }


            }

            else if (Command == "FilterGridByDate")
            {

                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                // FillAllSearchGrid(true);

                DataTable dtdata = new DataTable();
                // dtdata = objSalesReturnBL.GetCustomerReturnListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "SC");

                //  dtdata = objSalesReturnBL.GetSalesReturnNormalListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);


                dtdata = objPurchaseReturnBL.GetPRListGridData(userbranch, lastCompany, "RDEV", FromDate, ToDate, finyear, BranchID);
                //    Session["PurchaseReturnListingDetails"] = dtdata;
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdPurchaseReturn.DataSource = dtdata;
                    GrdPurchaseReturn.DataBind();
                }
                else
                {
                    GrdPurchaseReturn.DataSource = null;
                    GrdPurchaseReturn.DataBind();
                }

            }
        }
        public void GetPurchaseReturnListGridData(string userbranch, string lastCompany, string FinyearStartDate, string FinYearEndDate)
        {
         
        }

        #endregion

     

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\RateDiffVendor\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\RateDiffVendor\DocDesign\Normal";
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

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            // e.KeyExpression = "SrlNo";
            e.KeyExpression = "SrlNo";

         //   string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);



                    var q = from d in dc.v_RateDifferenceEntryVendorLists
                            where d.Rn_Date >= Convert.ToDateTime(strFromDate) && d.Rn_Date <= Convert.ToDateTime(strToDate)

                            && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                           && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)

                            orderby d.Rn_Date descending
                            select d;

                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_RateDifferenceEntryVendorLists
                            where
                            d.Rn_Date >= Convert.ToDateTime(strFromDate) && d.Rn_Date <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                           && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)
                            orderby d.Rn_Date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_RateDifferenceEntryVendorLists
                        where d.BranchID == '0'
                        orderby d.Rn_Date descending
                        select d;
                e.QueryableSource = q;
            }
        }
    }
}