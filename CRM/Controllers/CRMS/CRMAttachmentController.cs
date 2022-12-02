using BusinessLogicLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMAttachmentController : Controller
    {
        //
        // GET: /CRMAttachment/
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        List<AttachmentViewModel> listmodel = new List<AttachmentViewModel>();
        //
        // GET: /Attachment/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AttachmentDocument(String module, String DocNo)
        {
            TempData.Clear();
            ViewBag.Module = module;
            ViewBag.DocNo = DocNo;


            string[,] DocyType = objEngine.GetFieldValue("tbl_master_documentType", "dty_id,dty_documentType", " dty_applicableFor='" + module + "'", 2, "dty_documentType");
            if (DocyType[0, 0] != "n")
            {
                // Mantis Issue 21677,21676,23104 (03/06/2021) [existing issue solved. DocyType.Length evaluated as 6 for 3 elements since it is a 2D array. To take care (DocyType.Length)/2 is done instaed of i+2) ]
                //for (int i = 0; i < DocyType.Length; i = i + 2)
                for (int i = 0; i < (DocyType.Length)/2; i = i+1)
                // End of Mantis Issue 21677,21676,23104 (03/06/2021)
                {
                    AttachmentViewModel obj = new AttachmentViewModel();
                    obj.id = DocyType[i, 0];
                    obj.Type = DocyType[i, 1];
                    listmodel.Add(obj);
                }
                
            }
            return PartialView("_CRMAttachmentDocument", listmodel);
        }

        [HttpPost]
        public JsonResult AttachmentDocumentAddUpdate()
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
                    String doc_id = Convert.ToString(obj["doc_id"]);
                    string module = Convert.ToString(obj["module"]);
                    string docno = Convert.ToString(obj["docno"]);
                    string documentType = Convert.ToString(obj["documentType"]);
                    string docNumber = Convert.ToString(obj["docNumber"]);
                    string docFileName = Convert.ToString(obj["docFileName"]);
                    string docDate = Convert.ToString(obj["docDate"]);
                    string remarks = Convert.ToString(obj["remarks"]);
                    Int32 CreateUser = Int32.Parse(Convert.ToString(Session["userid"]));

                    for (int i = 0; i < files.Count; i++)
                    {
                        string CreateDate = Convert.ToDateTime(objEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                        String FileName = String.Empty;
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  
                        folderid = documentType;
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
                        path = Server.MapPath("~/Documents/");
                        string fulpath = path + "\\" + folderid;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);

                        }

                        fulpath = fulpath + "\\" + year;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);

                        }

                        fulpath = fulpath + "\\" + docno;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);

                        }

                        fname = Path.Combine(fulpath, fname);
                        file.SaveAs(fname);


                        if (!String.IsNullOrEmpty(fname))
                        {
                            if (Convert.ToInt64(doc_id) > 0)
                            {
                                Int32 update = objEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + remarks.Trim() + "',doc_documentTypeId='" + documentType + "',doc_documentName='" + docFileName + "',doc_source='" + documentType + "/" + year + "/" + docno + "/" + FileName + "',doc_FileNo='" + docNumber + "',LastModifyDate='" + CreateDate + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(docDate).ToString("yyyy-MM-dd") + "'", " doc_id='" + doc_id + "'");
                                if (update > 0)
                                {
                                    Success = true;
                                }
                            }
                            else
                            {
                                //objEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + remarks + "',doc_documentTypeId='" + documentType + "',doc_documentName='" + docFileName + "',doc_source='" + documentType + "/" + year + "/" + FileName + "',doc_receivedate='" + "" + "'");
                                Int32 insert = objEngine.InsurtFieldValue("tbl_master_document", "doc_Type,doc_documentTypeId,doc_documentName,doc_source,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_Note1,doc_Note2,doc_RefID", "'" + module + "','" + documentType + "','" + docFileName + "','" + documentType + "/" + year + "/" + docno + "/" + FileName + "','','','','" + docNumber + "','" + CreateDate + "','" + CreateUser + "','" + Convert.ToDateTime(docDate).ToString("yyyy-MM-dd") + "','" + remarks.Trim() + "','','" + docno + "'");
                                if (insert > 0)
                                {
                                    Success = true;
                                }
                            }
                        }
                    }

                    //var datasetobj = objPM.GetProductionOrderData("RemovePOData", productionorderid, 0);
                    //if (datasetobj.Rows.Count > 0)
                    //{

                    //    foreach (DataRow item in datasetobj.Rows)
                    //    {
                    //        Success = Convert.ToBoolean(item["Success"]);
                    //    }
                    //}
                }
            }
            catch { }
            return Json(Success);
        }

        public ActionResult GetAttachmentDocumentList(String module, String DocNo)
        {
            DataTable DT = new DataTable();

            List<AttachmentDocViewModel> list = new List<AttachmentDocViewModel>();
            try
            {
                if (!String.IsNullOrEmpty(DocNo))
                {
                    AttachmentDocViewModel obj = new AttachmentDocViewModel();
                    DT = objEngine.GetDataTable("select doc_id,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_FileNo,doc.CreateDate,CandDocId,doc_receivedate,doc_renewdate,doc_verifydatetime,doc_verifyuser,doc_Note1,doc_verifyremarks,doc_RefID,doc_Type,dtype.dty_documentType from tbl_master_document doc INNER JOIN tbl_master_documentType dtype ON doc.doc_documentTypeId = dtype.dty_id  where doc_Type = '" + module + "' and doc_RefID =" + DocNo + " order by doc_id desc");

                    foreach (DataRow row in DT.Rows)
                    {
                        obj = new AttachmentDocViewModel();
                        obj.doc_id = Convert.ToInt64(row["doc_id"]);
                        obj.doc_documentTypeId = Convert.ToInt64(row["doc_documentTypeId"]);
                        obj.doc_documentName = Convert.ToString(row["doc_documentName"]);
                        obj.doc_source = "/Documents/" + Convert.ToString(row["doc_source"]);
                        obj.doc_Note1 = Convert.ToString(row["doc_Note1"]);
                        obj.doc_documentType = Convert.ToString(row["dty_documentType"]);
                        obj.doc_FileNo = Convert.ToString(row["doc_FileNo"]);
                        //obj.doc_FileNo = Convert.ToString(row["doc_FileNo"]);
                        obj.doc_receivedate = Convert.ToDateTime(row["doc_receivedate"]).ToString("dd-MM-yyyy");
                        list.Add(obj);
                    }
                }
            }
            catch { }
            TempData["AttachmentDataTable"] = DT;
            TempData.Keep();
            return PartialView("_CRMAttachmentDocumentGrid", list);
        }

        public JsonResult GetData(Int64 doc_id = 0)
        {
            DataTable DT = new DataTable();
            AttachmentDocViewModel obj = new AttachmentDocViewModel();
            try
            {
                DT = objEngine.GetDataTable("select doc_id,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_FileNo,doc.CreateDate,CandDocId,doc_receivedate,doc_renewdate,doc_verifydatetime,doc_verifyuser,doc_Note1,doc_verifyremarks,doc_RefID,doc_Type,dtype.dty_documentType from tbl_master_document doc INNER JOIN tbl_master_documentType dtype ON doc.doc_documentTypeId = dtype.dty_id  where doc_id = " + doc_id + " ");
                var siteurl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                foreach (DataRow row in DT.Rows)
                {
                    obj = new AttachmentDocViewModel();
                    obj.doc_id = Convert.ToInt64(row["doc_id"]);
                    obj.doc_documentTypeId = Convert.ToInt64(row["doc_documentTypeId"]);
                    obj.doc_documentName = Convert.ToString(row["doc_documentName"]);
                    obj.doc_source = siteurl + "/Documents/" + Convert.ToString(row["doc_source"]);
                    obj.doc_Note1 = Convert.ToString(row["doc_Note1"]);
                    obj.doc_receivedate = Convert.ToDateTime(row["doc_receivedate"]).ToString("dd-MM-yyyy");
                    obj.doc_FileNo = Convert.ToString(row["doc_FileNo"]);
                    obj.doc_documentType = Convert.ToString(row["dty_documentType"]);
                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult RemoveData(Int64 doc_id = 0)
        {
            Boolean Success = false;
            try
            {
                objEngine.GetDataTable("delete from tbl_master_document where doc_id = " + doc_id + " ");
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public ActionResult ExportAttachmentList(int type)
        {
            ViewData["AttachmentDataTable"] = TempData["AttachmentDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["AttachmentDataTable"];

            if (ViewData["AttachmentDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetAttachmentGridView(ViewData["AttachmentDataTable"]), ViewData["AttachmentDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetAttachmentGridView(ViewData["AttachmentDataTable"]), ViewData["AttachmentDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetAttachmentGridView(ViewData["AttachmentDataTable"]), ViewData["AttachmentDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetAttachmentGridView(ViewData["AttachmentDataTable"]), ViewData["AttachmentDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetAttachmentGridView(ViewData["AttachmentDataTable"]), ViewData["AttachmentDataTable"]);
                    default:
                        break;
                }

            }
            return null;
        }

        private GridViewSettings GetAttachmentGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "Document Attachment";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Document Attachment";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "dty_documentType" || datacolumn.ColumnName == "doc_FileNo"
                    || datacolumn.ColumnName == "doc_documentName" || datacolumn.ColumnName == "doc_receivedate" || datacolumn.ColumnName == "doc_Note1")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "dty_documentType")
                        {
                            column.Caption = "Document Type";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "doc_FileNo")
                        {
                            column.Caption = "Number";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "doc_documentName")
                        {
                            column.Caption = "Document Name";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "doc_receivedate")
                        {
                            column.Caption = "Attachement Date";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "doc_Note1")
                        {
                            column.Caption = "Remarks";
                            column.VisibleIndex = 4;
                        }

                        column.FieldName = datacolumn.ColumnName;
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
        public ActionResult GetDocNumberName(string Module_Name,string Module_id)
        {
            
            return null;
        }
    }
}