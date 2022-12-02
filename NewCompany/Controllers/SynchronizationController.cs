using BusinessLogicLayer;
using NewCompany.DataAccessLayer;
using NewCompany.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace NewCompany.Controllers
{
    public class SynchronizationController : Controller
    {
        //
        // GET: /Synchronization/

        DBEngine odb = new DBEngine();
        MasterDbEngine modb = new MasterDbEngine();
        public ActionResult ProductSync()
        {

            DataTable dtProducts = odb.GetDataTable("select cast(sProducts_ID as INT) Id,sProducts_Name product_Name from Master_sProducts where sProduct_Status<>'D'");
            DataTable dtCompany = modb.GetDataTable("select DbName DbName,Name Company_Name from ERP_Company_List where IsActive=1 and Company_code<>'" + Convert.ToString(Session["LastCompany"]) + "'");




            ProductSyncClass Psync = new ProductSyncClass();
            Psync.Company_list = APIHelperMethods.ToModelList<Master_Company>(dtCompany);
            Psync.product_List = APIHelperMethods.ToModelList<Products>(dtProducts); //new List<Products>();




            return View(@"~\Views\NewCompany\Sync\Product\ProductSync.cshtml", Psync);
        }


        public ActionResult TaxSync()
        {

            DataTable dtCompany = modb.GetDataTable("select DbName DbName,Name Company_Name from ERP_Company_List where IsActive=1 and Company_code<>'" + Convert.ToString(Session["LastCompany"]) + "'");
            TaxSyncClass Psync = new TaxSyncClass();
            Psync.Company_list = APIHelperMethods.ToModelList<Master_Company>(dtCompany);
            return View(@"~\Views\NewCompany\Sync\Tax\TaxSync.cshtml", Psync);
        }



      

        public PartialViewResult ProductLookUp()
        {
            DataTable dtProducts = odb.GetDataTable("select cast(sProducts_ID as INT) Id,sProducts_Name product_Name from Master_sProducts where sProduct_Status<>'D'");
            List<Products> pro = new List<Products>();
            pro = APIHelperMethods.ToModelList<Products>(dtProducts);
            return PartialView(@"~\Views\NewCompany\Sync\Product\ProductLookUp.cshtml", pro);
        }

        public PartialViewResult CompanyLookUp()
        {
            DataTable dtCompany = modb.GetDataTable("select DbName DbName,Name Company_Name from ERP_Company_List where IsActive=1 and Company_code<>'" + Convert.ToString(Session["LastCompany"]) + "'");
            List<Master_Company> comp_list = new List<Master_Company>();
            comp_list = APIHelperMethods.ToModelList<Master_Company>(dtCompany);
            return PartialView(@"~\Views\NewCompany\Sync\Product\CompanyLookUp.cshtml", comp_list);
        }

        public JsonResult SyncProduct(SyncInput input)
        {

            string[] prod = input.Product.Split(',');
            string[] Comp = input.Company.Split(',');
            string Output = prod.Length.ToString() + " Number of product(s) Synced successfully.";





            try
            {
                foreach (string co in Comp)
                {
                    List<KeyObj> param = new List<KeyObj>();
                    ExecProcedure exep = new ExecProcedure();
                    param.Add(new KeyObj("@Comp", co));
                    exep.ProcedureName = "Sp_SyncClassAndBrannd";
                    exep.param = new List<KeyObj>();
                    exep.param = param;
                    exep.ExecuteProcedureNonQuery();
                }

            }
            catch
            {
                Output = "Eror! Please try after sometime.";
            }


            




            foreach (string pro in prod)
            {
                try
                
                {










                    DataTable proddt = odb.GetDataTable("select P.sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_Type,ProductClass_Code,sProducts_GlobalCode,sProducts_TradingLot,sProducts_TradingLotUnit,sProducts_QuoteCurrency,sProducts_QuoteLot,sProducts_QuoteLotUnit,sProducts_DeliveryLot,sProducts_DeliveryLotUnit,sProducts_Color,sProducts_Size,sProducts_CreateUser,sProducts_CreateTime,sProducts_ModifyUser,sProducts_ModifyTime,sProducts_SizeApplicable,sProducts_ColorApplicable,sProducts_barCodeType,sProducts_barCode,sProduct_IsInventory,sProduct_Stockvaluation,sProduct_SalePrice,sProduct_MinSalePrice,sProduct_PurPrice,sProduct_MRP,sProduct_StockUOM,sProduct_MinLvl,sProduct_reOrderLvl,sProduct_NegativeStock,sProduct_TaxCodeSale,sProduct_TaxCodePur,sProduct_TaxScheme,sProduct_AutoApply,sProduct_ImagePath,sProduct_Status,warehouse_id,Is_active_warehouse,Is_active_serialno,sProducts_HsnCode,Is_active_Batch,sProducts_serviceTax,Is_stock_Block,sProduct_TaxSchemeSale,sProduct_TaxSchemePur,sProducts_isInstall,sProducts_Brand,sProduct_IsCapitalGoods,sProducts_IsOldUnit,sInv_MainAccount,sRet_MainAccount,pInv_MainAccount,pRet_MainAccount,FurtheranceToBusiness,Is_ServiceItem,Is_BarCode_Active,Reorder_Quantity,EcomId,MaxLvl,Is_ComponentsMandatory,isOverideConvertion,sProduct_quantity,sProduct_SaleUom,packing_quantity,packing_saleUOM,isOverideConvertion,TDSTCS_ID from Master_sProducts P LEFT JOIN tbl_master_product_packingDetails On P.sProducts_ID=packing_sProductId LEFT JOIN tbl_master_productTdsMap TDS ON TDS.sProducts_ID=P.sProducts_ID where P.sProducts_ID='" + pro + "'");


                    if (proddt != null && proddt.Rows.Count > 0)
                    {
                        List<KeyObj> param = new List<KeyObj>();
                        ExecProcedure exep = new ExecProcedure();
                        param.Add(new KeyObj("@sProducts_ID", Convert.ToString(proddt.Rows[0]["sProducts_ID"])));
                        param.Add(new KeyObj("@ProductCode", Convert.ToString(proddt.Rows[0]["sProducts_Code"])));
                        param.Add(new KeyObj("@ProductName", Convert.ToString(proddt.Rows[0]["sProducts_Name"])));
                        param.Add(new KeyObj("@ProductDescription", Convert.ToString(proddt.Rows[0]["sProducts_Description"])));
                        param.Add(new KeyObj("@ProductClassCode", Convert.ToString(proddt.Rows[0]["ProductClass_Code"])));
                        param.Add(new KeyObj("@ProductGlobalCode", Convert.ToString(proddt.Rows[0]["sProducts_GlobalCode"])));
                        param.Add(new KeyObj("@ProductTradingLot", Convert.ToString(proddt.Rows[0]["sProducts_TradingLot"])));
                        param.Add(new KeyObj("@productTradingLotUnit", Convert.ToString(proddt.Rows[0]["sProducts_TradingLotUnit"])));
                        param.Add(new KeyObj("@ProductQuoteCurrency", Convert.ToString(proddt.Rows[0]["sProducts_QuoteCurrency"])));
                        param.Add(new KeyObj("@ProductQuoteLot", Convert.ToString(proddt.Rows[0]["sProducts_QuoteLot"])));
                        param.Add(new KeyObj("@productQuoteLotUnit", Convert.ToString(proddt.Rows[0]["sProducts_QuoteLotUnit"])));
                        param.Add(new KeyObj("@ProductDeliveryLot", Convert.ToString(proddt.Rows[0]["sProducts_DeliveryLot"])));
                        param.Add(new KeyObj("@ProductDeliveryLotUnit", Convert.ToString(proddt.Rows[0]["sProducts_DeliveryLotUnit"])));
                        param.Add(new KeyObj("@ProductColor", Convert.ToString(proddt.Rows[0]["sProducts_Color"])));
                        param.Add(new KeyObj("@ProductSize", Convert.ToString(proddt.Rows[0]["sProducts_Size"])));
                        param.Add(new KeyObj("@ProductCreateUser", Convert.ToString(proddt.Rows[0]["sProducts_CreateUser"])));
                        param.Add(new KeyObj("@sProduct_quantity", Convert.ToString(proddt.Rows[0]["sProduct_quantity"])));
                        param.Add(new KeyObj("@packing_quantity", Convert.ToString(proddt.Rows[0]["packing_quantity"])));
                        param.Add(new KeyObj("@packing_saleUOM", Convert.ToString(proddt.Rows[0]["packing_saleUOM"])));
                        param.Add(new KeyObj("@sProducts_tdsCode", Convert.ToString(proddt.Rows[0]["TDSTCS_ID"])));

                        param.Add(new KeyObj("@sProducts_SizeApplicable", Convert.ToString(proddt.Rows[0]["sProducts_SizeApplicable"])));
                        param.Add(new KeyObj("@sProducts_ColorApplicable", Convert.ToString(proddt.Rows[0]["sProducts_ColorApplicable"])));
                        param.Add(new KeyObj("@ProductBarCodeType", Convert.ToString(proddt.Rows[0]["sProducts_barCodeType"])));
                        param.Add(new KeyObj("@sProducts_barCode", Convert.ToString(proddt.Rows[0]["sProducts_barCode"])));
                        param.Add(new KeyObj("@sProductsIsInventory", Convert.ToString(proddt.Rows[0]["sProduct_IsInventory"])));
                        param.Add(new KeyObj("@sProduct_Stockvaluation", Convert.ToString(proddt.Rows[0]["sProduct_Stockvaluation"])));
                        param.Add(new KeyObj("@sProduct_SalePrice", Convert.ToString(proddt.Rows[0]["sProduct_SalePrice"])));
                        param.Add(new KeyObj("@sProduct_MinSalePrice", Convert.ToString(proddt.Rows[0]["sProduct_MinSalePrice"])));
                        param.Add(new KeyObj("@sProduct_PurPrice", Convert.ToString(proddt.Rows[0]["sProduct_PurPrice"])));
                        param.Add(new KeyObj("@sProduct_MRP", Convert.ToString(proddt.Rows[0]["sProduct_MRP"])));
                        param.Add(new KeyObj("@sProduct_StockUOM", Convert.ToString(proddt.Rows[0]["sProduct_StockUOM"])));
                        param.Add(new KeyObj("@sProduct_MinLvl", Convert.ToString(proddt.Rows[0]["sProduct_MinLvl"])));
                        param.Add(new KeyObj("@sProduct_reOrderLvl", Convert.ToString(proddt.Rows[0]["sProduct_reOrderLvl"])));
                        param.Add(new KeyObj("@sProduct_NegativeStock", Convert.ToString(proddt.Rows[0]["sProduct_NegativeStock"])));
                        param.Add(new KeyObj("@sProduct_TaxCodeSale", Convert.ToString(proddt.Rows[0]["sProduct_TaxCodeSale"])));
                        param.Add(new KeyObj("@sProduct_TaxCodePur", Convert.ToString(proddt.Rows[0]["sProduct_TaxCodePur"])));
                        param.Add(new KeyObj("@sProduct_TaxScheme", Convert.ToString(proddt.Rows[0]["sProduct_TaxScheme"])));
                        param.Add(new KeyObj("@sProduct_AutoApply", Convert.ToString(proddt.Rows[0]["sProduct_AutoApply"])));
                        param.Add(new KeyObj("@sProduct_ImagePath", Convert.ToString(proddt.Rows[0]["sProduct_ImagePath"])));
                        param.Add(new KeyObj("@sProduct_Status", Convert.ToString(proddt.Rows[0]["sProduct_Status"])));
                        param.Add(new KeyObj("@Is_active_warehouse", Convert.ToString(proddt.Rows[0]["Is_active_warehouse"])));
                        param.Add(new KeyObj("@Is_active_serialno", Convert.ToString(proddt.Rows[0]["Is_active_serialno"])));
                        param.Add(new KeyObj("@sProducts_HsnCode", Convert.ToString(proddt.Rows[0]["sProducts_HsnCode"])));
                        param.Add(new KeyObj("@Is_active_Batch", Convert.ToString(proddt.Rows[0]["Is_active_Batch"])));
                        param.Add(new KeyObj("@sProducts_serviceTax", Convert.ToString(proddt.Rows[0]["sProducts_serviceTax"])));
                        param.Add(new KeyObj("@Is_stock_Block", Convert.ToString(proddt.Rows[0]["Is_stock_Block"])));
                        param.Add(new KeyObj("@isOverideConvertion", Convert.ToString(proddt.Rows[0]["isOverideConvertion"])));
                        param.Add(new KeyObj("@sProduct_TaxSchemeSale", Convert.ToString(proddt.Rows[0]["sProduct_TaxSchemeSale"])));
                        param.Add(new KeyObj("@sProduct_TaxSchemePur", Convert.ToString(proddt.Rows[0]["sProduct_TaxSchemePur"])));
                        param.Add(new KeyObj("@sProducts_isInstall", Convert.ToString(proddt.Rows[0]["sProducts_isInstall"])));
                        param.Add(new KeyObj("@sProducts_Brand", Convert.ToString(proddt.Rows[0]["sProducts_Brand"])));
                        param.Add(new KeyObj("@sProducts_isCapitalGoods", Convert.ToString(proddt.Rows[0]["sProduct_IsCapitalGoods"])));
                        param.Add(new KeyObj("@sProducts_IsOldUnit", Convert.ToString(proddt.Rows[0]["sProducts_IsOldUnit"])));
                        param.Add(new KeyObj("@sInv_MainAccount", Convert.ToString("")));
                        param.Add(new KeyObj("@sRet_MainAccount", Convert.ToString("")));

                        param.Add(new KeyObj("@pInv_MainAccount", Convert.ToString("")));

                        param.Add(new KeyObj("@pRet_MainAccount", Convert.ToString("")));
                        param.Add(new KeyObj("@BusinessFurtherness", Convert.ToString(proddt.Rows[0]["FurtheranceToBusiness"])));
                        param.Add(new KeyObj("@sProducts_IsServiceItem", Convert.ToString(proddt.Rows[0]["Is_ServiceItem"])));

                        param.Add(new KeyObj("@Is_BarCode_Active", Convert.ToString(proddt.Rows[0]["Is_BarCode_Active"])));
                        param.Add(new KeyObj("@reorder_qty", Convert.ToString(proddt.Rows[0]["Reorder_Quantity"])));
                        param.Add(new KeyObj("@maxLvl", Convert.ToString(proddt.Rows[0]["MaxLvl"])));
                        param.Add(new KeyObj("@sProducts_isComponentsMandatory", Convert.ToString(proddt.Rows[0]["Is_ComponentsMandatory"])));
                        param.Add(new KeyObj("@Finyear", Convert.ToString(Session["LastFinYear"])));

                        foreach (string co in Comp)
                        {
                            exep.ProcedureName = "Sp_SyncProduct";
                            exep.param = new List<KeyObj>();
                            exep.param = param;
                            exep.ExecuteProcedureNonQuery(GetConnectionString(co));
                        }

                        param.Clear();

                    }


                }

                catch
                {
                    Output = "Eror! Please try after sometime.";
                }







            }

            try
            {

                DataTable proddt = odb.GetDataTable("select Product_id,Component_prodId from tbl_master_ProdComponent where Product_id in (" + input.Product + ")");


                if (proddt != null && proddt.Rows.Count > 0)
                {
                    List<KeyObj> param = new List<KeyObj>();
                    ExecProcedure exep = new ExecProcedure();
                    param.Add(new KeyObj("@componenttable", proddt));


                    foreach (string co in Comp)
                    {
                        exep.ProcedureName = "Sp_SyncProductComponent";
                        exep.param = new List<KeyObj>();
                        exep.param = param;
                        exep.ExecuteProcedureNonQuery(GetConnectionString(co));
                    }

                    param.Clear();

                }


            }

            catch
            {
                Output = "Eror! Please try after sometime.";
            }


            try
            {

                DataTable proddt = odb.GetDataTable("select ID,PRODUCT_ID,LENGTH,WIDTH,THICKNESS,SIZE_UOM,SIZE_APPLICABLE_ON,SERIES,FINISH,LEADTIME,COVERAGE,COVERAGE_UOM,VOLUME,VOLUME_UOM,WEIGHT,PRINT_NAME,WEIGHT_UOM,SUBCATEGORY from PRODUCT_ATTRIBUTES where Product_id in (" + input.Product + ")");


                if (proddt != null && proddt.Rows.Count > 0)
                {
                    List<KeyObj> param = new List<KeyObj>();
                    ExecProcedure exep = new ExecProcedure();
                    param.Add(new KeyObj("@ID", Convert.ToString(proddt.Rows[0]["ID"])));
                    param.Add(new KeyObj("@PRODUCT_ID", Convert.ToString(proddt.Rows[0]["PRODUCT_ID"])));
                    param.Add(new KeyObj("@LENGTH", Convert.ToString(proddt.Rows[0]["LENGTH"])));
                    param.Add(new KeyObj("@WIDTH", Convert.ToString(proddt.Rows[0]["WIDTH"])));
                    param.Add(new KeyObj("@THICKNESS", Convert.ToString(proddt.Rows[0]["THICKNESS"])));
                    param.Add(new KeyObj("@SIZE_UOM", Convert.ToString(proddt.Rows[0]["SIZE_UOM"])));
                    param.Add(new KeyObj("@SIZE_APPLICABLE_ON", Convert.ToString(proddt.Rows[0]["SIZE_APPLICABLE_ON"])));
                    param.Add(new KeyObj("@SERIES", Convert.ToString(proddt.Rows[0]["SERIES"])));
                    param.Add(new KeyObj("@FINISH", Convert.ToString(proddt.Rows[0]["FINISH"])));
                    param.Add(new KeyObj("@LEADTIME", Convert.ToString(proddt.Rows[0]["LEADTIME"])));
                    param.Add(new KeyObj("@COVERAGE", Convert.ToString(proddt.Rows[0]["COVERAGE"])));
                    param.Add(new KeyObj("@COVERAGE_UOM", Convert.ToString(proddt.Rows[0]["COVERAGE_UOM"])));
                    param.Add(new KeyObj("@VOLUME", Convert.ToString(proddt.Rows[0]["VOLUME"])));
                    param.Add(new KeyObj("@VOLUME_UOM", Convert.ToString(proddt.Rows[0]["VOLUME_UOM"])));
                    param.Add(new KeyObj("@PRINT_NAME", Convert.ToString(proddt.Rows[0]["PRINT_NAME"])));
                    param.Add(new KeyObj("@WEIGHT", Convert.ToString(proddt.Rows[0]["WEIGHT"])));
                    param.Add(new KeyObj("@WEIGHT_UOM", Convert.ToString(proddt.Rows[0]["WEIGHT_UOM"])));
                    param.Add(new KeyObj("@SUBCATEGORY", Convert.ToString(proddt.Rows[0]["SUBCATEGORY"])));

                    foreach (string co in Comp)
                    {
                        exep.ProcedureName = "Sp_SyncProductAttribute";
                        exep.param = new List<KeyObj>();
                        exep.param = param;
                        exep.ExecuteProcedureNonQuery(GetConnectionString(co));
                    }

                    param.Clear();

                }


            }

            catch
            {
                Output = "Eror! Please try after sometime.";
            }





            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        private string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }



        public JsonResult SyncTax(SyncInput input)
        {

            // string[] prod = input.Product.Split(',');
            string[] Comp = input.Company.Split(',');
            string Output = Comp.Length.ToString() + " Number of company Synced successfully.";

            try
            {
                DataTable Master_Taxesdt = odb.GetDataTable("select * from Master_Taxes");
                DataTable Config_TaxRatesdt = odb.GetDataTable("select * from Config_TaxRates");
                DataTable HSNTaxratedt = odb.GetDataTable("select * from tbl_trans_HSNTaxrate");

                foreach (string pro in Comp)
                {
                    try
                    {
                        if (Master_Taxesdt != null && Master_Taxesdt.Rows.Count > 0)
                        {
                            for (int i = 0; i < Master_Taxesdt.Rows.Count; i++)
                            {
                                foreach (string co in Comp)
                                {
                                    List<KeyObj> param = new List<KeyObj>();
                                    ExecProcedure exep = new ExecProcedure();
                                    param.Add(new KeyObj("@Taxes_ID", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_ID"])));
                                    param.Add(new KeyObj("@Taxes_Code", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_Code"])));
                                    param.Add(new KeyObj("@Taxes_Name", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_Name"])));
                                    param.Add(new KeyObj("@Taxes_Description", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_Description"])));
                                    param.Add(new KeyObj("@Taxes_ApplicableFor", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_ApplicableFor"])));
                                    param.Add(new KeyObj("@Taxes_ApplicableOn", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_ApplicableOn"])));
                                    param.Add(new KeyObj("@Taxes_OtherTax", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_OtherTax"])));

                                    param.Add(new KeyObj("@Taxes_CreateUser", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_CreateUser"])));
                                    //param.Add(new KeyObj("@Taxes_CreateTime", (Master_Taxesdt.Rows[i]["Taxes_CreateTime"]== null ? null :   Convert.ToDateTime(Master_Taxesdt.Rows[i]["Taxes_CreateTime"]))));
                                    param.Add(new KeyObj("@Taxes_ModifyUser", Convert.ToString(Master_Taxesdt.Rows[i]["Taxes_ModifyUser"])));

                                    //param.Add(new KeyObj("@Taxes_ModifyTime", (Master_Taxesdt.Rows[i]["Taxes_ModifyTime"] == null ? null : Convert.ToDateTime(Master_Taxesdt.Rows[i]["Taxes_ModifyTime"]))));
                                    param.Add(new KeyObj("@TaxTypeCode", Convert.ToString(Master_Taxesdt.Rows[i]["TaxTypeCode"])));
                                    param.Add(new KeyObj("@TaxItemlevel", Convert.ToString(Master_Taxesdt.Rows[i]["TaxItemlevel"])));

                                    param.Add(new KeyObj("@TaxCalculateMethods", Convert.ToString(Master_Taxesdt.Rows[i]["TaxCalculateMethods"])));
                                    param.Add(new KeyObj("@ShowInDesign", Convert.ToString(Master_Taxesdt.Rows[i]["ShowInDesign"])));

                                    param.Add(new KeyObj("@ACTION", "Master_Taxes"));

                                    param.Add(new KeyObj("@DBNAME", co));

                                    exep.ProcedureName = "usp_SyncMasterTax";
                                    exep.param = new List<KeyObj>();
                                    exep.param = param;
                                    exep.ExecuteProcedureNonQuery();
                                    param.Clear();
                                }
                            }
                        }

                        if (Config_TaxRatesdt != null && Config_TaxRatesdt.Rows.Count > 0)
                        {
                            for (int i = 0; i < Config_TaxRatesdt.Rows.Count; i++)
                            {
                                foreach (string co in Comp)
                                {
                                    List<KeyObj> param = new List<KeyObj>();
                                    ExecProcedure exep = new ExecProcedure();
                                    param.Add(new KeyObj("@TaxRates_ID", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_ID"])));
                                    param.Add(new KeyObj("@TaxRates_TaxCode", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_TaxCode"])));
                                    param.Add(new KeyObj("@TaxRates_ProductClass", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_ProductClass"])));
                                    param.Add(new KeyObj("@TaxRates_Country", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_Country"])));
                                    param.Add(new KeyObj("@TaxRates_State", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_State"])));
                                    param.Add(new KeyObj("@TaxRates_City", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_City"])));
                                    param.Add(new KeyObj("@TaxRates_DateFrom", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_DateFrom"])));

                                    param.Add(new KeyObj("@TaxRates_DateTo", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_DateTo"])));
                                    param.Add(new KeyObj("@TaxRates_RateOrSlab", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_RateOrSlab"])));
                                    param.Add(new KeyObj("@TaxRates_Rate", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_Rate"])));

                                    param.Add(new KeyObj("@TaxRates_MinAmount", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_MinAmount"])));
                                    param.Add(new KeyObj("@TaxRates_SlabCode", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SlabCode"])));
                                    param.Add(new KeyObj("@TaxRates_SurchargeApplicable", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SurchargeApplicable"])));

                                    param.Add(new KeyObj("@TaxRates_SurchargeCriteria", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SurchargeCriteria"])));
                                    param.Add(new KeyObj("@TaxRates_SurchargeAbove", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SurchargeAbove"])));


                                    param.Add(new KeyObj("@TaxRates_SurchargeOn", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SurchargeOn"])));
                                    param.Add(new KeyObj("@Taxes_SurchargeRate", Convert.ToString(Config_TaxRatesdt.Rows[i]["Taxes-SurchargeRate"])));
                                    param.Add(new KeyObj("@TaxRates_MainAccount", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_MainAccount"])));

                                    param.Add(new KeyObj("@TaxRates_SubAccount", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_SubAccount"])));
                                    param.Add(new KeyObj("@TaxRates_TaxSlab_Code", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_TaxSlab_Code"])));

                                    param.Add(new KeyObj("@TaxRates_CreateUser", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_CreateUser"])));
                                    param.Add(new KeyObj("@TaxRates_CreateTime", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_CreateTime"])));
                                    param.Add(new KeyObj("@TaxRates_ModifyUser", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_ModifyUser"])));

                                    param.Add(new KeyObj("@TaxRates_ModifyTime", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_ModifyTime"])));
                                    param.Add(new KeyObj("@TaxRatesSchemeName", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRatesSchemeName"])));

                                    param.Add(new KeyObj("@Exempted", Convert.ToString(Config_TaxRatesdt.Rows[i]["Exempted"])));
                                    param.Add(new KeyObj("@TaxRates_Sequence", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_Sequence"])));

                                    param.Add(new KeyObj("@TaxRates_GSTtype", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_GSTtype"])));
                                    param.Add(new KeyObj("@TaxRates_IsCess", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_IsCess"])));
                                    param.Add(new KeyObj("@TaxRates_RoundingOff", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_RoundingOff"])));

                                    param.Add(new KeyObj("@TaxRates_ReverseChargeMainAccount", Convert.ToString(Config_TaxRatesdt.Rows[i]["TaxRates_ReverseChargeMainAccount"])));

                                    param.Add(new KeyObj("@ACTION", "Config_TaxRates"));

                                    param.Add(new KeyObj("@DBNAME", co));

                                    exep.ProcedureName = "usp_SyncMasterTax";
                                    exep.param = new List<KeyObj>();
                                    exep.param = param;
                                    exep.ExecuteProcedureNonQuery();
                                    param.Clear();
                                }
                            }
                        }

                        if (HSNTaxratedt != null && HSNTaxratedt.Rows.Count > 0)
                        {
                            for (int i = 0; i < HSNTaxratedt.Rows.Count; i++)
                            {
                                foreach (string co in Comp)
                                {
                                    List<KeyObj> param = new List<KeyObj>();
                                    ExecProcedure exep = new ExecProcedure();
                                    param.Add(new KeyObj("@HSNTaxrate_id", Convert.ToString(HSNTaxratedt.Rows[i]["HSNTaxrate_id"])));
                                    param.Add(new KeyObj("@TaxRates_ID_HSNTaxrate", Convert.ToString(HSNTaxratedt.Rows[i]["TaxRates_ID"])));
                                    param.Add(new KeyObj("@TaxRates_TaxCode_HSNTaxrate", Convert.ToString(HSNTaxratedt.Rows[i]["TaxRates_TaxCode"])));
                                    param.Add(new KeyObj("@TaxRatesSchemeName_HSNTaxrate", Convert.ToString(HSNTaxratedt.Rows[i]["TaxRatesSchemeName"])));

                                    param.Add(new KeyObj("@HsnCode", Convert.ToString(HSNTaxratedt.Rows[i]["HsnCode"])));

                                    param.Add(new KeyObj("@ACTION", "tbl_trans_HSNTaxrate"));

                                    param.Add(new KeyObj("@DBNAME", co));

                                    exep.ProcedureName = "usp_SyncMasterTax";
                                    exep.param = new List<KeyObj>();
                                    exep.param = param;
                                    exep.ExecuteProcedureNonQuery();
                                    param.Clear();
                                }
                            }
                        }

                    }
                    catch
                    {
                        Output = "Eror! Please try after sometime.";
                    }

                }
            }
            catch
            {
                Output = "Eror! Please try after sometime.";
            }

            return Json(Output, JsonRequestBehavior.AllowGet);
        }





    }

}
