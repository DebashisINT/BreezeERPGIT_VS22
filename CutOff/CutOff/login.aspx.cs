using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace CutOff.CutOff
{
    public partial class login : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
     
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(txtPass.Text.Trim());
            string Validuser;
            Validuser = oDBEngine.AuthenticateUser(this.userName.Text, Encryptpass).ToString();
            if (Validuser == "Y")
            {
                Response.Redirect("~/CutOff/Master/FinYearList.aspx");
            }
                   
        }
    }
}