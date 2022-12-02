using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//////using DevExpress.Web;
using BusinessLogicLayer;
using DevExpress.Web;

namespace ERP.OMS
{
    public partial class _Default : System.Web.UI.Page
    {
        Converter Oconverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxDateEdit1.EditFormat = EditFormat.Custom;
            ASPxDateEdit1.EditFormatString = Oconverter.GetDateFormat();
            ASPxDateEdit1.UseMaskBehavior = true;
            ASPxDateEdit1.Value = System.DateTime.Now;
            
        }
    }
}
