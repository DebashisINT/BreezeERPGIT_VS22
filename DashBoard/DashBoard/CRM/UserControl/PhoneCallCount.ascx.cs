using DashBoard.DashBoard.Models;
using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.CRM.UserControl
{
    public partial class PhoneCallCount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
                proc.AddVarcharPara("@Action", 100, "GetMaxCallDetails");
                DataTable maxTable = proc.GetTable();
                if (maxTable.Rows[0][0] != null)
                        FromDate.Date = Convert.ToDateTime(maxTable.Rows[0][0]);
                else
                    FromDate.Date = DateTime.Now.Date;

                Todate.Date = DateTime.Now.Date;
                FromDate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                Todate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                 
            }
            else { 
            gridPhone.SettingsText.EmptyDataRow = "No data found for the selected criteria.";
                gridPhone.Styles.EmptyDataRow.CssClass = "serverMsg";
            }
        }


       
        protected void gridPhone_DataBinding1(object sender, EventArgs e)
        {
            DateTime toDate = new DateTime();
            toDate = Todate.Date;
            toDate = toDate.Date.AddHours(23);

            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetPhoneCallCount");
            proc.AddPara("@FromDate", FromDate.Date);
            proc.AddPara("@Todate", toDate);
            proc.AddPara("@ActivityState", dpActivitylist.SelectedValue);
            // proc.AddPara("@customerId", hdnCustomerId.Value);
            DataTable CallcountData = proc.GetTable();

            gridPhone.DataSource = CallcountData;
            

        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
                DateTime toDate = new DateTime();
                toDate = Todate.Date;
                toDate = toDate.Date.AddHours(23);

                e.KeyExpression = "phd_id";
                if (IsPostBack)
                {
                    e.QueryableSource = from d in dbcontext.v_phoneCallDetails
                                        where d.smanId == hidesalesman.Text //hdSalesmanId.Value
                                        && d.slv_nextActivityType == Convert.ToInt32(dpActivitylist.SelectedValue)
                                        && d.phd_callDate >= FromDate.Date && d.phd_callDate <= toDate
                                        orderby d.phd_id descending
                                        select d;
                }
                else
                {
                    e.QueryableSource = from d in dbcontext.v_phoneCallDetails
                                        where d.phd_id == -1
                                        orderby d.phd_id descending
                                        select d;
                }

        
           
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
    }
}