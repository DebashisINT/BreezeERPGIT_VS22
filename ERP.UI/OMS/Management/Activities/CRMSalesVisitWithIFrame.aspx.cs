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
using System.Collections.Specialized;
using DataAccessLayer;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.Activities
{
    public partial class management_Activities_CRMSalesVisitWithIFrame : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        clsDropDownList clsdropdown = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string BranchORClient = "";
        public string pageAccess = "";
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        Employee_BL objemployeebal = new Employee_BL();
        public EntityLayer.CommonELS.UserRightsForPage rightsQuotation = new UserRightsForPage();
        public EntityLayer.CommonELS.UserRightsForPage rightsSalesOrder = new UserRightsForPage();
        public EntityLayer.CommonELS.UserRightsForPage rightsSalesVisitStatus = new UserRightsForPage();

        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        Converter objConverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              

                if (HttpContext.Current.Session["userid"] == null)
                {
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
                rightsQuotation = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesQuotationList.aspx");
                rightsSalesOrder = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesOrderEntityList.aspx");
                rightsSalesVisitStatus = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/crm_sales.aspx");
                idrightsquotation.Value = Convert.ToString(rightsQuotation.CanAdd);
                idrightsSaleorder.Value = Convert.ToString(rightsSalesOrder.CanAdd);
                pnlData.Enabled = false;
              
                if (!IsPostBack)
                {
                    if (Request.QueryString["Pid"] == "3")
                    { ASPxNextVisit.MinDate = DateTime.Now; }
                    else { ASPxNextVisit.MinDate = DateTime.Now.AddDays(-1); }
                    
                    hdndisableChoosen.Value = "Yes";
                    //btntbl.Visible = false;
                    string ReceiverEmail = string.Empty;
                    int transSaleId = 0;
                    btnSavePhoneCallDetails.Attributes.Add("onclick", "return chkOnSaveClick123();");
                    btnSavePhoneCallDetails.Enabled = true;
                  //  string today = objConverter.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
                    ASPxDateEdit.EditFormatString = objConverter.GetDateFormat("DateTime");
                   // ASPxNextVisit.EditFormatString = objConverter.GetDateFormat("DateTime");
                    ASPxDateEdit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                 //   ASPxNextVisit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                    BindActivityName();
                    string EditPermitValue = string.Empty;
                    int cntId = 0;

                    string cnt_id = Convert.ToString(Session["cntId"]);
                    if (Request.QueryString["TransSale"] != null)
                    {
                        transSaleId = Convert.ToInt32(Request.QueryString["TransSale"]);
                        ShowSaleDetail(transSaleId);

                        EditPermitValue = EditPermissionShow(transSaleId, cnt_id);
                    }
                    //if (EditPermitValue == "1")
                    //{
                    //    ASPxNextVisit.Enabled = true;
                    //}
                    //else
                    //{
                    //    ASPxNextVisit.Enabled = false;
                    //}
                    //showdetailstbl.Visible = false;
                    //pnldatatbl.Visible = false;
                    //ShowDetails();
                    BindVisitPlaceAndNextVistPlace();
                    BindVisitPlace();
                    rdrCall.Attributes.Add("onclick", "funChangeNext(this)");
                    rdrVisit.Attributes.Add("onclick", "funChangeNext(this)");
                    //BtnSelect();
                    BindActivityDetails();


                    SalesVisitBind();


                    #region Radio Button Event
                    //Add javascript on Lead Radio button Change event by sam on 02012017
                    Lrd.Attributes.Add("onclick", "All_CheckedChanged();");
                    //Add javascript on Customer Radio button Change event by sam on 02012017
                    Erd.Attributes.Add("onclick", "Specific_CheckedChanged();");
                    #endregion Radio Button Event

                    rdClient.Attributes.Add("onclick", "BranchOrClient('C');");
                    rdBranch.Attributes.Add("onclick", "BranchOrClient('B');");
                    //txtbranch.Attributes.Add("onkeyup", "SearchByBranchName(this,'BranchName',event)");
                    //txtbranch.Attributes.Add("onkeyup", "SearchByBranchName(this,'BranchName',event)");
                    //txtNBranch.Attributes.Add("onkeyup", "SearchByBranchName(this,'BranchName',event)");
                    rdNClient.Attributes.Add("onclick", "BranchOrClientN('C');");
                    rdNBranch.Attributes.Add("onclick", "BranchOrClientN('B');");
                    Page.ClientScript.RegisterStartupScript(GetType(), "DefaultSettings", "<script language='JavaScript'>BranchOrClientN('C');</script>");
                    //kaushik 18_1_2017
                    BindPhoneFollowUP();
                    SetCountry();
                    bindClassandCustomer();
                    if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
                    {
                        lblcustomer.Text = Convert.ToString(Request.QueryString["Name"]);

                    }
                     if (!string.IsNullOrEmpty(Request.QueryString["Cid"]))
                    {
                        DataTable dtEmail_To = new DataTable();
                        string AssignTo = Convert.ToString(Request.QueryString["Cid"]);
                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);

                        if (dtEmail_To.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                            {
                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                               
                            }
                            else
                            {
                                ReceiverEmail = "";
                                idmail.Visible = false;
                            }
                        }
                        else
                        {

                            idmail.Visible = false;
                        }

                        Type officeType = Type.GetTypeFromProgID("Outlook.Application");
                        if (officeType == null)
                        {
                            idmail.Visible = false;
                        }
                        
                        mailassignfield.Value = ReceiverEmail;
                        

                    }


                   
                }
                //string a = Request.QueryString["id1"].ToString();
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "DefaultSettings", "<script language='JavaScript'>BranchOrClientN('C');</script>");
                    BindVisitPlace();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

            //BindActivityDetails();
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>iframesource();</script>");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }



        public string EditPermissionShow(string cntid)
        {


            string EditPermissionval = "0";
            try
            {
                BusinessLogicLayer.Others objOtherBL = new BusinessLogicLayer.Others();

                DataTable PermissionDt = new DataTable();
                PermissionDt = objOtherBL.GetSalesManDeactivateDocomentActivity(cntid);
                if (PermissionDt != null && PermissionDt.Rows.Count > 0)
                {
                    EditPermissionval = Convert.ToString(PermissionDt.Rows[0]["EditPermission"]);

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                //return;
            }


            return EditPermissionval;
        }
        public string EditPermissionShow(int transSaleId, string cntid)
        {


            string EditPermissionval = "0";
            try
            {
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();

                DataTable PermissionDt = new DataTable();
                PermissionDt = objSlaesActivitiesBL.GetEditablePermissionOfSupervisorInActivity(transSaleId, cntid);
                if (PermissionDt != null && PermissionDt.Rows.Count > 0)
                {
                    EditPermissionval = Convert.ToString(PermissionDt.Rows[0]["EditPermission"]);

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                //return;
            }


            return EditPermissionval;
        }




        #region Sales Visit Status
        public void SalesVisitBind()
        {
            string EditPermitValue = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(Session["cntId"])))
            {

                string cntId = Convert.ToString(Session["cntId"]);
                
                if (!string.IsNullOrEmpty(Request.QueryString["Assigned"]))
                {
                    string Assigned = Convert.ToString(Request.QueryString["Assigned"]);



                    //ddl_activitystatus.Items.Add(new ListItem("Open", "0", true));
                    

                    string cnt_id = Convert.ToString(Session["cntId"]);

                    EditPermitValue = EditPermissionShow(cnt_id);


                    if (rightsSalesVisitStatus.DocumentCollection) ddl_activitystatus.Items.Add(new ListItem("Document Collection", "1", true));
                   
                    if (rightsSalesVisitStatus.ClosedSales && cntId == Assigned)
                    {
                        ddl_activitystatus.Items.Add(new ListItem("Closed Sales", "2"));
                    }
                   
                    if (rightsSalesVisitStatus.FutureSales) ddl_activitystatus.Items.Add(new ListItem("Future Sales", "3"));


                    if (rightsSalesVisitStatus.ClarificationRequired) ddl_activitystatus.Items.Add(new ListItem("Clarification Required", "5"));

                    /*if (EditPermitValue == "0")
                    {


                        ddl_activitystatus.Items.Add(new ListItem("Document Collection", "1", true));
                    }
                    if (cntId == Assigned)
                    {
                        ddl_activitystatus.Items.Add(new ListItem("Closed Sales", "2"));
                    }
                    ddl_activitystatus.Items.Add(new ListItem("Future Sales", "3"));
                    ddl_activitystatus.Items.Add(new ListItem("Clarification Required", "5"));*/


                }

            }
            if (EditPermitValue == "0")
            {
                ddl_activitystatus.Items.FindByValue("1").Selected = true;
            }
        }
        #endregion


        #region PhoneFollowUP hide if phone call not allowed
        public void BindPhoneFollowUP()
        {
            DataTable dt = new DataTable();
           string sid = Convert.ToString(Request.QueryString["TransSale"]);
           dt = objSlaesActivitiesBL.GetPhoneStatus(sid);
            if(dt==null|| dt.Rows.Count==0)
            {
                rdrCall.Enabled = false;
            }
            else { rdrCall.Enabled = true; }
        }
        #endregion
        #region Old and New Code of ShowDetails
        public void ShowSaleDetail(int transSaleId)
        {
            try
            {


                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                string date = oDBEngine.GetDate().ToString();
                //ViewState["callstart"] = date.ToString();
                //TxtNext.Visible = false;
                string salevisitid = "";
                string activityId = "";
                string leadcotactId = "";
                int cntid = 0;
                //Session["contactInternalId"]
                DataTable salevisitDt = new DataTable();
                salevisitDt = objSlaesActivitiesBL.GetSaleVisitDtlToShowDetail(transSaleId);
                if (salevisitDt != null && salevisitDt.Rows.Count > 0)
                {
                    salevisitid = Convert.ToString(salevisitDt.Rows[0]["slv_id"]);
                    activityId = Convert.ToString(salevisitDt.Rows[0]["slv_activityId"]);
                    leadcotactId = Convert.ToString(salevisitDt.Rows[0]["slv_leadcotactId"]);
                    cntid = Convert.ToInt32(salevisitDt.Rows[0]["cnt_id"]);
                    Session["phonecallid"] = null;
                    Session["SalesVisitID"] = salevisitid;
                    Session["SalesActivityId"] = activityId;
                    Session["InternalId"] = leadcotactId;
                    Session["KeyVal_InternalID"] = "leadcotactId";
                    Session["ContactType"] = "Lead";
                    Session["requesttype"] = "Lead";
                    Session["SalescntId"] = cntid;
                    string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_salesExpenditure", "expnd_empId", " expnd_empId='" + Convert.ToString(Session["SalesVisitID"]) + "'", 1);
                    if (Id1[0, 0] != "n")
                    {

                    }
                    else
                    {
                        // .............................Code Commented and Added by Sam on 03012017.due to error in query ..................................... 
                        oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
                        //oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
                        // .............................Code Above Commented and Added by Sam on 03012017...................................... 
                        oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
                    }
                    if (Session["ACtivityID"] != null)
                    {
                        DDLActivity.SelectedValue = Convert.ToString(Session["ACtivityID"]);
                        string Id = Convert.ToString(Request.QueryString["id"]);
                        PnlShowDetails.Visible = true;
                        pnlData.Visible = true;
                        activityRow.Visible = false;
                        showdetailstbl.Visible = true;
                        pnldatatbl.Visible = true;
                        NewBindPanelDetails();
                        //BindPanelDetails();
                    }
                    else
                    {
                        NewBindPanelDetails();
                        //BindPanelDetails();
                    }
                    //btntbl.Visible = false;
                    grdtbl.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }
        public void ShowDetails()
        {
            //if (Request.QueryString["id"] != null)
            //{
            //    Session["phonecallid"] = null;
            //    Session["SalesVisitID"] = Convert.ToString(Request.QueryString["id2"]);
            //    Session["SalesActivityId"] = Convert.ToString(Request.QueryString["id"]);
            //    Session["InternalId"] = Convert.ToString(Request.QueryString["id1"]);
            //    string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_salesExpenditure", "expnd_empId", " expnd_empId='" + Convert.ToString(Session["SalesVisitID"]) + "'", 1);
            //    if (Id1[0, 0] != "n")
            //    {

            //    }
            //    else
            //    {
            //        // .............................Code Commented and Added by Sam on 03012017.due to error in query ..................................... 
            //        oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
            //        //oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
            //        // .............................Code Above Commented and Added by Sam on 03012017...................................... 
            //        oDBEngine.InsurtFieldValue("tbl_trans_salesExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Convert.ToString(Session["SalesVisitID"]) + "','1','1','1','1'");
            //    }
            //    if (Session["ACtivityID"] != null)
            //    {
            //        DDLActivity.SelectedValue = Convert.ToString(Session["ACtivityID"]);
            //        string Id = Convert.ToString(Request.QueryString["id"]);
            //        PnlShowDetails.Visible = true;
            //        pnlData.Visible = true;
            //        activityRow.Visible = false;
            //        showdetailstbl.Visible = true;
            //        pnldatatbl.Visible = true;
            //        NewBindPanelDetails();
            //        BindPanelDetails();
            //    }
            //    else
            //    {
            //        NewBindPanelDetails();
            //        BindPanelDetails();
            //    }
            //    btntbl.Visible = false;
            //    grdtbl.Visible = false;
            //}
            //else
            //{
            //    if (Session["SalesActivityId"] != null)
            //    {
            //        PnlShowDetails.Visible = true;
            //        pnlData.Visible = true;
            //        NewBindPanelDetails();
            //        BindPanelDetails();
            //    }
            //}
        }

        #endregion


        #region Old and New Code of BindPanelDetails
        public void NewBindPanelDetails()
        {
            try
            {
                //string Id = Convert.ToString(Request.QueryString["id"]);
                string Id = Convert.ToString(Session["SalesActivityId"]);
                string transSaleId;
                transSaleId = Convert.ToString(Request.QueryString["TransSale"]);
                DataTable dt = new DataTable();
                dt = objBL.GetSalesVisitInfo(Id, transSaleId);
                if(dt!=null && dt.Rows.Count>0)
                {

                    lblShortNote.Text = Convert.ToString(dt.Rows[0]["act_instruction"]);
                    if (lblShortNote.Text == "")
                    {

                        DataTable dtNote = new DataTable();

                       
                        dtNote = objBL.GetNoteSaleInfo(transSaleId);
                        if (dtNote != null && dtNote.Rows.Count > 0)
                        {
                            lblShortNote.Text = Convert.ToString(dtNote.Rows[0]["act_instruction"]);
                        }
                        else { idNoteRemarks.Visible = false; }
                       
                       

                    }
                   
                    string Priorty = Convert.ToString(dt.Rows[0]["Priority"]);
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
                    txtDateOfAllottment.Text = Convert.ToString(dt.Rows[0]["CreateDate"]);
                    txtSeheduleStart.Text = Convert.ToString(dt.Rows[0]["ScheduleDate"]);
                    txtSeheduleEnd.Text = Convert.ToString(dt.Rows[0]["ExpDate"]);
                    txtAcutalStart.Text = Convert.ToString(dt.Rows[0]["StartDate"]);
                    txtAlloatedBy.Text = Convert.ToString(dt.Rows[0]["AssignedBy"]);

                }
                // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                //string[,] Activity = oDBEngine.GetFieldValue("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_trans_Activies.act_activityNo AS ActNo, tbl_trans_Activies.act_instruction, tbl_trans_Activies.act_assignedBy AS AssignBy, tbl_trans_Activies.act_priority AS Priority,  (convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) AS CreateDate, (convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3))) AS ScheduleDate,  (convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3))) AS ExpDate, (convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) AS StartDate, tbl_trans_Activies.act_id,   tbl_master_user.user_name as AssignedBy", "tbl_trans_Activies.act_id=" + Id, 10);
             //   string[,] Activity = oDBEngine.GetFieldValue("tbl_trans_Activies INNER JOIN   tbl_master_contact ON tbl_trans_Activies.act_assignedBy = tbl_master_contact.cnt_id left  join tbl_master_user on tbl_master_user.user_contactId=tbl_master_contact.cnt_internalId ", "tbl_trans_Activies.act_activityNo AS ActNo, tbl_trans_Activies.act_instruction, tbl_trans_Activies.act_assignedBy AS AssignBy, tbl_trans_Activies.act_priority AS Priority,  (convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) AS CreateDate, (convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3))) AS ScheduleDate,  (convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3))) AS ExpDate, (convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) AS StartDate, tbl_trans_Activies.act_id,   tbl_master_user.user_name as AssignedBy", "tbl_trans_Activies.act_id=" + Id, 10);
                // .............................Code Above Commented and Added by Sam on 03012017...................................... 
                //if (Activity[0, 0] != "n")
                //{
                //    lblShortNote.Text = Activity[0, 1];
                //    if( lblShortNote.Text=="")
                //    {
                //        idNoteRemarks.Visible = false;

                //    }
                //    string Priorty = Activity[0, 3];
                //    switch (Priorty)
                //    {
                //        case "0":
                //            txtPriority.Text = "Low";
                //            break;
                //        case "1":
                //            txtPriority.Text = "Normal";
                //            break;
                //        case "2":
                //            txtPriority.Text = "High";
                //            break;
                //        case "3":
                //            txtPriority.Text = "Urgent";
                //            break;
                //        case "4":
                //            txtPriority.Text = "Immediate";
                //            break;
                //    }
                //    txtDateOfAllottment.Text = Activity[0, 4];
                //    txtSeheduleStart.Text = Activity[0, 5];
                //    txtSeheduleEnd.Text = Activity[0, 6];
                //    txtAcutalStart.Text = Activity[0, 7];
                //    txtAlloatedBy.Text = Activity[0, 9];
                //}
                string LId = Convert.ToString(Session["InternalId"]);
                //string LId = Convert.ToString(Request.QueryString["id1"]);

                string[,] LActivity = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN   tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "tbl_trans_salesVisit.slv_leadcotactId, (convert(varchar(11),tbl_trans_salesVisit.slv_lastdatevisit,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 3))) as LastVisitDate, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate,  tbl_trans_salesVisit.slv_nextvisitplace as NextVisitPlace , tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as OutCome", "tbl_trans_salesVisit.slv_leadcotactId = '" + LId + "' AND tbl_trans_salesVisit.slv_activityId = '" + Id + "'", 5);
                if (LActivity[0, 0] != "n")
                {
                    lblLastVisit.Text = LActivity[0, 1];
                    if (lblLastVisit.Text == "" || lblLastVisit.Text == "N/A")
                    {
                       // lblLastVisit.Text = "N/A";
                        lilastvisitstatus.Visible = false;
                    }
                    lblLastOutcome.Text = LActivity[0, 4];
                    if (lblLastOutcome.Text == "" || lblLastOutcome.Text == "N/A")
                    {
                        lilLastOutcome.Visible = false;
                       // lblLastOutcome.Text = "N/A";
                    }
                    else { lilLastOutcome.Visible = true; }
                    lblNextVisit.Text = LActivity[0, 2];
                    if (lblNextVisit.Text == "")
                    {
                       // lblNextVisit.Text = "N/A";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }
        public void BindPanelDetails()
        {
            //if (Request.QueryString["id"] != null)
            //{
            //    string Id = Convert.ToString(Request.QueryString["id"]);
            //    // .............................Code Commented and Added by Sam on 03012017. ..................................... 
            //    //string[,] Activity = oDBEngine.GetFieldValue("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_trans_Activies.act_activityNo AS ActNo, tbl_trans_Activies.act_instruction, tbl_trans_Activies.act_assignedBy AS AssignBy, tbl_trans_Activies.act_priority AS Priority,  (convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) AS CreateDate, (convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3))) AS ScheduleDate,  (convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3))) AS ExpDate, (convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) AS StartDate, tbl_trans_Activies.act_id,   tbl_master_user.user_name as AssignedBy", "tbl_trans_Activies.act_id=" + Id, 10);
            //    string[,] Activity = oDBEngine.GetFieldValue("tbl_trans_Activies INNER JOIN   tbl_master_contact ON tbl_trans_Activies.act_assignedBy = tbl_master_contact.cnt_id join tbl_master_user on tbl_master_user.user_contactId=tbl_master_contact.cnt_internalId ", "tbl_trans_Activies.act_activityNo AS ActNo, tbl_trans_Activies.act_instruction, tbl_trans_Activies.act_assignedBy AS AssignBy, tbl_trans_Activies.act_priority AS Priority,  (convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) AS CreateDate, (convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3))) AS ScheduleDate,  (convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3))) AS ExpDate, (convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) AS StartDate, tbl_trans_Activies.act_id,   tbl_master_user.user_name as AssignedBy", "tbl_trans_Activies.act_id=" + Id, 10);
            //    // .............................Code Above Commented and Added by Sam on 03012017...................................... 
            //    if (Activity[0, 0] != "n")
            //    {
            //        lblShortNote.Text = Activity[0, 1];
            //        string Priorty = Activity[0, 3];
            //        switch (Priorty)
            //        {
            //            case "0":
            //                txtPriority.Text = "Low";
            //                break;
            //            case "1":
            //                txtPriority.Text = "Normal";
            //                break;
            //            case "2":
            //                txtPriority.Text = "High";
            //                break;
            //            case "3":
            //                txtPriority.Text = "Urgent";
            //                break;
            //            case "4":
            //                txtPriority.Text = "Immediate";
            //                break;
            //        }
            //        txtDateOfAllottment.Text = Activity[0, 4];
            //        txtSeheduleStart.Text = Activity[0, 5];
            //        txtSeheduleEnd.Text = Activity[0, 6];
            //        txtAcutalStart.Text = Activity[0, 7];
            //        txtAlloatedBy.Text = Activity[0, 9];
            //    }
            //    string LId = Convert.ToString(Request.QueryString["id1"]);
            //    string[,] LActivity = oDBEngine.GetFieldValue("tbl_trans_salesVisit INNER JOIN   tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id", "tbl_trans_salesVisit.slv_leadcotactId, (convert(varchar(11),tbl_trans_salesVisit.slv_lastdatevisit,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_lastdatevisit, 22), 3))) as LastVisitDate, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate,  tbl_trans_salesVisit.slv_nextvisitplace as NextVisitPlace , tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as OutCome", "tbl_trans_salesVisit.slv_leadcotactId = '" + LId + "' AND tbl_trans_salesVisit.slv_activityId = '" + Id + "'", 5);
            //    if (LActivity[0, 0] != "n")
            //    {
            //        lblLastVisit.Text = LActivity[0, 1];
            //        if (lblLastVisit.Text == "")
            //        {
            //            lblLastVisit.Text = "N/A";
            //        }
            //        lblLastOutcome.Text = LActivity[0, 4];
            //        if (lblLastOutcome.Text == "")
            //        {
            //            lblLastOutcome.Text = "N/A";
            //        }
            //        lblNextVisit.Text = LActivity[0, 2];
            //        if (lblNextVisit.Text == "")
            //        {
            //            lblNextVisit.Text = "N/A";
            //        }
            //    }
            //}
        }

        #endregion

        #region Activity DropDown Detail
        public void BindActivityName()
        {
            try
            {
                string AllUser = oDBEngine.getChildUser_for_AllEmployee(Convert.ToString(Session["userid"]), "");

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
                // .............................Code Commented and Added by Sam on 02012017. to use cnt_id of tbl_master_contact instead of Createuser . ..................................... 
                int SelectActivityindex = 0;
                DataTable SelectActivity = new DataTable();
                SelectActivity = objSlaesActivitiesBL.PopulateSalesActivitiesDropDownForSalesVisit(AllUser);
                if (SelectActivity != null && SelectActivity.Rows.Count > 0)
                {
                    DDLActivity.DataTextField = "user_name";
                    DDLActivity.DataValueField = "cnt_id";
                    DDLActivity.DataSource = SelectActivity;
                    DDLActivity.DataBind();
                    foreach (ListItem li in DDLActivity.Items)
                    {
                        if (li.Value == Convert.ToString(Session["SalescntId"]))
                        {
                            break;
                        }
                        else
                        {
                            SelectActivityindex = SelectActivityindex + 1;
                        }
                    }
                    if (SelectActivityindex == SelectActivity.Rows.Count)
                    {
                        DDLActivity.SelectedIndex = -1;
                    }
                    else
                    {
                        DDLActivity.SelectedIndex = SelectActivityindex;
                    }

                }
                DDLActivity.SelectedValue = Convert.ToString(Session["SalescntId"]);
                //string[,] SelectActivity = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in (" + AllUser + ")", 2, "user_name");
                //string[,] SelectActivity = oDBEngine.GetFieldValue()
                //string[,] SelectActivity = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in (" + AllUser + ")", 2, "user_name");
                // .............................Code Above Commented and Added by Sam on 02012017. ...................................... 

                //if (SelectActivity[0, 0] != "n")
                //{
                //    clsdropdown.AddDataToDropDownList(SelectActivity, DDLActivity);
                //    // .............................Code Commented and Added by Sam on 02012017. to use cnt_id of tbl_master_contact instead of Createuser . ..................................... 

                //    DDLActivity.SelectedValue = Convert.ToString(Session["cntId"]);
                // .............................Code Above Commented and Added by Sam on 02012017. ...................................... 

                //DDLActivity.SelectedValue = Convert.ToString(Session["userid"]);
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        protected void DDLActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PnlShowDetails.Visible = false;
            //pnlData.Visible = false;
            ////BindActivityDetails();
            //grdtbl.Visible = true;
            //btntbl.Visible = true;
        }

        #endregion Activity DropDown Detail
        public void BindActivityDetails()
        {
            try
            {
                string ActivityId = DDLActivity.SelectedItem.Value;
                Session["ACtivityID"] = ActivityId;
                //string Condition = "";
                //if (Session["condition"] != null)
                //{
                //    Condition = "" + ActivityId + " " + Convert.ToString(Session["condition"]) + "";
                //}
                //else
                //{
                //    Condition = "" + ActivityId + "  and tbl_trans_salesVisit.slv_lastdatevisit IS NULL";
                //}
                //BindDatatable(Condition);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        public void BindDatatable(string Condition1)
        {
            //DataTable dt = new DataTable();
            //DataTable dt1 = new DataTable();
            //DataColumn col1 = new DataColumn("Id");
            //DataColumn col2 = new DataColumn("Name");
            //DataColumn col3 = new DataColumn("ActId");
            //DataColumn col4 = new DataColumn("SalesVisitId");
            //DataColumn col5 = new DataColumn("AssignBy");
            //DataColumn col6 = new DataColumn("NextVisitDate");
            //DataColumn col7 = new DataColumn("Address1");
            //DataColumn col8 = new DataColumn("LastOutcome");
            //dt.Columns.Add(col1);
            //dt.Columns.Add(col2);
            //dt.Columns.Add(col3);
            //dt.Columns.Add(col4);
            //dt.Columns.Add(col5);
            //dt.Columns.Add(col6);
            //dt.Columns.Add(col7);
            //dt.Columns.Add(col8);
            //// .............................Code Commented and Added by Sam on 02012017. due to use same table but using contacttype it differenciate the lead and customer . .....................................  
            //if (Lrd.Checked == true)
            //{
            //    dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_contact.cnt_internalid AS Id,ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '')  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + " and cnt_contactType='LD' Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            //    //dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_lead.cnt_internalid AS Id,ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')  + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + "  Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            //}
            //else
            //{
            //    dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_contact.cnt_internalid AS Id,ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '')  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + " and cnt_contactType='CL'  Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");

            //    //dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id  INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_salesVisit.slv_salesvisitoutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_Master_SalesVisitOutcomeCategory ON tbl_master_SalesVisitOutCome.slv_Category = tbl_Master_SalesVisitOutcomeCategory.Int_id", "tbl_master_contact.cnt_internalid AS Id,ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '')  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name,tbl_trans_salesVisit.slv_activityId as ActId,tbl_trans_salesVisit.slv_id AS SalesVisitId, tbl_trans_Activies.act_assignedBy AssignBy, (convert(varchar(11),tbl_trans_salesVisit.slv_nextvisitdatetime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesVisit.slv_nextvisitdatetime, 22), 3))) as NextVisitDate, CASE (SELECT TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' +  ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '')  + ' ], '  + ISNULL(add_pin, '') AS Address FROM tbl_master_address WHERE add_cntid = slv_leadcotactId) END AS Address1, tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome as LastOutcome", " tbl_trans_Activies.act_assignedTo =" + Condition1 + "  Order by convert(datetime,tbl_trans_salesVisit.slv_nextvisitdatetime,101)");
            //}
            //// .............................Code Above Commented and Added by Sam on 02012017. ...................................... 
            //if (dt1.Rows.Count != 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        DataRow RowNew = dt.NewRow();
            //        RowNew["Id"] = dt1.Rows[i][0];
            //        RowNew["Name"] = dt1.Rows[i][1];
            //        RowNew["ActId"] = dt1.Rows[i][2];
            //        RowNew["SalesVisitId"] = dt1.Rows[i][3];
            //        RowNew["AssignBy"] = dt1.Rows[i][4];
            //        RowNew["NextVisitDate"] = dt1.Rows[i][5];
            //        RowNew["Address1"] = dt1.Rows[i][6];
            //        RowNew["LastOutcome"] = dt1.Rows[i][7];
            //        dt.Rows.Add(RowNew);
            //    }
            //}
            //AspxActivity.DataSource = dt;
            //AspxActivity.DataBind();


        }



        public void BindVisitPlaceAndNextVistPlace()
        {
            try
            {
                //if (Request.QueryString["id1"] != null)
                if (Session["InternalId"] != null)
                {
                    string InterNalID = Convert.ToString(Request.QueryString["id1"]);
                    string[,] AddressData = oDBEngine.GetFieldValue("tbl_master_address", "add_id as Id,add_address1 as Address", " add_cntid='" + Convert.ToString(Session["InternalId"]).Trim() + "'", 2, "add_address1");
                    if (AddressData[0, 0] != "n")
                    {
                        clsdropdown.AddDataToDropDownList(AddressData, drpVisitPlace);
                       
                        clsdropdown.AddDataToDropDownList(AddressData, drpNextVisitPlace);
                       
                        //drpNextVisitPlace.SelectedIndex = 1;
                    }
                    drpVisitPlace.Items.Insert(0, new ListItem("Select", "0"));
                    drpNextVisitPlace.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        public void BindVisitPlace()
        {
            //if (Request.QueryString["id1"] != null)
            try
            {


                if (Session["InternalId"] != null)
                {
                    string InterNalID = Convert.ToString(Request.QueryString["id1"]);
                    string[,] AddType = oDBEngine.GetFieldValue("tbl_trans_salesvisit", "slv_nextplacetype,slv_nextvisitplace", "slv_leadcotactid='" + Convert.ToString(Session["InternalId"]).Trim() + "'", 2);
                    if (AddType[0, 0].Trim() == "C")
                    {
                        rdClient.Checked = true;
                        hdcleint.Value = "CLIENT";
                        Page.ClientScript.RegisterStartupScript(GetType(), "Visibility", "<script language='JavaScript'>BranchOrClient('C');</script>");
                        // .............................Code Commented and Added by Sam on 03012017. ..................................... 

                        string[,] AddressData = oDBEngine.GetFieldValue("tbl_master_address", "add_id as Id,add_address1 as Address", " add_cntid='" + Convert.ToString(Session["InternalId"]) + "'", 2, "add_address1");
                        //string[,] AddressData = oDBEngine.GetFieldValue("tbl_master_address", "add_id as Id,add_address1 as Address", " add_cntid='" + InterNalID + "'", 2, "add_address1");

                        // .............................Code Above Commented and Added by Sam on 03012017...................................... 

                        if (AddressData[0, 0] != "n")
                        {
                            clsdropdown.AddDataToDropDownList(AddressData, drpVisitPlace);
                            //oDBEngine.AddDataToDropDownList(AddressData, drpNextVisitPlace);
                            drpVisitPlace.SelectedValue = AddressData[0, 0];
                        }
                    }
                    else if (AddType[0, 0].Trim() == "B")
                    {
                        rdBranch.Checked = true;
                        hdcleint.Value = "BRANCH";
                        Page.ClientScript.RegisterStartupScript(GetType(), "Visibility", "<script language='JavaScript'>BranchOrClient('B');</script>");

                        // .............................Code Commented and Added by Sam on 09012017. to use branch_internalId instead of branch_id ..................................... 
                        string[,] branch_add = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,branch_description", "branch_internalId='" + Convert.ToString(AddType[0, 1]).Trim() + "'", 2);

                        //string[,] branch_add = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,branch_description", "branch_id='" + Convert.ToString(AddType[0, 1]).Trim() + "'", 2);


                        // .............................Code Above Commented and Added by Sam on 09012017...................................... 

                        if (branch_add[0, 0] != "n")
                        {
                            int branchindex = 0;
                            int cnt = 0;
                            foreach (ListItem li in lstBranch.Items)
                            {
                                if (li.Value == Convert.ToString(branch_add[0, 0]))
                                {
                                    cnt = 1;
                                    break;
                                }
                                else
                                {
                                    branchindex += 1;
                                }
                            }
                            if (cnt == 1)
                            {
                                lstBranch.SelectedIndex = branchindex;
                            }
                            else
                            {
                                lstBranch.SelectedIndex = -1;
                            }

                            //txtbranch.Text = Convert.ToString(branch_add[0, 1]);
                            //txtbranch_hidden.Value = Convert.ToString(branch_add[0, 0]);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {

                string Ldate = Convert.ToDateTime(ASPxDateEdit.Value).ToString("yyyy-MM-dd");
                if(string.IsNullOrEmpty(Ldate))
                {
                    Ldate = Convert.ToString(System.DateTime.Today);

                }
                //string sDate = Convert.ToDateTime(ASPxNextVisit.Value).ToString("yyyy-MM-dd hh:mm:ss"); 

                var curr = DateTime.Now;
                String s = curr.ToString("HH:mm:ss");
                DateTime date = Convert.ToDateTime(ASPxNextVisit.Value);


                if (Convert.ToDateTime(curr).ToString("yyyy-MM-dd") == Convert.ToDateTime(date).ToString("yyyy-MM-dd") && (Request.QueryString["Pid"] == "3"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "SameDate();", true);

                }
                DateTime time = Convert.ToDateTime(s);
                DateTime dtCOMPLTDTTM = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                string sDate = Convert.ToDateTime(dtCOMPLTDTTM).ToString("yyyy-MM-dd HH:mm:ss"); 
                
            if (string.IsNullOrEmpty(sDate))
            {
                sDate = Convert.ToDateTime(System.DateTime.Today).ToString("yyyy-MM-dd HH:mm:ss"); ;

            }
            string enddate = sDate;
                //kaushik
          //  ViewState["end"] = Convert.ToDateTime(enddate.ToString()).AddDays(1).ToShortDateString();

            ViewState["end"] = Convert.ToDateTime(Ldate).ToString("yyyy-MM-dd ");
            string startdate = Convert.ToDateTime(ASPxDateEdit.Value).ToString("yyyy-MM-dd HH:mm:ss");
            ViewState["start"] = Convert.ToDateTime(sDate.ToString()).AddMinutes(-30).ToString("yyyy-MM-dd");
            string SalesVisitId = Convert.ToString(Session["SalesVisitID"]);
            string BranchId = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
            string VisitPlace = "";
            string VisitPlaceType = "";
            string NextVisitPlaceType = "";
            string NextVisitPlaceCode = "";
            string expenses = Convert.ToString(txtExp.Text);
            string OutCome = Convert.ToString(TxtOut.Text);

            string Customername = Convert.ToString(lblcustomer.Text);
            string PersonDesignation = Convert.ToString(txtpersonmetdesignation.Text);
            string CountryValue = Convert.ToString(lstCountry.SelectedValue);
            string StateValue = Convert.ToString(txtState_hidden.Value);
            string City = Convert.ToString(txtCity_hidden.Value);
            string ProductManuCapacity = Convert.ToString(txtproductmanufacture.Text);
            string RawMaterial = Convert.ToString(txtrawmaterial.Text);
            string CurrSourcing = Convert.ToString(txtcurrsourceprice.Text);
            string Discussonfuture = Convert.ToString(txtdisscussonfuture.Text);
            string Otherrsbusiness = Convert.ToString(txtotherbusiness.Text);
            string Othercustomer = Convert.ToString(txtContPhone.Text);
            string EmailIds = Convert.ToString(txtContPhone.Text);
          string PhoneNo = Convert.ToString(txtemailids.Text);

            int i = 0;

            string OutCome1 = "";
            try
            {
                i = OutCome.LastIndexOf("|");
                OutCome1 = OutCome.Substring(0, i);
            }
            catch
            {
                i = OutCome.LastIndexOf("|");
                OutCome1 = OutCome.Substring(0, i);
            }
            string Notes = Convert.ToString(txtNotes.Text);
         //   string NextVisit = Convert.ToDateTime(ASPxNextVisit.Value).ToString("yyyy-MM-dd");



            var curr1 = DateTime.Now;
            String s1 = curr1.ToString("HH:mm:ss");
            DateTime date1 = Convert.ToDateTime(ASPxNextVisit.Value);
            DateTime time1 = Convert.ToDateTime(s1);
            DateTime dtCOMPLTDTTM1 = new DateTime(date1.Year, date1.Month, date1.Day, time1.Hour, time1.Minute, time1.Second);
            string NextVisit = Convert.ToDateTime(dtCOMPLTDTTM1).ToString("yyyy-MM-dd HH:mm:ss"); 


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
            string CreateUser = Convert.ToString(Session["ACtivityID"]);
            string CreateDate = oDBEngine.GetDate().ToString("yyyy-MM-dd  HH':'mm':'ss");
            string LastModifyUser = Convert.ToString(Session["userid"]);
            string LastModifyDate = oDBEngine.GetDate().ToString("yyyy-MM-dd");
            if (rdClient.Checked == true)
            {
                if (drpVisitPlace.Items.Count > 0)
                {
                    VisitPlace = Convert.ToString(drpVisitPlace.SelectedItem.Value);
                }
                else
                {
                    VisitPlace = "0";
                }
                VisitPlaceType = "C";
            }
            else if (rdBranch.Checked == true)
            {
                if (hdnBranchVal.Value != null)
                {
                    VisitPlace = Convert.ToString(hdnBranchVal.Value);
                }
                else
                {
                    VisitPlace = "0";
                }

                //VisitPlace = Convert.ToString(txtbranch_hidden.Value);
                VisitPlaceType = "B";
            }
            if (rdNClient.Checked == true)
            {
                NextVisitPlaceType = "C";
                if (drpNextVisitPlace.Items.Count > 0)
                {
                    NextVisitPlaceCode = Convert.ToString(drpNextVisitPlace.SelectedItem.Value);
                }
                else
                {
                    NextVisitPlaceCode = "0";
                }
            }
            else if (rdNBranch.Checked == true)
            {
                if (hdnNBranchVal.Value != null)
                {
                    NextVisitPlaceCode = Convert.ToString(hdnNBranchVal.Value);
                }
                else
                {
                    NextVisitPlaceCode = "0";
                }
                //NextVisitPlaceCode = Convert.ToString(txtNBranch_hidden.Value);
                NextVisitPlaceType = "B";
            }



            string fromtype = Convert.ToString(Request.QueryString["Pid"]);
            if (fromtype == "2")
            {
                fromtype = "1";
            }
            else if (fromtype == "3")
            {
                fromtype = "3";
            }


            else if (fromtype == "4")
            {
                fromtype = "5";
            }

            else
            {
                fromtype = "2";
            }
            string nexttype = Convert.ToString(ddl_activitystatus.SelectedValue);


          ///  oDBEngine.InsurtFieldValue("tbl_trans_SalesVisitDetail", "slv_SalesVisitId,slv_Branchid,slv_VisitDateTime,slv_StartTime,slv_EndTime,slv_VisitPlace,slv_Expenses,slv_SalesVisitOutcome,slv_notes,slv_nextVisit,slv_nextactivityType,slv_ActivityType,CreateUser,CreateDate,LastModifyUser,LastModifyDate,slv_VisitPlaceType,slv_NextVisitPlaceType,slv_NextVisitPlace,Personmet,PhoneNo,Country,State,City,Customer,ProdctManucapasity,Rawmaterial,CurrentSourcingPrice,DiscussionfutureAction,Otherbusiness,ReferenceOtherCustomer,CusEmailids", "'" + SalesVisitId + "','" + BranchId + "','" + startdate + "','" + enddate + "','" + enddate + " ','" + VisitPlace + "','" + expenses + "','" + OutCome1 + "','" + Notes + "','" + NextVisit + "','" + activity + "','" + Convert.ToString(ViewState["ActivityType"]) + "','" + CreateUser + "','" + CreateDate + "','" + LastModifyUser + "','" + LastModifyDate + "','" + VisitPlaceType + "','" + Convert.ToString(NextVisitPlaceType).Trim() + "','" + Convert.ToString(NextVisitPlaceCode).Trim() + "','" + PersonDesignation + "','" + PhoneNo + "','" + CountryValue + "','" + StateValue + "','" + City + "','" + Customername + "','" + ProductManuCapacity + "','" + RawMaterial + "','" + CurrSourcing + "','" + Discussonfuture + "','" + Otherrsbusiness + "','" + Othercustomer + "','"+txtemailids.Text+"'");

            oDBEngine.InsurtFieldValue("tbl_trans_SalesVisitDetail", "slv_SalesVisitId,slv_Branchid,slv_VisitDateTime,slv_StartTime,slv_EndTime,slv_VisitPlace,slv_Expenses,slv_SalesVisitOutcome,slv_notes,slv_nextVisit,slv_nextactivityType,slv_ActivityType,CreateUser,CreateDate,LastModifyUser,LastModifyDate,slv_VisitPlaceType,slv_NextVisitPlaceType,slv_NextVisitPlace,Personmet,PhoneNo,Country,State,City,Customer,ProdctManucapasity,Rawmaterial,CurrentSourcingPrice,DiscussionfutureAction,Otherbusiness,ReferenceOtherCustomer,CusEmailids", "'" + SalesVisitId + "','" + BranchId + "','" + startdate + "','" + enddate + "','" + enddate + " ','" + VisitPlace + "','" + expenses + "','" + OutCome1 + "','" + Notes + "','" + NextVisit + "','" + nexttype + "','" + fromtype + "','" + CreateUser + "','" + CreateDate + "','" + LastModifyUser + "','" + LastModifyDate + "','" + VisitPlaceType + "','" + Convert.ToString(NextVisitPlaceType).Trim() + "','" + Convert.ToString(NextVisitPlaceCode).Trim() + "','" + PersonDesignation + "','" + PhoneNo + "','" + CountryValue + "','" + StateValue + "','" + City + "','" + Customername + "','" + ProductManuCapacity + "','" + RawMaterial + "','" + CurrSourcing + "','" + Discussonfuture + "','" + Otherrsbusiness + "','" + Othercustomer + "','" + txtemailids.Text + "'");


            BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
            obl.UpdateNextActivityDate(Convert.ToString(Request.QueryString["TransSale"]), NextVisit);
                oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_lastdatevisit='" + startdate + "',slv_lastvisitPlace='" + VisitPlace + "',slv_lastvisitDuration=0,slv_salesvisitoutcome=" + OutCome1 + ",slv_nextvisitdatetime='" + NextVisit + "',slv_nextvisitplace='" + Convert.ToString(NextVisitPlaceCode).Trim() + "',slv_PlaceType='" + VisitPlaceType + "',slv_nextplacetype='" + Convert.ToString(NextVisitPlaceType).Trim() + "'", " slv_id=" + SalesVisitId);
            
            // Code Added by sam on 17012017 to hide permission icon after phone follow up selection
            int transsalesid = 0;
            if (Request.QueryString["TransSale"]!=null)
            {
                transsalesid = Convert.ToInt32(Request.QueryString["TransSale"]);
            }
            if (rdrCall.Checked)
            {
                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextactivitystatus=2", "sls_id=" + transsalesid);
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextactivitystatus=1", "sls_id=" + transsalesid);
            }
            // Code Above Added by sam on 17012017 to hide permission icon after phone follow up selection
                #region
            if (OutCome1 == "1" || OutCome1 == "2" || OutCome1 == "3" || OutCome1 == "5" || OutCome1 == "8" || OutCome1 == "9" || OutCome1 == "10" || OutCome1 == "11" || OutCome1 == "13" || OutCome1 == "15" || OutCome1 == "18" || OutCome1 == "23" || OutCome1 == "26" || OutCome1 == "6")
            {
                string New_Startdate = enddate;
                string New_StartTime = enddate.Substring(11);
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                string Activity = "";
                try
                {
                    ds = oDBEngine.PopulateData("TOP 1 CASE slv_nextActivityType WHEN '1' THEN ' Phone Call  ' WHEN '2' THEN 'Meeting With ' END as Activity", "tbl_trans_SalesVisitDetail", " (slv_SalesVisitId = '" + Convert.ToString(Session["SalesVisitID"]) + "') order by slv_Id DESC");
                    Activity = Convert.ToString(ds.Tables[0].Rows[0]["Activity"]);
                }
                catch
                {
                }
                string Id1 = Convert.ToString(Session["InternalId"]);
                string id = Id1.Substring(0, 2);
                // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                ds1 = oDBEngine.PopulateData("ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as Name", "tbl_master_contact", "cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                //if (id == "LD")
                //{
                //    ds1 = oDBEngine.PopulateData("ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as Name", "tbl_master_lead", "cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                //}
                //else
                //{
                //    ds1 = oDBEngine.PopulateData("ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as Name", "tbl_master_contact", "cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                //}
                // .............................Code Above Commented and Added by Sam on 03012017...................................... 

                string Name1 = Convert.ToString(ds1.Tables[0].Rows[0]["Name"]);
                string PhNo = "";
                try
                {
                    ds2 = oDBEngine.PopulateData("phf_phoneNumber", "tbl_master_phonefax", "(phf_entity = 'Lead') AND (phf_type = 'mobile') AND (phf_cntId = '" + Convert.ToString(Session["InternalId"]) + "')");

                    if (ds2 != null && ds2.Tables[0].Rows.Count>0)
                    { PhNo = Convert.ToString(ds2.Tables[0].Rows[0]["phf_phoneNumber"]); }
                   
                }
                catch
                {
                }
                string Rem;
                if (OutCome1 != "26")
                {
                    Rem = Activity + "Meeting With " + Name1 + "  [ Phone Number :- " + PhNo + " ]  AT " + enddate + " " + " [" + txtNotes.Text + "]";
                }
                else
                {
                    Rem = Activity + "Call Back " + Name1 + "  [ Phone Number :- " + PhNo + " ]  AT " + enddate + " " + " [" + txtNotes.Text + "]";
                }

                SetReminder(Rem);
                if (OutCome1 == "8")
                {
                    string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", " act_activityNo");
                    string Fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                    string Values = "'" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','6','" + actNo + "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToString(Session["userid"]) + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + oDBEngine.GetDate().ToShortTimeString() + "','" + Ldate + "','" + oDBEngine.GetDate().ToShortTimeString() + "','" + txtNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'";
                    oDBEngine.InsurtFieldValue("tbl_trans_activies", Fields, Values);
                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_offeredproduct", "ofp_id", "ofp_leadid='" + Convert.ToString(Session["InternalId"]) + "'");
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        DataTable dt_salesvisit = new DataTable();
                        dt_salesvisit = oDBEngine.GetDataTable("tbl_trans_offeredProduct", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_offeredProduct.ofp_probableAmount", " tbl_trans_offeredProduct.ofp_id=" + Convert.ToString(dt.Rows[k][0]) + " and tbl_trans_offeredProduct.ofp_leadId='" + Convert.ToString(Session["InternalId"]) + "'");
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
                                string[,] id4 = oDBEngine.GetFieldValue("tbl_trans_salesvisit", "slv_nextvisitdatetime", " slv_leadcotactId='" + Convert.ToString(Session["InternalId"]) + "'", 1);
                                if (id4[0, 0] != "n")
                                {
                                    id3 = id4[0, 0];
                                }
                                string fields1 = "sls_activity_id, sls_contactlead_id, sls_branch_id, sls_sales_status, sls_date_closing, sls_ProductType ,sls_product, sls_estimated_value,sls_nextvisitdate, CreateDate,CreateUser";
                                string values1 = "'" + id2 + "','" + Convert.ToString(Session["InternalId"]) + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','4','','" + Convert.ToString(dt_salesvisit.Rows[0]["ProductType"]) + "','" + Convert.ToString(dt_salesvisit.Rows[0]["Product"]) + "','" + Convert.ToString(dt_salesvisit.Rows[0]["ofp_probableAmount"]) + "','" + id3 + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'";
                                oDBEngine.InsurtFieldValue("tbl_trans_sales", fields1, values1);
                                oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_salesactivityId='" + actNo + "'", " ofp_id='" + Convert.ToString(dt.Rows[k][0]) + "'");
                                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + Convert.ToString(Session["InternalId"]) + "'");
                                string access = "";
                                // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                                string[,] access1 = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_useraccess", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'", 1);
                                //string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'", 1);
                                // .............................Code Above Commented and Added by Sam on 03012017...................................... 
                                if (access1[0, 0] != "n")
                                {
                                    access = access1[0, 0];
                                }
                                string Sid = Convert.ToString(Session["InternalId"]);
                                string Sid1 = Sid.Substring(0, 2);
                                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                                // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                                //if (Sid1 == "LD")
                                //{
                                //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + Convert.ToString(Session["userid"]) + "',cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                                //}
                                //else
                                //{
                                //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                                //}

                                // .............................Code Above Commented and Added by Sam on 03012017...................................... 

                                oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", " tbl_trans_salesvisit.slv_leadcotactid='" + Convert.ToString(Session["InternalId"]) + "'");
                            }
                        }
                    }
                }

            }
            int assignby = 0;
            int assignto = 0;
            if (OutCome1 == "4" || OutCome1 == "12")
            {
                DataTable dt = new DataTable();
                bool flag = true;
                dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_salesVisit.slv_nextvisitdatetime, tbl_trans_offeredProduct.ofp_probableAmount, tbl_trans_offeredProduct.ofp_id as ProductId", " tbl_trans_salesvisit.slv_id='" + Convert.ToString(Session["SalesVisitID"]) + "'");
                if (dt.Rows.Count != 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Convert.ToString(dt.Rows[j]["ProductType"]) == "Refreal Agent")
                        {
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    DataTable saleActivitydt = new DataTable();
                    saleActivitydt = objSlaesActivitiesBL.GetSaleActivitydtl(Convert.ToInt32(Request.QueryString["TransSale"]));
                    if(saleActivitydt.Rows.Count>0)
                    {
                        assignby = Convert.ToInt32(saleActivitydt.Rows[0]["act_assignedBy"]);
                        assignto = Convert.ToInt32(saleActivitydt.Rows[0]["act_assignedTo"]);
                    }
                        

                    string sStartdate = enddate;
                    string sStartTime = oDBEngine.GetDate().ToShortTimeString();//enddate.Substring(11);
                    string actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                    string fields = "act_branchId, act_activityType, act_activityNo,  act_assignedBy, act_assignedTo, act_createDate, act_scheduledDate, act_scheduledTime, act_expectedDate, act_expectedTime, act_instruction,CreateDate,CreateUser";
                    // .............................Code Commented and Added by Sam on 10012017 to use cnt_id instead of session user id. ..................................... 
                    string values = Convert.ToString(HttpContext.Current.Session["userbranchID"]) + ",'6','" + actNo + "','" + assignby + "','" + assignto + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartdate + "','" + sStartTime + "','" + sStartdate + "','" + sStartTime + "','" + txtNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'";

                    //string values = Convert.ToString(HttpContext.Current.Session["userbranchID"]) + ",'6','" + actNo + "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToString(Session["userid"]) + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + sStartdate + "','" + sStartTime + "','" + sStartdate + "','" + sStartTime + "','" + txtNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'";
                   

                    // .............................Code Above Commented and Added by Sam on 10012017...................................... 
                    
                    oDBEngine.InsurtFieldValue("tbl_trans_activies", fields, values);
                    DataTable salesvisit = new DataTable();
                    salesvisit = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN tbl_trans_offeredProduct ON tbl_trans_salesVisit.slv_leadcotactId = tbl_trans_offeredProduct.ofp_leadId INNER JOIN tbl_trans_Activies ON tbl_trans_salesVisit.slv_activityId = tbl_trans_Activies.act_id", "tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, tbl_trans_offeredProduct.ofp_productId AS Product, tbl_trans_salesVisit.slv_leadcotactId, tbl_trans_salesVisit.slv_nextvisitdatetime, tbl_trans_offeredProduct.ofp_probableAmount, tbl_trans_offeredProduct.ofp_id as ProductId", " tbl_trans_salesvisit.slv_id='" + Convert.ToString(Session["SalesVisitID"]) + "'");
                    if (salesvisit.Rows.Count != 0)
                    {
                        string id = "";
                        string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_activies", "act_id", "act_activityNo='" + actNo + "'", 1);
                        if (id1[0, 0] != "n")
                        {
                            id = id1[0, 0];
                        }
                        string field = "sls_activity_id, sls_contactlead_id, sls_branch_id, sls_sales_status, sls_date_closing, sls_ProductType ,sls_product, sls_estimated_value, sls_datetime";
                        // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                        string value = "'" + id + "','" + Convert.ToString(Session["InternalId"]) + "','" + Convert.ToString(Session["userbranchID"]) + "','4','','" + Convert.ToString(salesvisit.Rows[0]["ProductType"]) + "','" + Convert.ToString(salesvisit.Rows[0]["Product"]) + "','" + Convert.ToString(salesvisit.Rows[0]["ofp_probableAmount"]) + "','" + Convert.ToString(salesvisit.Rows[0]["slv_nextvisitdatetime"]) + "'";
                        //string value = "'" + id + "','" + Convert.ToString(Session["SalesActivityId"]) + "','" + Convert.ToString(Session["userbranchID"]) + "','4','','" + Convert.ToString(salesvisit.Rows[0]["ProductType"]) + "','" + Convert.ToString(salesvisit.Rows[0]["Product"]) + "','" + Convert.ToString(salesvisit.Rows[0]["ofp_probableAmount"]) + "','" + Convert.ToString(salesvisit.Rows[0]["slv_nextvisitdatetime"]) + "'";
                        // .............................Code Above Commented and Added by Sam on 03012017......................................

                        oDBEngine.InsurtFieldValue("tbl_trans_sales", field, value);
                        oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_salesactivityId='" + actNo + "'", " ofp_id= '" + salesvisit.Rows[0]["productid"] + "'");
                        // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + Convert.ToString(Session["InternalId"]) + "'");
                        //oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_leadcotactid='" + Convert.ToString(Session["SalesActivityId"]) + "'");
                        // .............................Code Above Commented and Added by Sam on 03012017......................................
                        string access = "";
                        // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                        string[,] access1 = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_useraccess", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'", 1);
                        //string[,] access1 = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_useraccess", " cnt_internalid='" + Convert.ToString(Session["SalesActivityId"]) + "'", 1);

                        // .............................Code Above Commented and Added by Sam on 03012017...................................... 

                        if (access1[0, 0] != "n")
                        {
                            access = access1[0, 0];
                        }
                        string sid1 = Convert.ToString(Session["SalesActivityId"]);
                        string sid = sid1.Substring(0, 2);

                        // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["InternalId"]) + "'");
                        //if (sid == "LD")
                        //{
                        //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + access + "," + Convert.ToString(Session["userid"]) + "',cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["SalesActivityId"]) + "'");
                        //}
                        //else
                        //{
                        //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + actNo + "'", " cnt_internalid='" + Convert.ToString(Session["SalesActivityId"]) + "'");
                        //}

                        oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", "  tbl_trans_salesvisit.slv_leadcotactId='" + Convert.ToString(Session["InternalId"]) + "'");
                        //oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId='" + actNo + "'", "  tbl_trans_salesvisit.slv_leadcotactId='" + Convert.ToString(Session["SalesActivityId"]) + "'");

                        // .............................Code Above Commented and Added by Sam on 03012017...................................... 
                    }
                }
            }


            //BindActivityDetails();
            //pnlData.Visible = false;
            //PnlShowDetails.Visible = false;
            //grdtbl.Visible = true;
            //btntbl.Visible = true;
            // .............................Code Above Commented and Added by Sam on 03012017...................................... 


            if (Session["mode"] != null)
            {
                // .............................Code Commented and Added by Sam on 03012017. ..................................... 
                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_rating='" + Convert.ToString(Session["mode"]) + "'", " cnt_internalId='" + Convert.ToString(Session["InternalId"]) + "'");
                //string iid1 = Convert.ToString(Session["InternalId"]);
                //string iid = iid1.Substring(0, 2);
                //if (iid == "LD")
                //{
                //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_rating='" + Convert.ToString(Session["mode"]) + "'", " cnt_internalId='" + Convert.ToString(Session["InternalId"]) + "'");
                //}
                //else
                //{
                //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_rating='" + Convert.ToString(Session["mode"]) + "'", " cnt_internalId='" + Convert.ToString(Session["InternalId"]) + "'");
                //}

                // .............................Code Above Commented and Added by Sam on 03012017...................................... 
            }
#endregion

            // .............................Code Commented and Added by Sam on 10012017 to change status of trans_sale from 4 to 1 or 2 or 3. ..................................... 
            if (ddl_activitystatus.SelectedValue != "0")
            {
                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextvisitdate='" + NextVisit + "',sls_sales_status='" + ddl_activitystatus.SelectedValue + "',sls_dateTime='" + oDBEngine.GetDate().ToString() + "',sls_nextvisitplace='" + NextVisitPlaceCode + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " sls_id='" + Request.QueryString["TransSale"].ToString() + "'");
            }
            // .............................Code Above Commented and Added by Sam on 10012017...................................... 
            
            oDBEngine.SystemGeneratedMails(SalesVisitId, "Sales Visit");
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "jAlert('Saved sucessfully');window.location ='crm_sales.aspx';", true);
         //   ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Saved sucessfully.'); window.location='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "OMS/Management/Activities/crm_sales.aspx';", true);
            var pageid = Convert.ToString(Request.QueryString["Pid"]);
            var pagename = "";
            if (pageid == "1")
            {
                pagename = "crm_sales.aspx";
            }
            else if (pageid == "2")
            { pagename = "frmDocument.aspx"; }
            else if (pageid == "3")
            { pagename = "futuresale.aspx"; }
            else { pagename = "ClarificationSales.aspx"; }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpRedirect('/OMS/Management/Activities/" + pagename + "','" + ddl_activitystatus.SelectedValue + "');", true);
            //Page.ClientScript.RegisterStartupScript(GetType(), "test1", "<script language='javascript'>height();</script>");

            //Subhabrata
            #region SendmailFunctionality

            if (chkMail.Checked)
            {
                string TransSales = Convert.ToString(Request.QueryString["TransSale"]);
                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                string RecvEmail = string.Empty;
                string ActvName = string.Empty;

                DataTable dt_AssignedUserDetails = new DataTable();
                DataTable dtbl_AssignedBy = new DataTable();
                DataTable dtEmail_To = new DataTable();
                DataTable dtActivityName = new DataTable();
                DataTable dt_EmailConfig = new DataTable();
                DataTable dtbl_AssignedTo = new DataTable();
                DataTable dt_CallDisposeDetails = new DataTable();

                Employee_BL objemployeebal = new Employee_BL();

                dt_AssignedUserDetails = objemployeebal.GetEmailAccountConfigDetails(TransSales, 6);
                dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(UserId, 3);
                dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 4);
                dtActivityName = objemployeebal.GetEmailAccountConfigDetails(TransSales, 8);
                dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 1);
                dt_CallDisposeDetails = objemployeebal.GetEmailAccountConfigDetails(OutCome1, 9);

                if (!string.IsNullOrEmpty(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo"))))
                {
                    dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 2);
                }

                if (dtEmail_To.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                    {
                        RecvEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                    }
                    else
                    {
                        RecvEmail = "";
                    }
                }

                if (!string.IsNullOrEmpty(dtActivityName.Rows[0].Field<string>("act_activityNo")))
                {
                    ActvName = dtActivityName.Rows[0].Field<string>("act_activityNo");
                }
                else
                {
                    ActvName = "";
                }

                ListDictionary replacements = new ListDictionary();
                if (dtbl_AssignedTo.Rows.Count > 0)
                {
                    replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                }
                else
                {
                    replacements.Add("<%AssigneeTo%>", "");
                }
                if (dtbl_AssignedBy.Rows.Count > 0)
                {
                    replacements.Add("<%PhoneStatus%>", Convert.ToString(dt_CallDisposeDetails.Rows[0].Field<string>("slv_SalesVisitOutcome")));
                }
                else
                {
                    replacements.Add("<%PhoneStatus%>", "");
                }
                if (!string.IsNullOrEmpty(txtNotes.Text))
                {
                    replacements.Add("<%Remarks%>", Convert.ToString(txtNotes.Text));
                }
                else
                {
                    replacements.Add("<%Remarks%>", "");
                }

                if (!string.IsNullOrEmpty(RecvEmail))
                {
                    //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName, 5);
                    int MailStatus ;
                    MailStatus=ExceptionLogging.SendEmailToAssigneeByUser(RecvEmail, "", replacements, dt_EmailConfig, ActvName, 10);
                    if(MailStatus==1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Mail Send');", true);
                        return;
                    }
                    else if(MailStatus==-1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Mail Not Send.Please try again');", true);
                        return;
                    }
                }


            }

            #endregion
                //End

            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }
        public void SetReminder(string Note)
        {
            try
            {
                string[,] Rem_Id = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", "rem_sourceid='" + Convert.ToString(Session["SalesVisitID"]) + "'", 1);
                string RemId = "";
                if (Rem_Id[0, 0] != "n")
                {
                    RemId = Rem_Id[0, 0];
                }
                string new_StartDate = Convert.ToString(ViewState["start"]);
                string[] new_EndTime = Convert.ToDateTime(new_StartDate.ToString()).AddMinutes(+30).ToString().Split(' ');
                string new_endDate = Convert.ToString(ViewState["end"]);
                string msg = txtNotes.Text;
                if (RemId == "")
                {
                    oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Convert.ToString(Session["userid"]) + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Convert.ToString(Session["userid"]) + "','" + new_StartDate + "','" + new_endDate + " " + Convert.ToString(new_EndTime[1]) + " " + Convert.ToString(new_EndTime[2]) + "','" + Note + "','1','0','" + Convert.ToString(Session["SalesVisitID"]) + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Convert.ToString(Session["userid"]) + "'");
                }
                else
                {
                    oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_createUser=" + Convert.ToString(Session["userid"]) + ",rem_createDate='" + oDBEngine.GetDate().ToShortDateString() + "',rem_targetUser=" + Convert.ToString(Session["userid"]) + ",rem_startDate='" + new_StartDate + "',rem_endDate='" + new_endDate + " " + Convert.ToString(new_EndTime[1]) + "',rem_reminderContent='" + Note + "',rem_displayTricker=1,rem_actionTaken=0,rem_sourceid='" + Convert.ToString(Session["SalesVisitID"]) + "',LastModifyDate='" + oDBEngine.GetDate().ToShortDateString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " rem_id=" + RemId);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        protected void btnSetReminder_Click(object sender, EventArgs e)
        {
            try
            {
                oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken", "'" + DDLActivity.SelectedItem.Value + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + DDLActivity.SelectedItem.Value + "','" + txtStartDate.Text + "','" + txtEndTime.Text + "','" + txtNotes.Text + "',1,0");
                // .............................Code Commented and Added by Sam on 03012017.to use chosen dropdown instead ofthis textbox ..................................... 
                //txtOutCome.Text = "";
                // .............................Code Above Commented and Added by Sam on 03012017...................................... 

                txtNotes.Text = "";
                ASPxDateEdit.Text = "";
                ASPxNextVisit.Text = "";
                pnlData.Enabled = false;
               // lstCallDisposion.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        protected void btnUpdateVisit_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdateVisit.CssClass = "btnUpdate btn btn-primary active";
               btnPhoneFollowUP.CssClass = "btnUpdate btn btn-primary";
                btnUpdateVisit.Enabled = false;
                btnPhoneFollowUP.Enabled = true;
                ViewState["ActivityType"] = "1";
                Session["selectedsession"] = "FaceToFace";
                pnlData.Visible = true;
                pnlData.Enabled = true;
              //  lstCallDisposion.Enabled = true;
                btnSavePhoneCallDetails.Enabled = false;
                lblVisitDateTime.Text = "Visit DateTime";
                lblNextVisitDate.Text = "Next Visit DateTime:";
                lblNextVisitPlace.Text = "Next Visit Place:";
                //btnUpdateVisit.ForeColor = System.Drawing.Color.Blue;
                //btnPhoneFollowUP.ForeColor = System.Drawing.Color.Black;
                string next_date = oDBEngine.GetDate().ToString();
                string next_time = oDBEngine.GetDate().AddDays(1).ToString();
                //ASPxDateEdit.Text = objConverter.DateConverter(next_date, "dd/mm/yyyy hh:mm");
                //ASPxNextVisit.Text = objConverter.DateConverter(next_time, "dd/mm/yyyy hh:mm");
                ASPxNextVisit.Value = Convert.ToDateTime(next_time);
                ASPxDateEdit.Value = Convert.ToDateTime(next_date);

                //txtOutCome.Attributes.Add("onclick", "calldispose('" + txtOutCome.ID + "','salesvisit')");
                Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'> iframelocation(); </script>");
                if (hdcleint.Value == "CLIENT")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "kk1", "<script language='JavaScript'>BranchOrClient('C');</script>");
                }
                else if (hdcleint.Value == "BRANCH")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "kk", "<script language='JavaScript'>BranchOrClient('B');</script>");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
            //ElementVisit(true);
        }
        protected void btnPhoneFollowUP_Click(object sender, EventArgs e)
        {
            try
            {
              btnPhoneFollowUP.CssClass = "btnUpdate btn btn-primary active";
               btnUpdateVisit.CssClass = "btnUpdate btn btn-primary";
                //btnPhoneFollowUP.Attributes.Add("CssClass", "Active");
                //btnUpdateVisit.CssClass.Replace("Active", "");
                btnPhoneFollowUP.Enabled = false;
                btnUpdateVisit.Enabled = true;
                ViewState["ActivityType"] = "2";
                Session["selectedsession"] = "PhoneFollowUp";
                pnlData.Visible = true;
                pnlData.Enabled = true;
              //  lstCallDisposion.Enabled = true;
                btnSavePhoneCallDetails.Enabled = false;
                lblVisitDateTime.Text = "Visit DateTime";
                lblNextVisitDate.Text = "Next Call/Visit DateTime";
                lblNextVisitPlace.Text = "Visit Place";
                //btnUpdateVisit.ForeColor = System.Drawing.Color.Black;
                //btnPhoneFollowUP.ForeColor = System.Drawing.Color.Blue;
                string next_date = oDBEngine.GetDate().ToShortDateString();
                string next_time = oDBEngine.GetDate().AddDays(1).ToShortDateString();
                //ASPxDateEdit.Text = objConverter.DateConverter(next_date, "dd/mm/yyyy hh:mm");
                //ASPxNextVisit.Text = objConverter.DateConverter(next_time, "dd/mm/yyyy hh:mm");
                ASPxNextVisit.Value = Convert.ToDateTime(oDBEngine.GetDate().AddDays(1).ToString());
                ASPxDateEdit.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                //.......... Pending function by sam on 04012016............
                //txtOutCome.Attributes.Add("onclick", "calldispose('" + txtOutCome.ID + "','phonecall')");
                //.......... Pending function by sam on 04012016............
                //ElementVisit(false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        protected void BtnPending_Click(object sender, EventArgs e)
        {
            string ActivityId = DDLActivity.SelectedItem.Value;
            string Condition = " and tbl_trans_salesVisit.slv_lastdatevisit IS NULL";
            Session["condition"] = Convert.ToString(Condition);
            //BindActivityDetails();
            AspxActivity.Columns[8].Visible = true;
            PnlShowDetails.Visible = false;
            pnlData.Visible = false;
            Session["BtnSelect"] = "Pending";
            //BtnPending.ForeColor = System.Drawing.Color.Blue;
            //BtnOpen.ForeColor = System.Drawing.Color.Black;
            //BtnClosed.ForeColor = System.Drawing.Color.Black;
            //BtnConfirm.ForeColor = System.Drawing.Color.Black;
            //Page.ClientScript.RegisterStartupScript(GetType(), "calling1", "<script language='JavaScript'>height();</script>");
        }
        protected void BtnOpen_Click(object sender, EventArgs e)
        {

            //string ActivityId = DDLActivity.SelectedItem.Value;
            //string Condition = "and tbl_trans_salesVisit.slv_lastdatevisit IS Not NULL and tbl_Master_SalesVisitOutcomeCategory.Int_id in (1,2,3,5,8,9,10,11)";
            //Session["condition"] = Convert.ToString(Condition);
            ////BindActivityDetails();
            //AspxActivity.Columns[8].Visible = true;
            //PnlShowDetails.Visible = false;
            //pnlData.Visible = false;
            //Session["BtnSelect"] = "Open";
            //BtnPending.ForeColor = System.Drawing.Color.Black;
            //BtnOpen.ForeColor = System.Drawing.Color.Blue;
            //BtnClosed.ForeColor = System.Drawing.Color.Black;
            //BtnConfirm.ForeColor = System.Drawing.Color.Black;
            //Page.ClientScript.RegisterStartupScript(GetType(), "calling2", "<script language='JavaScript'>height();</script>");
        }
        protected void BtnClosed_Click(object sender, EventArgs e)
        {

            //string ActivityId = DDLActivity.SelectedItem.Value;
            //string Condition = "and tbl_Master_SalesVisitOutcomeCategory.Int_id in (6,7,13)";
            //Session["condition"] = Convert.ToString(Condition);
            ////BindActivityDetails();
            //AspxActivity.Columns[8].Visible = true;
            //PnlShowDetails.Visible = false;
            //pnlData.Visible = false;
            //Session["BtnSelect"] = "Closed";
            //BtnPending.ForeColor = System.Drawing.Color.Black;
            //BtnOpen.ForeColor = System.Drawing.Color.Black;
            //BtnClosed.ForeColor = System.Drawing.Color.Blue;
            //BtnConfirm.ForeColor = System.Drawing.Color.Black;
            //Page.ClientScript.RegisterStartupScript(GetType(), "calling3", "<script language='JavaScript'>height();</script>");
        }
        protected void BtnConfirm_Click(object sender, EventArgs e)
        {
            //string ActivityId = DDLActivity.SelectedItem.Value;
            //string Condition = "and tbl_Master_SalesVisitOutcomeCategory.Int_id in (4,12)";
            //Session["condition"] = Convert.ToString(Condition);
            ////BindActivityDetails();
            //AspxActivity.Columns[8].Visible = false;
            //// .............................Code Commented and Added by Sam on 03012017. due to pending reason ..................................... 
            ////AspxActivity.Columns[9].Visible = true; 
            //// .............................Code Above Commented and Added by Sam on 03012017...................................... 

            //PnlShowDetails.Visible = false;
            //pnlData.Visible = false;
            //Session["BtnSelect"] = "Confirm";
            //BtnPending.ForeColor = System.Drawing.Color.Black;
            //BtnOpen.ForeColor = System.Drawing.Color.Black;
            //BtnClosed.ForeColor = System.Drawing.Color.Black;
            //BtnConfirm.ForeColor = System.Drawing.Color.Blue;
            //Page.ClientScript.RegisterStartupScript(GetType(), "calling", "<script language='JavaScript'>height();</script>");
        }
        //public void BtnSelect()
        //{
        //    if (Session["BtnSelect"] != null)
        //    {
        //        string select = Convert.ToString(Session["BtnSelect"]);
        //        switch (select)
        //        {
        //            case "Pending":
        //                //BtnPending.ForeColor = System.Drawing.Color.Blue;
        //                //BtnOpen.ForeColor = System.Drawing.Color.Black;
        //                //BtnClosed.ForeColor = System.Drawing.Color.Black;
        //                //BtnConfirm.ForeColor = System.Drawing.Color.Black;
        //                break;
        //            case "Open":
        //                //BtnPending.ForeColor = System.Drawing.Color.Black;
        //                //BtnOpen.ForeColor = System.Drawing.Color.Blue;
        //                //BtnClosed.ForeColor = System.Drawing.Color.Black;
        //                //BtnConfirm.ForeColor = System.Drawing.Color.Black;
        //                break;
        //            case "Closed":
        //                //BtnPending.ForeColor = System.Drawing.Color.Black;
        //                //BtnOpen.ForeColor = System.Drawing.Color.Black;
        //                //BtnClosed.ForeColor = System.Drawing.Color.Blue;
        //                //BtnConfirm.ForeColor = System.Drawing.Color.Black;
        //                break;
        //            case "Confirm":
        //                //BtnPending.ForeColor = System.Drawing.Color.Black;
        //                //BtnOpen.ForeColor = System.Drawing.Color.Black;
        //                //BtnClosed.ForeColor = System.Drawing.Color.Black;
        //                //BtnConfirm.ForeColor = System.Drawing.Color.Blue;
        //                break;

        //        }
        //    }
        //    else
        //    {
        //        //BtnPending.ForeColor = System.Drawing.Color.Blue;
        //    }
        //}
        protected void BtnSCancel_Click(object sender, EventArgs e)
        {
        //pnlData.Visible = false;
        //  PnlShowDetails.Visible = false;
           
        // activityRow.Visible = true;
            //Response.Redirect("crm_sales.aspx", false);
            pnlData.Enabled = false;

            btnUpdateVisit.CssClass = "btnUpdate btn btn-primary";
            btnPhoneFollowUP.CssClass = "btnUpdate btn btn-primary";
            btnUpdateVisit.Enabled = true;
            btnPhoneFollowUP.Enabled = true;

           // lstCallDisposion.Enabled = false;
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
            //BindActivityDetails();
        }

        // .............................Code Commented and Added by Sam on 29122016.to Bind Call disposition list ..................................... 

        [WebMethod]
        public static List<string> GetCallDispositionList()
        {
            try
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string addtime = oDBEngine.GetDate().AddMinutes(60).ToShortTimeString().ToString();
                int t = addtime.LastIndexOf(":");
                int t1 = Convert.ToInt32(addtime.Substring(0, t));
                string t2 = addtime.Substring(t + 1, 2);
                string t3 = addtime.Substring(t + 3);
                if (t3 == " PM")
                {
                    t1 += 12;
                }
                string SelectedAddTime = t1 + ":" + t2;

                string nowtime = oDBEngine.GetDate().ToShortTimeString().ToString();
                int n = nowtime.LastIndexOf(":");
                int n1 = Convert.ToInt32(nowtime.Substring(0, n));
                string n2 = nowtime.Substring(n + 1, 2);
                string n3 = nowtime.Substring(n + 3);
                if (n3 == " PM")
                {
                    n1 += 12;
                }
                string SelectedNowTime = n1 + ":" + n2;
                DataTable dtCallDisposition = new DataTable();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                if (HttpContext.Current.Session["selectedsession"] == "FaceToFace")
                {
                    dtCallDisposition = objSlaesActivitiesBL.PopulateSaleDispositionDetailForSaleVisitOfSalesActivities(1);
                }
                else
                {
                    dtCallDisposition = objSlaesActivitiesBL.PopulateSaleDispositionDetailForSaleVisitOfSalesActivities(0);
                }

                List<string> obj = new List<string>();

                foreach (DataRow dr in dtCallDisposition.Rows)
                {
                    obj.Add(Convert.ToString(dr["slv_SalesVisitOutcome"]) + "|" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "," + SelectedAddTime + "," + oDBEngine.GetDate().ToShortDateString().ToString() + "," + SelectedNowTime + "!" + Convert.ToString(dr["Id"]));
                    //obj.Add(Convert.ToString(dr["call_dispositions"]) + "|" + Convert.ToString(dr["Id"]));
                }

                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        [WebMethod]
        public static string GetBOIsExistInBI(string keyValue)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            string ispermission = string.Empty;
            ispermission = objCRMSalesOrderDtlBL.GetCustomerDormantOrNot(keyValue);

            //}
            return Convert.ToString(ispermission);

        }

        [WebMethod]
        public static List<string> GetBranchList()
        {
            try
            {
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtBranch = new DataTable();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                dtBranch = objSlaesActivitiesBL.PopulateBranchList();
                List<string> obj = new List<string>();

                foreach (DataRow dr in dtBranch.Rows)
                {
                    obj.Add(Convert.ToString(dr["BranchName"]) + "|" + Convert.ToString(dr["branch_internalId"]));
                    //obj.Add(Convert.ToString(dr["call_dispositions"]) + "|" + Convert.ToString(dr["Id"]));
                }

                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        #region Country State City

        public void SetCountry()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_country", "  ltrim(rtrim(cou_country)) Name,ltrim(rtrim(cou_id)) Code ", null);
            lstCountry.DataSource = DT;
            lstCountry.DataMember = "Code";
            lstCountry.DataTextField = "Name";
            lstCountry.DataValueField = "Code";
            lstCountry.DataBind();
        }


        [WebMethod]
        public static List<string> GetStates(string CountryCode)
        {
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = new DataTable();
            if (CountryCode != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_state", " ltrim(rtrim(state)) Name,ltrim(rtrim(id)) Code", "countryId=" + CountryCode);
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Code"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> GetCities(string StateCode)
        {
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            if (StateCode != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_city", " ltrim(rtrim(city_name)) Name,ltrim(rtrim(city_id))Code", "state_id=" + StateCode);
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Code"]));
            }
            return obj;
        }

        #endregion

        #region Product Class and Industry Bind  customer

        public void bindClassandCustomer()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TransSale"]) && !string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("GetproductCellsandCustomer");
                proc.AddVarcharPara("@SalesId", 500, Request.QueryString["TransSale"]);
                proc.AddVarcharPara("@TypeId", 500, Request.QueryString["type"]);
                dt = proc.GetTable();

                if (dt!=null && dt.Rows.Count > 0)
                {
                    lblcustomer1.Text = Convert.ToString(dt.Rows[0]["CustomerName"]);
                    lblcustomer.Text = Convert.ToString(dt.Rows[0]["CustomerName"]);
                    lblproductclass.Text = Convert.ToString(dt.Rows[0]["ProductClass"]);
                    if (lblproductclass.Text == "")
                    {

                        liproductclass.Visible = false;
                    }
                }
            }

        }

        #endregion
        // .............................Code Above Commented and Added by Sam on 29122016...................................... 


        #region Sale Quotation and Sales Order
        protected void btnSaleQuotation_Click(object sender, EventArgs e)
        {
            int SalesId = Int32.Parse(Request.QueryString["TransSale"]);
            int TypeId = Int32.Parse(Request.QueryString["type"]);
         
            Response.Redirect("SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId, false);
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");

        }


        protected void btnSaleOrder_Click(object sender, EventArgs e)
        {
            int SalesId = Int32.Parse(Request.QueryString["TransSale"]);
            int TypeId = Int32.Parse(Request.QueryString["type"]);
        
            Response.Redirect("SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId, false);
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");

        }
        #endregion
    }
}