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
    public partial class closedUc : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                closedFromdate.Date = DateTime.Now.AddDays(-30);
                closedTodate.Date = DateTime.Now;
            }
        }

        protected void gridClosed_DataBinding(object sender, EventArgs e)
        {
            DateTime asOnDate = new DateTime();
            asOnDate = closedFromdate.Date;
            asOnDate = asOnDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "GetClosedCount");
            proc.AddPara("@FromDate", asOnDate);
            proc.AddPara("@Todate", closedTodate.Date);
            DataTable CallcountData = proc.GetTable();

            gridClosed.DataSource = CallcountData;
        }

        protected void LinkButton1closed_Click(object sender, EventArgs e)
        {
            exporterclosed.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}