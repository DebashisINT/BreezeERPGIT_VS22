using System;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_SalesVisitOutCome : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            showCallOutcomes();
        }
        public void showCallOutcomes()
        {
            string SelectedAddTime = GetTime(oDBEngine.GetDate().AddMinutes(60).ToShortTimeString().ToString());
            string SelectedNowTime = GetTime(oDBEngine.GetDate().ToShortTimeString().ToString());
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_Master_SalesVisitOutcomeCategory INNER JOIN  tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category", "CAST(tbl_master_SalesVisitOutCome.slv_Id AS NVARCHAR) + '!' + CAST(tbl_Master_SalesVisitOutcomeCategory.Int_id AS NVARCHAR) AS Id,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome", " tbl_master_SalesVisitOutCome.slv_Category not in (9,10,11,12,13) and tbl_master_salesvisitoutcome.slv_id <> 9");
            if (dt.Rows.Count != 0)
            {
                Response.Write("<table width='100%'>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 3 == 0)
                    {
                        if (i != 0)
                        {
                            Response.Write("</tr>");
                        }
                        Response.Write("<tr style='background-color:#DDECFE'>");
                    }
                    Response.Write("<td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:10px; width:300px'><input type='checkbox' id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["slv_SalesVisitOutcome"].ToString() + "' onclick= javascript:checkevent('" + Request.QueryString["obj"].ToString() + "',this,'txtNextVisitDate1','txtNextVisitTime','" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "','" + SelectedAddTime + "','" + oDBEngine.GetDate().ToShortDateString().ToString() + "','" + SelectedNowTime + "')>" + dt.Rows[i]["slv_SalesVisitOutcome"].ToString() + "</td>");
                }
                Response.Write("</tr></table>");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_Master_SalesVisitOutcomeCategory INNER JOIN  tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category", "CAST(tbl_master_SalesVisitOutCome.slv_Id AS NVARCHAR) + '!' + CAST(tbl_Master_SalesVisitOutcomeCategory.Int_id AS NVARCHAR) AS Id,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome", "tbl_master_SalesVisitOutCome.slv_Category in (9,10,11,12,13) and tbl_master_salesvisitoutcome.slv_id <> 9");
                if (dt.Rows.Count != 0)
                {
                    Response.Write("<table width='100%'>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i % 3 == 0)
                        {
                            if (i != 0)
                            {
                                Response.Write("</tr>");
                            }
                            Response.Write("<tr style='background-color:#DDECFE'>");
                        }
                        Response.Write("<td style='border:1px blue; width:300px'><input type='checkbox' id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["slv_SalesVisitOutcome"].ToString() + "' onclick= Javascript:checkevent('" + Request.QueryString["obj"].ToString() + "',this,'txtNextVisitDate1','txtNextVisitTime','" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "','" + SelectedAddTime + "','" + oDBEngine.GetDate().ToShortDateString().ToString() + "','" + SelectedNowTime + "')>" + dt.Rows[i]["slv_SalesVisitOutcome"].ToString() + "</td>");
                    }
                    Response.Write("</tr></table>");
                }
            }
        }
        public string GetTime(string time)
        {
            int t = time.LastIndexOf(":");
            int t1 = Convert.ToInt32(time.Substring(0, t));
            string t2 = time.Substring(t + 1, 2);
            string t3 = time.Substring(t + 3);
            if (t3 == " PM")
            {
                t1 += 12;
            }

            return t1 + ":" + t2;
        }
    }
}