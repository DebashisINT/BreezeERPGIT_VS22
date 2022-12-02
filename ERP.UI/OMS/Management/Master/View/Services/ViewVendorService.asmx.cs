using DataAccessLayer;
using ERP.OMS.Management.Master.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Master.View.Services
{
    /// <summary>
    /// Summary description for ViewVendorService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ViewVendorService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorDetails(string id)
        {
            VendorDetails return_obj = new VendorDetails();

            GeneralVendor gc = new GeneralVendor();
            List<Correspondence> _Correspondence = new List<Correspondence>();
            List<Phone> _phone = new List<Phone>();
            List<Email> _Email = new List<Email>();
            List<Bank> _bank = new List<Bank>();
            List<Registration> _Registration = new List<Registration>();
            List<Membership> _Membership = new List<Membership>();
            List<Group> _group = new List<Group>();
            List<branch> _branch = new List<branch>();
            List<ContactList> _contact = new List<ContactList>();
            List<DocumentDetails> _documentdtls = new List<DocumentDetails>();
            //Rev Bapi
            List<product> _product = new List<product>();
            //Rev Bapi


            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_view_vendor_details");
                    proc.AddVarcharPara("@Action", 50, "get_vendor_details");
                    proc.AddVarcharPara("@ID", 50, id);
                    DataSet ds = proc.GetDataSet();
                    gc.legal_status = ds.Tables[0].Rows[0]["legal_status"].ToString().Trim();
                    gc.unique_id = ds.Tables[0].Rows[0]["unique_id"].ToString().Trim();
                    gc.vendor_name = ds.Tables[0].Rows[0]["vendor_name"].ToString().Trim();
                    gc.reffered_by = ds.Tables[0].Rows[0]["reffered_by"].ToString().Trim();
                    gc.status = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                    gc.dt_incorp = ds.Tables[0].Rows[0]["dt_incorp"].ToString().Trim();
                    gc.main_acnt = ds.Tables[0].Rows[0]["main_acnt"].ToString().Trim();
                    gc.gstin = ds.Tables[0].Rows[0]["gstin"].ToString().Trim();
                    gc.register = Convert.ToInt32(ds.Tables[0].Rows[0]["register"].ToString().Trim());
                    gc.cheque_print = ds.Tables[0].Rows[0]["cheque_print"].ToString().Trim();
                    gc.acnts_grp = ds.Tables[0].Rows[0]["acnts_grp"].ToString().Trim();
                    gc.vendor_type = ds.Tables[0].Rows[0]["vendor_type"].ToString().Trim();
                    gc.TDS_Deductees = ds.Tables[0].Rows[0]["TDS_Deductees"].ToString().Trim();
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
                                           distance = Convert.ToString(dr["distance"])
                                         


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
                    _branch = (from DataRow dr in ds.Tables[8].Rows
                               select new branch()
                               {
                                   branch_code = Convert.ToString(dr["branch_code"]),
                                   branch_description = Convert.ToString(dr["branch_description"]),




                               }).ToList();

                    //Rev Bapi
                    _product = (from DataRow dr in ds.Tables[11].Rows
                               select new product()
                               {
                                   product_code = Convert.ToString(dr["product_code"]),
                                   product_description = Convert.ToString(dr["product_description"]),




                               }).ToList();
                    //End Rev Bapi


                    _contact = (from DataRow dr in ds.Tables[9].Rows
                                select new ContactList()
                             {
                                 name = Convert.ToString(dr["name"]),
                                 cp_relationShip = Convert.ToString(dr["cp_relationShip"]),
                                 cp_designation = Convert.ToString(dr["cp_designation"]),
                                 phf_phoneNumber = Convert.ToString(dr["phf_phoneNumber"]),
                                 eml_email = Convert.ToString(dr["eml_email"]),
                                 status = Convert.ToString(dr["status"])


                             }).ToList();

                    _documentdtls = (from DataRow dr in ds.Tables[10].Rows
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
                return_obj.branch_dtls = _branch;
                return_obj.contact_person = _contact;
                return_obj.doc_details = _documentdtls;
                return_obj.product_dtls = _product;

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
        public object NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForVendorMaster";

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
        public string getVendorNumberSchemeSettings()
        {
            string NmberingSettingVal = "0";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='UniqueAutoNumberVendorMaster' and IsActive=1");
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

    }
    public class GeneralVendor
    {
        //public int InvoiceId { get; set; }
        public string legal_status { get; set; }
        public string unique_id { get; set; }
        public string vendor_name { get; set; }
        public string reffered_by { get; set; }
        public string status { get; set; }
        public string dt_incorp { get; set; }
        public string main_acnt { get; set; }
        public string gstin { get; set; }
        public int register { get; set; }
        public string cheque_print { get; set; }
        public string acnts_grp { get; set; }
        public string vendor_type { get; set; }
        public string TDS_Deductees { get; set; }

    }
    public class branch
    {
        public string branch_code { get; set; }
        public string branch_description { get; set; }
    }
    //rev bapi
    public class product
    {
        public string product_code { get; set; }
        public string product_description { get; set; }
    }
    //rev end bapi
    public class ContactList
    {
        public string name { get; set; }
        public string cp_relationShip { get; set; }
        public string cp_designation { get; set; }
        public string phf_phoneNumber { get; set; }
        public string eml_email { get; set; }
        public string status { get; set; }

    }
    public class DocumentDetails
    {
        public string doc_type { get; set; }
        public string doc_name { get; set; }
        public string doc_loc { get; set; }
        public string doc_source { get; set; }
        public string doc { get; set; }
    }

    public class VendorDetails
    {
        public string msg { get; set; }
        public GeneralVendor general { get; set; }
        public List<Correspondence> corspndnc_dtls { get; set; }
        public List<Phone> Phone_number { get; set; }
        public List<Email> email_dtls { get; set; }
        public List<Bank> bank_dtls { get; set; }
        public List<Registration> reg_dtls { get; set; }
        public List<Membership> membershp_dtls { get; set; }
        public List<Group> Group_dtls { get; set; }
        public List<branch> branch_dtls { get; set; }
        public List<ContactList> contact_person { get; set; }
        public List<DocumentDetails> doc_details { get; set; }
        //Rev Bapi
        public List<product> product_dtls { get; set; }
        //End Rev Bapi
    }
    public class NumberingDetails
    {

        public string Id { get; set; }
        public string SchemaName { get; set; }
    }

}
