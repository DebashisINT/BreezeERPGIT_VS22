using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using DevExpress.Web;
using System.Net.Mime;
using DevExpress.XtraPrinting;
using System.IO;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_CrmPhoneCallActivityWithIFrame : System.Web.UI.Page
    {

        clsDropDownList clsdropdown = new clsDropDownList();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable _dtUserInfo = new DataTable();
        System.DateTime dStartDate = new DateTime();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        string _ActId = "";

        //its has been added by sanjib 13122016 for filtering which bind by datatable so there needed the page_int to rebind the grid.
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                FillGrid();
            }
            else
            {
                FillGridInfo();

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/ActivityManagement/CrmPhoneCallActivityWithIFrame.aspx");
            drpPriority.SelectedValue = "1";
            if (!IsPostBack)
            {
                Label12.Text = "Start Date And Time (Phone Calls):";
                Label13.Text = "End Date And Time (Phone Calls):";
                ltrActivity.Text = Convert.ToString(Session["act_activityNo"]);
                JavaScript();
                ViewState["val"] = "";
                if (Request.QueryString["id"] != null)
                {
                    //FillGrid();//comment by sanjib dueto grid chnage 13122016 and already being call in page_int
                    pnlShowDetail.Visible = true; //comment by sanjib dueto grid chnage 13122016
                }
                else
                {
                    //FillGridInfo();//comment by sanjib dueto grid chnage 13122016 and already being call in page_int
                    FillDropDown();
                    btnModify.Visible = false;
                    pnlShowDetail.Visible = false;//comment by sanjib dueto grid chnage 13122016

                    BindActivityTypeList();
                }


                BindSalesAddForm();
            }
            TxtStartDate.EditFormatString = objConverter.GetDateFormat("DateTime");
            TxtEndDate.EditFormatString = objConverter.GetDateFormat("DateTime");
            FromDate.EditFormatString = objConverter.GetDateFormat("Date");
            ToDate.EditFormatString = objConverter.GetDateFormat("Date");
            SalesEndDate.EditFormatString = objConverter.GetDateFormat("Date");
            SalesStartDate.EditFormatString = objConverter.GetDateFormat("Date");
            CourtesyEndCallDate.EditFormatString = objConverter.GetDateFormat("Date");
            CourtesyStartCallDate.EditFormatString = objConverter.GetDateFormat("Date");
            Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }

        public void BindActivityTypeList()
        {

            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (string.IsNullOrEmpty(Convert.ToString(Session["act_assign_task"])) || (Convert.ToString(Session["act_assign_task"])=="0"))
            {
                dtbl = objbl.GetActivityTypeList();
            }
            else
            {
                dtbl = objbl.GetActivityTypeListBySalesActivity(Convert.ToString(Session["act_id"]));
            }
           // dtbl = objbl.GetActivityTypeList();
            drpActType.DataSource = dtbl;
            drpActType.DataTextField= "aty_activityType";
            drpActType.DataValueField = "aty_id";
            drpActType.DataBind();
        }
        public void FillGridInfo()
        {
            // .............................Code Commented and Added by Sanjib on 12122016 to use Convert.tostring instead of tostring(). ................
            string _temp = "";
            string _userId1 = oDBEngine.getChildUser(Convert.ToString(Session["userid"]), _temp);
            DataTable _dt1 = new DataTable();
            DataTable _dt = new DataTable();
            _dt1 = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id", "tbl_trans_Activies.act_id AS id, ISNULL(tbl_master_user.user_name, '') AS Name, tbl_master_user.user_id AS userid, tbl_trans_phonecall.phc_callDate, tbl_trans_Activies.act_expectedDate,(isnull(tbl_trans_Activies.act_scheduledDate,'') + ' ' + isnull(tbl_trans_Activies.act_scheduledTime,'')) as SchDate, CASE tbl_trans_phonecall.phc_callDispose WHEN 11 THEN 11 ELSE (SELECT tbl_Master_DispositionCategory.Int_id FROM tbl_master_calldispositions INNER JOIN tbl_Master_DispositionCategory ON tbl_master_calldispositions.Call_Category = tbl_Master_DispositionCategory.Int_id WHERE tbl_master_calldispositions.call_id = phc_callDispose) END AS phcid, CASE tbl_trans_Activies.act_priority WHEN 0 THEN 'Low' WHEN 1 THEN 'Normal' WHEN 2 THEN 'High' WHEN 3 THEN 'Urgent' WHEN 4 THEN 'Complete' ELSE 'Not Known' END AS Pripority", "tbl_trans_Activies.act_assignedBy IN (" + _userId1 + ") or tbl_trans_Activies.act_assignedto in(" + _userId1 + ") AND act_actualenddate IS NULL order by Name, tbl_trans_Activies.act_id, tbl_trans_phonecall.phc_callDate");
            DataRow _row = default(DataRow);
            _row = _dt.NewRow();
            DataColumn _colUserid = new DataColumn("UserId");
            DataColumn _colSN = new DataColumn("SNo");
            DataColumn _colUser = new DataColumn("User");
            DataColumn _colPenAct = new DataColumn("Pending Acttivity");
            DataColumn _colSchExpDt = new DataColumn("Scheduled End Date");
            DataColumn _colExpEndDt = new DataColumn("Expected End Date");
            DataColumn _colPenCall = new DataColumn("Pending Call");
            DataColumn _colCallBack = new DataColumn("Call Back");
            DataColumn _colNonContactable = new DataColumn("Non Contactable");
            DataColumn _colNonUsable = new DataColumn("Non Usable");
            DataColumn _colPipelineSalesVisits = new DataColumn("Pipeline/Sales Visits");
            DataColumn _colWonConfirmSales = new DataColumn("Won/Confirm Sales");
            DataColumn _colLostNotInterested = new DataColumn("Lost/Not Interested");
            _dt.Columns.Add(_colUserid);
            _dt.Columns.Add(_colSN);
            _dt.Columns.Add(_colUser);
            _dt.Columns.Add(_colPenAct);
            _dt.Columns.Add(_colSchExpDt);
            _dt.Columns.Add(_colExpEndDt);
            _dt.Columns.Add(_colPenCall);
            _dt.Columns.Add(_colCallBack);
            _dt.Columns.Add(_colNonContactable);
            _dt.Columns.Add(_colNonUsable);
            _dt.Columns.Add(_colPipelineSalesVisits);
            _dt.Columns.Add(_colWonConfirmSales);
            _dt.Columns.Add(_colLostNotInterested);
            _dt.Rows.Add(_row);
            _dtUserInfo = _dt.Clone();
            int _userid = 0;
            int _count = 0;
            int _countAct = 1;
            int _sNo = 0;
            //Dim _actId As String = "" 
            System.DateTime dDate = new DateTime();
            System.DateTime dTempDate = new DateTime();
            System.DateTime _startDate = new DateTime();
            int _Call_Back = 0;
            int _Non_Contactable = 0;
            int _Non_Usable = 0;
            int _Pipeline_Sales_Visits = 0;
            int _Won_Confirm_Sales = 0;
            int _Lost_Not_Interested = 0;
            string _schEndDate = "";
            if (_dt1 != null & _dt1.Rows.Count > 0)
            {
                for (int i = 0; i <= _dt1.Rows.Count - 1; i++)
                {
                    _Call_Back = 0;
                    _Non_Contactable = 0;
                    _Non_Usable = 0;
                    _Pipeline_Sales_Visits = 0;
                    _Won_Confirm_Sales = 0;
                    _Lost_Not_Interested = 0;
                    _count = 0;
                    if (_dt1.Rows[i][3].ToString() != "")
                    {
                        dDate = Convert.ToDateTime(_dt1.Rows[i][3].ToString());
                    }
                    _count = 0;
                    _countAct = 1;
                    _ActId = _dt1.Rows[i][0].ToString();
                    _userid = Convert.ToInt32(_dt1.Rows[i][2].ToString());

                    string _Name = _dt1.Rows[i][1].ToString();
                    if (_dt1.Rows[i]["phc_CallDate"].ToString() == "")
                    {
                        while (_Name == _dt1.Rows[i][1].ToString())
                        {
                            switch (Convert.ToInt32(_dt1.Rows[i]["phcid"].ToString()))
                            {
                                case 1:
                                    _Call_Back += 1;
                                    break;
                                case 2:
                                    _Non_Contactable += 1;
                                    break;
                                case 3:
                                    _Non_Usable += 1;
                                    break;
                                case 4:
                                    _Pipeline_Sales_Visits += 1;
                                    break;
                                case 5:
                                    _Won_Confirm_Sales += 1;
                                    break;
                                case 6:
                                    _Lost_Not_Interested += 1;
                                    break;
                                case 11:
                                    _count += 1;
                                    break;
                            }

                            if (_ActId != _dt1.Rows[i][0].ToString())
                            {
                                _countAct += 1;
                                _ActId = _dt1.Rows[i][0].ToString();
                            }
                            //If i = _dt1.Rows.Count - 1 Then Exit Do 
                            if (_dt1.Rows[i][3].ToString() != "")
                            {
                                dTempDate = Convert.ToDateTime(_dt1.Rows[i][3].ToString());
                            }
                            if (oDBEngine.IsDate(dStartDate.ToString()) == false)
                            {
                                dStartDate = Convert.ToDateTime(_dt1.Rows[i]["SchDate"].ToString());
                            }
                            else
                            {
                                if (oDBEngine.IsDate(_dt1.Rows[i]["SchDate"].ToString()) == true)
                                {
                                    if (dStartDate > Convert.ToDateTime(_dt1.Rows[i]["SchDate"].ToString())) dStartDate = Convert.ToDateTime(_dt1.Rows[i]["SchDate"].ToString());
                                }
                            }

                            if (dDate < dTempDate) dDate = dTempDate;
                            _schEndDate = _dt1.Rows[i]["act_expectedDate"].ToString();
                            i += 1;
                            if (i > _dt1.Rows.Count - 1) break; // TODO: might not be correct. Was : Exit Do 

                        }
                        DataRow _row1 = _dtUserInfo.NewRow();
                        _sNo += 1;
                        int _totalCall = 0;
                        System.DateTime _dTempDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                        while (_totalCall == 0)
                        {
                            _dt.Clear();
                            _dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", " COUNT(*) ", "tbl_trans_Activies.act_assignedTo='" + _userid + "'");
                            if (_dt.Rows.Count > 0)
                            {
                                _totalCall = Convert.ToInt32(_dt.Rows[0][0].ToString());
                            }
                            else
                            {
                                if (dTempDate > oDBEngine.GetDate().AddDays(-500))
                                {
                                    dTempDate = dTempDate.AddDays(-1);
                                }
                            }
                        }
                        string _dExpectedEndDate = null;

                        if (_dTempDate == oDBEngine.GetDate())
                        {
                            DateTime dtemptime = new DateTime(oDBEngine.GetDate().Year, oDBEngine.GetDate().Month, oDBEngine.GetDate().Day, 10, 0, 0);
                            DateTime dCurrentTime = oDBEngine.GetDate();
                            TimeSpan k = dCurrentTime - dtemptime;
                            double _dDiff = k.TotalSeconds;
                        }

                        _row1["UserId"] = _userid;
                        _row1["SNo"] = _sNo;
                        _row1["User"] = _Name;
                        _row1["Pending Call"] = _count;
                        _row1["Pending Acttivity"] = _countAct;
                        Converter oConverter = new Converter();
                        _row1["Expected End Date"] = oConverter.ArrangeDate(oDBEngine.getEndDate(dStartDate.ToString(), (_count + _Call_Back + _Non_Contactable + _Non_Usable + _Pipeline_Sales_Visits + _Won_Confirm_Sales + _Lost_Not_Interested), (_Call_Back + _Non_Contactable + _Non_Usable + _Pipeline_Sales_Visits + _Won_Confirm_Sales + _Lost_Not_Interested)));
                        _row1["Scheduled End Date"] = oConverter.ArrangeDate(_schEndDate);
                        _row1["Call Back"] = _Call_Back;
                        _row1["Non Contactable"] = _Non_Contactable;
                        _row1["Non Usable"] = _Non_Usable;
                        _row1["Pipeline/Sales Visits"] = _Pipeline_Sales_Visits;
                        _row1["Won/Confirm Sales"] = _Won_Confirm_Sales;
                        _row1["Lost/Not Interested"] = _Lost_Not_Interested;
                        _dtUserInfo.Rows.Add(_row1);

                        i = i - 1;
                    }
                }
            }
            //comment by sanjib dueto grid chnage 13122016
            //grdUserInfo.DataSource = _dtUserInfo.DefaultView;
            //grdUserInfo.DataBind();

            grdUserInfo.DataSource = _dtUserInfo.DefaultView;
            grdUserInfo.DataBind();

        }
        public void FillGrid()
        {
            //grdUserInfo.Visible = false;//comment by sanjib dueto grid chnage 13122016
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            PnlBtn.Visible = true;
            btnModify.Visible = true;
            btnCreate.Enabled = false;
            btnGenratedSalesVisit.Enabled = false;
            DataTable _dt1 = new DataTable();
            _dt1 = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN    tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id", "tbl_trans_Activies.act_id AS id, ISNULL(tbl_master_user.user_name, '') AS Name, tbl_master_user.user_id AS userid, tbl_trans_phonecall.phc_callDate, tbl_trans_Activies.act_expectedDate, CASE tbl_trans_phonecall.phc_callDispose WHEN 11 THEN 11 ELSE (SELECT tbl_Master_DispositionCategory.Int_id FROM tbl_master_calldispositions INNER JOIN tbl_Master_DispositionCategory ON tbl_master_calldispositions.Call_Category = tbl_Master_DispositionCategory.Int_id WHERE tbl_master_calldispositions.call_id = phc_callDispose) END AS phcid, CASE tbl_trans_Activies.act_priority WHEN 0 THEN 'Low' WHEN 1 THEN 'Normal' WHEN 2 THEN 'High' WHEN 3 THEN 'Urgent' WHEN 4 THEN 'Complete' ELSE 'Not Known' END AS Pripority", "tbl_master_user.user_id =" + Request.QueryString["id"].ToString() + " AND act_actualenddate IS NULL order by Name, tbl_trans_Activies.act_id, tbl_trans_phonecall.phc_callDate");
            txtUser.Text = _dt1.Rows[0]["Name"].ToString();
            //Session["tempUserName"] = txtUser.Text;
            txtUser.Visible = true;
            lblUserName.Visible = true;
            DataTable _dtUserInfo1 = new DataTable();
            DataTable _dt2 = new DataTable();
            DataRow _row = _dtUserInfo1.NewRow();
            int _sNo = 0;
            int _count = 0;
            int _CountWon = 0;
            int _CountInProcess = 0;
            int _CountPending = 0;
            int _CountNonUsable = 0;

            DataColumn _colNo = new DataColumn("SNO");
            DataColumn _colActNO = new DataColumn("Activity NO");
            DataColumn _colTotalCalls = new DataColumn("Total Calls");
            DataColumn _colWon = new DataColumn("Won");
            DataColumn _colInProcess = new DataColumn("In Process");
            DataColumn _colPending = new DataColumn("Pending");
            DataColumn _colNonUsable = new DataColumn("Non Usable");
            _dtUserInfo1.Columns.Add(_colNo);
            _dtUserInfo1.Columns.Add(_colActNO);
            _dtUserInfo1.Columns.Add(_colTotalCalls);
            _dtUserInfo1.Columns.Add(_colWon);
            _dtUserInfo1.Columns.Add(_colInProcess);
            _dtUserInfo1.Columns.Add(_colPending);
            _dtUserInfo1.Columns.Add(_colNonUsable);
            _dtUserInfo1.Rows.Add(_row);

            _dt2 = _dtUserInfo1.Clone();
            if (_dt1.Rows.Count > 0)
            {
                for (int i = 0; i <= _dt1.Rows.Count - 1; i++)
                {
                    _ActId = _dt1.Rows[i][0].ToString();
                    _CountPending = 0;
                    _count = 0;
                    _CountInProcess = 0;
                    _CountWon = 0;
                    _CountNonUsable = 0;
                    while (_ActId == _dt1.Rows[i][0].ToString())
                    {
                        _count += 1;
                        switch (Convert.ToInt32(_dt1.Rows[i]["phcid"].ToString()))
                        {
                            case 1:
                                _CountInProcess += 1;
                                break;
                            case 4:
                                _CountWon += 1;
                                break;
                            case 5:
                                _CountWon += 1;
                                break;
                            case 2:
                                _CountNonUsable += 1;
                                break;
                            case 3:
                                _CountNonUsable += 1;
                                break;
                            case 6:
                                _CountNonUsable += 1;
                                break;
                            case 11:
                                _CountPending += 1;
                                break;
                        }
                        i = i + 1;
                        if (i > _dt1.Rows.Count - 1)
                        {
                            break; // TODO: might not be correct. Was : Exit Do 
                        }
                    }
                    DataRow _row1 = _dt2.NewRow();
                    _sNo += 1;
                    _row1["SNO"] = _sNo;
                    _row1["Activity No"] = _ActId;
                    _row1["Total Calls"] = _count;
                    _row1["Won"] = _CountWon;
                    _row1["In Process"] = _CountInProcess;
                    _row1["Pending"] = _CountPending;
                    _row1["Non Usable"] = _CountNonUsable;
                    //If _CountPending <> 0 Then 
                    _dt2.Rows.Add(_row1);
                    //End If 
                    i = i - 1;
                }
            }
            if (_dt2.Rows.Count > 0)
            {
                //comment by sanjib dueto grid chnage 13122016

                //grdDetail.DataSource = _dt2.DefaultView;
                //grdDetail.DataBind();

                grdDetailNew.DataSource = _dt2.DefaultView;
                grdDetailNew.DataBind();
            }
            btnCancel1.Visible = true;
        }
        public void FillDropDown()
        {
            string[,] ActType = oDBEngine.GetFieldValue("tbl_master_activitytype", "aty_id,aty_activityType", null, 2, "aty_activityType");
            //if (ActType[0, 0] != "n")
            //{
            //    clsdropdown.AddDataToDropDownList(ActType, drpActType);
            //    // drpActType.SelectedItem.Value = "1";
            //}
            string temp = "";
            string Userid = oDBEngine.getChildUser(Session["userid"].ToString(), temp);
            string[,] UserWork = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", "user_id in(" + Userid + ")", 2, "user_name");
            if (UserWork[0, 0] != "n")
            {
                clsdropdown.AddDataToDropDownList(UserWork, drpUserWork);
            }
            string[,] SUser = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", "user_id in(" + Userid + ")", 2, "user_name");
            if (ActType[0, 0] != "n")
            {
                clsdropdown.AddDataToDropDownList(SUser, drpSelectedUser);
                drpSelectedUser.Items.Add(new ListItem("All", "0"));
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // .............................Code Commented and Added by Sanjib on 13122016 to use Convert.tostring instead of tostring(). ................
            string sStartDate1 = Convert.ToDateTime(Convert.ToString(TxtStartDate.Value)).ToShortDateString();
            string sStartTime = Convert.ToDateTime(Convert.ToString(TxtStartDate.Value)).ToShortTimeString();
            string sEndDate = Convert.ToDateTime(Convert.ToString(TxtEndDate.Value)).ToShortDateString();
            string sEndTime = Convert.ToDateTime(Convert.ToString(TxtEndDate.Value)).ToShortTimeString();
            string id = "";
            string ActNo = "";
            string _idSales = "";
            string column = "";
            DataTable dt = new DataTable();
            string[,] ActidForProduct;

            string[] assignto;
            int rowCount;
            switch (Convert.ToString(ViewState["status"]))
            {
                case "CreateNew":
                    hd1UserList.Value = Convert.ToString(Session["AssignedTaskId"]);
                    string tempLeadId = hd1UserList.Value;
                    int count12 = 0;
                    if (Session["Count"] != null)
                    {
                        count12 = Convert.ToInt32(Convert.ToString(Session["Count"]));
                    }
                    else
                    {
                        count12 = 0;
                    }
                    string[] st = tempLeadId.Split(',');

                      string slead = Convert.ToString(Session["AssignedActivity"]);
                        string[] temp1 = slead.Split(',');
                       
                        string sSales = Convert.ToString(Session["AssignedSalesId"]);
                        string[] tempSales = sSales.Split(',');
                        string sact_id = Convert.ToString(Session["act_id"]);
                        string[] tempact_id = sact_id.Split(',');                    

                    for (int i = 0; i <= st.Length - 1; i++)
                    {                      
                        //phone call
                        if (drpActType.SelectedItem.Value == "1")
                        {
                            int tempLeadValue = Convert.ToInt32(st.GetValue(i));
                            string actNo = oDBEngine.GetInternalId("PC", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                         
                            oDBEngine.messageTableUpdate(tempLeadValue.ToString(), "0", "Phone Calls", sStartDate1, sEndDate, drpPriority.SelectedItem.Text, txtInstNote.Text, null, "message");
                           
                            string sendStr = "";
                            int ij = 0;
                           
                            //kaushik 26-12-2016 
                          
                            bool bVal = false;
                            int startVal = 0 * count12;
                            int endVal = (0 + 1) * count12;                           
                            try
                            {
                             
                                bVal = true;                               
                                oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + tempact_id[i] + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + temp1[i] + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "','" + tempSales[i] + "'");                                  
                                    sendStr = temp1[i];                                  
                                    sendStr = oDBEngine.sepComma(sendStr);
                                    string useraccess = "";
                                    if (sendStr != "")
                                    {
                                        string AllUser1 = "";
                                        string AllUser2 = "";                                      
                                        string[,] useraccess1 = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_useraccess", " cnt_internalid in (" + sendStr + ")", 1);
                                        if (useraccess1[0, 0] != "n")
                                        {
                                            useraccess = useraccess1[0, 0];
                                        }
                                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "',cnt_useraccess=cnt_useraccess+'," + tempLeadId + "'", " cnt_internalId in(" + sendStr + ")");
                                    }
                                    sendStr = "";
                                    ij = 0;                            

                            }
                            catch
                            {
                            }
                        }
                        //sales visit
                        if (drpActType.SelectedItem.Value == "4")
                        {

                         
                                ActNo = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                                rowCount = Convert.ToInt32(tempact_id[i]);
                                ActidForProduct = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + ActNo.ToString() + "' and act_assignedto='" + st[i] + "'", 1);
                                                           
                                //kaushik                              
                                string endOfString = temp1[i].Substring(0, 2);
                                if (endOfString == "LD")
                                {
                                    Session["callType"] = "lead";
                                }
                                else { Session["callType"] = ""; }
                             
                                dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + temp1[i] + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }

                                    if (tempPhonecallid == "")
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + tempact_id[i] + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + temp1[i] + "','9','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + "" + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "','" + tempSales[i] + "'");
                                    }
                                    else
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + tempact_id[i] + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + temp1[i] + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + "" + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "','" + tempSales[i] + "'");
                                    }

                                    if (Session["callType"] != null)
                                    {
                                        if (Session["callType"].ToString() == "lead")
                                        {
                                            oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + ActNo + "'", " cnt_internalid='" + temp1[i] + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + ActNo + "'", " cnt_internalid='" + temp1[i] + "'");
                                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + ActNo + "'", " phc_NextActivityId='allot' And phc_leadcotactId='" + temp1[i] + "'");
                                        }
                                    }                             
                        }
                    }


                    break;
                case "Modify":
                    string temp2 = "";
                    if (Convert.ToString(Session["lead"]) == "")
                    {
                        temp2 = ViewState["ids"].ToString();
                    }
                    else
                    {
                        temp2 = ViewState["ids"].ToString() + "," + Convert.ToString(Session["lead"]);
                    }
                    string[] tmp1 = temp2.Split(',');
                    column = "act_branchId='" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "',act_activityType='" + drpActType.SelectedValue + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + Convert.ToString(Session["userid"]) + "',act_assignedTo='" + drpUserWork.SelectedValue + "',act_contactlead='" + tmp1.Length + "',act_scheduledDate='" + sStartDate1 + "',act_expectedDate='" + sEndDate + "',act_priority='" + drpPriority.SelectedValue + "',act_scheduledTime='" + sStartTime + "',act_expectedTime='" + sEndTime + "'";
                    oDBEngine.SetFieldValue("tbl_trans_Activies", column, " act_id='" + ViewState["edit"].ToString() + "'");
                    oDBEngine.messageTableUpdate(drpUserWork.SelectedValue, "0", "Phone Calls", sStartDate1, sEndDate, drpPriority.SelectedValue, txtInstNote.Text, ViewState["edit"].ToString(), "activity");
                    string actNo2 = "";
                    string[] tempp = Session["lead"].ToString().Split(',');
                    foreach (string chrk in tempp)
                    {
                        string[] chrk1 = chrk.Split('|');
                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser", "'" + Convert.ToString(ViewState["edit"]) + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + chrk1.GetValue(0) + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo2 + "'", " cnt_internalid='" + chrk1.GetValue(0) + "'");
                    }
                    break;
                case "Reassign":
                    string temp3 = "";
                    if (Convert.ToString(Session["lead"]) == "")
                    {
                        temp3 = Convert.ToString(ViewState["ids"]);
                    }
                    else
                    {
                        temp3 = Convert.ToString(ViewState["ids"]) + "," + Convert.ToString(Session["lead"]);
                    }
                    string[] tmp = temp3.Split(',');
                    string column1 = "act_branchId='" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "',act_activityType='" + drpActType.SelectedValue + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + Convert.ToString(Session["userid"]) + "',act_assignedTo='" + drpUserWork.SelectedValue + "',act_contactlead='" + tmp.Length + "',act_scheduledDate='" + sStartDate1 + "',act_expectedDate='" + sEndDate + "',act_scheduledTime='" + sStartTime + "',act_expectedTime='" + sEndTime + "'";
                    oDBEngine.SetFieldValue("tbl_trans_Activies", column1, " act_id='" + ViewState["edit"].ToString() + "'");
                    oDBEngine.messageTableUpdate(drpUserWork.SelectedValue, "0", "Phone Calls", sStartDate1, sEndDate, drpPriority.SelectedValue, txtInstNote.Text, ViewState["edit"].ToString(), "activity");
                    string actNo3 = "";
                    string[] temp11 = Session["lead"].ToString().Split(',');
                    foreach (string chrk in temp11)
                    {
                        string[] chrk1 = chrk.Split('|');
                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser", "'" + Convert.ToString(ViewState["edit"]) + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + chrk1.GetValue(0) + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo3 + "'", " cnt_internalid='" + chrk1.GetValue(0) + "'");
                    }
                    break;
                case "Reschedule":
                    string temp4 = "";
                    if (Convert.ToString(Session["lead"]) == "")
                    {
                        temp4 = Convert.ToString(ViewState["ids"]);
                    }
                    else
                    {
                        temp4 = Convert.ToString(ViewState["ids"]) + "," + Convert.ToString(Session["lead"]);
                    }
                    string[] tmp4 = temp4.Split(',');
                    string column4 = "act_branchId='" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "',act_activityType='" + drpActType.SelectedValue + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + Convert.ToString(Session["userid"]) + "',act_assignedTo='" + drpUserWork.SelectedValue + "',act_contactlead='" + tmp4.Length + "',act_scheduledDate='" + sStartDate1 + "',act_expectedDate='" + sEndDate + "',act_scheduledTime='" + sStartTime + "',act_expectedTime='" + sEndTime + "'";
                    oDBEngine.SetFieldValue("tbl_trans_Activies", column4, " act_id='" + Convert.ToString(ViewState["edit"]) + "'");
                    oDBEngine.messageTableUpdate(drpUserWork.SelectedValue, "0", "Phone Calls", sStartDate1, sEndDate, drpPriority.SelectedValue, txtInstNote.Text, Convert.ToString(ViewState["edit"]), "activity");
                    break;
            }
            Response.Redirect("CrmPhoneCallActivityWithIFrame.aspx");

            // .............................Code Above Commented and Added by Sanjib on 13122016 to use Convert.tostring instead of tostring(). .....................................
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {

            ViewState["status"] = "CreateNew";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            trgrid.Visible = false;
            pnlCall.Visible = true;
            drpUserWork.Enabled = true;
            pnlActivityDetail.Visible = false;
            PnlBtn.Visible = false;
            FrmShowGenratedSales.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCourtesyCall.Visible = false;
            frmShowCall.Visible = true;
            BtnProperty(btnCancel1, btnGenratedSalesVisit, btnGenratedSales, btnCourtesyCall, btnCreate);
          //  Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }
        public void FillActivityData()
        {
            DataTable dt = new DataTable();

            //comment by sanjib dueto grid chnage 13122016

            //for (int i = 0; i < grdDetail.Rows.Count; i++)
            //{
            //    GridViewRow row = grdDetail.Rows[i];
            //for (int i = 0; i < grdDetailNew.Rows.Count; i++)

            //end


            for (int i = 0; i < grdDetailNew.VisibleRowCount; i++)
            {
                //comment by sanjib dueto grid chnage 13122016

                //GridViewRow row = grdDetailNew.(i);
                //CheckBox chkDetail = (CheckBox)row.FindControl("chkDetail");
                //end
                GridViewDataColumn col1 = grdDetailNew.Columns[0] as GridViewDataColumn;

                ASPxCheckBox chkIsVal = grdDetailNew.FindRowCellTemplateControl(i, col1, "chkDetail") as ASPxCheckBox;


                if (chkIsVal != null)
                {
                    //if (chkDetail.Checked == true)
                    if (chkIsVal.Checked == true)
                    {
                        dt.Clear();
                        //Label lbl = (Label)row.FindControl("lblActNo"); //change label for grid chnaged 13122016
                        ASPxTextBox lbls = grdDetailNew.FindRowCellTemplateControl(i, col1, "lblActNo") as ASPxTextBox;
                        string lbl = Convert.ToString(lbls.Text);
                        if (lbl != string.Empty)
                        {
                            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "tbl_trans_Activies.act_activityType as Type, tbl_trans_Activies.act_assignedTo AS userId, Isnull(tbl_trans_Activies.act_scheduledDate,'') AS StartDate, isnull(tbl_trans_Activies.act_expectedDate,'') AS EndDate,   Isnull(tbl_trans_Activies.act_instruction,'') AS Instruction, tbl_trans_Activies.act_priority AS Priority, isnull(tbl_trans_Activies.act_description,'') AS Description,   tbl_trans_phonecall.phc_id AS PhoneCallid,isNull(tbl_trans_Activies.act_scheduledTime,'') as StartTime,isNull(tbl_trans_Activies.act_expectedTime,'') as EndTime", " tbl_trans_Activies.act_id ='" + lbl + "'");
                            if (dt != null)
                            {
                                drpActType.SelectedValue = dt.Rows[0]["Type"].ToString();
                                drpUserWork.SelectedValue = dt.Rows[0]["userId"].ToString();
                                //DateTime dDate1 = Convert.ToDateTime(dt.Rows[0]["StartDate"].ToString() + " " + dt.Rows[0]["StartTime"].ToString());
                                txtDesc.Text = dt.Rows[0]["Description"].ToString();
                                txtInstNote.Text = dt.Rows[0]["Instruction"].ToString();
                                if (dt.Rows[0]["Priority"].ToString() != "")
                                {
                                    drpPriority.SelectedValue = dt.Rows[0]["Priority"].ToString();
                                }
                            }
                            //ViewState["edit"] = lbl.Text;
                            ViewState["edit"] = lbl;
                            FillDropDown();
                            pnlCall.Visible = true;
                            pnlActivityDetail.Visible = false;
                            //grdDetail.Visible = false;
                            grdDetailNew.Visible = false;
                            drpActType.Enabled = false;
                            drpUserWork.Enabled = false;
                            break;
                        }
                    }
                }
            }
            if (dt != null)
            {
                string temp = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp += dt.Rows[i]["PhoneCallid"].ToString() + ",";
                }
                temp = oDBEngine.sepComma(temp);
                ViewState["ids"] = temp;
            }
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Modify";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            trgrid.Visible = false;
            FillActivityData();
            BtnProperty1(btnReschedule, btnShowDetail, btnDelegate, btnModify, btnReassign);
        }
        protected void btnGenratedSalesVisit_Click(object sender, EventArgs e)
        {
            grdGenratedSalesVisit.Visible = false;
            frmAllot.Visible = false;
            lblTotalRecord.Text = "";
            btnSelectAll.Visible = false;
            btnExport.Visible = false;
            pnlShowDetail.Visible = false;
            frmShowCall.Visible = false;
            FrmShowGenratedSales.Visible = false;
            frmShowGenratedSalesvisit.Visible = true;
            frmShowCourtesyCall.Visible = false;
            ViewState["status"] = "Modify";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            trgrid.Visible = false;
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
            pnlActivityDetail.Visible = false;
            PnlBtn.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "SalesVisit", "<script language='JavaScript'>InVisibilityForSalesVisit();</script>");
            BtnProperty(btnGenratedSales, btnCourtesyCall, btnCreate, btnCancel1, btnGenratedSalesVisit);
          //  Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }
        protected void btnGenratedSales_Click(object sender, EventArgs e)
        {
            //TdGeneratedSales.Visible = false;
            grdSales.Visible = false;
            btnSelectAllSales.Visible = false;
            btnSalesExport.Visible = false;
            frmSalesAllot.Visible = false;
            pnlShowDetail.Visible = false;
            frmShowCall.Visible = false;
            FrmShowGenratedSales.Visible = true;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCourtesyCall.Visible = false;
            ViewState["status"] = "Modify";
            //grdUserInfo.Visible = false; 
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            trgrid.Visible = false;
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "Visibility", "<script language='JavaScript'>InVisibilityForSales();</script>");
            BtnProperty(btnGenratedSalesVisit, btnCourtesyCall, btnCreate, btnCancel1, btnGenratedSales);
            //Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }
        protected void btnCourtesyCall_Click(object sender, EventArgs e)
        {
            tdcourtesy.Visible = false;
            pnlShowDetail.Visible = false;
            frmShowCall.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCourtesyCall.Visible = true;
            FrmShowGenratedSales.Visible = false;
            ViewState["status"] = "Modify";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;//sanjib added for export excel/pdf14122016

            trgrid.Visible = false;
            pnlActivityDetail.Visible = false;
            drpUserWork.Enabled = false;
            drpActType.Enabled = false;
            PnlBtn.Visible = false;
            btnModify.Visible = false;
            BtnProperty(btnGenratedSalesVisit, btnCreate, btnCancel1, btnGenratedSales, btnCourtesyCall);
            //Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }
        protected void btnReassign_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Reassign";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;//sanjib added for export excel/pdf14122016

            trgrid.Visible = false;
            FillActivityData();
            BtnProperty1(btnReschedule, btnShowDetail, btnDelegate, btnReassign, btnModify);
        }
        protected void btnReschedule_Click(object sender, EventArgs e)
        {
            ViewState["status"] = "Reschedule";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;//sanjib added for export excel/pdf14122016

            trgrid.Visible = false;
            FillActivityData();
            BtnProperty1(btnShowDetail, btnDelegate, btnReassign, btnReschedule, btnModify);
        }
        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            pnlActivityDetail.Visible = true;
            pnlCall.Visible = false;
            pnlShowDetail.Visible = false;
            int count = 0;
            //for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            //{
            for (int i = 0; i < grdDetailNew.VisibleRowCount; i++)
            {
                //GridViewRow row = grdDetail.Rows[i];
                //CheckBox chkDetail = (CheckBox)row.FindControl("ChkDetail");
                //if (chkDetail.Checked == true)
                //{
                GridViewDataColumn col1 = grdDetailNew.Columns[0] as GridViewDataColumn;

                ASPxCheckBox chkIsVal = grdDetailNew.FindRowCellTemplateControl(i, col1, "chkDetail") as ASPxCheckBox;
                if (chkIsVal != null)
                {
                    if (chkIsVal.Checked == true)
                    {
                        dt.Clear();

                        //Label lbl = (Label)row.FindControl("lblActNo");
                        ASPxTextBox lbls = grdDetailNew.FindRowCellTemplateControl(i, col1, "lblActNo") as ASPxTextBox;
                        string lbl = Convert.ToString(lbls.Text);
                        if (lbl != string.Empty)
                        {
                            dt = oDBEngine.GetDataTable("tbl_master_phonefax INNER JOIN    tbl_trans_Activies INNER JOIN    tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN    tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId ON    tbl_master_phonefax.phf_cntId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId", "DISTINCT (tbl_master_contact.cnt_internalid),ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') + ISNULL(tbl_master_contact.cnt_lastName, '') AS name, tbl_master_phonefax.phf_phoneNumber as PhNo, ISNULL(tbl_master_address.add_address1, '') + ISNULL(tbl_master_address.add_address2, '')    + ISNULL(tbl_master_address.add_address3, '') AS Address,    CASE tbl_trans_Activies.act_priority WHEN 0 THEN 'Low' WHEN 1 THEN 'Normal' WHEN 2 THEN 'High' WHEN 3 THEN 'Urgent' WHEN 4 THEN 'Complete'     ELSE 'Not Known' END AS Priority", " tbl_trans_phonecall.phc_activityId=" + lbl + " and phc_calldispose=11 order by name");
                            DataTable dt1 = dt.Clone();
                            DataRow row1;
                            for (int j = 0; j <= dt.Rows.Count - 1; j++)
                            {
                                string name = dt.Rows[j]["name"].ToString();
                                row1 = dt1.NewRow();
                                while (name == dt.Rows[j]["name"].ToString())
                                {
                                    for (int k = 0; k <= dt.Columns.Count - 1; k++)
                                    {
                                        row1[k] = dt.Rows[j][k].ToString();
                                    }
                                    j = j + 1;
                                    if (j > dt.Rows.Count - 1) break;
                                }
                                dt1.Rows.Add(row1);
                                count += 1;
                                j -= 1;
                                if (j == dt.Rows.Count - 1) break;
                            }
                            if (dt1 != null)
                            {
                                grdActivityDetail.DataSource = dt1.DefaultView;
                                grdActivityDetail.DataBind();
                            }
                        }
                    }
                }
            }
            BtnProperty1(btnDelegate, btnReassign, btnReschedule, btnShowDetail, btnModify);
        }
        protected void btnDelegate_Click(object sender, EventArgs e)
        {
            //comment by sanjib dueto grid chnage 13122016

            //for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            //{
            //GridViewRow row = grdDetail.Rows[i];
            //CheckBox chkDetail = (CheckBox)row.FindControl("ChkDetail");
            for (int i = 0; i < grdDetailNew.VisibleRowCount; i++)
            {
                GridViewDataColumn col1 = grdDetailNew.Columns[0] as GridViewDataColumn;

                ASPxCheckBox chkIsVal = grdDetailNew.FindRowCellTemplateControl(i, col1, "chkDetail") as ASPxCheckBox;
                if (chkIsVal != null)
                {
                    if (chkIsVal.Checked == true)
                    {
                        //Label lbl = (Label)row.FindControl("lblActNo");//comment by sanjib dueto grid chnage 13122016
                        ASPxTextBox lbls = grdDetailNew.FindRowCellTemplateControl(i, col1, "lblActNo") as ASPxTextBox;
                        string lbl = Convert.ToString(lbls.Text);
                        if (lbl != string.Empty)
                        {
                            //Response.Redirect("frm_ReallocatePhonecall.aspx?type=PhoneCall&id=" + lbl.Text);
                            btnDelegate.Attributes.Add("onclick", "javascript:frmOpenNewWindow1('frm_ReallocatePhonecall.aspx?type=PhoneCall&id=" + lbl + "',500,600)");
                        }
                    }
                }
            }
        }
        public void JavaScript()
        {
            string today = objConverter.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
            txtUserList.Attributes.Add("onclick", "UserList()");
            txtUserList.Attributes.Add("onblur", "checkButton()");
            //btnExport.Attributes.Add("onclick", "frmOpenNewWindow_custom('../management/frmmessage_history.aspx','250','800','250','210')");
            //btnSalesExport.Attributes.Add("onclick", "frmOpenNewWindow_custom('../management/frmmessage_history.aspx','250','800','250','210')");
            btnExport.Attributes.Add("onclick", "CallExport()");
            btnSalesExport.Attributes.Add("onclick", "CallExport()");
            TxtStartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            TxtEndDate.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(1).ToString());
            SalesEndDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            SalesStartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            ToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            FromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            CourtesyEndCallDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            CourtesyStartCallDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

        }
        protected void grdActivityDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdActivityDetail.PageIndex = e.NewPageIndex;
            btnShowDetail_Click(sender, e);
        }
        protected void btnshowGenratedSaleVisit_Click(object sender, EventArgs e)
        {
            grdGenratedSalesVisit.Visible = true;
            btnSelectAll.Visible = false;
            btnExport.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "SalesVisit1", "<script language='JavaScript'>InVisibilityForSalesVisit();</script>");

            lblTotalRecord.Text = "";
            string fdate = "";
            string productcon = "";
            string drpselectedval = "";
            string drpselectedproduct = "";
            string leadid = "";
            string todate = "";
            string[] fdate1;
            string[] todate1;
            if (e.ToString() != "")
            {
                fdate = FromDate.Value.ToString();
                todate = ToDate.Value.ToString();
                fdate1 = fdate.Split(' ');
                todate1 = todate.Split(' ');
                fdate = fdate1[0].ToString() + " 01:00 AM";
                todate = todate1[0].ToString() + " 11:55 PM";

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
            // table name has been changed tbl_master_lead to tbl_master_contact 14122016

            if (productcon == "")
            {
                if (Lrd.Checked == true)
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextcall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 4) and (tbl_trans_phonecall.phc_nextcall >= '" + fdate + "') and (tbl_trans_phonecall.phc_nextcall <= '" + todate + "') order by CAST(tbl_trans_phonecall.phc_nextCall AS dateTime),add_address3");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextcall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 4) and (tbl_trans_phonecall.phc_nextcall >= '" + fdate + "') and (tbl_trans_phonecall.phc_nextcall <='" + todate + "') order by CAST(tbl_trans_phonecall.phc_nextCall AS dateTime),add_address3");
                }
            }
            else
            {
                if (Lrd.Checked == true)
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextcall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 4) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "') and (tbl_trans_phonecall.phc_nextcall >= '" + fdate + "') and (tbl_trans_phonecall.phc_nextcall <= '" + todate + "') order by CAST(tbl_trans_phonecall.phc_nextCall AS dateTime),add_address3");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextcall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 4) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "') and (tbl_trans_phonecall.phc_nextcall >= '" + fdate + "') and (tbl_trans_phonecall.phc_nextcall <= '" + todate + "') order by CAST(tbl_trans_phonecall.phc_nextCall AS dateTime),add_address3");
                }
            }

            // End table name has been changed tbl_master_lead to tbl_master_contact 14122016

            DataTable dt_main = new DataTable();
            dt_main.Columns.Add("NextVisit DateTime");
            dt_main.Columns.Add("LeadId");
            dt_main.Columns.Add("Name");
            dt_main.Columns.Add("Address");
            dt_main.Columns.Add("PhoneNo");
            dt_main.Columns.Add("Product");
            dt_main.Columns.Add("TeleCaller");
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
                    lblError.Text = "There is no sales visit";
                    btnSelectAll.Visible = false;
                }
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow drNew = dt_main.NewRow();
                    drNew[0] = dt.Rows[i]["phc_nextcall"].ToString();
                    drNew[1] = dt.Rows[i]["phc_leadcotactid"].ToString();
                    drNew[2] = dt.Rows[i]["Name"].ToString();
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
                            DataTable dt_product = new DataTable();
                            dt_product = oDBEngine.GetDataTable("tbl_trans_OfferedProduct", "CASE (isnull(ofp_productId, '0')) WHEN '0' THEN isnull(ofp_productTypeId, '') ELSE (SELECT prds_description FROM tbl_master_products WHERE (prds_internalId = tbl_trans_offeredproduct.ofp_productid)) END AS Product", " ofp_productTypeId='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'");
                            if (dt_product != null)
                            {
                                for (int j = 0; j <= dt_product.Rows.Count - 1; j++)
                                {
                                    if (j != 0)
                                    {
                                        product += "  (" + j + 1 + ")" + dt_product.Rows[j]["Product"].ToString();
                                    }
                                    else
                                    {
                                        product = dt_product.Rows[j]["Product"].ToString();
                                    }
                                }
                            }
                            break;
                    }
                    string phoneno = "";
                    DataTable dt_phonecall = new DataTable();
                    dt_phonecall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'");
                    if (dt_phonecall != null)
                    {
                        for (int ij = 0; ij <= dt_phonecall.Rows.Count - 1; ij++)
                        {
                            switch (dt_phonecall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    phoneno += "(M)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    phoneno += "(R)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    phoneno += "(O)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    if (phoneno == "")
                    {
                        phoneno = "N/A";
                    }
                    drNew[5] = product;
                    drNew[4] = phoneno;
                    drNew[3] = dt.Rows[i]["Address"].ToString();
                    string[,] uname = oDBEngine.GetFieldValue("tbl_master_user", "User_name", " user_id=" + dt.Rows[i]["act_AssignedTo"].ToString(), 1);
                    if (uname[0, 0] != "n")
                    {
                        drNew[6] = uname[0, 0];
                    }
                    DataTable dt_temp = new DataTable();
                    dt_temp = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN tbl_master_costCenter ON tbl_trans_Activies.act_branchId = tbl_master_costCenter.cost_id INNER JOIN tbl_master_branch ON tbl_trans_salesVisit.slv_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId AND tbl_trans_Activies.act_activityNo = tbl_trans_offeredProduct.ofp_activityId", "tbl_master_user.user_name, tbl_master_costCenter.cost_description, tbl_master_branch.branch_description, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome, tbl_master_user.user_name AS ReportingHead", " (tbl_trans_salesVisit.slv_leadcotactId = '" + dt.Rows[i]["phc_leadcotactid"].ToString() + "')  AND (tbl_trans_offeredProduct.ofp_activityId IS NOT NULL) and (tbl_trans_offeredproduct.ofp_productTypeid='" + dt.Rows[i]["Product"].ToString() + "')");
                    if (dt_temp != null)
                    {
                        if (dt_temp.Rows.Count == 0)
                        {
                            drNew[7] = "N/A";
                            drNew[8] = "N/A";
                            drNew[9] = "N/A";
                            drNew[10] = "Due";
                        }
                        else
                        {
                            if (leadid == "")
                            {
                                leadid = i.ToString();
                            }
                            else
                            {
                                leadid += "," + i.ToString();
                            }
                            drNew[7] = dt_temp.Rows[0]["cost_description"].ToString();
                            drNew[8] = dt_temp.Rows[0]["branch_description"].ToString();
                            drNew[9] = dt_temp.Rows[0]["user_name"].ToString() + "[" + dt_temp.Rows[0]["ReportingHead"].ToString() + "]";
                            drNew[10] = dt_temp.Rows[0]["slv_SalesVisitOutcome"].ToString();
                        }
                    }
                    string[,] ofp_id = oDBEngine.GetFieldValue("tbl_trans_offeredProduct", "top 1 ofp_id", " ofp_producttypeid='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'", 1);
                    if (ofp_id[0, 0] != "n")
                    {
                        drNew[11] = ofp_id[0, 0];
                    }
                    switch (drpselectedval)
                    {
                        case "All":
                            dt_main.Rows.Add(drNew);
                            break;
                        case "Assigned":
                            if (drNew[7].ToString() != "N/A")
                                dt_main.Rows.Add(drNew);
                            break;
                        case "UnAssigned":
                            if (drNew[7].ToString() == "N/A")
                                dt_main.Rows.Add(drNew);
                            break;
                    }
                }
            }
            lblTotalRecord.Text = "Total Record  : " + dt_main.Rows.Count;
            if (dt_main.Rows.Count != 0)
            {
                btnSelectAll.Visible = true;
                btnExport.Visible = true;
                frmAllot.Visible = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "SalesVisit2", "<script language='JavaScript'>VisibilityForSalesVisit();</script>");

            }
            else
            {
                btnSelectAll.Visible = false;
                btnExport.Visible = false;
                frmAllot.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "SalesVisit3", "<script language='JavaScript'>VisibilityForSalesVisit();</script>");

            }
            grdGenratedSalesVisit.DataSource = dt_main.DefaultView;
            grdGenratedSalesVisit.DataBind();
            ViewState["tempDt"] = dt_main;
            DataTable temp_dt = new DataTable();
            temp_dt.Columns.Add("NextVisit DateTime");
            temp_dt.Columns.Add("LeadId");
            temp_dt.Columns.Add("Name");
            temp_dt.Columns.Add("Address");
            temp_dt.Columns.Add("PhoneNo");
            temp_dt.Columns.Add("Product");
            temp_dt.Columns.Add("TellCaller");
            temp_dt.Columns.Add("Alloted Department");
            temp_dt.Columns.Add("Alloted Branch");
            temp_dt.Columns.Add("Alloted User");
            temp_dt.Columns.Add("Status");
            for (int i = 0; i <= dt_main.Rows.Count - 1; i++)
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
            if (dt_main.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('No Record Found')</script>");
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
        protected void grdGenratedSalesVisit_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdGenratedSalesVisit_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["DtSorting"] = sortExpression;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            dv.Sort = sortExpression;
            grdGenratedSalesVisit.DataSource = dv;
            grdGenratedSalesVisit.DataBind();
        }
        protected void btnCancelGenratedSalesVisit_Click(object sender, EventArgs e)
        {
            userInfo1.Visible = true;
            pnlShowDetail.Visible = false;
            pnlActivityDetail.Visible = false;
            frmShowCall.Visible = false;
            btnCreate.Enabled = true;
            btnGenratedSalesVisit.Enabled = true;
            btnModify.Enabled = false;
            FillGridInfo();
            pnlShowDetail.Visible = false;
            frmShowCall.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            FrmShowGenratedSales.Visible = false;
            frmShowCourtesyCall.Visible = false;
            ViewState["Status"] = "";
            //grdUserInfo.Visible = true;
            grdUserInfo.Visible = true;
            drdExport.Visible = true;//sanjib added for export excel/pdf14122016

            trgrid.Visible = true;
            //grdUserInfo.Enabled = true;
            grdUserInfo.Enabled = false;
            pnlActivityDetail.Visible = true;
            drpActType.Enabled = true;
            drpUserWork.Enabled = true;
            userInfo1.Enabled = true;
            btnDelegate.Visible = false;
            btnModify.Enabled = false;
            btnReassign.Visible = false;
            btnReschedule.Visible = false;
            btnShowDetail.Visible = false;
        }
        protected void btnShowCourtesyCall_Click(object sender, EventArgs e)
        {
            string sdate = "";
            string edate = "";
            string fdate;
            string todate;
            string user = "";
            string[] fdate1;
            string[] todate1;
            fdate = CourtesyStartCallDate.Value.ToString();
            todate = CourtesyEndCallDate.Value.ToString();
            fdate1 = fdate.Split(' ');
            todate1 = todate.Split(' ');
            fdate = fdate1[0].ToString() + " 01:00 AM";
            todate = todate1[0].ToString() + " 11:55 PM";
            if (e.ToString() != "")
            {

                if (drpSelectedUser.SelectedValue == "0")
                {
                    user = oDBEngine.getChildUser(Session["userid"].ToString(), user);
                }
                else
                {
                    user = drpSelectedUser.SelectedValue.ToString();
                }
            }
            //drpSelectedUser.SelectedValue = Convert.ToString(drpSelectedUser.Items.FindByValue(user).Value);
            //if (sdate == "" || edate == "")
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Please select the From and Todate";
            //}
            //else
            //{
            //    lblError.Visible = false;
            //    lblError.Text = "";
            //}

            DataTable dt_main = new DataTable();
            dt_main.Columns.Add("Courtesy Call");
            dt_main.Columns.Add("Orginal Meeting Date");
            dt_main.Columns.Add("LeadId");
            dt_main.Columns.Add("Name");
            dt_main.Columns.Add("Address");
            dt_main.Columns.Add("Phone");
            dt_main.Columns.Add("TeleCaller");
            dt_main.Columns.Add("Assigned To");
            dt_main.Columns.Add("Last Outcome");
            dt_main.Columns.Add("NextVisit");
            dt_main.Columns.Add("Last CourtesyCall");
            dt_main.Columns.Add("History");
            DataTable dt = new DataTable();
            if (CLrd.Checked == true)
            {
                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN tbl_trans_salesVisit ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_salesVisit.slv_leadcotactId INNER JOIN tbl_trans_Activies tbl_trans_Activies_1 ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies_1.act_id INNER JOIN tbl_master_user ON tbl_trans_Activies_1.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", " distinct tbl_trans_phonecall.phc_leadcotactId AS LeadId, convert(varchar(17),tbl_trans_salesVisit.slv_nextvisitdatetime,113) AS NextVisit, tbl_master_user.user_name AS AssignedTo, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, (SELECT     TOP 1 ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address FROM tbl_master_address WHERE      add_cntid = tbl_master_contact.cnt_internalid) AS Address,convert(varchar,tbl_trans_phonecall.phc_nextCall,100) AS NextCall,  tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as OutCome", " tbl_trans_Activies.act_assignedTo in (" + user + ") and (tbl_trans_phonecall.phc_nextcall  >'" + fdate + "') and (tbl_trans_phonecall.phc_nextcall > '" + todate + "')");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN tbl_trans_salesVisit ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_salesVisit.slv_leadcotactId INNER JOIN tbl_trans_Activies tbl_trans_Activies_1 ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies_1.act_id INNER JOIN tbl_master_user ON tbl_trans_Activies_1.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "distinct tbl_trans_phonecall.phc_leadcotactId AS LeadId, convert(varchar(17),tbl_trans_salesVisit.slv_nextvisitdatetime,113) AS NextVisit, tbl_master_user.user_name AS AssignedTo, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, (SELECT     TOP 1 ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address FROM tbl_master_address WHERE      add_cntid = tbl_master_contact.cnt_internalid) AS Address,convert(varchar(17),tbl_trans_phonecall.phc_nextCall,113) AS NextCall,  tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as OutCome", " tbl_trans_Activies.act_assignedTo in (" + user + ") and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) >= '" + fdate + "') and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) <= '" + todate + "')");
            }
            if (dt != null)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt_main.NewRow();
                    dr[0] = "<div style='CURSOR: hand;color:#330099;' onclick= javascript:frmOpenNewWindow1('frmphoneCall_CourtesyCall.aspx?id=" + dt.Rows[i]["LeadId"].ToString() + "',300,800)>CourtesyCall</div>";
                    dr[1] = dt.Rows[i]["NextCall"].ToString();
                    dr[2] = dt.Rows[i]["LeadId"].ToString();
                    dr[3] = dt.Rows[i]["Name"].ToString() + "[" + dt.Rows[i]["LeadId"].ToString() + "]";
                    dr[4] = dt.Rows[i]["Address"].ToString();
                    string phoneno = "";
                    DataTable dt_phonecall = new DataTable();
                    dt_phonecall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["LeadId"].ToString() + "'");
                    if (dt_phonecall != null)
                    {
                        for (int ij = 0; ij <= dt_phonecall.Rows.Count - 1; ij++)
                        {
                            switch (dt_phonecall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    phoneno += "(M)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    phoneno += "(R)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    phoneno += "(O)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    if (phoneno == "")
                    {
                        phoneno = "N/A";
                    }
                    dr[5] = phoneno;
                    string[,] name1 = oDBEngine.GetFieldValue("tbl_master_user INNER JOIN tbl_trans_Activies ON tbl_master_user.user_id = tbl_trans_Activies.act_assignedTo INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "tbl_master_user.user_name", " phc_leadcotactid='" + dt.Rows[i]["LeadId"].ToString() + "'", 1);
                    if (name1[0, 0] != "n")
                    {
                        dr[6] = name1[0, 0];
                    }
                    dr[7] = dt.Rows[i]["AssignedTo"].ToString();
                    dr[8] = dt.Rows[i]["Outcome"].ToString();
                    dr[9] = dt.Rows[i]["NextVisit"].ToString();
                    string[,] stime = oDBEngine.GetFieldValue("tbl_trans_CourtesyCalls", "top 1 convert(varchar(25),cpc_callStartTime,113)", " cpc_leadcontactid='" + dt.Rows[i]["LeadId"].ToString() + "'", 1);
                    if (stime[0, 0] != "n")
                    {
                        dr[10] = stime[0, 0];
                    }
                    dr[11] = "<div style='CURSOR: hand;color:#330099;' onclick= javascript:frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + dt.Rows[i]["LeadId"].ToString() + "',300,800)>History</div>";
                    dt_main.Rows.Add(dr);
                }
            }
            ViewState["tempDt"] = dt_main;
            grdCourtestCall.DataSource = dt_main.DefaultView;
            grdCourtestCall.DataBind();
            if (dt_main.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('No Record Found')</script>");
            }
        }
        protected void grdCourtestCall_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCourtestCall.PageIndex = e.NewPageIndex;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            if (ViewState["DtSorting"] != null)
            {
                dv.Sort = ViewState["DtSorting"].ToString();
            }
            grdCourtestCall.DataSource = dv;
            grdCourtestCall.DataBind();
        }
        protected void grdCourtestCall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = Server.HtmlDecode(e.Row.Cells[i].Text.ToString());
                }
            }
        }
        protected void grdCourtestCall_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["DtSorting"] = sortExpression;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            dv.Sort = sortExpression;
            grdCourtestCall.DataSource = dv;
            grdCourtestCall.DataBind();
        }
        protected void btnShowSales_Click(object sender, EventArgs e)
        {
            btnSelectAllSales.Visible = false;
            btnSalesExport.Visible = false;
            grdSales.Visible = true;
            frmSalesAllot.Visible = false;
            btnSalesExport.Visible = false;
            btnSelectAllSales.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "Visibility1", "<script language='JavaScript'>InVisibilityForSales();</script>");

            lblSalesTotalRecord.Text = "";
            string fdate = "";
            string productcon = "";
            string drpselectedval = "";
            string drpselectedproduct = "";
            string leadid = "";
            string todate = "";
            string[] fdate1;
            string[] todate1;
            if (e.ToString() != "")
            {
                fdate = SalesStartDate.Value.ToString();
                todate = SalesEndDate.Value.ToString();
                fdate1 = fdate.Split(' ');
                todate1 = todate.Split(' ');
                fdate = fdate1[0].ToString() + " 01:00 AM";
                todate = todate1[0].ToString() + " 11:55 PM";

                if (Radio3.Checked == true)
                {
                    productcon = "ALL";
                }
                else
                {
                    productcon = "";
                }
                drpselectedproduct = drpSalesProduct.SelectedItem.ToString();
                drpselectedval = drpSalesSelect.SelectedItem.ToString();
            }
            if (productcon == "ALL")
            {
                Radio3.Checked = true;
            }
            else
            {
                Radio4.Checked = true;
                drpSalesProduct.SelectedValue = Convert.ToString(drpSalesProduct.Items.FindByText(drpselectedproduct).Value);
            }
            drpSalesSelect.SelectedValue = Convert.ToString(drpSalesSelect.Items.FindByText(drpselectedval).Value);
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
                if (LGLrd.Checked == true)
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextCall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) and tbl_trans_phonecall.phc_addid=tbl_master_address.add_id AND (tbl_master_calldispositions.Call_Category = 5) and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) >= '" + fdate + "') and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) <= '" + todate + "')");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextCall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 5) and tbl_trans_phonecall.phc_addid=tbl_master_address.add_id and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) >= '" + fdate + "') and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) <= '" + todate + "')");
                }
            }
            else
            {
                if (LGLrd.Checked == true)
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextCall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 5) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "')and tbl_trans_phonecall.phc_addid=tbl_master_address.add_id and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) >='" + fdate + "') and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) <= '" + todate + "')");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar,tbl_trans_phonecall.phc_nextCall,100) as phc_nextCall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " (tbl_trans_Activies.act_assignedTo IN (" + all_user + ")) AND (tbl_master_calldispositions.Call_Category = 5) and (tbl_trans_offeredproduct.ofp_producttypeid='" + drpselectedproduct + "') and tbl_trans_phonecall.phc_addid=tbl_master_address.add_id and(CAST(tbl_trans_phonecall.phc_nextcall AS datetime) >= '" + fdate + "') and (CAST(tbl_trans_phonecall.phc_nextcall AS datetime) <= '" + todate + "')");
                }
            }
            DataTable dt_main = new DataTable();
            dt_main.Columns.Add("NextVisit DateTime");
            dt_main.Columns.Add("LeadId");
            dt_main.Columns.Add("Name");
            dt_main.Columns.Add("Address");
            dt_main.Columns.Add("PhoneNo");
            dt_main.Columns.Add("Product");
            dt_main.Columns.Add("TeleCaller");
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
                    lblError.Text = "There is no sales visit";
                    //btnSalesSelectAll.Visible = false;
                }
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow drNew = dt_main.NewRow();
                    drNew[0] = dt.Rows[i]["phc_nextcall"].ToString();
                    drNew[1] = dt.Rows[i]["phc_leadcotactid"].ToString();
                    drNew[2] = dt.Rows[i]["Name"].ToString();
                    DataTable dt_product = new DataTable();
                    dt_product = oDBEngine.GetDataTable("tbl_trans_OfferedProduct", "CASE (isnull(ofp_productId, '0')) WHEN '0' THEN isnull(ofp_productTypeId, '') ELSE (SELECT prds_description FROM tbl_master_products WHERE (prds_internalId = tbl_trans_offeredproduct.ofp_productid)) END AS Product", " ofp_productTypeId='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'");
                    string product = "";
                    if (dt_product != null)
                    {
                        for (int j = 0; j <= dt_product.Rows.Count - 1; j++)
                        {
                            if (j != 0)
                            {
                                product += "  (" + j + 1 + ")" + dt_product.Rows[j]["Product"].ToString();
                            }
                            else
                            {
                                product = dt_product.Rows[j]["Product"].ToString();
                            }
                        }
                    }
                    string phoneno = "";
                    DataTable dt_phonecall = new DataTable();
                    dt_phonecall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'");
                    if (dt_phonecall != null)
                    {
                        for (int ij = 0; ij <= dt_phonecall.Rows.Count - 1; ij++)
                        {
                            switch (dt_phonecall.Rows[ij]["phf_type"].ToString().ToUpper())
                            {
                                case "MOBILE":
                                    phoneno += "(M)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "RESIDENCE":
                                    phoneno += "(R)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                                case "OFFICIAL":
                                    phoneno += "(O)" + dt_phonecall.Rows[ij]["phf_phoneNumber"].ToString();
                                    break;
                            }
                        }
                    }
                    if (phoneno == "")
                    {
                        phoneno = "N/A";
                    }
                    drNew[5] = product;
                    drNew[4] = phoneno;
                    drNew[3] = dt.Rows[i]["Address"].ToString();
                    string[,] uname = oDBEngine.GetFieldValue("tbl_master_user", "User_name", " user_id=" + dt.Rows[i]["act_AssignedTo"].ToString(), 1);
                    if (uname[0, 0] != "n")
                    {
                        drNew[6] = uname[0, 0];
                    }
                    DataTable dt_temp = new DataTable();
                    dt_temp = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_trans_Activies ON tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id INNER JOIN tbl_master_SalesStatus ON tbl_trans_Sales.sls_sales_status = tbl_master_SalesStatus.sls_id INNER JOIN  tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_reportTo = tbl_master_employee.emp_id INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_master_employee.emp_contactId = tbl_master_user_1.user_contactId INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id", "tbl_master_SalesStatus.sls_status, tbl_trans_employeeCTC.emp_reportTo, tbl_master_user.user_name, tbl_master_user_1.user_name AS ReportingHead, tbl_master_branch.branch_description, tbl_master_costCenter.cost_description", " tbl_trans_Sales.sls_contactlead_id='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'");
                    if (dt_temp != null)
                    {
                        if (dt_temp.Rows.Count == 0)
                        {
                            drNew[7] = "N/A";
                            drNew[8] = "N/A";
                            drNew[9] = "N/A";
                            drNew[10] = "Due";
                        }
                        else
                        {
                            if (leadid == "")
                            {
                                leadid = i.ToString();
                            }
                            else
                            {
                                leadid += "," + i.ToString();
                            }
                            drNew[7] = dt_temp.Rows[0]["cost_description"].ToString();
                            drNew[8] = dt_temp.Rows[0]["branch_description"].ToString();
                            drNew[9] = dt_temp.Rows[0]["user_name"].ToString() + "[" + dt_temp.Rows[0]["ReportingHead"].ToString() + "]";
                            drNew[10] = dt_temp.Rows[0]["sls_status"].ToString();
                        }
                    }
                    string[,] ofp_id = oDBEngine.GetFieldValue("tbl_trans_offeredProduct", "top 1 ofp_id", " ofp_producttypeid='" + dt.Rows[i]["Product"].ToString() + "' and ofp_leadid='" + dt.Rows[i]["phc_leadcotactid"].ToString() + "'", 1);
                    if (ofp_id[0, 0] != "n")
                    {
                        drNew[11] = ofp_id[0, 0];
                    }
                    switch (drpselectedval)
                    {
                        case "All":
                            dt_main.Rows.Add(drNew);
                            break;
                        case "Assigned":
                            if (drNew[7].ToString() != "N/A")
                                dt_main.Rows.Add(drNew);
                            break;
                        case "UnAssigned":
                            if (drNew[7].ToString() == "N/A")
                                dt_main.Rows.Add(drNew);
                            break;
                    }
                }
            }
            lblTotalRecord.Text = "Total Record  : " + dt_main.Rows.Count;
            if (dt_main.Rows.Count != 0)
            {
                //btnSalesSelectAll.Visible = true;
                btnSalesExport.Visible = true;
                btnSelectAllSales.Visible = true;
                frmSalesAllot.Visible = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "Visibility", "<script language='JavaScript'>VisibilityForSales();</script>");


            }
            else
            {
                //btnSalesSelectAll.Visible = false;


                btnSalesExport.Visible = false;
                btnSelectAllSales.Visible = false;
                frmSalesAllot.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "Visibility2", "<script language='JavaScript'>InVisibilityForSales();</script>");

            }
            grdSales.DataSource = dt_main.DefaultView;
            grdSales.DataBind();
            ViewState["tempDt"] = dt_main;
            DataTable temp_dt = new DataTable();
            temp_dt.Columns.Add("NextVisit DateTime");
            temp_dt.Columns.Add("LeadId");
            temp_dt.Columns.Add("Name");
            temp_dt.Columns.Add("Address");
            temp_dt.Columns.Add("PhoneNo");
            temp_dt.Columns.Add("Product");
            temp_dt.Columns.Add("TellCaller");
            temp_dt.Columns.Add("Alloted Department");
            temp_dt.Columns.Add("Alloted Branch");
            temp_dt.Columns.Add("Alloted User");
            temp_dt.Columns.Add("Status");
            for (int i = 0; i <= dt_main.Rows.Count - 1; i++)
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
            string LeadID1 = txtLeadId1.Value;
            Session["GridDateReport"] = temp_dt;
            Session["FileName"] = "Genrated Sales Visit From " + fdate + " To " + todate;
            if (LeadID1 != null)
            {
                for (int g = 0; g <= grdSales.Rows.Count - 1; g++)
                {
                    string[] str = LeadID1.Split(',');
                    for (int i = 0; i <= str.GetUpperBound(0); i++)
                    {
                        if (str[i] != "")
                        {
                            string[] id = str[i].Split('@');
                            Label lbl = (Label)grdSales.Rows[g].FindControl("lblId");
                            string[] l = lbl.Text.Split('@');
                            if (l[0] == id[0] && l[4] == id[1])
                            {
                                CheckBox chk = (CheckBox)grdSales.Rows[g].FindControl("chkSel");
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
            if (dt_main.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('No Record Found')</script>");
            }
        }
        protected void grdSales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSales.PageIndex = e.NewPageIndex;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            grdSales.DataSource = dv;
            grdSales.DataBind();
            string LeadID1 = txtLeadId1.Value;
            if (LeadID1 != null)
            {
                for (int g = 0; g <= grdSales.Rows.Count - 1; g++)
                {
                    string[] str = LeadID1.Split(',');
                    for (int i = 0; i <= str.GetUpperBound(0); i++)
                    {
                        if (str[i] != "")
                        {
                            string[] id = str[i].Split('@');
                            Label lbl = (Label)grdSales.Rows[g].FindControl("lblId");
                            string[] l = lbl.Text.Split('@');
                            if (l[0] == id[0] && l[4] == id[1])
                            {
                                CheckBox chk = (CheckBox)grdSales.Rows[g].FindControl("chkSel");
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        protected void grdSales_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        chk.Attributes.Add("onclick", "javascript:FillLeadId1(this,'" + lbl.Text + "');");
                    }
                    else
                    {
                        if (ViewState["val"].ToString() != "")
                        {
                            chk.Checked = true;
                            Label lbl = (Label)e.Row.FindControl("lblId");
                            chk.Attributes.Add("onclick", "javascript:FillLeadId1(this,'" + lbl.Text + "');");
                            string Jscript = "<script language='javascript'>";
                            Jscript += "FillLeadId123(this,'" + lbl.Text + "');";
                            Jscript = "</script>";
                            ClientScript.RegisterStartupScript(GetType(), "JScript", Jscript);
                        }
                        else
                        {
                            chk.Checked = false;
                            Label lbl = (Label)e.Row.FindControl("lblId");
                            chk.Attributes.Add("onclick", "javascript:FillLeadId1(this,'" + lbl.Text + "');");
                        }
                    }
                }
            }
        }
        protected void grdSales_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["DtSorting"] = sortExpression;
            DataView dv = new DataView((DataTable)ViewState["tempDt"]);
            dv.Sort = sortExpression;
            grdGenratedSalesVisit.DataSource = dv;
            grdGenratedSalesVisit.DataBind();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Activities/crm_sales.aspx");
            //Page.ClientScript.RegisterStartupScript(GetType(), "CallHeight", "<script language='JavaScrit'>height();</script>");
        }
        //protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //e.Row.Cells[2].Visible = false;
        //}
        protected void btnSalesCanCel_Click(object sender, EventArgs e)
        {
            userInfo1.Visible = true;
            pnlShowDetail.Visible = false;
            pnlActivityDetail.Visible = false;
            frmShowCall.Visible = false;
            btnCreate.Enabled = true;
            btnGenratedSalesVisit.Enabled = true;
            btnModify.Enabled = false;
            FillGridInfo();
            pnlShowDetail.Visible = false;
            frmShowCall.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            FrmShowGenratedSales.Visible = false;
            frmShowCourtesyCall.Visible = false;
            ViewState["Status"] = "";
            //grdUserInfo.Visible = true;
            grdUserInfo.Visible = true;
            drdExport.Visible = true;//sanjib added for export excel/pdf14122016

            trgrid.Visible = true;
            //grdUserInfo.Enabled = true;
            grdUserInfo.Enabled = true;
            pnlActivityDetail.Visible = true;
            drpActType.Enabled = true;
            drpUserWork.Enabled = true;
            userInfo1.Enabled = true;
            btnDelegate.Visible = false;
            btnModify.Enabled = false;
            btnReassign.Visible = false;
            btnReschedule.Visible = false;
            btnShowDetail.Visible = false;
            //DataTable dt = new DataTable();
            //dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_trans_offeredProduct ON tbl_trans_phonecall.phc_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId  AND  tbl_trans_phonecall.phc_id = tbl_master_address.add_activityId", "convert(varchar(17),tbl_trans_phonecall.phc_nextCall,113) as phc_nextCall, tbl_trans_phonecall.phc_leadcotactId, tbl_trans_offeredProduct.ofp_productTypeId as Product, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middleName,'') + ' ' +  isnull(tbl_master_contact.cnt_lastName,'') as Name,  ISNULL(tbl_master_address.add_landMark, '') + '[' + ISNULL(tbl_master_address.add_address1, '') + ' , ' + ISNULL(tbl_master_address.add_address2, '') + ' , ' + ISNULL(tbl_master_address.add_address3, '') + '] '  + ISNULL(tbl_master_address.add_pin, '') AS Address, tbl_trans_Activies.act_assignedTo, tbl_trans_phonecall.phc_note", " 1=2");
            //grdSales.DataSource = dt;
            //grdSales.DataBind();
        }
        public void BtnProperty(Button b1, Button b2, Button b3, Button b4, Button b5)
        {
            //b1.ForeColor = System.Drawing.Color.Black;
            //b2.ForeColor = System.Drawing.Color.Black;
            //b3.ForeColor = System.Drawing.Color.Black;
            //b4.ForeColor = System.Drawing.Color.Black;
            //b5.ForeColor = System.Drawing.Color.Blue;
        }
        public void BtnProperty1(Button b1, Button b2, Button b3, Button b4, Button b5)
        {
            //b1.ForeColor = System.Drawing.Color.Black;
            //b2.ForeColor = System.Drawing.Color.Black;
            //b3.ForeColor = System.Drawing.Color.Black;
            //b4.ForeColor = System.Drawing.Color.Blue;
            //b5.ForeColor = System.Drawing.Color.Black;
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            //comment by sanjib due to the url does't change while cancel the button when new entry or other button are fire with query string  13122016

            //userInfo1.Visible = true;
            //pnlShowDetail.Visible = false;
            //pnlActivityDetail.Visible = false;
            //frmShowCall.Visible = false;
            //btnCreate.Enabled = true;
            //btnGenratedSalesVisit.Enabled = true;
            //btnModify.Enabled = false;
            //FillGridInfo();
            //pnlShowDetail.Visible = false;
            //frmShowCall.Visible = false;
            //frmShowGenratedSalesvisit.Visible = false;
            //FrmShowGenratedSales.Visible = false;
            //frmShowCourtesyCall.Visible = false;
            ////grdUserInfo.Visible = true;
            //grdUserInfo.Visible = true; 
            //trgrid.Visible = true;
            ////grdUserInfo.Enabled = true;
            //grdUserInfo.Enabled = true; 

            //pnlActivityDetail.Visible = true;
            //drpActType.Enabled = true;
            //drpUserWork.Enabled = true;
            //userInfo1.Enabled = true;
            //btnDelegate.Visible = false;
            //btnModify.Enabled = false;
            //btnReassign.Visible = false;
            //btnReschedule.Visible = false;
            //btnShowDetail.Visible = false;
            //BtnProperty(btnGenratedSalesVisit, btnGenratedSales, btnCourtesyCall, btnCreate, btnCancel1);
            //btnModify.Visible = false;
            //lblUserName.Visible = false;
            //txtUser.Visible = false;

            //added by sanjib on 13-12-2016 due to url was unable to change while click on grid cloumn.
            Response.Redirect("CrmPhoneCallActivityWithIFrame.aspx");


        }
        //comment by sanjib dueto grid chnage 13122016
        //protected void grdDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdDetail.PageIndex = e.NewPageIndex;
        //    FillGrid();
        //}
        //protected void grdUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdUserInfo.PageIndex = e.NewPageIndex;
        //    FillGridInfo();
        //}
        //protected void btnSalesSelectAll_Click(object sender, EventArgs e)
        //{
        //{
        //    if (btnSelectAll.Text == "Deselect All")
        //    {
        //        btnSelectAll.Text = "Select All";
        //        txtLeadId.Value = "";
        //        foreach (GridViewRow row in grdGenratedSalesVisit.Rows)
        //        {
        //            CheckBox chk = (CheckBox)row.FindControl("chkSel");
        //            if (chk.Checked == true)
        //            {
        //                chk.Checked = false;
        //            }
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        btnSelectAll.Text = "Deselect All";
        //        btnshowGenratedSaleVisit_Click(sender, e);
        //        DataTable dt = (DataTable)ViewState["tempDt"];
        //        ViewState["val"] = "Yes";
        //        grdGenratedSalesVisit.DataSource = dt.DefaultView;
        //        grdGenratedSalesVisit.DataBind();
        //        ViewState["val"] = "";
        //    }
        //    ViewState["val"] = "";
        //    if ((DataTable)ViewState["tempDt"] != null)
        //    {
        //        string leadid = "";
        //        DataTable dt = (DataTable)ViewState["tempDt"];
        //        if (dt != null)
        //        {
        //            for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                string lblId = dt.Rows[i]["LeadId"].ToString() + "@@@@" + dt.Rows[i]["ProductId"].ToString();
        //                string str = dt.Rows[i]["Alloted Branch"].ToString();
        //                if (str == "N/A")
        //                {
        //                    if (leadid == "")
        //                    {
        //                        leadid = lblId;
        //                    }
        //                    else
        //                    {
        //                        leadid += "," + lblId;
        //                    }
        //                }
        //            }
        //            //grdGenratedSalesVisit.DataSource = dt.DefaultView;
        //            //grdGenratedSalesVisit.DataBind();
        //        }
        //        txtLeadId.Value = leadid;
        //    }

        //}
        //}

        protected void grdCourtestCall_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btnSelectAllSales_Click(object sender, EventArgs e)
        {
            if (btnSelectAllSales.Text == "Deselect All")
            {
                btnSelectAllSales.Text = "Select All";
                txtLeadId.Value = "";
                foreach (GridViewRow row in grdSales.Rows)
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
                btnSelectAllSales.Text = "Deselect All";
                btnShowSales_Click(sender, e);
                DataTable dt = (DataTable)ViewState["tempDt"];
                ViewState["val"] = "Yes";
                grdSales.DataSource = dt.DefaultView;
                grdSales.DataBind();
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

        protected void grdUserInfo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void grdDetailNew_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        //protected void grdUserInfo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{

        //}

        //protected void grdUserInfo_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        //{

        //}

        #region Export System
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string filename = string.Empty;
            if (Session["Contactrequesttype"] != null)
            {
                filename = Convert.ToString(Session["Contactrequesttype"]);
                exporter.PageHeader.Center = Convert.ToString(Session["Contactrequesttype"]);
            }
            else
            {
                filename = "PhoneCallActivity";
                exporter.PageHeader.Center = "Phone Call Activity";
            }

            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            exporter.FileName = filename;
            if (Convert.ToString(Session["Contactrequesttype"]) != "referalagent")
            {


            }
            else
            {
                exporter.ReportHeader = "User phone call";
            }

            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            grdUserInfo.Columns[2].Visible = false;
            grdUserInfo.Columns[1].Visible = false;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    //bindpdf();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }



        #endregion



        public void BindSalesAddForm()
        {

            ViewState["status"] = "CreateNew";
            //grdUserInfo.Visible = false;
            grdUserInfo.Visible = false;
            drdExport.Visible = false;

            trgrid.Visible = false;
            pnlCall.Visible = true;
            drpUserWork.Enabled = true;
            pnlActivityDetail.Visible = false;
            PnlBtn.Visible = false;
            FrmShowGenratedSales.Visible = false;
            frmShowGenratedSalesvisit.Visible = false;
            frmShowCourtesyCall.Visible = false;
            frmShowCall.Visible = true;
            BtnProperty(btnCancel1, btnGenratedSalesVisit, btnGenratedSales, btnCourtesyCall, btnCreate);
            Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }



    }
}


