using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.OMS.Management.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;

namespace ERP.OMS.Reports.Master
{
    public partial class SalesAnalysis : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        static string _ClassID = "";
        static string _CategoryID = "";
        static string _ProductID = "";
        static decimal _PU_RATE = 0;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lookupClass.DataSource = GetClassList();
                lookupClass.DataBind();

                lookupBrand.DataSource = GetBrandList();
                lookupBrand.DataBind();

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                DataTable dt = oDBEngine.GetDataTable("Select FinYear_Code,FinYear_StartDate,FinYear_EndDate From Master_FinYear Where FinYear_Code='" + strFinYear + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strStartDate = Convert.ToString(dt.Rows[0]["FinYear_StartDate"]);
                    DateTime StartDate = Convert.ToDateTime(strStartDate);

                    ASPxFromDate.Value = StartDate;
                    ASPxToDate.Value = DateTime.Now;
                }
                else
                {
                    ASPxFromDate.Value = DateTime.Now;
                    ASPxToDate.Value = DateTime.Now;
                }
            }
        }

        #region Lookup Details

        protected void lookupClass_DataBinding(object sender, EventArgs e)
        {
            lookupClass.DataSource = GetClassList();
        }
        protected void lookupBrand_DataBinding(object sender, EventArgs e)
        {
            lookupBrand.DataSource = GetBrandList();
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strProductName = "", strProducCode = "";

            DataTable dt = oDBEngine.GetDataTable("Select sProducts_Code,sProducts_Name From Master_sProducts Where sProducts_ID='" + strProductID + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                strProducCode = Convert.ToString(dt.Rows[0]["sProducts_Code"]).Replace("'", "squot").Replace(",", "coma").Replace("/", "slash").Trim();
                strProductName = Convert.ToString(dt.Rows[0]["sProducts_Name"]).Replace("'", "squot").Replace(",", "coma").Replace("/", "slash").Trim();
            }

            CallbackPanel.JSProperties["cpProductValue"] = strProducCode + "||@||" + strProductName;
        }

        #endregion

        #region Grid Details

        protected void ShowGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable dtanalysis = new DataTable();
            Task PopulateStockTrialDataTask = new Task(() =>   GetSalesAnalysisHeader()  );
            PopulateStockTrialDataTask.RunSynchronously(); 
        }
        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["ReportDataSource"] != null) 
            {
                ShowGrid.DataSource = (DataTable)Session["ReportDataSource"]; 
            }
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void ShowGridDetails_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            _ClassID = Convert.ToString(hdnClassID.Value);
            _CategoryID = Convert.ToString(hdnCategoryID.Value);
            _ProductID = Convert.ToString(hdnProductID.Value);

            if (Convert.ToString(hdnRate.Value) != "")
            {
                _PU_RATE = Convert.ToDecimal(hdnRate.Value);
            }
            else
            {
                _PU_RATE = 0;
            }
            ShowGridDetails.DataSource = GetSalesAnalysisDetails();
            ShowGridDetails.DataBind();
        }
        protected void ShowGridDetails_DataBinding(object sender, EventArgs e)
        {
            if (Session["DetReportDataSource"] != null)
            {
                ShowGridDetails.DataSource = (DataTable)Session["DetReportDataSource"];
            }
        }
        protected void ShowGridDetails_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region Export Details
        protected void ddldetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details(Filter);
            }
        }
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        public void bindexport(int Filter)
        {
            string filename = "SalesAnalysis Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Sales Analysis Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
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

                default:
                    return;
            }
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        public void bindexport_Details(int Filter)
        {
            string filename = "SalesAnalysisDetails Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "SalesAnalysisDetailsReport";

            exporterDetails.PageHeader.Left = "SalesAnalysisDetails Report";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsToResponse();
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        #endregion

        #region Database Details

        public DataTable GetClassList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("Select ProductClass_ID,ProductClass_Name From Master_ProductClass Order By ProductClass_Name Asc");
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetBrandList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("Select Brand_Id,Brand_Name From tbl_master_brand Where Brand_IsActive=1 Order By Brand_Name Asc");
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetSalesAnalysisHeader()
        {
            string strClassList = "", strBrandList = "";
            string strCompany = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            string strRateCriteria = Convert.ToString(ddlRateCriteria.SelectedValue);

            if (lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID").Count() != GetClassList().Rows.Count)
            {
                List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                foreach (object Class in ClassList)
                {
                    strClassList += "," + Class;
                }
                strClassList = strClassList.TrimStart(',');
            }

            if (lookupBrand.GridView.GetSelectedFieldValues("Brand_Id").Count() != GetBrandList().Rows.Count)
            {
                List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                foreach (object Brand in BrandList)
                {
                    strBrandList += "," + Brand;
                }
                strBrandList = strBrandList.TrimStart(',');
            }
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesAnalysis_Report");
                proc.AddPara("@COMPANYID", strCompany);
                proc.AddPara("@FINYEAR", strFinYear);
                proc.AddPara("@FROMDATE", strFromDate);
                proc.AddPara("@TODATE", strToDate);
                proc.AddPara("@CLASS", strClassList);
                proc.AddPara("@CATEGORY", strBrandList);
                proc.AddBooleanPara("@ISGST", Convert.ToBoolean(strRateCriteria));
                dt = proc.GetTable();

                Session["ReportDataSource"] = dt;
                ShowGrid.DataBind();
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetSalesAnalysisDetails()
        {
            string strClassList = "", strBrandList = "";
            string strCompany = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            string strRateCriteria = Convert.ToString(ddlRateCriteria.SelectedValue);
            string strClassID = Convert.ToString(_ClassID);
            string strCategoryID = Convert.ToString(_CategoryID);
            string strProductID = Convert.ToString(_ProductID);
            decimal Rate = Convert.ToDecimal(_PU_RATE);

            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesAnalysisDetails_Report");
                proc.AddPara("@COMPANYID", strCompany);
                proc.AddPara("@FINYEAR", strFinYear);
                proc.AddPara("@FROMDATE", strFromDate);
                proc.AddPara("@TODATE", strToDate);
                proc.AddPara("@CLASS", strClassID);
                proc.AddPara("@CATEGORY", strCategoryID);
                proc.AddPara("@PRODUCT_ID", strProductID);
                proc.AddPara("@PU_RATE", Rate);
                proc.AddBooleanPara("@ISGST", Convert.ToBoolean(strRateCriteria));
                dt = proc.GetTable();

                Session["DetReportDataSource"] = dt;
                ShowGridDetails.DataBind();

                return dt;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}