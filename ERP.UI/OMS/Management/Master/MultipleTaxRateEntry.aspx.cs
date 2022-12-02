using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class MultipleTaxRateEntry : System.Web.UI.Page
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetHsnSac(string SearchKey)
        {
            List<HsnSac> listHsn = new List<HsnSac>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DataTable cust = oDBEngine.GetDataTable("select * from v_HsnSac where Code Like '%" + SearchKey + "%' or [Description] like '%" + SearchKey + "%'");
                listHsn = (from DataRow dr in cust.Rows
                            select new HsnSac()
                            {
                               Sl=Convert.ToInt32(dr["Id"]),
                               HsnSacCode=Convert.ToString(dr["Code"]),
                               Description = Convert.ToString(dr["Description"]),
                               Type = Convert.ToString(dr["Type"])



                            }).ToList();
            }

            return listHsn;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }

    public class HsnSac
    {
        public int Sl { get; set; }
        public string HsnSacCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}