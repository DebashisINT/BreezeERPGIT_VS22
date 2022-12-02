using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web.Services;
using System.Collections.Generic;
namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frm_sales_Activity : System.Web.UI.Page
    {
        string UserId;
        string _userId1;
        string _temp;
        string _ActId;
        string Id = "";
        string[] chrk1;
        string leadid;//Lead Id and Product Id is used for assign the activity(Calling the page frmallot_sales_new)
        string ProductId;
        string product;
        string[] _tempProd;
        string[] _tempProdDetail;
        String DataValue;
        string _actNo;

       // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        clsDropDownList clsdropdown = new clsDropDownList();
        public string pageAccess = "";

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
            //Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script language='JavaScript'>height();</script>");

            //string todayDate = convert.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");

            UserId = HttpContext.Current.Session["userid"].ToString();//Session UserID
            if (Request.QueryString["id"] != null)
            {
                Id = Request.QueryString["id"].ToString();
            }

            ActivitySalesDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            txtStartDate.EditFormatString = OConvert.GetDateFormat("DateTime");
            txtEndDate.EditFormatString = OConvert.GetDateFormat("DateTime");
            txtStartDate.Attributes.Add("readonly", "true");
            txtEndDate.Attributes.Add("readonly", "true");
            txtStartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            txtEndDate.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(1).ToString());
            drpPriority.SelectedValue = "1";

           
            if (!IsPostBack)
            {
                ShowData();
                FilldrpActType();
              //  FilldrpUserWork();
                drpActType.SelectedValue = "6";
                drpActType.Enabled = false;
                //txtAssigned.Attributes.Add("onfocus", "CallList(this,'SearchByEmpRportto',event)");
                //txtAssigned.Attributes.Add("onclick", "CallList(this,'SearchByEmpRportto',event)");
                //txtAssigned.Attributes.Add("onkeyup", "CallList(this,'SearchByEmpRportto',event)");


            }
            if (Id != "")
            {
                if (txtId.Text != "" && txtId.Text == Id)
                {
                }
                else
                {
                    fillGrid();
                    txtId.Text = Id;
                }
            }
        }

        protected void FilldrpActType()
        {
            string[,] Data = objEngine.GetFieldValue("tbl_master_activitytype", "aty_id,aty_activityType", null, 2, "aty_activityType");
            string CheckData = Data[0, 0];
            if (CheckData != "n")
            {
                clsdropdown.AddDataToDropDownList(Data, drpActType);
            }
        }
        protected void FilldrpUserWork()
        {
            string[,] Data = objEngine.GetFieldValue("tbl_master_user", "user_id,user_name", "user_id IN(" + _userId1 + ")", 2, "user_name");
            string CheckData = Data[0, 0];
            if (CheckData != "n")
            {
                clsdropdown.AddDataToDropDownList(Data, drpUserWork);
            }
        }
        protected void ShowData()
        {
            DataTable _dt1 = new DataTable();
            string _temp = "";
            _userId1 = objEngine.getChildUser(UserId, _temp);
            //Session["userid1"] = _userId1;
            _dt1 = objEngine.GetDataTable(" tbl_trans_Activies,tbl_trans_Sales,tbl_master_user", " tbl_trans_Activies.act_id AS Id, tbl_master_user.user_name AS Name, tbl_master_user.user_id AS UserId,  tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_trans_Activies.act_expectedDate AS EndDate, tbl_trans_Sales.sls_ProductType as PrdouctType,tbl_trans_Sales.sls_estimated_value, tbl_trans_Sales.sls_datetime AS SalesDate", " (tbl_trans_Activies.act_assignedBy IN (" + _userId1 + ")OR tbl_trans_Activies.act_assignedto in( " + _userId1 + ")) And tbl_trans_Activies.act_id = tbl_trans_Sales.sls_activity_id And tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id Order by tbl_master_user.user_name,tbl_trans_Sales.sls_datetime,tbl_trans_Activies.act_id");
            if (_dt1.Rows.Count > 0)
            {
                DataTable _dtUser = new DataTable();
                DataTable _dtUserInfo = new DataTable();
                DataRow _row = _dtUser.NewRow();
                DataColumn _colUserid = new DataColumn("UserId");
                DataColumn _colSN = new DataColumn("SNo");
                DataColumn _colUser = new DataColumn("User");
                DataColumn _colPenAct = new DataColumn("Pending Acttivity");
                DataColumn _colSchExpDt = new DataColumn("Scheduled End Date");
                DataColumn _colExpEndDt = new DataColumn("Expected End Date");
                DataColumn _colPenCall = new DataColumn("Pending Sales");
                DataColumn _colPenMF = new DataColumn("Pending MF");
                DataColumn _colPenIns = new DataColumn("Pending Ins");
                DataColumn _colPenEq = new DataColumn("Pending EQ");
                DataColumn _colPenIPO = new DataColumn("Pending IPO");
                DataColumn _colComMF = new DataColumn("Completed MF");
                DataColumn _colComIns = new DataColumn("Completed Ins");
                DataColumn _colComEQ = new DataColumn("Completed EQ");
                DataColumn _colComIPO = new DataColumn("Completed IPO");
                _dtUser.Columns.Add(_colUserid);
                _dtUser.Columns.Add(_colSN);
                _dtUser.Columns.Add(_colUser);
                _dtUser.Columns.Add(_colPenAct);
                _dtUser.Columns.Add(_colSchExpDt);
                _dtUser.Columns.Add(_colExpEndDt);
                _dtUser.Columns.Add(_colPenCall);
                _dtUser.Columns.Add(_colPenMF);
                _dtUser.Columns.Add(_colPenIns);
                _dtUser.Columns.Add(_colPenEq);
                _dtUser.Columns.Add(_colPenIPO);
                _dtUser.Columns.Add(_colComMF);
                _dtUser.Columns.Add(_colComIns);
                _dtUser.Columns.Add(_colComEQ);
                _dtUser.Columns.Add(_colComIPO);
                _dtUser.Rows.Add(_row);
                _dtUserInfo = _dtUser.Clone();
                int _userid = 0;
                int _Pencount = 0;
                int _countAct = 1;
                int _sNo = 0;
                DateTime dDate = Convert.ToDateTime(oDBEngine.GetDate());
                DateTime dTempDate = new DateTime();
                int _PendingMF = 0;
                int _PendingIns = 0;
                int _PendingEQ = 0;
                int _PendingIPO = 0;
                int _CompleteMF = 0;
                int _CompleteIns = 0;
                int _CompleteEQ = 0;
                int _CompleteIPO = 0;
                string _schEndDate = "";
                for (int i = 0; i <= _dt1.Rows.Count - 1; i++)
                {
                    _PendingMF = 0;
                    _PendingIns = 0;
                    _PendingEQ = 0;
                    _PendingIPO = 0;
                    _CompleteMF = 0;
                    _CompleteIns = 0;
                    _CompleteEQ = 0;
                    _CompleteIPO = 0;
                    _Pencount = 0;
                    _countAct = 1;
                    _ActId = _dt1.Rows[i]["Id"].ToString();
                    _userid = Convert.ToInt32(_dt1.Rows[i]["UserId"]);
                    string _Name = _dt1.Rows[i]["Name"].ToString();
                    if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                    {
                        string Name = _dt1.Rows[i]["Name"].ToString();
                        while (_Name.Trim() == Name.Trim())
                        {
                            Name = _dt1.Rows[i]["Name"].ToString();
                            string _filterRow = Convert.ToString(_dt1.Select("Id=" + _dt1.Rows[i]["Id"] + "And SalesDate is null"));
                            if (_filterRow.Length > 0)
                            {
                                switch (_dt1.Rows[i]["PrdouctType"].ToString())
                                {
                                    case "Equity/ F & O":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingEQ += 1;
                                        else
                                            _CompleteEQ += 1;
                                        break;
                                    case "Broking & DP Account":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingEQ += 1;
                                        else
                                            _CompleteEQ += 1;
                                        break;
                                    case "Commodity":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingEQ += 1;
                                        else
                                            _CompleteEQ += 1;
                                        break;
                                    case "Mutual Fund":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingMF += 1;
                                        else
                                            _CompleteMF += 1;
                                        break;
                                    case "Insurance-Life":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingIns += 1;
                                        else
                                            _CompleteIns += 1;
                                        break;
                                    case "Insurance-General":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingIns += 1;
                                        else
                                            _CompleteIns += 1;
                                        break;

                                    case "IPO":
                                        if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                            _PendingIPO += 1;
                                        else
                                            _CompleteIPO += 1;
                                        break;

                                }
                                if (_dt1.Rows[i]["SalesDate"].ToString() == "")
                                    _Pencount += 1;
                                if (_ActId != _dt1.Rows[i]["Id"].ToString())
                                {
                                    _countAct += 1;
                                    _ActId = _dt1.Rows[i][0].ToString();

                                }
                                i = i + 1;
                                if (_dt1.Rows.Count > i)
                                {
                                    Name = _dt1.Rows[i]["Name"].ToString();
                                }
                                if (i > _dt1.Rows.Count - 1)
                                    break;
                            }
                        }

                        DataRow _row1 = _dtUserInfo.NewRow();
                        _sNo += 1;
                        //int _totalCall = 0;
                        DateTime _dTempDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                        DateTime Dtime = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString().ToString());
                        if (dTempDate == Dtime)
                        {
                            DateTime dtemptime = new DateTime(oDBEngine.GetDate().Year, oDBEngine.GetDate().Month, oDBEngine.GetDate().Day, 10, 0, 0);
                            DateTime dCurrentTime = oDBEngine.GetDate();
                            TimeSpan k = dCurrentTime - dtemptime;
                            double _dDiff = k.TotalSeconds;
                        }
                        _row1["UserId"] = _userid;
                        _row1["SNo"] = _sNo;
                        _row1["User"] = _Name;
                        _row1["Pending Acttivity"] = _countAct;
                        _row1["Scheduled End Date"] = OConvert.ArrangeDate(dDate.ToShortDateString());
                        _row1["Expected End Date"] = OConvert.ArrangeDate(_schEndDate);
                        _row1["Pending Sales"] = _Pencount;
                        _row1["Pending Mf"] = _PendingMF;
                        _row1["Pending Ins"] = _PendingIns;
                        _row1["Pending EQ"] = _PendingEQ;
                        _row1["Pending IPO"] = _PendingIPO;
                        _row1["Completed Mf"] = _CompleteMF;
                        _row1["Completed Ins"] = _CompleteIns;
                        _row1["Completed EQ"] = _CompleteEQ;
                        _row1["Completed IPO"] = _CompleteIPO;
                        _dtUserInfo.Rows.Add(_row1);
                        i = i - 1;
                    }
                }
                ActivitySalesGrid.DataSource = _dtUserInfo;
                ActivitySalesGrid.DataBind();
            }
        }

        protected void fillGrid()
        {
            DataTable _dt1 = new DataTable();
            string UID = Request.QueryString["id"].ToString();
            _dt1 = objEngine.GetDataTable(" tbl_trans_Activies ,tbl_trans_Sales,tbl_master_user", " tbl_trans_Activies.act_id AS Id,tbl_master_user.user_name AS Name,tbl_master_user.user_id AS UserId,tbl_trans_Activies.act_scheduledDate AS SchDate,tbl_trans_Activies.act_expectedDate AS EndDate,tbl_trans_Sales.sls_ProductType as PrdouctType,tbl_trans_Sales.sls_estimated_value,tbl_trans_Sales.sls_datetime AS SalesDate,tbl_trans_Sales.sls_sales_status", "(tbl_trans_Activies.act_assignedto = '" + UID + "') And tbl_trans_Activies.act_id = tbl_trans_Sales.sls_activity_id  And tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id Order by tbl_trans_Activies.act_id, tbl_trans_Sales.sls_datetime");
            txtUser.Text = _dt1.Rows[0]["Name"].ToString();
            if (txtUser.Text != "")
            {
                lblUserName.Visible = true;

            }
            ViewState["tempUserName"] = txtUser.Text;
            hduser.Value = UID;
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
                    do
                    {
                        _count += 1;
                        int Condition = Convert.ToInt32(_dt1.Rows[i]["sls_sales_status"].ToString());
                        switch (Condition)
                        {
                            case 2:
                                _CountWon += 1;
                                break;
                            case 3:
                                _CountInProcess += 1;
                                break;
                            case 4:
                                _CountPending += 1;
                                break;
                            case 5:
                                _CountNonUsable += 1;
                                break;
                            case 1:
                                _CountInProcess += 1;
                                break;
                        }
                        i = i + 1;
                        if (i > _dt1.Rows.Count - 1)
                        {
                            break;
                        }

                    } while (Convert.ToInt32(_ActId) == Convert.ToInt32(_dt1.Rows[i][0].ToString()));
                    DataRow _row1 = _dt2.NewRow();
                    _sNo += 1;
                    _row1["SNO"] = _sNo;
                    _row1["Activity No"] = _ActId;
                    _row1["Total Calls"] = _count;
                    _row1["Won"] = _CountWon;
                    _row1["In Process"] = _CountInProcess;
                    _row1["Pending"] = _CountPending;
                    _row1["Non Usable"] = _CountNonUsable;
                    _dt2.Rows.Add(_row1);
                    i = i - 1;
                }
            }
            if (_dt2.Rows.Count > 0)
            {
                grdDetail.DataSource = _dt2;
                grdDetail.DataBind();
                //btnModify.Visible = true;
                btnShowDetail.Visible = true;
                btnReassign.Visible = true;
                //btnReschedule.Visible = true;
                btnCreate.Visible = false;
                ActivitySalesGrid.Visible = false;
                PanelActivitySales.Visible = false;
                //btnDelegate.Visible = true;
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ViewState["Status"] = "CreateNew";
            pnlCall.Visible = true;
            pnlActivityDetail.Visible = false;
            drpUserWork.Enabled = true;
            ActivityRow.Visible = false;
            EnableControls();
        }
        protected void EnableControls()
        {
            pnlShowDetail.Visible = false;
            //userInfo1.Enabled = false;
            btnModify.Visible = false;
            btnCreate.Visible = false;
        }
        protected void fillActivityData()
        {
            DataTable _dt1 = new DataTable();
            DataTable _dt2 = new DataTable();
            for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            {
                GridViewRow _row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)_row.FindControl("chkDetail");
                Label lblActivity = (Label)_row.FindControl("lblActivity");
                if (chkDetail.Checked == true)
                {
                    _dt1 = objEngine.GetDataTable(" tbl_trans_Sales,tbl_trans_Activies", " tbl_trans_Activies.act_activityType as Type,tbl_trans_Activies.act_assignedTo AS userId,convert(varchar(25),Isnull(tbl_trans_Activies.act_scheduledDate,''),103) AS StartDate, convert (varchar(25),isnull(tbl_trans_Activies.act_expectedDate,''),103) AS EndDate, Isnull(tbl_trans_Activies.act_instruction,'') AS Instruction, tbl_trans_Activies.act_priority AS Priority,isnull(tbl_trans_Activies.act_description,'') AS Description,tbl_trans_Sales.sls_id AS PhoneCallid,isNull(tbl_trans_Activies.act_scheduledTime,'') as StartTime,isNull(tbl_trans_Activies.act_expectedTime,'') as EndTime", " tbl_trans_Activies.act_id ='" + lblActivity.Text + "' And tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id");
                    if (_dt1.Rows.Count > 0)
                    {
                        _dt2 = _dt1.Copy();
                        drpActType.SelectedValue = _dt1.Rows[0]["Type"].ToString();
                        drpUserWork.SelectedValue = _dt1.Rows[0]["userId"].ToString();
                        txtStartDate.Text = Convert.ToString(_dt1.Rows[0]["StartDate"] + " " + _dt1.Rows[0]["StartTime"]);
                        txtEndDate.Text = Convert.ToString(_dt1.Rows[0]["EndDate"] + " " + _dt1.Rows[0]["EndTime"]);
                        txtDesc.Text = _dt1.Rows[0]["Description"].ToString();
                        txtInstNote.Text = _dt1.Rows[0]["Instruction"].ToString();
                        if (_dt1.Rows[0]["Priority"].ToString() != "")
                            drpPriority.SelectedValue = _dt1.Rows[0]["Priority"].ToString();
                    }
                    ViewState["edit"] = lblActivity.Text;
                    break;
                }
            }
            if (_dt2.Rows.Count > 0)
            {
                if (_dt1.Rows.Count > 0)
                {
                    string _temp = "";
                    for (int i = 0; i <= _dt1.Rows.Count - 1; i++)
                    {
                        _temp += _dt1.Rows[0]["PhoneCallid"].ToString() + ",";
                    }
                    string sTemp = "";
                    for (int i = 0; i <= _temp.Length - 2; i++)
                    {
                        sTemp += _temp[i];
                    }
                    ViewState["ids"] = sTemp;
                }
            }
        }
        protected void btnReassign_Click(object sender, EventArgs e)
        {

            ViewState["Status"] = "Reassign";
            Session["ActNo"] = null;
            for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            {
                GridViewRow _row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)_row.FindControl("ChkDetail");
                Label lblActivity = (Label)_row.FindControl("lblActivity");
                if (chkDetail.Checked == true)
                {
                    Session["ActNo"] = lblActivity.Text;
                    DataTable sLeadId = objEngine.GetDataTable("tbl_trans_Sales", "sls_contactlead_id", " sls_activity_id='" + lblActivity.Text + "' and sls_sales_status!=2");
                    Session["ContactLeadID"] = sLeadId.Rows[0][0].ToString();
                    if (sLeadId.Rows.Count > 1)
                    {
                        for (int ld = 1; ld < sLeadId.Rows.Count; ld++)
                        {
                            Session["ContactLeadID"] = Session["ContactLeadID"] + "," + sLeadId.Rows[ld][0].ToString();
                        }
                    }
                    ProductId = objEngine.GetFieldValue("tbl_trans_sales inner join tbl_trans_offeredProduct on tbl_trans_sales.sls_contactlead_id = tbl_trans_offeredProduct.ofp_leadId", "tbl_trans_offeredProduct.ofp_id", "tbl_trans_sales.sls_contactlead_id in('" + sLeadId.Rows[0][0].ToString() + "')", 1)[0, 0];

                }
            }
            if (Session["ActNo"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Select First Any Activity')</script>");
                return;
            }
            //fillActivityData();
            //pnlCall.Visible = true;
            //pnlActivityDetail.Visible = false;
            //EnableControls();
            leadid = Session["ContactLeadID"].ToString() + "@@@@" + ProductId + ",";
            ClientScript.RegisterStartupScript(GetType(), "Jscript1", "<script language='JavaScript'>windowopenform1('" + leadid + "');</script>");
        }
        protected void btnReschedule_Click(object sender, EventArgs e)
        {
            ViewState["Status"] = "Reschedule";
            Session["ActNo"] = null;
            for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            {
                GridViewRow _row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)_row.FindControl("ChkDetail");
                Label lblActivity = (Label)_row.FindControl("lblActivity");
                if (chkDetail.Checked == true)
                {
                    Session["ActNo"] = lblActivity.Text;
                    DataTable sLeadId = objEngine.GetDataTable("tbl_trans_Sales", "sls_contactlead_id", " sls_activity_id='" + lblActivity.Text + "'");
                    Session["ContactLeadID"] = sLeadId.Rows[0][0].ToString();
                }
            }
            if (Session["ActNo"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Select First Any Activity')</script>");
                return;
            }
            fillActivityData();
            pnlCall.Visible = true;
            pnlActivityDetail.Visible = false;
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
            EnableControls();
        }
        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            ViewState["check"] = null;
            DataTable _dt = new DataTable();
            pnlActivityDetail.Visible = true;
            pnlCall.Visible = false;
            int _count = 0;
            for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            {
                GridViewRow _row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)_row.FindControl("ChkDetail");
                Label lblActivity = (Label)_row.FindControl("lblActivity");
                if (chkDetail.Checked == true)
                {
                    ViewState["check"] = "1";
                    Session["ActNo"] = lblActivity.Text;
                    DataTable sLeadId = objEngine.GetDataTable("tbl_trans_Sales", "sls_contactlead_id", " sls_activity_id='" + lblActivity.Text + "'");
                    Session["ContactLeadID"] = sLeadId.Rows[0][0].ToString();
                    _dt = objEngine.GetDataTable(" tbl_trans_Sales , tbl_trans_Activies ,tbl_master_contact , tbl_master_address ,tbl_master_phonefax", " DISTINCT (tbl_master_contact.cnt_id),ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '')  + ISNULL(tbl_master_contact.cnt_lastName, '') AS name,tbl_master_phonefax.phf_phoneNumber AS PhNo,ISNULL(tbl_master_address.add_address1, '')+ ISNULL(tbl_master_address.add_address2, '') + ISNULL(tbl_master_address.add_address3, '') AS Address,CASE tbl_trans_Activies.act_priority WHEN 0 THEN 'Low' WHEN 1 THEN 'Normal' WHEN 2 THEN 'High' WHEN 3 THEN 'Urgent' WHEN 4 THEN 'Complete' ELSE 'Not Known' END AS Priority", " tbl_trans_Sales.sls_activity_id ='" + lblActivity.Text + "' And tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id And tbl_trans_Sales.sls_contactlead_id = tbl_master_contact.cnt_internalId And tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId And tbl_master_contact.cnt_internalId = tbl_master_phonefax.phf_cntId and ISNULL(tbl_master_address.add_address2, '') + ISNULL(tbl_master_address.add_address3, '') !='' Order by Name");
                    //DataTable _dt1 = _dt.Clone();
                    //DataRow _row1 = _dt1.NewRow();
                    //for (int j = 0; j <= _dt.Rows.Count - 1; j++)
                    //{
                    //    string _name = _dt.Rows[j]["name"].ToString();
                    //    string _name1 = _dt.Rows[j]["name"].ToString();
                    //    do
                    //    {
                    //        for (int k = 0; k <= _dt.Columns.Count - 1; k++)
                    //        {
                    //            _row1[k] = _dt.Rows[j][k].ToString();
                    //        }
                    //        j = j + 1;
                    //        if (j > _dt.Rows.Count - 1)
                    //            break;

                    //    } while (_name == _name1);
                    //    _dt1.Rows.Add(_row1);
                    //    _count += 1;
                    //    j -= 1;
                    //    if (j == _dt.Rows.Count - 1)
                    //        break;
                    //}
                    if (_dt.Rows.Count > 0)
                    {
                        pnlActivityDetail.Visible = true;
                        grdActivityDetail.DataSource = _dt;
                        grdActivityDetail.DataBind();
                    }
                    else
                    {
                        pnlActivityDetail.Visible = false;
                    }
                }
            }
            if (ViewState["check"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Select First Any Activity')</script>");
            }
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            ViewState["Status"] = "Modify";
            Session["ActNo"] = null;
            for (int i = 0; i <= grdDetail.Rows.Count - 1; i++)
            {
                GridViewRow _row = grdDetail.Rows[i];
                CheckBox chkDetail = (CheckBox)_row.FindControl("ChkDetail");
                Label lblActivity = (Label)_row.FindControl("lblActivity");
                if (chkDetail.Checked == true)
                {
                    Session["ActNo"] = lblActivity.Text;
                    DataTable sLeadId = objEngine.GetDataTable("tbl_trans_Sales", "sls_contactlead_id", " sls_activity_id='" + lblActivity.Text + "'");
                    Session["ContactLeadID"] = sLeadId.Rows[0][0].ToString();
                }
            }
            if (Session["ActNo"] == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Select First Any Activity')</script>");
                return;
            }
            fillActivityData();
            pnlCall.Visible = true;
            pnlActivityDetail.Visible = false;
            drpActType.Enabled = false;
            drpUserWork.Enabled = false;
            EnableControls();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string sStartDate = txtStartDate.Value.ToString();
            string sEndDate = txtEndDate.Value.ToString();
            string[] AssignTo;
            string[] StartDateArr = sStartDate.Split(' ');
            string startDate = StartDateArr[0].ToString();
            string startTime = StartDateArr[1].ToString();

            string[] EndDateArr = sEndDate.Split(' ');
            string endDate = EndDateArr[0].ToString();
            string endTime = EndDateArr[1].ToString();
            int StartVal = 0;
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            string _id = "";
            switch (ViewState["Status"].ToString())
            {
                case "CreateNew":
                    if (Session["lead"] != null && Session["Product"] != null)
                    {
                        DateTime date1 = oDBEngine.GetDate();
                        string date = Convert.ToString(date1.ToShortDateString());
                        string[] _temp;
                        string Lead = Session["lead"].ToString();
                        _temp = Lead.Split(',');
                        string _sendStr = "";
                        int ij = 0;
                        bool _bVal = false;
                        AssignTo = hd1UserList.Value.Split(',');
                        int count12 = 0;
                        if (Session["Count"] != null)
                        {
                            count12 = Convert.ToInt32(Session["Count"].ToString());
                        }
                        else
                        {
                            count12 = 0;
                        }
                        for (int arow = 0; arow < AssignTo.Length; arow++)
                        {


                            _actNo = objEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                            objEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedValue.ToString() + "','" + txtDesc.Text + "','" + UserId.ToString() + "','" + AssignTo[arow].ToString() + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + UserId.ToString() + "'");
                            string[,] Data = objEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            if (arow != 0)
                            {
                                StartVal = count12;
                                count12 = StartVal + count12;
                            }
                            else
                            {
                                StartVal = 0;
                            }
                            for (int ijk = StartVal; ijk < count12; ijk++)
                            {
                                chrk1 = _temp[ijk].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {
                                    if (Session["Product"] == null)
                                    {
                                        objEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser", "'" + _id.ToString() + "','" + chrk1[0].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','" + chrk1[2].ToString() + "','" + chrk1[3].ToString() + "','0','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + CreateDate.ToString() + "','" + UserId.ToString() + "'");
                                        objEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + chrk1[0].ToString() + "'");
                                        //objEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_actId='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " ofp_id='" + chrk1[5].ToString() + "'");
                                    }
                                    else
                                    {
                                        product = Session["Product"].ToString();
                                        _tempProd = product.Split(',');
                                        for (int k = 0; k < _tempProd.Length - 1; k++)
                                        {
                                            _tempProdDetail = _tempProd[k].Split(':');
                                            string _Prod = "";
                                            _Prod = _tempProdDetail.GetValue(2).ToString();
                                            objEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser", "'" + _id.ToString() + "','" + chrk1[0].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','" + _tempProdDetail.GetValue(0).ToString() + "','" + _Prod.ToString() + "','" + _tempProdDetail.GetValue(1).ToString() + "','" + oDBEngine.GetDate().AddDays(1).ToString() + "','" + CreateDate.ToString() + "','" + UserId.ToString() + "'");
                                            objEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + chrk1[0].ToString() + "'");
                                            //ADD PRODUCT INTO OFFERED PRODUCT TABLE
                                            //string[] ld = Session["LeadId"].ToString().Split(',');
                                            //for (int h = 0; h <= ld.Length - 1; h++)
                                            //{
                                            string[] ld = _temp[ijk].ToString().Split('|');
                                            string[,] act_id = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", "act_activityno='" + _actNo.ToString() + "'", 1);
                                            String columns = "ofp_actId,ofp_leadId,ofp_productTypeId,ofp_probableAmount,ofp_productId,ofp_activityid";
                                            DataValue += act_id[0, 0].ToString() + ",";
                                            DataValue += "'" + ld[0].ToString() + "'" + ",";
                                            DataValue += "'" + _tempProdDetail.GetValue(0) + "'" + ",";
                                            DataValue += "'" + _tempProdDetail.GetValue(1) + "'" + ",'" + _tempProdDetail.GetValue(2) + "','" + _actNo.ToString() + "'";

                                            oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", columns, DataValue);
                                            DataValue = "";
                                        }

                                    }

                                }



                                catch
                                {
                                }
                            }
                        }

                        Session["Product"] = null;
                        Session["Lead"] = null;


                    }


                    break;
                case "Modify":
                    string _temp3 = "";
                    _temp3 = ViewState["ids"].ToString();
                    string[] _tmp = _temp3.Split(',');
                    objEngine.SetFieldValue("tbl_trans_Activies", "act_branchId='" + HttpContext.Current.Session["userbranchID"].ToString() + "',act_activityType ='" + drpActType.SelectedValue.ToString() + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + UserId.ToString() + "',act_assignedTo='" + drpUserWork.SelectedValue.ToString() + "',act_contactlead='" + _tmp.Length.ToString() + "',act_scheduledDate='" + startDate.ToString() + "',act_expectedDate='" + endDate.ToString() + "',act_priority='" + drpPriority.SelectedValue.ToString() + "',act_scheduledTime='" + startTime.ToString() + "',act_expectedTime='" + endTime.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " act_id ='" + ViewState["edit"] + "'");
                    break;
                //case "Reassign":
                //    string _temp1 = "";
                //    _temp1 = ViewState["ids"].ToString();
                //    string[] _tmp1 = _temp1.Split(',');
                //    objEngine.SetFieldValue("tbl_trans_Activies", "act_branchId='" + HttpContext.Current.Session["userbranchID"].ToString() + "',act_activityType ='" + drpActType.SelectedValue.ToString() + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + UserId.ToString() + "',act_assignedTo='" + drpUserWork.SelectedValue.ToString() + "',act_contactlead='" + _tmp1.Length.ToString() + "',act_scheduledDate='" + startDate.ToString() + "',act_expectedDate='" + endDate.ToString() + "',act_priority='" + drpPriority.SelectedValue.ToString() + "',act_scheduledTime='" + startTime.ToString() + "',act_expectedTime='" + endTime.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " act_id ='" + ViewState["edit"] + "'");
                //    break;
                //case "Reschedule":
                //    string _temp2 = "";
                //    _temp2 = ViewState["ids"].ToString();
                //    string[] _tmp2 = _temp2.Split(',');
                //    objEngine.SetFieldValue("tbl_trans_Activies", "act_branchId='" + HttpContext.Current.Session["userbranchID"].ToString() + "',act_activityType ='" + drpActType.SelectedValue.ToString() + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy='" + UserId.ToString() + "',act_assignedTo='" + drpUserWork.SelectedValue.ToString() + "',act_contactlead='" + _tmp2.Length.ToString() + "',act_scheduledDate='" + startDate.ToString() + "',act_expectedDate='" + endDate.ToString() + "',act_priority='" + drpPriority.SelectedValue.ToString() + "',act_scheduledTime='" + startTime.ToString() + "',act_expectedTime='" + endTime.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " act_id ='" + ViewState["edit"] + "'");
                //    break;
            }
            Response.Redirect("frm_Sales_Activity.aspx");

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlCall.Visible = false;
            pnlShowDetail.Visible = true;
            pnlActivityDetail.Visible = false;
            if (ViewState["Status"].ToString() == "CreateNew")
            {
                btnCreate.Visible = true;
                btnModify.Visible = false;
            }
            else
            {
                //btnModify.Visible = true;
                btnCreate.Visible = false;
            }
            ActivityRow.Visible = true;
        }
        protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkDetail");
                Label lbl = (Label)e.Row.FindControl("lblActivity");
                chkBox.Attributes.Add("onclick", "javascript:chkGenral(this,'" + lbl.Text + "')");
            }
        }


         [WebMethod]
        public static List<string> GetAllUserListAutocomplete(string KeyWord)
        {
            BusinessLogicLayer.DBEngine objBL = new BusinessLogicLayer.DBEngine();
            DataTable DT = objBL.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + KeyWord + "%' or cnt_shortName like '" + KeyWord + "%')");
            List<string> obj = new List<string>();

            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }

        

            return obj;
        }
    }
}