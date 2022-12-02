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
    public partial class StockValuationWithAgeing : System.Web.UI.Page
    {
        DateTime dtTo;
        decimal TotalQty_Grp = 0, TotalValuation_30_Grp = 0, TotalValuation_60_Grp = 0, TotalValuation_90_Grp = 0, TotalValuation_120_Grp = 0, TotalValuation_A120_Grp = 0, TotalValuation_Grp = 0;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/StockValuationWithAgeing.aspx");
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockValuationWithAgeing.aspx");
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Valuation with Ageing";
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
                //Session["dt_StockValuationWithAgRpt"] = null;
                Session["IsStockValAgeingFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtTo = DateTime.Now;
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxToDate.Value = DateTime.Now;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                //lookupClass.DataSource = GetClassList();
                //lookupClass.DataBind();

                //lookupBrand.DataSource = GetBrandList();
                //lookupBrand.DataBind();
            }
            else
            {
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

        #region Export Valuation 
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "Stock Valuation with Ageing";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Stock Valuation With Ageing" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 24-12-2018   0017670
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
        //Rev Subhra 24-12-2018   0017670
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

        #region =======================Valuation Summary =========================
        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_StockValuationWithAgRpt");
            //ShowGrid.JSProperties["cpSave"] = null;

            string IsStockValAgeingFilter = Convert.ToString(hfIsStockValAgeingFilter.Value);
            Session["IsStockValAgeingFilter"] = IsStockValAgeingFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchComponent2 = "";
            List<object> BranchList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Br2 in BranchList2)
            {
                BranchComponent2 += "," + Br2;
            }
            BRANCH_ID = BranchComponent2.TrimStart(',');

            //string PRODUCTID = "";
            //string ProductComponent = "";
            //List<object> ProductList = lookup_product.GridView.GetSelectedFieldValues("ID");
            //foreach (object Pro in ProductList)
            //{
            //    ProductComponent += "," + Pro;
            //}
            //PRODUCTID = ProductComponent.TrimStart(',');

            //Task PopulateStockTrialDataTask = new Task(() => GetStockValuationdata(FROMDATE, TODATE, BRANCH_ID, PRODUCTID));
            //Task PopulateStockTrialDataTask = new Task(() => GetStockValuationAgeingdata(TODATE, BRANCH_ID, PRODUCTID));

            //Rev Subhra 24-12-2018   0017670
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

            Task PopulateStockTrialDataTask = new Task(() => GetStockValuationAgeingdata(TODATE, BRANCH_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        //public void GetStockValuationdata(string FROMDATE, string TODATE, string BRANCH_ID, string ProductIds)
        public void GetStockValuationAgeingdata(string TODATE, string BRANCH_ID)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;
                //string strClassList = "", strBrandList = "";
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
                //ds = GetvaluationSummary(BRANCH_ID, ProductIds, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), TODATE, "F", "Details", strClassList, strBrandList);
                //if (ds.Rows.Count > 0)
                //{
                //    Session["dt_StockValuationWithAgRpt"] = ds;
                //    ShowGrid.DataSource = ds;
                //    ShowGrid.DataBind();
                //    ShowGrid.ExpandAll();
                //}

                DataTable ds = new DataTable();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ProductValuationWithAgeing_Report", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdncWiseProductId.Value);
                //Rev Debashis
                //cmd.Parameters.AddWithValue("@val_type", "F");
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                //End of Rev Debashis
                cmd.Parameters.AddWithValue("@GetType", "Details");
                cmd.Parameters.AddWithValue("@Class", hdnClassId.Value);
                cmd.Parameters.AddWithValue("@Brand", hdnBranndId.Value);
                //Rev Debashis
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                //End of Rev Debashis
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
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

        //public DataTable GetvaluationSummary(string branch, string product, string company, string Finyear, string TODATE, string valtype, string gettype, string classname, string brand)
        //{
        //    DataTable ds = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    SqlCommand cmd = new SqlCommand("prc_ProductValuationWithAgeing_Report", con);

        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@COMPANYID", company);
        //    cmd.Parameters.AddWithValue("@FINYEAR", Finyear);
        //    cmd.Parameters.AddWithValue("@TODATE", TODATE);
        //    cmd.Parameters.AddWithValue("@BRANCHID", branch);
        //    cmd.Parameters.AddWithValue("@PRODUCT_ID", product);
        //    cmd.Parameters.AddWithValue("@val_type", valtype);
        //    cmd.Parameters.AddWithValue("@GetType", gettype);
        //    cmd.Parameters.AddWithValue("@Class", classname);
        //    cmd.Parameters.AddWithValue("@Brand", brand);

        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(ds);

        //    cmd.Dispose();
        //    con.Dispose();

        //    return ds;
        //}

        //protected void gridvaluationsummary_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_StockValuationWithAgRpt"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_StockValuationWithAgRpt"];
        //    }
        //}

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);

            //Group Summary Calculation
            ASPxGridView grid = sender as ASPxGridView;
            if (e.IsGroupSummary)
            {
                string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
                switch (summaryTAG)
                {
                    case "Item_Qty":
                        e.Text = string.Format("{0}", e.Value);
                        TotalQty_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_30Days":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_30_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_60Days":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_60_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_90Days":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_90_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_120Days":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_120_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_A120Days":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_A120_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_TotVal":
                        e.Text = string.Format("{0}", e.Value);
                        TotalValuation_Grp = Convert.ToDecimal(e.Value);
                        break;
                    case "Item_Brand":
                        e.Text = "Total";
                        break;
                }
            }
        }        

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}
            CommonBL stkvalage = new CommonBL();
            DataTable dtstkvalage = new DataTable();

            dtstkvalage = stkvalage.GetDateFinancila(Finyear);
            if (dtstkvalage.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkvalage.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkvalage.Rows[0]["FinYear_StartDate"]));
                DateTime MaximumDate = Convert.ToDateTime((dtstkvalage.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkvalage.Rows[0]["FinYear_StartDate"]));
                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;
                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                }
            }
        }


        #region  ===========Product  Bind====================
        //protected void ComponentProduct_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        string type = e.Parameter.Split('~')[1];

        //        DataTable ComponentTable = new DataTable();

        //        if (e.Parameter.Split('~')[1] == "0" || e.Parameter.Split('~')[1] == "")
        //        {
        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' ,sProducts_Hsncode as Hsn FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //             "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");
        //        }

        //        else
        //        {

        //            ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name',sProducts_Hsncode as Hsn  FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //                     "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) and ProductClass_Code in(" + e.Parameter.Split('~')[1] + ") ORDER BY sProducts_Description");

        //        }


        //        if (ComponentTable.Rows.Count > 0)
        //        {

        //            Session["SI_ComponentData"] = ComponentTable;
        //            lookup_product.DataSource = ComponentTable;
        //            lookup_product.DataBind();

        //        }
        //        else
        //        {
        //            Session["SI_ComponentData"] = null;
        //            lookup_product.DataSource = null;
        //            lookup_product.DataBind();

        //        }
        //    }
        //}

        //protected void lookup_product_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_product.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}

        #endregion

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

        //protected void ShowGrid_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        //{
        //    string summaryTAG = (e.Item as ASPxSummaryItem).Tag;

        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        //    {
        //        TotalQty = 0;
        //        TotalValuation_30 = 0; 
        //        TotalValuation_60 = 0; 
        //        TotalValuation_90 = 0; 
        //        TotalValuation_120 = 0;
        //        TotalValuation_A120 = 0;
        //        TotalValuation = 0;
        //    }
        //    else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        //    {
        //        TotalQty += Convert.ToDecimal(e.GetValue("IN_QTY_OP"));
        //        TotalValuation_30 += Convert.ToDecimal(e.GetValue("0-30 Days"));
        //        TotalValuation_60 += Convert.ToDecimal(e.GetValue("31-60 Days"));
        //        TotalValuation_90 += Convert.ToDecimal(e.GetValue("61-90 Days"));
        //        TotalValuation_120 += Convert.ToDecimal(e.GetValue("91-120 Days"));
        //        TotalValuation_A120 += Convert.ToDecimal(e.GetValue("120 & Above"));
        //        TotalValuation += Convert.ToDecimal(e.GetValue("TOTAL_VALUE"));
        //    }
        //    else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //    {
        //        switch (summaryTAG)
        //        {
        //            case "Item_Qty":
        //                e.TotalValue = TotalQty;
        //                break;
        //            case "Item_30Days":
        //                e.TotalValue = TotalValuation_30;
        //                break;
        //            case "Item_60Days":
        //                e.TotalValue = TotalValuation_60;
        //                break;
        //            case "Item_90Days":
        //                e.TotalValue = TotalValuation_90;
        //                break;
        //            case "Item_120Days":
        //                e.TotalValue = TotalValuation_120;
        //                break;
        //            case "Item_A120Days":
        //                e.TotalValue = TotalValuation_A120;
        //                break;
        //            case "Item_TotVal":
        //                e.TotalValue = TotalValuation;
        //                break;
        //            case "Item_Brand":
        //                e.TotalValue = "Total";
        //                break;
        //        }
        //    }
        //}

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockValAgeingFilter"]) == "Y")
            {
                var q = from d in dc.STOCKVALUATIONWITHAGEING_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKVALUATIONWITHAGEING_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }
        #endregion
    }
}