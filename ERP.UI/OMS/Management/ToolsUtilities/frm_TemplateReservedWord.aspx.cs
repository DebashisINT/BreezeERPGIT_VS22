using System;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_TemplateReservedWord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["Type"].ToString() == "AL")
            {
                tr1.Visible = true;
                tr2.Visible = true;
            }
            else if (Request.QueryString["Type"].ToString() == "EM")
            {
                tr1.Visible = true;
                tr2.Visible = true;
            }
            else if (Request.QueryString["Type"].ToString() == "CL")
            {
                tr1.Visible = false;
                tr2.Visible = false;

            }

        }
    }
}