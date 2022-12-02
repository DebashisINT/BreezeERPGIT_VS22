using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMCampaign
    {


        public string SaveCampaign(CRMCampaign CRMCampaignobj)
        {

            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMCampaign", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", CRMCampaignobj.Action);
                cmd.Parameters.AddWithValue("@Campaign_Name", CRMCampaignobj.Campaign_Name);
                cmd.Parameters.AddWithValue("@Campaign_Id", CRMCampaignobj.Campaign_Id);
                cmd.Parameters.AddWithValue("@Campaign_Code", CRMCampaignobj.Campaign_Code);
                cmd.Parameters.AddWithValue("@Expected_Response", CRMCampaignobj.Expected_Response);
                //if (CRMCampaignobj.Expected_Start.Year!=1)
                cmd.Parameters.AddWithValue("@Expected_Start", CRMCampaignobj.Expected_Start);
                //if (CRMCampaignobj.Expected_End.Year != 1)
                cmd.Parameters.AddWithValue("@Expected_End", CRMCampaignobj.Expected_End);
                //if (CRMCampaignobj.Actual_Start.Year != 1)
                cmd.Parameters.AddWithValue("@Actual_Start", CRMCampaignobj.Actual_Start);
                //if (CRMCampaignobj.Actual_End.Year != 1)
                cmd.Parameters.AddWithValue("@Actual_End", CRMCampaignobj.Actual_End);
                cmd.Parameters.AddWithValue("@Offer", CRMCampaignobj.Offer);
                cmd.Parameters.AddWithValue("@Estimate_Revenue", CRMCampaignobj.Estimate_Revenue);
                cmd.Parameters.AddWithValue("@Status_Id", CRMCampaignobj.Status_Id);
                cmd.Parameters.AddWithValue("@Type_id", CRMCampaignobj.TYPE_ID);
                cmd.Parameters.AddWithValue("@Woner", CRMCampaignobj.Woner);
                cmd.Parameters.AddWithValue("@Activity_Cost", CRMCampaignobj.Activity_Cost);
                cmd.Parameters.AddWithValue("@Allocated_Budget", CRMCampaignobj.Allocated_Budget);
                cmd.Parameters.AddWithValue("@Misc_Cost", CRMCampaignobj.Misc_Cost);
                cmd.Parameters.AddWithValue("@Total_Cost", CRMCampaignobj.Total_Cost);
                cmd.Parameters.AddWithValue("@SourceID", CRMCampaignobj.SourceID);
                cmd.Parameters.AddWithValue("@WonerId", CRMCampaignobj.WonerID);
                cmd.Parameters.AddWithValue("@AssignedId", CRMCampaignobj.AssignedID);

                cmd.Parameters.AddWithValue("@USER_ID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return Convert.ToString("Data save");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }


        public DataSet EditCampaign(CRMCampaign CRMCampaignobj)
        {

            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMCampaign", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "EditDetails");
                cmd.Parameters.AddWithValue("@Campaign_Id", CRMCampaignobj.Campaign_Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }









        public string Action { get; set; }

        [MaxLength(500)]
        public string Campaign_Name { get; set; }
        [MaxLength(50)]
        public string Campaign_Code { get; set; }
        public int Campaign_Id { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Expected Response must be a natural number")]
        [Range(0, 100.00)]
        public decimal Expected_Response { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Expected_Start { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Expected_End { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Actual_Start { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Actual_End { get; set; }
        public string Offer { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Estimate Revenue must be a natural number")]
        [Range(0, 999999999.99)]
        public decimal Estimate_Revenue { get; set; }
        public Int64 Status_Id { get; set; }
        public Int64 TYPE_ID { get; set; }
        public string Woner { get; set; }


        public Int64 WonerID { get; set; }
        public Int64 AssignedID { get; set; }
        public Int64 SourceID { get; set; }
        public List<CrmEntityList> Entity_List { get; set; }
        public List<crm_StatusDetail> Status_Details { get; set; }
        public List<crm_CampaignType> Campaign_Type { get; set; }
        public List<V_UserLIst> Users { get; set; }
        public List<v_ContactSource> ContactSource { get; set; }



        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Activity Cost must be a natural number")]
        [Range(0, 999999999.99)]
        public decimal Activity_Cost { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Allocated Budget must be a natural number")]
        [Range(0, 999999999.99)]
        public decimal Allocated_Budget { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Misc Cost must be a natural number")]
        [Range(0, 999999999.99)]
        public decimal Misc_Cost { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Total Cost must be a natural number")]
        [Range(0, 999999999.99)]
        public decimal Total_Cost { get; set; }


        internal string DeleteCampaign(CRMCampaign newCRMCampaignobj)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMCampaign", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DeleteDetails");
                cmd.Parameters.AddWithValue("@Campaign_Id", newCRMCampaignobj.Campaign_Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return "Deleted Successfully.";
            }
            catch (Exception ex)
            {
                return "Please Try Again Later";
            }
        }
    }
    public class CrmEntityList
    {
        public string internal_Id { get; set; }
        public string Entity_Name { get; set; }
    }
}