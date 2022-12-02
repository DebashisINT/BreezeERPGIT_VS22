using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
public partial class Pages_GenericAjaxList : System.Web.UI.Page
{
    string SegmentName = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        DataTable DT = new DataTable();
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        ///////////////////////////////Generic Ajax List Call////////////////////////////////////////
        #region GenericAjaxList
        if (Request.QueryString["GenericAjaxList"] == "1")
        {
            string RequestLetter = Request.QueryString["letters"].ToString();
            string[] param = Request.QueryString["search_param"].ToString().Replace("--", "+").Replace("^^", "%").Split('$');
            string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;
            string strQuery_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
            string strQuery_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
            string strQuery_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
            string strQuery_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
            if (strQuery_Table != null)
            {
                strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
            }
            if (strQuery_FieldName != null)
            {
                strQuery_FieldName = strQuery_FieldName.Replace("RequestLetter", RequestLetter);
            }
            if (strQuery_WhereClause != null)
            {
                strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
            }
            DT = GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);
            if (DT.Rows.Count != 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                }
            }
            else
                Response.Write("No Record Found###No Record Found|");
        }
        #endregion
        #region GenericAjaxListSP
        if (Request.QueryString["GenericAjaxListSP"] == "1")
        {
            string RequestLetter = Request.QueryString["letters"].ToString();
            string[] param = Request.QueryString["search_param"].ToString().Split('$');
            char SplitChar = Convert.ToChar(param[4]);
            string ProcedureName = param[0].ToString();
            string[] InputName = param[1].Split(SplitChar);
            string[] InputType = param[2].Split(SplitChar);
            string SetRequestLetter = param[3].Replace("RequestLetter", RequestLetter);
            string[] InputValue = SetRequestLetter.Split(SplitChar);
            if (ProcedureName.Trim() != String.Empty && (InputName.Length == InputType.Length) && (InputType.Length == InputValue.Length))
            {
                DT = SelectProcedureArr(ProcedureName, InputName, InputType, InputValue);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            else
                Response.Write("No Record Found###No Record Found|");
        }
        #endregion
        ////////////////////////////End Generic Ajax List Call////////////////////////////////////////
    }
    ////////////////////////////Generic Ajax List Variable////////////////////////////////////////

    SqlConnection oSqlConnection = new SqlConnection();

    //////////////////////////////End Generic Ajax List Variable//////////////////////////////////

    public void GetConnection()
    {
        if (oSqlConnection.State.Equals(ConnectionState.Open))
        {
        }
        else
        {
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //DBConnectionRead  //DBConnectionDefault
            //oSqlConnection.ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"];MULTI
            oSqlConnection.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        oSqlConnection.Open();
        }
        //return lcon;
    }
    public void CloseConnection()
    {
        if (oSqlConnection.State.Equals(ConnectionState.Open))
        {
            oSqlConnection.Close();
        }
    }


    //////////////////////////Start Generic Ajax List Method/////////////////////////////////////////

    public DataTable GetDataTable(
                    String cTableName,     // TableName from which the field value is to be fetched
                    String cFieldName,     // The name if the field whose value needs to be returned
                    String cWhereClause)   // WHERE condition [if any]
    {
        // Now we construct a SQL command that will fetch fields from the Suplied table

        String lcSql;
        lcSql = "Select " + cFieldName + " from " + cTableName;
        if (cWhereClause != null)
        {
            lcSql += " WHERE " + cWhereClause;
        }
        //SqlConnection lcon = GetConnection();
        GetConnection();
        SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
        // createing an object of datatable        
        DataTable GetTable = new DataTable();
        lda.SelectCommand.CommandTimeout = 0;
        lda.Fill(GetTable);
        oSqlConnection.Close();
        return GetTable;

    }
    public DataTable GetDataTable(
                        String cTableName,     // TableName from which the field value is to be fetched
                        String cFieldName,     // The name if the field whose value needs to be returned
                        String cWhereClause,   // WHERE condition [if any]
                        string cOrderBy)       // Order by condition
    {
        // Now we construct a SQL command that will fetch fields from the Suplied table

        String lcSql;
        lcSql = "Select " + cFieldName + " from " + cTableName;
        if (cWhereClause != null)
        {
            lcSql += " WHERE " + cWhereClause;
        }
        if (cOrderBy != null)
        {
            lcSql += " Order By " + cOrderBy;
        }
        //SqlConnection lcon = GetConnection();
        GetConnection();
        SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
        // createing an object of datatable
        DataTable GetTable = new DataTable();
        lda.SelectCommand.CommandTimeout = 0;
        lda.Fill(GetTable);
        oSqlConnection.Close();
        return GetTable;

    }
    public DataTable GetDataTableGroup(
                        String cTableName,     // TableName from which the field value is to be fetched
                        String cFieldName,     // The name if the field whose value needs to be returned
                        String cWhereClause,   // WHERE condition [if any]
                        string groupBy)       // Group by condition
    {
        // Now we construct a SQL command that will fetch fields from the Suplied table

        String lcSql;
        lcSql = "Select " + cFieldName + " from " + cTableName;
        if (cWhereClause != null)
        {
            lcSql += " WHERE " + cWhereClause;
        }
        if (groupBy != null)
        {
            lcSql += " group By " + groupBy;
        }
        //SqlConnection lcon = GetConnection();
        GetConnection();
        SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
        // createing an object of datatable
        DataTable GetTable = new DataTable();
        lda.SelectCommand.CommandTimeout = 0;
        lda.Fill(GetTable);
        oSqlConnection.Close();
        return GetTable;

    }
    public DataTable GetDataTable(
                        String cTableName,     // TableName from which the field value is to be fetched
                        String cFieldName,     // The name if the field whose value needs to be returned
                        String cWhereClause,   // WHERE condition [if any]
                        string groupBy,         // Gropu by condition
                        string cOrderBy)        // Order by condition
    {
        // Now we construct a SQL command that will fetch fields from the Suplied table

        String lcSql;
        lcSql = "Select " + cFieldName + " from " + cTableName;
        if (cWhereClause != null)
        {
            lcSql += " WHERE " + cWhereClause;
        }
        if (groupBy != null)
        {
            lcSql += " group By " + groupBy;
        }
        if (cOrderBy != null)
        {
            lcSql += " Order By " + cOrderBy;
        }
        //SqlConnection lcon = GetConnection();
        GetConnection();
        SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
        // createing an object of datatable
        DataTable GetTable = new DataTable();
        lda.SelectCommand.CommandTimeout = 0;
        lda.Fill(GetTable);
        oSqlConnection.Close();
        return GetTable;

    }
    public DataTable SelectProcedureArr(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue)
    {
        GetConnection();
        SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, oSqlConnection);
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
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, 255));
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
            oSqlConnection.Close();
        }
        oSqlConnection.Close();
        return DT;
    }
    /////////////////////////End Generic Ajax List Method///////////////////////////////////////////


}
