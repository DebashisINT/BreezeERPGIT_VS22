using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class EnquiriesDet
    {
        public string Action_type { get; set; }
        public DateTime Date { get; set; }  
        public string Customer_Name { get; set; }
        public string Contact_Person { get; set; }
        public string PhoneNo  { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
             ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }
        public string Location { get; set; }
        public string Product_Required { get; set; }
        public string Qty { get; set; }
        public decimal Order_Value { get; set; }
        public string Enq_Details { get; set; }
        public string vend_type { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }
        public string HeaderName { get; set; }
        public string EnquiriesFrom { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int is_pageload { get; set; }
        public List<EnquriesFrom> enqfrom { get; set; }
        public bool SUPERVISOR { get; set; }
        public bool SALESMAN { get; set; }
        public bool VERIFY { get; set; }

        public UserRightsForPage UserRightsForPage { get; set; }
    }
    public class Msg
    {
        public string response_code { get; set; }
        public string response_msg { get; set; }
    }

    public class EnquriesFrom
    {
        public string EnqName { get; set; }
        public int EnqId { get; set; }
    }
    public class EnquiriesSupervisorFeedback
    {
        public string Unique_ID { get; set; }
        public DateTime date {get;set;}
        public string source {get; set;}
        public int IndustryId {get; set;}
        public string Misc_comments {get; set;}
        public int enq_priorityID { get; set; }
        public string priorityName { get; set; }
        public string checkedcustomer { get; set; }
        public int created_by  {get; set;}
        public string created_date { get; set; }
        public IEnumerable<Industry> listIndustry { get; set; }
        public IEnumerable<Priority> listpriority { get; set; }
        public IEnumerable<ExistOrNotCustomer> listExistOrNotCustomer { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }
    }
    public class Priority
    {
        public int priorityID { get; set; }
        public string priorityName { get; set; }

    }
    public class Industry
    {
        public int Industry_Id { get; set; }
        public string Industry_Name { get; set; }
    }

    public class ExistOrNotCustomer
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class EnquiriesSalesmanFeedback
    {
        public string Unique_ID { get; set; }
        public string enq_prodreq { get; set; }
        public string feedback { get; set; }
        public int Final_IndustryId { get; set; }
        public string usefullid { get; set; }
        public IEnumerable<Usefull> isusefull{get;set;}
        public DateTime last_contactdate { get; set; }
        public DateTime next_contactdate { get; set; }
        public int Contractedby { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public IEnumerable<Industry> listIndustry { get; set; }
        public IEnumerable<Salesman> listsalesman { get; set; }
        //Rev 1.0  revison details in Enquiries Controller
        public IEnumerable<Employee> listemployee { get; set; }
        //End of Rev 1.0

        public string source { get; set; }
        public string Industry { get; set; }
        public string Misc_comments { get; set; }
        public string priorityName { get; set; }
        public string checkedcustomer { get; set; }

        public string response_code { get; set; }
        public string response_msg { get; set; }

    }
    public class EnquiriesShowHistorySalesman
    {
        public string Unique_ID { get; set; }
        public string enq_prodreq { get; set; }
        public string feedback { get; set; }
        public string Final_IndustryId { get; set; }
        public string usefullid { get; set; }
        public DateTime last_contactdate { get; set; }
        public DateTime next_contactdate { get; set; }
        public string Contactedby { get; set; }
        public DateTime created_date { get; set; }
    }

    public class Usefull
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class EnquiriesVerify
    {
        public string Unique_ID { get; set; }
        public string verify_by { get; set; }
        public DateTime verified_on { get; set; }
        public DateTime closure_date { get; set; }
        public IEnumerable<Employee> listemployee { get; set; }
        public int created_by { get; set; }
        public string created_date { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }
    }

    public class Employee
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class Salesman
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}