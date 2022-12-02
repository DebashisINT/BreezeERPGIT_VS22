using System;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_SalesOutCome1 : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowCallOutComes();
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
        public void ShowCallOutComes()
        {
            string SelectedAddTime = GetTime(oDBEngine.GetDate().AddMinutes(60).ToShortTimeString().ToString());
            string SelectedNowTime = GetTime(oDBEngine.GetDate().ToShortTimeString().ToString());
            DataTable dt = new DataTable();

            if (Request.QueryString["call"].ToString() == "sales")
            {
                dt = oDBEngine.GetDataTable("tbl_Master_SalesVisitOutcomeCategory INNER JOIN  tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category", "CAST(tbl_master_SalesVisitOutCome.slv_Id AS NVARCHAR) + '!' + CAST(tbl_Master_SalesVisitOutcomeCategory.Int_id AS NVARCHAR) AS Id,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome", null);
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
            }
            else
                if (Request.QueryString["call"].ToString() == "phonecall")
                {
                    dt = oDBEngine.GetDataTable("tbl_master_calldispositions", "CAST(call_id AS NVARCHAR) + '|' + CAST(ISNULL(Call_Category, 0) AS NVARCHAR) AS Id,call_dispositions", " call_id<>11");
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
                            Response.Write("<td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:10px; width:300px'><input type='checkbox' id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["call_dispositions"].ToString() + "' onclick= javascript:checkevent('" + Request.QueryString["obj"].ToString() + "',this,'txtNextVisitDate1','txtNextVisitTime','" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "','" + SelectedAddTime + "','" + oDBEngine.GetDate().ToShortDateString().ToString() + "','" + SelectedNowTime + "')>" + dt.Rows[i]["call_dispositions"].ToString() + "</td>");
                        }
                        Response.Write("</tr></table>");
                    }
                }
                else
                {
                    if (Request.QueryString["call"].ToString() == "salesvisit")
                    {
                        dt = oDBEngine.GetDataTable("tbl_Master_SalesVisitOutcomeCategory INNER JOIN  tbl_master_SalesVisitOutCome ON tbl_Master_SalesVisitOutcomeCategory.Int_id = tbl_master_SalesVisitOutCome.slv_Category", "CAST(tbl_master_SalesVisitOutCome.slv_Id AS NVARCHAR) + '!' + CAST(tbl_Master_SalesVisitOutcomeCategory.Int_id AS NVARCHAR) AS Id,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome", null);
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
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("tbl_master_CourtesyCallFeedback", "select ccf_id as Id,ccf_feedback as FeedBack", null);
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
                                Response.Write("<td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:10px; width:300px'><input type='checkbox' id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["slv_SalesVisitOutcome"].ToString() + "' onclick= javascript:checkevent('" + Request.QueryString["obj"].ToString() + "',this,'txtNextVisitDate1','txtNextVisitTime','" + oDBEngine.GetDate().AddDays(1).ToShortDateString().ToString() + "','" + SelectedAddTime + "','" + oDBEngine.GetDate().ToShortDateString().ToString() + "','" + SelectedNowTime + "')>" + dt.Rows[i]["FeedBack"].ToString() + "</td>");
                            }
                            Response.Write("</tr></table>");
                        }
                    }

                }
        }
    }
}