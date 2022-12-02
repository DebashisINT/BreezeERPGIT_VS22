using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BusinessLogicLayer.TaskCreation
{
   public class TaskCreationBal
    {
       public DataTable FetchTaskCreationSP(string taskcreationid, string action)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_GetTaskCreation");


           proc.AddPara("@TaskCreationId", taskcreationid);
           proc.AddPara("@action", action);
           proc.AddPara("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
           
           ds = proc.GetTable();
           return ds;
       }

       

       public DataTable getAllOtherTask(string taskcreationid, string action)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_GetTaskCreation");


           proc.AddPara("@TaskCreationId", taskcreationid);
           proc.AddPara("@action", action);
           proc.AddPara("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));

           ds = proc.GetTable();
           return ds;
       }

       public string SaveTaskCreationSP(string ACTION, DataTable USERTABLE, string Task_ACTION, string recur, DateTime? startdate, DateTime? duedate,string TaskCreation_ID, bool IsActive,string subject,string description
           , string priority, string start_day, string due_day, string Before_Time, string On_Time, string After_Time, string parentTaskId)
       {
           //DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_TaskCreation_ADDEDIT");

           proc.AddPara("@ACTION", ACTION);
           proc.AddPara("@USERTABLE", USERTABLE);
           proc.AddPara("@TASK_ACTION", Task_ACTION);
           proc.AddPara("@Recur", recur);
           proc.AddPara("@startdate", startdate);
           proc.AddPara("@duedate", duedate);
           proc.AddPara("@taskcreation_ID", TaskCreation_ID);
           proc.AddBooleanPara("@IsActive", IsActive);
           proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
           proc.AddPara("@subject", subject);
           proc.AddPara("@description", description);
           proc.AddPara("@priority", priority);
           proc.AddPara("@Before_Time", Before_Time);
           proc.AddPara("@On_Time", On_Time);
           proc.AddPara("@After_Time", After_Time);
           proc.AddPara("@PARENT_TASK_ID", parentTaskId);
           if (Task_ACTION=="3")
           {
               proc.AddPara("@start_day", start_day);
               proc.AddPara("@due_day", due_day);
           }
           
           proc.AddPara("@returntext", SqlDbType.VarChar, 500, ParameterDirection.Output);
           //ds = proc.GetTable();
           proc.GetScalar();
           string OutputId = Convert.ToString(proc.GetParaValue("@returntext"));
           return OutputId;
       }

       public string DeleteTaskCreation(string ACTION, String ID)
       {
           //DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_TaskCreation_ADDEDIT");

           proc.AddPara("@ACTION", ACTION);
           proc.AddPara("@taskcreation_ID", ID);
           
           proc.AddPara("@returntext", SqlDbType.VarChar, 500, ParameterDirection.Output);
           //ds = proc.GetTable();
           proc.GetScalar();
           string OutputId = Convert.ToString(proc.GetParaValue("@returntext"));
           return OutputId;
       }


       public string DeleteTaskCreationProcessed(string ACTION, String ID)
       {
           //DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_TaskCreation_ADDEDIT");

           proc.AddPara("@ACTION", ACTION);
           proc.AddPara("@taskcreation_ID", ID);

           proc.AddPara("@returntext", SqlDbType.VarChar, 500, ParameterDirection.Output);
           //ds = proc.GetTable();
           proc.GetScalar();
           string OutputId = Convert.ToString(proc.GetParaValue("@returntext"));
           return OutputId;
       }

       

    }
}
