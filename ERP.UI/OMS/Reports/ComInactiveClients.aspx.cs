using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;


namespace ERP.OMS.Reports
{
    public partial class Reports_ComInactiveClients : System.Web.UI.Page
    {
        MISReports mis = new MISReports();
        DataTable dtClients = new DataTable();
        DataTable dt = new DataTable();
        DataTable Distinctgroup = new DataTable();
        DataTable dt1 = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        string data;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        int pageindex = 0;
        DataTable dtgrp = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        string TradingAmount;
        string MarginAmount;
        string footerTxt = "";
        string BranchName = null;
        DateTime Date;
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

            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                settno();
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


        void procedure()
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //        SqlCommand cmd = new SqlCommand();
            //        cmd.Connection = con;
            //        cmd.CommandText = "[Report_ComMISReport_Inactive]";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        if (ddlActive.SelectedValue =="0")
            //        {
            //            cmd.Parameters.AddWithValue("@days", txtNo.Text);
            //        }
            //        else if (ddlActive.SelectedValue == "1")
            //        {
            //            cmd.Parameters.AddWithValue("@days", 30* Convert.ToInt32(txtNo.Text));
            //        }
            //        else if (ddlActive.SelectedValue == "2")
            //        {
            //            cmd.Parameters.AddWithValue("@days", 365 * Convert.ToInt32(txtNo.Text));
            //        }
            //        if (rdbSegAll.Checked == true)
            //        {
            //            cmd.Parameters.AddWithValue("@segment", "All");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
            //        }
            //        cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
            //        SqlDataAdapter da = new SqlDataAdapter();
            //        da.SelectCommand = cmd;
            //        cmd.CommandTimeout = 0;
            //        ds.Reset();
            //        da.Fill(ds);
            //        da.Dispose();
            //        ViewState["dataset"] = ds;

            //}
            int days = 0;
            if (ddlActive.SelectedValue == "0")
            {
                days = Convert.ToInt32(txtNo.Text);
            }
            else if (ddlActive.SelectedValue == "1")
            {
                days = 30 * Convert.ToInt32(txtNo.Text);
            }
            else if (ddlActive.SelectedValue == "2")
            {
                days = 365 * Convert.ToInt32(txtNo.Text);
            }
            ds = mis.Report_ComMISReport_Inactive(days, rdbSegAll.Checked == true ? "All" : Session["usersegid"].ToString(), Session["LastCompany"].ToString());
            ViewState["dataset"] = ds;
        }


        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }



        void export()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();

            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Ucc", Type.GetType("System.String"));
            dtExport.Columns.Add("Last Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Trading Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Margin Amount", Type.GetType("System.String"));

            string branchname = null;
            string TradingAmount = null;
            string MarginAmount = null;
            int i = 0;

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (branchname != ds.Tables[0].Rows[i]["branchname"].ToString().Trim())
                {
                    if (i != 0)
                    {
                        DataRow rowbranch1 = dtExport.NewRow();
                        rowbranch1[0] = "Branch Total :";
                        rowbranch1[3] = ds.Tables[0].Rows[i - 1]["SumTradingAmount"].ToString();
                        rowbranch1[4] = ds.Tables[0].Rows[i - 1]["SumMarginAmount"].ToString();
                        dtExport.Rows.Add(rowbranch1);
                    }
                    DataRow rowbranch = dtExport.NewRow();
                    rowbranch[0] = "Branch Name : " + ds.Tables[0].Rows[i]["BranchName"].ToString();
                    rowbranch[1] = "Test";
                    dtExport.Rows.Add(rowbranch);
                }
                branchname = ds.Tables[0].Rows[i]["branchname"].ToString().Trim();

                DataRow row2 = dtExport.NewRow();

                row2["Client Name"] = ds.Tables[0].Rows[i]["ClientName"].ToString();
                row2["Ucc"] = ds.Tables[0].Rows[i]["Ucc"].ToString();

                if (ds.Tables[0].Rows[i]["Date"] != DBNull.Value)
                {
                    row2["Last Date"] = oconverter.ArrangeDate2(ds.Tables[0].Rows[i]["Date"].ToString());
                }

                if (ds.Tables[0].Rows[i]["TradingAmount"] != DBNull.Value)
                {
                    row2["Trading Amount"] = ds.Tables[0].Rows[i]["TradingAmount"].ToString();
                }

                if (ds.Tables[0].Rows[i]["MarginAmount"] != DBNull.Value)
                {
                    row2["Margin Amount"] = ds.Tables[0].Rows[i]["MarginAmount"].ToString();
                }

                dtExport.Rows.Add(row2);

            }

            /////Branch Total
            DataRow rowbranch2 = dtExport.NewRow();
            rowbranch2[0] = "Branch Total :";
            rowbranch2[3] = ds.Tables[0].Rows[i - 1]["SumTradingAmount"].ToString();
            rowbranch2[4] = ds.Tables[0].Rows[i - 1]["SumMarginAmount"].ToString();
            dtExport.Rows.Add(rowbranch2);

            ////Blank Row
            DataRow rowbranch3 = dtExport.NewRow();
            dtExport.Rows.Add(rowbranch3);

            /////Grand Total

            DataRow row5 = dtExport.NewRow();
            row5["Client Name"] = "Grand Total";
            row5["Trading Amount"] = ds.Tables[0].Rows[0]["GrandTradingAmount"];
            row5["Margin Amount"] = ds.Tables[0].Rows[0]["GrandMarginAmount"];
            dtExport.Rows.Add(row5);


            //////////////////////////////////SEGMENTNAME FETCH///////////////////////////////////////

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = " Clients Inactive For Last : " + txtNo.Text + " " + ddlActive.SelectedItem.Text + " " + " As On " + oconverter.ArrangeDate2(oDbEngine.GetDate().ToString());
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
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
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            objExcel.ExportToExcelforExcel(dtExport, "Inactive Clients Report", "Branch Total :", dtReportHeader, dtReportFooter);


        }


        protected void btnexport_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                export();
            }

        }



        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);

            }
            else
            {
                html();
            }
        }

        void html()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #FFF;\">";
            strHtml += "<td align=\"center\" colspan=5><B>" + " Clients Inactive For Last : " + txtNo.Text + " " + ddlActive.SelectedItem.Text + " As On " + oconverter.ArrangeDate2(oDbEngine.GetDate().ToString()) + "</B></td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>&nbsp;</td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #DBEEE4;\">";
            strHtml += "<td align=\"center\" ><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" ><b>UCC</b></td>";
            strHtml += "<td align=\"center\"><b>Last Date </b></td>";
            strHtml += "<td align=\"center\"><b>Trading Amount. </b></td>";
            strHtml += "<td align=\"center\"><b>Margin Amount. </b></td>";
            strHtml += "</tr>";
            int flag = 0;


            int i;
            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    ///////////////////////ALL CLIENT 

                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    if (BranchName != ds.Tables[0].Rows[i]["BranchName"].ToString().Trim())
                    {
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" ><b>" + " Branch Name : " + ds.Tables[0].Rows[i]["BranchName"].ToString() + "</b></td>";
                        strHtml += "<td>&nbsp;</td>";
                        strHtml += "<td>&nbsp;</td>";
                        strHtml += "<td align=\"right\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["SumTradingAmount"].ToString() + "</b></td>";
                        strHtml += "<td align=\"right\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["SumMarginAmount"].ToString() + "</b></td>";
                        strHtml += "</tr>";
                        flag = flag + 1;

                    }
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    BranchName = ds.Tables[0].Rows[i]["BranchName"].ToString().Trim();
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["UCC"].ToString() + " </td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["Date"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["TradingAmount"].ToString() + " </td>";
                    if (dt1.Rows[i]["MarginAmount"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt1.Rows[i]["MarginAmount"].ToString() + "</b></td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                }
                ///////////////////////GRAND TOTAL


            }
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\"><B>" + " Grand Total : " + "</B></td>";
            strHtml += "<td>&nbsp;</td>";
            strHtml += "<td>&nbsp;</td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["GrandTradingAmount"].ToString() + "</B> </td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["GrandMarginAmount"].ToString() + "</B> </td>";
            strHtml += "</tr></table>";

            displayALL.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);

        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();

        }
    }
}
