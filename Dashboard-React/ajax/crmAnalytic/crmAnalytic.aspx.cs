using Dashboard_React.Models;
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
    public partial class crmAnalytic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getPageload()
        {
            AnalyticPageloadcls cls = new AnalyticPageloadcls();
            ProcedureExecute proc1 = new ProcedureExecute("prc_crmDb");
            proc1.AddVarcharPara("@Action", 100, "GetmaxActHistory");
            DataTable maxTable = proc1.GetTable();
            if (maxTable.Rows[0][0] != null)
                cls.FromDateActHis = Convert.ToDateTime(maxTable.Rows[0][0]).ToString("dd-MM-yyyy");
            else
                cls.FromDateActHis = DateTime.Now.Date.ToString("dd-MM-yyyy");


            cls.TodateActHis = DateTime.Now.ToString("dd-MM-yyyy");
            cls.FromDateActHis = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");
            cls.TodateActHis = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");

            return cls;
        }
        [WebMethod]
        public static object GetCampaignCost(string frmdate, string todate, string smanId,  string LastCount,  string ActivityType )
        {
            //DateTime toDate1 = new DateTime();
            //toDate1 = TodateActHis.Date;
            //toDate1 = toDate1.Date.AddHours(23);
            ProcedureExecute proc = new ProcedureExecute("prc_ActHistory");
            proc.AddPara("@Fromdate", frmdate);
            proc.AddPara("@Todate", todate);
            proc.AddPara("@Userid", HttpContext.Current.Session["userid"]);
            proc.AddPara("@smanId", smanId);
            proc.AddPara("@LastCount", LastCount);
            proc.AddPara("@ActivityType", ActivityType);
            proc.RunActionQuery();


            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DashBoardDataContext dbcontext = new DashBoardDataContext(connectionString);
            List<tbl_CRMDb_ActHistory> cl = new List<tbl_CRMDb_ActHistory>();
            var obj = (from d in dbcontext.tbl_CRMDb_ActHistories
                       where d.UserId == Convert.ToInt32(HttpContext.Current.Session["userid"])
                       select d);
            cl=obj.ToList();
            return cl;      
        }


        public class AnalyticPageloadcls
        {
            public String FromDateActHis { get; set; }
            public String TodateActHis { get; set; }
            public String FromDateActHisMinDate { get; set; }
            public String TodateActHisMinDate { get; set; }
        }  
    }
}