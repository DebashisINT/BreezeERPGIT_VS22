using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class erpAddHolidayBL
    {
    }

    public class HolidayMain
    {
        public string holidayCode { get; set; }
        public string holidayName { get; set; }
        public string branch { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string holiday_ID { get; set; }
        public string Action { get; set; }
        public String Gu_id { get; set; }
    }

    public class HolidayDetails
    {
        public string holidayName { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public String Guid { get; set; }
    }
}
