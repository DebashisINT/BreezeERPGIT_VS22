using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_POAClients : System.Web.UI.Page
    {
        MasterReports mr = new MasterReports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>height();</script>");
                if (!IsPostBack)
                {
                    dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtto.EditFormatString = oconverter.GetDateFormat("Date");
                    dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                    dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                }
            }

        }
        void procedure()
        {
            ds = mr.POACLIENT(Session["LastCompany"].ToString().Trim(), Session["usersegid"].ToString().Trim(), dtfrom.Value.ToString(), dtto.Value.ToString(), Session["userbranchHierarchy"].ToString().Trim());

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                htmltable();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }

            // }
        }
        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Sr No.</b></td>";
            strHtml += "<td align=\"center\" ><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" ><b>Code</b></td>";
            strHtml += "<td align=\"center\" ><b>DP Details</b></td>";
            strHtml += "<td align=\"center\" ><b>POA Name</b></td>";
            strHtml += "<td align=\"center\"><b>Pan</b></td>";
            strHtml += "<td align=\"center\" ><b>Address</b></td>";
            strHtml += "</tr>";

            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Sr No.", Type.GetType("System.String"));
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Code", Type.GetType("System.String"));
            dtExport.Columns.Add("DP Details", Type.GetType("System.String"));
            dtExport.Columns.Add("POA Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Pan", Type.GetType("System.String"));
            dtExport.Columns.Add("Address", Type.GetType("System.String"));

            int flag = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                DataRow row = dtExport.NewRow();

                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["SrNo"].ToString() + "</td>";
                row[0] = ds.Tables[0].Rows[i]["SrNo"].ToString();

                if (ds.Tables[0].Rows[i]["ClientName"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</td>";
                    row[1] = ds.Tables[0].Rows[i]["ClientName"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["CNT_UCC"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["CNT_UCC"].ToString() + "</td>";
                    row[2] = ds.Tables[0].Rows[i]["CNT_UCC"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";



                if (ds.Tables[0].Rows[i]["dpdetalis"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["dpdetalis"].ToString() + "</td>";
                    row[3] = ds.Tables[0].Rows[i]["dpdetalis"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                if (ds.Tables[0].Rows[i]["DPD_POANAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["DPD_POANAME"].ToString() + "</td>";
                    row[4] = ds.Tables[0].Rows[i]["DPD_POANAME"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["PAN"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["PAN"].ToString() + "</td>";
                    row[5] = ds.Tables[0].Rows[i]["PAN"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["ADDRESS"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["ADDRESS"].ToString() + "</td>";
                    row[6] = ds.Tables[0].Rows[i]["ADDRESS"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "</tr>";
                dtExport.Rows.Add(row);
            }
            strHtml += "</table>";
            ViewState["dtExport"] = dtExport;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);

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
            dtExport = (DataTable)ViewState["dtExport"];
            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            DrRowR1[0] = "POA Clients";

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
            dtReportHeader.Rows.Add(DrRowR2);
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

            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "POA Clients Report", "Client Name", dtReportHeader, dtReportFooter);
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
        }
    }
}