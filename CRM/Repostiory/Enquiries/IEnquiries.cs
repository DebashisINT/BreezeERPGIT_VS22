using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Data;
using System.Collections;


namespace CRM.Repostiory.Enquiries
{
    public interface IEnquiries
    {
        void save(EnquiriesDet apply, string uniqueID, ref int ReturnCode, ref string ReturnMsg);
        EnquiriesDet getEnquiryById(string _enquiryId, string EditFlag);
        EnquiriesSupervisorFeedback getSuperVisorById(string _enquiryId);
        EnquiriesSalesmanFeedback getSalesmanById(string _enquiryId);
        List<EnquiriesShowHistorySalesman> getShowHistorySalesmanById(string _enquiryId);
        EnquiriesVerify getVerifyId(string _enquiryId);
        string Delete(string ActionType, string id, ref int ReturnCode);
        string MassDelete(string ActionType, string id, ref int ReturnCode);
        void GetListing(string EnquiriesFrom, string FromDate, string ToDate);
        List<Industry> getIndustry();
        List<Employee> getEmployee();
        List<Salesman> getSalesman();
        void Supervisorsave(EnquiriesSupervisorFeedback apply, ref int ReturnCode, ref string ReturnMsg);
        void Salesmansave(EnquiriesSalesmanFeedback apply, ref int ReturnCode, ref string ReturnMsg);
        void Verifiedsave(EnquiriesVerify apply, ref int ReturnCode, ref string ReturnMsg);
    }
}
