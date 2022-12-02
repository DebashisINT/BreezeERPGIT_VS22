using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class CreditInvoice_Import : System.Web.UI.Page
    {
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
         BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        Converter oconverter = new Converter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    string strCompanyID = Convert.ToString(Session["LastCompany"]);
                    string FinYear = Convert.ToString(Session["LastFinYear"]);
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    string strBranchID = Convert.ToString(Session["userbranchID"]);

                    SetDateWithFinyearCheck();
                    BindDropdown();
                    Populate_MainAccount();

                    gridInvoice.DataSource = GetGridData();
                    gridInvoice.DataBind();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #region Database Section

        public void BindDropdown()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);

            DataSet dst = GetAllDropDownDetailForSalesInvoice(BranchList, strCompanyID, FinYear);

            #region Branch Dropdown

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataSource = dst.Tables[1];
                ddl_Branch.DataBind();
            }

            if (Session["userbranchID"] != null)
            {
                if (ddl_Branch.Items.Count > 0)
                {
                    int branchindex = 0;
                    int cnt = 0;
                    foreach (ListItem li in ddl_Branch.Items)
                    {
                        if (li.Value == Convert.ToString(Session["userbranchID"]))
                        {
                            cnt = 1;
                            break;
                        }
                        else
                        {
                            branchindex += 1;
                        }
                    }
                    if (cnt == 1)
                    {
                        ddl_Branch.SelectedIndex = branchindex;
                    }
                    else
                    {
                        ddl_Branch.SelectedIndex = cnt;
                    }
                }
            }

            #endregion

            #region Schema Dropdown

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = dst.Tables[0];
                ddl_numberingScheme.DataBind();
            }

            #endregion
        }
        public void Populate_MainAccount()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);
            DataTable dt = GetMainAccount(strBranchID, strCompanyID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lookup_MainAccount.DataSource = dt;
                lookup_MainAccount.DataBind();
            }
        }

        public DataSet GetAllDropDownDetailForSalesInvoice(string BranchList, string CompanyID, string Finyear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_CRMSalesInvoiceImport_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDown_Credit");
            proc.AddVarcharPara("@BranchList", 3000, BranchList);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@FinYear", 100, Finyear);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetMainAccount(string strBranch, string strCompany)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_CRMSalesInvoiceImport_Details");
            proc.AddVarcharPara("@Action", 500, "GetMainAccount");
            proc.AddVarcharPara("@BranchID", 3000, strBranch);
            proc.AddVarcharPara("@CompanyID", 100, strCompany);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSubAccount(string strMainAccount, string strBranch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FethSubAccount");
            proc.AddVarcharPara("@CashBank_MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@clause", 500, "");
            proc.AddVarcharPara("@branch", 500, strBranch);
            proc.AddVarcharPara("@SelectionType", 500, "");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_CRMSalesInvoiceImport_Details");
            proc.AddVarcharPara("@Action", 100, "GetImportCreditNote");
            dt = proc.GetTable();
            return dt;
        }

        #endregion

        #region Grid Section

        protected void gridInvoice_DataBinding(object sender, EventArgs e)
        {
            gridInvoice.DataSource = GetGridData();
        }

        #endregion

        #region Lookup Section

        protected void lookup_MainAccount_DataBinding(object sender, EventArgs e)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable dt = GetMainAccount(strBranchID, strCompanyID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lookup_MainAccount.DataSource = dt;
            }
        }
        protected void lookup_SubAccount_DataBinding(object sender, EventArgs e)
        {
            string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);
            string MainAccount = Convert.ToString(lookup_SubAccount.Value);
            DataTable dt = GetSubAccount(MainAccount, strBranchID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lookup_SubAccount.DataSource = dt;
            }
        }
        protected void MainAccountPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            Populate_MainAccount();
        }
        protected void SubAccountPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);
            string MainAccount = Convert.ToString(e.Parameter.Split('~')[0]);
            //string MainAccount = Convert.ToString(lookup_SubAccount.Value);
            DataTable dt = GetSubAccount(MainAccount, strBranchID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lookup_SubAccount.DataSource = dt;
                lookup_SubAccount.DataBind();
            }
        }

        #endregion

        #region Other Section

        protected void btnImport_Click(object sender, EventArgs e)
        {
            string messege = "";
            txtErrorMessege.Text = "";

            try
            {
                if (OFDBankSelect.FileContent.Length != 0)
                {
                    BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                    string FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                    string Original_FileName = Path.GetFileName(FilePath);
                    string FileName = Original_FileName.Substring(0, Original_FileName.Length - 4) + "_" + oconverter.GetAutoGenerateNo() + ".csv";

                    string UploadPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + FileName);
                    OFDBankSelect.PostedFile.SaveAs(UploadPath);

                    string[] numberingScheme = (ddl_numberingScheme.SelectedValue).Split(new string[] { "~" }, StringSplitOptions.None);
                    string strNumberingScheme = numberingScheme[0];

                    string strPostingDate = "1990-01-01";
                    if (dtPostingDate.Date != null) strPostingDate = Convert.ToDateTime(dtPostingDate.Date).ToString("yyyy-MM-dd");

                    string strCompanyID = Convert.ToString(Session["LastCompany"]);
                    string strFinYear = Convert.ToString(Session["LastFinYear"]);
                    string strBranch = Convert.ToString(hdnBranchID.Value);

                    string strMainAccount = Convert.ToString(lookup_MainAccount.Value);
                    string strSubAccount = Convert.ToString(lookup_SubAccount.Value);

                    //UploadPath = "D:\\Peekay\\ERP\\CommonFolder/haldia list updated as on 15th november 2017_636480117697548170.csv";
                    int strIsComplete = 0;
                    string strDuplicateList = "";

                    if (Path.GetExtension(FilePath) == ".csv" || Path.GetExtension(FilePath) == ".CSV")
                    {
                        ModifyImport(strNumberingScheme, strPostingDate, strCompanyID, strFinYear, strBranch, strMainAccount, strSubAccount, UploadPath, ref strIsComplete, ref strDuplicateList);
                    }
                    else
                    {
                        strIsComplete = -70;
                    }

                    string[] split;
                    int i = 1;

                    if (strIsComplete == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "jAlert('Import Successfully.');window.location='CreditInvoice_Import.aspx';", true);

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                    }
                    else if (strIsComplete == -20)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "jAlert('Data in excel are mismatch.');", true);
                    }
                    else if (strIsComplete == -70)
                    {
                        txtErrorMessege.Text = "Only files with following extensions allowed : CSV.";
                        txtErrorList.Text = "";
                    }
                    else if (strIsComplete == -30)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Customer data in excel are mismatch :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -35)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Customer data in excel are duplicate :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -40)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Adjusted Document No. data in excel are mismatch :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -45)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Adjusted Document No. data in excel are duplicate :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -40)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Adjusted Document No. data in excel are mismatch :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -50)
                    {
                        split = strDuplicateList.Split(';');
                        foreach (string item in split)
                        {
                            if (messege.Trim() == "") messege = Convert.ToString(i) + ". " + item;
                            else messege = messege + "</br> " + Convert.ToString(i) + ". " + item;
                            i++;
                        }

                        txtErrorMessege.Text = "Following Adjusted Document No. already adjust :";
                        txtErrorList.Text = messege;
                    }
                    else if (strIsComplete == -65)
                    {
                        txtErrorList.Text = strDuplicateList;
                    }
                    else if (strIsComplete == -10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "jAlert('Please try again later.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "jAlert('Please attach file..');", true);
                }
            }
            catch (Exception ex)
            {

            }

            ddl_Branch.SelectedValue = Convert.ToString(hdnBranchID.Value);
        }
        public void ModifyImport(string NumberingScheme,string PostingDate, string strCompanyID, string strFinYear, string strBranch, string strMainAccount, string strSubAccount, string strPath, ref int strIsComplete, ref string strDuplicateList)
        {
            try
            {
                DataSet dsInst = new DataSet();
                
               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("proc_CRMCreditNoteImport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NumberingScheme", NumberingScheme);
                cmd.Parameters.AddWithValue("@PostingDate", PostingDate);
                cmd.Parameters.AddWithValue("@CompanyID", strCompanyID);
                cmd.Parameters.AddWithValue("@Finyear", strFinYear);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@MainAccount", strMainAccount);
                cmd.Parameters.AddWithValue("@SubAccount", strSubAccount);
                cmd.Parameters.AddWithValue("@FilePath", strPath);
                cmd.Parameters.AddWithValue("@DocType", "Cr");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@ReturnDuplicate", SqlDbType.VarChar, 4000);
                cmd.Parameters["@ReturnDuplicate"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strDuplicateList = Convert.ToString(cmd.Parameters["@ReturnDuplicate"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        protected void lnlDownloader_Click(object sender, EventArgs e)
        {
            string strFileName = "Sample_CreditNote_Import.csv";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/CSV"; ;
            Response.AppendHeader("Content-Disposition", "attachment; filename=CreditNote.csv");
            Response.TransmitFile(strPath);
            Response.End();
        }
        public void SetDateWithFinyearCheck()
        {
            string finyear = "";
            DateTime MinDate, MaxDate;

            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    MinDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearStartDate"]);
                    MaxDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearEndDate"]);

                    dtPostingDate.Value = null;
                    dtPostingDate.MinDate = MinDate;
                    dtPostingDate.MaxDate = MaxDate;

                    if (DateTime.Now > MaxDate)
                    {
                        dtPostingDate.Value = MaxDate;
                    }
                    else
                    {
                        dtPostingDate.Value = DateTime.Now;
                    }
                }
            }
        }

        #endregion
    }
}