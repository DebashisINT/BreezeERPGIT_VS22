using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.prollLeaveApplication
{
    public class LeaveApplicationLogic:ILeaveApplicationLogic
    {
        public void LeaveApplicationModify(DataTable dtLeaveDetails, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveApplicationModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "AddApplication");
                cmd.Parameters.AddWithValue("@LeaveApplication", dtLeaveDetails);
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
        public void ApproveLeave(string LEAVE_IDS, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveApplicationModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ApproveLeaveDetails");
                cmd.Parameters.AddWithValue("@LEAVE_IDS", LEAVE_IDS);              

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
        public void RejectLeave(string LEAVE_IDS, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveApplicationModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "RejectLeaveDetails");
                cmd.Parameters.AddWithValue("@LEAVE_IDS", LEAVE_IDS);

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

       ///Ref Bapi
        public void LeaveApplicationEdit(List<classLeaveApplicationEdit> model, ref int strIsComplete, ref string strMessage)
        {
            try
            {


                //DateTime LeaveFromDate = DateTime.ParseExact(model.FirstOrDefault().LeaveFromDate, "yyyy-MM-dd HH:mm tt", null);
                //DateTime LeaveToDate = DateTime.ParseExact(model.FirstOrDefault().LeaveToDate, "yyyy-MM-dd HH:mm tt", null);

                model.FirstOrDefault().LeaveFromDate = model.FirstOrDefault().LeaveFromDate != "" ? setDateFormat(model.FirstOrDefault().LeaveFromDate).ToString("yyyy-MM-dd") : "";
                model.FirstOrDefault().LeaveToDate = model.FirstOrDefault().LeaveToDate != "" ? setDateFormat(model.FirstOrDefault().LeaveToDate).ToString("yyyy-MM-dd") : "";

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveApplicationEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@Doc_ID", model.FirstOrDefault().ApplicationID);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@EmployeeID", model.FirstOrDefault().EmployeeID);
                cmd.Parameters.AddWithValue("@LeaveStructureID", model.FirstOrDefault().LeaveStructureID);
                cmd.Parameters.AddWithValue("@LeaveApplicationNo", model.FirstOrDefault().LeaveApplicationNo);
                cmd.Parameters.AddWithValue("@LeaveApplicationDetails", model.FirstOrDefault().LeaveApplicationDetails);
                cmd.Parameters.AddWithValue("@LeaveID", model.FirstOrDefault().LeaveID);
                cmd.Parameters.AddWithValue("@DayPart", model.FirstOrDefault().DayPart);
                cmd.Parameters.AddWithValue("@LeaveFromDate", model.FirstOrDefault().LeaveFromDate);
                cmd.Parameters.AddWithValue("@LeaveToDate", model.FirstOrDefault().LeaveToDate);
                cmd.Parameters.AddWithValue("@ApplyDays", model.FirstOrDefault().ApplyDays);
                cmd.Parameters.AddWithValue("@LeaveReason", model.FirstOrDefault().LeaveReason);
             

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
        public static DateTime setDateFormat(string dt)
        {
            CultureInfo c = new CultureInfo("en-us");
            DateTime _dt = DateTime.Now;
            if (dt.Contains("/"))
                DateTime.TryParseExact(dt, "dd/MM/yyyy", c, DateTimeStyles.None, out _dt);
            else
                DateTime.TryParseExact(dt, "dd-MM-yyyy", c, DateTimeStyles.None, out _dt);
            return _dt;
        }
        public void DeleteLeaves(string DocID, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveApplicationEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Doc_ID", DocID);


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