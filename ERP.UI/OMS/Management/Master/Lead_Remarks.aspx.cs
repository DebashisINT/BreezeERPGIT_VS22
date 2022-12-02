using System;
using System.Web;
using System.Web.UI;
////using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Lead_Remarks : ERP.OMS.ViewState_class.VSPage
    {

        public string pageAccess = "";
        int UpperBound;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                   // SqlRemarks.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                }
                else
                {
                  //  SqlRemarks.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            SqlRemarks.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlCategory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            GridRemarks.Settings.ShowFilterRow = true;
        }
        protected void GridRemarks_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "cat_id")
            {

                (e.Editor as ASPxComboBox).DataBound += new EventHandler(editCombo_DataBound);
            }
        }
        private void editCombo_DataBound(object sender, EventArgs e)
        {
            ListEditItem noneItem = new ListEditItem("None", null);
            (sender as ASPxComboBox).Items.Insert(0, noneItem);

        }


        protected void GridRemarks_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GridRemarks.ClearSort();
            GridRemarks.DataBind();
            if (e.Parameters == "s")
                GridRemarks.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                GridRemarks.FilterExpression = string.Empty;
            }
        }
    }
}