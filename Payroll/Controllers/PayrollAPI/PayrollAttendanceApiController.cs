using BusinessLogicLayer;
using DataAccessLayer;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.PayrollAPI
{
    public class PayrollAttendanceApiController : Controller
    {
        string ConnectionString = null;
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        ReadWriteMasterDatabaseBL bbl = new ReadWriteMasterDatabaseBL();
        public PayrollAttendanceApiController()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ConnectionString = bbl.GetDefaultConnectionStringWithoutSession();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AttendanceImport()
        {
            Boolean Success = false;
            List<PayrollSetting> list = new List<PayrollSetting>();
            String biometricpath = String.Empty;
            String Employeemapwith = String.Empty;

            if (Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = @"  SELECT ID,SettingKey,SettingValue FROM PayrollSetting";
                DataTable dt = oDBEngine.GetDataTable(Query);
                list = (from DataRow dr in dt.Rows
                        select new PayrollSetting()
                        {
                            ID = Convert.ToInt64(dr["ID"]),
                            SettingKey = Convert.ToString(dr["SettingKey"]),
                            SettingValue = Convert.ToString(dr["SettingValue"])
                        }).ToList();

                biometricpath = Convert.ToString(list.Where(x => x.SettingKey == "Attendance biometric path (provide full file path)").Select(x => x.SettingValue).FirstOrDefault());
                Employeemapwith = Convert.ToString(list.Where(x => x.SettingKey == "Employee map with").Select(x => x.SettingValue).FirstOrDefault());
                string[] filePaths = Directory.GetFiles(biometricpath, "*.xlsx");
                foreach (string fileName in filePaths)
                {

                    //StreamReader objInput = new StreamReader(fileName, System.Text.Encoding.Default);
                    //string contents = objInput.ReadToEnd().Trim();
                    //string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
                    //foreach (string s in split)
                    //{

                    //}



                    string folderid = "";
                    string path = String.Empty;
                    //  Get all files from Request object  
                    int year = objEngine.GetDate().Year;
                    Int32 CreateUser = Int32.Parse(Convert.ToString(Session["userid"]));
                    string CreateDate = Convert.ToDateTime(objEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                    String FileName = String.Empty;
                    //HttpPostedFileBase file = null;
                    string fname = fileName;

                    // Get the complete folder path and store the file inside it.
                    path = Server.MapPath("~/Documents/Attendance/");
                    string fulpath = path + "\\" + folderid;
                    if (!Directory.Exists(fulpath))
                    {
                        Directory.CreateDirectory(fulpath);
                    }

                    string destFile = Path.Combine(fulpath, (Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + Path.GetExtension(fileName)));

                    System.IO.File.Copy(fileName, destFile, true);
                    String extension = Path.GetExtension(fileName);

                    if (!String.IsNullOrEmpty(fileName))
                    {
                        Success = Import_To_Grid(fileName, extension, "", Employeemapwith);
                    }

                    if (Success)
                    {
                        path = Server.MapPath("~/Documents/Attendance/ImportBiometricsSuccess");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        destFile = Path.Combine(path, (Path.GetFileName(fileName)));
                        System.IO.File.Move(fileName, destFile);
                        //System.IO.File.Copy(fileName, destFile, true);
                    }
                    else
                    {
                        path = Server.MapPath("~/Documents/Attendance/ImportBiometricsFailure");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        destFile = System.IO.Path.Combine(path, (Path.GetFileName(fileName)));
                        System.IO.File.Move(fileName, destFile);
                        //System.IO.File.Copy(fileName, path, true);
                    }
                }
            }
            return Json(Success);
        }

        public Boolean Import_To_Grid(string FilePath, string Extension, string periodid, String map)
        {
            Boolean Success = false;
            Int32 HasLog = 0;
            string conn = string.Empty;
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
                        string userid = Session["userid"].ToString();
                        DataSet dtEmp = GetImportAttendance(AttendanceTable, Convert.ToInt64(userid), periodid, map);
                        if (dtEmp != null)
                        {
                            if (dtEmp.Tables.Count > 3)
                            {
                                if (dtEmp != null && dtEmp.Tables[dtEmp.Tables.Count - 1].Rows.Count > 0)
                                {
                                    String ReturnValue = String.Empty;
                                    ReturnValue = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                                    if (ReturnValue == "1")
                                    {
                                        Success = true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return Success;
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
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
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

        public DataSet GetImportAttendance(DataTable dt, Int64 UserID, string periodid, String map)
        {

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                cmd = new SqlCommand("proll_BiometricAttendanceImport", con);
                cmd.Parameters.AddWithValue("@PeriodID", periodid);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@EmpMap", map);
                cmd.Parameters.AddWithValue("@udtAttendance", dt);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception x) { }
            finally
            {
                cmd.Dispose();
            }
            return ds;
        }


        //[HttpGet]
        public JsonResult SubmitBiometricAttendance(String MachineCode = null, String CardNumber = null, DateTime? PunchTime = null)
        {
            Boolean Success = false;
            String Message = "Success";

            if (!String.IsNullOrEmpty(MachineCode) && !String.IsNullOrEmpty(CardNumber) && PunchTime != null)
            {
                try
                {
                    DateTime PunchTimeDate = Convert.ToDateTime(PunchTime);
                    DataSet ds = BiometricAttendanceInsert(MachineCode, CardNumber, PunchTimeDate);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds != null && ds.Tables[ds.Tables.Count - 1 ].Rows.Count > 0)
                            {
                                String ReturnValue = String.Empty;
                                ReturnValue = ds.Tables[ds.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                                Message = ds.Tables[ds.Tables.Count - 1].Rows[0]["ReturnMessage"].ToString();
                                if (ReturnValue == "1")
                                {
                                    Success = true;
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            var result = new { Message = Message, Success = Success };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public DataSet BiometricAttendanceInsert(String MachineCode, String CardNumber, DateTime PunchTimeDate)
        {

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                cmd = new SqlCommand("proll_BiometricAttendanceInsert", con);
                cmd.Parameters.AddWithValue("@strMachineCode", MachineCode);
                cmd.Parameters.AddWithValue("@strCardNumber", CardNumber);
                cmd.Parameters.AddWithValue("@dtPunchTimeDate", PunchTimeDate);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception x) { }
            finally
            {
                cmd.Dispose();
            }
            return ds;
        }
    }

    public class PayrollSetting
    {
        public Int64 ID { get; set; }

        public String SettingKey { get; set; }

        public String SettingValue { get; set; }
    }
}