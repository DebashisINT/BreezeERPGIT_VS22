using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class root_VendorUser : ERP.OMS.ViewState_class.VSPage
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
                    //RootUserDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    RootUserDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //RootUserDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    RootUserDataSource.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------


            if (!IsPostBack)
            {

                Session["addedituser"] = "";
                Session["exportval"] = null;
            }

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightPL", "<script>height();</script>");
            //       RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId  and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";


           // RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId  and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";

            RootUserDataSource.SelectCommand = "SELECT user_id,user_contactId,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus FROM [tbl_master_Vendoruser] ,tbl_master_contact where tbl_master_contact.cnt_internalId=tbl_master_Vendoruser.user_contactId";

        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            userGrid.Columns[5].Visible = false;
            userGrid.Columns[6].Visible = false;
            string filename = "Users";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Users";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
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
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 20122016 to use Covert.Tostring() instead of Tostring()
            //Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }

        }
        protected void userGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // Code  Added and Commented By Priti on 20122016 to use Covert.Tostring() instead of Tostring()
            ////RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            //if (Session["addedituser"].ToString() == "yes")
            if (Convert.ToString(Session["addedituser"]) == "yes")
            {
                Session["addedituser"] = "";
                if (userGrid.FilterExpression == "")
                {
                    RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
                    userGrid.DataBind();
                }
                else
                {
                    RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and " + userGrid.FilterExpression;
                    userGrid.DataBind();
                }
            }
            else
            {
                //if (e.Parameters == "s")
                //    userGrid.Settings.ShowFilterRow = true;
                if (e.Parameters == "All")
                {
                    userGrid.FilterExpression = string.Empty;
                    RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (user_inactive ='Y') then 'Inactive' else 'Active' end as Status,user_status as Onlinestatus, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
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