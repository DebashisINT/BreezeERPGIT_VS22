using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_brokerageslab : ERP.OMS.ViewState_class.VSPage
    {
        string aa, ss, dd;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/brokerageslab.aspx");
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //BrokerageSlabDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    BrokerageSlabDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //BrokerageSlabDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    BrokerageSlabDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            BrokerageSlabDataSource.SelectCommand = "SELECT *  from Master_BrokerageSlab order by BrokerageSlab_Code,BrokerageSlab_MinRange,BrokerageSlab_MaxRange";
            BrokerageSlabGrid.DataBind();
            BrokerageSlabGrid.JSProperties["cpDelmsg"] = null;
        }



        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            string filename = "Tax Slabs";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Tax Slabs";
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
        protected void btnSearch(object sender, EventArgs e)
        {
            BrokerageSlabGrid.Settings.ShowFilterRow = true;
        }

        protected void BrokerageSlabGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString() == "s")
            {
                BrokerageSlabGrid.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                BrokerageSlabGrid.FilterExpression = string.Empty;
            }

            BrokerageSlabGrid.ClearSort();
            BrokerageSlabGrid.DataBind();
        }

        protected void BrokerageSlabGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Keys["BrokerageSlab_ID"] != null && e.Keys["BrokerageSlab_ID"].ToString().Trim() != "")
            {
                string slabid = e.Keys["BrokerageSlab_ID"].ToString();

                string[,] dd = oDBEngine.GetFieldValue("Master_BrokerageSlab",
                    "BrokerageSlab_Code", "BrokerageSlab_ID='" + slabid + "'", 1);

                string[,] dlcode = oDBEngine.GetFieldValue("config_brokeragedetail",
                   "brokeragedetail_ID", "brokeragedetail_slabcode='" + dd[0, 0] + "'", 1);
                if (dlcode[0, 0] == "n")
                {
                    Int32 rowsdelete = oDBEngine.DeleteValue("Master_BrokerageSlab", " BrokerageSlab_Code='" + dd[0, 0] + "'");
                    if (rowsdelete > 0)
                        BrokerageSlabGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                    else
                    {
                        BrokerageSlabGrid.JSProperties["cpDelmsg"] = "Not Deleted, There Are Some Problem";
                        e.Cancel = true;
                    }
                }
                else
                {
                    BrokerageSlabGrid.JSProperties["cpDelmsg"] = "Cannot Delete. This Slab Code Is In Use";
                    e.Cancel = true;
                }

            }

        }
        protected void BrokerageSlabGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }

        protected void BrokerageSlabGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }
        }
    }
}