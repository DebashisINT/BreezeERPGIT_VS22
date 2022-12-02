using System;
using System.Web.UI;

namespace ERP.OMS.Management
{
    public partial class management_frm_WelcomeLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script language='Javascript'>height();</script>");
        }
    }
}