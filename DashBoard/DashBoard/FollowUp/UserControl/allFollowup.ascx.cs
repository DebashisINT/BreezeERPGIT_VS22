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
    public partial class allFollowup : System.Web.UI.UserControl
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
                    allFormDate.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    allFormDate.Date = DateTime.Now.Date;


                 
                alltoDate.Date = DateTime.Now;
                allFormDate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                alltoDate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);

               



            }
            else
            {
                gridEF.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridEF.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }

        protected void gridEF_DataBinding(object sender, EventArgs e)
        {
            DateTime ToDate = new DateTime();
            ToDate = alltoDate.Date;
            ToDate = ToDate.Date.AddHours(23);


            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "totalFollowup");
            proc.AddPara("@FromDate", allFormDate.Date);
            proc.AddPara("@Todate", ToDate);
            DataTable CallcountData = proc.GetTable();

            gridEF.DataSource = CallcountData;
        }

        protected void LinkButton1pactallfollow_Click(object sender, EventArgs e)
        {
            exporterpactallfollowup.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}