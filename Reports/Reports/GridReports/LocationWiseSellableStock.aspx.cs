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
    public partial class LocationWiseSellableStock : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ArrayList amountfields_caption = null;
        ArrayList amountfields_fields = null;
        ArrayList bandedfields = null;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/LocationWiseSellableStock.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Location Wise - Stock In Hand";
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

                Session["exportval"] = null;
                Session["exportTotalSummary"] = "0";
                Session["LocationewiseStkStat"] = null;
                Session["LocationwiseStkStatTotal"] = null;
                Session["BR_ID"] = null;
                Session["BR_NAME"] = null;
                Session["LCStkStatHeadersAmountCaption"] = null;
                Session["LCStkStatHeadersAmountFields"] = null;
                Session["LCStkStatGridviewsBandedFields"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["WarehouseData"] = null;

                BranchHoOffice();

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
            }

            //if (ShowGridLocationwiseStockStatus.Columns["PRODDESC"] == null)
            if (Session["LocationewiseStkStat"] == null)
            {
                //==============By default Grid populate in load===================
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODCODE");
                dt.Columns.Add("PRODDESC");
                //dt.Columns.Add("WHDESC");
                dt.Columns.Add("PRODCLASS");
                dt.Columns.Add("BRANDNAME");
                dt.Columns.Add("STOCKUOM");

                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "PRODCODE";
                col1.Caption = "Item Code";
                col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 350;
                ShowGridLocationwiseStockStatus.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PRODDESC";
                col2.Caption = "Item Details";
                col2.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 350;
                ShowGridLocationwiseStockStatus.Columns.Add(col2);

                //GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                //col3.FieldName = "WHDESC";
                //col3.Caption = "Warehouse";
                //col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                //col3.Width = 350;
                //ShowGridLocationwiseStockStatus.Columns.Add(col3);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "PRODCLASS";
                col3.Caption = "Class Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                ShowGridLocationwiseStockStatus.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "BRANDNAME";
                col4.Caption = "Brand Name";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 350;
                ShowGridLocationwiseStockStatus.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "STOCKUOM";
                col5.Caption = "Main Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 350;
                ShowGridLocationwiseStockStatus.Columns.Add(col5);
                //=====================================================================

            }
            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
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

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

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

        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                Session["exportTotalSummary"] = "1";
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

            string filename = "LocationwiseStockInHand";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Location Wise - Stock In Hand" + Environment.NewLine + "As On :" + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGridLocationwiseStockStatus";
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
                default:
                    return;
            }

        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if(Hoid.ToString()=="null")
                {
                    Hoid = "All";
                }
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

        #region Warehouse Populate

        protected void Componentwarehouse_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindWarehouseGrid")
            {
                DataTable WarehouseTable = new DataTable();
                WarehouseTable = GetWarehouse();

                if (WarehouseTable.Rows.Count > 0)
                {
                    Session["WarehouseData"] = WarehouseTable;
                    lookup_warehouse.DataSource = WarehouseTable;
                    lookup_warehouse.DataBind();
                }
                else
                {
                    Session["WarehouseData"] = WarehouseTable;
                    lookup_warehouse.DataSource = null;
                    lookup_warehouse.DataBind();
                }
            }
        }

        public DataTable GetWarehouse()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_WAREHOUSESELECTION_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_warehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["WarehouseData"] != null)
            {
                lookup_warehouse.DataSource = (DataTable)Session["WarehouseData"];
            }
        }

        #endregion

        #region Location wise Stock Status grid
        protected void ShowGridLocationwiseStockStatus_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowGridLocationwiseStockStatus.DataSource = null;
            Session["LocationewiseStkStat"] = null;
            Session["LocationwiseStkStatTotal"] = null;
            Session["BR_ID"] = null;
            Session["BR_NAME"] = null;
            Session["LCStkStatHeadersAmountCaption"] = null;
            Session["LCStkStatHeadersAmountFields"] = null;
            Session["LCStkStatGridviewsBandedFields"] = null;

            string returnPara = Convert.ToString(e.Parameters);

            DateTime dtTodate;

            dtTodate = Convert.ToDateTime(ASPxToDate.Date);
            string ASONDATE = dtTodate.ToString("yyyy-MM-dd");

            string BR_ID = "";
            string BRID = "";
            List<object> BridList = lookup_branch.GridView.GetSelectedFieldValues("ID");

            string BR_Name = "";
            string BRNameComponent = "";
            List<object> BRNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description", "ID");

            foreach (object[] item in BRNameList)
            {
                BRNameComponent += "," + item[0].ToString();
                BRID += "," + item[1].ToString();
            }

            string WH_ID = "";

            string WHID = "";
            List<object> WhidList = lookup_warehouse.GridView.GetSelectedFieldValues("ID");
            foreach (object WH in WhidList)
            {
                WHID += "," + WH;
            }
            WH_ID = WHID.TrimStart(',');

            BR_ID = BRID.TrimStart(',');
            Session["BR_ID"] = BR_ID;

            BR_Name = BRNameComponent.TrimStart(',');
            Session["BR_NAME"] = BR_Name;

            Task PopulateStockTrialDataTask = new Task(() => GetLocationwiseStockStatusdata(ASONDATE, BR_ID, WH_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetLocationwiseStockStatusdata(string ASONDATE, string BR_ID, string WHID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BRANCHWISESTOCKSTATUSFETCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BR_ID);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdncWiseProductId.Value);
                cmd.Parameters.AddWithValue("@WAREHOUSE_ID", WHID);
                cmd.Parameters.AddWithValue("@CLASS", hdnClassId.Value);
                cmd.Parameters.AddWithValue("@BRAND", hdnBranndId.Value);
                cmd.Parameters.AddWithValue("@CONSIDERWH", (chkIsWHWise.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@EXCLUDEZEROBAL", (chkExcludeZero.Checked) ? "1" : "0");

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();

                DataTable dt = new DataTable();

                DataView dvData = new DataView(ds.Tables[0]);
                dvData.RowFilter = "STOCKUOM <> 'Net Total:'";
                Session["LocationewiseStkStat"] = dvData.ToTable();

                DataView dvData1 = new DataView(ds.Tables[0]);
                dvData1.RowFilter = "STOCKUOM = 'Net Total:'";
                Session["LocationwiseSummaryTotal"] = dvData1.ToTable();

                ShowGridLocationwiseStockStatus.DataSource = dvData.ToTable();
                ShowGridLocationwiseStockStatus.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridLocationwiseStockStatus_DataBinding(object sender, EventArgs e)
        {
            DataTable dt_LocationwiseStkStat = (DataTable)Session["LocationewiseStkStat"];
            if (Session["exportTotalSummary"].ToString()=="1")
            {
                DataTable dt_WarehousewiseSummaryTotal = (DataTable)Session["LocationwiseSummaryTotal"];
                dt_LocationwiseStkStat.Merge(dt_WarehousewiseSummaryTotal);
            }

            if (dt_LocationwiseStkStat.Rows.Count > 0)
            {
                int maxColumnIndex = ShowGridLocationwiseStockStatus.Columns.Count - 1;
                for (int i = maxColumnIndex; i >= 0; i--)
                {
                    ShowGridLocationwiseStockStatus.Columns.RemoveAt(i);
                }
            }

            if (dt_LocationwiseStkStat.Rows.Count == 0)
            {
                if (chkIsWHWise.Checked == false)
                {
                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "PRODCODE";
                    col1.Caption = "Item Code";
                    col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 250;
                    ShowGridLocationwiseStockStatus.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "PRODDESC";
                    col2.Caption = "Item Details";
                    col2.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 250;
                    ShowGridLocationwiseStockStatus.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "PRODCLASS";
                    col3.Caption = "Class Name";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "BRANDNAME";
                    col4.Caption = "Brand Name";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "STOCKUOM";
                    col5.Caption = "Main Unit";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col5);
                }
                else
                {
                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "PRODCODE";
                    col1.Caption = "Item Code";
                    col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 250;
                    ShowGridLocationwiseStockStatus.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "PRODDESC";
                    col2.Caption = "Item Details";
                    col2.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 250;
                    ShowGridLocationwiseStockStatus.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "WHDESC";
                    col3.Caption = "Warehouse";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "PRODCLASS";
                    col4.Caption = "Class Name";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "BRANDNAME";
                    col5.Caption = "Brand Name";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col5);

                    GridViewDataTextColumn col6 = new GridViewDataTextColumn();
                    col6.FieldName = "STOCKUOM";
                    col6.Caption = "Main Unit";
                    col6.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col6.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col6);
                }
            }

            if (dt_LocationwiseStkStat.Rows.Count > 0)
            {
                string fieldname = "";
                if (chkIsWHWise.Checked == false)
                {
                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "PRODCODE";
                    col1.Caption = "Item Code";
                    col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 300;
                    ShowGridLocationwiseStockStatus.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "PRODDESC";
                    col2.Caption = "Item Details";
                    col2.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 300;
                    ShowGridLocationwiseStockStatus.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "PRODCLASS";
                    col3.Caption = "Class Name";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "BRANDNAME";
                    col4.Caption = "Brand Name";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "STOCKUOM";
                    col5.Caption = "Main Unit";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col5);
                }
                else
                {
                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "PRODCODE";
                    col1.Caption = "Item Code";
                    col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 300;
                    ShowGridLocationwiseStockStatus.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "PRODDESC";
                    col2.Caption = "Item Details";
                    col2.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 300;
                    ShowGridLocationwiseStockStatus.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "WHDESC";
                    col3.Caption = "Warehouse";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "PRODCLASS";
                    col4.Caption = "Class Name";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "BRANDNAME";
                    col5.Caption = "Brand Name";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col5);

                    GridViewDataTextColumn col6 = new GridViewDataTextColumn();
                    col6.FieldName = "STOCKUOM";
                    col6.Caption = "Main Unit";
                    col6.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col6.Width = 100;
                    ShowGridLocationwiseStockStatus.Columns.Add(col6);
                }

                if (Session["BR_ID"] != null)
                {
                    ArrayList BRID = null;
                    ArrayList BR_Name = null;
                    BRID = new ArrayList((Session["BR_ID"].ToString()).Split(new char[] { ',' }));
                    BR_Name = new ArrayList((Session["BR_NAME"].ToString()).Split(new char[] { ',' }));

                    for (int i = 0; i < BRID.Count; i++)
                    {
                        if (dt_LocationwiseStkStat.Columns.Contains(BRID[i].ToString()))
                        {
                            GridViewBandColumn bandColumn = new GridViewBandColumn();
                            GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                            fieldname = BRID[i].ToString() + "_" + "CLOSE_QTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Qty.";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 110;
                            coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);

                            bandColumn.Caption = BR_Name[i].ToString();
                            bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            ShowGridLocationwiseStockStatus.Columns.Add(bandColumn);

                            if (i == 0)
                            {
                                hdnLCStkStatNoBandedColumn.Value = BR_Name[i].ToString();
                                hdnLCStkStatNoCaption.Value = "Qty.";
                                hdnLCStkStatNoFields.Value = BRID[i].ToString() + "_" + "CLOSE_QTY";
                            }
                            else
                            {
                                hdnLCStkStatNoBandedColumn.Value = hdnLCStkStatNoBandedColumn.Value + "~" + BR_Name[i].ToString();
                                hdnLCStkStatNoCaption.Value = hdnLCStkStatNoCaption.Value + "~" + "Qty.";
                                hdnLCStkStatNoFields.Value = hdnLCStkStatNoFields.Value + "~" + BRID[i].ToString() + "_" + "CLOSE_QTY";
                            }
                        }
                    }
                }

                if (Session["LCStkStatGridviewsBandedFields"] == null)
                {
                    Session["LCStkStatGridviewsBandedFields"] = hdnLCStkStatNoBandedColumn.Value;
                    Session["LCStkStatHeadersAmountCaption"] = hdnLCStkStatNoCaption.Value;
                    Session["LCStkStatHeadersAmountFields"] = hdnLCStkStatNoFields.Value;
                }

                GridViewDataTextColumn Totcol1 = new GridViewDataTextColumn();
                Totcol1.FieldName = "TOTAL_CLOSEQTY";
                Totcol1.Caption = "Total Qty.";
                Totcol1.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol1.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol1.Width = 110;
                Totcol1.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                Totcol1.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ShowGridLocationwiseStockStatus.Columns.Add(Totcol1);

                ShowGridLocationwiseStockStatus.DataSource = dt_LocationwiseStkStat;
            }
        }

        protected void ShowGridLocationwiseStockStatus_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                DataTable dt_WarehousewiseSummaryTotal = (DataTable)Session["LocationwiseSummaryTotal"];
                if (dt_WarehousewiseSummaryTotal.Rows.Count > 0)
                {

                    if (amountfields_caption == null)
                    {
                        amountfields_caption = new ArrayList((Session["LCStkStatHeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                        amountfields_fields = new ArrayList((Session["LCStkStatHeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                        bandedfields = new ArrayList((Session["LCStkStatGridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                    }
                    else
                    {
                        if (chkIsWHWise.Checked == false && e.Column.Caption != "Item Code" && e.Column.Caption != "Item Details" && e.Column.Caption != "Class Name" && e.Column.Caption != "Brand Name" && e.Column.Caption != "Main Unit" && e.Column.Caption != "Total Qty.")
                        {
                            if (e.Column.ParentBand.ToString() == bandedfields[(e.Column.ParentBand.VisibleIndex) - 5].ToString())
                            {
                                if (e.Column.Caption == amountfields_caption[0].ToString())
                                {
                                    if (e.IsTotalFooter)
                                    {
                                        e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                                        amountfields_fields.RemoveAt(0);
                                        amountfields_caption.RemoveAt(0);
                                    }
                                }
                            }
                        }
                        else if (chkIsWHWise.Checked == true && e.Column.Caption != "Item Code" && e.Column.Caption != "Item Details" && e.Column.Caption != "Warehouse" && e.Column.Caption != "Class Name" && e.Column.Caption != "Brand Name" && e.Column.Caption != "Main Unit" && e.Column.Caption != "Total Qty.")
                        {
                            if (e.Column.ParentBand.ToString() == bandedfields[(e.Column.ParentBand.VisibleIndex) - 6].ToString())
                            {
                                if (e.Column.Caption == amountfields_caption[0].ToString())
                                {
                                    if (e.IsTotalFooter)
                                    {
                                        e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                                        amountfields_fields.RemoveAt(0);
                                        amountfields_caption.RemoveAt(0);
                                    }
                                }
                            }
                        }
                        else if (e.Column.Caption == "Total Qty.")
                        {
                            if (e.IsTotalFooter)
                            {
                                if (e.Column.Caption == "Total Qty.")
                                {
                                    e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0]["TOTAL_CLOSEQTY"].ToString();
                                }
                            }
                        }
                    }
                }
                if (e.Column.Caption == "Main Unit")
                {
                    if (e.IsTotalFooter)
                    {
                        e.Cell.Text = "Net Total:";
                    }
                }
            }

        }
    }
        #endregion
}