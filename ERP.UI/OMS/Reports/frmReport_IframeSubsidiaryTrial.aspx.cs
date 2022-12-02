using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_IframeSubsidiaryTrial : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        string data;
        string BranchId = null;
        string Clients;
        string Group = null;
        string MainAcc = null;
        static string CompanyID = null;
        string SegmentID = null;
        String LinkFirst;
        String LinkPrev;
        String LinkNext;
        String LinkLast;
        DataTable dtSubsidiary = new DataTable();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.FAReportsOther oFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        ExcelFile objExcel = new ExcelFile();
        string SegMentName;
        public string strHtext = "";
        string SendEmailTag = "N";
        BusinessLogicLayer.GenericMethod oGenericMethod = null;
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        AspxHelper oAspxHelper;
        BusinessLogicLayer.Converter oConverter;
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDbEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (Session["userlastsegment"].ToString() == "5")
            {
                HDNAccInd.Value = "N";
            }
            else
            {
                HDNAccInd.Value = "Y";
            }

            if (!IsPostBack)
            {
                chkBranchNet.Checked = true;
                MainAcc = null;
                SegmentID = null;
                BranchId = null;
                CompanyID = null;
                DataTable DtSegComp = new DataTable();
                //if (Session["userlastsegment"].ToString() == "5")
                //{
                //    ViewState["SegmentID"] = "0";
                //    TrSeg.Visible = false;

                //}
                //else
                //{
                DataTable dtSeg = oDbEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDbEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDbEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                        {
                            SegmentID = DtSegComp.Rows[i][1].ToString();
                            SegMentName = DtSegComp.Rows[i][2].ToString();
                        }
                        else
                        {
                            SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                            SegMentName = SegMentName + "," + DtSegComp.Rows[i][2].ToString();
                        }
                    }
                    ViewState["SegmentID"] = SegmentID;
                    Span2.InnerText = SegMentName;
                    HDNSeg.Value = SegMentName;
                    HdnSegment.Value = SegmentID;
                }



                //  }
                if (Request.QueryString["mainacc"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>FromGeneralLedger();</script>");
                    FillGrid();
                }
                else
                {
                    string[] FinYear = Session["LastFinYear"].ToString().Split('-');
                    dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                    dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtTo.EditFormatString = oconverter.GetDateFormat("Date");

                    dtDate.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());
                    dtFrom.Value = Convert.ToDateTime(Session["FinYearStart"].ToString());
                    dtTo.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    //txtsubscriptionID.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SelectMainAccountName',event)");
                    //txtsubscriptionID.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SearchMainAccountBranchSegment',event)");
                    //rdbMainAll.Attributes.Add("OnClick", "MainAll('all','MainAcc')");
                    //rdbMainSelected.Attributes.Add("OnClick", "MainAll('Selc','MainAcc')");
                    //rdAllBranch.Attributes.Add("onclick", "MainAll('all','Branch')");
                    //rdSelBranch.Attributes.Add("onclick", "MainAll('Selc','Branch')");
                    rdAllSegment.Attributes.Add("onclick", "MainAll('all','Segment')");
                    rdSelSegment.Attributes.Add("onclick", "MainAll('Selc','Segment')");
                }

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            string str2 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                    str2 = val[0];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                    str2 += "," + val[0];
                }
            }
            if (idlist[0] == "MainAcc")
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                DataTable DtSubLedgerTypeCount = new DataTable();
                DataTable DtSubLedgerType = new DataTable();
                MainAcc = str;
                data = "MainAcc~" + str;
                DtSubLedgerTypeCount = oGenericMethod.GetDataTable(@"Select Count(SubLedgerType) TotalSubLedgerType From
            (Select Distinct(MainAccount_SubLedgerType) SubLedgerType From Master_MainAccount 
		    Where MainAccount_AccountCode in (" + str + ")) T1");
                if (DtSubLedgerTypeCount != null)
                    if (DtSubLedgerTypeCount.Rows.Count > 0)
                        if (Convert.ToInt32(DtSubLedgerTypeCount.Rows[0][0]) == 1)
                        {
                            DtSubLedgerType = oGenericMethod.GetDataTable(@"Select distinct MainAccount_SubLedgerType
                        From Master_MainAccount Where MainAccount_AccountCode in (" + str + ")");
                            if (DtSubLedgerType != null)
                                if (DtSubLedgerType.Rows.Count > 0)
                                {
                                    data = data + "~" + DtSubLedgerType.Rows[0][0].ToString().Trim();
                                }
                        }

            }
            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str;
                ViewState["Clients"] = Clients;
            }

            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                BranchId = str;
                data = "Branch~" + str;
            }
            else if (idlist[0] == "Segment")
            {
                SegmentID = str;
                data = "Segment~" + str1;
            }
            else if (idlist[0] == "Employee")
            {
                data = "Employee~" + str2;
            }
            else if (idlist[0] == "MailToEmployee")
            {
                data = "MailToEmployee~" + str2;
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            FillGrid();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataTable dtBr = oDbEngine.GetDataTable("tbl_master_branch", "branch_parentID", "branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid='" + HttpContext.Current.Session["usercontactID"].ToString() + "')");

            if (RadAsOnDate.Checked == true)
            {

                if (rdbMainAll.Checked == true)
                {

                    if (dtBr.Rows[0][0].ToString() == "0")
                    {
                        DrpdownClick();
                        FillGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "JS", " SelectAccount()", true);
                    }

                }
                else
                {
                    FillGrid();
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
                }

            }
            else
            {

                if (rdbMainAll.Checked == true)
                {
                    if (dtBr.Rows[0][0].ToString() == "0")
                    {
                        DrpdownClick();
                        FillGridForPeriod();
                        ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "JS1", " SelectAccount()", true);
                    }

                }
                else
                {
                    FillGridForPeriod();
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
                }
                //ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
            }
        }
        public void FillGrid()
        {
            try
            {
                string BranchName = null;
                string ForGroup = null;
                BranchId = HdnBranchId.Value;
                Group = HdnGroup.Value;
                MainAcc = HdnMainAcc.Value;
                string TransactionDate = null;
                DataTable dtSubsidiary_New = new DataTable();
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                pageSize = 150000;
                string WehereMainAccount = null;
                string WhereMainBranch = null;
                DateTime date;
                decimal SumForBranchDR = 0;
                decimal SumForBranchCR = 0;
                decimal DifOfDRCR = 0;
                string[] ClientValue = null;
                decimal DebitCreditGreterEqualAmount = 0;
                if (rdbMainSelected.Checked == true)
                {
                    ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                    ViewState["SubType"] = ClientValue[1].ToString().Trim();
                }
                else
                {
                    ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                    ViewState["SubType"] = ClientValue[1].ToString().Trim();
                }

                #region New Part
                string MainAccID = "";
                string SubAccId = "";
                string Brnch = "";
                string Segmnt = "";
                decimal DrCrAmt = 0;
                string Grp = "";
                string RptType = "";
                string BranchGroupType = "";
                string GroupType = "";
                String CheckDrCr = "";
                string ZeroBal = "";
                if (rdbClientSelected.Checked == true)
                    SubAccId = HdnClients.Value;

                //Chenge For Consolidate Account Feature
                if (!chkConsolidateAccounts.Checked)
                    MainAccID = ClientValue[0].ToString();
                else
                    MainAccID = MainAcc.Replace("'", "").Replace(",", "','");
                if (RadAsOnDate.Checked == true)
                    RptType = "A";
                else
                    RptType = "P";

                Brnch = Session["userbranchHierarchy"].ToString();

                if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                {
                    if (ddlGroup.SelectedItem.Value == "0")
                    {
                        BranchGroupType = "B";
                        if (rdbMainSelected.Checked == true)
                        {

                            if (rdbranchAll.Checked == true)
                                Brnch = Session["userbranchHierarchy"].ToString();
                            else
                                Brnch = BranchId;

                        }
                    }
                    else
                    {
                        BranchGroupType = "G";
                        if (rdbMainSelected.Checked == true)
                        {
                            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                            {
                                if (rdddlgrouptypeAll.Checked == true)
                                {
                                    if (ddlgrouptype.SelectedItem.Value == "0")
                                        GroupType = "N";
                                    else
                                        GroupType = ddlgrouptype.SelectedItem.Text.ToString();
                                }
                                else
                                    Grp = Group;
                            }

                        }
                    }
                }

                if (rdDebit.Checked == true)
                    CheckDrCr = "D";
                else if (rdCredit.Checked == true)
                    CheckDrCr = "C";
                else
                    CheckDrCr = "B";


                if (rdAllSegment.Checked == true)
                {
                    DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "*", null);
                    Segmnt = DT.Rows[0][0].ToString();
                    for (int i = 1; i < DT.Rows.Count; i++)
                    {
                        Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                        ViewState["SegmentID"] = Segmnt;
                    }
                }
                else
                {
                    // Segmnt = ViewState["SegmentID"].ToString();
                    if (Session["userlastsegment"].ToString() == "5")
                    {
                        DataTable DtSeg = new DataTable();
                        DtSeg = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                        Segmnt = DtSeg.Rows[0][1].ToString();

                    }
                    else
                    {
                        if (HdnSegment.Value != "")
                        {
                            Segmnt = HdnSegment.Value;
                            ViewState["SegmentID"] = HdnSegment.Value;
                        }
                        else
                        {
                            Segmnt = ViewState["SegmentID"].ToString();
                        }
                    }
                }

                if (chkZero.Checked == true)
                {
                    ZeroBal = "Y";
                }
                else
                {
                    ZeroBal = "N";

                }

                //For Sever Debugger Variable
                string[,] strParam = new string[22, 2];
                string SpName = String.Empty;
                string LType = string.Empty;

                if (txtDebitCredit.Text != "")
                    DrCrAmt = Convert.ToDecimal(txtDebitCredit.Text);
                string strSubLedgerType = "";
                DataSet ds = new DataSet();
                ds = oFAReportsOther.Fetch_SubSideryTrial(
                      Convert.ToString(MainAccID),
                       Convert.ToString(SubAccId),
                       Convert.ToString(Brnch),
                      Convert.ToString(dtFrom.Value),
                       Convert.ToString(dtDate.Value),
                       Convert.ToString(Segmnt),
                       Convert.ToString(Session["LastFinYear"]),
                       Convert.ToString(Session["LastCompany"]),
                      Convert.ToString(DrCrAmt),
                       Convert.ToString(Grp),
                        Convert.ToString(RptType),
                       Convert.ToString(BranchGroupType),
                       Convert.ToString(GroupType),
                       Convert.ToString(CheckDrCr),
                     Convert.ToString(ZeroBal),
                       Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]),
                     Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]),
                      "N",
                       "N",
                       "N",
                       Convert.ToString(chkConsolidateAccounts.Checked ? "T" : "F"),
                       out LType
                    );
                //SD Code (Server Debugging Code)
                strParam[0, 0] = "MainAccountID"; strParam[0, 1] = "'" + MainAccID.Replace("'", "''") + "'";
                strParam[1, 0] = "SubAccount"; strParam[1, 1] = "'" + SubAccId + "'";
                strParam[2, 0] = "Branch"; strParam[2, 1] = "'" + Brnch + "'";
                strParam[3, 0] = "FromDate"; strParam[3, 1] = "'" + Convert.ToDateTime(dtFrom.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                strParam[4, 0] = "ToDate"; strParam[4, 1] = "'" + Convert.ToDateTime(dtDate.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                strParam[5, 0] = "Segment"; strParam[5, 1] = "'" + Segmnt + "'";
                strParam[6, 0] = "FinancialYr"; strParam[6, 1] = "'" + Session["LastFinYear"].ToString() + "'";
                strParam[7, 0] = "Company"; strParam[7, 1] = "'" + Session["LastCompany"].ToString() + "'";
                strParam[8, 0] = "DrCrAmt"; strParam[8, 1] = "'" + DrCrAmt.ToString() + "'";
                strParam[9, 0] = "Group"; strParam[9, 1] = "'" + Grp + "'";
                strParam[10, 0] = "ReportType"; strParam[10, 1] = "'" + RptType + "'";
                strParam[11, 0] = "BranchGroutType"; strParam[11, 1] = "'" + BranchGroupType + "'";
                strParam[12, 0] = "GroupType"; strParam[12, 1] = "'" + GroupType + "'";
                strParam[13, 0] = "ShowStatus"; strParam[13, 1] = "'" + CheckDrCr + "'";
                strParam[14, 0] = "ZeroBal"; strParam[14, 1] = "'" + ZeroBal + "'";
                strParam[15, 0] = "ActiveCurrency"; strParam[15, 1] = "'" + Session["ActiveCurrency"].ToString().Split('~')[0] + "'";
                strParam[16, 0] = "TradeCurrency"; strParam[16, 1] = "'" + Session["TradeCurrency"].ToString().Split('~')[0] + "'";
                strParam[17, 0] = "IsSegmentWiseBreakUp"; strParam[17, 1] = "'N'";
                strParam[18, 0] = "IsTDayBal"; strParam[18, 1] = "'N'";
                strParam[19, 0] = "IsConsolidate"; strParam[19, 1] = "'N'";
                strParam[20, 0] = "IsAcConsolidate"; strParam[20, 1] = "'" + (chkConsolidateAccounts.Checked ? "T" : "F") + "'";
                strParam[21, 0] = "LType"; strParam[21, 1] = "''";
                //For Server Debugging Purpose
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 2)
                {
                    string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                    string FilePath = "../ExportFiles/ServerDebugging/Fetch_SubSideryTrial" + strDateTime + ".txt";
                    oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, "Fetch_SubSideryTrial"), FilePath, false);
                }


                strSubLedgerType = LType;




                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_SubSideryTrial", con))
                //    {
                //        SqlParameter oparaType = new SqlParameter("@LType", SqlDbType.VarChar, 100);
                //        oparaType.Direction = ParameterDirection.Output;
                //        da.SelectCommand.Parameters.AddWithValue("@MainAccountID", MainAccID);
                //        da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                //        da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                //        da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                //        da.SelectCommand.Parameters.AddWithValue("@ToDate", dtDate.Value);
                //        da.SelectCommand.Parameters.AddWithValue("@Segment", Segmnt);
                //        da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                //        da.SelectCommand.Parameters.AddWithValue("@Company", Session["LastCompany"].ToString());
                //        da.SelectCommand.Parameters.AddWithValue("@DrCrAmt", DrCrAmt);
                //        da.SelectCommand.Parameters.AddWithValue("@Group", Grp);
                //        da.SelectCommand.Parameters.AddWithValue("@ReportType", RptType);
                //        da.SelectCommand.Parameters.AddWithValue("@BranchGroutType", BranchGroupType);
                //        da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                //        da.SelectCommand.Parameters.AddWithValue("@ShowStatus", CheckDrCr);
                //        da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                //        ///Currency Setting
                //        da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                //        da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                //        //Segment Wise Break Up
                //        da.SelectCommand.Parameters.AddWithValue("@IsSegmentWiseBreakUp", "N");
                //        da.SelectCommand.Parameters.AddWithValue("@IsTDayBal", "N");
                //        da.SelectCommand.Parameters.AddWithValue("@IsConsolidate","N");
                //        //For Consolidate Same Type Of Subledger Accounts
                //        da.SelectCommand.Parameters.AddWithValue("@IsAcConsolidate", chkConsolidateAccounts.Checked ? "T" : "F");

                //        //SD Code (Server Debugging Code)
                //        strParam[0, 0] = "MainAccountID"; strParam[0, 1] = "'" + MainAccID.Replace("'", "''") + "'";
                //        strParam[1, 0] = "SubAccount"; strParam[1, 1] = "'" + SubAccId + "'";
                //        strParam[2, 0] = "Branch"; strParam[2, 1] = "'" + Brnch + "'";
                //        strParam[3, 0] = "FromDate"; strParam[3, 1] = "'" + Convert.ToDateTime(dtFrom.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                //        strParam[4, 0] = "ToDate"; strParam[4, 1] = "'" + Convert.ToDateTime(dtDate.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                //        strParam[5, 0] = "Segment"; strParam[5, 1] = "'" + Segmnt + "'";
                //        strParam[6, 0] = "FinancialYr"; strParam[6, 1] = "'" + Session["LastFinYear"].ToString() + "'";
                //        strParam[7, 0] = "Company"; strParam[7, 1] = "'" + Session["LastCompany"].ToString() + "'";
                //        strParam[8, 0] = "DrCrAmt"; strParam[8, 1] = "'" + DrCrAmt.ToString() + "'";
                //        strParam[9, 0] = "Group"; strParam[9, 1] = "'" + Grp + "'";
                //        strParam[10, 0] = "ReportType"; strParam[10, 1] = "'" + RptType + "'";
                //        strParam[11, 0] = "BranchGroutType"; strParam[11, 1] = "'" + BranchGroupType + "'";
                //        strParam[12, 0] = "GroupType"; strParam[12, 1] = "'" + GroupType + "'";
                //        strParam[13, 0] = "ShowStatus"; strParam[13, 1] = "'" + CheckDrCr + "'";
                //        strParam[14, 0] = "ZeroBal"; strParam[14, 1] = "'" + ZeroBal + "'";
                //        strParam[15, 0] = "ActiveCurrency"; strParam[15, 1] = "'" + Session["ActiveCurrency"].ToString().Split('~')[0] + "'";
                //        strParam[16, 0] = "TradeCurrency"; strParam[16, 1] = "'" + Session["TradeCurrency"].ToString().Split('~')[0] + "'";
                //        strParam[17, 0] = "IsSegmentWiseBreakUp"; strParam[17, 1] = "'N'";
                //        strParam[18, 0] = "IsTDayBal"; strParam[18, 1] = "'N'";
                //        strParam[19, 0] = "IsConsolidate"; strParam[19, 1] = "'N'";
                //        strParam[20, 0] = "IsAcConsolidate"; strParam[20, 1] = "'" + (chkConsolidateAccounts.Checked ? "T" : "F") + "'";
                //        strParam[21, 0] = "LType"; strParam[21, 1] = "''";
                //        //For Server Debugging Purpose
                //        oGenericMethod = new BusinessLogicLayer.GenericMethod();
                //        if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 2)
                //        {
                //            string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                //            string FilePath = "../ExportFiles/ServerDebugging/Fetch_SubSideryTrial" + strDateTime + ".txt";
                //            oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, "Fetch_SubSideryTrial"), FilePath, false);
                //        }

                //        da.SelectCommand.Parameters.Add(oparaType);
                //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //        da.SelectCommand.CommandTimeout = 0;

                //        if (con.State == ConnectionState.Closed)
                //            con.Open();
                //        ds.Reset();
                //        da.Fill(ds);
                //        //ViewState["dataset"] = ds;
                //        strSubLedgerType = Convert.ToString(oparaType.Value);

                //    }
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (SendEmailTag != "Y")
                    {
                        lblReportHeader.Text = "Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
                    }
                }
                else
                {
                    if (SendEmailTag != "Y")
                    {
                        lblReportHeader.Text = "No Record Found!...";
                    }
                }

                #endregion

                ///-------------------------------------------------------



                decimal SumDr = 0;
                decimal SumCr = 0;
                decimal diffTot = 0;
                pagecount = dtSubsidiary.Rows.Count / pageSize + 1;
                TotalPages.Value = pagecount.ToString();
                if (pageindex <= 0)
                {
                    pageindex = 0;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                grdSubsidiaryTrial.PageIndex = pageindex;
                CurrentPage.Value = pageindex.ToString();
                rowcount = 0;




                //------New Datatable Bind ------
                dtSubsidiary = ds.Tables[0];
                dtSubsidiary_New.Columns.Add("BranchName");
                dtSubsidiary_New.Columns.Add("accountsledger_mainaccountid");
                dtSubsidiary_New.Columns.Add("accountsledger_subaccountid");
                dtSubsidiary_New.Columns.Add("Ucc");
                dtSubsidiary_New.Columns.Add("AmountDR");
                dtSubsidiary_New.Columns.Add("AmountCR");
                dtSubsidiary_New.Columns.Add("MainID");
                dtSubsidiary_New.Columns.Add("SubID");
                dtSubsidiary_New.Columns.Add("PhoneNumber");
                dtSubsidiary_New.Columns.Add("AmtDebit");
                dtSubsidiary_New.Columns.Add("AmtCredit");
                dtSubsidiary_New.Columns.Add("Trading_UCC");

                if (dtSubsidiary.Rows.Count > 0)
                {
                    for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                    {
                        if (k == 0)
                            BranchName = dtSubsidiary.Rows[k]["BRANCH"].ToString();
                        if (dtSubsidiary.Rows[k]["BRANCH"].ToString() != BranchName)
                        {
                            if (chkBranchNet.Checked == true)
                            {
                                DataRow DrNew1 = dtSubsidiary_New.NewRow();
                                DrNew1[0] = "Total";
                                DrNew1[4] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                                DrNew1[5] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);

                                DrNew1[9] = oconverter.formatmoneyinUs(SumForBranchDR);
                                DrNew1[10] = oconverter.formatmoneyinUs(SumForBranchCR);
                                dtSubsidiary_New.Rows.Add(DrNew1);
                                DifOfDRCR = SumForBranchDR - SumForBranchCR;
                                DataRow DrNew2 = dtSubsidiary_New.NewRow();

                                if (ddlGroup.SelectedItem.Value == "0")
                                    DrNew2[0] = "Branch Net";
                                else
                                    DrNew2[0] = "Group Net";
                                if (DifOfDRCR >= 0)
                                {
                                    DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                                    DrNew2[9] = oconverter.formatmoneyinUs(DifOfDRCR);
                                }
                                else
                                {
                                    DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                                    DrNew2[10] = oconverter.formatmoneyinUs(Math.Abs(DifOfDRCR));
                                }

                                dtSubsidiary_New.Rows.Add(DrNew2);
                                DifOfDRCR = 0;
                                SumForBranchDR = 0;
                                SumForBranchCR = 0;
                                DataRow DrNewBlank = dtSubsidiary_New.NewRow();
                                DrNewBlank[0] = "Blank";
                                dtSubsidiary_New.Rows.Add(DrNewBlank);
                            }
                        }
                        DataRow DrNew = dtSubsidiary_New.NewRow();
                        DrNew[0] = dtSubsidiary.Rows[k]["BRANCH"].ToString();
                        DrNew[1] = dtSubsidiary.Rows[k]["MainAccountName"].ToString();
                        DrNew[2] = dtSubsidiary.Rows[k]["ACCOUNT_NAME"].ToString();
                        DrNew[3] = dtSubsidiary.Rows[k]["UCC"].ToString();
                        if (dtSubsidiary.Rows[k]["ClDr"].ToString() != "")
                        {
                            //dtSubsidiary.Rows[k]["ClDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString()));
                            //DrNew[4] = dtSubsidiary.Rows[k]["ClDr"].ToString();
                            //DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString()));
                            //   dtSubsidiary.Rows[k]["ClDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString()));
                            DrNew[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString()));
                            DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString()));
                        }
                        else
                        {
                            DrNew[4] = "";
                            DrNew[9] = "";
                        }
                        if (dtSubsidiary.Rows[k]["ClCr"].ToString() != "")
                        {
                            // dtSubsidiary.Rows[k]["ClCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClCr"].ToString()));
                            DrNew[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClCr"].ToString()));
                            DrNew[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtSubsidiary.Rows[k]["ClCr"].ToString()));
                        }
                        else
                        {
                            DrNew[5] = "";
                            DrNew[10] = "";
                        }
                        DrNew[6] = dtSubsidiary.Rows[k]["MAINACCOUNTID"].ToString();
                        DrNew[7] = dtSubsidiary.Rows[k]["SUBACCOUNTID"].ToString();
                        DrNew[8] = dtSubsidiary.Rows[k]["PHONE_NUMBER"].ToString();
                        DrNew[11] = dtSubsidiary.Rows[k]["Trading_UCC"].ToString();
                        if (dtSubsidiary.Rows[k]["ClDr"] != DBNull.Value)
                        {
                            SumDr += Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString());
                            SumForBranchDR += Convert.ToDecimal(dtSubsidiary.Rows[k]["ClDr"].ToString());
                        }
                        if (dtSubsidiary.Rows[k]["ClCr"] != DBNull.Value)
                        {
                            SumCr += Convert.ToDecimal(dtSubsidiary.Rows[k]["ClCr"].ToString());
                            SumForBranchCR += Convert.ToDecimal(dtSubsidiary.Rows[k]["ClCr"].ToString());
                        }
                        BranchName = dtSubsidiary.Rows[k]["BRANCH"].ToString();
                        dtSubsidiary_New.Rows.Add(DrNew);
                        if (rdbMainSelected.Checked == true)
                        {
                            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                            {
                                if (chkBranchNet.Checked == true)
                                {
                                    if (k == dtSubsidiary.Rows.Count - 1)
                                    {

                                        DataRow DrNew3 = dtSubsidiary_New.NewRow();
                                        DrNew3[0] = "Total";
                                        DrNew3[4] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                                        DrNew3[5] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);

                                        DrNew3[9] = oconverter.formatmoneyinUs(SumForBranchDR);
                                        DrNew3[10] = oconverter.formatmoneyinUs(SumForBranchCR);
                                        dtSubsidiary_New.Rows.Add(DrNew3);
                                        DifOfDRCR = SumForBranchDR - SumForBranchCR;
                                        if (chkBranchNet.Checked == true)
                                        {
                                            DataRow DrNew4 = dtSubsidiary_New.NewRow();
                                            if (ddlGroup.SelectedItem.Value == "0")
                                                DrNew4[0] = "Branch Net";
                                            else
                                                DrNew4[0] = "Group Net";
                                            if (DifOfDRCR >= 0)
                                            {
                                                DrNew4[4] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                                                DrNew4[9] = oconverter.formatmoneyinUs(DifOfDRCR);
                                            }
                                            else
                                            {
                                                DrNew4[5] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                                                DrNew4[10] = oconverter.formatmoneyinUs(Math.Abs(DifOfDRCR));
                                            }
                                            dtSubsidiary_New.Rows.Add(DrNew4);
                                        }
                                        DifOfDRCR = 0;
                                        SumForBranchDR = 0;
                                        SumForBranchCR = 0;
                                    }
                                }
                            }
                        }
                    }
                }

                if (strSubLedgerType == "NSDL Clients" || strSubLedgerType == "CDSL Clients")
                {

                    strHtext = "BenAccount No";

                }
                else
                {
                    strHtext = "UCC";

                }
                grdSubsidiaryTrial.DataSource = dtSubsidiary_New;
                grdSubsidiaryTrial.DataBind();

                if (strSubLedgerType == "NSDL Clients" || strSubLedgerType == "CDSL Clients")
                {
                    grdSubsidiaryTrial.Columns[3].Visible = true;

                }
                else
                {
                    grdSubsidiaryTrial.Columns[3].Visible = false;

                }


                DataRow DrNew8 = dtSubsidiary_New.NewRow();

                if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                    DrNew8[1] = "Total";
                else
                    DrNew8[0] = "Total";


                if (SumDr == 0)
                {
                    DrNew8[4] = "";
                    DrNew8[9] = "";
                }
                else
                {
                    DrNew8[4] = SumDr.ToString("c", currencyFormat);
                    DrNew8[9] = oconverter.formatmoneyinUs(SumDr);

                }
                if (SumCr == 0)
                {
                    DrNew8[5] = "";
                    DrNew8[10] = "";
                }
                else
                {
                    DrNew8[5] = SumCr.ToString("c", currencyFormat);
                    DrNew8[10] = oconverter.formatmoneyinUs(SumCr);
                }
                dtSubsidiary_New.Rows.Add(DrNew8);
                //------------NET TOTAL---------

                DataRow DrNew5 = dtSubsidiary_New.NewRow();
                if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                    DrNew5[1] = "Net";
                else
                    DrNew5[0] = "Net";
                diffTot = SumDr - SumCr;
                if (diffTot >= 0)
                {
                    DrNew5[4] = oconverter.getFormattedvaluewithoriginalsign(diffTot);
                    DrNew5[9] = oconverter.formatmoneyinUs(diffTot);
                }
                else
                {
                    DrNew5[5] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(diffTot));
                    DrNew5[10] = oconverter.formatmoneyinUs(Math.Abs(diffTot));
                }
                dtSubsidiary_New.Rows.Add(DrNew5);
                //----


                ViewState["SumDr"] = SumDr.ToString("c", currencyFormat);
                ViewState["SumCr"] = SumCr.ToString("c", currencyFormat);
                ViewState["ExportTab"] = dtSubsidiary_New;

                if (Request.QueryString["mainacc"] != null)
                {
                    grdSubsidiaryTrial.Columns[1].Visible = false;
                    grdSubsidiaryTrial.Columns[0].Visible = false;
                }
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                    {
                        grdSubsidiaryTrial.Columns[0].Visible = false;
                        grdSubsidiaryTrial.Columns[9].Visible = false;
                    }
                    else
                    {
                        grdSubsidiaryTrial.Columns[0].Visible = true;
                        if (chkPhNumber.Checked == true)
                            grdSubsidiaryTrial.Columns[9].Visible = true;
                        else
                            grdSubsidiaryTrial.Columns[9].Visible = false;
                    }
                }
                if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                {
                    grdSubsidiaryTrial.Columns[0].Visible = false;
                    grdSubsidiaryTrial.Columns[9].Visible = false;
                }
                else
                {
                    grdSubsidiaryTrial.Columns[0].Visible = true;
                    if (chkPhNumber.Checked == true)
                    {
                        grdSubsidiaryTrial.Columns[9].Visible = true;
                    }

                }
                if (rdBoth.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[5].Visible = true;
                    grdSubsidiaryTrial.Columns[6].Visible = true;
                }
                else if (rdDebit.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[5].Visible = true;
                    grdSubsidiaryTrial.Columns[6].Visible = false;
                }
                else if (rdCredit.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[6].Visible = true;
                    grdSubsidiaryTrial.Columns[5].Visible = false;
                }


                diffTot = SumDr - SumCr;

                string RowHTML = "";
                string RowHTMLCr = "";
                string RowHTMLDr = "";
                RowHTML = "<table border=\"0\"><tr><td style=\"font-weight:bold;color:White;\">Total</td></tr><tr><td style=\"font-weight:bold;color:White;\">Net</td></tr></table>";
                if (diffTot >= 0)
                {
                    RowHTMLDr = "<table border=\"0\"><tr><td style=\"font-weight:bold;color:White;\">" + SumDr.ToString("c", currencyFormat) + "</td></tr><tr><td style=\"font-weight:bold;color:White;\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(diffTot)) + "</td></tr></table>";
                    RowHTMLCr = "<table border=\"0\"><tr><td style=\"font-weight:bold;color:White;\">" + SumCr.ToString("c", currencyFormat) + "</td></tr><tr><td>&nbsp;</td></tr></table>";
                }
                else
                {
                    RowHTMLDr = "<table><tr><td style=\"font-weight:bold;color:White;\">" + SumDr.ToString("c", currencyFormat) + "</td></tr><tr ><td>&nbsp;</td></tr></table>";
                    RowHTMLCr = "<table><tr><td style=\"font-weight:bold;color:White;\">" + SumCr.ToString("c", currencyFormat) + "</td></tr><tr ><td style=\"font-weight:bold;color:White;\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(diffTot)) + "</td></tr></table>";
                }

                ViewState["RowHTMLDr"] = RowHTMLDr;
                ViewState["RowHTMLCr"] = RowHTMLCr;
                ViewState["RowHTML"] = RowHTML;

                //if (rdbMainSelected.Checked == true)
                //{
                if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                    grdSubsidiaryTrial.FooterRow.Cells[1].Text = RowHTML;
                else
                    grdSubsidiaryTrial.FooterRow.Cells[0].Text = RowHTML;
                // }

                if (SumDr == 0)
                    grdSubsidiaryTrial.FooterRow.Cells[5].Text = "";
                else
                    grdSubsidiaryTrial.FooterRow.Cells[5].Text = RowHTMLDr;
                if (SumCr == 0)
                    grdSubsidiaryTrial.FooterRow.Cells[6].Text = "";
                else
                    grdSubsidiaryTrial.FooterRow.Cells[6].Text = RowHTMLCr;



                //if (rdbMainSelected.Checked == true)
                //{
                //    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients")
                //        grdSubsidiaryTrial.FooterRow.Cells[1].Text = "Total";
                //    else
                //        grdSubsidiaryTrial.FooterRow.Cells[0].Text = "Total";
                //}

                //if (SumDr == 0)
                //    grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
                //else
                //    grdSubsidiaryTrial.FooterRow.Cells[4].Text = SumDr.ToString("c", currencyFormat);
                //if (SumCr == 0)
                //    grdSubsidiaryTrial.FooterRow.Cells[5].Text = "";
                //else
                //    grdSubsidiaryTrial.FooterRow.Cells[5].Text = SumCr.ToString("c", currencyFormat);


                //grdSubsidiaryTrial.FooterRow.Visible = true;


                //if (rdbMainSelected.Checked == true)
                //{
                //    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients")
                //        grdSubsidiaryTrial.FooterRow.Cells[1].Text = "Total";
                //    else
                //        grdSubsidiaryTrial.FooterRow.Cells[0].Text = "Total";
                //}

                //if (SumDr == 0)
                //    grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
                //else
                //    grdSubsidiaryTrial.FooterRow.Cells[4].Text = SumDr.ToString("c", currencyFormat);
                //if (SumCr == 0)
                //    grdSubsidiaryTrial.FooterRow.Cells[5].Text = "";
                //else
                //    grdSubsidiaryTrial.FooterRow.Cells[5].Text = SumCr.ToString("c", currencyFormat);


                grdSubsidiaryTrial.FooterRow.Visible = true;


                grdSubsidiaryTrial.FooterRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#507CD1");
                grdSubsidiaryTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrial.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrial.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrial.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[1].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[6].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[0].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[1].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[5].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[6].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[5].Wrap = false;
                grdSubsidiaryTrial.FooterRow.Cells[6].Wrap = false;
                //  ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
            }
            catch
            {
            }
        }
        protected void grdSubsidiaryTrial_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFE9BA';");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';");
            //}
            string rowID = String.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                rowID = "row" + e.Row.RowIndex;

                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);

                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtSubsidiary.Rows.Count + "'" + ")");

            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                //LinkFirst = string.Empty;
                //LinkPrev = string.Empty;
                //LinkNext = string.Empty;
                //LinkLast = string.Empty;
                //GridViewRow row = e.Row;
                //LinkButton btnFirst = (LinkButton)row.FindControl("FirstPage");
                //LinkButton btnPrevious = (LinkButton)row.FindControl("PreviousPage");
                //LinkButton btnNext = (LinkButton)row.FindControl("NextPage");
                //LinkButton btnLast = (LinkButton)row.FindControl("LastPage");
                //LinkFirst = btnFirst.ClientID;
                //LinkPrev = btnPrevious.ClientID;
                //LinkNext = btnNext.ClientID;
                //LinkLast = btnLast.ClientID;
            }
        }

        protected void BtnDropdown_Click(object sender, EventArgs e)
        {
            int p = 0;
            FillDropDown();
            if (cmbclientsPager.Items.Count > 1)
            {
                for (int i = 0; i < cmbclientsPager.Items.Count; i++)
                {
                    string[] ClientValue = cmbclientsPager.Items[i].Value.ToString().Split('~');
                    string Type = ClientValue[1].ToString().Trim();
                    if (Type != "MailToEmployee" && Type != "Customers" && Type != "Employees" && Type != "Relationship Partners" && Type != "Business Partners" && Type != "Brokers" && Type != "Sub Brokers" && Type != "Franchisees" && Type != "Vendors" && Type != "Data Vendors" && Type != "Recruitment Agents" && Type != "Agents" && Type != "Consultants" && Type != "Share Holder" && Type != "Debtors" && Type != "Creditors")
                    {
                        p = p + 1;
                    }

                }
                if (p > 0)
                {
                    string Type = "BranchVisibleFalse";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
                }
                else
                {
                    string Type = "Customers";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
                }

            }
            else
            {
                string[] ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                string Type = ClientValue[1].ToString().Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
            }
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            int curentIndex = cmbclientsPager.SelectedIndex;
            int totalNo = cmbclientsPager.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalClient.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbclientsPager.SelectedIndex = curentIndex;
            if (RadAsOnDate.Checked == true)
                FillGrid();
            else
                FillGridForPeriod();
        }
        public void FillDropDown()
        {
            MainAcc = HdnMainAcc.Value;
            if (MainAcc != "")
            {
                if (rdbMainAll.Checked == true)
                {
                    DataTable Clients = oDbEngine.GetDataTable("master_mainaccount", " distinct MainAccount_AccountCode+'~'+MainAccount_SubLedgerType as mainaccount_accountcode,ltrim(rtrim(mainaccount_name))+' ['+mainaccount_accountcode+']' as mainaccount_name", " mainaccount_accountcode in  (select distinct AccountsLedger_MainAccountID  from trans_accountsledger ) and  MainAccount_SubLedgerType != 'none' ");
                    cmbclientsPager.DataSource = Clients;
                    cmbclientsPager.DataValueField = "mainaccount_accountcode";
                    cmbclientsPager.DataTextField = "mainaccount_name";
                    cmbclientsPager.DataBind();

                }
                else
                {
                    DataTable Clients = oDbEngine.GetDataTable("master_mainaccount", " distinct MainAccount_AccountCode+'~'+MainAccount_SubLedgerType as mainaccount_accountcode,ltrim(rtrim(mainaccount_name))+' ['+mainaccount_accountcode+']' as mainaccount_name", " mainaccount_accountcode in(" + MainAcc + ")");
                    cmbclientsPager.DataSource = Clients;
                    cmbclientsPager.DataValueField = "mainaccount_accountcode";
                    cmbclientsPager.DataTextField = "mainaccount_name";
                    cmbclientsPager.DataBind();
                }
            }
            else
            {
                DataTable Clients = oDbEngine.GetDataTable("master_mainaccount", " distinct MainAccount_AccountCode+'~'+MainAccount_SubLedgerType as mainaccount_accountcode,ltrim(rtrim(mainaccount_name))+' ['+mainaccount_accountcode+']' as mainaccount_name", " mainaccount_accountcode in  (select distinct AccountsLedger_MainAccountID  from trans_accountsledger ) and  MainAccount_SubLedgerType != 'none' ");
                cmbclientsPager.DataSource = Clients;
                cmbclientsPager.DataValueField = "mainaccount_accountcode";
                cmbclientsPager.DataTextField = "mainaccount_name";
                cmbclientsPager.DataBind();



            }
            //FillGrid();
        }
        protected void cmbclientsPager_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllistType.SelectedItem.Value.ToString() == "0")
            {
                //MainAcc = cmbclientsPager.SelectedItem.Value;
                if (RadAsOnDate.Checked == true)
                {
                    lblReportHeader.Text = "Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
                    FillGrid();
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "height();", true);
                }
                else
                {
                    lblReportHeader.Text = "Subsidiary Trial Period From Date [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "] ";
                    FillGridForPeriod();
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "height();", true);

                }
            }

        }

        //protected void grdSubsidiaryTrial_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //string sortExpression = e.SortExpression;

        //if (GridViewSortDirection == SortDirection.Ascending)
        //{

        //    GridViewSortDirection = SortDirection.Descending;

        //    SortGridView(sortExpression, " DESC");

        //}

        //else
        //{

        //    GridViewSortDirection = SortDirection.Ascending;

        //    SortGridView(sortExpression, " ASC");

        //}
        //}
        //public SortDirection GridViewSortDirection
        //{

        //get
        //{

        //    if (ViewState["sortDirection"] == null)

        //        ViewState["sortDirection"] = SortDirection.Ascending;

        //    return (SortDirection)ViewState["sortDirection"];

        //}

        //set { ViewState["sortDirection"] = value; }

        //}
        //private void SortGridView(string sortExpression, string direction)
        //{

        //// You can cache the DataTable for improving performance

        ////DataTable dt = dtSubsidiary;
        //dtSubsidiary = (DataTable)ViewState["ExportTab"];
        //System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
        //currencyFormat.CurrencySymbol = "";
        //currencyFormat.CurrencyNegativePattern = 2;
        //DataView dv = new DataView(dtSubsidiary);

        //dv.Sort = sortExpression + direction;
        ////decimal SumDr = Convert.ToDecimal(dtSubsidiary.Compute("sum(DR)", ""));
        ////decimal SumCr = Convert.ToDecimal(dtSubsidiary.Compute("sum(CR)", ""));
        //grdSubsidiaryTrial.DataSource = dv;
        //grdSubsidiaryTrial.DataBind();

        //if (rdbMainSelected.Checked == true)
        //{
        //    if (ViewState["SubType"] != "Customers" && ViewState["SubType"] != "NSDL Clients" && ViewState["SubType"] != "CDSL Clients")
        //        grdSubsidiaryTrial.FooterRow.Cells[1].Text = ViewState["RowHTML"].ToString();
        //    else
        //        grdSubsidiaryTrial.FooterRow.Cells[0].Text = ViewState["RowHTML"].ToString();
        //}

        //grdSubsidiaryTrial.FooterRow.Cells[4].Text = ViewState["RowHTMLDr"].ToString();
        //grdSubsidiaryTrial.FooterRow.Cells[5].Text = ViewState["RowHTMLCr"].ToString();
        ////if (SumDr == 0)
        ////    grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
        ////else
        ////    grdSubsidiaryTrial.FooterRow.Cells[4].Text = ViewState["RowHTMLDr"].ToString();
        ////if (SumCr == 0)
        ////    grdSubsidiaryTrial.FooterRow.Cells[5].Text = "";
        ////else
        ////    grdSubsidiaryTrial.FooterRow.Cells[5].Text = ViewState["RowHTMLCr"].ToString();

        //grdSubsidiaryTrial.FooterRow.Visible = true;

        ////grdSubsidiaryTrial.FooterRow.Cells[0].Text = "Total";
        ////if (SumDr == 0)
        ////    grdSubsidiaryTrial.FooterRow.Cells[3].Text = "";
        ////else
        ////    grdSubsidiaryTrial.FooterRow.Cells[3].Text = SumDr.ToString("c", currencyFormat);
        ////if (SumCr == 0)
        ////    grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
        ////else
        ////    grdSubsidiaryTrial.FooterRow.Cells[4].Text = SumCr.ToString("c", currencyFormat);
        //grdSubsidiaryTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
        //grdSubsidiaryTrial.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        //grdSubsidiaryTrial.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
        //grdSubsidiaryTrial.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        //grdSubsidiaryTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
        //grdSubsidiaryTrial.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
        //grdSubsidiaryTrial.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
        //grdSubsidiaryTrial.FooterRow.Cells[0].Font.Bold = true;
        //grdSubsidiaryTrial.FooterRow.Cells[3].Font.Bold = true;
        //grdSubsidiaryTrial.FooterRow.Cells[4].Font.Bold = true;
        //grdSubsidiaryTrial.FooterRow.Cells[3].Wrap = false;
        //grdSubsidiaryTrial.FooterRow.Cells[4].Wrap = false;
        //}

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }
        public void FillGridForPeriod()
        {
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;

            DataSet DS = new DataSet();
            DataTable dtSubsidiary_New = new DataTable();
            decimal Amountdr = 0;
            decimal AmountCr = 0;
            decimal OpenDr = 0;
            decimal OpenCr = 0;
            decimal CloseDr = 0;
            decimal CloseCr = 0;
            string SelectMainForSub = null;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 15;
            string WehereMainAccount = null;
            string WhereMainBranch = null;
            decimal SumForBranchDR = 0;
            decimal SumForBranchCR = 0;
            decimal DifOfDRCR = 0;
            string BranchName = null;
            DateTime date;
            string[] ClientValue = null;
            if (rdbMainSelected.Checked == true)
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
            else
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');

            if (rdbMainSelected.Checked == true)
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }
            else
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }

            ///------------------------New Part --------------------------
            #region New Part
            string MainAccID = "";
            string SubAccId = "";
            string Brnch = "";
            string Segmnt = "";
            decimal DrCrAmt = 0;
            string Grp = "";
            string RptType = "";
            string BranchGroupType = "";
            string GroupType = "";
            String CheckDrCr = "";
            string ZeroBal = "";

            //Chenge For Consolidate Account Feature
            if (!chkConsolidateAccounts.Checked)
                MainAccID = ClientValue[0].ToString();
            else
                MainAccID = MainAcc.Replace("'", "").Replace(",", "','");
            if (RadAsOnDate.Checked == true)
                RptType = "A";
            else
                RptType = "P";

            Brnch = Session["userbranchHierarchy"].ToString();

            if (rdbClientSelected.Checked == true)
                SubAccId = HdnClients.Value;
            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
            {
                if (ddlGroup.SelectedItem.Value == "0")
                {
                    BranchGroupType = "B";
                    if (rdbMainSelected.Checked == true)
                    {
                        if (rdbranchAll.Checked == true)
                            Brnch = Session["userbranchHierarchy"].ToString();
                        else
                            Brnch = BranchId;

                    }
                }
                else
                {
                    BranchGroupType = "G";
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                        {
                            if (rdddlgrouptypeAll.Checked == true)
                            {
                                if (ddlgrouptype.SelectedItem.Value == "0")
                                    GroupType = "N";
                                else
                                    GroupType = ddlgrouptype.SelectedItem.Text.ToString();
                            }
                            else
                                Grp = Group;
                        }

                    }
                }
            }



            if (rdDebit.Checked == true)
                CheckDrCr = "D";
            else if (rdCredit.Checked == true)
                CheckDrCr = "C";
            else
                CheckDrCr = "B";

            Segmnt = ViewState["SegmentID"].ToString();

            if (txtDebitCredit.Text != "")
                DrCrAmt = Convert.ToDecimal(txtDebitCredit.Text);

            if (rdAllSegment.Checked == true)
            {
                DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "*", null);
                Segmnt = DT.Rows[0][0].ToString();
                for (int i = 1; i < DT.Rows.Count; i++)
                {
                    Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                    ViewState["SegmentID"] = Segmnt;
                }
            }
            else
            {
                // Segmnt = ViewState["SegmentID"].ToString();

                if (Session["userlastsegment"].ToString() == "5")
                {
                    DataTable DtSeg = new DataTable();
                    DtSeg = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                    Segmnt = DtSeg.Rows[0][1].ToString();

                }
                else
                {

                    if (HdnSegment.Value != "")
                    {
                        Segmnt = HdnSegment.Value;
                        ViewState["SegmentID"] = HdnSegment.Value;
                    }
                    else
                    {
                        Segmnt = ViewState["SegmentID"].ToString();
                    }
                }
            }
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            //if (HdnSegment.Value != "")
            //    {
            //    Segmnt = HdnSegment.Value;
            //    ViewState["SegmentID"] = HdnSegment.Value;
            //}
            //else
            //{
            //    Segmnt = ViewState["SegmentID"].ToString();
            //}
            //DataSet DS = new DataSet();

            //For Sever Debugger Variable
            string[,] strParam = new string[22, 2];
            string SpName = String.Empty;
            //SegmentWise BreakUP
            string strSubLedgerType = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_SubSideryTrial", con))
                {
                    SqlParameter oparaType = new SqlParameter("@LType", SqlDbType.VarChar, 100);
                    oparaType.Direction = ParameterDirection.Output;
                    da.SelectCommand.Parameters.AddWithValue("@MainAccountID", MainAccID);
                    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                    da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
                    da.SelectCommand.Parameters.AddWithValue("@Segment", Segmnt);
                    da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@Company", Session["LastCompany"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@DrCrAmt", DrCrAmt);
                    da.SelectCommand.Parameters.AddWithValue("@Group", Grp);
                    da.SelectCommand.Parameters.AddWithValue("@ReportType", RptType);
                    da.SelectCommand.Parameters.AddWithValue("@BranchGroutType", BranchGroupType);
                    da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                    da.SelectCommand.Parameters.AddWithValue("@ShowStatus", CheckDrCr);
                    da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                    ///Currency Setting
                    da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);

                    ///Segment Wise BreakUp Parameter
                    da.SelectCommand.Parameters.AddWithValue("@IsSegmentWiseBreakUp", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsTDayBal", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsConsolidate", "N");
                    //For Consolidate Same Type Of Subledger Accounts
                    da.SelectCommand.Parameters.AddWithValue("@IsAcConsolidate", chkConsolidateAccounts.Checked ? "T" : "F");

                    //SD Code (Server Debugging Code)
                    strParam[0, 0] = "MainAccountID"; strParam[0, 1] = "'" + MainAccID.Replace("'", "''") + "'";
                    strParam[1, 0] = "SubAccount"; strParam[1, 1] = "'" + SubAccId + "'";
                    strParam[2, 0] = "Branch"; strParam[2, 1] = "'" + Brnch + "'";
                    strParam[3, 0] = "FromDate"; strParam[3, 1] = "'" + Convert.ToDateTime(dtFrom.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                    strParam[4, 0] = "ToDate"; strParam[4, 1] = "'" + Convert.ToDateTime(dtDate.Value.ToString()).ToString("yyyy-MM-dd") + "'";
                    strParam[5, 0] = "Segment"; strParam[5, 1] = "'" + Segmnt + "'";
                    strParam[6, 0] = "FinancialYr"; strParam[6, 1] = "'" + Session["LastFinYear"].ToString() + "'";
                    strParam[7, 0] = "Company"; strParam[7, 1] = "'" + Session["LastCompany"].ToString() + "'";
                    strParam[8, 0] = "DrCrAmt"; strParam[8, 1] = "'" + DrCrAmt.ToString() + "'";
                    strParam[9, 0] = "Group"; strParam[9, 1] = "'" + Grp + "'";
                    strParam[10, 0] = "ReportType"; strParam[10, 1] = "'" + RptType + "'";
                    strParam[11, 0] = "BranchGroutType"; strParam[11, 1] = "'" + BranchGroupType + "'";
                    strParam[12, 0] = "GroupType"; strParam[12, 1] = "'" + GroupType + "'";
                    strParam[13, 0] = "ShowStatus"; strParam[13, 1] = "'" + CheckDrCr + "'";
                    strParam[14, 0] = "ZeroBal"; strParam[14, 1] = "'" + ZeroBal + "'";
                    strParam[15, 0] = "ActiveCurrency"; strParam[15, 1] = "'" + Session["ActiveCurrency"].ToString().Split('~')[0] + "'";
                    strParam[16, 0] = "TradeCurrency"; strParam[16, 1] = "'" + Session["TradeCurrency"].ToString().Split('~')[0] + "'";
                    strParam[17, 0] = "IsSegmentWiseBreakUp"; strParam[17, 1] = "'N'";
                    strParam[18, 0] = "IsTDayBal"; strParam[18, 1] = "'N'";
                    strParam[19, 0] = "IsConsolidate"; strParam[19, 1] = "'N'";
                    strParam[20, 0] = "IsAcConsolidate"; strParam[20, 1] = "'" + (chkConsolidateAccounts.Checked ? "T" : "F") + "'";
                    strParam[21, 0] = "LType"; strParam[21, 1] = "''";
                    //For Server Debugging Purpose
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 2)
                    {
                        string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                        string FilePath = "../ExportFiles/ServerDebugging/Fetch_SubSideryTrial" + strDateTime + ".txt";
                        oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, "Fetch_SubSideryTrial"), FilePath, false);
                    }
                    da.SelectCommand.Parameters.Add(oparaType);


                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    DS.Reset();
                    da.Fill(DS);
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                    dtSubsidiary = DS.Tables[0];
                    //ViewState["dataset"] = ds;
                    strSubLedgerType = Convert.ToString(oparaType.Value);

                }
            }
            if (DS.Tables[0].Rows.Count > 0)
            {
                if (SendEmailTag != "Y")
                {

                    lblReportHeader.Text = "Subsidiary Trial Period From Date [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "] ";
                }
            }
            else
            {
                if (SendEmailTag != "Y")
                {
                    lblReportHeader.Text = "No Record Found!...";
                }
            }

            #endregion

            ///-------------------------------------------------------
            dtSubsidiary_New.Columns.Add("BranchName");
            dtSubsidiary_New.Columns.Add("MainID");
            dtSubsidiary_New.Columns.Add("SubID");
            dtSubsidiary_New.Columns.Add("accountsledger_mainaccountid");
            dtSubsidiary_New.Columns.Add("accountsledger_subaccountid");
            dtSubsidiary_New.Columns.Add("Ucc");
            //dtSubsidiary_New.Columns.Add("PhoneNumber");

            dtSubsidiary_New.Columns.Add("OpeningDR");
            dtSubsidiary_New.Columns.Add("OpeningCR");
            dtSubsidiary_New.Columns.Add("AmountDR");
            dtSubsidiary_New.Columns.Add("AmountCR");
            dtSubsidiary_New.Columns.Add("ClosingDR");
            dtSubsidiary_New.Columns.Add("ClosingCR");

            dtSubsidiary_New.Columns.Add("ExOpeningDR");
            dtSubsidiary_New.Columns.Add("ExOpeningCR");
            dtSubsidiary_New.Columns.Add("ExAmountDR");
            dtSubsidiary_New.Columns.Add("ExAmountCR");
            dtSubsidiary_New.Columns.Add("ExClosingDR");
            dtSubsidiary_New.Columns.Add("ExClosingCR");
            dtSubsidiary_New.Columns.Add("Trading_UCC");
            if (ViewState["FirstTime"] == null)
            {
                //info.AddMergedColumns(new int[] { 4, 5 }, "Opening");
                //info.AddMergedColumns(new int[] { 6, 7 }, "During the Period");
                //info.AddMergedColumns(new int[] { 8, 9 }, "Closing");
                info.AddMergedColumns(new int[] { 5, 6 }, "Opening");
                info.AddMergedColumns(new int[] { 7, 8 }, "During the Period");
                info.AddMergedColumns(new int[] { 9, 10 }, "Closing");
            }
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {

                if (i == 0)
                    BranchName = DS.Tables[0].Rows[i]["BRANCH"].ToString();
                if (DS.Tables[0].Rows[i]["BRANCH"].ToString() != BranchName)
                {
                    if (chkBranchNet.Checked == true)
                    {
                        DataRow DrNew1 = dtSubsidiary_New.NewRow();
                        DrNew1[0] = "Total";
                        DrNew1[10] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                        DrNew1[11] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);

                        DrNew1[16] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                        DrNew1[17] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);

                        dtSubsidiary_New.Rows.Add(DrNew1);
                        DifOfDRCR = SumForBranchDR - SumForBranchCR;
                        DataRow DrNew2 = dtSubsidiary_New.NewRow();
                        DrNew2[0] = "Branch/Group Net";
                        if (DifOfDRCR >= 0)
                        {
                            DrNew2[10] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                            DrNew2[16] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                        }
                        else
                        {
                            DrNew2[11] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                            DrNew2[17] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                        }
                        dtSubsidiary_New.Rows.Add(DrNew2);
                        DifOfDRCR = 0;
                        SumForBranchDR = 0;
                        SumForBranchCR = 0;
                        DataRow DrNewBlank = dtSubsidiary_New.NewRow();
                        DrNewBlank[0] = "Blank";
                        dtSubsidiary_New.Rows.Add(DrNewBlank);
                    }
                }
                DataRow DrNew = dtSubsidiary_New.NewRow();
                DrNew[0] = DS.Tables[0].Rows[i]["BRANCH"].ToString();
                DrNew[1] = DS.Tables[0].Rows[i]["MAINACCOUNTID"].ToString();
                DrNew[2] = DS.Tables[0].Rows[i]["SUBACCOUNTID"].ToString();
                DrNew[3] = DS.Tables[0].Rows[i]["MainAccountName"].ToString();
                DrNew[4] = DS.Tables[0].Rows[i]["ACCOUNT_NAME"].ToString();
                DrNew[5] = DS.Tables[0].Rows[i]["UCC"].ToString();


                if (DS.Tables[0].Rows[i]["AmDr"] != DBNull.Value)
                {
                    Amountdr += Convert.ToDecimal(DS.Tables[0].Rows[i]["AmDr"].ToString());
                    DrNew[8] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmDr"].ToString()));
                    DrNew[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmDr"].ToString()));

                }
                else
                {
                    DrNew[8] = "";
                    DrNew[14] = "";
                }
                if (DS.Tables[0].Rows[i]["AmCr"] != DBNull.Value)
                {
                    AmountCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["AmCr"].ToString());
                    DrNew[9] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmCr"].ToString()));
                    DrNew[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmCr"].ToString()));
                }
                else
                {
                    DrNew[9] = "";
                    DrNew[15] = "";
                }
                if (DS.Tables[0].Rows[i]["OpDr"] != DBNull.Value)
                {
                    OpenDr += Convert.ToDecimal(DS.Tables[0].Rows[i]["OpDr"].ToString());
                    DrNew[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpDr"].ToString()));
                    DrNew[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpDr"].ToString()));

                }
                else
                {
                    DrNew[6] = "";
                    DrNew[12] = "";
                }
                if (DS.Tables[0].Rows[i]["OpCr"] != DBNull.Value)
                {
                    OpenCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["OpCr"].ToString());
                    // DS.Tables[0].Rows[i]["OpCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpCr"].ToString()));
                    DrNew[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpCr"].ToString()));
                    DrNew[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpCr"].ToString()));
                }
                else
                {
                    DrNew[7] = "";
                    DrNew[13] = "";
                }
                if (DS.Tables[0].Rows[i]["ClDr"] != DBNull.Value)
                {
                    CloseDr += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClDr"].ToString());
                    SumForBranchDR += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClDr"].ToString());
                    DrNew[10] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClDr"].ToString()));
                    DrNew[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClDr"].ToString()));
                }
                else
                {
                    DrNew[16] = "";
                    DrNew[10] = "";
                }
                if (DS.Tables[0].Rows[i]["ClCr"] != DBNull.Value)
                {
                    CloseCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClCr"].ToString());
                    SumForBranchCR += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClCr"].ToString());
                    DrNew[11] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClCr"].ToString()));
                    DrNew[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClCr"].ToString()));
                }
                else
                {
                    DrNew[17] = "";
                    DrNew[11] = "";
                }
                DrNew[18] = DS.Tables[0].Rows[i]["Trading_UCC"].ToString();
                BranchName = DS.Tables[0].Rows[i]["BRANCH"].ToString();
                dtSubsidiary_New.Rows.Add(DrNew);
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() == "Customers")
                    {
                        if (chkBranchNet.Checked == true)
                        {
                            if (i == dtSubsidiary.Rows.Count - 1)
                            {
                                DataRow DrNew3 = dtSubsidiary_New.NewRow();
                                DrNew3[0] = "Total";
                                DrNew3[10] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                                DrNew3[11] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);

                                DrNew3[16] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                                DrNew3[17] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);


                                dtSubsidiary_New.Rows.Add(DrNew3);
                                DifOfDRCR = SumForBranchDR - SumForBranchCR;
                                DataRow DrNew4 = dtSubsidiary_New.NewRow();
                                DrNew4[0] = "Branch Net";
                                if (DifOfDRCR >= 0)
                                {
                                    DrNew4[10] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                                    DrNew4[16] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                                }
                                else
                                {
                                    DrNew4[11] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                                    DrNew4[17] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                                }
                                dtSubsidiary_New.Rows.Add(DrNew4);
                                DifOfDRCR = 0;
                                SumForBranchDR = 0;
                                SumForBranchCR = 0;
                            }
                        }
                    }
                }
            }
            ViewState["ExportTab"] = dtSubsidiary_New;
            ViewState["AmDr"] = Amountdr.ToString("c", currencyFormat);
            ViewState["AmCr"] = AmountCr.ToString("c", currencyFormat);
            ViewState["OpDr"] = OpenDr.ToString("c", currencyFormat);
            ViewState["OpCr"] = OpenCr.ToString("c", currencyFormat);
            ViewState["ClDr"] = CloseDr.ToString("c", currencyFormat);
            ViewState["ClCr"] = CloseCr.ToString("c", currencyFormat);
            if (strSubLedgerType == "NSDL Clients" || strSubLedgerType == "CDSL Clients")
            {

                strHtext = "BenAccount No";

            }
            else
            {
                strHtext = "UCC";

            }
            grdSubsidiaryTrialPeriod.DataSource = dtSubsidiary_New;
            grdSubsidiaryTrialPeriod.DataBind();
            if (strSubLedgerType == "NSDL Clients" || strSubLedgerType == "CDSL Clients")
                grdSubsidiaryTrialPeriod.Columns[4].Visible = true;
            else
                grdSubsidiaryTrialPeriod.Columns[4].Visible = false;

            if (dtSubsidiary_New.Rows.Count > 0)
            {
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                    {
                        grdSubsidiaryTrialPeriod.Columns[0].Visible = false;
                        grdSubsidiaryTrialPeriod.FooterRow.Cells[1].Text = "Total";
                    }
                    else
                    {
                        grdSubsidiaryTrialPeriod.Columns[0].Visible = true;
                        grdSubsidiaryTrialPeriod.FooterRow.Cells[0].Text = "Total";
                    }
                }
                if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients" && ClientValue[1].ToString().Trim() != "Employees" && ClientValue[1].ToString().Trim() != "Relationship Partners" && ClientValue[1].ToString().Trim() != "Business Partners" && ClientValue[1].ToString().Trim() != "Brokers" && ClientValue[1].ToString().Trim() != "Sub Brokers" && ClientValue[1].ToString().Trim() != "Franchisees" && ClientValue[1].ToString().Trim() != "Vendors" && ClientValue[1].ToString().Trim() != "Data Vendors" && ClientValue[1].ToString().Trim() != "Recruitment Agents" && ClientValue[1].ToString().Trim() != "Agents" && ClientValue[1].ToString().Trim() != "Consultants" && ClientValue[1].ToString().Trim() != "Share Holder" && ClientValue[1].ToString().Trim() != "Debtors" && ClientValue[1].ToString().Trim() != "Creditors")
                {
                    grdSubsidiaryTrialPeriod.Columns[0].Visible = false;
                    //  grdSubsidiaryTrialPeriod.Columns[8].Visible = false;
                }
                //else
                //{
                //    grdSubsidiaryTrialPeriod.Columns[0].Visible = true;
                //    if (chkPhNumber.Checked == true)
                //    {
                //        grdSubsidiaryTrialPeriod.Columns[8].Visible = true;
                //    }

                //}


                if (OpenDr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Text = OpenDr.ToString("c", currencyFormat);
                if (OpenCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Text = OpenCr.ToString("c", currencyFormat);
                if (Amountdr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Text = Amountdr.ToString("c", currencyFormat);
                if (AmountCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Text = AmountCr.ToString("c", currencyFormat);
                if (CloseDr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Text = CloseDr.ToString("c", currencyFormat);
                if (CloseCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[10].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[10].Text = CloseCr.ToString("c", currencyFormat);
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[10].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[10].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[10].Wrap = false;


            }
            // ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
            ViewState["FirstTime"] = "FirstTime";
        }
        protected void NavigationLinkPeriod_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPagePeriod.Value) + 1;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPagePeriod.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPagesPeriod.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            FillGridForPeriod();
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtExport = new DataTable();
            for (int i = 0; i < cmbclientsPager.Items.Count; i++)
            {
                cmbclientsPager.SelectedValue = cmbclientsPager.Items[i].Value;
                if (!cmbclientsPager.SelectedValue.Split('~')[1].Contains("Pro"))
                {
                    if (RadAsOnDate.Checked == true)
                    {
                        if (rdbMainAll.Checked)
                        {
                            FillGrid();
                        }
                        else
                        {
                            if (i != 0)
                            {
                                FillGrid();
                            }
                        }
                        dtSubsidiary = (DataTable)ViewState["ExportTab"];
                        dtSubsidiary.Columns[1].ColumnName = "Main Account";
                        dtSubsidiary.Columns[2].ColumnName = "Sub Account";
                        dtSubsidiary.Columns[3].ColumnName = "UCC";
                        dtSubsidiary.Columns[9].ColumnName = "Debit";
                        dtSubsidiary.Columns[10].ColumnName = "Credit";
                        dtSubsidiary.Columns.Remove("MainID");
                        dtSubsidiary.Columns.Remove("SubID");
                        dtSubsidiary.Columns.Remove("AmountDR");
                        dtSubsidiary.Columns.Remove("AmountCR");
                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            dtSubsidiary.Columns.Remove("BranchName");
                            dtSubsidiary.Columns.Remove("PhoneNumber");

                        }
                        else
                        {
                            if (chkPhNumber.Checked == false)
                            {

                                dtSubsidiary.Columns.Remove("PhoneNumber");

                            }

                        }

                    }
                    else
                    {
                        if (i != 0)
                        {
                            FillGridForPeriod();
                        }
                        //FillGridForPeriod();
                        dtSubsidiary = (DataTable)ViewState["ExportTab"];
                        dtSubsidiary.Columns[3].ColumnName = "Main Account";
                        dtSubsidiary.Columns[4].ColumnName = "Sub Account";
                        dtSubsidiary.Columns[5].ColumnName = "UCC";
                        dtSubsidiary.Columns[12].ColumnName = "Opening Debit";
                        dtSubsidiary.Columns[13].ColumnName = "Opening Credit";
                        dtSubsidiary.Columns[14].ColumnName = "Amount Debit";
                        dtSubsidiary.Columns[15].ColumnName = "Amount Credit";
                        dtSubsidiary.Columns[16].ColumnName = "Closing Debit";
                        dtSubsidiary.Columns[17].ColumnName = "Closing Credit";
                        dtSubsidiary.Columns.Remove("OpeningDR");
                        dtSubsidiary.Columns.Remove("OpeningCR");
                        dtSubsidiary.Columns.Remove("AmountDR");
                        dtSubsidiary.Columns.Remove("AmountCR");
                        dtSubsidiary.Columns.Remove("ClosingDR");
                        dtSubsidiary.Columns.Remove("ClosingCR");


                        DataRow NewRow = dtSubsidiary.NewRow();

                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            NewRow[4] = "Total";
                        }
                        else
                        {
                            NewRow[4] = "Total";
                        }
                        NewRow[5] = "";

                        if (ViewState["OpDr"] != null)
                            NewRow[6] = ViewState["OpDr"].ToString();
                        else
                            NewRow[6] = "0.00";
                        if (ViewState["OpCr"] != null)
                            NewRow[7] = ViewState["OpCr"].ToString();
                        else
                            NewRow[7] = "0.00"; ;
                        if (ViewState["AmDr"] != null)
                            NewRow[8] = ViewState["AmDr"].ToString();
                        else
                            NewRow[8] = "0.00";
                        if (ViewState["AmCr"] != null)
                            NewRow[9] = ViewState["AmCr"].ToString();
                        else
                            NewRow[9] = "0.000";
                        if (ViewState["ClDr"] != null)
                            NewRow[10] = ViewState["ClDr"].ToString();
                        else
                            NewRow[10] = "0.00";
                        if (ViewState["ClCr"] != null)
                            NewRow[11] = ViewState["ClCr"].ToString();
                        else
                            NewRow[11] = "0.00";
                        dtSubsidiary.Rows.Add(NewRow);

                        dtSubsidiary.Columns.Remove("MainID");
                        dtSubsidiary.Columns.Remove("SubID");
                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            dtSubsidiary.Columns.Remove("BranchName");

                            //  dtSubsidiary.Columns.Remove("PhoneNumber");
                        }

                    }






                    if (ViewState["SubType"].ToString().Trim() == "Customers" || ViewState["SubType"].ToString().Trim() == "NSDL Clients" || ViewState["SubType"].ToString().Trim() == "CDSL Clients" || ViewState["SubType"].ToString().Trim() == "Employees" || ViewState["SubType"].ToString().Trim() == "Relationship Partners" || ViewState["SubType"].ToString().Trim() == "Business Partners" || ViewState["SubType"].ToString().Trim() == "Brokers" || ViewState["SubType"].ToString().Trim() == "Sub Brokers" || ViewState["SubType"].ToString().Trim() == "Franchisees" || ViewState["SubType"].ToString().Trim() == "Vendors" || ViewState["SubType"].ToString().Trim() == "Data Vendors" || ViewState["SubType"].ToString().Trim() == "Recruitment Agents" || ViewState["SubType"].ToString().Trim() == "Agents" || ViewState["SubType"].ToString().Trim() == "Consultants" || ViewState["SubType"].ToString().Trim() == "Share Holder" || ViewState["SubType"].ToString().Trim() == "Debtors" || ViewState["SubType"].ToString().Trim() == "Creditors")
                    {
                        dtSubsidiary.Columns.Remove("Main Account");
                        if (i == 0)
                        {
                            dtExport = dtSubsidiary.Clone();
                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        else
                        {

                            DataRow row8 = dtExport.NewRow();
                            row8[1] = "";
                            row8[2] = "Test";
                            dtExport.Rows.Add(row8);


                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                        {
                            if (k == 0)
                            {
                                DataRow row1 = dtExport.NewRow();
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    row1[1] = "Branch Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                                }
                                else
                                {
                                    row1[1] = "Group Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                                }
                                row1[2] = "Test";
                                dtExport.Rows.Add(row1);
                            }
                            if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Blank")
                            {
                                DataRow row9 = dtExport.NewRow();
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    row9[1] = "Branch Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                                }
                                else
                                {
                                    row9[1] = "Group Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                                }
                                row9[2] = "Test";
                                dtExport.Rows.Add(row9);

                            }
                            else
                            {
                                if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Total" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Group Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch/Group Net")
                                {
                                    dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["BranchName"].ToString();
                                    dtSubsidiary.AcceptChanges();
                                    dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                                }
                                else
                                {
                                    dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                                }
                            }

                        }


                    }
                    else
                    {

                        if (i == 0)
                        {
                            dtExport = dtSubsidiary.Clone();
                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        else
                        {
                            DataRow row8 = dtExport.NewRow();
                            row8[1] = "";
                            row8[2] = "Test";
                            dtExport.Rows.Add(row8);

                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }

                        for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                        {


                            if (dtSubsidiary.Rows[k]["Main Account"].ToString() == "Total" || dtSubsidiary.Rows[k]["Main Account"].ToString() == "Net")
                            {
                                dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["Main Account"].ToString();
                                dtSubsidiary.AcceptChanges();
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }
                            else
                            {
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }

                        }

                    }
                }
                else
                {
                    if (RadAsOnDate.Checked == true)
                    {
                        if (rdbMainAll.Checked)
                        {
                            FillGrid();
                        }
                        else
                        {
                            if (i != 0)
                            {
                                FillGrid();
                            }
                        }
                        dtSubsidiary = (DataTable)ViewState["ExportTab"];
                        dtSubsidiary.Columns[1].ColumnName = "Main Account";
                        dtSubsidiary.Columns[2].ColumnName = "Sub Account";
                        dtSubsidiary.Columns[3].ColumnName = "UCC";
                        dtSubsidiary.Columns[9].ColumnName = "Debit";
                        dtSubsidiary.Columns[10].ColumnName = "Credit";
                        dtSubsidiary.Columns.Remove("MainID");
                        dtSubsidiary.Columns.Remove("SubID");
                        dtSubsidiary.Columns.Remove("AmountDR");
                        dtSubsidiary.Columns.Remove("AmountCR");
                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            dtSubsidiary.Columns.Remove("BranchName");
                            dtSubsidiary.Columns.Remove("PhoneNumber");

                        }
                        else
                        {
                            if (chkPhNumber.Checked == false)
                            {

                                dtSubsidiary.Columns.Remove("PhoneNumber");

                            }

                        }

                    }
                    else
                    {
                        if (i != 0)
                        {
                            FillGridForPeriod();
                        }
                        //FillGridForPeriod();
                        dtSubsidiary = (DataTable)ViewState["ExportTab"];
                        dtSubsidiary.Columns[3].ColumnName = "Main Account";
                        dtSubsidiary.Columns[4].ColumnName = "Sub Account";
                        dtSubsidiary.Columns[5].ColumnName = "UCC";
                        dtSubsidiary.Columns[12].ColumnName = "Opening Debit";
                        dtSubsidiary.Columns[13].ColumnName = "Opening Credit";
                        dtSubsidiary.Columns[14].ColumnName = "Amount Debit";
                        dtSubsidiary.Columns[15].ColumnName = "Amount Credit";
                        dtSubsidiary.Columns[16].ColumnName = "Closing Debit";
                        dtSubsidiary.Columns[17].ColumnName = "Closing Credit";
                        dtSubsidiary.Columns.Remove("OpeningDR");
                        dtSubsidiary.Columns.Remove("OpeningCR");
                        dtSubsidiary.Columns.Remove("AmountDR");
                        dtSubsidiary.Columns.Remove("AmountCR");
                        dtSubsidiary.Columns.Remove("ClosingDR");
                        dtSubsidiary.Columns.Remove("ClosingCR");


                        DataRow NewRow = dtSubsidiary.NewRow();

                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            NewRow[4] = "Total";
                        }
                        else
                        {
                            NewRow[4] = "Total";
                        }
                        NewRow[5] = "";

                        if (ViewState["OpDr"] != null)
                            NewRow[6] = ViewState["OpDr"].ToString();
                        else
                            NewRow[6] = "0.00";
                        if (ViewState["OpCr"] != null)
                            NewRow[7] = ViewState["OpCr"].ToString();
                        else
                            NewRow[7] = "0.00"; ;
                        if (ViewState["AmDr"] != null)
                            NewRow[8] = ViewState["AmDr"].ToString();
                        else
                            NewRow[8] = "0.00";
                        if (ViewState["AmCr"] != null)
                            NewRow[9] = ViewState["AmCr"].ToString();
                        else
                            NewRow[9] = "0.000";
                        if (ViewState["ClDr"] != null)
                            NewRow[10] = ViewState["ClDr"].ToString();
                        else
                            NewRow[10] = "0.00";
                        if (ViewState["ClCr"] != null)
                            NewRow[11] = ViewState["ClCr"].ToString();
                        else
                            NewRow[11] = "0.00";
                        dtSubsidiary.Rows.Add(NewRow);

                        dtSubsidiary.Columns.Remove("MainID");
                        dtSubsidiary.Columns.Remove("SubID");
                        if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                        {
                            dtSubsidiary.Columns.Remove("BranchName");

                            //  dtSubsidiary.Columns.Remove("PhoneNumber");
                        }

                    }






                    if (ViewState["SubType"].ToString().Trim() == "Customers" || ViewState["SubType"].ToString().Trim() == "NSDL Clients" || ViewState["SubType"].ToString().Trim() == "CDSL Clients" || ViewState["SubType"].ToString().Trim() == "Employees" || ViewState["SubType"].ToString().Trim() == "Relationship Partners" || ViewState["SubType"].ToString().Trim() == "Business Partners" || ViewState["SubType"].ToString().Trim() == "Brokers" || ViewState["SubType"].ToString().Trim() == "Sub Brokers" || ViewState["SubType"].ToString().Trim() == "Franchisees" || ViewState["SubType"].ToString().Trim() == "Vendors" || ViewState["SubType"].ToString().Trim() == "Data Vendors" || ViewState["SubType"].ToString().Trim() == "Recruitment Agents" || ViewState["SubType"].ToString().Trim() == "Agents" || ViewState["SubType"].ToString().Trim() == "Consultants" || ViewState["SubType"].ToString().Trim() == "Share Holder" || ViewState["SubType"].ToString().Trim() == "Debtors" || ViewState["SubType"].ToString().Trim() == "Creditors")
                    {
                        dtSubsidiary.Columns.Remove("Main Account");
                        if (i == 0)
                        {
                            dtExport = dtSubsidiary.Clone();
                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        else
                        {

                            DataRow row8 = dtExport.NewRow();
                            row8[1] = "";
                            row8[2] = "Test";
                            dtExport.Rows.Add(row8);


                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                        {
                            if (k == 0)
                            {
                                DataRow row1 = dtExport.NewRow();
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    row1[1] = "Branch Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                                }
                                else
                                {
                                    row1[1] = "Group Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                                }
                                row1[2] = "Test";
                                dtExport.Rows.Add(row1);
                            }
                            if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Blank")
                            {
                                DataRow row9 = dtExport.NewRow();
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    row9[1] = "Branch Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                                }
                                else
                                {
                                    row9[1] = "Group Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                                }
                                row9[2] = "Test";
                                dtExport.Rows.Add(row9);

                            }
                            else
                            {
                                if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Total" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Group Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch/Group Net")
                                {
                                    dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["BranchName"].ToString();
                                    dtSubsidiary.AcceptChanges();
                                    dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                                }
                                else
                                {
                                    dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                                }
                            }

                        }


                    }
                    else
                    {

                        if (i == 0)
                        {
                            dtExport = dtSubsidiary.Clone();
                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }
                        else
                        {
                            DataRow row8 = dtExport.NewRow();
                            row8[1] = "";
                            row8[2] = "Test";
                            dtExport.Rows.Add(row8);

                            DataRow row4 = dtExport.NewRow();
                            row4[1] = "Main Account :  " + cmbclientsPager.Items[i].Text.ToString().ToUpper();
                            row4[2] = "Test";
                            dtExport.Rows.Add(row4);
                        }

                        for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                        {


                            if (dtSubsidiary.Rows[k]["Main Account"].ToString() == "Total" || dtSubsidiary.Rows[k]["Main Account"].ToString() == "Net")
                            {
                                dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["Main Account"].ToString();
                                dtSubsidiary.AcceptChanges();
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }
                            else
                            {
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }

                        }

                    }


                }

            }//loop continued


            if (dtExport.Columns.Contains("BranchName"))
            {
                dtExport.Columns.Remove("BranchName");
            }
            else if (dtExport.Columns.Contains("Main Account"))
            {
                dtExport.Columns.Remove("Main Account");
            }

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = lblReportHeader.Text;
            dtReportHeader.Rows.Add(DrRowR1);
            //DataRow DrRowR2 = dtReportHeader.NewRow();
            //string ManAcc = "";
            ////for (int p = 0; p < cmbclientsPager.Items.Count; p++)
            ////{
            ////    if (p == 0)
            ////    {
            ////        ManAcc = ManAcc + cmbclientsPager.Items[p].Text.ToString().ToUpper();
            ////    }
            ////    else
            ////    {
            ////        ManAcc = ManAcc + " , " +  cmbclientsPager.Items[p].Text.ToString().ToUpper();

            ////    }
            ////}
            //DrRowR2[0] = "Main Account: " + ManAcc;
            //dtReportHeader.Rows.Add(DrRowR2);

            DataRow DrRowR3 = dtReportHeader.NewRow();
            if (rdAllSegment.Checked == true)
            {
                DrRowR3[0] = "Segment: ALL ";
            }
            else
            {
                DrRowR3[0] = "Segment: " + HDNSeg.Value.ToString();
            }
            dtReportHeader.Rows.Add(DrRowR3);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlExport.SelectedItem.Value == "E")
            {
                //oconverter.ExcelImport(dtBilling, "Daily Billing");
                if (dtExport != null && dtExport.Rows.Count > 0)
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Subsidiary Trial", "Total", dtReportHeader, dtReportFooter);
                }
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                if (dtExport != null && dtExport.Rows.Count > 0)
                {
                    objExcel.ExportToPDF(dtExport, "Subsidiary Trial", "Total", dtReportHeader, dtReportFooter);
                }
            }
        }
        private void RenderHeader(HtmlTextWriter output, System.Web.UI.Control container)
        {
            for (int i = 0; i < container.Controls.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[i];
                //stretch non merged columns for two rows
                if (!info.MergedColumns.Contains(i))
                {
                    cell.Attributes["rowspan"] = "2";
                    cell.RenderControl(output);
                }
                else //render merged columns common title
                    if (info.StartColumns.Contains(i))
                    {
                        output.Write(string.Format("<th align='center' colspan='{0}'>{1}</th>",
                                 info.StartColumns[i], info.Titles[i]));
                    }
            }

            //close the first row	
            output.RenderEndTag();
            //set attributes for the second row
            grdSubsidiaryTrialPeriod.HeaderStyle.AddAttributesToRender(output);
            //start the second row
            output.RenderBeginTag("tr");

            //render the second row (only the merged columns)
            for (int i = 0; i < info.MergedColumns.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[info.MergedColumns[i]];
                cell.RenderControl(output);
            }
        }
        private MergedColumnsInfo info
        {
            get
            {
                if (ViewState["info"] == null)
                    ViewState["info"] = new MergedColumnsInfo();
                return (MergedColumnsInfo)ViewState["info"];
            }
        }
        [Serializable]
        private class MergedColumnsInfo
        {
            // indexes of merged columns
            public List<int> MergedColumns = new List<int>();
            // key-value pairs: key = the first column index, value = number of the merged columns
            public Hashtable StartColumns = new Hashtable();
            // key-value pairs: key = the first column index, value = common title of the merged columns 
            public Hashtable Titles = new Hashtable();

            //parameters: the merged columns indexes, common title of the merged columns 
            public void AddMergedColumns(int[] columnsIndexes, string title)
            {
                MergedColumns.AddRange(columnsIndexes);
                StartColumns.Add(columnsIndexes[0], columnsIndexes.Length);
                Titles.Add(columnsIndexes[0], title);
            }
        }
        protected void grdSubsidiaryTrialPeriod_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.SetRenderMethodDelegate(RenderHeader);
            }


            string rowID = String.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                rowID = "row" + e.Row.RowIndex;

                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);

                e.Row.Attributes.Add("onclick", "ChangeRowColorForPeriod(" + "'" + rowID + "','" + dtSubsidiary.Rows.Count + "'" + ")");

            }
        }
        protected void grdSubsidiaryTrial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.Header)
            {
                if (ddlGroup.SelectedItem.Value == "0")
                    gvr.Cells[0].Text = "Branch Name";
                else
                    gvr.Cells[0].Text = "Group Name";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["SubType"] != null)
                {
                    //if (ViewState["SubType"].ToString().ToUpper() == "CUSTOMERS" || ViewState["SubType"].ToString().ToUpper() == "CDSL CLIENTS" || ViewState["SubType"].ToString().ToUpper() == "NSDL CLIENTS" || ViewState["SubType"].ToString().ToUpper() == "SUB BROKERS")
                    //{
                    string lblMainID = ((DataRowView)e.Row.DataItem)["MainID"].ToString();
                    string lblSubID = ((DataRowView)e.Row.DataItem)["SubID"].ToString();

                    string MainAccName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();
                    string SubAccName = ((DataRowView)e.Row.DataItem)["accountsledger_subaccountid"].ToString();
                    string UCC = ((DataRowView)e.Row.DataItem)["Ucc"].ToString();
                    string dt = Convert.ToDateTime(dtDate.Value.ToString()).ToShortDateString();

                    //Label lblMainID = (Label)e.Row.FindControl("lblMainID");
                    //Label lblSubID = (Label)e.Row.FindControl("lblSubID");
                    ((Label)e.Row.FindControl("lblTrDate")).Attributes.Add("onclick", "javascript:ShowLedger('" + lblMainID + "','" + lblSubID + "','" + ViewState["SegmentID"].ToString() + "','" + MainAccName + "','" + SubAccName + "','" + UCC + "','" + dt + "');");
                    e.Row.Cells[1].Style.Add("cursor", "hand");
                    e.Row.Cells[1].ToolTip = "Click to View Detail!";
                    //}
                }

                string BrName = ((DataRowView)e.Row.DataItem)["BranchName"].ToString();
                string AcName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();

                string AmtDr = ((DataRowView)e.Row.DataItem)["AmountDr"].ToString();
                string AmtCr = ((DataRowView)e.Row.DataItem)["AmountCr"].ToString();
                if (BrName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Branch Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Group Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Blank")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                else if (BrName == "Total")
                {
                    // e.Row.BackColor = System.Drawing.Color.Lavender;
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Maroon;

                }
                else if (BrName == "Net")
                {
                    //  e.Row.BackColor = System.Drawing.Color.Lavender;
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Maroon;
                }
                else if (AcName == "Total")
                {
                    // e.Row.BackColor = System.Drawing.Color.Lavender;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Maroon;

                }
                else if (AcName == "Net")
                {
                    //  e.Row.BackColor = System.Drawing.Color.Lavender;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Maroon;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Maroon;
                }


            }
        }
        protected void grdSubsidiaryTrialPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.Header)
            {
                if (ddlGroup.SelectedItem.Value == "0")
                    gvr.Cells[0].Text = "Branch Name";
                else
                    gvr.Cells[0].Text = "Group Name";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["SubType"] != null)
                {
                    if (ViewState["SubType"].ToString() == "Customers" || ViewState["SubType"].ToString().Trim() == "Employees" || ViewState["SubType"].ToString().Trim() == "Relationship Partners" || ViewState["SubType"].ToString().Trim() == "Business Partners" || ViewState["SubType"].ToString().Trim() == "Brokers" || ViewState["SubType"].ToString().Trim() == "Sub Brokers" || ViewState["SubType"].ToString().Trim() == "Franchisees" || ViewState["SubType"].ToString().Trim() == "Vendors" || ViewState["SubType"].ToString().Trim() == "Data Vendors" || ViewState["SubType"].ToString().Trim() == "Recruitment Agents" || ViewState["SubType"].ToString().Trim() == "Agents" || ViewState["SubType"].ToString().Trim() == "Consultants" || ViewState["SubType"].ToString().Trim() == "Share Holder" || ViewState["SubType"].ToString().Trim() == "Debtors" || ViewState["SubType"].ToString().Trim() == "Creditors")
                    {
                        string lblMainID = ((DataRowView)e.Row.DataItem)["MainID"].ToString();
                        string lblSubID = ((DataRowView)e.Row.DataItem)["SubID"].ToString();

                        string MainAccName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();
                        string SubAccName = ((DataRowView)e.Row.DataItem)["accountsledger_subaccountid"].ToString();
                        string UCC = ((DataRowView)e.Row.DataItem)["Ucc"].ToString();
                        string dt = Convert.ToDateTime(dtDate.Value.ToString()).ToShortDateString();

                        //Label lblMainID = (Label)e.Row.FindControl("lblMainID");
                        //Label lblSubID = (Label)e.Row.FindControl("lblSubID");
                        ((Label)e.Row.FindControl("lblTrDate")).Attributes.Add("onclick", "javascript:ShowLedger('" + lblMainID + "','" + lblSubID + "','" + ViewState["SegmentID"].ToString() + "','" + MainAccName + "','" + SubAccName + "','" + UCC + "','" + dt + "');");
                        e.Row.Cells[1].Style.Add("cursor", "hand");
                        e.Row.Cells[1].ToolTip = "Click to View Detail!";
                    }
                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string BrName = ((DataRowView)e.Row.DataItem)["BranchName"].ToString();
                if (BrName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Branch Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Blank")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        public void DrpdownClick()
        {

            int p = 0;
            FillDropDown();
            if (cmbclientsPager.Items.Count > 1)
            {
                for (int i = 0; i < cmbclientsPager.Items.Count; i++)
                {
                    string[] ClientValue = cmbclientsPager.Items[i].Value.ToString().Split('~');
                    string Type = ClientValue[1].ToString().Trim();
                    if (Type != "Customers" && Type != "Employees" && Type != "Relationship Partners" && Type != "Business Partners" && Type != "Brokers" && Type != "Sub Brokers" && Type != "Franchisees" && Type != "Vendors" && Type != "Data Vendors" && Type != "Recruitment Agents" && Type != "Agents" && Type != "Consultants" && Type != "Share Holder" && Type != "Debtors" && Type != "Creditors")
                    {
                        p = p + 1;
                    }

                }
                if (p > 0)
                {
                    string Type = "BranchVisibleFalse";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
                }
                else
                {
                    string Type = "Customers";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
                }

            }
            else
            {
                string[] ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                string Type = ClientValue[1].ToString().Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCR", "ShowDropDown()", true);
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            string ZeroBal = "";
            string BranchName = null;
            string ForGroup = null;
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;
            string TransactionDate = null;
            DataTable dtSubsidiary_New = new DataTable();
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 150000;
            string WehereMainAccount = null;
            string WhereMainBranch = null;
            DateTime date;
            decimal SumForBranchDR = 0;
            decimal SumForBranchCR = 0;
            decimal DifOfDRCR = 0;
            string[] ClientValue = null;
            decimal DebitCreditGreterEqualAmount = 0;
            if (rdbMainSelected.Checked == true)
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }
            else
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }

            string MainAccID = "";
            string SubAccId = "";
            string Brnch = "";
            string Segmnt = "";
            decimal DrCrAmt = 0;
            string Grp = "";
            string RptType = "";
            string BranchGroupType = "";
            string GroupType = "";
            String CheckDrCr = "";
            if (rdbClientSelected.Checked == true)
                SubAccId = HdnClients.Value;
            MainAccID = ClientValue[0].ToString();
            if (RadAsOnDate.Checked == true)
                RptType = "A";
            else
                RptType = "P";

            Brnch = Session["userbranchHierarchy"].ToString();

            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
            {
                if (ddlGroup.SelectedItem.Value == "0")
                {
                    BranchGroupType = "B";
                    if (rdbMainSelected.Checked == true)
                    {

                        if (rdbranchAll.Checked == true)
                            Brnch = Session["userbranchHierarchy"].ToString();
                        else
                            Brnch = BranchId;

                    }
                }
                else
                {
                    BranchGroupType = "G";
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                        {
                            if (rdddlgrouptypeAll.Checked == true)
                            {
                                if (ddlgrouptype.SelectedItem.Value == "0")
                                    GroupType = "N";
                                else
                                    GroupType = ddlgrouptype.SelectedItem.Text.ToString();
                            }
                            else
                                Grp = Group;
                        }

                    }
                }
            }

            if (rdDebit.Checked == true)
                CheckDrCr = "D";
            else if (rdCredit.Checked == true)
                CheckDrCr = "C";
            else
                CheckDrCr = "B";


            if (rdAllSegment.Checked == true)
            {
                DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
                Segmnt = DT.Rows[0][0].ToString();
                for (int i = 1; i < DT.Rows.Count; i++)
                {
                    Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                }
            }
            else
            {
                // Segmnt = ViewState["SegmentID"].ToString();
                if (HdnSegment.Value != "")
                {
                    Segmnt = HdnSegment.Value;
                    ViewState["SegmentID"] = HdnSegment.Value;
                }
                else
                {
                    Segmnt = ViewState["SegmentID"].ToString();
                }
            }

            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";
            }

            if (txtDebitCredit.Text != "")
                DrCrAmt = Convert.ToDecimal(txtDebitCredit.Text);



            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_SubSideryTrial", con))
                {
                    SqlParameter oparaType = new SqlParameter("@LType", SqlDbType.VarChar, 100);
                    oparaType.Direction = ParameterDirection.Output;
                    da.SelectCommand.Parameters.AddWithValue("@MainAccountID", MainAccID);
                    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                    da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtDate.Value);
                    da.SelectCommand.Parameters.AddWithValue("@Segment", Segmnt);
                    da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@Company", Session["LastCompany"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@DrCrAmt", DrCrAmt);
                    da.SelectCommand.Parameters.AddWithValue("@Group", Grp);
                    da.SelectCommand.Parameters.AddWithValue("@ReportType", RptType);
                    da.SelectCommand.Parameters.AddWithValue("@BranchGroutType", BranchGroupType);
                    da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                    da.SelectCommand.Parameters.AddWithValue("@ShowStatus", CheckDrCr);
                    da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                    ///Currency Setting
                    da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                    //Segment Wise Break Up
                    da.SelectCommand.Parameters.AddWithValue("@IsSegmentWiseBreakUp", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsTDayBal", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsConsolidate", "N");
                    // Consolidate Same SubLedger Type Accounts
                    da.SelectCommand.Parameters.AddWithValue("@IsAcConsolidate", "N");

                    da.SelectCommand.Parameters.Add(oparaType);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    //ViewState["dataset"] = ds;
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                }
            }

            DataTable dtComp = oDbEngine.GetDataTable("tbl_master_company", " cmp_name,(Select top 1 phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId=cmp_internalid) as cmpphno,(select top 1(isnull(add_address1,'')+' '+ isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(city_name,'')+'-'+  isnull(add_pin,'')) from tbl_master_address,tbl_master_city where add_city=city_id and add_cntID=cmp_internalid AND add_entity='Company' AND add_addressType='Office')as cmpaddress,(select top 1 eml_email from tbl_master_email   where eml_cntid=cmp_internalid) as Email  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");

            ReportDocument report = new ReportDocument();
            // ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\SubSidiaryTrial.xsd");
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            string tmpPdfPath = string.Empty;
            if (RadAsOnDate.Checked == true)
            {
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\SubSidiaryTrial.rpt");
            }
            else
            {
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\SubSidiaryTrialPeriod.rpt");
            }
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();

            if (RadAsOnDate.Checked == true)
            {
                report.SetParameterValue("@ReprtDt", (string)"Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]");

            }
            else
            {
                report.SetParameterValue("@ReprtDt", (string)"Subsidiary Trial Period From Date [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]");


            }
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                report.SetParameterValue("@BGType", (string)"G");
            }
            else
            {
                report.SetParameterValue("@BGType", (string)"B");
            }

            if (dtComp.Rows.Count > 0)
            {
                if (dtComp.Rows[0]["cmp_name"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyName", (string)dtComp.Rows[0]["cmp_name"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyName", (string)"COMPANY NAME");
                }

            }
            if (rdAllSegment.Checked == true)
            {
                report.SetParameterValue("@Segment", (string)"ALL");
            }
            else
            {
                report.SetParameterValue("@Segment", (string)HDNSeg.Value.ToString());
            }

            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "SubsidiaryTrial");
            report.Dispose();
            GC.Collect();

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            int b = 0;
            SendEmailTag = "Y";
            DataTable dtExport = new DataTable();
            DataTable dtComp = oDbEngine.GetDataTable("tbl_master_company", " cmp_name  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");

            for (int i = 0; i < cmbclientsPager.Items.Count; i++)
            {
                cmbclientsPager.SelectedValue = cmbclientsPager.Items[i].Value;
                if (RadAsOnDate.Checked == true)
                {
                    FillGrid();
                    dtSubsidiary = (DataTable)ViewState["ExportTab"];
                    dtSubsidiary.Columns[1].ColumnName = "Main Account";
                    dtSubsidiary.Columns[2].ColumnName = "Sub Account";
                    dtSubsidiary.Columns[3].ColumnName = "UCC";
                    dtSubsidiary.Columns[9].ColumnName = "Debit";
                    dtSubsidiary.Columns[10].ColumnName = "Credit";
                    dtSubsidiary.Columns.Remove("MainID");
                    dtSubsidiary.Columns.Remove("SubID");
                    dtSubsidiary.Columns.Remove("AmountDR");
                    dtSubsidiary.Columns.Remove("AmountCR");
                    if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                    {
                        dtSubsidiary.Columns.Remove("BranchName");
                        dtSubsidiary.Columns.Remove("PhoneNumber");

                    }
                    else
                    {
                        if (chkPhNumber.Checked == false)
                        {

                            dtSubsidiary.Columns.Remove("PhoneNumber");

                        }

                    }

                }
                else
                {
                    FillGridForPeriod();
                    dtSubsidiary = (DataTable)ViewState["ExportTab"];
                    dtSubsidiary.Columns[3].ColumnName = "Main Account";
                    dtSubsidiary.Columns[4].ColumnName = "Sub Account";
                    dtSubsidiary.Columns[5].ColumnName = "UCC";
                    dtSubsidiary.Columns[12].ColumnName = "Opening Debit";
                    dtSubsidiary.Columns[13].ColumnName = "Opening Credit";
                    dtSubsidiary.Columns[14].ColumnName = "Amount Debit";
                    dtSubsidiary.Columns[15].ColumnName = "Amount Credit";
                    dtSubsidiary.Columns[16].ColumnName = "Closing Debit";
                    dtSubsidiary.Columns[17].ColumnName = "Closing Credit";
                    dtSubsidiary.Columns.Remove("OpeningDR");
                    dtSubsidiary.Columns.Remove("OpeningCR");
                    dtSubsidiary.Columns.Remove("AmountDR");
                    dtSubsidiary.Columns.Remove("AmountCR");
                    dtSubsidiary.Columns.Remove("ClosingDR");
                    dtSubsidiary.Columns.Remove("ClosingCR");


                    DataRow NewRow = dtSubsidiary.NewRow();

                    if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                    {
                        NewRow[0] = "Total";
                    }
                    else
                    {
                        NewRow[0] = "Total";
                    }

                    if (ViewState["OpDr"] != null)
                        NewRow[6] = ViewState["OpDr"].ToString();
                    else
                        NewRow[6] = "0.00";
                    if (ViewState["OpCr"] != null)
                        NewRow[7] = ViewState["OpCr"].ToString();
                    else
                        NewRow[7] = "0.00"; ;
                    if (ViewState["AmDr"] != null)
                        NewRow[8] = ViewState["AmDr"].ToString();
                    else
                        NewRow[8] = "0.00";
                    if (ViewState["AmCr"] != null)
                        NewRow[9] = ViewState["AmCr"].ToString();
                    else
                        NewRow[9] = "0.000";
                    if (ViewState["ClDr"] != null)
                        NewRow[10] = ViewState["ClDr"].ToString();
                    else
                        NewRow[10] = "0.00";
                    if (ViewState["ClCr"] != null)
                        NewRow[11] = ViewState["ClCr"].ToString();
                    else
                        NewRow[11] = "0.00";
                    dtSubsidiary.Rows.Add(NewRow);

                    dtSubsidiary.Columns.Remove("MainID");
                    dtSubsidiary.Columns.Remove("SubID");
                    if (ViewState["SubType"].ToString().Trim() != "Customers" && ViewState["SubType"].ToString().Trim() != "NSDL Clients" && ViewState["SubType"].ToString().Trim() != "CDSL Clients" && ViewState["SubType"].ToString().Trim() != "Employees" && ViewState["SubType"].ToString().Trim() != "Relationship Partners" && ViewState["SubType"].ToString().Trim() != "Business Partners" && ViewState["SubType"].ToString().Trim() != "Brokers" && ViewState["SubType"].ToString().Trim() != "Sub Brokers" && ViewState["SubType"].ToString().Trim() != "Franchisees" && ViewState["SubType"].ToString().Trim() != "Vendors" && ViewState["SubType"].ToString().Trim() != "Data Vendors" && ViewState["SubType"].ToString().Trim() != "Recruitment Agents" && ViewState["SubType"].ToString().Trim() != "Agents" && ViewState["SubType"].ToString().Trim() != "Consultants" && ViewState["SubType"].ToString().Trim() != "Share Holder" && ViewState["SubType"].ToString().Trim() != "Debtors" && ViewState["SubType"].ToString().Trim() != "Creditors")
                    {
                        dtSubsidiary.Columns.Remove("BranchName");

                        //  dtSubsidiary.Columns.Remove("PhoneNumber");
                    }

                }






                if (ViewState["SubType"].ToString().Trim() == "Customers" || ViewState["SubType"].ToString().Trim() == "NSDL Clients" || ViewState["SubType"].ToString().Trim() == "CDSL Clients" || ViewState["SubType"].ToString().Trim() == "Employees" || ViewState["SubType"].ToString().Trim() == "Relationship Partners" || ViewState["SubType"].ToString().Trim() == "Business Partners" || ViewState["SubType"].ToString().Trim() == "Brokers" || ViewState["SubType"].ToString().Trim() == "Sub Brokers" || ViewState["SubType"].ToString().Trim() == "Franchisees" || ViewState["SubType"].ToString().Trim() == "Vendors" || ViewState["SubType"].ToString().Trim() == "Data Vendors" || ViewState["SubType"].ToString().Trim() == "Recruitment Agents" || ViewState["SubType"].ToString().Trim() == "Agents" || ViewState["SubType"].ToString().Trim() == "Consultants" || ViewState["SubType"].ToString().Trim() == "Share Holder" || ViewState["SubType"].ToString().Trim() == "Debtors" || ViewState["SubType"].ToString().Trim() == "Creditors")
                {
                    dtSubsidiary.Columns.Remove("Main Account");
                    if (i == 0)
                    {
                        dtExport = dtSubsidiary.Clone();
                    }
                    for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                    {
                        if (k == 0)
                        {
                            DataRow row1 = dtExport.NewRow();
                            if (ddlGroup.SelectedItem.Value == "0")
                            {
                                row1[1] = "Branch Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                            }
                            else
                            {
                                row1[1] = "Group Name:  " + dtSubsidiary.Rows[k]["BranchName"].ToString();
                            }
                            row1[2] = "Test";
                            dtExport.Rows.Add(row1);
                        }
                        if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Blank")
                        {
                            DataRow row9 = dtExport.NewRow();
                            if (ddlGroup.SelectedItem.Value == "0")
                            {
                                row9[1] = "Branch Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                            }
                            else
                            {
                                row9[1] = "Group Name:  " + dtSubsidiary.Rows[k + 1]["BranchName"].ToString();
                            }
                            row9[2] = "Test";
                            dtExport.Rows.Add(row9);

                        }
                        else
                        {
                            if (dtSubsidiary.Rows[k]["BranchName"].ToString() == "Total" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Group Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Net" || dtSubsidiary.Rows[k]["BranchName"].ToString() == "Branch/Group Net")
                            {
                                dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["BranchName"].ToString();
                                dtSubsidiary.AcceptChanges();
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }
                            else
                            {
                                dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                            }
                        }

                    }

                    dtExport.Columns.Remove("BranchName");
                }
                else
                {



                    for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                    {


                        if (dtSubsidiary.Rows[k]["Main Account"].ToString() == "Total" || dtSubsidiary.Rows[k]["Main Account"].ToString() == "Net")
                        {
                            dtSubsidiary.Rows[k]["Sub Account"] = dtSubsidiary.Rows[k]["Main Account"].ToString();
                            dtSubsidiary.AcceptChanges();
                            dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                        }
                        else
                        {
                            dtExport.Rows.Add(dtSubsidiary.Rows[k].ItemArray);
                        }

                    }
                    dtExport.Columns.Remove("Main Account");
                }




                if (ResUser.Checked == true)
                {
                    #region UserWise
                    string HTMLTable = "";
                    if (RadAsOnDate.Checked == true)
                    {
                        HTMLTable = "<table width=\"100%\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Sub Account</td><td>UCC</td><td>Closing Debit</td><td>Closing Credit</td></tr>";

                        for (int j = 0; j < dtExport.Rows.Count; j++)
                        {
                            if (dtExport.Rows[j]["UCC"].ToString() == "Test")
                            {
                                HTMLTable = HTMLTable + "<tr style=\"background-color: #FFD4AA; color: Black;\"><td colspan=\"4\">" + dtExport.Rows[j][0].ToString() + "</td></tr>";
                            }
                            else
                            {
                                HTMLTable = HTMLTable + "<tr><td>&nbsp;" + dtExport.Rows[j][0].ToString() + "</td><td align=\"center\">&nbsp;" + dtExport.Rows[j][1].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][2].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][3].ToString() + "</td></tr>";
                            }
                        }
                        HTMLTable = HTMLTable + "</table>";

                        string Type = "";
                        Type = cmbclientsPager.SelectedItem.Text;
                        string billdate = Type + " </br> Date :" + "  " + oconverter.ArrangeDate2(dtDate.Value.ToString());

                        if (rdAllSegment.Checked == true)
                            billdate = billdate + "(ALL Segment)";
                        else
                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";

                        billdate = billdate + "</br>Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                        string emailbdy = HTMLTable;
                        string Subject = "Subsidery Trial As On Date For " + billdate;
                        string contactid = HdnEmployee.Value;
                        if (contactid.ToString() != "")
                        {
                            string[] Cnt = contactid.Split(',');
                            for (int k = 0; k < Cnt.Length; k++)
                            {
                                if (oDbEngine.SendReport(emailbdy, Cnt[k].ToString(), billdate, Subject) == true)
                                {
                                    b = b + 1;

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);
                                }
                                else
                                {
                                    b = b + 1;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSuccs", "alert('Error On Sending!Please try again...')", true);
                                }
                            }
                        }

                    }
                    else
                    {
                        HTMLTable = "<table width=\"100%\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Sub Account</td><td>UCC</td><td>Opening Debit</td><td>Opening Credit</td><td>Amount Debit</td><td>Amount Credit</td><td>Closing Debit</td><td>Closing Credit</td></tr>";
                        for (int j = 0; j < dtExport.Rows.Count; j++)
                        {
                            if (dtExport.Rows[j]["UCC"].ToString() == "Test")
                            {
                                HTMLTable = HTMLTable + "<tr style=\"background-color: #FFD4AA; color: Black;\"><td colspan=\"8\">" + dtExport.Rows[j][0].ToString() + "</td></tr>";
                            }
                            else
                            {
                                HTMLTable = HTMLTable + "<tr><td>" + dtExport.Rows[j][0].ToString() + "</td><td align=\"center\">&nbsp;" + dtExport.Rows[j][1].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][2].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][3].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][4].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][5].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][6].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][7].ToString() + "</td></tr>";
                            }
                        }
                        HTMLTable = HTMLTable + "</table>";

                        string Type = "";
                        Type = cmbclientsPager.SelectedItem.Text;
                        string billdate = Type + " Period :" + "From  " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        if (rdAllSegment.Checked == true)
                            billdate = billdate + "(ALL Segment)";
                        else
                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";
                        billdate = billdate + "</br>Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                        string emailbdy = HTMLTable;
                        string Subject = "Subsidery Trial As On Date For " + billdate;
                        string contactid = HdnEmployee.Value;
                        if (contactid.ToString() != "")
                        {
                            string[] Cnt = contactid.Split(',');
                            for (int k = 0; k < Cnt.Length; k++)
                            {
                                if (oDbEngine.SendReport(emailbdy, Cnt[k].ToString(), billdate, Subject) == true)
                                {
                                    HTMLTable = "";
                                    b = b + 1;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);

                                }
                                else
                                {
                                    HTMLTable = "";
                                    b = b + 1;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Fail To Send Email..!Please Try Again.')", true);
                                }
                            }
                        }

                    }
                    #endregion
                }
                else if (ResBranch.Checked == true)
                {
                    #region BranchWise
                    string BranchName = "";
                    string BranchC = "";
                    string BranchEmail = "";
                    string BranchContact = "";
                    string GroupName = "";
                    string GroupC = "";
                    string GroupEmail = "";
                    string GroupContact = "";


                    string HTMLTable = "";

                    for (int j = 0; j < dtExport.Rows.Count; j++)
                    {

                        if (dtExport.Rows[j]["UCC"].ToString() == "Test" || (j == dtExport.Rows.Count - 1))
                        {

                            if (j == dtExport.Rows.Count - 1)
                            {
                                #region ForLastRecord
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    if (BranchEmail == "")
                                    {
                                        HTMLTable = "";

                                    }
                                    else
                                    {
                                        HTMLTable = HTMLTable + "</table>";
                                        string Type = "";
                                        Type = cmbclientsPager.SelectedItem.Text;
                                        string billdate = Type + "  Date :" + "  " + oconverter.ArrangeDate2(dtDate.Value.ToString());
                                        if (rdAllSegment.Checked == true)
                                            billdate = billdate + "(ALL Segment)";
                                        else
                                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";
                                        billdate = billdate + "Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                                        string emailbdy = HTMLTable;
                                        string Subject = "Subsidery Trial As On Date For " + billdate;
                                        if (oDbEngine.SendReportBr(emailbdy, BranchEmail, billdate, Subject, BranchContact) == true)
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            BranchEmail = "";
                                            BranchContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);
                                        }
                                        else
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            BranchEmail = "";
                                            BranchContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Fail To Send Email..!Please Try Again.')", true);

                                        }
                                        HTMLTable = "";
                                        BranchEmail = "";
                                        BranchContact = "";
                                        emailbdy = "";


                                    }

                                }
                                else
                                {

                                    if (GroupEmail == "")
                                    {
                                        HTMLTable = "";

                                    }
                                    else
                                    {
                                        HTMLTable = HTMLTable + "</table>";
                                        string Type = "";
                                        Type = cmbclientsPager.SelectedItem.Text;
                                        string billdate = Type + "Date :" + "  " + oconverter.ArrangeDate2(dtDate.Value.ToString());
                                        if (rdAllSegment.Checked == true)
                                            billdate = billdate + "(ALL Segment)";
                                        else
                                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";
                                        billdate = billdate + "Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                                        string emailbdy = HTMLTable;
                                        string Subject = "Subsidery Trial As On Date For " + billdate;
                                        if (oDbEngine.SendReportBr(emailbdy, GroupEmail, billdate, Subject, GroupContact) == true)
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            GroupEmail = "";
                                            GroupContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);
                                        }
                                        else
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            GroupEmail = "";
                                            GroupContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Fail To Send Email..!Please Try Again.')", true);
                                        }
                                        HTMLTable = "";
                                        GroupEmail = "";
                                        GroupContact = "";
                                        emailbdy = "";
                                    }
                                }
                                #endregion

                            }
                            else if (j > 0)
                            {
                                #region FromStart
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    if (BranchEmail == "")
                                    {
                                        HTMLTable = "";
                                    }
                                    else
                                    {
                                        HTMLTable = HTMLTable + "</table>";
                                        string Type = "";
                                        Type = cmbclientsPager.SelectedItem.Text;
                                        string billdate = Type + " Date :" + "  " + oconverter.ArrangeDate2(dtDate.Value.ToString());
                                        if (rdAllSegment.Checked == true)
                                            billdate = billdate + "(ALL Segment)";
                                        else
                                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";
                                        billdate = billdate + "Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                                        string emailbdy = HTMLTable;
                                        string Subject = "Subsidery Trial As On Date For " + billdate;
                                        if (oDbEngine.SendReportBr(emailbdy, BranchEmail, billdate, Subject, BranchContact) == true)
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            BranchEmail = "";
                                            BranchContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);
                                        }
                                        else
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            BranchEmail = "";
                                            BranchContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Fail To Send Email..!Please Try Again.')", true);

                                        }
                                        HTMLTable = "";
                                        BranchEmail = "";
                                        BranchContact = "";
                                        emailbdy = "";



                                    }

                                }
                                else
                                {

                                    if (GroupEmail == "")
                                    {
                                        HTMLTable = "";
                                    }
                                    else
                                    {
                                        HTMLTable = HTMLTable + "</table>";
                                        string Type = "";
                                        Type = cmbclientsPager.SelectedItem.Text;
                                        string billdate = Type + "  Date :" + "  " + oconverter.ArrangeDate2(dtDate.Value.ToString());
                                        if (rdAllSegment.Checked == true)
                                            billdate = billdate + "(ALL Segment)";
                                        else
                                            billdate = billdate + "(" + HDNSeg.Value.ToString() + ")";
                                        billdate = billdate + "Company: " + dtComp.Rows[0]["cmp_name"].ToString();
                                        string emailbdy = HTMLTable;
                                        string Subject = "Subsidery Trial As On Date For " + billdate;
                                        if (oDbEngine.SendReportBr(emailbdy, GroupEmail, billdate, Subject, GroupContact) == true)
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            GroupEmail = "";
                                            GroupContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSSucc", "alert('Mail Send Successfully')", true);
                                        }
                                        else
                                        {
                                            b = b + 1;
                                            HTMLTable = "";
                                            GroupEmail = "";
                                            GroupContact = "";
                                            emailbdy = "";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Fail To Send Email..!Please Try Again.')", true);
                                        }
                                        HTMLTable = "";
                                        GroupEmail = "";
                                        GroupContact = "";
                                        emailbdy = "";
                                    }
                                }
                                #endregion

                            }
                            string[] BRGR = dtExport.Rows[j]["Sub Account"].ToString().Split(':');
                            if (BRGR[0].ToString() == "Branch Name")
                            {
                                string[] BrC = BRGR[1].ToString().Split('[');
                                BranchName = BrC[0].ToString();
                                string[] BrCode = BrC[1].ToString().Split(']');
                                BranchC = BrCode[0].ToString();
                                DataTable dtBr = oDbEngine.GetDataTable("tbl_master_branch", " ltrim(rtrim(isnull(branch_head,'')))  as  branch_head,ltrim(rtrim(isnull(branch_cpEmail,''))) as branch_cpEmail  ", "ltrim(rtrim(branch_code)) ='" + BranchC.ToString().Trim() + "'");
                                if (dtBr.Rows.Count > 0)
                                {
                                    if (dtBr.Rows[0]["branch_cpEmail"].ToString() != "")
                                    {
                                        BranchEmail = dtBr.Rows[0]["branch_cpEmail"].ToString();
                                        BranchContact = dtBr.Rows[0]["branch_head"].ToString();
                                    }
                                }

                            }
                            else if (BRGR[0].ToString() == "Group Name")
                            {
                                if (BRGR[1].ToString().Trim() != "")
                                {
                                    string[] GrC = BRGR[1].ToString().Split('[');
                                    GroupName = GrC[0].ToString();
                                    string[] GrCode = GrC[1].ToString().Split(']');
                                    GroupC = GrCode[0].ToString();
                                    DataTable dtGr = oDbEngine.GetDataTable("tbl_master_groupmaster", "ltrim(rtrim(isnull(gpm_emailid,''))) as  gpm_emailid,ltrim(rtrim(isnull(gpm_ccemailid,''))) as gpm_ccemailid,ltrim(rtrim(isnull(gpm_Owner,''))) as gpm_Owner ", " ltrim(rtrim(gpm_code))='" + GroupC.ToString().Trim() + "' and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'");
                                    if (dtGr.Rows.Count > 0)
                                    {
                                        GroupEmail = dtGr.Rows[0]["gpm_emailid"].ToString();
                                        GroupContact = dtGr.Rows[0]["gpm_Owner"].ToString();
                                    }
                                }

                            }


                            if (RadAsOnDate.Checked == true)
                            {

                                HTMLTable = "<table width=\"100%\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Sub Account</td><td>UCC</td><td>Closing Debit</td><td>Closing Credit</td></tr>";
                                HTMLTable = HTMLTable + "<tr style=\"background-color: #FFD4AA; color: Black;\"><td colspan=\"4\">" + dtExport.Rows[j][0].ToString() + "</td></tr>";
                            }
                            else
                            {

                                HTMLTable = "<table width=\"100%\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Sub Account</td><td>UCC</td><td>Opening Debit</td><td>Opening Credit</td><td>Amount Debit</td><td>Amount Credit</td><td>Closing Debit</td><td>Closing Credit</td></tr>";
                                HTMLTable = HTMLTable + "<tr style=\"background-color: #FFD4AA; color: Black;\"><td colspan=\"8\">" + dtExport.Rows[j][0].ToString() + "</td></tr>";

                            }
                        }
                        else
                        {
                            if (RadAsOnDate.Checked == true)
                            {
                                HTMLTable = HTMLTable + "<tr><td>&nbsp;" + dtExport.Rows[j][0].ToString() + "</td><td align=\"center\">&nbsp;" + dtExport.Rows[j][1].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][2].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][3].ToString() + "</td></tr>";
                            }
                            else
                            {
                                HTMLTable = HTMLTable + "<tr><td>" + dtExport.Rows[j][0].ToString() + "</td><td align=\"center\">&nbsp;" + dtExport.Rows[j][1].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][2].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][3].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][4].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][5].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][6].ToString() + "</td><td align=\"right\">&nbsp;" + dtExport.Rows[j][7].ToString() + "</td></tr>";
                            }
                        }
                    }

                    #endregion
                    HTMLTable = HTMLTable + "</table>";

                }

            }


            if (b == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSerr", "alert('Mail Id Not Found..!')", true);
            }




        }
        protected void btndos_Click(object sender, EventArgs e)
        {
            string ZeroBal = "";
            string BranchName = null;
            string ForGroup = null;
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;
            string TransactionDate = null;
            DataTable dtSubsidiary_New = new DataTable();
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 150000;
            string WehereMainAccount = null;
            string WhereMainBranch = null;
            DateTime date;
            decimal SumForBranchDR = 0;
            decimal SumForBranchCR = 0;
            decimal DifOfDRCR = 0;
            string[] ClientValue = null;
            decimal DebitCreditGreterEqualAmount = 0;
            if (rdbMainSelected.Checked == true)
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }
            else
            {
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                ViewState["SubType"] = ClientValue[1].ToString().Trim();
            }

            string MainAccID = "";
            string SubAccId = "";
            string Brnch = "";
            string Segmnt = "";
            decimal DrCrAmt = 0;
            string Grp = "";
            string RptType = "";
            string BranchGroupType = "";
            string GroupType = "";
            String CheckDrCr = "";
            if (rdbClientSelected.Checked == true)
                SubAccId = HdnClients.Value;
            MainAccID = ClientValue[0].ToString();
            if (RadAsOnDate.Checked == true)
                RptType = "A";
            else
                RptType = "P";

            Brnch = Session["userbranchHierarchy"].ToString();

            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
            {
                if (ddlGroup.SelectedItem.Value == "0")
                {
                    BranchGroupType = "B";
                    if (rdbMainSelected.Checked == true)
                    {

                        if (rdbranchAll.Checked == true)
                            Brnch = Session["userbranchHierarchy"].ToString();
                        else
                            Brnch = BranchId;

                    }
                }
                else
                {
                    BranchGroupType = "G";
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients" || ClientValue[1].ToString().Trim() == "Employees" || ClientValue[1].ToString().Trim() == "Relationship Partners" || ClientValue[1].ToString().Trim() == "Business Partners" || ClientValue[1].ToString().Trim() == "Brokers" || ClientValue[1].ToString().Trim() == "Sub Brokers" || ClientValue[1].ToString().Trim() == "Franchisees" || ClientValue[1].ToString().Trim() == "Vendors" || ClientValue[1].ToString().Trim() == "Data Vendors" || ClientValue[1].ToString().Trim() == "Recruitment Agents" || ClientValue[1].ToString().Trim() == "Agents" || ClientValue[1].ToString().Trim() == "Consultants" || ClientValue[1].ToString().Trim() == "Share Holder" || ClientValue[1].ToString().Trim() == "Debtors" || ClientValue[1].ToString().Trim() == "Creditors")
                        {
                            if (rdddlgrouptypeAll.Checked == true)
                            {
                                if (ddlgrouptype.SelectedItem.Value == "0")
                                    GroupType = "N";
                                else
                                    GroupType = ddlgrouptype.SelectedItem.Text.ToString();
                            }
                            else
                                Grp = Group;
                        }

                    }
                }
            }

            if (rdDebit.Checked == true)
                CheckDrCr = "D";
            else if (rdCredit.Checked == true)
                CheckDrCr = "C";
            else
                CheckDrCr = "B";


            if (rdAllSegment.Checked == true)
            {
                DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
                Segmnt = DT.Rows[0][0].ToString();
                for (int i = 1; i < DT.Rows.Count; i++)
                {
                    Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                }
            }
            else
            {
                // Segmnt = ViewState["SegmentID"].ToString();
                if (HdnSegment.Value != "")
                {
                    Segmnt = HdnSegment.Value;
                    ViewState["SegmentID"] = HdnSegment.Value;
                }
                else
                {
                    Segmnt = ViewState["SegmentID"].ToString();
                }
            }

            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }





            if (txtDebitCredit.Text != "")
                DrCrAmt = Convert.ToDecimal(txtDebitCredit.Text);



            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_SubSideryTrial", con))
                {
                    SqlParameter oparaType = new SqlParameter("@LType", SqlDbType.VarChar, 100);
                    oparaType.Direction = ParameterDirection.Output;
                    da.SelectCommand.Parameters.AddWithValue("@MainAccountID", MainAccID);
                    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                    da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtDate.Value);
                    da.SelectCommand.Parameters.AddWithValue("@Segment", Segmnt);
                    da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@Company", Session["LastCompany"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@DrCrAmt", DrCrAmt);
                    da.SelectCommand.Parameters.AddWithValue("@Group", Grp);
                    da.SelectCommand.Parameters.AddWithValue("@ReportType", RptType);
                    da.SelectCommand.Parameters.AddWithValue("@BranchGroutType", BranchGroupType);
                    da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                    da.SelectCommand.Parameters.AddWithValue("@ShowStatus", CheckDrCr);
                    da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                    ///Currency Setting
                    da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                    //Segment Wise Break Up
                    da.SelectCommand.Parameters.AddWithValue("@IsSegmentWiseBreakUp", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsTDayBal", "N");
                    da.SelectCommand.Parameters.AddWithValue("@IsConsolidate", "N");
                    // Consolidate Same SubLedger Type Accounts
                    da.SelectCommand.Parameters.AddWithValue("@IsAcConsolidate", "N");
                    da.SelectCommand.Parameters.Add(oparaType);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    //ViewState["dataset"] = ds;

                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {

                DataTable dtComp = oDbEngine.GetDataTable("tbl_master_company", " cmp_name,(Select top 1 phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId=cmp_internalid) as cmpphno,(select top 1(isnull(add_address1,'')+' '+ isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(city_name,'')+'-'+  isnull(add_pin,'')) from tbl_master_address,tbl_master_city where add_city=city_id and add_cntID=cmp_internalid AND add_entity='Company' AND add_addressType='Office')as cmpaddress,(select top 1 eml_email from tbl_master_email   where eml_cntid=cmp_internalid) as Email  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");

                ReportDocument report = new ReportDocument();
                // ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\SubSidiaryTrial.xsd");
                report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                string tmpPdfPath = string.Empty;
                if (RadAsOnDate.Checked == true)
                {
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\SubSidiaryTrialdos.rpt");
                }
                else
                {
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\SubSidiaryTrialPerioddos.rpt");
                }
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.VerifyDatabase();

                if (RadAsOnDate.Checked == true)
                {
                    report.SetParameterValue("@ReprtDt", (string)"Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]");

                }
                else
                {
                    report.SetParameterValue("@ReprtDt", (string)"Subsidiary Trial For The Period From  [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]");


                }
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    report.SetParameterValue("@BGType", (string)"G");
                }
                else
                {
                    report.SetParameterValue("@BGType", (string)"B");
                }

                if (dtComp.Rows.Count > 0)
                {
                    if (dtComp.Rows[0]["cmp_name"].ToString() != "")
                    {
                        report.SetParameterValue("@CompanyName", (string)dtComp.Rows[0]["cmp_name"].ToString());
                    }
                    else
                    {
                        report.SetParameterValue("@CompanyName", (string)"COMPANY NAME");
                    }

                }
                if (dtComp.Rows.Count > 0)
                {
                    if (dtComp.Rows[0]["cmpphno"].ToString() != "")
                    {
                        report.SetParameterValue("@Companyphone", (string)dtComp.Rows[0]["cmpphno"].ToString());
                    }
                    else
                    {
                        report.SetParameterValue("@Companyphone", (string)"COMPANY PHONE");
                    }

                }
                if (dtComp.Rows.Count > 0)
                {
                    if (dtComp.Rows[0]["cmpaddress"].ToString() != "")
                    {
                        report.SetParameterValue("@Companyaddress", (string)dtComp.Rows[0]["cmpaddress"].ToString());
                    }
                    else
                    {
                        report.SetParameterValue("@Companyaddress", (string)"COMPANY ADDRESS");
                    }

                }
                if (dtComp.Rows.Count > 0)
                {
                    if (dtComp.Rows[0]["Email"].ToString() != "")
                    {
                        report.SetParameterValue("@Companyemail", (string)dtComp.Rows[0]["Email"].ToString());
                    }
                    else
                    {
                        report.SetParameterValue("@Companyemail", (string)"COMPANY EMAIL");
                    }

                }
                if (rdAllSegment.Checked == true)
                {
                    report.SetParameterValue("@Segment", (string)"ALL");
                }
                else
                {
                    report.SetParameterValue("@Segment", (string)HDNSeg.Value.ToString());
                }

                string tmpPdfPath1;
                tmpPdfPath1 = string.Empty;
                tmpPdfPath1 = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                string abcd = tmpPdfPath1 + "Subsidiatry.pdf";
                report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, abcd);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4567", "<script language='javascript'>window.open('subsidiary.aspx');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('No Record Found');</script>");
            }

        }

        //================= Start Modify on Show Segment Wise Break Up BL==================================

        protected void CmbMailSendTo_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CmbMailSendTo.Items.Clear();
            CmbMailSendTo.Items.Insert(0, new ListEditItem("Select Mail Recipient", "0"));
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "MailToBranch")
            {
                CmbMailSendTo.Items.Add("Mail To Branch", "1");
                CmbMailSendTo.Items.Add("Mail To Employee", "3");
            }
            if (WhichCall == "MailToGroup")
            {
                CmbMailSendTo.Items.Add("Mail To Group", "2");
                CmbMailSendTo.Items.Add("Mail To Employee", "3");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string MainAccID = "";
            string SubAccId = "";
            string Brnch = "";
            string Segmnt = "";
            decimal DrCrAmt = 0;
            string Grp = "";
            string RptType = "";
            string BranchGroupType = "";
            string GroupType = "";
            string CheckDrCr = "";
            string ZeroBal = "";
            string FromDate = Session["FinYearStart"].ToString();
            string ToDate = Session["FinYearEnd"].ToString();

            MainAccID = HdnMainAcc.Value;
            MainAccID = MainAccID.Substring(1, MainAccID.Length - 2);
            if (MainAccID.Contains(","))
            {
                MainAccID = MainAccID.Replace(",", "','");
            }
            if (RadAsOnDate.Checked == true)
            {
                RptType = "A";
                FromDate = Session["FinYearStart"].ToString();
                ToDate = dtDate.Date.ToString();
            }
            else
            {
                RptType = "P";
                FromDate = dtFrom.Date.ToString();
                ToDate = dtTo.Date.ToString();
            }
            Brnch = Session["userbranchHierarchy"].ToString();

            if (rdbClientSelected.Checked == true)
                SubAccId = HdnClients.Value;

            if (SubAccId.Contains("'"))
                SubAccId.Replace("'", "");

            if (ddlGroup.SelectedItem.Value == "0")
            {
                BranchGroupType = "B";
                if (rdbMainSelected.Checked == true)
                {
                    if (rdbranchAll.Checked == true)
                        Brnch = Session["userbranchHierarchy"].ToString();
                    else
                    {
                        Brnch = HdnBranchId.Value;
                        if (Brnch.Contains("'"))
                            Brnch = Brnch.Replace("'", "");
                    }
                }
                GroupType = "ALL";
            }
            else
            {
                BranchGroupType = "G";
                if (rdbMainSelected.Checked == true)
                {
                    if (ddlgrouptype.SelectedItem.Value == "0")
                        GroupType = "";
                    else
                        GroupType = ddlgrouptype.SelectedItem.Text.ToString();

                    if (rdddlgrouptypeAll.Checked == true)
                        Grp = "ALL";
                    else
                    {
                        Grp = HdnGroup.Value;
                        Grp = Grp.Substring(1, Grp.Length - 2);
                        Grp = Grp.Replace("','", ",");
                    }
                }
            }

            if (rdDebit.Checked == true)
                CheckDrCr = "D";
            else if (rdCredit.Checked == true)
                CheckDrCr = "C";
            else
                CheckDrCr = "B";

            Segmnt = ViewState["SegmentID"].ToString();

            if (txtDebitCredit.Text != "")
                DrCrAmt = Convert.ToDecimal(txtDebitCredit.Text);

            if (rdAllSegment.Checked == true)
            {
                DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "*", null);
                Segmnt = DT.Rows[0][0].ToString();
                for (int i = 1; i < DT.Rows.Count; i++)
                {
                    Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                    ViewState["SegmentID"] = Segmnt;
                }
            }
            else
            {
                // Segmnt = ViewState["SegmentID"].ToString();

                if (Session["userlastsegment"].ToString() == "5")
                {
                    DataTable DtSeg = new DataTable();
                    DtSeg = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                    Segmnt = DtSeg.Rows[0][1].ToString();

                }
                else
                {

                    if (HdnSegment.Value != "")
                    {
                        Segmnt = HdnSegment.Value;
                        ViewState["SegmentID"] = HdnSegment.Value;
                    }
                    else
                    {
                        Segmnt = ViewState["SegmentID"].ToString();
                    }
                }
            }
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            //Insert Data
            string[] strSpParam = new string[21];
            oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            strSpParam[0] = "MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + MainAccID + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "SubAccount|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + SubAccId + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "Branch|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + Brnch + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "FromDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + FromDate + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "ToDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + ToDate + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "Segment|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + Segmnt + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "FinancialYr|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Session["LastFinYear"].ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Session["LastCompany"].ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "DrCrAmt|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + DrCrAmt + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "Group|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + Grp + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[10] = "ReportType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + RptType + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[11] = "BranchGroupType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + BranchGroupType + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[12] = "GroupType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + GroupType + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[13] = "ShowStatus|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + CheckDrCr + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[14] = "ZeroBal|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + ZeroBal + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[15] = "ActiveCurrency|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|3|" + Session["ActiveCurrency"].ToString().Split('~')[0] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[16] = "TradeCurrency|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|3|" + Session["TradeCurrency"].ToString().Split('~')[0] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //Segment Wise Break Up
            strSpParam[17] = "IsSegmentWiseBreakUp|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|3|" + "Y" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[18] = "IsTDayBal|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + (Chk_TDayBal.Checked ? "Y" : "N") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[19] = "IsConsolidate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + (Chk_ConsolidateBal.Checked ? "Y" : "N") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //Consolidate Same SubLedger Type Of Accounts
            strSpParam[20] = "IsAcConsolidate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|N|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //Out Put Variable (No Use Here But We Have To Pass That)
            strSpParam[21] = "LType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + String.Empty + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            //oGenericStoreProcedure = new GenericStoreProcedure();
            //if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
            //{
            //    oGenericMethod = new GenericMethod();
            //    string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
            //    string FilePath = "../ExportFiles/ServerDebugging/Fetch_SubSideryTrial" + strDateTime + ".txt";
            //    oGenericMethod.WriteFile(oGenericStoreProcedure.PrepareExecuteSpContent(strSpParam, "Fetch_SubSideryTrial"), FilePath, false);
            //}
            //string str = oGenericStoreProcedure.Procedure_String(strSpParam, "Fetch_SubSideryTrial");

            //===============New Additon
            oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            DataSet DsResult = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Fetch_SubSideryTrial");

            if (DsResult != null)
            {
                if (DsResult.Tables.Count > 0)
                {
                    if (DsResult.Tables[0].Rows.Count > 1)
                    {
                        if (Chk_SegWiseBreakUp.Checked == true)
                        {
                            if (CmbReportType.SelectedItem.Value.ToString() == "XL")
                            {
                                //DataTable DtExcel = DsResult.Tables[0];
                                ExportToExcel_Generic(DsResult, "2007");
                            }
                            else
                            {
                                ExportToHTML_Generic(DsResult, "2007");
                            }
                        }
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD234", "alert('No Record Found.'); Reset();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD235", "alert('No Record Found.');Reset();", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD236", "alert('No Record Found.');Reset();", true);

        }

        protected void ExportToHTML_Generic(DataSet Ds, string ExcelVersion)
        {
            if (Ds.Tables.Count > 0)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                # region Email Report Header Content
                string strDownloadFileName = "";
                string strReportHeader = null;
                string searchCriteria = null;
                oConverter = new BusinessLogicLayer.Converter();
                if (RadAsOnDate.Checked != true)
                    strReportHeader = "SubSidiery Trial Report For the Date Between " + oConverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oConverter.ArrangeDate2(dtTo.Value.ToString()) + ".";
                else
                    strReportHeader = "SubSidiery Trial Report As On Date " + oConverter.ArrangeDate2(dtDate.Value.ToString()) + ".";

                searchCriteria = "Search By";
                if (rdbMainAll.Checked == true)
                    searchCriteria = searchCriteria + " All Main Account,";
                else
                    searchCriteria = searchCriteria + " Selected Main Account,";

                if (rdAllSegment.Checked == true)
                    searchCriteria = searchCriteria + " All Segment,";
                else if (rdSelSegment.Checked == true)
                    searchCriteria = searchCriteria + " Selected Segment,";

                if (ddlGroup.SelectedValue.ToString() == "0")
                {
                    if (rdbranchAll.Checked == true)
                        searchCriteria = searchCriteria + " All Branch,";
                    if (rdbranchSelected.Checked == true)
                        searchCriteria = searchCriteria + " Selected Branch,";
                }
                else
                {
                    if (rdbranchAll.Checked == true)
                        searchCriteria = searchCriteria + " All Group,";
                    if (rdbranchSelected.Checked == true)
                        searchCriteria = searchCriteria + " Selected Group,";
                }

                if (rdbClientSelected.Checked == true)
                    searchCriteria = searchCriteria + " Selected Client";
                else if (rdbClientALL.Checked == true)
                    searchCriteria = searchCriteria + " All Client";
                else
                    searchCriteria = searchCriteria + " POA Client";

                string exlDateTime = oDbEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");

                string FileName = "SubSidieryTrial_" + exlTime;
                strDownloadFileName = "~/Documents/";

                string[] strHead = new string[4];
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = strReportHeader;
                GenericMethod oGenericMethod = new GenericMethod();
                DataTable dtcompname = oGenericMethod.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[3] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();
                # endregion

                # region Declare Mail Sending Variables
                string branchID = string.Empty;
                string groupID = string.Empty;
                string branchEmail = string.Empty;
                string groupEmail = string.Empty;
                string groupCCEmail = string.Empty;
                //string customerID = string.Empty;
                //string clientEmail = string.Empty;
                //string clientCCEmail = string.Empty;
                string usersList = null;
                string userID = string.Empty;
                string userEmail = string.Empty;
                //========Mail Sending SP Param Value=========
                string mailSubject = strReportHeader;
                string mailBody = string.Empty;
                string contactID = string.Empty;
                string mailID = string.Empty;
                # endregion


                if (Ds.Tables.Count > 1)
                {
                    if (Ds.Tables[1].Rows.Count > 0)
                    {
                        DataTable DtHtml = Ds.Tables[0];
                        string[] ColumnType = (Ds.Tables[2].Rows[2][0].ToString()).Split('~');
                        string[] ColumnSize = (Ds.Tables[2].Rows[1][0].ToString()).Split('~');
                        string[] ColumnWidthSize = (Ds.Tables[2].Rows[0][0].ToString()).Split('~');

                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, DtHtml, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

                        # region Mail send To Branch/Group
                        DataTable Dt = Ds.Tables[1];
                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (CmbMailSendTo.SelectedItem.Value.ToString() == "1")
                            {
                                if (Dt.Rows[i]["BranchGroupID"].ToString() != string.Empty) branchID = Dt.Rows[i]["BranchGroupID"].ToString();
                                if (Dt.Rows[i]["branch_cpEmail"].ToString() != string.Empty) branchEmail = Dt.Rows[i]["branch_cpEmail"].ToString();
                                contactID = branchID;
                                mailID = branchEmail;
                            }
                            else if (CmbMailSendTo.SelectedItem.Value.ToString() == "2")
                            {
                                if (Dt.Rows[i]["BranchGroupID"].ToString() != string.Empty) groupID = Dt.Rows[i]["BranchGroupID"].ToString();
                                if (Dt.Rows[i]["gpm_emailID"].ToString() != string.Empty) groupEmail = Dt.Rows[i]["gpm_emailID"].ToString();
                                if (Dt.Rows[i]["gpm_ccemailID"].ToString() != string.Empty) groupCCEmail = Dt.Rows[i]["gpm_ccemailID"].ToString();
                                contactID = groupID;
                                mailID = groupEmail;
                                if (groupCCEmail != string.Empty) mailID += "#" + groupCCEmail;
                                groupCCEmail = string.Empty;
                            }

                            // Run SP For Save Mail Details in Two Tables On EMailID/CCEmailID and For Different User
                            if (CmbMailSendTo.SelectedItem.Value.ToString() == "1")//For Mail Send To Branch                            
                            {
                                MailSend(mailSubject, mailBody, contactID, mailID);
                            }
                            if (CmbMailSendTo.SelectedItem.Value.ToString() == "2")  //Mail Send To Group 
                            {
                                if (mailID.Contains("#"))
                                {
                                    string[] mailIDList = mailID.Split('#');
                                    for (int j = 0; j < mailIDList.Length; j++)
                                    {
                                        MailSend(mailSubject, mailBody, contactID, mailIDList[j]);
                                    }
                                }
                                else
                                    MailSend(mailSubject, mailBody, contactID, mailID);
                            }
                        }
                        #endregion

                        # region Mail Send To Employee
                        if (CmbMailSendTo.SelectedItem.Value.ToString() == "3")
                        {
                            usersList = HiddenField_MailToEmp.Value;
                            if (usersList.Contains("#"))
                            {
                                string[] userIDMailList = usersList.Split('#');
                                for (int j = 0; j < userIDMailList.Length; j++)
                                {
                                    contactID = userIDMailList[j].Split('~')[0].ToString();
                                    mailID = userIDMailList[j].Split('~')[1].ToString();
                                    oGenericMethod = new GenericMethod();
                                    DataTable dt = oGenericMethod.GetDataTable("select * from tbl_master_email where eml_email='" + mailID + "' and eml_cntId='" + contactID + "' and eml_type='Official'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        MailSend(mailSubject, mailBody, contactID, mailID);
                                    }
                                }
                            }
                            else
                            {
                                contactID = usersList.Split('~')[0].ToString();
                                mailID = usersList.Split('~')[1].ToString();
                                oGenericMethod = new GenericMethod();
                                DataTable dt = oGenericMethod.GetDataTable("select * from tbl_master_email where eml_email='" + mailID + "' and eml_cntId='" + contactID + "' and eml_type='Official'");
                                if (dt.Rows.Count > 0)
                                {
                                    MailSend(mailSubject, mailBody, contactID, mailID);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "alert('No Record Found.');", true);
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript4", "<script language='javascript'>Reset(); </script>");
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "alert('No Record Found.');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript4", "<script language='javascript'>Reset(); </script>");
            }
        }

        protected void ExportToExcel_Generic(DataSet DsExcel, string ExcelVersion)
        {
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();

            # region Excel Report Header Content
            string strDownloadFileName = "";

            string strReportHeader = null;
            oConverter = new BusinessLogicLayer.Converter();
            if (RadAsOnDate.Checked != true)
                strReportHeader = "SubSidiery Trial Report For the Date Between " + oConverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oConverter.ArrangeDate2(dtTo.Value.ToString()) + ".";
            else
                strReportHeader = "SubSidiery Trial Report As On Date " + oConverter.ArrangeDate2(dtDate.Value.ToString()) + ".";

            string searchCriteria = null;
            searchCriteria = "Search By";

            if (rdbMainAll.Checked == true)
                searchCriteria = searchCriteria + " All Main Account,";
            else
                searchCriteria = searchCriteria + " Selected Main Account,";

            if (rdAllSegment.Checked == true)
                searchCriteria = searchCriteria + " All Segment,";
            else if (rdSelSegment.Checked == true)
                searchCriteria = searchCriteria + " Selected Segment,";

            if (ddlGroup.SelectedValue.ToString() == "0")
            {
                if (rdbranchAll.Checked == true)
                    searchCriteria = searchCriteria + " All Branch,";
                if (rdbranchSelected.Checked == true)
                    searchCriteria = searchCriteria + " Selected Branch,";
            }
            else
            {
                if (rdddlgrouptypeAll.Checked == true)
                    searchCriteria = searchCriteria + " All Group,";
                if (rdddlgrouptypeSelected.Checked == true)
                    searchCriteria = searchCriteria + " Selected Group,";
            }

            if (rdbClientSelected.Checked == true)
                searchCriteria = searchCriteria + " Selected Client";
            else if (rdbClientALL.Checked == true)
                searchCriteria = searchCriteria + " All Client";
            else
                searchCriteria = searchCriteria + " POA Client";

            string exlDateTime = oDbEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");

            string FileName = "SubSidiery_" + exlTime;
            strDownloadFileName = "~/Documents/";

            string[] strHead = new string[4];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = strReportHeader;
            DataTable dtcompname = oDbEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
            strHead[3] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();
            # endregion

            string[] ColumnType = (DsExcel.Tables[2].Rows[2][0].ToString()).Split('~');
            string[] ColumnSize = (DsExcel.Tables[2].Rows[1][0].ToString()).Split('~');
            string[] ColumnWidthSize = (DsExcel.Tables[2].Rows[0][0].ToString()).Split('~');

            //string[] ColumnType =      { "V", "N", "N", "N", "N" };
            //string[] ColumnSize =      { "150", "28,2", "28,2", "28,2", "28,2" };
            //string[] ColumnWidthSize = { "20", "15", "15", "15", "15" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DsExcel.Tables[0], Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

        }

        protected void MailSend(string mailSubject, string mailBody, string contactID, string mailID)
        {
            if (mailID != null)
            {
                string senderEmail = string.Empty;
                string[,] data = oDbEngine.GetFieldValue(" tbl_master_User,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + Session["userid"], 1);
                if (data[0, 0] != "n")
                    senderEmail = data[0, 0];

                string[] PageName = Request.Url.ToString().Split('/');
                DataTable dt = oDbEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + Session["userlastsegment"] + "'");
                string menuId = "";
                if (dt.Rows.Count != 0)
                    menuId = dt.Rows[0]["mnu_id"].ToString();
                else
                    menuId = "";
                DataTable dtsg = oDbEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + Session["userlastsegment"].ToString() + "'");
                string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                string[] strSpParam = new string[13];
                strSpParam[0] = "Emails_SenderEmailID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + senderEmail + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[1] = "Emails_Subject|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + mailSubject + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[2] = "Emails_Content|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + mailBody + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "Emails_HasAttachement|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|N|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "Emails_CreateApplication|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|10|" + menuId + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "Emails_CreateUser|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|10|" + Session["userid"].ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "Emails_Type|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|N|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "Emails_CompanyID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + Session["LastCompany"].ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[8] = "Emails_Segment|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + segmentname + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[9] = "EmailRecipients_ContactLeadID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|16|" + contactID + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[10] = "EmailRecipients_RecipientEmailID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|500|" + mailID.ToLower() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[11] = "EmailRecipients_RecipientType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|2|TO|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[12] = "EmailRecipients_Status|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|P|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;


                oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                int result = Convert.ToInt32(oGenericStoreProcedure.Procedure_String(strSpParam, "Insert_EmailDetails"));
                if (result == 1)
                    Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script language='javascript'>alert('Mail Sent Successfully.');</script>");
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "jscript2", "<script language='javascript'>alert('Mail Not Sent.');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript5", "<script language='javascript'>Reset();</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript2", "<script language='javascript'>alert('Mail Not Sent.');</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "jscript5", "<script language='javascript'>Reset();</script>");

            //================= End Modify on 12062013 for Mail Send Branch/Group/Employee==================================
        }
    }
}