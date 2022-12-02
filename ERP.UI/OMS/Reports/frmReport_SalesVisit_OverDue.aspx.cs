using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_SalesVisit_OverDue : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable dt_treeView = new DataTable();
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

            BtnReport_Click();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        public void BtnReport_Click()
        {
            dt_treeView = new DataTable();
            DataColumn dc_id = new DataColumn("ID");
            DataColumn dc_parentId = new DataColumn("ParentID");
            DataColumn dc_Name = new DataColumn("Name1");
            DataColumn dc_sno = new DataColumn("SNo");
            DataColumn dc_visitDatetime = new DataColumn("Visit DateTime");
            DataColumn dc_ActivityType = new DataColumn("ActivityType");
            DataColumn dc_LeadId = new DataColumn("LeadId");
            DataColumn dc_Address = new DataColumn("Address");
            DataColumn dc_PhoneNo = new DataColumn("PhoneNo");
            DataColumn dc_LastOutCome = new DataColumn("LastOutCome");
            DataColumn dc_LastVisit = new DataColumn("Last Visit");
            dt_treeView.Columns.Add(dc_id);
            dt_treeView.Columns.Add(dc_parentId);
            dt_treeView.Columns.Add(dc_Name);
            dt_treeView.Columns.Add(dc_sno);
            dt_treeView.Columns.Add(dc_visitDatetime);
            dt_treeView.Columns.Add(dc_ActivityType);
            dt_treeView.Columns.Add(dc_LeadId);
            dt_treeView.Columns.Add(dc_Address);
            dt_treeView.Columns.Add(dc_PhoneNo);
            dt_treeView.Columns.Add(dc_LastOutCome);
            dt_treeView.Columns.Add(dc_LastVisit);
            DataTable DT = new DataTable();
            string All_User = oDBEngine.getChildUser_for_AllEmployee(Session["userid"].ToString(), "");
            DT = oDBEngine.GetDataTable("tbl_master_employee e,tbl_master_user u", "e.emp_id,u.user_name", " e.emp_contactid=u.user_contactid and u.user_id='" + Session["userid"].ToString() + "'");

            DataRow dr1 = dt_treeView.NewRow();
            dr1["ID"] = DT.Rows[0][0].ToString();
            dr1["ParentID"] = "0";
            dr1["Name1"] = DT.Rows[0][1].ToString();
            dr1["SNo"] = "";
            dr1["Visit DateTime"] = "";
            dr1["ActivityType"] = "";
            dr1["LeadId"] = "";
            dr1["Address"] = "";
            dr1["PhoneNo"] = "";
            dr1["LastOutCome"] = "";
            dr1["Last Visit"] = "";
            dt_treeView.Rows.Add(dr1);
            populatetreeview(DT.Rows[0][0].ToString(), "0");
            GetUserList_call_CalculateEmployeeCurtesyCall(DT.Rows[0][0].ToString());
            TvoOverDue.DataSource = dt_treeView.DefaultView;
            TvoOverDue.DataBind();
        }
        public void populatetreeview(string ParentID, string PId)
        {
            DataTable ParentTable = new DataTable();
            ParentTable = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_lead.cnt_internalId = tbl_master_address.add_cntId INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "convert(varchar(20),tbl_trans_salesVisit.slv_nextvisitdatetime,100) AS VisitDateTime, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') + ' [' + tbl_trans_salesVisit.slv_leadcotactId + ']' AS Name,slv_leadcotactid, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome, convert(varchar(20),tbl_trans_salesVisit.slv_lastdatevisit,100) as slv_lastdatevisit, act_Assignedto,tbl_trans_salesVisit.slv_id as Id", " (CONVERT(datetime, tbl_trans_salesVisit.slv_nextvisitdatetime, 101) < CONVERT(datetime, GETDATE(), 101)) AND (tbl_trans_salesVisit.slv_nextvisitdatetime IS NOT NULL) AND (tbl_trans_salesVisit.slv_nextvisitdatetime <> '') AND (tbl_master_SalesVisitOutCome.slv_Category IN (1, 2, 3, 5, 8, 9, 10)) and  (tbl_trans_Activies.act_assignedTo IN ('" + ParentID + "')) Order by tbl_trans_Activies.act_assignedTo,convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            if (ParentTable.Rows.Count > 0)
            {
                for (int i = 0; i < ParentTable.Rows.Count; i++)
                {
                    DataRow DR_New = dt_treeView.NewRow();
                    string CallType = "";
                    string[,] calltype1 = oDBEngine.GetFieldValue("tbl_trans_salesvisitdetail", "top 1 isnull(slv_nextactivityType,2)", " slv_SalesVisitId='" + ParentTable.Rows[i]["Id"].ToString() + "'", 1);
                    if (calltype1[0, 0] != "n")
                    {
                        CallType = calltype1[0, 0];
                    }
                    if (CallType.ToString() == "")
                    {
                        DR_New["ActivityType"] = "Meetings";
                    }
                    else
                    {
                        if (CallType == "1")
                        {
                            DR_New["ActivityType"] = "Phone Call";
                        }
                        else
                        {
                            DR_New["ActivityType"] = "Meetings";
                        }
                    }
                    string PNo = "";
                    DataTable PhoneNo = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", "phf_cntId='" + ParentTable.Rows[i]["slv_leadcotactid"].ToString() + "'");
                    if (PhoneNo.Rows.Count > 0)
                    {
                        for (int ij = 0; ij < PhoneNo.Rows.Count; ij++)
                        {
                            switch (PhoneNo.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    PNo += "(M)" + PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    PNo += "(R)" + PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    PNo += "(O)" + PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    if (PNo == "")
                    {
                        PNo = "N/A";
                    }
                    DR_New["PhoneNo"] = PNo;
                    DR_New["Address"] = ParentTable.Rows[i]["Address"].ToString();
                    string Type = "";
                    string[,] type1 = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "tbl_master_SalesVisitOutCome.slv_Category", " tbl_trans_salesVisit.slv_id='" + ParentTable.Rows[i]["Id"].ToString() + "'", 1);
                    if (type1[0, 0] != "n")
                    {
                        Type = type1[0, 0];
                    }
                    Int32 a = 0;
                    if (int.TryParse(Type, out a))
                    {
                        if (Convert.ToInt32(Type) > 9)
                        {
                            if (Convert.ToInt32(Type) != 9)
                            {
                                Type = "Phone Call";
                            }
                            else
                            {
                                Type = "";
                            }
                        }
                        else
                        {
                            Type = "Meeting";
                        }
                    }
                    else
                    {
                        Type = "Meetings";
                    }
                    DR_New["LastOutCome"] = Type + " [" + ParentTable.Rows[i]["slv_SalesVisitOutcome"].ToString() + "]";
                    DR_New["Last Visit"] = ParentTable.Rows[i]["slv_lastdatevisit"].ToString();
                    DR_New["Visit DateTime"] = ParentTable.Rows[i]["VisitDateTime"].ToString();
                    DR_New["SNo"] = i + 1;
                    DR_New["LeadId"] = ParentTable.Rows[i]["slv_leadcotactId"].ToString();
                    DR_New["ID"] = ParentTable.Rows[i]["Id"].ToString() + i;
                    DR_New["ParentID"] = ParentID;
                    DR_New["Name1"] = ParentTable.Rows[i]["Name"].ToString();
                    dt_treeView.Rows.Add(DR_New);
                    //populatetreeview(ParentTable.Rows[i]["Id"].ToString());
                }
            }
        }
        private void GetUserList_call_CalculateEmployeeCurtesyCall(string EmployeeID)
        {
            if (EmployeeID != "")
            {

                string[,] userids = oDBEngine.GetFieldValue(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=" + EmployeeID + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' )", 1);
                if (userids[0, 0] != "n")
                {
                    for (int i = 0; i < userids.Length; i++)
                    {
                        if (userids[i, 0] != "")
                        {
                            populatetreeview(userids[i, 0], EmployeeID);
                            GetUserList_call_CalculateEmployeeCurtesyCall(userids[i, 0]);
                        }
                    }
                }
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
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (RBReportType.SelectedItem.Value == "Screen")
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

        }
    }
}