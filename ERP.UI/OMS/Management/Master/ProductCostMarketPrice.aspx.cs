using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using DevExpress.Web;
using BusinessLogicLayer;
using DataAccessLayer;
using iTextSharp.text;
using System.Collections.Generic;
using System.Net;
using System.Data.OleDb;
using System.Data.Common;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections;
using DevExpress.Web.Data;
using System.ComponentModel;
using System.Linq;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Drawing;
using ERP.OMS.Tax_Details.ClassFile;
using ERP.Models;
using EntityLayer.MailingSystem;
using UtilityLayer;
using BusinessLogicLayer.EmailDetails;
using System.Web.Script.Services;
using DevExpress.XtraGrid;
using System.Data;
using GemBox.Spreadsheet;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
//using ClosedXML.Excel;

namespace ERP.OMS.Management.Master
{
    public partial class ProductCostMarketPrice : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        BusinessLogicLayer.Recruitment_Agents oHrRecritmentGeneralBL = new BusinessLogicLayer.Recruitment_Agents();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        //public virtual int[] GetSelectedRows();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

        }
          protected void Page_Load(object sender, EventArgs e)
          {
              #region Button Wise Right Access Section Start
              rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/ProductCostMarketPrice.aspx");
              if (rights.CanAdd == false)
              {
                  return;
              }
              #endregion Button Wise Right Access Section End

              if(!IsPostBack)
              {
                  Session["exportval"] = null;
              Session["SI_ProductDetails"] = null;
              hdnyear.Value = "";
              }           
          }
          //public class excel
          //{
          //    public int ProductId { get; set; }
              
          //}
         
          //[System.Web.Services.WebMethod]
          //public static string AutoPopulateAltQuantity(string Year)
          //{
          //    string jsonStringList = "";
          //    try
          //    {
                  
          //        excel contextDB = new excel();

          //        System.IO.FileStream fs = null;
          //        string path = "D:/BreezeErp_GitHubClone(Latest as on 06-04-2020)/BreezeERP-GIT/ERP.UI/assests/ProductCostMarketPriceExcel/ProductCostMarketPrice.xlsx";
          //        fs = System.IO.File.Open(path, System.IO.FileMode.Open);
          //        byte[] btFile = new byte[fs.Length];
          //        fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
          //        fs.Close();
          //        HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + path);
          //        HttpContext.Current.Response.ContentType = "application/octet-stream";
          //        HttpContext.Current.Response.BinaryWrite(btFile);
          //        HttpContext.Current.Response.Flush();
          //        HttpContext.Current.ApplicationInstance.CompleteRequest();

          //        fs = null;


          //    }
          //    catch (Exception ex)
          //    {

          //    }
          //    return jsonStringList;
          //}

          protected void btndownload_click(object sender, EventArgs e)
          {
              try
              {
                  //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please  Year')</script>");

                  if (hdnyear.Value == "")
                  {
                     // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Select Year')</script>");
                      Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >showexportpopup();</script>");
                      return;
                  }
                  DataTable dt = new DataTable();
                  foreach (DataRow row in dt.Rows)
                  {
                      row.Delete();
                  }
                  dt.Columns.Add("SrlNo", typeof(String));
                  dt.Columns.Add("Code", typeof(String));
                  dt.Columns.Add("Name", typeof(String));
                  dt.Columns.Add("Jan", typeof(String));
                  dt.Columns.Add("Feb", typeof(String));
                  dt.Columns.Add("Mar", typeof(String));
                  dt.Columns.Add("Apr", typeof(String));
                  dt.Columns.Add("May", typeof(String));
                  dt.Columns.Add("Jun", typeof(String));
                  dt.Columns.Add("Jul", typeof(String));
                  dt.Columns.Add("Aug", typeof(String));
                  dt.Columns.Add("Sep", typeof(String));
                  dt.Columns.Add("Oct", typeof(String));
                  dt.Columns.Add("Nov", typeof(String));
                  dt.Columns.Add("Dec", typeof(String));

                  var SrlNo = gridrateupdate.GetSelectedFieldValues("SrlNo");
                  var sProducts_ID = gridrateupdate.GetSelectedFieldValues("sProducts_ID");
                  var sProducts_Code = gridrateupdate.GetSelectedFieldValues("sProducts_Code");
                  var sProducts_Name = gridrateupdate.GetSelectedFieldValues("sProducts_Name");
                  var sProducts_Description = gridrateupdate.GetSelectedFieldValues("sProducts_Description");
                  if (SrlNo.Count <= 0)
                  {
                     // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Select Product')</script>");
                      Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >getbacktolisting();</script>");
                      return;
                  }
                  for (int i = 0; i < SrlNo.Count; i++)
                  {
                      DataRow dr = dt.NewRow();
                      var Srno = SrlNo[i];
                      var ProductsCode = sProducts_Code[i];
                      var ProductName = sProducts_Name[i];

                      dr["SrlNo"] = Srno.ToString();
                      dr["Code"] = ProductsCode.ToString();
                      dr["Name"] = ProductName.ToString();
                      dr["Jan"] = "0.000";
                      dr["Feb"] = "0.000";
                      dr["Mar"] = "0.000";
                      dr["Apr"] = "0.000";
                      dr["May"] = "0.000";
                      dr["Jun"] = "0.000";
                      dr["Jul"] = "0.000";
                      dr["Aug"] = "0.000";
                      dr["Sep"] = "0.000";
                      dr["Oct"] = "0.000";
                      dr["Nov"] = "0.000";
                      dr["Dec"] = "0.000";
                   
                      dt.Rows.Add(dr);
                  }
                  dt.Columns.Remove("SrlNo");
                  
                  #region excel export

                  DataSet ds = new System.Data.DataSet();
                  ds.Tables.Add(dt);


                //  ExportDataSet(ds, "");


                  //string attachment = "attachment; filename=" + hdnyear.Value.ToString() + DateTime.Now + ".XLSX";
                  //Response.ClearContent();
                  //Response.AddHeader("content-disposition", attachment);
                  //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                  ////Response.ContentType = "application/vnd.ms-excel";
                  //string tab = "";
                  //foreach (DataColumn dc in dt.Columns)
                  //{
                  //    Response.Write(tab + dc.ColumnName);
                  //    tab = "\t";
                  //}
                  //Response.Write("\n");
                  //int j;


                  //foreach (DataRow dr in dt.Rows)
                  //{
                  //    tab = "";
                  //    for (j = 0; j < dt.Columns.Count; j++)
                  //    {
                  //        Response.Write(tab + dr[j].ToString());
                  //        tab = "\t";
                  //    }
                  //    Response.Write("\n");
                  //}
                  //Response.End();
                  //dt.TableName = "table";


                  using (XLWorkbook wb = new XLWorkbook())
                  {
                      wb.Worksheets.Add(dt);

                      Response.Clear();
                      Response.Buffer = true;
                      Response.Charset = "";
                      Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                      Response.AddHeader("content-disposition", "attachment;filename=" + hdnyear.Value.ToString() + DateTime.Now + ".xlsx");
                      using (MemoryStream MyMemoryStream = new MemoryStream())
                      {
                          wb.SaveAs(MyMemoryStream);
                          MyMemoryStream.WriteTo(Response.OutputStream);
                          Response.Flush();
                          Response.End();
                      }
                  }







                  
                  //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Enter Valid PAN.!')</script>");
                 
                  #endregion excel export
              }
              catch
              { 
              
              }
              
          }



          private void ExportDataSet(DataSet ds, string destination)
          {
              using (var workbook = SpreadsheetDocument.Create(Server.MapPath("~"), DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
              {
                  var workbookPart = workbook.AddWorkbookPart();

                  workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                  workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                  foreach (System.Data.DataTable table in ds.Tables)
                  {

                      var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                      var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                      sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                      DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                      string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                      uint sheetId = 1;
                      if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                      {
                          sheetId =
                              sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                      }

                      DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                      sheets.Append(sheet);

                      DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                      List<String> columns = new List<string>();
                      foreach (System.Data.DataColumn column in table.Columns)
                      {
                          columns.Add(column.ColumnName);

                          DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                          cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                          cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                          headerRow.AppendChild(cell);
                      }


                      sheetData.AppendChild(headerRow);

                      foreach (System.Data.DataRow dsrow in table.Rows)
                      {
                          DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                          foreach (String col in columns)
                          {
                              DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                              cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                              cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                              newRow.AppendChild(cell);
                          }

                          sheetData.AppendChild(newRow);
                      }

                  }
              }
          }

          protected void btnImport_click(object sender, EventArgs e)
          {
              //if (ddlyearimport.Value == "--Select--")
              //{
              //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Select Year')</script>");
              //    return;
              //}
         
              if (FileImport.PostedFile != null)
              {
                  //try
                  //{
                  //    string path = "D:/BreezeErp_GitHubClone(Latest as on 06-04-2020)/BreezeERP-GIT/ERP.UI/assests/"+ FileImport.FileName;
                  //    FileImport.SaveAs(path);


                      if (FileImport.HasFile)
                         {
                         string FileName = Path.GetFileName(FileImport.PostedFile.FileName);
                         string Extension = Path.GetExtension(FileImport.PostedFile.FileName);
                         string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                         string FilePath = Server.MapPath("~/CommonFolder/ProductCostMarketPrice/") + FileName;
                         FileImport.SaveAs(FilePath);
                         Import_To_Grid(FilePath, Extension, "No");

                         //File.Delete(FilePath);

                       }


                  //}
                  //catch (Exception)
                  //{

                  //}
              }  
          }

          public void Import_To_Grid(string FilePath, string Extension, string isHDR)
          {
              try
              {
                  DataTable dtproducts = new DataTable();

                  if (FileImport.FileName.Trim() != "")
                  {

                      string fileName = Path.GetFileName(FileImport.PostedFile.FileName);

                      string extention = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));
                      extention = extention.TrimStart('.');
                      extention = extention.ToUpper();



                      if (extention == "XLS" || extention == "XLSX")
                      {
                          fileName = fileName.Replace(fileName.Substring(0, fileName.IndexOf('.')), "Productupload");

                          DataTable dt = new DataTable();

                          //using (SpreadsheetDocument doc = SpreadsheetDocument.Open(FilePath, false))
                          //{
                          //    Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                          //    Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                          //    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>().DefaultIfEmpty();



                              //foreach (Row row in rows)
                              //{
                              //    if (row.RowIndex.Value == 1)
                              //    {
                              //        foreach (Cell cell in row.Descendants<Cell>())
                              //        {
                              //            if (cell.CellValue != null)
                              //            {
                              //                dt.Columns.Add(GetValue(doc, cell));
                              //            }
                              //        }
                              //    }
                              //    else
                              //    {
                              //        dt.Rows.Add();
                              //        int i = 0;
                              //        foreach (Cell cell in row.Descendants<Cell>())
                              //        {
                              //            if (cell.CellValue != null)
                              //            {
                              //                dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
                              //            }
                              //            i++;
                              //        }
                              //    }
                              //    dtproducts = dt.Copy();
                              //}






                              DataTable dtExcelData = new DataTable();
                              string conString = string.Empty;
                              conString = ConfigurationManager.AppSettings["ExcelConString"];
                              conString = string.Format(conString, FilePath);
                              using (OleDbConnection excel_con = new OleDbConnection(conString))
                              {
                                  excel_con.Open();
                                  string sheet1 = "table1$"; //excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[1]["TABLE_NAME"].ToString();

                                  dtExcelData.Columns.Add("Code", typeof(string));
                                  dtExcelData.Columns.Add("Name", typeof(string));
                                  dtExcelData.Columns.Add("Jan", typeof(Decimal));
                                  dtExcelData.Columns.Add("Feb", typeof(Decimal));
                                  dtExcelData.Columns.Add("Mar", typeof(Decimal));
                                  dtExcelData.Columns.Add("Apr", typeof(Decimal));
                                  dtExcelData.Columns.Add("May", typeof(Decimal));
                                  dtExcelData.Columns.Add("Jun", typeof(Decimal));
                                  dtExcelData.Columns.Add("Jul", typeof(Decimal));
                                  dtExcelData.Columns.Add("Aug", typeof(Decimal));
                                  dtExcelData.Columns.Add("Sep", typeof(Decimal));
                                  dtExcelData.Columns.Add("Oct", typeof(Decimal));
                                  dtExcelData.Columns.Add("Nov", typeof(Decimal));
                                  dtExcelData.Columns.Add("Dec", typeof(Decimal));

                                  using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                                  {
                                      oda.Fill(dtproducts);
                                  }
                                  excel_con.Close();
                              }

 //}


                          if (hdnimportyear.Value != "" && hdnimportyear.Value != null)
                          {

                              //DataTable dtCmb = new DataTable();
                              //ProcedureExecute proc = new ProcedureExecute("Proc_ImportProductsCostMarketPrice");
                              //proc.AddPara("@PRODUCTIMPORT", dtproducts);
                              //proc.AddPara("@user_Id", Convert.ToInt32(Session["userid"]));
                              //proc.AddPara("@year", hdnimportyear.Value.ToString());
                            

                              DataSet dsInst = new DataSet();
                              SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                              SqlCommand cmd = new SqlCommand("Proc_ImportProductsCostMarketPrice", con);
                              cmd.CommandType = CommandType.StoredProcedure;
                              //cmd.Parameters.AddWithValue("@Action", Action);
                              cmd.Parameters.AddWithValue("@PRODUCTIMPORT", dtproducts);
                              cmd.Parameters.AddWithValue("@user_Id", Convert.ToInt32(Session["userid"]));
                              cmd.Parameters.AddWithValue("@year", hdnimportyear.Value.ToString());
                              cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                              cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                              cmd.CommandTimeout = 0;
                              SqlDataAdapter Adap = new SqlDataAdapter();
                              Adap.SelectCommand = cmd;
                              Adap.Fill(dsInst);
                              int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                              
                              //productlog.DataSource = dtCmb;
                              //productlog.DataBind();

                              //Session["Datlog"] = dtCmb;
                              if (ReturnValue == 1)
                              {
                                  //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Rate Imported Succesfully!')</script>");
                                  Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script>hideimportpopup();</script>");
                              }
                              else if (ReturnValue == -10)
                              {
                                  Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script>hideimportfailurepopup();</script>");
                              }
                                                         
                          }
                          else
                          {
                              Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Select Year')</script>");
                          }
                      }
                      else
                      {
                          ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('invalid File')", true);
                      }
                  }
              }
              catch
              {
                  Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script>hideimportfailurepopup();</script>");
              }
          }
          protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
          {
              Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
              if (Filter != 0)
              {
                  if (Session["exportval"] == null)
                  {
                      Session["exportval"] = Filter;
                      bindexport(Filter);
                  }
                  else if (Convert.ToInt32(Session["exportval"]) != Filter)
                  {
                      Session["exportval"] = Filter;
                      bindexport(Filter);
                  }
              }
          }
          public void bindexport(int Filter)
          {
              //GrdQuotation.Columns[6].Visible = false;
              string filename = "Product Cost Market Price" + hdnyear.Value.ToString() + DateTime.Now + "";
              exporter.FileName = filename;

              exporter.PageHeader.Left = "Product Cost Market Price";
              exporter.PageFooter.Center = "[Page # of Pages #]";
              exporter.PageFooter.Right = "[Date Printed]";

              switch (Filter)
              {
                  case 1:
                      exporter.WritePdfToResponse();
                      break;
                  case 2:
                      exporter.WriteXlsToResponse();
                      break;
                  case 3:
                      exporter.WriteRtfToResponse();
                      break;
                  case 4:
                      exporter.WriteCsvToResponse();
                      break;
              }
          }
          private string GetValue(SpreadsheetDocument doc, Cell cell)
          {
              string value = cell.CellValue.InnerText;
              if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
              {
                  return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
              }
              return value;
          }

          protected void GrdPayReq_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          { }
          protected void gridPayReq_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
          {
            

          }
          protected void gridlogCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          {
          }
          protected void gridrateupdate_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          {
              string strSplitCommand = e.Parameters.Split('~')[0];
              if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
              gridrateupdate.ClearSort();
              gridrateupdate.DataBind();
          }
          protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
          
          {
              e.KeyExpression = "sProducts_ID";
              //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
              string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
              ERPDataClassesDataContext dc1 = new ERPDataClassesDataContext(connectionString);

              var q = from d in dc1.v_Products
               select d;
              e.QueryableSource = q;
          }
          protected void EntityServerModelogDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
          {
              e.KeyExpression = "MarketcostLog_id";
              //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
              string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
              DataTable dtCmb = new DataTable();
              string year = string.Empty;
              year = hdnyearlog.Value;
              ProcedureExecute proc = new ProcedureExecute("Proc_FillProductCostMarketLog");
              proc.AddPara("@user_Id", Convert.ToInt32(Session["userid"]));
              proc.AddPara("@year", hdnyear.Value.ToString());
              dtCmb = proc.GetTable();
              ERPDataClassesDataContext dc1 = new ERPDataClassesDataContext(connectionString);

              var q = from d in dc1.FillProductCostMarketLogs
                      where d.Year == year
                      select d;
              e.QueryableSource = q;


          }
          protected void gridlisting_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
          {
              e.KeyExpression = "ProductCode";
              string year = string.Empty;
              year = hdngridyear.Value;
              DataTable dtCmb = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("Proc_FillProductCostMarket");           
              proc.AddPara("@user_Id", Convert.ToInt32(Session["userid"]));
              proc.AddPara("@year", hdnyear.Value.ToString());
              dtCmb = proc.GetTable();
              string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
              if (year != "" && year != null && year != "--Select--")
              {
                  //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI

                  ERPDataClassesDataContext dc1 = new ERPDataClassesDataContext(connectionString);
                  var q = from d in dc1.FillProductCostMarkets
                          where d.Year == year
                          select d;
                  e.QueryableSource = q;
              }
              else
              {
                  ERPDataClassesDataContext dc1 = new ERPDataClassesDataContext(connectionString);
                  var q = from d in dc1.FillProductCostMarkets
                          where d.Year == ""
                          select d;
                  e.QueryableSource = q;
              }
          }
           protected void gridlog_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
          {
          }
          protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
          {
              DataTable Costmarket = new DataTable();
              DataTable dtTemp = new DataTable();
              if (Session["SI_ProductDetails"] != null)
              {
                  DataTable dt = new DataTable();
                  dtTemp = (DataTable)Session["SI_ProductDetails"];
                  dt = dtTemp.Copy();
                  foreach (DataRow row in dt.Rows)
                  {
                    DataColumnCollection dtC = dt.Columns;

                  //    if (dtC.Contains("MarketCost_Id"))
                  //    { dt.Columns.Remove("MarketCost_Id"); }
                      
                  //    if (dtC.Contains("srlno"))
                  //    { dt.Columns.Remove("srlno"); }
                      
                  //    if (dtC.Contains("ProductCode"))
                  //    { dt.Columns.Remove("ProductCode"); }
                  //    if (dtC.Contains("Year"))
                  //    { dt.Columns.Remove("Year"); }
                    if (dtC.Contains("Productdescription"))
                    { dt.Columns.Remove("Productdescription"); }
                    if (dtC.Contains("CreatedBy"))
                  { dt.Columns.Remove("CreatedBy"); }
                    if (dtC.Contains("CreatedOn"))
                  { dt.Columns.Remove("CreatedOn"); }
                    if (dtC.Contains("ModifiedBy"))
                  { dt.Columns.Remove("ModifiedBy"); }
                    if (dtC.Contains("ModifiedOn"))
                  { dt.Columns.Remove("ModifiedOn"); }
                  //    if (dtC.Contains("January"))
                  //    { dt.Columns.Remove("January"); }

                  //    if (dtC.Contains("February"))
                  //    { dt.Columns.Remove("February"); }
                  //    if (dtC.Contains("March"))
                  //    { dt.Columns.Remove("March"); }
                  //    if (dtC.Contains("April"))
                  //    { dt.Columns.Remove("April"); }
                  //    if (dtC.Contains("May"))
                  //    { dt.Columns.Remove("May"); }
                  //    if (dtC.Contains("June"))
                  //    { dt.Columns.Remove("June"); }
                  //    if (dtC.Contains("July"))
                  //    { dt.Columns.Remove("July"); } 
                  //    if (dtC.Contains("August"))
                  //    { dt.Columns.Remove("August"); }
                  //    if (dtC.Contains("September"))
                  //    { dt.Columns.Remove("September"); }
                  //    if (dtC.Contains("October"))
                  //    { dt.Columns.Remove("October"); }
                  //    if (dtC.Contains("November"))
                  //    { dt.Columns.Remove("November"); }
                  //    if (dtC.Contains("December"))
                  //    { dt.Columns.Remove("December"); }
                      break;
                  }
                  Costmarket = dt;
              }
              else
              {
                  Costmarket.Columns.Add("MarketCost_Id", typeof(int));
                  Costmarket.Columns.Add("srlno", typeof(int));
                  Costmarket.Columns.Add("ProductCode", typeof(string));
                 // Costmarket.Columns.Add("Amount", typeof(decimal));
                  Costmarket.Columns.Add("Year", typeof(int));
                  //Costmarket.Columns.Add("CreatedBy", typeof(string));
                  //Costmarket.Columns.Add("CreatedOn", typeof(string));
                  //Costmarket.Columns.Add("ModifiedBy", typeof(string));
                  //Costmarket.Columns.Add("ModifiedOn", typeof(decimal));
                  Costmarket.Columns.Add("January", typeof(string));
                  Costmarket.Columns.Add("February", typeof(string));
                  Costmarket.Columns.Add("March", typeof(string));
                  Costmarket.Columns.Add("April", typeof(string));
                  Costmarket.Columns.Add("May", typeof(string));
                  Costmarket.Columns.Add("June", typeof(string));
                  Costmarket.Columns.Add("July", typeof(string));
                  Costmarket.Columns.Add("August", typeof(string));
                  Costmarket.Columns.Add("September", typeof(string));
                  Costmarket.Columns.Add("October", typeof(string));
                  Costmarket.Columns.Add("November", typeof(string));
                  Costmarket.Columns.Add("December", typeof(string));
              }
              int InitVal = Costmarket.Columns.Count + 1;
              foreach (var args in e.InsertValues)
              {
                  //string Particulars = Convert.ToString(args.NewValues["Particulars"]);
                  //decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                  //string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                  //// DataTable Quotationdt = (DataTable)Session["PayReqDetails"];
                  //if (Particulars == "" || Particulars == null)
                  //{
                  //    //grid.JSProperties["cpProductNotExists"] = "Please enter Particulars";
                  //    //return;
                  //}
                  //if (Amount == 0 || Amount == null)
                  //{
                  //    //grid.JSProperties["cpProductNotExists"] = "Please enter First";
                  //    //return;
                  //}




                  //if ((Particulars != "" && Particulars != null) && (Amount != 0 && Amount != null))
                  //{
                  //    if (Paymentreq.Columns.Contains("Paymentrequisition_Number"))
                  //    {
                  //        Paymentreq.Columns.Remove("Paymentrequisition_Number");
                  //    }
                  //    Paymentreq.Rows.Add(InitVal, Particulars, Amount, Remarks);
                  //    InitVal = InitVal + 1;
                  //}

              }
              foreach (var args in e.UpdateValues)
              {
                  // dtTemp = (DataTable)Session["PayReqDetails"];

                  int newsrlno = Convert.ToInt32(args.NewValues["SrlNo"]);
                  //int MarketCost_Id = Convert.ToInt32(args.NewValues["MarketCost_Id"]);
                  string newProductCode = Convert.ToString(args.NewValues["ProductCode"]);
                  string newYear = Convert.ToString(args.NewValues["Year"]);
                  string newJanuary = Convert.ToString(args.NewValues["January"]);
                  string newFebruary = Convert.ToString(args.NewValues["February"]);
                  string newMarch = Convert.ToString(args.NewValues["March"]);
                  string newApril = Convert.ToString(args.NewValues["April"]);
                  string newMay = Convert.ToString(args.NewValues["May"]);
                  string newJune = Convert.ToString(args.NewValues["June"]);
                  string newJuly = Convert.ToString(args.NewValues["July"]);
                  string newAugust = Convert.ToString(args.NewValues["August"]);
                  string newSeptember = Convert.ToString(args.NewValues["September"]);
                  string newOctober = Convert.ToString(args.NewValues["October"]);
                  string newNovember = Convert.ToString(args.NewValues["November"]);
                  string newDecember = Convert.ToString(args.NewValues["December"]);
                //if ((newParticulars != "" && newParticulars != null) && (newAmount != 0 && newAmount != null))
                //  {
                      bool Isexists = false;
                      foreach (DataRow drr in Costmarket.Rows)
                      {
                          int oldsrlno = Convert.ToInt32(drr["SrlNo"]);
                          string oldProductCode = Convert.ToString(drr["ProductCode"]);
                          string MarketCost_Id = Convert.ToString(drr["srlno"]);
                          string oldYear = Convert.ToString(drr["Year"]);
                          string oldJanuary = Convert.ToString(drr["January"]);
                          string oldFebruary = Convert.ToString(drr["February"]);
                          string oldMarch = Convert.ToString(drr["March"]);
                          string oldApril = Convert.ToString(drr["April"]);
                          string oldMay = Convert.ToString(drr["May"]);
                          string oldJune = Convert.ToString(drr["June"]);
                          string oldJuly = Convert.ToString(drr["July"]);
                          string oldAugust = Convert.ToString(drr["August"]);
                          string oldSeptember = Convert.ToString(drr["September"]);
                          string oldOctober = Convert.ToString(drr["October"]);
                          string oldNovember = Convert.ToString(drr["November"]);
                          string oldDecember = Convert.ToString(drr["December"]);
                          //decimal OldAmount = Convert.ToDecimal(drr["Amount"]);
                          //string oldsrnl = drr["SrlNo"].ToString();

                          if (oldsrlno == newsrlno)
                          {
                              Isexists = true;
                              drr["SrlNo"] = newsrlno;
                              drr["srlno"] = MarketCost_Id;
                              drr["ProductCode"] = newProductCode;
                              //drr["Year"] = newYear;
                              drr["January"] = newJanuary;
                              drr["February"] = newFebruary;
                              drr["March"] = newMarch;
                              drr["April"] = newApril;
                              drr["May"] = newMay;
                              drr["June"] = newJune;
                              drr["July"] = newJuly;
                              drr["August"] = newAugust;
                              drr["September"] = newSeptember;
                              drr["October"] = newOctober;
                              drr["November"] = newNovember;
                              drr["December"] = newDecember;
                              //drr["Year"] = newAmount;
                              break;
                          }
                      }

                      if (Isexists == false)
                      {
                          Costmarket.Rows.Add(InitVal, newProductCode, newYear, newJanuary, newFebruary, newMarch, newApril, newMay, newJune, newAugust, newSeptember, newOctober,newNovember, newDecember);
                      }
                    
                      Costmarket.AcceptChanges();
                      
                  //}
              }
              if (hdnflag.Value == "1")
              {
                  Modifyproductcost(Costmarket, "Update");
              }
          }
          public int Modifyproductcost(DataTable Costmarket, string mode)
          {

              //if (Costmarket.Columns.Contains("SrlNo"))
              //{
              //    Costmarket.Columns.Remove("SrlNo");
              //}
              //if (Costmarket.Columns.Contains("Year"))
              //{
              //    Costmarket.Columns.Remove("Year");
              //}
              DataTable dtCmb = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("Proc_UpdateProductsCostMarketPrice");
              proc.AddPara("@PRODUCTIMPORT", Costmarket);
              proc.AddPara("@user_Id", Convert.ToInt32(Session["userid"]));
             // proc.AddPara("@year", hdnyear.Value.ToString());
              dtCmb = proc.GetTable();
              //productlog.DataSource = dtCmb;
              //productlog.DataBind();

              //Session["Datlog"] = dtCmb;
              Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Products Imported Succesfully!')</script>");
              Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script>hideimportpopup();</script>");
            
              return 0;

          }
             
          protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          {
          }
          protected void gridlog_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          {
          }
          protected void gridlog_DataBinding(object sender, EventArgs e)
          {
             
           }
          protected void grid_DataBinding(object sender, EventArgs e)
          {
 
              //if (Session["PayReqDetails"] != null)
              //{
              //    DataTable Quotationdt = (DataTable)Session["PayReqDetails"];
              //    DataView dvData = new DataView(Quotationdt);
              //    //dvData.RowFilter = "Status <> 'D'";
              //    grid.DataSource = GetPayReq(dvData.ToTable());

              //}
              string year = string.Empty;
              year = hdngridyear.Value;
              DataTable dtCmb = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("Proc_FillProductCostMarket");
              proc.AddPara("@user_Id", Convert.ToInt32(Session["userid"]));
              proc.AddVarcharPara("@year",100, year);
              dtCmb = proc.GetTable();
              DataView dvData = new DataView(dtCmb);
              Session["SI_ProductDetails"] = dtCmb;
              grid.DataSource = Getlisting(dvData.ToTable());
          }
          public IEnumerable Getlisting(DataTable dvData)
          
          {
              List<listinginbatchgrid> OrderList = new List<listinginbatchgrid>();
              DataColumnCollection dtC = dvData.Columns;
              for (int i = 0; i < dvData.Rows.Count; i++)
              {
                  listinginbatchgrid listinginbatchgrid = new listinginbatchgrid();

                  listinginbatchgrid.SrlNo = Convert.ToString(dvData.Rows[i]["srlno"]);
                  //Orders.OrderID = Convert.ToString(SalesOrderdt.Rows[i]["OrderID"]);
                  listinginbatchgrid.ProductCode = Convert.ToString(dvData.Rows[i]["ProductCode"]);
                  listinginbatchgrid.January = Convert.ToString(dvData.Rows[i]["January"]);
                  listinginbatchgrid.February = Convert.ToString(dvData.Rows[i]["February"]);
                  listinginbatchgrid.March = Convert.ToString(dvData.Rows[i]["March"]);
                  listinginbatchgrid.April = Convert.ToString(dvData.Rows[i]["April"]);
                  listinginbatchgrid.May = Convert.ToString(dvData.Rows[i]["May"]);
                  listinginbatchgrid.June = Convert.ToString(dvData.Rows[i]["June"]);
                  listinginbatchgrid.July = Convert.ToString(dvData.Rows[i]["July"]);
                  listinginbatchgrid.August = Convert.ToString(dvData.Rows[i]["August"]);
                  listinginbatchgrid.September = Convert.ToString(dvData.Rows[i]["September"]);
                  listinginbatchgrid.October = Convert.ToString(dvData.Rows[i]["October"]);
                  listinginbatchgrid.November = Convert.ToString(dvData.Rows[i]["November"]);
                  listinginbatchgrid.December = Convert.ToString(dvData.Rows[i]["December"]);
                  listinginbatchgrid.Marketcost_id = Convert.ToString(dvData.Rows[i]["srlno"]);
                  listinginbatchgrid.Productdescription = Convert.ToString(dvData.Rows[i]["Productdescription"]);
                  OrderList.Add(listinginbatchgrid);
              }

              return OrderList;
          }
          public class listinginbatchgrid
          {
              public string Marketcost_id { get; set; }
              public string Productdescription { get; set; }
              public string SrlNo { get; set; }
              public string ProductCode { get; set; }
              public string January { get; set; }
              public string February { get; set; }
              public string March { get; set; }
              public string April { get; set; }
              public string May { get; set; }
              public string June { get; set; }
              public string July { get; set; }
              public string August { get; set; }
              public string September { get; set; }
              public string October { get; set; }
              public string November { get; set; }
              public string December { get; set; }

          }
          protected void gridlog_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
          {
             
              //if (e.Column.FieldName == "SrlNo")
              //{ e.Editor.ReadOnly = true; }
              //else
              //{ e.Editor.ReadOnly = false; }

              
          }
          protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
          {
             
              //if (e.Column.FieldName == "SrlNo")
              //{ e.Editor.ReadOnly = true; }
              //else
              //{ e.Editor.ReadOnly = false; }

              
          }
          protected void gridlog_RowInserting(object sender, ASPxDataInsertingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void gridlog_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void gridlog_RowDeleting(object sender, ASPxDataUpdatingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
          {
              e.Cancel = true;
          }
          protected void gridlog_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
          {

          }
          protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
          {

          }
          protected void gridlog_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
          {

          }
          protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
          {

          }
          protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
          {
              //if (e.Column.FieldName == "Number")
              //{
              //    e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
              //}
              //if (e.Column.FieldName == "Warehouse")
              //{
              //    //e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
              //    //e.Row.Cells[6].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
              //}

          }
    }
}
