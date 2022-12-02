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
using BusinessLogicLayer;
/// <summary>
/// Summary description for clsCdslHolding
/// </summary>
public class clsCdslHolding
{
    //public DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
    public DBEngine oDBEngine = new DBEngine();
    private static DataTable dates;
    Converter objConverter = new Converter();
    Converter OConvert = new Converter();
    public int chkStatus;
    PagedDataSource pds = new PagedDataSource();
    public string holdingdate;
    public string HoldingDate
    {
        get
        {
            return holdingdate;
        }

        set
        {
            holdingdate = value;
        }
    }
	public clsCdslHolding()
	{
		//
		// TODO: Add constructor logic here
        //
	}
    public DataTable showCdslHolding(string BOID)
    {
        return bindGrid(BOID);
    }
    public string populateCmbDate()
    {
        string strdate;
        dates = oDBEngine.GetDataTable(" Trans_CdslHolding ", " distinct CdslHolding_HoldingDateTime as date, CdslHolding_HoldingDateTime ", null, "CdslHolding_HoldingDateTime desc");
        if (dates.Rows.Count > 0)
        {
            strdate = (Convert.ToDateTime(dates.Rows[0][0].ToString())).ToString();
            return strdate;
        }
        return "1900-01-01 00:00:00.000"; 
    }
    public string populateCmbTime()
    {
        DataTable dttime = new DataTable();
        //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + dt + "' ", " CdslHolding_HoldingDateTime desc");
        //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + txtDate.Value.ToString() + "' ", " CdslHolding_HoldingDateTime desc");
        dttime = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " convert(varchar(12),CdslHolding_HoldingDateTime,113) = convert(varchar(12),cast('" + populateCmbDate() + "' as datetime),113) ", " CdslHolding_HoldingDateTime desc");
        string strtime=dttime.Rows[0]["CdslHolding_HoldingDateTime"].ToString();
        return strtime;
        
    }
    DataTable FetchBoid(string BOID)
    {
        string orderBy = "";
        DataTable dtable = new DataTable();
        string strgetdatetime=populateCmbTime();
        HoldingDate = strgetdatetime;
        string[] dat = strgetdatetime.Split(' ');
        string where = " CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";
        where = where + "and CdslHolding_HoldingDateTime between '" + dat[0] + " 00:00:00 AM'  and '" + strgetdatetime + "'";
        where = where + " and CdslClients_BOID  in (" + BOID + ")";
        dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName", where, null);
        return dtable;
    }

    DataTable bindGrid(string BOID)
    {
        DataTable dt = FetchBoid(BOID);

        pds.DataSource = dt.DefaultView;
        
        if (pds.DataSourceCount < 1)
        {
           return bindRepeter(BOID);
        }
        else
        {
           return bindRepeter(BOID);
            
        }

        return null; 

    }
    DataTable bindRepeter(string BOID)
    {
        try
        {
            string select, where, id, sql;
            id = BOID;
            string strdatetime=populateCmbTime();
            string[] date = strdatetime.Split(' ');
            DataTable dt1 = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationSSettings.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("cdslHoldingReport1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("startTime", date[0] + " 00:00:00 AM");
                cmd.Parameters.AddWithValue("endTime", strdatetime);
                cmd.Parameters.AddWithValue("isin", "NA");
                cmd.Parameters.AddWithValue("settlementId", "NA");
                if (BOID.Length == 16)
                {
                    cmd.Parameters.AddWithValue("boid", "'" + BOID.Substring(8) + "'");
                }
                else
                {
                    cmd.Parameters.AddWithValue("boid", "NoId");
                }
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt1);
            }
            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                if (dt1.Rows[k]["CdslHolding_CurrentBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_CurrentBalance"].ToString()));

                if (dt1.Rows[k]["CdslHolding_FreeBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_FreeBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PledgeBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PledgeBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_EarmarkedBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_EarmarkedBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PendingRematBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingRematBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PendingDematBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingDematBalance"].ToString()));

                if (dt1.Rows[k]["Rate"] != DBNull.Value)
                    dt1.Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["Rate"].ToString()));

                if (dt1.Rows[k]["ISINVAlue"] != DBNull.Value)
                    dt1.Rows[k]["ISINVAlue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString()));
                //VALUE_Sum += Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString());
                //ViewState["ISINVAlue"] = VALUE_Sum;

            }
            return dt1;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public double ShowCdslHoldingValueTotal(string dpid,string BOID)
    {
        DataTable dt = showCdslHolding(dpid+BOID);
        double value = 0;
        int count = dt.Rows.Count;
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ISINValue"] != DBNull.Value)
                {
                    value = value + Convert.ToDouble(dr["ISINValue"]);
                }

            }
        }
        return value;
    }

    public double ShowCdslHoldingValueTotalNew(string BOID)
    {
        // DataTable dt = showCdslHolding(dpid + BOID);
        DataTable dt = bindGridNew(BOID);
        double value = 0;
        int count = dt.Rows.Count;
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ISINValue"] != DBNull.Value)
                {
                    value = value + Convert.ToDouble(dr["ISINValue"]);
                }

            }
        }
        return value;
    }

    DataTable bindGridNew(string BOID)
    {
        //DataTable dt = FetchBoid(BOID);
        DataTable dt = FetchBoidNew(BOID);
        //pds.DataSource = dt.DefaultView;

        //if (pds.DataSourceCount < 1)
        //{
        //    return bindRepeter(BOID);
        //}
        //else
        //{
        //    return bindRepeter(BOID);

        //}

        //return null;
        return bindRepeternew(BOID);

    }

    DataTable FetchBoidNew(string BOID)
    {
        string orderBy = "";
        DataTable dtable = new DataTable();
        string strgetdatetime = populateCmbTime();
        HoldingDate = strgetdatetime;
        string[] dat = strgetdatetime.Split(' ');
        string where = " CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";
        where = where + "and CdslHolding_HoldingDateTime between '" + dat[0] + " 00:00:00 AM'  and '" + strgetdatetime + "'";
        where = where + " and substring(CdslClients_BOID,9,8)='" + BOID + "'";
        dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName", where, null);
        return dtable;
    }

    DataTable bindRepeternew(string BOID)
    {
        try
        {
            //string select, where, id, sql;
            //id = BOID;
            string strdatetime = populateCmbTime();
            string[] date = strdatetime.Split(' ');
            DataTable dt1 = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationSSettings.AppSettings["DBConnectionDefault"]))  MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("cdslHoldingReport1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("startTime", date[0] + " 00:00:00 AM");
                cmd.Parameters.AddWithValue("endTime", strdatetime);
                cmd.Parameters.AddWithValue("isin", "NA");
                cmd.Parameters.AddWithValue("settlementId", "NA");
                if (BOID != "")
                {
                    cmd.Parameters.AddWithValue("boid", "'" + BOID + "'");
                }
                else
                {
                    cmd.Parameters.AddWithValue("boid", "NoId");
                }
                //if (BOID.Length == 16)
                //{
                //    cmd.Parameters.AddWithValue("boid", "'" + BOID.Substring(8) + "'");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("boid", "NoId");
                //}
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt1);
            }
            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                if (dt1.Rows[k]["CdslHolding_CurrentBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_CurrentBalance"].ToString()));

                if (dt1.Rows[k]["CdslHolding_FreeBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_FreeBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PledgeBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PledgeBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_EarmarkedBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_EarmarkedBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PendingRematBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingRematBalance"].ToString()));


                if (dt1.Rows[k]["CdslHolding_PendingDematBalance"] != DBNull.Value)
                    dt1.Rows[k]["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingDematBalance"].ToString()));

                if (dt1.Rows[k]["Rate"] != DBNull.Value)
                    dt1.Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["Rate"].ToString()));

                if (dt1.Rows[k]["ISINVAlue"] != DBNull.Value)
                    dt1.Rows[k]["ISINVAlue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString()));
                //VALUE_Sum += Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString());
                //ViewState["ISINVAlue"] = VALUE_Sum;

            }
            return dt1;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public int TotalNoOfIsin(string BenAccNo)
    {
        return Convert.ToInt32(oDBEngine.GetDataTable(" Trans_CdslHolding ", "count (CdslHolding_ISIN) as TotalISIN", "CdslHolding_HoldingDateTime=(Select max(CdslHolding_HoldingDateTime) from Trans_CdslHolding) and CdslHolding_BenAccountNumber='" + BenAccNo + "'").Rows[0][0]);
    }
}
