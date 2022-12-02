using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Management
{
    public partial class JSFUNCTION_TimeSetForTicker : System.Web.UI.Page
    {
        public string pageAccess = "";
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
                lblMessage.Text = "";
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_user ", " user_TimeForTickerRefrsh=" + cmbTimeForTicker.SelectedItem.Value + " ", " user_id=" + HttpContext.Current.Session["userid"]);
            if (NoOfRowsEffected > 0)
            {
                lblMessage.Text = "Saved Successfully!";
                HttpContext.Current.Session["TimeForTickerDisplay"] = cmbTimeForTicker.SelectedItem.Value;
            }
            else
                lblMessage.Text = "Unsuccessful!";
        }
    }
}