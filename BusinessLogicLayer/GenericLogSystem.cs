using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using DataAccessLayer;
using BusinessLogicLayer;
namespace BusinessLogicLayer
{
    public class GenericLogSystem : VirtualPathProvider
    {
        ProcedureExecute proc = new ProcedureExecute();
        DBEngine oDbEngine;
        string XmlPath_IndexTable;
        string XmlPath_MasterTable;
        string XmlPath_DetailTable;
        int Counter;
        DataTable XmlDataTable;
        DateTime XmlServerDateTime;
        public enum LogState { Inserted, Updating, Updated, Deleting, Deleted, Error, XmlOpenForEdit, XmlAdd, XmlUpdated, XmlExit, XmlSaving, XmlSaved, XmlDeleting, XmlDeleted, XmlError }
        public enum LogType { ALL, JV, CB }
        Converter oconverter;
        #region Properties
        public string pXmlPath_IndexTable
        {
            get { return XmlPath_IndexTable; }
            set { XmlPath_IndexTable = value; }
        }
        public string pXmlPath_MasterTable
        {
            get { return XmlPath_MasterTable; }
            set { XmlPath_MasterTable = value; }
        }
        public string pXmlPath_DetailTable
        {
            get { return XmlPath_DetailTable; }
            set { XmlPath_DetailTable = value; }
        }
        public DateTime pXmlServerDateTime
        {
            get { return XmlServerDateTime; }
            set { XmlServerDateTime = value; }
        }
        public int XmlCounter
        {
            get { return Counter; }
            set { Counter = value; }
        }

        #endregion

        #region Dictionary
        Dictionary<string, string> LogTypeFullName = new Dictionary<string, string>();
        #endregion

        public GenericLogSystem()
        {
            //oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            oDbEngine = new DBEngine();
            pXmlPath_IndexTable = @"\LogFiles\Log_IndexTable";
            pXmlPath_MasterTable = @"\LogFiles\Log_MasterTable";
      SqlDataReader Dr = oDbEngine.GetReader("Select GetDate() as DateTime");
      //  SqlDataReader Dr = proc.GetReader("Select GetDate() as DateTime");
            if (Dr.HasRows)
            {
                Dr.Read();
                try
                {
                    XmlServerDateTime = Convert.ToDateTime(Dr["DateTime"].ToString());
                }
                catch { Dr.Close(); }
                finally { Dr.Close(); }
            }
            oconverter = new Converter();
            LogTypeFullName.Add(LogType.JV.ToString(), "JournalVoucher");
            LogTypeFullName.Add(LogType.CB.ToString(), "CashBank");
        }
        bool IsXmlFileExist(string strXmlPath)
        {
            string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + strXmlPath;

            if (File.Exists(Path.GetFullPath(LogFilesPath)))
            {
                return true;
            }
            return false;
        }
        DataTable GetTableSchema(string TableName)
        {
            return oDbEngine.GetDataTable("information_schema.columns", "(Select Count(*)  from information_schema.columns WHERE table_name = '" + TableName.Trim() + "') as TotalCol,column_name 'Column Name',data_type 'Data Type'", "table_name = '" + TableName.Trim() + "'");
        }
        void PrepareDetailXmlFile(DataTable DtTableDetail, DataTable DtTableSchema, string TableName, string RowRef)
        {
            try
            {
                string fname = "\\LogDetail.xml";
                pXmlPath_DetailTable = @"\LogFiles\" + TableName;
                string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + pXmlPath_DetailTable + fname;
                var Fpath = HttpContext.Current.Server.MapPath(LogFilesPath).Replace("OMS\\management\\dailytask\\ERP\\", "");
                // string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + pXmlPath_DetailTable;
                if (!IsXmlFileExist(pXmlPath_DetailTable))
                {
                    File.Open(Fpath, FileMode.Create).Close();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilesPath))
                    {
                        file.WriteLine("<?xml version=\"1.0\" standalone=\"yes\"?>");
                        file.WriteLine("<NewDataSet>");
                        file.WriteLine("</NewDataSet>");
                    }
                }
                int TotalColumn = Convert.ToInt32(DtTableSchema.Rows[0][0].ToString());
                int TotalRow = Convert.ToInt32(DtTableDetail.Rows.Count);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(LogFilesPath);
                XmlText elmNewValue;
                XmlElement elmRoot = xmlDoc.DocumentElement;


                elmRoot = xmlDoc.DocumentElement;

                for (int Row = 0; Row < TotalRow; Row++)
                {
                    XmlElement elmNew = xmlDoc.CreateElement(TableName);
                    elmRoot.AppendChild(elmNew);
                    for (int Col = 0; Col < TotalColumn; Col++)
                    {
                        elmNew = xmlDoc.CreateElement(DtTableSchema.Rows[Col][1].ToString());
                        elmNewValue = xmlDoc.CreateTextNode(DtTableDetail.Rows[Row][Col].ToString());
                        elmRoot.LastChild.AppendChild(elmNew);
                        elmRoot.LastChild.LastChild.AppendChild(elmNewValue);
                    }
                    elmNew = xmlDoc.CreateElement("Detail_RowRef");
                    elmNewValue = xmlDoc.CreateTextNode(RowRef);
                    elmRoot.LastChild.AppendChild(elmNew);
                    elmRoot.LastChild.LastChild.AppendChild(elmNewValue);
                }

                xmlDoc.Save(Fpath);
            }
            catch { }
        }
        void PrepareIndexXMLFile(string TableName, string TotalField)
        { try
            {
            string fname = "\\LogIndex.xml";
            DataSet XmlIndexDs = new DataSet();
            string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + pXmlPath_IndexTable + fname;
            if (IsXmlFileExist(pXmlPath_IndexTable))
            {
                XmlIndexDs.ReadXml(LogFilesPath);
                XmlIndexDs.Tables[0].PrimaryKey = new DataColumn[] { XmlIndexDs.Tables[0].Columns["Index_TableName"] };
                DataRow row = XmlIndexDs.Tables[0].Rows.Find(TableName);
                if (row != null) return;
                if (XmlIndexDs.Tables[0].Rows.Count > 0)
                {
                    XmlCounter = Convert.ToInt32(XmlIndexDs.Tables[0].Rows[XmlIndexDs.Tables[0].Rows.Count - 1]["Index_TableID"].ToString()) + 1;
                }
            }
            else
            {
                XmlCounter = 1;
                XmlIndexDs.Tables.Add(new DataTable());
                XmlIndexDs.Tables[0].Columns.Add("Index_TableID", typeof(int));//0
                XmlIndexDs.Tables[0].Columns.Add("Index_TableName", typeof(string));//1
                XmlIndexDs.Tables[0].Columns.Add("Index_TotalField", typeof(int));//2
                XmlIndexDs.Tables[0].Columns.Add("Index_ModifyDateTime", typeof(DateTime));//3
            }

            DataRow XmlIndexDr = XmlIndexDs.Tables[0].NewRow(); ;
            XmlIndexDr[0] = XmlCounter;
            XmlIndexDr[1] = TableName;
            XmlIndexDr[2] = TotalField;
            XmlIndexDr[3] = XmlServerDateTime;

            XmlIndexDs.Tables[0].Rows.Add(XmlIndexDr);
            XmlIndexDs.Tables[0].AcceptChanges();
            XmlIndexDs.Tables[0].TableName = "IndexTable";
            var Fpath = HttpContext.Current.Server.MapPath(LogFilesPath).Replace("OMS\\management\\dailytask\\ERP\\", "");
         
            //XmlIndexDs.WriteXml(HttpContext.Current.Server.MapPath(LogFilesPath).Replace("OMS\\management\\dailytask\\ERP\\" + fname, ""));
           XmlIndexDs.WriteXml(Fpath);
            }
        catch { }
        }
        void PrepareMasterXMLFile(string TableName, string LogStatus, string UserID, string RowRef, string PhysicalTableRowRef, string LogType)
        {
            try
            {
                string fname = "\\LogMaster.xml";
                DataSet XmlMasterDs = new DataSet();
                string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + pXmlPath_MasterTable + fname;
                var Fpath = HttpContext.Current.Server.MapPath(LogFilesPath).Replace("OMS\\management\\dailytask\\ERP\\", "");
                if (IsXmlFileExist(pXmlPath_MasterTable))
                {
                    XmlMasterDs.ReadXml(LogFilesPath);
                    if (XmlMasterDs.Tables[0].Rows.Count > 0)
                    {
                        XmlCounter = Convert.ToInt32(XmlMasterDs.Tables[0].Rows[XmlMasterDs.Tables[0].Rows.Count - 1]["Master_TableID"].ToString()) + 1;
                    }
                }
                else
                {
                    XmlCounter = 1;
                    XmlMasterDs.Tables.Add(new DataTable());
                    XmlMasterDs.Tables[0].Columns.Add("Master_TableID", typeof(int));//0
                    XmlMasterDs.Tables[0].Columns.Add("Master_TableName", typeof(string));//1
                    XmlMasterDs.Tables[0].Columns.Add("Master_LogStatus", typeof(string));//2
                    XmlMasterDs.Tables[0].Columns.Add("Master_UserID", typeof(string));//3
                    XmlMasterDs.Tables[0].Columns.Add("Master_LogDateTime", typeof(DateTime));//4
                    XmlMasterDs.Tables[0].Columns.Add("Master_RowRef", typeof(string));//5
                    XmlMasterDs.Tables[0].Columns.Add("Master_PhysicalTableRowRef", typeof(string));//6
                    XmlMasterDs.Tables[0].Columns.Add("Master_LogType", typeof(string));//7
                }


                DataRow XmlMasterDr = XmlMasterDs.Tables[0].NewRow();
                XmlMasterDr[0] = XmlCounter;
                XmlMasterDr[1] = TableName;
                XmlMasterDr[2] = LogStatus;
                XmlMasterDr[3] = UserID;
                XmlMasterDr[4] = XmlServerDateTime;
                XmlMasterDr[5] = RowRef;
                XmlMasterDr[6] = PhysicalTableRowRef;
                XmlMasterDr[7] = LogType;

                XmlMasterDs.Tables[0].Rows.Add(XmlMasterDr);
                XmlMasterDs.Tables[0].AcceptChanges();
                XmlMasterDs.Tables[0].TableName = "MasterTable";
                // XmlMasterDs.WriteXml(LogFilesPath);
                XmlMasterDs.WriteXml(Fpath);
            }
            catch { }
            
        }
        //LogCreation Main Methods
        public string GetLogID()
        {
            return oconverter.GetAutoGenerateNo() + "_" + HttpContext.Current.Session["UserID"].ToString();
        }
        public void CreateLog(string FromClause, string WhereClause, Enum LogState, string UserID, string PhysicalTableRowRef, string TableName, string AutoGenNumber, LogType LogType)
        {

            DataTable DtTableSchema = GetTableSchema(TableName);
            if (LogState.ToString() == "XmlSaving" || LogState.ToString() == "XmlDeleting")
            {
                DataTable DtFetchRecord = oDbEngine.GetDataTable(FromClause, "*", WhereClause);
                if (DtFetchRecord.Rows.Count > 0)
                {
                    PrepareIndexXMLFile(TableName, DtTableSchema.Rows[0][0].ToString());
                    PrepareMasterXMLFile(TableName, LogState.ToString(), UserID, AutoGenNumber, PhysicalTableRowRef, LogType.ToString());
                    PrepareDetailXmlFile(DtFetchRecord, DtTableSchema, TableName, AutoGenNumber);
                }
            }
            else
            {
                PrepareIndexXMLFile(TableName, DtTableSchema.Rows[0][0].ToString());
                PrepareMasterXMLFile(TableName, LogState.ToString(), UserID, AutoGenNumber, PhysicalTableRowRef, LogType.ToString());
            }
        }
        //End LogCreation Main Methods

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Fetch Data From Log
        public DataSet GetSingleXmlFileFromSameNameSeparateFile(string ModuleType, DateTime DateFrom, DateTime DateTo, Boolean IsWithDateRange, out string AllPathXmlFile)
        {
            //With DateRange Or WithOut DateRange
            string strWithDateRange = @"Select AuditTrail_XMLFile,AuditTrail_RefID from Trans_AuditTrail WHERE ((AuditTrail_DateFrom between Convert(Varchar,'" + DateFrom + @"',111) and Convert(Varchar,'" + DateTo + @"',111))
            or
            (AuditTrail_DateTo between Convert(Varchar,'" + DateFrom + @"',111) and Convert(Varchar,'" + DateTo + @"',111)))
            and
            AuditTrail_ModuleType='" + ModuleType + @"'
            and
            AuditTrail_XmlFileType='Master'";
            string strWithOutDateRange = @"Select AuditTrail_XMLFile,AuditTrail_RefID from Trans_AuditTrail WHERE AuditTrail_ModuleType='" + ModuleType + @"' and AuditTrail_XmlFileType='Master'";
            //Get Master File XML
            SqlDataReader DrSingleXmlFile;
            XmlDocument XmlDocFile;
            string XmlPath = ConfigurationManager.AppSettings["SaveCSVsql"] + @"\LogFiles\Log_" + ModuleType.Trim() + "_MasterTable_" + HttpContext.Current.Session["UserID"].ToString();
            string strOutCombineXmlfilePaths = null;
            DataSet DsGetAllTable;

            if (File.Exists(XmlPath))
            {
                try { File.Delete(XmlPath); }
                catch { File.Delete(XmlPath); }
            }
            //if (IsWithDateRange)
            //    DrSingleXmlFile = oDbEngine.GetReader(strWithDateRange);
            //else
            //    DrSingleXmlFile = oDbEngine.GetReader(strWithOutDateRange);
            if (IsWithDateRange)
                DrSingleXmlFile = proc.GetReader(strWithDateRange);
            else
                DrSingleXmlFile = proc.GetReader(strWithOutDateRange);

            string strRefID = String.Empty;
            while (DrSingleXmlFile.Read())
            {
                string strXml = DrSingleXmlFile.GetString(0);
                if (strRefID == String.Empty) strRefID = DrSingleXmlFile.GetString(1);
                else strRefID = strRefID + "," + DrSingleXmlFile.GetString(1);
                XmlDocFile = new XmlDocument();
                if (!File.Exists(XmlPath))
                {
                    XmlDocFile.LoadXml(strXml);
                    XmlDocFile.Save(XmlPath);
                }
                else
                {
                    DataSet DsAlreadyTableExist = new DataSet();
                    DsAlreadyTableExist.ReadXml(XmlPath);
                    int TotalColumn = Convert.ToInt32(DsAlreadyTableExist.Tables[0].Columns.Count);
                    int TotalRow = Convert.ToInt32(DsAlreadyTableExist.Tables[0].Rows.Count);
                    XmlDocFile.LoadXml(strXml);
                    XmlText elmNewValue;
                    XmlElement elmRoot = XmlDocFile.DocumentElement;


                    elmRoot = XmlDocFile.DocumentElement;

                    for (int Row = 0; Row < TotalRow; Row++)
                    {
                        XmlElement elmNew = XmlDocFile.CreateElement(DsAlreadyTableExist.Tables[0].TableName);
                        elmRoot.AppendChild(elmNew);
                        for (int Col = 0; Col < TotalColumn; Col++)
                        {
                            elmNew = XmlDocFile.CreateElement(DsAlreadyTableExist.Tables[0].Columns[Col].ColumnName);
                            elmNewValue = XmlDocFile.CreateTextNode(DsAlreadyTableExist.Tables[0].Rows[Row][Col].ToString());
                            elmRoot.LastChild.AppendChild(elmNew);
                            elmRoot.LastChild.LastChild.AppendChild(elmNewValue);
                        }
                    }
                    XmlDocFile.Save(XmlPath);
                }
            }
            DrSingleXmlFile.Close();
            oDbEngine.CloseConnection();

            //Add All Tables In DataSet & Get The Path Of All Table
            DsGetAllTable = new DataSet();
            DataSet DsTempGetXml;
            DataView DvTempGetXml;
            if (File.Exists(XmlPath))
            {
                DsTempGetXml = new DataSet();
                DsTempGetXml.ReadXml(XmlPath);
                DvTempGetXml = DsTempGetXml.Tables[0].DefaultView;
                DsGetAllTable.Tables.Add(DvTempGetXml.Table.Copy());
                strOutCombineXmlfilePaths = "'" + XmlPath + "'";
                DsTempGetXml.Clear();
                DvTempGetXml.Table.Clear();
            }
            //End Add All Tables In DataSet & Get The Path Of All Table

            //Get Detail File/Files XML
            DataSet DsGetDetailFile = new DataSet();
            string DetailTableNames = String.Empty;
            if (File.Exists(XmlPath))
            {
                DataRow[] result = null;
                DsGetDetailFile.ReadXml(XmlPath);
                string Expression = "Master_LogType='" + ModuleType.Trim() + "'";
                result = DsGetDetailFile.Tables[0].Select(Expression);
                string field = "Master_TableName";

                //loop through all the rows

                foreach (DataRow row in result)
                {
                    if (!DetailTableNames.Contains(row[field].ToString()))
                    {
                        if (DetailTableNames != String.Empty) DetailTableNames = DetailTableNames + "," + row[field].ToString();
                        else DetailTableNames = row[field].ToString();
                    }
                }
            }
            foreach (string TableName in DetailTableNames.Split(','))
            {
                XmlPath = ConfigurationManager.AppSettings["SaveCSVsql"] + @"\LogFiles\Log_" + ModuleType.Trim() + "_" + TableName + "_" + HttpContext.Current.Session["UserID"].ToString();
                if (File.Exists(XmlPath))
                {
                    try { File.Delete(XmlPath); }
                    catch { File.Delete(XmlPath); }
                }
                foreach (string strIndividualRefID in strRefID.Split(','))
                {
                    string Query = @"Select AuditTrailDetail_DetailXmlFile from Trans_AuditTrailDetail Where AuditTrailDetail_RefID='" + strIndividualRefID + @"'
                                and AuditTrailDetail_TableName='" + TableName + "'";
                    //DrSingleXmlFile = oDbEngine.GetReader(Query);
                    DrSingleXmlFile = proc.GetReader(Query);
                    while (DrSingleXmlFile.Read())
                    {
                        string strXml = DrSingleXmlFile.GetString(0);
                        XmlDocFile = new XmlDocument();
                        if (!File.Exists(XmlPath))
                        {
                            XmlDocFile.LoadXml(strXml);
                            XmlDocFile.Save(XmlPath);
                        }
                        else
                        {
                            DataSet DsAlreadyTableExist = new DataSet();
                            DsAlreadyTableExist.ReadXml(XmlPath);
                            int TotalColumn = Convert.ToInt32(DsAlreadyTableExist.Tables[0].Columns.Count);
                            int TotalRow = Convert.ToInt32(DsAlreadyTableExist.Tables[0].Rows.Count);
                            XmlDocFile.LoadXml(strXml);
                            XmlText elmNewValue;
                            XmlElement elmRoot = XmlDocFile.DocumentElement;


                            elmRoot = XmlDocFile.DocumentElement;

                            for (int Row = 0; Row < TotalRow; Row++)
                            {
                                XmlElement elmNew = XmlDocFile.CreateElement(DsAlreadyTableExist.Tables[0].TableName);
                                elmRoot.AppendChild(elmNew);
                                for (int Col = 0; Col < TotalColumn; Col++)
                                {
                                    elmNew = XmlDocFile.CreateElement(DsAlreadyTableExist.Tables[0].Columns[Col].ColumnName);
                                    elmNewValue = XmlDocFile.CreateTextNode(DsAlreadyTableExist.Tables[0].Rows[Row][Col].ToString());
                                    elmRoot.LastChild.AppendChild(elmNew);
                                    elmRoot.LastChild.LastChild.AppendChild(elmNewValue);
                                }
                            }
                            XmlDocFile.Save(XmlPath);
                        }
                    }
                    DrSingleXmlFile.Close();
                }
                //Add All Tables In DataSet & Get The Path Of All Table
                if (File.Exists(XmlPath))
                {
                    DsTempGetXml = new DataSet();
                    DsTempGetXml.ReadXml(XmlPath);
                    DvTempGetXml = DsTempGetXml.Tables[0].DefaultView;
                    DsGetAllTable.Tables.Add(DvTempGetXml.Table.Copy());
                    strOutCombineXmlfilePaths = strOutCombineXmlfilePaths + ",'" + XmlPath + "'";
                    DsTempGetXml.Clear();
                    DvTempGetXml.Table.Clear();
                }
                //End Add All Tables In DataSet & Get The Path Of All Table
            }
            oDbEngine.CloseConnection();
            AllPathXmlFile = strOutCombineXmlfilePaths;
            return DsGetAllTable;
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////-------

        //EOD Processing 

        public int EODProcess_ForLog(Enum EModuleType)
        {
            if (EModuleType.ToString() != String.Empty)
            {
                string[] filePaths;
                string[] sqlParameterName = new string[7];
                string[] sqlParameterType = new string[7];
                string[] sqlParameterValue = new string[7];
                string[] sqlParameterSize = new string[7];
                string strXmlFilePath = String.Empty;
                string strXmlDetailFilePath = String.Empty;
                DateTime DateFrom = Convert.ToDateTime("1900-01-01");
                DateTime DateTo = Convert.ToDateTime("1900-01-01");
                DataSet XmlMasterDs = new DataSet();
                string LogFilesPath = ConfigurationManager.AppSettings["SaveCSVsql"] + pXmlPath_MasterTable;
                string FileName = null;
                int FileCount;
                int TotalNoLogType = Enum.GetValues(typeof(LogType)).Length;
                string ModuleTypeFullName = null;
                bool IsModuleType_All = false;

                foreach (string ModuleType in Enum.GetNames(typeof(LogType)))
                {
                    filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\LogFiles\");
                    FileCount = filePaths.Length - 1;
                    if (ModuleType == "ALL")
                    {
                        IsModuleType_All = true;
                        continue;
                    }
                    ModuleTypeFullName = LogTypeFullName[ModuleType].ToString();
                    while (FileCount >= 0)
                    {
                        FileName = Path.GetFileName(filePaths[FileCount].ToString());
                        if (FileName == "Log_IndexTable" || FileName == "Log_MasterTable")
                        {
                            if (strXmlFilePath.Trim() != String.Empty)
                                strXmlFilePath = strXmlFilePath + ",'" + filePaths[FileCount].ToString() + "'";
                            else
                                strXmlFilePath = "'" + filePaths[FileCount].ToString() + "'";
                        }
                        else
                        {
                            if (!FileName.Contains("Log"))
                            {
                                if (strXmlDetailFilePath.Trim() != String.Empty)
                                {
                                    if (filePaths[FileCount].ToString().Contains(ModuleTypeFullName))
                                    {
                                        strXmlDetailFilePath = strXmlDetailFilePath + ",'" + filePaths[FileCount].ToString() + "'";
                                    }
                                }
                                else
                                    strXmlDetailFilePath = "'" + filePaths[FileCount].ToString() + "'";
                            }
                        }
                        FileCount = FileCount - 1;
                    }
                    strXmlDetailFilePath = strXmlDetailFilePath == null ? "None" : strXmlDetailFilePath.Trim();
                    strXmlFilePath = strXmlFilePath == null ? "None" : strXmlFilePath.Trim();
                    sqlParameterName[0] = "AuditTrail_XmlFilePath";
                    sqlParameterValue[0] = strXmlFilePath;
                    sqlParameterType[0] = "V";
                    sqlParameterSize[0] = "200";
                    sqlParameterName[1] = "AuditTrailDetail_XmlfilePath";
                    sqlParameterValue[1] = strXmlDetailFilePath;
                    sqlParameterType[1] = "V";
                    sqlParameterSize[1] = "200";

                    if (IsXmlFileExist(pXmlPath_MasterTable))
                    {
                        XmlMasterDs.ReadXml(LogFilesPath);
                        DataView XmlMasterView = new DataView();
                        XmlMasterView = XmlMasterDs.Tables[0].DefaultView;
                        XmlMasterView.Sort = "Master_LogDateTime";
                        int MasterDsMaxCount = XmlMasterDs.Tables[0].Rows.Count - 1;

                        DateFrom = Convert.ToDateTime(XmlMasterDs.Tables[0].Rows[0]["Master_LogDateTime"].ToString());
                        DateTo = Convert.ToDateTime(XmlMasterDs.Tables[0].Rows[MasterDsMaxCount]["Master_LogDateTime"].ToString());
                    }
                    sqlParameterName[2] = "AuditTrail_DateFrom";
                    sqlParameterValue[2] = DateFrom.ToString();
                    sqlParameterType[2] = "D";
                    sqlParameterSize[2] = "";
                    sqlParameterName[3] = "AuditTrail_DateTo";
                    sqlParameterValue[3] = DateTo.ToString();
                    sqlParameterType[3] = "D";
                    sqlParameterSize[3] = "";
                    sqlParameterName[4] = "AuditTrail_UserID";
                    sqlParameterValue[4] = HttpContext.Current.Session["UserID"].ToString();
                    sqlParameterType[4] = "I";
                    sqlParameterSize[4] = "";
                    sqlParameterName[5] = "AuditTrail_ModuleType";
                    sqlParameterValue[5] = (IsModuleType_All) ? "AL" : ModuleType;
                    sqlParameterType[5] = "V";
                    sqlParameterSize[5] = "2";

                    DataTable DtEOD = null;
                    //string UserLastSegment = HttpContext.Current.Session["userlastsegment"].ToString();
                    string SegmentID = "R";
                    //if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6")
                    //{
                    //    if (UserLastSegment == "9" || UserLastSegment == "10")
                    //    {
                    //        DtEOD = oDbEngine.GetDataTable("tbl_master_CompanyExchange", "Exch_InternalID", "Exch_CompID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Exch_TMCode='" + HttpContext.Current.Session["UserSegID"].ToString() + "'");
                    //        SegmentID = DtEOD.Rows[0][0].ToString();
                    //        DtEOD = null;
                    //    }
                    //    else
                    //    {
                    //        SegmentID = HttpContext.Current.Session["UserSegID"].ToString();
                    //    }
                    //}
                    sqlParameterName[6] = "ExchangeSegmentID";
                    sqlParameterValue[6] = SegmentID;
                    sqlParameterType[6] = "V";
                    sqlParameterSize[6] = "10";

                    if (SegmentID != null)
                    {
                        int RowAffected = SQLProcedures.Execute_StoreProcedure("Insert_AuditTrail", sqlParameterName, sqlParameterType, sqlParameterValue, sqlParameterSize);
                        if (RowAffected == 1)
                        {
                            Array.ForEach(Directory.GetFiles(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\LogFiles\"), delegate(string path)
                            {
                                if (path.Contains(ModuleTypeFullName) || path.Contains("Log_IndexTable") || path.Contains("Log_MasterTable"))
                                {
                                    File.Delete(path);
                                }
                            });
                        }
                    }
                    strXmlFilePath = String.Empty;
                    strXmlDetailFilePath = String.Empty;
                }
            }
            return 0;
        }
        //End EOD Processing 
    }
}
