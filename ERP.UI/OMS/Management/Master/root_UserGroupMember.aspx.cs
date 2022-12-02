using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Configuration;
using System.Data;
namespace ERP.OMS.Management.Master
{
    public partial class root_UserGroupMember : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Code  Added and Commented By Priti on 20122016 to use Covert.Tostring() instead of Tostring()
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                //string sPath = HttpContext.Current.Request.Url.ToString();
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_user.aspx");
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //RootUserDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    RootUserDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //RootUserDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    RootUserDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------


            if (!IsPostBack)
            {
                userGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesroot_useruserGrid";

                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesroot_useruserGrid');</script>");

                Session["addedituser"] = "";
                Session["exportval"] = null;
            }

            if (Request.QueryString["grp"] != null)
            {
                string grp = Convert.ToString(Request.QueryString["grp"]);
                DataTable dt = oDBEngine.GetDataTable("select grp_name from tbl_master_usergroup where grp_id='" + grp + "'");
                if (dt.Rows.Count > 0)
                {
                    txtgrpname.Text = Convert.ToString(dt.Rows[0]["grp_name"]);
                }
               
                RootUserDataSource.SelectCommand = "SELECT  distinct user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select top 1  deg_designation from tbl_master_designation where deg_id in (select top 1 emp_Designation from tbl_trans_employeeCTC where emp_CntId= user_contactId order by emp_id desc )) as designation FROM [tbl_master_user],tbl_master_employee where emp_ContactId=user_contactId  and user_group=" + grp + "";
            }
            // Mantis Issue 24367
            hdnCalledFromModule.Value = "";
            if (Request.QueryString["callfrommodule"] != null)
            {
                hdnCalledFromModule.Value = Request.QueryString["callfrommodule"];
            }
            // End of Mantis Issue 24367
           

        }
     
       
        protected void userGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
             string grp=Convert.ToString( Request.QueryString["grp"]);
            if (Convert.ToString(Session["addedituser"]) == "yes")
            {
                Session["addedituser"] = "";
                if (userGrid.FilterExpression == "")
                {
                    RootUserDataSource.SelectCommand = "SELECT  distinct user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select top 1  deg_designation from tbl_master_designation where deg_id in (select top 1 emp_Designation from tbl_trans_employeeCTC where emp_CntId= user_contactId order by emp_id desc )) as designation FROM [tbl_master_user],tbl_master_employee where emp_ContactId=user_contactId  and  user_group=" + grp + "";
                    userGrid.DataBind();
                }
                else
                {
                    RootUserDataSource.SelectCommand = "SELECT  distinct user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select top 1  deg_designation from tbl_master_designation where deg_id in (select top 1 emp_Designation from tbl_trans_employeeCTC where emp_CntId= user_contactId order by emp_id desc )) as designation FROM [tbl_master_user],tbl_master_employee where emp_ContactId=user_contactId and user_group="+grp+" and " + userGrid.FilterExpression;
                    userGrid.DataBind();
                }
            }
            else
            {
              
                if (e.Parameters == "All")
                {
                    userGrid.FilterExpression = string.Empty;
                    RootUserDataSource.SelectCommand = "SELECT  distinct user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select top 1  deg_designation from tbl_master_designation where deg_id in (select top 1 emp_Designation from tbl_trans_employeeCTC where emp_CntId= user_contactId order by emp_id desc )) as designation FROM [tbl_master_user],tbl_master_employee where emp_ContactId=user_contactId and user_group="+grp+"";
                    userGrid.DataBind();
                }
            }
        }
        protected void userGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "1";
        }

    }
}