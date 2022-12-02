using System;
using System.Web.UI;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_attendance_Lock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"];
            ////////////////////  Old Code   /////////////////////////////
            //string obj = "A";
            //lblHead.Text = "Attendance Lock";
            //if (id != null)
            //{
            //    obj = "B";
            //    lblHead.Text = "Convert PD to CL";
            //}
            //Page.ClientScript.RegisterStartupScript(GetType(), "testttt", "<script>pagecall('" + obj + "');</script>");
            ////////////////////////////////////////////////////////////////


            //////////////////  New Code  //////////////////////////////
            if (id != null)
                Response.Redirect("frm_attendance_PD_calculation.aspx");
            else
                Response.Redirect("frm_attendance_Lock_iframe.aspx");
            ////////////////////////////////////////////////////////////////
        }
    }
}