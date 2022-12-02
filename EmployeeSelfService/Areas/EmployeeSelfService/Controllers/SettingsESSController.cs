using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class SettingsESSController : Controller
    {
        //
        // GET: /EmployeeSelfService/SettingsESS/

        public ActionResult SettingsESS()
        {
            return View();
        }

    }
}
