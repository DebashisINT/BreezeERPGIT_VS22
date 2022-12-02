using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.crmAnalytic
{
    public partial class performaQuotSalemanwise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getPageloadCall()
        {
            AnalyticPageloadcls cls = new AnalyticPageloadcls();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetMaxCallDetails");
            DataTable maxTable = proc.GetTable();
            if (maxTable.Rows[0][0] != null)
                cls.FromDateActHis = Convert.ToDateTime(maxTable.Rows[0][0]).ToString("dd-MM-yyyy");
            else
                cls.FromDateActHis = DateTime.Now.Date.ToString("dd-MM-yyyy");


            cls.TodateActHis = DateTime.Now.ToString("dd-MM-yyyy");
            cls.FromDateActHisMinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");
            cls.TodateActHisMinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");

            return cls;
        }
        [WebMethod]
        public static object GetPhoneCount(string frmdate, string todate, string ActivityType)
        {
            List<PhonecallCounts> cls = new List<PhonecallCounts>();
            //DateTime toDate1 = new DateTime();
            //toDate1 = TodateActHis.Date;
            //toDate1 = toDate1.Date.AddHours(23);
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetPhoneCallCount");
            proc.AddPara("@FromDate", frmdate);
            proc.AddPara("@Todate", todate);
            proc.AddPara("@ActivityState", ActivityType);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new PhonecallCounts()
                       {
                           CallCount = Convert.ToString(dr["CallCount"]),
                           cnt_internalId = Convert.ToString(dr["cnt_internalId"]),
                           custCount = Convert.ToString(dr["custCount"]),
                           SalesManName = Convert.ToString(dr["SalesManName"]),

                       }).ToList();
            }
            return cls;
        }
        public class AnalyticPageloadcls
        {
            public String FromDateActHis { get; set; }
            public String TodateActHis { get; set; }
            public String FromDateActHisMinDate { get; set; }
            public String TodateActHisMinDate { get; set; }
        }

        public class PhonecallCounts
        {
            public String cnt_internalId { get; set; }
            public String SalesManName { get; set; }
            public String CallCount { get; set; }
            public String custCount { get; set; }
        }
    }
}