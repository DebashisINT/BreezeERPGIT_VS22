using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Services;
using System.IO;

namespace ERP.OMS.Management.Activities
{
    public partial class PendingDeliveryGRNList : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        PurchaseChallanBL objPurchaseChallanBL = new PurchaseChallanBL();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PendingDeliveryGRNList.aspx");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

            }
            FillGrid();
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
            DataTable dtdata = new DataTable();
            string BranchID = Convert.ToString(cmbBranchfilter.Value);
            string FromDate = FormDate.Date.ToString("yyyy-MM-dd");
            string ToDate = toDate.Date.ToString("yyyy-MM-dd");

            if (HttpContext.Current.Session["LastCompany"] != null)
            {

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                {
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    dtdata = objPurchaseChallanBL.GetPurchaseGRNPendingDeliveryListByDateFiltering(lastCompany, BranchID, FromDate, ToDate);
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
            string filename = "Sales Challan";
            exporter.FileName = filename;
            exporter.FileName = "GrdChallan";

            exporter.PageHeader.Left = "Sales Challan";
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
        public static String GetChallanTransporterDetails(string Challan_Id)
        {

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dt = objSlaesActivitiesBL.GetChallanIdOnTransporter(Challan_Id);
            string trns_Id = "";
            if (dt.Rows.Count > 0)
            {
                trns_Id = Convert.ToString(dt.Rows[0]["ID"]);
            }

            return trns_Id;
        }

        protected void GrdOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdOrder.JSProperties["cpinsert"] = null;
            GrdOrder.JSProperties["cpEdit"] = null;
            GrdOrder.JSProperties["cpUpdate"] = null;
            GrdOrder.JSProperties["cpDelete"] = null;
            GrdOrder.JSProperties["cpExists"] = null;
            GrdOrder.JSProperties["cpUpdateValid"] = null;


            string Validate = string.Empty;

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (WhichCall == "FilterGridByDate")
            {
                //DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                //DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                //string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //string finyear = Convert.ToString(Session["LastFinYear"]);

                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];

                string branchID = (branch == "0") ? Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) : branch;

                DataTable dtdata = new DataTable();

                dtdata = objPurchaseChallanBL.GetPurchaseGRNPendingDeliveryListByDateFiltering(lastCompany, branchID, fromdate, toDate);
                if (dtdata != null)
                {
                    GrdOrder.DataSource = dtdata;
                    GrdOrder.DataBind();
                }

                // FillGrid();

            }
            else if (WhichCall == "SaveTransporter")//Newly Added for saving Transporter
            {
                if (!string.IsNullOrEmpty(WhichType))
                {
                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(WhichType), "PC", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                        Validate = "UpdateTransport";
                    }
                }
                GrdOrder.JSProperties["cpBindTransport"] = Validate;
            }
            else if (WhichCall == "BindTransPorter")//Newly Added for saving Transporter
            {

                if (!string.IsNullOrEmpty(WhichType))
                {

                    VehicleDetailsControl.BindDataByDocID(WhichType, "SC");
                    //GrdOrder.JSProperties["cpBindTransport"] = Validate;

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

           // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    //DesignPath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\Pending";
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\Normal";
                }
                else
                {
                    //DesignPath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\Pending";
                    DesignPath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\Normal";
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
                //string NoofCopy = "";
                //if (selectOriginal.Checked == true)
                //{
                //    NoofCopy += 1 + ",";
                //}
                //if (selectDuplicate.Checked == true)
                //{
                //    NoofCopy += 2 + ",";
                //}
                //if (selectTriplicate.Checked == true)
                //{
                //    NoofCopy += 3 + ",";
                //}
                //SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                //SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        [WebMethod]
        public static String GetTransporterDetailsForInvoice(string GRN_Id)
        {
            string ExistsFlag = "0";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetPendingDeliveryGRN");
            proc.AddVarcharPara("@Action", 100, "TransporterForPurchaseChallan");
            proc.AddNVarcharPara("@Doc_Id", 50, GRN_Id);
            proc.AddVarcharPara("@Doc_type", 100, "PC");
            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                ExistsFlag = Convert.ToString(dt.Rows[0]["trp_Id"]);
            }
            return ExistsFlag;
        }

    }
}