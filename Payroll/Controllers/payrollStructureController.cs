using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Models;
using Payroll.Repostiory.StructureMaster;
using System.Data;
using UtilityLayer;

namespace Payroll.Controllers
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class payrollStructureController : Controller
    {
        PayStructureEngine objModel = new PayStructureEngine();
        public IStructureLogic objIStructureLogic;

        public ActionResult Dashboard()
        {
            return View(GetPaystructure());
        }
        public ActionResult PartialStructureGrid()
        {
            return PartialView("PartialStructureGrid", GetPaystructure());
        }
        public IEnumerable GetPaystructure()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_PayStructureMasterLists
                    orderby d.StructureID descending
                    select d;
            return q;
        }
        public ActionResult ViewDetails(string ActionType, string StructureID)
        {
            if (ActionType == "VIEW")
            {
                objIStructureLogic = new StructureLogic();
                DataSet ds = objIStructureLogic.GetStructureDetails(StructureID);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    objModel.StructureID = Convert.ToString(dt.Rows[0]["StructureID"]);
                    objModel.StructureName = Convert.ToString(dt.Rows[0]["StructureName"]);
                    objModel.StructureCode = Convert.ToString(dt.Rows[0]["StructureCode"]);
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Payroll.Models.PayStructureEngine.PayHeadsDetails> oview = new List<Payroll.Models.PayStructureEngine.PayHeadsDetails>();
                        oview = APIHelperMethods.ToModelList<Payroll.Models.PayStructureEngine.PayHeadsDetails>(dt);
                        objModel.AllowanceDetails = oview;
                    }
                }

                objModel.StructureHeaderName = "View Pay Structure";
            }

            return View(objModel);
        }
        public ActionResult Index(string ActionType, string StructureID)
        {
            if (ActionType == "ADD")
            {
                objModel.StructureHeaderName = "Add Pay Structure";
                Session["StructureID"] = null;
                Session["StructureDetails"] = null;
                Session["HeadDetails"] = null;
            }
            else if (ActionType == "EDIT")
            {
                Session["StructureID"] = StructureID;
                objIStructureLogic = new StructureLogic();
                DataSet ds = objIStructureLogic.GetStructureDetails(StructureID);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    TempData["StructureDetails"] = dt;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TempData["HeadDetails"] = dt;
                }

                objModel.StructureHeaderName = "Edit Pay Structure";
            }

            return View(objModel);
        }
        public PartialViewResult PayStructure()
        {
            if (TempData["StructureDetails"] != null)
            {
                DataTable dt = (DataTable)TempData["StructureDetails"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    objModel.StructureID = Convert.ToString(dt.Rows[0]["StructureID"]);
                    objModel.StructureName = Convert.ToString(dt.Rows[0]["StructureName"]);
                    objModel.StructureCode = Convert.ToString(dt.Rows[0]["StructureCode"]);
                }
            }

            return PartialView(objModel);
        }
        public PartialViewResult PayHeads()
        {
            objModel.PayHeadTypeList = objModel.PopulatePayHeadType();
            objModel.CalculationTypeList = objModel.PopulateCalculationType();
            objModel.RoundOffTypeList = objModel.PopulateRoundOffType();
            objModel.PayType = "AL";
            objModel.CalculationType = "EO";
            objModel.RoundOffType = "NR";

            if (TempData["HeadDetails"] != null)
            {
                DataTable dt = (DataTable)TempData["HeadDetails"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Payroll.Models.PayStructureEngine.PayHeadsDetails> oview = new List<Payroll.Models.PayStructureEngine.PayHeadsDetails>();
                    oview = APIHelperMethods.ToModelList<Payroll.Models.PayStructureEngine.PayHeadsDetails>(dt);
                    objModel.AllowanceDetails = oview;
                }
            }

            return PartialView(objModel);
        }
        [HttpPost]
        public JsonResult PayStructureSubmit(PayStructureEngine model)
        {
            if (Convert.ToString(model.StructureName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Structure Name is mandatory";
            }
            else if (Convert.ToString(model.StructureCode) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Structure Short Name is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";
                string StructureID = "";

                objIStructureLogic = new StructureLogic();
                objIStructureLogic.StructureModify(model, ref strIsComplete, ref strMessage, ref StructureID);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";

                    Session["StructureID"] = StructureID;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
            }

            return Json(model);
        }
        [HttpPost]
        public JsonResult PayHeadsSubmit(PayStructureEngine model)
        {
            if (Convert.ToString(model.PayHeadName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Pay Head Name is mandatory";
            }
            else if (Convert.ToString(model.PayHeadShortName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Pay Head Short Name is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";
                string PayHeadID = "";
                model.StructureID = Convert.ToString(Session["StructureID"]);
                DataTable dtDetails = new DataTable();

                objIStructureLogic = new StructureLogic();
                objIStructureLogic.PayheadSaveModify(model, ref strIsComplete, ref strMessage, ref PayHeadID, ref dtDetails);
                if (strIsComplete == 1)
                {
                    List<Payroll.Models.PayStructureEngine.PayHeadsDetails> oview = new List<Payroll.Models.PayStructureEngine.PayHeadsDetails>();
                    if (dtDetails != null && dtDetails.Rows.Count > 0)
                    {
                        oview = APIHelperMethods.ToModelList<Payroll.Models.PayStructureEngine.PayHeadsDetails>(dtDetails);
                    }

                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";
                    model.AllowanceDetails = oview;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
            }
            return Json(model);
        }
        [HttpGet]
        public JsonResult PopulatePayHead()
        {
            string StructureID = Convert.ToString(Session["StructureID"]);

            objIStructureLogic = new StructureLogic();
            DataTable dtDetails = objIStructureLogic.PopulatePayHead(StructureID);

            List<Payroll.Models.PayStructureEngine.PayHeadIDList> oview = new List<Payroll.Models.PayStructureEngine.PayHeadIDList>();
            oview = APIHelperMethods.ToModelList<Payroll.Models.PayStructureEngine.PayHeadIDList>(dtDetails);
            objModel.PayHeadList = oview;

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckFormula(string Formula)
        {
            DataTable dtDetails = new DataTable();

            try
            {
                objIStructureLogic = new StructureLogic();
                dtDetails = objIStructureLogic.CheckFormula(Formula);
                objModel.ResponseCode = "Success";
            }
            catch
            {
                objModel.ResponseCode = "Error";
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteStructure(PayStructureEngine model)
        {
            int strIsComplete = 0;
            string strMessage = "";

            objIStructureLogic = new StructureLogic();
            objIStructureLogic.DeleteStructure(model, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                model.ResponseCode = "Success";
                model.ResponseMessage = "Success";
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = strMessage;
            }

            return Json(model);
        }
    }
}