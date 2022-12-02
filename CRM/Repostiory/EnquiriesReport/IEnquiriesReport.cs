using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Repostiory.EnquiriesReport
{
    public interface IEnquiriesReport
    {
        void GetEnqListing(string EnquiriesFrom, string FromDate, string ToDate);
        //Rev Subhra 11-04-2019
        string Restore(string ActionType, string id, ref int ReturnCode);
        string PermanentDelete(string ActionType, string id, ref int ReturnCode);
        //End of Rev Subhra

    }
}
