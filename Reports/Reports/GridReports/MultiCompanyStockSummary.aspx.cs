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
    public partial class MultiCompanyStockSummary : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.MasterDbEngine oDBEngine = new BusinessLogicLayer.MasterDbEngine();
        BusinessLogicLayer.DBEngine oDBEngine1 = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ArrayList amountfields_caption = null;
        ArrayList amountfields_fields = null;
        ArrayList bandedfields = null;
        int gridvisibleindex = 1;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine1.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/MultiCompanyStockSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                Session["GridviewsBandedFields"] = null;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Multi Company Stock Summary";
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

                Session["Company_Name"] = null;
                Session["SI_ComponentData_Company"] = null;
                Session["Company_ID"] = null;
                DataTable dt_MultiCompanyStock = null;                

                //if (dt_MultiCompanyStock == null)
                //{
                //    string fieldname = "";
                //    //string fieldnamecheck = "";

                //    if (gridvisibleindex == 1)
                //    {
                //        GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                //        col1.FieldName = "PRODDESC";
                //        col1.Caption = "Product Description";
                //        col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                //        col1.Width = 600;
                //        ShowGridCompanyStockSummary.Columns.Add(col1);

                //        GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                //        col2.FieldName = "PRODCLASSDESC";
                //        col2.Caption = "Class";
                //        col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                //        col2.Width = 600;
                //        ShowGridCompanyStockSummary.Columns.Add(col2);

                //        GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                //        col3.FieldName = "BRANDNAME";
                //        col3.Caption = "Brand";
                //        col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                //        col3.Width = 600;
                //        ShowGridCompanyStockSummary.Columns.Add(col3);
                //        gridvisibleindex = gridvisibleindex + 1;
                //    }
                //}

                //if (dt_MultiCompanyStock == null)
                    if (ShowGridCompanyStockSummary.Columns["PRODDESC"] == null)
                    {

                        GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                        col1.FieldName = "PRODDESC";
                        col1.Caption = "Product Description";
                        col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col1.Width = 600;
                        ShowGridCompanyStockSummary.Columns.Add(col1);

                        GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                        col2.FieldName = "PRODCLASSDESC";
                        col2.Caption = "Class";
                        col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col2.Width = 600;
                        ShowGridCompanyStockSummary.Columns.Add(col2);

                        GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                        col3.FieldName = "BRANDNAME";
                        col3.Caption = "Brand";
                        col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col3.Width = 600;
                        ShowGridCompanyStockSummary.Columns.Add(col3);
                    }

            }
        }
        public void Date_finyearwise(string Finyear)
        {
            CommonBL pledgcust = new CommonBL();
            DataTable dtpledgcust = new DataTable();

            dtpledgcust = pledgcust.GetDateFinancila(Finyear);
            if (dtpledgcust.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

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

            string filename = "Multi Company Stock Summary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Multi Company Stock Summary" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridCompanyStockSummary";
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
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion Export
        protected void ComponentCompany_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindCompanyGrid")
            {                
                DataTable dtCompany = oDBEngine.GetDataTable("select DbName DbName,Name Company_Name,Company_Code from ERP_Company_List where IsActive=1");
                if (dtCompany.Rows.Count > 0)
                {
                    Session["SI_ComponentData_Company"] = dtCompany;

                    lookup_company.DataSource = dtCompany;
                    lookup_company.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Company"] = dtCompany;
                    lookup_company.DataSource = null;
                    lookup_company.DataBind();
                }
            }
        }
        protected void lookup_company_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Company"] != null)
            {               
                lookup_company.DataSource = (DataTable)Session["SI_ComponentData_Company"];
            }
        }       

        protected void ShowGridCompanyStockSummary_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session["GridviewsBandedFields"] = null;
            ShowGridCompanyStockSummary.DataSource = null;
           
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);


            string Company_ID = "";
            string ComanyComponent = "";
            List<object> BranchList = lookup_company.GridView.GetSelectedFieldValues("Company_Code");
            //foreach (object Brnch in BranchList)
            //{
            //    ComanyComponent += "," + Brnch;
            //}
            //Company_ID = ComanyComponent.TrimStart(',');

            //Session["Company_ID"] = Company_ID;

            string db_NAME = "";
            string dbNameComponent = "";
            List<object> dbNameList = lookup_company.GridView.GetSelectedFieldValues("DbName");
            foreach (object dbName in dbNameList)
            {
                dbNameComponent += "," + dbName;
            }
            db_NAME = dbNameComponent.TrimStart(',');

            string Company_Name = "";
            string ComanyNameComponent = "";
            List<object> ComanyNameList = lookup_company.GridView.GetSelectedFieldValues("Company_Name", "Company_Code");
            //foreach (object CompantName in ComanyNameList)
            //{
            //    ComanyNameComponent += "," + CompantName;
            //}

          
            foreach (object[] item in ComanyNameList)
            {
                ComanyNameComponent += "," + item[0].ToString();
                ComanyComponent += "," + item[1].ToString();
            }

            Company_Name = ComanyNameComponent.TrimStart(',');
            Session["Company_Name"] = Company_Name;

            Company_ID = ComanyComponent.TrimStart(',');
            Session["Company_ID"] = Company_ID;

            
            Task PopulateStockTrialDataTask = new Task(() => GetCompanyStockSummarydata(Company_ID, db_NAME));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetCompanyStockSummarydata(string Company_ID, string db_NAME)
        {
            try
            {
                string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_MULTICOMPANYSTOCKSUMMARYFETCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Company_ID);
                cmd.Parameters.AddWithValue("@MASTERDB",  masterdbname);
                cmd.Parameters.AddWithValue("@FROMDATE", ASPxFromDate.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TODATE", ASPxToDate.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdncWiseProductId.Value);
                
                
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();

                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("PRODDESC") != "Net Total:").CopyToDataTable();
                }

                DataView dvData = new DataView(ds.Tables[0]);
                dvData.RowFilter = "PRODDESC <> 'Net Total:'";

                Session["CompanyStockSummary"] = dvData.ToTable();

                DataView dvData1 = new DataView(ds.Tables[0]);
                dvData1.RowFilter = "PRODDESC = 'Net Total:'";
               
                Session["CompanyStockSummaryTotal"] = dvData1.ToTable();

                ShowGridCompanyStockSummary.DataSource = dvData.ToTable();
                ShowGridCompanyStockSummary.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridCompanyStockSummary_DataBinding(object sender, EventArgs e)
        {
            DataTable dt_MultiCompanyStock = (DataTable)Session["CompanyStockSummary"];

            //if (dt_MultiCompanyStock != null)
            if (dt_MultiCompanyStock.Rows.Count>0)
            {
                int maxColumnIndex = ShowGridCompanyStockSummary.Columns.Count - 1;
                for (int i = maxColumnIndex; i >= 0; i--)
                {
                    ShowGridCompanyStockSummary.Columns.RemoveAt(i);
                }
            }

            if (dt_MultiCompanyStock.Rows.Count == 0)
            {
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "PRODDESC";
                col1.Caption = "Product Description";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 600;
                ShowGridCompanyStockSummary.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PRODCLASSDESC";
                col2.Caption = "Class";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 600;
                ShowGridCompanyStockSummary.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "BRANDNAME";
                col3.Caption = "Brand";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 600;
                ShowGridCompanyStockSummary.Columns.Add(col3);
            }

            //if (dt_MultiCompanyStock != null)
            if (dt_MultiCompanyStock.Rows.Count > 0)
            {
                string fieldname = "";
                //string fieldnamecheck = "";
                
                //if (gridvisibleindex == 1)
                //{
                //if (ShowGridCompanyStockSummary.Columns["PRODDESC"] == null)
                //{
                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "PRODDESC";
                    col1.Caption = "Product Description";
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 350;
                    ShowGridCompanyStockSummary.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "PRODCLASSDESC";
                    col2.Caption = "Class";
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 200;
                    ShowGridCompanyStockSummary.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "BRANDNAME";
                    col3.Caption = "Brand";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 200;
                    ShowGridCompanyStockSummary.Columns.Add(col3);
                    //    gridvisibleindex = gridvisibleindex + 1;
                    //}
                //}
                if (Session["Company_ID"]!=null)
                {
                    ArrayList CompanyCode = null;
                    ArrayList Company_Name = null;
                    CompanyCode = new ArrayList((Session["Company_ID"].ToString()).Split(new char[] { ',' }));
                    Company_Name= new ArrayList((Session["Company_Name"].ToString()).Split(new char[] { ',' }));

                    for(int i=0;i<CompanyCode.Count;i++)
                    {
                        if (dt_MultiCompanyStock.Columns.Contains(CompanyCode[i].ToString()))
                        {
                            GridViewBandColumn bandColumn = new GridViewBandColumn();

                            GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                            fieldname = CompanyCode[i].ToString() + "_" + "IN_QTY_OP";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Opening";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 200;
                            bandColumn.Columns.Add(coldyn);

                            fieldname = CompanyCode[i].ToString() + "_" + "IN_QTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Received";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 200;
                            bandColumn.Columns.Add(coldyn);

                            fieldname = CompanyCode[i].ToString() + "_" + "OUT_QTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Delivered";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 200;
                            bandColumn.Columns.Add(coldyn);

                            fieldname = CompanyCode[i].ToString() + "_" + "CLOSE_QTY";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = "Close";
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 200;
                            bandColumn.Columns.Add(coldyn);

                            bandColumn.Caption = Company_Name[i].ToString();
                            bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            ShowGridCompanyStockSummary.Columns.Add(bandColumn);

                            if(i==0)
                            {
                                hdnNoBandedColumn.Value =  Company_Name[i].ToString();
                                hdnNoCaption.Value = "Opening" + "~" + "Received" + "~" + "Delivered" + "~" + "Close";
                                hdnNoFields.Value = CompanyCode[i].ToString() + "_" + "IN_QTY_OP" + "~" + CompanyCode[i].ToString() + "_" + "IN_QTY" + "~" + CompanyCode[i].ToString() + "_" + "OUT_QTY" + "~" + CompanyCode[i].ToString() + "_" + "CLOSE_QTY";
                            }
                            else
                            {
                                hdnNoBandedColumn.Value = hdnNoBandedColumn.Value + "~" + Company_Name[i].ToString();
                                hdnNoCaption.Value = hdnNoCaption.Value+"~"+ "Opening" + "~" + "Received" + "~" + "Delivered" + "~" + "Close";
                                hdnNoFields.Value = hdnNoFields.Value + "~" + CompanyCode[i].ToString() + "_" + "IN_QTY_OP" + "~" + CompanyCode[i].ToString() + "_" + "IN_QTY" + "~" + CompanyCode[i].ToString() + "_" + "OUT_QTY" + "~" + CompanyCode[i].ToString() + "_" + "CLOSE_QTY";
                            }                          
                        }
                    }
                }

                if (Session["GridviewsBandedFields"] == null)
                {
                    Session["GridviewsBandedFields"] = hdnNoBandedColumn.Value;
                    Session["HeadersAmountCaption"] = hdnNoCaption.Value;
                    Session["HeadersAmountFields"] = hdnNoFields.Value;
                }

                ShowGridCompanyStockSummary.DataSource = dt_MultiCompanyStock;
            }
            
        }

        protected void ShowGridCompanyStockSummary_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                DataTable dt_CompanyStockSummaryTotal = (DataTable)Session["CompanyStockSummaryTotal"];
                //if (dt_CompanyStockSummaryTotal != null)
                if (dt_CompanyStockSummaryTotal.Rows.Count>0)
                {

                    if (amountfields_caption == null)
                    {
                        amountfields_caption = new ArrayList((Session["HeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                        amountfields_fields = new ArrayList((Session["HeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                        bandedfields = new ArrayList((Session["GridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                    }
                    else
                    {
                        if (e.Column.Caption != "Product Description" && e.Column.Caption != "Class" && e.Column.Caption != "Brand")
                        {
                            if (e.Column.ParentBand.ToString() == bandedfields[(e.Column.ParentBand.VisibleIndex) - 3].ToString())
                            //if (e.Column.ParentBand.ToString() == bandedfields[e.Column.ParentBand.VisibleIndex].ToString())
                            {
                                if (e.Column.Caption == amountfields_caption[0].ToString())
                                {
                                    if (e.IsTotalFooter)
                                    {
                                        e.Cell.Text = dt_CompanyStockSummaryTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                                        amountfields_fields.RemoveAt(0);
                                        amountfields_caption.RemoveAt(0);
                                       // bandedfields.RemoveAt(0);
                                    }


                                }
                            }
                        }
                    }

                }
                if (e.Column.Caption == "Product Description")
                {
                    if (e.IsTotalFooter)
                    {
                        e.Cell.Text = "Net Total:";
                    }
                }
            }
        }


    }
}