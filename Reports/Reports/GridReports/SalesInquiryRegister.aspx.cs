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
    public partial class SalesInquiryRegister : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtINQTotal = null;
        string INQTotalBalDesc = "";
        string INQQty = "";
        string INQMQty = "";
        string INQGrossAmt = "";
        string INQCAmt = "";
        string INQSAmt = "";
        string INQIAmt = "";
        string INQUAmt = "";
        string INQTAmt = "";
        string INQOAmt="";
        string INQTotAmt = "";

        protected void Page_PreInit(object sender, EventArgs e)
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SalesInquiryRegister.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    Label2.Visible = true;
                    ShowGrid.Columns[1].Visible = true;
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    Label2.Visible = false;
                    ShowGrid.Columns[1].Visible = false;
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Sales Inquiry Register";
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
                Session["IsSaleINQRegDetSumFilter"] = null;
                BranchHoOffice();

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }


            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsSaleINQRegDetSumFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
            else
            {
                BranchHoOffice();
            }
        }

        public void bindexport(int Filter)
        {

            string filename = "SalesInquiryRegister";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHNAME,PROJ_NAME,PARTYNAME,CITY_NAME,STATE,COU_COUNTRY,SHIP_TO_PARTY,INQNO,INQDATE,SALESMAN_NAME,REFERENCE,QONO,QODATE,ORDNO,ORDDATE,PRODUCTDESC,PRODUCTCLASS_NAME,QTY,STOCKUOM,MULTIUOM,MULTQTY,CONVFACTOR,RATE,GROSAMOUNT,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHER_AMT,TAX_MISC,TOTALAMOUNT,CREATEDBY FROM SALESINQUIRYREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ<>999999999999 AND BRANCHNAME<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select BRANCHNAME,QTY,MULTQTY,GROSAMOUNT,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHER_AMT,TAX_MISC,TOTALAMOUNT from SALESINQUIRYREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND BRANCHNAME='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportinqregsumdetdataset"] = ds;

                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Customer Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("City", typeof(string)));
                dtExport.Columns.Add(new DataColumn("State", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Country", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ship to Party Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Inquiry No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Salesman Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Reference", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Pro./Quo. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Pro./Quo. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Class", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Multi UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Multi Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Conv. Factor", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Rate", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Value", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("CGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("SGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("IGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UTGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Other Charges(Line)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Tax Misc.(Global)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Total Value", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Created by", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHNAME"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["Customer Name"] = dr1["PARTYNAME"];
                    row2["City"] = dr1["CITY_NAME"];
                    row2["State"] = dr1["STATE"];
                    row2["Country"] = dr1["COU_COUNTRY"];
                    row2["Ship to Party Name"] = dr1["SHIP_TO_PARTY"];
                    row2["Inquiry No."] = dr1["INQNO"];
                    row2["Date"] = dr1["INQDATE"];
                    row2["Salesman Name"] = dr1["SALESMAN_NAME"];
                    row2["Reference"] = dr1["REFERENCE"];
                    row2["Pro./Quo. No."] = dr1["QONO"];
                    row2["Pro./Quo. Date"] = dr1["QODATE"];
                    row2["Order No."] = dr1["ORDNO"];
                    row2["Order Date"] = dr1["ORDDATE"];
                    row2["Item Name"] = dr1["PRODUCTDESC"];
                    row2["Item Class"] = dr1["PRODUCTCLASS_NAME"];
                    row2["Qty."] = dr1["QTY"];
                    row2["UOM"] = dr1["STOCKUOM"];
                    row2["Multi UOM"] = dr1["MULTIUOM"];
                    row2["Multi Qty."] = dr1["MULTQTY"];
                    row2["Conv. Factor"] = dr1["CONVFACTOR"];
                    row2["Rate"] = dr1["RATE"];
                    row2["Value"] = dr1["GROSAMOUNT"];
                    row2["CGST"] = dr1["CGST_AMT"];
                    row2["SGST"] = dr1["SGST_AMT"];
                    row2["IGST"] = dr1["IGST_AMT"];
                    row2["UTGST"] = dr1["UTGST_AMT"];
                    row2["Other Charges(Line)"] = dr1["OTHER_AMT"];
                    row2["Tax Misc.(Global)"] = dr1["TAX_MISC"];
                    row2["Total Value"] = dr1["TOTALAMOUNT"];
                    row2["Created by"] = dr1["CREATEDBY"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");
                if(chkSummary.Checked==true)
                {
                    dtExport.Columns.Remove("Item Name");
                    dtExport.Columns.Remove("Item Class");
                    dtExport.Columns.Remove("Qty.");
                    dtExport.Columns.Remove("UOM");
                    dtExport.Columns.Remove("Rate");
                }
                if(chkMUOM.Checked==false)
                {
                    dtExport.Columns.Remove("Multi UOM");
                    dtExport.Columns.Remove("Multi Qty.");
                    dtExport.Columns.Remove("Conv. Factor");
                }
                if(chkCreateBy.Checked==false)
                    dtExport.Columns.Remove("Created by");

                dtExport.Columns.Remove("BRANCHNAME");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("CITY_NAME");
                dtExport.Columns.Remove("STATE");
                dtExport.Columns.Remove("COU_COUNTRY");
                dtExport.Columns.Remove("SHIP_TO_PARTY");
                dtExport.Columns.Remove("INQNO");
                dtExport.Columns.Remove("INQDATE");
                dtExport.Columns.Remove("SALESMAN_NAME");
                dtExport.Columns.Remove("REFERENCE");
                dtExport.Columns.Remove("QONO");
                dtExport.Columns.Remove("QODATE");
                dtExport.Columns.Remove("ORDNO");
                dtExport.Columns.Remove("ORDDATE");
                dtExport.Columns.Remove("PRODUCTDESC");
                dtExport.Columns.Remove("PRODUCTCLASS_NAME");
                dtExport.Columns.Remove("QTY");
                dtExport.Columns.Remove("STOCKUOM");
                dtExport.Columns.Remove("MULTIUOM");
                dtExport.Columns.Remove("MULTQTY");
                dtExport.Columns.Remove("CONVFACTOR");
                dtExport.Columns.Remove("RATE");
                dtExport.Columns.Remove("GROSAMOUNT");
                dtExport.Columns.Remove("CGST_AMT");
                dtExport.Columns.Remove("SGST_AMT");
                dtExport.Columns.Remove("IGST_AMT");
                dtExport.Columns.Remove("UTGST_AMT");
                dtExport.Columns.Remove("OTHER_AMT");
                dtExport.Columns.Remove("TAX_MISC");
                dtExport.Columns.Remove("TOTALAMOUNT");
                dtExport.Columns.Remove("CREATEDBY");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHNAME"].ToString();
                if (chkSummary.Checked == false)
                    row3["Qty."] = ds.Tables[1].Rows[0]["QTY"].ToString();
                if (chkMUOM.Checked == true)
                    row3["Multi Qty."] = ds.Tables[1].Rows[0]["MULTQTY"].ToString();
                row3["Value"] = ds.Tables[1].Rows[0]["GROSAMOUNT"].ToString();
                row3["CGST"] = ds.Tables[1].Rows[0]["CGST_AMT"].ToString();
                row3["SGST"] = ds.Tables[1].Rows[0]["SGST_AMT"].ToString();
                row3["IGST"] = ds.Tables[1].Rows[0]["IGST_AMT"].ToString();
                row3["UTGST"] = ds.Tables[1].Rows[0]["UTGST_AMT"].ToString();
                row3["Other Charges(Line)"] = ds.Tables[1].Rows[0]["OTHER_AMT"].ToString();
                row3["Tax Misc.(Global)"] = ds.Tables[1].Rows[0]["TAX_MISC"].ToString();
                row3["Total Value"] = ds.Tables[1].Rows[0]["TOTALAMOUNT"].ToString();
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
                HeaderRow6[0] = "Sales Inquiry Register";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
                exporter.GridViewID = "ShowGrid";
            }
            switch (Filter)
            {
                case 1:
                    objExcel.ExportToExcelforExcel(dtExport, "SalesInquiryRegister", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "SalesInquiryRegister", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region Sales Inquiry Register Details/Summary grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsSaleINQRegDetSumFilter = Convert.ToString(hfIsSaleINQRegDetSumFilter.Value);
            Session["IsSaleINQRegDetSumFilter"] = IsSaleINQRegDetSumFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object branch in BranchList)
            {
                BranchComponent += "," + branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count == 0)
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

            Task PopulateStockTrialDataTask = new Task(() => GetSalesINQRegisterdata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetSalesINQRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SALESINQUIRYREGISTERDETSUM_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdnProductId.Value);
                cmd.Parameters.AddWithValue("@SHOWMUOM", (chkMUOM.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWSUMMARY", (chkSummary.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWCREATEBY", (chkCreateBy.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
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

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsSaleINQRegDetSumFilter"]) == "Y")
            {
                dtINQTotal = oDBEngine.GetDataTable("Select BRANCHNAME,QTY,MULTQTY,GROSAMOUNT,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHER_AMT,TAX_MISC,TOTALAMOUNT from SALESINQUIRYREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND BRANCHNAME='Total :'");
                INQTotalBalDesc = dtINQTotal.Rows[0][0].ToString();
                INQQty = dtINQTotal.Rows[0][1].ToString();
                INQMQty = dtINQTotal.Rows[0][2].ToString();
                INQGrossAmt = dtINQTotal.Rows[0][3].ToString();
                INQCAmt = dtINQTotal.Rows[0][4].ToString();
                INQSAmt = dtINQTotal.Rows[0][5].ToString();
                INQIAmt = dtINQTotal.Rows[0][6].ToString();
                INQUAmt = dtINQTotal.Rows[0][7].ToString();
                INQOAmt = dtINQTotal.Rows[0][8].ToString();
                INQTAmt  = dtINQTotal.Rows[0][9].ToString();
                INQTotAmt = dtINQTotal.Rows[0][10].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Branch":
                        e.Text = INQTotalBalDesc;
                        break;
                    case "Qty":
                        e.Text = INQQty;
                        break;
                    case "MQty":
                        e.Text = INQMQty;
                        break;
                    case "GrossAmt":
                        e.Text = INQGrossAmt;
                        break;
                    case "CAmt":
                        e.Text = INQCAmt;
                        break;
                    case "SAmt":
                        e.Text = INQSAmt;
                        break;
                    case "IAmt":
                        e.Text = INQIAmt;
                        break;
                    case "UAmt":
                        e.Text = INQUAmt;
                        break;
                    case "TAmt":
                        e.Text = INQOAmt;
                        break;
                    case "OAmt":
                        e.Text = INQTAmt;
                        break;
                    case "TotAmt":
                        e.Text = INQTotAmt;
                        break;
                }
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSaleINQRegDetSumFilter"]) == "Y")
            {
                var q = from d in dc.SALESINQUIRYREGISTERDETSUM_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SEQ) != "999999999999" && Convert.ToString(d.BRANCHNAME) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESINQUIRYREGISTERDETSUM_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if (chkSummary.Checked == true)
            {
                ShowGrid.Columns[15].Visible = false;
                ShowGrid.Columns[16].Visible = false;
                ShowGrid.Columns[17].Visible = false;
                ShowGrid.Columns[18].Visible = false;
                ShowGrid.Columns[19].Visible = false;
                ShowGrid.Columns[20].Visible = false;
                ShowGrid.Columns[21].Visible = false;
                ShowGrid.Columns[22].Visible = false;
            }
            else
            {
                ShowGrid.Columns[15].Visible = true;
                ShowGrid.Columns[16].Visible = true;
                ShowGrid.Columns[17].Visible = true;
                ShowGrid.Columns[18].Visible = true;
                ShowGrid.Columns[19].Visible = true;
                ShowGrid.Columns[20].Visible = true;
                ShowGrid.Columns[21].Visible = true;
                ShowGrid.Columns[22].Visible = true;
            }

            if (chkMUOM.Checked == true)
            {
                ShowGrid.Columns[19].Visible = true;
                ShowGrid.Columns[20].Visible = true;
                ShowGrid.Columns[21].Visible = true;
            }
            else
            {
                ShowGrid.Columns[19].Visible = false;
                ShowGrid.Columns[20].Visible = false;
                ShowGrid.Columns[21].Visible = false;
            }

            if (chkCreateBy.Checked == false)
            {
                ShowGrid.Columns[31].Visible = false;
            }
            else
            {
                ShowGrid.Columns[31].Visible = true;
            }
        }

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL salereg = new CommonBL();
            DataTable dtsalereg = new DataTable();

            dtsalereg = salereg.GetDateFinancila(Finyear);
            if (dtsalereg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

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

        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

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

        #region Branch Populate

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
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        #region Project Populate
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
        #endregion

    }
}