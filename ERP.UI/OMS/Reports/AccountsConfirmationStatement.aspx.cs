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
    public partial class Reports_AccountsConfirmationStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();

        string data;
        string SubLedgerType = "";
        string Branch;
        string Segment;
        string SegmentT;
        string MainAcID;
        string SubAcID;
        string SegMentName;
        string Group;
        string BranchId;
        string MainAcc;
        string Clients;
        string SegN = "";

        string CompanyID = null;
        string SegmentID = null;

        AccountsConfirmationStatementBL OAccountsConfirmationStatementBL = new AccountsConfirmationStatementBL();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
            {
                ddlAccountType.SelectedValue = "3";
                //  ddlAccountType.Attributes.Add("onchange", "AccountChange(this.value)");
                Page.ClientScript.RegisterStartupScript(GetType(), "JSd", "<script language='JavaScript'>AccountChange('" + ddlAccountType.SelectedItem.Value + "');</script>");

            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtseg = oDBEngine.GetDataTable("tbl_master_companyexchange", "exch_internalid", "exch_compid='" + Session["LastCompany"].ToString().Trim() + "' and exch_segmentid is not null");
                for (int i = 0; i < dtseg.Rows.Count; i++)
                {
                    if (SegmentID == null)
                    {
                        SegmentID = SegmentID + dtseg.Rows[i][0].ToString();


                    }
                    else
                    {
                        SegmentID = SegmentID + "," + dtseg.Rows[i][0].ToString();
                    }
                }
                ViewState["SegmentID"] = SegmentID;
            }


            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                DtSatementDate.EditFormatString = oconverter.GetDateFormat("Date");
                SetDatteFinYear();


                //dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                //dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DtSatementDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());


                DataTable DtSegComp = new DataTable();
                SegmentID = null;
                CompanyID = null;
                string SegMentName = null;


                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                //if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                //else
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId ,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg , isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                        {
                            SegmentID = DtSegComp.Rows[i][1].ToString();
                            SegMentName = DtSegComp.Rows[i][2].ToString();
                            SegN = "'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                        else
                        {
                            SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                            SegMentName = SegMentName + "," + DtSegComp.Rows[i][2].ToString();
                            SegN = SegN + ",'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                    }
                    //ViewState["SegMentName"] = SegMentName;
                    //Session["CompanyID"] = CompanyID;
                    ViewState["SegmentID"] = SegmentID;
                    // HdnSegment.Value = SegmentID;

                    litSegment.InnerText = SegN;
                    //     litSegment.InnerText = SegMentName;


                }
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }

        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", "FINYEAR_STARTDATE ,FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_STARTDATE"].ToString());
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_ENDDATE"].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
                dtTo.Value = EndDate;
            else
                dtTo.Value = TodayDate;

            dtFrom.Value = StartDate;

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
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
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

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            return data;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Ac Name")
                    {
                        SubLedgerType = "";
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
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        SubLedgerType = AcVal[1];
                    }
                }
                if (idlist[0] == "Branch")
                {
                    Branch = str;
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    SegmentT = str;
                    data = "Segment~" + str1;
                }
                else if (idlist[0] == "Ac Name")
                {
                    MainAcID = str;
                    data = "Ac Name~" + str + "~" + SubLedgerType;
                    // FillDropDown();
                }
                else if (idlist[0] == "Sub Ac")
                {
                    SubAcID = str;
                    data = "Sub Ac~" + str;
                }
                else if (idlist[0] == "Clients")
                {
                    SubAcID = str;
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    SubAcID = str;
                    data = "Group~" + str;
                }
            }
        }

        #endregion
        void fn_Client()
        {
            BranchId = HdnBranch.Value;
            Group = HdnGroup.Value;
            if (ddlAccountType.SelectedItem.Value == "0")
                MainAcc = "'SYSTM00001'";
            else if (ddlAccountType.SelectedItem.Value == "1")
                MainAcc = "'SYSTM00002'";
            else if (ddlAccountType.SelectedItem.Value == "2")
                MainAcc = "'SYSTM00001','SYSTM00002'";
            else
                MainAcc = HdnMainAc.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();

            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + BranchId + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;

            ViewState["Clients"] = Clients;
        }
        void fn_ClientCDSL()
        {
            ViewState["Clients"] = null;
            BranchId = HdnBranch.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAc.Value;
            SubLedgerType = HdnSubLedgerType.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();
            string NSDlCdsl = null;
            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", "  grp_contactid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", " grp_contactid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;
            ViewState["Clients"] = Clients;
        }
        public void fn_Custom()
        {
            MainAcID = HdnMainAc.Value;
            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                DataTable dtclient = new DataTable();
                dtclient = oDBEngine.GetDataTable("Trans_AccountsLedger", "AccountsLedger_SubAccountID", " AccountsLedger_MainAccountID=" + MainAcID + " and AccountsLedger_TransactionDate between '" + Convert.ToDateTime(dtFrom.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(dtTo.Value).ToShortDateString() + "'");
                if (dtclient.Rows.Count > 0)
                {
                    for (int i = 0; i < dtclient.Rows.Count; i++)
                    {
                        if (Clients == null)
                            Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                        else
                            Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                    }

                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;
            ViewState["Clients"] = Clients;
        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnSegment.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;

            string EXCHID = "";
            string ExchSeg = "";

            if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
            {
                EXCHID = "N";
            }
            else
            {
                EXCHID = Session["ExchangeSegmentID"].ToString();

            }
            if (HdnSegment.Value.ToString() != "")
            {
                ExchSeg = HdnSegment.Value;
            }
            else
            {
                ExchSeg = ViewState["SegmentID"].ToString();
            }
            string GpType = null;
            string GpId = null;
            string AddType = null;
            string Debit = "0";
            string Credit = "0";
            string GracePeriod = "0";
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
            {
                fn_ClientCDSL();
            }
            else if (SubLedgerType == "")
            {
                fn_Client();
            }
            else
            {
                fn_Custom();
            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                Segment = Session["usersegid"].ToString();
            }
            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();

            string employeeId = txtSignature_hidden.Value;
            string SubAccountSearch = " and AccountsLedger_SubAccountID in(" + ViewState["Clients"].ToString() + ")";
            string SingleDouble = null;
            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";
            string GrpType = "NA";
            if (ddlgrouptype.SelectedItem != null)
                GrpType = ddlgrouptype.SelectedItem.Text;
            MainAcID = MainAcID.Replace("'", "");

            /* For Tier Structure 
             using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
             {
         
                 SqlCommand cmd3 = new SqlCommand("Fetch_AccountsConfirmation", con);
                 cmd3.CommandType = CommandType.StoredProcedure;
                 cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
                 cmd3.Parameters.AddWithValue("@segmentID", ExchSeg);
                 cmd3.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(dtFrom.Value));
                 cmd3.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(dtTo.Value));

                 cmd3.Parameters.AddWithValue("@SubAccountSearch", SubAccountSearch);
                 cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                 cmd3.Parameters.AddWithValue("@SingleDouble", SingleDouble);
                 cmd3.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
                 cmd3.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);
                 cmd3.Parameters.AddWithValue("@SubAcID", ViewState["Clients"].ToString());
                 cmd3.Parameters.AddWithValue("@ExchangeSegment", EXCHID);
                 cmd3.Parameters.AddWithValue("@groupType", GrpType);
                 cmd3.Parameters.AddWithValue("@orderType", ddlPrintOrder.SelectedItem.Value);
                 cmd3.Parameters.AddWithValue("@StatementDate", Convert.ToDateTime(DtSatementDate.Value));
                 cmd3.Parameters.AddWithValue("@MainAccID", MainAcID);

             

                 cmd3.CommandTimeout = 0;
                 SqlDataAdapter Adap = new SqlDataAdapter();
                 Adap.SelectCommand = cmd3;
                 Adap.Fill(dsCrystal);
                 cmd3.Dispose();
                 con.Dispose();

                 */

            dsCrystal = OAccountsConfirmationStatementBL.Fetch_AccountsConfirmation(Session["LastCompany"].ToString(), ExchSeg, dtFrom.Value.ToString(), dtTo.Value.ToString(),
               SubAccountSearch, Session["LastFinYear"].ToString(), SingleDouble, txtHeader_hidden.Value,
               txtFooter_hidden.Value, ViewState["Clients"].ToString(), EXCHID, GrpType, ddlPrintOrder.SelectedItem.Value.ToString(),
               DtSatementDate.Value.ToString(), MainAcID);


            GC.Collect();
            byte[] logoinByte = null;
            byte[] SignatureinByte;

            //dsCrystal.Tables[1].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
            //dsCrystal.Tables[1].Columns.Add("Status", System.Type.GetType("System.String"));
            dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[0].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));
            DataTable dtcmp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
            string filePath = "";
            filePath = @"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp";
            //if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_test.bmp"), out logoinByte) != 1)
            //{
            //   ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

            //}
            //else
            //{
            if (ChkLogo.Checked == true)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) == 1)
                {
                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                    }
                }
                else
                {
                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                }

            }

            if (ChkSignatory.Checked == true)
            {
                if (oconverter.getSignatureImage(employeeId, out SignatureinByte, "NSE") == 1)
                {
                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Signature"] = SignatureinByte;
                        dsCrystal.Tables[0].Rows[i]["Status"] = "a";
                    }
                }
            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Status"] = "This is an electronicaly generated statement that requires no Signature. *";
                }
            }
            //dsCrystal.Tables[0].WriteXmlSchema("E:\\CommonFolderInfluxCRM\\Reports\\AccountsConfirmationStatement.xsd");
            //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Detailaccounts.xsd");
            //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//AccountsConfirmationStatement.xsd");
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');MULTI
            string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\Reports\\AccountsConfirmationStatement.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(dsCrystal);
            string param = "";
            if (dsCrystal.Tables[0].Rows.Count > 0)
            {
                if (dsCrystal.Tables[0].Rows[0]["sebino"].ToString().StartsWith("--") == true)
                    param = "SEBI Reg No. - " + dsCrystal.Tables[0].Rows[0]["sebino"].ToString().Split('-')[2];
                if (dsCrystal.Tables[0].Rows[0]["sebino"].ToString().StartsWith("_") == true)
                    param = "FMC Reg No. - " + dsCrystal.Tables[0].Rows[0]["sebino"].ToString().Split('_')[1];
                if (dsCrystal.Tables[0].Rows[0]["sebino"].ToString().StartsWith(":") == true)
                    param = "";
            }
            DataTable dtsubledgertype = oDBEngine.GetDataTable("select ltrim(rtrim(isnull(MAINACCOUNT_SUBLEDGERTYPE,''))) FROM MASTER_MAINACCOUNT  WHERE MAINACCOUNT_ACCOUNTCODE in ('" + MainAcID.ToString() + "')");
            string subledgertype = "";
            if (dtsubledgertype.Rows.Count > 0)
                subledgertype = dtsubledgertype.Rows[0][0].ToString();
            else
                subledgertype = "Customers";
            reportObj.SetParameterValue("@parameter", (object)param.ToString().Trim());
            reportObj.SetParameterValue("@suppressparameter", (object)subledgertype.ToString().Trim());
            //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsConfirmationStatement");
            reportObj.Dispose();
            GC.Collect();
        }
        //}
    }
}