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
    public partial class management_Activities_frmReports_IframePhoneCall_DailyTask : ERP.OMS.ViewState_class.VSPage
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
                //string[,] drpVal = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", null, 2);
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
            FillGridView();
        }
        public void FillGridView()
        {
            DataTable dt_main = new DataTable();
            string new_starttime = Convert.ToDateTime(dtDate.Value).ToShortDateString();
            int iRow = 0;
            dt_main.Columns.Add("SNo");
            dt_main.Columns.Add("LeadName");
            dt_main.Columns.Add("PhoneNumber");
            dt_main.Columns.Add("NextCall");
            dt_main.Columns.Add("LastOutcome");
            dt_main.Columns.Add("History");
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", "isnull((select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_lead where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId),(select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId)) AS Name,isnull((select cnt_internalId from tbl_master_lead where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId),(select cnt_internalid from tbl_master_contact where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId)) AS cnt_internalid, tbl_trans_phonecall.phc_nextCall, tbl_master_calldispositions.call_dispositions", " tbl_trans_Activies.act_assignedTo = " + drpUser.SelectedItem.Value.ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,phc_nextcall)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + new_starttime + "')) as datetime)  and Call_Category in (1,2)", "Call_Category,cast(phc_nextcall as datetime)");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt_main.NewRow();
                    dr[0] = iRow + 1;
                    dr[1] = dt.Rows[i]["Name"].ToString() + " [" + dt.Rows[i]["cnt_internalid"].ToString() + "]";
                    string PhoneNo = "";
                    DataTable dt_PhoneNo = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["cnt_internalid"].ToString() + "'");
                    if (dt_PhoneNo.Rows.Count > 0)
                    {
                        for (int ij = 0; ij < dt_PhoneNo.Rows.Count; ij++)
                        {
                            switch (dt_PhoneNo.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    PhoneNo += "(M)" + dt_PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    PhoneNo += "(R)" + dt_PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    PhoneNo += "(O)" + dt_PhoneNo.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    dr[2] = PhoneNo;
                    dr[3] = oconverter.ArrangeDate1(dt.Rows[i]["phc_nextCall"].ToString());
                    dr[4] = dt.Rows[i]["call_dispositions"].ToString();
                    dr[5] = "<div style='CURSOR: hand;color:#330099;' onclick=javascript:frmOpenNewWindow1('../ShowHistory_Phonecall.aspx?id1=" + dt.Rows[i]["cnt_internalid"].ToString() + "',300,800)>History</div>";
                    iRow += 1;
                    dt_main.Rows.Add(dr);
                }
            }
            if (iRow < 200)
            {
                int row = 200 - iRow;
                dt = new DataTable();
                dt = oDBEngine.GetDataTable("tbl_trans_phonecall  INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", "top " + row + " isnull((select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_lead where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId),(select isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId)) AS Name,isnull((select cnt_internalId from tbl_master_lead where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId),(select cnt_internalid from tbl_master_contact where cnt_internalId=tbl_trans_phonecall.phc_leadcotactId)) AS cnt_internalid, tbl_trans_phonecall.phc_nextCall, tbl_master_calldispositions.call_dispositions", " tbl_trans_Activies.act_assignedTo = " + drpUser.SelectedItem.Value.ToString() + " and phc_callDispose = 11");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt_main.NewRow();
                        dr[0] = iRow + 1;
                        dr[1] = dt.Rows[i]["Name"].ToString() + " [" + dt.Rows[i]["cnt_internalid"].ToString() + "]";
                        string PhoneNo = "";
                        DataTable dt_phoneCall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["cnt_internalid"].ToString() + "'");
                        if (dt_phoneCall.Rows.Count > 0)
                        {
                            for (int ij = 0; ij < dt_phoneCall.Rows.Count; ij++)
                            {
                                switch (dt_phoneCall.Rows[ij]["phf_type"].ToString().ToUpper())
                                {
                                    case "MOBILE":
                                        PhoneNo += "(M)" + dt_phoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                        break;
                                    case "RESIDENCE":
                                        PhoneNo += "(R)" + dt_phoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                        break;
                                    case "OFFICIAL":
                                        PhoneNo += "(O)" + dt_phoneCall.Rows[ij]["phf_phoneNumber"].ToString();
                                        break;
                                }
                            }
                        }
                        dr[2] = PhoneNo;
                        dr[3] = "New Calls";
                        dr[4] = "";
                        dr[5] = "";
                        iRow += 1;
                        dt_main.Rows.Add(dr);
                    }
                }
            }
            string spanText = "TelleCaller Daily Task :" + drpUser.SelectedItem.Text.ToString() + " For " + oconverter.ArrangeDate2(Convert.ToDateTime(dtDate.Value).ToShortDateString());

            if (dt_main.Rows.Count > 0)
            {
                grdReports.DataSource = dt_main;
                grdReports.DataBind();
                spanText = "";
            }
            else
            {
                grdReports.DataSource = dt_main;
                grdReports.DataBind();
                spanText = "There is No Row";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Jscript", "visibleProperty('" + spanText + "');", true);
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