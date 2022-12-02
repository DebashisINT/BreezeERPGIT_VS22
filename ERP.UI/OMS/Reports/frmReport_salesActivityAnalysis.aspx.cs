using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_salesActivityAnalysis : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                TxtStartDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtEndDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtStartDate.Value = oDBEngine.GetDate();
                TxtEndDate.Value = oDBEngine.GetDate();


            }
            BtnReport_Click(sender, e);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void BtnReport_Click(object sender, EventArgs e)
        {
            dt_treeView = new DataTable();
            DataColumn dc_id = new DataColumn("ID");
            DataColumn dc_parentId = new DataColumn("ParentID");
            DataColumn dc_sno = new DataColumn("SNo");
            DataColumn dc_LeadName = new DataColumn("LeadName");
            DataColumn dc_PhoneNo = new DataColumn("PhoneNo");
            DataColumn dc_Visited = new DataColumn("Visited");
            DataColumn dc_PhoneFollowUp = new DataColumn("Phone FollowUp");
            DataColumn dc_Outcome = new DataColumn("Outcome");
            DataColumn dc_LeadId = new DataColumn("LeadId");
            //DataColumn dc_History = new DataColumn("History");
            dt_treeView.Columns.Add(dc_id);
            dt_treeView.Columns.Add(dc_parentId);
            dt_treeView.Columns.Add(dc_sno);
            dt_treeView.Columns.Add(dc_LeadName);
            dt_treeView.Columns.Add(dc_PhoneNo);
            dt_treeView.Columns.Add(dc_Visited);
            dt_treeView.Columns.Add(dc_PhoneFollowUp);
            dt_treeView.Columns.Add(dc_Outcome);
            dt_treeView.Columns.Add(dc_LeadId);
            //dt_treeView.Columns.Add(dc_History);
            DataTable DT = new DataTable();
            string All_User = oDBEngine.getChildUser_for_AllEmployee(Session["userid"].ToString(), "");
            DT = oDBEngine.GetDataTable("tbl_master_employee e,tbl_master_user u", "e.emp_id,u.user_name", " e.emp_contactid=u.user_contactid and u.user_id='" + Session["userid"].ToString() + "'");

            DataRow dr1 = dt_treeView.NewRow();
            dr1["ID"] = DT.Rows[0][0].ToString();
            dr1["ParentID"] = "0";
            dr1["SNo"] = "";
            dr1["LeadName"] = DT.Rows[0][1].ToString();
            dr1["PhoneNo"] = "";
            dr1["Visited"] = "";
            dr1["Phone FollowUp"] = "";
            dr1["Outcome"] = "";
            dr1["LeadId"] = "";
            //dr1["History"] = "";
            dt_treeView.Rows.Add(dr1);
            populatetreeview(DT.Rows[0][0].ToString(), "0");
            GetUserList_call_CalculateEmployeeCurtesyCall(DT.Rows[0][0].ToString());
            TvSalesActivityAnalyst.DataSource = dt_treeView.DefaultView;
            TvSalesActivityAnalyst.DataBind();
        }
        public void populatetreeview(string ParentID, string PId)
        {
            if (TxtStartDate.Value != null)
                ViewState["startDate"] = TxtStartDate.Value.ToString();
            else
                ViewState["startDate"] = "";
            if (TxtEndDate.Value != null)
                ViewState["enddate"] = TxtEndDate.Value.ToString();
            else
                ViewState["enddate"] = "";
            if (ViewState["startDate"].ToString() == "")
            {
                //CollapseAll.Visible = false;
                GridTree.Visible = false;
            }
            else
            {
                //CollapseAll.Visible = false;
                GridTree.Visible = true;
            }
            DataTable ParentTable = new DataTable();
            ParentTable = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome AS Outcome, tbl_trans_salesVisit.slv_leadcotactId AS CNTID,(SELECT COUNT(*) AS Expr1 FROM tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id WHERE tbl_master_SalesVisitOutCome.slv_category IN (1, 2, 3, 4, 5, 6, 7, 8) AND tbl_trans_salesvisitdetail.slv_salesvisitid = tbl_trans_salesvisit.slv_id) AS Totalvisit,(SELECT COUNT(*) AS Expr1 FROM tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id WHERE tbl_master_SalesVisitOutCome.slv_category IN (9, 10, 11, 12, 13) AND tbl_trans_salesvisitdetail.slv_salesvisitid = tbl_trans_salesvisit.slv_id) AS Phonecall,tbl_trans_Activies.act_assignedTo ", " (CAST(tbl_trans_salesvisit.createdate AS datetime) >= CONVERT(varchar,'" + ViewState["startDate"].ToString() + "', 101)) and (CAST(tbl_trans_salesvisit.createdate AS datetime) <= CONVERT(varchar,'" + ViewState["enddate"] + "', 101))  and tbl_trans_Activies.act_assignedTo in (" + ParentID + ") Order by tbl_trans_Activies.act_assignedTo");
            if (ParentTable.Rows.Count > 0)
            {
                for (int i = 0; i < ParentTable.Rows.Count; i++)
                {
                    DataRow DR_New = dt_treeView.NewRow();
                    string PNo = "";
                    DataTable PhoneNo = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", "phf_cntId='" + ParentTable.Rows[i]["CNTID"].ToString() + "'");
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
                    string Name = "";
                    string[,] name1 = oDBEngine.GetFieldValue("tbl_master_lead", "isnull(cnt_firstname,'')  + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", " cnt_internalid='" + ParentTable.Rows[i]["CNTID"].ToString() + "'", 1);
                    if (name1[0, 0] != "n")
                    {
                        Name = name1[0, 0];
                    }
                    DR_New["LeadName"] = Name + "[" + ParentTable.Rows[i]["CNTID"].ToString() + "]";
                    DR_New["Visited"] = ParentTable.Rows[i]["TotalVisit"].ToString();
                    DR_New["Phone FollowUp"] = ParentTable.Rows[i]["Phonecall"].ToString();
                    DR_New["SNo"] = i + 1;
                    DR_New["Outcome"] = ParentTable.Rows[i]["Outcome"].ToString();
                    DR_New["ID"] = ParentTable.Rows[i]["CNTID"].ToString() + i;
                    DR_New["LeadId"] = ParentTable.Rows[i]["CNTID"].ToString();
                    DR_New["ParentID"] = ParentID;
                    //DR_New["History"] = "<div style='CURSOR: hand;color:#330099;' onclick='frmOpenNewWindow1(../management/frmShowHistory_phoneCall.aspx?id=" + ParentTable.Rows[i]["CNTID"].ToString() + ",300,800)'>History</div>";
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
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbExport.Value.ToString() != "")
        //    {
        //        if (cmbExport.Value.ToString() == "Pdf")
        //            ASPxTreeListExporter1.WritePdfToResponse();
        //        if (cmbExport.Value.ToString() == "Xls")
        //            ASPxTreeListExporter1.WriteXlsToResponse();
        //        if (cmbExport.Value.ToString() == "Rtf")
        //            ASPxTreeListExporter1.WriteRtfToResponse();
        //    }
        //}
    }
}