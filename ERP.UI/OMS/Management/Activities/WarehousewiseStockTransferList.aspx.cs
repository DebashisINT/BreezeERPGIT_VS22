//==================================================== Revision History =================================================================================================
// 1.0  Priti V2.0.38    05-06-2023  0026257: Excess Qty for an Item to be Stock Transferred automatically to a specific Warehouse while making Issue for Production
//====================================================End Revision History===============================================================================================
using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
using System.Web.Services;
//Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill
namespace ERP.OMS.Management.Activities
{
    public partial class WarehousewiseStockTransferList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        WarehouseStockJournalBL blLayer = new WarehouseStockJournalBL();
        MasterSettings objmaster = new MasterSettings();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/WarehousewiseStockTransferList.aspx");

            //Rev 1.0
            string IsConsiderExcessQty = objmaster.GetSettings("IsConsiderExcessQtyIssueforProduction");
            if (!String.IsNullOrEmpty(IsConsiderExcessQty))
            {
                if (IsConsiderExcessQty == "1")
                {                    
                    gridAdvanceAdj.Columns[15].Width = 200;
                }
                else if (IsConsiderExcessQty == "0")
                {                    
                    gridAdvanceAdj.Columns[15].Width = 0;
                }
            }
            //Rev 1.0 End
            string TechnicianStockAdjustment = objmaster.GetSettings("TechnicianStockTransfer");
            if (!String.IsNullOrEmpty(TechnicianStockAdjustment))
            {
                if (TechnicianStockAdjustment == "1")
                {
                   // gridAdvanceAdj.Columns[4].Visible = true;
                    gridAdvanceAdj.Columns[4].Width = 200;

                }
                else if (TechnicianStockAdjustment == "0")
                {
                    //gridAdvanceAdj.Columns[4].Visible = false;
                    gridAdvanceAdj.Columns[4].Width = 0;
                }
            }
            string EmployeeStockTransfer = objmaster.GetSettings("EmployeeStockTransfer");
            if (!String.IsNullOrEmpty(EmployeeStockTransfer))
            {
                if (EmployeeStockTransfer == "1")
                {
                    // gridAdvanceAdj.Columns[4].Visible = true;
                    gridAdvanceAdj.Columns[8].Width = 200;

                }
                else if (EmployeeStockTransfer == "0")
                {
                    //gridAdvanceAdj.Columns[4].Visible = false;
                    gridAdvanceAdj.Columns[8].Width = 0;
                }
            }

            string EntityRequiredWarehouseStockTransfer = objmaster.GetSettings("EntityRequiredWarehouseStockTransfer");
            if (!String.IsNullOrEmpty(EntityRequiredWarehouseStockTransfer))
            {
                if (EntityRequiredWarehouseStockTransfer == "1")
                {
                    // gridAdvanceAdj.Columns[4].Visible = true;
                    gridAdvanceAdj.Columns[10].Width = 200;
                    gridAdvanceAdj.Columns[9].Width = 200;

                }
                else if (EntityRequiredWarehouseStockTransfer == "0")
                {
                    //gridAdvanceAdj.Columns[4].Visible = false;
                    gridAdvanceAdj.Columns[10].Width = 0;
                    gridAdvanceAdj.Columns[9].Width = 0;
                }
            }
            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {

                    gridAdvanceAdj.Columns[5].Width = 160;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    gridAdvanceAdj.Columns[5].Width = 0;
                }
            }


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=46");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(10),Lock_Fromdate,105) LockCon_Fromdate,convert(varchar(10),Lock_Todate,105) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=46");
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
            e.KeyExpression = "StockTransfer_ID";

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
                    var q = from d in dc.v_WarehouseWiseStocktransferLists
                            where d.Stock_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Stock_Date <= Convert.ToDateTime(strToDate)
                            orderby d.StockTransfer_ID descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_WarehouseWiseStocktransferLists
                            where d.Stock_Date >= Convert.ToDateTime(strFromDate) &&
                                  d.Stock_Date <= Convert.ToDateTime(strToDate) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Branch_ID))
                            orderby d.StockTransfer_ID descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_WarehouseWiseStocktransferLists
                        where d.Branch_ID == '0'
                        orderby d.StockTransfer_ID descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void gridAdvanceAdj_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // Rev Sayantani
            WarehousewiseStockTransfer blLayer = new WarehousewiseStockTransfer();
            // End of Rev Sayantani
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                int rowsNo = blLayer.DeleteAdj(param.Split('~')[1], hdnWDelete.Value);
                if (rowsNo ==1 )
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
                }
                else if(rowsNo ==-3)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Product is going negative can not delete.";
                }
                else if (rowsNo == -2)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = -2;
                }
                //Rev 1.0
                else if (rowsNo == -10)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Used in other module.can not delete.";
                }
                //Rev 1.0 End
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
            string filename = "Warehouse Wise Stock Transfer";
            exporter.FileName = filename;
            exporter.FileName = "Warehouse Wise Stock Transfer";

            exporter.PageHeader.Left = "Warehouse Wise Stock Transfer";
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

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\WarehoseStockTranfer\DocDesign\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\WarehoseStockTranfer\DocDesign\";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }
        //Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
        [WebMethod]
        public static string UpdateEWayBill(string Stk_Id, string UpdateEWayBill, string EWayBillDate)
        {
            WarehousewiseStockTransfer objlist = new WarehousewiseStockTransfer();
            int EWayBill = 0;
            EWayBill = objlist.UpdateEWayBillFor(Stk_Id, UpdateEWayBill, EWayBillDate);
            return Convert.ToString(EWayBill);
        }
        //Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill

    }
}