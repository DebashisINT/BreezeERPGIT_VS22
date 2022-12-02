using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;

//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTreeList;
////using DevExpress.Web;
//using System.Collections.Generic;
//using DevExpress.Web;
////using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Member_usergroup : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected string Id;
        int CreateUser;
        DateTime CreateDate;
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
            if (!IsPostBack)
            {

                Session["addedituser"] = "";
            }
            Id = Request.QueryString["id"].ToString();
            HiddenField1.Value = Id;

            CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DataTable dtuser = oDBEngine.GetDataTable("select user_id,user_group,user_name,user_loginid from tbl_master_user");
            for (int i = 0; i < dtuser.Rows.Count; i++)
            {
                string usergroup = dtuser.Rows[i]["user_group"].ToString();
                string[] s = usergroup.Split(',');
                //for (int k = 0; k < s.Length; k++)
                //{
                //    if (s[k].ToString() == Id)
                //    {

                //    }

                //}


            }
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightPL", "<script>height();</script>");
            fillGrid();
            //RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId  and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            //RootUserDataSource.SelectCommand = "select USER_ID,USER_NAME,user_loginId from tbl_master_user WHERE user_group LIKE '"+ id1 +"'";
        }
        private void fillGrid()
        {
            string id1 = '%' + Convert.ToString(Id) + '%';
            DataTable dtbind = oDBEngine.GetDataTable("select USER_ID,USER_NAME,user_loginId from tbl_master_user WHERE user_group LIKE '" + id1 + "'");
            userGrid.DataSource = dtbind.DefaultView;
            userGrid.DataBind();
        }
        protected void userGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string code1 = e.KeyValue.ToString();
            DataTable dtcompare = oDBEngine.GetDataTable("Select user_group from tbl_master_user where user_id='" + code1 + "'");
            string compare = dtcompare.Rows[0]["user_group"].ToString();
            string[] s = compare.Split(',');
            string id2 = Convert.ToString(Id);// +',';
            compare = compare.Replace(id2, "");
            if (compare.Contains(",,"))
            {
                compare = compare.Replace(",,", ",");
            }
            if (compare.StartsWith(","))
            {
                //string sub= compare.Substring(0,1);
                compare = compare.Remove(0, 1);
            }
            if (compare.EndsWith(","))
            {
                compare = compare.Remove(compare.Length - 1) + "";
            }
            int nowofrowsaffested = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + compare + "'", "user_id='" + code1 + "'");
            fillGrid();
        }
        //protected void userGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    string code1 = e.Keys[0].ToString();
        //    DataTable dtcompare=oDBEngine.GetDataTable("Select user_group from tbl_master_user where user_id='"+ code1 +"'");
        //    string compare = dtcompare.Rows[0]["user_group"].ToString();
        //    string[] s = compare.Split(',');
        //    string id2 = Convert.ToString(Id) +',';
        //    compare = compare.Replace(id2, "");
        //    //for (int k = 0; k < s.Length; k++)
        //    //{
        //    //    if (s[k].ToString() == Id)
        //    //    {
        //    //        s[k].Replace(Id, "");
        //    //    }
        //    //    s[k] = s[k] + s[k];
        //    //    int nowofrowsaffested = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + s[k] + "'", "user_id='" + code1 + "'");
        //    //}
        //    int nowofrowsaffested = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + compare + "'", "user_id='" + code1 + "'");

        //}
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}
        protected void userGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ////RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            //if (Session["addedituser"].ToString() == "yes")
            //{
            //    Session["addedituser"] = "";
            //    if (userGrid.FilterExpression == "")
            //    {
            //        RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            //        userGrid.DataBind();
            //    }
            //    else
            //    {
            //        RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and " + userGrid.FilterExpression;
            //        userGrid.DataBind();
            //    }
            //}
            //else
            //{
            //    if (e.Parameters == "s")
            //        userGrid.Settings.ShowFilterRow = true;
            //    if (e.Parameters == "All")
            //    {
            //        userGrid.FilterExpression = string.Empty;
            //        RootUserDataSource.SelectCommand = "SELECT user_id,user_name,user_loginId,case when  (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000') then 'Active' else 'Deactive' end as Status, (select deg_designation from tbl_master_designation where deg_id =emp_Designation) as designation FROM [tbl_master_user],tbl_trans_employeeCTC where emp_cntId=user_contactId and user_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            //        userGrid.DataBind();
            //    }
            //}
        }
        protected void userGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //e.Properties["cpHeight"] = "1";
        }

    }
}