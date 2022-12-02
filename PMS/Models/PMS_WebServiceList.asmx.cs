using DataAccessLayer;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace PMS.Models
{
    /// <summary>
    /// Summary description for PMS_WebServiceList
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PMS_WebServiceList : System.Web.Services.WebService
    {

        string ConnectionString = String.Empty;
        public PMS_WebServiceList()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsList(string SearchKey, String Type = null)
        {
            List<ProductDetails> list = new List<ProductDetails>();

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                    string Query = "";
                    if (!String.IsNullOrEmpty(Type) && Type == "nonInventory")
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code
	 from Master_sProducts LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProduct_IsInventory = 0) AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                        // LEFT OUTER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID 
                    }
                    else if (Type == "SellableProduct")
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='0' THEN 'No' WHEN sProduct_IsInventory='1' THEN 'Yes' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code
	 from Master_sProducts LEFT OUTER JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProducts_ItemType in ('Sellable','Both')) AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                        // LEFT OUTER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID 
                    }
                    else if (Type != "nonInventory")
                    {

                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='1' THEN 'Yes' WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM, sProduct_PurPrice,0 AS WarehouseID,'' AS WarehouseName,BRN.Brand_Name,PCLASS.ProductClass_Code
	 from Master_sProducts JOIN Master_UOM ON Master_UOM.UOM_ID = Master_sProducts.sProduct_StockUOM LEFT JOIN tbl_master_brand BRN ON Master_sProducts.sProducts_Brand = BRN.Brand_Id
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE  (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%')  ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                    }//((sProducts_Type = 'A' OR sProducts_Type = 'B') OR (sProduct_IsInventory = 0)) AND INNER JOIN v_Stock_WarehouseDetails WarehouseD ON WarehouseD.ProductID = Master_sProducts.sProducts_ID AND sProduct_IsInventory=1 

                    else
                    {
                        Query = @"   select TOP 250 sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,sProducts_HsnCode,(CASE WHEN sProduct_IsInventory='1' THEN 'Yes' WHEN sProduct_IsInventory='0' THEN 'No' END) AS IsInventory ,UOM_Name AS StockUOM,BRN.Brand_Name,PCLASS.ProductClass_Code
	 LEFT JOIN Master_ProductClass PCLASS ON PCLASS.ProductClass_ID = Master_sProducts.ProductClass_Code WHERE (sProducts_Type = 'C' OR sProducts_Type = 'B') AND (sProducts_Code like '%" + SearchKey + "%' OR sProducts_Name like '%" + SearchKey + "%') ORDER BY sProducts_Name ASC, LEN(sProducts_Name) ASC ";
                    }

                    DataTable dt = oDBEngine.GetDataTable(Query);

                    if (!String.IsNullOrEmpty(Type))
                    {

                        list = (from DataRow dr in dt.Rows
                                select new ProductDetails()
                                {
                                    sProducts_ID = Convert.ToString(dr["sProducts_ID"]) + "|" + Convert.ToString(dr["StockUOM"]) + "|" + Convert.ToString(dr["sProducts_Description"]) + "|" + Convert.ToString(dr["sProduct_PurPrice"]) + "|" + Convert.ToString(dr["WarehouseID"]) + "|" + Convert.ToString(dr["WarehouseName"]) + "|" + Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["sProducts_HsnCode"]),
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
                                select new ProductDetails()
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
        public object GetProposalList(string SearchKey, string CustomerId, string BranchId)
        {
            List<ProposaluoteList> list = new List<ProposaluoteList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                    string Query = "";
                    Query = @"   select TOP 10  Quote_Id,ISNULL(Quote_Number,'') Quote_Number,Convert(nvarchar(10),Quote_Date,105) Quote_Date FROM tbl_trans_Quotation WHERE  Quote_Id not in (select Doc_Id from trans_transactionprojectmapping where Doc_Type='Project_Quotation') and Quote_BranchId='" + BranchId + "' and  Customer_Id='" + CustomerId + "' and (isnull(IsProjectQuotation,0) = 1) and (isnull(IsClosed,0)<>1) AND (Quote_Number like '%" + SearchKey + "%' OR Quote_Id like '%" + SearchKey + "%') ORDER BY Quote_Id desc ";

                   
                    DataTable dt = oDBEngine.GetDataTable(Query);

                    list = (from DataRow dr in dt.Rows
                            select new ProposaluoteList()
                            {
                                Id = Convert.ToInt64(dr["Quote_Id"]),
                                ProposalNo = Convert.ToString(dr["Quote_Number"]),
                                ProposalDate = Convert.ToString(dr["Quote_Date"])

                            }).ToList();

                }

            }
            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProposalListWithProject(string SearchKey, string CustomerId, string BranchId, Int64 ProjectId)
        {
            List<ProposaluoteList> list = new List<ProposaluoteList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (SearchKey != "")
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                    string Query = "";

                    Query = @"   select TOP 10  Quote_Id,ISNULL(Quote_Number,'') Quote_Number,Convert(nvarchar(10),Quote_Date,105) Quote_Date FROM tbl_trans_Quotation inner join trans_transactionprojectmapping on  Quote_Id=Doc_Id and Doc_Type='Project_Quotation' WHERE Project_Id='" + Convert.ToInt64(ProjectId) + "' AND  Quote_BranchId='" + BranchId + "' and  Customer_Id='" + CustomerId + "' and (isnull(IsProjectQuotation,0) = 1) and (isnull(IsClosed,0)<>1) AND (Quote_Number like '%" + SearchKey + "%' OR Quote_Id like '%" + SearchKey + "%') ORDER BY Quote_Id desc ";

                    DataTable dt = oDBEngine.GetDataTable(Query);

                    list = (from DataRow dr in dt.Rows
                            select new ProposaluoteList()
                            {
                                Id = Convert.ToInt64(dr["Quote_Id"]),
                                ProposalNo = Convert.ToString(dr["Quote_Number"]),
                                ProposalDate = Convert.ToString(dr["Quote_Date"])

                            }).ToList();

                }

            }
            return list;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetContractList(string SearchKey, String ProductID, String BranchID, DateTime? BOMDate)
        {

            List<ContractDetails> list = new List<ContractDetails>();
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
                //proc.AddVarcharPara("@CompanyID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //proc.AddVarcharPara("@USERID", 50, Convert.ToString(HttpContext.Current.Session["userbranchID"]));
                //proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

                ds = proc.GetDataSet();

                //Query = @" select Details_ID,Production_ID,BOM_No,BOM_Date,REV_No from BOM_Details where Finished_ProductID = '" + ProductID + "' AND (BOM_No like '%" + SearchKey + "%')";

                //DataTable dt = oDBEngine.GetDataTable(Query);
                DataTable dt = ds.Tables[0];
                list = (from DataRow dr in dt.Rows
                        select new ContractDetails()
                        {
                            Details_ID = Convert.ToString(dr["Details_ID"]) + "|" + Convert.ToString(dr["Production_ID"]) + "|" + Convert.ToString(dr["BOM_No"]) + "|" + Convert.ToString(dr["REV_No"]) + "|" + Convert.ToString(dr["BOM_Date"]) + "|" + Convert.ToDecimal(dr["Rate"]).ToString(),
                            //Production_ID = Convert.ToString(dr["Production_ID"]),
                            Contract_No = Convert.ToString(dr["BOM_No"]),
                            Contract_Date = Convert.ToDateTime(dr["BOM_Date"]).ToString("dd-MM-yyyy"),
                            REV_Date = Convert.ToString(dr["REV_Date"]) != "" ? Convert.ToDateTime(dr["REV_Date"]).ToString("dd-MM-yyyy") : "",
                            REV_No = Convert.ToString(dr["REV_No"])
                        }).ToList();
            }

            return list;
        }

    }

    public class ProductDetails
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
    public class ProposaluoteList
    {
        public Int64 Id { get; set; }
        public string ProposalNo { get; set; }
        public string ProposalDate { get; set; }

    }


}
