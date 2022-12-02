using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_master_root_Companies : System.Web.UI.Page
    {
        public string pageAccess = "";

        /* For Tier Structure
        DBEngine oDBEngine = new DBEngine(string.Empty);
         */

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
        protected void Page_Init(object sender, EventArgs e)
        {
            CompaniesDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlParentComp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            CompanyGrid.Settings.ShowFilterRow = true;
        }
        protected void CompanyGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "cmp_parentid")
            {

                (e.Editor as ASPxComboBox).DataBound += new EventHandler(editCombo_DataBound);
            }
        }
        private void editCombo_DataBound(object sender, EventArgs e)
        {
            ListEditItem noneItem = new ListEditItem("None", null);
            (sender as ASPxComboBox).Items.Insert(0, noneItem);
        }
        protected void CompanyGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                CompanyGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                CompanyGrid.FilterExpression = string.Empty;
            }
            CompanyGrid.DataBind();
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
    }
}