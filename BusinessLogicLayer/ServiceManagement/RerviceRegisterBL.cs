using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class RerviceRegisterBL : IDisposable
    {
        public void Dispose()
        {

        }
    }

    public class ServiceRegisterReports
    {
        public List<Problem> ProblemList { get; set; }
        public List<EntityCodes> EntityCodeList { get; set; }
        public List<Modeles> ModelList { get; set; }
        public List<Tecchnician> TecchnicianList { get; set; }
        public List<Branches> BranchesList { get; set; }
    }

    public class Problem
    {
        public String ProblemID { get; set; }
        public String ProblemDesc { get; set; }
    }

    public class EntityCodes
    {
        public String EntityCode { get; set; }
    }

    public class Modeles
    {
        public String Model { get; set; }
    }

    public class Tecchnician
    {
        public String cnt_id { get; set; }
        public String cnt_firstName { get; set; }
    }

    public class Branches
    {
        public String branch_id { get; set; }
        public String branch_description { get; set; }
    }

    public class ReceiptChallanReports
    {
        public String ReceiptChallan { get; set; }
        public String Date { get; set; }
        public String EntryType { get; set; }
        public String Location { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactName { get; set; }
        public String ContactNo { get; set; }
        public String ProblemReported { get; set; }
        public String Cord { get; set; }
        public String Adapter { get; set; }
        public String Technician { get; set; }
        
        public String DeliveredTo { get; set; }
        public String Status { get; set; }

        public String ModelNo { get; set; }
        public String SerialNo { get; set; }
        public String ReceivedBy { get; set; }
        public String ReceivedOn { get; set; }
        public String AssignedBy { get; set; }
        public String AssignedOn { get; set; }
        public String ServicedBy { get; set; }
        public String ServicedOn { get; set; }
        public String DeliveredBy { get; set; }
        public String DeliveredOn { get; set; }

        public String ServiceAction { get; set; }
        public String Component { get; set; }
        public String ProblemFound { get; set; }
        public String ProblemRemarks { get; set; }
        public String Billable { get; set; }

        public String ConfirmDelivery { get; set; }
    }

    public class DeliveryReports
    {
        public String ReceiptChallan { get; set; }
        public String Date { get; set; }
        public String EntryType { get; set; }
        public String Location { get; set; }
        public String EntityCode { get; set; }
        public String Model { get; set; }
        public String Technician { get; set; }
        public String SerialNo { get; set; }
        public String ServiceAction { get; set; }        
        public String Warranty { get; set; }
        public String WarrantyStatus { get; set; }
        public String StockEntry { get; set; }
        public String NewModel { get; set; }
        public String NewSerialNo { get; set; }
        public String Billable { get; set; }
        public String ReturnReason { get; set; }
        public String ProblemFound { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public String Component { get; set; }
        public String ProblemReported { get; set; }

        public String ReceivedBy { get; set; }
        public String ReceivedOn { get; set; }
        public String AssignedBy { get; set; }
        public String AssignedOn { get; set; }
        public String ServicedBy { get; set; }
        public String ServicedOn { get; set; }
        public String DeliveredBy { get; set; }
        public String DeliveredOn { get; set; }

        public String NetworkName { get; set; }

        public String ConfirmDelivery { get; set; }
    }

    public class ServiceRegisterReportInput
    {
        //public List<String> Type { get; set; }
        public String Type { get; set; }
        public List<String> Report { get; set; }
        public String FromDate { get; set; }
        public String ToDate { get; set; }
        public String EntityCode { get; set; }
        public List<String> EntryType { get; set; }
        public String Model { get; set; }
        public String ProblemFound { get; set; }
        public String Technician { get; set; }
        public String Location { get; set; }
        public String IsBillable { get; set; }
        public String ProblemReported { get; set; }
        public String IsProbLemReport { get; set; }

        public String IsDelivery { get; set; }
    }
}