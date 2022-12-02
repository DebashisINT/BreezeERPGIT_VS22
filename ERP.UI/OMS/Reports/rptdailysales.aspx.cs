using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_rptdailysales : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable DtUserid = new DataTable();
        DataTable dt_treeView = new DataTable();
        //Converter OConvert = new Converter();
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
              //  Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                TxtStartDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtStartDate.Value = oDBEngine.GetDate();
                LoadTreeView();
                TreeAccessebility.Attributes.Add("onexpand", "heightw()");
                //ImgStartDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_TxtStartDate,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_TxtStartDate,true,null,'0',0)");
                //TxtStartDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_TxtStartDate ,'dd/mm/yyyy',this,true,null,'0',0)");
                All.Attributes.Add("onclick", "All_CheckedChanged();");
                Specific.Attributes.Add("onclick", "Specific_CheckedChanged();");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>page_load()</script>");

            }
            TxtStartDate.Attributes.Add("readonly", "true");
            if (IsPostBack)
            {
                BindAll();
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        private void PopulateTreeView(int Node, TreeNode treeNode, string EmployeeId)
        {
            string EmpcId = "";
            if (EmployeeId != "")
            {
                DtUserid = oDBEngine.GetDataTable(" tbl_master_employee e,tbl_master_contact c ", " e.emp_id,(c.cnt_firstname + ' '+c.cnt_middlename+' '+c.cnt_lastname) as name ", " e.emp_contactid in (select tbl_master_employee.emp_contactid from tbl_trans_employeeCTC,tbl_master_employee where tbl_trans_employeeCTC.emp_reportTo = '" + EmployeeId + "' and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900')) and e.emp_contactid=c.cnt_internalid");
                if (DtUserid.Rows.Count != 0)
                {
                    for (int J = 0; J < DtUserid.Rows.Count; J++)
                    {
                        TreeNode parentNode = new TreeNode(DtUserid.Rows[J][1].ToString(), DtUserid.Rows[J][0].ToString());
                        EmpcId = DtUserid.Rows[J][0].ToString();
                        if (EmpcId != "")
                        {
                            DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_employee e,tbl_master_contact c ", " e.emp_id,(c.cnt_firstname + ' '+c.cnt_middlename+' '+c.cnt_lastname) as name ", " e.emp_contactid in (select tbl_master_employee.emp_contactid from tbl_trans_employeeCTC,tbl_master_employee where tbl_trans_employeeCTC.emp_reportTo = '" + EmpcId + "' and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900')) and e.emp_contactid=c.cnt_internalid");
                            if (dt1.Rows.Count != 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    TreeNode ChildNode = new TreeNode(dt1.Rows[i][1].ToString(), dt1.Rows[i][0].ToString());
                                    ChildNode.ShowCheckBox = true;
                                    parentNode.ChildNodes.Add(ChildNode);
                                }
                            }
                        }
                        parentNode.ShowCheckBox = true;
                        treeNode.ChildNodes.Add(parentNode);
                        //PopulateTreeView(Int32.Parse(DtUserid.Rows[J][0].ToString()), ChildNode, EmpcId);
                    }
                }
            }
        }
        private void LoadTreeView()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_employee e,tbl_master_user u", "e.emp_id,u.user_name", " e.emp_contactid=u.user_contactid and u.user_id='" + Session["userid"].ToString() + "'");
            TreeAccessebility.Nodes.Clear();
            TreeNode NodeNew = new TreeNode();
            NodeNew.Value = dt.Rows[0][0].ToString();
            NodeNew.Text = dt.Rows[0][1].ToString();
            NodeNew.ShowCheckBox = true;
            TreeAccessebility.Nodes.Add(NodeNew);
            TreeNode NodeNew1 = new TreeNode();
            NodeNew1 = TreeAccessebility.Nodes[0];
            PopulateTreeView(0, NodeNew1, dt.Rows[0][0].ToString());
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (drdExport.Value.ToString() != "")
        //    {
        //        if (drdExport.Value.ToString() == "Pdf")
        //            ASPxTreeListExporter1.WritePdfToResponse();
        //        if (drdExport.Value.ToString() == "Xls")
        //            ASPxTreeListExporter1.WriteXlsToResponse();
        //        if (drdExport.Value.ToString() == "Rtf")
        //            ASPxTreeListExporter1.WriteRtfToResponse();
        //    }
        //}
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (RBReportType.SelectedItem.Value.ToString() == "Screen")
            {
                ShowTreeList();
            }
            else
            {
                showCrystalReport();
            }
        }

        public void showCrystalReport()
        {

        }

        public void ShowTreeList()
        {
            int parent_id = -1;
            string id = "";
            string Date = TxtStartDate.Value.ToString();
            DataTable DT = new DataTable();
            if (TreeAccessebility.CheckedNodes.Count > 0)
            {
                for (int i = 0; i < TreeAccessebility.CheckedNodes.Count; i++)
                {
                    if (TreeAccessebility.CheckedNodes[i].Checked)
                    {
                        if (parent_id == -1 || parent_id != int.Parse(TreeAccessebility.CheckedNodes[i].Value))
                        {
                            parent_id = int.Parse(TreeAccessebility.CheckedNodes[i].Value);
                        }
                    }
                    id += TreeAccessebility.CheckedNodes[i].Value.ToString() + ",";

                }
            }
            if (All.Checked == true)
            {
                GridDailyActivity.Visible = true;
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TrTreeExport').style.display = 'inline';document.getElementById('TdTree').style.display = 'none';document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'inline';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                BindAll();
                //DT = oDBEngine.GetDataTable("dbo.tbl_trans_Activies INNER JOIN  dbo.tbl_trans_salesVisit ON dbo.tbl_trans_Activies.act_id = dbo.tbl_trans_salesVisit.slv_activityId INNER JOIN  dbo.tbl_master_user ON dbo.tbl_trans_Activies.act_assignedBy = dbo.tbl_master_user.user_id INNER JOIN dbo.tbl_master_user tbl_master_user_1 ON dbo.tbl_trans_Activies.act_assignedTo = tbl_master_user_1.user_id", "DISTINCT TOP 100 PERCENT tbl_master_user_1.user_name AS [Assign To], dbo.tbl_master_user.user_name AS [Assign By], ISNULL ((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Fresh Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Follow Ups/Old Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) + ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9)GROUP BY tbl_trans_Activies.act_assignedTo) vv  WHERE     (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Total Vists], ISNULL((SELECT     T  FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (tbl_trans_salesVisit.slv_salesvisitoutcome IN (9)) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Pending Visits], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (2, 3))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Followup/Rescheduled], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (4)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Confirm Sales], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM tbl_master_SalesVisitOutCome WHERE      slv_category IN (7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE  (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Interested/Lost], ISNULL ((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (5)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Contactable], ISNULL((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM          tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (6))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Useable], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM  tbl_master_SalesVisitOutCome WHERE slv_category IN (4))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)) * 100 / (SELECT     T  FROM          (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (2, 3, 4, 7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE      (ff = tbl_trans_Activies.act_assignedTo)), 0) AS Effectiveness, dbo.tbl_trans_Activies.act_assignedTo AS assignedtoid, dbo.tbl_trans_Activies.act_assignedBy AS assignedByid ", null);
            }
            else
            {
                try
                {
                    GridDailyActivity.Visible = true;
                    int UId = id.LastIndexOf(',');
                    id = id.Substring(0, UId);
                    DT = oDBEngine.GetDataTable("dbo.tbl_trans_Activies INNER JOIN  dbo.tbl_trans_salesVisit ON dbo.tbl_trans_Activies.act_id = dbo.tbl_trans_salesVisit.slv_activityId INNER JOIN  dbo.tbl_master_user ON dbo.tbl_trans_Activies.act_assignedBy = dbo.tbl_master_user.user_id INNER JOIN dbo.tbl_master_user tbl_master_user_1 ON dbo.tbl_trans_Activies.act_assignedTo = tbl_master_user_1.user_id ", "DISTINCT TOP 100 PERCENT tbl_master_user_1.user_name AS [Assign To], dbo.tbl_master_user.user_name AS [Assign By], ISNULL ((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Fresh Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Follow Ups/Old Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) + ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9)GROUP BY tbl_trans_Activies.act_assignedTo) vv  WHERE     (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Total Vists], ISNULL((SELECT     T  FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (tbl_trans_salesVisit.slv_salesvisitoutcome IN (9)) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Pending Visits], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (2, 3))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Followup/Rescheduled], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (4)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Confirm Sales], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM tbl_master_SalesVisitOutCome WHERE      slv_category IN (7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE  (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Interested/Lost], ISNULL ((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (5)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Contactable], ISNULL((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM          tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (6))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Useable], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM  tbl_master_SalesVisitOutCome WHERE slv_category IN (4))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)) * 100 / (SELECT     T  FROM          (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (2, 3, 4, 7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE      (ff = tbl_trans_Activies.act_assignedTo)), 0) AS Effectiveness, dbo.tbl_trans_Activies.act_assignedTo AS assignedtoid, dbo.tbl_trans_Activies.act_assignedBy AS assignedByid ", "dbo.tbl_trans_Activies.act_assignedTo in (" + id + ") GROUP BY dbo.tbl_trans_Activies.act_assignedTo, dbo.tbl_trans_salesVisit.slv_salesvisitoutcome, dbo.tbl_trans_Activies.act_assignedBy, tbl_master_user_1.user_name, dbo.tbl_master_user.user_name");
                    if (DT.Rows.Count > 0)
                    {
                        string popupScript = "";
                        popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'none';</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                        GridDailyActivity.DataSource = DT.DefaultView;
                        GridDailyActivity.DataBind();
                    }
                    else
                    {
                        string popupScript = "";
                        popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TdExpand').style.display = 'none';document.getElementById('TdCollapse').style.display = 'none';</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                        GridDailyActivity.DataSource = DT.DefaultView;
                        GridDailyActivity.DataBind();
                    }
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please Select Any User')</script>");
                    GridDailyActivity.Visible = false;
                }
            }
        }
        public void BindAll()
        {
            dt_treeView = new DataTable();
            DataColumn dc_ID = new DataColumn("ID");
            DataColumn dc_ParentID = new DataColumn("ParentID");
            DataColumn dc_AssignTo = new DataColumn("Assign To");
            DataColumn dc_AssignBy = new DataColumn("Assign By");
            DataColumn dc_FreshVisits = new DataColumn("Fresh Visits");
            DataColumn dc_FollowUpsOldVisits = new DataColumn("Follow Ups/Old Visits");
            DataColumn dc_TotalVists = new DataColumn("Total Vists");
            DataColumn dc_PendingVisits = new DataColumn("Pending Visits");
            DataColumn dc_FollowupRescheduled = new DataColumn("Followup/Rescheduled");
            DataColumn dc_ConfirmSales = new DataColumn("Confirm Sales");
            DataColumn dc_NotInterestedLost = new DataColumn("Not Interested/Lost");
            DataColumn dc_NotContactable = new DataColumn("Not Contactable");
            DataColumn dc_NotUseable = new DataColumn("Not Useable");
            DataColumn dc_Effectiveness = new DataColumn("Effectiveness");
            DataColumn dc_assignedtoid = new DataColumn("assignedtoid");
            DataColumn dc_assignedByid = new DataColumn("assignedByid");
            dt_treeView.Columns.Add(dc_ID);
            dt_treeView.Columns.Add(dc_ParentID);
            dt_treeView.Columns.Add(dc_AssignTo);
            dt_treeView.Columns.Add(dc_AssignBy);
            dt_treeView.Columns.Add(dc_FreshVisits);
            dt_treeView.Columns.Add(dc_FollowUpsOldVisits);
            dt_treeView.Columns.Add(dc_TotalVists);
            dt_treeView.Columns.Add(dc_PendingVisits);
            dt_treeView.Columns.Add(dc_FollowupRescheduled);
            dt_treeView.Columns.Add(dc_ConfirmSales);
            dt_treeView.Columns.Add(dc_NotInterestedLost);
            dt_treeView.Columns.Add(dc_NotContactable);
            dt_treeView.Columns.Add(dc_NotUseable);
            dt_treeView.Columns.Add(dc_Effectiveness);
            dt_treeView.Columns.Add(dc_assignedtoid);
            dt_treeView.Columns.Add(dc_assignedByid);
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_employee e,tbl_master_user u", "e.emp_id,u.user_name", " e.emp_contactid=u.user_contactid and u.user_id='" + Session["userid"].ToString() + "'");
            populatetreeGrid(DT.Rows[0][0].ToString(), "0");
            GetUserList_call_CalculateEmployeeDailyCall(DT.Rows[0][0].ToString());
            GridDailyActivity.DataSource = dt_treeView.DefaultView;
            GridDailyActivity.DataBind();
        }
        private void GetUserList_call_CalculateEmployeeDailyCall(string EmployeeID)
        {
            if (EmployeeID != "")
            {

                string[,] userids = oDBEngine.GetFieldValue(" tbl_master_employee e,tbl_master_contact c ", " e.emp_id,(c.cnt_firstname + ' '+c.cnt_middlename+' '+c.cnt_lastname) as name ", " e.emp_contactid in (select tbl_master_employee.emp_contactid from tbl_trans_employeeCTC,tbl_master_employee where tbl_trans_employeeCTC.emp_reportTo in (select emp.emp_id from tbl_master_employee emp,tbl_master_user user1 where emp.emp_contactid=user1.user_contactid and user1.user_id='" + EmployeeID + "') and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900')) and e.emp_contactid=c.cnt_internalid", 2);
                if (userids[0, 0] != "n")
                {
                    for (int i = 0; i < userids.Length / 2; i++)
                    {
                        if (userids[i, 0] != "")
                        {
                            populatetreeGrid(userids[i, 0], EmployeeID);
                            GetUserList_call_CalculateEmployeeDailyCall(userids[i, 0]);
                        }
                    }
                }
            }
        }
        private void populatetreeGrid(string EmployeeId, string ReportingHeadId)
        {
            if (EmployeeId != "")
            {
                string Date = TxtStartDate.Value.ToString();
                //  DateTime Date = DateTime.Today;

                DataTable DT = oDBEngine.GetDataTable("dbo.tbl_trans_Activies INNER JOIN  dbo.tbl_trans_salesVisit ON dbo.tbl_trans_Activies.act_id = dbo.tbl_trans_salesVisit.slv_activityId INNER JOIN  dbo.tbl_master_user ON dbo.tbl_trans_Activies.act_assignedBy = dbo.tbl_master_user.user_id INNER JOIN dbo.tbl_master_user tbl_master_user_1 ON dbo.tbl_trans_Activies.act_assignedTo = tbl_master_user_1.user_id ", "DISTINCT TOP 100 PERCENT tbl_master_user_1.user_name AS [Assign To], dbo.tbl_master_user.user_name AS [Assign By], ISNULL ((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Fresh Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Follow Ups/Old Visits], ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime),101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9) GROUP BY tbl_trans_Activies.act_assignedTo) vv WHERE(dd = tbl_trans_Activies.act_assignedTo)), 0) + ISNULL((SELECT j FROM(SELECT tbl_trans_Activies.act_assignedTo AS dd, COUNT(tbl_trans_salesVisit.slv_id) AS j FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar,CAST(tbl_trans_Activies.act_scheduledDate AS datetime),101) < CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (CONVERT(varchar,CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome <> 9)GROUP BY tbl_trans_Activies.act_assignedTo) vv  WHERE     (dd = tbl_trans_Activies.act_assignedTo)), 0) AS [Total Vists], ISNULL((SELECT     T  FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (tbl_trans_salesVisit.slv_salesvisitoutcome IN (9)) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Pending Visits], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (2, 3))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Followup/Rescheduled], ISNULL ((SELECT     T FROM (SELECT COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (4)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Confirm Sales], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN(SELECT     slv_id FROM tbl_master_SalesVisitOutCome WHERE      slv_category IN (7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE  (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Interested/Lost], ISNULL ((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM  tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome  WHERE      slv_category IN (5)))GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Contactable], ISNULL((SELECT     T FROM         (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff FROM          tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (6))) GROUP BY tbl_trans_Activies.act_assignedTo) dd WHERE     (ff = tbl_trans_Activies.act_assignedTo)), 0) AS [Not Useable], ISNULL ((SELECT     T FROM (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM  tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId WHERE(CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id FROM  tbl_master_SalesVisitOutCome WHERE slv_category IN (4))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE     (ff = tbl_trans_Activies.act_assignedTo)) * 100 / (SELECT     T  FROM          (SELECT     COUNT(tbl_trans_salesVisit.slv_id) AS T, tbl_trans_Activies.act_assignedTo AS ff  FROM          tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId  WHERE      (CONVERT(varchar, CAST(tbl_trans_salesVisit.slv_lastDatevisit AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)) AND (tbl_trans_salesVisit.slv_salesvisitoutcome IN (SELECT     slv_id  FROM          tbl_master_SalesVisitOutCome WHERE      slv_category IN (2, 3, 4, 7))) GROUP BY tbl_trans_Activies.act_assignedTo) dd  WHERE      (ff = tbl_trans_Activies.act_assignedTo)), 0) AS Effectiveness, dbo.tbl_trans_Activies.act_assignedTo AS assignedtoid, dbo.tbl_trans_Activies.act_assignedBy AS assignedByid ", "dbo.tbl_trans_Activies.act_assignedTo in (" + EmployeeId + ") GROUP BY dbo.tbl_trans_Activies.act_assignedTo, dbo.tbl_trans_salesVisit.slv_salesvisitoutcome, dbo.tbl_trans_Activies.act_assignedBy, tbl_master_user_1.user_name, dbo.tbl_master_user.user_name");

                if (DT.Rows.Count > 0)
                {
                    DataRow DR = dt_treeView.NewRow();
                    DR["Assign To"] = DT.Rows[0]["Assign To"].ToString();
                    DR["Assign By"] = DT.Rows[0]["Assign By"].ToString();
                    DR["Fresh Visits"] = DT.Rows[0]["Fresh Visits"].ToString();
                    DR["Follow Ups/Old Visits"] = DT.Rows[0]["Follow Ups/Old Visits"].ToString();
                    DR["Total Vists"] = DT.Rows[0]["Total Vists"].ToString();
                    DR["Pending Visits"] = DT.Rows[0]["Pending Visits"].ToString();
                    DR["Followup/Rescheduled"] = DT.Rows[0]["Followup/Rescheduled"].ToString();
                    DR["Confirm Sales"] = DT.Rows[0]["Confirm Sales"].ToString();
                    DR["Not Interested/Lost"] = DT.Rows[0]["Not Interested/Lost"].ToString();
                    DR["Not Contactable"] = DT.Rows[0]["Not Contactable"].ToString();
                    DR["Not Useable"] = DT.Rows[0]["Not Useable"].ToString();
                    DR["Effectiveness"] = DT.Rows[0]["Effectiveness"].ToString();
                    DR["assignedtoid"] = DT.Rows[0]["assignedtoid"].ToString();
                    DR["assignedByid"] = DT.Rows[0]["assignedByid"].ToString();
                    DR["ID"] = EmployeeId;
                    DR["ParentID"] = ReportingHeadId;
                    dt_treeView.Rows.Add(DR);
                }
            }
        }
        // protected void GridDailyActivity_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        // {

        //     if (RBReportType.SelectedItem.Value.ToString() == "Screen")
        //     {
        //         ShowTreeList();
        //     }
        //     else
        //     {
        //         showCrystalReport();
        //     }
        //}




    }
}