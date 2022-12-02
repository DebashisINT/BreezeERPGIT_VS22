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
    public partial class effRatio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getPageloadCall()
        {
            AnalyticPageloadcls cls = new AnalyticPageloadcls();
            cls.FromDateEF = DateTime.Now.Date.AddDays(-30).ToString("dd-MM-yyyy");

            cls.TodateEF = DateTime.Now.ToString("dd-MM-yyyy");
            //cls.FromDateEF.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");
            //cls.TodateEF.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");
            return cls;
        }
        [WebMethod]
        public static object GetEff(string asOnDate, string todate)
        {
            List<GetAcSalesmanWiseCls> cls = new List<GetAcSalesmanWiseCls>();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetSefficiency");
            proc.AddPara("@FromDate", asOnDate);
            proc.AddPara("@Todate", todate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new GetAcSalesmanWiseCls()
                       {
                           EF = Convert.ToString(dr["EF"]),
                           cnt_internalId = Convert.ToString(dr["cnt_internalId"]),
                           SalesManName = Convert.ToString(dr["SalesManName"]),

                       }).ToList();
            }
            return cls;
        }


        public class GetAcSalesmanWiseCls
        {
            public String cnt_internalId { get; set; }
            public String SalesManName { get; set; }
            public String EF { get; set; }
        }

        public class AnalyticPageloadcls
        {
            public String FromDateEF { get; set; }
            public String TodateEF { get; set; }
        } 
    }
}