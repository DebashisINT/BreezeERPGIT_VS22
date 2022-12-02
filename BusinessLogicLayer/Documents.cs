using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataAccessLayer;

/// <summary>
/// Summary description for Documents
/// </summary>
namespace BusinessLogicLayer
{
    public class Documents
    {
        public Documents()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet Get_DocumentMaster(string vDocumentFor, string vDocumentType, string vBranch, string vGroup, string vGroupType, string vClient,
                                  string vDateRange, string vDateRangeType, string vFromDate, string vToDate, string vparam)
        {


            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_DocumentMaster");

            proc.AddVarcharPara("@DocumentFor", 100, vDocumentFor);
            proc.AddVarcharPara("@DocumentType", 100, vDocumentType);
            proc.AddVarcharPara("@Branch", 100, vBranch);
            proc.AddVarcharPara("@Group", 100, vGroup);
            proc.AddVarcharPara("@GroupType", 100, vGroupType);
            proc.AddNVarcharPara("@Client", 100, vClient);
            proc.AddVarcharPara("@DateRange", 100, vDateRange);
            proc.AddVarcharPara("@DateRangeType", 100, vDateRangeType);
            proc.AddVarcharPara("@FromDate", 100, vFromDate);
            proc.AddVarcharPara("@ToDate", 100, vToDate);
            proc.AddNVarcharPara("@param", 100, vparam);

            ds = proc.GetDataSet();
            return ds;
        }
    }
}