using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;

namespace Reports.Reports.GridReports
{
    public partial class SubLedgerDetails : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        decimal TotalDebit = 0, TotalCredit = 0;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/LedgerPostingReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                Session["dtLedger"] = null;
                Session["SI_ComponentData_Branch"] = null;

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                BranchHoOffice();
                lookupCashBank.DataBind();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                Session.Remove("dtLedger");
                ShowGrid.JSProperties["cpSave"] = null;

                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                string LEDGERID = "";

                if (hdnSelectedLedger.Value != "")
                {
                    LEDGERID = hdnSelectedLedger.Value;
                }
            }
        }
        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //stbill = bll1.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }
        //End of Rev Debashis

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }
                drdExport.SelectedValue = "0";
            }

        }

        public void BindDropDownList()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLS");
            options.Add("3", "RTF");
            options.Add("4", "CSV");

            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }

        public void bindexport(int Filter)
        {
            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Ledger Posting"));
            exporter.FileName = filename;

            exporter.PageHeader.Left = Convert.ToString((Session["Contactrequesttype"] ?? "Ledger Posting Report"));
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
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

        protected void BindLedgerPosting()
        {
            try
            {
                if (Session["dtLedger"] != null)
                {
                    ShowGrid.DataSource = (DataTable)Session["dtLedger"];
                    ShowGrid.DataBind();
                }
            }
            catch { }
        }

        public string getSubLedgerList(string strLedger)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            DataTable dtbl = new DataTable();
            string strSubLedgerList = "";

            string strsql = "";
            strsql = strsql + "";



            return strSubLedgerList;

        }
        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSubLedgerType = ddlSubLedgerType.SelectedValue;
            string strLedgerValue = Convert.ToString(ASPxGridLookupledger.Value);

            Session.Remove("dtLedger");

            ShowGrid.JSProperties["cpSave"] = null;
            string[] CallVal = e.Parameters.ToString().Split('~');
            string type = CallVal[1];
            string code = CallVal[2];
            if (CallVal[1] == "null")
            {
                type = "";
            }
            if (CallVal[2] == "null")
            {
                code = "";
            }
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string TABLENAME = "Ledger Details";
            string BRANCH_ID = "";

            string QuoComponent = "";
            List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in QuoList)
            {
                QuoComponent += "," + Quo;
            }
            BRANCH_ID = QuoComponent.TrimStart(',');

            string CASHBANKTYPE = "";
            string CASHBANKID = "";
            string UserId = "";
            string CUSTVENDID = "";
            string EMPVENDID = "";
            string EMPVENDTYPE = "";
            string LEDGERID = "";
            string ISCREATEORPREVIEW = "P";

            LEDGERID = Convert.ToString(ASPxGridLookupledger.Value);

            EMPVENDID = code;
            EMPVENDTYPE = type;

            Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, TABLENAME, BRANCH_ID, CASHBANKTYPE, CASHBANKID, CUSTVENDID, LEDGERID, ISCREATEORPREVIEW, UserId, EMPVENDID, EMPVENDTYPE));
            PopulateStockTrialDataTask.RunSynchronously();
            ShowGrid.ExpandAll();
        }

        public void GetLedgerdata(string FROMDATE, string TODATE, string TABLENAME, string BRANCH_ID, string CASHBANKTYPE, string CASHBANKID,
                                 string CUSTVENDID, string LEDGERID, string ISCREATEORPREVIEW, string UserId, string EMPVENDID, string EMPVENDTYPE)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("PROC_SUBLEDGER_DETAIL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@EMPLOYEEID", EMPVENDID);
                cmd.Parameters.AddWithValue("@LEDGERID", LEDGERID);
                cmd.Parameters.AddWithValue("@EMPVENDTYPE", EMPVENDTYPE);
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", chkparty.Checked == true ? "1" : "0");

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                Session["dtLedger"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        [WebMethod]
        public static List<string> BindLedgerType(String Ids)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();      
            if (Ids == "null")
            {

                Ids = "0";
            }
            List<string> obj = new List<string>();

            obj = GetLedgerBind(Ids.Trim());
            return obj;
        }

        [WebMethod]
        public static List<string> BindCustomerVendor()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();
            dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {
                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
        }

        

        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dtLedger"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dtLedger"];
            }

        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
               
                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }

        }

        public static List<string> GetLedgerBind(string branch)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetLedgerBind(branch);
            List<string> obj = new List<string>();
            if (stbill.Rows.Count > 0)
            {
                foreach (DataRow dr in stbill.Rows)
                {

                    obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
                }
            }

            return obj;

        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            if (e.Item.FieldName == "CREDIT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "DEBIT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }

            if (e.Item.FieldName == "DRCR")
            {
                if ((TotalDebit - TotalCredit) > 0)
                {
                    e.Text = "Dr.";
                }
                else if ((TotalDebit - TotalCredit) < 0)
                {
                    e.Text = "Cr.";
                }
                else
                {
                    e.Text = "";
                }
            }
            else if (e.Item.FieldName == "CustomerVendor")
            {
                e.Text = "Net Total";
            }
            else
            {
                e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
            }
        }

        protected void dgvVIEW_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.Item == ShowGrid.TotalSummary["NET_AMT"])
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    Decimal gmv = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
                    Decimal equity = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));

                    e.TotalValue = gmv - equity;
                    e.TotalValueReady = true;
                }
            }

        }



        protected void ComponentEmployee_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
                //string type = e.Parameter.Split('~')[1];
                string type = Convert.ToString(ASPxGridLookupledger.Value);

                DataTable dtbl = new DataTable();
                if (Session["userbranchHierarchy"] != null)
                {
                    if (type != "")
                    {
                        dtbl = oDBEngine.GetDataTable("SELECT DISTINCT ID, CNT_INTERNALID AS 'Doc Code', Description"+
                        "  FROM TRANS_ACCOUNTSLEDGER AL"+
                        "       INNER JOIN MASTER_MAINACCOUNT MA ON AccountsLedger_MainAccountID = MainAccount_AccountCode"+
                        "       INNER JOIN "+
                        "	   ("+
                        "		SELECT CNT_ID AS ID, CNT_INTERNALID,  ISNULL(CNT_FIRSTNAME, '') + ' ' + ISNULL(CNT_MIDDLENAME, '') + '  ' + ISNULL(CNT_LASTNAME, '') AS 'Description', "+
                        "			   CASE WHEN CNT_CONTACTTYPE = 'CL' THEN 'Customer' "+
                        "					WHEN CNT_CONTACTTYPE = 'DV' THEN 'Vendor' "+
                        "					WHEN CNT_CONTACTTYPE = 'EM' THEN 'Employee' ELSE '' END AS Type"+
                        "		  FROM TBL_MASTER_CONTACT"+
                        "		 WHERE CNT_CONTACTTYPE IN ('CL','DV','EM')"+
                        "	   ) AS CNT ON AL.ACCOUNTSLEDGER_SUBACCOUNTID = CNT.CNT_INTERNALID"+
                        " WHERE MainAccount_ReferenceID = '" + type + "'"+
                        " ORDER BY CNT_INTERNALID");
                    }
                    else
                    {
                        dtbl = oDBEngine.GetDataTable("SELECT DISTINCT ID, CNT_INTERNALID AS 'Doc Code', Description" +
                        "  FROM TRANS_ACCOUNTSLEDGER AL" +
                        "       INNER JOIN MASTER_MAINACCOUNT MA ON AccountsLedger_MainAccountID = MainAccount_AccountCode" +
                        "       INNER JOIN " +
                        "	   (" +
                        "		SELECT CNT_ID AS ID, CNT_INTERNALID,  ISNULL(CNT_FIRSTNAME, '') + ' ' + ISNULL(CNT_MIDDLENAME, '') + '  ' + ISNULL(CNT_LASTNAME, '') AS 'Description', " +
                        "			   CASE WHEN CNT_CONTACTTYPE = 'CL' THEN 'Customer' " +
                        "					WHEN CNT_CONTACTTYPE = 'DV' THEN 'Vendor' " +
                        "					WHEN CNT_CONTACTTYPE = 'EM' THEN 'Employee' ELSE '' END AS Type" +
                        "		  FROM TBL_MASTER_CONTACT" +
                        "		 WHERE CNT_CONTACTTYPE IN ('CL','DV','EM')" +
                        "	   ) AS CNT ON AL.ACCOUNTSLEDGER_SUBACCOUNTID = CNT.CNT_INTERNALID" +
                        " ORDER BY CNT_INTERNALID");
                    }


                    //if (dtbl.Rows.Count > 0)
                    //{
                    //    Session["SI_ComponentData_Subledger"] = dtbl;
                    //    grid_lookupemployee.DataSource = dtbl;
                    //    grid_lookupemployee.DataBind();
                    //}
                    //else
                    //{
                    //    Session["SI_ComponentData_Subledger"] = null;
                    //    grid_lookupemployee.DataSource = null;
                    //    grid_lookupemployee.DataBind();
                    //}
                }
            }
        }

        protected void ComponentLedger_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {


                string type = e.Parameter.Split('~')[1];

                DataTable dtbl = new DataTable();

                string Ids = "";
                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                Ids = QuoComponent.TrimStart(',');

                if (Ids == "null")
                {

                    Ids = "0";
                }

                CommonBL bll1 = new CommonBL();

                dtbl = bll1.GetLedgerBind(Ids);


                if (dtbl.Rows.Count > 0)
                {
                    Session["SI_ComponentData_ledger"] = dtbl;
                    ASPxGridLookupledger.DataSource = dtbl;
                    ASPxGridLookupledger.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_ledger"] = null;
                    ASPxGridLookupledger.DataSource = null;
                    ASPxGridLookupledger.DataBind();
                }
            }
        }

        protected void lookup_ledger_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_ledger"] != null)
            {
                ASPxGridLookupledger.DataSource = (DataTable)Session["SI_ComponentData_ledger"];
            }
        }

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        protected void CashBankPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            lookupCashBank.GridView.Selection.CancelSelection();
            lookupCashBank.DataSource = GetCashBankList();
            lookupCashBank.DataBind();
        }
        public DataTable GetCashBankList()
        {
            string query = "";

            try
            {
                query = @"SELECT AccountGroup_ReferenceID ID, AccountGroup_Name Name FROM Master_AccountGroup order by AccountGroup_Name";

                DataTable dt = oDBEngine.GetDataTable(query);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        protected void lookupCashBank_DataBinding(object sender, EventArgs e)
        {
            lookupCashBank.DataSource = GetCashBankList();
        }
    }
}