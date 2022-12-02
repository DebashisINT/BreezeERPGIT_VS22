using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;

using DevExpress.Web;

using BusinessLogicLayer;
namespace ERP.OMS.Reports
{

    public partial class management_journalvoucherledgeredit : System.Web.UI.Page
    {
        #region Local_Varibal
        BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable DtTable = new DataTable();
        DataTable dtReport = new DataTable();
        DataTable TempDt = new DataTable();
        static int ReptID = 0;
        static int ID = 0;
        static string ForJournalDate = null;
        string JournalEntryID = null;
        int NoofRowsAffected = 0;
        string CurrentSegment;
        string CompanyID;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        Dictionary<string, object> values = new Dictionary<string, object>();
        Boolean Lock = false;
        DataSet DsSearchCallBack;
        SqlDataAdapter DaSearchCallBack;
        string JournalVoucherFile_XMLPATH = null;
        string JournalVoucherFile_VALIDATEXMLPATH = null;
        string QS_TDate, QS_VNum, QS_CID, QS_SegID;
        //This For Log Purpose
        GenericLogSystem oGenericLogSystem;
        #endregion
        #region Page Property
        public int PCounter
        {
            get { return (int)ViewState["Counter"]; }
            set { ViewState["Counter"] = value; }
        }
        public int PBranch
        {
            get { return (int)ViewState["BranchCounter"]; }
            set { ViewState["BranchCounter"] = value; }
        }
        public int PNonBranch
        {
            get { return (int)ViewState["NonBranchCounter"]; }
            set { ViewState["NonBranchCounter"] = value; }
        }
        public decimal TotalDebit
        {
            get { return (decimal)ViewState["TotalDebit"]; }
            set { ViewState["TotalDebit"] = value; }
        }
        public decimal TotalCredit
        {
            get { return (decimal)ViewState["TotalCredit"]; }
            set { ViewState["TotalCredit"] = value; }
        }
        //Currency Setting
        public string ChoosenCurrency
        {
            get { return (string)Session["ChoosenCurrency"]; }
            set { Session["ChoosenCurrency"] = value; }
        }

        //This For Log Purpose
        public string LogID
        {
            get { return (string)ViewState["LogID"]; }
            set { ViewState["LogID"] = value; }
        }
        #endregion
        #region PageClass
        void BindGrid(ASPxGridView Grid)
        {
            TotalDebit = 0;
            TotalCredit = 0;
            Grid.DataSource = null;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataSet Ds)
        {
            TotalCredit = 0;
            TotalDebit = 0;
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
            TotalCredit = 0;
            TotalDebit = 0;
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
        void BindGrid(ASPxGridView Grid, DataSet Ds, String WhichSort)
        {
            TotalCredit = 0;
            TotalDebit = 0;
            DataView TempDV = new DataView(Ds.Tables[0]);
            TempDV.Sort = "EntryDateTime " + WhichSort;
            Grid.DataSource = TempDV;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataTable Dt, String WhichSort)
        {
            TotalCredit = 0;
            TotalDebit = 0;
            DataView TempDV = new DataView(Dt);
            TempDV.Sort = "EntryDateTime " + WhichSort;
            Grid.DataSource = TempDV;
            Grid.DataBind();
        }
        bool IsNumeric(string value)
        {
            try { Convert.ToInt32(value); return true; }
            catch { return false; }
        }
        bool IsRecordAreadyInserted(string MainAccount, string SubAccountCode, string BranchNonBranch)
        {
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                DataSet Ds_RecordExists = new DataSet();
                Ds_RecordExists.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                DataColumn[] colPk = new DataColumn[2];
                colPk[0] = Ds_RecordExists.Tables[0].Columns["JournalVoucherDetail_MainAccountCode"];
                colPk[1] = Ds_RecordExists.Tables[0].Columns["JournalVoucherDetail_SubAccountCode"];
                Ds_RecordExists.Tables[0].PrimaryKey = colPk;
                object[] findTheseVals = new object[2];
                findTheseVals[0] = MainAccount;
                if (!IsNumeric(BranchNonBranch))
                    findTheseVals[1] = SubAccountCode.Split('~')[0];
                else
                    findTheseVals[1] = SubAccountCode.Substring(0, SubAccountCode.LastIndexOf('~'));
                DataRow foundRow = Ds_RecordExists.Tables[0].Rows.Find(findTheseVals);
                if (foundRow != null)
                    return true;
                else
                    return false;
            }
            return false;
        }
        void FillSearchGrid()
        {
            DaSearchCallBack = new SqlDataAdapter();
            DsSearchCallBack = new DataSet();

            DsSearchCallBack = objFAReportsOther.Fetch_CBE_DataSet(
               Convert.ToString(Session["LastFinYear"]),
               Convert.ToString(CompanyID),
                Convert.ToString(ViewState["WhichSegment"]),
                Convert.ToString(Session["StrQuery"]));

            if (DsSearchCallBack.Tables.Count > 0)
            {
                if (DsSearchCallBack.Tables[0].Rows.Count > 0)
                {
                    BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
                }
                else
                {
                    BindGrid(GvJvSearch);
                }
            }
            else
            {
                BindGrid(GvJvSearch);
            }
        }
        void CreateJVE_XMLFile(string IBRef, string VoucherNumber)
        {
            DataSet Ds_JVE;
            SqlDataAdapter Da_JVE;
            string JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
            if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
            {
                Ds_JVE = new DataSet();
                Ds_JVE.ReadXml(Server.MapPath(JVEFile_XMLPATH));
                string UserID = Ds_JVE.Tables[0].Rows[0]["UserID"].ToString();
                string[] UserName = oDBEngine.GetFieldValue1("tbl_master_user", "User_Name", "user_id=" + UserID, 1);
                ViewState["JVE_FileAlreadyUsedBy"] = UserName[0] + "~" + UserID;
            }
            else
            {
                Ds_JVE = new DataSet();
                Ds_JVE = objFAReportsOther.Fetch_JVE_DataSet(
               Convert.ToString(ViewState["SegmentName"]),
               Convert.ToString(Session["userid"]),
                Convert.ToString(VoucherNumber),
                Convert.ToString(IBRef),
                 Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]),
                Convert.ToString(Session["LastCompany"]));

                if (Ds_JVE.Tables.Count > 0)
                {
                    if (Ds_JVE.Tables[0].Rows.Count > 0)
                    {
                        Ds_JVE.Tables[0].TableName = "DtJournalVoucher";
                        Ds_JVE.WriteXml(Server.MapPath(JVEFile_XMLPATH));
                    }
                }
            }
        }

        #endregion
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            oGenericLogSystem = new GenericLogSystem();
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            string[] segmentname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
            ViewState["SegmentName"] = segmentname[0];
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script>PageLoad();</script>");
            ViewState["DebitWhenEdit"] = null;
            ViewState["CreditWhenEdit"] = null;
            string UserPageAccess = HttpContext.Current.Session["PageAccess"].ToString();
            CurrentSegment = HttpContext.Current.Session["userlastsegment"].ToString();
            CompanyID = HttpContext.Current.Session["LastCompany"].ToString();

            QS_TDate = Request.QueryString["date"] != null ? Request.QueryString["date"] : String.Empty;
            QS_CID = Request.QueryString["Compid"] != null ? Request.QueryString["Compid"] : String.Empty;
            QS_SegID = Request.QueryString["exch"] != null ? Request.QueryString["exch"] : String.Empty;
            QS_VNum = Request.QueryString["id"] != null ? Request.QueryString["id"] : String.Empty;
            if (!IsPostBack)
            {
                string Where = null;
                TotalDebit = 0;
                TotalCredit = 0;
                //Intializing Variable
                ForJournalDate = Session["FinYearEnd"].ToString();
                if (Request.QueryString != null)
                {
                    DataTable vsegmentname = oDBEngine.GetDataTable("tbl_master_companyExchange", "isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as CompSegmentName", "exch_compID='" + QS_CID + "' and  exch_internalID=" + QS_SegID);
                    bSegmentName.InnerText = "Segment : " + vsegmentname.Rows[0][0].ToString();

                    if (QS_TDate != String.Empty && QS_CID != String.Empty && QS_SegID != String.Empty && QS_VNum != String.Empty)
                    {
                        tDate.EditFormatString = objConverter.GetDateFormat("Date");
                        tDate.Date = Convert.ToDateTime(QS_TDate);
                        Where = " JournalVoucher_TransactionDate='" + QS_TDate + "' and JournalVoucher_VoucherNumber='" + QS_VNum + "' and JournalVoucher_CompanyID='" + QS_CID + "' and JournalVoucher_ExchangeSegmentID=" + QS_SegID;
                        TempDt = oDBEngine.GetDataTable("Trans_JournalVoucher", "JournalVoucher_VoucherNumber,JournalVoucher_IBRef,JournalVoucher_InterBranch", Where);
                        if (TempDt.Rows.Count > 0)
                        {
                            Session["IBRef"] = TempDt.Rows[0]["JournalVoucher_IBRef"].ToString();
                            Session["WhichTypeItem"] = TempDt.Rows[0]["JournalVoucher_InterBranch"].ToString();
                            Session["VoucherNumber"] = TempDt.Rows[0]["JournalVoucher_VoucherNumber"].ToString();
                            ViewState["WhichSegment"] = QS_SegID;
                            hdnSegmentid.Value = QS_SegID;

                        }
                    }
                }
                if (Session["StrQuery"] != null) Session["StrQuery"] = null;
                //This For Log Purpose
                ViewState["LogID"] = oGenericLogSystem.GetLogID();

                //Currency Setting
                this.Page.ClientScript.RegisterStartupScript(GetType(), "CS", "<script>PageLoad_ForCurrency();</script>");

                //End Intializing Variable

                //Currency Setting
                B_ChoosenCurrency.InnerText = "Voucher Currency : " + Session["ActiveCurrency"].ToString().Split('~')[1].Trim() + "[" +
                       Session["ActiveCurrency"].ToString().Split('~')[2].Trim() + "]";
                if (!CbpChoosenCurrency.IsCallback)
                {
                    if (Session["LocalCurrency"].ToString().Trim() != Session["TradeCurrency"].ToString().Trim())
                    {
                        B_ChoosenCurrency.Attributes.Add("onclick", "ChangeCurrency()");
                        B_ChoosenCurrency.Style.Add("cursor", "hand");
                    }
                }
            }
            if (Session["StrQuery"] != null) FillSearchGrid();
            Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>Page_Load1()</script>");
            hdnCompanyid.Value = HttpContext.Current.Session["LastCompany"].ToString();
            if (Session["IBRef"] != null)
            {
                JournalVoucherFile_XMLPATH = "../Documents/" + "JVE_" + Session["IBRef"].ToString();
                JournalVoucherFile_VALIDATEXMLPATH = "../Documents/" + "JVE_" + Session["IBRef"].ToString() + "Validate";
            }
            DataTable DtWhichSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");

            OnLoadBindGrid();
        }

        public void OnLoadBindGrid()
        {
            DataSet DsOnLoad = new DataSet();
            DataSet DsOnLoadValidation = new DataSet();
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
                DsOnLoad.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                BindGrid(DetailsGrid, DsOnLoad, "DESC");
                if (DsOnLoad.Tables.Count > 0)
                {
                    ViewState["BillNo"] = DsOnLoad.Tables[0].Rows[0][22].ToString();
                    ViewState["JvNarration"] = DsOnLoad.Tables[0].Rows[0][8].ToString();
                    ViewState["BranchSelectedValue"] = DsOnLoad.Tables[0].Rows[0][23].ToString();
                    ViewState["Prefix"] = DsOnLoad.Tables[0].Rows[0][19].ToString();
                    //Currency Setting
                    DetailsGrid.JSProperties["cpSetCurrencyNameSymbol"] = null;
                    ChoosenCurrency = DsOnLoad.Tables[0].Rows[0]["ChoosenCurrency"].ToString();
                    DetailsGrid.JSProperties["cpSetCurrencyNameSymbol"] = ChoosenCurrency.ToString().Split('~')[1].Trim() + "~" +
                        ChoosenCurrency.Split('~')[2].Trim();
                }
            }
            else
            {
                BindGrid(DetailsGrid);
                ViewState["BillNo"] = null;
                ViewState["JvNarration"] = null;
                ViewState["BranchSelectedValue"] = null;
                ViewState["Prefix"] = null;
                //Currency Setting
                ChoosenCurrency = Session["ActiveCurrency"].ToString();
            }
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                if (DsOnLoadValidation.Tables.Count > 0) { DsOnLoadValidation.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoadValidation.Clear(); }
                DsOnLoadValidation.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                if (DsOnLoadValidation.Tables.Count > 0)
                {
                    if (DsOnLoadValidation.Tables[0].Columns.Count > 3)
                    {
                        File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        foreach (DataRow row in DsOnLoad.Tables[0].Rows)
                            AddData_EntryValidationXML(row["BranchNonBranch"].ToString());
                    }
                }

            }
            else
            {
                if (DsOnLoad.Tables.Count > 0)
                {
                    if (DsOnLoad.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in DsOnLoad.Tables[0].Rows)
                            AddData_EntryValidationXML(row["BranchNonBranch"].ToString());
                    }
                }
            }

            DsOnLoad.Dispose();
        }

        protected void grdAdd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }



        [System.Web.Services.WebMethod]
        public static string GetContactName(string custid)
        {
            string closingBalance = null;
            management_journalvoucherledgeredit objPage = new management_journalvoucherledgeredit();
            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();
            Converter objConverter1 = new Converter();
            string[] dateandparam = custid.Split('~');
            // DateTime date = Convert.ToDateTime(objConverter1.DateConverter1(dateandparam[2].ToString(), "mm/dd/yyyy"));

            DateTime date = Convert.ToDateTime(ForJournalDate);

            string[,] mainacc = oDbEngine1.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + dateandparam[3] + "", 1);
            string mainaccCode = mainacc[0, 0];
            mainaccCode = "'" + mainaccCode + "'";
            string SubAccID = dateandparam[4];
            DataTable dtClose = oDbEngine1.OpeningBalanceJournal1(mainaccCode, SubAccID, date, dateandparam[1], dateandparam[0], date, Convert.ToInt32(objPage.ChoosenCurrency.Split('~')[0]));
            closingBalance = dtClose.Rows[0][0].ToString();
            return closingBalance;
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            DataSet DSJournalReturn = new DataSet();
            DSJournalReturn = (DataSet)Session["DSJournalReturn"];
            DataTable DtJournalReturn = DSJournalReturn.Tables[0];
            DateTime TranDate = Convert.ToDateTime(DtJournalReturn.Rows[0][3].ToString()).Date;
            string tabledata = objConverter.ConvertDataTableToXML(DtJournalReturn);

            dsCrystal = objFAReportsOther.JournalVoucherPrintFromInsert(
         Convert.ToString(tabledata),
        TranDate
         );


            dsCrystal.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Journal.xsd");
            string[] connPath =Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\Reports\\Journal.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(dsCrystal);
            //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Journal");
            reportObj.Dispose();
            GC.Collect();
            Session["DSJournalReturn"] = null;

        }
        protected void DetailsGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            decimal diff = 0.00M; ;
            string RemainingCredit = "0.00";
            string RemainingDebit = "0.00";
            if (ViewState["CreditWhenEdit"] != null)
            {
                if (ViewState["CreditWhenEdit"].ToString().Trim() != "0.00")
                {
                    TotalCredit = Convert.ToDecimal(ViewState["CreditWhenEdit"].ToString()) + TotalCredit;
                    TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
                    TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
                }
            }


            if (ViewState["DebitWhenEdit"] != null)
            {
                if (ViewState["DebitWhenEdit"].ToString().Trim() != "0.00")
                {
                    TotalDebit = Convert.ToDecimal(ViewState["DebitWhenEdit"].ToString()) + TotalDebit;
                    TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
                    TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
                }
            }

            if (ViewState["DebitWhenEdit"] == null && ViewState["CreditWhenEdit"] == null)
            {
                TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
                TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
            }


            if (TotalDebit < TotalCredit)
            {
                diff = TotalCredit - TotalDebit;
                RemainingCredit = "0.00";
                RemainingDebit = diff.ToString(); ;
                tdSaveButton.Visible = false;
            }
            else if (TotalDebit > TotalCredit)
            {
                diff = TotalDebit - TotalCredit;
                RemainingCredit = diff.ToString();
                RemainingDebit = "0.00";
                tdSaveButton.Visible = false;
            }
            else
            {
                RemainingCredit = "0.00";
                RemainingDebit = "0.00";
                tdSaveButton.Visible = true;
            }
            ViewState["CreditWhenEdit"] = "0.00";
            ViewState["DebitWhenEdit"] = "0.00";
            e.Properties["cpTotalDebitCredit"] = TotalDebit + "~" + TotalCredit + "~" + RemainingDebit + "~" + RemainingCredit;

        }
        protected void DetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Add")
            {
                string strSplitBranch = e.Parameters.Split('~')[1];
                //if (!IsRecordAreadyInserted(txtMainAccount_hidden.Value, txtSubAccount_hidden.Value, strSplitBranch))
                //{
                Boolean AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntry(strSplitBranch).Split('~')[0]);
                if (AllowDisAllow)
                {
                    try
                    {
                        AddData_ToGrid(strSplitBranch);
                        AddData_EntryValidationXML(strSplitBranch);
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlAdd, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlAdd, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                    catch
                    {
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                    DetailsGrid.JSProperties["cpEntryNotAllow"] = "Empty";
                    DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                    DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                    DetailsGrid.JSProperties["cpEntryData"] = "undefined";
                }
                else
                {
                    string Reason = AllowDisAllowEntry(strSplitBranch).Split('~')[1];
                    if (Reason == "Branch") DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Non-Branch Accounts For This Entry";
                    else DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Branch Accounts For This Entry";
                    DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                    DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                    DetailsGrid.JSProperties["cpEntryData"] = "undefined";
                }
                //}
                //else
                //{
                //    DetailsGrid.JSProperties["cpEntryNotAllow"] = "Record Already Exist.";
                //}

            }
            if (strSplitCommand == "Save")
            {
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = Save_Records();
                DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
            if (strSplitCommand == "Discard")
            {
                DetailsGrid.JSProperties["cpSuccessDiscard"] = Discard_All();
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
            if (strSplitCommand == "New")
            {

            }
            if (strSplitCommand == "Entry")
            {
                EntryButtonClick();
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpDrCrAfterAdd"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "EntryData";
                DetailsGrid.JSProperties["cpBillNo"] = ViewState["BillNo"] != null ? ViewState["BillNo"].ToString() : "EmptyString";
                DetailsGrid.JSProperties["cpJvNarration"] = ViewState["JvNarration"] != null ? ViewState["JvNarration"].ToString() : "EmptyString";
                DetailsGrid.JSProperties["cpBranchSelectedValue"] = ViewState["BranchSelectedValue"] != null ? ViewState["BranchSelectedValue"].ToString() : "EmptyString";
                DetailsGrid.JSProperties["cpPrefix"] = ViewState["Prefix"] != null ? ViewState["Prefix"].ToString() : "EmptyString";



            }
            if (strSplitCommand == "DiscardOnRefresh")
            {
                ViewState["BillNo"] = null;
                ViewState["JvNarration"] = null;
                ViewState["BranchSelectedValue"] = null;
                ViewState["Prefix"] = null;
                Discard_All();
                Session["IBRef"] = null;
                Session["OldJVVoucherNumber"] = null;
                Session["OldWhichTypeItem"] = null;
            }


        }
        void AddData_ToGrid(string BranchID)
        {
            DataSet DsAddXML = new DataSet();
            string Segment = "";
            string MainAccID = null;
            string[] mainAccountID = txtMainAccount_hidden.Value.ToString().Split('~');
            if (mainAccountID.Length > 1)
                MainAccID = mainAccountID[0];
            else
                MainAccID = txtMainAccount_hidden.Value.ToString();
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
                DsAddXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                if (DsAddXML.Tables[0].Rows.Count > 0)
                {
                    PCounter = Convert.ToInt32(DsAddXML.Tables[0].Rows[DsAddXML.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;

                }
                DataRow drReport = DsAddXML.Tables[0].NewRow();
                drReport[0] = PCounter;
                drReport[1] = String.Empty;
                drReport[2] = QS_SegID;
                drReport[3] = (BranchID != "NAB") ? BranchID : ddlBranch.SelectedItem.Value.ToString();
                drReport[4] = MainAccID;
                drReport[5] = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                drReport[6] = txtdebit.Text;
                drReport[7] = txtcredit.Text;
                drReport[11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtdebit.Text));
                drReport[12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtcredit.Text));
                drReport[8] = txtNarration.Text != "Type Main Narration Here" ? txtNarration.Text : String.Empty;
                drReport[10] = txtMainAccount.Text.ToString();
                drReport[9] = txtSubAccount.Text.ToString();
                drReport[13] = tDate.Value;
                drReport[14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
                string[] MainAC = txtMainAccount_hidden.Value.ToString().Split('~');
                DataTable dtType = new DataTable();
                if (MainAC.Length > 1)
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + MainAC[0].ToString() + "");
                else
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccount_hidden.Value.ToString() + "");
                drReport[15] = dtType.Rows[0][0].ToString();
                drReport[16] = Session["userid"].ToString();
                drReport[17] = oDBEngine.GetDate();
                drReport[18] = hdn_Brch_NonBrch.Value;
                drReport[19] = txtAccountCode.Text;
                drReport[20] = txtSubAccount_hidden.Value != "" ? txtSubAccount_hidden.Value.Split('~')[3] : String.Empty;
                drReport[21] = txtNarration1.Text.Trim();
                drReport[22] = txtBillNo.Text;
                drReport[23] = ddlBranch.SelectedIndex;
                drReport[24] = String.Empty;
                drReport[25] = "GenerateNew";
                drReport[26] = String.Empty;
                //Currency Setting
                drReport[27] = ChoosenCurrency;
                DsAddXML.Tables[0].Rows.Add(drReport);
                DsAddXML.Tables[0].AcceptChanges();
                DsAddXML.Tables[0].WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                DsAddXML.Dispose();
                txtSubAccount_hidden.Value = String.Empty;
                hdn_Brch_NonBrch.Value = String.Empty;
            }
            else
            {


                if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
                dtReport = DsAddXML.Tables.Add();
                dtReport.Columns.Add(new DataColumn("CashReportID", typeof(int))); //0
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ID", typeof(String)));//1
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ExchangeSegmentID", typeof(String)));//2
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_BranchID", typeof(String)));//3
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_MainAccountCode", typeof(String)));//4
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_SubAccountCode", typeof(String)));//5
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountDr", typeof(Decimal)));//6
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountCr", typeof(Decimal)));//7
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_Narration", typeof(String)));//8
                dtReport.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//9
                dtReport.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//10
                dtReport.Columns.Add(new DataColumn("WithDrawl", typeof(string)));//11
                dtReport.Columns.Add(new DataColumn("Receipt", typeof(string)));//12
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate", typeof(DateTime)));//13
                dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate1", typeof(string)));//14
                dtReport.Columns.Add(new DataColumn("Type", typeof(string)));//15
                dtReport.Columns.Add(new DataColumn("UserID", typeof(string)));//16
                dtReport.Columns.Add(new DataColumn("EntryDateTime", typeof(DateTime)));//17
                dtReport.Columns.Add(new DataColumn("BranchNonBranch", typeof(string)));//18
                dtReport.Columns.Add(new DataColumn("Prefix", typeof(string)));//19
                dtReport.Columns.Add(new DataColumn("SubAccountName", typeof(string)));//20
                dtReport.Columns.Add(new DataColumn("SubNarration", typeof(string)));//21
                dtReport.Columns.Add(new DataColumn("BillNo", typeof(string)));//22
                dtReport.Columns.Add(new DataColumn("DDLSelectedBranch", typeof(string)));//23
                dtReport.Columns.Add(new DataColumn("OldJVTranDate", typeof(string)));//24
                dtReport.Columns.Add(new DataColumn("OldJVVoucherNumber", typeof(string)));//25
                dtReport.Columns.Add(new DataColumn("OldWhichTypeItem", typeof(string)));//26
                //Currency Setting
                dtReport.Columns.Add(new DataColumn("ChoosenCurrency", typeof(string)));//27 To Remember Last ChoosenCurrency

                DataRow drReport = dtReport.NewRow();
                drReport[0] = 1;
                drReport[1] = String.Empty;
                drReport[2] = QS_SegID;
                drReport[3] = (BranchID != "NAB") ? BranchID : ddlBranch.SelectedItem.Value.ToString();
                drReport[4] = MainAccID;
                drReport[5] = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                drReport[6] = txtdebit.Text;
                drReport[7] = txtcredit.Text;
                drReport[11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtdebit.Text));
                drReport[12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtcredit.Text));
                drReport[8] = txtNarration.Text != "Type Main Narration Here" ? txtNarration.Text : String.Empty;
                drReport[10] = txtMainAccount.Text.ToString();
                drReport[9] = txtSubAccount.Text.ToString();
                drReport[13] = tDate.Value;
                drReport[14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
                string[] MainAC = txtMainAccount_hidden.Value.ToString().Split('~');
                DataTable dtType = new DataTable();
                if (MainAC.Length > 1)
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + MainAC[0].ToString() + "");
                else
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccount_hidden.Value.ToString() + "");
                drReport[15] = dtType.Rows[0][0].ToString();
                drReport[16] = Session["userid"].ToString();
                drReport[17] = oDBEngine.GetDate();
                drReport[18] = hdn_Brch_NonBrch.Value;
                drReport[19] = txtAccountCode.Text;
                drReport[20] = txtSubAccount_hidden.Value != "" ? txtSubAccount_hidden.Value.Split('~')[3] : String.Empty;
                drReport[21] = txtNarration1.Text.Trim();
                drReport[22] = txtBillNo.Text;
                drReport[23] = ddlBranch.SelectedIndex;
                drReport[24] = String.Empty;
                drReport[25] = "GenerateNew";
                drReport[24] = String.Empty;
                drReport[25] = String.Empty;
                drReport[26] = String.Empty;
                //Currency Setting
                drReport[27] = ChoosenCurrency;
                dtReport.Rows.Add(drReport);
                dtReport.AcceptChanges();
                DsAddXML.Tables[0].TableName = "DtJournalVoucher";
                DsAddXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));

            }
            BindGrid(DetailsGrid, DsAddXML, "DESC");
            txtSubAccount_hidden.Value = String.Empty;
            hdn_Brch_NonBrch.Value = String.Empty;
            txtMainAccount_hidden.Value = String.Empty;
            DsAddXML.Dispose();
        }
        string Save_Records()
        {
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
            string StrAutoGeneration = oconverter.GetAutoGenerateNo();
            if (ViewState["WhichSegment"] != null)
            {
                DataSet DsSaveRecordXML = new DataSet();
                DataSet DsSaveRecordXMLValidation = new DataSet();
                DataSet DsFetchToPrint;
                SqlDataAdapter DaFetchToPrint;
                string WhichTypeItemsExist = null;
                int NonBranchItemsCount, BranchItemsCount;
                if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                {
                    if (DsSaveRecordXML.Tables.Count > 0) { DsSaveRecordXML.Tables.Remove(DsSaveRecordXML.Tables[0]); DsSaveRecordXML.Clear(); }
                    DsSaveRecordXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                    if (DsSaveRecordXML.Tables.Count > 0)
                    {
                        if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < DsSaveRecordXML.Tables[0].Rows.Count; i++)
                            {
                                DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_Narration"] = txtNarration.Text.Trim() != String.Empty ? txtNarration.Text : String.Empty;
                                DsSaveRecordXML.Tables[0].Rows[i]["SubNarration"] = DsSaveRecordXML.Tables[0].Rows[i]["SubNarration"].ToString().Trim();
                                DsSaveRecordXML.Tables[0].Rows[i]["BillNo"] = txtBillNo.Text.Trim() != String.Empty ? txtBillNo.Text : String.Empty;
                                DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_ValueDate"] = tDate.Value;
                                DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_ValueDate1"] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
                            }
                            DsSaveRecordXML.AcceptChanges();
                        }
                    }
                }
                if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                {
                    if (DsSaveRecordXMLValidation.Tables.Count > 0) { DsSaveRecordXMLValidation.Tables.Remove(DsSaveRecordXMLValidation.Tables[0]); DsSaveRecordXMLValidation.Clear(); }
                    DsSaveRecordXMLValidation.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    NonBranchItemsCount = Convert.ToInt32(DsSaveRecordXMLValidation.Tables[0].Rows[1][1].ToString());
                    BranchItemsCount = Convert.ToInt32(DsSaveRecordXMLValidation.Tables[0].Rows[0][1].ToString());
                    if (NonBranchItemsCount == 0 && BranchItemsCount > 0)
                        WhichTypeItemsExist = "B";
                    else if (BranchItemsCount == 0 && NonBranchItemsCount > 0)
                        WhichTypeItemsExist = "NB";
                    else if (NonBranchItemsCount == 1 && BranchItemsCount > 0)
                        WhichTypeItemsExist = "BGN";
                    else
                        WhichTypeItemsExist = "NGB";

                }

                if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
                {
                    string IBRef = string.Empty;
                    DsFetchToPrint = new DataSet();
                    string OldVoucherNumber = string.Empty;
                    string OldIBRef = string.Empty;
                    string OldWhichTypeItem = string.Empty;
                    OldVoucherNumber = (Session["VoucherNumber"] != null) ? Session["VoucherNumber"].ToString() : String.Empty;
                    OldIBRef = (Session["IBRef"] != null) ? Session["IBRef"].ToString() : String.Empty;
                    OldWhichTypeItem = (Session["WhichTypeItem"] != null) ? Session["WhichTypeItem"].ToString() : String.Empty;
                    // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    // {
                    //  using (SqlCommand com = new SqlCommand("Insert_JournalVoucherEntry", con))
                    // {

                    DsFetchToPrint = oOtherTasks.InsertJournalVoucherEntry(DsSaveRecordXML.GetXml(), Session["userid"].ToString(), Session["LastFinYear"].ToString(), CompanyID,
                      Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), txtBillNo.Text,
                     txtAccountCode.Text, ViewState["WhichSegment"].ToString().Trim(), WhichTypeItemsExist, StrAutoGeneration, Session["EntryProfileType"].ToString(),
                      "Edit", OldIBRef, OldVoucherNumber, OldWhichTypeItem, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));

                    //This For Log Purpose
                    if (Session["IBRef"] != null)
                    {
                        IBRef = Session["IBRef"].ToString();
                        oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }


                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    //  }
                    if (DsFetchToPrint.Tables.Count > 0)
                    {
                        if (DsFetchToPrint.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                                File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            }
                            catch
                            {
                                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                                File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            }
                            BindGrid(DetailsGrid);
                            Session["DSJournalReturn"] = DsFetchToPrint;
                            Session["StrQuery"] = null;
                            Session["IBRef"] = null;
                            Session["VoucherNumber"] = null;
                            Session["WhichTypeItem"] = null;

                            //Currency Setting
                            Session["ActiveCurrency"] = Session["TradeCurrency"];
                            return "Success";

                        }
                    }
                    else
                    {
                        //This For Log Purpose
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), StrAutoGeneration, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        return "Problem";
                    }
                    //  }
                    // }

                }
                DsSaveRecordXMLValidation.Dispose();
                DsSaveRecordXML.Dispose();
                //This For Log Purpose
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), StrAutoGeneration, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                return "Problem";
            }
            else
            {
                //This For Log Purpose
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), StrAutoGeneration, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                return "Problem";
            }

        }
        string Discard_All()
        {
            string IBRef = Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty;
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                try
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                    File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                }
                catch
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                    File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                }
                BindGrid(DetailsGrid);
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                return "SuccessDiscard";
            }
            return "Problem";
        }
        protected void DetailsGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            TotalDebit = 0;
            TotalCredit = 0;
        }
        void EntryButtonClick()
        {
            if (TotalDebit < TotalCredit)
            {
                tdSaveButton.Visible = false;
            }
            else if (TotalDebit > TotalCredit)
            {
                tdSaveButton.Visible = false;
            }
            else if (TotalDebit == TotalCredit)
            {
                tdSaveButton.Visible = true;
            }
            else
            {

            }
        }
        void AddData_EntryValidationXML(string branchID)
        {
            DataSet DsEntryValidXML = new DataSet();
            Boolean RecordExist = false;
            string BranchIDContant = String.Empty;
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                if (DsEntryValidXML.Tables.Count > 0) { DsEntryValidXML.Tables.Remove(DsEntryValidXML.Tables[0]); DsEntryValidXML.Tables[0].Clear(); }
                DsEntryValidXML.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                if (DsEntryValidXML.Tables.Count > 0)
                {
                    if (DsEntryValidXML.Tables[0].Rows.Count > 0)
                    {
                        PCounter = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[DsEntryValidXML.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                    }
                }
                if (DsEntryValidXML.Tables[0].Rows.Count > 0)
                {
                    if (branchID == "NAB")
                    {
                        DsEntryValidXML.Tables[0].Rows[1][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[1][1].ToString()) + 1;
                        DsEntryValidXML.Tables[0].AcceptChanges();
                    }
                    else
                    {
                        for (int i = 2; i < DsEntryValidXML.Tables[0].Rows.Count; i++)
                        {
                            BranchIDContant = DsEntryValidXML.Tables[0].Rows[i][0].ToString();
                            if (branchID == BranchIDContant)
                            {
                                RecordExist = true;
                                DsEntryValidXML.Tables[0].Rows[i][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[i][1].ToString()) + 1;
                                DsEntryValidXML.Tables[0].AcceptChanges();
                                break;
                            }
                        }
                        if (!RecordExist)
                        {
                            if (IsNumeric(branchID))
                            {
                                DataRow DrValidEntry = DsEntryValidXML.Tables[0].NewRow();
                                DsEntryValidXML.Tables[0].Rows[0][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[0][1].ToString()) + 1;
                                DsEntryValidXML.Tables[0].AcceptChanges();
                                DrValidEntry[0] = branchID;
                                DrValidEntry[1] = 1;
                                DrValidEntry[2] = PCounter;
                                DsEntryValidXML.Tables[0].Rows.Add(DrValidEntry);
                            }
                        }
                    }
                }
                DsEntryValidXML.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                hdn_Brch_NonBrch.Value = String.Empty;
            }
            else
            {
                if (DsEntryValidXML.Tables.Count > 0) { DsEntryValidXML.Tables.Remove(DsEntryValidXML.Tables[0]); DsEntryValidXML.Tables[0].Clear(); }
                DataTable DtValidEntry = DsEntryValidXML.Tables.Add();
                DtValidEntry.Columns.Add(new DataColumn("ID", typeof(string))); //0
                DtValidEntry.Columns.Add(new DataColumn("EntryCount", typeof(int)));//1
                DtValidEntry.Columns.Add(new DataColumn("CashReportID", typeof(int))); //2
                //DtValidEntry.Columns["CashReportID"].AutoIncrement = true;
                //DtValidEntry.Columns["CashReportID"].AutoIncrementSeed = 1;
                DataRow DrValidEntry = DtValidEntry.NewRow();
                DrValidEntry[0] = "BR";
                DrValidEntry[1] = 0;
                DrValidEntry[2] = 1;
                DtValidEntry.Rows.Add(DrValidEntry);
                DrValidEntry = DtValidEntry.NewRow();
                DrValidEntry[0] = "NB";
                DrValidEntry[1] = 0;
                DrValidEntry[2] = 2;
                DtValidEntry.Rows.Add(DrValidEntry);
                if (IsNumeric(branchID))
                {
                    DrValidEntry = DtValidEntry.NewRow();
                    DrValidEntry[0] = branchID;
                    DrValidEntry[1] = 1;
                    DrValidEntry[2] = 3;
                    DtValidEntry.Rows.Add(DrValidEntry);
                    DtValidEntry.Rows[0][1] = Convert.ToInt32(DtValidEntry.Rows[0][1].ToString()) + 1;
                    DtValidEntry.AcceptChanges();
                }
                else
                {
                    DrValidEntry = DtValidEntry.NewRow();
                    DtValidEntry.Rows[1][1] = Convert.ToInt32(DtValidEntry.Rows[1][1].ToString()) + 1;
                    DtValidEntry.AcceptChanges();
                }
                DsEntryValidXML.Tables[0].TableName = "JvValidEntry";
                DsEntryValidXML.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                hdn_Brch_NonBrch.Value = String.Empty;
            }
            DsEntryValidXML.Dispose();
        }
        string AllowDisAllowEntry(string branchID)
        {
            DataSet DsAllowDisAllowEntry = new DataSet();
            int BranchCount, NonBranchCount;
            Boolean NotInBranchCount = true;
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                DsAllowDisAllowEntry.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                DsAllowDisAllowEntry.Tables[0].PrimaryKey = new DataColumn[] { DsAllowDisAllowEntry.Tables[0].Columns["ID"] };
                DataRow row = DsAllowDisAllowEntry.Tables[0].Rows.Find(branchID);
                if (row != null)
                {
                    NotInBranchCount = false;
                }
                BranchCount = Convert.ToInt32(DsAllowDisAllowEntry.Tables[0].Rows[0][1].ToString());
                NonBranchCount = Convert.ToInt32(DsAllowDisAllowEntry.Tables[0].Rows[1][1].ToString());
                DsAllowDisAllowEntry.Dispose();
                if (IsNumeric(branchID))
                {
                    if (NonBranchCount > 1)
                        if (BranchCount == 1 && NotInBranchCount)
                            return "false~Branch";
                        else
                            return "true~";
                    else
                        return "true~";
                }
                else
                {
                    if (BranchCount > 1)
                        if (NonBranchCount == 1)
                            return "false~NonBranch";
                        else
                            return "true~";
                    else
                        return "true~";
                }

            }
            return "true~";

        }
        string AllowDisAllowEntryWhenNAB(string DiffbranchIDOrNonBranch, string EditObjId)
        {
            int RowEntryCount = 0;
            Boolean NotInBranchCount = true;
            DataSet DsEntryWhenNAB = new DataSet();
            if (DsEntryWhenNAB.Tables.Count > 0) { DsEntryWhenNAB.Tables.Remove(DsEntryWhenNAB.Tables[0]); DsEntryWhenNAB.Clear(); }
            int BranchCount = 0, NonBranchCount = 0;
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                DsEntryWhenNAB.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                DsEntryWhenNAB.Tables[0].PrimaryKey = new DataColumn[] { DsEntryWhenNAB.Tables[0].Columns["ID"] };
                if (DiffbranchIDOrNonBranch != "NAB" && EditObjId == "NAB")
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                    NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString()) - 1;
                }
                else if (DiffbranchIDOrNonBranch == "NAB" && EditObjId != "NAB")
                {
                    DataRow row = DsEntryWhenNAB.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                    if (row != null)
                    {
                        RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                        if (RowEntryCount == 1)
                        {
                            BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString()) - 1;
                            NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                        }
                        else
                        {
                            BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                            NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                        }
                    }
                    else
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                        NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                    }
                }
                else
                {
                    NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                    BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                }
                DataRow row1 = DsEntryWhenNAB.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                if (row1 != null)
                {
                    NotInBranchCount = false;
                }
            }
            DsEntryWhenNAB.Dispose();

            if (IsNumeric(DiffbranchIDOrNonBranch))
            {
                if (NonBranchCount > 1 && NotInBranchCount)
                    if (BranchCount == 1)
                        return "false~Branch";
                    else
                        return "true~";
                else
                    return "true~";
            }
            else
            {
                if (BranchCount > 1)
                    if (NonBranchCount == 1)
                        return "false~NonBranch";
                    else
                        return "true~";
                else
                    return "true~";
            }


        }
        string AllowDisAllowEntryWhenBranch(string DiffbranchIDOrNonBranch, string EditObjId)
        {
            DataSet DsEntryWhenBranch;
            int BranchCount = 0, NonBranchCount = 0;
            int RowEntryCount;
            Boolean NotInBranchCount = true;
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                DsEntryWhenBranch = new DataSet();
                if (DsEntryWhenBranch.Tables.Count > 0) { DsEntryWhenBranch.Tables.Remove(DsEntryWhenBranch.Tables[0]); DsEntryWhenBranch.Clear(); }
                DsEntryWhenBranch.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                DsEntryWhenBranch.Tables[0].PrimaryKey = new DataColumn[] { DsEntryWhenBranch.Tables[0].Columns["ID"] };
                DataRow row = DsEntryWhenBranch.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                if (row != null)
                {
                    RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                    if (RowEntryCount == 1)
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString()) - 1;
                        NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                    }
                    else
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                        NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                    }
                }
                else
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                    if (BranchCount == 1)
                    {
                        row = DsEntryWhenBranch.Tables[0].Rows.Find(EditObjId);
                        if (row != null)
                        {
                            RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                            if (RowEntryCount == 1)
                            {
                                BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString()) - 1;
                                NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                            }
                            else
                            {
                                BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                                NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                            }
                        }
                    }
                    else
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                        NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                    }
                    DataRow row1 = DsEntryWhenBranch.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                    if (row1 != null)
                    {
                        NotInBranchCount = false;
                    }
                }
                DsEntryWhenBranch.Dispose();
                if (IsNumeric(DiffbranchIDOrNonBranch))
                {
                    if (NonBranchCount > 1 && NotInBranchCount)
                        if (BranchCount == 1)
                            return "false~Branch";
                        else
                            return "true~";
                    else
                        return "true~";
                }
                else
                {
                    if (BranchCount > 1)
                        if (NonBranchCount == 1)
                            return "false~NonBranch";
                        else
                            return "true~";
                    else
                        return "true~";
                }
            }
            return "true~";

        }
        protected void DetailsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataSet DsRowDeleteXML = new DataSet();
            DataSet DsRowDeleteXMLValid = new DataSet();
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                if (DsRowDeleteXML.Tables.Count > 0) { DsRowDeleteXML.Tables.Remove(DsRowDeleteXML.Tables[0]); DsRowDeleteXML.Clear(); }
                DsRowDeleteXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                DsRowDeleteXML.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXML.Tables[0].Columns["CashReportID"] };
                DataRow row = DsRowDeleteXML.Tables[0].Rows.Find(e.Keys["CashReportID"]);
                string branchNonBranch = row["BranchNonBranch"].ToString();
                DsRowDeleteXML.Tables[0].Rows.Remove(row);
                e.Cancel = true;
                BindGrid(DetailsGrid, DsRowDeleteXML, "DESC");
                try
                {
                    if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                        if (DsRowDeleteXML.Tables.Count > 0)
                        {
                            if (DsRowDeleteXML.Tables[0].Rows.Count > 0)
                            {
                                DsRowDeleteXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                            }
                        }
                    }
                    if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                    {
                        DsRowDeleteXMLValid = new DataSet();
                        DsRowDeleteXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        DsRowDeleteXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXMLValid.Tables[0].Columns["ID"] };
                        if (branchNonBranch == "NAB")
                        {
                            row = DsRowDeleteXMLValid.Tables[0].Rows.Find("NB");
                            row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                            row.AcceptChanges();
                        }
                        else
                        {
                            row = DsRowDeleteXMLValid.Tables[0].Rows.Find(branchNonBranch);
                            row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                            row.AcceptChanges();
                            if (row["EntryCount"].ToString() == "0")
                            {
                                DsRowDeleteXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsRowDeleteXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                DsRowDeleteXMLValid.Tables[0].Rows.Remove(row);
                                DsRowDeleteXMLValid.AcceptChanges();
                            }
                        }
                        if (DsRowDeleteXMLValid.Tables.Count > 0)
                        {
                            if (DsRowDeleteXMLValid.Tables[0].Rows.Count > 0)
                            {
                                DsRowDeleteXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            }
                        }
                    }
                    if (DetailsGrid.VisibleRowCount == 0)
                    {
                        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                        }
                        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                    }

                    TotalCredit = 0;
                    TotalDebit = 0;
                }
                catch
                {
                    DsRowDeleteXMLValid.Dispose();
                    DsRowDeleteXML.Dispose();
                }
            }
            else
            {
                if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                }
            }
            DsRowDeleteXMLValid.Dispose();
            DsRowDeleteXML.Dispose();
        }
        protected void DetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            TotalCredit = 0;
            TotalDebit = 0;
            int rowindex = DetailsGrid.EditingRowVisibleIndex;
            //EditRecord BranchNonBranchStatus
            string BranchNonBranchE = DetailsGrid.GetRowValues(rowindex, "BranchNonBranch").ToString();
            //EditRecord MainAccount
            string MainAccountE = DetailsGrid.GetRowValues(rowindex, "MainAccount1").ToString();
            //EditRecord MainAccount
            string SubAccountE = DetailsGrid.GetRowValues(rowindex, "SubAccount1").ToString();
            //EditRecord Debit
            string DebitE = DetailsGrid.GetRowValues(rowindex, "WithDrawl").ToString();
            ViewState["DebitWhenEdit"] = DebitE;
            //EditRecord Credit
            string CreditE = DetailsGrid.GetRowValues(rowindex, "Receipt").ToString();
            ViewState["CreditWhenEdit"] = CreditE;
            //EditRecord Narration
            string NarrationE = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_Narration").ToString();
            //BranchID
            string BranchID = DetailsGrid.GetRowValues(rowindex, "BranchNonBranch").ToString();

            //RecordId
            string strRecordid = DetailsGrid.GetRowValues(rowindex, "CashReportID").ToString();
            //MainAccount && MainAccountHidden
            TextBox TxtMainAccountE = (TextBox)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE");
            HiddenField TxtMainAccount_hiddenE = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
            //SubAccount && SubAccountHidden
            TextBox TxtSubAccountE = (TextBox)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE");
            HiddenField TxtSubAccount_hiddenE = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");
            //Debit
            ASPxTextBox TxtDebitE = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtdebit");
            //Credit
            ASPxTextBox TxtCreditE = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtcredit");
            //Narration
            TextBox TxtNarrationE = (TextBox)DetailsGrid.FindEditFormTemplateControl("txtNarration1");

            values.Clear();
            if (MainAccountE.Trim() == TxtMainAccountE.Text.Trim() && SubAccountE.Trim() == TxtSubAccountE.Text.Trim() && DebitE.Trim() == TxtDebitE.Text.Trim() && CreditE.Trim() == TxtCreditE.Text.Trim() && NarrationE.Trim() == TxtNarrationE.Text.Trim())
            {
                values.Add("IsUpdating", "False");
            }
            else
            {
                values.Add("IsUpdating", "True");
                if (MainAccountE.Trim() == TxtMainAccountE.Text.Trim() && SubAccountE.Trim() == TxtSubAccountE.Text.Trim()) values.Add("MainAcOrSubAcChange", "False");
                else values.Add("MainAcOrSubAcChange", "True");
                values.Add("RecordID", strRecordid);
                values.Add("MainAccount", TxtMainAccount_hiddenE.Value != String.Empty ? ((TxtMainAccount_hiddenE.Value.ToString().Split('~').Length > 1) ? TxtMainAccount_hiddenE.Value.ToString().Split('~')[0] : TxtMainAccount_hiddenE.Value.ToString()) : String.Empty);
                values.Add("MainAccountText", TxtMainAccount_hiddenE.Value != String.Empty ? TxtMainAccountE.Text : String.Empty);
                values.Add("SubAccount", TxtSubAccount_hiddenE.Value != String.Empty ? TxtSubAccount_hiddenE.Value.ToString() : String.Empty);
                values.Add("SubAccountText", TxtSubAccount_hiddenE.Value != String.Empty ? TxtSubAccountE.Text : String.Empty);
                values.Add("BranchID", TxtSubAccount_hiddenE.Value != String.Empty ? ((TxtSubAccount_hiddenE.Value.Split('~').Length > 1) ? TxtSubAccount_hiddenE.Value.Split('~')[1] : "NAB") : BranchID);
                values.Add("Debit", TxtDebitE.Text);
                values.Add("Credit", TxtCreditE.Text);
                values.Add("Narration", TxtNarrationE.Text);
                values.Add("WhichRecordEdit", BranchNonBranchE);
                string[] MainAC = TxtMainAccount_hiddenE.Value.ToString().Split('~');
                if (TxtMainAccount_hiddenE.Value != String.Empty)
                {
                    DataTable dtType = new DataTable();
                    if (MainAC.Length > 1)
                        dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + TxtMainAccount_hiddenE.Value.ToString().Split('~')[0] + "");
                    else
                        dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + TxtMainAccount_hiddenE.Value.ToString() + "");
                    values.Add("Type", dtType.Rows[0][0].ToString());
                }
                else
                {
                    values.Add("Type", "NotChange");
                }
            }
            if (ViewState["CancelClick"] != null)
            {
                ViewState["DebitWhenEdit"] = null;
                ViewState["CreditWhenEdit"] = null;
                ViewState["CancelClick"] = null;
            }
        }
        protected void DetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (values["IsUpdating"].Equals("False"))
            {
                e.Cancel = true;
                DetailsGrid.CancelEdit();
                return;
            }
            Boolean AllowDisAllow;
            DataSet DsUpdateXMLValid = new DataSet();
            String WhichRecordEdit = null;
            WhichRecordEdit = values["WhichRecordEdit"].ToString();
            if (IsNumeric(WhichRecordEdit))
            {
                if (WhichRecordEdit != values["BranchID"].ToString())
                {
                    if (values["BranchID"].ToString() == "NAB")
                        AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenNAB(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                    else
                        AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenBranch(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                    if (AllowDisAllow)
                    {
                        UpdateMainXML();
                        if (DsUpdateXMLValid.Tables.Count > 0) { DsUpdateXMLValid.Tables.Remove(DsUpdateXMLValid.Tables[0]); DsUpdateXMLValid.Clear(); }
                        DataRow row;
                        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                        {
                            DsUpdateXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                            if (values["BranchID"].ToString() == "NAB")
                            {
                                row = DsUpdateXMLValid.Tables[0].Rows.Find("NB");
                                row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                                row.AcceptChanges();
                                row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                                row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                                row.AcceptChanges();
                                if (row["EntryCount"].ToString() == "0")
                                {
                                    DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                    DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                    DsUpdateXMLValid.AcceptChanges();
                                }


                            }
                            else
                            {
                                row = DsUpdateXMLValid.Tables[0].Rows.Find(values["BranchID"].ToString());
                                if (row == null)
                                {
                                    DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                                    row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                                    row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                                    row.AcceptChanges();
                                    if (row["EntryCount"].ToString() == "0")
                                    {
                                        DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                        DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                    }
                                    DsUpdateXMLValid.AcceptChanges();
                                    DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["CashReportID"] };
                                    DataRow NewRow = DsUpdateXMLValid.Tables[0].NewRow();
                                    NewRow["ID"] = values["BranchID"].ToString();
                                    NewRow["EntryCount"] = "1";
                                    NewRow["CashReportID"] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[DsUpdateXMLValid.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                                    DsUpdateXMLValid.Tables[0].Rows.Add(NewRow);
                                    DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) + 1;
                                    DsUpdateXMLValid.AcceptChanges();
                                }
                                else
                                {
                                    row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                                    row.AcceptChanges();
                                    row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                                    row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                                    if (row["EntryCount"].ToString() == "0")
                                    {
                                        DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                        DsUpdateXMLValid.AcceptChanges();
                                        DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                    }

                                }
                            }
                            if (DsUpdateXMLValid.Tables.Count > 0)
                            {
                                if (DsUpdateXMLValid.Tables[0].Rows.Count > 0)
                                {
                                    DsUpdateXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                                }
                            }
                        }
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                }
                else
                {
                    UpdateMainXML();
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
            }
            else if (WhichRecordEdit == "NAB")
            {
                if (WhichRecordEdit != values["BranchID"].ToString())
                {
                    AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenNAB(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                    if (AllowDisAllow)
                    {
                        UpdateMainXML();
                        if (DsUpdateXMLValid.Tables.Count > 0) { DsUpdateXMLValid.Tables.Remove(DsUpdateXMLValid.Tables[0]); DsUpdateXMLValid.Clear(); }
                        DsUpdateXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                        DataRow row = DsUpdateXMLValid.Tables[0].Rows.Find(values["BranchID"].ToString());
                        if (row == null)
                        {
                            DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["CashReportID"] };
                            DataRow NewRow = DsUpdateXMLValid.Tables[0].NewRow();
                            NewRow["ID"] = values["BranchID"].ToString();
                            NewRow["EntryCount"] = "1";
                            NewRow["CashReportID"] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[DsUpdateXMLValid.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                            DsUpdateXMLValid.Tables[0].Rows.Add(NewRow);
                            DsUpdateXMLValid.Tables[0].Rows[1][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[1][1].ToString()) - 1;
                            DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) + 1;
                            DsUpdateXMLValid.AcceptChanges();
                        }
                        else
                        {
                            row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                            row.AcceptChanges();
                            DsUpdateXMLValid.Tables[0].Rows[1][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[1][1].ToString()) - 1;
                            DsUpdateXMLValid.AcceptChanges();
                        }
                    }
                    if (DsUpdateXMLValid.Tables.Count > 0)
                    {
                        if (DsUpdateXMLValid.Tables[0].Rows.Count > 0)
                        {
                            DsUpdateXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                    }
                }
                else
                {
                    UpdateMainXML();
                }

            }
            TotalCredit = 0;
            TotalCredit = 0;
            DsUpdateXMLValid.Dispose();
            e.Cancel = true;
            DetailsGrid.CancelEdit();

        }
        void UpdateMainXML()
        {
            DataSet DsUpdateXML = new DataSet();
            if (DsUpdateXML.Tables.Count > 0) { DsUpdateXML.Tables.Remove(DsUpdateXML.Tables[0]); DsUpdateXML.Clear(); }
            DsUpdateXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXML.Tables[0].Columns["CashReportID"] };
            DataRow row = DsUpdateXML.Tables[0].Rows.Find(values["RecordID"]);
            if (values["BranchID"].ToString() == "NAB")
            {
                row["JournalVoucherDetail_BranchID"] = ddlBranch.SelectedValue;
                row["JournalVoucherDetail_SubAccountCode"] = values["SubAccount"].ToString() != String.Empty ? values["SubAccount"].ToString().Split('~')[0] : row["JournalVoucherDetail_SubAccountCode"].ToString();
            }
            else
            {
                row["JournalVoucherDetail_BranchID"] = values["BranchID"].ToString();
                row["JournalVoucherDetail_SubAccountCode"] = values["SubAccount"].ToString() != String.Empty ? values["SubAccount"].ToString().Split('~')[0] + '~' + values["SubAccount"].ToString().Split('~')[1] + '~' + values["SubAccount"].ToString().Split('~')[2] : row["JournalVoucherDetail_SubAccountCode"].ToString();
            }
            row["JournalVoucherDetail_MainAccountCode"] = values["MainAccount"].ToString() != String.Empty ? values["MainAccount"].ToString() : row["JournalVoucherDetail_MainAccountCode"].ToString();
            row["JournalVoucherDetail_AmountDr"] = values["Debit"].ToString();
            row["JournalVoucherDetail_AmountCr"] = values["Credit"].ToString();
            row["SubNarration"] = values["Narration"].ToString();
            row["MainAccount1"] = values["MainAccountText"].ToString() != String.Empty ? values["MainAccountText"].ToString() : row["MainAccount1"].ToString();
            row["SubAccount1"] = values["SubAccountText"].ToString() != String.Empty ? values["SubAccountText"].ToString() : row["SubAccount1"].ToString();
            row["WithDrawl"] = values["Debit"].ToString();
            row["Receipt"] = values["Credit"].ToString();
            row["Type"] = values["Type"].ToString() != String.Empty ? values["Type"].ToString() : row["Type"].ToString();
            row["BranchNonBranch"] = values["BranchID"].ToString() != String.Empty ? values["BranchID"].ToString() : row["BranchNonBranch"].ToString();
            if (values["MainAcOrSubAcChange"].ToString() == "True")
                row["OldJVVoucherNumber"] = "GenerateNew";
            row.AcceptChanges();
            DsUpdateXML.AcceptChanges();
            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.Dispose();
        }

        protected void DetailsGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            DataSet DsCanCancelEdit = new DataSet();
            TotalCredit = 0;
            TotalCredit = 0;
            ViewState["CreditWhenEdit"] = null;
            ViewState["DebitWhenEdit"] = null;
            if (DsCanCancelEdit.Tables.Count > 0) { DsCanCancelEdit.Tables.Remove(DsCanCancelEdit.Tables[0]); DsCanCancelEdit.Clear(); }
            DsCanCancelEdit.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            ViewState["CancelClick"] = "true";
            BindGrid(DetailsGrid, DsCanCancelEdit, "DESC");
            DsCanCancelEdit.Dispose();
        }
        protected void DetailsGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string RowDebitValue = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_AmountDr").ToString();
            string RowCreditValue = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_AmountCr").ToString();
            TotalDebit = TotalDebit + Convert.ToDecimal(RowDebitValue != String.Empty ? RowDebitValue : "0.00");
            TotalCredit = TotalCredit + Convert.ToDecimal(RowCreditValue != String.Empty ? RowCreditValue : "0.00");
        }
        protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataSet Ds_JVE;
            SqlDataAdapter Da_JVE;
            int RowIndex;
            string PCBCommandName = e.Parameters.Split('~')[0];
            string IBRef;
            string OldVoucherNumber = String.Empty;
            string OldWhichTypeOfItem = String.Empty;
            string VoucherNumber;
            string JVEFile_XMLPATH;
            if (PCBCommandName == "PCB_BtnShow")
            {
                BindGrid(GvJvSearch);
                Session["StrQuery"] = null;
                string StrQuery = " and JournalVoucher_TransactionDate='" + tDate.Value.ToString().Substring(0, 10) + "'";
                DsSearchCallBack = new DataSet();
                DaSearchCallBack = new SqlDataAdapter();
                if (ChkBranch.Checked) StrQuery = StrQuery + " and JournalVoucher_BranchID='" + ddlBranch.SelectedValue + "'";
                if (ChkBillNo.Checked) StrQuery = StrQuery + " and JournalVoucher_BillNumber='" + txtBillNo.Text + "'";
                if (ChkPrefix.Checked) StrQuery = StrQuery + " and JournalVoucher_Prefix='" + txtAccountCode.Text + "'";
                if (ChkNarration.Checked) StrQuery = StrQuery + " and JournalVoucher_Narration='" + txtNarration.Text + "'";
                Session["StrQuery"] = StrQuery;
                FillSearchGrid();
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;
            }
            if (PCBCommandName == "PCB_EditEnd")
            {
                IBRef = Session["IBRef"].ToString();
                JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
                if (Session["IBRef"] != null)
                {
                    if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(JVEFile_XMLPATH));
                        File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                    }
                }
                Session["StrQuery"] = null;
                Session["IBRef"] = null;
                GvJvSearch.Visible = false;
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;

            }
            if (PCBCommandName == "PCB_BtnOkE")
            {
                IBRef = Session["IBRef"].ToString();
                OldWhichTypeOfItem = Session["WhichTypeItem"].ToString();
                VoucherNumber = Session["VoucherNumber"].ToString();
                CreateJVE_XMLFile(IBRef, VoucherNumber);
                if (ViewState["JVE_FileAlreadyUsedBy"] != null)
                {
                    if (ViewState["JVE_FileAlreadyUsedBy"].ToString().Split('~')[1] == Session["userid"].ToString())
                    {
                        GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "HimSelef~";
                    }
                    else
                    {
                        GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "Other~" + ViewState["JVE_FileAlreadyUsedBy"].ToString();
                    }
                    GvJvSearch.JSProperties["cpEntryEventFire"] = null;

                }
                else
                {
                    BindGrid(GvJvSearch);
                    GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                    GvJvSearch.JSProperties["cpEntryEventFire"] = "true";
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;

            }
            if (PCBCommandName == "PCB_DeleteBtnOkE")
            {
                int RowUpdated;
                IBRef = Session["IBRef"].ToString();
                OldWhichTypeOfItem = Session["WhichTypeItem"].ToString();
                VoucherNumber = Session["VoucherNumber"].ToString();
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);

                RowUpdated = objFAReportsOther.Delete_JV(
                Convert.ToString(IBRef),
                Convert.ToString(VoucherNumber));
                if (RowUpdated > 0)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                    //This For Log Purpose
                    oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting.Sry for Inconvenience";
                    //This For Log Purpose
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            }
            if (PCBCommandName == "PCB_BindAfterDelete")
            {
                FillSearchGrid();
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;
            }
            if (PCBCommandName == "PCB_ContinueWith")
            {
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = "true";
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
            if (PCBCommandName == "PCB_FreshEntry")
            {
                IBRef = Session["IBRef"].ToString();
                OldWhichTypeOfItem = Session["WhichTypeItem"].ToString();
                VoucherNumber = Session["VoucherNumber"].ToString();
                JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
                Ds_JVE = new DataSet();


                Ds_JVE = objFAReportsOther.Fetch_JVE_DataSet(
             Convert.ToString(ViewState["SegmentName"]),
             Convert.ToString(Session["userid"]),
              Convert.ToString(VoucherNumber),
              Convert.ToString(IBRef),
               Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]),
              Convert.ToString(Session["LastCompany"]));
                if (Ds_JVE.Tables.Count > 0)
                {
                    if (Ds_JVE.Tables[0].Rows.Count > 0)
                    {
                        if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
                        {
                            File.Delete(Server.MapPath(JVEFile_XMLPATH));
                            File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                        }
                        Ds_JVE.Tables[0].TableName = "DtJournalVoucher";
                        Ds_JVE.WriteXml(Server.MapPath(JVEFile_XMLPATH));
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                }
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = true;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = null;
            }
            if (PCBCommandName == "PCB_CloseEntry")
            {
                IBRef = Session["IBRef"].ToString();
                JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
                if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
                {
                    File.Delete(Server.MapPath(JVEFile_XMLPATH));
                    File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                Session["StrQuery"] = null;
                Session["IBRef"] = null;
                Session["IBRef"] = null;
                Session["WhichTypeItem"] = null;
                Session["VoucherNumber"] = null;
                GvJvSearch.JSProperties["cpJVClose"] = "File Successfully Close";
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
                GvJvSearch.JSProperties["cpJVDelete"] = null;
                Session["IBRef"] = null;
                Session["OldJVVoucherNumber"] = null;
                Session["OldWhichTypeItem"] = null;
            }



        }
        protected void GvJvSearch_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void DetailsGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (Session["LCKJV"] != null)
            {
                DateTime LockDate = Convert.ToDateTime(Session["LCKJV"].ToString());
                if (Convert.ToDateTime(tDate.Value) <= LockDate)
                {
                    e.Visible = false;
                    DetailsGrid.JSProperties["cpHideAddBtnOnLock"] = "false";
                }
                else
                {
                    DetailsGrid.JSProperties["cpHideAddBtnOnLock"] = "true";
                }
            }
        }

        //Currency Setting
        protected void CbpChoosenCurrency_Callback(object source, CallbackEventArgsBase e)
        {
            CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = null;

            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "ChangeCurrency")
            {
                string ActiveCurrencyID = String.Empty;
                string ActiveCurrencyName = String.Empty;
                string ActiveCurrencySymbol = String.Empty;
                string CurrencyInXmlFile = String.Empty;
                string ActiveCurrency = Session["ActiveCurrency"].ToString();
                string TradeCurrency = Session["TradeCurrency"].ToString();
                string LocalCurrency = Session["LocalCurrency"].ToString();

                //Change ChoosenCurrency In XmlFile if Exists
                if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                {
                    DataSet DsChangeChooseCurrency = new DataSet();
                    DsChangeChooseCurrency.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                    CurrencyInXmlFile = DsChangeChooseCurrency.Tables[0].Rows[0]["ChoosenCurrency"].ToString();
                    if (CurrencyInXmlFile != ActiveCurrency)
                    {
                        if (ActiveCurrency == TradeCurrency)
                        {
                            ActiveCurrencyID = TradeCurrency.Split('~')[0];
                            ActiveCurrencyName = TradeCurrency.Split('~')[1];
                            ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                            Session["ActiveCurrency"] = TradeCurrency;
                        }
                        else
                        {
                            ActiveCurrencyID = LocalCurrency.Split('~')[0];
                            ActiveCurrencyName = LocalCurrency.Split('~')[1];
                            ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                            Session["ActiveCurrency"] = LocalCurrency;
                        }
                        for (int i = 0; i < DsChangeChooseCurrency.Tables[0].Rows.Count; i++)
                        {
                            DsChangeChooseCurrency.Tables[0].Rows[i]["ChoosenCurrency"] = Session["ActiveCurrency"].ToString();
                        }
                        DsChangeChooseCurrency.AcceptChanges();
                        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                        DsChangeChooseCurrency.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                        DsChangeChooseCurrency.Dispose();
                        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
                    }
                    else
                    {
                        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = CurrencyInXmlFile.Split('~')[1] + '~' + CurrencyInXmlFile.Split('~')[2];

                    }
                }
                else
                {
                    if (ActiveCurrency == TradeCurrency)
                    {
                        ActiveCurrencyID = TradeCurrency.Split('~')[0];
                        ActiveCurrencyName = TradeCurrency.Split('~')[1];
                        ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = TradeCurrency;
                    }
                    else
                    {
                        ActiveCurrencyID = LocalCurrency.Split('~')[0];
                        ActiveCurrencyName = LocalCurrency.Split('~')[1];
                        ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = LocalCurrency;
                    }
                    CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
                }
            }
        }
        protected void CbpAcBalance_Callback(object source, CallbackEventArgsBase e)
        {
            CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
            string command = e.Parameter.Split('~')[0];
            if (command == "AcBalance")
            {
                string MainAccountID = e.Parameter.Split('~')[1];
                string SubAccountID = e.Parameter.Split('~')[2] != String.Empty ? e.Parameter.Split('~')[2] : null;
                string[,] MainAcCode = oDBEngine.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + MainAccountID + "", 1);
                DateTime FinYearEnd = Convert.ToDateTime(Session["FinYearEnd"].ToString()); //Session["FinYearEnd"].ToString();
                DataTable DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                String CurrentSegment = DtCurrentSegment.Rows[0][0].ToString();
                DataTable DtAcBalance = oDBEngine.OpeningBalanceJournal1("'" + MainAcCode[0, 0] + "'", SubAccountID, FinYearEnd, CurrentSegment,
                    Session["LastCompany"].ToString(), FinYearEnd, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));
                Converter oConverter = new Converter();
                if (DtAcBalance.Rows.Count > 0 && DtAcBalance.Rows[0][0].ToString() != String.Empty)
                {
                    string strColor = null;
                    string strDrCr = null;
                    strColor = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                    strDrCr = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? " Dr." : " Cr.";
                    CbpAcBalance.JSProperties["cpAcBalance"] = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString())) + strDrCr + '~' + strColor;
                }
                else
                {
                    CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
                }

            }
        }
        //////////////////////////
    }
}