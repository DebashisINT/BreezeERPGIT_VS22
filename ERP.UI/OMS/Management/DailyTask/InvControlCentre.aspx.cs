using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using System.Data;
using System.Web.Services;
using DevExpress.Web;
//using DevExpress.Web.ASPxCallbackPanel;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
//using DevExpress.Web;

namespace ERP.OMS.Management.DailyTask
{
    public partial class Management_DailyTask_InvControlCentre : ERP.OMS.ViewState_class.VSPage
    {
        DailyTask_Inventory oDailyTask_Inventory = new DailyTask_Inventory();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        StockDetails stock = new StockDetails();
        DataTable dtActivelist = new DataTable();
        DBEngine oDBEngine = new DBEngine();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // ucpOrdPopCon.Visible = false;
            if (!IsPostBack)
            {
                PositionGrigBind();
                StockGridBing();
                TransactionGridBind();
                ConversionTableFetch();
            }

        }
        #region PrivateMethod

        protected void Hidden_Click(object sender, EventArgs e)
        {
            PositionGrigBind();
        }



        private void PositionGrigBind()
        {
            try
            {

                string qstr = @"SELECT A.[StockPosition_ID]                   ,A.[StockPosition_Company]
											                              ,pOrder.pOrder_RefNumber AS pOrderRefNo
                                                                          ,A.[StockPosition_Branch]
                                                                          ,A.[StockPosition_FinYear]
                                                                          ,A.[StockPosition_Type]
                                                                          ,C.cnt_firstName  as StockPosition_ContactID 
                                                                          ,A.[StockPosition_OrderNumber]
                                                                          ,A.[StockPosition_OrderDate]
                                                                          ,A.[StockPosition_ProductID]
                                                                          ,P.sProducts_Name + '(' + P.sProducts_Code +') / ' + ISNULL((SELECT Size_Name FROM Master_Size WHERE Size_ID = A.StockPosition_Size),'') + ' / ' +ISNULL((SELECT Color_Name FROM Master_Color WHERE Color_ID = A.StockPosition_Color),'') as [StockPosition_ProductName]     
                                                                          ,A.[StockPosition_Brand]
                                                                          ,A.[StockPosition_Size]
                                                                          ,A.[StockPosition_Color]
                                                                          ,A.[StockPosition_BestBeforeMonth]
                                                                          ,A.[StockPosition_BestBeforeYear]
																		  ,UOM.UOM_Name AS [StockPosition_QuantityUnitName]  
                                                                          ,A.[StockPosition_QuantityUnit]
                                                                          ,CASE WHEN (ROUND(CAST(A.[StockPosition_ToReceive] AS NUMERIC(18,2)),2) > 0) THEN ROUND(CAST(A.[StockPosition_ToReceive] AS NUMERIC(18,2)),2) ELSE NULL END AS [StockPosition_ToReceive]
                                                                          ,CASE WHEN (ROUND(CAST(A.[StockPosition_Received] AS NUMERIC(18,2)),2) > 0) THEN ROUND(CAST(A.[StockPosition_Received] AS NUMERIC(18,2)),2) ELSE NULL END AS [StockPosition_Received]
                                                                          ,CASE WHEN (ROUND(CAST(A.[StockPosition_ToDeliver] AS NUMERIC(18,2)),2) > 0) THEN ROUND(CAST(A.[StockPosition_ToDeliver] AS NUMERIC(18,2)),2) ELSE NULL END AS [StockPosition_ToDeliver]
                                                                          ,CASE WHEN (ROUND(CAST(A.[StockPosition_Delivered] AS NUMERIC(18,2)),2) > 0) THEN ROUND(CAST(A.[StockPosition_Delivered] AS NUMERIC(18,2)),2) ELSE NULL END AS [StockPosition_Delivered]  
																		  ,CASE WHEN (ROUND(CAST((ISNULL(A.[StockPosition_ToReceive],0) - ISNULL(A.[StockPosition_Received],0)) AS NUMERIC(18,2)),2) <> 0) THEN ROUND(CAST((ISNULL(A.[StockPosition_ToReceive],0) - ISNULL(A.[StockPosition_Received],0)) AS NUMERIC(18,2)),2) ELSE NULL END AS PndgIn
																		  ,CASE WHEN (ROUND(CAST((ISNULL(A.[StockPosition_ToDeliver],0) - ISNULL(A.[StockPosition_Delivered],0)) AS NUMERIC(18,2)),2) <> 0) THEN ROUND(CAST((ISNULL(A.[StockPosition_ToDeliver],0) - ISNULL(A.[StockPosition_Delivered],0)) AS NUMERIC(18,2)),2) ELSE NULL END AS PndgOut
                                                                          ,A.[StockPosition_ShortReceipt]
                                                                          ,A.[StockPosition_ShortDeliveries]
                                                                          ,A.[StockPosition_OrderDetailID]
                                                                          ,A.[StockPosition_ProductDescription]
                                                                          ,A.[StockPosition_UpdateUser]
                                                                          ,A.[StockPosition_UpdateTime] 
                                                                      FROM [dbo].[Trans_StockPosition] A   
																	  LEFT JOIN  tbl_master_contact C
																	  on A.StockPosition_ContactID=C.cnt_internalId
																	  LEFT JOIN Master_sProducts P
																	  on P.sProducts_ID = A.StockPosition_ProductID  
																	  LEFT JOIN  [Master_ProductClass] PC
																	  on PC.ProductClass_ID=P.sProducts_ID
																	  LEFT JOIN tbl_master_branch BR
																	  on BR.branch_id=A.StockPosition_Branch
																	  LEFT JOIN Master_UOM UOM 
																	  on UOM.UOM_ID = A.StockPosition_QuantityUnit
																	  LEFT JOIN Trans_pOrder pOrder
																	  ON pOrder.pOrder_Number = A.StockPosition_OrderNumber " + Convert.ToString(Session["qry"]); //+ '('+C.cnt_internalId+')'


                DataTable oDatatable = oGenericMethod.GetDataTable(qstr);
                grdPosition.DataSource = oDatatable;
                Session["_FilteredStockPositionTable"] = oDatatable;
                grdPosition.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StockGridBing()
        {
            try
            {
                grdStock.DataSource = null;
                grdStock.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TransactionGridBind()
        {
            grdTransaction.DataSource = null;
            grdTransaction.DataBind();
        }

        private void ConversionTableFetch()
        {
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            DataTable oDataTable = oGeneric.GetDataTable("select * from Config_Conversion");
            Session["_ConversionTableFetch"] = oDataTable;
        }




        #endregion

        #region WebService
        [WebMethod]
        public static Stock[] GetStockGridData(string id)
        {
            DailyTask_Inventory oDailyTask_Inventory = new DailyTask_Inventory();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            List<Stock> oStock = new List<Stock>();
            int StockPositionId = Convert.ToInt32(id);
            DataTable dt = oDailyTask_Inventory.GetStockTable(StockPositionId);
            HttpContext.Current.Session["StockDesc"] = dt;
            foreach (DataRow dtRow in dt.Rows)
            {
                Stock DataObj = new Stock();
                DataObj.Stock_ID = Convert.ToString(dtRow["Stock_ID"]);
                DataObj.productId = Convert.ToString(dtRow["productId"]);
                DataObj.Products_Name = Convert.ToString(dtRow["Products_Name"]);
                DataObj.Color_Name = Convert.ToString(dtRow["Color_Name"]);
                DataObj.Size_Name = Convert.ToString(dtRow["Size_Name"]);
                DataObj.Stock_In = Convert.ToString(dtRow["Stock_In"]);
                DataObj.Stock_Out = Convert.ToString(dtRow["Stock_Out"]);
                DataObj.Stock_In_Hand = Convert.ToString(dtRow["Stock_In_Hand"]);
                DataObj.Location_Id = Convert.ToString(dtRow["Location_Id"]);
                DataObj.Location_Name = Convert.ToString(dtRow["Location_Name"]);
                DataObj.BestBeforeYear = Convert.ToString(dtRow["StockPosition_BestBeforeYear"]);
                DataObj.BestBeforeMonth = Convert.ToString(dtRow["StockPosition_BestBeforeMonth"]);
                DataObj.BatchNo = Convert.ToString(dtRow["Stock_Batch"]);
                DataObj.ProductDescription = DataObj.Products_Name + "/" + DataObj.Color_Name + "/" + DataObj.Size_Name + "/(" + DataObj.BestBeforeMonth + "-" + DataObj.BestBeforeYear + ")/" + DataObj.BatchNo;
                DataObj.QuantityUnit = Convert.ToString(dtRow["QuantityUnit"]);
                oStock.Add(DataObj);
            }

            return oStock.ToArray();
        }
        [WebMethod]
        public static string GetTransactionDetailsData(string id, string mode)
        {
            string FillValue = string.Empty;
            GenericMethod oGeneric = new GenericMethod();
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            DataTable odt = new DataTable();
            if (mode == "edit")
            {
                odt = oStore_MasterBL.GetTransactionEditDetailsById(Convert.ToInt32(id));
            }
            else if (mode == "insert")
            {
                odt = oStore_MasterBL.GetTransactionInsertDetailsById(Convert.ToInt32(id));
            }
            if (odt.Rows.Count > 0)
            {

                FillValue = Convert.ToString(odt.Rows[0]["StockPosition_Type"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_OrderDate"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_OrderNumber"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_ContactName"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_ContactID"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_ProductName"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_ProductID"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_Brand"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_Size"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_Color"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_Quantity"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_QuantityUnitName"]) + "," +
                    Convert.ToString(odt.Rows[0]["StockPosition_QuantityUnit"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_CurrencyName"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_CurrencyID"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_Price"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_PriceLot"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_PriceLotUnitName"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_PriceLotUnitID"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_Desctiption"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrder_DeliveryAt"]) + "," +
                    Convert.ToString(odt.Rows[0]["PorderBranchName"]) + "," +
                    Convert.ToString(odt.Rows[0]["PorderBranchId"]) + "," +
                    Convert.ToString(odt.Rows[0]["PorderWareHouse"]) + "," +
                    Convert.ToString(odt.Rows[0]["PorderAddress"]) + "," +
                    Convert.ToString(odt.Rows[0]["PorderOtherAddress"]) + "," +
                    Convert.ToString(odt.Rows[0]["BestBeforeMonth"]) + "," +
                    Convert.ToString(odt.Rows[0]["BestBeforeYear"]) + "," +
                    Convert.ToString(odt.Rows[0]["pOrderDetail_QuantityUnitName"]) + "," +
                    Convert.ToString(odt.Rows[0]["Inventory_BatchNumber"]) + "," +
                    //Convert.ToString(odt.Rows[0]["Inventory_ID"]) + "," +
                     Convert.ToString(odt.Rows[0]["Inventory_Received_Date"]) + "," +
                     Convert.ToString(odt.Rows[0]["StockPosition_RefNo"]) + "," +
                    Convert.ToString(odt.Rows[0]["PieceNo"]);


            }
            return FillValue;
        }
        [WebMethod]
        public static string CheckUniqueInventiryPieceNo(string pcno)
        {
            string FillValue = "0";
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGeneric.GetDataTable(@"select * from trans_inventory where isnull(inventory_pieceno,inventory_id)='" + pcno + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                FillValue = "1";
            }

            return FillValue;
        }

        [WebMethod]
        public static Transactions[] GetTransactionGridData(string id)
        {
            DailyTask_Inventory oDailyTask_Inventory = new DailyTask_Inventory();
            GenericMethod oGenericMethod = new GenericMethod();
            List<Transactions> oTransactions = new List<Transactions>();
            int StockPositionId = Convert.ToInt32(id);
            DataTable dt = oDailyTask_Inventory.FetchInventoryTransaction(StockPositionId);
            foreach (DataRow dtRow in dt.Rows)
            {
                Transactions DataObj = new Transactions();
                DataObj.InventoryId = Convert.ToString(dtRow["Inventory_ID"]);
                DataObj.StockPosition_ID = Convert.ToString(dtRow["StockPosition_ID"]);
                DataObj.Inventory_Date = Convert.ToString(dtRow["Inventory_Date"]);
                DataObj.Inventory_Type = Convert.ToString(dtRow["Inventory_Type"]);
                DataObj.Inventory_ProductDetails = Convert.ToString(dtRow["Inventory_ProductDetails"]);
                DataObj.Inventory_BatchNumber = Convert.ToString(dtRow["Inventory_BatchNumber"]);
                DataObj.Inventory_QuantityIn = Convert.ToString(dtRow["Inventory_QuantityIn"]);
                DataObj.Inventory_QuantityOut = Convert.ToString(dtRow["Inventory_QuantityOut"]);
                DataObj.UOM_Name = Convert.ToString(dtRow["UOM_Name"]);
                DataObj.RecievedAt = Convert.ToString(dtRow["RecievedAt"]);
                DataObj.DeliveredFrom = Convert.ToString(dtRow["DeliveredFrom"]);
                DataObj.Inventory_PieceNo = Convert.ToString(dtRow["Inventory_PieceNo"]);
                oTransactions.Add(DataObj);
            }

            return oTransactions.ToArray();
        }

        [WebMethod]
        public static bool DeleteTransaction(string id)
        {

            DailyTask_Inventory oDailyTask_Inventory = new DailyTask_Inventory();
            string mode = "Delete";
            bool flag = false;
            int InventoryId = Convert.ToInt32(id);
            int NoofRowEffected = oDailyTask_Inventory.DeleteTransaction(mode, InventoryId);
            if (NoofRowEffected > 0)
            {
                flag = true;
            }
            return flag;
        }
        [WebMethod]
        public static Stock_UOM[] GetWarehouseStock(string WarehouseId, string ProductId, string ColorId, string SizeId)
        {


            DataTable oDataTable = (DataTable)HttpContext.Current.Session["StockDesc"];
            DataView dv = new DataView(oDataTable);
            dv.RowFilter = "Location_Id =" + Convert.ToInt32(WarehouseId);
            oDataTable = dv.ToTable();


            List<Stock_UOM> detailsList = new List<Stock_UOM>();
            foreach (DataRow item in oDataTable.Rows)
            {
                Stock_UOM details = new Stock_UOM();
                details.Stock_Id = Convert.ToString(item["Stock_ID"].ToString());
                details.Text = item["unit_StockInHand"].ToString();
                detailsList.Add(details);
            }

            return detailsList.ToArray();

        }

        [WebMethod]
        public static string SetConvertForm(string StockId, string OrderQuantityId, string OrderQuantityName)
        {
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            string SetConvertFormValue = string.Empty;
            DataTable oDataTable = oGeneric.GetDataTable(@"select (ISNULL(Stock_Open,0) + ISNULL(Stock_In,0) -ISNULL(Stock_Out,0)) as StockInHand
	                                                         ,Stock_QuantityUnit
	                                                         ,(select UOM_Name from Master_UOM where UOM_ID = Stock_QuantityUnit) QuantityUnitName
                                                       from Trans_Stock where Stock_ID = " + StockId);
            if (oDataTable.Rows.Count > 0)
            {
                foreach (DataRow item in oDataTable.Rows)
                {
                    SetConvertFormValue = Convert.ToString(item["StockInHand"]) + "," + Convert.ToString(item["Stock_QuantityUnit"]) + "," + Convert.ToString(item["QuantityUnitName"]) + "," + OrderQuantityId + "," + OrderQuantityName;
                }
            }
            return SetConvertFormValue;
        }
        [WebMethod]
        public static string ConvertOperation(string WarehouseQuantity, string WarehouseQuantityUnit, string ToConvertQuantityUnit)
        {
            GenericMethod oGeneric = new GenericMethod();
            decimal ToConvertValue = Convert.ToDecimal(WarehouseQuantity);
            int ConversionFromUnit = Convert.ToInt32(WarehouseQuantityUnit);
            int ConversionToUnit = Convert.ToInt32(ToConvertQuantityUnit);
            decimal Multipier = 0;
            string ConvertedValue = string.Empty;
            DataTable oDataTable = (DataTable)HttpContext.Current.Session["_ConversionTableFetch"];
            DataView DataViewer = new DataView(oDataTable);
            DataViewer.RowFilter = "Conversion_FromUOM =" + Convert.ToInt32(WarehouseQuantityUnit);
            DataViewer.RowFilter = "Conversion_ToUOM = " + Convert.ToInt32(ToConvertQuantityUnit);
            oDataTable = DataViewer.ToTable();
            if (oDataTable.Rows.Count > 0)
            {
                Multipier = Convert.ToDecimal(oDataTable.Rows[0]["Conversion_Multiplier"]);
                decimal RoundValue = Math.Round((ToConvertValue * (1 / Multipier)), 2);
                ConvertedValue = Convert.ToString(RoundValue);
            }
            else
            {
                DataViewer.RowFilter = "Conversion_FromUOM =" + Convert.ToInt32(ToConvertQuantityUnit);
                DataViewer.RowFilter = "Conversion_ToUOM = " + Convert.ToInt32(WarehouseQuantityUnit);
                oDataTable = DataViewer.ToTable();
                if (oDataTable.Rows.Count > 0)
                {
                    Multipier = Convert.ToDecimal(oDataTable.Rows[0]["Conversion_Multiplier"]);
                    decimal RoundValue = Math.Round((ToConvertValue * Multipier), 2);
                    ConvertedValue = Convert.ToString(RoundValue);
                }
            }
            return ConvertedValue;

        }

        [WebMethod]
        public static bool TransactionPopupCondition(string OrderpositopnId)
        {
            bool flag = true;
            DataTable oDataTable = (DataTable)HttpContext.Current.Session["_FilteredStockPositionTable"];
            DataView oDataView = new DataView(oDataTable);
            oDataView.RowFilter = "StockPosition_ID=" + Convert.ToString(OrderpositopnId);

            oDataTable = oDataView.ToTable();
            if (oDataTable.Rows.Count > 0)
            {
                decimal Torecieved = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(oDataTable.Rows[0]["StockPosition_ToReceive"])))
                {
                    Torecieved = Convert.ToDecimal(oDataTable.Rows[0]["StockPosition_ToReceive"]);
                }
                decimal Recieved = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(oDataTable.Rows[0]["StockPosition_Received"])))
                {
                    Recieved = Convert.ToDecimal(oDataTable.Rows[0]["StockPosition_Received"]);
                }
                if ((Torecieved > 0) && (Recieved >= Torecieved))
                {
                    flag = false;
                }
            }
            else
            {
                flag = true;
            }
            return flag;

        }

        #endregion
    }

    #region Class

    public class Stock_UOM
    {
        public string Stock_Id { get; set; }
        public string Text { get; set; }
    }
    public class Stock
    {
        public string Stock_ID { get; set; }
        public string productId { get; set; }
        public string Products_Name { get; set; }
        public string Color_Name { get; set; }
        public string Size_Name { get; set; }
        public string Stock_In { get; set; }
        public string Stock_Out { get; set; }
        public string Stock_In_Hand { get; set; }
        public string Location_Id { get; set; }
        public string QuantityUnit { get; set; }
        public string Location_Name { get; set; }
        public string ProductDescription { get; set; }
        public string BestBeforeYear { get; set; }
        public string BestBeforeMonth { get; set; }
        public string BatchNo { get; set; }

    }
    public class Transactions
    {
        public string StockPosition_ID { get; set; }
        public string InventoryId { get; set; }
        public string Inventory_Date { get; set; }
        public string Inventory_Type { get; set; }
        public string Inventory_ProductDetails { get; set; }
        public string Inventory_BatchNumber { get; set; }
        public string Inventory_QuantityIn { get; set; }
        public string Inventory_QuantityOut { get; set; }
        public string UOM_Name { get; set; }
        public string RecievedAt { get; set; }
        public string DeliveredFrom { get; set; }
        public string Inventory_PieceNo { get; set; }
    }

    #endregion
}