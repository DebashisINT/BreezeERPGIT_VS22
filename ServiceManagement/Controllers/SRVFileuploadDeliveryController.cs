using BusinessLogicLayer.MenuBLS;
using EntityLayer.MenuHelperELS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ServiceManagement.Controllers
{
    public class SRVFileuploadDeliveryController : Controller
    {
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AttachmentDocumentAddUpdate()
        {
            Boolean Success = false;
            try
            {
                if (Request.Files.Count > 0)
                {
                    string folderid = "";
                    string path = String.Empty;
                    int year = objEngine.GetDate().Year;
                    HttpFileCollectionBase files = Request.Files;

                    var obj = Request.Form;

                    string docFileName = Convert.ToString(obj["docFileName"]);
                    Int32 CreateUser = Int32.Parse(Convert.ToString(Session["userid"]));

                    for (int i = 0; i < files.Count; i++)
                    {
                        string CreateDate = Convert.ToDateTime(objEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                        String FileName = String.Empty;
                        if (docFileName=="STBReturnRequisition")
                        {
                            folderid = "STBReturnRequisition";
                        }
                        else
                        {
                            folderid = "DeliveryFile";
                        }                        
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            FileName = fname;
                        }
                        else
                        {
                            fname = file.FileName;
                            FileName = fname;
                        }

                        // Get the complete folder path and store the file inside it.
                        path = Server.MapPath("~/Documents/");
                        string fulpath = path + "\\" + folderid;
                        if (!Directory.Exists(fulpath))
                        {
                            Directory.CreateDirectory(fulpath);
                        }

                        //fulpath = fulpath + "\\" + year;
                        //if (!Directory.Exists(fulpath))
                        //{
                        //    Directory.CreateDirectory(fulpath);
                        //}

                        //fulpath = fulpath + "\\" + "1";
                        //if (!Directory.Exists(fulpath))
                        //{
                        //    Directory.CreateDirectory(fulpath);
                        //}
                        fname = Path.Combine(fulpath, fname);
                        file.SaveAs(fname);
                        Success = true;
                    }
                }
            }
            catch { }
            return Json(Success);
        }


        [HttpPost]
        public JsonResult DeviceNumberDetails(String DeviceNumber)
        {
            DeviceDetails entDtls = new DeviceDetails();
            String ServcEntityUser = ConfigurationManager.AppSettings["ServcEntityUser"];
            String ServcEntityPass = ConfigurationManager.AppSettings["ServcEntityPass"];
            // <ITEM_CODE>CHA061170C</ITEM_CODE>   Model
            //<ITEM_DESCR>NSTV-CHA061170C-HD</ITEM_DESCR>
            //<PROV_ATTRIBUTE>000083060029CE14</PROV_ATTRIBUTE>
            //<SERIAL_NUMBER>000083060029CE14</SERIAL_NUMBER>
            //<ENTITY_CODE>J01KC158</ENTITY_CODE>
            //<CUSTOMER_NBR>0</CUSTOMER_NBR>
            //<MOBILE_PHONE>0</MOBILE_PHONE>

            //Rev 24465
            ////Creating Web Service reference object  
            //GTPLService.KolWebsiteSoapClient objPayRef = new GTPLService.KolWebsiteSoapClient();

            ////calling and storing web service output into the variable  
            //DataSet dsDeviceDetails = null;

            //DataSet EntityCodeds = objPayRef.GetSTB(DeviceNumber, ServcEntityUser, ServcEntityPass);
            //if (EntityCodeds != null && EntityCodeds.Tables[0] != null && EntityCodeds.Tables[0].Rows.Count > 0)
            //{
            //    String entity_code = EntityCodeds.Tables[0].Rows[0]["ENTITY_CODE"].ToString();
            //    String ITEM_CODE = EntityCodeds.Tables[0].Rows[0]["ITEM_CODE"].ToString();

            //    dsDeviceDetails = objPayRef.GetEntityDetail(entity_code, ServcEntityUser, ServcEntityPass);

            //    if (dsDeviceDetails != null && dsDeviceDetails.Tables[0] != null && dsDeviceDetails.Tables[0].Rows.Count > 0)
            //    {
            //        entDtls.Model = ITEM_CODE;
            //        entDtls.EntityCode = dsDeviceDetails.Tables[0].Rows[0]["ENTITY_CODE"].ToString();
            //        entDtls.NetworkName = dsDeviceDetails.Tables[0].Rows[0]["NETWORK_NAME"].ToString();
            //        entDtls.ContactPerson = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_PERSON"].ToString();
            //        entDtls.ContactNumber = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_NO"].ToString();
            //    }

            //}


            ////returning josn result  
            //return Json(entDtls, JsonRequestBehavior.AllowGet);

            var url = "https://oneims.gtplkcbpl.com/api/get_stb_details";
            string _ContentType = "application/json";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            client.DefaultRequestHeaders.Add("token-auth", "C4XVsf4nGm3EAcQZPo4FSAqCxz5BJcoxZy");
            client.DefaultRequestHeaders.Add("USER", "amit_podder");
            client.DefaultRequestHeaders.Add("PASSWORD", "#APIStore$@2098");

            var data = new Dictionary<string, string>
                {
               {"stb_no", DeviceNumber}
              
                };
            var content = new FormUrlEncodedContent(data);
            var response = client.PostAsync(url, content).Result;
            var content1 = response.Content.ReadAsStringAsync().Result;


            RootMain newdat = JsonConvert.DeserializeObject<RootMain>(content1);

            DataTable dt = new DataTable();
            dt = ObjectToData(newdat.data);
           
          

            
            //DataTable dt = ConvertJsonToDatatable(content1);
     
            //calling and storing web service output into the variable  
            DataSet dsDeviceDetails = null;
           
            DataSet EntityCodeds = new DataSet();
            EntityCodeds.Tables.Add(dt);
            if (EntityCodeds != null && EntityCodeds.Tables[0] != null && EntityCodeds.Tables[0].Rows.Count > 0)
            {
                String entity_code = EntityCodeds.Tables[0].Rows[0]["ENTITY_CODE"].ToString();
                String ITEM_CODE = EntityCodeds.Tables[0].Rows[0]["ITEM_CODE"].ToString();

                dsDeviceDetails = EntityCodeDetails(entity_code);

                if (dsDeviceDetails != null && dsDeviceDetails.Tables[0] != null && dsDeviceDetails.Tables[0].Rows.Count > 0)
                {
                    entDtls.Model = ITEM_CODE;
                    //Rev 24465
                    //entDtls.EntityCode = dsDeviceDetails.Tables[0].Rows[0]["ENTITY_CODE"].ToString();
                    entDtls.EntityCode = entity_code;
                    //End Rev 24465
                    entDtls.NetworkName = dsDeviceDetails.Tables[0].Rows[0]["NETWORK_NAME"].ToString();
                    entDtls.ContactPerson = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_PERSON"].ToString();
                    entDtls.ContactNumber = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_NO"].ToString();
                }

            }

            //returning josn result  
            return Json(entDtls, JsonRequestBehavior.AllowGet);
            //REv End 24465

        }

        //Rev 24465
    protected DataTable ConvertJsonToDatatable(string jsonString)
    {
        DataTable dt = new DataTable();
        //strip out bad characters
        string[] jsonParts = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
        //hold column names
        List<string> dtColumns = new List<string>();
        //get columns
        foreach (string jp in jsonParts)
        {
            //only loop thru once to get column names
            string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", "").Replace("\"data\":",""), ",");
            foreach (string rowData in propData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string n = rowData.Substring(0, idx - 1);
                    string v = rowData.Substring(idx + 1);
                    if (!dtColumns.Contains(n))
                    {
                        dtColumns.Add(n.Replace("\"", ""));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                }
            }
            break; // TODO: might not be correct. Was : Exit For
        }
        //build dt
        foreach (string c in dtColumns)
        {
            dt.Columns.Add(c);
        }
        //get table data
        foreach (string jp in jsonParts)
        {
            string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", "").Replace("\"data\":", ""), ",");
            DataRow nr = dt.NewRow();
            foreach (string rowData in propData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                    string v = rowData.Substring(idx + 1).Replace("\"", "");
                    nr[n] = v;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            dt.Rows.Add(nr);
        }
        return dt;
    }
    public DataSet EntityCodeDetails(String entity_code)
    {
        var url = "https://oneims.gtplkcbpl.com/api/get_entity_details";
        string _ContentType = "application/json";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
        client.DefaultRequestHeaders.Add("token-auth", "C4XVsf4nGm3EAcQZPo4FSAqCxz5BJcoxZy");
        client.DefaultRequestHeaders.Add("USER", "amit_podder");
        client.DefaultRequestHeaders.Add("PASSWORD", "#APIStore$@2098");

        var data = new Dictionary<string, string>
                {
               {"entity_code", entity_code}
              
                };
        var content = new FormUrlEncodedContent(data);



      
        var response = client.PostAsync(url, content).Result;
        var content1 = response.Content.ReadAsStringAsync().Result;
        var res = content1; //Json that has to be converted

        Root newdat = JsonConvert.DeserializeObject<Root>(res);

        DataTable dt = new DataTable();
        dt = ObjectToData(newdat.data);
        DataSet dsDeviceDetails = new DataSet();
        dsDeviceDetails.Tables.Add(dt);


        return dsDeviceDetails;
        
    }
    //End Rev 24465
        [HttpPost]
        public JsonResult entity_codeDetails(String entity_code)
        {
            DeviceDetails entDtls = new DeviceDetails();
            String ServcEntityUser = ConfigurationManager.AppSettings["ServcEntityUser"];
            String ServcEntityPass = ConfigurationManager.AppSettings["ServcEntityPass"];

            //Console.WriteLine(response);

            //string Uri = "";
            //var cl = new HttpClient();
            //cl.BaseAddress = new Uri(Uri);
            //int _TimeoutSec = 90;
            //cl.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            
            //cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            //var _CredentialBase64 = "RWRnYXJTY2huaXR0ZW5maXR0aWNoOlJvY2taeno=";
            //cl.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", _CredentialBase64));


            //Creating Web Service reference object  
            GTPLService.KolWebsiteSoapClient objPayRef = new GTPLService.KolWebsiteSoapClient();

            //calling and storing web service output into the variable  
            DataSet dsDeviceDetails = null;
            dsDeviceDetails = objPayRef.GetEntityDetail(entity_code, ServcEntityUser, ServcEntityPass);

            if (dsDeviceDetails != null && dsDeviceDetails.Tables[0] != null && dsDeviceDetails.Tables[0].Rows.Count > 0)
            {
                entDtls.EntityCode = dsDeviceDetails.Tables[0].Rows[0]["ENTITY_CODE"].ToString();
                entDtls.NetworkName = dsDeviceDetails.Tables[0].Rows[0]["NETWORK_NAME"].ToString();
                entDtls.ContactPerson = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_PERSON"].ToString();
                entDtls.ContactNumber = dsDeviceDetails.Tables[0].Rows[0]["CONTACT_NO"].ToString();
                entDtls.Das = dsDeviceDetails.Tables[0].Rows[0]["DAS_PHASE"].ToString();
            }
            //returning josn result  
            return Json(entDtls, JsonRequestBehavior.AllowGet);
        }
        //Rev 24465
        public static DataTable ObjectToData(object o)
        {
            DataTable dt = new DataTable("OutputData");
            if (o != null)
            {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
           
                o.GetType().GetProperties().ToList().ForEach(f =>
                {
                    try
                    {
                        f.GetValue(o, null);
                        dt.Columns.Add(f.Name, f.PropertyType);
                        dt.Rows[0][f.Name] = f.GetValue(o, null);
                    }
                    catch { }
                });
            }
            return dt;
        }
        //End Rev 24465
  
        public class DeviceDetails
        {
            public String Model { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNumber { get; set; }
            public String Das { get; set; }
        }

        //Rev 24465
        public class data
        {
            public string ENTITY_TYPE { get; set; }
            public string ENTITY_CODE { get; set; }
            public string NETWORK_NAME { get; set; }
            public string CONTACT_PERSON { get; set; }
            public string CONTACT_NO { get; set; }
            public string ADDRESS1 { get; set; }
            public string DAS_PHASE { get; set; }
            public string AREA { get; set; }
            public string ZONE { get; set; }
            public string CITY { get; set; }
            public string DISTRICT { get; set; }
            public string ZIPCODE { get; set; }
            public DateTime CREATED_DATE { get; set; }
            public string STATUS { get; set; }
            public string UNIT_NAME { get; set; }
            public string BRANCH_NAME { get; set; }
            public string PAN_NO { get; set; }
        }

        public class Root
        {
            public string message { get; set; }
            public string response { get; set; }
            public data data { get; set; }
        }


        public class DataMain
        {
            public string ITEM_CODE { get; set; }
            public string ITEM_DESCR { get; set; }
            public string PROV_ATTRIBUTE { get; set; }
            public string SERIAL_NUMBER { get; set; }
            public string ENTITY_CODE { get; set; }
            public string CUSTOMER_NBR { get; set; }
            public string MOBILE_PHONE { get; set; }
        }

        public class RootMain
        {
            public string message { get; set; }
            public string response { get; set; }
            public DataMain data { get; set; }
        }


        //End Rev 24465
    }
}