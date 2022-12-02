using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;
using System.Web;
using System.Configuration;

namespace BusinessLogicLayer
{
    public class Others
    {
        string rtrnvalue = String.Empty;
        int ReturnValue = 0;

        public DataSet SearchContactBank(string SEARCH_TYPE, string BNK_LIKE)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[SEARCH_CONTACT_BANK]");
            proc.AddVarcharPara("@SEARCH_TYPE", 100, SEARCH_TYPE);
            proc.AddVarcharPara("@BNK_LIKE", 100, BNK_LIKE);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SearchContactByDP(string SEARCH_TYPE, string SEARCH_ENTITY, string DP_LIKE)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[SEARCH_CONTACT_BYDP]");
            proc.AddVarcharPara("@SEARCH_TYPE", 100, SEARCH_TYPE);
            proc.AddVarcharPara("@SEARCH_ENTITY", 100, SEARCH_ENTITY);
            proc.AddVarcharPara("@DP_LIKE", 100, DP_LIKE);
            ds = proc.GetDataSet();
            return ds;
        }
        public int ReassignedSupervisorActivity(string SuID, string ActID, string UserId, string OSuID)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "ReassignedSupervisorActivity");
            proc.AddDecimalPara("@SupervisorID", 2, 18, Convert.ToDecimal(SuID));
            proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActID));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddDecimalPara("@OSupervisorID", 2, 18, Convert.ToDecimal(OSuID));
            i = proc.RunActionQuery();

            return i;
        }


        public DataTable GetSalesManDeactivateDocomentActivity(string cntid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetSalesManDeactivateDocomentActivity");


            proc.AddDecimalPara("@Cnt_id", 2, 18, Convert.ToDecimal(cntid));
            return proc.GetTable();
        }
        public string SettlementInsert(string Settlements_Number, long Settlements_ExchangeSegmentID, string Settlements_Type,
            string Settlements_TypeSuffix, string Settlements_StartDateTime, string Settlements_EndDateTime, string Settlements_FundsPayin,
            string Settlements_FundsPayout, string Settlements_DeliveryPayin, string Settlements_DeliveryPayout, string Settlements_CustConfirmDate,
            string Settlements_IsLocked, int Settlements_CreateUser, string Settlements_CreateDateTime, int Settlements_LastModifyUser,
            string Settlements_FinYear)
        {
            using (ProcedureExecute proc = new ProcedureExecute("SettlementInsert"))
            {
                proc.AddVarcharPara("@Settlements_Number", 100, Settlements_Number);
                proc.AddBigIntegerPara("@Settlements_ExchangeSegmentID", Settlements_ExchangeSegmentID);
                proc.AddVarcharPara("@Settlements_Type", 100, Settlements_Type);
                proc.AddVarcharPara("@Settlements_TypeSuffix", 100, Settlements_TypeSuffix);
                proc.AddVarcharPara("@Settlements_StartDateTime", 100, Settlements_StartDateTime);
                proc.AddVarcharPara("@Settlements_EndDateTime", 100, Settlements_EndDateTime);
                proc.AddVarcharPara("@Settlements_FundsPayin", 100, Settlements_FundsPayin);
                proc.AddVarcharPara("@Settlements_FundsPayout", 100, Settlements_FundsPayout);
                proc.AddVarcharPara("@Settlements_DeliveryPayin", 100, Settlements_DeliveryPayin);
                proc.AddVarcharPara("@Settlements_DeliveryPayout", 100, Settlements_DeliveryPayout);
                proc.AddVarcharPara("@Settlements_CustConfirmDate", 100, Settlements_CustConfirmDate);
                proc.AddVarcharPara("@Settlements_IsLocked", 100, Settlements_IsLocked);
                proc.AddIntegerPara("@Settlements_CreateUser", Settlements_CreateUser);
                proc.AddVarcharPara("@Settlements_CreateDateTime", 100, Settlements_CreateDateTime);
                proc.AddIntegerPara("@Settlements_LastModifyUser", Settlements_LastModifyUser);
                proc.AddVarcharPara("@Settlements_FinYear", 100, Settlements_FinYear);
                rtrnvalue = Convert.ToString(proc.RunActionQuery());
            }
            return rtrnvalue;
        }

        public string GetAvailableStockCheckForSCorBOorIST(DataTable duplicatedt, string BranchId)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);

                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                    DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + ") as branchopenstock");
                    if (dtAvailableStockCheck.Rows.Count > 0)
                    {
                        StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                        if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                        {
                            StockCheckMsg = "MoreThanStock";
                        }
                        if (StockCheck == "0.00")
                        {
                            StockCheckMsg = "ZeroStock";
                        }

                    }
                }
            }//End
            return StockCheckMsg;
        }

        public string GetAvailableStockCheckForOutModules(DataTable duplicatedt, string BranchId, string Date)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);
                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {

                        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                        //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                        DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                        if (dtAvailableStockCheck.Rows.Count > 0)
                        {
                            StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));

                            if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                            {
                                StockCheckMsg = "MoreThanStock";
                                break;
                            }
                            if (StockCheck == "0.00")
                            {
                                StockCheckMsg = "ZeroStock";
                                break;
                            }

                        }
                    }
                }
            }//End
            return StockCheckMsg;
        }

        public string GetAvailableStockCheckSalesChallan(DataTable duplicatedt, string BranchId, string Date, string ActionType, int OutDocNo)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);
                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {

                        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                        //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                        DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                        if (dtAvailableStockCheck.Rows.Count > 0)
                        {
                            StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                            if (ActionType != "Add")
                            {
                                DataTable dtAvailableOldStock = oDBEngine.GetDataTable("SELECT * FROM dbo.tbl_trans_saleschallanProducts where ChallanDetails_ProductId='" + ProductId + "' and ChallanDetails_OrderId='" + OutDocNo + "'");
                                if (dtAvailableOldStock != null && dtAvailableOldStock.Rows.Count > 0)
                                {
                                    StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2) + Math.Round(Convert.ToDecimal(dtAvailableOldStock.Rows[0]["ChallanDetails_Quantity"]), 2));
                                }
                                else
                                {
                                    StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                                }

                                if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                                {
                                    StockCheckMsg = "MoreThanStock";
                                    break;
                                }
                                if (StockCheck == "0.00")
                                {
                                    StockCheckMsg = "ZeroStock";
                                    break;
                                }
                            }
                            else
                            {
                                if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                                {
                                    StockCheckMsg = "MoreThanStock";
                                    break;
                                }
                                if (StockCheck == "0.00")
                                {
                                    StockCheckMsg = "ZeroStock";
                                    break;
                                }
                            }

                        }
                    }
                }
            }//End
            return StockCheckMsg;
        }


        public string GetAvailableStockCheckForSalesOrder(DataTable duplicatedt, string BranchId, string Date)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);
                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {

                        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                        //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                        DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                        if (dtAvailableStockCheck.Rows.Count > 0)
                        {
                            StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));

                            if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                            {
                                StockCheckMsg = "MoreThanStock";
                                break;
                            }
                            if (StockCheck == "0.00")
                            {
                                StockCheckMsg = "ZeroStock";
                                break;
                            }

                        }
                    }
                }
            }//End
            return StockCheckMsg;
        }

        public string getProductIsInventoryExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }

        public string GetAvailableStockCheckForOutModulesBO(DataTable duplicatedt, string BranchId, string Date, string ActionType, int OutDocNo)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);

                    // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                    DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date) + "') as branchopenstock");

                    if (dtAvailableStockCheck.Rows.Count > 0)
                    {
                        StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                        if (ActionType != "Add")
                        {
                            DataTable dtAvailableOldStock = oDBEngine.GetDataTable("SELECT * FROM dbo.tbl_trans_StockOutDetails where StkDetails_ProductId='" + ProductId + "' and StkDetails_StkId='" + OutDocNo + "'");
                            if (dtAvailableOldStock != null && dtAvailableOldStock.Rows.Count > 0)
                            {
                                StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2) + Math.Round(Convert.ToDecimal(dtAvailableOldStock.Rows[0]["StkDetails_Quantity"]), 2));
                            }
                            else
                            {
                                StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                            }

                            if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                            {
                                StockCheckMsg = "MoreThanStock";
                                break;
                            }
                            if (StockCheck == "0.00")
                            {
                                StockCheckMsg = "ZeroStock";
                                break;
                            }

                        }
                        else
                        {
                            if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                            {
                                StockCheckMsg = "MoreThanStock";
                                break;
                            }
                            if (StockCheck == "0.00")
                            {
                                StockCheckMsg = "ZeroStock";
                                break;
                            }
                        }
                    }
                }
            }//End
            return StockCheckMsg;
        }

        public string GetAvailableStockCheckForBTO(DataTable duplicatedt, string BranchId, string Date, string ActionType, int OutDocNo)
        {
            CommonBL ComBL = new CommonBL();
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;

            string StockCheckingComponentBTOBTI = ComBL.GetSystemSettingsResult("StockCheckingComponentBTOBTI");

            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);

                    string IsInventory = getProductIsInventoryExists(Convert.ToString(ProductId));
                     if (IsInventory.ToUpper() != "N")
                     {
                         string IsComponant = getProductIsComponantExists(Convert.ToString(ProductId));
                         if (IsComponant.ToUpper() != "Y" && StockCheckingComponentBTOBTI != "No")
                         {

                             // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                             BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                             //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                             DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date) + "') as branchopenstock");

                             if (dtAvailableStockCheck.Rows.Count > 0)
                             {
                                 StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                                 if (ActionType != "Add")
                                 {
                                     DataTable dtAvailableOldStock = oDBEngine.GetDataTable("SELECT * FROM dbo.tbl_trans_StockOutDetails where StkDetails_ProductId='" + ProductId + "' and StkDetails_StkId='" + OutDocNo + "'");
                                     if (dtAvailableOldStock != null && dtAvailableOldStock.Rows.Count > 0)
                                     {
                                         StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2) + Math.Round(Convert.ToDecimal(dtAvailableOldStock.Rows[0]["StkDetails_Quantity"]), 2));
                                     }
                                     else
                                     {
                                         StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));
                                     }

                                     if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                                     {
                                         StockCheckMsg = "MoreThanStock";
                                         break;
                                     }
                                     if (StockCheck == "0.00")
                                     {
                                         StockCheckMsg = "ZeroStock";
                                         break;
                                     }

                                 }
                                 else
                                 {
                                     if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                                     {
                                         StockCheckMsg = "MoreThanStock";
                                         break;
                                     }
                                     if (StockCheck == "0.00")
                                     {
                                         StockCheckMsg = "ZeroStock";
                                         break;
                                     }
                                 }
                             }
                         }
                     }
                }
            }//End
            return StockCheckMsg;
        }
        public string getProductIsComponantExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            proc.AddVarcharPara("@Action", 50, "CheckedComponantProduct");

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                IsInventory = Convert.ToString(dt.Rows[0]["ComponentProduct"]);
            }
            return IsInventory;
        }
        public void BatchRegenerate(string dp, string reason, string user)
        {
            using (ProcedureExecute proc = new ProcedureExecute("batch_regenerate"))
            {
                proc.AddVarcharPara("@dp", 100, dp);
                proc.AddVarcharPara("@reason", 100, reason);
                proc.AddVarcharPara("@reason", 100, reason);
                proc.RunActionQuery();
            }
        }

        public DataSet FetchMissingImportDatesNsdl(string startDate, string endDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_MissingImportDates_Nsdl");
            proc.AddVarcharPara("@startDate", 100, startDate);
            proc.AddVarcharPara("@endDate", 100, endDate);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet FetchMissingImportDatesCdsl(string startDate, string endDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_MissingImportDates_Cdsl");
            proc.AddVarcharPara("@startDate", 100, startDate);
            proc.AddVarcharPara("@endDate", 100, endDate);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SpImportNsdlBPList1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_MissingImportDates_Cdsl");
            proc.AddVarcharPara("@file_path", 300, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
            proc.AddVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public void SpImportNsdlCalendarTest(string file_path, string userid, string create_modify_datetime, string fromdate, string todate)
        {
            using (ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlCalendar_test"))
            {
                proc.AddVarcharPara("@file_path", 300, file_path);
                proc.AddVarcharPara("@userid", 100, userid);
                proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
                proc.AddVarcharPara("@fromdate", 100, fromdate);
                proc.AddVarcharPara("@todate", 100, todate);
                proc.RunActionQuery();
            }
        }




        public DataTable PopulateSupervisorHistory(string ActivityId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActivityId));
            proc.AddVarcharPara("@Mode", 30, "SupervisorHistory");

            ds = proc.GetTable();
            return ds;
        }
        public DataSet ReportPledgedStocks(string Companyid, string Finyear, string Client, string PledgedStocksID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_PledgedStocks]");
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Finyear", 100, Finyear);
            proc.AddVarcharPara("@Client", 100, Client);
            proc.AddVarcharPara("@PledgedStocksID", 100, PledgedStocksID);
            ds = proc.GetDataSet();
            return ds;
        }
        public void InsertPledgedStocks(string CompanyId, string Segmentid, string Finyear, string Client,
            string Productid, string PurchaseDate, decimal Quatity, string PledgeDate, string UnPledgeDate,
            string CreateUser, string UserSelection, string PledgedStocksID, string Pledgee)
        {
            using (ProcedureExecute proc = new ProcedureExecute("Insert_PledgedStocks"))
            {
                proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                proc.AddVarcharPara("@Segmentid", 100, Segmentid);
                proc.AddVarcharPara("@Finyear", 100, Finyear);
                proc.AddVarcharPara("@Client", 100, Client);
                proc.AddVarcharPara("@Productid", 100, Productid);
                proc.AddVarcharPara("@PurchaseDate", 100, PurchaseDate);
                proc.AddDecimalPara("@Quatity", 0, 28, Quatity);
                proc.AddVarcharPara("@PledgeDate", 100, PledgeDate);
                proc.AddVarcharPara("@UnPledgeDate", 100, UnPledgeDate);
                proc.AddVarcharPara("@CreateUser", 100, CreateUser);
                proc.AddVarcharPara("@UserSelection", 100, UserSelection);
                proc.AddVarcharPara("@PledgedStocksID", 100, PledgedStocksID);
                proc.AddVarcharPara("@Pledgee", 100, Pledgee);
                proc.RunActionQuery();
            }
        }

        public DataSet InterestCalculation(string CompanyId, string SegmentID, string Segment, string FinYear,
            string FromDate, string ToDate, string MainAccount, string Client, int CreateUser, string GenType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[InterestCalculation]");
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@SegmentID", 100, SegmentID);
            proc.AddVarcharPara("@Segment", 100, Segment);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@FromDate", 100, FromDate);
            proc.AddVarcharPara("@ToDate", 100, ToDate);
            proc.AddVarcharPara("@MainAccount", 100, MainAccount);
            proc.AddVarcharPara("@Client", 100, Client);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@GenType", 100, GenType);
            ds = proc.GetDataSet();
            return ds;
        }
        public int InterestCalculationIns(string AllData, string CompanyId, string Segment, string FromDate, string ToDate,
            string Finyear, int CreateUser, string MainAccountSubLedgerType)
        {
            using (ProcedureExecute proc = new ProcedureExecute("[InterestCalculation]"))
            {
                proc.AddVarcharPara("@AllData", -1, AllData);
                proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                proc.AddVarcharPara("@Segment", 100, Segment);
                proc.AddVarcharPara("@FromDate", 100, FromDate);
                proc.AddVarcharPara("@ToDate", 100, ToDate);
                proc.AddVarcharPara("@Finyear", 100, Finyear);
                proc.AddIntegerPara("@CreateUser", CreateUser);
                proc.AddVarcharPara("@MainAccountSubLedgerType", 100, MainAccountSubLedgerType);

                ReturnValue = proc.RunActionQuery();
            }
            return ReturnValue;
        }
        public int InterestCalculationIns_2(string AllData, string CompanyId, string Segment, string FromDate, string ToDate)
        {
            using (ProcedureExecute proc = new ProcedureExecute("[InterestCalculation]"))
            {
                proc.AddVarcharPara("@AllData", -1, AllData);
                proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                proc.AddVarcharPara("@Segment", 100, Segment);
                proc.AddVarcharPara("@FromDate", 100, FromDate);
                proc.AddVarcharPara("@ToDate", 100, ToDate);
                ReturnValue = proc.RunActionQuery();
            }
            return ReturnValue;
        }

        public string GetProductType(string Products_ID)
        {
            string productID = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[AddNewProduct]");

            proc.AddIntegerPara("@sProducts_ID", Convert.ToInt32(Products_ID));
            proc.AddVarcharPara("@Module", 20, "GetProductType");
            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                productID = Convert.ToString(dt.Rows[0]["sProducts_ID"]);

            }
            return productID;
        }


        public DataTable GatSales()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetSales");

            dt = proc.GetTable();
            return dt;
        }

        public int DeleteSalesActivity(int SalesActivityID)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "DeleteSalesActivity");
            proc.AddIntegerPara("@SalesActivityID", SalesActivityID);

            i = proc.RunActionQuery();

            return i;
        }


        public int ClosedStatusActivity(string UserId, string sls_ID)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "ClosedStatusActivity");
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));

            i = proc.RunActionQuery();

            return i;
        }

        public int ClosedClarificationStatusActivity(string UserId, string sls_ID, string Remarks)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "ClosedClarificationStatusActivity");
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddNVarcharPara("@Remarks", 200, Remarks);
            i = proc.RunActionQuery();

            return i;
        }
        public DataTable GetActivityTypeList()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetActivityTypeList");

            dt = proc.GetTable();
            return dt;
        }




        public DataTable GetSalesVisitInfo(string SalesActivityID, string sls_ID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetSalesVisitInfo");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            dt = proc.GetTable();
            return dt;
        }
        //public DataTable GetPhoneInfo(string SalesActivityID,string cntid )
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
        //    proc.AddVarcharPara("@Mode", 50, "GetPhoneInfo");
        //    proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
        //    proc.AddIntegerPara("@cntid", Convert.ToInt32(cntid));
        //    dt = proc.GetTable();
        //    return dt;
        //}

        public DataTable GetPhoneInfo(string SalesActivityID, string cntid, string sls_ID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetPhoneInfo");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
            proc.AddIntegerPara("@cntid", Convert.ToInt32(cntid));
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetNoteInfo(string SalesActivityID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetNoteInfo");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetNoteSaleInfo(string SalesActivityID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetNoteSaleInfo");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetOtherInfo(string SalesActivityID, string cntid, string sls_ID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetOtherInfo");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesActivityID));
            proc.AddIntegerPara("@cntid", Convert.ToInt32(cntid));
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetActivityTypeList(String NoteId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");

            proc.AddVarcharPara("@Mode", 50, "GetActivityTypeListByNoteId");
            proc.AddVarcharPara("@NoteId", 50, NoteId);


            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetActivityTypeListBySalesActivity(String ActivityId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");

            proc.AddVarcharPara("@Mode", 50, "GetActivityTypeBySalesId");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(ActivityId));


            dt = proc.GetTable();
            return dt;
        }
        public void setTasksActivity(string task_title, string activity_list, string activity_mode, int activity_taskid)
        {
            ProcedureExecute proc = new ProcedureExecute("[AddTaskActivity]");
            proc.AddVarcharPara("@task_title", 255, task_title);
            proc.AddVarcharPara("@activity_list", 100, activity_list);
            proc.AddVarcharPara("@activity_mode", 5, activity_mode);
            proc.AddIntegerPara("@activity_taskid", activity_taskid);
            proc.RunActionQuery();
        }


        public void UpdateNextActivityDate(string sls_ID, string ActivityDate)
        {
            using (ProcedureExecute proc = new ProcedureExecute("sp_sales"))
            {
                proc.AddNVarcharPara("@Mode", 100, "UpdateActivityDate");
                proc.AddDateTimePara("@NextActivityDate", Convert.ToDateTime(ActivityDate));
                proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
                proc.RunActionQuery();
            }
        }
        public int ReassignedActivity(string AssignedID, string sls_ID, string UserId, string Remarks)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "ReassignedActivity");
            proc.AddDecimalPara("@AssignedID", 2, 18, Convert.ToDecimal(AssignedID));
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddNVarcharPara("@Remarks", 200, Remarks);
            i = proc.RunActionQuery();

            return i;
        }

        public int ReassignedSalesmanActivity(string AssignedID, string ActivityId, string UserId, string Remarks)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "ReassignedSalesmanActivity");
            proc.AddDecimalPara("@AssignedID", 2, 18, Convert.ToDecimal(AssignedID));
            proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActivityId));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddNVarcharPara("@Remarks", 200, Remarks);
            i = proc.RunActionQuery();

            return i;
        }
        public DataTable GetActivityTypeListBySalesActivity()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");

            proc.AddVarcharPara("@Mode", 50, "GetSalesActivityDetailsByNewCallDisposition");


            dt = proc.GetTable();
            return dt;
        }


        public int FeedbackActivity(string DetailsId, string UserId, string Remarks, string Tid, string sls_ID)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "FeedbackActivity");
            proc.AddIntegerPara("@DetailsId", Convert.ToInt32(DetailsId));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddNVarcharPara("@Remarks", 200, Remarks);
            proc.AddNVarcharPara("@Tid", 200, Tid);
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            i = proc.RunActionQuery();
            return i;
        }
        public int FeedbackActivity(string DetailsId, string UserId, string Remarks, string Tid, string sls_ID, string type, string nxtdate)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "FeedbackActivity");
            proc.AddIntegerPara("@DetailsId", Convert.ToInt32(DetailsId));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(UserId));
            proc.AddNVarcharPara("@Remarks", 200, Remarks);
            proc.AddNVarcharPara("@Tid", 200, Tid);
            proc.AddNVarcharPara("@Type", 50, type);
            proc.AddDateTimePara("@NextActivityDate", Convert.ToDateTime(nxtdate));
            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            i = proc.RunActionQuery();
            return i;
        }


        public DataTable GetFeedbackgethistorydetails(string FedId, string TypeId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_FeedbackDetails]");

            proc.AddIntegerPara("@ProdId", Convert.ToInt32(FedId));
            proc.AddIntegerPara("@TypeId", Convert.ToInt32(TypeId));
            dt = proc.GetTable();
            return dt;
        }

        public int EditCallDispositionIsActiveStatus(string call_id, string IsActive)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "EditCallDispositionIsActiveStatus");
            proc.AddIntegerPara("@call_id", Convert.ToInt32(call_id));
            proc.AddBooleanPara("@IsActive", Convert.ToBoolean(IsActive));
            i = proc.RunActionQuery();

            return i;
        }
        public DataTable GetAllCustomer(string BranchID, string AllUserCntId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");

            proc.AddVarcharPara("@Mode", 50, "GetAllCustomer");
            proc.AddNVarcharPara("@cnt_branchid", 250, BranchID);
            proc.AddNVarcharPara("@AllUserCntId", 250, AllUserCntId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetAllCustomerByIndustryId(string BranchID, string AllUserCntId, string IndustryID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");

            proc.AddVarcharPara("@Mode", 50, "GetAllCustomerByIndustryID");
            proc.AddNVarcharPara("@cnt_branchid", 250, BranchID);
            proc.AddIntegerPara("@IndustryID", Convert.ToInt32(IndustryID));
            proc.AddNVarcharPara("@AllUserCntId", 250, AllUserCntId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProductByActivity(string ActivityId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetSalesProduct");
            proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActivityId));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProductCellsByActivity(string SalesId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[sp_Sales]");
            proc.AddVarcharPara("@Mode", 50, "GetSalesProductClass");
            proc.AddIntegerPara("@SalesActivityID", Convert.ToInt32(SalesId));
            dt = proc.GetTable();
            return dt;
        }


        public int EditSalesVisitOutcomeIsActiveStatus(string slv_id, string IsActive)
        {
            int i;

            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddNVarcharPara("@Mode", 100, "EditSalesVisitOutcomeIsActiveStatus");
            proc.AddIntegerPara("@slv_id", Convert.ToInt32(slv_id));
            proc.AddBooleanPara("@IsActive", Convert.ToBoolean(IsActive));
            i = proc.RunActionQuery();

            return i;
        }
        public DataTable GetQuotationOnSalesOrder(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetQuotationOnSalesOrder");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);
            //proc.AddVarcharPara("@ModType", 10, Type);
            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }



        public DataTable GetDocOnSalesOrder(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory, string Type)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_GetDocOnSalesOrder");



            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);

            proc.AddVarcharPara("@ModType", 10, Type);


            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetDocOnContactOrder(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory, string Type)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_GetDocOnContractOrder");



            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);

            proc.AddVarcharPara("@ModType", 10, Type);


            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetDocOnProjectSalesOrder(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory, string Type)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_GetDocOnProjectOrder");



            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);

            proc.AddVarcharPara("@ModType", 10, Type);


            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInquiryOnSalesQuotation(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetInquiryOnSalesQuotation");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);
            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInquiryOnProjectQuotation(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetInquiryOnProjectQuotation");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);
            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetQuotationOnProjectOrder(string customer, string OrderDate, string Status, Int32 BranchId, string Inventory)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetQuotationOnProjectOrder");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            DateTime dtime = Convert.ToDateTime(OrderDate);
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            //proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@IsInventory", 10, Inventory);
            //proc.AddIntegerPara("@Branch_Id", BranchId);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetSalesOrderonSalesChallan(string customer, string ChallanDate, string Status, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetSalesOrderOnSalesChallan");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            proc.AddDateTimePara("@Challandate", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            //proc.AddIntegerPara("@Branch_Id", BranchId);
            proc.AddVarcharPara("@userbranchlist", 2000, BranchId);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceonSalesChallan(string customer, string ChallanDate, string Status, int BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetSalesInvoiceOnSalesOrder");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            proc.AddDateTimePara("@InvoiceDate", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@Branch_Id", BranchId);
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectSalesInvoiceOnMaterialIssue(string customer, string ChallanDate, string Status, int BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProjectSalesInvoiceOnMaterialIssue");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            proc.AddDateTimePara("@InvoiceDate", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@Branch_Id", BranchId);
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectInvoiceonProjectChallan(string customer, string ChallanDate, string Status, int BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProjectInvoiceOnProjectOrder");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            proc.AddDateTimePara("@InvoiceDate", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@Branch_Id", BranchId);
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }



        public DataTable GetProjectInvoiceonSalesChallan(string customer, string ChallanDate, string Status, int BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProjectInvoiceOnSalesOrder");

            proc.AddVarcharPara("@Customer_Id", 20, customer);
            proc.AddDateTimePara("@InvoiceDate", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@Branch_Id", BranchId);
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBRRequisition(string ChallanDate, string Status, string strBranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBranchIndentsOnStockOut");

            proc.AddDateTimePara("@date", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            //proc.AddIntegerPara("@BranchId", Convert.ToInt32(strBranchId));
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(strBranchId));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetStockOutDetails(string ChallanDate, string Status, string strBranchId)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetStockOutDetailsFromStockIn");
            proc.AddDateTimePara("@date", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 4000, strBranchId);
            //proc.AddIntegerPara("@BranchId", Convert.ToInt32(strBranchId));

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetIssueToServiceDetails(string ChallanDate, string Status)
        {
            HttpContext context = HttpContext.Current;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetIssueToServiceCentreDetails");
            proc.AddDateTimePara("@date", Convert.ToDateTime(ChallanDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@userbranchlist", 1000, Convert.ToString(context.Session["userbranchHierarchy"]));

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSchemaLengthSalesOrder()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[prc_SalesOrder_Details]");

            proc.AddVarcharPara("@Action", 100, "GetSchemaLengthSalesOrder");

            dt = proc.GetTable();
            return dt;
        }
    }
}
