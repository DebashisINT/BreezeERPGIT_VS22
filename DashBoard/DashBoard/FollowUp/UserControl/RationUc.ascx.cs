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
    public partial class RationUc : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ProcedureExecute proc1 = new ProcedureExecute("Prc_followupDb");
                proc1.AddVarcharPara("@Action", 100, "GetMaxFollowDate");
                DataTable maxTable = proc1.GetTable();
                //Rev Maynak 19-11-2019 Refer:0021360
                //if (maxTable.Rows[0][0] != null)
                if ((!(maxTable.Rows[0][0] is DBNull)) && (maxTable.Rows[0][0] != null))
                    //End of Rev Maynak
                    ratioFromdate.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    ratioFromdate.Date = DateTime.Now.Date;


                 
                ratioTodate.Date = DateTime.Now;
            }
        }
        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {
            DateTime ToDate = new DateTime();
            ToDate = ratioTodate.Date;
            ToDate = ToDate.Date.AddHours(23);


            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "Conversion");
            proc.AddPara("@FromDate", ratioFromdate.Date);
            proc.AddPara("@Todate", ToDate);
            DataTable CallcountData = proc.GetTable();

            ratio.DataSource = CallcountData;
        }

        protected void LinkButton1closedrtio_Click(object sender, EventArgs e)
        {
            exporterclosedratio.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}