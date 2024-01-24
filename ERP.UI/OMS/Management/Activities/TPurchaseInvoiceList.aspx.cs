/**********************************************************************************************************************************
 1.0      04-12-2023        V2.0.41       Priti         0027044: Feature to enable to edit Party Invoice Number and Party Invoice date in Transit Purchase Invoice
 2.0      24-01-2024        V2.0.43       Sanchita      0027206: Listing page for modules Transit Purchase invoice      
 ***********************************************************************************************************************************/
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
using System.Drawing;
using System.IO;
using ERP.OMS.ViewState_class;
using ERP.Models;
using System.Linq;
using static ERP.OMS.Management.Activities.PurchaseInvoicelist;
using System.Threading.Tasks;

namespace ERP.OMS.Management.Activities
{
    public partial class TPurchaseInvoicelist : VSPage //: System.Web.UI.Page
    {
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        //PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
        TransitPurchaseInvoiceBL objtransitPurchaseInvoice = new TransitPurchaseInvoiceBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        int KeyValue = 0;
        #region Page Load Section Start


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
            DataTable branchtable = objtransitPurchaseInvoice.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            //DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            //if (name.Length > 0)
            //{
            //    //branchName.Text = Convert.ToString(name[0]["branch_description"]);
            //}


        }
        //public void PopulateGridByFilter(string fromdate, string toDate, string branch)
        //{
        //    DataTable dtdata = new DataTable();
        //    dtdata = objtransitPurchaseInvoice.GetTransitInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
        //    //if (dtdata != null)
        //    //{
        //        Session["TPBList"] = dtdata;
        //        GrdQuotation.DataSource = dtdata;
        //        GrdQuotation.DataBind();
        //    //}
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Button Wise Right Access Section Start
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/TPurchaseInvoiceList.aspx");
            #endregion Button Wise Right Access Section End


            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            if (!IsPostBack)
            {
                //GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookies<GridName>";

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookies<GridName>');</script>");

                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                //string prevPage = Request.UrlReferrer.ToString();
                //string[] prepage = prevPage.Split('/');
                //int cnt = prepage.Length;
                //if (prepage[cnt - 1] != "ProjectMainPage.aspx")
                //{
                //    if (Session["TPBFromDate"] != null)
                //    {
                //        string fromdate = Convert.ToString(Session["TPBFromDate"]);
                //        FormDate.Date = Convert.ToDateTime(fromdate);
                //        string todate = Convert.ToString(Session["TPBtoDate"]);
                //        toDate.Date = Convert.ToDateTime(todate);
                //        //toDate
                //        cmbBranchfilter.Value = Convert.ToString(Session["TPBfilteredbranch"]);
                //        PopulateGridByFilter(fromdate, todate, Convert.ToString(cmbBranchfilter.Value));
                //    }
                //    else
                //    {
                        //#region Company and Branch Hierarchy Wise Transit Purchase Invoice Detail List Section Start
                        //if (HttpContext.Current.Session["LastCompany"] != null)
                        //{

                        //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        //    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                        //    {
                        //        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                        //        if (HttpContext.Current.Session["LastFinYear"] != null)
                        //        {
                        //            string lastfinyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                        //            GetPurchaseInvoiceListGridData(userbranch, lastCompany, lastfinyear);
                        //        }

                        //    }
                        //}
                        //#endregion Company and Branch Hierarchy Wise Transit Purchase Invoice Detail List Section End
                //    }
                //}
                //else
                //{
                //    Session["TPBFromDate"] = null;
                //    Session["TPBtoDate"] = null;
                //    Session["TPBfilteredbranch"] = null;
                    //#region Company and Branch Hierarchy Wise Transit Purchase Invoice Detail List Section Start
                    //if (HttpContext.Current.Session["LastCompany"] != null)
                    //{

                    //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    //    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                    //    {
                    //        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    //        if (HttpContext.Current.Session["LastFinYear"] != null)
                    //        {
                    //            string lastfinyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                    //            GetPurchaseInvoiceListGridData(userbranch, lastCompany, lastfinyear);
                    //        }

                    //    }
                    //}
                    //#endregion Company and Branch Hierarchy Wise Transit Purchase Invoice Detail List Section End
                //}
                #region Session Remove Section Start
                Session.Remove("TPendingApproval");
                Session.Remove("TUserWiseERPDocCreation");
                Session.Remove("TSaveModePB");  // Use this session to remove default cursor from manual oe auto schema type
                Session.Remove("TschemavaluePB");// Use this session to remove default cursor from manual oe auto schema type
                Session.Remove("SaveModeTPB");
               
                #endregion Session Remove Section End

                ConditionWiseShowStatusButton();

                #region Is is using in Quotation Page need to remove from this side
                Session.Remove("TSaveMode");
                Session.Remove("Tschemavalue");
                #endregion Is is using in Quotation Page need to remove from this side
                
                
            }
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
            }
            #region Show Count Pending Approval  in Popup grid User Level Wise Start
            Session["Texportval"] = null;
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
        #endregion Page Load Section End

        #region Main Grid Event Section Start
        //public void GetPurchaseInvoiceListGridData(string userbranch, string lastCompany, string lastfinyear)
        //{
        //    DataTable dtdata = new DataTable();
        //    dtdata = objtransitPurchaseInvoice.GetTransitPurchaseInvoiceListGridData(userbranch, lastCompany, lastfinyear);
        //    //if (dtdata != null && dtdata.Rows.Count > 0)
        //    //{
        //        Session["TPBList"] = dtdata;
        //        GrdQuotation.DataSource = dtdata;
        //        GrdQuotation.DataBind();
        //    //}
        //    //else
        //    //{
        //    //    GrdQuotation.DataSource = null;
        //    //    GrdQuotation.DataBind();
        //    //}


        //}
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

                DataTable dtQuotationStatus = objCRMSalesDtlBL.GetQuotationStatusByQuotationID(WhichType);


                if (dtQuotationStatus.Rows.Count > 0 && dtQuotationStatus != null)
                {
                    string quoteid = Convert.ToString(dtQuotationStatus.Rows[0]["quoteid"]);
                    string quoteNumber = Convert.ToString(dtQuotationStatus.Rows[0]["quoteNumber"]);
                    string Status = Convert.ToString(dtQuotationStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtQuotationStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtQuotationStatus.Rows[0]["CustomerName"]);
                    GrdQuotation.JSProperties["cpEdit"] = quoteid + "~"
                                                    + quoteNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            if (WhichCall == "update")
            {
                if (Convert.ToString(e.Parameters).Contains("~"))
                {
                    if (Convert.ToString(e.Parameters).Split('~')[2] != "")
                    {
                        QuoteStatus = Convert.ToString(e.Parameters).Split('~')[2];
                        remarks = Convert.ToString(e.Parameters).Split('~')[3];
                    }
                }
                int dtQuotationStatus = objCRMSalesDtlBL.UpdateQuotationStatusByCustomer(Convert.ToInt32(WhichType), Convert.ToInt32(QuoteStatus), remarks);
                if (dtQuotationStatus == 1)
                {
                    GrdQuotation.JSProperties["cpUpdate"] = "Save Successfully";
                    //GetPurchaseInvoiceListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpUpdate"] = "Save unsuccessful";
                }


            }
            else if (WhichCall == "Delete")
            {
                string UserID = Convert.ToString(Session["userid"]);
                deletecnt = objtransitPurchaseInvoice.DeletePurchaseInvoice(WhichType, UserID);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                    //if (Session["TPBFromDate"] != null)
                    //{
                    //    string fromdate = Convert.ToString(Session["TPBFromDate"]);
                    //    FormDate.Date = Convert.ToDateTime(fromdate);
                    //    string todate = Convert.ToString(Session["TPBtoDate"]);
                    //    toDate.Date = Convert.ToDateTime(todate);
                    //    //toDate
                    //    cmbBranchfilter.Value = Convert.ToString(Session["TPBfilteredbranch"]);
                    //    PopulateGridByFilter(fromdate, todate, Convert.ToString(cmbBranchfilter.Value));
                    //}
                    //else
                    //{
                    //    GetPurchaseInvoiceListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                    //}

                    
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
            else if (WhichCall == "FilterGridByDate")
            {
                //string fromdate = e.Parameters.Split('~')[1];
                //string toDate = e.Parameters.Split('~')[2];
                //string branch = e.Parameters.Split('~')[3];
                //Session["TPBFromDate"] = fromdate;
                //Session["TPBtoDate"] = toDate;
                //Session["TPBfilteredbranch"] = branch;
                //DataTable dtdata = new DataTable();
                //dtdata = objtransitPurchaseInvoice.GetTransitInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
                ////if (dtdata != null)
                ////{
                //    Session["TPBList"] = dtdata;
                //    GrdQuotation.DataSource = dtdata;
                //    GrdQuotation.DataBind();
                //}
            }

        }

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

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

        [WebMethod]
        public static string UpdateEWayBill(string InvoiceID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
        {
            PurchaseInvoiceBL objPurchaseInvoiceBL = new PurchaseInvoiceBL();
            int EWayBill = 0;
            EWayBill = objPurchaseInvoiceBL.UpdateEWayBillTransitPB(InvoiceID, UpdateEWayBill, EWayBillDate, EWayBillValue, TransporterGSTIN, TransporterName, TransportationMode, TransportationDistance,
                TransporterDocNo, TransporterDocDate, VehicleNo, VehicleType);
            return Convert.ToString(EWayBill);
        }

        #endregion Web Method For Child Page Section End

        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["Texportval"] == null)
                {
                    Session["Texportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["Texportval"]) != Filter)
                {
                    Session["Texportval"] = Filter;
                    bindexport(Filter);
                }
                
            }
        }
        public void bindexport(int Filter)
        {
            drdExport.SelectedValue = "0";
            if (GrdQuotation.VisibleRowCount > 0)
            {
                GrdQuotation.Columns[12].Visible = false;
                //GrdQuotation.Columns[13].Visible = false;
                string filename = "Transit Purchase Invoice";
                exporter.FileName = filename;
                //exporter.FileName = "GrdQuotation";

                exporter.PageHeader.Left = "Transit Purchase Invoice";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";

                switch (Filter)
                {
                    case 1:
                        //HttpContext.Current.ApplicationInstance.CompleteRequest 
                        exporter.WritePdfToResponse();
                        //exporter.WritePdfToResponse();
                        break;
                    case 2:
                        exporter.WriteXlsToResponse();
                        //exporter.WriteXlsToResponse();
                        break;
                    case 3:
                        exporter.WriteRtfToResponse();
                        break;
                    case 4:
                        exporter.WriteCsvToResponse();
                        break;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('There is no record to export.');", true);
                return;
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

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "PB");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        gridPendingApproval.DataSource = dtdata;
                        gridPendingApproval.DataBind();
                        Session["TPendingApproval"] = dtdata;
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PB");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["id"]) + ")";
            }
            else
            {
                lblWaiting.Text = "";
            }
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
                Session.Remove("TPendingApproval"); // To Rebind from database due to Grid approvalval functionality
                userid = Convert.ToInt32(Session["userid"]);
                PopulateERPDocApprovalPendingListByUserLevel();
                gridPendingApproval.JSProperties["cpEdit"] = "F";
                Session.Remove("TUserWiseERPDocCreation"); //To Rebind from database due to GridPending approvalval functionality effects this grid
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
            if (Session["TUserWiseERPDocCreation"] == null)
            {
                //dtdata = new DataTable();
                dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PB");
            }
            else
            {
                dtdata = (DataTable)Session["TUserWiseERPDocCreation"];
            }
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridUserWiseQuotation.DataSource = dtdata;
                gridUserWiseQuotation.DataBind();
                Session["TUserWiseERPDocCreation"] = dtdata;
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
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(8, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PB");

            if(j==1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(8, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PB");

            if (k == 1)
            {
                divPendingWaiting.Visible = true;
            }
            else
            {
                divPendingWaiting.Visible = false;
            }



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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PB");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["id"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End


        #region trash Code Section Start
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
        //        Session["TPendingApproval"] = dtdata;
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
        //#region Quotation Status By Client Section Start
        //public void UpdAteQuotationStatus(string chkbox, int keyVal, int userid)
        //{
        //    //int userid = 0;
        //    //userid = Convert.ToInt32(Session["userid"]);
        //    int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);
        //    Session["userlevel"] = userlevel;
        //    Int64 i = -1;
        //    i = objCRMSalesDtlBL.UpdatePendingQuotationListByUserLevel(chkbox, keyVal, userlevel);
        //}

        //#endregion Quotation Status By Client Section End
        #endregion

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
            //    var PInvoice_id = Convert.ToString(e.Parameters.Split('~')[1]);
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

        //protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string NoofCopy = "";
        //    if (selectOriginal.Checked == true)
        //    {
        //        NoofCopy += 1 + ",";
        //    }
        //    if (selectDuplicate.Checked == true)
        //    {
        //        NoofCopy += 2 + ",";
        //    }
        //    if (selectTriplicate.Checked == true)
        //    {
        //        NoofCopy += 3 + ",";
        //    }
        //    SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
        //}

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
              //  BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();


                DataTable dtRptModules = new DataTable();
               
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\Normal";
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
                    CmbDesignName.Items.Add(name, reportValue);
                    //}
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
                    NoofCopy += 4 + ",";
                }
                //if (selectOfficecopy.Checked == true)
                //{
                //    NoofCopy += 5 + ",";
                //}
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }

        protected void propanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {
                PurchaseInvoiceBL objPurchaseInvoiceBL = new PurchaseInvoiceBL();
                DataTable productdt = objPurchaseInvoiceBL.PopulateProductDtlByInvoiceId(Convert.ToString(data[1]));
                grdproduct.DataSource = productdt;
                grdproduct.DataBind();

            }
        }

        protected void GrdQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            //if (Session["TPBList"] != null)
            //{
            //    DataTable TPBdt = (DataTable)Session["TPBList"];
            //    GrdQuotation.DataSource = TPBdt;
            //    GrdQuotation.DataBind();
            //}
            //else
            //{
            //    GetPurchaseInvoiceListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            //}
        }

        protected void FormDate_Init(object sender, EventArgs e)
        {
            FormDate.Date = DateTime.Now;
            //FormDate.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month , DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        protected void toDate_Init(object sender, EventArgs e)
        {
            toDate.Date = DateTime.Now;
        }

        protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        {
            //if (Session["TPBList"] != null)
            //{
            //    DataTable PBdt = (DataTable)Session["TPBList"];
            //    GrdQuotation.DataSource = PBdt;
            //    //GrdQuotation.DataBind();
            //}
        }

        protected void GrdQuotation_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        // Rev 2.0
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
            Task PopulateStockTrialDataTask = new Task(() => GetTPurchaseInvoicedata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetTPurchaseInvoicedata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_TPurchaseInvoice_List", con);
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
        // End of Rev 2.0

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

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
                    // Rev 2.0
                    //if (strBranchID == "0")
                    //{
                    //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_TransitPBLists
                    //            where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                    //            && branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //            && d.user_id == userid
                    //            orderby d.Invoice_Id descending
                    //            select d;

                    //    e.QueryableSource = q;
                    //}
                    //else
                    //{
                    //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_TransitPBLists
                    //            where
                    //            d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate) &&
                    //            branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //            && d.user_id == userid
                    //            orderby d.Invoice_Id descending
                    //            select d;
                    //    e.QueryableSource = q;
                    //}

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.TPurchaseInvoiceLists
                            where d.user_id == userid
                             && d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    // End of Rev 2.0
                }
                else
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    // Rev 2.0
                    //var q = from d in dc.v_TransitPBLists
                    //        where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                    //        && branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //        && d.user_id == userid
                    //        orderby d.Invoice_Id descending
                    //        select d;
                    var q = from d in dc.TPurchaseInvoiceLists
                            where d.branchid == 0
                            && d.user_id == userid
                            && d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    // End of Rev 2.0

                    e.QueryableSource = q;
                    //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_TransitPBLists
                    //        where d.branch == "0"
                    //        select d;
                    //e.QueryableSource = q;
                }
            }
            else
            {
                if (IsFilter == "Y")
                {
                    // Rev 2.0
                    //if (strBranchID == "0")
                    //{
                    //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_TransitPBLists
                    //            where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                    //            && branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //            orderby d.Invoice_Id descending
                    //            select d;

                    //    e.QueryableSource = q;
                    //}
                    //else
                    //{
                    //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //    var q = from d in dc.v_TransitPBLists
                    //            where
                    //            d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate) &&
                    //            branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //            orderby d.Invoice_Id descending
                    //            select d;
                    //    e.QueryableSource = q;
                    //}
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.TPurchaseInvoiceLists
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    // End of Rev 2.0
                }
                else
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    // Rev 2.0
                    //var q = from d in dc.v_TransitPBLists
                    //        where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                    //        && branchidlist.Contains(Convert.ToInt32(d.branchid))
                    //        orderby d.Invoice_Id descending
                    //        select d;
                    var q = from d in dc.TPurchaseInvoiceLists
                            where d.branchid == 0
                            && d.USERID == userid
                            select d;
                    // End of Rev 2.0
                    e.QueryableSource = q;
                    //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_TransitPBLists
                    //        where d.branch == "0"
                    //        select d;
                    //e.QueryableSource = q;
                }
            }
        }

        [WebMethod]
        public static object EditEWayBill(string DocID)
        {
            List<getEwayBill> EWayBill = new List<getEwayBill>();
            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            DataTable dt = new DataTable();
            dt = objSalesInvoiceBL.EditEWayBill(DocID, "TransitPurchaseInvoice");
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
        //Rev 1.0
        [WebMethod]
        public static object EditPartyInvDate(string DocID)
        {
            List<PartyInv> OBjpartyinv = new List<PartyInv>();
            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            DataTable dt = new DataTable();
            dt = objSalesInvoiceBL.EditPartyInvDT(DocID, "TransitPurchaseInvoicePartyInvDate");
            if (dt != null && dt.Rows.Count > 0)
            {
                OBjpartyinv = (from DataRow dr in dt.Rows
                               select new PartyInv()
                               {
                                   PartyInvoiceNo = Convert.ToString(dr["PartyInvoiceNo"]),
                                   PartyInvoiceDate = Convert.ToString(dr["PartyInvoiceDate"])
                               }).ToList();
            }
            return OBjpartyinv;
        }
        [WebMethod]
        public static string UpdatePartyINVDt(string InvoiceID, string PartyInvoiceNo, string PartyInvoiceDate)
        {            
            int PartyInvdt = 0;
            PartyInvdt = UpdatePartyINV(InvoiceID, PartyInvoiceNo, PartyInvoiceDate);
            return Convert.ToString(PartyInvdt);

        }
        public static int UpdatePartyINV(string InvoiceID, string PartyInvValue, string PartyInvDate)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_UpdatePartyInvNoDate");
            proc.AddVarcharPara("@Action", 100, "UpdateTransitPurchaseInvoicePartyINvDT");
            proc.AddBigIntegerPara("@DocId", Convert.ToInt32(InvoiceID));
            proc.AddPara("@PartyInvoiceNo", PartyInvValue);
            proc.AddVarcharPara("@PartyInvoiceDate", 50, PartyInvDate);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        //Rev 1.0 End
    }
}