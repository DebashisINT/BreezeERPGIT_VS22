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

namespace DashBoard.DashBoard.CRM.UserControl
{
    public partial class ActivityHistory : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc1 = new ProcedureExecute("prc_crmDb");
                proc1.AddVarcharPara("@Action", 100, "GetmaxActHistory");
                DataTable maxTable = proc1.GetTable();
                if (maxTable.Rows[0][0] != null)
                    FromDateActHis.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDateActHis.Date = DateTime.Now.Date;

                 
                TodateActHis.Date = DateTime.Now;
                FromDateActHis.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                TodateActHis.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            }
        }

        protected void EntityServerModeActHis_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            DateTime toDate = new DateTime();
            toDate = TodateActHis.Date;
            toDate = toDate.Date.AddHours(23);


            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            e.KeyExpression = "SMenId";
            if (IsPostBack)
            {
                e.QueryableSource = from d in dbcontext.tbl_CRMDb_ActHistories
                                    where d.UserId == Convert.ToInt32(Session["userid"]) 
                                    select d;

                
                    gridActHis.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                    gridActHis.Styles.EmptyDataRow.CssClass = "serverMsg";
                 
            }
            else
            {
                e.QueryableSource = from d in dbcontext.tbl_CRMDb_ActHistories
                                    where d.UserId ==99999
                                    select d;
            }

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporterActHis.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }

        protected void gridActHis_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = TodateActHis.Date;
            toDate1 = toDate1.Date.AddHours(23);
            ProcedureExecute proc = new ProcedureExecute("prc_ActHistory");
            proc.AddPara("@Fromdate", FromDateActHis.Date);
            proc.AddPara("@Todate", toDate1);
            proc.AddPara("@Userid", Session["userid"]);
            proc.AddPara("@smanId", EmpIdActHis.Value);
            proc.AddPara("@LastCount", txtlastDays.Text);
            proc.AddPara("@ActivityType", dpActivitylist.SelectedValue);
            proc.RunActionQuery();
            gridActHis.DataBind();

            for (int i = 1; i <= 5; i++)
            {
                gridActHis.Columns["Outcomeband" + i.ToString()].Visible = false;
                gridActHis.Columns["FeedBackband" + i.ToString()].Visible = false; 
            }

            for (int i = 1; i <= Convert.ToInt32(txtlastDays.Text); i++)
            {
                gridActHis.Columns["Outcomeband" + i.ToString()].Visible = true;
                gridActHis.Columns["FeedBackband" + i.ToString()].Visible = true; 
            }



        }
    }
}