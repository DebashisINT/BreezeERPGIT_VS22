using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.accountDB
{
    public partial class AccountDB : System.Web.UI.Page
    {
         BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    [WebMethod]
    public static object getPageloadPerm()
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            boxValues cls = new boxValues();

                
                DataTable dt = oDBEngine.GetDataTable(@"select ExpThismonth,TodayBankStat,Incomeexpratio,Receentpay,AccWatchlist
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(HttpContext.Current.Session["userid"]));

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows.Count > 0)
                    {
                        cls.Expensethismonth = Convert.ToBoolean(dt.Rows[0]["ExpThismonth"]);
                        cls.bankBalance = Convert.ToBoolean(dt.Rows[0]["TodayBankStat"]);
                        cls.incomeExpenseChart = Convert.ToBoolean(dt.Rows[0]["Incomeexpratio"]);
                        cls.RecentPayment = Convert.ToBoolean(dt.Rows[0]["Receentpay"]);
                        cls.AccountWatchlist = Convert.ToBoolean(dt.Rows[0]["AccWatchlist"]);

                    }
                }

            return cls;
            }
        
    }
    public class boxValues
    {
        public bool Expensethismonth { get; set; }
        public bool bankBalance { get; set; }
        public bool incomeExpenseChart { get; set; }
        public bool RecentPayment { get; set; }
        public bool AccountWatchlist { get; set; }
    }
    
}