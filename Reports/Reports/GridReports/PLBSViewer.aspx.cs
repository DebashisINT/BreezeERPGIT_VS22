using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class PLBSViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ASPxDocumentViewer1.Report = (XtraReport)Session["report"];

            if (!IsPostBack)
            {
                lnkClose.HRef = Request.QueryString["key"]+".aspx";
            }
        }
    }
}