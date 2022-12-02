using BreezeERPAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Mvc;

namespace BreezeERPAPI.Controllers
{
    public class TopsalesMonthBranchController : Controller
    {

        public JsonResult Topsales()
        {
            List<Topsalesmonthbranch> lomodel = new List<Topsalesmonthbranch>();
            topbranchbonthsalesoutput omodel = new topbranchbonthsalesoutput();

            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "April",
                amount = 1642.85
            });

            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "May",
                amount = 1132.49
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "June",
                amount = 974.55
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "July",
                amount = 948.28
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "August",
                amount = 854.28
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "September",
                amount = 819.08
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "October",
                amount = 760.65
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "November",
                amount = 751.32
            });

            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "December",
                amount = 691.63
            });

            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "January",
                amount = 670.27
            });

            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "February",
                amount = 649.21
            });


            lomodel.Add(new Topsalesmonthbranch()
            {
                month = "March",
                amount = 607.13
            });


            omodel.unit = "Rs.(In Lakh)";
            omodel.branch = "Dalhousie";
            omodel.year = "2017";
            omodel.data = lomodel;


            //return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);

            return Json(omodel, JsonRequestBehavior.AllowGet);

        }

    }
}
