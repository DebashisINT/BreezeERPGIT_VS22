using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CutOff.Controllers
{
    public class DummyController : Controller
    {
        public ActionResult PartialRender()
        {
            return PartialView();
        }
    }
}