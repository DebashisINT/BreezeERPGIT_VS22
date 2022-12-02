using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Management
{
    public partial class management_CRMSalesVisit : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        Management_BL Management_BL = new Management_BL();
        clsDropDownList clsDropDownList = new clsDropDownList();
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                btnSavePhoneCallDetails.Attributes.Add("onclick", "return chkOnSaveClick123();");
                string today = objConverter.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
                //Image1.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_ASPxNextVisit,'dd/mm/yyyy hh:ii',ctl00_ContentPlaceHolder3_ASPxNextVisit,true,null,0,300)");
                //ASPxNextVisit.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_ASPxNextVisit ,'dd/mm/yyyy hh:ii',this,true,null,0,300)");
                //Image2.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_ASPxDateEdit,'dd/mm/yyyy hh:ii',ctl00_ContentPlaceHolder3_ASPxDateEdit,true,null,0,180)");
                //ASPxDateEdit.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_ASPxDateEdit ,'dd/mm/yyyy hh:ii',this,true,null,0,180)");            
                ASPxDateEdit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxNextVisit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

                BindActivityName();
                showdetailstbl.Visible = false;
                pnldatatbl.Visible = false;
                ShowDetails();
                BindVisitPlace();
                rdrCall.Attributes.Add("onclick", "funChangeNext(this)");
                rdrVisit.Attributes.Add("onclick", "funChangeNext(this)");
                BtnSelect();
                Lrd.Attributes.Add("onclick", "All_CheckedChanged();");
                Erd.Attributes.Add("onclick", "Specific_CheckedChanged();");

            }
            //ASPxNextVisit.Attributes.Add("readonly", "true");
            //ASPxDateEdit.Attributes.Add("readonly", "true");        
            BindActivityDetails();
        }
        public void BindActivityName()
        {
            try
            {
                string AllUser = oDBEngine.getChildUser_for_AllEmployee(Session["userid"].ToString(), "");
                //string AllUser1 = "";
                //string AllUser2 = "";
                //string[] st = AllUser.Split(',');
                //for (int i = 0; i <= st.GetUpperBound(0); i++)
                //{
                //    AllUser1 += "," + "'" + st[i] + "'";
                //    int ii = AllUser1.LastIndexOf(",,");
                //    AllUser2 = AllUser1.Substring(ii + 2);
                //}
                //int ii1 = AllUser2.LastIndexOf(",");
                //string All = AllUser2.Substring(0, ii1);
                //All = All + "," + "'" + Session["userid"].ToString() + "'";
                string[,] SelectActivity = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in (" + AllUser + ")", 2, "user_name");
                if (SelectActivity[0, 0] != "n")
                {
                    clsDropDownList.AddDataToDropDownList(SelectActivity, DDLActivity);
                    DDLActivity.SelectedValue = Session["userid"].ToString();
                }
            }
            catch
            {
            }
        }
        protected void DDLActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            BindActivityDetails();
            grdtbl.Visible = true;
            btntbl.Visible = true;
        }
        public void BindActivityDetails()
        {
            try
            {
                string ActivityId = DDLActivity.SelectedItem.Value;
                Session["ACtivityID"] = ActivityId;
                string Condition = "";
                if (Session["condition"] != null)
                {
                    Condition = "" + ActivityId + "" + Session["condition"].ToString() + "";
                }
                else
                {
                    Condition = "" + ActivityId + " and tbl_trans_salesVisit.slv_lastdatevisit IS NULL";
                }
                BindDatatable(Condition);
            }
            catch
            {
            }
        }
        public void BindDatatable(string Condition1)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Name");
            DataColumn col3 = new DataColumn("ActId");
            DataColumn col4 = new DataColumn("SalesVisitId");
            DataColumn col5 = new DataColumn("AssignBy");
            DataColumn col6 = new DataColumn("NextVisitDate");
            DataColumn col7 = new DataColumn("Address1");
            DataColumn col8 = new DataColumn("LastOutcome");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            dt.Columns.Add(col7);
            dt.Columns.Add(col8);
            if (Lrd.Checked == true)
            {
                dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_lead.cnt_internalid AS Id,ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')  + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + "  Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            }
            else
            {
                dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_contact.cnt_internalid AS Id,ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '')  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + "  Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            }
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow RowNew = dt.NewRow();
                    RowNew["Id"] = dt1.Rows[i][0];
                    RowNew["Name"] = dt1.Rows[i][1];
                    RowNew["ActId"] = dt1.Rows[i][2];
                    RowNew["SalesVisitId"] = dt1.Rows[i][3];
                    RowNew["AssignBy"] = dt1.Rows[i][4];
                    RowNew["NextVisitDate"] = dt1.Rows[i][5];
                    RowNew["Address1"] = dt1.Rows[i][6];
                    RowNew["LastOutcome"] = dt1.Rows[i][7];
                    dt.Rows.Add(RowNew);
                }
            }
            AspxActivity.DataSource = dt;
            AspxActivity.DataBind();


        }
        public void ShowDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                Session["phonecallid"] = null;
                Session["SalesVisitID"] = Request.QueryString["id2"].ToString();
                Session["SalesActivityId"] = Request.QueryString["id"].ToString();
                Session["InternalId"] = Request.QueryString["id1"].ToString();
                string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_salesExpenditure", "expnd_empId", " expnd_empId='" + Session["SalesVisitID"].ToString() + "'", 1);
                if (Id1[0, 0] != "n")
                {

                }
                else
                {
                    oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Session["SalesVisitID"].ToString() + "','1','1','1','1'");
                    oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Session["SalesVisitID"].ToString() + "','1','1','1','1'");
                }
                if (Session["ACtivityID"] != null)
                {
                    DDLActivity.SelectedValue = Session["ACtivityID"].ToString();
                    string Id = Request.QueryString["id"].ToString();
                    PnlShowDetails.Visible = true;
                    pnlData.Visible = true;
                    activityRow.Visible = false;
                    showdetailstbl.Visible = true;
                    pnldatatbl.Visible = true;
                    BindPanelDetails();
                }
                else
                {
                    BindPanelDetails();
                }
                btntbl.Visible = false;
                grdtbl.Visible = false;
            }
            else
            {
                if (Session["SalesActivityId"] != null)
                {
                    PnlShowDetails.Visible = true;
                    pnlData.Visible = true;
                    BindPanelDetails();
                }
            }
        }
        public void BindPanelDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                string Id = Request.QueryString["id"].ToString();
                string[,] Activity = oDBEngine.GetFieldValue("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_trans_Activies.act_activityNo AS ActNo, tbl_trans_Activies.act_instruction, tbl_trans_Activies.act_assignedBy AS AssignBy, tbl_trans_Activies.act_priority AS Priority,  (convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) AS CreateDate, (convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3))) AS ScheduleDate,  (convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3))) AS ExpDate, (convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) AS StartDate, tbl_trans_Activies.act_id,   tbl_master_user.user_name as AssignedBy", "tbl_trans_Activies.act_id=" + Id, 10);
                if (Activity[0, 0] != "n")
                {
                    lblShortNote.Text = Activity[0, 1];
                    string Priorty = Activity[0, 3];
                    switch (Priorty)
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
                    txtDateOfAllottment.Text = Activity[0, 4];
                    txtSeheduleStart.Text = Activity[0, 5];
                    txtSeheduleEnd.Text = Activity[0, 6];
                    txtAcutalStart.Text = Activity[0, 7];
                    txtAlloatedBy.Text = Activity[0, 9];
                }
                string LId = Request.QueryString["id1"].ToString();
                string[,] LActivity = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN   tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "tbl_trans_salesVisit.slv_leadcotactId, (convert(varchar(11),tbl_trans_salesVisit.slv_lastdatevisit,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 3))) as LastVisitDate, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate,  tbl_trans_salesVisit.slv_nextvisitplace as NextVisitPlace , tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as OutCome", "tbl_trans_salesVisit.slv_leadcotactId = '" + LId + "' AND tbl_trans_salesVisit.slv_activityId = '" + Id + "'", 5);
                if (LActivity[0, 0] != "n")
                {
                    lblLastVisit.Text = LActivity[0, 1];
                    if (lblLastVisit.Text == "")
                    {
                        lblLastVisit.Text = "N/A";
                    }
                    lblLastOutcome.Text = LActivity[0, 4];
                    if (lblLastOutcome.Text == "")
                    {
                        lblLastOutcome.Text = "N/A";
                    }
                    lblNextVisit.Text = LActivity[0, 2];
                    if (lblNextVisit.Text == "")
                    {
                        lblNextVisit.Text = "N/A";
                    }
                }
            }
        }
        public void BindVisitPlace()
        {
            if (Request.QueryString["id1"] != null)
            {
                string InterNalID = Request.QueryString["id1"].ToString();
                string[,] AddressData = oDBEngine.GetFieldValue("tbl_master_address", "add_id as Id,add_address1 as Address", " add_cntid='" + InterNalID + "'", 2, "add_address1");
                if (AddressData[0, 0] != "n")
                {
                    clsDropDownList.AddDataToDropDownList(AddressData, drpVisitPlace);
                    clsDropDownList.AddDataToDropDownList(AddressData, drpNextVisitPlace);
                }
            }
        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate = txtEndTime.Text;
                string enddate = sDate;
                ViewState["end"] = enddate.ToString();
                string startdate = txtStartDate.Text;
                ViewState["start"] = startdate.ToString();
                string SalesVisitId = Session["SalesVisitID"].ToString();
                string BranchId = HttpContext.Current.Session["userbranchID"].ToString();
                string VisitPlace = drpVisitPlace.SelectedItem.Value.ToString();
                string expenses = txtExp.Text.ToString();
                string OutCome = TxtOut.Text.ToString();
                int i = 0;
                string OutCome1 = "";
                try
                {
                    i = OutCome.LastIndexOf("!");
                    OutCome1 = OutCome.Substring(0, i);
                }
                catch
                {
                    i = OutCome.LastIndexOf("|");
                    OutCome1 = OutCome.Substring(0, i);
                }
                string Notes = txtNotes.Text.ToString();
                string NextVisit = objConverter.DateConverter_d_m_y(ASPxNextVisit.Text);
                // string NextVisit = ASPxNextVisit.Value.ToString();
                int activity = 0;
                if (rdrCall.Checked == true)
                {
                    activity = 1;
                }
                else
                {
                    activity = 2;
                }
                string CreateUser = Session["ACtivityID"].ToString();
                string CreateDate = oDBEngine.GetDate().ToString();
                string LastModifyUser = Session["userid"].ToString();
                string LastModifyDate = oDBEngine.GetDate().ToString();
                oDBEngine.InsurtFieldValue("tbl_trans_SalesVisitDetail", "slv_SalesVisitId,slv_Branchid,slv_VisitDateTime,slv_StartTime,slv_EndTime,slv_VisitPlace,slv_Expenses,slv_SalesVisitOutcome,slv_notes,slv_nextVisit,slv_nextactivityType,slv_ActivityType,CreateUser,CreateDate,LastModifyUser,LastModifyDate", "'" + SalesVisitId + "','" + BranchId + "','" + startdate + "','" + enddate + "','" + enddate + " ','" + VisitPlace + "','" + expenses + "','" + OutCome1 + "','" + Notes + "','" + NextVisit + "','" + activity + "','" + ViewState["ActivityType"].ToString() + "','" + CreateUser + "','" + CreateDate + "','" + LastModifyUser + "','" + LastModifyDate + "'");
                oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_lastdatevisit='" + startdate + "',slv_lastvisitPlace=" + VisitPlace + ",slv_lastvisitDuration=0,slv_salesvisitoutcome=" + OutCome1 + ",slv_nextvisitdatetime='" + NextVisit + "',slv_nextvisitplace='" + drpNextVisitPlace.SelectedItem.Value + "'", " slv_id=" + SalesVisitId);
                //oDBEngine.SetFieldValue("tbl_trans_Sales", "sls_nextvisitdate='" + NextVisit + "'", " sls_contactlead_id='" + Session["InternalId"].ToString() + "'");
                if (OutCome1 == "1" || OutCome1 == "2" || OutCome1 == "3" || OutCome1 == "5" || OutCome1 == "8" || OutCome1 == "9" || OutCome1 == "10" || OutCome1 == "11")
                {
                    //string a = enddate.Substring(3, 2);
                    //int a1 = Convert.ToInt32(a) - 1;
                    //string a2 = enddate.Substring(0, 2);
                    //string a3 = enddate.Substring(6, 4);
                    string New_Startdate = enddate;
                    string New_StartTime = enddate.Substring(11);
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();
                    string Activity = "";
                    try
                    {
                        ds = oDBEngine.PopulateData("TOP 1 CASE slv_nextActivityType WHEN '1' THEN ' Phone Call  ' WHEN '2' THEN 'Meeting With ' END as Activity", "tbl_trans_SalesVisitDetail", " (slv_SalesVisitId = '" + Session["SalesActivityId"].ToString() + "') order by slv_Id DESC");
                        Activity = ds.Tables[0].Rows[0]["Activity"].ToString();
                    }
                    catch
                    {
                    }
                    string Id1 = Session["InternalId"].ToString();
                    string id = Id1.Substring(0, 2);
                    if (id == "LD")
                    {
                        ds1 = oDBEngine.PopulateData("ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as Name", "tbl_master_lead", "cnt_internalid='" + Session["InternalId"].ToString() + "'");
                    }
                    else
                    {
                        ds1 = oDBEngine.PopulateData("ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as Name", "tbl_master_contact", "cnt_internalid='" + Session["InternalId"].ToString() + "'");
                    }
                    string Name1 = ds1.Tables[0].Rows[0]["Name"].ToString();
                    string PhNo = "";
                    try
                    {
                        ds2 = oDBEngine.PopulateData("phf_phoneNumber", "tbl_master_phonefax", "(phf_entity = 'Lead') AND (phf_type = 'mobile') AND (phf_cntId = '" + Session["InternalId"].ToString() + "')");
                        PhNo = ds2.Tables[0].Rows[0]["phf_phoneNumber"].ToString();
                    }
                    catch
                    {
                    }
                    string Rem = Activity + Name1 + "  [ Phone Number :- " + PhNo + " ]  AT " + enddate + " " + New_StartTime + " [" + txtNotes.Text + "]";
                    SetReminder(Rem);
                    if (OutCome1 == "8")
                    {
                        string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                        string Fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                        string Values = "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','6','" + actNo + "','" + Session["userid"].ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + oDBEngine.GetDate().ToShortTimeString() + "','" + ASPxDateEdit.Value + "','" + oDBEngine.GetDate().ToShortTimeString() + "','" + txtNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                        oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields, Values);
                        DataTable dt = oDBEngine.GetDataTable("tbl_trans_offeredproduct", "ofp_id", "ofp_leadid='" + Session["InternalId"].ToString() + "'");
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            DataTable dt_salesvisit = new DataTable();
                            dt_salesvisit = oDBEngine.GetDataTable("tbl_trans_offeredProduct", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_offeredProduct.ofp_probableAmount", " tbl_trans_offeredProduct.ofp_id=" + dt.Rows[k][0].ToString() + " and tbl_trans_offeredProduct.ofp_leadId='" + Session["InternalId"].ToString() + "'");
                            if (dt_salesvisit != null)
                            {
                                if (dt_salesvisit.Rows.Count != 0)
                                {
                                    string id2 = "";
                                    string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", " act_activityNo='" + actNo + "'", 1);
                                    if (id1[0, 0] != "n")
                                    {
                                        id2 = id1[0, 0];
                                    }
                                    string id3 = "";
                                    string[,] id4 = oDBEngine.GetFieldValue("tbl_trans_salesvisit", "slv_nextvisitdatetime", " slv_leadcotactId='" + Session["InternalId"].ToString() + "'", 1);
                                    if (id4[0, 0] != "n")
                                    {
                                        id3 = id4[0, 0];
                                    }
                                    string fields1 = "sls_activity_id, sls_contactlead_id, sls_branch_id, sls_sales_status, sls_date_closing, sls_ProductType ,sls_product, sls_estimated_value,sls_nextvisitdate, CreateDate,CreateUser";
                                    string values1 = "'" + id2 + "','" + Session["InternalId"].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','','" + dt_salesvisit.Rows[0]["ProductType"].ToString() + "','" + dt_salesvisit.Rows[0]["Product"].ToString() + "','" + dt_salesvisit.Rows[0]["ofp_probableAmount"].ToString() + "','" + id3 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                                    oDBEngine.InsurtFieldValue("tbl_trans_sales", fields1, values1);
                                    oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_salesactivityId='" + actNo + "'", " ofp_id='" + dt.Rows[k][0].ToString() + "'");
                                    oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + Session["InternalId"].ToString() + "'");
                                    string access = "";
                                    string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + Session["InternalId"].ToString() + "'", 1);
                                    if (access1[0, 0] != "n")
                                    {
                                        access = access1[0, 0];
                                    }
                                    string Sid = Session["InternalId"].ToString();
                                    string Sid1 = Sid.Substring(0, 2);
                                    if (Sid1 == "LD")
                                    {
                                        oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + Session["userid"].ToString() + "',cnt_status='" + actNo + "'", " cnt_internalid='" + Session["InternalId"].ToString() + "'");
                                    }
                                    else
                                    {
                                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Session["InternalId"].ToString() + "'");
                                    }
                                    oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", " tbl_trans_salesvisit.slv_leadcotactid='" + Session["InternalId"].ToString() + "'");
                                }
                            }
                        }
                    }

                }
                if (OutCome1 == "4" || OutCome1 == "12")
                {
                    DataTable dt = new DataTable();
                    bool flag = true;
                    dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_salesVisit.slv_nextvisitdatetime, tbl_trans_offeredProduct.ofp_probableAmount, tbl_trans_offeredProduct.ofp_id as ProductId", " tbl_trans_salesvisit.slv_id='" + Session["SalesVisitID"].ToString() + "'");
                    if (dt.Rows.Count != 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["ProductType"].ToString() == "Refreal Agent")
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        //string a = enddate.Substring(3, 2);
                        //int a1 = Convert.ToInt32(a) - 1;
                        //string a2 = enddate.Substring(0, 2);
                        //string a3 = enddate.Substring(6, 4);
                        string sStartdate = enddate;
                        string sStartTime = oDBEngine.GetDate().ToShortTimeString();//enddate.Substring(11);
                        string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                        string fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                        string values = HttpContext.Current.Session["userbranchID"].ToString() + ",'6','" + actNo + "','" + Session["userid"].ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartdate + "','" + sStartTime + "','" + sStartdate + "','" + sStartTime + "','" + txtNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                        oDBEngine.InsurtFieldValue("tbl_trans_activies", fields, values);
                        DataTable salesvisit = new DataTable();
                        salesvisit = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_salesVisit.slv_nextvisitdatetime, tbl_trans_offeredProduct.ofp_probableAmount, tbl_trans_offeredProduct.ofp_id as ProductId", " tbl_trans_salesvisit.slv_id='" + Session["SalesVisitID"].ToString() + "'");
                        if (salesvisit.Rows.Count != 0)
                        {
                            string id = "";
                            string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", "act_activityNo='" + actNo + "'", 1);
                            if (id1[0, 0] != "n")
                            {
                                id = id1[0, 0];
                            }
                            string field = "sls_activity_id, sls_contactlead_id, sls_branch_id, sls_sales_status, sls_date_closing, sls_ProductType ,sls_product, sls_estimated_value, sls_datetime";
                            string value = "'" + id + "','" + Session["SalesActivityId"].ToString() + "','" + Session["userbranchID"].ToString() + "','4','','" + salesvisit.Rows[0]["ProductType"].ToString() + "','" + salesvisit.Rows[0]["Product"].ToString() + "','" + salesvisit.Rows[0]["ofp_probableAmount"].ToString() + "','" + salesvisit.Rows[0]["slv_nextvisitdatetime"].ToString() + "'";
                            oDBEngine.InsurtFieldValue("tbl_trans_sales", field, value);
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_salesactivityId='" + actNo + "'", " ofp_id= '" + salesvisit.Rows[0]["productid"] + "'");
                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + Session["SalesActivityId"].ToString() + "'");
                            string access = "";
                            string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + Session["SalesActivityId"].ToString() + "'", 1);
                            if (access1[0, 0] != "n")
                            {
                                access = access1[0, 0];
                            }
                            string sid1 = Session["SalesActivityId"].ToString();
                            string sid = sid1.Substring(0, 2);
                            if (sid == "LD")
                            {
                                oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + Session["userid"].ToString() + "',cnt_status='" + actNo + "'", " cnt_internalid='" + Session["SalesActivityId"].ToString() + "'");
                            }
                            else
                            {
                                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Session["SalesActivityId"].ToString() + "'");
                            }
                            oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", "  tbl_trans_salesvisit.slv_leadcotactId='" + Session["SalesActivityId"].ToString() + "'");
                        }
                    }
                }
                BindActivityDetails();
                pnlData.Visible = false;
                PnlShowDetails.Visible = false;
                grdtbl.Visible = true;
                btntbl.Visible = true;
                if (Session["mode"] != null)
                {
                    string iid1 = Session["InternalId"].ToString();
                    string iid = iid1.Substring(0, 2);
                    if (iid == "LD")
                    {
                        oDBEngine.SetFieldValue("tbl_master_lead", "cnt_rating='" + Session["mode"].ToString() + "'", " cnt_internalId='" + Session["InternalId"].ToString() + "'");
                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_rating='" + Session["mode"].ToString() + "'", " cnt_internalId='" + Session["InternalId"].ToString() + "'");
                    }
                }
                oDBEngine.SystemGeneratedMails(SalesVisitId, "Sales Visit");
            }
            catch
            {

            }

        }
        public void SetReminder(string Note)
        {
            string[,] Rem_Id = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", "rem_sourceid='" + Session["SalesVisitID"].ToString() + "'", 1);
            string RemId = "";
            if (Rem_Id[0, 0] != "n")
            {
                RemId = Rem_Id[0, 0];
            }
            string new_StartDate = ViewState["start"].ToString();
            string new_endDate = ViewState["end"].ToString();
            string msg = txtNotes.Text;
            if (RemId == "")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Session["userid"].ToString() + "','" + new_endDate + "','" + new_StartDate + "','" + Note + "','1','0','" + Session["SalesVisitID"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Session["userid"].ToString() + "'");
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_createUser=" + Session["userid"].ToString() + ",rem_createDate='" + oDBEngine.GetDate().ToShortDateString() + "',rem_targetUser=" + Session["userid"].ToString() + ",rem_startDate='" + new_endDate + "',rem_endDate='" + new_StartDate + "',rem_reminderContent='" + Note + "',rem_displayTricker=1,rem_actionTaken=0,rem_sourceid='" + Session["SalesVisitID"].ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToShortDateString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rem_id=" + RemId);
            }
        }
        protected void btnSetReminder_Click(object sender, EventArgs e)
        {
            oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken", "'" + DDLActivity.SelectedItem.Value + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + DDLActivity.SelectedItem.Value + "','" + txtStartDate.Text + "','" + txtEndTime.Text + "','" + txtNotes.Text + "',1,0");
            txtOutCome.Text = "";
            txtNotes.Text = "";
            ASPxDateEdit.Text = "";
            ASPxNextVisit.Text = "";
            pnlData.Enabled = false;
        }
        protected void btnUpdateVisit_Click(object sender, EventArgs e)
        {
            ViewState["ActivityType"] = "1";
            Session["selectedsession"] = "FaceToFace";
            pnlData.Visible = true;
            pnlData.Enabled = true;
            btnSavePhoneCallDetails.Enabled = false;
            lblVisitDateTime.Text = "Visit DateTime";
            lblNextVisitDate.Text = "Next Visit DateTime";
            lblNextVisitPlace.Text = "Next Visit Place";
            btnUpdateVisit.ForeColor = System.Drawing.Color.Blue;
            btnPhoneFollowUP.ForeColor = System.Drawing.Color.Black;
            string next_date = oDBEngine.GetDate().ToString();
            string next_time = oDBEngine.GetDate().AddDays(1).ToString();
            //ASPxDateEdit.Text = objConverter.DateConverter(next_date, "dd/mm/yyyy hh:mm");
            //ASPxNextVisit.Text = objConverter.DateConverter(next_time, "dd/mm/yyyy hh:mm");
            ASPxNextVisit.Value = Convert.ToDateTime(next_time);
            ASPxDateEdit.Value = Convert.ToDateTime(next_date);

            txtOutCome.Attributes.Add("onclick", "calldispose('" + txtOutCome.ID + "','salesvisit')");
            ElementVisit(true);
        }
        protected void btnPhoneFollowUP_Click(object sender, EventArgs e)
        {
            ViewState["ActivityType"] = "2";
            Session["selectedsession"] = "PhoneFollowUp";
            pnlData.Visible = true;
            pnlData.Enabled = true;
            btnSavePhoneCallDetails.Enabled = false;
            lblVisitDateTime.Text = "Call DateTime";
            lblNextVisitDate.Text = "Next Call/Visit DateTime";
            lblNextVisitPlace.Text = "Visit Place";
            btnUpdateVisit.ForeColor = System.Drawing.Color.Black;
            btnPhoneFollowUP.ForeColor = System.Drawing.Color.Blue;
            string next_date = oDBEngine.GetDate().ToShortDateString();
            string next_time = oDBEngine.GetDate().AddDays(1).ToShortDateString();
            //ASPxDateEdit.Text = objConverter.DateConverter(next_date, "dd/mm/yyyy hh:mm");
            //ASPxNextVisit.Text = objConverter.DateConverter(next_time, "dd/mm/yyyy hh:mm");
            ASPxNextVisit.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(1).ToString());
            ASPxDateEdit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            txtOutCome.Attributes.Add("onclick", "calldispose('" + txtOutCome.ID + "','phonecall')");
            ElementVisit(false);
        }
        protected void BtnPending_Click(object sender, EventArgs e)
        {
            string ActivityId = DDLActivity.SelectedItem.Value;
            string Condition = " and tbl_trans_salesVisit.slv_lastdatevisit IS NULL";
            Session["condition"] = Condition.ToString();
            BindActivityDetails();
            AspxActivity.Columns[8].Visible = true;
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            Session["BtnSelect"] = "Pending";
            BtnPending.ForeColor = System.Drawing.Color.Blue;
            BtnOpen.ForeColor = System.Drawing.Color.Black;
            BtnClosed.ForeColor = System.Drawing.Color.Black;
            BtnConfirm.ForeColor = System.Drawing.Color.Black;
        }
        protected void BtnOpen_Click(object sender, EventArgs e)
        {

            string ActivityId = DDLActivity.SelectedItem.Value;
            string Condition = "and tbl_trans_salesVisit.slv_lastdatevisit IS Not NULL and tbl_Master_SalesVisitOutcomeCategory.Int_id in (1,2,3,5,8,9,10,11)";
            Session["condition"] = Condition.ToString();
            BindActivityDetails();
            AspxActivity.Columns[8].Visible = true;
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            Session["BtnSelect"] = "Open";
            BtnPending.ForeColor = System.Drawing.Color.Black;
            BtnOpen.ForeColor = System.Drawing.Color.Blue;
            BtnClosed.ForeColor = System.Drawing.Color.Black;
            BtnConfirm.ForeColor = System.Drawing.Color.Black;
        }
        protected void BtnClosed_Click(object sender, EventArgs e)
        {

            string ActivityId = DDLActivity.SelectedItem.Value;
            string Condition = "and tbl_Master_SalesVisitOutcomeCategory.Int_id in (6,7,13)";
            Session["condition"] = Condition.ToString();
            BindActivityDetails();
            AspxActivity.Columns[8].Visible = true;
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            Session["BtnSelect"] = "Closed";
            BtnPending.ForeColor = System.Drawing.Color.Black;
            BtnOpen.ForeColor = System.Drawing.Color.Black;
            BtnClosed.ForeColor = System.Drawing.Color.Blue;
            BtnConfirm.ForeColor = System.Drawing.Color.Black;
        }
        protected void BtnConfirm_Click(object sender, EventArgs e)
        {
            string ActivityId = DDLActivity.SelectedItem.Value;
            string Condition = "and tbl_Master_SalesVisitOutcomeCategory.Int_id in (4,12)";
            Session["condition"] = Condition.ToString();
            BindActivityDetails();
            AspxActivity.Columns[8].Visible = false;
            AspxActivity.Columns[9].Visible = true;
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            Session["BtnSelect"] = "Confirm";
            BtnPending.ForeColor = System.Drawing.Color.Black;
            BtnOpen.ForeColor = System.Drawing.Color.Black;
            BtnClosed.ForeColor = System.Drawing.Color.Black;
            BtnConfirm.ForeColor = System.Drawing.Color.Blue;
        }
        public void BtnSelect()
        {
            if (Session["BtnSelect"] != null)
            {
                string select = Session["BtnSelect"].ToString();
                switch (select)
                {
                    case "Pending":
                        BtnPending.ForeColor = System.Drawing.Color.Blue;
                        BtnOpen.ForeColor = System.Drawing.Color.Black;
                        BtnClosed.ForeColor = System.Drawing.Color.Black;
                        BtnConfirm.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "Open":
                        BtnPending.ForeColor = System.Drawing.Color.Black;
                        BtnOpen.ForeColor = System.Drawing.Color.Blue;
                        BtnClosed.ForeColor = System.Drawing.Color.Black;
                        BtnConfirm.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "Closed":
                        BtnPending.ForeColor = System.Drawing.Color.Black;
                        BtnOpen.ForeColor = System.Drawing.Color.Black;
                        BtnClosed.ForeColor = System.Drawing.Color.Blue;
                        BtnConfirm.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "Confirm":
                        BtnPending.ForeColor = System.Drawing.Color.Black;
                        BtnOpen.ForeColor = System.Drawing.Color.Black;
                        BtnClosed.ForeColor = System.Drawing.Color.Black;
                        BtnConfirm.ForeColor = System.Drawing.Color.Blue;
                        break;

                }
            }
            else
            {
                BtnPending.ForeColor = System.Drawing.Color.Blue;
            }
        }
        protected void BtnSCancel_Click(object sender, EventArgs e)
        {
            pnlData.Visible = false;
            PnlShowDetails.Visible = false;
            grdtbl.Visible = true;
            btntbl.Visible = true;
            activityRow.Visible = true;
        }
        public void ElementVisit(bool show)
        {
            drpVisitPlace.Visible = show;
            lblVisitPlace.Visible = show;
            lblVisitExp.Visible = show;
            txtExp.Visible = show;
        }
        protected void AspxActivity_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindActivityDetails();
        }
    }
}