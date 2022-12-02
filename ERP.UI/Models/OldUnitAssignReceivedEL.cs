using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class OldUnitAssignReceivedEL
    {
    }
    public class tbl_trans_oldunit
    {
        public int Id { get; set; }
        public int Invoice_Id { get; set; }
        public string financial_year { get; set; }
        public string assign_from_branch { get; set; }
        public string assignee_remark { get; set; }
        public int assigned_by { get; set; }
        public string assign_to_branch { get; set; }
        public string receiver_remark { get; set; }
        public int received_by { get; set; }
        public DateTime assigned_on { get; set; }
        public DateTime received_on { get; set; }
        public int current_status { get; set; }
        public string company_Id { get; set; }
    }
}