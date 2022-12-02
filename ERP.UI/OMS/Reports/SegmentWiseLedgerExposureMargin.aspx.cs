using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_SegmentWiseLedgerExposureMargin : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        int rowcount = 0;
        string data;
        string BranchId = null;
        string Clients;
        string Group = null;
        string MainAcc = null;
        static string CompanyID = null;
        string SegmentID = null;
        string SegMentName;
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
                MainAcc = null;
                SegmentID = null;
                BranchId = null;
                CompanyID = null;
                DataTable DtSegComp = new DataTable();

                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
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
                }



                //  }
                if (Request.QueryString["mainacc"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>FromGeneralLedger();</script>");

                }
                else
                {
                    string[] FinYear = Session["LastFinYear"].ToString().Split('-');
                    dtDate.EditFormatString = oconverter.GetDateFormat("Date");

                    dtDate.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    rdAllSegment.Attributes.Add("onclick", "MainAll('all','Segment')");
                    rdSelSegment.Attributes.Add("onclick", "MainAll('Selc','Segment')");
                }
                BindGroup();
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
                Clients = String.Empty;
                foreach (string strSplit in cl)
                {
                    if (Clients != String.Empty)
                        Clients = Clients + "," + strSplit.Split(';')[0];
                    else
                        Clients = strSplit.Split(';')[0];

                }
                data = "Clients~" + Clients;
                ViewState["Clients"] = Clients;
            }

            else if (idlist[0] == "Group")
            {
                Group = String.Empty;
                foreach (string strSplit in cl)
                {
                    if (Group != String.Empty)
                        Group = Group + "," + strSplit.Split(';')[0];
                    else
                        Group = strSplit.Split(';')[0];

                }
                data = "Group~" + Group;
            }
            else if (idlist[0] == "Branch")
            {
                BranchId = String.Empty;
                foreach (string strSplit in cl)
                {
                    if (BranchId != String.Empty)
                        BranchId = BranchId + "," + strSplit.Split(';')[0];
                    else
                        BranchId = strSplit.Split(';')[0];

                }
                data = "Branch~" + BranchId;
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
            if (idlist[0] == "Company")
            {
                data = "Company~" + str1;
            }


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            string[] InputName = new string[11];
            string[] InputType = new string[11];
            string[] InputValue = new string[11];
            DataSet DsShow = new DataSet();
            /////////////////Parameter Name
            InputName[0] = "Date";
            InputName[1] = "Branch";
            InputName[2] = "GroupTypeCode";
            InputName[3] = "Segment";
            InputName[4] = "Company";
            InputName[5] = "Client";
            InputName[6] = "OnlyDrCrBoth";
            InputName[7] = "DrCrAmtLimit";
            InputName[8] = "SelectionMode";
            InputName[9] = "IfGroupTypeOrCode";
            InputName[10] = "ConsolidateBy";


            /////////////////Parameter Data Type
            InputType[0] = "D";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";

            //Parameter Values
            InputValue[0] = Convert.ToDateTime(dtDate.Value).ToString("MM-dd-yyyy");
            InputValue[1] = ddlGroup.SelectedValue == "0" && HdnBranchId.Value.Trim() != String.Empty && rdbranchSelected.Checked ? HdnBranchId.Value : HttpContext.Current.Session["userbranchHierarchy"].ToString();
            InputValue[2] = ddlGroup.SelectedValue == "1" && rdddlgrouptypeSelected.Checked ? HdnGroup.Value : ddlgrouptype.SelectedItem.Text.Trim();
            InputValue[3] = rdSelSegment.Checked && HdnSegment.Value.Trim() != String.Empty ? HdnSegment.Value.Trim() : "ALL";
            InputValue[4] = RdbAllCompany.Checked ? "ALL" : RdbSelectedCompany.Checked && hidden_Company.Value.Trim() != String.Empty ? hidden_Company.Value : Session["LastCompany"].ToString();
            InputValue[5] = rdbClientALL.Checked ? "ALL" : rdbClientSelected.Checked && HdnClients.Value.Trim() != String.Empty ? HdnClients.Value : "NoClientSelected";
            InputValue[6] = rdCredit.Checked ? "OC" : rdDebit.Checked ? "OD" : "B";
            InputValue[7] = txtDebitCredit.Text;
            InputValue[8] = ddlGroup.SelectedValue == "0" ? "BranchWise" : "GroupWise";
            InputValue[9] = ddlGroup.SelectedValue == "1" && rdddlgrouptypeSelected.Checked ? "GroupCode~" + ddlgrouptype.SelectedItem.Text.Trim() : "GroupType";
            InputValue[10] = ChkConsolidateBy.Checked ? ddlGroup.SelectedValue == "0" ? "B" : "G" : "";
            DsShow = SQLProcedures.SelectProcedureArrDS("SegmentWiseLedgerExposureMargin", InputName, InputType, InputValue);
            if (DsShow.Tables.Count > 0)
            {
                if (DsShow.Tables[0].Rows.Count > 0)
                {
                    Export(DsShow);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script>alert('No Record Found');Page_Load();</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script>alert('No Record Found');Page_Load();</script>");
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
        void Export(DataSet ds)
        {
            ExcelFile objExcel = new ExcelFile();
            DataTable dtExport = new DataTable();
            dtExport = ds.Tables[0].Copy();
            String CurrentDate = oDBEngine.GetDate(106);
            string str = null;
            //str = "Segment Wise Ledger Exposure Margin On " + CurrentDate.ToString("dd-MMM-yyyy");
            str = "Segment Wise Ledger Exposure Margin On " + CurrentDate;
            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlGroup.SelectedValue == "0")
                objExcel.ExportToExcelforExcel(dtExport, "SegmentWiseLedgerExposureMargin", "Branch Total", dtReportHeader, dtReportFooter);
            else
                objExcel.ExportToExcelforExcel(dtExport, "SegmentWiseLedgerExposureMargin", "Group Total", dtReportHeader, dtReportFooter);

        }


        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
        }
    }
}