using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.attendance
{
    public partial class attendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getPageloadPerm()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                boxValues cls = new boxValues();
                DataTable dt = oDBEngine.GetDataTable(@"select EmpCount,PresentCount,AbsentCount,LateComersCount,OnLeaveCount,InOutCount,AttendanceDet,RecentAtt
                                                from tbl_master_dashboard_setting_details 
                                                where user_id=" + Convert.ToString(HttpContext.Current.Session["userid"]));
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            cls.EmpCount = Convert.ToBoolean(dt.Rows[0]["EmpCount"]);
                            cls.PresentCount = Convert.ToBoolean(dt.Rows[0]["PresentCount"]);
                            cls.AbsentCount = Convert.ToBoolean(dt.Rows[0]["AbsentCount"]);
                            cls.LateComersCount = Convert.ToBoolean(dt.Rows[0]["LateComersCount"]);
                            cls.OnLeaveCount = Convert.ToBoolean(dt.Rows[0]["OnLeaveCount"]);
                            cls.InOutCount = Convert.ToBoolean(dt.Rows[0]["InOutCount"]);
                            cls.AttendanceDet = Convert.ToBoolean(dt.Rows[0]["AttendanceDet"]);
                            cls.RecentAtt = Convert.ToBoolean(dt.Rows[0]["RecentAtt"]);
                        }
                    }
             return cls;
        }

        [WebMethod]
        public static object RecentAttendance()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            attendanceRecentClass cls = new attendanceRecentClass();
            DataTable dt = oDBEngine.GetDataTable(@"select Emp_InternalId,Name,isnull(convert(nvarchar(10),In_Time,105),'') In_Time from  v_TodaysAttendance order by In_Time desc");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    cls.Emp_InternalId = Convert.ToString(dt.Rows[0]["Emp_InternalId"]);
                    cls.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    cls.In_Time = Convert.ToString(dt.Rows[0]["In_Time"]);
                }
            }
            return cls;
            }

            [WebMethod]
            public static object inOutAttendance()
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                inoutClass cls = new inoutClass();
                DataTable dt = oDBEngine.GetDataTable(@"select Emp_InternalId,Name,count from  v_TodaysAttendanceCount order by Name desc");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        cls.Emp_InternalId = Convert.ToString(dt.Rows[0]["Emp_InternalId"]);
                        cls.Name = Convert.ToString(dt.Rows[0]["Name"]);
                        cls.count = Convert.ToInt32(dt.Rows[0]["count"]);
                    }
                }
                return cls;
            }

        }

    public class attendanceRecentClass
    {
        public string Emp_InternalId { get; set; }
        public string Name { get; set; }
        public string In_Time { get; set; }
       
    }
    public class inoutClass
    {
        public string Emp_InternalId { get; set; }
        public string Name { get; set; }
        public Int32 count { get; set; }

    }
    public class boxValues
    {
        public bool EmpCount { get; set; }
        public bool PresentCount { get; set; }
        public bool AbsentCount { get; set; }
        public bool LateComersCount { get; set; }
        public bool OnLeaveCount { get; set; }
        public bool InOutCount { get; set; }
        public bool RecentAtt { get; set; }
        public bool AttendanceDet { get; set; }
    }
}