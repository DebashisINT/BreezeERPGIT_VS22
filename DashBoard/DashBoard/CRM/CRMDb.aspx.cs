using DashBoard.DashBoard.CRM.UserControl;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.CRM
{
    public partial class CRMDb : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
      
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataTable dt = oDBEngine.GetDataTable(@"select totPhCall,totSv,TotEnt,PendAct,OrdCnt,QuoteCount,efficiencyrat,ActHistory,newVsreapeat
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));

                if (dt.Rows.Count > 0)
                {
                    CallDiv.Visible = Convert.ToBoolean(dt.Rows[0]["totPhCall"]);
                    CallDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["totPhCall"]);

                    SVDiv.Visible = Convert.ToBoolean(dt.Rows[0]["totSv"]);
                    SVDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["totSv"]);

                    totEntDiv.Visible = Convert.ToBoolean(dt.Rows[0]["TotEnt"]);
                    totEntDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["TotEnt"]);

                    pendingActDiv.Visible = Convert.ToBoolean(dt.Rows[0]["PendAct"]);
                    pendingActDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["PendAct"]);

                    OrderCntdiv.Visible = Convert.ToBoolean(dt.Rows[0]["OrdCnt"]);
                    OrderCntdivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["OrdCnt"]);

                    QuoteCountdiv.Visible = Convert.ToBoolean(dt.Rows[0]["QuoteCount"]);
                    QuoteCountdivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["QuoteCount"]);

                    EFDiv.Visible = Convert.ToBoolean(dt.Rows[0]["efficiencyrat"]);
                    EFDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["efficiencyrat"]);

                    AhDiv.Visible = Convert.ToBoolean(dt.Rows[0]["ActHistory"]);
                    AhDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["ActHistory"]);

                    nrDiv.Visible = Convert.ToBoolean(dt.Rows[0]["newVsreapeat"]);
                    nrDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["newVsreapeat"]);
                }
            }

        }

       
    }
}