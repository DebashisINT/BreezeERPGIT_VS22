using CRM.Models;
using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CRMMembersController : Controller
    {
        //
        // GET: /CRMMembers/
        public ActionResult doAddMembers(string Module_Name,string Module_Id)
        {
            Session["SelectedValues"] = null;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmMembers objcrmMembers = new crmMembers();
            var queryv_EntityLists = from c in dcon.v_EntityLists
                                     where c.ContactType == "CL"
                                     select c;

            DataTable dtSelected = objcrmMembers.GetEditedData(Module_Name, Module_Id);

            
                        
            objcrmMembers.EntityList = queryv_EntityLists.ToList();

            if (dtSelected != null && dtSelected.Rows.Count > 0)
            {
                List<string> list = dtSelected.AsEnumerable()
                               .Select(r => r.Field<string>("Entity_id"))
                               .ToList();
                objcrmMembers.Selectedvalues = list;
                Session["SelectedValues"] = list;
            }
            else
            {
                objcrmMembers.Selectedvalues = new List<string>();
            }
            return PartialView("~/Views/CRMS/UserControl/PartialMembers.cshtml", objcrmMembers);
        }

        public ActionResult refreshMembersGrid(string EntityType)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmMembers objcrmMembers = new crmMembers();
            var queryv_EntityLists = from c in dcon.v_EntityLists
                                     where c.ContactType == EntityType
                                         select c;
            objcrmMembers.EntityList = queryv_EntityLists.ToList();
            objcrmMembers.Selectedvalues = (List<string>)Session["SelectedValues"];
            return PartialView("~/Views/CRMS/UserControl/PartialMembersGridView.cshtml", objcrmMembers);
        }
        public void AddDataToSelectedList(List<string> lists){

            Session["SelectedValues"] = lists;

        }

        public ActionResult SaveMembers(string Module_Name, string Module_id)
        {
            string output = "";
            crmMembers crmMembersobj = new crmMembers();

            if (Session["SelectedValues"] != null)
            {
                List<string> lst = (List<string>)Session["SelectedValues"];
                if (lst.Count == 0)
                {
                    output = "Please select atleast one entity to proceed.";
                    return Json(output, JsonRequestBehavior.AllowGet);
                }
                string Entity_list = String.Join(",", lst);
                output = crmMembersobj.SaveCRMMember(Module_Name, Module_id, Entity_list);
            }
            else
            {
                output = "Please select atleast one entity to proceed.";
            }
            return Json(output,JsonRequestBehavior.AllowGet);
        } 

	}
}