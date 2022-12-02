using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMCompetitorController : Controller
    {
        //
        // GET: /CRMCompetitor/
        public ActionResult Index()
        {
            return View(@"~/Views/CRMS/Competitor/Competitors.cshtml");
        }
	}
}