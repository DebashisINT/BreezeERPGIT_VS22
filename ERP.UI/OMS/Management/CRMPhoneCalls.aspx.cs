using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_CRMPhoneCalls : System.Web.UI.Page
    {
        int cpage = 1;
        int showitemperpage = 20;
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

        }

    }
}