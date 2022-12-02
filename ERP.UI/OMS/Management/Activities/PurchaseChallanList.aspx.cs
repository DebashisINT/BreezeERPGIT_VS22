using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ERP.Models;
using System.Threading.Tasks;
 
namespace ERP.OMS.Management.Activities
{
    public partial class PurchaseChallanList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PurchaseChallanBL objPurchaseChallanBLBL = new PurchaseChallanBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        int KeyValue = 0;
        CommonBL cbl = new CommonBL();

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

            //Rev Tanmoy
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_PurchaseChallan.Columns[14].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_PurchaseChallan.Columns[14].Visible = false;
                }
            }
            //End Rev Tanmoy

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseChallanList.aspx");
            if (!IsPostBack)
            {
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                Session["exportval"] = null;
                //PopulateApprovalPendingCountByUserLevel();

                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

                //FillGrid();
            }
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
            }
            #endregion Sandip Section For Approval Dtl Section End
            //#region Show Count Pending Approval  in Popup grid User Level Wise Start
            //Session["exportval"] = null;
    
            //#endregion Show Count Pending Approval in Popup grid User Level Wise End
        }
        protected void Page_PreInit(object sender, EventArgs e)
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
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "PurchaseChallan");
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
            //Grid_PurchaseChallan.Columns[4].Visible = false;
            string filename = "Purchase GRN";
            exporter.FileName = filename;
            exporter.FileName = "GRN";

            exporter.PageHeader.Left = "Purchase GRN";
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
        public DataTable GetPurchaseIndentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "PurchaseChallan");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void Grid_PurchaseChallan_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                if (!IsPcTransactionExist(POID))
                {
                    if (!IsPcStockTransactionExist(POID))
                    {
                        deletecnt = objPurchaseChallanBLBL.DeletePurchaseChallan(POID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt32(Session["userid"]));
                        Grid_PurchaseChallan.JSProperties["cpDelete"] = "Deleted successfully.";
                    }
                    else
                    {
                        Grid_PurchaseChallan.JSProperties["cpDelete"] = "Available stock will become negative. Cannot Delete.";
                    }
                }
                else
                {

                    Grid_PurchaseChallan.JSProperties["cpDelete"] = "Transaction exist. Cannot Delete.";
                }

                //if (deletecnt == 1)
                //{
                //    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                //    GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //}
                //else
                //{
                //    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                //}
            }
            else if (Command == "FilterGridByDate")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataTable dtdata = new DataTable();
                dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "DV");
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    Grid_PurchaseChallan.DataSource = dtdata;
                    Grid_PurchaseChallan.DataBind();
                }
                else
                {
                    Grid_PurchaseChallan.DataSource = null;
                    Grid_PurchaseChallan.DataBind();
                }
            }
        }
        protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateERPDocApprovalPendingListByUserLevel();
        }
        protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }
        protected void Grid_PurchaseChallan_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        // Mantis Issue 25402
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
            Task PopulateStockTrialDataTask = new Task(() => GetPurchaseChallandata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPurchaseChallandata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PurchaseChallan_List", con);
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
        // End of Mantis Issue 25402
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseChallan_Id";

          //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            // Mantis Issue 25402
            string UserID = Convert.ToString(HttpContext.Current.Session["userid"]);
            // End of Mantis Issue 25402

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                // Mantis Issue 25402
                //if (strBranchID == "0")
                //{
                //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //    var q = from d in dc.v_PurchaseChallanLists
                //            where d.Challan_Date >= Convert.ToDateTime(strFromDate) && d.Challan_Date <= Convert.ToDateTime(strToDate)
                //            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && Convert.ToString(d.Entity_Type) == "DV" && Convert.ToString(d.Doc_Type) != "SC"
                //            orderby d.Challan_Date descending
                //            select d;
                //    e.QueryableSource = q;
                //}
                //else
                //{
                //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //    var q = from d in dc.v_PurchaseChallanLists
                //            where
                //            d.Challan_Date >= Convert.ToDateTime(strFromDate) && d.Challan_Date <= Convert.ToDateTime(strToDate)
                //            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && Convert.ToString(d.Entity_Type) == "DV" && Convert.ToString(d.Doc_Type) !="SC"
                //            orderby d.Challan_Date descending
                //            select d;
                //    e.QueryableSource = q;
                //}

                string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.PurchaseChallanLists
                        where Convert.ToString(d.USERID) == UserID
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                // End of Mantis Issue 25402
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
               // Mantis Issue 25402
                //var q = from d in dc.v_PurchaseChallanLists
                //        where Convert.ToString(d.PurchaseChallan_Id) == "0"
                //        orderby d.Challan_Date descending
                //        select d;
                var q = from d in dc.PurchaseChallanLists
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ descending
                        select d;
                // End of Mantis Issue 25402
                e.QueryableSource = q;
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

          //  BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();


            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }

        [WebMethod]
        public static string UpdateEWayBill(string InvoiceID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
        {
            PurchaseChallanBL objPurchaseChallanBLBL = new PurchaseChallanBL();
            int EWayBill = 0;
            EWayBill = objPurchaseChallanBLBL.UpdateEWayBill(InvoiceID, UpdateEWayBill, EWayBillDate, EWayBillValue, TransporterGSTIN, TransporterName, TransportationMode, TransportationDistance,
                TransporterDocNo, TransporterDocDate, VehicleNo, VehicleType);
            return Convert.ToString(EWayBill);
        }

        [WebMethod]
        public static bool CheckInvoiceTagged(string ChallanId)
        {
            bool IstaggedExist = false;
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PurchaseChallanTagOrNot");
            proc.AddVarcharPara("@PCID", 200, ChallanId);

            dt = proc.GetTable();
            if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
            {
                IstaggedExist = true;
            }
            return IstaggedExist;
        }

        #region Check Data Is Exist
        private bool IsPcTransactionExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseChallanBLBL.CheckPCTraanaction(pcid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        private bool IsPcStockTransactionExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseChallanBLBL.CheckPCTranslation(pcid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        #endregion

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

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "PC");
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PC");
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

            dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PC");
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
            int branchid = 0;
            if (Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(Session["userbranchID"]);
            }
            i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(10, branchid, Convert.ToString(Session["userid"]), "PC");  // 7 for Purchase Order Module 
            //i = objERPDocPendingApproval.ConditionWiseShowStatusButton(10, branchid, Convert.ToString(Session["userid"]));  // 8 for Purchase Challan Module 
            if (i == 1)
            {
                spanStatus.Visible = true;
                divPendingWaiting.Visible = true;
            }
            else if (i == 2)
            {
                spanStatus.Visible = false;
                divPendingWaiting.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
                divPendingWaiting.Visible = false;
            }
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PC");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseChallan\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseChallan\DocDesign\Normal";
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
                    NoofCopy += 3 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }


        #endregion Sandip Section For Approval Dtl Section End
        
        //public void FillGrid()
        //{
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));
        //    string finyear = Convert.ToString(Session["LastFinYear"]);

        //    DataTable dtdata = new DataTable();

        //    if (HttpContext.Current.Session["LastCompany"] != null)
        //    {
        //        string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //        if (HttpContext.Current.Session["userbranchHierarchy"] != null)
        //        {
        //            string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //            dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"DV");
        //        }
        //    }


        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        Grid_PurchaseChallan.DataSource = dtdata;
        //        Grid_PurchaseChallan.DataBind();
        //    }
        //    else
        //    {
        //        Grid_PurchaseChallan.DataSource = null;
        //        Grid_PurchaseChallan.DataBind();
        //    }
        //}
        

        //public void PopulateApprovalPendingCountByUserLevel()
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
        //    dtdata = objPurchaseChallanBLBL.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PC");
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        //lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["pendinQuotation"]) + ")";
        //        lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
        //    }
        //    else
        //    {
        //        lblWaiting.Text = "";
        //    }
        //}

       
        //protected void Grid_PurchaseChallan_DataBinding(object sender, EventArgs e)
        //{
        //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //    string finyear = Convert.ToString(Session["LastFinYear"]);
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

        //    DataTable dtdata = new DataTable();
        //    dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "DV");
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        Grid_PurchaseChallan.DataSource = dtdata;
        //    }
        //    else
        //    {
        //        Grid_PurchaseChallan.DataSource = null;
        //    }
        //}

        [WebMethod]
        public static String CheckGRNFullTagOrNot(string ChallanId)
        {
            string IstaggedExist = "";
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GRNFullTagOrNot");
            proc.AddVarcharPara("@ChallanId", 200, ChallanId);

            dt = proc.GetTable();
            if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "")
            {
                IstaggedExist = Convert.ToString(dt.Rows[0]["isexist"]);
            }
            return IstaggedExist;
        }

        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                String GRNID = "";
                GRNID = e.Parameters.Split('~')[1];             

                string strAction = "GetCloseGRNProducts";

                DataTable dtDetails = GetComponentProductList(strAction, GRNID);

                grid_Products.DataSource = dtDetails;
                grid_Products.DataBind();

                if (dtDetails != null && dtDetails.Rows.Count > 0)
                {
                    for (int j = 0; j < dtDetails.Rows.Count; j++)
                    {                        
                        grid_Products.Selection.SelectRow(j);
                        //grid_Products.Enabled = false;
                        

                    }
                }

                //grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate + "~" + ProjId;
            }
            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }

        public DataTable GetComponentProductList(string Action, string GRDId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ChallanId", 20, GRDId);
            dt = proc.GetTable();
            return dt;
        }


        [WebMethod]
        public static String ClosePartialData(string ChallanId, string ChallanDetails_ID, string Product_IDS)
        {
            string IstaggedExist = "";
            DataTable dt = new DataTable();


            if (!IsClosePcStockTransactionExist(ChallanId,ChallanDetails_ID))
            {
                ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
                proc.AddVarcharPara("@Action", 100, "CloseGRNProductsByID");
                proc.AddVarcharPara("@ChallanId", 200, ChallanId);
                proc.AddVarcharPara("@ChallanDetailsIDs", 200, ChallanDetails_ID);
                proc.AddVarcharPara("@ChallanProductIDs", 200, Product_IDS);
                dt = proc.GetTable();
                IstaggedExist = "Delete";
            }
            else
            {
                IstaggedExist = "negative";
            }
            return IstaggedExist;
        }


        private static bool IsClosePcStockTransactionExist(string ChallanId, string ChallanDetails_ID)
        {
            bool IsExist = false;
            if (ChallanId != "" && Convert.ToString(ChallanId).Trim() != "")
            { 
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
                proc.AddVarcharPara("@Action", 100, "ISClose_PC_Translation");
                proc.AddVarcharPara("@ChallanId", 200, ChallanId);
                proc.AddVarcharPara("@ChallanDetailsIDs", 200, ChallanDetails_ID);
                dt = proc.GetTable();
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }

        [WebMethod]
        public static object EditEWayBill(string DocID)
        {
            List<getEwayBill> EWayBill = new List<getEwayBill>();
            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            DataTable dt = new DataTable();
            dt = objSalesInvoiceBL.EditEWayBill(DocID, "PurchaseChallan");
            if (dt != null && dt.Rows.Count > 0)
            {
                EWayBill = (from DataRow dr in dt.Rows
                            select new getEwayBill()
                            {
                                TransporterName = Convert.ToString(dr["cnt_firstName"]),
                                TransporterGSTIN = Convert.ToString(dr["CNT_GSTIN"]),
                                Transporter_Mode = Convert.ToString(dr["Transporter_Mode"]),
                                Transporter_Distance = Convert.ToString(dr["Transporter_Distance"]),
                                Transporter_DocNo = Convert.ToString(dr["Transporter_DocNo"]),
                                Transporter_DocDate = Convert.ToString(dr["Transporter_DocDate"]),
                                Vehicle_No = Convert.ToString(dr["Vehicle_No"]),
                                Vehicle_type = Convert.ToString(dr["Vehicle_type"])
                            }).ToList();
            }
            return EWayBill;
        }

        public class getEwayBill
        {
            public String TransporterName { get; set; }
            public String TransporterGSTIN { get; set; }
            public String Transporter_Mode { get; set; }
            public String Transporter_Distance { get; set; }
            public String Transporter_DocNo { get; set; }
            public String Transporter_DocDate { get; set; }
            public String Vehicle_No { get; set; }
            public String Vehicle_type { get; set; }
        }
    }
}