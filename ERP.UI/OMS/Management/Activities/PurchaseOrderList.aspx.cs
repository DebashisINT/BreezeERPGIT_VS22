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
using System.Net;
using System.Threading.Tasks;
namespace ERP.OMS.Management.Activities
{
    public partial class PurchaseOrderList : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        CommonBL cbl = new CommonBL();
        string Opening = string.Empty;
        public bool IsApprove { get; set; }
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
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_PurchaseOrder.Columns[15].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_PurchaseOrder.Columns[15].Visible = false;
                }
            }
            //End Tanmoy Project

            //Rev Tanmoy Reveision No Date
            string PurchaseOrderInEntryModule = cbl.GetSystemSettingsResult("RevisionNoDateinPurchasedOrder");
            if (!String.IsNullOrEmpty(PurchaseOrderInEntryModule))
            {
                if (PurchaseOrderInEntryModule == "Yes")
                {
                    
                    IsApprove = true;
                    Grid_PurchaseOrder.Columns[8].Visible = true;
                    Grid_PurchaseOrder.Columns[9].Visible = true;
                    Grid_PurchaseOrder.Columns[17].Visible = true;
                }
                else if (PurchaseOrderInEntryModule.ToUpper().Trim() == "NO")
                {
                    IsApprove = false;
                    Grid_PurchaseOrder.Columns[8].Visible = false;
                    Grid_PurchaseOrder.Columns[9].Visible = false;
                    Grid_PurchaseOrder.Columns[17].Visible = false;
                }
            }
            //Rev Tanmoy Reveision No Date

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
                string ApprovalUserPurchaseOrder = cbl.GetSystemSettingsResult("ApprovalUserPurchaseOrder");

                if (!String.IsNullOrEmpty(ApprovalUserPurchaseOrder))
                {
                    //if (ApprovalUserPurchaseOrder == "No" && PurchaseOrderInEntryModule == "No")
                    if (PurchaseOrderInEntryModule == "No")
                    {
                        ConditionWiseShowStatusButton();
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
                Session["InvoicePopUpPanel"] = null;

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseOrderList.aspx");
            //Mantis Issue 25152
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string DBName = con.Database;
            GetConnectionString(DBName);
            //End of Mantis Issue 25152
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

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "PO");
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PO");
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

            dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PO");
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
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(7, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PO");

            if (j == 1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(7, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PO");

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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PO");
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
            CancelOrder = objCRMSalesOrderDtlBL.CancelPurchaseOrder(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        [WebMethod]
        public static string ClosedPurchaseOrderOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelOrder = 0;
            CancelOrder = objCRMSalesOrderDtlBL.ClosedPurchaseOrder(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        [WebMethod]
        public static string GetType(string keyValue)
        {
            string Type = "";
            DataTable Details_dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetType");
            proc.AddVarcharPara("@PurchaseOrder_Id", 500, keyValue);
            Details_dt = proc.GetTable();

            if (Details_dt != null && Details_dt.Rows.Count > 0)
            {
                Type = Convert.ToString(Details_dt.Rows[0]["IsInventory"]);

            }


            return Convert.ToString(Type);

        }

        [WebMethod]
        public static string GetStockSerial(string keyValue)
        {
            string IsSerialActive = "", IsBarcodeActive = "";
            //DataTable Details_dt = GetIsSerial(keyValue);

            DataTable Details_dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "GetIsSerial");
            proc.AddVarcharPara("@PO_Number", 500, keyValue);
            Details_dt = proc.GetTable();

            if (Details_dt != null && Details_dt.Rows.Count > 0)
            {
                IsSerialActive = Convert.ToString(Details_dt.Rows[0]["IsSerialActive"]);
                IsBarcodeActive = Convert.ToString(Details_dt.Rows[0]["IsBarcodeActive"]);
            }


            return Convert.ToString(IsSerialActive + '~' + IsBarcodeActive);

        }

        public DataTable GetIsSerial(string PO_Number)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "GetIsSerial");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            ds = proc.GetTable();
            return ds;
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



            string PurchaseOrderInEntryModule = cbl.GetSystemSettingsResult("RevisionNoDateinPurchasedOrder");
            if (!String.IsNullOrEmpty(PurchaseOrderInEntryModule))
            {
                if (PurchaseOrderInEntryModule == "Yes")
                {
                    string ApprovalUserPurchaseOrder = cbl.GetSystemSettingsResult("ApprovalUserPurchaseOrder");
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
                            dtdata = objPurchaseOrderBL.PopulatePODocApprovalByUserLevel(userid, "PO");
                            if (dtdata != null && dtdata.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtdata.Rows)
                                {
                                    
                                    string PurchaseOrderId = Convert.ToString(dr["id"]);
                                    if (PurchaseOrder_Id == PurchaseOrderId)
                                    {
                                        IsApprove = true;
                                        break;
                                    }
                                    else
                                    {
                                        IsApprove = false;
                                    }
                                }
                            }
                            else
                            {
                                IsApprove = false;
                            }
                            }

                        }
                    }
                }
            }

        // Mantis Issue 25394
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
            Task PopulateStockTrialDataTask = new Task(() => GetPurchaseOrderdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPurchaseOrderdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PurchaseOrder_List", con);
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
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
                //cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.Parameters.AddWithValue("@ACTION", "ALL");
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
        // End of Mantis Issue 25394
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
                    // Mantis Issue 25394
                    //if (strBranchID == "0")
                    //{
                    //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_PurchaseOrderLists
                    //            //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                    //            where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                    //             && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)

                    //            && d.OrderAdd_addressType == "Shipping"
                    //            && d.CreatedBy == userid
                    //            orderby d.PurchaseOrder_Id descending
                    //            select d;

                    //    e.QueryableSource = q;
                    //    // var cnt = q.Count();
                    //}
                    //else
                    //{
                    //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_PurchaseOrderLists
                    //            where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                    //            && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                    //            && d.OrderAdd_addressType == "Shipping"
                    //            && d.CreatedBy == userid
                    //            orderby d.PurchaseOrder_Id descending
                    //            select d;
                    //    e.QueryableSource = q;
                    //}

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.PurchaseOrderLists
                                //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                                where Convert.ToInt32(d.USERID) == userid && d.CreatedBy == Convert.ToInt32(userid)
                               orderby d.SEQ
                               select d;

                        e.QueryableSource = q;


                }
                else
                {
                   // Mantis Issue 25394
                    //branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_PurchaseOrderLists
                    //        // where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                    //        where d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                    //        && d.PurchaseOrder_BranchId == 0
                    //        && d.OrderAdd_addressType == "Shipping"
                    //        && d.CreatedBy == userid
                    //        orderby d.PurchaseOrder_Id descending
                    //        select d;
                    //e.QueryableSource = q;
                   
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.PurchaseOrderLists
                            where Convert.ToString(d.SEQ) == "0"
                            orderby d.SEQ
                           select d;
                    e.QueryableSource = q;
                    // End of Mantis Issue 25394
                }
            }
            else
            {
                if (IsFilter == "Y")
                {
                    // Mantis Issue 25394
                    //if (strBranchID == "0")
                    //{
                    //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_PurchaseOrderLists
                    //            //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                    //            where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                    //            && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                    //            && d.OrderAdd_addressType == "Shipping"
                    //            orderby d.PurchaseOrder_Id descending
                    //            select d;

                    //    e.QueryableSource = q;
                    //    // var cnt = q.Count();
                    //}
                    //else
                    //{
                    //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_PurchaseOrderLists
                    //            where branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                    //            && d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                    //            && d.OrderAdd_addressType == "Shipping"
                    //            orderby d.PurchaseOrder_Id descending
                    //            select d;
                    //    e.QueryableSource = q;
                    //}

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                     var q = from d in dc.PurchaseOrderLists
                             where Convert.ToInt32(d.USERID) == userid 
                             orderby d.SEQ
                             select d;

                        e.QueryableSource = q;
                    // Mantis Issue 25394
                }
                else
                {
                   // Mantis Issue 25394
                    //branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_PurchaseOrderLists
                    //        //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                    //        where d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                    //        && d.PurchaseOrder_BranchId == 0
                    //        && d.OrderAdd_addressType == "Shipping"
                    //        orderby d.PurchaseOrder_Id descending
                    //        select d;
                    //e.QueryableSource = q;

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.PurchaseOrderLists
                            where Convert.ToString(d.SEQ) == "0"
                            orderby d.SEQ
                            select d;
                    e.QueryableSource = q;
                    // End of Mantis Issue 25394
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
            Grid_PurchaseOrder.Columns[7].Visible = false;
            exporter.GridViewID = "Grid_PurchaseOrder";
            exporter.FileName = "PurchaseOrder";

            exporter.PageHeader.Left = "Purchase Order";
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
            e.Text = string.Format("{0}", e.Value);
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
            else if (strSplitCommand == "UpdateSerial")
            {
                string PO_Number = Convert.ToString(hdfDocNumber.Value);
                string PO_DetailsNumber = Convert.ToString(hdfDocDetailsNumber.Value);
                string MapID = Convert.ToString(hdfMapID.Value);
                string SerialNo = Convert.ToString(cSerialNo.Value);
                hdfMapID.Value = "";
                cSerialNo.Value = "";
                if (SerialNo != "")
                {
                    DataSet ds_Details = UpdateSerial(PO_Number, PO_DetailsNumber, MapID, SerialNo);
                    DataTable Transaction_dt = ds_Details.Tables[0];
                    StockGrid.DataSource = Transaction_dt;
                    StockGrid.DataBind();
                }


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
        public DataSet UpdateSerial(string PO_Number, string PO_DetailsNumber, string MapID, string SerialNo)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_BarcodeGeneration_FromOrder");
            proc.AddVarcharPara("@Action", 500, "UpdateSerial");
            proc.AddVarcharPara("@PO_Number", 500, PO_Number);
            proc.AddVarcharPara("@PO_DetailsNumber", 500, PO_DetailsNumber);
            proc.AddVarcharPara("@MapID", 500, MapID);
            proc.AddVarcharPara("@SerialNo", 500, SerialNo);
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

        protected void InvoiceDatepanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {
                Session["InvoicePopUpPanel"] = null;
                DataTable InvoiceDatedt = PopulateSalesInvoiceNumberByOrderId(Convert.ToString(data[1]));
                Session["InvoicePopUpPanel"] = InvoiceDatedt;
                grdInvoiceDate.DataSource = InvoiceDatedt;
                grdInvoiceDate.DataBind();

            }
        }
        public DataTable PopulateSalesInvoiceNumberByOrderId(string strOrderID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 50, "PopulatePurchaseInvoiceNumberByOrderId");
            proc.AddVarcharPara("@Product_Id", 20, strOrderID);
            dt = proc.GetTable();
            return dt;
        }
        protected void InvoiceDatepanel_DataBinding(object sender, EventArgs e)
        {
            if (Session["InvoicePopUpPanel"] != null)
            {
                grdInvoiceDate.DataSource = (DataTable)Session["InvoicePopUpPanel"];
            }
        }
        protected void InvoiceNumberpanel_DataBinding(object sender, EventArgs e)
        {
            if (Session["InvoicePopUpPanel"] != null)
            {
                grdInvoiceNumber.DataSource = (DataTable)Session["InvoicePopUpPanel"];
            }
        }
        protected void InvoiceNumberpanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {
                Session["InvoicePopUpPanel"] = null;
                DataTable InvoiceNumberdt = PopulateSalesInvoiceNumberByOrderId(Convert.ToString(data[1]));
                Session["InvoicePopUpPanel"] = InvoiceNumberdt;
                grdInvoiceNumber.DataSource = InvoiceNumberdt;
                grdInvoiceNumber.DataBind();

            }
        }

        [WebMethod]
        public static bool PurchaseOrderApproval(string keyValue)
        {
            Boolean Success = false;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ApprovalCheckforMultiUserApproval");
            proc.AddVarcharPara("@Action", 500, "Purchase Order");
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

                if(Productdt.Rows.Count==0)
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
        protected void ChkProductID_Init(object sender, EventArgs e)
        {

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
        //Mantis Issue 25152
        [WebMethod]
        public static string SendSMSManualNo(String POrderId, String EmployeeId)
        {
            //string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
            //if (SMSRequiredInDirectorApproval == "Yes")
            //{
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            string DataBase = con.Database;

            string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
            //string baseUrl = "https://3.7.30.86:85";
            string OrderId = POrderId;
            string LongURL = baseUrl + "/ServiceManagement/Transaction/OrderMView/OrderApproval.aspx?id=" + OrderId + "&UniqueKey=" + Convert.ToString(DataBase);

            string tinyURL = ShortURL(LongURL);
            string EmpId = EmployeeId;
            //ProcedureExecute proc1 = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            //proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
            //proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
            //proc1.AddPara("@EmpId", Convert.ToString(EmpId));
            //proc1.GetTable();
            //}
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
                    proc.AddVarcharPara("@Action", 500, "ApprovalSendSMS");
                    proc.AddPara("@tinyURL", Convert.ToString(tinyURL));
                    proc.AddPara("@EmpId", Convert.ToString(EmpId));
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                    }
                    output = "true";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        private static string ShortURL(string LongUrl)
        {
            try
            {
                if (LongUrl.Length <= 30)
                {
                    return LongUrl;
                }
                if (!LongUrl.ToLower().StartsWith("http") && !LongUrl.ToLower().StartsWith("ftp"))
                {
                    LongUrl = "http://" + LongUrl;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + LongUrl);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return LongUrl;
            }
        }

        [WebMethod]
        public static object AddModalEmployee(string DBName)
        {
            DataTable branchtable = new DataTable();
            //DataTable ds = new DataTable();
            //string oSql = hdDbName.value;
            string oSql = Convert.ToString(DBName);
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("prc_PurchaseOrderDetailsList", oSqlConnection);
            cmd.Parameters.AddWithValue("@ACTION", "FetchEmployee");
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(branchtable);
            oSqlConnection.Close();

            List<ddlDirEmployee> All = new List<ddlDirEmployee>();
            All = (from DataRow dr in branchtable.Rows
                   select new ddlDirEmployee()
                   {
                       cnt_internalId = dr["cnt_internalId"].ToString(),
                       DirectorName = dr["DirectorName"].ToString()
                   }).ToList();

            return All;
        }
        public class ddlDirEmployee
        {
            public string cnt_internalId { get; set; }
            public string DirectorName { get; set; }
        }
        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;
            }
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);
            string str = connectionString.ConnectionString;
            hdDbName.Value = str;
            return str;
        }
        //End of Mantis Issue 25152
    }
}