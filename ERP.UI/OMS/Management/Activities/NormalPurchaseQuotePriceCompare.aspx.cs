using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class NormalPurchaseQuotePriceCompare : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
           rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/NormalPurchaseQuotePriceCompare.aspx");
          //  rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseQuotePriceComparison.aspx");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                Session["SI_ComponentData"] = null;
                Session["SI_ProductData"] = null;
                Session["LocationewiseStkStat"] = null;
            }

            if (ShowgridPurchaseQuotationPrice.Columns["Document_No"] == null)
            {
                //==============By default Grid populate in load===================
                DataTable dt = new DataTable();
                dt.Columns.Add("Document_No");
                dt.Columns.Add("sProducts_Code");
                dt.Columns.Add("sProducts_Description");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Unit_Name");

                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "Document_No";
                col1.Caption = "Quotation No.";
                col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 200;
                ShowgridPurchaseQuotationPrice.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "sProducts_Code";
                col2.Caption = "Item Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 250;
                ShowgridPurchaseQuotationPrice.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "sProducts_Description";
                col3.Caption = "Description";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                ShowgridPurchaseQuotationPrice.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "Quantity";
                col4.Caption = "Quantity";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 100;
                ShowgridPurchaseQuotationPrice.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "Unit_Name";
                col5.Caption = "Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 120;
                ShowgridPurchaseQuotationPrice.Columns.Add(col5);
                //=====================================================================

            }


        }






        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                //Session["exportTotalSummary"] = "1";
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

            string filename = "QuotationPriceComparison";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            // FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Location Wise - Stock In Hand" + Environment.NewLine + "As On :" + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowgridPurchaseQuotationPrice";
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
        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ComponentQuotationPanel.JSProperties["cpCloseQupotation"] = "";
            if (hdncWiseVendorId.Value != "" && hdncWiseVendorId.Value != null)
            {
                string FromDate = Convert.ToString(FFromDate.Date.ToString("yyyy-MM-dd"));
                string ToDate = Convert.ToString(FToDate.Date.ToString("yyyy-MM-dd"));
                DataTable ComponentTable = GetQuotation(hdncWiseVendorId.Value, FromDate, ToDate);
                Session["SI_ComponentData"] = ComponentTable;
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();


                ComponentQuotationPanel.JSProperties["cpCloseQupotation"] = "CloseQupotation";
            }
            else
            {
                Session["SI_ComponentData"] = null;
                lookup_quotation.DataBind();
            }


        }


        protected void ShowgridPurchaseQuotationPrice_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowgridPurchaseQuotationPrice.DataSource = null;

            Session["LocationewiseStkStat"] = null;
            DateTime dtTodate, dtFromdate;




            dtTodate = Convert.ToDateTime(FToDate.Date);
            dtFromdate = Convert.ToDateTime(FFromDate.Date);
            string ASONDATE = dtTodate.ToString("yyyy-MM-dd");
            string ASONFromDATE = dtFromdate.ToString("yyyy-MM-dd");


            if (Convert.ToString(hdnProductId.Value) != "")
            {
                Task PopulateStockTrialDataTask = new Task(() => GetGriddata(ASONFromDATE, ASONDATE, hdncWiseVendorId.Value, hdnQuoteId.Value, hdnProductId.Value));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }



        protected void ShowgridPurchaseQuotationPrice_DataBinding(object sender, EventArgs e)
        {
            DataTable dt_LocationwiseStkStat = (DataTable)Session["LocationewiseStkStat"];
            //if (Session["exportTotalSummary"].ToString() == "1")
            //{
            //    DataTable dt_WarehousewiseSummaryTotal = (DataTable)Session["LocationwiseSummaryTotal"];
            //    dt_LocationwiseStkStat.Merge(dt_WarehousewiseSummaryTotal);
            //}

            if (dt_LocationwiseStkStat.Rows.Count > 0)
            {
                int maxColumnIndex = ShowgridPurchaseQuotationPrice.Columns.Count - 1;
                for (int i = maxColumnIndex; i >= 0; i--)
                {
                    ShowgridPurchaseQuotationPrice.Columns.RemoveAt(i);
                }
            }

            if (dt_LocationwiseStkStat.Rows.Count == 0)
            {
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "Document_No";
                col1.Caption = "Quotation No.";
                col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 200;
                ShowgridPurchaseQuotationPrice.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "sProducts_Code";
                col2.Caption = "Item Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 250;
                ShowgridPurchaseQuotationPrice.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "sProducts_Description";
                col3.Caption = "Description";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                ShowgridPurchaseQuotationPrice.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "Quantity";
                col4.Caption = "Quantity";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 100;
                ShowgridPurchaseQuotationPrice.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "Unit_Name";
                col5.Caption = "Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 120;
                ShowgridPurchaseQuotationPrice.Columns.Add(col5);
            }



            if (dt_LocationwiseStkStat.Rows.Count > 0)
            {
                string fieldname = "";
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "Document_No";
                col1.Caption = "Quotation No.";
                col1.FixedStyle = DevExpress.Web.GridViewColumnFixedStyle.Left;
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 200;
                ShowgridPurchaseQuotationPrice.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "sProducts_Code";
                col2.Caption = "Item Name";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 250;
                ShowgridPurchaseQuotationPrice.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "sProducts_Description";
                col3.Caption = "Description";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                ShowgridPurchaseQuotationPrice.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "Quantity";
                col4.Caption = "Quantity";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 100;
                ShowgridPurchaseQuotationPrice.Columns.Add(col4);

                GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                col5.FieldName = "Unit_Name";
                col5.Caption = "Unit";
                col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col5.Width = 120;
                ShowgridPurchaseQuotationPrice.Columns.Add(col5);

                if (Convert.ToString(hdncWiseVendorId.Value) != "")
                {

                    ArrayList VendorID = null;
                    ArrayList Vendor_Name = null;
                    if (hdncWiseVendorId.Value == "AllVEND")
                    {
                        DataTable dt = oDBEngine.GetDataTable("select  STUFF((SELECT DISTINCT ',' + +T.Customer_Id FROM tbl_trans_PurchaseQuotation AS T where ISNULL(T.IsProjectPurchaseQuotation,0)=0 FOR XML PATH('') ),1,1,'') AS VENDORID");
                        DataTable dts = oDBEngine.GetDataTable("select STUFF((  SELECT DISTINCT ',' + +(isnull(tmc.cnt_firstName,'')+''+isnull(tmc.cnt_middleName,'')+''+isnull(tmc.cnt_lastName,'')) FROM tbl_trans_PurchaseQuotation ttp inner join tbl_master_contact tmc on ttp.Customer_Id=tmc.cnt_internalId  where ISNULL(IsProjectPurchaseQuotation,0)=0 FOR XML PATH('') ),1,1,'') AS VendorName");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string VENDORID = Convert.ToString(dt.Rows[0]["VENDORID"]);
                            string VendorName = Convert.ToString(dts.Rows[0]["VendorName"]);
                            VendorID = new ArrayList((VENDORID.ToString()).Split(new char[] { ',' }));
                            Vendor_Name = new ArrayList((Convert.ToString(VendorName)).Split(new char[] { ',' }));
                        }

                    }
                    else
                    {
                        VendorID = new ArrayList((hdncWiseVendorId.Value.ToString()).Split(new char[] { ',' }));
                        Vendor_Name = new ArrayList((Convert.ToString(txtVendorName.Text)).Split(new char[] { ',' }));
                    }

                    for (int i = 0; i < VendorID.Count; i++)
                    {
                        if (dt_LocationwiseStkStat.Columns.Contains(VendorID[i].ToString()))
                        {
                            GridViewBandColumn bandColumn = new GridViewBandColumn();
                            GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                            fieldname = VendorID[i].ToString() + "_" + "PRICE";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "PRICE.";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 200;
                            coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);

                            bandColumn.Caption = Vendor_Name[i].ToString();
                            bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            ShowgridPurchaseQuotationPrice.Columns.Add(bandColumn);

                            if (i == 0)
                            {
                                hdnLCStkStatNoBandedColumn.Value = Vendor_Name[i].ToString();
                                hdnLCStkStatNoCaption.Value = "PRICE.";
                                hdnLCStkStatNoFields.Value = VendorID[i].ToString() + "_" + "PRICE";
                            }
                            else
                            {
                                hdnLCStkStatNoBandedColumn.Value = hdnLCStkStatNoBandedColumn.Value + "~" + Vendor_Name[i].ToString();
                                hdnLCStkStatNoCaption.Value = hdnLCStkStatNoCaption.Value + "~" + "PRICE.";
                                hdnLCStkStatNoFields.Value = hdnLCStkStatNoFields.Value + "~" + VendorID[i].ToString() + "_" + "PRICE";
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

                //GridViewDataTextColumn Totcol1 = new GridViewDataTextColumn();
                //Totcol1.FieldName = "TOTAL_CLOSEQTY";
                //Totcol1.Caption = "Total Qty.";
                //Totcol1.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                //Totcol1.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                //Totcol1.Width = 110;
                //Totcol1.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.0000;";
                //Totcol1.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                //ShowgridPurchaseQuotationPrice.Columns.Add(Totcol1);

                ShowgridPurchaseQuotationPrice.DataSource = dt_LocationwiseStkStat;
            }
        }
        protected void ShowgridPurchaseQuotationPrice_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                //DataTable dt_WarehousewiseSummaryTotal = (DataTable)Session["LocationwiseSummaryTotal"];
                //if (dt_WarehousewiseSummaryTotal.Rows.Count > 0)
                //{

                //    if (amountfields_caption == null)
                //    {
                //        amountfields_caption = new ArrayList((Session["LCStkStatHeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                //        amountfields_fields = new ArrayList((Session["LCStkStatHeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                //        bandedfields = new ArrayList((Session["LCStkStatGridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                //    }
                //    else
                //    {
                //        if (e.Column.Caption != "Item Details" && e.Column.Caption != "Class Name" && e.Column.Caption != "Brand Name" && e.Column.Caption != "Main Unit" && e.Column.Caption != "Total Qty.")
                //        {
                //            if (e.Column.ParentBand.ToString() == bandedfields[(e.Column.ParentBand.VisibleIndex) - 4].ToString())
                //            {
                //                if (e.Column.Caption == amountfields_caption[0].ToString())
                //                {
                //                    if (e.IsTotalFooter)
                //                    {
                //                        e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                //                        amountfields_fields.RemoveAt(0);
                //                        amountfields_caption.RemoveAt(0);
                //                    }
                //                }
                //            }
                //        }
                //        else if (e.Column.Caption == "Total Qty.")
                //        {
                //            if (e.IsTotalFooter)
                //            {
                //                if (e.Column.Caption == "Total Qty.")
                //                {
                //                    e.Cell.Text = dt_WarehousewiseSummaryTotal.Rows[0]["TOTAL_CLOSEQTY"].ToString();
                //                }
                //            }
                //        }
                //    }
                //}
                //if (e.Column.Caption == "Main Unit")
                //{
                //    if (e.IsTotalFooter)
                //    {
                //        e.Cell.Text = "Net Total:";
                //    }
                //}
            }

        }
        public void GetGriddata(string ASONFromDATE, string ASONDATE, string VendorId, string QuoteId, string ProductId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_VendorWiseNormalQuotationPriceFETCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", ASONFromDATE);
                cmd.Parameters.AddWithValue("@TODATE", ASONDATE);
                cmd.Parameters.AddWithValue("@VendorId", VendorId);
                cmd.Parameters.AddWithValue("@QuoteId", QuoteId);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();

                DataTable dt = new DataTable();

                DataView dvData = new DataView(ds.Tables[0]);
                //dvData.RowFilter = "STOCKUOM <> 'Net Total:'";
                Session["LocationewiseStkStat"] = dvData.ToTable();

                //DataView dvData1 = new DataView(ds.Tables[0]);
                //dvData1.RowFilter = "STOCKUOM = 'Net Total:'";
                //Session["LocationwiseSummaryTotal"] = dvData1.ToTable();

                ShowgridPurchaseQuotationPrice.DataSource = dvData.ToTable();
                ShowgridPurchaseQuotationPrice.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable GetQuotation(string CustId, string FromDate, String ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_NormalPurchaseQuotation_PriceComparison");
            proc.AddVarcharPara("@Action", 500, "GetPurchaseQuotationBasedOnVendor");
            proc.AddVarcharPara("@CustomerID", 500, CustId);
            proc.AddDateTimePara("@FromDate", Convert.ToDateTime(FromDate));
            proc.AddDateTimePara("@ToDate", Convert.ToDateTime(ToDate));
            dt = proc.GetTable();
            return dt;
        }
        protected void cMaterialsCallbackpanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            MaterialsCallbackpanel.JSProperties["cpCloseMaterialsCallback"] = "";
            // string strSplitCommandValue = e.Parameter.Split('~')[0];
            string FromDate = Convert.ToString(FFromDate.Date.ToString("yyyy-MM-dd"));
            string ToDate = Convert.ToString(FToDate.Date.ToString("yyyy-MM-dd"));
            string ComponentDetailsIDs = string.Empty;
            if (Convert.ToString(hdnQuoteId.Value) == "")
            {
                if (chkAllQuotation.Value == null || Convert.ToString(chkAllQuotation.Value) == "false")
                {
                    for (int i = 0; i < lookup_quotation.GridView.GetSelectedFieldValues("PurchaseQuotation_Id").Count; i++)
                    {
                        ComponentDetailsIDs += "," + Convert.ToString(lookup_quotation.GridView.GetSelectedFieldValues("PurchaseQuotation_Id")[i]);

                    }
                }
                if (chkAllQuotation.Value == null || Convert.ToString(chkAllQuotation.Value) == "false")
                {
                    ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');

                    hdnQuoteId.Value = ComponentDetailsIDs;
                }
            }


            if (hdnQuoteId.Value != "" && hdnQuoteId.Value != null)
            {
                DataTable ComponentTable = GetProductDetails(hdncWiseVendorId.Value, hdnQuoteId.Value, FromDate, ToDate);
                Session["SI_ProductData"] = ComponentTable;
                MaterialsLookup.GridView.Selection.CancelSelection();
                MaterialsLookup.DataSource = ComponentTable;
                MaterialsLookup.DataBind();


                MaterialsCallbackpanel.JSProperties["cpCloseMaterialsCallback"] = "CloseMaterialsCallback";
            }
            else
            {
                Session["SI_ProductData"] = null;
                MaterialsLookup.DataBind();
            }


        }


        public DataTable GetProductDetails(string CustId, string QuoteId, string FromDate, String ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_NormalPurchaseQuotation_PriceComparison");
            proc.AddVarcharPara("@Action", 500, "GetProductBasedQuotationBasedOnVendor");
            proc.AddVarcharPara("@CustomerID", 4000, CustId);
            proc.AddVarcharPara("@QuoteId", 4000, QuoteId);
            proc.AddDateTimePara("@FromDate", Convert.ToDateTime(FromDate));
            proc.AddDateTimePara("@ToDate", Convert.ToDateTime(ToDate));
            dt = proc.GetTable();
            return dt;
        }

        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
            }
            else
            {
                lookup_quotation.DataSource = null;
            }

        }
        protected void lookup_Materials_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ProductData"] != null)
            {
                MaterialsLookup.DataSource = (DataTable)Session["SI_ProductData"];
            }
            else
            {
                MaterialsLookup.DataSource = null;
            }
        }

    }
}