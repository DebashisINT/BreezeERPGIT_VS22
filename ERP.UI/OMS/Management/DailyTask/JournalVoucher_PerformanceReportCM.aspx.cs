using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_JournalVoucher_PerformanceReportCM : ERP.OMS.ViewState_class.VSPage
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        static DataSet ds = new DataSet();

        JournalVoucher_PerformanceReportCMBL OJournalVoucher_PerformanceReportCMBL = new JournalVoucher_PerformanceReportCMBL();

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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>Page_Load();</script>");

            if (!IsPostBack)
            {
                date();
                segmentfetch();
            }

        }

        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtValuationDate.EditFormatString = oconverter.GetDateFormat("Date");
            dtValuationDate.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
        }
        void segmentfetch()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_companyexchange", " DISTINCT case when exch_exchid='EXN0000002'  then 'NSE-CM' when exch_exchid='EXB0000001'  then 'BSE-CM' ELSE 'Accounts' END AS SEGMENT,EXCH_INTERNALID", "EXCH_COMPID='" + Session["LastCompany"].ToString() + "' and ISNULL(EXCH_SEGMENTID,'CM')='CM'");
            if (dt.Rows.Count > 0)
            {
                ddlgeneratein.DataSource = dt;
                ddlgeneratein.DataTextField = "SEGMENT";
                ddlgeneratein.DataValueField = "EXCH_INTERNALID";
                ddlgeneratein.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["Segment"] == null)
                        ViewState["Segment"] = dt.Rows[i][1].ToString();
                    else
                        ViewState["Segment"] += "," + dt.Rows[i][1].ToString();
                }
            }

        }
        void fetchclients()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_contact", "distinct cnt_internalid", "cnt_clienttype in('Pro Trading') and cnt_internalid like 'CL%'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["Clients"] == null)
                        ViewState["Clients"] = "'" + dt.Rows[i][0].ToString() + "'";
                    else
                        ViewState["Clients"] += "," + "'" + dt.Rows[i][0].ToString() + "'";
                }
            }
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
        }
        void procedure()
        {

            /* For Tier Structure

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                fetchclients();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[PerformanceReportCM_JournalVoucher]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@segment", ViewState["Segment"].ToString());
                cmd.Parameters.AddWithValue("@fromdate", dtfor.Value);
                cmd.Parameters.AddWithValue("@todate", "NA");
                cmd.Parameters.AddWithValue("@clients", ViewState["Clients"].ToString());
                cmd.Parameters.AddWithValue("@Seriesid", "ALL");
                cmd.Parameters.AddWithValue("@finyear", HttpContext.Current.Session["LastFinYear"]);
                cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                cmd.Parameters.AddWithValue("@rpttype", "1");
                cmd.Parameters.AddWithValue("@PRINTCHK", "SHOW");
                cmd.Parameters.AddWithValue("@clienttype", "cnt_clienttype='Pro Trading'");
                cmd.Parameters.AddWithValue("@closemethod", ddlclosingstock.SelectedItem.Value.ToString().Trim());
                cmd.Parameters.AddWithValue("@jvcreate", (chkConsBillDate.Checked) ? "YES~CALLBILL" : "YES");
                cmd.Parameters.AddWithValue("@SEGMENTJV", ddlgeneratein.SelectedItem.Value.ToString().Trim());
                cmd.Parameters.AddWithValue("@chkexcludesqr", "CHK");
                cmd.Parameters.AddWithValue("@chkstt", "CHK");
                cmd.Parameters.AddWithValue("@ValuationDate", dtValuationDate.Value);
                cmd.Parameters.AddWithValue("@CreateUser", HttpContext.Current.Session["userid"]);
                cmd.Parameters.AddWithValue("@ReportView", "N");

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();

                */


            fetchclients();
            string vYes;
            if ((chkConsBillDate.Checked) == true)
            {
                vYes = "YES~CALLBILL";
            }
            else
            {
                vYes = "YES";
            }

            ds = OJournalVoucher_PerformanceReportCMBL.PerformanceReportCM_JournalVoucher(Session["LastCompany"].ToString(), ViewState["Segment"].ToString(),
                           dtfor.Value.ToString(), "NA", ViewState["Clients"].ToString(),
                           "ALL", HttpContext.Current.Session["LastFinYear"].ToString(), "BRANCH", "1", "SHOW", "cnt_clienttype='Pro Trading'",
                           ddlclosingstock.SelectedItem.Value.ToString().Trim(), vYes, ddlgeneratein.SelectedItem.Value.ToString().Trim(), "CHK", "CHK",
                           dtValuationDate.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), "N");




            JournalVoucherCreate(ds);
            //}

        }
        void JournalVoucherCreate(DataSet ds)
        {
            DataTable DtJournalVoucher = new DataTable();
            if (ds != null)
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DtJournalVoucher = ds.Tables[0].Copy();
                        ds.Dispose();

                        string journalData = oconverter.ConvertDataTableToXML(DtJournalVoucher);/////////////////DATATABLE TO CONVERT XML DATA

                        /* For Tier Structure------------
                    
                        String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                        SqlConnection con = new SqlConnection(conn);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("[xmlJournalVoucherInsert_Update]", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@journalData", journalData);
                        cmd.Parameters.AddWithValue("@createuser", Convert.ToInt32(Session["userid"].ToString()));
                        cmd.Parameters.AddWithValue("@finyear", HttpContext.Current.Session["LastFinYear"]);
                        cmd.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                        cmd.Parameters.AddWithValue("@JournalVoucher_Narration", "");
                        cmd.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", dtfor.Value);
                        cmd.Parameters.AddWithValue("@JournalVoucher_SettlementNumber ", "");
                        cmd.Parameters.AddWithValue("@JournalVoucher_SettlementType", "");
                        cmd.Parameters.AddWithValue("@JournalVoucher_BillNumber", "");
                        cmd.Parameters.AddWithValue("@JournalVoucher_Prefix", "NA");
                        cmd.Parameters.AddWithValue("@segmentid", ddlgeneratein.SelectedItem.Value.ToString());
                                       
                        ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        cmd.CommandTimeout = 0;
                        ds.Reset();
                        da.Fill(ds);
                        da.Dispose();

                        */


                        ds = OJournalVoucher_PerformanceReportCMBL.xmlJournalVoucherInsert_Update(journalData, (Session["userid"].ToString()), HttpContext.Current.Session["LastFinYear"].ToString(),
                        Session["LastCompany"].ToString(), "", dtfor.Value.ToString(), "", "", "", "NA", ddlgeneratein.SelectedItem.Value.ToString());


                        ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Generated Successfully !!');", true);
                        return;
                    }

            ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('No Record Found To Generate. !!');", true);
        }
    }
}