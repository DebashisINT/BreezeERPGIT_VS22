using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using System.Data;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRCostCenter : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DepartmentSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    DepartmentSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //DepartmentSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    DepartmentSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }
           
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/HRCostCenter.aspx");

            if (!IsPostBack)
            {
                ParentSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                HeadSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            }
            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void CostDepartmentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                CostDepartmentGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                CostDepartmentGrid.FilterExpression = string.Empty;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
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
        protected void CostDepartmentGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }

        protected void CostDepartmentGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string grp_id = Convert.ToString(e.KeyValue);
            string commanName = e.CommandArgs.CommandName;
            if (commanName == "delete")
            {
                string ID = grp_id;
                oDBEngine.DeleteValue("tbl_master_costCenter", "cost_id='" + ID + "'");

                bindgrid();
                CostDepartmentGrid.CancelEdit();
            }
        }
        protected void bindgrid()
        {
            DepartmentSource.SelectCommand = "HrCostCenterSelect";
            CostDepartmentGrid.DataBind();
        }
    }
}