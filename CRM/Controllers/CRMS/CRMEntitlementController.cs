using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMEntitlementController : Controller
    {
        //
        // GET: /CRMEntitlement/
        public ActionResult Index()
        {
            return View(@"~/Views/CRMS/Entitlement/Entitlement.cshtml");
        }
	}
}