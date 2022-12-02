using BusinessLogicLayer;
using CutOff.Views.CutOff.dbml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CutOff.Models
{
    public class JournalModel
    {
        ReadWriteMasterDatabaseBL readwritemasterdatabaseBL = new ReadWriteMasterDatabaseBL();
        public IEnumerable GetJournals(DateTime? cutoffdate)
        {
         
            AccountsDataContainer atc = new AccountsDataContainer(BuildConnectionString(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])));

            //List<Trans_CashBankVouchers> ctx = new List<Trans_CashBankVouchers>();

            var query = (from c in atc.v_YearEnding_Journal
                         //join s in ctx.States
                         //on c.StateId equals s.StateId
                         where c.JournalVoucher_TransactionDate <= cutoffdate
                         select c).ToList();

            return query;
        }
        private String BuildConnectionString(String connString)
        {


            EntityConnectionStringBuilder esb = new EntityConnectionStringBuilder();
            esb.Metadata = "res://*/Views.CutOff.dbml.AccountsData.csdl|res://*/Views.CutOff.dbml.AccountsData.ssdl|res://*/Views.CutOff.dbml.AccountsData.msl";
            esb.Provider = "System.Data.SqlClient";
            esb.ProviderConnectionString = connString;

            // Generate the full string and return it
            return esb.ToString();


        }
    }
}