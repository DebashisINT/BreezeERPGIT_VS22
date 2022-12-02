
using DataAccessLayer;
using ImportModuleBusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Import.Import.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class Import_Master : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<WarehouseDetails> GetWarehouse(string Branch)
        {
            //ImportMasterSettings masterBl = new ImportMasterSettings();
            //string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            List<WarehouseDetails> list = new List<WarehouseDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string Query = "select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + Branch + "') order by bui_Name";
                DataTable cust = new DataTable();
                //if (multiwarehouse == "1")
                //    cust = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + Branch + "'");
                //else
                    cust = oDBEngine.GetDataTable(Query);

                list = (from DataRow dr in cust.Rows
                        select new WarehouseDetails()
                        {
                            Value = Convert.ToString(dr["WarehouseID"]),
                            Text = Convert.ToString(dr["WarehouseName"])
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

        public object GetVendorWithBranch(string SearchKey, string BranchID)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map Where branch_id in('" + BranchID + "','0'))) as tbl " +
                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);

                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"])
                           }).ToList();

            }

            return listVen;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


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

        public object GetIncotermsValue(string SearchKey, string BranchID)
        {
            List<IncotermsModel> listVen = new List<IncotermsModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select IncoId,Incoterms,Description as Name 
                                    From Incoterms_Master " +
                                    "Where Incoterms like '%" + SearchKey + "%' or Description like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);

                listVen = (from DataRow dr in dt.Rows
                           select new IncotermsModel()
                           {
                               id = dr["IncoId"].ToString(),
                               Desc = dr["Name"].ToString(),
                               Incoterms = Convert.ToString(dr["Incoterms"])
                           }).ToList();

            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetGoodsinTransit(string SearchKey, string BranchID)
        {
            List<MainAccountForImpInvoice> listVen = new List<MainAccountForImpInvoice>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 MainAccount_ReferenceID,MainAccount_Name,MainAccount_AccountCode,MainAccount_PaymentType 
                                   From Master_MainAccount 
                                   where 
                                    MainAccount_PaymentType='TrnstGoods'
                                   And (MainAccount_BankCompany='" + HttpContext.Current.Session["LastCompany"] + "' Or IsNull(MainAccount_BankCompany,'')='')" + 
                                   " and (MainAccount_ExchangeSegment=1 or MainAccount_ExchangeSegment=0)" +
                                   " and (MainAccount_branchId='" + BranchID + "' OR MainAccount_branchId=0 )" +
                                   " and (MainAccount_Name like '%" + SearchKey + "%' OR MainAccount_AccountCode like '%" + SearchKey + "%')";

                DataTable dt = oDBEngine.GetDataTable(strQuery);

                listVen = (from DataRow dr in dt.Rows
                           select new MainAccountForImpInvoice()
                           {
                               MainAccount_ReferenceID = dr["MainAccount_ReferenceID"].ToString(),
                               MainAccount_Name = dr["MainAccount_Name"].ToString(),
                               MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"])
                           }).ToList();

            }

            return listVen;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetLedger(string SearchKey, string BranchID)
        {
            List<MainAccountForImpInvoice> listVen = new List<MainAccountForImpInvoice>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 MainAccount_ReferenceID,MainAccount_Name,MainAccount_AccountCode,MainAccount_PaymentType 
                                   From Master_MainAccount 
                                   where 
                                    MainAccount_BankCashType Not In ('Bank','Cash') and
                                  (MainAccount_BankCompany='" + HttpContext.Current.Session["LastCompany"] + "' Or IsNull(MainAccount_BankCompany,'')='')" +
                                   " and (MainAccount_ExchangeSegment=1 or MainAccount_ExchangeSegment=0)" +
                                   " and (MainAccount_branchId='" + BranchID + "' OR MainAccount_branchId=0 )" +
                                   " and (MainAccount_Name like '%" + SearchKey + "%' OR MainAccount_AccountCode like '%" + SearchKey + "%')";

                DataTable dt = oDBEngine.GetDataTable(strQuery);

                listVen = (from DataRow dr in dt.Rows
                           select new MainAccountForImpInvoice()
                           {
                               MainAccount_ReferenceID = dr["MainAccount_ReferenceID"].ToString(),
                               MainAccount_Name = dr["MainAccount_Name"].ToString(),
                               MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"])
                           }).ToList();

            }

            return listVen;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseProductForPO(string SearchKey, string InventoryType)
        {

            List<POProductModel> ProductList = new List<POProductModel>();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopUp");
            proc.AddVarcharPara("@InventoryType", 2, InventoryType);
            proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataTable Producttbl = proc.GetTable();

            ProductList = (from DataRow dr in Producttbl.Rows
                           select new POProductModel()
                           {
                               Id = Convert.ToString(dr["Products_ID"]),
                               ProductCode = Convert.ToString(dr["Products_Name"]),
                               Name = Convert.ToString(dr["ProductsName"]),

                               IsInventory = Convert.ToString(dr["IsInventory"]),
                               HSNSAC = Convert.ToString(dr["HSNSAC"]),
                               ClassCode = Convert.ToString(dr["ClassCode"]),
                               BrandName = Convert.ToString(dr["BrandName"])


                           }).ToList();

            return ProductList;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountForPaymentDet(string BranchId)
        {
            List<MainActPaymentDet> listMainAct = new List<MainActPaymentDet>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_ucPayementDetails");
                proc.AddVarcharPara("@action", 50, "GetMainActByBranch");
                proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
                DataTable Addresstbl = proc.GetTable();

                listMainAct = (from DataRow dr in Addresstbl.Rows
                               select new MainActPaymentDet()
                               {
                                   MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),
                                   MainAccount_Name = Convert.ToString(dr["MainAccount_Name"]),
                                   MainAccount_BankCashType = Convert.ToString(dr["MainAccount_BankCashType"]),
                                   MainAccount_branchId = Convert.ToInt64(dr["MainAccount_branchId"])
                               }).ToList();
            }

            return listMainAct;
        }


     


    }

    public class WarehouseDetails
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class VendorModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
    }

    public class MainActPaymentDet
    {
        public string MainAccount_Name { get; set; }
        public string MainAccount_AccountCode { get; set; }
        public string MainAccount_BankCashType { get; set; }
        public Int64 MainAccount_branchId { get; set; }
    }

    public class MainAccountForImpInvoice
    {
        public string MainAccount_ReferenceID { get; set; }
        public string MainAccount_Name { get; set; }
        public string MainAccount_AccountCode { get; set; }
       
    }
    public class IncotermsModel
    {
        public string id { get; set; }
        public string Incoterms { get; set; }
        public string Desc { get; set; }
    }

    public class POProductModel
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string IsInventory { get; set; }
        public string HSNSAC { get; set; }
        public string ClassCode { get; set; }
        public string BrandName { get; set; }
        //public string isInstall { get; set; }

    }

    public class CustomerModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }

}