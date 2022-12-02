using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer;
using NewCompany.Models;
using System.Web;
using System.Web.Mvc;
using System.Data;
using UtilityLayer;
using EntityLayer.CommonELS;

namespace NewCompany.Controllers
{
    public class TaskCreationController : Controller
    {
        // GET: TaskCreation

        BusinessLogicLayer.TaskCreation.TaskCreationBal taskcration = new BusinessLogicLayer.TaskCreation.TaskCreationBal();
        UserRightsForPage rights = new UserRightsForPage();

        public ActionResult Dashboard()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "TaskCreation");
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;

            return View("~/Views/NewCompany/TaskCreation/Dashboard.cshtml");
        }

        public PartialViewResult PartialTaskCreationList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "TaskCreation");
          

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanView = rights.CanView;
            return PartialView("~/Views/NewCompany/TaskCreation/PartialTaskCreation.cshtml", GetFormulaList());
        }
        public IEnumerable GetFormulaList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            NewCompany.Models.DBML.NewCompanyDBMLDataContext dc = new NewCompany.Models.DBML.NewCompanyDBMLDataContext(connectionString);
            var q = from d in dc.V_TaskCreations
                    orderby d.ID descending
                    select d;
            return q;
        }

        public PartialViewResult PartialAssignUserGrid(string ID)
        {
            return PartialView("~/Views/NewCompany/TaskCreation/PartialAssignUser.cshtml", GetFormulaList1(ID));
        }

        public IEnumerable GetFormulaList1(string ID)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            NewCompany.Models.DBML.NewCompanyDBMLDataContext dc = new NewCompany.Models.DBML.NewCompanyDBMLDataContext(connectionString);
            var q = from d in dc.V_AssignUsers
                    where (d.TASK_ID == Convert.ToInt64(ID))
                    orderby d.serial descending
                    select d;
            return q;
        }

        public ActionResult Index(string ActionType,string taskcrationid="")
        {

            TaskCreationClass task_creation = new TaskCreationClass();
            DataTable dtuser = new DataTable();
            DataTable dtselecteduser = new DataTable();
            DataTable dtedit = new DataTable();
            DataTable dtUserOtherTask = new DataTable();


            //DataTable dtuser = notificationbl.FetchNotificationSP(notificationid, "", "GetFirstTimeUser", "");
            //DataTable dtselecteduser = notificationbl.FetchNotificationSP(notificationid, "", "Getslecteduser", "");
            //DataTable dtstate = notificationbl.FetchNotificationSP(notificationid, "", "GetDesignationByState", "");
            //DataTable dtedit = notificationbl.FetchNotificationSP(notificationid, "", "GetEditdetails", "");
            dtUserOtherTask = taskcration.FetchTaskCreationSP("", "GetUpperTask"); 
           

            if (ActionType == "ADD")
            {
                task_creation.every = "1";
                ViewBag.Action = "";
                task_creation.IsActive = false;

               dtuser = taskcration.FetchTaskCreationSP("", "GetFirstTimeUser"); 
            }
            else if (ActionType == "EDIT")
            {
                dtuser = taskcration.FetchTaskCreationSP(taskcrationid, "GetFirstTimeUser");
                dtselecteduser = taskcration.FetchTaskCreationSP(taskcrationid,"Getslecteduser");
                dtedit = taskcration.FetchTaskCreationSP(taskcrationid, "GetEditdetails");
                task_creation.SelectedUser = APIHelperMethods.ToModelList<UserTaskCreationList>(dtselecteduser);

                if (dtedit!= null || dtedit.Rows.Count>0)
                {
                    task_creation.TaskCreation_ID = Convert.ToString(dtedit.Rows[0]["ID"]);
                    task_creation.TASK_SUBJECT = Convert.ToString(dtedit.Rows[0]["TASK_SUBJECT"]);
                    task_creation.TASK_DESCRIPTION = Convert.ToString(dtedit.Rows[0]["TASK_DESCRIPTION"]);
                    task_creation.ddlAction = Convert.ToString(dtedit.Rows[0]["TASK_ACTION"]);
                    task_creation.start_date = Convert.ToDateTime(dtedit.Rows[0]["TASK_STARTDATE"]);
                    task_creation.due_date = Convert.ToDateTime(dtedit.Rows[0]["TASK_DUEDATE"]);
                    task_creation.every = Convert.ToString(dtedit.Rows[0]["REPEATED_EVERYINTERVAL"]);
                    task_creation.ddlpriority = Convert.ToString(dtedit.Rows[0]["TASK_PRIORITY"]);
                    task_creation.flag = Convert.ToString(dtedit.Rows[0]["flag"]);
                    task_creation.IsActive = Convert.ToBoolean(dtedit.Rows[0]["IsActive"]);
                    task_creation.start_day = Convert.ToString(dtedit.Rows[0]["Start_Day"]);
                    task_creation.ddlday = Convert.ToString(dtedit.Rows[0]["Due_Day"]);
                    ViewBag.Action = Convert.ToString(dtedit.Rows[0]["TASK_ACTION"]);
                    task_creation.BeforeTime = Convert.ToString(dtedit.Rows[0]["Before_Time"]);
                    task_creation.OnTime = Convert.ToString(dtedit.Rows[0]["On_Time"]);
                    task_creation.AfterTime = Convert.ToString(dtedit.Rows[0]["After_Time"]);
                    task_creation.ddlUpperTask = Convert.ToString(dtedit.Rows[0]["PARENT_TASK_ID"]);

                   
                }

                

            }
            else if (ActionType == "View")
            {
                dtuser = taskcration.FetchTaskCreationSP(taskcrationid, "GetFirstTimeUser");
                dtselecteduser = taskcration.FetchTaskCreationSP(taskcrationid, "Getslecteduser");
                dtedit = taskcration.FetchTaskCreationSP(taskcrationid, "GetEditdetails");
                task_creation.SelectedUser = APIHelperMethods.ToModelList<UserTaskCreationList>(dtselecteduser);
                


                if (dtedit != null || dtedit.Rows.Count > 0)
                {
                    task_creation.TaskCreation_ID = Convert.ToString(dtedit.Rows[0]["ID"]);
                    task_creation.TASK_SUBJECT = Convert.ToString(dtedit.Rows[0]["TASK_SUBJECT"]);
                    task_creation.TASK_DESCRIPTION = Convert.ToString(dtedit.Rows[0]["TASK_DESCRIPTION"]);
                    task_creation.ddlAction = Convert.ToString(dtedit.Rows[0]["TASK_ACTION"]);
                    task_creation.start_date = Convert.ToDateTime(dtedit.Rows[0]["TASK_STARTDATE"]);
                    task_creation.due_date = Convert.ToDateTime(dtedit.Rows[0]["TASK_DUEDATE"]);
                    task_creation.every = Convert.ToString(dtedit.Rows[0]["REPEATED_EVERYINTERVAL"]);
                    task_creation.IsActive = Convert.ToBoolean(dtedit.Rows[0]["IsActive"]);
                    task_creation.ddlpriority = Convert.ToString(dtedit.Rows[0]["TASK_PRIORITY"]);
                    task_creation.start_day = Convert.ToString(dtedit.Rows[0]["Start_Day"]);
                    task_creation.ddlday = Convert.ToString(dtedit.Rows[0]["Due_Day"]);
                    ViewBag.Action = Convert.ToString(dtedit.Rows[0]["TASK_ACTION"]);

                }
                ViewBag.ActionType = "View";
            }
            task_creation.UpperTaskList = APIHelperMethods.ToModelList<UserOtherTask>(dtUserOtherTask);
            List<ActionList> ACTIONLIST = new List<ActionList>();
               
            //if (taskcrationid != "1" && taskcrationid != "2")
           // {
                
                ACTIONLIST.Add(new ActionList { ActionID = "3", actionname = "Weekly" });
                ACTIONLIST.Add(new ActionList { ActionID = "2", actionname = "Daily" });
                ACTIONLIST.Add(new ActionList { ActionID = "4", actionname = "Monthly" });
                ACTIONLIST.Add(new ActionList { ActionID = "6", actionname = "Yearly" });
                ACTIONLIST.Add(new ActionList { ActionID = "7", actionname = "One-time" });
                
                //ACTIONLIST.Add(new ActionList { ActionID = "5", actionname = "Minutes" });
            //}



            task_creation.ActionList = ACTIONLIST;
            task_creation.UserList = APIHelperMethods.ToModelList<UserTaskCreationList>(dtuser);
            



            return View("~/Views/NewCompany/TaskCreation/Index.cshtml",task_creation);

        }


        [HttpPost]
        public JsonResult SaveTaskCreationSetting(TaskCreationData data)
        {
            string ActionName = string.Empty;
            DataTable dtselected = CreateDataTable(data.Selecteduser);
            if(data.TaskCreation_ID!="")
            {
                ActionName = "UpdateSchedule";

            }
            else
            {
               
                ActionName = "InsertSchedule";
            }
            string OutputId = taskcration.SaveTaskCreationSP(ActionName, dtselected, data.Action, data.every, data.start_date,
                data.due_date, data.TaskCreation_ID, Convert.ToBoolean(data.IsActive), data.TASK_SUBJECT, data.TASK_DESCRIPTION,
                data.ddlpriority, data.start_day, data.ddlday, data.BeforeTime, data.OnTime, data.AfterTime, data.ddlUpperTask);
            return Json(OutputId);
        }


        [HttpPost]
        public JsonResult TaskDelete(string ActionType, string id)
        {
            string OutputId = string.Empty;

            
            try
            {
                OutputId = taskcration.DeleteTaskCreation(ActionType, id);
           

            }

            catch (Exception ex)
            {
               
            }

           return Json(OutputId);
        }

        [HttpPost]

        public JsonResult TaskDeleteProcessed(string ActionType, string id)
        {
            string OutputId = string.Empty;


            try
            {
                OutputId = taskcration.DeleteTaskCreationProcessed(ActionType, id);


            }

            catch (Exception ex)
            {

            }

            return Json(OutputId);
        }


        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SelectedUser", typeof(Int32));

            foreach (T entity in list)
            {
                dataTable.Rows.Add(entity);
            }

            return dataTable;
        }


    }
}