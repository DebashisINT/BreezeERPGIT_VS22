using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.CustomerProfile
{
    public partial class customerProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

       [WebMethod]
        public static object GetCustomerProfileSearch()
        {
            List<GetCustomerProfileSearchClass> listCust = new List<GetCustomerProfileSearchClass>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable("select cnt_internalId, ISNULL(MCONT.cnt_firstName,'')+' '+ISNULL(MCONT.cnt_middleName,'')+'  '+ISNULL(MCONT.cnt_lastName,'') as 'Name', isnull(MADDR.add_Email,'') as Email, isnull(MADDR.add_Phone,'') as Phone from tbl_master_contact MCONT left outer join tbl_master_address MADDR on MADDR.add_cntId = MCONT.cnt_internalId and MADDR.add_addressType='Billing' where mcont.Is_Active=0");


                listCust = (from DataRow dr in cust.Rows
                            select new GetCustomerProfileSearchClass()
                            {
                                cnt_internalId = dr["cnt_internalid"].ToString(),
                                Name = dr["Name"].ToString(),
                                Email = Convert.ToString(dr["Email"]),
                                Phone = Convert.ToString(dr["Phone"])
                            }).ToList();
            }

            return listCust;
        }

        public class GetCustomerProfileSearchClass
        {

            public string cnt_internalId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }



        [WebMethod]
        public static object GetCustomer(string cntID)
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_Get_Customer_Profile", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "MAINTABLE");
            cmd.Parameters.AddWithValue("@CUSTOMER_ID", cntID);
            
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<GetCustomerClass> objleadbyIndustryClass = new List<GetCustomerClass>();
            objleadbyIndustryClass = (from DataRow dr in dsInst.Rows
                                      select new GetCustomerClass()
                                      {
                                          cnt_internalId = Convert.ToString(dr["cnt_internalId"]),
                                          Lead_Name = Convert.ToString(dr["Lead_Name"]),
                                          Mobile_no = Convert.ToString(dr["Mobile_no"]),
                                          Email = Convert.ToString(dr["Email"]),
                                          RATING = Convert.ToString(dr["RATING"]),
                                          Customer = Convert.ToString(dr["Customer"]),
                                          contactperson_Name = Convert.ToString(dr["contactperson_Name"]),
                                          contactperson_Email = Convert.ToString(dr["contactperson_Email"]),
                                          contactperson_Phone = Convert.ToString(dr["contactperson_Phone"]),
                                          Customer_Since = Convert.ToString(dr["Customer_Since"]),
                                          Client_Category = Convert.ToString(dr["Client_Category"]),
                                          Contract_date = Convert.ToString(dr["Contract_date"]),
                                          Type_Of_Service = Convert.ToString(dr["Type_Of_Service"]),
                                          Frequency = Convert.ToString(dr["Frequency"]),
                                          Value = Convert.ToString(dr["Value"]),
                                          Existing_Warrenty = Convert.ToString(dr["Existing_Warrenty"]),
                                          No_Of_Bills_Monthly = Convert.ToString(dr["No_Of_Bills_Monthly"]),
                                          No_Of_Service_Pointy = Convert.ToString(dr["No_Of_Service_Pointy"]),
                                          Next_Servcie_Schedule = Convert.ToString(dr["Next_Servcie_Schedule"]),
                                          Sales_Person = Convert.ToString(dr["Sales_Person"]),
                                          Custom_Cordinator = Convert.ToString(dr["Custom_Cordinator"]),
                                          Main_Service_Brannch = Convert.ToString(dr["Main_Service_Brannch"]),
                                          Collection_Cordinator = Convert.ToString(dr["Collection_Cordinator"]),
                                          Info_On_Outgoing_Work_Like_Termite = Convert.ToString(dr["Info_On_Outgoing_Work_Like_Termite"]),
                                          Service_Completed_For_tHe_Month = Convert.ToString(dr["Service_Completed_For_tHe_Month"]),
                                      }).ToList();
            return objleadbyIndustryClass;
        }

        public class GetCustomerClass 
        {

           public string cnt_internalId { get; set; }
           public string Lead_Name	 { get; set; }
           public string Mobile_no	 { get; set; }
           public string Email	 { get; set; }
           public string RATING	 { get; set; }
           public string Customer	 { get; set; }
           public string contactperson_Name	 { get; set; }
           public string contactperson_Email	 { get; set; }
           public string contactperson_Phone	 { get; set; }
           public string Customer_Since	 { get; set; }
           public string Client_Category	 { get; set; }
           public string Contract_date	 { get; set; }
           public string Type_Of_Service	 { get; set; }
           public string Frequency { get; set; }
           public string Value { get; set; }
           public string Existing_Warrenty	 { get; set; }
           public string No_Of_Bills_Monthly { get; set; }	
           public string No_Of_Service_Pointy	 { get; set; }
           public string Next_Servcie_Schedule	 { get; set; }
           public string Sales_Person	 { get; set; }
           public string Custom_Cordinator	 { get; set; }
           public string Main_Service_Brannch	 { get; set; }
           public string Collection_Cordinator	 { get; set; }
           public string Info_On_Outgoing_Work_Like_Termite	 { get; set; }
           public string Service_Completed_For_tHe_Month { get; set; }
        }


        [WebMethod]
        public static object GetCustomerService(string cntID) 
        {
            //campaignClass objcampaignClass = new campaignClass();
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_Get_Customer_Profile", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "SERVICE_STATUS_DETAILS");
            cmd.Parameters.AddWithValue("@CUSTOMER_ID", cntID);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            List<GetServiceClass> objleadbyIndustryClass = new List<GetServiceClass>();
            objleadbyIndustryClass = (from DataRow dr in dsInst.Rows
                                      select new GetServiceClass()
                                      {
                                          DETAILS_ID = Convert.ToString(dr["DETAILS_ID"]),
                                          SCHEDULE_ID = Convert.ToString(dr["SCHEDULE_ID"]),
                                          SCH_CODE = Convert.ToString(dr["SCH_CODE"]),
                                          CONTRACT_NO = Convert.ToString(dr["CONTRACT_NO"]),
                                          CUSTOMER = Convert.ToString(dr["CUSTOMER"]),
                                          SEGMENT1 = Convert.ToString(dr["SEGMENT1"]),
                                          SEGMENT2 = Convert.ToString(dr["SEGMENT2"]),
                                          SEGMENT3 = Convert.ToString(dr["SEGMENT3"]),
                                          SEGMENT4 = Convert.ToString(dr["SEGMENT4"]),
                                          SEGMENT5 = Convert.ToString(dr["SEGMENT5"]),
                                          SERVICE = Convert.ToString(dr["SERVICE"]),
                                          QUANTITY = Convert.ToString(dr["QUANTITY"]),
                                          UOM = Convert.ToString(dr["UOM"]),
                                          SCHEDULE_DATE = Convert.ToString(dr["SCHEDULE_DATE"]),
                                          ASSIGNEDBRANCH = Convert.ToString(dr["ASSIGNEDBRANCH"]),
                                          BRANCHASSIGNEDBY = Convert.ToString(dr["BRANCHASSIGNEDBY"]),
                                          BRANCH_ASSIGNED_ON = Convert.ToString(dr["BRANCH_ASSIGNED_ON"]),
                                          ACTUALASSIGNEDBRANCH = Convert.ToString(dr["ACTUALASSIGNEDBRANCH"]),
                                          BRANCHUNASSIGNEDBY = Convert.ToString(dr["BRANCHUNASSIGNEDBY"]),
                                          BRANCH_UNASSIGNED_ON = Convert.ToString(dr["BRANCH_UNASSIGNED_ON"]),
                                          ASSIGNEDTECHNICIAN = Convert.ToString(dr["ASSIGNEDTECHNICIAN"]),
                                          TECHNICIANASSIGNEDBY = Convert.ToString(dr["TECHNICIANASSIGNEDBY"]),
                                          TECHNICIAN_ASSIGNED_ON = Convert.ToString(dr["TECHNICIAN_ASSIGNED_ON"]),
                                          ACTUALASSIGNED_ECHNICIAN = Convert.ToString(dr["ACTUALASSIGNED_ECHNICIAN"]),
                                          TECHNICIANUNASSIGNEDBY = Convert.ToString(dr["TECHNICIANUNASSIGNEDBY"]),
                                          TECHNICIAN_UNASSIGNED_ON = Convert.ToString(dr["TECHNICIAN_UNASSIGNED_ON"]),
                                          sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                          sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                          sProducts_Description = Convert.ToString(dr["sProducts_Description"]),
                                          SCH_STATUS = Convert.ToString(dr["SCH_STATUS"]),
                                          SUB_TECHNICIANID = Convert.ToString(dr["SUB_TECHNICIANID"]),
                                          CUSTOMER_ID = Convert.ToString(dr["CUSTOMER_ID"]),
                                          PRODUCT_ID = Convert.ToString(dr["PRODUCT_ID"]),
                                          SUB_TECHNICIAN = Convert.ToString(dr["SUB_TECHNICIAN"]),
                                          ASSIGNED_BRANCH = Convert.ToString(dr["ASSIGNED_BRANCH"]),
                                          ACTUAL_ASSIGNED_BRANCH = Convert.ToString(dr["ACTUAL_ASSIGNED_BRANCH"]),
                                          ASSIGNED_TECHNICIAN = Convert.ToString(dr["ASSIGNED_TECHNICIAN"]),
                                          ACTUAL_ASSIGNED_TECHNICIAN = Convert.ToString(dr["ACTUAL_ASSIGNED_TECHNICIAN"]),
                                          HoldStyle = Convert.ToString(dr["HoldStyle"]),
                                          UnHoldStyle = Convert.ToString(dr["UnHoldStyle"]),
                                          STATUS = Convert.ToString(dr["STATUS"]),
                                      }).ToList();
            return objleadbyIndustryClass;
        }



        public class GetServiceClass
        {
            public string DETAILS_ID { get; set; }
            public string SCHEDULE_ID	{ get; set; }
            public string SCH_CODE	{ get; set; }
            public string CONTRACT_NO	{ get; set; }
            public string CUSTOMER	{ get; set; }
            public string SEGMENT1	{ get; set; }
            public string SEGMENT2	{ get; set; }
            public string SEGMENT3	{ get; set; }
            public string SEGMENT4	{ get; set; }
            public string SEGMENT5	{ get; set; }
            public string SERVICE	{ get; set; }
            public string QUANTITY	{ get; set; }
            public string UOM	{ get; set; }
            public string SCHEDULE_DATE	{ get; set; }
            public string ASSIGNEDBRANCH	{ get; set; }
            public string BRANCHASSIGNEDBY	{ get; set; }
            public string BRANCH_ASSIGNED_ON	{ get; set; }
            public string ACTUALASSIGNEDBRANCH	{ get; set; }
            public string BRANCHUNASSIGNEDBY { get; set; }	
            public string BRANCH_UNASSIGNED_ON	{ get; set; }
            public string ASSIGNEDTECHNICIAN	{ get; set; }
            public string TECHNICIANASSIGNEDBY	{ get; set; }
            public string TECHNICIAN_ASSIGNED_ON	{ get; set; }
            public string ACTUALASSIGNED_ECHNICIAN	{ get; set; }
            public string TECHNICIANUNASSIGNEDBY	{ get; set; }
            public string TECHNICIAN_UNASSIGNED_ON	{ get; set; }
            public string sProducts_Code	{ get; set; }
            public string sProducts_Name	{ get; set; }
            public string sProducts_Description	{ get; set; }
            public string SCH_STATUS	{ get; set; }
            public string SUB_TECHNICIANID { get; set; }	
            public string CUSTOMER_ID	{ get; set; }
            public string PRODUCT_ID	{ get; set; }
            public string SUB_TECHNICIAN	{ get; set; }
            public string ASSIGNED_BRANCH	{ get; set; }
            public string ACTUAL_ASSIGNED_BRANCH	{ get; set; }
            public string ASSIGNED_TECHNICIAN	{ get; set; }
            public string ACTUAL_ASSIGNED_TECHNICIAN	{ get; set; }
            public string HoldStyle	{ get; set; }
            public string UnHoldStyle{ get; set; }	
            public string STATUS { get; set; }

        }
    }
}