using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvAssignJobBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataTable GetAssignJobDetails(String userbranchHierachy)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@ACTION", 500, "DETAILS");
            proc.AddVarcharPara("@BranchID", 100, userbranchHierachy);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetTechnicianBind(string user_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@ACTION", 500, "TechnicianBind");
            proc.AddPara("@USER_ID", user_id);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetUserType(string user_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@ACTION", 500, "FindUserType");
            proc.AddPara("@USER_ID", user_id);
            dt = proc.GetTable();
            return dt;
        }

    }

    public class SRV_MassJobAssign
    {
        public string ReceiptChallan_ID { get; set; }
        public string EntryType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDate { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
    }

    public class SRV_MassInsert
    {
        public string Technician_ID { get; set; }
        public List<string> RecptID { get; set; }
    }

    public class SRV_JobCount
    {
        public string TOTAL { get; set; }
        public string Assigned { get; set; }
        public string Unassigned { get; set; }
        //Mantis Issue 24665
        public string RepairPending { get; set; }
        //End of Mantis Issue 24665
    }

    public class SRV_TotalJobsList
    {
        public String ReceiptChallan_ID { get; set; }
        public string ReceiptChallan { get; set; }
        public string Type { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string Technician { get; set; }
        public string Location { get; set; }
        public string Receivedby { get; set; }
        public string Receivedon { get; set; }
        public string Assignedby { get; set; }
        public string Assignedon { get; set; }
        public string Status { get; set; }
        public String Action { get; set; }
    }

    public class SRV_UnassignedJobsList
    {
        public String ReceiptChallan_ID { get; set; }
        public string ReceiptChallan { get; set; }
        public string Type { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string Location { get; set; }
        public string Receivedby { get; set; }
        public string Receivedon { get; set; }
        public string Status { get; set; }
        public String Action { get; set; }
    }

    public class srv_ReceptChallanDtls
    {
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string ReceivedBy { get; set; }
        public List<srv_ReceptChallanDtlsList> DetailsList { get; set; }
    }

    public class srv_ReceptChallanDtlsList
    {
        public String SLNO { get; set; }
        public string DeviceNumber { get; set; }
        public string ModelNumber { get; set; }
        public string Problem { get; set; }
        public string CordAdaptor { get; set; }
        public string Remote{get;set;}
        public string Others{get;set;}
    }

    public class srv_SearchFilterInput
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Branch { get; set; }
        public string SearchType { get; set; }
        public String Technician_ID { get; set; }
    }

    public class SRV_AssignMyJobData
    {
        public string TOTAL { get; set; }
        public string Assigned { get; set; }
        public string Unassigned { get; set; }
        public List<SRV_TotalJobsList> DetailsList { get; set; }
    }

    public class SRV_UnAssignJobData
    {
        public string TOTAL { get; set; }
        public string Assigned { get; set; }
        public string Unassigned { get; set; }
        public List<SRV_UnassignedJobsList> DetailsList { get; set; }
    }
}
