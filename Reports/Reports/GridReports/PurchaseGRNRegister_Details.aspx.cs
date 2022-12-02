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
    public partial class PurchaseGRNRegister_Details : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //Rev Debashis
        CommonBL cbl = new CommonBL();
        //End of Rev Debashis

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtUnitTotal = null;
        string PCTotalDesc = "";
        string PCTotalQty = "";
        string PCTotalVal = "";
        string PCTotalCGST = "";
        string PCTotalSGST = "";
        string PCTotalIGST = "";
        string PCTotalUTGST = "";
        string PCTotalOTHS = "";
        string PCTotalTM = "";
        string PCTotalTV = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/PurchaseGRNRegister_Details.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            //Rev Debashis
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    ShowGrid.Columns[1].Visible = true;
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    ShowGrid.Columns[1].Visible = false;
                    hdnProjectSelection.Value = "0";
                }
            }
            //End of Rev Debashis
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                //Rev Debashis
                DataTable dtProjectSelection = new DataTable();
                //End of Rev Debashis
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Purchase GRN Register - Detail";
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
                Session["IsPurchaseGRNRegDetFilter"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
                //Rev Debashis
                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
                //End of Rev Debashis
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

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsPurchaseGRNRegDetFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "PurchaseGRNRegisterDetails";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PROJ_NAME,VENDOR_NAME,CITY_NAME,STATE,COU_COUNTRY,SHIP_TO_PARTY,PCNO,PCDATE,PARTYINVNO,PARTYINVDT,PONO,PODATE,PINVNO,PINVDATE,PRODDESC,QUANTITY,PCRATE,PCVALUE,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHS_AMT,TAXMISC,PCTOTALVALUE,EWAYBILLNO,EWAYBILLDT FROM PURCHASEGRNREGISTER_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID<>999999999999 AND BRANCHDESC<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT BRANCHDESC,QUANTITY,PCVALUE,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHS_AMT,TAXMISC,PCTOTALVALUE from PURCHASEGRNREGISTER_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportgrnregdetdataset"] = ds;
                //Rev Debashis
                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                //End of Rev Debashis

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                //Rev Debashis
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                //End of Rev Debashis
                dtExport.Columns.Add(new DataColumn("Vendor Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("City", typeof(string)));
                dtExport.Columns.Add(new DataColumn("State", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Country", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ship to Party Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("GRN No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("GRN Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Inv. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Inv. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("PO No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("PO Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("PI No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("PI Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Descprition", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Rate", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Purc. Value", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("CGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("SGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("IGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UTGST", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Other Charges", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Tax Misc.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Total Value", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("E-Way bill Number", typeof(string)));
                dtExport.Columns.Add(new DataColumn("E-Way bill Date", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    //Rev Debashis
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    //End of Rev Debashis
                    row2["Vendor Name"] = dr1["VENDOR_NAME"];
                    row2["City"] = dr1["CITY_NAME"];
                    row2["State"] = dr1["STATE"];
                    row2["Country"] = dr1["COU_COUNTRY"];
                    row2["Ship to Party Name"] = dr1["SHIP_TO_PARTY"];
                    row2["GRN No."] = dr1["PCNO"];
                    row2["GRN Date"] = dr1["PCDATE"];
                    row2["Party Inv. No."] = dr1["PARTYINVNO"];
                    row2["Party Inv. Date"] = dr1["PARTYINVDT"];
                    row2["PO No."] = dr1["PONO"];
                    row2["PO Date"] = dr1["PODATE"];
                    row2["PI No."] = dr1["PINVNO"];
                    row2["PI Date"] = dr1["PINVDATE"];
                    row2["Item Descprition"] = dr1["PRODDESC"];
                    row2["Qty."] = dr1["QUANTITY"];
                    row2["Rate"] = dr1["PCRATE"];
                    row2["Purc. Value"] = dr1["PCVALUE"];
                    row2["CGST"] = dr1["CGST_AMT"];
                    row2["SGST"] = dr1["SGST_AMT"];
                    row2["IGST"] = dr1["IGST_AMT"];
                    row2["UTGST"] = dr1["UTGST_AMT"];
                    row2["Other Charges"] = dr1["OTHS_AMT"];
                    row2["Tax Misc."] = dr1["TAXMISC"];
                    row2["Total Value"] = dr1["PCTOTALVALUE"];
                    row2["E-Way bill Number"] = dr1["EWAYBILLNO"];
                    row2["E-Way bill Date"] = dr1["EWAYBILLDT"];

                    dtExport.Rows.Add(row2);
                }

                //Rev Debashis
                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");
                //End of Rev Debashis

                dtExport.Columns.Remove("BRANCHDESC");
                //Rev Debashis
                dtExport.Columns.Remove("PROJ_NAME");
                //End of Rev Debashis
                dtExport.Columns.Remove("VENDOR_NAME");
                dtExport.Columns.Remove("CITY_NAME");
                dtExport.Columns.Remove("STATE");
                dtExport.Columns.Remove("COU_COUNTRY");
                dtExport.Columns.Remove("SHIP_TO_PARTY");
                dtExport.Columns.Remove("PCNO");
                dtExport.Columns.Remove("PCDATE");
                dtExport.Columns.Remove("PARTYINVNO");
                dtExport.Columns.Remove("PARTYINVDT");
                dtExport.Columns.Remove("PONO");
                dtExport.Columns.Remove("PODATE");
                dtExport.Columns.Remove("PINVNO");
                dtExport.Columns.Remove("PINVDATE");
                dtExport.Columns.Remove("PRODDESC");
                dtExport.Columns.Remove("QUANTITY");
                dtExport.Columns.Remove("PCRATE");
                dtExport.Columns.Remove("PCVALUE");
                dtExport.Columns.Remove("CGST_AMT");
                dtExport.Columns.Remove("SGST_AMT");
                dtExport.Columns.Remove("IGST_AMT");
                dtExport.Columns.Remove("UTGST_AMT");
                dtExport.Columns.Remove("OTHS_AMT");
                dtExport.Columns.Remove("TAXMISC");
                dtExport.Columns.Remove("PCTOTALVALUE");
                dtExport.Columns.Remove("EWAYBILLNO");
                dtExport.Columns.Remove("EWAYBILLDT");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["Qty."] = ds.Tables[1].Rows[0]["QUANTITY"].ToString();
                row3["Purc. Value"] = ds.Tables[1].Rows[0]["PCVALUE"].ToString();
                row3["CGST"] = ds.Tables[1].Rows[0]["CGST_AMT"].ToString();
                row3["SGST"] = ds.Tables[1].Rows[0]["SGST_AMT"].ToString();
                row3["IGST"] = ds.Tables[1].Rows[0]["IGST_AMT"].ToString();
                row3["UTGST"] = ds.Tables[1].Rows[0]["UTGST_AMT"].ToString();
                row3["Other Charges"] = ds.Tables[1].Rows[0]["OTHS_AMT"].ToString();
                row3["Tax Misc."] = ds.Tables[1].Rows[0]["TAXMISC"].ToString();
                row3["Total Value"] = ds.Tables[1].Rows[0]["PCTOTALVALUE"].ToString();
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
                HeaderRow6[0] = "Purchase GRN Register - Detail";
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
                    objExcel.ExportToExcelforExcel(dtExport, "PurchaseGRNRegisterDetails", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "PurchaseGRNRegisterDetails", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsPurchaseGRNRegDetFilter = Convert.ToString(hfIsPurchaseGRNRegDetFilter.Value);
            Session["IsPurchaseGRNRegDetFilter"] = IsPurchaseGRNRegDetFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranComponent = "";
            List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Bran in BranList)
            {
                BranComponent += "," + Bran;
            }
            BRANCH_ID = BranComponent.TrimStart(',');

            //Rev Debashis
            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');
            //End of Rev Debashis

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

            Task PopulateStockTrialDataTask = new Task(() => GetPurchaseGRNRegisterdata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPurchaseGRNRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PURCHASEGRNREGISTER_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@P_BRANCHID", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTYCODE", hdnVendorId.Value);
                cmd.Parameters.AddWithValue("@ISINVENTORY", ddlisinventory.SelectedValue);
                cmd.Parameters.AddWithValue("@PARTYDATE", chkpartyInvDate.Checked);
                //Rev Debashis
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                //End of Rev Debashis
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
            if (Convert.ToString(Session["IsPurchaseGRNRegDetFilter"]) == "Y")
            {
                dtUnitTotal = oDBEngine.GetDataTable("SELECT BRANCHDESC,QUANTITY,PCVALUE,CGST_AMT,SGST_AMT,IGST_AMT,UTGST_AMT,OTHS_AMT,TAXMISC,PCTOTALVALUE from PURCHASEGRNREGISTER_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'");
                PCTotalDesc = dtUnitTotal.Rows[0][0].ToString();
                PCTotalQty = dtUnitTotal.Rows[0][1].ToString();
                PCTotalVal = dtUnitTotal.Rows[0][2].ToString();
                PCTotalCGST = dtUnitTotal.Rows[0][3].ToString();
                PCTotalSGST = dtUnitTotal.Rows[0][4].ToString();
                PCTotalIGST = dtUnitTotal.Rows[0][5].ToString();
                PCTotalUTGST = dtUnitTotal.Rows[0][6].ToString();
                PCTotalOTHS = dtUnitTotal.Rows[0][7].ToString();
                PCTotalTM = dtUnitTotal.Rows[0][8].ToString();
                PCTotalTV = dtUnitTotal.Rows[0][9].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "TagUnit":
                        e.Text = PCTotalDesc;
                        break;
                    case "TagQty":
                        e.Text = PCTotalQty;
                        break;
                    case "TagVal":
                        e.Text = PCTotalVal;
                        break;
                    case "TagCGST":
                        e.Text = PCTotalCGST;
                        break;
                    case "TagSGST":
                        e.Text = PCTotalSGST;
                        break;
                    case "TagIGST":
                        e.Text = PCTotalIGST;
                        break;
                    case "TagUTGST":
                        e.Text = PCTotalUTGST;
                        break;
                    case "TagOTHS":
                        e.Text = PCTotalOTHS;
                        break;
                    case "TagTM":
                        e.Text = PCTotalTM;
                        break;
                    case "TagPCTV":
                        e.Text = PCTotalTV;
                        break;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL purchreg = new CommonBL();
            DataTable dtpurchreg = new DataTable();

            dtpurchreg = purchreg.GetDateFinancila(Finyear);
            if (dtpurchreg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtpurchreg.Rows[0]["FinYear_StartDate"]));

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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPurchaseGRNRegDetFilter"]) == "Y")
            {
                var q = from d in dc.PURCHASEGRNREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PURCHASEGRNREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion
        #region Project Selection
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