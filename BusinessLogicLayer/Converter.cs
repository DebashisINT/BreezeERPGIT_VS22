using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using DataAccessLayer;
using Ionic.Zip;
using System.Globalization;

namespace BusinessLogicLayer
{
    public class Converter
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ProcedureExecute proc = new ProcedureExecute();
        DBEngine oDBEngine = new DBEngine();
        public Converter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region converting date to mm/dd/yyyy,mm/dd/yyyy hh:mm format from dd/mm/yyyy hh:mm
        public string DateConverter1(string date,
                                   string format   // This will have format of date
                                  )
        {
            if (date != "")
            {
                string[] date1 = date.Split('-');
                string dd = date1.GetValue(0).ToString();
                if (dd.Length == 1)
                    dd = "0" + dd;
                string mm = date1.GetValue(1).ToString();
                if (mm.Length == 1)
                    mm = "0" + mm;
                string yy = (date1.GetValue(2).ToString()).Substring(0, 4);
                string hm = "00:00";
                string Time = "";
                if ((date1.GetValue(2).ToString()).Length > 4)
                {
                    string lengthg = (date1.GetValue(2).ToString()).Substring(5);
                    string[] hh_mm = lengthg.Split(':');
                    string hh = hh_mm.GetValue(0).ToString();
                    if (hh.Length == 1)
                    {
                        hh = "0" + hh;
                    }
                    string mnt = hh_mm.GetValue(1).ToString();
                    if (mnt.Length == 1)
                    {
                        mnt = "0" + mnt;
                    }

                    hm = hh + ":" + mnt;
                }
                if (format == "mm/dd/yyyy hh:mm" || format == "MM/dd/yyyy hh:mm")
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm;
                }
                else if (format == "mm/dd/yyyy" || format == "MM/dd/yyyy")
                {
                    Time = mm + "/" + dd + "/" + yy;
                }
                else if (format == "dd/mm/yyyy" || format == "dd/MM/yyyy")
                {
                    Time = dd + "/" + mm + "/" + yy;
                }
                else
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm;
                }
                return Time;

            }
            else
            {
                string tm = "";
                return tm;
            }
        }
        public string DateConverter(string date,
                                    string format   // This will have format of date
                                   )
        {
            if (date != "")
            {
                string[] date1 = date.Split('/');
                string dd = date1.GetValue(0).ToString();
                if (dd.Length == 1)
                    dd = "0" + dd;
                string mm = date1.GetValue(1).ToString();
                if (mm.Length == 1)
                    mm = "0" + mm;
                string yy = (date1.GetValue(2).ToString()).Substring(0, 4);
                string hm = "00:00";
                string Time = "";
                if ((date1.GetValue(2).ToString()).Length > 4)
                {
                    string lengthg = (date1.GetValue(2).ToString()).Substring(5);
                    string[] hh_mm = lengthg.Split(':');
                    string hh = hh_mm.GetValue(0).ToString();
                    if (hh.Length == 1)
                    {
                        hh = "0" + hh;
                    }
                    string mnt = hh_mm.GetValue(1).ToString();
                    if (mnt.Length == 1)
                    {
                        mnt = "0" + mnt;
                    }

                    hm = hh + ":" + mnt;
                }
                if (format == "mm/dd/yyyy hh:mm" || format == "MM/dd/yyyy hh:mm")
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm;
                }
                else if (format == "mm/dd/yyyy" || format == "MM/dd/yyyy")
                {
                    Time = mm + "/" + dd + "/" + yy;
                }
                else if (format == "dd/mm/yyyy" || format == "dd/MM/yyyy")
                {
                    Time = dd + "/" + mm + "/" + yy;
                }
                else
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm;
                }
                return Time;

            }
            else
            {
                string tm = "";
                return tm;
            }
        }
        public string DateConverterFromMMtoDD(string date,
                                    string format   // This will have format of date
                                   )
        {
            if (date != "")
            {
                string am_pm = "";
                string[] date1 = date.Split('/');
                string mm = date1.GetValue(0).ToString();
                if (mm.Length == 1)
                    mm = "0" + mm;
                string dd = date1.GetValue(1).ToString();
                if (dd.Length == 1)
                    dd = "0" + dd;
                string yy = (date1.GetValue(2).ToString()).Substring(0, 4);
                string hm = "00:00";
                string Time = "";
                if ((date1.GetValue(2).ToString()).Length > 4)
                {
                    string lengthg = (date1.GetValue(2).ToString()).Substring(5);
                    string[] hh_mm = lengthg.Split(':');
                    string hh = hh_mm.GetValue(0).ToString();
                    am_pm = hh_mm.GetValue(2).ToString();
                    am_pm = am_pm.Substring(3);
                    if (Convert.ToInt32(hh) >= Convert.ToInt32("24"))
                    {
                        hh = "23";
                    }
                    if (hh.Length == 1)
                    {
                        hh = "0" + hh;
                    }
                    string mnt = hh_mm.GetValue(1).ToString();
                    if (mnt.Length == 1)
                    {
                        mnt = "0" + mnt;
                    }

                    hm = hh + ":" + mnt;
                }
                if (format == "mm/dd/yyyy hh:mm" || format == "MM/dd/yyyy hh:mm")
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm + " " + am_pm;
                }
                else if (format == "mm/dd/yyyy" || format == "MM/dd/yyyy")
                {
                    Time = mm + "/" + dd + "/" + yy;
                }
                else
                {
                    Time = mm + "/" + dd + "/" + yy + " " + hm;
                }
                return Time;

            }
            else
            {
                string tm = "";
                return tm;
            }
        }
        #endregion
        #region converting date to dd/mm/yyyy hh:mm from mm/dd/yyyy hh:mm
        public string DateConverter_d_m_y(string date)
        {
            if (date != "")
            {
                string[] indmm = date.Split('/');
                string mm = indmm.GetValue(0).ToString();
                if (mm.Length == 1)
                    mm = "0" + mm;
                string dd = indmm.GetValue(1).ToString();
                if (dd.Length == 1)
                    dd = "0" + dd;
                string yy = (indmm.GetValue(2).ToString()).Substring(0, 4);
                string Time = "";
                int lengthg = date.Length;

                if (lengthg > 19)
                {
                    string last = date.Substring(lengthg - 2);

                    if (last == "AM")
                    {
                        string time = (indmm.GetValue(2).ToString()).Substring(5, (indmm.GetValue(2).ToString()).Length - 5);
                        string[] hh_mm = time.Split(':');
                        string h1 = hh_mm[0];
                        if (hh_mm[0].Length == 1)
                            h1 = "0" + hh_mm[0].ToString();
                        string hm = h1 + ":" + hh_mm[1];// +":" + hh_mm[2]; //(indmm.GetValue(2).ToString()).Substring(4, indmm.GetValue(2).ToString().Length - 10);

                        Time = dd + "/" + mm + "/" + yy + " " + hm;

                    }
                    else if (last == "PM")
                    {

                        string time = (indmm.GetValue(2).ToString()).Substring(5, (indmm.GetValue(2).ToString()).Length - 5);
                        string[] hh_mm = time.Split(':');
                        if (int.Parse(hh_mm.GetValue(0).ToString()) < 12)
                        {
                            int hh = 12 + int.Parse(hh_mm.GetValue(0).ToString());
                            int mm1 = int.Parse(hh_mm.GetValue(1).ToString());
                            Time = dd + "/" + mm + "/" + yy + " " + hh + ":" + mm1;
                        }
                        else
                        {
                            Time = dd + "/" + mm + "/" + yy + " " + hh_mm[0] + ":" + hh_mm[1];
                        }

                    }
                }
                else
                {
                    Time = indmm[1] + "/" + indmm[0] + "/" + indmm[2];
                }

                return Time;
            }
            else
            {
                string tm = "";
                return tm;
            }
        }
        public string DateConverter_d_m_y(string date, string format)
        {
            if (date != "")
            {
                string[] indmm = date.Split('/');
                string mm = indmm.GetValue(0).ToString();
                if (mm.Length == 1)
                    mm = "0" + mm;
                string dd = indmm.GetValue(1).ToString();
                if (dd.Length == 1)
                    dd = "0" + dd;
                string yy = (indmm.GetValue(2).ToString()).Substring(0, 4);
                string Time = "";
                int lengthg = date.Length;
                string last = date.Substring(lengthg - 2);
                if (format == "dd/mm/yyyy")
                {
                    Time = dd + "/" + mm + "/" + yy;
                }
                if (format == "mm/DD/yyyy")
                {
                    Time = mm + "/" + dd + "/" + yy;
                }
                if (format == "mm/dd/yyyy")
                {
                    Time = mm + "/" + dd + "/" + yy;
                }
                else
                {
                    if (last == "AM")
                    {
                        string hm = (indmm.GetValue(2).ToString()).Substring(5, indmm.GetValue(2).ToString().Length - 10);
                        string[] hm1 = hm.Split(':');
                        string hh = hm1[0].ToString();
                        string m = hm1[1].ToString();
                        if (hm1[0].ToString().Length == 1)
                            hh = "0" + hm1[0];
                        if (hm1[1].ToString().Length == 1)
                            m = "0" + hm1[1];
                        Time = dd + "/" + mm + "/" + yy + " " + hh + ":" + m;

                    }
                    else if (last == "PM")
                    {

                        string time = (indmm.GetValue(2).ToString()).Substring(5, (indmm.GetValue(2).ToString()).Length - 5);
                        //string[] time = time1.Split(':'); 
                        string[] hh_mm = time.Split(':');
                        if (int.Parse(hh_mm.GetValue(0).ToString()) < 12)
                        {
                            int hh = 12 + int.Parse(hh_mm.GetValue(0).ToString());
                            int mm1 = int.Parse(hh_mm.GetValue(1).ToString());
                            Time = dd + "/" + mm + "/" + yy + " " + hh + ":" + mm1;
                        }
                        else
                        {
                            Time = dd + "/" + mm + "/" + yy + " " + hh_mm[0] + ":" + hh_mm[1];
                        }

                    }
                }
                return Time;
            }
            else
            {
                string tm = "";
                return tm;
            }
        }
        #endregion


        public static void PrintWebControl(Control ctrl, string Script)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            if (ctrl is WebControl)
            {
                Unit w = new Unit(70, UnitType.Percentage);
                ((WebControl)ctrl).Width = w;
            }
            Page pg = new Page();
            if (Script != string.Empty)
            {
                pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
            }
            HtmlForm frm = new HtmlForm();
            pg.Controls.Add(frm);
            frm.Attributes.Add("runat", "server");
            frm.Controls.Add(ctrl);
            pg.RenderControl(htmlWriter);
            string strHTML = stringWriter.ToString();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strHTML);
            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
        }
        public static void PrintWebControl(Control ctrl)
        {
            PrintWebControl(ctrl, string.Empty);
        }

        //returns '2 aug 2009'
        public string ArrangeDate(string dDate)
        {
            if (dDate.ToString() != "")
            {
                string[] st = dDate.Split(' ');
                string tim = "";
                if (st.Length > 2)
                    tim = " " + st[1] + " " + st[2];
                string sDay = "";
                string sMonth = "";
                string sYear = "";
                DateTime dDate1 = Convert.ToDateTime(dDate.ToString());
                sDay = Convert.ToString(dDate1.Day);
                sMonth = Convert.ToString(dDate1.Month);
                sYear = Convert.ToString(dDate1.Year);
                switch (sMonth)
                {
                    case "1":
                        sMonth = "Jan";
                        break;
                    case "2":
                        sMonth = "Feb";
                        break;
                    case "3":
                        sMonth = "Mar";
                        break;
                    case "4":
                        sMonth = "Apr";
                        break;
                    case "5":
                        sMonth = "May";
                        break;
                    case "6":
                        sMonth = "Jun";
                        break;
                    case "7":
                        sMonth = "Jul";
                        break;
                    case "8":
                        sMonth = "Aug";
                        break;
                    case "9":
                        sMonth = "Sep";
                        break;
                    case "10":
                        sMonth = "Oct";
                        break;
                    case "11":
                        sMonth = "Nov";
                        break;
                    case "12":
                        sMonth = "Dec";
                        break;
                }
                dDate = sDay + " " + sMonth + " " + sYear;
                dDate = dDate + tim;
                return dDate;
            }
            else
                return dDate;
        }
        //returns '02 aug 2009'
        public string ArrangeDate1(string dDate)
        {
            if (dDate.ToString() != "")
            {
                string[] st = dDate.Split(' ');
                string tim = "";
                if (st.Length > 2)
                    tim = " " + st[1] + " " + st[2];
                string sDay = "";
                string sMonth = "";
                string sYear = "";
                DateTime dDate1 = Convert.ToDateTime(dDate.ToString());
                sDay = Convert.ToString(dDate1.Day);
                sMonth = Convert.ToString(dDate1.Month);
                sYear = Convert.ToString(dDate1.Year);
                switch (sMonth)
                {
                    case "1":
                        sMonth = "Jan";
                        break;
                    case "2":
                        sMonth = "Feb";
                        break;
                    case "3":
                        sMonth = "Mar";
                        break;
                    case "4":
                        sMonth = "Apr";
                        break;
                    case "5":
                        sMonth = "May";
                        break;
                    case "6":
                        sMonth = "Jun";
                        break;
                    case "7":
                        sMonth = "Jul";
                        break;
                    case "8":
                        sMonth = "Aug";
                        break;
                    case "9":
                        sMonth = "Sep";
                        break;
                    case "10":
                        sMonth = "Oct";
                        break;
                    case "11":
                        sMonth = "Nov";
                        break;
                    case "12":
                        sMonth = "Dec";
                        break;
                }
                if (int.Parse(sDay.Trim()) < 10)
                    sDay = "0" + sDay.Trim();
                dDate = sDay + " " + sMonth + " " + sYear;
                dDate = dDate + tim;
                return dDate;
            }
            else
                return dDate;
        }
        public string ArrangeDate2(string dDate)
        {
            if (dDate.ToString() != "")
            {
                string[] st = dDate.Split(' ');
                string tim = "";
                if (st.Length > 2)
                    tim = " " + st[1] + " " + st[2];
                string sDay = "";
                string sMonth = "";
                string sYear = "";
                DateTime dDate1 = Convert.ToDateTime(dDate.ToString());
                sDay = Convert.ToString(dDate1.Day);
                sMonth = Convert.ToString(dDate1.Month);
                sYear = Convert.ToString(dDate1.Year);
                switch (sMonth)
                {
                    case "1":
                        sMonth = "Jan";
                        break;
                    case "2":
                        sMonth = "Feb";
                        break;
                    case "3":
                        sMonth = "Mar";
                        break;
                    case "4":
                        sMonth = "Apr";
                        break;
                    case "5":
                        sMonth = "May";
                        break;
                    case "6":
                        sMonth = "Jun";
                        break;
                    case "7":
                        sMonth = "Jul";
                        break;
                    case "8":
                        sMonth = "Aug";
                        break;
                    case "9":
                        sMonth = "Sep";
                        break;
                    case "10":
                        sMonth = "Oct";
                        break;
                    case "11":
                        sMonth = "Nov";
                        break;
                    case "12":
                        sMonth = "Dec";
                        break;
                }
                if (int.Parse(sDay.Trim()) < 10)
                    sDay = "0" + sDay.Trim();
                dDate = sDay + " " + sMonth + " " + sYear;
                return dDate;
            }
            else
                return dDate;
        }
        public string ArrangeDate2(string dDate, string Format)
        {
            if (dDate.ToString() != "")
            {
                string[] st = dDate.Split(' ');
                string tim = "";
                if (st.Length > 2)
                    tim = " " + st[1] + " " + st[2];
                string sDay = "";
                string sMonth = "";
                string sYear = "";
                DateTime dDate1 = Convert.ToDateTime(dDate.ToString());
                sDay = Convert.ToString(dDate1.Day);
                sMonth = Convert.ToString(dDate1.Month);
                sYear = Convert.ToString(dDate1.Year);
                switch (sMonth)
                {
                    case "1":
                        sMonth = "Jan";
                        break;
                    case "2":
                        sMonth = "Feb";
                        break;
                    case "3":
                        sMonth = "Mar";
                        break;
                    case "4":
                        sMonth = "Apr";
                        break;
                    case "5":
                        sMonth = "May";
                        break;
                    case "6":
                        sMonth = "Jun";
                        break;
                    case "7":
                        sMonth = "Jul";
                        break;
                    case "8":
                        sMonth = "Aug";
                        break;
                    case "9":
                        sMonth = "Sep";
                        break;
                    case "10":
                        sMonth = "Oct";
                        break;
                    case "11":
                        sMonth = "Nov";
                        break;
                    case "12":
                        sMonth = "Dec";
                        break;
                }
                if (int.Parse(sDay.Trim()) < 10)
                    sDay = "0" + sDay.Trim();
                dDate = sDay + " " + sMonth + " " + sYear + " " + st[1] + " " + st[2];
                return dDate;
            }
            else
                return dDate;
        }
        public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox)
        {
            ComboBox.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                for (int i = 0; i < length1; i++)
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                }
            }
        }
        public string[,] ReservedWord_recipient()
        {
            string[,] recipient = new string[3, 2];
            recipient[0, 0] = "Recipient Name";
            recipient[0, 1] = "IsNull(cnt_firstname,'')+' '+IsNull(cnt_middlename,'')+' '+IsNull(cnt_lastname,'')|tbl_master_contact|cnt_internalid ";
            recipient[1, 0] = "Recipient Address";
            recipient[1, 1] = "Top 1 IsNull(add_address1,'')+' '+IsNull(add_address2,'')+' '+IsNull(add_address3,'')|tbl_master_address|add_cntid ";
            recipient[2, 0] = "Recipient Number";
            recipient[2, 1] = " Top 1 phf_phoneNumber|tbl_master_phonefax|(tbl_master_phonefax.phf_type = 'Official') and phf_cntid";

            return recipient;
        }
        public string[,] ReservedWord_sender()
        {
            string[,] sender = new string[5, 2];
            sender[0, 0] = "Sender Name";
            sender[0, 1] = " IsNull(cnt_firstname,'')+' '+IsNull(cnt_middlename,'')+' '+IsNull(cnt_lastname,'')|tbl_master_user INNER JOIN  tbl_master_contact ON tbl_master_user.user_contactId = tbl_master_contact.cnt_internalId|user_id ";
            sender[1, 0] = "Sender Department";
            sender[1, 1] = " TOP 1 tbl_master_costCenter.cost_description |tbl_master_user INNER JOIN tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id|user_id";
            sender[2, 0] = "Sender Number";
            sender[2, 1] = "Top 1 phf_phoneNumber|tbl_master_user INNER JOIN  tbl_master_phonefax ON tbl_master_user.user_contactId = tbl_master_phonefax.phf_cntId | (tbl_master_phonefax.phf_type = 'Official') and  user_id";
            sender[3, 0] = "Sender Email";
            sender[3, 1] = " Top 1 phf_Phonenuber|tbl_master_user INNER JOIN tbl_master_email ON tbl_master_user.user_contactId = tbl_master_email.eml_cntId |eml_type='Official' and user_id";
            sender[4, 0] = "Sender Branch";
            sender[4, 1] = "Top 1 tbl_master_branch.branch_description|tbl_master_user INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id | user_id ";

            return sender;
        }
        public void SendMail(string From, string To, string Subject, string Body)
        {
            MailMessage Message = new MailMessage();
            Message.To = To;
            Message.From = From;
            Message.Subject = Subject;
            Message.BodyFormat = MailFormat.Html;
            Message.Body = Body;
            SmtpMail.Send(Message);
        }

        public string SendMailHtmlBody(string From, string To, string Subject, string Body)
        {

            string rtn = "";
            System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
            Message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["CredentialUserName"].ToString());
            Message.To.Add(new System.Net.Mail.MailAddress(To));
            Message.Subject = Subject;
            Message.IsBodyHtml = true;
            Message.Body = Body;
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                client.EnableSsl = true;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["CredentialUserName"].ToString()
                                                                       , ConfigurationManager.AppSettings["CredentialPassword"].ToString());
                client.Send(Message);
            }
            catch (Exception ex)
            {
                rtn = ex.Message;
            }
            return rtn;
        }

        public string getFormattedvalue(decimal Value)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;

            if (Value < 0)
                Value = Value * -1;
            return Value.ToString("c", currencyFormat);

        }
        public string getFormattedvaluewithoriginalsign(decimal Value)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            return Value.ToString("c", currencyFormat);

        }
        public string formatmoneyinUs(Decimal d)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("en-us").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            return d.ToString("c", currencyFormat);
            //return String.Format(CultureInfo.CreateSpecificCulture("en-us"), "{0:C}", d);
        }

        public string GetDateFormat()
        {
            return "dd MM yyyy";
        }
        public string GetDateFormat(string format)
        {
            if (format == "Date")
                return "dd-MM-yyyy";
            else if (format == "DateTime")
                return "dd-MM-yyyy hh:mm tt";
            else
                return "dd-MM-yyyy";
        }
        public string getFormattedvalueWithounDecimalPlace(decimal Value)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyDecimalDigits = 0;
            currencyFormat.CurrencyNegativePattern = 2;

            if (Value < 0)
                Value = Value * -1;
            return Value.ToString("c", currencyFormat);

        }
        public string getFormattedvalueWithounDecimalPlaceOriginalSign(decimal Value)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyDecimalDigits = 0;
            currencyFormat.CurrencyNegativePattern = 2;
            return Value.ToString("c", currencyFormat);

        }

        public string getFormattedvalueWithCheckingDecimalPlaceOriginalSign(decimal Value)
        {
            Double num;

            num = Convert.ToDouble(Value);

            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";

            if (num.ToString().IndexOf('.') > 0)
            {
                currencyFormat.CurrencyDecimalDigits = num.ToString().Split('.').GetValue(1).ToString().Length;
            }
            else
            {
                currencyFormat.CurrencyDecimalDigits = 0;
            }


            currencyFormat.CurrencyNegativePattern = 2;

            return Value.ToString("c", currencyFormat);//.Replace(".000", "");

        }
        public string getFormattedvaluecheckorginaldecimaltwoorfour(decimal Value)
        {
            Double num;

            num = Convert.ToDouble(Value);
            int i = 0;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";



            if (num.ToString().IndexOf('.') > 0)
            {
                i = num.ToString().Split('.').GetValue(1).ToString().Length;
            }

            if (i > 2)
            {
                currencyFormat.CurrencyDecimalDigits = 2;
                //if (i == 3)
                //{
                //    currencyFormat.CurrencyDecimalDigits = 4;
                //}
                //else
                //{
                //    currencyFormat.CurrencyDecimalDigits = i;
                //}
            }
            else
            {
                currencyFormat.CurrencyDecimalDigits = 2;
            }


            currencyFormat.CurrencyNegativePattern = 2;

            return Value.ToString("c", currencyFormat);//.Replace(".000", "");

        }

        public void getFirstAndLastDate(String strmonth, out String StartDate, out String EndDate, out String billnoFinancialYear)
        {
            String financialYear;
            StartDate = String.Empty;
            EndDate = String.Empty;


            financialYear = HttpContext.Current.Session["LastFinYear"].ToString();
            int month = DateTime.Parse("1." + strmonth + " 2008").Month;
            string[] yearSplit;

            yearSplit = financialYear.Split('-');

            billnoFinancialYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

            if (month <= 3)
            {
                financialYear = yearSplit[1];
            }
            else
            {
                financialYear = yearSplit[0];
            }

            StartDate = "01 " + strmonth + " " + financialYear;

            DateTime firstDay = new DateTime(Convert.ToInt32(financialYear), month, 1);
            DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);

            EndDate = String.Format("{0:dd MMMM yyyy}", lastDayOfMonth);//dateSplit[1] + " " + month1 + " " + dateSplit[2];


        }
        #region ExportData
        //_this will populate open page content of the grid to EXCEL. NOT ALL DATA
        public void ExportToExcel(GridView LgridView, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.xls";

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            LgridView.RenderControl(htmlWrite);
            HttpContext.Current.Response.Write(stringWrite.ToString());
            HttpContext.Current.Response.End();

        }
        //_this will populate ALL DATA to EXCEL, datasource
        public void ExportToExcel(SqlDataSource dataSrc, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.xls";
            //GET Data From Database                
            SqlConnection cn = new SqlConnection(dataSrc.ConnectionString);
            string query =
                dataSrc.SelectCommand.Replace("\r\n", " ").Replace("\t", " ");

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.CommandTimeout = 999999;
            cmd.CommandType = CommandType.Text;
            try
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                StringBuilder sb = new StringBuilder();
                //Add Header          
                for (int count = 0; count < dr.FieldCount; count++)
                {
                    if (dr.GetName(count) != null)
                        sb.Append(dr.GetName(count));
                    if (count < dr.FieldCount - 1)
                    {
                        sb.Append(",");
                    }
                }
                HttpContext.Current.Response.Write(sb.ToString() + "\n");
                HttpContext.Current.Response.Flush();
                //Append Data
                while (dr.Read())
                {
                    sb = new StringBuilder();

                    for (int col = 0; col < dr.FieldCount - 1; col++)
                    {
                        if (!dr.IsDBNull(col))
                            sb.Append(dr.GetValue(col).ToString().Replace(",", " "));
                        sb.Append(",");
                    }
                    if (!dr.IsDBNull(dr.FieldCount - 1))
                        sb.Append(dr.GetValue(
                        dr.FieldCount - 1).ToString().Replace(",", " "));
                    HttpContext.Current.Response.Write(sb.ToString() + "\n");
                    HttpContext.Current.Response.Flush();
                }
                dr.Dispose();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
                cn.Close();
            }
            HttpContext.Current.Response.End();

        }
        //_this will populate ALL DATA to EXCEL, sqlQuery
        public void ExportToExcel(string SqlQuery, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.xls";
            //GET Data From Database   
             
            //SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            SqlConnection cn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            string query = SqlQuery;

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.CommandTimeout = 999999;
            cmd.CommandType = CommandType.Text;
            try
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                StringBuilder sb = new StringBuilder();
                //Add Header          
                for (int count = 0; count < dr.FieldCount; count++)
                {
                    if (dr.GetName(count) != null)
                        sb.Append(dr.GetName(count));
                    if (count < dr.FieldCount - 1)
                    {
                        sb.Append(",");
                    }
                }
                HttpContext.Current.Response.Write(sb.ToString() + "\n");
                HttpContext.Current.Response.Flush();
                //Append Data
                while (dr.Read())
                {
                    sb = new StringBuilder();

                    for (int col = 0; col < dr.FieldCount - 1; col++)
                    {
                        if (!dr.IsDBNull(col))
                            sb.Append(dr.GetValue(col).ToString().Replace(",", " "));
                        sb.Append(",");
                    }
                    if (!dr.IsDBNull(dr.FieldCount - 1))
                        sb.Append(dr.GetValue(
                        dr.FieldCount - 1).ToString().Replace(",", " "));
                    HttpContext.Current.Response.Write(sb.ToString() + "\n");
                    HttpContext.Current.Response.Flush();
                }
                dr.Dispose();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
                cn.Close();
            }
            HttpContext.Current.Response.End();

        }
        //_this will populate open page content of the grid to Word. NOT ALL DATA
        public void ExportToDoc(GridView LgridView, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.doc", fileName));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.word";

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            LgridView.RenderControl(htmlWrite);
            HttpContext.Current.Response.Write(stringWrite.ToString());
            HttpContext.Current.Response.End();

        }
        public void ExportToExcelWithProcedure(string Procedure, string fileName, string[] InputName, string[] InputValue)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.xls";
            //GET Data From Database                
           // SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            SqlConnection cn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            // string query = SqlQuery;
            DataTable ObjDT = new DataTable();
            //GetConnection();
            SqlCommand cmd = new SqlCommand(Procedure, cn);

            cmd.CommandTimeout = 999999;
            cmd.CommandType = CommandType.StoredProcedure;

            if (InputName != null)
            {
                for (int i = 0; i < (InputName.Length); i++)
                {
                    string PVariable = InputName[i];
                    //string DbType = ProcedureData[i, 1];
                    string PValue = InputValue[i];
                    cmd.Parameters.AddWithValue(PVariable, PValue);


                }
            }
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                //Populate the DataTable with the results
                da.Fill(ObjDT);
            }
            //oSqlConnection.Close();
            //return ObjDT;

            try
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                StringBuilder sb = new StringBuilder();
                //Add Header          
                for (int count = 0; count < dr.FieldCount; count++)
                {
                    if (dr.GetName(count) != null)
                        sb.Append(dr.GetName(count));
                    if (count < dr.FieldCount - 1)
                    {
                        sb.Append(",");
                    }
                }
                HttpContext.Current.Response.Write(sb.ToString() + "\n");
                HttpContext.Current.Response.Flush();
                //Append Data
                while (dr.Read())
                {
                    sb = new StringBuilder();

                    for (int col = 0; col < dr.FieldCount - 1; col++)
                    {
                        if (!dr.IsDBNull(col))
                            sb.Append(dr.GetValue(col).ToString().Replace(",", " "));
                        sb.Append(",");
                    }
                    if (!dr.IsDBNull(dr.FieldCount - 1))
                        sb.Append(dr.GetValue(
                        dr.FieldCount - 1).ToString().Replace(",", " "));
                    HttpContext.Current.Response.Write(sb.ToString() + "\n");
                    HttpContext.Current.Response.Flush();
                }
                //dr.Close();

            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
                cn.Close();
            }
            HttpContext.Current.Response.End();

        }
        public void ExportWithDatatable(DataTable table, DataTable table1, string name)
        {

            HttpContext context = HttpContext.Current;

            context.Response.Clear();

            context.Response.Write(Environment.NewLine);

            foreach (DataRow row in table1.Rows)
            {

                for (int i = 0; i < table1.Columns.Count; i++)
                {

                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");

                }

                context.Response.Write(Environment.NewLine);

            }


            context.Response.Write(Environment.NewLine);

            foreach (DataColumn column in table.Columns)
            {

                context.Response.Write(column.ColumnName + ",");

            }



            context.Response.Write(Environment.NewLine);



            foreach (DataRow row in table.Rows)
            {

                for (int i = 0; i < table.Columns.Count; i++)
                {

                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");

                }

                context.Response.Write(Environment.NewLine);

            }



            context.Response.ContentType = "text/csv";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");

            context.Response.End();

        }
        public void ExportToExcel(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }

            context.Response.Write(Environment.NewLine);

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }



            context.Response.ContentType = "text/csv";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");

            context.Response.End();

        }




        #endregion
        #region convertImageToByte

        public int getSignatureImage(string employeeID, out byte[] imgbyte, string dp)
        {
            string tmpimgname, signaturePath, str;
            int signstatus;
            string[] DataBasePath;


            imgbyte = new byte[1];
            imgbyte.Initialize();

            tmpimgname = string.Empty;
            signaturePath = string.Empty;
            signstatus = 0;


            //DataBasePath = oDBEngine.GetFieldValue1(" tbl_master_document ", " doc_source ", " doc_contactId='" + employeeID + "' and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ", 1);
            DataBasePath = proc.GetFieldValue1(" tbl_master_document ", " doc_source ", " doc_contactId='" + employeeID + "' and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ", 1);

            if (DataBasePath.Length > 0)
            {
                if ((DataBasePath.Length == 1) && (DataBasePath[0] == "n"))
                {
                }
                else
                {
                    signaturePath = DataBasePath.GetValue(0).ToString().Split('~').GetValue(1).ToString();
                    tmpimgname = "thumble_" + signaturePath;

                    signaturePath = HttpContext.Current.Server.MapPath(@"..\Documents\" + signaturePath);
                    tmpimgname = HttpContext.Current.Server.MapPath(@"..\Documents\" + tmpimgname);
                    string[] splitimage = signaturePath.Split('.');
                    tmpimgname = splitimage[0] + "_1." + splitimage[1];

                    FileStream fs;
                    BinaryReader br;

                    if (File.Exists(signaturePath))
                    {


                        System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(signaturePath);

                        System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                        System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(200, 20, dummyCallBack, IntPtr.Zero);

                        thumbNailImg.Save(tmpimgname, ImageFormat.MemoryBmp);
                        thumbNailImg.Dispose();

                        fs = new FileStream(tmpimgname, FileMode.Open);
                        br = new BinaryReader(fs);

                        imgbyte = new byte[fs.Length + 1];

                        imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));

                        // SignatureName = txtEmpName.Text.Split('[').GetValue(0);

                        br.Close();
                        fs.Close();
                        File.Delete(tmpimgname);
                        signstatus = 1;
                    }
                }
            }

            return signstatus;

        }

        public int getLogoImage(string logoPath, out byte[] logoByte)
        {
            int logostauts;
            FileStream fs;
            BinaryReader br;
            String tmpPath = HttpContext.Current.Server.MapPath(@"..\images\thumble_logo.jpg");

            logostauts = 0;
            logoByte = new byte[1];
            logoByte.Initialize();


            if (File.Exists(logoPath))
            {
                System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(logoPath);

                System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(fullSizeImg.Width, fullSizeImg.Height, dummyCallBack, IntPtr.Zero);
                thumbNailImg.Save(tmpPath, ImageFormat.Jpeg);

                thumbNailImg.Dispose();

                fs = new FileStream(tmpPath, FileMode.Open);
                br = new BinaryReader(fs);
                logoByte = new byte[fs.Length + 1];

                logoByte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                br.Close();
                fs.Close();
                File.Delete(tmpPath);
                logostauts = 1;
            }
            return logostauts;
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
        #endregion
        public string DirectoryPath(out string VirtualPath)
        {
            string Physicalpath = string.Empty;
            VirtualPath = string.Empty;
            int year = System.DateTime.Now.Year;
            string month = year.ToString() + "\\" + String.Format("{0:MMM}", System.DateTime.Now);

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\SIGNEDDOCS\")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\SIGNEDDOCS\"));
            }

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\SIGNEDDOCS\" + month)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\SIGNEDDOCS\" + month));
            }

            Physicalpath = HttpContext.Current.Server.MapPath(@"..\Documents\SIGNEDDOCS\" + month);
            VirtualPath = "Documents/SIGNEDDOCS/" + month.Replace("\\", "/");
            return Physicalpath;

        }
        #region DigitalSignature


        public string DigitalCertificate(String tempPdfPath, String signPath, String password,
                                             String signReason, String signLocation, String CompanyID,
                                                  String Segment_OR_DPID, String DocumentType, String Signatory,
                                                      String RecipientEmailID, String strDate, String BranchID,
                                                          String VirtualPath, String signPdfPath,
                                                              String user, String LastFinYear, int EmailCreateAppMenuId)
        {
            bool isUsed;
            int numberofPages;
            string filename, certrificatesStatus;
            filename = tempPdfPath.Substring(tempPdfPath.LastIndexOf('\\') + 1);
            isUsed = false;

            try
            {
                string zipfilepath = signPdfPath;
                string zipfilename = filename.Split('.')[0];
                string compresstotalpath = "";
                signPdfPath = signPdfPath + "\\" + filename;

                if (File.Exists(signPdfPath))
                {
                    isUsed = IsFileLocked(signPdfPath);
                }

                if (!isUsed)
                {
                    //if (password.Length > 0)
                    //{
                    Aspose.Pdf.Kit.Certificate cert = new Aspose.Pdf.Kit.Certificate(signPath, password);

                    Aspose.Pdf.Kit.PdfFileInfo pf = new Aspose.Pdf.Kit.PdfFileInfo(tempPdfPath);
                    Aspose.Pdf.Kit.PdfFileSignature pdfSign = new Aspose.Pdf.Kit.PdfFileSignature(cert);

                    numberofPages = pf.NumberofPages;
                    pdfSign.BindPdf(tempPdfPath);
                    pdfSign.Sign(numberofPages, signReason, "success", signLocation, true, new System.Drawing.Rectangle(500, 49, 300, 48));

                    pdfSign.Save(signPdfPath);

                    compresstotalpath = zipfilepath + "\\" + zipfilename + ".zip";
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.AddFile(signPdfPath, zipfilename);
                        zip.Save(compresstotalpath);
                    }
                    //}
                    filename = filename.Replace("pdf", "zip");

                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                    if (File.Exists(signPdfPath))
                    {
                        File.Delete(signPdfPath);
                    }
                    int j = 0;

                    string[] str = oDBEngine.GetFieldValue1("tbl_master_segment", "seg_name", "seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                    //if ((str[0].ToString() == "ICEX-COMM")  )
                    //{
                    //    j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                    //                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                    //                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                    //}
                    if ((str[0].ToString() == "ICEX-COMM") || (str[0].ToString() == "NSE-CM") || (str[0].ToString() == "NSE-FO") || (str[0].ToString() == "BSE-CM") || (str[0].ToString() == "NSE-CDX") || (str[0].ToString() == "BSE-CDX") || (str[0].ToString() == "MCXSX-CDX") || (str[0].ToString() == "USE-CDX") || (str[0].ToString() == "MCX-COMM") || (str[0].ToString() == "NSEL-SPOT") || (str[0].ToString() == "NCDEX-COMM") || (str[0].ToString() == "BSE-FO") || (str[0].ToString() == "MCXSX-FO") || (str[0].ToString() == "MCXSX-CM") || (str[0].ToString() == "UCX-COMM"))
                    {
                        if (RecipientEmailID != "")
                        {
                            j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                                            BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                                            filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                        }
                        else
                        {
                            j = 1;
                        }

                    }
                    else
                    {
                        j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                                        BranchID, filename.Split('-').GetValue(3).ToString(), DocumentType,
                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);

                    }

                    if (j != 0)
                    {
                        certrificatesStatus = "Success";
                    }
                    else
                    {
                        if (File.Exists(signPdfPath))
                        {
                            File.Delete(signPdfPath);
                        }
                        certrificatesStatus = "Cannot Generate Signed Document.";
                    }
                }
                else
                {
                    certrificatesStatus = "File Path is being Used by another Process.";
                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                }
            }
            catch (Exception e)
            {

                certrificatesStatus = e.Message;
            }




            return certrificatesStatus;
        }
        public string DigitalCertificate1(String tempPdfPath, String signPath, String password,
                                            String signReason, String signLocation, String CompanyID,
                                                 String Segment_OR_DPID, String DocumentType, String Signatory,
                                                     String RecipientEmailID, String strDate, String strDate2, String BranchID,
                                                         String VirtualPath, String signPdfPath,
                                                             String user, String LastFinYear, int EmailCreateAppMenuId)
        {
            bool isUsed;
            int numberofPages;
            string filename, certrificatesStatus;
            filename = tempPdfPath.Substring(tempPdfPath.LastIndexOf('\\') + 1);
            isUsed = false;

            try
            {
                string zipfilepath = signPdfPath;
                string zipfilename = filename.Split('.')[0];
                string compresstotalpath = "";
                signPdfPath = signPdfPath + "\\" + filename;

                if (File.Exists(signPdfPath))
                {
                    isUsed = IsFileLocked(signPdfPath);
                }

                if (!isUsed)
                {
                    Aspose.Pdf.Kit.Certificate cert = new Aspose.Pdf.Kit.Certificate(signPath, password);

                    Aspose.Pdf.Kit.PdfFileInfo pf = new Aspose.Pdf.Kit.PdfFileInfo(tempPdfPath);
                    Aspose.Pdf.Kit.PdfFileSignature pdfSign = new Aspose.Pdf.Kit.PdfFileSignature(cert);

                    numberofPages = pf.NumberofPages;
                    pdfSign.BindPdf(tempPdfPath);
                    pdfSign.Sign(numberofPages, signReason, "success", signLocation, true, new System.Drawing.Rectangle(500, 49, 300, 48));

                    pdfSign.Save(signPdfPath);


                    compresstotalpath = zipfilepath + "\\" + zipfilename + ".zip";
                    using (ZipFile zip = new ZipFile())
                    {

                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.AddFile(signPdfPath, zipfilename);
                        zip.Save(compresstotalpath);
                    }
                    //}
                    filename = filename.Replace("pdf", "zip");

                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                    if (File.Exists(signPdfPath))
                    {
                        File.Delete(signPdfPath);
                    }
                    int j = 0;

                    string[] str = oDBEngine.GetFieldValue1("tbl_master_segment", "seg_name", "seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                    //if ((str[0].ToString() == "ICEX-COMM"))
                    //{
                    //    j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,strDate2,
                    //                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                    //                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                    //}
                    if ((str[0].ToString() == "ICEX-COMM") || (str[0].ToString() == "NSE-CM") || (str[0].ToString() == "NSE-FO") || (str[0].ToString() == "BSE-CM") || (str[0].ToString() == "NSE-CDX") || (str[0].ToString() == "BSE-CDX") || (str[0].ToString() == "MCXSX-CDX") || (str[0].ToString() == "USE-CDX") || (str[0].ToString() == "MCX-COMM") || (str[0].ToString() == "NSEL-SPOT") || (str[0].ToString() == "NCDEX-COMM") || (str[0].ToString() == "BSE-FO") || (str[0].ToString() == "MCXSX-FO") || (str[0].ToString() == "MCXSX-CM") || (str[0].ToString() == "UCX-COMM"))
                    {
                        if (RecipientEmailID != "")
                        {
                            j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate, strDate2,
                                            BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                                            filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                        }
                        else
                        {
                            j = 1;
                        }

                    }
                    else
                    {
                        j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate, strDate2,
                                        BranchID, filename.Split('-').GetValue(3).ToString(), DocumentType,
                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);

                    }

                    if (j != 0)
                    {
                        certrificatesStatus = "Success";
                    }
                    else
                    {
                        if (File.Exists(signPdfPath))
                        {
                            File.Delete(signPdfPath);
                        }
                        certrificatesStatus = "Cannot Generate Signed Document.";
                    }
                }
                else
                {
                    certrificatesStatus = "File Path is being Used by another Process.";
                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                }
            }
            catch (Exception e)
            {

                certrificatesStatus = e.Message;
            }




            return certrificatesStatus;
        }
        public bool IsFileLocked(String Fullpath)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(Fullpath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            //file is not locked 
            return false;


        }

        #endregion




        #region EmailBodyStyle
        public string Email_TableStyle()
        {
            string strStyle;
            strStyle = string.Empty;
            strStyle = " width=" + Convert.ToChar(34) + "100%" + Convert.ToChar(34) +
                       " aligh=" + Convert.ToChar(34) + "left" + Convert.ToChar(34) +
                       " border=" + Convert.ToChar(34) + "2" + Convert.ToChar(34) +
                       " cellpadding=" + Convert.ToChar(34) + "4" + Convert.ToChar(34) +
                       " cellspacing=" + Convert.ToChar(34) + "0" + Convert.ToChar(34) +
                       " bordercolor=" + Convert.ToChar(34) + "#FFCC00" + Convert.ToChar(34) +
                       " style=" + Convert.ToChar(34) +
                       " padding: 2px 1px; font-size: 10px;" +
                       " font-family: Tahoma,Arial,Verdana,sans-serif;" + Convert.ToChar(34) + " ";
            // " border-style:solid ; border-width:2px; border-color:#FFCC00;"+Convert.ToChar(34) + " ";

            return strStyle;
        }
        public string Email_HeaderColor()
        {
            string strStyle;
            strStyle = string.Empty;

            strStyle = " bgcolor=" + Convert.ToChar(34) + "#FFE682" + Convert.ToChar(34) +
                       " align=" + Convert.ToChar(34) + "center" + Convert.ToChar(34) + " ";

            return strStyle;
        }
        public string Email_HeaderText(string Headertext)
        {
            string strText;
            strText = string.Empty;

            strText = "<strong><font size=" + Convert.ToChar(34) + "3" + Convert.ToChar(34) + ">"
                        + Headertext + "</font></strong>";

            return strText;
        }


        public string Email_SubHeaderColor()
        {
            string strStyle;
            strStyle = string.Empty;

            strStyle = " bgcolor=" + Convert.ToChar(34) + "#FFCC33" + Convert.ToChar(34) +
                       " align=" + Convert.ToChar(34) + "center" + Convert.ToChar(34) + " ";

            return strStyle;
        }
        public string Email_ContentHeaderColor()
        {
            string strStyle;
            strStyle = string.Empty;

            strStyle = " bgcolor=" + Convert.ToChar(34) + "#FFE682" + Convert.ToChar(34) +
                       " align=" + Convert.ToChar(34) + "left" + Convert.ToChar(34) + " ";

            return strStyle;

        }

        public string Email_BoldText(string Text)
        {
            string strText;
            strText = string.Empty;

            strText = "<b>" + Text + "</b>";

            return strText;
        }


        #endregion

        public string GetAutoGenerateNo()
        {
           
            //string datetd = Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy hh:mm:ss tt");
            //string[] slashsp = datetd.Split('/');
            //string mm = slashsp[0].ToString();
            //string dd = slashsp[1].ToString();
            //string[] dashsp = slashsp[2].ToString().Split(' ');
            //string yy = dashsp[0].ToString();
            //string ampm = dashsp[2].ToString();
            //string[] tmsp = dashsp[1].ToString().Split(':');
            //string hr = tmsp[0].ToString();
            //string min = tmsp[1].ToString();
            //string mls = tmsp[2].ToString();
            //string totnum = mm + dd + yy + hr + min + mls + ampm;
            //return totnum;

            //string datetd = Convert.ToDateTime(DateTime.Now).ToString();
            //string[] slashsp = datetd.Split('-');
            //string yy = slashsp[0].ToString();
            //string mm = slashsp[1].ToString();
            //string[] dashsp = slashsp[2].ToString().Split(' ');
            //string dd = dashsp[0].ToString();
            //string ampm = dashsp[1].ToString();
            //string[] tmsp = dashsp[1].ToString().Split(':');
            //string hr = tmsp[0].ToString();
            //string min = tmsp[1].ToString();
            //string mls = tmsp[2].ToString();
            //string totnum = mm + dd + yy + hr + min + mls + ampm.Replace(':', 'o');
            //return totnum;

            string totnum = System.DateTime.Now.Ticks.ToString();
            return totnum;
        }
        public string GetAutoGenerateNoForFileUpload()
        {
            //string datetd = Convert.ToDateTime(DateTime.Now).ToString();
            //string[] slashsp = datetd.Split('-');
            //string yy = slashsp[0].ToString();
            //string mm = slashsp[1].ToString();
            //string[] dashsp = slashsp[2].ToString().Split(' ');
            //string dd = dashsp[0].ToString();
            //string ampm = dashsp[1].ToString();
            //string[] tmsp = dashsp[1].ToString().Split(':');
            //string hr = tmsp[0].ToString();
            //string min = tmsp[1].ToString();
            //string mls = tmsp[2].ToString();
            //string totnum = mm + dd + yy + hr + min + mls + ampm.Replace(':', 'o');
            //return totnum;
            string totnum = System.DateTime.Now.Ticks.ToString();
            return totnum;
        }
        public string ConvertDataTableToXML(DataTable dtBuildSQL)
        {
            DataSet dsBuildSQL = new DataSet();
            StringBuilder sbSQL;
            StringWriter swSQL;
            string XMLformat;
            sbSQL = new StringBuilder();
            swSQL = new StringWriter(sbSQL);
            dsBuildSQL.Merge(dtBuildSQL, true, MissingSchemaAction.AddWithKey);
            dsBuildSQL.Tables[0].TableName = "Table";
            foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
            {
                col.ColumnMapping = MappingType.Attribute;
            }
            dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
            XMLformat = sbSQL.ToString();
            return XMLformat;
        }

        public string QuarterlyDate(DateTime date)
        {
            int month = date.Month;
            string FromDate = null;
            string ToDate = null;
            if (month == 4 || month == 5 || month == 6)
            {
                FromDate = "04" + "/01/" + date.Year;
                ToDate = DateTime.Now.ToShortDateString();
            }
            else if (month == 7 || month == 8 || month == 9)
            {
                FromDate = "04" + "/01/" + date.Year;
                ToDate = "06" + "/30/" + date.Year;
            }
            else if (month == 10 || month == 11 || month == 12)
            {
                FromDate = "07" + "/01/" + date.Year;
                ToDate = "09" + "/30/" + date.Year;
            }
            else if (month == 1 || month == 2 || month == 3)
            {
                FromDate = "10" + "/01/" + date.Year;
                ToDate = "12" + "/31/" + date.Year;
            }
            return FromDate + "~" + ToDate;
        }

    }
}
