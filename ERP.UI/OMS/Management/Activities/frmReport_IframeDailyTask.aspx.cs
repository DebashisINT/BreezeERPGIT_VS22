using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web;
namespace ERP.OMS.Management.Activities
{
    public partial class management_Activities_frmReport_IframeDailyTask : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        clsDropDownList clsdropdown = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = Convert.ToDateTime(DateTime.Today);
                string allUser = oDBEngine.getChildUser_for_report(Session["userid"].ToString(), "") + Session["userid"].ToString();
                string[,] drpVal = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in(" + allUser + ")", 2);

                if (drpVal[0, 0] != "n")
                {
                    clsdropdown.AddDataToDropDownList(drpVal, drpUser);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "Js", "<script language='javascript'>Page_Load();</script>");
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            DataTable dt_main = new DataTable();
            DataColumn myColumn0 = new DataColumn("SNo");
            DataColumn myColumn1 = new DataColumn("Call/Visit DateTime");
            DataColumn myColumn2 = new DataColumn("Lead Name");
            DataColumn myColumn3 = new DataColumn("Address");
            DataColumn myColumn4 = new DataColumn("Phone");
            DataColumn myColumn5 = new DataColumn("Last Call/Visit DateTime");
            DataColumn myColumn6 = new DataColumn("LastOutCome");
            DataColumn myColumn7 = new DataColumn("History");
            dt_main.Columns.Add(myColumn0);
            dt_main.Columns.Add(myColumn1);
            dt_main.Columns.Add(myColumn2);
            dt_main.Columns.Add(myColumn3);
            dt_main.Columns.Add(myColumn4);
            dt_main.Columns.Add(myColumn5);
            dt_main.Columns.Add(myColumn6);
            dt_main.Columns.Add(myColumn7);
            string new_starttime = Convert.ToDateTime(dtDate.Value).ToShortDateString(); //+ " 1:00 AM";
            string new_endtime = Convert.ToDateTime(dtDate.Value).ToShortDateString(); //+ " 11:55 PM";
            bool flag = false;
            DataTable dt = new DataTable();
            DataTable dt_call = new DataTable();
            DataTable dt_visit = new DataTable();
            DataTable dt_salescall = new DataTable();
            DataTable dt_salesvisit = new DataTable();
            dt_call.Columns.Add("Call/Visit DateTime");
            dt_call.Columns.Add("Lead Name");
            dt_call.Columns.Add("Address");
            dt_call.Columns.Add("Phone");
            dt_call.Columns.Add("Last Call/Visit DateTime");
            dt_call.Columns.Add("LastOutCome");
            dt_call.Columns.Add("History");

            dt_visit.Columns.Add("Call/Visit DateTime");
            dt_visit.Columns.Add("Lead Name");
            dt_visit.Columns.Add("Address");
            dt_visit.Columns.Add("Phone");
            dt_visit.Columns.Add("Last Call/Visit DateTime");
            dt_visit.Columns.Add("LastOutCome");
            dt_visit.Columns.Add("History");

            dt_salescall.Columns.Add("Call/Visit DateTime");
            dt_salescall.Columns.Add("Lead Name");
            dt_salescall.Columns.Add("Address");
            dt_salescall.Columns.Add("Phone");
            dt_salescall.Columns.Add("Last Call/Visit DateTime");
            dt_salescall.Columns.Add("LastOutCome");
            dt_salescall.Columns.Add("History");

            dt_salesvisit.Columns.Add("Call/Visit DateTime");
            dt_salesvisit.Columns.Add("Lead Name");
            dt_salesvisit.Columns.Add("Address");
            dt_salesvisit.Columns.Add("Phone");
            dt_salesvisit.Columns.Add("Last Call/Visit DateTime");
            dt_salesvisit.Columns.Add("LastOutCome");
            dt_salesvisit.Columns.Add("History");

            dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "isnull((select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_lead where cnt_internalId=tbl_trans_salesVisit.slv_leadcotactId),(select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=tbl_trans_salesVisit.slv_leadcotactId)) AS Name,isnull((select cnt_internalId from tbl_master_lead where cnt_internalId=tbl_trans_salesVisit.slv_leadcotactId),(select cnt_internalid from tbl_master_contact where cnt_internalId=tbl_trans_salesVisit.slv_leadcotactId)) AS LeadId, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome AS Outcome, tbl_trans_salesVisit.slv_lastdatevisit AS LastVisit, convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) AS VisitDate, tbl_trans_salesvisit.slv_id", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,tbl_trans_salesvisit.slv_nextvisitdatetime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + new_starttime + "')) as datetime)  and act_assignedTo=" + drpUser.SelectedItem.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr;
                    string calltype = "";
                    string[,] calltype1 = oDBEngine.GetFieldValue("tbl_trans_salesvisitdetail", "top 1 isnull(slv_nextactivityType,2)", " slv_SalesVisitId=" + dt.Rows[i]["slv_id"].ToString() + "", 1);
                    if (calltype1[0, 0] != "n")
                    {
                        calltype = calltype1[0, 0];
                    }
                    if (calltype == "")
                        dr = dt_visit.NewRow();
                    else
                    {
                        if (calltype == "1")
                            dr = dt_call.NewRow();
                        else
                            dr = dt_visit.NewRow();
                    }
                    dr[0] = dt.Rows[i]["VisitDate"].ToString();
                    dr[1] = dt.Rows[i]["Name"].ToString() + " [" + dt.Rows[i]["LeadId"].ToString() + "]";
                    string address = "";
                    string[,] address1 = oDBEngine.GetFieldValue("tbl_master_address", "Top 1    ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' + ISNULL(add_pin, '') AS Address", " add_cntId='" + dt.Rows[i]["LeadId"].ToString() + "'", 1);
                    if (address1[0, 0] != "n")
                        address = address1[0, 0];
                    dr[2] = address;
                    DataTable dt_PhoneCall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["LeadId"].ToString() + "'");
                    if (dt_PhoneCall.Rows.Count > 0)
                    {
                        for (int ij = 0; ij < dt_PhoneCall.Rows.Count; ij++)
                        {
                            switch (dt_PhoneCall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    dr[3] += "(M)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    dr[3] += "(R)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    dr[3] += "(O)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    dr[4] = oconverter.ArrangeDate1(dt.Rows[i]["LastVisit"].ToString());
                    dr[5] = dt.Rows[i]["Outcome"].ToString();
                    dr[6] = "<div style='CURSOR: hand;color:#330099;' onclick=javascript:frmOpenNewWindow1('../management/ShowHistory_Phonecall.aspx?id1=" + dt.Rows[i]["LeadId"].ToString() + "',300,800)>History</div>";
                    if (calltype == "")
                        dt_visit.Rows.Add(dr);
                    else
                    {
                        if (calltype != "2")
                            dt_call.Rows.Add(dr);
                        else
                            dt_visit.Rows.Add(dr);
                    }
                }
            }
            dt.Dispose();
            dt = new DataTable();
            dt = oDBEngine.GetDataTable(" tbl_trans_Sales INNER JOIN tbl_trans_Activies ON tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id INNER JOIN tbl_master_SalesStatus ON tbl_trans_Sales.sls_sales_status = tbl_master_SalesStatus.sls_id", "isnull((select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_lead where cnt_internalId=tbl_trans_Sales.sls_contactlead_id ),(select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=tbl_trans_Sales.sls_contactlead_id)) AS Name,isnull((select cnt_internalId from tbl_master_lead where cnt_internalId=tbl_trans_Sales.sls_contactlead_id),(select cnt_internalid from tbl_master_contact where cnt_internalId=tbl_trans_Sales.sls_contactlead_id)) AS LeadId, tbl_trans_Sales.sls_datetime AS LastVisit, convert(varchar(11),tbl_trans_Sales.sls_nextvisitdate,113) AS VisitDate, tbl_master_SalesStatus.sls_status AS Outcome, tbl_trans_Sales.sls_id", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,tbl_trans_Sales.sls_nextvisitdate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + new_starttime + "')) as datetime)  and act_assignedTo=" + drpUser.SelectedItem.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr;
                    string calltype = "";
                    string[,] calltype1 = oDBEngine.GetFieldValue("tbl_trans_salesdetails", "top 1 isnull(sad_nextActivityType,2)", " sad_salesId=" + dt.Rows[i]["sls_id"].ToString() + " ", 1);
                    if (calltype1[0, 0] != "n")
                    {
                        calltype = calltype1[0, 0];
                    }
                    if (calltype == "")
                        dr = dt_visit.NewRow();
                    else
                    {
                        if (calltype == "1")
                            dr = dt_salescall.NewRow();
                        else
                            dr = dt_salesvisit.NewRow();
                    }
                    dr[0] = dt.Rows[i]["VisitDate"].ToString();
                    dr[1] = dt.Rows[i]["Name"].ToString() + " [" + dt.Rows[i]["LeadId"].ToString() + "]";
                    string address = "";
                    string[,] address1 = oDBEngine.GetFieldValue("tbl_master_address", "Top 1    ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address", " add_cntId='" + dt.Rows[i]["LeadId"].ToString() + "'", 1);
                    if (address1[0, 0] != "n")
                        address = address1[0, 0];
                    dr[2] = address;
                    DataTable dt_PhoneCall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["LeadId"].ToString() + "'");
                    if (dt_PhoneCall.Rows.Count > 0)
                    {
                        for (int ij = 0; ij < dt_PhoneCall.Rows.Count; ij++)
                        {
                            switch (dt_PhoneCall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    dr[3] += "(M)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    dr[3] += "(R)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    dr[3] += "(O)" + dt_PhoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    dr[4] = oconverter.ArrangeDate1(dt.Rows[i]["LastVisit"].ToString());
                    dr[5] = dt.Rows[i]["Outcome"].ToString();
                    dr[6] = "<div style='CURSOR: hand;color:#330099;' onclick= \"javascript:frmOpenNewWindow1('../management/ShowHistory_Phonecall.aspx?id1=" + dt.Rows[i]["LeadId"].ToString() + "',300,800)\">History</div>";
                    if (calltype == "")
                        dt_visit.Rows.Add(dr);
                    else
                    {
                        if (calltype != "2")
                            dt_salescall.Rows.Add(dr);
                        else
                            dt_salesvisit.Rows.Add(dr);
                    }
                }
            }
            int totalCall = 0;
            for (int i = 0; i < dt_call.Rows.Count; i++)
            {
                if (i == 0)
                {
                    DataRow dr1 = dt_main.NewRow();
                    dr1[3] = "<b>Phone Call</b>";
                    dt_main.Rows.Add(dr1);
                }
                DataRow dr = dt_main.NewRow();
                dr[0] = i + 1;
                dr[1] = dt_call.Rows[i][0].ToString();
                dr[2] = dt_call.Rows[i][1].ToString();
                dr[3] = dt_call.Rows[i][2].ToString();
                dr[4] = dt_call.Rows[i][3].ToString();
                dr[5] = dt_call.Rows[i][4].ToString();
                dr[6] = dt_call.Rows[i][5].ToString();
                dr[7] = dt_call.Rows[i][6].ToString();
                dt_main.Rows.Add(dr);
                totalCall = i + 1;
            }
            for (int i = 0; i < dt_salescall.Rows.Count; i++)
            {
                if (totalCall == 0)
                {
                    DataRow dr1 = dt_main.NewRow();
                    dr1[3] = "<b>Phone Call</b>";
                    dt_main.Rows.Add(dr1);
                }
                DataRow dr = dt_main.NewRow();
                dr[0] = totalCall + 1;
                totalCall += 1;
                dr[1] = dt_salescall.Rows[i][0].ToString();
                dr[2] = dt_salescall.Rows[i][1].ToString();
                dr[3] = dt_salescall.Rows[i][2].ToString();
                dr[4] = dt_salescall.Rows[i][3].ToString();
                dr[5] = dt_salescall.Rows[i][4].ToString();
                dr[6] = dt_salescall.Rows[i][5].ToString();
                dr[7] = dt_salescall.Rows[i][6].ToString();
                dt_main.Rows.Add(dr);
            }
            int totalVisit = 0;
            for (int i = 0; i < dt_visit.Rows.Count; i++)
            {
                if (i == 0)
                {
                    DataRow dr1 = dt_main.NewRow();
                    dr1[3] = "<b>Meeting Visit</b>";
                    dt_main.Rows.Add(dr1);
                }
                DataRow dr = dt_main.NewRow();
                dr[0] = i + 1;
                dr[1] = dt_visit.Rows[i][0].ToString();
                dr[2] = dt_visit.Rows[i][1].ToString();
                dr[3] = dt_visit.Rows[i][2].ToString();
                dr[4] = dt_visit.Rows[i][3].ToString();
                dr[5] = dt_visit.Rows[i][4].ToString();
                dr[6] = dt_visit.Rows[i][5].ToString();
                dr[7] = dt_visit.Rows[i][6].ToString();
                dt_main.Rows.Add(dr);
                totalVisit = i + 1;
            }
            for (int i = 0; i < dt_salesvisit.Rows.Count; i++)
            {
                if (totalVisit == 0)
                {
                    DataRow dr1 = dt_main.NewRow();
                    dr1[3] = "<b>Meetings</b>";
                    dt_main.Rows.Add(dr1);
                }
                DataRow dr = dt_main.NewRow();
                dr[0] = totalVisit + 1;
                totalVisit += 1;
                dr[1] = dt_salesvisit.Rows[i][0].ToString();
                dr[2] = dt_salesvisit.Rows[i][1].ToString();
                dr[3] = dt_salesvisit.Rows[i][2].ToString();
                dr[4] = dt_salesvisit.Rows[i][3].ToString();
                dr[5] = dt_salesvisit.Rows[i][4].ToString();
                dr[6] = dt_salesvisit.Rows[i][5].ToString();
                dr[7] = dt_salesvisit.Rows[i][6].ToString();
                dt_main.Rows.Add(dr);
            }
            grdReports.DataSource = dt_main.DefaultView;
            grdReports.DataBind();
            DataTable dt_temp = new DataTable();
            dt_temp.Columns.Add("SNo");
            dt_temp.Columns.Add("Call/Visit DateTime");
            dt_temp.Columns.Add("Lead Name");
            dt_temp.Columns.Add("Address");
            dt_temp.Columns.Add("Phone");
            dt_temp.Columns.Add("Last Call/Visit DateTime");
            dt_temp.Columns.Add("LastOutCome");
            for (int i = 0; i < dt_main.Rows.Count; i++)
            {
                DataRow dr = dt_temp.NewRow();
                dr[0] = dt_main.Rows[i][0].ToString();
                dr[1] = dt_main.Rows[i][1].ToString();
                dr[2] = dt_main.Rows[i][2].ToString();
                dr[3] = dt_main.Rows[i][3].ToString();
                dr[4] = dt_main.Rows[i][4].ToString();
                dr[5] = dt_main.Rows[i][5].ToString();
                dr[6] = dt_main.Rows[i][6].ToString();
                dt_temp.Rows.Add(dr);
            }
            string spantext = "";
            if (dt_main.Rows.Count == 0)
                spantext = "No Data Found";
            else
                spantext = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Jscript", "visibleProperty('" + spantext + "');", true);
        }
        protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Text = Server.HtmlDecode(e.Row.Cells[i].Text.ToString());
                }
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Text = Server.HtmlDecode(e.Row.Cells[i].Text.ToString());
                    }
                }
            }
        }
    }
}