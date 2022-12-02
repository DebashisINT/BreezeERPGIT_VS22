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
using Reports.Model;
namespace Reports.Reports.GridReports
{
    public partial class LedgerPostingReport : ERP.OMS.ViewState_class.VSPage
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
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
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Ledger Posting - Detail";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                //Session["dtLedger"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["exportval"] = null;
                Session["IsLedgerPostingFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");


                // Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                //Rev Debashis && Implement Head Branch
                BranchHoOffice();
                //End of Rev Debashis && Implement Head Branch
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                //Session.Remove("dtLedger");


                ShowGrid.JSProperties["cpSave"] = null;
                //  string[] CallVal = e.Parameters.ToString().Split('~');

                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                string EMPVENDID = "";
                string EMPVENDTYPE = "";
                string LEDGERID = "";

                if (hdnSelectedLedger.Value != "")
                {
                    LEDGERID = hdnSelectedLedger.Value;
                }

                //GetLedgerdata(FROMDATE, TODATE, TABLENAME, BRANCH_ID, CASHBANKTYPE, CASHBANKID, CUSTVENDID, LEDGERID, ISCREATEORPREVIEW, UserId, EMPVENDID);

            }


        }
        //Rev Debashis && Implement Head Branch
        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable dt = new DataTable();
            DateTime MinDate, MaxDate;

            dt = cbl.GetDateFinancila(Finyear);
            if (dt.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((dt.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dt.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dt.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dt.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dt.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dt.Rows[0]["FinYear_StartDate"]));

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
        //End of Rev Debashis && Implement Head Branch

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

                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                //drdExport.SelectedValue = "0";
            }

        }       

        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");


            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            //drdExport.SelectedValue = "0";                       
        }

        public void bindexport(int Filter)
        {
            //string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Ledger Posting"));
            //exporter.FileName = filename;

            //exporter.PageHeader.Left = Convert.ToString((Session["Contactrequesttype"] ?? "Ledger Posting Report"));
            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            string filename = "Ledger Posting";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Ledger Posting" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 18-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    //exporter.WriteXlsxToResponse();
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        //Rev Subhra 18-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev


        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dtLedger"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsLedgerPostingFilter = Convert.ToString(hfIsLedgerPostingFilter.Value);
            Session["IsLedgerPostingFilter"] = IsLedgerPostingFilter;

            //Session.Remove("dtLedger");
            
            ShowGrid.JSProperties["cpSave"] = null;
            //string returnPara = Convert.ToString(e.Parameters);
            //string HEAD_BRANCH = returnPara.Split('~')[3];


            ////////string[] CallVal = e.Parameter.ToString().Split('~');
            ////////string type = CallVal[1];
            ////////string code = CallVal[2];
            ////////if (CallVal[1]== "null")
            ////////{
            ////////    type = "";
            ////////}
            ////////if (CallVal[2]== "null")
            ////////{
            ////////    code = "";
            ////////}
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
            //if (hdnSelectedBranches.Value != "")
            //{
            //    BRANCH_ID = hdnSelectedBranches.Value;
            //}



            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchList)
            {
                BranchComponent += "," + Branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string EMPVENDID = "";
            string EMPVENDTYPE = "";

            string LEDGERID = "";

            //if (hdnSelectedLedger.Value != "")
            //{
            //    LEDGERID = hdnSelectedLedger.Value;
            //}


            //string QuoComponent2 = "";
            //List<object> QuoList2 = lookup_quotation.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo2 in QuoList2)
            //{
            //    QuoComponent += "," + Quo2;
            //}
            //CUSTVENDID = QuoComponent2.TrimStart(',');


            //string QuoComponentledger = "";
            //List<object> QuoListledger = ASPxGridLookupledger.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo in QuoListledger)
            //{
            //    QuoComponentledger += "," + Quo;
            //}

            //LEDGERID = QuoComponentledger.TrimStart(',');







            ////////////LEDGERID = Convert.ToString(ASPxGridLookupledger.Value);
            LEDGERID = hdnSelectedLedger.Value;




            //string QuoComponent3 = "";
            //List<object> QuoList3 = grid_lookupemployee.GridView.GetSelectedFieldValues("Doc Code");

            //foreach (object Quo3 in QuoList3)
            //{
            //    QuoComponent3 += "," + Quo3;
            //}
            //EMPVENDID = QuoComponent3.TrimStart(',');

            //EMPVENDID = Convert.ToString(grid_lookupemployee.Value);
            EMPVENDID = hdnSelectedSubLedger.Value;
            EMPVENDTYPE = hdnSelectedSubLedgerType.Value;

            ////GetLedgerdata(FROMDATE, TODATE, TABLENAME, BRANCH_ID, CASHBANKTYPE, CASHBANKID, CUSTVENDID, LEDGERID, ISCREATEORPREVIEW, UserId, EMPVENDID);

            //Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, TABLENAME, BRANCH_ID, CASHBANKTYPE, CASHBANKID, CUSTVENDID, LEDGERID, ISCREATEORPREVIEW, UserId, EMPVENDID));
            //Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, TABLENAME, BRANCH_ID, CASHBANKTYPE, CASHBANKID, CUSTVENDID, LEDGERID, ISCREATEORPREVIEW, UserId, EMPVENDID, EMPVENDTYPE, HEAD_BRANCH));

            //Rev Subhra 18-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev
            
            Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, BRANCH_ID, LEDGERID, EMPVENDID, EMPVENDTYPE));
            PopulateStockTrialDataTask.RunSynchronously();
            ShowGrid.ExpandAll();
        }

        //public void GetLedgerdata(string FROMDATE, string TODATE, string TABLENAME, string BRANCH_ID, string CASHBANKTYPE, string CASHBANKID,
        //                          string CUSTVENDID, string LEDGERID, string ISCREATEORPREVIEW, string UserId, string EMPVENDID, string EMPVENDTYPE, string HEAD_BRANCH)
        public void GetLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string LEDGERID, string EMPVENDID, string EMPVENDTYPE)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                //SqlCommand cmd = new SqlCommand("PROC_LEDGERPOSTING", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@TABLENAME", TABLENAME);
                //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                //cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                //cmd.Parameters.AddWithValue("@TODATE", TODATE);
                //cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                //cmd.Parameters.AddWithValue("@CASHBANKTYPE", CASHBANKTYPE);
                //cmd.Parameters.AddWithValue("@CASHBANKID", CASHBANKID);
                //cmd.Parameters.AddWithValue("@CUSTVENDID", CUSTVENDID);
                //cmd.Parameters.AddWithValue("@EMPLOYEEID", EMPVENDID);
                //cmd.Parameters.AddWithValue("@LEDGERID", LEDGERID);
                //cmd.Parameters.AddWithValue("@ISCREATEORPREVIEW", ISCREATEORPREVIEW);
                //cmd.Parameters.AddWithValue("@USERID", UserId);

                SqlCommand cmd = new SqlCommand("PROC_LEDGERPOSTING_RUNNINGBAL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                //Rev Debashis && Implement Party Invoice Date and remove below parameters as those were not needed.
                //cmd.Parameters.AddWithValue("@CASHBANKID", CASHBANKID);
                //cmd.Parameters.AddWithValue("@USERID", UserId);
                //cmd.Parameters.AddWithValue("@CUSTVENDID", CUSTVENDID);                
                //End of Rev Debashis && Implement Party Invoice Date and remove below parameters as those were not needed.
                cmd.Parameters.AddWithValue("@EMPLOYEEID", EMPVENDID);
                cmd.Parameters.AddWithValue("@LEDGERID", LEDGERID);                
                cmd.Parameters.AddWithValue("@EMPVENDTYPE", EMPVENDTYPE);
                //Rev Debashis && Implement Party Invoice Date and remove below parameters as those were not needed.
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", chkparty.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                //End of Rev Debashis && Implement Party Invoice Date and remove below parameters as those were not needed.
                //cmd.Parameters.AddWithValue("@P_BRANCHID", HEAD_BRANCH);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dtLedger"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            //e.KeyExpression = "SEQ";
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsLedgerPostingFilter"]) == "Y")
            {
                var q = from d in dc.LEDGERPOSTRUNNINGBAL_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        //orderby d.ACLEDGERID ascending,d.SEQ ascending
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.LEDGERPOSTRUNNINGBAL_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        //orderby d.SEQ ascending
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion



        [WebMethod]
        //Rev Debashis && Implement Head Branch
        //public static List<string> GetBranchesList(String NoteId)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");
        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}
        //End of Rev Debashis && Implement Head Branch

        //[WebMethod]
        public static List<string> BindLedgerType(String Ids)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();


            //if (Ids.Trim() != "")
            //{
            //    dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

            //}         
            if (Ids == "null")
            {

                Ids = "0";
            }
            List<string> obj = new List<string>();

            obj = GetLedgerBind(Ids.Trim());

            //foreach (DataRow dr in dtbl.Rows)
            //{

            //    obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
            //}
            return obj;
        }

        [WebMethod]
        public static List<string> BindCustomerVendor()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();

            //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') AND cnt_branchid IN("+ Ids +") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV','RA') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
        }

        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dtLedger"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //        //  ShowGrid.DataBind();
        //    }

        //}
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
            Decimal total_debit = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
            Decimal total_credit = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));


            if (e.Item.FieldName == "CREDIT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "DEBIT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "NET_AMT")
            {
                e.Text = Convert.ToString(Math.Abs(total_debit - total_credit));
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
            else if (e.Item.FieldName == "CUSTOMERVENDOR")
            {
                e.Text = "Net Total";
            }
            else
            {
                //e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
                if (e.Item.FieldName == "NET_AMT")
                {
                    e.Text = Convert.ToString(Math.Abs(total_debit - total_credit));
                }
                else
                {
                    e.Text = string.Format("{0}", e.Value);
                }
               
            }
        }

        //protected void dgvVIEW_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        //{
        //    if (e.Item == ShowGrid.TotalSummary["NET_AMT"])
        //    {
        //        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //        {
        //            Decimal gmv = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
        //            Decimal equity = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));

        //            e.TotalValue = gmv - equity;
        //            e.TotalValueReady = true;
        //        }
        //    }

        //}

        #region Customer Bind

        //protected void ComponentProduct_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];


        //        string type = e.Parameter.Split('~')[1];

        //        DataTable dtbl = new DataTable();

        //        dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

        //        if (dtbl.Rows.Count > 0)
        //        {
        //            // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);

        //            Session["SI_ComponentData"] = dtbl;

        //            lookup_quotation.DataSource = dtbl;
        //            lookup_quotation.DataBind();

        //        }
        //        else
        //        {
        //            Session["SI_ComponentData"] = null;
        //            lookup_quotation.DataSource = null;
        //            lookup_quotation.DataBind();

        //        }
        //    }
        //}

        //protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        //{
        //    //   DataTable ComponentTable = new DataTable();

        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}

        #endregion

        //#region Employee Bind

        //protected void ComponentEmployee_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
        //        //string type = e.Parameter.Split('~')[1];
        //        ////////////string type = Convert.ToString(ASPxGridLookupledger.Value);
        //        string type = "";

        //        DataTable dtbl = new DataTable();
        //        if (Session["userbranchHierarchy"] != null)
        //        {
        //            if (type != "") 
        //            {
        //                //dtbl = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                // "union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType = 'CL' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Vendor' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType ='DV' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType IN('CL','DV','EM') and cnt_branchid = 0 " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //                //"master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' and MainAccount_ReferenceID = '" + type + "' ");
        //                dtbl = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                 "union ALL " +
        //                "select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type " +
        //                "FROM tbl_master_contact WHERE cnt_contactType ='EM' and cnt_branchid = 0 " +
        //                "union ALL " +
        //                "select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //                "master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' and MainAccount_ReferenceID = '" + type + "' ");
        //            }
        //            else
        //            {
        //                //dtbl = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                // "union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType = 'CL' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Vendor' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType ='DV' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //                //"FROM tbl_master_contact WHERE cnt_contactType IN('CL','DV','EM') and cnt_branchid = 0 " +
        //                //"union ALL " +
        //                //"select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //                //"master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' ");
        //                dtbl = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +                        
        //                "union ALL " +
        //                "select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type " +
        //                "FROM tbl_master_contact WHERE cnt_contactType ='EM' and cnt_branchid = 0 " +
        //                "union ALL " +
        //                "select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //                "master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' ");
        //            }
        //            //dtbl = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description'    FROM tbl_master_contact WHERE cnt_contactType in('EM')  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
                    

        //            if (dtbl.Rows.Count > 0)
        //            {


        //                grid_lookupemployee.DataSource = dtbl;
        //                grid_lookupemployee.DataBind();

        //            }
        //            else
        //            {

        //                grid_lookupemployee.DataSource = null;
        //                grid_lookupemployee.DataBind();

        //            }
        //        }
        //    }
        //}

        //protected void lookup_Employee_DataBinding(object sender, EventArgs e)
        //{
        //    //   DataTable ComponentTable = new DataTable();

        //    //string type = Convert.ToString(ASPxGridLookupledger.Value);
        //    string type = "";

        //    if (Session["userbranchHierarchy"] != null)
        //    {
        //        //grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description'    FROM tbl_master_contact WHERE cnt_contactType in('EM')  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //        //grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //        //"union ALL " +
        //        //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //        //"FROM tbl_master_contact WHERE cnt_contactType = 'CL' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //        //"union ALL " +
        //        //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Vendor' Type " +
        //        //"FROM tbl_master_contact WHERE cnt_contactType ='DV' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //        //"union ALL " +
        //        //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //        //"FROM tbl_master_contact WHERE cnt_contactType IN('CL','DV','EM') and cnt_branchid = 0 " +
        //        //"union ALL " +
        //        //"select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //        //"master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' ");

        //        if (type != "") 
        //        {
        //            //grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType = 'CL' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Vendor' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType ='DV' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType IN('CL','DV','EM') and cnt_branchid = 0 " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //            //"master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' and MainAccount_ReferenceID = '" + type + "'");
        //            grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +                    
        //            "union ALL " +
        //            "select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type " +
        //            "FROM tbl_master_contact WHERE cnt_contactType ='EM' and cnt_branchid = 0 " +
        //            "union ALL " +
        //            "select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //            "master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' and MainAccount_ReferenceID = '" + type + "'");
        //        }
        //        else
        //        {
        //            //grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType = 'CL' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Vendor' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType ='DV' and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Customer' Type " +
        //            //"FROM tbl_master_contact WHERE cnt_contactType IN('CL','DV','EM') and cnt_branchid = 0 " +
        //            //"union ALL " +
        //            //"select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //            //"master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' ");
        //            grid_lookupemployee.DataSource = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type FROM tbl_master_contact WHERE cnt_contactType ='EM'  and cnt_branchid IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") " +                    
        //            "union ALL " +
        //            "select ROW_NUMBER() over(order by cnt_id) SrlNo, cnt_id AS ID,cnt_internalId as 'Doc Code',ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Description','Employee' Type " +
        //            "FROM tbl_master_contact WHERE cnt_contactType ='EM' and cnt_branchid = 0 " +
        //            "union ALL " +
        //            "select ROW_NUMBER() over(order by A.SubAccount_ReferenceID,A.SubAccount_Name) SrlNo, A.SubAccount_ReferenceID AS ID,A.SubAccount_Code as 'Doc Code',A.SubAccount_Name as 'Description','Sub Ledger' Type FROM " +
        //            "master_subaccount A INNER JOIN Master_MainAccount B ON B.MainAccount_AccountCode=A.SubAccount_MainAcReferenceID WHERE B.MainAccount_SubLedgerType ='Custom' ");
        //        }
        //    }

        //}

        //#endregion

        #region Ledger Bind

        //protected void ComponentLedger_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {

        //        string type = e.Parameter.Split('~')[1];
        //        DataTable dtbl = new DataTable();

        //        // string Ids = e.Parameter.Split('~')[1];

        //        string Ids = "";
        //        string QuoComponent = "";
        //        List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
        //        foreach (object Quo in QuoList)
        //        {
        //            QuoComponent += "," + Quo;
        //        }
        //        Ids = QuoComponent.TrimStart(',');

        //        if (Ids == "null")
        //        {
        //            Ids = "0";
        //        }
        //        hdnSelectedLedger.Value = Ids;

        //        //CommonBL bll1 = new CommonBL();
        //        //dtbl = bll1.GetLedgerBind(Ids);
        //        //if (dtbl.Rows.Count > 0)
        //        //{
        //        //    // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);

        //        //    Session["SI_ComponentData_ledger"] = dtbl;
        //        //    ASPxGridLookupledger.DataSource = dtbl;
        //        //    ASPxGridLookupledger.DataBind();

        //        //}
        //        //else
        //        //{
        //        //    Session["SI_ComponentData_ledger"] = null;
        //        //    ASPxGridLookupledger.DataSource = null;
        //        //    ASPxGridLookupledger.DataBind();

        //        //}
        //    }
           
        //}

        //protected void lookup_ledger_DataBinding(object sender, EventArgs e)
        //{
        //    //   DataTable ComponentTable = new DataTable();

        //    if (Session["SI_ComponentData_ledger"] != null)
        //    {
        //        ASPxGridLookupledger.DataSource = (DataTable)Session["SI_ComponentData_ledger"];
        //    }
        //}

        #endregion

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                //Rev Debashis && Implement Head Branch
                //if (Session["userbranchHierarchy"] != null)
                //{
                //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
                //}
                //if (ComponentTable.Rows.Count > 0)
                //{
                //    Session["SI_ComponentData_Branch"] = ComponentTable;
                //    lookup_branch.DataSource = ComponentTable;
                //    lookup_branch.DataBind();
                //}
                //else
                //{
                //    lookup_branch.DataSource = null;
                //    lookup_branch.DataBind();
                //}
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
                //End of Rev Debashis && Implement Head Branch
            }
        }

        //Rev Debashis && Implement Head Branch
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
        //End of Rev Debashis && Implement Head Branch

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

    }
}