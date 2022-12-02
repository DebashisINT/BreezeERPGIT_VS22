using CRM.Models.DataContext;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    
    public class crmLeads
    {


        public bool IsDefault { get; set; }
        public string CredentialSubmitted { get; set; }
        public string TypeMaterial { get; set; }
        public string VolumeBusiness { get; set; }
        public string ValueBusinessExpected { get; set; }
        public string EnquiryExpected { get; set; }       

        public List<V_ModeOfVisit> Mode_Visit { get; set; }
        public List<V_BusinessActivity> Business_Activities { get; set; }
        public List<V_BusinessPresence> Business_Presence { get; set; }

        public List<V_ProductApplication> Product_Application { get; set; }
        public List<V_Brand> Brand { get; set; }
        public List<V_PainArea> Pain_Area { get; set; }
        public List<V_CurrentRequirement> Current_Requirement { get; set; }
        public Int64 ModeOfVisit { get; set; }
        public Int64 BusinessActivities { get; set; }
        public Int64 ProductApplication { get; set; }
        public Int64 BusinessPresence { get; set; }

        public List<V_ProductClass> Product_Class { get; set; }
        public Int64 ProductClass { get; set; }
        
        public List<V_SectorList> Sector_List { get; set; }
        public Int64 Sector { get; set; }
        public List<V_Designation> Person_Designation { get; set; }
        public Int64 DesignationID { get; set; }
        public Int64 BrandId { get; set; }
        public Int64 PainAreaID { get; set; }
        public Int64 CurrentRequirementID { get; set; }

        public string Competitor { get; set; }
        public string FinancialStatus { get; set; }
        public string Remarks { get; set; }
        
        public string Action { get; set; }
        public string Lead_Id { get; set; }
        public Int64 ServiceCnt_Id { get; set; }
        public Int64 OwnerID { get; set; }
        public Int64 AssignedID { get; set; }
        public string AssignerName { get; set; }
        public Int64 SourceID { get; set; }
        public List<V_UserLIst> Assigners { get; set; }
        public Int64 RatingID { get; set; }
        public Int64 Status_Id { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public string LeadCode { get; set; }

        // Rev Mantis Issue 23438
        public string QualifyAccount { get; set; }
        public string QualifyContact { get; set; }
        public string QualifyOpportunity { get; set; }
        // End of Rev Mantis Issue 23438
        public string firstname { get; set; }
        public string ServiceStatusId { get; set; }
        public string lastname { get; set; }
        public string ServiceName { get; set; }
        public string UniqueId { get; set; }
        public int job_titleId { get; set; }
        public string Business_phone { get; set; }
        public string Mobile { get; set; }
        public string email { get; set; }
        public string company_name { get; set; }
        public string website { get; set; }

        public CustomerDetails custdetails { get; set; }
        public string company_description { get; set; }
        public Int64 IndustryID { get; set; }
        public decimal Annual_Reveneu { get; set; }
        public decimal ServiceAmount { get; set; }
        public decimal ProdServCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public int No_of_Employee { get; set; }
        public int Source_campaign { get; set; }
        public int Marketing_Materials { get; set; }
        public DateTime? Last_LeadCampaign_Date { get; set; }

        public string stree1 { get; set; }
        public string stree2 { get; set; }
        public string stree3 { get; set; }
        public string Landmark { get; set; }
        public string job_title { get; set; }
        public string pin { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string Owner { get; set; }
        public string LeadValuesInternalID { get; set; }
        public string LeadValuesInternalName { get; set; }
        public string LeadValuesType { get; set; }
        public string LeadValue { get; set; }
        //public string Assign_to { get; set; }
        //public string Lead_Source { get; set; }
        //public string rating { get; set; }
        public string Status { get; set; }
        //public string company_industry { get; set; }
        //public string GSTIN { get; set; }


        //public string SIC_Code { get; set; }
        //public string Currency { get; set; }
        //public string TypeDetails { get; set; }
        public string cnt_shortName { get; set; }

        public String Customer { get; set; }

        public String Customer_ID { get; set; }

        public string RenewalDate { get; set; }
        public string RenewalStartDate { get; set; }
        public string RenewalEndDate { get; set; }
        public string Servicedesc { get; set; }
        public string Expected_Reveneu { get; set; }
        public List<V_UserLIst> Users { get; set; }
        public List<V_AssigneList> AssignTo { get; set; }
        public List<v_ContactSource> ContactSource { get; set; }
        public List<v_StatusDetail> Status_Details { get; set; }
        public List<V_Rating> Ratings { get; set; }
        public List<V_Industry> Industries { get; set; }
        public List<v_jobResponsibility> jobtitles { get; set; }

        public List<v_crmContact> crmcontacts { get; set; }
        public List<String> cntids { get; set; }

        public string crmcontacts_id { get; set; }
        public String ProductId { get; set; }
        public String ProductQty { get; set; }

        public String ProductPrice { get; set; }
        public String ProductAmount { get; set; }

        public DateTime? WarrStartdate { get; set; }
        public DateTime? WarrEnddate { get; set; }
        public List<MarketingMaterials> marketingmaterials { get; set; }
        public List<v_Campaign> CampaignList { get; set; }

        public String ActivityFilter { get; set; }
        



        public class ServiceContactProduct
        {
           
            public String ProductName { get; set; }

            public String ProductId { get; set; }
            public String ProductQty { get; set; }

           public String Price { get; set; }

            public String Amount { get; set; }
            public String ProductDetailsID { get; set; }
           public String WarrentyStartdate { get; set; }
            public String WarrentyEnddate { get; set; }

            public String HIddenID { get; set; }
         
        }

        public string SaveLeads(crmLeads CRMLeads)
        {

            try
            {
                string output = string.Empty;

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", CRMLeads.Action);
                cmd.Parameters.AddWithValue("@contacttype", "Lead");
                cmd.Parameters.AddWithValue("@LeadValue", CRMLeads.LeadValue);
                cmd.Parameters.AddWithValue("@Owner_id", CRMLeads.OwnerID);
                cmd.Parameters.AddWithValue("@Assign_To", CRMLeads.AssignedID);
                cmd.Parameters.AddWithValue("@LeadSource_id", CRMLeads.SourceID);
                cmd.Parameters.AddWithValue("@Rating_id", CRMLeads.RatingID);
                cmd.Parameters.AddWithValue("@Status_id", CRMLeads.Status_Id);
                cmd.Parameters.AddWithValue("@contacts", CRMLeads.crmcontacts_id);

                cmd.Parameters.AddWithValue("@Topic", CRMLeads.Topic);
                cmd.Parameters.AddWithValue("@Lead_Code", CRMLeads.LeadCode);

                cmd.Parameters.AddWithValue("@cnt_FirstName", CRMLeads.firstname);
                cmd.Parameters.AddWithValue("@cnt_LastName", CRMLeads.lastname);
                cmd.Parameters.AddWithValue("@JobTitle", CRMLeads.job_titleId);
                cmd.Parameters.AddWithValue("@BusinessPhone", CRMLeads.Business_phone);
                cmd.Parameters.AddWithValue("@MobilePhone", CRMLeads.Mobile);
                cmd.Parameters.AddWithValue("@Email", CRMLeads.email);


                cmd.Parameters.AddWithValue("@CompanyName", CRMLeads.company_name);
                cmd.Parameters.AddWithValue("@Website", CRMLeads.website);
                cmd.Parameters.AddWithValue("@Address1", CRMLeads.custdetails.Address1);
                cmd.Parameters.AddWithValue("@Address2", CRMLeads.custdetails.Address2);
                cmd.Parameters.AddWithValue("@Address3", CRMLeads.custdetails.Address3);
                cmd.Parameters.AddWithValue("@Landmark", CRMLeads.custdetails.Landmark);
                cmd.Parameters.AddWithValue("@PinZip_Id", CRMLeads.custdetails.PinId);
                cmd.Parameters.AddWithValue("@Country_Id", CRMLeads.custdetails.CountryId);
                cmd.Parameters.AddWithValue("@State_Id", CRMLeads.custdetails.StateId);
                cmd.Parameters.AddWithValue("@City_Id", CRMLeads.custdetails.CityId);

                cmd.Parameters.AddWithValue("@Description", CRMLeads.company_description);
                cmd.Parameters.AddWithValue("@Industry_Id", CRMLeads.IndustryID);
                cmd.Parameters.AddWithValue("@Anual_Avenue", CRMLeads.Annual_Reveneu);
                cmd.Parameters.AddWithValue("@NoOfEmployee", CRMLeads.No_of_Employee);

                cmd.Parameters.AddWithValue("@SourceCampaign", CRMLeads.Source_campaign);
                cmd.Parameters.AddWithValue("@MarketingMaterials", CRMLeads.Marketing_Materials);
                cmd.Parameters.AddWithValue("@LastCapaignDate", CRMLeads.Last_LeadCampaign_Date);
                cmd.Parameters.AddWithValue("@Crm_Leadid", CRMLeads.Lead_Id);
                cmd.Parameters.AddWithValue("@LeadValuesInternalID", CRMLeads.LeadValuesInternalID);
                cmd.Parameters.AddWithValue("@LeadValuesType", CRMLeads.LeadValuesType);
                cmd.Parameters.AddWithValue("@Expected_Reveneu", CRMLeads.Expected_Reveneu);

                // Rev Mantis Issue 23438
                if (CRMLeads.Status_Id == 13) //Qualify
                {
                    cmd.Parameters.AddWithValue("@QUALIFYACCOUNT", CRMLeads.QualifyAccount);
                    cmd.Parameters.AddWithValue("@QUALIFYCONTACT", CRMLeads.QualifyContact);
                    cmd.Parameters.AddWithValue("@QUALIFYOPPORTUNITY", CRMLeads.QualifyOpportunity);
                }
                // End of Rev Mantis Issue 23438

                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));


                cmd.Parameters.AddWithValue("@Sector", CRMLeads.Sector);
                cmd.Parameters.AddWithValue("@ModeOfVisit", CRMLeads.ModeOfVisit);
                cmd.Parameters.AddWithValue("@CredentialSubmitted", CRMLeads.CredentialSubmitted);
                cmd.Parameters.AddWithValue("@BusinessActivities", CRMLeads.BusinessActivities);
                cmd.Parameters.AddWithValue("@ProductApplication", CRMLeads.ProductApplication);
                cmd.Parameters.AddWithValue("@TypeMaterial", CRMLeads.TypeMaterial);
                cmd.Parameters.AddWithValue("@VolumeBusiness", CRMLeads.VolumeBusiness);
                cmd.Parameters.AddWithValue("@ValueBusinessExpected", CRMLeads.ValueBusinessExpected);
                cmd.Parameters.AddWithValue("@BusinessPresence", CRMLeads.BusinessPresence);
                cmd.Parameters.AddWithValue("@BrandId", CRMLeads.BrandId);
                cmd.Parameters.AddWithValue("@Competitor", CRMLeads.Competitor);
                cmd.Parameters.AddWithValue("@PainAreaID", CRMLeads.PainAreaID);
                cmd.Parameters.AddWithValue("@CurrentRequirementID", CRMLeads.CurrentRequirementID);
                cmd.Parameters.AddWithValue("@FinancialStatus", CRMLeads.FinancialStatus);
                cmd.Parameters.AddWithValue("@EnquiryExpected", CRMLeads.EnquiryExpected);
                cmd.Parameters.AddWithValue("@Remarks", CRMLeads.Remarks);
                cmd.Parameters.AddWithValue("@ProductClass", CRMLeads.ProductClass);
                


                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return Convert.ToString(output);



            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }


        public string SaveKeyDetails(System.Data.DataTable dtCRMLeadKey,string InternalId)
        {

            try
            {
                string output = string.Empty;

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "AddKeyPerson");
               // cmd.Parameters.AddWithValue("@contacttype", "Lead");
                cmd.Parameters.Add("@Crm_internalid", InternalId);
                cmd.Parameters.AddWithValue("@LeadKeyDetails", dtCRMLeadKey);
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));           

                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = cmd.Parameters["@result"].Value.ToString();
                con.Dispose();

                return Convert.ToString("Data save");



            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public DataTable GetCRMProductsDetails(string Lead_Id)
        {
            try
            {   
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PROC_CRM_LEADCONTACTINSERT");
                proc.AddVarcharPara("@ACTION", 100, "EditKeyPerson");
                proc.AddVarcharPara("@Crm_Leadid", 100, Lead_Id);
                ds = proc.GetTable();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetServiceContactProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_CRM_ServiceCNTDetails");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsId", DetailsID);
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetDetailsEntryListByID(String Action, Int64 DetailsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PROC_CRM_ServiceCNTDetails");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsId", DetailsID);
            ds = proc.GetDataSet();
            return ds;
        }

        public string DeleteService(Int64 Id)
        {
            try
            {
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_ServiceINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Delete");

                cmd.Parameters.AddWithValue("@ServiceCnt_Id", Id);


                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return Convert.ToString("Data Delete Successfully.");
            }
            catch(Exception e)
            {
                return "Please try again later.";
            }
        }
        public string SaveService(Int64 ServiceCnt_Id, string Action, Int64 OwnerID, Int64 AssignedID, string ServiceStatusId, string RenewalDate, string ServiceName, string UniqueId, string Customer_ID, string RenewalStartDate, string RenewalEndDate,
            string Servicedesc, decimal ServiceAmount, decimal ProdServCost, decimal AdditionalCost, string crmcontacts_id, DataTable ProductDT)
        {

            try
            {
                string output = string.Empty;

                if (ProductDT.Columns.Contains("HIddenID"))
                {
                    ProductDT.Columns.Remove("HIddenID");
                    ProductDT.AcceptChanges();
                }



                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_ServiceINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", Action);
                
                cmd.Parameters.AddWithValue("@Service_OwnerId", Convert.ToInt64(OwnerID));
                cmd.Parameters.AddWithValue("@Service_AssignId", Convert.ToInt64(AssignedID));
                cmd.Parameters.AddWithValue("@Service_statusId", ServiceStatusId);
                cmd.Parameters.AddWithValue("@ServiceCnt_Id", ServiceCnt_Id);
                if (RenewalDate != null && RenewalDate != "")
                {
                    //cmd.Parameters.AddWithValue("@Service_RenewalDate", Convert.ToDateTime(RenewalDate));
                    cmd.Parameters.AddWithValue("@Service_RenewalDate", DateTime.ParseExact(RenewalDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@Service_Name", ServiceName);
                cmd.Parameters.AddWithValue("@Service_UniqueId", UniqueId);
                cmd.Parameters.AddWithValue("@CustomerId", Customer_ID);
                if (RenewalStartDate != null && RenewalStartDate != "")
                {
                   // cmd.Parameters.AddWithValue("@Service_StartDate", Convert.ToDateTime(RenewalStartDate));
                    cmd.Parameters.AddWithValue("@Service_StartDate", DateTime.ParseExact(RenewalStartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                if (RenewalEndDate != null && RenewalEndDate != "")
                {
                    cmd.Parameters.AddWithValue("@Service_EndDate", DateTime.ParseExact(RenewalEndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    //cmd.Parameters.AddWithValue("@Service_EndDate", Convert.ToDateTime(RenewalEndDate));
                }
                cmd.Parameters.AddWithValue("@Service_Description", Servicedesc);
                cmd.Parameters.AddWithValue("@Service_amount", Convert.ToDecimal(ServiceAmount));
                cmd.Parameters.AddWithValue("@Service_ProductCost", Convert.ToDecimal(ProdServCost));
                cmd.Parameters.AddWithValue("@Service_AddlCost", Convert.ToDecimal(AdditionalCost));
                cmd.Parameters.AddWithValue("@contacts", crmcontacts_id);
                cmd.Parameters.AddWithValue("@SeviceProduct", ProductDT);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return Convert.ToString("Data Save Successfully");



            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }
        public DataSet EditCRMLeads(crmLeads CRMLeads)
        {

            try
            {
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@Crm_Leadid", CRMLeads.Lead_Id);
                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string DeleteLeads(crmLeads CRMLeads)
        {

            try
            {
                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Crm_Leadid", CRMLeads.Lead_Id);
                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();
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

        public string QualifiedLeads(crmLeads CRMLeads)
        {
            try
            {
                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_CONVERSION", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Qualify");
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", CRMLeads.LeadCode);
                cmd.Parameters.AddWithValue("@LEAD_STATUS", CRMLeads.Status_Id);
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                // Rev Mantis Issue 23438
                cmd.Parameters.AddWithValue("@QUALIFYACCOUNT", CRMLeads.QualifyAccount);
                cmd.Parameters.AddWithValue("@QUALIFYCONTACT", CRMLeads.QualifyContact);
                cmd.Parameters.AddWithValue("@QUALIFYOPPORTUNITY", CRMLeads.QualifyOpportunity);
                // End of Rev Mantis Issue 23438

                cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@RETURNCODE"].Value;
                con.Dispose();
                if (output.Trim() == "1")
                {
                    returnmessage = "Qualified Successfully.";
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

        public string LostLeads(crmLeads CRMLeads)
        {

            try
            {
                //string returnmessage = "";
                //string output = string.Empty;
                //DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Action", "Lost");
                //cmd.Parameters.AddWithValue("@Crm_Leadid", CRMLeads.Lead_Id);
                //cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                //cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                //cmd.CommandTimeout = 0;
                //SqlDataAdapter Adap = new SqlDataAdapter();
                //Adap.SelectCommand = cmd;
                //Adap.Fill(dsInst);
                //cmd.Dispose();
                //output = (string)cmd.Parameters["@result"].Value;
                //con.Dispose();
                //if (output.Trim() == "1")
                //{
                //    returnmessage = "Cancelled/Lost has been done Successfully.";
                //}
                //else
                //{
                //    returnmessage = "Error Occured.";
                //}
                //return returnmessage;

                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_CONVERSION", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Lost");
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", CRMLeads.LeadCode);
                cmd.Parameters.AddWithValue("@LEAD_STATUS", 0);
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@RETURNCODE"].Value;
                con.Dispose();
                if (output.Trim() == "1")
                {
                    returnmessage = "Cancelled/Lost has been done Successfully.";
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

        public string SelectedSourceType(crmLeads CRMLeads)
        {

            try
            {

                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Lost");
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", CRMLeads.LeadCode);
                cmd.Parameters.AddWithValue("@LEAD_STATUS", 0);
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@RETURNCODE"].Value;
                con.Dispose();
                if (output.Trim() == "1")
                {
                    returnmessage = "Cancelled/Lost has been done Successfully.";
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


        public List<crmLeadMobileDetails> GetLeadMobileDetails(string MobileNo)
        {

            try
            {
                List<crmLeadMobileDetails> objlst = new List<crmLeadMobileDetails>();
                string returnmessage = "";
                string output = string.Empty;
                DataTable dsInst = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_DETAILS", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION", "GETMOBILEDATA");
                cmd.Parameters.AddWithValue("@MOBILE_NO", MobileNo);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();

                if (dsInst != null && dsInst.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsInst.Rows)
                    {
                        crmLeadMobileDetails objcrmLeadMobileDetails = new crmLeadMobileDetails();
                        objcrmLeadMobileDetails.Name = Convert.ToString(dr["NAME"]);
                        objcrmLeadMobileDetails.Unique_id = Convert.ToString(dr["UNIQUE_ID"]);
                        objlst.Add(objcrmLeadMobileDetails);
                    }


                }


                return objlst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
    public class crmLeadsQualify
    {
        public string Lead_Id { get; set; }
        public List<V_UserLIst> ConverttoUsers { get; set; }
        public string Remarks { get; set; }
    }
    public class MarketingMaterials
    {
        public int material_id { get; set; }
        public string material_Name { get; set; }
    }

    public class crmLeadMobileDetails
    {
        public string Name { get; set; }
        public string Unique_id { get; set; }
    }

    public class CustomerDetails
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
        public string add_Website { get; set; }
        public bool Isdefault { get; set; }

    }

    public class crmLeadValuesModel
    {
        public string id { get; set; }
        public string Name { get; set; }

    }

    public class CONTACT_IDs
    {
        public string CONTACT_ID { get; set; }
    }

    public class crmKey
    {
        public string guid { get; set; }
        public string Name { get; set; }
        public string DesignationID { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public string IsDefault { get; set; }  
    }

     




}