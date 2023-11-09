//========================================================== Revision History ============================================================================================
//   1.0   Priti        V2.0.36     02-02-2023      0025253: listing view upgradation required of Journals of Accounts & Finance
//   2.0   Sanchita     V2.0.43     21-09-2023      26831 : Data Freeze is not working properly for Journal
//   3.0   Sanchita     V2.0.43     09-11-2023      While Saving Journal Voucher getting an error in Peekay Local DB. Mantis: 26977 
//========================================== End Revision History =======================================================================================================


using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.OMS.Management.Master.Mobileaccessconfiguration;

namespace ERP.OMS.Management.DailyTask
{
    public partial class JournalVoucherEntry : ERP.OMS.ViewState_class.VSPage
    {
        #region Global Veriable

        BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        Converter oconverter = new Converter();
        string JVNumStr = string.Empty;
        //string Session["ErrorMsg"] = string.Empty;
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        public static EntityLayer.CommonELS.UserRightsForPage rightsforpROJECT;
        bool globalBranchFilter = true;

        string GlobalBranchforOverheadcost = string.Empty;
        string GlobalDateforOverheadcost = string.Empty;

        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsSupplyState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsTaxType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceMainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceSubAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DTtds.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (!IsPostBack)
            {
                grid.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL cbl = new CommonBL();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string AllowProjectInDetailsLevel = cbl.GetSystemSettingsResult("AllowProjectInDetailsLevel");

            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");

            //Add setting for Lead Tanmoy 01-12-2020 Start
            string IsLeadAvailableinTransactions = cbl.GetSystemSettingsResult("IsLeadAvailableinTransactions");
            hdnIsLeadAvailableinTransactions.Value = IsLeadAvailableinTransactions;
            //Add setting for Lead Tanmoy 01-12-2020 End

            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";



                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";


                }
            }


            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                    lookupTDS_Project.ClientVisible = true;
                    lbl_ProjectTDS.Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                    lookupTDS_Project.ClientVisible = false;
                    lbl_ProjectTDS.Visible = false;

                }
            }

            if (!String.IsNullOrEmpty(AllowProjectInDetailsLevel))
            {
                if (AllowProjectInDetailsLevel.ToUpper().Trim() == "NO")
                {
                    hdnAllowProjectInDetailsLevel.Value = "0";
                    grid.Columns[5].Width = 0;
                    gridTDS.Columns[5].Width = 0;
                }
            }

            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    GvJvSearch.Columns[14].Visible = true;
                    GridFullInfo.Columns[5].Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GvJvSearch.Columns[14].Visible = false;
                    GridFullInfo.Columns[5].Visible = false;
                }
            }

            //Tanmoy Hierarchy
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                    //For TDS
                    ddlHierarchyTDS.Visible = true;
                    lblHierarchyTDS.Visible = true;
                    lookupTDS_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                    //For TDS
                    ddlHierarchyTDS.Visible = false;
                    lblHierarchyTDS.Visible = false;
                    lookupTDS_Project.Columns[3].Visible = false;
                }
            }
            //Tanmoy Hierarchy End
            // Rev 2.0
            DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=55");
            if (dtposTime != null && dtposTime.Rows.Count > 0)
            {
                hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
            }

            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=55");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=55");
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
            // End of Rev 2.0
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    Session["lookup_GRNOverhead"] = null;

                    String finyear = "";
                    Session["VendorPayRecProjectCodefromDoc"] = null;
                    Session["VendorPayRecProjectCodefromDocTDS"] = null;
                    finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                    SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                    DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                    FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                    FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                    toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                    toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/dailytask/JournalEntry.aspx");

                    string[] segmentname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                    ViewState["SegmentName"] = segmentname[0];

                    #region Journal Date

                    tDate.EditFormatString = objConverter.GetDateFormat("Date");
                    string fDate = null;

                    string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
                    string FinYearEnd = FinYEnd[0];
                    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string ForJournalDate = Convert.ToString(date3);
                    int month = oDBEngine.GetDate().Month;
                    int date = oDBEngine.GetDate().Day;
                    int Year = oDBEngine.GetDate().Year;

                    if (date3 < oDBEngine.GetDate().Date)
                    {
                        fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
                    }
                    else
                    {
                        fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
                    }

                    tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    #endregion

                    //DataTable DtWhichSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_UserID=" + Session["userid"] + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");                
                    DataTable DtWhichSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId),exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Convert.ToString(Session["userlastsegment"]) + " and ls_UserID=" + Session["userid"] + ") and exch_compId='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Convert.ToString(Session["userlastsegment"]) + ")");

                    if (DtWhichSegment.Rows.Count > 0)
                    {
                        ViewState["WhichSegment"] = 1; //Convert.ToString(DtWhichSegment.Rows[0][0]);
                        hdnSegmentid.Value = Convert.ToString(ViewState["WhichSegment"]);
                    }
                    else
                    {
                        ViewState["WhichSegment"] = null;
                        hdnSegmentid.Value = null;
                    }

                    globalBranchFilter = false;
                    //FillSearchGrid();
                    BindBranchFrom();

                    //Tanmoy Hierarchy Start
                    bindHierarchy();
                    ddlHierarchy.Enabled = false;
                    ddlHierarchyTDS.Enabled = false;
                    //Tanmoy Hierarchy End

                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;

                    Session["VoucherNumber"] = null;
                    Session["VoucherIBRef"] = null;
                    Session["exportval"] = null;
                    Session.Remove("GridfullInfo");
                    Session.Remove("ErrorMsg");
                    hdnJournalNo.Value = "";
                    hdnIBRef.Value = "";
                    txt_Credit.Enabled = false;
                    txt_Debit.Enabled = false;


                    if (Request.QueryString.AllKeys.Contains("IsTagged"))
                    {
                        btncross.Visible = false;
                        Session["VoucherNumber"] = Request.QueryString["req"];
                        Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>journalledger(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                    }

                    #region ####### Search Grid Filter #############

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    DataSet AllDet = new DataSet();
                    DataTable branchtable = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
                    proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
                    proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(Session["userbranchHierarchy"]));

                    AllDet = proc.GetDataSet();

                    branchtable = AllDet.Tables[0];

                    cmbBranchfilter.DataSource = branchtable;
                    cmbBranchfilter.ValueField = "branch_id";
                    cmbBranchfilter.TextField = "branch_description";
                    cmbBranchfilter.DataBind();

                    if (AllDet.Tables[1] != null && AllDet.Tables[1].Rows.Count > 0)
                    {
                        HiddenSubMandatory.Value = Convert.ToString(AllDet.Tables[1].Rows[0]["IsSubledgerMandatory"]);
                    }


                    #endregion
                    BindSystemSettings();
                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                    CmbScheme.Items.Insert(0, new ListItem("--Select Scheme--", "0"));                   
                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }           
        }
        //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
        public static DataSet NumberingSchemData(string Type, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData_New");
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@BranchList", 4000, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }
        public class ddlNumberingSchema
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public class AllddlCustNote
        {
            public List<ddlNumberingSchema> NumberingSchema { get; set; }
        }        
        [WebMethod]
        public static object GetNumberingSchemeByType(string JvID, string Type)
        {
            string BranchID = "";
            if (Type == "Copy")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtBranch = oDBEngine.GetDataTable("SELECT  JournalVoucher_BranchID from Trans_JournalVoucher WHERE  JournalVoucher_ID='" + JvID + "'");
                if (dtBranch.Rows.Count > 0)
                {
                    BranchID = dtBranch.Rows[0]["JournalVoucher_BranchID"].ToString();
                }
            }
            else
            {
                BranchID = HttpContext.Current.Session["userbranchHierarchy"].ToString();
            }
            DataSet Schemadt = NumberingSchemData(Type, BranchID);
            DataTable numberingschemeData = Schemadt.Tables[3];
            AllddlCustNote All = new AllddlCustNote();
            All.NumberingSchema = (from DataRow dr in numberingschemeData.Rows
                                   select new ddlNumberingSchema()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();

            return All;
        }
        //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
        #region System setup
        public DataSet GetSystemSettings()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetSystemSettingValue");
            dt = proc.GetDataSet();
            return dt;
        }
        public void BindSystemSettings()
        {
            DataSet dtSystemSettings = new DataSet();
            dtSystemSettings = GetSystemSettings();
            if (dtSystemSettings.Tables[0] != null && dtSystemSettings.Tables[0].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[0].Rows[0]["Variable_Value"]);
                hdnAutoPrint.Value = Variable_Value;
            }
        }
        #endregion

        #region Main Grid

        public void BindBranchFrom()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
        }
        protected void GridFullInfo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (WhichCall == "FilterGridByDate")
            {
                globalBranchFilter = true;
                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);


                Session["GridfullInfo"] = FillSearchGridGridFullInfo(ToDate, FromDate, BranchID);
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
                GridFullInfo.DataBind();
            }
        }
        public DataTable FillSearchGridGridFullInfo(string Todate, string Fromdate, string BranchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "GetGridDetailsFull");
            proc.AddVarcharPara("@TODATE", 500, Todate);
            proc.AddVarcharPara("@FROMDATE", 500, Fromdate);
            proc.AddVarcharPara("@BranchID", 500, BranchID);
            dt = proc.GetTable();
            return dt;

        }
        protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int RowIndex;
            string PCBCommandName = e.Parameters.Split('~')[0];

            GvJvSearch.JSProperties["cpJVDelete"] = null;

            if (PCBCommandName == "PCB_BindAfterDelete")
            {
                FillSearchGrid();
            }
            else if (PCBCommandName == "PCB_DeleteBtnOkE")
            {
                int RowUpdated = 0;
                RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
                string VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();

                RowUpdated = oDailyTaskOther.Delete_JV(Convert.ToString(IBRef), Convert.ToString(VoucherNumber), Convert.ToInt32(Session["userid"].ToString()));

                if (RowUpdated > 0)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                }
                else if (RowUpdated == -99)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Used in other module can not delete.";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sry for Inconvenience";
                }
            }
            else if (PCBCommandName == "FilterGridByDate")
            {
                globalBranchFilter = true;
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                GvJvSearch.DataSource = FillSearchGrid(true);
                GvJvSearch.DataBind();
            }
        }
        protected void GridFullInfo_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            GridFullInfo.GroupBy(GridFullInfo.Columns["Voucher_NO"]);
        }
        private bool EditButtonVisibleCriteria(ASPxGridView grid, int visibleIndex)
        {
            object row = grid.GetRow(visibleIndex);
            return true;
        }
        protected void GvJvSearch_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonID == "CustomBtnDelete")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanEdit)
            {
                if (e.ButtonID == "CustomBtnEdit")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanPrint)
            {
                if (e.ButtonID == "CustomBtnPrint")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanView)
            {
                if (e.ButtonID == "CustomBtnView")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }

            //if (e.ButtonID == "CustomBtnEdit")
            //{
            //    if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "visible").ToString() == "0")
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    else
            //        e.Visible = DevExpress.Utils.DefaultBoolean.True;
            //}
            //if (e.ButtonID == "CustomBtnDelete")
            //{
            //    if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "visible").ToString() == "0")
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    else
            //        e.Visible = DevExpress.Utils.DefaultBoolean.True;
            //}
        }
        protected void GridFullInfo_DataBinding(object sender, EventArgs e)
        {

            if (Session["GridfullInfo"] != null)
            {
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
            }

        }
        //protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        //{
        //    GvJvSearch.DataSource = FillSearchGrid(globalBranchFilter);

        //    //globalBranchFilter = false;
        //    #region ########## Existing Code ################

        //    //SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
        //    //DataSet DsSearchCallBack = new DataSet();
        //    //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    //{
        //    //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //    //    string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
        //    //    string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //    //    string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //    //    using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
        //    //    {
        //    //        com.CommandType = CommandType.StoredProcedure;
        //    //        com.Parameters.AddWithValue("@FinYear", FinYear);
        //    //        com.Parameters.AddWithValue("@CompanyID", CompanyID);
        //    //        com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);
        //    //        com.Parameters.AddWithValue("@BranchID", branchHierarchy);

        //    //        if (Session["StrQuery"] != null)
        //    //        {
        //    //            com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
        //    //        }
        //    //        else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

        //    //        using (DaSearchCallBack = new SqlDataAdapter(com))
        //    //        {
        //    //            DsSearchCallBack.Clear();
        //    //            DaSearchCallBack.Fill(DsSearchCallBack);
        //    //        }
        //    //    }
        //    //}

        //    //if (DsSearchCallBack.Tables.Count > 0)
        //    //{
        //    //    if (DsSearchCallBack.Tables[0].Rows.Count > 0)
        //    //    {
        //    //        GvJvSearch.DataSource = DsSearchCallBack.Tables[0];
        //    //    }
        //    //}

        //    #endregion
        //}
        //void FillSearchGrid()
        //{
        //    SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
        //    DataSet DsSearchCallBack = new DataSet();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //        string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
        //        string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //        string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //        using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
        //        {
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@FinYear", FinYear);
        //            com.Parameters.AddWithValue("@CompanyID", CompanyID);
        //            com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);
        //            com.Parameters.AddWithValue("@BranchID", branchHierarchy);

        //            if (Session["StrQuery"] != null)
        //            {
        //                com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
        //            }
        //            else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

        //            using (DaSearchCallBack = new SqlDataAdapter(com))
        //            {
        //                DsSearchCallBack.Clear();
        //                DaSearchCallBack.Fill(DsSearchCallBack);
        //            }
        //        }
        //    }
        //    if (DsSearchCallBack.Tables.Count > 0)
        //    {
        //        if (DsSearchCallBack.Tables[0].Rows.Count > 0)
        //        {
        //            BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
        //        }
        //        else
        //        {
        //            BindGrid(GvJvSearch);
        //        }
        //    }
        //    else
        //    {
        //        BindGrid(GvJvSearch);
        //    }
        //}
        private DataTable FillSearchGrid(bool filter = false)  /// filter used for From Date, To Date & Branch wise filter in List Grid
        {
            SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
            DataSet DsSearchCallBack = new DataSet();

            // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
                string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
                string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
                string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

                using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@FinYear", FinYear);
                    com.Parameters.AddWithValue("@CompanyID", CompanyID);
                    com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);

                    if (filter == true)
                    {
                        com.Parameters.AddWithValue("@BranchID", (Convert.ToString(cmbBranchfilter.Value).Trim() == "0") ? Convert.ToString(Session["userbranchHierarchy"]).Trim() : Convert.ToString(cmbBranchfilter.Value).Trim());
                        com.Parameters.AddWithValue("@FromDate", FormDate.Date.ToString("yyyy-MM-dd"));
                        com.Parameters.AddWithValue("@ToDate", toDate.Date.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@BranchID", branchHierarchy);
                    }

                    if (Session["StrQuery"] != null)
                    {
                        com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
                    }
                    else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

                    using (DaSearchCallBack = new SqlDataAdapter(com))
                    {
                        DsSearchCallBack.Clear();
                        DaSearchCallBack.Fill(DsSearchCallBack);
                    }
                }
            }

            return DsSearchCallBack.Tables[0];


            #region ######## Existing Code #############
            //SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
            //DataSet DsSearchCallBack = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            //    string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
            //    string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
            //    string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            //    using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@FinYear", FinYear);
            //        com.Parameters.AddWithValue("@CompanyID", CompanyID);
            //        com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);

            //        if (filter == true)
            //        {
            //            com.Parameters.AddWithValue("@BranchID", (Convert.ToString(cmbBranchfilter.Value).Trim() == "0") ? Convert.ToString(Session["userbranchHierarchy"]).Trim() : Convert.ToString(cmbBranchfilter.Value).Trim());
            //            com.Parameters.AddWithValue("@FromDate", FormDate.Date.ToString("yyyy-MM-dd"));
            //            com.Parameters.AddWithValue("@ToDate", toDate.Date.ToString("yyyy-MM-dd"));
            //        }
            //        else
            //        {
            //            com.Parameters.AddWithValue("@BranchID", branchHierarchy);
            //        }

            //        if (Session["StrQuery"] != null)
            //        {
            //            com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
            //        }
            //        else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

            //        using (DaSearchCallBack = new SqlDataAdapter(com))
            //        {
            //            DsSearchCallBack.Clear();
            //            DaSearchCallBack.Fill(DsSearchCallBack);
            //        }
            //    }
            //}
            //if (DsSearchCallBack.Tables.Count > 0)
            //{
            //    if (DsSearchCallBack.Tables[0].Rows.Count > 0)
            //    {
            //        BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
            //    }
            //    else
            //    {
            //        BindGrid(GvJvSearch);
            //    }
            //}
            //else
            //{
            //    BindGrid(GvJvSearch);
            //}
            #endregion
        }

        #endregion

        #region Classes

        public class MainAccount
        {
            public string CountryID { get; set; }
            public string CountryName { get; set; }
        }
        public class SubAccount
        {
            public string CityID { get; set; }
            public string CityName { get; set; }
        }
        public class VOUCHERLIST
        {
            public string CashReportID { get; set; }
            public string MainAccount { get; set; }
            public string bthSubAccount { get; set; }
            public string WithDrawl { get; set; }
            public string Receipt { get; set; }
            public string Narration { get; set; }
            public string gvColMainAccount { get; set; }
            public string gvColSubAccount { get; set; }
            public string gvMainAcCode { get; set; }
            public string IsSubledger { get; set; }
            public Int64 ProjectId { get; set; }
            public string Project_Code { get; set; }
        }


        public class VOUCHERLISTTDS
        {
            public string CashReportID { get; set; }
            public string MainAccountTDS { get; set; }
            public string bthSubAccountTDS { get; set; }
            public string WithDrawlTDS { get; set; }
            public string ReceiptTDS { get; set; }
            public string NarrationTDS { get; set; }
            public string gvColMainAccountTDS { get; set; }
            public string gvColSubAccountTDS { get; set; }
            public string gvMainAcCodeTDS { get; set; }
            public string IsSubledgerTDS { get; set; }
            public string IsTDS { get; set; }
            public string UniqueID { get; set; }

            public string IsTDSSource { get; set; }

            public string TDSPercentage { get; set; }
            public Int64 ProjectId { get; set; }
            public string Project_Code { get; set; }


        }
        #endregion



        public DataTable GetJournalTDSProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("CashReportID", typeof(string));
            RcpDt.Columns.Add("MainAccount", typeof(string));
            RcpDt.Columns.Add("SubAccount", typeof(string));
            RcpDt.Columns.Add("WithDrawl", typeof(string));
            RcpDt.Columns.Add("Receipt", typeof(string));
            RcpDt.Columns.Add("Narration", typeof(string));
            RcpDt.Columns.Add("Status", typeof(string));
            RcpDt.Columns.Add("IsTDS", typeof(string));
            RcpDt.Columns.Add("UniqueID", typeof(string));
            RcpDt.Columns.Add("IsTDSSource", typeof(string));
            RcpDt.Columns.Add("TDSPercentage", typeof(string));


            return RcpDt;
        }

        public DataTable GetJournalTDSEDitProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("CashReportID", typeof(string));
            RcpDt.Columns.Add("MainAccount", typeof(string));
            RcpDt.Columns.Add("bthSubAccount", typeof(string));
            RcpDt.Columns.Add("WithDrawl", typeof(string));
            RcpDt.Columns.Add("Receipt", typeof(string));
            RcpDt.Columns.Add("Narration", typeof(string));
            RcpDt.Columns.Add("Status", typeof(string));
            RcpDt.Columns.Add("IsTDS", typeof(string));
            RcpDt.Columns.Add("UniqueID", typeof(string));
            RcpDt.Columns.Add("IsTDSSource", typeof(string));
            RcpDt.Columns.Add("TDSPercentage", typeof(string));


            return RcpDt;
        }
        public DataTable GetJournalProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("CashReportID", typeof(string));
            RcpDt.Columns.Add("MainAccount", typeof(string));
            RcpDt.Columns.Add("SubAccount", typeof(string));
            RcpDt.Columns.Add("WithDrawl", typeof(string));
            RcpDt.Columns.Add("Receipt", typeof(string));
            RcpDt.Columns.Add("Narration", typeof(string));
            RcpDt.Columns.Add("Status", typeof(string));


            return RcpDt;
        }
        public DataTable GetJournalEditProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("CashReportID", typeof(string));
            RcpDt.Columns.Add("MainAccount", typeof(string));
            RcpDt.Columns.Add("bthSubAccount", typeof(string));
            RcpDt.Columns.Add("WithDrawl", typeof(string));
            RcpDt.Columns.Add("Receipt", typeof(string));
            RcpDt.Columns.Add("Narration", typeof(string));
            RcpDt.Columns.Add("Status", typeof(string));


            return RcpDt;
        }

        #region Get Mainaccount, Subaccount & Journal Details

        public IEnumerable GetAllMainAccount()
        {
            List<MainAccount> MainAccountList = new List<MainAccount>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_ReferenceID as CountryID,MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName ", " MainAccount_BankCashType Not In ('Bank','Cash')");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                MainAccount MainAccounts = new MainAccount();
                MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                MainAccountList.Add(MainAccounts);
            }

            return MainAccountList;
        }
        public IEnumerable GetMainAccount(string branchId)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);

            List<MainAccount> MainAccountList = new List<MainAccount>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_ReferenceID as CountryID,MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName ", " MainAccount_BankCashType Not In ('Bank','Cash') AND MainAccount_branchId in ('" + branchId + "','0') AND MainAccount_BankCompany in ('" + strCompanyID + "','')");

            DataTable restrictedDT = objEngine.GetDataTable("select branch_id,MainAccount_id from tbl_master_ledgerBranch_map");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DataRow[] restrictedTablerow = restrictedDT.Select("MainAccount_id=" + Convert.ToString(DT.Rows[i]["CountryID"]));

                if (restrictedTablerow.Length > 0)
                {
                    DataTable restrictedTable = restrictedTablerow.CopyToDataTable();
                    DataRow[] restrictedRow = restrictedTable.Select("branch_id=" + branchId);
                    if (restrictedRow.Length > 0)
                    {
                        MainAccount MainAccounts = new MainAccount();
                        MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                        MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                        MainAccountList.Add(MainAccounts);
                    }
                }
                else
                {
                    MainAccount MainAccounts = new MainAccount();
                    MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                    MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                    MainAccountList.Add(MainAccounts);
                }
            }

            return MainAccountList;
        }
        public IEnumerable GetSubAccount(string strMainAccount, string strBranch, string strType, string strSubAccount)
        {
            DataTable DT = GetSubAccountTable(strMainAccount, strBranch, strType, strSubAccount);

            List<SubAccount> SubAccountList = new List<SubAccount>();

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                SubAccount SubAccounts = new SubAccount();
                SubAccounts.CityID = Convert.ToString(DT.Rows[i][1]);
                SubAccounts.CityName = Convert.ToString(DT.Rows[i][0]);
                SubAccountList.Add(SubAccounts);
            }

            return SubAccountList;
        }
        public DataTable GetSubAccountTable(string strMainAccount, string strBranch, string strType, string strSubAccount)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FethSubAccount");
            proc.AddVarcharPara("@CashBank_MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@clause", 500, "");
            proc.AddVarcharPara("@branch", 500, strBranch);
            proc.AddVarcharPara("@SelectionType", 500, strType);
            proc.AddVarcharPara("@SubAccount", 500, strSubAccount);
            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable GetVoucher()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            string IBRef = Convert.ToString(hdnIBRef.Value);
            DataTable Voucherdt = GetJournalDetails("Details", VoucherNumber, IBRef);

            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    Vouchers.ProjectId = Convert.ToInt64(Voucherdt.Rows[i]["ProjectId"]);
                    Vouchers.Project_Code = Convert.ToString(Voucherdt.Rows[i]["Project_Code"]).Trim();
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }

        public IEnumerable GetVoucherTDS()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLISTTDS> VoucherList = new List<VOUCHERLISTTDS>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            string IBRef = Convert.ToString(hdnIBRef.Value);
            DataTable Voucherdt = GetJournalDetailsTDS("Details", VoucherNumber, IBRef);

            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLISTTDS Vouchers = new VOUCHERLISTTDS();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccountTDS = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccountTDS = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawlTDS = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.ReceiptTDS = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.NarrationTDS = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccountTDS = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccountTDS = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCodeTDS = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledgerTDS = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    Vouchers.IsTDS = Convert.ToString(Voucherdt.Rows[i]["IsTDS"]).Trim();
                    Vouchers.UniqueID = Convert.ToString(Voucherdt.Rows[i]["UniqueID"]).Trim();
                    Vouchers.IsTDSSource = Convert.ToString(Voucherdt.Rows[i]["IsTDSSource"]).Trim();
                    Vouchers.TDSPercentage = Convert.ToString(Voucherdt.Rows[i]["TDSPercentage"]).Trim();
                    Vouchers.ProjectId = Convert.ToInt64(Voucherdt.Rows[i]["ProjectId"]);
                    Vouchers.Project_Code = Convert.ToString(Voucherdt.Rows[i]["Project_Code"]).Trim();
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }



        //public IEnumerable GetVoucher(DataTable journalDetails)
        //{
        //    DataSet DsOnLoad = new DataSet();
        //    DataTable tempdt = new DataTable();
        //    List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
        //    DataTable Voucherdt = journalDetails;

        //    if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
        //    {
        //        for (int i = 0; i < Voucherdt.Rows.Count; i++)
        //        {
        //            VOUCHERLIST Vouchers = new VOUCHERLIST();
        //            Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
        //            Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
        //            Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
        //            Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
        //            Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
        //            Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
        //            Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
        //            Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
        //            Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
        //            VoucherList.Add(Vouchers);
        //        }
        //    }

        //    return VoucherList;
        //}
        public DataTable GetJournalDetails(string Action, string JournalID, string IBRef)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@JournalID", 500, JournalID);
            proc.AddVarcharPara("@IBRef", 500, IBRef);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditData(string journalId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "ProjectJournalEditdata");
            proc.AddVarcharPara("@JournalID", 200, journalId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetJournalDetailsTDS(string Action, string JournalID, string IBRef)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetailsTDS");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@JournalID", 500, JournalID);
            proc.AddVarcharPara("@IBRef", 500, IBRef);
            dt = proc.GetTable();
            return dt;
        }

        public static DataTable GetSelectedStateOfSupply(string Action, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@BranchID", 500, BranchId);
            dt = proc.GetTable();
            return dt;
        }



        public DataTable GetRCM(string Action, string MNainAcId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@MainAcId", 500, MNainAcId);
            dt = proc.GetTable();
            return dt;
        }


        #endregion

        #region Grid Events


        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";

                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }


        }
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    //((GridViewDataComboBoxColumn)grid.Columns["CountryID"]).PropertiesComboBox.DataSource = GetAllMainAccount();
        //    // ((GridViewDataComboBoxColumn)grid.Columns["CityID"]).PropertiesComboBox.DataSource = GetSubAccount("", Convert.ToString(Session["userbranchID"]), "ALL", "");

        //    if (!IsPostBack)
        //    {
        //        grid.DataBind();
        //    }
        //}
        protected void CityCmb_Callback(object sender, CallbackEventArgsBase e)
        {
            if (Convert.ToString(e.Parameter.Split('~')[0]) != "null" && Convert.ToString(e.Parameter.Split('~')[0]) != "")
            {
                string countryID = Convert.ToString(e.Parameter.Split('~')[0]);
                string cityID = Convert.ToString(e.Parameter.Split('~')[1]);

                ASPxComboBox c = sender as ASPxComboBox;
                c.DataSource = GetSubAccount(Convert.ToString(countryID), "", "", cityID);//DataProvider.GetCities(countryID);
                c.DataBind();

                DataTable dt = GetRCM("GetRCM", countryID);
                string BranchStateId = Convert.ToString(dt.Rows[0]["IsRCM"]);
                c.JSProperties["cpIsRCM"] = BranchStateId;

            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                if (Session["TagViewJournal"] != null)
                {


                    grid.DataSource = GetVoucherTagging("", (DataTable)Session["TagViewJournal"]);


                }

            }

            else
            {
                if (Convert.ToString(Session["ErrorMsg"]) == "")
                {
                    grid.DataSource = GetVoucher();
                }
                else
                {

                }
            }
        }

        protected void gridTDS_DataBinding(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                if (Session["TagViewJournal"] != null)
                {


                    gridTDS.DataSource = GetVoucherTagging("", (DataTable)Session["TagViewJournalTDS"]);


                }

            }

            else
            {
                if (Convert.ToString(Session["ErrorMsg"]) == "")
                {
                    gridTDS.DataSource = GetVoucherTDS();
                }
                else
                {

                }
            }
        }


        public void BindVoucherGrid()
        {
            grid.DataSource = GetVoucher();
            grid.DataBind();
        }
        protected void CityCmb_Init(object sender, EventArgs e)
        {
            ASPxComboBox cityCombo = sender as ASPxComboBox;
            GridViewEditItemTemplateContainer container = cityCombo.NamingContainer as GridViewEditItemTemplateContainer;
            string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "CountryID"));
            grid.JSProperties["cplastCountryID"] = countryID;
            cityCombo.DataSource = GetSubAccount(Convert.ToString(countryID), "", "", "");
        }
        private void bindMainAccount(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetMainAccount(e.Parameter);
            currentCombo.DataBind();
        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            //if (e.Column.FieldName == "CountryID")
            //{
            //    ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            //}

            if (e.Column.FieldName == "MainAccount")
            {
                e.Editor.ReadOnly = true;
                e.Editor.Focus();
            }
            else if (e.Column.FieldName == "bthSubAccount")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Project_Code")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }




        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            hdnType.Value = type;
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            if (type == "Edit")
            {
                //lookup_Project.ClientEnabled = false;
                //lookup_Project.ClearButton.Visibility = AutoBoolean.False;

                //lookupTDS_Project.ClientEnabled = false;
                // lookupTDS_Project.ClearButton.Visibility = AutoBoolean.False;

                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
                string IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                hdnIBRef.Value = IBRef;
                Session["VoucherNumber"] = VoucherNumber;
                Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetails("Details", VoucherNumber, IBRef);
                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));
                DataTable dtj = GetProjectEditData(hdnJournalNo.Value);
                Int64 ProjJId = 0;
                if (dtj != null)
                {
                    ProjJId = Convert.ToInt64(dtj.Rows[0]["Proj_Id"]);

                }

                string TransactionDate = "";
                string BranchId = "";
                string Transaction_Date = "";
                string BillNumber = "", JvNarration = "", PlaceOfSupply = "", Taxoption = "", IsPartyJournal = "", PartyCount = "", IsRCMD = "";

                DataTable Detailsdt = GetJournalDetails("Header", VoucherNumber, IBRef);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911                   
                    //BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    if (hdnVal.Value == "Copy")
                    {
                        //BillNumber = "Auto";
                        BillNumber = "";
                    }
                    else
                    {
                        BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    }
                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911  
                    TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);
                    IsPartyJournal = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsPartyJournal"]);
                    PartyCount = Convert.ToString(Detailsdt.Rows[0]["PartyCount"]);
                    IsRCMD = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsRCM"]);
                    Transaction_Date = Convert.ToString(Detailsdt.Rows[0]["TransactionDate"]);
                    //hdnIsPartyLedger.Value = PartyCount;

                    GlobalBranchforOverheadcost = BranchId;
                    GlobalDateforOverheadcost = TransactionDate;

                    //grid.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD + "~" + ProjJId +"~"+ Transaction_Date;
                }

                DataTable OverHeadCostdt = GetJournalDetails("OVERHEADCOSTMAP", VoucherNumber, IBRef);
                //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                DataTable Journaldata = GetJournalDetails("Details_New", VoucherNumber, IBRef);
                Session["JournalDetails"] = Journaldata;
                //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                grid.DataSource = GetVoucher();
                grid.DataBind();

                string GRN_IDs = "";
                if (OverHeadCostdt != null && OverHeadCostdt.Rows.Count > 0)
                {
                    //lookup_GRNOverhead.GridView.DataBind();
                    //BindGRNOverheadLookUp(Transaction_Date, BranchId);
                    foreach (DataRow val in OverHeadCostdt.Rows)
                    {
                        GRN_IDs += Convert.ToString(val["GRN_ID"]) + ",";
                        //lookup_GRNOverhead.GridView.Selection.SelectRowByKey(Convert.ToInt32(val["GRN_ID"]));
                    }
                }

                grid.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD + "~" + ProjJId + "~" + Transaction_Date + "~" + GRN_IDs;




                int IsJournalUsed = CheckJournal(VoucherNumber, IBRef);

                //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                if (hdnVal.Value != "Copy")
                {
                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                    if (IsJournalUsed == -99)
                    {
                        //lbl_quotestatusmsg.Text = "*** Used in other module.";

                        grid.JSProperties["cpCheck"] = "-99";
                        cpTagged.Value = "-99";
                    }
                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                }
                //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            }


            else if (type == "View")
            {

                string VoucherNumber = e.Parameters.Split('~')[1];

                hdnJournalNo.Value = e.Parameters.Split('~')[2];
               

                DataTable Voucherdt = GetJournalDetails("DetailsTagging", VoucherNumber, "0");

                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));

                DataTable dtj = GetProjectEditData(hdnJournalNo.Value);
                Int64 ProjJId = 0;
                if (dtj != null)
                {
                    ProjJId = Convert.ToInt64(dtj.Rows[0]["Proj_Id"]);

                }


                string TransactionDate = "";
                string BranchId = "";
                string Transaction_Date = "";
                string BillNumber = "", JvNarration = "", PlaceOfSupply = "", Taxoption = "", IsPartyJournal = "", PartyCount = "", IsRCMD = "", IBRef="";
                DataTable Detailsdt = GetJournalDetails("HeaderTagging", VoucherNumber, "0");
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                     BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                     BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                     TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                     JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                     PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                     Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);
                     Transaction_Date = Convert.ToString(Detailsdt.Rows[0]["TransactionDate"]);
                     IsPartyJournal = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsPartyJournal"]);
                     PartyCount = Convert.ToString(Detailsdt.Rows[0]["PartyCount"]);
                     IsRCMD = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsRCM"]);
                     IBRef = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IBRef"]);

                    GlobalBranchforOverheadcost = BranchId;
                    GlobalDateforOverheadcost = TransactionDate;

                }

                

                DataTable Voucherdt1 = GetJournalDetails("DetailsTagging", VoucherNumber, "0");
                Session["TagViewJournal"] = Voucherdt1;
                DataTable OverHeadCostdt = GetJournalDetails("OVERHEADCOSTMAP", VoucherNumber, IBRef);
                string GRN_IDs = "";
                if (OverHeadCostdt != null && OverHeadCostdt.Rows.Count > 0)
                {                    
                    foreach (DataRow val in OverHeadCostdt.Rows)
                    {
                        GRN_IDs += Convert.ToString(val["GRN_ID"]) + ",";                       
                    }
                }
                grid.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD + "~" + ProjJId + "~" + Transaction_Date + "~" + GRN_IDs; ;

                grid.DataSource = GetVoucherTagging(VoucherNumber, Voucherdt1);
                grid.DataBind();
                Session.Remove("TagViewJournal");

                if (OverHeadCostdt != null && OverHeadCostdt.Rows.Count > 0)
                {
                    //lookup_GRNOverhead.GridView.DataBind();
                    //foreach (DataRow val in OverHeadCostdt.Rows)
                    //{
                    //    lookup_GRNOverhead.GridView.Selection.SelectRowByKey(Convert.ToInt32(val["GRN_ID"]));
                    //}
                }

            }
            else if (type == "BlanckEdit")
            {
                grid.DataSource = null;
                grid.DataBind();

            }
        }
        protected void BindGRNOverheadLookUp(string Date, string userbranch)
        {

            //DataTable GRNOverhead = oDBEngine.GetDataTable("Select * from v_OverHeadCostPurchaseServiceInvoice where PurchaseChallan_BranchId='" + userbranch + "' AND PurchaseChallan_Date<='" + Date + "'");

            DataTable GRNOverhead = BindGRNOverhead(Date, userbranch);
            lookup_GRNOverhead.GridView.Selection.CancelSelection();
            if (GRNOverhead != null && GRNOverhead.Rows.Count > 0)
            {
                Session["lookup_GRNOverhead"] = GRNOverhead;
                lookup_GRNOverhead.DataSource = GRNOverhead;
                lookup_GRNOverhead.DataBind();

            }
            else
            {
                Session["lookup_GRNOverhead"] = null;
                lookup_GRNOverhead.DataSource = GRNOverhead;
                lookup_GRNOverhead.DataBind();
            }


        }

        public int CheckJournal(string VoucherNumber, string IBRef)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "CheckJournal");
            proc.AddNVarcharPara("@IBRef", 50, IBRef);
            proc.AddNVarcharPara("@JournalID", 50, VoucherNumber);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public IEnumerable GetVoucherTagging(string journal, DataTable Voucherdt)
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            //  string IBRef = Convert.ToString(hdnIBRef.Value);


            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {



            grid.JSProperties["cpVouvherNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            string ValidateSubAccount = "";
            string Action = Convert.ToString(hdnMode.Value);
            DataTable Journaldt = new DataTable();
            // Rev 3.0
            string SchemeID = "";
            // End of Rev 3.0
            // Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            if (Action == "0")
            {
                if (Session["JournalDetails"] != null && hdnVal.Value=="Copy")
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = (DataTable)Session["JournalDetails"];
                    DataTable dt = new DataTable();
                    dt = dtTemp.Copy();
                    
                    Journaldt = dt;

                    foreach (DataRow dr in Journaldt.Rows)
                    {
                        dr["MainAccount"] = Convert.ToString(dr["gvColMainAccount"]);
                        dr["SubAccount"] = Convert.ToString(dr["gvColSubAccount"]);
                    }
                    foreach (DataRow row in Journaldt.Rows)
                    {
                        DataColumnCollection dtC = Journaldt.Columns;
                        if (dtC.Contains("gvColSubAccount"))
                        {
                            Journaldt.Columns.Remove("gvColSubAccount");
                        }
                        if (dtC.Contains("gvColMainAccount"))
                        {
                            Journaldt.Columns.Remove("gvColMainAccount");
                        }
                        if (dtC.Contains("gvMainAcCode"))
                        {
                            Journaldt.Columns.Remove("gvMainAcCode");
                        }
                        if (dtC.Contains("IsSubledger"))
                        {
                            Journaldt.Columns.Remove("IsSubledger");
                        }
                        break;
                    }
                }
                else
                {
                    // Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                    Journaldt.Columns.Add("CashReportID", typeof(string));
                    Journaldt.Columns.Add("MainAccount", typeof(string));
                    Journaldt.Columns.Add("SubAccount", typeof(string));
                    Journaldt.Columns.Add("WithDrawl", typeof(string));
                    Journaldt.Columns.Add("Receipt", typeof(string));
                    Journaldt.Columns.Add("Narration", typeof(string));
                    Journaldt.Columns.Add("Status", typeof(string));

                    Journaldt.Columns.Add("ProjectId", typeof(Int64));
                    Journaldt.Columns.Add("Project_Code", typeof(string));
                    // Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                }
                // Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            }
            else
            {
                string VoucherNo = Convert.ToString(Session["VoucherNumber"]);
                string IBRef = Convert.ToString(Session["VoucherIBRef"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {

                    Journaldt = GetJournalDetails("Details", VoucherNo, IBRef);
                    foreach (DataRow dr in Journaldt.Rows)
                    {
                        dr["MainAccount"] = Convert.ToString(dr["gvColMainAccount"]);
                        dr["bthSubAccount"] = Convert.ToString(dr["gvColSubAccount"]);
                    }
                    Journaldt.Columns.Remove("gvColSubAccount");
                    Journaldt.Columns.Remove("gvColMainAccount");
                    Journaldt.Columns.Remove("gvMainAcCode");
                    Journaldt.Columns.Remove("IsSubledger");
                }
                else
                {
                    Journaldt.Columns.Add("CashReportID", typeof(string));
                    Journaldt.Columns.Add("MainAccount", typeof(string));
                    Journaldt.Columns.Add("SubAccount", typeof(string));
                    Journaldt.Columns.Add("WithDrawl", typeof(string));
                    Journaldt.Columns.Add("Receipt", typeof(string));
                    Journaldt.Columns.Add("Narration", typeof(string));
                    Journaldt.Columns.Add("Status", typeof(string));

                    Journaldt.Columns.Add("ProjectId", typeof(Int64));
                    Journaldt.Columns.Add("Project_Code", typeof(string));
                }
            }

            foreach (var args in e.InsertValues)
            {
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = Convert.ToString(args.NewValues["Receipt"]);
                string Narration = Convert.ToString(args.NewValues["Narration"]);

                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);

                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);

                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                {
                    Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", ProjectId, Project_Code);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = Convert.ToString(args.NewValues["Receipt"]);
                string Narration = Convert.ToString(args.NewValues["Narration"]);
                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);

                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);

                bool isDeleted = false;

                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["CashReportID"]);
                    if (DeleteID == CashReportID)
                    {
                        isDeleted = true;
                        break;
                    }
                }
                Journaldt.AcceptChanges();

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                    {

                        if (Convert.ToString(Session["ErrorMsg"]) == "")
                        {
                            DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any   
                            //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                           /* if (dr != null)
                            {
                                dr["MainAccount"] = MainAccount;
                                dr["bthSubAccount"] = SubAccount;
                                dr["WithDrawl"] = WithDrawl;
                                dr["Receipt"] = Receipt;
                                dr["Narration"] = Narration;
                                dr["Status"] = "U";
                                dr["ProjectId"] = ProjectId;
                                dr["Project_Code"] = Project_Code;
                            }*/
                            bool Isexists = false;
                            foreach (DataRow drr in Journaldt.Rows)
                            {
                                string OldCashReportID = Convert.ToString(drr["CashReportID"]);
                                if (OldCashReportID == CashReportID)
                                {
                                    Isexists = true;
                                    dr["MainAccount"] = MainAccount;
                                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911                                   
                                    //dr["bthSubAccount"] = SubAccount;
                                    if (Isexists == true && hdnVal.Value == "Copy")
                                    {
                                        dr["SubAccount"] = SubAccount;
                                    }
                                    else
                                    {
                                        dr["bthSubAccount"] = SubAccount;
                                    }
                                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                                    dr["WithDrawl"] = WithDrawl;
                                    dr["Receipt"] = Receipt;
                                    dr["Narration"] = Narration;
                                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                                    //dr["Status"] = "U";
                                    if (Isexists == true && hdnVal.Value == "Copy")
                                    {
                                        dr["Status"] = "I";
                                    }
                                    else
                                    {
                                        dr["Status"] = "U";
                                    }
                                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                                    dr["ProjectId"] = ProjectId;
                                    dr["Project_Code"] = Project_Code;
                                    break;
                                }
                            }
                            if (Isexists == false)
                            {
                                Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", ProjectId, Project_Code);
                            }
                            //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                        }
                        else

                            Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", ProjectId, Project_Code);
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                }
            }




            //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            //string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(CmbScheme.SelectedValue));
            // Rev 3.0
            //string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(hdnSchemeVal.Value));
            SchemeID = Convert.ToString(Convert.ToInt32(hdnSchemeVal.Value));
            // End of Rev 3.0
            //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            //foreach (DataRow dr in Journaldt.Rows)
            //{
            //    string SupplyState = Convert.ToString(ddlSupplyState.SelectedItem.Text);
            //    string Supplystate = SupplyState.Substring(SupplyState.IndexOf(':') + 1, (SupplyState.Length - (SupplyState.IndexOf(':') + 1))).Replace(")", "");
            //    string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
            //    DataTable dt = new DataTable();
            //    ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            //    proc.AddVarcharPara("@Action", 500, "GetReverseLedger");
            //    proc.AddVarcharPara("@MainAcIDForReverseLedger", 500, Convert.ToString(dr["MainAccount"]));
            //    proc.AddVarcharPara("@AmountCr", 500, Convert.ToString(dr["Receipt"]));
            //    proc.AddVarcharPara("@AmountDr", 500, Convert.ToString(dr["Withdrawl"]));
            //    proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(Session["LastCompany"]));
            //    proc.AddVarcharPara("@SupplyState", 500, Convert.ToString(Supplystate));
            //    proc.AddVarcharPara("@BranchID", 500, Convert.ToString(strBranchID));
            //    dt = proc.GetTable();
            //    if (dt.Rows.Count > 0)
            //    {
            //        string str = Convert.ToString(dt.Rows[0]["RcmType"]);
            //        if (str != "NonRcm")
            //        {
            //            //var ddr = dt.AsEnumerable().Where(x => x.Field<string>("TaxRates_ReverseChargeMainAccount") == "").Single();
            //            DataRow drr = dt.Select("TaxRates_ReverseChargeMainAccount is null or TaxRates_ReverseChargeMainAccount =''").FirstOrDefault();
            //            //.Select("TaxRates_Reverse is null or TaxRates_Reverse=''").FirstOrDefault();
            //            if (drr != null)
            //            {
            //                validate = "HasError";
            //                break;
            //            }

            //        }
            //        //string str = "";
            //        //string str = "";
            //    }

            //}


            // Rev 3.0
            //if (validate == "outrange" || validate == "duplicate" || validate == "HasError")
            //{
            //    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
            //}
            //else if (ValidateSubAccount == "Subaccountmandatory")
            if (ValidateSubAccount == "Subaccountmandatory")
                // End of Rev 3.0
            {
                grid.JSProperties["cpSaveSuccessOrFail"] = ValidateSubAccount;
            }
            else
            {
                if (Action == "0")
                {
                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                    //DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.SelectedValue));
                    /*if (hdnSchemeVal.Value == "0")
                    {
                        return;
                    }
                    else
                    {*/
                        //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                        DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(hdnSchemeVal.Value));
                        //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                        int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                        if (scheme_type != 0)
                        {
                            grid.JSProperties["cpVouvherNo"] = JVNumStr;
                        }
                        //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                   // }
                    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                }               
                Int64 ProjId = 0;
                CommonBL cbl = new CommonBL();
                string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                if (Session["VoucherNumber"] == null)
                {
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                    }
                    else
                    {
                        ProjId = 0;
                    }
                }
                else if (Session["VoucherNumber"] != null)
                {
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                    }
                    else if (lookup_Project.Text == "")
                    {
                        //if (ProjectSelectcashbankModule.ToUpper().Trim() == "NO")
                        //{
                        //    DataTable dtproj = GetProjectEditData(Convert.ToString(Session["VoucherNumber"]));
                        //   // DataTable dt = GetProjectEditData(hdnJournalNo.Value);
                        //    if (dtproj != null)
                        //    {
                        //        ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                        //    }
                        //    else
                        //    {
                        //        ProjId = 0;
                        //    }
                        //}
                        //else
                        //{
                        ProjId = 0;
                        //}

                    }

                    else
                    {
                        ProjId = 0;
                    }
                }

                DataTable Overheadcost = new DataTable();

                Overheadcost.Columns.Add("DOCUMENT_ID", typeof(Int64));
                Overheadcost.Columns.Add("AMOUNT", typeof(decimal));

                List<object> ChallanList = lookup_GRNOverhead.GridView.GetSelectedFieldValues("PurchaseChallan_Id");
                foreach (object Challan in ChallanList)
                {
                    Overheadcost.Rows.Add(Challan, 0);
                }

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strSegmentID = Convert.ToString(ViewState["WhichSegment"]);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);
                string JournalDate = Convert.ToString(tDate.Value);
                string MainNarration = Convert.ToString(txtNarration.Text);
                string SupplyState = Convert.ToString(ddlSupplyState.SelectedItem.Text);
                string TaxOption = Convert.ToString(ddl_AmountAre.SelectedItem.Value);
                string Supplystate = SupplyState.Substring(SupplyState.IndexOf(':') + 1, (SupplyState.Length - (SupplyState.IndexOf(':') + 1))).Replace(")", "");
                string SupplyStateId = Convert.ToString(ddlSupplyState.SelectedItem.Value);
                bool IsRCM = (bool)IsRcm.Value;
                string JournalID = "", IBRef = "";
                if (Action == "0")
                {
                    Action = "Add";
                    JournalID = "";
                    IBRef = oconverter.GetAutoGenerateNo();
                }
                    ////Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
               /* else if(hdnVal.Value=="Copy")
                {
                    Action = "Add";
                    JournalID = "";
                    IBRef = oconverter.GetAutoGenerateNo();
                }*/
               //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                else
                {
                    Action = "Edit";
                    JournalID = Convert.ToString(Session["VoucherNumber"]);
                    IBRef = Convert.ToString(Session["VoucherIBRef"]);
                }
                int i = 1;
                foreach (DataRow Dr in Journaldt.Rows)
                {

                    Dr["CashReportID"] = i;
                    i = i + 1;
                }
                Journaldt.AcceptChanges();
                if (Journaldt.Rows.Count > 0)
                {
                    // Rev 3.0
                    string strJournalNumber = "";
                    // End of Rev 3.0

                    // Rev 3.0 [parameter "SchemeID" and "ref strJournalNumber" added]
                    if (ModifyJournal(Action, JournalID, JVNumStr, strFinYear, strCompanyID, strBranchID, JournalDate, strSegmentID, strCurrency, IBRef, 
                        MainNarration, strUserID, Journaldt, Supplystate, TaxOption, SupplyStateId, IsRCM, ProjId, Overheadcost
                        ,SchemeID , ref strJournalNumber) == true)
                    {
                        Session["VoucherNumber"] = null;
                        Session["VoucherIBRef"] = null;
                        Session["ErrorMsg"] = null;
                        hdnJournalNo.Value = "";
                        hdnIBRef.Value = "";
                        grid.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                    }

                    else
                    {
                        if (Convert.ToString(Session["ErrorMsg"]) == "" || Convert.ToString(Session["ErrorMsg"]) == null)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                        else
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = Session["ErrorMsg"];


                        }
                    }
                }
            }
        }
        protected void GridFullInfo_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));

        }
        #endregion

        #region Database Function

        void BindGrid(ASPxGridView Grid)
        {
            Grid.DataSource = null;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataSet Ds)
        {
            if (Ds.Tables.Count > 0)
            {
                Grid.DataSource = Ds;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }
        void BindGrid(ASPxGridView Grid, DataTable Dt)
        {
            if (Dt.Rows.Count > 0)
            {
                Grid.DataSource = Dt;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }

        // Rev 3.0 [ parameter "string strSchemeID" and "ref string strJournalNumber" added ]
        public bool ModifyJournal(string ActionType, string JournalID, string BillNo, string FinYear, string CompanyID, string BranchID, string JournalDate, string SegmentID,
                                   string CurrencyID, string IBRef, string Narration, string UserID, DataTable JournalDetails, string SupplyState, string TaxOption, 
                                   string SupplyStateId, bool IsRCM, Int64 ProjId
                                    , DataTable Overheadcost, string strSchemeID, ref string strJournalNumber)
        {
            //try
            //{
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_InsertJournalVoucherEntry", con);


                DataTable dtJournal = new DataTable();
                //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                if (ActionType != "Edit")
                //if (ActionType != "Edit" && hdnVal.Value!="Copy")
                {
                    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                    dtJournal = GetJournalProjectDataSource();
                    foreach (DataRow dr in JournalDetails.Rows)
                    {
                        dtJournal.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["SubAccount"]),
                           Convert.ToString(dr["WithDrawl"]), Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]));
                    }
                }
                //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                /*else if(ActionType != "Edit" && hdnVal.Value=="Copy")
                {
                    dtJournal = GetJournalProjectDataSource();
                    foreach (DataRow dr in JournalDetails.Rows)
                    {
                        dtJournal.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["bthSubAccount"]),
                           Convert.ToString(dr["WithDrawl"]), Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]));
                    }
                }*/
                //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
                else
                {
                    dtJournal = GetJournalEditProjectDataSource();
                    foreach (DataRow dr in JournalDetails.Rows)
                    {
                        dtJournal.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["bthSubAccount"]),
                           Convert.ToString(dr["WithDrawl"]), Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]));
                    }
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@JournalID", JournalID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@JournalDate", Convert.ToDateTime(JournalDate));
                cmd.Parameters.AddWithValue("@SegmentID", SegmentID);
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@IBRef", IBRef);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@JournalDetails", dtJournal);
                cmd.Parameters.AddWithValue("@JournalDetailsWithProject", JournalDetails);
                cmd.Parameters.AddWithValue("@SupplyState", SupplyState);
                cmd.Parameters.AddWithValue("@TaxOption", TaxOption);
                cmd.Parameters.AddWithValue("@SupplyStateId", SupplyStateId);
                cmd.Parameters.AddWithValue("@IsRCM", IsRCM);
                // Rev Sayantani
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                // End of Rev Sayantani
                cmd.Parameters.AddWithValue("@Overheadcost_Ids", Overheadcost);
                // Rev 3.0
                cmd.Parameters.AddWithValue("@SchemeID", strSchemeID);
                cmd.Parameters.Add("@ReturnJournalNumber", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnJournalNumber"].Direction = ParameterDirection.Output;
                // End of Rev 3.0

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                // Rev 3.0
                strJournalNumber = Convert.ToString(cmd.Parameters["@ReturnJournalNumber"].Value.ToString());
                grid.JSProperties["cpVouvherNo"] = strJournalNumber;
                // End of Rev Sachita
                cmd.Dispose();
                con.Dispose();



                Session["ErrorMsg"] = Convert.ToString(dsInst.Tables[0].Rows[0]["Error"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {
                    return true;
                }
                else
                {
                    List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
                    if (dsInst.Tables[1].Rows.Count > 0 && dsInst.Tables[1] != null)
                    {
                        for (int i = 0; i < dsInst.Tables[1].Rows.Count; i++)
                        {
                            VOUCHERLIST Vouchers = new VOUCHERLIST();
                            Vouchers.CashReportID = Convert.ToString(dsInst.Tables[1].Rows[i]["CashReportID"]);
                            Vouchers.gvColMainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColMainAccount"]).Trim();
                            Vouchers.gvColSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColSubAccount"]).Trim();
                            Vouchers.WithDrawl = Convert.ToString(dsInst.Tables[1].Rows[i]["WithDrawl"]);
                            Vouchers.Receipt = Convert.ToString(dsInst.Tables[1].Rows[i]["Receipt"]);
                            Vouchers.Narration = Convert.ToString(dsInst.Tables[1].Rows[i]["Narration"]);
                            Vouchers.MainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["MainAccount"]).Trim();
                            Vouchers.bthSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["bthSubAccount"]).Trim();
                            Vouchers.gvMainAcCode = Convert.ToString(dsInst.Tables[1].Rows[i]["gvMainAcCode"]).Trim();
                            Vouchers.IsSubledger = Convert.ToString(dsInst.Tables[1].Rows[i]["IsSubledger"]).Trim();
                            VoucherList.Add(Vouchers);
                        }
                    }



                    grid.DataSource = VoucherList;
                    grid.DataBind();
                    return false;
                }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        // Rev 3.0 [ parameter "string strSchemeID" and "strJournalNumber" added ]
        public bool ModifyJournalTDS(string ActionType, string JournalID, string BillNo, string FinYear, string CompanyID, string BranchID, string JournalDate, string SegmentID,
                                   string CurrencyID, string IBRef, string Narration, string UserID, DataTable JournalDetails, string SupplyState, string TaxOption, string SupplyStateId, bool IsRCM, Int64 ProjId,
                                   string tdsAmount, string tdsCode, DataTable Overheadcost,
                                   string strSchemeID, ref string strJournalNumber,
                                   bool IsSalary = false, bool isTdsConsideration = false, bool NILRateTDS = false)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_InsertTDSJournalVoucherEntry", con);

                DataTable dtJournalTDS = new DataTable();



                if (ActionType != "Edit")
                {
                    dtJournalTDS = GetJournalTDSProjectDataSource();
                    foreach (DataRow dr in JournalDetails.Rows)
                    {
                        dtJournalTDS.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["SubAccount"]),
                           Convert.ToString(dr["WithDrawl"]), Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]), Convert.ToString(dr["IsTDS"]), Convert.ToString(dr["UniqueID"]), Convert.ToString(dr["IsTDSSource"]), Convert.ToString(dr["TDSPercentage"]));
                    }
                }
                else
                {
                    dtJournalTDS = GetJournalTDSEDitProjectDataSource();
                    foreach (DataRow dr in JournalDetails.Rows)
                    {
                        dtJournalTDS.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["bthSubAccount"]),
                           Convert.ToString(dr["WithDrawl"]), Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]), Convert.ToString(dr["IsTDS"]), Convert.ToString(dr["UniqueID"]), Convert.ToString(dr["IsTDSSource"]), Convert.ToString(dr["TDSPercentage"]));
                    }

                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@JournalID", JournalID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@JournalDate", Convert.ToDateTime(JournalDate));
                cmd.Parameters.AddWithValue("@SegmentID", SegmentID);
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@IBRef", IBRef);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@JournalDetails", dtJournalTDS);
                cmd.Parameters.AddWithValue("@JournalDetailsWithProject", JournalDetails);
                cmd.Parameters.AddWithValue("@SupplyState", SupplyState);
                cmd.Parameters.AddWithValue("@TaxOption", TaxOption);
                cmd.Parameters.AddWithValue("@SupplyStateId", SupplyStateId);
                cmd.Parameters.AddWithValue("@IsRCM", IsRCM);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@IsSalary", IsSalary);

                cmd.Parameters.AddWithValue("@tdsCode", tdsCode);
                cmd.Parameters.AddWithValue("@tdsAmount", tdsAmount);

                cmd.Parameters.AddWithValue("@isTdsConsideration", isTdsConsideration);

                cmd.Parameters.AddWithValue("@Overheadcost_Ids", Overheadcost);

                //Nil Rate TDS add Tanmoy 01-12-2020
                cmd.Parameters.AddWithValue("@NILRateTDS", NILRateTDS);
                //Nil Rate TDS add Tanmoy 01-12-2020
                // Rev 3.0
                cmd.Parameters.AddWithValue("@SchemeID", strSchemeID);
                cmd.Parameters.Add("@ReturnJournalNumber", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnJournalNumber"].Direction = ParameterDirection.Output;
                // End of Rev 3.0

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                // Rev 3.0
                strJournalNumber = Convert.ToString(cmd.Parameters["@ReturnJournalNumber"].Value.ToString());
                gridTDS.JSProperties["cpVouvherNo"] = strJournalNumber;
                // End of Rev Sachita
                cmd.Dispose();
                con.Dispose();



                Session["ErrorMsg"] = Convert.ToString(dsInst.Tables[0].Rows[0]["Error"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {
                    return true;
                }
                else
                {
                    List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
                    if (dsInst.Tables[1].Rows.Count > 0 && dsInst.Tables[1] != null)
                    {
                        for (int i = 0; i < dsInst.Tables[1].Rows.Count; i++)
                        {
                            VOUCHERLIST Vouchers = new VOUCHERLIST();
                            Vouchers.CashReportID = Convert.ToString(dsInst.Tables[1].Rows[i]["CashReportID"]);
                            Vouchers.gvColMainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColMainAccount"]).Trim();
                            Vouchers.gvColSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColSubAccount"]).Trim();
                            Vouchers.WithDrawl = Convert.ToString(dsInst.Tables[1].Rows[i]["WithDrawl"]);
                            Vouchers.Receipt = Convert.ToString(dsInst.Tables[1].Rows[i]["Receipt"]);
                            Vouchers.Narration = Convert.ToString(dsInst.Tables[1].Rows[i]["Narration"]);
                            Vouchers.MainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["MainAccount"]).Trim();
                            Vouchers.bthSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["bthSubAccount"]).Trim();
                            Vouchers.gvMainAcCode = Convert.ToString(dsInst.Tables[1].Rows[i]["gvMainAcCode"]).Trim();
                            Vouchers.IsSubledger = Convert.ToString(dsInst.Tables[1].Rows[i]["IsSubledger"]).Trim();
                            VoucherList.Add(Vouchers);
                        }
                    }



                    grid.DataSource = VoucherList;
                    grid.DataBind();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region WebMethod

        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo, string Type)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "JournalVoucher_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));


            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");

                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }

            DataTable dt = GetSelectedStateOfSupply("GetBranchStateCode", strschemaBranch);
            string BranchStateId = Convert.ToString(dt.Rows[0]["StateCode"]);
            strschemavalue = strschemavalue + "~" + BranchStateId + "~" + Valid_From + "~" + Valid_Upto;
            return Convert.ToString(strschemavalue);
        }

        [WebMethod]
        public static string GetTDSLedger(string TDSCode, string tdsdate)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string act_dt = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(tdsdate))
                act_dt = DateTime.ParseExact(tdsdate, "dd-MM-yyyy", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            DataTable DT = objEngine.GetDataTable(" select TOP 1 LTRIM(RTRIM(TDSTCS_MainAccountCode))+'(' +TDSTCS_Code +')'+'~'+ CAST(ma.MainAccount_ReferenceID AS VARCHAR(200)) + '~' +CAST(TDSTCSRates_Rate as VARCHAR(20)) from Master_TDSTCS TDS  inner join Master_MainAccount MA on MA.MainAccount_AccountCode=tds.TDSTCS_MainAccountCode  inner join Config_MultiTDSTCSRates mul on mul.TDSTCSRates_Code=TDS.TDSTCS_code  WHERE TDSTCS_Code='" + Convert.ToString(TDSCode) + "' and  CAST(TDSTCSRates_DateFrom as DATE)<='" + act_dt + "' and CAST(isnull(TDSTCSRates_DateTo,dateadd(year,100,getdate())) as date)>='" + act_dt + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                return Convert.ToString(DT.Rows[0][0]);
            }
            else
            {
                return "";
            }
        }


        [WebMethod]
        public static string GetTDSSubLedger(string EntityId, string TDSCode, string tdsdate)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string act_dt = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(tdsdate))
                act_dt = DateTime.ParseExact(tdsdate, "dd-MM-yyyy", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            DataTable DT = objEngine.GetDataTable(" select TDSTCSRates_Rate,ISNULL(TDSTCSRates_Rouding,0) TDSTCSRates_Rouding from Config_MultiTDSTCSRates RATE INNER JOIN TBL_MASTER_CONTACT CNT ON CNT.TDSRATE_TYPE=RATE.TDSTCSRates_ApplicableFor AND CNT.cnt_internalId='" + EntityId + "' AND RATE.TDSTCSRates_Code='" + TDSCode + "' and  CAST(TDSTCSRates_DateFrom as date)<='" + act_dt + "' and cast(isnull(TDSTCSRates_DateTo,dateadd(year,100,getdate())) as date)>='" + act_dt + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                return Convert.ToString(DT.Rows[0][0]) + "~" + Convert.ToString(DT.Rows[0][1]);
            }
            else
            {
                return "0.00~0";
            }
        }

        // Rev 2.0
        [WebMethod]
        public static string GetAddLock(DateTime LockDate)
        {
            string rtrnvalue = "0";
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForJournalStatus");
            proc.AddDateTimePara("@TransactionDate", LockDate);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        // End of Rev 2.0

        #endregion

        #region Others

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
            // GvJvSearch.Columns[11].Visible = false;

            string filename = "JournalVoucher";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Journal Voucher";
            exporter.PageFooter.Center = "[Page # of Pages #]";

            if (ASPxPageControl1.ActiveTabPage.Index == 0)
            {
                exporter.GridViewID = "GvJvSearch";
            }
            else if (ASPxPageControl1.ActiveTabPage.Index == 1)
            {
                exporter.GridViewID = "GridFullInfo";
                exporter.Landscape = true;
            }
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

        // Rev Sayantani 23-08-2019
        //protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        //{
        //    DataTable dtSchema = new DataTable();
        //    DataTable dtC = new DataTable();
        //    string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
        //    int EmpCode, prefLen, sufxLen, paddCounter;
        //    bool suppressZero = false;

        //    if (sel_schema_Id > 0)
        //    {
        //        dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
        //        int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

        //        if (scheme_type != 0)
        //        {
        //            startNo = dtSchema.Rows[0]["startno"].ToString();
        //            paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
        //            paddedStr = startNo.PadLeft(paddCounter, '0');
        //            prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (tDate.Date.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
        //            sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + tDate.Date.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
        //            prefLen = Convert.ToInt32(prefCompCode.Length);
        //            sufxLen = Convert.ToInt32(sufxCompCode.Length);
        //            suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

        //            if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
        //            {
        //                sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
        //                if (prefLen > 0)
        //                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                else if (scheme_type == 2)
        //                    sqlQuery += "^";
        //                sqlQuery += "[0-9]{" + paddCounter + "}";
        //                if (sufxLen > 0)
        //                    sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
        //                //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
        //                sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
        //                if (scheme_type == 2)
        //                    sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
        //                dtC = oDBEngine.GetDataTable(sqlQuery);

        //                if (dtC.Rows[0][0].ToString() == "")
        //                {
        //                    sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
        //                    if (prefLen > 0)
        //                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                    else if (scheme_type == 2)
        //                        sqlQuery += "^";
        //                    sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
        //                    if (sufxLen > 0)
        //                        sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
        //                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
        //                    sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
        //                    if (scheme_type == 2)
        //                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
        //                    dtC = oDBEngine.GetDataTable(sqlQuery);
        //                }

        //                if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
        //                {
        //                    string uccCode = dtC.Rows[0][0].ToString().Trim();
        //                    int UCCLen = uccCode.Length;
        //                    int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
        //                    string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
        //                    EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
        //                    // out of range journal scheme
        //                    if (EmpCode.ToString().Length > paddCounter)
        //                    {
        //                        return "outrange";
        //                    }
        //                    else
        //                    {
        //                        paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
        //                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
        //                        return "ok";
        //                    }
        //                }
        //                else
        //                {
        //                    JVNumStr = startNo.PadLeft(paddCounter, '0');
        //                    JVNumStr = prefCompCode + paddedStr + sufxCompCode;
        //                    return "ok";
        //                }
        //            }
        //            else
        //            {

        //                if (!suppressZero)
        //                {
        //                    sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
        //                    if (prefLen > 0)
        //                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                    else if (scheme_type == 2)
        //                        sqlQuery += "^";
        //                    sqlQuery += "[0-9]{" + paddCounter + "}";
        //                    if (sufxLen > 0)
        //                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
        //                    sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
        //                    if (scheme_type == 2)
        //                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
        //                    dtC = oDBEngine.GetDataTable(sqlQuery);
        //                }

        //                else
        //                {


        //                    sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
        //                    if (prefLen > 0)
        //                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                    sqlQuery += "[0-9]*";
        //                    if (sufxLen > 0)
        //                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
        //                    sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
        //                    dtC = oDBEngine.GetDataTable(sqlQuery);
        //                }

        //                if (dtC.Rows[0][0].ToString() == "")
        //                {
        //                    sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
        //                    if (prefLen > 0)
        //                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                    else if (scheme_type == 2)
        //                        sqlQuery += "^";
        //                    sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
        //                    if (sufxLen > 0)
        //                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
        //                    sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
        //                    if (scheme_type == 2)
        //                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
        //                    dtC = oDBEngine.GetDataTable(sqlQuery);
        //                }

        //                if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
        //                {
        //                    string uccCode = dtC.Rows[0][0].ToString().Trim();
        //                    int UCCLen = uccCode.Length;
        //                    int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
        //                    string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
        //                    EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
        //                    // out of range journal scheme
        //                    if (EmpCode.ToString().Length > paddCounter)
        //                    {
        //                        return "outrange";
        //                    }
        //                    else
        //                    {

        //                        if (!suppressZero)
        //                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
        //                        else
        //                            paddedStr = EmpCode.ToString();
        //                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
        //                        return "ok";
        //                    }
        //                }
        //                else
        //                {
        //                    if (!suppressZero)
        //                        paddedStr = startNo.PadLeft(paddCounter, '0');
        //                    else
        //                        paddedStr = startNo;
        //                    JVNumStr = prefCompCode + paddedStr + sufxCompCode;
        //                    return "ok";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            sqlQuery = "SELECT JournalVoucher_BillNumber FROM Trans_JournalVoucher WHERE JournalVoucher_BillNumber LIKE '" + manual_str.Trim() + "'";
        //            dtC = oDBEngine.GetDataTable(sqlQuery);
        //            // duplicate manual entry check
        //            if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
        //            {
        //                return "duplicate";
        //            }

        //            JVNumStr = manual_str.Trim();
        //            return "ok";
        //        }
        //    }
        //    else
        //    {
        //        return "noid";
        //    }
        //}
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (tDate.Date.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + tDate.Date.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

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

                        if (!suppressZero)
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
                        }

                        else
                        {

                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.JournalVoucher_BillNumber)=" + i;
                                }

                                dtC = oDBEngine.GetDataTable(sqlQuery);
                                if (dtC.Rows[0][0].ToString() == "")
                                {
                                    break;
                                }
                                i++;
                            }
                            if (i != 1)
                            {
                                sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber)) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.JournalVoucher_BillNumber)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }


                            //sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            //if (prefLen > 0)
                            //    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            //sqlQuery += "[0-9]*";
                            //if (sufxLen > 0)
                            //    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            ////sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                            //dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

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

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.JournalVoucher_BillNumber)=" + (paddCounter - 1);
                            }

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

                                if (!suppressZero)
                                    paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                else
                                    paddedStr = EmpCode.ToString();
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            if (!suppressZero)
                                paddedStr = startNo.PadLeft(paddCounter, '0');
                            else
                                paddedStr = startNo;
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
        // End Of Rev Sayantani 23-08-2019
        protected object GetSummaryValue(string fieldName)
        {
            ASPxSummaryItem summaryItem = grid.TotalSummary.FirstOrDefault(i => i.Tag == fieldName + "_Sum");
            return grid.GetTotalSummaryValue(summaryItem);
        }
        protected object GetTotalSummaryValue()
        {
            ASPxSummaryItem summaryItem = grid.TotalSummary.First(i => i.Tag == "C2_Sum");
            return grid.GetTotalSummaryValue(summaryItem);
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("JournalEntry.aspx");
        }

        #endregion
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes";
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


        protected void EntityServerModeDataSourceProjectForTDS_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            Int64 BranchIdOnnum = 1;

            if (CmbSchemeTDS.SelectedValue != "0")
            {
                DataTable DT = BEngine.GetDataTable("tbl_master_Idschema", "Branch", " Id = " + Convert.ToInt64(CmbSchemeTDS.SelectedValue));
                BranchIdOnnum = Convert.ToInt64(DT.Rows[0]["Branch"]);
            }

            else if (Session["VoucherNumber"] != null)
            {
                DataTable DT = BEngine.GetDataTable("select JournalVoucher_BranchID from  Trans_JournalVoucher where JournalVoucher_BillNumber='" + Session["VoucherNumber"] + "'");
                BranchIdOnnum = Convert.ToInt64(DT.Rows[0]["JournalVoucher_BranchID"]);
            }
            //ProcedureExecute proc = new ProcedureExecute("PRC_ALLProjectList");
            //proc.AddVarcharPara("@WHICHMODULE", 100, "ProjectCodeBind");
            //proc.RunActionQuery();


            //DataTable ds = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            //proc.AddVarcharPara("@Action", 100, "CashBankProjectEditDetails");
            //proc.AddVarcharPara("@CashBank_ID", 100, Convert.ToString(Session["CashBank_ID"]));
            //ds = proc.GetTable();

            //List<V_ProjectList> ProjectList = new List<V_ProjectList>();

            //ProjectList = (from DataRow row in ds.Rows

            //               select new V_ProjectList
            //       {
            //           Proj_Code = row["ProjectCode"].ToString(),
            //           Proj_Id = Convert.ToInt64(row["Proj_Id"]),
            //           Customer = row["Customer"].ToString(),
            //           IsInUse = false



            //       }).ToList();

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == BranchIdOnnum
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }
        // Rev Sayantani
        protected void EntityServerModeDataJournal_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            Int64 BranchIdOnnum = 0;
            if(hdnToUnit.Value!="")
            {
                 BranchIdOnnum = Convert.ToInt64(hdnToUnit.Value);
            }
            
            


            //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            //if (CmbScheme.SelectedValue != "0")
            //if (CmbScheme.SelectedItem.Value != "0")
            //{
            //    //Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            //    //DataTable DT = BEngine.GetDataTable("tbl_master_Idschema", "Branch", " Id = " + Convert.ToInt64(CmbScheme.SelectedValue));
            //    DataTable DT = BEngine.GetDataTable("tbl_master_Idschema", "Branch", " Id = " + Convert.ToInt64(CmbScheme.SelectedItem.Value));
            //    //Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
            //    BranchIdOnnum = Convert.ToInt64(DT.Rows[0]["Branch"]);
            //}

            if (Session["VoucherNumber"] != null)
            {
                DataTable DT = BEngine.GetDataTable("select JournalVoucher_BranchID from  Trans_JournalVoucher where JournalVoucher_BillNumber='" + Session["VoucherNumber"] + "'");
                BranchIdOnnum = Convert.ToInt64(DT.Rows[0]["JournalVoucher_BranchID"]);
            }
            //ProcedureExecute proc = new ProcedureExecute("PRC_ALLProjectList");
            //proc.AddVarcharPara("@WHICHMODULE", 100, "ProjectCodeBind");
            //proc.RunActionQuery();


            //DataTable ds = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            //proc.AddVarcharPara("@Action", 100, "CashBankProjectEditDetails");
            //proc.AddVarcharPara("@CashBank_ID", 100, Convert.ToString(Session["CashBank_ID"]));
            //ds = proc.GetTable();

            //List<V_ProjectList> ProjectList = new List<V_ProjectList>();

            //ProjectList = (from DataRow row in ds.Rows

            //               select new V_ProjectList
            //       {
            //           Proj_Code = row["ProjectCode"].ToString(),
            //           Proj_Id = Convert.ToInt64(row["Proj_Id"]),
            //           Customer = row["Customer"].ToString(),
            //           IsInUse = false



            //       }).ToList();

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == BranchIdOnnum
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        // End of Rev Sayantani
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "VoucherNumber";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);  //---- REV 1.0
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //----REV 1.0
                    //var q = from d in dc.v_JournalEntryLists
                    //        where d.TransactionDate >= Convert.ToDateTime(strFromDate) && d.TransactionDate <= Convert.ToDateTime(strToDate)
                    //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        orderby d.TransactionDate descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.JOURNALENTRYLISTs
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //----REV 1.0
                    //var q = from d in dc.v_JournalEntryLists
                    //        where
                    //        d.TransactionDate >= Convert.ToDateTime(strFromDate) && d.TransactionDate <= Convert.ToDateTime(strToDate) &&
                    //        branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        orderby d.TransactionDate descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.JOURNALENTRYLISTs
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //----REV 1.0
                //var q = from d in dc.v_JournalEntryLists
                //        where d.BranchID == '0'
                //        orderby d.TransactionDate descending
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.JOURNALENTRYLISTs
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
                //----END REV 1.0
            }
        }
        #region Main Account Pop Up



        //chinmoy added for inline project code start 10-12-2019
        protected void ProjectCodeCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocFroProject = new DataTable();
            string Projectid = e.Parameter.Split('~')[1];
            //string DocNo = e.Parameter.Split('~')[2];
            if (hdnProjectSelectInEntryModule.Value == "1")
            {

                if (Projectid != "0")
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument(Projectid);
                }
                else
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument();
                }
            }
            else
            {
                dtDocFroProject = GetProjectCodeDetailsOnDocument();
            }

            Session["VendorPayRecProjectCodefromDoc"] = dtDocFroProject;
            if (dtDocFroProject != null && dtDocFroProject.Rows.Count > 0)
            {
                lookupPopup_ProjectCode.DataSource = dtDocFroProject;
                lookupPopup_ProjectCode.DataBind();
            }
            else
            {
                lookupPopup_ProjectCode.DataSource = null;
                lookupPopup_ProjectCode.DataBind();
            }

        }

        //chinmoy added for projecvt code start 10-12-2019
        protected void lookup_ProjectCode_DataBinding(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDoc"];
            lookupPopup_ProjectCode.DataSource = dsdata;
        }

        //End


        //chinmoy added for inline project code start 10-12-2019
        protected void ProjectCodeCallbackTDS_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocFroProject = new DataTable();
            string Projectid = e.Parameter.Split('~')[1];
            //string DocNo = e.Parameter.Split('~')[2];
            if (hdnProjectSelectInEntryModule.Value == "1")
            {

                if (Projectid != "0")
                {
                    dtDocFroProject = GetProjectCodeTDSDetailsOnDocument(Projectid);
                }
                else
                {
                    dtDocFroProject = GetProjectCodeTDSDetailsOnDocument();
                }
            }
            else
            {
                dtDocFroProject = GetProjectCodeTDSDetailsOnDocument();
            }

            Session["VendorPayRecProjectCodefromDocTDS"] = dtDocFroProject;
            if (dtDocFroProject != null && dtDocFroProject.Rows.Count > 0)
            {
                lookupPopup_ProjectCodeTDS.DataSource = dtDocFroProject;
                lookupPopup_ProjectCodeTDS.DataBind();
            }
            else
            {
                lookupPopup_ProjectCodeTDS.DataSource = null;
                lookupPopup_ProjectCodeTDS.DataBind();
            }

        }

        //chinmoy added for projecvt code start 10-12-2019
        protected void lookup_ProjectCode_DataBindingTDS(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDocTDS"];
            lookupPopup_ProjectCodeTDS.DataSource = dsdata;
        }

        //End





        protected void ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string strBranchID = (Convert.ToString(hdnBranchId.Value) == "") ? "0" : Convert.ToString(hdnBranchId.Value);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                dt = GetMainAccountTableNew(strBranchID, filter, startindex, EndIndex, strCompanyID);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        protected void ASPxMainComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceMainAccount.SelectCommand = @"SELECT MainAccount_ReferenceID,MainAccount_Name,MainAccount_SubLedgerType,MainAccount_ReverseApplicable,TAXable,MainAccount_AccountCode FROM v_MainAccountList_journal WHERE (MainAccount_ReferenceID = @ID) ";

            SqlDataSourceMainAccount.SelectParameters.Clear();
            SqlDataSourceMainAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceMainAccount;
            comboBox.DataBind();
        }

        public DataTable GetMainAccountTableNew(string strBranchID, string filter, int startindex, int EndIndex, string strCompanyID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetMainAccountList");
            proc.AddVarcharPara("@CompanyID", 500, strCompanyID);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            proc.AddVarcharPara("@BranchID", 100, strBranchID);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetProjectCodeDetailsOnDocument()
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "JournalProjectList");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectCodeDetailsOnDocument(string ProjectId)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "JournalProjectListMappingProjectCode");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            proc.AddBigIntegerPara("@ProjectId", Convert.ToInt64(ProjectId));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetProjectCodeTDSDetailsOnDocument()
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "JournalProjectList");
            proc.AddVarcharPara("@BranchId", 20, ddlBranchTDS.SelectedValue);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectCodeTDSDetailsOnDocument(string ProjectId)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "JournalProjectListMappingProjectCode");
            proc.AddVarcharPara("@BranchId", 20, ddlBranchTDS.SelectedValue);
            proc.AddBigIntegerPara("@ProjectId", Convert.ToInt64(ProjectId));
            dt = proc.GetTable();
            return dt;
        }



        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string MainAccountID = hdnMainAccountId.Value;

                dt = GetSubAccountTableNew(MainAccountID, filter, startindex, EndIndex);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        public DataTable GetSubAccountTableNew(string strMainAccount, string filter, int startindex, int EndIndex)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetSubAccountListBYMainAccount");
            proc.AddVarcharPara("@MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            dt = proc.GetTable();
            return dt;
        }
        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceSubAccount.SelectCommand = @"SELECT SubAccount_ReferenceID,Contact_Name,MainAccount_SubLedgerType,mainaccount_referenceid FROM v_SubAccountList WHERE (SubAccount_ReferenceID = @ID) ";

            SqlDataSourceSubAccount.SelectParameters.Clear();
            SqlDataSourceSubAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceSubAccount;
            comboBox.DataBind();
        }

        #endregion

        protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {

            string IsSubledger = "";
            string SubAccount = "";
            string MainAccount = "";

            foreach (GridViewColumn column in grid.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "IsSubledger")
                    IsSubledger = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "bthSubAccount")
                    SubAccount = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "MainAccount")
                    MainAccount = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                //e.Errors[dataColumn] = "Value cannot be null.";
            }

            if (HiddenSubMandatory.Value == "Yes" && IsSubledger != "" && IsSubledger != null && IsSubledger != "None" && string.IsNullOrEmpty(SubAccount) && !string.IsNullOrEmpty(MainAccount))
            {
                AddError(e.Errors, grid.Columns["bthSubAccount"], "Sub ledger is set as mandatory.");
            }
        }
        void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }

        protected void gridTDS_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            gridTDS.JSProperties["cpVouvherNo"] = "";
            gridTDS.JSProperties["cpSaveSuccessOrFail"] = null;
            string ValidateSubAccount = "";
            string Action = Convert.ToString(hdnMode.Value);
            DataTable Journaldt = new DataTable();
            // Rev 3.0
            string SchemeID = "";
            // End of Rev 3.0

            if (Action == "0")
            {
                Journaldt.Columns.Add("CashReportID", typeof(string));
                Journaldt.Columns.Add("MainAccount", typeof(string));
                Journaldt.Columns.Add("SubAccount", typeof(string));
                Journaldt.Columns.Add("WithDrawl", typeof(string));
                Journaldt.Columns.Add("Receipt", typeof(string));
                Journaldt.Columns.Add("Narration", typeof(string));
                Journaldt.Columns.Add("Status", typeof(string));
                Journaldt.Columns.Add("IsTDS", typeof(string));
                Journaldt.Columns.Add("UniqueID", typeof(string));
                Journaldt.Columns.Add("IsTDSSource", typeof(string));
                Journaldt.Columns.Add("TDSPercentage", typeof(string));
                Journaldt.Columns.Add("ProjectId", typeof(Int64));
                Journaldt.Columns.Add("Project_Code", typeof(string));
            }
            else
            {
                string VoucherNo = Convert.ToString(Session["VoucherNumber"]);
                string IBRef = Convert.ToString(Session["VoucherIBRef"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {

                    Journaldt = GetJournalDetailsTDS("Details", VoucherNo, IBRef);
                    foreach (DataRow dr in Journaldt.Rows)
                    {
                        dr["MainAccount"] = Convert.ToString(dr["gvColMainAccount"]);
                        dr["bthSubAccount"] = Convert.ToString(dr["gvColSubAccount"]);
                        dr["TDSPercentage"] = Convert.ToString(dr["TDSPercentage"]).Split('~')[0];
                    }
                    Journaldt.Columns.Remove("gvColSubAccount");
                    Journaldt.Columns.Remove("gvColMainAccount");
                    Journaldt.Columns.Remove("gvMainAcCode");
                    Journaldt.Columns.Remove("IsSubledger");
                }
                else
                {
                    Journaldt.Columns.Add("CashReportID", typeof(string));
                    Journaldt.Columns.Add("MainAccount", typeof(string));
                    Journaldt.Columns.Add("SubAccount", typeof(string));
                    Journaldt.Columns.Add("WithDrawl", typeof(string));
                    Journaldt.Columns.Add("Receipt", typeof(string));
                    Journaldt.Columns.Add("Narration", typeof(string));
                    Journaldt.Columns.Add("Status", typeof(string));
                    Journaldt.Columns.Add("IsTDS", typeof(string));
                    Journaldt.Columns.Add("UniqueID", typeof(string));
                    Journaldt.Columns.Add("IsTDSSource", typeof(string));
                    Journaldt.Columns.Add("TDSPercentage", typeof(string));
                    Journaldt.Columns.Add("ProjectId", typeof(Int64));
                    Journaldt.Columns.Add("Project_Code", typeof(string));
                }
            }

            foreach (var args in e.InsertValues)
            {
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccountTDS"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccountTDS"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawlTDS"]);
                string Receipt = Convert.ToString(args.NewValues["ReceiptTDS"]);
                string Narration = Convert.ToString(args.NewValues["NarrationTDS"]);

                string IsTDS = Convert.ToString(args.NewValues["IsTDS"]);
                string UniqueID = Convert.ToString(args.NewValues["UniqueID"]);
                string IsTDSSource = Convert.ToString(args.NewValues["IsTDSSource"]);
                string TDSPercentage = Convert.ToString(args.NewValues["TDSPercentage"]).Split('~')[0];

                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);
                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);
                if (string.IsNullOrEmpty(IsTDSSource))
                {
                    IsTDSSource = "0";
                }

                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null) || (chkNILRateTDS.Checked && MainAccount != "" && MainAccount != null))
                {
                    Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", IsTDS, UniqueID, IsTDSSource, TDSPercentage, ProjectId, Project_Code);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccountTDS"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccountTDS"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawlTDS"]);
                string Receipt = Convert.ToString(args.NewValues["ReceiptTDS"]);
                string Narration = Convert.ToString(args.NewValues["NarrationTDS"]);

                string IsTDS = Convert.ToString(args.NewValues["IsTDS"]);
                string UniqueID = Convert.ToString(args.NewValues["UniqueID"]);
                string IsTDSSource = Convert.ToString(args.NewValues["IsTDSSource"]);
                string TDSPercentage = Convert.ToString(args.NewValues["TDSPercentage"]);

                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);
                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);

                if (string.IsNullOrEmpty(IsTDSSource))
                {
                    IsTDSSource = "0";
                }


                bool isDeleted = false;

                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["CashReportID"]);
                    if (DeleteID == CashReportID)
                    {
                        isDeleted = true;
                        break;
                    }
                }
                Journaldt.AcceptChanges();

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null) || (chkNILRateTDS.Checked && MainAccount != "" && MainAccount != null))
                    {

                        if (Convert.ToString(Session["ErrorMsg"]) == "")
                        {
                            DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any

                            if (dr != null)
                            {
                                dr["MainAccount"] = MainAccount;
                                dr["bthSubAccount"] = SubAccount;
                                dr["WithDrawl"] = WithDrawl;
                                dr["Receipt"] = Receipt;
                                dr["Narration"] = Narration;
                                dr["Status"] = "U";
                                dr["IsTDS"] = IsTDS;
                                dr["UniqueID"] = UniqueID;
                                dr["IsTDSSource"] = IsTDSSource;
                                dr["TDSPercentage"] = TDSPercentage.Split('~')[0];
                                dr["ProjectId"] = ProjectId;
                                dr["Project_Code"] = Project_Code;


                            }

                        }
                        else

                            Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", IsTDS, UniqueID, IsTDSSource, TDSPercentage, ProjectId, Project_Code);
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                }
            }


            bool ValidTDSmsg = true;
            DataTable dtValidTDS = ValidateConsiderTDS(Journaldt);

            if (dtValidTDS != null && dtValidTDS.Rows.Count > 0 && chkTDSJournal.Checked)
            {
                ValidTDSmsg = Convert.ToBoolean(dtValidTDS.Rows[0][0]);
            }

            //if (chkNILRateTDS.Checked)
            //{
            //    foreach (DataRow Dr in Journaldt.Rows)
            //    {
            //        if (Dr["IsTDS"].ToString() != "")
            //        {
            //            if (Convert.ToDecimal(Dr["WithDrawl"]) > 0 || Convert.ToDecimal(Dr["Receipt"]) > 0)
            //            {
            //                gridTDS.JSProperties["cpNilTDSCheckZeroAmt"] = "Faild";
            //                break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (DataRow Dr in Journaldt.Rows)
            //    {
            //        if (Dr["IsTDS"].ToString() != "")
            //        {
            //            if (Convert.ToDecimal(Dr["WithDrawl"]) <= 0 || Convert.ToDecimal(Dr["Receipt"]) <= 0)
            //            {
            //                gridTDS.JSProperties["cpNilTDSChecknotZeroAmt"] = "Faild";
            //                break;
            //            }
            //        }
            //    }
            //}

            // Rev 3.0
            //string validate = checkNMakeJVCode(Convert.ToString(txtBillNoTDS.Text), Convert.ToInt32(CmbSchemeTDS.SelectedValue));
            //if (validate == "outrange" || validate == "duplicate" || validate == "HasError")
            //{
            //    gridTDS.JSProperties["cpSaveSuccessOrFail"] = validate;
            //}
            //else if (ValidateSubAccount == "Subaccountmandatory")
            SchemeID = Convert.ToString(CmbSchemeTDS.SelectedValue);

            if (ValidateSubAccount == "Subaccountmandatory")
            // End of Rev 3.0
            {
                gridTDS.JSProperties["cpSaveSuccessOrFail"] = ValidateSubAccount;
            }
            else if (!ValidTDSmsg)
            {
                gridTDS.JSProperties["cpSaveSuccessOrFail"] = "InValidTDS";
            }
            else
            {
                if (Action == "0")
                {
                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbSchemeTDS.SelectedValue));
                    int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    if (scheme_type != 0)
                    {
                        gridTDS.JSProperties["cpVouvherNo"] = JVNumStr;
                    }
                }

                Int64 ProjId = 0;
                CommonBL cbl = new CommonBL();
                string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                if (Session["VoucherNumber"] == null)
                {
                    if (lookupTDS_Project.Text != "")
                    {
                        string projectCode = lookupTDS_Project.Text;
                        DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                    }
                    else
                    {
                        ProjId = 0;
                    }
                }
                else if (Session["VoucherNumber"] != null)
                {
                    if (lookupTDS_Project.Text != "")
                    {
                        string projectCode = lookupTDS_Project.Text;
                        DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                    }
                    else if (lookupTDS_Project.Text == "")
                    {
                        //if (ProjectSelectcashbankModule.ToUpper().Trim() == "NO")
                        //{
                        //    DataTable dtproj = GetProjectEditData(Convert.ToString(Session["VoucherNumber"]));
                        //    // DataTable dt = GetProjectEditData(hdnJournalNo.Value);
                        //    if (dtproj != null)
                        //    {
                        //        ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                        //    }
                        //    else
                        //    {
                        //        ProjId = 0;
                        //    }
                        //}
                        //else
                        //{
                        ProjId = 0;
                        //}

                    }

                    else
                    {
                        ProjId = 0;
                    }
                }

                DataTable Overheadcost = new DataTable();

                Overheadcost.Columns.Add("DOCUMENT_ID", typeof(Int64));
                Overheadcost.Columns.Add("AMOUNT", typeof(decimal));

                List<object> ChallanList = lookup_GRNOverheadTDS.GridView.GetSelectedFieldValues("PurchaseChallan_Id");
                foreach (object Challan in ChallanList)
                {
                    Overheadcost.Rows.Add(Challan, 0);
                }

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranchTDS.SelectedValue);
                string strSegmentID = Convert.ToString(ViewState["WhichSegment"]);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);
                string JournalDate = Convert.ToString(tDateTDS.Value);
                string MainNarration = Convert.ToString(txtNarrationTDS.Text);
                string SupplyState = Convert.ToString(ddlSupplyStateTDS.SelectedItem.Text);
                string TaxOption = Convert.ToString(ddl_AmountAreTDS.SelectedItem.Value);
                string Supplystate = SupplyState.Substring(SupplyState.IndexOf(':') + 1, (SupplyState.Length - (SupplyState.IndexOf(':') + 1))).Replace(")", "");
                string SupplyStateId = Convert.ToString(ddlSupplyStateTDS.SelectedItem.Value);
                bool IsRCM = (bool)IsRcmTDS.Value;
                bool isTdsConsideration = chkTDSJournal.Checked;

                string tdsSection = "";
                if (cmbtds.Value != null)
                {
                    tdsSection = cmbtds.Value.ToString();
                }


                //Nil Rate TDS add Tanmoy 05-01-2021
                bool NILRateTDS = chkNILRateTDS.Checked;
                //Nil Rate TDS add Tanmoy 05-01-2021

                string JournalID = "", IBRef = "";
                if (Action == "0")
                {
                    Action = "Add";
                    JournalID = "";
                    IBRef = oconverter.GetAutoGenerateNo();
                }
                else
                {
                    Action = "Edit";
                    JournalID = Convert.ToString(Session["VoucherNumber"]);
                    IBRef = Convert.ToString(Session["VoucherIBRef"]);
                }
                int i = 1;
                foreach (DataRow Dr in Journaldt.Rows)
                {

                    Dr["CashReportID"] = i;
                    i = i + 1;
                }
                Journaldt.AcceptChanges();

                if (Journaldt.Rows.Count > 0)
                {
                    // Rev 3.0
                    string strJournalNumber = "";
                    // End of Rev 3.0

                    // Rev 3.0 [parameter "SchemeID" and "ref strJournalNumber" added]
                    if (ModifyJournalTDS(Action, JournalID, JVNumStr, strFinYear, strCompanyID, strBranchID, JournalDate, strSegmentID, strCurrency, IBRef, 
                        MainNarration, strUserID, Journaldt, Supplystate, TaxOption, SupplyStateId, IsRCM, ProjId, txtTDSAmount.Text, tdsSection, Overheadcost,
                        SchemeID, ref strJournalNumber,
                        chkIsSalary.Checked, isTdsConsideration, NILRateTDS
                        
                        ) == true)
                    {
                        Session["VoucherNumber"] = null;
                        Session["VoucherIBRef"] = null;
                        Session["ErrorMsg"] = null;
                        hdnJournalNo.Value = "";
                        hdnIBRef.Value = "";
                        gridTDS.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                    }

                    else
                    {
                        if (Convert.ToString(Session["ErrorMsg"]) == "" || Convert.ToString(Session["ErrorMsg"]) == null)
                        {
                            gridTDS.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                        else
                        {
                            gridTDS.JSProperties["cpSaveSuccessOrFail"] = Session["ErrorMsg"];


                        }
                    }
                }
            }
        }


        DataTable ValidateConsiderTDS(DataTable dtDetails)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetailsTDS");
            proc.AddVarcharPara("@Action", 500, "ValidateConsiderTDS");
            proc.AddPara("@JournalDetails", dtDetails);
            dt = proc.GetTable();
            return dt;
        }

        protected void gridTDS_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "MainAccountTDS")
            {
                e.Editor.ReadOnly = true;
                e.Editor.Focus();
            }
            else if (e.Column.FieldName == "bthSubAccountTDS")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Project_Code")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }




        }

        protected void gridTDS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            gridTDS.JSProperties["cpSaveSuccessOrFail"] = null;

            if (type == "Edit")
            {
                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
                string IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                hdnIBRef.Value = IBRef;
                Session["VoucherNumber"] = VoucherNumber;
                Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetailsTDS("Details", VoucherNumber, IBRef);
                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));
                DataTable dtj = GetProjectEditData(hdnJournalNo.Value);
                Int64 ProjJId = 0;
                if (dtj != null)
                {
                    ProjJId = Convert.ToInt64(dtj.Rows[0]["Proj_Id"]);

                }
                string BranchId = "", BillNumber = "", TransactionDate = "", JvNarration = "", PlaceOfSupply = "", Taxoption = "", IsPartyJournal = "", PartyCount = "", Transaction_Date = "";
                string IsRCMD = "", IsSalary = "", CONSIDERTDS = "", TDS_CODE = "", TDS_Amount = "";
                bool NILRateTDS = false;

                DataTable Detailsdt = GetJournalDetailsTDS("Header", VoucherNumber, IBRef);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);
                    IsPartyJournal = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsPartyJournal"]);
                    PartyCount = Convert.ToString(Detailsdt.Rows[0]["PartyCount"]);
                    IsRCMD = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsRCM"]);
                    IsSalary = Convert.ToString(Detailsdt.Rows[0]["IsSalary"]);

                    CONSIDERTDS = Convert.ToString(Detailsdt.Rows[0]["IsConsiderAsTDS"]);
                    TDS_CODE = Convert.ToString(Detailsdt.Rows[0]["TDS_CODE"]);
                    TDS_Amount = Convert.ToString(Detailsdt.Rows[0]["TDS_Amount"]);
                    Transaction_Date = Convert.ToString(Detailsdt.Rows[0]["TransactionDate"]);
                    //Add Nil Rate TDS Tanmoy 05-01-2021
                    NILRateTDS = string.IsNullOrEmpty(Detailsdt.Rows[0]["IsNilRated"].ToString()) ? false : Convert.ToBoolean(Detailsdt.Rows[0]["IsNilRated"]);
                    //Add Nil Rate TDS Tanmoy 05-01-2021
                    //hdnIsPartyLedger.Value = PartyCount;

                    // gridTDS.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD + "~" + ProjJId + "~" + IsSalary + "~" + CONSIDERTDS + "~" + TDS_CODE + "~" + TDS_Amount;
                }

                DataTable OverHeadCostdt = GetJournalDetailsTDS("OVERHEADCOSTMAP", VoucherNumber, IBRef);

                gridTDS.DataSource = GetVoucherTDS();
                gridTDS.DataBind();

                string GRN_IDs = "";
                if (OverHeadCostdt != null && OverHeadCostdt.Rows.Count > 0)
                {
                    //lookup_GRNOverhead.GridView.DataBind();
                    foreach (DataRow val in OverHeadCostdt.Rows)
                    {
                        GRN_IDs += Convert.ToString(val["GRN_ID"]) + ",";
                        //lookup_GRNOverhead.GridView.Selection.SelectRowByKey(Convert.ToInt32(val["GRN_ID"]));
                    }
                }

                gridTDS.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD + "~" + ProjJId + "~" + IsSalary + "~" + CONSIDERTDS + "~" + TDS_CODE + "~" + TDS_Amount + "~" + Transaction_Date + "~" + GRN_IDs + "~" + NILRateTDS;
               
                    int IsJournalUsed = CheckJournal(VoucherNumber, IBRef);
                    if (IsJournalUsed == -99)
                    {
                        //lbl_quotestatusmsg.Text = "*** Used in other module.";

                        gridTDS.JSProperties["cpCheck"] = "-99";

                        cpTagged.Value = "-99";
                    }
            }


            else if (type == "View")
            {

                string VoucherNumber = e.Parameters.Split('~')[1];
                //string IBRef = GvJvSearch.GetRowValues(0, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                // hdnIBRef.Value = IBRef;


                // Session["VoucherNumber"] = VoucherNumber;
                // Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetailsTDS("DetailsTagging", VoucherNumber, "0");

                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                ///   DataTable Detailsdt = GetJournalDetails("Header", VoucherNumber, IBRef);

                DataTable Detailsdt = GetJournalDetailsTDS("HeaderTagging", VoucherNumber, "0");



                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    string JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    string PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    string Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);

                    gridTDS.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption;
                }


                DataTable OverHeadCostdt = GetJournalDetailsTDS("OVERHEADCOSTMAP", VoucherNumber, "0");

                DataTable Voucherdt1 = GetJournalDetailsTDS("DetailsTagging", VoucherNumber, "0");
                Session["TagViewJournal"] = Voucherdt1;

                gridTDS.DataSource = GetVoucherTagging(VoucherNumber, Voucherdt1);
                gridTDS.DataBind();
                Session.Remove("TagViewJournal");

                if (OverHeadCostdt != null && OverHeadCostdt.Rows.Count > 0)
                {
                    //lookup_GRNOverhead.GridView.DataBind();
                    //foreach (DataRow val in OverHeadCostdt.Rows)
                    //{
                    //    lookup_GRNOverhead.GridView.Selection.SelectRowByKey(Convert.ToInt32(val["GRN_ID"]));
                    //}
                }

            }
            else if (type == "BlanckEdit")
            {
                gridTDS.DataSource = null;
                gridTDS.DataBind();

            }
        }

        protected void GridTDS_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void GridTDS_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void GridTDS_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        //Tanmoy Hierarchy

        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();

                ddlHierarchyTDS.DataTextField = "H_Name";
                ddlHierarchyTDS.DataValueField = "ID";
                ddlHierarchyTDS.DataSource = hierarchydt;
                ddlHierarchyTDS.DataBind();
            }
        }

        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");

            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }

        protected void btnSaveRecords_Click(object sender, EventArgs e)
        {

        }
        //Tanmoy Hierarchy End



        protected void EntityServerModeDataOverheadCost_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseChallan_Id";
            string DocumentDate = string.Empty;
            String branchid = "0";
            if (hdnType.Value == "Edit" || hdnType.Value == "View")
            {
                branchid = GlobalBranchforOverheadcost;
                DocumentDate = GlobalDateforOverheadcost;
            }
            else
            {
                branchid = ddlBranch.SelectedValue;
                DocumentDate = Convert.ToString(tDate.Date.ToString("yyyy-MM-dd"));
            }

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            var q = from d in dc.v_OverHeadCostPurchaseServiceInvoices
                    where d.PurchaseChallan_BranchId == Convert.ToInt64(branchid) && d.PurchaseChallan_Date <= Convert.ToDateTime(DocumentDate)
                    orderby d.PurchaseChallan_Id descending
                    select d;

            e.QueryableSource = q;
        }


        protected void EntityServerModeDataOverheadCostTDS_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PurchaseChallan_Id";

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            string DocumentDate = Convert.ToString(tDateTDS.Date.ToString("yyyy-MM-dd"));
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            var q = from d in dc.v_OverHeadCostPurchaseServiceInvoices
                    where d.PurchaseChallan_BranchId == Convert.ToInt64(ddlBranchTDS.SelectedValue) && d.PurchaseChallan_Date <= Convert.ToDateTime(DocumentDate)
                    orderby d.PurchaseChallan_Id descending
                    select d;

            e.QueryableSource = q;
        }

        //Tanmoy Setting Wise Lead show
        [WebMethod]
        public static String getMainAccountType(string MainAccountCode)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string SubLedgerType = "";
            DataTable dt2 = oDBEngine.GetDataTable("select top 1 MainAccount_SubLedgerType from v_SubAccountList where mainaccount_referenceid='" + MainAccountCode + "'");
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                SubLedgerType = Convert.ToString(dt2.Rows[0]["MainAccount_SubLedgerType"]);
                return SubLedgerType;
            }
            else
            {
                SubLedgerType = "";
                return SubLedgerType;
            }
        }

        protected void Panellookup_GRNOverhead_Callback(object sender, CallbackEventArgsBase e)
        {

            string Date = string.Empty;
            string userbranch = string.Empty;
            string GRN_IDs = "";

            if (e.Parameter.Split('~')[0] == "BindOverheadCostGrid")
            {
                //if (e.Parameter.Split('~')[2] != null) 
                Date = e.Parameter.Split('~')[1];

                userbranch = ddlBranch.SelectedValue;

                //if (e.Parameter.Split('~')[3] == "DateCheck")
                //{
                //    lookup_GRNOverhead.GridView.Selection.UnselectAll();
                //}

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable GRNOverhead = BindGRNOverhead(Date, userbranch);


                lookup_GRNOverhead.GridView.Selection.CancelSelection();
                lookup_GRNOverhead.DataSource = GRNOverhead;
                lookup_GRNOverhead.DataBind();

                Session["lookup_GRNOverhead"] = GRNOverhead;
            }
            else if (e.Parameter.Split('~')[0] == "BindOverheadCostGridEdit")
            {

                Date = e.Parameter.Split('~')[1];

                userbranch = e.Parameter.Split('~')[2];

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable GRNOverhead = BindGRNOverhead(Date, userbranch);
                lookup_GRNOverhead.GridView.Selection.CancelSelection();
                lookup_GRNOverhead.DataSource = GRNOverhead;
                lookup_GRNOverhead.DataBind();

                Session["lookup_GRNOverhead"] = GRNOverhead;


                GRN_IDs = e.Parameter.Split('~')[3];
                if (GRN_IDs != "")
                {
                    string[] eachQuo = GRN_IDs.Split(',');
                    foreach (string val in eachQuo)
                    {
                        if (val != "")
                        {
                            lookup_GRNOverhead.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }

                    }
                }


            }
        }

        protected void lookup_GRNOverhead_DataBinding(object sender, EventArgs e)
        {
            if (Session["lookup_GRNOverhead"] != null)
            {
                lookup_GRNOverhead.DataSource = (DataTable)Session["lookup_GRNOverhead"];
            }

        }

        public DataTable BindGRNOverhead(string PurchaseChallan_Date, string BranchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "BindOverheadCost");
            proc.AddVarcharPara("@PurchaseChallan_Date", 500, PurchaseChallan_Date);
            proc.AddVarcharPara("@BranchID", 500, BranchID);
            dt = proc.GetTable();
            return dt;

        }

        protected void Panellookup_GRNOverheadTDS_Callback(object sender, CallbackEventArgsBase e)
        {
            string Date = string.Empty;
            string userbranch = string.Empty;
            string GRN_IDs = "";

            if (e.Parameter.Split('~')[0] == "BindOverheadCostGridTDS")
            {

                Date = e.Parameter.Split('~')[1];
                userbranch = ddlBranchTDS.SelectedValue;

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable GRNOverhead = BindGRNOverhead(Date, userbranch);


                lookup_GRNOverheadTDS.GridView.Selection.CancelSelection();
                lookup_GRNOverheadTDS.DataSource = GRNOverhead;
                lookup_GRNOverheadTDS.DataBind();

                Session["lookup_GRNOverheadTDS"] = GRNOverhead;
            }
            else if (e.Parameter.Split('~')[0] == "BindOverheadCostGridEditTDS")
            {

                Date = e.Parameter.Split('~')[1];

                userbranch = e.Parameter.Split('~')[2];

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable GRNOverhead = BindGRNOverhead(Date, userbranch);
                lookup_GRNOverheadTDS.GridView.Selection.CancelSelection();
                lookup_GRNOverheadTDS.DataSource = GRNOverhead;
                lookup_GRNOverheadTDS.DataBind();

                Session["lookup_GRNOverheadTDS"] = GRNOverhead;


                GRN_IDs = e.Parameter.Split('~')[3];
                if (GRN_IDs != "")
                {
                    string[] eachQuo = GRN_IDs.Split(',');
                    foreach (string val in eachQuo)
                    {
                        if (val != "")
                        {
                            lookup_GRNOverheadTDS.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }

                    }
                }

            }
        }

        protected void lookup_GRNOverheadTDS_DataBinding(object sender, EventArgs e)
        {
            if (Session["lookup_GRNOverheadTDS"] != null)
            {
                lookup_GRNOverheadTDS.DataSource = (DataTable)Session["lookup_GRNOverheadTDS"];
            }
        }

        //Rev Nil TDS Checking Tanmoy
        [WebMethod]
        public static object IsNillTDSCheck(string ID)
        {
            String Status = "";
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_NillTDSCheckinEditDelete");
            proc.AddPara("@TDS_DocType", "JVTDS");
            proc.AddPara("@TDS_DocId", ID);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                Status = dt.Rows[0]["IsNilRated"].ToString();
            }
            return Status;
        }
        //End of rev Nil TDS Checking Tanmoy


        //REV 1.0
        
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
            Task PopulateStockTrialDataTask = new Task(() => GetJOURNALENTRYdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetJOURNALENTRYdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_JOURNALENTRY_LIST", con);
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
        //END REV 1.0
    }
}