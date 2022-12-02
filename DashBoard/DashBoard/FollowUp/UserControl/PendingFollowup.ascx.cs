using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.FollowUp.UserControl
{
    public partial class PendingFollowup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FromDatePAct.Date = DateTime.Now;
            }
        }

        protected void gridpact_DataBinding(object sender, EventArgs e)
        {
            DateTime asOnDate = new DateTime();
            asOnDate = FromDatePAct.Date;
            asOnDate = asOnDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "GetPendingAct");
            proc.AddPara("@FromDate", asOnDate); 
            DataTable CallcountData = proc.GetTable();

            gridpact.DataSource = CallcountData;
        }

        protected void LinkButton1pact_Click(object sender, EventArgs e)
        {

        }

        protected void LinkButton1pactfollowup_Click(object sender, EventArgs e)
        {
            exporterpending.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}