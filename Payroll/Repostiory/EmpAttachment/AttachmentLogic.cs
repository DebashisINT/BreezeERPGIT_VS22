using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.EmpAttachment
{
    public class AttachmentLogic : IAttachmentLogic
    {
        public void AttachmentModify(p_EmpAttactchmentEngine model, DataTable dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlCommand cmd = new SqlCommand("proll_EmployeeAttactchment_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Add");
                cmd.Parameters.AddWithValue("@PayStructureCode", model.PayStructureCode);
                cmd.Parameters.AddWithValue("@Pay_ApplicationFrom", model.Pay_ApplicationFrom);
                cmd.Parameters.AddWithValue("@Pay_ApplicationTo", model.Pay_ApplicationTo);
                cmd.Parameters.AddWithValue("@LeaveStructureCode", model.LeaveStructureCode);
                cmd.Parameters.AddWithValue("@Leave_ApplicationFrom", model.Leave_ApplicationFrom);
                cmd.Parameters.AddWithValue("@Leave_ApplicationTo", model.Leave_ApplicationTo);
                cmd.Parameters.AddWithValue("@dt_EmployeeList", dt);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));

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