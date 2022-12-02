using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmContact
    {
        public string Contact_Id { get; set; }
        public string Action { get; set; }
        public string Topic { get; set; }
        public string Mobile { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string Unique_Id { get; set; }

        public string Business_phone { get; set; }
        public Int64 job_title { get; set; }
        public string email { get; set; }

        public string Fax { get; set; }
        public string company_name { get; set; }
        public string website { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LandMark { get; set; }

        public string pin { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string owner { get; set; }
        public string Assign_to { get; set; }
        public string Lead_Source { get; set; }
        public string rating { get; set; }
        public string Status { get; set; }
        public string company_description { get; set; }
        public string company_industry { get; set; }
        public string Annual_Reveneu { get; set; }
        public string No_of_Employee { get; set; }
        public string GSTIN { get; set; }
        public string Currency { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Source_campaign { get; set; }
        public string Spouce_Name { get; set; }
        public string Freight_Terms { get; set; }

        public DateTime? BirthDay { get; set; }
        public DateTime? Anniversary { get; set; }
        public string Marketing_Materials { get; set; }

        public string Originating_Lead { get; set; }
        public string Credit_holdId { get; set; }

        public decimal Credit_Limit { get; set; }
        public DateTime? Last_Campaign_Date { get; set; }
        public Int64 RatingID { get; set; }
        public Int64 Status_Id { get; set; }
        public Int64 TYPE_ID { get; set; }
        public Int64 WonerID { get; set; }
        public Int64 AssignedID { get; set; }
        public Int64 SourceID { get; set; } 

        public Int64 PaymentTermID { get; set; }
        public Int64 ShippingMethodID { get; set; }
        public Int64 PrefferedMethodofContactID { get; set; }

        public Int64 jobResponsibilityID { get; set; }
        public Int64 maritalstatus_ID { get; set; }
        public string Account_ID { get; set; }
        public string crmleads_id { get; set; }

        // Mantis Issue 21677,21676,23104 (03/06/2021)
        public string crmoportunity_id { get; set; }
        public List<String> oppids { get; set; }
        // End of Mantis Issue 21677,21676,23104 (03/06/2021)

        public List<String> LEAD_LIST { get; set; }
        public List<CrmEntityList> Entity_List { get; set; }
        public List<crm_StatusDetail> Status_Details { get; set; }
        public List<crm_CampaignType> Campaign_Type { get; set; }
        public List<V_UserLIst> Users { get; set; }
        public List<v_ContactSource> ContactSource { get; set; }
        public List<v_jobResponsibility> jobResponsibility_List { get; set; }
        public List<V_CNTACCOUNTLIST> V_CNTACCOUNTLISTS { get; set; }
        public List<V_Rating> Rating_List { get; set; }
        public List<MarketingMaterials> marketingmaterials { get; set; }
        public List<v_maritalstatus> maritalstatus_List { get; set; }

        public List<Master_PaymentTerm> PaymentTerm_List { get; set; }
        public List<Master_ShippingMethod> ShippingMethod_List { get; set; }
        public List<Master_PrefferedMethodofContact> PrefferedMethodofContact_List { get; set; }

        internal string SaveContact(crmContact crmContactobj)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMContact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", crmContactobj.Action);
                cmd.Parameters.AddWithValue("@Contact_Id", crmContactobj.Contact_Id);
                cmd.Parameters.AddWithValue("@Unique_Id", crmContactobj.Unique_Id);

                cmd.Parameters.AddWithValue("@firstname", crmContactobj.firstname);
                cmd.Parameters.AddWithValue("@lastname", crmContactobj.lastname);
                cmd.Parameters.AddWithValue("@job_title", crmContactobj.job_title);
                cmd.Parameters.AddWithValue("@jobResponsibilityID", crmContactobj.jobResponsibilityID);
                cmd.Parameters.AddWithValue("@email", crmContactobj.email);
                cmd.Parameters.AddWithValue("@Business_phone", crmContactobj.Business_phone);
                cmd.Parameters.AddWithValue("@Mobile", crmContactobj.Mobile);
                cmd.Parameters.AddWithValue("@Fax", crmContactobj.Fax);
                cmd.Parameters.AddWithValue("@PrefferedMethodofContactID", crmContactobj.PrefferedMethodofContactID);
                cmd.Parameters.AddWithValue("@owner", crmContactobj.WonerID);
                cmd.Parameters.AddWithValue("@Status_Id", crmContactobj.Status_Id);
                cmd.Parameters.AddWithValue("@Lead_Source", crmContactobj.SourceID);
                cmd.Parameters.AddWithValue("@RatingID", crmContactobj.RatingID);
                cmd.Parameters.AddWithValue("@Account_ID", crmContactobj.Account_ID);
                cmd.Parameters.AddWithValue("@AssignedID", crmContactobj.AssignedID);
                cmd.Parameters.AddWithValue("@Gender", crmContactobj.Gender);
                cmd.Parameters.AddWithValue("@maritalstatus_ID", crmContactobj.maritalstatus_ID);
                cmd.Parameters.AddWithValue("@Spouce_Name", crmContactobj.Spouce_Name);
                cmd.Parameters.AddWithValue("@BirthDay", crmContactobj.BirthDay);
                cmd.Parameters.AddWithValue("@Anniversary", crmContactobj.Anniversary);
                cmd.Parameters.AddWithValue("@Credit_Limit", crmContactobj.Credit_Limit);
                cmd.Parameters.AddWithValue("@PaymentTermID", crmContactobj.PaymentTermID);
                cmd.Parameters.AddWithValue("@Credit_holdId", crmContactobj.Credit_holdId);
                cmd.Parameters.AddWithValue("@Originating_Lead", crmContactobj.Originating_Lead);
                cmd.Parameters.AddWithValue("@Last_Campaign_Date", crmContactobj.Last_Campaign_Date);
                cmd.Parameters.AddWithValue("@Marketing_Materials", crmContactobj.Marketing_Materials);
                cmd.Parameters.AddWithValue("@ShippingMethodID", crmContactobj.ShippingMethodID);
                cmd.Parameters.AddWithValue("@Freight_Terms", crmContactobj.Freight_Terms);
                cmd.Parameters.AddWithValue("@USER_ID", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                cmd.Parameters.AddWithValue("@Address1", crmContactobj.Address1);
                cmd.Parameters.AddWithValue("@Address2", crmContactobj.Address2);
                cmd.Parameters.AddWithValue("@Address3", crmContactobj.Address3);
                cmd.Parameters.AddWithValue("@LankMark", crmContactobj.LandMark);
                cmd.Parameters.AddWithValue("@pin", crmContactobj.pin);
                cmd.Parameters.AddWithValue("@Leads", crmContactobj.crmleads_id);

                // Mantis Issue 21677,21676,23104 (03/06/2021)
                cmd.Parameters.AddWithValue("@opportunity", crmContactobj.crmoportunity_id);
                // End of Mantis Issue 21677,21676,23104 (03/06/2021)



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

        internal DataSet EditCampaign(crmContact newCRMCampaignobj)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMContact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "EditDetails");
                cmd.Parameters.AddWithValue("@Contact_Id", newCRMCampaignobj.Contact_Id);
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

        internal string DeleteContact(crmContact newCRMContactobj)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMContact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DeleteDetails");
                cmd.Parameters.AddWithValue("@Contact_Id", newCRMContactobj.Contact_Id);
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
}