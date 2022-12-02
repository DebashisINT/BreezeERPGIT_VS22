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
using ERP.OMS.Management.Master;
using DataAccessLayer;
using System.Threading.Tasks;

using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;

namespace ERP.OMS.Reports.Master
{

   public partial class Stock_Ageing : System.Web.UI.Page
    {
        ReportData rpt = new ReportData();
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string strClassList = "", strBrandList = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/GstrReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["dt_PartyLedgerRpt"] = null;
                Session["dt_GSTRRpt"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
          


                lookupClass.DataSource = GetClassList();
                lookupClass.DataBind();

                lookupBrand.DataSource = GetBrandList();
                lookupBrand.DataBind();
           
               // ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

            
                //ASPxToDate.Value = DateTime.Now;
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

            
                ASPxToDate.Value = DateTime.Now;

                BranchpopulateGSTN();
          

            }
            else
            {

            }



        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
               
                ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {

                    bindexport(Filter);
              
            }

        }
   
        public void bindexport(int Filter)
        {
            string filename = "Stock Aging";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Stock Aging" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 50;
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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt");
            DateTime dtFrom;
            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string TODATE = dtTo.ToString("yyyy-MM-dd");
            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }


                Task PopulateStockTrialDataTask = new Task(() => GetStockAging(TODATE, WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously(); 



            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dt_PartyLedgerRpt"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

            }

        }

        public void GetStockAging(string Todate,string Gstn)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;


                dtTo = Convert.ToDateTime(ASPxToDate.Date);

              
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                foreach (object Class in ClassList)
                {
                    strClassList += "," + Class;
                }
                strClassList = strClassList.TrimStart(',');

                List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                foreach (object Brand in BrandList)
                {
                    strBrandList += "," + Brand;
                }
                strBrandList = strBrandList.TrimStart(',');


                dttab = GetStockagingbind(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), Gstn, TODATE, strClassList, strBrandList);

                if (dttab.Rows.Count > 0)
                {
                    Session["dt_PartyLedgerRpt"] = dttab;
                    ShowGrid.DataSource = dttab;
                    ShowGrid.DataBind();
                }
                else
                {
                    Session["dt_PartyLedgerRpt"] = null;
                    ShowGrid.DataSource = null;
                    ShowGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
            }


        }

        public DataTable GetStockagingbind(string Company, string Finyear, string Gstn, string TODATE, string ClassList, string strBrandList)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_StockAgeing_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@ASONDATE", TODATE);
            proc.AddPara("@CLASS", strClassList);
            proc.AddPara("@CATEGORY", strBrandList);
            ds = proc.GetTable();
            return ds;
        }

        #region ########  Branch GRN Populate  #######
        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);



            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            cmd.Dispose();
            con.Dispose();


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgstn.DataSource = ds.Tables[0];
                ddlgstn.DataTextField = "branch_GSTIN";
                ddlgstn.DataValueField = "branch_GSTIN";
                ddlgstn.DataBind();
                ddlgstn.Items.Insert(0, "");
            }


        }

        #endregion

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void lookupClass_DataBinding(object sender, EventArgs e)
        {
            lookupClass.DataSource = GetClassList();
        }

        protected void lookupBrand_DataBinding(object sender, EventArgs e)
        {
            lookupBrand.DataSource = GetBrandList();
        }

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
      

    }
}