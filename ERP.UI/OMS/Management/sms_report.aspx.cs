using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_sms_report : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine db = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine db = new BusinessLogicLayer.DBEngine();
        getautomaticsms sms = new getautomaticsms();
        string sBody = String.Empty;
        String searchString, sPostData, sTo, phoneNumber;
        string[] str = new string[10];

        protected void Page_Load(object sender, EventArgs e)
        {
            //cdslHolding_frmPhone.aspx

            #region a
            #region sms

            try
            {

                sBody = getsms();
                // }

                sTo = String.Empty;
                searchString = String.Empty;
                phoneNumber = String.Empty;
                phoneNumber = Request.QueryString["number"].ToString().Trim();
                sTo = phoneNumber;
                int length;
                length = sTo.Length;

                if (sTo.Length > 10)
                {
                    length = length - 10;
                    sTo = sTo.Substring(length);
                }

                sTo = "'" + sTo + "'";

                if (sBody == String.Empty)
                {
                    sBody = "No Result Found";
                }

                if (sBody != "No Result Found" && sBody != "a")
                {
                    if (sBody.Length > 160)
                    {
                        for (int i = 0; i < sBody.Length; i = i + 152)
                        {
                            if ((i + 152) <= sBody.Length)
                            {
                                trimsms(i, 152);
                            }
                            else
                            {
                                trimsms(i, sBody.Length - i);
                            }
                        }
                    }
                    else
                    {
                        smsnottrim();
                    }
                    sms.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), sms.contactidnew1, Request.QueryString["number"].ToString(), "S", ds.Tables[2].Rows[0][0].ToString(), "s");


                }
                else if (sBody != "a")
                {
                    sms.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), sms.contactidnew1, Request.QueryString["number"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "N");//no subscription
                    Response.Write("Do not have any value in the segment");
                }

            }

            catch (Exception ex)
            {
                sms.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), sms.contactidnew1, Request.QueryString["number"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "F");// failed while sending
                Response.Write("   Can not send");
            }

            #endregion sms
            #endregion a


        }
        private string getsms()
        {
            try
            {
                ds.Reset();
                ds = sms.fetch_status_sms(Request.QueryString["number"].ToString(), getwords(Request.QueryString["subject"].Remove(0, 3).ToString()));
                string contactid = "";

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    contactid = contactid + ds.Tables[1].Rows[i][0].ToString() + '|';

                }
                contactid = contactid.Remove(contactid.Length - 1, 1);
                contactid = contactid.Trim();
                string s = "";
                switch (ds.Tables[0].Rows[0][0].ToString())
                {
                    case "CLNT000004":
                        try
                        {
                            s = sms.cdslHoldingFrmsms(contactid, getwords(Request.QueryString["subject"].Remove(0, 3).ToString()));
                        }
                        catch
                        {

                        }
                        break;
                    case "CLNT000036":
                        try
                        {
                            s = sms.tradessms(contactid, getwords(Request.QueryString["subject"].Remove(0, 3).ToString()));
                        }
                        catch
                        {

                        }
                        break;
                    case "CLNT000019":
                        try
                        {
                            s = sms.dpbillsms(contactid, getwords(Request.QueryString["subject"].Remove(0, 3).ToString()));
                        }
                        catch
                        {

                        }
                        break;
                    case "CLNT000006":
                        try
                        {
                            s = sms.dp5trnsms(contactid, getwords(Request.QueryString["subject"].Remove(0, 3).ToString()));
                        }
                        catch
                        {

                        }
                        break;
                }
                ViewState["topiccode"] = ds.Tables[0].Rows[0][0];
                if (ViewState["topiccode"] == null)
                {
                    ViewState["topiccode"] = "";
                }
                ViewState["RequestID"] = ds.Tables[2].Rows[0][0];
                //string strrrr=ConfigurationSettings.AppSettings["SmsProvider"].ToString();
                return s;
            }
            catch
            {
                ViewState["topiccode"] = "";
                ViewState["RequestID"] = "";
                sms.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), sms.contactidnew1, Request.QueryString["number"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "W");//wrong subject
                Response.Write("Can not send subject is invalid");
                return "a";
            }
        }

        public string[] getwords(string sub)
        {
            string str = sub;
            str = str.Trim();
            string[] str1 = str.Split(' ');
            string j = "";
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != "")
                {
                    j = j + " " + str1[i];
                }
                else
                {
                    j = j;
                }


            }
            j = j.Trim();
            str1 = j.Split(' ');

            return str1;
        }



        public void trimsms(int start, int end)
        {

            string sBody1 = "";
            if (start == 0)
            {
                sBody1 = sBody.Substring(start, end) + " contd:";
            }
            else if (end == sBody.Length - start)
            {
                sBody1 = sBody.Substring(start, end);
            }
            else
            {
                sBody1 = sBody.Substring(start, end) + " contd:";
            }


            sPostData = "<?xml version='1.0' encoding='ISO-8859-1'?><!DOCTYPE MESSAGE SYSTEM 'http://127.0.0.1/psms/dtd/messagev12.dtd' ><MESSAGE VER='1.2'><USER USERNAME='" + ConfigurationSettings.AppSettings["sSMSLiveUser"] + "' PASSWORD='" + ConfigurationSettings.AppSettings["sSMSLivePass"] + "'/><SMS UDH='0' CODING='1' TEXT='" + Server.UrlEncode(sBody1) + "' PROPERTY='0' ID='1'><ADDRESS FROM='" + ConfigurationSettings.AppSettings["sSMSLiveSender"] + "' TO='" + Server.UrlEncode(phoneNumber) + "' SEQ='1' TAG='Auto SMS'/></SMS></MESSAGE>";


            //   sPostData = "User=" + ConfigurationSettings.AppSettings["sSMSLiveUser"] + "&Pws=" + ConfigurationSettings.AppSettings["sSMSLivePass"] + "&Receipent=" + Server.UrlEncode(phoneNumber) + "&Sms=" + Server.UrlEncode(sBody1);


            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();


            byte[] buffer = enc.GetBytes(sPostData);
            //HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://www.SMSLive.in/Push/default.aspx");
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(ConfigurationSettings.AppSettings["SmsProvider"].ToString());
            WebReq.Method = "POST";
            WebReq.ContentType = "application/x-www-form-urlencoded";


            Stream PostData = WebReq.GetRequestStream();

            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);


            Response.Write(_Answer.ReadToEnd().ToString());


            Response.Write(sBody1);
        }
        public void smsnottrim()
        {
            string sBody1 = "";
            sPostData = "<?xml version='1.0' encoding='ISO-8859-1'?><!DOCTYPE MESSAGE SYSTEM 'http://127.0.0.1/psms/dtd/messagev12.dtd' ><MESSAGE VER='1.2'><USER USERNAME='" + ConfigurationSettings.AppSettings["sSMSLiveUser"] + "' PASSWORD='" + ConfigurationSettings.AppSettings["sSMSLivePass"] + "'/><SMS UDH='0' CODING='1' TEXT='" + Server.UrlEncode(sBody1) + "' PROPERTY='0' ID='1'><ADDRESS FROM='" + ConfigurationSettings.AppSettings["sSMSLiveSender"] + "' TO='" + Server.UrlEncode(phoneNumber) + "' SEQ='1' TAG='Auto SMS'/></SMS></MESSAGE>";


            //   sPostData = "User=" + ConfigurationSettings.AppSettings["sSMSLiveUser"] + "&Pws=" + ConfigurationSettings.AppSettings["sSMSLivePass"] + "&Receipent=" + Server.UrlEncode(phoneNumber) + "&Sms=" + Server.UrlEncode(sBody1);


            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();


            byte[] buffer = enc.GetBytes(sPostData);
            //HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://www.SMSLive.in/Push/default.aspx");
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(ConfigurationSettings.AppSettings["SmsProvider"].ToString());
            WebReq.Method = "POST";
            WebReq.ContentType = "application/x-www-form-urlencoded";


            Stream PostData = WebReq.GetRequestStream();

            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);


            Response.Write(_Answer.ReadToEnd().ToString());


            Response.Write(sBody);
        }

    }
}