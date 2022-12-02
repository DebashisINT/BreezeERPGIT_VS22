using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_FrmReport_MonthWiseTrialBalance : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
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

        ExcelFile objExcel = new ExcelFile();
        string SegMentName;
        FrmReport_MonthWiseTrialBalanceBL OFrmReport_MonthWiseTrialBalanceBL = new FrmReport_MonthWiseTrialBalanceBL();

        string SendEmailTag = "N";
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

            TxtMainAcc.Attributes.Add("onkeyup", "CallMainAccount(this,'SearchAllMainAccount',event)");


            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }



            if (!IsPostBack)
            {


                DataTable dsBind = new DataTable();

                /* For Tier structure

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_MonthYearCurrentFinyear", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        dsBind.Reset();
                        da.Fill(dsBind);
                */


                dsBind = OFrmReport_MonthWiseTrialBalanceBL.Fetch_GeneralTrial(Session["LastFinYear"].ToString());

                int j = dsBind.Rows.Count;
                for (int i = 0; i < dsBind.Rows.Count; i++)
                {
                    dtStartMonth.Items.Add(new ListItem(dsBind.Rows[i][1].ToString(), dsBind.Rows[i][0].ToString()));
                    dtEndMonth.Items.Add(new ListItem(dsBind.Rows[i][1].ToString(), dsBind.Rows[i][0].ToString()));
                    //if (i == (dsBind.Rows.Count - 1))
                    //    dtEndMonth.SelectedIndex =Convert.ToInt32(dsBind.Rows[i][0].ToString());
                }
                dtEndMonth.SelectedValue = j.ToString();


                //    }
                //}



                chkBranchNet.Checked = true;
                MainAcc = null;
                SegmentID = null;
                BranchId = null;
                CompanyID = null;
                DataTable DtSegComp = new DataTable();
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
                }
                if (Request.QueryString["mainacc"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>FromGeneralLedger();</script>");
                    FillGrid();
                }
                else
                {
                    string[] FinYear = Session["LastFinYear"].ToString().Split('-');

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
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
                MainAcc = str;
                data = "MainAcc~" + str;
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

            if (rdbMainAll.Checked == true)
            {

                if (dtBr.Rows[0][0].ToString() == "0")
                {
                    DrpdownClick();
                    FillGrid();
                    // ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", " SelectAccount()", true);
                }

            }
            else
            {
                FillGrid();
                // ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
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

                RptType = "A";

                if (ViewState["SubType"].ToString().Trim() == "Customers" || ViewState["SubType"].ToString().Trim() == "NSDL Clients" || ViewState["SubType"].ToString().Trim() == "CDSL Clients")
                {


                    if (ddlGroup.SelectedItem.Value == "0")
                    {
                        BranchGroupType = "B";
                        //if (rdbMainSelected.Checked == true)
                        //{

                        if (rdbranchAll.Checked == true)
                            Brnch = Session["userbranchHierarchy"].ToString();
                        else
                            Brnch = BranchId;

                        // }
                    }
                    else
                    {
                        BranchGroupType = "G";
                        //if (rdbMainSelected.Checked == true)
                        //{
                        if (ViewState["SubType"].ToString().Trim() == "Customers" || ViewState["SubType"].ToString().Trim() == "NSDL Clients" || ViewState["SubType"].ToString().Trim() == "CDSL Clients")
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

                        //}
                    }
                }

                if (rdAllSegment.Checked == true)
                {
                    DataTable DT = oDbEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
                    Segmnt = DT.Rows[0][0].ToString();
                    for (int i = 1; i < DT.Rows.Count; i++)
                    {
                        Segmnt = Segmnt + "," + DT.Rows[i][0].ToString();
                        ViewState["SegmentID"] = Segmnt;
                    }

                }
                else
                {

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


                DataSet ds = new DataSet();

                /* For Tier Structure

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("[Fetch_MonthWiseTrialBalance]", con))
                    {

                        da.SelectCommand.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                        da.SelectCommand.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                        da.SelectCommand.Parameters.AddWithValue("@FromInd", dtStartMonth.SelectedItem.Value);
                        da.SelectCommand.Parameters.AddWithValue("@ToInd", dtEndMonth.SelectedItem.Value);
                        da.SelectCommand.Parameters.AddWithValue("@MainAccount", HdnMainAcc.Value);
                        da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                        da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                        da.SelectCommand.Parameters.AddWithValue("@ReportType", RptType);
                        da.SelectCommand.Parameters.AddWithValue("@Segment", rdAllSegment.Checked ? "ALL" : Segmnt);
                        da.SelectCommand.Parameters.AddWithValue("@Group", Grp);
                        da.SelectCommand.Parameters.AddWithValue("@BranchGroutType", BranchGroupType);
                        da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                        da.SelectCommand.Parameters.AddWithValue("@SubledgerType", ViewState["SubType"].ToString());
                        da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                        da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                        da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                                      
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);

                        */



                ds = OFrmReport_MonthWiseTrialBalanceBL.Fetch_MonthWiseTrialBalance(Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(),
                    dtStartMonth.SelectedItem.Value.ToString(), dtEndMonth.SelectedItem.Value.ToString(), HdnMainAcc.Value.ToString(),
                    SubAccId, Brnch, RptType, rdAllSegment.Checked ? "ALL" : Segmnt, Grp, BranchGroupType, GroupType, ViewState["SubType"].ToString(),
                    ZeroBal, Session["ActiveCurrency"].ToString().Split('~')[0],
                    Session["TradeCurrency"].ToString().Split('~')[0]);


                //ViewState["dataset"] = ds;

                //    }
                //}

                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataTable dtLD = new DataTable();
                    dtLD = ds.Tables[0];
                    DataTable dtEx = new DataTable();
                    //DataTable dtEx = dtLD.Clone();
                    for (int z = 0; z < dtLD.Columns.Count; z++)
                    {
                        dtEx.Columns.Add(dtLD.Columns[z].ColumnName.ToString());
                    }




                    int minid = Convert.ToInt32(dtStartMonth.SelectedItem.Value.ToString());
                    int maxid = Convert.ToInt32(dtEndMonth.SelectedItem.Value.ToString());
                    int h = (maxid - minid) + 2;
                    Decimal[] SumC;
                    SumC = new Decimal[h];
                    int x = 0;
                    int y = 0;

                    for (int i = 0; i < dtLD.Rows.Count; i++)
                    {
                        //if (i != dtLD.Rows.Count - 1)
                        //{

                        //    if (dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString().Trim() == dtLD.Rows[i + 1]["MAIN_MAINACCOUNTID"].ToString().Trim())
                        //    {
                        //        if (i == 0)
                        //        {
                        //            DataRow row5 = dtEx.NewRow();
                        //            row5[4] = "";
                        //            row5[5] = "Test";
                        //            dtEx.Rows.Add(row5);

                        //            DataRow row4 = dtEx.NewRow();
                        //            row4[4] = "Main Account:  " + dtLD.Rows[i]["MAIN_MAINACNAME"].ToString() + " [ " + dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString() + " ] ";
                        //            row4[5] = "Test";
                        //            dtEx.Rows.Add(row4);
                        //        }


                        //    }
                        //    else
                        //    {
                        //        DataRow row5 = dtEx.NewRow();
                        //        row5[4] = "";
                        //        row5[5] = "Test";
                        //        dtEx.Rows.Add(row5);

                        //        DataRow row4 = dtEx.NewRow();
                        //        row4[4] = "Main Account:  " + dtLD.Rows[i]["MAIN_MAINACNAME"].ToString() + " [ " + dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString() + " ] ";
                        //        row4[5] = "Test";
                        //        dtEx.Rows.Add(row4);
                        //      }

                        //}
                        //else
                        //{
                        //    if (dtLD.Rows.Count > 1)
                        //    {

                        //        if (dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString().Trim() != dtLD.Rows[i - 1]["MAIN_MAINACCOUNTID"].ToString().Trim())
                        //        {      
                        //            DataRow row5 = dtEx.NewRow();
                        //            row5[4] = "";
                        //            row5[5] = "Test";
                        //            dtEx.Rows.Add(row5);

                        //            DataRow row4 = dtEx.NewRow();
                        //            row4[4] = "Main Account:  " + dtLD.Rows[i]["MAIN_MAINACNAME"].ToString() + " [ " + dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString() + " ] ";
                        //            row4[5] = "Break";
                        //            dtEx.Rows.Add(row4);                              

                        //        }


                        //    }
                        //    else
                        //    {
                        //        DataRow row5 = dtEx.NewRow();
                        //        row5[4] = "";
                        //        row5[5] = "Test";
                        //        dtEx.Rows.Add(row5);

                        //        DataRow row4 = dtEx.NewRow();
                        //        row4[4] = "Main Account:  " + dtLD.Rows[i]["MAIN_MAINACNAME"].ToString() + " [ " + dtLD.Rows[i]["MAIN_MAINACCOUNTID"].ToString() + " ] ";
                        //        row4[5] = "Break";
                        //        dtEx.Rows.Add(row4);

                        //    }

                        //}



                        if (dtLD.Rows[i]["MAIN_SUBLEDGERTYPE"].ToString() == "Customers" || dtLD.Rows[i]["MAIN_SUBLEDGERTYPE"].ToString() == "Brokers" || dtLD.Rows[i]["MAIN_SUBLEDGERTYPE"].ToString() == "Sub Brokers" || dtLD.Rows[i]["MAIN_SUBLEDGERTYPE"].ToString() == "NSDL Clients" || dtLD.Rows[i]["MAIN_SUBLEDGERTYPE"].ToString() == "CDSL Clients")
                        {
                            if (x == 0)
                            {
                                DataRow row5 = dtEx.NewRow();
                                row5[4] = "";
                                row5[5] = "Test";
                                dtEx.Rows.Add(row5);

                                DataRow row1 = dtEx.NewRow();
                                if (ddlGroup.SelectedItem.Value == "0")
                                {
                                    row1[4] = "Branch Name:  " + dtLD.Rows[i]["MAIN_BRANCHGROUP"].ToString();
                                }
                                else
                                {
                                    row1[4] = "Group Name:  " + dtLD.Rows[i]["MAIN_BRANCHGROUP"].ToString();
                                }
                                row1[5] = "Break";
                                dtEx.Rows.Add(row1);
                                x = x + 1;

                            }

                            //dtEx.Rows.Add(dtLD.Rows[i].ItemArray);
                            int b = dtLD.Columns.Count;
                            string[] rec;
                            rec = new string[b];
                            for (int s = 0; s < dtLD.Columns.Count; s++)
                            {
                                if (s >= 10)
                                {
                                    if (dtLD.Rows[i][s] != DBNull.Value)
                                    {
                                        rec[s] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLD.Rows[i][s].ToString()));
                                    }
                                    else
                                    {
                                        rec[s] = "";
                                    }
                                }
                                else
                                {
                                    rec[s] = dtLD.Rows[i][s].ToString();
                                }
                            }
                            dtEx.Rows.Add(rec);

                            if (i != dtLD.Rows.Count - 1)
                            {
                                if (dtLD.Rows[i]["MAIN_BRANCHGROUPID"].ToString() == dtLD.Rows[i + 1]["MAIN_BRANCHGROUPID"].ToString())
                                {
                                    for (int p = 0; p < h; p++)
                                    {
                                        if (dtLD.Rows[i][10 + p].ToString() != "")
                                        {
                                            SumC[p] = SumC[p] + Convert.ToDecimal(dtLD.Rows[i][10 + p]);
                                        }
                                        else
                                        {
                                            SumC[p] = SumC[p] + 0;
                                        }

                                    }
                                }
                                else
                                {
                                    for (int p = 0; p < h; p++)
                                    {
                                        if (dtLD.Rows[i][10 + p].ToString() != "")
                                        {
                                            SumC[p] = SumC[p] + Convert.ToDecimal(dtLD.Rows[i][10 + p]);
                                        }
                                        else
                                        {
                                            SumC[p] = SumC[p] + 0;
                                        }

                                    }
                                    DataRow dr = dtEx.NewRow();



                                    if (chkBranchNet.Checked == true)
                                    {
                                        DataRow row5 = dtEx.NewRow();
                                        row5[4] = "";
                                        row5[5] = "Test";
                                        dtEx.Rows.Add(row5);

                                        for (int m = 0; m < 9; m++)
                                        {
                                            if (m == 0 || m == 7)
                                                dr[m] = 0;
                                            else if (m == 4)
                                                dr[m] = "Branch/Group Net";
                                            else

                                                dr[m] = "";
                                        }
                                        int t = 0;
                                        for (int r = 10; r < h + 10; r++)
                                        {
                                            dr[r] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumC[t].ToString()));
                                            t = t + 1;
                                        }
                                        dtEx.Rows.Add(dr);
                                        for (int p = 0; p < h; p++)
                                        {
                                            SumC[p] = 0;
                                        }
                                    }
                                    x = 0;


                                }
                            }
                            else
                            {
                                if (dtLD.Rows.Count > 1)
                                {

                                    if (dtLD.Rows[i]["MAIN_BRANCHGROUPID"].ToString() == dtLD.Rows[i - 1]["MAIN_BRANCHGROUPID"].ToString())
                                    {
                                        for (int p = 0; p < h; p++)
                                        {
                                            if (dtLD.Rows[i][10 + p].ToString() != "")
                                            {
                                                SumC[p] = SumC[p] + Convert.ToDecimal(dtLD.Rows[i][10 + p]);
                                            }
                                            else
                                            {
                                                SumC[p] = SumC[p] + 0;
                                            }

                                        }

                                        if (chkBranchNet.Checked == true)
                                        {
                                            DataRow row5 = dtEx.NewRow();
                                            row5[4] = "";
                                            row5[5] = "Test";
                                            dtEx.Rows.Add(row5);

                                            DataRow dr = dtEx.NewRow();
                                            for (int m = 0; m < 9; m++)
                                            {
                                                if (m == 0 || m == 7)
                                                    dr[m] = 0;
                                                else if (m == 4)
                                                    dr[m] = "Branch/Group Net";
                                                else

                                                    dr[m] = "";
                                            }
                                            int t = 0;
                                            for (int r = 10; r < h + 10; r++)
                                            {
                                                dr[r] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumC[t].ToString()));
                                                t = t + 1;
                                            }
                                            dtEx.Rows.Add(dr);
                                            for (int p = 0; p < h; p++)
                                            {
                                                SumC[p] = 0;
                                            }
                                        }
                                        x = 0;
                                    }
                                    else
                                    {
                                        for (int p = 0; p < h; p++)
                                        {
                                            if (dtLD.Rows[i][10 + p].ToString() != "")
                                            {
                                                SumC[p] = SumC[p] + Convert.ToDecimal(dtLD.Rows[i][10 + p]);
                                            }
                                            else
                                            {
                                                SumC[p] = SumC[p] + 0;
                                            }

                                        }
                                        if (chkBranchNet.Checked == true)
                                        {
                                            DataRow row5 = dtEx.NewRow();
                                            row5[4] = "";
                                            row5[5] = "Test";
                                            dtEx.Rows.Add(row5);

                                            DataRow dr = dtEx.NewRow();
                                            for (int m = 0; m < 9; m++)
                                            {
                                                if (m == 0 || m == 7)
                                                    dr[m] = 0;
                                                else if (m == 4)
                                                    dr[m] = "Branch/Group Net";
                                                else

                                                    dr[m] = "";
                                            }
                                            int t = 0;
                                            for (int r = 10; r < h + 10; r++)
                                            {
                                                dr[r] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumC[t].ToString()));
                                                t = t + 1;
                                            }
                                            dtEx.Rows.Add(dr);
                                            for (int p = 0; p < h; p++)
                                            {
                                                SumC[p] = 0;
                                            }
                                        }
                                        x = 0;


                                    }
                                }
                                else
                                {
                                    for (int p = 0; p < h; p++)
                                    {
                                        if (dtLD.Rows[i][10 + p].ToString() != "")
                                        {
                                            SumC[p] = SumC[p] + Convert.ToDecimal(dtLD.Rows[i][10 + p]);
                                        }
                                        else
                                        {
                                            SumC[p] = SumC[p] + 0;
                                        }

                                    }
                                    if (chkBranchNet.Checked == true)
                                    {
                                        DataRow row5 = dtEx.NewRow();
                                        row5[4] = "";
                                        row5[5] = "Test";
                                        dtEx.Rows.Add(row5);

                                        DataRow dr = dtEx.NewRow();
                                        for (int m = 0; m < 9; m++)
                                        {
                                            if (m == 0 || m == 7)
                                                dr[m] = 0;
                                            else if (m == 4)
                                                dr[m] = "Branch/Group Net";
                                            else

                                                dr[m] = "";
                                        }
                                        int t = 0;
                                        for (int r = 10; r < h + 10; r++)
                                        {
                                            dr[r] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumC[t].ToString()));
                                            t = t + 1;
                                        }
                                        dtEx.Rows.Add(dr);

                                        for (int p = 0; p < h; p++)
                                        {
                                            SumC[p] = 0;
                                        }
                                    }
                                    x = 0;
                                }

                            }


                        }
                        else
                        {
                            //dtEx.Rows.Add(dtLD.Rows[i].ItemArray);
                            int b = dtLD.Columns.Count;
                            string[] rec;
                            rec = new string[b];
                            for (int s = 0; s < dtLD.Columns.Count; s++)
                            {
                                if (s >= 10)
                                {
                                    if (dtLD.Rows[i][s] != DBNull.Value)
                                    {
                                        rec[s] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLD.Rows[i][s].ToString()));
                                    }
                                    else
                                    {
                                        rec[s] = "";
                                    }
                                }
                                else
                                {
                                    rec[s] = dtLD.Rows[i][s].ToString();
                                }
                            }
                            dtEx.Rows.Add(rec);

                        }

                    }



                    dtEx.Columns[4].ColumnName = "Name";
                    dtEx.Columns[7].ColumnName = "Opening Balance";
                    dtEx.Columns["MAIN_CLOSINGBALANCE"].ColumnName = "Closing Balance";
                    dtEx.Columns.Remove("MAIN_ID");
                    dtEx.Columns.Remove("MAIN_MAINACCOUNTID");
                    dtEx.Columns.Remove("MAIN_SUBACCOUNTID");
                    dtEx.Columns.Remove("MAIN_SUBLEDGERTYPE");
                    dtEx.Columns.Remove("MAIN_BRANCHGROUPID");
                    dtEx.Columns.Remove("MAIN_GROUPTYPE");
                    dtEx.Columns.Remove("MAIN_BRANCHGROUP");
                    dtEx.Columns.Remove("MAIN_MAINACNAME");
                    dtEx.AcceptChanges();

                    //DataRow row7 = dtEx.NewRow();
                    //row7[0] = "";
                    //row7[1] = "Test";
                    //dtEx.Rows.Add(row7);

                    //dtEx.Rows.Add(ds.Tables[1].Rows[0].ItemArray);

                    DataTable dtTot = new DataTable();
                    dtTot = ds.Tables[1];
                    int j = dtTot.Columns.Count;
                    string[] arr2;
                    arr2 = new string[j];
                    for (int s = 0; s < dtTot.Columns.Count; s++)
                    {
                        if (s >= 1)
                        {
                            if (dtTot.Rows[0][s] != DBNull.Value)
                            {
                                arr2[s] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtTot.Rows[0][s].ToString()));
                            }
                            else
                            {
                                arr2[s] = "";
                            }
                        }
                        else
                        {
                            arr2[s] = dtTot.Rows[0][s].ToString();
                        }
                    }
                    dtEx.Rows.Add(arr2);

                    DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                    DataTable dtReportHeader = new DataTable();
                    dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                    DataRow HeaderRow = dtReportHeader.NewRow();
                    HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                    dtReportHeader.Rows.Add(HeaderRow);
                    DataRow DrRowR1 = dtReportHeader.NewRow();
                    DrRowR1[0] = lblReportHeader.Text;
                    dtReportHeader.Rows.Add(DrRowR1);


                    DataRow DrRowR7 = dtReportHeader.NewRow();
                    DrRowR7[0] = "Main Account : " + TxtMainAcc.Text;
                    dtReportHeader.Rows.Add(DrRowR7);


                    DataRow DrRowR2 = dtReportHeader.NewRow();
                    DrRowR2[0] = "Month Wise Trial Balance From " + dtStartMonth.SelectedItem.Text + " To " + dtEndMonth.SelectedItem.Text;
                    dtReportHeader.Rows.Add(DrRowR2);

                    DataRow DrRowR3 = dtReportHeader.NewRow();
                    if (rdAllSegment.Checked == true)
                    {
                        DrRowR3[0] = "Segment: ALL ";
                    }
                    else
                    {
                        DrRowR3[0] = "Segment: " + Span2.InnerHtml.ToString();
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


                    //oconverter.ExcelImport(dtBilling, "Daily Billing");
                    objExcel.ExportToExcelforExcel(dtEx, "Month Wise Trial", "Total", dtReportHeader, dtReportFooter);




                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "NoRecord", "<script>HideAll();</script>");


                }


                #endregion

            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "NoRecord", "<script>HideAll();</script>");
            }
        }

        protected void BtnDropdown_Click(object sender, EventArgs e)
        {

            DataTable dtSub = oDbEngine.GetDataTable("MASTER_MAINACCOUNT ", " MAINACCOUNT_SUBLEDGERTYPE ", "MAINACCOUNT_ACCOUNTCODE ='" + HdnMainAcc.Value + "'");
            if (dtSub.Rows[0][0].ToString() == "Customers" || dtSub.Rows[0][0].ToString() == "NSDL Clients" || dtSub.Rows[0][0].ToString() == "CDSL Clients")
            {

                string Type = dtSub.Rows[0][0].ToString();
                ViewState["SubType"] = dtSub.Rows[0][0].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
            }
            else
            {
                string Type = dtSub.Rows[0][0].ToString();
                ViewState["SubType"] = dtSub.Rows[0][0].ToString();
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

            FillGrid();

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

                // lblReportHeader.Text = "Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
                FillGrid();


            }

        }


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

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                    if (Type != "Customers")
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

            RptType = "A";


            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
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
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
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

            //if (rdDebit.Checked == true)
            //    CheckDrCr = "D";
            //else if (rdCredit.Checked == true)
            //    CheckDrCr = "C";
            //else
            //    CheckDrCr = "B";


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








            DataSet ds = new DataSet();

            /* For Tier Structure
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_SubSideryTrial", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MainAccountID", MainAccID);
                    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccId);
                    da.SelectCommand.Parameters.AddWithValue("@Branch", Brnch);
                    //da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                    //  da.SelectCommand.Parameters.AddWithValue("@ToDate", dtDate.Value);
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
                    da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);




                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);

                    */


            ds = OFrmReport_MonthWiseTrialBalanceBL.Fetch_SubSideryTrial(MainAccID, SubAccId, Brnch, Segmnt, Session["LastFinYear"].ToString(), Session["LastCompany"].ToString(),
              DrCrAmt.ToString(), Grp, RptType, BranchGroupType, GroupType, CheckDrCr, ZeroBal, Session["ActiveCurrency"].ToString().Split('~')[0],
              Session["TradeCurrency"].ToString().Split('~')[0]);

            //ViewState["dataset"] = ds;

            //    }
            //}

            DataTable dtComp = oDbEngine.GetDataTable("tbl_master_company", " cmp_name,(Select top 1 phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId=cmp_internalid) as cmpphno,(select top 1(isnull(add_address1,'')+' '+ isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(city_name,'')+'-'+  isnull(add_pin,'')) from tbl_master_address,tbl_master_city where add_city=city_id and add_cntID=cmp_internalid AND add_entity='Company' AND add_addressType='Office')as cmpaddress,(select top 1 eml_email from tbl_master_email   where eml_cntid=cmp_internalid) as Email  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");

            ReportDocument report = new ReportDocument();
            // ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\SubSidiaryTrial.xsd");
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            string tmpPdfPath = string.Empty;

            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\SubSidiaryTrial.rpt");

            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();


            //  report.SetParameterValue("@ReprtDt", (string)"Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]");


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
                report.SetParameterValue("@Segment", (string)Span2.InnerText.ToString());
            }

            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "SubsidiaryTrial");
            report.Dispose();
            GC.Collect();

        }


    }
}