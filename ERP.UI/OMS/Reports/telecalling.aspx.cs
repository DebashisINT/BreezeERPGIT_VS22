using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_telecalling : System.Web.UI.Page
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
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            if (!IsPostBack)
            {

            }
            showReport(cmbCallType.Value);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void showReport(object CallType)
        {
            string userids = oDBEngine.getChildUser_for_report(HttpContext.Current.Session["userid"].ToString(), "") + HttpContext.Current.Session["userid"];
            if (RBReportType.Value.ToString() == "Screen")
            {
                gridCallOutcome.Visible = true;
                DataTable DT = oDBEngine.GetDataTable(" tbl_trans_phonecalldetails INNER JOIN  tbl_trans_phonecall ON tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id INNER JOIN  tbl_master_lead ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN  tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id ", " DISTINCT CASE (SELECT COUNT(*) FROM (SELECT TOP 1 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt]  FROM  tbl_trans_phonecalldetails  WHERE  (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id)  ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 1 THEN (SELECT     TOP 1 [Second Attemt] FROM (SELECT TOP 1 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails   WHERE  (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id)  ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA   ORDER BY [Second Attemt] DESC) ELSE '-' END AS [First Attempt], CASE (SELECT  COUNT(*) FROM (SELECT  TOP 2 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails  WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 2 THEN (SELECT     TOP 1 [Second Attemt] FROM  (SELECT TOP 2 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM  tbl_trans_phonecalldetails WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA ORDER BY [Second Attemt] DESC) ELSE '-' END AS [Second Attempt],CASE  (SELECT COUNT(*) FROM (SELECT TOP 3 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails  WHERE      (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id)    ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 3 THEN  (SELECT     TOP 1 [Second Attemt]  FROM (SELECT     TOP 3 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM  tbl_trans_phonecalldetails   WHERE      (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA ORDER BY [Second Attemt] DESC) ELSE '-' END AS [Third Attempt],CASE (SELECT  COUNT(*) FROM (SELECT  TOP 2 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails  WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 4 THEN (SELECT     TOP 1 [Second Attemt] FROM  (SELECT TOP 4 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM  tbl_trans_phonecalldetails WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA ORDER BY [Second Attemt] DESC) ELSE '-' END AS [Forth Attempt], CASE (SELECT  COUNT(*) FROM (SELECT  TOP 5 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails  WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 5 THEN (SELECT     TOP 1 [Second Attemt] FROM  (SELECT TOP 5 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM  tbl_trans_phonecalldetails WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA ORDER BY [Second Attemt] DESC) ELSE '-' END AS [Fifth Attempt], CASE (SELECT  COUNT(*) FROM (SELECT  TOP 6 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM tbl_trans_phonecalldetails  WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA) WHEN 6 THEN (SELECT     TOP 1 [Second Attemt] FROM  (SELECT TOP 6 tbl_trans_phonecalldetails.phd_callStart AS [Second Attemt] FROM  tbl_trans_phonecalldetails WHERE (tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id) ORDER BY tbl_trans_phonecalldetails.phd_callStart) AS AA ORDER BY [Second Attemt] DESC) ELSE '-' END AS [Sixth Attempt], tbl_master_user.user_name,tbl_master_lead.cnt_firstName as Name ", " (tbl_trans_phonecall.phc_callDispose = " + CallType + ") AND (tbl_trans_Activies.act_assignedTo IN (" + userids + "))");
                gridCallOutcome.DataSource = DT.DefaultView;
                gridCallOutcome.DataBind();
            }
            if (RBReportType.Value.ToString() == "Print")
            {
                gridCallOutcome.Visible = false;
            }
        }

        protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showReport(cmbCallType.Value);
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            showReport(cmbCallType.Value);
        }
    }
}