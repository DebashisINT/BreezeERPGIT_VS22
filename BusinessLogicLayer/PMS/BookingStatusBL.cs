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
   public class BookingStatusBL
    {
        public DataSet DropDownDetailForRole()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_BOOKINGSTATUS_BIND");
            ds = proc.GetDataSet();
            return ds;
        }

        public string SaveBookingData(string BOOKING_ID, string BOOKING_NAME, string BOOKING_TYPE, string STATUS, string DESCRIPTION, string BRANCH, string COLOR)
        {
            try
            {
                string returns = "";
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BOOKINGSTATUS_ADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BOOKING_ID", BOOKING_ID);
                cmd.Parameters.AddWithValue("@BOOKING_NAME", BOOKING_NAME);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE);
                cmd.Parameters.AddWithValue("@STATUS", STATUS);
                cmd.Parameters.AddWithValue("@DESCRIPTION", DESCRIPTION);
                cmd.Parameters.AddWithValue("@BRANCH", BRANCH);
                cmd.Parameters.AddWithValue("@COLOR", COLOR);
                cmd.Parameters.AddWithValue("@CREATE_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@UPDATE_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables[0]!=null && dsInst.Tables[0].Rows.Count>0)
                {
                    returns = dsInst.Tables[0].Rows[0]["msg"].ToString();
                    return returns;
                }
                return Convert.ToString("Data save");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public DataTable GetBookingList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_BOOKINGSTATUS_LIST");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewTranscatin(string BOOKING_ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_BOOKINGSTATUS_VIEW");
            proc.AddNVarcharPara("@BOOKING_ID", 10, BOOKING_ID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteBooking(string BOOKING_ID)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_BOOKINGSTATUS_DELETE");
            proc.AddNVarcharPara("@BOOKING_ID", 10, BOOKING_ID);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet DropDownDetailForStatus(string BOOKING_TYPEID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_BOOKINGSTATUS_status");
            proc.AddNVarcharPara("@BOOKING_TYPEID", 10, BOOKING_TYPEID);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
