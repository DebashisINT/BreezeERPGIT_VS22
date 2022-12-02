using DataAccessLayer;
//using ERP.OMS.Management.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace EImport.Import.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class Master_Import : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorWithBranch(string SearchKey, string BranchID)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' AND
                                    cnt_internalId in (Select Ven_InternalId from tbl_master_VendorBranch_map Where branch_id in('" + BranchID + "','0'))) as tbl " +
                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";
                                
                DataTable dt = oDBEngine.GetDataTable(strQuery);

                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               id = dr["cnt_internalid"].ToString(),
                               Na = dr["Name"].ToString(),
                               UId = Convert.ToString(dr["uniquename"])
                           }).ToList();

            }

            return listVen;
        }
    }

    public class VendorModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
    }


}
