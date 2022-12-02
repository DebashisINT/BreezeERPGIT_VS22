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
    public partial class StockTrialSummary : System.Web.UI.Page
    {

        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/StockTrialSummary.aspx");
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockTrialSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Trial Summary";
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

                Session["IsStockTrialSummaryFilter"] = null;
                Session["exportval"] = null;
                //   BindDropDownList();                
                //Session["dt_PartyLedgerRpt"] = null;
                //cmbbranch.DataSource = CmbBranch();
                //cmbbranch.DataBind();
                //cmbbranch.Value = Convert.ToString(Session["userbranchID"]);
                ///Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

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
                //string TABLENAME = "Ledger Details";

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                BranchHoOffice();
                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                //string CASHBANKTYPE = "";
                //string CASHBANKID = "";
                //string UserId = "";
                //string PRODUCTID = "";
                //if (hdnSelectedCustomerVendor.Value != "")
                //{
                //    PRODUCTID = hdnSelectedCustomerVendor.Value;
                //}              
                //lookupClass.DataSource = GetClassList();
                //lookupClass.DataBind();

                //lookupBrand.DataSource = GetBrandList();
                //lookupBrand.DataBind();
                //Task PopulateStockTrialDataTask = new Task(() => Getstocktrial(FROMDATE, TODATE, BRANCH_ID, PRODUCTID));
                //PopulateStockTrialDataTask.RunSynchronously(); 
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

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    //Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                //drdExport.SelectedValue = "0";
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "StockTrialSummary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Stock Trial Summary Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
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

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];
            //Session.Remove("dt_PartyLedgerRpt");
            //ShowGrid.JSProperties["cpSave"] = null;
            string IsStockTrialSummaryFilter = Convert.ToString(hfIsStockTrialSummaryFilter.Value);
            Session["IsStockTrialSummaryFilter"] = IsStockTrialSummaryFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";
            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');

            //string PRODUCTID = "";
            //string QuoComponent = "";
            //List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo in QuoList)
            //{
            //    QuoComponent += "," + Quo;
            //}
            //PRODUCTID = QuoComponent.TrimStart(',');

            //string strClassList = "", strBrandList = "";


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
            //Task PopulateStockTrialDataTask = new Task(() => GetStocktrialSummaryData(FROMDATE, TODATE, BRANCH_ID, PRODUCTID, strClassList, strBrandList, HEAD_BRANCH));

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

            Task PopulateStockTrialDataTask = new Task(() => GetStocktrialSummaryData(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
            //  PopulateStockTrialDataTask.Start();

        }
        //protected void grid2_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_PartyLedgerRpt"] != null)
        //    {

        //        ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

        //    }

        //}
        //public void GetStocktrialSummaryData(string FROMDATE, string TODATE, string BRANCH_ID, string ProductsId, string classname, string brand, string HEAD_BRANCH)
        public void GetStocktrialSummaryData(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {

                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;

                //DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                ////SqlCommand cmd = new SqlCommand("PROC_MATERIALINOUTREG", con);
                //SqlCommand cmd = new SqlCommand("PROC_STOCKTRIALSUMMARY_REPORT", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                //cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                //cmd.Parameters.AddWithValue("@TODATE", TODATE);
                //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                //cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                //cmd.Parameters.AddWithValue("@PRODUCT_ID", ProductsId);
                //cmd.Parameters.AddWithValue("@ISSERIAL", (chksrl.Checked) ? "1" : "0");
                //cmd.Parameters.AddWithValue("@Class", classname);
                //cmd.Parameters.AddWithValue("@Brand", brand);
                ////cmd.Parameters.AddWithValue("@HO", ddlbranchHO.SelectedValue);
                //cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                //proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                //cmd.CommandTimeout = 0;
                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //da.Fill(ds);

                //cmd.Dispose();
                //con.Dispose();

                //Session["dt_PartyLedgerRpt"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PROC_STOCKTRIALSUMMARY_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@BRANCH_ID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", hdncWiseProductId.Value);
                proc.AddPara("@ISSERIAL", (chksrl.Checked) ? "1" : "0");
                proc.AddPara("@Class", hdnClassId.Value);
                proc.AddPara("@Brand", hdnBranndId.Value);
                proc.AddPara("@HO", HEAD_BRANCH);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

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
            CommonBL stktrial = new CommonBL();
            DataTable dtstktrial = new DataTable();

            dtstktrial = stktrial.GetDateFinancila(Finyear);
            if (dtstktrial.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_StartDate"]));

                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstktrial.Rows[0]["FinYear_StartDate"]));

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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SL";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockTrialSummaryFilter"]) == "Y")
            {
                var q = from d in dc.STOCKTRIALSUMMARY_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        orderby d.SL
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKTRIALSUMMARY_REPORTs
                        where Convert.ToString(d.SL) == "0"
                        orderby d.SL
                        select d;
                e.QueryableSource = q;
            }
        }        
        #endregion

        //protected void ComponentProduct_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string status = string.Empty;
        //    string Customer = string.Empty;
        //    string InvoicecreatedDate = string.Empty;
        //    string BranchId = string.Empty;
        //    string ComponentType = string.Empty;
        //    string Action = string.Empty;
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        //   if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];

        //        DataTable ComponentTable = new DataTable();

        //        if (e.Parameter.Split('~')[1] == "0" || e.Parameter.Split('~')[1] == "")
        //        {
        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //            "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");
        //        }

        //        else
        //        {
        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //            "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) and ProductClass_Code in(" + e.Parameter.Split('~')[1] + ") ORDER BY sProducts_Description");
        //        }


        //        if (ComponentTable.Rows.Count > 0)
        //        {
        //            // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);

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
        //    //     DataTable ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //    //"AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");

        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}


        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #region Branch Populate


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

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                //ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                //if (Session["userbranchHierarchy"] != null)
                //{
                //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                //}
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


    }
}