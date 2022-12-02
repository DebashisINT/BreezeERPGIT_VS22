using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvServiceDataBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataSet GetServiceEntryList()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryDetails");
            dt = proc.GetDataSet();
            return dt;
        }

        public DataSet GetServiceEntryByReceptChallanid(String ReceiptChallan_ID, String User_id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 300, "ReceiptChalanHeader");
            proc.AddVarcharPara("@ReceiptChallan_ID", 100, ReceiptChallan_ID);
            proc.AddVarcharPara("@USER_ID", 100, User_id);
            ds = proc.GetDataSet();
            return ds;
        }
    }

    public class SRV_ServiceData
    {
        public string TOTAL { get; set; }
        public string Assigned { get; set; }
        public string Unassigned { get; set; }

        //Mantis Issue 24665
        public string RepairPending { get; set; }
        //End of Mantis Issue 24665
        public List<SRV_ServiceDataList> DetailsList { get; set; }
    }

    public class SRV_ServiceDataList
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
        public string ServEnteredBy { get; set; }
        public string ServEnteredOn { get; set; }
        public String Status { get; set; }
        public String Action { get; set; }
        //Mantis Issue 24840
        public string Repair_date { get; set; }
        public string RepairStatus { get; set; }
        //End of Mantis Issue 24840
    }

    public class srv_RcptChallanDtls
    {
        public string RcptChallan_ID { get; set; }
        public string RcptChallanDtls_ID { get; set; }
        public string Rcpt_Model { get; set; }
        public string SerialNo { get; set; }
        public string ProblemReported { get; set; }
        public string ServiceAction { get; set; }
        public string Component { get; set; }
        public string Warrenty { get; set; }
        public string StockEntry { get; set; }
        public string Entry_Model { get; set; }
        public string NewSerialNo { get; set; }
        public string ProblemFound { get; set; }
        public string Remarks { get; set; }
        public string WarrentyStatus { get; set; }
        public string Action { get; set; }

        public string ReturnReasonID { get; set; }
        public string IsBillable { get; set; }
        public string ReturnReason { get; set; }
        public string Billable { get; set; }

        public string ServiceActionText { get; set; }

        public string Reason { get; set; }

        // Mantis Issue 25172
        public string LevelID { get; set; }
        public string LevelDesc { get; set; }
        // End of Mantis Issue 25172
    }


    public class srv_AddServiceEntryInput
    {
        public string RcptChallan_ID { get; set; }
        public string RcptChallanDtls_ID { get; set; }
        public string ServiceAction { get; set; }
        public string Component { get; set; }
        public string Warrenty { get; set; }
        public string StockEntry { get; set; }
        public string Entry_Model { get; set; }
        public string NewSerialNo { get; set; }
        public string ProblemFound { get; set; }
        public string Remarks { get; set; }
        public string WarrentyStatus { get; set; }
        public string ServiceActionID { get; set; }
        public string ComponentID { get; set; }
        public string StockEntryID { get; set; }
        public string Entry_ModelID { get; set; }
        public string ProblemFound_ID { get; set; }
        public string WarrentyStatusID { get; set; }

        public string ReturnReasonID { get; set; }
        public string IsBillable { get; set; }
        public string ReturnReason { get; set; }
        public string Billable { get; set; }

        public string Reason { get; set; }
        // Mantis Issue 25172
        public string LevelID { get; set; }
        public string LevelDesc { get; set; }
        // End of Mantis Issue 25172

        public List<ComponentKeyValue> com_qty { get; set; }
    }

    public class Srv_ServiceEntryHistory
    {
        public String EntityCode { get; set; }
        public String ReceiptNo { get; set; }
        public String ServiceAction { get; set; }
        public String Remarks { get; set; }
        public String Billable { get; set; }
    }

    public class Srv_ServiceComponentList
    {
        public String Productid { get; set; }
        public String ProductName { get; set; }
        public String ProductCode { get; set; }
        public String Replaceable { get; set; }
        public String Quantity { get; set; }
        public String TEXTBOX { get; set; }
    }

    public class ComponentKeyValue
    {
        public String id { get; set; }
        public String Value { get; set; }
    }
}
