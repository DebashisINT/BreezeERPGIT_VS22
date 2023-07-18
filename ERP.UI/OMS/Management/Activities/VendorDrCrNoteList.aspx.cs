/**********************************************************************************************************************************
 1.0    Sanchita  V2.0.38   30-05-2023      ERP - Listing Views - Vendor Debit/Credit Note. refer: 26589  
***********************************************************************************************************************************/
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
    public partial class VendorDrCrNoteList : System.Web.UI.Page
    {
        DebitCreditNoteBL objDebitCreditBL = new DebitCreditNoteBL();
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSourceapplicable.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    GvJvSearch.Columns[17].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    GvJvSearch.Columns[17].Visible = false;
                }
            }


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


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=6");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=6");
            DataTable dtposTimeEditReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=7");
            DataTable dtposTimeDeleteReceipt = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=7");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateeditDataFreeze.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateeditDataFreeze.Value = Convert.ToString(dtposTimeEdit.Rows[0]["DataFreeze_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "Vendor Debit Note DATA is Freezed between   " + hdnLockFromDateeditDataFreeze.Value + " to " + hdnLockToDateeditDataFreeze.Value + "  for Edit. ";

            }
            if (dtposTimeEditReceipt != null && dtposTimeEditReceipt.Rows.Count > 0)
            {
                hdnLockFromDateReceiptEdit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateReceiptedit.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateReceiptEditDataFreeze.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateReceipteditDataFreeze.Value = Convert.ToString(dtposTimeEditReceipt.Rows[0]["DataFreeze_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = (spnEditLock.InnerText) + "Vendor Credit Note DATA is Freezed between   " + hdnLockFromDateReceiptEditDataFreeze.Value + " to " + hdnLockToDateReceipteditDataFreeze.Value + "  for Edit. ";
            }

            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                hdnLockFromDatedeleteDataFreeze.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDatedeleteDataFreeze.Value = Convert.ToString(dtposTimeDelete.Rows[0]["DataFreeze_Todate"]);
                spnDeleteLock.InnerText = "Vendor Debit Note DATA is Freezed between   " + hdnLockFromDatedeleteDataFreeze.Value + " to " + hdnLockToDatedeleteDataFreeze.Value + " for Delete. ";
            }

            if (dtposTimeDeleteReceipt != null && dtposTimeDeleteReceipt.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateReceiptdelete.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateReceiptdeleteDataFreeze.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateReceiptdeleteDataFreeze.Value = Convert.ToString(dtposTimeDeleteReceipt.Rows[0]["DataFreeze_Todate"]);
                spnDeleteLock.InnerText = (spnDeleteLock.InnerText) + "Vendor Credit Note DATA is Freezed between   " + hdnLockFromDateReceiptdeleteDataFreeze.Value + " to " + hdnLockToDateReceiptdeleteDataFreeze.Value + " for Delete. ";
            }

            if (!IsPostBack)
            {

                Session["schemaText"] = null;
                Session["SaveNewValues"] = null;
                BindBranchListGrid();
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                rights = new UserRightsForPage();
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/VendorDrCrNoteList.aspx");
            }
        }
        private void BindBranchListGrid()
        {
            DataSet dst = new DataSet();
            string userbranch = Convert.ToString(Session["userbranchHierarchy"]);

            dst = GetAllDropDownBranchForVendorDrCrNote(userbranch);
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataSource = dst.Tables[0];
            cmbBranchfilter.DataBind();

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        public DataSet GetAllDropDownBranchForVendorDrCrNote(string @userbranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownBranchForVendorDrCrNote");
            proc.AddVarcharPara("@userbranchlist", 4000, @userbranch);
            ds = proc.GetDataSet();
            return ds;
        }
        #region Main Grid      
        //REV RAJDIP

        public DataTable GetDebitNotevalid(string Action, string NotelNo)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            //DataTable dt = objEngine.GetDataTable("select ISNULL(SUM(ISNULL(Adjusted_Amount,0)),0) as ADJAMT "+
            //" from tbl_trans_CrNoteVendorAdvanceAdjustment where Adjusted_doc_id='" + NotelNo + "'");
            //return dt;

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "CheckUsedOrNot");
            proc.AddVarcharPara("@NoteID", 200, NotelNo);
            dt = proc.GetTable();
            return dt;
        }
        //END REV RAJDIP

        protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //int RowIndex;
            string PCBCommandName = e.Parameters.Split('~')[0];

            GvJvSearch.JSProperties["cpJVDelete"] = null;

           
            if (PCBCommandName == "PCB_DeleteBtnOkE")
            {
                int strIsComplete = 0;
                int NoteID = Convert.ToInt32(e.Parameters.Split('~')[1]);
                //string NoteID = GvJvSearch.GetRowValues(RowIndex, "DCNote_ID").ToString();
                //REV RAJDIP
                DataTable Dtdebitnotevalidation = GetDebitNotevalid("VendorNoteDelete", NoteID.ToString());
                 decimal Adjamt=Convert.ToDecimal(Dtdebitnotevalidation.Rows[0]["ADJAMT"].ToString());
                 if (Adjamt > 0)
                 {
                     GvJvSearch.JSProperties["cpJVDelete"] = "Vendor Debit Note already used in another module";
                     return;
                 }
                //END REV RAJDIP
                objDebitCreditBL.DeleteDrCrNote("VendorNoteDelete", Convert.ToString(NoteID), ref strIsComplete);

                if (strIsComplete == 1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                }
                else if (strIsComplete == -1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Vendor Debit/Credit Note already used in another module.";
                }
                //Chinmoy commented Manis Id:20658 start
                //else if (strIsComplete == -2)
                //{
                //    GvJvSearch.JSProperties["cpJVDelete"] = "Vendor Debit/Credit Note already used in Vendor advance with Credit note adjustment.";
                //}
                //else
                //{
                //    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sorry for Inconvenience";
                //}
                //Chinmoy commented Manis Id:20658 End
            }          
        }

        // Rev 1.0
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
            Task PopulateStockTrialDataTask = new Task(() => GetVendorDbCrNotedata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetVendorDbCrNotedata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_VendorDbCrNote_List", con);
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
        // End of Rev 1.0
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "DCNote_ID";

           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                // Rev 1.0
                //if (strBranchID == "0")
                //{
                //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                //    var q = from d in dc.V_VendorDrCrNoteDetailsLists
                //            where d.DCNote_DocumentDate >= Convert.ToDateTime(strFromDate) &&
                //                  d.DCNote_DocumentDate <= Convert.ToDateTime(strToDate) &&
                //                  d.DCNote_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                //                  d.DCNote_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                //                  branchidlist.Contains(Convert.ToInt32(d.DCNote_BranchID))
                //            orderby d.DCNote_ID descending
                //            select d;
                //    e.QueryableSource = q;
                //}
                //else
                //{
                //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                //    var q = from d in dc.V_VendorDrCrNoteDetailsLists
                //            where d.DCNote_DocumentDate >= Convert.ToDateTime(strFromDate) &&
                //                  d.DCNote_DocumentDate <= Convert.ToDateTime(strToDate) &&
                //                  d.DCNote_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                //                  d.DCNote_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                //                  branchidlist.Contains(Convert.ToInt32(d.DCNote_BranchID))
                //            orderby d.DCNote_ID descending
                //            select d;
                //    e.QueryableSource = q;
                //}

                string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                var q = from d in dc.VendorDbCrNoteLists
                        where d.USERID == userid
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                // End of Rev 1.0
            }
            else
            {
                // Rev 1.0
                //var q = from d in dc.V_VendorDrCrNoteDetailsLists
                //        where d.DCNote_BranchID == '0'
                //        orderby d.DCNote_ID descending
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.VendorDbCrNoteLists
                        where d.DCNote_BranchID == 0
                        && d.USERID == userid
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                // End of Rev 1.0
            }
        }
        protected void GvJvSearch_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
           
        }      
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedValue));
            drdExport.SelectedValue = "0";

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
            string filename = "Vendor Debit/Credit Note";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Debit/Credit Note";
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

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\VendDrCrNote\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\VendDrCrNote\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");
                string CrDrNoteType = Convert.ToString(HdCrDrNoteType.Value);
                if (CrDrNoteType == "Credit Note")
                {
                    CrDrNoteType = "Cr";
                }
                else
                {
                    CrDrNoteType = "Dr";
                }

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Contains(CrDrNoteType))
                    {
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
    }
}