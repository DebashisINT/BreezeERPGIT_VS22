using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class TdsNillChallanBL
    {
        private object branch_id;
        //rev srijeeta
        //public void AddEditTDSNillChallanEntry(string Mode, string SectionID, string DeductionON, string Deposit_Date, string FinYear, string Quater,
        // string EntityType, string Surcharge, string eduCess, string Interest, string LateFees,
        // string Total_Amount, string Tax_Amount, string Others_Amount, string BankName, string BankBrach, string BRS, string ChallanNo, string TDSIDS
        // , string userId, ref int AdjustedId, ref string ReturnNumber, ref int ErrorCode, string TDS_id)
        
             public void AddEditTDSNillChallanEntry(string Mode, string SectionID, string DeductionON, string Deposit_Date, string FinYear, string Quater,
         string EntityType, string Surcharge, string eduCess, string Interest, string LateFees,
         string Total_Amount, string Tax_Amount, string Others_Amount, string BankName, string BankBrach, string BRS, string ChallanNo, string TDSIDS
         , string userId, ref int AdjustedId, ref string ReturnNumber, ref int ErrorCode, string TDS_id, string branch_id)
            //end of rev srijeeta

        {
            DataTable dsInst = new DataTable();


            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_TDSNilChallan", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", Mode);           
            cmd.Parameters.AddWithValue("@SectionID", SectionID);
            cmd.Parameters.AddWithValue("@DeductionON", DeductionON);            
            cmd.Parameters.AddWithValue("@tdsdt", Deposit_Date);
            cmd.Parameters.AddWithValue("@tdsfinyear", FinYear);
            cmd.Parameters.AddWithValue("@Quater", Quater);
            cmd.Parameters.AddWithValue("@Surcharge", Surcharge);
            cmd.Parameters.AddWithValue("@eduCess", eduCess);
            cmd.Parameters.AddWithValue("@Interest", Interest);
            cmd.Parameters.AddWithValue("@LateFees", LateFees);
            cmd.Parameters.AddWithValue("@Total", Total_Amount);
            cmd.Parameters.AddWithValue("@Tax", Tax_Amount);
            cmd.Parameters.AddWithValue("@Others", Others_Amount);
            cmd.Parameters.AddWithValue("@BankName", BankName);
            
            cmd.Parameters.AddWithValue("@BankBrach", BankBrach);        
            cmd.Parameters.AddWithValue("@BRS", BRS);
            cmd.Parameters.AddWithValue("@ChallanNo", ChallanNo);
            cmd.Parameters.AddWithValue("@Type", EntityType);  

            cmd.Parameters.AddWithValue("@TDSIDS", TDSIDS);

            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@TDS_id", TDS_id);
            cmd.Parameters.AddWithValue("@TDSNillChallan_ID", AdjustedId);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());           
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;
            //rev srijeeta 
            cmd.Parameters.AddWithValue("@branch_id ", branch_id);
            //end of rev srijeeta

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);

            cmd.Dispose();
            con.Dispose();
        }


        public int DeleteData(string ChallanId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_TDSNilChallan");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@TDSNillChallan_ID",500, ChallanId);
            return proc.RunActionQuery();
        }
        public DataSet GetEditedData(string TDSId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_TDSNilChallan");
            proc.AddVarcharPara("@Action", 50, "GETTDSHEADERDETAILSEDIT");
            proc.AddVarcharPara("@TDS_id", 10, TDSId);            
            return proc.GetDataSet();
        }

        public DataSet BindLoadData()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_TDSNilChallan");
            proc.AddVarcharPara("@Action", 50, "BindPageLoad");            
            return proc.GetDataSet();
        }
        public DataTable GetTDSPaymentEdit(DateTime TDSPaymentDate, string TDSCode, string TDSQuarter, string TDSYear, string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSNilChallan");
            proc.AddVarcharPara("@Action", 100, "BindTDSDetailsEditMode");
            proc.AddPara("@TDSPaymentDate", TDSPaymentDate);
            proc.AddPara("@TDSCode", TDSCode);
            proc.AddPara("@Quater", TDSQuarter);
            proc.AddPara("@Year", TDSYear);
            proc.AddPara("@Type", Type);

            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetTDSPayment(DateTime TDSPaymentDate, string TDSCode, string TDSQuarter, string TDSYear, string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSNilChallan");
            proc.AddVarcharPara("@Action", 100, "GETTDSDOCDETAILS");
            proc.AddPara("@TDSPaymentDate", TDSPaymentDate);
            proc.AddPara("@TDSCode", TDSCode);
            proc.AddPara("@Quater", TDSQuarter);
            proc.AddPara("@Year", TDSYear);
            proc.AddPara("@Type", Type);

            ds = proc.GetTable();
            return ds;
        }

        public object Branch { get; set; }

        public object BranchId { get; set; }
    }
}
