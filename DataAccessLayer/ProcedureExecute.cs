using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Web;

namespace DataAccessLayer
{

    public enum QueryParameterDirection : int
    {
        Input = 1,
        Output = 2,
        Return = 3
    }
    public class ProcedureExecute : IDisposable
    {
        SqlConnection oSqlConnection = new SqlConnection();

        private string strCommandText = string.Empty;
        private bool blnSP = true;

        private ArrayList oParameters = new ArrayList();
        private bool blnLocalConn = true;

        #region Constructor
        public ProcedureExecute(string StoredProcName)
            : this(StoredProcName, false)
        {

        }

        public ProcedureExecute(string SqlString, bool IsTextQuery)
        {
            blnSP = !IsTextQuery;
            strCommandText = SqlString;
        }
        public ProcedureExecute()
        {

        }
        #endregion

        #region DataTable
        // REturn a Datatable  
        public DataTable GetTable()
        {
            DataTable dt = null;
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);
            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                if ((null != ds) && (ds.Tables.Count > 0))
                {
                    dt = ds.Tables[0];
                }

                
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            catch (SystemException ex)
            {
                //throw ex;
                
                Type exception = ex.GetType();
                string name = exception.Name;
                if (name.ToLower().Contains("sqlexception"))
                {
                    SqlException sqlex = ex as SqlException;
                    throw sqlex;
                }

                if (name.ToLower().Contains("exception"))
                {
                    Exception ex1 = ex as Exception;
                    throw ex1;
                }
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                   
                }
                //Debu
                this.oConn.Dispose();

                oCmd.Dispose();
                da.Dispose();
            }
            return dt;
        }

        public DataTable GetTableModified()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);
            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(dt);
                //if ((null != ds) && (ds.Tables.Count > 0))
                //{
                //    dt = ds.Tables[0];
                //}


            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            catch (SystemException ex)
            {
                //throw ex;

                Type exception = ex.GetType();
                string name = exception.Name;
                if (name.ToLower().Contains("sqlexception"))
                {
                    SqlException sqlex = ex as SqlException;
                    throw sqlex;
                }

                if (name.ToLower().Contains("exception"))
                {
                    Exception ex1 = ex as Exception;
                    throw ex1;
                }
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();

                }
                //Debu
                this.oConn.Dispose();

                oCmd.Dispose();
                da.Dispose();
            }
            return dt;
        }
        #endregion

        #region DataReader
        public SqlDataReader GetReader()
        {
            SqlDataReader dr = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                this.InitQuery(oCmd);
                dr = oCmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception v1)
            {
                return dr;
            }
            finally
            {
                //Debu
                oConn.Dispose();
                oCmd.Dispose();
            }
        }
        #endregion

        #region DataSet
        // REturn a Datatable  
        public DataSet GetDataSet()
        {
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);
            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);

            }
            catch (SystemException ex)
            {
                //throw ex;
                Type exception = ex.GetType();
                string name = exception.Name;
                if (name.ToLower().Contains("sqlexception"))
                {
                    SqlException sqlex = ex as SqlException;
                    throw sqlex;
                }

                if (name.ToLower().Contains("exception"))
                {
                    Exception ex1 = ex as Exception;
                    throw ex1;
                }
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                //Debu
                oConn.Dispose();
                oCmd.Dispose();
                da.Dispose();
            }
            return ds;
        }
        #endregion

        #region NonQuery

        public int RunActionQuery()
        {
            int intRowsAffected = -1;

            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);

            try
            {
                intRowsAffected = oCmd.ExecuteNonQuery();
            }
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
            catch (SystemException ex)
            {

                //-------------------------------------------

                // For Read Only User Error Notification Start-----------------------------------------

                if (ex.Message.Contains("The INSERT permission was denied on the object"))
                {

                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (ex.Message.Contains("The UPDATE permission was denied on the object"))
                {

                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (ex.Message.Contains("The DELETE permission was denied on the object"))
                {

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


                //------------------------------------------


                //throw ex;
                Type exception = ex.GetType();
                string name = exception.Name;
                if (name.ToLower().Contains("sqlexception"))
                {
                    SqlException sqlex = ex as SqlException;
                    throw sqlex;
                }

                if (name.ToLower().Contains("exception"))
                {
                    Exception ex1 = ex as Exception;
                    throw ex1;
                }
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                  
                }
                oCmd.Dispose();
                //Debu
                this.oConn.Dispose();
            }

            return intRowsAffected;
        }
        #endregion

        #region Scalar
        public object GetScalar()
        {
            object oRetVal = null;

            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);

            try
            {
                oRetVal = oCmd.ExecuteScalar();
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            catch (SystemException ex)
            {
                //throw ex;
                Type exception = ex.GetType();
                string name = exception.Name;
                if (name.ToLower().Contains("sqlexception"))
                {
                    SqlException sqlex = ex as SqlException;
                    throw sqlex;
                }

                if (name.ToLower().Contains("exception"))
                {
                    Exception ex1 = ex as Exception;
                    throw ex1;
                }
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                  
                }
                //Debu
                this.oConn.Dispose();
                oCmd.Dispose();
            }

            return oRetVal;
        }
        #endregion

        #region Initializes a Query
        private void InitQuery(SqlCommand oCmd)
        {
            blnLocalConn = (this.oConn == null);
            if (blnLocalConn)
            {
                string conn = string.Empty;
                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {
                        //conn = System.Configuration.ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; //DBReadOnlyConnection MULTI
                        conn =  Convert.ToString(HttpContext.Current.Session["ErpConnection"]); 

                    }
                    else
                    {
                        //conn = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        conn = Convert.ToString(HttpContext.Current.Session["ErpConnection"]); 
                    }
                }

                // string conn = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                Open(conn);
                blnLocalConn = true;
            }
            oCmd.Connection = oConn;


            oCmd.CommandText = this.strCommandText;
            oCmd.CommandType = (this.blnSP ? CommandType.StoredProcedure : CommandType.Text);


            oCmd.CommandTimeout = (24 * 60 * 60);	// 1 Day

            foreach (object oItem in this.oParameters)
            {
                oCmd.Parameters.Add((SqlParameter)oItem);
            }
        }
        #endregion

        #region Parameter handling

        #region Type: General

        public void AddPara(string ParameterName, object value)
        {
            SqlParameter oPara = new SqlParameter(ParameterName, value);
            this.oParameters.Add(oPara);
        }

        public void AddPara(string ParameterName, SqlDbType type, int size)
        {
            SqlParameter oPara = new SqlParameter(ParameterName, type, size);
            this.oParameters.Add(oPara);
        }

        public void AddPara(string ParameterName, SqlDbType type, int size, ParameterDirection direction)
        {
            SqlParameter oPara = new SqlParameter(ParameterName, type, size);
            oPara.Direction = direction;
            this.oParameters.Add(oPara);
        }

        public void AddPara(string ParameterName, SqlDbType type, ParameterDirection direction)
        {
            SqlParameter oPara = new SqlParameter(ParameterName, type);
            oPara.Direction = direction;
            this.oParameters.Add(oPara);
        }

        public void AddPara(string ParameterName, SqlDbType type, ParameterDirection direction, object value)
        {
            SqlParameter oPara = new SqlParameter(ParameterName, type);
            oPara.Direction = direction;
            oPara.Value = value;
            this.oParameters.Add(oPara);
        }

        #endregion

        #region Type: Integer

        public void AddIntegerPara(string Name, int Value)
        {
            AddIntegerPara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddIntegerPara(string Name, int? Value, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }
        public void AddIntegerNullPara(string Name)
        {
            AddIntegerNullPara(Name, QueryParameterDirection.Input);
        }
        public void AddIntegerNullPara(string Name, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = DBNull.Value;
            this.oParameters.Add(oPara);
        }

        #endregion


        #region Type: BigInt

        public void AddBigIntegerPara(string Name, long Value)
        {
            AddBigIntegerPara(Name, Value, QueryParameterDirection.Input);
        }


        public void AddBigIntegerPara(string Name, long Value, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.BigInt);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }
        public void AddBigIntegerNullPara(string Name)
        {
            AddIntegerNullPara(Name, QueryParameterDirection.Input);
        }
        public void AddBigIntegerNullPara(string Name, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = DBNull.Value;
            this.oParameters.Add(oPara);
        }

        #endregion

        #region Type: Char

        public void AddCharPara(string Name, int Size, char Value)
        {
            AddCharPara(Name, Size, Value, QueryParameterDirection.Input);
        }

        public void AddCharPara(string Name, int Size, char Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value.Equals(null))
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Char, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: NChar

        public void AddNCharPara(string Name, int Size, char Value)
        {
            AddNCharPara(Name, Size, Value, QueryParameterDirection.Input);
        }

        public void AddNCharPara(string Name, int Size, char Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value.Equals(null))
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: Varchar

        public void AddVarcharPara(string Name, int Size, string Value)
        {
            AddVarcharPara(Name, Size, Value, QueryParameterDirection.Input);
        }

        public void AddVarcharPara(string Name, int Size, string Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }

            SqlParameter oPara = new SqlParameter(Name, SqlDbType.VarChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: NVarchar

        public void AddNVarcharPara(string Name, int Size, string Value)
        {
            AddNVarcharPara(Name, Size, Value, QueryParameterDirection.Input);
        }

        public void AddNVarcharPara(string Name, int Size, string Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NVarChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: Boolean

        public void AddBooleanPara(string Name, bool Value)
        {
            AddBooleanPara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddBooleanPara(string Name, bool Value, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Bit);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: DateTime

        public void AddDateTimePara(string Name, DateTime Value)
        {
            AddDateTimePara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddDateTimePara(string Name, DateTime Value, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.DateTime);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: Text

        public void AddTextPara(string Name, string Value)
        {
            AddTextPara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddTextPara(string Name, string Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Text);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: NText

        public void AddNTextPara(string Name, string Value)
        {
            AddNTextPara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddNTextPara(string Name, string Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NText);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: Decimal

        public void AddDecimalPara(string Name, byte Scale, byte Precision, decimal Value)
        {
            AddDecimalPara(Name, Scale, Precision, Value, QueryParameterDirection.Input);
        }

        public void AddDecimalPara(string Name, byte Scale, byte Precision, decimal Value, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Decimal);
            oPara.Scale = Scale;
            oPara.Precision = Precision;
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: Image

        public void AddImagePara(string Name, byte[] Value)
        {
            AddImagePara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddImagePara(string Name, byte[] Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Image);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Type: XML

        public void AddXMLPara(string Name, string Value)
        {
            AddXMLPara(Name, Value, QueryParameterDirection.Input);
        }

        public void AddXMLPara(string Name, string Value, QueryParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
                oValue = DBNull.Value;

            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Xml);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Adds a NULL value Parameter

        public void AddNullValuePara(string Name)
        {
            SqlParameter oPara = new SqlParameter(Name, DBNull.Value);
            oPara.Direction = ParameterDirection.Input;
            this.oParameters.Add(oPara);
        }


        public void AddNullValuePara(string Name, QueryParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, DBNull.Value);
            oPara.Direction = GetParaType(Direction);
            this.oParameters.Add(oPara);
        }
        #endregion

        #region Adds the Return Parameter

        public void AddReturnPara()
        {
            this.AddIntegerPara("ReturnIntPara", 0, QueryParameterDirection.Return);
        }
        #endregion

        #region Returns the value of the passed parameter

        public object GetParaValue(string ParaName)
        {
            object oValue = null;
            SqlParameter oPara = null;

            ParaName = ParaName.Trim().ToLower();
            foreach (object oItem in this.oParameters)
            {
                oPara = (SqlParameter)oItem;
                if (oPara.ParameterName.ToLower() == ParaName)
                {
                    oValue = oPara.Value;
                    break;
                }
            }

            return oValue;
        }
        #endregion

        #region Returns the value of the Return Parameter

        public object GetReturnParaValue()
        {
            return this.GetParaValue("ReturnIntPara");
        }
        #endregion

        #region Clears the parameters

        public void ClearParameters()
        {
            this.oParameters.Clear();
        }
        #endregion

        #region Converts enum to parameter direction

        private ParameterDirection GetParaType(QueryParameterDirection Direction)
        {
            switch (Direction)
            {
                case QueryParameterDirection.Output:
                    return ParameterDirection.InputOutput;
                case QueryParameterDirection.Return:
                    return ParameterDirection.ReturnValue;
                default:
                    return ParameterDirection.Input;
            }
        }
        #endregion
        #endregion

        #region Dispose

        public void Dispose()
        {
            this.oConn.Dispose();
            this.oParameters.Clear();
        }
        #endregion

        #region Opens a connection

        public bool Open(string ConnectionString)
        {
            blnIsOpen = false;
            oConn = new SqlConnection(ConnectionString);
            
            oConn.Open();
            blnIsOpen = true;
            return blnIsOpen;
        }
        #endregion

        #region Connection
        private SqlConnection oConn = null;

        public SqlConnection Connection
        {
            set
            {
                oConn = value;
            }
        }
        #endregion

        #region IsOpen
        private bool blnIsOpen = false;

        public bool IsOpen
        {
            get
            {
                return blnIsOpen;
            }
        }
        #endregion

        # region ConfiguredAdapter

        public SqlDataAdapter GetConfiguredAdapter()
        {
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);

            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);

            return da;
        }

        # endregion

        #region DBengine

        // ------------------------------------------------------------------------ //
        // This method returns a SqlConnection object
        // ------------------------------------------------------------------------ //      
        public void GetConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
               // oSqlConnection.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI

                oSqlConnection.ConnectionString = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);


                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {


                        //oSqlConnection.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBReadOnlyConnection"];//DBReadOnlyConnection MULTI

                        oSqlConnection.ConnectionString = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);//DBReadOnlyConnection
                    }
                    else
                    {
                        //oSqlConnection.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI

                        oSqlConnection.ConnectionString = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                    }
                }


                //oSqlConnection.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                oSqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
                oSqlConnection.Close();
            }
        }

        // ------------------------------------------------------------------------ //
        // This generic method returns a SqlDataReader object from the passed
        // SQL command.
        // ------------------------------------------------------------------------ //
        public SqlDataReader GetReader(String cSql)
        {
            // Now we create a Command object and execute the command.
            GetConnection();
            SqlDataReader lsdr;
            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);
            lsdr = lcmd.ExecuteReader();
            return lsdr;
        }
        // ------------------------------------------------------------------------ //
        // This generic method returns a SqlDataReader object from the passed
        // SQL command and Connection.
        // ------------------------------------------------------------------------ //
        public SqlDataReader GetReader1(String cSql, string SqlCOnnnection)
        {
            // calling GetConnection Method to connect database           
            GetConnection();
            // Now we create a Command object and execute the command.          
            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);
            SqlDataReader lsdr;
            lsdr = lcmd.ExecuteReader();
            //Debu
            oSqlConnection.Close();
            oSqlConnection.Dispose();
            return lsdr;
        }
        public string[] GetFieldValue1(
              String cTableName,      // TableName from which the field value is to be fetched
              String cFieldName,     // The name if the field whose value needs to be returned
              String cWhereClause,    // Optional : WHERE condition [if any]
              int NoField)   // Number of field value to selection
        {
            // Return Value
            string[] vRetVal = new string[NoField];

            // Now we construct a SQL command that will fetch the field from the table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }

            SqlDataReader lsdr;
            lsdr = GetReader(lcSql);

            if (lsdr.HasRows)
            {
                while (lsdr.Read())
                {
                    for (int i = 0; i < NoField; i++)
                    {
                        vRetVal[i] = lsdr[i].ToString();
                    }
                }
            }
            else
            {
                vRetVal[0] = "n";
            }

            // We close the DataReader
            lsdr.Close();

            return vRetVal;
        }

        #endregion
    }

    public class CustomPager
    {
        DataTable _BaseTable;
        int _PageSize = 0;
        int _CurrentPageIndex = 0;
        int _PageCount;

        # region Constructors

        public CustomPager(DataTable TableToPage)
        {
            _BaseTable = TableToPage;
        }

        #endregion

        # region public properties

        public int CurrentPageIndex
        {
            get
            {
                return _CurrentPageIndex;
            }
            set
            {
                if (CurrentPageIndex < _PageCount)
                    _CurrentPageIndex = value;
                else
                    _CurrentPageIndex = 0;
            }
        }

        public int PageSize // Also sets page count
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
                double dblPgCnt = Convert.ToDouble(_BaseTable.Rows.Count) / Convert.ToDouble(_PageSize);
                if (dblPgCnt > Convert.ToDouble(Convert.ToInt32(dblPgCnt)))
                    _PageCount = (_BaseTable.Rows.Count / _PageSize) + 1;
                else
                    _PageCount = (_BaseTable.Rows.Count / _PageSize);
            }
        }

        # endregion

        # region public functions

        public DataTable GetPagedTable(int CurrentPageIndex)
        {
            DataTable dtPaged;
            if (!(CurrentPageIndex < _PageCount))
                CurrentPageIndex = 0;
            dtPaged = _BaseTable.Clone();

            int StartPoint = _PageSize * CurrentPageIndex;
            int EndPoint = StartPoint + PageSize;
            for (int cntRow = StartPoint; cntRow < EndPoint; cntRow++)
            {
                dtPaged.Rows.Add(_BaseTable.Rows[cntRow]);
            }

            return dtPaged;
        }
        #endregion



    }








}