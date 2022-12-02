using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using OpeningEntry.OpeningEntry.DBML;
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

namespace OpeningEntry.OpeningEntry
{
    public partial class OpeningTSalesInvoiceList_aspx : System.Web.UI.Page
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        TransitSalesInvoiceBL objSalesInvoiceBL = new TransitSalesInvoiceBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            //cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        //public void PopulateGridByFilter(string fromdate, string toDate, string branch)
        //{
        //    DataTable dtdata = new DataTable();
        //    dtdata = objtransitPurchaseInvoice.GetTransitInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
        //    if (dtdata != null)
        //    {
        //        Session["TSBList"] = dtdata;
        //        GrdQuotation.DataSource = dtdata;
        //        GrdQuotation.DataBind();
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/TSalesInvoiceList.aspx");

            if (!IsPostBack)
            {

                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                //GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookiesTransitSalesInvoiceGrdQuotation";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesTransitSalesInvoiceGrdQuotation');</script>");

                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    string finyear = Convert.ToString(Session["LastFinYear"]);
                    PopulateBranchByHierchy(userbranch);
                    //FormDate.Date = DateTime.Now;
                    //toDate.Date = DateTime.Now;
                    DateTime fromDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]);
                    fromDate = fromDate.AddDays(-1);


                    toDate.Date = fromDate;
                    FormDate.Date = fromDate;
                    toDate.MaxDate = fromDate;
                    FormDate.MaxDate = fromDate;
                    // GetQuotationListGridData(userbranch, lastCompany, finyear);

                    DataTable dt = objSalesInvoiceBL.GetTotalDuesData(userbranch, lastCompany);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string strTotalDues = Convert.ToString(dt.Rows[0]["TotalDues"]);

                        if (Convert.ToDecimal(strTotalDues) >= 0)
                        {
                            //lblAvailableDues.Text = strTotalDues + " Db";
                        }
                        else
                        {
                            //lblAvailableDues.Text = strTotalDues + " Cr";
                            //lblAvailableDues.ForeColor = Color.Red;
                        }
                    }
                }

                #region User Wise Document Show

                //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

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
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
            }
            #endregion Sandip Section For Approval Dtl Section End

            //selectOriginal.Checked = false;
            //selectOriginal.Enabled = true;
            //selectDuplicate.Checked = false;
            //selectTriplicate.Checked = false;
            //selectDuplicate.Enabled = false;
            //selectTriplicate.Enabled = false;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }
        [WebMethod]
        public static string UpdateEWayBill(string InvoiceID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue)
        {
            PurchaseInvoiceBL objPurchaseInvoiceBL = new PurchaseInvoiceBL();
            int EWayBill = 0;
            EWayBill = objPurchaseInvoiceBL.UpdateEWayBillTransitSB(InvoiceID, UpdateEWayBill, EWayBillDate, EWayBillValue,"","","0","0","",null,"","");
            return Convert.ToString(EWayBill);
        }
        protected void cgridDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            //string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "BindDocumentsDetails")
            //{
            //    CmbDesignName.SelectedIndex = 0;
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
            //    var InvoiceId = Convert.ToString(e.Parameters.Split('~')[1]);
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

        #region Export Grid Section Start
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    if (Filter != 0)
        //    {
        //        if (Session["exportval"] == null)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //        else //if (Convert.ToInt32(Session["exportval"]) != Filter)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //    }
        //}
        public void bindexport(int Filter)
        {
            GrdQuotation.Columns[7].Visible = false;
            string filename = "Transit Sales Invoice";
            exporter.FileName = filename;
            exporter.FileName = "Transit Sales Invoice";

            exporter.PageHeader.Left = "Transit Sales Invoice";
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

        #region Grid Section Start

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

            if (WhichCall == "Delete")
            {
               // deletecnt = objSalesInvoiceBL.DeleteInvoice(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
            //else if (WhichCall == "FilterGridByDate")
            //{
            //    DateTime fromdate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
            //    DateTime toDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
            //    //string fromdate = e.Parameters.Split('~')[1];
            //    //string toDate = e.Parameters.Split('~')[2];
            //    string branch = e.Parameters.Split('~')[3];
            //    Session["TSIFromDate"] = fromdate;
            //    Session["TSItoDate"] = toDate;
            //    Session["TSIfilteredbranch"] = branch;
            //    DataTable dtdata = new DataTable();
            //    dtdata = objSalesInvoiceBL.GetTransitSalesInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), branch, fromdate, toDate);
            //    if (dtdata != null && dtdata.Rows.Count>0)
            //    {
            //        Session["TSBList"] = dtdata;
            //        GrdQuotation.DataSource = dtdata;
            //        GrdQuotation.DataBind();
            //    }
            //    else
            //    {
            //        Session["TSBList"] = null;
            //        GrdQuotation.DataSource = null;
            //        GrdQuotation.DataBind();
            //    }
            //}
        }
        public void GetQuotationListGridData(string userbranch, string lastCompany, string finyear)
        {
            //DataTable dtdata = new DataTable();
            //dtdata = objSalesInvoiceBL.GetTransitSalesInvoiceListGridData(userbranch, lastCompany, finyear);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    Session["TSBList"] = dtdata;
            //    GrdQuotation.DataSource = dtdata;
            //    GrdQuotation.DataBind();
            //}
            //else
            //{
            //    Session["TSBList"] = null;
            //    GrdQuotation.DataSource = null;
            //    GrdQuotation.DataBind();
            //}
        }
        protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        {
            //if (Session["TSBList"] != null)
            //{
            //    DataTable dtdata = (DataTable)Session["TSBList"]; 
            //    GrdQuotation.DataSource = dtdata;

            //}
            //else
            //{
            //    Session["TSBList"] = null;
            //    GrdQuotation.DataSource = null; 
            //}

            //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //string finyear = Convert.ToString(Session["LastFinYear"]);

            //DataTable dtdata = new DataTable();
            //dtdata = objSalesInvoiceBL.GetTransitSalesInvoiceListGridData(userbranch, lastCompany, finyear);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    GrdQuotation.DataSource = dtdata;
            //}
            //else
            //{
            //    GrdQuotation.DataSource = null;
            //}
            //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //string finyear = Convert.ToString(Session["LastFinYear"]);
            //string BranchID = Convert.ToString(cmbBranchfilter.Value);
            //DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            //DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            //DataTable dtdata = new DataTable();
            //dtdata = objSalesInvoiceBL.GetTransitSalesInvoiceListGridDataByDate(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    GrdQuotation.DataSource = dtdata;
            //}
            //else
            //{
            //    GrdQuotation.DataSource = null;
            //}
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

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "SI");
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SI");
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

            dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "SI");
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

            i = objERPDocPendingApproval.ConditionWiseShowStatusButton(4, branchid, Convert.ToString(Session["userid"]));  // 4 for Sale Invoice Module 
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
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "SI");
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
                //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtRptModules = new DataTable();
                //string query = "";
                //query = @"Select Design_Fullpath from tbl_trans_SetDefaultDesign_Report WHERE Module_Type='SI' order by Module_Id ";
                //dtRptModules = oDbEngine.GetDataTable(query);
                //if (dtRptModules.Rows.Count > 1)
                //{
                //    string Savereportname = Path.GetFileNameWithoutExtension(Convert.ToString(dtRptModules.Rows[0][0]));
                //    string Rname = "";
                //    if (Savereportname.Split('~').Length > 1)
                //    {
                //        Rname = Savereportname.Split('~')[0];
                //    }
                //    else
                //    {
                //        Rname = Savereportname;
                //    }
                //    string SavereportValue = Savereportname;
                //    CmbDesignName.Items.Add(Rname, SavereportValue);
                //}                

                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit";
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
                if (selectOfficecopy.Checked == true)
                {
                    NoofCopy += 5 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }




        #endregion Sandip Section For Approval Dtl Section End
        protected void FormDate_Init(object sender, EventArgs e)
        {
            FormDate.Date = DateTime.Now;
            //FormDate.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month , DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        protected void toDate_Init(object sender, EventArgs e)
        {
            toDate.Date = DateTime.Now;
        }

        protected void GrdQuotation_PageIndexChanged(object sender, EventArgs e)
        {

        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);
            string IsUserwise = Convert.ToString(hfIsUserwise.Value);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    if (IsUserwise == "Y")
                    {
                        OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                        var q = from d in dc.v_TransitSBLists
                                where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.branchid))
                                && d.CreatedBy == userid
                                orderby d.Invoice_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                        var q = from d in dc.v_TransitSBLists
                                where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.branchid))
                                orderby d.Invoice_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    if (IsUserwise == "Y")
                    {
                        OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                        var q = from d in dc.v_TransitSBLists
                                where
                                d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate) &&
                                branchidlist.Contains(Convert.ToInt32(d.branchid))
                                 && d.CreatedBy == userid
                                orderby d.Invoice_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                        var q = from d in dc.v_TransitSBLists
                                where
                                d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate) &&
                                branchidlist.Contains(Convert.ToInt32(d.branchid))
                                orderby d.Invoice_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
            }
            else
            {
                string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                var q = from d in dc.v_TransitSBLists
                        where d.InvoiceDate >= Convert.ToDateTime(strFromDate) && d.InvoiceDate <= Convert.ToDateTime(strToDate)
                        && branchidlist.Contains(Convert.ToInt32(d.branchid))
                        orderby d.Invoice_Id descending
                        select d;

                e.QueryableSource = q;
                //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_TransitSBLists
                //        where d.branchid == 0
                //        select d;
                //e.QueryableSource = q;
            }
        }

    }
}
