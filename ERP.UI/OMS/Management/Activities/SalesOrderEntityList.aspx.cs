//====================================================Revision History=========================================================================
// 1.0  Priti       V2.0.36                     Change Approval Realted Dev Express Table Bind to HTML table 
// 2.0	Sanchita	V2.0.39		10/07/2023		Need to export Sales Order Amount column in the excel format. Refer: 26533
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
//using DocumentFormat.OpenXml.Drawing.Charts;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesOrderEntityList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        public string ShowProductWiseClose { get; set; }
        public string ShowDeliverySchedule { get; set; }

      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();       
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        public bool isApprove { get; set; }
        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesOrderEntityList.aspx");


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
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");

                Session.Remove("watingOrder");
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                


                #endregion Session Remove Section End
                //ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //DataTable watingInvoice = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(userbranchHierachy);

                //waitingOrderCount.Value = Convert.ToString(watingInvoice.Rows.Count);
                //lblQuoteweatingCount.Text = Convert.ToString(watingInvoice.Rows.Count);
                //watingOrdergrid.DataSource = watingInvoice;
                //watingOrdergrid.DataBind();
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
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                //PopulateUserWiseERPDocCreation();
                //PopulateApprovalPendingCountByUserLevel();
                //PopulateERPDocApprovalPendingListByUserLevel();
            }
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

        [WebMethod]
        public static string GetSalesInvoiceIsExistInSalesInvoice(string keyValue)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.GetIdFromInvoiceOrChallan(keyValue);

            //}
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
            // Rev 2.0
            //GrdOrder.Columns[5].Visible = false;
            // End of Rev 2.0
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
        #region Sandip Section For Approval Section Start

        #region Approval Waiting or Pending User Level Wise Section Start

        public void PopulateERPDocApprovalPendingListByUserLevel() // Checked and Modified By Sandip
        {
            DataTable dtdata = new DataTable();
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    int userid = 0;
                    userid = Convert.ToInt32(Session["userid"]);

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "SO");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        //gridPendingApproval.DataSource = dtdata;
                      //  gridPendingApproval.DataBind();
                        Session["PendingApproval"] = dtdata;  // Commented For Temporary Purpose
                    }
                    else
                    {
                        //gridPendingApproval.DataSource = null;
                       // gridPendingApproval.DataBind();
                    }
                }
            }
        }

        public void PopulateApprovalPendingCountByUserLevel()  // Checked and Modified By Sandip 
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {

                    userid = Convert.ToInt32(Session["userid"]);
                }
            }
            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }
            else
            {
                lblWaiting.Text = "";
            }
        }

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
            else  if (receivedString.Split('~')[0] == "BindOrder")
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

        //REV 1.0 Start
        //protected void gridPendingApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) // Checked and Modified By Sandip
        //{
        //    gridPendingApproval.JSProperties["cpinsert"] = null;
        //    gridPendingApproval.JSProperties["cpEdit"] = null;
        //    gridPendingApproval.JSProperties["cpUpdate"] = null;
        //    gridPendingApproval.JSProperties["cpDelete"] = null;
        //    gridPendingApproval.JSProperties["cpExists"] = null;
        //    gridPendingApproval.JSProperties["cpUpdateValid"] = null;
        //    int userid = 0;
        //    if (Session["userid"] != null)
        //    {
        //        //Session.Remove("PendingApproval"); // Temporary Commented To Rebind from database due to Grid approvalval functionality
        //        userid = Convert.ToInt32(Session["userid"]);
        //        PopulateERPDocApprovalPendingListByUserLevel();
        //        gridPendingApproval.JSProperties["cpEdit"] = "F";
        //        //Session.Remove("UserWiseERPDocCreation"); // Temporary Commented To Rebind from database due to GridPending approvalval functionality effects this grid
        //    }
        //    if (Session["KeyValue"] != null)
        //    {
        //        Session.Remove("KeyValue");
        //    }

        //}
        //protected void chkapprove_Init(object sender, EventArgs e)  // Checked and Modified By Sandip
        //{
        //    ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
        //    int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
        //    KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
        //    Session["KeyValue"] = KeyValue;
        //    Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        //}
        //protected void chkreject_Init(object sender, EventArgs e) // Checked and Modified By Sandip
        //{
        //    ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
        //    int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
        //    KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
        //    Session["KeyValue"] = KeyValue;
        //    Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetRejectedQuoteId(s, e, {0}) }}", itemindex);

        //}

        //REV 1.0 End


        #endregion Approval Waiting or Pending User Level Wise Section End


        #region Created User Wise List Quotation after Clicking on Status Button Section Start  (call in page load)

        //REV 1.0 Start
        //protected void gridUserWiseQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    PopulateUserWiseERPDocCreation();
        //}
        //public void PopulateUserWiseERPDocCreation()
        //{
        //    int userid = 0;
        //    if (Session["userid"] != null)
        //    {
        //        if (Session["userbranchID"] != null)
        //        {
        //            userid = Convert.ToInt32(Session["userid"]);
        //        }
        //    }
        //    DataTable dtdata = new DataTable();
        //    //if (Session["UserWiseERPDocCreation"] == null)
        //    //{

        //    dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "SO");
        //    //}
        //    //else
        //    //{
        //    //    dtdata = (DataTable)Session["UserWiseERPDocCreation"];  // Temporary Commented By Sandip
        //    //}
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        gridUserWiseQuotation.DataSource = dtdata;
        //        gridUserWiseQuotation.DataBind();
        //        Session["UserWiseERPDocCreation"] = dtdata; // Temporary Commented By Sandip
        //    }
        //    else
        //    {
        //        gridUserWiseQuotation.DataSource = null;
        //        gridUserWiseQuotation.DataBind();
        //    }

        //}

        //REV 1.0 End
        #endregion #region Created User Wise List Quotation after Clicking on Status Button Section End


        #region To Show Hide Status and Pending Approval Button Configuration Wise Start
        public void ConditionWiseShowStatusButton()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int branchid = 0;
            if (Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(Session["userbranchID"]);
            }
            //Session["userbranchHierarchy"])

            #region Sam Section For Showing Status and Approval waiting Button on 22052017
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(2, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "SO");

            if (j == 1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(2, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "SO");

            if (k == 1)
            {
                divPendingWaiting.Visible = true;
            }
            else
            {
                divPendingWaiting.Visible = false;
            }



            #endregion Sam Section For Showing Status and Approval waiting Button on 22052017

        }

        #endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        //#region To Show Hide Status and Pending Approval Button Configuration Wise Start
        //public void ConditionWiseShowStatusButton()
        //{
        //    int i = 0;
        //    int branchid = 0;
        //    if (Session["userbranchID"] != null)
        //    {
        //        branchid = Convert.ToInt32(Session["userbranchID"]);
        //    }

        //    i = objERPDocPendingApproval.ConditionWiseShowStatusButton(2, branchid, Convert.ToString(Session["userid"]));  // 2 for Sale Order Module 
        //    if (i == 1)
        //    {
        //        spanStatus.Visible = true;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else if (i == 2)
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = false;
        //    }
        //}

        //#endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        #region After Approval Or rejected Number to reflect of Pending Approval Section  Start

        [WebMethod]
        public static string GetPendingCase()
        {
            string strPending = "(0)";

            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);

            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End
        //REV 1.0 Start
        //protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        //{
        //    PopulateERPDocApprovalPendingListByUserLevel();
        //}
        //REV 1.0 End
        #endregion Sandip Section For Approval Dtl Section End
        //REV 1.0 Start
        //protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        //{
        //    PopulateUserWiseERPDocCreation();
        //}
        //REV 1.0 End

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
            else if (availableClosed.ToUpper()=="TRUE")
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
                        //var q = from d in dc.v_GetSalesOrderEntityLists
                        //        where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.SalesOrderEntityLists
                                where d.USERID == User_id
                                orderby d.SEQ descending 
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetSalesOrderEntityLists
                        //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;

                        var q1 = from d in dc.SalesOrderEntityLists
                                 where d.CreatedBy == User_id 
                                 && d.USERID == User_id
                                 orderby  d.SEQ descending 
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
                        //var q = from d in dc.v_GetSalesOrderEntityLists
                        //        where
                        //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.SalesOrderEntityLists
                                where d.USERID == User_id
                                orderby d.SEQ descending 
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetSalesOrderEntityLists
                        //         where
                        //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;

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
                //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_GetSalesOrderEntityLists
                //        where d.Order_BranchId == -1
                //        orderby d.Order_CheckDate descending
                //        select d;
                //e.QueryableSource = q;

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.SalesOrderEntityLists
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
                    if (type=="SI")
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
                    if (type=="SI")
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

        //Add Rev Transporter Tanmoy
        [WebMethod]
        public static string InsertTransporterControlDetails(long id, String hfControlData)
        {
            CommonBL objCommonBL = new CommonBL();
            objCommonBL.InsertTransporterControlDetails(id, "SO", hfControlData, Convert.ToString(HttpContext.Current.Session["userid"]));

            return "";
        }
        //End of Rev Transporter Tanmoy

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
                    // btnCloseProduct.Visible = false;
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

        //  REV 1.0 Start
        //protected void gridPendingApproval_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["PendingApproval"] != null)
        //    {
        //        DataTable Quotationdt = (DataTable)Session["PendingApproval"];
        //        DataView dvData = new DataView(Quotationdt);
        //        gridPendingApproval.DataSource = Quotationdt;
        //    }

        //}

        //protected void gridUserWiseQuotation_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["UserWiseERPDocCreation"] != null)
        //    {
        //        DataTable Quotationdt = (DataTable)Session["UserWiseERPDocCreation"];
        //        DataView dvData = new DataView(Quotationdt);
        //        gridUserWiseQuotation.DataSource = Quotationdt;
        //    }

        //}
        //  REV 1.0 End


        [WebMethod]
        public static object ButtonCountApprovalWaiting()
        {
            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            ApprovalCount cls = new ApprovalCount();
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
                if (Convert.ToString(watingInvoice.Rows.Count)==null)
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
            ButtonShow cls = new ButtonShow();
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

        protected void EntityServerModeDataSalesOrder_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";

            string IsFilter = Convert.ToString(hdnIsFilter.Value);
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string  User_id = Convert.ToString(Session["userid"]);
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
                        string _Approved = "", _Rejected="";
                    //_Approved = _Approved + " <span class='actionInput text-center' onclick='Edit(" + item["ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Approved' ></i> </span>";
                    //_Rejected = _Rejected + " <span class='actionInput text-center' onclick='Edit(" + item["ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Rejected' ></i> </span>";

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

        [WebMethod]
        public static UserWiseData UserWiseApproval_List()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
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
                        Branch= item["Branch"].ToString(),
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


    }
    public class ApprovalCount
    {
        public String ApprovalPending { get; set; }
        public String OrderWaiting { get; set; }
       
    }

    public class ButtonShow
    {
        public String divPendingWaiting { get; set; }
        public String spanStatus { get; set; }

    }
}