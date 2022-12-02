using System;
using System.Data;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frmallot_sales_new : System.Web.UI.Page
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        clsDropDownList clsDropDown = new clsDropDownList();
        string[,] nextcall1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDownDepartment();
                FillDropDownBranch();
                FillDropDownUser();
            }
        }
        public void FillDropDownDepartment()
        {
            string[,] Deptt = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id,cost_description", " cost_costCenterType='department'", 2, "cost_description");
            if (Deptt[0, 0] != "n")
            {
                clsDropDown.AddDataToDropDownList(Deptt, drpDepartment);
            }
        }
        public void FillDropDownBranch()
        {
            string[,] Branch = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,branch_description", null, 2, "branch_description");
            if (Branch[0, 0] != "n")
            {
                clsDropDown.AddDataToDropDownList(Branch, drpBranch);
            }
        }
        public void FillDropDownUser()
        {
            string[,] User = oDBEngine.GetFieldValue("tbl_trans_employeeCTC INNER JOIN tbl_master_user ON tbl_trans_employeeCTC.emp_cntId = tbl_master_user.user_contactId", "tbl_master_user.user_id AS Id, tbl_master_user.user_name AS Name", " (tbl_trans_employeeCTC.emp_Department = " + drpDepartment.SelectedValue.ToString() + ") AND (tbl_master_user.user_branchId = " + drpBranch.SelectedValue.ToString() + ")", 2, "tbl_master_user.user_name");
            if (User[0, 0] != "n")
            {
                clsDropDown.AddDataToDropDownList(User, drpUser);
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
            string CallingFrom = Request.QueryString["Calling"].ToString();
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
                            leadid += String.Format(",{0}", sub_str.ToString().Trim().Substring((sub_str.ToString().Trim().Length - 10), 10));
                        }
                    }
                    if (s.Length > 1)
                    {
                        if (s.GetValue(4) != "")
                        {
                            if (i == 0)
                            {
                                productid = s.GetValue(4).ToString();
                            }
                            else
                            {
                                if (productid.ToString() == "")
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

                    if (CallingFrom == "PhoneCall")
                    {
                        nextcall1 = oDBEngine.GetFieldValue("tbl_trans_phonecall INNER JOIN tbl_trans_offeredProduct ON    tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_offeredProduct.ofp_activityId = tbl_trans_Activies.act_activityNo AND tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", "phc_nextcall", " phc_leadcotactid='" + ld[i] + "'", 1);
                    }
                    else if (CallingFrom == "SalesVisit")
                    {
                        nextcall1 = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_offeredProduct.ofp_activityId = tbl_trans_Activies.act_activityNo AND tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "slv_nextvisitdatetime", " slv_leadcotactid='" + ld[i] + "'", 1);
                    }
                    else if (CallingFrom == "Sales")
                    {
                        nextcall1 = oDBEngine.GetFieldValue("tbl_trans_sales INNER JOIN tbl_trans_offeredProduct ON tbl_trans_sales.sls_contactlead_id = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_offeredProduct.ofp_activityId = tbl_trans_Activies.act_activityNo AND tbl_trans_sales.sls_activity_id = tbl_trans_Activies.act_id", "sls_nextvisitdate", " sls_contactlead_id='" + ld[i] + "'", 1);
                    }
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


                    DateTime dDate1 = mindate;
                    string sStartDate = dDate1.ToShortDateString();
                    string sStartTime = dDate1.ToShortTimeString();
                    ViewState["SDate"] = sStartDate.ToString();
                    ViewState["STime"] = sStartTime.ToString();
                    dDate1 = maxdate;
                    string sEndDate = dDate1.ToShortDateString();
                    string sEndTime = dDate1.ToShortTimeString();
                    string[] pid = productid.Split(',');
                    if (Session["selectedbutton"] == null)
                    {
                        Session["selectedbutton"] = "";
                    }
                    string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                    string Fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                    string Values = "'" + drpBranch.SelectedItem.Value + "','6','" + actNo + "','" + Session["userid"].ToString() + "','" + drpUser.SelectedValue.ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartDate + "','" + sStartTime + "','" + sEndDate + "','" + sEndTime + "','" + TxtInstruction.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                    oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields, Values);
                    //for (int j = 0; j <= ld.GetUpperBound(0); j++)
                    //{
                    DataTable dt_salesvisit = new DataTable();

                    if (CallingFrom == "PhoneCall")
                    {
                        dt_salesvisit = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_nextcall, tbl_trans_offeredProduct.ofp_probableAmount", " tbl_trans_offeredProduct.ofp_id=" + pid[i] + " and tbl_trans_phonecall.phc_leadcotactid='" + ld[i] + "'");
                    }
                    else if (CallingFrom == "SalesVisit")
                    {
                        dt_salesvisit = oDBEngine.GetDataTable(" tbl_trans_salesvisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesvisit.slv_activityid = tbl_trans_Activies.act_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesvisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_salesvisit.slv_leadcotactId, tbl_trans_salesvisit.slv_nextvisitdatetime, tbl_trans_offeredProduct.ofp_probableAmount", " tbl_trans_offeredProduct.ofp_id=" + pid[i] + " and tbl_trans_salesvisit.slv_leadcotactid='" + ld[i] + "'");
                    }
                    else if (CallingFrom == "Sales")
                    {
                        string oft_act;
                        oft_act = oDBEngine.GetFieldValue("tbl_trans_offeredproduct", "ofp_actid", "ofp_id='" + pid[0] + "'", 1)[0, 0];
                        dt_salesvisit = oDBEngine.GetDataTable(" tbl_trans_sales INNER JOIN tbl_trans_Activies ON tbl_trans_sales.sls_activity_id = tbl_trans_Activies.act_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_sales.sls_contactlead_id = tbl_trans_offeredProduct.ofp_leadId", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_sales.sls_contactlead_id, tbl_trans_sales.sls_nextvisitdate, tbl_trans_offeredProduct.ofp_probableAmount", " tbl_trans_offeredProduct.ofp_actid='" + oft_act + "' and tbl_trans_sales.sls_contactlead_id='" + ld[i] + "'");
                    }

                    if (dt_salesvisit != null)
                    {
                        if (dt_salesvisit.Rows.Count != 0)
                        {
                            string id = "";
                            string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + actNo + "'", 1);
                            if (id1[0, 0] != "n")
                            {
                                id = id1[0, 0];
                            }
                            string fields1 = "sls_activity_id, sls_contactlead_id, sls_branch_id, sls_sales_status, sls_date_closing, sls_ProductType ,sls_product, sls_estimated_value, sls_nextvisitdate,CreateDate,CreateUser";
                            string values1 = "";
                            if (CallingFrom == "PhoneCall")
                            {
                                values1 = "'" + id + "','" + ld[i] + "','" + drpBranch.SelectedItem.Value + "','4','','" + dt_salesvisit.Rows[0]["ProductType"].ToString() + "','" + dt_salesvisit.Rows[0]["Product"].ToString() + "','" + dt_salesvisit.Rows[0]["ofp_probableAmount"].ToString() + "','" + dt_salesvisit.Rows[0]["phc_nextcall"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                            }
                            else if (CallingFrom == "SalesVisit")
                            {
                                values1 = "'" + id + "','" + ld[i] + "','" + drpBranch.SelectedItem.Value + "','4','','" + dt_salesvisit.Rows[0]["ProductType"].ToString() + "','" + dt_salesvisit.Rows[0]["Product"].ToString() + "','" + dt_salesvisit.Rows[0]["ofp_probableAmount"].ToString() + "','" + dt_salesvisit.Rows[0]["slv_nextvisitdatetime"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                            }
                            else if (CallingFrom == "Sales")
                            {
                                values1 = "'" + id + "','" + ld[i] + "','" + drpBranch.SelectedItem.Value + "','4','','" + dt_salesvisit.Rows[0]["ProductType"].ToString() + "','" + dt_salesvisit.Rows[0]["Product"].ToString() + "','" + dt_salesvisit.Rows[0]["ofp_probableAmount"].ToString() + "','" + dt_salesvisit.Rows[0]["sls_nextvisitdate"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                            }
                            oDBEngine.InsurtFieldValue("tbl_trans_sales", fields1, values1);
                            if (CallingFrom == "Sales")
                            {
                                oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_activityId='" + actNo + "'", " ofp_id='" + pid[0] + "'");
                            }
                            else
                            {
                                oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_activityId='" + actNo + "'", " ofp_id='" + pid[i] + "'");
                            }
                            if (CallingFrom == "PhoneCall")
                            {
                                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + ld[i] + "'");
                            }
                            else if (CallingFrom == "SalesVisit")
                            {
                                oDBEngine.SetFieldValue("tbl_trans_salesvisit", "slv_NextActivityId='" + actNo + "',slv_activityId='" + id + "'", " slv_leadcotactid='" + ld[i] + "'");
                            }
                            else if (CallingFrom == "Sales")
                            {
                                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_PreviousActivityId='" + actNo + "',sls_activity_id='" + id + "'", " sls_contactlead_id='" + ld[i] + "'");
                            }
                            string access = "";
                            string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + ld[i] + "'", 1);
                            if (access1[0, 0] != "n")
                            {
                                access = access1[0, 0];
                            }
                            string Sid = ld[i];
                            string Sid1 = Sid.Substring(0, 2);
                            if (Sid1 == "LD")
                            {
                                oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + drpUser.SelectedValue.ToString() + "',cnt_status='" + actNo + "'", " cnt_internalid='" + ld[i] + "'");
                            }
                            else
                            {
                                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + ld[i] + "'");
                            }
                            //oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", " tbl_trans_salesvisit.slv_leadcotactid='" + ld[i] + "'");
                            string enddate = ViewState["SDate"].ToString();
                            string endtime = Convert.ToDateTime(dDate1).TimeOfDay.ToString();
                            string new_endtime = Convert.ToDateTime(enddate).AddDays(1).ToShortDateString();
                            string new_endtime1 = Convert.ToDateTime(ViewState["STime"].ToString()).AddMinutes(-30).ToString();

                            string[] aa = new_endtime1.Split(' ');
                            new_endtime1 = aa[1].ToString();
                            string[] hh1 = aa[1].Split(':');
                            if (aa[2].ToString() == "PM" && hh1[0].ToString() != "12")
                            {
                                string[] hh_mm = new_endtime1.Split(':');
                                int hh = 12 + int.Parse(hh_mm.GetValue(0).ToString());
                                new_endtime1 = hh + ":" + hh_mm[1];
                            }
                            string[,] access2 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_firstname+''+cnt_middlename+''+cnt_lastname", " cnt_internalid='" + ld[i] + "'", 1);

                            string note = "Meeting with " + access2[0, 0] + " AT " + sStartDate + " " + sStartTime + "[" + TxtInstruction.Text + "]";
                            oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + drpUser.SelectedValue.ToString() + "','" + enddate + " " + new_endtime1 + "','" + new_endtime + " " + endtime + "','" + note + "',1,0,'" + id + "','" + oDBEngine.GetDate().ToString() + "','0'");
                            oDBEngine.messageTableUpdate(drpUser.SelectedValue.ToString(), Session["userid"].ToString(), "Sales", sStartDate + sStartTime, sEndDate + endtime, TxtInstruction.Text, TxtInstruction.Text, id, "activity");
                        }

                    }

                    //}

                }

                string popupScript = "";
                popupScript += "<script language='javascript'>" + "alert('Successfully Done');";
                popupScript += "parent.editwin.close();";
                popupScript += "</script>";
                ClientScript.RegisterStartupScript(GetType(), "JScript", popupScript);
            }
        }
    }
}