using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class PriceListController : Controller
    {
        //
        // GET: /PriceList/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PriceListView()
        {
            return View();
        }
	}
}