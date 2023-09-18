/*******************************************************************************************************************
 * Rev 1.0      Sanchita    V2.0.39     18/09/2023      Update Transporter Action required in Project Mgmt../ Sales Invoice
 *                                                      Mantis : 26806
 *******************************************************************************************************************/
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
using System.Globalization;

namespace ERP.OMS.Management.Activities
{
    public partial class ProjectInvoiceList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectInvoiceList.aspx");

            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {

                if (ProjectSelectInEntryModule == "Yes")
                {
                    GrdQuotation.Columns[10].Visible = true;


                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GrdQuotation.Columns[10].Visible = false;

                }
            }

            string Show_Hierarchy = cSOrder.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(Show_Hierarchy))
            {
                if (Show_Hierarchy == "Yes")
                {
                    GrdQuotation.Columns[27].Visible = true;
                }
                else if (Show_Hierarchy.ToUpper().Trim() == "NO")
                {
                    GrdQuotation.Columns[27].Visible = false;
                }
            }

            if (!IsPostBack)
            {
                Session["SI_ComponentData"] = null;
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

                GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookiesProjectInvoiceGrdQuotation";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesProjectInvoiceGrdQuotation');</script>");

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

                    //DataTable dt = objProjectInvoiceBL.GetTotalDuesData(userbranch, lastCompany);
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
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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
            string filename = "Sales Invoice";
            exporter.FileName = filename;
            exporter.FileName = "GrdProjectInvoice";

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
                deletecnt = objProjectInvoiceBL.DeleteInvoice(WhichType);
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
                dtdata = objProjectInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "CL");
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
        //    dtdata = objProjectInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
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
        //    dtdata = objProjectInvoiceBL.GetQuotationList_GridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate,"CL");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
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
        public static string UpdateEWayBill(string InvoiceID, string UpdateEWayBill, string EWayBillDate, string EWayBillValue)
        {
            ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
            int EWayBill = 0;
            EWayBill = objProjectInvoiceBL.UpdateEWayBill(InvoiceID, UpdateEWayBill, EWayBillDate, EWayBillValue);
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

            ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
            int Bill = 0;

            Bill = objProjectInvoiceBL.UpdateShipBill(actionname, shipdate, ShippingBill_Number, ShippingBill_PortId, ShippingBill_InvoiceId, Convert.ToString(HttpContext.Current.Session["userid"]));
            return Convert.ToString(Bill);
        }


        [WebMethod]
        public static object RetentionDetails(string invoice_id)
        {

            ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
            DataTable dtRetention = null;
            string output = "";
            string Invoice_Number, invoice_ret_Percentage, invoice_ret_amount, invoice_ret_Remarks, invoice_ret_GL, invoice_unpaid_ret_amount, Invoice_TotalAmount;
            dtRetention = objProjectInvoiceBL.GetRetentionDetails(invoice_id);

            if (dtRetention != null && dtRetention.Rows.Count > 0)
            {
                Invoice_Number = Convert.ToString(dtRetention.Rows[0]["Invoice_Number"]);
                invoice_ret_Percentage = Convert.ToString(dtRetention.Rows[0]["invoice_ret_Percentage"]);
                invoice_ret_amount = Convert.ToString(dtRetention.Rows[0]["invoice_ret_amount"]);
                invoice_ret_Remarks = Convert.ToString(dtRetention.Rows[0]["invoice_ret_Remarks"]);
                invoice_ret_GL = Convert.ToString(dtRetention.Rows[0]["invoice_ret_GL"]);
                invoice_unpaid_ret_amount = Convert.ToString(dtRetention.Rows[0]["invoice_unpaid_ret_amount"]);
                Invoice_TotalAmount = Convert.ToString(dtRetention.Rows[0]["Invoice_TotalAmount"]);
                output = Invoice_Number + "~" + invoice_ret_Percentage + "~" + invoice_ret_amount + "~" + invoice_ret_Remarks + "~" + invoice_ret_GL + "~" + invoice_unpaid_ret_amount + "~" + Invoice_TotalAmount;
            }



            return Convert.ToString(output);
        }

        [WebMethod]
        public static object SaveRetentionDetails(string invoice_id, string Ret_Amount, string schema_id, string doc_no, string trans_date)
        {

            ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
            DataTable dtRetention = null;
            string output = "";

            try
            {
                string bill_no = "";
                checkNMakeJVCode(doc_no, Convert.ToInt32(schema_id), DateTime.ParseExact(trans_date, "dd-MM-yyyy", CultureInfo.InvariantCulture), ref bill_no);

                dtRetention = objProjectInvoiceBL.SaveRetentionDetails(invoice_id, Ret_Amount, schema_id, bill_no, trans_date);

                if (dtRetention != null && dtRetention.Rows.Count > 0)
                {
                    output = Convert.ToString(dtRetention.Rows[0][0]);
                }
                else
                {
                    output = "error";
                }
            }
            catch
            {
                output = "error";
            }






            return Convert.ToString(output);
        }


        protected static string checkNMakeJVCode(string manual_str, int sel_schema_Id, DateTime postingdate, ref string JVNumStr)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            DBEngine oDBEngine = new DBEngine();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {

                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (postingdate.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + postingdate.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                }
                else
                {
                    sqlQuery = "SELECT JournalVoucher_BillNumber FROM Trans_JournalVoucher WHERE JournalVoucher_BillNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        #endregion

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

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
                        var q = from d in dc.v_ProjectInvoiceLists
                                where d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                                && d.FinYear == FinYear
                                && d.CreatedBy == userid
                                orderby d.Invoice_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectInvoiceLists
                                where
                                d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                                && d.FinYear == FinYear
                                && d.CreatedBy == userid
                                orderby d.Invoice_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ProjectInvoiceLists
                            where Convert.ToString(d.Invoice_Id) == "-1" && d.InvoiceFor == "CL" && d.CompanyID == CompanyID && d.FinYear == FinYear
                            && d.CreatedBy == userid
                            orderby d.Invoice_Date descending
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
                        var q = from d in dc.v_ProjectInvoiceLists
                                where d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                                && d.FinYear == FinYear
                                orderby d.Invoice_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_ProjectInvoiceLists
                                where
                                d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                                && d.FinYear == FinYear
                                orderby d.Invoice_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_ProjectInvoiceLists
                            where Convert.ToString(d.Invoice_Id) == "-1" && d.InvoiceFor == "CL" && d.CompanyID == CompanyID && d.FinYear == FinYear
                            orderby d.Invoice_Date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
        }

        //REV 1.0
        [WebMethod]
        public static string InsertTransporterControlDetails(long id, String hfControlData)
        {
            CommonBL objCommonBL = new CommonBL();
            objCommonBL.InsertTransporterControlDetails(id, "SI", hfControlData, Convert.ToString(HttpContext.Current.Session["userid"]));
            return "";
        }
        //End of REV 1.0
    }


}