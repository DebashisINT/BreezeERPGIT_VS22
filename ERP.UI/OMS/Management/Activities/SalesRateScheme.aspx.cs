using ClosedXML.Excel;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesRateScheme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Datlog"] = null;
                Fromdt.Date = DateTime.Now;
                ToDate.Date = DateTime.Now;

                Hiddenvalidfrom.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Hiddenvalidupto.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                BindCalculatedOn();
            }
        }


        public void BindCalculatedOn()
        {
            ddlCalculatedOn.DataSource = GetScheme();
            ddlCalculatedOn.DataValueField = "SaleRateLockID";
            ddlCalculatedOn.DataTextField = "SchemeName";
            ddlCalculatedOn.DataBind();
            ddlCalculatedOn.SelectedIndex = 0;
        }

        public DataTable GetScheme()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
            proc.AddPara("@Action", "GetScheme");
            DataTable dt = proc.GetTable();
            return dt;
        }

        [WebMethod(EnableSession = true)]
        public static object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing,Type   from v_SaleRateLock_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"]),
                                TYPE = Convert.ToString(dr["Type"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object GetProduct(string SearchKey)
        {
            List<PosProductModel> listCust = new List<PosProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10  Products_ID,Products_Name ,Products_Description ,sProduct_MinSalePrice  from v_Product_SaleRateLock where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");

                listCust = (from DataRow dr in cust.Rows
                            select new PosProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                MinSalePrice = Convert.ToString(dr["sProduct_MinSalePrice"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string addSaleRateLock(string SaleRateLockID, List<String> CustID, List<String> ProductID, string DiscSalesPrice, string MinSalePrice,
            string discount, string fromdt, string todate, string action, string FixRate, String SCHEME, String SchemeCode, int IsActive, String RateType, String MaxSalePrice
            , String Method, String CalculatedOn, String CalculationBasis, String Margin)
        {
            try
            {
                DateTime fdt = Convert.ToDateTime(fromdt);
                DateTime tdt = Convert.ToDateTime(todate);
                if (fdt > tdt)
                {
                    return "-12";
                }
                else if (fdt == tdt)
                {
                    return "-13";
                }
                else
                {
                    //DataTable dtvalue = new DataTable();
                    ////dtvalue = null;
                    ////if (dtvalue == null)
                    ////{
                    //dtvalue.Columns.Add("Entity", typeof(String));
                    //dtvalue.Columns.Add("Product", typeof(String));
                    ////}

                    String PRODUCT_ID = "";
                    String CUSTOMER_CODE = "";

                    foreach (String item in CustID)
                    {
                        if (CUSTOMER_CODE == "")
                        {
                           // Mantis Issue 25020
                            //CUSTOMER_CODE = item;
                            CUSTOMER_CODE = item.Trim();
                            // End of Mantis Issue 25020
                        }
                        else
                        {
                            // Mantis Issue 25020
                            //CUSTOMER_CODE = CUSTOMER_CODE + "," + item;
                            CUSTOMER_CODE = CUSTOMER_CODE + "," + item.Trim();
                            // End of Mantis Issue 25020
                        }
                    }

                    foreach (String prod in ProductID)
                    {
                        if (PRODUCT_ID == "")
                        {
                            // Mantis Issue 25020
                            //PRODUCT_ID = prod.ToString();
                            PRODUCT_ID = prod.ToString().Trim();
                            // End of Mantis Issue 25020
                        }
                        else
                        {
                            // Mantis Issue 25020
                            //PRODUCT_ID = PRODUCT_ID + "," + prod.ToString();
                            PRODUCT_ID = PRODUCT_ID + "," + prod.ToString().Trim();
                            // End of Mantis Issue 25020
                        }
                    }

                    //List<String> ProductList = ProductID.Split(',');
                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                    ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
                    proc.AddVarcharPara("@SaleRateLockID", 50, SaleRateLockID);
                    //proc.AddVarcharPara("@CustomerID", 50, CustID);
                    // proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductID));
                    proc.AddDecimalPara("@DiscSalesPrice", 2, 18, Convert.ToDecimal(DiscSalesPrice));
                    proc.AddIntegerPara("@ApprovedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@ValidFrom", 50, fdt.ToString("yyyy-MM-dd HH:mm:ss"));
                    proc.AddVarcharPara("@ValidUpto", 50, tdt.ToString("yyyy-MM-dd HH:mm:ss"));
                    proc.AddDecimalPara("@MinSalePrice", 2, 18, Convert.ToDecimal(MinSalePrice));
                    proc.AddDecimalPara("@Disc", 2, 18, Convert.ToDecimal(discount));
                    proc.AddVarcharPara("@Action", 4000, action);
                    //proc.AddDecimalPara("@FixedRate", 2, 18, Convert.ToDecimal(FixRate));
                    proc.AddPara("@PRODUCT_ID", PRODUCT_ID);
                    proc.AddPara("@CUSTOMER_CODE", CUSTOMER_CODE);
                    proc.AddVarcharPara("@SchemeCode", 100, SchemeCode);
                    proc.AddVarcharPara("@SchemeName", 300, SCHEME);
                    proc.AddPara("@IsActive", IsActive);
                    proc.AddVarcharPara("@RateType", 300, RateType);
                    proc.AddDecimalPara("@MaxSalePrice", 2, 18, Convert.ToDecimal(MaxSalePrice));

                    proc.AddPara("@Method", Method);
                    proc.AddPara("@CalculatedOn", CalculatedOn);
                    proc.AddPara("@CalculationBasis", CalculationBasis);
                    proc.AddDecimalPara("@Margin", 2, 18, Convert.ToDecimal(Margin));
                    //proc.AddPara("@UDT_RATELIST", dtvalue);
                    DataTable dtSaleRateLock = proc.GetTable();
                    if (dtSaleRateLock.Rows.Count > 0)
                    {
                        if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-11")
                        {
                            return "-11";
                        }
                        else if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-33")
                        {
                            return "-33";
                        }
                        else
                        {
                            return "1";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string DeleteSaleRateLock(string SaleRateLockID)
        {
            try
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
                proc.AddVarcharPara("@SaleRateLockID", 50, SaleRateLockID);
                proc.AddVarcharPara("@Action", 4000, "delete");
                DataTable dtSaleRateLock = proc.GetTable();
                if (dtSaleRateLock.Rows.Count > 0)
                {
                    if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-999")
                    {
                        return "-999";
                    }
                    else if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-998")
                    {
                        return "-998";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SaleRateLockID";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.v_SaleRateLockLists
                    orderby d.SaleRateLockID descending
                    select d;
            e.QueryableSource = q;
        }

        protected void GridSaleRate_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static List<SaleRateLock> GetSaleRateLock(string SaleRateLockID)
        {
            List<SaleRateLock> listSaleRateLock = new List<SaleRateLock>();
            List<Products> listProducts = new List<Products>();
            List<Customers> listCustomers = new List<Customers>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
                proc.AddVarcharPara("@Action", 50, "GetSaleRateLockDetails");
                proc.AddVarcharPara("@SaleRateLockID", 10, SaleRateLockID);
                DataSet dtSaleRateLock = proc.GetDataSet();


                listProducts = (from DataRow dr in dtSaleRateLock.Tables[1].Rows
                                select new Products()
                                {
                                    ProductID = Convert.ToString(dr["PRODUCT_ID"])
                                }).ToList();


                listCustomers = (from DataRow dr in dtSaleRateLock.Tables[2].Rows
                                 select new Customers()
                                 {
                                     CustomerID = dr["ENTITY_CODE"].ToString()
                                 }).ToList();


                listSaleRateLock = (from DataRow dr in dtSaleRateLock.Tables[0].Rows
                                    select new SaleRateLock()
                                    {
                                        //CustomerID = dr["CustomerID"].ToString(),
                                        //CustomerName = dr["CustName"].ToString(),
                                        //ProductID = Convert.ToString(dr["ProductID"]),
                                        //Products_Name = Convert.ToString(dr["Products_Name"]),
                                        DiscSalesPrice = dr["DiscSalesPrice"].ToString(),
                                        MinSalePrice = dr["MinSalePrice"].ToString(),
                                        Disc = Convert.ToString(dr["Disc"]),
                                        ValidFrom = Convert.ToString(dr["ValidFrom"]),
                                        ValidUpto = Convert.ToString(dr["ValidUpto"]),
                                        IsInUse = Convert.ToString(dr["IsInUse"]),
                                        //FixRate = Convert.ToString(dr["FixedRate"]),
                                        Scheme = Convert.ToString(dr["Scheme"]),
                                        SchemeCode = Convert.ToString(dr["SchemeCode"]),
                                        IsActive = Convert.ToString(dr["IsActive"]),
                                        RateType = Convert.ToString(dr["RateType"]),
                                        MaxSalePrice = Convert.ToString(dr["MaxSalePrice"]),
                                        CustomersList = listCustomers,
                                        ProductsList = listProducts,
                                        Method = Convert.ToString(dr["Method"]),
                                        CalculatedOn = Convert.ToString(dr["CalculatedOn"]),
                                        CalculationBasis = Convert.ToString(dr["CalculationBasis"]),
                                        MarginPercentage = Convert.ToString(dr["MarginPercentage"])
                                    }).ToList();

            }

            return listSaleRateLock;

        }

        protected void EntityServerModeDataProduct_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "sProductsID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            var q = from d in dc.v_Product_SaleRateLocks
                    orderby d.sProductsID descending
                    select d;

            e.QueryableSource = q;

        }

        protected void EntityServerModeData_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_internalid";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            var q = from d in dc.v_SaleRateLock_customerDetails
                    orderby d.Name //descending
                    select d;
            e.QueryableSource = q;

        }


        public class SaleRateLock
        {
            public int SaleRateLockID { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string ProductID { get; set; }
            public string Products_Name { get; set; }
            public string DiscSalesPrice { get; set; }
            public string MinSalePrice { get; set; }
            public string Disc { get; set; }
            public string ValidFrom { get; set; }
            public string ValidUpto { get; set; }
            public string IsInUse { get; set; }
            public string FixRate { get; set; }
            public string Scheme { get; set; }
            public string SchemeCode { get; set; }
            public string IsActive { get; set; }
            public string RateType { get; set; }
            public string MaxSalePrice { get; set; }
            public List<Customers> CustomersList { get; set; }
            public List<Products> ProductsList { get; set; }

            public string Method { get; set; }
            public string CalculatedOn { get; set; }
            public string CalculationBasis { get; set; }
            public string MarginPercentage { get; set; }
        }

        public class Products
        {
            public string ProductID { get; set; }
        }

        public class Customers
        {
            public string CustomerID { get; set; }
        }

        public class PosProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string MinSalePrice { get; set; }
        }

        public class CustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string add { get; set; }
            public string TYPE { get; set; }
        }

        [WebMethod]
        [HttpGet]
        public static void download(String State)
        {
            DataTable dt = new DataTable();
            try
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
                proc.AddVarcharPara("@Action", 50, "GetImportData");
                proc.AddVarcharPara("@StateID", 10, State);
                dt = proc.GetTable();

                //HttpContext.Current.Session["DownloadData"] = dt;

                GridViewExtension.ExportToXlsx(GetShopListTemplateByAreaExcel(dt, ""), dt, true, getXlsExportOptions());

            }
            catch { }
            // return "true";
        }

        private static XlsxExportOptionsEx getXlsExportOptions()
        {
            DevExpress.XtraPrinting.XlsxExportOptionsEx obj = new DevExpress.XtraPrinting.XlsxExportOptionsEx(DevExpress.XtraPrinting.TextExportMode.Text);
            obj.ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFile;
            obj.ExportType = DevExpress.Export.ExportType.WYSIWYG;

            return obj;
        }

        private static GridViewSettings GetShopListTemplateByAreaExcel(object datatable, String dates)
        {
            var settings = new GridViewSettings();
            settings.Name = "ProductRate";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "ProductRate";
            settings.Name = "ProductRate";

            DataTable dt = (DataTable)datatable;



            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "STATE" || datacolumn.ColumnName == "Code"
                    || datacolumn.ColumnName == "Description" || datacolumn.ColumnName == "PricetoDistributor" || datacolumn.ColumnName == "PricetoRetailer")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "STATE")
                        {
                            column.Caption = "STATE";
                        }
                        else if (datacolumn.ColumnName == "Code")
                        {
                            column.Caption = "Code";
                        }
                        else if (datacolumn.ColumnName == "Description")
                        {
                            column.Caption = "Description";
                        }
                        else if (datacolumn.ColumnName == "PricetoDistributor")
                        {
                            column.Caption = "Price to Distributor";
                        }
                        else if (datacolumn.ColumnName == "PricetoRetailer")
                        {
                            column.Caption = "Price to Retailer";
                        }


                        column.FieldName = datacolumn.ColumnName;

                        if (datacolumn.DataType.FullName == "System.DateTime")
                        {
                            column.PropertiesEdit.DisplayFormatString = "DD-MM-YYYY";
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

        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        protected void gridProductRate_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            
        }

        protected void ProductRateServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SaleRateLockID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.V_SalesRateSchemes
                    //orderby d.ID descending
                    select d;

            e.QueryableSource = q;
        }

        protected void grid_RateLog_DataBinding(object sender, EventArgs e)
        {
            if (Session["Datlog"] != null)
            {
                grid_RateLog.DataSource = (DataTable)Session["Datlog"];
            }
        }

        protected void ProductRateLog_Callback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["Datlog"] != null)
            {
                DataTable ComponentTable = (DataTable)Session["Datlog"];
                grid_RateLog.Selection.CancelSelection();
                grid_RateLog.DataSource = ComponentTable;
                grid_RateLog.DataBind();
            }
            else
            {
                grid_RateLog.Selection.CancelSelection();
                grid_RateLog.DataSource = null;
                grid_RateLog.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            string filename = "Product Rate Import Log";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Product Rate Import Log";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            switch (Filter)
            {
                case 1:
                    using (MemoryStream stream = new MemoryStream())
                    {
                        exporter.WritePdf(stream);
                        WriteToResponse("Area", true, "pdf", stream);
                    }
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

        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
        }

        protected void ComponentQuotationPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];

            if (strSplitCommand == "Customers")
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
                proc.AddVarcharPara("@Action", 50, "GetSaleRateLockDetails");
                proc.AddVarcharPara("@SaleRateLockID", 10, HiddenSaleRateLockID.Value);
                DataSet dtSaleRateLock = proc.GetDataSet();

                foreach (DataRow val in dtSaleRateLock.Tables[2].Rows)
                {
                    lookup_Entity.GridView.Selection.SelectRowByKey(Convert.ToString(val["ENTITY_CODE"]));
                }

                foreach (DataRow val in dtSaleRateLock.Tables[1].Rows)
                {
                    lookup_Product.GridView.Selection.SelectRowByKey(Convert.ToInt32(val["PRODUCT_ID"]));
                }

            }
            //else if (strSplitCommand == "Products")
            //{
            //    ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateScheme");
            //    proc.AddVarcharPara("@Action", 50, "GetSaleRateLockDetails");
            //    proc.AddVarcharPara("@SaleRateLockID", 10, HiddenSaleRateLockID.Value);
            //    DataSet dtSaleRateLock = proc.GetDataSet();

                
            //}
        }

    }
}