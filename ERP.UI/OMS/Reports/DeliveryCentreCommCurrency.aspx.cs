using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ERP.OMS.Reports
{

    public partial class Reports_DeliveryCentreCommCurrency : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();

            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtTradeFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTradeTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtPayoutFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtPayoutTo.EditFormatString = oconverter.GetDateFormat("Date");

            DtTradeFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(-2).ToShortDateString());
            DtTradeTo.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(-2).ToShortDateString());
            DtPayoutFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(2).ToShortDateString());
            DtPayoutTo.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(2).ToShortDateString());
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
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
                    }
                }


                if (idlist[0] == "ScripCriteria")
                {
                    data = "ScripCriteria~" + str;
                }
                if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }

            }
        }
        void procedure()
        {

            string FromDate = string.Empty;
            string ToDate = string.Empty;
            string ForClient = string.Empty;
            string Clients = string.Empty;
            string SettNo = string.Empty;
            string SettType = string.Empty;
            string Product = string.Empty;
            string Movement = string.Empty;
            string Nature = string.Empty;
            string OrderByType = string.Empty;

            if (ddlsearchBy.SelectedItem.Value.ToString().Trim() == "TDATE")
            {
                FromDate = Convert.ToString(DtTradeFrom.Value);
                ToDate = Convert.ToString(DtTradeTo.Value);
            }
            else
            {
                FromDate = Convert.ToString(DtPayoutFrom.Value);
                ToDate = Convert.ToString(DtPayoutTo.Value);
            }
            if (radClient.Checked)
            {
                ForClient = "Client";
            }
            if (radExchange.Checked)
            {
                ForClient = "Exchange";
            }
            if (radBoth.Checked)
            {
                ForClient = "Both";
            }
            if (radAll.Checked)
            {
                Clients = "ALL";
            }
            if (radPOAClient.Checked)
            {
                Clients = "POACLIENT";
            }
            if (radSelected.Checked)
            {
                Clients = Convert.ToString(HiddenField_Client.Value);
            }
            if (rdSettNoALL.Checked)
            {
                SettNo = "All";
            }
            else
            {
                SettNo = Convert.ToString(txtSettNo_hidden.Text);
            }
            if (rdSettNoTypeALL.Checked)
            {
                SettType = "All";
            }
            else
            {
                SettType = Convert.ToString(txtSettType_hidden.Text);
            }
            if (rdbProductAll.Checked)
            {
                Product = "ALL";
            }
            if (rdbProductSelected.Checked)
            {
                Product = Convert.ToString(HiddenField_ScripCriteria.Value);
            }
            if (radIncoming.Checked)
            {
                Movement = "Incoming";
            }
            if (radOutgoing.Checked)
            {
                Movement = "Outgoing";
            }
            if (radMovBoth.Checked)
            {
                Movement = "Both";
            }
            if (radTransfered.Checked)
            {
                Nature = "Transfered";
            }
            if (radUntransfered.Checked)
            {
                Nature = "UnTransfered";
            }
            if (radNatBoth.Checked)
            {
                Nature = "Both";
            }
            if (rdOrderBy1ST.Checked)
            {
                OrderByType = "1ST";
            }
            if (rdOrderBy2ND.Checked)
            {
                OrderByType = "2ND";
            }
            ds = oReports.Report_DematCentreCommCurrency(
                    Convert.ToString(Session["usersegid"]),
                    Convert.ToString(Session["LastCompany"]),
                    Convert.ToString(Session["LastFinYear"]),
                    Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                    Convert.ToString(ddlType.SelectedItem.Value),
                    Convert.ToString(ddlsearchBy.SelectedItem.Value),
                    FromDate,
                    ToDate,
                    ForClient,
                    Clients,
                    SettNo,
                    SettType,
                    Product,
                    Movement,
                    Nature,
                    OrderByType

                );
            ViewState["dataset"] = ds;


        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            SpCall();

        }
        void SpCall()
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                if (ddlGeneration.SelectedItem.Text.ToString().Trim() == "Screen")
                {
                    if (ddlType.SelectedItem.Value.ToString().Trim() != "S")
                    {
                        GridBind_OtherWithOutStocks(ds);
                    }
                }
                else
                {
                    Export(ds);
                }
            }

        }
        void GridBind_OtherWithOutStocks(DataSet ds)
        {

            if (radIncoming.Checked)
            {
                grdDematCentre.Columns[9].Visible = false;
                grdDematCentre.Columns[10].Visible = false;
                grdDematCentre.Columns[12].Visible = false;
            }
            if (radOutgoing.Checked)
            {
                grdDematCentre.Columns[7].Visible = false;
                grdDematCentre.Columns[8].Visible = false;
                grdDematCentre.Columns[11].Visible = false;
            }
            if (radMovBoth.Checked)
            {

                grdDematCentre.Columns[7].Visible = true;
                grdDematCentre.Columns[8].Visible = true;
                grdDematCentre.Columns[9].Visible = true;
                grdDematCentre.Columns[10].Visible = true;
                grdDematCentre.Columns[11].Visible = true;
                grdDematCentre.Columns[12].Visible = true;
            }

            grdDematCentre.DataSource = ds;
            grdDematCentre.DataBind();

            ////////////Header Bind
            string str = null;
            if (ddlsearchBy.SelectedItem.Value.ToString().Trim() == "TDATE")
            {
                str = " Report Period [ " + ddlsearchBy.SelectedItem.Text.ToString().Trim() + " Wise ] " + oconverter.ArrangeDate2(DtTradeFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTradeTo.Value.ToString());
            }
            else
            {
                str = " Report Period [ " + ddlsearchBy.SelectedItem.Text.ToString().Trim() + " Wise ]" + oconverter.ArrangeDate2(DtPayoutFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtPayoutTo.Value.ToString());
            }
            str = "Type :" + ddlType.SelectedItem.Text.ToString().Trim() + str;

            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=\"left\" nowrap=\"nowrap;\" style=\"color:Blue;\">" + str + "</td></tr></table>";
            display.InnerHtml = strHtml;

            string idfetch = "grdDematCentre_ctl02_ddlSelectaType";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "displaygrid", "displaygrid('" + idfetch + "');", true);
        }
        protected void grdDematCentre_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            }

        }
        protected void grdDematCentre_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblColourType = (Label)row.FindControl("lblColourType");
                if (lblColourType.Text == "POA")
                {
                    e.Row.Cells[0].Style.Add("color", "green");
                    e.Row.Cells[0].Font.Bold = true;
                }
                else if (lblColourType.Text == "DP")
                {
                    e.Row.Cells[0].Style.Add("color", "blue");
                    e.Row.Cells[0].Font.Bold = true;
                }
            }
        }
        protected void ddlSelectaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ClientID1 = ((DropDownList)sender).ClientID;//grdDematCentre_ctl02__ddlSelectaType
            string[] RowNo1 = ClientID1.Split('_');
            string RowNo2 = RowNo1[1].ToString();
            int RowNo = Convert.ToInt32(RowNo2.Substring(3));
            int rowzero = Convert.ToInt32(RowNo2.Substring(3, 1));
            DropDownList ddlSelectaType = (DropDownList)grdDematCentre.Rows[RowNo - 2].FindControl("ddlSelectaType");
            Label lblproductid = (Label)grdDematCentre.Rows[RowNo - 2].FindControl("lblproductid");
            Label lblclientid = (Label)grdDematCentre.Rows[RowNo - 2].FindControl("lblclientid");
            Label lblSETTNO = (Label)grdDematCentre.Rows[RowNo - 2].FindControl("lblSETTNO");
            Label lblQUANTITY = (Label)grdDematCentre.Rows[RowNo - 2].FindControl("lblQUANTITY");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Fn_ddlSelectaType", "Fn_ddlSelectaType('" + ddlSelectaType.SelectedItem.Value + "','" + lblclientid.Text.ToString().Trim() + "','" + lblproductid.Text.ToString().Trim() + "','" + lblSETTNO.Text.ToString().Trim() + "','" + lblQUANTITY.Text.ToString().Trim() + "')", true);

        }
        protected void btncallback_Click(object sender, EventArgs e)
        {
            SpCall();
        }

        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            string str = null;
            if (ddlsearchBy.SelectedItem.Value.ToString().Trim() == "TDATE")
            {
                str = " Report Period [ " + ddlsearchBy.SelectedItem.Text.ToString().Trim() + " Wise ] " + oconverter.ArrangeDate2(DtTradeFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTradeTo.Value.ToString());
            }
            else
            {
                str = " Report Period [ " + ddlsearchBy.SelectedItem.Text.ToString().Trim() + " Wise ]" + oconverter.ArrangeDate2(DtPayoutFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtPayoutTo.Value.ToString());
            }
            str = "Type :" + ddlType.SelectedItem.Text.ToString().Trim() + str;

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

            objExcel.ExportToExcelforExcel(dtExport, "Delivery Centre", "Total:", dtReportHeader, dtReportFooter);

        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            SpCall();
        }
    }
}