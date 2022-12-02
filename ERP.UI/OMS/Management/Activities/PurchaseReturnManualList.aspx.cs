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
    public partial class PurchaseReturnManualList : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ProjectPurchaseReturnBL objPurchaseReturnBL = new ProjectPurchaseReturnBL();

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

            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PurchaseReturnManualList.aspx");

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


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=9");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=9");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateeditDatafreeze.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateeditDatafreeze.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "DATA is Freezed between   " + hdnLockFromDateeditDatafreeze.Value + " to " + hdnLockToDateeditDatafreeze.Value + " for Edit. ";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                hdnLockFromDatedeleteDatafreeze.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDatedeleteDatafreeze.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Todate"]);
                spnDeleteLock.InnerText = spnEditLock.InnerText + " DATA is Freezed between   " + hdnLockFromDatedeleteDatafreeze.Value + " to " + hdnLockToDatedeleteDatafreeze.Value + " for Delete.";
                spnEditLock.InnerText = "";
            }

            if (!IsPostBack)
            {
                //Session.Remove("PurchaseReturnManualListingDetails");
                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

                    PopulateBranchByHierchy(userbranch);

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    // GetFinacialYearBasedQouteDate();
                    // string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
                    //   string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
                    //   GetPurchaseReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
                    GrdPurchaseReturn.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesPurchaseReturnManualWithStockListDetailsGrid";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesPurchaseReturnManualWithStockListDetailsGrid');</script>");

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
        //public void GetFinacialYearBasedQouteDate()
        //{
        //    String finyear = "";
        //    if (Session["LastFinYear"] != null)
        //    {
        //        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        //        finyear = Convert.ToString(Session["LastFinYear"]).Trim();
        //        DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
        //        if (dtFinYear != null && dtFinYear.Rows.Count > 0)
        //        {
        //            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
        //            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

        //        }
        //    }
        //    //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        //}
        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
                //if (Session["exportval"] == null)
                //{
                //    Session["exportval"] = Filter;
                //    bindexport(Filter);
                //}
                //else if (Convert.ToInt32(Session["exportval"]) != Filter)
                //{
                //    Session["exportval"] = Filter;
                //    bindexport(Filter);
                //}
            }
        }
        public void bindexport(int Filter)
        {
            GrdPurchaseReturn.Columns[5].Visible = false;
            string filename = "Purchase Return Manual";
            exporter.FileName = filename;
            //  exporter.FileName = "PurchaseReturnWithStockAndAccount";

            exporter.PageHeader.Left = "Purchase Return Manual";
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
                deletecnt = objPurchaseReturnBL.DeletePReturn(PurchaseReturnID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userbranchID"]), "PRM");
                
                if (deletecnt > 0)
                {
                    string BranchID = Convert.ToString(cmbBranchfilter.Value);
                    string FromDate = Convert.ToString(FormDate.Value);
                    string ToDate = Convert.ToString(toDate.Value);
                    DataTable dtdata = new DataTable();

                    dtdata = objPurchaseReturnBL.GetPRListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), "PRM", FromDate, ToDate, Convert.ToString(Session["LastFinYear"]), BranchID);

                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        GrdPurchaseReturn.DataBind();
                    }

                    GrdPurchaseReturn.JSProperties["cpDelete"] = "Deleted successfully.";
                }
                else if (deletecnt == -99)
                {
                    GrdPurchaseReturn.JSProperties["cpDelete"] = "Used in other module can not delete.";
                }
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


                dtdata = objPurchaseReturnBL.GetPRListGridData(userbranch, lastCompany, "PRM", FromDate, ToDate, finyear, BranchID);
                //  Session["PurchaseReturnManualListingDetails"] = dtdata;
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
            //DataTable dtdata = new DataTable();
            //dtdata = objPurchaseReturnBL.GetPurchaseReturnListGridData(userbranch, lastCompany, "PRM", FinyearStartDate, FinYearEndDate);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    GrdPurchaseReturn.DataSource = dtdata;
            //    GrdPurchaseReturn.DataBind();
            //}
            //else
            //{
            //    GrdPurchaseReturn.DataSource = null;
            //    GrdPurchaseReturn.DataBind();
            //}
        }

        #endregion

        //  protected void GrdPurchaseReturn_DataBinding(object sender, EventArgs e)
        // {
        //string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
        //string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
        //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //DataTable dtdata = new DataTable();
        //dtdata = objPurchaseReturnBL.GetPurchaseReturnListGridData(userbranch, lastCompany, "PRM", FinyearStartDate, FinYearEndDate);
        //if (dtdata != null && dtdata.Rows.Count > 0)
        //{
        //    GrdPurchaseReturn.DataSource = dtdata;

        //}
        //else
        //{
        //    GrdPurchaseReturn.DataSource = null;

        //}

        //  DataTable dsdata = (DataTable)Session["PurchaseReturnManualListingDetails"];
        // GrdPurchaseReturn.DataSource = dsdata;
        //  }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseReturn\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseReturn\DocDesign\Normal";
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
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
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

                    //var q = from d in dc.v_SalesReturnLists
                    //        where DateTime.ParseExact(d.Return_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) >=
                    //        DateTime.ParseExact(strFromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        && DateTime.ParseExact(d.Rn_Date.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        <= DateTime.ParseExact(strToDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                    //       && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.Rn_Date descending
                    //        select d;   
                    //e.QueryableSource = q;

                    var q = from d in dc.v_PurchaseReturnManualLists
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
                    var q = from d in dc.v_PurchaseReturnManualLists
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
                var q = from d in dc.v_PurchaseReturnManualLists
                        where d.BranchID == '0'
                        orderby d.Rn_Date descending
                        select d;
                e.QueryableSource = q;
            }
        }
    }
}