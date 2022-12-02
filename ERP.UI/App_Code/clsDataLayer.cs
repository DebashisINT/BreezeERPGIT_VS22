using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// Summary description for clsDataLayer
/// </summary>
public class clsDataLayer
{
    private SqlConnection m_SQLCon;
    string m_ConString;

    public clsDataLayer()
	{
		//
		// TODO: Add constructor logic here
		//
        //m_ConString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionDefault"].ToString(); MULTI
        m_ConString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
	 }
    public DataSet GetDataSet(string sqlstr,string TableName)
		{
			try
			{
				SqlConnection constr = new SqlConnection(m_ConString);
				SqlDataAdapter da	= new SqlDataAdapter(sqlstr,constr);                
				DataSet ds = new DataSet();
				da.Fill(ds,""+TableName+"");
				return ds;
			}
			catch(Exception ex)
			{
				LogError(ex);
				return null;
			}
		}

		#region Connection open and closed

		public SqlConnection GetConnection()
		{
			m_SQLCon=new SqlConnection(m_ConString); 
			if(m_SQLCon.State == ConnectionState.Closed)
			{
				try{
					   m_SQLCon.Open();                       
				 }
				catch(Exception ex)
				{
					LogError(ex);
				}
			}
			return m_SQLCon;
		}
		public void CloseConnection()
		{
			if(m_SQLCon.State != ConnectionState.Closed )
			{
				m_SQLCon.Close(); 
				m_SQLCon.Dispose();
			}
		}

		
		#endregion
		
		#region Through Error Message

		public void LogError(Exception e)
		{
			throw e;
			//MessageBox.Show(e.Message,clsGlobal.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
           
		}
		
		#endregion Through Error Message
		
		# region commands

		public SqlCommand CreateCommand(string SQLText,SqlParameter []Params ,CommandType cmdSQLType)
		{
			try
			{
				SqlCommand _mCommand;
				_mCommand = new SqlCommand(SQLText, GetConnection());
				_mCommand.CommandType = cmdSQLType;
                _mCommand.CommandTimeout = 0;
				if(Params!=null)
				{
					foreach(SqlParameter _mParam in Params)
					{
						if (_mParam!=null)
							_mCommand.Parameters.Add(_mParam);
					}
				}
				return _mCommand;
			}
			catch(Exception ex)
			{
				LogError(ex);
				CloseConnection();
				return null;
			}
		}

		public object ExecuteScalar(string SqlText, SqlParameter []Param)
		{
			SqlCommand myCommand = CreateCommand(SqlText, Param, CommandType.Text);
			object retVal = null;
			try
			{
				if(myCommand!=null)
					retVal = myCommand.ExecuteScalar();
				CloseConnection();
				return retVal;
			}
			catch (Exception ex)
			{
				LogError(ex);
				CloseConnection();
				return null;
			}
		}

		public void ExecuteScalar(string SqlText,byte[] bArr)
		{
			SqlCommand myCommand = new SqlCommand(SqlText, GetConnection());
//			object retVal = null;
			try
			{
				if(myCommand!=null)
					bArr =(byte[]) myCommand.ExecuteScalar();
				CloseConnection();				
			}
			catch (Exception ex)
			{
				LogError(ex);
				CloseConnection();				
			}
		}
		public object ExecuteScalar(string SqlText)
		{
			SqlCommand myCommand = new SqlCommand(SqlText, GetConnection());
			//			object retVal = null;
			object result=DBNull.Value;
			try
			{
				if(myCommand!=null)
					result=myCommand.ExecuteScalar();
				CloseConnection();
				return result;
			}
			catch (Exception ex)
			{
				LogError(ex);
				CloseConnection();
				return DBNull.Value;
			}
		}
		
		# endregion commands

        #region EXECUTE SP RETURN READER
        public SqlDataReader ExecuteSPReader(string SProcName, SqlParameter[] Param, DataSet ds, string tablename)
        {
            SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
            SqlDataReader reader = null;
    
            try
            {
                reader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                LogError(ex);
                if (this.m_SQLCon != null)
                {
                    m_SQLCon.Dispose();
                }
                return null;
            }

            return reader;
        }


        public SqlDataReader ExecuteSPReader(string SProcName, SqlParameter[] Param)
        {
            SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
            SqlDataReader reader = null;

            try
            {
                reader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                LogError(ex);
                if (this.m_SQLCon != null)
                {
                    m_SQLCon.Dispose();
                }
                return null;
            }

            return reader;
        }

        #endregion

        # region ExucateSP

        public bool ExecuteSP(string SProcName ,SqlParameter []Param,DataSet ds, string tablename)
		{
			SqlCommand myCommand = CreateCommand(SProcName,Param,CommandType.StoredProcedure);
			SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
			myAdapter.AcceptChangesDuringFill = true;
			myAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			try
			{
				if(tablename=="")
					myAdapter.Fill(ds);
				else
					myAdapter.Fill(ds,tablename);
				if(this.m_SQLCon != null)
				{                             
					m_SQLCon.Dispose();
				}
				return true;
			}
			catch(Exception ex)
			{
				LogError(ex);
				if(this.m_SQLCon != null)
				{                             
					m_SQLCon.Dispose();
				}
				return false;
			}
		}

    public bool ExecuteSP(string SProcName, SqlParameter[] Param, DataTable dt)
    {
        SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
        SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
        myAdapter.AcceptChangesDuringFill = true;
        myAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        try
        {

            myAdapter.Fill(dt);
            if (this.m_SQLCon != null)
            {
                m_SQLCon.Dispose();
            }
            return true;
        }

        catch (Exception ex)
        {
            LogError(ex);
            if (this.m_SQLCon != null)
            {
                m_SQLCon.Dispose();
            }
            return false;
        }
    }
		public bool ExecuteSP(string SProcName,SqlParameter []Param,ref SqlDataReader dtr)
		{
			SqlCommand myCopmmand = CreateCommand(SProcName,Param,CommandType.StoredProcedure);
			try
			{
				dtr = myCopmmand.ExecuteReader();
				return true;
			}
			catch(Exception ex)
			{
				LogError(ex);
				if(this.m_SQLCon != null)
				{                       
					m_SQLCon.Dispose();
				}
				return false;
			}
		}
		public bool ExecuteSP(string SProcName, SqlParameter []Param)
		{
            try
            {
                SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
                myCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
		}



		public void ExecuteSP(ref SqlParameter []Param,string SProcName)
		{
            try
            {
                SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            { 
            
            }
		
		}

        // Modified by sunipa 190608
        public int ExecuteSPWithRetVal(string SProcName, SqlParameter[] Param)
        {
            Param[1] = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            Param[1].Direction = ParameterDirection.ReturnValue;
            SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
            myCommand.ExecuteNonQuery();

            int retVal = 0;

            retVal = Convert.ToInt32(Param[1].Value);

            return retVal;
        }
        // end sunipa

    

		# endregion ExucateSP
		
		public long SP_InsertUpdateTable(Hashtable mHashTable, string StoredProc)
		{
			// To Call This Your Identity element in stored procedure Mustbe @ID
			
			IDictionaryEnumerator inum = mHashTable.GetEnumerator();
			
			SqlParameter []sqlparm = new SqlParameter[mHashTable.Count + 1];   
			int i =0;
			while(inum.MoveNext())
			{				
				if (inum.Key.ToString()=="Photo")
				{
					sqlparm[i]= new SqlParameter("@" + inum.Key.ToString(),SqlDbType.Image,2147483646);
					sqlparm[i].Value=inum.Value;
				}
				else
				{
					sqlparm[i]= new SqlParameter("@" + inum.Key.ToString(),inum.Value.ToString());
				}				
				sqlparm[i].Direction = ParameterDirection.Input;				
				i++;
			}
			try
			{
				bool retval = ExecuteSP(StoredProc.ToString(),sqlparm);
				if (retval==true)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			catch(Exception ex)
			{
				LogError(ex);
				return 0;
			}
			
		}
		public long SP_InsertUpdateTable( DataTable dtList, string StoredProc)
		{
			// To Call This Your Identity element in stored procedure Mustbe @ID
			SqlParameter []sqlparm = new SqlParameter[dtList.Rows.Count ];   
			int i =0;
			while(i<dtList.Rows.Count)
			{
				sqlparm[i]= new SqlParameter("@" + dtList.Rows[i]["Col"],dtList.Rows[i]["Dat"]);
				sqlparm[i].Direction = ParameterDirection.Input;
				i++;			
			}
//			IDictionaryEnumerator inum = mHashTable.GetEnumerator();
//			
//			SqlParameter []sqlparm = new SqlParameter[mHashTable.Count + 1];   
//			int i =0;
//			while(inum.MoveNext())
//			{
//				sqlparm[i]= new SqlParameter("@" + inum.Key.ToString(),inum.Value.ToString());
//				sqlparm[i].Direction = ParameterDirection.Input;
//				i++;
//			}
			try
			{
				bool retval =ExecuteSP(StoredProc.ToString(),sqlparm);
				if (retval==true)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			catch(Exception ex)
			{
				LogError(ex);
				return 0;
			}
			
		}
//		public long ExecuteSPDataTable(DataTable dtList, string StoredProc)
//		{
//			// To Call This Your Identity element in stored procedure Mustbe @ID
//			// To Call This Your Identity element in stored procedure Mustbe @ID 
//			IDictionaryEnumerator inum = mHashTable.GetEnumerator();
//			SqlParameter []sqlparm = new SqlParameter[mHashTable.Count + 1];   
//			int i =0;
//			while(inum.MoveNext())
//			{
//				sqlparm[i]= new SqlParameter("@" + inum.Key.ToString(),inum.Value.ToString());
//				sqlparm[i].Direction = ParameterDirection.Input;
//				i++;
//			}
//			sqlparm[i] = new SqlParameter("@ID", SqlDbType.BigInt);
//			sqlparm[i].Direction = ParameterDirection.Output;   
//
//
//
//			//remin to be finished
//			ExecuteSP(StoredProc.ToString(),sqlparm);
//			return(Convert.ToInt64(sqlparm[i].Value)); 
//			
//		}
		public long SP_InsertTableReturnIdentity( Hashtable mHashTable, string StoredProc)
		{
			// To Call This Your Identity element in stored procedure Mustbe @ID 
			IDictionaryEnumerator inum = mHashTable.GetEnumerator();
			SqlParameter []sqlparm = new SqlParameter[mHashTable.Count + 1];   
			int i =0;
			while(inum.MoveNext())
			{
				sqlparm[i]= new SqlParameter("@" + inum.Key.ToString(),inum.Value.ToString());
				sqlparm[i].Direction = ParameterDirection.Input;
				i++;
			}
			sqlparm[i] = new SqlParameter("@ID", SqlDbType.BigInt);
            sqlparm[i].Direction = ParameterDirection.Output;   



			//remin to be finished
			ExecuteSP(StoredProc.ToString(),sqlparm);
			return(Convert.ToInt64(sqlparm[i].Value));   
		}

		
		# region ExecuteText

		public bool ExecuteText(string SqlText,SqlParameter []Param,SqlDataReader dtr)
		{
			SqlCommand myCommand = CreateCommand(SqlText, Param, CommandType.Text);
			try{
				dtr = myCommand.ExecuteReader(CommandBehavior.Default);
				return true;
			}
			catch(Exception e)
			{
				return false;
			}
		}
		public SqlDataReader ExecuteText(SqlParameter []Param, string SqlText)
		{
			SqlCommand myCommand = CreateCommand(SqlText, Param, CommandType.Text);
			SqlDataReader dtr=null;
			try
			{
				 dtr = myCommand.ExecuteReader(CommandBehavior.Default);
				return dtr;
			}
			catch
			{
				return dtr;
			}
		}
		
		public bool ExecuteText(string SqlText,SqlParameter []Param, DataSet dset,string tablename)
		{
			SqlCommand myCommand = CreateCommand(SqlText, Param, CommandType.Text);
			SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
			myAdapter.AcceptChangesDuringFill = true;
			myAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			
			try
			{
				if(tablename == "")
					myAdapter.Fill(dset);
				else
					myAdapter.Fill(dset, tablename);
				return true;
			}
			catch(Exception e)
			{
				return false;
			}
			finally
			{
				myCommand.Dispose();
				myAdapter.Dispose();
				CloseConnection();
			}
		}

		public bool ExecuteText(string SqlText,SqlParameter []Param)
		{
			SqlCommand myCommand=CreateCommand(SqlText,Param,CommandType.Text);
			bool Retval= true;
			try
			{
				myCommand.ExecuteNonQuery();
				Retval =true;
				
			}
			catch
			{
				Retval= false;
			}
			finally
			{
				myCommand.Dispose();
				CloseConnection();

			}
			return Retval;
		}
		public bool ExecuteText(string SqlText)
		{
			SqlCommand myCommand=new SqlCommand(SqlText, GetConnection());
			myCommand.CommandType =CommandType.Text;
			bool Retval= true;
			try
			{
				myCommand.ExecuteNonQuery();
				Retval =true;
				
			}
			catch
			{
				Retval= false;
			}
			finally
			{
				myCommand.Dispose();
				CloseConnection();

			}
			return Retval;
		}

		public object ExecuteScalarText(string SqlText)
		{
			object obj;
			SqlCommand myCommand=new SqlCommand(SqlText, GetConnection());
			myCommand.CommandType =CommandType.Text;
			obj=myCommand.ExecuteScalar();
			myCommand.Dispose();
			CloseConnection();			
			if (obj==null)
				obj="";
			return obj;
		}

		public int RowCount(string strTableName)
		{
			string strSql	=	"select count(*) from "+strTableName;
			SqlCommand myCommand=new SqlCommand(strSql, GetConnection());
			myCommand.CommandType	=	CommandType.Text;
			
			
			return Convert.ToInt32(myCommand.ExecuteScalar());
		}

		public bool ExecuteText(string SqlText,DataSet dset,string tablename)
		{
			SqlCommand myCommand=new SqlCommand(SqlText, GetConnection());
			myCommand.CommandType =CommandType.Text;
			SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
			myAdapter.AcceptChangesDuringFill = true;
			myAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			try
			{
				if(tablename == "")
					myAdapter.Fill(dset);
				else
					myAdapter.Fill(dset, tablename);
				return true;
			}
			catch(Exception ex)
			{
				string str= ex.Message.ToString();
				return false;
			}
			finally
			{
				myCommand.Dispose();
				myAdapter.Dispose();
				CloseConnection();
			}
		}
		
		
		# endregion ExecuteText

		#region ExecuteNonQuery
		public void ExecNonQry(string strsql)
		{
			try
			{
				//SqlConnection constr = new SqlConnection(m_ConString);				
				SqlCommand comm=new SqlCommand(strsql,GetConnection());
				comm.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				LogError(ex);				
			}
		}
		#endregion ExecuteNonQuery
		
		#region ExecuteSqlDatareader
		
		/// <summary>
		/// Execute a sql and returns the result in a sqldatareader
		/// </summary>
		/// <param name="Sql">Sql Text</param>
		/// <returns>SqlDateReader</returns>
		public SqlDataReader ExecuteSqlDatareader(string Sql)
		{
			SqlCommand Mcommnd = new SqlCommand(Sql,GetConnection());
			SqlDataReader Mdr = null;
			try
			{
				Mdr= Mcommnd.ExecuteReader(); 
			}
			catch
			{
				CloseConnection();
						
			}
			return Mdr;
		}


		#endregion ExecuteSqlDatareader
		
		#region FetchRecords
		
		public DataTable FetchRecords(string strSQL)
		{
			try
			{
				SqlConnection conSQL = new SqlConnection(m_ConString);
				SqlDataAdapter da	= new SqlDataAdapter(strSQL,conSQL);
				DataSet ds = new DataSet("mDS");
				da.Fill(ds);
				return ds.Tables[0];
			}
			catch(Exception ex)
			{
				LogError(ex);
				return null;
			}
		}
		
		
		#endregion FetchRecords
		
		#region Generate Parameter
		
		public SqlParameter[] ReturnParameter(Hashtable HTable)
		{
			IDictionaryEnumerator myEnum = HTable.GetEnumerator();
			SqlParameter []param = new SqlParameter[HTable.Count];
			int i=0;
			while(myEnum.MoveNext())
			{
				param[i] = new SqlParameter("@"+ myEnum.Key.ToString(),myEnum.Value.ToString());
				param[i].Direction = ParameterDirection.Input;
				i++;
			}

			return param;
		}
		
		
		#endregion Generate Parameter
	
		#region Validate Exist check
		
		public bool ValCheck(string TableName,string condition)
		{
			bool retval=false;
			string exetext="select * from "+TableName;
				if (condition!=null && condition.Length>0)
				{exetext+=" where "+condition;
				}
            SqlCommand myCommand=new SqlCommand(exetext, GetConnection());
			SqlDataReader drd=myCommand.ExecuteReader();
			if (drd.HasRows)
			{
				retval=true;
			}
			else
			{retval=false;
			}
			CloseConnection();
			return retval;
		}
		
		#endregion	

        #region return string
        //by avijit on 01.07.2008 
        public string SP_ReturnStringPara(Hashtable mHashTable, string StoredProc, string outPara)
        {
            // To Call This Your Identity element in stored procedure Mustbe @ID 
            IDictionaryEnumerator inum = mHashTable.GetEnumerator();
            SqlParameter[] sqlparm = new SqlParameter[mHashTable.Count + 1];
            int i = 0;
            while (inum.MoveNext())
            {
                sqlparm[i] = new SqlParameter("@" + inum.Key.ToString(), inum.Value.ToString());
                sqlparm[i].Direction = ParameterDirection.Input;
                i++;
            }
            sqlparm[i] = new SqlParameter("@" + outPara, SqlDbType.VarChar);
            sqlparm[i].Direction = ParameterDirection.Output;
            sqlparm[i].Size = 50;
            sqlparm[i].Value = "";


            //remin to be finished
            ExecuteSP(StoredProc.ToString(), sqlparm);
            return (Convert.ToString(sqlparm[i].Value));
        }
        #endregion
        #region partha
        //partha Integrate

        public long SP_InsertAffiliateTableReturnIdentity(Hashtable mHashTable, string StoredProc)
        {

            // To Call This Your Identity element in stored procedure Mustbe @ID 
            IDictionaryEnumerator inum = mHashTable.GetEnumerator();
            SqlParameter[] sqlparm = new SqlParameter[mHashTable.Count + 1];
            int i = 0;
            while (inum.MoveNext())
            {
                sqlparm[i] = new SqlParameter("@" + inum.Key.ToString(), inum.Value.ToString());
                sqlparm[i].Direction = ParameterDirection.Input;
                i++;
            }


            sqlparm[i] = new SqlParameter("@ID", SqlDbType.BigInt);
            sqlparm[i].Direction = ParameterDirection.Output;


            //remin to be finished
            ExecuteSP(StoredProc.ToString(), sqlparm);
            return (Convert.ToInt64(sqlparm[i].Value));

        }

        #endregion

        #region "MAINAK"
        public DataTable ExecuteProcedure(String SProcName, SqlParameter[] Param)
    {
        DataTable dtTable=null;
        DataSet myDataset =new DataSet();
        try
        {
            SqlCommand myCommand = CreateCommand(SProcName, Param, CommandType.StoredProcedure);
            SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
             myAdapter.Fill(myDataset, "table");
            if(myDataset!=null)
            {
            dtTable = myDataset.Tables[0];
            }
        }
        catch (Exception ex)
        {
        }
        return dtTable;
        
    }
        #endregion
    }
