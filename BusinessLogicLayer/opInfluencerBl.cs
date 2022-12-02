using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class opInfluencerBl
    {
        public DataSet GetInfluencerDetails(string InvoiceId,string connectionstring)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("prc_PosInfluencer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GetAllDetailsById");
            cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);
            cmd.Parameters.AddWithValue("@IsOpening", "1");

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(ds);
            cmd.Dispose();
            con.Dispose();
            return ds;
        }

        
    }
}
