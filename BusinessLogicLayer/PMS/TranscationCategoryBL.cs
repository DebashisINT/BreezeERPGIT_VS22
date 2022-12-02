using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.PMS
{
    public class TranscationCategoryBL
    {
        public DataSet DropDownDetailForTrans()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_MASTERVVALUE");
            ds = proc.GetDataSet();
            return ds;
        }

        public string SaveSkillData(string Trans_id, string TransName, string Branch, string BillingType)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INSERTTRANSCATIONCATEGORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TRANS_ID", Trans_id);
                cmd.Parameters.AddWithValue("@TRANS_NAME", TransName);
                cmd.Parameters.AddWithValue("@BRANCH", Branch);
                cmd.Parameters.AddWithValue("@BILLING_TYPE", BillingType);
                cmd.Parameters.AddWithValue("@CREATE_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@UPDATE_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables[0].Rows[0]["Count"].ToString()=="50")
                {
                    return "Name Already Exists.";
                }
                else
                {
                    return Convert.ToString("Saved Successfully.");
                }
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public DataTable GetTransList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_LIST_TRANSCATIONCATEGORY");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewTranscatin(string TRANS_ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_VIEWTRANSCATIONCATEGORY");
            proc.AddNVarcharPara("@TRANS_ID", 10, TRANS_ID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteTranscation(string TRANS_ID, ref String ReturnMsg)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_DELETETRANSCATIONCATEGORY");
            proc.AddNVarcharPara("@TRANS_ID", 10, TRANS_ID);
            proc.AddNVarcharPara("@ReturnMsg", 500, ReturnMsg, QueryParameterDirection.Output);
            ret = proc.RunActionQuery();
            ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
            return ret;
        }
    }
}
