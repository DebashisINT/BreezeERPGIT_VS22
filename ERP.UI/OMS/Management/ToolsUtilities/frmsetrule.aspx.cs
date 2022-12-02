using System;
using System.Web.UI;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmsetrule : System.Web.UI.Page
    {
        public string[] set_rules = new string[4];
        protected void Page_Load(object sender, EventArgs e)
        {
            set_rules[0] = "On Approval";
            set_rules[1] = "On Rejection";
            set_rules[2] = "On Approval with Modifications";
            set_rules[3] = "On Request For Approval by Higher Authority";
        }
        public void showlist()
        {
            string str = Request.QueryString["id"].ToString();
            string[] st = str.Split(',');
            string i = "1";
            string check_status;
            Response.Write("<table>");
            foreach (string x in set_rules)
            {
                bool flag = true;
                check_status = Convert.ToBoolean(language_status(str, i)).ToString();
                if (Convert.ToBoolean(check_status) == false)
                {
                    check_status = "";
                }
                else
                {
                    check_status = "checked='CHECKED'";
                } Response.Write("<tr><td><input " + check_status + " type='checkbox' id='chk' name='chk' value='" + i + "'></td><td>" + x.ToString() + "</td></tr>");
                i = i + 1;
            }
            Response.Write("</table>");
        }
        public bool language_status(string lng_collection, string lng)
        {
            bool status = false;
            string[] menus = null;
            if (lng_collection != null)
            {
                menus = lng_collection.Split(',');
                for (int j = 0; j <= menus.Length - 1; j++)
                {
                    if (menus[j] == lng)
                        status = true;
                }
            }
            return status;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string chk = Request["chk"];
            string popupScript = "";
            popupScript = "<script language='javascript'>" + "window.opener.document.aspnetForm" + Request.QueryString["Hcontrol"] + ".value='" + chk + "';";
            popupScript += "window.close();</script>";
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", popupScript);
        }
    }
}