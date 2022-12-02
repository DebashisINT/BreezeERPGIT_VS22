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
using System.Linq;
using ERP.Models;
namespace ERP.OMS.Management.Activities
{  
    public partial class PurchaseChallanListForCustomer : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PurchaseChallanBL objPurchaseChallanBLBL = new PurchaseChallanBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        int KeyValue = 0;
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseChallan_Id";

           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_PurchaseChallanLists
                            where
                            d.Challan_Date >= Convert.ToDateTime(strFromDate) && d.Challan_Date <= Convert.ToDateTime(strToDate)
                            &&
                            branchidlist.Contains(Convert.ToInt32(d.BranchID)) && 
                            Convert.ToString(d.Entity_Type) == "CL"
                            //--&& Convert.ToString(d.Doc_Type) != "SC"
                            orderby d.PurchaseChallan_Id descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_PurchaseChallanLists
                            where
                            d.Challan_Date >= Convert.ToDateTime(strFromDate) && d.Challan_Date <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && Convert.ToString(d.Entity_Type) == "CL"
                            //&& Convert.ToString(d.Doc_Type) != "SC"
                            orderby d.PurchaseChallan_Id descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_PurchaseChallanLists
                        where Convert.ToString(d.PurchaseChallan_Id) == "0"
                        orderby d.PurchaseChallan_Id descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseChallanList.aspx");
            if (!IsPostBack)
            {
                //Grid_PurchaseChallan.SettingsCookies.CookiesID = "BreeezeErpGridCookies<GridName>";

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookies<GridName>');</script>");
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
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

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
        //            dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
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
            Grid_PurchaseChallan.Columns[4].Visible = false;
            string filename = "Purchase GRN for Customer";
            exporter.FileName = filename;
            exporter.FileName = "GRN";

            exporter.PageHeader.Left = "Purchase GRN for Customer";
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
                        deletecnt = objPurchaseChallanBLBL.DeletePurchaseChallan(POID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
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
            //else if (Command == "FilterGridByDate")
            //{
            //    DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
            //    DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
            //    string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

            //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //    string finyear = Convert.ToString(Session["LastFinYear"]);

            //    DataTable dtdata = new DataTable();
            //    dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
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
        }
        protected void Grid_PurchaseChallan_DataBinding(object sender, EventArgs e)
        {
            //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //string finyear = Convert.ToString(Session["LastFinYear"]);
            //string BranchID = Convert.ToString(cmbBranchfilter.Value);
            //DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            //DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            //DataTable dtdata = new DataTable();
            //dtdata = objPurchaseChallanBLBL.GetPurchaseChallanListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    Grid_PurchaseChallan.DataSource = dtdata;
            //}
            //else
            //{
            //    Grid_PurchaseChallan.DataSource = null;
            //}
        }

        protected void cgridDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "BindDocumentsDetails")
            //{
            //    DataTable dtDesign = new DataTable();
            //    for (int i = 0; i < grid_Documents.VisibleRowCount; i++)
            //    {
            //        grid_Documents.Selection.UnselectRow(i);
            //    }
            //    dtDesign.Columns.Add("ID");
            //    dtDesign.Columns.Add("NAME");
            //    DataRow drDesign;
            //    string DesignName;
            //    for (int i = 1; i < 4; i++)
            //    {
            //        if (i == 1)
            //        {
            //            DesignName = "Original";
            //        }
            //        else if (i == 2)
            //        {
            //            DesignName = "Duplicate";
            //        }
            //        else
            //        {
            //            DesignName = "Triplicate";
            //        }
            //        drDesign = dtDesign.NewRow();
            //        drDesign[0] = i;
            //        drDesign[1] = DesignName;
            //        dtDesign.Rows.Add(drDesign);
            //    }
            //    grid_Documents.DataSource = dtDesign;
            //    grid_Documents.DataBind();
            //    grid_Documents.JSProperties["cpSuccess"] = null;
            //}
            //else if (strSplitCommand == "BindDocumentsGridOnSelection")
            //{
            //    string SelectedDocList = "";
            //    var ChallanId = Convert.ToString(e.Parameters.Split('~')[1]);
            //    string NoofCopy = "";
            //    List<object> docList = grid_Documents.GetSelectedFieldValues("NAME");
            //    foreach (object Dobj in docList)
            //    {
            //        SelectedDocList += "," + Dobj;
            //        if (Dobj.ToString() == "Triplicate")
            //        {
            //            NoofCopy += 3 + ",";
            //        }
            //        else if (Dobj.ToString() == "Duplicate")
            //        {
            //            NoofCopy += 2 + ",";
            //        }
            //        else if (Dobj.ToString() == "Original")
            //        {
            //            NoofCopy += 1 + ",";
            //        }
            //    }
            //    SelectedDocList = SelectedDocList.TrimStart(',');
            //    if (SelectedDocList.Trim() == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Please Select Some Document(s)')", true);
            //    }
            //    else
            //    {
            //        Session["SelectedDocumentList"] = SelectedDocList;
            //        grid_Documents.JSProperties["cpSuccess"] = NoofCopy;
            //    }
            //}

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
    }
}