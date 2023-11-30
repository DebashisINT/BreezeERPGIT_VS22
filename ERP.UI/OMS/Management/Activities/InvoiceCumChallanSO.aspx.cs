//========================================================== Revision History ============================================================================================
// 1.0   Priti V2.0.41   28/11/2023        0027028: Customer code column is required in the listing module of Sales entry
//========================================== End Revision History =======================================================================================================--%>


using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class InvoiceCumChallanSO : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();



        //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        public bool isApprove { get; set; }
        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/InvoiceCumChallanSO.aspx");


            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApproveSettingsSalesOrder = cSOrder.GetSystemSettingsResult("ApproveSettingsSalesOrder");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    //Rev 1.0
                    //GrdOrder.Columns[4].Visible = true;
                    GrdOrder.Columns[5].Visible = true;
                    //Rev 1.0 end
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    //Rev 1.0
                    //GrdOrder.Columns[4].Visible = false;
                    GrdOrder.Columns[5].Visible = false;
                    //Rev 1.0 end
                }
            }


            MasterSettings objmaster = new MasterSettings();
            string ActinveInvoice = objmaster.GetSettings("ActiveEInvoice");
            hdnActinveInvst.Value = ActinveInvoice;

            string SalesRegCust = cSOrder.GetSystemSettingsResult("EInvoiceREgisteredCustomer");

            if (!String.IsNullOrEmpty(SalesRegCust))
            {
                if (SalesRegCust.ToUpper() == "YES")
                {
                    hdnSalesinvSt.Value = "1";
                }
                else if (SalesRegCust.ToUpper().Trim() == "NO")
                {
                    hdnSalesinvSt.Value = "0";
                }
            }


            if (!String.IsNullOrEmpty(ApproveSettingsSalesOrder))
            {
                if (ApproveSettingsSalesOrder == "Yes")
                {
                    //Rev 1.0
                    //GrdOrder.Columns[12].Visible = true;
                    //GrdOrder.Columns[13].Visible = true;
                    //GrdOrder.Columns[14].Visible = true;
                    GrdOrder.Columns[13].Visible = true;
                    GrdOrder.Columns[14].Visible = true;
                    GrdOrder.Columns[15].Visible = true;
                    //Rev 1.0 End
                    isApprove = true;
                }
                else if (ApproveSettingsSalesOrder.ToUpper().Trim() == "NO")
                {
                    //Rev 1.0
                    //GrdOrder.Columns[12].Visible = false;
                    //GrdOrder.Columns[13].Visible = false;
                    //GrdOrder.Columns[14].Visible = false;
                    GrdOrder.Columns[13].Visible = false;
                    GrdOrder.Columns[14].Visible = false;
                    GrdOrder.Columns[15].Visible = false;
                    //Rev 1.0 End
                    isApprove = false;
                }
            }

            string PrintButton = cSOrder.GetSystemSettingsResult("PrintbuttonSOlistingForInvoiceDetails");
            if (!String.IsNullOrEmpty(PrintButton))
            {
                if (PrintButton == "Yes")
                {
                    hddnPrintButton.Value = "1";
                }
                else if (PrintButton.ToUpper().Trim() == "NO")
                {
                    hddnPrintButton.Value = "0";
                }
            }



            if (!IsPostBack)
            {
                Session["SO_ProductDetails"] = null;
                Session["Entry_Type"] = null;
                Session["SO_ProductDetails"] = null;
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




                Session["exportval"] = null;
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
              
                #endregion Sandip Section For Approval Dtl Section End
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
              
                if (objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["userid"])))
                {
                    hdnIsUserwiseFilter.Value = "1";
                }
                else
                {
                    hdnIsUserwiseFilter.Value = "0";
                }
                Session["InvoicePopUpPanel"] = null;

                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

                bindStatus();
            }
            #region Sandip Section For Approval Section Start
          
            #endregion Sandip Section For Approval Dtl Section End


            //FillGrid();
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = new DataTable();
            GetFinacialYearBasedQouteDate();
            string BranchID = Convert.ToString(cmbBranchfilter.Value);

            DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));
            if (HttpContext.Current.Session["LastCompany"] != null)
            {

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                {
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
                    string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
                    //dtdata = objCRMSalesOrderDtlBL.GetOrderListGridData(userbranch, lastCompany,FinyearStartDate,FinYearEndDate);
                    dtdata = objCRMSalesOrderDtlBL.GetOrderListGridDataBydate(userbranch, lastCompany, FromDate, ToDate, BranchID);
                }
            }


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GrdOrder.DataSource = dtdata;
                GrdOrder.DataBind();
            }
            else
            {
                GrdOrder.DataSource = null;
                GrdOrder.DataBind();
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

        private void bindStatus()
        {
            DataTable dtStatus = GetStatusData();
            cmbStage.DataSource = dtStatus;
            cmbStage.ValueField = "ID";
            cmbStage.TextField = "Stage_Name";
            cmbStage.DataBind();
            cmbStage.SelectedIndex = 1;
        }

        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            if (Session["LastFinYear"] != null)
            {
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrder");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetStatusData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReadyToInvoiceStatusList");
            dt = proc.GetTable();
            return dt;
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
        public void bindexport(int Filter)
        {
            GrdOrder.Columns[5].Visible = false;
            string filename = "Sales Order";
            exporter.FileName = filename;
            exporter.FileName = "SalesOrder";

            exporter.PageHeader.Left = "Sales Order";
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

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }

        [WebMethod]
        public static string GetSalesOrderStatusInvoice(string OrderId)
        {
            String ispermission = "No";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select InvoiceDetails_Id from tbl_trans_SalesInvoiceProducts where InvoiceCreatedFromDoc='SO' and InvoiceCreatedFromDocID=" + OrderId + "");
            if (dt!=null && dt.Rows.Count > 0)
            {
                ispermission = "Yes";
            }
            return Convert.ToString(ispermission);

        }

        protected void GrdOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdOrder.JSProperties["cpinsert"] = null;
            GrdOrder.JSProperties["cpEdit"] = null;
            GrdOrder.JSProperties["cpUpdate"] = null;
            GrdOrder.JSProperties["cpDelete"] = null;
            GrdOrder.JSProperties["cpExists"] = null;
            GrdOrder.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (WhichCall == "Edit")
            {

                DataTable dtOrderStatus = objCRMSalesOrderDtlBL.GetSalesOrderStatusByOrderID(WhichType);


                if (dtOrderStatus.Rows.Count > 0 && dtOrderStatus != null)
                {
                    string orderid = Convert.ToString(dtOrderStatus.Rows[0]["orderid"]);
                    string orderNumber = Convert.ToString(dtOrderStatus.Rows[0]["orderNumber"]);
                    string Status = Convert.ToString(dtOrderStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtOrderStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtOrderStatus.Rows[0]["CustomerName"]);
                    GrdOrder.JSProperties["cpEdit"] = orderid + "~"
                                                    + orderNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            else if (WhichCall == "FilterGridByDate")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataTable dtdata = new DataTable();
                dtdata = objCRMSalesOrderDtlBL.GetOrderListGridDataBydate(userbranch, lastCompany, FromDate, ToDate, BranchID);
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdOrder.DataSource = dtdata;
                    GrdOrder.DataBind();
                }
                else
                {
                    GrdOrder.DataSource = null;
                    GrdOrder.DataBind();
                }
            }
            if (WhichCall == "Delete")
            {
                deletecnt = objCRMSalesOrderDtlBL.DeleteSalesOrder(WhichType);
                if (deletecnt == 1)
                {
                    //FillGrid();
                    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetGridData();
                }
                else
                {
                    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";

                }

            }
        }
        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

            // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\Designes";
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
      

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string available = Convert.ToString(e.GetValue("IsCancel"));
            string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            if (available.ToUpper() == "TRUE")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                e.Row.Font.Strikeout = true;
            }
            else if (availableClosed.ToUpper() == "TRUE")
            {

                e.Row.ForeColor = System.Drawing.Color.Gray;

            }
            else
            {
                e.Row.ForeColor = System.Drawing.Color.Black;
            }

        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SlNo";

            //   string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string FilterType = Convert.ToString(hFilterType.Value);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int userid = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (FilterType == "Invoice_Generated")
                {
                    #region Invoice_Generated
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        && d.ReadyforInvoice == "Invoice generated"
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid && d.ReadyforInvoice == "Invoice generated"
                                    orderby d.SEQ descending
                                    select d;                                   
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         && d.ReadyforInvoice == "Invoice generated"
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid && d.ReadyforInvoice == "Invoice generated"
                                     orderby d.SEQ descending
                                     select d;    
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where
                            //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        && d.ReadyforInvoice == "Invoice generated"
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid && d.ReadyforInvoice == "Invoice generated"
                                    orderby d.SEQ descending
                                    select d;     
                                  
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where
                            //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         && d.ReadyforInvoice == "Invoice generated"
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid && d.ReadyforInvoice == "Invoice generated"
                                     orderby d.SEQ descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }
                    hFilterType.Value = "";
                    #endregion
                }
                else if (FilterType == "PendingforInvoice")
                {
                    #region Pending for Invoice
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        && d.ReadyforInvoice == "Ready For Invoice"
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid  && d.ReadyforInvoice == "Ready For Invoice"
                                    orderby d.SEQ descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         && d.ReadyforInvoice == "Ready For Invoice"
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid && d.ReadyforInvoice == "Ready For Invoice"
                                     orderby d.SEQ descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where
                            //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        && d.ReadyforInvoice == "Ready For Invoice"
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid && d.ReadyforInvoice == "Ready For Invoice"
                                    orderby d.SEQ descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where
                            //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         && d.ReadyforInvoice == "Ready For Invoice"
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid && d.ReadyforInvoice == "Ready For Invoice"
                                     orderby d.SEQ descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }
                    hFilterType.Value = "";
                    #endregion
                }
                else
                {
                    #region All
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid 
                                    orderby d.SEQ descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                            //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid
                                     orderby d.SEQ descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //        where
                            //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                            //        orderby d.Order_CheckDate descending
                            //        select d;
                            //e.QueryableSource = q;

                            var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                                    where d.USERID == userid
                                    orderby d.SEQ descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            //var q1 = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                            //         where
                            //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                            //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                            //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                            //         orderby d.Order_CheckDate descending
                            //         select d;
                            //e.QueryableSource = q1;

                            var q1 = from d in dc.InvoiceCumChallanWithSOEntityLists
                                     where d.USERID == userid && d.CreatedBy == userid
                                     orderby d.SEQ descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }
                    #endregion                    
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.V_GetInvoiceCumChallanWithSOEntityLists
                //        where d.Order_BranchId == -1
                //        orderby d.Order_CheckDate descending
                //        select d;
                //e.QueryableSource = q;

                var q = from d in dc.InvoiceCumChallanWithSOEntityLists
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
            }


        }

        protected void InvoiceNumberpanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {
                Session["InvoicePopUpPanel"] = null;
                CRMSalesOrderDtlBL objsalesOrderBL = new CRMSalesOrderDtlBL();
                DataTable InvoiceNumberdt = objsalesOrderBL.PopulateSalesInvoiceNumberByOrderId(Convert.ToString(data[1]));
                Session["InvoicePopUpPanel"] = InvoiceNumberdt;
                grdInvoiceNumber.DataSource = InvoiceNumberdt;
                grdInvoiceNumber.DataBind();

            }
        }

        protected void InvoiceDatepanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {
                Session["InvoicePopUpPanel"] = null;
                CRMSalesOrderDtlBL objsalesOrderBL = new CRMSalesOrderDtlBL();
                DataTable InvoiceDatedt = objsalesOrderBL.PopulateSalesInvoiceNumberByOrderId(Convert.ToString(data[1]));
                Session["InvoicePopUpPanel"] = InvoiceDatedt;
                grdInvoiceDate.DataSource = InvoiceDatedt;
                grdInvoiceDate.DataBind();

            }
        }

        protected void InvoiceNumberpanel_DataBinding(object sender, EventArgs e)
        {
            if (Session["InvoicePopUpPanel"] != null)
            {
                grdInvoiceNumber.DataSource = (DataTable)Session["InvoicePopUpPanel"];
            }
        }
        protected void InvoiceDatepanel_DataBinding(object sender, EventArgs e)
        {
            if (Session["InvoicePopUpPanel"] != null)
            {
                grdInvoiceDate.DataSource = (DataTable)Session["InvoicePopUpPanel"];
            }
        }



        protected void SelectInvoicePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string type = e.Parameter.Split('~')[1];
            if (strSplitCommand == "Bindalldesignes")
            {
                //  BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtRptModules = new DataTable();

                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    if (type == "SI")
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal";
                    }
                    else
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit";
                    }
                }
                else
                {
                    if (type == "SI")
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit";
                    }
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
                    //if (reportValue != SavereportValue)
                    //{
                    CmbInvoiceDesignName.Items.Add(name, reportValue);
                    //}
                }
                CmbInvoiceDesignName.SelectedIndex = 0;
                SelectInvoicePanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;

                string reportName = Convert.ToString(CmbInvoiceDesignName.Value);
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
                if (selectOfficecopy.Checked == true)
                {
                    NoofCopy += 5 + ",";
                }
                SelectInvoicePanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectInvoicePanel.JSProperties["cpChecked"] = "Checked";
                SelectInvoicePanel.JSProperties["cpType"] = type;
            }
        }


        [WebMethod]
        public static string InsertUpdateReadyToInvoice(string Orderid, string Stage, string Remarks)
        {
            String Statuss = "";
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReadyToInvoiceInsertUpdate");
            proc.AddPara("@Document_ID", Orderid);
            proc.AddPara("@Status", Stage);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@USER_ID", userid);
            int i = proc.RunActionQuery();
            if (i > 0)
            {
                Statuss = "OK";
            }
            return Convert.ToString(Statuss);
        }

        [WebMethod]
        public static object viewReadyToInvoice(string Orderid)
        {
            ShowReadyToInvoiceInvoicecum Statuss = new ShowReadyToInvoiceInvoicecum();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReadyToInvoiceInsertUpdate");
            proc.AddPara("@ACTION", "FETCH");
            proc.AddPara("@Document_ID", Orderid);
            proc.AddPara("@USER_ID", userid);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                Statuss.Status = dt.Rows[0]["Status"].ToString();
                Statuss.Remarks = dt.Rows[0]["Remarks"].ToString();
            }
            return Statuss;
        }

        [WebMethod]
        public static object ButtonCountShow(String FormDate, String toDate, String Branch)
        {
            InvoiceCumChallanSOCount cls = new InvoiceCumChallanSOCount();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReadyToInvoiceInsertUpdate");
            proc.AddPara("@ACTION", "InvoiceCumChallanSOCount");
            proc.AddPara("@FormDate", FormDate);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@Branch", Branch);
            proc.AddPara("@USER_ID", userid);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                cls.InvoiceGenerated = dt.Rows[0]["InvoiceGenerated"].ToString();
                cls.PendingforInvoice = dt.Rows[0]["PendingforInvoice"].ToString();
                cls.All = dt.Rows[0]["All"].ToString();
            }
            return cls;
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
            Task PopulateStockTrialDataTask = new Task(() => GetInvoiceCumChallanSOdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetInvoiceCumChallanSOdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_InvoiceCumChallanWithSOEntity_List", con);
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

    public class InvoiceCumChallanSOCount
    {
        public String InvoiceGenerated { get; set; }
        public String PendingforInvoice { get; set; }
        public String All { get; set; }
    }

    public class ShowReadyToInvoiceInvoicecum
    {
        public String Status { get; set; }
        public String Remarks { get; set; }
    }
}