using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace OpeningEntry.OpeningEntry.OpeningServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class OpeningMaster : System.Web.Services.WebService
    {
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
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

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
                    cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                }
                else
                {
                    cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_VendorTransporterDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
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
        public object GetCustomerOnIndustry(string SearchKey, string CustomerIds)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                CustomerIds = CustomerIds.TrimStart(',');

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string Query = "select top 10 * From(select cnt_internalid ,cnt_id,uniquename ,Name ,Billing   from v_pos_customerDetails";

                if (!string.IsNullOrEmpty(CustomerIds))
                {
                    Query = Query + " Where cnt_id in (" + CustomerIds + ")";
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



                strQuery = @" Select top 10  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename
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
                    strQuery = @"Select top 10  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name,
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
                    strQuery = @" select top 10 cnt_internalid ,uniquename ,Name,'Customer' as Type   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name";
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
        public object GetSalesOrderProduct(string SearchKey, string InventoryType, string ProductIds)
        {
            List<SalesOrderProductModel> listCust = new List<SalesOrderProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                ProductIds = ProductIds.TrimStart(',');
                string Query = "Select Top 10 * From (select Products_ID,Products_Name ,Products_Description ,HSNSAC,BrandName,sProducts_isInstall,ClassCode,ProductId from v_GetProductDetails_SalesOrder";

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
        public object GetPurchaseProduct(string SearchKey, string InventoryType)
        {
            List<ProductGRNModel> listCust = new List<ProductGRNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                string Query = "Select Top 10 * From (select sProduct_ID,Product_Code,Product_Name,IsInventory,HSNSAC,ClassCode,BrandName from v_Purchase_ProductDetails";

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

                string Query = "Select Top 10 Products_ID,Products_Name,Products_Description,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_Product_MargeDetails";

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
                string Query = "Select Top 10 Products_ID,ProductsName,sProducts_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_IndentRequisitionProductList";

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
                string Query = "Select Top 10 Products_ID,ProductsName,Products_Code,ProductsDescription,IsInventory,HSNSAC,ClassCode,BrandName From (select * from v_BranchRequisitionProductList";

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
        public object GetSalesReturnProduct(string SearchKey)
        {
            List<ProductSalesReturnModel> listProd = new List<ProductSalesReturnModel>();

            SearchKey = SearchKey.Replace("'", "''");

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
                               HSNSAC = Convert.ToString(dr["HSNSAC"]),
                               Class = Convert.ToString(dr["ClassCode"]),
                               Brand = Convert.ToString(dr["BrandName"]),
                           }).ToList();

            return ProductList;
        }

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
                                 Status = Convert.ToString(dr["Status"])
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
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<WarehouseDetails> GetWarehouse(string Branch)
        {
            List<WarehouseDetails> list = new List<WarehouseDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string Query = "select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + Branch + "') order by bui_Name";
                DataTable cust = oDBEngine.GetDataTable(Query);

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

        public class EntityModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
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
        public class MainAccountJournal
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
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
            public bool deflt { get; set; }
        }
        public class MainActPaymentDet
        {
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_BankCashType { get; set; }
            public Int64 MainAccount_branchId { get; set; }
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
        public class WarehouseRate
        {
            public string WarehouseID { get; set; }
            public decimal Rate { get; set; }
        }
        public class StockBlock
        {
            public string IsStockBlock { get; set; }
            public decimal AvailableQty { get; set; }
        }
        public class ProductStockDetails
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
        }
        public class OpeningStock
        {
            public List<WarehouseRate> WarehouseRate { get; set; }
            public List<ProductStockDetails> ProductStockDetails { get; set; }
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

               DataTable dt = oDBEngine.GetDataTable(" SELECT CODE,[DESC] From proll_Main_Master Where Active='Y' AND [DESC] like '%" + SearchKey + "%' AND RID='"+rid+"' ORDER BY [DESC]");


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

    }
}

  