using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using ERP.Models;
using DevExpress.Web;
using System.Web.Script.Services;
namespace ERP.OMS.Management.Activities
{
    public partial class CustSaleRateLock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fromdt.Date = DateTime.Now;
                ToDate.Date = DateTime.Now;

                Hiddenvalidfrom.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Hiddenvalidupto.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                
                
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_SaleRateLock_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


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
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object GetProduct(string SearchKey)
        {
            List<PosProductModel> listCust = new List<PosProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                DataTable cust = oDBEngine.GetDataTable("select top 10  Products_ID,Products_Name ,Products_Description ,sProduct_MinSalePrice  from v_Product_SaleRateLock where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");


                listCust = (from DataRow dr in cust.Rows
                            select new PosProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                MinSalePrice = Convert.ToString(dr["sProduct_MinSalePrice"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string addSaleRateLock(string SaleRateLockID, string CustID, string ProductID, string DiscSalesPrice, string MinSalePrice, string discount, string fromdt, string todate, string action)
        {
            try
            {
                DateTime fdt = Convert.ToDateTime(fromdt);
                DateTime tdt = Convert.ToDateTime(todate);
                if (fdt > tdt)
                {
                    return "-12";
                }
                else if (fdt == tdt)
                {
                    return "-13";
                }
                else
                {
                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                    ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateLock");
                    proc.AddVarcharPara("@SaleRateLockID", 50, SaleRateLockID);
                    proc.AddVarcharPara("@CustomerID", 50, CustID);
                    proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductID));
                    proc.AddDecimalPara("@DiscSalesPrice", 2, 18, Convert.ToDecimal(DiscSalesPrice));
                    proc.AddIntegerPara("@ApprovedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@ValidFrom", 50, fdt.ToString("yyyy-MM-dd HH:mm:ss"));
                    proc.AddVarcharPara("@ValidUpto", 50, tdt.ToString("yyyy-MM-dd HH:mm:ss"));
                    proc.AddDecimalPara("@MinSalePrice", 2, 18, Convert.ToDecimal(MinSalePrice));
                    proc.AddDecimalPara("@Disc", 2, 18, Convert.ToDecimal(discount));
                    proc.AddVarcharPara("@Action", 4000, action);
                    DataTable dtSaleRateLock = proc.GetTable();
                    if (dtSaleRateLock.Rows.Count > 0)
                    {
                        if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-11")
                        {
                            return "-11";
                        }
                        else
                        {
                            return "1";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }



            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string DeleteSaleRateLock(string SaleRateLockID )
        {
            try
            {
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateLock");
                proc.AddVarcharPara("@SaleRateLockID", 50, SaleRateLockID);
                proc.AddVarcharPara("@Action", 4000, "delete");
                DataTable dtSaleRateLock = proc.GetTable();
                if (dtSaleRateLock.Rows.Count > 0)
                {
                    if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-999")
                    {
                        return "-999";
                    }
                    else if (dtSaleRateLock.Rows[0]["Insertmsg"].ToString() == "-998")
                    {
                        return "-998";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }



            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SaleRateLockID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

             var q = from d in dc.v_SaleRateLockLists
                     orderby d.SaleRateLockID descending
                     select d;
             e.QueryableSource = q;
        }
        protected void GridSaleRate_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }
       

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static List<SaleRateLock> GetSaleRateLock(string SaleRateLockID)
        {

            List<SaleRateLock> listSaleRateLock = new List<SaleRateLock>();
            if (HttpContext.Current.Session["userid"] != null)
            {
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                 ProcedureExecute proc = new ProcedureExecute("PRC_SaleRateLock");
                proc.AddVarcharPara("@Action", 50, "GetSaleRateLockDetails");
                proc.AddVarcharPara("@SaleRateLockID", 10, SaleRateLockID);
                DataTable dtSaleRateLock = proc.GetTable();


                listSaleRateLock = (from DataRow dr in dtSaleRateLock.Rows
                                   select new SaleRateLock()
                                   {
                                       CustomerID = dr["CustomerID"].ToString(),
                                       CustomerName = dr["CustName"].ToString(),
                                       ProductID = Convert.ToString(dr["ProductID"]),
                                       Products_Name = Convert.ToString(dr["Products_Name"]),
                                       DiscSalesPrice = dr["DiscSalesPrice"].ToString(),
                                       MinSalePrice = dr["MinSalePrice"].ToString(),
                                       Disc = Convert.ToString(dr["Disc"]),
                                       ValidFrom = Convert.ToString(dr["ValidFrom"]),
                                       ValidUpto = Convert.ToString(dr["ValidUpto"]),
                                       IsInUse = Convert.ToString(dr["IsInUse"])
                                   }).ToList();
            }

            return listSaleRateLock;
        
        }
        public class SaleRateLock
        {
            public int SaleRateLockID { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string ProductID { get; set; }
            public string Products_Name { get; set; }
            public string DiscSalesPrice { get; set; }
            public string MinSalePrice { get; set; }
            public string Disc { get; set; }
            public string ValidFrom { get; set; }
            public string ValidUpto { get; set; }
            public string IsInUse { get; set; }

        }
        public class PosProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            public string MinSalePrice { get; set; }
        }
        public class CustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string add { get; set; }
        }
    }
}