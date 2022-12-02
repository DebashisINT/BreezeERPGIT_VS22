using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using ERP.Models;
using System.Linq;

namespace ERP.OMS.Management.Activities
{
    public partial class OldUnit_SalesInvoiceList : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        public EntityLayer.CommonELS.UserRightsForPage ChallanRights = new UserRightsForPage();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();

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

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/OldUnit_SalesInvoiceList.aspx");
            ChallanRights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesChallan.aspx");

            if (!IsPostBack)
            {
                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);
                Session["SecondHandChallanGrid"] = null;

                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

                AssignedBranch.DataSource = posSale.getBranchListByBranchList(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(Session["userbranchID"]));
                AssignedBranch.ValueField = "branch_id";
                AssignedBranch.TextField = "branch_description";
                AssignedBranch.DataBind();
                AssignedBranch.Value = "0";

                if (ChallanRights.CanAdd == false) 
                {
                    ASPxPageControl1.TabPages[1].Visible = false;
                }

            }
        }

        #region Grid Section Start
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string finyear = Convert.ToString(Session["LastFinYear"]);

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    
                    var q = from d in dc.V_SalesInvoiceOldUnitLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_FinYear == finyear &&
                                  d.Invoice_CompanyID == lastCompany
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_SalesInvoiceOldUnitLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_FinYear == finyear &&
                                  d.Invoice_CompanyID == lastCompany &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                
                var q = from d in dc.V_SalesInvoiceOldUnitLists
                        where d.Invoice_FinYear == finyear &&
                              d.Invoice_CompanyID == lastCompany &&
                              d.Invoice_BranchId == '0'
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void GrdQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            GrdQuotation.JSProperties["cpinsert"] = null;
            GrdQuotation.JSProperties["cpEdit"] = null;
            GrdQuotation.JSProperties["cpUpdate"] = null;
            GrdQuotation.JSProperties["cpDelete"] = null;
            GrdQuotation.JSProperties["cpExists"] = null;
            GrdQuotation.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Delete")
            {
                deletecnt = objSalesInvoiceBL.DeleteInvoice(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted Successfully?";
                    //GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
                }
                else if (deletecnt == -99)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module can not delete.";
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Problem in Deleting. Sorry for Inconvenience";
                }
            }
            else if (WhichCall == "FilterGridByDate")
            {
                //DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                //DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                //string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //string finyear = Convert.ToString(Session["LastFinYear"]);

                //DataTable dtdata = new DataTable();
                //dtdata = objSalesInvoiceBL.GetQuotationList_GridData_OldUnit(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                //if (dtdata != null && dtdata.Rows.Count > 0)
                //{
                //    GrdQuotation.DataSource = dtdata;
                //    GrdQuotation.DataBind();
                //}
                //else
                //{
                //    GrdQuotation.DataSource = null;
                //    GrdQuotation.DataBind();
                //}
            }
        }
        
        protected void GrdQuotation_DataBinding(object sender, EventArgs e)
        {
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            string finyear = Convert.ToString(Session["LastFinYear"]);
            string BranchID = Convert.ToString(cmbBranchfilter.Value);
            DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            DataTable dtdata = new DataTable();
            dtdata = objSalesInvoiceBL.GetQuotationList_GridData_OldUnit(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GrdQuotation.DataSource = dtdata;
            }
            else
            {
                GrdQuotation.DataSource = null;
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

        #endregion

        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
            GrdQuotation.Columns[6].Visible = false;
            string filename = "Sales Invoice";

            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                exporter.GridViewID = "GrdQuotation";
                exporter.FileName = "Salesinvoice";
                exporter.PageHeader.Left = "Sales Invoice";
            }
            else
            {
                filename = "Sales Challan";
                exporter.GridViewID = "GridChallan";
                exporter.FileName = "SalesChallan";
                exporter.PageHeader.Left = "Sales Challan";
            }

            exporter.FileName = filename;
           
          
          
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

        #endregion


        #region Auto challan block
        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }


        [WebMethod]
        public static string GetCustomerId(string KeyVal)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            string ispermission = string.Empty;
            ispermission = objCRMSalesOrderDtlBL.GetInvoiceCustomerId(Convert.ToInt32(KeyVal));


            return Convert.ToString(ispermission);

        }

        #endregion

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SecondHandSales\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SecondHandSales\DocDesign\Normal";
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
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
                //SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        protected void BranchRequUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string Parameter = e.Parameter;

            if (Parameter.Split('~')[0] == "AssignBranch")
            {   
                string InvoiceId = e.Parameter.Split('~')[1];
                string AssignBranch = Convert.ToString(AssignedBranch.Value);
                posSale.AssignOldunitBranch(InvoiceId, AssignBranch);
                BranchRequUpdatePanel.JSProperties["cpAssignUpdated"] = "y";
            }
            else if (Parameter.Split('~')[0] == "Cancel_Assignment")
            {
                string InvoiceId = e.Parameter.Split('~')[1];
                posSale.CancelBranchAssignment(Convert.ToInt32(InvoiceId));
                BranchRequUpdatePanel.JSProperties["cpAssignUpdated"] = "CancelAssign";
            }

        }

        protected void cGridChallan_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters;
            if (param.Split('~')[0] == "bindchallan")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataTable dtdata = new DataTable();
                dtdata = objSalesInvoiceBL.GetChallanGridData_OldUnit(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
                Session["SecondHandChallanGrid"] = dtdata;
                GridChallan.DataBind();
            }
        }
        protected void cGridChallan_DataBinding(object sender, EventArgs e)
        {
            if (Session["SecondHandChallanGrid"] != null)
            {
                DataTable ChallanTable = (DataTable)Session["SecondHandChallanGrid"];
                GridChallan.DataSource = ChallanTable;
            }
        }

        

    }
}