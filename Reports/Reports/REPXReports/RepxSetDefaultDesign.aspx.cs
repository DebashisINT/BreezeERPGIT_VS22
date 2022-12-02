using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogicLayer;
using System.Configuration;
using System.IO;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;

namespace Reports.Reports.REPXReports
{
    public partial class RepxSetDefaultDesign : System.Web.UI.Page
    {
        public DBEngine oDBEngine = new DBEngine();
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                DataTable dtRptModules = new DataTable();
                string query = "";
                query = @"Select Module_Name from tbl_trans_SetDefaultDesign_Report order by Module_Id ";
                dtRptModules = oDbEngine.GetDataTable(query);
               
                foreach (DataRow row in dtRptModules.Rows)
                {
                    ddModuleName.Items.Add(Convert.ToString(row[0]));
                }
                ddModuleName.SelectedIndex = 0;
                loaddesign(Convert.ToString(ddModuleName.Value));
            }
        }

        //protected void ddModuleName_SelectedIndexChanged(object sender, EventArgs e)
        //protected void ddModuleName_CustomCallback(object source, CallbackEventArgsBase e)
        protected void ddReportName_CustomCallback(object source, CallbackEventArgsBase e)
        {
            string[] filePaths = new string[] { };
            //string ModuleName = Convert.ToString(ddModuleName.Value);
            string ModuleName = Convert.ToString(e.Parameter);

            if (ModuleName == "Sales Order")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes"), "*.repx");
            }
            else if (ModuleName == "Sales Challan")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Designes"), "*.repx");
            }
            else if (ModuleName == "Sales Invoice")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal"), "*.repx");
            }
            else if (ModuleName == "Sales Invoice(POS)")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS"), "*.repx");
            }
            else if (ModuleName == "Installation Coupon")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon"), "*.repx");
            }
            else if (ModuleName == "Purchase Order")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes"), "*.repx");
            }

            else if (ModuleName == "Branch Requisition")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes"), "*.repx");
            }
            //ddReportName.Items.Add("--Select--");
            ddReportName.Items.Clear();
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
                ddReportName.Items.Add(name, reportValue);
            }
            ddReportName.SelectedIndex = 0;
        }

        protected void loaddesign(string ModuleName)
        {
            string[] filePaths = new string[] { };

            if (ModuleName == "Sales Order")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes"), "*.repx");
            }
            else if (ModuleName == "Sales Challan")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Designes"), "*.repx");
            }
            else if (ModuleName == "Sales Invoice")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal"), "*.repx");
            }
            else if (ModuleName == "Sales Invoice(POS)")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS"), "*.repx");
            }
            else if (ModuleName == "Installation Coupon")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon"), "*.repx");
            }
            else if (ModuleName == "Purchase Order")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes"), "*.repx");
            }

            else if (ModuleName == "Branch Requisition")
            {
                filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes"), "*.repx");
            }
            //ddReportName.Items.Add("--Select--");
            ddReportName.Items.Clear();
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
                ddReportName.Items.Add(name, reportValue);
            }
            ddReportName.SelectedIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ModuleName = Convert.ToString(ddModuleName.Value);
            string DesignName = Convert.ToString(ddReportName.Value);
            string filePath = "";
            if (ModuleName == "Sales Invoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal/" + DesignName + ".repx");
            }
            else if (ModuleName == "Sales Invoice(POS)")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS/" + DesignName + ".repx");
            }
            else if (ModuleName == "Installation Coupon")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon/" + DesignName + ".repx");
            }
            else if (ModuleName == "Branch Requisition")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes/" + DesignName + ".repx");
            }
            else if (ModuleName == "Purchase Order")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes/" + DesignName + ".repx");
            }
            else if (ModuleName == "Sales Order")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes/" + DesignName + ".repx");
            }
            //DesignName = DesignName.Split('~')[0];
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PROC_UPDATESetDefaultDesign_Report", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Module_Name", ModuleName);//1
            cmd.Parameters.AddWithValue("@Design_name", DesignName);//2
            cmd.Parameters.AddWithValue("@Design_Fullpath", filePath);//3
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}