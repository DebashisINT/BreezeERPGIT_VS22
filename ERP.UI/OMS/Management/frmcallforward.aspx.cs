using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmcallforward : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        public string pageAccess = "";
        clsDropDownList cls = new clsDropDownList();
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                SaveData();
                FillData(Session["phonecallid"].ToString());
            }
        }
        public void FillData(string id)
        {
            DataTable dt = new DataTable();
            string[,] drpval = oDBEngine.GetFieldValue("tbl_trans_employeeCTC INNER JOIN tbl_master_user ON tbl_trans_employeeCTC.emp_cntId = tbl_master_user.user_contactId", "tbl_master_user.user_Id as Id,tbl_master_user.user_name as Name", " (tbl_trans_employeeCTC.emp_Department =(SELECT tbl_trans_employeeCTC.emp_Department FROM tbl_trans_employeeCTC INNER JOIN tbl_master_user ON tbl_trans_employeeCTC.emp_cntId = tbl_master_user.user_contactId WHERE tbl_master_user.user_id ='" + Session["userid"].ToString() + "'))", 2, "tbl_master_user.user_name");
            if (drpval[0, 0] != "n")
            {
                cls.AddDataToDropDownList(drpval, drpCallForward);
            }
            dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id", "tbl_master_calldispositions.call_dispositions, tbl_trans_phonecall.phc_nextCall, tbl_trans_phonecall.phc_note", " tbl_trans_phonecall.phc_id = '" + id + "'");
            if (dt.Rows.Count != 0)
            {
                txtCallOutcome.Text = dt.Rows[0][0].ToString();
                txtInsutruction.Text = dt.Rows[0][2].ToString();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            DateTime dDate = oDBEngine.GetDate();
            string sStartDate = dDate.ToShortDateString();
            string sStartTime = dDate.ToShortTimeString();
            DataTable dt = new DataTable();
            bool flag = false;
            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "tbl_trans_Activies.act_id, tbl_trans_Activies.act_branchId", "tbl_trans_phonecall.phc_id ='" + Session["phonecallid"].ToString() + "'");
            if (dt.Rows.Count != 0)
            {
                string ActNo = "";
                string[,] Actno1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_activityNo)", " act_activityNo like '%PC%'", 1);
                if (Actno1[0, 0] != "n")
                {
                    ActNo = Actno1[0, 0];
                }
                string fields = "act_branchId, act_activityType, act_activityNo, act_previousActno, act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction";
                string values = "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','1','" + ActNo + "','" + dt.Rows[0][0].ToString() + "','" + Session["userid"].ToString() + "','" + drpCallForward.SelectedItem.Value + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartDate + "','" + sStartTime + "','" + sStartDate + "','" + sStartTime + "','" + txtInsutruction.Text + "'";
                oDBEngine.InsurtFieldValue("tbl_trans_activies", fields, values);
                string id = "";
                string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + ActNo + "'", 1);
                if (id1[0, 0] != "n")
                {
                    id = id1[0, 0];
                }
                if (id != "")
                {
                    flag = Convert.ToBoolean(oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_activityId='" + id + "',phc_forwardcall='" + Session["userid"].ToString() + "'", " phc_id='" + Session["phonecallid"].ToString() + "'"));
                }
            }
            string script = "";
            if (flag)
            {
                script += "<script language='javascript'>";
                //script += "window.opener.document.getElementById('ctl00_ContentPlaceHolder3_showLeadInfo').style.display = 'none';";
                script += "window.opener.document.getElementById('ctl00_ContentPlaceHolder3_btnSavePhoneCallDetails').disabled= 'false';";
                //script += "window.opener.document.getElementById('ctl00_ContentPlaceHolder3_drpCallDispose').disabled = 'true';";
                script += "window.opener.document.getElementById('ctl00_ContentPlaceHolder3_txtNotes').disabled = 'true';";
                script += "window.close();";
                script += "</script>";
            }
            ClientScript.RegisterStartupScript(GetType(), "script", script);

        }
        public void SaveData()
        {
            string str = "";// oDBEngine.getDateTimeFormat(Request.QueryString["txtnextcalldate"].ToString());
            string calldispose1 = Request.QueryString["txtCallDispose_id"];
            int i = calldispose1.LastIndexOf("|");
            string calldispose = calldispose1.Substring(0, i);
            string calldispose_cat = calldispose1.Substring(i + 1);
            bool flag;
            string branchid = "";
            if (HttpContext.Current.Session["userbranchID"] != null)
            {
                branchid = HttpContext.Current.Session["userbranchID"].ToString();
            }
            else
            {
                branchid = "0";
            }
            string callDuration = "0";
            string lastcall = "";
            string calldate = Request.QueryString["callstarttime"].ToString();
            if (calldate == "")
            {
                calldate = oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString();
            }
            else
            {
                if (Request.QueryString["callendtime"].ToString() != null)
                {
                    lastcall = oDBEngine.GetDate().ToString();
                }
                else
                {
                    lastcall = oDBEngine.GetDate().ToString();
                }
                TimeSpan tSpan = Convert.ToDateTime(lastcall).Subtract(Convert.ToDateTime(calldate));
                callDuration = Convert.ToString(tSpan.Seconds.ToString() + (tSpan.Minutes * 60));
            }
            DateTime start = Convert.ToDateTime(calldate);
            string note = Request.QueryString["txtNote"].ToString();
            DateTime endcall = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString());
            flag = Convert.ToBoolean(oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId,phd_branchId,phd_callDate,phd_callStart,phd_callEnd,phd_callDispose,phd_note,phd_nextCall,phd_callduration", "'" + Session["phonecallid"].ToString() + "','" + branchid + "','" + calldate + "','" + start.ToShortDateString() + " " + start.ToShortTimeString() + "','" + endcall.ToShortDateString() + " " + endcall.ToShortTimeString() + "','" + calldispose + "','" + note + "','" + str + "','" + callDuration.ToString() + "'"));
            if (flag)
            {
                flag = Convert.ToBoolean(oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + calldate + "',phc_callDispose='" + calldispose + "',phc_nextCall='" + str + "',phc_note='" + note + "',phc_lastCallDuration='" + callDuration + "'", " phc_id='" + Session["phonecallid"].ToString() + "'"));
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Session["newactivityid"].ToString() + "'");
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0][0].ToString() == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().Date + " " + oDBEngine.GetDate().ToShortTimeString() + "'", " act_id='" + Session["newactivityid"].ToString() + "'");
                    }
                    else
                    {
                        if (dt.Rows[0][1].ToString() == "")
                        {
                            DataTable temp = new DataTable();
                            temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId = '" + Session["newactivityid"].ToString() + "'");
                            if (temp.Rows.Count != 0)
                            {
                                if (temp.Rows[0][0].ToString() == "0")
                                {
                                    oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().Date + " " + oDBEngine.GetDate().ToShortTimeString() + "'", " act_id='" + Session["newactivityid"].ToString() + "'");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}