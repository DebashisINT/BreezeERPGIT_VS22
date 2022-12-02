using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_NetPosition_MTM : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        GenericMethod oGenericMethod = null;
        string data;
        int pageindex = 0;
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
                chkboxliststyle();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }

        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");

            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        void chkboxliststyle()
        {
            foreach (ListItem item in chktfilter.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
            }
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
                    if (idlist[0].ToString().Trim() != "Clients" && idlist[0] != "Expiry" && idlist[0] != "Broker")
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
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str = AcVal[0];

                            }
                            else
                            {
                                str = "'" + AcVal[0] + "'";
                                str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str += "," + AcVal[0];
                            }
                            else
                            {
                                str += ",'" + AcVal[0] + "'";
                                str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                    }
                }


                if (idlist[0] == "Product")
                {
                    data = "Product~" + str;

                }
                else if (idlist[0] == "ScripsExchange")
                {
                    data = "ScripsExchange~" + str;

                }
                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Broker")
                {
                    data = "Broker~" + str;
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
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
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
        protected void btnhide_Click(object sender, EventArgs e)
        {

            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
            {
                Procedure("NA");
            }
            else
            {

                selectionchkboxlist();
            }

        }
        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
            {
                Procedure("NA");
            }
            else
            {

                selectionchkboxlist();
            }

        }

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
            {
                Procedure("NA");
            }
            else
            {
                selectionchkboxlist();
            }
        }

        void selectionchkboxlist()
        {
            string parameter = "";
            int colcount = 0;
            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "BF Lots")
                    {
                        parameter = "[BF Lots]";
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Open Price")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Open Price]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Open Price]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "BF Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[BF Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[BF Value]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Buy Lots")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Buy Lots]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Buy Lots]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Buy Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Buy Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Buy Value]";

                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Buy Avg.")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Buy Avg.]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Buy Avg.]";
                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Lots")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Lots]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Lots]";
                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Value]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Avg.")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Avg.]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Avg.]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Asn/Exc Lot")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Asn/Exc Lot]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Asn/Exc Lot]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "CF Lots")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[CF Lots]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[CF Lots]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sett Price")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sett Price]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sett Price]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "CF Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[CF Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[CF Value]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Premium")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Premium]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Premium]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Mtm")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Mtm]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Mtm]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Fin Sett")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Fin Sett]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Fin Sett]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Asn/Exc Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Asn/Exc Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Asn/Exc Value]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Net Obligation")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Net Obligation]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Net Obligation]";
                        }

                        colcount = colcount + 1;
                    }
                }
            }
            if (colcount > 2)
            {
                Procedure(parameter.ToString().Trim());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(7);", true);
            }

        }
        void Procedure(string parameter)
        {
            string Broker = "";
            string ClientsID = "";
            string Instrument = "";
            string UNDERLYING = "";
            string Expiry = "";
            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Broker.Value;
                }

            }

            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "NA";
                if (rdbClientALL.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Client.Value;
                }
            }
            if (rdbInstrumentAll.Checked)
            {
                Instrument = "ALL";
            }
            else
            {
                Instrument = HiddenField_ScripsExchange.Value;
            }
            if (rdbunderlyingAll.Checked)
            {
                UNDERLYING = "ALL";
            }
            else
            {
                UNDERLYING = HiddenField_Product.Value;
            }

            if (rdbExpiryAll.Checked)
            {
                Expiry = "ALL";
            }
            else
            {
                Expiry = HiddenField_Expiry.Value;
            }

            string GRPTYPE = "";
            string GRPID = "";

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Group.Value;
                }
            }
            else
            {
                GRPTYPE = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_BranchGroup.Value;
                }
            }


            ds = dailyrep.NetPositionCommCurrency(ddldate.SelectedItem.Value.ToString() == "0" ? "NA" : dtfrom.Value.ToString(),
                ddldate.SelectedItem.Value.ToString() == "0" ? dtfor.Value.ToString() : dtfor.Value.ToString(), Session["usersegid"].ToString(),
                HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(),
                ddlNetMktValue.SelectedItem.Value.ToString() == "0" ? "NetVal" : "MrktVal", Broker, ClientsID, Instrument,
                UNDERLYING, Expiry, Session["userbranchHierarchy"].ToString(), GRPTYPE, GRPID, chkOpenClientFUT.Checked ? "CHK" : "UNCHK",
                ChkCalculateCharge.Checked ? "CHK" : "UNCHK", Chksign.Checked ? "CHK" : "UNCHK", ddlrptview.SelectedItem.Value.ToString().Trim(),
                ChkCall.Checked ? "CHK" : "UNCHK", ChkPut.Checked ? "CHK" : "UNCHK", parameter.ToString().Trim());

            ViewState["dataset"] = ds;
            SpProcessCall(ds);

        }

        void SpProcessCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "0")/////////Screen
                {
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                    {
                        FnHtml("Na", ds);
                    }
                    else
                    {
                        DistinctRecordBind(ds);
                    }
                }
                else if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")/////////////Export To Excel
                {
                    DataSet ds1 = new DataSet();
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {

                        DataView viewClient = new DataView(ds.Tables[0]);
                        viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'Clearing Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%' and TabNameOrder not like 'Stamp Duty%' and TabNameOrder not like 'SEBI Fee' and TabNameOrder not like 'STax On%'";
                        DataTable dtex = viewClient.ToTable();
                        ds1.Tables.Add(dtex);
                        ds1.Tables[0].Columns.Remove("Client Name");
                        ds1.Tables[0].Columns.Remove("Customerid");
                        ds1.Tables[0].Columns.Remove("TabNameOrder");
                        ds1.Tables[0].Columns.Remove("GRPID");

                        //  Client Wise Group Wise  

                        ds1.Tables[0].Columns.Remove("GRPNAME");

                    }
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        ds.Tables[0].Columns.Remove("Instrument");
                        ds.Tables[0].Columns.Remove("Productid");

                    }
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                    {
                        ds.Tables[0].Columns.Remove("ClientName");
                        ds.Tables[0].Columns.Remove("TabNameOrder");
                        ds.Tables[0].Columns.Remove("GRPID");
                    }
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        Export(ds1);
                    }
                    else
                    {
                        Export(ds);
                    }
                }
                else if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "3")/////////////Export To PDF
                {
                    DataSet dsExportPdf = new DataSet();
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        DataView viewClient = new DataView(ds.Tables[0]);
                        viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'Clearing Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%' and TabNameOrder not like 'Stamp Duty%' and TabNameOrder not like 'SEBI Fee' and TabNameOrder not like 'STax On%'";
                        DataTable dtex = viewClient.ToTable();
                        dsExportPdf.Tables.Add(dtex);
                        dsExportPdf.Tables[0].Columns.Remove("Client Name");
                        //dsExportPdf.Tables[0].Columns.Remove("Customerid");
                        dsExportPdf.Tables[0].Columns.Remove("TabNameOrder");


                        // For Client Wise Group is done GRPID in  Crystal Report 

                        dsExportPdf.Tables[0].Columns.Remove("GRPNAME");

                        if (ddlrptview.SelectedIndex > 0)
                        {

                            dsExportPdf.Tables[0].Columns.Remove("GRPID");


                        }


                        ExportToPDF(dsExportPdf);
                    }
                }
                else if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")/////////////Email
                {
                    Email(ds);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void DistinctRecordBind(DataSet ds)
        {
            DataView viewClient = new DataView();
            DataTable Distinctclient = new DataTable();
            if (ddlrptview.SelectedItem.Value == "0")
            {
                viewClient = new DataView(ds.Tables[0]);
                Distinctclient = viewClient.ToTable(true, new string[] { "GRPNAME" });

                // Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });

                //DataRow[] result = Distinctclient.Select("TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'");
                //viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'";

                //27/08/2015

                //  Below Code Was Commented  For Client Wise Group wise view  Start 27/08/2015

                // viewClient.RowFilter = "Instrument like 'Total%'";
                //Distinctclient = viewClient.ToTable();

                //----------------------------





            }
            else
            {
                viewClient = new DataView(ds.Tables[0]);
                Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });

            }
            //foreach (DataRow row in result)
            //{
            //    dataTable1.ImportRow(row);
            //}
            //int j=Distinctclient.Rows.Count;
            //for (; j > 1; j--)
            //{
            //    Distinctclient.Rows[j - 1].Delete();
            //}

            if (Distinctclient.Rows.Count > 0)
            {

                if (ddlrptview.SelectedItem.Value == "0")
                {
                    DdlRecord.DataSource = Distinctclient;
                    DdlRecord.DataValueField = "GRPNAME";
                    DdlRecord.DataTextField = "GRPNAME";
                    DdlRecord.DataBind();
                }

                else
                {

                    DdlRecord.DataSource = Distinctclient;
                    DdlRecord.DataValueField = "TabNameOrder";
                    DdlRecord.DataTextField = "TabNameOrder";
                    DdlRecord.DataBind();

                }

            }
            FnHtml(DdlRecord.SelectedItem.Value.ToString(), ds);
        }

        string FnHtml(string RecordValueFeild, DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            str = str + " ; Report View : " + ddlrptview.SelectedItem.Text.ToString().Trim();


            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            //if (ddlrptview.SelectedItem.Value.ToString().Trim() != "2" && ddlrptview.SelectedItem.Value.ToString().Trim() != "3")
            //{
            //    viewclient.RowFilter = "TabNameOrder='" + RecordValueFeild.ToString().Trim() + "'";
            //}
            if (RecordValueFeild.ToString().Trim() != "Na" && RecordValueFeild.ToString().Trim() != "NA")
            {
                if (ddlrptview.SelectedItem.Value == "0")
                {
                    viewclient.RowFilter = "GRPNAME='" + RecordValueFeild.ToString().Trim() + "'";
                }

                else
                {

                    viewclient.RowFilter = "TabNameOrder='" + RecordValueFeild.ToString().Trim() + "'";

                }
            }

            DataTable dt = new DataTable();
            dt = viewclient.ToTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                string record = "NA";
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                {

                    record = dt.Rows[0]["Customerid"].ToString().Trim();
                    if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                        strHtmlheader += "<td align=\"left\" colspan=20>Client Name :&nbsp;<b>" + dt.Rows[0]["Client Name"].ToString().Trim();
                        strHtmlheader += "</td></tr>";

                    }
                    dt.Columns.Remove("Client Name");
                    dt.Columns.Remove("TabNameOrder");
                    dt.Columns.Remove("Customerid");
                    dt.Columns.Remove("GRPID");
                    dt.Columns.Remove("GRPNAME");
                    dt.Rows[0].Delete();
                }
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "1")
                {
                    record = dt.Rows[0]["TabNameOrder"].ToString().Trim();
                    if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                        strHtmlheader += "<td align=\"left\" colspan=20>Scrip Name :&nbsp;<b>" + dt.Rows[0]["Instrument"].ToString().Trim();
                        strHtmlheader += "</td></tr>";

                    }
                    dt.Columns.Remove("Instrument");
                    dt.Columns.Remove("TabNameOrder");
                    dt.Columns.Remove("Productid");
                    //dt.Columns.Remove("GRPID");
                    dt.Rows[0].Delete();
                }
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                {
                    record = dt.Rows[0]["TabNameOrder"].ToString().Trim();
                    if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                        strHtmlheader += "<td align=\"left\" colspan=20>Client Name :&nbsp;<b>" + dt.Rows[0]["ClientName"].ToString().Trim();
                        strHtmlheader += "</td></tr>";

                    }
                    dt.Columns.Remove("ClientName");
                    dt.Columns.Remove("TabNameOrder");
                    dt.Columns.Remove("GRPID");

                }

                if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")
                {
                    strHtmlheader += "<tr><td align=\"left\" colspan=" + dt.Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";
                }


                ////////Detail Display
                String strHtml = String.Empty;
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (ddlrptview.SelectedItem.Value == "0")
                    {
                        if (i > 20)
                            break;
                    }
                    if (ddlrptview.SelectedItem.Value == "1")
                    {
                        if (i > 19)
                            break;
                    }
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in dt.Rows)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (ddlrptview.SelectedItem.Value == "0")
                        {
                            if (j > 20)
                                break;
                        }
                        if (ddlrptview.SelectedItem.Value == "1")
                        {
                            if (j > 19)
                                break;
                        }
                        if (dr1[j] != DBNull.Value)
                        {
                            if (dr1[j].ToString().Trim().StartsWith("Net Total:") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("STax") || dr1[j].ToString().Trim().StartsWith("Transaction") || dr1[j].ToString().Trim().StartsWith("Clearing") || dr1[j].ToString().Trim().StartsWith("Stamp") || dr1[j].ToString().Trim().StartsWith("SEBI") || dr1[j].ToString().Trim().StartsWith("Overall") || dr1[j].ToString().Trim().StartsWith("Grand"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                            }
                            else if (dr1[j].ToString().Trim().StartsWith("Test"))
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                            else if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                // For Client Wise Group

                                if (ddlrptview.SelectedItem.Value == "0")
                                {

                                    if (dr1[1].ToString().Trim().StartsWith("Test"))
                                    {

                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;color:blue;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                    }
                                    else
                                    {

                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                    }

                                }

                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
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
                int width = 990;
                //display.Attributes.Add("style", "width: " + hidScreenwd.Value + "px; overflow:scroll");
                DivHeader.Attributes.Add("style", "width: " + width + "px; ");
                Divdisplay.Attributes.Add("style", "width: " + width + "px; overflow:scroll");
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;

                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "0")
                {
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2," + dt.Columns.Count + ");", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1," + dt.Columns.Count + ");", true);

                    }
                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                {
                    return MailSend(strHtmlheader + strHtml, record.ToString().Trim());
                }
            }
            return null;
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        protected void DdlRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            FnHtml(DdlRecord.SelectedItem.Value.ToString(), ds);
        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = DdlRecord.SelectedIndex;
            int totalNo = DdlRecord.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalGrp.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            DdlRecord.SelectedIndex = curentIndex;
            ds = (DataSet)ViewState["dataset"];
            FnHtml(DdlRecord.SelectedItem.Value.ToString(), ds);


        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            GenericMethod oGenericMethod = new GenericMethod();
            string EechangeSegmentName = oGenericMethod.GetExchangeSegmentName();

            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + EechangeSegmentName + " " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + EechangeSegmentName + " of Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            str = str + " ; Report View : " + ddlrptview.SelectedItem.Text.ToString().Trim();


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

            objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total:", dtReportHeader, dtReportFooter);

        }
        void Email(DataSet ds)
        {
            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView();
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")////Client Wise
            {
                string errorreport = null;
                //viewClient = (ds.Tables[0]);

                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                    viewClient.RowFilter = "TabNameOrder<>''";

                if (ddlrptview.SelectedItem.Value == "0")
                {
                    viewClient = new DataView(ds.Tables[0]);
                    Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });
                    DataRow[] result = Distinctclient.Select("TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'");
                    //viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'";
                    viewClient.RowFilter = "Instrument like 'Total%'";
                    Distinctclient = viewClient.ToTable();
                }
                else
                {
                    viewClient = new DataView(ds.Tables[0]);
                    Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });

                }

                //Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });


                if (Distinctclient.Rows.Count > 0)
                {
                    DdlRecord.DataSource = Distinctclient;
                    DdlRecord.DataValueField = "TabNameOrder";
                    DdlRecord.DataTextField = "TabNameOrder";
                    DdlRecord.DataBind();

                }
                for (int k = 0; k < DdlRecord.Items.Count; k++)
                {
                    errorreport = FnHtml(DdlRecord.Items[k].Text.ToString().Trim(), ds);

                }
                if (errorreport.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
                }
                if (errorreport.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                if (errorreport.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
                }

            }
            else if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")
            {
                string Html = null;
                string billdate = null;
                string error = "NA";
                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    billdate = oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    billdate = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }

                if (ddlrptview.SelectedItem.Value.ToString().Trim() != "2" && ddlrptview.SelectedItem.Value.ToString().Trim() != "3")
                {
                    //viewClient = (ds.Tables[0]);
                    //DataTable Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });
                    if (ddlrptview.SelectedItem.Value == "0")
                    {
                        viewClient = new DataView(ds.Tables[0]);
                        Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });
                        DataRow[] result = Distinctclient.Select("TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'");
                        //viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'";
                        viewClient.RowFilter = "Instrument like 'Total%'";
                        Distinctclient = viewClient.ToTable();
                    }
                    else
                    {
                        viewClient = new DataView(ds.Tables[0]);
                        Distinctclient = viewClient.ToTable(true, new string[] { "TabNameOrder" });

                    }
                    string errorreport = null;
                    int someclienterror = 0;
                    int success = 0;
                    int errorsuccess = 0;

                    if (Distinctclient.Rows.Count > 0)
                    {
                        DdlRecord.DataSource = Distinctclient;
                        DdlRecord.DataValueField = "TabNameOrder";
                        DdlRecord.DataTextField = "TabNameOrder";
                        DdlRecord.DataBind();

                    }
                    for (int k = 0; k < DdlRecord.Items.Count; k++)
                    {
                        errorreport = FnHtml(DdlRecord.Items[k].Text.ToString().Trim(), ds);
                        Html += errorreport.ToString().Trim();
                    }
                }
                else
                {
                    Html = FnHtml("NA", ds);
                }

                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(Html.ToString().Trim(), clnt[i].ToString().Trim(), billdate.ToString().Trim(), "[ " + Session["Segmentname"].ToString() + " ] Net Position [" + billdate.ToString().Trim() + "]") == true)
                    {
                        if (error.ToString().Trim() == "errorsuccess")
                        {
                            error = "someclienterror";
                        }
                        else
                        {
                            error = "success";
                        }
                    }
                    else
                    {

                        error = "errorsuccess";
                    }
                }
                if (error.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
                }
                if (error.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                if (error.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
                }
            }
            else if (ddloptionformail.SelectedItem.Value == "3")
            {
                string errorreport = null;
                viewClient = new DataView(ds.Tables[0]);

                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                    viewClient.RowFilter = "GRPID<>'ZZZZ'";

                if (ddlrptview.SelectedItem.Value == "0")
                {
                    viewClient = new DataView(ds.Tables[0]);
                    Distinctclient = viewClient.ToTable(true, new string[] { "GRPID" });
                    //DataRow[] result = Distinctclient.Select("TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'");
                    //viewClient.RowFilter = "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'";
                    viewClient.RowFilter = "Instrument like 'Total%'";
                    Distinctclient = viewClient.ToTable();
                }
                else
                {
                    Distinctclient = viewClient.ToTable(true, new string[] { "GRPID" });
                }
                //Distinctclient = viewClient.ToTable(true, new string[] { "GRPID" });


                if (Distinctclient.Rows.Count > 0)
                {
                    DdlRecord.DataSource = Distinctclient;
                    DdlRecord.DataValueField = "GRPID";
                    DdlRecord.DataTextField = "GRPID";
                    DdlRecord.DataBind();

                }

                for (int k = 0; k < DdlRecord.Items.Count; k++)
                {
                    errorreport = BranchMailHtml(DdlRecord.Items[k].Text.ToString().Trim(), ds);

                }
                if (errorreport.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
                }
                if (errorreport.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                if (errorreport.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
                }


            }

        }
        string MailSend(string Html, string clientid)
        {
            string billdate = null;
            string chkrecord = "no";
            string Email_Subject = null;
            oGenericMethod = new GenericMethod();
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                billdate = oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                billdate = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")///Client Wise
            {
                Email_Subject = "[ " + Session["Segmentname"].ToString() + " ][ " + oGenericMethod.GetClientInfo(clientid.ToString().Trim(), 1).Rows[0][2].ToString().Trim() + " ] Net Position [" + billdate.ToString().Trim() + "]";
                if (oDBEngine.SendReport(Html.ToString().Trim(), clientid.ToString().Trim(), billdate.ToString().Trim(), Email_Subject) == true)
                {
                    if (chkrecord.ToString().Trim() == "errorsuccess")
                    {
                        chkrecord = "someclienterror";
                    }
                    else
                    {
                        chkrecord = "success";
                    }
                }
                else
                {

                    chkrecord = "errorsuccess";
                }

            }
            else if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "3")//Branchwise
            {
                if (oDBEngine.SendReportBranchWise(Html.ToString().Trim(), clientid.ToString().Trim(), billdate.ToString().Trim(), "[ " + Session["Segmentname"].ToString() + " ] Net Position [" + billdate.ToString().Trim() + "]", ddloptionformail.SelectedItem.Text) == true)
                {
                    if (chkrecord.ToString().Trim() == "errorsuccess")
                    {
                        chkrecord = "someclienterror";
                    }
                    else
                    {
                        chkrecord = "success";
                    }
                }
                else
                {

                    chkrecord = "errorsuccess";
                }

            }
            else
            {
                chkrecord = Html.ToString().Trim();
            }

            return chkrecord;
        }
        protected void ddlrptview_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void SetGenerateType(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedValue == "0" || ddlrptview.SelectedValue == "2" || ddlrptview.SelectedValue == "3")
            {
                if (ddlGroup.SelectedValue == "0" || ddlGroup.SelectedValue == "1")
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("Client", "1"));
                    ddloptionformail.Items.Add(new ListItem("User", "2"));
                    ddloptionformail.Items.Add(new ListItem(ddlGroup.SelectedItem.Text, "3"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jsshowuseropt", "fnddloptionformail(1);", true);
                }
                else
                {
                    if (ddlGeneration.SelectedValue == "1")
                    {
                        ddloptionformail.Items.Clear();
                        ddloptionformail.Items.Add(new ListItem("User", "2"));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jsshowuseropt", "fnddloptionformail(2);", true);
                    }
                }
            }
            else if (ddlrptview.SelectedValue == "1")
            {

            }


        }

        string BranchMailHtml(string RecordValueFeild, DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            str = str + " ; Report View : " + ddlrptview.SelectedItem.Text.ToString().Trim();


            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                viewclient.RowFilter = "[Instrument Name]<>'Total:'";

            //if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2")
            //{
            //    DataView dv = new DataView();
            //    dv = ds.Tables[0].DefaultView;
            //    dv.RowFilter = "[Buy Position]<>'Test'";
            //    DataTable dtfilt = new DataTable();
            //    dtfilt = dv.ToTable();
            //    viewclient = dtfilt.DefaultView;
            //}

            //if (ddlrptview.SelectedItem.Value.ToString().Trim() != "2" && ddlrptview.SelectedItem.Value.ToString().Trim() != "3")
            //{
            //    viewclient.RowFilter = "TabNameOrder='" + RecordValueFeild.ToString().Trim() + "'";
            //}
            if (ddlrptview.SelectedItem.Value == "0")
            {
                if (RecordValueFeild.ToString().Trim() != "Na")
                    viewclient.RowFilter = "GRPID='" + RecordValueFeild.ToString().Trim() + "'and TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%' and TabNameOrder not like 'Stamp Duty%' and TabNameOrder not like 'SEBI Fee' and TabNameOrder not like 'STax On%'";
            }
            else
            {
                if (RecordValueFeild.ToString().Trim() != "Na")
                {
                    viewclient.RowFilter = "GRPID='" + RecordValueFeild.ToString().Trim() + "'";
                }
            }
            //viewclient.RowFilter= "TabNameOrder not like 'Transaction Charges%' and TabNameOrder not like 'STax On Trn.Charges%' and TabNameOrder not like 'Total%'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            string record = "NA";
            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {

                record = dt.Rows[0]["GRPID"].ToString().Trim();
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                {
                    strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                    strHtmlheader += "<td align=\"left\" colspan=20>Client Name :&nbsp;<b>" + dt.Rows[0]["Client Name"].ToString().Trim();
                    strHtmlheader += "</td></tr>";

                }
                dt.Columns.Remove("Client Name");
                dt.Columns.Remove("TabNameOrder");
                dt.Columns.Remove("Customerid");
                dt.Columns.Remove("GRPID");
                // dt.Rows[0].Delete();
            }
            //if (ddlrptview.SelectedItem.Value.ToString().Trim() == "1")
            //{
            //    record = dt.Rows[0]["GRPID"].ToString().Trim();
            //    if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            //    {
            //        strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
            //        strHtmlheader += "<td align=\"left\" colspan=20>Scrip Name :&nbsp;<b>" + dt.Rows[0]["Instrument"].ToString().Trim();
            //        strHtmlheader += "</td></tr>";

            //    }
            //    dt.Columns.Remove("Instrument");
            //    dt.Columns.Remove("TabNameOrder");
            //    dt.Columns.Remove("Productid");
            //    dt.Rows[0].Delete();
            //}
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2")
            {
                record = dt.Rows[0]["GRPID"].ToString().Trim();
                //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                //{
                //    strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                //    strHtmlheader += "<td align=\"left\" colspan=20>Client Name :&nbsp;<b>" + dt.Rows[0]["ClientName"].ToString().Trim();
                //    strHtmlheader += "</td></tr>";

                //}
                dt.Columns.Remove("ClientName");
                dt.Columns.Remove("TabNameOrder");
                dt.Columns.Remove("GRPID");
                dt.Rows[0].Delete();

            }
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
            {
                record = dt.Rows[0]["GRPID"].ToString().Trim();

                dt.Columns.Remove("ClientName");
                dt.Columns.Remove("TabNameOrder");
                dt.Columns.Remove("GRPID");
                dt.Rows[0].Delete();

            }

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")
            {
                strHtmlheader += "<tr><td align=\"left\" colspan=" + dt.Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";
            }

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Net Total:") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("STax") || dr1[j].ToString().Trim().StartsWith("Transaction") || dr1[j].ToString().Trim().StartsWith("Stamp") || dr1[j].ToString().Trim().StartsWith("SEBI") || dr1[j].ToString().Trim().StartsWith("Overall") || dr1[j].ToString().Trim().StartsWith("Grand"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
            //int width = 990;
            //display.Attributes.Add("style", "width: " + hidScreenwd.Value + "px; overflow:scroll");
            //DivHeader.Attributes.Add("style", "width: " + width + "px; overflow:scroll");
            //Divdisplay.Attributes.Add("style", "width: " + width + "px; overflow:scroll");
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;

            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "0")
            //{
            //    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "2" || ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2," + dt.Columns.Count + ");", true);

            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1," + dt.Columns.Count + ");", true);

            //    }
            //}
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                return MailSend(strHtml, record.ToString().Trim());
            }
            return null;
        }

        protected void ExportToPDF(DataSet DsNetPosition)
        {
            //====Start For Logo Add==============
            byte[] logoinByte;
            DataTable DtLogo = new DataTable();
            DtLogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                DtLogo.Rows.Add(logoinByte);
            }
            DtLogo.AcceptChanges();
            DataSet DsLogo = new DataSet();
            DsLogo.Tables.Add(DtLogo);

            //======Start Company Detail=================
            //        DataTable dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
            //                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
            //	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
            //	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
            //                                                                   @cmpPan=isnull(cmp_panNo,''),
            //                                                                   @cmpHoldingStatementAsAt='Net Position MTM Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString()) +' '+'['+Session["segmentname"].ToString()+']'+ "',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");

            DataTable dtCompany;
            if (ddldate.SelectedItem.Value == "0")
            {
                dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
                                                                   @cmpPan=isnull(cmp_panNo,''),
                                                                   @cmpHoldingStatementAsAt='Net Position MTM Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + ' ' + '[' + Session["segmentname"].ToString() + ']' + "',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' and exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");

            }
            else
            {
                dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
                                                                   @cmpPan=isnull(cmp_panNo,''),
                                                                   @cmpHoldingStatementAsAt='Net Position MTM Report From " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString()) + ' ' + '[' + Session["segmentname"].ToString() + ']' + "',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' and exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");

            }


            //  DsNetPosition.Tables[0].WriteXmlSchema("E:\\RPTXSD\\NetPosMTM_Detail.xsd");


            DataSet DsCompany = new DataSet();
            DsCompany.Tables.Add(dtCompany);

            //   DsCompany.Tables[0].WriteXmlSchema("E:\\RPTXSD\\NetPosMTM__CompanyLogo.xsd");

            //   DsNetPosition.Tables[0].WriteXmlSchema("E:\\RPTXSD\\NetPosMTM_Detail.xsd");

            //======Generate XSD===========================            
            //DsNetPosition.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM_Detail.xsd");
            //DsCompany.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM_Company.xsd");
            //DsLogo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM__CompanyLogo.xsd");

            ReportDocument NetPositionMTMReportDoc = new ReportDocument();
            string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');

            string path = "";

            //-------------------  For Client Wise Group Wise New Crystal Report  Start 27/08/2015--------- 

            if (ddlrptview.SelectedIndex == 0)
            {

                path = Server.MapPath("..\\Reports\\NetPositionMTM_COMMCDXClient.rpt");

            }
            else
            {
                path = Server.MapPath("..\\Reports\\NetPositionMTM_COMMCDX.rpt");


            }

            //-------------------  For Client Wise Group Wise New Crystal Report  End--------- 



            NetPositionMTMReportDoc.Load(path);
            NetPositionMTMReportDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            NetPositionMTMReportDoc.SetDataSource(DsNetPosition.Tables[0]);
            NetPositionMTMReportDoc.Subreports["CompanyDetail"].SetDataSource(DsCompany.Tables[0]);
            NetPositionMTMReportDoc.Subreports["CompanyLogo"].SetDataSource(DsLogo.Tables[0]);
            NetPositionMTMReportDoc.SetParameterValue("@SegmentName", (object)Session["Segmentname"].ToString().Split('-')[1]);
            NetPositionMTMReportDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Net Position MTM");
            DsNetPosition.Dispose();
            DsCompany.Clear();
            DsLogo.Dispose();
        }
    }
}