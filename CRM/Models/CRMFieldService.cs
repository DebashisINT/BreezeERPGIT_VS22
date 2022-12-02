using CRM.Models.DataContext;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMFieldService
    {
        public string Action { get; set; }
        public string Lead_Id { get; set; }
        public Int64 ServiceCnt_Id { get; set; }
        public Int64 OwnerID { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 AssignedID { get; set; }
        public Int64 SourceID { get; set; }
        public Int64 RatingID { get; set; }
        public Int64 Status_Id { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public string LeadCode { get; set; }
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
        public Int64  Servicetype { get; set; }
       
        public string Technician { get; set; }
        public Int64 TechnicianId { get; set; }
        public Int64  JobId { get; set; }
        public string JobName { get; set; }
        public string Fault_Description { get; set; }
        public string Remarks { get; set; }
        public decimal Balance { get; set; }

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
        public string AssignerName { get; set; }
        public string LeadValuesInternalID { get; set; }
        public string LeadValuesInternalName { get; set; }
        public string LeadValuesType { get; set; }
        public string LeadValue { get; set; }
      
        public string Status { get; set; }
      
        public string cnt_shortName { get; set; }

        public String Customer { get; set; }

        public String Customer_ID { get; set; }


        public Int64 CASE_ID { get; set; }

        public String CaseName { get; set; }

        public string ServiceDate { get; set; }
        public string CallAttendDate { get; set; }
        public DateTime? WarrStartdate { get; set; }
        public DateTime? WarrEnddate { get; set; }
        public string CloseDate { get; set; }
        public string Expected_Reveneu { get; set; }
        public List<V_UserLIst> Users { get; set; }
        public List<V_PROJECTLISTFieldService> ProjectList { get; set; }
        public List<Technician_Report> TechList { get; set; }
        public List<V_UserLIst> Assigners { get; set; }
        public List<v_ContactSource> ContactSource { get; set; }
        public List<v_StatusDetail> Status_Details { get; set; }
        public List<V_Rating> Ratings { get; set; }
        public List<V_Industry> Industries { get; set; }
        public List<v_jobResponsibility> jobtitles { get; set; }
        public List<V_CaseList> Caseslist { get; set; }
        public List<v_crmContact> crmcontacts { get; set; }
        public List<String> cntids { get; set; }

        public string crmcontacts_id { get; set; }
        public String ProductId { get; set; }
        public String ProductQty { get; set; }

       

        public String ProductPrice { get; set; }
        public String ProductAmount { get; set; }

        
        public List<MarketingMaterials> marketingmaterials { get; set; }
        public List<v_Campaign> CampaignList { get; set; }


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

        public DataSet GetDetailsEntryListByID(String Action, Int64 DetailsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PROC_CRM_FieldCNTDetails");
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
                SqlCommand cmd = new SqlCommand("PROC_CRM_FieldServiceINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Delete");

                cmd.Parameters.AddWithValue("@FieldService_id", Id);


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
        public DataTable GetServiceContactProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_CRM_FieldCNTDetails");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsId", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public string SaveFieldService(Int64 ServiceCnt_Id, string Action, Int64 OwnerID, string ServiceStatusId, Int64  AssignedID, string  ServiceName, string UniqueId, string Customer_ID,
                           string ServiceDate,Int64 Servicetype, string CallAttendDate, Int64 TechnicianId, Int64 JobId, string crmcontacts_id,DataTable Bind_PRODUCTS,string Fault_Description
                           , decimal Balance, string CloseDate, string Remarks, Int64 CASE_ID, string CaseName)
        {

            try
            {
                string output = string.Empty;

                if (Bind_PRODUCTS.Columns.Contains("HIddenID"))
                {
                    Bind_PRODUCTS.Columns.Remove("HIddenID");
                    Bind_PRODUCTS.AcceptChanges();
                }



                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_FieldServiceINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", Action);

                cmd.Parameters.AddWithValue("@Service_OwnerId", Convert.ToInt64(OwnerID));
                cmd.Parameters.AddWithValue("@Service_AssignId", Convert.ToInt64(AssignedID));
                cmd.Parameters.AddWithValue("@Service_statusId", ServiceStatusId);
                cmd.Parameters.AddWithValue("@FieldService_id", ServiceCnt_Id);
                if (ServiceDate != null && ServiceDate != "")
                {
                    //cmd.Parameters.AddWithValue("@Service_RenewalDate", Convert.ToDateTime(RenewalDate));
                    cmd.Parameters.AddWithValue("@Service_Date", DateTime.ParseExact(ServiceDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@Service_Name", ServiceName);
                cmd.Parameters.AddWithValue("@Service_UniqueId", UniqueId);
                cmd.Parameters.AddWithValue("@Service_TypeId", Servicetype);
                cmd.Parameters.AddWithValue("@CustomerId", Customer_ID);
                if (CallAttendDate != null && CallAttendDate != "")
                {
                    // cmd.Parameters.AddWithValue("@Service_StartDate", Convert.ToDateTime(RenewalStartDate));
                    cmd.Parameters.AddWithValue("@CallAttend_Date", DateTime.ParseExact(CallAttendDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                if (CloseDate != null && CloseDate != "")
                {
                    cmd.Parameters.AddWithValue("@Close_Date", DateTime.ParseExact(CloseDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    //cmd.Parameters.AddWithValue("@Service_EndDate", Convert.ToDateTime(RenewalEndDate));
                }
                cmd.Parameters.AddWithValue("@TechnicianId", TechnicianId);
                cmd.Parameters.AddWithValue("@Job_Id", JobId);
                cmd.Parameters.AddWithValue("@Balance_amount", Convert.ToDecimal(Balance));
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Fault_Description", Fault_Description);
                cmd.Parameters.AddWithValue("@contacts", crmcontacts_id);
                cmd.Parameters.AddWithValue("@SeviceProduct", Bind_PRODUCTS);
                cmd.Parameters.AddWithValue("@CaseId", Convert.ToInt64(CASE_ID));
                cmd.Parameters.AddWithValue("@CaseName", CaseName);
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


    }
}