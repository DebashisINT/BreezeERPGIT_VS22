using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;
using System.Web;

namespace BusinessLogicLayer
{
    public class Store_MasterBL
    {
        #region Market
        public DataTable GetMarketList()
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
             
                using (proc = new ProcedureExecute("Sp_sMarketList"))
                {
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
          

             
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertsMarket(string MarketCode, int Country, int State, int City, string MarketName, string MarketDescription,
            string MarketAddress, string MarketEmail, string MarketPhoneNo, string MarketWebsite, string MarketContactPerson, int CreatedUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertsMarket"))
                {
                    proc.AddVarcharPara("@MarketCode", 100, MarketCode);
                    proc.AddIntegerPara("@Country", Country);
                    proc.AddIntegerPara("@State", State);
                    proc.AddIntegerPara("@City", City);
                    proc.AddVarcharPara("@MarketName", 100, MarketName);
                    proc.AddVarcharPara("@MarketDescription", 300, MarketDescription);
                    proc.AddVarcharPara("@MarketAddress", 100, MarketAddress);
                    proc.AddVarcharPara("@MarketEmail", 100, MarketEmail);
                    proc.AddVarcharPara("@MarketPhoneNo", 20, MarketPhoneNo);
                    proc.AddVarcharPara("@MarketWebsite", 100, MarketWebsite);
                    proc.AddVarcharPara("@MarketContactPerson", 300, MarketContactPerson);
                    proc.AddIntegerPara("@CreatedUser", CreatedUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int UpdatesMarket(int MarketId, string MarketCode, int Country, int State, int City, string MarketName, string MarketDescription,
            string MarketAddress, string MarketEmail, string MarketPhoneNo, string MarketWebsite, string MarketContactPerson, int ModifyUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("UpdatesMarket"))
                {
                    proc.AddIntegerPara("@MarketId", MarketId);
                    proc.AddVarcharPara("@MarketCode", 100, MarketCode);
                    proc.AddIntegerPara("@Country", Country);
                    proc.AddIntegerPara("@State", State);
                    proc.AddIntegerPara("@City", City);
                    proc.AddVarcharPara("@MarketName", 100, MarketName);
                    proc.AddVarcharPara("@MarketDescription", 500, MarketDescription);
                    proc.AddVarcharPara("@MarketAddress", 100, MarketAddress);
                    proc.AddVarcharPara("@MarketEmail", 100, MarketEmail);
                    proc.AddVarcharPara("@MarketPhoneNo", 20, MarketPhoneNo);
                    proc.AddVarcharPara("@MarketWebsite", 100, MarketWebsite);
                    proc.AddVarcharPara("@MarketContactPerson", 100, MarketContactPerson);
                    proc.AddIntegerPara("@ModifyUser", ModifyUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertsMarketLog(int MarketId)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertsMarketLog"))
                {
                    proc.AddIntegerPara("@MarketId", MarketId);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetMarketListById(int MarketId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_sMarketListById"))
                {
                    proc.AddIntegerPara("@MarketId", MarketId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        #endregion

        #region Product
        public DataTable GetsProductList()
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {

                using (proc = new ProcedureExecute("Sp_sProductList"))
                {
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
                

                //proc = new ProcedureExecute("PRC_ALLMASTERPAGELISTING");
                //proc.AddVarcharPara("@WHICHMODULE", 100, "PRODUCTS");
                //proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                //rtrnvalue = proc.GetTable();
                //return rtrnvalue;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertProduct( string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
            string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
            int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ProductCreateUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, String BarCode, Boolean isInventory, string stkValuation
            , decimal salePrice, decimal minSalePrice, decimal purPrice,
            //rev srijeeta
            decimal packageqty ,
            //end of rev srijeeta
            decimal MRP, int? stockUOM, decimal minLvl, decimal reOrderlvl,
            string negativeStock, int? taxCodeSaleScheme, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string ProdStatus, string hsnValue, int serviceTax,
            decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, Boolean isInstall, int Brand, Boolean isCapitalGoods, int TdsCode, string FinYear, Boolean isOldUnit,
            string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
            decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage, string covuom, string volume, string volumeuom, string weight, string printname, string subcat
            
)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertsProduct"))
                {
                    proc.AddVarcharPara("@ProductCode", 80, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 5000, ProductDescription);
                    proc.AddVarcharPara("@ProductType", 10, ProductType);
                    if (ProductClassCode != null)
                    {
                        proc.AddIntegerPara("@ProductClassCode", ProductClassCode, QueryParameterDirection.Input);
                    }
                    proc.AddVarcharPara("@ProductGlobalCode", 100, ProductGlobalCode);
                    if (ProductTradingLot != null)
                    {
                        proc.AddIntegerPara("@ProductTradingLot", ProductTradingLot, QueryParameterDirection.Input);
                    }
                    if (productTradingLotUnit != null)
                    {
                        proc.AddIntegerPara("@productTradingLotUnit", productTradingLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteCurrency != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteCurrency", ProductQuoteCurrency, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteLot != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteLot", ProductQuoteLot, QueryParameterDirection.Input);
                    }
                    if (productQuoteLotUnit != null)
                    {
                        proc.AddIntegerPara("@productQuoteLotUnit", productQuoteLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLot != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLot", ProductDeliveryLot, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLotUnit != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLotUnit", ProductDeliveryLotUnit, QueryParameterDirection.Input);
                    }
                    //if (ProductColor != null)
                    //{
                    //    proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    //}
                    //if (ProductSize != null)
                    //{
                    //    proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    //}
                    if (ProductColor != 0)
                    {
                        proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    }
                    if (ProductSize != 0)
                    {
                        proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    }
                    if (ProductCreateUser != null)
                    {
                        proc.AddIntegerPara("@ProductCreateUser", ProductCreateUser, QueryParameterDirection.Input);
                    }


                    //.................Code Added By Sam on 25102016..........................................................
                    proc.AddBooleanPara("@sProducts_SizeApplicable", sizeapplicable, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_ColorApplicable", colorapplicable, QueryParameterDirection.Input);
                    //.................Code Above Added By Sam on 25102016....................................................

                    //----------Code Added by Surojit 08-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isOverideConvertion", OverideConvertion, QueryParameterDirection.Input);

                    //----------Code Added by Surojit 11-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isComponentsMandatory", IsMandatory, QueryParameterDirection.Input);

                    //Code Added by Debjyoti 30-12-2016
                    proc.AddIntegerPara("@ProductBarCodeType", BarCodeSymbology, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProducts_barCode", 50, BarCode);
                    //End here 30-12-2016

                    //Code Added by Debjyoti 04-01-2017
                    proc.AddBooleanPara("@sProductsIsInventory", isInventory, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_Stockvaluation", 5, stkValuation);
                    proc.AddDecimalPara("@sProduct_SalePrice", 5, 18, salePrice);
                    proc.AddDecimalPara("@sProduct_MinSalePrice", 5, 18, minSalePrice);
                    proc.AddDecimalPara("@sProduct_PurPrice", 5, 18, purPrice);
                    //rev srijeeta
                    proc.AddDecimalPara("@sProduct_packageqty", 5, 18, packageqty);
                    //end of rev srijeeta

                    proc.AddDecimalPara("@sProduct_MRP", 5, 18, MRP);
                    if (stockUOM != null)
                    {
                        proc.AddIntegerPara("@sProduct_StockUOM", stockUOM, QueryParameterDirection.Input);
                    }
                    proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrderlvl);
                    proc.AddVarcharPara("@sProduct_NegativeStock", 5, negativeStock);
                    proc.AddIntegerPara("@sProduct_TaxSchemeSale", taxCodeSaleScheme, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxSchemePur", taxCodePur, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxScheme", taxScheme, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProduct_AutoApply", autoApply, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_ImagePath", 200, ImagePath);
                    proc.AddVarcharPara("@ProdComponent", 1000, ProdComponent);
                    proc.AddVarcharPara("@sProduct_Status", 5, ProdStatus);
                    //End here 04-01-2017
                    proc.AddVarcharPara("@sProducts_HsnCode", 10, hsnValue);

                    //02-02-2017
                    proc.AddIntegerPara("@sProducts_serviceTax", serviceTax, QueryParameterDirection.Input);

                    //For Packing Details
                    proc.AddDecimalPara("@sProduct_quantity", 5, 18, quantity);
                    proc.AddDecimalPara("@packing_quantity", 5, 18, packing);
                    proc.AddIntegerPara("@packing_saleUOM", packingUOM, QueryParameterDirection.Input);
                    //Packing work end here
                    proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                    proc.AddBooleanPara("@sProducts_isInstall", isInstall);
                    proc.AddBooleanPara("@BusinessFurtherness", FurtheranceToBusiness);//Subhabrata
                    proc.AddIntegerPara("@sProducts_Brand", Brand, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_isCapitalGoods", isCapitalGoods);
                    proc.AddIntegerPara("@sProducts_tdsCode", TdsCode);
                    proc.AddVarcharPara("@Finyear", 20, FinYear);
                    proc.AddBooleanPara("@sProducts_IsOldUnit", isOldUnit);
                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddBooleanPara("@sProducts_IsServiceItem", IsServiceItem);
                    proc.AddDecimalPara("@reorder_qty", 4, 18, reorder_qty);
                    //rev 24514
                    //proc.AddPara("@maxLvl", maxLvl);
                    proc.AddDecimalPara("@maxLvl", 2, 18, maxLvl);
                    //End of rev 24514


                    proc.AddPara("@lenght", lenght);
                    proc.AddPara("@width", width);
                    proc.AddPara("@Thickness", Thickness);
                    proc.AddPara("@size", size);
                    proc.AddPara("@SUOM", SUOM);
                    proc.AddPara("@series", series);

                    proc.AddPara("@Finish", Finish);
                    proc.AddPara("@LeadTime", LeadTime);
                    proc.AddPara("@Coverage", Coverage);
                    //proc.AddPara("@covuom", covuom);
                    proc.AddPara("@volume", volume);
                    //proc.AddPara("@volumeuom", volumeuom);
                    proc.AddPara("@weight", weight);
                    proc.AddPara("@printname", printname);
                    proc.AddPara("@subcat", subcat);

                    


                    proc.RunActionQuery();
                    int NoOfRowEffected = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        public int InsertProducts(string strdimension,string strpedestalno,string strcatno,string strwarranty, int? cisv, string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
           string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
           int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ProductCreateUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, String BarCode, Boolean isInventory, string stkValuation
           , decimal salePrice, decimal minSalePrice, decimal purPrice, 
            //rev srijeeta
            decimal packageqty ,
            //end of rev srijeeta
            decimal MRP, int? stockUOM, decimal minLvl, decimal reOrderlvl,
           string negativeStock, int? taxCodeSaleScheme, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string ProdStatus, string hsnValue, int serviceTax,
           decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, Boolean isInstall, int Brand, Boolean isCapitalGoods, int TdsCode, string FinYear, Boolean isOldUnit,
           string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
           decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage, string covuom, string volume,
            string volumeuom, string weight, string printname, string subcat, int ComponentService, String ModelList, string _DesignNo, string _RevisionNo, string ItemType, Boolean Replaceable, int numberingId = 0, int Application = 0, int Nature = 0, int ApplicationArea = 0, string Movement = null, decimal CostPrice = 0,string VendorIDS=null
            
            )
        {
            ProcedureExecute proc;
          
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertsProduct"))
                {
                    proc.AddVarcharPara("@ProductCode", 80, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 5000, ProductDescription);
                    proc.AddVarcharPara("@ProductType", 10, ProductType);
                    if (ProductClassCode != null)
                    {
                        proc.AddIntegerPara("@ProductClassCode", ProductClassCode, QueryParameterDirection.Input);
                    }
                    proc.AddVarcharPara("@ProductGlobalCode", 100, ProductGlobalCode);
                    if (ProductTradingLot != null)
                    {
                        proc.AddIntegerPara("@ProductTradingLot", ProductTradingLot, QueryParameterDirection.Input);
                    }
                    if (productTradingLotUnit != null)
                    {
                        proc.AddIntegerPara("@productTradingLotUnit", productTradingLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteCurrency != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteCurrency", ProductQuoteCurrency, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteLot != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteLot", ProductQuoteLot, QueryParameterDirection.Input);
                    }
                    if (productQuoteLotUnit != null)
                    {
                        proc.AddIntegerPara("@productQuoteLotUnit", productQuoteLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLot != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLot", ProductDeliveryLot, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLotUnit != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLotUnit", ProductDeliveryLotUnit, QueryParameterDirection.Input);
                    }
                    //if (ProductColor != null)
                    //{
                    //    proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    //}
                    //if (ProductSize != null)
                    //{
                    //    proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    //}
                    if (ProductColor != 0)
                    {
                        proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    }
                    if (ProductSize != 0)
                    {
                        proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    }
                    if (ProductCreateUser != null)
                    {
                        proc.AddIntegerPara("@ProductCreateUser", ProductCreateUser, QueryParameterDirection.Input);
                    }
                    //Rev Rajdip
                    if (cisv != null)
                    {
                        proc.AddIntegerPara("@cisv", cisv, QueryParameterDirection.Input);
                    }
                    //End Rev Rajdip
                    //.................Code Added By Sam on 25102016..........................................................
                    proc.AddBooleanPara("@sProducts_SizeApplicable", sizeapplicable, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_ColorApplicable", colorapplicable, QueryParameterDirection.Input);
                    //.................Code Above Added By Sam on 25102016....................................................

                    //----------Code Added by Surojit 08-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isOverideConvertion", OverideConvertion, QueryParameterDirection.Input);

                    //----------Code Added by Surojit 11-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isComponentsMandatory", IsMandatory, QueryParameterDirection.Input);

                    //Code Added by Debjyoti 30-12-2016
                    proc.AddIntegerPara("@ProductBarCodeType", BarCodeSymbology, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProducts_barCode", 50, BarCode);
                    //End here 30-12-2016

                    //Code Added by Debjyoti 04-01-2017
                    proc.AddBooleanPara("@sProductsIsInventory", isInventory, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_Stockvaluation", 5, stkValuation);
                    proc.AddDecimalPara("@sProduct_SalePrice", 5, 18, salePrice);
                    proc.AddDecimalPara("@sProduct_MinSalePrice", 5, 18, minSalePrice);
                    proc.AddDecimalPara("@sProduct_PurPrice", 5, 18, purPrice);
                    //rev srijeeta
                    proc.AddDecimalPara("@sProduct_packageqty", 5, 18, packageqty);
                    //end of rev srijeeta
                    proc.AddDecimalPara("@sProduct_MRP", 5, 18, MRP);
                    if (stockUOM != null)
                    {
                        proc.AddIntegerPara("@sProduct_StockUOM", stockUOM, QueryParameterDirection.Input);
                    }
                    //rev 24514
                    //proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_MinLvl", 2, 18, minLvl);
                    //End of rev 24514
                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrderlvl);
                    proc.AddVarcharPara("@sProduct_NegativeStock", 5, negativeStock);
                    proc.AddIntegerPara("@sProduct_TaxSchemeSale", taxCodeSaleScheme, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxSchemePur", taxCodePur, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxScheme", taxScheme, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProduct_AutoApply", autoApply, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_ImagePath", 200, ImagePath);
                    proc.AddVarcharPara("@ProdComponent", 1000, ProdComponent);
                    proc.AddVarcharPara("@sProduct_Status", 5, ProdStatus);
                    //End here 04-01-2017
                    proc.AddVarcharPara("@sProducts_HsnCode", 10, hsnValue);

                    //02-02-2017
                    proc.AddIntegerPara("@sProducts_serviceTax", serviceTax, QueryParameterDirection.Input);

                    //For Packing Details
                    proc.AddDecimalPara("@sProduct_quantity", 5, 18, quantity);
                    proc.AddDecimalPara("@packing_quantity", 5, 18, packing);
                    proc.AddIntegerPara("@packing_saleUOM", packingUOM, QueryParameterDirection.Input);
                    //Packing work end here
                    proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                    proc.AddBooleanPara("@sProducts_isInstall", isInstall);
                    proc.AddBooleanPara("@BusinessFurtherness", FurtheranceToBusiness);//Subhabrata
                    proc.AddIntegerPara("@sProducts_Brand", Brand, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_isCapitalGoods", isCapitalGoods);
                    proc.AddIntegerPara("@sProducts_tdsCode", TdsCode);
                    proc.AddVarcharPara("@Finyear", 20, FinYear);
                    proc.AddBooleanPara("@sProducts_IsOldUnit", isOldUnit);
                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddBooleanPara("@sProducts_IsServiceItem", IsServiceItem);
                    proc.AddDecimalPara("@reorder_qty", 4, 18, reorder_qty);
                    
                    proc.AddPara("@maxLvl", maxLvl);
                    proc.AddPara("@lenght", lenght);
                    proc.AddPara("@width", width);
                    proc.AddPara("@Thickness", Thickness);
                    proc.AddPara("@size", size);
                    proc.AddPara("@SUOM", SUOM);
                    proc.AddPara("@series", series);

                    proc.AddPara("@Finish", Finish);
                    proc.AddPara("@LeadTime", LeadTime);
                    proc.AddPara("@Coverage", Coverage);
                    //proc.AddPara("@covuom", covuom);
                    proc.AddPara("@volume", volume);
                    //proc.AddPara("@volumeuom", volumeuom);
                    proc.AddPara("@weight", weight);
                    proc.AddPara("@printname", printname);
                    proc.AddPara("@subcat", subcat);

                    proc.AddPara("@ProductDimension", strdimension);
                    proc.AddPara("@ProductPedestalNo", strpedestalno);
                    proc.AddPara("@ProductCatNo", strcatno);
                    proc.AddPara("@ProductWarranty", strwarranty);

                    proc.AddIntegerPara("@NumberSchemaId", Convert.ToInt32(numberingId));
                    proc.AddIntegerPara("@Application", Convert.ToInt32(Application));
                    proc.AddIntegerPara("@Nature", Convert.ToInt32(Nature));

                    //Rev Tanmoy 24-04-2020
                    proc.AddIntegerPara("@isComponentService", ComponentService);
                    proc.AddPara("@ModelList", ModelList);
                    //Rev Tanmoy 24-04-2020
                    proc.AddPara("@DesignNo", _DesignNo);
                    proc.AddPara("@RevisionNo", _RevisionNo);
                    proc.AddBooleanPara("@sProducts_Replaceable", Replaceable);
                    proc.AddPara("@ItemType", ItemType);
                    proc.AddIntegerPara("@Application_Area", Convert.ToInt32(ApplicationArea));
                    proc.AddPara("@Movement", Movement);
                    proc.AddDecimalPara("@sProduct_Cost", 0, 18, CostPrice);
                    //Rev Bapi 
                    proc.AddPara("@VENDORIDS", VendorIDS);
                    //End of Rev

                    proc.RunActionQuery();
                    int NoOfRowEffected = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));                 
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        //
        //Rev Rajdip
        //Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        public DataSet InsertProductDataFromExcel(string ProductCode, string ProductName, string AddlDesc, string AltName, int Inventory, 
            //string NonInvt,
             int Service, string PurchaseUOM, string StockUOM, string SaleUOM, string SaleRate, string BuyRate, string ConvMainQty, string AltQty,
             string AltUOM, string Brand, string Class, string HSN, Boolean FurtherBuis, string ValTech, string MinLvl, string MaxLvl, string RecordLvl, string ReorderQty,
            string SaleLedger, string SaleRetLedger, string PurLedger, string PurRetLedger, string ColorCode,string  SizeCode,string  ProductSeries,string Surface,
            string LeadTime, string Weight, string SubCategory, string Length, string Width, string Thickness, string UOM, string UserId, string Year, int NumberSchemaId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTIMPORTFROMEXCEL");
            proc.AddVarcharPara("@Action", 100, "InsertproductDataFromExcel");
            proc.AddVarcharPara("@PRODUCTCODE", 100, ProductCode);
            proc.AddVarcharPara("@PRODUCTNAME", 200, ProductName);
            proc.AddVarcharPara("@PRODUCTDESCRIPTION", 100, AddlDesc);
            proc.AddVarcharPara("@PRINTNAME", 100, AltName);
            proc.AddIntegerPara("@SPRODUCTSISINVENTORY", Convert.ToInt32(Inventory));
            proc.AddIntegerPara("@SPRODUCTS_ISSERVICEITEM", Convert.ToInt32(Service));
            /*proc.AddBooleanPara("@SPRODUCTSISINVENTORY",Inventory, QueryParameterDirection.Input);
            proc.AddBooleanPara("@SPRODUCTS_ISSERVICEITEM", Service, QueryParameterDirection.Input);*/

            if (PurchaseUOM=="")
            {
                proc.AddIntegerPara("@PRODUCTDELIVERYLOTUNIT", 0);
            }
            else
            {
                proc.AddIntegerPara("@PRODUCTDELIVERYLOTUNIT", Convert.ToInt32(PurchaseUOM));
            }            
            if (StockUOM == "")
            {
                proc.AddIntegerPara("@SPRODUCT_STOCKUOM",0);
            }
            else
            {
                proc.AddIntegerPara("@SPRODUCT_STOCKUOM", Convert.ToInt32(StockUOM));
            }
            if (SaleUOM == "")
            {
                proc.AddIntegerPara("@PRODUCTTRADINGLOTUNIT", 0);
            }
            else
            {
                proc.AddIntegerPara("@PRODUCTTRADINGLOTUNIT", Convert.ToInt32(SaleUOM));
            }
            if (SaleRate == "")
            {
                proc.AddDecimalPara("@SPRODUCT_SALEPRICE", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SPRODUCT_SALEPRICE", 4, 19, Convert.ToDecimal(SaleRate));
            }
            if (BuyRate == "")
            {
                proc.AddDecimalPara("@SPRODUCT_PURPRICE", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SPRODUCT_PURPRICE", 4, 19, Convert.ToDecimal(BuyRate));
            }
            if (ConvMainQty=="")
            {
                proc.AddDecimalPara("@SPRODUCT_QUANTITY", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SPRODUCT_QUANTITY", 4, 19, Convert.ToDecimal(ConvMainQty));
            }            
            if (AltQty=="")
            {
                proc.AddDecimalPara("@PACKING_QUANTITY", 4, 19, 0);
            }
            else
            { 
                proc.AddDecimalPara("@PACKING_QUANTITY", 4, 19, Convert.ToDecimal(AltQty)); 
            }            
            if (AltUOM=="")
            {
                proc.AddDecimalPara("@PACKING_SALEUOM", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@PACKING_SALEUOM", 4, 19, Convert.ToDecimal(AltUOM));
            }
            if (Brand=="")
            {
                proc.AddIntegerPara("@SPRODUCTS_BRAND", 0);
            }
            else
            {
                proc.AddIntegerPara("@SPRODUCTS_BRAND", Convert.ToInt32(Brand));
            }            
            proc.AddVarcharPara("@PRODUCTCLASSCODE", 50,Class);
            proc.AddVarcharPara("@SPRODUCTS_HSNCODE", 10, HSN);            
            //proc.AddIntegerPara("@BUSINESSFURTHERNESS",Convert.ToInt32(FurtherBuis));
            proc.AddBooleanPara("@BUSINESSFURTHERNESS", FurtherBuis, QueryParameterDirection.Input);
            proc.AddVarcharPara("@SPRODUCT_STOCKVALUATION",10, ValTech);

            if (MinLvl=="")
            {
                proc.AddDecimalPara("@SPRODUCT_MINLVL", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SPRODUCT_MINLVL", 4, 19, Convert.ToDecimal(MinLvl));
            }
            if (MaxLvl == "")
            {
                proc.AddDecimalPara("@MAXLVL", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@MAXLVL", 4, 19, Convert.ToDecimal(MaxLvl));
            }
            if (RecordLvl == "")
            {
                proc.AddDecimalPara("@SPRODUCT_REORDERLVL", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SPRODUCT_REORDERLVL", 4, 19, Convert.ToDecimal(RecordLvl));
            }
            if (ReorderQty == "")
            {
                proc.AddDecimalPara("@REORDER_QTY", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@REORDER_QTY", 4, 19, Convert.ToDecimal(ReorderQty));
            }
            proc.AddVarcharPara("@SINV_MAINACCOUNT", 50, SaleLedger);
            proc.AddVarcharPara("@SRET_MAINACCOUNT", 50, SaleRetLedger);
            proc.AddVarcharPara("@PINV_MAINACCOUNT", 50, PurLedger);
            proc.AddVarcharPara("@PRET_MAINACCOUNT", 50, PurRetLedger);
            //Rev work start 08.08.2022 Mantise issue:0024783: Customer & Product master import required for ERP
           // proc.AddIntegerPara("@PRODUCTCOLOR", Convert.ToInt32(ColorCode));
            //proc.AddIntegerPara("@PRODUCTSIZE", Convert.ToInt32(SizeCode));
            if (ColorCode=="")
            {
                proc.AddIntegerPara("@PRODUCTCOLOR", 0);
            }
            else
            {
                proc.AddIntegerPara("@PRODUCTCOLOR", Convert.ToInt32(ColorCode));
            }
            if (SizeCode == "")
            {
                proc.AddIntegerPara("@PRODUCTSIZE",0);
            }
            else
            {
                proc.AddIntegerPara("@PRODUCTSIZE", Convert.ToInt32(SizeCode));
            }
            //Rev work close 08.08.2022 Mantise issue:0024783: Customer & Product master import required for ERP
            if (Length=="")
            {
                proc.AddDecimalPara("@LENGHT", 4, 19,0);
            }
            else
            {
                proc.AddDecimalPara("@LENGHT", 4, 19, Convert.ToDecimal(Length));
            }
            if (Width == "")
            {
                proc.AddDecimalPara("@WIDTH", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@WIDTH", 4, 19, Convert.ToDecimal(Width));
            }
            
            if (Thickness=="")
            {
                proc.AddDecimalPara("@THICKNESS", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@THICKNESS", 4, 19, Convert.ToDecimal(Thickness));
            }
            if (UOM == "")
            {
                proc.AddDecimalPara("@SIZE", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SIZE", 4, 19, Convert.ToDecimal(UOM));
            }
            if (UOM == "")
            {
                proc.AddDecimalPara("@SUOM", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@SUOM", 4, 19, 1);
            }            

            proc.AddVarcharPara("@SERIES", 50, ProductSeries);
            proc.AddVarcharPara("@FINISH", 50, Surface);
            if (LeadTime == "")
            {
                proc.AddDecimalPara("@LEADTIME", 4, 19, 0);
            }
            else
            {
                proc.AddIntegerPara("@LEADTIME", Convert.ToInt32(LeadTime));
            }
            if (Weight == "")
            {
                proc.AddDecimalPara("@WEIGHT", 4, 19, 0);
            }
            else
            {
                proc.AddDecimalPara("@WEIGHT", 4, 19, Convert.ToDecimal(Weight));
            }
            
            proc.AddVarcharPara("@SUBCAT", 50, SubCategory);

            proc.AddVarcharPara("@PRODUCTCREATEUSER", 50, UserId);
            proc.AddVarcharPara("@FINYEAR", 50, Year);
            proc.AddIntegerPara("@NumberSchemaId", NumberSchemaId);
          
            ds = proc.GetDataSet();
            return ds;
        }
        public int InsertProductImportLOg(string prodcode, int loopnumber, string prodname, string userid, string filename, string description, string status)
        {

            int i;
            //int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTMASERLOG");
            proc.AddVarcharPara("@action", 150, "insertlog");
            proc.AddVarcharPara("@ProductCode", 50, prodcode);
            proc.AddIntegerPara("@LoopNumber", loopnumber);
            proc.AddVarcharPara("@ProductName", 150, prodname);
            proc.AddVarcharPara("@UserId", 150, userid);
            proc.AddVarcharPara("@FileName", 150, filename);
            proc.AddVarcharPara("@decription", 150, description);
            proc.AddVarcharPara("@status", 150, status);
            i = proc.RunActionQuery();

            return i;
        }
        public DataSet GetproductLog(string Filename)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTMASERLOG");
            proc.AddVarcharPara("@action", 150, "getProductLog");
            proc.AddVarcharPara("@FileName", 150, Filename);
            ds = proc.GetDataSet();
            return ds;
        }
        //Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        public int InsertProductAfterCopy(string strdimension,string strpedestalno,string strcatno,string strwarranty,int? cisv, int ProductId, string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
           string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
           int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ProductCreateUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, String BarCode, Boolean isInventory, string stkValuation
           , decimal salePrice, decimal minSalePrice, decimal purPrice,
            //rev srijeeta
            decimal packageqty ,
            //end of rev srijeeta
            decimal MRP, int? stockUOM, decimal minLvl, decimal reOrderlvl,
           string negativeStock, int? taxCodeSaleScheme, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string ProdStatus, string hsnValue, int serviceTax,
           decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, Boolean isInstall, int Brand, Boolean isCapitalGoods, int TdsCode, string FinYear, Boolean isOldUnit,
           string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
        decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage, string covuom, string volume
            , string volumeuom, string weight, string printname, string subcat, int ComponentService, String ModelList, string _DesignNo, string _RevisionNo,Boolean Replaceable, int numberingId = 0
            , int Application = 0, int Nature = 0, int ApplicationArea = 0, string Movement = null, decimal CostPrice = 0
               
            )

        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertsProductAfterCopy"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);
                    proc.AddVarcharPara("@ProductCode", 80, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 5000, ProductDescription);
                    proc.AddVarcharPara("@ProductType", 10, ProductType);
                    if (ProductClassCode != null)
                    {
                        //rev 24514
                        proc.AddIntegerPara("@ProductClassCode", ProductClassCode, QueryParameterDirection.Input);
                        //proc.AddVarcharPara("@ProductClassCode", 100, ProductClassCode);
                        //rev 24514
                    }
                    proc.AddVarcharPara("@ProductGlobalCode", 100, ProductGlobalCode);
                    if (ProductTradingLot != null)
                    {
                        proc.AddIntegerPara("@ProductTradingLot", ProductTradingLot, QueryParameterDirection.Input);
                    }
                    if (productTradingLotUnit != null)
                    {
                        proc.AddIntegerPara("@productTradingLotUnit", productTradingLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteCurrency != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteCurrency", ProductQuoteCurrency, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteLot != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteLot", ProductQuoteLot, QueryParameterDirection.Input);
                    }
                    if (productQuoteLotUnit != null)
                    {
                        proc.AddIntegerPara("@productQuoteLotUnit", productQuoteLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLot != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLot", ProductDeliveryLot, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLotUnit != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLotUnit", ProductDeliveryLotUnit, QueryParameterDirection.Input);
                    }
                    //if (ProductColor != null)
                    //{
                    //    proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    //}
                    //if (ProductSize != null)
                    //{
                    //    proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    //}
                    if (ProductColor != 0)
                    {
                        proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    }
                    if (ProductSize != 0)
                    {
                        proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    }
                    if (ProductCreateUser != null)
                    {
                        proc.AddIntegerPara("@ProductCreateUser", ProductCreateUser, QueryParameterDirection.Input);
                    }
                    //Rev Rajdip
                    if (ProductCreateUser != null)
                    {
                        proc.AddIntegerPara("@cisv", cisv, QueryParameterDirection.Input);
                    }
                    //End Rev Rajdip
                    //.................Code Added By Sam on 25102016..........................................................
                    proc.AddBooleanPara("@sProducts_SizeApplicable", sizeapplicable, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_ColorApplicable", colorapplicable, QueryParameterDirection.Input);
                    //.................Code Above Added By Sam on 25102016....................................................

                    //----------Code Added by Surojit 08-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isOverideConvertion", OverideConvertion, QueryParameterDirection.Input);

                    //----------Code Added by Surojit 11-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isComponentsMandatory", IsMandatory, QueryParameterDirection.Input);

                    //Code Added by Debjyoti 30-12-2016
                    proc.AddIntegerPara("@ProductBarCodeType", BarCodeSymbology, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProducts_barCode", 50, BarCode);
                    //End here 30-12-2016

                    //Code Added by Debjyoti 04-01-2017
                    proc.AddBooleanPara("@sProductsIsInventory", isInventory, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_Stockvaluation", 5, stkValuation);
                    proc.AddDecimalPara("@sProduct_SalePrice", 5, 18, salePrice);
                    proc.AddDecimalPara("@sProduct_MinSalePrice", 5, 18, minSalePrice);
                    proc.AddDecimalPara("@sProduct_PurPrice", 5, 18, purPrice);
                    //rev srijeeta
                    proc.AddDecimalPara("@sProduct_packageqty", 5, 18,packageqty);
                    //end of rev srijeeta
                    proc.AddDecimalPara("@sProduct_MRP", 5, 18, MRP);
                    if (stockUOM != null)
                    {
                        proc.AddIntegerPara("@sProduct_StockUOM", stockUOM, QueryParameterDirection.Input);
                    }
                    //rev 24514
                    //proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_MinLvl", 2, 18, minLvl);
                    //End of rev 24514

                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrderlvl);
                    proc.AddVarcharPara("@sProduct_NegativeStock", 5, negativeStock);
                    proc.AddIntegerPara("@sProduct_TaxSchemeSale", taxCodeSaleScheme, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxSchemePur", taxCodePur, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxScheme", taxScheme, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProduct_AutoApply", autoApply, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_ImagePath", 200, ImagePath);
                    proc.AddVarcharPara("@ProdComponent", 1000, ProdComponent);
                    proc.AddVarcharPara("@sProduct_Status", 5, ProdStatus);
                    //End here 04-01-2017
                    proc.AddVarcharPara("@sProducts_HsnCode", 10, hsnValue);

                    //02-02-2017
                    proc.AddIntegerPara("@sProducts_serviceTax", serviceTax, QueryParameterDirection.Input);

                    //For Packing Details
                    proc.AddDecimalPara("@sProduct_quantity", 5, 18, quantity);
                    proc.AddDecimalPara("@packing_quantity", 5, 18, packing);
                    proc.AddIntegerPara("@packing_saleUOM", packingUOM, QueryParameterDirection.Input);
                    //Packing work end here
                    proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                    proc.AddBooleanPara("@sProducts_isInstall", isInstall);
                    proc.AddBooleanPara("@BusinessFurtherness", FurtheranceToBusiness);//Subhabrata
                    proc.AddIntegerPara("@sProducts_Brand", Brand, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_isCapitalGoods", isCapitalGoods);
                    proc.AddIntegerPara("@sProducts_tdsCode", TdsCode);
                    proc.AddVarcharPara("@Finyear", 20, FinYear);
                    proc.AddBooleanPara("@sProducts_IsOldUnit", isOldUnit);
                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddBooleanPara("@sProducts_IsServiceItem", IsServiceItem);
                    proc.AddDecimalPara("@reorder_qty", 4, 18, reorder_qty);
                    //rev 24514
                    //proc.AddPara("@maxLvl", maxLvl);
                    proc.AddDecimalPara("@maxLvl", 2, 18, maxLvl);
                    //End of rev 24514
                    //rev 24514
                    //proc.AddPara("@lenght", lenght);
                    proc.AddPara("@lenght", Convert.ToDecimal(lenght));
                    //proc.AddPara("@width", width);
                    proc.AddPara("@width", Convert.ToDecimal(width));
                    //proc.AddPara("@Thickness", Thickness);
                    proc.AddPara("@Thickness", Convert.ToDecimal(Thickness));
                    //proc.AddPara("@size", size);
                    proc.AddPara("@size", Convert.ToDecimal(size));
                    //proc.AddPara("@SUOM", SUOM);
                    proc.AddPara("@SUOM", Convert.ToDecimal(SUOM));
                    proc.AddPara("@series", series);

                    proc.AddPara("@Finish", Finish);
                    //proc.AddPara("@LeadTime", LeadTime);
                    proc.AddIntegerPara("@LeadTime", Convert.ToInt32(LeadTime));
                    //proc.AddPara("@Coverage", Coverage);
                    if (Coverage != null && Coverage != "")
                    {
                        proc.AddPara("@Coverage", Convert.ToDecimal(Coverage));
                    }
                    
                    //proc.AddPara("@covuom", covuom);
                    //proc.AddPara("@volume", volume);
                    proc.AddPara("@volume", Convert.ToDecimal(volume));
                    //proc.AddPara("@volumeuom", volumeuom);
                    //proc.AddPara("@weight", weight);
                    proc.AddPara("@weight", Convert.ToDecimal(weight));
                    proc.AddPara("@printname", printname);
                    proc.AddPara("@subcat", subcat);
                    proc.AddPara("@ProductDimension", strdimension);
                    proc.AddPara("@ProductPedestalNo", strpedestalno);
                    proc.AddPara("@ProductCatNo", strcatno);
                    proc.AddPara("@ProductWarranty", strwarranty);
                    //End of rev 24514
                    proc.AddIntegerPara("@NumberSchemaId", Convert.ToInt32(numberingId));
                    proc.AddIntegerPara("@Application", Convert.ToInt32(Application));
                    proc.AddIntegerPara("@Nature", Convert.ToInt32(Nature));
                    //Rev Tanmoy 24-04-2020
                    //rev 24514
                    //proc.AddIntegerPara("@isComponentService", ComponentService);
                    proc.AddIntegerPara("@isComponentService", Convert.ToInt32(ComponentService));
                    //End of rev 24514
                    proc.AddPara("@ModelList", ModelList);
                    //Rev Tanmoy 24-04-2020
                    proc.AddPara("@DesignNo", _DesignNo);
                    proc.AddPara("@RevisionNo", _RevisionNo);
                    proc.AddBooleanPara("@sProducts_Replaceable", Replaceable);
                    proc.AddIntegerPara("@Application_Area", Convert.ToInt32(ApplicationArea));
                    proc.AddPara("@Movement", Movement);
                    proc.AddDecimalPara("@sProduct_Cost", 5, 18, CostPrice);
                    proc.RunActionQuery();
                    int NoOfRowEffected = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        //end Rev Rajdip
        public int InsertStockWithNewProduct(int ProductId, string Finyear)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertStockWithNewProduct"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);
                    proc.AddVarcharPara("@Finyear", 20, Finyear);

                    proc.RunActionQuery();
                    int NoOfRowEffected = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        public int UpdateProduct(int ProductId, string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
            string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
            int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ModifyUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, string BarCode,
            Boolean isInventory, string stkValuation, decimal salePrice, decimal minSalePrice, decimal purPrice,
            //rev srijeeta
            decimal packageqty  ,
            //end of rev srijeeta
            decimal MRP, int? stockUOM, decimal minLvl, decimal reOrdrLvl,
            string negativeStock, int taxCodeSale, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string prodStatus, string hsnCode, int serviceTax,
            decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, bool isInstall, int Brand, Boolean isCapitalGoods, int tdsCode, Boolean sProducts_IsOldUnit,
             string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
            decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage, string covuom, string volume, string volumeuom, string weight, string printname,string subcat
           )
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_UpdatesProduct"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);
                    proc.AddVarcharPara("@ProductCode", 100, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 5000, ProductDescription);
                    proc.AddVarcharPara("@ProductType", 10, ProductType);
                    if (ProductClassCode != null)
                    {
                        proc.AddIntegerPara("@ProductClassCode", ProductClassCode, QueryParameterDirection.Input);
                    }
                    proc.AddVarcharPara("@ProductGlobalCode", 100, ProductGlobalCode);
                    if (ProductTradingLot != null)
                    {
                        proc.AddIntegerPara("@ProductTradingLot", ProductTradingLot, QueryParameterDirection.Input);
                    }
                    if (productTradingLotUnit != null)
                    {
                        proc.AddIntegerPara("@productTradingLotUnit", productTradingLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteCurrency != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteCurrency", ProductQuoteCurrency, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteLot != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteLot", ProductQuoteLot, QueryParameterDirection.Input);
                    }
                    if (productQuoteLotUnit != null)
                    {
                        proc.AddIntegerPara("@productQuoteLotUnit", productQuoteLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLot != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLot", ProductDeliveryLot, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLotUnit != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLotUnit", ProductDeliveryLotUnit, QueryParameterDirection.Input);
                    }
                    //if (ProductColor != null)
                    //{
                    //    proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    //}
                    //if (ProductSize != null)
                    //{
                    //    proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    //}
                    if (ProductColor != 0)
                    {
                        proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    }
                    if (ProductSize != 0)
                    {
                        proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    }
                    if (ModifyUser != null)
                    {
                        proc.AddIntegerPara("@ModifyUser", ModifyUser, QueryParameterDirection.Input);
                    }
                    //.................Code Added By Sam on 25102016..........................................................
                    proc.AddBooleanPara("@sProducts_SizeApplicable", sizeapplicable, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_ColorApplicable", colorapplicable, QueryParameterDirection.Input);
                    //.................Code Above Added By Sam on 25102016....................................................

                    //.................Code Added By Debjyoti on 30122016..........................................................
                    proc.AddIntegerPara("@ProductBarCodeType", BarCodeSymbology, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProducts_barCode", 50, BarCode);

                    //----------Code Added by Surojit 08-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isOverideConvertion", OverideConvertion, QueryParameterDirection.Input);

                    //----------Code Added by Surojit 11-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isComponentsMandatory", IsMandatory, QueryParameterDirection.Input);

                    //-----------------Code added by debjyoti 04-01-2017
                    proc.AddBooleanPara("@sProducts_isInventory", isInventory, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_Stockvaluation", 5, stkValuation);
                    proc.AddDecimalPara("@sProduct_SalePrice", 0, 18, salePrice);
                    proc.AddDecimalPara("@sProduct_MinSalePrice", 0, 18, minSalePrice);
                    proc.AddDecimalPara("@sProduct_PurPrice", 0, 18, purPrice);
                    //rev srijeeta
                    proc.AddDecimalPara("@sProduct_packageqty", 0, 18, packageqty);
                    //end of rev srijeeta
                    proc.AddDecimalPara("@sProduct_MRP", 0, 18, MRP);
                    if (stockUOM != null)
                    {
                        proc.AddIntegerPara("@sProduct_StockUOM", stockUOM, QueryParameterDirection.Input);
                    }
                    proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrdrLvl);
                    proc.AddVarcharPara("@sProduct_NegativeStock", 5, negativeStock);
                    proc.AddIntegerPara("@sProduct_TaxSchemeSale", taxCodeSale, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxSchemePur", taxCodePur, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxScheme", taxScheme, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProduct_AutoApply", autoApply, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_ImagePath", 200, ImagePath);
                    proc.AddVarcharPara("@ProdComponent", 1000, ProdComponent);
                    proc.AddVarcharPara("@sProduct_Status", 5, prodStatus);
                    proc.AddVarcharPara("@sProducts_HsnCode", 10, hsnCode);
                    //02-02-2017
                    proc.AddIntegerPara("@sProducts_serviceTax", serviceTax, QueryParameterDirection.Input);

                    //For Packing Details
                    proc.AddDecimalPara("@sProduct_quantity", 5, 18, quantity);
                    proc.AddDecimalPara("@packing_quantity", 5, 18, packing);
                    proc.AddIntegerPara("@packing_saleUOM", packingUOM, QueryParameterDirection.Input);
                    //Packing work end here
                    proc.AddBooleanPara("@sProducts_isInstall", isInstall);
                    proc.AddIntegerPara("@sProducts_Brand", Brand, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_isCapitalGoods", isCapitalGoods);

                    proc.AddIntegerPara("@sProducts_tdsCode", tdsCode);
                    proc.AddBooleanPara("@sProducts_IsOldUnit", sProducts_IsOldUnit);

                    proc.AddBooleanPara("@BusinessFurtherness", FurtheranceToBusiness);//Subhabrata

                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddBooleanPara("@sProducts_IsServiceItem", IsServiceItem);
                    proc.AddDecimalPara("@reorder_qty", 4, 18, reorder_qty);
                    proc.AddPara("@maxLvl", maxLvl);


                    proc.AddPara("@lenght", lenght);
                    proc.AddPara("@width", width);
                    proc.AddPara("@Thickness", Thickness);
                    proc.AddPara("@size", size);
                    proc.AddPara("@SUOM", SUOM);
                    proc.AddPara("@series", series);

                    proc.AddPara("@Finish", Finish);
                    proc.AddPara("@LeadTime", LeadTime);
                    proc.AddPara("@Coverage", Coverage);
                   // proc.AddPara("@covuom", covuom);
                    proc.AddPara("@volume", volume);
                   // proc.AddPara("@volumeuom", volumeuom);
                    proc.AddPara("@weight", weight);
                    proc.AddPara("@printname", printname);
                    proc.AddPara("@subcat", subcat);

                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        //Rev Rajdip
        public int UpdateProducts(string strdimension, string strpedestalno, string strcatno, string strwarranty, int? cisv, int ProductId, string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
           string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
           int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ModifyUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, string BarCode,
           Boolean isInventory, string stkValuation, decimal salePrice, decimal minSalePrice, decimal purPrice, 
            //rev srijeeta
            decimal packageqty  ,
            //end of rev srijeeta
            decimal MRP, int? stockUOM, decimal minLvl, decimal reOrdrLvl,
           string negativeStock, int taxCodeSale, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string prodStatus, string hsnCode, int serviceTax,
           decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, bool isInstall, int Brand, Boolean isCapitalGoods, int tdsCode, Boolean sProducts_IsOldUnit,
            string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
            decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage,
            string covuom, string volume, string volumeuom, string weight, string printname, string subcat, int ComponentService, String ModelList, string _DesignNo, string _RevisionNo,string ItemType
            , Boolean Replaceable, int Application = 0, int Nature = 0, int ApplicationArea = 0, string Movement = null, decimal CostPrice = 0, string VendorIDs=null
            )
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_UpdatesProduct"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);
                    proc.AddVarcharPara("@ProductCode", 100, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 5000, ProductDescription);
                    proc.AddVarcharPara("@ProductType", 10, ProductType);
                    if (ProductClassCode != null)
                    {
                        proc.AddIntegerPara("@ProductClassCode", ProductClassCode, QueryParameterDirection.Input);
                    }
                    proc.AddVarcharPara("@ProductGlobalCode", 100, ProductGlobalCode);
                    if (ProductTradingLot != null)
                    {
                        proc.AddIntegerPara("@ProductTradingLot", ProductTradingLot, QueryParameterDirection.Input);
                    }
                    if (productTradingLotUnit != null)
                    {
                        proc.AddIntegerPara("@productTradingLotUnit", productTradingLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteCurrency != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteCurrency", ProductQuoteCurrency, QueryParameterDirection.Input);
                    }
                    if (ProductQuoteLot != null)
                    {
                        proc.AddIntegerPara("@ProductQuoteLot", ProductQuoteLot, QueryParameterDirection.Input);
                    }
                    if (productQuoteLotUnit != null)
                    {
                        proc.AddIntegerPara("@productQuoteLotUnit", productQuoteLotUnit, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLot != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLot", ProductDeliveryLot, QueryParameterDirection.Input);
                    }
                    if (ProductDeliveryLotUnit != null)
                    {
                        proc.AddIntegerPara("@ProductDeliveryLotUnit", ProductDeliveryLotUnit, QueryParameterDirection.Input);
                    }
                    //if (ProductColor != null)
                    //{
                    //    proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    //}
                    //if (ProductSize != null)
                    //{
                    //    proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    //}
                    if (ProductColor != 0)
                    {
                        proc.AddIntegerPara("@ProductColor", ProductColor, QueryParameterDirection.Input);
                    }
                    if (ProductSize != 0)
                    {
                        proc.AddIntegerPara("@ProductSize", ProductSize, QueryParameterDirection.Input);
                    }
                    if (ModifyUser != null)
                    {
                        proc.AddIntegerPara("@ModifyUser", ModifyUser, QueryParameterDirection.Input);
                    }
                    //Rev Rajdip
                    if (cisv != null)
                    {
                        proc.AddIntegerPara("@cisv", cisv, QueryParameterDirection.Input);
                    }
                    //End Rev Rajdip
                    //.................Code Added By Sam on 25102016..........................................................
                    proc.AddBooleanPara("@sProducts_SizeApplicable", sizeapplicable, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_ColorApplicable", colorapplicable, QueryParameterDirection.Input);
                    //.................Code Above Added By Sam on 25102016....................................................

                    //.................Code Added By Debjyoti on 30122016..........................................................
                    proc.AddIntegerPara("@ProductBarCodeType", BarCodeSymbology, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProducts_barCode", 50, BarCode);

                    //----------Code Added by Surojit 08-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isOverideConvertion", OverideConvertion, QueryParameterDirection.Input);

                    //----------Code Added by Surojit 11-02-2019---------------------------------------
                    proc.AddBooleanPara("@sProducts_isComponentsMandatory", IsMandatory, QueryParameterDirection.Input);

                    //-----------------Code added by debjyoti 04-01-2017
                    proc.AddBooleanPara("@sProducts_isInventory", isInventory, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_Stockvaluation", 5, stkValuation);
                    proc.AddDecimalPara("@sProduct_SalePrice", 0, 18, salePrice);
                    proc.AddDecimalPara("@sProduct_MinSalePrice", 0, 18, minSalePrice);
                    proc.AddDecimalPara("@sProduct_PurPrice", 0, 18, purPrice);
                    //rev srijeeta 
                    proc.AddDecimalPara("@sProduct_packageqty", 0, 18, packageqty);
                    //end of rev srijeeta
                    proc.AddDecimalPara("@sProduct_MRP", 0, 18, MRP);
                    if (stockUOM != null)
                    {
                        proc.AddIntegerPara("@sProduct_StockUOM", stockUOM, QueryParameterDirection.Input);
                    }
                    proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrdrLvl);
                    proc.AddVarcharPara("@sProduct_NegativeStock", 5, negativeStock);
                    proc.AddIntegerPara("@sProduct_TaxSchemeSale", taxCodeSale, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxSchemePur", taxCodePur, QueryParameterDirection.Input);
                    proc.AddIntegerPara("@sProduct_TaxScheme", taxScheme, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProduct_AutoApply", autoApply, QueryParameterDirection.Input);
                    proc.AddVarcharPara("@sProduct_ImagePath", 200, ImagePath);
                    proc.AddVarcharPara("@ProdComponent", 1000, ProdComponent);
                    proc.AddVarcharPara("@sProduct_Status", 5, prodStatus);
                    proc.AddVarcharPara("@sProducts_HsnCode", 10, hsnCode);
                    //02-02-2017
                    proc.AddIntegerPara("@sProducts_serviceTax", serviceTax, QueryParameterDirection.Input);

                    //For Packing Details
                    proc.AddDecimalPara("@sProduct_quantity", 5, 18, quantity);
                    proc.AddDecimalPara("@packing_quantity", 5, 18, packing);
                    proc.AddIntegerPara("@packing_saleUOM", packingUOM, QueryParameterDirection.Input);
                    //Packing work end here
                    proc.AddBooleanPara("@sProducts_isInstall", isInstall);
                    proc.AddIntegerPara("@sProducts_Brand", Brand, QueryParameterDirection.Input);
                    proc.AddBooleanPara("@sProducts_isCapitalGoods", isCapitalGoods);

                    proc.AddIntegerPara("@sProducts_tdsCode", tdsCode);
                    proc.AddBooleanPara("@sProducts_IsOldUnit", sProducts_IsOldUnit);

                    proc.AddBooleanPara("@BusinessFurtherness", FurtheranceToBusiness);//Subhabrata

                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddBooleanPara("@sProducts_IsServiceItem", IsServiceItem);
                    proc.AddDecimalPara("@reorder_qty", 4, 18, reorder_qty);
                    proc.AddPara("@maxLvl", maxLvl);


                    proc.AddPara("@lenght", lenght);
                    proc.AddPara("@width", width);
                    proc.AddPara("@Thickness", Thickness);
                    proc.AddPara("@size", size);
                    proc.AddPara("@SUOM", SUOM);
                    proc.AddPara("@series", series);

                    proc.AddPara("@Finish", Finish);
                    proc.AddPara("@LeadTime", LeadTime);
                    proc.AddPara("@Coverage", Coverage);
                    // proc.AddPara("@covuom", covuom);
                    proc.AddPara("@volume", volume);
                    // proc.AddPara("@volumeuom", volumeuom);
                    proc.AddPara("@weight", weight);
                    proc.AddPara("@printname", printname);
                    proc.AddPara("@ProductDimension", strdimension);
                    proc.AddPara("@ProductPedestalNo", strpedestalno);
                    proc.AddPara("@ProductCatNo", strcatno);
                    proc.AddPara("@ProductWarranty", strwarranty);
                    proc.AddPara("@subcat", subcat);
                    proc.AddIntegerPara("@Application", Convert.ToInt32(Application));
                    proc.AddIntegerPara("@Nature", Convert.ToInt32(Nature));

                    //Rev Tanmoy 24-04-2020
                    proc.AddIntegerPara("@isComponentService", ComponentService);
                    proc.AddPara("@ModelList", ModelList);
                    //Rev Tanmoy 24-04-2020
                    proc.AddPara("@DesignNo", _DesignNo);
                    proc.AddPara("@RevisionNo", _RevisionNo);
                    proc.AddBooleanPara("@sProducts_Replaceable", Replaceable);
                    proc.AddPara("@ItemType", ItemType);
                    proc.AddIntegerPara("@Application_Area", Convert.ToInt32(ApplicationArea));
                    proc.AddPara("@Movement", Movement);
                    proc.AddDecimalPara("@sProduct_Cost", 0, 18, CostPrice);
                    //Rev Bapi
                    proc.AddPara("@VENDORIDS", VendorIDs);
                    //end rev Bapi
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        //End Rev Rajdip
        /*-----------------------------update active dormant product   Arindam  05-03-2019-------------------------------------------------*/


        public int UpdateActiveDormant(int ProductId, decimal minLvl, decimal reOrdrLvl, string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, decimal reorder_qty,
   decimal maxLvl, int? ModifyUser, decimal MinSalesPrice, decimal SalesPrice, decimal MRPSalesPrice, decimal PurchasePrice, decimal ActiveCostPrice
             )
        {                                      
            ProcedureExecute proc;          
            try
            {
                using (proc = new ProcedureExecute("Sp_UpdatesActiveDormantProduct"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);

                    if (ModifyUser != null)
                    {
                        proc.AddIntegerPara("@ModifyUser", ModifyUser, QueryParameterDirection.Input);
                    }

                    proc.AddPara("@sProduct_MinLvl", minLvl);
                    proc.AddDecimalPara("@sProduct_reOrderLvl", 0, 18, reOrdrLvl);


                    proc.AddVarcharPara("@sInv_MainAccount", 50, salesInvMainAct);
                    proc.AddVarcharPara("@sRet_MainAccount", 50, salesRetMainAct);
                    proc.AddVarcharPara("@pInv_MainAccount", 50, purInv);
                    proc.AddVarcharPara("@pRet_MainAccount", 50, purRetMainAct);
                    proc.AddPara("@maxLvl", maxLvl);
                    proc.AddPara("@MinSalesPrice", MinSalesPrice);
                    proc.AddDecimalPara("@SalesPrice", 4, 18, SalesPrice);
                    proc.AddPara("@MRPSalesPrice", MRPSalesPrice);
                    proc.AddPara("@PurchasePrice", PurchasePrice);
                    proc.AddPara("@ActiveCostPrice", ActiveCostPrice);
                    
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }


        /*-----------------------------update active dormant product   Arindam  05-03-2019-------------------------------------------------*/















        public DataTable GetProductDetails(int ProductId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                 using (proc = new ProcedureExecute("Sp_sProductDetailsById"))
                {
                    proc.AddIntegerPara("@ProductId", ProductId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        #endregion

        #region Size
        public DataTable GetSizeList()
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_SizeList"))
                {
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertSize(string SizeName, string SizeDescription, int CreatedUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertSize"))
                {
                    proc.AddVarcharPara("@SizeName", 100, SizeName);
                    proc.AddVarcharPara("@SizeDescription", 100, SizeDescription);
                    proc.AddIntegerPara("@CreatedUser", CreatedUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int UpdateSize(int SizeId, string SizeName, string SizeDescription, int ModifiedUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_UpdateSize"))
                {
                    proc.AddIntegerPara("@SizeId", SizeId);
                    proc.AddVarcharPara("@SizeName", 100, SizeName);
                    proc.AddVarcharPara("@SizeDescription", 100, SizeDescription);
                    proc.AddIntegerPara("@ModifiedUser", ModifiedUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetSizeDetails(int SizeId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_SizeDetailsById"))
                {
                    proc.AddIntegerPara("@SizeId", SizeId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetSizeUOMMapDetails(int SizeDetailsId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_SizeUOMMapDetailsById"))
                {
                    proc.AddIntegerPara("@SizeDetailsId", SizeDetailsId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertSizeDetails(int SizeDetailsMainId, string SizeDetailsAttributeName, string SizeDetailsValue, int SizeDetailsUOM, int CreatedUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertSizeDetails"))
                {
                    proc.AddIntegerPara("@SizeDetailsMainId", SizeDetailsMainId);
                    proc.AddVarcharPara("@SizeDetailsAttributeName", 100, SizeDetailsAttributeName);
                    proc.AddVarcharPara("@SizeDetailsValue", 100, SizeDetailsValue);
                    proc.AddIntegerPara("@SizeDetailsUOM", SizeDetailsUOM);
                    proc.AddIntegerPara("@CreatedUser", CreatedUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int UpdateSizeDetails(int SizeDetailsId, string SizeDetailsAttributeName, string SizeDetailsValue, int SizeDetailsUOM, int ModifiedUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_UpdatetSizeDetails"))
                {
                    proc.AddIntegerPara("@SizeDetailsId", SizeDetailsId);
                    proc.AddVarcharPara("@SizeDetailsAttributeName", 100, SizeDetailsAttributeName);
                    proc.AddVarcharPara("@SizeDetailsValue", 100, SizeDetailsValue);
                    proc.AddIntegerPara("@SizeDetailsUOM", SizeDetailsUOM);
                    proc.AddIntegerPara("@ModifiedUser", ModifiedUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetSizeDetailsList(int SizeDetailsId)
        {

            {
                ProcedureExecute proc;
                DataTable rtrnvalue;
                try
                {
                    using (proc = new ProcedureExecute("Sp_SizeDetailsListById"))
                    {
                        proc.AddIntegerPara("@SizeDetailsId", SizeDetailsId);
                        rtrnvalue = proc.GetTable();
                        return rtrnvalue;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                finally
                {
                    proc = null;
                }
            }
        }
        #endregion

        #region Order
        public int InsertOrder(string OrderCompany, int OrderBranch, string OrderDate, string OrderFinYear, string OrderType, string OrderNo,
            string OrderContactId, string OrderRefNumber, string OrderAgentId, string OrderInstruction, string OrderPaymentTerm, string OrderPaymentDate,
            string OrderDeliveryDate, string OrderDeliveryAt, int OrderDeliveryBranch, int OrderDeliveryWareHouse, int OrderDeliveryAddress,
            string OrderDeliveryOther, int CreatedUser, int OrderPaymentDays, string ParentOrderNo)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("SP_InsertpOrder"))
                {
                    proc.AddVarcharPara("@OrderCompany", 100, OrderCompany);
                    proc.AddIntegerPara("@OrderBranch", OrderBranch);
                    proc.AddVarcharPara("@OrderDate", 50, OrderDate);
                    proc.AddVarcharPara("@OrderFinYear", 20, OrderFinYear);
                    proc.AddVarcharPara("@OrderType", 20, OrderType);
                    proc.AddVarcharPara("@OrderNo", 50, OrderNo);
                    proc.AddVarcharPara("@OrderContactId", 20, OrderContactId);
                    proc.AddVarcharPara("@OrderRefNumber", 20, OrderRefNumber);
                    proc.AddVarcharPara("@OrderAgentId", 50, OrderAgentId);
                    proc.AddVarcharPara("@OrderInstruction", 50, OrderInstruction);
                    proc.AddVarcharPara("@OrderPaymentTerm", 20, OrderPaymentTerm);
                    proc.AddVarcharPara("@OrderPaymentDate", 20, OrderPaymentDate);
                    proc.AddVarcharPara("@OrderDeliveryDate", 20, OrderDeliveryDate);
                    proc.AddVarcharPara("@OrderDeliveryAt", 20, OrderDeliveryAt);
                    proc.AddIntegerPara("@OrderDeliveryBranch", OrderDeliveryBranch);
                    proc.AddIntegerPara("@OrderDeliveryWareHouse", OrderDeliveryWareHouse);
                    proc.AddIntegerPara("@OrderDeliveryAddress", OrderDeliveryAddress);
                    proc.AddVarcharPara("@OrderDeliveryOther", 20, OrderDeliveryOther);
                    proc.AddIntegerPara("@CreatedUser", CreatedUser);
                    proc.AddIntegerPara("@OrderPaymentDays", OrderPaymentDays);
                    proc.AddVarcharPara("@pOrder_ParentOrderNo", 20, ParentOrderNo);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertOrderDetails(int OrderDetailsOrderId, int OrderDetailsProductId, string OrderDetailsDrand, int OrderDetailsSize,
            int OrderDetailsColor, int OrderDetaisBestBeforeMonth, int OrderDetailsBestBeforeYear, int OrderDetailsQuoteCurrency, string OrderDetailsUnitPrice,
            int OrderDetailsPriceLot, string OrderDetailsQuantity, int OrderDetailsQuantityUnit, int OrderDetailsPriceLotUnit, string OrderDetailsRemarks,
            int CreatedUser, string OrderDetailsProductDescription)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertOrderDetails"))
                {
                    proc.AddIntegerPara("@OrderDetailsOrderId", OrderDetailsOrderId);
                    proc.AddIntegerPara("@OrderDetailsProductId", OrderDetailsProductId);
                    proc.AddVarcharPara("@OrderDetailsDrand", 100, OrderDetailsDrand);
                    proc.AddIntegerPara("@OrderDetailsSize", OrderDetailsSize);
                    proc.AddIntegerPara("@OrderDetailsColor", OrderDetailsColor);
                    proc.AddIntegerPara("@OrderDetaisBestBeforeMonth", OrderDetaisBestBeforeMonth);
                    proc.AddIntegerPara("@OrderDetailsBestBeforeYear", OrderDetailsBestBeforeYear);
                    proc.AddIntegerPara("@OrderDetailsQuoteCurrency", OrderDetailsQuoteCurrency);
                    proc.AddVarcharPara("OrderDetailsUnitPrice", 50, OrderDetailsUnitPrice);
                    proc.AddIntegerPara("@OrderDetailsPriceLot", OrderDetailsPriceLot);
                    proc.AddVarcharPara("@OrderDetailsQuantity", 50, OrderDetailsQuantity);
                    proc.AddIntegerPara("@OrderDetailsQuantityUnit", OrderDetailsQuantityUnit);
                    proc.AddIntegerPara("@OrderDetailsPriceLotUnit", OrderDetailsPriceLotUnit);
                    proc.AddVarcharPara("@OrderDetailsRemarks", 100, OrderDetailsRemarks);
                    proc.AddIntegerPara("@CreatedUser", CreatedUser);
                    proc.AddVarcharPara("@OrderDetailsProductDescription", 100, OrderDetailsProductDescription);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int UpdateOrder(int OrderId, int OrderBranch, string OrderDate, string OrderFinYear, string OrderType,
            string OrderContactID, string OrderRefNumber, string OrderAgentID, string OrderInstructions, string OrderPaymentTerm, string OrderPaymentDate,
            string OrderDeliveryDate, string OrderDeliveryAt, int OrderDeliveryBranch, int OrderDeliveryWareHouse, int OrderDeliveryAddress,
            string OrderDeliveryOther, int OrderModifyUser, int OrderPaymentDays, string ParentOrderRefNo)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("sp_UpdatepOrder"))
                {
                    proc.AddIntegerPara("@OrderId", OrderId);
                    if (OrderBranch != 0)
                    {
                        proc.AddIntegerPara("@OrderBranch", OrderBranch);
                    }
                    proc.AddVarcharPara("@OrderDate", 50, OrderDate);
                    proc.AddVarcharPara("@OrderFinYear", 20, OrderFinYear);
                    proc.AddVarcharPara("@OrderType", 20, OrderType);
                    proc.AddVarcharPara("@OrderContactID", 20, OrderContactID);
                    proc.AddVarcharPara("@OrderRefNumber", 50, OrderRefNumber);
                    proc.AddVarcharPara("@OrderAgentID", 50, OrderAgentID);
                    proc.AddVarcharPara("@OrderInstructions", 200, OrderInstructions);
                    proc.AddVarcharPara("@OrderPaymentTerm", 10, OrderPaymentTerm);
                    proc.AddVarcharPara("@OrderPaymentDate", 20, OrderPaymentDate);
                    proc.AddVarcharPara("@OrderDeliveryDate", 20, OrderDeliveryDate);
                    proc.AddVarcharPara("@OrderDeliveryAt", 20, OrderDeliveryAt);
                    if (OrderDeliveryBranch != 0)
                    {
                        proc.AddIntegerPara("@OrderDeliveryBranch", OrderDeliveryBranch);
                    }
                    if (OrderDeliveryWareHouse != 0)
                    {
                        proc.AddIntegerPara("@OrderDeliveryWareHouse", OrderDeliveryWareHouse);
                    }
                    if (OrderDeliveryAddress != 0)
                    {
                        proc.AddIntegerPara("@OrderDeliveryAddress", OrderDeliveryAddress);
                    }
                    proc.AddVarcharPara("@OrderDeliveryOther", 20, OrderDeliveryOther);
                    proc.AddIntegerPara("@OrderModifyUser", OrderModifyUser);
                    proc.AddIntegerPara("@OrderPaymentDays", OrderPaymentDays);
                    proc.AddVarcharPara("@pOrder_ParentOrderNo", 50, ParentOrderRefNo);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetOrderListById(int OrderId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_GetOrderListById"))
                {
                    proc.AddIntegerPara("@OrderId", OrderId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetOrderDetailsListById(int OrderDetailsId)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_GetpOrderDetailsListById"))
                {
                    proc.AddIntegerPara("@OrderDetailsId", OrderDetailsId);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetJobWOrkStockListById(int Id)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_GetJobWOrkStockListById"))
                {
                    proc.AddIntegerPara("@Id", Id);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int InsertJobWorkStock(int JWorkStock_OrderID, string JWorkStock_Number, string JWorkStock_Type, int JWorkStock_ProductID, string JWorkStock_Brand,
            int JWorkStock_Size, int JWorkStock_Color, int JWorkStock_BestBeforeMonth, int JWorkStock_BestBeforeYear, string JWorkStock_ProductDescription,
            string JWorkStock_Quantity, int JWorkStock_QuantityUnit, string JWorkStock_Remarks, int JWorkStock_CreateUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_InsertJobWorkStock"))
                {
                    proc.AddIntegerPara("@JWorkStock_OrderID", JWorkStock_OrderID);
                    proc.AddVarcharPara("@JWorkStock_Number", 50, JWorkStock_Number);
                    proc.AddVarcharPara("@JWorkStock_Type", 10, JWorkStock_Type);
                    proc.AddIntegerPara("@JWorkStock_ProductID", JWorkStock_ProductID);
                    proc.AddVarcharPara("@JWorkStock_Brand", 50, JWorkStock_Brand);
                    proc.AddIntegerPara("@JWorkStock_Size", JWorkStock_Size);
                    proc.AddIntegerPara("@JWorkStock_Color", JWorkStock_Color);
                    proc.AddIntegerPara("@JWorkStock_BestBeforeMonth", JWorkStock_BestBeforeMonth);
                    proc.AddIntegerPara("@JWorkStock_BestBeforeYear", JWorkStock_BestBeforeYear);
                    proc.AddVarcharPara("@JWorkStock_ProductDescription", 200, JWorkStock_ProductDescription);
                    proc.AddVarcharPara("@JWorkStock_Quantity", 50, JWorkStock_Quantity);
                    proc.AddIntegerPara("@JWorkStock_QuantityUnit", JWorkStock_QuantityUnit);
                    proc.AddVarcharPara("@JWorkStock_Remarks", 50, JWorkStock_Remarks);
                    proc.AddIntegerPara("@JWorkStock_CreateUser", JWorkStock_CreateUser);
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        #endregion

        #region InventoryControlCenter
        public DataTable GetTransactionEditDetailsById(int Id)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_GetTransactionEditDetailsById"))
                {
                    proc.AddIntegerPara("@Id", Id);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetTransactionInsertDetailsById(int Id)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("Sp_GetTransactionInsertDetails"))
                {
                    proc.AddIntegerPara("@Id", Id);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        #endregion




        public string getUsedMainAccountByProductId(int ProductId)
        {
            string returnValue = "";

            ProcedureExecute proc;
            DataTable retTable;
            try
            {
                using (proc = new ProcedureExecute("prc_ProductMaster_bindData"))
                {
                    proc.AddVarcharPara("@action", 20, "GetUsedMainAccount");
                    proc.AddIntegerPara("@Id", ProductId);
                    retTable = proc.GetTable();
                    if (retTable.Rows.Count > 0)
                    {
                        returnValue = Convert.ToString(retTable.Rows[0][0]);
                    }
                    return returnValue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
            

            return returnValue;

        }

    }
}
