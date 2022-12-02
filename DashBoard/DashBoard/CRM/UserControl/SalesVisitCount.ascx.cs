using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.CRM.UserControl
{
    public partial class SalesVisitCount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
                proc.AddVarcharPara("@Action", 100, "GetMaxSvCount");
                DataTable maxTable = proc.GetTable();
                if (maxTable.Rows[0][0] != null)
                    FromDateSV.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDateSV.Date = DateTime.Now.Date;

                 
                TodateSV.Date = DateTime.Now;
                FromDateSV.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateSV.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);

            }
            else {
                gridSV.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridSV.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = TodateSV.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetSvCount");
            proc.AddPara("@FromDate", FromDateSV.Date);
            proc.AddPara("@Todate", toDate);
            proc.AddPara("@ActivityState", dpActivitylistsv.SelectedValue);
            // proc.AddPara("@customerId", hdnCustomerId.Value);
            DataTable CallcountData = proc.GetTable();

            gridSV.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exportersv.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}