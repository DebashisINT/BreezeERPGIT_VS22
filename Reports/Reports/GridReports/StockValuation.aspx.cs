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

namespace Reports.Reports.GridReports
{
    public partial class StockValuation : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        TotalvaluationClass objvaluation = new TotalvaluationClass();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/Totalvaluation.aspx");
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockValuation.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Valuation";
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
                //Session["dt_TotalvaluationRpt"] = null;
                //Session["dt_PartyLedgerRpt"] = null;
                Session["IsStockValFilter"] = null;
                Session["IsStockValDetFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                // Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                //lookupClass.DataSource = GetClassList();
                //lookupClass.DataBind();

                //lookupBrand.DataSource = GetBrandList();
                //lookupBrand.DataBind();

                //Rev Debashis
                if (ddlValTech.SelectedValue == "F")
                {
                    grivaluation.Columns[8].Visible = true;
                    grivaluation.Columns[9].Visible = true;
                    grivaluation.Columns[10].Visible = true;
                    grivaluation.Columns[11].Visible = true;
                    grivaluation.Columns[12].Visible = true;
                    grivaluation.Columns[13].Visible = false;
                    grivaluation.Columns[14].Visible = false;
                    grivaluation.Columns[15].Visible = false;
                    grivaluation.Columns[16].Visible = false;
                    grivaluation.Columns[17].Visible = false;
                    grivaluation.Columns[18].Visible = false;
                    grivaluation.Columns[19].Visible = false;
                    grivaluation.Columns[20].Visible = false;
                    grivaluation.Columns[21].Visible = false;
                    grivaluation.Columns[22].Visible = false;
                }
                else
                {
                    grivaluation.Columns[8].Visible = false;
                    grivaluation.Columns[9].Visible = true;
                    grivaluation.Columns[10].Visible = false;
                    grivaluation.Columns[11].Visible = true;
                    grivaluation.Columns[12].Visible = false;
                    grivaluation.Columns[13].Visible = true;
                    grivaluation.Columns[14].Visible = true;
                    grivaluation.Columns[15].Visible = true;
                    grivaluation.Columns[16].Visible = true;
                    grivaluation.Columns[17].Visible = true;
                    grivaluation.Columns[18].Visible = true;
                    grivaluation.Columns[19].Visible = true;
                    grivaluation.Columns[20].Visible = true;
                    grivaluation.Columns[21].Visible = true;
                    grivaluation.Columns[22].Visible = true;
                }
                //End of Rev Debashis
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
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
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //stbill = bll1.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
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

        #region Export Valuation Summary
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                //   drdExport.SelectedValue = "0";
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "StockValuation";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Stock Valuation Summary Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 20-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
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
            }

        }

        //Rev Subhra 20-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        #region Export Valuation Details
        public void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    // BindDropDownList();
                    bindexport1(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    // BindDropDownList();
                    bindexport1(Filter);
                }

                //      drdExport.SelectedValue = "0";
            }

        }


        public void bindexport1(int Filter)
        {
            string filename = "StockValuationdetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Valuation Details Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "grivaluation";
            exporter.RenderBrick += exporter_RenderBrick;
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
            }

        }
        #endregion




        #region =======================Valuation Summary =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_PartyLedgerRpt");
            //ShowGrid.JSProperties["cpSave"] = null;

            string IsStockValFilter = Convert.ToString(hfIsStockValFilter.Value);
            Session["IsStockValFilter"] = IsStockValFilter;

            //string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
            //if (hdnSelectedBranches.Value != "")
            //{
            //    BRANCH_ID = hdnSelectedBranches.Value;
            //}

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');

            string Product = "";
            //string QuoComponent = "";
            //List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo in QuoList)
            //{
            //    QuoComponent += "," + Quo;
            //}
            //CUSTVENDID = QuoComponent.TrimStart(',');
            Product = hdncWiseProductId.Value;

            ///  GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);


            //Rev Subhra 20-12-2018   0017670
            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
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

            Task PopulateStockTrialDataTask = new Task(() => GetStockValuationdata(FROMDATE, TODATE, BRANCH_ID, Product));
            PopulateStockTrialDataTask.RunSynchronously();

        }



        public void GetStockValuationdata(string FROMDATE, string TODATE, string BRANCH_ID, string ProductIds)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;                
                //DataTable ds = new DataTable();
                //List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                //foreach (object Class in ClassList)
                //{
                //    strClassList += "," + Class;
                //}
                //strClassList = strClassList.TrimStart(',');
                //List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                //foreach (object Brand in BrandList)
                //{
                //    strBrandList += "," + Brand;
                //}
                //strBrandList = strBrandList.TrimStart(',');
                //if (ds.Rows.Count > 0)
                //{
                //    Session["dt_PartyLedgerRpt"] = ds;
                //    ShowGrid.DataSource = ds;
                //    ShowGrid.DataBind();
                //}
                //ds = objvaluation.GetvaluationSummary(BRANCH_ID, ProductIds, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), FROMDATE, TODATE, "F", "Summary", strClassList, strBrandList, Convert.ToInt32(Session["userid"]));
                
                string strClassList = "", strBrandList = "";
                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ProductValuation_Report");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@GETTYPE", "Summary");
                //proc.AddPara("@val_type", "F");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@CONSOPASONDATE", (chkConsopasondt.Checked) ? "1" : "0");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@PAGELINK", "/GridReports/StockValuation.aspx");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }


        //protected void gridvaluationsummary_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_PartyLedgerRpt"] != null)
        //    {

        //        ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

        //    }
        //}

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion



        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}
            CommonBL stkval = new CommonBL();
            DataTable dtstkval = new DataTable();

            dtstkval = stkval.GetDateFinancila(Finyear);
            if (dtstkval.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

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


        //#region  ===========Product  Bind====================
        //protected void ComponentProduct_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];


        //        string type = e.Parameter.Split('~')[1];

        //        DataTable ComponentTable = new DataTable();

        //        //              ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name',sProducts_Hsncode as Hsn FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //        //"AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");

        //        if (e.Parameter.Split('~')[1] == "0" || e.Parameter.Split('~')[1] == "")
        //        {
        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' ,sProducts_Hsncode as Hsn FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //  "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");
        //        }

        //        else
        //        {

        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name',sProducts_Hsncode as Hsn  FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //                     "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) and ProductClass_Code in(" + e.Parameter.Split('~')[1] + ") ORDER BY sProducts_Description");

        //        }


        //        if (ComponentTable.Rows.Count > 0)
        //        {

        //            Session["SI_ComponentData"] = ComponentTable;
        //            lookup_quotation.DataSource = ComponentTable;
        //            lookup_quotation.DataBind();

        //        }
        //        else
        //        {
        //            Session["SI_ComponentData"] = null;
        //            lookup_quotation.DataSource = null;
        //            lookup_quotation.DataBind();

        //        }
        //    }
        //}

        //protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        //{
        //    //   DataTable ComponentTable = new DataTable();

        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}

        //#endregion


        #region =====================Valuation Details===========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsStockValDetFilter = Convert.ToString(hfIsStockValDetFilter.Value);
            Session["IsStockValDetFilter"] = IsStockValDetFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branch = returnPara.Split('~')[1];
                string prodId = returnPara.Split('~')[2];
                Getvaluation(prodId, branch);
            }

        }

        //protected void grid_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_TotalvaluationRpt"] != null)
        //    {

        //        grivaluation.DataSource = (DataTable)Session["dt_TotalvaluationRpt"];

        //    }
        //}

        public void Getvaluation(string ProductIds, string BRANCH_ID)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;
                string strClassList = "", strBrandList = "";

                //DataTable ds = new DataTable();
                //List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                //foreach (object Class in ClassList)
                //{
                //    strClassList += "," + Class;
                //}
                //strClassList = strClassList.TrimStart(',');
                
                //List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                //foreach (object Brand in BrandList)
                //{
                //    strBrandList += "," + Brand;
                //}
                //strBrandList = strBrandList.TrimStart(',');
                //if (ds.Rows.Count > 0)
                //{

                //    Session["dt_TotalvaluationRpt"] = ds;

                //    grivaluation.DataSource = ds;
                //    grivaluation.DataBind();
                //}

                //else
                //{

                //    Session["dt_TotalvaluationRpt"] = null;

                //    grivaluation.DataSource = null;
                //    grivaluation.DataBind();
                //}

                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ProductValuation_Report");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@GETTYPE", "Details");                
                //proc.AddPara("@val_type", "F");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@CONSOPASONDATE", (chkConsopasondt.Checked) ? "1" : "0");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@PAGELINK", "/GridReports/StockValuation.aspx");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
               
            }
            catch (Exception ex)
            {
            }


        }

        protected void ShowGrid1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion


        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                //if (Session["userbranchHierarchy"] != null)
                //{
                //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                //}

                //ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and  branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");
                if (Hoid != "All")
                {
                    //Rev Debashis && Hierarchy wise Branch Bind
                    //ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                    //End of Rev Debashis
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

        //Rev Debashis && Hierarchy wise Branch Bind
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
        //End of Rev Debashis

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        //public DataTable GetClassList()
        //{
        //    try
        //    {
        //        DataTable dt = oDBEngine.GetDataTable("Select ProductClass_ID,ProductClass_Name From Master_ProductClass Order By ProductClass_Name Asc");
        //        return dt;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public DataTable GetBrandList()
        //{
        //    try
        //    {
        //        DataTable dt = oDBEngine.GetDataTable("Select Brand_Id,Brand_Name From tbl_master_brand Where Brand_IsActive=1 Order By Brand_Name Asc");
        //        return dt;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //protected void lookupClass_DataBinding(object sender, EventArgs e)
        //{
        //    lookupClass.DataSource = GetClassList();
        //}

        //protected void lookupBrand_DataBinding(object sender, EventArgs e)
        //{
        //    lookupBrand.DataSource = GetBrandList();
        //}

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockValFilter"]) == "Y")
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && Convert.ToString(d.PAGELINK) == "/GridReports/StockValuation.aspx"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            //ShowGridList.ExpandAll();
        }
        protected void GenerateEntityServerDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockValDetFilter"]) == "Y")
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details" && Convert.ToString(d.PAGELINK) == "/GridReports/StockValuation.aspx"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            //ShowGridList.ExpandAll();
            //Rev Debashis
            string strlandcost = (chkConsLandCost.Checked) ? "1" : "0";

            if (ddlValTech.SelectedValue == "F")
            {
                grivaluation.Columns[8].Visible = true;
                grivaluation.Columns[9].Visible = true;
                grivaluation.Columns[10].Visible = true;
                if (Convert.ToString(strlandcost) == "1")
                {
                    grivaluation.Columns[11].Visible = true;
                }
                else if (Convert.ToString(strlandcost) == "0")
                {
                    grivaluation.Columns[11].Visible = false;
                }
                grivaluation.Columns[12].Visible = true;
                grivaluation.Columns[13].Visible = false;
                grivaluation.Columns[14].Visible = false;
                grivaluation.Columns[15].Visible = false;
                grivaluation.Columns[16].Visible = false;
                grivaluation.Columns[17].Visible = false;
                grivaluation.Columns[18].Visible = false;
                grivaluation.Columns[19].Visible = false;
                grivaluation.Columns[20].Visible = false;
                grivaluation.Columns[21].Visible = false;
                grivaluation.Columns[22].Visible = false;
            }
            else
            {
                grivaluation.Columns[8].Visible = false;
                grivaluation.Columns[9].Visible = true;
                grivaluation.Columns[10].Visible = false;
                if (Convert.ToString(strlandcost) == "1")
                {
                    grivaluation.Columns[11].Visible = true;
                }
                else if (Convert.ToString(strlandcost) == "0")
                {
                    grivaluation.Columns[11].Visible = false;
                }
                grivaluation.Columns[12].Visible = false;
                grivaluation.Columns[13].Visible = true;
                grivaluation.Columns[14].Visible = true;
                grivaluation.Columns[15].Visible = true;
                grivaluation.Columns[16].Visible = true;
                grivaluation.Columns[17].Visible = true;
                grivaluation.Columns[18].Visible = true;
                grivaluation.Columns[19].Visible = true;
                grivaluation.Columns[20].Visible = true;
                grivaluation.Columns[21].Visible = true;
                grivaluation.Columns[22].Visible = true;
            }
            //End of Rev Debashis
        }
        #endregion
    }
}