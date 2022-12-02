using DataAccessLayer;
//using ERP.OMS.Management.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ServiceManagement.ServiceManagement.Transaction.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class Master : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetClassWiseProductJobsheet(string SearchKey, string ClassID)
        {
            List<ClassWiseProductModel> listcwiseProducts = new List<ClassWiseProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cwiseProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("PRC_PRODUCTSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@ProductClass", ClassID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cwiseProduct);


                cmd.Dispose();
                con.Dispose();

                listcwiseProducts = (from DataRow dr in cwiseProduct.Rows
                                     select new ClassWiseProductModel()
                                     {
                                         id = dr["ID"].ToString(),
                                         Code = dr["Code"].ToString(),
                                         Name = dr["Name"].ToString(),
                                         Hsn = dr["Hsn"].ToString()

                                     }).ToList();
            }

            return listcwiseProducts;
        }

        public class ClassWiseProductModel
        {
            public string id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Hsn { get; set; }
        }
    }
}
