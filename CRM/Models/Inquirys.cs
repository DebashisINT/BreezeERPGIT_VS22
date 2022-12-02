using BusinessLogicLayer;
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
    public class crmInquirys
    {
        public string Action { get; set; }
        public string Inquiry_Id { get; set; }
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
        public string InquiryCode { get; set; }
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
        public DateTime? Last_InquiryCampaign_Date { get; set; }

        public string stree1 { get; set; }
        public string stree2 { get; set; }
        public string stree3 { get; set; }
        public string Landmark { get; set; }
        public string job_title { get; set; }
        public string pin { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }


        public string s_Address1 { get; set; }
        public string s_Address2 { get; set; }
        public string s_Address3 { get; set; }
        public string s_Landmark { get; set; }
        public string s_Pin { get; set; }
        public string s_City { get; set; }
        public string s_State { get; set; }
        public string s_Country { get; set; }
        public string s_Pinid { get; set; }
        public string s_Cityid { get; set; }
        public string s_Stateid { get; set; }
        public string s_Countryid { get; set; }



        public string Owner { get; set; }
        public string InquiryValuesInternalID { get; set; }
        public string InquiryValuesInternalName { get; set; }
        public string InquiryValuesType { get; set; }
        public string InquiryValue { get; set; }

        public List<V_MultiGroup> groupList { get; set; }
        public string GroupId { get; set; }

        //public string Assign_to { get; set; }
        //public string Inquiry_Source { get; set; }
        //public string rating { get; set; }
        public string Status { get; set; }
        //public string company_industry { get; set; }
        //public string GSTIN { get; set; }


        //public string SIC_Code { get; set; }
        //public string Currency { get; set; }
        //public string TypeDetails { get; set; }
        public string cnt_shortName { get; set; }
        public string GSTIN { get; set; }

        public String Customer { get; set; }

        public String Customer_ID { get; set; }

        public string RenewalDate { get; set; }
        public string RenewalStartDate { get; set; }
        public string RenewalEndDate { get; set; }
        public string Servicedesc { get; set; }
        public string Expected_Reveneu { get; set; }
        public List<V_UserLIst> Users { get; set; }
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
        public string existingCustomeriD { get; set; }

        public List<crmAddProd> Productlist { get; set; }
        public string SalesType { get; set; }
        public string ServiceLocation { get; set; }

        public List<v_BranchList> BranchList { get; set; }
        public string BranchId { get; set; }

        public List<V_UserLIst> AccountManagerList { get; set; }
        public string AccountManagerId { get; set; }

        // Rev Sanchita
        public string TypeOfCustomer { get; set; }
        // End of Rev Sanchita


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

        public string SaveInquirys(crmInquirys CRMInquirys,DataTable dtproduct)
        {
            string output = string.Empty;

            try
            {


                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_InquiryCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", CRMInquirys.Action);
                cmd.Parameters.AddWithValue("@contacttype", "Inquiry");
                cmd.Parameters.Add("@InquiryValue", CRMInquirys.InquiryValue);
                cmd.Parameters.AddWithValue("@Owner_id", CRMInquirys.OwnerID);
                cmd.Parameters.AddWithValue("@Assign_To", CRMInquirys.AssignedID);
                cmd.Parameters.AddWithValue("@InquirySource_id", CRMInquirys.SourceID);
                cmd.Parameters.AddWithValue("@Rating_id", CRMInquirys.RatingID);
                cmd.Parameters.AddWithValue("@Status_id", CRMInquirys.Status_Id);
                cmd.Parameters.AddWithValue("@contacts", CRMInquirys.crmcontacts_id);

                cmd.Parameters.AddWithValue("@Topic", CRMInquirys.Topic);
                cmd.Parameters.AddWithValue("@Inquiry_Code", CRMInquirys.InquiryCode);

                cmd.Parameters.AddWithValue("@cnt_FirstName", CRMInquirys.firstname);
                cmd.Parameters.AddWithValue("@cnt_LastName", CRMInquirys.lastname);
                cmd.Parameters.AddWithValue("@JobTitle", CRMInquirys.job_titleId);
                cmd.Parameters.AddWithValue("@BusinessPhone", CRMInquirys.Business_phone);
                cmd.Parameters.AddWithValue("@MobilePhone", CRMInquirys.Mobile);
                cmd.Parameters.AddWithValue("@Email", CRMInquirys.email);


                cmd.Parameters.AddWithValue("@CompanyName", CRMInquirys.company_name);
                cmd.Parameters.AddWithValue("@Website", CRMInquirys.website);
                cmd.Parameters.AddWithValue("@Address1", CRMInquirys.custdetails.Address1);
                cmd.Parameters.AddWithValue("@Address2", CRMInquirys.custdetails.Address2);
                cmd.Parameters.AddWithValue("@Address3", CRMInquirys.custdetails.Address3);
                cmd.Parameters.AddWithValue("@Landmark", CRMInquirys.custdetails.Landmark);
                cmd.Parameters.AddWithValue("@PinZip_Id", CRMInquirys.custdetails.PinId);
                cmd.Parameters.AddWithValue("@Country_Id", CRMInquirys.custdetails.CountryId);
                cmd.Parameters.AddWithValue("@State_Id", CRMInquirys.custdetails.StateId);
                cmd.Parameters.AddWithValue("@City_Id", CRMInquirys.custdetails.CityId);

                cmd.Parameters.AddWithValue("@Description", CRMInquirys.company_description);
                cmd.Parameters.AddWithValue("@Industry_Id", CRMInquirys.IndustryID);
                cmd.Parameters.AddWithValue("@Anual_Avenue", CRMInquirys.Annual_Reveneu);
                cmd.Parameters.AddWithValue("@NoOfEmployee", CRMInquirys.No_of_Employee);

                cmd.Parameters.AddWithValue("@SourceCampaign", CRMInquirys.Source_campaign);
                cmd.Parameters.AddWithValue("@MarketingMaterials", CRMInquirys.Marketing_Materials);
                cmd.Parameters.AddWithValue("@LastCapaignDate", CRMInquirys.Last_InquiryCampaign_Date);
                cmd.Parameters.AddWithValue("@Crm_Inquiryid", CRMInquirys.Inquiry_Id);
                cmd.Parameters.AddWithValue("@InquiryValuesInternalID", CRMInquirys.InquiryValuesInternalID);
                cmd.Parameters.AddWithValue("@InquiryValuesType", CRMInquirys.InquiryValuesType);
                cmd.Parameters.AddWithValue("@Expected_Reveneu", CRMInquirys.Expected_Reveneu);

                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));



                cmd.Parameters.AddWithValue("@s_Address1", CRMInquirys.s_Address1);
                cmd.Parameters.AddWithValue("@s_Address2", CRMInquirys.s_Address2);
                cmd.Parameters.AddWithValue("@s_Address3", CRMInquirys.s_Address3);
                cmd.Parameters.AddWithValue("@s_Landmark", CRMInquirys.s_Landmark);
                cmd.Parameters.AddWithValue("@s_PinZip_Id", CRMInquirys.s_Pin);
                cmd.Parameters.AddWithValue("@s_Country_Id", CRMInquirys.s_Country);
                cmd.Parameters.AddWithValue("@s_State_Id", CRMInquirys.s_State);
                cmd.Parameters.AddWithValue("@s_City_Id", CRMInquirys.s_City);

                cmd.Parameters.AddWithValue("@GSTIN", CRMInquirys.GSTIN);
                cmd.Parameters.AddWithValue("@GroupId", CRMInquirys.GroupId);
                cmd.Parameters.AddWithValue("@existingCustomeriD", CRMInquirys.existingCustomeriD);


                cmd.Parameters.AddWithValue("@ACTIVITYPRODUCTS", dtproduct);

                cmd.Parameters.AddWithValue("@SalesType", CRMInquirys.SalesType);
                cmd.Parameters.AddWithValue("@ServiceLocation", CRMInquirys.ServiceLocation);
                cmd.Parameters.AddWithValue("@BranchId", CRMInquirys.BranchId);
                cmd.Parameters.AddWithValue("@AccountManagerId", CRMInquirys.AccountManagerId);
                // Rev Sanchita
                cmd.Parameters.AddWithValue("@TypeOfCustomer", CRMInquirys.TypeOfCustomer);
                // End of Rev Sanchita

                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                // return Convert.ToString("Data save");



            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }

            if (string.IsNullOrEmpty(CRMInquirys.existingCustomeriD) && CRMInquirys.Action == "Add")
            {

                try
                {
                    string output_LEAD = string.Empty;

                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PROC_CRM_LEADCONTACTINSERT", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", CRMInquirys.Action);
                    cmd.Parameters.AddWithValue("@contacttype", "Lead");
                    cmd.Parameters.Add("@LeadValue", CRMInquirys.InquiryValue);
                    cmd.Parameters.AddWithValue("@Owner_id", CRMInquirys.OwnerID);
                    cmd.Parameters.AddWithValue("@Assign_To", CRMInquirys.AssignedID);
                    cmd.Parameters.AddWithValue("@LeadSource_id", CRMInquirys.SourceID);
                    cmd.Parameters.AddWithValue("@Rating_id", CRMInquirys.RatingID);
                    cmd.Parameters.AddWithValue("@Status_id", CRMInquirys.Status_Id);
                    cmd.Parameters.AddWithValue("@contacts", CRMInquirys.crmcontacts_id);
                    cmd.Parameters.AddWithValue("@Topic", CRMInquirys.Topic);

                    cmd.Parameters.AddWithValue("@cnt_FirstName", CRMInquirys.firstname);
                    cmd.Parameters.AddWithValue("@cnt_LastName", CRMInquirys.lastname);
                    cmd.Parameters.AddWithValue("@JobTitle", CRMInquirys.job_titleId);
                    cmd.Parameters.AddWithValue("@BusinessPhone", CRMInquirys.Business_phone);
                    cmd.Parameters.AddWithValue("@MobilePhone", CRMInquirys.Mobile);
                    cmd.Parameters.AddWithValue("@Email", CRMInquirys.email);


                    cmd.Parameters.AddWithValue("@CompanyName", CRMInquirys.company_name);
                    cmd.Parameters.AddWithValue("@Website", CRMInquirys.website);
                    cmd.Parameters.AddWithValue("@Address1", CRMInquirys.custdetails.Address1);
                    cmd.Parameters.AddWithValue("@Address2", CRMInquirys.custdetails.Address2);
                    cmd.Parameters.AddWithValue("@Address3", CRMInquirys.custdetails.Address3);
                    cmd.Parameters.AddWithValue("@Landmark", CRMInquirys.custdetails.Landmark);
                    cmd.Parameters.AddWithValue("@PinZip_Id", CRMInquirys.custdetails.PinId);
                    cmd.Parameters.AddWithValue("@Country_Id", CRMInquirys.custdetails.CountryId);
                    cmd.Parameters.AddWithValue("@State_Id", CRMInquirys.custdetails.StateId);
                    cmd.Parameters.AddWithValue("@City_Id", CRMInquirys.custdetails.CityId);

                    cmd.Parameters.AddWithValue("@Description", CRMInquirys.company_description);
                    cmd.Parameters.AddWithValue("@Industry_Id", CRMInquirys.IndustryID);
                    cmd.Parameters.AddWithValue("@Anual_Avenue", CRMInquirys.Annual_Reveneu);
                    cmd.Parameters.AddWithValue("@NoOfEmployee", CRMInquirys.No_of_Employee);
                    cmd.Parameters.AddWithValue("@SourceCampaign", CRMInquirys.Source_campaign);
                    cmd.Parameters.AddWithValue("@MarketingMaterials", CRMInquirys.Marketing_Materials);
                    cmd.Parameters.AddWithValue("@LastCapaignDate", CRMInquirys.Last_InquiryCampaign_Date);
                    cmd.Parameters.AddWithValue("@LeadValuesInternalID", CRMInquirys.InquiryValuesInternalID);
                    cmd.Parameters.AddWithValue("@LeadValuesType", CRMInquirys.InquiryValuesType);
                    cmd.Parameters.AddWithValue("@Expected_Reveneu", CRMInquirys.Expected_Reveneu);
                    cmd.Parameters.AddWithValue("@Inquiry_id", output);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                    cmd.Parameters.AddWithValue("@SalesType", CRMInquirys.SalesType);
                    cmd.Parameters.AddWithValue("@ServiceLocation", CRMInquirys.ServiceLocation);
                    cmd.Parameters.AddWithValue("@BranchId", CRMInquirys.BranchId);
                    cmd.Parameters.AddWithValue("@AccountManagerId", CRMInquirys.AccountManagerId);
                    // Rev Sanchita
                    cmd.Parameters.AddWithValue("@TypeOfCustomer", CRMInquirys.TypeOfCustomer);
                    // End of Rev Sanchita

                    cmd.Parameters.AddWithValue("@GroupId", CRMInquirys.GroupId);

                    cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                    cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    output_LEAD = (string)cmd.Parameters["@result"].Value;
                    con.Dispose();

                    if (CRMInquirys.Action == "Add")
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("update crm_InquiryContact set entity_id='" + output_LEAD + "' where crm_Inquirycontact_internalId='" + output + "'");
                    }
                    return Convert.ToString("Data save");



                }
                catch (Exception ex)
                {
                    return "Please try again later.";
                }
            }
            return Convert.ToString("Data save");
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
            catch (Exception e)
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
        public DataSet EditCRMInquirys(crmInquirys CRMInquirys)
        {

            try
            {
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_InquiryCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@Crm_Inquiryid", CRMInquirys.Inquiry_Id);
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
        public string DeleteInquirys(crmInquirys CRMInquirys)
        {

            try
            {
                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_InquiryCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Crm_Inquiryid", CRMInquirys.Inquiry_Id);
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

        public string QualifiedInquirys(crmInquirys CRMInquirys)
        {
            try
            {
                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_Inquiry_CONVERSION", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Qualify");
                cmd.Parameters.AddWithValue("@Inquiry_ENTITY_ID", CRMInquirys.InquiryCode);
                cmd.Parameters.AddWithValue("@Inquiry_STATUS", CRMInquirys.Status_Id);
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

        public string LostInquirys(crmInquirys CRMInquirys)
        {

            try
            {
                //string returnmessage = "";
                //string output = string.Empty;
                //DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("PROC_CRM_InquiryCONTACTINSERT", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Action", "Lost");
                //cmd.Parameters.AddWithValue("@Crm_Inquiryid", CRMInquirys.Inquiry_Id);
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
                SqlCommand cmd = new SqlCommand("PRC_Inquiry_CONVERSION", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Lost");
                cmd.Parameters.AddWithValue("@Inquiry_ENTITY_ID", CRMInquirys.InquiryCode);
                cmd.Parameters.AddWithValue("@Inquiry_STATUS", 0);
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

        public string SelectedSourceType(crmInquirys CRMInquirys)
        {

            try
            {

                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_InquiryCONTACTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Lost");
                cmd.Parameters.AddWithValue("@Inquiry_ENTITY_ID", CRMInquirys.InquiryCode);
                cmd.Parameters.AddWithValue("@Inquiry_STATUS", 0);
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


        public List<crmInquiryMobileDetails> GetInquiryMobileDetails(string MobileNo)
        {

            try
            {
                List<crmInquiryMobileDetails> objlst = new List<crmInquiryMobileDetails>();
                string returnmessage = "";
                string output = string.Empty;
                DataTable dsInst = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_Inquiry_DETAILS", con);
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
                        crmInquiryMobileDetails objcrmInquiryMobileDetails = new crmInquiryMobileDetails();
                        objcrmInquiryMobileDetails.cnt_id = Convert.ToString(dr["cnt_id"]);
                        objcrmInquiryMobileDetails.Name = Convert.ToString(dr["NAME"]);
                        objcrmInquiryMobileDetails.Unique_id = Convert.ToString(dr["UNIQUE_ID"]);
                        objlst.Add(objcrmInquiryMobileDetails);
                    }


                }


                return objlst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        internal DataSet EditCRMLeadsInquiry(string Entity_id)
        {
            string output = string.Empty;
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_Inquiry_DETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GETENTITYDATA");
            cmd.Parameters.AddWithValue("@Entity_id", Entity_id);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            return dsInst;
        }
    }
    public class crmInquirysQualify
    {
        public string Inquiry_Id { get; set; }
        public List<V_UserLIst> ConverttoUsers { get; set; }
        public string Remarks { get; set; }
    }

    public class crmInquiryMobileDetails
    {
        public string cnt_id { get; set; }
        public string Name { get; set; }
        public string Unique_id { get; set; }
    }



    public class crmInquiryValuesModel
    {
        public string id { get; set; }
        public string Name { get; set; }

    }


    public class crmAddProd
    {
        public string guid { get; set; }
        public int ActivityId { get; set; }
        public string Lead_Entity_id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Remarks { get; set; }
        public string Frequency { get; set; }
        public decimal Amount { get; set; }
    }

}