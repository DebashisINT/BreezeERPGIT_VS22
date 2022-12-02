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
    public partial class NewVsRep : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FromDateNvR.Date = DateTime.Now;
                FromDateNvR.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            }
            else {
                gridNvR.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridNvR.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridNvR_DataBinding(object sender, EventArgs e)
        {

            DateTime toDate = new DateTime();
            toDate = FromDateNvR.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetNewVsOldCount");
            proc.AddPara("@FromDate", toDate);
            proc.AddPara("@prodOrClass", ddProdOrClass.SelectedValue);
            DataTable CallcountData = proc.GetTable();

            gridNvR.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterNvR.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}