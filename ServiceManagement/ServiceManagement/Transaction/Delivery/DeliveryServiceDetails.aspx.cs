using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ServiceManagement.ServiceManagement.Transaction.Delivery
{
    public partial class DeliveryServiceDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["DataBase"] != null)
            {
                string masterDbanem = Request.QueryString["DataBase"];
                Session["ErpConnection"] = GetDefaultConnectionString(masterDbanem);
            }
        }
        public string GetDefaultConnectionString(string masterDbanem)
        {
            string output = "";
            try
            {
                output = GetConnectionString(Convert.ToString(masterDbanem));
            }

            catch (Exception es)
            {
                throw es;
            }
            return output;
        }
        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.ConnectTimeout = 0;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;
            return str;
        }
        [WebMethod]
        //for Activities
        public static object GetTopData(string FINYEAR, string COMPANYID, string FULLPATH, string RCID, string ISCREATEORPREVIEW)
        { 
            DataTable dt = new DataTable();
            List<TopDEtails> lEfficency = new List<TopDEtails>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);

            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_DeliveryChallanPrint_FromSMS", oSqlConnection);
             sqlcmd.Parameters.Add("@TABLENAME", "CompanyMaster");
            sqlcmd.Parameters.Add("@COMPANYID", COMPANYID);
            sqlcmd.Parameters.Add("@FINYEAR", FINYEAR);
            sqlcmd.Parameters.Add("@FULLPATH", FULLPATH);
            sqlcmd.Parameters.Add("@RCID", RCID);
            sqlcmd.Parameters.Add("@ISCREATEORPREVIEW", ISCREATEORPREVIEW);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();

            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TopDEtails()
                          {
                              cmp_Name = Convert.ToString(dr["cmp_Name"]),
                              upper_CompanyName = Convert.ToString(dr["upper_CompanyName"]),
                              Address = Convert.ToString(dr["Address"])
                          }).ToList();
            return lEfficency;
        }
        
        [WebMethod]
        //for Activities
        public static object GetTableData(string FINYEAR, string COMPANYID, string FULLPATH, string RCID, string ISCREATEORPREVIEW)
        {
            DataTable dt = new DataTable();
            List<TableClass> lEfficency = new List<TableClass>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);

            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_DeliveryChallanPrint_FromSMS", oSqlConnection);
             sqlcmd.Parameters.Add("@TABLENAME", "Details");
            sqlcmd.Parameters.Add("@COMPANYID", COMPANYID);
            sqlcmd.Parameters.Add("@FINYEAR", FINYEAR);
            sqlcmd.Parameters.Add("@FULLPATH", FULLPATH);
            sqlcmd.Parameters.Add("@RCID", RCID);
            sqlcmd.Parameters.Add("@ISCREATEORPREVIEW", ISCREATEORPREVIEW);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();

            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TableClass()
                          {
                              Details_ID = Convert.ToString(dr["Details_ID"]),
                              relation_Id = Convert.ToString(dr["relation_Id"]),
                              Model = Convert.ToString(dr["Model"]),
                              DeviceNumber = Convert.ToString(dr["DeviceNumber"]),
                              Service_Action = Convert.ToString(dr["Service_Action"]),
                              Remarks = Convert.ToString(dr["Remarks"]),
                              Billable = Convert.ToString(dr["Billable"]),
                              NewSerialNo = Convert.ToString(dr["NewSerialNo"]),
                              Warranty_Upto = Convert.ToString(dr["Warranty_Upto"]),
                              Reason = Convert.ToString(dr["Reason"]),
                              NewWarrantyDate = Convert.ToString(dr["NewWarrantyDate"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //for Activities 
        public static object ChallanDetailsData( string FINYEAR, string COMPANYID, string FULLPATH, string RCID, string ISCREATEORPREVIEW)
        {
            DataTable dt = new DataTable();
            List<ChallanDetailsClass> lEfficency = new List<ChallanDetailsClass>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);

            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_DeliveryChallanPrint_FromSMS", oSqlConnection);
             sqlcmd.Parameters.Add("@TABLENAME", "Header");
            sqlcmd.Parameters.Add("@COMPANYID", COMPANYID);
            sqlcmd.Parameters.Add("@FINYEAR", FINYEAR);
            sqlcmd.Parameters.Add("@FULLPATH", FULLPATH);
            sqlcmd.Parameters.Add("@RCID", RCID);
            sqlcmd.Parameters.Add("@ISCREATEORPREVIEW", ISCREATEORPREVIEW);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();

            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new ChallanDetailsClass()
                          {
                              Id = Convert.ToString(dr["Id"]),
                              Receipt_Challan_No = Convert.ToString(dr["Receipt_Challan_No"]),
                              EntityCode = Convert.ToString(dr["EntityCode"]),
                              LCO_Name = Convert.ToString(dr["LCO Name"]),
                              Contact_Name = Convert.ToString(dr["Contact Name"]),
                              Create_date = Convert.ToString(dr["Create_date"]),
                              branch_description = Convert.ToString(dr["branch_description"]),
                              DeliveredTo = Convert.ToString(dr["DeliveredTo"]),
                              DeliveredOn = Convert.ToString(dr["DeliveredOn"]),
                              Address = Convert.ToString(dr["Address"]),
                              Branch_Phone = Convert.ToString(dr["Branch_Phone"]),
                              Received_By	 = Convert.ToString(dr["Received By"]),
                              Total_STB = Convert.ToString(dr["Total STB"]),
                              cmp_bigLogo = Convert.ToString(dr["cmp_bigLogo"]),
                              Upload_Picture = Convert.ToString(dr["Upload_Picture"])
                          }).ToList();
            return lEfficency;
        }

    }
    public class TableClass
    {
        public string Details_ID	{ get; set; }
        public string relation_Id	{ get; set; }
        public string Model	{ get; set; }
        public string DeviceNumber	{ get; set; }
        public string Service_Action	{ get; set; }
        public string Remarks	{ get; set; }
        public string Billable	{ get; set; }
        public string NewSerialNo	{ get; set; }
        public string Warranty_Upto{ get; set; }	
        public string Reason	{ get; set; }
        public string NewWarrantyDate	{ get; set; }
    }

    public class ChallanDetailsClass
    { 
        public string Id { get; set; }
        public string Receipt_Challan_No	{ get; set; }
        public string EntityCode	{ get; set; }
        public string LCO_Name	{ get; set; }
        public string Contact_Name	{ get; set; }
        public string Create_date	{ get; set; }
        public string branch_description	{ get; set; }
        public string DeliveredTo	{ get; set; }
        public string DeliveredOn	{ get; set; }
        public string Address	{ get; set; }
        public string Branch_Phone	{ get; set; }
        public string Received_By	{ get; set; } 
        public string Total_STB	{ get; set; }
        public string cmp_bigLogo	{ get; set; }
        public string Upload_Picture { get; set; }
    }

    public class TopDEtails
    {
        public string cmp_id { get; set; }
        public string cmp_internalid { get; set; }
        public string cmp_Name { get; set; }
        public string cmp_parentid { get; set; }
        public string cmp_natureOfBusiness { get; set; }
        public string cmp_directors { get; set; }
        public string cmp_authorizedSignatories { get; set; }
        public string cmp_exchange { get; set; }
        public string cmp_registrationNo { get; set; }
        public string cmp_sebiRegnNo { get; set; }
        public string cmp_panNo { get; set; }
        public string cmp_serviceTaxNo { get; set; }
        public string cmp_salesTaxNo { get; set; }
        public string CreateDate { get; set; }
        public string  CreateUser	{ get; set; } 
        public string  LastModifyDate	{ get; set; } 
        public string  LastModifyUser	{ get; set; } 
        public string  cmp_DateIncorporation	{ get; set; } 
        public string  cmp_CIN	{ get; set; }
        public string    cmp_CINdt	{ get; set; } 
        public string  cmp_VregisNo	{ get; set; } 
        public string  cmp_VPanNo	{ get; set; } 
        public string  cmp_OffRoleShortName{ get; set; } 	
        public string  cmp_OnRoleShortName	{ get; set; } 
        public string  com_Add	{ get; set; } 
        public string  com_logopath	{ get; set; } 
        public string  cmp_currencyid	{ get; set; } 
        public string  cmp_KYCPrefix	{ get; set; } 
        public string  cmp_KRAIntermediaryID	{ get; set; } 
        public string  cmp_LedgerView	{ get; set; } 
        public string  cmp_CombinedCntrDate { get; set; } 	
        public string  cmp_CombCntrNumber	{ get; set; } 
        public string  cmp_CombCntrReset	{ get; set; } 
        public string  cmp_CombCntrOrder	{ get; set; } 
        public string  cmp_vat_no	{ get; set; } 
        public string  cmp_EPFRegistrationNo	{ get; set; } 
        public string  cmp_EPFRegistrationNoValidfrom	{ get; set; } 
        public string  cmp_EPFRegistrationNoValidupto	{ get; set; } 
        public string  cmp_ESICRegistrationNo	{ get; set; } 
        public string  cmp_ESICRegistrationNoValidfrom	{ get; set; } 
        public string  cmp_ESICRegistrationNoValidupto	{ get; set; } 
        public string  onrole_schema_id	{ get; set; } 
        public string  offrole_schema_id	{ get; set; } 
        public string  cmp_bigLogo	{ get; set; } 
        public string  cmp_smallLogo	{ get; set; } 
        public string  cmp_gstin	{ get; set; } 
        public string  cmp_tin_no	{ get; set; } 
        public string  deductcat_value	{ get; set; } 
        public string  MSME_UdyamRCNo	{ get; set; } 
        public string  Upload_Picture	{ get; set; } 
        public string  upper_CompanyName	{ get; set; } 
        public string  phone	{ get; set; } 
        public string  email	{ get; set; } 
        public string  Address	{ get; set; } 
        public string  add_city	{ get; set; } 
        public string  add_pin { get; set; } 
    } 
}