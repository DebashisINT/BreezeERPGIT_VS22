using System;
using System.Web;
using System.Web.UI.WebControls;
//using DevExpress.Web;
////using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_frmAddDosPrinter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //Printerdata.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    Printerdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //Printerdata.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    Printerdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //Session["userid"] = 1;//ASPxGridViewTemplateReplacement
            // form1.Attributes.Add("onload", "height()");
            string suser = "";
            //string SQuery = "";
            //DataTable DtblDosPrint = new DataTable();
            //Converter objConverter = new Converter();
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            if (HttpContext.Current.Session["userid"].ToString() != "")
            {
                suser = HttpContext.Current.Session["userid"].ToString();
            }
            else
            {

                Response.End();
            }
            if (!IsPostBack)
            {
                //SQuery = "select * from config_dosprinter where DosPrinter_User=" + suser;
                //DtblDosPrint = oDBEngine.GetDataTable("config_dosprinter", "*", "DosPrinter_User=" + suser);
                //if (DtblDosPrint.Rows.Count > 0)
                //{
                //    DosPrinterGrid.DataSource = DtblDosPrint;
                //    DosPrinterGrid.DataBind();
                //}
            }


        }
        protected void DosPrinterGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            string user = HttpContext.Current.Session["userid"].ToString();
            TextBox txtPrinterName = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtPrinterName");
            TextBox txtLocation = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtLocation");
            if (txtPrinterName.Text.ToString() != "" && txtLocation.Text.ToString() != "")
            {
                e.NewValues["DosPrinter_Name"] = txtPrinterName.Text;
                //selectLocationPrinter = (ASPxUploadControl)DosPrinterGrid.FindEditFormTemplateControl("selectLocationPrinter");
                //selectlocationpath = (FileUpload)DosPrinterGrid.FindEditFormTemplateControl("uplLocationPath");
                //string locationpath = Path.GetFullPath(selectlocationpath.FileName);
                e.NewValues["DosPrinter_Location"] = txtLocation.Text;
                //Session["location"] = null;
                e.NewValues["DosPrinter_User"] = Session["userid"].ToString();
            }
            else
            {

            }

        }

        protected void DosPrinterGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string id = e.Keys[0].ToString();
            string user = HttpContext.Current.Session["userid"].ToString();
            TextBox txtPrinterName = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtPrinterName");
            TextBox txtLocation = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtLocation");
            if (txtPrinterName.Text.ToString() != "" && txtLocation.Text.ToString() != "")
            {
                e.NewValues["DosPrinter_Name"] = txtPrinterName.Text;
                //selectLocationPrinter = (ASPxUploadControl)DosPrinterGrid.FindEditFormTemplateControl("selectLocationPrinter");
                //selectlocationpath = (FileUpload)DosPrinterGrid.FindEditFormTemplateControl("uplLocationPath");
                //string locationpath = Path.GetFullPath(selectlocationpath.FileName);
                e.NewValues["DosPrinter_Location"] = txtLocation.Text;
                //Session["location"] = null;
                e.NewValues["DosPrinter_User"] = Session["userid"].ToString();
                e.NewValues["DosPrinter_ID"] = id;
            }
            else
            {
            }

        }


        protected void selectLocationPrinter_upload(object sender, FileUploadCompleteEventArgs e)
        {
            Session["location"] = (sender as TextBox).Text.ToString();

            //if ((sender as ASPxUploadControl).UploadedFiles[0].IsValid)
            //{


            //}
        }
        protected void DosPrinterGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //Printerdata.SelectCommand = "select * from config_dosprinter";
            Printerdata.DataBind();
            if (e.Parameters == "s")
            {
                DosPrinterGrid.Settings.ShowFilterRow = true;
            }
            if (e.Parameters == "All")
            {

                DosPrinterGrid.FilterExpression = String.Empty;
            }

        }

        protected void DosPrinterGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string user = HttpContext.Current.Session["userid"].ToString();
            TextBox txtPrinterName = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtPrinterName");
            TextBox txtLocation = (TextBox)DosPrinterGrid.FindEditFormTemplateControl("txtLocation");
            if (txtPrinterName.Text.ToString() == "" || txtLocation.Text.ToString() == "" || user == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "abcd", "<Script>alert('Please fill up the blank fields');</Script>");
                Response.Write("Please fill up the blank fields");
            }
        }
    }
}