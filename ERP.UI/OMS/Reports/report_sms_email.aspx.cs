using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_report_sms_email : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        public string pageAccess = "";
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
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
            LeadGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtName.Attributes.Add("onkeyup", "CallAjax(this,'SearchClinetsmsemailreport',event)");
            txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
            ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            if (HttpContext.Current.Session["userid"] == null)
            {
              //  Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            showgrid();
           // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                txtFromDate.Value = Convert.ToDateTime(DateTime.Today);
                txtToDate.Value = Convert.ToDateTime(DateTime.Today);

            }
        }

        private void showgrid()
        {
            string types = rbUser.Value.ToString();
            string startdate = "";
            string Enddate = "";
            string ContactID = "%";
            if (txtName_hidden.Text != "")
            {
                ContactID = txtName_hidden.Text.ToString();
            }
            if (txtFromDate.Text.Contains("-"))
            {
                startdate = txtFromDate.Text.Split('-')[2] + "-" + txtFromDate.Text.Split('-')[1] + "-" + txtFromDate.Text.Split('-')[0];
                Enddate = txtToDate.Text.Split('-')[2] + "-" + txtToDate.Text.Split('-')[1] + "-" + txtToDate.Text.Split('-')[0] + "  23:59:59";
            }

            DataTable dt = new DataTable();
            dt = rep.sp_fetch_report_sms_email(startdate, Enddate, ContactID, Session["userbranchHierarchy"].ToString(), rbsegment.SelectedItem.Value.ToString());
            LeadGrid.DataSource = dt.DefaultView;
            LeadGrid.DataBind();


        }
        protected void btnSearch(object sender, EventArgs e)
        {
            LeadGrid.Settings.ShowFilterRow = true;
        }

        protected void LeadGrid_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LeadGrid.ClearSort();
            showgrid();
            if (e.Parameters == "s")
                LeadGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                LeadGrid.FilterExpression = string.Empty;
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            //switch (Filter)
            //{
            //    case 1:
            //        exporter.WritePdfToResponse();
            //        break;
            //    case 2:
            //        exporter.WriteXlsToResponse();
            //        break;
            //    case 3:
            //        exporter.WriteRtfToResponse();
            //        break;
            //    case 4:
            //        exporter.WriteCsvToResponse();
            //        break;
            //}
        }
        protected void LeadGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            showgrid();
        }

    }
}