using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections.Specialized;
using DataAccessLayer;
using EntityLayer.CommonELS;


namespace ERP.OMS.Management
{


    public partial class management_CRMPhoneCallWithFrame : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        public EntityLayer.CommonELS.UserRightsForPage rightsQuotation = new UserRightsForPage();
        public EntityLayer.CommonELS.UserRightsForPage rightsSalesOrder = new UserRightsForPage();
        public EntityLayer.CommonELS.UserRightsForPage rightsSalesActivity = new UserRightsForPage();
        //public EntityLayer.CommonELS.UserRightsForPage rightsPhoneStatus = new UserRightsForPage();
        Management_BL Management_BL = new Management_BL();

        int cpage = 1;
        int showitemperpage = 20;
        string id1 = "";
        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        string ReceiverEmail = string.Empty;
        Employee_BL objemployeebal = new Employee_BL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] == null)
                {
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
              

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
                rightsQuotation = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/SalesQuotationList.aspx");
                rightsSalesOrder = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/SalesOrderEntityList.aspx");
                rightsSalesActivity = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/crm_sales.aspx");

                //rightsPhoneStatus = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/crm_sales.aspx"); //Rights for phone status /Management/CRMPhoneCallWithFrame.aspx


                idrightsquotation.Value = Convert.ToString(rightsQuotation.CanAdd);
                idrightsSaleorder.Value = Convert.ToString(rightsSalesOrder.CanAdd);
                idrightsSaleordFromActivity.Value = Convert.ToString(rightsSalesActivity.CanCreateOrder);
                if (!IsPostBack)
                {



                    if (Request.QueryString["Pid"] == "1")
                    {

                        dccrossaccess.InnerHtml = "<a href=Activities/crm_sales.aspx><i class='fa fa-times'></i></a>";
                    }
                    else if (Request.QueryString["Pid"] == "2")
                    {

                        dccrossaccess.InnerHtml = "<a href=Activities/frmDocument.aspx><i class='fa fa-times'></i></a>";
                    }
                    else if (Request.QueryString["Pid"] == "3")
                    {

                        dccrossaccess.InnerHtml = "<a href=Activities/futuresale.aspx><i class='fa fa-times'></i></a>";
                    }
                    else if (Request.QueryString["Pid"] == "4")
                    {

                        dccrossaccess.InnerHtml = "<a href=Activities/ClarificationSales.aspx><i class='fa fa-times'></i></a>";
                    }
                    else
                    {

                        dccrossaccess.InnerHtml = "<a href=Activities/crm_sales.aspx><i class='fa fa-times'></i></a>";
                    }


                    string EditPermitValue = string.Empty;
                    if (Request.QueryString["Pid"] == "3")
                    { ASPxNextDate.MinDate = DateTime.Now; }
                    else { ASPxNextDate.MinDate = DateTime.Now.AddDays(-1); }

                    int transSaleId = 0;
                    int cntId = 0;
                    //CallDetailShow(77);
                    string cnt_id = Convert.ToString(Session["cntId"]);
                    if (Convert.ToString(Request.QueryString["TransSale"]) != null)
                    {
                        transSaleId = Convert.ToInt32(Request.QueryString["TransSale"]);
                        cntId = Convert.ToInt32(Request.QueryString["Cid"]);
                        CallDetailShow(transSaleId);

                        EditPermitValue = EditPermissionShow(transSaleId, cnt_id);
                    }

                    //if (EditPermitValue=="1")
                    //{
                    //    ASPxNextDate.Enabled = true; 
                    //}
                    //else
                    //{ 
                    //    ASPxNextDate.Enabled = false; 
                    //}

                    lblTotalRecord.Visible = false;
                    //Session["KeyVal_InternalID"] = null;
                    btnSavePhoneCallDetails.Attributes.Add("onclick", "return chkOnSaveClick123();");
                    ViewState["Cpage"] = 1;
                    //FillGridUserInfo();
                    string next_date = oDBEngine.GetDate().AddDays(1).ToShortDateString();
                    string next_time = oDBEngine.GetDate().AddDays(1).ToShortDateString();
                    //  ASPxNextDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                    // ASPxNextDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                    color();
                    JavaScr();
                    bindClassandCustomer();
                    PhoneCall();
                    // Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");


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
                if (Session["contactInternalId"] != null)
                {
                    string Cid1 = Convert.ToString(Session["contactInternalId"]);
                    string Cid = Cid1.Substring(0, 2);
                    if (Cid.ToUpper() != "LD")
                    {
                        chkStage.Visible = false;
                        chkStage.Checked = false;
                    }
                    else
                    {
                        chkStage.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }



        #region Phone Status
        public void PhoneCall()
        {

            if (!string.IsNullOrEmpty(Convert.ToString(Session["cntId"])))
            {
                // var optionList = new List<ListItem>();
                string cntId = Convert.ToString(Session["cntId"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Assigned"]))
                {
                    string Assigned = Convert.ToString(Request.QueryString["Assigned"]);

                    string EditPermitValue = string.Empty;

                    string cnt_id = Convert.ToString(Session["cntId"]);

                    EditPermitValue = EditPermissionShow(cnt_id);

                    if (rightsSalesActivity.DocumentCollection /*&& EditPermitValue == "0"*/)
                    {

                        // ddl_activitystatus.Items.Add(new ListItem("Document Collection", "1", true));

                        RdActivityList.Items.Add(new ListItem("Document Collection", "1", true));
                    }






                    //ddl_activitystatus.Items.Add(new ListItem("Open", "0",true));

                    if ( rightsSalesActivity.ClosedSales && cntId == Assigned)
                    {
                        //   ddl_activitystatus.Items.Add(new ListItem("Closed Sales", "2"));

                        RdActivityList.Items.Add(new ListItem("Closed Sales", "2", true));
                    }
                    //  ddl_activitystatus.Items.Add(new ListItem("Future Sales", "3"));
                    //  ddl_activitystatus.Items.Add(new ListItem("Clarification Required", "5"));

                    if (rightsSalesActivity.FutureSales) RdActivityList.Items.Add(new ListItem("Future Sales", "3", true));


                    if (rightsSalesActivity.ClarificationRequired) RdActivityList.Items.Add(new ListItem("Clarification Required", "5", true));






                }

            }
        }
        #endregion

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
        public void CallDetailShow(int transSaleId)
        {
            try
            {
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                string date = oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss");
                ViewState["callstart"] = date.ToString();
                //TxtNext.Visible = false;
                string id = "";
                string phccontactid = "";
                //Session["contactInternalId"]
                DataTable phonecallDt = new DataTable();
                phonecallDt = objSlaesActivitiesBL.GetPhoneCallIdToShowDetail(transSaleId);
                if (phonecallDt != null && phonecallDt.Rows.Count > 0)
                {
                    id = Convert.ToString(phonecallDt.Rows[0]["phc_id"]);
                    phccontactid = Convert.ToString(phonecallDt.Rows[0]["phc_leadcotactId"]);
                    Session["contactInternalId"] = phccontactid;
                    string Cid1 = Convert.ToString(Session["contactInternalId"]);
                    string Cid = Cid1.Substring(0, 2);
                    if (Cid.ToUpper() != "LD")
                    {
                        chkStage.Visible = false;
                        chkStage.Checked = false;
                    }
                    else
                    {
                        chkStage.Visible = true;
                    }
                }


                //int k = grdUserInfo.Rows.Count;
                //foreach (GridViewRow gvrow in grdUserInfo.Rows)
                //{
                //    Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");

                //    CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                //    if (chk.Checked == true)
                //    {
                //        id = Convert.ToString(lbl.Text);

                //    }
                //}
                if (id != "")
                {
                    ShowCallDetails(id);
                    LeadInformation(id);

                    pnlFButton.Visible = false;

                    if (Session["btn"] != null)
                    {
                        PnlGridStatus(true, false, false, true);
                    }
                    else
                    {
                        PnlGridStatus(false, true, true, false);
                    }
                    //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>iframesource();height();</script>");
                }
                else
                {
                    //if (Session["btn"] != null)
                    //{
                    //    PnlGridStatus(true, false, false, true);
                    //}
                    //else
                    //{
                    //    PnlGridStatus(false, true, true, false);
                    //}
                    ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>jAlert('No Record Found')</script>");
                    //Response.Redirect("Activities/crm_sales.aspx", false);
                }
                TotalNoOfCalls.Visible = false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
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



        #region Bind Grid Section
        public void FillGridUserInfo()
        {
            //try
            //{
            //    grdForwardCall.Visible = false;
            //    DataTable forware = new DataTable();
            //    forware.Columns.Add("Id");
            //    forware.Columns.Add("UID");
            //    forware.Columns.Add("Name");
            //    forware.Columns.Add("PhoneNumber");
            //    forware.Columns.Add("NextCall");
            //    forware.Columns.Add("CallOutCome");
            //    forware.Columns.Add("Assign To");
            //    string discbledStr = "";
            //    DataTable UserInfo = new DataTable();
            //    DataTable dt = new DataTable();
            //    DataTable temp = new DataTable();
            //    DataTable CallForward = new DataTable();
            //    UserInfo.Columns.Add("Id");
            //    UserInfo.Columns.Add("UID");
            //    UserInfo.Columns.Add("Name");
            //    UserInfo.Columns.Add("PhoneNumber");
            //    string SelectBtn = "";
            //    if (Session["PhoneButton"] != null)
            //    {
            //        SelectBtn = Convert.ToString(Session["PhoneButton"]);
            //    }
            //    else
            //    {
            //        SelectBtn = "newcalls";
            //        // BtnCalls.ForeColor = System.Drawing.Color.Blue;
            //    }
            //    if (SelectBtn == "requestsalesvisit" || SelectBtn == "courtesycalls")
            //    {
            //        UserInfo.Columns.Add("NextVisit");
            //        UserInfo.Columns.Add("Address");
            //        UserInfo.Columns.Add("Assign To");
            //        UserInfo.Columns.Add("History");
            //    }
            //    else
            //    {
            //        if (SelectBtn != "newcalls")
            //        {
            //            UserInfo.Columns.Add("NextCall");
            //        }
            //        UserInfo.Columns.Add("CallOutCome");
            //        if (SelectBtn != "newcalls")
            //        {
            //            UserInfo.Columns.Add("History");
            //        }
            //    }
            //    if (SelectBtn == "forwardCalls")
            //    {
            //        UserInfo.Columns.Add("Assign By");
            //    }
            //    DataTable Totalrecord = new DataTable();
            //    string record = "";
            //    int contactId = 0;
            //    if (HttpContext.Current.Session["cntId"] != null)
            //    {
            //        contactId = Convert.ToInt32(HttpContext.Current.Session["cntId"]);
            //    }
            //    switch (SelectBtn)
            //    {
            //        case "newcalls":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "' AND (phc_callDispose=11)");
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo =30242 AND (phc_callDispose=11)");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo =" + contactId + " AND (phc_callDispose=11)");
            //            break;
            //        case "callback":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 1 and call_id!=11))");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = " + contactId + " ) AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 1 and call_id!=11))");
            //            break;
            //        case "notinterested":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 2))");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = " + contactId + " ) AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 2))");
            //            break;
            //        case "outofservice":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 3))");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = " + contactId + " ) AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 3))");
            //            break;
            //        case "notreachable":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "' AND tbl_trans_phonecall.phc_callDispose = 2");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo =" + contactId + "  AND tbl_trans_phonecall.phc_callDispose = 2");
            //            break;
            //        case "noresponse":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 5))");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo =" + contactId + " ) AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 5))");
            //            break;
            //        case "interested":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 6))");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo =" + contactId + " ) AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 6))");
            //            break;
            //        case "requestsalesvisit":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "' AND tbl_trans_phonecall.phc_callDispose = 9");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(tbl_trans_phonecall.phc_leadcotactId)", " tbl_trans_Activies.act_assignedTo = " + contactId + " AND tbl_trans_phonecall.phc_callDispose = 9");
            //            break;
            //        case "courtesycalls":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_CourtesyCalls", "count(cpc_leadcontactId) AS phc_leadcotactid", " cpc_userid = '" + Session["userid"].ToString() + "'");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_CourtesyCalls", "count(cpc_leadcontactId) AS phc_leadcotactid", " cpc_userid =" + contactId + "");
            //            break;
            //        case "forwardCalls":
            //            //Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo = '" + Session["userid"].ToString() + "') AND (tbl_trans_phonecall.phc_forwardCall <> '') and tbl_trans_phonecall.LastModifyUser <> '" + Session["userid"].ToString() + "'");
            //            Totalrecord = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "count(phc_leadcotactId)", " (tbl_trans_Activies.act_assignedTo =" + contactId + ") AND (tbl_trans_phonecall.phc_forwardCall <> '') and tbl_trans_phonecall.LastModifyUser <> " + Session["userid"].ToString() + "");
            //            break;
            //    }
            //    record = Totalrecord.Rows[0][0].ToString();
            //    int ctotalItem = 0;
            //    if (ViewState["Cpage"] != null)
            //    {
            //        cpage = Convert.ToInt32(ViewState["Cpage"]);
            //        ctotalItem = cpage * showitemperpage;
            //    }
            //    else
            //    {
            //        ctotalItem = 20;
            //    }
            //    int pg = Convert.ToInt32(record);
            //    pg -= ctotalItem;
            //    int currentItemNumber = showitemperpage;
            //    if (ctotalItem > Convert.ToInt32(record))
            //    {
            //        currentItemNumber = Convert.ToInt32(record) - ((cpage - 1) * showitemperpage);
            //        lblTotalRecord.Text = "Remaining Calls :  " + (Convert.ToInt32(record) - Convert.ToInt32(record));
            //    }
            //    if (cpage == 1)
            //    {
            //        TxtPrevious.Visible = false;
            //    }
            //    else
            //    {
            //        TxtPrevious.Visible = true;
            //    }
            //    if (Convert.ToInt32(record) < ctotalItem)
            //    {
            //        TxtNext.Visible = false;
            //    }
            //    else
            //    {
            //        TxtNext.Visible = true;
            //    }
            //    lblTotalNumberofCalls.Text = "Total Number of Calls : " + record;
            //    switch (SelectBtn)
            //    {
            //        case "newcalls":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " tbl_trans_Activies.act_assignedTo =30242 AND (phc_callDispose=11) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =30242)) order by tbl_trans_phonecall.createdate desc) DERIVEDTBL ");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " tbl_trans_Activies.act_assignedTo =" + contactId + " AND (phc_callDispose=11) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by tbl_trans_phonecall.createdate desc) DERIVEDTBL ");
            //            break;
            //        case "callback":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id,(convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 1)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id,(convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 1)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "notinterested":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 2)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 2)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "outofservice":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 3)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 3)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "notreachable":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + " AND tbl_trans_phonecall.phc_callDispose = 2 AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo =" + contactId + " AND tbl_trans_phonecall.phc_callDispose = 2 AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Session["userid"].ToString() + ")) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "noresponse":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 5)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 5)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "interested":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 6)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 6)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "requestsalesvisit":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall, tbl_trans_phonecall.phc_id, CASE isnull(tbl_trans_phonecall.phc_NextActivityId, '') WHEN '' THEN 'None' ELSE (SELECT top 1 tbl_master_contact.cnt_firstName FROM tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_master_user.user_contactId = tbl_master_contact.cnt_internalId WHERE (tbl_trans_Activies.act_activityNo = tbl_trans_phonecall.phc_NextActivityId)) END AS NewActivity,tbl_trans_phonecall.phc_NextActivityId as NextActivityId", " tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + " AND tbl_trans_phonecall.phc_callDispose = 9 AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall, tbl_trans_phonecall.phc_id, CASE isnull(tbl_trans_phonecall.phc_NextActivityId, '') WHEN '' THEN 'None' ELSE (SELECT top 1 tbl_master_contact.cnt_firstName FROM tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_master_user.user_contactId = tbl_master_contact.cnt_internalId WHERE (tbl_trans_Activies.act_activityNo = tbl_trans_phonecall.phc_NextActivityId)) END AS NewActivity,tbl_trans_phonecall.phc_NextActivityId as NextActivityId", " tbl_trans_Activies.act_assignedTo = " + contactId + " AND tbl_trans_phonecall.phc_callDispose = 9 AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "courtesycalls":
            //            //commented by Sam on 28122016 Pending due to cpc_userid unknown type
            //            //dt = oDBEngine.GetDataTable("tbl_trans_CourtesyCalls", "top " + currentItemNumber + " * from (select top " + ctotalItem + " cpc_leadcontactId AS phc_leadcotactid, cpc_phoneCallId AS phc_id, CASE isnull(tbl_trans_CourtesyCalls.cpc_salesVisitId, '') WHEN '' THEN 'None' ELSE (SELECT     tbl_master_contact.cnt_firstName FROM tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_master_user.user_contactId = tbl_master_contact.cnt_internalId WHERE (tbl_trans_Activies.act_activityNo = tbl_trans_CourtesyCalls.cpc_salesVisitId)) END AS NewActivity, tbl_trans_CourtesyCalls.cpc_salesVisitId as NextActivityId", " cpc_userid = " + Session["userid"].ToString() + ") DERIVEDTBL order by phc_id");
            //            dt = oDBEngine.GetDataTable("tbl_trans_CourtesyCalls", "top " + currentItemNumber + " * from (select top " + ctotalItem + " cpc_leadcontactId AS phc_leadcotactid, cpc_phoneCallId AS phc_id, CASE isnull(tbl_trans_CourtesyCalls.cpc_salesVisitId, '') WHEN '' THEN 'None' ELSE (SELECT     tbl_master_contact.cnt_firstName FROM tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_contact ON tbl_master_user.user_contactId = tbl_master_contact.cnt_internalId WHERE (tbl_trans_Activies.act_activityNo = tbl_trans_CourtesyCalls.cpc_salesVisitId)) END AS NewActivity, tbl_trans_CourtesyCalls.cpc_salesVisitId as NextActivityId", " cpc_userid =" + contactId + ") DERIVEDTBL order by phc_id");
            //            // code end
            //            break;
            //        case "confirmsale":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + " AND tbl_trans_phonecall.phc_callDispose = 10  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo = " + contactId + " AND tbl_trans_phonecall.phc_callDispose = 10  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "numbernotexist":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " bl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + " AND tbl_trans_phonecall.phc_callDispose = 5  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " bl_trans_Activies.act_assignedTo = " + contactId + " AND tbl_trans_phonecall.phc_callDispose = 5  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");
            //            break;
            //        case "pendingcall":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + " AND tbl_trans_phonecall.phc_callDispose Not in (1,2,3,4,5,6,7,9,10)  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser = " + Session["userid"].ToString() + ")) order by phc_id) DERIVEDTBL");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall", " tbl_trans_Activies.act_assignedTo = " + contactId + " AND tbl_trans_phonecall.phc_callDispose Not in (1,2,3,4,5,6,7,9,10)  AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by phc_id) DERIVEDTBL");
            //            break;
            //        case "forwardCalls":
            //            //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose,tbl_trans_Activies.act_assignedTo,tbl_trans_Activies.act_assignedBy", " (tbl_trans_Activies.act_assignedTo = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_forwardCall <> '' or tbl_trans_phonecall.phc_forwardcall is not null) and tbl_trans_phonecall.LastModifyUser <> " + Session["userid"].ToString() + " order by phc_id desc) DERIVEDTBL order by phc_id");
            //            dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose,tbl_trans_Activies.act_assignedTo,tbl_trans_Activies.act_assignedBy", " (tbl_trans_Activies.act_assignedTo =" + contactId + ") AND (tbl_trans_phonecall.phc_forwardCall <> '' or tbl_trans_phonecall.phc_forwardcall is not null) and tbl_trans_phonecall.LastModifyUser <> " + Convert.ToString(Session["userid"]) + " order by phc_id desc) DERIVEDTBL order by phc_id");


            //            //CallForward = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose,tbl_trans_Activies.act_assignedTo,tbl_trans_Activies.act_assignedBy", " (tbl_trans_Activies.act_assignedBy = " + Session["userid"].ToString() + ") AND (tbl_trans_phonecall.phc_forwardCall <> '' or tbl_trans_phonecall.phc_forwardcall is not null) OR (tbl_trans_phonecall.phc_forwardCall <> '') AND (tbl_trans_phonecall.phc_forwardCall = '" + Session["userid"].ToString() + "') order by phc_id desc");
            //            CallForward = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "phc_leadcotactId,phc_id,(convert(varchar(11),phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose,tbl_trans_Activies.act_assignedTo,tbl_trans_Activies.act_assignedBy", " (tbl_trans_Activies.act_assignedBy = " + contactId + ") AND (tbl_trans_phonecall.phc_forwardCall <> '' or tbl_trans_phonecall.phc_forwardcall is not null) OR (tbl_trans_phonecall.phc_forwardCall <> '') AND (tbl_trans_phonecall.phc_forwardCall = '" + Convert.ToString(Session["userid"]) + "') order by phc_id desc");

            //            break;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        grdUserInfo.DataSource = UserInfo.DefaultView;
            //        grdUserInfo.DataBind();
            //        lblTotalNumberofCalls.Text = "No Calls For You";
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string IID1 = Convert.ToString(dt.Rows[i][0]);
            //        string IID = IID1.Substring(0, 2);
            //        if (IID == "LD")
            //        {
            //            temp = oDBEngine.GetDataTable("tbl_master_lead", "*", "cnt_internalId='" + Convert.ToString(dt.Rows[i][0]) + "'");
            //        }
            //        else
            //        {
            //            temp = oDBEngine.GetDataTable("tbl_master_contact", "*", "cnt_internalId='" + Convert.ToString(dt.Rows[i][0]) + "'");
            //        }
            //        Session["contactInternalId"] = Convert.ToString(dt.Rows[i][0]);
            //        for (int j = 0; j < temp.Rows.Count; j++)
            //        {
            //            DataRow Row = UserInfo.NewRow();
            //            Row[0] = Convert.ToString(dt.Rows[i]["phc_id"]);
            //            Row[1] = Convert.ToString(dt.Rows[i]["phc_leadcotactId"]);
            //            Session["PhoneLeadId"] = Convert.ToString(dt.Rows[i]["phc_leadcotactId"]);
            //            Row[2] = Convert.ToString(temp.Rows[0]["cnt_firstName"]) + " " + Convert.ToString(temp.Rows[0]["cnt_middleName"]) + " " + Convert.ToString(temp.Rows[0]["cnt_lastName"]);
            //            DataTable PhoneCall = new DataTable();
            //            Session["contactInternalId"] = Convert.ToString(temp.Rows[0]["cnt_internalId"]);
            //            PhoneCall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + Convert.ToString(temp.Rows[0]["cnt_internalId"]) + "'");
            //            for (int ij = 0; ij < PhoneCall.Rows.Count; ij++)
            //            {
            //                string PhoneType = Convert.ToString(PhoneCall.Rows[ij]["phf_type"]).ToUpper();
            //                switch (PhoneType)
            //                {
            //                    case "MOBILE":
            //                        Row[3] += "(M)" + Convert.ToString(PhoneCall.Rows[ij]["phf_countryCode"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_areaCode"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]);
            //                        break;
            //                    case "RESIDENCE":
            //                        Row[3] += "(R)" + Convert.ToString(PhoneCall.Rows[ij]["phf_countryCode"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]);
            //                        break;
            //                    case "OFFICIAL":
            //                        Row[3] += "(O)" + Convert.ToString(PhoneCall.Rows[ij]["phf_countryCode"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_areaCode"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]) + " " + Convert.ToString(PhoneCall.Rows[ij]["phf_extension"]);
            //                        break;
            //                }
            //            }
            //            bool Flag = false;
            //            if (SelectBtn != "newcalls")
            //            {
            //                if (SelectBtn == "requestsalesvisit" || SelectBtn == "courtesycalls")
            //                {
            //                    if (Convert.ToString(dt.Rows[i]["NewActivity"]) != "None")
            //                    {
            //                        DataTable NextCall = new DataTable();
            //                        NextCall = oDBEngine.GetDataTable("tbl_trans_salesVisit", "convert(datetime,slv_nextvisitdatetime ,101) as NextCall", " (slv_leadcotactId = '" + Convert.ToString(dt.Rows[i]["phc_leadcotactId"]) + "') AND (slv_activityId in (SELECT act_id FROM tbl_trans_Activies WHERE (act_activityNo = '" + Convert.ToString(dt.Rows[i]["NextActivityId"]) + "')))");
            //                        if (NextCall.Rows.Count != 0)
            //                        {
            //                            string call = Convert.ToString(NextCall.Rows[0]["NextCall"]);
            //                            DateTime nCall = Convert.ToDateTime(call);
            //                            DateTime Now = Convert.ToDateTime(Convert.ToString(oDBEngine.GetDate()));
            //                            if (nCall < Now)
            //                            {
            //                                Flag = true;
            //                            }
            //                            Row[4] = Convert.ToString(NextCall.Rows[0]["NextCall"]);
            //                        }
            //                        else
            //                        {
            //                            Row[4] = "N/A";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Row[4] = Convert.ToString(dt.Rows[i]["NextCall"]);
            //                    }
            //                }
            //                else
            //                {
            //                    Row[4] = Convert.ToString(dt.Rows[i]["NextCall"]);
            //                }
            //            }
            //            if (SelectBtn == "requestsalesvisit" || SelectBtn == "courtesycalls")
            //            {
            //                DataTable Address = new DataTable();
            //                Address = oDBEngine.GetDataTable("tbl_master_address", "ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' + ISNULL(add_pin, '') AS Address", " add_cntId='" + Convert.ToString(temp.Rows[0]["cnt_internalId"]) + "' and add_activityId='" + Convert.ToString(dt.Rows[i]["phc_id"]) + "'");
            //                if (Address.Rows.Count != 0)
            //                {
            //                    Row[5] = Convert.ToString(Address.Rows[0]["Address"]);
            //                }
            //                else
            //                {
            //                    Address.Dispose();
            //                    Address = oDBEngine.GetDataTable("tbl_master_address", "(isnull(add_address1,'')+isnull(add_address2,'')+isnull(add_address3,'')+' '+isnull(add_landMark,''))as Address", " add_cntId='" + Convert.ToString(temp.Rows[0]["cnt_internalId"]) + "'");
            //                    if (Address.Rows.Count != 0)
            //                    {
            //                        Row[5] = Convert.ToString(Address.Rows[0]["Address"]);
            //                    }
            //                }
            //                if (Convert.ToString(dt.Rows[i]["NewActivity"]) == "None")
            //                {
            //                    Row[6] = Convert.ToString(dt.Rows[i]["NewActivity"]);
            //                }
            //                else
            //                {
            //                    if (Flag)
            //                    {
            //                        discbledStr += i + "-" + i + ",";
            //                    }
            //                    else
            //                    {
            //                        discbledStr += i + ",";
            //                    }
            //                    Row[6] = Convert.ToString(dt.Rows[i]["NewActivity"]);
            //                }

            //            }
            //            else
            //            {
            //                if (SelectBtn != "newcalls")
            //                {
            //                    LinkButton Lnk = new LinkButton();
            //                    Lnk.Attributes.Add("onclick", "frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + Convert.ToString(dt.Rows[i]["phc_leadcotactid"]) + "','History',300,800)");
            //                    Lnk.Text = "History";
            //                    Row[5] = Convert.ToString(dt.Rows[i]["CallDispose"]);
            //                }
            //                else
            //                    Row[4] = Convert.ToString(dt.Rows[i]["CallDispose"]);
            //            }
            //            if (SelectBtn == "forwardCalls")
            //            {
            //                Row[6] = oDBEngine.GetDataTable("tbl_master_user", "user_Name", " user_id='" + Convert.ToString(dt.Rows[i]["act_assignedby"]) + "'");
            //            }
            //            UserInfo.Rows.Add(Row);
            //        }
            //    }
            //    grdUserInfo.DataSource = UserInfo.DefaultView;
            //    grdUserInfo.DataBind();
            //    if (grdUserInfo.Columns.Count != 0)
            //    {
            //        grdUserInfo.Columns[1].Visible = false;
            //    }
            //    if (SelectBtn == "forwardCalls")
            //    {
            //        for (int forwd = 0; forwd < CallForward.Rows.Count; forwd++)
            //        {
            //            DataRow NRow = forware.NewRow();
            //            NRow[0] = Convert.ToString(CallForward.Rows[forwd]["phc_id"]);
            //            NRow[1] = Convert.ToString(CallForward.Rows[forwd]["phc_leadcotactId"]);
            //            string[,] name = oDBEngine.GetFieldValue("tbl_master_Lead", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastName,'')  as Name", " cnt_internalid='" + Convert.ToString(CallForward.Rows[forwd]["phc_leadcotactId"]) + "'", 1);
            //            if (name[0, 0] != "n")
            //            {
            //                NRow[2] = name[0, 0];
            //            }
            //            DataTable PhoneCall = new DataTable();
            //            PhoneCall = oDBEngine.GetDataTable("tbl_master_phoneFax", "*", " phf_cntId='" + Convert.ToString(CallForward.Rows[forwd]["phc_leadcotactId"]) + "'");
            //            for (int ij = 0; ij < PhoneCall.Rows.Count; ij++)
            //            {
            //                string PhType = Convert.ToString(PhoneCall.Rows[ij]["phf_type"]).ToUpper();
            //                switch (PhType)
            //                {
            //                    case "MOBILE":
            //                        NRow[3] += "(M)" + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]);
            //                        break;
            //                    case "RESIDENCE":
            //                        NRow[3] += "(R)" + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]);
            //                        break;
            //                    case "OFFICIAL":
            //                        NRow[3] += "(O)" + Convert.ToString(PhoneCall.Rows[ij]["phf_phoneNumber"]);
            //                        break;
            //                }

            //            }
            //            NRow[4] = Convert.ToString(CallForward.Rows[forwd]["NextCall"]);
            //            NRow[5] = Convert.ToString(CallForward.Rows[forwd]["CallDispose"]);
            //            string[,] Uname = oDBEngine.GetFieldValue("tbl_master_user", "user_Name", " user_id='" + Convert.ToString(CallForward.Rows[forwd]["act_assignedTo"]) + "'", 1);
            //            if (Uname[0, 0] != "n")
            //            {
            //                NRow[6] = Uname[0, 0];
            //            }
            //            forware.Rows.Add(NRow);
            //        }
            //        grdForwardCall.Visible = true; ;
            //        grdForwardCall.DataSource = forware.DefaultView;
            //        grdForwardCall.DataBind();
            //    }

            //    //foreach (GridViewRow item in grdUserInfo.)
            //    //{
            //    //    for (int i = 1; i < item.Cells.Count; i++)
            //    //    {
            //    //        item.Cells[i].Text = "1";
            //    //    }
            //    //}


            //    for (int i = 0; i < grdUserInfo.Rows.Count; i++)
            //    {
            //        Label lbl = (Label)grdUserInfo.Rows[i].FindControl("lblId");
            //        if (SelectBtn != "newcalls")
            //        {
            //            LinkButton lnk = new LinkButton();
            //            lnk.Attributes.Add("onclick", "frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + Convert.ToString(dt.Rows[i]["phc_leadcotactid"]) + "','History','900px','450px')");
            //            lnk.Text = "History";
            //            if (SelectBtn == "requestsalesvisit" || SelectBtn == "courtesycalls")
            //            {
            //                grdUserInfo.Rows[i].Cells[9].Controls.Clear();
            //                grdUserInfo.Rows[i].Cells[9].Text = "<div style='CURSOR: hand;color:#330099;' onclick= javascript:frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + Convert.ToString(grdUserInfo.Rows[i].Cells[3].Text) + "','History','900px','450px')>History</div>";
            //            }
            //            else
            //            {
            //                grdUserInfo.Rows[i].Cells[8].Controls.Clear();
            //                grdUserInfo.Rows[i].Cells[8].Text = "<div style='CURSOR: hand;color:#330099;' onclick= javascript:frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + Convert.ToString(grdUserInfo.Rows[i].Cells[3].Text) + "','History','900px','450px')>History</div>";
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    grdUserInfo.Visible = false;
            //}

        }

        #endregion Bind Grid Section
        public void ShowCallDetails(string id)
        {
            try
            {
                Session["btn"] = null;
                DataTable dt = new DataTable();
                //dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", "tbl_trans_phonecall.phc_leadcotactId as phc_leadcotactId,tbl_trans_Activies.act_id as act_id,tbl_trans_Activies.act_assignedBy as act_assignedBy,(convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) as act_createDate,(convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) as act_actualStartDate,tbl_trans_Activies.act_priority as act_priority,convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5))+RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3)as act_scheduledDate ,tbl_trans_Activies.act_scheduledTime as act_scheduledTime,(convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3)))as act_expectedDate,tbl_trans_Activies.act_expectedTime as act_expectedTime,tbl_trans_Activies.act_instruction as act_instruction", " tbl_trans_phonecall.phc_id = '" + id + "'");
                //   dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id", "tbl_trans_phonecall.phc_leadcotactId as phc_leadcotactId,tbl_trans_Activies.act_id as act_id,tbl_trans_Activies.act_assignedBy as act_assignedBy,(convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) as act_createDate,(convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) as act_actualStartDate,tbl_trans_Activies.act_priority as act_priority,convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113)+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 10, 5))+RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_scheduledDate, 22), 3)as act_scheduledDate ,tbl_trans_Activies.act_scheduledTime as act_scheduledTime,(convert(varchar(11),tbl_trans_Activies.act_expectedDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_expectedDate, 22), 3)))as act_expectedDate,tbl_trans_Activies.act_expectedTime as act_expectedTime,tbl_trans_Activies.act_instruction as act_instruction", " tbl_trans_phonecall.phc_id = '" + id + "'");
                string cntId;

                cntId = Convert.ToString(Request.QueryString["Cid"]);
                string transSaleId = Convert.ToString(Request.QueryString["TransSale"]);
                dt = objBL.GetPhoneInfo(id, cntId, transSaleId);
                if (dt != null && dt.Rows.Count != 0)
                {
                    ViewState["PhcLeadContactId"] = Convert.ToString(dt.Rows[0]["phc_leadcotactId"]);
                    Session["LeadId"] = Convert.ToString(ViewState["PhcLeadContactId"]);
                    if (Convert.ToString(dt.Rows[0]["phc_leadcotactId"]).Substring(0, 2) == "LD")
                    {
                        Session["ContactType"] = "Lead";
                        Session["KeyVal_InternalID"] = Convert.ToString(dt.Rows[0]["phc_leadcotactId"]);
                    }
                    else if (Convert.ToString(dt.Rows[0]["phc_leadcotactId"]).Substring(0, 2) == "CL")
                    {
                        Session["ContactType"] = "Customer/Client";
                        Session["KeyVal_InternalID"] = Convert.ToString(dt.Rows[0]["phc_leadcotactId"]);
                    }
                    // .............................Pending Code Commented and Added by Sam on 03012017 to unused currently. .....................................                     
                    //aa.Attributes.Add("onclick", "frmOpenNewWindow1('ShowHistory_Phonecall.aspx?id1=" + Convert.ToString(Session["LeadId"]) + "','Show History','950px','450px')");
                    //HyperLink1.Attributes.Add("onclick", "frmOpenNewWindow1('Contact_Correspondence.aspx?typeP=modify&requesttypeP=lead&formtypeP=lead','Phone/Adress','950px','450px')");
                    //HyperLink2.Attributes.Add("onclick", "frmOpenNewWindow1('Lead_general.aspx?typeP=modify&requesttypeP=lead&formtypeP=lead','Contact Details','950px','450px')");
                    // .............................Code Above Commented and Added by Sam on 03012017...................................... 
                    Session["newactivityid"] = Convert.ToString(dt.Rows[0]["act_id"]);
                    string assignby = Convert.ToString(dt.Rows[0]["act_assignedBy"]);
                    //string[,] assign = oDBEngine.GetFieldValue("tbl_master_user", "user_name", " user_id='" + assignby + "'", 1);
                    string[,] assign = oDBEngine.GetFieldValue("tbl_master_contact", "   isnull(tbl_master_contact.cnt_firstName,'')+' ' +isnull(tbl_master_contact.cnt_middleName,'')+' ' +isnull(tbl_master_contact.cnt_lastName,'') user_name", "  tbl_master_contact.cnt_id='" + assignby + "'", 1);


                    if (assign[0, 0] != "n")
                    {
                        txtAlloatedBy.Text = assign[0, 0];
                    }
                    txtDateOfAllottment.Text = Convert.ToString(dt.Rows[0]["act_createDate"]);
                    txtAcutalStart.Text = Convert.ToString(dt.Rows[0]["act_actualStartDate"]);
                    string s = Convert.ToString(dt.Rows[0]["act_priority"]);
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
                    //string txtsdate = objConverter.DateConverter_d_m_y(dt.Rows[0]["act_scheduledDate"].ToString());
                    string txtsdate = Convert.ToString(dt.Rows[0]["act_scheduledDate"]);
                    //txtSeheduleStart.Text = txtsdate.Substring(0, 10) + " " + dt.Rows[0]["act_scheduledTime"].ToString();
                    txtSeheduleStart.Text = Convert.ToString(dt.Rows[0]["act_scheduledDate"]);
                    txtTotalNumberofCalls.Text = getTotalNoOfCalls(Convert.ToString(dt.Rows[0]["act_id"]));
                    //string txtsend = objConverter.DateConverter_d_m_y(dt.Rows[0]["act_expectedDate"].ToString());
                    string txtsend = Convert.ToString(dt.Rows[0]["act_expectedDate"]);
                    //txtSeheduleEnd.Text = txtsend.Substring(0, 10) + " " + dt.Rows[0]["act_expectedTime"].ToString();
                    txtSeheduleEnd.Text = Convert.ToString(dt.Rows[0]["act_expecteddate"]);
                    lblShortNote.Text = Convert.ToString(dt.Rows[0]["act_instruction"]);
                    if (lblShortNote.Text == "")
                    {
                        DataTable dtNote = new DataTable();
                        dtNote = objBL.GetNoteInfo(id);
                        if (dtNote != null && dtNote.Rows.Count > 0)
                        {
                            lblShortNote.Text = Convert.ToString(dtNote.Rows[0]["act_instruction"]);
                        }
                        else { lilastnote.Visible = false; }

                    }

                }

            }
            catch
            {
                Session["btn"] = "a";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;

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



        public string getTotalNoOfCalls(string actid)
        {
            try
            {
                string str = "";
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "count(phc_id)", " phc_activityId='" + actid + "'");
                if (dt.Rows.Count != 0)
                {
                    str = Convert.ToString(dt.Rows[0][0]);
                }
                return str;
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return null;
            }
        }
        public void JavaScr()
        {
            try
            {
                //  string today = objConverter.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
                txtCallDispose.Attributes.Add("onclick", "calldispose('" + txtCallDispose.ID + "','phonecall')");
                //ASPxNextDate.Attributes.Add("onfocus", "displayCalendar('ctl00_ContentPlaceHolder3_ASPxNextDate','dd/mm/yyyy hh:ii',this,true,null,'0','300')");
                //Image1.Attributes.Add("OnClick", "displayCalendar('ctl00_ContentPlaceHolder3_ASPxNextDate','dd/mm/yyyy hh:ii',ctl00_ContentPlaceHolder3_ASPxNextDate,true,null,'157','300')");
                //ASPxNextDate.Attributes.Add("readonly", "true");
                //   ASPxNextDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ///  ASPxNextDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        public void LeadInformation(string phcid)
        {
            try
            {
                Session["btn"] = null;
                Session["SalesVisitID"] = null;
                Session["phonecallid"] = phcid;
                SetValueforLabels("N/A");
                string leadid = "";
                DataTable dt = new DataTable();
                string SelectBtn = "";
                if (Session["PhoneButton"] != null)
                {
                    SelectBtn = Convert.ToString(Session["PhoneButton"]);
                }
                else
                {
                    SelectBtn = "newcalls";
                }
                dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id", "tbl_master_calldispositions.call_dispositions, (convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) as phc_nextCall, (convert(varchar(11),tbl_trans_phonecall.phc_callDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_callDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20) , tbl_trans_phonecall.phc_callDate, 22), 3))) as phc_callDate,phc_leadcotactId,phc_note,(SELECT tbl_Master_DispositionCategory.Int_id FROM tbl_Master_DispositionCategory INNER JOIN tbl_master_calldispositions ON tbl_Master_DispositionCategory.Int_id = tbl_master_calldispositions.Call_Category WHERE (tbl_master_calldispositions.call_id = phc_callDispose)) as Category,phc_callDispose", " tbl_trans_phonecall.phc_id ='" + phcid + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0][2]) != "")
                    {
                        lblLastVisit.Text = Convert.ToString(dt.Rows[0][2]);
                    }
                    if (Convert.ToString(dt.Rows[0][0]) != "")
                    {
                        lblLastOutcome.Text = Convert.ToString(dt.Rows[0][0]);
                        string callDispose = Convert.ToString(dt.Rows[0]["phc_callDispose"]) + "|" + Convert.ToString(dt.Rows[0]["Category"]);
                    }
                    if (Convert.ToString(dt.Rows[0][1]) != "")
                    {
                        lblNextVisit.Text = Convert.ToString(dt.Rows[0][1]);
                    }
                    leadid = Convert.ToString(dt.Rows[0][3]);
                    if (lblLastVisit.Text == "" || lblLastVisit.Text == "N/A")
                    {

                        lastcallliid.Visible = false;
                    }
                }
                filldrpdownlistUserAddress();

            }
            catch
            {
                Session["btn"] = "a";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;

            }

        }
        public void filldrpdownlistUserAddress()
        {
            try
            {


                if (Session["phonecallid"] != null)
                {
                    string id = Convert.ToString(Session["phonecallid"]);
                    DataTable dt = new DataTable();
                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_address ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_address.add_cntId", "tbl_master_address.add_address1 as Address, tbl_master_address.add_id as Id", " tbl_trans_phonecall.phc_id='" + id + "'");
                    if (dt.Rows.Count != 0)
                    {
                        drpVisitPlace.DataTextField = "Address";
                        drpVisitPlace.DataValueField = "Id";
                        drpVisitPlace.DataSource = dt.DefaultView;
                        drpVisitPlace.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }
        }
        public void SetValueforLabels(string txt)
        {
            lblLastVisit.Text = txt;
            lblLastOutcome.Text = txt;
            lblNextVisit.Text = txt;
        }


        public void PnlGridStatus(bool b1, bool b2, bool b3, bool b4)
        {
            grdUserInfo.Visible = b1;
            PnlShowDetails.Visible = b2;
            pnlData.Visible = b3;
            pnlBtn.Visible = b4;
        }


        public void SetReminder(string note)
        {
            try
            {
                string Rem_Id = "";
                string[,] remid = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", " rem_sourceid='" + Convert.ToString(Session["phonecallid"]) + "'", 1);
                if (remid[0, 0] != "n")
                {
                    Rem_Id = remid[0, 0];
                }
                string enddate = Convert.ToString(ViewState["SDate"]);
                string endtime = Convert.ToDateTime(ASPxNextDate.Value).TimeOfDay.ToString();
                string new_endtime = Convert.ToDateTime(enddate.ToString()).AddDays(1).ToShortDateString();
                //string kk = Convert.ToDateTime(ASPxNextDate.Text.ToString()).AddDays(1).ToShortDateString(); 
                string new_endtime1 = Convert.ToDateTime(ViewState["STime"].ToString()).AddMinutes(-30).ToString();

                //string StartDate = ViewState["SDate"].ToString() + " " + ViewState["STime"].ToString();
                string[] aa = new_endtime1.Split(' ');
                new_endtime1 = Convert.ToString(aa[1]);
                string[] hh1 = aa[1].Split(':');
                if (Convert.ToString(aa[2]) == "PM" && Convert.ToString(hh1[0]) != "12")
                {
                    string[] hh_mm = new_endtime1.Split(':');
                    int hh = 12 + int.Parse(Convert.ToString(hh_mm.GetValue(0)));
                    new_endtime1 = hh + ":" + hh_mm[1];
                }
                string msg = "";
                msg = txtNotes.Text;
                if (msg == "")
                {
                    msg = "Blank Message";
                }
                if (Rem_Id == "")
                {

                    oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Convert.ToString(Session["userid"]) + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Convert.ToString(Session["userid"]) + "','" + enddate + " " + new_endtime1 + "','" + new_endtime + " " + endtime + "','" + note + "',1,0,'" + Convert.ToString(Session["phonecallid"]) + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'");
                }
                else
                {

                    oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_createUser='" + Convert.ToString(Session["userid"]) + "',rem_createDate='" + oDBEngine.GetDate().ToShortDateString() + "',rem_targetUser='" + Convert.ToString(Session["userid"]) + "',rem_startDate='" + enddate + " " + new_endtime1 + "',rem_endDate='" + new_endtime + " " + endtime + "',rem_reminderContent='" + note + "',rem_displayTricker=1,rem_actionTaken=0,rem_sourceid='" + Convert.ToString(Session["phonecallid"]) + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " rem_id='" + Rem_Id + "'");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }




        public void color()
        {
            if (Session["color"] != null)
            {
                if (Convert.ToString(Session["color"]) == "a")
                {
                    BtnCalls.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "b")
                {
                    BtnBack.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "c")
                {
                    BtnContact.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "d")
                {
                    BtnUsable.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "e")
                {
                    BtnWon.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "f")
                {
                    BtnLost.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "g")
                {
                    BtnPipeline.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "h")
                {
                    BtnCourtesy.ForeColor = System.Drawing.Color.Blue;
                }
                if (Convert.ToString(Session["color"]) == "i")
                {
                    BtnForward.ForeColor = System.Drawing.Color.Blue;
                }
            }
        }

        #region Button Function
        protected void BtnCalls_Click(object sender, EventArgs e)
        {

            Session["PhoneButton"] = "newcalls";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            BtnCalls.ForeColor = System.Drawing.Color.Blue;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "a";
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnBack_Click(object sender, EventArgs e)
        {

            Session["PhoneButton"] = "callback";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);

            //BtnBack.Attributes.Remove("class", "btn-primary");
            // BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Blue;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            //Session["color"] = "b";
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnContact_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "notinterested";
            FillGridUserInfo();
            //PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            BtnContact.ForeColor = System.Drawing.Color.White;
            BtnContact.BackColor = System.Drawing.Color.Green;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "c";
            //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnUsable_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "outofservice";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            BtnUsable.ForeColor = System.Drawing.Color.Blue;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "d";
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnWon_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "noresponse";
            FillGridUserInfo();
            //PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            BtnWon.ForeColor = System.Drawing.Color.Blue;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "e";
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnLost_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "interested";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            BtnLost.ForeColor = System.Drawing.Color.Blue;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "f";
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnPipeline_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "requestsalesvisit";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            BtnPipeline.ForeColor = System.Drawing.Color.Blue;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            //BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "g";
            //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnCourtesy_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "courtesycalls";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            BtnCourtesy.ForeColor = System.Drawing.Color.Blue;
            // BtnForward.ForeColor = System.Drawing.Color.Black;
            Session["color"] = "h";
            //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        protected void BtnForward_Click(object sender, EventArgs e)
        {
            Session["PhoneButton"] = "forwardCalls";
            FillGridUserInfo();
            PnlGridStatus(true, false, false, true);
            //BtnCalls.ForeColor = System.Drawing.Color.Black;
            //BtnBack.ForeColor = System.Drawing.Color.Black;
            //BtnContact.ForeColor = System.Drawing.Color.Black;
            //BtnUsable.ForeColor = System.Drawing.Color.Black;
            //BtnWon.ForeColor = System.Drawing.Color.Black;
            //BtnLost.ForeColor = System.Drawing.Color.Black;
            //BtnPipeline.ForeColor = System.Drawing.Color.Black;
            //BtnCourtesy.ForeColor = System.Drawing.Color.Black;
            BtnForward.ForeColor = System.Drawing.Color.Blue;
            Session["color"] = "i";
            //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }

        protected void BtnCall_Click(object sender, EventArgs e)
        {
            string date = oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss");
            ViewState["callstart"] = date.ToString();
            //TxtNext.Visible = false;
            string id = "";
            int k = grdUserInfo.Rows.Count;
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");

                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = Convert.ToString(lbl.Text);

                }
            }
            if (id != "")
            {
                ShowCallDetails(id);
                LeadInformation(id);

                pnlFButton.Visible = false;

                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(false, true, true, false);
                }
                //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>iframesource();height();</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }
            TotalNoOfCalls.Visible = false;
        }
        protected void BtnService_Click(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");
                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = Convert.ToString(lbl.Text);
                }
            }
            if (id != "")
            {
                chkStage.Checked = false;
                txtCallDispose.Text = "";
                txtNotes.Text = "";
                string nextcall = objConverter.DateConverterFromMMtoDD(oDBEngine.GetDate().AddHours(48).ToString(), "mm/dd/yyyy hh:mm");
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + oDBEngine.GetDate().ToString() + "',phc_lastCallDuration='0', phc_callDispose=6,phc_nextCall='" + nextcall + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + id + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall,CreateDate,CreateUser", "'" + id + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "',6,0,'','" + nextcall + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'");
                oDBEngine.messageTableUpdate(Convert.ToString(Session["userid"]), Convert.ToString(Session["userid"]), "Phone Calls", oDBEngine.GetDate().ToString(), nextcall, txtPriority.Text, txtNotes.Text, "0", "message");
                ShowCallDetails(id);
                FillGridUserInfo();
                DataTable dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0][1]) == "")
                        {
                            DataTable temp = new DataTable();
                            temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId ='" + Convert.ToString(Session["newactivityid"]) + "'");
                            if (temp.Rows.Count != 0)
                            {
                                if (Convert.ToString(temp.Rows[0][0]) == "0")
                                {
                                    oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                }
                            }
                        }
                    }
                }
                grdUserInfo.Visible = true;
                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(true, false, false, true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }

        }
        protected void BtnReachable_Click(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");
                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = Convert.ToString(lbl.Text);
                }
            }
            if (id != "")
            {
                chkStage.Checked = false;
                txtCallDispose.Text = "";
                txtNotes.Text = "";
                string nextcall = "";
                string Currenttime = oDBEngine.GetDate().ToShortTimeString();
                string AM_PM = Currenttime.Substring(5);
                if (AM_PM == "AM")
                {
                    nextcall = objConverter.DateConverterFromMMtoDD(oDBEngine.GetDate().AddHours(2).ToString(), "mm/dd/yyyy hh:mm");
                }
                else
                {
                    string CTime = Currenttime.Substring(0, 1);
                    if (Convert.ToInt32(CTime) > 4)
                    {
                        string NHour = "10:30 AM";
                        string NDate = oDBEngine.GetDate().AddDays(1).ToShortDateString();
                        nextcall = objConverter.DateConverterFromMMtoDD(NDate + " " + NHour, "mm/dd/yyyy hh:mm");
                    }
                    else
                    {
                        nextcall = objConverter.DateConverterFromMMtoDD(oDBEngine.GetDate().AddHours(2).ToString(), "mm/dd/yyyy hh:mm");
                    }
                }
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + oDBEngine.GetDate().ToString() + "',phc_lastCallDuration='0', phc_callDispose=2,phc_nextCall='" + nextcall + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + id + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall,CreateDate,CreateUser", "'" + id + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "',2,0,'','" + nextcall + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'");
                oDBEngine.messageTableUpdate(Convert.ToString(Session["userid"]), Convert.ToString(Session["userid"]), "Phone Calls", oDBEngine.GetDate().ToString(), nextcall, txtPriority.Text, txtNotes.Text, "0", "message");
                ShowCallDetails(id);
                FillGridUserInfo();
                LeadInformation(id);
                DataTable dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                        //oDBEngine.messageTableUpdate();
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0][1]) == "")
                        {
                            DataTable temp = new DataTable();
                            temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId ='" + Convert.ToString(Session["newactivityid"]) + "'");
                            if (temp.Rows.Count != 0)
                            {
                                if (Convert.ToString(temp.Rows[0][0]) == "0")
                                {
                                    oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                }
                            }
                        }
                    }
                }
                grdUserInfo.Visible = true;
                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(true, false, false, true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }
        }
        protected void BtnResponse_Click(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");
                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = Convert.ToString(lbl.Text);
                }
            }
            if (id != "")
            {
                chkStage.Checked = false;
                txtCallDispose.Text = "";
                txtNotes.Text = "";
                string nextcall = "";
                string Currenttime = oDBEngine.GetDate().ToShortTimeString();
                string AM_PM = Currenttime.Substring(5);
                if (AM_PM == "AM")
                {
                    nextcall = objConverter.DateConverterFromMMtoDD(oDBEngine.GetDate().AddHours(1).ToString(), "mm/dd/yyyy hh:mm");
                }
                else
                {
                    string CTime = Currenttime.Substring(0, 1);
                    if (Convert.ToInt32(CTime) > 5)
                    {
                        string NHour = "10:30 AM";
                        string NDate = oDBEngine.GetDate().AddDays(1).ToShortDateString();
                        nextcall = objConverter.DateConverterFromMMtoDD(NDate + " " + NHour, "mm/dd/yyyy hh:mm");
                    }
                    else
                    {
                        nextcall = objConverter.DateConverterFromMMtoDD(oDBEngine.GetDate().AddHours(1).ToString(), "mm/dd/yyyy hh:mm");
                    }
                }
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + oDBEngine.GetDate().ToString() + "',phc_lastCallDuration='0', phc_callDispose=3,phc_nextCall='" + nextcall + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + id + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall,CreateDate,CreateUser", "'" + id + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "',3,0,'','" + nextcall + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'");
                oDBEngine.messageTableUpdate(Convert.ToString(Session["userid"]), Convert.ToString(Session["userid"]), "Phone Calls", oDBEngine.GetDate().ToString(), nextcall, txtPriority.Text, txtNotes.Text, "0", "message");
                ShowCallDetails(id);
                FillGridUserInfo();
                LeadInformation(id);
                DataTable dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                        //oDBEngine.messageTableUpdate();
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0][1]) == "")
                        {
                            DataTable temp = new DataTable();
                            temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId ='" + Convert.ToString(Session["newactivityid"]) + "'");
                            if (temp.Rows.Count != 0)
                            {
                                if (Convert.ToString(temp.Rows[0][0]) == "0")
                                {
                                    oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                }
                            }
                        }
                    }
                }
                grdUserInfo.Visible = true;
                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(true, false, false, true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }
        }
        protected void BtnExist_Click(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");
                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = Convert.ToString(lbl.Text);
                }
            }
            if (id != "")
            {
                chkStage.Checked = false;
                txtCallDispose.Text = "";
                txtNotes.Text = "";
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + oDBEngine.GetDate().ToString() + "',phc_lastCallDuration='0', phc_callDispose=3,phc_nextCall='',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + id + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall,CreateDate,CreateUser", "'" + id + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.GetDate().ToString() + "',3,0,'','','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(Session["userid"]) + "'");

                FillGridUserInfo();
                LeadInformation(id);
                grdUserInfo.Visible = true;
                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(true, false, false, true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }
        }
        protected void btnSavePhoneCallDetails_Click(object sender, EventArgs e)
        {
            try
            {
                //string str = objConverter.DateConverter(ASPxNextDate.Text, "mm/dd/yyyy hh:mm");
                //DataTable dt = new DataTable();
                //dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId", "top " + currentItemNumber + " * from (select top " + ctotalItem + " tbl_trans_phonecall.phc_leadcotactId, tbl_trans_phonecall.phc_id,(convert(varchar(11),tbl_trans_phonecall.phc_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecall.phc_nextCall, 22), 3))) AS NextCall,(SELECT call_dispositions FROM tbl_master_calldispositions WHERE call_id = tbl_trans_phonecall.phc_callDispose) AS CallDispose", " (tbl_trans_Activies.act_assignedTo = " + contactId + ") AND (tbl_trans_phonecall.phc_callDispose IN (SELECT call_id FROM  tbl_master_calldispositions WHERE Call_Category = 1)) AND ((phc_forwardcall IS NULL) OR (tbl_trans_phonecall.lastmodifyuser =" + Convert.ToString(Session["userid"]) + ")) order by CONVERT(datetime, tbl_trans_phonecall.phc_nextCall, 101)) DERIVEDTBL ORDER BY CONVERT(datetime, NextCall, 101) DESC");

                var curr = DateTime.Now;
                String s = curr.ToString("HH:mm:ss");
                DateTime date = Convert.ToDateTime(ASPxNextDate.Value);

                if (Convert.ToDateTime(curr).ToString("yyyy-MM-dd") == Convert.ToDateTime(date).ToString("yyyy-MM-dd") && (Request.QueryString["Pid"] == "3"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "SameDate();", true);

                }


                DateTime time = Convert.ToDateTime(s);
                DateTime dtCOMPLTDTTM = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

                string str = Convert.ToDateTime(dtCOMPLTDTTM).ToString("yyyy-MM-dd HH:mm:ss");
                string calldispose1 = TxtOut.Text;
                int i = calldispose1.LastIndexOf("|");
                string calldispose = calldispose1.Substring(0, i);
                string calldispose_cat = calldispose1.Substring(i + 1);
                string branchid = "";
                if (HttpContext.Current.Session["userbranchID"] != null)
                {
                    branchid = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                }
                else
                {
                    branchid = "0";
                }
                string calldate = oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss");
                string CallStart = Convert.ToString(ViewState["callstart"]);
                string callend = oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss");
                ViewState["callEnd"] = Convert.ToString(callend);
                string call = Convert.ToString(ViewState["callEnd"]);
                DateTime CallStart1 = Convert.ToDateTime(CallStart);
                DateTime call1 = Convert.ToDateTime(call);
                TimeSpan span = call1.Subtract(CallStart1);
                string Callduration = Convert.ToString(span.Seconds);
                bool flag;
                string phonecallid = "";
                if (Session["phonecallid"] != null)
                {
                    phonecallid = Convert.ToString(Session["phonecallid"]);
                }
                else
                {
                    phonecallid = "0";
                }


               /////06-09-2018  Sudip Pal
                //flag = Convert.ToBoolean(oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId,phd_branchId,phd_callDate,phd_callStart,phd_callEnd,phd_callDispose,phd_note,phd_nextCall,phd_callduration,CreateDate,CreateUser,slv_nextActivityType,slv_ActivityType", "'" + phonecallid + "','" + branchid + "','" + calldate + "','" + CallStart + "','" + call + "','" + calldispose + "','" + txtNotes.Text + "','" + str + "','" + Callduration + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToString(Session["userid"]) + "'"));
              
                string fromtype = Convert.ToString(Request.QueryString["Pid"]);
                if(fromtype=="2")
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
                string nexttype = Convert.ToString(RdActivityList.SelectedValue);
                flag = Convert.ToBoolean(oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId,phd_branchId,phd_callDate,phd_callStart,phd_callEnd,phd_callDispose,phd_note,phd_nextCall,phd_callduration,CreateDate,CreateUser,slv_nextActivityType,slv_ActivityType", "'" + phonecallid + "','" + branchid + "','" + calldate + "','" + CallStart + "','" + call + "','" + calldispose + "','" + txtNotes.Text + "','" + str + "','" + Callduration + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToString(Session["userid"]) + "','" + nexttype + "','"+fromtype+"'"));

                /////06-09-2018  Sudip Pal


                if (flag)
                {

                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
                    obl.UpdateNextActivityDate(Convert.ToString(Request.QueryString["TransSale"]), str);

                    flag = Convert.ToBoolean(oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + calldate + "',phc_callDispose='" + calldispose + "',phc_nextCall='" + str + "',phc_note='" + txtNotes.Text + "',phc_lastCallDuration='" + Callduration + "',phc_addId='" + Convert.ToString(drpVisitPlace.SelectedValue) + "',LastModifyDate='" + oDBEngine.GetDate().ToString("yyyy-MM-dd") + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + Convert.ToString(Session["phonecallid"]) + "'"));
                    // Code Added by sam on 17012017 to hide permission icon after phone follow up selection
                    int transsalesid = 0;
                    if (Request.QueryString["TransSale"] != null)
                    {
                        transsalesid = Convert.ToInt32(Request.QueryString["TransSale"]);
                    }
                    oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextactivitystatus=1", "sls_id=" + transsalesid);
                    DataTable dt = new DataTable();
                    oDBEngine.messageTableUpdate(Convert.ToString(Session["phonecallid"]), Convert.ToString(Session["userid"]), "Phone Calls", oDBEngine.GetDate().ToString(), str, txtPriority.Text, txtNotes.Text, "0", "message");
                    dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToString(dt.Rows[0][0]) == "")
                        {
                            oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                            //oDBEngine.messageTableUpdate();
                        }
                        else
                        {
                            if (Convert.ToString(dt.Rows[0][1]) == "")
                            {
                                DataTable temp = new DataTable();
                                temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                if (temp.Rows.Count != 0)
                                {
                                    if (temp.Rows[0][0].ToString() == "0")
                                    {
                                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                    }
                                }
                            }
                        }
                    }
                }
                string Cid1 = Convert.ToString(Session["contactInternalId"]);
                string Cid = Cid1.Substring(0, 2);
                if (calldispose_cat == "4" || calldispose_cat == "1")
                {
                    //if (Cid == "LD")
                    //{
                    //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_Lead_Stage=2,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                    //}
                    //else
                    //{
                    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Lead_Stage=2,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                    //}
                }
                else
                    if (calldispose_cat == "5")
                    {
                        //if (Cid == "LD")
                        //{
                        //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_Lead_Stage=4,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                        //}
                        //else
                        //{
                        oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Lead_Stage=4,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                        //}
                    }
                    else
                    {
                        if (chkStage.Checked == true)
                        {
                            //if (Cid == "LD")
                            //{
                            //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_Lead_Stage=5,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                            //}
                            //else
                            //{
                            oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Lead_Stage=5,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                            //}
                        }
                    }
                if (calldispose_cat == "4" || calldispose_cat == "5")
                {
                    string val = "";
                    string[,] val1 = oDBEngine.GetFieldValue("tbl_master_address", "add_id", " add_cntId='" + Convert.ToString(ViewState["PhcLeadContactId"]) + "'", 1);
                    if (val1[0, 0] != "n")
                    {
                        val = val1[0, 0];
                    }
                    DataTable address = new DataTable();
                    string id123 = "0";
                    address = oDBEngine.GetDataTable("tbl_master_address", "*", " add_cntId='" + Convert.ToString(ViewState["PhcLeadContactId"]) + "'");
                    bool fi = false;
                    try
                    {
                        for (int ij = 0; ij < address.Rows.Count; ij++)
                        {
                            id123 = Convert.ToString(address.Rows[ij]["add_id"]);
                            if (val == Convert.ToString(address.Rows[ij]["add_id"]))
                            {
                                fi = true;
                                oDBEngine.SetFieldValue("tbl_master_address", "add_activityId='" + Convert.ToString(Session["phonecallid"]) + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " add_id='" + id123 + "'");
                            }
                            else
                            {
                                oDBEngine.SetFieldValue("tbl_master_address", "add_activityId=NULL,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " add_id='" + Convert.ToString(address.Rows[i]["add_id"]) + "'");
                            }
                        }
                        if (fi == false)
                        {
                            oDBEngine.SetFieldValue("tbl_master_address", "add_activityId='" + Convert.ToString(Session["phonecallid"]) + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " add_id='" + id123 + "'");
                        }
                    }
                    catch
                    {
                        oDBEngine.SetFieldValue("tbl_master_address", "add_activityId='" + Convert.ToString(Session["phonecallid"]) + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " add_id='" + id123 + "'");
                    }
                }
                string Phone = "";
                string LeadName = "";
                DataTable temp1 = new DataTable();
                string LCID1 = Convert.ToString(Session["LeadId"]);
                string LCID = LCID1.Substring(0, 2);
                //if (LCID == "LD")
                //{
                //    temp1 = oDBEngine.GetDataTable("tbl_master_lead INNER JOIN tbl_master_phonefax ON tbl_master_lead.cnt_internalId = tbl_master_phonefax.phf_cntId", "tbl_master_lead.cnt_firstName, tbl_master_phonefax.phf_phoneNumber", "tbl_master_lead.cnt_internalId ='" + Convert.ToString(Session["LeadId"]) + "'");
                //}
                //else
                //{
                temp1 = oDBEngine.GetDataTable("tbl_master_contact INNER JOIN tbl_master_phonefax ON tbl_master_contact.cnt_internalId = tbl_master_phonefax.phf_cntId", "tbl_master_contact.cnt_firstName, tbl_master_phonefax.phf_phoneNumber", "tbl_master_contact.cnt_internalId ='" + Convert.ToString(Session["LeadId"]) + "'");
                //}
                for (int j = 0; j < temp1.Rows.Count; j++)
                {
                    Phone = Convert.ToString(temp1.Rows[j]["phf_phoneNumber"]);
                    LeadName = Convert.ToString(temp1.Rows[j]["cnt_firstName"]);
                }
                try
                {
                    string sStartdate = str;
                    //string sStartdate = oDBEngine.GetDate().ToShortDateString();
                    ViewState["SDate"] = Convert.ToDateTime(sStartdate).ToShortDateString();
                    string sStartTime = Convert.ToDateTime(sStartdate).ToShortTimeString();
                    ViewState["STime"] = sStartTime;
                    if (calldispose_cat == "1")
                    {
                        SetReminder("Call Back " + LeadName + " on " + Phone + " AT " + objConverter.DateConverter_d_m_y(Convert.ToDateTime(sStartdate).ToShortDateString()) + " " + sStartTime + "[" + txtNotes.Text + "]");
                    }
                    if (calldispose_cat == "2")
                    {
                        SetReminder("Call Back " + LeadName + " on " + Phone + " AT " + objConverter.DateConverter_d_m_y(Convert.ToDateTime(sStartdate).ToShortDateString()) + " " + sStartTime + "[" + txtNotes.Text + "]");
                    }
                    if (calldispose_cat == "4")
                    {
                        SetReminder("Meeting With " + LeadName + " on " + Phone + " AT " + objConverter.DateConverter_d_m_y(Convert.ToDateTime(sStartdate).ToShortDateString()) + " " + sStartTime + "[" + txtNotes.Text + "]");
                    }
                    if (calldispose_cat == "5")
                    {
                        SetReminder("Meeting With " + LeadName + " on " + Phone + " AT " + objConverter.DateConverter_d_m_y(Convert.ToDateTime(sStartdate).ToShortDateString()) + " " + sStartTime + "[" + txtNotes.Text + "]");
                    }
                }
                catch
                { }
                pnlFButton.Visible = true;
                try
                {
                    oDBEngine.SystemGeneratedMails(Convert.ToString(Session["newactivityid"]), "Phone Calls");
                }
                catch
                {
                }
                if (Session["mode"] != null)
                {
                    string ContactID1 = Convert.ToString(Session["contactInternalId"]);
                    string ContactID = ContactID1.Substring(0, 2);
                    //if (ContactID == "LD")
                    //{
                    //    oDBEngine.SetFieldValue("tbl_master_lead", "cnt_rating='" + Convert.ToString(Session["mode"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                    //}
                    //else
                    //{
                    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_rating='" + Convert.ToString(Session["mode"]) + "'", " cnt_internalId='" + Convert.ToString(Session["contactInternalId"]) + "'");
                    //}
                }

                if (RdActivityList.SelectedValue != "0")
                {
                    oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextvisitdate='" + str + "',sls_sales_status='" + RdActivityList.SelectedValue + "',sls_dateTime='" + oDBEngine.GetDate().ToString() + "', LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " sls_id='" + Request.QueryString["TransSale"].ToString() + "'");
                }
                // .............................Code Commented and Added by Sam on 29122016. ..................................... 

                //Response.Redirect("CRMPhoneCallWithFrame.aspx");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "jAlert('Saved sucessfully');window.location ='Activities/crm_sales.aspx';", true);

                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Saved sucessfully.','Msg',function(){ window.location='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "OMS/Management/Activities/crm_sales.aspx';   });", true);


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Saved sucessfully.','Sales Activity',function(){ window.location='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "OMS/Management/Activities/crm_sales.aspx';   });", true);
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpRedirect('/OMS/Management/Activities/" + pagename + "','" + RdActivityList.SelectedValue + "');", true);



                ///  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);


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
                    dt_CallDisposeDetails = objemployeebal.GetEmailAccountConfigDetails(calldispose, 7);

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
                        replacements.Add("<%PhoneStatus%>", Convert.ToString(dt_CallDisposeDetails.Rows[0].Field<string>("call_dispositions")));
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
                        int MailStatus;
                        //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName, 5);
                        MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(RecvEmail, "", replacements, dt_EmailConfig, ActvName, 7);

                        if (MailStatus == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Mail Send');", true);
                            return;
                        }
                        else if (MailStatus == -1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Mail Not Send.Please try again');", true);
                            return;
                        }
                    }


                }

                #endregion
                //End


                //Response.Redirect("Activities/crm_sales.aspx", false);
                // .............................Code Above Commented and Added by Sam on 29122016...................................... 
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                return;
            }

        }
        protected void BtnExist_Click1(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gvrow in grdUserInfo.Rows)
            {
                Label lbl = (Label)grdUserInfo.Rows[gvrow.RowIndex].FindControl("lblId");
                CheckBox chk = (CheckBox)grdUserInfo.Rows[gvrow.RowIndex].FindControl("chkSel");
                if (chk.Checked == true)
                {
                    id = lbl.Text.ToString();
                }
            }
            if (id != "")
            {
                chkStage.Checked = false;
                txtCallDispose.Text = "";
                txtNotes.Text = "";
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDate='" + oDBEngine.GetDate().ToString() + "',phc_lastCallDuration='0', phc_callDispose=5,phc_nextCall='',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " phc_id='" + id + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall,CreateDate,CreateUser", "'" + id + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd") + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd") + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd") + "',5,0,'','','" + oDBEngine.GetDate().ToString("yyyy-MM-dd") + "','" + Convert.ToString(Session["userid"]) + "'");
                ShowCallDetails(id);
                FillGridUserInfo();
                LeadInformation(id);
                DataTable dt = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate,act_actualEndDate", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id='" + Convert.ToString(Session["newactivityid"]) + "'");
                        //oDBEngine.messageTableUpdate();
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0][1]) == "")
                        {
                            DataTable temp = new DataTable();
                            temp = oDBEngine.GetDataTable("tbl_trans_phonecall", "COUNT(phc_id)", " phc_callDate IS NULL AND phc_activityId ='" + Convert.ToString(Session["newactivityid"]) + "'");
                            if (temp.Rows.Count != 0)
                            {
                                if (Convert.ToString(temp.Rows[0][0]) == "0")
                                {
                                    oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualEndDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " act_id ='" + Convert.ToString(Session["newactivityid"]) + "'");
                                }
                            }
                        }
                    }
                }
                grdUserInfo.Visible = true;
                if (Session["btn"] != null)
                {
                    PnlGridStatus(true, false, false, true);
                }
                else
                {
                    PnlGridStatus(true, false, false, true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script>alert('You Have to Select One Call List')</script>");
            }
        }
        protected void btnSCancel_Click(object sender, EventArgs e)
        {
            pnlFButton.Visible = true;
            Response.Redirect("Activities/crm_sales.aspx", false);
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");


        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string test = Convert.ToString(Session["contactInternalId"]);
                Page.ClientScript.RegisterStartupScript(GetType(), "Test", "<script language='JavaScript'>CallEmail1('" + test + "')</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='jscript1'>iframesource();height();</script>");

                //ds = oDBEngine.PopulateData("tbl_master_calldispositions.call_dispositions, tbl_trans_phonecall.phc_nextCall, tbl_trans_phonecall.phc_callDate,phc_leadcotactId,phc_activityId", "tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id", " tbl_trans_phonecall.phc_id = '" + Session["contactInternalId"].ToString() + "'");

                //string email_id = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_cntId='" + test + "'", 1)[0, 0];
                ////oDBEngine.SystemGeneratedMails(Session["newactivityid"].ToString(), "Phone Calls");
                ////if (email_id != "n")
                ////{
                //    string body = "<table><tr><td> hi </td></tr></table>";
                //    objConverter.SendMail("NoResponse@gmail.com", "indira.bhatta@gmail.com", "Phone Call", body);
                //}
            }
            catch
            {
            }
        }

        protected void btnPhoneFollowUP_Click(object sender, EventArgs e)
        {

        }



        #endregion Button Function

        // .............................Code Commented and Added by Sam on 29122016.to Bind Call disposition list ..................................... 

        [WebMethod]
        public static List<string> GetCallDispositionList()
        {
            try
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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
                dtCallDisposition = objSlaesActivitiesBL.PopulateCallDispositionDetailForPhoneCallOfSalesActivities();
                List<string> obj = new List<string>();

                foreach (DataRow dr in dtCallDisposition.Rows)
                {
                    obj.Add(Convert.ToString(dr["call_dispositions"]) + "|" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "," + SelectedAddTime + "," + oDBEngine.GetDate().ToShortDateString().ToString() + "," + SelectedNowTime + "!" + Convert.ToString(dr["Id"]));
                    //obj.Add(Convert.ToString(dr["call_dispositions"]) + "|" + Convert.ToString(dr["Id"]));
                }

                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        // .............................Code Above Commented and Added by Sam on 29122016...................................... 



        #region Completed Section

        #region Page Event Section
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #endregion Page Event Section

        #endregion Completed Section


        #region Trash Code

        #region Paging Section
        protected void TxtNext_Click(object sender, EventArgs e)
        {
            if (ViewState["Cpage"] == null)
            {
                ViewState["Cpage"] = 1;
            }
            else
            {
                ViewState["Cpage"] = Convert.ToInt32(ViewState["Cpage"]) + 1;
            }
            //ViewState["call"] = "a";
            FillGridUserInfo();
        }
        protected void TxtPrevious_Click(object sender, EventArgs e)
        {
            int cpage = Convert.ToInt32(ViewState["Cpage"]);
            ViewState["Cpage"] = cpage - 1;
            // ViewState["call"] = "a";
            FillGridUserInfo();
        }

        #endregion Paging Section

        protected void grdUserInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkSel");
                Label lbl = (Label)e.Row.FindControl("lblId");
                chkBox.Attributes.Add("onclick", "javascript:chkGenral(this,'" + lbl.Text + "')");
            }
        }

        protected void grdUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUserInfo.PageIndex = e.NewPageIndex;
            FillGridUserInfo();
        }
        protected void grdUserInfo_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;    
        }
        protected void grdForwardCall_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        #endregion Trash Code


        #region Product Class and Industry Bind  customer

        public void bindClassandCustomer()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TransSale"]) && !string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("GetproductCellsandCustomer");
                proc.AddVarcharPara("@SalesId", 500, Request.QueryString["TransSale"]);
                proc.AddVarcharPara("@TypeId", 500, Request.QueryString["type"]);
                ds = proc.GetTable();

                if (ds != null && ds.Rows.Count > 0)
                {
                    lblcustomer.Text = Convert.ToString(ds.Rows[0]["CustomerName"]);
                    lblproductclass.Text = Convert.ToString(ds.Rows[0]["ProductClass"]);
                    if (lblproductclass.Text == "")
                    {

                        liproductclass.Visible = false;
                    }
                }
            }

        }

        #endregion

        #region Sale Quotation and Sales Order
        protected void btnSaleQuotation_Click(object sender, EventArgs e)
        {
            int SalesId = Int32.Parse(Request.QueryString["TransSale"]);
            int TypeId = Int32.Parse(Request.QueryString["type"]);
            pnlFButton.Visible = true;

            string url = "Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
            string s = "window.open('" + url + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            //Response.Redirect("Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId, false);
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");
        }


        //protected void btnSaleOrder_Click(object sender, EventArgs e)
        //{
        //    int SalesId = Int32.Parse(Request.QueryString["TransSale"]);
        //    int TypeId = Int32.Parse(Request.QueryString["type"]);
        //    pnlFButton.Visible = true;
        //   // Response.Redirect("Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId, false);
        //    string url = "Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
        //    string s = "window.open('" + url + "');";
        //    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //    //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>height();</script>");

        //}
        #endregion

    }

}