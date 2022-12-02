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
    public partial class Reports_NetPositionUnprocessed : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
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
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                date();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

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
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "Broker")
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
                else if (idlist[0].ToString().Trim() == "UserID")
                {
                    string[] val = cl[i].Split(';');

                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = "'" + val[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                    }
                }
                else if (idlist[0].ToString().Trim() == "TradeCode")
                {
                    string[] val = cl[i].Split(';');

                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = "'" + val[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                    }
                }
                else
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

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Broker")
            {
                data = "Broker~" + str;
            }
            else if (idlist[0] == "ScripsExchange")
            {
                data = "ScripsExchange~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
            }
            else if (idlist[0] == "UserID")
            {
                data = "UserID~" + str;
            }
            else if (idlist[0] == "TradeCode")
            {
                data = "TradeCode~" + str;
            }

        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
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
        void Procedure()
        {
            string CommandText = "";
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "19" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
            {
                CommandText = "Report_UnprocessTrades";
            }
            else
            {
                CommandText = "Report_UnprocessTradesComm";
            }

            string Broker = "";
            string ClientId = "";
            string GrpType = "";
            string GrpId = "";

            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Broker.Value;
                }

            }

            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "NA";
                if (rdbClientALL.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Client.Value;
                }
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GrpType = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }

            ds = rep.Report_UnprocessTrades_Comm(CommandText, DtFrom.Value.ToString(), DtTo.Value.ToString(), Broker, ClientId,
                rdbInstrumentAll.Checked ? "ALL" : HiddenField_Scrips.Value, rdbTerminalIDAll.Checked ? "ALL" : HiddenField_TerminalID.Value,
                rdbTradeCodeAll.Checked ? "ALL" : HiddenField_TradeCode.Value, HttpContext.Current.Session["usersegid"].ToString().Trim(),
                 Session["LastCompany"].ToString(), GrpType, GrpId, Session["userbranchHierarchy"].ToString(),
                 DLLRptView.SelectedItem.Value.ToString().Trim(), ddlGeneration.SelectedItem.Value.ToString().Trim());

            ViewState["dataset"] = ds;

        }

        protected void btnScreen_Click(object sender, EventArgs e)
        {
            Procedure();
            FnGeneRationCall();
        }
        void FnGeneRationCall()
        {
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    FnHtml(ds);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                {
                    Export(ds);
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "3")////Email
                {
                    Email(ds);
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
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report ;" + str + " Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();

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
                        if (dr1[j].ToString().Trim().StartsWith("Client Name : ") || dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Terminal-Id : ") || dr1[j].ToString().Trim().StartsWith("Scrip Name : ") || dr1[j].ToString().Trim().StartsWith("Trade Code : ") || dr1[j].ToString().Trim().StartsWith("Date"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtmlheader + strHtml;
            }



        }
        protected void btnSendmail_Click(object sender, EventArgs e)
        {
            Procedure();
            FnGeneRationCall();
        }
        void Email(DataSet ds)
        {

            ViewState["mail"] = "mail";
            FnHtml(ds);
            string Date = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(DtTo.Value.ToString());
            string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
            for (int i = 0; i < clnt.Length; i++)
            {
                if (oDBEngine.SendReportSt(ViewState["mail"].ToString().Trim(), clnt[i].ToString().Trim(), Date.ToString().Trim(), "Net Position (Unprocessed Trades) [" + Date.ToString().Trim() + "]") == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('2');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('3');", true);

                }
            }


        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
            FnGeneRationCall();
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report ;" + str + " Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();


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

            objExcel.ExportToExcelforExcel(dtExport, "Net Position (Unprocessed Trades)", "Total: ", dtReportHeader, dtReportFooter);

        }
    }
}