using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvDeliveryBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataSet ShowReceiptChallanDetails(string ReceiptChallan_ID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "ShowReceiptChallanDetails");
            proc.AddPara("@ReceiptChallan_ID", ReceiptChallan_ID);
            ds = proc.GetDataSet();
            return ds;
        }
    }

    public class Srv_deliveryCount
    {
        public String TotalEntered { get; set; }
        public String TotalDelivered { get; set; }
        public String Pendingdelivery { get; set; }
    }

    public class Srv_DeliveryList
    {
        public String ReceiptChallan_ID { get; set; }
        public String ReceiptChallan { get; set; }
        public String Type { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactPerson { get; set; }
        public String Technician { get; set; }
        public String Location { get; set; }
        public String Receivedby { get; set; }
        public String Receivedon { get; set; }
        public String Assignedby { get; set; }
        public String Assignedon { get; set; }
        public String ServEnteredBy { get; set; }
        public String ServEnteredOn { get; set; }
        public String DeliveryBy { get; set; }
        public String DeliveryOn { get; set; }
        public String UpdateBy { get; set; }
        public String UpdateOn { get; set; }
        public String Status { get; set; }
        public String Action { get; set; }

        public String Warranty { get; set; }

        public String ConfirmDelivery { get; set; }

        public String ConfirmDeliveryDate { get; set; }

        public String DocumentDate { get; set; }
    }

    public class srv_DeliveryHeadr
    {
        public String DocumentNumber { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactPerson { get; set; }
        public String ReceivedOn { get; set; }
        public String ReceivedBy { get; set; }
        public String AssignedTo { get; set; }
        public String AssignedBy { get; set; }
        public String AssignedOn { get; set; }
        public List<Srv_DeliveryDetails> DetailsList { get; set; }
    }

    public class Srv_DeliveryDetails
    {
        public String Model { get; set; }
        public String DeviceNumber { get; set; }
        public String ProblemDesc { get; set; }
        public String SrvActionDesc { get; set; }
        public String NewSerialNo { get; set; }
        public String Warrenty { get; set; }
        public String CordAdaptor_Status { get; set; }
        public String Remote_Status { get; set; }
        public String DeviceType { get; set; }
    }

    public class srv_DeliverySearch
    {
        public String TotalEntered { get; set; }
        public String TotalDelivered { get; set; }
        public String Pendingdelivery { get; set; }
        public List<Srv_DeliveryList> DetailsList { get; set; }
    }

    public class srv_DeliveryHeadrView
    {
        public String DocumentNumber { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactPerson { get; set; }
        public String ReceivedOn { get; set; }
        public String ReceivedBy { get; set; }
        public String AssignedTo { get; set; }
        public String AssignedBy { get; set; }
        public String AssignedOn { get; set; }
        public String DeliveryTo { get; set; }
        public String ContactNo { get; set; }
        public String Remarks { get; set; }
        public String isRcptChallanNotReceived { get; set; }
        public String ReceiptRemarks { get; set; }
        public String DeliveryID { get; set; }
        public String Attachment { get; set; }
        public List<Srv_DeliveryDetails> DetailsList { get; set; }
    }

    public class SerialSearch
    {
        public String ChallanNo { get; set; }
        public String SearialNo { get; set; }
        public String Model { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ProblemReported { get; set; }
        public String ServiceAction { get; set; }
        public String Component { get; set; }
        public String Warranty { get; set; }
        public String StockEntry { get; set; }
        public String NewSerialNo { get; set; }
        public String ItemModel { get; set; }
        public String ReturnReason { get; set; }
        public String Billable { get; set; }
        public String ProblemFound { get; set; }
        public String Remarks { get; set; }
        public String WarrantyStatus { get; set; }
        public String Status { get; set; }
        public String Action { get; set; }

        public String Technician { get; set; }

        public String NewWarranty { get; set; }

        public String ConfirmDelivery { get; set; }
        public String ConfirmDeliveryDate { get; set; }
        //Rev Add new column Tanmoy
        public String UnbillableReason { get; set; }
        //End of Add new column Tanmoy

        //Rev Add new column Tanmoy
        public String DocumentDate { get; set; }
        //End of Add new column Tanmoy
        // Mantis Issue 24265
        public String Location { get; set; }
        // End of Mantis Issue 24265
    }

    public class SRV_ReceptChallanHeader
    {
        public string EntryType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDate { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string Branch { get; set; }
        public string ContactNo { get; set; }
        public string Technician { get; set; }
        public List<srv_ReceptChallanDetails> DetailsList { get; set; }
    }

    public class srv_ReceptChallanDetails
    {
        public string Model { get; set; }
        public string DeviceNumber { get; set; }
        public string Warranty { get; set; }
        public string Remarks { get; set; }
        public string DeviceType { get; set; }
        public string Problem { get; set; }
        public string Remotes { get; set; }
        public string CardAdaptor { get; set; }
    }

    public class SRV_SearchServiceEntryView
    {
        public string ChallanNo { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string ReceivedOn { get; set; }
        public string ReceivedBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedOn { get; set; }
        public List<SRV_SearchServiceEntryViewDetails> DetailsList { get; set; }
    }

    public class SRV_SearchServiceEntryViewDetails
    {
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string ProblemReported { get; set; }
        public string ServiceAction { get; set; }
        public string Components { get; set; }
        public string Warranty { get; set; }
        public string StockEntry { get; set; }
        public string NewModel { get; set; }
        public string NewSerialNo { get; set; }
        public string ProblemFound { get; set; }
        public string Remarks { get; set; }
        public string WarrantyStatus { get; set; }
        public string ReturnReason { get; set; }
        public string Billable { get; set; }

        public string NewWarranty { get; set; }
    }

    public class SRV_SearchDeliveryView
    {
        public string ChallanNo { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string ReceivedOn { get; set; }
        public string ReceivedBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedOn { get; set; }
        public string DeliveredTo { get; set; }
        public string PhoneNo { get; set; }
        public string Remarks { get; set; }
        public List<SRV_SearchDeliveryViewDetails> DetailsList { get; set; }
    }

    public class SRV_SearchDeliveryViewDetails
    {
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string DeviceNumber { get; set; }
        public string Problemfound { get; set; }
        public string ServiceAction { get; set; }
        public string Warranty { get; set; }
        public string CardAdaptor { get; set; }
        public string Remotes { get; set; }
    }

    public class SRV_SearchJobSheetView
    {
        public string ChallanNumber { get; set; }
        public string PostingDate { get; set; }
        public string RefJobsheet { get; set; }
        public string AssignTo { get; set; }
        public string WorkDoneOn { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public List<SRV_SearchJobSheetViewDetails> DetailsList { get; set; }
    }

    public class SRV_SearchJobSheetViewDetails
    {
        public string SerialNumber { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string Problemfound { get; set; }
        public string Other { get; set; }
        public string ServiceAction { get; set; }
        public string Components { get; set; }
        public string Warranty { get; set; }
        public string ReturnReason { get; set; }
        public string Remarks { get; set; }
        public string Billable { get; set; }
    }

    public class SRV_MassConfirmShow
    {
        public String ReceiptChallan_ID { get; set; }
        public string EntryType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDate { get; set; }
        public string EntityCode { get; set; }
        public string NetworkName { get; set; }
        public string ContactPerson { get; set; }
        public string DeliveredTo { get; set; }
        public string DELIVERY_DATE { get; set; }
    }
}
