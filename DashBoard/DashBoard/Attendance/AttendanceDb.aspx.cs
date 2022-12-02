using DashBoard.DashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.Attendance
{
    public partial class AttendanceDb : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = oDBEngine.GetDataTable(@"select EmpCount,PresentCount,AbsentCount,LateComersCount,OnLeaveCount,InOutCount,AttendanceDet,RecentAtt
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows.Count > 0)
                    {
                        EmpCount.Visible = Convert.ToBoolean(dt.Rows[0]["EmpCount"]);
                        PresentCount.Visible = Convert.ToBoolean(dt.Rows[0]["PresentCount"]);
                        AbsentCount.Visible = Convert.ToBoolean(dt.Rows[0]["AbsentCount"]);
                        LateComersCount.Visible = Convert.ToBoolean(dt.Rows[0]["LateComersCount"]);
                        OnLeaveCount.Visible = Convert.ToBoolean(dt.Rows[0]["OnLeaveCount"]);
                        InOutCount.Visible = Convert.ToBoolean(dt.Rows[0]["InOutCount"]);
                        AttendanceDet.Visible = Convert.ToBoolean(dt.Rows[0]["AttendanceDet"]);
                        RecentAtt.Visible = Convert.ToBoolean(dt.Rows[0]["RecentAtt"]);

                    }
                }

            }
        }

        protected void EntityServerModeActHis_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            e.KeyExpression = "Emp_InternalId";
            e.QueryableSource = from d in dbcontext.v_TodaysAttendances 
                                orderby d.In_Time descending
                                select d;
        }

        protected void LinqServerModeDataSource1Count_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            e.KeyExpression = "Emp_InternalId";
            e.QueryableSource = from d in dbcontext.v_TodaysAttendanceCounts
                                orderby d.Name descending
                                select d;
        }


       

    }
}