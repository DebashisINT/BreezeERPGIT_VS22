using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frm_SalesVisitActivity : System.Web.UI.Page
    {
        clsDropDownList oclsDropDownList = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable dtUserInfo = new DataTable();
        string dStartDate = System.DateTime.Now.ToShortDateString();
        string dEndDate = System.DateTime.Now.AddDays(1).ToShortDateString();
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

            string today = objConverter.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
            try
            {
                drpPriority.SelectedValue = "1";
            }
            catch
            {
            }
            if (!IsPostBack)
            {
                ViewState["val"] = "";
                BinddropDown();
                drpActType.SelectedValue = "4";
                FillGridView();
                if (Request.QueryString["id"] != null)
                {
                    FillActivityDetails();
                }
                TxtStartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                TxtEndDate.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(1).ToString());
                FromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }

        }
        public void BinddropDown()
        {
            string tempUser = "";
            string userid = oDBEngine.getChildUser(Session["userid"].ToString(), tempUser);
            userid = userid + "," + Session["userid"].ToString();
            string[,] actType = oDBEngine.GetFieldValue("tbl_master_activitytype", "aty_id,aty_activityType", null, 2, "aty_activityType");
            if (actType[0, 0] != "n")
            {
                oclsDropDownList.AddDataToDropDownList(actType, drpActType);
            }
            string[,] userWork = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_Id in(" + userid + ")", 2, "user_name");
            if (userWork[0, 0] != "n")
            {
                oclsDropDownList.AddDataToDropDownList(userWork, drpUserWork);
            }
        }
        public void FillGridView()
        {
            string temp = "";
            string userId1 = oDBEngine.getChildUser(Session["userid"].ToString(), temp);
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN    tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id", "tbl_trans_Activies.act_id AS id, tbl_master_user.user_name AS name, tbl_master_user.user_id AS userid,  ISNULL(tbl_trans_salesVisit.slv_lastdatevisit, '') AS LastVisitDate,(isnull(tbl_trans_Activies.act_scheduledDate,'') + ' ' + isnull(tbl_trans_Activies.act_scheduledTime,'')) as SchDate, ISNULL(tbl_trans_salesVisit.slv_nextvisitdatetime, '') AS nextVisitDate,   tbl_trans_salesVisit.slv_salesvisitoutcome AS outcomeId, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome AS Outcome,tbl_trans_Activies.act_scheduledDate AS SchEndDate,  CASE tbl_trans_salesvisit.slv_salesvisitoutcome WHEN 9 THEN 9 WHEN 9 THEN 9 ELSE     (SELECT     tbl_Master_SalesVisitOutcomeCategory.Int_id    FROM          tbl_Master_SalesVisitOutcomeCategory INNER JOIN   tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category WHERE      tbl_master_SalesVisitOutCome.slv_Id = tbl_trans_salesvisit.slv_salesvisitoutcome) END AS outcomeCat", " tbl_trans_Activies.act_assignedTo in(" + userId1 + ") order by tbl_master_user.user_id,tbl_trans_salesVisit.slv_lastdatevisit,tbl_trans_Activies.act_id");
            DataRow row = dt.NewRow();
            DataColumn colUserid = new DataColumn("UserId");
            DataColumn colSN = new DataColumn("SNo");
            DataColumn colUser = new DataColumn("User");
            DataColumn colPenAct = new DataColumn("Pending Acttivity");
            DataColumn colSchExpDt = new DataColumn("Scheduled End Date");
            DataColumn colExpEndDt = new DataColumn("Expected End Date");
            DataColumn colPenCall = new DataColumn("Pending Call");
            DataColumn colInterested_Pipeline = new DataColumn("Int/Pipeline");
            DataColumn colRefixed_ByFOS = new DataColumn("Refixed By FOS");
            DataColumn colRefixed_ByClient = new DataColumn("Refixed By Client");
            DataColumn colWonConfirm_Sale = new DataColumn("Won/Confirm Sale");
            DataColumn colNon_Contactable = new DataColumn("Non Contactable");
            DataColumn colNon_Usable_Fake = new DataColumn("Non Usable/Fake");
            DataColumn colLost_Not_Interested = new DataColumn("Lost/Not Int");
            DataColumn colRefixed_By_TeleCaller = new DataColumn("Refixed By TeleCaller");
            dt.Columns.Add(colUserid);
            dt.Columns.Add(colSN);
            dt.Columns.Add(colUser);
            dt.Columns.Add(colPenAct);
            dt.Columns.Add(colSchExpDt);
            dt.Columns.Add(colExpEndDt);
            dt.Columns.Add(colPenCall);
            dt.Columns.Add(colInterested_Pipeline);
            dt.Columns.Add(colRefixed_ByFOS);
            dt.Columns.Add(colRefixed_ByClient);
            dt.Columns.Add(colWonConfirm_Sale);
            dt.Columns.Add(colNon_Contactable);
            dt.Columns.Add(colNon_Usable_Fake);
            dt.Columns.Add(colLost_Not_Interested);
            dt.Columns.Add(colRefixed_By_TeleCaller);
            dt.Rows.Add(row);
            dtUserInfo = dt.Clone();
            int userid = 0;
            int PendingAct = 0;
            int PenCall = 0;
            int InterestedPipeline = 0;
            int Reflexedbyfos = 0;
            int ReflexedByClient = 0;
            int WonConfirmSale = 0;
            int NonContactable = 0;
            int NonUsable = 0;
            int Lost = 0;
            int ReflexedbyTellecaller = 0;
            //System.DateTime stsrtDate = new DateTime();
            int count = 0;
            int countAct = 1;
            int sNo = 0;
            string actId = "";
            System.DateTime dDate = new DateTime();
            System.DateTime dDate1 = new DateTime();
            System.DateTime dTempDate = new DateTime();
            //bool tTempVal = false;
            if (dt1 != null & dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    PendingAct = 0;
                    PenCall = 0;
                    InterestedPipeline = 0;
                    Reflexedbyfos = 0;
                    ReflexedByClient = 0;
                    WonConfirmSale = 0;
                    NonContactable = 0;
                    NonUsable = 0;
                    Lost = 0;
                    ReflexedbyTellecaller = 0;
                    count = 0;
                    if (dt1.Rows[i][4].ToString() != "")
                    {
                        dDate = Convert.ToDateTime(dt1.Rows[i][4].ToString());
                    }
                    count = 0;
                    countAct = 1;
                    actId = dt1.Rows[i][0].ToString();
                    userid = Convert.ToInt32(dt1.Rows[i][2].ToString());
                    string Name = dt1.Rows[i][1].ToString();

                    if (dt1.Rows[i]["LastVisitDate"].ToString() == "")
                    {
                        string Name1 = dt1.Rows[i][1].ToString();
                        while (Name.Trim() == Name1.Trim())
                        {
                            int condition = Convert.ToInt32(dt1.Rows[i]["outcomeId"].ToString());
                            string filterRow = Convert.ToString(dt1.Select("Id=" + dt1.Rows[i][0] + "And LastVisitDate is null"));
                            if (filterRow.Length > 0)
                            {
                                switch (condition)
                                {
                                    case 1:
                                        InterestedPipeline += 1;
                                        break;
                                    case 2:
                                        Reflexedbyfos += 1;
                                        break;
                                    case 3:
                                        ReflexedByClient += 1;
                                        break;
                                    case 4:
                                        WonConfirmSale += 1;
                                        break;
                                    case 5:
                                        NonContactable += 1;
                                        break;
                                    case 6:
                                        NonUsable += 1;
                                        break;
                                    case 7:
                                        Lost += 1;
                                        break;
                                    case 8:
                                        ReflexedbyTellecaller += 1;
                                        break;
                                }
                                if (dt1.Rows[i]["LastVisitDate"].ToString() == "")
                                {
                                    count += 1;
                                }
                                if (oDBEngine.IsDate(dStartDate) == false)
                                {
                                    dStartDate = dt1.Rows[i]["SchDate"].ToString();
                                }
                                if (actId != dt1.Rows[i][0].ToString())
                                {
                                    countAct += 1;
                                    actId = dt1.Rows[i][0].ToString();
                                }
                                if (dt1.Rows[i][4].ToString() != "")
                                {
                                    dTempDate = Convert.ToDateTime(dt1.Rows[i][4].ToString());
                                }
                                dDate1 = Convert.ToDateTime(dt1.Rows[i][8].ToString());
                                i = i + 1;
                                if (dt1.Rows.Count > i)
                                {
                                    Name1 = dt1.Rows[i][1].ToString();
                                }
                                if (i > dt1.Rows.Count - 1) break;
                            }

                        }
                        i = i - 1;
                        DataRow row1 = dtUserInfo.NewRow();
                        sNo += 1;
                        int totalCall = 0;
                        //DateTime dTempDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                        do
                        {
                            dt.Clear();
                            dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "COUNT(*)", " tbl_trans_Activies.act_assignedTo='" + userid + "'");
                            if (dt.Rows.Count > 0)
                            {
                                totalCall = Convert.ToInt32(dt.Rows[0][0]);
                                if (totalCall == 0)
                                {
                                    if (dTempDate > oDBEngine.GetDate().AddDays(-50))
                                    {
                                        dTempDate = dTempDate.AddDays(-1);
                                    }
                                }
                            }
                            else
                            {
                                if (dTempDate > oDBEngine.GetDate().AddDays(-50))
                                {
                                    dTempDate = dTempDate.AddDays(-1);
                                }
                            }
                        } while (totalCall == 0);
                        //string dExpectedEnddate = "";
                        if (dTempDate.ToString() == oDBEngine.GetDate().ToShortDateString())
                        {
                            DateTime dtemptime = new DateTime(oDBEngine.GetDate().Year, oDBEngine.GetDate().Month, oDBEngine.GetDate().Day, 10, 0, 0);
                            DateTime dCurrentTime = oDBEngine.GetDate();
                            TimeSpan different = dCurrentTime - dtemptime;
                            double dDiff = different.TotalSeconds;
                        }
                        row1["UserId"] = userid;
                        row1["SNo"] = sNo;
                        row1["User"] = Name;
                        row1["Pending Call"] = count;
                        row1["Pending Acttivity"] = countAct;
                        row1["Scheduled End Date"] = objConverter.ArrangeDate(dDate1.ToShortDateString());
                        row1["Expected End Date"] = objConverter.ArrangeDate(oDBEngine.getEndDate(dStartDate, (count + InterestedPipeline + Reflexedbyfos + ReflexedByClient + WonConfirmSale + NonContactable + NonUsable + Lost), (InterestedPipeline + Reflexedbyfos + ReflexedByClient + WonConfirmSale + NonContactable + NonUsable + Lost)));
                        row1["Int/Pipeline"] = InterestedPipeline;
                        row1["Refixed By FOS"] = Reflexedbyfos;
                        row1["Refixed By Client"] = ReflexedByClient;
                        row1["Won/Confirm Sale"] = WonConfirmSale;
                        row1["Non Contactable"] = NonContactable;
                        row1["Non Usable/Fake"] = NonUsable;
                        row1["Lost/Not Int"] = Lost;
                        row1["Refixed By TeleCaller"] = ReflexedbyTellecaller;
                        dtUserInfo.Rows.Add(row1);
                    }

                }
            }
            grdUserInfo.DataSource = dtUserInfo.DefaultView;
            grdUserInfo.DataBind();
        }
        protected void grdUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUserInfo.PageIndex = e.NewPageIndex;
            FillGridView();
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "CreateNew";
            frmShowCall.Visible = true;
            pnlCall.Visible = true;
            userInfo1.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            grdActivityDetail.Visible = false;
        }
        public void FillActivityData()
        {
            DataTable dt = new DataTable();
            ViewState["check"] = null;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                GridViewRow row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)row.FindControl("chkDetail");
                if (chkDetail.Checked == true)
                {
                    ViewState["check"] = "1";
                    Label lbl = (Label)row.FindControl("lblActNo");
                    dt.Clear();
                    dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId", "tbl_trans_Activies.act_activityType as Type, tbl_trans_Activies.act_assignedTo AS userId, Isnull(tbl_trans_Activies.act_scheduledDate,'') AS StartDate, isnull(tbl_trans_Activies.act_expectedDate,'') AS EndDate,   Isnull(tbl_trans_Activies.act_instruction,'') AS Instruction, tbl_trans_Activies.act_priority AS Priority, isnull(tbl_trans_Activies.act_description,'') AS Description,tbl_trans_salesVisit.slv_id AS PhoneCallid,isnull(tbl_trans_Activies.act_scheduledTime,'') as StartTime, isnull(tbl_trans_Activies.act_expectedTime,'') as EndTime", " tbl_trans_Activies.act_id ='" + lbl.Text + "'");
                    if (dt != null)
                    {
                        drpActType.SelectedItem.Value = dt.Rows[0]["Type"].ToString();
                        drpUserWork.SelectedItem.Value = dt.Rows[0]["userId"].ToString();
                        string dDate = objConverter.DateConverter(dt.Rows[0]["StartDate"].ToString(), "mm/dd/yyyy") + " " + dt.Rows[0]["StartTime"].ToString();
                        TxtStartDate.Value = "";
                        TxtStartDate.Value = Convert.ToDateTime(dDate);
                        string dDate1 = objConverter.DateConverter(dt.Rows[0]["EndDate"].ToString(), "mm/dd/yyyy") + " " + dt.Rows[0]["EndTime"].ToString();
                        TxtEndDate.Value = "";
                        TxtEndDate.Value = Convert.ToDateTime(dDate1);
                        txtDesc.Text = dt.Rows[0]["Description"].ToString();
                        txtInstNote.Text = dt.Rows[0]["Instruction"].ToString();
                        drpPriority.SelectedItem.Value = dt.Rows[0]["Priority"].ToString();
                        pnlCall.Visible = true;
                        pnlActivityDetail.Visible = false;
                        grdDetail.Visible = false;
                    }
                    ViewState["edit"] = lbl.Text;
                }
            }
            if (ViewState["check"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('At First Check Activity')</script>");
            }
            if (dt != null)
            {
                string temp = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp += dt.Rows[i]["PhoneCallid"] + ",";
                }
                temp = oDBEngine.sepComma(temp);
                ViewState["ids"] = temp;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string id = "";
            string ActNo = "";
            string temp1 = "";
            string column = "";
            DataTable dt = new DataTable();
            string sStartDate1 = Convert.ToDateTime(TxtStartDate.Value.ToString()).ToShortDateString();
            string sStartTime = Convert.ToDateTime(TxtStartDate.Value.ToString()).ToShortTimeString();
            string sEndDate = Convert.ToDateTime(TxtEndDate.Value.ToString()).ToShortDateString();
            string sEndTime = Convert.ToDateTime(TxtEndDate.Value.ToString()).ToShortTimeString();
            switch (ViewState["status"].ToString())
            {
                case "CreateNew":
                    try
                    {
                        if (Session["lead"] != null)
                        {
                            ActNo = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                            int rowCount = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,CreateDate,CreateUser", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedItem.Value + "','" + txtDesc.Text + "','" + Session["userid"].ToString() + "','" + drpUserWork.SelectedItem.Value + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Session["Count"].ToString() + "','" + sStartDate1 + "','" + sEndDate + "','" + ActNo + "','" + drpPriority.SelectedItem.Value + "','" + sStartTime + "','" + sEndTime + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                            if (rowCount != 0)
                            {
                                string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max( act_id)", null, 1);
                                if (id1[0, 0] != "n")
                                {
                                    id = id1[0, 0];
                                }
                                string[] temp = Session["lead"].ToString().Split(',');
                                foreach (string chrk in temp)
                                {
                                    string[] chrk1 = chrk.Split('|');
                                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + chrk1.GetValue(0).ToString() + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }
                                    if (chrk1.GetValue(5).ToString() == "")
                                    {
                                        string ofpId = "";
                                        oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", "ofp_actId, ofp_leadId, ofp_productTypeId, ofp_productId, ofp_probableAmount,CreateDate,CreateUser", "'" + ActNo + "','" + chrk1.GetValue(0).ToString() + "','" + chrk1.GetValue(2).ToString() + "','" + chrk1.GetValue(3).ToString() + "','0','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                                        string[,] ofpId1 = oDBEngine.GetFieldValue("tbl_trans_offeredProduct", "max( ofp_id)", null, 1);
                                        if (ofpId1[0, 0] != "n")
                                        {
                                            ofpId = ofpId1[0, 0];
                                        }
                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser", "'" + id + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + ofpId + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser", "'" + id + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + ofpId + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                                        }
                                        string slvId = "";
                                        string[,] slvId1 = oDBEngine.GetFieldValue("tbl_trans_salesVisit", "max( slv_id)", null, 1);
                                        if (slvId1[0, 0] != "n")
                                        {
                                            slvId = slvId1[0, 0];
                                        }
                                        // oDBEngine.SetFieldValue("tbl_master_address", "add_activityId='" + slvId + "'", " add_id='" + chrk1.GetValue(2).ToString() + "'");
                                    }
                                    else
                                    {
                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_nextvisitdatetime,slv_productoffered", "'" + id + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + nextvisitdateTime + "','" + chrk1.GetValue(5).ToString() + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered", "'" + id + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + nextvisitdateTime + "','" + chrk1.GetValue(5).ToString() + "'");
                                        }
                                        //  oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_actId='" + ActNo + "'", " ofp_id='" + chrk1.GetValue(5).ToString() + "'");
                                    }
                                    if (Session["call"] != null)
                                    {
                                        if (Session["call"].ToString() == "lead")
                                        {
                                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ActNo + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ActNo + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + ActNo + "'", " phc_NextActivityId='allot' And phc_leadcotactId='" + chrk1.GetValue(0).ToString() + "'");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    break;
                case "Modify":
                    if (Session["lead"].ToString() == "")
                    {
                        temp1 = ViewState["ids"].ToString();
                    }
                    else
                    {
                        temp1 = ViewState["ids"].ToString() + "," + Session["lead"].ToString();
                    }
                    string[] tmp = temp1.Split(',');
                    column += "act_branchId=" + HttpContext.Current.Session["userbranchID"].ToString();
                    column += ",act_activityType='" + drpActType.SelectedItem.Value + "'";
                    column += ",act_description='" + txtDesc.Text + "'";
                    column += ",act_instruction='" + txtInstNote.Text + "'";
                    column += ",act_assignedBy='" + Session["userid"].ToString() + "'";
                    column += ",act_assignedTo='" + drpUserWork.SelectedItem.Value + "'";
                    column += ",act_contactlead='" + tmp.Length + "'";
                    column += ",act_scheduledDate='" + sStartDate1 + "'";
                    column += ",act_expectedDate='" + sEndDate + "'";
                    column += ",act_priority='" + drpPriority.SelectedItem.Value + "'";
                    column += ",act_scheduledTime='" + sStartTime + "'";
                    column += ",act_expectedTime='" + sEndTime + "'";
                    if (Session["lead"].ToString() == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", column, "  act_id='" + ViewState["edit"].ToString() + "'");
                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", column, "  act_id='" + ViewState["edit"].ToString() + "'");
                        string[] temp = Session["lead"].ToString().Split(',');
                        foreach (string chrk in temp)
                        {
                            string[] chrk1 = chrk.Split('|');
                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,CreateDate,CreateUser", "'" + ViewState["edit"].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "',9,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                            DataTable rdr = oDBEngine.GetDataTable("tbl_trans_Activies", "act_activityNo, act_id", " act_id ='" + ViewState["edit"].ToString() + "'");
                            id = rdr.Rows[0]["act_id"].ToString();
                            if (Session["call"].ToString() == "lead")
                            {
                                oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                            }
                            else
                            {
                                oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + ViewState["edit"].ToString() + "'", " phc_NextActivityId='allot'");
                            }
                        }
                    }
                    break;
                case "Reassign":
                    if (Session["lead"].ToString() == "")
                    {
                        temp1 = ViewState["ids"].ToString();
                    }
                    else
                    {
                        temp1 = ViewState["ids"].ToString() + "," + Session["lead"].ToString();
                    }
                    string[] tmp1 = temp1.Split(',');
                    column += "act_branchId=" + HttpContext.Current.Session["userbranchID"].ToString();
                    column += ",act_activityType='" + drpActType.SelectedItem.Value + "'";
                    column += ",act_description='" + txtDesc.Text + "'";
                    column += ",act_instruction='" + txtInstNote.Text + "'";
                    column += ",act_assignedBy='" + Session["userid"].ToString() + "'";
                    column += ",act_assignedTo='" + drpUserWork.SelectedItem.Value + "'";
                    column += ",act_contactlead='" + tmp1.Length + "'";
                    column += ",act_scheduledDate='" + sStartDate1 + "'";
                    column += ",act_expectedDate='" + sEndDate + "'";
                    column += ",act_priority='" + drpPriority.SelectedItem.Value + "'";
                    column += ",act_scheduledTime='" + sStartTime + "'";
                    column += ",act_expectedTime='" + sEndTime + "'";
                    oDBEngine.SetFieldValue("tbl_trans_Activies", column, " act_id='" + ViewState["edit"].ToString() + "'");
                    string[] temp2 = Session["lead"].ToString().Split(',');
                    foreach (string chrk in temp2)
                    {
                        string[] chrk1 = chrk.Split('|');
                        oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,CreateDate,CreateUser", "'" + ViewState["edit"].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "',9,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                        DataTable rdr = oDBEngine.GetDataTable("tbl_trans_Activies", "act_activityNo, act_id", " act_id ='" + ViewState["edit"].ToString() + "'");
                        id = rdr.Rows[0]["act_id"].ToString();
                        if (Session["call"].ToString() == "lead")
                        {
                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                        }
                        else
                        {
                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + ViewState["edit"].ToString() + "'", " phc_NextActivityId='allot'");
                        }
                    }
                    break;
                case "Reschedule":
                    if (Session["lead"].ToString() == "")
                    {
                        temp1 = ViewState["ids"].ToString();
                    }
                    else
                    {
                        temp1 = ViewState["ids"].ToString() + "," + Session["lead"].ToString();
                    }
                    string[] tmp2 = temp1.Split(',');
                    column += "act_branchId=" + HttpContext.Current.Session["userbranchID"].ToString();
                    column += ",act_activityType='" + drpActType.SelectedItem.Value + "'";
                    column += ",act_description='" + txtDesc.Text + "'";
                    column += ",act_instruction='" + txtInstNote.Text + "'";
                    column += ",act_assignedBy='" + Session["userid"].ToString() + "'";
                    column += ",act_assignedTo='" + drpUserWork.SelectedItem.Value + "'";
                    column += ",act_contactlead='" + tmp2.Length + "'";
                    column += ",act_scheduledDate='" + sStartDate1 + "'";
                    column += ",act_expectedDate='" + sEndDate + "'";
                    column += ",act_priority='" + drpPriority.SelectedItem.Value + "'";
                    column += ",act_scheduledTime='" + sStartTime + "'";
                    column += ",act_expectedTime='" + sEndTime + "'";
                    oDBEngine.SetFieldValue("tbl_trans_Activies", column, " act_id='" + ViewState["edit"].ToString() + "'");
                    string[] temp3 = Session["lead"].ToString().Split(',');
                    foreach (string chrk in temp3)
                    {
                        string[] chrk1 = chrk.Split('|');
                        oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,CreateDate,CreateUser", "'" + ViewState["edit"].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "',9,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                        DataTable rdr = oDBEngine.GetDataTable("tbl_trans_Activies", "act_activityNo, act_id", " act_id ='" + ViewState["edit"].ToString() + "'");
                        id = rdr.Rows[0]["act_id"].ToString();
                        if (Session["call"].ToString() == "lead")
                        {
                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                        }
                        else
                        {
                            oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + ViewState["edit"].ToString() + "'", " cnt_internalid='" + chrk1.GetValue(0).ToString() + "'");
                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + ViewState["edit"].ToString() + "'", " phc_NextActivityId='allot'");
                        }
                    }
                    break;
            }
            Response.Redirect("frm_SalesVisitActivity.aspx");
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Modify";
            FillActivityData();
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
        }
        protected void btnReassign_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Reassign";
            FillActivityData();
            drpUserWork.Enabled = true;
        }
        protected void btnReschedule_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Reschedule";
            FillActivityData();
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
        }
        protected void btnshowGenratedSaleVisit_Click(object sender, EventArgs e)
        {
            frmAllot.Visible = false;
            btnSelectAll.Visible = false;
            btnExport.Visible = false;
            lblTotalRecord.Text = "";
            string fdate = "";
            string productcon = "";
            string drpselectedval = "";
            string drpselectedproduct = "";
            string leadid = "";
            string todate = "";
            if (e.ToString() != "")
            {
                fdate = objConverter.DateConverter(FromDate.Value.ToString(), "dd/mm/yyyy");
                todate = objConverter.DateConverter(ToDate.Value.ToString(), "dd/mm/yyyy");
                if (Radio1.Checked == true)
                {
                    productcon = "ALL";
                }
                else
                {
                    productcon = "";
                }
                drpselectedproduct = drpProduct.SelectedItem.ToString();
                drpselectedval = drpSelect.SelectedItem.ToString();
            }
            if (productcon == "ALL")
            {
                Radio1.Checked = true;
            }
            else
            {
                Radio2.Checked = true;
                drpProduct.SelectedValue = Convert.ToString(drpProduct.Items.FindByText(drpselectedproduct).Value);
            }
            drpSelect.SelectedValue = Convert.ToString(drpSelect.Items.FindByText(drpselectedval).Value);
            if (fdate == "" || todate == "")
            {
                lblError.Visible = true;
                lblError.Text = "Please select the From and todate";
            }
            else
            {
                lblError.Visible = false;
                lblError.Text = "";
            }
            fdate += " 01:00 AM";
            todate += " 11:55 PM";
            if (productcon == "ALL")
            {
                productcon = "";
            }
            else
            {
                productcon = drpselectedproduct;
            }
            string all_user = oDBEngine.getChildUser_for_report(Session["userid"].ToString(), "") + Session["userid"].ToString();
            DataTable dt = new DataTable();
            if (productcon == "")
            {
                if (ViewState["GSales"] != null)
                {
                    if (Lrd.Checked == true)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_salesVisit.slv_salesvisitoutcome= tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId and tbl_trans_offeredProduct.ofp_activityId=tbl_trans_Activies.act_activityNo INNER JOIN tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_lead.cnt_internalId = tbl_master_address.add_cntId ", "distinct convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_lead.cnt_firstName,'') + ' ' + isnull(tbl_master_lead.cnt_middleName,'') + ' ' +  isnull(tbl_master_lead.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + "))  and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) <= CONVERT(varchar(10),'" + todate + "', 101))");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_salesVisit.slv_salesvisitoutcome= tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId and tbl_trans_offeredProduct.ofp_activityId=tbl_trans_Activies.act_activityNo INNER JOIN tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId ", "distinct convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + "))  and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) <= CONVERT(varchar(10),'" + todate + "', 101))");
                    }
                }
                else
                {
                    if (Lrd.Checked == true)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId INNER JOIN tbl_master_address INNER JOIN tbl_master_lead ON tbl_master_address.add_cntId = tbl_master_lead.cnt_internalId ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_trans_salesvisit.slv_leadcotactid, tbl_trans_offeredProduct.ofp_productTypeId AS Product, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' +  ISNULL(tbl_master_address.add_pin, '') AS Address, convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId,tbl_trans_Activies.act_assignedBy as Assignedby,tbl_trans_Activies.act_assignedto as act_assignedTo, tbl_trans_offeredProduct.ofp_Id as ProductId", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) <= CONVERT(varchar(10),'" + todate + "', 101)) and (tbl_master_address.add_activityId IS NOT NULL)");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId INNER JOIN tbl_master_address INNER JOIN tbl_master_contact ON tbl_master_address.add_cntId = tbl_master_contact.cnt_internalId ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId", "tbl_trans_salesvisit.slv_leadcotactid, tbl_trans_offeredProduct.ofp_productTypeId AS Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' +  ISNULL(tbl_master_address.add_pin, '') AS Address, convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId,tbl_trans_Activies.act_assignedBy as Assignedby,tbl_trans_Activies.act_assignedto as act_assignedTo, tbl_trans_offeredProduct.ofp_Id as ProductId", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) <= CONVERT(varchar(10),'" + todate + "', 101)) and (tbl_master_address.add_activityId IS NOT NULL)");
                    }
                }
            }
            else
            {
                if (ViewState["GSales"] != null)
                {
                    if (Lrd.Checked == true)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_salesVisit.slv_salesvisitoutcome= tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId and tbl_trans_offeredProduct.ofp_activityId=tbl_trans_Activies.act_activityNo INNER JOIN tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_lead.cnt_internalId = tbl_master_address.add_cntId ", "distinct convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_lead.cnt_firstName,'') + ' ' + isnull(tbl_master_lead.cnt_middleName,'') + ' ' +  isnull(tbl_master_lead.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_trans_salesVisit.slv_salesvisitoutcome=8) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "') and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) <= CONVERT(varchar(10),'" + todate + "', 101))");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_salesVisit.slv_salesvisitoutcome= tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId and tbl_trans_offeredProduct.ofp_activityId=tbl_trans_Activies.act_activityNo INNER JOIN tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId ", "distinct convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_trans_salesVisit.slv_salesvisitoutcome=8) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "') and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (convert(varchar(10),tbl_trans_salesVisit.slv_nextvisitdatetime,101) <= CONVERT(varchar(10),'" + todate + "', 101))");
                    }
                    //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId INNER JOIN tbl_master_address INNER JOIN tbl_master_lead ON tbl_master_address.add_cntId = tbl_master_lead.cnt_internalId ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_trans_salesvisit.slv_leadcotactid, tbl_trans_offeredProduct.ofp_productTypeId AS Product, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' +  ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_salesVisit.slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_Activies.act_assignedBy as Assignedby,tbl_trans_Activies.act_assignedto as act_assignedTo, tbl_trans_offeredProduct.ofp_Id as ProductId", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) <= CONVERT(varchar(10),'" + todate + "', 101)) and (ofp_producttypeid='" + drpselectedproduct + "') and (tbl_master_address.add_activityId IS NOT NULL)");
                }
                else
                {
                    if (Lrd.Checked == true)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId INNER JOIN tbl_master_address INNER JOIN tbl_master_lead ON tbl_master_address.add_cntId = tbl_master_lead.cnt_internalId ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_trans_salesvisit.slv_leadcotactid, tbl_trans_offeredProduct.ofp_productTypeId AS Product, ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' +  ISNULL(tbl_master_address.add_pin, '') AS Address, convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_Activies.act_assignedBy as Assignedby,tbl_trans_Activies.act_assignedto as act_assignedTo, tbl_trans_offeredProduct.ofp_Id as ProductId", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) <= CONVERT(varchar(10),'" + todate + "', 101)) and (ofp_producttypeid='" + drpselectedproduct + "') and (tbl_master_address.add_activityId IS NOT NULL)");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId INNER JOIN tbl_master_address INNER JOIN tbl_master_contact ON tbl_master_address.add_cntId = tbl_master_contact.cnt_internalId ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId", "tbl_trans_salesvisit.slv_leadcotactid, tbl_trans_offeredProduct.ofp_productTypeId AS Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] ' +  ISNULL(tbl_master_address.add_pin, '') AS Address, convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113) as slv_nextvisitdatetime, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_Activies.act_assignedBy as Assignedby,tbl_trans_Activies.act_assignedto as act_assignedTo, tbl_trans_offeredProduct.ofp_Id as ProductId", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) >= CONVERT(varchar(10),'" + fdate + "', 101)) and (CAST(tbl_trans_Activies.act_scheduledDate AS datetime) <= CONVERT(varchar(10),'" + todate + "', 101)) and (ofp_producttypeid='" + drpselectedproduct + "') and (tbl_master_address.add_activityId IS NOT NULL)");
                    }
                }
            }
            DataTable dt_main = new DataTable();
            dt_main.Columns.Add("NextVisit DateTime");
            dt_main.Columns.Add("LeadId");
            dt_main.Columns.Add("Name");
            dt_main.Columns.Add("Address");
            dt_main.Columns.Add("PhoneNo");
            dt_main.Columns.Add("Product");
            dt_main.Columns.Add("Assigned By");
            dt_main.Columns.Add("Alloted Department");
            dt_main.Columns.Add("Alloted Branch");
            dt_main.Columns.Add("Alloted User");
            dt_main.Columns.Add("Status");
            dt_main.Columns.Add("ProductId");
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    lblError.Visible = true;
                    lblError.Text = "There Is No record";
                    grdGenratedSalesVisit.DataSource = dt.DefaultView;
                    grdGenratedSalesVisit.DataBind();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drnew = dt_main.NewRow();
                    drnew[0] = dt.Rows[i]["slv_nextvisitdatetime"].ToString();
                    drnew[1] = dt.Rows[i]["slv_leadcotactid"].ToString();
                    drnew[2] = dt.Rows[i]["Name"].ToString();
                    drnew[3] = dt.Rows[i]["Address"].ToString();
                    string product = "";
                    string PTypeId = dt.Rows[i]["Product"].ToString();
                    switch (PTypeId)
                    {
                        case "Broking And DP Account":
                            product = "Broking And DP Account";
                            break;
                        case "Refreal Agent":
                            product = "Refreal Agent";
                            break;
                        case "Sub Broker":
                            product = "Sub Broker";
                            break;
                        default:
                            DataTable dtProduct = new DataTable();
                            dtProduct = oDBEngine.GetDataTable("tbl_trans_OfferedProduct", "CASE (isnull(ofp_productId, '0')) WHEN '0' THEN isnull(ofp_productTypeId, '') ELSE (SELECT     prds_description FROM tbl_master_products WHERE      (prds_internalId = tbl_trans_offeredproduct.ofp_productid)) END AS Product", " ofp_productTypeId='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["slv_leadcotactid"].ToString() + "'");
                            if (dtProduct != null)
                            {
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    if (j != 0)
                                    {
                                        product += "  (" + j + 1 + ")" + dtProduct.Rows[j]["Product"].ToString();
                                    }
                                    else
                                    {
                                        product = dtProduct.Rows[j]["Product"].ToString();
                                    }
                                }
                            }
                            break;
                    }
                    string phoneno = "";
                    DataTable dt_phonecall = new DataTable();
                    dt_phonecall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["slv_leadcotactid"].ToString() + "'");
                    if (dt_phonecall != null)
                    {
                        for (int ij = 0; ij < dt_phonecall.Rows.Count; ij++)
                        {
                            switch (dt_phonecall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    phoneno += " (M)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    phoneno += " (R)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    phoneno += " (O)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    if (phoneno == "")
                    {
                        phoneno = "N/A";
                    }
                    drnew[4] = phoneno;
                    drnew[5] = product;
                    string[,] uname = oDBEngine.GetFieldValue("tbl_master_user", "User_name", " user_id=" + dt.Rows[i]["act_assignedTo"].ToString(), 1);
                    if (uname[0, 0] != "n")
                    {
                        drnew[6] = uname[0, 0];
                    }
                    DataTable dt_temp = new DataTable();
                    if (ViewState["GSales"] != null)
                    {
                        dt_temp = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_trans_Activies ON tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id INNER JOIN tbl_master_SalesStatus ON tbl_trans_Sales.sls_sales_status = tbl_master_SalesStatus.sls_id INNER JOIN  tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactid INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_master_employee.emp_contactId = tbl_master_user_1.user_contactId INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id", "tbl_master_SalesStatus.sls_status as slv_SalesVisitOutcome,tbl_trans_employeeCTC.emp_reportTo, tbl_master_user.user_name, tbl_master_user_1.user_name AS ReportingHead, tbl_master_branch.branch_description, tbl_master_costCenter.cost_description", " tbl_trans_Sales.sls_contactlead_id='" + dt.Rows[i]["slv_leadcotactid"].ToString() + "' and (tbl_trans_sales.sls_ProductType='" + dt.Rows[i]["Product"].ToString() + "')");
                    }
                    else
                    {
                        dt_temp = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id INNER JOIN tbl_master_branch ON tbl_trans_salesVisit.slv_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId AND  tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId", "tbl_master_user.user_name, tbl_master_costCenter.cost_description, tbl_master_branch.branch_description,  tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome, tbl_master_user.user_name AS ReportingHead", " (tbl_trans_salesVisit.slv_leadcotactId = '" + dt.Rows[i]["slv_leadcotactid"].ToString() + "')  AND (tbl_trans_offeredProduct.ofp_activityId IS NOT NULL) and (tbl_trans_offeredproduct.ofp_productTypeid='" + dt.Rows[i]["Product"].ToString() + "')");
                    }
                    if (dt_temp != null)
                    {
                        if (dt_temp.Rows.Count == 0)
                        {
                            drnew[7] = "N/A";
                            drnew[8] = "N/A";
                            drnew[8] = "N/A";
                            drnew[10] = "Due";
                        }
                        else
                        {
                            if (leadid == "")
                            {
                                leadid = i.ToString();
                            }
                            else
                            {
                                leadid += "," + i;
                            }
                            drnew[7] = dt_temp.Rows[0]["cost_description"].ToString();
                            drnew[8] = dt_temp.Rows[0]["branch_description"].ToString();
                            drnew[9] = dt_temp.Rows[0]["user_name"].ToString() + "[" + dt_temp.Rows[0]["ReportingHead"].ToString() + "]";
                            drnew[10] = dt_temp.Rows[0]["slv_SalesVisitOutcome"].ToString();
                        }
                    }
                    string[,] ofp_id = oDBEngine.GetFieldValue("tbl_trans_offeredProduct", "top 1 ofp_id", " ofp_producttypeid='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["slv_leadcotactid"].ToString() + "'", 1);
                    if (ofp_id[0, 0] != "n")
                    {
                        drnew[11] = ofp_id[0, 0];
                    }
                    switch (drpselectedval)
                    {
                        case "All":
                            dt_main.Rows.Add(drnew);
                            break;
                        case "Assigned":
                            if (drnew[7].ToString() != "N/A")
                            {
                                dt_main.Rows.Add(drnew);
                            }
                            break;
                        case "UnAssigned":
                            if (drnew[7].ToString() == "N/A")
                            {
                                dt_main.Rows.Add(drnew);
                            }
                            break;
                    }
                }
            }
            lblTotalRecord.Text = "Total Record  : " + dt_main.Rows.Count;
            grdGenratedSalesVisit.DataSource = dt_main.DefaultView;
            grdGenratedSalesVisit.DataBind();
            ViewState["tempDt"] = dt_main;
            frmAllot.Visible = true;
            btnSelectAll.Visible = true;
            btnExport.Visible = true;
            DataTable temp_dt = new DataTable();
            temp_dt.Columns.Add("NextVisit DateTime");
            temp_dt.Columns.Add("LeadId");
            temp_dt.Columns.Add("Name");
            temp_dt.Columns.Add("Address");
            temp_dt.Columns.Add("PhoneNo");
            temp_dt.Columns.Add("Product");
            temp_dt.Columns.Add("Assigned By");
            temp_dt.Columns.Add("Alloted Department");
            temp_dt.Columns.Add("Alloted Branch");
            temp_dt.Columns.Add("Alloted User");
            temp_dt.Columns.Add("Status");
            for (int i = 0; i < dt_main.Rows.Count; i++)
            {
                DataRow dr = temp_dt.NewRow();
                dr[0] = dt_main.Rows[i][0].ToString();
                dr[1] = dt_main.Rows[i][1].ToString();
                dr[2] = dt_main.Rows[i][2].ToString();
                dr[3] = dt_main.Rows[i][3].ToString();
                dr[4] = dt_main.Rows[i][4].ToString();
                dr[5] = dt_main.Rows[i][5].ToString();
                dr[6] = dt_main.Rows[i][6].ToString();
                dr[7] = dt_main.Rows[i][7].ToString();
                dr[8] = dt_main.Rows[i][8].ToString();
                dr[9] = dt_main.Rows[i][9].ToString();
                dr[10] = dt_main.Rows[i][10].ToString();
                temp_dt.Rows.Add(dr);
            }
            Session["GridDateReport"] = "";
            Session["FileName"] = "";
            Session["GridDateReport"] = temp_dt;
            Session["FileName"] = "Genrated Sales Visit From " + fdate + " To " + todate;
            string LeadID1 = txtLeadId.Value;
            if (ViewState["GSales"] != null)
            {
                if (LeadID1 != null)
                {
                    for (int g = 0; g <= grdGenratedSalesVisit.Rows.Count - 1; g++)
                    {
                        string[] str = LeadID1.Split(',');
                        for (int i = 0; i <= str.GetUpperBound(0); i++)
                        {
                            if (str[i] != "")
                            {
                                string[] id = str[i].Split('@');
                                Label lbl = (Label)grdGenratedSalesVisit.Rows[g].FindControl("lblId");
                                string[] l = lbl.Text.Split('@');
                                if (l[0] == id[0] && l[4] == id[1])
                                {
                                    CheckBox chk = (CheckBox)grdGenratedSalesVisit.Rows[g].FindControl("chkSel");
                                    chk.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            if (dt_main.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('No Record Found')</script>");
                frmAllot.Visible = false;
                btnSelectAll.Visible = false;
                btnExport.Visible = false;
            }
        }
        protected void btnReallocateSalesVisit_Click(object sender, EventArgs e)
        {
            frmShowGenratedSalesvisit.Visible = true;
            frmShowCall.Visible = false;
            pnlShowDetail.Visible = false;
            userInfo1.Visible = false;
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            btnMainCancel.Visible = true;
            ViewState["GSales"] = null;
            frmAllot.Visible = false;
            Td1.Visible = true;
        }
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (btnSelectAll.Text == "Deselect All")
            {
                btnSelectAll.Text = "Select All";
                txtLeadId.Value = "";
                foreach (GridViewRow row in grdGenratedSalesVisit.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSel");
                    if (chk.Checked == true)
                    {
                        chk.Checked = false;
                    }
                }
                return;
            }
            else
            {
                btnSelectAll.Text = "Deselect All";
                btnshowGenratedSaleVisit_Click(sender, e);
                DataTable dt = (DataTable)ViewState["tempDt"];
                ViewState["val"] = "Yes";
                grdGenratedSalesVisit.DataSource = dt.DefaultView;
                grdGenratedSalesVisit.DataBind();
                ViewState["val"] = "";
            }
            ViewState["val"] = "";
            if ((DataTable)ViewState["tempDt"] != null)
            {
                string leadid = "";
                DataTable dt = (DataTable)ViewState["tempDt"];
                if (dt != null)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string lblId = dt.Rows[i]["LeadId"].ToString() + "@@@@" + dt.Rows[i]["ProductId"].ToString();
                        string str = dt.Rows[i]["Alloted Branch"].ToString();
                        if (str == "N/A")
                        {
                            if (leadid == "")
                            {
                                leadid = lblId;
                            }
                            else
                            {
                                leadid += "," + lblId;
                            }
                        }
                    }
                    //grdGenratedSalesVisit.DataSource = dt.DefaultView;
                    //grdGenratedSalesVisit.DataBind();
                }
                txtLeadId.Value = leadid;
            }
        }
        protected void grdGenratedSalesVisit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ViewState["GSales"] != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkSel");
                    if (e.Row.Cells[9].Text.ToString() != "N/A")
                    {
                        chk.Checked = true;
                        chk.Enabled = false;
                    }
                    else
                    {
                        if ((DataTable)ViewState["tempDt"] == null)
                        {
                            Label lbl = (Label)e.Row.FindControl("lblId");
                            chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                        }
                        else
                        {
                            if (ViewState["val"].ToString() != "")
                            {
                                chk.Checked = true;
                                Label lbl = (Label)e.Row.FindControl("lblId");
                                chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                                string Jscript = "<script language='javascript'>";
                                Jscript += "FillLeadId123(this,'" + lbl.Text + "');";
                                Jscript = "</script>";
                                ClientScript.RegisterStartupScript(GetType(), "JScript", Jscript);
                            }
                            else
                            {
                                chk.Checked = false;
                                Label lbl = (Label)e.Row.FindControl("lblId");
                                chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                            }
                        }
                    }
                }
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkSel");
                    if (e.Row.Cells[9].Text.ToString() != "N/A")
                    {

                    }
                    else
                    {
                        if ((DataTable)ViewState["tempDt"] == null)
                        {
                            Label lbl = (Label)e.Row.FindControl("lblId");
                            chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                        }
                        else
                        {
                            if (ViewState["val"].ToString() != "")
                            {
                                chk.Checked = true;
                                Label lbl = (Label)e.Row.FindControl("lblId");
                                chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                                string Jscript = "<script language='javascript'>";
                                Jscript += "FillLeadId123(this,'" + lbl.Text + "');";
                                Jscript = "</script>";
                                ClientScript.RegisterStartupScript(GetType(), "JScript", Jscript);
                            }
                            else
                            {
                                Label lbl = (Label)e.Row.FindControl("lblId");
                                chk.Attributes.Add("onclick", "javascript:FillLeadId(this,'" + lbl.Text + "');");
                            }
                        }
                    }
                }
            }
        }
        protected void grdGenratedSalesVisit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGenratedSalesVisit.PageIndex = e.NewPageIndex;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            grdGenratedSalesVisit.DataSource = dv;
            grdGenratedSalesVisit.DataBind();
            string LeadID1 = txtLeadId.Value;
            if (LeadID1 != null)
            {
                for (int g = 0; g <= grdGenratedSalesVisit.Rows.Count - 1; g++)
                {
                    string[] str = LeadID1.Split(',');
                    for (int i = 0; i <= str.GetUpperBound(0); i++)
                    {
                        if (str[i] != "")
                        {
                            string[] id = str[i].Split('@');
                            Label lbl = (Label)grdGenratedSalesVisit.Rows[g].FindControl("lblId");
                            string[] l = lbl.Text.Split('@');
                            if (l[0] == id[0] && l[4] == id[1])
                            {
                                CheckBox chk = (CheckBox)grdGenratedSalesVisit.Rows[g].FindControl("chkSel");
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        protected void grdGenratedSalesVisit_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["DtSorting"] = sortExpression;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            dv.Sort = sortExpression;
            grdGenratedSalesVisit.DataSource = dv;
            grdGenratedSalesVisit.DataBind();
        }
        public void FillActivityDetails()
        {
            lblUserName.Visible = true;
            txtUser.Visible = true;
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN    tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id", "tbl_trans_Activies.act_id AS id, tbl_master_user.user_name AS name, tbl_master_user.user_id AS userid,ISNULL(tbl_trans_salesVisit.slv_lastdatevisit, '') AS LastVisitDate, ISNULL(tbl_trans_salesVisit.slv_nextvisitdatetime, '') AS nextVisitDate,tbl_trans_salesVisit.slv_salesvisitoutcome AS outcomeId, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome AS Outcome,tbl_trans_Activies.act_scheduledDate AS SchEndDate,  CASE tbl_trans_salesvisit.slv_salesvisitoutcome WHEN 9 THEN 9 WHEN 9 THEN 9 ELSE     (SELECT     tbl_Master_SalesVisitOutcomeCategory.Int_id FROM tbl_Master_SalesVisitOutcomeCategory INNER JOIN   tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category WHERE tbl_master_SalesVisitOutCome.slv_Id = tbl_trans_salesvisit.slv_salesvisitoutcome) END AS outcomeCat", " tbl_trans_Activies.act_assignedTo ='" + Request.QueryString["id"].ToString() + "' order by tbl_master_user.user_id, tbl_trans_Activies.act_id");
            txtUser.Text = dt.Rows[0]["Name"].ToString();
            DataTable dtUserInfo = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow drRow = dtUserInfo.NewRow();
            int actId = 0;
            int sNo = 0;
            int count = 0;
            int CountWon = 0;
            int CountInProcess = 0;
            int CountPending = 0;
            int CountNonUsable = 0;
            DataColumn colNo = new DataColumn("SNO");
            DataColumn colActNO = new DataColumn("Activity NO");
            DataColumn colTotalCalls = new DataColumn("Total Calls");
            DataColumn colWon = new DataColumn("Won");
            DataColumn colInProcess = new DataColumn("In Process");
            DataColumn colPending = new DataColumn("Pending");
            DataColumn colNonUsable = new DataColumn("Non Usable");
            dtUserInfo.Columns.Add(colNo);
            dtUserInfo.Columns.Add(colActNO);
            dtUserInfo.Columns.Add(colTotalCalls);
            dtUserInfo.Columns.Add(colWon);
            dtUserInfo.Columns.Add(colInProcess);
            dtUserInfo.Columns.Add(colPending);
            dtUserInfo.Columns.Add(colNonUsable);
            dtUserInfo.Rows.Add(drRow);
            dt1 = dtUserInfo.Clone();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    actId = Convert.ToInt32(dt.Rows[i][0].ToString());
                    CountPending = 0;
                    count = 0;
                    CountInProcess = 0;
                    CountWon = 0;
                    CountNonUsable = 0;
                    while (actId == Convert.ToInt32(dt.Rows[i][0].ToString()))
                    {
                        count += 1;
                        switch (Convert.ToInt32(dt.Rows[i]["outcomeId"].ToString()))
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 8:
                                CountInProcess += 1;
                                break;
                            case 4:
                                CountWon += 1;
                                break;
                            case 5:
                            case 6:
                            case 7:
                                CountNonUsable += 1;
                                break;
                            case 9:
                                CountPending += 1;
                                break;
                        }
                        i = i + 1;
                        if (i > dt.Rows.Count - 1)
                        {
                            break;
                        }
                    }
                    DataRow row = dt1.NewRow();
                    sNo += 1;
                    row["SNO"] = sNo;
                    row["Activity No"] = actId;
                    row["Total Calls"] = count;
                    row["Won"] = CountWon;
                    row["In Process"] = CountInProcess;
                    row["Pending"] = CountPending;
                    row["Non Usable"] = CountNonUsable;
                    dt1.Rows.Add(row);
                    i = i - 1;
                }
            }
            if (dt1.Rows.Count > 0)
            {
                grdDetail.DataSource = dt1.DefaultView;
                grdDetail.DataBind();
                PnlBtn.Visible = true;
                btnModify.Visible = true;
                userInfo1.Visible = false;
                btnCreate.Visible = false;
                btnMainCancel.Visible = true;
            }
        }
        protected void btnMainCancel_Click(object sender, EventArgs e)
        {
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            btnCreate.Visible = true;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCall.Visible = false;
            pnlShowDetail.Visible = false;
            userInfo1.Visible = true;
            btnMainCancel.Visible = false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            btnCreate.Visible = true;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCall.Visible = false;
            pnlShowDetail.Visible = false;
            userInfo1.Visible = true;
            btnMainCancel.Visible = false;
        }
        public void FillLeadData()
        {
            ViewState["clear"] = null;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                DataTable dt = new DataTable();
                GridViewRow row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)row.FindControl("chkDetail");
                if (chkDetail.Checked == true)
                {
                    ViewState["clear"] = "1";
                    Label lbl = (Label)row.FindControl("lblActNo");
                    dt.Clear();
                    dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_lead INNER JOIN tbl_master_address ON tbl_master_lead.cnt_internalId = tbl_master_address.add_cntId ON   tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_master_phonefax ON tbl_master_lead.cnt_internalId = tbl_master_phonefax.phf_cntId INNER JOIN     tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '') + ISNULL(tbl_master_lead.cnt_lastName, '') AS name,ISNULL(tbl_master_address.add_address1, '') + ISNULL(tbl_master_address.add_address2, '') + ISNULL(tbl_master_address.add_address3, '') AS Address, tbl_master_phonefax.phf_phoneNumber, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as Outcome", " tbl_trans_salesVisit.slv_activityId ='" + lbl.Text + "'");
                    DataTable dt1 = dt.Clone();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string name = dt.Rows[j]["name"].ToString();
                        DataRow row1 = dt1.NewRow();
                        while (name == dt.Rows[j]["name"].ToString())
                        {
                            for (int k = 0; k < dt.Columns.Count; k++)
                            {
                                row1[k] = dt.Rows[j][k].ToString();
                            }
                            j = j + 1;
                            if (j > dt.Rows.Count - 1)
                                break;
                        }
                        dt1.Rows.Add(row1);
                        j -= 1;
                        if (j == dt.Rows.Count - 1)
                            break;
                    }
                    if (dt1 != null)
                    {
                        grdActivityDetail.DataSource = dt1.DefaultView;
                        grdActivityDetail.DataBind();
                        btnModify.Visible = true;
                        PnlBtn.Visible = true;
                    }
                }
            }
            if (ViewState["clear"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('At First Check Activity')</script>");
            }
        }
        protected void grdActivityDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdActivityDetail.PageIndex = e.NewPageIndex;
            FillLeadData();
        }
        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            pnlActivityDetail.Visible = true;
            FillLeadData();
        }
        protected void btnCancelGenratedSalesVisit_Click(object sender, EventArgs e)
        {
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            btnCreate.Visible = true;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCall.Visible = false;
            pnlShowDetail.Visible = false;
            userInfo1.Visible = true;
            btnMainCancel.Visible = false;
        }
        protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkDetail");
                Label lbl = (Label)e.Row.FindControl("lblActNo");
                chkBox.Attributes.Add("onclick", "javascript:chkGenral(this,'" + lbl.Text + "')");
            }
        }
        protected void btnGenratedSales_Click(object sender, EventArgs e)
        {
            frmShowGenratedSalesvisit.Visible = true;
            frmShowCall.Visible = false;
            pnlShowDetail.Visible = false;
            userInfo1.Visible = false;
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            btnMainCancel.Visible = true;
            ViewState["GSales"] = "Sales";
            frmAllot.Visible = true;
            Td1.Visible = false;
        }
        protected void grdDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDetail.PageIndex = e.NewPageIndex;
            FillActivityDetails();
        }
    }
}