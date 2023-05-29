//====================================================Revision History=========================================================================
// Create by: PRITI on 29-05-2023. Refer:
//0025832: Create New page for Sales Order Listing & ADD
//====================================================End Revision History=====================================================================


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
using System.Linq;
using iTextSharp.text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using BusinessLogicLayer.ServiceManagement;
using Aspose.Pdf.Kit;
namespace ERP.OMS.Management.Activities
{
    public partial class SalesOrderNewList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        public string ShowProductWiseClose { get; set; }
        public string ShowDeliverySchedule { get; set; }

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();       
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();       
        public bool isApprove { get; set; }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesOrderNewList.aspx");


            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApproveSettingsSalesOrder = cSOrder.GetSystemSettingsResult("ApproveSettingsSalesOrder");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    GrdOrder.Columns[4].Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GrdOrder.Columns[4].Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(ApproveSettingsSalesOrder))
            {
                if (ApproveSettingsSalesOrder == "Yes")
                {
                    GrdOrder.Columns[17].Visible = true;
                    GrdOrder.Columns[18].Visible = true;
                    GrdOrder.Columns[19].Visible = true;
                    isApprove = true;
                }
                else if (ApproveSettingsSalesOrder.ToUpper().Trim() == "NO")
                {
                    GrdOrder.Columns[17].Visible = false;
                    GrdOrder.Columns[18].Visible = false;
                    GrdOrder.Columns[19].Visible = false;
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

            string CloseQuantityRequire = cSOrder.GetSystemSettingsResult("CloseQuantityRequire");
            if (!String.IsNullOrEmpty(CloseQuantityRequire))
            {
                if (CloseQuantityRequire == "Yes")
                {
                    ShowProductWiseClose = "1";
                }
                else if (CloseQuantityRequire.ToUpper().Trim() == "NO")
                {
                    ShowProductWiseClose = "0";
                }
            }
            else
            {
                ShowProductWiseClose = "0";
            }

            string DeliveryScheduleRequired = cSOrder.GetSystemSettingsResult("DeliveryScheduleRequired");
            if (!String.IsNullOrEmpty(DeliveryScheduleRequired))
            {
                if (DeliveryScheduleRequired == "Yes")
                {
                    ShowDeliverySchedule = "1";
                }
                else if (DeliveryScheduleRequired.ToUpper().Trim() == "NO")
                {
                    ShowDeliverySchedule = "0";
                }
            }
            else
            {
                ShowDeliverySchedule = "0";
            }



            string IsMultiuserApprovalRequired = cSOrder.GetSystemSettingsResult("IsMultiuserApprovalRequired");
            hdnIsMultiuserApprovalRequired.Value = IsMultiuserApprovalRequired;


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
               
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");

                Session.Remove("watingOrder");
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");

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
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
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

        }

        #region Export
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

        #endregion Export

        #region AjaxCall
        [WebMethod]
        public static string GetSalesInvoiceIsExistInSalesInvoice(string keyValue)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.GetIdFromInvoiceOrChallan(keyValue);
            return Convert.ToString(ispermission);

        }

        [WebMethod]
        public static string GetTotalWatingOrderCount()
        {
            CRMSalesOrderDtlBL CRMSales = new CRMSalesOrderDtlBL();
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            return Convert.ToString(CRMSales.GetorderCount(userbranchHierachy));
        }


        [WebMethod]
        public static string CancelSalesOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.CancelSalesOrder(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        [WebMethod]
        public static string ClosedSalesOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.ClosedSalesOrder(keyValue, Reason);
            return Convert.ToString(CancelOrder);
        }


        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }

        [WebMethod]
        public static string GetPendingCase()
        {
            string strPending = "(0)";

            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);

            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        [WebMethod]
        public static string InsertTransporterControlDetails(long id, String hfControlData)
        {
            CommonBL objCommonBL = new CommonBL();
            objCommonBL.InsertTransporterControlDetails(id, "SO", hfControlData, Convert.ToString(HttpContext.Current.Session["userid"]));

            return "";
        }
        [WebMethod]
        public static object ButtonCountApprovalWaiting()
        {
            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            ApprovalCountNew cls = new ApprovalCountNew();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                cls.ApprovalPending = dtdata.Rows[0]["ID"].ToString();
            }

            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            DataTable watingInvoice = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(userbranchHierachy);

            if (watingInvoice != null && watingInvoice.Rows.Count > 0)
            {
                if (Convert.ToString(watingInvoice.Rows.Count) == null)
                {
                    cls.OrderWaiting = Convert.ToString("0");
                }
                else
                {
                    cls.OrderWaiting = Convert.ToString(watingInvoice.Rows.Count);
                }

            }

            return cls;
        }

        [WebMethod]
        public static object ApprovalButtonVisible()
        {
            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            ButtonShowNew cls = new ButtonShowNew();
            int i = 0;
            int j = 0;
            int k = 0;
            int branchid = 0;
            if (HttpContext.Current.Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(HttpContext.Current.Session["userbranchID"]);
            }
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(2, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["userid"]), "SO");

            if (j == 1)
            {
                cls.spanStatus = "1";
            }
            else
            {
                cls.spanStatus = "0";
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(2, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["userid"]), "SO");

            if (k == 1)
            {
                cls.divPendingWaiting = "1";
            }
            else
            {
                cls.divPendingWaiting = "0";
            }

            return cls;
        }
        [WebMethod]
        public static bool SalesOrderApproval(string keyValue)
        {
            Boolean Success = false;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ApprovalCheckforMultiUserApproval");
            proc.AddVarcharPara("@Action", 500, "Sales Order");
            proc.AddVarcharPara("@User_Id", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ModuleId", 50, keyValue);
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                Success = true;
            }

            return Success;

        }
        [WebMethod]
        public static PendingApprovalData PendingApproval_List()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
            PendingApprovalData ret = new PendingApprovalData();
            List<PendingApprovalDataList> listStatues = new List<PendingApprovalDataList>();

            DataTable dtdata = new DataTable();
            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = 0;
            userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "SO");

            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                foreach (DataRow item in dtdata.Rows)
                {
                    string _Approved = "", _Rejected = "";                   
                    _Approved = _Approved + " <input type='checkbox' class='form-check-input' onclick='OnGetApprovedRowValues(" + item["ID"].ToString() + ")'>";
                    _Rejected = _Rejected + " <input type='checkbox' class='form-check-input' onclick='OnGetRejectedRowValues(" + item["ID"].ToString() + ")'>";

                    listStatues.Add(new PendingApprovalDataList
                    {
                        DocumentNo = item["Number"].ToString(),
                        PartyName = item["customer"].ToString(),
                        PostingDate = item["CreateDate"].ToString(),
                        Unit = item["branch_description"].ToString(),
                        EnteredBy = item["craetedby"].ToString(),
                        Approved = _Approved,
                        Rejected = _Rejected,

                    });
                }
                ret.DetailsList = listStatues;
            }
            return ret;
        }
        [WebMethod]
        public static UserWiseData UserWiseApproval_List()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            UserWiseData ret = new UserWiseData();
            List<UserWiseDataList> listStatues = new List<UserWiseDataList>();
            DataTable dtdata = new DataTable();
            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = 0;
            userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "SO");

            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                foreach (DataRow item in dtdata.Rows)
                {
                    listStatues.Add(new UserWiseDataList
                    {
                        Branch = item["Branch"].ToString(),
                        SaleOrderNo = item["number"].ToString(),
                        Date = item["OrderedDate"].ToString(),
                        Customer = item["Customer"].ToString(),
                        ApprovalUser = item["approvedby"].ToString(),
                        UserLevel = item["UserLevel"].ToString(),
                        Status = item["status"].ToString(),
                    });
                }
                ret.DetailsList = listStatues;
            }
            return ret;
        }
        #endregion AjaxCall

        #region Grid Sales Oder Listing
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
            if (WhichCall == "Delete")
            {
                deletecnt = objCRMSalesOrderDtlBL.DeleteSalesOrder(WhichType);
                if (deletecnt == 1)
                {

                    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";

                }
                else
                {
                    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";

                }

            }
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

        #endregion Grid Sales Oder Listing

        #region Printing
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
        #endregion Printing
        protected void watingOrdergrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receivedString = e.Parameters;

            if (receivedString.Split('~')[0] == "Remove")
            {
                string key = receivedString.Split('~')[1];
                objCRMSalesOrderDtlBL.DeleteBasketDetailsFromtable(key, Convert.ToInt32(Session["userid"]));
                watingOrdergrid.JSProperties["cpReturnMsg"] = "Billing Request has been Deleted Successfully.";
                watingOrdergrid.DataBind();
            }
            else if (receivedString.Split('~')[0] == "BindOrder")
            {
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                DataTable watingInvoice = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(userbranchHierachy);
                watingOrdergrid.DataSource = watingInvoice;
                watingOrdergrid.DataBind();
                Session["watingOrder"] = watingInvoice;

            }
        }

        protected void watingOrdergrid_DataBinding(object sender, EventArgs e)
        {
            //watingOrdergrid.DataSource = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (Session["watingOrder"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["watingOrder"];
                //gridPendingApproval.DataSource = Quotationdt;
            }
        }

        //protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        //{
        //    e.Text = string.Format("{0}", e.Value);
        //}

       

        #region Sales Order Grid Listing Bind
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SlNo";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                    if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                    {
                        var q = from d in dc.SalesOrderEntityLists
                                where d.USERID == User_id
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {

                        var q1 = from d in dc.SalesOrderEntityLists
                                 where d.CreatedBy == User_id
                                 && d.USERID == User_id
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

                        var q = from d in dc.SalesOrderEntityLists
                                where d.USERID == User_id
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {

                        var q1 = from d in dc.SalesOrderEntityLists
                                 where d.CreatedBy == User_id
                                  && d.USERID == User_id
                                 orderby d.SEQ descending
                                 select d;
                        e.QueryableSource = q1;
                    }
                }
            }
            else
            {

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.SalesOrderEntityLists
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
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
            Task PopulateStockTrialDataTask = new Task(() => GetSalesOrderdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetSalesOrderdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_SalesOrder_List", con);
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
        #endregion Sales Order Grid Listing Bind


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
                    CmbInvoiceDesignName.Items.Add(name, reportValue);

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

        protected void gridProductwiseClose_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] data = e.Parameters.Split('~');

            if (data[0] == "ShowData")
            {
                Session["ProductPopUpPanel"] = null;
                DataTable Productdt = PopulateProductDetailsByOrderId(Convert.ToString(data[1]));
                Session["ProductPopUpPanel"] = Productdt;
                gridProductwiseClose.DataSource = Productdt;
                gridProductwiseClose.DataBind();

                if (Productdt.Rows.Count == 0)
                {
                    gridProductwiseClose.JSProperties["cpBtnCloseVIsible"] = "NO";

                }

            }
            else if (data[0] == "CloseData")
            {
                String OrderId = "";
                string ProductId = "";
                string OrderDetails_Id = "";

                if (gridProductwiseClose.GetSelectedFieldValues("OrderDetails_Id").Count > 0)
                {
                    for (int i = 0; i < gridProductwiseClose.GetSelectedFieldValues("OrderDetails_Id").Count; i++)
                    {

                        OrderId += "," + Convert.ToString(gridProductwiseClose.GetSelectedFieldValues("OrderDetails_OrderId")[i]);
                        ProductId += "," + Convert.ToString(gridProductwiseClose.GetSelectedFieldValues("OrderDetails_ProductId")[i]);
                        OrderDetails_Id += "," + Convert.ToString(gridProductwiseClose.GetSelectedFieldValues("OrderDetails_Id")[i]);

                    }
                    OrderId = OrderId.TrimStart(',');
                    ProductId = ProductId.TrimStart(',');
                    OrderDetails_Id = OrderDetails_Id.TrimStart(',');

                    int Closecnt = 0;
                    Closecnt = CloseProductDetailsByDetailsId(Convert.ToString(OrderId), OrderDetails_Id, ProductId);
                    if (Closecnt == 1)
                    {
                        gridProductwiseClose.JSProperties["cpProductClose"] = "YES";
                    }
                    else
                    {
                        gridProductwiseClose.JSProperties["cpProductClose"] = "NO";
                    }

                }
            }

        }
        public int CloseProductDetailsByDetailsId(string strOrderID, string Details_Id, string Product_Id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_SALESORDER_DETAILS");
            proc.AddVarcharPara("@Action", 50, "ClosePurchaseOrderProductsWise");
            proc.AddVarcharPara("@SALESORDER_ID", 20, strOrderID);
            proc.AddVarcharPara("@Details_Id", 1000, Details_Id);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Id);
            proc.AddVarcharPara("@UserId", 1000, Convert.ToString(Session["userid"]));
            proc.AddVarcharPara("@ReturnValueDelete", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValueDelete"));
            return rtrnvalue;
        }
        public DataTable PopulateProductDetailsByOrderId(string strOrderID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SALESORDER_DETAILS");
            proc.AddVarcharPara("@Action", 50, "PopulatePurchaseOrderProducts");
            proc.AddVarcharPara("@SALESORDER_ID", 20, strOrderID);
            dt = proc.GetTable();
            return dt;
        }

        protected void gridProductwiseClose_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void gridProductwiseClose_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void gridProductwiseClose_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;

        }


        
        protected void EntityServerModeDataSalesOrder_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";

            string IsFilter = Convert.ToString(hdnIsFilter.Value);
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string User_id = Convert.ToString(Session["userid"]);
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            if (IsFilter == "Y")
            {
                var q = from d in dc.v_PendingApprovals
                        where d.ERPApprover_UserId == Convert.ToInt64(User_id)
                        orderby d.CreateDate descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {

                var q = from d in dc.v_PendingApprovals
                        where d.ERPApprover_UserId == 0
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void EntityServerModeDataSalesOrderUserWise_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";

            string IsFilter = Convert.ToString(hdnIsFilter.Value);
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string User_id = Convert.ToString(Session["userid"]);
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();


            if (IsFilter == "Y")
            {
                var q = from d in dc.v_SalesOrderStatus
                        where d.CreatedBy == Convert.ToInt64(User_id)
                        orderby d.ID, d.UserLevel descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {

                var q = from d in dc.v_SalesOrderStatus
                        where d.CreatedBy == 0
                        select d;
                e.QueryableSource = q;
            }

        }

        #region class
        public class PendingApprovalData
        {

            public List<PendingApprovalDataList> DetailsList { get; set; }
        }
        public class PendingApprovalDataList
        {
            public String DocumentNo { get; set; }
            public string PartyName { get; set; }
            public string PostingDate { get; set; }
            public string Unit { get; set; }
            public string EnteredBy { get; set; }
            public string Approved { get; set; }
            public string Rejected { get; set; }

        }

        public class UserWiseData
        {
            public List<UserWiseDataList> DetailsList { get; set; }
        }

        public class UserWiseDataList
        {
            public String Branch { get; set; }
            public string SaleOrderNo { get; set; }
            public string Date { get; set; }
            public string Customer { get; set; }
            public string ApprovalUser { get; set; }
            public string UserLevel { get; set; }
            public string Status { get; set; }

        }

        #endregion class

    }

    public class ApprovalCountNew
    {
        public String ApprovalPending { get; set; }
        public String OrderWaiting { get; set; }

    }

    public class ButtonShowNew
    {
        public String divPendingWaiting { get; set; }
        public String spanStatus { get; set; }

    }

}
