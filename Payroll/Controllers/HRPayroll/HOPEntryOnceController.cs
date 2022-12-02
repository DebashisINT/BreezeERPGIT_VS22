using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Payroll.Models;
using Payroll.Repostiory.payrollStructureMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class HOPEntryOnceController : Controller
    {
        public IStructureLogic objIStructureLogic;

        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/HOPEntryOnce/Dashboard.cshtml");
        }
        public object GetEmployeeHOPList(string PayStructureID)
        {
            payrollStructureEngine model = new payrollStructureEngine();

            objIStructureLogic = new StructureLogic();
            DataSet ds = objIStructureLogic.GetOnceEmployeeHOPDetails(PayStructureID, "EO");

            DataTable dt_Status = new DataTable();
            DataTable dt_PayHeadList = new DataTable();
            DataTable dt_PayHeadTypeList = new DataTable();
            DataTable dt_HOPList = new DataTable();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) dt_Status = ds.Tables[0];
            string ReturnValue = Convert.ToString(dt_Status.Rows[0]["ReturnValue"]);
            string ReturnMessage = Convert.ToString(dt_Status.Rows[0]["ReturnMessage"]);

            if (ReturnValue == "Success")
            {
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0) dt_PayHeadList = ds.Tables[1];
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0) dt_PayHeadTypeList = ds.Tables[2];
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0) dt_HOPList = ds.Tables[3];

                List<PayHeadIDList> oPayHeadList = new List<PayHeadIDList>();
                List<PayHeadDataTypeList> oPayHeadTypeList = new List<PayHeadDataTypeList>();
                List<dynamic> oHOPList = new List<dynamic>();

                if (dt_PayHeadList != null && dt_PayHeadList.Rows.Count > 0)
                {
                    oPayHeadList = APIHelperMethods.ToModelList<PayHeadIDList>(dt_PayHeadList);
                }

                if (dt_PayHeadTypeList != null && dt_PayHeadTypeList.Rows.Count > 0)
                {
                    oPayHeadTypeList = APIHelperMethods.ToModelList<PayHeadDataTypeList>(dt_PayHeadTypeList);
                }

                if (dt_HOPList != null && dt_HOPList.Rows.Count > 0)
                {
                    oHOPList = ToDynamicList(dt_HOPList);
                }

                model.ResponseCode = "Success";
                model.PayHeadList = oPayHeadList;
                model.PayHeadDataTypeList = oPayHeadTypeList;
                model.EmployeeHOPList = oHOPList;
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = ReturnMessage;
            }

            return Json(model);
        }
        public object SaveEmployeeHOPData(HopHead TableDetails)
        {
            payrollStructureEngine model = new payrollStructureEngine();

            DataTable PayMaster_dt = new DataTable();
            PayMaster_dt.Columns.Add("EmployeeCode", typeof(string));
            PayMaster_dt.Columns.Add("PayHeadID", typeof(string));
            PayMaster_dt.Columns.Add("Amount", typeof(string));
            PayMaster_dt.Columns.Add("Value", typeof(string));

            for (var i = 0; i < TableDetails.MainArray.Count; i++)
            {
                string EmployeeCode = "";

                for (var j = 0; j < TableDetails.MainArray[i].classPayHead.Count; j++)
                {
                    classPayHead objclassPayHead = new classPayHead();

                    objclassPayHead = TableDetails.MainArray[i].classPayHead[j];
                    string _Key = objclassPayHead.Keys;
                    string _Amount = objclassPayHead.Amount;
                    string _Values = objclassPayHead.Values;

                    if (_Key == "EmployeeCode") EmployeeCode = _Values;

                    if (_Key != "EmployeeCode" && _Key != "Employee")
                    {
                        PayMaster_dt.Rows.Add(EmployeeCode, _Key, _Amount, _Values);
                    }
                }
            }

            int strIsComplete = 0;
            string strMessage = "";

            objIStructureLogic = new StructureLogic();
            objIStructureLogic.SavePayrollDetails("EO", PayMaster_dt, ref strIsComplete, ref strMessage);
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

        public JsonResult GetActivePeriodGeneration(string ID)
        {

            var jsontable = (String)null; ;
            Msg _msg = new Msg();
            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable(@"select proll_PayStructureMaster.StructureID,proll_PayrollClass_Master.PayrollClassID,proll_PayrollClass_Master.PeriodFrom,proll_PayrollClass_Master.PeriodTo,a.YYMM,a.Period from proll_PayStructureMaster
            inner join proll_PayrollClass_Master
            on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
            left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period  from proll_PeriodGeneration where IsActive=1)a

            on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID

            where proll_PayStructureMaster.StructureID='" + ID + "'");
                if (dt != null)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsontable = serializer.Serialize(rows);


                    //ViewData["PeriodFrm"] = dt.Rows[0]["PeriodFrom"].ToString();
                    //ViewData["PeriodTo"] = dt.Rows[0]["PeriodTo"].ToString();
                    //ViewData["Period"] = dt.Rows[0]["Period"].ToString();
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            var result = new { data = jsontable, data2 = _msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttachmentFileRead()
        {
            Boolean Success = false;
            List<String> objlist = new List<string>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    string folderid = "";
                    string path = String.Empty;
                    //  Get all files from Request object  

                    HttpFileCollectionBase files = Request.Files;

                    var obj = Request.Form;
                    string AttachmentHOPType = Convert.ToString(obj["AttachmentHOPType"]);
                    string PayStructureID = Convert.ToString(obj["PayStructureID"]);

                    for (int i = 0; i < files.Count; i++)
                    {

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
                        path = Server.MapPath("~/Documents/HOPEnteredOnce/");
                        //string fulpath = path + "\\" + folderid;
                        string fulpath = path;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);

                        }

                        fname = Path.Combine(fulpath, fname);
                        file.SaveAs(fname);
                        String extension = Path.GetExtension(fname);

                        if (!String.IsNullOrEmpty(fname))
                        {
                            objlist = ReadExcelFile(fname, extension, file);
                        }

                        if (AttachmentHOPType == "Insert")
                        {
                            List<ExcelMap> maplist = (List<ExcelMap>)TempData["ExcelMap"];
                            String SettingName = (string)TempData["SettingName"];
                            int strIsComplete = 0;
                            string strMessage = "";
                            DataTable ExcelData = (DataTable)TempData["HOPEnteredOnceData"];

                            DataTable PayMaster_dt = new DataTable();
                            PayMaster_dt.Columns.Add("EmployeeCode", typeof(string));
                            PayMaster_dt.Columns.Add("PayHead", typeof(string));
                            PayMaster_dt.Columns.Add("Value", typeof(string));

                            for (var row = 0; row < ExcelData.Rows.Count; row++)
                            {
                                //String filedname = maplist[row].Map;
                                String filedname = maplist[0].Map;
                                string EmployeeCode = Convert.ToString(ExcelData.Rows[row][filedname]);
                               
                                //string EmployeeCode = Convert.ToString(ExcelData.Rows[0][filedname]);
                                for (var j = 2; j < ExcelData.Rows[row].ItemArray.Count(); j++)
                                {
                                    DataRow dr = PayMaster_dt.NewRow();
                                    
                                    dr["EmployeeCode"] = EmployeeCode;
                                    dr["PayHead"] = maplist[j].Column; //objlist[j];
                                    dr["Value"] = ExcelData.Rows[row][maplist[j].Map];
                                    PayMaster_dt.Rows.Add(dr);
                                }
                            }

                            objIStructureLogic = new StructureLogic();
                            objIStructureLogic.SaveImportPayrollDetails("EO", PayStructureID, PayMaster_dt, ref strIsComplete, ref strMessage);
                            if (strIsComplete == 1)
                            {
                                objIStructureLogic.SaveMappingExcel("InsertUpdate",ToDataTable(maplist), SettingName);
                            }

                            if (strIsComplete == 1)
                            {
                                objlist = new List<String>();
                                Success = true;
                                objlist.Add(Success.ToString());
                            }
                            else
                            {
                                objlist = new List<String>();
                                objlist.Add(Success.ToString());
                            }
                        }
                    }
                }
            }
            catch { }
            return Json(objlist);
        }

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

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

        public List<String> ReadExcelFile(string FilePath, string Extension, HttpPostedFileBase file)
        {
            Boolean Success = false;
            Int32 HasLog = 0;
            List<String> obj = new List<String>();
            //string StructureID = Convert.ToString(Session["StructureID"]);
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


                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TempData["HOPEnteredOnceData"] = dt;
                        TempData.Keep();
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName != "")
                            {
                                String ExcelColumnName = column.ColumnName;
                                obj.Add(ExcelColumnName);
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


        public object GetExcelMapList(String SettingName, List<ExcelMap> maplist)
        {
            Boolean Success = false;
            if (maplist.Count > 0)
            {
                TempData["ExcelMap"] = null;
                TempData["SettingName"] = null;
                TempData["SettingName"] = SettingName;
                TempData["ExcelMap"] = maplist;
                TempData.Keep();
                Success = true;
            }

            return Json(Success);
        }

        public JsonResult GetExcelMappingList()
        {
            List<String> maplist = new List<String>();
            DataSet ds = new DataSet();
            objIStructureLogic = new StructureLogic();
            ds = objIStructureLogic.SaveMappingExcel("GetData", new DataTable(), "");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    TempData["ExcelSetting"] = ds;
                    TempData.Keep();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        String SettingName = Convert.ToString(row["SettingName"]);
                        maplist.Add(SettingName);
                    }
                    maplist = maplist.Distinct().ToList(); 
                }
            }

            return Json(maplist);
        }

        public JsonResult GetSettingDetailsByName(String settingName)
        {
            List<ExcelMap> maplist = new List<ExcelMap>();
            try
            {
                DataTable dt = ((DataSet)TempData["ExcelSetting"]).Tables[0];
                TempData.Keep();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["SettingName"]) == settingName)
                    {
                        ExcelMap obj = new ExcelMap();
                        obj.Column = Convert.ToString(row["Column"]);
                        obj.Map = Convert.ToString(row["Map"]);
                        maplist.Add(obj);
                    }
                }
            }
            catch { }
           

            return Json(maplist);
        }
    }
}