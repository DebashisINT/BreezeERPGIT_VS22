using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Text;
using System.Reflection;
using DevExpress.Web;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Activities
{
    public partial class TodayTask : System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        DateTime dtFrom;
        DateTime dtTo;
        protected void Page_Init(object sender, EventArgs e)
        {
            SalesDetailsGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

               
              

                Session["exportval"] = null;
                SalesDetailsGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesFuturePDetailsGrid";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesFuturePDetailsGrid');</script>");

             
            }
            else
            {
               
            }
              string id = Request.QueryString["Id"];
            if(id=="3")
            {
                lblActivity.Text = "Today's Task - Future Sales";
            
            }
            else if (id == "5")
            {
                lblActivity.Text = "Today's Task - Clarification Required";

            }
            else  if (id == "2")
            {
                lblActivity.Text = "Today's Task - Closed Sales";

            }
            else if (id == "4")
            {
                lblActivity.Text = "Today's Task - Open Activities";

            }
            
            Session["export"] = null;
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/crm_sales.aspx");
            BindGrid();


            string previousPageUrl = string.Empty;
            if (Request.UrlReferrer != null)
                previousPageUrl = Request.UrlReferrer.AbsoluteUri;

            if (Request.QueryString["frmdate"] != null && Request.QueryString["todate"] != null)
            {
                if (!previousPageUrl.Contains("?"))
                {
                    previousPageUrl = previousPageUrl + "?frmdate=" + Request.QueryString["frmdate"] + "&todate=" + Request.QueryString["todate"];

                }

            }
            else
                previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

            ViewState["previousPageUrl"] = previousPageUrl;
            goBackCrossBtn.NavigateUrl = previousPageUrl;
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        //protected void futureSalesGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    BindGrid();
        //}
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        public string EditPermissionShow(string cntid)
        {


            string EditPermissionval = "0";
            try
            {
                BusinessLogicLayer.Others objOtherBL = new BusinessLogicLayer.Others();

                DataTable PermissionDt = new DataTable();
                PermissionDt = objOtherBL.GetSalesManDeactivateDocomentActivity(cntid);
                if (PermissionDt != null && PermissionDt.Rows.Count > 0)
                {
                    EditPermissionval = Convert.ToString(PermissionDt.Rows[0]["EditPermission"]);

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                //return;
            }


            return EditPermissionval;
        }
       


      
        void BindGrid()
        {

            string id = Request.QueryString["Id"];
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID
            //int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            //FutureSalesDataSource.SelectCommand = "Select tbl_trans_Sales.sls_sales_status AS Status,(SELECT cnt_firstName+' '+isnull(cnt_middleName,'')+ ' ' +isnull(cnt_lastName,'') FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id) AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], '  + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'IPO' THEN 'frmSalesIPO1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance' THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id,tbl_trans_Sales.sls_estimated_value AS Amount, CASE isnull(sls_product, '') WHEN '' THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId,case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales ,tbl_trans_Activies  Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND tbl_trans_Activies.act_assignedTo ='" + UserId.ToString() + "' AND sls_sales_status in (3) Order by convert(datetime,sls_nextvisitdate,101)";
            SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsGridDataSource.SelectParameters.Clear();
            SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetTodaySalesActivityDetails");
            SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesDetailsGridDataSource.SelectParameters.Add("ActivityTypeId", id);
           
            SalesDetailsGrid.DataBind();
        }

      
        //protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        //{

        //}

        protected void drdSalesActivityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["export"] = "1";
            Int32 Filter = int.Parse(Convert.ToString(drdSalesActivityDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
            }
        }
        public void bindSalesActivityDetailsexport(int Filter)
        {
            string lblName = String.Empty;
            string id = Request.QueryString["Id"];
            if (id == "3")
            {
                lblName = "Today's Task - Future Sales";

            }
            else if (id == "5")
            {
                lblName = "Today's Task - Clarification Required";

            }
            else if (id == "2")
            {
                lblName = "Today's Task - Closed Sales";

            }
            else if (id == "4")
            {
                lblName = "Today's Task - Open Activities";

            }


            exporter.GridViewID = "SalesDetailsGrid";
            exporter.FileName = lblName;

            exporter.PageHeader.Left = lblName;
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
           SalesDetailsGrid.Columns[11].Visible = false;
           SalesDetailsGrid.Columns[12].Visible = false;
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

        protected void AspxProductGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductByActivity(Convert.ToString(e.Parameters));


            AspxProductGrid.DataSource = DtProduct;
            AspxProductGrid.DataBind();
        }
        protected void AspxProductclassGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductCellsByActivity(Convert.ToString(e.Parameters));


            ASPxGridProductClass.DataSource = DtProduct;
            ASPxGridProductClass.DataBind();
        }
      
    }
}