using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Models;
using System.Data;
using UtilityLayer;
using Payroll.Repostiory.payrollStructureMaster;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class StructureMasterController : Controller
    {
        payrollStructureEngine objModel = new payrollStructureEngine();
        public IStructureLogic objIStructureLogic;

        #region Structure Listing

        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/StructureMaster/Dashboard.cshtml", GetPaystructure());
        }
        public ActionResult PartialStructureGrid()
        {
            return PartialView("~/Views/HRPayroll/StructureMaster/PartialStructureGrid.cshtml", GetPaystructure());
        }
        public IEnumerable GetPaystructure()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_PayStructureMasterLists
                    orderby d.StructureID descending
                    select d;
            return q;
        }

        #endregion

        #region Structure View Mode
        public ActionResult ViewDetails(string ActionType, string StructureID)
        {
            if (ActionType == "VIEW")
            {
                objIStructureLogic = new StructureLogic();
                DataSet ds = objIStructureLogic.GetStructureDetails(StructureID);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    objModel.StructureID = Convert.ToString(dt.Rows[0]["StructureID"]);
                    objModel.StructureName = Convert.ToString(dt.Rows[0]["StructureName"]);
                    objModel.StructureCode = Convert.ToString(dt.Rows[0]["StructureCode"]);
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<PayHeadsDetails> oview = new List<PayHeadsDetails>();
                        oview = APIHelperMethods.ToModelList<PayHeadsDetails>(dt);
                        objModel.AllowanceDetails = oview;
                    }
                }

                objModel.StructureHeaderName = "View Pay Structure";
            }

            return View("~/Views/HRPayroll/StructureMaster/ViewDetails.cshtml", objModel);
        }
        #endregion

        #region Structure Add Mode

        public ActionResult Index(string ActionType, string StructureID)
        {
            if (ActionType == "ADD")
            {
                objModel.StructureHeaderName = "Add Pay Structure";
                Session["StructureID"] = null;
                Session["StructureDetails"] = null;
                Session["HeadDetails"] = null;
            }
            else if (ActionType == "EDIT")
            {
                Session["StructureID"] = StructureID;
                objIStructureLogic = new StructureLogic();
                DataSet ds = objIStructureLogic.GetStructureDetails(StructureID);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    TempData["StructureDetails"] = dt;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TempData["HeadDetails"] = dt;
                }

                objModel.StructureHeaderName = "Edit Pay Structure";
            }

            return View("~/Views/HRPayroll/StructureMaster/Index.cshtml", objModel);
        }
        public PartialViewResult PayStructure()
        {
            objModel._PClassName = objModel.PopulateClassName();
            if (TempData["StructureDetails"] != null)
            {
                DataTable dt = (DataTable)TempData["StructureDetails"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    objModel.StructureID = Convert.ToString(dt.Rows[0]["StructureID"]);
                    objModel.StructureName = Convert.ToString(dt.Rows[0]["StructureName"]);
                    objModel.StructureCode = Convert.ToString(dt.Rows[0]["StructureCode"]);
                    objModel._PClassId = Convert.ToString(dt.Rows[0]["ClassId"]);
                }
            }

            return PartialView("~/Views/HRPayroll/StructureMaster/PayStructure.cshtml", objModel);
        }
        public PartialViewResult PayHeads()
        {
            objModel.PayHeadTypeList = objModel.PopulatePayHeadType();
            objModel.CalculationTypeList = objModel.PopulateCalculationType();
            objModel.RoundOffTypeList = objModel.PopulateRoundOffType();
            objModel.DataTypeList = objModel.PopulateDataType();
            objModel.PayType = "AL";
            objModel.DataType = "NM";
            objModel.CalculationType = "EO";
            objModel.RoundOffType = "NR";

            if (TempData["HeadDetails"] != null)
            {
                DataTable dt = (DataTable)TempData["HeadDetails"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<PayHeadsDetails> oview = new List<PayHeadsDetails>();
                    oview = APIHelperMethods.ToModelList<PayHeadsDetails>(dt);
                    objModel.AllowanceDetails = oview;
                }
            }

            return PartialView("~/Views/HRPayroll/StructureMaster/PayHeads.cshtml", objModel);
        }
        [HttpPost]
        public JsonResult PayStructureSubmit(payrollStructureEngine model)
        {
            if (Convert.ToString(model.StructureName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Structure Name is mandatory";
            }
            else if (Convert.ToString(model.StructureCode) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Structure Short Name is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";
                string StructureID = "";

                objIStructureLogic = new StructureLogic();
                objIStructureLogic.StructureModify(model, ref strIsComplete, ref strMessage, ref StructureID);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";

                    Session["StructureID"] = StructureID;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
            }

            return Json(model);
        }
        [HttpPost]
        public JsonResult PayHeadsSubmit(payrollStructureEngine model)
        {
            if (Convert.ToString(model.PayHeadName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Pay Head Name is mandatory";
            }
            else if (Convert.ToString(model.PayHeadShortName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Pay Head Short Name is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";
                string PayHeadID = "";
                model.StructureID = Convert.ToString(Session["StructureID"]);
                DataTable dtDetails = new DataTable();

                if (CheckPayHeadFormula(model.StructureID, model.Cal_CalculateFormula) == true)
                {
                   objIStructureLogic = new StructureLogic();
                    objIStructureLogic.PayheadSaveModify(model, ref strIsComplete, ref strMessage, ref PayHeadID, ref dtDetails);
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
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = "Please check formula";
                }
            }

            return Json(model);
        }
        [HttpGet]
        public JsonResult PopulatePayHead()
        {
            string StructureID = Convert.ToString(Session["StructureID"]);

            objIStructureLogic = new StructureLogic();
            DataTable dtDetails = objIStructureLogic.PopulatePayHead(StructureID);

            List<PayHeadIDList> oview = new List<PayHeadIDList>();
            oview = APIHelperMethods.ToModelList<PayHeadIDList>(dtDetails);
            objModel.PayHeadList = oview;

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit/Delete Section

        [HttpPost]
        public JsonResult DeleteStructure(payrollStructureEngine model)
        {
            int strIsComplete = 0;
            string strMessage = "";

            objIStructureLogic = new StructureLogic();
            objIStructureLogic.DeleteStructure(model, ref strIsComplete, ref strMessage);
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
        [HttpPost]
        public JsonResult DeletePayHead(string ActionType, string id)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            objIStructureLogic = new StructureLogic();
            DataTable dtDetails = new DataTable();
            Msg _msg = new Msg();
            try
            {
                string StructureID = Convert.ToString(Session["StructureID"]);
                output_msg = objIStructureLogic.DeletePayHead(ActionType, id, ref ReturnCode, StructureID, ref dtDetails);
                if (output_msg == "Success" && ReturnCode == 1)
                {
                    objModel.ResponseCode = "Success";
                    objModel.ResponseMessage = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
                {
                    objModel.ResponseCode = "Error";
                    objModel.ResponseMessage = output_msg;
                }
                else
                {
                    objModel.ResponseCode = "Error";
                    objModel.ResponseMessage = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                objModel.ResponseCode = "CatchError";
                objModel.ResponseMessage = "Please try again later";
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditPayHead(string ActionType, string id)
        {
            objIStructureLogic = new StructureLogic();
            DataTable dtDetails = new DataTable();

            try
            {
                dtDetails = objIStructureLogic.EditPayHead(id);

                if (dtDetails != null && dtDetails.Rows.Count > 0)
                {
                    objModel.PayHeadName = Convert.ToString(dtDetails.Rows[0]["PayHeadName"]);
                    objModel.PayHeadShortName = Convert.ToString(dtDetails.Rows[0]["PayHeadCode"]);
                    objModel.PayType = Convert.ToString(dtDetails.Rows[0]["PayType"]);
                    objModel.CalculationType = Convert.ToString(dtDetails.Rows[0]["CalculationType"]);
                    objModel.Cal_DisplayFormula = Convert.ToString(dtDetails.Rows[0]["Formula"]);
                    objModel.RoundOffType = Convert.ToString(dtDetails.Rows[0]["RoundingOffType"]);
                    objModel.DataType = Convert.ToString(dtDetails.Rows[0]["DataType"]);
                    objModel.IsProrataCalculated = Convert.ToBoolean(dtDetails.Rows[0]["IsProrataCalculated"].ToString());
                    objModel.Comments = Convert.ToString(dtDetails.Rows[0]["Comments"].ToString());
                    objModel.ResponseCode = "Success";
                }
                else
                {
                    objModel.ResponseCode = "Error";
                }
            }
            catch (Exception ex)
            {
                objModel.ResponseCode = "Error";
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Formula Builder Section

        public PartialViewResult FormulaBuilder()
        {
            objModel.PayHeadDetails = objModel.PopulatePayHeadDetailsType(Convert.ToString(Session["StructureID"]));
            objModel.FunctionDetails = objModel.PopulateFunctionList();
            objModel.TableDetails = objModel.PopulateTableList();

            return PartialView("~/Views/HRPayroll/StructureMaster/FormulaBuilder.cshtml", objModel);
        }
        public bool CheckPayHeadFormula(string StructureID, string Formula)
        {
            bool IsCheck = true;

            if (string.IsNullOrEmpty(Formula) == false)
            {
                if (Formula.Trim() != "")
                {
                    try
                    {
                        objIStructureLogic = new StructureLogic();
                        DataTable dtDetails = objIStructureLogic.CheckFormula(StructureID, Formula);
                        IsCheck = true;
                    }
                    catch
                    {
                        IsCheck = false;
                    }
                }
            }

            return IsCheck;
        }
        [HttpGet]
        public JsonResult CheckFormula(string Formula)
        {
            DataTable dtDetails = new DataTable();
            string StructureID = Convert.ToString(Session["StructureID"]);

            try
            {
                objIStructureLogic = new StructureLogic();
                dtDetails = objIStructureLogic.CheckFormula(StructureID, Formula);
                objModel.ResponseCode = "Success";
            }
            catch
            {
                objModel.ResponseCode = "Error";
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PayHead Listing

        public ActionResult PartialPayHeadGrid()
        {
            return PartialView("~/Views/HRPayroll/StructureMaster/PartialPayHeadGrid.cshtml", GetPayHead());
        }
        public IEnumerable GetPayHead()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string StructureID = Convert.ToString(System.Web.HttpContext.Current.Session["StructureID"]);

            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_PayHeadLists
                    where d.StructureID == StructureID
                    orderby d.PayHeadID descending
                    select d;
            return q;
        }

        #endregion

        #region Report Widgets

        public object SaveReportWidgets(List<DisplayPayHeadList> AllowancePayHeadList, List<DisplayPayHeadList> DeductionPayHeadList, List<DisplayPayHeadList> OthersPayHeadList)
        {
            payrollStructureEngine model = new payrollStructureEngine();

            DataTable Allowance_dt = ListToDatatable(AllowancePayHeadList);
            DataTable Deduction_dt = ListToDatatable(DeductionPayHeadList);
            DataTable Others_dt = ListToDatatable(OthersPayHeadList);

            int strIsComplete = 0;
            string strMessage = "";
            string StructureID = Convert.ToString(Session["StructureID"]);

            objIStructureLogic = new StructureLogic();
            objIStructureLogic.SaveReportWidgets(StructureID, Allowance_dt, Deduction_dt, Others_dt, ref strIsComplete, ref strMessage);
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
        public DataTable ListToDatatable(List<DisplayPayHeadList> PayHeadList)
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("DisplayIndex", typeof(string));
            Dt.Columns.Add("PayHeadID", typeof(string));

            if (PayHeadList != null && PayHeadList.Count > 0)
            {
                for (var i = 0; i < PayHeadList.Count; i++)
                {
                    string DisplayIndex = Convert.ToString(PayHeadList[i].DisplayIndex);
                    string PayHeadID = Convert.ToString(PayHeadList[i].PayHeadID);

                    Dt.Rows.Add(DisplayIndex, PayHeadID);
                }
            }

            return Dt;
        }
        public object GetReportWidgets()
        {
            try
            {
                string StructureID = Convert.ToString(Session["StructureID"]);
                objIStructureLogic = new StructureLogic();
                DataSet ds = objIStructureLogic.GetStructureDetails(StructureID);

                #region Report Widgets

                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[2];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Display_PayHeadList> oview = new List<Display_PayHeadList>();
                        oview = APIHelperMethods.ToModelList<Display_PayHeadList>(dt);
                        objModel.AllowanceHeadDetails = oview;
                    }
                }

                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Display_PayHeadList> oview = new List<Display_PayHeadList>();
                        oview = APIHelperMethods.ToModelList<Display_PayHeadList>(dt);
                        objModel.DeductionHeadDetails = oview;
                    }
                }

                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[4];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Display_PayHeadList> oview = new List<Display_PayHeadList>();
                        oview = APIHelperMethods.ToModelList<Display_PayHeadList>(dt);
                        objModel.OthersHeadDetails = oview;
                    }
                }

                #endregion

                objModel.ResponseCode = "Success";
            }
            catch
            {

            }


            return Json(objModel);
        }
        public PartialViewResult ReportWidgets()
        {
            return PartialView("~/Views/HRPayroll/StructureMaster/ReportWidgets.cshtml");
        }

        #endregion

        #region Payhead Import Log

        public ActionResult PayHeadImportLogGrid()
        {
            return PartialView("~/Views/HRPayroll/StructureMaster/PayHeadImportLogGrid.cshtml", GetPayHeadImportLog());
        }
        public IEnumerable GetPayHeadImportLog()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_PayHeadImportLogLists
                    orderby d.ID descending
                    select d;
            return q;
        }

        #endregion

        public ActionResult DownloadFormatPayHead()
        {
            string strFileName = "PayHead.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + "/");
            byte[] fileBytes = System.IO.File.ReadAllBytes(strPath + "PayHead.xlsx");
            return File(fileBytes, "application/vnd.ms-excel", strFileName);
        }

        [HttpPost]
        public JsonResult AttachmentPayHeadAddUpdate()
        {
            Boolean Success = false;
            try
            {
                if (Request.Files.Count > 0)
                {
                    string folderid = "";
                    string path = String.Empty;
                    //  Get all files from Request object  

                    HttpFileCollectionBase files = Request.Files;

                    var obj = Request.Form;
                    string PayHeadID = Convert.ToString(obj["PayHeadID"]);
                    String StructureID = Convert.ToString(obj["StructureID"]);
                    if (StructureID == "" || StructureID == null)
                    {
                        StructureID = Convert.ToString(Session["StructureID"]);
                    }
                    Int32 CreateUser = Int32.Parse(Convert.ToString(Session["userid"]));

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
                            ImportAttendance objimport = Import_To_Grid(fname, extension, file, PayHeadID, StructureID);
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

        public ImportAttendance Import_To_Grid(string FilePath, string Extension, HttpPostedFileBase file, String PayHeadID, String StructureID)
        {
            Boolean Success = false;
            Int32 HasLog = 0;
            ImportAttendance obj = new ImportAttendance();
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
                    DataTable PayHeadTable = new DataTable();
                    if (PayHeadTable != null)
                    {
                        PayHeadTable = new DataTable();
                    }
                    DataColumn workCol = PayHeadTable.Columns.Add("ID", typeof(Int32));
                    workCol.AllowDBNull = false;
                    workCol.Unique = true;

                    PayHeadTable.Columns.Add("PayHead", typeof(string));
                    PayHeadTable.Columns.Add("ShortName", typeof(string));
                    PayHeadTable.Columns.Add("PayHeadType", typeof(string));
                    PayHeadTable.Columns.Add("DataType", typeof(string));
                    PayHeadTable.Columns.Add("CalculationType", typeof(string));
                    PayHeadTable.Columns.Add("Formula", typeof(string));

                    PayHeadTable.Columns.Add("RoundingOff", typeof(string));
                    PayHeadTable.Columns.Add("Comments", typeof(string));
                    PayHeadTable.Columns.Add("ProrataCalculation", typeof(Boolean));
                    PayHeadTable.Columns.Add("Success", typeof(Boolean));
                    PayHeadTable.Columns.Add("Message", typeof(string));

                    int r = 0;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string Message = "Sucess";
                            DateTime? Date = null;
                            String PayHead = "";
                            String ShortName = "";
                            String PayHeadType = "";
                            String DataType = "";
                            String CalculationType = "";
                            String Formula = "";
                            String RoundingOff = "";
                            String Comments = "";
                            String Proratatxt = "";
                            Boolean Prorata = false;

                            bool STATUS = true;
                            try
                            {
                                PayHead = Convert.ToString(row[0]);
                                if (PayHead == null || PayHead == "")
                                {
                                    Message = "Pay Head name is missing";
                                    STATUS = false;
                                }
                            }
                            catch
                            {
                                Message = "Pay Head not valid";
                                STATUS = false;
                            }

                            if (row[1].ToString() != "")
                            {
                                ShortName = Convert.ToString(row[1]);
                            }
                            else
                            {
                                Message = "Short Name is missing";
                                STATUS = false;
                            }

                            if (row[2].ToString() != "")
                            {
                                PayHeadType = Convert.ToString(row[2]);
                                if (PayHeadType == "Allowance")
                                {
                                    PayHeadType = "AL";
                                }
                                else if (PayHeadType == "Deduction")
                                {
                                    PayHeadType = "DT";
                                }
                                else if (PayHeadType == "Others")
                                {
                                    PayHeadType = "OT";
                                }
                                else
                                {
                                    Message = "Pay head type is wrong. Found in excel- " + PayHeadType + " .";
                                    STATUS = false;
                                }
                            }

                            if (row[3].ToString() != "")
                            {
                                DataType = Convert.ToString(row[3]);
                                if (DataType == "Number")
                                {
                                    DataType = "NM";
                                }
                                else if (DataType == "String")
                                {
                                    DataType = "ST";
                                }
                                else
                                {
                                    Message = "Data type is wrong. Found in excel- " + DataType + " .";
                                    STATUS = false;
                                }
                            }

                            if (row[4].ToString() != "")
                            {
                                CalculationType = Convert.ToString(row[4]);
                                if (CalculationType == "Calculated")
                                {
                                    CalculationType = "CL";
                                }
                                else if (CalculationType == "Entered Always")
                                {
                                    CalculationType = "EA";
                                }
                                else if (CalculationType == "Entered Once")
                                {
                                    CalculationType = "EO";
                                }
                                else
                                {
                                    Message = "Calculation Type is wrong. Found in excel- " + CalculationType + " .";
                                    STATUS = false;
                                }
                            }

                            if (row[5].ToString() != "")
                            {
                                Formula = Convert.ToString(row[5]);
                                if (CalculationType == "CL")
                                {
                                    //objIStructureLogic = new StructureLogic();
                                    //DataTable dtDetails = objIStructureLogic.CheckFormula(StructureID, Formula);                                   
                                    //if (objModel.ResponseCode != "Success")
                                    //{
                                    //    Formula = "";
                                    //    Message = "Formula is not valid. Found in excel- " + Formula + " .";
                                    //    STATUS = false;
                                    //}
                                    try
                                    {
                                        objIStructureLogic = new StructureLogic();
                                        DataTable dtDetails = objIStructureLogic.CheckFormula(StructureID, Formula);
                                        //Formula = "";
                                        objModel.ResponseCode = "Success";
                                        STATUS = true;
                                    }
                                    catch
                                    {
                                        objModel.ResponseCode = "Error";
                                        Formula = "";
                                        Message = "Formula is not valid. Found in excel- " + Formula + " .";
                                        STATUS = false;
                                    }
                                }
                                else
                                {
                                    Formula = "";
                                }
                            }

                            if (row[6].ToString() != "")
                            {
                                RoundingOff = Convert.ToString(row[6]);
                                if (RoundingOff == "Downward Rounding")
                                {
                                    RoundingOff = "DR";
                                }
                                else if (RoundingOff == "Not Applicable")
                                {
                                    RoundingOff = "NA";
                                }
                                else if (RoundingOff == "Normal Rounding")
                                {
                                    RoundingOff = "NR";
                                }
                                else if (RoundingOff == "Upward Rounding")
                                {
                                    RoundingOff = "UP";
                                }
                                else
                                {
                                    Message = "Rounding Off is wrong. Found in excel- " + RoundingOff + " .";
                                    STATUS = false;
                                }
                            }

                            if (row[7].ToString() != "")
                            {
                                Comments = Convert.ToString(row[7]);
                            }

                            if (row[8].ToString() != "")
                            {
                                Proratatxt = Convert.ToString(row[8]);
                                if (Proratatxt.ToLower() == "yes")
                                {
                                    Prorata = true;
                                }
                                else if (Proratatxt.ToLower() == "no")
                                {
                                    Prorata = false;
                                }
                                

                            }

                            DataRow dr = PayHeadTable.NewRow();
                            dr["ID"] = ++r;
                            dr["PayHead"] = PayHead;
                            dr["ShortName"] = ShortName;
                            dr["PayHeadType"] = PayHeadType;
                            dr["DataType"] = DataType;
                            dr["CalculationType"] = CalculationType;
                            dr["Formula"] = Formula;

                            dr["RoundingOff"] = RoundingOff;
                            dr["Comments"] = Comments;
                            dr["ProrataCalculation"] = Prorata;
                            dr["Success"] = STATUS;
                            dr["Message"] = Message;
                            PayHeadTable.Rows.Add(dr);


                            if (PayHeadTable != null)
                            {
                                if (PayHeadTable.Rows.Count > 0)
                                {
                                    string userid = Session["userid"].ToString();
                                    objIStructureLogic = new StructureLogic();
                                    DataSet dtEmp = objIStructureLogic.GetImportPayHead(PayHeadTable, Convert.ToInt64(userid), PayHeadID, StructureID);
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
                                        obj.ReturnMessage = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnMessage"].ToString();
                                        obj.ReturnValue = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                                        obj.HasLog = 1;
                                    }
                                }
                            }

                        }
                        //if (PayHeadTable != null)
                        //{
                        //    if (PayHeadTable.Rows.Count > 0)
                        //    {
                        //        string userid = Session["userid"].ToString();
                        //        objIStructureLogic = new StructureLogic();
                        //        DataSet dtEmp = objIStructureLogic.GetImportPayHead(PayHeadTable, Convert.ToInt64(userid), PayHeadID, StructureID);
                        //        if (dtEmp.Tables.Count > 1)
                        //        {
                        //            if (dtEmp != null && dtEmp.Tables[dtEmp.Tables.Count - 1].Rows.Count > 0)
                        //            {
                        //                obj.ReturnMessage = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnMessage"].ToString();
                        //                obj.ReturnValue = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                        //                obj.HasLog = 1;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            obj.ReturnMessage = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnMessage"].ToString();
                        //            obj.ReturnValue = dtEmp.Tables[dtEmp.Tables.Count - 1].Rows[0]["ReturnValue"].ToString();
                        //            obj.HasLog = 1;
                        //        }
                        //    }
                        //}
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
    }
}