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
    public class crmCases
    {
        public Int64 CASE_ID { get; set; }
        public string TITLE { get; set; }
        public string CODE { get; set; }
        public string SUBJECTS { get; set; }
       


        public String Customer { get; set; }

        public String Customer_ID { get; set; }
        public Int64 ORIGIN_ID { get; set; }
        public string CONTACT { get; set; }
        public string ENTITILEMENT { get; set; }
        public string PRODUCT { get; set; }
        public string DESCRIPTIONS { get; set; }

        public string ResponseSent { get; set; }
        public Int64 RESPONSED_BY { get; set; }
        public Int64 RESOLVED_BY { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string LANDMARK { get; set; }
        public string PIN_ID { get; set; }
        public Int64 CITY_ID { get; set; }
        public Int64 STATE_ID { get; set; }
        public Int64 COUNTRY_ID { get; set; }
        public string PIN_Code { get; set; }
        public string CITY_Name { get; set; }
        public string STATE_Name { get; set; }
        public string COUNTRY_Name { get; set; }
        public Int64 OWNER_ID { get; set; }
        public Int64 ASSIGNED_ID { get; set; }
        public string EscalatedDate { get; set; }
        public List<v_crmContact> crmcontacts { get; set; }
        
        public List<V_CaseList> CasesList { get; set; }
        public string crmcontacts_id { get; set; }
        public Int64 CaseListId { get; set; }
        public string crmParentCase_id { get; set; }
        public Int64 STATUS_ID { get; set; }
        public string STATUS_DATE { get; set; }
        public string EST_CLOSE_DATE { get; set; }
        public string CASE_TYPE { get; set; }
        public string PARENT_ID { get; set; }
        public string Escalated { get; set; }
        public string Escalated_DATE { get; set; }
        public Int64 EsCalatedTo { get; set; }

        public string First_Response_Sent { get; set; }
        public string CREATED_BY { get; set; }
        public string Action { get; set; }
        public List<String> cntids { get; set; }
        public List<String> ParentCaseids { get; set; }
        public List<V_CNTACCOUNTLIST> V_CNTACCOUNTLISTS { get; set; }
        public List<V_UserLIst> Users { get; set; }

        public String ProductId { get; set; }
        public String ProductQty { get; set; }

        public String ProductPrice { get; set; }
        public String ProductAmount { get; set; }

        public DateTime? WarrStartdate { get; set; }
        public DateTime? WarrEnddate { get; set; }
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
        public DataTable GetServiceContactProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("CRM_CASE_Details");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@CaseId", DetailsID);
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetDetailsEntryListByID(String Action, Int64 DetailsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CRM_CASE_Details");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@CaseId", DetailsID);
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
                SqlCommand cmd = new SqlCommand("CRM_CASEADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Delete");

                cmd.Parameters.AddWithValue("@CASE_ID", Id);


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

        internal string SaveCase(Int64 CASE_ID, string Action, Int64 OWNER_ID, Int64 ASSIGNED_ID, Int64 STATUS_ID, string STATUS_DATE, string EST_CLOSE_DATE,
                string TITLE, string CODE, string SUBJECTS, string Customer_ID, Int64 ORIGIN_ID, string crmcontacts_id, string crmParentCase_id,
                Int64 RESPONSED_BY, string ResponseSent, Int64 RESOLVED_BY, string Escalated_DATE, Int64 EsCalatedTo, string ADDRESS1, string ADDRESS2,
                string ADDRESS3, string PIN_ID, Int64 CITY_ID, Int64 STATE_ID, Int64 COUNTRY_ID,DataTable ProductDt)
        {
            try
            {
                string output = string.Empty;

                if (ProductDt.Columns.Contains("HIddenID"))
                {
                    ProductDt.Columns.Remove("HIddenID");
                    ProductDt.AcceptChanges();
                }


                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("CRM_CASEADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CASE_ID", CASE_ID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@OWNER_ID", OWNER_ID);
                cmd.Parameters.AddWithValue("@ASSIGNED_ID", ASSIGNED_ID);
                cmd.Parameters.AddWithValue("@STATUS_ID", STATUS_ID);
                if (STATUS_DATE != null && STATUS_DATE != "")
                {
                    //cmd.Parameters.AddWithValue("@STATUS_DATE", STATUS_DATE);
                    cmd.Parameters.AddWithValue("@STATUS_DATE", DateTime.ParseExact(STATUS_DATE, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }

                if (EST_CLOSE_DATE != null && EST_CLOSE_DATE != "")
                {
                    //cmd.Parameters.AddWithValue("@EST_CLOSE_DATE", EST_CLOSE_DATE);
                    cmd.Parameters.AddWithValue("@EST_CLOSE_DATE", DateTime.ParseExact(EST_CLOSE_DATE, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@TITLE", TITLE);
                cmd.Parameters.AddWithValue("@CODE", CODE);
                cmd.Parameters.AddWithValue("@SUBJECTS", SUBJECTS);
                cmd.Parameters.AddWithValue("@CUSTOMER_ID", Customer_ID);
                cmd.Parameters.AddWithValue("@ORIGIN_ID", ORIGIN_ID);
                cmd.Parameters.AddWithValue("@Contacts", crmcontacts_id);
                cmd.Parameters.AddWithValue("@PARENT_ID", (crmParentCase_id));
                cmd.Parameters.AddWithValue("@RESPONSED_BY", RESPONSED_BY);
                cmd.Parameters.AddWithValue("@First_Response_Sent", ResponseSent);
                cmd.Parameters.AddWithValue("@RESOLVED_BY", RESOLVED_BY);


                if (Escalated_DATE != null && Escalated_DATE != "")
                {
                   // cmd.Parameters.AddWithValue("@Escalated_DATE", Escalated_DATE);
                    cmd.Parameters.AddWithValue("@Escalated_DATE", DateTime.ParseExact(Escalated_DATE, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@Escalted_To", EsCalatedTo);

                cmd.Parameters.AddWithValue("@ADDRESS1", ADDRESS1);
                cmd.Parameters.AddWithValue("@ADDRESS2", ADDRESS2);
                cmd.Parameters.AddWithValue("@ADDRESS3", ADDRESS3);
                cmd.Parameters.AddWithValue("@LANDMARK", LANDMARK);
                cmd.Parameters.AddWithValue("@PIN_ID", Convert.ToInt64(PIN_ID));
                cmd.Parameters.AddWithValue("@CITY_ID", CITY_ID);
                cmd.Parameters.AddWithValue("@STATE_ID", STATE_ID);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", COUNTRY_ID);

                //cmd.Parameters.AddWithValue("@CASE_TYPE", CASE_TYPE);
                //cmd.Parameters.AddWithValue("@DESCRIPTIONS", DESCRIPTIONS);
                //cmd.Parameters.AddWithValue("@ENTITILEMENT",ENTITILEMENT);
                //cmd.Parameters.AddWithValue("@Escalated", Escalated);
                //cmd.Parameters.AddWithValue("@PARENT_ID", PARENT_ID);
               cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
               cmd.Parameters.AddWithValue("@SeviceProduct", ProductDt);

               cmd.Parameters.Add("@result", SqlDbType.Char, 50);
               cmd.Parameters["@result"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return Convert.ToString("Data save successfully.");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        internal DataSet EditCase(crmCases newCRMcasesobj)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("CRM_CASEADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@CASE_ID", newCRMcasesobj.CASE_ID);
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

        public DataTable GetCustomerAddress(String Action, string CustomerId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("CRM_CASE_Details");
            proc.AddVarcharPara("@Action", 200, Action);
            proc.AddVarcharPara("@CustomerId", 100, CustomerId);
            ds = proc.GetTable();
            return ds;
        }

        internal string DeleteCase(crmCases newCRMCasesobj)
        {

            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("CRM_CASEADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@CASE_ID", newCRMCasesobj.CASE_ID);
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