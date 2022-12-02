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
/// Summary description for GetAutomaticMail
/// </summary>
public class GetAutomaticMail
{
    
    //SqlConnection con=new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
    cdslBillViewCalculation cbill = new cdslBillViewCalculation();
    DBEngine oDBEngine = new DBEngine(string.Empty);
    public string[] contactidnew;
    public string contactidnew1 = "";
    ledger l = new ledger();
    Converter oconverter = new Converter();
	public GetAutomaticMail()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}
    //public string fetch_status(string emailid, string subject)
    //{
    //    if (con.State == ConnectionState.Open)
    //    {
    //        con.Close();
    //    }
    //    con.Open();
    //   SqlCommand com=new SqlCommand("sp_Valid_Emailid", con);
    //   com.CommandType = CommandType.StoredProcedure;
    //   SqlParameter param = com.Parameters.AddWithValue("@emailid", emailid);
    //   com.Parameters.AddWithValue("@subjectline", subject);
    //   com.Parameters.Add("@out", SqlDbType.VarChar,20);
    //   com.Parameters["@out"].Direction = ParameterDirection.Output;
    //   com.Parameters.Add("@outContactID", SqlDbType.VarChar, 50);
    //   com.Parameters["@outContactID"].Direction = ParameterDirection.Output;
    //   com.ExecuteNonQuery();
    //   return (com.Parameters["@out"].Value.ToString() + "," + com.Parameters["@outContactID"].Value.ToString());            

    //}

    public void insert_notification_log(string NotificationLog_TopicCode, string NotificationLog_NotificationMethod, string NotificationLog_RequestID, string NotificationLog_ContactID, string NotificationLog_RecipientEmailPhone, string NotificationLog_DeliveryStatus,string notificationid,string rejectreason)
    {
        SqlCommand com = new SqlCommand("sp_insert_notification_log", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlParameter param = com.Parameters.AddWithValue("@NotificationLog_TopicCode", NotificationLog_TopicCode);
        com.Parameters.AddWithValue("@NotificationLog_NotificationMethod", NotificationLog_NotificationMethod);
        com.Parameters.AddWithValue("@NotificationLog_RequestID", NotificationLog_RequestID);
        com.Parameters.AddWithValue("@NotificationLog_ContactID", NotificationLog_ContactID);
        com.Parameters.AddWithValue("@NotificationLog_RecipientEmailPhone", NotificationLog_RecipientEmailPhone);
        com.Parameters.AddWithValue("@NotificationLog_DeliveryStatus", NotificationLog_DeliveryStatus);
        com.Parameters.AddWithValue("@notificationid", notificationid);
        com.Parameters.AddWithValue("@rejectreason", rejectreason);
        
         com.ExecuteNonQuery();
        
    }
  

    public DataSet fetch_status_email(string emailid, string[] subject)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        string modifiedsubject = "";
        for (int i = 0; i < subject.Length; i++)
        {
            modifiedsubject = modifiedsubject + subject[i] + '~';

        }
        SqlCommand com = new SqlCommand("sp_fetch_topicid_contactid", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlParameter param = com.Parameters.AddWithValue("@phoneormail", emailid);
        com.Parameters.AddWithValue("@subject", modifiedsubject);
        com.Parameters.AddWithValue("@delivarymode", "email");
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
    public DataSet fetch_status_email(string emailid, string subject)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        
        SqlCommand com = new SqlCommand("sp_fetch_topicid_contactid", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlParameter param = com.Parameters.AddWithValue("@phoneormail", emailid);
        com.Parameters.AddWithValue("@subject", subject);
        com.Parameters.AddWithValue("@delivarymode", "email");
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


    public String cdslHoldingFrmEmail(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        DataTable HoldingDT = new DataTable();
        DataTable NSDLHoldingDT = new DataTable();
        String str, str1 = "";

        str = String.Empty;

        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] == "CDSL" || subject[2] == "cdsl")
            {
               
                    for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        if (mulcontactid[i].ToString().Substring(0, 1) != "I")
                        {
                            
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 =  contactid;
                            str1 = str1 + contactid.Substring(contactid.Length - 8, 8) + "|";
                        }
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        SqlCommand cmd = new SqlCommand("Email_Cdsl_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@boid", contactidnew[j]);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(HoldingDT);
                        #region CDSL
                        if (HoldingDT.Rows.Count > 0)
                        {


                            DataView HoldingDV = new DataView(HoldingDT);
                            DataTable FilteredHolding = new DataTable();
                            DataTable DistinctBenAccount = HoldingDV.ToTable(true, new string[] { "CdslHolding_BenAccountNumber" });

                            foreach (DataRow dr in DistinctBenAccount.Rows)
                            {

                                HoldingDV.RowFilter = "CdslHolding_BenAccountNumber = '" + dr["CdslHolding_BenAccountNumber"] + "'";
                                FilteredHolding = HoldingDV.ToTable();
                                if (FilteredHolding.Rows.Count > 0)
                                {
                                    string subject1 = " ";
                                    for (int a = 0; a < subject.Length; a++)
                                    {
                                        subject1 = subject1 + " " + subject[a];
                                    }
                                    subject1 = subject1.Trim();
                                    str += generateMailHoldingBody(FilteredHolding, subject1);
                                }
                                FilteredHolding.Reset();



                            }

                        }

                        if (str == string.Empty)
                        {
                            str = "No Record Found";
                        }
                        str += "<br/>";


                        HoldingDT.Reset();
                        # endregion CDSL
                    }
                

               
            }
            else
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
                        #region NSDL
                        if (NSDLHoldingDT.Rows.Count > 0)
                        {


                            DataView HoldingDV = new DataView(NSDLHoldingDT);
                            DataTable FilteredHolding = new DataTable();
                            DataTable DistinctBenAccount = HoldingDV.ToTable(true, new string[] { "NsdlHolding_BenAccountNumber" });

                            foreach (DataRow dr in DistinctBenAccount.Rows)
                            {

                                HoldingDV.RowFilter = "NsdlHolding_BenAccountNumber = '" + dr["NsdlHolding_BenAccountNumber"] + "'";
                                FilteredHolding = HoldingDV.ToTable();
                                if (FilteredHolding.Rows.Count > 0)
                                {
                                    string subject1 = " ";
                                    for (int a = 0; a < subject.Length; a++)
                                    {
                                        subject1 = subject1 + " " + subject[a];
                                    }
                                    subject1 = subject1.Trim();
                                    str += generateMailNsdlHoldingBody(FilteredHolding, subject1);
                                }
                                FilteredHolding.Reset();



                            }

                        }

                        if (str == string.Empty)
                        {
                            str = "No Record Found";
                        }
                        str += "<br/>";
                        //str += new Converter().SendMailHtmlBody("abcd@gmail.com", Email, subject, str);

                        NSDLHoldingDT.Reset();
                        # endregion NSDL
                    }


                
               
            }
        return str;
    }

    public String generateMailHoldingBody(DataTable Dt, string subject)
    {
        Converter objConverter = new Converter();
        String strHtml;

        strHtml = String.Empty;

        //strHtml = "<table style=" + Convert.ToChar(34) + "font-family: Tahoma,Arial, Verdana, sans-serif; font-size: 12px; padding-left: 1px; padding-right: 1px; padding-bottom:2px; padding-top:2px;" + Convert.ToChar(34) + " width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " >";

        strHtml = "<table " + objConverter.Email_TableStyle() + " >";
        strHtml += "<tr><td " + objConverter.Email_HeaderColor() + " colspan=" +
                    Convert.ToChar(34) + 10 + Convert.ToChar(34) + ">" +
                    objConverter.Email_HeaderText(Dt.Rows[0]["companyname"].ToString()) +
                    "<br/>" + Dt.Rows[0]["address1"].ToString() +
                    "<br/>" + Dt.Rows[0]["address2"].ToString() +
                    "," + Dt.Rows[0]["Phone"].ToString() +
                    "<br/>ServTaxRegn No:" + Dt.Rows[0]["StaxRegNo"].ToString() +
                    "  Pan:" + Dt.Rows[0]["Pan"].ToString() +
                    "<br/>" + subject.ToUpper() +
                    "</td></tr>";


        strHtml += "<tr><td " + objConverter.Email_SubHeaderColor() + " colspan=" + Convert.ToChar(34) + 10 + Convert.ToChar(34) + "><b>Holding of : " + Dt.Rows[0]["CdslClients_FirstHolderName"].ToString() + " [" + Dt.Rows[0]["CdslHolding_BenAccountNumber"].ToString() + "]</b> at " + Dt.Rows[0]["CdslHolding_HoldingDateTime"].ToString() + "</td></tr>";

        strHtml += "<tr><td colspan=" +
                Convert.ToChar(34) + 10 + Convert.ToChar(34) + ">" +
                "&nbsp;</td></tr>";



        strHtml += "<tr " + objConverter.Email_ContentHeaderColor() + "><td><b>ISIN</b></td><td>" +
                            "<b>Security Name</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Current</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Free</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Pledge</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Earmarked</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Remat</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Demat</b>" + 
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Rate</b>" +
                                "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>ISINValue</b>" +"</td></tr>";

        foreach (DataRow dr in Dt.Rows)
        {
            strHtml += "<tr ><td>" + dr["CdslHolding_ISIN"].ToString() + "</td>";
            strHtml += "<td>" + dr["CDSLISIN_ShortName"].ToString() + "</td>";

            if (dr["CdslHolding_CurrentBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_CurrentBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CdslHolding_FreeBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_FreeBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CdslHolding_PledgeBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_PledgeBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CdslHolding_EarmarkedBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_EarmarkedBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CdslHolding_PendingRematBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_PendingRematBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CdslHolding_PendingDematBalance"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CdslHolding_PendingDematBalance"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Rate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Rate"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ISINVAlue"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ISINVAlue"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            strHtml += "</tr>";



        }

        strHtml += "</table><br/><br/>";
        strHtml += "This is an automatically generated mail, please do not reply to this";
        return strHtml;

    }

    public String generateMailNsdlHoldingBody(DataTable Dt, string subject)
    {
        Converter objConverter = new Converter();
        String strHtml;

        strHtml = String.Empty;

        //strHtml = "<table style=" + Convert.ToChar(34) + "font-family: Tahoma,Arial, Verdana, sans-serif; font-size: 12px; padding-left: 1px; padding-right: 1px; padding-bottom:2px; padding-top:2px;" + Convert.ToChar(34) + " width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " >";

        strHtml = "<table " + objConverter.Email_TableStyle() + " >";
        strHtml += "<tr><td " + objConverter.Email_HeaderColor() + " colspan=" +
                    Convert.ToChar(34) + 9 + Convert.ToChar(34) + ">" +
                    objConverter.Email_HeaderText(Dt.Rows[0]["companyname"].ToString()) +
                    "<br/>" + Dt.Rows[0]["address1"].ToString() +
                    "<br/>" + Dt.Rows[0]["address2"].ToString() +
                    "," + Dt.Rows[0]["Phone"].ToString() +
                    "<br/>ServTaxRegn No:" + Dt.Rows[0]["StaxRegNo"].ToString() +
                    "  Pan:" + Dt.Rows[0]["Pan"].ToString() +
                    "<br/>" + subject.ToUpper() +
                    "</td></tr>";


        strHtml += "<tr><td " + objConverter.Email_SubHeaderColor() + " colspan=" + Convert.ToChar(34) + 9 + Convert.ToChar(34) + "><b>Holding of : " + Dt.Rows[0]["NsdlClients_BenFirstHolderName"].ToString() + " [" + Dt.Rows[0]["NsdlHolding_BenAccountNumber"].ToString() + "]</b> at " + Dt.Rows[0]["NsdlHolding_HoldingDateTime"].ToString() + "</td></tr>";

        strHtml += "<tr><td colspan=" +
                Convert.ToChar(34) + 9 + Convert.ToChar(34) + ">" +
                "&nbsp;</td></tr>";



        strHtml += "<tr " + objConverter.Email_ContentHeaderColor() + "><td><b>ISIN</b></td><td>" +
                            "<b>Security Name</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Current</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Free</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Pledge</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Remat</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Demat</b>" + 
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Rate</b>" + 
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                                               "<b>ISINValue</b>" + "</td></tr>";


        foreach (DataRow dr in Dt.Rows)
        {
            strHtml += "<tr ><td>" + dr["NsdlHolding_ISIN"].ToString() + "</td>";
            strHtml += "<td>" + dr["NSDLISIN_CompanyName"].ToString() + "</td>";

            if (dr["Total"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Total"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["Free"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Free"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if ((Convert.ToDecimal(dr["Pledged"]) !=0.000M))
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Pledged"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

          
            if (dr["Remat"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Remat"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["Demat"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Demat"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Rate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Rate"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ISINValue"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ISINValue"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            strHtml += "</tr>";



        }

        //strHtml += "</table><br/>";
        strHtml += "</table>";
        return strHtml;

    }

    public String generateMailHoldingBodyNSE(DataTable Dt, DataTable Dt1,DataTable Dt2,string subject)
    {

        Converter objConverter = new Converter();
        String strHtml;
        int jj = 0;
        strHtml = String.Empty;

        //strHtml = "<table style=" + Convert.ToChar(34) + "font-family: Tahoma,Arial, Verdana, sans-serif; font-size: 12px; padding-left: 1px; padding-right: 1px; padding-bottom:2px; padding-top:2px;" + Convert.ToChar(34) + " width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " >";

        strHtml = "<table " + objConverter.Email_TableStyle() + " >";
        strHtml += "<tr><td " + objConverter.Email_HeaderColor() + " colspan=" +
                    Convert.ToChar(34) + 20 + Convert.ToChar(34) + ">" +
                    objConverter.Email_HeaderText(Dt.Rows[0]["companyname"].ToString()) +
                    "<br/>" + Dt.Rows[0]["address1"].ToString() +
                    "<br/>" + Dt.Rows[0]["address2"].ToString() +
                    "," + Dt.Rows[0]["Phone"].ToString() +
                    "<br/>ServTaxRegn No:" + Dt.Rows[0]["StaxRegNo"].ToString() +
                    "  Pan:" + Dt.Rows[0]["Pan"].ToString() +
                    "<br/>" + subject +
                    "</td></tr>";


        strHtml += "<tr><td " + objConverter.Email_SubHeaderColor() + " colspan=" + Convert.ToChar(34) + 20 + Convert.ToChar(34) + "><b>Trade Register of : " + Dt.Rows[0]["cnt_firstname"].ToString() + " [" + Dt.Rows[0]["cnt_ucc"].ToString() + "]</b> at " + Dt.Rows[0]["CustomerTrades_TradeDate"].ToString() + "</td></tr>";

        strHtml += "<tr><td colspan=" +
                Convert.ToChar(34) + 20 + Convert.ToChar(34) + ">" +
                "&nbsp;</td></tr>";



        strHtml += "<tr " + objConverter.Email_ContentHeaderColor() + "><td><b>TradeDate</b></td><td>" +
                            "<b>Segment</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>SettNo</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>OrderNo</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>OrderTime</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>TradeNo</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>TradeTime</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Instrument</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Bought</b>" +
                                "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Sold</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>UnitPrice</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>BrkgType</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Brkg</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>TotalBrkg</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>NetRate</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>NetValue</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>SrvTax</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>NetAmount</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Quantity</b>" + "</td></tr>";

        DataTable dt3 = new DataTable();
        dt3 = gettotalquantity(Dt).Copy();
        
        foreach (DataRow dr in Dt.Rows)
        {
            //strHtml += "<tr ><td>" + dr["CdslHolding_ISIN"].ToString() + "</td>";
            //strHtml += "<td>" + dr["CDSLISIN_ShortName"].ToString() + "</td>";
            strHtml += "<tr >";
            if (dr["CustomerTrades_TradeDate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["CustomerTrades_TradeDate"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["Segmentname"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["Segmentname"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CustomerTrades_SettlementNumber"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["CustomerTrades_SettlementNumber"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["OrderNo"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" +dr["OrderNo"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["OrderEntryTime"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" +dr["OrderEntryTime"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["TradeNo"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["TradeNo"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["TradeEntryTime"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["TradeEntryTime"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Symbol"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["Symbol"].ToString()+"  "+ dr["Series"].ToString()+ "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CustomerTrades_UnitPriceQuantity_Bought"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_UnitPriceQuantity_Bought"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_UnitPriceQuantity_sold"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_UnitPriceQuantity_sold"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_UnitPrice"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_UnitPrice"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_BrokerageType"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["CustomerTrades_BrokerageType"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_UnitBrokerage"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_UnitBrokerage"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["CustomerTrades_TotalBrokerage"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_TotalBrokerage"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_NetRatePerUnit"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_NetRatePerUnit"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["CustomerTrades_NetValue"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["CustomerTrades_NetValue"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
           
            if (dr["servicetax"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["servicetax"].ToString() + " " + dr["servicetaxmode"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["netamount"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["netamount"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            
            
            if ( dt3.Rows[jj][0]!= "no value")
            {
                decimal aa = Convert.ToDecimal(dt3.Rows[jj][0]);
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(aa) + "</td>";

            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            jj++;
            strHtml += "</tr>";



        }
        //int bought = Convert.ToInt32(Dt.Rows[0]["CustomerTrades_UnitPriceQuantity_Bought"]);
        //int sold = Convert.ToInt32(Dt.Rows[0]["CustomerTrades_UnitPriceQuantity_sold"]);
        //int net = bought - sold;
        //strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" +net + "</td>";
       // strHtml += "<tr ><td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + "Client Total" +"<br/>"+"Grand Total"+"</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_TotalBrokerage"])) + "</td>";//"<br>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customerbrkgamount"])) + "</td>";
        strHtml += "<td>&nbsp;</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_NetValue"])) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customernetamount"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_SrvTax"].ToString())) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customersrvtax"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_NetAmount"].ToString())) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customernetamountgrand"]) )+ "</td>";

        strHtml += "</tr></table><br/>";

        return strHtml;

    }

    public String generateMailHoldingBodyICEX(DataTable Dt, DataTable Dt1, DataTable Dt2, string subject)
    {

        Converter objConverter = new Converter();
        String strHtml;
        int jj = 0;
        strHtml = String.Empty;

        //strHtml = "<table style=" + Convert.ToChar(34) + "font-family: Tahoma,Arial, Verdana, sans-serif; font-size: 12px; padding-left: 1px; padding-right: 1px; padding-bottom:2px; padding-top:2px;" + Convert.ToChar(34) + " width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " >";

        strHtml = "<table " + objConverter.Email_TableStyle() + " >";
        strHtml += "<tr><td " + objConverter.Email_HeaderColor() + " colspan=" +
                    Convert.ToChar(34) + 20 + Convert.ToChar(34) + ">" +
                    objConverter.Email_HeaderText(Dt.Rows[0]["companyname"].ToString()) +
                    "<br/>" + Dt.Rows[0]["address1"].ToString() +
                    "<br/>" + Dt.Rows[0]["address2"].ToString() +
                    "," + Dt.Rows[0]["Phone"].ToString() +
                    "<br/>ServTaxRegn No:" + Dt.Rows[0]["StaxRegNo"].ToString() +
                    "  Pan:" + Dt.Rows[0]["Pan"].ToString() +
                    "<br/>" + subject +
                    "</td></tr>";


        strHtml += "<tr><td " + objConverter.Email_SubHeaderColor() + " colspan=" + Convert.ToChar(34) + 20 + Convert.ToChar(34) + "><b>Trade Register of : " + Dt.Rows[0]["cnt_firstname"].ToString() + " [" + Dt.Rows[0]["cnt_ucc"].ToString() + "]</b> at " + Dt.Rows[0]["ComCustomerTrades_TradeDate"].ToString() + "</td></tr>";

        strHtml += "<tr><td colspan=" +
                Convert.ToChar(34) + 20 + Convert.ToChar(34) + ">" +
                "&nbsp;</td></tr>";



        strHtml += "<tr " + objConverter.Email_ContentHeaderColor() + "><td><b>TradeDate</b></td><td>" +
                            "<b>Segment</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Order No</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Trade No</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>TradeTime</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Instrument</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Lots</b>" +
                                "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Lots Size</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Bought(Price Units)</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Sold(Price Units)</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Unit Price</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Quote Unit</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Type</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Brkg</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Total Brkg</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Net Rate</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Net Value</b>" + 
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Srv Tax</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Net Amount</b>" +
                             "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Net</b>" +
                            "</td></tr>";
                            

        DataTable dt3 = new DataTable();
        dt3 = gettotalquantityicex(Dt).Copy();

        foreach (DataRow dr in Dt.Rows)
        {
            //strHtml += "<tr ><td>" + dr["CdslHolding_ISIN"].ToString() + "</td>";
            //strHtml += "<td>" + dr["CDSLISIN_ShortName"].ToString() + "</td>";
            strHtml += "<tr >";
            if (dr["ComCustomerTrades_TradeDate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["ComCustomerTrades_TradeDate"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["Segmentname"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["Segmentname"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["OrderNo"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["OrderNo"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["TradeNo"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["TradeNo"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["TradeEntryTime"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["TradeEntryTime"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["productseries"] != DBNull.Value)
            {
                //string str = dr["productseries"].ToString();
                //if (dr["productseries"].ToString().Contains("&nbsp;"))
                //{
                   
                //    string[] str1=str.Split(';');
                //    str = "";
                //    for (int i = 0; i < str1.Length; i++)
                //    {
                //        str = str + " " + str1[i];
                //    }
                //    str = str.Trim();
                //}
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["productseries"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_QuantityLots"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_QuantityLots"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["LotsSize"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["LotsSize"].ToString()  + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["ComCustomerTrades_UnitPriceQuantity_Bought"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_UnitPriceQuantity_Bought"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_UnitPriceQuantity_sold"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_UnitPriceQuantity_sold"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_UnitPrice"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_UnitPrice"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Priceper"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["Priceper"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_BrokerageType"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["ComCustomerTrades_BrokerageType"].ToString()+ "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["ComCustomerTrades_UnitBrokerage"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_UnitBrokerage"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_TotalBrokerage"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_TotalBrokerage"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["ComCustomerTrades_NetRatePerUnit"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_NetRatePerUnit"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["ComCustomerTrades_NetValue"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["ComCustomerTrades_NetValue"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["servicetax"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["servicetax"])) +" " + dr["servicetaxmode"].ToString()+"</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["netamount"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["netamount"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dt3.Rows[jj][0] != "no value")
            {
                decimal aa = Convert.ToDecimal(dt3.Rows[jj][0]);
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(aa) + "</td>";

            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            jj++;
            strHtml += "</tr>";



        }
        //int bought = Convert.ToInt32(Dt.Rows[0]["CustomerTrades_UnitPriceQuantity_Bought"]);
        //int sold = Convert.ToInt32(Dt.Rows[0]["CustomerTrades_UnitPriceQuantity_sold"]);
        //int net = bought - sold;
        //strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" +net + "</td>";
        strHtml += "<tr ><td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + "Client Total" + "</td>";
        strHtml +=  "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_TotalBrokerage"])) + "</td>";//"<br>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customerbrkgamount"])) + "</td>";
        strHtml += "<td>&nbsp;</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_NetValue"])) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customernetamount"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_SrvTax"].ToString())) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customersrvtax"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt1.Rows[0]["Sum_NetAmount"].ToString())) + "</td>";//"<br/>" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt2.Rows[0]["customernetamountgrand"])) + "</td>";

        strHtml += "</tr></table><br/>";

        return strHtml;

    }

    public String NSEFrmEmail(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        //DataTable NSE = new DataTable();
        DataSet NSE = new DataSet();
        DataTable dt = new DataTable();
        String str;

        str = String.Empty;

        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
           
                {
                    
                    SqlCommand cmd = new SqlCommand("email_traderegister_html", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@clients_internalId", contactid);
                    cmd.Parameters.AddWithValue("@segmentname", subject[2].Split('-')[0]);
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(NSE);

                    SqlCommand cmd1 = new SqlCommand("email_traderegister_html_netsum", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@clients_internalId", contactid);
                    cmd1.Parameters.AddWithValue("@segmentname", subject[2].Split('-')[0]);
                    cmd1.CommandTimeout = 0;
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd1;
                    da1.Fill(dt);
                }
                if (subject[2].Split('-')[0].ToUpper() == "NSE")
                {
                    #region NSE-CM
                    if (NSE.Tables[0].Rows.Count > 0)
                    {


                        DataView HNSEDV = new DataView(NSE.Tables[0]);
                        DataTable FilteredNSE = new DataTable();
                        DataTable DistinctAccount = HNSEDV.ToTable(true, new string[] { "CustomerTrades_CustomerID" });

                        foreach (DataRow dr in DistinctAccount.Rows)
                        {

                            HNSEDV.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "'";
                            FilteredNSE = HNSEDV.ToTable();
                            if (FilteredNSE.Rows.Count > 0)
                            {
                                string subject1 = " ";
                                for (int a = 0; a < subject.Length; a++)
                                {
                                    subject1 = subject1 + " " + subject[a];
                                }
                                subject1 = subject1.Trim();
                                str += generateMailHoldingBodyNSE(FilteredNSE, NSE.Tables[1], dt, subject1);
                            }
                            FilteredNSE.Reset();



                        }

                    }

                    if (str == string.Empty)
                    {
                        str = "No Record Found";
                    }
                    str += "<br/>";
                    //str += new Converter().SendMailHtmlBody("abcd@gmail.com", Email, subject, str);

                    return str;
                    # endregion NSE-CM
                }
                else
                {
                    #region ICEX
                    if (NSE.Tables[0].Rows.Count > 0)
                    {


                        DataView HNSEDV = new DataView(NSE.Tables[0]);
                        DataTable FilteredNSE = new DataTable();
                        DataTable DistinctAccount = HNSEDV.ToTable(true, new string[] { "ComCustomerTrades_CustomerID" });

                        foreach (DataRow dr in DistinctAccount.Rows)
                        {

                            HNSEDV.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "'";
                            FilteredNSE = HNSEDV.ToTable();
                            if (FilteredNSE.Rows.Count > 0)
                            {
                                string subject1 = " ";
                                for (int a = 0; a < subject.Length; a++)
                                {
                                    subject1 = subject1 + " " + subject[a];
                                }
                                subject1 = subject1.Trim();
                                str += generateMailHoldingBodyICEX(FilteredNSE, NSE.Tables[1], dt, subject1);
                            }
                            FilteredNSE.Reset();



                        }

                    }

                    if (str == string.Empty)
                    {
                        str = "No Record Found";
                    }
                    str += "<br/>";
                    //str += new Converter().SendMailHtmlBody("abcd@gmail.com", Email, subject, str);

                    return str;
                    # endregion ICEX
                }
            }

    public String ledgerFrmEmail(string contactid, string[] subject)
    {
        DataSet ds = new DataSet();
        int all = 0;
        DataSet NSE = new DataSet();
        DataTable dt = new DataTable();
        String str, str1 = "";
        string segment = "0";
        str = String.Empty;
        string fromdate = null, todate = null, compid="",segmentname="";
        string[] mulcontactid = contactid.Split('|');
        try
        {

            

            segment = subject[2].ToUpper();
            if (segment == "ICEX-COMM")
            {
                segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][0].ToString();
                compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][1].ToString();
                 HttpContext.Current.Session["segmentname"] = "ICEX-COMM";
                HttpContext.Current.Session["CompanyID"] = compid;
                HttpContext.Current.Session["LastCompany"] = compid;
                HttpContext.Current.Session["segmentname"] = "ICEX-COMM";
               
            }
            else if (segment == "MCX-COMM")
            {
                segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][0].ToString();
                compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][1].ToString();
                HttpContext.Current.Session["segmentname"] = "MCX-COMM";
                HttpContext.Current.Session["CompanyID"] = compid;
                HttpContext.Current.Session["LastCompany"] = compid;
                HttpContext.Current.Session["segmentname"] = "MCX-COMM";
            }
            else if (segment == "NSE-FO")
            {
                segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][0].ToString();
                compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][1].ToString();

                HttpContext.Current.Session["segmentname"] = "NSE-FO";
                HttpContext.Current.Session["CompanyID"] = compid;
                HttpContext.Current.Session["LastCompany"] = compid;
                HttpContext.Current.Session["segmentname"] = "NSE-FO";
            }
            else if (segment == "NSE-CM")
            {
                segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][0].ToString();
                compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][1].ToString();
                HttpContext.Current.Session["segmentname"] = "NSE-CM";
                HttpContext.Current.Session["CompanyID"] = compid;
                HttpContext.Current.Session["LastCompany"] = compid;
                HttpContext.Current.Session["segmentname"] = "NSE-CM";
            }
            else if (segment == "ALL")
            {
                
            }
            else
            {
                all = 1;
            }
            int flag = 0;
            
            for (int a = 0; a < subject.Length; a++)
            {

                if (subject[a].Contains("/") && flag == 0)
                {
                    fromdate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                    flag = 1;
                }
                else if (flag == 1)
                {
                    todate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                }
                else
                {

                    fromdate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
                    todate = System.DateTime.Now.ToShortDateString(); 

                }
            }
            HttpContext.Current.Session["fromdate"] = fromdate+" 12:00:00 AM";
            HttpContext.Current.Session["todate"] = todate + " 12:00:00 AM";
            HttpContext.Current.Session["mailpath"] = "";
            HttpContext.Current.Session["mail"] = "yes";
        }
        catch
        {
            fromdate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            todate = System.DateTime.Now.ToShortDateString();
            HttpContext.Current.Session["fromdate"] = fromdate + " 12:00:00 AM";
            HttpContext.Current.Session["todate"] = todate + " 12:00:00 AM";
            all = 1;
        } 
    
        
        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
                for (int i = 0; i < mulcontactid.Length; i++)
                    {
                        
                            contactid = mulcontactid[i].ToString();
                            contactidnew1 = contactid;
                            str1 = str1 + contactidnew1 + "|";
                        
                    }
                    str1 = str1.Remove(str1.Length - 1, 1);
                    contactidnew = str1.Split('|');
                    for (int j = 0; j < contactidnew.Length; j++)
                    {
                        if (all == 0)
                        {
                            HttpContext.Current.Session["userid"] = contactidnew[j].ToString();
                            HttpContext.Current.Session["userbranchHierarchy"] = "2,6,9,10,12,51,52,13,14,15,16,17,18,19,20,21,48,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,41,45,38,40,42,39,43,44,46,47,49,50,1";
                            HttpContext.Current.Session["SegmentID"] = segment;

                            l.Exportledger();
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (i == 0)
                                {
                                    segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][0].ToString();
                                    compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ").Rows[0][1].ToString();
                                    HttpContext.Current.Session["SegmentID"] = segment;
                                    HttpContext.Current.Session["CompanyID"] = compid;
                                    HttpContext.Current.Session["LastCompany"] = compid;
                                    HttpContext.Current.Session["segmentname"] = "ICEX-COMM";
                                    ds.Reset();
                                    ds = fetch_status_email(HttpContext.Current.Request.QueryString["senderid"].ToString(), "TRADE~LEDGER~ICEX-COMM");
                                }
                                if (i == 1)
                                {
                                    segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][0].ToString();
                                    compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ").Rows[0][1].ToString();
                                    HttpContext.Current.Session["SegmentID"] = segment;
                                    HttpContext.Current.Session["CompanyID"] = compid;
                                    HttpContext.Current.Session["LastCompany"] = compid;
                                    HttpContext.Current.Session["segmentname"] = "MCX-COMM";
                                    ds.Reset();
                                    ds = fetch_status_email(HttpContext.Current.Request.QueryString["senderid"].ToString(), "TRADE~LEDGER~MCX-COMM");
                                }
                                if (i == 2)
                                {
                                    segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][0].ToString();
                                    compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ").Rows[0][1].ToString();
                                    HttpContext.Current.Session["SegmentID"] = segment;
                                    HttpContext.Current.Session["CompanyID"] = compid;
                                    HttpContext.Current.Session["LastCompany"] = compid;
                                    HttpContext.Current.Session["segmentname"] = "NSE-FO";
                                    ds.Reset();
                                    ds = fetch_status_email(HttpContext.Current.Request.QueryString["senderid"].ToString(), "TRADE~LEDGER~NSE-FO");
                                }
                                if (i == 3)
                                {
                                    segment = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][0].ToString();
                                    compid = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ").Rows[0][1].ToString();
                                    HttpContext.Current.Session["segmentname"] = "NSE-CM";
                                    HttpContext.Current.Session["SegmentID"] = segment;
                                    HttpContext.Current.Session["CompanyID"] = compid;
                                    HttpContext.Current.Session["LastCompany"] = compid;
                                    ds.Reset();
                                    ds = fetch_status_email(HttpContext.Current.Request.QueryString["senderid"].ToString(), "TRADE~LEDGER~NSE-CM");
                                }
                                HttpContext.Current.Session["userid"] = contactidnew[j].ToString();
                                HttpContext.Current.Session["userbranchHierarchy"] = "2,6,9,10,12,51,52,13,14,15,16,17,18,19,20,21,48,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,41,45,38,40,42,39,43,44,46,47,49,50,1";
                                if (ds.Tables[1].Rows.Count != 0)
                                {
                                    l.Exportledger();
                                }
                            }
                        }
                    }
        }


        return "s";

        }

    public DataTable gettotalquantity(DataTable Dt)
    {
        int rowcount = Dt.Rows.Count;
        string prev_productseries, prev_customer;
        prev_productseries = "";
        prev_customer = "";
        decimal Quantity=0;
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("quantity", System.Type.GetType("System.String"));

        for (int j = 0; j < rowcount; j++)
        {

            if (j == 0)
            {
                prev_productseries = Dt.Rows[j]["Symbol"].ToString();
                prev_customer = Dt.Rows[j]["CustomerTrades_CustomerID"].ToString();
                Quantity = Convert.ToDecimal(Dt.Rows[j]["CustomerTrades_Quantity"]);
                dt2.Rows.Add("no value");
            }
           
               

            else if (prev_productseries == Dt.Rows[j]["Symbol"].ToString() && prev_customer == Dt.Rows[j]["CustomerTrades_CustomerID"].ToString())
            {
               
                Quantity += Convert.ToDecimal(Dt.Rows[j]["CustomerTrades_Quantity"]);
                prev_productseries = Dt.Rows[j]["Symbol"].ToString();
                prev_customer = Dt.Rows[j]["CustomerTrades_CustomerID"].ToString();
                dt2.Rows.Add("no value");
                
            }
            else 
            {
               
                dt2.Rows.Add(Quantity);
                Quantity = Convert.ToDecimal(Dt.Rows[j]["CustomerTrades_Quantity"]);
                prev_productseries = Dt.Rows[j]["Symbol"].ToString();
                prev_customer = Dt.Rows[j]["CustomerTrades_CustomerID"].ToString();
               
            }

           
        }
        dt2.Rows.RemoveAt(0);
        dt2.Rows.Add(Quantity);
           
        return dt2;

    }

    public DataTable gettotalquantityicex(DataTable Dt)
    {
        int rowcount = Dt.Rows.Count;
        string prev_productseries, prev_customer;
        prev_productseries = "";
        prev_customer = "";
        decimal Quantity = 0;
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("quantity", System.Type.GetType("System.String"));

        for (int j = 0; j < rowcount; j++)
        {

            if (j == 0)
            {
                prev_productseries = Dt.Rows[j]["productseries"].ToString();
                prev_customer = Dt.Rows[j]["ComCustomerTrades_CustomerID"].ToString();
                Quantity = Convert.ToDecimal(Dt.Rows[j]["QuantityLots"]);
                dt2.Rows.Add("no value");
            }



            else if (prev_productseries == Dt.Rows[j]["productseries"].ToString() && prev_customer == Dt.Rows[j]["comCustomerTrades_CustomerID"].ToString())
            {

                Quantity += Convert.ToDecimal(Dt.Rows[j]["QuantityLots"]);
                prev_productseries = Dt.Rows[j]["productseries"].ToString();
                prev_customer = Dt.Rows[j]["comCustomerTrades_CustomerID"].ToString();
                dt2.Rows.Add("no value");

            }
            else
            {

                dt2.Rows.Add(Quantity);
                Quantity = Convert.ToDecimal(Dt.Rows[j]["QuantityLots"]);
                prev_productseries = Dt.Rows[j]["productseries"].ToString();
                prev_customer = Dt.Rows[j]["comCustomerTrades_CustomerID"].ToString();

            }


        }
        dt2.Rows.RemoveAt(0);
        dt2.Rows.Add(Quantity);

        return dt2;

    }


    public DataTable gettotalquantityledger(DataTable Dt,DataTable dt1,decimal opening)
    {
        int rowcount = Dt.Rows.Count;
        decimal balance,debit=0.0M,credit=0.0M;
        balance = opening;
        string prev_productseries, prev_customer;
        prev_productseries = "";
        prev_customer = "";
        
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("quantity", System.Type.GetType("System.String"));
       
        for (int j = 0; j < rowcount; j++)
        {
            if (j == 0)
            {
                if (Dt.Rows[j]["Accountsledger_AmountCr"] == DBNull.Value)
                {
                    debit = balance;
                    balance = debit;
                }
                else
                {
                    debit = opening - Convert.ToDecimal(Dt.Rows[j]["Accountsledger_AmountCr"]);
                    balance = debit;
                }
                if (Dt.Rows[j]["Accountsledger_AmountDr"] == DBNull.Value)
                {
                    credit = balance;
                    balance = credit;
                }
                else
                {
                    credit = balance + Convert.ToDecimal(Dt.Rows[j]["Accountsledger_AmountDr"]);
                    balance = credit;
                }
            }
            else
            {

                if (Dt.Rows[j]["Accountsledger_AmountCr"] == DBNull.Value)
                {

                }
                else
                {
                    debit = balance - Convert.ToDecimal(Dt.Rows[j]["Accountsledger_AmountCr"]);
                    balance = debit;
                }
                if (Dt.Rows[j]["Accountsledger_AmountDr"] == DBNull.Value)
                {

                }
                else
                {
                    credit = balance + Convert.ToDecimal(Dt.Rows[j]["Accountsledger_AmountDr"]);
                    balance = credit;
                }
            }
               
                //balance =credit - debit ;
                dt2.Rows.Add(balance);
                     
        }
        //dt2.Rows.RemoveAt(0);
        dt2.Rows.Add(balance);

        return dt2;

    }

    public String generateMailHoldingBodyledger(DataTable Dt, DataTable Dt2, string subject, DataTable Dt3,string segment)
    {

        Converter objConverter = new Converter();
        String strHtml;
        int jj = 0;
        strHtml = String.Empty;

        //strHtml = "<table style=" + Convert.ToChar(34) + "font-family: Tahoma,Arial, Verdana, sans-serif; font-size: 12px; padding-left: 1px; padding-right: 1px; padding-bottom:2px; padding-top:2px;" + Convert.ToChar(34) + " width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " >";

        strHtml = "<table " + objConverter.Email_TableStyle() + " >";
        strHtml += "<tr><td " + objConverter.Email_HeaderColor() + " colspan=" +
                    Convert.ToChar(34) + 9 + Convert.ToChar(34) + ">" +
                    objConverter.Email_HeaderText(Dt.Rows[0]["companyname"].ToString()) +
                    "<br/>" + Dt.Rows[0]["address1"].ToString() +
                    "<br/>" + Dt.Rows[0]["address2"].ToString() +
                    "," + Dt.Rows[0]["Phone"].ToString() +
                    "<br/>ServTaxRegn No:" + Dt.Rows[0]["StaxRegNo"].ToString() +
                    "  Pan:" + Dt.Rows[0]["Pan"].ToString() +
                    "<br/>" + subject +
                    "</td></tr>";


        strHtml += "<tr><td " + objConverter.Email_SubHeaderColor() + " colspan=" + Convert.ToChar(34) + 9 + Convert.ToChar(34) + "><b>Ledger of : " + Dt.Rows[0]["AccountName"].ToString() + "</b> at " + Dt.Rows[0]["accountsledger_transactiondate"].ToString() + "</td></tr>";

        strHtml += "<tr><td colspan=" +
                Convert.ToChar(34) + 9 + Convert.ToChar(34) + ">" +
                "&nbsp;</td></tr>";

      
             




        strHtml += "<tr " + objConverter.Email_ContentHeaderColor() + "><td><b>Tr. Date</b></td><td>" +
                            "<b>ValueDate</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Voucher No</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Description</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Instrument No.</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Settlement No.</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>debit</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Credit</b>" +
                            "</td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
                            "<b>Closing</b>" +
                             "</td></tr>";
        DataTable d = new DataTable();
        d = oDBEngine.OpeningBalance_a(Dt.Rows[0]["AccountsLedger_SubAccountID"].ToString(), Convert.ToDateTime(Dt.Rows[0]["AccountsLedger_TransactionDate"]), Convert.ToDateTime(Convert.ToDateTime(Dt.Rows[0]["AccountsLedger_TransactionDate"]).ToShortDateString() + " " + "23:59:59"),segment,"").Copy();
        decimal openinng = Convert.ToDecimal(d.Rows[0][1]);
        strHtml += "<tr><td>&nbsp;</td>" +
              "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" +
               "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" +
             "<td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">"
             + "<b>Opening Balance</b></td>" + "<td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + ">" +
              d.Rows[0][1].ToString()+"</td></tr>";
       
        DataTable dt3 = new DataTable();
        dt3 = gettotalquantityledger(Dt, d, openinng).Copy();

        foreach (DataRow dr in Dt.Rows)
        {
            
            strHtml += "<tr >";
            if (dr["AccountsLedger_TransactionDate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["AccountsLedger_TransactionDate"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["valuedate"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["valuedate"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["AccountsLedger_TransactionReferenceID"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["AccountsLedger_TransactionReferenceID"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["AccountsLedger_Narration"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["AccountsLedger_Narration"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["AccountsLedger_InstrumentNumber"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["AccountsLedger_InstrumentNumber"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

            if (dr["SettlementNumber"] != DBNull.Value)
            {

                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + dr["SettlementNumber"].ToString() + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Accountsledger_AmountCr"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Accountsledger_AmountCr"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dr["Accountsledger_AmountDr"] != DBNull.Value)
            {
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dr["Accountsledger_AmountDr"])) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }

          
            if (dt3.Rows[jj][0] != DBNull.Value)
            {
                decimal aa = Convert.ToDecimal(dt3.Rows[jj][0]);
                strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(aa) + "</td>";

            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            jj++;
            strHtml += "</tr>";



        }
        
        strHtml += "<tr ><td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + "Client Total" + "</td>";
        strHtml += "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>" + "<td>&nbsp;</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt3.Rows[0]["sumAccountsledger_AmountDr"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(Dt3.Rows[0]["sumAccountsledger_AmountCr"])) + "</td>";
        strHtml += "<td style=" + Convert.ToChar(34) + "text-align:right " + Convert.ToChar(34) + ">" + objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt3.Rows[dt3.Rows.Count-1][0])) + "</td>";


        strHtml += "</tr></table><br/>";

        return strHtml;

    }

    public string cdslbillFrmEmail(string contactid, string[] subject)
    {
        string[] mulcontactid = contactid.Split('|');
        DataSet HoldingDT = new DataSet();
        DataSet NSDLHoldingDT = new DataSet();
        
        String str, str1 = "";

        str = String.Empty;
        HttpContext.Current.Session["mailpath"] = "";
        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] == "CDSL" || subject[2] == "cdsl")
            {
                HttpContext.Current.Session["userlastsegment"] = 10;
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
                //string month = System.DateTime.Now.ToLongDateString().Split(',')[1].Substring(1,3);
                //string month = "DEC";
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
                    HttpContext.Current.Session["mail"] = "yes";
                    HttpContext.Current.Session["no"] = j;
                    HttpContext.Current.Session["LastFinYear"] = HoldingDT.Tables[2].Rows[0][0].ToString();
                    cbill.callCrystalReport(HoldingDT.Tables[0].Rows[0][0].ToString().Substring(1,3), contactidnew[j],
                    "", "PinCode", "CDSL",
                    "0.01", "Print", 0, "", "", "", 1408,0,0);
                    
                    HoldingDT.Reset();
                    
                }
            }
            else
            {
                HttpContext.Current.Session["userlastsegment"] = 9;
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
                   
                    
                }

            }
        return "ss";
        
    }

    public string cdsltransactionFrmEmail(string contactid, string[] subject)
    {
        transaction t= new transaction();       
        string[] mulcontactid = contactid.Split('|');
        DataSet HoldingDT = new DataSet();
        DataSet NSDLHoldingDT = new DataSet();
        string fromdate = null, todate = null;
        String str, str1 = "";
        int flag = 0;
        for (int a = 0; a < subject.Length; a++)
        {           

            if (subject[a].Contains("/") && flag == 0)
            {
                fromdate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                flag = 1;
            }
            else if (flag == 1)
            {
                todate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
            }
            else
            {

                fromdate = System.DateTime.Now.ToShortDateString().Split('/')[0] +"/"+ "01"  + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];

                todate = System.DateTime.Now.ToShortDateString();

            }
        }
        HttpContext.Current.Session["fromdate"] = fromdate;
        HttpContext.Current.Session["todate"] = todate;
       
        str = String.Empty;
        HttpContext.Current.Session["mailpath"] = "";
        //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            if (subject.Length == 2 || subject[2] != "NSDL" )
            {
                HttpContext.Current.Session["userlastsegment"] = 10;
                for (int i = 0; i < mulcontactid.Length; i++)
                {
                    if (mulcontactid[i].ToString().Substring(0, 1) != "I")
                    {

                        contactid = mulcontactid[i].ToString();
                        contactidnew1 = contactid;
                        str1 = str1 + contactid + "|";
                    }
                }
                str1 = str1.Remove(str1.Length - 1, 1);
                contactidnew = str1.Split('|');
                //string month = System.DateTime.Now.ToLongDateString().Split(',')[1].Substring(1,3);
                //string month = "DEC";
                for (int j = 0; j < contactidnew.Length; j++)
                {

                    SqlCommand cmd = new SqlCommand("dp_bill_cdsl_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@boid", contactidnew[j].ToString().Substring(contactidnew[j].Length - 8, 8));
                    cmd.Parameters.AddWithValue("@seg", "CDSL");
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HoldingDT);
                    HttpContext.Current.Session["mail"] = "yes";
                    HttpContext.Current.Session["no"] = j;
                    HttpContext.Current.Session["LastFinYear"] = HoldingDT.Tables[2].Rows[0][0].ToString();
                    HttpContext.Current.Session["userid"] = contactidnew[j].ToString();
                    t.showCrystalReport();

                    HoldingDT.Reset();

                }
            }
            else
            {
                HttpContext.Current.Session["userlastsegment"] = 9;
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


                }

            }
        return "ss";

    }


    public String GetLedgerWithObligationBreakup(string contactid, string[] subject)
    {
        //---Develope By Goutam Das---
        DataTable dtSeg = new DataTable();
        string SegmentID = string.Empty;
        String BranchID = string.Empty;
        String company = string.Empty;
        string FinYear = string.Empty;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string segment = string.Empty;
        string exch1 = string.Empty;
        string exch2 = string.Empty;
        string MainAccountCode = string.Empty;
        int flag = 0;

        string strMain = "<table>";

        // DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + contactid + "'");
        DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");
        //BranchID = DTBR.Rows[0][0].ToString();
        FinYear = dTFY.Rows[0][0].ToString();
        StartDate = dTFY.Rows[0][1].ToString();
        EndDate = dTFY.Rows[0][2].ToString();

        string[] mulcontactid = contactid.Split('|');
        try
        {
            segment = subject[2].ToUpper();
            if (segment == "ICEX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = "'" + dtSeg.Rows[0][1].ToString() + "'";
            }
            else if (segment == "MCX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = "'" + dtSeg.Rows[0][1].ToString() + "'";
            }
            else if (segment == "NSE-FO")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = "'" + dtSeg.Rows[0][1].ToString() + "'";
            }
            else if (segment == "NSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = "'" + dtSeg.Rows[0][1].ToString() + "'";
            }
            else if (segment == "BSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXB0000001' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = "'" + dtSeg.Rows[0][1].ToString() + "'";
            }



            for (int a = 0; a < subject.Length; a++)
            {
                if (subject[a].Contains("/") && flag == 0)
                {
                    StartDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                    flag = 1;
                }
                else if (flag == 1)
                {
                    EndDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                }
                else
                {

                    StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
                    EndDate = System.DateTime.Now.ToShortDateString();

                }
            }
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 12:00:00 AM";

        }
        catch
        {
            StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            EndDate = System.DateTime.Now.ToShortDateString();
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 12:00:00 AM";
        }


        for (int p = 0; p < mulcontactid.Length; p++)
        {
            int s = 0;

            DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[p] + "'");
            BranchID = DTBR.Rows[0][0].ToString();
            DataSet ds = new DataSet();

            if (segment == "ALL")
            {

                string[,] data = oDBEngine.GetFieldValue(" tbl_master_contactExchange ", " crg_exchange,crg_company ", " crg_cntid='" + mulcontactid[p] + "'", 2);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        if (exch1 == "")
                        {
                            exch1 = "'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 = "'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            company = "'" + data[i, 1] + "'";

                        }
                        else
                        {
                            exch1 += ",'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 += ",'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            company += ",'" + data[i, 1].Trim() + "'";

                        }
                    }
                }
                data = oDBEngine.GetFieldValue("  (SELECT     exch_internalId, exch_compId,(SELECT exh_shortName  FROM  tbl_master_exchange  WHERE (exh_cntId = tbl_master_companyExchange.exch_exchId)) AS exch, exch_segmentId FROM   tbl_master_companyExchange  WHERE  (exch_compId IN (" + company + "))) AS d ", "  exch_internalId ", "  (exch in (" + exch1 + ")) AND (exch_segmentId in (" + exch2 + "))", 1);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (SegmentID == "")
                            SegmentID = data[i, 0];
                        else
                            SegmentID += "," + data[i, 0];
                    }
                }

            }

            for (s = 0; s < 2; s++)
            {
                if (s == 0)
                    MainAccountCode = "'SYSTM00001'";
                else
                    MainAccountCode = "'SYSTM00002'";


                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("[Fetch_LedgerForClientApp]", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@CompanyID", company);
                        da.SelectCommand.Parameters.AddWithValue("@FinYear", FinYear);
                        da.SelectCommand.Parameters.AddWithValue("@FromDate", StartDate);
                        da.SelectCommand.Parameters.AddWithValue("@ToDate", EndDate);
                        da.SelectCommand.Parameters.AddWithValue("@MainAccount", MainAccountCode);
                        da.SelectCommand.Parameters.AddWithValue("@SubAccount", "'" + mulcontactid[p] + "'");
                        da.SelectCommand.Parameters.AddWithValue("@Branch", BranchID);
                        da.SelectCommand.Parameters.AddWithValue("@ReportType", "ObligationBrkUp");
                        da.SelectCommand.Parameters.AddWithValue("@Segment", SegmentID);
                        da.SelectCommand.Parameters.AddWithValue("@UserID", "275");
                        da.SelectCommand.Parameters.AddWithValue("@SubledgerType", "Customers");
                        da.SelectCommand.Parameters.AddWithValue("@Settlement", "");

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        
                    }
                }

                DataSet dsHTML = ds;
                DataTable dtMain = dsHTML.Tables[2];
                DataTable dtScripCM = dsHTML.Tables[0];
                DataTable dtTax = dsHTML.Tables[1];
                DataTable dtScripFO = dsHTML.Tables[5];
                string SegmentName = "";
                string SegID = "";
                String CustomerID = "";
                string SettlementNo = "";
                string SettlementType = "";
                string SettlementNOType = "";
                string TransactionDate = "";
                string IsConsolidate = "";
                string strHTML = "";
                decimal TotOblg = 0;
                decimal TotChrg = 0;
                decimal TotNet = 0;

                if (s == 0)
                {
                    strMain = strMain + "<tr><td align=\"left\"  style=\"background-color:#ffffff;font-weight:bold;\">Name: " + ds.Tables[4].Rows[0]["CustomerName"].ToString() + "</td></tr>";
                    strMain = strMain + "<tr><td align=\"left\"  style=\"background-color:#ffffff;font-weight:bold;\">Address: " + ds.Tables[4].Rows[0]["CustomerAddress"].ToString() + "</td></tr>";
                    strMain = strMain + "<tr><td align=\"left\"  style=\"background-color:#ffffff;font-weight:bold;\">Phone: " + ds.Tables[4].Rows[0]["CustomerPhone"].ToString() + "</td></tr>";
                    strMain = strMain + "<tr><td align=\"left\"  style=\"background-color:#ffffff;font-weight:bold;\">PAN: " + ds.Tables[4].Rows[0]["CustomerPhone"].ToString() + "</td></tr>";
                }
                else
                {
                    strMain = strMain + "<tr><td align=\"left\"  style=\"background-color:#ffffff;font-weight:bold;\">Main Account: Client Margin Account [SYSTM]</td></tr>";

                }

                if (dtMain.Rows.Count > 0)
                {
                    strHTML = "<table border=\"1\" width=\"850px\" cellspacing=\"0\" cellpadding=\"0\" style=\"font-family:Verdana;font-size:8pt;\" >";
                    strHTML = strHTML + "<tr style=\"background-color:#B7CEEC;font-weight:bold;height:30px\">";
                    strHTML = strHTML + "<td style=\"font-weight:bold;  width:7%\">Tr. Date</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\">Value Date</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\">Inst. No.</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\">Description</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\">Trade Date</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\">Settlement No.</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Debit</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Credit</td>";
                    strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Closing</td>";
                    strHTML = strHTML + "</tr>";
                    if (dtMain.Rows[0]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceDr"] == "")
                        dtMain.Rows[0]["OpeningBalanceDr"] = "0";
                    if (dtMain.Rows[0]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceCr"] == "")
                        dtMain.Rows[0]["OpeningBalanceCr"] = "0";
                    if (dtMain.Rows[0]["OpeningTotal"] == DBNull.Value || dtMain.Rows[0]["OpeningTotal"] == "")
                        dtMain.Rows[0]["OpeningTotal"] = "0";
                    if (dtMain.Rows[0]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceDr"].ToString() == "" || dtMain.Rows[0]["OpeningBalanceDr"].ToString() == "0")
                        strHTML = strHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;</td>";
                    else
                        strHTML = strHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningBalanceDr"].ToString())) + "</td>";
                    if (dtMain.Rows[0]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceCr"].ToString() == "" || dtMain.Rows[0]["OpeningBalanceCr"].ToString() == "0")
                        strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td>";
                    else
                        strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningBalanceCr"].ToString())) + "</td>";

                    if (dtMain.Rows[0]["OpeningTotal"] == DBNull.Value || dtMain.Rows[0]["OpeningTotal"].ToString() == "" || dtMain.Rows[0]["OpeningTotal"].ToString() == "0")
                        strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td></tr>";
                    else
                        strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningTotal"].ToString())) + "</td></tr>";


                    for (int i = 0; i < dtMain.Rows.Count; i++)
                    {
                        //--------------------Get Variable-------------------
                        SegmentName = dtMain.Rows[i]["SegmentName"].ToString();
                        SegID = dtMain.Rows[i]["SegID"].ToString();
                        CustomerID = dtMain.Rows[i]["SubID"].ToString();
                        SettlementNo = dtMain.Rows[i]["SettlementN"].ToString();
                        SettlementType = dtMain.Rows[i]["SettlementT"].ToString();
                        SettlementNOType = dtMain.Rows[i]["SettlementNumber"].ToString();
                        TransactionDate = dtMain.Rows[i]["accountsledger_transactiondate"].ToString();
                        IsConsolidate = dtMain.Rows[i]["accountsledger_Narration"].ToString().Substring(0, 21);
                        //--------------------End -------------------
                        strHTML = strHTML + "<tr>";
                        strHTML = strHTML + "<td >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                        strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                        strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                        strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                        if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                        {
                            strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                        }
                        else
                        {
                            strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                        }
                        strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                        if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                            strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                            strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                        {
                            strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                strHTML = strHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            else
                                strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                        }

                        strHTML = strHTML + "</tr>";
                        string SubStr = "";
                        if (IsConsolidate == "Settlement Obligation")
                        {

                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                if (dtScripCM.Rows.Count > 0)
                                {
                                    TotOblg = 0;
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"4\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                    for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                    {

                                        if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                        {

                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td>";

                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                        }

                                    }
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg - TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }

                            }
                            else
                            {

                                if (dtScripFO.Rows.Count > 0)
                                {
                                    SubStr = "<table border=\"1\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                    for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                    {
                                        if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";


                                            if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                            if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";
                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                        }

                                    }

                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg - TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";

                                    SubStr = SubStr + "</table>";

                                }




                            }
                        }

                        strHTML = strHTML + "<tr><td>&nbsp;</td><td colspan=\"5\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        SegmentName = "";
                        SegID = "";
                        CustomerID = "";
                        SettlementNo = "";
                        SettlementType = "";
                        SettlementNOType = "";
                        TransactionDate = "";
                        IsConsolidate = "";
                        TotOblg = 0;
                        TotChrg = 0;
                        TotNet = 0;


                    }
                    if (dtMain.Rows[0]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[0]["ClosingBalanceDr"] == "")
                        dtMain.Rows[0]["ClosingBalanceDr"] = "0";
                    if (dtMain.Rows[0]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[0]["ClosingBalancecr"] == "")
                        dtMain.Rows[0]["ClosingBalancecr"] = "0";
                    if (dtMain.Rows[0]["ClosingTotal"] == DBNull.Value || dtMain.Rows[0]["ClosingTotal"] == "")
                        dtMain.Rows[0]["ClosingTotal"] = "0";
                    strHTML = strHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingTotal"].ToString())) + "</td></tr>";
                    strHTML = strHTML + "</table>";
                }
                strMain = strMain + "<tr><td>" + strHTML + "</td></tr>";
            }


        }
        strMain = strMain + "</table>";
        return strMain;

    }


    public String GetTradeRegister(string contactid, string[] subject)
    {

        //---Develope By Goutam Das---
        DataTable dtSeg = new DataTable();

        string SegmentID = string.Empty;
        String BranchID = string.Empty;
        String company = string.Empty;
        string FinYear = string.Empty;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string segment = string.Empty;
        string exch1 = string.Empty;
        string exch2 = string.Empty;
        string BranchHr = string.Empty;
        string strMain = "<table>";


        int flag = 0;
        //DataTable dtBrHr = oDBEngine.GetDataTable(null, " SUBSTRING((SELECT ',' +  cast(branch_id as varchar) FROM tbl_master_branch  ORDER BY branch_id   FOR XML PATH('')),2,200000) ", null);
        //  DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + contactid + "'");
        DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");
        //BranchID = DTBR.Rows[0][0].ToString();
        FinYear = dTFY.Rows[0][0].ToString();
        StartDate = dTFY.Rows[0][1].ToString();
        EndDate = dTFY.Rows[0][2].ToString();
        //BranchHr = dtBrHr.Rows[0][0].ToString();

        string[] mulcontactid = contactid.Split('|');
        try
        {
            segment = subject[2].ToUpper();
            if (segment == "ICEX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "MCX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "NSE-FO")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "NSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "BSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXB0000001' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            for (int a = 0; a < subject.Length; a++)
            {
                if (subject[a].Contains("/") && flag == 0)
                {
                    StartDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                    EndDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                    flag = 1;
                }

                else
                {

                    StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
                    EndDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];

                }
            }
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 11:59:00 PM";

        }
        catch
        {
            StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            EndDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 11:59:00 PM";
        }

        for (int p = 0; p < mulcontactid.Length; p++)
        {

            DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[p] + "'");
            BranchID = DTBR.Rows[0][0].ToString();

            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  Multi
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("[TradeRegister]", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@fromdate", StartDate);
                    da.SelectCommand.Parameters.AddWithValue("@todate", EndDate);
                    da.SelectCommand.Parameters.AddWithValue("@tradetype", "3");
                    da.SelectCommand.Parameters.AddWithValue("@clients", "'" + mulcontactid[p] + "'");
                    da.SelectCommand.Parameters.AddWithValue("@instruments", "All");
                    da.SelectCommand.Parameters.AddWithValue("@segment", SegmentID);
                    da.SelectCommand.Parameters.AddWithValue("@SettlementNo", "All");
                    da.SelectCommand.Parameters.AddWithValue("@SettlementType", "All");
                    da.SelectCommand.Parameters.AddWithValue("@grptype", "BRANCH");
                    da.SelectCommand.Parameters.AddWithValue("@grp", BranchID);
                    da.SelectCommand.Parameters.AddWithValue("@companyid", company);
                    da.SelectCommand.Parameters.AddWithValue("@TERMINALID", "ALL");
                    da.SelectCommand.Parameters.AddWithValue("@BRANCHHierarchy", BranchID);
                    da.SelectCommand.Parameters.AddWithValue("@CTCLID", "All");
                    da.SelectCommand.Parameters.AddWithValue("@rpttype", "4");
                    da.SelectCommand.Parameters.AddWithValue("@Categoy", "ALL");
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    da.Dispose();
                    // ViewState["dataset"] = ds;
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                }
            }



            // DataView viewclient = new DataView();
            //viewclient = ds.Tables[0].DefaultView;
            //viewclient.RowFilter = "CustomerID='" + contactid.ToString().Trim() + "'";
            //dtClients.Clear();
            //dtClients = viewclient.ToTable();
            DataTable dtClients = ds.Tables[0];

            //////////NET CALCULATION

            string prv_Quantity = null;
            string Quantity = null;
            string productseries = null;
            int j;
            for (j = 0; j < dtClients.Rows.Count; j++)
            {
                Quantity = dtClients.Rows[j]["Quantity"].ToString();
                dtClients.Rows[j]["Quantity"] = DBNull.Value;
                if (productseries != null)
                {
                    if (productseries != dtClients.Rows[j]["ProductSeriesID"].ToString().Trim())
                    {
                        dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;
                    }
                }
                productseries = dtClients.Rows[j]["ProductSeriesID"].ToString();
                prv_Quantity = Quantity;
            }

            dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;
            String strHtml = String.Empty;
            flag = 0;
            /////////HTML TABLE HEADER
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=5><b>" + dtClients.Rows[0]["CLIENTNAME"].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade Date</b></td>";
            //if (rdbSegAll.Checked)
            //{
            //    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Segment</b></td>";
            //}
            //if (rdbSettNoAll.Checked && RadbSettlementTypeAll.Checked)
            //{
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sett No.</b></td>";
            // }
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Terminal Id</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>TrdCode</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Order No</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade No</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade Time</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Bought</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sold</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Mkt Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Type</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Rate</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Srv Tax</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Amnt</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</b></td>";

            for (int k = 0; k < dtClients.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeDate"].ToString().Trim() + "</td>";
                //if (rdbSegAll.Checked)
                //{
                //    strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["segmentname"].ToString().Trim() + "</td>";
                //}
                //if (rdbSettNoAll.Checked && RadbSettlementTypeAll.Checked)
                //{
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["settno"].ToString().Trim() + "</td>";
                // }
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Terminalid"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["EXCHGECUSTOMERUCC"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["OrderNumber"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeNumber"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeEntryTime"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Symbol"].ToString().Trim() + "</td>";

                if (dtClients.Rows[k]["Bought"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Bought"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["Sold"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dtClients.Rows[k]["Sold"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["MKTPRICE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["MKTPRICE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["brkgtype"].ToString().Trim() + "</td>";
                if (dtClients.Rows[k]["UnitBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["UnitBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["TotalBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dtClients.Rows[k]["TotalBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["NetRatePerUnit"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["NetRatePerUnit"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["NetValue"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["NetValue"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["servicetax"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["servicetax"].ToString() + "[" + dtClients.Rows[k]["ServiceTaxMode"].ToString().Trim() + "]</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["netamount"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["netamount"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["Quantity"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Quantity"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
            }
            //////////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total :</b></td>";
            //if (rdbSegAll.Checked)
            //{
            //    strHtml += "<td align=\"right\" >&nbsp;</td>";
            //}
            //if (rdbSettNoAll.Checked && RadbSettlementTypeAll.Checked)
            //{
            strHtml += "<td align=\"right\" >&nbsp;</td>";
            //}
            strHtml += "<td align=\"right\" colspan=6>&nbsp;</td>";
            if (dtClients.Rows[0]["Boughtsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["Boughtsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["Soldsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["Soldsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" colspan=3>&nbsp;</td>";
            if (dtClients.Rows[0]["TotalBrokeragesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["TotalBrokeragesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["NetValueesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["NetValueesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["servicetaxsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["servicetaxsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["netamntesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["netamntesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "</tr>";
            strHtml += "</table>";

            strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";
            strMain = strMain + "</table>";



        }
        return strMain;

    }



    protected string GetRowColor(int i)
    {
        if (i++ % 2 == 0)
            return "#fff0f5";
        else
            return "White";
    }



    public String GetNetPosition(string contactid, string[] subject)
    {

        //---Develope By Goutam Das---
        DataTable dtSeg = new DataTable();
        string SegmentID = string.Empty;
        String BranchID = string.Empty;
        String company = string.Empty;
        string FinYear = string.Empty;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string segment = string.Empty;
        string exch1 = string.Empty;
        string exch2 = string.Empty;
        string BranchHr = string.Empty;
        string UserSegID = string.Empty;
        String strHtml = String.Empty;
        String strHtml1 = String.Empty;
        string strMain = "<table>";
        int flag = 0;

        DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");

        FinYear = dTFY.Rows[0][0].ToString();
        StartDate = dTFY.Rows[0][1].ToString();
        EndDate = dTFY.Rows[0][2].ToString();
        string[] mulcontactid = contactid.Split('|');
        try
        {
            segment = subject[2].ToUpper();
            if (segment == "ICEX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
                UserSegID = "12";
            }
            else if (segment == "MCX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
                UserSegID = "7";
            }
            else if (segment == "NSE-FO")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
                UserSegID = "2";
            }
            else if (segment == "NSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
                UserSegID = "1";
            }
            else if (segment == "BSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXB0000001' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
                UserSegID = "4";
            }


            for (int a = 0; a < subject.Length; a++)
            {
                if (subject[a].Contains("/") && flag == 0)
                {
                    StartDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                    flag = 1;
                }
                else if (flag == 1)
                {
                    EndDate = subject[a].Split('/')[1] + "/" + subject[a].Split('/')[0] + "/" + subject[a].Split('/')[2];
                }
                else
                {

                    StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
                    EndDate = System.DateTime.Now.ToShortDateString();

                }
            }
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 12:00:00 AM";

        }
        catch
        {
            StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            EndDate = System.DateTime.Now.ToShortDateString();
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 12:00:00 AM";
        }

        if (segment == "NSE-CM" || segment == "BSE-CM")
        {

            for (int i = 0; i < mulcontactid.Length; i++)
            {
                strHtml = "";
                strHtml1 = "";

                DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[i] + "'");
                BranchID = DTBR.Rows[0][0].ToString();


                #region NSE-CM,BSE-CM
                DataSet ds = new DataSet();
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("[NetPositionCM]", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@fromdate", StartDate);
                        da.SelectCommand.Parameters.AddWithValue("@todate", EndDate);
                        da.SelectCommand.Parameters.AddWithValue("@SettNo", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@segment", SegmentID);
                        da.SelectCommand.Parameters.AddWithValue("@MasterSegment", UserSegID);
                        da.SelectCommand.Parameters.AddWithValue("@Companyid", company);
                        da.SelectCommand.Parameters.AddWithValue("@ClientsID", "'" + mulcontactid[i] + "'");
                        da.SelectCommand.Parameters.AddWithValue("@instrument", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@settype", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@Branch", BranchID);
                        da.SelectCommand.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
                        da.SelectCommand.Parameters.AddWithValue("@GRPID", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@openposition", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ChkCharge", "CHK");
                        da.SelectCommand.Parameters.AddWithValue("@Chksign", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@rptview", "0");
                        da.SelectCommand.Parameters.AddWithValue("@AmntGreaterThan", "0");
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        da.Dispose();
                        // ViewState["dataset"] = ds;
                      
                    }
                }

                flag = 0;
                string str = null;
                DataTable dt1 = ds.Tables[0];
                strMain = strMain + "<tr><td>" + dt1.Rows[0]["CLIENTNAME"].ToString() + "[" + dt1.Rows[0]["UCC"].ToString() + "]" + "</td></tr>";

                //strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                //strHtml1 += "<tr><td align=\"left\" colspan=\" 20\" style=\"color:Blue;\">" + str + "</td></tr></table>";
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Qty </b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Mkt Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Qty </b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Mkt Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>Qty</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>P/L</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Qty</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg</br>Dlv</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Amount</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ST</br>Tax</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Close</br>Price</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>MTM</b></td>";
                strHtml += "</tr>";



                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";

                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["BUYMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["DIFFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["DIFFPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["DLVQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["DLVVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["AVGDLV"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["AVGDLV"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["NETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["NETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["STTAX"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["STTAX"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["CLOSEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["CLOSEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total :</b></td>";

                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["BUYNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BUYNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SELLNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SELLNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["DIFFPL_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DIFFPL_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" colspan=1>&nbsp;</td>";
                if (dt1.Rows[0]["DLVVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DLVVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["NETAMNT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["NETAMNT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["STT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["STT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (dt1.Rows[0]["MTM_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["MTM_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
                //strHtml += "</table>";

                //strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" colspan=\" 20\"><b>Charges</b></td></tr>";

                ///////////STax On Brkg
                if (dt1.Rows[0]["BRKGCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["BRKGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["BRKGCHARGE_SUM"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Transaction Charges
                if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["TRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On Trn.Charges
                if (dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["SRVTAXTRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Stamp Duty
                if (dt1.Rows[0]["STAMPCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["STAMPCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["STAMPCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STT Tax
                if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["STTAX_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["STTAX_SUM"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////SEBI Fee
                if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SEBICHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////DELIVERY CHARGES
                if (dt1.Rows[0]["DELIVERYCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["DELIVERYCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["DELIVERYCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STTAX ON DELIVERY CHARGES
                if (dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["SRVTAXDELIVERYCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Round Off Adjustment
                if (dt1.Rows[0]["NETROUNDOFF_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["NETROUNDOFF_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\">" + dt1.Rows[0]["NETROUNDOFF"].ToString() + "</td>";
                    strHtml += "</tr>";
                }

                ///////////Net Total
                if (dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" ><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=\"20\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                }
                strHtml += "</table>";
                strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";
            }
            strMain = strMain + "</table>";
                #endregion
        }
        else if (segment == "NSE-FO")
        {
            #region NSE-FO

            string UNDERLYING = null;
            DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + UserSegID + "'", " Equity_ProductID");
            DataSet ds = new DataSet();
            if (dtunderlying.Rows.Count > 0)
            {
                for (int i = 0; i < dtunderlying.Rows.Count; i++)
                {
                    if (UNDERLYING == null)
                        UNDERLYING = dtunderlying.Rows[i][0].ToString();
                    else
                        UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                }
            }
            for (int i = 0; i < mulcontactid.Length; i++)
            {
                strHtml = "";
                strHtml1 = "";

                DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[i] + "'");
                BranchID = DTBR.Rows[0][0].ToString();

                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("[NetPositionFO]", con))
                    {

                        da.SelectCommand.Parameters.AddWithValue("@fromdate", StartDate);
                        da.SelectCommand.Parameters.AddWithValue("@todate", EndDate);
                        da.SelectCommand.Parameters.AddWithValue("@segment", SegmentID);
                        da.SelectCommand.Parameters.AddWithValue("@Finyear", FinYear);
                        da.SelectCommand.Parameters.AddWithValue("@MasterSegment", UserSegID);
                        da.SelectCommand.Parameters.AddWithValue("@Companyid", company);
                        da.SelectCommand.Parameters.AddWithValue("@ClientsID", "'" + mulcontactid[i] + "'");
                        da.SelectCommand.Parameters.AddWithValue("@UNDERLYING", UNDERLYING);
                        da.SelectCommand.Parameters.AddWithValue("@Expiry", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@Branch", BranchID);
                        da.SelectCommand.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
                        da.SelectCommand.Parameters.AddWithValue("@GRPID", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@OPENFUT", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@OPENOPT", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ChkCharge", "CHK");
                        da.SelectCommand.Parameters.AddWithValue("@Chksign", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@rptview", "0");
                        da.SelectCommand.Parameters.AddWithValue("@InstryType", "0");
                        da.SelectCommand.Parameters.AddWithValue("@ExposureBuyCall", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ExposureBuyPut", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ExposurePrice", "1");
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        da.Dispose();
                        // ViewState["dataset"] = ds;
                    }
                }
                DataTable dt1 = ds.Tables[0];

                flag = 0;
                string str = null;

                strMain = strMain + "<tr><td>" + dt1.Rows[0]["CLIENTNAME"].ToString() + "[" + dt1.Rows[0]["UCC"].ToString() + "]" + "</td></tr>";

                strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml1 += "<tr><td align=\"left\" colspan=11 style=\"color:Blue;\">" + str + "</td></tr></table>";
                /////////********FOR HEADER END


                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                ////HTML TABLE HEADER
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Expiry </br>Date</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Qty</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" ><b>Open </br>Price</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Buy</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Avg.</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Sell</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Avg.</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Qty</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Qty</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sett </br>Price</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Value</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Premium</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Mtm</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Future </br>FinSett</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Amount</b></td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net </br>Obligation</b></td>";
                strHtml += "</tr>";

                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";
                    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["EXPIRY"].ToString() + "</td>";
                    if (dt1.Rows[k]["BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F  Qty\">" + dt1.Rows[k]["BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  title=\"Open Price\">" + dt1.Rows[k]["BFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[k]["BFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Buy\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[k]["BUYVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["BUYAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[k]["BUYAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Sell\">" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Value\">" + dt1.Rows[k]["SELLVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["SELLAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[k]["SELLAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["EXCASNQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Qty\">" + dt1.Rows[k]["EXCASNQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["CFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Qty\">" + dt1.Rows[k]["CFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["CFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sett Price\">" + dt1.Rows[k]["CFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["CFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[k]["CFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["PRM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[k]["PRM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["FINSETT"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[k]["FINSETT"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["EXCASNVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[k]["EXCASNVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    if (dt1.Rows[k]["NETOBLIGATION"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[k]["NETOBLIGATION"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                //////////TOTAL DISPLAY
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"color:maroon;font-size:xx-small;\" colspan=2><b>Total :</b></td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["BFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[0]["BFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["BUYQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  nowrap=\"nowrap;\" title=\"Buy  Qty\">" + dt1.Rows[0]["BUYQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["BUYVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[0]["BUYVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["BUYAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[0]["BUYAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SELLQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell  Qty\">" + dt1.Rows[0]["SELLQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SELLVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Value\">" + dt1.Rows[0]["SELLVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SELLAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[0]["SELLAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["CFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[0]["CFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["PRMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[0]["PRMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["MTMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[0]["MTMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["FINSETTSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[0]["FINSETTSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["EXCASNVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[0]["EXCASNVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["NETOBLIGATIONSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[0]["NETOBLIGATIONSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
                strHtml += "</table>";

                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" colspan=\"20\"><b>Charges</b></td></tr>";

                ///////////STax On Brkg
                if (dt1.Rows[0]["BRKGCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["BRKGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BRKGCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Transaction Charges
                if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["TRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["TRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On Trn.Charges
                if (dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVTAXTRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Stamp Duty
                if (dt1.Rows[0]["STAMPCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["STAMPCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STAMPCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STT Tax
                if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["STTAX_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STTAX"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////SEBI Fee
                if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SEBICHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Net Total
                if (dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                }

                strHtml += "</table>";

                strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";

            }
            strMain = strMain + "</table>";
            #endregion

        }
        else if (segment == "ICEX-COMM" || segment == "MCX-COMM")
        {
            #region comCurrency

            for (int k = 0; k < mulcontactid.Length; k++)
            {
                strHtml = "";
                strHtml1 = "";

                DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[k] + "'");
                BranchID = DTBR.Rows[0][0].ToString();
                DataSet ds = new DataSet();
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("[NetPositionCommCurrency]", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@fromdate", StartDate);
                        da.SelectCommand.Parameters.AddWithValue("@todate", EndDate);
                        da.SelectCommand.Parameters.AddWithValue("@segment", SegmentID);
                        da.SelectCommand.Parameters.AddWithValue("@MasterSegment", UserSegID);
                        da.SelectCommand.Parameters.AddWithValue("@Companyid", company);
                        da.SelectCommand.Parameters.AddWithValue("@ClientsID", "'" + mulcontactid[k] + "'");
                        da.SelectCommand.Parameters.AddWithValue("@Instrument", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@UNDERLYING", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@Expiry", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@BranchHierchy", BranchID);
                        da.SelectCommand.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
                        da.SelectCommand.Parameters.AddWithValue("@GRPID", "ALL");
                        da.SelectCommand.Parameters.AddWithValue("@OPENFUT", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ChkCharge", "CHK");
                        da.SelectCommand.Parameters.AddWithValue("@Chksign", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@rptview", "0");
                        da.SelectCommand.Parameters.AddWithValue("@ExposureBuyCall", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ExposureBuyPut", "UNCHK");
                        da.SelectCommand.Parameters.AddWithValue("@ParameterFeild", "[BF Lots],[Open Price],[BF Value],[Buy Lots],[Buy Value],[Buy Avg.],[Sell Lots],[Sell Value],[Sell Avg.],[Asn/Exc Lot],[CF Lots],[Sett Price],[CF Value],[Mtm],[Premium],[Fin Sett],[Asn/Exc Value],[Net Obligation]");
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        da.Dispose();
                        // ViewState["dataset"] = ds;
                    }
                }

                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {

                    strMain = strMain + "<tr><td>" + dt.Rows[0]["Client Name"].ToString() + "</td></tr>";
                    dt.Columns.Remove("Client Name");
                    dt.Columns.Remove("TabNameOrder");
                    dt.Columns.Remove("Customerid");

                    strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



                    //////////////TABLE HEADER BIND
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                    }
                    strHtml += "</tr>";

                    flag = 0;
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (dr1[j].ToString().Trim().StartsWith("Net Total:") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("STax") || dr1[j].ToString().Trim().StartsWith("Transaction") || dr1[j].ToString().Trim().StartsWith("Stamp") || dr1[j].ToString().Trim().StartsWith("SEBI") || dr1[j].ToString().Trim().StartsWith("Overall") || dr1[j].ToString().Trim().StartsWith("Grand"))
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                }
                                else if (dr1[j].ToString().Trim().StartsWith("Test"))
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                                else if (IsNumeric(dr1[j].ToString()) == true)
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                    strHtml += "</table>";

                    strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";
                }
            }
            strMain = strMain + "</table>";

            #endregion

        }

        return strMain;


    }
    public static bool IsNumeric(object value)
    {
        double dbl;
        return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
    }

    public String GetCollateralReport(string contactid, string[] subject)
    {
        DataTable dtSeg = new DataTable();
        string SegmentID = string.Empty;
        String BranchID = string.Empty;
        String company = string.Empty;
        string FinYear = string.Empty;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string segment = string.Empty;
        string exch1 = string.Empty;
        string exch2 = string.Empty;
        int flag = 0;

        string strMain = "<table>";
        DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");
        FinYear = dTFY.Rows[0][0].ToString();
        StartDate = dTFY.Rows[0][1].ToString();
        EndDate = dTFY.Rows[0][2].ToString();
        string[] mulcontactid = contactid.Split('|');
        try
        {
            segment = subject[2].ToUpper();
            if (segment == "ICEX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "MCX-COMM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "NSE-FO")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "NSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            else if (segment == "BSE-CM")
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXB0000001' and exch_segmentid='CM' ");
                SegmentID = dtSeg.Rows[0][0].ToString();
                company = dtSeg.Rows[0][1].ToString();
            }
            StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            StartDate = StartDate + " 12:00:00 AM";
        }
        catch
        {
            StartDate = System.DateTime.Now.ToShortDateString().Split('/')[0] + "/" + "01" + "/" + System.DateTime.Now.ToShortDateString().Split('/')[2];
            EndDate = System.DateTime.Now.ToShortDateString();
            StartDate = StartDate + " 12:00:00 AM";
            EndDate = EndDate + " 12:00:00 AM";
        }

        for (int p = 0; p < mulcontactid.Length; p++)
        {


            if (segment == "ALL")
            {

                string[,] data = oDBEngine.GetFieldValue(" tbl_master_contactExchange ", " crg_exchange,crg_company ", " crg_cntid='" + mulcontactid[p] + "'", 2);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        if (exch1 == "")
                        {
                            exch1 = "'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 = "'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            company = data[i, 1];

                        }
                        else
                        {
                            exch1 += ",'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 += ",'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            //company += ",'" + data[i, 1].Trim() + "'";

                        }
                    }
                }
                data = oDBEngine.GetFieldValue("  (SELECT     exch_internalId, exch_compId,(SELECT exh_shortName  FROM  tbl_master_exchange  WHERE (exh_cntId = tbl_master_companyExchange.exch_exchId)) AS exch, exch_segmentId FROM   tbl_master_companyExchange  WHERE  (exch_compId ='" + company + "')) AS d ", "  exch_internalId ", "  (exch in (" + exch1 + ")) AND (exch_segmentId in (" + exch2 + "))", 1);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (SegmentID == "")
                            SegmentID = data[i, 0];
                        else
                            SegmentID += "," + data[i, 0];
                    }
                }

            }


            DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[p] + "'");
            BranchID = DTBR.Rows[0][0].ToString();
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("[Report_MarginStocksInwardOutwardRegister]", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ForDate", StartDate);
                    da.SelectCommand.Parameters.AddWithValue("@ClientId", "'" + mulcontactid[p] + "'");
                    da.SelectCommand.Parameters.AddWithValue("@BranchHierchy", BranchID);
                    da.SelectCommand.Parameters.AddWithValue("@GrpType", "BRANCH");
                    da.SelectCommand.Parameters.AddWithValue("@Grpid", "ALL");
                    da.SelectCommand.Parameters.AddWithValue("@Segment", SegmentID);
                    da.SelectCommand.Parameters.AddWithValue("@Companyid", company);
                    da.SelectCommand.Parameters.AddWithValue("@FinYear", FinYear);
                    da.SelectCommand.Parameters.AddWithValue("@Productid", "ALL");
                    da.SelectCommand.Parameters.AddWithValue("@Accountid", "ALL");
                    da.SelectCommand.Parameters.AddWithValue("@CheckDp", "Chk");
                    da.SelectCommand.Parameters.AddWithValue("@CheckPur", "Chk");
                    da.SelectCommand.Parameters.AddWithValue("@CheckSale", "Chk");
                    da.SelectCommand.Parameters.AddWithValue("@Chkledgerbaln", "Chk");
                    da.SelectCommand.Parameters.AddWithValue("@Chkcashmargin", "Chk");
                    da.SelectCommand.Parameters.AddWithValue("@ReportFor", "1");
                    da.SelectCommand.Parameters.AddWithValue("@PendingPurSalevalueMethod", "Close");
                    da.SelectCommand.Parameters.AddWithValue("@Stocknegative", "Chk");
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    
                }
            }


            String strHtmlheader = String.Empty;
            string str = null;
            //if (ddlGroup.SelectedItem.Value.ToString() == "2")
            //{
            //    str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            //}
            //str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            //str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            //DataView viewclient = new DataView();
            //viewclient = ds.Tables[0].DefaultView;
            //viewclient.RowFilter = "ClientId='" + Clientid.ToString().Trim() + "'";
            //DataTable dt = new DataTable();
            //dt = viewclient.ToTable();
            DataTable dt = ds.Tables[0];

            dt.Columns.Remove("ClientId");
            dt.Columns.Remove("Grpname");
            dt.Columns.Remove("GrpId");
            dt.Columns.Remove("GrpEmail");


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            dt.Rows[0].Delete();
            dt.AcceptChanges();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Net:") || dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";

            strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";


        }
        strMain = strMain + "</table>";
        return strMain;
    }

    public String GetNetWorth(string contactid, string[] subject)
    {
        DataTable dtSeg = new DataTable();
        string SegmentID = string.Empty;
        String BranchID = string.Empty;
        String company = string.Empty;
        string FinYear = string.Empty;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string segment = string.Empty;
        string exch1 = string.Empty;
        string exch2 = string.Empty;
        int flag = 0;

        string strMain = "<table>";
        DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");
        FinYear = dTFY.Rows[0][0].ToString();
        StartDate = dTFY.Rows[0][1].ToString();
        EndDate = dTFY.Rows[0][2].ToString();
        string[] mulcontactid = contactid.Split('|');

        segment = subject[2].ToUpper();
        if (segment == "ICEX-COMM")
        {
            dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXI0000001' ");
            SegmentID = dtSeg.Rows[0][0].ToString();
            company = dtSeg.Rows[0][1].ToString();
        }
        else if (segment == "MCX-COMM")
        {
            dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXM0000001' ");
            SegmentID = dtSeg.Rows[0][0].ToString();
            company = dtSeg.Rows[0][1].ToString();
        }
        else if (segment == "NSE-FO")
        {
            dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='FO' ");
            SegmentID = dtSeg.Rows[0][0].ToString();
            company = dtSeg.Rows[0][1].ToString();
        }
        else if (segment == "NSE-CM")
        {
            dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXN0000002' and exch_segmentid='CM' ");
            SegmentID = dtSeg.Rows[0][0].ToString();
            company = dtSeg.Rows[0][1].ToString();
        }
        else if (segment == "BSE-CM")
        {
            dtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange join tbl_master_contactexchange on tbl_master_companyexchange.exch_compid=tbl_master_contactexchange.crg_company", "top 1 exch_internalid,exch_compid", "exch_exchid='EXB0000001' and exch_segmentid='CM' ");
            SegmentID = dtSeg.Rows[0][0].ToString();
            company = dtSeg.Rows[0][1].ToString();
        }


        for (int p = 0; p < mulcontactid.Length; p++)
        {


            if (segment == "ALL")
            {

                string[,] data = oDBEngine.GetFieldValue(" tbl_master_contactExchange ", " crg_exchange,crg_company ", " crg_cntid='" + mulcontactid[p] + "'", 2);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        if (exch1 == "")
                        {
                            exch1 = "'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 = "'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            company = data[i, 1];

                        }
                        else
                        {
                            exch1 += ",'" + (data[i, 0].Split('-'))[0].Trim() + "'";
                            exch2 += ",'" + (data[i, 0].Split('-'))[1].Trim() + "'";
                            //company += ",'" + data[i, 1].Trim() + "'";

                        }
                    }
                }
                data = oDBEngine.GetFieldValue("  (SELECT     exch_internalId, exch_compId,(SELECT exh_shortName  FROM  tbl_master_exchange  WHERE (exh_cntId = tbl_master_companyExchange.exch_exchId)) AS exch, exch_segmentId FROM   tbl_master_companyExchange  WHERE  (exch_compId ='" + company + "')) AS d ", "  exch_internalId ", "  (exch in (" + exch1 + ")) AND (exch_segmentId in (" + exch2 + "))", 1);
                if (data[0, 0] != "n")
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (SegmentID == "")
                            SegmentID = data[i, 0];
                        else
                            SegmentID += "," + data[i, 0];
                    }
                }

            }


            DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[p] + "'");
            BranchID = DTBR.Rows[0][0].ToString();
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("[Report_ClientRisk]", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ClientId", "'" + mulcontactid[p] + "'");
                    da.SelectCommand.Parameters.AddWithValue("@BranchHierchy", BranchID);
                    da.SelectCommand.Parameters.AddWithValue("@GrpType", "BRANCH");
                    da.SelectCommand.Parameters.AddWithValue("@Grpid", "ALL");
                    da.SelectCommand.Parameters.AddWithValue("@Segment", SegmentID);
                    da.SelectCommand.Parameters.AddWithValue("@Companyid", company);
                    da.SelectCommand.Parameters.AddWithValue("@FinYear", FinYear);
                    da.SelectCommand.Parameters.AddWithValue("@RptType", "Detail");
                    da.SelectCommand.Parameters.AddWithValue("@ApplyHaircut", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@PendingPurSalevalueMethod", "Close");
                    da.SelectCommand.Parameters.AddWithValue("@OnlyShortageClient", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@ShortageMoreThanType", "Value");
                    da.SelectCommand.Parameters.AddWithValue("@ShortageMoreThan", "0");
                    da.SelectCommand.Parameters.AddWithValue("@ColumnMrgnHldbkDpStocks", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@ColumnPndgSales", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@ColumnPndgPur", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@ColumnAppMargin", "Y");
                    da.SelectCommand.Parameters.AddWithValue("@ColumnShortExcess", "Y");
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                   
                }
            }





            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.Columns.Remove("GrpName");
            int b = dt.Rows.Count;
            dt.Rows[b - 1].Delete();
            dt.Rows[b - 2].Delete();
            dt.Rows[0].Delete();
            dt.AcceptChanges();
            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToString().Trim() != "SegId" && dt.Columns[i].ColumnName.ToString().Trim() != "Customerid")
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString().Trim() != "SegId" && dt.Columns[j].ColumnName.ToString().Trim() != "Customerid")
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            if (dr1[j].ToString().Trim().StartsWith("Group/Branch") || dr1[j].ToString().Trim().StartsWith("Total:"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                            }
                            else if (dr1[j].ToString().Trim().StartsWith("Test"))
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Unclrd Balnc")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','UnClear'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Mrgn/Hldbk/DP")
                            {
                                //if (DdlrptView.SelectedItem.Value.ToString().Trim() == "Detail")
                                //{
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:" && dt.Rows[flag - 1]["Segment"].ToString().Trim() != "NSDL" && dt.Rows[flag - 1]["Segment"].ToString().Trim() != "CDSL")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','MarginHldbk'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else if (dt.Rows[flag - 1]["Segment"].ToString().Trim() == "NSDL" || dt.Rows[flag - 1]["Segment"].ToString().Trim() == "CDSL")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','DP'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                }
                                //}
                                //else
                                //{
                                //    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                //}
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Pndg.Pur")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','PendgPur'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Pndg.Sales")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','PendgSale'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";

            strMain = strMain + "<tr><td>" + strHtml + "</td></tr>";


        }
        strMain = strMain + "</table>";
        return strMain;

    }

    public string GetPendingDelivery(string contactid, string[] subject)
    {

        string strMain = "<table>";
        string[] mulcontactid = contactid.Split('|');
        for (int p = 0; p < mulcontactid.Length; p++)
        {

            DataTable DTBR = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "CNT_BRANCHID", "CNT_INTERNALID='" + mulcontactid[p] + "'");
            string BranchID = DTBR.Rows[0][0].ToString();

            DataTable dTFY = oDBEngine.GetDataTable("MASTER_FINYEAR ", "  FINYEAR_CODE,FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " GETDATE() BETWEEN  FINYEAR_STARTDATE AND FINYEAR_ENDDATE ");
            string FinYear = dTFY.Rows[0][0].ToString();

            DataTable dtComp = oDBEngine.GetDataTable("TBL_MASTER_CONTACTEXCHANGE", " CRG_COMPANY ", "CRG_CNTID='" + mulcontactid[p] + "'");
            DataSet ds = new DataSet();
            //using (SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da1 = new SqlDataAdapter("[FETCH_DELIVERYPOSITION]", con1))
                {
                    da1.SelectCommand.Parameters.AddWithValue("@SettSelection", "A");
                    da1.SelectCommand.Parameters.AddWithValue("@SettlementN", "");
                    da1.SelectCommand.Parameters.AddWithValue("@SettlementT", "");
                    da1.SelectCommand.Parameters.AddWithValue("@Clients", "'" + mulcontactid[p] + "'");
                    da1.SelectCommand.Parameters.AddWithValue("@ReportType", "C");
                    da1.SelectCommand.Parameters.AddWithValue("@Scrips", "");
                    da1.SelectCommand.Parameters.AddWithValue("@TransferredType", "P");
                    da1.SelectCommand.Parameters.AddWithValue("@PositionType", "B");
                    da1.SelectCommand.Parameters.AddWithValue("@OrderBy", "C");
                    da1.SelectCommand.Parameters.AddWithValue("@BranchHr", BranchID);
                    da1.SelectCommand.Parameters.AddWithValue("@Company", dtComp.Rows[0][0].ToString());
                    da1.SelectCommand.Parameters.AddWithValue("@UserSegId", "24");
                    da1.SelectCommand.Parameters.AddWithValue("@BranchGroupType", "B");
                    da1.SelectCommand.Parameters.AddWithValue("@GroupType", "");
                    da1.SelectCommand.Parameters.AddWithValue("@Group", "");
                    da1.SelectCommand.Parameters.AddWithValue("@FinYear", FinYear);
                    da1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da1.SelectCommand.CommandTimeout = 0;
                    if (con1.State == ConnectionState.Closed)
                        con1.Open();
                    ds.Reset();
                    da1.Fill(ds);
                    con1.Close();
                }

            }
            DataTable DtDeliverPosition = ds.Tables[0];
            string TABLEPOA = oconverter.ConvertDataTableToXML(DtDeliverPosition);/////////////////DATATABLE TO CONVERT XML DATA
            //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"]; MUlTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
              SqlConnection con = new SqlConnection(conn);
            SqlCommand com = new SqlCommand("[SP_POAACHolding]", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ALLPOADATA", TABLEPOA);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = com;
            DataSet ds1 = new DataSet();
            com.CommandTimeout = 0;
            ds1.Reset();
            da.Fill(ds1);
            da.Dispose();
            DtDeliverPosition.Clear();
            DtDeliverPosition.Dispose();
            DtDeliverPosition = new DataTable();
            DtDeliverPosition = ds1.Tables[0].Copy();

            #region clients

            string strHtmlAllClient = "";
            string MainTblEmail = "";
            string TempHtml = "";

            strHtmlAllClient += "<table border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "  width=\"100%\" style=\"border:solid 1px blue\">";
            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";

            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Scrip</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">ISIN</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">SETTLEMENT</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Qty To Rec</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Qty Rcvd</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Qty To Dlvr</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Qty Dlvrd</td>";
            strHtmlAllClient += "<td align=\"center\" rowspan=\"2\">Pending Incoming</td>";
            strHtmlAllClient += "<td align=\"center\" rowspan=\"2\">Pending Outgoing</td>";
            strHtmlAllClient += "<td align=\"center\" colspan=\"3\">Stock Position</td>";
            strHtmlAllClient += "<td align=\"center\"  rowspan=\"2\">Final Shortage</td></tr>";
            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
            strHtmlAllClient += "<td align=\"center\">Inter Sett</td>";
            strHtmlAllClient += "<td align=\"center\">Mrgn/Hldbk</td>";
            strHtmlAllClient += "<td align=\"center\">POA A/C Holding</td></tr>";
            MainTblEmail = strHtmlAllClient;
            for (int i = 0; i < DtDeliverPosition.Rows.Count; i++)
            {
                if (i == 0)
                {
                    TempHtml += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                    TempHtml += "<td colspan=\"13\" align=\"left\" style=\"font-size:11px\">" + DtDeliverPosition.Rows[i]["Client"].ToString() + "  [" + DtDeliverPosition.Rows[i]["UCC"].ToString() + "]       Branch :" + DtDeliverPosition.Rows[i]["BranchName"].ToString() + " [" + DtDeliverPosition.Rows[i]["Branch"].ToString() + "]</td></tr>";

                }

                TempHtml += "<tr style=\"background-color:white;text-align:center\">";
                TempHtml += "<td align=\"left\" style=\"font-size:xx-small\">" + DtDeliverPosition.Rows[i]["Scrip"].ToString() + "</td>";
                TempHtml += "<td align=\"left\" style=\"font-size:xx-small\">" + DtDeliverPosition.Rows[i]["DematPosition_ISIN"].ToString() + "</td>";
                TempHtml += "<td align=\"left\" style=\"font-size:xx-small\">" + DtDeliverPosition.Rows[i]["Settlement"].ToString() + "</td>";
                if (DtDeliverPosition.Rows[i]["DematPosition_QtyToReceive"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["DematPosition_QtyToReceive"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";

                if (DtDeliverPosition.Rows[i]["DematPosition_QtyReceived"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["DematPosition_QtyReceived"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";

                if (DtDeliverPosition.Rows[i]["DematPosition_QtyToDeliver"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["DematPosition_QtyToDeliver"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";

                if (DtDeliverPosition.Rows[i]["DematPosition_QtyDelivered"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["DematPosition_QtyDelivered"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";


                if (DtDeliverPosition.Rows[i]["IncomingPending"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["IncomingPending"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";
                if (DtDeliverPosition.Rows[i]["OutgoingPending"] != DBNull.Value)
                    TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["OutgoingPending"])) + "</td>";
                else
                    TempHtml += "<td>&nbsp;</td>";
                if (DtDeliverPosition.Rows[i]["InterPosition"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(DtDeliverPosition.Rows[i]["InterPosition"].ToString()) != 0)
                        TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["InterPosition"])) + "</td>";
                    else
                        TempHtml += "<td>&nbsp;</td>";

                }
                else
                {
                    TempHtml += "<td>&nbsp;</td>";
                }

                if (DtDeliverPosition.Rows[i]["MarginPosotion"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(DtDeliverPosition.Rows[i]["MarginPosotion"].ToString()) != 0)
                        TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["MarginPosotion"])) + "</td>";
                    else
                        TempHtml += "<td>&nbsp;</td>";
                }
                else
                {
                    TempHtml += "<td>&nbsp;</td>";
                }
                if (DtDeliverPosition.Rows[i]["POADP"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(DtDeliverPosition.Rows[i]["POADP"].ToString()) != 0)
                        TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliverPosition.Rows[i]["POADP"])) + "</td>";
                    else
                        TempHtml += "<td>&nbsp;</td>";
                }
                else
                {
                    TempHtml += "<td>&nbsp;</td>";
                }
                if (DtDeliverPosition.Rows[i]["Shortage"] != DBNull.Value)
                {
                    if (DtDeliverPosition.Rows[i]["IncomingPending"] != DBNull.Value)
                    {
                        decimal FinalShortage = 0;
                        decimal incpending = Convert.ToDecimal(DtDeliverPosition.Rows[i]["IncomingPending"].ToString());
                        decimal shortage = Convert.ToDecimal(DtDeliverPosition.Rows[i]["Shortage"].ToString());
                        if (incpending <= shortage)
                        {
                            TempHtml += "<td>&nbsp;</td></tr>";
                        }
                        else
                        {
                            FinalShortage = incpending - shortage;
                            TempHtml += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(FinalShortage) + "</td></tr>";
                        }
                    }
                    else
                    {
                        TempHtml += "<td>&nbsp;</td></tr>";
                    }

                }
                else
                {
                    TempHtml += "<td>&nbsp;</td></tr>";
                }
                strHtmlAllClient += TempHtml;
                TempHtml = "";
            }
            strHtmlAllClient += "</table>";
            strMain = strMain + "<tr><td>" + strHtmlAllClient + "</td></tr>";
        }
            #endregion
        strMain = strMain + "</table>";
        return strMain;
    }


    
}
//SqlCommand cmd1 = new SqlCommand("ledger_html", con);
//cmd1.CommandType = CommandType.StoredProcedure;
//cmd1.Parameters.AddWithValue("@clients_internalId", contactidnew[j]);
//cmd1.Parameters.AddWithValue("@segment", segment);
//cmd1.Parameters.AddWithValue("@fromdate", fromdate);
//cmd1.Parameters.AddWithValue("@todate", todate);
//cmd1.CommandTimeout = 0;
//SqlDataAdapter da1 = new SqlDataAdapter();
//da1.SelectCommand = cmd1;
//da1.Fill(NSE);
//#region ledger
//if (NSE.Tables[0].Rows.Count > 0)
//{


//    DataView HNSEDV = new DataView(NSE.Tables[0]);
//    DataTable FilteredNSE = new DataTable();
//    DataTable DistinctAccount = HNSEDV.ToTable(true, new string[] { "AccountsLedger_SubAccountID" });

//    foreach (DataRow dr in DistinctAccount.Rows)
//    {

//        HNSEDV.RowFilter = "AccountsLedger_SubAccountID = '" + dr["AccountsLedger_SubAccountID"] + "'";
//        FilteredNSE = HNSEDV.ToTable();
//        if (FilteredNSE.Rows.Count > 0)
//        {
//            string subject1 = " ";
//            for (int a = 0; a < subject.Length; a++)
//            {
//                subject1 = subject1 + " " + subject[a];
//            }
//            subject1 = subject1.Trim();
//            str += generateMailHoldingBodyledger(FilteredNSE, dt, subject1, NSE.Tables[1], segment);
//        }
//        FilteredNSE.Reset();



//    }

//}

//if (str == string.Empty)
//{
//    str = "No Record Found";
//}
//str += "<br/>";
//NSE.Reset();

//# endregion ledger


