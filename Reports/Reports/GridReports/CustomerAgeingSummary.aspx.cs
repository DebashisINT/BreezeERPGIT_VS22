#region =======================Revision History=========================================================================================================
//1.0   v2 .0.42    Debashis    26/03/2024  Customer code column is required in various reports.Refer: 0027273
#endregion=======================End Revision History====================================================================================================
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class CustomerAgeingSummary : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        CommonBL cbl = new CommonBL();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //Rev Debashis
        ExcelFile objExcel = new ExcelFile();
        DataTable CompanyInfo = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        //End of Rev Debashis

        DataTable dtPartyTotal = null;

        string PartyTotalDocAmt = "";
        string PartyTotalDays30Amt = "";
        string PartyTotalDays60Amt = "";
        string PartyTotalDays90Amt = "";
        string PartyTotalDays120Amt = "";
        string PartyTotalDays150Amt = "";
        string PartyTotalDays180Amt = "";
        string PartyTotalDays180AAmt = "";
        string PartyTotalCumBalAmt = "";
        string PartyTotalBalDesc = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/CustomerAgeingSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGridCustAgeing.Columns[3].Visible = true;
                    ShowGridCustAgeing.Columns[4].Visible = true;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGridCustAgeing.Columns[3].Visible = false;
                    ShowGridCustAgeing.Columns[4].Visible = false;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Ageing - Summary";
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

                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();
                Session["IsCustAgeSummFilter"] = null;
                Session["exportval"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //chkallcust.Attributes.Add("OnClick", "CustAll('allcust')");
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            }


            if (!IsPostBack)
            {
                //Session.Remove("dt_CustomerOutDetRpt");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                //string strduedatechk = (chkduedate.Checked) ? "1" : "0";
                //string strprintdatechk = (chkprintdays.Checked) ? "1" : "0";
                //if (Convert.ToString(strduedatechk) == "0")
                //{
                //    ShowGridCustAgeing.Columns[5].Visible = false;
                //}
                //else
                //{
                //    ShowGridCustAgeing.Columns[5].Visible = true;
                //}

                //if (Convert.ToString(strprintdatechk) == "0")
                //{
                //    ShowGridCustAgeing.Columns[8].Visible = false;
                //}
                //else
                //{
                //    ShowGridCustAgeing.Columns[8].Visible = true;
                //}
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {
                ASPxAsOnDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }
            }

        }
        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Rev Debashis
            //if (Filter != 0)
            //{

            //    BranchHoOffice();
            //    if (Session["exportval"] == null)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //}
            if (Convert.ToString(Session["IsCustAgeSummFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
            else { 
                BranchHoOffice(); 
            }
            //End of Rev Debashis
        }
        public void bindexport(int Filter)
        {

            string filename = "Customer Ageing-Summary";
            exporter.FileName = filename;
            //Rev Debashis
            //string FileHeader = "";

            //BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Customer Ageing-Summary" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
            ////Rev Subhra 24-12-2018   0017670
            //FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            ////End of Rev
            //exporter.PageHeader.Left = FileHeader;
            //exporter.PageHeader.Font.Size = 10;
            //exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.GridViewID = "ShowGridCustAgeing";
            //exporter.RenderBrick += exporter_RenderBrick;
            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                //Rev 1.0 Mantis: 0027273
                //string selectQuery = "SELECT PARTYNAME,BRANCH_DESCRIPTION,DOC_TYPE,PROJ_NAME,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DOC_AMOUNT,'(',CASE WHEN SUBSTRING(DOC_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS DOC_AMOUNT,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS30,'(',CASE WHEN SUBSTRING(DAYS30,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS30,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS60,'(',CASE WHEN SUBSTRING(DAYS60,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS60,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS90,'(',CASE WHEN SUBSTRING(DAYS90,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS90,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS120,'(',CASE WHEN SUBSTRING(DAYS120,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS120,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS150,'(',CASE WHEN SUBSTRING(DAYS150,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS150,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180,'(',CASE WHEN SUBSTRING(DAYS180,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180A,'(',CASE WHEN SUBSTRING(DAYS180A,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180A,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(CUMBAL_AMOUNT,'(',CASE WHEN SUBSTRING(CUMBAL_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS CUMBAL_AMOUNT FROM PARTYWISEAGEINGSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO<>999999999 AND DOC_TYPE<>'Net Total:' AND REPORT_TYPE='Summary' AND PARTYTYPE='C' order by PARTYID";
                string selectQuery = "SELECT PARTYCODE,PARTYNAME,BRANCH_DESCRIPTION,DOC_TYPE,PROJ_NAME,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DOC_AMOUNT,'(',CASE WHEN SUBSTRING(DOC_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS DOC_AMOUNT,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS30,'(',CASE WHEN SUBSTRING(DAYS30,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS30,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS60,'(',CASE WHEN SUBSTRING(DAYS60,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS60,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS90,'(',CASE WHEN SUBSTRING(DAYS90,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS90,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS120,'(',CASE WHEN SUBSTRING(DAYS120,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS120,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS150,'(',CASE WHEN SUBSTRING(DAYS150,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS150,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180,'(',CASE WHEN SUBSTRING(DAYS180,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180A,'(',CASE WHEN SUBSTRING(DAYS180A,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180A,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(CUMBAL_AMOUNT,'(',CASE WHEN SUBSTRING(CUMBAL_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS CUMBAL_AMOUNT FROM PARTYWISEAGEINGSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO<>999999999 AND DOC_TYPE<>'Net Total:' AND REPORT_TYPE='Summary' AND PARTYTYPE='C' order by PARTYID";
                //End of Rev 1.0 Mantis: 0027273
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT DOC_TYPE,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DOC_AMOUNT,'(',CASE WHEN SUBSTRING(DOC_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS DOC_AMOUNT,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS30,'(',CASE WHEN SUBSTRING(DAYS30,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS30,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS60,'(',CASE WHEN SUBSTRING(DAYS60,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS60,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS90,'(',CASE WHEN SUBSTRING(DAYS90,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS90,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS120,'(',CASE WHEN SUBSTRING(DAYS120,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS120,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS150,'(',CASE WHEN SUBSTRING(DAYS150,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS150,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180,'(',CASE WHEN SUBSTRING(DAYS180,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DAYS180A,'(',CASE WHEN SUBSTRING(DAYS180A,1,1)='(' THEN '-' ELSE '' END),')','')) AS DAYS180A,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(CUMBAL_AMOUNT,'(',CASE WHEN SUBSTRING(CUMBAL_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS CUMBAL_AMOUNT FROM PARTYWISEAGEINGSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Net Total:' AND REPORT_TYPE='Summary' AND PARTYTYPE='C'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportcustagesumdataset"] = ds;

                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                //Rev 1.0 Mantis: 0027273
                dtExport.Columns.Add(new DataColumn("Code", typeof(string)));
                //End of Rev 1.0 Mantis: 0027273
                dtExport.Columns.Add(new DataColumn("Customer Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. Amt.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("0-30 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("31-60 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("61-90 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("91-120 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("121-150 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("151-180 Days", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("181 & Above", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    //Rev 1.0 Mantis: 0027273
                    row2["Code"] = dr1["PARTYCODE"];
                    //End of Rev 1.0 Mantis: 0027273
                    row2["Customer Name"] = dr1["PARTYNAME"];
                    row2["Unit"] = dr1["BRANCH_DESCRIPTION"];
                    row2["Doc. Type"] = dr1["DOC_TYPE"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["Doc. Amt."] = dr1["DOC_AMOUNT"];
                    row2["0-30 Days"] = dr1["DAYS30"];
                    row2["31-60 Days"] = dr1["DAYS60"];
                    row2["61-90 Days"] = dr1["DAYS90"];
                    row2["91-120 Days"] = dr1["DAYS120"];
                    row2["121-150 Days"] = dr1["DAYS150"];
                    row2["151-180 Days"] = dr1["DAYS180"];
                    row2["181 & Above"] = dr1["DAYS180A"];
                    row2["Balance"] = dr1["CUMBAL_AMOUNT"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");

                //Rev 1.0 Mantis: 0027273
                dtExport.Columns.Remove("PARTYCODE");
                //End of Rev 1.0 Mantis: 0027273
                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("BRANCH_DESCRIPTION");
                dtExport.Columns.Remove("DOC_TYPE");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("DOC_AMOUNT");
                dtExport.Columns.Remove("DAYS30");
                dtExport.Columns.Remove("DAYS60");
                dtExport.Columns.Remove("DAYS90");
                dtExport.Columns.Remove("DAYS120");
                dtExport.Columns.Remove("DAYS150");
                dtExport.Columns.Remove("DAYS180");
                dtExport.Columns.Remove("DAYS180A");
                dtExport.Columns.Remove("CUMBAL_AMOUNT");

                DataRow row3 = dtExport.NewRow();
                row3["Doc. Type"] = ds.Tables[1].Rows[0]["DOC_TYPE"].ToString();
                row3["Doc. Amt."] = ds.Tables[1].Rows[0]["DOC_AMOUNT"].ToString();
                row3["0-30 Days"] = ds.Tables[1].Rows[0]["DAYS30"].ToString();
                row3["31-60 Days"] = ds.Tables[1].Rows[0]["DAYS60"].ToString();
                row3["61-90 Days"] = ds.Tables[1].Rows[0]["DAYS90"].ToString();
                row3["91-120 Days"] = ds.Tables[1].Rows[0]["DAYS120"].ToString();
                row3["121-150 Days"] = ds.Tables[1].Rows[0]["DAYS150"].ToString();
                row3["151-180 Days"] = ds.Tables[1].Rows[0]["DAYS180"].ToString();
                row3["181 & Above"] = ds.Tables[1].Rows[0]["DAYS180A"].ToString();
                row3["Balance"] = ds.Tables[1].Rows[0]["CUMBAL_AMOUNT"].ToString();
                dtExport.Rows.Add(row3);

                //For Excel/PDF Header
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String)));

                string GridHeader = "";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                HeaderRow1[0] = Convert.ToString(Session["BranchNames"]);
                dtReportHeader.Rows.Add(HeaderRow1);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                HeaderRow2[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow2);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                DataRow HeaderRow3 = dtReportHeader.NewRow();
                HeaderRow3[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow3);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                DataRow HeaderRow4 = dtReportHeader.NewRow();
                HeaderRow4[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow4);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                DataRow HeaderRow5 = dtReportHeader.NewRow();
                HeaderRow5[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow5);
                DataRow HeaderRow6 = dtReportHeader.NewRow();
                HeaderRow6[0] = "Customer Ageing-Summary";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "As On: " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);

                //For Excel/PDF Footer
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
            }
            else
            {
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.GridViewID = "ShowGridCustAgeing";
            }
            //End of Rev Debashis

            switch (Filter)
            {
                case 1:
                    //Rev Debashis
                    //exporter.WritePdfToResponse();
                    objExcel.ExportToExcelforExcel(dtExport, "Customer Ageing-Summary", "Party Total:", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    //End of Rev Debashis
                    break;
                case 2:
                    //Rev Debashis
                    //exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    objExcel.ExportToPDF(dtExport, "Customer Ageing-Summary", "Party Total:", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    //End of Rev Debashis
                    break;
                //Rev Debashis
                //case 3:
                //    exporter.WriteRtfToResponse();
                //    break;
                //case 4:
                case 3:
                    //End of Rev Debashis
                    exporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        //Rev Debashis
        ////Rev Subhra 24-12-2018   0017670
        //public string ReplaceFirst(string text, string search, string replace)
        //{
        //    int pos = text.IndexOf(search);
        //    if (pos < 0)
        //    {
        //        return text;
        //    }
        //    return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        //}
        ////End of Rev
        //protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        //{
        //    e.BrickStyle.BackColor = Color.White;
        //    e.BrickStyle.ForeColor = Color.Black;
        //}
        //End of Rev Debashis
        #endregion

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //tcbl = cbl.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }
        //End of Rev Debashis

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                    
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion



        #region Customer Outstanding grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];
            //Session.Remove("dt_CustomerOutDetRpt");

            string IsCustAgeSummFilter = Convert.ToString(hfIsCustAgeSummFilter.Value);
            Session["IsCustAgeSummFilter"] = IsCustAgeSummFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            //dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
            //string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Brnch in BranchList)
            {
                BranchComponent += "," + Brnch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            if (Convert.ToString(lookup_project.Value) != null)
            {
                PROJECT_ID = Convert.ToString(lookup_project.Value);
            }

            string PARTY_TYPE = "C";
            //string DUEDATE = (chkduedate.Checked) ? "1" : "0";
            string ALLPARTY = (chkallcust.Checked) ? "1" : "0";
            //string PRINT_DAYS = (chkprintdays.Checked) ? "1" : "0";
            string CBVOUCHER = (chkcb.Checked) ? "1" : "0";
            string JVOUCHER = (chkjv.Checked) ? "1" : "0";
            string DNCNNOTE = (chkdncn.Checked) ? "1" : "0";

            //Rev Subhra 24-12-2018   0017670
            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count==0)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);
            //End of Rev

            Task PopulateStockTrialDataTask = new Task(() => GetCustAgeingSummarydata(ASONDATE, BRANCH_ID, HEAD_BRANCH, PARTY_TYPE, ALLPARTY, CBVOUCHER, JVOUCHER, DNCNNOTE, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
            //ShowGridCustAgeing.ExpandAll();
        }
        public void GetCustAgeingSummarydata(string ASONDATE, string BRANCH_ID, string HEAD_BRANCH, string PARTY_TYPE, string ALLPARTY, string CBVOUCHER, string JVOUCHER, string DNCNNOTE, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYWISEAGEINGSUMDET_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@ALLPARTY", ALLPARTY);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@AGEDAYS", ddldays.SelectedValue);
                cmd.Parameters.AddWithValue("@PARTY_TYPE", PARTY_TYPE);
                cmd.Parameters.AddWithValue("@INCLUDECB", CBVOUCHER);
                cmd.Parameters.AddWithValue("@INCLUDEJV", JVOUCHER);
                cmd.Parameters.AddWithValue("@EXCLUDEDNCN", DNCNNOTE);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@REPORT_TYPE", "Summary");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridCustAgeing_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsCustAgeSummFilter"]) == "Y")
            {

                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,DOC_AMOUNT,DAYS30,DAYS60,DAYS90,DAYS120,DAYS150,DAYS180,DAYS180A,CUMBAL_AMOUNT from PARTYWISEAGEINGSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Net Total:' AND REPORT_TYPE='Summary' AND PARTYTYPE='C'");
                
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalDocAmt = dtPartyTotal.Rows[0][1].ToString();
                PartyTotalDays30Amt = dtPartyTotal.Rows[0][2].ToString();
                PartyTotalDays60Amt = dtPartyTotal.Rows[0][3].ToString();
                PartyTotalDays90Amt = dtPartyTotal.Rows[0][4].ToString();
                PartyTotalDays120Amt = dtPartyTotal.Rows[0][5].ToString();
                PartyTotalDays150Amt = dtPartyTotal.Rows[0][6].ToString();
                PartyTotalDays180Amt = dtPartyTotal.Rows[0][7].ToString();
                PartyTotalDays180AAmt = dtPartyTotal.Rows[0][8].ToString();
                PartyTotalCumBalAmt = dtPartyTotal.Rows[0][9].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                   
                    case "Item_Doc_Amount":
                        e.Text = PartyTotalDocAmt;
                        break;
                    case "Item_Days30":
                        e.Text = PartyTotalDays30Amt;
                        break;
                    case "Item_Days60":
                        e.Text = PartyTotalDays60Amt;
                        break;
                    case "Item_Days90":
                        e.Text = PartyTotalDays90Amt;
                        break;
                    case "Item_Days120":
                        e.Text = PartyTotalDays120Amt;
                        break;
                    case "Item_Days150":
                        e.Text = PartyTotalDays150Amt;
                        break;
                    case "Item_Days180":
                        e.Text = PartyTotalDays180Amt;
                        break;
                    case "Item_Days180A":
                        e.Text = PartyTotalDays180AAmt;
                        break;
                    case "Item_BalAmt":
                        e.Text = PartyTotalCumBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCustAgeSummFilter"]) == "Y")
            {
                var q = from d in dc.PARTYWISEAGEINGSUMDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" && Convert.ToString(d.REPORT_TYPE) == "Summary" && Convert.ToString(d.PARTYTYPE) == "C"
                        //orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYWISEAGEINGSUMDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0" && Convert.ToString(d.REPORT_TYPE) == "Summary"
                        //orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if (ddldays.SelectedValue == "All")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = true;
                //ShowGridCustAgeing.Columns[6].Visible = true;
                //ShowGridCustAgeing.Columns[7].Visible = true;
                //ShowGridCustAgeing.Columns[8].Visible = true;
                //ShowGridCustAgeing.Columns[9].Visible = true;
                //ShowGridCustAgeing.Columns[10].Visible = true;
                //ShowGridCustAgeing.Columns[11].Visible = true;
                ShowGridCustAgeing.Columns[6].Visible = true;
                ShowGridCustAgeing.Columns[7].Visible = true;
                ShowGridCustAgeing.Columns[8].Visible = true;
                ShowGridCustAgeing.Columns[9].Visible = true;
                ShowGridCustAgeing.Columns[10].Visible = true;
                ShowGridCustAgeing.Columns[11].Visible = true;
                ShowGridCustAgeing.Columns[12].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D30")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = true;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = true;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D60")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = true;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = true;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D90")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = true;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = true;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D120")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = true;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = true;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D150")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = true;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = true;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D180")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = true;
                //ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = true;
                ShowGridCustAgeing.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else if (ddldays.SelectedValue == "D180A")
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAgeing.Columns[5].Visible = false;
                //ShowGridCustAgeing.Columns[6].Visible = false;
                //ShowGridCustAgeing.Columns[7].Visible = false;
                //ShowGridCustAgeing.Columns[8].Visible = false;
                //ShowGridCustAgeing.Columns[9].Visible = false;
                //ShowGridCustAgeing.Columns[10].Visible = false;
                //ShowGridCustAgeing.Columns[11].Visible = true;
                ShowGridCustAgeing.Columns[6].Visible = false;
                ShowGridCustAgeing.Columns[7].Visible = false;
                ShowGridCustAgeing.Columns[8].Visible = false;
                ShowGridCustAgeing.Columns[9].Visible = false;
                ShowGridCustAgeing.Columns[10].Visible = false;
                ShowGridCustAgeing.Columns[11].Visible = false;
                ShowGridCustAgeing.Columns[12].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            ShowGridCustAgeing.ExpandAll();
        }
        #endregion

        protected void ShowGridCustAgeing_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }

        protected void ShowGridCustAgeing_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if(e.DataColumn.FieldName=="DOC_TYPE")
            {
                if (Convert.ToString(e.CellValue) == "Party Total:")
                {
                    e.Cell.Font.Bold = true;
                }
            }

        }

        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }
    }
}