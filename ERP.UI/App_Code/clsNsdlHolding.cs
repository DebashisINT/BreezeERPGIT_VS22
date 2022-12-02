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
using System.Data.SqlClient;
using BusinessLogicLayer;
/// <summary>
/// Summary description for clsNsdlHolding
/// </summary>
public class clsNsdlHolding
{
    //public DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
    public DBEngine oDBEngine = new DBEngine();
    private static DataTable dates;
    public int chkStatus;
    PagedDataSource pds = new PagedDataSource();
    string selectedDay = String.Empty;
    string selectedDate = String.Empty;
    static DataTable dt = new DataTable();
    static DataTable dt1 = new DataTable();
    DataView dv = new DataView();
    DataView dv1 = new DataView();
    static string BenType;
    Converter oconverter = new Converter();
    string data;
    static string Clients;
    ExcelFile objExcel = new ExcelFile();
    static decimal VALUE_Sum;
    Converter objConverter = new Converter();
    Converter OConvert = new Converter();
    static string Group;
    static string Branch;
    static string User;
    string[] InputName = new string[7];
    string[] InputType = new string[7];
    string[] InputValue = new string[7];
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

	public clsNsdlHolding()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataView ShowHolding(string BenAccNo)
    {
        dt.Rows.Clear();
        dt1.Rows.Clear();
        string strDate=populateCmbDate();
        HoldingDate = strDate;
        if (dt.Rows.Count > 0)
        {
            DataRow[] drDv = dt.Select(" UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]));
            foreach (DataRow d in drDv)
                dt.Rows.Remove(d);

        }

        if (dt1.Rows.Count > 0)
        {
            DataRow[] drDv1 = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
            foreach (DataRow d in drDv1)
                dt1.Rows.Remove(d);
        }
        bindGrid(BenAccNo,strDate);
        if (dt1.Rows.Count > 0)
        {
            dv1 = dt1.DefaultView;
            dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            return dv1;
        }
        return null;

    }
    //public string ShowHolding()
    //{
    //    populateCmbDate();
    //    if (dt.Rows.Count > 0)
    //    {
    //        DataRow[] drDv = dt.Select(" UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]));
    //        foreach (DataRow d in drDv)
    //            dt.Rows.Remove(d);

    //    }

    //    if (dt1.Rows.Count > 0)
    //    {
    //        DataRow[] drDv1 = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
    //        foreach (DataRow d in drDv1)
    //            dt1.Rows.Remove(d);
    //    }
    //    bindGrid();
    //    return dt1.Rows[0][""].ToString();;

    //}

    void bindGrid(string BenAccNo, string strDate)
    {
        
        if (dt.Rows.Count > 0)
        {
            dv = dt.DefaultView;
            dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
        }

        if (dv.Count == 0)
        {
            string where = " NsdlClients_BenAccountID = NsdlHolding_BenAccountNumber and LTRIM(RTRIM(NsdlClients_DPID)) = LTRIM(RTRIM(NsdlHolding_DPID)) ";

            // where = where + " and NsdlHolding_HoldingDateTime='" + txtDate.Value + "' and NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString()+ ")";

            where = where + " and NsdlHolding_HoldingDateTime='" + Convert.ToDateTime(strDate) + "'";
            where = where + " and NsdlClients_BenAccountID  in (" + BenAccNo + ")";

            string orderBy = " NsdlClients_BenAccountID ";
            where = where + " and NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
            dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);

        }


        dv = dt.DefaultView;
        dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
        pds.DataSource = dv;
        pds.AllowPaging = true;

        if (pds.DataSourceCount < 1)
        {

        }
        else
        {
            BenType = dt.DefaultView.Table.Rows[pds.CurrentPageIndex]["NsdlClients_BenType"].ToString();
        }

        bindRepeater(BenAccNo, strDate);


       
    }
    protected void bindRepeater(string BenAccNo, string strDate)
    {

        try
        {
            //if (dt.Rows.Count > 0)
            //{
            //    dv = dt.DefaultView;
            //    dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            //}
            //if (dt1.Rows.Count > 0)
            //{
            //    dv1 = dt1.DefaultView;
            //    dv1.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            //}
            if (dt.Rows.Count > 0)
            {
                dv = dt.DefaultView;
                dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            }
            if (dt1.Rows.Count > 0)
            {
                dv1 = dt1.DefaultView;
                dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            }
            if (dv.Count > 0 && ((dv1 == null) || (dv1.Count == 0)))
            {
                string select, where, where_NsdlHolding_HoldingDateTime, id, orderBy;

                string strISIN, strSettlementNumber;
                id = BenAccNo;

                where_NsdlHolding_HoldingDateTime = Convert.ToDateTime(strDate).ToString();
                strISIN = "na";
                strSettlementNumber = "na";
                ClearArray();

                InputName[0] = "BenAccId";
                InputName[1] = "where_NsdlHolding_HoldingDateTime";
                InputName[2] = "BenType";
                InputName[3] = "isin";
                InputName[4] = "Settlement_Id";
                InputName[5] = "branhid";
                InputName[6] = "user";

                InputType[0] = "V";
                InputType[1] = "V";
                InputType[2] = "V";
                InputType[3] = "V";
                InputType[4] = "V";
                InputType[5] = "V";
                InputType[6] = "V";

                InputValue[0] = id;
                //InputValue[1] = where_NsdlHolding_HoldingDateTime.Substring(0, 10);
                InputValue[1] = where_NsdlHolding_HoldingDateTime.Substring(0, where_NsdlHolding_HoldingDateTime.Length - 11);
                InputValue[2] = BenType;
                InputValue[3] = strISIN;
                InputValue[4] = strSettlementNumber;
                InputValue[5] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                InputValue[6] = Convert.ToString(HttpContext.Current.Session["userid"]);

                dt1 = SQLProcedures.SelectProcedureArr("sp_ShowNsdlHolding", InputName, InputType, InputValue);

            }
            //dv1 = dt1.DefaultView;
            //dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            //gridHolding.DataSource = dv1;
            //gridHolding.DataBind();
            //gridHolding.FilterExpression = string.Empty;


        }
        catch
        {
        }

    }
    public string populateCmbDate()
    {

        dates = oDBEngine.GetDataTable(" Trans_NsdlHolding ", " distinct CAST(DAY(NsdlHolding_HoldingDateTime) AS VARCHAR(2)) as selDay,CAST(DAY(NsdlHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, NsdlHolding_HoldingDateTime) + ' ' + CAST(YEAR(NsdlHolding_HoldingDateTime) AS VARCHAR(4))as date, NsdlHolding_HoldingDateTime ", null, "NsdlHolding_HoldingDateTime desc");

        if (dates.Rows.Count > 0)
        {
            selectedDay = Convert.ToString(dates.Rows[0][0]);
            selectedDate = Convert.ToString(dates.Rows[0][1]);

            if (Convert.ToInt32(selectedDay) < 10)
                return ("0" + selectedDate);
            else
                return selectedDate;

        }
        return "1900-01-01 00:00:00.000"; 

    }
    protected void ClearArray()
    {
        Array.Clear(InputName, 0, InputName.Length - 1);
        Array.Clear(InputType, 0, InputType.Length - 1);
        Array.Clear(InputValue, 0, InputValue.Length - 1);

    }

    public double ShowNsdlHoldingValueTotal(string BenAccNo)
    {
        DataView dv=ShowHolding(BenAccNo);
        double value=0;
        if (dv != null)
            foreach (DataRow dr in dv.Table.Rows)
            {
                if(dr["ISINValue"] != DBNull.Value)
                    value = value + Convert.ToDouble(dr["ISINValue"]);

            }
        return value;
    }
    public int TotalNoOfIsin(string BenAccNo)
    {
      return Convert.ToInt32(oDBEngine.GetDataTable(" Trans_NsdlHolding ", "count (NsdlHolding_ISIN) as TotalISIN","NsdlHolding_HoldingDateTime=(Select max(NsdlHolding_HoldingDateTime) from Trans_NsdlHolding) and NsdlHolding_BenAccountNumber='"+BenAccNo+"'").Rows[0][0]);
    }
}
