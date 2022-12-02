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

namespace DashBoard.DashBoard.CRM.UserControl
{
    public partial class PendingAct : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 
                FromDatePAct.Date = DateTime.Now;
                FromDatePAct.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            }
            else {
                gridpact.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridpact.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }



        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime asOnDate = new DateTime();
            asOnDate = FromDatePAct.Date;
            asOnDate = asOnDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetPendingAct");
            proc.AddPara("@FromDate", asOnDate);
            proc.AddPara("@ActivityState", dpActivitylistPAct.SelectedValue);
            DataTable CallcountData = proc.GetTable();

            gridpact.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterpact.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}