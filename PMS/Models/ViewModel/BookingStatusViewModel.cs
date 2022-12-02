using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace PMS.Models.ViewModel
{
    public class BookingStatusViewModel
    {
        public string BOOKING_ID    { get; set; }
        public string BOOKING_NAME  { get; set; }
        public string BOOKING_TYPE  { get; set; }
        public string STATUS        { get; set; }
        public string DESCRIPTION { get; set; }
        public string BRANCH { get; set; }
        public string COLOR { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public List<Units> BranchList { get; set; }
        public List<Types> BOOKING_TYPEList { get; set; }
        public List<Statues> STATUSList { get; set; }
        public IList<SelectListItem> StatusNames { get; set; } 
    }

    public class Types
    {
        public string TYPEID { get; set; }
        public string TYPENAME { get; set; }
    }

    public class Statues
    {
        public string STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
    }

    public class bookingList
    {
        public long BOOKING_ID { get; set; }
        public string BOOKING_NAME { get; set; }
        public long BOOKING_TYPE { get; set; }
        public long STATUS { get; set; }
        public string DESCRIPTION { get; set; }
        public long BRANCH { get; set; }
        public string COLOR { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string CREATE_NAME { get; set; }
        public string UPDATE_NAME { get; set; }
        public string STATUS_NAME { get; set; }
        public string TYPENAME { get; set; }
    }
}