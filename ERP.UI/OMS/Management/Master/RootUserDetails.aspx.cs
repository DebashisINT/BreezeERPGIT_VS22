using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Drawing;
using System.Web.Services;
using System.Collections.Generic;
using UtilityLayer;
using DataAccessLayer;
using Newtonsoft.Json;
using System.Net.Http;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_RootUserDetails : ERP.OMS.ViewState_class.VSPage
    {
        MasterSettings masterbl = new MasterSettings();
        string Id;
        int CreateUser;
        DateTime CreateDate;

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        CommonBL CBL = new CommonBL();
        public string pageAccess = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string SyncUsertoWhileCreating = CBL.GetSystemSettingsResult("SyncUsertoWhileCreating");
            hdnSyncUsertoWhileCreating.Value = SyncUsertoWhileCreating;

            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            hdnServicemanagement.Value = mastersettings;
            if (mastersettings == "0")
            {
                divServiceManagemnet.Style.Add("display", "none");
            }
            else
            {
                divServiceManagemnet.Style.Add("display", "!inline-block");
            }

            //add rev for STB Management Tanmoy
            string STBManagementmastersettings = masterbl.GetSettings("IsSTBManagementRequired");
            hdnSTBmanagement.Value = STBManagementmastersettings;
            if (STBManagementmastersettings == "0")
            {
                divSTBManagemnet.Style.Add("display", "none");
            }
            else
            {
                divSTBManagemnet.Style.Add("display", "!inline-block");
            }
            //add rev for STB Management Tanmoy

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            Id = Request.QueryString["id"];
            hdnEntryMode.Value = Id;
            ActionMode = Request.QueryString["id"];
            CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (!IsPostBack)
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_user.aspx");
                Session["addedituser"] = "yes";
                BindUserGroups();
                FillComboBranch();
                Fillgridview();


                hdnEmailMadatory.Value = Convert.ToString(oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='LoinIdEmailMandatory'").Rows[0]["Variable_Value"]);
                //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
                //if (Id != "Add")
                if (Id != "Add" && Request.QueryString["key"]!="Copy")
                //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
                {
                    if (!rights.CanEdit)
                    {
                        Response.Redirect("/OMS/Management/master/root_user.aspx");
                    }
                    ShowData(Id);
                    txtusername.Enabled = false;
                }
                //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
                if (Id != "Add" && Request.QueryString["key"] == "Copy")                
                {
                    if (!rights.CanAdd)
                    {
                        Response.Redirect("/OMS/Management/master/root_user.aspx");
                    }
                    ShowData(Id);
                   
                }
                else
                {
                    if (!rights.CanAdd)
                    {
                        Response.Redirect("/OMS/Management/master/root_user.aspx");
                    }
                }

                if (Convert.ToString(HttpContext.Current.Session["superuser"]).Trim() == "Y")
                {
                    cbSuperUser.Visible = true;
                }
                else
                {
                    cbSuperUser.Visible = false;
                }


            }
            /*--Set Page Accesss--*/
            string pageAccess = oDBEngine.CheckPageAccessebility("root_user.aspx");
            Session["PageAccess"] = pageAccess;
            //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
            if (Request.QueryString["key"]=="Copy")
            {
                ActionMode = Request.QueryString["key"].ToString();
            }
            //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
        }

        private void BindUserGroups()
        {
            ddlGroups.Items.Clear();

            DataTable dt = new BusinessLogicLayer.UserGroupsBLS.UserGroupBL().FetchAllGroupsDataTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlGroups.DataSource = dt;
                ddlGroups.DataTextField = "grp_name";
                ddlGroups.DataValueField = "grp_id";
                ddlGroups.DataBind();
            }

            ddlGroups.Items.Insert(0, "Select Group");
        }

        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> ALLEmployee(string reqStr)
        {

            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            UserBL objUserBL = new UserBL();
            //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
            //if (ActionMode == "Add")
            if (ActionMode == "Add")
            {
                DT = objUserBL.PopulateAssociatedEmployee(0, "Add");
            }
            //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
            else if(ActionMode=="Copy")
            {
                DT = objUserBL.PopulateAssociatedEmployee(0, "Add");
            }
            //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
            else
            {
                DT = objUserBL.PopulateAssociatedEmployee(Convert.ToInt32(ActionMode), "Edit");
            }
            if (DT.Rows.Count > 0)
            {
                List<string> obj = new List<string>();
                foreach (DataRow dr in DT.Rows)
                {
                    obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
                }

                return obj;
            }
            else
            {
                return null;
            }

        }

        protected void FillComboBranch()
        {
            string[,] DataBranch = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description", null, 2);
            oclsDropDownList.AddDataToDropDownList(DataBranch, dropdownlistbranch);
        }
        protected void Fillgridview()
        {
            //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataSet dsDocument = new DataSet();
            dsDocument = oDBEngine.PopulateData("seg_id as Id, seg_name as SegmentName", "tbl_master_segment", null);
            if (dsDocument.Tables["TableName"].Rows.Count > 0)
            {
                grdUserAccess.DataSource = dsDocument.Tables["TableName"];
                grdUserAccess.DataBind();
            }

        }
        protected void grdUserAccess_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DropDownList drpUserGroup = (DropDownList)e.Row.FindControl("drpUserGroup");
                Label lbl = (Label)e.Row.FindControl("lblId");
                string[,] DatadropDown = oDBEngine.GetFieldValue("tbl_master_userGroup", "grp_id, grp_name", "grp_segmentId='" + lbl.Text + "'", 2, "grp_name");
                string checkId1 = DatadropDown[0, 0];
                if (checkId1 != "n")
                {
                    //oDBEngine.AddDataToDropDownList(DatadropDown, drpUserGroup);
                    oclsDropDownList.AddDataToDropDownList(DatadropDown, drpUserGroup);
                }
            }
        }
        protected void ShowData(string Id)
        {
            //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
            //user_password.Visible = false;
            if(Request.QueryString["key"]=="Copy")
            {
                user_password.Visible = true;
            }
            else
            {
                user_password.Visible = false;
            }            
            //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
            Int16 userId = Convert.ToInt16(Id);
            DataSet dsUserDetail = new DataSet();
            //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
            //dsUserDetail = oDBEngine.PopulateData("u.user_name as user1 , u.user_loginId as Login,u.user_branchId as Branchid,u.user_group as usergroup,u.user_AllowAccessIP,u.user_contactId as ContactId, c.cnt_firstName + ' ' +c.cnt_lastName+'['+c.cnt_shortName+']' AS Name,c.cnt_internalId,c.cnt_id,u.user_id,u.user_superUser ,u.user_inactive,u.user_maclock,u.user_EntryProfile,u.user_IsUserwise,ISNULL(u.DefaultServiceDashboard,0) AS DefaultServiceDashboard,ISNULL(u.DefaultSTBDashboard,0) AS DefaultSTBDashboard,ISNULL(u.DefaultSTBDashboard,0) AS DefaultSTBDashboard ", "tbl_master_user u,tbl_master_contact c", "u.user_id='" + userId + "' AND u.user_contactId=c.cnt_internalId");
            dsUserDetail = oDBEngine.PopulateData("u.user_name as user1 , u.user_loginId as Login,u.user_branchId as Branchid,u.user_group as usergroup,u.user_AllowAccessIP,u.user_contactId as ContactId, c.cnt_firstName + ' ' +c.cnt_lastName+'['+c.cnt_shortName+']' AS Name,c.cnt_internalId,c.cnt_id,u.user_id,u.user_superUser ,u.user_inactive,u.user_maclock,u.user_EntryProfile,u.user_IsUserwise,ISNULL(u.DefaultServiceDashboard,0) AS DefaultServiceDashboard,ISNULL(u.DefaultSTBDashboard,0) AS DefaultSTBDashboard,ISNULL(u.DefaultSTBDashboard,0) AS DefaultSTBDashboard,ISNULL(u.AssignToLead,0) AS AssignToLead ", "tbl_master_user u,tbl_master_contact c", "u.user_id='" + userId + "' AND u.user_contactId=c.cnt_internalId");
            //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
            if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
            {
                //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master                
                //txtusername.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user1"]);
                //txtuserid.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["Login"]);
                //txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["cnt_internalId"]);
                if (Request.QueryString["key"] == "Copy")
                {
                    txtusername.Text = "";
                    txtuserid.Text ="";
                    txtReportTo_hidden.Value = "";
                }
                else
                {
                    txtusername.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user1"]);
                    txtuserid.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["Login"]);
                    txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["cnt_internalId"]);
                }
                dropdownlistbranch.SelectedValue = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["Branchid"]);
                string usergroup = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["usergroup"]);

                try
                {
                    ddlGroups.SelectedValue = usergroup.Trim();
                }
                catch
                {
                    ddlGroups.SelectedIndex = 0;
                }
                //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master 
                //txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["ContactId"]);
                if(Request.QueryString["key"]=="Copy")
                {
                    txtReportTo_hidden.Value ="";
                }
                else
                {
                    txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["ContactId"]);
                }
                ddDataEntry.SelectedValue = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_EntryProfile"]);
                selectedValue(usergroup);

                if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_superUser"]).Trim() == "Y")
                {
                    cbSuperUser.Checked = true;
                }
                else
                {
                    cbSuperUser.Checked = false;
                }


                if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_inactive"]).Trim() == "Y")
                {
                    chkIsActive.Checked = true;
                }
                else
                {
                    chkIsActive.Checked = false;
                }

                if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_maclock"]).Trim() == "Y")
                {
                    chkmac.Checked = true;
                }
                else
                {
                    chkmac.Checked = false;
                }

                bool isuserwiseView = Convert.ToBoolean(dsUserDetail.Tables["TableName"].Rows[0]["user_IsUserwise"]);
                chkUserWiseView.Checked = isuserwiseView;

                //For service management Start Tanmoy 
                bool isDefaultServiceDashboard = Convert.ToBoolean(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]);
                chkServiceManagement.Checked = isDefaultServiceDashboard;
                //For service management End Tanmoy

                //For STB management Start Tanmoy 
                bool isDefaultSTBDashboard = Convert.ToBoolean(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]);
                chkSTBManagement.Checked = isDefaultSTBDashboard;
                //For STB management End Tanmoy
                //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                bool isAssignToLead = Convert.ToBoolean(dsUserDetail.Tables["TableName"].Rows[0]["AssignToLead"]);
                chkLeadAssign.Checked = isAssignToLead;
                //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_AllowAccessIP"]) != "")
                {
                    string IP = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_AllowAccessIP"]);
                    string[] IParray = IP.Split('.');
                    if (IParray.Length == 4)
                    {
                        txtIp1.Text = IParray[0];
                        txtIp2.Text = IParray[1];
                        txtIp3.Text = IParray[2];
                        txtIp4.Text = IParray[3];
                    }
                    if (IParray.Length == 3)
                    {
                        txtIp1.Text = IParray[0];
                        txtIp2.Text = IParray[1];
                        txtIp3.Text = IParray[2];
                    }
                    if (IParray.Length == 2)
                    {
                        txtIp1.Text = IParray[0];
                        txtIp2.Text = IParray[1];
                    }
                    if (IParray.Length == 1)
                    {
                        txtIp1.Text = IParray[0];
                    }
                }
            }

        }
        protected void selectedValue(string str)
        {
            for (int i = 0; i <= grdUserAccess.Rows.Count - 1; i++)
            {
                DropDownList drp = (DropDownList)grdUserAccess.Rows[i].FindControl("drpUserGroup");
                for (int j = 0; j <= drp.Items.Count - 1; j++)
                {
                    string[] s = str.Split(',');
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (s[k].ToString() == drp.Items[j].Value)
                        {
                            CheckBox chk = (CheckBox)grdUserAccess.Rows[i].FindControl("chkSegmentId");
                            chk.Checked = true;
                            drp.SelectedValue = Convert.ToString(drp.Items.FindByValue(s[k].ToString()).Value);
                        }
                    }
                }
            }
        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string ChangePassOfUserId = Convert.ToString(Request.QueryString["id"]);
                Session["ChangePassOfUserid"] = ChangePassOfUserId;
                Response.Redirect("../ToolsUtilities/frmchangeuserspassword.aspx");
            }
            catch { }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string contact;


            //-------------Allow Ip Adress---
            string IP1 = txtIp1.Text.Trim();
            string IP2 = txtIp2.Text.Trim();
            string IP3 = txtIp3.Text.Trim();
            string IP4 = txtIp4.Text.Trim();
            string IPAddress = string.Empty;
            if (IP1 != "")
            {
                IPAddress = IP1;
                if (IP2 != "")
                {
                    IPAddress = IPAddress + "." + IP2;
                    if (IP3 != "")
                    {
                        IPAddress = IPAddress + "." + IP3;
                        if (IP4 != "")
                        {
                            IPAddress = IPAddress + "." + IP4;
                        }
                    }
                }
            }
            //------------------------------



            if (txtReportTo_hidden.Value.ToString() != "")
            {
                contact = txtReportTo_hidden.Value;
            }
            else
            {
                contact = txtReportTo_hidden.Value;
            }


            string usergroup = getuserGroup();
            string[,] grpsegment = oDBEngine.GetFieldValue("tbl_master_userGroup", "top 1 grp_segmentid", "grp_id in (" + usergroup.ToString() + ")", 1);
            string[,] segname = oDBEngine.GetFieldValue("tbl_master_segment", "seg_name", "seg_id='" + grpsegment[0, 0] + "'", 1);
            string[,] BranchId = oDBEngine.GetFieldValue("tbl_master_contact", "top 1 cnt_branchid", " cnt_internalId='" + txtReportTo_hidden.Value.ToString() + "'", 1);
            string b_id = BranchId[0, 0];

            if (b_id == "n")
            {
                b_id = "1";
            }
            string superuser = "";
            if (cbSuperUser.Checked == true)
                superuser = "Y";
            else
                superuser = "N";

            string isactive = "";
            string isactivemac = "";
            if (chkIsActive.Checked == true)
                isactive = "Y";
            else
                isactive = "N";

            if (chkmac.Checked == true)
                isactivemac = "Y";
            else
                isactivemac = "N";


            int isUserWiseView = 0;
            if (chkUserWiseView.Checked == true)
            {
                isUserWiseView = 1;
            }

            //FOR Service management Tanmoy
            int isDefaultServiceDashboard = 0;
            if (chkServiceManagement.Checked == true)
            {
                isDefaultServiceDashboard = 1;
            }
            //end for service management Tanmoy

            //FOR STB management Tanmoy
            int isDefaultSTBDashboard = 0;
            if (chkSTBManagement.Checked == true)
            {
                isDefaultSTBDashboard = 1;
            }
            //end for STB management Tanmoy
            //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
            int ischkLeadAssign = 0;
            if (chkLeadAssign.Checked == true)
            {
                ischkLeadAssign = 1;
            }
            //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
            if (Id == "Add")
            {
                string[,] checkUser = oDBEngine.GetFieldValue("tbl_master_user", "user_loginId", " user_loginId='" + txtuserid.Text.ToString().Trim() + "'", 1);
                string check = checkUser[0, 0];
                if (check == "n")
                {
                    if (Convert.ToString(Session["PageAccess"]).Trim() == "All" || Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd")
                    {
                        //// Encrypt  the Password
                        Encryption epasswrd = new Encryption();
                        string Encryptpass = epasswrd.Encrypt(txtpassword.Text.Trim());
                        //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                        //oDBEngine.InsertDataFromAnotherTable(" tbl_master_user ", " user_name,user_branchId,user_loginId,user_password,user_contactId,user_group,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive,user_maclock,user_IsUserwise,PassStrength,DefaultServiceDashboard,DefaultSTBDashboard", null, "'" + txtusername.Text.Trim() + "','" + b_id + "','" + txtuserid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + usergroup + "','" + CreateDate.ToString() + "','" + CreateUser + "',( select top 1 grp_segmentId from tbl_master_userGroup where grp_id in(" + usergroup + ")),86400,'" + superuser + "','" + ddDataEntry.SelectedItem.Value + "','" + IPAddress.Trim() + "','" + isactive + "','" + isactivemac + "','" + isUserWiseView + "', '" + hdPassstrength.Value.ToString() + "','" + isDefaultServiceDashboard + "','" + isDefaultSTBDashboard + "'", null);
                        oDBEngine.InsertDataFromAnotherTable(" tbl_master_user ", " user_name,user_branchId,user_loginId,user_password,user_contactId,user_group,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive,user_maclock,user_IsUserwise,PassStrength,DefaultServiceDashboard,DefaultSTBDashboard,AssignToLead", null, "'" + txtusername.Text.Trim() + "','" + b_id + "','" + txtuserid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + usergroup + "','" + CreateDate.ToString() + "','" + CreateUser + "',( select top 1 grp_segmentId from tbl_master_userGroup where grp_id in(" + usergroup + ")),86400,'" + superuser + "','" + ddDataEntry.SelectedItem.Value + "','" + IPAddress.Trim() + "','" + isactive + "','" + isactivemac + "','" + isUserWiseView + "', '" + hdPassstrength.Value.ToString() + "','" + isDefaultServiceDashboard + "','" + isDefaultSTBDashboard +"','" +ischkLeadAssign+"'", null);
                        //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                        string[,] userid = oDBEngine.GetFieldValue("tbl_master_user", "max(user_id)", null, 1);

                        string splitsegname = segname[0, 0].Split('-')[0].ToString().Trim();
                        string[,] exchsegid = oDBEngine.GetFieldValue("Master_Exchange", "top 1 Exchange_Id", "Exchange_ShortName='" + splitsegname + "'", 1);

                        // Jitendra- Need to work in Financial year validation, this time removed it temporarly
                        //string[,] finyr = oDBEngine.GetFieldValue("Master_FinYear", "top 1 FinYear_Code", "Getdate() between FinYear_StartDate and FinYear_EndDate", 1);

                        //string FinancialYear = GetFinancialYear();

                        string[,] fincYear = oDBEngine.GetFieldValue("Master_FinYear", "top 1 FinYear_Code", "finyear_isactive=1", 1);

                        string FinancialYear = fincYear[0, 0];

                        string[,] exhCntID = oDBEngine.GetFieldValue("Tbl_Master_Exchange", "top 1 exh_CntID", "Exh_ShortName= '" + splitsegname.ToString().Trim() + "'", 1);


                        string[,] userInternalId = oDBEngine.GetFieldValue("tbl_master_user", "user_Contactid", "user_id=" + userid[0, 0] + "", 1);
                        DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select top 1 emp_organization  from tbl_trans_employeectc where emp_cntId='" + userInternalId[0, 0] + "' and emp_id=(select MAX(emp_id) from tbl_trans_employeectc e where e.emp_cntId='" + userInternalId[0, 0] + "'))");

                        if (dtcmp.Rows.Count > 0)
                        {
                            string SegmentId = "1";
                            oDBEngine.InsurtFieldValue("Master_UserCompany", "UserCompany_UserID,UserCompany_CompanyID,UserCompany_CreateUser,UserCompany_CreateDateTime", "'" + userid[0, 0] + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + Convert.ToString(Session["userid"]) + "','" + DateTime.Now + "'");
                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + SegmentId + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + FinancialYear.Trim() + "','','N'");

                        }
                        else
                        {
                            string[,] companymain = oDBEngine.GetFieldValue("Tbl_Master_companyExchange", "top 1 Exch_InternalID,Exch_CompID", "Exch_ExchID='" + exhCntID[0, 0].ToString().Trim() + "' and exch_segmentId='1'", 2);
                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + companymain[0, 0] + "','" + companymain[0, 1].ToString() + "','" + FinancialYear.Trim() + "','','N'");
                        }
                        //--------------------------------

                        //User sync to FSM Start
                        if (hdnSyncUsertoWhileCreating.Value == "Yes")
                        {
                            string[,] cntId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_id", "cnt_internalid='" + userInternalId[0, 0] + "'", 1);
                            UserSyncToFSM(cntId[0, 0]);
                        }

                        //User sync to FSM End

                        Response.Redirect("/OMS/Management/Master/root_user.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Add Records!') </script>");
                    }

                }
                else
                {
                    //txtuserid.Text = "Login Id All Ready Exist !! ";
                    //txtuserid.ForeColor = Color.Red;

                    //lblLoginId.Text = "Login Id All Ready Exist !! ";
                    //lblLoginId.ForeColor = Color.Red;

                    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> jAlert('Login Id All Ready Exist !!') </script>");
                }
            }
//Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
            else if(Request.QueryString["key"]=="Copy")
            {
                string[,] checkUser = oDBEngine.GetFieldValue("tbl_master_user", "user_loginId", " user_loginId='" + txtuserid.Text.ToString().Trim() + "'", 1);
                string check = checkUser[0, 0];
                if (check == "n")
                {
                    if (Convert.ToString(Session["PageAccess"]).Trim() == "All" || Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd")
                    {
                        //// Encrypt  the Password
                        Encryption epasswrd = new Encryption();
                        string Encryptpass = epasswrd.Encrypt(txtpassword.Text.Trim());
                        //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                        //oDBEngine.InsertDataFromAnotherTable(" tbl_master_user ", " user_name,user_branchId,user_loginId,user_password,user_contactId,user_group,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive,user_maclock,user_IsUserwise,PassStrength,DefaultServiceDashboard,DefaultSTBDashboard", null, "'" + txtusername.Text.Trim() + "','" + b_id + "','" + txtuserid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + usergroup + "','" + CreateDate.ToString() + "','" + CreateUser + "',( select top 1 grp_segmentId from tbl_master_userGroup where grp_id in(" + usergroup + ")),86400,'" + superuser + "','" + ddDataEntry.SelectedItem.Value + "','" + IPAddress.Trim() + "','" + isactive + "','" + isactivemac + "','" + isUserWiseView + "', '" + hdPassstrength.Value.ToString() + "','" + isDefaultServiceDashboard + "','" + isDefaultSTBDashboard + "'", null);
                        oDBEngine.InsertDataFromAnotherTable(" tbl_master_user ", " user_name,user_branchId,user_loginId,user_password,user_contactId,user_group,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive,user_maclock,user_IsUserwise,PassStrength,DefaultServiceDashboard,DefaultSTBDashboard,AssignToLead", null, "'" + txtusername.Text.Trim() + "','" + b_id + "','" + txtuserid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + usergroup + "','" + CreateDate.ToString() + "','" + CreateUser + "',( select top 1 grp_segmentId from tbl_master_userGroup where grp_id in(" + usergroup + ")),86400,'" + superuser + "','" + ddDataEntry.SelectedItem.Value + "','" + IPAddress.Trim() + "','" + isactive + "','" + isactivemac + "','" + isUserWiseView + "', '" + hdPassstrength.Value.ToString() + "','" + isDefaultServiceDashboard + "','" + isDefaultSTBDashboard + "','" + ischkLeadAssign + "'", null);
                        //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                        string[,] userid = oDBEngine.GetFieldValue("tbl_master_user", "max(user_id)", null, 1);

                        string splitsegname = segname[0, 0].Split('-')[0].ToString().Trim();
                        string[,] exchsegid = oDBEngine.GetFieldValue("Master_Exchange", "top 1 Exchange_Id", "Exchange_ShortName='" + splitsegname + "'", 1);

                        // Jitendra- Need to work in Financial year validation, this time removed it temporarly
                        //string[,] finyr = oDBEngine.GetFieldValue("Master_FinYear", "top 1 FinYear_Code", "Getdate() between FinYear_StartDate and FinYear_EndDate", 1);

                        //string FinancialYear = GetFinancialYear();

                        string[,] fincYear = oDBEngine.GetFieldValue("Master_FinYear", "top 1 FinYear_Code", "finyear_isactive=1", 1);

                        string FinancialYear = fincYear[0, 0];

                        string[,] exhCntID = oDBEngine.GetFieldValue("Tbl_Master_Exchange", "top 1 exh_CntID", "Exh_ShortName= '" + splitsegname.ToString().Trim() + "'", 1);


                        string[,] userInternalId = oDBEngine.GetFieldValue("tbl_master_user", "user_Contactid", "user_id=" + userid[0, 0] + "", 1);
                        DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select top 1 emp_organization  from tbl_trans_employeectc where emp_cntId='" + userInternalId[0, 0] + "' and emp_id=(select MAX(emp_id) from tbl_trans_employeectc e where e.emp_cntId='" + userInternalId[0, 0] + "'))");

                        if (dtcmp.Rows.Count > 0)
                        {
                            string SegmentId = "1";
                            oDBEngine.InsurtFieldValue("Master_UserCompany", "UserCompany_UserID,UserCompany_CompanyID,UserCompany_CreateUser,UserCompany_CreateDateTime", "'" + userid[0, 0] + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + Convert.ToString(Session["userid"]) + "','" + DateTime.Now + "'");
                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + SegmentId + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + FinancialYear.Trim() + "','','N'");

                        }
                        else
                        {
                            string[,] companymain = oDBEngine.GetFieldValue("Tbl_Master_companyExchange", "top 1 Exch_InternalID,Exch_CompID", "Exch_ExchID='" + exhCntID[0, 0].ToString().Trim() + "' and exch_segmentId='1'", 2);
                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + companymain[0, 0] + "','" + companymain[0, 1].ToString() + "','" + FinancialYear.Trim() + "','','N'");
                        }
                        //--------------------------------

                        //User sync to FSM Start
                        if (hdnSyncUsertoWhileCreating.Value == "Yes")
                        {
                            string[,] cntId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_id", "cnt_internalid='" + userInternalId[0, 0] + "'", 1);
                            UserSyncToFSM(cntId[0, 0]);
                        }

                        //User sync to FSM End

                        Response.Redirect("/OMS/Management/Master/root_user.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Add Records!') </script>");
                    }

                }
                else
                {
                    //txtuserid.Text = "Login Id All Ready Exist !! ";
                    //txtuserid.ForeColor = Color.Red;

                    //lblLoginId.Text = "Login Id All Ready Exist !! ";
                    //lblLoginId.ForeColor = Color.Red;

                    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> jAlert('Login Id All Ready Exist !!') </script>");
                }
            }
            //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
            else
            {
                Int16 userId = Convert.ToInt16(Id);
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Edit" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                {
                    string[,] checkUserEdit = oDBEngine.GetFieldValue("tbl_master_user", "user_id", " user_loginId='" + txtuserid.Text.ToString().Trim() + "'", 1);
                    string checkEdit = checkUserEdit[0, 0];
                    if (checkEdit != "n")
                    {
                        if (Convert.ToInt32(checkEdit) == userId)
                        {
                            //Rev work start 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                            //oDBEngine.SetFieldValue("tbl_master_user", "user_name='" + txtusername.Text + "',user_branchId=" + b_id + ",user_group='" + usergroup + "',user_loginId='" + txtuserid.Text + "',user_inactive='" + isactive + "',user_maclock='" + isactivemac + "',user_contactid='" + contact + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + CreateUser + "',user_superuser ='" + superuser + "',user_EntryProfile='" + ddDataEntry.SelectedItem.Value + "',user_AllowAccessIP='" + IPAddress.Trim() + "',user_IsUserwise='" + isUserWiseView + "',DefaultServiceDashboard=" + isDefaultServiceDashboard + ",DefaultSTBDashboard=" + isDefaultSTBDashboard + "", " user_id ='" + userId + "'");
                            oDBEngine.SetFieldValue("tbl_master_user", "user_name='" + txtusername.Text + "',user_branchId=" + b_id + ",user_group='" + usergroup + "',user_loginId='" + txtuserid.Text + "',user_inactive='" + isactive + "',user_maclock='" + isactivemac + "',user_contactid='" + contact + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + CreateUser + "',user_superuser ='" + superuser + "',user_EntryProfile='" + ddDataEntry.SelectedItem.Value + "',user_AllowAccessIP='" + IPAddress.Trim() + "',user_IsUserwise='" + isUserWiseView + "',DefaultServiceDashboard=" + isDefaultServiceDashboard + ",DefaultSTBDashboard=" + isDefaultSTBDashboard + ",AssignToLead="+ischkLeadAssign+"", " user_id ='" + userId + "'");
                            //Rev work close 12.05.2022 Show in Assign To in Lead Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)
                            Fillgridview();
                            Response.Redirect("/OMS/Management/Master/root_user.aspx");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> jAlert('Login Id All Ready Exist !!') </script>");
                        }
                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_master_user", "user_name='" + txtusername.Text + "',user_branchId=" + b_id + ",user_group='" + usergroup + "',user_loginId='" + txtuserid.Text + "',user_inactive='" + isactive + "',user_maclock='" + isactivemac + "',user_contactid='" + contact + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + CreateUser + "',user_superuser ='" + superuser + "',user_EntryProfile='" + ddDataEntry.SelectedItem.Value + "',user_AllowAccessIP='" + IPAddress.Trim() + "',user_IsUserwise='" + isUserWiseView + "',DefaultServiceDashboard=" + isDefaultServiceDashboard + ",DefaultSTBDashboard=" + isDefaultSTBDashboard + "", " user_id ='" + userId + "'");
                        Fillgridview();
                        Response.Redirect("/OMS/Management/Master/root_user.aspx");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Modify Records!') </script>");
                }
            }

        }

        public static string GetFinancialYear()
        {
            string finyear = "";
            DateTime dt = Convert.ToDateTime(System.DateTime.Now);
            int m = dt.Month;
            int y = dt.Year;
            if (m > 3)
            {
                finyear = y.ToString() + "-" + Convert.ToString((y + 1));
                //get last  two digits (eg: 10 from 2010);
            }
            else
            {
                finyear = Convert.ToString((y - 1)) + "-" + y.ToString();
            }
            return finyear;
        }



        protected string getuserGroup()
        {
            //string str = "";
            //bool flag = true;
            //for (int i = 0; i <= grdUserAccess.Rows.Count - 1; i++)
            //{
            //    CheckBox chk = (CheckBox)grdUserAccess.Rows[i].FindControl("chkSegmentId");
            //    if (chk.Checked == true)
            //    {
            //        DropDownList drp = (DropDownList)grdUserAccess.Rows[i].FindControl("drpUserGroup");
            //        if (flag == true)
            //        {
            //            str += drp.SelectedValue;
            //            flag = false;
            //        }
            //        else
            //        {
            //            str += "," + drp.SelectedValue;
            //        }
            //    }
            //}
            //return str;
            return ddlGroups.SelectedValue.ToString();
        }

        public static string ActionMode { get; set; }
        public void UserSyncToFSM(String cnt_id)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["PortalShopEdit"];
            string apiUrl = "http://3.7.30.86:82/ShopRegisterPortal/UserSync";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(Session["UserID"]);
            EmployeeSyncInput empDtls = new EmployeeSyncInput();
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("FTS_EmployeeUserSyncData");
            proc.AddPara("@ACTION", "UserDetails");
            proc.AddPara("@CNT_ID", cnt_id);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                empDtls.Branch = Convert.ToString(dt.Rows[0]["Branch"]);
                empDtls.cnt_UCC = Convert.ToString(dt.Rows[0]["cnt_UCC"]);
                empDtls.Salutation = Convert.ToString(dt.Rows[0]["Salutation"]);
                empDtls.FirstName = Convert.ToString(dt.Rows[0]["cnt_firstName"]);
                empDtls.MiddleName = Convert.ToString(dt.Rows[0]["cnt_middleName"]);
                empDtls.LastName = Convert.ToString(dt.Rows[0]["cnt_lastName"]);
                empDtls.ContactType = Convert.ToString(dt.Rows[0]["cnt_contactType"]);
                empDtls.ReferedBy = Convert.ToString(dt.Rows[0]["cnt_referedBy"]);
                empDtls.DOB = Convert.ToString(dt.Rows[0]["cnt_dOB"]);
                empDtls.MaritalStatus = Convert.ToString(dt.Rows[0]["cnt_maritalStatus"]);
                empDtls.AnniversaryDate = Convert.ToString(dt.Rows[0]["cnt_anniversaryDate"]);
                empDtls.Sex = Convert.ToString(dt.Rows[0]["cnt_sex"]);
                empDtls.CreateDate = Convert.ToString(dt.Rows[0]["CreateDate"]);
                empDtls.CreateUser = Convert.ToString(378);
                empDtls.Bloodgroup = Convert.ToString(dt.Rows[0]["cnt_bloodgroup"]);
                empDtls.SettlementMode = Convert.ToString(dt.Rows[0]["cnt_SettlementMode"]);
                empDtls.ContractDeliveryMode = Convert.ToString(dt.Rows[0]["cnt_ContractDeliveryMode"]);
                empDtls.DirectTMClient = Convert.ToString(dt.Rows[0]["cnt_DirectTMClient"]);
                empDtls.RelationshipWithDirector = Convert.ToString(dt.Rows[0]["cnt_RelationshipWithDirector"]);
                empDtls.HasOtherAccount = Convert.ToString(dt.Rows[0]["cnt_HasOtherAccount"]);
                empDtls.Is_Active = Convert.ToString(dt.Rows[0]["Is_Active"]);
                empDtls.cnt_IdType = Convert.ToString(dt.Rows[0]["cnt_IdType"]);
                empDtls.AccountGroupID = Convert.ToString(dt.Rows[0]["AccountGroupID"]);
                empDtls.DateofJoining = Convert.ToString(dt.Rows[0]["emp_dateofJoining"]);
                empDtls.Organization = Convert.ToString(dt.Rows[0]["Organization"]);
                empDtls.JobResponsibility = Convert.ToString(dt.Rows[0]["job_responsibility"]);
                empDtls.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                empDtls.emp_type = Convert.ToString(dt.Rows[0]["emp_type"]);
                empDtls.Department = Convert.ToString(dt.Rows[0]["Department"]);
                empDtls.ReportTo = Convert.ToString(dt.Rows[0]["ReportTo"]);
                empDtls.Deputy = Convert.ToString(dt.Rows[0]["Deputy"]);
                empDtls.Colleague = Convert.ToString(dt.Rows[0]["Colleague"]);

                empDtls.workinghours = Convert.ToString(dt.Rows[0]["workinghours"]);
                empDtls.TotalLeavePA = Convert.ToString(dt.Rows[0]["TotalLeavePA"]);
                empDtls.LeaveSchemeAppliedFrom = Convert.ToString(dt.Rows[0]["LeaveSchemeAppliedFrom"]);
                empDtls.username = Convert.ToString(dt.Rows[0]["user_name"]);
                empDtls.Encryptpass = Convert.ToString(dt.Rows[0]["user_password"]);
                empDtls.UserLoginId = Convert.ToString(dt.Rows[0]["user_loginId"]);
                empDtls.usergroup = Convert.ToString(dt.Rows[0]["usergroup"]);
            }

            String Status = "Failed";
            String FailedReason = "";
            string data = JsonConvert.SerializeObject(empDtls);
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(data), "data");
            var result = httpClient.PostAsync(apiUrl, form).Result;
            oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

            if (Convert.ToString(oview.status) == "200")
            {
                Status = "Success";
            }
            else if (Convert.ToString(oview.status) == "202")
            {
                FailedReason = "Unique Code Duplicate";
            }
            else if (Convert.ToString(oview.status) == "205")
            {
                FailedReason = "Failed";
            }
            else if (Convert.ToString(oview.status) == "204")
            {
                FailedReason = oview.message;
            }

            ProcedureExecute proc1 = new ProcedureExecute("FTS_EmployeeUserSyncData");
            proc1.AddPara("@ACTION", "SyncLog");
            proc1.AddPara("@InternalID", Convert.ToString(dt.Rows[0]["cnt_internalId"]));
            proc1.AddPara("@SyncBy", userid);
            proc1.AddPara("@Status", Status);
            proc1.AddPara("@FailedReason", FailedReason);
            proc1.AddPara("@CNT_ID", oview.Cnt_id);
            proc1.AddPara("@FSMUser_id", oview.User_id);
            proc1.AddPara("@User_Name", Convert.ToString(dt.Rows[0]["user_name"]));
            proc1.AddPara("@LoginID", Convert.ToString(dt.Rows[0]["user_loginId"]));
            //proc1.AddPara("@AssignedUser", Convert.ToString(dt.Rows[0]["cnt_internalId"]));
            proc1.AddPara("@BranchName", Convert.ToString(dt.Rows[0]["Branch"]));
            proc1.AddPara("@GroupName", Convert.ToString(dt.Rows[0]["usergroup"]));
            int i = proc1.RunActionQuery();
        }

        public class EmployeeSyncInput
        {
            public String Branch { get; set; }
            public String cnt_UCC { get; set; }
            public String Salutation { get; set; }
            public String FirstName { get; set; }
            public String MiddleName { get; set; }
            public String LastName { get; set; }
            //public String cnt_contactSource { get; set; }
            public String ContactType { get; set; }
            //public String cnt_legalStatus { get; set; }
            public String ReferedBy { get; set; }
            public String DOB { get; set; }
            public String MaritalStatus { get; set; }
            public String AnniversaryDate { get; set; }
            //public String cnt_education { get; set; }
            //public String cnt_profession { get; set; }
            public String Sex { get; set; }
            public String CreateDate { get; set; }
            public String CreateUser { get; set; }
            public String Bloodgroup { get; set; }
            //public String WebLogIn { get; set; }
            //public String PassWord { get; set; }
            public String SettlementMode { get; set; }
            public String ContractDeliveryMode { get; set; }
            public String DirectTMClient { get; set; }
            public String RelationshipWithDirector { get; set; }
            public String HasOtherAccount { get; set; }
            public String Is_Active { get; set; }
            public String cnt_IdType { get; set; }
            public String AccountGroupID { get; set; }
            public String DateofJoining { get; set; }
            public String Organization { get; set; }
            public String JobResponsibility { get; set; }
            public String Designation { get; set; }
            public String emp_type { get; set; }
            public String Department { get; set; }
            public String ReportTo { get; set; }
            public String Deputy { get; set; }
            public String Colleague { get; set; }
            public String workinghours { get; set; }
            public String TotalLeavePA { get; set; }
            public String LeaveSchemeAppliedFrom { get; set; }

            public String username { get; set; }
            public String Encryptpass { get; set; }
            public String UserLoginId { get; set; }
            public String usergroup { get; set; }
        }

        public class RegisterShopOutput
        {
            public String status { get; set; }
            public String message { get; set; }
            public String Cnt_id { get; set; }
            public String User_id { get; set; }
        }
    }
}