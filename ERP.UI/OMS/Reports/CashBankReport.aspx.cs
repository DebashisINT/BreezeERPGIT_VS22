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

namespace ERP.OMS.Reports
{
    public partial class Reports_CashBankReport : System.Web.UI.Page
    {
        BusinessLogicLayer.FAReportsOther objfaReportOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string str = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
            if (!IsPostBack)
            {
                btnReport.Visible = false;
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                dtToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
                hdnPrint.Value = "true";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Hide();</script>");

                //In Case Of NSDL And CDSL ExchangeSegmentID For Bank Fetch
                if (Session["UserSegID"].ToString().Length == 8)
                    hdn_NsdlCdslExchangeSegment.Value = oDBEngine.GetFieldValue("tbl_master_companyExchange", "exch_internalId", "exch_TMCode='" + Session["UserSegID"].ToString() + "'", 1)[0, 0];

                SetBankNames();
            }
            else
                BindGrid();
            if (ddlPrintMode.SelectedValue == "2" && hdnPrint.Value == "true")
            {
                showdata();
            }
        }

        public void SetBankNames()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_Bank", " ltrim(rtrim(bnk_id)) code,ltrim(rtrim(bnk_bankName)) Name", null);
            txtBankName.DataSource = DT;
            txtBankName.DataMember = "Code";
            txtBankName.DataTextField = "Name";
            txtBankName.DataValueField = "Code";
            txtBankName.DataBind();
        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            string SelectedIds = "";
            for (int i = 0; i < gridCashBank.VisibleRowCount; i++)
            {
                //DataView dr= gridJournalVouchar.GetDataRow(i);//.GetRow(i);
                if (gridCashBank.Selection.IsRowSelected(i))
                {
                    if (SelectedIds == "")
                        SelectedIds = gridCashBank.GetRowValues(i, "CashbankDetailsId").ToString();
                    else
                        SelectedIds += "," + gridCashBank.GetRowValues(i, "CashbankDetailsId").ToString();
                }

            }
            if (SelectedIds == "")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "JSalert", "<script>alert('Please Select Any Voucher !');</script>");
            }
            else
            {
                DataSet dsCrystal = new DataSet();
                string BankName = txtBankName_hidden.Value;
                string FromDate = dtDate.Value.ToString();
                string ToDate = dtToDate.Value.ToString();
                string Type = null;
                string Mode = null;
                if (chkAll.Checked == true)
                    Type = "All";
                else
                    Type = "No";
                if (rbtnSingle.Checked == true)
                    Mode = "Single";
                else if (rbtnDouble.Checked == true)
                    Mode = "Double";
                else if (rbtnTriple.Checked == true)
                    Mode = "Triple";
                dsCrystal = objfaReportOther.CashBankVoucherPrint(
                   Convert.ToString(BankName),
                  Convert.ToString(FromDate),
                  Convert.ToString(ToDate),
                  Convert.ToString(Type),
                Convert.ToString(chkDebit.Checked),
                 Convert.ToString(chkCredit.Checked),
                   Convert.ToString(chkContra.Checked),
                  Convert.ToString(Session["usersegid"]),
                  Convert.ToString(Mode),
                  Convert.ToString(txtFromCheque.Text),
                Convert.ToString(txtToCheque.Text),
                 Convert.ToString('F'),
                  "",
                 Convert.ToString(SelectedIds)
                   );
                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection con = new SqlConnection(conn);
                //SqlCommand cmd3 = new SqlCommand("CashBankVoucherPrint", con);
                //cmd3.CommandType = CommandType.StoredProcedure;
                //cmd3.Parameters.AddWithValue("@BankName", BankName);
                //cmd3.Parameters.AddWithValue("@FromDate", FromDate);
                //cmd3.Parameters.AddWithValue("@ToDate", ToDate);
                //cmd3.Parameters.AddWithValue("@Type", Type);
                //cmd3.Parameters.AddWithValue("@Debit", chkDebit.Checked.ToString());
                //cmd3.Parameters.AddWithValue("@Credit", chkCredit.Checked.ToString());
                //cmd3.Parameters.AddWithValue("@Contra", chkContra.Checked.ToString());
                //cmd3.Parameters.AddWithValue("@Segment", Session["usersegid"].ToString());
                //cmd3.Parameters.AddWithValue("@Mode", Mode);
                //cmd3.Parameters.AddWithValue("@FromChequeNumber", txtFromCheque.Text.Trim());
                //cmd3.Parameters.AddWithValue("@ToChequeNumber", txtToCheque.Text.Trim());
                //cmd3.Parameters.AddWithValue("@CallFromCashBankEntry", 'F');
                //cmd3.Parameters.AddWithValue("@IBRefString", "");
                //cmd3.Parameters.AddWithValue("@CBDetailsIds", SelectedIds);
                //cmd3.CommandTimeout = 0;
                //SqlDataAdapter Adap = new SqlDataAdapter();
                //Adap.SelectCommand = cmd3;
                //Adap.Fill(dsCrystal);
                //cmd3.Dispose();
                //con.Dispose();
                //GC.Collect();

                dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");

                if (ddlPrintMode.SelectedValue == "1")
                {
                    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                    ReportDocument reportObj = new ReportDocument();
                    string ReportPath = Server.MapPath("..\\Reports\\CashBank.rpt");
                    reportObj.Load(ReportPath);
                    reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    reportObj.SetDataSource(dsCrystal);
                    if (user.Checked)
                    {
                        reportObj.SetParameterValue("@user", (object)"CHK".ToString().Trim());
                    }
                    else
                    {
                        reportObj.SetParameterValue("@user", (object)"UNCHK".ToString().Trim());
                    }
                    if (time.Checked)
                    {
                        reportObj.SetParameterValue("@time", (object)"CHK".ToString().Trim());
                    }
                    else
                    {
                        reportObj.SetParameterValue("@time", (object)"UNCHK".ToString().Trim());
                    }
                    //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
                    //dsCrystalPrint.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
                    //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
                    reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "CashBank");
                    reportObj.Dispose();
                    GC.Collect();
                   // this.CrystalReportViewer1.Visible = false;
                }
                else if (ddlPrintMode.SelectedValue == "2")
                {
                    //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
                   // this.CrystalReportSource1.ReportDocument.SetDataSource(dsCrystal);

                    //this.CrystalReportSource1.ReportDocument.PrintOptions.PrinterName = "";
                    if (user.Checked)
                    {
                       // this.CrystalReportSource1.ReportDocument.SetParameterValue("@user", (object)"CHK");
                    }
                    else
                    {
                        //this.CrystalReportSource1.ReportDocument.SetParameterValue("@user", (object)"UNCHK");
                    }
                    if (time.Checked)
                    {
                       // this.CrystalReportSource1.ReportDocument.SetParameterValue("@time", (object)"CHK");
                    }
                    else
                    {
                       // this.CrystalReportSource1.ReportDocument.SetParameterValue("@time", (object)"UNCHK");
                    }
                    //this.CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                    //this.CrystalReportViewer1.HasPrintButton = true;
                    //this.CrystalReportViewer1.HasViewList = false;
                    //this.CrystalReportViewer1.HasGotoPageButton = false;
                    //this.CrystalReportViewer1.HasExportButton = false;
                    //this.CrystalReportViewer1.HasPageNavigationButtons = false;
                    //this.CrystalReportViewer1.HasSearchButton = false;
                    //this.CrystalReportViewer1.HasZoomFactorList = false;
                    //this.CrystalReportViewer1.HasRefreshButton = false;
                    //this.CrystalReportViewer1.HasDrillUpButton = false;
                    //this.CrystalReportViewer1.HasToggleGroupTreeButton = false;
                    //this.CrystalReportViewer1.DisplayGroupTree = false;
                    //this.CrystalReportViewer1.SeparatePages = false;
                    //this.CrystalReportViewer1.Visible = true;
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Show();</script>");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightM", "<script>height();</script>");
                }
            }
        }
        public void showdata()
        {
            string SelectedIds = "";
            for (int i = 0; i < gridCashBank.VisibleRowCount; i++)
            {
                //DataView dr= gridJournalVouchar.GetDataRow(i);//.GetRow(i);
                if (gridCashBank.Selection.IsRowSelected(i))
                {
                    if (SelectedIds == "")
                        SelectedIds = gridCashBank.GetRowValues(i, "CashbankDetailsId").ToString();
                    else
                        SelectedIds += "," + gridCashBank.GetRowValues(i, "CashbankDetailsId").ToString();
                }

            }

            DataSet dsCrystalPrint = new DataSet();
            string BankName = txtBankName_hidden.Value;
            string FromDate = dtDate.Value.ToString();
            string ToDate = dtToDate.Value.ToString();
            string Type = null;
            string Mode = null;
            if (chkAll.Checked == true)
                Type = "All";
            else
                Type = "No";
            if (rbtnSingle.Checked == true)
                Mode = "Single";
            else if (rbtnDouble.Checked == true)
                Mode = "Double";
            else if (rbtnTriple.Checked == true)
                Mode = "Triple";
            dsCrystalPrint = objfaReportOther.CashBankVoucherPrint(
                  Convert.ToString(BankName),
                 Convert.ToString(FromDate),
                 Convert.ToString(ToDate),
                 Convert.ToString(Type),
               Convert.ToString(chkDebit.Checked),
                Convert.ToString(chkCredit.Checked),
                  Convert.ToString(chkContra.Checked),
                 Convert.ToString(Session["usersegid"]),
                 Convert.ToString(Mode),
                 Convert.ToString(txtFromCheque.Text),
               Convert.ToString(txtToCheque.Text),
                Convert.ToString('F'),
                 "",
                Convert.ToString(SelectedIds)
                  );
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("CashBankVoucherPrint", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@BankName", BankName);
            //cmd3.Parameters.AddWithValue("@FromDate", FromDate);
            //cmd3.Parameters.AddWithValue("@ToDate", ToDate);
            //cmd3.Parameters.AddWithValue("@Type", Type);
            //cmd3.Parameters.AddWithValue("@Debit", chkDebit.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Credit", chkCredit.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Contra", chkContra.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Segment", Session["usersegid"].ToString());
            //cmd3.Parameters.AddWithValue("@Mode", Mode);
            //cmd3.Parameters.AddWithValue("@FromChequeNumber", txtFromCheque.Text.Trim());
            //cmd3.Parameters.AddWithValue("@ToChequeNumber", txtToCheque.Text.Trim());
            //cmd3.Parameters.AddWithValue("@CallFromCashBankEntry", 'F');
            //cmd3.Parameters.AddWithValue("@IBRefString", String.Empty);
            //cmd3.Parameters.AddWithValue("@CBDetailsIds", SelectedIds);
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(dsCrystalPrint);
            //cmd3.Dispose();
            //con.Dispose();

            dsCrystalPrint.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");

            if (ddlPrintMode.SelectedValue == "1")
            {
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                ReportDocument reportObj = new ReportDocument();

                string ReportPath = Server.MapPath("..\\Reports\\CashBank.rpt");
                reportObj.Load(ReportPath);
                reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                reportObj.SetDataSource(dsCrystalPrint);
                dsCrystalPrint.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
                //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
                if (user.Checked)
                {
                    reportObj.SetParameterValue("@user", (object)"CHK");
                }
                else
                {
                    reportObj.SetParameterValue("@user", (object)"UNCHK");
                }
                if (time.Checked)
                {
                    reportObj.SetParameterValue("@time", (object)"CHK");
                }
                else
                {
                    reportObj.SetParameterValue("@time", (object)"UNCHK");
                }

                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "CashBank");
                reportObj.Dispose();
                GC.Collect();
                //this.CrystalReportViewer1.Visible = false;
            }
            else if (ddlPrintMode.SelectedValue == "2")
            {
                //dsCrystalPrint.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
               // this.CrystalReportSource1.ReportDocument.SetDataSource(dsCrystalPrint);

                //this.CrystalReportSource1.ReportDocument.PrintOptions.PrinterName = "";
                if (user.Checked)
                {
                  //  this.CrystalReportSource1.ReportDocument.SetParameterValue("@user", (object)"CHK");
                }
                else
                {
                  //  this.CrystalReportSource1.ReportDocument.SetParameterValue("@user", (object)"UNCHK");
                }
                if (time.Checked)
                {
                  //  this.CrystalReportSource1.ReportDocument.SetParameterValue("@time", (object)"CHK");
                }
                else
                {
                   // this.CrystalReportSource1.ReportDocument.SetParameterValue("@time", (object)"UNCHK");
                }
                //this.CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                //this.CrystalReportViewer1.HasPrintButton = true;
                //this.CrystalReportViewer1.HasViewList = false;
                //this.CrystalReportViewer1.HasGotoPageButton = false;
                //this.CrystalReportViewer1.HasExportButton = false;
                //this.CrystalReportViewer1.HasPageNavigationButtons = false;
                //this.CrystalReportViewer1.HasSearchButton = false;
                //this.CrystalReportViewer1.HasZoomFactorList = false;
                //this.CrystalReportViewer1.HasRefreshButton = false;
                //this.CrystalReportViewer1.HasDrillUpButton = false;
                //this.CrystalReportViewer1.HasToggleGroupTreeButton = false;
                //this.CrystalReportViewer1.DisplayGroupTree = false;
                //this.CrystalReportViewer1.SeparatePages = false;
                //this.CrystalReportViewer1.Visible = true;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Show();</script>");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightM", "<script>height();</script>");
            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            gridCashBank.DataSource = null;
            gridCashBank.DataBind();

            BindGrid();
        }
        protected void BindGrid()
        {
            DataSet dsCrystalPrint = new DataSet();
            //string BankName = txtBankName_hidden.Value;
            string BankName = Convert.ToString(txtBankName.SelectedItem.Text);
            string FromDate = dtDate.Value.ToString();
            string ToDate = dtToDate.Value.ToString();
            string Type = null;
            string Mode = null;
            if (chkAll.Checked == true)
                Type = "All";
            else
                Type = "No";
            if (rbtnSingle.Checked == true)
                Mode = "Single";
            else if (rbtnDouble.Checked == true)
                Mode = "Double";
            else if (rbtnTriple.Checked == true)
                Mode = "Triple";
            dsCrystalPrint = objfaReportOther.CashBankVoucherPrint(
                 Convert.ToString(BankName),
                Convert.ToString(FromDate),
                Convert.ToString(ToDate),
                Convert.ToString(Type),
              Convert.ToString(chkDebit.Checked),
               Convert.ToString(chkCredit.Checked),
                 Convert.ToString(chkContra.Checked),
                Convert.ToString(Session["usersegid"]),
                Convert.ToString(Mode),
                Convert.ToString(txtFromCheque.Text),
              Convert.ToString(txtToCheque.Text),
               Convert.ToString('F'),
                "",
              "show"
                 );
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("CashBankVoucherPrint", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@BankName", BankName);
            //cmd3.Parameters.AddWithValue("@FromDate", FromDate);
            //cmd3.Parameters.AddWithValue("@ToDate", ToDate);
            //cmd3.Parameters.AddWithValue("@Type", Type);
            //cmd3.Parameters.AddWithValue("@Debit", chkDebit.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Credit", chkCredit.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Contra", chkContra.Checked.ToString());
            //cmd3.Parameters.AddWithValue("@Segment", Session["usersegid"].ToString());
            //cmd3.Parameters.AddWithValue("@Mode", Mode);
            //cmd3.Parameters.AddWithValue("@FromChequeNumber", txtFromCheque.Text.Trim());
            //cmd3.Parameters.AddWithValue("@ToChequeNumber", txtToCheque.Text.Trim());
            //cmd3.Parameters.AddWithValue("@CallFromCashBankEntry", 'F');
            //cmd3.Parameters.AddWithValue("@IBRefString", String.Empty);
            //cmd3.Parameters.AddWithValue("@Typedata", "show");
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(dsCrystalPrint);
            //cmd3.Dispose();
            //con.Dispose();

            gridCashBank.DataSource = dsCrystalPrint.Tables[2];
            gridCashBank.DataBind();

            if (dsCrystalPrint.Tables[1].Rows.Count > 0)
                btnReport.Visible = true;
            else
                btnReport.Visible = false;


            //this.CrystalReportViewer1.Visible = false;

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightM", "<script>height();</script>");
        }
    }
}