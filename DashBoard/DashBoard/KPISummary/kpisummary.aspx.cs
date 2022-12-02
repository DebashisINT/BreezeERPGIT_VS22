using BusinessLogicLayer;
using DashBoard.DashBoard.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.KPISummary
{
    public partial class kpisummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "KPI Summary");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    liPerformance.Visible = Convert.ToBoolean(dt.Rows[0]["KPIPerformance"]);
                    liActivities.Visible = Convert.ToBoolean(dt.Rows[0]["KPIActivities"]);
                    liEmployeeInfo.Visible = Convert.ToBoolean(dt.Rows[0]["KPIEmployeeInfo"]);
                    liResolution.Visible = Convert.ToBoolean(dt.Rows[0]["KPIResolution"]);
                }
                else
                {
                    liPerformance.Visible = Convert.ToBoolean(0);
                    liActivities.Visible = Convert.ToBoolean(0);
                    liEmployeeInfo.Visible = Convert.ToBoolean(0);
                    liResolution.Visible = Convert.ToBoolean(0);
                }
            }
        }

        [WebMethod]
        public static object GetPerformanceTopBox(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            boxData lEfficency = new boxData();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "Performance");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataSet CallData = proc.GetDataSet();
            var random = new Random();
            var one = (from DataRow dr in CallData.Tables[0].Rows
                       select new performanceBoxesOne()
                       {
                           LDCNT = Convert.ToString(dr["LDCNT"]),
                           LDTOTAMT = Convert.ToString(dr["LDTOTAMT"])

                       }).ToList();
            var two = (from DataRow dr in CallData.Tables[1].Rows
                       select new performanceBoxesTwo()
                       {
                           INQCNT = Convert.ToString(dr["INQCNT"]),
                           INQTOTAMT = Convert.ToString(dr["INQTOTAMT"])

                       }).ToList();
            var three = (from DataRow dr in CallData.Tables[2].Rows
                         select new performanceBoxesThree()
                         {
                             QOCNT = Convert.ToString(dr["QOCNT"]),
                             QOTOTAMT = Convert.ToString(dr["QOTOTAMT"])

                         }).ToList();
            var four = (from DataRow dr in CallData.Tables[3].Rows
                        select new performanceBoxesFour()
                        {
                            SOCNT = Convert.ToString(dr["SOCNT"]),
                            SOTOTAMT = Convert.ToString(dr["SOTOTAMT"])

                        }).ToList();
            var five = (from DataRow dr in CallData.Tables[4].Rows
                        select new performanceBoxesFive()
                        {
                            SICNT = Convert.ToString(dr["SICNT"]),
                            SITOTAMT = Convert.ToString(dr["SITOTAMT"])

                        }).ToList();
            var six = (from DataRow dr in CallData.Tables[5].Rows
                       select new performanceBoxesSix()
                       {
                           CRPCNT = Convert.ToString(dr["CRPCNT"]),
                           CRPTOTAMT = Convert.ToString(dr["CRPTOTAMT"])

                       }).ToList();
            lEfficency.one = one;
            lEfficency.two = two;
            lEfficency.three = three;
            lEfficency.four = four;
            lEfficency.five = five;
            lEfficency.six = six;
            return lEfficency;
        }
        [WebMethod]
        public static object GetEmplyee(string branchid)
        {
            DataTable CallData = new DataTable();

            List<employeeClass> empList = new List<employeeClass>();

            DBEngine obj = new DBEngine();

            if (!string.IsNullOrEmpty(branchid))
                CallData = obj.GetDataTable("select cnt_internalId EMP_CODE,RTRIM(LTRIM(CONCAT(LTRIM(COALESCE(CNT.cnt_firstName + ' ', '')), LTRIM(COALESCE(CNT.cnt_middleName + ' ', ''))	, COALESCE(CNT.cnt_lastName, '')))) EMP_NAME from tbl_master_contact cnt where cnt_contactType='EM' and cnt_branchid='" + branchid + "'");

            else
                CallData = obj.GetDataTable("select cnt_internalId EMP_CODE,RTRIM(LTRIM(CONCAT(LTRIM(COALESCE(CNT.cnt_firstName + ' ', '')), LTRIM(COALESCE(CNT.cnt_middleName + ' ', ''))	, COALESCE(CNT.cnt_lastName, '')))) EMP_NAME from tbl_master_contact cnt where cnt_contactType='EM'");

            empList = (from DataRow dr in CallData.Rows
                       select new employeeClass()
                       {
                           EMP_CODE = Convert.ToString(dr["EMP_CODE"]),
                           EMP_NAME = Convert.ToString(dr["EMP_NAME"])

                       }).ToList();
            return empList;
        }

        [WebMethod]
        //for PInquery
        public static object GetPInquery(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<PInqueryClass> lEfficency = new List<PInqueryClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "PInquery");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new PInqueryClass()
                          {
                              NEW = Convert.ToString(dr["NEW"]),
                              BACKLOG = Convert.ToString(dr["BACKLOG"])

                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for PQuotation
        public static object GetPQuotation(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<PInqueryClass> lEfficency = new List<PInqueryClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "PQuotation");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new PInqueryClass()
                          {
                              NEW = Convert.ToString(dr["NEW"]),
                              BACKLOG = Convert.ToString(dr["BACKLOG"])

                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for POrder
        public static object GetPOrder(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<PInqueryClass> lEfficency = new List<PInqueryClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "POrder");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new PInqueryClass()
                          {
                              NEW = Convert.ToString(dr["NEW"]),
                              BACKLOG = Convert.ToString(dr["BACKLOG"])

                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for PSInv
        public static object GetPSInv(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<PInqueryClass> lEfficency = new List<PInqueryClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "PSInv");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new PInqueryClass()
                          {
                              NEW = Convert.ToString(dr["NEW"]),
                              BACKLOG = Convert.ToString(dr["BACKLOG"])

                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Lead
        public static object GetPLead(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<PLeadClass> lEfficency = new List<PLeadClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "PLead");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new PLeadClass()
                          {
                              LDSTATUS = Convert.ToString(dr["LDSTATUS"]),
                              LDCNT = Convert.ToString(dr["LDCNT"]),
                              LDTOTAMT = Convert.ToString(dr["LDTOTAMT"])

                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //for Activities
        public static object GetActivitiesBox(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<ActBoxClass> lEfficency = new List<ActBoxClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "Activities");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new ActBoxClass()
                          {
                              ACTIVCNT = Convert.ToString(dr["ACTIVCNT"]),
                              EMAILCNT = Convert.ToString(dr["EMAILCNT"]),
                              CALLSMSCNT = Convert.ToString(dr["CALLSMSCNT"]),
                              VISITCNT = Convert.ToString(dr["VISITCNT"]),
                              SOCIALCNT = Convert.ToString(dr["SOCIALCNT"]),
                              OTHERSCNT = Convert.ToString(dr["OTHERSCNT"])

                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Activities
        public static object GetTransacVolume(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<TransacVolumeClass> lEfficency = new List<TransacVolumeClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "TransacVolume");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TransacVolumeClass()
                          {
                              DOCTYPE = Convert.ToString(dr["DOCTYPE"]),
                              TODAYCNT = Convert.ToString(dr["TODAYCNT"]),
                              TOTAL = Convert.ToString(dr["TOTAL"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Activities
        public static object GetTaskVolume(string ASONDATE, string BRANCH_ID, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<TaskVolumeClass> lEfficency = new List<TaskVolumeClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "TaskVolume");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, BRANCH_ID);
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TaskVolumeClass()
                          {
                              TOPIC = Convert.ToString(dr["TOPIC"]),
                              POINT = Convert.ToString(dr["POINT"]),
                              RATING = Convert.ToString(dr["RATING"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for PInquery
        public static object GetEmployeeTab(string ASONDATE, string EMPID)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(ASONDATE);
            toDate1 = toDate1.Date.AddHours(23);

            List<EmplyeeTabClass> lEfficency = new List<EmplyeeTabClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_KPISUMMARYDB_REPORT");
            proc.AddVarcharPara("@Action", 100, "EmpInfo");
            proc.AddVarcharPara("@ASONDATE", 400, ASONDATE);
            proc.AddVarcharPara("@BRANCH_ID", 400, "");
            proc.AddVarcharPara("@EMPID", 1000, EMPID);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new EmplyeeTabClass()
                          {
                              WORKINGDAYS = Convert.ToString(dr["WORKINGDAYS"]),
                              PRESENTS = Convert.ToString(dr["PRESENTS"]),
                              LEAVES = Convert.ToString(dr["LEAVES"]),
                              HALFDAYS = Convert.ToString(dr["HALFDAYS"]),
                              EMPCTC = Convert.ToString(dr["EMPCTC"]),
                              EXPAMT = Convert.ToString(dr["EXPAMT"])
                          }).ToList();
            return lEfficency;
        }
    }			
    public class EmplyeeTabClass
    {
        public string WORKINGDAYS { get; set; }
        public string PRESENTS { get; set; }
        public string LEAVES { get; set; }
        public string HALFDAYS { get; set; }
        public string EMPCTC { get; set; }
        public string EXPAMT { get; set; }
    }
    public class TransacVolumeClass
    {
        public string DOCTYPE { get; set; }
        public string TODAYCNT { get; set; }
        public string TOTAL { get; set; }

    }
    public class TaskVolumeClass
    {
        public string TOPIC { get; set; }
        public string POINT { get; set; }
        public string RATING { get; set; }

    }
    public class ActBoxClass
    {
        public string ACTIVCNT { get; set; }
        public string EMAILCNT { get; set; }
        public string CALLSMSCNT { get; set; }
        public string VISITCNT { get; set; }
        public string SOCIALCNT { get; set; }
        public string OTHERSCNT { get; set; }

    }
    public class PLeadClass
    {
        public string LDSTATUS { get; set; }
        public string LDCNT { get; set; }
        public string LDTOTAMT { get; set; }

    }
    public class PInqueryClass
    {
        public string NEW { get; set; }
        public string BACKLOG { get; set; }

    }
    public class boxData
    {
        public List<performanceBoxesOne> one { get; set; }
        public List<performanceBoxesTwo> two { get; set; }
        public List<performanceBoxesThree> three { get; set; }
        public List<performanceBoxesFour> four { get; set; }
        public List<performanceBoxesFive> five { get; set; }
        public List<performanceBoxesSix> six { get; set; }

    }

    public class employeeClass
    {
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }

    }
    public class performanceBoxesOne
    {
        public string LDCNT { get; set; }
        public string LDTOTAMT { get; set; }

    }
    public class performanceBoxesTwo
    {
        public string INQCNT { get; set; }
        public string INQTOTAMT { get; set; }

    }
    public class performanceBoxesThree
    {
        public string QOCNT { get; set; }
        public string QOTOTAMT { get; set; }
    }
    public class performanceBoxesFour
    {
        public string SOCNT { get; set; }
        public string SOTOTAMT { get; set; }

    }
    public class performanceBoxesFive
    {
        public string SICNT { get; set; }
        public string SITOTAMT { get; set; }

    }
    public class performanceBoxesSix
    {
        public string CRPCNT { get; set; }
        public string CRPTOTAMT { get; set; }
    }
}