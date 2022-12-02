using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CutOff.Models
{
    public class PendingDocView
    {
        public List<pendingDocument> pendingDocument { get; set; }
        public List<NegativeProduct> NegativeProduct { get; set; }
    }

    public class  pendingDocument
    {
        public string DocumentNo { get; set; }
        public string Branch { get; set; }
        public DateTime? Date { get; set; }
        public String Name { get; set; }
    }
    public class NegativeProduct
    {
        public Int64 ProductId { get; set; }
        public string Product { get; set; }
        public string Branch { get; set; }
        public Decimal Quantity { get; set; }
        
    }
}