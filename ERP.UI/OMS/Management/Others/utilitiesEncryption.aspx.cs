using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Others
{
    public partial class utilitiesEncryption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromArea.Text = "";
                txtToArea.Text = "";
            }
        }
        protected void btnEncode_Click(object sender, EventArgs e)
        {
            try
            {
                string textboxValue = Convert.ToString(txtFromArea.Text);
                txtToArea.Text = BusinessLogicLayer.SaltEncryptionDecryption.Encrypt(textboxValue, "IndUs@321");
            }
            catch
            {
                txtToArea.Text = "";
            }
        }
        protected void btnDecode_Click(object sender, EventArgs e)
        {
            try
            {
                string textboxValue = Convert.ToString(txtFromArea.Text);
                txtToArea.Text = BusinessLogicLayer.SaltEncryptionDecryption.Decrypt(textboxValue, "IndUs@321");
            }
            catch
            {
                txtToArea.Text = "";
            }
        }
    }
}