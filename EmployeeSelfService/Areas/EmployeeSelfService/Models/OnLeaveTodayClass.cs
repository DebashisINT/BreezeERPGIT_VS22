using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{
    public class OnLeaveInfoModelInput
    {
        public string UserId { get; set; }
    }

    public class OnLeaveInfoModelOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public OnLeaveInfo LeaveTodayinfo { get; set; }
    }
    public class OnLeaveInfo
    {
        public string EmpName { get; set; }
        public string ProfileIMG { get; set; }
    }
}