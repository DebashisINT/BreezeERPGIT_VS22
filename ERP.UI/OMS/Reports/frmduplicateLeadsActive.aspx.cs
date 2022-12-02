using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmduplicateLeadsActive : System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ShowReport();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void ShowReport()
        {
            // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_phonefax a, tbl_master_phonefax b, tbl_master_lead k, tbl_trans_activies k1, tbl_master_user k2 ", " DISTINCT  a.phf_cntId AS col1, k.cnt_firstName AS col2, a.phf_phoneNumber AS col3, k1.act_activityno AS col4, k2.user_name AS col5,  CASE LEFT(k.cnt_Status, 2) WHEN 'PC' THEN 'Active' ELSE 'Due' END AS col6 ", " k.cnt_internalId = a.phf_cntId AND a.phf_phoneNumber = b.phf_phoneNumber AND a.phf_cntId <> b.phf_cntId AND a.phf_entity = 'Lead' AND   a.phf_phoneNumber <> ' ' AND k.cnt_Status LIKE '%pc%' AND k1.act_activityno = k.cnt_Status AND k2.user_id = k1.act_assignedTo ORDER BY a.phf_phoneNumber ");

            ReportDocument LeadReportDocu = new ReportDocument();
            string path = Server.MapPath("..\\Reports\\DuplicateAssignRepo.rpt");
            LeadReportDocu.Load(path);
            //_________Code to chnage Look and display area__________//

            //___________Enad________________________________________//

            LeadReportDocu.SetDataSource(DT);
            //CrystalReportViewer1.ReportSource = LeadReportDocu;
            //CrystalReportViewer1.DataBind();
        }
    }
}