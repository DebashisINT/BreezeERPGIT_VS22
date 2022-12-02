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
    public partial class newVrepeat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object GetNewVrepeat(string fromdate, string prodClass)
        {
            List<GetAcSalesmanWiseCls> cls = new List<GetAcSalesmanWiseCls>();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetNewVsOldCount");
            proc.AddPara("@FromDate", fromdate);
            proc.AddPara("@prodOrClass", prodClass);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new GetAcSalesmanWiseCls()
                       {
                           New = Convert.ToString(dr["New"]),
                           Regular = Convert.ToString(dr["Regular"]),
                           SalesManName = Convert.ToString(dr["SalesManName"]),
                           CreatedBy = Convert.ToString(dr["CreatedBy"]),
                       }).ToList();
            }
            return cls;
        }	
        public class GetAcSalesmanWiseCls
        {
            public String New { get; set; }
            public String Regular { get; set; }
            public String SalesManName { get; set; }
            public String CreatedBy { get; set; }
        }
    }
}