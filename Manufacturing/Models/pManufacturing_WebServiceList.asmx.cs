using BusinessLogicLayer;
using DataAccessLayer;
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace Manufacturing.Models
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class pManufacturing_WebServiceList : System.Web.Services.WebService
    {
        string ConnectionString = String.Empty;
        public pManufacturing_WebServiceList()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsList(string SearchKey, String Type = null)
        {

            List<ProductDetails> list = new List<ProductDetails>();
            //List<NonProductDetails> List1 = new List<NonProductDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = "";
                Query = @"   EXEC usp_Manufacturing_Warehouse_Get '" + Type + "','" + SearchKey + "' ";
              

                DataTable dt = oDBEngine.GetDataTable(Query);

                if (!String.IsNullOrEmpty(Type))
                {

                    list = (from DataRow dr in dt.Rows
                            select new ProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProduct_PurPrice"]) + "|" + Convert.ToString(dr["WarehouseID"]) + "|" + Convert.ToString(dr["WarehouseName"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]) + "|" + Convert.ToString(dr["StockUOMId"]),
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }
                else
                {
                    list = (from DataRow dr in dt.Rows
                            select new ProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]),

                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }

            }

            return list;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsListMPS(string SearchKey, String Type = null)
        {

            List<ProductDetails> list = new List<ProductDetails>();
            //List<NonProductDetails> List1 = new List<NonProductDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = "";
                Query = @"   EXEC usp_Manufacturing_Warehouse_Get '" + Type + "','" + SearchKey + "' ";


                DataTable dt = oDBEngine.GetDataTable(Query);

                if (!String.IsNullOrEmpty(Type))
                {

                    list = (from DataRow dr in dt.Rows
                            select new ProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProduct_PurPrice"]) + "|" + Convert.ToString(dr["WarehouseID"]) + "|" + Convert.ToString(dr["WarehouseName"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]) + "|" + Convert.ToString(dr["StockUOMId"]),
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }
                else
                {
                    list = (from DataRow dr in dt.Rows
                            select new ProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]),

                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }

            }

            return list;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJobOrderProductDetailsList(string SearchKey, String Type = null)
        {

            List<JobProductDetails> list = new List<JobProductDetails>();
            //List<NonProductDetails> List1 = new List<NonProductDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = "";
                Query = @"   EXEC usp_Manufacturing_JobWarehouse_Get '" + Type + "','" + SearchKey + "' ";


                DataTable dt = oDBEngine.GetDataTable(Query);

                if (!String.IsNullOrEmpty(Type))
                {

                    list = (from DataRow dr in dt.Rows
                            select new JobProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProduct_PurPrice"]) + "|" + Convert.ToString(dr["WarehouseID"]) + "|" + Convert.ToString(dr["WarehouseName"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]) + "|" + Convert.ToString(dr["StockUOMId"]),
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                UOM = Convert.ToString(dr["StockUOM"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }
                else
                {
                    list = (from DataRow dr in dt.Rows
                            select new JobProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["IsInventory"]),

                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                UOM = Convert.ToString(dr["StockUOM"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }

            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBalanceFGProductDetailsList(string SearchKey, String Type = null,string IssueId="0")
        {

            List<FGProductDetails> list = new List<FGProductDetails>();
            //List<NonProductDetails> List1 = new List<NonProductDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt =new DataTable();
                SearchKey = SearchKey.Replace("'", "''");
                ProcedureExecute proc = new ProcedureExecute("usp_ProductionIssueDataGet");
                proc.AddVarcharPara("@ACTION", 150, "GetProductionIssueFinishItem");
                proc.AddVarcharPara("@SearchKey", 400, SearchKey);
                proc.AddBigIntegerPara("@ProductionIssueID", Convert.ToInt64(IssueId));
                dt = proc.GetTable();


                

                    list = (from DataRow dr in dt.Rows
                            select new FGProductDetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["StockUOMId"]) + "|" + Convert.ToString(dr["BalanceQty"]) + "|" + Convert.ToString(dr["Rate"]) + "|" + Convert.ToString(dr["OldQty"]) + "|" + Convert.ToString(dr["maxQty"]) + "|" + Convert.ToString(dr["HeaderInventoryType"]),
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                UOM = Convert.ToString(dr["StockUOM"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                Brand = Convert.ToString(dr["Brand_Name"]),
                                Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                
            }

            return list;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBalanceReturnFGProductDetailsList(string SearchKey, String Type = null, string IssueId = "0")
        {

            List<FGProductDetails> list = new List<FGProductDetails>();
            //List<NonProductDetails> List1 = new List<NonProductDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                SearchKey = SearchKey.Replace("'", "''");
                ProcedureExecute proc = new ProcedureExecute("usp_ProductionIssueDataGet");
                proc.AddVarcharPara("@ACTION", 150, "GetReturnFGFinishItem");
                proc.AddVarcharPara("@SearchKey", 400, SearchKey);
                proc.AddBigIntegerPara("@ProductionIssueID", Convert.ToInt64(IssueId));
                dt = proc.GetTable();




                list = (from DataRow dr in dt.Rows
                        select new FGProductDetails()
                        {
                            sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["ItemRevisionNo"]) + "|" + Convert.ToString(dr["StockUOMId"]) + "|" + Convert.ToString(dr["BalanceQty"]) + "|" + Convert.ToString(dr["Rate"]) + "|" + Convert.ToString(dr["OldQty"]) + "|" + Convert.ToString(dr["maxQty"]) + "|" + Convert.ToString(dr["HeaderInventoryType"]),
                            sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                            sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                            UOM = Convert.ToString(dr["StockUOM"]),
                            IsInventory = Convert.ToString(dr["IsInventory"]),
                            sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                            Brand = Convert.ToString(dr["Brand_Name"]),
                            Class = Convert.ToString(dr["ProductClass_Code"])
                        }).ToList();

            }

            return list;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPartNoDetailsList(string SearchKey, String Action)
        {
            List<PartNODetails> list = new List<PartNODetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = "";
                Query = @"   EXEC usp_Manufacturing_Warehouse_Get ' ','" + SearchKey + "','" + Action + "' ";             
                DataTable dt = oDBEngine.GetDataTable(Query);
                if (!String.IsNullOrEmpty(Action))
                {
                    list = (from DataRow dr in dt.Rows
                            select new PartNODetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["sProducts_Code"]) + "|" + Convert.ToString(dr["DesignNo"]) + "|" + Convert.ToString(dr["RevisionNo"]) ,
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                //sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                //IsInventory = Convert.ToString(dr["IsInventory"]),
                                //sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                //Brand = Convert.ToString(dr["Brand_Name"]),
                                //Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }
                else
                {
                    list = (from DataRow dr in dt.Rows
                            select new PartNODetails()
                            {
                                sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]),
                                sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                //sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                //IsInventory = Convert.ToString(dr["IsInventory"]),
                                //sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                //Brand = Convert.ToString(dr["Brand_Name"]),
                                //Class = Convert.ToString(dr["ProductClass_Code"])
                            }).ToList();
                }
            }
            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBOMList(string SearchKey, String ProductID, String BranchID, DateTime? BOMDate)
        {
            List<BOMDetails> list = new List<BOMDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string Query = "";

                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
                proc.AddVarcharPara("@ACTION", 150, "GetBOMList");
                proc.AddBigIntegerPara("@ProductID", Convert.ToInt64(ProductID));
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchID));
                proc.AddVarcharPara("@SearchKey", 250, SearchKey);
                proc.AddPara("@BOM_Date", BOMDate);         

                ds = proc.GetDataSet();
             
                DataTable dt = ds.Tables[0];
                list = (from DataRow dr in dt.Rows
                        select new BOMDetails()
                        {
                            Details_ID = Convert.ToString(dr["Details_ID"]) + "|" + Convert.ToString(dr["Production_ID"]) + "|" + Convert.ToString(dr["BOM_No"]) + "|" + Convert.ToString(dr["REV_No"]) + "|" + Convert.ToString(dr["BOM_Date"]) + "|" + Convert.ToDecimal(dr["Rate"]).ToString() + "|" + Convert.ToString(dr["REV_Date"]),                            
                            BOM_No = Convert.ToString(dr["BOM_No"]),
                            BOM_Date = Convert.ToDateTime(dr["BOM_Date"]).ToString("dd-MM-yyyy"),
                            REV_No = Convert.ToString(dr["REV_No"]),
                            REV_Date = Convert.ToString(dr["REV_Date"]) != "" ? Convert.ToDateTime(dr["REV_Date"]).ToString("dd-MM-yyyy") : ""
                           
                        }).ToList();
            }

            return list;
        }

       


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int CheckDuplicateSerial(string SerialNo, string ProductID, string BranchID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("proc_CheckDuplicateProduct");
                proc.AddVarcharPara("@Action", 500, "PurchaseChallan");
                proc.AddVarcharPara("@SerialNo", 500, SerialNo);
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@BranchID", 500, BranchID);
                proc.AddVarcharPara("@DocID", 500, Convert.ToString(Session["PurchaseChallan_Id"]));
                dt = proc.GetTable();
                return dt.Rows.Count;
            }
            return 0;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                DataTable cust = oDBEngine.GetDataTable("select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_group_customerDetails  where  uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMPSProductDetailsListForMRP(string SearchKey, String MPSID = null)
        {
            List<MPSProductDetails> list = new List<MPSProductDetails>();
            DataTable dt;
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    
                    string Query = "";
                    if (!String.IsNullOrEmpty(MPSID))
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,sProduct_IsInventory
                        ,(CASE WHEN sProduct_IsInventory='0' THEN 'No' WHEN sProduct_IsInventory='1' THEN 'Yes' END) AS IsInventory 
                        ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,Isnull(BRN.Brand_Name,'')Brand_Name,PCLASS.ProductClass_Code,UOM_ID,Qty
                        from Master_sProducts 
                        inner join MPS_Products on ProductID=sProducts_ID
                        LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM 
                        LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
                        LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code 
                        WHERE Details_ID='" + MPSID + "' and (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC";
 
                        
                       
                    }                
                    
                    dt = oDBEngine.GetDataTable(Query);

                    if (!String.IsNullOrEmpty(MPSID))
                    {

                        list = (from DataRow dr in dt.Rows
                                select new MPSProductDetails()
                                {
                                    sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["Qty"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["UOM_ID"]),
                                    sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                    sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                    UOM = Convert.ToString(dr["StockUOM"]),
                                    IsInventory = Convert.ToString(dr["IsInventory"]),
                                    sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                    Brand = Convert.ToString(dr["Brand_Name"]),
                                    Class = Convert.ToString(dr["ProductClass_Code"])


                                }).ToList();
                    }
                    
                }
                else
                {
                    string Query = "";
                    if (!String.IsNullOrEmpty(MPSID))
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,sProduct_IsInventory
                        ,(CASE WHEN sProduct_IsInventory='0' THEN 'No' WHEN sProduct_IsInventory='1' THEN 'Yes' END) AS IsInventory 
                        ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,Isnull(BRN.Brand_Name,'')Brand_Name,PCLASS.ProductClass_Code,UOM_ID,Qty
                        from Master_sProducts 
                        inner join MPS_Products on ProductID=sProducts_ID
                        LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM 
                        LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
                        LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code 
                        WHERE Details_ID='" + MPSID + "'  ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC";
                    }


                    dt = oDBEngine.GetDataTable(Query);

                    if (!String.IsNullOrEmpty(MPSID))
                    {

                        list = (from DataRow dr in dt.Rows
                                select new MPSProductDetails()
                                {
                                    sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["Qty"])   + "|" + Convert.ToString(dr["sProducts_Name"])  + "|" + Convert.ToString(dr["UOM_ID"]),
                                    sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                    sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                    UOM = Convert.ToString(dr["StockUOM"]),
                                    IsInventory = Convert.ToString(dr["IsInventory"]),
                                    sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                    Brand = Convert.ToString(dr["Brand_Name"]),
                                    Class = Convert.ToString(dr["ProductClass_Code"])


                                }).ToList();
                    }
                }
            }
            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMPSProductDetailsList(string SearchKey, String Type = null)
        {
            List<MPSProductDetails> list = new List<MPSProductDetails>();

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                    string Query = "";
                    if (!String.IsNullOrEmpty(Type) && Type == "nonInventory")
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code,UOM_ID
	 from Master_sProducts LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProduct_IsInventory = 0) AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') and sProducts_Type in ('B','C') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                        // LEFT OUTER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID 
                    }
                    else if (Type == "SellableProduct")
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='0' THEN 'No' WHEN sProduct_IsInventory='1' THEN 'Yes' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code,UOM_ID
	 from Master_sProducts LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProducts_ItemType in ('Sellable','Both')) AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') and sProducts_Type in ('B','C') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                        // LEFT OUTER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID 
                    }
                    else if (Type != "nonInventory")
                    {

                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='1' THEN 'Yes' WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code,UOM_ID
	 from Master_sProducts JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code 
     WHERE  (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%')  and sProducts_Type in ('B','C') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";

                    }//((sProducts_Type = 'A' OR sProducts_Type = 'B') OR (sProduct_IsInventory = 0)) AND INNER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID AND sProduct_IsInventory=1 

                    else
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='1' THEN 'Yes' WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM,BRN.Brand_Name,PCLASS.ProductClass_Code,UOM_ID
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProducts_Type = 'C' OR sProducts_Type = 'B') AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%')  ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                    }

                    DataTable dt = oDBEngine.GetDataTable(Query);

                    if (!String.IsNullOrEmpty(Type))
                    {

                        list = (from DataRow dr in dt.Rows
                                select new MPSProductDetails()
                                {
                                    sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProduct_PurPrice"]) + "|" + Convert.ToString(dr["WarehouseID"]) + "|" + Convert.ToString(dr["WarehouseName"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["sProducts_HsnCode"]) + "|" + Convert.ToString(dr["UOM_ID"]),
                                    sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                    sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                    UOM = Convert.ToString(dr["StockUOM"]),
                                    IsInventory = Convert.ToString(dr["IsInventory"]),
                                    sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                    Brand = Convert.ToString(dr["Brand_Name"]),
                                    Class = Convert.ToString(dr["ProductClass_Code"])


                                }).ToList();
                    }
                    else
                    {
                        list = (from DataRow dr in dt.Rows
                                select new MPSProductDetails()
                                {
                                    sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]),
                                    sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                    sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                    UOM = Convert.ToString(dr["StockUOM"]),
                                    IsInventory = Convert.ToString(dr["IsInventory"]),
                                    sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"]),
                                    Brand = Convert.ToString(dr["Brand_Name"]),
                                    Class = Convert.ToString(dr["ProductClass_Code"])

                                }).ToList();
                    }

                }
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMPSOrderDetailsList(string SearchKey, string CustomerId, string BranchID)
        {
            List<MPSOrderDetails> list = new List<MPSOrderDetails>();

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                    string Query = "";

                    if (SearchKey != "")
                    {

                        Query = @"    SELECT Order_Id,Order_Number,Order_Date FROM tbl_trans_salesorder sls		
                    WHERE  Order_Number like '%" + SearchKey + "%' and sls.Customer_Id= '" + CustomerId + "' AND sls.Order_BranchId='" + BranchID + "'";
                    }
                    
                    DataTable dt = oDBEngine.GetDataTable(Query);

                    if (!String.IsNullOrEmpty(CustomerId))
                    {

                        list = (from DataRow dr in dt.Rows
                                select new MPSOrderDetails()
                                {
                                    Order_Id = Convert.ToString(dr["Order_Id"]),
                                    Order_Number = Convert.ToString(dr["Order_Number"]),
                                    Order_Date = Convert.ToString(dr["Order_Date"]),
                                    

                                }).ToList();
                    }
                    

                }
            }
            return list;
        }

    }
    public class CustomerModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }
    public class ProductDetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string IsInventory { get; set; }
        public string sProducts_HsnCode { get; set; }

        public string Brand { get; set; }

        public string Class { get; set; }
        //public string StockUOMId { get; set; }
    }
    public class MPSProductDetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string UOM { get; set; }
        public string IsInventory { get; set; }
        public string sProducts_HsnCode { get; set; }

        public string Brand { get; set; }

        public string Class { get; set; }




    }

    public class MPSOrderDetails
    {
        public string Order_Id { get; set; }
        public string Order_Number { get; set; }
        public string Order_Date { get; set; }        

    }
    public class JobProductDetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string UOM { get; set; }
        public string IsInventory { get; set; }
        public string sProducts_HsnCode { get; set; }

        public string Brand { get; set; }
        
        public string Class { get; set; }
        //public string StockUOMId { get; set; }
    }
    public class FGProductDetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string UOM { get; set; }
        public string IsInventory { get; set; }
        public string sProducts_HsnCode { get; set; }

        public string Brand { get; set; }

        public string Class { get; set; }
        //public string StockUOMId { get; set; }
    }

    public class NonProductDetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string IsInventory { get; set; }
        public string sProducts_HsnCode { get; set; }

        public string Brand { get; set; }

        public string Class { get; set; }
       // public string StockUOMId { get; set; }
    }

    public class PartNODetails
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        //public string sProducts_Name { get; set; }
        //public string IsInventory { get; set; }
        //public string sProducts_HsnCode { get; set; }

        //public string Brand { get; set; }

        //public string Class { get; set; }
    }
}
