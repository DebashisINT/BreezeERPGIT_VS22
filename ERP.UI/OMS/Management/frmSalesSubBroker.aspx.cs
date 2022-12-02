using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmSalesSubBroker : System.Web.UI.Page
    {
        clsDropDownList cls = new clsDropDownList();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DateTime new_startdate = new DateTime();
        DateTime new_starttime1 = new DateTime();
        string new_starttime;
        string new_enddate;
        string new_endtime;
        protected void Page_Load(object sender, EventArgs e)
        {
            rdrCall.Attributes.Add("onclick", "funChangeNext(this)");
            rdrVisit.Attributes.Add("onclick", "funChangeNext(this)");
            drpSalesStage.Attributes.Add("onchange", "functionSalesStage('" + drpSalesStage.ID + "')");
            chReg.Attributes.Add("onclick", "visibility(this)");
            if (!IsPostBack)
            {
                ASPxNextVisitDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ASPxvisitDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ASPxNextVisitDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxvisitDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string[,] conid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_contactlead_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
                if (conid[0, 0] != "n")
                {
                    Session["InternalId"] = conid[0, 0];
                }
                fillSalesStage();
                fillValues();
                showCallsDetails();
                FillNextVisitPlace();
            }
            Session["SalesID"] = Request.QueryString["id"].ToString();
            ViewState["ActivityId"] = "1";
            Page.ClientScript.RegisterStartupScript(GetType(), "JavaScript", "<script language='JavaScript'>height();</script>");
        }
        public void showCallsDetails()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_sales INNER JOIN tbl_trans_Activies ON tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id ", "tbl_trans_Activies.act_id as act_id,tbl_trans_Activies.act_assignedBy as act_assignedBy,(convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) as act_createDate,(convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) as act_actualStartDate,tbl_trans_Activies.act_priority as act_priority,convert(varchar(11),tbl_trans_Activies.act_scheduledDate ,113)as act_scheduledDate,tbl_trans_Activies.act_scheduledTime as act_scheduledTime,convert(varchar(11),tbl_trans_Activies.act_expectedDate ,113)as act_expectedDate,tbl_trans_Activies.act_expectedTime as act_expectedTime,tbl_trans_Activies.act_instruction as act_instruction", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            if (dt.Rows.Count != 0)
            {
                Session["newactivityid"] = dt.Rows[0]["act_id"].ToString();
                txtAlloatedBy.Text = getName(dt.Rows[0]["act_assignedBy"].ToString());
                txtDateOfAllottment.Text = dt.Rows[0]["act_createDate"].ToString();
                txtAcutalStart.Text = dt.Rows[0]["act_actualStartDate"].ToString();
                string s = dt.Rows[0]["act_priority"].ToString();
                switch (s)
                {
                    case "0":
                        txtPriority.Text = "Low";
                        break;
                    case "1":
                        txtPriority.Text = "Normal";
                        break;
                    case "2":
                        txtPriority.Text = "High";
                        break;
                    case "3":
                        txtPriority.Text = "Urgent";
                        break;
                    case "4":
                        txtPriority.Text = "Immediate";
                        break;
                }
                txtSeheduleStart.Text = dt.Rows[0]["act_scheduledDate"].ToString() + " " + dt.Rows[0]["act_scheduledTime"].ToString();
                txtTotalNumberofCalls.Text = getTotalNumberofCalls(Convert.ToInt32(dt.Rows[0]["act_id"].ToString())).ToString();
                txtSeheduleEnd.Text = dt.Rows[0]["act_expectedDate"].ToString() + " " + dt.Rows[0]["act_expectedTime"].ToString();
                lblShortNote.Text = dt.Rows[0]["act_instruction"].ToString();
                ShowCallInformation.Visible = true;
            }
            else
            {
                ShowCallInformation.Visible = false;
            }
        }
        public string getName(string id)
        {
            string name = "";
            DataTable dt = oDBEngine.GetDataTable("tbl_master_user", "user_name", " user_id='" + id + "'");
            if (dt.Rows.Count != 0)
            {
                name = dt.Rows[0][0].ToString();
            }
            else
            {
                name = "";
            }
            return name;
        }
        public int getTotalNumberofCalls(int actid)
        {
            int str = 0;
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "count(phc_id)", " phc_activityId='" + actid + "'");
            if (dt.Rows.Count != 0)
            {
                str = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return str;
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

            //btnEdit.Enabled = false;
            //btnPhoneFollowup.Enabled = true;
            //btnSave.Enabled = true;
            //btnDiscard.Enabled = true;
            //pnl.Enabled = true;
            //lblVisitDate.InnerText = "Visit Date";
            //lblVisitPurpose.Text = "Visit Purpose";
            //TdPlace.Visible = true;
            //TdPlace1.Visible = true;
            ////fillValues();
        }
        protected void btnPhoneFollowup_Click(object sender, EventArgs e)
        {
            ViewState["ActivityId"] = "2";
            Session["SalesID"] = Request.QueryString["id"].ToString();
            //string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_SExpenditure", "expnd_empId", " expnd_empId='" + Session["SalesID"].ToString() + "'", 1);
            //if (Id1[0, 0] != "n")
            //{
            //}
            //else
            //{
            //    oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Session["SalesID"].ToString() + "','1','1','1','1'");
            //    oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Session["SalesID"].ToString() + "','1','1','1','1'");
            //}
            //btnEdit.Enabled = true;
            //btnPhoneFollowup.Enabled = false;
            //btnSave.Enabled = true;
            //btnDiscard.Enabled = true;
            //pnl.Enabled = true;
            //lblVisitDate.InnerText = "Call Date";
            //lblVisitPurpose.Text = "Call Purpose";
            //TdPlace.Visible = false;
            //TdPlace1.Visible = false;
            //fillValues();
        }
        public void fillSalesStage()
        {
            string[,] sales = oDBEngine.GetFieldValue("tbl_master_SalesStatus", "sls_id,sls_status", " sls_id<>4", 2);
            if (sales[0, 0] != "n")
            {
                cls.AddDataToDropDownList(sales, drpSalesStage);
            }
        }
        public void fillValues()
        {
            string ID = Session["InternalId"].ToString();
            string ID2 = "";
            ID2 = ID.Substring(0, 2);
            DataTable dt = new DataTable();
            if (ID2 == "LD")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_salesDetails", "*,(SELECT cnt_firstName FROM tbl_master_lead WHERE cnt_internalId = sad_cntId) AS Name", " sad_salesId='" + Request.QueryString["id"].ToString() + "' order by sad_id desc");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_salesDetails", "*,(SELECT cnt_firstName FROM tbl_master_contact WHERE cnt_internalId = sad_cntId) AS Name", " sad_salesId='" + Request.QueryString["id"].ToString() + "' order by sad_id desc");
            }
            if (dt.Rows.Count != 0)
            {
                txtName.Text = dt.Rows[0]["Name"].ToString();
                //if (drpSalesStage.Items.FindByValue(dt.Rows[0]["sad_SaleStage"].ToString()).Value != null)
                //{
                drpSalesStage.SelectedValue = Convert.ToString(drpSalesStage.Items.FindByValue(dt.Rows[0]["sad_SaleStage"].ToString()).Value);
                //}
                txtNote.Text = dt.Rows[0]["sad_Notes"].ToString();
                ASPxNextVisitDate.Value = Convert.ToDateTime(dt.Rows[0]["sad_nextvisitdate"].ToString());
                ASPxvisitDate.Value = Convert.ToDateTime(dt.Rows[0]["sad_visitdate"].ToString());
                if (dt.Rows[0]["sad_reg"].ToString() == "Y")
                {
                    chReg.Checked = true;
                    //txtNameEBrok.Visible = true;
                    txtNameEBrok.Text = dt.Rows[0]["sad_ExistingBroker"].ToString();
                }
                else
                {
                    chReg.Checked = false;
                    //txtNameEBrok.Visible = false;
                }
                string[] yy = dt.Rows[0]["sad_accosiated"].ToString().Split('-');
                string mn = yy[0].ToString().Trim();
                drpmonth.SelectedValue = Convert.ToString(drpmonth.Items.FindByValue(mn).Value);
                txtAsso.Text = yy[1].ToString().Trim();
                txtTotEx.Text = dt.Rows[0]["sad_TotalExperience"].ToString();
                txtclient.Text = dt.Rows[0]["sad_ClientNo"].ToString();
                txTurnOver.Text = objConverter.getFormattedvalue(Convert.ToDecimal(dt.Rows[0]["sad_Turnover"].ToString()));
                txtsRatio.Text = dt.Rows[0]["sad_sRatio"].ToString();
                txtDSharing.Text = dt.Rows[0]["sad_dSharing"].ToString();
                txtPterms.Text = dt.Rows[0]["sad_PTerms"].ToString();
                txtTerms.Text = dt.Rows[0]["sad_Terms"].ToString();
                txtclientterms.Text = dt.Rows[0]["sad_TermsClient"].ToString();
                txtfeedback.Text = dt.Rows[0]["sad_feedBack"].ToString();
            }
            DataTable dt_info = new DataTable();
            if (ID2 == "LD")
            {
                dt_info = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_master_lead ON tbl_trans_Sales.sls_contactlead_id = tbl_master_lead.cnt_internalId  INNER JOIN tbl_master_branch ON tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_legalStatus ON tbl_master_lead.cnt_legalStatus = tbl_master_legalStatus.lgl_id", "tbl_master_lead.cnt_firstName + ' ' + isnull(tbl_master_lead.cnt_middlename,' ') + ' ' + isnull(tbl_master_lead.cnt_lastname,' ') AS Name, tbl_master_branch.branch_description, tbl_master_legalStatus.lgl_legalStatus, case isnull(cnt_referedby,'0') when '0' then 'N/A' else (select tbl_master_contact.cnt_firstname from tbl_master_contact where tbl_master_contact.cnt_internalid = tbl_master_lead.cnt_referedby) end as Introducer", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            }
            else
            {
                dt_info = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_master_contact ON tbl_trans_Sales.sls_contactlead_id = tbl_master_contact.cnt_internalId  INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_legalStatus ON tbl_master_contact.cnt_legalStatus = tbl_master_legalStatus.lgl_id", "tbl_master_contact.cnt_firstName + ' ' + isnull(tbl_master_contact.cnt_middlename,' ') + ' ' + isnull(tbl_master_contact.cnt_lastname,' ') AS Name, tbl_master_branch.branch_description, tbl_master_legalStatus.lgl_legalStatus, case isnull(cnt_referedby,'0') when '0' then 'N/A' else (select c.cnt_firstname from tbl_master_contact c where c.cnt_internalid = tbl_master_contact.cnt_referedby) end as Introducer", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            }
            if (dt_info.Rows.Count != 0)
            {
                txtName.Text = dt_info.Rows[0]["Name"].ToString();
                lblBranch.Text = dt_info.Rows[0]["branch_description"].ToString();
                lblLegalStatus.Text = dt_info.Rows[0]["lgl_legalStatus"].ToString();
                lblIntroducer.Text = dt_info.Rows[0]["Introducer"].ToString();
            }


        }
        public void FillNextVisitPlace()
        {
            string[,] NextVisitPlace = oDBEngine.GetFieldValue("tbl_master_address", "add_id,ISNULL(add_address1, '') + ' ' + ISNULL(add_address2, '') + ' ' + ISNULL(add_address3, '') AS ADDRESS", " add_cntId ='" + Session["InternalId"].ToString() + "'", 2);
            if (NextVisitPlace[0, 0] != "n")
            {
                cls.AddDataToDropDownList(NextVisitPlace, DrpNextVisitPlace);
                cls.AddDataToDropDownList(NextVisitPlace, DrpVisitPlace);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[,] conid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_contactlead_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
            string contactid = "";
            string branchid = "";
            int acttype = 0;
            string nextvisit = "";
            int nextacttype = 0;
            if (conid[0, 0] != "n")
            {
                contactid = conid[0, 0];
            }
            string[,] branid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_branch_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
            if (branid[0, 0] != "n")
            {
                branchid = branid[0, 0];
            }
            if (lblVisitDate.InnerText == "Call Date")
            {
                acttype = 1;
            }
            else
            {
                acttype = 2;
            }
            nextvisit = ASPxNextVisitDate.Value.ToString();
            if (drpSalesStage.SelectedValue == "2")
            {
                nextvisit = "";
            }
            if (rdrCall.Checked == true)
            {
                nextacttype = 1;
            }
            else
            {
                nextacttype = 2;
            }
            string nextvisitdate = ASPxNextVisitDate.Value.ToString();
            string visitdate = ASPxvisitDate.Value.ToString();

            string FieldName = "sad_cntId,sad_Branch,sad_FinancialYear,sad_SalesId,sad_SaleStage,sad_Notes,sad_reg, sad_ExistingBroker, sad_accosiated, sad_TotalExperience, sad_ClientNo, sad_Turnover, sad_sRatio, sad_dSharing,sad_PTerms, sad_Terms, sad_TermsClient, sad_feedBack,sad_visitdate,sad_visitpurpose,sad_nextvisitdate,sad_nextvisitpurpose,sad_nextactivityType,sad_nextvisitplace,sad_ActivityType,sad_visitplace,CreateDate,CreateUser";
            string reg = "";
            if (chReg.Checked == true)
            {
                reg = "Y";
            }
            else
            {
                reg = "N";
            }
            string AccoFrom = drpmonth.SelectedItem.Value.ToString().Trim() + "-" + txtAsso.Text.ToString();
            string FieldValues = "'" + contactid + "'," + branchid + ",'" + oDBEngine.GetDate().Year.ToString() + "'," + Request.QueryString["id"].ToString() + ",'" + drpSalesStage.Text.ToString() + "','" + txtNote.Text.ToString() + "','" + reg + "','" + txtNameEBrok.Text.ToString().Trim() + "','" + AccoFrom.ToString().Trim() + "','" + txtTotEx.Text.ToString() + "'," + txtclient.Text.ToString() + "," + txTurnOver.Text.ToString() + ",'" + txtsRatio.Text.ToString() + "','" + txtDSharing.Text.ToString() + "','" + txtPterms.Text.ToString() + "','" + txtTerms.Text.ToString() + "','" + txtclientterms.Text.ToString() + "','" + txtfeedback.Text.ToString() + "','" + visitdate + "','" + drpVisitPurpose.SelectedValue.ToString() + "','" + nextvisitdate + "','" + drpNextVisitPurpose.SelectedValue.ToString() + "'," + nextacttype + ",'" + DrpNextVisitPlace.SelectedItem.Value + "','" + ViewState["ActivityId"].ToString() + "','" + DrpVisitPlace.SelectedItem.Value + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
            int NoOfEffected = oDBEngine.InsurtFieldValue("tbl_trans_salesDetails", FieldName, FieldValues);
            DataTable dt1 = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate", " act_id='" + Session["newactivityid"].ToString() + "'");
            if (dt1.Rows.Count != 0)
            {
                oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " act_id='" + Session["newactivityid"].ToString() + "'");

            }
            if (NoOfEffected != 0)
            {
                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextvisitdate='" + ASPxNextVisitDate.Value.ToString() + "',sls_sales_status='" + drpSalesStage.SelectedValue.ToString() + "',sls_dateTime='" + oDBEngine.GetDate().ToString() + "',sls_nextvisitplace='" + DrpNextVisitPlace.SelectedItem.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " sls_id='" + Request.QueryString["id"].ToString() + "'");
            }
            if (drpSalesStage.SelectedValue.ToString() == "1" || drpSalesStage.SelectedValue.ToString() == "3")
            {
                string str_startnote = "";
                //if (drpSalesStage.SelectedValue.ToString() == "1")
                if (rdrCall.Checked == true)
                {
                    str_startnote = "Phone Call to  ";
                }
                else
                {
                    str_startnote = "Metting With  ";
                }
                try
                {
                    //new_startdate = Convert.ToDateTime(nextvisit).AddDays(-1);
                    new_startdate = Convert.ToDateTime(nextvisit);
                    new_starttime = Convert.ToDateTime(nextvisit).ToString();
                }
                catch
                {
                    new_startdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString()).AddDays(-1);
                    new_starttime = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                }
                string rem = "";
                string[,] rem1;
                string ID = Session["InternalId"].ToString();
                string ID1 = ID.Substring(0, 2);
                if (ID1 == "LD")
                {
                    rem1 = oDBEngine.GetFieldValue("tbl_master_lead", "ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'')", " cnt_internalid='" + Session["InternalId"].ToString() + "'", 1);
                }
                else
                {
                    rem1 = oDBEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'')", " cnt_internalid='" + Session["InternalId"].ToString() + "'", 1);
                }
                if (rem1[0, 0] != "n")
                {
                    rem = rem1[0, 0];
                }
                string Phno = "";
                string[,] phno1 = oDBEngine.GetFieldValue("tbl_master_phonefax", "phf_phoneNumber", " (phf_entity = 'Lead') AND (phf_type = 'mobile') AND (phf_cntId = '" + Session["InternalId"].ToString() + "')", 1);
                if (phno1[0, 0] != "n")
                {
                    Phno = phno1[0, 0];
                }

                setReminder(str_startnote + rem + " [ Phone Number :-" + Phno + " ]  AT " + new_starttime + " [" + txtNote.Text + "]");
                Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='Javascript'>alert('Save Successfull');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "Afetrsave", "<script language='Javascript'>ActionAfterSave();</script>");
            }
        }
        public void setReminder(string note)
        {
            string remId = "";
            if (Request.QueryString["id"] != null)
            {
                string[,] remid1 = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", " rem_sourceid='" + Request.QueryString["id"].ToString() + "'", 1);
                if (remid1[0, 0] != "n")
                {
                    remId = remid1[0, 0];
                }
            }
            string enddate = Convert.ToDateTime(new_startdate).AddDays(1).ToString();
            string endtime = Convert.ToDateTime(ASPxNextVisitDate.Value).TimeOfDay.ToString();
            //string new_endtime = Convert.ToDateTime(txtnextvisitdate.Text.ToString()).ToShortDateString();
            string new_endtime1 = Convert.ToDateTime(ASPxNextVisitDate.Value).AddMinutes(-30).ToString();

            //string StartDate = ViewState["SDate"].ToString() + " " + ViewState["STime"].ToString();
            string[] aa = new_endtime1.Split(' ');
            new_endtime1 = aa[1].ToString();
            string[] hh1 = aa[1].Split(':');
            if (aa[2].ToString() == "PM" && hh1[0].ToString() != "12")
            {
                string[] hh_mm = new_endtime1.Split(':');
                int hh = 12 + int.Parse(hh_mm.GetValue(0).ToString());
                new_endtime1 = hh + ":" + hh_mm[1];
            }
            new_endtime1 = aa[0].ToString() + " " + new_endtime1;
            string msg = "";
            //msg = txtNotes.Text;
            if (remId == "")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().Date + "','" + Session["userid"].ToString() + "','" + Convert.ToDateTime(new_endtime1) + "','" + Convert.ToDateTime(enddate) + "','" + note + "',1,0,'" + Request.QueryString["id"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_createUser='" + Session["userid"].ToString() + "',rem_createDate='" + oDBEngine.GetDate().ToString() + "',rem_targetUser='" + Session["userid"].ToString() + "',rem_startDate='" + Convert.ToDateTime(new_endtime1) + "',rem_endDate='" + Convert.ToDateTime(enddate) + "',rem_reminderContent='" + note + "',rem_displayTricker=1,rem_actionTaken=0,rem_sourceid='" + Request.QueryString["id"].ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rem_id='" + remId + "'");
            }
        }
    }
}