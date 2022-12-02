using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_ReportTransferAmount : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther oFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        string data;
        static string Branch;
        static string SegmentID;
        static string Clients;
        static string CompanyID = null;
        static string Group = null;
        DataTable DTMainTable = new DataTable();
        DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        Converter oDbConverter = new Converter();
        DataTable DtSource = new DataTable();
        DataTable DtTarget = new DataTable();
        int NoofRowsAffected = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Group = null;
                SegmentID = null;
                Branch = null;
                CompanyID = null;
                DataTable DtSegComp = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    SegmentID = DtSegComp.Rows[0][1].ToString();
                }
                dtDate.EditFormatString = oDbConverter.GetDateFormat("Date");
                dtTransaction.EditFormatString = oDbConverter.GetDateFormat("Date");
                string[] FinalCialYear = Session["LastFinYear"].ToString().Split('-');
                dtDate.Value = Convert.ToDateTime("03/31/" + FinalCialYear[1].ToString());
                //dtTransaction.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                dtTransaction.Value = Convert.ToDateTime(oDbEngine.GetDate().ToShortDateString());
                FillComboSegment();
                Page.ClientScript.RegisterStartupScript(GetType(), "Bind", "<script language='JavaScript'>setcombovalue(" + Session["userlastsegment"] + ");</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "Bind12", "<script language='JavaScript'>Page_Load();</script>");
                //rdAll.Attributes.Add("OnClick", "MainAll('Client','all')");
                //rdSelected.Attributes.Add("OnClick", "MainAll('Client','Selc')");
                //rdAllBranch.Attributes.Add("OnClick", "MainAll('Branch','all')");
                //rdSelBrnh.Attributes.Add("OnClick", "MainAll('Branch','Selc')");
                //txtsegselected.Attributes.Add("onkeyup", "showOptionsforSunAc(this,'selectSubAccountForMainAccountAndBranch',event," + Session["userlastsegment"].ToString() + ")");
                if (Session["userlastsegment"].ToString() == "9")
                {
                    cmbSourceAc.Items.Insert(0, new ListItem("NSDL Clients A/c", "SYSTM00043"));
                }
                else if (Session["userlastsegment"].ToString() == "10")
                {
                    cmbSourceAc.Items.Insert(0, new ListItem("CDSL Clients A/c", "SYSTM00042"));
                }
                else if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18" || Session["userlastsegment"].ToString() == "28" || Session["userlastsegment"].ToString() == "11" || Session["userlastsegment"].ToString() == "12" || Session["userlastsegment"].ToString() == "14" || Session["userlastsegment"].ToString() == "15" || Session["userlastsegment"].ToString() == "16" || Session["userlastsegment"].ToString() == "17" || Session["userlastsegment"].ToString() == "23" || Session["userlastsegment"].ToString() == "24" || Session["userlastsegment"].ToString() == "25" || Session["userlastsegment"].ToString() == "26" || Session["userlastsegment"].ToString() == "29" || Session["userlastsegment"].ToString() == "30" || Session["userlastsegment"].ToString() == "31" || Session["userlastsegment"].ToString() == "32" || Session["userlastsegment"].ToString() == "33" || Session["userlastsegment"].ToString() == "34" || Session["userlastsegment"].ToString() == "35" || Session["userlastsegment"].ToString() == "36" || Session["userlastsegment"].ToString() == "37")
                {
                    cmbSourceAc.Items.Insert(0, new ListItem("Clients - Trading A/c", "SYSTM00001"));
                    cmbSourceAc.Items.Insert(1, new ListItem("Clients - Margin Deposit A/c", "SYSTM00002"));
                    cmbSourceAc.Items.Insert(2, new ListItem("Clients - FDR A/c", "SYSTM00003"));
                }

            }

            Page.ClientScript.RegisterStartupScript(GetType(), "callHight", "<script language='Javascript'>height();</script>");

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }
        public void FillComboSegment()
        {
            string[,] DT = oDbEngine.GetFieldValue("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as KK,tbl_master_segment", "SEGMENTID,EXCHANGENAME", "EXCHANGENAME=seg_name and seg_id=" + Session["userlastsegment"].ToString() + "", 2);
            cls.AddDataToDropDownList(DT, cmbSourceSeg);
            string[,] DT1 = oDbEngine.GetFieldValue("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as KK,tbl_master_segment", "SEGMENTID,EXCHANGENAME", "EXCHANGENAME=seg_name and seg_id not in(" + Session["userlastsegment"].ToString() + ",9,10)", 2);
            cls.AddDataToDropDownList(DT1, cmbTargetSeg);
        }
        protected void cmbSourceSeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[,] DT = oDbEngine.GetFieldValue("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID )as d", "d.segmentid,d.exchangename", " d.segmentid<>'" + cmbSourceSeg.SelectedItem.Value.ToString() + "'", 2);
            cls.AddDataToDropDownList(DT, cmbTargetSeg);
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }
            if (idlist[0] == "Clients")
            {
                Clients = str;
                Session["KeyValSegment"] = str;
                data = "Client~" + str1;
            }
            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                Branch = str;
                Session["KeyVal"] = str;
                data = "Branch~" + str1 + ":" + str;
            }

        }

        protected void grdInterSegmentTransfer_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.Header)
            {
                if (Session["userlastsegment"].ToString() == "9")
                {
                    gvr.Cells[4].Text = cmbSourceSeg.SelectedItem.Text + "[" + cmbSourceAc.SelectedItem.Text + "]";
                    gvr.Cells[5].Text = cmbTargetSeg.SelectedItem.Text + "[" + cmbTargetAc.SelectedItem.Text + "]";
                }
                else if (Session["userlastsegment"].ToString() == "10")
                {
                    gvr.Cells[4].Text = cmbSourceSeg.SelectedItem.Text + "[" + cmbSourceAc.SelectedItem.Text + "]";
                    gvr.Cells[5].Text = cmbTargetSeg.SelectedItem.Text + "[" + cmbTargetAc.SelectedItem.Text + "]";
                }
                else if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18" || Session["userlastsegment"].ToString() == "28" || Session["userlastsegment"].ToString() == "11" || Session["userlastsegment"].ToString() == "12" || Session["userlastsegment"].ToString() == "14" || Session["userlastsegment"].ToString() == "15" || Session["userlastsegment"].ToString() == "16" || Session["userlastsegment"].ToString() == "17" || Session["userlastsegment"].ToString() == "23" || Session["userlastsegment"].ToString() == "24" || Session["userlastsegment"].ToString() == "25" || Session["userlastsegment"].ToString() == "26" || Session["userlastsegment"].ToString() == "29" || Session["userlastsegment"].ToString() == "30" || Session["userlastsegment"].ToString() == "31" || Session["userlastsegment"].ToString() == "32" || Session["userlastsegment"].ToString() == "33" || Session["userlastsegment"].ToString() == "34" || Session["userlastsegment"].ToString() == "35" || Session["userlastsegment"].ToString() == "36" || Session["userlastsegment"].ToString() == "37")
                {
                    gvr.Cells[4].Text = cmbSourceSeg.SelectedItem.Text + "[" + cmbSourceAc.SelectedItem.Text + "]";
                    gvr.Cells[5].Text = cmbTargetSeg.SelectedItem.Text + "[" + cmbTargetAc.SelectedItem.Text + "]";
                }
            }
        }
        protected void grdInterSegmentTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtStock = (TextBox)row.FindControl("txtStock");
                Label lblCredit = (Label)row.FindControl("lblCredit");
                txtStock.Attributes.Add("onBlur", "javascript:DeliverableValue(" + txtStock.ClientID + ",'" + lblCredit.Text + "')");
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void grdInterSegmentTransfer_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
        void BindGrid()
        {
            if (Clients != "")
            {
                decimal Amount = 0;
                if (txtStartAmount.Text == "")
                    Amount = 0;
                else
                    Amount = Convert.ToDecimal(txtStartAmount.Text.ToString());
                string DebitCredit = null;
                if (rdAllDebit.Checked == true)
                    DebitCredit = "DR";
                else
                    DebitCredit = "CR";

                //if (Branch == null)
                //    Branch = "ALL";
                string CDSL_NSDL_NSE = null;
                if (Session["userlastsegment"].ToString() == "9")
                {
                    CDSL_NSDL_NSE = "NSDL";
                }
                else if (Session["userlastsegment"].ToString() == "10")
                {
                    CDSL_NSDL_NSE = "CDSL";
                }
                else if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18" || Session["userlastsegment"].ToString() == "28" || Session["userlastsegment"].ToString() == "11" || Session["userlastsegment"].ToString() == "12" || Session["userlastsegment"].ToString() == "14" || Session["userlastsegment"].ToString() == "15" || Session["userlastsegment"].ToString() == "16" || Session["userlastsegment"].ToString() == "17" || Session["userlastsegment"].ToString() == "23" || Session["userlastsegment"].ToString() == "24" || Session["userlastsegment"].ToString() == "25" || Session["userlastsegment"].ToString() == "26" || Session["userlastsegment"].ToString() == "29" || Session["userlastsegment"].ToString() == "30" || Session["userlastsegment"].ToString() == "31" || Session["userlastsegment"].ToString() == "32" || Session["userlastsegment"].ToString() == "33" || Session["userlastsegment"].ToString() == "34" || Session["userlastsegment"].ToString() == "35" || Session["userlastsegment"].ToString() == "36" || Session["userlastsegment"].ToString() == "37")
                {
                    CDSL_NSDL_NSE = "NA";
                }
                Branch = "ALL";
                DataSet DS = new DataSet();
                DS = oFAReportsOther.SelectInterSegmentTransfer(
                 Convert.ToString(cmbSourceAc.SelectedItem.Value),
           Convert.ToString(dtDate.Value),
           Convert.ToString(cmbSourceSeg.SelectedItem.Value),
           Convert.ToString(Session["LastCompany"]),
            Convert.ToString(Clients),
           Convert.ToString(Session["LastFinYear"]),
           Convert.ToDecimal(Amount),
           Convert.ToString(DebitCredit),
           Convert.ToString(Branch),
           Convert.ToString(cmbTargetSeg.SelectedItem.Value),
           Convert.ToString(cmbTargetAc.SelectedItem.Value),
           Convert.ToString(CDSL_NSDL_NSE),
             Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]),
           Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0])
         );
                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection con = new SqlConnection(conn);
                //SqlCommand cmd3 = new SqlCommand("SelectInterSegmentTransfer", con);
                //cmd3.CommandType = CommandType.StoredProcedure;
                //cmd3.Parameters.AddWithValue("@SAccount", cmbSourceAc.SelectedItem.Value);
                //cmd3.Parameters.AddWithValue("@Date", dtDate.Value);
                //cmd3.Parameters.AddWithValue("@Segment", cmbSourceSeg.SelectedItem.Value);
                //cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                //cmd3.Parameters.AddWithValue("@Client", Clients);
                //cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                //cmd3.Parameters.AddWithValue("@Amount", Amount);
                //cmd3.Parameters.AddWithValue("@DrCr", DebitCredit);
                //cmd3.Parameters.AddWithValue("@BranchID", Branch);
                //cmd3.Parameters.AddWithValue("@TargetSegment", cmbTargetSeg.SelectedItem.Value);
                //cmd3.Parameters.AddWithValue("@TargetAccount", cmbTargetAc.SelectedItem.Value);
                //cmd3.Parameters.AddWithValue("@CDSL_NSDL", CDSL_NSDL_NSE);
                /////Currency Setting
                //cmd3.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                //cmd3.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                //cmd3.CommandTimeout = 0;
                //SqlDataAdapter Adap = new SqlDataAdapter();
                //Adap.SelectCommand = cmd3;
                //Adap.Fill(DS);
                //con.Dispose();
                ViewState["DS"] = DS;
                grdInterSegmentTransfer.DataSource = DS;
                grdInterSegmentTransfer.DataBind();
                if (DS.Tables[0].Rows.Count > 0)
                {
                    if (Session["userlastsegment"].ToString() == "9")
                    {
                        grdInterSegmentTransfer.Columns[3].Visible = true;
                    }
                    else if (Session["userlastsegment"].ToString() == "10")
                    {
                        grdInterSegmentTransfer.Columns[3].Visible = true;
                    }
                    else if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18" || Session["userlastsegment"].ToString() == "28" || Session["userlastsegment"].ToString() == "11" || Session["userlastsegment"].ToString() == "12" || Session["userlastsegment"].ToString() == "14" || Session["userlastsegment"].ToString() == "15" || Session["userlastsegment"].ToString() == "16" || Session["userlastsegment"].ToString() == "17" || Session["userlastsegment"].ToString() == "23" || Session["userlastsegment"].ToString() == "24" || Session["userlastsegment"].ToString() == "25" || Session["userlastsegment"].ToString() == "26" || Session["userlastsegment"].ToString() == "29" || Session["userlastsegment"].ToString() == "30" || Session["userlastsegment"].ToString() == "31" || Session["userlastsegment"].ToString() == "32" || Session["userlastsegment"].ToString() == "33" || Session["userlastsegment"].ToString() == "34" || Session["userlastsegment"].ToString() == "35" || Session["userlastsegment"].ToString() == "36" || Session["userlastsegment"].ToString() == "37")
                    {
                        grdInterSegmentTransfer.Columns[3].Visible = false;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "AfterShow('a');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "AfterShow('b')", true);
                DS.Dispose();
                GC.Collect();
            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "Bind1234", "<script language='JavaScript'>alert('No Record Found !!');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "Bind123", "<script language='JavaScript'>Page_Load();</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "AfterShow('b');", true);
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18" || Session["userlastsegment"].ToString() == "28" || Session["userlastsegment"].ToString() == "11" || Session["userlastsegment"].ToString() == "12" || Session["userlastsegment"].ToString() == "14" || Session["userlastsegment"].ToString() == "15" || Session["userlastsegment"].ToString() == "16" || Session["userlastsegment"].ToString() == "17" || Session["userlastsegment"].ToString() == "23" || Session["userlastsegment"].ToString() == "24" || Session["userlastsegment"].ToString() == "25" || Session["userlastsegment"].ToString() == "26" || Session["userlastsegment"].ToString() == "29" || Session["userlastsegment"].ToString() == "30" || Session["userlastsegment"].ToString() == "31" || Session["userlastsegment"].ToString() == "32" || Session["userlastsegment"].ToString() == "33" || Session["userlastsegment"].ToString() == "34" || Session["userlastsegment"].ToString() == "35" || Session["userlastsegment"].ToString() == "36" || Session["userlastsegment"].ToString() == "37")
                fn_Client();
            else
                fn_ClientCDSL();
            BindGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtReport = new DataTable();
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
            foreach (GridViewRow row in grdInterSegmentTransfer.Rows)
            {
                Label lblSubAccID = (Label)row.FindControl("lblSubAccID");
                Label lblBranchID = (Label)row.FindControl("lblBranchID");
                TextBox txtStock = (TextBox)row.FindControl("txtStock");
                Label lblBenID = (Label)row.FindControl("lblBenID");
                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                if (ChkDelivery.Checked == true)
                {
                    DataRow DrRow = dtReport.NewRow();
                    DrRow["CashReportID"] = "0";
                    DrRow["JournalVoucherDetail_ID"] = "0";
                    DrRow["JournalVoucherDetail_ExchangeSegmentID"] = cmbSourceSeg.SelectedItem.Value;
                    DrRow["JournalVoucherDetail_BranchID"] = lblBranchID.Text;
                    DrRow["JournalVoucherDetail_MainAccountCode"] = cmbSourceAc.SelectedItem.Value;
                    DrRow["JournalVoucherDetail_SubAccountCode"] = lblBenID.Text;
                    if (rdAllDebit.Checked == true)
                    {
                        if (txtStock.Text != "")
                            DrRow["JournalVoucherDetail_AmountCr"] = txtStock.Text;
                        else
                            DrRow["JournalVoucherDetail_AmountCr"] = "0";
                        DrRow["JournalVoucherDetail_AmountDr"] = "0";
                    }
                    else
                    {
                        if (txtStock.Text != "")
                            DrRow["JournalVoucherDetail_AmountDr"] = txtStock.Text;
                        else
                            DrRow["JournalVoucherDetail_AmountDr"] = "0";
                        DrRow["JournalVoucherDetail_AmountCr"] = "0";
                    }
                    DrRow["JournalVoucherDetail_Narration"] = "Transfered from " + cmbSourceSeg.SelectedItem.Text.ToString() + "   To " + cmbTargetSeg.SelectedItem.Text.ToString();
                    DrRow["SubAccount1"] = "";
                    DrRow["MainAccount1"] = "";
                    DrRow["WithDrawl"] = "";
                    DrRow["Receipt"] = "";
                    DrRow["JournalVoucherDetail_ValueDate"] = dtTransaction.Value;
                    DrRow["JournalVoucherDetail_ValueDate1"] = oDbConverter.ArrangeDate(Convert.ToDateTime(dtTransaction.Value.ToString()).ToShortDateString()); ;
                    DrRow["Type"] = "";
                    dtReport.Rows.Add(DrRow);
                    ///////////////Reverse///////////
                    DataRow DrRowReverse = dtReport.NewRow();
                    DrRowReverse["CashReportID"] = "0";
                    DrRowReverse["JournalVoucherDetail_ID"] = "0";
                    DrRowReverse["JournalVoucherDetail_ExchangeSegmentID"] = cmbTargetSeg.SelectedItem.Value;
                    DrRowReverse["JournalVoucherDetail_BranchID"] = lblBranchID.Text;
                    DrRowReverse["JournalVoucherDetail_MainAccountCode"] = cmbTargetAc.SelectedItem.Value;
                    DrRowReverse["JournalVoucherDetail_SubAccountCode"] = lblSubAccID.Text;
                    if (rdAllDebit.Checked == true)
                    {
                        if (txtStock.Text != "")
                            DrRowReverse["JournalVoucherDetail_AmountDr"] = txtStock.Text;
                        else
                            DrRowReverse["JournalVoucherDetail_AmountDr"] = "0";
                        DrRowReverse["JournalVoucherDetail_AmountCr"] = "0";
                    }
                    else
                    {
                        if (txtStock.Text != "")
                            DrRowReverse["JournalVoucherDetail_AmountCr"] = txtStock.Text;
                        else
                            DrRowReverse["JournalVoucherDetail_AmountCr"] = "0";
                        DrRowReverse["JournalVoucherDetail_AmountDr"] = "0";
                    }
                    DrRowReverse["JournalVoucherDetail_Narration"] = "Transfered To " + cmbTargetSeg.SelectedItem.Text.ToString() + "  from " + cmbSourceSeg.SelectedItem.Text.ToString();
                    DrRowReverse["SubAccount1"] = "";
                    DrRowReverse["MainAccount1"] = "";
                    DrRowReverse["WithDrawl"] = "";
                    DrRowReverse["Receipt"] = "";
                    DrRowReverse["JournalVoucherDetail_ValueDate"] = dtTransaction.Value;
                    DrRowReverse["JournalVoucherDetail_ValueDate1"] = oDbConverter.ArrangeDate(Convert.ToDateTime(dtTransaction.Value.ToString()).ToShortDateString()); ;
                    DrRowReverse["Type"] = "";
                    dtReport.Rows.Add(DrRowReverse);

                }
            }
            string tabledata = oDbConverter.ConvertDataTableToXML(dtReport);

            NoofRowsAffected = oFAReportsOther.xmlJournalVoucherInterSegmentInsert(
                 Convert.ToString(tabledata),
           Convert.ToString(Session["userid"]),
           Convert.ToString(Session["LastFinYear"]),
           Convert.ToString(CompanyID),
           Convert.ToString(txtNarration.Text),
             Convert.ToDateTime(dtTransaction.Value).ToString("yyyy-MM-dd"),
             "",
             "",
               "",
               "YF",
                Convert.ToString(cmbSourceSeg.SelectedValue)
         );
            //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //con.Open();
            //SqlCommand com = new SqlCommand("xmlJournalVoucherInterSegmentInsert", con);
            //com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@journalData", tabledata);
            //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
            //com.Parameters.AddWithValue("@finyear", Session["LastFinYear"].ToString());
            //com.Parameters.AddWithValue("@compID", CompanyID.ToString());
            //com.Parameters.AddWithValue("@JournalVoucher_Narration", txtNarration.Text);
            //com.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(dtTransaction.Value).ToShortDateString());
            //com.Parameters.AddWithValue("@JournalVoucher_SettlementNumber", "");
            //com.Parameters.AddWithValue("@JournalVoucher_SettlementType", "");
            //com.Parameters.AddWithValue("@JournalVoucher_BillNumber", "");
            //com.Parameters.AddWithValue("@JournalVoucher_Prefix", "YF");
            //com.Parameters.AddWithValue("@segmentid", cmbSourceSeg.SelectedValue.ToString());
            //com.CommandTimeout = 0;
            //NoofRowsAffected = com.ExecuteNonQuery();
            //con.Close();
            //com.Dispose();
            //con.Dispose();
            //GC.Collect();
            if (NoofRowsAffected > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JScript", "alert('Transfer Successfully !!')", true);
            BindGrid();
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (Group == null)
                {
                    BindGroup();
                }
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
        void fn_Client()
        {

            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "')) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "')) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                            if (dtclient.Rows.Count > 0)
                            {
                                Clients = string.Empty;
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == "")
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }
                            else
                            {
                                Clients = "";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in (select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ")) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ")) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact,tbl_master_contactExchange", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalId=crg_cntID and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId='" + cmbTargetSeg.SelectedItem.Value + "')");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {

                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + Branch + ")");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_branchid in (" + Branch + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_branchid in (" + Branch + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            dtclient = oDbEngine.GetDataTable(@"select * from (Select distinct cnt_internalID from tbl_master_contact,
                        tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid 
                        (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in
                        (select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster 
                        where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + @"')) and cnt_internalId=crg_cntID and 
                        crg_company='" + Session["LastCompany"].ToString() + @"'and crg_exchange in (select (select exh_shortName 
                        from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange 
                        from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + @"' and 
                        exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + @"'))) as A 
                        inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  
                        cnt_internalId like 'CL%'  and cnt_internalid (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) 
                        and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from 
                        tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + @"')) and cnt_internalId=crg_cntID 
                        and crg_company='" + Session["LastCompany"].ToString() + @"'and crg_exchange in (select (select exh_shortName 
                        from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange 
                        from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + @"' and 
                        exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + @"'))) as B 
                        on A.cnt_internalId=B.cnt_internalId");
                            if (dtclient.Rows.Count > 0)
                            {
                                Clients = string.Empty;
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == "")
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }
                            else
                            {
                                Clients = "";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ")) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ")) and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Branch + ")");
                        dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in (" + Branch + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in (" + Branch + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
            }
            ////////////////
            else if (rdbClientSelected.Checked)
            {
                DataTable dtclient = new DataTable();
                dtclient = oDbEngine.GetDataTable("select * from (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalId in (" + Clients + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "' and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbSourceSeg.SelectedItem.Value + "'))) as A inner join (Select distinct cnt_internalID from tbl_master_contact,tbl_master_contactExchange WHERE  cnt_internalId like 'CL%'  and cnt_internalId in (" + Clients + ") and cnt_internalId=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "'and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentId is not null and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))) as B on A.cnt_internalId=B.cnt_internalId");
                if (dtclient.Rows.Count > 0)
                {
                    Clients = string.Empty;
                    for (int i = 0; i < dtclient.Rows.Count; i++)
                    {
                        if (Clients == "")
                            Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                        else
                            Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                    }

                }
                else
                {
                    Clients = "";
                }
            }


            /////////////////
        }

        void fn_ClientCDSL()
        {
            string NSDlCdsl = null;
            if (Session["userlastsegment"].ToString() == "10")
                NSDlCdsl = "select CDSLClients_ContactID from Master_CDSLClients where CDSLClients_ContactID is not null";
            if (Session["userlastsegment"].ToString() == "9")
                NSDlCdsl = "select NSDLClients_ContactID from Master_NSDLClients where NSDLClients_ContactID is not null";
            //NSDlCdsl = "select * from (Select distinct NsdlClients_ContactID from Master_NSDLClients,tbl_master_contactExchange WHERE  NsdlClients_ContactID like 'CL%' and NsdlClients_ContactID=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "' and crg_exchange ='NSDL') as A inner join (Select distinct NsdlClients_ContactID from Master_NSDLClients,tbl_master_contactExchange WHERE  NsdlClients_ContactID like 'CL%'  and NsdlClients_ContactID=crg_cntID and crg_company='" + Session["LastCompany"].ToString() + "' and crg_exchange='NSDL') as B on A.NsdlClients_ContactID=B.NsdlClients_ContactID";
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "')) and cnt_internalID in(" + NSDlCdsl + ") and  crg_cntID=cnt_internalId and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "'and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                Clients = string.Empty;
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == "")
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }
                            else
                            {
                                Clients = "";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ")) and cnt_internalID in(" + NSDlCdsl + ") and  crg_cntID=cnt_internalId and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "'and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                Clients = string.Empty;
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalID in(" + NSDlCdsl + ")");
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact,tbl_master_contactExchange", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalID in(" + NSDlCdsl + ") and  crg_cntID=cnt_internalId and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "'and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        //dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + Branch + ") and cnt_internalID in(" + NSDlCdsl + ")");
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact,tbl_master_contactExchange", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Branch + ") and cnt_internalID in(" + NSDlCdsl + ") and  crg_cntID=cnt_internalId and crg_exchange in (select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "'and exch_internalId in ('" + cmbTargetSeg.SelectedItem.Value + "'))");
                        if (dtclient.Rows.Count > 0)
                        {
                            Clients = string.Empty;
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == "")
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                        else
                        {
                            Clients = "";
                        }
                    }
                }
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                GenericMethod oGenericMethod = new GenericMethod();
                string CombinedQuery = null;
                DataTable DtPOAClient = new DataTable();
                if (Session["userlastsegment"].ToString() == "10")
                    NSDlCdsl = "Left(DPID,2)='12' and ";
                if (Session["userlastsegment"].ToString() == "9")
                    NSDlCdsl = "Left(DPID,2)='IN' and ";

                Clients = string.Empty;
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    if (rdbranchAll.Checked)
                        DtPOAClient = oGenericMethod.GetClient_FullDetail("D", ref CombinedQuery, 0, NSDlCdsl + "POAFlag=1 and Replace(SegName,' ','')='" + cmbTargetSeg.SelectedItem.Text + "'", "*", 1);
                    else
                        DtPOAClient = oGenericMethod.GetClient_FullDetail("D", ref CombinedQuery, 0, NSDlCdsl + "POAFlag=1 and Replace(SegName,' ','')='" + cmbTargetSeg.SelectedItem.Text + "' and BranchID in (" + Branch + ")", "*", 1);
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                        DtPOAClient = oGenericMethod.GetClient_FullDetail("D", ref CombinedQuery, 0, NSDlCdsl + "POAFlag=1 and Replace(SegName,' ','')='" + cmbTargetSeg.SelectedItem.Text + "' and GType='" + ddlgrouptype.SelectedItem.Text + "'", "*", 1);
                    else
                        DtPOAClient = oGenericMethod.GetClient_FullDetail("D", ref CombinedQuery, 0, NSDlCdsl + "POAFlag=1 and Replace(SegName,' ','')='" + cmbTargetSeg.SelectedItem.Text + "' and GType='" + ddlgrouptype.SelectedItem.Text + @"' and
                    GpmID in (" + Group + ")", "*", 1);
                }

                if (DtPOAClient.Rows.Count > 0)
                {
                    for (int i = 0; i < DtPOAClient.Rows.Count; i++)
                    {
                        if (Clients == "")
                            Clients = "'" + DtPOAClient.Rows[i][7].ToString() + "'";
                        else
                            Clients += "," + "'" + DtPOAClient.Rows[i][7].ToString() + "'";
                    }

                }

            }
        }
    }
}