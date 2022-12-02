using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Master.UserControls
{
    /// <summary>
    /// Summary description for GroupContact
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class GroupContact : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                DataTable cust = oDBEngine.GetDataTable("select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_group_customerDetails  where  uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorWithOutBranch(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string strQuery = "";

                strQuery = @"Select top 10  * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name
                                    From tbl_master_contact Where cnt_contactType ='DV' and not exists(select grp_contactId from tbl_trans_group where grp_contactId=cnt_internalId)  and cnt_contactStatus<>3
                                    union
                                    Select cnt_internalid,IsNull(cnt_UCC,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name
                                    From tbl_master_contact Where cnt_contactType='TR' and not exists(select grp_contactId from tbl_trans_group where grp_contactId=cnt_internalId)
                                  ) as tbl " +
                               " Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"]),

                           }).ToList();
            }

            return listVen;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInfluencer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v__group_InfluencerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTransporter(string SearchKey)
        {
            List<TransporterModel> listVen = new List<TransporterModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                //                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                //                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                //                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map Where branch_id in('" + BranchID + "','0'))) as tbl " +
                //                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
                proc.AddVarcharPara("@Action", 100, "PopulateTransporterDetailWithOutBranch");
                //proc.AddVarcharPara("@branchId", 200, Convert.ToString(BranchID));
                proc.AddVarcharPara("@filter", 200, Convert.ToString(SearchKey));
                dt = proc.GetTable();
                //DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new TransporterModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["shortname"]),
                               type = Convert.ToString(dr["Type"])
                           }).ToList();
            }

            return listVen;
        }

    }
    public class CustomerModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }

    public class VendorModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
    }
    public class TransporterModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string type { get; set; }
    }
}