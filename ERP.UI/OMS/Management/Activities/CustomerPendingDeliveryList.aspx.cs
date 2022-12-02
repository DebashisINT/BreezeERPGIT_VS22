using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerPendingDeliveryList : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/CustomerPendingDeliveryList.aspx");
            if (!IsPostBack)
            {

                String finyear = "";
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);


                Session["CUSTexportval"] = null;
                Session["CustomerDeliverypending"] = null;
                //GrdOrder.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerPendingDelivery";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerPendingDelivery');</script>");
                
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;



            }

            //FillGrid();
        }
        public void bindexport(int Filter)
        {
            //GrdOrder.Columns[5].Visible = false;
            string filename = "Customer Pending Delivery List";
            exporter.FileName = filename;
            exporter.FileName = "CustomerDelivery";

            exporter.PageHeader.Left = "Customer Pending Delivery List";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            drdExport.SelectedValue = "0";
            Session["CUSTexportval"] = null;
            if (Filter != 0)
            {
                if (Session["CUSTexportval"] == null)
                {
                    Session["CUSTexportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["CUSTexportval"]) != Filter)
                {
                    Session["CUSTexportval"] = Filter;
                    bindexport(Filter);
                }

               
            }
        }

        protected void GrdOrder_DataBinding(object sender, EventArgs e)
        {
            DataTable dtdata = new DataTable();
            string branchID = Convert.ToString(cmbBranchfilter.Value) == null ? "0" : Convert.ToString(cmbBranchfilter.Value);
            string fromDate = FormDate.Date.ToString("yyyy-MM-dd");
            string ToDate = toDate.Date.ToString("yyyy-MM-dd");
            if (HttpContext.Current.Session["LastCompany"] != null)
            {

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                {
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    //dtdata = objCRMSalesOrderDtlBL.GetStockOutListGridData(userbranch, lastCompany);
                    if (Session["CustomerDeliverypending"] != null)
                    {
                        dtdata = (DataTable)Session["CustomerDeliverypending"];
                    }

                }
            }


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GrdOrder.DataSource = dtdata;
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "slno";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //string IsFilter = Convert.ToString(hfIsFilter.Value);
            //string strFromDate = Convert.ToString(hfFromDate.Value);
            //string strToDate = Convert.ToString(hfToDate.Value);
            //string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            //List<int> branchidlist;
            //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //if (IsFilter == "Y")
            //{
            //    if (strBranchID == "0")
            //    {
            //        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            //        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            //        var q = from d in dc.v_CustomerPendingDeliveryLists
            //                    where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
            //                      d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
            //                     branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
            //                orderby d.Invoice_DateTimeFormat descending
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //    else
            //    {
            //        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
            //        var q = from d in dc.v_CustomerPendingDeliveryLists
            //                       where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
            //                      d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
            //                      branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
            //                orderby d.Invoice_DateTimeFormat descending
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //}
            //else
            //{
            //    var q = from d in dc.v_CustomerPendingDeliveryLists
            //            where d.Invoice_BranchId == '0'
            //            orderby d.Invoice_DateTimeFormat descending
            //            select d;
            //    e.QueryableSource = q;
            //}
        }
        
        public void FillGrid()
        {
            DataTable dtdata = new DataTable();
            
            string branchID = Convert.ToString(cmbBranchfilter.Value) == null ? "0" : Convert.ToString(cmbBranchfilter.Value);
            string fromDate = FormDate.Date.ToString("yyyy-MM-dd");
            string ToDate = toDate.Date.ToString("yyyy-MM-dd");

            branchID = (branchID == "0") ? Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) : branchID;
            dtdata = GetSalesInvoiceOnPendingDeliveryList(branchID, fromDate, ToDate);
             
            GrdOrder.DataSource = dtdata;
           
            GrdOrder.DataBind();
            
        }
          
        
        public DataTable GetSalesInvoiceOnPendingDeliveryList(string branchID, string frmDate = "", string toDate = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "CustomerPendingDeliveryList");
            proc.AddVarcharPara("@loggedInBranch", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@DelivBranchID", 500, branchID);
            proc.AddVarcharPara("@FromDelivDate", 10, frmDate.Trim());
            proc.AddVarcharPara("@ToDelivDate", 10, toDate.Trim());

            dt = proc.GetTable();
            return dt;
        }
          
        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            if (name.Length > 0)
            {
                branchName.Text = Convert.ToString(name[0]["branch_description"]);
            }
        }
        protected void GrdOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            
            if (WhichCall == "FilterGridByDate")
            {
                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];

                string branchID = (branch == "0") ? Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) : branch;

                DataTable dtdata = new DataTable();

                dtdata = GetSalesInvoiceOnPendingDeliveryList(branchID, fromdate, toDate);
                if (dtdata != null)
                {
                    GrdOrder.DataSource = dtdata;
                    Session["CustomerDeliverypending"] = dtdata;
                    GrdOrder.DataBind();
                }
            }
             
        }
    }
}