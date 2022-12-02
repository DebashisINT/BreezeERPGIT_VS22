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

namespace ERP.OMS.Management.Activities
{
    public partial class AdjustmentOfPendingDocuments : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        AdjustmentPendingDocumentBL objPurchaseInvoice = new AdjustmentPendingDocumentBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        #region Page Load Section Start

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            AdjustmentPendingDocumentBL objAdjustmentPendingDocumentBL = new AdjustmentPendingDocumentBL(); 
            DataTable branchtable = objAdjustmentPendingDocumentBL.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            if (name.Length > 0)
            {
                //branchName.Text = Convert.ToString(name[0]["branch_description"]);
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

        public void PopulateGridByFilter(string fromdate, string toDate, string branch, string status, string type)
        {
            DataTable dtdata = new DataTable();
            dtdata = objPurchaseInvoice.PopulatePendingDocumentForAdjustmentByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch, status, type);
            
            //if (dtdata != null)
            //{
                Session["AdjustList"] = dtdata;
                GrdQuotation.DataSource = dtdata;
                GrdQuotation.DataBind();
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Button Wise Right Access Section Start
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/AdjustmentOfPendingDocuments.aspx");
            #endregion Button Wise Right Access Section End

            if (!IsPostBack)
            {
                string status = rdl_Adjustment.SelectedValue;
                string type = Convert.ToString(ddl_type.Value);
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                //Session["AdjustFromDate"] = fromdate;
                //Session["AdjusttoDate"] = toDate;
                //Session["Adjustfilteredbranch"] = branch;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.UrlReferrer)))
                {
                    string prevPage = Request.UrlReferrer.ToString();
                    string[] prepage = prevPage.Split('/');
                    int cnt = prepage.Length;
                    if (prepage[cnt - 1] != "ProjectMainPage.aspx")
                    {
                        if (Session["AdjustFromDate"] != null)
                        {
                            string fromdate = Convert.ToString(Session["AdjustFromDate"]);
                            FormDate.Date = Convert.ToDateTime(fromdate);
                            string todate = Convert.ToString(Session["AdjusttoDate"]);
                            toDate.Date = Convert.ToDateTime(todate);

                            //toDate
                            cmbBranchfilter.Value = Convert.ToString(Session["Adjustfilteredbranch"]);
                            status = Convert.ToString(Session["AdjustStatus"]);
                            PopulateGridByFilter(fromdate, todate, Convert.ToString(cmbBranchfilter.Value), status, type);
                        }
                        else
                        {
                            
                            #region Company and Branch Hierarchy Wise Quotation Detail List Section Start
                            if (HttpContext.Current.Session["LastCompany"] != null)
                            {

                                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                                if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                                {
                                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                                    if (HttpContext.Current.Session["LastFinYear"] != null)
                                    {
                                        string lastfinyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                                        PopulatePendingDocumentForAdjustment(userbranch, lastCompany, lastfinyear, status, type);
                                    }

                                }
                            }
                            #endregion Company and Branch Hierarchy Wise Quotation Detail List Section End
                        }
                    }
                    else
                    {
                        Session["AdjustFromDate"] = null;
                        Session["AdjusttoDate"] = null;
                        Session["Adjustfilteredbranch"] = null;
                        #region Company and Branch Hierarchy Wise Quotation Detail List Section Start
                        if (HttpContext.Current.Session["LastCompany"] != null)
                        {

                            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                            if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                            {
                                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                                if (HttpContext.Current.Session["LastFinYear"] != null)
                                {
                                    string lastfinyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                                    PopulatePendingDocumentForAdjustment(userbranch, lastCompany, lastfinyear, status, type);
                                }

                            }
                        }
                        #endregion Company and Branch Hierarchy Wise Quotation Detail List Section End
                    }
                }

                #region Session Remove Section Start
                //Session.Remove("PendingApproval");
                //Session.Remove("UserWiseERPDocCreation");
                //Session.Remove("SaveModePB");  // Use this session to remove default cursor from manual oe auto schema type
                //Session.Remove("schemavaluePB"); // Use this session to remove default cursor from manual oe auto schema type
               
                #endregion Session Remove Section End

                //ConditionWiseShowStatusButton();

                #region Is is using in Quotation Page need to remove from this side
                //Session.Remove("SaveMode");
                //Session.Remove("schemavalue");
                #endregion Is is using in Quotation Page need to remove from this side
                
            }
            //if (divPendingWaiting.Visible == true)
            //{
            //    PopulateUserWiseERPDocCreation();
            //    PopulateApprovalPendingCountByUserLevel();
            //    PopulateERPDocApprovalPendingListByUserLevel();
            //}

            #region Show Count Pending Approval  in Popup grid User Level Wise Start
            //Session["exportval"] = null;
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
        public void PopulatePendingDocumentForAdjustment(string userbranch, string lastCompany, string lastfinyear,string status,string type )
        {
            DataTable dtdata = new DataTable();
            dtdata = objPurchaseInvoice.PopulatePendingDocumentForAdjustment(userbranch, lastCompany, lastfinyear, status, type);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
                Session["AdjustList"] = dtdata;
                GrdQuotation.DataSource = dtdata;
                GrdQuotation.DataBind();
            //}
            //else
            //{
            //    GrdQuotation.DataSource = null;
            //    GrdQuotation.DataBind();
            //}


        }
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
            //if (WhichCall == "update")
            //{
            //    if (Convert.ToString(e.Parameters).Contains("~"))
            //    {
            //        if (Convert.ToString(e.Parameters).Split('~')[2] != "")
            //        {
            //            QuoteStatus = Convert.ToString(e.Parameters).Split('~')[2];
            //            remarks = Convert.ToString(e.Parameters).Split('~')[3];
            //        }
            //    }
            //    int dtQuotationStatus = objCRMSalesDtlBL.UpdateQuotationStatusByCustomer(Convert.ToInt32(WhichType), Convert.ToInt32(QuoteStatus), remarks);
            //    if (dtQuotationStatus == 1)
            //    {
            //        GrdQuotation.JSProperties["cpUpdate"] = "Save Successfully";
            //       PopulatePendingDocumentForAdjustment(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            //    }
            //    else
            //    {
            //        GrdQuotation.JSProperties["cpUpdate"] = "Save unsuccessful";
            //    }


            //}
           //else if (WhichCall == "Delete")
           // {
           //     deletecnt = objPurchaseInvoice.DeletePurchaseInvoice(WhichType);
           //     if (deletecnt == 1)
           //     {
           //         GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
           //         if (Session["AdjustFromDate"] != null)
           //         {
           //             string fromdate = Convert.ToString(Session["AdjustFromDate"]);
           //             FormDate.Date = Convert.ToDateTime(fromdate);
           //             string todate = Convert.ToString(Session["AdjusttoDate"]);
           //             toDate.Date = Convert.ToDateTime(todate);
           //             //toDate
           //             cmbBranchfilter.Value = Convert.ToString(Session["Adjustfilteredbranch"]);
           //             PopulateGridByFilter(fromdate, todate, Convert.ToString(cmbBranchfilter.Value));
           //         }
           //         else
           //         {
           //            PopulatePendingDocumentForAdjustment(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           //         }
                    
           //     }
           //     else
           //     {
           //         GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
           //     }

           // }
            else if (WhichCall == "FilterGridByDate")
            {
                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];
                string status = e.Parameters.Split('~')[4];
                string type = Convert.ToString(ddl_type.Value);
                Session["AdjustFromDate"] = fromdate;
                Session["AdjusttoDate"] = toDate;
                Session["Adjustfilteredbranch"] = branch;
                Session["AdjustStatus"] = status;
                DataTable dtdata = new DataTable();
                dtdata = objPurchaseInvoice.PopulatePendingDocumentForAdjustmentByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch, status, type);
                //if (dtdata != null)
                //{
                Session["AdjustList"] = dtdata;
                GrdQuotation.DataSource = dtdata;
                GrdQuotation.DataBind();
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

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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



        #endregion Web Method For Child Page Section End

        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    if (Session["exportval"] == null)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //}
        }
         public void bindexport(int Filter)
        {
            if (GrdQuotation.VisibleRowCount > 0)
            {
                //GrdQuotation.Columns[9].Visible = false;
                //GrdQuotation.Columns[10].Visible = false;
                //GrdQuotation.Columns[11].Visible = false;
                GrdQuotation.Columns[12].Visible = false;
                GrdQuotation.Columns[13].Visible = false;
                string filename = "Purchase Invoice";
                exporter.FileName = filename;
                //exporter.FileName = "PurchaseInvoice";

                exporter.PageHeader.Left = "Purchase Invoice";
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PB");
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["id"]) + ")";
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
            if (Session["UserWiseERPDocCreation"] == null)
            {
                //dtdata = new DataTable();
                dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PB");
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
        #endregion #region Created User Wise List Quotation after Clicking on Status Button Section End


        #region To Show Hide Status and Pending Approval Button Configuration Wise Start
        public void ConditionWiseShowStatusButton()
        {
            //int i = 0;
            //int j = 0;
            //int k = 0;
            //int branchid = 0;
            //if (Session["userbranchID"] != null)
            //{
            //    branchid = Convert.ToInt32(Session["userbranchID"]);
            //}
            ////Session["userbranchHierarchy"])

            #region Sam Section For Showing Status and Approval waiting Button on 22052017
            //j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(8, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PB");

            //if(j==1)
            //{
            //    spanStatus.Visible = true;
            //}
            //else
            //{
            //    spanStatus.Visible = false;
            //}


            //k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(8, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PB");

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

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {            
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtRptModules = new DataTable();
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseInvoice\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseInvoice\DocDesign\Normal";
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

        protected void propanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Show")
            {

                AdjustmentPendingDocumentBL objAdjustmentPendingDocumentBL = new AdjustmentPendingDocumentBL();
                
                string csutven=Convert.ToString(ddl_type.Value);
                DataTable custvendt = new DataTable();
                custvendt = objAdjustmentPendingDocumentBL.PopulateCustVendByCondition(csutven);
                if (custvendt.Rows.Count > 0)
                {
                    ddl_custVend.TextField = "Name";
                    ddl_custVend.ValueField = "cnt_internalid";
                    ddl_custVend.DataSource = custvendt;
                    ddl_custVend.DataBind();
                }
                string branchid = "";
                string custvenId = "";
                //PurchaseInvoiceBL objPurchaseInvoiceBL = new PurchaseInvoiceBL();
                DataSet docdtldt = objAdjustmentPendingDocumentBL.PopulateAdjustmentDocDtlByDocIdandDocType(Convert.ToString(data[1]), Convert.ToString(data[2]));
                if (docdtldt.Tables[0].Rows.Count>0)
                {
                    txt_docno.Text = Convert.ToString(docdtldt.Tables[0].Rows[0]["DocNo"]);
                    string Invoice_Date = Convert.ToString(docdtldt.Tables[0].Rows[0]["Docdate"]);
                    dt_docdate.Date = Convert.ToDateTime(Invoice_Date);
                    txt_docbranch.Text = Convert.ToString(docdtldt.Tables[0].Rows[0]["branchname"]);
                    txt_AdjusttedAmt.Text = Convert.ToString(data[3]);
                    ddl_custVend.Value=Convert.ToString(docdtldt.Tables[0].Rows[0]["CustVendId"]);
                     custvenId=Convert.ToString(docdtldt.Tables[0].Rows[0]["CustVendId"]);
                     branchid = Convert.ToString(docdtldt.Tables[0].Rows[0]["branchid"]);
                    

                }
                string Doctype = Convert.ToString(ddl_Document.Value);
                DataTable adjustdocdt = new DataTable();
                
                if (csutven == "0")
                {
                    lbltype.InnerText = "Customer";
                }
                else
                {
                    lbltype.InnerText = "Vendor";
                    adjustdocdt = objPurchaseInvoice.PopulateDocumentForAdjustmentByVendorid(branchid, Convert.ToString(HttpContext.Current.Session["LastCompany"]),  Doctype, custvenId);
                    aspxGridTax.DataSource = adjustdocdt;
                    aspxGridTax.DataBind();
                }


            }
        }

        protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["AdjustList"] != null)
            {
                DataTable PBdt = (DataTable)Session["AdjustList"];
                GrdQuotation.DataSource = PBdt;
                //GrdQuotation.DataBind();
            }
        }

        protected void GrdQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            if (Session["AdjustList"] != null)
            {
                DataTable PBdt = (DataTable)Session["AdjustList"];
                GrdQuotation.DataSource = PBdt;
                GrdQuotation.DataBind();
            }
        }

        #region Default date Section
        protected void FormDate_Init(object sender, EventArgs e)
        {
            FormDate.Date = DateTime.Now;
            //FormDate.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month , DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        protected void toDate_Init(object sender, EventArgs e)
        {
            toDate.Date = DateTime.Now;
        }

        #endregion Default date Section

        protected void GrdQuotation_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


         
         #region pending  document Grid for Adjustment
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            //int slNo = Convert.ToInt32(HdSerialNo.Value);
            //DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
            //foreach (var args in e.UpdateValues)
            //{

            //    string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //    decimal Percentage = 0;

            //    Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);

            //    decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
            //    string TaxCode = "0";
            //    if (!Convert.ToString(args.Keys[0]).Contains('~'))
            //    {
            //        TaxCode = Convert.ToString(args.Keys[0]);
            //    }



            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Percentage;
                     
            //        finalRow[0]["Amount"] = Amount;

            //        finalRow[0]["TaxCode"] = args.Keys[0];
            //        finalRow[0]["AltTaxCode"] = "0";

            //    }
            //    else
            //    {
            //        DataRow newRow = TaxRecord.NewRow();
            //        newRow["slNo"] = slNo;
            //        newRow["Percentage"] = Percentage;
            //        newRow["TaxCode"] = TaxCode;
            //        newRow["AltTaxCode"] = "0";
            //        newRow["Amount"] = Amount;
            //        TaxRecord.Rows.Add(newRow);
            //    }


            //}

            
            //if (cmbGstCstVat.Value != null)
            //{

            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        finalRow[0]["Amount"] = txtGstCstVat.Text;
            //        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

            //    }
            //    else
            //    {
            //        DataRow newRowGST = TaxRecord.NewRow();
            //        newRowGST["slNo"] = slNo;
            //        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        newRowGST["TaxCode"] = "0";
            //        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //        newRowGST["Amount"] = txtGstCstVat.Text;
            //        TaxRecord.Rows.Add(newRowGST);
            //    }
            //}
            


            //Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;


           

        }

         protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //string retMsg = "";
            //if (e.Parameters.Split('~')[0] == "SaveGST")
            //{
            //    DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
            //    int slNo = Convert.ToInt32(HdSerialNo.Value);
            //    //For GST/CST/VAT
            //    if (cmbGstCstVat.Value != null)
            //    {

            //        DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
            //        if (finalRow.Length > 0)
            //        {
            //            finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //            finalRow[0]["Amount"] = txtGstCstVat.Text;
            //            finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

            //        }
            //        else
            //        {
            //            DataRow newRowGST = TaxRecord.NewRow();
            //            newRowGST["slNo"] = slNo;
            //            newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //            newRowGST["TaxCode"] = "0";
            //            newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //            newRowGST["Amount"] = txtGstCstVat.Text;
            //            TaxRecord.Rows.Add(newRowGST);
            //        }
            //    }
            //    //End Here

            //    aspxGridTax.JSProperties["cpUpdated"] = "";

            //    Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
            //}
            //else
            //{
            //    #region fetch All data For Tax

            //    DataTable taxDetail = new DataTable();
            //    DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
            //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            //    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
            //    proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            //    proc.AddVarcharPara("@ProductsID", 10, Convert.ToString(setCurrentProdCode.Value));
            //    taxDetail = proc.GetTable();

            //    //Get Company Gstin 09032017
            //    string CompInternalId = Convert.ToString(Session["LastCompany"]);
            //    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

            //    //Get BranchStateCode
            //    string BrancgStateCode = "", BranchGSTIN = "";
            //    DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
            //    if (BranchTable != null)
            //    {
            //        BrancgStateCode = Convert.ToString(BranchTable.Rows[0][0]);
            //        BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
            //        if (BranchGSTIN.Trim() != "")
            //        {
            //            BrancgStateCode = BranchGSTIN.Substring(0, 2);
            //        }
            //    }

            //    if (BranchGSTIN.Trim() == "")
            //    {
            //        BrancgStateCode = compGstin[0].Substring(0, 2);
            //    }



            //    string VendorState = "";
                 

            //    ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
            //    GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
            //    GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
            //    GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(lookup_Customer.Value));
            //    DataTable VendorGstin = GetVendorGstin.GetTable();

            //    if(VendorGstin.Rows.Count>0)
            //    {
            //        if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
            //        {
            //            VendorState = Convert.ToString(VendorGstin.Rows[0][0]).Substring(0, 2);
            //        }
                
            //    }


            //    #endregion


            //    if (VendorState.Trim() != "" && BrancgStateCode != "")
            //    {

            //        if (BrancgStateCode == VendorState)
            //        {
                         
            //            if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "35" || VendorState == "31" || VendorState == "34")
            //            {
            //                foreach (DataRow dr in taxDetail.Rows)
            //                {
            //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
            //                    {
            //                        dr.Delete();
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                foreach (DataRow dr in taxDetail.Rows)
            //                {
            //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
            //                    {
            //                        dr.Delete();
            //                    }
            //                }
            //            }
            //            taxDetail.AcceptChanges();
            //        }
            //        else
            //        {
            //            foreach (DataRow dr in taxDetail.Rows)
            //            {
            //                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
            //                {
            //                    dr.Delete();
            //                }
            //            }
            //            taxDetail.AcceptChanges();

            //        }


            //    }

            //    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
            //    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
            //    {
            //        foreach (DataRow dr in taxDetail.Rows)
            //        {
            //            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
            //            {
            //                dr.Delete();
            //            }
            //        }
            //        taxDetail.AcceptChanges();
            //    }

                 



            //    int slNo = Convert.ToInt32(HdSerialNo.Value);

            //    //Get Gross Amount and Net Amount 
            //    decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
            //    decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);



            //    List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();

            //    //Debjyoti 09032017
            //    decimal totalParcentage = 0;
            //    foreach (DataRow dr in taxDetail.Rows)
            //    {
            //        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
            //        {
            //            totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
            //        }
            //    }



            //    if (e.Parameters.Split('~')[0] == "New")
            //    {
            //        foreach (DataRow dr in taxDetail.Rows)
            //        {
            //            TaxDetails obj = new TaxDetails();
            //            obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
            //            obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
            //            obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
            //            obj.Amount = 0.0;

            //            #region set calculated on
            //            //Check Tax Applicable on and set to calculated on
            //            if (Convert.ToString(dr["ApplicableOn"]) == "G")
            //            {
            //                obj.calCulatedOn = ProdGrossAmt;
            //            }
            //            else if (Convert.ToString(dr["ApplicableOn"]) == "N")
            //            {
            //                obj.calCulatedOn = ProdNetAmt;
            //            }
            //            else
            //            {
            //                obj.calCulatedOn = 0;
            //            }
            //            //End Here
            //            #endregion

            //            //Debjyoti 09032017
            //            if (Convert.ToString(ddl_AmountAre.Value) == "2")
            //            {
            //                if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
            //                {
            //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
            //                    {
            //                        decimal finalCalCulatedOn = 0;
            //                        decimal backProcessRate = (1 + (totalParcentage / 100));
            //                        finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
            //                        obj.calCulatedOn = finalCalCulatedOn;
            //                    }
            //                }
            //            }

            //            if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
            //            {
            //                obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

            //            }
            //            else
            //            {
            //                obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
            //            }

            //            obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));




            //            DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
            //            if (filtr.Length > 0)
            //            {
            //                obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
            //                if (obj.Taxes_ID == 0)
            //                {
            //                    //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
            //                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
            //                }
            //                else
            //                    obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
            //            }

            //            TaxDetailsDetails.Add(obj);
            //        }
            //    }
            //    else
            //    {
            //        string keyValue = e.Parameters.Split('~')[0];

            //        DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];


            //        foreach (DataRow dr in taxDetail.Rows)
            //        {
            //            TaxDetails obj = new TaxDetails();
            //            obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
            //            obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

            //            if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
            //                obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
            //            else
            //                obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
            //            obj.TaxField = "";
            //            obj.Amount = 0.0;

            //            #region set calculated on
            //            //Check Tax Applicable on and set to calculated on
            //            if (Convert.ToString(dr["ApplicableOn"]) == "G")
            //            {
            //                obj.calCulatedOn = ProdGrossAmt;
            //            }
            //            else if (Convert.ToString(dr["ApplicableOn"]) == "N")
            //            {
            //                obj.calCulatedOn = ProdNetAmt;
            //            }
            //            else
            //            {
            //                obj.calCulatedOn = 0;
            //            }
            //            //End Here
            //            #endregion

            //            //Debjyoti 09032017
            //            if (Convert.ToString(ddl_AmountAre.Value) == "2")
            //            {
            //                if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
            //                {
            //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
            //                    {
            //                        decimal finalCalCulatedOn = 0;
            //                        decimal backProcessRate = (1 + (totalParcentage / 100));
            //                        finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
            //                        obj.calCulatedOn =finalCalCulatedOn;
            //                    }
            //                }
            //            }

            //            DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
            //            if (filtronexsisting1.Length > 0)
            //            {
            //                if (obj.Taxes_ID == 0)
            //                {
            //                    obj.TaxField = "0";
            //                }
            //                else
            //                {
            //                    obj.TaxField = Convert.ToString(filtronexsisting1[0]["Percentage"]);
            //                }
            //                obj.Amount = Convert.ToDouble(filtronexsisting1[0]["Amount"]);
            //            }
            //            else
            //            {
                            


            //                DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
            //                if (filtronexsisting.Length > 0)
            //                {
            //                    DataRow taxRecordNewRow = TaxRecord.NewRow();
            //                    taxRecordNewRow["SlNo"] = slNo;
            //                    taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
            //                    taxRecordNewRow["AltTaxCode"] = "0";
            //                    taxRecordNewRow["Percentage"] = 0.0;
            //                    taxRecordNewRow["Amount"] = "0";

            //                    TaxRecord.Rows.Add(taxRecordNewRow);
            //                }

            //            }
            //            TaxDetailsDetails.Add(obj);

                         
            //            DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
            //            if (filtrIndex.Length > 0)
            //            {
            //                aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
            //            }
            //        }
            //        Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;

            //    }
            //    //New Changes 170217
            //    //GstCode should fetch everytime
            //    DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
            //    if (finalFiltrIndex.Length > 0)
            //    {
            //        aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
            //    }

            //    aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);

            //    retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
            //    aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

            //    TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
            //    aspxGridTax.DataSource = TaxDetailsDetails;
            //    aspxGridTax.DataBind();

            //    #endregion
            //}
        }

         protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "type")
            {
                e.Editor.Enabled = true;
                //e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "docno")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "docdate")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "branch")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "balanceAmt")
            {
                e.Editor.Enabled = true;
            }
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.Enabled = true;
            //}
            else if (e.Column.FieldName == "TotalAmount")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }

            
            
        }

        protected void taxgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
    
        
        #endregion pending  document Grid for Adjustment

        protected void aspxGridTax_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }



    }
}