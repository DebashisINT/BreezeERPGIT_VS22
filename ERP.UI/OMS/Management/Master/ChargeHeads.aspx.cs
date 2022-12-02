using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ChargeHeads : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
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
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //MasterDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    MasterDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //MasterDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    MasterDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            MasterDataSource.SelectCommand = "SELECT * from Master_OtherCharges";
            MasterGrid.DataBind();
            MasterGrid.JSProperties["cpDelmsg"] = null;
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
        protected void btnSearch(object sender, EventArgs e)
        {
            MasterGrid.Settings.ShowFilterRow = true;
        }

        protected void MasterGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            MasterGrid.ClearSort();
            MasterGrid.DataBind();
        }

        protected void MasterGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }
        protected void MasterGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (Convert.ToString(e.Values["OtherCharges_Code"]) != "")
            {
                string chargeId = e.Values["OtherCharges_Code"].ToString().Trim();
                SqlDataReader objReader = oDBEngine.GetReader("select othercharge_id from config_otherchargerates where othercharge_chargegroupid='" + chargeId + "' union select Otherchargemember_ID from trans_otherchargemember where Otherchargemember_Otherchargecode='" + chargeId + "'");
                if (objReader.HasRows)
                {
                    MasterGrid.JSProperties["cpDelmsg"] = "Cannot Delete. This ProfileTrade Code Is In Use";
                    e.Cancel = true;
                }
                else
                {
                    MasterGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                }

                oDBEngine.CloseConnection();
                objReader.Close();

            }
        }
    }
}