using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataAccessLayer;
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Data.SqlClient;
namespace BusinessLogicLayer
{
    public static class JournalVoucher
    {
        static HttpSessionState Session { get { return HttpContext.Current.Session; } }

        static DataObject DemoData
        {
            get
            {
                const string key = "DemoDataFB1EB35F-86F5-4FFE-BB23-CBAAF1514C49";
                if (Session[key] == null)
                {
                    var obj = new DataObject();
                    obj.FillObj();
                    Session[key] = obj;
                }
                return (DataObject)Session[key];
            }
        }

        public static IEnumerable GetCustomers()
        {
            return DemoData.Customers;
        }

        public static void InsertCustomer(Customer customerInfo)
        {
            DemoData.Customers.Add(customerInfo);
        }

        public static void InsertCustomer(string countryID, string cityID, string withDrawl, string receipt, string narration)
        {
            var c = new Customer()
            {
                CustomerID = DemoData.Customers.Count.ToString(),
                MainAccount1 = countryID,
                SubAccount1 = cityID,
                WithDrawl = withDrawl,
                Receipt = receipt,
                Narration = narration
            };

            DemoData.Customers.Add(c);
        }

        public static void UpdateCustomer(Customer customerInfo)
        {
            var c = DemoData.Customers.First(i => i.CustomerID == customerInfo.CustomerID);

            c.MainAccount1 = customerInfo.MainAccount1;
            c.SubAccount1 = customerInfo.SubAccount1;
            c.WithDrawl = customerInfo.WithDrawl;
            c.Receipt = customerInfo.Receipt;
            c.Narration = customerInfo.Narration;
        }

        public static void UpdateCustomer(string customerID, string countryID, string cityID, string withDrawl, string receipt, string narration)
        {
            var c = DemoData.Customers.First(i => i.CustomerID == customerID);

            c.MainAccount1 = countryID;
            c.SubAccount1 = cityID;
            c.WithDrawl = withDrawl;
            c.Narration = narration;
        }

        public static void DeleteCustomer(string customerID)
        {
            var c = DemoData.Customers.First(i => i.CustomerID == customerID);
            DemoData.Customers.Remove(c);
        }

        public static IEnumerable GetMainAccount()
        {
            return DemoData.MainAccounts;
        }

        public static IEnumerable GetSubAccount()
        {
            return DemoData.SubAccounts;
        }

        public static IEnumerable GetSubAccount(string ProcedureSubName, string[] InputSubName, string[] InputSubType, string[] InputSubValue)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            
            DataTable DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);

            List<SubAccount> SubAccounts = new List<SubAccount>();

            //read data from DataTable 
            //using lamdaexpression


            var emp = (from DataRow row in DT.Rows

                       select new SubAccount
                       {
                           CityID = row[1].ToString(),
                           CityName = row[0].ToString()

                       }).ToList();

            return emp;
        }
    }

    public class DataObject
    {
        public List<Customer> Customers { get; set; }
        public List<MainAccount> MainAccounts { get; set; }
        public List<SubAccount> SubAccounts { get; set; }

        public static DataTable GetMainAccount()
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC~'+MainAccount_AccountType as CountryID ", null);

            return DT;
        }
        public static DataTable GetSubAccount()
        {
           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            
            DataTable DT = objEngine.GetDataTable("tbl_master_state", " id as ID, state as State ", null);

            return DT;
        }
        public void FillObj()
        {
            Customers = new List<Customer>();
            MainAccounts = new List<MainAccount>();
            SubAccounts = new List<SubAccount>();

            var myEnumerable = GetMainAccount().AsEnumerable();
            DataTable dt = GetSubAccount();

            MainAccounts =
                (from item in myEnumerable
                 select new MainAccount
                 {
                     CountryName = item.Field<string>("CountryName"),
                     CountryID = item.Field<string>("CountryID")
                 }).ToList();


            SubAccounts = new List<SubAccount>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                SubAccount model;
                foreach (DataRow row in dt.Rows)
                {
                    model = new SubAccount();
                    model.CityID = row["ID"].ToString();
                    model.CityName = row["State"].ToString();
                    //model.CountryID = Convert.ToInt32(row["Country"].ToString());

                    SubAccounts.Add(model);
                }
            }
        }
    }
    public class Customer
    {
        public string CustomerID { get; set; }
        public string MainAccount1 { get; set; }
        public string SubAccount1 { get; set; }
        public string WithDrawl { get; set; }
        public string Receipt { get; set; }
        public string Narration { get; set; }
    }
    public class MainAccount
    {
        public string CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class SubAccount
    {
        public string CityID { get; set; }
        public string CityName { get; set; }
    }
}
