using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Dashboard_React.DAL
{
    public class ExecProcedure
    {
        public List<KeyObj> param { get; set; }
        public string ProcedureName { get; set; }
        public List<KeyValues> outputPara { get; set; }
        public int  NoOfRows { get; set; }
        
        public ExecProcedure(string PorcName)
        {
            ProcedureName = PorcName;
            this.outputPara = new List<KeyValues>();
        }
        public ExecProcedure() { }

        public async Task<DataTable> GetDatatable()
        {
            DataTable ReturnTable = await GetDatatableAsync();
            return ReturnTable;
        }
        private Task<DataTable> GetDatatableAsync()
        {
            return Task.Factory.StartNew(() => ExecuteProcedureGetTable());
        }
        public async Task<bool> ExecuteNonQuery()
        {
            bool ReturnBool = await ExecuteNonQueryAsync();
            return ReturnBool;
        }
        private Task<bool> ExecuteNonQueryAsync()
        {
            return Task.Factory.StartNew(() => ExecuteProcedureNonQuery());
        }
        public DataTable ExecuteProcedureGetTable()
        {
            SqlConnection Con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand(this.ProcedureName, Con);
            com.CommandTimeout = 0;
            SqlParameter opPara;
            com.CommandType = CommandType.StoredProcedure;
            foreach (KeyObj para in this.param)
            {
                if (para.isReturn == null)
                    com.Parameters.AddWithValue(para.key, para.Obj);
                else
                {
                    if (para.isReturn == true)
                    {
                        opPara = new SqlParameter();
                        opPara.ParameterName = para.key;
                        opPara.Value = para.Obj;
                        opPara.Size = 500;
                        opPara.Direction = ParameterDirection.Output;
                        com.Parameters.Add(opPara);
                    }
                    else
                        com.Parameters.AddWithValue(para.key, para.Obj);
                }
            }

            SqlDataAdapter sqlDA = new SqlDataAdapter(com);
            DataSet FinalDs = new DataSet();
            Con.Open();

            sqlDA.Fill(FinalDs);

            List<KeyValues> returnList = new List<KeyValues>();
            foreach (SqlParameter exp in com.Parameters)
            {
                if (exp.Direction == ParameterDirection.Output)
                    returnList.Add(new KeyValues(exp.ParameterName, Convert.ToString(exp.Value)));
            }
            this.outputPara = returnList;
            Con.Close();
            Con.Dispose();
            sqlDA.Dispose();
            return FinalDs.Tables[0];
        }
        public DataSet ExecuteProcedureGetDataSet()
        {
            SqlConnection Con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand(this.ProcedureName, Con);
            com.CommandTimeout = 0;
            SqlParameter opPara;
            com.CommandType = CommandType.StoredProcedure;
            foreach (KeyObj para in this.param)
            {
                if (para.isReturn == null)
                    com.Parameters.AddWithValue(para.key, para.Obj);
                else
                {
                    if (para.isReturn == true)
                    {
                        opPara = new SqlParameter();
                        opPara.ParameterName = para.key;
                        opPara.Value = para.Obj;
                        opPara.Direction = ParameterDirection.Output;
                        opPara.Size = 300;
                        com.Parameters.Add(opPara);
                    }
                    else
                        com.Parameters.AddWithValue(para.key, para.Obj);
                }
            }

            SqlDataAdapter sqlDA = new SqlDataAdapter(com);
            DataSet FinalDs = new DataSet();
            Con.Open();

            sqlDA.Fill(FinalDs);

            List<KeyValues> returnList = new List<KeyValues>();
            foreach (SqlParameter exp in com.Parameters)
            {
                if (exp.Direction == ParameterDirection.Output)
                    returnList.Add(new KeyValues(exp.ParameterName, Convert.ToString(exp.Value)));
            }
            this.outputPara = returnList;


            Con.Close();
            Con.Dispose();
            sqlDA.Dispose();
            return FinalDs;
        }
        public bool ExecuteProcedureNonQuery()
        {
            bool returnValue = true;
            SqlConnection Con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand(this.ProcedureName, Con);
            com.CommandTimeout = 0;
            SqlParameter opPara;
            com.CommandType = CommandType.StoredProcedure;

            try
            {
                foreach (KeyObj para in this.param)
                {
                    if (para.isReturn == null)
                        com.Parameters.AddWithValue(para.key, para.Obj);
                    else
                    {
                        if (para.isReturn == true)
                        {
                            opPara = new SqlParameter();
                            opPara.ParameterName = para.key;
                            opPara.Value = para.Obj;
                            opPara.Direction = ParameterDirection.Output;
                            opPara.SqlDbType = SqlDbType.VarChar;
                            opPara.Size=300;
                            com.Parameters.Add(opPara);
                        }
                        else
                            com.Parameters.AddWithValue(para.key, para.Obj);
                    }
                }

                Con.Open();
               this.NoOfRows= com.ExecuteNonQuery();

                List<KeyValues> returnList = new List<KeyValues>();
                foreach (SqlParameter exp in com.Parameters)
                {
                    if (exp.Direction == ParameterDirection.Output)
                        returnList.Add(new KeyValues(exp.ParameterName,Convert.ToString(exp.Value)));
                }
                this.outputPara = returnList;
                Con.Close();
                Con.Dispose();
                
                

            }
            catch (Exception Ex)
            {
                throw Ex;
                returnValue = false;
            }
            finally
            {
                Con.Close();
                Con.Dispose();

            }
            return returnValue;
        }
        public List<T> ExecuteProcedureByReader<T>() where T : new()
        {
            List<T> res = new List<T>();
            List<T> rights = new List<T>();

            SqlConnection Con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand(this.ProcedureName, Con);
            com.CommandTimeout = 0;
            com.CommandType = CommandType.StoredProcedure;
            foreach (KeyObj para in this.param)
            {
                com.Parameters.AddWithValue(para.key, para.Obj);
            }
            Con.Open();
            SqlDataReader r = com.ExecuteReader();
            T t = new T();
            while (r.Read())
            {
                for (int inc = 0; inc < r.FieldCount; inc++)
                {
                    Type type = t.GetType();
                    PropertyInfo prop = type.GetProperty(r.GetName(inc));
                    prop.SetValue(t, r.GetValue(inc), null);
                }

                
            }

            r.NextResult();            

                DataTable userR = new DataTable();
                userR.Load(r);

                Type Rightstype = t.GetType();
                PropertyInfo proprights = Rightstype.GetProperty("UserRights");
                proprights.SetValue(t, userR, null);
                res.Add(t);        

            r.Close();
            Con.Close();
            return res;
        }
    }
}
