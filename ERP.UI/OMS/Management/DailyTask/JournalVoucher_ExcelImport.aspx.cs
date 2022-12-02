using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using DevExpress.Web;
//using DevExpress.Web.ASPxClasses;
////using DevExpress.Web;


namespace ERP.OMS.Management.DailyTask
{
    public partial class JournalVoucher_ExcelImport : ERP.OMS.ViewState_class.VSPage
    {
        #region LocalVariable
        BusinessLogicLayer.DBEngine oDBEngine;
        BusinessLogicLayer.SQLProcedures oSqlProcedure;
        string strCon;
        DataTable DtCurrentSegment;
        DataTable dtXML = new DataTable();
        BusinessLogicLayer.GenericLogSystem oGenericLogSystem;
        #endregion
        #region Page Property
        public string PCompanyID
        {
            get { return (string)ViewState["CompanyID"]; }
            set { ViewState["CompanyID"] = value; }
        }
        public string PCurrentSegment
        {
            get { return (string)ViewState["CurrentSegment"]; }
            set { ViewState["CurrentSegment"] = value; }
        }
        public string PUserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        public string PFinYear
        {
            get { return (string)ViewState["FinYear"]; }
            set { ViewState["FinYear"] = value; }
        }
        public string PBranchID
        {
            get { return (string)ViewState["BranchID"]; }
            set { ViewState["BranchID"] = value; }
        }
        public string PXMLPATH
        {
            get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
            set { Session["CashBankVoucherFile_XMLPATH"] = value; }
        }
        //This For Log Purpose
        public string LogID
        {
            get { return (string)ViewState["LogID"]; }
            set { ViewState["LogID"] = value; }
        }
        #endregion
        #region PageClass
        void Bind_Combo(ASPxComboBox Combo, int SelectedIndex)
        {
            for (int i = 0; i < 26; i++)
                Combo.Items.Add(Convert.ToChar(65 + i).ToString(), (0 + i));
            Combo.SelectedIndex = SelectedIndex;
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
                //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                oDBEngine = new BusinessLogicLayer.DBEngine();


                DtCurrentSegment = new DataTable();
                PCompanyID = HttpContext.Current.Session["LastCompany"].ToString();
                PUserID = HttpContext.Current.Session["userid"].ToString();
                PFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                DtCurrentSegment = oDBEngine.GetDataTable(@"(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange 
            where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment 
            from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment 
            where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + @") 
            and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*",
                " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                PCurrentSegment = DtCurrentSegment.Rows[0][0].ToString();
                hdn_CurrentSegment.Value = DtCurrentSegment.Rows[0][0].ToString();
                DataTable vsegmentname = oDBEngine.GetDataTable("tbl_master_companyExchange", "isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as CompSegmentName", "exch_compID='" + PCompanyID + "' and  exch_internalID=" + PCurrentSegment);
                hdn_SegID_SegmentName.Value = PCurrentSegment + "~" + vsegmentname.Rows[0][0].ToString().Replace("-", " - ");

                ////Add Defalut Value To Amount Column Combo
                Bind_Combo(ComboLoanNAdvance, 15);
                Bind_Combo(ComboSalaryAccount, 8);
                Bind_Combo(ComboSalaryPayable, 17);
                Bind_Combo(ComboProfTaxM, 9);
                Bind_Combo(ComboTds, 10);
                ///Temprarily Disable Combo
                ComboLoanNAdvance.Enabled = false;
                ComboSalaryAccount.Enabled = false;
                ComboSalaryPayable.Enabled = false;
                ComboProfTaxM.Enabled = false;
                ComboTds.Enabled = false;

                /////////////////////////////////////////////

                //tDate.Date = oDBEngine.GetDate();
                tDate.Date = oDBEngine.GetDate();

            }
        }
        protected void CbpImportFile_Callback(object source, CallbackEventArgsBase e)
        {
            string FilePath = e.Parameter;
            int Counter = 0;
            DataSet ExcelToDs = getData(FilePath, "Salary");
            ArrayList ListOfColToDelete = new ArrayList();
            for (; Counter < ExcelToDs.Tables[0].Columns.Count; )
            {
                if (Convert.ToInt32(ComboSalaryAccount.SelectedItem.Value) != Counter &&
                    Convert.ToInt32(ComboSalaryPayable.SelectedItem.Value) != Counter &&
                    Convert.ToInt32(ComboLoanNAdvance.SelectedItem.Value) != Counter &&
                    Convert.ToInt32(ComboProfTaxM.SelectedItem.Value) != Counter &&
                    Convert.ToInt32(ComboTds.SelectedItem.Value) != Counter)
                {
                    if (Counter != 1) ListOfColToDelete.Add(ExcelToDs.Tables[0].Columns[Counter].ColumnName);
                }
                Counter = Counter + 1;
            }
            foreach (object Dc in ListOfColToDelete) ExcelToDs.Tables[0].Columns.Remove(Dc.ToString());
            ExcelToDs.Tables[0].Columns[0].ColumnName = "Emp_Code";
            ExcelToDs.Tables[0].Columns[1].ColumnName = "Gross";
            ExcelToDs.Tables[0].Columns[2].ColumnName = "P_Tax";
            ExcelToDs.Tables[0].Columns[3].ColumnName = "TDS";
            ExcelToDs.Tables[0].Columns[4].ColumnName = "Loan_Adv";
            ExcelToDs.Tables[0].Columns[5].ColumnName = "Net_Amt";
            Import_Record(ExcelToDs);

        }
        void Import_Record(DataSet DsImport)
        {
            string[] sqlParameterName = new string[15];
            string[] sqlParameterType = new string[15];
            string[] sqlParameterValue = new string[15];
            string[] sqlParameterSize = new string[15];

            if (PCurrentSegment != "" && PCompanyID != "" && PFinYear != "" && Session["UserID"] != null && Session["EntryProfileType"] != null)
            {
                string CreateUser = Session["UserID"].ToString();
                string FinYear = PFinYear.ToString();
                string CompanyID = PCompanyID.ToString();
                string TransactionDate = tDate.Value.ToString();
                string SegmentID = PCurrentSegment.ToString();
                string SalaryAccountMID = txtSalaryAcc_hidden.Value.Split('~')[0];
                string SalaryPayableMID = txtSalaryPayable_hidden.Value.Split('~')[0];
                string LoanNAdvanceMID = txtLoanNAdvance_hidden.Value.Split('~')[0];
                string ProfTaxMID = txtProfTaxM_hidden.Value.Split('~')[0];
                string ProfTaxSID = String.Empty;
                if (txtProfTaxM_hidden.Value.Split('~')[1] != "NONE")
                    ProfTaxSID = txtProfTaxS_hidden.Value.Split('~')[0];
                string TdsMID = txtTdsM_hidden.Value.Split('~')[0];
                string TdsSID = String.Empty;
                if (txtTdsM_hidden.Value.Split('~')[1] != "NONE")
                    TdsSID = txtTdsS_hidden.Value.Split('~')[0];
                string CommonNarration = txtCommonNarration.Text;


                sqlParameterName[0] = "JournalVoucherXML";
                sqlParameterValue[0] = DsImport.GetXml();
                sqlParameterType[0] = "V";
                sqlParameterSize[0] = "20000";
                sqlParameterName[1] = "CreateUser";
                sqlParameterValue[1] = CreateUser;
                sqlParameterType[1] = "V";
                sqlParameterSize[1] = "10";
                sqlParameterName[2] = "FinYear";
                sqlParameterValue[2] = FinYear;
                sqlParameterType[2] = "V";
                sqlParameterSize[2] = "12";
                sqlParameterName[3] = "CompanyID";
                sqlParameterValue[3] = CompanyID;
                sqlParameterType[3] = "V";
                sqlParameterSize[3] = "15";
                sqlParameterName[4] = "TransactionDate";
                sqlParameterValue[4] = TransactionDate;
                sqlParameterType[4] = "V";
                sqlParameterSize[4] = "50";
                sqlParameterName[5] = "SegmentID";
                sqlParameterValue[5] = SegmentID;
                sqlParameterType[5] = "V";
                sqlParameterSize[5] = "10";
                sqlParameterName[6] = "SalaryAccountMID";
                sqlParameterValue[6] = SalaryAccountMID;
                sqlParameterType[6] = "V";
                sqlParameterSize[6] = "22";
                sqlParameterName[7] = "SalaryPayableMID";
                sqlParameterValue[7] = SalaryPayableMID;
                sqlParameterType[7] = "V";
                sqlParameterSize[7] = "22";
                sqlParameterName[8] = "LoanNAdvanceMID";
                sqlParameterValue[8] = LoanNAdvanceMID;
                sqlParameterType[8] = "V";
                sqlParameterSize[8] = "22";
                sqlParameterName[9] = "ProfTaxMID";
                sqlParameterValue[9] = ProfTaxMID;
                sqlParameterType[9] = "V";
                sqlParameterSize[9] = "22";
                sqlParameterName[10] = "ProfTaxSID";
                sqlParameterValue[10] = ProfTaxSID;
                sqlParameterType[10] = "V";
                sqlParameterSize[10] = "22";
                sqlParameterName[11] = "TdsMID";
                sqlParameterValue[11] = TdsMID;
                sqlParameterType[11] = "V";
                sqlParameterSize[11] = "22";
                sqlParameterName[12] = "TdsSID";
                sqlParameterValue[12] = TdsSID;
                sqlParameterType[12] = "V";
                sqlParameterSize[12] = "22";
                sqlParameterName[13] = "CommanNarration";
                sqlParameterValue[13] = CommonNarration;
                sqlParameterType[13] = "V";
                sqlParameterSize[13] = "500";
                sqlParameterName[14] = "EntryUserProfile";
                sqlParameterValue[14] = Session["EntryProfileType"].ToString();
                sqlParameterType[14] = "V";
                sqlParameterSize[14] = "1";
                int RowAffected = BusinessLogicLayer.SQLProcedures.Execute_StoreProcedure("Import_JournalVoucherExcelFile", sqlParameterName, sqlParameterType, sqlParameterValue, sqlParameterSize);
                if (RowAffected == 1)
                {
                    CbpImportFile.JSProperties["cpImportStatus"] = "File Successfully Imported.";
                }
                else
                {
                    CbpImportFile.JSProperties["cpImportStatus"] = "There is Some Problem To Import File.";
                }
            }
        }
        DataSet getData(string FilePath, string NameofSheet)
        {
            string strSQL;
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source='" + FilePath + "';" +
            "Extended Properties=Excel 8.0;";

            strSQL = "SELECT * FROM [" + NameofSheet + "$]";
            DataSet dsExcel = new DataSet();
            OleDbDataAdapter daExcel = new OleDbDataAdapter(strSQL, strConn);
            daExcel.Fill(dsExcel);
            return dsExcel;
        }
        DataSet getData()
        {
           // string strExcelConn = System.Configuration.ConfigurationManager.ConnectionStrings["ExcelConnection"].ToString();

            string strExcelConn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            OleDbConnection dbConn = new OleDbConnection(strExcelConn);
            string strSQL;

            strSQL = "SELECT S.StudentId, S.StudentName, M.Marks, G.Marks, (M.Marks+G.Marks) AS Total " +
                     @"FROM [Students$] S, [Mathematics$] M, [Geography$] G " +
                     @"WHERE(S.StudentId = M.StudentId And S.StudentId = G.StudentId) " +
                     @"ORDER BY (M.Marks+G.Marks) DESC";
            dbConn.Open();

            OleDbCommand cmd = new OleDbCommand(strSQL, dbConn);
            DataSet dsExcel = new DataSet();
            OleDbDataAdapter daExcel = new OleDbDataAdapter(cmd);

            daExcel.Fill(dsExcel);

            return dsExcel;
        }

    }
}