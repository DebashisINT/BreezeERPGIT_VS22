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
    public partial class WarehousewiseStockStatus : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ArrayList amountfields_caption = null;
        ArrayList amountfields_fields = null;
        ArrayList bandedfields = null;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/WarehousewiseStockStatus.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Warehouse wise Stock Status";
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
                Session["WarehousewiseStkStat"] = null;
                Session["WarehousewiseStkStatTotal"] = null;
                Session["WH_ID"] = null;
                Session["WH_NAME"] = null;
                Session["WHStkStatHeadersAmountCaption"] = null;
                Session["WHStkStatHeadersAmountFields"] = null;
                Session["WHStkStatGridviewsBandedFields"] = null;
                Session["exportvalue"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
            }

            if (ShowGridWarehousewiseStockStatus.Columns["PRODDESC"] == null)
            {
                //==============By default Grid populate in load===================
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODDESC");
                dt.Columns.Add("PRODCLASS");
                dt.Columns.Add("BRANDNAME");
                dt.Columns.Add("STOCKUOM");
                dt.Columns.Add("ALTUOM");

                //GridViewBandColumn bandColumn = new GridViewBandColumn();
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "PRODDESC";
                col1.Caption = "Item Details";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 350;
                //bandColumn.Columns.Add(col1);
                ShowGridWarehousewiseStockStatus.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PRODCLASS";
                col2.Caption = "Class Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 350;
                //bandColumn.Columns.Add(col2);
                ShowGridWarehousewiseStockStatus.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "BRANDNAME";
                col3.Caption = "Brand Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                //bandColumn.Columns.Add(col3);
                ShowGridWarehousewiseStockStatus.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "STOCKUOM";
                col4.Caption = "Main Unit";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 350;
                //bandColumn.Columns.Add(col4);
                ShowGridWarehousewiseStockStatus.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "ALTUOM";
                col5.Caption = "Alt. Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 350;
                //bandColumn.Columns.Add(col5);
                ShowGridWarehousewiseStockStatus.Columns.Add(col5);

                //bandColumn.Caption = "Item(s) Detail";
                //bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //ShowGridWarehousewiseStockStatus.Columns.Add(bandColumn);

                //ShowGridWarehousewiseStockStatus.DataSource = dt;
                //ShowGridWarehousewiseStockStatus.DataBind();
                //=====================================================================

            }
            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtTo= Convert.ToDateTime(ASPxToDate.Date);
            }
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
            string filename = "WarehousewiseStockStatus_Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Warehouse wise Stock Staus" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridWarehousewiseStockStatus";
            exporter.RenderBrick += exporter_RenderBrick;
            if(Filter==3)
            {
                Session["exportvalue"] = "1";
            }
            else
            {
                Session["exportvalue"] = "0";
            }
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

        #region Warehouse Populate

        protected void Componentwarehouse_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindWarehouseGrid")
            {
                DataTable WarehouseTable = new DataTable();
                WarehouseTable = GetWarehouse();

                if (WarehouseTable.Rows.Count > 0)
                {
                    Session["Warehouse_Data"] = WarehouseTable;
                    lookup_warehouse.DataSource = WarehouseTable;
                    lookup_warehouse.DataBind();
                }
                else
                {
                    Session["Warehouse_Data"] = WarehouseTable;
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
            if (Session["Warehouse_Data"] != null)
            {
                lookup_warehouse.DataSource = (DataTable)Session["Warehouse_Data"];
            }
        }

        #endregion

        #region Warehouse wise Stock Status grid
        protected void ShowGridWarehousewiseStockStatus_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowGridWarehousewiseStockStatus.DataSource = null;
            Session["WarehousewiseStkStat"] = null;
            Session["WarehousewiseStkStatTotal"] = null;
            Session["WH_ID"] = null;
            Session["WH_NAME"] = null;
            Session["WHStkStatHeadersAmountCaption"] = null;
            Session["WHStkStatHeadersAmountFields"] = null;
            Session["WHStkStatGridviewsBandedFields"] = null;

            string returnPara = Convert.ToString(e.Parameters);

            DateTime dtTodate;

            dtTodate = Convert.ToDateTime(ASPxToDate.Date);
            string TODATE = dtTodate.ToString("yyyy-MM-dd");

            string WH_ID = "";
            string WHID = "";
            List<object> WhidList = lookup_warehouse.GridView.GetSelectedFieldValues("ID");
            //foreach (object WH in WhidList)
            //{
            //    WHID += "," + WH;
            //}
            //WH_ID = WHID.TrimStart(',');

            string WH_Name = "";
            string WHNameComponent = "";
            //List<object> WHNameList = lookup_warehouse.GridView.GetSelectedFieldValues("Description");
            List<object> WHNameList = lookup_warehouse.GridView.GetSelectedFieldValues("Description", "ID");
            //foreach (object WHNAME in WHNameList)
            //{
            //    WHNameComponent += "," + WHNAME;
            //}
            //WH_Name = WHNameComponent.TrimStart(',');

            foreach (object[] item in WHNameList)
            {
                WHNameComponent += "," + item[0].ToString();
                WHID += "," + item[1].ToString();
            }

            WH_ID = WHID.TrimStart(',');
            Session["WH_ID"] = WH_ID;

            WH_Name = WHNameComponent.TrimStart(',');
            Session["WH_NAME"] = WH_Name;            

            Task PopulateStockTrialDataTask = new Task(() => GetWarehousewiseStockStatusdata(TODATE, WH_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetWarehousewiseStockStatusdata(string TODATE, string WH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_WAREHOUSEWISESTOCKSTATUSFETCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdncWiseProductId.Value);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@CLASS", hdnClassId.Value);
                cmd.Parameters.AddWithValue("@BRAND", hdnBranndId.Value);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@WAREHOUSE_ID", WH_ID);
                cmd.Parameters.AddWithValue("@CONSOPASONDATE", (chkConsopasondt.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();

                DataTable dt = new DataTable();
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    dt = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("PRODDESC") != "Net Total:").CopyToDataTable();
                //}

                DataView dvData = new DataView(ds.Tables[0]);
                dvData.RowFilter = "ALTUOM <> 'Net Total:'";
                Session["WarehousewiseStkStat"] = dvData.ToTable();

                //DataTable dt_nTot = new DataTable();
                //dt_nTot = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("PRODDESC") == "Net Total:").CopyToDataTable();
                //Session["WarehousewiseStkStatTotal"] = dt_nTot;

                DataView dvData1 = new DataView(ds.Tables[0]);
                dvData1.RowFilter = "ALTUOM = 'Net Total:'";
                Session["WarehousewiseSummaryTotal"] = dvData1.ToTable();

                //ShowGridWarehousewiseStockStatus.DataSource = dt;
                ShowGridWarehousewiseStockStatus.DataSource = dvData.ToTable();
                ShowGridWarehousewiseStockStatus.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridWarehousewiseStockStatus_DataBinding(object sender, EventArgs e)
        {
            DataTable dt_WarehousewiseStkStat = (DataTable)Session["WarehousewiseStkStat"];
            //GridViewBandColumn bandColumn = new GridViewBandColumn();

            if (dt_WarehousewiseStkStat.Rows.Count > 0)
            {
                int maxColumnIndex = ShowGridWarehousewiseStockStatus.Columns.Count - 1;
                for (int i = maxColumnIndex; i >= 0; i--)
                {
                    ShowGridWarehousewiseStockStatus.Columns.RemoveAt(i);
                }
            }

            if (dt_WarehousewiseStkStat.Rows.Count == 0)
            {
                //GridViewBandColumn bandColumn = new GridViewBandColumn();
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "PRODDESC";
                col1.Caption = "Item Details";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 250;
                //bandColumn.Columns.Add(col1);
                ShowGridWarehousewiseStockStatus.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PRODCLASS";
                col2.Caption = "Class Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 250;
                //bandColumn.Columns.Add(col2);
                ShowGridWarehousewiseStockStatus.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "BRANDNAME";
                col3.Caption = "Brand Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 250;
                //bandColumn.Columns.Add(col3);
                ShowGridWarehousewiseStockStatus.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "STOCKUOM";
                col4.Caption = "Main Unit";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 250;
                //bandColumn.Columns.Add(col4);
                ShowGridWarehousewiseStockStatus.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col4.FieldName = "ALTUOM";
                col4.Caption = "Alt. Unit";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 250;
                //bandColumn.Columns.Add(col5);
                ShowGridWarehousewiseStockStatus.Columns.Add(col5);

                //bandColumn.Caption = "Item(s) Detail";
                //bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //ShowGridWarehousewiseStockStatus.Columns.Add(bandColumn);
            }

            if (dt_WarehousewiseStkStat.Rows.Count > 0)
            {
                string fieldname = "";
                //GridViewBandColumn bandColumn = new GridViewBandColumn();
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "PRODDESC";
                col1.Caption = "Item Details";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 300;
                //bandColumn.Columns.Add(col1);
                ShowGridWarehousewiseStockStatus.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PRODCLASS";
                col2.Caption = "Class Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 200;
                //bandColumn.Columns.Add(col2);
                ShowGridWarehousewiseStockStatus.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "BRANDNAME";
                col3.Caption = "Brand Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 200;
                //bandColumn.Columns.Add(col3);
                ShowGridWarehousewiseStockStatus.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "STOCKUOM";
                col4.Caption = "Main Unit";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 100;
                //bandColumn.Columns.Add(col4);
                ShowGridWarehousewiseStockStatus.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "ALTUOM";
                col5.Caption = "Alt. Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 100;
                //bandColumn.Columns.Add(col5);
                ShowGridWarehousewiseStockStatus.Columns.Add(col5);

                //bandColumn.Caption = "Item(s) Detail";
                //bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //ShowGridWarehousewiseStockStatus.Columns.Add(bandColumn);

                if (Session["WH_ID"] != null)
                {
                    ArrayList WHID = null;
                    ArrayList WH_Name = null;
                    WHID = new ArrayList((Session["WH_ID"].ToString()).Split(new char[] { ',' }));
                    WH_Name = new ArrayList((Session["WH_NAME"].ToString()).Split(new char[] { ',' }));

                    for (int i = 0; i < WHID.Count; i++)
                    {
                        if (dt_WarehousewiseStkStat.Columns.Contains(WHID[i].ToString()))
                        {
                            GridViewBandColumn bandColumn = new GridViewBandColumn();
                            GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                            fieldname = WHID[i].ToString() + "_" + "MAINQTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Main Qty.";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 110;
                            //coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                            if (Convert.ToString(Session["exportvalue"]) == "1")
                            {
                                coldyn.PropertiesTextEdit.DisplayFormatString = "############0.0000;";
                            }
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);

                            fieldname = WHID[i].ToString() + "_" + "ALTQTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Alt. Qty.";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 110;
                            //coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                            if (Convert.ToString(Session["exportvalue"]) == "1")
                            {
                                coldyn.PropertiesTextEdit.DisplayFormatString = "############0.0000;";
                            }
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);

                            fieldname = WHID[i].ToString() + "_" + "AMOUNT";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Amount";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 130;
                            //coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                            if (Convert.ToString(Session["exportvalue"]) == "1")
                            {
                                coldyn.PropertiesTextEdit.DisplayFormatString = "############0.00;";
                            }
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);

                            bandColumn.Caption = WH_Name[i].ToString();
                            bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            ShowGridWarehousewiseStockStatus.Columns.Add(bandColumn);

                            if (i == 0)
                            {
                                hdnWHStkStatNoBandedColumn.Value = WH_Name[i].ToString();
                                hdnWHStkStatNoCaption.Value = "Main Qty." + "~" + "Alt. Qty." + "~" + "Amount";
                                hdnWHStkStatNoFields.Value = WHID[i].ToString() + "_" + "MAINQTY" + "~" + WHID[i].ToString() + "_" + "ALTQTY" + "~" + WHID[i].ToString() + "_" + "AMOUNT";
                            }
                            else
                            {
                                hdnWHStkStatNoBandedColumn.Value = hdnWHStkStatNoBandedColumn.Value + "~" + WH_Name[i].ToString();
                                hdnWHStkStatNoCaption.Value = hdnWHStkStatNoCaption.Value + "~" + "Main Qty." + "~" + "Alt. Qty." + "~" + "Amount";
                                hdnWHStkStatNoFields.Value = hdnWHStkStatNoFields.Value + "~" + WHID[i].ToString() + "_" + "MAINQTY" + "~" + WHID[i].ToString() + "_" + "ALTQTY" + "~" + WHID[i].ToString() + "_" + "AMOUNT";
                            }
                        }
                    }
                }

                if (Session["WHStkStatGridviewsBandedFields"] == null)
                {
                    Session["WHStkStatGridviewsBandedFields"] = hdnWHStkStatNoBandedColumn.Value;
                    Session["WHStkStatHeadersAmountCaption"] = hdnWHStkStatNoCaption.Value;
                    Session["WHStkStatHeadersAmountFields"] = hdnWHStkStatNoFields.Value;
                }

                GridViewDataTextColumn Totcol1 = new GridViewDataTextColumn();
                Totcol1.FieldName = "TOTAL_MAINSTOCK";
                Totcol1.Caption = "Total Main Qty.";
                Totcol1.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol1.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol1.Width = 110;
                //Totcol1.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                if (Convert.ToString(Session["exportvalue"]) == "1")
                {
                    Totcol1.PropertiesTextEdit.DisplayFormatString = "############0.0000;";
                }
                Totcol1.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                //bandColumn.Columns.Add(Totcol1);
                ShowGridWarehousewiseStockStatus.Columns.Add(Totcol1);

                GridViewDataTextColumn Totcol2 = new GridViewDataTextColumn();
                Totcol2.FieldName = "TOTAL_ALTSTOCK";
                Totcol2.Caption = "Total Alt. Qty.";
                Totcol2.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol2.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol2.Width = 110;
                //Totcol2.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                if (Convert.ToString(Session["exportvalue"]) == "1")
                {
                    Totcol2.PropertiesTextEdit.DisplayFormatString = "############0.0000;";
                }
                Totcol2.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                //bandColumn.Columns.Add(Totcol2);
                ShowGridWarehousewiseStockStatus.Columns.Add(Totcol2);

                GridViewDataTextColumn Totcol3 = new GridViewDataTextColumn();
                Totcol3.FieldName = "TOTAL_AMOUNT";
                Totcol3.Caption = "Total Amount";
                Totcol3.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol3.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                Totcol3.Width = 130;
                //Totcol3.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                if (Convert.ToString(Session["exportvalue"]) == "1")
                {
                    Totcol3.PropertiesTextEdit.DisplayFormatString = "############0.00;";
                }
                Totcol3.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                //bandColumn.Columns.Add(Totcol3);
                ShowGridWarehousewiseStockStatus.Columns.Add(Totcol3);

                //bandColumn.Caption = "Total";
                //bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //ShowGridWarehousewiseStockStatus.Columns.Add(bandColumn);

                ShowGridWarehousewiseStockStatus.DataSource = dt_WarehousewiseStkStat;
            }
        }

        protected void ShowGridWarehousewiseStockStatus_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                DataTable dt_WarehousewiseSummaryTotal = (DataTable)Session["WarehousewiseSummaryTotal"];
                if (dt_WarehousewiseSummaryTotal.Rows.Count > 0)
                {

                    if (amountfields_caption == null)
                    {
                        amountfields_caption = new ArrayList((Session["WHStkStatHeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                        amountfields_fields = new ArrayList((Session["WHStkStatHeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                        bandedfields = new ArrayList((Session["WHStkStatGridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                    }
                    else
                    {
                        if (e.Column.Caption != "Item Details" && e.Column.Caption != "Class Name" && e.Column.Caption != "Brand Name" && e.Column.Caption != "Main Unit" && e.Column.Caption != "Alt. Unit" && e.Column.Caption != "Total Main Qty." && e.Column.Caption != "Total Alt. Qty." && e.Column.Caption != "Total Amount")
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
                        else if (e.Column.Caption == "Total Main Qty." || e.Column.Caption == "Total Alt. Qty." || e.Column.Caption == "Total Amount")
                        {                            
                            if (e.IsTotalFooter)
                            {
                                if (e.Column.Caption == "Total Main Qty.")
                                {
                                    e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0]["TOTAL_MAINSTOCK"].ToString();
                                }
                                else if (e.Column.Caption == "Total Alt. Qty.")
                                {
                                    e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0]["TOTAL_ALTSTOCK"].ToString();
                                }
                                else if (e.Column.Caption == "Total Amount")
                                {
                                    e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0]["TOTAL_AMOUNT"].ToString();
                                }
                            }
                        }
                    }
                }
                if (e.Column.Caption == "Alt. Unit")
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