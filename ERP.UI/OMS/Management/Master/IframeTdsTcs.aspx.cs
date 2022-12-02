using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_IframeTdsTcs : ERP.OMS.ViewState_class.VSPage //SSystem.Web.UI.Page
    {
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
                    //SqlTdsCts.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    SqlTdsCts.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlTdsCts.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    SqlTdsCts.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/IframeTdsTcs.aspx");
            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            gridTdsTcs.JSProperties["cpDelete"] = null;

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }
        protected void gridTdsTcs_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            MasterDataCheckingBL objMasterDataCheckingBL = new MasterDataCheckingBL();
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (WhichCall == "Delete")
            {
                deletecnt = objMasterDataCheckingBL.DeleteTDSTCS(WhichType);
                 //oGenericMethod.Delete_Table("Master_sProducts", "sProducts_ID=" + WhichType + "");
                if (deletecnt > 0)
                {
                    gridTdsTcs.JSProperties["cpDelete"] = "Success";
                    //gridTdsTcs.DataBind();
                }
                else
                    gridTdsTcs.JSProperties["cpDelete"] = "Deletion Failed";
            }
             
            gridTdsTcs.DataBind();

        }
        protected void gridTdsTcs_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }

        #region Export event

        public void bindexport(int Filter)
        {
            gridTdsTcs.Columns[6].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "TDS/TCS";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "TDS/TCS";
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
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
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
        #endregion
    }
}