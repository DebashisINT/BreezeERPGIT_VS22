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

namespace Payroll.Repostiory.payrollStructureMaster
{
    public class StructureLogic : IStructureLogic
    {
        public void StructureModify(payrollStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "AddStructure");
                cmd.Parameters.AddWithValue("@StructureName", model.StructureName);
                cmd.Parameters.AddWithValue("@StructureCode", model.StructureCode);
                cmd.Parameters.AddWithValue("@PayClassId", model._PClassId);
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
        public void DeleteStructure(payrollStructureEngine model, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
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
        public void PayheadSaveModify(payrollStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID, ref DataTable dt)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (Convert.ToString(model.PayHeadID) == "")
                {
                    cmd.Parameters.AddWithValue("@Action", "AddPayHead");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Action", "EditandSavePayHead");
                    cmd.Parameters.AddWithValue("@PayHeadID", model.PayHeadID);
                }               

                cmd.Parameters.AddWithValue("@StructureID", model.StructureID);
                cmd.Parameters.AddWithValue("@PayHeadName", model.PayHeadName);
                cmd.Parameters.AddWithValue("@PayHeadCode", model.PayHeadShortName);
                cmd.Parameters.AddWithValue("@PayType", model.PayType);
                cmd.Parameters.AddWithValue("@DataType", model.DataType);
                cmd.Parameters.AddWithValue("@IsProrataCalculated", model.IsProrataCalculated);
                cmd.Parameters.AddWithValue("@CalculationType", model.CalculationType);
                cmd.Parameters.AddWithValue("@RoundingOffType", model.RoundOffType);
                cmd.Parameters.AddWithValue("@CalculateFormula", model.Cal_CalculateFormula);

                cmd.Parameters.AddWithValue("@Comments", model.Comments);

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
            proc.AddVarcharPara("@Formula", 4000, strFormula);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckFormula(string strStructureID, string strFormula)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
            proc.AddVarcharPara("@Action", 100, "CheckFormulaByStructure");
            proc.AddVarcharPara("@StructureID", 100, strStructureID);
            proc.AddVarcharPara("@Formula", 4000, strFormula);
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
        public DataSet GetOnceEmployeeHOPDetails(string strStructureID, string CalculationType)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
                proc.AddVarcharPara("@Action", 100, "GetOnceEmployeeHOPDetails");
                proc.AddVarcharPara("@StructureID", 100, strStructureID);
                proc.AddVarcharPara("@CalculationType", 100, CalculationType);
                ds = proc.GetDataSet();
                return ds;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAlwaysEmployeeHOPDetails(string strStructureID, string CalculationType)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_PayStructureDetails");
                proc.AddVarcharPara("@Action", 100, "GetAlwaysEmployeeHOPDetails");
                proc.AddVarcharPara("@StructureID", 100, strStructureID);
                proc.AddVarcharPara("@CalculationType", 100, CalculationType);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void SavePayrollDetails(string Type, DataTable dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_ModifyPayrollData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@dt_PayrollData", dt);

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
        public string DeletePayHead(string ActionType, string id, ref int ReturnCode, string StructureID, ref DataTable dt)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataSet dsInst = new DataSet();

            try
            {
                
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_PayStructureModify";
                    paramList.Add(new KeyObj("@Action", ActionType));
                    paramList.Add(new KeyObj("@StructureID", StructureID));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@PayHeadID", id));

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
                    execProc.param = paramList;
                    dsInst = execProc.ExecuteProcedureGetDataSet();
                    paramList.Clear();
                    //NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                    if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                    {
                        dt = dsInst.Tables[0];
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
        public DataTable EditPayHead(string PayHeadID)
        {
            string output = string.Empty;
            string action = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_PayStructureDetails";
                    paramList.Add(new KeyObj("@Action", "EditPayHead"));
                    paramList.Add(new KeyObj("@PayHeadID", PayHeadID));

                    execProc.param = paramList;
                    dt = execProc.ExecuteProcedureGetTable();
                    paramList.Clear();
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }

            return dt;
        }
        public void SaveReportWidgets(string StructureID, DataTable Allowance_dt, DataTable Deduction_dt, DataTable Others_dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_PayStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ReportWidgets");
                cmd.Parameters.AddWithValue("@StructureID", StructureID);
                cmd.Parameters.AddWithValue("@Allowance_dt", Allowance_dt);
                cmd.Parameters.AddWithValue("@Deduction_dt", Deduction_dt);
                cmd.Parameters.AddWithValue("@Others_dt", Others_dt);

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

        public DataSet GetImportPayHead(DataTable dt, Int64 UserID, String PayHeadID, String StructureID)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_PayHeadImport");
                proc.AddPara("@StructureID", StructureID);
                proc.AddPara("@PayHeadID", PayHeadID);
                proc.AddPara("@UserID", UserID);
                proc.AddPara("@udtPayHead", dt);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SaveImportPayrollDetails(String Type, String PayStructureID,DataTable dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_InsertPayrollImportData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@PayStructureID", PayStructureID);
                cmd.Parameters.AddWithValue("@dt_PayrollData", dt);

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

        public DataSet SaveMappingExcel(String Action, DataTable dt, String SettingName)
        {
            DataSet dsInst = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_ExcelMappingData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@SettingName", SettingName);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@dt_ExcelMappingData", dt);
                    }
                }

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
            return dsInst;
        }
    }
} 