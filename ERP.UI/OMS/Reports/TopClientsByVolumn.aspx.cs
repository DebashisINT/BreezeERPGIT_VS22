using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ClientsByVolumn : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable DT = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        MISReports misrep = new MISReports();
        string data;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
             //   Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                settno();

                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.Value = oDBEngine.GetDate().AddMonths(-1);

                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.Value = System.DateTime.Today;
            }

        }
        void settno()
        {
            DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegment.InnerText = DtSegComp.Rows[0][2].ToString(); ///Segment disply within braket
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();

            }

        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void procedure()
        {
            ds = misrep.Report_MISReport("TopClientsByVolumn", txtNo.Text, dtFrom.Value.ToString(), dtTo.Value.ToString(),
                rdbSegAll.Checked == true ? "All" : Session["usersegid"].ToString(), Session["LastCompany"].ToString(),
                HttpContext.Current.Session["LastFinYear"].ToString(), Session["userbranchHierarchy"].ToString());
            ViewState["dataset"] = ds;
            ViewState["NoOfClients"] = txtNo.Text;
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {

            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                String strHtml = String.Empty;
                String strHtml1 = String.Empty;
                int colcount = ds.Tables[0].Columns.Count;
                string str = null;
                DataView viewclient = new DataView();
                viewclient = ds.Tables[0].DefaultView;
                DataTable dt1 = new DataTable();
                dt1 = viewclient.ToTable();


                strHtml += "<tr style=\"background-color: #D2B9D3;\">";
                strHtml += "<td align=\"center\" colspan=6><B>" + "Top " + txtNo.Text + " Clients For The Period : " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</B></td>";
                strHtml += "</tr>";
                strHtml += "<tr>";
                strHtml += "<td></td>";
                strHtml += "</tr>";
                strHtml += "<tr style=\"background-color: #FFF;\">";
                strHtml += "<td align=\"left\" colspan=6><B>" + "Total Market Value During The Period :  " + dt1.Rows[0]["TotalValue"].ToString() + "</B></td>";
                strHtml += "</tr>";
                strHtml += "<tr style=\"background-color: #DBEEE5;\">";
                strHtml += "<td align=\"center\" ><b>Sr.No</b></td>";
                strHtml += "<td align=\"center\" ><b>Client Name</b></td>";
                strHtml += "<td align=\"center\" ><b>UCC</b></td>";
                strHtml += "<td align=\"center\"><b>Branch </b></td>";
                strHtml += "<td align=\"center\"><b>Value</b></td>";
                strHtml += "<td align=\"center\"><b>Ratio (%)</b></td>";
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
                        strHtml += "<td align=\"left\">" + dt1.Rows[i]["Sr.No"].ToString() + "</td>";
                        strHtml += "<td align=\"left\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                        strHtml += "<td align=\"left\">" + dt1.Rows[i]["UCC"].ToString() + " </td>";
                        strHtml += "<td align=\"left\">" + dt1.Rows[i]["BranchCode"].ToString() + " </td>";
                        strHtml += "<td align=\"right\">" + dt1.Rows[i]["Value"].ToString() + " </td>";
                        strHtml += "<td align=\"right\">" + dt1.Rows[i]["Ratio"].ToString() + " </td>";
                    }
                    ///////////////////////GRAND TOTAL

                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td></td>";
                    strHtml += "<td align=\"left\" colspan=2><B>" + "Total : " + "</B></td>";
                    strHtml += "<td></td>";
                    strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["SumValue"].ToString() + "</B> </td>";
                    strHtml += "</tr>";

                    displayALL.InnerHtml = strHtml;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);
                }
            }
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            procedure();
            export();
        }
        protected void export()
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                ds = (DataSet)ViewState["dataset"];
                DataTable dtExport1 = new DataTable();
                dtExport1.Columns.Add("Sr.No", Type.GetType("System.String"));
                dtExport1.Columns.Add("Client Name", Type.GetType("System.String"));
                dtExport1.Columns.Add("Ucc", Type.GetType("System.String"));
                dtExport1.Columns.Add("Branch", Type.GetType("System.String"));
                dtExport1.Columns.Add("Value", Type.GetType("System.String"));
                dtExport1.Columns.Add("Ratio (%)", Type.GetType("System.String"));

                dtExport1.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dtExport1.NewRow();
                        row["Sr.No"] = ds.Tables[0].Rows[i]["Sr.No"].ToString();
                        if (ds.Tables[0].Rows[i]["ClientName"] != DBNull.Value)
                        {
                            row["Client Name"] = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        }


                        if (ds.Tables[0].Rows[i]["Ucc"] != DBNull.Value)
                        {
                            row["Ucc"] = ds.Tables[0].Rows[i]["Ucc"].ToString();
                        }

                        if (ds.Tables[0].Rows[i]["BranchCode"] != DBNull.Value)
                        {
                            row["Branch"] = ds.Tables[0].Rows[i]["BranchCode"].ToString();
                        }

                        if (ds.Tables[0].Rows[i]["Value"] != DBNull.Value)
                        {
                            row["Value"] = ds.Tables[0].Rows[i]["Value"].ToString();
                        }


                        if (ds.Tables[0].Rows[i]["Ratio"] != DBNull.Value)
                        {
                            row["Ratio (%)"] = ds.Tables[0].Rows[i]["Ratio"].ToString();
                        }
                        dtExport1.Rows.Add(row);

                    }
                    DataRow row1 = dtExport1.NewRow();
                    row1["Client Name"] = "Total : ";

                    if (ds.Tables[0].Rows[0]["SumValue"] != DBNull.Value)
                    {
                        row1["Value"] = ds.Tables[0].Rows[0]["SumValue"].ToString();

                    }

                    dtExport1.Rows.Add(row1);


                    DataTable dtReportHeader1 = new DataTable();
                    DataTable dtReportFooter1 = new DataTable();

                    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                    dtReportHeader1.Columns.Add(new DataColumn("Header", typeof(String))); //0
                    DataRow HeaderRow = dtReportHeader1.NewRow();
                    HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                    dtReportHeader1.Rows.Add(HeaderRow);
                    DataRow DrRowR1 = dtReportHeader1.NewRow();

                    string str = null;

                    str = " Top " + ViewState["NoOfClients"] + "  Clients By Volumn " + " For Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

                    DrRowR1[0] = str; ;

                    dtReportHeader1.Rows.Add(DrRowR1);
                    DataRow DrRowR2 = dtReportHeader1.NewRow();
                    DrRowR2[0] = "Total Market Value During The Period :  " + ds.Tables[0].Rows[0]["TotalValue"].ToString();
                    dtReportHeader1.Rows.Add(DrRowR2);

                    DataRow HeaderRow1 = dtReportHeader1.NewRow();
                    dtReportHeader1.Rows.Add(HeaderRow1);
                    DataRow HeaderRow2 = dtReportHeader1.NewRow();
                    dtReportHeader1.Rows.Add(HeaderRow2);

                    dtReportFooter1.Columns.Add(new DataColumn("Footer", typeof(String)));
                    DataRow FooterRow1 = dtReportFooter1.NewRow();
                    dtReportFooter1.Rows.Add(FooterRow1);
                    DataRow FooterRow2 = dtReportFooter1.NewRow();
                    dtReportFooter1.Rows.Add(FooterRow2);
                    DataRow FooterRow = dtReportFooter1.NewRow();
                    FooterRow[0] = "* * *  End Of Report * * *   ";
                    dtReportFooter1.Rows.Add(FooterRow);

                    objExcel.ExportToExcelforExcel(dtExport1, "Top Clients By Volumn", "Total : ", dtReportHeader1, dtReportFooter1);
                }
            }

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }
    }
}