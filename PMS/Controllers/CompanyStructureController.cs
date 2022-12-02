using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using System.IO;
using System.Web.Routing;
using DevExpress.XtraPrinting;
using PMS.Models;
using PMS.Models.DataContext;
using BusinessLogicLayer;
using System.Data;
using DataAccessLayer;
using EntityLayer.CommonELS;


namespace PMS.Controllers
{
    public class CompanyStructureController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        OrgStructureModel OSM = null;
        public CompanyStructureController()
        {
            OSM = new OrgStructureModel();
        }
        //
        // GET: /CompanyStructure/
        public ActionResult Index()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CompanyStructure");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;

            return View("~/Views/CompanyStructure/HierarchyList.cshtml");
        }

        public ActionResult HierarchyGrid()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CompanyStructure");
            ViewBag.CanEdit = rights.CanEdit;
 


            PMS.Models.DataContext.PMSDataClassesDataContext dc = new PMS.Models.DataContext.PMSDataClassesDataContext(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            List<Master_Hierarchy> lst = (from d in dc.Master_Hierarchies
                                          select d).ToList();
            return PartialView("~/Views/CompanyStructure/_partialHierarchyList.cshtml", lst);
        }

        public ActionResult DataViewPertial()
        {
            List<OrgStructureModel> Lst = new List<OrgStructureModel>();



            //Lst = OSM.GetList();
            return PartialView("~/Views/CompanyStructure/_DataViewPertial.cshtml", Lst);
        }

        public ActionResult CompanyStructure(string h_id)
        {
            Session["h_id"] = h_id;
            return View("CompanyStruc");
        }

        public ActionResult CompStrucTreeListPartial()
        {
            //List<PMS.Models.OrgStructureModel.UnitDescription> Lst = new List<PMS.Models.OrgStructureModel.UnitDescription>();
            //Lst = OSM.GetUnitList();



            return PartialView("~/Views/CompanyStructure/CompStrucTreeListPartial.cshtml", GetOpHierarchy());
        }

        [HttpPost]
        public ActionResult CompStrucTreeListPartialAddNew(PMS_OPRHIERARCHY item)
        {

            if (Validation(item))
            {
                try
                {
                    string desc = "";
                    if (!string.IsNullOrEmpty(item.DESCRIPTION))
                    {
                        desc = Convert.ToString(item.DESCRIPTION).Replace('"', ' ').Trim();
                    }

                    ProcedureExecute proc = new ProcedureExecute("prc_Hierarchy_Addedit");
                    proc.AddVarcharPara("@Action", 500, "SaveHierarchyDetails");
                    proc.AddVarcharPara("@OID", 500, item.OID);
                    proc.AddPara("@PARENT_ID", item.PARENT_ID);
                    proc.AddVarcharPara("@NAME", 500, item.NAME.Replace('"', ' ').Trim());
                    proc.AddPara("@Hierarchy_ID", Convert.ToString(Session["h_id"]));
                    proc.AddPara("@OPERHEAD", item.OPERHEAD);
                    proc.AddVarcharPara("@DESCRIPTION", 500, desc);
                    proc.AddPara("@user_id", Convert.ToInt32(Session["userid"]));
                    proc.GetScalar();
                }
                catch
                {

                }
            }
            return PartialView("~/Views/CompanyStructure/CompStrucTreeListPartial.cshtml", GetOpHierarchy());


        }

        private bool Validation(PMS_OPRHIERARCHY item)
        {
            if (string.IsNullOrEmpty(Convert.ToString(item.ID)))
            {
                ViewBag.msg = "Please enter a ID to proceed.";
                return false;
            }
            if (string.IsNullOrEmpty(Convert.ToString(item.NAME)))
            {
                ViewBag.msg = "Please enter a name to proceed.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public JsonResult CheckUniqueID(string txtOID, string mode)
        {

            int i = 1;

            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_Hierarchy_Addedit");
                proc.AddVarcharPara("@Action", 500, "CheckUniqueID");
                proc.AddVarcharPara("@OID", 500, txtOID);
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    i = 0;
                }
                else
                {
                    i = 1;
                }

            }
            catch
            {
                i = 0;

            }

            return Json(i, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        public ActionResult CompStrucTreeListPartialUpdate(PMS_OPRHIERARCHY item)
        {

            try
            {
                string desc = "";
                if (!string.IsNullOrEmpty(item.DESCRIPTION))
                {
                    desc = Convert.ToString(item.DESCRIPTION).Replace('"', ' ').Trim();
                }
                ProcedureExecute proc = new ProcedureExecute("prc_Hierarchy_Addedit");
                proc.AddVarcharPara("@Action", 500, "UpdateHierarchyDetails");
                proc.AddPara("@ID", item.ID);
                proc.AddVarcharPara("@OID", 500, item.OID);
                proc.AddPara("@PARENT_ID", item.PARENT_ID);
                proc.AddVarcharPara("@NAME", 500, item.NAME.Replace('"', ' ').Trim());
                proc.AddPara("@Hierarchy_ID", Convert.ToString(Session["h_id"]));
                proc.AddPara("@OPERHEAD", item.OPERHEAD);
                proc.AddVarcharPara("@DESCRIPTION", 500, desc);
                proc.AddPara("@user_id", Convert.ToInt32(Session["userid"]));
                proc.GetScalar();
            }
            catch
            {

            }



            return PartialView("~/Views/CompanyStructure/CompStrucTreeListPartial.cshtml", GetOpHierarchy());
        }


        [HttpPost]
        public ActionResult CompStrucTreeListPartialDelete(PMS_OPRHIERARCHY item)
        {
            string output;
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_Hierarchy_Addedit");
                proc.AddVarcharPara("@Action", 500, "DeleteHierarchyDetails");
                proc.AddPara("@ID", item.ID);
                dt = proc.GetTable();
                output = Convert.ToString(dt.Rows[0][0]);
            }
            catch
            {
                output = "error";
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public Heirarchy_details GetOpHierarchy()
        {
            Heirarchy_details det = new Heirarchy_details();
            PMS.Models.DataContext.PMSDataClassesDataContext dc = new PMS.Models.DataContext.PMSDataClassesDataContext(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            List<PMS_OPRHIERARCHY> lst = (from d in dc.PMS_OPRHIERARCHies
                                          where Convert.ToString(d.Hierarchy_ID) == Convert.ToString(Session["h_id"])
                                          select d).ToList();



            List<v_employee_detail> lsts = (from d in dc.v_employee_details
                                            select d).ToList();

            det.listHierarchy = lst;
            det.employee_List = lsts;
            return det;
        }

        public ActionResult SetConpanyStructureSessionId(string Id)
        {

            Session["_StructureId"] = Id;
            //  var data = Id;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRecord(string Name, string Desc)
        {
            string output = "";
            bool Valid = true;


            if (string.IsNullOrEmpty(Name))
            {
                output = "Name can not be blank";
                Valid = false;
            }



            if (Valid)
            {
                try
                {
                    output = "Data Saved";

                    ProcedureExecute proc = new ProcedureExecute("prc_Hierarchy_Addedit");
                    proc.AddVarcharPara("@Action", 500, "SaveHierarchy");
                    proc.AddVarcharPara("@h_name", 500, Name);
                    proc.AddVarcharPara("@h_Description", 500, Desc);
                    proc.AddPara("@user_id", Convert.ToInt32(Session["userid"]));
                    proc.GetScalar();
                }
                catch
                {
                    output = "Please try again later";
                }
            }

            return Json(output, JsonRequestBehavior.AllowGet);


        }



    }
    public class Heirarchy_details
    {
        public List<PMS_OPRHIERARCHY> listHierarchy { get; set; }
        public List<v_employee_detail> employee_List { get; set; }

    }
}


