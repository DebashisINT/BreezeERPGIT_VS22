using System;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmphoneCall_CourtesyCall : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        clsDropDownList cls = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDown();
                drpSalesVisitOutcome.Attributes.Add("onchange", "funJScript(this)");
                string today = ObjConvert.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");

                ImgVisit.Attributes.Add("OnClick", "displayCalendar(VisitDate,'dd/mm/yyyy hh:ii',VisitDate,true,null,'0',220)");
                VisitDate.Attributes.Add("onfocus", "displayCalendar(VisitDate ,'dd/mm/yyyy hh:ii',this,true,null,'0',220)");
                VisitDate.Attributes.Add("readonly", "true");

            }
        }
        public void FillDropDown()
        {
            string[,] courtsy = oDBEngine.GetFieldValue("tbl_master_CourtesyCallFeedback", "ccf_id as Id,ccf_feedback as FeedBack", null, 2, "ccf_feedback");
            if (courtsy[0, 0] != "n")
            {
                cls.AddDataToDropDownList(courtsy, drpCourtesy);
            }
            string[,] visitOutcome = oDBEngine.GetFieldValue("tbl_master_SalesVisitOutCome", "(cast(slv_id as nvarchar)) + '!!' + (cast(slv_Category as nvarchar)) as Id,slv_SalesVisitOutcome", null, 2, "slv_SalesVisitOutcome");
            if (visitOutcome[0, 0] != "n")
            {
                cls.AddDataToDropDownList(visitOutcome, drpSalesVisitOutcome);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["id"].ToString();
                string phonecallid = "";
                string[,] phonecallid1 = oDBEngine.GetFieldValue("tbl_trans_phonecall", "phc_id", " phc_leadcotactid='" + id + "'", 1);
                if (phonecallid1[0, 0] != "n")
                {
                    phonecallid = phonecallid1[0, 0];
                }
                string salesvisitid = "";
                string[,] salesvisitid1 = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "tbl_trans_Activies.act_activityNo", " slv_leadcotactid='" + id + "'", 1);
                if (salesvisitid1[0, 0] != "n")
                {
                    salesvisitid = salesvisitid1[0, 0];
                }
                string callStartDate = oDBEngine.GetDate().ToShortDateString();
                string callStartTime = oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString();
                string callendTime = oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString();
                string callDuration = "0";
                string feedBack = drpCourtesy.SelectedValue.ToString();
                string[] outcome1 = (drpSalesVisitOutcome.SelectedValue.ToString().Trim()).Split('!');
                string outcome = outcome1.GetValue(0).ToString();
                string userId = Session["userid"].ToString();
                if (Convert.ToInt32(outcome1.GetValue(2)) != 8)
                {
                    oDBEngine.InsurtFieldValue("tbl_trans_CourtesyCalls", "cpc_phoneCallId,cpc_salesVisitId,cpc_leadcontactId,cpc_callStartDate,cpc_callStartTime,cpc_callendTime,cpc_callDuration,cpc_feedBack,cpc_outcome,cpc_userId,cpc_note", "'" + phonecallid + "','" + salesvisitid + "','" + id + "','" + callStartDate + "','" + callStartTime + "','" + callendTime + "','" + callDuration + "','" + feedBack + "','" + outcome + "','" + userId + "','" + txtNote.Text + "'");
                }
                else
                {
                    string nextvisit = ObjConvert.DateConverter_d_m_y(VisitDate.Text);
                    oDBEngine.InsurtFieldValue("tbl_trans_CourtesyCalls", "cpc_phoneCallId,cpc_salesVisitId,cpc_leadcontactId,cpc_callStartDate,cpc_callStartTime,cpc_callendTime,cpc_callDuration,cpc_feedBack,cpc_outcome,cpc_userId,cpc_note,cpc_nextvisit", "'" + phonecallid + "','" + salesvisitid + "','" + id + "','" + callStartDate + "','" + callStartTime + "','" + callendTime + "','" + callDuration + "','" + feedBack + "','" + outcome + "','" + userId + "','" + txtNote.Text + "','" + nextvisit + "'");
                    oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_nextvisitdatetime='" + nextvisit + "',slv_salesvisitoutcome='" + outcome + "'", " slv_leadcotactId='" + id + "'");
                    string salesvisitid123 = "";
                    string[,] salesvisitid1234 = oDBEngine.GetFieldValue("tbl_trans_SalesVisitDetail INNER JOIN tbl_trans_salesVisit ON tbl_trans_SalesVisitDetail.slv_SalesVisitId = tbl_trans_salesVisit.slv_id", "TOP 1 tbl_trans_salesVisit.slv_id", " tbl_trans_salesvisit.slv_leadcotactid='" + id + "'", 1);
                    if (salesvisitid1234[0, 0] != "n")
                    {
                        salesvisitid123 = salesvisitid1234[0, 0];
                    }
                    if (salesvisitid123 != "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_salesvisitdetail", "slv_nextActivityType=2", " slv_id='" + salesvisitid123 + "'");
                    }
                }
            }
            string jScript = "<script language='javascript'>window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", jScript);
        }
    }
}