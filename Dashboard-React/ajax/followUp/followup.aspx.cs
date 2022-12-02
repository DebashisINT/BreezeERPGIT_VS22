using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.followUp
{
    public partial class followup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getGrid(string fromdate, string todate)
        {
            List<GridClass> cls = new List<GridClass>();
            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "totalFollowup");
            proc.AddPara("@FromDate", fromdate);
            proc.AddPara("@Todate", todate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData!=null && CallcountData.Rows.Count>0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new GridClass()
                        {
                            cnt = Convert.ToString(dr["cnt"]),
                            CustID = Convert.ToString(dr["CustID"]),
                            Document = Convert.ToString(dr["Document"]),
                            name = Convert.ToString(dr["name"]),
                            user_name = Convert.ToString(dr["user_name"])
                        }).ToList();
            }
            return cls;
        }

        [WebMethod]
        public static object getPageload()
        {
            Pageloadcls cls = new Pageloadcls();
            ProcedureExecute proc1 = new ProcedureExecute("Prc_followupDb");
            proc1.AddVarcharPara("@Action", 100, "GetMaxFollowDate");
            DataTable maxTable = proc1.GetTable();
            if ((!(maxTable.Rows[0][0] is DBNull)) && (maxTable.Rows[0][0] != null))
                cls.allFormDate = Convert.ToDateTime(maxTable.Rows[0][0]).ToString("dd-MM-yyyy");
            else
                cls.allFormDate = DateTime.Now.Date.ToString("dd-MM-yyyy");

            cls.alltoDate = DateTime.Now.ToString("dd-MM-yyyy");
            cls.allFormDateMinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");
            cls.alltoDateMinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1).ToString("dd-MM-yyyy");

            return cls;
        }


        [WebMethod]
        public static object getPendingFollowup(string fromdate)
        {
            List<PendingFollowUp> cls = new List<PendingFollowUp>();
            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "GetPendingAct");
            proc.AddPara("@FromDate", fromdate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new PendingFollowUp()
                       {
                           cnt = Convert.ToString(dr["cnt"]),
                           CustID = Convert.ToString(dr["CustID"]),
                           name = Convert.ToString(dr["name"]),
                          
                       }).ToList();
            }
            return cls;
        }
        [WebMethod]
        public static object getClosedFollowup(string fromdate, string todate)
        {
            List<ColsedFollowUp> cls = new List<ColsedFollowUp>();
            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "GetClosedCount");
            proc.AddPara("@FromDate", fromdate);
            proc.AddPara("@Todate", todate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new ColsedFollowUp()
                       {
                           followedBy = Convert.ToString(dr["followedBy"]),
                           user_name = Convert.ToString(dr["user_name"]),
                           cnt = Convert.ToString(dr["cnt"]),
                           followedByname = Convert.ToString(dr["followedByname"]),
                           total = Convert.ToString(dr["total"]),
                           ratio = Convert.ToString(dr["ratio"]),
                       }).ToList();
            }
            return cls;
        }
        [WebMethod]
        public static object getRationuc(string fromdate, string todate)
        {
            List<RationUc> cls = new List<RationUc>();
            ProcedureExecute proc = new ProcedureExecute("Prc_followupDb");
            proc.AddVarcharPara("@Action", 100, "Conversion");
            proc.AddPara("@FromDate", fromdate);
            proc.AddPara("@Todate", todate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new RationUc()
                       {
                           name = Convert.ToString(dr["name"]),
                           CustID = Convert.ToString(dr["CustID"]),
                           Ratio = Convert.ToString(dr["Ratio"]),
                          
                       }).ToList();
            }
            return cls;
        }

        [WebMethod]
        public static object getfollowpHistory(string frmdate, string todate)
        {
            List<flhistory> cls = new List<flhistory>();
            ProcedureExecute proc = new ProcedureExecute("fl_history");
            proc.AddPara("@fromdate", frmdate);
            proc.AddPara("@todate", todate);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new flhistory()
                       {
                           id = Convert.ToString(dr["id"]),
                           user_name = Convert.ToString(dr["user_name"]),
                           FollowDate = Convert.ToString(dr["FollowDate"]),
                           FollowUsing = Convert.ToString(dr["FollowUsing"]),
                           Document = Convert.ToString(dr["Document"]),
                           DocDate = Convert.ToString(dr["DocDate"]),
                           name = Convert.ToString(dr["name"]),
                           openClsoe = Convert.ToString(dr["openClsoe"]),
                           NextFollowDate = Convert.ToString(dr["NextFollowDate"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                       }).ToList();
            }
            return cls;
        }


        [WebMethod]
        public static object getfl_using(string frmdate, string todate, string value)
        {
            List<flhistory> cls = new List<flhistory>();
            ProcedureExecute proc = new ProcedureExecute("fl_using");
            proc.AddPara("@fromdate", frmdate);
            proc.AddPara("@todate", todate);
            proc.AddPara("@val", value);
            DataTable CallcountData = proc.GetTable();
            if (CallcountData != null && CallcountData.Rows.Count > 0)
            {
                cls = (from DataRow dr in CallcountData.Rows
                       select new flhistory()
                       {
                           id = Convert.ToString(dr["id"]),
                           user_name = Convert.ToString(dr["user_name"]),
                           FollowDate = Convert.ToString(dr["FollowDate"]),
                           FollowUsing = Convert.ToString(dr["FollowUsing"]),
                           Document = Convert.ToString(dr["Document"]),
                           DocDate = Convert.ToString(dr["DocDate"]),
                           name = Convert.ToString(dr["name"]),
                           openClsoe = Convert.ToString(dr["openClsoe"]),
                           NextFollowDate = Convert.ToString(dr["NextFollowDate"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                       }).ToList();
            }
            return cls;
        }

        public class flhistory
        {
         public string   id	{ get; set; }
         public string   user_name	{ get; set; }
         public string   FollowDate	{ get; set; }
         public string   FollowUsing	{ get; set; }
         public string   Document	{ get; set; }
         public string   DocDate	{ get; set; }
         public string   name	{ get; set; }
         public string   openClsoe	{ get; set; }
         public string   NextFollowDate	{ get; set; }
         public string   Remarks { get; set; }
        }

        public class RationUc
        {
            public string CustID { get; set; }
            public string name { get; set; }
            public string Ratio { get; set; }
        }
    public class ColsedFollowUp{
        public string followedBy { get; set; }	
        public string user_name	{ get; set; }
        public string cnt	{ get; set; }
        public string followedByname { get; set; }	
        public string total	 { get; set; }
        public string ratio { get; set; }
    }
        public class PendingFollowUp
        {
            public string CustID { get; set; }
            public string name { get; set; }
            public string cnt { get; set; }
        }
        public class GridClass
        {
            public string CustID { get; set; }
            public string name { get; set; }
            public string user_name { get; set; }
            public string Document { get; set; }
            public string cnt { get; set; }
        }

        public class Pageloadcls
        {
            public String allFormDate { get; set; }
            public String alltoDate { get; set; }
            public String allFormDateMinDate { get; set; }
            public String alltoDateMinDate { get; set; }
        }
    }
}