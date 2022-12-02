using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace OpeningEntry.ERP.Services
{
    /// <summary>
    /// Summary description for OpeningMaster
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class OpeningMaster : System.Web.Services.WebService
    {

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
    }

    public class MainAccountCashBank
    {
        public int MainAccount_ReferenceID { get; set; }
        public string MainAccount_Name { get; set; }
        public string MainAccount_SubLedgerType { get; set; }
        public string MainAccount_ReverseApplicable { get; set; }
        public string TaxAble { get; set; }
    }
}
