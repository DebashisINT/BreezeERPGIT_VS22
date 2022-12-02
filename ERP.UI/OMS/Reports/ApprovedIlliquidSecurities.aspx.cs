using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ApprovedIlliquidSecurities : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        MasterReports mr = new MasterReports();

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
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                YearBind();
                DtDate.EditFormatString = oconverter.GetDateFormat("Date");
                DtDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                ddlMonth.SelectedValue = oDBEngine.GetDate().Month.ToString().Trim();
            }


        }
        void YearBind()
        {
            int GetYear = System.DateTime.Today.Year;
            for (int year = 0; year < 7; year++)
            {
                ListItem li = new ListItem();
                // if (year == 1)
                //{
                //    li.Text = Convert.ToString(GetYear);
                //    li.Value = Convert.ToString(GetYear);
                //}
                //else
                //{
                li.Text = Convert.ToString(GetYear - year);
                li.Value = Convert.ToString(GetYear - year);

                //}
                DdlYear.Items.Add(li);
            }
            DdlYear.SelectedValue = GetYear.ToString().Trim();
        }
        void Procedure()
        {
            ds = mr.Report_ApprovedIlliquidSecurities(DtDate.Value.ToString(), DdlYear.SelectedItem.Value.ToString().Trim(), ddlMonth.SelectedItem.Value.ToString().Trim(),
                DdlRptView.SelectedItem.Value.ToString().Trim());
            ViewState["dataset"] = ds;
            FnCall(ds);
        }
        void FnCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                {
                    FnHtml(ds);
                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Export")
                {
                    Export(ds);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }
        }
        void FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            str = " Report View : " + DdlRptView.SelectedItem.Text.ToString().Trim();

            if (DdlRptView.SelectedItem.Value.ToString().Trim() == "Approved")
            {
                str = str + " ; As On Date " + oconverter.ArrangeDate2(DtDate.Value.ToString());
            }
            else
            {
                str = str + " ; Year " + DdlYear.SelectedItem.Text.ToString().Trim() + "; Month " + ddlMonth.SelectedItem.Text.ToString().Trim();
            }


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('3');", true);


        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            Procedure();

        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            str = " Report View : " + DdlRptView.SelectedItem.Text.ToString().Trim();

            if (DdlRptView.SelectedItem.Value.ToString().Trim() == "Approved")
            {
                str = str + " ; As On Date " + oconverter.ArrangeDate2(DtDate.Value.ToString());
            }
            else
            {
                str = str + " ; Year " + DdlYear.SelectedItem.Text.ToString().Trim() + "; Month " + ddlMonth.SelectedItem.Text.ToString().Trim();
            }

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

            if (dtExport.Columns.Count < 4)
            {
                dtExport.Columns.Add("Remarks");
            }
            objExcel.ExportToExcelforExcel(dtExport, DdlRptView.SelectedItem.Text.ToString().Trim(), "Total: ", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
    }
}
