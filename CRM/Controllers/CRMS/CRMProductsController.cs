using CRM.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMProductsController : Controller
    {
        //
        // GET: /CRMProducts/
        public ActionResult AddProducts()
        {
            return PartialView("~/Views/CRMS/UserControl/PartialCRMProducts.cshtml");
        }

        public ActionResult SaveCRMProductDetails(string list, string Module_Name, string Module_id)
        {
            string output = "";
            crmProducts pro = new crmProducts();
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<crmProd> finalResult = jsSerializer.Deserialize<List<crmProd>>(list);
            DataTable dt_activityproducts = new DataTable();
            dt_activityproducts.Columns.Add("Id", typeof(string));
            dt_activityproducts.Columns.Add("ProdId", typeof(Int32));
            dt_activityproducts.Columns.Add("Act_Prod_Qty", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Rate", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Remarks", typeof(String));
            dt_activityproducts.Columns.Add("Act_Prod_Frequency", typeof(String));
            dt_activityproducts.Columns.Add("Act_Prod_Amount", typeof(Decimal));
            foreach (var item in finalResult)
            {
                dt_activityproducts.Rows.Add(item.guid, item.ProductId, item.Quantity, item.Rate, item.Remarks, item.Frequency, item.Amount);
            }

            string Output = pro.SaveCRMProducts(dt_activityproducts, Module_Name, Module_id);

            return Json(Output, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCRMProductsDetails(string Module_Name, string Module_id)
        {
            crmProducts pro = new crmProducts();

            DataTable dtProds = pro.GetCRMProductsDetails(Module_Name, Module_id);
            List<crmProd> finalResult = new List<crmProd>();
            if (dtProds != null)
            {
                finalResult = DbHelpers.ToModelList<crmProd>(dtProds);
            }

            return Json(finalResult);
        }

	}
}