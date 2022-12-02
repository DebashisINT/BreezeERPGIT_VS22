using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using DataAccessLayer;
using System.Data;

namespace ERP.OMS.Management.Master
{
    public partial class SpecialEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object ValidateGrn(string grnNumber, string grnType)
        {

            List<vlgrn> validatedData = new List<vlgrn>();
            ProcedureExecute proc = new ProcedureExecute("Correction_GETDoc_Details");
            //proc.AddVarcharPara("@Action", 100, "GetvalidatedData");
            proc.AddPara("@Doc_Number", grnNumber);
            proc.AddPara("@Doc_type", grnType);
            DataTable CallData = proc.GetTable();
            var random = new Random();
            validatedData = (from DataRow dr in CallData.Rows
                          select new vlgrn()
                          {
                              Doc_Number = Convert.ToString(dr["Doc_Number"]),
                              Doc_Date = Convert.ToDateTime(dr["Doc_Date"]).ToString("yyyy-MM-dd"),
                              cnt_firstName = Convert.ToString(dr["Entity_name"])
                          }).ToList();
            return validatedData;
        }

        public class vlgrn
        {
            public string Doc_Number { get; set; }
            public string Doc_Date { get; set; }
            public string cnt_firstName { get; set; }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object resetGrnDate(string Grn_Number, string New_Grn_Date)
        {

            ProcedureExecute proc = new ProcedureExecute("Correction_GRNDateChange");

            proc.AddVarcharPara("@Grn_Number", 100, Grn_Number );
            proc.AddVarcharPara("@New_Grn_Date", 100, New_Grn_Date);
            proc.RunActionQuery();
            
            return "1";
        }

        [WebMethod]
        public static object GetCustomer(string SearchKey)
        {
            List<DashBoardCustomerModel> listCust = new List<DashBoardCustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                // DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev
                DataTable cust = oDBEngine.GetDataTable(" select * from (select distinct top 10  pcd.cnt_internalid ,pcd.uniquename ,Replace(pcd.Name,'''','&#39;') as Name ,pcd.Billing+',  '+pcd.phf_phoneNumber as Billing from v_pos_customerDetails pcd LEFT OUTER JOIN tbl_master_phonefax mp on mp.phf_cntId=pcd.cnt_internalid LEFT OUTER JOIN tbl_master_address MA ON MA.add_cntId=pcd.cnt_internalid where pcd.uniquename like '%" + SearchKey + "%' or pcd.Name like '%" + SearchKey + "%' or  mp.phf_phoneNumber like '%" + SearchKey + "%' OR MA.add_phone LIKE '%" + SearchKey + "%' ) as t order by t.Name ");

                listCust = (from DataRow dr in cust.Rows
                            select new DashBoardCustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod]

        public static object GetMainAccountJournal(string SearchKey)
        {
            List<MainAccountJournal> listMainAccount = new List<MainAccountJournal>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable("select top 10 MainAccount_Name,MainAccount_ReferenceID,MainAccount_SubLedgerType,MainAccount_branchId,MainAccount_BankCompany,MainAccount_AccountCode  from v_MainAccountList_journal where (MainAccount_Name like '%" + SearchKey + "%' or MainAccount_AccountCode like '%" + SearchKey + "%') order by Len(MainAccount_Name),MainAccount_Name asc");


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
        [WebMethod]

        public static object MasterCall(string SearchKey, string mainAcc)
        {
            ProcedureExecute proc = new ProcedureExecute("CORRECTION_CHANGE_CUSTOMER_MAINACCOUNT");

            proc.AddVarcharPara("@CUSTOMER_ID", 100, SearchKey);
            proc.AddVarcharPara("@MAIN_ACCOUNT_SHORT_CODE", 100, mainAcc);
            proc.RunActionQuery();

            return "1";
        }
        [WebMethod]
        public static object MasterCallWithVendor(string SearchKey, string mainAcc)
        {
            ProcedureExecute proc = new ProcedureExecute("CORRECTION_CHANGE_VENDOR_MAINACCOUNT");

            proc.AddVarcharPara("@VENDOR_ID", 100, SearchKey);
            proc.AddVarcharPara("@MAIN_ACCOUNT_SHORT_CODE", 100, mainAcc);
            proc.RunActionQuery();

            return "1";
        }

        [WebMethod]
        public static object GetVendorWithBranch(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name
                                  From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV') as tbl " +
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
        [WebMethod]

        public static object MasterDocSet(string DocumentNo, string DocumentDate)
        {
            ProcedureExecute proc = new ProcedureExecute("Correction_UpdatePurchaseInvoiceDocDate");

            proc.AddVarcharPara("@DocumentNo", 100, DocumentNo);
            proc.AddVarcharPara("@DocumentDate", 100, DocumentDate);
            proc.RunActionQuery();

            return "1";
        }
        public class VendorModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
        }
        public class MainAccountJournal
        {
            public int MainAccount_ReferenceID { get; set; }
            public string MainAccount_Name { get; set; }
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_SubLedgerType { get; set; }
        }
        public class DashBoardCustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string add { get; set; }
        }
    }

    
}