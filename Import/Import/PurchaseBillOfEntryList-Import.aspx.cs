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
using EntityLayer.CommonELS;
using DevExpress.Web;
using BusinessLogicLayer;
using ImportModuleBusinessLayer.Purchaseorder;
using Import.Models;
using ImportModuleBusinessLayer.PurchaseorderAcceptance;
using ImportModuleBusinessLayer.BillOfEntry;


namespace Import.Import
{
    public partial class PurchaseBillOfEntryList_Import : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        ImportPurchaseOrderacceptanceBL objPurchaseOrderBL = new ImportPurchaseOrderacceptanceBL();

        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Section Start

        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        string Opening = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/PurchaseBillOfEntryList-Import.aspx");
            if (!IsPostBack)
            {
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                //................Cookies..................
                Grid_PurchaseOrder.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_BillOfEntryPageBillOfEntry";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_BillOfEntryPageBillOfEntry');</script>");
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


                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);
                PopulateBranchByHierchy(userbranch);
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

            }
            #region Sandip Section For Approval Section Start
            if (spanStatus.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
            }
            #endregion Sandip Section For Approval Dtl Section End
            /// FillGrid();

          

        }

        public void Date_finyearwise(string Finyear)
        {

            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                FormDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                toDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }


        public DataTable GetDateFinancila(string Finyear)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Getdatefromfinyear");
            proc.AddPara("@Finyear", Finyear);
            ds = proc.GetTable();
            return ds;
        }

        #region Export Grid Section Start
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
            //    Grid_PurchaseOrder.Columns[7].Visible = false;
            //string filename = "Purchase Order";
            //exporter.FileName = filename;
            exporter.GridViewID = "Grid_PurchaseOrder";
            exporter.FileName = "BillOfEntry";

            exporter.PageHeader.Left = "Import Bill Of Entry (B/E)";
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

        #endregion Export Grid Section End

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = GetPurchaseIndentListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_PurchaseOrder.DataSource = dtdata;
                Grid_PurchaseOrder.DataBind();
            }
            else
            {
                Grid_PurchaseOrder.DataSource = null;
                Grid_PurchaseOrder.DataBind();
            }
        }

        public DataTable GetPurchaseIndentListGridData()
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "ProductOrderDetailsOpening");
            // proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString("2018-2019"));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 500, Convert.ToString(Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Opening", 50, Opening);
            dt = proc.GetTable();
            return dt;
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

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
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
            Grid_PurchaseOrder.JSProperties["cpDelete"] = null;
            if (Command == "Delete")
            {
                //if (!IsPITransactionExist(POID))
                //{
                ImportPurchaseinvoiceBillEntry objBillEntryBL = new ImportPurchaseinvoiceBillEntry();
                deletecnt = objBillEntryBL.DeletePurchaseOrder(POID);

                    if (deletecnt == 1)
                    {
                        Grid_PurchaseOrder.JSProperties["cpDelete"] = "Deleted successfully";

                    }

                //}
                //else
                //{
                //    Grid_PurchaseOrder.JSProperties["cpDelete"] = "This Purchase Order is tagged in other modules. Cannot Delete.";
                //}

            }
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
        //private bool IsPITransactionExistInPOIn(string Poid)
        //{
        //    bool IsExist = false;
        //    if (Poid != "" && Convert.ToString(Poid).Trim() != "")
        //    {
        //        DataTable dt = new DataTable();
        //        dt = objPurchaseOrderBL.CheckPOTraanactionForPoINV(Poid);
        //        if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
        //        {
        //            IsExist = true;
        //        }
        //    }

        //    return IsExist;
        //}

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
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            //}
            //else
            //{
            //    lblWaiting.Text = "";
            //}
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
            int branchid = 0;
            if (Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(Session["userbranchID"]);
            }

            i = objERPDocPendingApproval.ConditionWiseShowStatusButton(7, branchid, Convert.ToString(Session["userid"]));  // 7 for Purchase Order Module 
            if (i == 1)
            {
                spanStatus.Visible = true;
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End


        #endregion Sandip Section For Approval Dtl Section End


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseOrder_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;


            if (strBranchID == "0")
            {
                string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                ImportmoduleclassDataContext dc = new ImportmoduleclassDataContext(connectionString);
                var q = from d in dc.v_PurchaseOrderList_Import_BillEntries
                        //where d.PurchaseOrder_Date >= Convert.ToDateTime(strFromDate) && d.PurchaseOrder_Date <= Convert.ToDateTime(strToDate)
                        where
                        //branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                        d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                        && d.OrderAdd_addressType == "Shipping"
                        //&& d.CreatedBy == userid
                     
                        orderby d.PurchaseOrder_Id descending
                        select d;

                e.QueryableSource = q;
                // var cnt = q.Count();
            }
            else
            {
                branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                ImportmoduleclassDataContext dc = new ImportmoduleclassDataContext(connectionString);
                var q = from d in dc.v_PurchaseOrderList_Import_BillEntries
                        where
                        //branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                         d.PurchaseOrderDt >= Convert.ToDateTime(strFromDate) && d.PurchaseOrderDt <= Convert.ToDateTime(strToDate)
                        && d.OrderAdd_addressType == "Shipping"
                        //&& d.CreatedBy == userid
                        && branchidlist.Contains(Convert.ToInt32(d.PurchaseOrder_BranchId))
                        orderby d.PurchaseOrder_Id descending
                        select d;
                e.QueryableSource = q;
            }

        }

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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }


        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


        [WebMethod]
        public static string TaggigExistance(string Id)
        {
            // Mantis Issue 24310_Rev2 [ Incorrect checking]
            return "0";
            // End of Mantis Issue 24310_Rev2
            string sexits = "0";
            bool IsExist = false;
            if (Id != "" && Convert.ToString(Id).Trim() != "")
            {
                DataTable dt = new DataTable();
                ImportPurchaseOrderBL objPurchaseOrderBL = new ImportPurchaseOrderBL();
                dt = objPurchaseOrderBL.CheckPOTraanaction(Id);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    sexits = "1";
                }
            }
           
            return sexits;
        }


    }
}