using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using PMS.Model;


namespace BusinessLogicLayer.PMS
{
    public class TimeSheetBL
    {

        public DataSet DropDownDetailForTimeSheet()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_TimeSheet_Details");
            proc.AddNVarcharPara("@Action", 100, "GetAllLoadDetais");
            proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));           
            //proc.AddNVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable BindProjectByBranchID(string BranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_TimeSheet_Details");
            proc.AddNVarcharPara("@Action", 100, "BindProjectByBranchID");
            proc.AddNVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable BindProjectTaskByProjectID(string ProjectID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_TimeSheet_Details");
            proc.AddNVarcharPara("@Action", 100, "BindProjectTaskByProjectID");
            proc.AddNVarcharPara("@Proj_Id", 100, ProjectID);
            ds = proc.GetTable();
            return ds;
        }
        public void AddEditTimeSheet(string TimeSheetID,DateTime startDate,string Duration, string Time_Type, string Time_Project, string Time_ProjectTask, string Time_Roll, string txtDescription, string txtExternalComments, string Action_type, string BranchID,ref int ReturnCode, ref string ReturnMsg)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);                

                ProcedureExecute proc = new ProcedureExecute("PRC_TIMESHEET_ADDEDIT");
                proc.AddNVarcharPara("@Action", 10, Action_type);
                proc.AddNVarcharPara("@TimeSheetID", 100, TimeSheetID);
                proc.AddNVarcharPara("@StartDate", 100, Convert.ToString(startDate));
                proc.AddNVarcharPara("@Duration", 100, Duration);
                proc.AddNVarcharPara("@Time_Type", 100, Time_Type);
                proc.AddNVarcharPara("@Time_Project", 100, Time_Project);
                proc.AddNVarcharPara("@Time_ProjectTask", 100, Time_ProjectTask);
                proc.AddNVarcharPara("@Time_Roll", 100, Time_Roll);
                proc.AddNVarcharPara("@Description", 500, txtDescription);
                proc.AddNVarcharPara("@ExternalComments", 500, txtExternalComments);
                proc.AddNVarcharPara("@UserID", 10, Convert.ToString(user_id));               
                proc.AddNVarcharPara("@FinYear", 50,HttpContext.Current.Session["LastFinYear"].ToString());
                proc.AddNVarcharPara("@CompanyID", 50,HttpContext.Current.Session["LastCompany"].ToString());
                proc.AddNVarcharPara("@BranchId", 100, BranchID);
                proc.AddNVarcharPara("@ReturnCode", 10, Convert.ToString(ReturnCode), QueryParameterDirection.Output);
                proc.AddNVarcharPara("@ReturnMsg", 10, ReturnMsg, QueryParameterDirection.Output);   

                proc.RunActionQuery();

                ReturnCode = Convert.ToInt32(proc.GetParaValue("@ReturnCode"));
                ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
                

            }


        }

        public DataTable ViewTimeSheet(string TimeSheetID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_TimeSheet_Details");
            proc.AddNVarcharPara("@Action", 100, "ViewTimeSheet");
            proc.AddNVarcharPara("@TimeSheetID", 10, TimeSheetID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteTimeSheet(string TimeSheetID)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_TimeSheet_Details");
            proc.AddNVarcharPara("@Action", 100, "DeleteTimeSheet");
            proc.AddNVarcharPara("@TimeSheetID", 10, TimeSheetID);
            ret = proc.RunActionQuery();
            return ret;
        }
    }
}
