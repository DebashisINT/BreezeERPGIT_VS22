using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_TelePerformance_Target : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataTable DtUserid = new DataTable();
        DataTable dt_treeView = new DataTable();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
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
                LoadTreeView();
                All.Attributes.Add("onclick", "All_CheckedChanged();");
                Specific.Attributes.Add("onclick", "Specific_CheckedChanged();");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>page_load()</script>");
                TxtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                TxtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                TxtFromDate.Value = oDBEngine.GetDate();
                TxtToDate.Value = oDBEngine.GetDate();
            }
            BindAll();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
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
        public void ShowTreeList()
        {
            int parent_id = -1;
            string id = "";
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
                GridActivitytarget.Visible = true;
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TrTreeExport').style.display = 'inline';document.getElementById('TdTree').style.display = 'none';document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'inline';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                BindAll();
            }
            else
            {
                try
                {
                    GridActivitytarget.Visible = true;
                    int UId = id.LastIndexOf(',');
                    id = id.Substring(0, UId);
                    populategrid(id);
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please Select Any User')</script>");
                    GridActivitytarget.Visible = false;
                }
            }
        }
        public void BindAll()
        {
            dt_treeView = new DataTable();
            DataColumn dc_ID = new DataColumn("ID");
            DataColumn dc_ParentID = new DataColumn("ParentID");
            DataColumn dc_AssignTo = new DataColumn("Assign To");
            DataColumn dc_TotalCalls = new DataColumn("Total Calls");
            DataColumn dc_Assigned = new DataColumn("Assigned Calls");
            DataColumn dc_Activity = new DataColumn("Activity Calls");
            DataColumn dc_Performance = new DataColumn("Performance Vs Target(%)");
            dt_treeView.Columns.Add(dc_ID);
            dt_treeView.Columns.Add(dc_ParentID);
            dt_treeView.Columns.Add(dc_AssignTo);
            dt_treeView.Columns.Add(dc_TotalCalls);
            dt_treeView.Columns.Add(dc_Assigned);
            dt_treeView.Columns.Add(dc_Activity);
            dt_treeView.Columns.Add(dc_Performance);
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_employee e,tbl_master_user u", "e.emp_id,u.user_name", " e.emp_contactid=u.user_contactid and u.user_id='" + Session["userid"].ToString() + "'");
            populatetreeGrid(DT.Rows[0][0].ToString(), "0");
            GetUserList_call_CalculateEmployeeDailyCall(DT.Rows[0][0].ToString());
            GridActivitytarget.DataSource = dt_treeView.DefaultView;
            GridActivitytarget.DataBind();
        }
        private void GetUserList_call_CalculateEmployeeDailyCall(string EmployeeID)
        {
            if (EmployeeID != "")
            {

                string[,] userids = oDBEngine.GetFieldValue(" tbl_master_employee e,tbl_master_contact c ", " e.emp_id,(c.cnt_firstname + ' '+c.cnt_middlename+' '+c.cnt_lastname) as name ", " e.emp_contactid in (select tbl_master_employee.emp_contactid from tbl_trans_employeeCTC,tbl_master_employee where tbl_trans_employeeCTC.emp_reportTo = '" + EmployeeID + "' and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900')) and e.emp_contactid=c.cnt_internalid", 2);
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
            try
            {
                if (EmployeeId != "")
                {
                    DataTable DT = oDBEngine.GetDataTable("tbl_targetschemes t,tbl_targetsetup ts,tbl_master_user tu", "t.tgt_newcalls as NewCalls,t.tgt_salesvisit_ratio_call as sales ,(select count(distinct pcd.phd_phonecallid) from tbl_trans_phonecalldetails pcd,tbl_trans_phonecall p where p.phc_id=pcd.phd_phonecallid and p.phc_allotuser='" + EmployeeId + "' and pcd.phd_callstart between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as atemptPhoneCall,(select count(distinct pcd.phd_phonecallid) from tbl_trans_phonecalldetails pcd,tbl_trans_phonecall p where pcd.phd_calldispose='9' and p.phc_id=pcd.phd_phonecallid and p.phc_allotuser='" + EmployeeId + "' and pcd.phd_callstart between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as atemptCall,(select user_name from tbl_master_user where user_id='" + EmployeeId + "') as UserName", "ts.tgtset_scheme=t.tgt_id and ts.tgtset_entity=tu.user_contactid and tu.user_id='" + EmployeeId + "'");
                    if (DT.Rows.Count != 0 && DT.Rows[0]["atemptPhoneCall"].ToString() != "0")
                    {
                        DataRow DR = dt_treeView.NewRow();
                        DR["Total Calls"] = DT.Rows[0]["atemptPhoneCall"].ToString();
                        string TotalAttemptCall = DT.Rows[0]["atemptPhoneCall"].ToString();
                        DR["Activity Calls"] = DT.Rows[0]["atemptCall"].ToString();
                        string attempedcall = DT.Rows[0]["atemptCall"].ToString();
                        DR["Assigned Calls"] = DT.Rows[0]["NewCalls"].ToString();
                        string AssignCall = DT.Rows[0]["NewCalls"].ToString();
                        string percent = DT.Rows[0]["sales"].ToString();
                        DR["Assign To"] = DT.Rows[0]["UserName"].ToString();
                        float Performance1 = (Convert.ToInt32(AssignCall) / Convert.ToInt32(percent));
                        float Performance = 100 * (Convert.ToInt32(attempedcall) / Performance1);
                        if (Performance.ToString() == "NaN")
                        {
                            Performance = 0;
                        }
                        DR["Performance Vs Target(%)"] = Performance.ToString();
                        DR["ID"] = EmployeeId;
                        DR["ParentID"] = ReportingHeadId;
                        dt_treeView.Rows.Add(DR);

                    }
                }
            }
            catch
            {
            }
        }
        private void populategrid(string EmployeeId)
        {
            DataTable DT1 = new DataTable();
            DataColumn dc_AssignTo = new DataColumn("Assign To");
            DataColumn dc_TotalCalls = new DataColumn("Total Calls");
            DataColumn dc_Assigned = new DataColumn("Assigned Calls");
            DataColumn dc_Activity = new DataColumn("Activity Calls");
            DataColumn dc_Performance = new DataColumn("Performance Vs Target(%)");
            DT1.Columns.Add(dc_AssignTo);
            DT1.Columns.Add(dc_TotalCalls);
            DT1.Columns.Add(dc_Assigned);
            DT1.Columns.Add(dc_Activity);
            DT1.Columns.Add(dc_Performance);
            DataTable DT = new DataTable();
            string[] EmpId = EmployeeId.Split(',');
            for (int i = 0; i < EmpId.Length; i++)
            {
                DT = oDBEngine.GetDataTable("tbl_targetschemes t,tbl_targetsetup ts,tbl_master_user tu", "t.tgt_newcalls as NewCalls,t.tgt_salesvisit_ratio_call as sales ,(select count(distinct pcd.phd_phonecallid) from tbl_trans_phonecalldetails pcd,tbl_trans_phonecall p where p.phc_id=pcd.phd_phonecallid and p.phc_allotuser='" + EmpId[i] + "' and pcd.phd_callstart between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as atemptPhoneCall,(select count(distinct pcd.phd_phonecallid) from tbl_trans_phonecalldetails pcd,tbl_trans_phonecall p where pcd.phd_calldispose='9' and p.phc_id=pcd.phd_phonecallid and p.phc_allotuser='" + EmpId[i] + "' and pcd.phd_callstart between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as atemptCall,(select user_name from tbl_master_user where user_id='" + EmpId[i] + "') as UserName", "ts.tgtset_scheme=t.tgt_id and ts.tgtset_entity=tu.user_contactid and tu.user_id='" + EmpId[i] + "'");
                if (DT.Rows.Count != 0 && DT.Rows[0]["atemptPhoneCall"].ToString() != "0")
                {
                    DataRow DR = DT1.NewRow();
                    DR["Total Calls"] = DT.Rows[0]["atemptPhoneCall"].ToString();
                    string TotalAttemptCall = DT.Rows[0]["atemptPhoneCall"].ToString();
                    DR["Activity Calls"] = DT.Rows[0]["atemptCall"].ToString();
                    string attempedcall = DT.Rows[0]["atemptCall"].ToString();
                    DR["Assigned Calls"] = DT.Rows[0]["NewCalls"].ToString();
                    string AssignCall = DT.Rows[0]["NewCalls"].ToString();
                    string percent = DT.Rows[0]["sales"].ToString();
                    DR["Assign To"] = DT.Rows[0]["UserName"].ToString();
                    float Performance1 = (Convert.ToInt32(AssignCall) / Convert.ToInt32(percent));
                    float Performance = 100 * (Convert.ToInt32(attempedcall) / Performance1);
                    if (Performance.ToString() == "NaN")
                    {
                        Performance = 0;
                    }
                    DR["Performance Vs Target(%)"] = Performance.ToString();
                    DT1.Rows.Add(DR);
                }
            }
            if (DT1.Rows.Count != 0)
            {
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'none';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                GridActivitytarget.DataSource = DT1.DefaultView;
                GridActivitytarget.DataBind();
            }
            else
            {
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TdExpand').style.display = 'none';document.getElementById('TdCollapse').style.display = 'none';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                GridActivitytarget.DataSource = DT1.DefaultView;
                GridActivitytarget.DataBind();
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExport.Value.ToString() != "")
            {
                if (cmbExport.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (cmbExport.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (cmbExport.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();
            }
        }
        public void showCrystalReport()
        {

        }
    }
}