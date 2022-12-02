using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvJobSheetEntryBL:IDisposable
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

    public class SrvJobSheetInput
    {
        public String Action { get; set; }
        public String NumberingScheme { get; set; }
        public String ChallanNumber { get; set; }
        public String RefJobsheet { get; set; }
        public String AssignTo { get; set; }
        public String WorkDoneOn { get; set; }
        public String Location { get; set; }
        public String HeaderRemarks { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public String SerialNumber { get; set; }
        public String DeviceType { get; set; }
        public String Model { get; set; }
        public String Problem { get; set; }
        public String OtherProblem { get; set; }
        public String ServiceAction { get; set; }
        public String Components { get; set; }
        public String Warranty { get; set; }
        public String ReturnReason { get; set; }
        public String NewModel { get; set; }
        public String DetailsRemarks { get; set; }
        public String Billable { get; set; }
        public String JobsheetID { get; set; }
        public String PostingDate { get; set; }

    }

    public class SrvJobSheetList
    {
        public String ChallanNumber { get; set; }
        public String RefJobsheet { get; set; }
        public String AssignTo { get; set; }
        public String WorkDoneOn { get; set; }
        public String Location { get; set; }
        public String EntityCode { get; set; }
        public String NetworkName { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public String Receivedby { get; set; }
        public String Receivedon { get; set; }
        public String Status { get; set; }
        public String Action { get; set; }
        public String PostingDate { get; set; }

    }
}
