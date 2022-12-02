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
    public partial class QuoteCount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc1 = new ProcedureExecute("prc_crmDb");
                proc1.AddVarcharPara("@Action", 100, "GetmaxQuoteCount");
                DataTable maxTable = proc1.GetTable();
                if (maxTable.Rows[0][0] != null)
                    FromDateQt.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDateQt.Date = DateTime.Now.Date;


                 
                TodateQt.Date = DateTime.Now;
                FromDateQt.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateQt.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);

                ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
                proc.AddVarcharPara("@Action", 100, "quotecmbcalldispositions"); 
                DataTable disposition = proc.GetTable();
                quotecmbcalldispositions.DataSource = disposition;
                quotecmbcalldispositions.TextField = "call_dispositions";
                quotecmbcalldispositions.ValueField = "call_id";
                quotecmbcalldispositions.DataBind();
                quotecmbcalldispositions.SelectedIndex = 0;


            }
            else {
                gridQt.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridQt.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = TodateQt.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetQuoteCount");
            proc.AddPara("@FromDate", FromDateQt.Date);
            proc.AddPara("@Todate", toDate);
            proc.AddPara("@disposition", quotecmbcalldispositions.Value);
            DataTable CallcountData = proc.GetTable();

            gridQt.DataSource = CallcountData;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterQt.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}