using CRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMSharingController : Controller
    {
        //
        // GET: /CRMSharing/
        public ActionResult DoSharing()
        {
            return PartialView("~/Views/CRMS/UserControl/PartialSharingTemplate.cshtml");
        }
        public ActionResult GetEntityDetails(string Module_Name, string Module_id)
        {
            crmSharing crmShareobj = new crmSharing();
            DataSet output = crmShareobj.GetEntityDetails(Module_Name, Module_id);

            crmShareobj.emails = (from DataRow dr in output.Tables[1].Rows
                                  select new crmShareEmail()
                                  {
                                      Entity_Email = dr["Entity_Email"].ToString(),
                                      Entity_Name = dr["Entity_Name"].ToString()
                                  }).ToList();

            crmShareobj.phones = (from DataRow dr in output.Tables[0].Rows
                                  select new crmShareSMS()
                 {
                     Entity_Name = dr["Entity_Name"].ToString(),
                     Entity_Phone = dr["Entity_Phone"].ToString(),
                 }).ToList();


            return Json(crmShareobj);
        }
        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult crmSendEmail(string toinput, string ccinput, string bccinput, string subjectinput, string bodyInput, string module_id, string module_name)
        {

            crmSharing crmShareobj = new crmSharing();

            try
            {
                HttpFileCollectionBase files = null;
                if (Request.Files.Count > 0)
                {
                    files = Request.Files;
                }
                
                //for (int i = 0; i < files.Count; i++)
                //{

                //    file = files[i];
                //    string fname;

                //    if (file.ContentLength == 0)
                //    {
                //        byte[] fileContent = new byte[file.ContentLength];
                //        file.InputStream.Read(fileContent, 0, file.ContentLength);
                //    }
                //}

                crmShareobj.SaveLog("Save", toinput, ccinput, bccinput, subjectinput, bodyInput, module_id, module_name);
                //string[] strTo = toinput.Split(',');

                //foreach (string str in strTo)
                //{
                //    HttpFileCollectionBase files = null;
                //    if (Request.Files.Count > 0)
                //    {
                //        files = Request.Files;
                //    }
                    crmShareobj.SendMail("Add", toinput, ccinput, bccinput, subjectinput, bodyInput, module_id, module_name, files);
                //}


                return Json("Email Sent Successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }






        [HttpPost]
        public ActionResult crmSMS(string MobileNo, string SmsContent, string module_id, string module_name)
        {

            crmSharing crmShareobj = new crmSharing();
                try
                {
                    string ErrorMsg = "";
                    string status = string.Empty;

                    string tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                    
                    string username = System.Configuration.ConfigurationSettings.AppSettings["username"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["password"];
                    string Provider = System.Configuration.ConfigurationSettings.AppSettings["Provider"];
                    string sender = System.Configuration.ConfigurationSettings.AppSettings["sender"];
                   
                    string con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                    string output = crmShareobj.SaveSMS("Save", module_id, MobileNo, SmsContent, module_name);

                    string[] mobiles = MobileNo.Split(',');

                    foreach (string mob in mobiles)
                    {
                        try
                        {
                            string res = crmShareobj.SmsSent(username, password, Provider, sender, mob, SmsContent, "Text");
                        }
                        catch
                        {
                            if (ErrorMsg == "")
                                ErrorMsg = mob;
                            else
                                ErrorMsg = ErrorMsg + "," + mob;
                        }
                    }

                    
                    if (ErrorMsg != "")
                    {

                    }


                    return Json("SMS Sent Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
           
    }
}