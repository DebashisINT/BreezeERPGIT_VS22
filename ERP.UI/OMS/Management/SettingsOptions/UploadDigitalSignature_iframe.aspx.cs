using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_UploadDigitalSignature_iframe : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        GlobalSettings globalsetting = new GlobalSettings();
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

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            bindGrid();
            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "height();", true);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/SettingOptions/UploadDigitalSignature_iframe.aspx");
        }

        void bindGrid()
        {
            DataTable DT = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand cmd = new SqlCommand("fetchDigitalSignHolders", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 0;
            //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            //        {

            //            DT.Reset();
            //            da.Fill(DT);
            //        }
            //    }
            //}
            DT = globalsetting.fetchDigitalSignHolders();
            gridSign.DataSource = DT;
            gridSign.DataBind();
        }
        public void bindexport(int Filter)
        {
            gridSign.Columns[3].Visible = false;
            string filename = "Financer";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Financer";
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
        protected void gridSign_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                gridSign.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                gridSign.FilterExpression = string.Empty;
            }

        }
    }
}