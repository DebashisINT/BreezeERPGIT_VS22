using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace ERP.OMS.Reports
{

    public partial class Reports_DeliveryTransactionCommCurrency : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
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
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
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
            ds = oReports.Report_DeliveryTransationCommCurrency(
                deliverymode,
                Convert.ToString(dtfrom.Value),
                Convert.ToString(dtto.Value),
                Convert.ToString(A1[0]),
                Product,
                ForClient,
                Clients,
                 Convert.ToString(Session["LastFinYear"]),
                Convert.ToString(Session["usersegid"]),
                 Convert.ToString(Session["LastCompany"])
                );
            ViewState["dataset"] = ds;
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{


            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "Report_DeliveryTransationCommCurrency";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    if (rdDeliveryModeDemat.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@deliverymode", "D");
            //    }
            //    if (rdDeliveryModePhysical.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@deliverymode", "P");
            //    }
            //    if (rdDeliveryModeBoth.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@deliverymode", "B");
            //    }
            //    cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
            //    cmd.Parameters.AddWithValue("@todate", dtto.Value);

            //    //////////For Split
            //    string[] A1 = ddlDPAc.SelectedItem.Value.Split('~');

            //    cmd.Parameters.AddWithValue("@dpac", A1[0].ToString().Trim());
            //    if (rdbProductAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Product", "ALL");
            //    }
            //    if (rdbProductSelected.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Product", HiddenField_ScripCriteria.Value);
            //    }
            //    if (radClient.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ForClient", "Client");
            //    }
            //    if (radExchange.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ForClient", "Exchange");
            //    }
            //    if (radBoth.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ForClient", "Both");
            //    }
            //    if (radAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Clients", "ALL");
            //    }
            //    if (radPOAClient.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Clients", "POACLIENT");
            //    }
            //    if (radSelected.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Clients", HiddenField_Client.Value);
            //    }

            //    cmd.Parameters.AddWithValue("@finYear", Session["LastFinYear"].ToString());
            //    cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
            //    cmd.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ViewState["dataset"] = ds;

            //}

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

                str = str + "; Report Period :" + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
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
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>TranId</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Date</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Customer </br> Exchange</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>DPID</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Settlement No.</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>ICIN</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Description</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>InQty</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>OutQty</b></td>";
            hederhtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Running Baln.</b></td>";
            hederhtml += "</tr>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            string product = null;
            string dpid = null;
            int flag = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (dpid != ds.Tables[0].Rows[i]["dpid"].ToString().Trim())
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
                    strHtml += "<td align=\"left\" colspan=10 nowrap=\"nowrap;\"><b>Transaction For : [ </b> " + ds.Tables[0].Rows[i]["TransactionFor"].ToString().Trim() + "<b> ]</b>";
                    strHtml += "</tr>";

                    strHtml += hederhtml;

                    dpid = ds.Tables[0].Rows[i]["dpid"].ToString().Trim();
                }
                if (product != ds.Tables[0].Rows[i]["ProductId"].ToString().Trim())
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" colspan=10 style=\"font-size:xx-small;color:#348017\" nowrap=\"nowrap;\"><b>Product Name : [</b>" + ds.Tables[0].Rows[i]["ProductName"].ToString() + " <b>]</b></td>";
                    strHtml += "</tr>";

                    product = ds.Tables[0].Rows[i]["ProductId"].ToString().Trim();
                }

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                if (ds.Tables[0].Rows[i]["TranId"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["TranId"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["Date"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Date"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["CustName"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["CustName"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["DPName"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DPName"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["SettNoType"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SettNoType"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["ICIN"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["ICIN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["Description"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Description"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["InQty"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["InQty"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["OutQty"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["OutQty"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["RunningBaln"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["RunningBaln"].ToString() + "</td>";
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
            dtExport.Columns.Add("TranId", Type.GetType("System.String"));
            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Customer Exchange", Type.GetType("System.String"));
            dtExport.Columns.Add("DPID", Type.GetType("System.String"));
            dtExport.Columns.Add("Settlement No.", Type.GetType("System.String"));
            dtExport.Columns.Add("ICIN", Type.GetType("System.String"));
            dtExport.Columns.Add("Description", Type.GetType("System.String"));
            dtExport.Columns.Add("InQty", Type.GetType("System.String"));
            dtExport.Columns.Add("OutQty", Type.GetType("System.String"));
            dtExport.Columns.Add("Running Baln.", Type.GetType("System.String"));


            string product = null;
            string dpid = null;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (dpid != ds.Tables[0].Rows[i]["dpid"].ToString().Trim())
                {
                    DataRow row1 = dtExport.NewRow();
                    row1[0] = "Transaction For :" + ds.Tables[0].Rows[i]["TransactionFor"].ToString().Trim();
                    row1[1] = "Test";
                    dtExport.Rows.Add(row1);

                    dpid = ds.Tables[0].Rows[i]["dpid"].ToString().Trim();
                }
                if (product != ds.Tables[0].Rows[i]["ProductId"].ToString().Trim())
                {
                    DataRow row2 = dtExport.NewRow();
                    row2[0] = "Product Name :" + ds.Tables[0].Rows[i]["ProductName"].ToString().Trim();
                    row2[1] = "Test";
                    dtExport.Rows.Add(row2);

                    product = ds.Tables[0].Rows[i]["ProductId"].ToString().Trim();
                }

                /////////all record
                DataRow row3 = dtExport.NewRow();
                if (ds.Tables[0].Rows[i]["TranId"] != DBNull.Value)
                    row3[0] = ds.Tables[0].Rows[i]["TranId"].ToString();

                if (ds.Tables[0].Rows[i]["Date"] != DBNull.Value)
                    row3[1] = ds.Tables[0].Rows[i]["Date"].ToString();

                if (ds.Tables[0].Rows[i]["CustName"] != DBNull.Value)
                    row3[2] = ds.Tables[0].Rows[i]["CustName"].ToString();

                if (ds.Tables[0].Rows[i]["DPName"] != DBNull.Value)
                    row3[3] = ds.Tables[0].Rows[i]["DPName"].ToString();

                if (ds.Tables[0].Rows[i]["SettNoType"] != DBNull.Value)
                    row3[4] = ds.Tables[0].Rows[i]["SettNoType"].ToString();

                if (ds.Tables[0].Rows[i]["ICIN"] != DBNull.Value)
                    row3[5] = ds.Tables[0].Rows[i]["ICIN"].ToString();

                if (ds.Tables[0].Rows[i]["Description"] != DBNull.Value)
                    row3[6] = ds.Tables[0].Rows[i]["Description"].ToString();

                if (ds.Tables[0].Rows[i]["InQty"] != DBNull.Value)
                    row3[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["InQty"].ToString()));

                if (ds.Tables[0].Rows[i]["OutQty"] != DBNull.Value)
                    row3[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OutQty"].ToString()));

                if (ds.Tables[0].Rows[i]["RunningBaln"] != DBNull.Value)
                    row3[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["RunningBaln"].ToString()));

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