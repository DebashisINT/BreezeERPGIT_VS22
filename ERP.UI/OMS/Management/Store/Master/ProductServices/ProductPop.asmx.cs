

using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Store.Master.ProductServices
{
    /// <summary>
    /// Summary description for ProductPop
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   [System.Web.Script.Services.ScriptService]
    public class ProductPop : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  object GetClassDetails(string SearchKey)
        {
            List<ClassModel> listClass = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable classes = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CLASSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(classes);

                cmd.Dispose();
                con.Dispose();

                listClass = (from DataRow dr in classes.Rows
                             select new ClassModel()
                             {
                                 id = dr["ID"].ToString(),
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            }

            return listClass;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUnit()
        {
            DataTable dt = new DataTable();
            List<unitModel> listClass = new List<unitModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
               
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                dt = oGenericMethod.GetDataTable("select UOM_ID,UOM_Name from Master_UOM  order by UOM_Name");

                listClass = (from DataRow dr in dt.Rows
                             select new unitModel()
                             {
                                 UomId = dr["UOM_ID"].ToString(),
                                 UomName = dr["UOM_Name"].ToString(),
                             }).ToList();
            }

            return listClass;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        
        public  bool CheckUniqueName(string ProductName, int procode)
        {
            DataTable dt = new DataTable();
            ProductName = ProductName.Replace("'", "''");
            bool IsPresent = false;
            //BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            if (procode == 0)
            {
                dt = oDBEngine.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");
            }
            else
            {
                dt = oDBEngine.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "' and sProducts_ID<>" + procode + "");
            }
            //DataTable dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["sProducts_Name"]) > 0)
                {
                    IsPresent = true;
                }
            }
            return IsPresent;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  object GetHSNDetails(string SearchKey)
        {
            List<HSNModel> listMainAccount = new List<HSNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
               
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("select top 10  Code , isnull(Code,'') HSNCode,isnull(Description,'') HSNDesc from tbl_HSN_Master where Code like '%" + SearchKey + "%' or Description like '%" + SearchKey + "%'   order by Description");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new HSNModel()
                                   {
                                       HSNId = Convert.ToInt32(dr["Code"]),
                                       id = Convert.ToString(dr["HSNCode"]),
                                       Name = Convert.ToString(dr["HSNDesc"]),

                                   }).ToList();
            }

            return listMainAccount;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSACDetails(string SearchKey)
        {
            List<SACModel> listMainAccount = new List<SACModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("select top 10  TAX_ID , isnull(SERVICE_CATEGORY_CODE,'') SACCode,isnull(SERVICE_TAX_NAME,'') SACDesc from TBL_MASTER_SERVICE_TAX where SERVICE_CATEGORY_CODE like '%" + SearchKey + "%' or SERVICE_TAX_NAME like '%" + SearchKey + "%'   order by SERVICE_TAX_NAME");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new SACModel()
                                   {
                                       SACId = Convert.ToInt32(dr["TAX_ID"]),
                                       SACCode = Convert.ToString(dr["SACCode"]),
                                       SACDEsc = Convert.ToString(dr["SACDesc"]),

                                   }).ToList();
            }

            return listMainAccount;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForProductMaster";

            List<NumberingDetails> GrpDet = new List<NumberingDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
                proc.AddVarcharPara("@Action", 100, actionqry);
                proc.AddVarcharPara("@userbranchlist", 4000, userbranchHierarchy);

                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new NumberingDetails
                          {
                              Id = Convert.ToString(dr["Id"]),
                              SchemaName = Convert.ToString(dr["SchemaName"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getProductNumberSchemeSettings()
        {
            string NmberingSettingVal = "0";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='UniqueAutoNumberProductMaster' and IsActive=1");
            if (dt.Rows.Count == 1)
            {
                string AutoNumberAllow = Convert.ToString(dt.Rows[0]["Variable_Value"]);
                if (!String.IsNullOrEmpty(AutoNumberAllow))
                {
                    if (AutoNumberAllow == "Yes")
                    {
                        NmberingSettingVal = "1";
                    }
                    else
                    {
                        NmberingSettingVal = "0";
                    }
                }

            }
            return NmberingSettingVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object ProductPopUpSettings()
        {

            CommonBL cbl = new CommonBL();
            string AltNameMandatory = cbl.GetSystemSettingsResult("AltNameMandatory");
            return AltNameMandatory;
            
      }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object ProductUOMConversionSettings()
        {

            BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();
            string ShowUOMConversion = objmaster.GetSettings("ShowUOMConversionInEntry");
            return ShowUOMConversion;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public int InsertLightweightProductPopUp(string ProductCode, string ProductName, string ProductDescription, string ProductType, int? ProductClassCode,
               string ProductGlobalCode, int? ProductTradingLot, int? productTradingLotUnit, int? ProductQuoteCurrency, int? ProductQuoteLot,
               int? productQuoteLotUnit, int? ProductDeliveryLot, int? ProductDeliveryLotUnit, int? ProductColor, int? ProductSize, int? ProductCreateUser, Boolean sizeapplicable, Boolean colorapplicable, int? BarCodeSymbology, String BarCode, Boolean isInventory, string stkValuation
               , decimal salePrice, decimal minSalePrice, decimal purPrice, decimal MRP, int? stockUOM, decimal minLvl, decimal reOrderlvl,
               string negativeStock, int? taxCodeSaleScheme, int? taxCodePur, int? taxScheme, Boolean autoApply, string ImagePath, string ProdComponent, string ProdStatus, string hsnValue, int serviceTax,
               decimal quantity, decimal packing, int packingUOM, Boolean OverideConvertion, Boolean IsMandatory, Boolean isInstall, int Brand, Boolean isCapitalGoods, int TdsCode, string FinYear, Boolean isOldUnit,
               string salesInvMainAct, string salesRetMainAct, string purInv, string purRetMainAct, Boolean FurtheranceToBusiness, Boolean IsServiceItem, decimal reorder_qty,
               decimal maxLvl, string lenght, string width, string Thickness, string size, string SUOM, string series, string Finish, string LeadTime, string Coverage, string covuom, string volume, string volumeuom, string weight, string printname, string subcat, int NumberingId=0)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Sp_LightWeight_InsertsProduct"))
                {
                    proc.AddVarcharPara("@ProductCode", 80, ProductCode);
                    proc.AddVarcharPara("@ProductName", 100, ProductName);
                    proc.AddVarcharPara("@ProductDescription", 500, ProductDescription);
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
                    proc.AddIntegerPara("@NumberSchemaId", Convert.ToInt32(NumberingId));




                    proc.RunActionQuery();
                    int NoOfRowEffected = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));

                    DataTable dts = new DataTable();

                    dts = oDBEngine.GetDataTable("select isnull(sProducts_Code,'') sProducts_Code from master_sproducts where sProducts_ID='" + NoOfRowEffected + "'");
                    if (dts.Rows.Count == 1)
                    {
                        if (Convert.ToString(dts.Rows[0]["sProducts_Code"]) == "Auto")
                        {
                            ProcedureExecute pr = new ProcedureExecute("Prc_DeleteAutoProduct");
                            pr.AddBigIntegerPara("@Product_Id", NoOfRowEffected);
                            pr.RunActionQuery();
                            NoOfRowEffected = 0;
                        }
                    }




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
    }


    public class ClassModel
    {
        public string id { get; set; }
        public string Name { get; set; }
    }
    public class unitModel
    {
        public string UomId { get; set; }
        public string UomName { get; set; }
    }
    public class HSNModel
    {
        public int HSNId { get; set; }
        public string id { get; set; }
        public string Name { get; set; }
    }
    public class SACModel
    {
        public int SACId { get; set; }
        public string SACCode { get; set; }
        public string SACDEsc { get; set; }
    }


    public class NumberingDetails
    {

        public string Id { get; set; }
        public string SchemaName { get; set; }
    }
}
