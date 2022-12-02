using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class StockReceiptModel
    {
        string ConnectionString = String.Empty;
        public StockReceiptModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetStockReceiptData(string Action = null, Int64 ID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_StockReceiptDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ID", ID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable StockReceiptInsertUpdate(string Action = null, StockReceiptViewModel obj = null, String LastCompany = null, String LastFinYear = null, DataTable Warehouse = null, DataTable WarehouseFresh = null, DataTable dtWarehouseWC = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_StockReceiptInsertUpdate");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@StockReceiptID", obj.StockReceiptID);
            proc.AddPara("@QualityControlID", obj.QualityControlID);
            // proc.AddPara("@Product_ID", obj.ProductID);
            proc.AddPara("@SchemaID", obj.ReceiptSchemaID);
            proc.AddPara("@Receipt_No", obj.StockReceipt_No);
            proc.AddPara("@Receipt_Date", obj.StockReceiptDate);

            proc.AddPara("@Fresh_ReceiptQty", obj.FreshQuantity);
            proc.AddPara("@Rejected_ReceiptQty", obj.RejectedQuantity);
            proc.AddPara("@UserID", obj.UserID);

            proc.AddPara("@Product_ID", obj.ProductID);
            proc.AddPara("@CompanyID", LastCompany);
            proc.AddPara("@FinYear", LastFinYear);

            proc.AddPara("@ProductionIssueID", obj.ProductionIssueID);
            proc.AddPara("@Doctype", obj.Doctype);
            proc.AddPara("@DetailsID", obj.Details_ID);
            proc.AddPara("@WorkCenterID", obj.WorkCenterID);

            if (Warehouse != null)
            {
                foreach (DataRow dr in Warehouse.Rows)
                {
                    IFormatProvider culture = new CultureInfo("en-US", true);

                    string _ViewMfgDate = Convert.ToString(dr["ViewMfgDate"]);
                    DateTime dateVal4 = DateTime.ParseExact(_ViewMfgDate, "dd-MM-yyyy", culture);
                    string ViewMfgDate = dateVal4.ToString("yyyy-MM-dd");


                    string _ViewExpiryDate = Convert.ToString(dr["ViewExpiryDate"]);
                    DateTime dateVal5 = DateTime.ParseExact(_ViewExpiryDate, "dd-MM-yyyy", culture);
                    string ViewExpiryDate = dateVal5.ToString("yyyy-MM-dd");

                    if (ViewMfgDate != null)
                    {
                        dr["ViewMfgDate"] = ViewMfgDate;
                    }
                    if (ViewExpiryDate != null)
                    {
                        dr["ViewExpiryDate"] = ViewExpiryDate;
                    }

                }
                Warehouse.AcceptChanges();


                if (Warehouse.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILS", Warehouse);
                }
            }

            if (WarehouseFresh != null)
            {

                //if (Warehouse.Rows.Count == 0)
                //{
                    foreach (DataRow dr1 in WarehouseFresh.Rows)
                    {
                        IFormatProvider culture = new CultureInfo("en-US", true);

                        string _ViewMfgDate = Convert.ToString(dr1["ViewMfgDate"]);
                        DateTime dateVal1 = DateTime.ParseExact(_ViewMfgDate, "dd-MM-yyyy", culture);
                        string ViewMfgDate = dateVal1.ToString("yyyy-MM-dd");


                        string _ViewExpiryDate = Convert.ToString(dr1["ViewExpiryDate"]);
                        DateTime dateVal = DateTime.ParseExact(_ViewExpiryDate, "dd-MM-yyyy", culture);
                        string ViewExpiryDate = dateVal.ToString("yyyy-MM-dd");

                        if (ViewMfgDate != null)
                        {
                            dr1["ViewMfgDate"] = ViewMfgDate;
                        }
                        if (ViewExpiryDate != null)
                        {
                            dr1["ViewExpiryDate"] = ViewExpiryDate;
                        }

                    }
                    WarehouseFresh.AcceptChanges();
               // }
                if (WarehouseFresh.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILSFresh", WarehouseFresh);
                }
            }

            if (dtWarehouseWC != null)
            {

                if (WarehouseFresh.Rows.Count == 0)
                {
                    foreach (DataRow dr in dtWarehouseWC.Rows)
                    {
                        IFormatProvider culture = new CultureInfo("en-US", true);

                        string _ViewMfgDate = Convert.ToString(dr["ViewMfgDate"]);
                        DateTime dateVal2 = DateTime.ParseExact(_ViewMfgDate, "dd-MM-yyyy", culture);
                        string ViewMfgDate = dateVal2.ToString("yyyy-MM-dd");


                        string _ViewExpiryDate = Convert.ToString(dr["ViewExpiryDate"]);
                        DateTime dateVal3 = DateTime.ParseExact(_ViewExpiryDate, "dd-MM-yyyy", culture);
                        string ViewExpiryDate = dateVal3.ToString("yyyy-MM-dd");

                        if (ViewMfgDate != null)
                        {
                            dr["ViewMfgDate"] = ViewMfgDate;
                        }
                        if (ViewExpiryDate != null)
                        {
                            dr["ViewExpiryDate"] = ViewExpiryDate;
                        }

                    }
                    dtWarehouseWC.AcceptChanges();
                }
                if (dtWarehouseWC.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILSWC", dtWarehouseWC);
                }
            }

            proc.AddPara("@FGPrice", obj.FGPrice);
            proc.AddPara("@TotalAmount", obj.TotalAmount);

            ds = proc.GetTable();
            return ds;
        }
    }
}