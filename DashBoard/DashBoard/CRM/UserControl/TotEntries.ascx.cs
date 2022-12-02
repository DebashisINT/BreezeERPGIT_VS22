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
    public partial class TotEntries : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
                proc.AddVarcharPara("@Action", 100, "GetMaxTotActcount");
                DataTable maxTable = proc.GetTable();
                if (maxTable.Rows[0][0] != null)
                    FromDateTE.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDateTE.Date = DateTime.Now.Date;


                TodateTE.Date = DateTime.Now;

                FromDateTE.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateTE.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            }
            else {
                gridTE.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridTE.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = TodateTE.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetTotActcount");
            proc.AddPara("@FromDate", FromDateTE.Date);
            proc.AddPara("@Todate", toDate);  
            DataTable CallcountData = proc.GetTable();

            gridTE.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterTE.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });

        }
    }
}