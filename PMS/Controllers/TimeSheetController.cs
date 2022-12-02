using BusinessLogicLayer;
using BusinessLogicLayer.PMS;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class TimeSheetController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        TimeSheetBL bl = new TimeSheetBL();
        DBEngine oDBEngine = new DBEngine();
        CommonBL cSOrder = new CommonBL();
        //public ActionResult TimeSheetEntry(string ActionType, string _uniqueid)
        //{
        //   // TimeSheetViewModel _apply = new TimeSheetViewModel();
        //    TimeSheetViewModel objList = new TimeSheetViewModel();
        //    if (ActionType == "ADD" && (_uniqueid == "" || _uniqueid == null))
        //    {

        //        objList.Action_type = ActionType;
        //        objList.StartDate = DateTime.Today;
        //        ViewBag.Action = "ADD";
               
        //    }
        //    else if (ActionType == "EDIT" && _uniqueid != "")
        //    {
               
        //        ViewBag.uniqueid = _uniqueid;
        //        ViewBag.Action = "EDIT";
        //        //_apply.HeaderName = "Modify Enquiries";
        //        //ViewBag.title = "Enquiries";

        //    }
        //    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/TimeSheetEntry", "TimeSheet");
        //    ViewBag.CanView = rights.CanView;
        //    ViewBag.CanEdit = rights.CanEdit;
        //    ViewBag.CanDelete = rights.CanDelete;
        //    ViewBag.CanAdd = rights.CanAdd;
        //    ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
        //    ViewBag.Verified = rights.Verified;




           
        //    List<Projects> listProjects = new List<Projects>();
        //    List<Rolls> listRollsList = new List<Rolls>();

        //    var datasetobj = bl.DropDownDetailForTimeSheet();

        //    if (datasetobj.Tables[0].Rows.Count > 0)
        //    {
        //        Projects obj = new Projects();
        //        foreach (DataRow item in datasetobj.Tables[0].Rows)
        //        {
        //            obj = new Projects();
        //            obj.Proj_Id = Convert.ToString(item["Proj_Id"]);
        //            obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
        //            listProjects.Add(obj);
        //        }
        //    }

        //    if (datasetobj.Tables[1].Rows.Count > 0)
        //    {
        //        Rolls obj = new Rolls();
        //        foreach (DataRow item in datasetobj.Tables[1].Rows)
        //        {
        //            obj = new Rolls();
        //            obj.ROLE_ID = Convert.ToString(item["ROLE_ID"]);
        //            obj.ROLE_NAME = Convert.ToString(item["ROLE_NAME"]);
        //            listRollsList.Add(obj);
        //        }
        //    }


        //    objList.ProjectsList = listProjects;
        //    objList.RollsList = listRollsList;

        //   // return View("~/Views/PMS/TimeSheet/TimeSheetEntry.cshtml", _apply);
        //    return View("~/Views/PMS/TimeSheet/TimeSheetEntry.cshtml", objList);

        //}

        public ActionResult TimeSheetEntry()
        {
            TimeSheetViewModel objList = new TimeSheetViewModel();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/TimeSheetEntry", "TimeSheet");
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            ViewBag.CanExport = rights.CanExport;

            objList.StartDate = DateTime.Today;

            
            List<Projects> listProjects = new List<Projects>();
            List<Rolls> listRollsList = new List<Rolls>();
            List<Durations> listDurations = new List<Durations>();
            List<ProTypes> lisTypesList = new List<ProTypes>();
            List<ProjectTasks> lisProjectTasksList = new List<ProjectTasks>();
            List<Units> Units = new List<Units>();

            var datasetobj = bl.DropDownDetailForTimeSheet();

            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Projects obj = new Projects();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Projects();
                    obj.Proj_Id = Convert.ToString(item["Proj_Id"]);
                    obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                    listProjects.Add(obj);
                }
            }

            if (datasetobj.Tables[1].Rows.Count > 0)
            {
                Rolls obj = new Rolls();
                foreach (DataRow item in datasetobj.Tables[1].Rows)
                {
                    obj = new Rolls();
                    obj.ROLE_ID = Convert.ToString(item["ROLE_ID"]);
                    obj.ROLE_NAME = Convert.ToString(item["ROLE_NAME"]);
                    listRollsList.Add(obj);
                }
            }
            if (datasetobj.Tables[2].Rows.Count > 0)
            {
                Durations obj = new Durations();
                foreach (DataRow item in datasetobj.Tables[2].Rows)
                {
                    obj = new Durations();
                    obj.Duration_ID = Convert.ToString(item["Duration_ID"]);
                    obj.Time_Duration = Convert.ToString(item["Time_Duration"]);
                    listDurations.Add(obj);
                }
            }
            if (datasetobj.Tables[3].Rows.Count > 0)
            {
                ProTypes obj = new ProTypes();
                foreach (DataRow item in datasetobj.Tables[3].Rows)
                {
                    obj = new ProTypes();
                    obj.Type_ID = Convert.ToString(item["Type_ID"]);
                    obj.Type_Name = Convert.ToString(item["Type_Name"]);
                    lisTypesList.Add(obj);
                }
            }
            if (datasetobj.Tables[4].Rows.Count > 0)
            {
                ProjectTasks obj = new ProjectTasks();
                foreach (DataRow item in datasetobj.Tables[4].Rows)
                {
                    obj = new ProjectTasks();
                    obj.ProjectTask_ID = Convert.ToString(item["ProjectTask_ID"]);
                    obj.ProjectTask_Name = Convert.ToString(item["ProjectTask_Name"]);
                    lisProjectTasksList.Add(obj);
                }
            }

            if (datasetobj.Tables[5].Rows.Count > 0)
            {
                Units obj = new Units();
                foreach (DataRow item in datasetobj.Tables[5].Rows)
                {
                    obj = new Units();
                    obj.branch_id = Convert.ToString(item["branch_id"]);
                    obj.branch_description = Convert.ToString(item["branch_description"]);
                    Units.Add(obj);
                }
            }

            objList.ProjectsList = listProjects;
            objList.RollsList = listRollsList;
            objList.DurationsList = listDurations;
            objList.TypesList = lisTypesList;
            objList.ProjectTasksList = lisProjectTasksList;
            objList.BranchList = Units;
            return View("~/Views/PMS/TimeSheet/TimeSheetEntry.cshtml", objList);
            // return View();
        }
        [HttpPost]
        //public JsonResult SaveData(TimeSheetViewModel timeSt, string uniqueid, string Date)
      
        public JsonResult PopulateBranchByHierchy()
        {
            List<UnitList> list = new List<UnitList>();
            string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchHierachy);
            if (branchtable.Rows.Count > 0)
            {
                UnitList obj = new UnitList();
                foreach (DataRow item in branchtable.Rows)
                {
                    obj = new UnitList();
                    obj.ID = Convert.ToString(item["branch_id"]);
                    obj.Name = Convert.ToString(item["branch_description"]);
                    list.Add(obj);
                }
            }
            return Json(list);
        }
        public JsonResult SetTimesheetDateFilter(Int64 unitid, string FromDate, string ToDate)
        {
            Boolean Success = false;
            try
            {
                TempData["BranchID"] = unitid;
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }
        public JsonResult SaveData(TimeSheetViewModel timeSt)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            try
            {
                bl.AddEditTimeSheet(timeSt.TimeSheetID,timeSt.StartDate,timeSt.Duration, timeSt.Time_Type, timeSt.Time_Project, timeSt.Time_ProjectTask, timeSt.Time_Roll, timeSt.txtDescription, timeSt.txtExternalComments, timeSt.Action_type,timeSt.BranchID, ref ReturnCode, ref ReturnMsg);
            if (ReturnMsg == "Success" && ReturnCode == 1)
            {
                timeSt.response_code = "Success";
                timeSt.response_msg = "Success";
            }
            else if (ReturnMsg != "Success" && ReturnCode == -10)
            {
                timeSt.response_code = "Error";
                timeSt.response_msg = ReturnMsg;
            }
            else if (ReturnMsg == "Update" && ReturnCode == 1)
            {
                timeSt.response_code = "Update";
                timeSt.response_msg = "Update";
            }
            else
            {
                timeSt.response_code = "Error";
                timeSt.response_msg = "Please try again later";
            }
           }

            catch (Exception ex)
            {
                timeSt.response_code = "CatchError";
                timeSt.response_msg = "Please try again later";
            }

            return Json(timeSt, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PartialTimeSheetGrid(TimeSheetViewModel timeSt)
        {
            string UserwiseTimeSheet = cSOrder.GetSystemSettingsResult("UserwiseTimeSheet");
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/TimeSheetEntry", "TimeSheet");
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            ViewBag.CanExport = rights.Verified;

            Int64 BranchID = 0;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DataTable dt = new DataTable();
            if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                BranchID = Convert.ToInt64(TempData["BranchID"]);
                FromDate = Convert.ToDateTime(TempData["FromDate"]);
                ToDate = Convert.ToDateTime(TempData["ToDate"]);
                TempData.Keep();
            }
            if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                if (BranchID > 0)
                {
                    dt = oDBEngine.GetDataTable("select * from v_PMS_TimeSheet where branch_id =" + BranchID + " AND (TimeSheet_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("select * from v_PMS_TimeSheet where TimeSheet_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                }
            }       

            TempData["TimesheetDetailsListDataTable"] = dt;
            return PartialView("~/Views/PMS/TimeSheet/PartialTimeSheetGrid.cshtml", GetFormulaList(timeSt.is_pageload, timeSt.FromDate, timeSt.ToDate, timeSt.ListBranch, UserwiseTimeSheet));
        }
        public IEnumerable GetFormulaList(int is_pageload, string FromDate, string ToDate, string ListBranch, string UserwiseTimeSheet)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (is_pageload != 0)
            {
                PMS.Models.DataContext.PMSDataClassesDataContext dc = new PMS.Models.DataContext.PMSDataClassesDataContext(connectionString);
                if (UserwiseTimeSheet == "No")
                {
                    if (Convert.ToInt16(ListBranch) > 0)
                    {
                        var q = from d in dc.v_PMS_TimeSheets
                                where
                                d.TimeSheet_Date >= Convert.ToDateTime(FromDate) && d.TimeSheet_Date <= Convert.ToDateTime(ToDate)
                                && d.branch_id == Convert.ToInt16(ListBranch)
                                orderby d.Create_Date descending
                                select d;
                        return q;
                    }
                    else
                    {
                        var q = from d in dc.v_PMS_TimeSheets
                                where d.TimeSheet_Date >= Convert.ToDateTime(FromDate) &&
                                      d.TimeSheet_Date <= Convert.ToDateTime(ToDate)
                                //&& d.branch_id = Convert.ToInt16(ListBranch)
                                orderby d.Create_Date descending
                                select d;
                        return q;
                    }
                }
                else
                {
                    if (Convert.ToInt16(ListBranch) > 0)
                    {
                        var q = from d in dc.v_PMS_TimeSheets
                                where
                                d.TimeSheet_Date >= Convert.ToDateTime(FromDate) && d.TimeSheet_Date <= Convert.ToDateTime(ToDate)
                                && d.branch_id == Convert.ToInt16(ListBranch)
                                && d.Create_By == Convert.ToInt64(Session["userid"])
                                orderby d.Create_Date descending
                                select d;
                        return q;
                    }
                    else
                    {
                        var q = from d in dc.v_PMS_TimeSheets
                                where d.TimeSheet_Date >= Convert.ToDateTime(FromDate) &&
                                      d.TimeSheet_Date <= Convert.ToDateTime(ToDate)
                                        && d.Create_By == Convert.ToInt64(Session["userid"])
                                //&& d.branch_id = Convert.ToInt16(ListBranch)
                                orderby d.Create_Date descending
                                select d;
                        return q;
                    }
                }
                
            }
            else
            {               
                return null;
            }
        }

        public ActionResult ExportTimesheetGridList(int type)
        {
            ViewData["TimesheetDetailsListDataTable"] = TempData["TimesheetDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["TimesheetDetailsListDataTable"];
            if (ViewData["TimesheetDetailsListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetBOMGridView(ViewData["TimesheetDetailsListDataTable"]), ViewData["TimesheetDetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetBOMGridView(ViewData["TimesheetDetailsListDataTable"]), ViewData["TimesheetDetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetBOMGridView(ViewData["TimesheetDetailsListDataTable"]), ViewData["TimesheetDetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetBOMGridView(ViewData["TimesheetDetailsListDataTable"]), ViewData["TimesheetDetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetBOMGridView(ViewData["TimesheetDetailsListDataTable"]), ViewData["TimesheetDetailsListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("BOMEntryList", "BOMEntry");
            }
        }
        private GridViewSettings GetBOMGridView(object datatable)
        {            
            var settings = new GridViewSettings();
            settings.Name = "Timesheet";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Timesheet";           
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "TimeSheet_Date" || datacolumn.ColumnName == "branch_description"
                    || datacolumn.ColumnName == "Time_Duration" || datacolumn.ColumnName == "Type_Name" || datacolumn.ColumnName == "Proj_Name" || datacolumn.ColumnName == "ProjectTask_Name"
                    || datacolumn.ColumnName == "ROLE_NAME" || datacolumn.ColumnName == "TimeSheet_Description" || datacolumn.ColumnName == "TimeSheet_ExternalComments" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "Create_Date" || datacolumn.ColumnName == "UpdatedBy")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "TimeSheet_Date")
                        {
                            column.Caption = "Date";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "branch_description")
                        {
                            column.Caption = "Branch";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "Time_Duration")
                        {
                            column.Caption = "Duration";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "Type_Name")
                        {
                            column.Caption = "Type";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "Proj_Name")
                        {
                            column.Caption = "Project";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "ProjectTask_Name")
                        {
                            column.Caption = "Project Task";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "ROLE_NAME")
                        {
                            column.Caption = "Roll";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "TimeSheet_Description")
                        {
                            column.Caption = "Description";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "TimeSheet_ExternalComments")
                        {
                            column.Caption = "External Comments";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Created By";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "Create_Date")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "UpdatedBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 10;
                        }
                        //else if (datacolumn.ColumnName == "ModifyDate")
                        //{
                        //    column.Caption = "Modified On";
                        //    column.VisibleIndex = 11;
                        //}
                        //else
                        //{
                        //    column.Caption = datacolumn.ColumnName;
                        //}
                        column.FieldName = datacolumn.ColumnName;
                        if (datacolumn.DataType.FullName == "System.Decimal")
                        {
                            column.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if (datacolumn.DataType.FullName == "System.DateTime")
                        {
                            column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                        }
                    });
                }

            }

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
        public PartialViewResult PartialTimeSheetGridSummary(TimeSheetViewModel timeSt)
        {
            string UserwiseTimeSheet = cSOrder.GetSystemSettingsResult("UserwiseTimeSheet");
            return PartialView("~/Views/PMS/TimeSheet/PartialTimeSheetSummaryGrid.cshtml", GetSummaryFormulaList(timeSt.is_pageload, timeSt.FromDate, timeSt.ToDate, UserwiseTimeSheet));
        }
        public DataTable GetSummaryFormulaList(int is_pageload, string FromDate, string ToDate, string UserwiseTimeSheet)
        {
            if (UserwiseTimeSheet == "Yes")
            {
                if (is_pageload != 0)
                {
                    DataTable ds = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PRC_PMSTIMESHEETENTRYSUMMARY");
                    proc.AddNVarcharPara("@FROMDATE", 10, FromDate);
                    proc.AddNVarcharPara("@TODATE", 10, ToDate);
                    proc.AddNVarcharPara("@UserwiseTimeSheet", 10, UserwiseTimeSheet);
                    proc.AddIntegerPara("@Userid", Convert.ToInt32(Session["userid"]));
                  
                    ds = proc.GetTable();

                    return ds;
                }
                else
                {
                    return null;
                }
            }
            else 
            {
                if (is_pageload != 0)
                {
                    DataTable ds = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PRC_PMSTIMESHEETENTRYSUMMARY");
                    proc.AddNVarcharPara("@FROMDATE", 10, FromDate);
                    proc.AddNVarcharPara("@TODATE", 10, ToDate);
                    ds = proc.GetTable();
                    return ds;
                }
                else
                {
                    return null;
                }
            }
        }
        [HttpPost]
        public JsonResult ViewDataShow(string TimeSheetID)
        {
            TimeSheetViewModel viewMDL = new TimeSheetViewModel();
            DataTable Timedt = bl.ViewTimeSheet(TimeSheetID);
            if (Timedt != null && Timedt.Rows.Count > 0)
            {
                viewMDL.TimeSheetID = Timedt.Rows[0]["TimeSheet_ID"].ToString();
                viewMDL.StartDate =Convert.ToDateTime(Timedt.Rows[0]["TimeSheet_Date"].ToString());
                viewMDL.txtExternalComments = Timedt.Rows[0]["TimeSheet_ExternalComments"].ToString();
                viewMDL.txtDescription = Timedt.Rows[0]["TimeSheet_Description"].ToString();
                viewMDL.Time_Type = Timedt.Rows[0]["TimeSheet_TypeID"].ToString();
                viewMDL.Time_ProjectTask = Timedt.Rows[0]["TimeSheet_ProjectTaskID"].ToString();
                viewMDL.Duration = Timedt.Rows[0]["TimeSheet_Duration"].ToString();
                viewMDL.Time_Project = Timedt.Rows[0]["TimeSheet_ProjectID"].ToString();

                viewMDL.Time_Roll = Timedt.Rows[0]["TimeSheet_RollID"].ToString();
                viewMDL.BranchID = Timedt.Rows[0]["TimeSheet_BranchID"].ToString();

            }
            return Json(viewMDL);
        }
        [HttpPost]
        public JsonResult DeleteData(string TimeSheetID)
        {
            string returns = "Data not Deleted please try again later";
            int val = bl.DeleteTimeSheet(TimeSheetID);
            if (val > 0)
            {
                returns = "Deleted Successfully";
            }
            return Json(returns);
        }
         [HttpPost]
        public JsonResult BindProject(String BranchID)
        {
            List<Projects> listProjects = new List<Projects>();
            DataTable datasetobj = bl.BindProjectByBranchID(BranchID);
            if (datasetobj.Rows.Count > 0)
            {
                Projects obj = new Projects();
                foreach (DataRow item in datasetobj.Rows)
                {
                    obj = new Projects();
                    obj.Proj_Id = Convert.ToString(item["Proj_Id"]);
                    obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                    listProjects.Add(obj);
                }
            }

            return Json(listProjects);
        }

         [HttpPost]
         public JsonResult BindProjectTask(String ProjectID)
         {
             List<ProjectsTask> listProjectsTask = new List<ProjectsTask>();
             DataTable datasetobj = bl.BindProjectTaskByProjectID(ProjectID);
             if (datasetobj.Rows.Count > 0)
             {
                 ProjectsTask obj = new ProjectsTask();
                 foreach (DataRow item in datasetobj.Rows)
                 {
                     obj = new ProjectsTask();
                     obj.ProjectTask_ID = Convert.ToString(item["ProjectTask_ID"]);
                     obj.ProjectTask_Name = Convert.ToString(item["ProjectTask_Name"]);
                     listProjectsTask.Add(obj);
                 }
             }

             return Json(listProjectsTask);
         }
	}
}