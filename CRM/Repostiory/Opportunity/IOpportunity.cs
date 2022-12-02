using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Repostiory.Opportunity
{
    public interface IOpportunity
    {
        List<QuotationDetailsList> getVerifyId(string _AccountID);
    }
}