using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using EntityLayer.CommonELS;
using DevExpress.Web;
using BusinessLogicLayer;
using ERP.Models;
using System.Linq;
using System.Data.SqlClient;
using DevExpress.XtraPrinting;
using DevExpress.Export;

namespace ERP.OMS.Management.Activities
{
    public partial class ProjectPurchaseOrderList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        ProjectPurchaseOrderBL objPurchaseOrderBL = new ProjectPurchaseOrderBL();
        CommonBL cbl = new CommonBL();
        string Opening = string.Empty;
        decimal TotalBasicAmount = 0, TotalAmount = 0;
        public bool isApprove { get; set; }
        public string ShowProductWiseClose { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            String finanyear = "";
            finanyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finanyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            //Rev Tanmoy Project
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApproveProjPurchaseOrder = cbl.GetSystemSettingsResult("ApproveProjPurchaseOrder");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_PurchaseOrder.Columns[19].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_PurchaseOrder.Columns[19].Visible = false;
                }
            }
            //End Tanmoy Project

            string IsMultiuserApprovalRequired = cbl.GetSystemSettingsResult("IsMultiuserApprovalRequired");
            hdnIsMultiuserApprovalRequired.Value = IsMultiuserApprovalRequired;

            if (!String.IsNullOrEmpty(ApproveProjPurchaseOrder))
            {
                if (ApproveProjPurchaseOrder == "Yes")
                {
                    Grid_PurchaseOrder.Columns[27].Visible = true;
                    Grid_PurchaseOrder.Columns[9].Visible = true;
                    Grid_PurchaseOrder.Columns[10].Visible = true;
                    isApprove = true;
                }
                else if (ApproveProjPurchaseOrder.ToUpper().Trim() == "NO")
                {
                    Grid_PurchaseOrder.Columns[27].Visible = false;
                    Grid_PurchaseOrder.Columns[9].Visible = false;
                    Grid_PurchaseOrder.Columns[10].Visible = false;
                    isApprove = false;
                }
            }
            string CloseQuantityRequire = cbl.GetSystemSettingsResult("CloseQuantityRequire");
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

            if (!IsPostBack)
            {
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End

                string ApprovalUserProjectPurchaseOrder = cbl.GetSystemSettingsResult("ApprovalUserProjectPurchaseOrder");

                if (!String.IsNullOrEmpty(ApprovalUserProjectPurchaseOrder))
                {
                    if (ApprovalUserProjectPurchaseOrder == "No" && ApproveProjPurchaseOrder == "No")
                    {
                       // ConditionWiseShowStatusButton();
                        divPendingWaiting.Visible = false;
                        spanStatus.Visible = false;
                    }
                    else
                    {
                        divPendingWaiting.Visible = false;
                        spanStatus.Visible = false;
                    }
                }
                #endregion Sandip Section For Approval Dtl Section End
                //................Cookies..................
                //Grid_PurchaseOrder.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_PurchaseOrderPagePurchaseOrder";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_PurchaseOrderPagePurchaseOrder');</script>");

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                //...........Cookies End............... 
                Session["SaveModePO"] = null;
                Session["exportval"] = null;
                if (Request.QueryString["op"] == "yes")
                {
                    Opening = "yes";
                    hdfOpening.Value = "Opening";
                }
                else
                {
                    Opening = "NO";

                }

                #region Barcode Section

                if (IsBarcodeGeneratete() == true)
                {
                    btnGenerate.Visible = true;
                    btnPrint.Visible = true;
                    hfIsBarcode.Value = "true";
                    hdfIsBarcodeActive.Value = "Y";
                    hdfIsBarcodeGenerator.Value = "Y";
                }
                else
                {
                    btnGenerate.Visible = false;
                    btnPrint.Visible = false;
                    hfIsBarcode.Value = "false";
                    hdfIsBarcodeActive.Value = "N";
                    hdfIsBarcodeGenerator.Value = "N";
                }

                //btnGenerate.Visible = false;
                //btnPrint.Visible = false;
                //hfIsBarcode.Value = "false";
                //hdfIsBarcodeActive.Value = "N";

                //if (IsBarcodeGeneratete() == true) hdfIsBarcodeGenerator.Value = "Y";
                //else hdfIsBarcodeGenerator.Value = "N";

                #endregion
            }
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
            }
            #endregion Sandip Section For Approval Dtl Section End
            //  FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectPurchaseOrderList.aspx");
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
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

                    dtdata = objERPDocPendingApproval.ProjectPopulateERPDocApprovalPendingListByUserLevel(userid, "PO");
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
            dtdata = objERPDocPendingApproval.ProjectPopulateERPDocApprovalPendingCountByUserLevel(userid, "PO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }
            else
            {
                lblWaiting.Text = "";
            }
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
            //KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            //Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        }


        protected void chkreject_Init(object sender, EventArgs e) // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            //KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            //Session["KeyValue"] = KeyValue;
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

            dtdata = objERPDocPendingApproval.ProjectPopulateUserWiseERPDocCreation(userid, "PO");
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
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(7, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PPO");

            if (j == 1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(7, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PPO");

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
        #region After Approval Or rejected Number to reflect of Pending Approval Section  Start

        [WebMethod]
        public static string GetPendingCase()
        {
            string strPending = "(0)";

            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);

            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.ProjectPopulateERPDocApprovalPendingCountByUserLevel(userid, "PO");
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
        protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }

        #endregion Sandip Section For Approval Dtl Section End

        #region Other Region

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            DataTable branchtable = getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        public DataTable getBranchListByHierchy(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }
        private bool IsPITransactionExist(string Poid)
        {
            bool IsExist = false;
            if (Poid != "" && Convert.ToString(Poid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseOrderBL.CheckPOTraanaction(Poid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }


        [WebMethod]
        public static string CancelPurchaseOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.CancelProjectPurchaseOrder(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        [WebMethod]
        public static string ClosedPurchaseOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.ClosedProjectPurchaseOrder(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string available = Convert.ToString(e.GetValue("IsCancel"));
            string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            string PurchaseOrder_Id = Convert.ToString(e.GetValue("PurchaseOrder_Id"));
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

            string PurchaseOrderInEntryModule = cbl.GetSystemSettingsResult("ApproveProjPurchaseOrder");
            if (!String.IsNullOrEmpty(PurchaseOrderInEntryModule))
            {
                if (PurchaseOrderInEntryModule == "Yes")
                {
                    string ApprovalUserPurchaseOrder = cbl.GetSystemSettingsResult("ApprovalUserProjectPurchaseOrder");
                    if (!String.IsNullOrEmpty(ApprovalUserPurchaseOrder))
                    {
                        if (ApprovalUserPurchaseOrder == "Yes")
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
                            dtdata = objPurchaseOrderBL.PopulatePODocApprovalByUserLevel(userid, "PPO");
                            if (dtdata != null && dtdata.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtdata.Rows)
                                {

                                    string PurchaseOrderId = Convert.ToString(dr["id"]);
                                    if (PurchaseOrder_Id == PurchaseOrderId)
                                    {
                                        isApprove = true;
                                        break;
                                    }
                                    else
                                    {
                                        isApprove = false;
                                    }
                                }
                            }
                            else
                            {
                                isApprove = false;
                            }

                        }

                    }
                }
            }


        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseOrder_Id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;
            bool i = objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["UserID"]));
            if (i)
            {
                if (IsFilter == "Y")
                {
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectPurchaseOrderLists
                                //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                                where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                                 && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)

                                && d.OrderAdd_addressType == "Shipping"
                                && d.CreatedBy == userid
                                orderby d.PurchaseOrder_Id descending
                                select d;

                        e.QueryableSource = q;
                        // var cnt = q.Count();
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectPurchaseOrderLists
                                where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                                && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                                && d.OrderAdd_addressType == "Shipping"
                                && d.CreatedBy == userid
                                orderby d.PurchaseOrder_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ProjectPurchaseOrderLists
                            // where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                            where d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                            && d.PurchaseOrder_BranchId == 0
                            && d.OrderAdd_addressType == "Shipping"
                            && d.CreatedBy == userid
                            orderby d.PurchaseOrder_Id descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                if (IsFilter == "Y")
                {
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectPurchaseOrderLists
                                //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                                where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                                && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                                && d.OrderAdd_addressType == "Shipping"
                                orderby d.PurchaseOrder_Id descending
                                select d;

                        e.QueryableSource = q;
                        // var cnt = q.Count();
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectPurchaseOrderLists
                                where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                                && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                                && d.OrderAdd_addressType == "Shipping"
                                orderby d.PurchaseOrder_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ProjectPurchaseOrderLists
                            //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                            where d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                            && d.PurchaseOrder_BranchId == 0
                            && d.OrderAdd_addressType == "Shipping"
                            orderby d.PurchaseOrder_Id descending
                            select d;
                    e.QueryableSource = q;
                }
            }
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes";
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

        #endregion

        #region Export Grid Section
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
           // Grid_PurchaseOrder.Columns[7].Visible = false;
            exporter.GridViewID = "Grid_PurchaseOrder";
            exporter.FileName = "PurchaseOrder";

            exporter.PageHeader.Left = "Project Purchase Order";
           // exporter.PageFooter.Center = "[Page # of Pages #]";
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

        #endregion

        #region Grid & Barcode Region

        protected void Grid_PurchaseOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string POID = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    POID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (Command == "Delete")
            {
                if (!IsPITransactionExist(POID))
                {
                    deletecnt = objPurchaseOrderBL.DeletePurchaseOrder(POID);

                    if (deletecnt == 1)
                    {
                        Grid_PurchaseOrder.JSProperties["cpDelete"] = "Deleted successfully";
                    }
                }
                else
                {
                    Grid_PurchaseOrder.JSProperties["cpDelete"] = "This Purchase Order is tagged in other modules. Cannot Delete.";
                }

            }
            else if (Command == "FilterGridByDate")
            {
                //DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                //DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                //string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //string finyear = Convert.ToString(Session["LastFinYear"]);

                //DataTable dtdata = new DataTable();
                ////dtdata = GetPOListGridDataByFilter(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                //if (dtdata != null && dtdata.Rows.Count > 0)
                //{
                //    Grid_PurchaseOrder.DataSource = dtdata;
                //    Grid_PurchaseOrder.DataBind();
                //}
                //else
                //{
                //    Grid_PurchaseOrder.DataSource = null;
                //    Grid_PurchaseOrder.DataBind();
                //}
            }
            else if (Command == "GenerateBarcode")
            {
                string strPOID = Convert.ToString(e.Parameters.Split('~')[1]);
                int strIsComplete = 0;
                GenerateBarcode(strPOID, ref strIsComplete);

                if (strIsComplete == 1)
                {
                    Grid_PurchaseOrder.JSProperties["cpMessage"] = "Generated";
                }
                else if (strIsComplete == -10)
                {
                    Grid_PurchaseOrder.JSProperties["cpMessage"] = "Error";
                }
                else if (strIsComplete == -20)
                {
                    Grid_PurchaseOrder.JSProperties["cpMessage"] = "NullStock";
                }
                else if (strIsComplete == -30)
                {
                    Grid_PurchaseOrder.JSProperties["cpMessage"] = "NullBarcode";
                }
                else
                {
                    Grid_PurchaseOrder.JSProperties["cpMessage"] = "Error";
                }
            }
        }
        protected void Grid_PurchaseOrder_DataBinding(object sender, EventArgs e)
        {
            //string BranchID = Convert.ToString(cmbBranchfilter.Value);
            //DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            //DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //string finyear = Convert.ToString(Session["LastFinYear"]);

            //DataTable dtdata = new DataTable();
            //dtdata = GetPOListGridDataByFilter(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    Grid_PurchaseOrder.DataSource = dtdata;

            //}
            //else
            //{
            //    Grid_PurchaseOrder.DataSource = null;

            //}
        }
        protected void Grid_PurchaseOrder_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //e.Text = string.Format("{0}", e.Value);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
             string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);
            bool i = objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["UserID"]));
            List<int> branchidlist;
            string BranchList="";
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
                if (i)
                {
                    if (strBranchID == "0")
                    {
                        dtPartyTotal = oDBEngine.GetDataTable("select isnull(SUM(BasicAmount),0) BasicAmount,isnull(sum(ValueInBaseCurrency),0) Amount from v_ProjectPurchaseOrderList where cast(PurchaseOrderDt as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and OrderAdd_addressType ='Shipping'  and CreatedBy = '" + Convert.ToInt32(userid) + "' and LastEntryValue=1");
                    }
                    else
                    {
                        dtPartyTotal = oDBEngine.GetDataTable("select isnull(SUM(BasicAmount),0) BasicAmount,isnull(sum(ValueInBaseCurrency),0) Amount from v_ProjectPurchaseOrderList where cast(PurchaseOrderDt as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and OrderAdd_addressType ='Shipping' and PurchaseOrder_BranchId in('" + Convert.ToInt32(BranchList) + "') and CreatedBy = '" + Convert.ToInt32(userid) + "' and LastEntryValue=1");
                    }
                }
                else
                {
                    if (strBranchID == "0")
                    {
                        dtPartyTotal = oDBEngine.GetDataTable("select isnull(SUM(BasicAmount),0) BasicAmount,isnull(sum(ValueInBaseCurrency),0) Amount from v_ProjectPurchaseOrderList where cast(PurchaseOrderDt as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and OrderAdd_addressType ='Shipping'   and LastEntryValue=1");
                    }
                    else
                    {
                        dtPartyTotal = oDBEngine.GetDataTable("select isnull(SUM(BasicAmount),0) BasicAmount,isnull(sum(ValueInBaseCurrency),0) Amount from v_ProjectPurchaseOrderList where cast(PurchaseOrderDt as date) BETWEEN'" + Convert.ToDateTime(strFromDate) + "' and '" + Convert.ToDateTime(strToDate) + "' and OrderAdd_addressType ='Shipping' and PurchaseOrder_BranchId in('" + Convert.ToInt32(BranchList) + "')  and LastEntryValue=1");
                    }
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
        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


                DataTable DT_TC = objEngine.GetDataTable("tbl_Master_SystemControl", " BarcodeGeneration ", null);
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    IsGeneratete = Convert.ToBoolean(DT_TC.Rows[0]["BarcodeGeneration"]);
                }

                return IsGeneratete;
            }
            catch
            {
                return IsGeneratete;
            }
        }

        public bool GenerateBarcode(string strPOID, ref int strIsComplete)
        {
            try
            {
                DataSet dsInst = new DataSet();


                // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("proc_GenerateBarcode_DocumentWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IN_DocNumber", strPOID);
                cmd.Parameters.AddWithValue("@Doc_Type", "PO");
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                cmd.Dispose();
                con.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Barcode Section

        protected void DocumentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = Convert.ToString(e.Parameter.Split('~')[0]);

            if (strSplitCommand == "GetDocumentDetails")
            {
                string PO_Number = Convert.ToString(e.Parameter.Split('~')[1]);

                DataSet ds_Details = GetDetails(PO_Number);
                DataTable Details_dt = ds_Details.Tables[0];
                DataTable Transaction_dt = ds_Details.Tables[1];

                if (Details_dt != null && Details_dt.Rows.Count > 0)
                {
                    string DocNumber = Convert.ToString(Details_dt.Rows[0]["DocNumber"]);
                    string Vendor = Convert.ToString(Details_dt.Rows[0]["Vendor"]);
                    string BranchID = Convert.ToString(Details_dt.Rows[0]["BranchID"]);
                    string Branch = Convert.ToString(Details_dt.Rows[0]["Branch"]);

                    DocumentPanel.JSProperties["cpDocDetails"] = DocNumber + "~" + Vendor + "~" + Branch + "~" + BranchID;
                }

                DocumentGrid.DataSource = Transaction_dt;
                DocumentGrid.DataBind();

                StockGrid.DataSource = null;
                StockGrid.DataBind();
            }
            else if (strSplitCommand == "GetDocumentStock")
            {
                string PO_Number = Convert.ToString(hdfDocNumber.Value);
                string PO_DetailsNumber = Convert.ToString(hdfDocDetailsNumber.Value);

                DataSet ds_Details = GetStock(PO_Number, PO_DetailsNumber);
                DataTable Details_dt = ds_Details.Tables[0];
                DataTable Transaction_dt = ds_Details.Tables[1];

                if (Details_dt != null && Details_dt.Rows.Count > 0)
                {
                    string Products_Name = Convert.ToString(Details_dt.Rows[0]["Products_Name"]);
                    string Quantity = Convert.ToString(Details_dt.Rows[0]["Quantity"]);

                    DocumentPanel.JSProperties["cpStockDetails"] = Products_Name + "~" + Quantity;

                    ctxtQty.MaxValue = Convert.ToDecimal(Quantity);
                }

                StockGrid.DataSource = Transaction_dt;
                StockGrid.DataBind();
            }
            else if (strSplitCommand == "GenerateBarcode")
            {
                string Branch = Convert.ToString(hdfBranch.Value);
                string PO_Number = Convert.ToString(hdfDocNumber.Value);
                string PO_DetailsNumber = Convert.ToString(hdfDocDetailsNumber.Value);
                string Quantity = Convert.ToInt32(e.Parameter.Split('~')[1]).ToString();

                DataSet ds_Details = ProductBarcodeGenerate(PO_Number, PO_DetailsNumber, Branch, Quantity);
                DataTable Transaction_dt = ds_Details.Tables[0];

                StockGrid.DataSource = Transaction_dt;
                StockGrid.DataBind();
            }
            else if (strSplitCommand == "SaveSerial")
            {
                string PO_Number = Convert.ToString(hdfDocNumber.Value);
                string PO_DetailsNumber = Convert.ToString(hdfDocDetailsNumber.Value);
                string MapID = Convert.ToString(hdfMapID.Value);
                string SerialNo = Convert.ToString(cSerialNo.Value);

                hdfMapID.Value = "";
                cSerialNo.Value = "";

                DataSet ds_Details = SaveSerial(PO_Number, PO_DetailsNumber, MapID, SerialNo);
                DataTable Transaction_dt = ds_Details.Tables[0];

                StockGrid.DataSource = Transaction_dt;
                StockGrid.DataBind();
            }
        }
        public DataSet GetDetails(string PO_Number)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "GetDocumentDetails");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetStock(string PO_Number, string PO_DetailsNumber)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "GetDocumentStock");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            proc.AddVarcharPara("@PO_DetailsNumber", 500, PO_DetailsNumber);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet SaveSerial(string PO_Number, string PO_DetailsNumber, string MapID, string SerialNo)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "SaveSerial");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            proc.AddVarcharPara("@PO_DetailsNumber", 500, PO_DetailsNumber);
            proc.AddVarcharPara("@MapID", 500, MapID);
            proc.AddVarcharPara("@SerialNo", 500, SerialNo);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet ProductBarcodeGenerate(string PO_Number, string PO_DetailsNumber, string Branch, string Quantity)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "BarcodeGenerate");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            proc.AddVarcharPara("@PO_DetailsNumber", 500, PO_DetailsNumber);
            proc.AddVarcharPara("@Branch", 500, Branch);
            proc.AddVarcharPara("@Quantity", 500, Quantity);
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CreatedBy", 500, Convert.ToString(Session["userid"]));
            ds = proc.GetDataSet();
            return ds;
        }

        #endregion

        [WebMethod]
        public static bool PurchaseOrderApproval(string keyValue)
        {
            Boolean Success = false;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ApprovalCheckforMultiUserApproval");
            proc.AddVarcharPara("@Action", 500, "Project Purchase Order");
            proc.AddVarcharPara("@User_Id", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ModuleId", 50, keyValue);
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count>0)
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 50, "ClosePurchaseOrderProductsWise");
            proc.AddVarcharPara("@PurchaseOrder_Id", 20, strOrderID);
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 50, "PopulatePurchaseOrderProducts");
            proc.AddVarcharPara("@PurchaseOrder_Id", 20, strOrderID);
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