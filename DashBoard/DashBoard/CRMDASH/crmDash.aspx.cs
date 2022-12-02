using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using UtilityLayer;

namespace DashBoard.DashBoard.CRMDASH
{
    public partial class crmDash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "Customer Relationship Management");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DivActivities.Visible = Convert.ToBoolean(dt.Rows[0]["CRMActivities"]);
                    DivCampaigns.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCampaigns"]);
                    DivLeads.Visible = Convert.ToBoolean(dt.Rows[0]["CRMLeads"]);
                    DivOpportunities.Visible = Convert.ToBoolean(dt.Rows[0]["CRMOpportunities"]);
                    DivAccounts.Visible = Convert.ToBoolean(dt.Rows[0]["CRMAccounts"]);
                    DivContacts.Visible = Convert.ToBoolean(dt.Rows[0]["CRMContacts"]);
                    DivCases.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCases"]);
                    DivServices.Visible = Convert.ToBoolean(dt.Rows[0]["CRMServices"]);
                    DivCollateral.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCollateral"]);

                    fTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMActivities"]);
                    STab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCampaigns"]);
                    tTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMLeads"]);
                    frTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMOpportunities"]);
                    fvTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMAccounts"]);
                    sxTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMContacts"]);
                    seTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCases"]);
                    etTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMServices"]);
                    enTab.Visible = Convert.ToBoolean(dt.Rows[0]["CRMCollateral"]);
                    
                }
                else
                {
                    DivActivities.Visible = Convert.ToBoolean(0);
                    DivCampaigns.Visible = Convert.ToBoolean(0);
                    DivLeads.Visible = Convert.ToBoolean(0);
                    DivOpportunities.Visible = Convert.ToBoolean(0);
                    DivAccounts.Visible = Convert.ToBoolean(0);
                    DivContacts.Visible = Convert.ToBoolean(0);
                    DivCases.Visible = Convert.ToBoolean(0);
                    DivServices.Visible = Convert.ToBoolean(0);
                    DivCollateral.Visible = Convert.ToBoolean(0);

                    fTab.Visible = Convert.ToBoolean(0);
                    STab.Visible = Convert.ToBoolean(0);
                    tTab.Visible = Convert.ToBoolean(0);
                    frTab.Visible = Convert.ToBoolean(0);
                    fvTab.Visible = Convert.ToBoolean(0);
                    sxTab.Visible = Convert.ToBoolean(0);
                    seTab.Visible = Convert.ToBoolean(0);
                    etTab.Visible = Convert.ToBoolean(0);
                    enTab.Visible = Convert.ToBoolean(0);
                }
            }
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
               objQuickMenuLink.TotalLeads =Convert.ToString(dsInst.Rows[0][0]);
               objQuickMenuLink.openL =Convert.ToString(dsInst.Rows[0][1]);
               objQuickMenuLink.QualifiedL =Convert.ToString(dsInst.Rows[0][2]);
               objQuickMenuLink.LostL =Convert.ToString(dsInst.Rows[0][3]);

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
        
    }
}