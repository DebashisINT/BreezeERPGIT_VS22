//1.0   Priti V2.0.36   06-02-2023    0025645: Branch Requisition - While Adding a Product, the Search is not working properly

using BusinessLogicLayer;
using DataAccessLayer;
//using DocumentFormat.OpenXml.Drawing.Charts;
using ERP.Models;
using ERP.OMS.Management.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class Master : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEntityByBranch(string BranchId, string SearchKey)
        {
            List<Entity> listMainAct = new List<Entity>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
                proc.AddVarcharPara("@action", 50, "GetEntity");
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                proc.AddVarcharPara("@filter", 100, SearchKey);
                System.Data.DataTable Addresstbl = proc.GetTable();

                listMainAct = (from DataRow dr in Addresstbl.Rows
                               select new Entity()
                               {
                                   Entity_ID = Convert.ToString(dr["ID"]),
                                   Entity_Code = Convert.ToString(dr["cnt_shortName"]),
                                   EntityName = Convert.ToString(dr["EntityName"]),


                               }).ToList();
            }


            return listMainAct;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTechnician( string SearchKey)
        {
            List<Technician> listMainAct = new List<Technician>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
                proc.AddVarcharPara("@action", 50, "GetTechnician");                
                proc.AddVarcharPara("@filter", 100, SearchKey);
                DataTable Addresstbl = proc.GetTable();

                listMainAct = (from DataRow dr in Addresstbl.Rows
                               select new Technician()
                               {
                                   ID = Convert.ToString(dr["ID"]),
                                   Technician_Name = Convert.ToString(dr["Technician_Name"]),
                                   

                               }).ToList();
            }


            return listMainAct;
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
                // DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev

                CommonBL obj = new CommonBL();

                string settings = obj.GetSystemSettingsResult("IsLeadAvailableinTransactions");
                DataTable cust = new DataTable();
                if (settings.ToUpper() == "YES")
                    cust = oDBEngine.GetDataTable(" select * from (select distinct top 250  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");
                else
                    cust = oDBEngine.GetDataTable(" select * from (select distinct top 250  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where (pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%') and Type<>'Lead' ) as t order by t.Name ");

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
        public object GetVendorwiseAmountdocument(string VendorId, string FromDate, string TodoDate, string BranchID, string InventoryType)
        {
            List<VendorAmountDocument> listCust = new List<VendorAmountDocument>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                
                  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
            proc.AddVarcharPara("@action", 500, "GetVendorWiseTotalTransactedAmtDetails");
            proc.AddIntegerPara("@branch", Convert.ToInt32(BranchID));
            proc.AddVarcharPara("@InternalId", 500, VendorId);
            proc.AddVarcharPara("@InvType", 100, InventoryType);
            proc.AddVarcharPara("@AmtFromDate", 100, (FromDate));
            proc.AddVarcharPara("@AmtToDate", 100, (TodoDate));
            DataTable dt = proc.GetTable();



            listCust = (from DataRow dr in dt.Rows
                        select new VendorAmountDocument()
                            {
                                id = dr["id"].ToString(),
                                Invoice_Number = dr["Invoice_Number"].ToString(),
                                Invoice_Date = Convert.ToString(dr["Invoice_Date"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                UnPaidAmount = Convert.ToString(dr["UnPaidAmount"])
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerInvChallan(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                // DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev
                string SalesRegCust = "";
                CommonBL obj = new CommonBL();
                MasterSettings objmaster = new MasterSettings();
                string ActinveInvoice = objmaster.GetSettings("ActiveEInvoice");
                string settings = obj.GetSystemSettingsResult("IsLeadAvailableinTransactions");
                SalesRegCust = obj.GetSystemSettingsResult("EInvoiceREgisteredCustomer");
                DataTable cust = new DataTable();
                //Rev work start 19.07.2022 mantice no :0025043: Unable to fetch Customer in the Customer Search in the Sales Invoice Cum Challan module
                //Funcationality done as per sales invoice customer search
                /*if (settings.ToUpper() == "YES")
                {
                    if (ActinveInvoice == "1" || SalesRegCust.ToUpper() == "YES")
                    {
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where  isnull(pcd.GSTIN,'')<>'' and  pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");

                    }
                    else
                    {
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");

                    }
                }
                else
                {
                    if (ActinveInvoice == "1" || SalesRegCust.ToUpper() == "YES")
                    {
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where isnull(pcd.GSTIN,'')<>'' and (pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%') and Type<>'Lead' ) as t order by t.Name ");
                    }
                    else
                    {
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where (pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%') and Type<>'Lead' ) as t order by t.Name ");

                    }
                }*/
                if (settings.ToUpper() == "YES")
                    cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");
                else
                    cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where (pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%') and Type<>'Lead' ) as t order by t.Name ");
                //Rev work close mantice no :0025043: Unable to fetch Customer in the Customer Search in the Sales Invoice Cum Challan module
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
        public object GetTechnicianByBranch(string BranchId, string SearchKey)
        {
            List<Entity> listMainAct = new List<Entity>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
                proc.AddVarcharPara("@action", 50, "GetTechnician");
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                proc.AddVarcharPara("@filter", 100, SearchKey);
                DataTable Addresstbl = proc.GetTable();

                listMainAct = (from DataRow dr in Addresstbl.Rows
                               select new Entity()
                               {
                                   Entity_ID = Convert.ToString(dr["ID"]),
                                   EntityName = Convert.ToString(dr["Technician_Name"]),
                                   Entity_Code = Convert.ToString(dr["cnt_shortName"]),

                               }).ToList();
            }


            return listMainAct;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEInvErrorMsg(string DocId, string DocType, string ErrType)
        {
            List<EInvErrorLog> EInvErrorLog = new List<EInvErrorLog>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
                proc.AddVarcharPara("@action", 50, "GetEInvErrorMsg");
                proc.AddBigIntegerPara("@DocId", Convert.ToInt64(DocId));
                proc.AddVarcharPara("@DocType", 100, DocType);
                proc.AddVarcharPara("@ErrType", 100, ErrType);
                DataTable Addresstbl = proc.GetTable();

                EInvErrorLog = (from DataRow dr in Addresstbl.Rows
                                select new EInvErrorLog()
                                {
                                    DocumentNo = Convert.ToString(dr["DocumentNo"]),
                                    DocumentDate = Convert.ToString(dr["DocumentDate"]),
                                    Errorcode = Convert.ToString(dr["Errorcode"]),
                                    Errormsg = Convert.ToString(dr["Errormsg"]),
                                    ErrorType = Convert.ToString(dr["ErrorType"])
                                }).ToList();
            }


            return EInvErrorLog;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetParty(string SearchKey)
        {
            List<VendorModelVPR> listCust = new List<VendorModelVPR>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,shortname , Name ,Type   from v_PBVendorDetail where shortname like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


                listCust = (from DataRow dr in cust.Rows
                            select new VendorModelVPR()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["shortname"]),
                                Type = Convert.ToString(dr["Type"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWarehouseByBranch(string BranchID)
        {
            Allddl All = new Allddl();

            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                MasterSettings masterBl = new MasterSettings();
                DataTable WareHouse = new DataTable();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                if (multiwarehouse != "1")
                {
                    WareHouse = oDBEngine.GetDataTable("select  tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + BranchID + "') order by bui_Name");
                }
                else
                {
                    WareHouse = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + BranchID + "'");
                }
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,shortname , Name ,Type   from v_PBVendorDetail where shortname like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //DataTable WareHouse = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + BranchID + "') order by bui_Name");

                All.ForWareHouse = (from DataRow dr in WareHouse.Rows
                                    select new ddlClass()
                                    {
                                        Id = dr["WarehouseID"].ToString(),
                                        Name = dr["WarehouseName"].ToString()

                                    }).ToList();
            }

            return All;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWarehouseWisePRoductStock(string WarehouseID, string productid)
        {
            string availablestock = "0";

            DataTable dtMainAccount = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "Getstock");
            proc.AddVarcharPara("@productid", 500, productid);
            proc.AddVarcharPara("@WarehouseID", 100, WarehouseID);

            dtMainAccount = proc.GetTableModified();

            if (dtMainAccount != null && dtMainAccount.Rows.Count > 0)
            {
                availablestock = Convert.ToString(dtMainAccount.Rows[0][0]);
            }


            return availablestock;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWarehouseByBranchStockTransfer(string SearchKey, string BranchID)
        {
            //AllddlWH All = new AllddlWH();
            List<ddlWH> listWH = new List<ddlWH>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                // string Query = " Select * from ( select   cast(bui_id as varchar)+'~'+isnull(bui_address1,'') +space(1)+isnull(bui_address2,'')+space(1)+isnull(bui_address3,'') as WarehouseID,bui_Name as WarehouseName ";
                //Query = Query + " from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + BranchID + "')) as tbl where  WarehouseName like '%" + SearchKey + "%'";


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // DataTable WareHouse = oDBEngine.GetDataTable(Query);
                DataTable WareHouse = new DataTable();

                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                if (multiwarehouse != "1")
                {
                    WareHouse = oDBEngine.GetDataTable("select  tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + BranchID + "') and bui_Name like '%" + SearchKey + "%' order by bui_Name");
                }
                else
                {
                    WareHouse = oDBEngine.GetDataTable("EXEC [GET_BRANCHFilterWISEWAREHOUSE] '1','" + BranchID + "','" + SearchKey + "'");
                }

                // DataTable WareHouse = oDBEngine.GetDataTable("select '0~'as WarehouseID,'-Select-'as WarehouseName union select   cast(bui_id as varchar)+'~'+isnull(bui_address1,'') +space(1)+isnull(bui_address2,'')+space(1)+isnull(bui_address3,'') as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + BranchID + "')");

                listWH = (from DataRow dr in WareHouse.Rows
                          select new ddlWH()
                          {
                              Id = dr["WarehouseID"].ToString(),
                              Name = dr["WarehouseName"].ToString(),

                          }).ToList();
            }

            return listWH;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWarehouseByFilterStockTransfer(string SearchKey, string BranchID, string WarehouseName)
        {
            //AllddlWH All = new AllddlWH();
            List<ddlWH> listWH = new List<ddlWH>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //string Query = " select * from ( select   cast(bui_id as varchar)+'~'+isnull(bui_address1,'') +space(1)+isnull(bui_address2,'')+space(1)+isnull(bui_address3,'') as WarehouseID,bui_Name as WarehouseName ";
                // Query = Query + " from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + BranchID + "'))tbl where WarehouseName<>'" + WarehouseName + "' and  WarehouseName like '%" + SearchKey + "%'";


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                //DataTable WareHouse = oDBEngine.GetDataTable(Query);
                DataTable WareHouse = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                if (multiwarehouse != "1")
                {
                    WareHouse = oDBEngine.GetDataTable("select  tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + BranchID + "') and bui_Name like '%" + SearchKey + "%' order by bui_Name");
                }
                else
                {
                    WareHouse = oDBEngine.GetDataTable("EXEC [GET_BRANCHFilterWISEWAREHOUSE] '1','" + BranchID + "','" + SearchKey + "'");
                }


                DataView dvData = new DataView(WareHouse);
                dvData.RowFilter = "WarehouseName <> '" + WarehouseName + "'";

                //DataTable WareHouse = oDBEngine.GetDataTable("select * from (select '0~'as WarehouseID,'-Select-'as WarehouseName union select   cast(bui_id as varchar)+'~'+isnull(bui_address1,'') +space(1)+isnull(bui_address2,'')+space(1)+isnull(bui_address3,'') as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + BranchID + "'))tbl where WarehouseName<>'"+ WarehouseName +"'");

                listWH = (from DataRow dr in dvData.ToTable().Rows
                          select new ddlWH()
                          {
                              Id = dr["WarehouseID"].ToString(),
                              Name = dr["WarehouseName"].ToString(),

                          }).ToList();
            }

            return listWH;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object LoadBankDetails(string BankCode, string BranchId)
        {
            List<bankDetails> listBank = new List<bankDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BankCode = BankCode.Replace("'", "''");
                DataTable dtBankdet = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_TermsAndCondition");
                proc.AddVarcharPara("@Action", 100, "LoadBankDetails");
                proc.AddVarcharPara("@BankCode", 100, BankCode);
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));

                dtBankdet = proc.GetTableModified();
                listBank = (from DataRow dr in dtBankdet.Rows
                            select new bankDetails()
                            {
                                BankBranchName = Convert.ToString(dr["BankBranchName"]),
                                BankBranchAddress = Convert.ToString(dr["BankBranchAddress"]),
                                BankBranchLandmark = Convert.ToString(dr["BankBranchLandmark"]),
                                BankBranchPin = Convert.ToString(dr["BankBranchPin"]),
                                BankBranchAccountNumber = Convert.ToString(dr["BankBranchAccountNumber"]),
                                SWIFT = Convert.ToString(dr["SWIFT"]),
                                RTGS = Convert.ToString(dr["RTGS"]),
                                IFSC = Convert.ToString(dr["IFSC"]),
                                Remarks = Convert.ToString(dr["Remarks"])
                            }).ToList();
            }

            return listBank;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetInvoiceDetails(string CustVenID, string BranchList, string SearchKey)
        {
            List<PurINvoiceDetails> listBank = new List<PurINvoiceDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                DataTable dtBankdet = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
                proc.AddVarcharPara("@Action", 100, "VendorCredit");
                proc.AddVarcharPara("@CustVenID", 500, CustVenID);
                proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                proc.AddVarcharPara("@BranchList", 500, BranchList);
                proc.AddVarcharPara("@filter", 100, SearchKey);

                dtBankdet = proc.GetTableModified();
                listBank = (from DataRow dr in dtBankdet.Rows
                            select new PurINvoiceDetails()
                            {
                                InvoiceID = Convert.ToString(dr["InvoiceID"]),
                                InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                PartyInvoiceNo = Convert.ToString(dr["PartyInvoiceNo"]),
                                PartyInvoiceDate = Convert.ToString(dr["PartyInvoiceDate"]),

                            }).ToList();
            }

            return listBank;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetInvoiceDetailsWithProject(string CustVenID, string BranchList, string SearchKey, Int64 ProjectId)
        {
            List<PurINvoiceDetails> listBank = new List<PurINvoiceDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                DataTable dtBankdet = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
                proc.AddVarcharPara("@Action", 100, "VendorCreditWithProject");
                proc.AddVarcharPara("@CustVenID", 500, CustVenID);
                proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                proc.AddVarcharPara("@BranchList", 500, BranchList);
                proc.AddVarcharPara("@filter", 100, SearchKey);
                proc.AddBigIntegerPara("@ProjectId", ProjectId);

                dtBankdet = proc.GetTableModified();
                listBank = (from DataRow dr in dtBankdet.Rows
                            select new PurINvoiceDetails()
                            {
                                InvoiceID = Convert.ToString(dr["InvoiceID"]),
                                InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                PartyInvoiceNo = Convert.ToString(dr["PartyInvoiceNo"]),
                                PartyInvoiceDate = Convert.ToString(dr["PartyInvoiceDate"]),

                            }).ToList();
            }

            return listBank;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetProjectDetails(string BranchList, string SearchKey)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            List<ProjectDetails> listBank = new List<ProjectDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                DataTable dtBankdet = new DataTable();

                dtBankdet = oDBEngine.GetDataTable(" select isnull(Proj_Id,0) Proj_Id,isnull(Proj_Code,'') Proj_Code,isnull(Proj_Name,'') Proj_Name,isnull(Customer,'') Customer,isnull(HIERARCHY_NAME,'') HIERARCHY_NAME   from V_ProjectList where ProjBracnchid='" + BranchList + "' and ProjectStatus ='Approved' and  Proj_Name like '%" + SearchKey + "%'  order by Proj_Code");


                listBank = (from DataRow dr in dtBankdet.Rows
                            select new ProjectDetails()
                            {
                                Proj_Id = Convert.ToInt64(dr["Proj_Id"]),
                                Proj_Code = Convert.ToString(dr["Proj_Code"]),
                                Proj_Name = Convert.ToString(dr["Proj_Name"]),
                                Customer = Convert.ToString(dr["Customer"]),
                                HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"]),
                            }).ToList();
            }

            return listBank;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountCashBankByProcedure(string SearchKey, string branchId)
        {
            List<MainAccountCashBank> listMainAccount = new List<MainAccountCashBank>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtMainAccount = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
                proc.AddVarcharPara("@Action", 100, "GetMainAcount");
                proc.AddVarcharPara("@BranchID", 500, branchId);
                proc.AddVarcharPara("@filter", 100, SearchKey);

                dtMainAccount = proc.GetTableModified();
                listMainAccount = (from DataRow dr in dtMainAccount.Rows
                                   select new MainAccountCashBank()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"]),
                                       MainAccount_ReverseApplicable = Convert.ToString(dr["MainAccount_ReverseApplicable"]),
                                       TaxAble = Convert.ToString(dr["TaxAble"])

                                   }).ToList();
            }

            return listMainAccount;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetledgerAccountVendorPaymentReceipt(string SearchKey, string branchId)
        {
            List<MainAccountCashBank> listMainAccount = new List<MainAccountCashBank>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtMainAccount = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
                proc.AddVarcharPara("@Action", 100, "GetLedgerAcount");
                proc.AddVarcharPara("@BranchId", 500, branchId);
                proc.AddVarcharPara("@filter", 100, SearchKey);

                dtMainAccount = proc.GetTableModified();
                listMainAccount = (from DataRow dr in dtMainAccount.Rows
                                   select new MainAccountCashBank()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"]),
                                       MainAccount_ReverseApplicable = Convert.ToString(dr["MainAccount_ReverseApplicable"]),
                                       TaxAble = Convert.ToString(dr["TaxAble"])

                                   }).ToList();
            }

            return listMainAccount;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInfluencer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_InfluencerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");

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
        public object GetInfluencerWithMainAccount(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid+'~'+MainAccount_ReferenceID+'~'+MainAccount_AccountCode cnt_internalid,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_InfluencerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");

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
        public object GetAllCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_All_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


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
        public object GetEmployee(string SearchKey)
        {
            List<EntityModel> listCust = new List<EntityModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalId  ,cnt_firstName+rtrim(space(1)+isnull(cnt_middleName,''))+space(1)+cnt_lastName Name,cnt_UCC    from tbl_master_contact cnt inner join tbl_master_employee emp on cnt.cnt_internalId = emp.emp_contactId  where cnt_firstName+rtrim(space(1)+isnull(cnt_middleName,''))+space(1)+cnt_lastName like '%" + SearchKey + "%' or cnt_UCC like '%" + SearchKey + "%'   order by Name");


                listCust = (from DataRow dr in cust.Rows
                            select new EntityModel()
                            {
                                id = dr["cnt_internalId"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["cnt_UCC"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerCRP(string SearchKey, string contactType)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();

                if (contactType == "CL")
                {
                    // Rev 0019246 Subhra 26-12-2018 
                    //cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    // cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    //End of Rev
                    // cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' ) as t order by t.Name");
                    //Rev Tanmoy 29-07-2019
                    CommonBL obj = new CommonBL();
                    string settings = obj.GetSystemSettingsResult("IsLeadAvailableinTransactions");
                    if (settings.ToUpper() == "YES")
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");
                    else
                        cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where (pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%') and Type<>'Lead' ) as t order by t.Name ");

                    //End of Rev
                }
                else
                {
                    // Rev 0019246 Subhra 26-12-2018 
                    //cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_VendorTransporterDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_VendorTransporterDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                    //End of Rev
                }

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
        public object GetServiceContract(string CustomerId)
        {
            List<ServiceContractModel> listServiceContractModel = new List<ServiceContractModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                CustomerId = CustomerId.Replace("'", "''");

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable ServiceContract = new DataTable();

                ServiceContract = oDBEngine.GetDataTable("select CUSTOMERNAME,ORDERNO,ORDER_DATE,ORDER_ID from V_ServiceContractList where isfromlead=1 AND cnt_internalId='" + CustomerId + "'");

                listServiceContractModel = (from DataRow dr in ServiceContract.Rows
                                            select new ServiceContractModel()
                                            {
                                                id = dr["ORDER_ID"].ToString(),
                                                Doc_No = dr["ORDERNO"].ToString(),
                                                Doc_date = Convert.ToString(dr["ORDER_DATE"]),
                                                Customer = Convert.ToString(dr["CUSTOMERNAME"])
                                            }).ToList();
            }

            return listServiceContractModel;
        }


        //Rev Subhra
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNormalProduct(string SearchKey)
        {
            List<NormalProductModel> listnormalProducts = new List<NormalProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable normalProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_NORMALPRODUCTSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(normalProduct);

                cmd.Dispose();
                con.Dispose();

                listnormalProducts = (from DataRow dr in normalProduct.Rows
                                      select new NormalProductModel()
                                      {
                                          id = dr["ID"].ToString(),
                                          Code = dr["Code"].ToString(),
                                          Name = dr["Name"].ToString(),
                                          Hsn = dr["Hsn"].ToString()

                                      }).ToList();
            }

            return listnormalProducts;
        }
        //End of Rev Subhra
        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetInfluencer(string SearchKey, string contactType)
        //{
        //    List<CustomerModel> listCust = new List<CustomerModel>();
        //    if (HttpContext.Current.Session["userid"] != null)
        //    {
        //        SearchKey = SearchKey.Replace("'", "''");

        //        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        //        DataTable cust = new DataTable();

        //        cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_InfluencerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");

        //        listCust = (from DataRow dr in cust.Rows
        //                    select new CustomerModel()
        //                    {
        //                        id = dr["cnt_internalid"].ToString(),
        //                        Na = dr["Name"].ToString(),
        //                        UId = Convert.ToString(dr["uniquename"]),
        //                        add = Convert.ToString(dr["Billing"])
        //                    }).ToList();
        //    }

        //    return listCust;
        //}

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEntity(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_EntityDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


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
        public object GetCustomerSaleOrder(string SearchKey, string CustomerIds)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                CustomerIds = CustomerIds.TrimStart(',');

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //string Query = "select top 10 * From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_SalesOrder_customerDetails";
                string Query = "";
                CommonBL obj = new CommonBL();
                string settings = obj.GetSystemSettingsResult("IsLeadAvailableinTransactions");
                if (settings.ToUpper() == "YES")
                    Query = "select top 250 cnt_internalid ,cnt_id,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_SalesOrder_customerDetails where 1=1";
                else
                    Query = "select top 250 cnt_internalid ,cnt_id,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_SalesOrder_customerDetails WHERE Type<>'Lead'";

                //End of Rev
                if (!string.IsNullOrEmpty(CustomerIds))
                {
                    Query = Query + " and cnt_id in (" + CustomerIds + ")";
                }

                Query = Query + ") as tbl where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name";

                DataTable cust = oDBEngine.GetDataTable(Query);



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
        public object GetSegment1(string SearchKey, string CustomerIds)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();




                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ENTITY_SEGMENT_MAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindSigment1");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@InetrnalId", CustomerIds);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();

                //DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSegment2(string SearchKey, string CustomerIds)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();




                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ENTITY_SEGMENT_MAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindSigment2");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@InetrnalId", CustomerIds);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment1(string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment1List");
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment2(string Segment1_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment1_Id = Segment1_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment2List");
                cmd.Parameters.AddWithValue("@Segment_Id1", Segment1_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment3(string Segment2_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment2_Id = Segment2_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment3List");
                cmd.Parameters.AddWithValue("@Segment_Id2", Segment2_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment4(string Segment3_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment3_Id = Segment3_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment4List");
                cmd.Parameters.AddWithValue("@Segment_Id3", Segment3_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment5(string Segment4_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment4_Id = Segment4_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment5List");
                cmd.Parameters.AddWithValue("@Segment_Id4", Segment4_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }





        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleAssignSegment1(string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SegmentAssign1List");
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleAssignSegment2(string Segment1_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment1_Id = Segment1_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SegmentAssign2List");
                cmd.Parameters.AddWithValue("@Segment_Id1", Segment1_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleAssignSegment3(string Segment2_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment2_Id = Segment2_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SegmentAssign3List");
                cmd.Parameters.AddWithValue("@Segment_Id2", Segment2_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleAssignSegment4(string Segment3_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment3_Id = Segment3_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SegmentAssign4List");
                cmd.Parameters.AddWithValue("@Segment_Id3", Segment3_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleAssignSegment5(string Segment4_Id, string CustomerIds, string order_id, string orderdetails_id)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment4_Id = Segment4_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SegmentAssign5List");
                cmd.Parameters.AddWithValue("@Segment_Id4", Segment4_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }





        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSegment3(string SearchKey, string CustomerIds)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ENTITY_SEGMENT_MAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindSigment3");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@InetrnalId", CustomerIds);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();

                //DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSegment4(string SearchKey, string CustomerIds)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                //Query = Query + ") as tbl where  like '%" + SearchKey + "%' ";


                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ENTITY_SEGMENT_MAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindSigment4");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@InetrnalId", CustomerIds);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();

                //DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSegment5(string SearchKey, string CustomerIds)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();




                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ENTITY_SEGMENT_MAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindSigment5");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@InetrnalId", CustomerIds);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();

                //DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerOnIndustry(string SearchKey, string CustomerIds)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                CustomerIds = CustomerIds.TrimStart(',');

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //string Query = "select top 10 * From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_pos_customerDetails";

                string Query = "select distinct * from  (select top 10 cnt_internalid ,cnt_id,uniquename ,Name ,Billing From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_pos_customerDetails  ";

                if (!string.IsNullOrEmpty(CustomerIds))
                {
                    Query = Query + " Where cnt_id in (" + CustomerIds + ")";
                }

                // Query = Query + ") as tbl where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name";

                Query = Query + ") as tbl  LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=tbl.cnt_internalid  where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%' or mp.phf_phoneNumber like '%" + SearchKey + "%' ) as t order by t.Name ";

                DataTable cust = oDBEngine.GetDataTable(Query);


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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesManAgent(string SearchKey)
        {
            List<SalesManAgntModel> listSalesMan = new List<SalesManAgntModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10 cnt_id ,Name from v_GetAllSalesManAgent where  Name like '%" + SearchKey + "%'");


                listSalesMan = (from DataRow dr in cust.Rows
                                select new SalesManAgntModel()
                                {
                                    id = dr["cnt_id"].ToString(),
                                    Na = dr["Name"].ToString()
                                }).ToList();
            }

            return listSalesMan;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountJournal(string SearchKey, string branchId)
        {
            List<MainAccountJournal> listMainAccount = new List<MainAccountJournal>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode  from v_MainAccountList_journal where (MainAccount_Name like '%" + SearchKey + "%' or MainAccount_AccountCode like '%" + SearchKey + "%') and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "') order by Len(MainAccount_Name),MainAccount_Name asc");


                listMainAccount = (from DataRow dr in cust.Rows
                                   select new MainAccountJournal()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),

                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"])

                                   }).ToList();
            }

            return listMainAccount;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountJournalTDS(string SearchKey, string branchId, bool considerTDS, string TDSCode)
        {
            List<MainAccountJournalTDS> listMainAccount = new List<MainAccountJournalTDS>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = new DataTable();

                if (considerTDS != null && !considerTDS)
                    cust = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode,MainAccount_TDSRate  from v_MainAccountList_journalTDS where (MainAccount_Name like '%" + SearchKey + "%' or MainAccount_AccountCode like '%" + SearchKey + "%') and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "')and  MainAccount_AccountCode not in (select distinct TDSTCS_MainAccountCode from Master_TDSTCS) order by Len(MainAccount_Name),MainAccount_Name asc");
                else
                    cust = oDBEngine.GetDataTable("select top 10 * from (select MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode,MainAccount_TDSRate  from v_MainAccountList_journalTDS where (MainAccount_Name like '%" + SearchKey + "%' or MainAccount_AccountCode like '%" + SearchKey + "%') and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "') and MainAccount_AccountCode in (select TDSTCS_MainAccountCode from Master_TDSTCS where TDSTCS_Code='" + TDSCode + "') union select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode,MainAccount_TDSRate  from v_MainAccountList_journalTDS where (MainAccount_Name like '%" + SearchKey + "%' or MainAccount_AccountCode like '%" + SearchKey + "%') and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "')and  MainAccount_AccountCode not in (select distinct TDSTCS_MainAccountCode from Master_TDSTCS)) tbl  order by Len(MainAccount_Name),MainAccount_Name asc");


                listMainAccount = (from DataRow dr in cust.Rows
                                   select new MainAccountJournalTDS()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),
                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"]),
                                       MainAccount_TDSRate = Convert.ToString(dr["MainAccount_TDSRate"])
                                   }).ToList();
            }

            return listMainAccount;
        }











        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountCashBank(string SearchKey, string branchId)
        {
            List<MainAccountCashBank> listMainAccount = new List<MainAccountCashBank>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_ReverseApplicable,isnull(TAXable,'') TaxAble  from v_MainAccountList where MainAccount_Name like '%" + SearchKey + "%' and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "')");


                listMainAccount = (from DataRow dr in cust.Rows
                                   select new MainAccountCashBank()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"]),
                                       MainAccount_ReverseApplicable = Convert.ToString(dr["MainAccount_ReverseApplicable"]),
                                       TaxAble = Convert.ToString(dr["TaxAble"])

                                   }).ToList();
            }

            return listMainAccount;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountCustomerNote(string SearchKey, string branchId)
        {
            List<MainAccountCustomerDbCr> listMainAccount = new List<MainAccountCustomerDbCr>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode  from v_MainAccountList where MainAccount_Name like '%" + SearchKey + "%' and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "')");


                listMainAccount = (from DataRow dr in cust.Rows
                                   select new MainAccountCustomerDbCr()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),
                                       MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"]),



                                   }).ToList();
            }

            return listMainAccount;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSubAccountJournal(string SearchKey, string MainAccountCode)
        {
            List<SubAccount> listSubAccount = new List<SubAccount>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10 Contact_Name,SubAccount_ReferenceID,MainAccount_SubLedgerType  from v_SubAccountList where Contact_Name like '%" + SearchKey + "%'  and mainaccount_referenceid='" + MainAccountCode + "'  order by Contact_Name");


                listSubAccount = (from DataRow dr in cust.Rows
                                  select new SubAccount()
                                  {
                                      SubAccount_ReferenceID = Convert.ToString(dr["SubAccount_ReferenceID"]),
                                      Contact_Name = dr["Contact_Name"].ToString(),
                                      MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"])

                                  }).ToList();
            }

            return listSubAccount;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorWithBranch(string SearchKey, string BranchID)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 250 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
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
        public object GetVendorWithBranchPO(string SearchKey, string BranchID)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 250 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
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
        public object GetVendorWithBranchPC(string SearchKey, string BranchID)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 250 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
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
        public object GetVendorWithQuotationPrice(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map )) as tbl " +
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



        #region Transporter Section

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTransporterWithBranch(string SearchKey, string BranchID)
        {
            List<TransporterModel> listVen = new List<TransporterModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                //                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                //                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                //                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map Where branch_id in('" + BranchID + "','0'))) as tbl " +
                //                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
                proc.AddVarcharPara("@Action", 100, "PopulateTransporterDetail");
                proc.AddVarcharPara("@branchId", 200, Convert.ToString(BranchID));
                proc.AddVarcharPara("@filter", 200, Convert.ToString(SearchKey));
                dt = proc.GetTable();
                //DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new TransporterModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["shortname"]),
                               type = Convert.ToString(dr["Type"])
                           }).ToList();
            }

            return listVen;
        }
        #endregion Transporter Section


        #region Ledger Section For Self Invoice

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountForSelfInvoice(string SearchKey)
        {
            List<PurchaseLedger> listVen = new List<PurchaseLedger>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                //                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                //                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                //                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map Where branch_id in('" + BranchID + "','0'))) as tbl " +
                //                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
                proc.AddVarcharPara("@Action", 100, "GetMainAccountForSelfInvoice");
                proc.AddVarcharPara("@filter", 200, Convert.ToString(SearchKey));
                dt = proc.GetTable();
                //DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new PurchaseLedger()
                           {
                               id = dr["MainAccount_AccountCode"].ToString(),
                               Na = dr["MainAccount_Name"].ToString(),
                           }).ToList();
            }

            return listVen;
        }
        #endregion Ledger Section For Self Invoice

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorWithOutBranch(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string strQuery = "";

                strQuery = @"Select top 10  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name
                                    From tbl_master_contact Where cnt_contactType ='DV'  and cnt_contactStatus<>3
                                    union
                                    Select cnt_internalid,IsNull(cnt_UCC,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name
                                    From tbl_master_contact Where cnt_contactType='TR'
                                  ) as tbl " +
                               " Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"]),

                           }).ToList();
            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorWithOutBranchDrCr(string SearchKey)
        {


            List<VendorModelWithBranch> listVen = new List<VendorModelWithBranch>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = "";



                strQuery = @" Select top 250  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename
                ,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
                case 
                when (tbl_master_Gstin.GSTIN IS Not NULL OR tbl_master_Gstin.GSTIN<>'') then tbl_master_Gstin.GSTIN
                when tbl_master_contact.CNT_GSTIN IS Not NULL OR tbl_master_contact.CNT_GSTIN<>'' then tbl_master_contact.CNT_GSTIN				
                end
                as CNT_GSTIN
                From tbl_master_contact 
                left outer join tbl_master_Gstin on tbl_master_contact.cnt_id=tbl_master_Gstin.contact_id
                Where cnt_contactType ='DV'  and cnt_contactStatus<>3
                union
                Select cnt_internalid,IsNull(cnt_UCC,'NA') as uniquename
                ,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
                case 
                when (tbl_master_Gstin.GSTIN IS Not NULL OR tbl_master_Gstin.GSTIN<>'') then tbl_master_Gstin.GSTIN
                when tbl_master_contact.CNT_GSTIN IS Not NULL OR tbl_master_contact.CNT_GSTIN<>'' then tbl_master_contact.CNT_GSTIN				
                end
                as CNT_GSTIN
                From tbl_master_contact 
                left outer join tbl_master_Gstin on tbl_master_contact.cnt_id=tbl_master_Gstin.contact_id
                Where cnt_contactType='TR'
                ) as tbl " +
               " Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";



                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModelWithBranch()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"]),
                               GSTIN = Convert.ToString(dr["CNT_GSTIN"]),
                           }).ToList();
            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNumberingSchemaVDCNote(string SearchKey, string NoteType)
        {

            List<SchemaModel> listVen = new List<SchemaModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = "";

                if (NoteType == "Dr")
                {
                    strQuery = @"Select top 10  * From ( Select '' as Id,'Select' as  SchemaName,'' as branch_description union all 
			        select Convert(varchar(15),Id)+'~'+Convert(varchar(15),schema_type)+'~'+CONVERT(varchar(3),length) +'~'+CONVERT(VARCHAR(10),IsNull(Branch,0))
			        as Id,SchemaName
			        +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' 
			        Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End)	,
                    (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) as branch_description
			        From tbl_master_idschema where [type_Id]=27 AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + HttpContext.Current.Session["userbranchHierarchy"] + "')) " +
                    " AND Isnull(comapanyInt,'')='" + HttpContext.Current.Session["LastCompany"] + "'" +
                    " AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + HttpContext.Current.Session["LastFinYear"] + "') " +
                    "  ) as tbl " +
                    " Where SchemaName like '%" + SearchKey + "%' or branch_description like '%" + SearchKey + "%'";

                }
                else
                {
                    strQuery = @"Select top 10  * From ( Select '' as Id,'Select' as  SchemaName,'' as branch_description union all 
			        select Convert(varchar(15),Id)+'~'+Convert(varchar(15),schema_type)+'~'+CONVERT(varchar(3),length) +'~'+CONVERT(VARCHAR(10),IsNull(Branch,0))
			        as Id,SchemaName
			        +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' 
			        Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End)	,
                    (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) as branch_description	
			        From tbl_master_idschema where [type_Id]=28 AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + HttpContext.Current.Session["userbranchHierarchy"] + "')) " +
                    " AND Isnull(comapanyInt,'')='" + HttpContext.Current.Session["LastCompany"] + "'" +
                    " AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + HttpContext.Current.Session["LastFinYear"] + "') " +
                    "  ) as tbl " +
                    " Where SchemaName like '%" + SearchKey + "%' or branch_description like '%" + SearchKey + "%'";

                }

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new SchemaModel()
                           {
                               id = dr["Id"].ToString(),
                               Name = dr["SchemaName"].ToString(),
                           }).ToList();
            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorForVendorPayRec(string SearchKey, string type)
        {
            //string[] SearchKeyList = SearchList.Split(new string[] { "||@||" }, StringSplitOptions.None);
            //string SearchKey = SearchKeyList[0];
            //string BranchID = SearchKeyList[1];

            List<VendorModelVPR> listVen = new List<VendorModelVPR>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string strQuery = "";
                if (type == "DV")
                {
                    strQuery = @"Select top 250  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
                                    'Vendor' as Type
                                    From tbl_master_contact Where cnt_contactType ='DV'  and cnt_contactStatus<>3
                                    union
                                    Select cnt_internalid,IsNull(cnt_UCC,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
                                    'Transpoter' as Type
                                    From tbl_master_contact Where cnt_contactType='TR' and Statustype<>'D'
                                  ) as tbl " +
                                   " Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                }
                else
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    // strQuery = @" select top 10 cnt_internalid ,uniquename ,Name,'Customer' as Type   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name";
                    strQuery = @" select * from (select distinct top 250  pcd.cnt_internalid ,pcd.uniquename ,pcd.Name,'Customer' as Type from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' ) as t order by t.Name ";
                }


                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModelVPR()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"]),
                               Type = Convert.ToString(dr["Type"]),
                           }).ToList();
            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnlyVendor(string SearchKey, string type)
        {
            List<VendorModelVPR> listVen = new List<VendorModelVPR>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string strQuery = "";
                if (type == "DV")
                {
                    strQuery = @"Select top 10  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
                                    'Vendor' as Type
                                    From tbl_master_contact Where cnt_contactType ='DV'  and cnt_contactStatus<>3
                                   
                                  ) as tbl " +
                                   " Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                }
                else
                {
                    SearchKey = SearchKey.Replace("'", "''");
                    //strQuery = @" select top 10 cnt_internalid ,uniquename ,Name,'Customer' as Type   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name";

                    strQuery = @" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,pcd.Name,'Customer' as Type from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' ) as t order by t.Name ";
                }


                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModelVPR()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"]),
                               Type = Convert.ToString(dr["Type"]),
                           }).ToList();
            }

            return listVen;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPosProduct(string SearchKey)
        {
            List<PosProductModel> listCust = new List<PosProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10  Products_ID,Products_Name ,Products_Description ,HSNSAC  from v_Product_MargeDetailsPOS where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");


                listCust = (from DataRow dr in cust.Rows
                            select new PosProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"])
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPosProductWithBatch(string SearchKey)
        {
            List<PosProductModelWithBatch> listCust = new List<PosProductModelWithBatch>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10  Products_ID,Products_Name ,Products_Description ,HSNSAC,BatchNo  from v_Product_MargeDetailsPOSWithBatch where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' or BatchNo  like '%" + SearchKey + "%' order by Products_Name,Products_Description");


                listCust = (from DataRow dr in cust.Rows
                            select new PosProductModelWithBatch()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"]),
                                BatchNo = Convert.ToString(dr["BatchNo"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesOrderProduct(string SearchKey, string InventoryType, string ProductIds)
       {
            List<SalesOrderProductModel> listCust = new List<SalesOrderProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                ProductIds = ProductIds.TrimStart(',');
                string Query = "Select Top 250 * From (select Products_ID,Products_Name ,Products_Description ,HSNSAC,BrandName,sProducts_isInstall,ClassCode,ProductId from v_GetProductDetails_SalesOrder";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C
                if (InventoryType == "Y" && string.IsNullOrEmpty(ProductIds))
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='0'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='0'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='1'";
                }
                else if (!string.IsNullOrEmpty(ProductIds) && InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='0' AND ProductId IN (" + ProductIds + ")";
                }

                //if(!string.IsNullOrEmpty(ProductIds))
                //{
                //    Query = Query + ") as tbl Where ProductId IN (" + ProductIds + ")" +" and Products_Name like '%" + SearchKey + "%' OR Products_Description like '%" + SearchKey + "%'";
                //}
                //else
                //{
                Query = Query + ") as tbl Where Products_Name like '%" + SearchKey + "%' OR Products_Description like '%" + SearchKey + "%'";
                //}



                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new SalesOrderProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"]),
                                //Brand = Convert.ToString(dr["BrandName"]),
                                //IsInsta = Convert.ToString(dr["sProducts_isInstall"]),
                                Class = Convert.ToString(dr["ClassCode"])
                            }).ToList();


            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesOrderProductList(string SearchKey, string InventoryType, string ProductIds)
        {
            List<SalesOrderProductModelList> listCust = new List<SalesOrderProductModelList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                ProductIds = ProductIds.TrimStart(',');
                // Mantis Issue Pratik
                //string Query = "Select Top 10 * From (select Products_ID,Products_Name ,Products_Description ,HSNSAC,BrandName,sProducts_isInstall,ClassCode,ProductId,IsInventory from v_GetProductDetails_SalesOrder";
                string Query = "Select Top 250 * From (select Products_ID,Products_Name ,Products_Description ,HSNSAC,BrandName,sProducts_isInstall,ClassCode,ProductId,IsInventory from v_GetProductDetails_SalesOrder";
                // End of Mantis Issue Pratik

                //Both-->B;Inventory Item-->Y;Capital Goods-->C
                if (InventoryType == "Y" && string.IsNullOrEmpty(ProductIds))
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='0'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='0'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='1'";
                }
                else if (!string.IsNullOrEmpty(ProductIds) && InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='0' AND ProductId IN (" + ProductIds + ")";
                }

                //if(!string.IsNullOrEmpty(ProductIds))
                //{
                //    Query = Query + ") as tbl Where ProductId IN (" + ProductIds + ")" +" and Products_Name like '%" + SearchKey + "%' OR Products_Description like '%" + SearchKey + "%'";
                //}
                //else
                //{
                Query = Query + ") as tbl Where Products_Name like '%" + SearchKey + "%' OR Products_Description like '%" + SearchKey + "%'";
                //}



                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new SalesOrderProductModelList()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                //IsInsta = Convert.ToString(dr["sProducts_isInstall"]),
                                Class = Convert.ToString(dr["ClassCode"])
                            }).ToList();


            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseProduct(string SearchKey, string InventoryType)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                //Rev Pratik
                //string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Purchase_ProductDetails";
                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Purchase_ProductDetails";
                //End of rev Pratik
                //Both-->B;Inventory Item-->Y;Capital Goods-->C;Service Item-->S
                if (InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsCapitalGoods='Yes'";
                }
                else if (InventoryType == "S")
                {
                    Query = Query + " Where IsInventory='No' AND IsServiceItem='Yes'";
                }

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["sProduct_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseChallanProduct(string SearchKey, string InventoryType)
        {
            List<ProductGRNRateModel> listCust = new List<ProductGRNRateModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,GSTRate,ClassCode,BrandName from v_PurchaseChallan_ProductDetails";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C;Service Item-->S
                if (InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsCapitalGoods='Yes'";
                }
                else if (InventoryType == "S")
                {
                    Query = Query + " Where IsInventory='No' AND IsServiceItem='Yes'";
                }

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNRateModel()
                            {
                                Products_ID = Convert.ToString(dr["sProduct_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                GSTRate = Convert.ToString(dr["GSTRate"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseChallanProductForPOpup(string SearchKey, string InventoryType)
        {
            List<ProductGRNRateModel> listCust = new List<ProductGRNRateModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 250 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,GSTRate,ClassCode,BrandName from v_PurchaseChallan_ProductDetails";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C;Service Item-->S
                if (InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='No' AND IsServiceItem='No'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsCapitalGoods='Yes'";
                }
                else if (InventoryType == "S")
                {
                    Query = Query + " Where IsInventory='No' AND IsServiceItem='Yes'";
                }

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNRateModel()
                            {
                                Products_ID = Convert.ToString(dr["sProduct_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                GSTRate = Convert.ToString(dr["GSTRate"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductForStockAdjustment(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //string Query = "Select Top 10 * From (select Product_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Purchase_ProductDetails";               
                //Query = Query + " Where  IsCapitalGoods='No' AND IsServiceItem='No'";    

                string Query = " Select Top 10 * From ( select sProducts_ID Product_ID,sProducts_Code Product_Code,sProducts_Name Product_Name,";
                Query = Query + " CASE when sProduct_IsInventory=1 THEN 'Yes' else 'No'end IsInventory,";
                Query = Query + " IsNull((CASE WHEN sProduct_IsInventory=1 THEN ISNULL(sProducts_HsnCode,'') ELSE SERVICE_CATEGORY_CODE END),'') HSNSAC,";
                Query = Query + " IsNull(ProductClass_Name,'') ClassCode,IsNull(Brand_Name,'') BrandName";
                Query = Query + " from Master_sProducts";
                Query = Query + " Left Outer Join Master_ProductClass on ProductClass_ID=Master_sProducts.ProductClass_Code";
                Query = Query + " Left Outer Join  tbl_master_brand On Brand_Id=sProducts_Brand";
                Query = Query + " Left Outer Join TBL_MASTER_SERVICE_TAX on TAX_ID=sProducts_serviceTax";
                Query = Query + " where Isnull(Is_active_warehouse,0)=1 and Isnull(Is_active_serialno,0)=0 and Isnull(Is_active_Batch,0)=0 and Isnull(sProduct_IsCapitalGoods,0)=0 and Isnull(Is_ServiceItem,0)=0 and (Master_sProducts.sProduct_Status <> 'D') OR  ";
                Query = Query + " (Master_sProducts.sProduct_Status IS NULL)";

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Product_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductForWHStockTransfer(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //string Query = " Select Top 10 * From ( select  cast( sProducts_ID as varchar)+'||@||'+sProducts_Description+'||@||'+cast(Isnull(Is_active_serialno,0)as varchar)+'||@||'+cast(Isnull(Is_active_Batch,0)as varchar) Product_ID";
                //Query = Query + " ,sProducts_Code Product_Code,sProducts_Name Product_Name,";
                //Query = Query + " CASE when sProduct_IsInventory=1 THEN 'Yes' else 'No'end IsInventory,";
                //Query = Query + " IsNull((CASE WHEN sProduct_IsInventory=1 THEN ISNULL(sProducts_HsnCode,'') ELSE SERVICE_CATEGORY_CODE END),'') HSNSAC,";
                //Query = Query + " IsNull(ProductClass_Name,'') ClassCode,IsNull(Brand_Name,'') BrandName";
                //Query = Query + " from Master_sProducts";
                //Query = Query + " Left Outer Join Master_ProductClass on ProductClass_ID=Master_sProducts.ProductClass_Code";
                //Query = Query + " Left Outer Join  tbl_master_brand On Brand_Id=sProducts_Brand";
                //Query = Query + " Left Outer Join TBL_MASTER_SERVICE_TAX on TAX_ID=sProducts_serviceTax";                
                //Query = Query + " where Isnull(Is_active_warehouse,0)=1  and Isnull(sProduct_IsInventory,0)=1  and (Master_sProducts.sProduct_Status <> 'D') OR  ";                
                //Query = Query + " (Master_sProducts.sProduct_Status IS NULL)";
                //Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                string Query = " Select Top 50 * From ( select Product_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Product_ForWHStockTransfer";
                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Product_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductForWHStockInOut(string SearchKey)
        {
            List<ProductWHStockINOUT> listCust = new List<ProductWHStockINOUT>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = " Select Top 50 * From ( select Product_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName,IsReplaceable from v_Product_ForWHStockIN_OUT";
                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductWHStockINOUT()
                            {
                                Products_ID = Convert.ToString(dr["Product_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),
                                IsReplaceable = Convert.ToString(dr["IsReplaceable"]),
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerAddress(string CustomerId)
        {
            List<AddressDetails> address = new List<AddressDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
                proc.AddVarcharPara("@Action", 50, "PopulatebyCustomerId");
                proc.AddVarcharPara("@customerId", 10, CustomerId);
                DataTable Addresstbl = proc.GetTable();

                address = (from DataRow dr in Addresstbl.Rows
                           select new AddressDetails()
                           {
                               id = Convert.ToInt32(dr["add_id"]),
                               Type = Convert.ToString(dr["Type"]),
                               add_address1 = Convert.ToString(dr["add_address1"]),
                               add_address2 = Convert.ToString(dr["add_address2"]),
                               add_address3 = Convert.ToString(dr["add_address3"]),
                               couId = Convert.ToInt32(dr["couId"]),
                               conName = Convert.ToString(dr["conName"]),
                               stId = Convert.ToInt32(dr["stId"]),
                               stName = Convert.ToString(dr["stName"]),
                               pnId = Convert.ToInt32(dr["pnId"]),
                               pnCd = Convert.ToString(dr["pnCd"]),
                               ctyId = Convert.ToInt32(dr["ctyId"]),
                               ctyName = Convert.ToString(dr["ctyName"]),
                               landMk = Convert.ToString(dr["add_landMark"]),
                               GSTIN = Convert.ToString(dr["GSTIN"]),
                               deflt = Convert.ToBoolean(dr["Isdefault"])

                           }).ToList();

                return address;
            }

            return null;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseProductForGRN(string SearchKey, string InventoryType)
        {
            List<ProductGRNModel> ProductList = new List<ProductGRNModel>();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopUp");
            proc.AddVarcharPara("@InventoryType", 2, InventoryType);
            proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataTable Producttbl = proc.GetTable();

            ProductList = (from DataRow dr in Producttbl.Rows
                           select new ProductGRNModel()
                           {
                               Products_ID = Convert.ToString(dr["Products_ID"]),
                               Products_Name = Convert.ToString(dr["Products_Name"]),
                               IsInventory = Convert.ToString(dr["IsInventory"]),
                               HSNSAC = Convert.ToString(dr["HSNSAC"]),
                               ClassCode = Convert.ToString(dr["ClassCode"]),
                               BrandName = Convert.ToString(dr["BrandName"])
                           }).ToList();

            return ProductList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseProductList(string SearchKey, string InventoryType)
        {
            List<PurchaseProductModel> listCust = new List<PurchaseProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,HSNSAC from v_Purchase_ProductDetails";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C
                if (InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='No'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='Yes'";
                }

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%'";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new PurchaseProductModel()
                            {
                                id = dr["sProduct_ID"].ToString(),
                                Na = dr["Product_Code"].ToString(),
                                Des = Convert.ToString(dr["Product_Name"]),
                                HSN = Convert.ToString(dr["HSNSAC"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsForSI(string SearchKey, string InventoryType)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 250 Products_ID,Products_Name,Products_Description,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_Product_MargeDetails";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C
                if (InventoryType == "Y")
                {
                    Query = Query + " Where IsInventory='Yes' AND IsCapitalGoods='0' AND Is_ServiceItem='0'";
                }
                else if (InventoryType == "N")
                {
                    Query = Query + " Where IsInventory='No' AND IsCapitalGoods='0' AND Is_ServiceItem='0'";
                }
                else if (InventoryType == "C")
                {
                    Query = Query + " Where IsCapitalGoods='1'";
                }
                else if (InventoryType == "S")
                {
                    Query = Query + " Where IsInventory='No' AND Is_ServiceItem='1'";
                }

                Query = Query + ") as tbl Where Products_Name like '%" + SearchKey + "%' OR Products_Description like '%" + SearchKey + "%'";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Products_ID"]),
                                Product_Code = Convert.ToString(dr["Products_Name"]),
                                Products_Name = Convert.ToString(dr["Products_Description"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])
                            }).ToList();
            }

            return listCust;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsIndentRequisition(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                string Query = "Select Top 250 Products_ID,ProductsName,sProducts_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_IndentRequisitionProductList";

                Query = Query + ") as tbl Where ProductsName like '%" + SearchKey + "%' OR sProducts_Code like '%" + SearchKey + "%'";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Products_ID"]),
                                Product_Code = Convert.ToString(dr["sProducts_Code"]),
                                Products_Name = Convert.ToString(dr["ProductsName"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])
                            }).ToList();
            }
            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAllProductDetailsIndentRequisition(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                string Query = "Select Top 10 Products_ID,ProductsName,sProducts_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_IndentRequisitionAllProductList";

                Query = Query + ") as tbl Where ProductsName like '%" + SearchKey + "%' OR sProducts_Code like '%" + SearchKey + "%'";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Products_ID"]),
                                Product_Code = Convert.ToString(dr["sProducts_Code"]),
                                Products_Name = Convert.ToString(dr["ProductsName"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])
                            }).ToList();
            }
            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetailsBranchRequisition(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                //string Query = "Select Top 10 Products_ID,ProductsName,Products_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_BranchRequisitionProductList";//1.0
                string Query = "Select Top 250 Products_ID,ProductsName,Products_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_BranchRequisitionProductList";

                Query = Query + ") as tbl Where ProductsName like '%" + SearchKey + "%' OR Products_Code like '%" + SearchKey + "%'";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Products_ID"]),
                                Product_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["ProductsName"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])
                            }).ToList();
            }
            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNonInventoryProductDetailsBranchRequisition(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
               // string Query = "Select Top 10 Products_ID,ProductsName,Products_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_BranchRequisitionNonInventoryProductList";//1.0
                string Query = "Select Top 250 Products_ID,ProductsName,Products_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_BranchRequisitionNonInventoryProductList";

                Query = Query + ") as tbl Where ProductsName like '%" + SearchKey + "%' OR Products_Code like '%" + SearchKey + "%'";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["Products_ID"]),
                                Product_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["ProductsName"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])
                            }).ToList();
            }
            return listCust;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseProductForPO(string SearchKey, string InventoryType)
        {

            List<POProductModel> ProductList = new List<POProductModel>();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
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
        public object GetPurchaseProductForPONormal(string SearchKey, string InventoryType, string VendorId)
        {

            List<POProductModel> ProductList = new List<POProductModel>();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopUpPONormal");
            proc.AddVarcharPara("@InventoryType", 2, InventoryType);
            proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@VendorId", 500, VendorId);

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
        public object GetSalesReturnProduct(string SearchKey)
        {



            List<ProductSalesReturnModel> listProd = new List<ProductSalesReturnModel>();

            //SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
                proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopUp");
                proc.AddVarcharPara("@CompanyId", 10, companyid);
                proc.AddVarcharPara("@FinYear", 10, FinYear);
                proc.AddVarcharPara("@SearchKey", 100, SearchKey);
                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows
                            select new ProductSalesReturnModel()
                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])



                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }

        // Code Added by Sam on 19012018 Section Start for All Purchase Module
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseInvoiceProduct(string SearchKey, string InventoryType, string TDSCode)
        {
            List<ProductInvoiceModel> listProd = new List<ProductInvoiceModel>();

            SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
                proc.AddVarcharPara("@Action", 50, "PopulateProductOnDemandForLightWeightpurpose");
                proc.AddVarcharPara("@filter", 100, SearchKey);
                proc.AddVarcharPara("@IsInventory", 10, InventoryType);
                proc.AddVarcharPara("@SchemeID", 10, TDSCode);
                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows
                            select new ProductInvoiceModel()
                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])



                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }
        // Code above Added by sam on 19012018 Section End  for All Purchase Module

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetNormalPurchaseInvoiceProduct(string SearchKey, string InventoryType, string TDSCode)
        {
            List<NormalProductInvoiceModel> listProd = new List<NormalProductInvoiceModel>();


            SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");

                proc.AddVarcharPara("@Action", 50, "POPULATEPRODUCTONDEMANDFORLIGHTWEIGHTPURPOSEPI");

                proc.AddVarcharPara("@filter", 100, SearchKey);
                proc.AddVarcharPara("@IsInventory", 10, InventoryType);
                proc.AddVarcharPara("@SchemeID", 10, TDSCode);
                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows

                            select new NormalProductInvoiceModel()

                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),

                                GSTRate = Convert.ToString(dr["GSTRate"]),

                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])



                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseInvoiceCumChallanProduct(string SearchKey, string InventoryType, string TDSCode)
        {
            List<ProductInvoiceModel> listProd = new List<ProductInvoiceModel>();

            SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
                proc.AddVarcharPara("@Action", 50, "PopulateProductOnDemandForLightWeightpurpose");
                proc.AddVarcharPara("@filter", 100, SearchKey);
                proc.AddVarcharPara("@IsInventory", 10, InventoryType);
                proc.AddVarcharPara("@SchemeID", 10, TDSCode);
                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows
                            select new ProductInvoiceModel()
                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])



                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseReturnProduct(string SearchKey)
        {
            List<ProductSalesReturnModel> listProd = new List<ProductSalesReturnModel>();

            SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseReturn_Details");
                proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopUp");
                proc.AddVarcharPara("@CompanyId", 10, companyid);
                proc.AddVarcharPara("@FinYear", 10, FinYear);
                proc.AddVarcharPara("@SearchKey", 100, SearchKey);
                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows
                            select new ProductSalesReturnModel()
                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])



                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesProductForQuotation(string SearchKey, string InventoryType)
        {
            List<ProductSalesQuotation> ProductList = new List<ProductSalesQuotation>();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopup");
            proc.AddVarcharPara("@InventoryType", 2, InventoryType);
            proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
            DataTable Producttbl = proc.GetTable();

            ProductList = (from DataRow dr in Producttbl.Rows
                           select new ProductSalesQuotation()
                           {
                               Products_ID = Convert.ToString(dr["Products_ID"]),
                               Products_Name = Convert.ToString(dr["Products_Name"]),
                               isInventory = Convert.ToString(dr["IsInventory"]),
                               //Products_Desc = Convert.ToString(dr["Products_Description"]),
                               // Mantis Issue 24375
                               Products_Desc = Convert.ToString(dr["Products_Description"]),
                               // End of Rev Sacnhita
                               HSNSAC = Convert.ToString(dr["HSNSAC"]),
                               Class = Convert.ToString(dr["ClassCode"]),
                               Brand = Convert.ToString(dr["BrandName"]),
                           }).ToList();

            return ProductList;
        }

        // Rev Mantis Issue 24567
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductForPurchaseQuotation(string SearchKey, string InventoryType, string CustomerId)
        {
            List<ProductSalesQuotation> ProductList = new List<ProductSalesQuotation>();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 50, "ProductDetailsHTMLPopup");
            proc.AddVarcharPara("@InventoryType", 2, InventoryType);
            proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
            proc.AddVarcharPara("@customerid", 4000, CustomerId);

            DataTable Producttbl = proc.GetTable();

            ProductList = (from DataRow dr in Producttbl.Rows
                           select new ProductSalesQuotation()
                           {
                               Products_ID = Convert.ToString(dr["Products_ID"]),
                               Products_Name = Convert.ToString(dr["Products_Name"]),
                               isInventory = Convert.ToString(dr["IsInventory"]),
                               //Products_Desc = Convert.ToString(dr["Products_Description"]),
                               // Mantis Issue 24375
                               Products_Desc = Convert.ToString(dr["Products_Description"]),
                               // End of Rev Sacnhita
                               HSNSAC = Convert.ToString(dr["HSNSAC"]),
                               Class = Convert.ToString(dr["ClassCode"]),
                               Brand = Convert.ToString(dr["BrandName"]),

                           }).ToList();

            return ProductList;
        }
        // End of Rev Mantis Issue 24567

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccountVendorDrCrNote(string SearchKey, string branchId)
        {
            List<MainAccountVendorDrCrNote> listMainAccount = new List<MainAccountVendorDrCrNote>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                //ProcedureExecute proc = new ProcedureExecute("sp_FetchVendorDrCrNoteMainAccountHTML");
                //proc.AddVarcharPara("@CompanyID", 10, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //proc.AddIntegerPara("@BranchID", Convert.ToInt32(branchId));
                //proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
                //DataTable mainaccnttbl = proc.GetTable();

                DataTable mainaccnttbl = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_ReverseApplicable,isnull(TAXable,'') TaxAble  from v_MainAccountList where MainAccount_Name like '%" + SearchKey + "%' and (MainAccount_branchId=0 or MainAccount_branchId='" + branchId + "')");


                listMainAccount = (from DataRow dr in mainaccnttbl.Rows
                                   select new MainAccountVendorDrCrNote()
                                   {
                                       MainAccount_ReferenceID = Convert.ToInt32(dr["MainAccount_ReferenceID"]),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),

                                       Subledger_Type = Convert.ToString(dr["MainAccount_SubLedgerType"]),

                                       TaxAble = Convert.ToString(dr["TaxAble"])
                                   }).ToList();
            }

            return listMainAccount;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSubAccountVendorDrCrNote(string SearchKey, string MainAccountCode)
        {
            List<SubAccount> listSubAccount = new List<SubAccount>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                ProcedureExecute proc = new ProcedureExecute("sp_FetchVendorDrCrNoteSubAccountByMainAccountHTML");
                proc.AddIntegerPara("@MainAccountID", Convert.ToInt32(MainAccountCode));
                proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
                DataTable Subaccnttbl = proc.GetTable();

                listSubAccount = (from DataRow dr in Subaccnttbl.Rows
                                  select new SubAccount()
                                  {
                                      SubAccount_ReferenceID = Convert.ToString(dr["SubAccount_ReferenceID"]),
                                      Contact_Name = dr["Contact_Name"].ToString(),
                                      MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"])

                                  }).ToList();
            }

            return listSubAccount;
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
        public object GetOpeningStockDetails(string ProductID, string BranchID)
        {
            List<OpeningStock> openingStock = new List<OpeningStock>();
            OpeningStock returnitem = new OpeningStock();

            #region Product Stock

            List<ProductStockDetails> StockList = new List<ProductStockDetails>();
            ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
            proc.AddVarcharPara("@Action", 50, "GetStockDetailsService");
            proc.AddVarcharPara("@ProductID", 4000, ProductID);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataTable Producttbl = proc.GetTable();

            if (Producttbl != null)
            {
                StockList = (from DataRow dr in Producttbl.Rows
                             select new ProductStockDetails()
                             {
                                 Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]),
                                 SrlNo = Convert.ToString(dr["SrlNo"]),
                                 WarehouseID = Convert.ToString(dr["WarehouseID"]),
                                 WarehouseName = Convert.ToString(dr["WarehouseName"]),
                                 Quantity = Convert.ToString(dr["Quantity"]),
                                 SalesQuantity = Convert.ToString(dr["SalesQuantity"]),
                                 Batch = Convert.ToString(dr["BatchNo"]),
                                 MfgDate = Convert.ToString(dr["MfgDate"]),
                                 ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                                 Rate = Convert.ToString(dr["Rate"]),
                                 SerialNo = Convert.ToString(dr["SerialNo"]),
                                 Barcode = Convert.ToString(dr["Barcode"]),
                                 ViewBatch = Convert.ToString(dr["BatchNo"]),
                                 ViewMfgDate = Convert.ToString(dr["MfgDate"]),
                                 ViewExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                                 ViewRate = Convert.ToString(dr["Rate"]),
                                 IsOutStatus = Convert.ToString(dr["IsOutStatus"]),
                                 IsOutStatusMsg = Convert.ToString(dr["IsOutStatusMsg"]),
                                 LoopID = Convert.ToString(dr["LoopID"]),
                                 Status = Convert.ToString(dr["Status"]),
                                 Value = Convert.ToString(dr["Stock_OpeningValue"]),
                                 AlterQty = Convert.ToString(dr["StockBranchWarehouse_AltStockIn"]),
                                 AltUOM = Convert.ToString(dr["UOM"])
                             }).ToList();

                returnitem.ProductStockDetails = StockList;
            }

            #endregion

            #region Product Stock Rate & Stock Block

            List<WarehouseRate> RateList = new List<WarehouseRate>();
            List<StockBlock> StockBlock = new List<StockBlock>();

            ProcedureExecute _proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
            _proc.AddVarcharPara("@Action", 50, "GetWarehouseRate");
            _proc.AddVarcharPara("@ProductID", 4000, ProductID);
            _proc.AddVarcharPara("@BranchID", 4000, BranchID);
            _proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            _proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet tblSet = _proc.GetDataSet();

            DataTable _Producttbl = tblSet.Tables[0];
            DataTable _Blocktbl = tblSet.Tables[1];

            if (_Producttbl != null)
            {
                RateList = (from DataRow dr in _Producttbl.Rows
                            select new WarehouseRate()
                            {
                                WarehouseID = Convert.ToString(dr["WarehouseID"]),
                                Rate = Convert.ToDecimal(dr["OpeningRate"])
                            }).ToList();
            }

            if (_Blocktbl != null)
            {
                StockBlock = (from DataRow dr in _Blocktbl.Rows
                              select new StockBlock()
                              {
                                  IsStockBlock = Convert.ToString(dr["IsStockBlock"]),
                                  AvailableQty = Convert.ToDecimal(dr["AvailableQty"])
                              }).ToList();
            }

            returnitem.WarehouseRate = RateList;
            returnitem.StockBlock = StockBlock;

            #endregion

            return returnitem;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProjectOpeningStockDetails(string ProductID, string BranchID)
        {
            List<OpeningStock> openingStock = new List<OpeningStock>();
            ProjectOpeningStock returnitem = new ProjectOpeningStock();

            #region Product Stock

            List<ProjectProductStockDetails> ProStockList = new List<ProjectProductStockDetails>();
            ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
            proc.AddVarcharPara("@Action", 50, "GetProjectStockDetailsService");
            proc.AddVarcharPara("@ProductID", 4000, ProductID);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataTable Producttbl = proc.GetTable();

            if (Producttbl != null)
            {
                ProStockList = (from DataRow dr in Producttbl.Rows
                                select new ProjectProductStockDetails()
                                {
                                    Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]),
                                    SrlNo = Convert.ToString(dr["SrlNo"]),
                                    WarehouseID = Convert.ToString(dr["WarehouseID"]),
                                    WarehouseName = Convert.ToString(dr["WarehouseName"]),
                                    Quantity = Convert.ToString(dr["Quantity"]),
                                    SalesQuantity = Convert.ToString(dr["SalesQuantity"]),
                                    Batch = Convert.ToString(dr["BatchNo"]),
                                    MfgDate = Convert.ToString(dr["MfgDate"]),
                                    ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                                    Rate = Convert.ToString(dr["Rate"]),
                                    SerialNo = Convert.ToString(dr["SerialNo"]),
                                    Barcode = Convert.ToString(dr["Barcode"]),
                                    ViewBatch = Convert.ToString(dr["BatchNo"]),
                                    ViewMfgDate = Convert.ToString(dr["MfgDate"]),
                                    ViewExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                                    ViewRate = Convert.ToString(dr["Rate"]),
                                    IsOutStatus = Convert.ToString(dr["IsOutStatus"]),
                                    IsOutStatusMsg = Convert.ToString(dr["IsOutStatusMsg"]),
                                    LoopID = Convert.ToString(dr["LoopID"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    Value = Convert.ToString(dr["Stock_OpeningValue"]),
                                    AlterQty = Convert.ToString(dr["StockBranchWarehouse_AltStockIn"]),
                                    AltUOM = Convert.ToString(dr["UOM"]),
                                    ProjectID = Convert.ToString(dr["Project_ID"]),
                                    ProjectCode = Convert.ToString(dr["ProjectCode"]),
                                    HierarchyID = Convert.ToString(dr["HierarchyID"]),
                                    Hierarchy = Convert.ToString(dr["Hierarchy"])

                                }).ToList();

                returnitem.ProjectProductStockDetails = ProStockList;
            }

            #endregion

            #region Product Stock Rate & Stock Block

            List<WarehouseRate> RateList = new List<WarehouseRate>();
            List<StockBlock> StockBlock = new List<StockBlock>();

            ProcedureExecute _proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
            _proc.AddVarcharPara("@Action", 50, "GetWarehouseRate");
            _proc.AddVarcharPara("@ProductID", 4000, ProductID);
            _proc.AddVarcharPara("@BranchID", 4000, BranchID);
            _proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            _proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet tblSet = _proc.GetDataSet();

            DataTable _Producttbl = tblSet.Tables[0];
            DataTable _Blocktbl = tblSet.Tables[1];

            if (_Producttbl != null)
            {
                RateList = (from DataRow dr in _Producttbl.Rows
                            select new WarehouseRate()
                            {
                                WarehouseID = Convert.ToString(dr["WarehouseID"]),
                                Rate = Convert.ToDecimal(dr["OpeningRate"])
                            }).ToList();
            }

            if (_Blocktbl != null)
            {
                StockBlock = (from DataRow dr in _Blocktbl.Rows
                              select new StockBlock()
                              {
                                  IsStockBlock = Convert.ToString(dr["IsStockBlock"]),
                                  AvailableQty = Convert.ToDecimal(dr["AvailableQty"])
                              }).ToList();
            }

            returnitem.WarehouseRate = RateList;
            returnitem.StockBlock = StockBlock;

            #endregion

            return returnitem;
        }


        public class ProjectProductStockDetails
        {
            public string Product_SrlNo { get; set; }
            public string SrlNo { get; set; }
            public string WarehouseID { get; set; }
            public string WarehouseName { get; set; }
            public string Quantity { get; set; }
            public string SalesQuantity { get; set; }
            public string Batch { get; set; }
            public string MfgDate { get; set; }
            public string ExpiryDate { get; set; }
            public string Rate { get; set; }
            public string SerialNo { get; set; }
            public string Barcode { get; set; }
            public string ViewBatch { get; set; }
            public string ViewMfgDate { get; set; }
            public string ViewExpiryDate { get; set; }
            public string ViewRate { get; set; }
            public string IsOutStatus { get; set; }
            public string IsOutStatusMsg { get; set; }
            public string LoopID { get; set; }
            public string Status { get; set; }
            public string Value { get; set; }
            public string AltQuantity { get; set; }
            public string UOM { get; set; }
            public string AltUOM { get; set; }
            public string AlterQty { get; set; }

            public string ProjectID { get; set; }
            public string ProjectCode { get; set; }
            public string HierarchyID { get; set; }
            public string Hierarchy { get; set; }


        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetStockJournalProduct(string SearchKey, string InventoryType)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_stockjournal_ProductDetails";

                //Both-->B;Inventory Item-->Y;Capital Goods-->C
                Query = Query + " Where IsInventory='Yes'";
                //if (InventoryType == "Y")
                //{
                //    Query = Query + " Where IsInventory='Yes'";
                //}
                //else if (InventoryType == "N")
                //{
                //    Query = Query + " Where IsInventory='No'";
                //}
                //else if (InventoryType == "C")
                //{
                //    Query = Query + " Where IsCapitalGoods='Yes'";
                //}

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {

                                Products_ID = Convert.ToString(dr["sProduct_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),


                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSubAccount(string SearchKey, string MainAccountCode)
        {
            List<SubAccount> listSubAccount = new List<SubAccount>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable("select top 10 Contact_Name,SubAccount_ReferenceID,MainAccount_SubLedgerType  from v_SubAccountList where Contact_Name like '%" + SearchKey + "%'  and mainaccount_referenceid='" + MainAccountCode + "'  order by Contact_Name");


                listSubAccount = (from DataRow dr in cust.Rows
                                  select new SubAccount()
                                  {
                                      SubAccount_ReferenceID = Convert.ToString(dr["SubAccount_ReferenceID"]),
                                      Contact_Name = dr["Contact_Name"].ToString(),
                                      MainAccount_SubLedgerType = Convert.ToString(dr["MainAccount_SubLedgerType"])

                                  }).ToList();
            }

            return listSubAccount;
        }
        //-------------------------------Rev Rajdip--------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUom(string ProductID)
        {
            OpeingUOMDetails objOpeingUOMDetails = new OpeingUOMDetails();
            List<Uom> Uom = new List<Uom>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dtUOM = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM ");
                DataTable dtUOM = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM");

                //DataTable dtUOMId = oDBEngine.GetDataTable("select packing_saleUOM from tbl_master_product_packingDetails " +
                //                                          " where packing_sProductId='" + ProductID + "'");
                DataTable dtUOMId = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_ID in(select packing_saleUOM from tbl_master_product_packingDetails " +
                                                           " where packing_sProductId='" + ProductID + "')");

                //Rev Bapi
                DataTable dtActuallUOMId = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_ID in(select sProduct_stockuom from Master_sProducts " +
                                                          " where sProducts_ID='" + ProductID + "')");
                //End  Rev Bapi

                Uom = (from DataRow dr in dtUOM.Rows
                       select new Uom()
                                  {
                                      UOM_Id = Convert.ToString(dr["UOM_ID"]),
                                      UOM_Name = dr["UOM_Name"].ToString(),
                                  }).ToList();

                if (dtUOMId != null && dtUOMId.Rows.Count > 0)
                {
                    objOpeingUOMDetails.uom_id = Convert.ToString(dtUOMId.Rows[0][0]);
                    objOpeingUOMDetails.uom_name = Convert.ToString(dtUOMId.Rows[0][1]);
                }
                if (dtActuallUOMId != null && dtActuallUOMId.Rows.Count > 0)
                {
                    objOpeingUOMDetails.uomac_id = Convert.ToString(dtActuallUOMId.Rows[0][0]);
                    objOpeingUOMDetails.uomac_name = Convert.ToString(dtActuallUOMId.Rows[0][1]);
                }


            }

            objOpeingUOMDetails.uom = Uom;



            return objOpeingUOMDetails;
        }


        public class OpeingUOMDetails
        {
            public List<Uom> uom { get; set; }
            public string uom_id { get; set; }
            public string uom_name { get; set; }

            public string uomac_id { get; set; }
            public string uomac_name { get; set; }

        }

        //As discussed with pijush da alternate quantity in serial number to be there as it is
        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object IsserialActivate(string ProductID)
        //{
        //    List<Uom> Uom = new List<Uom>();
        //    if (HttpContext.Current.Session["userid"] != null)
        //    {
        //        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //        //DataTable dtUOM = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM ");
        //        DataTable dtUOM = oDBEngine.GetDataTable("select Is_active_serialno from Master_sProducts where sProducts_ID='" + ProductID + "'");

        //        Uom = (from DataRow dr in dtUOM.Rows
        //               select new Uom()
        //               {
        //                   Isserial = Convert.ToString(dr["Is_active_serialno"]),
        //               }).ToList();
        //    }

        //    return Uom;
        //}


        //----------------------------End Rev Rajdip-------------------------------------------

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<WarehouseDetails> GetWarehouse(string Branch)
        {
            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            List<WarehouseDetails> list = new List<WarehouseDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string Query = "select  tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + Branch + "')  order by bui_Name";
                DataTable cust = new DataTable();
                if (multiwarehouse == "1")
                    cust = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + Branch + "'");
                else
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
        public object GetStateListByCountryId(string SearchKey, string countryId)
        {
            List<StateDetails> ProDet = new List<StateDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string query = "select top 10  id as Id,state as StateName,IsNull(StateCode,'') as StateCode from tbl_master_state where countryId='" + countryId + "' and State Like'%" + SearchKey + "%'";
                DataTable dt = oDBEngine.GetDataTable(query);

                ProDet = (from DataRow dr in dt.Rows
                          select new StateDetails()
                          {
                              Id = Convert.ToString(dr["Id"]),
                              StateName = Convert.ToString(dr["StateName"]),
                              StateCode = Convert.ToString(dr["StateCode"])
                          }).ToList();

            }
            return ProDet;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRecipientsBind(string SearchKey)
        {
            List<RecipientsBindModel> listRecpt = new List<RecipientsBindModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable recpt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_EMAILSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(recpt);


                cmd.Dispose();
                con.Dispose();

                listRecpt = (from DataRow dr in recpt.Rows
                             select new RecipientsBindModel()
                             {
                                 ID = dr["ID"].ToString(),
                                 Email = dr["Email"].ToString()

                             }).ToList();
            }

            return listRecpt;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMasterSettings(string Key)
        {
            MasterSettings masterBl = new MasterSettings();

            string value = masterBl.GetSettings(Key);
            return value;
        }





        public class ddlWH
        {
            public string Id { get; set; }
            public string Name { get; set; }


        }
        public class AllddlWH
        {

            public List<ddlWH> ForWareHouse { get; set; }
        }

        public class ddlClass
        {
            public string Id { get; set; }
            public string Name { get; set; }

        }
        public class Allddl
        {
            public List<ddlClass> ForWareHouse { get; set; }
        }

        public class RecipientsBindModel
        {
            public string ID { get; set; }
            public string Email { get; set; }
        }

        public class StateDetails
        {

            public string Id { get; set; }
            public string StateName { get; set; }
            public string StateCode { get; set; }
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
        public class VendorAmountDocument
        {
            public string id { get; set; }
            public string Invoice_Number { get; set; }
            public string Invoice_Date { get; set; }
            public string Amount { get; set; }
            public string UnPaidAmount { get; set; }
        }

        public class Segment1Model
        {
            public string id { get; set; }
            public string Segment1 { get; set; }
            public string SegmentName { get; set; }
        }

        public class ServiceTemplateModel
        {
            public string id { get; set; }
            public string Service_Description { get; set; }
            public string sProducts_Name { get; set; }
        }

        public class EntityModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
        }

        public class EInvErrorLog
        {
            public string DocumentNo { get; set; }
            public string DocumentDate { get; set; }
            public string Errorcode { get; set; }
            public string Errormsg { get; set; }
            public string ErrorType { get; set; }
        }

        public class SalesManAgntModel
        {
            public string id { get; set; }
            public string Na { get; set; }
        }
        public class VendorModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
        }
        public class VendorModelWithBranch
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string GSTIN { get; set; }
        }
        public class SchemaModel
        {
            public string id { get; set; }
            public string Name { get; set; }

        }
        public class VendorModelVPR
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string Type { get; set; }
        }

        #region Transporter Section Start
        public class TransporterModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string type { get; set; }
        }
        #endregion Transporter Section End

        #region Ledger Section for Self Invoice Start
        public class PurchaseLedger
        {
            public string id { get; set; }
            public string Na { get; set; }
        }
        #endregion Ledger Section for Self Invoice End

        public class PosProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string HSN { get; set; }
        }

        public class PosProductModelWithBatch
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string HSN { get; set; }
            public string BatchNo { get; set; }
        }

        public class SalesOrderProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string HSN { get; set; }
            //public string Brand { get; set; }
            //public string IsInsta { get; set; }
            public string Class { get; set; }
        }

        public class SalesOrderProductModelList
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string HSN { get; set; }
            public string IsInventory { get; set; }
            //public string IsInsta { get; set; }
            public string Class { get; set; }
        }
        public class MainAccountJournal
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
        }

        public class MainAccountJournalTDS
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
            public string MainAccount_TDSRate { get; set; }
        }


        public class MainAccountCashBank
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
            public string MainAccount_ReverseApplicable { get; set; }
            public string TaxAble { get; set; }
        }
        public class MainAccountCustomerDbCr
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_SubLedgerType { get; set; }

        }
        //----------------Rev Rajdip--------------
        public class Uom
        {
            public string Isserial { get; set; }
            public string UOM_Id { get; set; }
            public string UOM_Name { get; set; }
        }
        //----------------End Rev Rajdip--------------
        public class SubAccount
        {
            public string SubAccount_ReferenceID { get; set; }
            public string Contact_Name { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
        }
        public class AddressDetails
        {
            public int id { get; set; }
            public string Type { get; set; }
            public string add_address1 { get; set; }
            public string add_address2 { get; set; }
            public string add_address3 { get; set; }
            public int couId { get; set; }
            public string conName { get; set; }
            public int stId { get; set; }
            public string stName { get; set; }
            public int pnId { get; set; }
            public string pnCd { get; set; }
            public int ctyId { get; set; }
            public string ctyName { get; set; }
            public string landMk { get; set; }
            public string GSTIN { get; set; }
            public bool deflt { get; set; }
        }
        public class MainActPaymentDet
        {
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_BankCashType { get; set; }
            public Int64 MainAccount_branchId { get; set; }
        }

        public class Entity
        {
            public string Entity_ID { get; set; }
            public string Entity_Code { get; set; }
            public string EntityName { get; set; }

        }
        public class Technician
        {
            public string ID { get; set; }
            public string Technician_Name { get; set; }
           

        }
        public class ProductGRNModel
        {
            public string Products_ID { get; set; }
            public string Product_Code { get; set; }
            public string Products_Name { get; set; }
            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }
        }

        public class ProductGRNRateModel
        {
            public string Products_ID { get; set; }
            public string Product_Code { get; set; }
            public string Products_Name { get; set; }
            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string GSTRate { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }
        }

        public class ProductWHStockINOUT
        {
            public string Products_ID { get; set; }
            public string Product_Code { get; set; }
            public string Products_Name { get; set; }
            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }
            public string IsReplaceable { get; set; }

        }


        // Code Added By Sam Section Start
        public class PurchaseProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string HSN { get; set; }
        }
        // Code Added By Sam Section End
        public class ProductInvoiceModel
        {
            public string id { get; set; }
            public string Products_Code { get; set; }
            public string Products_Name { get; set; }
            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }

        }

        public class NormalProductInvoiceModel
        {
            public string id { get; set; }
            public string Products_Code { get; set; }
            public string Products_Name { get; set; }
            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string GSTRate { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }

        }
        public class ProductSalesReturnModel
        {
            public string id { get; set; }
            public string Products_Code { get; set; }
            public string Products_Name { get; set; }

            public string IsInventory { get; set; }
            public string HSNSAC { get; set; }
            public string ClassCode { get; set; }
            public string BrandName { get; set; }



        }
        public class ProductSalesQuotation
        {
            public string Products_ID { get; set; }
            public string Products_Name { get; set; }
            // public string Products_Desc { get; set; }
            // Mantis Issue 24375
            public string Products_Desc { get; set; }
            // End of Mantis Issue 24375
            public string isInventory { get; set; }
            public string HSNSAC { get; set; }
            public string Class { get; set; }
            public string Brand { get; set; }
        }
        public class MainAccountVendorDrCrNote
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }

            public string Subledger_Type { get; set; }
            public string TaxAble { get; set; }
        }
        public class OpeningStock
        {
            public List<WarehouseRate> WarehouseRate { get; set; }
            public List<ProductStockDetails> ProductStockDetails { get; set; }
            public List<StockBlock> StockBlock { get; set; }
        }

        public class ProjectOpeningStock
        {
            public List<WarehouseRate> WarehouseRate { get; set; }
            public List<ProjectProductStockDetails> ProjectProductStockDetails { get; set; }
            public List<StockBlock> StockBlock { get; set; }
        }
        public class WarehouseDetails
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        public class bankDetails
        {
            public string BankBranchName { get; set; }
            public string BankBranchAddress { get; set; }
            public string BankBranchLandmark { get; set; }
            public string BankBranchPin { get; set; }
            public string BankBranchAccountNumber { get; set; }
            public string SWIFT { get; set; }
            public string RTGS { get; set; }
            public string IFSC { get; set; }
            public string Remarks { get; set; }

        }
        public class PurINvoiceDetails
        {
            public string InvoiceID { get; set; }
            public string InvoiceNumber { get; set; }
            public string PartyInvoiceNo { get; set; }
            public string PartyInvoiceDate { get; set; }
            public Int64 ProjectId { get; set; }

        }
        public class ProjectDetails
        {
            public Int64 Proj_Id { get; set; }
            public string Proj_Code { get; set; }
            public string Proj_Name { get; set; }

            public string Customer { get; set; }
            public string HIERARCHY_NAME { get; set; }

        }
        //Rev Subhra
        public class NormalProductModel
        {
            public string id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Hsn { get; set; }

        }
        //End of Rev


        public class ServiceContractModel
        {
            public string id { get; set; }
            public string Doc_No { get; set; }
            public string Doc_date { get; set; }
            public string Customer { get; set; }

        }

        #region payroll employee other section
        public class EmployeeDetails
        {
            public string code { get; set; }
            public string desc { get; set; }
        }
        #endregion  payroll employee other section

        #region Code Added by Sam for SelfInvoice Product with Ledger Filter on 08032018 Section Start

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSelfInvoiceProduct(string SearchKey, string InventoryType, string TDSCode, string LedgerCode)
        {
            List<ProductSalesReturnModel> listProd = new List<ProductSalesReturnModel>();

            SearchKey = SearchKey.Replace("'", "''");

            string companyid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
                proc.AddVarcharPara("@Action", 100, "PopulateProductOnDemandforSelfInvoice");
                proc.AddVarcharPara("@filter", 100, SearchKey);
                proc.AddVarcharPara("@IsInventory", 10, InventoryType);
                proc.AddVarcharPara("@SchemeID", 10, TDSCode);
                proc.AddVarcharPara("@LedgerCode", 100, LedgerCode);

                DataTable Producttbl = proc.GetTable();

                listProd = (from DataRow dr in Producttbl.Rows
                            select new ProductSalesReturnModel()
                            {
                                id = Convert.ToString(dr["Products_ID"]),
                                Products_Code = Convert.ToString(dr["Products_Code"]),
                                Products_Name = Convert.ToString(dr["Products_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"])


                            }).ToList();

                return listProd;
            }

            return null;

            // }
        }

        #endregion Code Added by Sam for SelfInvoice Product with Ledger Filter on 08032018 Section Start


        #region Code Added by Arindam for payroll employee other details on 24-10-2018 Section Start

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmployeeOtherDetails(string SearchKey, string Type)
        {
            string rid = string.Empty;
            DataTable dt = new DataTable();
            List<EmployeeDetails> list = new List<EmployeeDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                if (Type == "unit")
                    rid = "PU";
                else if (Type == "dept")
                    rid = "DP";
                else if (Type == "desig")
                    rid = "DS";
                else if (Type == "grade")
                    rid = "LB";

                if (Type == "unit")
                {
                    dt = oDBEngine.GetDataTable(" Select branch_id as CODE,branch_description as [DESC] from tbl_master_Branch");
                }

                else
                {
                    dt = oDBEngine.GetDataTable(" SELECT CODE,[DESC] From proll_Main_Master Where Active='Y' AND [DESC] like '%" + SearchKey + "%' AND RID='" + rid + "' ORDER BY [DESC]");
                }




                list = (from DataRow dr in dt.Rows
                        select new EmployeeDetails()
                        {
                            code = Convert.ToString(dr["CODE"]),
                            desc = Convert.ToString(dr["DESC"]),
                        }).ToList();
            }

            return list;
        }

        #endregion Code Added by Arindam for payroll employee other details on 24-10-2018 Section Start

        #region Customer Pending Delivery Warehouse Section Start

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<StockWarehouseDetails> GetStockWarehouse(string ProductID, string Branch)
        {
            List<StockWarehouseDetails> list = new List<StockWarehouseDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
                // proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["QuotationID"]));
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
                proc.AddVarcharPara("@branchId", 2000, Branch);
                proc.AddVarcharPara("@companyId", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                dt = proc.GetTable();


                list = (from DataRow dr in dt.Rows
                        select new StockWarehouseDetails()
                        {
                            Value = Convert.ToString(dr["WarehouseID"]),
                            Text = Convert.ToString(dr["WarehouseName"])

                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<StockSerialDetails> GetStockSerial(string WarehouseID, string BatchID, string ProductID, string Branch, string Date)
        {
            List<StockSerialDetails> list = new List<StockSerialDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
                //proc.AddVarcharPara("@Action", 500, "GetSerialOnFIFOBasis");
                // proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanIDCDP"]));
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@BatchID", 500, BatchID);
                proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
                proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
                //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
                proc.AddVarcharPara("@branchId", 2000, Branch);
                proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
                proc.AddVarcharPara("@Doc_Type", 500, "SC");
                proc.AddVarcharPara("@SC_Date", 20, Date);
                dt = proc.GetTable();

                list = (from DataRow dr in dt.Rows
                        select new StockSerialDetails()
                        {
                            Value = Convert.ToString(dr["SerialID"]),
                            Text = Convert.ToString(dr["SerialName"])

                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<StockBatchDetails> GetStockBatch(string WarehouseID, string ProductID, string Branch)
        {
            List<StockBatchDetails> list = new List<StockBatchDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
                // proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["QuotationID"]));
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
                proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
                //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
                proc.AddVarcharPara("@branchId", 2000, Branch);
                proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
                dt = proc.GetTable();

                list = (from DataRow dr in dt.Rows
                        select new StockBatchDetails()
                        {
                            Value = Convert.ToString(dr["BatchID"]),
                            Text = Convert.ToString(dr["BatchName"])

                        }).ToList();
            }

            return list;
        }

        public class StockBatchDetails
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        public class StockWarehouseDetails
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        public class StockSerialDetails
        {
            public string Value { get; set; }
            public string Text { get; set; }

        }
        #endregion


        #region Get UOM Details

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMultiUOMDetails(string orderid, string action, string module, string strKey)
        {
            if (String.IsNullOrEmpty(orderid))
            {
                orderid = "0";
            }
            if (String.IsNullOrEmpty(strKey))
            {
                strKey = null;
            }
            string PackingQty = GetMultiUOMDetailsData(orderid, action, module, strKey);

            return PackingQty;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMultiUOMDetailsData(string orderid, string action, string module, string strKey)
        {

            DataTable Orderdt = MultiUOMDetailsData(orderid, action, module, strKey).Tables[0];
            string PackingQty = "";
            for (int i = 0; i < Orderdt.Rows.Count; i++)
            {
                PackingQty = Convert.ToString(Orderdt.Rows[i]["PackingQty"]);
            }

            return PackingQty;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DataSet MultiUOMDetailsData(string orderid, string action, string module, string strKey)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_MultiUOMDetails_Get");
            proc.AddVarcharPara("@ACTION", 500, action);
            proc.AddVarcharPara("@MODULE", 250, module);
            proc.AddVarcharPara("@KEY", 500, strKey);
            proc.AddBigIntegerPara("@ID", Convert.ToInt64(orderid));
            ds = proc.GetDataSet();
            return ds;
        }

        #endregion

        #region Sales Rate Scheme

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesRateSchemePrice(string CustomerID, String ProductID, DateTime PostingDate)
        {
            List<RateScheme> listRate = new List<RateScheme>();

            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_SalesRateSchemePrice");
                proc.AddPara("@ENTITY_CODE", CustomerID);
                proc.AddPara("@PRODUCT_ID", ProductID);
                proc.AddPara("@POSTING_DATE", PostingDate);
                DataTable Producttbl = proc.GetTable();

                listRate = (from DataRow dr in Producttbl.Rows
                            select new RateScheme()
                            {
                                MinSalePrice = Convert.ToDecimal(dr["MinSalePrice"]),
                                MaxSalePrice = Convert.ToDecimal(dr["MaxSalePrice"]),
                                RateType = Convert.ToInt32(dr["RateType"]),
                                MRP = Convert.ToDecimal(dr["MRP"])
                            }).ToList();
                return listRate;
            }
            return null;
        }

        public class RateScheme
        {
            public Decimal MinSalePrice { get; set; }
            public Decimal MaxSalePrice { get; set; }
            public int RateType { get; set; }
            public Decimal MRP { get; set; }
        }

        #endregion

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetServiceProduct(string SearchKey)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Purchase_ProductDetails";

                Query = Query + " Where IsInventory='No' AND IsServiceItem='Yes'";

                Query = Query + ") as tbl Where Product_Code like '%" + SearchKey + "%' OR Product_Name like '%" + SearchKey + "%' Order By len(Product_Code),Product_Code asc";

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new ProductGRNModel()
                            {
                                Products_ID = Convert.ToString(dr["sProduct_ID"]),
                                Product_Code = Convert.ToString(dr["Product_Code"]),
                                Products_Name = Convert.ToString(dr["Product_Name"]),
                                IsInventory = Convert.ToString(dr["IsInventory"]),
                                HSNSAC = Convert.ToString(dr["HSNSAC"]),
                                ClassCode = Convert.ToString(dr["ClassCode"]),
                                BrandName = Convert.ToString(dr["BrandName"]),

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetServiceTempleate(string SearchKey)
        {
            List<ServiceTemplateModel> listCust = new List<ServiceTemplateModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
              //  CustomerIds = CustomerIds.TrimStart(',');
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Prc_ServiceMaterialIssue_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindServiceTemplate");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);              
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();

                //DataTable cust = oDBEngine.GetDataTable(Query);
                listCust = (from DataRow dr in Sigment1.Rows
                            select new ServiceTemplateModel()
                            {
                                id = dr["ServiceTemplate_ID"].ToString(),
                                Service_Description = dr["Service_Description"].ToString(),
                                sProducts_Name = dr["sProducts_Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        #region Get segment without Order Tanmoy
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment1List(string CustomerIds, string order_id=null, string orderdetails_id=null)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceScheduleTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment1List");
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment2List(string Segment1_Id, string CustomerIds, string order_id=null, string orderdetails_id=null)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment1_Id = Segment1_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceScheduleTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment2List");
                cmd.Parameters.AddWithValue("@Segment_Id1", Segment1_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment3List(string Segment2_Id, string CustomerIds, string order_id=null, string orderdetails_id=null)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment2_Id = Segment2_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceScheduleTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment3List");
                cmd.Parameters.AddWithValue("@Segment_Id2", Segment2_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment4List(string Segment3_Id, string CustomerIds, string order_id=null, string orderdetails_id=null)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment3_Id = Segment3_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceScheduleTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment4List");
                cmd.Parameters.AddWithValue("@Segment_Id3", Segment3_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetScheduleSegment5List(string Segment4_Id, string CustomerIds, string order_id=null, string orderdetails_id=null)
        {
            List<Segment1Model> listCust = new List<Segment1Model>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                Segment4_Id = Segment4_Id.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable Sigment1 = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceScheduleTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Segment5List");
                cmd.Parameters.AddWithValue("@Segment_Id4", Segment4_Id);
                cmd.Parameters.AddWithValue("@Customer_id", CustomerIds);
                cmd.Parameters.AddWithValue("@order_id", order_id);
                cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Sigment1);

                cmd.Dispose();
                con.Dispose();


                listCust = (from DataRow dr in Sigment1.Rows
                            select new Segment1Model()
                            {
                                id = dr["Code"].ToString(),
                                Segment1 = dr["Code"].ToString(),
                                SegmentName = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }
        #endregion

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDeliverySchedule(string SearchKey, string ProductID, string OrderID, string OrderDetailsId)
        {
            List<DeliverySchedule> listCust = new List<DeliverySchedule>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //string Query = "Select DeliveryScheduleId,DeliverySchedule_Number,sProducts_Code,DeliverySchedule_DeliveryQty" +
                //    " From (select * from v_DeliveryScheduleDetails where DeliverySchedule_DocID=" + OrderID + "    and DeliverySchedule_DocDetailsID=" + OrderDetailsId + "   and DeliveryScheduleDetails_ProductId=" + ProductID + ")tbl";

                
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();        


                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_DeliveryScheduleDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindDeliveryScheduleList");
                cmd.Parameters.AddWithValue("@DocDetailsID", OrderDetailsId);
                cmd.Parameters.AddWithValue("@DocID", OrderID);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                //cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();
                //DataTable cust = oDBEngine.GetDataTable(Query);

                listCust = (from DataRow dr in cust.Rows
                            select new DeliverySchedule()
                            {
                                DeliverySchedule_Id = Convert.ToString(dr["DeliveryScheduleId"]),
                                DeliverySchedule_Number = Convert.ToString(dr["DeliverySchedule_Number"]),
                                Products_Name = Convert.ToString(dr["sProducts_Code"]),
                                DeliverySchedule_Quantity = Convert.ToString(dr["DeliverySchedule_DeliveryQty"]),
                                SerialNumber = Convert.ToString(dr["SerialNumber"]),
                                DeliverySchedule_DeliveryDate = Convert.ToString(dr["DeliverySchedule_DeliveryDate"]),
                            }).ToList();
            }

            return listCust;
        }

        public class DeliverySchedule
        {
            public string DeliverySchedule_Id { get; set; }
            public string DeliverySchedule_Number { get; set; }
            public string Products_Name { get; set; }
            public string DeliverySchedule_Quantity { get; set; }
            public string SerialNumber { get; set; }
            public string DeliverySchedule_DeliveryDate { get; set; }
        }
    }
}
