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
    public partial class SmenOrdCount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
                proc.AddVarcharPara("@Action", 100, "GetMaxOrderCount");
                DataTable maxTable = proc.GetTable();
                //if (maxTable.Rows[0][0] != null)
                if (!(maxTable.Rows[0][0] is DBNull))
                    FromDateOCount.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDateOCount.Date = DateTime.Now.Date;

                 
                TodateOCount.Date = DateTime.Now;
                FromDateOCount.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateOCount.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                 
            }
            else {
                gridOCount.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridOCount.Styles.EmptyDataRow.CssClass = "serverMsg";
            }

        }


        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = TodateOCount.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetOrderCount");
            proc.AddPara("@FromDate", FromDateOCount.Date);
            proc.AddPara("@Todate", toDate); 
            DataTable CallcountData = proc.GetTable();

            gridOCount.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterOCount.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}