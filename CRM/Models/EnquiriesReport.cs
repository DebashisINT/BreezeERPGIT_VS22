using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLayer.CommonELS;

namespace CRM.Models
{
    public class EnquiriesReport
    {
        public DateTime Date { get; set; }
        public List<EnquriesFrom> enqfrom { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EnquiriesFrom { get; set; }
        public int is_pageload { get; set; }
        public UserRightsForPage UserRightsForPage { get; set; }

    }
}