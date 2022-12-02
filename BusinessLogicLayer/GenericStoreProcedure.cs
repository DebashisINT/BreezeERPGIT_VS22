using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace BusinessLogicLayer
{
    public class GenericStoreProcedure
    {
        #region Enum
        public enum ParamType { ExParam, XParam }
        public enum ParamDBType { Int, Char, Varchar, Numeric, Decimal, bigInt, Bit, image, Text, DateTime }
        public enum ParamDBNull { None }
        #endregion

        #region Global Variable
        string XmlContent, strParamName, strParamType, strParamValue, strDBType, strDBTypeSize;
        SqlConnection SqlCon;
        SqlDataAdapter SqlDA;
        #endregion

        //Constructor
        public GenericStoreProcedure()
        {
            //initialize Global Variable
            XmlContent = @"
        DECLARE @XMLDoc int
        EXEC SP_XML_PREPAREDOCUMENT @XMLDoc OUTPUT,@XmlFile
	    Select * into #XmlTempTable
	    from OPENXML (@XMLDoc,'/NewDataSet/XmlStream',2)
	    WITH";

            strParamName = String.Empty;
            strDBType = String.Empty;
            strDBTypeSize = String.Empty;
            strParamValue = String.Empty;
            strParamType = String.Empty;

            //SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionDefault"].ToString());
        }

        public string PrepareSpContent(string[] Param, string SpName)
        {
            string strPrepareXmlTable = String.Empty;
            string strPrepareExParam = String.Empty;
            foreach (string str in Param)
            {
                string[] StrSplit = str.Split('|');
                strParamName = StrSplit[0];
                strDBType = StrSplit[1];
                strDBTypeSize = StrSplit[2] == "None" ? String.Empty : "(" + StrSplit[2] + ")";
                strParamType = StrSplit[4];

                if (strParamType == GenericStoreProcedure.ParamType.XParam.ToString())
                {
                    if (strPrepareXmlTable == String.Empty)
                        strPrepareXmlTable = "(" + strParamName + " " + strDBType + strDBTypeSize + ",";
                    else
                        strPrepareXmlTable = strPrepareXmlTable + strParamName + " " + strDBType + strDBTypeSize + ",";
                }
                else if (strParamType == GenericStoreProcedure.ParamType.ExParam.ToString())
                {
                    strPrepareExParam = strPrepareExParam + "@" + strParamName + " " + strDBType + " " + strDBTypeSize + "\n,";
                }
            }

            strPrepareXmlTable = (strPrepareXmlTable[strPrepareXmlTable.Length - 1] == ',' ? strPrepareXmlTable.Substring(0, strPrepareXmlTable.Length - 1)
                : strPrepareXmlTable) + @")
        
        ----Your Logic
    
        ---At The End of Sp Remove XParam
        EXEC SP_XML_REMOVEDOCUMENT @XMLDoc";

            strPrepareXmlTable = XmlContent + strPrepareXmlTable;
            if (strPrepareExParam.Trim() != string.Empty)
            {
                strPrepareExParam = strPrepareExParam[strPrepareExParam.Length - 1] == ',' ? strPrepareExParam.Substring(0, strPrepareExParam.Length - 1)
                    : strPrepareExParam;
                return
                    @"--------------------------External Parameter Format Below---------------
            Create/Alter Procedure " + SpName + @"(
            @XmlFile nVarchar(Max)," + "\n" +
                strPrepareExParam +
                    @")
            As
            Begin 
                  ---------------------------XParam Format Below----------------------------" +
                    strPrepareXmlTable + @"
            End";

            }
            else
            {
                return
                    @"--------------------------External Parameter Format Below---------------
            Create/Alter Procedure " + SpName + @"
            @XmlFile nVarchar(Max)
            As
            Begin 
                  ---------------------------XParam Format Below----------------------------" +
                    strPrepareXmlTable + @"
            End";
            }
        }
        DataSet CreateXmlDs(string[] Param, int TotalNoOfXmlCol)
        {
            string[] StrSplit;
            DataSet DsParam = new DataSet();
            DataTable DtParam = DsParam.Tables.Add();
            object[] rowVals = new object[TotalNoOfXmlCol];
            int Counter = 0;

            foreach (string str in Param)
            {
                StrSplit = str.Split('|');
                strParamName = StrSplit[0];
                strParamValue = StrSplit[3];
                strParamType = StrSplit[4];
                if (strParamType == ParamType.XParam.ToString())
                {
                    DtParam.Columns.Add(strParamName, typeof(string));
                    rowVals[Counter++] = strParamValue;
                }
            }
            DataRowCollection rowCollection = DtParam.Rows;
            rowCollection.Add(rowVals);
            DsParam.Tables[0].TableName = "XmlStream";

            return DsParam;
        }
        public string PrepareExecuteSpContent(string[] Param, string SpName)
        {
            string strPrepareExParam = String.Empty;
            foreach (string str in Param)
            {
                if (str != null)
                {
                    if (str.Trim() != "")
                    {
                        string[] StrSplit = str.Split('|');
                        strParamName = StrSplit[0];
                        strDBType = StrSplit[1];
                        strDBTypeSize = StrSplit[2] == "None" ? String.Empty : "(" + StrSplit[2] + ")";
                        strParamValue = StrSplit[3];
                        strParamType = StrSplit[4];

                        if (strParamType == GenericStoreProcedure.ParamType.ExParam.ToString())
                        {
                            if (strDBType == ParamDBType.Char.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToChar(strParamValue) + "'";
                            }
                            if (strDBType == ParamDBType.Int.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "=" + Convert.ToInt32(strParamValue);
                            }
                            else if (strDBType == ParamDBType.Varchar.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToString(strParamValue) + "'";
                            }
                            else if (strDBType == ParamDBType.Text.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToString(strParamValue) + "'";
                            }

                            else if (strDBType == ParamDBType.Decimal.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToDecimal(strParamValue) + "'";
                            }
                            else if (strDBType == ParamDBType.Numeric.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToDouble(strParamValue) + "'";
                            }
                            else if (strDBType == ParamDBType.DateTime.ToString())
                            {
                                strPrepareExParam = strPrepareExParam + "@" + strParamName + "='" + Convert.ToDateTime(strParamValue) + "'";
                            }
                            strPrepareExParam = strPrepareExParam + ",";
                        }
                    }
                }
            }
            if (strPrepareExParam != null)
            {
                if (strPrepareExParam.Trim() != "")
                    strPrepareExParam = strPrepareExParam[strPrepareExParam.Length - 1] == ',' ? strPrepareExParam.Substring(0, strPrepareExParam.Length - 1)
                        : strPrepareExParam;
            }
            else
                strPrepareExParam = "";


            return "Exec " + SpName + "\n" +
            strPrepareExParam;
        }
        public DataSet Procedure_DataSet(string[] Param, string SpName)
        {

            //string strSqlCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();  MULTI
            string strSqlCon = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            // added for read only user
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    strSqlCon = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                }
            }




            DataSet DsReturn = new DataSet();
            SqlParameter parameter;
            int XmlColumnCounter = 0;
            bool IsXmlParamExists = false;
            string[] StrSplit;
            try
            {
                using (SqlCon = new SqlConnection(strSqlCon))
                {
                    SqlDA = new SqlDataAdapter(SpName, SqlCon);
                    SqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                    foreach (string str in Param)
                    {
                        if (str != null)
                        {
                            if (str.Trim() != "")
                            {
                                StrSplit = str.Split('|');
                                strParamName = StrSplit[0];
                                strDBType = StrSplit[1];
                                strDBTypeSize = StrSplit[2] == "None" ? String.Empty : StrSplit[2];
                                strParamValue = StrSplit[3];
                                strParamType = StrSplit[4];
                                // Add the parameters.
                                if (strParamType == ParamType.ExParam.ToString())
                                {
                                    if (strDBType == ParamDBType.Char.ToString())
                                    {
                                        SqlDA.SelectCommand.Parameters.Add(strParamName, SqlDbType.Char, Convert.ToInt32(strDBTypeSize), strParamName);
                                    }
                                    if (strDBType == ParamDBType.Int.ToString())
                                    {
                                        SqlDA.SelectCommand.Parameters.Add(strParamName, SqlDbType.Int);
                                    }
                                    else if (strDBType == ParamDBType.Varchar.ToString())
                                    {
                                        SqlDA.SelectCommand.Parameters.Add(strParamName, SqlDbType.VarChar, Convert.ToInt32(strDBTypeSize), strParamName);
                                    }
                                    else if (strDBType == ParamDBType.Text.ToString())
                                    {
                                        SqlDA.SelectCommand.Parameters.Add(strParamName, SqlDbType.VarChar, Convert.ToInt32(strDBTypeSize), strParamName);
                                    }

                                    else if (strDBType == ParamDBType.Decimal.ToString())
                                    {
                                        parameter = new SqlParameter(strParamName, SqlDbType.Decimal);
                                        parameter.Precision = Convert.ToByte(strDBTypeSize.Split(',')[0]);
                                        parameter.Scale = Convert.ToByte(strDBTypeSize.Split(',')[1]);
                                        parameter.SourceColumn = strParamName;
                                        SqlDA.SelectCommand.Parameters.Add(parameter);
                                    }
                                    else if (strDBType == ParamDBType.Numeric.ToString())
                                    {
                                        parameter = new SqlParameter(strParamName, SqlDbType.Decimal);
                                        parameter.Precision = Convert.ToByte(strDBTypeSize.Split(',')[0]);
                                        parameter.Scale = Convert.ToByte(strDBTypeSize.Split(',')[1]);
                                        parameter.SourceColumn = strParamName;
                                        SqlDA.SelectCommand.Parameters.Add(parameter);
                                    }
                                    else if (strDBType == ParamDBType.DateTime.ToString())
                                    {
                                        SqlDA.SelectCommand.Parameters.Add(strParamName, SqlDbType.Date);
                                    }

                                    SqlDA.SelectCommand.Parameters[strParamName].Value = strParamValue;
                                }
                                else
                                {
                                    //Counter For XParam Column
                                    IsXmlParamExists = true;
                                    XmlColumnCounter++;

                                }
                            }
                        }

                    }
                    //This is Defalut Param For All Sps If There is a Single XParam Param
                    if (IsXmlParamExists)
                        SqlDA.SelectCommand.Parameters.Add("XmlFile", SqlDbType.VarChar, -1, CreateXmlDs(Param, XmlColumnCounter).GetXml());
                    SqlDA.SelectCommand.CommandTimeout = 0;
                    SqlDA.Fill(DsReturn);
                }
            }
            catch
            {

            }
            finally
            {
                SqlDA.Dispose();
                SqlCon.Close();
            }
            return DsReturn;
        }
        public DataTable Procedure_DataTable(string[] Param, string SpName)
        {
            DataSet DsReturn = Procedure_DataSet(Param, SpName);
            if (DsReturn.Tables.Count > 0)
                if (DsReturn.Tables[0].Rows.Count > 0)
                    if (DsReturn.Tables[0].Rows[0][0] != null)
                        return DsReturn.Tables[0];


            return null;
        }
        public string Procedure_String(string[] Param, string SpName)
        {
            DataSet DsReturn = Procedure_DataSet(Param, SpName);
            if (DsReturn.Tables.Count > 0)
                if (DsReturn.Tables[0].Rows.Count > 0)
                    if (DsReturn.Tables[0].Rows[0][0] != null)
                        if (DsReturn.Tables[0].Rows[0][0].ToString().Trim() != String.Empty)
                            return DsReturn.Tables[0].Rows[0][0].ToString();

            return null;
        }
        public int Procedure_Int(string[] Param, string SpName)
        {
            DataSet DsReturn = Procedure_DataSet(Param, SpName);
            if (DsReturn.Tables.Count > 0)
                if (DsReturn.Tables[0].Rows.Count > 0)
                    if (DsReturn.Tables[0].Rows[0][0] != null)
                        if (DsReturn.Tables[0].Rows[0][0].ToString().Trim() != String.Empty)
                            return Convert.ToInt32(DsReturn.Tables[0].Rows[0][0].ToString());

            return -1;
        }
    }
}
