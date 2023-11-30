//========================================================== Revision History ============================================================================================
// 1.0   Priti V2.0.41   28/11/2023        0027028: Customer code column is required in the listing module of Sales entry
//========================================== End Revision History =======================================================================================================--%>

using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerNoteList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CustomerNoteBL CustNoteBL = new CustomerNoteBL();
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        DebitCreditNoteBL objDebitCreditBL = new DebitCreditNoteBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    //Rev 1.0
                    //GvJvSearch.Columns[18].Visible = true;
                    GvJvSearch.Columns[19].Visible = true;
                    //Rev 1.0 End
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    //Rev 1.0
                    //GvJvSearch.Columns[18].Visible = false;
                    GvJvSearch.Columns[19].Visible = false;
                    //Rev 1.0 End
                }
            }
          


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=2");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=2");
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
                spnDeleteLock.InnerText = spnEditLock.InnerText +"DATA is Freezed between   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + " for Delete.";
                spnEditLock.InnerText = "";
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


                rights = new UserRightsForPage();
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerNoteList.aspx");
                BindBranchListGrid();
            }
        }

        private void BindBranchListGrid()
        {
            DataSet dst = new DataSet();
            string userbranch = Convert.ToString(Session["userbranchHierarchy"]);

            dst = CustNoteBL.CustCrNoteBranch(userbranch);
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataSource = dst.Tables[0];
            cmbBranchfilter.DataBind();

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "DCNote_ID";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_CustomerNoteLists
                    //        where branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)

                    //        orderby d.NoteDate descending
                    //        select d;
                    //e.QueryableSource = q;

                    var q = from d in dc.CustomerNoteLists
                            where d.USERID == userid 
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_CustomerNoteLists
                    //        where
                    //        branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)
                    //        orderby d.NoteDate descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.CustomerNoteLists
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_CustomerNoteLists
                //        where d.BranchID == '0'
                //        orderby d.NoteDate descending
                //        select d;
                //e.QueryableSource = q;

                var q = from d in dc.CustomerNoteLists
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
            }
        }

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
            // GvJvSearch.Columns[11].Visible = false;

            string filename = "Customer Debit/Credit Note";
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


        protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int RowIndex;
            string PCBCommandName = e.Parameters.Split('~')[0];

            GvJvSearch.JSProperties["cpJVDelete"] = null;
            if (PCBCommandName == "PCB_DeleteBtnOkE")
            {
                int strIsComplete = 0;
                RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string NoteID = Convert.ToString(RowIndex); //GvJvSearch.GetRowValues(RowIndex, "DCNote_ID").ToString();

                objDebitCreditBL.DeleteDrCrNote("Delete", Convert.ToString(NoteID), ref strIsComplete);

                if (strIsComplete == 1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                }
                //else if (strIsComplete == -1)
                //{
                //    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Customer Receipt/Payment.";
                //}
                //else if (strIsComplete == -2)
                //{
                //    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Adjustment of Document - advance with debit note.";
                //}
                //else if (strIsComplete == -3)
                //{
                //    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Adjustment of Document - Credit Note Customer against the sale Invoice.";
                //}
                else if (strIsComplete == -1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in another module.";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sry for Inconvenience";
                }
            }
            
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void GvJvSearch_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            //if (!rights.CanDelete)
            //{
            //    if (e.ButtonID == "CustomBtnDelete")
            //    {
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    }
            //}
            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonID == "CustomBtnEdit")
            //    {
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    }
            //}
            //if (!rights.CanPrint)
            //{
            //    if (e.ButtonID == "CustomBtnPrint")
            //    {
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    }
            //}
            //if (!rights.CanView)
            //{
            //    if (e.ButtonID == "CustomBtnView")
            //    {
            //        e.Visible = DevExpress.Utils.DefaultBoolean.False;
            //    }
            //}
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes";
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
            Task PopulateStockTrialDataTask = new Task(() => GetCustomerNotedata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetCustomerNotedata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CustomerNote_List", con);
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