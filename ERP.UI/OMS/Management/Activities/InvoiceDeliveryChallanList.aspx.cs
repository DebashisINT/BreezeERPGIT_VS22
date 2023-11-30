//========================================================== Revision History ============================================================================================
// 1.0   Priti V2.0.41   28/11/2023        0027028: Customer code column is required in the listing module of Sales entry
//========================================== End Revision History =======================================================================================================--%>


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
using ERP.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.OMS.Management.Activities
{
    public partial class InvoiceDeliveryChallanList : System.Web.UI.Page
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/InvoiceDeliveryChallanList.aspx");

            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    //REV 1.0
                    //GrdQuotation.Columns[10].Visible = true;
                    GrdQuotation.Columns[11].Visible = true;
                    //REV 1.0 End

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    //REV 1.0
                    //GrdQuotation.Columns[10].Visible = false;
                    GrdQuotation.Columns[11].Visible = false;
                    //REV 1.0 End
                }
            }


            MasterSettings objmaster = new MasterSettings();
            hdnActiveEInvoice.Value = objmaster.GetSettings("ActiveEInvoice");

            if (hdnActiveEInvoice.Value == "0")
            {
                //Rev 1.0
                //GrdQuotation.Columns[19].Visible = false;
                GrdQuotation.Columns[20].Visible = false;
                //Rev 1.0 End
            }
            else if (hdnActiveEInvoice.Value == "1")
            {
                //Rev 1.0
                //GrdQuotation.Columns[19].Visible = true;
                GrdQuotation.Columns[20].Visible = true;
                //Rev 1.0 End
            }

            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=11");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=11");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "DATA is Freezed between   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + " for Edit. ";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                spnDeleteLock.InnerText = spnEditLock.InnerText + "DATA is Freezed between   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + "  for Delete.";
                spnEditLock.InnerText = "";
            }
            if (!IsPostBack)
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




                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End

                GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesInvoiceGrdQuotation";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesInvoiceGrdQuotation');</script>");

                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    string finyear = Convert.ToString(Session["LastFinYear"]);

                    PopulateBranchByHierchy(userbranch);
                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;

                    //GetQuotationListGridData(userbranch, lastCompany, finyear);
                    BindApprovedSO();

                    //DataTable dt = objSalesInvoiceBL.GetTotalDuesData(userbranch, lastCompany);
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    string strTotalDues = Convert.ToString(dt.Rows[0]["TotalDues"]);

                    //    if (Convert.ToDecimal(strTotalDues) >= 0)
                    //    {
                    //        lblAvailableDues.Text = strTotalDues + " Db";
                    //    }
                    //    else
                    //    {
                    //        lblAvailableDues.Text = strTotalDues + " Cr";
                    //        lblAvailableDues.ForeColor = Color.Red;
                    //    }
                    //}
                }
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

        protected void Page_Init(object sender, EventArgs e)
        {
            dsPortCode.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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
            GrdQuotation.Columns[6].Visible = false;
            string filename = "Sales Invoice cum Challan";
            exporter.FileName = filename;
            exporter.FileName = "GrdSalesinvoice";

            exporter.PageHeader.Left = "Sales Invoice";
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
                deletecnt = objSalesInvoiceBL.DeleteInvoice(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully.";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
                }
                else if (deletecnt == -99)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module can not delete.";
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Problem in Deleting. Sorry for Inconvenience";
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
                dtdata = objSalesInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "CL");
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
        //public void GetQuotationListGridData(string userbranch, string lastCompany, string finyear)
        //{
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

        //    DataTable dtdata = new DataTable();
        //    dtdata = objSalesInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
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
        //    dtdata = objSalesInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        GrdQuotation.DataSource = dtdata;
        //    }
        //    else
        //    {
        //        GrdQuotation.DataSource = null;
        //    }
        //}

        #endregion

        #region Approval Section Start

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

            int k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(2, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "SO");

            if (k == 1)
            {
                span1.Visible = true;
            }
            else
            {
                span1.Visible = false;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal";
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
                //SelectPanel.JSProperties["cpSuccessPath"] = DesignFullPath;
                //HttpContext.Current.Response.Redirect(DesignFullPath + "?Previewrpt=" + reportName);
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

        #endregion Sandip Section For Approval Dtl Section End

        #region Approved Sales Order Section

        protected void gridApprovedSO_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            BindApprovedSO();
        }
        protected void gridApprovedSO_DataBinding(object sender, EventArgs e)
        {
            DataTable dtdata = PopulateApprovedSO();
            gridApprovedSO.DataSource = dtdata;
        }
        public void BindApprovedSO()
        {
            DataTable dtdata = PopulateApprovedSO();
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridApprovedSO.DataSource = dtdata;
                gridApprovedSO.DataBind();
            }
            else
            {
                gridApprovedSO.DataSource = null;
                gridApprovedSO.DataBind();
            }
        }
        public DataTable PopulateApprovedSO()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetApprovedSO");
            proc.AddVarcharPara("@BranchList", 3000, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
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
        public static string UpdateEWayBill(string InvoiceID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
        {
            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            int EWayBill = 0;
            EWayBill = objSalesInvoiceBL.UpdateEWayBill(InvoiceID, UpdateEWayBill, EWayBillDate, EWayBillValue, TransporterGSTIN, TransporterName, TransportationMode, TransportationDistance,
                TransporterDocNo, TransporterDocDate, VehicleNo, VehicleType);
            return Convert.ToString(EWayBill);
        }

        [WebMethod]
        public static object UpdateShipBillDet(string actionname, string ShippingBill_Date, string ShippingBill_Number, string ShippingBill_PortId, string ShippingBill_InvoiceId)
        {
            DateTime? shipdate;
            if (ShippingBill_Date != "")
            {
                shipdate = Convert.ToDateTime(ShippingBill_Date);
            }
            else
            {
                shipdate = null;
            }

            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            int Bill = 0;

            Bill = objSalesInvoiceBL.UpdateShipBill(actionname, shipdate, ShippingBill_Number, ShippingBill_PortId, ShippingBill_InvoiceId, Convert.ToString(HttpContext.Current.Session["userid"]));
            return Convert.ToString(Bill);
        }


        #endregion

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "INVOICE_ID";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string CompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
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
                        //var q = from d in dc.V_InvoiceChallanLISTs
                        //        where d.INVOICE_DATE >= Convert.ToDateTime(strFromDate) && d.INVOICE_DATE <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BRANCHID)) && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID
                        //        && d.FINYEAR == FinYear
                        //        && d.CREATEDBY == userid
                        //        orderby d.INVOICE_DATE descending
                        //        select d;
                        var q = from d in dc.InvoiceChallanLists
                                where d.USERID == userid && d.CREATEDBY == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.V_InvoiceChallanLISTs
                        //        where
                        //        d.INVOICE_DATE >= Convert.ToDateTime(strFromDate) && d.INVOICE_DATE <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BRANCHID)) && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID
                        //        && d.FINYEAR == FinYear
                        //        && d.CREATEDBY == userid
                        //        orderby d.INVOICE_DATE descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.InvoiceChallanLists
                                where d.USERID == userid && d.CREATEDBY == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.V_InvoiceChallanLISTs
                    //        where Convert.ToString(d.INVOICE_ID) == "-1" && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID && d.FINYEAR == FinYear
                    //        && d.CREATEDBY == userid
                    //        orderby d.INVOICE_DATE descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.InvoiceChallanLists
                            where d.SEQ == 0
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
                        //var q = from d in dc.V_InvoiceChallanLISTs
                        //        where d.INVOICE_DATE >= Convert.ToDateTime(strFromDate) && d.INVOICE_DATE <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BRANCHID)) && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID
                        //        && d.FINYEAR == FinYear
                        //        orderby d.INVOICE_DATE descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.InvoiceChallanLists
                                where d.USERID == userid 
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        //var q = from d in dc.V_InvoiceChallanLISTs
                        //        where
                        //        d.INVOICE_DATE >= Convert.ToDateTime(strFromDate) && d.INVOICE_DATE <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.BRANCHID)) && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID
                        //        && d.FINYEAR == FinYear
                        //        orderby d.INVOICE_DATE descending
                        //        select d;
                        //e.QueryableSource = q;

                        var q = from d in dc.InvoiceChallanLists
                                where d.USERID == userid
                                orderby d.SEQ descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.V_InvoiceChallanLISTs
                    //        where Convert.ToString(d.INVOICE_ID) == "-1" && d.INVOICEFOR == "CL" && d.COMPANYID == CompanyID && d.FINYEAR == FinYear
                    //        orderby d.INVOICE_DATE descending
                    //        select d;
                    //e.QueryableSource = q;

                    var q = from d in dc.InvoiceChallanLists
                            where d.SEQ == 0
                            select d;
                    e.QueryableSource = q;
                }
            }
        }

        [WebMethod]
        public static object EditEWayBill(string DocID)
        {
            List<getEwayBill> EWayBill = new List<getEwayBill>();
            SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
            DataTable dt = new DataTable();
            dt = objSalesInvoiceBL.EditEWayBill(DocID, "SalesInvoice");
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
        [WebMethod]
        public static string GetEditablePermissionFromEInvoice(string SalesInvoiceID, string Action)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            string ispermission = "";
            ispermission = objCRMSalesDtlBL.EditablePermissionInvoiceChallan(Convert.ToInt32(SalesInvoiceID), Action);
            return Convert.ToString(ispermission);
        }


        //Add Rev Transporter Tanmoy
        [WebMethod]
        public static string InsertTransporterControlDetails(long id, String hfControlData)
        {
            CommonBL objCommonBL = new CommonBL();
            objCommonBL.InsertTransporterControlDetails(id, "SI", hfControlData, Convert.ToString(HttpContext.Current.Session["userid"]));

            return "";
        }
        //End of Rev Transporter Tanmoy

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
            Task PopulateStockTrialDataTask = new Task(() => GetInvoiceChallandata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetInvoiceChallandata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_InvoiceChallan_List", con);
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


    public class InvShipBillPopup
    {
        public Int64 ShippingBill_Id { get; set; }
        public DateTime ShippingBill_Date { get; set; }
        public string ShippingBill_Number { get; set; }
        public Int64 ShippingBill_PortId { get; set; }
        public Int64 ShippingBill_InvoiceId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
