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

namespace ERP.OMS.Reports.XtraReports
{
    public partial class ReportMain : System.Web.UI.Page
    {
        
        public DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
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
            string reportType = HttpContext.Current.Request.QueryString["reportType"];
            Session["ReportType"] = reportType;
            if (!IsPostBack)
            {
                bindDropDown(reportType);
            }

            //string[] filePaths = System.IO.Directory.GetFiles(@"D:\VSS ERP\ERP.UI\OMS\Reports\RepxReportDesign");            
            //foreach (string filename in filePaths)
            //{
            //    string reportname = Path.GetFileNameWithoutExtension(filename);
            //    ddReportName.Items.Add(reportname);
            //}

        }

        public void bindDropDown(String modKey)
        {
            DataTable reportDt = oDBEngine.GetDataTable("tbl_reportDesigner", "Id,LayoutData,ReportName", "ModuleKey='"+modKey+"'");
            ddReportName.DataSource = reportDt;
            ddReportName.DataTextField = "ReportName";
            ddReportName.DataValueField = "Id";
            ddReportName.DataBind();
        
        }       

        protected void btnDesign_Click(object sender, EventArgs e)
        {
            int reportId = Convert.ToInt32(ddReportName.SelectedValue);
            HttpContext.Current.Response.Redirect("~/OMS/Reports/reportDesignerXtra.aspx?rptId="+reportId.ToString());
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            int reportId = Convert.ToInt32(ddReportName.SelectedValue);
            HttpContext.Current.Response.Redirect("~/OMS/Reports/XtraReportTest.aspx?rptId=" + reportId.ToString());
        }

      
        protected void btnNewFileSave_Click(object sender, EventArgs e)
        {
            int id = rpLayout.copyLayout(Convert.ToInt32(ddReportName.SelectedValue), txtFileName.Text, Convert.ToString(Session["ReportType"]));
            HttpContext.Current.Response.Redirect("~/OMS/Reports/reportDesignerXtra.aspx?rptId=" + Convert.ToString(id));

        }
        
    }
}