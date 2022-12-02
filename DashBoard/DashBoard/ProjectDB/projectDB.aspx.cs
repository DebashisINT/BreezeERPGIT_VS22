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

namespace DashBoard.DashBoard.ProjectDB
{
    public partial class projectDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "Project Management");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    LiProjectSummary.Visible = Convert.ToBoolean(dt.Rows[0]["pmsProjectSummary"]);
                    LiProjectDetails.Visible = Convert.ToBoolean(dt.Rows[0]["pmsProjectDetails"]);
                    LiTimeline.Visible = Convert.ToBoolean(dt.Rows[0]["pmsTimeline"]);
                    LiCostBreakup.Visible = Convert.ToBoolean(dt.Rows[0]["pmsCostBreakup"]);
                }
                else
                {
                    LiProjectSummary.Visible = Convert.ToBoolean(0);
                    LiProjectDetails.Visible = Convert.ToBoolean(0);
                    LiTimeline.Visible = Convert.ToBoolean(0);
                    LiCostBreakup.Visible = Convert.ToBoolean(0);
                }
            }
        }


        protected void EntityServerModeDataDashBoard_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            DashBoardDataContext dc = new DashBoardDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




            var q = from d in dc.V_ProjectList_DashBoards
                    where d.ProjectStatus == "Approved"
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        [WebMethod]
        public static object GetCustomer(string SearchKey)
        {
            List<DashBoardCustomerModel> listCust = new List<DashBoardCustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                // DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev
                DataTable cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");

                listCust = (from DataRow dr in cust.Rows
                            select new DashBoardCustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod]

        public static object GetProjSum(string toDate, string SearchKey, string partyId)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);
            SearchKey = SearchKey.Replace("'", "''");
            List<Efficency> lEfficency = new List<Efficency>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "ProjSum");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new Efficency()
                         {
                             initial_Est_Cost = Convert.ToString(dr["Initial Est. Cost"]),
                             Est_Revised = Convert.ToString(dr["Est. Revised Cost"]),
                             Order_Value = Convert.ToString(dr["Order Value"]),
                             Cost_Booked = Convert.ToString(dr["Cost Booked"]),
                             Est_Balance = Convert.ToString(dr["Est. Balance"]),
                             Initial_Revenue = Convert.ToString(dr["Initial Revenue"]),
                             Revised = Convert.ToString(dr["Revised Revenue"]),
                             Revenue_booked = Convert.ToString(dr["Revenue Booked"]),
                             Revenue_balance = Convert.ToString(dr["Revenue Balance"]),
                             Profit = Convert.ToString(dr["Est. Profit"]),
                             RevProfit = Convert.ToString(dr["Revenue Profit"])
                            //EF = Convert.ToString(dr["ActCount"]),
                            //  color = String.Format("#{0:X6}", random.Next(0x1000000))
                          }).ToList();
            return lEfficency;
                }

        [WebMethod]
        public static object GetProjDetail(string toDate, string SearchKey, string partyId)
        {


            List<procDetails> lEfficency = new List<procDetails>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "Project");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new procDetails()
                          {
                              New = Convert.ToString(dr["New"]),
                              Qualify = Convert.ToString(dr["Qualify"]),
                              Planning = Convert.ToString(dr["Planning"]),
                              Execution = Convert.ToString(dr["Execution"]),
                              Deliver = Convert.ToString(dr["Deliver"]),
                              Complete = Convert.ToString(dr["Complete"]),
                              Close = Convert.ToString(dr["Close"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object GetProjDetailBar(string toDate, string SearchKey, string partyId)
        {


            List<procDetailsBar> lEfficency = new List<procDetailsBar>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "ProjDetail");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new procDetailsBar()
                          {
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Name = Convert.ToString(dr["Proj_Name"]),
                              projStage_Desc = Convert.ToString(dr["projStage_Desc"]),
                              STAGESTATUS = Convert.ToString(dr["STAGESTATUS"]),
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object GetProjDetailSingle(string toDate, string SearchKey, string partyId, string Pid)
        {


            List<procDetailsTable> lEfficency = new List<procDetailsTable>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "ProjSubDetail");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);
            proc.AddVarcharPara("@SELECTPROJID ", 1000, Pid);
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new procDetailsTable()
                          {
                              PROJECT_ID = Convert.ToString(dr["PROJECT_ID"]),
                              PROJECT_CODE = Convert.ToString(dr["PROJECT_CODE"]),
                              PROJECT_NAME = Convert.ToString(dr["PROJECT_NAME"]),
                              PARTYID = Convert.ToString(dr["PARTYID"]),
                              CUSTOMERNAME = Convert.ToString(dr["CUSTOMERNAME"]),
                              PROJECTSTAGE = Convert.ToString(dr["PROJECTSTAGE"]),
                              PROJECT_MANAGER = Convert.ToString(dr["PROJECT_MANAGER"]),
                              HIERARCHY = Convert.ToString(dr["HIERARCHY"]),
                              PROJ_ESTLABOURCOST = Convert.ToString(dr["PROJ_ESTLABOURCOST"]),
                              PROJ_ESTEXPENSECOST = Convert.ToString(dr["PROJ_ESTEXPENSECOST"]),
                              PROJ_ESTTOTALCOST = Convert.ToString(dr["PROJ_ESTTOTALCOST"]),
                              PROJ_ESTIMATEHOURS = Convert.ToString(dr["PROJ_ESTIMATEHOURS"]),
                              PROJ_ESTIMATESTARTDATE = Convert.ToString(dr["PROJ_ESTIMATESTARTDATE"]),
                              PROJ_ACTUALLABOURCOST = Convert.ToString(dr["PROJ_ACTUALLABOURCOST"]),
                              PROJ_ACTUALEXPENSECOST = Convert.ToString(dr["PROJ_ACTUALEXPENSECOST"]),
                              PROJ_ACTUALHOURS = Convert.ToString(dr["PROJ_ACTUALHOURS"]),
                              PROJ_ACTUALSTARTDATE = Convert.ToString(dr["PROJ_ACTUALSTARTDATE"]),
                              PROJ_ACTUALENDDATE = Convert.ToString(dr["PROJ_ACTUALENDDATE"])
                              
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for timeline
        public static object GetTimeLineTble(string toDate, string SearchKey, string partyId)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);
            SearchKey = SearchKey.Replace("'", "''");
            List<timelineClass> lEfficency = new List<timelineClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "TimeLine");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new timelineClass()
                          {
                              PROJECT_ID = Convert.ToString(dr["PROJECT_ID"]),
                              PROJECT_CODE = Convert.ToString(dr["PROJECT_CODE"]),
                              PROJECT_NAME = Convert.ToString(dr["PROJECT_NAME"]),
                              PARTYID = Convert.ToString(dr["PARTYID"]),
                              CUSTOMERNAME = Convert.ToString(dr["CUSTOMERNAME"]),
                              PROJECTSTATUS = Convert.ToString(dr["PROJECTSTATUS"]),
                              PROJECTSTAGE = Convert.ToString(dr["PROJECTSTAGE"]),
                              PROJECT_MANAGER = Convert.ToString(dr["PROJECT_MANAGER"]),
                              HIERARCHY = Convert.ToString(dr["HIERARCHY"]),
                              PROJ_ESTLABOURCOST = Convert.ToString(dr["PROJ_ESTLABOURCOST"]),
                              PROJ_ESTEXPENSECOST = Convert.ToString(dr["PROJ_ESTEXPENSECOST"]),
                              PROJ_ESTTOTALCOST = Convert.ToString(dr["PROJ_ESTTOTALCOST"]),
                              PROJ_ESTIMATEHOURS = Convert.ToString(dr["PROJ_ESTIMATEHOURS"]),
                              PROJ_ESTIMATESTARTDATE = Convert.ToString(dr["PROJ_ESTIMATESTARTDATE"]),
                              PROJ_ESTIMATEENDDATE = Convert.ToString(dr["PROJ_ESTIMATEENDDATE"]),
                              PROJ_ACTUALLABOURCOST = Convert.ToString(dr["PROJ_ACTUALLABOURCOST"]),
                              PROJ_ACTUALEXPENSECOST = Convert.ToString(dr["PROJ_ACTUALEXPENSECOST"]),
                              PROJ_ACTUALHOURS = Convert.ToString(dr["PROJ_ACTUALHOURS"]),
                              PROJ_ACTUALSTARTDATE = Convert.ToString(dr["PROJ_ACTUALSTARTDATE"]),
                              PROJ_ACTUALENDDATE = Convert.ToString(dr["PROJ_ACTUALENDDATE"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //for timeline
        public static object GetCostBreakUp(string toDate, string SearchKey, string partyId)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);
            SearchKey = SearchKey.Replace("'", "''");
            List<costBreakupClass> lEfficency = new List<costBreakupClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "ICostBreakup");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new costBreakupClass()
                          {
                              Initial_Material_Cost = Convert.ToString(dr["Initial Material Cost"]),
                              Initial_Service_Cost = Convert.ToString(dr["Initial Service Cost"])
                             
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for timeline
        public static object GetCostBreakUpR(string toDate, string SearchKey, string partyId)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);
            SearchKey = SearchKey.Replace("'", "''");
            List<costBreakupClassR> lEfficency = new List<costBreakupClassR>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTMGMTDASHBOARD_REPORT");
            proc.AddVarcharPara("@Action", 100, "RCostBreakup");
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ASONDATE", 400, toDate);
            proc.AddVarcharPara("@PARTY_ID", 400, partyId);
            proc.AddVarcharPara("@PROJECT_ID", 1000, SearchKey);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new costBreakupClassR()
                          {
                              Revised_Material_Cost = Convert.ToString(dr["Revised Material Cost"]),
                              Revised_Service_Cost = Convert.ToString(dr["Revised Service Cost"])

                          }).ToList();
            return lEfficency;
        }
         [WebMethod]
        public static Object getGranttData(string id)
        {
            List<GranttChartDataClass> dtOutput = getGranttDatas(id);
            return dtOutput;
        }



         internal static List<GranttChartDataClass> getGranttDatas(string id)
         {

             Dictionary<int, string> My_dict1 =
                        new Dictionary<int, string>();

             My_dict1.Add(1, "gtaskgreen");
             My_dict1.Add(2, "gtaskyellow");
             My_dict1.Add(3, "gtaskred");
             My_dict1.Add(4, "gtaskblue");
             My_dict1.Add(5, "gtaskgray");
             My_dict1.Add(6, "gtaskaliceblue");
             My_dict1.Add(7, "gtaskblack");
             My_dict1.Add(8, "gtaskpink");


             int i = 0;
             List<GranttChartDataClass> objlstGranttChartDetails = new List<GranttChartDataClass>();
             ProcedureExecute proc = new ProcedureExecute("prc_WBS_GranttChart");
             proc.AddVarcharPara("@WBS_Id", 10, id);
             DataTable dtout = proc.GetTable();
             if (dtout != null && dtout.Rows.Count > 0)
             {
                 foreach (DataRow dr in dtout.Rows)
                 {

                     if (i > 8)
                     {
                         i = 1;
                     }
                     else
                     {
                         i = i + 1;
                     }
                     string val = My_dict1[i];

                     GranttChartDataClass objGranttChartDetails = new GranttChartDataClass();
                     objGranttChartDetails.pID = Convert.ToString(dr["pID"]);
                     objGranttChartDetails.pName = Convert.ToString(dr["pName"]);
                     objGranttChartDetails.pStart = Convert.ToString(dr["pStart"]);
                     objGranttChartDetails.pEnd = Convert.ToString(dr["pEnd"]);

                     objGranttChartDetails.pPlanStart = Convert.ToString(dr["pStart"]);
                     objGranttChartDetails.pPlanEnd = Convert.ToString(dr["pEnd"]);
                     objGranttChartDetails.pClass = val;

                     objGranttChartDetails.pLink = Convert.ToString("");
                     objGranttChartDetails.pMile = Convert.ToString("");
                     objGranttChartDetails.pRes = Convert.ToString("");
                     objGranttChartDetails.pComp = Convert.ToString("");
                     objGranttChartDetails.pGroup = Convert.ToString("");


                     objGranttChartDetails.pParent = Convert.ToString(dr["pParent"]);
                     objGranttChartDetails.pOpen = "1";
                     objGranttChartDetails.pDepend = Convert.ToString(dr["pDepend"]);
                     objGranttChartDetails.pCaption = Convert.ToString(dr["pLabel"]);

                     objGranttChartDetails.pNotes = Convert.ToString("");
                     objGranttChartDetails.category = Convert.ToString("");
                     objGranttChartDetails.sector = Convert.ToString("");


                     objlstGranttChartDetails.Add(objGranttChartDetails);

                 }
             }

             return objlstGranttChartDetails;


         }

       }


    public class GranttChartDataClass
    {
        public string pID { get; set; }
        public string pName { get; set; }
        public string pStart { get; set; }
        public string pEnd { get; set; }
        public string pPlanStart { get; set; }
        public string pPlanEnd { get; set; }
        public string pClass { get; set; }

        public string pLink { get; set; }
        public string pMile { get; set; }
        public string pRes { get; set; }
        public string pComp { get; set; }
        public string pGroup { get; set; }
        public string pParent { get; set; }
        public string pOpen { get; set; }
        public string pDepend { get; set; }
        public string pCaption { get; set; }
        public string pNotes { get; set; }
        public string category { get; set; }
        public string sector { get; set; }

    }
    public class costBreakupClass
    {
        public string Initial_Material_Cost { get; set; }
        public string Initial_Service_Cost { get; set; }
    }
    public class costBreakupClassR
    {
        public string Revised_Material_Cost { get; set; }
        public string Revised_Service_Cost { get; set; }
    } 
    public class timelineClass
    {
        public string PROJECT_ID { get; set; }
        public string PROJECT_CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public string PARTYID { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string PROJECTSTATUS { get; set; }
        public string PROJECTSTAGE { get; set; }
        public string PROJECT_MANAGER { get; set; }
        public string HIERARCHY { get; set; }
        public string PROJ_ESTLABOURCOST { get; set; }
        public string PROJ_ESTEXPENSECOST { get; set; }
        public string PROJ_ESTTOTALCOST { get; set; }
        public string PROJ_ESTIMATEHOURS { get; set; }
        public string PROJ_ESTIMATESTARTDATE { get; set; }
        public string PROJ_ESTIMATEENDDATE { get; set; }
        public string PROJ_ACTUALLABOURCOST { get; set; }
        public string PROJ_ACTUALEXPENSECOST { get; set; }
        public string PROJ_ACTUALHOURS { get; set; }
        public string PROJ_ACTUALSTARTDATE { get; set; }
        public string PROJ_ACTUALENDDATE { get; set; }
    }

    public class procDetailsTable
    {
        public string PROJECT_ID { get; set; }
        public string PROJECT_CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public string PARTYID { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string PROJECTSTAGE { get; set; }
        public string PROJECT_MANAGER { get; set; }
        public string HIERARCHY { get; set; }
        public string PROJ_ESTLABOURCOST { get; set; }
        public string PROJ_ESTEXPENSECOST { get; set; }
        public string PROJ_ESTTOTALCOST { get; set; }
        public string PROJ_ESTIMATEHOURS { get; set; }
        public string PROJ_ESTIMATESTARTDATE { get; set; }
        public string PROJ_ACTUALLABOURCOST { get; set; }
        public string PROJ_ACTUALEXPENSECOST { get; set; }
        public string PROJ_ACTUALHOURS { get; set; }
        public string PROJ_ACTUALSTARTDATE { get; set; }
        public string PROJ_ACTUALENDDATE { get; set; }
            
    }
    public class procDetailsBar
    {
        public string Proj_Id { get; set; }
        public string Proj_Name { get; set; }
        public string projStage_Desc { get; set; }
        public string STAGESTATUS { get; set; }

    }
    public class Efficency
    {
        public string initial_Est_Cost { get; set; }
        public string Est_Revised { get; set; }
        public string Order_Value { get; set; }
        public string Cost_Booked { get; set; }
        public string Est_Balance { get; set; }
        public string Initial_Revenue { get; set; }
        public string Revised { get; set; }
        public string Revenue_booked { get; set; }
        public string Revenue_balance { get; set; }
        public string Profit { get; set; }
        public string RevProfit { get; set; }
    }
    
       

        public class procDetails
        {
            public string New { get; set; }
            public string Qualify { get; set; }
            public string Planning { get; set; }
            public string Execution { get; set; }
            public string Deliver { get; set; }
            public string Complete { get; set; }
            public string Close { get; set; }
            
        }
        public class DashBoardCustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string add { get; set; }
        }

    
}