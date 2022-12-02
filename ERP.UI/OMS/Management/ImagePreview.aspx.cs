using System;

namespace ERP.OMS.Management
{
    public partial class Management_ImagePreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string str = Server.MapPath(Session["ImagePath"].ToString());
            //str.Replace("management", "Documents");
            imgCashBankDoc.ImageUrl = "~/Documents/" + Session["ImagePath"].ToString();
            //  imgCashBankDoc.ImageUrl = str.Replace("management", "Documents");
            //  imgCashBankDoc.ImageUrl="D:\\project\\Influx\\Documents\\112\\2015\\3993262015113537AMAlert2.png";

        }
    }
}