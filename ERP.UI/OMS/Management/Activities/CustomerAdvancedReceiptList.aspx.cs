using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerAdvancedReceiptList : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //................Cookies..................
                Grid_CustomerAdvancedReceipt.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerAdvancedReceiptList";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerAdvancedReceiptList');</script>");
                //...........Cookies End............... 
                Session["exportval"] = null;
                Session["SaveModeCRP"] = null;
                Session["AdvanceReceiptDetails"] = null;
            }
            FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerAdvancedReceiptList.aspx");
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            //exporter.GridView = Grid_ContraVoucher;

            exporter.GridViewID = "Grid_CustomerReceiptPayment";
            string filename = "CustomerReceiptPayment";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Customer Advanced Receipt";
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
        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = GetCustomerAdvancedReceiptListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_CustomerAdvancedReceipt.DataSource = dtdata;
                Grid_CustomerAdvancedReceipt.DataBind();
            }
            else
            {
                Grid_CustomerAdvancedReceipt.DataSource = null;
                Grid_CustomerAdvancedReceipt.DataBind();
            }
        }
        public DataTable GetCustomerAdvancedReceiptListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void Grid_CustomerReceiptPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string ReceiptPayment_ID = null;
            int deletecnt = 0;
            int _depCnt = 0;
            Boolean IsExist = false;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    ReceiptPayment_ID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (Command == "Delete")
            {
                if (!IsCRTTransactionExist(ReceiptPayment_ID))
                {
                    //DataTable dt = new DataTable();
                    //dt = objCustomerVendorReceiptPaymentBL.DeleteCustomerDependentOrder(ReceiptPayment_ID);
                    //if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                    //{
                    //    IsExist = true;
                    //}

                    //if (IsExist) //dependent record present
                    //{
                    //    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "This Receipt number is tagged in another Payment. Cannot Delete.";
                    //}
                    //else  //dependent record not present
                    //{
                    if (objCustomerVendorReceiptPaymentBL.IsUnpaidAmountEqual(ReceiptPayment_ID))
                    {
                        deletecnt = objCustomerVendorReceiptPaymentBL.DeleteCRPADVOrder(ReceiptPayment_ID);

                        if (deletecnt == 1)
                        {
                            Grid_CustomerAdvancedReceipt.JSProperties["cpDelete"] = "Deleted successfully";

                        }
                        else
                        {
                            Grid_CustomerAdvancedReceipt.JSProperties["cpDelete"] = "Try again.";
                        }
                        //}
                    }
                    else
                    {
                        Grid_CustomerAdvancedReceipt.JSProperties["cpDelete"] = "Cannot delete As Vouchar Amount and Unpaid Amount is not equal";
                    }

                    
                }
                else
                {
                    Grid_CustomerAdvancedReceipt.JSProperties["cpDelete"] = "This Customer Receipt Payment  is tagged in other modules. Cannot Delete.";
                }
            }
        }
        private bool IsCRTTransactionExist(string CRTid)
        {
            bool IsExist = false;
            if (CRTid != "" && Convert.ToString(CRTid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objCustomerVendorReceiptPaymentBL.CheckCRTTraanaction(CRTid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
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
        
    }
}