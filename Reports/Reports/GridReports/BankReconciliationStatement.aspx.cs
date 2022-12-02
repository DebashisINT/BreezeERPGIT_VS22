using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class BankReconciliationStatement : System.Web.UI.Page
    {
       BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
       public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        decimal TotalPayment = 0, TotalReceipt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/BankReconciliationStatement.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Bank Reconciliation Statement";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                lookupBank.DataBind();

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                DataTable dt = oDBEngine.GetDataTable("Select FinYear_Code,FinYear_StartDate,FinYear_EndDate From Master_FinYear Where FinYear_Code='" + strFinYear + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strStartDate = Convert.ToString(dt.Rows[0]["FinYear_StartDate"]);
                    DateTime StartDate = Convert.ToDateTime(strStartDate);

                    ASPxFromDate.Value = DateTime.Now;
                    ASPxToDate.Value = DateTime.Now;
                }
                else
                {
                    ASPxFromDate.Value = DateTime.Now;
                    ASPxToDate.Value = DateTime.Now;
                }
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                Session["IsBRS"] = null;

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }

        }        
        
        public DataTable GetBankList()
        {
            string query = "";

            try
            {
                query = @"Select MainAccount_ReferenceID AS ID, MainAccount_Name as Name
                    from Master_MainAccount WHERE MainAccount_BankCashType='Bank' AND MainAccount_PaymentType<>'Card' ORDER BY MAINACCOUNT_REFERENCEID";

                DataTable dt = oDBEngine.GetDataTable(query);
                return dt;
            }
            catch
            {
                return null;
            }
        }       

        protected void lookupBank_DataBinding(object sender, EventArgs e)
        {
            lookupBank.DataSource = GetBankList();
        }

        protected void BankPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            lookupBank.GridView.Selection.CancelSelection();
            lookupBank.DataSource = GetBankList();
            lookupBank.DataBind();
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        public void bindexport(int Filter)
        {
            string filename = "Bank Reconciliation Statement";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Bank Reconciliation Statement" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.RenderBrick += exporter_RenderBrick;
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "ShowGrid";
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
                default:
                    return;
            }
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsBRS = Convert.ToString(hfIsBRS.Value);
            Session["IsBRS"] = IsBRS;
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            
            Task PopulateStockTrialDataTask = new Task(() => GetBRS(FROMDATE, TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetBRS(string FROMDATE, string TODATE)
        {
            string BankIDList = "";
            string strCompany = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");

            List<object> BankList = lookupBank.GridView.GetSelectedFieldValues("ID");
            foreach (object bankIDs in BankList)
            {
                BankIDList += "," + bankIDs;
            }
            BankIDList = BankIDList.TrimStart(',');

            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("PRC_BANKRECONCILIATIONLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", strCompany);
                cmd.Parameters.AddWithValue("@FINYEAR", strFinYear);
                cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
                cmd.Parameters.AddWithValue("@TODATE", strToDate);
                cmd.Parameters.AddWithValue("@BANKID", BankIDList);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

            }
            catch
            {
                //return null;
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
           if (e.Item.FieldName == "RECEIVEDFROM_PAIDTO")
            {
                e.Text = "Net Total";
            }           
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }


            //Group Summary Calculation
            ASPxGridView grid = sender as ASPxGridView;
            if (e.IsGroupSummary)
            {
                string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
                switch (summaryTAG)
                {
                    case "Item_Payment":
                        e.Text = string.Format("{0}", e.Value);
                        TotalPayment = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_Receipt":
                        e.Text = string.Format("{0}", e.Value);
                        TotalReceipt = Convert.ToDecimal(e.Value);
                        break;
                }               
            }

            //Group Summary Calculation
        }
       

        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBRS"]) == "Y")
            {
                var q = from d in dc.BANKRECONCILIATIONSTATEMENT_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BANKRECONCILIATIONSTATEMENT_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }    
    }
}