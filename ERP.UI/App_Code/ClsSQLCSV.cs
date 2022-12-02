using System;
using System.IO;
using System.Web;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace ExportCSV.Classes
{
	/// <summary>
	/// Summary description for ClsSQLCSV.
	/// </summary>
	public class ClsSQLCSV
	{
		#region Protected Attributes
		protected SqlConnection conn = null;
		protected SqlCommand cmd = null;	
		#endregion

		#region Class Constructor
		public ClsSQLCSV()
		{
			GetConfigSettings();
		}
		
		private void GetConfigSettings()
		{
			NameValueCollection config  =  (NameValueCollection)ConfigurationSettings.GetConfig("appSettings");
			ConnSQL = config["connSQL"];
			ConnCSV = config["connxls"];
		}
		#endregion
		
		#region Variable Declaration
		private string ConnSQL="";
		private string ConnCSV="";
		string strTableName="";
		string strServerPath="";
		string strCsvDirOnServer="";
		private string strExportCSV="";
		private string strExportTable="";
		private string strExportCSVDir="";
		private string strExportAsCsvOrText="";
		bool blnDropExistingTable=false;
		bool blnSaveFileOnServer=false;
		FileInfo fi;
		#endregion

		#region Import Functionality
		public string GenerateTable(SetProperties Properties,String filepath)
		{
			string strreturn = "";

			blnSaveFileOnServer = Properties.SaveFileOnServer;
			blnDropExistingTable = Properties.DropExistingTable;
			strCsvDirOnServer = Properties.CsvDirOnServer;
			strTableName = Properties.TableName;
			fi = Properties.FileInformation;

			DataSet dsCSV = new DataSet();
            dsCSV = ConnectFile(fi, filepath);
			DataColumnCollection tableColumns =	dsCSV.Tables[0].Columns;
			DataRowCollection tableRows	= dsCSV.Tables[0].Rows;

			if (TableExists()==1)
			{
				if (blnDropExistingTable)
				{
					DropTable();
					CreateTable(tableColumns);
				}
			}
			else
			{
				CreateTable(tableColumns);
			}

			//strreturn = InsertRecords(tableColumns, tableRows);

			if (blnSaveFileOnServer)
				SaveFileToServer(fi);

			return strreturn;
		}


        private DataSet ConnectFile(FileInfo filetable, String filepath)
		{
			DataSet ds = new DataSet();
			string str;
			try
			{
                ConnCSV = ConnCSV + filepath;//filetable.DirectoryName.ToString();
				string sqlSelect;
				OleDbConnection objOleDBConn;
				OleDbDataAdapter objOleDBDa;
				objOleDBConn = new OleDbConnection(ConnCSV);
				objOleDBConn.Open();
				sqlSelect =	"select * from ["+ filetable.Name.ToString() +"]";
				objOleDBDa = new OleDbDataAdapter(sqlSelect,objOleDBConn);
				objOleDBDa.Fill(ds);
				objOleDBConn.Close();
			}
			catch(Exception ex)
			{
				str = ex.Message;
			}
			return ds;
		}

		
		private int TableExists()
		{
			OpenConnection();	
			string strTable = "select table_name from information_schema.tables ";
			strTable = strTable + " where table_name = '" + strTableName + "'";
			int cmdReturn = 0;
			cmd = new SqlCommand(strTable, conn);

			try
			{
				SqlDataReader Reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				while(Reader.Read())
				{
					cmdReturn++;				
				}
				Reader.Close();
			}
			catch{}
			finally	{CloseConnection();}

			return cmdReturn;
		}

		
		private void DropTable()
		{
			OpenConnection();
			
			string sqldrop = "DROP TABLE " + strTableName;
			cmd = new SqlCommand(sqldrop, conn);

			try	{cmd.ExecuteNonQuery();}
			catch{}
			finally{CloseConnection();}

		}

		
		private void CreateTable(DataColumnCollection tableColumns)
		{
			OpenConnection();

			string sqlcreate = "CREATE TABLE " + strTableName + " (";
			foreach(DataColumn dc in tableColumns)
			{
				sqlcreate += "[" + dc.ColumnName.ToString() + "]" + " varchar(255), ";
			}

			sqlcreate  = sqlcreate.Substring(0, sqlcreate.Length-2);
			sqlcreate +=")";

			cmd = new SqlCommand(sqlcreate, conn);

			try	{cmd.ExecuteNonQuery();}
			catch{}
			finally{CloseConnection();}
		}


		private string InsertRecords(DataColumnCollection tableColumns, DataRowCollection tableRows)
		{
			string strreturn = "";
			int rowscreated=0;

			OpenConnection();

			try
			{
				foreach (DataRow row in tableRows)
				{
					//Build Insert Query string to insert records
					string sqlinsert = "INSERT INTO " + strTableName + "(";
					string sqlvalues = "VALUES (";
					
					object[] rowItems = row.ItemArray;
					// loop through each column in a specific DataTable.
					int ctrColumn = 0;
					foreach (DataColumn dc in tableColumns)
					{
						if(ctrColumn<tableColumns.Count-1)
							sqlinsert += "[" + dc.ColumnName.ToString() + "]" +  ",";
						else
							sqlinsert += "[" + dc.ColumnName.ToString() + "]" +  ") ";


						//Prepare string of a record to be inserted
						if(ctrColumn < tableColumns.Count-1)
							sqlvalues += "'" + rowItems[ctrColumn].ToString().Replace("'","''") + "',";				
						else
							sqlvalues += "'" + rowItems[ctrColumn].ToString().Replace("'","''") + "') ";	
					
						ctrColumn++;
					}
					
					sqlinsert = sqlinsert + sqlvalues;
					//Execute the command
					cmd = new SqlCommand(sqlinsert, conn);
					rowscreated += cmd.ExecuteNonQuery();
				}
				strreturn = "Records Imported Successfully!<br>";
				strreturn += rowscreated.ToString();
				strreturn += " rows created in table " + strTableName;				
			}
			catch(SqlException ae)
			{
				strreturn = "Error at Record Number: ";
				strreturn += rowscreated.ToString(); 
				strreturn += "<br>Message: " + ae.Message.ToString();
				strreturn += "<br>" + "Error importing. Please try again";
			}
			finally
			{
				CloseConnection();
			}

			return strreturn;
		}
		
		
		private void OpenConnection()
		{
			conn = new SqlConnection(ConnSQL);
			if( conn.State	!=	ConnectionState.Open)
				conn.Open();
		}

		
		private void CloseConnection()
		{
			if (conn!=null)
			{
				conn.Close();
				conn=null;
			}
		}
		
		
		private string SaveFileToServer(FileInfo filetable)
		{
			string strFilename;
			strServerPath=HttpContext.Current.Server.MapPath(strCsvDirOnServer);
			
			if (!Directory.Exists(strServerPath))
				Directory.CreateDirectory(strServerPath);

			strFilename = strServerPath + "/" + filetable.Name.ToString();
			filetable.CopyTo(strFilename, true);
			
			return strFilename;
		}
		
		#endregion
		
		#region Export Functionality
		public string GenerateCSVFile(SetProperties Properties)
		{
			strExportCSV = Properties.ExportCSVasName;
			strExportTable = Properties.ExportTableName;
			strExportCSVDir = Properties.ExportCSVDirOnServer;
			strExportAsCsvOrText = Properties.ExportAsCsvOrText;

			string strReturn="";
			DataSet dsSQL = new DataSet();
			dsSQL = ConnectSQLTable(strExportTable);
			DataColumnCollection tableColumns	=	dsSQL.Tables[0].Columns;
			DataRowCollection tableRows			=	dsSQL.Tables[0].Rows;

			string strPath= CreateCSVFileOnServer();
			strReturn = WriteInExportedFile(strPath, tableColumns, tableRows);

			return strReturn;
		}

		
		public DataSet ConnectSQLTable(string filetable)
		{
			DataSet dsAccess = new DataSet();
			try
			{
				string sqlSelect;		
				OpenConnection();
				SqlDataAdapter objSQLDa;
				sqlSelect = "Select * from ["+ filetable +"]";
				objSQLDa = new SqlDataAdapter(sqlSelect,conn);
				objSQLDa.Fill(dsAccess);
			}
			catch{}
			finally	{ CloseConnection(); }

			return dsAccess;
		}

		
		private string CreateCSVFileOnServer()
		{
			string strFilename;	
			strServerPath=HttpContext.Current.Server.MapPath(strExportCSVDir);
			
			if (!Directory.Exists(strServerPath))
				Directory.CreateDirectory(strServerPath);

			strFilename = strServerPath + "/" + strExportCSV + ".csv";
			if (strExportAsCsvOrText.ToUpper() == "T")
				strFilename = strServerPath + "/" + strExportCSV + ".txt";

			if (strExportCSV.Substring(strExportCSV.Length-3,3)=="csv" || strExportCSV.Substring(strExportCSV.Length-3,3)=="txt")
				strFilename = strServerPath + "/" + strExportCSV;
			

			FileStream newFile	= new FileStream(strFilename, FileMode.Create, FileAccess.ReadWrite);
			newFile.Close();
	
			return strFilename;
		}

		
		private string WriteInExportedFile(string strPath, DataColumnCollection tableColumns, DataRowCollection tableRows)
		{
			string strReturn="";
			System.IO.StreamWriter File = new System.IO.StreamWriter(strPath);
			
			int rowscreated=0;
			string sqlinsert = "";
			try
			{
				//Loop through columns of table to generate first row of CSV file
				int ctrColumn = 0;
				foreach (DataColumn dc in tableColumns)
				{
					if(ctrColumn<tableColumns.Count-1)
						sqlinsert += dc.ColumnName.ToString() + ",";
					else
						sqlinsert += dc.ColumnName.ToString();

					ctrColumn++;
				}
				File.WriteLine(sqlinsert);
				
				foreach (DataRow row in tableRows)
				{
					sqlinsert = "";
					string sqlvalues = "";
					object[] rowItems = row.ItemArray;
					
					ctrColumn = 0;
					foreach (DataColumn dcol in tableColumns)
					{
						if(ctrColumn < tableColumns.Count-1)
							sqlvalues += rowItems[ctrColumn].ToString().Replace("''","'") + ",";
						else
							sqlvalues += rowItems[ctrColumn].ToString().Replace("''","'");

						ctrColumn++;
					}
					
					sqlinsert = sqlinsert + sqlvalues;	
					File.WriteLine(sqlinsert);
			
					rowscreated++;
				}
				strReturn = "Records Exported Successfully!<br>";
				strReturn += rowscreated.ToString();
				strReturn += " rows created in CSV file ";
				strReturn +=  "<a target=_blank href='" + strPath + "'>" + strPath + "</a>";
				File.Close();
			}
			catch(SqlException ae) //Error
			{
				strReturn = "Error at Record Number: ";
				strReturn += rowscreated.ToString();
				strReturn += "<br>Message: " + ae.Message.ToString() + "<br>";
				strReturn += "Error importing. Please try again";
			}
			finally
			{
				File.Close();
			}

			return strReturn;
		}

		#endregion
	
		#region Export Access Table to SQL
		public string GenerateAccessToSQLTable(SetProperties Properties)
		{
			ClsMSACSV objMSA = new ClsMSACSV();

			string strreturn = "";

			blnDropExistingTable = Properties.DropExistingTable;
			strTableName = Properties.TableName;
			
			DataSet dsMSA = new DataSet();
			dsMSA = objMSA.ConnectAccessTable(strTableName);
			DataColumnCollection tableColumns =	dsMSA.Tables[0].Columns;
			DataRowCollection tableRows	= dsMSA.Tables[0].Rows;

			if (TableExists()==1)
			{
				if (blnDropExistingTable)
				{
					DropTable();
					CreateTable(tableColumns);
				}
			}
			else
			{
				CreateTable(tableColumns);
			}

			strreturn = InsertRecords(tableColumns, tableRows);

			return strreturn;
		}
		#endregion

        #region Import data from CSV to Datatable
        public DataTable ImportCSVtoDt(FileInfo filetable, String filepath)
        {
            DataTable dt = new DataTable();
            DataSet myData = new DataSet();
            string str;
            try
            {
                ConnCSV = ConnCSV + filepath;//filetable.DirectoryName.ToString();
                string sqlSelect;
                OleDbConnection objOleDBConn;
                OleDbDataAdapter objOleDBDa;
                objOleDBConn = new OleDbConnection(ConnCSV);
                objOleDBConn.Open();
                sqlSelect = "select * from [" + filetable.Name.ToString() + "]";
                objOleDBDa = new OleDbDataAdapter(sqlSelect, objOleDBConn);
                objOleDBDa.Fill(dt);
                objOleDBDa.Fill(myData, "Table");
                myData.WriteXml(filepath+"test.xml");
                objOleDBConn.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return dt;
        }
        #endregion

        #region Import and insert record in temp table using CSV
        public string InserGenerateTable(SetProperties Properties, String filepath)
        {
            string strreturn = "";

            blnSaveFileOnServer = Properties.SaveFileOnServer;
            blnDropExistingTable = Properties.DropExistingTable;
            strCsvDirOnServer = Properties.CsvDirOnServer;
            strTableName = Properties.TableName;
            fi = Properties.FileInformation;

            DataSet dsCSV = new DataSet();
            dsCSV = ConnectFile(fi, filepath);
            DataColumnCollection tableColumns = dsCSV.Tables[0].Columns;
            DataRowCollection tableRows = dsCSV.Tables[0].Rows;

            if (TableExists() == 1)
            {
                if (blnDropExistingTable)
                {
                    DropTable();
                    CreateTable(tableColumns);
                }
            }
            else
            {
                CreateTable(tableColumns);
            }

            strreturn = InsertRecords(tableColumns, tableRows);

            if (blnSaveFileOnServer)
                SaveFileToServer(fi);

            return strreturn;
        }
        #endregion
    }
}