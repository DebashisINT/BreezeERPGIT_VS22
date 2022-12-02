using DataAccessLayer;
using DataAccessLayer.Model;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollEmpAttachment
{
    public class AttachmentLogic : IAttachmentLogic
    {
        public void AttachmentModify(EmployeeAttachmentEngine model, DataTable dt,int SundayCount, int MondayCount, int TuesdayCount, int WednesdayCount, int ThursdayCount,
            int FridayCount, int SaturdayCount, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
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

                cmd.Parameters.AddWithValue("@SundayCount", SundayCount);
                cmd.Parameters.AddWithValue("@MondayCount", MondayCount);
                cmd.Parameters.AddWithValue("@TuesdayCount", TuesdayCount);
                cmd.Parameters.AddWithValue("@WednesdayCount", WednesdayCount);
                cmd.Parameters.AddWithValue("@ThursdayCount", ThursdayCount);
                cmd.Parameters.AddWithValue("@FridayCount", FridayCount);
                cmd.Parameters.AddWithValue("@SaturdayCount", SaturdayCount);

                cmd.Parameters.AddWithValue("@RosterID", model.RosterID);
                cmd.Parameters.AddWithValue("@RosterFrom", model.RosterFrom);
                cmd.Parameters.AddWithValue("@RosterTo", model.RosterTo);

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
        public void AttachmentUpdate(EmployeeAttachmentEngine model, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_EmployeeAttactchment_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");               
               
                cmd.Parameters.AddWithValue("@Pay_ApplicationTo", model.Pay_ApplicationTo);               
                cmd.Parameters.AddWithValue("@Leave_ApplicationTo", model.Leave_ApplicationTo);
                cmd.Parameters.AddWithValue("@RosterTo", model.RosterTo);
                cmd.Parameters.AddWithValue("@ID", model.ID);

                cmd.Parameters.AddWithValue("@LeaveStructureCode", model.LeaveStructureCode);
                cmd.Parameters.AddWithValue("@RosterID", model.RosterID);

                cmd.Parameters.AddWithValue("@Leave_ApplicationFrom", model.Leave_ApplicationFrom);
                cmd.Parameters.AddWithValue("@RosterFrom", model.RosterFrom);

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
        
        public void DeleteStructure(string ID, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_EmployeeAttactchment_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@ID", ID);

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

        public DataTable EditAttachment(string AttachmentID)
        {
            string output = string.Empty;
            string action = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                //if (HttpContext.Current.Session["userid"] != null)
                //{
                   // int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_EmployeeAttactchment_Details";
                    paramList.Add(new KeyObj("@Action", "Details"));
                    paramList.Add(new KeyObj("@ID", AttachmentID));

                    execProc.param = paramList;
                    dt = execProc.ExecuteProcedureGetTable();
                    paramList.Clear();
               // }
            }
            catch (Exception ex)
            {
                dt = null;
            }

            return dt;
        }
    }
}