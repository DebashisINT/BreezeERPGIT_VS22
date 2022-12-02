using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ApprovedSalesOrderNew : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ApproveSaleasOrder.aspx");

            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ApproveSettingsSalesOrder = cSOrder.GetSystemSettingsResult("ApproveSettingsSalesOrder");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    //GridApprovedSalesOrder.Columns[4].Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    //GridApprovedSalesOrder.Columns[4].Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(ApproveSettingsSalesOrder))
            {
                //if (ApproveSettingsSalesOrder == "Yes")
                //{
                //    GridApprovedSalesOrder.Columns[16].Visible = true;
                //    GridApprovedSalesOrder.Columns[17].Visible = true;
                //    GridApprovedSalesOrder.Columns[18].Visible = true;
                //   // isApprove = true;
                //}
                //else if (ApproveSettingsSalesOrder.ToUpper().Trim() == "NO")
                //{
                //    GridApprovedSalesOrder.Columns[16].Visible = false;
                //    GridApprovedSalesOrder.Columns[17].Visible = false;
                //    GridApprovedSalesOrder.Columns[18].Visible = false;
                //   // isApprove = false;

                //}
            }

            string PrintButton = cSOrder.GetSystemSettingsResult("PrintbuttonSOlistingForInvoiceDetails");
            if (!String.IsNullOrEmpty(PrintButton))
            {
                if (PrintButton == "Yes")
                {
                    hddnPrintButton.Value = "1";
                }
                else if (PrintButton.ToUpper().Trim() == "NO")
                {
                    hddnPrintButton.Value = "0";
                }
            }

            if (!IsPostBack)
            {
                Session["SO_ProductDetails"] = null;
                Session["Entry_Type"] = null;
                Session["SO_ProductDetails"] = null;
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
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //if (objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["userid"])))
                //{
                //    hdnIsUserwiseFilter.Value = "1";
                //}
                //else
                //{
                    hdnIsUserwiseFilter.Value = "0";
                //}

                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
            }
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

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
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
            //GrdOrder.Columns[5].Visible = false;
            string filename = "Sales Order";
            exporter.FileName = filename;
            exporter.FileName = "SalesOrder";

            exporter.PageHeader.Left = "Sales Order";
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

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SlNo";

            //   string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string FilterType = Convert.ToString(hFilterType.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (FilterType == "Printed")
                {
                    #region Printed
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                    && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Printed (In Process)"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                     && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Printed (In Process)"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where
                                    d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                    branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Printed (In Process)"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where
                                     d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                     branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Printed (In Process)"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }
                    hFilterType.Value = "";

                    #endregion
                }
                else if (FilterType == "ReadyforInvoice")
                {
                    #region ReadyforInvoice
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                    && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Ready for Invoice"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                     && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Ready for Invoice"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where
                                    d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                    branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Ready for Invoice"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where
                                     d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                     branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Ready for Invoice"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }

                    hFilterType.Value = "";
                    #endregion
                }
                else if (FilterType == "Pending")
                {
                    #region Pending
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                    && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Pending"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                     && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Pending"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where
                                    d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                    branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    && d.ReadyforInvoice == "Pending"
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where
                                     d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                     branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     && d.ReadyforInvoice == "Pending"
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }

                    hFilterType.Value = "";
                    #endregion
                }
                else
                {
                    #region All
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                    && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                                     && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }

                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                        {
                            var q = from d in dc.V_GetApproveSalesOrderEntityLists
                                    where
                                    d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                    branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                    orderby d.Order_CheckDate descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            var q1 = from d in dc.V_GetApproveSalesOrderEntityLists
                                     where
                                     d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                                     branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                     orderby d.Order_CheckDate descending
                                     select d;
                            e.QueryableSource = q1;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_GetApproveSalesOrderEntityLists
                        where d.Order_BranchId == -1
                        orderby d.Order_CheckDate descending
                        select d;
                e.QueryableSource = q;
            }
        }
    }
}