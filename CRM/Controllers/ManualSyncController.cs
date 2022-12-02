using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer;
using System.Web;
using System.Web.Mvc;
using System.Data;
using UtilityLayer;
using EntityLayer.CommonELS;
using CRM.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net;


namespace CRM.Controllers
{
    public class ManualSyncController : Controller
    {
         private Random _random = new Random();
        // GET: /ManualSync/
        public ActionResult Index()
        {
            TaskCreationClass task_creation = new TaskCreationClass();
            return View("~/Views/CRMS/ManualSync/ManualSyncView.cshtml");
        }
        [HttpPost]
        public JsonResult ImportIndiaMart(string data, string date)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            DateTime startdate = Convert.ToDateTime(date.ToString());
            string output = string.Empty;
            string strerrormessage = "";
            string mobileno = "";
            string Action = "";
            long userid = Convert.ToInt64(Session["userid"]);

            List<IndiamartModelErrorClass> idnMart = new List<IndiamartModelErrorClass>();



            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();

            // String con = Convert.ToString(Session["ERPConnection"]);
            String con = obj.GetDefaultConnectionStringWithoutSession();

            SqlConnection sqlcon = new SqlConnection(con);

            sqlcon.Open();

            //SqlCommand sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfiguration", sqlcon);
            SqlCommand sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfigurationImport", sqlcon);//Proc_Get_Vendor_CrmConfigurationImport
            DataTable dtobsql=new DataTable();
            sqlcmd.Parameters.Add("@vendorID", "1,4");
            sqlcmd.Parameters.Add("@Startdate", startdate);
            sqlcmd.Parameters.Add("@ReturnValue", SqlDbType.Char, 50);
            sqlcmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output; 
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dtobsql);
            output = (string)sqlcmd.Parameters["@ReturnValue"].Value;
            if (output.ToString().Trim() != "" && output.ToString().Trim() != "1")
            {
                SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                DataTable dtError = new DataTable();
                sqlcmderror.Parameters.Add("@Type", data);
                sqlcmderror.Parameters.Add("@Name", data);
                sqlcmderror.Parameters.Add("@Message", output);
                sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                sqlcmderror.Parameters.Add("@CreatedBy", userid);
                sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                sqlcmderror.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                daerror.Fill(dtError);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            //import in diamart log table
            SqlCommand sqlcmdimportintoindiamart = new SqlCommand("Proc_ImportLog", sqlcon);
            DataTable dtimportindiamartlog = new DataTable();
            sqlcmdimportintoindiamart.Parameters.Add("@Type", data);
            sqlcmdimportintoindiamart.Parameters.Add("@Name", data);
            sqlcmdimportintoindiamart.Parameters.Add("@DateToSearch", startdate);
            sqlcmdimportintoindiamart.Parameters.Add("@CreatedBy", userid);
            sqlcmdimportintoindiamart.Parameters.Add("@CreatedDate",DateTime.Now);
            sqlcmdimportintoindiamart.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter daimportintoindiamart = new SqlDataAdapter(sqlcmdimportintoindiamart);
            daimportintoindiamart.Fill(dtimportindiamartlog);
            //-------------------------

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
                                try{
                                var json = webClient.DownloadString(URL);
                                // Now parse with JSON.Net


                                JavaScriptSerializer ser = new JavaScriptSerializer();

                                List<IndiamartModelClass> indiamartlist = ser.Deserialize<List<IndiamartModelClass>>(json);
                                if (indiamartlist.Count > 0)
                                {
                                    Action = "Success";
                                    objErr.jsonError = "Success";
                                    objErr.ErrorMessage = Action;
                                    omodel = new List<IndiamartModelClass>();
                                    foreach (var s2 in indiamartlist)
                                    {
                                        string rand = GenerateRandomNo();
                                        omodel.Add(new IndiamartModelClass()
                                        {
                                            Indiamart_Id = "Indiamart" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),
                                            Rn = s2.Rn,
                                            QUERY_ID = s2.QUERY_ID,
                                            QTYPE = s2.QTYPE,
                                            SENDERNAME = s2.SENDERNAME,
                                            SENDEREMAIL = s2.SENDEREMAIL,
                                            SUBJECT = s2.SUBJECT,
                                            DATE_RE = s2.DATE_RE,
                                            DATE_R = s2.DATE_R,
                                            DATE_TIME_RE = s2.DATE_TIME_RE,
                                            GLUSR_USR_COMPANYNAME = s2.GLUSR_USR_COMPANYNAME,
                                            READ_STATUS = s2.READ_STATUS,
                                            SENDER_GLUSR_USR_ID = s2.SENDER_GLUSR_USR_ID,
                                            MOB = s2.MOB,
                                            COUNTRY_FLAG = s2.COUNTRY_FLAG,
                                            QUERY_MODID = s2.QUERY_MODID,
                                            LOG_TIME = s2.LOG_TIME,
                                            QUERY_MODREFID = s2.QUERY_MODREFID,
                                            DIR_QUERY_MODREF_TYPE = s2.DIR_QUERY_MODREF_TYPE,
                                            ORG_SENDER_GLUSR_ID = s2.ORG_SENDER_GLUSR_ID,
                                            ENQ_MESSAGE = s2.ENQ_MESSAGE,
                                            ENQ_ADDRESS = s2.ENQ_ADDRESS,
                                            ENQ_CALL_DURATION = s2.ENQ_CALL_DURATION,
                                            ENQ_RECEIVER_MOB = s2.ENQ_RECEIVER_MOB,
                                            ENQ_CITY = s2.ENQ_CITY,
                                            ENQ_STATE = s2.ENQ_STATE,
                                            PRODUCT_NAME = s2.PRODUCT_NAME,
                                            COUNTRY_ISO = s2.COUNTRY_ISO,
                                            EMAIL_ALT = s2.EMAIL_ALT,
                                            MOBILE_ALT = s2.MOBILE_ALT,
                                            PHONE = s2.PHONE,
                                            PHONE_ALT = s2.PHONE_ALT,
                                            IM_MEMBER_SINCE = s2.IM_MEMBER_SINCE,
                                            TOTAL_COUNT = s2.TOTAL_COUNT


                                        });

                                    }
                                    objErr.Indiamart = omodel;

                                }
                                else
                                {
                                    //Action = "Error";
                                    //strerrormessage = json.ToString();
                                    //objErr.ErrorMessage = Action;
                                    //objErr.jsonError = strerrormessage;
                                    //error tracking
                                    SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                                    DataTable dtError = new DataTable();
                                    sqlcmderror.Parameters.Add("@Type", data);
                                    sqlcmderror.Parameters.Add("@Name", data);
                                    sqlcmderror.Parameters.Add("@Message", "Data Not Found");
                                    sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                                    sqlcmderror.Parameters.Add("@CreatedBy", userid);
                                    sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                                    sqlcmderror.CommandType = CommandType.StoredProcedure;
                                    SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                                    daerror.Fill(dtError);
                                    //end error tracking
                                    return Json("Data Not Found", JsonRequestBehavior.AllowGet);

                                }
                            }
                                catch(Exception ex)
                                  {
                                     
                                     //error tracking
                                     SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                                     DataTable dtError = new DataTable();
                                     sqlcmderror.Parameters.Add("@Type", data);
                                     sqlcmderror.Parameters.Add("@Name", data);
                                     sqlcmderror.Parameters.Add("@Message", ex.Message);
                                     sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                                     sqlcmderror.Parameters.Add("@CreatedBy", userid);
                                     sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                                     sqlcmderror.CommandType = CommandType.StoredProcedure;
                                     SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                                     daerror.Fill(dtError);
                                      //end error tracking
                                     return Json(ex.Message, JsonRequestBehavior.AllowGet);
                                }
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


                        sqlcmd = new SqlCommand("Proc_Import_IndiaMart", sqlcon);
                        sqlcmd.Parameters.Add("@JsonXML", JsonXML);
                        sqlcmd.Parameters.Add("@MobileNo", omodel.MobileNo);
                        sqlcmd.Parameters.Add("@Action", Action);
                        sqlcmd.Parameters.Add("@Errortext", omodel.jsonError);
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
                    SqlCommand sqlcmdsuccss = new SqlCommand("Proc_Import_Success_Log", sqlcon);
                    DataTable dtsuccss = new DataTable();
                    sqlcmdsuccss.Parameters.Add("@Type", data);
                    sqlcmdsuccss.Parameters.Add("@Name", data);
                    sqlcmdsuccss.Parameters.Add("@Message", "Success");
                    sqlcmdsuccss.Parameters.Add("@DateToSearch", startdate);
                    sqlcmdsuccss.Parameters.Add("@CreatedBy", userid);
                    sqlcmdsuccss.Parameters.Add("@CreatedDate", DateTime.Now);
                    sqlcmdsuccss.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dasuccss = new SqlDataAdapter(sqlcmdsuccss);
                    dasuccss.Fill(dtsuccss);
                }
                else
                {
                    //Erroe tracking
                    SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                    DataTable dtError = new DataTable();
                    sqlcmderror.Parameters.Add("@Type", data);
                    sqlcmderror.Parameters.Add("@Name", data);
                    sqlcmderror.Parameters.Add("@Message", "Data Not Found");
                    sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                    sqlcmderror.Parameters.Add("@CreatedBy", userid);
                    sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                    sqlcmderror.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                    daerror.Fill(dtError);
                    return Json("Data Not Found", JsonRequestBehavior.AllowGet);
                    //end error tracking
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
                //error tracking
                SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                DataTable dtError = new DataTable();
                sqlcmderror.Parameters.Add("@Type", data);
                sqlcmderror.Parameters.Add("@Name", data);
                sqlcmderror.Parameters.Add("@Message", ex.Message);
                sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                sqlcmderror.Parameters.Add("@CreatedBy", userid);
                sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                sqlcmderror.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                daerror.Fill(dtError);
                //end error tracking
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
            //return Json(1);
        }
          [HttpPost]
        public JsonResult ImportTradeIndia(string data, string date)
        {
            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string output = string.Empty;
            DateTime startdate = Convert.ToDateTime(date.ToString());
            long userid = Convert.ToInt64(Session["userid"]);
            // String con = Convert.ToString(Session["ERPConnection"]);
            String con = obj.GetDefaultConnectionStringWithoutSession();
            SqlConnection sqlcon = new SqlConnection(con);

            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfigurationImport", sqlcon);
            sqlcmd.Parameters.Add("@vendorID", 2);
            sqlcmd.Parameters.Add("@Startdate", startdate);
            sqlcmd.Parameters.Add("@ReturnValue", SqlDbType.Char, 50);
            sqlcmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output; 
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            DataTable dtobsql = new DataTable();
            da.Fill(dtobsql);
            output = (string)sqlcmd.Parameters["@ReturnValue"].Value;
            if (output.ToString().Trim() != "" && output.ToString().Trim() != "1")
            {
                SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                DataTable dtError = new DataTable();
                sqlcmderror.Parameters.Add("@Type", data);
                sqlcmderror.Parameters.Add("@Name", data);
                sqlcmderror.Parameters.Add("@Message", output);
                sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                sqlcmderror.Parameters.Add("@CreatedBy", userid);
                sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                sqlcmderror.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                daerror.Fill(dtError);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            //import in diamart log table
            SqlCommand sqlcmdimportintoindiamart = new SqlCommand("Proc_ImportLog", sqlcon);
            DataTable dtimportindiamartlog = new DataTable();
            sqlcmdimportintoindiamart.Parameters.Add("@Type", data);
            sqlcmdimportintoindiamart.Parameters.Add("@Name", data);
            sqlcmdimportintoindiamart.Parameters.Add("@DateToSearch", startdate);
            sqlcmdimportintoindiamart.Parameters.Add("@CreatedBy", userid);
            sqlcmdimportintoindiamart.Parameters.Add("@CreatedDate", DateTime.Now);
            sqlcmdimportintoindiamart.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter daimportintoindiamart = new SqlDataAdapter(sqlcmdimportintoindiamart);
            daimportintoindiamart.Fill(dtimportindiamartlog);
            //-------------------------
            sqlcon.Close();
            string URL = "";

            if (dtobsql != null)
            {
                if (dtobsql.Rows.Count > 0)
                {
                    URL = Convert.ToString(dtobsql.Rows[1][0]);

                }

                // string URL = "https://www.tradeindia.com/utils/my_inquiry.html?userid=2008551&profile_id=2467957&key=aae74985801ae0f73584d3ebc6645112&from_date=2016-01-01&to_date=2016-12-31&limit=10&page_no=2";


            }
            if (URL != "")
            {
                using (var webClient = new System.Net.WebClient())
                {
                    try{
                    var json = webClient.DownloadString(URL);
                    // Now parse with JSON.Net




                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    List<TradeIndiaModelClass> tradeindialist;
                    model = new List<TradeIndiaModelClass>();

                    try
                    {
                        tradeindialist = ser.Deserialize<List<TradeIndiaModelClass>>(json);

                    }
                    catch (Exception e)
                    {
                        string js = new JavaScriptSerializer().Serialize(json);
                        //error tracking
                        SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                        DataTable dtError = new DataTable();
                        sqlcmderror.Parameters.Add("@Type", data);
                        sqlcmderror.Parameters.Add("@Name", data);
                        sqlcmderror.Parameters.Add("@Message", js);
                        sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                        sqlcmderror.Parameters.Add("@CreatedBy", userid);
                        sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                        sqlcmderror.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                        daerror.Fill(dtError);
                        //end error tracking
                        return Json(js, JsonRequestBehavior.AllowGet);
                    }

                    foreach (var s2 in tradeindialist)
                    {
                        string rand = GenerateRandomNo();
                        model.Add(new TradeIndiaModelClass()
                        {

                            TradeIndia_Id = "Tradeindia" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),

                            receiver_mobile = s2.receiver_mobile,
                            order_value_min = s2.order_value_min,
                            ago_time = s2.ago_time,
                            sender_co = s2.sender_co,
                            product_source = s2.product_source,
                            unread_res_cnt = s2.unread_res_cnt,
                            //att_data = s2.att_data,
                            generated = s2.generated,
                            receiver_name = s2.receiver_name,
                            generated_time = s2.generated_time,
                            receiver_uid = s2.receiver_uid,
                            responded = s2.responded,
                            inquiry_type = s2.inquiry_type,
                            view_status = s2.view_status,
                            QUERY_MODID = s2.QUERY_MODID,
                            product_name = s2.product_name,
                            pref_supp_location = s2.pref_supp_location,
                            generated_date = s2.generated_date,
                            sender_name = s2.sender_name,
                            month_slot = s2.month_slot,
                            sender_country = s2.sender_country,
                            message = s2.message,
                            product_userid = s2.product_userid,
                            rfi_id = s2.rfi_id,
                            sender_uid = s2.sender_uid,
                            attachment_cnt = s2.attachment_cnt,
                            sender_city = s2.sender_city,
                            receiver_co = s2.receiver_co,
                            subject = s2.subject,
                            sender_email = s2.sender_email,
                            quantity = s2.quantity,
                            sender_mobile = s2.sender_mobile,
                            sender = s2.sender,
                            sender_state = s2.sender_state,
                            product_id = s2.product_id
                        });

                    }

                    string JsonXML = XmlConversion.ConvertToXml(model, 0);

                    sqlcon.Open();
                    sqlcmd = new SqlCommand("Proc_Import_TradeIndia", sqlcon);
                    sqlcmd.Parameters.Add("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {

                    }
                    //Track Status
                    SqlCommand sqlcmdsuccss = new SqlCommand("Proc_Import_Success_Log", sqlcon);
                    DataTable dtsuccss = new DataTable();
                    sqlcmdsuccss.Parameters.Add("@Type", data);
                    sqlcmdsuccss.Parameters.Add("@Name", data);
                    sqlcmdsuccss.Parameters.Add("@Message","Success");
                    sqlcmdsuccss.Parameters.Add("@DateToSearch", startdate);
                    sqlcmdsuccss.Parameters.Add("@CreatedBy", userid);
                    sqlcmdsuccss.Parameters.Add("@CreatedDate", DateTime.Now);
                    sqlcmdsuccss.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dasuccss = new SqlDataAdapter(sqlcmdsuccss);
                    dasuccss.Fill(dtsuccss);
                    return Json("Success", JsonRequestBehavior.AllowGet);
                    //End Track Status

                }
                    catch (Exception ex)
                    {
                        //error tracking
                        SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                        DataTable dtError = new DataTable();
                        sqlcmderror.Parameters.Add("@Type", data);
                        sqlcmderror.Parameters.Add("@Name", data);
                        sqlcmderror.Parameters.Add("@Message", ex.Message);
                        sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                        sqlcmderror.Parameters.Add("@CreatedBy", userid);
                        sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                        sqlcmderror.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter daerror = new SqlDataAdapter(sqlcmdimportintoindiamart);
                        daerror.Fill(dtError);
                        //end error tracking
                     return Json(ex.Message, JsonRequestBehavior.AllowGet);
                 }
               }
            }
            else
            {
                //error tracking
                SqlCommand sqlcmderror = new SqlCommand("Proc_Import_Error_Log", sqlcon);
                DataTable dtError = new DataTable();
                sqlcmderror.Parameters.Add("@Type", data);
                sqlcmderror.Parameters.Add("@Name", data);
                sqlcmderror.Parameters.Add("@Message", "Data Not Found");
                sqlcmderror.Parameters.Add("@DateToSearch", startdate);
                sqlcmderror.Parameters.Add("@CreatedBy", userid);
                sqlcmderror.Parameters.Add("@CreatedDate", DateTime.Now);
                sqlcmderror.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter daerror = new SqlDataAdapter(sqlcmderror);
                daerror.Fill(dtError);
                //end error tracking
                return Json("Data Not Found", JsonRequestBehavior.AllowGet);
            }
        }
        
        public class select
        {
            public string Select { get; set; }
        }
        [HttpPost]
        public JsonResult Bindlabel()
        {
            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();
            String con = obj.GetDefaultConnectionStringWithoutSession();
            SqlConnection sqlcon = new SqlConnection(con);
            SqlDataAdapter da = new SqlDataAdapter("select TOP 1  convert(varchar,[CreatedDate],0)  DateToSearch  from Import_Manual_Log Order By ID desc", con);
            DataTable dt=new DataTable();
            da.Fill(dt);
            string label;
            if(dt.Rows.Count>0)
            {
            label=dt.Rows[0]["DateToSearch"].ToString();
            }
            else
            {
                label = "No Record Found";
            }
            return Json(label);
        }
        public List<IndiamartModelClass> omodel { get; set; }
        public List<TradeIndiaModelClass> model { get; set; }
        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }
    }
}