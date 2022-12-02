using System;
using System.Data;
using System.Web;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmalloat : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        clsDropDownList cls = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDownDepartment();
                FillDropDownBranch();
                FillDropDownUser();
                if (Request.QueryString["id"].ToString() == "")
                {
                    string jscript = "<script language='javascript'>alert('Please select the lead For Allot');window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "JScript", jscript);
                }
            }
        }
        public void FillDropDownDepartment()
        {
            string[,] Deptt = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id,cost_description", " cost_costCenterType='department'", 2, "cost_description");
            if (Deptt[0, 0] != "n")
            {
                cls.AddDataToDropDownList(Deptt, drpDepartment);
            }
        }
        public void FillDropDownBranch()
        {
            string[,] Branch = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,branch_description", null, 2, "branch_description");
            if (Branch[0, 0] != "n")
            {
                cls.AddDataToDropDownList(Branch, drpBranch);
            }
        }
        public void FillDropDownUser()
        {
            string[,] User = oDBEngine.GetFieldValue("tbl_trans_employeeCTC INNER JOIN tbl_master_user ON tbl_trans_employeeCTC.emp_cntId = tbl_master_user.user_contactId", "tbl_master_user.user_id AS Id, tbl_master_user.user_name AS Name", " (tbl_trans_employeeCTC.emp_Department = " + drpDepartment.SelectedValue.ToString() + ") AND (tbl_master_user.user_branchId = " + drpBranch.SelectedValue.ToString() + ")", 2, "tbl_master_user.user_name");
            if (User[0, 0] != "n")
            {
                cls.AddDataToDropDownList(User, drpUser);
            }
        }
        protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropDownUser();
        }
        protected void drpBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropDownUser();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string str = Request.QueryString["id"].ToString();
            string productid = "";
            string leadid = "";
            if (str != "")
            {
                string[] st = str.Split(',');
                for (int i = 0; i < st.GetUpperBound(0); i++)
                {
                    string[] s = st[i].Split('@');
                    if (s.GetValue(0) != "")
                    {
                        if (i == 0)
                        {
                            string sub_str = s.GetValue(0).ToString();
                            leadid = sub_str.ToString().Trim().Substring((sub_str.ToString().Trim().Length - 10), 10);
                        }
                        else
                        {
                            string sub_str = s.GetValue(0).ToString();
                            leadid += "," + sub_str.ToString().Trim().Substring((sub_str.ToString().Trim().Length - 10), 10);
                        }
                    }
                    if (s.GetValue(4) != "")
                    {
                        if (i == 0)
                        {
                            productid = s.GetValue(4).ToString();
                        }
                        else
                        {
                            productid += "," + s.GetValue(4).ToString();
                        }
                    }
                }
            }
            if (leadid != "")
            {
                string[] ld = leadid.Split(',');
                DateTime mindate = oDBEngine.GetDate();
                DateTime maxdate = oDBEngine.GetDate();
                for (int i = 0; i <= ld.GetUpperBound(0); i++)
                {
                    string nextcall = "";
                    string[,] nextcall1 = oDBEngine.GetFieldValue("tbl_trans_phonecall", "phc_nextcall", " phc_leadcotactid='" + ld[i] + "'", 1);
                    if (nextcall1[0, 0] != "n")
                    {
                        nextcall = nextcall1[0, 0];
                    }
                    if (i == 0)
                    {
                        if (nextcall == "")
                        {
                            mindate = Convert.ToDateTime(oDBEngine.GetDate());
                            maxdate = Convert.ToDateTime(oDBEngine.GetDate());
                        }
                        else
                        {
                            mindate = Convert.ToDateTime(nextcall);
                            maxdate = Convert.ToDateTime(nextcall);
                        }
                    }
                    else
                    {
                        if ((DateTime)mindate > Convert.ToDateTime(nextcall))
                        {
                            mindate = Convert.ToDateTime(nextcall);
                        }
                        if ((DateTime)maxdate < Convert.ToDateTime(nextcall))
                        {
                            maxdate = Convert.ToDateTime(nextcall);
                        }
                    }
                }
                if (((DateTime)mindate).ToShortDateString() == ((DateTime)maxdate).ToShortDateString())
                {
                    DateTime dDate1 = mindate;
                    string sStartDate = dDate1.ToShortDateString();
                    string sStartTime = dDate1.ToShortTimeString();
                    dDate1 = maxdate;
                    string sEndDate = dDate1.ToShortDateString();
                    string sEndTime = dDate1.ToShortTimeString();
                    string[] pid = productid.Split(',');
                    if (Session["selectedbutton"] == null)
                    {
                        Session["selectedbutton"] = "";
                    }
                    string SButton = Session["selectedbutton"].ToString();
                    switch (SButton)
                    {
                        case "sales":
                            for (int i = 0; i <= ld.GetUpperBound(0); i++)
                            {
                                DataTable dt_salesvisit = new DataTable();
                                dt_salesvisit = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId", "tbl_trans_offeredProduct.ofp_id AS ProductId,tbl_trans_offeredProduct.ofp_productid,tbl_trans_offeredProduct.ofp_producttypeid tbl_trans_salesVisit.*,tbl_trans_activies.act_activityNo as ActivityNo", " slv_leadcotactid='" + ld.GetValue(i) + "' and act_assignedTo=" + Session["userid"].ToString());
                                if (dt_salesvisit != null)
                                {
                                    if (dt_salesvisit.Rows.Count != 0)
                                    {
                                        string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                                        string Fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction";
                                        string Values = "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','6','" + actNo + "','" + Session["userid"].ToString() + "','" + drpUser.SelectedValue.ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartDate + "','" + sStartTime + "','" + sEndDate + "','" + sEndTime + "','" + TxtInstruction.Text + "'";
                                        oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields, Values);
                                        string id = "";
                                        string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + actNo + "'", 1);
                                        if (id1[0, 0] != "n")
                                        {
                                            id = id1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + ld[i] + "'");
                                        string access = "";
                                        string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + ld[i] + "'", 1);
                                        if (access1[0, 0] != "n")
                                        {
                                            access = access1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + drpUser.SelectedValue.ToString() + "'", " cnt_internalid='" + ld[i] + "'");
                                        for (int u = 0; u <= dt_salesvisit.Rows.Count - 1; u++)
                                        {
                                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_activityId='" + actNo + "'", " ofp_id='" + dt_salesvisit.Rows[u]["ProductId"].ToString() + "'");
                                        }
                                    }
                                }
                            }
                            break;
                        case "salesvisit":
                            for (int i = 0; i <= ld.GetUpperBound(0); i++)
                            {
                                DataTable dt_salesvisit = new DataTable();
                                dt_salesvisit = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId", "tbl_trans_offeredProduct.ofp_id AS ProductId, tbl_trans_salesVisit.*,tbl_trans_activies.act_activityNo as ActivityNo", " slv_leadcotactid='" + ld[i] + "' and act_assignedTo=" + Session["userid"].ToString());
                                if (dt_salesvisit != null)
                                {
                                    if (dt_salesvisit.Rows.Count != 0)
                                    {
                                        string actNo2 = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                                        string Fields1 = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction";
                                        string Values1 = "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + actNo2 + "','" + Session["userid"].ToString() + "','" + drpUser.SelectedValue.ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartDate + "','" + sStartTime + "','" + sEndDate + "','" + sEndTime + "','" + TxtInstruction.Text + "'";
                                        oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields1, Values1);
                                        string id = "";
                                        string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + actNo2 + "'", 1);
                                        if (id1[0, 0] != "n")
                                        {
                                            id = id1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo2 + "'", " phc_leadcotactid='" + ld[i] + "'");
                                        string access = "";
                                        string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + ld[i] + "'", 1);
                                        if (access1[0, 0] != "n")
                                        {
                                            access = access1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + drpUser.SelectedValue.ToString() + "'", " cnt_internalid='" + ld[i] + "'");
                                        oDBEngine.SetFieldValue("tbl_trans_salesvisit", "slv_PreviousActivityId='" + dt_salesvisit.Rows[0]["ActivityNo"].ToString() + "'", " slv_id='" + dt_salesvisit.Rows[0]["slv_id"].ToString() + "'");
                                        for (int u = 0; u <= dt_salesvisit.Rows.Count - 1; u++)
                                        {
                                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_activityId='" + actNo2 + "'", " ofp_id='" + dt_salesvisit.Rows[u]["ProductId"].ToString() + "'");
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            for (int i = 0; i <= ld.GetUpperBound(0); i++)
                            {
                                DataTable dt_phonecall = new DataTable();
                                dt_phonecall = oDBEngine.GetDataTable("tbl_trans_phonecall", "*", " phc_leadcotactid='" + ld[i] + "'");
                                if (dt_phonecall != null)
                                {
                                    if (dt_phonecall.Rows.Count != 0)
                                    {
                                        string actNo4 = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                                        string bid = HttpContext.Current.Session["userbranchID"].ToString();
                                        string Fields2 = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                                        string Values2 = "'" + drpDepartment.SelectedItem.Value + "','4','" + actNo4 + "','" + Session["userid"].ToString() + "','" + drpUser.SelectedValue.ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartDate + "','" + sStartTime + "','" + sEndDate + "','" + sEndTime + "','" + TxtInstruction.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                                        oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields2, Values2);
                                        string id = "";
                                        string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + actNo4 + "'", 1);
                                        if (id1[0, 0] != "n")
                                        {
                                            id = id1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo4 + "',phc_allotedDate='" + oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString() + "',phc_allotuser='" + Session["userid"].ToString() + "',phc_alloteduser='" + drpUser.SelectedValue.ToString() + "'", " phc_leadcotactid='" + ld[i] + "'");
                                        string access = "";
                                        string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + ld[i] + "'", 1);
                                        if (access1[0, 0] != "n")
                                        {
                                            access = access1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + drpUser.SelectedValue.ToString() + "'", " cnt_internalid='" + ld[i] + "'");
                                        oDBEngine.InsurtFieldValue("tbl_trans_salesvisit", "slv_activityId,slv_branchid,slv_leadcotactid,slv_previousactivityId,slv_salesvisitoutcome,slv_nextvisitdatetime,CreateDate,CreateUser", "'" + id + "','" + drpBranch.SelectedItem.Value.ToString() + "','" + ld[i] + "','" + dt_phonecall.Rows[0]["phc_id"].ToString() + "','9','" + dt_phonecall.Rows[0]["phc_nextcall"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                                        oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_activityId='" + actNo4 + "'", " ofp_producttypeid=(select ofp_producttypeid from tbl_trans_offeredproduct where ofp_id=" + pid[i] + ") and ofp_leadid='" + ld[i] + "'");
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            //string popupScript = "";
            //popupScript += "<script language='javascript'>" + "alert('Successfully Done');";
            //popupScript += "close();";
            //popupScript += "</script>";
            ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script language='javaScript'>close();</script>");
            //Page.ClientScript.RegisterClientScriptBlock(GetType(), "Popup", "window.opener.location.reload(); window.self.close();", true);

        }
    }
}