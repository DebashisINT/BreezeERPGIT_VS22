using DataAccessLayer;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Master
{
    public partial class EmployeeSyncList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.v_CustomerMasterSyncLists
                    orderby d.cnt_Id descending
                    select d;
            e.QueryableSource = q;
        }

        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
            string apiUrl = weburl+"ShopRegisterPortal/CustomerSyncinShop";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(Session["UserID"]);
            RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
            DataTable dt = new DataTable();
            List<object> QuoList = GrdEmployee.GetSelectedFieldValues("cnt_Id");
            foreach (object Quo in QuoList)
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc.AddPara("@ACTION", "CustomerDetails");
                proc.AddPara("@ContactID", Quo);
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {

                    DateTime date1 = DateTime.Parse("1970-01-01");
                    DateTime date2 = System.DateTime.Now;
                    var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                    empDtls.session_token = "zksjfhjsdjkskjdh";
                    empDtls.user_id = Convert.ToString(378);
                    empDtls.shop_name = Convert.ToString(dt.Rows[0]["PartyName"]);
                    empDtls.address = Convert.ToString(dt.Rows[0]["ADDRESS1"]);
                    empDtls.pin_code = Convert.ToString(dt.Rows[0]["PinCode"]);
                    empDtls.shop_lat = Convert.ToString(dt.Rows[0]["PartyLocationLat"]);
                    empDtls.shop_long = Convert.ToString(dt.Rows[0]["PartyLocationLong"]);
                    empDtls.owner_name = Convert.ToString(dt.Rows[0]["Owner"]);
                    empDtls.owner_contact_no = Convert.ToString(dt.Rows[0]["Contact"]);
                    empDtls.owner_email = Convert.ToString(dt.Rows[0]["Email"]);
                    empDtls.type = Convert.ToInt32(dt.Rows[0]["Type"]);
                    empDtls.dob = Convert.ToString(dt.Rows[0]["DOB"]);
                    empDtls.date_aniversary = Convert.ToString(dt.Rows[0]["Anniversary"]);
                    empDtls.shop_id = Convert.ToString(dt.Rows[0]["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                    empDtls.added_date = Convert.ToString(System.DateTime.Now);
                    empDtls.assigned_to_pp_id = "";
                    empDtls.assigned_to_dd_id = "";
                    empDtls.EntityCode = Convert.ToString(dt.Rows[0]["PartyCode"]);
                    empDtls.Entity_Location = Convert.ToString(dt.Rows[0]["Location"]);
                    empDtls.Alt_MobileNo = Convert.ToString(dt.Rows[0]["AlternateContact"]);
                    //empDtls.State_ID = Convert.ToString(dt.Rows[0]["State"]);
                    empDtls.Entity_Status = Convert.ToString(dt.Rows[0]["Status"]);
                    empDtls.Entity_Type = Convert.ToString(dt.Rows[0]["EntityCategory"]);
                    empDtls.ShopOwner_PAN = Convert.ToString(dt.Rows[0]["OwnerPAN"]);
                    empDtls.ShopOwner_Aadhar = Convert.ToString(dt.Rows[0]["OwnerAadhaar"]);
                    empDtls.Remarks = Convert.ToString(dt.Rows[0]["Remarks"]);
                    empDtls.AreaId = Convert.ToString(dt.Rows[0]["Area"]);
                    empDtls.CityId = Convert.ToString(dt.Rows[0]["District"]);
                    empDtls.Entered_by = Convert.ToString(userid);
                    empDtls.retailer_id = Convert.ToString("0");
                    empDtls.dealer_id = Convert.ToString("0");
                    empDtls.entity_id = Convert.ToString("0");
                    empDtls.party_status_id = Convert.ToString(dt.Rows[0]["PartyStatus"]);
                    empDtls.beat_id = Convert.ToString(dt.Rows[0]["GroupBeat"]);
                    empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);
                }

                String Status = "Failed";
                String FailedReason = "";
                //String Data = Convert.ToString(empDtls);
                string data = JsonConvert.SerializeObject(empDtls);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                //byte[] fileBytes = new byte[1];
                //var fileContent = new StreamContent(null);
                form.Add(new StringContent(data), "data");
                //form.Add(emp, "shop_image", null);
                var result = httpClient.PostAsync(apiUrl, form).Result;

                oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);
                //try
                //{
                //using (WebClient webClient = new WebClient())
                //{
                //    webClient.BaseAddress = "http://3.7.30.86:82/api/";
                //    var url = "http://3.7.30.86:82/api/AddShop/RegisterShop";
                //    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                //    string data = JsonConvert.SerializeObject(empDtls);
                //    var response = webClient.UploadString(url, data);
                //    oview = JsonConvert.DeserializeObject<RegisterShopOutput>(response);

                //    oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);
                //}
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}  

                //string result1 = JsonConvert.SerializeObject(result.Content.ReadAsStringAsync().Result);



                if (Convert.ToString(oview.status) == "200")
                {
                    Status = "Success";
                }
                else if (Convert.ToString(oview.status) == "202")
                {
                    //Status = "Success";
                    FailedReason = "Customer Name Not found";
                }
                else if (Convert.ToString(oview.status) == "203")
                {
                    //Status = "Success";
                    FailedReason = "Entity Code not found";
                }
                else if (Convert.ToString(oview.status) == "204")
                {
                    //Status = "Success";
                    FailedReason = "Owner Name Not found";
                }
                else if (Convert.ToString(oview.status) == "205")
                {
                    //Status = "Success";
                    FailedReason = "Customer Address not found";
                }
                else if (Convert.ToString(oview.status) == "206")
                {
                    //Status = "Success";
                    FailedReason = "Pin Code not found";
                }
                else if (Convert.ToString(oview.status) == "207")
                {
                    //Status = "Success";
                    FailedReason = "Customer Contact number not found";
                }
                else if (Convert.ToString(oview.status) == "208")
                {
                    //Status = "Success";
                    FailedReason = "User or session token not matched";
                }
                else if (Convert.ToString(oview.status) == "209")
                {
                    //Status = "Success";
                    FailedReason = "Duplicate Customer Id or contact number";
                }
                else if (Convert.ToString(oview.status) == "210")
                {
                    //Status = "Success";
                    FailedReason = "Duplicate contact number";
                }

                ProcedureExecute proc1 = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc1.AddPara("@ACTION", "SyncLog");
                proc1.AddPara("@ContactID", Quo);
                proc1.AddPara("@CustomerName", Convert.ToString(dt.Rows[0]["PartyName"]));
                proc1.AddPara("@CustomerAddress", Convert.ToString(dt.Rows[0]["ADDRESS1"]));
                proc1.AddPara("@CustomerPhone", Convert.ToString(dt.Rows[0]["Contact"]));
                proc1.AddPara("@SyncBy", userid);
                proc1.AddPara("@Status", Status);
                proc1.AddPara("@FailedReason", FailedReason);
                proc1.AddPara("@Shop_Code", empDtls.shop_id);
                int i = proc1.RunActionQuery();
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "AfterSync()", true);
        }

        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeDetailsForSync");
            proc.AddPara("@ACTION", "CustomerSyncLog");
            DataTable dt = proc.GetTable();
            GvJvSearch.DataSource = dt;
        }

        public class RegisterShopInputPortal
        {
            public string session_token { get; set; }
            //[Required]
            public string user_id { get; set; }
            //[Required]
            public string shop_name { get; set; }
            //[Required]
            public string address { get; set; }
            //[Required]
            public string pin_code { get; set; }
            //[Required]
            public string shop_lat { get; set; }
            //[Required]
            public string shop_long { get; set; }
            //[Required]
            public string owner_name { get; set; }
            //[Required]
            public string owner_contact_no { get; set; }
            //[Required]
            public string owner_email { get; set; }
            public int? type { get; set; }
            public string dob { get; set; }
            public string date_aniversary { get; set; }
            public string shop_id { get; set; }
            public string added_date { get; set; }
            public string assigned_to_pp_id { get; set; }
            public string assigned_to_dd_id { get; set; }
            public string amount { get; set; }
            public Nullable<DateTime> family_member_dob { get; set; }
            public string director_name { get; set; }
            public string key_person_name { get; set; }
            public string phone_no { get; set; }
            public Nullable<DateTime> addtional_dob { get; set; }
            public Nullable<DateTime> addtional_doa { get; set; }
            public Nullable<DateTime> doc_family_member_dob { get; set; }
            public string specialization { get; set; }
            public string average_patient_per_day { get; set; }
            public string category { get; set; }
            public string doc_address { get; set; }
            public string doc_pincode { get; set; }
            public string is_chamber_same_headquarter { get; set; }
            public string is_chamber_same_headquarter_remarks { get; set; }
            public string chemist_name { get; set; }
            public string chemist_address { get; set; }
            public string chemist_pincode { get; set; }
            public string assistant_name { get; set; }
            public string assistant_contact_no { get; set; }
            public Nullable<DateTime> assistant_dob { get; set; }
            public Nullable<DateTime> assistant_doa { get; set; }
            public Nullable<DateTime> assistant_family_dob { get; set; }
            public string EntityCode { get; set; }
            public string Entity_Location { get; set; }
            public string Alt_MobileNo { get; set; }
            public string Entity_Status { get; set; }
            public string Entity_Type { get; set; }
            public string ShopOwner_PAN { get; set; }
            public string ShopOwner_Aadhar { get; set; }
            public string Remarks { get; set; }
            public string AreaId { get; set; }
            public string CityId { get; set; }
            public string Entered_by { get; set; }
            public string entity_id { get; set; }
            public string party_status_id { get; set; }
            public string retailer_id { get; set; }
            public string dealer_id { get; set; }
            public string beat_id { get; set; }
            public string IsServicePoint { get; set; }
        }

        public class RegisterShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
            public string session_token { get; set; }
        }

        protected void lnlSegmentSync_Click(object sender, EventArgs e)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
            string apiUrl = weburl+"ShopRegisterPortal/CustomerSyncinShop";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(Session["UserID"]);
            RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
            DataTable dt = new DataTable();
            List<object> QuoList = GrdEmployee.GetSelectedFieldValues("cnt_Id");
            foreach (object Quo in QuoList)
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_SegmentDetailsForSync");
                proc.AddPara("@ACTION", "Segment1Details");
                proc.AddPara("@ContactID", Quo);
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        String Status = "Failed";
                        String FailedReason = "";

                        if (Convert.ToString(item["IsCustomerSync"]) == "1")
                        {
                            DateTime date1 = DateTime.Parse("1970-01-01");
                            DateTime date2 = System.DateTime.Now;
                            var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                            empDtls.session_token = "zksjfhjsdjkskjdh";
                            empDtls.user_id = Convert.ToString(378);
                            empDtls.shop_name = Convert.ToString(item["PartyName"]);
                            empDtls.address = Convert.ToString(item["ADDRESS1"]);
                            empDtls.pin_code = Convert.ToString(item["PinCode"]);
                            empDtls.shop_lat = Convert.ToString(item["PartyLocationLat"]);
                            empDtls.shop_long = Convert.ToString(item["PartyLocationLong"]);
                            empDtls.owner_name = Convert.ToString(item["Owner"]);
                            empDtls.owner_contact_no = Convert.ToString(item["Contact"]);
                            empDtls.owner_email = Convert.ToString(item["Email"]);
                            empDtls.type = Convert.ToInt32(item["Type"]);
                            empDtls.dob = Convert.ToString(item["DOB"]);
                            empDtls.date_aniversary = Convert.ToString(item["Anniversary"]);
                            empDtls.shop_id = Convert.ToString(item["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                            empDtls.added_date = Convert.ToString(System.DateTime.Now);
                            empDtls.assigned_to_pp_id = Convert.ToString(item["assigned_to_pp_id"]); ;
                            empDtls.assigned_to_dd_id = Convert.ToString(item["assigned_to_dd_id"]); ;
                            empDtls.EntityCode = Convert.ToString(item["PartyCode"]);
                            empDtls.Entity_Location = Convert.ToString(item["Location"]);
                            empDtls.Alt_MobileNo = Convert.ToString(item["AlternateContact"]);
                            empDtls.Entity_Status = Convert.ToString(item["Status"]);
                            empDtls.Entity_Type = Convert.ToString(item["EntityCategory"]);
                            empDtls.ShopOwner_PAN = Convert.ToString(item["OwnerPAN"]);
                            empDtls.ShopOwner_Aadhar = Convert.ToString(item["OwnerAadhaar"]);
                            empDtls.Remarks = Convert.ToString(item["Remarks"]);
                            empDtls.AreaId = Convert.ToString(item["Area"]);
                            empDtls.CityId = Convert.ToString(item["District"]);
                            empDtls.Entered_by = Convert.ToString(userid);
                            empDtls.retailer_id = Convert.ToString("0");
                            empDtls.dealer_id = Convert.ToString("0");
                            empDtls.entity_id = Convert.ToString("0");
                            empDtls.party_status_id = Convert.ToString(item["PartyStatus"]);
                            empDtls.beat_id = Convert.ToString(item["GroupBeat"]);
                            empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);

                            string data = JsonConvert.SerializeObject(empDtls);

                            HttpClient httpClient = new HttpClient();
                            MultipartFormDataContent form = new MultipartFormDataContent();
                            //byte[] fileBytes = new byte[1];
                            //var fileContent = new StreamContent(null);
                            form.Add(new StringContent(data), "data");
                            //form.Add(emp, "shop_image", null);
                            var result = httpClient.PostAsync(apiUrl, form).Result;

                            oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);
                           // oview.status = "200";
                            if (Convert.ToString(oview.status) == "200")
                            {
                                Status = "Success";
                            }
                            else if (Convert.ToString(oview.status) == "202")
                            {
                                FailedReason = "Customer Name Not found";
                            }
                            else if (Convert.ToString(oview.status) == "203")
                            {
                                FailedReason = "Entity Code not found";
                            }
                            else if (Convert.ToString(oview.status) == "204")
                            {
                                FailedReason = "Owner Name Not found";
                            }
                            else if (Convert.ToString(oview.status) == "205")
                            {
                                FailedReason = "Customer Address not found";
                            }
                            else if (Convert.ToString(oview.status) == "206")
                            {
                                FailedReason = "Pin Code not found";
                            }
                            else if (Convert.ToString(oview.status) == "207")
                            {
                                FailedReason = "Customer Contact number not found";
                            }
                            else if (Convert.ToString(oview.status) == "208")
                            {
                                FailedReason = "User or session token not matched";
                            }
                            else if (Convert.ToString(oview.status) == "209")
                            {
                                FailedReason = "Duplicate Customer Id or contact number";
                            }
                            else if (Convert.ToString(oview.status) == "210")
                            {
                                FailedReason = "Duplicate contact number";
                            }
                        }
                        else
                        {
                            FailedReason = "Customer not Sync";
                        }


                        ProcedureExecute proc1 = new ProcedureExecute("PRC_SegmentDetailsForSync");
                        proc1.AddPara("@ACTION", "UpdateSegment");
                        proc1.AddPara("@ContactID", Quo);
                        proc1.AddPara("@SegemntName", Convert.ToString(item["PartyName"]));
                        proc1.AddPara("@SegemntAddress", Convert.ToString(item["ADDRESS1"]));
                        proc1.AddPara("@SegemntPhone", Convert.ToString(item["Contact"]));
                        proc1.AddPara("@SyncBy", userid);
                        proc1.AddPara("@Status", Status);
                        proc1.AddPara("@FailedReason", FailedReason);
                        proc1.AddPara("@Shop_Code", empDtls.shop_id);
                        proc1.AddPara("@SegmentID", Convert.ToString(item["ID"]));
                        proc1.AddPara("@SegmentCode", Convert.ToString(item["PartyCode"]));
                        proc1.AddPara("@InternalID", Convert.ToString(item["InternalID"]));
                        int i = proc1.RunActionQuery();
                    }
                }

                ProcedureExecute proc2 = new ProcedureExecute("PRC_SegmentDetailsForSync");
                proc2.AddPara("@ACTION", "Segment2Details");
                proc2.AddPara("@ContactID", Quo);
                dt = proc2.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        String Status = "Failed";
                        String FailedReason = "";

                        if (Convert.ToString(item["IsCustomerSync"]) == "1")
                        {
                            if (Convert.ToString(item["IsSeg1Sync"]) == "1")
                            {
                                DateTime date1 = DateTime.Parse("1970-01-01");
                                DateTime date2 = System.DateTime.Now;
                                var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                                empDtls.session_token = "zksjfhjsdjkskjdh";
                                empDtls.user_id = Convert.ToString(378);
                                empDtls.shop_name = Convert.ToString(item["PartyName"]);
                                empDtls.address = Convert.ToString(item["ADDRESS1"]);
                                empDtls.pin_code = Convert.ToString(item["PinCode"]);
                                empDtls.shop_lat = Convert.ToString(item["PartyLocationLat"]);
                                empDtls.shop_long = Convert.ToString(item["PartyLocationLong"]);
                                empDtls.owner_name = Convert.ToString(item["Owner"]);
                                empDtls.owner_contact_no = Convert.ToString(item["Contact"]);
                                empDtls.owner_email = Convert.ToString(item["Email"]);
                                empDtls.type = Convert.ToInt32(item["Type"]);
                                empDtls.dob = Convert.ToString(item["DOB"]);
                                empDtls.date_aniversary = Convert.ToString(item["Anniversary"]);
                                empDtls.shop_id = Convert.ToString(item["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                                empDtls.added_date = Convert.ToString(System.DateTime.Now);
                                empDtls.assigned_to_pp_id = Convert.ToString(item["assigned_to_pp_id"]); ;
                                empDtls.assigned_to_dd_id = Convert.ToString(item["assigned_to_dd_id"]); ;
                                empDtls.EntityCode = Convert.ToString(item["PartyCode"]);
                                empDtls.Entity_Location = Convert.ToString(item["Location"]);
                                empDtls.Alt_MobileNo = Convert.ToString(item["AlternateContact"]);
                                empDtls.Entity_Status = Convert.ToString(item["Status"]);
                                empDtls.Entity_Type = Convert.ToString(item["EntityCategory"]);
                                empDtls.ShopOwner_PAN = Convert.ToString(item["OwnerPAN"]);
                                empDtls.ShopOwner_Aadhar = Convert.ToString(item["OwnerAadhaar"]);
                                empDtls.Remarks = Convert.ToString(item["Remarks"]);
                                empDtls.AreaId = Convert.ToString(item["Area"]);
                                empDtls.CityId = Convert.ToString(item["District"]);
                                empDtls.Entered_by = Convert.ToString(userid);
                                empDtls.retailer_id = Convert.ToString("0");
                                empDtls.dealer_id = Convert.ToString("0");
                                empDtls.entity_id = Convert.ToString("0");
                                empDtls.party_status_id = Convert.ToString(item["PartyStatus"]);
                                empDtls.beat_id = Convert.ToString(item["GroupBeat"]);
                                empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);

                                string data = JsonConvert.SerializeObject(empDtls);

                                HttpClient httpClient = new HttpClient();
                                MultipartFormDataContent form = new MultipartFormDataContent();
                                //byte[] fileBytes = new byte[1];
                                //var fileContent = new StreamContent(null);
                                form.Add(new StringContent(data), "data");
                                //form.Add(emp, "shop_image", null);
                                var result = httpClient.PostAsync(apiUrl, form).Result;

                                oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                                //oview.status = "200";

                                if (Convert.ToString(oview.status) == "200")
                                {
                                    Status = "Success";
                                }
                                else if (Convert.ToString(oview.status) == "202")
                                {
                                    FailedReason = "Customer Name Not found";
                                }
                                else if (Convert.ToString(oview.status) == "203")
                                {
                                    FailedReason = "Entity Code not found";
                                }
                                else if (Convert.ToString(oview.status) == "204")
                                {
                                    FailedReason = "Owner Name Not found";
                                }
                                else if (Convert.ToString(oview.status) == "205")
                                {
                                    FailedReason = "Customer Address not found";
                                }
                                else if (Convert.ToString(oview.status) == "206")
                                {
                                    FailedReason = "Pin Code not found";
                                }
                                else if (Convert.ToString(oview.status) == "207")
                                {
                                    FailedReason = "Customer Contact number not found";
                                }
                                else if (Convert.ToString(oview.status) == "208")
                                {
                                    FailedReason = "User or session token not matched";
                                }
                                else if (Convert.ToString(oview.status) == "209")
                                {
                                    FailedReason = "Duplicate Customer Id or contact number";
                                }
                                else if (Convert.ToString(oview.status) == "210")
                                {
                                    FailedReason = "Duplicate contact number";
                                }
                            }
                            else
                            {
                                FailedReason = "Parent segment not Sync";
                            }
                        }
                        else
                        {
                            FailedReason = "Customer not Sync";
                        }


                        ProcedureExecute proc1 = new ProcedureExecute("PRC_SegmentDetailsForSync");
                        proc1.AddPara("@ACTION", "UpdateSegment");
                        proc1.AddPara("@ContactID", Quo);
                        proc1.AddPara("@SegemntName", Convert.ToString(item["PartyName"]));
                        proc1.AddPara("@SegemntAddress", Convert.ToString(item["ADDRESS1"]));
                        proc1.AddPara("@SegemntPhone", Convert.ToString(item["Contact"]));
                        proc1.AddPara("@SyncBy", userid);
                        proc1.AddPara("@Status", Status);
                        proc1.AddPara("@FailedReason", FailedReason);
                        proc1.AddPara("@Shop_Code", empDtls.shop_id);
                        proc1.AddPara("@SegmentID", Convert.ToString(item["ID"]));
                        proc1.AddPara("@SegmentCode", Convert.ToString(item["PartyCode"]));
                        proc1.AddPara("@InternalID", Convert.ToString(item["InternalID"]));
                        int i = proc1.RunActionQuery();
                    }
                }
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "AfterSegmentSync()", true);
        }

        protected void SegmentGrid_DataBinding(object sender, EventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SegmentDetailsForSync");
            proc.AddPara("@ACTION", "SegmentSyncLog");
            DataTable dt = proc.GetTable();
            SegmentGrid.DataSource = dt;
        }

        protected void GrdEmployee_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Green;
            }

            if (Convert.ToString(e.CellValue) == "Not Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Red;
            }
        }

        protected void SegmentStatusGrid_DataBinding(object sender, EventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SegmentDetailsForSync");
            proc.AddPara("@ACTION", "SegmentSyncStatus");
            proc.AddPara("@ContactID", hdnCustomerID.Value);
            DataTable dt = proc.GetTable();
            SegmentStatusGrid.DataSource = dt;
        }

        protected void SegmentStatusGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Green;
            }

            if (Convert.ToString(e.CellValue) == "Not Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Red;
            }
        }
    }
}