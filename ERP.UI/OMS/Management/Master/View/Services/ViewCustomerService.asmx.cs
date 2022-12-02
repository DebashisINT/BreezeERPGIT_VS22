using BusinessLogicLayer.CommonBLS;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Master.Services
{
    /// <summary>
    /// Summary description for ViewCustomerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ViewCustomerService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerDetails(string id)
        {
            CustomerDetails return_obj = new CustomerDetails();

            GeneralCustomer gc = new GeneralCustomer();
            List<Correspondence> _Correspondence = new List<Correspondence>();
            List<Phone> _phone = new List<Phone>();
            List<Email> _Email = new List<Email>();
            List<Bank> _bank = new List<Bank>();
            List<Registration> _Registration = new List<Registration>();
            List<Membership> _Membership = new List<Membership>();
            List<Group> _group = new List<Group>();
            List<DocumentDetails> _documentdtls = new List<DocumentDetails>();


            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_view_customer_details");
                    proc.AddVarcharPara("@Action", 50, "get_customer_details");
                    proc.AddVarcharPara("@ID", 50, id);
                    DataSet ds = proc.GetDataSet();
                    gc.lgl_legalStatus = ds.Tables[0].Rows[0]["lgl_legalStatus"].ToString().Trim();
                    gc.cnt_IdType = ds.Tables[0].Rows[0]["cnt_IdType"].ToString().Trim();
                    gc.unique_id = ds.Tables[0].Rows[0]["unique_id"].ToString().Trim();
                    gc.fulll_name = ds.Tables[0].Rows[0]["fulll_name"].ToString().Trim();
                    gc.dob = ds.Tables[0].Rows[0]["dob"].ToString().Trim();
                    gc.nationality = ds.Tables[0].Rows[0]["nationality"].ToString().Trim();
                    gc.anniver_dt = ds.Tables[0].Rows[0]["anniver_dt"].ToString().Trim();
                    gc.gender = ds.Tables[0].Rows[0]["gender"].ToString().Trim();
                    gc.credit_hold = Convert.ToBoolean(ds.Tables[0].Rows[0]["credit_hold"].ToString().Trim());
                    gc.credit_days = ds.Tables[0].Rows[0]["credit_days"].ToString().Trim();
                    gc.credit_limit = ds.Tables[0].Rows[0]["credit_limit"].ToString().Trim();
                    gc.maritial_status = ds.Tables[0].Rows[0]["maritial_status"].ToString().Trim();
                    gc.status_type = ds.Tables[0].Rows[0]["status_type"].ToString().Trim();
                    gc.main_acnt = ds.Tables[0].Rows[0]["main_acnt"].ToString().Trim();
                    gc.gstin = ds.Tables[0].Rows[0]["gstin"].ToString().Trim();
                    gc.register = Convert.ToInt32(ds.Tables[0].Rows[0]["register"].ToString().Trim());

                    _Correspondence = (from DataRow dr in ds.Tables[1].Rows
                                       select new Correspondence()
                                           {
                                               c_person = Convert.ToString(dr["c_person"]),
                                               type = Convert.ToString(dr["type"]),
                                               addr1 = Convert.ToString(dr["addr1"]),
                                               addr2 = Convert.ToString(dr["addr2"]),
                                               addr3 = Convert.ToString(dr["addr3"]),
                                               country = Convert.ToString(dr["country"]),
                                               state = Convert.ToString(dr["state"]),
                                               city = Convert.ToString(dr["city"]),
                                               pin = Convert.ToString(dr["pin"]),
                                               phone = Convert.ToString(dr["phone"]),
                                               distance = Convert.ToString(dr["distance"]),
                                               email = Convert.ToString(dr["email"]),
                                               designation = Convert.ToString(dr["designation"])


                                           }).ToList();

                    _phone = (from DataRow dr in ds.Tables[2].Rows
                              select new Phone()
                                  {
                                      contact_person = Convert.ToString(dr["contact_person"]),
                                      designation = Convert.ToString(dr["designation"]),
                                      ph_type = Convert.ToString(dr["ph_type"]),
                                      ph_nmbr = Convert.ToString(dr["ph_nmbr"])



                                  }).ToList();

                    _Email = (from DataRow dr in ds.Tables[3].Rows
                              select new Email()
                              {
                                  type = Convert.ToString(dr["type"]),
                                  email_id = Convert.ToString(dr["email_id"]),




                              }).ToList();

                    _bank = (from DataRow dr in ds.Tables[4].Rows
                             select new Bank()
                              {
                                  Category = Convert.ToString(dr["Category"]),
                                  AccountType = Convert.ToString(dr["AccountType"]),
                                  AccountNumber = Convert.ToString(dr["AccountNumber"]),
                                  AccountName = Convert.ToString(dr["AccountName"]),
                                  BankName = Convert.ToString(dr["BankName"]),
                                  Branch = Convert.ToString(dr["Branch"]),
                                  MICR = Convert.ToString(dr["MICR"]),
                                  IFSCcode = Convert.ToString(dr["IFSCcode"]),
                                  IsPOA = Convert.ToString(dr["IsPOA"]),
                                  POAName = Convert.ToString(dr["POAName"])




                              }).ToList();

                    _Registration = (from DataRow dr in ds.Tables[5].Rows
                                     select new Registration()
                                     {
                                         type = Convert.ToString(dr["type"]),
                                         number = Convert.ToString(dr["number"]),
                                         place_issue = Convert.ToString(dr["place_issue"]),
                                         date_issue = Convert.ToString(dr["date_issue"]),
                                         valid_untill = Convert.ToString(dr["valid_untill"])
                                     }).ToList();

                    _Membership = (from DataRow dr in ds.Tables[6].Rows
                                   select new Membership()
                                   {
                                       prof_assoc = Convert.ToString(dr["prof_assoc"]),
                                       member_nmbr = Convert.ToString(dr["member_nmbr"]),
                                       membership_type = Convert.ToString(dr["membership_type"]),
                                       notes = Convert.ToString(dr["notes"]),
                                       member_expiry_dt = Convert.ToString(dr["member_expiry_dt"]),
                                       validity_type = Convert.ToString(dr["validity_type"])
                                   }).ToList();
                    _group = (from DataRow dr in ds.Tables[7].Rows
                              select new Group()
                              {
                                  group_name = Convert.ToString(dr["group_name"]),
                                  group_type = Convert.ToString(dr["group_type"])

                              }).ToList();

                    _documentdtls = (from DataRow dr in ds.Tables[8].Rows
                                     select new DocumentDetails()
                                     {
                                         doc_type = Convert.ToString(dr["doc_type"]),
                                         doc_name = Convert.ToString(dr["doc_name"]),
                                         doc_loc = Convert.ToString(dr["doc_loc"]),
                                         doc_source = Convert.ToString(dr["doc_source"]),
                                         doc = Convert.ToString(dr["doc"]),
                                     }).ToList();
                }

                return_obj.general = gc;
                return_obj.corspndnc_dtls = _Correspondence;
                return_obj.Phone_number = _phone;
                return_obj.email_dtls = _Email;
                return_obj.bank_dtls = _bank;
                return_obj.reg_dtls = _Registration;
                return_obj.membershp_dtls = _Membership;
                return_obj.Group_dtls = _group;
                return_obj.doc_details = _documentdtls;

                return_obj.msg = "ok";
            }
            catch (Exception ex)
            {
                return_obj.msg = ex.Message;
            }
            return return_obj;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getGroupmasterDetails()
        {
            List<GroupDetails> GrpDet = new List<GroupDetails>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_SalesCRM_Details");
                pro.AddVarcharPara("@Action", 500, "GroupCustomerDetails");

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new GroupDetails
                            {
                                Id = Convert.ToString(dr["Id"]),
                                Name = Convert.ToString(dr["Name"]),
                                //Type = Convert.ToString(dr["Type"])

                            }).ToList();

            }
            return GrpDet;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getAllServiceBranch()
        {
            List<GroupDetails> GrpDet = new List<GroupDetails>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_SalesCRM_Details");
                pro.AddVarcharPara("@Action", 500, "GetAllBranch");

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new GroupDetails
                          {
                              Id = Convert.ToString(dr["branch_id"]),
                              Name = Convert.ToString(dr["branch_description"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getVendorGroupmasterDetails()
        {
            List<GroupDetails> GrpDet = new List<GroupDetails>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_SalesCRM_Details");
                pro.AddVarcharPara("@Action", 500, "GroupVendorDetails");

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new GroupDetails
                          {
                              Id = Convert.ToString(dr["Id"]),
                              Name = Convert.ToString(dr["Name"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForCustomerMaster";

            List<NumberingDetails> GrpDet = new List<NumberingDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
                proc.AddVarcharPara("@Action", 100, actionqry);
                proc.AddVarcharPara("@userbranchlist", 4000, userbranchHierarchy);

                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new NumberingDetails
                          {
                              Id = Convert.ToString(dr["Id"]),
                              SchemaName = Convert.ToString(dr["SchemaName"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PopulateAllModule()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "PopulateAllModule";

            List<ModuleDetails> GrpDet = new List<ModuleDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                proc.AddVarcharPara("@action", 100, actionqry);
                

                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new ModuleDetails
                          {
                              Module_Id = Convert.ToString(dr["Module_Id"]),
                              Module_Name = Convert.ToString(dr["Module_Name"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }

        //Rev Bapi
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PopulateAllProject()
        {


            List<ProductDetails> GrpDet = new List<ProductDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ProductMasterMultiselectDataselect");
                proc.AddVarcharPara("@action", 100, "");


                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new ProductDetails
                          {
                              // Rev Sanchita
                              //Id = Convert.ToInt16(dr["Product_ID"]),
                              Id = Convert.ToInt64(dr["Product_ID"]),
                              // End of Rev Sanchita
                              Name = Convert.ToString(dr["Product"]),
                            
                          }).ToList();

            }
            return GrpDet;
        }


        public class ProductDetails
        {

            // Rev Sanchita
            //public int  Id { get; set; }
            public long Id { get; set; }
            // End of Rev Sanchita
            public string  Name { get; set; }
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getVendorNumberSchemeSettings()
        {
            string NmberingSettingVal = "0";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='AllowPreferredProducts' and IsActive=1");
            if (dt.Rows.Count == 1)
            {
                string AutoNumberAllow = Convert.ToString(dt.Rows[0]["Variable_Value"]);
                if (!String.IsNullOrEmpty(AutoNumberAllow))
                {
                    if (AutoNumberAllow == "Yes")
                    {
                        NmberingSettingVal = "1";
                    }
                    else
                    {
                        NmberingSettingVal = "0";
                    }
                }

            }
            return NmberingSettingVal;
        }
        //End Rev Bapi


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getCustomerNumberSchemeSettings()
        {
            string NmberingSettingVal = "0";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='UniqueAutoNumberCustomerMaster' and IsActive=1");
            if (dt.Rows.Count == 1)
            {
                string AutoNumberAllow = Convert.ToString(dt.Rows[0]["Variable_Value"]);
                if (!String.IsNullOrEmpty(AutoNumberAllow))
                {
                    if (AutoNumberAllow == "Yes")
                    {
                        NmberingSettingVal = "1";
                    }
                    else
                    {
                        NmberingSettingVal = "0";
                    }
                }

            }
            return NmberingSettingVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getEinvoiceActiveSettings()
        {
            string ActiveEInvoice = "0";
            BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();
            ActiveEInvoice = objmaster.GetSettings("ActiveEInvoice");
            return ActiveEInvoice;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getDocumentSegmentSettings()
        {
            string DocumentSegment = "0";
            BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();
            DocumentSegment = objmaster.GetSettings("DocumentSegment");
            return DocumentSegment;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDocumentSegment()
        {

            List<SegmentLayouDetails> GrpDet = new List<SegmentLayouDetails>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_ProductMaster_bindData");
                pro.AddVarcharPara("@Action", 500, "GetSegmentLayout");

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new SegmentLayouDetails
                          {
                              Id = Convert.ToString(dr["Id"]),
                              Name = Convert.ToString(dr["Name"]),
                              

                          }).ToList();

            }
            return GrpDet;


            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //DataTable DT = new DataTable();
            //DT.Rows.Clear();
            //ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            //proc.AddVarcharPara("@action", 20, "GetSegmentLayout");
            //DT = proc.GetTable();
            //List<string> obj = new List<string>();
            //foreach (DataRow dr in DT.Rows)
            //{
            //    obj.Add(Convert.ToString(dr["ID"]) + "|" + Convert.ToString(dr["NAME"]));
            //}
            //return obj;
        }







        public class SegmentLayouDetails
        {
            public string Id { get; set; }
            public string Name { get; set; }          
        }

        public class GeneralCustomer
        {
            //public int InvoiceId { get; set; }
            public string lgl_legalStatus { get; set; }
            public string cnt_IdType { get; set; }
            public string unique_id { get; set; }
            public string fulll_name { get; set; }
            public string dob { get; set; }
            public string nationality { get; set; }
            public string anniver_dt { get; set; }
            public string gender { get; set; }
            public bool credit_hold { get; set; }
            public string credit_days { get; set; }
            public string credit_limit { get; set; }
            public string maritial_status { get; set; }
            public string status_type { get; set; }
            public string main_acnt { get; set; }
            public string gstin { get; set; }
            public int register { get; set; }

        }
        public class Correspondence
        {
            public string c_person { get; set; }
            public string type { get; set; }
            public string addr1 { get; set; }
            public string addr2 { get; set; }
            public string addr3 { get; set; }
            public string country { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string pin { get; set; }
            public string phone { get; set; }
            public string distance { get; set; }
            public string email { get; set; }
            public string designation { get; set; }
        }

        public class Email
        {
            public string type { get; set; }
            public string email_id { get; set; }

        }

        public class Bank
        {
            public string Category { get; set; }
            public string AccountType { get; set; }
            public string AccountNumber { get; set; }
            public string AccountName { get; set; }
            public string BankName { get; set; }
            public string Branch { get; set; }
            public string MICR { get; set; }
            public string IFSCcode { get; set; }
            public string IsPOA { get; set; }
            public string POAName { get; set; }
        }
        public class DocumentDetails
        {
            public string doc_type { get; set; }
            public string doc_name { get; set; }
            public string doc_loc { get; set; }
            public string doc_source { get; set; }
            public string doc { get; set; }
        }

        public class Phone
        {
            public string contact_person { get; set; }
            public string designation { get; set; }
            public string ph_type { get; set; }
            public string ph_nmbr { get; set; }
        }
        public class Registration
        {
            public string type { get; set; }
            public string number { get; set; }
            public string place_issue { get; set; }
            public string date_issue { get; set; }
            public string valid_untill { get; set; }

        }

        public class Membership
        {
            public string prof_assoc { get; set; }
            public string member_nmbr { get; set; }
            public string membership_type { get; set; }
            public string validity_type { get; set; }
            public string member_expiry_dt { get; set; }
            public string notes { get; set; }

        }
        public class Group
        {
            public string group_name { get; set; }
            public string group_type { get; set; }
        }

        public class CustomerDetails
        {
            public string msg { get; set; }
            public GeneralCustomer general { get; set; }
            public List<Correspondence> corspndnc_dtls { get; set; }
            public List<Phone> Phone_number { get; set; }
            public List<Email> email_dtls { get; set; }
            public List<Bank> bank_dtls { get; set; }
            public List<Registration> reg_dtls { get; set; }
            public List<Membership> membershp_dtls { get; set; }
            public List<Group> Group_dtls { get; set; }
            public List<DocumentDetails> doc_details { get; set; }
        }
        public class GroupDetails
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }


        public class NumberingDetails
        {

            public string Id { get; set; }
            public string SchemaName { get; set; }
        }

        public class ModuleDetails
        {

            public string Module_Id { get; set; }
            public string Module_Name { get; set; }
        }
    }
}
