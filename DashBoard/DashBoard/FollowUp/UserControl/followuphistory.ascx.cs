using DashBoard.DashBoard.Models;
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

namespace DashBoard.DashBoard.FollowUp.UserControl
{
    public partial class followuphistory : System.Web.UI.UserControl
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
                //Rev Maynak
                    HistoryFormDate.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    HistoryFormDate.Date = DateTime.Now.Date;

                 
                HistoryTodate.Date = DateTime.Now.Date;
            }
        }

        protected void EntityServerModePhoneDet_Selecting1(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            DateTime toDate = new DateTime();
            toDate = HistoryTodate.Date;
            toDate = toDate.Date.AddHours(23);

            e.KeyExpression = "id";
            if (IsPostBack)
            {
                e.QueryableSource = from d in dbcontext.v_followupDbHistories
                                    where d.FollowDate >= HistoryFormDate.Date && d.FollowDate <= toDate 
                                    select d;
            }
            else
            {
                e.QueryableSource = from d in dbcontext.v_followupDbHistories
                                    where d.openClsoe == "" 
                                    select d;
            }
        }

        protected void LinkButton1pactallHistory_Click(object sender, EventArgs e)
        {
            exporterpactHistory.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}