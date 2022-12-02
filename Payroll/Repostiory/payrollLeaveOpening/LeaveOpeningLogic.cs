using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollLeaveOpening
{
    public class LeaveOpeningLogic : ILeaveOpeningLogic
    {
        public DataSet GetEmployeeLeaveOpening(string strStructureID)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_LeaveBalanceCalculation");
                proc.AddVarcharPara("@Action", 100, "GetOpeningBalance");
                proc.AddVarcharPara("@LeaveStructureCode", 100, strStructureID);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void SaveOpeningBalance(DataTable dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveBalanceCalculation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ModifyOpeningBalance");
                cmd.Parameters.AddWithValue("@dt_OpeningData", dt);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}