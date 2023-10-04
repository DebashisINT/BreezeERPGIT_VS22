using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class projectOrderList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        public string ShowProductWiseClose { get; set; }
        

        //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        decimal TotalBasicAmount = 0, TotalAmount = 0;
        public bool isApprove { get; set; }
        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/projectOrderList.aspx");
           

            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApproveProjSalesOrder = cSOrder.GetSystemSettingsResult("ApproveProjSalesOrder");


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


            if (!String.IsNullOrEmpty(ApproveProjSalesOrder))
            {
                if (ApproveProjSalesOrder == "Yes")
                {
                    GrdOrder.Columns[25].Width = 150;
                    GrdOrder.Columns[5].Width = 150;
                    GrdOrder.Columns[6].Width = 150;
                    isApprove = true;
                }
                else if (ApproveProjSalesOrder.ToUpper().Trim() == "NO")
                {
                    GrdOrder.Columns[25].Width = 0;
                    GrdOrder.Columns[5].Width = 0;
                    GrdOrder.Columns[6].Width = 0;
                    isApprove = false;

                }
            }
            string IsMultiuserApprovalRequired = cSOrder.GetSystemSettingsResult("IsMultiuserApprovalRequired");
            hdnIsMultiuserApprovalRequired.Value = IsMultiuserApprovalRequired;

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

            // Rev Sanchita
            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) Dataedit_Fromdate,convert(varchar(10),Lock_Todate,105) Dataedit_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=61");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) Datadelete_Fromdate,convert(varchar(10),Lock_Todate,105) Datadelete_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=61");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["Dataedit_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["Dataedit_Todate"]);

                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "DATA is Freezed between   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + " for Edit .";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["Datadelete_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["Datadelete_Todate"]);
                spnDeleteLock.InnerText = spnEditLock.InnerText + "DATA is Freezed between    " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + " for Delete.";
                spnEditLock.InnerText = "";
            }
            // End of Rev Sanchita

            if (!IsPostBack)
            {
                Session["Entry_Type"] = null;
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
                ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                DataTable watingInvoice = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(userbranchHierachy);

                waitingOrderCount.Value = Convert.ToString(watingInvoice.Rows.Count);
                lblQuoteweatingCount.Text = Convert.ToString(watingInvoice.Rows.Count);
                watingOrdergrid.DataSource = watingInvoice;
                watingOrdergrid.DataBind();
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
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
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
        public static string CancelProjectSalesOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.CancelProjectSalesOrder(keyValue, Reason);


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
            //GrdOrder.Columns[5].Visible = false;
            string filename = "Project Sales  Order";
            exporter.FileName = filename;
            exporter.FileName = "ProjectOrder";

            exporter.PageHeader.Left = "Project Sales Order";
            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    //exporter.WriteXlsToResponse();
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
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
                deletecnt = objCRMSalesOrderDtlBL.DeleteProjectOrder(WhichType);
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
                        gridPendingApproval.DataSource = dtdata;
                        gridPendingApproval.DataBind();
                        //Session["PendingApproval"] = dtdata;  // Commented For Temporary Purpose
                    }
                    else
                    {
                        gridPendingApproval.DataSource = null;
                        gridPendingApproval.DataBind();
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
        }

        protected void watingOrdergrid_DataBinding(object sender, EventArgs e)
        {
            watingOrdergrid.DataSource = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
        }
        protected void gridPendingApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) // Checked and Modified By Sandip
        {
            gridPendingApproval.JSProperties["cpinsert"] = null;
            gridPendingApproval.JSProperties["cpEdit"] = null;
            gridPendingApproval.JSProperties["cpUpdate"] = null;
            gridPendingApproval.JSProperties["cpDelete"] = null;
            gridPendingApproval.JSProperties["cpExists"] = null;
            gridPendingApproval.JSProperties["cpUpdateValid"] = null;
            int userid = 0;
            if (Session["userid"] != null)
            {
                //Session.Remove("PendingApproval"); // Temporary Commented To Rebind from database due to Grid approvalval functionality
                userid = Convert.ToInt32(Session["userid"]);
                PopulateERPDocApprovalPendingListByUserLevel();
                gridPendingApproval.JSProperties["cpEdit"] = "F";
                //Session.Remove("UserWiseERPDocCreation"); // Temporary Commented To Rebind from database due to GridPending approvalval functionality effects this grid
            }
            if (Session["KeyValue"] != null)
            {
                Session.Remove("KeyValue");
            }

        }

        protected void chkapprove_Init(object sender, EventArgs e)  // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        }


        protected void chkreject_Init(object sender, EventArgs e) // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetRejectedQuoteId(s, e, {0}) }}", itemindex);

        }

        #endregion Approval Waiting or Pending User Level Wise Section End
        #region Created User Wise List Quotation after Clicking on Status Button Section Start  (call in page load)

        protected void gridUserWiseQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }
        public void PopulateUserWiseERPDocCreation()
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
            //if (Session["UserWiseERPDocCreation"] == null)
            //{

            dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "SO");
            //}
            //else
            //{
            //    dtdata = (DataTable)Session["UserWiseERPDocCreation"];  // Temporary Commented By Sandip
            //}
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridUserWiseQuotation.DataSource = dtdata;
                gridUserWiseQuotation.DataBind();
                //Session["UserWiseERPDocCreation"] = dtdata; // Temporary Commented By Sandip
            }
            else
            {
                gridUserWiseQuotation.DataSource = null;
                gridUserWiseQuotation.DataBind();
            }

        }
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

        protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateERPDocApprovalPendingListByUserLevel();
        }






        #endregion Sandip Section For Approval Dtl Section End

        protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateUserWiseERPDocCreation();
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
                        var q = from d in dc.v_ProjectOrderEntityLists
                                where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                orderby d.Order_CheckDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        var q1 = from d in dc.v_ProjectOrderEntityLists
                                 where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                 && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                 && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 orderby d.Order_CheckDate descending
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
                        var q = from d in dc.v_ProjectOrderEntityLists
                                where
                                d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                orderby d.Order_CheckDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        var q1 = from d in dc.v_ProjectOrderEntityLists
                                 where
                                 d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                 branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                 && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 orderby d.Order_CheckDate descending
                                 select d;
                        e.QueryableSource = q1;
                    }
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_ProjectOrderEntityLists
                        where d.Order_BranchId == -1
                        orderby d.Order_CheckDate descending
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

        protected void Grid_SalesOrder_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            //string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
           // List<int> branchidlist;
            string BranchList = "";
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    BranchList = Convert.ToString(Session["userbranchHierarchy"]);

                }
                else
                {
                    BranchList = strBranchID;

                }



                DataTable dtPartyTotal = new DataTable();
                if (strBranchID == "0")
                {
                    dtPartyTotal = oDBEngine.GetDataTable("select ISNULL(SUM(BasicAmount),0) BasicAmount,ISNULL(sum(Amount),0) Amount from v_ProjectOrderEntityList where cast(Order_CheckDate as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and Order_CompanyID='" + lastCompany + "' and Order_FinYear='" + FinYear + "' and LastEntryValue=1");
                }
                else
                {
                    dtPartyTotal = oDBEngine.GetDataTable("select ISNULL(SUM(BasicAmount),0) BasicAmount,ISNULL(sum(Amount),0) Amount from v_ProjectOrderEntityList where cast(Order_CheckDate as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and  Order_BranchId in('" + Convert.ToInt32(BranchList) + "') and CreatedBy = '" + Convert.ToInt32(User_id) + "' and Order_CompanyID='" + lastCompany + "' and Order_FinYear='" + FinYear + "' and LastEntryValue=1");
                }
                TotalBasicAmount = Convert.ToDecimal(dtPartyTotal.Rows[0]["BasicAmount"].ToString());
                TotalAmount = Convert.ToDecimal(dtPartyTotal.Rows[0]["Amount"].ToString());


                string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
                if (e.IsTotalSummary == true)
                {
                    switch (summaryTAG)
                    {
                        case "TotalBasicAmount":
                            e.Text = Convert.ToString(TotalBasicAmount);
                            break;
                        case "TotalValueInBaseCurrency":
                            e.Text = Convert.ToString(TotalAmount);
                            break;
                    }
                }
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
        public static bool SalesOrderApproval(string keyValue)
        {
            Boolean Success = false;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ApprovalCheckforMultiUserApproval");
            proc.AddVarcharPara("@Action", 500, "Project Sales Order");
            proc.AddVarcharPara("@User_Id", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ModuleId", 50, keyValue);
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                Success = true;
            }

            return Success;
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
    }
}