using Payroll.Models;
using Payroll.Repostiory.StructureMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Payroll.Controllers
{
    public class StructureController : Controller
    {
        PayStructureEngine objModel = new PayStructureEngine();
        public IStructureLogic objIStructureLogic;

        public ActionResult Index()
        {
            if (Session["StructureID"] != null)
            {
                string StructureID = Convert.ToString(Session["StructureID"]);

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
            else
            {
                objModel.StructureHeaderName = "Add Pay Structure";
            }          

            return View(objModel);
        }
        public PartialViewResult PayStructure()
        {
            if (TempData["StructureDetails"]!=null)
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

            if (Session["StructureID"]!=null)
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
                int strIsComplete=0;
                string strMessage="";
                string StructureID="";

                objIStructureLogic = new StructureLogic();
                objIStructureLogic.StructureModify(model, ref strIsComplete, ref strMessage, ref StructureID);
                if(strIsComplete==1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";

                    Session["StructureID"] = StructureID;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = "Please try again later";
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
                model.StructureID=Convert.ToString(Session["StructureID"]);
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
    }
}