using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class JustDialController : Controller
    {
        List<JustDialModelClass> omodel = new List<JustDialModelClass>();

        // String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
        private Random _random = new Random();


        [HttpGet]
        public ActionResult Index(JustDialModelClass model)
        {
            ReadWriteMasterDatabaseBL obj = new ReadWriteMasterDatabaseBL();
            try
            {
                // String con = Convert.ToString(Session["ERPConnection"]);
                String con = obj.GetDefaultConnectionStringWithoutSession();


                string rand = GenerateRandomNo();
                //omodel.Add(new JustDialModelClass()
                //{

                //    JustDial_Id = "JustDial" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid(),
                //    leadid = model.leadid,
                //    leadtype = model.leadtype,
                //    prefix = model.prefix,
                //    name = model.name,
                //    mobile = model.mobile,
                //    phone = model.phone,
                //    email = model.email,
                //    date = model.date,
                //    category = model.category,
                //    city = model.city,
                //    Area = model.Area,
                //    brancharea = model.brancharea,
                //    dncmobile = model.dncmobile,
                //    dncphone = model.dncphone,
                //    company = model.company,
                //    pincode = model.pincode,
                //    time = model.time,
                //    branchpin = model.branchpin,
                //    parentid = model.parentid
                //});





                string JsonXML = XmlConversion.ConvertToXml(omodel, 0);



                DataTable dt = new DataTable();
                // String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();

                sqlcmd = new SqlCommand("Proc_Import_JustDial", sqlcon);
                // sqlcmd.Parameters.Add("@JsonXML", JsonXML);
             //   sqlcmd.Parameters.Add("@JsonXML", JsonXML);

                sqlcmd.Parameters.Add("@JustDial_Id", "JustDial" + "_" + DateTime.Now.Ticks.ToString() + "_" + rand + "_" + Guid.NewGuid());
                sqlcmd.Parameters.Add("@leadid", model.leadid);
                sqlcmd.Parameters.Add("@leadtype", model.leadtype);
                sqlcmd.Parameters.Add("@prefix", model.prefix);
                sqlcmd.Parameters.Add("@name", model.name);
                sqlcmd.Parameters.Add("@mobile", model.mobile);
                sqlcmd.Parameters.Add("@phone", model.phone);
                sqlcmd.Parameters.Add("@email", model.email);
                sqlcmd.Parameters.Add("@date", model.date);
                sqlcmd.Parameters.Add("@category", model.category);
                sqlcmd.Parameters.Add("@city", model.city);
                sqlcmd.Parameters.Add("@Area", model.Area);
                sqlcmd.Parameters.Add("@brancharea", model.brancharea);
                sqlcmd.Parameters.Add("@dncmobile", model.dncmobile);
                sqlcmd.Parameters.Add("@dncphone", model.dncphone);
                sqlcmd.Parameters.Add("@company", model.company);
                sqlcmd.Parameters.Add("@pincode", model.pincode);
                sqlcmd.Parameters.Add("@time", model.time);
                sqlcmd.Parameters.Add("@branchpin", model.branchpin);
                sqlcmd.Parameters.Add("@parentid", model.parentid);


                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {

                }
                return Json("Success", JsonRequestBehavior.AllowGet);

            }

            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }
        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }





    }

    public class JustDialModelInputClass
    {
        //public string TokenID { get; set; }
        public List<JustDialModelClass> lstdetails { get; set; }
    }
    public class JustDialModelClass
    {
        public string JustDial_Id { get; set; }
        public string leadid { get; set; }
        public string leadtype { get; set; }
        public string prefix { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string date { get; set; }
        public string category { get; set; }
        public string city { get; set; }
        public string Area { get; set; }
        public string brancharea { get; set; }
        public string dncmobile { get; set; }
        public string dncphone { get; set; }
        public string company { get; set; }
        public string pincode { get; set; }
        public string time { get; set; }
        public string branchpin { get; set; }
        public string parentid { get; set; }


    }
}