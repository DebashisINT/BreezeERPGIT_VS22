using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_HeaderFooter_iframe : System.Web.UI.Page
    {
        string[,] AllType;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        // DBEngine oDBEngine = new DBEngine(string.Empty);
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
            HeaderFooter.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //  ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "height();", true);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HeaderFooter_iframe.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");




        }
        protected void HeaderFooterGrid_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

        }
        protected void HeaderFooterGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            //if (!HeaderFooterGrid.IsNewRowEditing)
            //{
            //    if (e.Column.FieldName == "HeaderFooter_Type")
            //        e.Editor.ReadOnly = true;
            //}


        }
        protected void HeaderFooterGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                HeaderFooterGrid.Settings.ShowFilterRow = true;
            else if (e.Parameters == "All")
            {
                HeaderFooterGrid.FilterExpression = string.Empty;
            }
            else
            {
                HeaderFooterGrid.ClearSort();
                HeaderFooterGrid.DataBind();
            }
        }
        protected void HeaderFooterGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

        }
        protected void HeaderFooterGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["HeaderFooter_Type"] = "Header";
        }
        protected void HeaderFooterGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void HeaderFooterGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {

        }
        //protected void HeaderFooterGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        //{
        //    e.Properties["cpEND"] = "2";
        //}
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
        public void bindexport(int Filter)
        {
            HeaderFooterGrid.Columns[3].Visible = false;
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
    }
}