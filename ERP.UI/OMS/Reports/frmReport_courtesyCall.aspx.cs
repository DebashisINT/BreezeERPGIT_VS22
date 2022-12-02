using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class frmReport_courtesyCall : System.Web.UI.Page
    {
        DataTable DT_main = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtFromDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();

                Session["mode"] = "off";
                ReportTable.Visible = false;
                ASPxComboBox1.Visible = false;
            }
            if (Session["mode"].ToString() == "on")
            {
                ShowReport();
                ReportTable.Visible = true;
                ASPxComboBox1.Visible = true; ;
            }

        }
        private void ShowReport()
        {
            Session["mode"] = "on";
            ReportTable.Visible = true;
            ASPxComboBox1.Visible = true; ;
            DT_main = new DataTable();
            //______Table Design______________///
            DataColumn col1 = new DataColumn("User");
            DataColumn col2 = new DataColumn("CrstyCallDate");
            DataColumn col3 = new DataColumn("Tellecaller");
            DataColumn col4 = new DataColumn("LeadName");
            DataColumn col5 = new DataColumn("Phone");
            DataColumn col6 = new DataColumn("Address");
            DataColumn col7 = new DataColumn("AllotedUser");
            DataColumn col8 = new DataColumn("VisitDate");
            DataColumn col9 = new DataColumn("UserRevert");
            DataColumn col10 = new DataColumn("CrtsyCallFeedback");
            DataColumn col11 = new DataColumn("User_ID");
            DataColumn col12 = new DataColumn("User_ParentId");
            DataColumn col13 = new DataColumn("Unique_ID");
            DataColumn col14 = new DataColumn("History");

            DT_main.Columns.Add(col1);
            DT_main.Columns.Add(col2);
            DT_main.Columns.Add(col3);
            DT_main.Columns.Add(col4);
            DT_main.Columns.Add(col5);
            DT_main.Columns.Add(col6);
            DT_main.Columns.Add(col7);
            DT_main.Columns.Add(col8);
            DT_main.Columns.Add(col9);
            DT_main.Columns.Add(col10);
            DT_main.Columns.Add(col11);
            DT_main.Columns.Add(col12);
            DT_main.Columns.Add(col13);
            DT_main.Columns.Add(col14);
            //__________-End Here______//

            //__________calculating Logedin user`s sales activity___________-//
            CalculateEmployeeCurtesyCall(HttpContext.Current.Session["userid"].ToString(), "0");

            //___________Now calling function to Chiled User`s work regarding sales_____//
            GetUserList_call_CalculateEmployeeCurtesyCall(HttpContext.Current.Session["userid"].ToString());

            TLCourtesy.DataSource = DT_main.DefaultView;
            TLCourtesy.DataBind();
        }
        private void CalculateEmployeeCurtesyCall(string EmployeeId, string ReportingHeadId)
        {
            if (EmployeeId != "")
            {
                string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 01:00 AM";
                string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:55 PM";
                string User_Name = "";
                int SlNo = 0;

                DataTable DT = oDBEngine.GetDataTable(" tbl_trans_CourtesyCalls INNER JOIN tbl_master_lead ON tbl_trans_CourtesyCalls.cpc_leadcontactId = tbl_master_lead.cnt_internalId INNER JOIN tbl_master_user ON tbl_trans_CourtesyCalls.CreateUser = tbl_master_user.user_id INNER JOIN tbl_master_CourtesyCallFeedback ON tbl_trans_CourtesyCalls.cpc_feedBack = tbl_master_CourtesyCallFeedback.ccf_id ", " DISTINCT tbl_trans_CourtesyCalls.cpc_leadcontactId AS Id, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, convert(varchar(17),tbl_trans_CourtesyCalls.cpc_callStartTime,113) as cpc_callStartTime, tbl_master_user.user_name, tbl_trans_CourtesyCalls.cpc_note, CASE cpc_outcome WHEN 0 THEN 'N/A' ELSE (SELECT     slv_salesvisitoutcome FROM tbl_master_salesvisitoutcome WHERE      slv_id = cpc_outcome) END AS slv_SalesVisitOutcome ", " (CAST(tbl_trans_CourtesyCalls.createdate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_CourtesyCalls.createdate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_CourtesyCalls.createuser=" + EmployeeId, " tbl_master_user.user_name ");
                if (DT.Rows.Count > 0)
                {
                    DataRow DR = DT_main.NewRow();
                    for (int i = 0; i > DT.Rows.Count; i++)
                    {
                        if (User_Name != DT.Rows[i]["user_name"].ToString())
                        {
                            SlNo = 0;
                            User_Name = DT.Rows[i]["user_name"].ToString();
                            DR = DT_main.NewRow();
                            DR[0] = DT.Rows[i]["user_name"].ToString();
                            DR[1] = "";
                            DR[2] = "";
                            DR[3] = "";
                            DR[4] = "";
                            DR[5] = "";
                            DR[6] = "";
                            DR[7] = "";
                            DR[8] = "";
                            DR[9] = "";
                            DR[10] = EmployeeId;
                            DR[11] = ReportingHeadId;
                            DR[12] = DT.Rows[i]["Id"].ToString();
                            DR[13] = "";
                            DT_main.Rows.Add(DR);
                        }

                        SlNo += 1;
                        DR = DT_main.NewRow();
                        DR[0] = SlNo;
                        DR[1] = DT.Rows[i]["cpc_callStartTime"].ToString();
                        DR[2] = DT.Rows[i]["user_name"].ToString();
                        DR[3] = DT.Rows[i]["Name"].ToString() + "[" + DT.Rows[i]["Id"].ToString() + "]";
                        string PhoneCall = "";
                        DataTable DT_phone = oDBEngine.GetDataTable(" tbl_master_phoneFax ", " * ", " phf_cntId='" + DT.Rows[i]["Id"].ToString() + "'");
                        if (DT_phone.Rows.Count > 0)
                        {
                            for (int j = 0; j < DT_phone.Rows.Count; j++)
                            {
                                switch (DT_phone.Rows[j]["phf_type"].ToString())
                                {
                                    case "MOBILE":
                                        PhoneCall += " (M)" + DT_phone.Rows[j]["phf_phoneNumber"].ToString();
                                        break;
                                    case "RESIDENCE":
                                        PhoneCall += " (R)" + DT_phone.Rows[j]["phf_phoneNumber"].ToString();
                                        break;
                                    case "OFFICIAL":
                                        PhoneCall += " (O)" + DT_phone.Rows[j]["phf_phoneNumber"].ToString();
                                        break;

                                }
                            }
                        }
                        if (PhoneCall == "")
                            PhoneCall = "N/A";

                        DR[4] = PhoneCall;
                        string Address = "";
                        DataTable DT_Add = oDBEngine.GetDataTable(" tbl_master_address ", " * ", " add_cntid='" + DT.Rows[i]["Id"].ToString() + "'");
                        if (DT_Add.Rows.Count > 0)
                        {
                            for (int j = 0; j < DT_Add.Rows.Count; j++)
                            {
                                switch (DT_Add.Rows[j]["add_addressType"].ToString())
                                {
                                    case "RESIDENCE":
                                        Address += " (R)" + DT_Add.Rows[j]["add_address1"].ToString() + " " + DT_Add.Rows[j]["add_address2"].ToString() + " " + DT_Add.Rows[j]["add_address3"].ToString() + " [" + DT_Add.Rows[j]["add_City"].ToString() + " " + DT_Add.Rows[j]["add_pin"].ToString() + "] ";
                                        break;
                                    case "OFFICIAL":
                                        Address += " (O)" + DT_Add.Rows[j]["add_address1"].ToString() + " " + DT_Add.Rows[j]["add_address2"].ToString() + " " + DT_Add.Rows[j]["add_address3"].ToString() + " [" + DT_Add.Rows[j]["add_City"].ToString() + " " + DT_Add.Rows[j]["add_pin"].ToString() + "] ";
                                        break;
                                    case "CORRESPONDENCE":
                                        Address += " (C)" + DT_Add.Rows[j]["add_address1"].ToString() + " " + DT_Add.Rows[j]["add_address2"].ToString() + " " + DT_Add.Rows[j]["add_address3"].ToString() + " [" + DT_Add.Rows[j]["add_City"].ToString() + " " + DT_Add.Rows[j]["add_pin"].ToString() + "] ";
                                        break;

                                }
                            }
                        }
                        if (Address == "")
                            Address = "N/A";

                        DR[5] = Address;
                        DR[6] = oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id ", " user_name ", " slv_leadcotactid=" + DT.Rows[i]["Id"].ToString(), 1)[0, 0];
                        DR[7] = oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_SalesVisitDetail ON tbl_trans_salesVisit.slv_id = tbl_trans_SalesVisitDetail.slv_SalesVisitId INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " tbl_trans_SalesVisitDetail.createdate ", " tbl_trans_salesvisit.slv_leadcotactid=" + DT.Rows[i]["Id"].ToString(), 1)[0, 0];
                        DR[8] = oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_SalesVisitDetail ON tbl_trans_salesVisit.slv_id = tbl_trans_SalesVisitDetail.slv_SalesVisitId INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome + ' [' + tbl_trans_salesvisitdetail.slv_notes + ']' as Outcome ", " tbl_trans_salesvisit.slv_leadcotactid=" + DT.Rows[i]["Id"].ToString(), 1)[0, 0];
                        DR[9] = DT.Rows[i]["slv_SalesVisitOutcome"].ToString() + " [" + DT.Rows[i]["cpc_note"].ToString() + "]";
                        DR[10] = EmployeeId;
                        DR[11] = ReportingHeadId;
                        DR[12] = DT.Rows[i]["Id"].ToString();
                        DR[13] = "<div style='CURSOR: hand;color:#330099;' onclick='frmOpenNewWindow1(../management/frmShowHistory_phoneCall.aspx?id=" + DT.Rows[i]["Id"].ToString() + ",300,800)'>History</div>";
                        DT_main.Rows.Add(DR);
                    }
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
                            CalculateEmployeeCurtesyCall(userids[i, 0], "0");
                            GetUserList_call_CalculateEmployeeCurtesyCall(userids[i, 0]);
                        }
                    }
                }
            }
        }
        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            ShowReport();
        }
        protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ASPxComboBox1.Value.ToString() != "")
            {
                //PrepareExporter();
                ASPxTreeListExporter1.Settings.PageSettings.Landscape = true;
                ASPxTreeListExporter1.Settings.PageSettings.Margins.Left = 2;
                ASPxTreeListExporter1.Settings.PageSettings.Margins.Right = 2;
                ASPxTreeListExporter1.TreeList.Font.Size = FontUnit.Parse("10px");
                if (ASPxComboBox1.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (ASPxComboBox1.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (ASPxComboBox1.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();
            }
        }
    }
}