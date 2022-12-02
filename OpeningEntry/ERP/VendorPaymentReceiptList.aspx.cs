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

namespace OpeningEntry.ERP
{
    public partial class VendorPaymentReceiptList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                Session["SaveModeVPR"] = null;
                //................Cookies..................
                Grid_CustomerReceiptPayment.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt');</script>");
                //...........Cookies End............... 
            }
            FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/VendorPaymentReceiptList.aspx");
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
            string filename = "VendorPaymentReceipt";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Vendor Payment/Receipt";
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
            DataTable dtdata = GetCustomerReceiptPaymentListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_CustomerReceiptPayment.DataSource = dtdata;
                Grid_CustomerReceiptPayment.DataBind();
            }
            else
            {
                Grid_CustomerReceiptPayment.DataSource = null;
                Grid_CustomerReceiptPayment.DataBind();
            }
        }
        public DataTable GetCustomerReceiptPaymentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "VendorReceiptPaymentDetailsOpening");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 500, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void Grid_CustomerReceiptPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string POID = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    POID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }
            Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = null;
            if (Command == "Delete")
            {
                deletecnt = objCustomerVendorReceiptPaymentBL.DeleteVendorPROrder(POID);

                if (deletecnt == 1)
                {
                    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Deleted successfully";

                }
                else
                {
                    Grid_CustomerReceiptPayment.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
        }
    }
}