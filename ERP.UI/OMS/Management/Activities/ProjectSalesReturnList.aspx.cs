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
using System.Linq;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.IO;
using ERP.Models;
using System.Globalization;
using System.Reflection;
namespace ERP.OMS.Management.Activities
{
    public partial class ProjectSalesReturnList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SalesReturnBL objSalesReturnBL = new SalesReturnBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectSalesReturnList.aspx");

            if (!IsPostBack)
            {

                Session["InlineRemarks"] = null;
                CommonBL cSOrder = new CommonBL();
                string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
                if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
                {
                    if (ProjectSelectInEntryModule == "Yes")
                    {
                        GrdSalesReturn.Columns[11].Visible = true;


                    }
                    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                        GrdSalesReturn.Columns[11].Visible = false;

                    }
                }


                Session.Remove("ReturnSalesListingDetails");
                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

                    PopulateBranchByHierchy(userbranch);

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    //  GetFinacialYearBasedQouteDate();
                    //string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
                    // string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
                    //   GetSalesReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);

                    GrdSalesReturn.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesReturnListDetailsGrid";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesReturnListDetailsGrid');</script>");

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
            // GrdSalesReturn.Columns[6].Visible = false;
            GrdSalesReturn.Columns[9].Visible = false;
            string filename = "Sales Return";
            exporter.FileName = filename;
            // exporter.FileName = "GrdSalesReturn";
            exporter.Landscape = true;
            exporter.PaperKind = System.Drawing.Printing.PaperKind.A4;
            exporter.MaxColumnWidth = 200;
            exporter.PageHeader.Left = "Sales Return";
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
        [WebMethod]
        public static string UpdateEWayBill(string ReturnID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue)
        {
            SalesReturnBL objSalesReturnBL = new SalesReturnBL();
            int EWayBill = 0;
            EWayBill = objSalesReturnBL.UpdateEWayBillForSalesReturn(ReturnID, UpdateEWayBill, EWayBillDate, EWayBillValue);
            return Convert.ToString(EWayBill);
        }

        #region Export Grid Section Start

        protected void GrdSalesReturn_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            if (Command == "Delete")
            {
                string SalesReturnID = Convert.ToString(e.Parameters).Split('~')[1];
                int deletecnt = 0;
                deletecnt = objSalesReturnBL.DeleteSalesReturnDetails(SalesReturnID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), "SR", Convert.ToString(Session["userbranchID"]));

                if (deletecnt == 1)
                {

                    string BranchID = Convert.ToString(cmbBranchfilter.Value);


                    string FromDate = Convert.ToString(FormDate.Value);
                    string ToDate = Convert.ToString(toDate.Value);

                    DataTable dtdata = new DataTable();
                    dtdata = objSalesReturnBL.GetSalesReturnListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), BranchID, FromDate, ToDate);
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        // Session["ReturnSalesListingDetails"] = dtdata;
                        GrdSalesReturn.DataBind();
                    }

                    GrdSalesReturn.JSProperties["cpDelete"] = "Deleted successfully";


                }
                else
                {
                    GrdSalesReturn.JSProperties["cpDelete"] = "Used in other module.can not delete.";
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

                dtdata = objSalesReturnBL.GetSalesReturnListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);

                // Session["ReturnSalesListingDetails"] = dtdata;
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdSalesReturn.DataSource = dtdata;
                    GrdSalesReturn.DataBind();
                }
                else
                {
                    GrdSalesReturn.DataSource = null;
                    GrdSalesReturn.DataBind();
                }

            }
        }
        //public void GetSalesReturnListGridData(string userbranch, string lastCompany, string FinyearStartDate, string FinYearEndDate)
        //{
        //    DataTable dtdata = new DataTable();
        //    dtdata = objSalesReturnBL.GetSalesReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        GrdSalesReturn.DataSource = dtdata;
        //        GrdSalesReturn.DataBind();
        //    }
        //    else
        //    {
        //        GrdSalesReturn.DataSource = null;
        //        GrdSalesReturn.DataBind();
        //    }
        //}

        #endregion

        // protected void GrdSalesReturn_DataBinding(object sender, EventArgs e)
        // {
        //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);


        //string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
        //string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);

        //DataTable dtdata = new DataTable();
        //dtdata = objSalesReturnBL.GetSalesReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
        //if (dtdata != null && dtdata.Rows.Count > 0)
        //{
        //    GrdSalesReturn.DataSource = dtdata;

        //}
        //else
        //{
        //    GrdSalesReturn.DataSource = null;

        //}

        //  DataTable dsdata = (DataTable)Session["ReturnSalesListingDetails"];
        // GrdSalesReturn.DataSource = dsdata;
        //   }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesReturn\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesReturn\DocDesign\Normal";
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
            //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

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

                    var q = from d in dc.v_ProjectSalesReturnLists
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
                    var q = from d in dc.v_ProjectSalesReturnLists
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
                var q = from d in dc.v_ProjectSalesReturnLists
                        where d.BranchID == '0'
                        orderby d.Rn_Date descending
                        select d;
                e.QueryableSource = q;
            }
        }

        //warehouse kaushik


        //warehouse kaushik


        [WebMethod]
        public static object GetInfluencerDetails(string invid)
        {
            DataSet ds = new DataSet();
            SalesReturnBL SRSale = new SalesReturnBL();
            ds = SRSale.GetInfluencerDetails(invid);

            Inf_Header_Details IHD = new Inf_Header_Details();

            INF_Inv_Details inv = new INF_Inv_Details();


            inv.Inv_Id = Convert.ToString(ds.Tables[0].Rows[0]["Inv_Id"]);
            inv.Inv_No = Convert.ToString(ds.Tables[0].Rows[0]["Inv_No"]);
            inv.Amount = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
            inv.Inv_BranchId = Convert.ToString(ds.Tables[0].Rows[0]["Inv_BranchId"]);

            IHD.INF_Inv_Details = inv;

            Influencer inf = new Influencer();
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                inf.MainAccount_AccountCode = Convert.ToString(ds.Tables[2].Rows[0]["MainAccount_AccountCode"]);
                inf.CALCULATED_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["CALCULATED_AMOUNT"]);
                inf.MAINACCOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["MAINACCOUNT_DR"]);
                inf.AMOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["AMOUNT_DR"]);
                inf.AUTOJV_ID = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_ID"]);
                inf.AUTOJV_NUMBER = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_NUMBER"]);
                inf.COMM_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["COMM_AMOUNT"]);
                inf.POSTING_DATE = Convert.ToDateTime(ds.Tables[2].Rows[0]["POSTING_DATE"]);
                inf.IsTagged = Convert.ToString(ds.Tables[2].Rows[0]["TaggedCount"]);
            }

            IHD.Influencer = inf;




            IHD.INF_Inv_Products = (from DataRow dr in ds.Tables[1].Rows
                                    select new INF_Inv_Products()
                                    {

                                        prod_details_id = Convert.ToString(dr["prod_details_id"]),
                                        Prod_id = Convert.ToString(dr["Prod_id"]),
                                        Prod_description = Convert.ToString(dr["Prod_description"]),
                                        prod_Qty = Convert.ToString(dr["prod_Qty"]),
                                        prod_Salesprice = Convert.ToString(dr["prod_Salesprice"]),
                                        prod_amt = Convert.ToString(dr["prod_amt"]),
                                        prod_SalespriceWithGST = Convert.ToString(dr["prod_amtWGST"]),
                                        Prod_Percentage = Convert.ToString(dr["Prod_Percentage"]),
                                        Applicable_On = Convert.ToString(dr["Applicable_On"]),
                                        PROD_COMM_AMOUNT = Convert.ToString(dr["PROD_COMM_AMOUNT"])


                                    }).ToList();

            IHD.Influencer_Details = (from DataRow dr in ds.Tables[3].Rows
                                      select new Influencer_Details()
                                      {

                                          DET_AMOUNT_CR = Convert.ToString(dr["DET_AMOUNT_CR"]),
                                          DET_INFLUENCER_ID = Convert.ToString(dr["DET_INFLUENCER_ID"]),
                                          INF_Name = Convert.ToString(dr["INF_Name"]),
                                          DET_MAINACCOUNT_CR = Convert.ToString(dr["DET_MAINACCOUNT_CR"]),
                                          DET_MAINACCOUNT_NAME = Convert.ToString(dr["DET_MAINACCOUNT_NAME"])
                                      }).ToList();



            if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                IHD.RemainingBalance = Convert.ToString(ds.Tables[4].Rows[0]["RemainingQty"]);
            }


            return IHD;
        }

        [WebMethod]
        public static object SaveInfluencer(infulencerSaveData infsave)
        {
            DataTable Prod = CreateDataTable(infsave.product);
            DataTable Influencer = CreateDataTable(infsave.Influencer);

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerDataReturn(infsave, Prod, Influencer, "");

            return output;
        }
        [WebMethod]
        public static object DeleteInfluencer(string Invoice_Id)
        {

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.DeleteInfluencerData(Invoice_Id);

            return output;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, typeof(System.String)));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                } dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}