using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_setMailDisclaimer : System.Web.UI.Page
    {
        public string pageAccess = "";
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
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
                txtDisclaimer.Text = "";
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_email_disclaimer ", " dec_status='C' ", " dec_status='O' ");
            NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_email_disclaimer ", " dec_disclaimer,dec_status, CreateDate,CreateUser ", "'" + txtDisclaimer.Text + "','O','" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"]);

        }
    }
}