using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.XtraScheduler;

namespace DashBoard.DashBoard
{
    public partial class MainDasBoard : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userid"] == null)
                {
                    Response.Redirect("/oms/login.aspx");
                }
                //this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSource2.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            MytaskDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxScheduler1.Start = DateTime.Now;

                ASPxScheduler1.WorkDays.BeginUpdate();
                ASPxScheduler1.WorkDays.Clear();
                ASPxScheduler1.WorkDays.Add(WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday | WeekDays.Friday | WeekDays.Saturday);
                ASPxScheduler1.WorkDays.EndUpdate();
                 
                
                ASPxScheduler1.ActiveViewType = SchedulerViewType.WorkWeek;
                
            }
            ASPxScheduler1.Views.DayView.Enabled = false;
            ASPxScheduler1.Views.WorkWeekView.WorkTime.Start = TimeSpan.FromHours(0);
            ASPxScheduler1.Views.WorkWeekView.WorkTime.End = TimeSpan.FromHours(24);

            ASPxScheduler1.Views.FullWeekView.WorkTime.Start = TimeSpan.FromHours(0);
            ASPxScheduler1.Views.FullWeekView.WorkTime.End = TimeSpan.FromHours(24);

            ASPxScheduler1.WorkWeekView.TimeScale = TimeSpan.FromMinutes(60);
            ASPxScheduler1.FullWeekView.TimeScale = TimeSpan.FromMinutes(60);
        }

     
        [WebMethod]
        public static object MarkComplete(string id)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("update Appointments set isComplete=1 where UniqueID=" + id, con);
            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return new { status = "Ok" };
            }
            catch (Exception ex)
            {
                con.Close();
                return new { status = ex.Message };
            }
        }

        protected void ASPxScheduler1_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
        {
            if (e.Menu.Id == SchedulerMenuItemId.DefaultMenu)
            {
                for (int i = e.Menu.Items.Count; i > 7; i--)
                {
                    e.Menu.Items.RemoveAt(i - 1);
                }
            }
        }

        
    }
}