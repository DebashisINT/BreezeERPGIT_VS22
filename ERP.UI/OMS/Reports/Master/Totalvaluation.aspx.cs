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
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
namespace ERP.OMS.Reports.Master
{

    public partial class Totalvaluation : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        TotalvaluationClass objvaluation = new TotalvaluationClass();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/Totalvaluation.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["SI_ComponentData"] = null;
                Session["dt_TotalvaluationRpt"] = null;
                Session["dt_PartyLedgerRpt"] = null;
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
                BranchHoOffice();
                lookupClass.DataSource = GetClassList();
                lookupClass.DataBind();

                lookupBrand.DataSource = GetBrandList();
                lookupBrand.DataBind();


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
            stbill = bll1.GetBranchheadoffice("HO");
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
            }
        }

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

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Stock Valuation Summary Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
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
            string filename = "StockValuationremaining";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Stock Valuation Details Report";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "grivaluation";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }
        #endregion




        #region =======================Valuation Summary =========================
        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_PartyLedgerRpt");


            ShowGrid.JSProperties["cpSave"] = null;


            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


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



            string CUSTVENDID = "";
            string QuoComponent = "";
            List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in QuoList)
            {
                QuoComponent += "," + Quo;
            }
            CUSTVENDID = QuoComponent.TrimStart(',');




            ///  GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);



            Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID));
            PopulateStockTrialDataTask.RunSynchronously();

        }



        public void GetSalesRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string ProductIds)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                string strClassList = "", strBrandList = "";
                DataTable ds = new DataTable();


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



                ds = objvaluation.GetvaluationSummary(BRANCH_ID, ProductIds, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), FROMDATE, TODATE, "F", "Summary", strClassList, strBrandList);
                if (ds.Rows.Count > 0)
                {
                    Session["dt_PartyLedgerRpt"] = ds;
                    ShowGrid.DataSource = ds;
                    ShowGrid.DataBind();
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void gridvaluationsummary_DataBinding(object sender, EventArgs e)
        {

            if (Session["dt_PartyLedgerRpt"] != null)
            {

                ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion



        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }


        #region  ===========Product  Bind====================
        protected void ComponentProduct_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];


                string type = e.Parameter.Split('~')[1];

                DataTable ComponentTable = new DataTable();

                //              ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name',sProducts_Hsncode as Hsn FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
                //"AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");

                if (e.Parameter.Split('~')[1] == "0" || e.Parameter.Split('~')[1] == "")
                {
                    ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' ,sProducts_Hsncode as Hsn FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
          "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");
                }

                else
                {

                    ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name',sProducts_Hsncode as Hsn  FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
                             "AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) and ProductClass_Code in(" + e.Parameter.Split('~')[1] + ") ORDER BY sProducts_Description");

                }


                if (ComponentTable.Rows.Count > 0)
                {

                    Session["SI_ComponentData"] = ComponentTable;
                    lookup_quotation.DataSource = ComponentTable;
                    lookup_quotation.DataBind();

                }
                else
                {
                    Session["SI_ComponentData"] = null;
                    lookup_quotation.DataSource = null;
                    lookup_quotation.DataBind();

                }
            }
        }

        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            //   DataTable ComponentTable = new DataTable();

            if (Session["SI_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
            }
        }

        #endregion


        #region =====================Valuation Details===========================
        protected void gridPendingApproval_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string returnPara = Convert.ToString(e.Parameters);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branch = returnPara.Split('~')[1];
                string prodId = returnPara.Split('~')[2];
                Getvaluation(prodId, branch);
            }

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {

            if (Session["dt_TotalvaluationRpt"] != null)
            {

                grivaluation.DataSource = (DataTable)Session["dt_TotalvaluationRpt"];

            }
        }

        public void Getvaluation(string ProductIds, string BRANCH_ID)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataTable ds = new DataTable();

                string strClassList = "", strBrandList = "";


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

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");


                ds = objvaluation.Getvaluationdetails(BRANCH_ID, ProductIds, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), FROMDATE, TODATE, strClassList, strBrandList);
                if (ds.Rows.Count > 0)
                {

                    Session["dt_TotalvaluationRpt"] = ds;

                    grivaluation.DataSource = ds;
                    grivaluation.DataBind();
                }

                else
                {

                    Session["dt_TotalvaluationRpt"] = null;

                    grivaluation.DataSource = null;
                    grivaluation.DataBind();
                }
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

                ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and  branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

                if (ComponentTable.Rows.Count > 0)
                {

                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();


                }
                else
                {
                    Session["SI_ComponentData_Branch"] = null;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();

                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

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
        protected void lookupClass_DataBinding(object sender, EventArgs e)
        {
            lookupClass.DataSource = GetClassList();
        }

        protected void lookupBrand_DataBinding(object sender, EventArgs e)
        {
            lookupBrand.DataSource = GetBrandList();
        }
    }
}