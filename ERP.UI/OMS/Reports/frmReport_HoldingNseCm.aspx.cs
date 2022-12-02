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
    public partial class Reports_frmReport_HoldingNseCm : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        string data;

        public string[] InputName = new string[1];
        public string[] InputType = new string[1];
        public string[] InputValue = new string[1];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                DtHoldingDate.EditFormatString = oconverter.GetDateFormat("Date");
                DtHoldingDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                AccountFetch();
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//


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
                    str = val[0];
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += "," + val[0];
                    str1 += "," + val[0] + ";" + val[1];
                }


            }

            if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void AccountFetch()
        {
            InputName[0] = "Criteria";
            InputType[0] = "V";

            InputValue[0] = " Where rtrim(DPACCounts_AccountType)<>'[SYSTM]' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' and rtrim(DPACCounts_AccountType) in ('[POOL]','[CISA]','[PLPAYOUT]','[PLPAYIN]') and DPACCounts_ExchangeSegmentID='" + Session["usersegid"].ToString().Trim() + "'";
            DataTable DtDpAc = new DataTable();

            DtDpAc = SQLProcedures.SelectProcedureArr("[Fetch_AccountName]", InputName, InputType, InputValue);

            InputValue[0] = " Where rtrim(DPACCounts_AccountType)<>'[SYSTM]' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' and rtrim(DPACCounts_AccountType) in ('[HOLDBK]','[MRGIN]','[Own]')";

            DataTable DtDpAc1 = new DataTable();
            DtDpAc1 = SQLProcedures.SelectProcedureArr("[Fetch_AccountName]", InputName, InputType, InputValue);

            DtDpAc.Merge(DtDpAc1);

            ddlDpAc.DataTextField = "ShortName";
            ddlDpAc.DataValueField = "ID";
            ddlDpAc.DataSource = DtDpAc;
            ddlDpAc.DataBind();

            ddlDpAc.Items.Insert(0, new ListItem("All", "All"));
        }



        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            string DpAc = ddlDpAc.SelectedItem.Value;
            string[] DAc = DpAc.Split('~');
            Procedure(DAc[0].ToString().Trim());

        }
        void Procedure(string accountid)
        {
            ds = rep.Report_Holding(Session["LastCompany"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString().Trim(),
                DtHoldingDate.Value.ToString(), accountid.ToString().Trim(), rdbProductAll.Checked ? "All" : HiddenField_Product.Value,
                RdSettNoAll.Checked ? "All" : txtSettlement_hidden.Text.ToString().Trim(), ChkHldingValue.Checked ? "Chk" : "UnChk",
                DdlSecurityType.SelectedItem.Value.ToString().Trim());
            ViewState["dataset"] = ds;
            FnGeneRationCall(ds);

        }
        void FnGeneRationCall(DataSet ds)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                FnHtml(ds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('3');", true);
            }
        }
        void FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;

            str = " As On Date : " + oconverter.ArrangeDate2(DtHoldingDate.Value.ToString());
            str = str + " ; DP Account : " + ddlDpAc.SelectedItem.Text.ToString().Trim();

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
                        if (dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Grand"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
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
            DivRecord.InnerHtml = strHtml;

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            str = " As On Date : " + oconverter.ArrangeDate2(DtHoldingDate.Value.ToString());
            str = str + " ; DP Account : " + ddlDpAc.SelectedItem.Text.ToString().Trim();


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

            objExcel.ExportToExcelforExcel(dtExport, "Demat Holding Report", "Total:", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }

    }
}