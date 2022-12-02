using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.CRMdash
{
    public partial class CRMdash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static object GetReportsLink()
        {
            QuickMenuLink objQuickMenuLink = new QuickMenuLink();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_crmdashboard", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@action", "totalshow");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            if (dsInst != null && dsInst.Rows.Count > 0)
            {
                objQuickMenuLink.TotalLeads = Convert.ToString(dsInst.Rows[0][0]);
                objQuickMenuLink.openL = Convert.ToString(dsInst.Rows[0][1]);
                objQuickMenuLink.QualifiedL = Convert.ToString(dsInst.Rows[0][2]);
                objQuickMenuLink.LostL = Convert.ToString(dsInst.Rows[0][3]);

            }

            return objQuickMenuLink;
        }


        public class QuickMenuLink
        {
            public string TotalLeads { get; set; }
            public string QualifiedL { get; set; }
            public string openL { get; set; }
            public string LostL { get; set; }
        }
        // campaign cost
        [WebMethod]
        public static object GetCampaignCost()
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_crmdashboard", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@action", "campaigncost");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<campaignClass> objcampaignClass = new List<campaignClass>();
            objcampaignClass = (from DataRow dr in dsInst.Rows
                                select new campaignClass()
                                {
                                    CampaignName = Convert.ToString(dr["CAMPAIGn_name"]),
                                    CampaignCost = Convert.ToString(dr["total_cost"])
                                }).ToList();
            return objcampaignClass;
        }

        [WebMethod]
        public static object getPageloadPermSS()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            boxValues cls = new boxValues();

            ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
            proc.AddPara("@SETTINGS_NAME", "Customer Relationship Management");
            proc.AddPara("@user_id", Convert.ToString(HttpContext.Current.Session["userid"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                cls.DivActivities = Convert.ToBoolean(dt.Rows[0]["CRMActivities"]);
                cls.DivCampaigns = Convert.ToBoolean(dt.Rows[0]["CRMCampaigns"]);
                cls.DivLeads = Convert.ToBoolean(dt.Rows[0]["CRMLeads"]);
                cls.DivOpportunities = Convert.ToBoolean(dt.Rows[0]["CRMOpportunities"]);
                cls.DivAccounts = Convert.ToBoolean(dt.Rows[0]["CRMAccounts"]);
                cls.DivContacts = Convert.ToBoolean(dt.Rows[0]["CRMContacts"]);
                cls.DivCases = Convert.ToBoolean(dt.Rows[0]["CRMCases"]);
                cls.DivServices = Convert.ToBoolean(dt.Rows[0]["CRMServices"]);
                cls.DivCollateral = Convert.ToBoolean(dt.Rows[0]["CRMCollateral"]);

                cls.fTab = Convert.ToBoolean(dt.Rows[0]["CRMActivities"]);
                cls.STab = Convert.ToBoolean(dt.Rows[0]["CRMCampaigns"]);
                cls.tTab = Convert.ToBoolean(dt.Rows[0]["CRMLeads"]);
                cls.frTab = Convert.ToBoolean(dt.Rows[0]["CRMOpportunities"]);
                cls.fvTab = Convert.ToBoolean(dt.Rows[0]["CRMAccounts"]);
                cls.sxTab = Convert.ToBoolean(dt.Rows[0]["CRMContacts"]);
                cls.seTab = Convert.ToBoolean(dt.Rows[0]["CRMCases"]);
                cls.etTab = Convert.ToBoolean(dt.Rows[0]["CRMServices"]);
                cls.enTab = Convert.ToBoolean(dt.Rows[0]["CRMCollateral"]);
                    
            }
            else
            {
                cls.DivActivities = Convert.ToBoolean(0);
                cls.DivCampaigns = Convert.ToBoolean(0);
                cls.DivLeads = Convert.ToBoolean(0);
                cls.DivOpportunities = Convert.ToBoolean(0);
                cls.DivAccounts = Convert.ToBoolean(0);
                cls.DivContacts = Convert.ToBoolean(0);
                cls.DivCases = Convert.ToBoolean(0);
                cls.DivServices = Convert.ToBoolean(0);
                cls.DivCollateral = Convert.ToBoolean(0);

                cls.fTab = Convert.ToBoolean(0);
                cls.STab = Convert.ToBoolean(0);
                cls.tTab = Convert.ToBoolean(0);
                cls.frTab = Convert.ToBoolean(0);
                cls.fvTab = Convert.ToBoolean(0);
                cls.sxTab = Convert.ToBoolean(0);
                cls.seTab = Convert.ToBoolean(0);
                cls.etTab = Convert.ToBoolean(0);
                cls.enTab = Convert.ToBoolean(0);
            }
        
           return cls;
      }
        
    public class boxValues
    {
        public bool DivActivities { get; set; }
        public bool DivCampaigns { get; set; }
        public bool DivLeads { get; set; }
        public bool DivOpportunities { get; set; }
        public bool DivAccounts { get; set; }
        public bool DivContacts { get; set; }
        public bool DivCases { get; set; }
        public bool DivServices { get; set; }
        public bool DivCollateral { get; set; }
        public bool fTab { get; set; }
        public bool  STab { get; set; }
        public bool tTab { get; set; }
        public bool frTab { get; set; }
        public bool fvTab { get; set; }
        public bool sxTab { get; set; }
        public bool seTab { get; set; }
        public bool etTab { get; set; }
        public bool enTab { get; set; }
        public bool nrDiv { get; set; }
        public bool nrDivbtn { get; set; }
    }

        public class campaignClass
        {
            public string CampaignName { get; set; }
            public string CampaignCost { get; set; }

        }
        [WebMethod]
        public static object LeadbyCampaign()
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_crmdashboard", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@action", "campaignleadcount");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<campaignLeadCountClass> objcampaignLeadCountClass = new List<campaignLeadCountClass>();
            objcampaignLeadCountClass = (from DataRow dr in dsInst.Rows
                                         select new campaignLeadCountClass()
                                         {
                                             Campaign_Nam = Convert.ToString(dr["Campaign_Name"]),
                                             Campaigncnt = Convert.ToString(dr["cnt"])
                                         }).ToList();
            return objcampaignLeadCountClass;
        }
        public class campaignLeadCountClass
        {
            public string Campaign_Nam { get; set; }
            public string Campaigncnt { get; set; }

        }

        [WebMethod]
        public static object GetLeadbyIndustry()
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_crmdashboard", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@action", "CampaignLeadCountByIndustry");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<leadbyIndustryClass> objleadbyIndustryClass = new List<leadbyIndustryClass>();
            objleadbyIndustryClass = (from DataRow dr in dsInst.Rows
                                      select new leadbyIndustryClass()
                                      {
                                          IndutryName = Convert.ToString(dr["ind_industry"]),
                                          IndutryCost = Convert.ToString(dr["cnt"])
                                      }).ToList();
            return objleadbyIndustryClass;
        }


        public class leadbyIndustryClass
        {
            public string IndutryName { get; set; }
            public string IndutryCost { get; set; }

        }
        [WebMethod]
        public static object getOpportunity()
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_crmdashboard", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@action", "OpportunityRatingwise");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<opportunityClass> objopportunityClass = new List<opportunityClass>();
            objopportunityClass = (from DataRow dr in dsInst.Rows
                                   select new opportunityClass()
                                   {
                                       ratingName = Convert.ToString(dr["op_rating"]),
                                       opportunityCount = Convert.ToString(dr["cnt"])
                                   }).ToList();
            return objopportunityClass;
        }


        public class opportunityClass
        {
            public string ratingName { get; set; }
            public string opportunityCount { get; set; }

        }

        [WebMethod]
        public static object getSettings()
        {
            CRMDashboardSettings cls = new CRMDashboardSettings();

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
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

        public class CRMDashboardSettings
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
}