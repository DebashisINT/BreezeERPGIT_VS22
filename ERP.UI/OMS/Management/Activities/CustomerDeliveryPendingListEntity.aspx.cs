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
    public partial class CustomerDeliveryPendingListEntity : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();


       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        string IsFirstTimeLoad = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFirstTimeLoad = "N";
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerDeliveryPendingListEntity.aspx");
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

                Session["exportval"] = null;
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                IsFirstTimeLoad = "Y";

                //GrdOrder.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrdOrder";

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrdOrder');</script>");
            }

            FillGridOnLoadDlvType();



            if (Request.QueryString["type"] != null)
            {
                if (Convert.ToString(Request.QueryString["type"]) == "SD")
                {
                    lblHeadertext.Text = "Customer Delivery - SD";
                }

            }
            else
            {
                lblHeadertext.Text = "Customer Delivery - OD";
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

        public void FillGridOnLoadDlvType()
        {
            string DlvType = string.Empty;
            DlvType = Convert.ToString(Request.QueryString["type"]);
            if (!string.IsNullOrEmpty(DlvType))
            {
                hddnTypeIdd.Value = "1";
            }
            else
            {
                hddnTypeIdd.Value = "0";
            }

            DataTable dt = GetConfigSettingForBRS();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["Variable_Value"]).ToUpper() == "YES")
                {
                    hddnBRSConfigSettings.Value = "1";
                }
                else
                {
                    hddnBRSConfigSettings.Value = "0";
                }


            }
        }
        public void FillGridOnLoad()
        {
            string DlvType = string.Empty;
            DlvType = Convert.ToString(Request.QueryString["type"]);
            if (Request.QueryString.AllKeys.Contains("OurD") || Request.QueryString.AllKeys.Contains("SelfD"))
            {
                FillGrid();
            }
            else
            {

                if (!string.IsNullOrEmpty(DlvType))
                {
                    hddnTypeIdd.Value = "1";
                    if (Session["CustomerDeliveryPendingSelfdt"] != null)
                    {
                        DataTable dt = (DataTable)Session["CustomerDeliveryPendingSelfdt"];
                        GrdOrder.DataSource = dt;
                        GrdOrder.DataBind();
                    }
                    else
                    {
                        GrdOrder.DataSource = null;
                        GrdOrder.DataBind();
                    }
                }
                else
                {
                    hddnTypeIdd.Value = "0";
                    if (Session["CustomerDeliveryPendingOurdt"] != null)
                    {
                        DataTable dt = (DataTable)Session["CustomerDeliveryPendingOurdt"];
                        GrdOrder.DataSource = dt;
                        GrdOrder.DataBind();
                    }
                    else
                    {
                        GrdOrder.DataSource = null;
                        GrdOrder.DataBind();
                    }
                }
            }

        }


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = new DataTable();

            if (HttpContext.Current.Session["LastCompany"] != null)
            {

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string DlvType = string.Empty;
                DlvType = Convert.ToString(Request.QueryString["type"]);

                string BranchID = Convert.ToString(cmbBranchfilter.Value);
                DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
                DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

                if (!string.IsNullOrEmpty(DlvType) && IsFirstTimeLoad == "N")
                {
                    hddnTypeIdd.Value = "1";
                    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                    {
                        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        //dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPending(userbranch, lastCompany, "CustomerDeliveryPendingSelf");
                        dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPendingDatewise(userbranch, lastCompany, "CustomerDeliveryPendingSelfByDate",
                                                           BranchID, FromDate, ToDate);
                        //Session["CustomerDeliveryPendingSelfdt"] = dtdata;

                    }
                }
                else if (IsFirstTimeLoad == "N")
                {
                    hddnTypeIdd.Value = "0";
                    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                    {
                        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        //dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPending(userbranch, lastCompany, "CustomerDeliveryPending");
                        dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPendingDatewise(userbranch, lastCompany, "CustomerDeliveryPendingByDateOur",
                                                            BranchID, FromDate, ToDate);
                        //Session["CustomerDeliveryPendingOurdt"] = dtdata;
                    }
                }

            }

            if (dtdata != null && dtdata.Rows.Count > 0)
            {

                GrdOrder.DataSource = dtdata;
                GrdOrder.DataBind();
            }
            else
            {
                GrdOrder.DataSource = null;
                GrdOrder.DataBind();
            }
        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string available = Convert.ToString(e.GetValue("BRS"));
            if (available.ToUpper() == "NOT-DONE")
                e.Row.ForeColor = System.Drawing.Color.Gray;
            else if (available.ToUpper() == "DONE")
                e.Row.ForeColor = System.Drawing.Color.Blue;
            else
                e.Row.ForeColor = System.Drawing.Color.Black;
            //else if (available > 0)
            //    e.Row.ForeColor = Color.Blue;
            //else
            //    e.Row.ForeColor = Color.Gray;
        }



        public DataTable GetConfigSettingForBRS()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_SystemsettingBRSForODSD");
            proc.AddVarcharPara("@Option", 500, "BRSMandatory");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrder");
            dt = proc.GetTable();
            return dt;
        }
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
            GrdOrder.Columns[5].Visible = false;
            string filename = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            { filename = "Customer Delivery Pending Self"; }
            else
            {
                filename = "Customer Delivery Pending Our";
            }

            exporter.FileName = filename;
            exporter.FileName = filename;

            exporter.PageHeader.Left = filename;
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

        [WebMethod]
        public static string GetChallanIdIsExistInSalesInvoice(string keyValue)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.GetIdForCustomerDeliveryPendingExists(keyValue);

            //}
            return Convert.ToString(ispermission);

        }

        protected void GrdOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdOrder.JSProperties["cpinsert"] = null;
            GrdOrder.JSProperties["cpEdit"] = null;
            GrdOrder.JSProperties["cpUpdate"] = null;
            GrdOrder.JSProperties["cpDelete"] = null;
            GrdOrder.JSProperties["cpExists"] = null;
            GrdOrder.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (WhichCall == "Edit")
            {

                DataTable dtOrderStatus = objCRMSalesOrderDtlBL.GetSalesOrderStatusByOrderID(WhichType);


                if (dtOrderStatus.Rows.Count > 0 && dtOrderStatus != null)
                {
                    string orderid = Convert.ToString(dtOrderStatus.Rows[0]["orderid"]);
                    string orderNumber = Convert.ToString(dtOrderStatus.Rows[0]["orderNumber"]);
                    string Status = Convert.ToString(dtOrderStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtOrderStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtOrderStatus.Rows[0]["CustomerName"]);
                    GrdOrder.JSProperties["cpEdit"] = orderid + "~"
                                                    + orderNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            else if (WhichCall == "Delete")
            {
                deletecnt = objCRMSalesOrderDtlBL.DeleteSalesChallan(WhichType);
                if (deletecnt == 1)
                {
                    FillGrid();
                    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetGridData();
                }
                else
                {
                    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
            else if (WhichCall == "FilterGridByDate")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string DlvType = string.Empty;
                DlvType = Convert.ToString(Request.QueryString["type"]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataTable dtdata = new DataTable();

                if (!string.IsNullOrEmpty(DlvType) && IsFirstTimeLoad != "Y")
                {
                    hddnTypeIdd.Value = "1";
                    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                    {
                        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPendingDatewise(userbranch, lastCompany, "CustomerDeliveryPendingSelfByDate",
                                                BranchID, FromDate, ToDate);
                        //Session["CustomerDeliveryPendingSelfdt"] = dtdata;

                    }
                }
                else if (IsFirstTimeLoad != "Y")
                {
                    hddnTypeIdd.Value = "0";
                    if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                    {
                        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnCustomerDeliveryPendingDatewise(userbranch, lastCompany, "CustomerDeliveryPendingByDateOur",
                                                            BranchID, FromDate, ToDate);
                        //Session["CustomerDeliveryPendingOurdt"] = dtdata;
                    }
                }



                //dtdata = objCRMSalesOrderDtlBL.GetSalesInvoiceOnPendingDeliveryListByDateFiltering(userbranch, lastCompany, BranchID, FromDate, ToDate);
                //Session["CustomerDeliveryPendingdt"] = dtdata;
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdOrder.DataSource = dtdata;
                    GrdOrder.DataBind();
                }
                else
                {
                    GrdOrder.DataSource = null;
                    GrdOrder.DataBind();
                }
            }
        }
        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                
                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtRptModules = new DataTable();
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\DeliveryChallan\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\DeliveryChallan\DocDesign\Designes";
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
                    //if (reportValue != SavereportValue)
                    //{
                    CmbDesignName.Items.Add(name, reportValue);
                    //}
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
                SelectPanel.JSProperties["cpSuccess"] = "Success";
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }



        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        
        {
            e.KeyExpression = "SlNo";

          //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            List<int> branchidlist;

            if (!string.IsNullOrEmpty(DlvType))
            {
                hddnTypeIdd.Value = "1";
                if (IsFilter == "Y")
                {
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.V_CustomerDeliveryPendingListSelfs
                                where d.Bill_Date >= Convert.ToDateTime(strFromDate) && d.Bill_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchAssigned))
                                orderby d.Bill_Date descending
                                select d;

                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.V_CustomerDeliveryPendingListSelfs
                                where
                                d.Bill_Date >= Convert.ToDateTime(strFromDate) && d.Bill_Date <= Convert.ToDateTime(strToDate) &&
                                branchidlist.Contains(Convert.ToInt32(d.BranchAssigned))
                                orderby d.Bill_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.V_CustomerDeliveryPendingListSelfs
                            where d.Invoice_BranchId == 0
                            orderby d.Bill_Date descending
                            select d;
                    e.QueryableSource = q;
                }

            }
            else
            {
                hddnTypeIdd.Value = "0";
                if (IsFilter == "Y")
                {
                    if (strBranchID == "0")
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_CustomerDeliveryPendingLists
                                where d.Bill_Date >= Convert.ToDateTime(strFromDate) && d.Bill_Date <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.BranchAssigned))
                                orderby d.Bill_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                        var q = from d in dc.v_CustomerDeliveryPendingLists
                                where
                                d.Bill_Date >= Convert.ToDateTime(strFromDate) && d.Bill_Date <= Convert.ToDateTime(strToDate)
                                &&  branchidlist.Contains(Convert.ToInt32(d.BranchAssigned))
                                orderby d.Bill_Date descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_CustomerDeliveryPendingLists
                            where d.Invoice_BranchId == '0'
                            orderby d.Bill_Date descending
                            select d;
                    e.QueryableSource = q;
                }
            }


        }
    }
}