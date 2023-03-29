/**************************************************************************************************************************
 *  1.0     29/03/2023      V2.0.37     Sanchita        Indiamart API upgradation for Peekay. Refer: 25758
 * ***********************************************************************************************************************/
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CRM.Controllers
{

    #region Using More than one Key
    public class IndiamartController : Controller
    {
        //String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];


        List<IndiamartModelClass> omodel = new List<IndiamartModelClass>();
        List<IndiamartModelErrorClass> indmdlerror = new List<IndiamartModelErrorClass>();
        private Random _random = new Random();
        DBEngine objsql = new DBEngine();
        DataTable dtobsql = new DataTable();


        DataTable dt = new DataTable();
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        public JsonResult Index()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            string output = string.Empty;
            string strerrormessage = "";
            string mobileno = "";
            string Action = "";
            // Rev 1.0
            int TotalCount = 0;
            // End of Rev 1.0

            List<IndiamartModelErrorClass> idnMart = new List<IndiamartModelErrorClass>();
            


            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();

            // String con = Convert.ToString(Session["ERPConnection"]);
            String con = obj.GetDefaultConnectionStringWithoutSession();

            SqlConnection sqlcon = new SqlConnection(con);

            sqlcon.Open();
            sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfiguration", sqlcon);
            sqlcmd.Parameters.Add("@vendorID", "1,4");

            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dtobsql);
            sqlcon.Close();
            string URL = "";
            try
            {
                if (dtobsql != null)
                {
                    if (dtobsql.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtobsql.Rows.Count; i++)
                        {
                            IndiamartModelErrorClass objErr = new IndiamartModelErrorClass();
                            URL = Convert.ToString(dtobsql.Rows[i][0]);
                            mobileno = Convert.ToString(dtobsql.Rows[i][1]);

                            objErr.MobileNo = mobileno;

                            using (var webClient = new System.Net.WebClient())
                            {
                                var json = webClient.DownloadString(URL);
                                // Now parse with JSON.Net


                                JavaScriptSerializer ser = new JavaScriptSerializer();

                                // Rev 1.0
                                //List<IndiamartModelClass> indiamartlist = ser.Deserialize<List<IndiamartModelClass>>(json);
                                //if (indiamartlist.Count>1)
                                //{
                                //    Action = "Success";
                                //    objErr.jsonError = "Success";
                                //    objErr.ErrorMessage = Action;
                                //    omodel = new List<IndiamartModelClass>();
                                //    foreach (var s2 in indiamartlist)
                                //    {
                                //        string rand = GenerateRandomNo();
                                //        omodel.Add(new IndiamartModelClass()
                                //        {
                                //            Indiamart_Id = "Indiamart" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),
                                //            Rn = s2.Rn,
                                //            QUERY_ID = s2.QUERY_ID,
                                //            QTYPE = s2.QTYPE,
                                //            SENDERNAME = s2.SENDERNAME,
                                //            SENDEREMAIL = s2.SENDEREMAIL,
                                //            SUBJECT = s2.SUBJECT,
                                //            DATE_RE = s2.DATE_RE,
                                //            DATE_R = s2.DATE_R,
                                //            DATE_TIME_RE = s2.DATE_TIME_RE,
                                //            GLUSR_USR_COMPANYNAME = s2.GLUSR_USR_COMPANYNAME,
                                //            READ_STATUS = s2.READ_STATUS,
                                //            SENDER_GLUSR_USR_ID = s2.SENDER_GLUSR_USR_ID,
                                //            MOB = s2.MOB,
                                //            COUNTRY_FLAG = s2.COUNTRY_FLAG,
                                //            QUERY_MODID = s2.QUERY_MODID,
                                //            LOG_TIME = s2.LOG_TIME,
                                //            QUERY_MODREFID = s2.QUERY_MODREFID,
                                //            DIR_QUERY_MODREF_TYPE = s2.DIR_QUERY_MODREF_TYPE,
                                //            ORG_SENDER_GLUSR_ID = s2.ORG_SENDER_GLUSR_ID,
                                //            ENQ_MESSAGE = s2.ENQ_MESSAGE,
                                //            ENQ_ADDRESS = s2.ENQ_ADDRESS,
                                //            ENQ_CALL_DURATION = s2.ENQ_CALL_DURATION,
                                //            ENQ_RECEIVER_MOB = s2.ENQ_RECEIVER_MOB,
                                //            ENQ_CITY = s2.ENQ_CITY,
                                //            ENQ_STATE = s2.ENQ_STATE,
                                //            PRODUCT_NAME = s2.PRODUCT_NAME,
                                //            COUNTRY_ISO = s2.COUNTRY_ISO,
                                //            EMAIL_ALT = s2.EMAIL_ALT,
                                //            MOBILE_ALT = s2.MOBILE_ALT,
                                //            PHONE = s2.PHONE,
                                //            PHONE_ALT = s2.PHONE_ALT,
                                //            IM_MEMBER_SINCE = s2.IM_MEMBER_SINCE,
                                //            TOTAL_COUNT = s2.TOTAL_COUNT


                                //        });

                                //    }
                                //    objErr.Indiamart = omodel;

                                //}
                                //else
                                //{
                                //     Action = "Error";
                                //     strerrormessage = json.ToString();
                                //     objErr.ErrorMessage = Action;
                                //     objErr.jsonError = strerrormessage;

                                //}

                                IndiamartModelClassKey indiamartListKey = ser.Deserialize<IndiamartModelClassKey>(json);

                                TotalCount = indiamartListKey.RESPONSE.Count;

                                if (indiamartListKey.STATUS == "SUCCESS" && TotalCount > 1)
                                {
                                    Action = "Success";
                                    objErr.jsonError = "Success";
                                    objErr.ErrorMessage = Action;
                                    omodel = new List<IndiamartModelClass>();
                                    foreach (var s2 in indiamartListKey.RESPONSE)
                                    {
                                        string rand = GenerateRandomNo();
                                        omodel.Add(new IndiamartModelClass()
                                        {
                                            Indiamart_Id = "Indiamart" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),
                                            UNIQUE_QUERY_ID = s2.UNIQUE_QUERY_ID,
                                            QUERY_TYPE = s2.QUERY_TYPE,
                                            QUERY_TIME = s2.QUERY_TIME,
                                            SENDER_NAME = s2.SENDER_NAME,
                                            SENDER_MOBILE = s2.SENDER_MOBILE,
                                            SENDER_EMAIL = s2.SENDER_EMAIL,
                                            SENDER_COMPANY = s2.SENDER_COMPANY,
                                            SENDER_ADDRESS = s2.SENDER_ADDRESS,
                                            SENDER_CITY = s2.SENDER_CITY,
                                            SENDER_STATE = s2.SENDER_STATE,
                                            SENDER_COUNTRY_ISO = s2.SENDER_COUNTRY_ISO,
                                            SENDER_MOBILE_ALT = s2.SENDER_MOBILE_ALT,
                                            SENDER_EMAIL_ALT = s2.SENDER_EMAIL_ALT,
                                            QUERY_PRODUCT_NAME = s2.QUERY_PRODUCT_NAME,
                                            QUERY_MESSAGE = s2.QUERY_MESSAGE,
                                            CALL_DURATION = s2.CALL_DURATION,
                                            RECEIVER_MOBILE = s2.RECEIVER_MOBILE
                                        });

                                    }
                                    objErr.Indiamart = omodel;

                                }
                                else
                                {
                                    Action = "Error";
                                    strerrormessage = json.ToString();
                                    objErr.ErrorMessage = Action;
                                    objErr.jsonError = strerrormessage;

                                }
                                // End of Rev 1.0
                            }

                            idnMart.Add(objErr);
                        }

                    }

                    foreach (IndiamartModelErrorClass omodel in idnMart)
                    {
                        string JsonXML = "";
                        if (omodel.Indiamart != null)
                        {
                             JsonXML = XmlConversion.ConvertToXml(omodel.Indiamart, 0);
                        }
                        else
                        {
                             JsonXML = "";
                        }
                 

                        DataTable dt = new DataTable();
                        //SqlCommand sqlcmd = new SqlCommand();
                        //SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();

                        // Rev 1.0
                        //sqlcmd = new SqlCommand("Proc_Import_IndiaMart", sqlcon);
                        sqlcmd = new SqlCommand("ERP_Proc_Import_IndiaMart", sqlcon);
                        // End of Rev 1.0
                        sqlcmd.Parameters.Add("@JsonXML", JsonXML);
                        sqlcmd.Parameters.Add("@MobileNo", omodel.MobileNo);
                        sqlcmd.Parameters.Add("@Action", Action);
                        sqlcmd.Parameters.Add("@Errortext", omodel.jsonError);
                        // Rev 1.0
                        sqlcmd.Parameters.Add("@TotalCount", TotalCount);
                        sqlcmd.Parameters.Add("@Vendor_Name", "IndiaMart");
                        // End of Rev 1.0
                        sqlcmd.Parameters.Add("@ReturnValue", SqlDbType.Char, 50);
                        sqlcmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output; 

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcmd.Dispose();
                        output = (string)sqlcmd.Parameters["@ReturnValue"].Value; 
                        sqlcon.Close();

                        if (dt.Rows.Count > 0)
                        {

                        }
                    }


               

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Duplicate", JsonRequestBehavior.AllowGet);
                    //  string URL = "http://mapi.indiamart.com/wservce/enquiry/listing/GLUSR_MOBILE/7042445112/GLUSR_MOBILE_KEY/NzA0MjQ0NTExMiMxMDM1OTU4MA==/";
                }

            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                //SqlCommand sqlcmd = new SqlCommand();
                //SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();


                sqlcmd = new SqlCommand("Proc_Import_IndiaMart", sqlcon);
               // sqlcmd.Parameters.Add("@JsonXML", JsonXML);
                sqlcmd.Parameters.Add("@MobileNo", "");
                sqlcmd.Parameters.Add("@Action", "Error");
                sqlcmd.Parameters.Add("@Errortext", ex.ToString());

                sqlcmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }
    }

    #endregion


    #region Using  one Key
    //public class IndiamartController : Controller
    //{
    //    //String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];


    //    List<IndiamartModelClass> omodel = new List<IndiamartModelClass>();
    //    private Random _random = new Random();
    //    DBEngine objsql = new DBEngine();
    //    DataTable dtobsql = new DataTable();


    //    DataTable dt = new DataTable();
    //    SqlCommand sqlcmd = new SqlCommand();
    //    SqlDataAdapter da = new SqlDataAdapter();
    //    public JsonResult Index()
    //    {

    //        ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();

    //        // String con = Convert.ToString(Session["ERPConnection"]);
    //        String con = obj.GetDefaultConnectionStringWithoutSession();

    //        SqlConnection sqlcon = new SqlConnection(con);

    //        sqlcon.Open();
    //        sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfiguration", sqlcon);
    //        sqlcmd.Parameters.Add("@vendorID", 1);

    //        sqlcmd.CommandType = CommandType.StoredProcedure;
    //        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
    //        da.Fill(dtobsql);
    //        sqlcon.Close();
    //        string URL = "";
    //        if (dtobsql.Rows.Count > 0)
    //        {
    //            URL = Convert.ToString(dtobsql.Rows[0][0]);

    //        }


    //        //  string URL = "http://mapi.indiamart.com/wservce/enquiry/listing/GLUSR_MOBILE/7042445112/GLUSR_MOBILE_KEY/NzA0MjQ0NTExMiMxMDM1OTU4MA==/";
    //        using (var webClient = new System.Net.WebClient())
    //        {
    //            var json = webClient.DownloadString(URL);
    //            // Now parse with JSON.Net


    //            JavaScriptSerializer ser = new JavaScriptSerializer();

    //            List<IndiamartModelClass> indiamartlist = ser.Deserialize<List<IndiamartModelClass>>(json);

    //            foreach (var s2 in indiamartlist)
    //            {
    //                string rand = GenerateRandomNo();
    //                omodel.Add(new IndiamartModelClass()
    //                {
    //                    Indiamart_Id = "Indiamart" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),
    //                    Rn = s2.Rn,
    //                    QUERY_ID = s2.QUERY_ID,
    //                    QTYPE = s2.QTYPE,
    //                    SENDERNAME = s2.SENDERNAME,
    //                    SENDEREMAIL = s2.SENDEREMAIL,
    //                    SUBJECT = s2.SUBJECT,
    //                    DATE_RE = s2.DATE_RE,
    //                    DATE_R = s2.DATE_R,
    //                    DATE_TIME_RE = s2.DATE_TIME_RE,
    //                    GLUSR_USR_COMPANYNAME = s2.GLUSR_USR_COMPANYNAME,
    //                    READ_STATUS = s2.READ_STATUS,
    //                    SENDER_GLUSR_USR_ID = s2.SENDER_GLUSR_USR_ID,
    //                    MOB = s2.MOB,
    //                    COUNTRY_FLAG = s2.COUNTRY_FLAG,
    //                    QUERY_MODID = s2.QUERY_MODID,
    //                    LOG_TIME = s2.LOG_TIME,
    //                    QUERY_MODREFID = s2.QUERY_MODREFID,
    //                    DIR_QUERY_MODREF_TYPE = s2.DIR_QUERY_MODREF_TYPE,
    //                    ORG_SENDER_GLUSR_ID = s2.ORG_SENDER_GLUSR_ID,
    //                    ENQ_MESSAGE = s2.ENQ_MESSAGE,
    //                    ENQ_ADDRESS = s2.ENQ_ADDRESS,
    //                    ENQ_CALL_DURATION = s2.ENQ_CALL_DURATION,
    //                    ENQ_RECEIVER_MOB = s2.ENQ_RECEIVER_MOB,
    //                    ENQ_CITY = s2.ENQ_CITY,
    //                    ENQ_STATE = s2.ENQ_STATE,
    //                    PRODUCT_NAME = s2.PRODUCT_NAME,
    //                    COUNTRY_ISO = s2.COUNTRY_ISO,
    //                    EMAIL_ALT = s2.EMAIL_ALT,
    //                    MOBILE_ALT = s2.MOBILE_ALT,
    //                    PHONE = s2.PHONE,
    //                    PHONE_ALT = s2.PHONE_ALT,
    //                    IM_MEMBER_SINCE = s2.IM_MEMBER_SINCE,
    //                    TOTAL_COUNT = s2.TOTAL_COUNT


    //                });

    //            }


    //        }

    //        string JsonXML = XmlConversion.ConvertToXml(omodel, 0);

    //        DataTable dt = new DataTable();
    //        //SqlCommand sqlcmd = new SqlCommand();
    //        //SqlConnection sqlcon = new SqlConnection(con);
    //        sqlcon.Open();


    //        sqlcmd = new SqlCommand("Proc_Import_IndiaMart", sqlcon);
    //        sqlcmd.Parameters.Add("@JsonXML", JsonXML);

    //        sqlcmd.CommandType = CommandType.StoredProcedure;
    //        da = new SqlDataAdapter(sqlcmd);
    //        da.Fill(dt);
    //        sqlcon.Close();

    //        if (dt.Rows.Count > 0)
    //        {

    //        }

    //        return Json("Success", JsonRequestBehavior.AllowGet);

    //    }

    //    public string GenerateRandomNo()
    //    {
    //        return _random.Next(0, 9999).ToString("D4");
    //    }
    //}

    #endregion
    // Rev 1.0
    //public class IndiamartModelClass
    //{

    //    public string Indiamart_Id { get; set; }
    //    public string Rn { get; set; }
    //    public string QUERY_ID { get; set; }
    //    public string QTYPE { get; set; }
    //    public string SENDERNAME { get; set; }
    //    public string SENDEREMAIL { get; set; }
    //    public string SUBJECT { get; set; }
    //    public string DATE_RE { get; set; }
    //    public string DATE_R { get; set; }
    //    public string DATE_TIME_RE { get; set; }
    //    public string GLUSR_USR_COMPANYNAME { get; set; }
    //    public string READ_STATUS { get; set; }
    //    public string SENDER_GLUSR_USR_ID { get; set; }
    //    public string MOB { get; set; }
    //    public string COUNTRY_FLAG { get; set; }
    //    public string QUERY_MODID { get; set; }
    //    public string LOG_TIME { get; set; }
    //    public string QUERY_MODREFID { get; set; }
    //    public string DIR_QUERY_MODREF_TYPE { get; set; }
    //    public string ORG_SENDER_GLUSR_ID { get; set; }
    //    public string ENQ_MESSAGE { get; set; }
    //    public string ENQ_ADDRESS { get; set; }
    //    public string ENQ_CALL_DURATION { get; set; }
    //    public string ENQ_RECEIVER_MOB { get; set; }
    //    public string ENQ_CITY { get; set; }
    //    public string ENQ_STATE { get; set; }
    //    public string PRODUCT_NAME { get; set; }
    //    public string COUNTRY_ISO { get; set; }
    //    public string EMAIL_ALT { get; set; }
    //    public string MOBILE_ALT { get; set; }
    //    public string PHONE { get; set; }
    //    public string PHONE_ALT { get; set; }
    //    public string IM_MEMBER_SINCE { get; set; }
    //    public string TOTAL_COUNT { get; set; }

    //}

    public class IndiamartModelClassKey
    {
        public Int32 CODE { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public Int32 TOTAL_RECORDS { get; set; }
        public List<IndiamartModelClass> RESPONSE { get; set; }

    }
    public class IndiamartModelClass
    {
        public string Indiamart_Id { get; set; }
        public string UNIQUE_QUERY_ID { get; set; }
        public string QUERY_TYPE { get; set; }
        public string QUERY_TIME { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_MOBILE { get; set; }
        public string SENDER_EMAIL { get; set; }
        public string SENDER_COMPANY { get; set; }
        public string SENDER_ADDRESS { get; set; }
        public string SENDER_CITY { get; set; }
        public string SENDER_STATE { get; set; }
        public string SENDER_COUNTRY_ISO { get; set; }
        public string SENDER_MOBILE_ALT { get; set; }
        public string SENDER_EMAIL_ALT { get; set; }
        public string QUERY_PRODUCT_NAME { get; set; }
        public string QUERY_MESSAGE { get; set; }
        public string CALL_DURATION { get; set; }
        public string RECEIVER_MOBILE { get; set; }

    }
    // End of Rev 1.0

    public class IndiamartModelErrorClass
    {
        public string MobileNo { get; set; }
        public string ErrorMessage { get; set; }
        public string jsonError { get; set; }

        public List<IndiamartModelClass> Indiamart { get; set; }

    }
    public class XmlConversion
    {
        #region ******************************************** Xml Conversion  ********************************************
        public static string ConvertToXml<T>(List<T> table, int metaIndex = 0)
        {
            XmlDocument ChoiceXML = new XmlDocument();
            ChoiceXML.AppendChild(ChoiceXML.CreateElement("root"));
            Type temp = typeof(T);

            foreach (var item in table)
            {
                XmlElement element = ChoiceXML.CreateElement("data");

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    element.AppendChild(ChoiceXML.CreateElement(pro.Name)).InnerText = Convert.ToString(item.GetType().GetProperty(pro.Name).GetValue(item, null));
                }
                ChoiceXML.DocumentElement.AppendChild(element);
            }

            return ChoiceXML.InnerXml.ToString();
        }


        #endregion

    }


}