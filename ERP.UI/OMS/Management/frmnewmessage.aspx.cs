using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frmnewmessage : System.Web.UI.Page
    {
        string strScrolling;

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            string innerHTML = showMessage();
            this.SpnResultId.InnerHtml = innerHTML;
        }
        private string showMessage()
        {
            try
            {
                DataTable dt = new DataTable();
                string[,] message1 = oDBEngine.GetFieldValue("tbl_master_message", " count(msg_messageread)", "(msg_messageread = 0) and (msg_targetuser=" + HttpContext.Current.Session["userid"] + ")", 1);
                if (message1[0, 0] != "n")
                {
                    MessageId.InnerHtml = "<img src=\"../images/New_messag1.gif\" style=\"border-width:0px; cursor: hand;\" onclick='javascript:imgClick();' alt=\"You Have " + message1[0, 0] + " New Messages!!\" />";
                }
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'> starttime();</script>");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }
            return strScrolling;

        }

    }
}