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
    public partial class efficiencyratio : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ProcedureExecute proc1 = new ProcedureExecute("prc_crmDb");
                //proc1.AddVarcharPara("@Action", 100, "GetMaxSefficiency");
                //DataTable maxTable = proc1.GetTable();
                //if (maxTable.Rows[0][0] != null)
                //    FromDateEF.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                //else
                //  FromDateEF.Date = DateTime.Now.Date;
                FromDateEF.Date = DateTime.Now.Date.AddDays(-30);
                 
                TodateEF.Date = DateTime.Now;
                FromDateEF.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateEF.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            }
            else {
                gridEF.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridEF.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = TodateEF.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetSefficiency");
            proc.AddPara("@FromDate", FromDateEF.Date);
            proc.AddPara("@Todate", toDate);
            DataTable CallcountData = proc.GetTable();

            gridEF.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterEF.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}