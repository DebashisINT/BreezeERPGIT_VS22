using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace ERP.OMS.Reports
{
    public partial class Reports_DeliveryHoldingCommCurrency : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
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

            dtasondate.EditFormatString = oconverter.GetDateFormat("Date");
            dtasondate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            BindDPAccounts();
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



                if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }

            }
        }
        protected void btndpac_Click(object sender, EventArgs e)
        {
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")
            {
                ddlDPAc.Items.Insert(0, new ListItem("All", "A~A"));
                ddlDPAc.SelectedIndex = 0;

            }
            else
            {
                ListItem item = ddlDPAc.Items.FindByText("All");
                if (item != null)
                {
                    ddlDPAc.Items.RemoveAt(0);
                }


            }
        }
        public void BindDPAccounts()
        {
            DataTable DtDPAcc = new DataTable();
            DtDPAcc = oDBEngine.GetDataTable("Master_DPAccounts", "cast(DPAccounts_ID as varchar)+'~'+rtrim(DPAccounts_AccountType) as ID,DPAccounts_ShortName", " DPAccounts_CompanyID='" + Session["LastCompany"].ToString() + "' and DPAccounts_ExchangeSegmentID in(" + Session["usersegid"].ToString() + ",0)");
            ddlDPAc.DataSource = DtDPAcc;
            ddlDPAc.DataTextField = "DPAccounts_ShortName";
            ddlDPAc.DataValueField = "ID";
            ddlDPAc.DataBind();



        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            SPCALL();
        }
        void procedure()
        {

            string deliverymode = string.Empty;
            string Product = string.Empty;
            string ForClient = string.Empty;
            string Clients = string.Empty;
            string[] A1 = ddlDPAc.SelectedItem.Value.Split('~');
            if (rdDeliveryModeDemat.Checked)
            {
                deliverymode = "D";
            }
            if (rdDeliveryModePhysical.Checked)
            {
                deliverymode = "P";
            }
            if (rdDeliveryModeBoth.Checked)
            {
                deliverymode = "B";
            }


            if (rdbProductAll.Checked)
            {
                Product = "ALL";
            }
            if (rdbProductSelected.Checked)
            {
                Product = Convert.ToString(HiddenField_ScripCriteria.Value);
            }

            ds = oReports.Report_DeliveryHoldingCommCurrency(
                deliverymode,
                 Convert.ToString(dtasondate.Value),
                 A1[0].ToString().Trim(),
                Product,
                 Convert.ToString(Session["LastFinYear"]),
                Convert.ToString(Session["usersegid"]),
                 Convert.ToString(Session["LastCompany"])
                );
            ViewState["dataset"] = ds;


        }
        void SPCALL()
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                string str = null;
                if (rdDeliveryModeDemat.Checked)
                {
                    str = "Delivery Mode :Demat";
                }
                if (rdDeliveryModePhysical.Checked)
                {
                    str = "Delivery Mode :Physical";
                }
                if (rdDeliveryModeBoth.Checked)
                {
                    str = "Delivery Mode :Demat and Physical";
                }

                str = str + "; Report Period :" + oconverter.ArrangeDate2(dtasondate.Value.ToString());
                ViewState["str"] = str;

                if (ddlGeneration.SelectedItem.Value.ToString() == "1")
                {
                    htmlbind();
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")
                {
                    Export();
                }
            }

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void htmlbind()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=10 style=\"color:Blue;\">" + ViewState["str"].ToString().Trim() + "</td></tr></table>";



            String hederhtml = String.Empty;
            hederhtml += "<tr style=\"background-color: #DBEEF3;\">";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Product Name</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Settlement No.</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>ICIN</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Balance</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Closing </br> Rate</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Value</b></td>";
            hederhtml += "</tr>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += hederhtml;
            string dpid = null;
            int flag = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (dpid != ds.Tables[0].Rows[i]["DPID"].ToString().Trim())
                {
                    if (i != 0)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=10>&nbsp;</td>";
                        strHtml += "</tr>";

                    }
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" colspan=10 nowrap=\"nowrap;\"><b>Transaction For : [ </b> " + ds.Tables[0].Rows[i]["DPAC"].ToString().Trim() + "<b> ]</b>";
                    strHtml += "</tr>";



                    dpid = ds.Tables[0].Rows[i]["DPID"].ToString().Trim();
                }

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (ds.Tables[0].Rows[i]["ProductName"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["ProductName"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["ICIN"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["ICIN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SettNoType"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SettNoType"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["Balance"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Balance"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["ClosingRate"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["ClosingRate"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["TOTValue"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["TOTValue"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

                strHtml += "</tr>";
            }
            strHtml += "</tr>";
            strHtml += "</table>";


            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Fn_Display();", true);
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SPCALL();
        }
        void Export()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Product Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Settlement No.", Type.GetType("System.String"));
            dtExport.Columns.Add("ICIN", Type.GetType("System.String"));
            dtExport.Columns.Add("Balance", Type.GetType("System.String"));
            dtExport.Columns.Add("Closing Rate", Type.GetType("System.String"));
            dtExport.Columns.Add("Tot Value", Type.GetType("System.String"));



            string dpid = null;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (dpid != ds.Tables[0].Rows[i]["DPID"].ToString().Trim())
                {
                    DataRow row1 = dtExport.NewRow();
                    row1[0] = "Transaction For :" + ds.Tables[0].Rows[i]["DPAC"].ToString().Trim();
                    row1[1] = "Test";
                    dtExport.Rows.Add(row1);

                    dpid = ds.Tables[0].Rows[i]["DPID"].ToString().Trim();
                }


                /////////all record
                DataRow row3 = dtExport.NewRow();

                if (ds.Tables[0].Rows[i]["ProductName"] != DBNull.Value)
                    row3[0] = ds.Tables[0].Rows[i]["ProductName"].ToString();

                if (ds.Tables[0].Rows[i]["SettNoType"] != DBNull.Value)
                    row3[1] = ds.Tables[0].Rows[i]["SettNoType"].ToString();

                if (ds.Tables[0].Rows[i]["ICIN"] != DBNull.Value)
                    row3[2] = ds.Tables[0].Rows[i]["ICIN"].ToString();

                if (ds.Tables[0].Rows[i]["Balance"] != DBNull.Value)
                    row3[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["Balance"].ToString()));

                if (ds.Tables[0].Rows[i]["ClosingRate"] != DBNull.Value)
                    row3[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["ClosingRate"].ToString()));

                if (ds.Tables[0].Rows[i]["TOTValue"] != DBNull.Value)
                    row3[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["TOTValue"].ToString()));

                dtExport.Rows.Add(row3);
            }
            ViewState["dtExport"] = dtExport;
            finalexport();
        }
        void finalexport()
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


            DrRowR1[0] = "Delivery Transaction Report :" + ViewState["str"].ToString().Trim();

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

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (cmbExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Delivery Transaction Report", "Total", dtReportHeader, dtReportFooter);
                }
            }
            else
            {
                objExcel.ExportToExcelforExcel(dtExport, "Delivery Transaction Report", "Total", dtReportHeader, dtReportFooter);
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Export();
        }
    }
}