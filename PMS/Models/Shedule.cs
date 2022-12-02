using DataAccessLayer;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class Shedules
    {
        public string Delete { get; set; }
        public string Slno { get; set; }
        public string ActivityName { get; set; }
        public string Effort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Predecessor { get; set; }
        public string Duration { get; set; }
        public string Resources { get; set; }
        public string Description { get; set; }
        public string AddNew { get; set; }
        public string UpdateEdit { get; set; }
        public string ParentId { get; set; }



        public DataSet DropDownDetailForEstimate(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID", branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "WBS");
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable GetProjectCode( String BRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECTALL");
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            ds = proc.GetTable();
            return ds;
        }


        public string SaveSchedule(WBSInput input,DataTable dtSchedule,DataTable dtResource)
        {


            if (dtSchedule.Columns.Contains("Resources"))
            {
                dtSchedule.Columns.Remove("Resources");
            }
            if (dtSchedule.Columns.Contains("UpdateEdit"))
            {
                dtSchedule.Columns.Remove("UpdateEdit");
            }



            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_PMSWBS_AddEdit", con);
            DataTable NoteTable = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTION", input.Action);
            cmd.Parameters.AddWithValue("@WBS_ID", input.WBS_ID);
            cmd.Parameters.AddWithValue("@WBS_Name", input.WBS_Name);
            cmd.Parameters.AddWithValue("@WBS_HierarchyId",  input.WBS_HierarchyId);
            cmd.Parameters.AddWithValue("@WBS_Branch",  input.WBS_Branch);
            cmd.Parameters.AddWithValue("@WBS_StartDate",  input.WBS_StartDate);
            cmd.Parameters.AddWithValue("@WBS_EndTime", input.WBS_EndTime);
            cmd.Parameters.AddWithValue("@WBS_Duration",  input.WBS_Duration);
            cmd.Parameters.AddWithValue("@WBS_Description", input.WBS_Description);
            cmd.Parameters.AddWithValue("@WBS_Workunit", input.WBS_Workunit);
            cmd.Parameters.AddWithValue("@WBS_ProjectCode",  input.WBS_ProjectCode);
            cmd.Parameters.AddWithValue("@ResourceDetails", dtResource);
            cmd.Parameters.AddWithValue("@ScheduleDetails", dtSchedule);
            cmd.Parameters.AddWithValue("@WBS_effort", input.WBS_Effort);
            cmd.Parameters.AddWithValue("@Create_By", Convert.ToString(HttpContext.Current.Session["userid"]));
            SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputText);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            string ReturnText = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
            return ReturnText;




        }


        internal DataSet GetWBSDataSet(string Doc_Id)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_WBS_Details");
            proc.AddVarcharPara("@ACTION", 500, "GetGridData");
            proc.AddVarcharPara("@WBS_Id", 10, Doc_Id);
            return proc.GetDataSet();
        }

        internal DataTable GetHeaderData(string id)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_WBS_Details");
            proc.AddVarcharPara("@ACTION", 500, "GetHeaderData");
            proc.AddVarcharPara("@WBS_Id", 10, id);
            return proc.GetTable();
        }

        internal string DeleteWBS(string id)
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_PMSWBS_AddEdit", con);
            DataTable NoteTable = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTION", "Delete");
            cmd.Parameters.AddWithValue("@WBS_Id", id);
            SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputText);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            string ReturnText = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
            return ReturnText;
        }

        internal List<GranttChartDetails> getGranttData(string id)
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
            List<GranttChartDetails> objlstGranttChartDetails = new List<GranttChartDetails>();
            ProcedureExecute proc = new ProcedureExecute("prc_WBS_GranttChart");
            proc.AddVarcharPara("@WBS_Id", 10, id);
            DataTable dtout= proc.GetTable();
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
                                   
                    GranttChartDetails objGranttChartDetails = new GranttChartDetails();
                    objGranttChartDetails.pID = Convert.ToString(dr["pID"]);
                    objGranttChartDetails.pName = Convert.ToString(dr["pName"]);
                    objGranttChartDetails.pStart = Convert.ToString(dr["pStart"]);
                    objGranttChartDetails.pEnd = Convert.ToString(dr["pEnd"]);

                    objGranttChartDetails.pPlanStart = Convert.ToString("");
                    objGranttChartDetails.pPlanEnd = Convert.ToString("");
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
    public class gnattChart
    {
        public string name { get; set; }
        public List<GranttChartDetails> values { get; set; }

    }

    public class GranttChartDetails
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



    public class WBSModel
    {
        public List<BranchUnit> UnitList { get; set; }
        public string Unit { get; set; }

        public string WBS_Id { get; set; }
        public string WBS_Code { get; set; }


        public string WBS_Name { get; set; }
        public string WBS_WorkUnit { get; set; }

        public string WBS_StartDate { get; set; }

        public string WBS_EndDate { get; set; }




    }

    public class WBSInput
    {
        public List<BranchUnit> UnitList { get; set; }
        public string Action { get; set; }
        public string       WBS_Name             { get; set; }
        public int          WBS_ID                { get; set; }
        public string       WBS_Code { get; set; }
        public int          WBS_HierarchyId       { get; set; }
        public string       WBS_Branch            { get; set; }
        public DateTime     WBS_StartDate         { get; set; }
        public DateTime     WBS_EndTime           { get; set; }
        public string       WBS_Duration          { get; set; }
        public string       WBS_Description       { get; set; }
        public string       WBS_Workunit          { get; set; }
        public string       WBS_ProjectCode       { get; set; }
        public string WBS_Effort { get; set; }
        public string Isdelete { get; set; }
        public List<HierarchyList> Hierarchy_List { get; set; }

        public String Hierarchy { get; set; }
    }              
}                  