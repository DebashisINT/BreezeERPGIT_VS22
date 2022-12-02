using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmDebitCreditNote : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.FAReportsOther oFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                date();
                //dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                //dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                //dtDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                //dtToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
                btnPrint.Visible = false;

            }

        }

        void date()
        {


            ////dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            ////dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            ////DateTime first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            ////DateTime last = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            ////dtFrom.Value = Convert.ToDateTime(first);
            ////dtTo.Value = Convert.ToDateTime(last);


            ///////////////
            string[] FinYear = Session["LastFinYear"].ToString().Split('-');
            // dtDate.EditFormatString = oconverter.GetDateFormat("Date");
            dtDate.EditFormatString = oconverter.GetDateFormat("Date");
            dtToDate.EditFormatString = oconverter.GetDateFormat("Date");

            // dtDate.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());
            dtDate.Value = Convert.ToDateTime(Session["FinYearStart"].ToString());
            dtToDate.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());


            //////////////////

        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string Fromdate = dtDate.Value.ToString();
            string ToDate = dtToDate.Value.ToString();
            //string voucherno=txtVoucherno.Text.ToString();
            string CompanyID = HttpContext.Current.Session["LastCompany"].ToString();
            string UsersegId = HttpContext.Current.Session["usersegid"].ToString();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            DataSet ds = new DataSet();
            string path = "";
            string Type = null;
            if (radAll.Checked == true)
                Type = "";
            else
                Type = txtAccountCode.Text;
            ds = oFAReportsOther.Debit_Note(
                Convert.ToString(CompanyID),
                 Convert.ToString(Fromdate),
                 Convert.ToString(ToDate),
                 Convert.ToString(UsersegId),
                  "NA",
                 Convert.ToString(Type)
                );
            gridJournalVouchar.DataSource = ds.Tables[0];
            gridJournalVouchar.DataBind();
            btnPrint.Visible = true;

            Page.ClientScript.RegisterStartupScript(GetType(), "JSht", "<script language='JavaScript'>Page_Load();</script>");

            //////////// ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\DebitNote.xsd");
            //////////// ReportDocument ICEXDoc = new ReportDocument();
            //////////// path = HttpContext.Current.Server.MapPath("..\\Reports\\DebitNote.rpt");
            ////////////ICEXDoc.Load(path);
            ////////////ICEXDoc.SetDataSource(ds.Tables[0]);
            ////////////ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "DebitCredit Report");
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            string Fromdate = dtDate.Value.ToString();
            string ToDate = dtToDate.Value.ToString();
            //string voucherno=txtVoucherno.Text.ToString();
            string CompanyID = HttpContext.Current.Session["LastCompany"].ToString();
            string UsersegId = HttpContext.Current.Session["usersegid"].ToString();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            DataSet ds = new DataSet();
            string path = "";
            string SelectedIds = "";
            for (int i = 0; i < gridJournalVouchar.VisibleRowCount; i++)
            {
                //DataView dr= gridJournalVouchar.GetDataRow(i);//.GetRow(i);
                if (gridJournalVouchar.Selection.IsRowSelected(i))
                {
                    if (SelectedIds == "")
                        SelectedIds = gridJournalVouchar.GetRowValues(i, "JournalVoucherDetail_ID").ToString();
                    else
                        SelectedIds += "," + gridJournalVouchar.GetRowValues(i, "JournalVoucherDetail_ID").ToString();
                }

            }
            if (SelectedIds == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('Please select Debit/Credit Note!');</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JSht1", "<script language='JavaScript'>Page_Load();</script>");
            }
            else
            {


                //DataSet ds1 = new DataSet();
                //string Fromdate = dtDate.Value.ToString();
                //string ToDate = dtToDate.Value.ToString();
                ds = oFAReportsOther.Debit_Note(
             Convert.ToString(CompanyID),
              Convert.ToString(Fromdate),
              Convert.ToString(ToDate),
              Convert.ToString(UsersegId),
               SelectedIds,
             ""
             );


                ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\DebitNote.xsd");
                ReportDocument ICEXDoc = new ReportDocument();
                path = HttpContext.Current.Server.MapPath("..\\Reports\\DebitNote.rpt");
                ICEXDoc.Load(path);
                ICEXDoc.SetDataSource(ds.Tables[0]);
                if (user.Checked)
                {
                    ICEXDoc.SetParameterValue("@user", (object)"CHK".ToString().Trim());
                }
                else
                {
                    ICEXDoc.SetParameterValue("@user", (object)"UNCHK".ToString().Trim());
                }
                if (time.Checked)
                {
                    ICEXDoc.SetParameterValue("@time", (object)"CHK".ToString().Trim());
                }
                else
                {
                    ICEXDoc.SetParameterValue("@time", (object)"UNCHK".ToString().Trim());
                }
                ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "DebitCredit Report");



            }

        }
    }

}