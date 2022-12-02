using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;


namespace ERP.OMS.Reports
{

    public partial class Reports_JournalVoucherPrint : System.Web.UI.Page
    {
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                dtToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            string Fromdate = dtDate.Value.ToString();
            string ToDate = dtToDate.Value.ToString();
            string Type = null;
            if (radAll.Checked == true)
                Type = "NA";
            else
                Type = txtAccountCode.Text;




            dsCrystal = objFAReportsOther.JournalVoucherPrint(
                     Convert.ToString(Fromdate),
                    Convert.ToString(ToDate),
                    Convert.ToString(Session["usersegid"]),
                    Convert.ToString(Type),
                "show"
                     );

            gridJournalVouchar.DataSource = dsCrystal.Tables[1]; 
            gridJournalVouchar.DataBind();

            Page.ClientScript.RegisterStartupScript(GetType(), "JSht", "<script language='JavaScript'>height();</script>");


        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string SelectedIds = "";
            for (int i = 0; i < gridJournalVouchar.VisibleRowCount; i++)
            {
                //DataView dr= gridJournalVouchar.GetDataRow(i);//.GetRow(i);
                if (gridJournalVouchar.Selection.IsRowSelected(i))
                {
                    if (SelectedIds == "")
                        SelectedIds = gridJournalVouchar.GetRowValues(i, "AccountsLedger_TransactionReferenceID").ToString();
                    else
                        SelectedIds += "," + gridJournalVouchar.GetRowValues(i, "AccountsLedger_TransactionReferenceID").ToString();
                }

            }

            if (SelectedIds == "")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "JSalert", "<script>alert('Please Select Any Voucher !');</script>");
            }
            else
            {

                DataSet dsCrystal = new DataSet();
                string Fromdate = dtDate.Value.ToString();
                string ToDate = dtToDate.Value.ToString();
                string Type = null;
                if (radAll.Checked == true)
                    Type = "NA";
                else
                    Type = txtAccountCode.Text;

                dsCrystal = objFAReportsOther.JournalVoucherPrintSelected(
                   Convert.ToString(Fromdate),
                  Convert.ToString(ToDate),
                  Convert.ToString(Session["usersegid"]),
                  Convert.ToString(Type),
               Convert.ToString(SelectedIds)
                   );

                //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Journal.xsd");
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                ReportDocument reportObj = new ReportDocument();
                string ReportPath = Server.MapPath("..\\Reports\\Journal.rpt");
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
                //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);

                dsCrystal.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//journal.xsd");
                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Journal");
                reportObj.Dispose();
                GC.Collect();
            }
        }
    }
}