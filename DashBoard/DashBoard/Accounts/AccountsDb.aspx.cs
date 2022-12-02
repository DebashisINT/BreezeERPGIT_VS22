using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.Accounts
{
    public partial class AccountsDb : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LocalCurrency"] != null)
                {
                    lblCurrencySymbol.InnerHtml = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                }

                DataTable dt = oDBEngine.GetDataTable(@"select ExpThismonth,TodayBankStat,Incomeexpratio,Receentpay,AccWatchlist
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows.Count > 0)
                    {
                        Expensethismonth.Visible = Convert.ToBoolean(dt.Rows[0]["ExpThismonth"]);
                        bankBalance.Visible = Convert.ToBoolean(dt.Rows[0]["TodayBankStat"]);
                        incomeExpenseChart.Visible = Convert.ToBoolean(dt.Rows[0]["Incomeexpratio"]);
                        RecentPayment.Visible = Convert.ToBoolean(dt.Rows[0]["Receentpay"]);
                        AccountWatchlist.Visible = Convert.ToBoolean(dt.Rows[0]["AccWatchlist"]);

                    }
                }

            }
        }
    }
}