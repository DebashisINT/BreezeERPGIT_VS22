using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_ActiveClient_NSDL : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();

        string data;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                settno();
                dtFrom.Value = DateTime.Today.AddDays(-30);
                dtTo.Value = DateTime.Today;
            }
        }
        void settno()
        {
            DataTable DtSegComp = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegment.InnerText = DtSegComp.Rows[0][2].ToString(); ///Segment disply within braket
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();

            }

        }

        protected void FetchValue()
        {
            string Segment = string.Empty;
            string RecordNo = string.Empty;
            string Percentage = string.Empty;
            if (rdbSegAll.Checked == true)
                Segment = "ALL";
            else if (rdbSegSelected.Checked == true)
                Segment = Convert.ToString(litSegment.InnerText);

            if (txtNo.Text.Trim() == "")
                RecordNo = "10";
            else
                RecordNo = Convert.ToString(txtNo.Text.Trim());

            if (txtPercentage.Text.Trim() == "")
                Percentage = "80";
            else
                Percentage = Convert.ToString(txtPercentage.Text.Trim());

            try
            {
                ds = oReports.Report_ActiveClientsNSDL(
                 Segment,
                 RecordNo,
                 Percentage,
                 Convert.ToString(dtFrom.Value),
                    Convert.ToString(dtTo.Value)

                 );
                ViewState["alldata"] = ds;

            }
            catch (Exception ex)
            { throw ex; }

        }
        protected void ShowData()
        {
            String strHtml = String.Empty;

            ds = (DataSet)ViewState["alldata"];
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #D2B9D3;\">";
            strHtml += "<td align=\"center\" colspan=5><B>" + txtNo.Text + "  Most Active Clients During The Period  " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</B></td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #FFF;\">";
            strHtml += "<td align=\"left\" colspan=5><B>" + "Total WorkingDays during the selected period :  " + ds.Tables[0].Rows[0]["WorkingDays"] + "</B></td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            //// strHtml += "<td align=\"center\" ><b>Sr.No</b></td>";
            strHtml += "<td align=\"center\" ><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" ><b>BenAccount</b></td>";
            strHtml += "<td align=\"center\"><b>Branch Name</b></td>";
            strHtml += "<td align=\"center\"><b>Transaction Days</b></td>";
            strHtml += "<td align=\"center\"><b>Transaction Rate(%)</b></td>";
            strHtml += "</tr>";
            int flag = 0;
            //ALL SEGMENT//
            DataTable dtNsdl = new DataTable();
            DataTable dtCdsl = new DataTable();
            ////DataView dv = new DataView();

            if (rdbSegAll.Checked == true)
            {
                dtNsdl = ds.Tables[0];
                dtCdsl = ds.Tables[1];
                dtNsdl.Merge(dtCdsl);

                ////dv = dtNsdl.DefaultView;


            }
            else
            {
                dtNsdl = ds.Tables[0];
                ////dv = ds.Tables[0].DefaultView;
            }
            //ALL //
            //NSDL/CDSL ONLY
            ////dv.Sort = "transactionrate desc";
            ////DataTable dt1 = new DataTable();
            DataRow[] arrdr;
            //dv.RowFilter = "transactionrate > '70'";// +txtPercentage.Text.Trim();
            ////dt1 = dv.Table;
            arrdr = dtNsdl.Select("", "transactionrate desc");
            if (arrdr.Length > 0)
            {
                for (int i = 0; i < arrdr.Length; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    //// strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["Sr.No"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + arrdr[i].ItemArray[3].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + arrdr[i].ItemArray[2].ToString() + " </td>";
                    strHtml += "<td align=\"left\">" + arrdr[i].ItemArray[4].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + arrdr[i].ItemArray[1].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + arrdr[i].ItemArray[0].ToString() + " </td>";
                    if (flag == Convert.ToInt32(txtNo.Text.Trim()))
                        break;


                }

                ////for (int i = 0; i < dv.Count; i++)
                ////{
                ////    flag = flag + 1;
                ////    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                ////    //// strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["Sr.No"].ToString() + "</td>";
                ////    strHtml += "<td align=\"left\">" + dv.Table.Rows[i]["clientname"].ToString() + "</td>";
                ////    strHtml += "<td align=\"left\">" + dv.Table.Rows[i]["ucc"].ToString() + " </td>";
                ////    strHtml += "<td align=\"left\">" + dv.Table.Rows[i]["branchname"].ToString() + " </td>";
                ////    strHtml += "<td align=\"right\">" + dv.Table.Rows[i]["transactiondays"].ToString() + " </td>";
                ////    strHtml += "<td align=\"right\">" + dv.Table.Rows[i]["transactionrate"].ToString() + " </td>";
                ////    if (flag > 9)
                ////        break;


                ////}

                displayALL.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "linehtml", "line(0);", true);
            }

            ////if (ds.Tables[0].Rows.Count > 0)
            ////{

            ////    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            ////    {
            ////        flag = flag + 1;
            ////        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            ////        //// strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["Sr.No"].ToString() + "</td>";
            ////        strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["clientname"].ToString() + "</td>";
            ////        strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["ucc"].ToString() + " </td>";
            ////        strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["branchname"].ToString() + " </td>";
            ////        strHtml += "<td align=\"right\">" + ds.Tables[0].Rows[i]["transactiondays"].ToString() + " </td>";
            ////        strHtml += "<td align=\"right\">" + ds.Tables[0].Rows[i]["transactionrate"].ToString() + " </td>";



            ////    }

            ////    displayALL.InnerHtml = strHtml;
            ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "linehtml", "line(0);", true);
            ////}
            //NSDL ONLY
        }

        protected void Export()
        {
            int flag = 0;
            ds = (DataSet)ViewState["alldata"];

            DataTable dt = new DataTable();
            dt.Columns.Add("Client Name", Type.GetType("System.String"));
            dt.Columns.Add("UCC", Type.GetType("System.String"));
            dt.Columns.Add("Branch Name", Type.GetType("System.String"));
            dt.Columns.Add("Transaction Days", Type.GetType("System.String"));
            dt.Columns.Add("Transaction Rate(%)", Type.GetType("System.String"));

            //Both Segment
            DataTable dtNsdl = new DataTable();
            DataTable dtCdsl = new DataTable();
            ////DataView dv = new DataView();

            if (rdbSegAll.Checked == true)
            {
                dtNsdl = ds.Tables[0];
                dtCdsl = ds.Tables[1];
                dtNsdl.Merge(dtCdsl);

                ////dv.Table = dtNsdl;
                ////dv.Sort = "transactionrate desc";

            }
            else
            {
                dtNsdl = ds.Tables[0];
                ////dv.Table = ds.Tables[0];
            }

            //Both Segment
            DataRow[] arrdr;
            arrdr = dtNsdl.Select("", "transactionrate desc");
            if (arrdr.Length > 0)
            {
                for (int i = 0; i < arrdr.Length; i++)
                {
                    flag = flag + 1;
                    DataRow dr = dt.NewRow();
                    if (arrdr[i].ItemArray[3] != DBNull.Value)
                        dr["Client Name"] = arrdr[i].ItemArray[3].ToString();

                    if (arrdr[i].ItemArray[2] != DBNull.Value)
                        dr["UCC"] = arrdr[i].ItemArray[2].ToString();

                    if (arrdr[i].ItemArray[4] != DBNull.Value)
                        dr["Branch Name"] = arrdr[i].ItemArray[4].ToString();

                    if (arrdr[i].ItemArray[1] != DBNull.Value)
                        dr["Transaction Days"] = arrdr[i].ItemArray[1].ToString();

                    if (arrdr[i].ItemArray[0] != DBNull.Value)
                        dr["Transaction Rate(%)"] = arrdr[i].ItemArray[0].ToString();

                    dt.Rows.Add(dr);
                    if (flag == Convert.ToInt32(txtNo.Text.Trim()))
                        break;
                }


                ////for (int i = 0; i < dv.Table.Rows.Count; i++)
                ////{

                ////    DataRow dr = dt.NewRow();
                ////    if (dv.Table.Rows[i]["clientname"] != DBNull.Value)
                ////        dr["Client Name"] = dv.Table.Rows[i]["clientname"].ToString();

                ////    if (dv.Table.Rows[i]["ucc"] != DBNull.Value)
                ////        dr["UCC"] = dv.Table.Rows[i]["ucc"].ToString();

                ////    if (dv.Table.Rows[i]["branchname"] != DBNull.Value)
                ////        dr["Branch Name"] = dv.Table.Rows[i]["branchname"].ToString();

                ////    if (dv.Table.Rows[i]["transactiondays"] != DBNull.Value)
                ////        dr["Transaction Days"] = dv.Table.Rows[i]["transactiondays"].ToString();

                ////    if (dv.Table.Rows[i]["transactionrate"] != DBNull.Value)
                ////        dr["Transaction Rate(%)"] = dv.Table.Rows[i]["transactionrate"].ToString();

                ////    dt.Rows.Add(dr);

                ////}



                ////for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                ////{

                ////    DataRow dr = dt.NewRow();
                ////    if (ds.Tables[0].Rows[i]["clientname"] != DBNull.Value)
                ////        dr["Client Name"] = ds.Tables[0].Rows[i]["clientname"].ToString();

                ////    if (ds.Tables[0].Rows[i]["ucc"] != DBNull.Value)
                ////        dr["UCC"] = ds.Tables[0].Rows[i]["ucc"].ToString();

                ////    if (ds.Tables[0].Rows[i]["branchname"] != DBNull.Value)
                ////        dr["Branch Name"] = ds.Tables[0].Rows[i]["branchname"].ToString();

                ////    if (ds.Tables[0].Rows[i]["transactiondays"] != DBNull.Value)
                ////        dr["Transaction Days"] = ds.Tables[0].Rows[i]["transactiondays"].ToString();

                ////    if (ds.Tables[0].Rows[i]["transactionrate"] != DBNull.Value)
                ////        dr["Transaction Rate(%)"] = ds.Tables[0].Rows[i]["transactionrate"].ToString();

                ////    dt.Rows.Add(dr);

                ////}


                DataTable dtReportHeader1 = new DataTable();
                DataTable dtReportFooter1 = new DataTable();

                DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                dtReportHeader1.Columns.Add(new DataColumn("Header", typeof(String))); //0

                DataRow HeaderRow = dtReportHeader1.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader1.Rows.Add(HeaderRow);

                DataRow DrRowR1 = dtReportHeader1.NewRow();
                string str = null;
                str = ViewState["NoOfClients"] + " Most Active Clients For The Period : " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                DrRowR1[0] = str;
                dtReportHeader1.Rows.Add(DrRowR1);

                DataRow DrRowR2 = dtReportHeader1.NewRow();
                DrRowR2[0] = "Total Working Days During the Selected Period is : " + ds.Tables[0].Rows[0]["WorkingDays"].ToString();
                dtReportHeader1.Rows.Add(DrRowR2);



                dtReportFooter1.Columns.Add(new DataColumn("Footer", typeof(String)));
                DataRow FooterRow1 = dtReportFooter1.NewRow();
                dtReportFooter1.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter1.NewRow();
                dtReportFooter1.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter1.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter1.Rows.Add(FooterRow);
                objExcel.ExportToExcelforExcel(dt, "Active Clients", "Active Clients", dtReportHeader1, dtReportFooter1);


            }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            FetchValue();
            ds = (DataSet)ViewState["alldata"];
            if (ds.Tables.Count > 1 || ds.Tables[0].Rows.Count > 0)
            {
                ShowData();
            }

            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(0);", true);

        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            FetchValue();
            ds = (DataSet)ViewState["alldata"];
            if (ds.Tables.Count > 1 || ds.Tables[0].Rows.Count > 0)
            {
                Export();
            }

            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(0);", true);

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Export();
        }
    }
}