using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_Report_teleCaller_salesVisit : System.Web.UI.Page
    {
        DataTable DT_main = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        int uniqueNo = 0;
        DataTable DT = new DataTable();
        DataTable DT_lead = new DataTable();
        DataTable DT_address = new DataTable();
        DataTable DT_Salesvisit = new DataTable();
        DataTable DT_SalesvisitDetail = new DataTable();
        DataTable DT_SalesvisitOutcome = new DataTable();
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
                Session["mode"] = "off";
                //txtFromDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgFromDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtFromDate,true,null,'0','0')");
                //txtToDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgToDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtToDate,true,null,'0','0')");
                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtFromDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();

                tblExport.Visible = false;
                TLTeleSalesvisit.Visible = false;
            }
            if (Session["mode"].ToString() == "on")
                ShowReport();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void ShowReport()
        {
            Session["mode"] = "on";
            tblExport.Visible = true;
            TLTeleSalesvisit.Visible = true;

            DT_main = new DataTable();

            //______Table Design______________///
            DataColumn col1 = new DataColumn("User");
            DataColumn col2 = new DataColumn("LeadName");
            DataColumn col3 = new DataColumn("VisitDate");
            DataColumn col4 = new DataColumn("VisitPlace");
            DataColumn col5 = new DataColumn("Status");
            DataColumn col6 = new DataColumn("User_ID");
            DataColumn col7 = new DataColumn("User_ParentId");
            DataColumn col8 = new DataColumn("Unique_ID");

            DT_main.Columns.Add(col1);
            DT_main.Columns.Add(col2);
            DT_main.Columns.Add(col3);
            DT_main.Columns.Add(col4);
            DT_main.Columns.Add(col5);
            DT_main.Columns.Add(col6);
            DT_main.Columns.Add(col7);
            DT_main.Columns.Add(col8);

            //_________________________________//
            string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 1:00:00 AM";

            string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:55:00 PM";

            string[,] calldisposition = oDBEngine.GetFieldValue(" tbl_master_calldispositions ", " call_id ", " Call_category =4", 1);
            string ids = "";
            for (int i = 0; i < calldisposition.Length; i++)
            {
                if (ids == "")
                    ids = calldisposition[i, 0];
                else
                    ids = "," + calldisposition[i, 0];

            }
            DT = oDBEngine.GetDataTable(" tbl_trans_phonecall ", " *, convert(varchar(20),phc_nextCall,100) as phc_nextCall1", " phc_callDispose in (" + ids + ") and CreateDate >= convert(datetime,'" + startdate + "') and CreateDate <= convert(datetime,'" + Enddate + "') ");
            DT_lead = oDBEngine.GetDataTable(" tbl_master_lead ", " (case when cnt_firstname is null then '' else cnt_firstname end+ ' '+ case when cnt_middlename is null then '' else cnt_middlename end + ' '+ case when cnt_lastName is null then '' else cnt_lastName end) as name, cnt_internalId ", null);
            DT_address = oDBEngine.GetDataTable(" tbl_master_address ", " (case when add_address1 is null then '' else add_address1 end +' '+case when add_address2 is null then '' else add_address2 end +' '+case when add_address3 is null then '' else add_address3 end+' '+case when add_landMark is null then '' else add_landMark end+', '+case when (add_country is null or add_country='0') then '' else (select cou_country from tbl_master_country where cou_id=add_country) end+', '+case when (add_state is null or add_state='0') then '' else (select state from tbl_master_state where id=add_state) end+', '+case when (add_city is null or add_city='0') then '' else (select city_name from tbl_master_city where city_id=add_city) end+', '+case when add_area is null then '' else (select area_name from tbl_master_area where area_id=add_area) end+', '+case when add_pin is null then '' else ('Pin:'+add_pin) end) as address,add_id,add_cntid ", " add_address1 is not null and add_cntId like 'LD%' ");//___Here we need only lead`s adress
            DT_SalesvisitDetail = oDBEngine.GetDataTable(" tbl_trans_salesVisitDetail ", " slv_id,slv_SalesVisitId,slv_notes,CreateDate ", " CreateDate > convert(datetime,'" + startdate + "')", " slv_SalesVisitId ");
            DT_Salesvisit = oDBEngine.GetDataTable(" tbl_trans_salesvisit ", " slv_id,slv_activityId,slv_PreviousActivityId,slv_salesvisitoutcome ", " CreateDate > convert(datetime,'" + startdate + "')");
            DT_SalesvisitOutcome = oDBEngine.GetDataTable(" tbl_master_SalesVisitOutcome ", " slv_id,slv_salesVisitOutcome ", null);

            //__________calculating Loged in user`s salesVisit activity___________-//
            CalculateEmployeeSalesCall(HttpContext.Current.Session["userid"].ToString(), "0");

            //___________Now calling function to Chiled User`s work regarding sales_____//
            GetUserList_call_CalculateEmployeeSalesCall(HttpContext.Current.Session["userid"].ToString());

            TLTeleSalesvisit.DataSource = DT_main.DefaultView;
            TLTeleSalesvisit.DataBind();
        }
        private void CalculateEmployeeSalesCall(string EmployeeId, string ReportingHeadId)
        {
            if (EmployeeId != "")
            {

                string User_Name = "";
                int SlNo = 0;


                //string expression = " CreateUser=" + EmployeeId + " and CreateDate > '" + startdate + "' and CreateDate <='" + Enddate + "'";
                string expression = " CreateUser=" + EmployeeId;// +" and CreateDate > '1/1/2009 1:00' and CreateDate <='3/3/2009 11:45'";
                string name = (oDBEngine.GetFieldValue(" tbl_master_user ", " user_name ", " user_id=" + EmployeeId, 1))[0, 0];
                if (name != "n")
                {
                    DataRow[] datarow = DT.Select(expression);
                    //SlNo += 1;
                    DataRow DR = DT_main.NewRow();
                    DR[0] = name;
                    DR[1] = datarow.Length.ToString();
                    DR[2] = "";
                    DR[3] = "";
                    DR[4] = "";
                    DR[5] = EmployeeId;
                    DR[6] = ReportingHeadId;
                    DR[7] = "";
                    DT_main.Rows.Add(DR);

                    for (int i = 0; i < datarow.Length; i++)
                    {
                        DR = DT_main.NewRow();
                        SlNo += 1;
                        uniqueNo += 1;
                        if (uniqueNo == int.Parse(HttpContext.Current.Session["userid"].ToString()))
                            uniqueNo += 1;
                        DR[0] = SlNo;
                        expression = "cnt_internalId='" + datarow[i]["phc_leadcotactId"].ToString() + "'";
                        DR[1] = this.DT_lead.Select(expression)[0]["name"];
                        DR[2] = datarow[i]["phc_nextCall1"];
                        expression = "add_id=" + datarow[i]["phc_addId"].ToString();
                        DataRow[] address = DT_address.Select(expression);
                        if (address.Length > 0)
                            DR[3] = address[0]["address"];
                        else
                            DR[3] = "N/A";
                        expression = " slv_PreviousActivityId=" + datarow[i]["phc_id"].ToString();
                        DataRow[] salesvisit = DT_Salesvisit.Select(expression);
                        if (salesvisit.Length > 0)
                        {
                            if (salesvisit[0]["slv_salesvisitoutcome"].ToString() != "9")
                            {
                                expression = " slv_salesvisitId='" + salesvisit[0]["slv_id"] + "'";// order by CreateDate DESC ";
                                DataRow[] salesvisitDetail = DT_SalesvisitDetail.Select(expression, " CreateDate DESC ");
                                string NOTES = salesvisitDetail[0]["slv_notes"].ToString();
                                expression = "slv_id=" + salesvisit[0]["slv_salesvisitoutcome"];
                                DataRow[] salesOutcome = DT_SalesvisitOutcome.Select(expression);
                                DR[4] = salesOutcome[0]["slv_salesVisitOutcome"] + "[" + NOTES + "]";
                            }
                            else
                                DR[4] = "Due";
                        }
                        else
                            DR[4] = "No one has been allocated to the sales Visit!";
                        DR[5] = "a" + uniqueNo;
                        DR[6] = EmployeeId;
                        DR[7] = "";
                        DT_main.Rows.Add(DR);
                    }
                }

            }
        }
        private void GetUserList_call_CalculateEmployeeSalesCall(string EmployeeID)
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
                            CalculateEmployeeSalesCall(userids[i, 0], "0");
                            GetUserList_call_CalculateEmployeeSalesCall(userids[i, 0]);
                        }
                    }
                }
            }
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ShowReport();
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExport.Value.ToString() != "")
            {
                //ASPxTreeListExporter1.Settings.PageSettings.Landscape = true;
                //ASPxTreeListExporter1.Settings.PageSettings.Margins.Left = 2;
                //ASPxTreeListExporter1.Settings.PageSettings.Margins.Right = 2;
                //ASPxTreeListExporter1.TreeList.Font.Size = FontUnit.Parse("10px");
                if (cmbExport.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (cmbExport.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (cmbExport.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();

                cmbExport.SelectedIndex = 0;
            }
        }
    }
}