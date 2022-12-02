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
    public partial class CombinedSalePurchaseReport : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //static string globalBranchHierachyIds = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                Session["SI_ComponentData_Branch"] = null;
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Groupwise Sales/Purchase";
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

                //Session.Remove("dt_CombineSalePurchaseRpt");
                Session["IsGrpWiseSalesPurchaseFilter"] = null;
                Session["exportval"] = null;
                DateTime dtTo = DateTime.Now;
                DateTime dtFrom = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");                
                fnBindBranch();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 11-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
               // checkcateg(Convert.ToString(ddlCriteria.SelectedValue));
            }
        }

        // public void checkcateg(string criteria)
        //{
        //    if (criteria == "Class-Category-Wise")
        //     {
        //         ShowGrid.Columns["Class Name"].VisibleIndex = 2;
        //         ShowGrid.Columns["Category Name"].VisibleIndex = 3;
        //         ShowGrid.Columns["Purchase Value"].Visible = false;
        //         ShowGrid.Columns["Sale Value"].Visible = true;
        //     }
        //    else if (criteria == "Category-Class Wise")
        //    {
        //        ShowGrid.Columns["Class Name"].VisibleIndex = 3;
        //        ShowGrid.Columns["Category Name"].VisibleIndex = 2;
        //        ShowGrid.Columns["Sale Value"].Visible = false;
        //        ShowGrid.Columns["Purchase Value"].Visible = true;
        //    }

        //}
        public void Date_finyearwise(string Finyear)
        {
            //Rev Debashis
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}
            CommonBL cmbsalepurchase = new CommonBL();
            DataTable dtcmbsalepurchase = new DataTable();

            dtcmbsalepurchase = cmbsalepurchase.GetDateFinancila(Finyear);
            if (dtcmbsalepurchase.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtcmbsalepurchase.Rows[0]["FinYear_StartDate"]));

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
        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ShowGrid.DataSource = null;
            //Session.Remove("dt_CombineSalePurchaseRpt");
            //ShowGrid.JSProperties["cpSave"] = null;
            string IsGrpWiseSalesPurchaseFilter = Convert.ToString(hfIsGrpWiseSalesPurchaseFilter.Value);
            Session["IsGrpWiseSalesPurchaseFilter"] = IsGrpWiseSalesPurchaseFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string Branch_ID = string.Empty;
            string BranchList = "";
            List<object> QuoList1 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
            foreach (object Quo in QuoList1)
            {
                BranchList += "," + Quo;
            }
            Branch_ID = BranchList.TrimStart(',');

            string TransType = Convert.ToString(ddlisdocument.SelectedValue);
            //if (TransType == "Sales")
            //{
            //    TransType = "SALE";
            //}
            //else
            //{
            //    TransType = "PURCHASE";
            //}

            //Rev Subhra 18-12-2018   0017670

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


            string Criteria = Convert.ToString(ddlCriteria.SelectedValue);
            Task PopulateStockTrialDataTask = new Task(() => GetCombinedSalePurchase(FROMDATE, TODATE, Branch_ID, TransType, Criteria));
            PopulateStockTrialDataTask.RunSynchronously();
         
        }

        private void GetCombinedSalePurchase(string FromDate, string TODATE, string BranchId, string TransType, string Criteria)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                if (Criteria == "Class-Category Wise")
                {
                                   
                    SqlCommand cmd = new SqlCommand("prc_ClassCategorySalePurchase_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    cmd.Parameters.AddWithValue("@BRANCHID", string.IsNullOrEmpty(BranchId) ? string.Empty : BranchId);
                    cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    cmd.Parameters.AddWithValue("@TODATE", TODATE);
                    cmd.Parameters.AddWithValue("@TRAN_TYPE", TransType);
                    cmd.Parameters.AddWithValue("@CRITERIA", Criteria);
                    cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    cmd.Dispose();
                    con.Dispose();
                    
                }
                else if (Criteria == "Category-Class Wise")
                {

                    //DataSet ds = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlCommand cmd = new SqlCommand("prc_CategoryClassSalePurchase_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    cmd.Parameters.AddWithValue("@BRANCHID", string.IsNullOrEmpty(BranchId) ? string.Empty : BranchId);
                    cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    cmd.Parameters.AddWithValue("@TODATE", TODATE);
                    cmd.Parameters.AddWithValue("@TRAN_TYPE", TransType);
                    cmd.Parameters.AddWithValue("@CRITERIA", Criteria);
                    cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    cmd.Dispose();
                    con.Dispose();

                }
                con.Dispose();

                //Session["dt_CombineSalePurchaseRpt"] = ds.Tables[0];
                ShowGrid.Columns.Clear();
                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
               
            }
            catch (Exception ex)
            {
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);            
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsGrpWiseSalesPurchaseFilter"]) == "Y")
            {
                var q = from d in dc.GROUPWISESALESPURCHASE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.TRAN_TYPE) == ddlisdocument.SelectedValue && Convert.ToString(d.CRITERIA) == ddlCriteria.SelectedValue
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.GROUPWISESALESPURCHASE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        //protected void Showgrid_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_CombineSalePurchaseRpt"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_CombineSalePurchaseRpt"];
        //    }
        //    else
        //    {
        //        ShowGrid.DataSource = null;
        //    }
        //}
        protected void Showgrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in grid.Columns)
            {
                if (Session["IsGrpWiseSalesPurchaseFilter"] == null)
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SEQ"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CRITERIA"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Sl_No"))
                    {
                        c.Visible = true;
                        c.Caption = "Sl.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("ClassID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Class_Name"))
                    {
                        c.Visible = true;
                        c.Caption = "Class Name";
                    }
                    if ((c.FieldName.ToString()).StartsWith("BrandID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Category_Name"))
                    {
                        c.Visible = true;
                        c.Caption = "Category Name";
                    }
                    if ((c.FieldName.ToString()).StartsWith("Quantity"))
                    {
                        c.Visible = true;
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                    {
                        c.Visible = true;
                        c.Caption = "Sales Value";
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                }
                else
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SEQ"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CRITERIA"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("BrandID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("ClassID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Sl_No"))
                    {
                        c.Caption = "Sl.";
                        c.Width = 75;
                        c.VisibleIndex = 1;
                        c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Quantity"))
                    {
                        c.VisibleIndex = 4;
                        c.Width = 350;
                        c.Caption = "Quantity";
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    //if ((c.FieldName.ToString()).StartsWith("Class_Name"))
                    //{
                    //    c.Width = 350;
                    //}
                    //if ((c.FieldName.ToString()).StartsWith("Category_Name"))
                    //{
                    //    c.Width = 350;
                    //}
                    if(ddlCriteria.SelectedValue=="Class-Category Wise")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Class_Name"))
                        {
                            c.Width = 350;
                            c.Caption = "Class Name";
                            c.VisibleIndex = 2;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Category_Name"))
                        {
                            c.Width = 350;
                            c.Caption = "Category Name";
                            c.VisibleIndex = 3;
                        }                        
                        if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                        {
                            c.VisibleIndex = 5;
                            c.Width = 350;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            if (ddlisdocument.SelectedValue == "Sales")
                            {
                                c.Caption = "Sales Value";
                            }
                            else
                            {
                                c.Caption = "Purchase Value";
                            }
                        }
                    }
                    else if(ddlCriteria.SelectedValue=="Category-Class Wise")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Category_Name"))
                        {
                            c.Width = 350;
                            c.Caption = "Category Name";
                            c.VisibleIndex = 2;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Class_Name"))
                        {
                            c.Width = 350;
                            c.Caption = "Class Name";
                            c.VisibleIndex = 3;
                        }                        
                        if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                        {
                            c.VisibleIndex = 5;
                            c.Width = 350;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            if (ddlisdocument.SelectedValue == "Sales")
                            {
                                c.Caption = "Sales Value";
                            }
                            else
                            {
                                c.Caption = "Purchase Value";
                            }
                        }
                    }
                }
            
            }
        }
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {

                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }

                //drdExport.SelectedValue = "0";
            }

        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Sub Total")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }
            if (e.DataColumn.FieldName == "SalePurchase_Value")
            {
                Session["chk_presenttotal"] = 0;
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "Groupwise Sales Purchase Report";
            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            string FileHeader = "";
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Groupwise Sales/Purchase Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 18-12-2018   0017670
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

        //Rev Subhra 18-12-2018   0017670
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

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        //protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        //{

        //}

       

        private void fnBindBranch()
        {
            CommonBL HObranch = new CommonBL();
            DataTable ComponentTable = new DataTable();
            if (Session["userbranchHierarchy"] != null)
            {
                //Commented By Subhra 18-04-2018
                //ComponentTable = oDBEngine.GetDataTable("SELECT DISTINCT A.branch_id,A.branch_code,A.branch_description FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId ORDER BY A.branch_id asc");
                //ComponentTable = oDBEngine.GetDataTable("SELECT  A.branch_id,A.branch_code,A.branch_description FROM tbl_master_branch A WHERE A.branch_parentId=0 UNION SELECT  A.branch_id,A.branch_code,A.branch_description FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId AND B.branch_parentId <>0");
                ComponentTable = HObranch.GetHierarchyWiseBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
               
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
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            //DataTable ComponentTable = oDBEngine.GetDataTable("SELECT DISTINCT A.branch_id,A.branch_code,A.branch_description FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId ORDER BY A.branch_id asc");

            //if (ComponentTable.Rows.Count > 0)
            //{
            //    lookup_branch.DataSource = ComponentTable;
            //}
            //else
            //{
            //    lookup_branch.DataSource = null;
            //}

            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        //private DataTable fnGetFinancier(string branchIds)
        //{
        //    DataTable ComponentTable = new DataTable();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        SqlCommand cmd = new SqlCommand("GetCustomerDropdownBind_DebtorsReport", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@BranchIds", branchIds);
        //        cmd.Parameters.AddWithValue("@Action", "FinancierBind");
        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ComponentTable);

        //        cmd.Dispose();
        //        con.Dispose();
        //    }
        //    return ComponentTable;
        //}
   
        //private DataTable fnGetCustomer(string branchIds)
        //{
        //    DataTable ComponentTable = new DataTable();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        SqlCommand cmd = new SqlCommand("GetCustomerDropdownBind_DebtorsReport", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@BranchIds", branchIds);
        //        cmd.Parameters.AddWithValue("@Action", "CustomerBind");
        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ComponentTable);

        //        cmd.Dispose();
        //        con.Dispose();
        //    }
        //    return ComponentTable;
        //}
       
    
        //private string GetBranchHierachyIds(string branchParam)
        //{
        //    string branchHierarchy = string.Empty;

        //    var strBranchIds = Convert.ToString(branchParam).Split(',');

        //    foreach (string data in strBranchIds)
        //    {
        //        if (!string.IsNullOrEmpty(data))
        //        {
        //            branchHierarchy = branchHierarchy + ",";
        //            branchHierarchy = oDBEngine.getBranch(data, "") + data;
        //        }
        //    }

        //    branchHierarchy = branchHierarchy.TrimStart(',');
        //    return branchHierarchy;
        //}

        //protected void ddlCriteria_SelectedIndexChanged(object sender, EventArgs e)
        //{
            //checkcateg(Convert.ToString(ddlCriteria.SelectedValue));
        //}
    }
}