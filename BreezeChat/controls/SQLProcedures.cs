using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web;
namespace BreezeChat.controls
{
    public class SQLProcedures
    {
        public SQLProcedures()
        {
        }


        public static string SqlConnectionString()
        {
            //string SqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString();//Default connection
            string SqlConnection = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")//DBReadOnlyConnection
                {
                    // SqlConnection = System.Configuration.ConfigurationManager.AppSettings["DBReadOnlyConnection"].ToString(); MULTI
                    SqlConnection = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }
            return SqlConnection;
        }



        public static DataTable SelectProcedure(string ProcedureName, string InputName, string OutputName, string InputType, string OutputType, string InputValue)
        {
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {
                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.
                if (InputType == "I")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName, SqlDbType.Int, 4));
                    //Assign the search value to the parameter.
                    MyDataAdapter.SelectCommand.Parameters["@" + InputName].Value = Convert.ToInt32(InputValue, 10);
                }
                else if (InputType == "V")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName, SqlDbType.VarChar, -1));
                    //Assign the search value to the parameter.
                    MyDataAdapter.SelectCommand.Parameters["@" + InputName].Value = InputValue;
                }
                else if (InputType == "T")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName, SqlDbType.Text, 20000000));
                    MyDataAdapter.SelectCommand.Parameters["@" + InputName].Value = InputValue;

                }

                //Create and add an output parameter to the Parameters collection. 
                if (OutputType == "I")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.Int, 4));
                }
                else if (OutputType == "V")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.VarChar, -1));
                }

                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;

                MyDataAdapter.Fill(DT);
            }
            catch
            {

            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }
            return DT;
        }


        public static DataTable SelectProcedureArr(string ProcedureName, string[] InputName, string OutputName, string[] InputType, string OutputType, string[] InputValue, ref int OutputValue)
        {

            //SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {
                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, -1));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 
                if (OutputType == "I")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.Int, 4));
                }
                else if (OutputType == "V")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.VarChar, -1));
                }
                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;

                MyDataAdapter.Fill(DT);
                OutputValue = Convert.ToInt32(MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Value);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }

            return DT;
        }

        public static DataSet SelectProcedureArrDS(string ProcedureName, string[] InputName, string OutputName, string[] InputType, string OutputType, string[] InputValue, ref string OutputValue)
        {
            //SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataSet DS = new DataSet();

            try
            {
                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, -1));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 
                if (OutputType == "I")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.Int, 4));
                }
                else if (OutputType == "V")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.VarChar, -1));
                }
                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;

                MyDataAdapter.SelectCommand.CommandTimeout = 0;
                MyDataAdapter.Fill(DS);
                OutputValue = Convert.ToString(MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Value);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }

            return DS;
        }

        public static DataTable SelectProcedureArr(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue)
        {
            //SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {

                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, -1));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 

                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                //MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;
                MyDataAdapter.SelectCommand.CommandTimeout = 0;
                MyDataAdapter.Fill(DT);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }

            return DT;
        }


        public static DataTable InsertProcedureArr(string ProcedureName, string[] InputName, string OutputName, string[] InputType, string OutputType, string[] InputValue, string tbl_name)
        {
            // SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {
                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, -1));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                }
                //Create and add an output parameter to the Parameters collection. 
                if (OutputType == "I")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.Int, 4));
                }
                else if (OutputType == "V")
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + OutputName, SqlDbType.VarChar, -1));
                }
                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;

                MyDataAdapter.Fill(DT);
            }
            catch
            {

            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }
            return DT;
        }

        public static DataSet SelectProcedureArrDS(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue)
        {
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            //SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataSet DS = new DataSet();

            try
            {

                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, -1));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "X")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.NText));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 

                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                //MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;
                MyDataAdapter.SelectCommand.CommandTimeout = 0;
                MyDataAdapter.Fill(DS);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();
            }

            return DS;
        }

        //New Procedure For Simple Execute a Procedure and get Value 1: For Success; and 0: For Failure;
        public static int Execute_StoreProcedure(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue, string[] InputSize)
        {
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            // SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {
                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, Convert.ToInt32(InputSize[LoopCnt])));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, Convert.ToInt32(InputSize[LoopCnt])));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                }
                MyDataAdapter.Fill(DT);
            }
            catch
            {
                return 0;
            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();

            }
            return 1;
        }
        //New Procedure For Simple Execute a Procedure and return integer value according your sp..
        public static int Execute_Return_StoreProcedure(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue, string[] InputSize)
        {
            SqlConnection MyConnection = new SqlConnection(SqlConnectionString());
            // SqlConnection MyConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, MyConnection);
            DataTable DT = new DataTable();

            try
            {
                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, Convert.ToInt32(InputSize[LoopCnt])));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, Convert.ToInt32(InputSize[LoopCnt])));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                }
                MyDataAdapter.Fill(DT);
            }
            catch
            {
                return 0;
            }
            finally
            {
                MyDataAdapter.Dispose();
                MyConnection.Close();

            }
            if (DT.Rows.Count > 0)
            {
                return Convert.ToInt32(DT.Rows[0][0].ToString());
            }
            return 1;
        }
    }
}