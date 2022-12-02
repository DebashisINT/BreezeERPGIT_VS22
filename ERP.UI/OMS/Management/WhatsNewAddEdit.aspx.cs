using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management
{
    public partial class WhatsNewAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] == "Add")
                {

                }
                else
                {
                    var directory = new DirectoryInfo(Server.MapPath("~/App_Data/WorkDirectory"));
                    var myFile = (from f in directory.GetFiles()
                                  orderby f.LastWriteTime descending
                                  select f).First();

                    whatsNew.Open(Path.Combine(Server.MapPath("~/App_Data/WorkDirectory"),myFile.FullName));
                    whatsNew.RibbonMode = DevExpress.Web.ASPxRichEdit.RichEditRibbonMode.None;
                        whatsNew.ReadOnly=true;

                }
            }
        }
    }
}