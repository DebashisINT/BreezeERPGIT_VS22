using DataAccessLayer;
using Payroll.Models;
using Payroll.Repostiory.prollLeaveApplication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace Payroll.Controllers.HRPayroll
{
    public class LeaveApplicationController : Controller
    {
        LeaveApplicationEngine objModel = new LeaveApplicationEngine();
        string _LeaveStructureCode = "";
        string _EmployeeCode = "";
        DBEngine oDBEngine = new DBEngine();
        UserRightsForPage rights = new UserRightsForPage();

        #region Structure Listing


        public ActionResult Index()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "LeaveApplication");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanDelete = rights.CanDelete;
            return View("~/Views/HRPayroll/LeaveApplication/Index.cshtml", objModel);
        }
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/LeaveApplication/Dashboard.cshtml"); ;
        }
        public PartialViewResult PartialLeaveHistroyGrid(string LeaveStructureCode, string EmployeeCode)
        {
            _LeaveStructureCode = LeaveStructureCode;
            _EmployeeCode = EmployeeCode;

            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveHistroyGrid.cshtml", GetLeaveHistroy());
        }
        public IEnumerable GetLeaveHistroy()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_prollLeaveBalances
                    where d.LeaveStructureCode == _LeaveStructureCode && d.EmployeeCode == _EmployeeCode
                    orderby d.LeaveName descending
                    select d;
            return q;
        }

        #endregion

        public PartialViewResult PartialLeaveApplyGrid()
        {
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveApplyGrid.cshtml");
        }
        public PartialViewResult PartialLeaveApplication()
        {
            //objModel._PLeaveType = objModel.PopulateLeaveType(_EmployeeCode, _LeaveStructureCode);
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveApplication.cshtml", objModel);
        }
        public PartialViewResult PartialLeaveApplicationEdit()
        {
            //objModel._PLeaveType = objModel.PopulateLeaveType(_EmployeeCode, _LeaveStructureCode);
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveApplicationEdit.cshtml", objModel);
        }


        [HttpPost]
        public JsonResult LeaveApplicationEdit(List<classLeaveApplicationEdit> model)
        {
            int strIsComplete = 0;
            string strMessage = "";

            
            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.LeaveApplicationEdit(model, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                objModel.ResponseCode = "Success";
                objModel.ResponseMessage = "Success";
            }
            else
            {
                objModel.ResponseCode = "Error";
                objModel.ResponseMessage = strMessage;
            }

            return Json(objModel);
        }

        [HttpPost]
        public JsonResult DeletLeaveApplication(string DocID)
        {
            int strIsComplete = 0;
            string strMessage = "";

            string ResponseCode = "";
            string ResponseMessage = "";

            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.DeleteLeaves(DocID, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                ResponseCode = "Success";
                ResponseMessage = "Success";
            }
            else
            {
                ResponseCode = "Error";
                ResponseMessage = strMessage;
            }

            return Json(ResponseCode);
        }
        [HttpPost]
        public JsonResult LeaveApplicationSubmit(List<classLeaveApplication> model)
        {
            int strIsComplete = 0;
            string strMessage = "";

            DataTable dtLeaveDetails = ToDataTable(model);

            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.LeaveApplicationModify(dtLeaveDetails, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                objModel.ResponseCode = "Success";
                objModel.ResponseMessage = "Success";
            }
            else
            {
                objModel.ResponseCode = "Error";
                objModel.ResponseMessage = strMessage;
            }

            return Json(objModel);
        }

        [HttpPost]
        public JsonResult GETApplyLeaveDetails(string EmployeeID, string StructureID)
        {
                    LeaveApprovalOutput oview = new LeaveApprovalOutput();
                    List<LeaveListClassForApp> obj = new List<LeaveListClassForApp>();
                    DataTable dt = new DataTable();


                    //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand("proll_LeaveApplicationModify", con);

                    ProcedureExecute proc = new ProcedureExecute("proll_LeaveApplicationModify");
                    proc.AddVarcharPara("@Action", 50,"GETApplyLeave");
                    proc.AddVarcharPara("@EmployeeCode",50, EmployeeID);
                    proc.AddVarcharPara("@Leave_StructureID",50, StructureID); 
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //da.Fill(dt);
                    //con.Close();
                    dt=proc.GetTable();

                    //DataTable dts = dt.Tables[0];
                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new LeaveListClassForApp()
                               {
                                   Doc_ID = Convert.ToString(dr["Doc_ID"]),
                                   Leave_ID = Convert.ToString(dr["Leave_ID"]),
                                   LeaveName = Convert.ToString(dr["LeaveName"]),
                                   No_Day = Convert.ToString(dr["No_Day"]),
                                   Lev_Date_From = Convert.ToString(dr["Lev_Date_From"]),
                                   Lev_Date_To = Convert.ToString(dr["Lev_Date_To"]),
                                   ReasonForLeave = Convert.ToString(dr["ReasonForLeave"]),
                                   CreatedDateTime = Convert.ToString(dr["CreatedDateTime"])
                               }).ToList();
                    }

                    oview.leaveListforApp = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    return Json(oview);
                }


        //Added by Bapi start
        public ActionResult LeaveEntryListing()
        {


            LeaveRegisterViewModel obj = new LeaveRegisterViewModel();
            TempData.Clear();

            return View("~/Views/HRPayroll/LeaveApplication/LeaveEntryListing.cshtml",obj); ;
        }
        public IEnumerable GetLeavesDetails(string EmployeeIDs, string Fromdate, string Todate)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_LeaveRegisters
                    where d.EmployeeCode.Contains(EmployeeIDs)
                    orderby d.EmployeeCode descending
                    select d;
            return q;
        }
        public ActionResult PartialLeaveListingGrid()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "LeaveApplication");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanDelete = rights.CanDelete;
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveListingGrid.cshtml");
        }

         public ActionResult PartialLeaveListingGridFilter(string type, string Empid, string Fromdate, string Todate)
        {
            if (Empid == null)
            {
                Empid = "";
                Todate = "";
                Fromdate = "";

            }
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveListingGridFilter.cshtml");
        
        
      }

     
        public ActionResult GetListingOfLeaveRegisterData()
        {


            DataTable dt = new DataTable();
            List<LeaveRegisterViewModel> list = new List<LeaveRegisterViewModel>();
            try
            {

                string type = "EM";
                string Empid = "";
                string FromDate = null;
                string Todate = null;

                if (TempData["Empid"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    Empid = Convert.ToString(TempData["Empid"]);
                    FromDate = Convert.ToString(TempData["FromDate"]);
                    Todate = Convert.ToString(TempData["ToDate"]);
                    TempData.Keep();
                }
                DataSet ds = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_LeaveRegister_Listing", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "List");
                cmd.Parameters.AddWithValue("@TYPE", type);
                cmd.Parameters.AddWithValue("@TAGGED_CODE", Empid);
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", Todate);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@DocID", "");

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

               

               //if (dt1.Rows.Count > 0)
               //{
               //    LeaveRegisterViewModel obj = new LeaveRegisterViewModel();
               //    foreach (DataRow item in dt1.Rows)
               //    {
               //        obj = new LeaveRegisterViewModel();
               //        obj.USERID = Convert.ToInt64(item["USERID"]);
               //        obj.REPORTTYPE = Convert.ToString(item["REPORTTYPE"]);
               //        obj.EMPLOYEECODE = Convert.ToString(item["EMPLOYEECODE"]);
               //        obj.EMPLOYEENAME = Convert.ToString(item["EMPLOYEENAME"]);
               //        obj.LEAVENAME = Convert.ToString(item["LEAVENAME"]);
               //        obj.LEV_DATE_FROM = Convert.ToDateTime(item["LEV_DATE_FROM"]);
               //        obj.LEV_DATE_TO = Convert.ToDateTime(item["LEV_DATE_TO"]);
               //        obj.LEAVEDAYS = Convert.ToInt64(item["LEAVEDAYS"]);
               //        obj.LEAVEAPPLIEDON = Convert.ToDateTime(item["LEAVEAPPLIEDON"]);
               //        obj.STATUS = Convert.ToString(item["STATUS"]);
               //        obj.USER_NAME = Convert.ToString(item["USER_NAME"]);
               //        list.Add(obj);
               //    }
               //}


 

                cmd.Dispose();
                con.Dispose();

            }
            catch (Exception ex)
            {

            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "LeaveApplication");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanDelete = rights.CanDelete;
            return PartialView("~/Views/HRPayroll/LeaveApplication/PartialLeaveListingGrid.cshtml", GetLeavesDetailsByFilter());
        }


        public JsonResult GetListingOfLeaveRegisterEditData()
        {

         string DocID = Convert.ToString(TempData["DocID"]);
        

            DataTable dt = new DataTable();
            List<LeaveRegisterViewModel> list = new List<LeaveRegisterViewModel>();
            try
            {

                string type = "EM";
                string Empid = "";
                string FromDate = null;
                string Todate = null;

                if (TempData["Empid"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    Empid = Convert.ToString(TempData["Empid"]);
                    FromDate = Convert.ToString(TempData["FromDate"]);
                    Todate = Convert.ToString(TempData["ToDate"]);
                    TempData.Keep();
                }
                DataSet ds = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_LeaveRegister_Listing", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@TYPE", type);
                cmd.Parameters.AddWithValue("@TAGGED_CODE", Empid);
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", Todate);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@DocID", DocID);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                DataTable dt1 = ds.Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    LeaveRegisterViewModel obj = new LeaveRegisterViewModel();
                 
                    foreach (DataRow item in dt1.Rows)
                    {
                        obj = new LeaveRegisterViewModel();

                      
                                      
                      
                        obj.EMPLOYEECODE = Convert.ToString(item["EMPLOYEECODE"]);
                        obj.STRUCTURECODE = Convert.ToString(item["StructureID"]);
                        obj.EMPLOYEENAME = Convert.ToString(item["EMPLOYEENAME"]);
                        obj.LEAVENAME = Convert.ToString(item["LEAVENAME"]);
                        obj.LEV_DATE_FROM = String.Format("{0:dd-MM-yyyy}", item["LEV_DATE_FROM"]);
                        obj.LEV_DATE_TO = String.Format("{0:dd-MM-yyyy}", item["LEV_DATE_TO"]);
                        obj.LEAVEDAYS = Convert.ToInt64(item["LEAVEDAYS"]);
                        obj.LEAVEAPPLIEDON = Convert.ToDateTime(item["LEAVEAPPLIEDON"]);
                        obj.STATUS = Convert.ToString(item["STATUS"]);
                        obj.ApplicationNo = Convert.ToString(item["ApplicationNo"]);
                        obj.ApplicationDetails = Convert.ToString(item["ApplicationDetails"]);
                        obj.ReasonForLeave = Convert.ToString(item["ReasonForLeave"]);
                        obj.LeaveID = Convert.ToString(item["Leave_ID"]);
                        obj.Day_Part = Convert.ToString(item["Day_Part"]);
                        obj.DocID = Convert.ToString(item["Doc_ID"]);
                        obj.Balance=Convert.ToInt64(item["Number"]);
                    
                        obj.Response = "Success";
                        list.Add(obj);
                    }
                }

                ///Convert.ToString(item["LEV_DATE_TO"]).ToString("dd-mm-yyyy");
                ///Convert.ToString(item["LEV_DATE_FROM"]);
 

                cmd.Dispose();
                con.Dispose();

            }
            catch (Exception ex)
            {

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable GetLeavesDetailsByEditFilter(string DocID)
        {
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(Session["userid"]);
            int userids = Convert.ToInt32(Session["UserID"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
         
           
                var q = from d in dc.VIEWLEAVEREGISTER_LISTINGREPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "EM" && Convert.ToString(d.DOCID) == DocID
                        orderby d.SLNO
                        select d;
                return q;
         
        }

        public IEnumerable GetLeavesDetailsByFilter()
        {
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(Session["userid"]);
            int userids = Convert.ToInt32(Session["UserID"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            Session["IsListLeaveregisterFilter"] = "Y";

            if (Convert.ToString(Session["IsListLeaveregisterFilter"]) == "Y")
            {
              
                DataTable dt = new DataTable();


                dt = oDBEngine.GetDataTable("select * from VIEWLEAVEREGISTER_LISTINGREPORT where USERID =" + Userid + " AND REPORTTYPE='EM'");
                TempData["LeaveListDataTable"] = dt;
            }




            if (Convert.ToString(Session["IsListLeaveregisterFilter"]) == "Y")
            {
                var q = from d in dc.VIEWLEAVEREGISTER_LISTINGREPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "EM"
                        orderby d.SLNO
                        select d;
                return q;
            }
            else
            {
                var q = from d in dc.VIEWLEAVEREGISTER_LISTINGREPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                return q;
            }
        }

        public JsonResult SetLeaveListingFilter(string  EmpIds, string FromDate, string ToDate)
        {
            Boolean Success = false;
            try
            {
                TempData["Empid"] = EmpIds;
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult SetLeaveDataByID(string DocID)
        {
            Boolean Success = false;
            try
            {
                TempData["DocID"] = DocID;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        
        public IEnumerable GetLeavesDetailsByFilter(string EmployeeIDs,string Fromdate, string Todate)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_LeaveRegisters
                    where  d.EmployeeCode.Contains(EmployeeIDs) 
                    orderby d.EmployeeCode descending
                    select d;
            return q;
        }

        public ActionResult ExportLeaveListingGridList(int type)
        {
            ViewData["LeaveListDataTable"] = TempData["LeaveListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["LeaveListDataTable"];
            if (ViewData["LeaveListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetLeaveGridView(ViewData["LeaveListDataTable"]), ViewData["LeaveListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetLeaveGridView(ViewData["LeaveListDataTable"]), ViewData["LeaveListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetLeaveGridView(ViewData["LeaveListDataTable"]), ViewData["LeaveListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetLeaveGridView(ViewData["LeaveListDataTable"]), ViewData["LeaveListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetLeaveGridView(ViewData["LeaveListDataTable"]), ViewData["LeaveListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("LeaveEntryListing", "LeaveApplication");
            }
        }





           

        private GridViewSettings GetLeaveGridView(object datatable)
        {
           
            var settings = new GridViewSettings();
            settings.Name = "Leave  Entry Listing";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Leave  Entry Listing";
          
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "EMPLOYEENAME" || datacolumn.ColumnName == "LEAVENAME"
                    || datacolumn.ColumnName == "LEV_DATE_FROM" || datacolumn.ColumnName == "LEV_DATE_TO" || datacolumn.ColumnName == "LEAVEDAYS" || datacolumn.ColumnName == "LEAVEAPPLIEDON" || datacolumn.ColumnName == "STATUS" || datacolumn.ColumnName == "ENTEREDBY")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "EMPLOYEENAME")
                        {
                            column.Caption = "Employee Name";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "LEAVENAME")
                        {
                            column.Caption = "Leave Type";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "LEV_DATE_FROM")
                        {
                            column.Caption = "Leave Start Date";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "LEV_DATE_TO")
                        {
                            column.Caption = "Leave End Date";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "LEAVEDAYS")
                        {
                            column.Caption = "Leave Days";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "LEAVEAPPLIEDON")
                        {
                            column.Caption = "Leave Applied On";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "STATUS")
                        {
                            column.Caption = "Status";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "ENTEREDBY")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 7;
                        }
                       
                      
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











           //Added by Bapi end

        public class LeaveApprovalOutput
        {
            public string status { get; set; }
            public string message { get; set; }

            public List<LeaveListClassForApp> leaveListforApp { get; set; }
        }
        public class LeaveListClassForApp
        {           
            public string Doc_ID { get; set; }
            public string Leave_ID { get; set; }
            public string LeaveName { get; set; }
            public string No_Day { get; set; }
            public string Lev_Date_From { get; set; }
            public string Lev_Date_To { get; set; }
            public string ReasonForLeave { get; set; }
            public string CreatedDateTime { get; set; }

            public string Leave_Status { get; set; }
        }

        [HttpPost]
        public JsonResult GETApplyLeaveDetailsByYear(string EmployeeID, string StructureID,string Year,string Month)
        {
            LeaveApprovalOutput oview = new LeaveApprovalOutput();
            List<LeaveListClassForApp> obj = new List<LeaveListClassForApp>();
            DataTable dt = new DataTable();


            //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //con.Open();
            //SqlCommand cmd = new SqlCommand("proll_LeaveApplicationModify", con);

            ProcedureExecute proc = new ProcedureExecute("proll_LeaveApplicationModify");
            proc.AddVarcharPara("@Action", 50, "GETApplyLeaveByDate");
            proc.AddVarcharPara("@EmployeeCode", 50, EmployeeID);
            proc.AddVarcharPara("@Leave_StructureID", 50, StructureID);
            proc.AddVarcharPara("@Year", 50, Year);
            proc.AddVarcharPara("@Month", 50, Month);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);
            //con.Close();
            dt = proc.GetTable();

            //DataTable dts = dt.Tables[0];
            if (dt != null)
            {
                obj = (from DataRow dr in dt.Rows
                       select new LeaveListClassForApp()
                       {
                           Doc_ID = Convert.ToString(dr["Doc_ID"]),
                           Leave_ID = Convert.ToString(dr["Leave_ID"]),
                           LeaveName = Convert.ToString(dr["LeaveName"]),
                           No_Day = Convert.ToString(dr["No_Day"]),
                           Lev_Date_From = Convert.ToString(dr["Lev_Date_From"]),
                           Lev_Date_To = Convert.ToString(dr["Lev_Date_To"]),
                           ReasonForLeave = Convert.ToString(dr["ReasonForLeave"]),
                           CreatedDateTime = Convert.ToString(dr["CreatedDateTime"]),
                           Leave_Status = Convert.ToString(dr["Leave_Status"])
                       }).ToList();
            }

            oview.leaveListforApp = obj;
            oview.status = "200";
            oview.message = "Successfully add task.";

            return Json(oview);
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        [HttpPost]
        public JsonResult LeaveApprovalSubmit(List<classLeaveApplication> model)
        {
            int strIsComplete = 0;
            string strMessage = "";

            DataTable dtLeaveDetails = ToDataTable(model);

            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.LeaveApplicationModify(dtLeaveDetails, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                objModel.ResponseCode = "Success";
                objModel.ResponseMessage = "Success";
            }
            else
            {
                objModel.ResponseCode = "Error";
                objModel.ResponseMessage = strMessage;
            }

            return Json(objModel);
        }


        [HttpPost]
        public JsonResult ApproveLeaveReq(string LEAVE_IDS)
        {

            int strIsComplete = 0;
            string strMessage = "";

            //DataTable dtLeaveDetails = ToDataTable(model);

            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.ApproveLeave(LEAVE_IDS, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                objModel.ResponseCode = "Success";
                objModel.ResponseMessage = "Success";
            }
            else
            {
                objModel.ResponseCode = "Error";
                objModel.ResponseMessage = strMessage;
            }

            return Json(objModel);
                

            
        }


        [HttpPost]
        public JsonResult RejectLeaveReq(string LEAVE_IDS)
        {

            int strIsComplete = 0;
            string strMessage = "";

            //DataTable dtLeaveDetails = ToDataTable(model);

            ILeaveApplicationLogic objLeaveApplicationLogic = new LeaveApplicationLogic();
            objLeaveApplicationLogic.RejectLeave(LEAVE_IDS, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                objModel.ResponseCode = "Success";
                objModel.ResponseMessage = "Success";
            }
            else
            {
                objModel.ResponseCode = "Error";
                objModel.ResponseMessage = strMessage;
            }

            return Json(objModel);



        }

    }
}