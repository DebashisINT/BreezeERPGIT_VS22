using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmperformanceReport_salesvisit : System.Web.UI.Page
    {
        DataTable DT_main = new DataTable();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
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
              //  Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtFromDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();
                //txtFromDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgFromDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtFromDate,true,null,'0','0')");
                //txtToDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgToDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtToDate,true,null,'0','0')");
                //txtFromDate.Attributes.Add("readonly", "true");
                //txtToDate.Attributes.Add("readonly", "true");
                Session["mode"] = "off";
                ASPxComboBox1.Visible = false;
                TLLead.Visible = false;
            }
            if (Session["mode"].ToString() == "on")
                ShowReport();
         //   //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void ShowReport()
        {
            Session["mode"] = "on";
            ASPxComboBox1.Visible = true;
            TLLead.Visible = true;
            DT_main = new DataTable();
            //______Table Design______________///
            DataColumn col1 = new DataColumn("User");
            DataColumn col2 = new DataColumn("Alotd");
            DataColumn col3 = new DataColumn("Contctd");
            DataColumn col4 = new DataColumn("VisitsT");
            DataColumn col5 = new DataColumn("VisitsU");
            DataColumn col6 = new DataColumn("CallsT");
            DataColumn col15 = new DataColumn("CallsU");
            DataColumn col7 = new DataColumn("Pending");
            DataColumn col8 = new DataColumn("Refix");
            DataColumn col9 = new DataColumn("CallBack");
            DataColumn col10 = new DataColumn("Convrtd");
            DataColumn col11 = new DataColumn("Lost");
            DataColumn col16 = new DataColumn("NonContactble");
            DataColumn col12 = new DataColumn("Nonusable/Fake");
            DataColumn col13 = new DataColumn("Contblty");
            DataColumn col14 = new DataColumn("Conv");


            DataColumn col17 = new DataColumn("User_ID");
            DataColumn col18 = new DataColumn("User_ParentId");

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
            DT_main.Columns.Add(col15);
            DT_main.Columns.Add(col16);
            DT_main.Columns.Add(col17);
            DT_main.Columns.Add(col18);
            //__________-End Here______//

            //__________calculating Logedin user`s sales activity___________-//
            CalculateEmployeeSales(HttpContext.Current.Session["userid"].ToString(), "0");

            //___________Now calling function to Chiled User`s work regarding sales_____//
            GetUserList_call_CalculateEmployeeSales(HttpContext.Current.Session["userid"].ToString());

            TLLead.DataSource = DT_main.DefaultView;
            TLLead.DataBind();
        }

        private void GetUserList_call_CalculateEmployeeSales(string EmployeeID)
        {
            if (EmployeeID != "")
            {
                //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string temp = "";
                string[] userids = oDBEngine.getUser(EmployeeID, temp).Split(',');
                for (int i = 0; i < userids.Length; i++)
                {
                    if (userids[i] != "")
                    {
                        CalculateEmployeeSales(userids[i], EmployeeID);
                        GetUserList_call_CalculateEmployeeSales(userids[i]);
                    }
                }
                //if (userids != "")
                //{
                //    string[] UserID = userids.Split(',');
                //    for (int i = 0; i < UserID.Length; i++)
                //    {
                //        if (i == 0)
                //        {
                //            CalculateEmployeeSales(UserID[i], EmployeeID);
                //            GetUserList_call_CalculateEmployeeSales(UserID[i]);
                //        }
                //        else
                //        {
                //            //CalculateEmployeeSales(UserID[i], UserID[i - 1]);
                //            GetUserList_call_CalculateEmployeeSales(UserID[i]);
                //        }
                //    }
                //}
            }
        }

        private void CalculateEmployeeSales(string EmployeeId, string ReportingHeadId)
        {
            if (EmployeeId != "")
            {
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string usercondition = "";
                usercondition = " and tbl_trans_salesvisitdetail.createuser=" + EmployeeId;
                // Converter oConverter = new Converter();
                string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 01:00 AM";
                string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:55 PM";

                DataTable DT = oDBEngine.GetDataTable(" tbl_trans_SalesVisitDetail ", " DISTINCT slv_SalesVisitId, CreateUser ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101))" + usercondition, " tbl_trans_salesvisitdetail.createuser, slv_SalesVisitId ");
                if (DT.Rows.Count > 0)
                {
                    DataRow DR = DT_main.NewRow();
                    int allotedvisit = 0;
                    int userid = 0;
                    int totalcall = 0;
                    int visit = 0;

                    int phonefollowup = 0;
                    int refixedbyuser = 0;
                    int refixedbylead = 0;
                    int callback = 0;
                    int totalsales = 0;
                    int notinterested = 0;
                    int noncontactable = 0;
                    int nonusable = 0;

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (userid != int.Parse(DT.Rows[i][1].ToString()))
                        {
                            if (i == 0)
                            {
                                DR = DT_main.NewRow();
                            }
                            else
                            {
                                DR[0] = oDBEngine.GetFieldValue(" tbl_master_user ", " user_name ", " user_id='" + DT.Rows[0]["CreateUser"].ToString() + "'", 1)[0, 0].ToString();
                                string Calculation = (oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id ", " COUNT(*) AS Expr1 ", " (CAST(tbl_trans_activies.createdate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_activies.createdate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_Activies.act_assignedTo = " + DT.Rows[0]["CreateUser"].ToString(), 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[1] = Calculation;
                                else
                                    DR[1] = "0";
                                DR[2] = totalcall;
                                Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(*) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (1,2,3,4,5,6,7,8)", 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[3] = Calculation;
                                else
                                    DR[3] = "0";
                                Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(distinct slv_salesvisitid) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (1,2,3,4,5,6,7,8)", 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[4] = Calculation;
                                else
                                    DR[4] = "0";
                                Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(*) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (9,10,11,12,13)", 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[5] = Calculation;
                                else
                                    DR[5] = "0";
                                Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", "distinct count(distinct slv_salesvisitid) as Id", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (9,10,11,12,13)", 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[6] = Calculation;
                                else
                                    DR[6] = "0";
                                Calculation = (oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id ", " COUNT(*) AS Expr1 ", " (CAST(tbl_trans_activies.createdate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_activies.createdate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_Activies.act_assignedTo = " + DT.Rows[0]["CreateUser"].ToString() + " and (tbl_trans_salesVisit.slv_id NOT IN (SELECT DISTINCT slv_SalesVisitId FROM tbl_trans_SalesVisitDetail WHERE  (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and createuser = " + DT.Rows[0]["CreateUser"].ToString() + "))", 1)[0, 0].ToString());
                                if (Calculation != "n")
                                    DR[7] = Calculation;
                                else
                                    DR[7] = "0";
                                DR[8] = refixedbyuser + refixedbylead;
                                DR[9] = callback;
                                DR[10] = totalsales;
                                DR[11] = notinterested;
                                DR[12] = noncontactable;
                                DR[13] = nonusable;
                                try
                                {
                                    DR[14] = (int)(((totalcall - (noncontactable + nonusable)) * 100) / totalcall);
                                    DR[15] = string.Format("{0:f}", ((totalsales * 100) / (totalcall - noncontactable)));
                                }
                                catch (Exception ex)
                                {
                                    DR[14] = 0;
                                    DR[15] = 0;
                                }
                                DR[16] = EmployeeId;
                                DR[17] = ReportingHeadId;

                                DT_main.Rows.Add(DR);

                                DR = DT_main.NewRow();

                                visit = 0;
                                phonefollowup = 0;
                                totalcall = 0;
                                refixedbyuser = 0;
                                refixedbylead = 0;
                                callback = 0;
                                totalsales = 0;
                                notinterested = 0;
                                noncontactable = 0;
                                nonusable = 0;
                            }
                        }
                        userid = int.Parse(DT.Rows[i][1].ToString());
                        DataTable DT_salesVisit = oDBEngine.GetDataTable(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " TOP 1 tbl_master_SalesVisitOutCome.slv_Category AS Category ", " tbl_trans_SalesVisitDetail.slv_SalesVisitId = " + DT.Rows[0]["slv_SalesVisitId"].ToString(), " tbl_trans_SalesVisitDetail.slv_id desc ");
                        if (DT_salesVisit.Rows.Count > 0)
                        {
                            switch (DT_salesVisit.Rows[0]["Category"].ToString())
                            {
                                case "1":
                                    totalcall += 1;
                                    refixedbyuser += 1;
                                    visit += 1;
                                    break;
                                case "2":
                                    totalcall += 1;
                                    refixedbyuser += 1;
                                    visit += 1;
                                    break;
                                case "3":
                                    totalcall += 1;
                                    refixedbylead += 1;
                                    visit += 1;
                                    break;
                                case "4":
                                    totalcall += 1;
                                    totalsales += 1;
                                    visit += 1;
                                    break;
                                case "5":
                                    totalcall += 1;
                                    noncontactable += 1;
                                    visit += 1;
                                    break;
                                case "6":
                                    totalcall += 1;
                                    nonusable += 1;
                                    visit += 1;
                                    break;
                                case "7":
                                    totalcall += 1;
                                    notinterested += 1;
                                    visit += 1;
                                    break;
                                case "8":
                                    totalcall += 1;
                                    refixedbyuser += 1;
                                    visit += 1;
                                    break;
                                case "9":
                                    totalcall += 1;
                                    callback += 1;
                                    phonefollowup += 1;
                                    break;
                                case "10":
                                    totalcall += 1;
                                    noncontactable += 1;
                                    phonefollowup += 1;
                                    break;
                                case "11":
                                    totalcall += 1;
                                    refixedbyuser += 1;
                                    phonefollowup += 1;
                                    break;
                                case "12":
                                    totalcall += 1;
                                    totalsales += 1;
                                    phonefollowup += 1;
                                    break;
                                case "13":
                                    totalcall += 1;
                                    notinterested += 1;
                                    phonefollowup += 1;
                                    break;
                            }
                        }
                        if (i == DT.Rows.Count - 1)
                        {
                            DR[0] = oDBEngine.GetFieldValue(" tbl_master_user ", " user_name ", " user_id='" + DT.Rows[0]["CreateUser"].ToString() + "'", 1)[0, 0].ToString();
                            string Calculation = (oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id ", " COUNT(*) AS Expr1 ", " (CAST(tbl_trans_activies.createdate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_activies.createdate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_Activies.act_assignedTo = " + DT.Rows[0]["CreateUser"].ToString(), 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[1] = Calculation;
                            else
                                DR[1] = "0";
                            DR[2] = totalcall;
                            Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(*) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (1,2,3,4,5,6,7,8)", 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[3] = Calculation;
                            else
                                DR[3] = "0";
                            Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(distinct slv_salesvisitid) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (1,2,3,4,5,6,7,8)", 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[4] = Calculation;
                            else
                                DR[4] = "0";
                            Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", " count(*) as Id ", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (9,10,11,12,13)", 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[5] = Calculation;
                            else
                                DR[5] = "0";
                            Calculation = (oDBEngine.GetFieldValue(" tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id ", "distinct count(distinct slv_salesvisitid) as Id", " (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_salesvisitdetail.createuser=" + DT.Rows[0]["CreateUser"].ToString() + " and  tbl_master_SalesVisitOutCome.slv_Category in (9,10,11,12,13)", 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[6] = Calculation;
                            else
                                DR[6] = "0";
                            Calculation = (oDBEngine.GetFieldValue(" tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id ", " COUNT(*) AS Expr1 ", " (CAST(tbl_trans_activies.createdate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_activies.createdate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and tbl_trans_Activies.act_assignedTo = " + DT.Rows[0]["CreateUser"].ToString() + " and (tbl_trans_salesVisit.slv_id NOT IN (SELECT DISTINCT slv_SalesVisitId FROM tbl_trans_SalesVisitDetail WHERE  (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_salesvisitdetail.slv_visitdatetime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and createuser = " + DT.Rows[0]["CreateUser"].ToString() + "))", 1)[0, 0].ToString());
                            if (Calculation != "n")
                                DR[7] = Calculation;
                            else
                                DR[7] = "0";
                            DR[8] = refixedbyuser + refixedbylead;
                            DR[9] = callback;
                            DR[10] = totalsales;
                            DR[11] = notinterested;
                            DR[12] = noncontactable;
                            DR[13] = nonusable;
                            try
                            {
                                DR[14] = (int)(((totalcall - (noncontactable + nonusable)) * 100) / totalcall);
                                DR[15] = string.Format("{0:f}", ((totalsales * 100) / (totalcall - noncontactable)));
                            }
                            catch (Exception ex)
                            {
                                DR[14] = 0;
                                DR[15] = 0;
                            }
                            DR[16] = EmployeeId;
                            DR[17] = ReportingHeadId;



                            DT_main.Rows.Add(DR);

                            visit = 0;
                            phonefollowup = 0;
                            totalcall = 0;
                            refixedbyuser = 0;
                            refixedbylead = 0;
                            callback = 0;
                            totalsales = 0;
                            notinterested = 0;
                            noncontactable = 0;
                            nonusable = 0;

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
        void PrepareExporter()
        {
            //ASPxTreeListExporter1.Settings.AutoWidth = chkAutoWidth.Checked;
            //ASPxTreeListExporter1.Settings.ExpandAllNodes = chkExpandAll.Checked;
        }
    }
}