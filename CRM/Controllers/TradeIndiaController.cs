using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CRM.Controllers
{
    public class TradeIndiaController : Controller
    {
        List<TradeIndiaModelClass> model = new List<TradeIndiaModelClass>();

        // String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
        private Random _random = new Random();
        DBEngine objsql = new DBEngine();
        DataTable dtobsql = new DataTable();

        DataTable dt = new DataTable();
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        public JsonResult Index()
        {

            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();

            // String con = Convert.ToString(Session["ERPConnection"]);
            String con = obj.GetDefaultConnectionStringWithoutSession();
            SqlConnection sqlcon = new SqlConnection(con);

            sqlcon.Open();
            sqlcmd = new SqlCommand("Proc_Get_Vendor_CrmConfiguration", sqlcon);
            sqlcmd.Parameters.Add("@vendorID", 2);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dtobsql);
            sqlcon.Close();
            string URL = "";

            if (dtobsql != null)
            {
                if (dtobsql.Rows.Count > 0)
                {
                    URL = Convert.ToString(dtobsql.Rows[0][0]);

                }

                // string URL = "https://www.tradeindia.com/utils/my_inquiry.html?userid=2008551&profile_id=2467957&key=aae74985801ae0f73584d3ebc6645112&from_date=2016-01-01&to_date=2016-12-31&limit=10&page_no=2";


            }
            if (URL != "")
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URL);
                    // Now parse with JSON.Net




                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    List<TradeIndiaModelClass> tradeindialist;

                    try
                    {
                        tradeindialist = ser.Deserialize<List<TradeIndiaModelClass>>(json);

                    }
                    catch(Exception e)
                    {
                        string js = new JavaScriptSerializer().Serialize(json);
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

                }


                string JsonXML = XmlConversion.ConvertToXml(model, 0);

                sqlcon.Open();
                sqlcmd = new SqlCommand("Proc_Import_TradeIndia", sqlcon);
                sqlcmd.Parameters.Add("@JsonXML", JsonXML);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();

                if (dt.Rows.Count > 0)
                {

                }


                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Duplicate", JsonRequestBehavior.AllowGet);
            }
        }
        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }

    }

    public class TradeIndiaModelClass
    {

        public string TradeIndia_Id { get; set; }
        public string receiver_mobile { get; set; }
        public string order_value_min { get; set; }
        public string ago_time { get; set; }
        public string sender_co { get; set; }
        public string product_source { get; set; }
        public string unread_res_cnt { get; set; }
        //public string att_data { get; set; }
        public string generated { get; set; }
        public string receiver_name { get; set; }
        public string generated_time { get; set; }
        public string receiver_uid { get; set; }
        public string responded { get; set; }
        public string inquiry_type { get; set; }
        public string view_status { get; set; }
        public string QUERY_MODID { get; set; }
        public string product_name { get; set; }
        public string pref_supp_location { get; set; }
        public string generated_date { get; set; }
        public string sender_name { get; set; }
        public string month_slot { get; set; }
        public string sender_country { get; set; }
        public string message { get; set; }
        public string product_userid { get; set; }
        public string rfi_id { get; set; }
        public string sender_uid { get; set; }
        public string attachment_cnt { get; set; }
        public string sender_city { get; set; }
        public string receiver_co { get; set; }
        public string subject { get; set; }
        public string sender_email { get; set; }
        public string quantity { get; set; }
        public string sender_mobile { get; set; }
        public string sender { get; set; }
        public string sender_state { get; set; }
        public string product_id { get; set; }

    }


}