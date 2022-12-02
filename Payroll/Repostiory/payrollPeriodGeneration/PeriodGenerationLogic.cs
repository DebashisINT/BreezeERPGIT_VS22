using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollPeriodGeneration
{
    public class PeriodGenerationLogic : IPeriodGeneration
    {
        public string setActivePrevNext(string ActionType, string PayClassID, ref string ActiveYYMM, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            
            try
            {
                if (ActionType == "activePrev")
                {
                    action = "ActivePrevious";
                }
                else if (ActionType == "activeNxt")
                {
                    action = "ActiveNext";
                }

                if (HttpContext.Current.Session["userid"] != null)
                {
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("proll_PeriodGenerationMOdify", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", action);
                    cmd.Parameters.AddWithValue("@PayClassID", PayClassID);

                    cmd.Parameters.Add("@ActiveYYMM", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);

                    cmd.Parameters["@ActiveYYMM"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);

                    ActiveYYMM = Convert.ToString(cmd.Parameters["@ActiveYYMM"].Value.ToString());
                    ReturnCode = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                    output = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                    cmd.Dispose();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return output;
        }
    }
}