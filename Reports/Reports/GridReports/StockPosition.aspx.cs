#region =======================Revision History=========================
//1.0   v2 .0.35    Debashis    06/01/2023  Unable to view Detail Report in Stock Position if the Item name consists Double or single Quotes.
//                              Now it has been taken care of.Refer: 0025569
#endregion

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
using DataAccessLayer;
using BusinessLogicLayer.YearEnding;

namespace Reports.Reports.GridReports
{
    public partial class StockPosition : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable CompanyInfo = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtProductTotal = null;
        DataTable dtProductDetTotal = null;
        string ProductTotalOpQty = "";
        string ProductTotalDetOpQty = "";
        string ProductTotalInQty = "";
        string ProductTotalDetInQty = "";
        string ProductTotalOutQty = "";
        string ProductTotalDetOutQty = "";
        string ProductTotalBalQty = "";
        string ProductTotalDetBalQty = "";
        string ProductTotalOrdQty = "";
        string ProductTotalBalOrdQty = "";
        string ProductTotalDesc = "";
        string ProductTotalDetDesc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockPosition.aspx");
            if (!IsPostBack)
            {
                Session["chk_bransumtotal"] = 0;
                Session["chk_branwhdettotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Position";
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

                Session["SI_ComponentData"] = null;
                Session["IsBranWiseSumFilter"] = null;
                Session["IsBranWHWiseDetFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                Session["BranchNames"] = null;
            }
            else
            {

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
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

        #region Summary Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsBranWiseSumFilter"]) == "Y")
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
            string filename = "StockPositionSummary";
            exporter.FileName = filename;
            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PRODCODE,PRODDESC,CLASSDESC,HSNCODE,BRANDNAME,STOCKUOM,OP_QTY,IN_QTY,OUT_QTY,BALQTY,ORDQTY,BALORDQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE<>'Gross Total :' AND REPORTTYPE='Summary' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT PRODCODE,OP_QTY,IN_QTY,OUT_QTY,BALQTY,ORDQTY,BALORDQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE='Gross Total :' AND REPORTTYPE='Summary'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportbranstockdetdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Code", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Class", typeof(string)));
                dtExport.Columns.Add(new DataColumn("HSN", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Brand", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Stock Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Opening Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Received Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Delivered Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Booked Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance Qty. after booked Qty.", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Item Code"] = dr1["PRODCODE"];
                    row2["Item Name"] = dr1["PRODDESC"];
                    row2["Class"] = dr1["CLASSDESC"];
                    row2["HSN"] = dr1["HSNCODE"];
                    row2["Brand"] = dr1["BRANDNAME"];
                    row2["Stock Unit"] = dr1["STOCKUOM"];
                    row2["Opening Qty."] = dr1["OP_QTY"];
                    row2["Received Qty."] = dr1["IN_QTY"];
                    row2["Delivered Qty."] = dr1["OUT_QTY"];
                    row2["Balance Qty."] = dr1["BALQTY"];
                    row2["Booked Qty."] = dr1["ORDQTY"];
                    row2["Balance Qty. after booked Qty."] = dr1["BALORDQTY"];

                    dtExport.Rows.Add(row2);
                }                

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PRODCODE");
                dtExport.Columns.Remove("PRODDESC");
                dtExport.Columns.Remove("CLASSDESC");
                dtExport.Columns.Remove("HSNCODE");
                dtExport.Columns.Remove("BRANDNAME");
                dtExport.Columns.Remove("STOCKUOM");
                dtExport.Columns.Remove("OP_QTY");
                dtExport.Columns.Remove("IN_QTY");
                dtExport.Columns.Remove("OUT_QTY");
                dtExport.Columns.Remove("BALQTY");
                dtExport.Columns.Remove("ORDQTY");
                dtExport.Columns.Remove("BALORDQTY");

                DataRow row3 = dtExport.NewRow();
                row3["Item Code"] = ds.Tables[1].Rows[0]["PRODCODE"].ToString();
                row3["Opening Qty."] = ds.Tables[1].Rows[0]["OP_QTY"].ToString();
                row3["Received Qty."] = ds.Tables[1].Rows[0]["IN_QTY"].ToString();
                row3["Delivered Qty."] = ds.Tables[1].Rows[0]["OUT_QTY"].ToString();
                row3["Balance Qty."] = ds.Tables[1].Rows[0]["BALQTY"].ToString();
                row3["Booked Qty."] = ds.Tables[1].Rows[0]["ORDQTY"].ToString();
                row3["Balance Qty. after booked Qty."] = ds.Tables[1].Rows[0]["BALORDQTY"].ToString();
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
                HeaderRow6[0] = "Stock Position - Summary";
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
                    objExcel.ExportToExcelforExcel(dtExport, "StockPositionSummary", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "StockPositionSummary", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region Details Export
        public void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Convert.ToString(Session["IsBranWHWiseDetFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexportDetails(Filter);
                }
                else
                {
                    BranchHoOffice();
                }
            }
        }

        public void bindexportDetails(int Filter)
        {
            string filename = "StockPositionDetails";
            exporter.FileName = filename;
            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,WHDESC,PRODCODE,PRODDESC,CLASSDESC,HSNCODE,BRANDNAME,STOCKUOM,DOCNO,DOCDATE,TRANS_TYPE,PARTY,PARTYINVNO,PARTYINVDT,SERIALNO,OP_QTY,IN_QTY,OUT_QTY,BALQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE<>'Gross Total :' AND REPORTTYPE='Details' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT PRODCODE,OP_QTY,IN_QTY,OUT_QTY,BALQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE='Gross Total :' AND REPORTTYPE='Details'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportbranwhstockdetdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Warehouse", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Code", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Class", typeof(string)));
                dtExport.Columns.Add(new DataColumn("HSN", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Brand", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Stock Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document No", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Inv. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Inv. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Serial No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Opening Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Received Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Delivered Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance Qty.", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Warehouse"] = dr1["WHDESC"];
                    row2["Item Code"] = dr1["PRODCODE"];
                    row2["Item Name"] = dr1["PRODDESC"];
                    row2["Class"] = dr1["CLASSDESC"];
                    row2["HSN"] = dr1["HSNCODE"];
                    row2["Brand"] = dr1["BRANDNAME"];
                    row2["Stock Unit"] = dr1["STOCKUOM"];
                    row2["Document No"] = dr1["DOCNO"];
                    row2["Date"] = dr1["DOCDATE"];
                    row2["Document Type"] = dr1["TRANS_TYPE"];
                    row2["Party Name"] = dr1["PARTY"];
                    row2["Party Inv. No."] = dr1["PARTYINVNO"];
                    row2["Party Inv. Date"] = dr1["PARTYINVDT"];
                    row2["Serial No."] = dr1["SERIALNO"];
                    row2["Opening Qty."] = dr1["OP_QTY"];
                    row2["Received Qty."] = dr1["IN_QTY"];
                    row2["Delivered Qty."] = dr1["OUT_QTY"];
                    row2["Balance Qty."] = dr1["BALQTY"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("WHDESC");
                dtExport.Columns.Remove("PRODCODE");
                dtExport.Columns.Remove("PRODDESC");
                dtExport.Columns.Remove("CLASSDESC");
                dtExport.Columns.Remove("HSNCODE");
                dtExport.Columns.Remove("BRANDNAME");
                dtExport.Columns.Remove("STOCKUOM");
                dtExport.Columns.Remove("DOCNO");
                dtExport.Columns.Remove("DOCDATE");
                dtExport.Columns.Remove("TRANS_TYPE");
                dtExport.Columns.Remove("PARTY");
                dtExport.Columns.Remove("PARTYINVNO");
                dtExport.Columns.Remove("PARTYINVDT");
                dtExport.Columns.Remove("SERIALNO");
                dtExport.Columns.Remove("OP_QTY");
                dtExport.Columns.Remove("IN_QTY");
                dtExport.Columns.Remove("OUT_QTY");
                dtExport.Columns.Remove("BALQTY");

                DataRow row3 = dtExport.NewRow();
                row3["Item Code"] = ds.Tables[1].Rows[0]["PRODCODE"].ToString();
                row3["Opening Qty."] = ds.Tables[1].Rows[0]["OP_QTY"].ToString();
                row3["Received Qty."] = ds.Tables[1].Rows[0]["IN_QTY"].ToString();
                row3["Delivered Qty."] = ds.Tables[1].Rows[0]["OUT_QTY"].ToString();
                row3["Balance Qty."] = ds.Tables[1].Rows[0]["BALQTY"].ToString();
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
                HeaderRow6[0] = "Stock Position - Details";
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
                exporter.GridViewID = "ShowGridDetail";
            }

            switch (Filter)
            {
                case 1:
                    objExcel.ExportToExcelforExcel(dtExport, "StockPositionDetails", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "StockPositionDetails", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }
        #endregion

        #region =======================Branch wise Stock Data =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsBranWiseSumFilter = Convert.ToString(hfIsBranWiseSumFilter.Value);
            Session["IsBranWiseSumFilter"] = IsBranWiseSumFilter;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchList = "";
            List<object> BranchList1 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchList1)
            {
                BranchList += "," + Branch;
            }
            BRANCH_ID = BranchList.TrimStart(',');

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

            Task PopulateStockTrialDataTask = new Task(() => GetBranchWiseStockkdata(FROMDATE, TODATE, BRANCH_ID));
            PopulateStockTrialDataTask.RunSynchronously();

        }

        public void GetBranchWiseStockkdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", hdncWiseProductId.Value);
                proc.AddPara("@WAREHOUSE_ID", "");
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@SHOWPARTY", "0");
                proc.AddPara("@SHOWPARTYINVNODT","0");
                proc.AddPara("@SHOWSERIAL", "0");
                proc.AddPara("@ZOOMTYPE", "BranchSummary");
                proc.AddPara("@REPORTTYPE", "Summary");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsBranWiseSumFilter"]) == "Y")
            {
                dtProductTotal = oDBEngine.GetDataTable("SELECT PRODCODE,OP_QTY,IN_QTY,OUT_QTY,BALQTY,ORDQTY,BALORDQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE='Gross Total :' AND REPORTTYPE='Summary'");
                ProductTotalDesc = dtProductTotal.Rows[0][0].ToString();
                ProductTotalOpQty = dtProductTotal.Rows[0][1].ToString();
                ProductTotalInQty = dtProductTotal.Rows[0][2].ToString();
                ProductTotalOutQty = dtProductTotal.Rows[0][3].ToString();
                ProductTotalBalQty = dtProductTotal.Rows[0][4].ToString();
                ProductTotalOrdQty = dtProductTotal.Rows[0][5].ToString();
                ProductTotalBalOrdQty = dtProductTotal.Rows[0][6].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Prod":
                        e.Text = ProductTotalDesc;
                        break;
                    case "Item_OpQty":
                        e.Text = ProductTotalOpQty;
                        break;
                    case "Item_InQty":
                        e.Text = ProductTotalInQty;
                        break;
                    case "Item_OutQty":
                        e.Text = ProductTotalOutQty;
                        break;
                    case "Item_BalQty":
                        e.Text = ProductTotalBalQty;
                        break;
                    case "Item_OrdQty":
                        e.Text = ProductTotalOrdQty;
                        break;
                    case "Item_BalOrdQty":
                        e.Text = ProductTotalBalOrdQty;
                        break;
                }
            }
        }

        protected void ShowGrid_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Item Code")
            {
                e.Cell.Style["text-align"] = "right";
            }
        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue).Contains("Total of : "))
            {
                Session["chk_bransumtotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_bransumtotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BALORDQTY")
            {
                Session["chk_bransumtotal"] = 0;
            }
        }
        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkdb = new CommonBL();
            DataTable dtstkdb = new DataTable();

            dtstkdb = stkdb.GetDateFinancila(Finyear);
            if (dtstkdb.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_StartDate"]));

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
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                }

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
        

        #region Summary LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBranWiseSumFilter"]) == "Y")
            {
                var q = from d in dc.BRANCHWHWISESTOCKPOSITIONDETSUM_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && Convert.ToString(d.PRODCODE) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BRANCHWHWISESTOCKPOSITIONDETSUM_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            
            //if (chkParty.Checked == true)
            //{
            //    ShowGrid.Columns[16].Visible = true;
            //}
            //else
            //{
            //    ShowGrid.Columns[16].Visible = false;
            //}

            //if (chkPartyInvNoDt.Checked == true)
            //{
            //    ShowGrid.Columns[17].Visible = true;
            //    ShowGrid.Columns[18].Visible = true;
            //}
            //else
            //{
            //    ShowGrid.Columns[17].Visible = false;
            //    ShowGrid.Columns[18].Visible = false;
            //}
            
            //if (chkSerial.Checked == true)
            //{
            //    ShowGrid.Columns[21].Visible = true;
            //}
            //else
            //{
            //    ShowGrid.Columns[21].Visible = false;
            //}
        }
        #endregion

        #region =======================Branch/Warehouse wise Stock Data =========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsBranWHWiseDetFilter = Convert.ToString(hfIsBranWHWiseDetFilter.Value);
            Session["IsBranWHWiseDetFilter"] = IsBranWHWiseDetFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];
            string BranchId = returnPara.Split('~')[1];
            string ProdId = returnPara.Split('~')[2];
            //Rev 1.0
            //string BranchDesc = returnPara.Split('~')[3];
            //string ProdDesc = returnPara.Split('~')[4];
            DataTable dtBranchSelection = new DataTable();
            DataTable dtProductSelection = new DataTable();
            dtBranchSelection = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where CONVERT(NVARCHAR(20),branch_id)='" + BranchId + "'");
            string BranchDesc = dtBranchSelection.Rows[0][0].ToString();
            dtProductSelection = oDBEngine.GetDataTable("Select sProducts_Name from Master_sProducts Where CONVERT(NVARCHAR(20),sProducts_ID)='" + ProdId + "'");
            string ProdDesc = dtProductSelection.Rows[0][0].ToString();
            //End of Rev 1.0

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            CallbackPanelDetail.JSProperties["cpBranchDesc"] = Convert.ToString(BranchDesc);
            CallbackPanelDetail.JSProperties["cpProdDesc"] = Convert.ToString(ProdDesc);
            CallbackPanelDetail.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
            CallbackPanelDetail.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");

            if (WhichCall == "BndPopupgrid")
            {
                GetBranchWHWiseStkDetdata(FROMDATE, TODATE, BranchId, ProdId);
            }

        }

        public void GetBranchWHWiseStkDetdata(string FROMDATE, string TODATE, string BranchId, string ProdId)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BranchId);
                proc.AddPara("@PRODUCT_ID", ProdId);
                proc.AddPara("@WAREHOUSE_ID", "");
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@SHOWPARTY", "1");
                proc.AddPara("@SHOWPARTYINVNODT", "1");
                proc.AddPara("@SHOWSERIAL", "1");
                proc.AddPara("@ZOOMTYPE", "BranchWHDet");
                proc.AddPara("@REPORTTYPE", "Details");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGridDetail_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsBranWHWiseDetFilter"]) == "Y")
            {
                dtProductDetTotal = oDBEngine.GetDataTable("Select PRODCODE,OP_QTY,IN_QTY,OUT_QTY,BALQTY FROM BRANCHWHWISESTOCKPOSITIONDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PRODCODE='Gross Total :' AND REPORTTYPE='Details'");
                ProductTotalDetDesc = dtProductDetTotal.Rows[0][0].ToString();
                ProductTotalDetOpQty = dtProductDetTotal.Rows[0][1].ToString();
                ProductTotalDetInQty = dtProductDetTotal.Rows[0][2].ToString();
                ProductTotalDetOutQty = dtProductDetTotal.Rows[0][3].ToString();
                ProductTotalDetBalQty = dtProductDetTotal.Rows[0][4].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_DetProd":
                        e.Text = ProductTotalDetDesc;
                        break;
                    case "Item_DetOpQty":
                        e.Text = ProductTotalDetOpQty;
                        break;
                    case "Item_DetInQty":
                        e.Text = ProductTotalDetInQty;
                        break;
                    case "Item_DetOutQty":
                        e.Text = ProductTotalDetOutQty;
                        break;
                    case "Item_DetBalQty":
                        e.Text = ProductTotalDetBalQty;
                        break;
                }
            }
        }

        protected void ShowGridDetail_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Item Code")
            {
                e.Cell.Style["text-align"] = "right";
            }
        }

        protected void ShowGridDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue).Contains("Total of : "))
            {
                Session["chk_branwhdettotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_branwhdettotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BALQTY")
            {
                Session["chk_branwhdettotal"] = 0;
            }
        }

        #endregion

        #region Details LinQ
        protected void GenerateEntityServerDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBranWHWiseDetFilter"]) == "Y")
            {
                var q = from d in dc.BRANCHWHWISESTOCKPOSITIONDETSUM_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details" && Convert.ToString(d.PRODCODE) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BRANCHWHWISESTOCKPOSITIONDETSUM_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

    }
}