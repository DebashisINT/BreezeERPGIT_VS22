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
using System.Net.Mail;
using BusinessLogicLayer;
/// <summary>
/// Summary description for getautomaticsms
/// </summary>
public class getautomaticsms
{
    //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);  MULTI
    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
    Converter objConverter = new Converter();
    DBEngine oDBEngine = new DBEngine(string.Empty);
    public string contactidnew1 = "";
    public string[] contactidnew ;
	public getautomaticsms()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    public void insert_notification_log(string NotificationLog_TopicCode, string NotificationLog_NotificationMethod, string NotificationLog_RequestID, string NotificationLog_ContactID, string NotificationLog_RecipientEmailPhone, string NotificationLog_DeliveryStatus, string notificationid, string rejectreason)
    {
        SqlCommand com = new SqlCommand("sp_insert_notification_log", con);
        com.CommandType = CommandType.StoredProcedure;
        //SqlParameter param = com.Parameters.AddWithValue("@NotificationLog_TopicCode", NotificationLog_TopicCode);
        com.Parameters.Add("@NotificationLog_TopicCode", SqlDbType.VarChar,50).Value = NotificationLog_TopicCode;
        com.Parameters.Add("@NotificationLog_NotificationMethod", SqlDbType.VarChar, 50).Value = NotificationLog_NotificationMethod;
        com.Parameters.Add("@NotificationLog_RequestID", SqlDbType.VarChar, 50).Value = NotificationLog_RequestID;
        com.Parameters.Add("@NotificationLog_ContactID", SqlDbType.VarChar, 50).Value = NotificationLog_ContactID;
        com.Parameters.Add("@NotificationLog_RecipientEmailPhone", SqlDbType.VarChar, 50).Value = NotificationLog_RecipientEmailPhone;
        com.Parameters.Add("@NotificationLog_DeliveryStatus", SqlDbType.VarChar, 50).Value = NotificationLog_DeliveryStatus;
        com.Parameters.Add("@notificationid", SqlDbType.VarChar, 50).Value = notificationid;
        com.Parameters.Add("@rejectreason", SqlDbType.VarChar,1).Value = rejectreason;
        //com.Parameters.AddWithValue("@NotificationLog_NotificationMethod", NotificationLog_NotificationMethod);
        //com.Parameters.AddWithValue("@NotificationLog_RequestID", NotificationLog_RequestID);
        //com.Parameters.AddWithValue("@NotificationLog_ContactID", NotificationLog_ContactID);
        //com.Parameters.AddWithValue("@NotificationLog_RecipientEmailPhone", NotificationLog_RecipientEmailPhone);
        //com.Parameters.AddWithValue("@NotificationLog_DeliveryStatus", NotificationLog_DeliveryStatus);
        //com.Parameters.AddWithValue("@notificationid", notificationid);
        //com.Parameters.AddWithValue("@rejectreason", reason);
        com.ExecuteNonQuery();

    }
    public DataSet fetch_status_sms(string emailid, string[] subject)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        string modifiedsubject="";
        for (int i = 0; i < subject.Length; i++)
        {
            modifiedsubject = modifiedsubject + subject[i] + '~';

        }
        SqlCommand com = new SqlCommand("sp_fetch_topicid_contactid", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlParameter param = com.Parameters.AddWithValue("@phoneormail", emailid);
        com.Parameters.AddWithValue("@subject", modifiedsubject);
        com.Parameters.AddWithValue("@delivarymode", "phone");
        SqlDataAdapter ad = new SqlDataAdapter(com);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        // Mantis Issue 24802
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        // End of Mantis Issue 24802
        return ds;
    }

    public String tradessms(string contactid, string[] subject)
    {
        
        string[] mulcontactid = contactid.Split('|');
        string str = "", ss = "",ss1="", compid = "", segment="";
        try
        {
            if (subject[3].Contains("-"))
            {
                string[] s = subject[3].Split('-');
                ss = s[0] + "-" + s[1] + "-" + s[2];
                ss1=s[0] + "-" + s[1] + "-" + s[2];
            }
            else if (subject[3].Contains("/"))
            {
                string[] s = subject[3].Split('/');
                ss = s[0] + "-" + s[1] + "-" + s[2];
                ss1=s[0] + "-" + s[1] + "-" + s[2];
            }

           
        }
        catch
        {
            ss = System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Year;
             ss1=System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year;
        }

        segment = subject[2].ToUpper();
        if (segment == "ICEX-COMM")
        {
            str = " " + segment + "" + ss1;
            segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][0].ToString();
            compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][1].ToString();
        }
        else if (segment == "MCX-COMM")
        {
            str = " " + segment + "" + ss1;
            segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][0].ToString();
            compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][1].ToString();
        }
        else if (segment == "NSE-FO")
        {
            str = " " + segment + "" + ss1;
            segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][0].ToString();
            compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][1].ToString();
           
        }
        else if (segment == "NSE-CM")
        {
            str = " " + segment + " " + ss1;
            segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][0].ToString();
            compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][1].ToString();
            
        }
      
        for (int i = 0; i < mulcontactid.Length; i++)
        {
                
                    DataTable HoldingDT = new DataTable();                
                    SqlCommand cmd = new SqlCommand("[NSE-CM_NETPOSITIONnew]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyid", compid);
                    cmd.Parameters.AddWithValue("@segment", segment);
                    cmd.Parameters.AddWithValue("@fromdate", ss);
                    cmd.Parameters.AddWithValue("@clients_internalId", mulcontactid[i].ToString());
                    cmd.Parameters.AddWithValue("@Type", "ALL");
                    cmd.Parameters.AddWithValue("@mastersegment", "");  
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HoldingDT);
                    string[,] cnt_ucc=oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc", "cnt_internalid='"+ mulcontactid[i].ToString()+"'", 1);
                    str = str + "; " + cnt_ucc[0,0] + "-";
                    for (int j = 0; j < HoldingDT.Rows.Count; j++)
                    {
                        if (HoldingDT.Rows[j]["Instrument"] == DBNull.Value)
                        {
                            HoldingDT.Rows[j]["Instrument"] = "";
                        }
                        if (HoldingDT.Rows[j]["BuyQty"] == DBNull.Value)
                        {
                            HoldingDT.Rows[j]["BuyQty"] = 0.00M;
                        }
                        if (HoldingDT.Rows[j]["BUYAvgMkt"] == DBNull.Value)
                        {
                            HoldingDT.Rows[j]["BUYAvgMkt"] = 0.00M;
                        }
                        if (HoldingDT.Rows[j]["SellQty"] == DBNull.Value)
                        {
                            HoldingDT.Rows[j]["SellQty"] = 0.00M;
                        }
                        if (HoldingDT.Rows[j]["SellAvgMkt"] == DBNull.Value)
                        {
                            HoldingDT.Rows[j]["SellAvgMkt"] = 0.00M;
                        }
                         if(Convert.ToDecimal(HoldingDT.Rows[j]["SellQty"])==0.00M && Convert.ToDecimal(HoldingDT.Rows[j]["SellAvgMkt"])==0.00M)
                            {
                                str = str + " " + HoldingDT.Rows[j]["Instrument"].ToString() + " B "
                              + Convert.ToDecimal(HoldingDT.Rows[j]["BuyQty"])
                              + "@" + Convert.ToDecimal(HoldingDT.Rows[j]["BUYAvgMkt"]);                     
                            }
                            else if (Convert.ToDecimal(HoldingDT.Rows[j]["BuyQty"]) == 0.00M && Convert.ToDecimal(HoldingDT.Rows[j]["BUYAvgMkt"]) == 0.00M)
                         {
                             str = str + " " + HoldingDT.Rows[j]["Instrument"].ToString() + " S "
                          + Convert.ToDecimal(HoldingDT.Rows[j]["SellQty"])
                          + "@" + Convert.ToDecimal(HoldingDT.Rows[j]["SellAvgMkt"]);                     
                         }
                         else
                        str = str + " "+HoldingDT.Rows[j]["Instrument"].ToString() + " B "
                            + Convert.ToDecimal(HoldingDT.Rows[j]["BuyQty"])
                            + "@" + Convert.ToDecimal(HoldingDT.Rows[j]["BUYAvgMkt"])  +
                            "  S " + Convert.ToDecimal(HoldingDT.Rows[j]["SellQty"]) + "@"
                            + Convert.ToDecimal(HoldingDT.Rows[j]["SellAvgMkt"]);
                    }
                    str = str.Trim();
                }
                str = str + ";";
            
            return str;
    }

    public String cdslHoldingFrmsms(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        //String[]  morecontactid=new string[10];
        //morecontactid[0] = "";
        DataTable HoldingDT = new DataTable();
        DataTable NSDLHoldingDT = new DataTable();
        String str,str1="";

        str = String.Empty;

        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] == "CDSL" || subject[2] == "cdsl")
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) != "I")
                        {
                             
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8)+ "|";
                        }
                        
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        SqlCommand cmd = new SqlCommand("Email_Cdsl_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@boid", contactidnew[j].ToString());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(HoldingDT);
                        if (HoldingDT.Rows.Count > 0)
                        {
                            str = str + "; " + contactidnew[j].ToString() + "-";
                            for (int i = 0; i < HoldingDT.Rows.Count; i++)
                            {

                                str = str + HoldingDT.Rows[i]["CDSLISIN_ShortName"].ToString() + "[" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(HoldingDT.Rows[i]["CdslHolding_FreeBalance"])).ToString().Trim() + "]";
                            }
                            str = str.Trim();
                            
                        }
                        HoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;


            }
            else
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) == "I")
                        {
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        SqlCommand cmd = new SqlCommand("Email_Nsdl_report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BenAccId", contactidnew[j]);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(NSDLHoldingDT);
                        if (NSDLHoldingDT.Rows.Count > 0)
                        {
                            str = str + "; " + contactidnew[j].ToString() + "-";
                            for (int i = 0; i < NSDLHoldingDT.Rows.Count; i++)
                            {
                                str = str + NSDLHoldingDT.Rows[i]["NSDLISIN_CompanyName"].ToString() + "[" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(NSDLHoldingDT.Rows[i]["Free"])) + "]";
                            }
                            str = str.Trim();
                        }
                        NSDLHoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;
            }
    }

    public String dpbillsms(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        //String[]  morecontactid=new string[10];
        //morecontactid[0] = "";
        DataSet HoldingDT = new DataSet();
        DataSet NSDLHoldingDT = new DataSet();
        String str, str1 = "";
        DateTime fromdate = System.DateTime.Now;
        //string fromdatelast = fromdate.Month + "/" + fromdate.Day + "/" + fromdate.Year;
        str = String.Empty;

        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] == "CDSL" || subject[2] == "cdsl")
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) != "I")
                        {

                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }

                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        SqlCommand cmd = new SqlCommand("dp_bill_cdsl_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@boid", contactidnew[j].ToString());
                        cmd.Parameters.AddWithValue("@seg", "CDSL");
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(HoldingDT);
                        str = str + "; " + contactidnew[j].ToString() + "-";
                        for (int i = 0; i < HoldingDT.Tables[0].Rows.Count; i++)
                        {
                            string strcrordr=oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00042'", contactidnew[j], fromdate, HoldingDT.Tables[1].Rows[i]["exch_internalid"].ToString(), HoldingDT.Tables[1].Rows[i]["exch_compid"].ToString(), fromdate).Rows[0][0].ToString();
                            if (strcrordr.Contains("-"))
                            {
                                strcrordr = strcrordr.Remove(0, 1) + "(Dr)";

                            }
                            else
                            {
                                strcrordr = strcrordr + "(Cr)";
                            }
                            str = str + "Bill for"+HoldingDT.Tables[0].Rows[i]["DPBILLSUMMARY_billnumber"].ToString() + HoldingDT.Tables[0].Rows[i]["a"].ToString() + "Ledger Balance: Rs" + strcrordr;
                        }
                        str = str.Trim();
                        HoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;


            }
            else
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) == "I")
                        {
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        SqlCommand cmd = new SqlCommand("dp_bill_cdsl_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BenAccId", contactidnew[j]);
                        cmd.Parameters.AddWithValue("@seg", "NSDL");
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(NSDLHoldingDT);
                        str = str + "; " + contactidnew[j].ToString() + "-";
                        for (int i = 0; i < NSDLHoldingDT.Tables[0].Rows.Count; i++)
                        {
                            string strcrordr = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00042'", contactidnew[j], fromdate, NSDLHoldingDT.Tables[1].Rows[i]["exch_internalid"].ToString(), NSDLHoldingDT.Tables[1].Rows[i]["exch_compid"].ToString(), fromdate).Rows[0][0].ToString();
                            if (strcrordr.Contains("-"))
                            {
                                strcrordr = strcrordr.Remove(0, 1) + "(Dr)";

                            }
                            else
                            {
                                strcrordr = strcrordr + "(Cr)";
                            }
                            str = str + "Bill for" + NSDLHoldingDT.Tables[0].Rows[i]["DPBILLSUMMARY_billnumber"].ToString() + NSDLHoldingDT.Tables[0].Rows[i]["a"].ToString() + "Ledger Balance: Rs" + strcrordr;
                        }
                        str = str.Trim();
                        NSDLHoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;
            }
    }

    public String dp5trnsms(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        //String[]  morecontactid=new string[10];
        //morecontactid[0] = "";
        DataTable HoldingDT = new DataTable();
        DataTable NSDLHoldingDT = new DataTable();
        String str, str1 = "";
        DateTime fromdate = System.DateTime.Now;
        //string fromdatelast = fromdate.Month + "/" + fromdate.Day + "/" + fromdate.Year;
        str = String.Empty;

        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] == "CDSL" || subject[2] == "cdsl")
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) != "I")
                        {

                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }

                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        try
                        {
                          //HoldingDT=  oDBEngine.GetDataTable("trans_cdsltransaction,MASTER_CDSLISIN", "top 5 substring(CDSLISIN_SHORTNAME,0,15) as CDSLISIN_SHORTNAME,convert(varchar(15),CDSLTRANSACTION_DATE,3) as CDSLTRANSACTION_DATE,CDSLTRANSACTION_DRCR,CDSLTRANSACTION_QUANTITY", "cdsltransaction_benaccountnumber='" + contactidnew[j] + "' and cdsltransaction_ermarked is NULL AND CDSLTRANSACTION_ISIN=CDSLISIN_NUMBER and CDSLISIN_SHORTNAME like '" + subject[3].ToString().Trim().ToUpper() + "%' order by cdsltransaction_date desc ").Copy();
                            SqlCommand cmd = new SqlCommand("select * from (select top 5  substring(CDSLISIN_SHORTNAME,0,15) as CDSLISIN_SHORTNAME,convert(varchar(15),CDSLTRANSACTION_DATE,1) as CDSLTRANSACTION_DATE, CDSLTRANSACTION_DRCR,CDSLTRANSACTION_QUANTITY from trans_cdsltransaction,MASTER_CDSLISIN Where cdsltransaction_benaccountnumber='" + contactidnew[j] + "' and cdsltransaction_ermarked is NULL AND CDSLTRANSACTION_ISIN=CDSLISIN_NUMBER and CDSLISIN_SHORTNAME like '" + subject[3].ToString().Trim().ToUpper() + "%' order by cdsltransaction_date desc ) a order by CDSLISIN_SHORTNAME ,cdsltransaction_date desc", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 0;
                            SqlDataAdapter da = new SqlDataAdapter();
                            da.SelectCommand = cmd;
                            da.Fill(HoldingDT); 
                        }
                        catch
                        {
                          
                            SqlCommand cmd = new SqlCommand("select * from (select top 5  substring(CDSLISIN_SHORTNAME,0,15) as CDSLISIN_SHORTNAME,convert(varchar(15),CDSLTRANSACTION_DATE,1) as CDSLTRANSACTION_DATE, CDSLTRANSACTION_DRCR,CDSLTRANSACTION_QUANTITY from trans_cdsltransaction,MASTER_CDSLISIN Where cdsltransaction_benaccountnumber='" + contactidnew[j] + "' and cdsltransaction_ermarked is NULL AND CDSLTRANSACTION_ISIN=CDSLISIN_NUMBER order by cdsltransaction_date desc ) a order by CDSLISIN_SHORTNAME ,cdsltransaction_date desc", con);
                            cmd.CommandType = CommandType.Text;                            
                            cmd.CommandTimeout = 0;
                            SqlDataAdapter da = new SqlDataAdapter();
                            da.SelectCommand = cmd;
                            da.Fill(HoldingDT); 
                        }
                        str = str + "; " + contactidnew[j].ToString() + ":";
                        if (HoldingDT.Rows.Count > 0)
                        {
                            for (int i = 0; i < HoldingDT.Rows.Count; i++)
                            {
                                if (HoldingDT.Rows[i]["CDSLTRANSACTION_QUANTITY"].ToString().Split('.')[1] == "000")
                                {
                                    HoldingDT.Rows[i]["CDSLTRANSACTION_QUANTITY"] = HoldingDT.Rows[i]["CDSLTRANSACTION_QUANTITY"].ToString().Split('.')[0];
                                }
                                if (i != 0)
                                {
                                    if (HoldingDT.Rows[i]["CDSLISIN_SHORTNAME"].ToString() == HoldingDT.Rows[i - 1]["CDSLISIN_SHORTNAME"].ToString())
                                    {
                                        HoldingDT.Rows[i]["CDSLISIN_SHORTNAME"] = "";
                                    }
                                    if (HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[1] + "/" + HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[0] + "/" + HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[2] == HoldingDT.Rows[i - 1]["CDSLTRANSACTION_DATE"].ToString())
                                    {
                                        HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"] = "";
                                    }
                                }
                                if (HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"] != "")
                                {
                                    HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"] = HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[1] + "/" + HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[0] + "/" + HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Split('/')[2];
                                }

                                str = str + HoldingDT.Rows[i]["CDSLISIN_SHORTNAME"].ToString().Trim() + "[" + HoldingDT.Rows[i]["CDSLTRANSACTION_DATE"].ToString().Trim() + " " + HoldingDT.Rows[i]["CDSLTRANSACTION_DRCR"] + " " + HoldingDT.Rows[i]["CDSLTRANSACTION_QUANTITY"].ToString().Trim() + "]";

                            }
                        }
                        str = str.Trim();
                        HoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;


            }
            else
            {
                {
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) == "I")
                        {
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                      
                        str = str.Trim();
                        NSDLHoldingDT.Reset();
                    }
                }
                str = str.Remove(0, 1);
                return str;
            }
    }
  

}
