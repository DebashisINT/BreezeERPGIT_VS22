using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data.OleDb;
using DevExpress.Web;
using System.Xml;
using BusinessLogicLayer;

/// <summary>
/// Summary description for GenericXML
/// </summary>
public class GenericXML
{
    /*global variables declaratiion*/
    
    DataSet DsFinal = new DataSet();
    DBEngine oDbEngine;
    //string strCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();  MULTI
    string strCon = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
    public enum XWhichMethod { RecordExist, Read, Update, ReadAt, UpdateAt, Insert, Write, Delete, DeleteAt };
    AspxHelper oAxHr=new AspxHelper();

	public GenericXML()
	{
		
	}


    public void WriteXml(DataSet DsWriteXml, string FilePath)
    {
        if (DsWriteXml.Tables.Count > 0)
            if (DsWriteXml.Tables[0].Rows.Count > 0)
        
        DsWriteXml.WriteXml(HttpContext.Current.Server.MapPath(FilePath));
    }
    public DataSet ReadXml(string FilePath)
    {

        if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
        {
            DataSet DsReadXml = new DataSet();
            //DsReadXml.ReadXml(System.IO.Path.GetFullPath(FilePath));
            DsReadXml.ReadXml(HttpContext.Current.Server.MapPath(FilePath));
            return DsReadXml;
        }
        return null;
    }
    bool IsFileExist(string FilePath)
    {
        if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
        
            return true;
        return false;
    }
    bool IsFileExist(string FilePath, Boolean InsideApplication)
    {
        if (!InsideApplication)
        {
            if (File.Exists(FilePath))
                return true;
            return false;
        }
        else
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
           //if (File.Exists(System.IO.Path.GetFullPath(FilePath)))
                return true;
            return false;
        }
    }









/* .....................For creating XML file and adding datas in the XML file...............................*/

    public void Add_XML(String XMLPath,                //path for creating XML file
                           String[] XMLColumnName,    //name of the array containing the column names
                           String[] XMLColumnType,   //name of the array containing the column types
                           String[] XMLColumnValue, //values in the column
                           String cTableName       //name of the table where the data will be stored
                           )
    {
        DataSet DsXml = new DataSet();
        oDbEngine = new DBEngine(strCon);
        //AddValueXML(XMLPath, XMLColumnValue, XMLColumnType,XMLColumnName,cTableName);

        DataRow drXML;
        DataTable DtNewCreated = new DataTable();
        int PCounter=0;                                      //initialising counter value for RowId
        if (!IsFileExist(XMLPath))                          //if file doesnot exists i.e., XML file is creating for the first time
        {
            DtNewCreated=CreateXML(XMLPath, XMLColumnName, XMLColumnType); //returns the value of CreateXML in the DataTable
            drXML = DtNewCreated.NewRow();
            drXML[0] = 1;
        }
        else                                              // if the XML file has already been created
        {
           DsXml=ReadXml(XMLPath);                       //reading the existing XML file
           if (DsXml.Tables.Count > 0)
            {
                if (DsXml.Tables[0].Rows.Count > 0)
                {  /* PCounter is increasing by 1 for every row */
                    PCounter = Convert.ToInt32(DsXml.Tables[0].Rows[DsXml.Tables[0].Rows.Count - 1]["RowID"].ToString()) + 1;
                }
            }
            drXML = DsXml.Tables[0].NewRow();         //taking new row for the same existing table for adding new value 
            drXML[0] = PCounter;
        }
        for (int loopCount = 0; loopCount <XMLColumnValue.Length; loopCount++)
        {
            
            if (XMLColumnType[loopCount] == "I")         //checking for Integer type value
            {
                drXML[loopCount+1] = Convert.ToInt32(XMLColumnValue[loopCount]);

            }

            if (XMLColumnType[loopCount] == "S")        //checking for String type value
            {
                drXML[loopCount+1] = XMLColumnValue[loopCount].ToString();
            }
        }
        if (!IsFileExist(XMLPath))                       //if file doesnot exists i.e., XML file is creating for the first time
        {
            DtNewCreated.Rows.Add(drXML);               //adding the first Row in the XML file
            DsXml.Tables.Add(DtNewCreated);
        }
        else                                            // if the XML file has already been created
        {
            DsXml.Tables[0].Rows.Add(drXML);
        }
        DsXml.Tables[0].TableName = cTableName;        //giving the table name where the datas will be stored
        WriteXml(DsXml, XMLPath);                     // writing in the XML file

        DsXml.Clear();
        DsXml.Dispose();
        oDbEngine = null;

    }



    public DataTable CreateXML(string XMLPath,string []XMLColumnName,string []XMLColumnType) // for creating the XML file and give the column names and the Type of values in the column
    {

        DataTable DtInsertData = new DataTable();
        oDbEngine = new DBEngine(strCon);
   
        //-- Add columns to the data DtInsertData
        DtInsertData.Columns.Add("RowID", typeof(int)); //row 1 which is always static
        for(int loopCount=0;loopCount<XMLColumnName.Length;loopCount++)
        {
            if (XMLColumnType[loopCount] == "I")             //checking for Integer type value
            {
                
                DtInsertData.Columns.Add(XMLColumnName[loopCount],typeof(int));
               
            }

            if (XMLColumnType[loopCount] == "S")        //checking for String type value
            {
                DtInsertData.Columns.Add(XMLColumnName[loopCount],typeof(string));
            }
        }

        return DtInsertData;
    }

    ////public void AddValueXML(String XMLPath, string[] XMLColumnValue, string[] XMLColumnType, string[] XMLColumnName, string TableName)
    ////{

    ////    oDbEngine = new DBEngine(strCon);
    ////    DataRow drXML;
    ////    DataTable DtNewCreated = new DataTable();
    ////    int PCounter=0;
    ////    if (!IsFileExist(XMLPath))                          //if file doesnot exists i.e., XML file is creating for the first time
    ////    {
    ////        DtNewCreated=CreateXML(XMLPath, XMLColumnName, XMLColumnType);
    ////        drXML = DtNewCreated.NewRow();
    ////        drXML[0] = 1;
    ////    }
    ////    else
    ////    {
    ////       DsXml=ReadXml(XMLPath);
    ////       if (DsXml.Tables.Count > 0)
    ////        {
    ////            if (DsXml.Tables[0].Rows.Count > 0)
    ////            {
    ////                PCounter = Convert.ToInt32(DsXml.Tables[0].Rows[DsXml.Tables[0].Rows.Count - 1]["RowID"].ToString()) + 1;
    ////            }
    ////        }
    ////        drXML = DsXml.Tables[0].NewRow();
    ////        drXML[0] = PCounter;
    ////    }
    ////    for (int loopCount = 0; loopCount <XMLColumnValue.Length; loopCount++)
    ////    {
            
    ////        if (XMLColumnType[loopCount] == "I")         //for Integer type value
    ////        {
    ////            drXML[loopCount+1] = Convert.ToInt32(XMLColumnValue[loopCount]);

    ////        }

    ////        if (XMLColumnType[loopCount] == "S")        //for String type value
    ////        {
    ////            drXML[loopCount+1] = XMLColumnValue[loopCount].ToString();
    ////        }
    ////    }
    ////    if (!IsFileExist(XMLPath))                       //if file doesnot exists i.e., XML file is creating for the first time
    ////    {
    ////        DtNewCreated.Rows.Add(drXML);       
    ////        DsXml.Tables.Add(DtNewCreated);
    ////    }
    ////    else
    ////    {
    ////        DsXml.Tables[0].Rows.Add(drXML);
    ////    }
    ////    DsXml.Tables[0].TableName = TableName;
    ////    WriteXml(DsXml, XMLPath);
    ////}





    public void Add_XML(String XMLPath,                //path for creating XML file
                           String[] XMLColumnName,    //name of the array containing the column names
                           String[] XMLColumnValue, //values in the column
                           String cTableName       //name of the table where the data will be stored
                           )
    {
        DataSet DsXml = new DataSet();
        oDbEngine = new DBEngine(strCon);
        
        DataRow drXML;
        DataTable DtNewCreated = new DataTable();
        int PCounter = 0;                                      //initialising counter value for RowId
        if (!IsFileExist(XMLPath))                          //if file doesnot exists i.e., XML file is creating for the first time
        {
            DtNewCreated = CreateXML(XMLPath, XMLColumnName); //returns the value of CreateXML in the DataTable
            drXML = DtNewCreated.NewRow();
            drXML[0] = 1;
        }
        else                                              // if the XML file has already been created
        {
            DsXml = ReadXml(XMLPath);                       //reading the existing XML file
            if (DsXml.Tables.Count > 0)
            {
                if (DsXml.Tables[0].Rows.Count > 0)
                {  /* PCounter is increasing by 1 for every row */
                    PCounter = Convert.ToInt32(DsXml.Tables[0].Rows[DsXml.Tables[0].Rows.Count - 1]["RowID"].ToString()) + 1;
                }
            }
            drXML = DsXml.Tables[0].NewRow();         //taking new row for the same existing table for adding new value 
            drXML[0] = PCounter;
        }
        for (int loopCount = 0; loopCount < XMLColumnValue.Length; loopCount++)
        {

            drXML[loopCount + 1] = XMLColumnValue[loopCount].ToString();
           
        }
        if (!IsFileExist(XMLPath))                       //if file doesnot exists i.e., XML file is creating for the first time
        {
            DtNewCreated.Rows.Add(drXML);               //adding the first Row in the XML file
            DsXml.Tables.Add(DtNewCreated);
        }
        else                                            // if the XML file has already been created
        {
            DsXml.Tables[0].Rows.Add(drXML);
        }
        DsXml.Tables[0].TableName = cTableName;        //giving the table name where the datas will be stored
        WriteXml(DsXml, XMLPath);                     // writing in the XML file

        DsXml.Clear();
        DsXml.Dispose();
        oDbEngine = null;

    }



    public DataTable CreateXML(string XMLPath, string[] XMLColumnName) // for creating the XML file and give the column names and the Type of values in the column
    {

        DataTable DtInsertData = new DataTable();
        oDbEngine = new DBEngine(strCon);

        //-- Add columns to the data DtInsertData
        DtInsertData.Columns.Add("RowID", typeof(int)); //row 1 which is always static
        for (int loopCount = 0; loopCount < XMLColumnName.Length; loopCount++)
        {
           
            DtInsertData.Columns.Add(XMLColumnName[loopCount], typeof(string));
            
        }

        return DtInsertData;
    }




        
    

    /* ...........For creating XML file and adding datas in the XML file..............*/ 



    public void Discard_XML(String  XMLPath           //path for creating XML file
                           )
    {
        if (IsFileExist(XMLPath))
        {
            File.Delete(HttpContext.Current.Server.MapPath(XMLPath));
        }
    }

    public DataSet GetDataSet(String XMLPath             //path for creating XML file
                              )
    {
                    
        return ReadXml(XMLPath);
       
    }





    public DataSet Save_Records(String XMLPath,                   //path of the XML file
                             String StoredProcedureName,       //the name of the store procedure by which the datas will be saved in the database
                            string[] sqlParameterName,
                            string[] sqlParameterValue,
                            string[] sqlParameterType,
                            string SPXMLparameterName,
                            string SPXMLparameterType
                            )
    {
        DataSet DsSaveRecordXML = new DataSet();
        oDbEngine = new DBEngine(strCon);

        if (IsFileExist(XMLPath))
        {
            DsSaveRecordXML = ReadXml(XMLPath);
            

            //string[] sqlParameterName = new string[1];
            //string[] sqlParameterValue = new string[1];
            //string[] sqlParameterType = new string[1];

            /*only the first value of the  sqlParameterName[],sqlParameterValue[],sqlParameterType[] will be defined here, the rest of the values will be assigned from the page */
            sqlParameterName[0] = SPXMLparameterName;
            sqlParameterValue[0] = DsSaveRecordXML.GetXml();
            sqlParameterType[0] = SPXMLparameterType;

           return SQLProcedures.SelectProcedureArrDS(StoredProcedureName, sqlParameterName, sqlParameterType, sqlParameterValue);

         }
         return null;
    }

    public DataSet Save_Records(
                           String StoredProcedureName,       //the name of the store procedure by which the datas will be saved in the database
                           string[] sqlParameterName,
                           string[] sqlParameterValue,
                           string[] sqlParameterType
                           )
    {
        
        return SQLProcedures.SelectProcedureArrDS(StoredProcedureName, sqlParameterName, sqlParameterType, sqlParameterValue);

    }


    public void Update_XML(
                           String XMLPath,             //path for creating XML file
                           String[] XMLColumnName,    //name of the array containing the column names
                           String[] XMLColumnType,   //name of the array containing the rows
                           String[] XMLColumnValue, //values in the column
                           String cTableName,      //name of the table where the data will be stored
                           string eRowID,
                           string rowkey
                          )
      {
        DataSet DsUpdateXML = new DataSet();
        if (DsUpdateXML.Tables.Count > 0)
        { 
            DsUpdateXML.Tables.Remove(DsUpdateXML.Tables[0]);
            DsUpdateXML.Clear(); 
        }
        DsUpdateXML=ReadXml(XMLPath);
        DsUpdateXML.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXML.Tables[0].Columns["RowID"] };

        DataRow row = DsUpdateXML.Tables[0].Rows.Find(rowkey);
       
        //DsXml=ReadXml(XMLPath);                       //reading the existing XML file
        //   if (DsXml.Tables.Count > 0)
        //    {
        //        if (DsXml.Tables[0].Rows.Count > 0)
        //        {  /* PCounter is increasing by 1 for every row */
        //            PCounter = Convert.ToInt32(DsXml.Tables[0].Rows[DsXml.Tables[0].Rows.Count - 1]["RowID"].ToString()) + 1;
        //        }
        //    }
        //    row = DsXml.Tables[0].NewRow();         //taking new row for the same existing table for adding new value 
        //    row[0] = PCounter;
        
        for (int loopCount = 0; loopCount <XMLColumnValue.Length; loopCount++)
        {
            
            if (XMLColumnType[loopCount] == "I")         //checking for Integer type value
            {
                row[loopCount + 1] = Convert.ToInt32(XMLColumnValue[loopCount]);

            }

            if (XMLColumnType[loopCount] == "S")        //checking for String type value
            {
                row[loopCount + 1] = XMLColumnValue[loopCount].ToString();
            }
        }

        DsUpdateXML.AcceptChanges();
        DsUpdateXML.Tables[0].TableName = cTableName;        //giving the table name where the datas will be stored
        File.Delete(HttpContext.Current.Server.MapPath(XMLPath));
        WriteXml(DsUpdateXML, XMLPath);                     // writing in the XML file
    

        oDbEngine = null;

    }

    public void Update_XML(
                           String XMLPath,             //path for creating XML file
                           String[] XMLColumnName,    //name of the array containing the column names
                           String[] XMLColumnValue, //values in the column
                           String cTableName      //name of the table where the data will be stored
                           )
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(HttpContext.Current.Server.MapPath(XMLPath));

        XmlNodeList xnList = xml.SelectNodes("/NewDataSet/" + cTableName);
        foreach (XmlNode xn in xnList)
        {
            for (int loopCount = 0; loopCount < XMLColumnValue.Length; loopCount++)
            {
                xn[XMLColumnName[loopCount].ToString()].InnerText = XMLColumnValue[loopCount].ToString();
            }
            //xn["ProductType"].InnerText = TxtProductType.Text.Trim();
            //xn["FHlderTitle"].InnerText = TxtTitl1.Text.Trim();
            //xn["FHlderSufx"].InnerText = TxtSufx1.Text.Trim();

            xml.Save(HttpContext.Current.Server.MapPath(XMLPath));

        }

    }
    public void Delete_Row_XML(String XMLPath,             //path for creating XML file
                               string RowID
                               )
    {
        DataSet DsRowDeleteXML = new DataSet();
        if (File.Exists(HttpContext.Current.Server.MapPath(XMLPath)))
        {
            if (DsRowDeleteXML.Tables.Count > 0) { DsRowDeleteXML.Tables.Remove(DsRowDeleteXML.Tables[0]); DsRowDeleteXML.Clear(); }
            DsRowDeleteXML.ReadXml(HttpContext.Current.Server.MapPath(XMLPath));
            DsRowDeleteXML.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXML.Tables[0].Columns["RowID"] };
            DataRow row = DsRowDeleteXML.Tables[0].Rows.Find(RowID);
            DsRowDeleteXML.Tables[0].Rows.Remove(row);
            //e.Cancel = true;
            //WriteXml(DsRowDeleteXML, XMLPath);    
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(XMLPath)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(XMLPath));
                    if (DsRowDeleteXML.Tables.Count > 0)
                    {
                        if (DsRowDeleteXML.Tables[0].Rows.Count > 0)
                        {
                            DsRowDeleteXML.WriteXml(HttpContext.Current.Server.MapPath(XMLPath));
                        }
                    }
                }
            }
            catch
            {
                DsRowDeleteXML.Dispose();
            }
        }
        DsRowDeleteXML.Dispose();
    }


    public void BindXmlWithGridView(ASPxGridView GridViewName, String XMLPath)
    {
        AspxHelper oAspxHelper = new AspxHelper();
        DataSet DsRecordXML = new DataSet();

        if (File.Exists(HttpContext.Current.Server.MapPath(XMLPath)))
        {
            DsRecordXML.ReadXml(HttpContext.Current.Server.MapPath(XMLPath));

            if (DsRecordXML.Tables[0].Rows.Count > 0)
            {
                oAspxHelper.BindGrid(GridViewName, DsRecordXML, "Desc", "RowId");
            }
            else
            {
                oAspxHelper.BindGrid(GridViewName);
            }
        }
        else
            oAspxHelper.BindGrid(GridViewName);
    }

    public string[,] XmlMethods(XWhichMethod WhichMethod, string XmlPath, string XmlFileName, string XmlParentNode, string[,] XmlValuesToUpdate,
       int XmlUpdateIndex, int XmlReadDeleteAt)
    {
        //XmlDataSource XDs = new XmlDataSource();
        //XDs.DataFile = Server.MapPath(@"KRADoc\KRA_KRAFNKMI00000030_F_Saved");
        //XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes("NewDataSet/KRA").Item(0);
        //string CmpID = selectedNode.ChildNodes[3].InnerText;
        //selectedNode.ChildNodes[3].InnerText = "COP0000001";
        //XDs.Save();
        XmlDataSource XDs = new XmlDataSource();
        string XmlFileFullPath = XmlPath + XmlFileName;
        XDs.DataFile = HttpContext.Current.Server.MapPath(XmlFileFullPath);
        int strLoopCounterRow = 0, strLoopCounterCol = 0, ParentNodeCount = 0, ChildNodeCount = 0;
        string[,] Message;

        if (WhichMethod.ToString() == "Read")
        {
            XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Item(0);
            ParentNodeCount = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Count;
            ChildNodeCount = selectedNode.ChildNodes.Count;
            string[,] strRead = new string[ParentNodeCount, ChildNodeCount];
            foreach (XmlNode XNode in XDs.GetXmlDocument().SelectNodes(XmlParentNode))
            {
                for (strLoopCounterCol = 0; strLoopCounterCol < ChildNodeCount; strLoopCounterCol++)
                    strRead[strLoopCounterRow, strLoopCounterCol] = XNode.ChildNodes[strLoopCounterCol].InnerText;

                strLoopCounterRow++;
            }
            return strRead;
        }
        if (WhichMethod.ToString() == "ReadAt")
        {
            XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Item(0);
            ParentNodeCount = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Count;
            string[,] strRead = new string[ParentNodeCount, 1];
            foreach (XmlNode XNode in XDs.GetXmlDocument().SelectNodes(XmlParentNode))
            {
                strRead[strLoopCounterRow, 0] = XNode.ChildNodes[XmlReadDeleteAt].InnerText;
                strLoopCounterRow++;
            }
            return strRead;
        }
        if (WhichMethod.ToString() == "UpdateAt")
        {
            Message = new string[1, 1];
            XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Item(0);
            ParentNodeCount = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Count;
            ChildNodeCount = selectedNode.ChildNodes.Count;
            if (XmlUpdateIndex >= 0 && XmlUpdateIndex < ChildNodeCount)
            {
                int IndexPositionCounter = 0;
                foreach (XmlNode XNode in XDs.GetXmlDocument().SelectNodes(XmlParentNode))
                {
                    XNode.ChildNodes[XmlUpdateIndex].InnerText = XmlValuesToUpdate[0, 0];
                    try
                    {
                        XDs.Save();
                        Message[0, 0] = "Save Saved!!!";
                    }
                    catch
                    {
                        Message[0, 0] = "Problem To Save!!!";
                    }
                }
            }
            else
                Message[0, 0] = "Index Not Found!!!";

            return Message;
        }
        if (WhichMethod.ToString() == "Update")
        {
            Message = new string[1, 1];
            XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Item(0);
            ParentNodeCount = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Count;
            ChildNodeCount = selectedNode.ChildNodes.Count;
            foreach (XmlNode XNode in XDs.GetXmlDocument().SelectNodes(XmlParentNode))
            {
                for (strLoopCounterCol = 0; strLoopCounterCol < ChildNodeCount; strLoopCounterCol++)
                    XNode.ChildNodes[strLoopCounterCol].InnerText = XmlValuesToUpdate[strLoopCounterRow, strLoopCounterCol];

                strLoopCounterRow++;
            }
            try
            {
                XDs.Save();
                Message[0, 0] = "Save Saved!!!";
            }
            catch
            {
                Message[0, 0] = "Problem To Save!!!";
            }
            return Message;
        }
        if (WhichMethod.ToString() == "Delete")
        {
            Discard_XML(XmlFileFullPath);
        }
        if (WhichMethod.ToString() == "DeleteAt")
        {
            Message = new string[1, 1];
            XmlNode selectedNode = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Item(0);
            ParentNodeCount = XDs.GetXmlDocument().SelectNodes(XmlParentNode).Count;
            if (XmlReadDeleteAt >= 0 && XmlReadDeleteAt <= ParentNodeCount)
            {
                foreach (XmlNode XNode in XDs.GetXmlDocument().SelectNodes(XmlParentNode))
                {
                    ++strLoopCounterCol;
                    if (XmlReadDeleteAt == strLoopCounterCol)
                    {
                        XNode.ParentNode.RemoveChild(XNode);
                        XDs.Save();
                        Message[0, 0] = "Successfully Deleted";
                    }
                    
                }
            }
            else
                Message[0, 0] = "Index Not Found!!!";

            return Message;
        }
        return null;
    }

    public string AppendXMLFiles( string filePath,string[] fileNameList)
    {
        string savedXmlPath = null;
        
        string XmlPath = filePath + @"\" + HttpContext.Current.Session["UserID"].ToString();
        if (File.Exists(XmlPath))
        {
            try { File.Delete(XmlPath); }
            catch { File.Delete(XmlPath); }
        }
        
        string strXml = String.Empty;
        for (int i = 0; i < fileNameList.Length; i++)
        {
            savedXmlPath = fileNameList[i].ToString();

            //--------------------------Start Append Xml files-------
            //Get Master File XML
            XmlDocument XmlDocFile;

            DataSet DsXmlFileToXmlString = new DataSet();
            DsXmlFileToXmlString.ReadXml(savedXmlPath);
            strXml = DsXmlFileToXmlString.GetXml();

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
                DsAlreadyTableExist = null;
            }

            DsXmlFileToXmlString = null;
            strXml = null;
            //--------------------------End Append Xml Files--------- 
        }
        return XmlPath;
    }

    ///This Code is written for Lincency Module
    ///We had a Encrypted XML File and we are directly getting the value from without having
    ///decrypted file Physically..

    ////public string Decrypt_License(XWhichMethod WhichMethod, string XmlContent, string Company, string Segment)
    ////{
    ////    XmlDataSource XDs = new XmlDataSource();
    ////    XDs.ID = "XmlSource1";
    ////    XDs.Data = XmlContent;
    ////    string Result = "";

    ////    XmlDocument doc = XDs.GetXmlDocument();
    ////    if (Company != null)
    ////    {
    ////        XmlNode companyNode = doc.DocumentElement.SelectSingleNode("./Company/cName[@Value='" + Company + "']");
    ////        if (companyNode != null)
    ////        {
    ////            if (Segment != null)
    ////            {
    ////                XmlNode segmentNode = companyNode.SelectSingleNode("./Segments[@Value='" + Segment + "']");
    ////                if (segmentNode != null)
    ////                {
    ////                    Result = segmentNode.SelectSingleNode("./Expiry").InnerText.ToString();
    ////                }
    ////                else
    ////                    Result = "Segment Not Found!!!";
    ////            }
    ////        }
    ////        else
    ////            Result = "Company Not Found!!!";
    ////    }
    ////    return Result;
    ////}
    public string Decrypt_License(XWhichMethod WhichMethod, string XmlContent, string Company, string Segment)
    {
        XmlDataSource XDs = new XmlDataSource();
        XDs.ID = "XmlSource1";
        XDs.Data = XmlContent;
        string Result = "";

        XmlDocument doc = XDs.GetXmlDocument();
        if (Company != null)
        {
            //if (Company.Contains("&"))
            //    Company = Company.Replace("&", "&amp;");
            XmlNode companyNode = doc.DocumentElement.SelectSingleNode("./Company/cName[@Value='" + Company + "']");
            if (companyNode != null)
            {
                if (Segment != null)
                {
                    //if (Segment.Contains("&"))
                    //    Segment = Segment.Replace("&", "&amp;");
                    XmlNode segmentNode = companyNode.SelectSingleNode("./Segments[@Value='" + Segment + "']");
                    if (segmentNode != null)
                    {
                        Result = segmentNode.SelectSingleNode("./Expiry").InnerText.ToString();
                    }
                    else
                        Result = "Segment Not Found!!!";
                }
            }
            else
                Result = "Company Not Found!!!";
        }
        return Result;
    }

    public string Decrypt_License(XWhichMethod WhichMethod, string XmlContent, string[] XmlNodes, string[] XmlAttributeValue)
    {
        XmlDataSource XDs = new XmlDataSource();
        string strDataSourceCreateTime = DateTime.Now.ToString("yyyyMMddHHmmss");       
        XDs.ID = "XmlSource_" + strDataSourceCreateTime;
        XDs.Data = XmlContent;
        string Result = "";
        XmlDocument doc = XDs.GetXmlDocument();
        int GetCurrentNodeIndex = 0;
        try
        {
            foreach (string Xpath in XmlNodes)
            {
                if (GetCurrentNodeIndex == (XmlNodes.GetLength(0) - 1))
                {
                    Result = doc.SelectSingleNode(Xpath).InnerText.ToString();
                }
                else
                {
                    if (doc.SelectSingleNode(XmlNodes[GetCurrentNodeIndex]).Value.ToString() == XmlAttributeValue[GetCurrentNodeIndex])
                        GetCurrentNodeIndex++;
                    else
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Result = "Invalid License!!!";
        }
        finally
        {
            doc = null;
            XDs = null;            
        }
        return Result;
    }    
}
