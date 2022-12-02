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

namespace ERP.OMS.Management.Master
{
    public partial class RootVendorDetails : ERP.OMS.ViewState_class.VSPage
    {
        public string Id;
        int CreateUser;
        DateTime CreateDate;


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();

        public string pageAccess = "";

        protected void Page_Load(object sender, EventArgs e)
        {
                

            Id = Request.QueryString["id"];
            ActionMode = Request.QueryString["id"];
            CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (!IsPostBack)
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_Vendoruser.aspx");
                Session["addedituser"] = "yes";
                
               
             
                if (Id != "Add")
                {                   
                    //if (!rights.CanEdit)
                    //{
                        //Response.Redirect("/OMS/Management/master/root_Vendoruser.aspx");
                    //}
                    ShowData(Id);
                    txtusername.Enabled = false;
                }
                else
                {                   
                    //if (!rights.CanAdd)
                    //{
                     //   Response.Redirect("/OMS/Management/master/root_Vendoruser.aspx");
                    //}
                }

               


            }
            /*--Set Page Accesss--*/
            string pageAccess = oDBEngine.CheckPageAccessebility("root_Vendoruser.aspx");
            Session["PageAccess"] = pageAccess;
        }

        

        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> ALLEmployee(string reqStr)
        {
            UserBL objUserBL = new UserBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //DT = oDBEngine.GetDataTable("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " cnt_contactType='DV' and tbl_master_contact.cnt_InternalId not in (select User_contactId from tbl_master_Vendoruser) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

          
                //DT = oDBEngine.GetDataTable("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " cnt_contactType='DV' and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
            if (ActionMode == "Add")
            {
                //DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC ", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId    and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
                //DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%') and tbl_master_contact.cnt_internalId not in (select user_contactId from tbl_master_user) group by tbl_trans_employeeCTC.emp_id,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' , cnt_internalId   having  max(tbl_trans_employeeCTC.emp_id) in (select MAX(tbl_trans_employeeCTC.emp_id)from tbl_trans_employeeCTC group by emp_cntId)");
                DT = objUserBL.PopulateAssociatedEmployee(0, "VendorAddMode");
            }
            else
            {
                DT = objUserBL.PopulateAssociatedEmployee(Convert.ToInt32(ActionMode), "VendorEditMode");
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
           
            //List<string> obj = new List<string>();
            //foreach (DataRow dr in DT.Rows)
            //{
            //    if (Convert.ToString(dr["Name"]) != "")
            //    {
            //        obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            //    }
            //}

            //return obj;
        }
        //...............code end........




        
      
       
       
        protected void ShowData(string Id)
        {
            
            user_password.Visible = false;
            Int16 userId = Convert.ToInt16(Id);
            DataSet dsUserDetail = new DataSet();
            dsUserDetail = oDBEngine.PopulateData("u.user_name as user1 , u.user_loginId as Login,u.user_branchId as Branchid,u.user_group as usergroup,u.user_AllowAccessIP,u.user_contactId as ContactId, c.cnt_firstName + ' ' +c.cnt_lastName+'['+c.cnt_shortName+']' AS Name,c.cnt_internalId,c.cnt_id,u.user_id,u.user_superUser ,u.user_inactive,u.user_EntryProfile", "tbl_master_Vendoruser u,tbl_master_contact c", "u.user_id='" + userId + "' AND u.user_contactId=c.cnt_internalId");
            if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
            {
                //txtusername.Text = dsUserDetail.Tables["TableName"].Rows[0]["user1"].ToString();
                txtusername.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user1"]);
                //txtloginid.Text = dsUserDetail.Tables["TableName"].Rows[0]["Login"].ToString();
                txtloginid.Text = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["Login"]);

                //txtReportTo.Text = dsUserDetail.Tables["TableName"].Rows[0]["Name"].ToString();
                //txtReportTo_hidden.Value = dsUserDetail.Tables["TableName"].Rows[0]["cnt_internalId"].ToString();
                txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["cnt_internalId"]);

                //dropdownlistbranch.SelectedValue = dsUserDetail.Tables["TableName"].Rows[0]["Branchid"].ToString();
               
                //string usergroup = dsUserDetail.Tables["TableName"].Rows[0]["usergroup"].ToString();
               
                // hdncontactId.Value = dsUserDetail.Tables["TableName"].Rows[0]["ContactId"].ToString();
                //txtReportTo_hidden.Value = dsUserDetail.Tables["TableName"].Rows[0]["ContactId"].ToString();
                txtReportTo_hidden.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["ContactId"]);
                //ddDataEntry.SelectedValue = dsUserDetail.Tables["TableName"].Rows[0]["user_EntryProfile"].ToString();
                
               
                //if (dsUserDetail.Tables["TableName"].Rows[0]["user_superUser"].ToString().Trim() == "Y")
               


                //if (dsUserDetail.Tables["TableName"].Rows[0]["user_inactive"].ToString().Trim() == "Y")
                if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["user_inactive"]).Trim() == "Y")
                {
                    chkIsActive.Checked = true;
                }
                else
                {
                    chkIsActive.Checked = false;
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

                        
            if (txtReportTo_hidden.Value.ToString() != "")
            {
                contact = txtReportTo_hidden.Value;
            }
            else
            {
                contact = txtReportTo_hidden.Value;
            }


            //string usergroup = getuserGroup();
            //string[,] grpsegment = oDBEngine.GetFieldValue("tbl_master_userGroup", "top 1 grp_segmentid", "grp_id in (" + usergroup.ToString() + ")", 1);
           // string[,] segname = oDBEngine.GetFieldValue("tbl_master_segment", "seg_name", "seg_id='" + grpsegment[0, 0] + "'", 1);
            //string[,] BranchId = oDBEngine.GetFieldValue("tbl_master_contact", "top 1 cnt_branchid", " cnt_internalId='" + txtReportTo_hidden.Value.ToString() + "'", 1);
            //string b_id = BranchId[0, 0];
            //if (b_id == "n")
            //{
            //    b_id = "1";
            //}
            string superuser = "";
            //if (cbSuperUser.Checked == true)
            //    superuser = "Y";
            //else
            //    superuser = "N";

            string isactive = "";
            if (chkIsActive.Checked == true)
                isactive = "Y";
            else
                isactive = "N";

            if (Id == "Add")
            {
                string[,] checkUser = oDBEngine.GetFieldValue("tbl_master_user", "user_loginId", " user_loginId='" + txtloginid.Text.ToString().Trim() + "'", 1);
                string check = checkUser[0, 0];
                if (check == "n")
                {
                    if (Convert.ToString(Session["PageAccess"]).Trim() == "All" || Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd")
                    {



                        //// Encrypt  the Password
                        Encryption epasswrd = new Encryption();
                        string Encryptpass = epasswrd.Encrypt(txtpassword.Text.Trim());
                        oDBEngine.InsertDataFromAnotherTable(" tbl_master_Vendoruser ", " user_name,user_branchId,user_loginId,user_password,user_contactId,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive", null, "'" + txtusername.Text.Trim() + "','','" + txtloginid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + CreateDate.ToString() + "','" + CreateUser + "',86400,'" + superuser + "',' ','',' ','" + isactive + "'", null);

                        //oDBEngine.InsertDataFromAnotherTable(" tbl_master_Vendor ", " user_name,user_branchId,user_loginId,user_password,user_contactId,user_group,CreateDate,CreateUser,user_lastsegement,user_TimeForTickerRefrsh,user_superuser,user_EntryProfile,user_AllowAccessIP,user_inactive", null, "'" + txtusername.Text.Trim() + "','" + b_id + "','" + txtloginid.Text.Trim() + "','" + Encryptpass + "','" + contact + "','" + usergroup + "','" + CreateDate.ToString() + "','" + CreateUser + "',( select top 1 grp_segmentId from tbl_master_userGroup where grp_id in(" + usergroup + ")),86400,'" + superuser + "','" + ddDataEntry.SelectedItem.Value + "','" + IPAddress.Trim() + "','" + isactive + "'", null);
                        string[,] userid = oDBEngine.GetFieldValue("tbl_master_Vendoruser", "max(user_id)", null, 1);


                      

                        //string splitsegname = segname[0, 0].Split('-')[0].ToString().Trim();

                       // string[,] exchsegid = oDBEngine.GetFieldValue("Master_Exchange", "top 1 Exchange_Id", "Exchange_ShortName='" + splitsegname + "'", 1);
                      //  string[,] finyr = oDBEngine.GetFieldValue("Master_FinYear", "top 1 FinYear_Code", "Getdate() between FinYear_StartDate and FinYear_EndDate", 1);
                      //  string[,] exhCntID = oDBEngine.GetFieldValue("Tbl_Master_Exchange", "top 1 exh_CntID", "Exh_ShortName= '" + splitsegname.ToString().Trim() + "'", 1);
                     

                        //Added New code to add eefault company in the tbl_master_user
                       // string[,] userInternalId = oDBEngine.GetFieldValue("tbl_master_user", "user_Contactid", "user_id=" + userid[0, 0] + "", 1);
                       // DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + userInternalId[0, 0] + "')");

                        //if (dtcmp.Rows.Count > 0)
                        //{
                        //    string SegmentId = "1";
                        //    oDBEngine.InsurtFieldValue("Master_UserCompany", "UserCompany_UserID,UserCompany_CompanyID,UserCompany_CreateUser,UserCompany_CreateDateTime", "'" + userid[0, 0] + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + Convert.ToString(Session["userid"]) + "','" + DateTime.Now + "'");
                        //    oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + SegmentId + "','" + Convert.ToString(dtcmp.Rows[0]["cmp_internalid"]) + "','" + finyr[0, 0].ToString().Trim() + "','','N'");

                        //}
                        //else
                        //{
                        //    string[,] companymain = oDBEngine.GetFieldValue("Tbl_Master_companyExchange", "top 1 Exch_InternalID,Exch_CompID", "Exch_ExchID='" + exhCntID[0, 0].ToString().Trim() + "' and exch_segmentId='1'", 2);
                        //    oDBEngine.InsurtFieldValue("tbl_trans_LastSegment", "ls_cntid,ls_lastsegment,ls_userid,ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType", "'" + contact + "','" + grpsegment[0, 0] + "','" + userid[0, 0] + "','" + companymain[0, 0] + "','" + companymain[0, 1].ToString() + "','" + finyr[0, 0].ToString().Trim() + "','','N'");
                        //}
                        //--------------------------------
                        Response.Redirect("/OMS/Management/Master/root_Vendoruser.aspx",true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Add Records!') </script>");
                    }

                }
                else
                {
                    txtloginid.Text = "LoginId All Ready Exist !! ";
                    txtloginid.ForeColor = Color.Red;
                }
            }
            else
            {
                Int16 userId = Convert.ToInt16(Id);
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Edit" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                {

                    oDBEngine.SetFieldValue("tbl_master_Vendoruser", "user_name='" + txtusername.Text + "',user_branchId=0,user_loginId='" + txtloginid.Text + "',user_inactive='" + isactive + "',user_contactid='" + contact + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + CreateUser + "',user_superuser ='" + superuser + "',user_EntryProfile='',user_AllowAccessIP=''", " user_id ='" + userId + "'");
                   
                    Response.Redirect("/OMS/Management/Master/root_Vendoruser.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Modify Records!') </script>");
                }
            }

        }
        //protected string getuserGroup()
        //{
           
        //    return ddlGroups.SelectedValue.ToString();
        //}

        // Code Added By Sandip on 22032017 to use Query String Value in Web Method for Chosen DropDown

        public static string ActionMode { get; set; }
       

    }
}