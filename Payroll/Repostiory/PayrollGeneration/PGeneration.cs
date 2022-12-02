using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.PayrollGeneration
{
    public class PGeneration : IPGeneration
    {
        public string PGenerate(string ClassId, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_PayrollGeneration";
                    paramList.Add(new KeyObj("@ClassID", ClassId));
                    paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
                    paramList.Add(new KeyObj("@ReturnMessage", output, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    ReturnCode = Convert.ToInt32(execProc.outputPara[0].value);
                    output = Convert.ToString(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }

        public string PGenerateEmployeeWise(string ClassId,string emp, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;



            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_PayrollGenerationEmployeeWise";
                    paramList.Add(new KeyObj("@ClassID", ClassId));
                    paramList.Add(new KeyObj("@EMPLOYEECODES", emp));

                    paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
                    paramList.Add(new KeyObj("@ReturnMessage", output, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    ReturnCode = Convert.ToInt32(execProc.outputPara[0].value);
                    output = Convert.ToString(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }


        public void UndoSalaryGeneration(DataTable EmployeeCode, String ClassCode, String yymm, ref int ReturnCode, ref string ReturnMessage)
        {
            string output = string.Empty;
            string action = string.Empty;
           // int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    //int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    //ExecProcedure execProc = new ExecProcedure();
                    //List<KeyObj> paramList = new List<KeyObj>();
                    //execProc.ProcedureName = "proll_PayrollUndoGeneration";
                    //paramList.Add(new KeyObj("@EmployeeCode", EmployeeCode));
                    //paramList.Add(new KeyObj("@ClassCode", ClassCode));
                    //paramList.Add(new KeyObj("@yymm", yymm));
                    //paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
                    //paramList.Add(new KeyObj("@ReturnMessage", output, true));

                    //execProc.param = paramList;
                    //execProc.ExecuteProcedureNonQuery();
                    //paramList.Clear();
                    //NoOfRowEffected = execProc.NoOfRows;
                    //ReturnCode = Convert.ToInt32(execProc.outputPara[0].value);
                    //output = Convert.ToString(execProc.outputPara[1].value);

                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("proll_PayrollUndoGeneration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                    cmd.Parameters.AddWithValue("@ClassCode", ClassCode);
                    cmd.Parameters.AddWithValue("@yymm", yymm);
                  

                    cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);

                    cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);

                    ReturnCode = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                    ReturnMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                    cmd.Dispose();
                    con.Dispose();

                }


            }
            catch (Exception ex)
            {
                throw;
            }



            //return output;

        }
    }
}
