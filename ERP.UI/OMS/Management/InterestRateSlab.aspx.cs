using System;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_InterestRateSlab : System.Web.UI.Page
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
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
                    //IntSlabDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    IntSlabDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //IntSlabDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    IntSlabDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            IntSlabDataSource.SelectCommand = "SELECT IntSlab_ID,IntSlab_Code,IntSlab_AmountFrom,IntSlab_AmountTo,IntSlab_Rate  from Master_IntSlab order by IntSlab_Code,IntSlab_AmountFrom,IntSlab_AmountTo";
            GridIntSlabSlab.DataBind();
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

        protected void GridIntSlabSlab_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Keys["IntSlab_ID"] != null && e.Keys["IntSlab_ID"].ToString().Trim() != "")
            {
                string slabid = e.Keys["IntSlab_ID"].ToString();

                string[,] dd = oDBEngine.GetFieldValue("Master_IntSlab",
                    "IntSlab_Code", "IntSlab_ID='" + slabid + "'", 1);
                Int32 rowsdelete = oDBEngine.DeleteValue("Master_IntSlab", " IntSlab_Code='" + dd[0, 0] + "'");

            }

        }
        protected void GridIntSlabSlab_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }
        protected void GridIntSlabSlab_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString() == "s")
            {
                GridIntSlabSlab.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                GridIntSlabSlab.FilterExpression = string.Empty;
            }

            GridIntSlabSlab.ClearSort();
            GridIntSlabSlab.DataBind();
        }
    }
}