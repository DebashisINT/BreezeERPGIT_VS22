using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmreport_daily : System.Web.UI.Page
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
                TxtStartDate.EditFormatString = OConvert.GetDateFormat("Date");
                LoadTreeView();
                All.Attributes.Add("onclick", "All_CheckedChanged();");
                Specific.Attributes.Add("onclick", "Specific_CheckedChanged();");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>page_load()</script>");
                TxtStartDate.Value = oDBEngine.GetDate().ToShortDateString();
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
                GridDailyActivity.Visible = true;
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TrTreeExport').style.display = 'inline';document.getElementById('TdTree').style.display = 'none';document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'inline';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                BindAll();
            }
            else
            {
                try
                {
                    GridDailyActivity.Visible = true;
                    int UId = id.LastIndexOf(',');
                    id = id.Substring(0, UId);
                    populategrid(id);
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
            DataColumn dc_TotalCalls = new DataColumn("Total Calls");
            DataColumn dc_CallBack = new DataColumn("Call Back");
            DataColumn dc_NonContactable = new DataColumn("Non Contactable");
            DataColumn dc_Nonusable = new DataColumn("Non Usable");
            DataColumn dc_LostCalls = new DataColumn("Lost Calls");
            DataColumn dc_ConfirmSale = new DataColumn("Confirm Sale");
            DataColumn dc_SalesVisit = new DataColumn("Sales Visit");
            DataColumn dc_PendingCalls = new DataColumn("Pending Calls");
            DataColumn dc_Performance = new DataColumn("Performance(%)");
            dt_treeView.Columns.Add(dc_ID);
            dt_treeView.Columns.Add(dc_ParentID);
            dt_treeView.Columns.Add(dc_AssignTo);
            dt_treeView.Columns.Add(dc_TotalCalls);
            dt_treeView.Columns.Add(dc_CallBack);
            dt_treeView.Columns.Add(dc_NonContactable);
            dt_treeView.Columns.Add(dc_Nonusable);
            dt_treeView.Columns.Add(dc_LostCalls);
            dt_treeView.Columns.Add(dc_ConfirmSale);
            dt_treeView.Columns.Add(dc_SalesVisit);
            dt_treeView.Columns.Add(dc_PendingCalls);
            dt_treeView.Columns.Add(dc_Performance);
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
            if (EmployeeId != "")
            {
                string Date = Convert.ToString(TxtStartDate.Value);
                DataTable DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId) as Pending,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='1' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)))  as CallBack,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='2' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as NonContacTable,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='3' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as NonUsable,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='6' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as LostCalls,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='5' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as ConfirmSale,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='9' and CreateUser='" + EmployeeId + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as SalesVisit,(select user_name from tbl_master_user where user_id='" + EmployeeId + "') as user_name", "tbl_trans_Activies.act_assignedTo = '" + EmployeeId + "' AND (phc_callDispose=11)");
                if (DT.Rows.Count != 0)
                {
                    DataRow DR = dt_treeView.NewRow();
                    DR["Pending Calls"] = DT.Rows[0]["Pending"].ToString();
                    string Pending = DT.Rows[0]["Pending"].ToString();
                    DR["Assign To"] = DT.Rows[0]["user_name"].ToString();
                    DR["Call Back"] = DT.Rows[0]["CallBack"].ToString();
                    string CallBack = DT.Rows[0]["CallBack"].ToString();
                    DR["Non Contactable"] = DT.Rows[0]["NonContacTable"].ToString();
                    string NonContactTable = DT.Rows[0]["NonContacTable"].ToString();
                    DR["Non Usable"] = DT.Rows[0]["NonUsable"].ToString();
                    string NonUsable = DT.Rows[0]["NonUsable"].ToString();
                    DR["Lost Calls"] = DT.Rows[0]["LostCalls"].ToString();
                    string LostCalls = DT.Rows[0]["LostCalls"].ToString();
                    DR["Confirm Sale"] = DT.Rows[0]["ConfirmSale"].ToString();
                    string ConfirmSale = DT.Rows[0]["ConfirmSale"].ToString();
                    DR["Sales Visit"] = DT.Rows[0]["SalesVisit"].ToString();
                    string SalesVisit = DT.Rows[0]["SalesVisit"].ToString();
                    int TotalCalls = Convert.ToInt32(Pending) + Convert.ToInt32(CallBack) + Convert.ToInt32(NonContactTable) + Convert.ToInt32(NonUsable) + Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit) + Convert.ToInt32(LostCalls);
                    DR["Total Calls"] = TotalCalls.ToString();
                    float Confirm_Sales = Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit);
                    float Confirm_Sales_CallBack = Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit) + Convert.ToInt32(CallBack);
                    float aa = Confirm_Sales / Confirm_Sales_CallBack;
                    float Performance = 100 * (Confirm_Sales / Confirm_Sales_CallBack);
                    if (Performance.ToString() == "NaN")
                    {
                        Performance = 0;
                    }
                    DR["Performance(%)"] = Performance.ToString();
                    DR["ID"] = EmployeeId;
                    DR["ParentID"] = ReportingHeadId;
                    if (TotalCalls != 0)
                    {
                        dt_treeView.Rows.Add(DR);
                    }
                }
            }
        }
        private void populategrid(string EmployeeId)
        {
            DataTable DT1 = new DataTable();
            DataColumn dc_AssignTo = new DataColumn("Assign To");
            DataColumn dc_TotalCalls = new DataColumn("Total Calls");
            DataColumn dc_CallBack = new DataColumn("Call Back");
            DataColumn dc_NonContactable = new DataColumn("Non Contactable");
            DataColumn dc_Nonusable = new DataColumn("Non Usable");
            DataColumn dc_LostCalls = new DataColumn("Lost Calls");
            DataColumn dc_ConfirmSale = new DataColumn("Confirm Sale");
            DataColumn dc_SalesVisit = new DataColumn("Sales Visit");
            DataColumn dc_PendingCalls = new DataColumn("Pending Calls");
            DataColumn dc_Performance = new DataColumn("Performance(%)");
            DT1.Columns.Add(dc_AssignTo);
            DT1.Columns.Add(dc_TotalCalls);
            DT1.Columns.Add(dc_CallBack);
            DT1.Columns.Add(dc_NonContactable);
            DT1.Columns.Add(dc_Nonusable);
            DT1.Columns.Add(dc_LostCalls);
            DT1.Columns.Add(dc_ConfirmSale);
            DT1.Columns.Add(dc_SalesVisit);
            DT1.Columns.Add(dc_PendingCalls);
            DT1.Columns.Add(dc_Performance);
            string Date = TxtStartDate.Value.ToString();
            DataTable DT = new DataTable();
            string[] EmpId = EmployeeId.Split(',');
            for (int i = 0; i < EmpId.Length; i++)
            {
                DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId) as Pending,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='1' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101)))  as CallBack,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='2' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as NonContacTable,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='3' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as NonUsable,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='6' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as LostCalls,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='5' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as ConfirmSale,(select count(phd_callDispose) from tbl_trans_phonecalldetails where phd_callDispose='9' and CreateUser='" + EmpId[i] + "' and (CONVERT(varchar, CAST(CreateDate AS datetime), 101) = CONVERT(varchar,cast('" + Date + "' as datetime), 101))) as SalesVisit,(select user_name from tbl_master_user where user_id='" + EmpId[i] + "') as user_name", "tbl_trans_Activies.act_assignedTo = '" + EmpId[i] + "' AND (phc_callDispose=11)");
                DataRow DR = DT1.NewRow();
                DR["Pending Calls"] = DT.Rows[0]["Pending"].ToString();
                string Pending = DT.Rows[0]["Pending"].ToString();
                DR["Assign To"] = DT.Rows[0]["user_name"].ToString();
                DR["Call Back"] = DT.Rows[0]["CallBack"].ToString();
                string CallBack = DT.Rows[0]["CallBack"].ToString();
                DR["Non Contactable"] = DT.Rows[0]["NonContacTable"].ToString();
                string NonContactTable = DT.Rows[0]["NonContacTable"].ToString();
                DR["Non Usable"] = DT.Rows[0]["NonUsable"].ToString();
                string NonUsable = DT.Rows[0]["NonUsable"].ToString();
                DR["Lost Calls"] = DT.Rows[0]["LostCalls"].ToString();
                string LostCalls = DT.Rows[0]["LostCalls"].ToString();
                DR["Confirm Sale"] = DT.Rows[0]["ConfirmSale"].ToString();
                string ConfirmSale = DT.Rows[0]["ConfirmSale"].ToString();
                DR["Sales Visit"] = DT.Rows[0]["SalesVisit"].ToString();
                string SalesVisit = DT.Rows[0]["SalesVisit"].ToString();
                int TotalCalls = Convert.ToInt32(Pending) + Convert.ToInt32(CallBack) + Convert.ToInt32(NonContactTable) + Convert.ToInt32(NonUsable) + Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit) + Convert.ToInt32(LostCalls);
                DR["Total Calls"] = TotalCalls.ToString();
                float Confirm_Sales = Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit);
                float Confirm_Sales_CallBack = Convert.ToInt32(ConfirmSale) + Convert.ToInt32(SalesVisit) + Convert.ToInt32(CallBack);
                float aa = Confirm_Sales / Confirm_Sales_CallBack;
                float Performance = 100 * (Confirm_Sales / Confirm_Sales_CallBack);
                if (Performance.ToString() == "NaN")
                {
                    Performance = 0;
                }
                DR["Performance(%)"] = Performance.ToString();
                DT1.Rows.Add(DR);
            }
            if (DT1.Rows.Count != 0)
            {
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TrExpndCollapse').style.display = 'none';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                GridDailyActivity.DataSource = DT1.DefaultView;
                GridDailyActivity.DataBind();
            }
            else
            {
                string popupScript = "";
                popupScript = "<script language='javascript'>document.getElementById('TdGrid').style.display = 'inline';document.getElementById('TrTreeExport').style.display = 'none';document.getElementById('TdExport').style.display = 'inline';document.getElementById('TdExpand').style.display = 'none';document.getElementById('TdCollapse').style.display = 'none';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
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