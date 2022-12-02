using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class StockAdjustmentList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        StockAdjustmentBL blLayer = new StockAdjustmentBL();
        MasterSettings objmaster = new MasterSettings();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL cbl = new CommonBL();
        public bool isReconcileStockAdjustment { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    gridAdvanceAdj.Columns[9].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    gridAdvanceAdj.Columns[9].Visible = false;
                }
            }

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            string TechnicianStockAdjustment = objmaster.GetSettings("TechnicianStockAdjustment");
            if (!String.IsNullOrEmpty(TechnicianStockAdjustment))
            {
                if (TechnicianStockAdjustment == "1")
                {
                   // gridAdvanceAdj.Columns[4].Visible = true;
                    gridAdvanceAdj.Columns[4].Width = 200;

                }
                else if (TechnicianStockAdjustment == "0")
                {
                    gridAdvanceAdj.Columns[4].Width = 0;
                    //gridAdvanceAdj.Columns[4].Visible = false;

                }
            }


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=47");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=47");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "DATA is Freezed between   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + " for Edit. ";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                spnDeleteLock.InnerText = spnEditLock.InnerText + "DATA is Freezed between   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + "  for Delete.";
                spnEditLock.InnerText = "";
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/StockAdjustmentList.aspx");
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

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;


        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Stock_ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_StockAdjustmentLists
                            where d.Stock_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Stock_Date <= Convert.ToDateTime(strToDate)
                            orderby d.Stock_ID descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_StockAdjustmentLists
                            where d.Stock_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Stock_Date <= Convert.ToDateTime(strToDate) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Branch_ID))
                            orderby d.Stock_ID descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_StockAdjustmentLists
                        where d.Branch_ID == '0'
                        orderby d.Stock_ID descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void gridAdvanceAdj_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                int rowsNo = blLayer.DeleteAdj(param.Split('~')[1]);
                if (rowsNo > 0)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
                }
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }

        private void bindexport(int Filter)
        {
            //gridAdvanceAdj.Columns[7].Visible = false;
            string filename = "Stock Adjustment";
            exporter.FileName = filename;
            exporter.FileName = "Stock Adjustment";

            exporter.PageHeader.Left = "Stock Adjustment";
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

        protected void gridAdvanceAdj_SummaryDisplayText(object sender, DevExpress.Web.ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }



        [WebMethod]
        public static string GetReconValue(string id)
        {
            string AdjIdStatus = "2";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            Int64 AdjId = Convert.ToInt64(id);
            DataTable recondt = oDBEngine.GetDataTable("Select Isnull(ReconcileStockDetails_id,0) ReconValue from tbl_master_ReconcileStockDetails where Reconcile_StkAdjId=" + Convert.ToInt64(AdjId) + "");
            if (recondt.Rows.Count > 0 && recondt != null)
            {
                if (Convert.ToString(recondt.Rows[0]["ReconValue"]) != "0" && Convert.ToString(recondt.Rows[0]["ReconValue"]) != "")
                {
                    AdjIdStatus = "1";
                }
                else
                {
                    AdjIdStatus = "2";
                }

            }
            return AdjIdStatus;
        }


    }
}