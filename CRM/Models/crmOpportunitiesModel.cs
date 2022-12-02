using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmOpportunitiesModel
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PinId { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }


        public string OpportunitiesID { get; set; }
        public string Topic { get; set; }
        public string  Contact { get; set; }
        public string  Account { get; set; }
        public string  PurchaseTimeframe { get; set; }
        public string  Rating { get; set; }
        public string BudgetAmount { get; set; }

        public String PurchaseProcess { get; set; }
        public String Description { get; set; }
        public String CurrentSituation { get; set; }
        public String CustomerNeed { get; set; }
        public string Unique_Id { get; set; }
        public String PropossedSolution { get; set; }
        public String CompanyName { get; set; }
        public String ContactName { get; set; }

        public string Status_Id { get; set; }
        public string TYPE_ID { get; set; }
        public String Woner { get; set; }
        public string Action { get; set; }
      

        public string Estimate_Revenue { get; set; }

        public string EstCloseDate { get; set; }

        public string PurchaseProcessID { get; set; }
        public string PurchaseTimeframeID { get; set; }
        public string RatingID { get; set; }
        public string WonerID { get; set; }
        public string AssignedID { get; set; }
        public string SourceID { get; set; }

        // Rev Mantis issue 20684 (27/05/2021)
        public string Probability_Perc { get; set; }
        // End of Rev Mantis issue 20684 (27/05/2021)
        public List<V_Rating> Rating_List { get; set; }
        public List<CrmEntityList> Entity_List { get; set; }
        public List<crm_StatusDetail> Status_Details { get; set; }
        public List<crm_CampaignType> Campaign_Type { get; set; }
        public List<V_UserLIst> Users { get; set; }
        public List<v_ContactSource> ContactSource { get; set; }
         public List<v_PurchaseTimeframe> Timeframe_List { get; set; }
         public List<v_PurchaseProcess> PurchaseProcess_List { get; set; }
         //public CustomerAddress custdetails { get; set; }

         public List<String> cntids { get; set; }

         public string crmcontacts_id { get; set; }

         public DateTime? estDate_fromDB { get; set; }

         public string SaveOpportunities(crmOpportunitiesModel CRMOper)
         {

             try
             {
                 string output = string.Empty;


                 DateTime? estClose = null;

                 if (CRMOper.EstCloseDate != "" && CRMOper.EstCloseDate != "01-01-0100")
                 {
                     estClose=DateTime.ParseExact(CRMOper.EstCloseDate,"dd-MM-yyyy",CultureInfo.CurrentCulture);
                 }


                 DataSet dsInst = new DataSet();
                 SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                 SqlCommand cmd = new SqlCommand("PROC_CRM_OpportunitiesCONTACTINSERT", con);
                 cmd.CommandType = CommandType.StoredProcedure;

                 cmd.Parameters.AddWithValue("@Action", CRMOper.Action);
                 cmd.Parameters.AddWithValue("@contacttype", "OP");

                 cmd.Parameters.AddWithValue("@Owner_id", CRMOper.Woner);
                 cmd.Parameters.AddWithValue("@Assign_To", CRMOper.AssignedID);
                 cmd.Parameters.AddWithValue("@LeadSource_id", CRMOper.SourceID);
                 cmd.Parameters.AddWithValue("@Rating_id", CRMOper.RatingID);
                 cmd.Parameters.AddWithValue("@Status_id", CRMOper.Status_Id);

                 cmd.Parameters.AddWithValue("@Topic", CRMOper.Topic);
                 cmd.Parameters.AddWithValue("@ContactID", CRMOper.crmcontacts_id);
                 cmd.Parameters.AddWithValue("@AccountID", CRMOper.Account);
                 cmd.Parameters.AddWithValue("@PurchaseTimeframe", CRMOper.PurchaseTimeframeID);
                 cmd.Parameters.AddWithValue("@Budget_Amount", CRMOper.BudgetAmount);
                 cmd.Parameters.AddWithValue("@Purchase_Process", CRMOper.PurchaseProcessID);
                 cmd.Parameters.AddWithValue("@Current_Situation", CRMOper.CurrentSituation);
                 cmd.Parameters.AddWithValue("@Customer_Need", CRMOper.CustomerNeed);
                 cmd.Parameters.AddWithValue("@Propossed_Solution", CRMOper.PropossedSolution);
                 cmd.Parameters.AddWithValue("@EstRevenue", CRMOper.Estimate_Revenue);
                 cmd.Parameters.AddWithValue("@EstCloseDate", estClose);

                 cmd.Parameters.AddWithValue("@CompanyName", CRMOper.CompanyName);
                 cmd.Parameters.AddWithValue("@Website", CRMOper.Website);
                 cmd.Parameters.AddWithValue("@Address1", CRMOper.Address1);
                 cmd.Parameters.AddWithValue("@Address2", CRMOper.Address2);
                 cmd.Parameters.AddWithValue("@Address3", CRMOper.Address3);

                 cmd.Parameters.AddWithValue("@Landmark", CRMOper.LandMark);
                 cmd.Parameters.AddWithValue("@PinZip_Id", CRMOper.PinId);
                 cmd.Parameters.AddWithValue("@Country_Id", CRMOper.CountryId);
                 cmd.Parameters.AddWithValue("@State_Id", CRMOper.StateId);
                 cmd.Parameters.AddWithValue("@City_Id", CRMOper.CityId);

                 cmd.Parameters.AddWithValue("@Description", CRMOper.Description);
                 cmd.Parameters.AddWithValue("@OpportunitiesID", CRMOper.OpportunitiesID);
                 cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                 cmd.Parameters.AddWithValue("@OPP_CODE", CRMOper.Unique_Id);
                 
                 // Rev Mantis issue 20684 (27/05/2021)
                 cmd.Parameters.AddWithValue("@Probability_Perc", CRMOper.Probability_Perc);
                 // End of Rec Mantis issue 20684 (27/05/2021)


                 
                 cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                 cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                 cmd.CommandTimeout = 0;
                 SqlDataAdapter Adap = new SqlDataAdapter();
                 Adap.SelectCommand = cmd;
                 Adap.Fill(dsInst);
                 cmd.Dispose();
                 output = (string)cmd.Parameters["@result"].Value;
                 con.Dispose();

                 return Convert.ToString("Data save");



             }
             catch (Exception ex)
             {
                 return "Please try again later.";
             }
         }


         public DataSet EditOpportunities(crmOpportunitiesModel CRMOper)
         {

             try
             {
                 string output = string.Empty;

                 DataSet dsInst = new DataSet();
                 SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                 SqlCommand cmd = new SqlCommand("PROC_CRM_OpportunitiesCONTACTINSERT", con);
                 cmd.CommandType = CommandType.StoredProcedure;

                 cmd.Parameters.AddWithValue("@Action", "Edit");
                 cmd.Parameters.AddWithValue("@Crm_Opportunitiesid",CRMOper.OpportunitiesID);


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

         public string Website { get; set; }
                
         public string CountryId { get; set; }
                
         public string StateId { get; set; }
                
         public string CityId { get; set; }

         internal static string Delete(crmOpportunitiesModel OBJ)
         {
             try
             {
                 string returnmessage = "";

                 string output = string.Empty;

                 DataSet dsInst = new DataSet();
                 SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                 SqlCommand cmd = new SqlCommand("PROC_CRM_OpportunitiesCONTACTINSERT", con);
                 cmd.CommandType = CommandType.StoredProcedure;

                 cmd.Parameters.AddWithValue("@Action", "Delete");
                 cmd.Parameters.AddWithValue("@Crm_Opportunitiesid", OBJ.OpportunitiesID);
                 cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                 cmd.Parameters["@result"].Direction = ParameterDirection.Output;

                 cmd.CommandTimeout = 0;
                 SqlDataAdapter Adap = new SqlDataAdapter();
                 Adap.SelectCommand = cmd;
                 Adap.Fill(dsInst);
                 cmd.Dispose();
                 con.Dispose();
                 output = (string)cmd.Parameters["@result"].Value;
                 if (output.Trim() == "1")
                 {
                     returnmessage = "Deleted Successfully.";
                 }
                 else
                 {
                     returnmessage = "Error Occured.";
                 }
                 return returnmessage;

             }
             catch (Exception ex)
             {
                 return null;
             }
         }
    }
    public class ContactModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string add { get; set; }
    }

    public class AccountModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }

    public class CompanyModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }

    public class CustomerAddress
    {
        public string Id { get; set; }
        public string CustType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int PinId { get; set; }
        public string PinCode { get; set; }

        public decimal Distance { get; set; }

        public string GSTIN { get; set; }
        public string Landmark { get; set; }

        public bool Isdefault { get; set; }
        public String WebSite { get; set; }

    }

    public class QuotationDetailsList
    {
        public string Quote_Id { get; set; }
        public string Quote_Number { get; set; }
        public DateTime Quote_Date { get; set; }
        public decimal Quote_TotalAmount { get; set; }
        public DateTime Quote_Expiry { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Customer_Id { get; set; }
    }

    public class QuotationDetails
    {
        public string Quote_Id { get; set; }
        public string Quote_Number { get; set; }
        public DateTime Quote_Date { get; set; }
        public decimal Quote_TotalAmount { get; set; }
        public DateTime Quote_Expiry { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Customer_Id { get; set; }
    }


}