using System;

namespace ERP.OMS.Reports
{
    public partial class Reports_mtest3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
            Label1.Text = "Page refreshed at " + DateTime.Now.ToString() + "<br>";
        }
    }
}