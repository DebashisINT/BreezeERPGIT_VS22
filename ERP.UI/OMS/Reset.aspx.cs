using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLogicLayer;
public partial class Reset : System.Web.UI.Page
{
    DBEngine oDBEngine = new DBEngine("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["cashJournal"] = "1";
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        
        string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;
        string UserName = txtLogIN.Text;
        string PassWord = txtPassword.Text;
        DataTable dtIPAddress = oDBEngine.GetDataTable("tbl_master_user", "user_lastIP", " user_loginID='" + UserName + "' and user_password='" + PassWord + "'");
        string GetIP = dtIPAddress.Rows[0][0].ToString();
        //if (GetIP.Trim() != IPNAme.Trim())
        //{
            int NoofRowsAffected = oDBEngine.SetFieldValue("tbl_master_user", " user_status='0',user_lastIP='" + IPNAme + "'", " user_loginID='" + UserName + "' and user_password='" + PassWord + "'");
            if (NoofRowsAffected > 0)
            {
                Session.Abandon();
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>alert('Reset Successfully !!')</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>alert('UserID or Password IS Incorrect !!')</script>");
        //}
        //else
        //    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>alert('You Can not Reset This ID in this Machine !!')</script>");
    }
}
