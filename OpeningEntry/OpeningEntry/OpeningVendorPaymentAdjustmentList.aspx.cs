using BusinessLogicLayer;
using EntityLayer.CommonELS;
using OpeningEntry.OpeningEntry.DBML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.OpeningEntry
{
    public partial class OpeningVendorPaymentAdjustmentList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        VendorPaymentAdjustmentBL VendorPaymentAdjustment = new VendorPaymentAdjustmentBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/VendorPaymentAdjustmentList.aspx");

            if (!IsPostBack)
            {
                LoadDataonPageLoad();
            }
        }

        public void LoadDataonPageLoad()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            //FormDate.Date = DateTime.Now;
            //toDate.Date = DateTime.Now;
            DateTime fromDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]);
            fromDate = fromDate.AddDays(-1);


            toDate.Date = fromDate;
            FormDate.Date = fromDate;
            toDate.MaxDate = fromDate;
            FormDate.MaxDate = fromDate;


        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Adj_id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_VendorAdvanceAdjustments
                            where d.Adjustment_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Adjustment_Date <= Convert.ToDateTime(strToDate)
                            orderby d.Adjustment_Date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_VendorAdvanceAdjustments
                            where d.Adjustment_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Adjustment_Date <= Convert.ToDateTime(strToDate) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Adjustment_Date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_VendorAdvanceAdjustments
                        where d.Branch == '0'
                        orderby d.Adjustment_Date descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void gridAdvanceAdj_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                int rowsNo = VendorPaymentAdjustment.DeleteAdj(param.Split('~')[1]);
                if (rowsNo > 0)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
                }
            }
        }


        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    if (Filter != 0)
        //    {
        //        bindexport(Filter);
        //    }
        //}

        private void bindexport(int Filter)
        {
            gridAdvanceAdj.Columns[7].Visible = false;
            string filename = "Adjustment of Document- Advances";
            exporter.FileName = filename;
            exporter.FileName = "Adjustment of Document- Advances";

            exporter.PageHeader.Left = "Adjustment of Document- Advances";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;


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
    }
}
