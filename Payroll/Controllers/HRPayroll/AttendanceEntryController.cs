using Payroll.Models;
using Payroll.Repostiory.payrollAttendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Payroll.Controllers.HRPayroll
{
    public class AttendanceEntryController : Controller
    {
        public IAttendanceLogic objAttendanceLogic;
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


        public AttendanceEntryController()
        {
            objAttendanceLogic = new AttendanceLogic();
        }
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/AttendanceEntry/Dashboard.cshtml");
        }
        public object GetEmployeeAttendance(string PayClassID, string YYMM)
        {
            AttendanceEngine model = new AttendanceEngine();

            objAttendanceLogic = new AttendanceLogic();
            DataSet ds = objAttendanceLogic.GetEmployeeAttendance(PayClassID, YYMM);

            DataTable dt_Status = new DataTable();
            DataTable dt_AttendanceDetailsList = new DataTable();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) dt_Status = ds.Tables[0];
            string ReturnValue = Convert.ToString(dt_Status.Rows[0]["ReturnValue"]);
            string ReturnMessage = Convert.ToString(dt_Status.Rows[0]["ReturnMessage"]);

            if (ReturnValue == "Success")
            {
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0) dt_AttendanceDetailsList = ds.Tables[1];

                List<dynamic> oEmployeeAttendanceList = new List<dynamic>();

                if (dt_AttendanceDetailsList != null && dt_AttendanceDetailsList.Rows.Count > 0)
                {
                    oEmployeeAttendanceList = ToDynamicList(dt_AttendanceDetailsList);
                }

                model.ResponseCode = "Success";
                model.EmployeeAttendanceList = oEmployeeAttendanceList;
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = ReturnMessage;
            }

            return Json(model);
        }

        public object RecalculateAndShowLeave(string PayClassID, string YYMM)
        {
            AttendanceEngine model = new AttendanceEngine();

            objAttendanceLogic = new AttendanceLogic();
            DataSet ds = objAttendanceLogic.GetEmployeeLeaveSummary(PayClassID, YYMM);

            DataTable dt_Status = new DataTable();
            DataTable dt_LeaveData = new DataTable();



            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) dt_LeaveData = ds.Tables[0];

            List<dynamic> oEmployeeLeaveData = new List<dynamic>();

            if (dt_LeaveData != null && dt_LeaveData.Rows.Count > 0)
            {
                oEmployeeLeaveData = ToDynamicList(dt_LeaveData);
            }


            return Json(oEmployeeLeaveData);
        }

        public object SaveLeave(string PayClassID, string YYMM)
        {
            AttendanceEngine model = new AttendanceEngine();
            string output = "";
            objAttendanceLogic = new AttendanceLogic();
            DataSet ds = objAttendanceLogic.SaveEmployeeLeaveSummary(PayClassID, YYMM);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                output = Convert.ToString(ds.Tables[0].Rows[0]["msg"]);
            }


            return Json(output);
        }


        public object SaveEmployeeAttendance(List<EmployeeAttendance> AttendanceDetails, string PayClassID, string Period)
        {
            LeaveOpeningEngine model = new LeaveOpeningEngine();

            DataTable LeaveDetails_dt = new DataTable();
            LeaveDetails_dt.Columns.Add("EmployeeID", typeof(string));
            LeaveDetails_dt.Columns.Add("AttendanceDate", typeof(string));
            LeaveDetails_dt.Columns.Add("InTime", typeof(string));
            LeaveDetails_dt.Columns.Add("OutTime", typeof(string));
            LeaveDetails_dt.Columns.Add("Status", typeof(string));

            for (var i = 0; i < AttendanceDetails.Count; i++)
            {
                string EmployeeID = AttendanceDetails[i].EmployeeID;
                string AttendanceDate = AttendanceDetails[i].AttendanceDate;
                string InTime = AttendanceDetails[i].InTime;
                string OutTime = AttendanceDetails[i].OutTime;
                string Status = AttendanceDetails[i].Status;

                LeaveDetails_dt.Rows.Add(EmployeeID, AttendanceDate, InTime, OutTime, Status);
            }

            int strIsComplete = 0;
            string strMessage = "";

            objAttendanceLogic = new AttendanceLogic();
            objAttendanceLogic.SaveAttendanceData(PayClassID, Period, LeaveDetails_dt, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                model.ResponseCode = "Success";
                model.ResponseMessage = "Success";
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = strMessage;
            }

            return Json(model);
        }
        public List<dynamic> ToDynamicList(DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        [HttpPost]
        public JsonResult AttachmentAttendanceAddUpdate()
        {
            Boolean Success = false;
            try
            {
                if (Request.Files.Count > 0)
                {
                    string folderid = "";
                    string path = String.Empty;
                    //  Get all files from Request object  
                    int year = objEngine.GetDate().Year;
                    HttpFileCollectionBase files = Request.Files;

                    var obj = Request.Form;
                    //String payclassid = Convert.ToString(obj["payclassid"]);
                    //string periodid = Convert.ToString(obj["periodid"]);
                    String payclassid = "";
                    string periodid = "";
                    string map = Convert.ToString(obj["map"]);
                    Int32 CreateUser = Int32.Parse(Convert.ToString(Session["userid"]));

                    for (int i = 0; i < files.Count; i++)
                    {
                        string CreateDate = Convert.ToDateTime(objEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                        String FileName = String.Empty;
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  
                        //folderid = documentType;
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            FileName = fname;
                        }
                        else
                        {
                            fname = file.FileName;
                            FileName = fname;
                        }

                        // Get the complete folder path and store the file inside it.
                        path = Server.MapPath("~/Documents/Attendance/");
                        string fulpath = path + "\\" + folderid;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);

                        }

                        fname = Path.Combine(fulpath, fname);
                        file.SaveAs(fname);
                        String extension = Path.GetExtension(fname);

                        if (!String.IsNullOrEmpty(fname))
                        {
                            //if (!String.IsNullOrEmpty(payclassid) && !String.IsNullOrEmpty(periodid))
                            //{
                            ImportAttendance objimport = Import_To_Grid(fname, extension, file, payclassid, periodid, map);
                            //Int32 update = objEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + remarks.Trim() + "',doc_documentTypeId='" + documentType + "',doc_documentName='" + docFileName + "',doc_source='" + documentType + "/" + year + "/" + docno + "/" + FileName + "',doc_FileNo='" + docNumber + "',LastModifyDate='" + CreateDate + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(docDate).ToString("yyyy-MM-dd") + "'", " doc_id='" + doc_id + "'");
                            if (Convert.ToInt16(objimport.ReturnValue) > 0)
                            {
                                Success = true;
                            }
                            // }
                        }
                    }
                }
            }
            catch { }
            return Json(Success);
        }

        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;

        }

        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        public ImportAttendance Import_To_Grid(string FilePath, string Extension, HttpPostedFileBase file, string payclassid, string periodid, String map)
        {
            Boolean Success = false;
            Int32 HasLog = 0;
            ImportAttendance obj = new ImportAttendance();
            string conn = string.Empty;
            if (file.FileName.Trim() != "")
            {
                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();
                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {
                        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                        IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                        string relationshipId = sheets.First().Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();

                        foreach (Cell cell in rows.ElementAt(0))
                        {
                            dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                        }
                        foreach (Row row in rows) //this will also include your header row...
                        {
                            DataRow tempRow = dt.NewRow();
                            int columnIndex = 0;
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                // Gets the column index of the cell with data
                                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                cellColumnIndex--; //zero based index
                                if (columnIndex < cellColumnIndex)
                                {
                                    do
                                    {
                                        tempRow[columnIndex] = ""; //Insert blank data here;
                                        columnIndex++;
                                    }
                                    while (columnIndex < cellColumnIndex);
                                }
                                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);
                                columnIndex++;
                            }
                            dt.Rows.Add(tempRow);
                        }
                    }
                    dt.Rows.RemoveAt(0);

                    //try
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        string b = Convert.ToString(dr[0]);
                    //        double d = double.Parse(b);
                    //        DateTime convertdate = DateTime.FromOADate(d);
                    //        dr[0] = convertdate;
                    //    }
                    //}
                    //catch { }



                    DataTable AttendanceTable = new DataTable();
                    //DataTable udtAttendanceTable = new DataTable();
                    if (AttendanceTable != null)
                    {
                        AttendanceTable = new DataTable();
                    }
                    DataColumn workCol = AttendanceTable.Columns.Add("ID", typeof(Int32));
                    workCol.AllowDBNull = false;
                    workCol.Unique = true;

                    AttendanceTable.Columns.Add("Date", typeof(DateTime));
                    AttendanceTable.Columns.Add("EmpRef", typeof(string));
                    AttendanceTable.Columns.Add("PunchInTime", typeof(string));
                    AttendanceTable.Columns.Add("PunchOutTime", typeof(string));
                    AttendanceTable.Columns.Add("STATUS", typeof(Boolean));
                    AttendanceTable.Columns.Add("STATUS_MESSAGE", typeof(string));

                    int r = 0;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string STATUS_MESSAGE = "Sucess";
                            DateTime? Date = null;
                            string EmpRef = "";
                            string PunchINTime = "";
                            string PunchOUTTime = "";
                            string txtdate = "";

                            bool STATUS = true;
                            string Datestr = null;

                            try
                            {
                                txtdate = Convert.ToString(row[1]);
                                double doubledate = double.Parse(txtdate);
                                Date = DateTime.FromOADate(doubledate);

                                //var date = ConvertDateYYYYMMDD(Convert.ToString(row[0]));
                                //Date = Convert.ToDateTime(date);
                                if (Date != null)
                                {
                                    Datestr = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");
                                }
                            }
                            catch
                            {
                                STATUS_MESSAGE = "Date not valid";
                                STATUS = false;
                            }

                            if (row[1].ToString() != "")
                            {
                                EmpRef = Convert.ToString(row[0]);
                            }
                            else
                            {
                                STATUS_MESSAGE = "Employee Ref is missing";
                                STATUS = false;
                            }

                            try
                            {
                                PunchINTime = Convert.ToString(row[2]);
                                double doubledate = double.Parse(PunchINTime);
                                PunchINTime = Convert.ToString(DateTime.FromOADate(doubledate).Hour + ":" + DateTime.FromOADate(doubledate).Minute);
                            }
                            catch
                            {
                                STATUS_MESSAGE = "Punch In Time Invalid";
                                STATUS = false;
                            }

                            try
                            {
                                PunchOUTTime = Convert.ToString(row[3]);
                                double doubledate = double.Parse(PunchOUTTime);
                                PunchOUTTime = Convert.ToString(DateTime.FromOADate(doubledate).Hour + ":" + DateTime.FromOADate(doubledate).Minute);
                            }
                            catch
                            {
                                STATUS_MESSAGE = "Punch Out Time Invalid";
                                STATUS = false;
                            }

                            if (!String.IsNullOrEmpty(EmpRef) || !String.IsNullOrEmpty(PunchINTime) || !String.IsNullOrEmpty(txtdate))
                            {

                                DataRow dr = AttendanceTable.NewRow();
                                dr["ID"] = ++r;
                                //dr["Date"] = monthNames;
                                if (Date == null)
                                {
                                    dr["Date"] = DBNull.Value;
                                }
                                else
                                {
                                    dr["Date"] = Date;
                                }
                                dr["EmpRef"] = EmpRef;
                                dr["PunchInTime"] = PunchINTime;
                                dr["PunchOutTime"] = PunchOUTTime;
                                dr["STATUS"] = STATUS;
                                dr["STATUS_MESSAGE"] = STATUS_MESSAGE;

                                AttendanceTable.Rows.Add(dr);
                            }

                        }
                        if (AttendanceTable != null && AttendanceTable.Rows.Count > 0)
                        {

                            //foreach (DataRow item in AttendanceTable.Rows)
                            //{
                            //    var date = item["Date"];
                            //    var EmpRef = item["EmpRef"];
                            //    var PunchTime = item["PunchTime"];

                            //    //item.Delete();
                            //    udtAttendanceTable.Rows.Add(item);
                            //}



                            string userid = Session["userid"].ToString();
                            DataSet dtEmp = objAttendanceLogic.GetImportAttendance(AttendanceTable, Convert.ToInt64(userid), payclassid, periodid, map);
                            if (dtEmp.Tables.Count > 1)
                            {
                                if (dtEmp != null && dtEmp.Tables[dtEmp.Tables.Count - 1].Rows.Count > 0)
                                {
                                    obj.ReturnMessage = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnMessage"].ToString();
                                    obj.ReturnValue = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                                    obj.HasLog = 1;
                                }
                            }
                            else
                            {
                                obj.ReturnMessage = dtEmp.Tables[dtEmp.Tables.Count].Rows[0]["ReturnMessage"].ToString();
                                obj.ReturnValue = dtEmp.Tables[dtEmp.Tables.Count].Rows[0]["ReturnValue"].ToString();
                                obj.HasLog = 1;
                            }
                        }
                    }
                }
            }
            return obj;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return "";
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }

        }

        public String ConvertDateYYYYMMDD(String dateformat)
        {
            String DateFormat = String.Empty;
            if (!String.IsNullOrEmpty(dateformat))
            {
                String[] format = null;
                if (dateformat.Contains('-'))
                {
                    format = dateformat.Split('-');
                }
                else if (dateformat.Contains('/'))
                {
                    format = dateformat.Split('/');
                }
                DateFormat = format[2] + "-" + format[1] + "-" + format[0];
            }
            return DateFormat;
        }

        public ActionResult DownloadFormatAttendance()
        {
            string strFileName = "Employee Attendance.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + "/");


            //Response.ContentType = "application/xlsx";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=Employee Attendance.xlsx");
            //Response.TransmitFile(strPath);
            //Response.End();
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strFileName);

            //string path = AppDomain.CurrentDomain.BaseDirectory + "FolderName/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(strPath + "Employee Attendance.xlsx");
            //string fileName = "filename.extension";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            return File(fileBytes, "application/vnd.ms-excel", strFileName);
        }

    }
}