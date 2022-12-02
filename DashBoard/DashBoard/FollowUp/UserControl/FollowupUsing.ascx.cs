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
    public partial class FollowupUsing : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc1 = new ProcedureExecute("Prc_followupDb");
                proc1.AddVarcharPara("@Action", 100, "GetMaxOpenFollowDate");
                DataTable maxTable = proc1.GetTable();
                //Rev Maynak 19-11-2019 Refer:0021360
                //if (maxTable.Rows[0][0] != null)
                if ((!(maxTable.Rows[0][0] is DBNull)) && (maxTable.Rows[0][0] != null))
                //End of Rev Maynak
                    usingFormDate.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    usingFormDate.Date = DateTime.Now.Date;


                 
                usingTodate.Date = DateTime.Now;
            }

        }


        protected void EntityServerModePhoneDet_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            DateTime toDate = new DateTime();
            toDate = usingTodate.Date;
            toDate = toDate.Date.AddHours(23);

            e.KeyExpression = "id";
            if (IsPostBack)
            {
                e.QueryableSource = from d in dbcontext.v_followupDbHistories
                                    where d.FollowDate >= usingFormDate.Date && d.FollowDate <= toDate
                                    && d.openClsoe == ddlusning.Text
                                    select d;
            }
            else
            {
                e.QueryableSource = from d in dbcontext.v_followupDbHistories
                                    where d.openClsoe == ""
                                    select d;
            }
        }

        protected void LinkButton1pactallusing_Click(object sender, EventArgs e)
        {
            exporterpactusing.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}