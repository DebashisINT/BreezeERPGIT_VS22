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
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Threading.Tasks;
namespace ERP.OMS.Management.Activities
{
    public partial class frm_EstimateCostSheetMain : ERP.OMS.ViewState_class.VSPage
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
        int KeyValue = 0;
        public bool IsApprove { get; set; }

        public bool IsInquiryStatus { get; set; }

        #region Page Load Section Start


        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Session["SO_ProductDetails"] = null;
            #region Button Wise Right Access Section Start
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/EstimateCostSheetList.aspx");
            #endregion Button Wise Right Access Section End
            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApprovalRevisionEstimateCostSheet = ComBL.GetSystemSettingsResult("ApprovalRevisionEstimateCostSheet");
            string StatusRequiredECS = ComBL.GetSystemSettingsResult("StatusRequiredECS");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {

                    GrdQuotation.Columns[7].Width = 250;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GrdQuotation.Columns[7].Width = 0;
                }
            }


            if (!String.IsNullOrEmpty(ApprovalRevisionEstimateCostSheet))
            {
                if (ApprovalRevisionEstimateCostSheet == "Yes")
                {
                    IsApprove = true;
                    GrdQuotation.Columns[12].Visible = true;
                    GrdQuotation.Columns[13].Visible = true;
                    GrdQuotation.Columns[14].Visible = true;
                }
                else if (ApprovalRevisionEstimateCostSheet.ToUpper().Trim() == "NO")
                {
                    IsApprove = false;
                    GrdQuotation.Columns[12].Visible = false;
                    GrdQuotation.Columns[13].Visible = false;
                    GrdQuotation.Columns[14].Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(StatusRequiredECS))
            {
                if (StatusRequiredECS == "Yes")
                {
                    IsInquiryStatus = true;
                    GrdQuotation.Columns[8].Visible = true;
                }
                else if (StatusRequiredECS.ToUpper().Trim() == "NO")
                {
                    IsInquiryStatus = false;
                    GrdQuotation.Columns[9].Visible = false;
                }
            }




            if (!IsPostBack)
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


                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End

                ConditionWiseShowStatusButton();
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                DataTable watingInvoice = posSale.SalesBasketDetails(userbranchHierachy);

                waitingQuotationCount.Value = Convert.ToString(watingInvoice.Rows.Count);
                //lblQuoteweatingCount.Text = Convert.ToString(watingInvoice.Rows.Count);
                //watingQuotegrid.DataSource = watingInvoice;
                //watingQuotegrid.DataBind();
                #region Is is using in EstimateCostSheet Page need to remove from this side
                Session.Remove("SaveMode");
                Session.Remove("schemavalue");
                #endregion Is is using in EstimateCostSheet Page need to remove from this side

                #region Company and Branch Hierarchy Wise EstimateCostSheet Detail List Section Start

                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;


                //if (HttpContext.Current.Session["LastCompany"] != null)
                //{
                //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                //    {
                //        GetQuotationListGridData(userbranch, lastCompany);
                //    }
                //}

                #endregion Company and Branch Hierarchy Wise EstimateCostSheet Detail List Section End

                #region User Wise Document Show

                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


                DataTable DT = objEngine.GetDataTable("tbl_master_user", " user_IsUserwise ", " user_id='" + Convert.ToString(Session["userid"]) + "'");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsUserwise = Convert.ToString(DT.Rows[0]["user_IsUserwise"]).Trim();

                    if (IsUserwise == "True") hfIsUserwise.Value = "Y";
                    else hfIsUserwise.Value = "N";
                }

                #endregion
            }

            //if (divPendingWaiting.Visible == true)
            //{
            //    PopulateUserWiseERPDocCreation();
            //    PopulateApprovalPendingCountByUserLevel();
            //    PopulateERPDocApprovalPendingListByUserLevel();
            //}

            #region Show Count Pending Approval  in Popup grid User Level Wise Start
            Session["exportval"] = null;

            #endregion Show Count Pending Approval in Popup grid User Level Wise End


            //int loginuserid = 0;

            //if (Session["userid"] != null)
            //{
            //    loginuserid = Convert.ToInt32(Session["userid"]);
            //    PopulateApprovalPendingCountByUserLevel(loginuserid);
            //}

            #region Jsproperties Section Initialized Section Start
            GrdQuotation.JSProperties["cpinsert"] = null;
            GrdQuotation.JSProperties["cpEdit"] = null;
            GrdQuotation.JSProperties["cpUpdate"] = null;
            GrdQuotation.JSProperties["cpDelete"] = null;
            GrdQuotation.JSProperties["cpExists"] = null;
            GrdQuotation.JSProperties["cpUpdateValid"] = null;
            #endregion Jsproperties Section Initialized Section Start

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

        #endregion Page Load Section End

        #region Main Grid Event Section Start

        protected void GrdQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            GrdQuotation.JSProperties["cpinsert"] = null;
            GrdQuotation.JSProperties["cpEdit"] = null;
            GrdQuotation.JSProperties["cpUpdate"] = null;
            GrdQuotation.JSProperties["cpDelete"] = null;
            GrdQuotation.JSProperties["cpExists"] = null;
            GrdQuotation.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Edit")
            {

                DataTable dtECStatus = GetQuotationStatusByQuotationID(WhichType);
                if (dtECStatus.Rows.Count > 0 && dtECStatus != null)
                {
                    string quoteid = Convert.ToString(dtECStatus.Rows[0]["quoteid"]);
                    string quoteNumber = Convert.ToString(dtECStatus.Rows[0]["quoteNumber"]);
                    string Status = Convert.ToString(dtECStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtECStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtECStatus.Rows[0]["CustomerName"]);
                    GrdQuotation.JSProperties["cpEdit"] = quoteid + "~"
                                                    + quoteNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            else if (WhichCall == "update")
            {
                if (Convert.ToString(e.Parameters).Contains("~"))
                {
                    if (Convert.ToString(e.Parameters).Split('~')[2] != "")
                    {
                        QuoteStatus = Convert.ToString(e.Parameters).Split('~')[2];
                        remarks = Convert.ToString(e.Parameters).Split('~')[3];
                    }
                }
                int dtECStatus = UpdateQuotationStatusByCustomer(Convert.ToInt32(WhichType), Convert.ToInt32(QuoteStatus), remarks);
                if (dtECStatus == 1)
                {
                    GrdQuotation.JSProperties["cpUpdate"] = "Save Successfully";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpUpdate"] = "Save unsuccessful";
                }


            }
            else if (WhichCall == "Delete")
            {
                deletecnt = DeleteECS(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
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
                dtdata = objCRMSalesDtlBL.GetQuotationListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdQuotation.DataSource = dtdata;
                    GrdQuotation.DataBind();
                }
                else
                {
                    GrdQuotation.DataSource = null;
                    GrdQuotation.DataBind();
                }
            }
        }
        public int DeleteECS(string Quoteid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_EstimateCostSheetAddEdit");
            proc.AddVarcharPara("@Action", 100, "DeleteECS");
            proc.AddVarcharPara("@DeleteQuoteId", 20, Quoteid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        //protected void watingQuotegrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    string receivedString = e.Parameters;

        //    if (receivedString.Split('~')[0] == "Remove")
        //    {
        //        string key = receivedString.Split('~')[1];
        //        posSale.DeleteBasketDetailsFromtable(key, Convert.ToInt32(Session["userid"]));
        //        watingQuotegrid.JSProperties["cpReturnMsg"] = "Billing Request has been Deleted Successfully.";
        //        watingQuotegrid.DataBind();
        //    }
        //}
        //protected void watingQuotegrid_DataBinding(object sender, EventArgs e)
        //{
        //    watingQuotegrid.DataSource = posSale.SalesBasketDetails(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
        //}
        //public void GetQuotationListGridData(string userbranch, string lastCompany)
        //{
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));
        //    string finyear = Convert.ToString(Session["LastFinYear"]);

        //    DataTable dtdata = new DataTable();
        //    dtdata = objCRMSalesDtlBL.GetQuotationListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        GrdQuotation.DataSource = dtdata;
        //        GrdQuotation.DataBind();
        //    }
        //    else
        //    {
        //        GrdQuotation.DataSource = null;
        //        GrdQuotation.DataBind();
        //    }
        //}        
        //protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        //{
        //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //    string finyear = Convert.ToString(Session["LastFinYear"]);
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

        //    DataTable dtdata = new DataTable();
        //    dtdata = objCRMSalesDtlBL.GetQuotationListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        GrdQuotation.DataSource = dtdata;
        //    }
        //    else
        //    {
        //        GrdQuotation.DataSource = null;
        //    }
        //}

        #endregion Main Grid Event Section End

        #region Web Method For Child Page Section Start

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

            //    BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();


            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {

            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }
        //REV RAJDIP

        [WebMethod]
        public static string GetEditablePermissions(string ActiveUser, string SalesDocId)
        {

            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermissions(Convert.ToInt32(ActiveUser), Convert.ToInt32(SalesDocId));

            //}
            return Convert.ToString(ispermission);

        }
        //END REV RAJDIP
        [WebMethod]
        public static string GetTotalWatingQuotationCount()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            return Convert.ToString(posSale.GetQuotationCount(userbranchHierachy));
        }



        #endregion Web Method For Child Page Section End

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
            //GrdQuotation.Columns[6].Visible = false;
            string filename = "Estimate / Cost Sheet";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Estimate / Cost Sheet";
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

        #region Approval Waiting or Pending User Level Wise Section Start

        public void PopulateERPDocApprovalPendingListByUserLevel()
        {
            DataTable dtdata = new DataTable();
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    int userid = 0;
                    userid = Convert.ToInt32(Session["userid"]);

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "QO");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        gridPendingApproval.DataSource = dtdata;
                        gridPendingApproval.DataBind();
                        Session["PendingApproval"] = dtdata;
                    }
                    else
                    {
                        gridPendingApproval.DataSource = null;
                        gridPendingApproval.DataBind();
                    }
                }
            }
        }

        public void PopulateApprovalPendingCountByUserLevel()
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "QO");
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["pendinQuotation"]) + ")";
            //}
            //else
            //{
            //    lblWaiting.Text = "";
            //}
        }


        protected void gridPendingApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                Session.Remove("PendingApproval"); // To Rebind from database due to Grid approvalval functionality
                userid = Convert.ToInt32(Session["userid"]);
                PopulateERPDocApprovalPendingListByUserLevel();
                gridPendingApproval.JSProperties["cpEdit"] = "F";
                Session.Remove("UserWiseERPDocCreation"); //To Rebind from database due to GridPending approvalval functionality effects this grid
            }
            if (Session["KeyValue"] != null)
            {
                Session.Remove("KeyValue");
            }

        }

        protected void chkapprove_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        }


        protected void chkreject_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetRejectedQuoteId(s, e, {0}) }}", itemindex);

        }

        #endregion Approval Waiting or Pending User Level Wise Section End

        #region Created User Wise List EstimateCostSheet after Clicking on Status Button Section Start  (call in page load)

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
            if (Session["UserWiseERPDocCreation"] == null)
            {
                //dtdata = new DataTable();
                dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "QO");
            }
            else
            {
                dtdata = (DataTable)Session["UserWiseERPDocCreation"];
            }
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridUserWiseQuotation.DataSource = dtdata;
                gridUserWiseQuotation.DataBind();
                Session["UserWiseERPDocCreation"] = dtdata;
            }
            else
            {
                gridUserWiseQuotation.DataSource = null;
                gridUserWiseQuotation.DataBind();
            }

        }
        #endregion #region Created User Wise List EstimateCostSheet after Clicking on Status Button Section End

        //#region To Show Hide Status and Pending Approval Button Configuration Wise Start
        //public void ConditionWiseShowStatusButton()
        //{
        //    int i = 0;
        //    int branchid = 0;
        //    if (Session["userbranchID"] != null)
        //    {
        //        branchid = Convert.ToInt32(Session["userbranchID"]);
        //    }

        //    i = objERPDocPendingApproval.ConditionWiseShowStatusButton(1, branchid, Convert.ToString(Session["userid"]));
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
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(1, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "QO");

            //if (j == 1)
            //{
            //    spanStatus.Visible = true;
            //}
            //else
            //{
            //    spanStatus.Visible = false;
            //}


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(1, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "QO");

            //if (k == 1)
            //{
            //    divPendingWaiting.Visible = true;
            //}
            //else
            //{
            //    divPendingWaiting.Visible = false;
            //}



            #endregion Sam Section For Showing Status and Approval waiting Button on 22052017
            // Cross Branch Section by Sam on 10052017 Start  
            //i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // Entity Id 8 For Purchase Invoice
            ////i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // 7 for Purchase Order Module 
            ////i = objERPDocPendingApproval.ConditionWiseShowStatusButton(8, branchid, Convert.ToString(Session["userid"])); //Entity Id 8 For Purchase Invoice
            //// Cross Branch Section by Sam on 10052017 End 
            //if (i == 1)
            //{
            //    spanStatus.Visible = true;
            //    divPendingWaiting.Visible = true;
            //}
            //else if (i == 2)
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = true;
            //}
            //else
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = false;
            //}
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "QO");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["pendinQuotation"]) + ")";
            }

            return strPending;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\Proforma\DocDesign\Designes";
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
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End

        #region trash Code Section Start
        protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateERPDocApprovalPendingListByUserLevel();
        }
        //public void PopulateApprovalPendingDtlByUserLevel(int userid)
        //{
        //    //int userid = 0;
        //    //userid = Convert.ToInt32(Session["userid"]);
        //    int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);
        //    Session["userlevel"] = userlevel;
        //    DataTable dtdata = new DataTable();
        //    //PopulateERPDocApprovalPendingListByUserLevel
        //    dtdata = objCRMSalesDtlBL.GetPendingQuotationListByUserLevel(userlevel);
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        gridPendingApproval.DataSource = dtdata;   
        //        gridPendingApproval.DataBind();
        //        Session["PendingApproval"] = dtdata;
        //    }
        //    else
        //    {
        //        gridPendingApproval.DataSource = null;
        //        gridPendingApproval.DataBind();
        //    }
        //}

        //public void GetUserLevelPermissionByUserId(int activeUser)
        //{
        //    CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        //    int ispermission = 0;
        //    ispermission = objCRMSalesDtlBL.QuotationEditablePermission(activeUser);
        //    if (ispermission == 2)
        //    {
        //        divPendingWaiting.Visible = true;
        //    }
        //    else
        //    {
        //        divPendingWaiting.Visible = false;
        //    }
        //}
        //#region EstimateCostSheet Status By Client Section Start
        //public void UpdAteECStatus(string chkbox, int keyVal, int userid)
        //{
        //    //int userid = 0;
        //    //userid = Convert.ToInt32(Session["userid"]);
        //    int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);
        //    Session["userlevel"] = userlevel;
        //    Int64 i = -1;
        //    i = objCRMSalesDtlBL.UpdatePendingQuotationListByUserLevel(chkbox, keyVal, userlevel);
        //}

        //#endregion EstimateCostSheet Status By Client Section End

        #endregion
        //Rev Rajdip
        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            // string available = Convert.ToString(e.GetValue("IsCancel"));
            //string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            //if (available.ToUpper() == "TRUE")
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Red;
            //    e.Row.Font.Strikeout = true;
            //}
            //if (availableClosed.ToString() == "Closed")
            //{

            //    //e.Row.ForeColor = System.Drawing.Color.Red;
            //    //e.Row.Font.Strikeout = true;
            //    e.Row.ForeColor = System.Drawing.Color.Gray;

            //}
            //else
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Black;
            //}

        }
        //End Rev Rajdip
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "EstimateCost_Id";

          //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string IsUserwise = Convert.ToString(hfIsUserwise.Value);
            //string UserID = Convert.ToString(Session["userid"]);

            string CompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            int userid = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    if(IsUserwise=="Y")
                    {
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.v_EstimateCostSheets
                        //        where d.EstimateCost_Date >= Convert.ToDateTime(strFromDate) && d.EstimateCost_Date <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))

                        //        orderby d.EstimateCost_Date descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.EstimateCostSheetLists
                                where d.USERID == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.v_EstimateCostSheets
                        //        where d.EstimateCost_Date >= Convert.ToDateTime(strFromDate) && d.EstimateCost_Date <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                        //        orderby d.EstimateCost_Date descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.EstimateCostSheetLists
                                where d.USERID == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    if (IsUserwise == "Y")
                    {
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.v_EstimateCostSheets
                        //        where
                        //        d.EstimateCost_Date >= Convert.ToDateTime(strFromDate) && d.EstimateCost_Date <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                        //        orderby d.EstimateCost_Date descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.EstimateCostSheetLists
                                where d.USERID == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.v_EstimateCostSheets
                        //        where
                        //        d.EstimateCost_Date >= Convert.ToDateTime(strFromDate) && d.EstimateCost_Date <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                        //        orderby d.EstimateCost_Date descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.EstimateCostSheetLists
                                where d.USERID == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_EstimateCostSheets
                //        where d.EstimateCost_Id == '0' && d.BranchID == 0
                //        orderby d.EstimateCost_Date descending
                //        select d;
                //e.QueryableSource = q;

                var q = from d in dc.EstimateCostSheetLists
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
            }
        }

        [WebMethod]
        public static string GetSalesQuoteIsExistInSalesOrder(string keyValue)
        {
            CRMSalesDtlBL objCRMSaleDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSaleDtlBL.GetIdFromSalesOrder(keyValue);
            return Convert.ToString(ispermission);
        }
        public DataTable GetProductDetails(string ECS_ID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EstimateCostSheetEditDetails");
            proc.AddVarcharPara("@Action", 500, "BindGridECSLinesProducts");
            proc.AddVarcharPara("@EstimateCostID", 500, ECS_ID);
            dt = proc.GetTable();
            return dt;
        }
        protected void grid_Products_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                Session["exportProductval"] = null;
                string ECS_ID = Convert.ToString(e.Parameters.Split('~')[1]);
                Session["ECS_ID"] = ECS_ID;


                if (ECS_ID != "")
                {
                    DataTable dt_EstimateCostDetails = new DataTable();
                    //string IdKey = Convert.ToString(Request.QueryString["key"]);
                    dt_EstimateCostDetails = GetProductDetails(ECS_ID);
                    //  Session["EstimateCostDetails"] = null;

                    Session["SO_ProductDetails"] = dt_EstimateCostDetails;
                    grid_Products.DataSource = GetProductsInfo(dt_EstimateCostDetails);
                    grid_Products.DataBind();


                    DataTable Orderdt = dt_EstimateCostDetails;
                    decimal Total_Qty_Mtr = 0;
                    decimal COSTVALUE = 0;
                    decimal TotalWt = 0;
                    decimal TotalSqMtr = 0;
                    decimal ToleranceTotalWt = 0;
                    decimal SalesVALUE = 0;
                    decimal ProfitMargin = 0;
                    for (int i = 0; i < Orderdt.Rows.Count; i++)
                    {
                        Total_Qty_Mtr = Total_Qty_Mtr + Convert.ToDecimal(Orderdt.Rows[i]["EstCostDetails_Qty_Mtr"]);
                        TotalSqMtr = TotalSqMtr + Convert.ToDecimal(Orderdt.Rows[i]["EstCostDetails_Sqmtr"]);
                        TotalWt = TotalWt + Convert.ToDecimal(Orderdt.Rows[i]["EstCostDetails_T_WT"]);
                        ToleranceTotalWt = ToleranceTotalWt + Convert.ToDecimal(Orderdt.Rows[i]["EstCostDetails_Tol_t_wt"]);
                        COSTVALUE = COSTVALUE + Convert.ToDecimal(Orderdt.Rows[i]["EstCostDetails_FOR_CP_VAL"]);
                        SalesVALUE = SalesVALUE + Convert.ToDecimal(Orderdt.Rows[i]["Value"]);
                    }

                    lbl_Total_Qty_Mtr.Text = Total_Qty_Mtr.ToString();
                    bnrLblTotalSqMtr.Text = TotalSqMtr.ToString();
                    bnrLblTotalWt.Text = TotalWt.ToString();
                    bnrlblToleranceTotalWt.Text = ToleranceTotalWt.ToString();
                    bnrCOSTVALUE.Text = COSTVALUE.ToString();
                    bnrSalesValue.Text = SalesVALUE.ToString();
                    string TOTALProfitMargin = "";
                    if (COSTVALUE != 0)
                    {
                        TOTALProfitMargin = (Math.Round(((SalesVALUE - COSTVALUE) / COSTVALUE) * 100, 2)).ToString();
                    }

                    // bnrLblProfitMargin.Text = (Math.Round(((SalesVALUE - COSTVALUE) / COSTVALUE) * 100, 2)).ToString();

                    grid_Products.JSProperties["cpRunningTotal"] = Total_Qty_Mtr + "~" + TotalSqMtr + "~" + TotalWt + "~" + ToleranceTotalWt + "~" + COSTVALUE + "~" + SalesVALUE + "~" + TOTALProfitMargin;

                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }

            }
        }

        public void BindGrid()
        {
            DataTable dt_EstimateCostDetails = new DataTable();
            dt_EstimateCostDetails = GetProductDetails(Convert.ToString(Session["ECS_ID"]));

            grid_Products.DataSource = GetProductsInfo(dt_EstimateCostDetails);
            grid_Products.DataBind();

        }
        protected void grid_Products_DataBinding(Object sender, EventArgs e)
        {

            if (Session["SO_ProductDetails"] != null)
            {
                DataTable EstimateCostdt = (DataTable)Session["SO_ProductDetails"];
                DataView dvData = new DataView(EstimateCostdt);

                grid_Products.DataSource = GetProductsInfo(EstimateCostdt);
            }
        }
        public class EstimateCostSheet
        {
            public string SrlNo { get; set; }
            public string EstCostDetails_Id { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }

            public string ProductName { get; set; }
            public string Remarks { get; set; }

            public string InquiryID { get; set; }
            public string DetailsId { get; set; }
            public string EstCostDetails_NB { get; set; }
            public string EstCostDetails_OD { get; set; }
            public string EstCostDetails_Thk { get; set; }
            public string EstCostDetails_Tol_thk { get; set; }
            public string EstCostDetails_Wt_Mtr { get; set; }
            public string EstCostDetails_Tol_wt { get; set; }
            //public string EstCostDetails_Wt_Mtr { get; set; }
            public string EstCostDetails_Qty_Mtr { get; set; }
            public string EstCostDetails_Sqmtr { get; set; }
            public string EstCostDetails_T_WT { get; set; }
            public string EstCostDetails_Tol_t_wt { get; set; }
            public string EstCostDetails_Rate_kg { get; set; }
            public string EstCostDetails_Frt { get; set; }
            public string EstCostDetails_Surface_prep { get; set; }
            public string EstCostDetails_Int_Coating { get; set; }
            public string EstCostDetails_Ext_Coating { get; set; }
            public string EstCostDetails_Rate_sqm { get; set; }
            public string EstCostDetails_Ratekg { get; set; }
            public string EstCostDetails_CP_MT { get; set; }
            public string EstCostDetails_FOR_CP_MTR { get; set; }
            public string EstCostDetails_FOR_CP_VAL { get; set; }

            public string EstCostDetails_Gross_Margin { get; set; }
            public string EstCostDetails_Gross_Marg_Value_Mtr { get; set; }
            public string EstCostDetails_SP_Mtr { get; set; }
            public string Value { get; set; }

            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }

            public string Inquiry_Number { get; set; }

            public string Inquiry_Date { get; set; }

        }
        public IEnumerable GetProductsInfo(DataTable EstimateCostdt)
        {
            List<EstimateCostSheet> QuotationList = new List<EstimateCostSheet>();

            if (EstimateCostdt != null && EstimateCostdt.Rows.Count > 0)
            {
                for (int i = 0; i < EstimateCostdt.Rows.Count; i++)
                {
                    EstimateCostSheet ECS = new EstimateCostSheet();

                    ECS.SrlNo = Convert.ToString(EstimateCostdt.Rows[i]["SrlNo"]);
                    ECS.EstCostDetails_Id = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Id"]);
                    ECS.ProductID = Convert.ToString(EstimateCostdt.Rows[i]["ProductID"]);
                    ECS.Description = Convert.ToString(EstimateCostdt.Rows[i]["Description"]);
                    ECS.Quantity = Convert.ToString(EstimateCostdt.Rows[i]["Quantity"]);
                    ECS.UOM = Convert.ToString(EstimateCostdt.Rows[i]["UOM"]);


                    ECS.InquiryID = Convert.ToString(EstimateCostdt.Rows[i]["InquiryID"]);
                    ECS.DetailsId = Convert.ToString(EstimateCostdt.Rows[i]["DetailsId"]);
                    ECS.EstCostDetails_NB = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_NB"]);
                    ECS.EstCostDetails_OD = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_OD"]);
                    ECS.EstCostDetails_Thk = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Thk"]);
                    ECS.EstCostDetails_Tol_thk = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Tol_thk"]);
                    ECS.EstCostDetails_Wt_Mtr = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Wt_Mtr"]);
                    ECS.EstCostDetails_Tol_wt = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Tol_wt"]);

                    ECS.EstCostDetails_Qty_Mtr = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Qty_Mtr"]);
                    ECS.EstCostDetails_Sqmtr = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Sqmtr"]);
                    ECS.EstCostDetails_T_WT = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_T_WT"]);
                    ECS.EstCostDetails_Tol_t_wt = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Tol_t_wt"]);
                    ECS.EstCostDetails_Rate_kg = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Rate_kg"]);


                    ECS.EstCostDetails_Frt = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Frt"]);
                    ECS.EstCostDetails_Surface_prep = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Surface_prep"]);
                    ECS.EstCostDetails_Int_Coating = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Int_Coating"]);
                    ECS.EstCostDetails_Ext_Coating = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Ext_Coating"]);
                    ECS.EstCostDetails_Rate_sqm = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Rate_sqm"]);
                    ECS.EstCostDetails_Ratekg = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Ratekg"]);
                    ECS.EstCostDetails_CP_MT = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_CP_MT"]);
                    ECS.EstCostDetails_FOR_CP_MTR = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_FOR_CP_MTR"]);
                    ECS.EstCostDetails_FOR_CP_VAL = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_FOR_CP_VAL"]);
                    ECS.EstCostDetails_Gross_Margin = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Gross_Margin"]);
                    ECS.EstCostDetails_Gross_Marg_Value_Mtr = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_Gross_Marg_Value_Mtr"]);


                    ECS.EstCostDetails_SP_Mtr = Convert.ToString(EstimateCostdt.Rows[i]["EstCostDetails_SP_Mtr"]);
                    ECS.Value = Convert.ToString(EstimateCostdt.Rows[i]["Value"]);

                    ECS.Order_AltQuantity = Convert.ToString(EstimateCostdt.Rows[i]["Order_AltQuantity"]);
                    ECS.Order_AltUOM = Convert.ToString(EstimateCostdt.Rows[i]["Order_AltUOM"]);

                    ECS.ProductName = Convert.ToString(EstimateCostdt.Rows[i]["ProductName"]);

                    if (EstimateCostdt.Columns.Contains("Remarks"))
                    {
                        ECS.Remarks = Convert.ToString(EstimateCostdt.Rows[i]["Remarks"]);
                    }
                    else
                    {
                        ECS.Remarks = "";
                    }

                    ECS.Inquiry_Number = Convert.ToString(EstimateCostdt.Rows[i]["Inquiry_Number"]);
                    ECS.Inquiry_Date = Convert.ToString(EstimateCostdt.Rows[i]["Inquiry_Date"]);

                    QuotationList.Add(ECS);
                }
            }

            return QuotationList;
        }

        protected void ExportProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ExportProduct.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportProductval"] == null)
                {
                    Session["exportProductval"] = Filter;
                    BindGrid();
                    bindexportProduct(Filter);
                }
                else if (Convert.ToInt32(Session["exportProductval"]) != Filter)
                {
                    Session["exportProductval"] = Filter;
                    BindGrid();
                    bindexportProduct(Filter);
                }
            }
        }
        public void bindexportProduct(int Filter)
        {

            string filename = "EstimateCostSheeLineItems";
            productGVExporter.FileName = filename;

            productGVExporter.PageHeader.Left = "Estimate / Cost Sheet";
            productGVExporter.PageFooter.Center = "[Page # of Pages #]";
            productGVExporter.PageFooter.Right = "[Date Printed]";
            productGVExporter.GridViewID = "grid_Products";
            switch (Filter)
            {
                case 1:
                    productGVExporter.WritePdfToResponse();
                    break;
                case 2:
                    //productGVExporter.WriteXlsToResponse();
                    productGVExporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    productGVExporter.WriteRtfToResponse();
                    break;
                case 4:
                    productGVExporter.WriteCsvToResponse();
                    break;
            }
        }

        protected void watingQuotegrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receivedString = e.Parameters;

            if (receivedString.Split('~')[0] == "Remove")
            {
                string key = receivedString.Split('~')[1];
                posSale.DeleteBasketDetailsFromtable(key, Convert.ToInt32(Session["userid"]));
                watingQuotegrid.JSProperties["cpReturnMsg"] = "Billing Request has been Deleted Successfully.";
                watingQuotegrid.DataBind();
            }
        }
        protected void watingQuotegrid_DataBinding(object sender, EventArgs e)
        {
            watingQuotegrid.DataSource = posSale.SalesBasketDetails(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
        }
        public DataTable GetQuotationStatusByQuotationID(string quoteid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EstimateCostSheetEditDetails");
            proc.AddNVarcharPara("@action", 150, "GetEstimateStatusByEstimateID");
            proc.AddIntegerPara("@EstimateCostID", Convert.ToInt32(quoteid));
            dt = proc.GetTable();
            return dt;
        }
        public int UpdateQuotationStatusByCustomer(int Qouteid, int QuoteStatus, string remarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_EstimateCostSheetEditDetails");
            proc.AddVarcharPara("@Action", 100, "UpdateEstimateStatusByCustomer");
            proc.AddVarcharPara("@remarks", 500, remarks);
            proc.AddIntegerPara("@EstimateCostID", Qouteid);
            proc.AddIntegerPara("@QuoteStatus", QuoteStatus);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

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
            Task PopulateStockTrialDataTask = new Task(() => GetEstimateCostSheedata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetEstimateCostSheedata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_EstimateCostSheet_List", con);
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
}