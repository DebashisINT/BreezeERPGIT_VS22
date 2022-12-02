using DataAccessLayer;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.StructureMaster
{
    public class StructureLogic : IStructureLogic
    {
        public void StructureModify(PayStructureEngine model, ref int strIsComplete,ref string strMessage, ref string StructureID)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "AddStructure");
                cmd.Parameters.AddWithValue("@StructureName", model.StructureName);
                cmd.Parameters.AddWithValue("@StructureCode", model.StructureCode);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@ReturnDocID", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnDocID"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                StructureID = Convert.ToString(cmd.Parameters["@ReturnDocID"].Value.ToString());
               
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public void DeleteStructure(PayStructureEngine model, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DeleteStructure");
                cmd.Parameters.AddWithValue("@StructureID", model.StructureID);

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
        public void PayheadSaveModify(PayStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID, ref DataTable dt)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "AddPayHead");

                cmd.Parameters.AddWithValue("@StructureID", model.StructureID);
                cmd.Parameters.AddWithValue("@PayHeadName", model.PayHeadName);
                cmd.Parameters.AddWithValue("@PayHeadCode", model.PayHeadShortName);
                cmd.Parameters.AddWithValue("@PayType", model.PayType);
                cmd.Parameters.AddWithValue("@CalculationType", model.CalculationType);
                cmd.Parameters.AddWithValue("@RoundingOffType", model.RoundOffType);
               


                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@ReturnDocID", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnDocID"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                StructureID = Convert.ToString(cmd.Parameters["@ReturnDocID"].Value.ToString());

                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    dt = dsInst.Tables[0];
                }     

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable PopulatePayHead(string strStructureID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
            proc.AddVarcharPara("@Action", 100, "PopulatePayHead");
            proc.AddVarcharPara("@StructureID", 100, strStructureID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckFormula(string strFormula)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
            proc.AddVarcharPara("@Action", 100, "CheckFormula");
            proc.AddVarcharPara("@Formula", 100, strFormula);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetStructureDetails(string strStructureID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
            proc.AddVarcharPara("@Action", 100, "GetStructureDetails");
            proc.AddVarcharPara("@StructureID", 100, strStructureID);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}