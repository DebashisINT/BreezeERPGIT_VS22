using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.crmAnalytic
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object getPageloadPerm()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            boxValues cls = new boxValues();
            DataTable dt = oDBEngine.GetDataTable(@"select totPhCall,totSv,TotEnt,PendAct,OrdCnt,QuoteCount,efficiencyrat,ActHistory,newVsreapeat
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(HttpContext.Current.Session["userid"]));
            if (dt.Rows.Count > 0)
            {
                cls.CallDiv = Convert.ToBoolean(dt.Rows[0]["totPhCall"]);
                cls.CallDivbtn = Convert.ToBoolean(dt.Rows[0]["totPhCall"]);
                cls.SVDiv = Convert.ToBoolean(dt.Rows[0]["totSv"]);
                cls.SVDivbtn = Convert.ToBoolean(dt.Rows[0]["totSv"]);
                cls.totEntDiv = Convert.ToBoolean(dt.Rows[0]["TotEnt"]);
                cls.totEntDivbtn = Convert.ToBoolean(dt.Rows[0]["TotEnt"]);
                cls.pendingActDiv = Convert.ToBoolean(dt.Rows[0]["PendAct"]);
                cls.pendingActDivbtn = Convert.ToBoolean(dt.Rows[0]["PendAct"]);
                cls.OrderCntdiv = Convert.ToBoolean(dt.Rows[0]["OrdCnt"]);
                cls.OrderCntdivbtn = Convert.ToBoolean(dt.Rows[0]["OrdCnt"]);
                cls.QuoteCountdiv = Convert.ToBoolean(dt.Rows[0]["QuoteCount"]);
                cls.QuoteCountdivbtn = Convert.ToBoolean(dt.Rows[0]["QuoteCount"]);
                cls.EFDiv = Convert.ToBoolean(dt.Rows[0]["efficiencyrat"]);
                cls.EFDivbtn = Convert.ToBoolean(dt.Rows[0]["efficiencyrat"]);
                cls.AhDiv = Convert.ToBoolean(dt.Rows[0]["ActHistory"]);
                cls.AhDivbtn = Convert.ToBoolean(dt.Rows[0]["ActHistory"]);
                cls.nrDiv = Convert.ToBoolean(dt.Rows[0]["newVsreapeat"]);
                cls.nrDivbtn = Convert.ToBoolean(dt.Rows[0]["newVsreapeat"]);
            }
                return cls;
           }
        }
    public class boxValues
    {
        public bool CallDiv { get; set; }
        public bool CallDivbtn { get; set; }
        public bool SVDiv { get; set; }
        public bool SVDivbtn { get; set; }
        public bool totEntDiv { get; set; }
        public bool totEntDivbtn { get; set; }
        public bool pendingActDiv { get; set; }
        public bool pendingActDivbtn { get; set; }
        public bool OrderCntdiv { get; set; }
        public bool OrderCntdivbtn { get; set; }
        public bool QuoteCountdiv { get; set; }
        public bool QuoteCountdivbtn { get; set; }
        public bool EFDiv { get; set; }
        public bool EFDivbtn { get; set; }
        public bool AhDiv { get; set; }
        public bool AhDivbtn { get; set; }
        public bool nrDiv { get; set; }
        public bool nrDivbtn { get; set; }
    }
}