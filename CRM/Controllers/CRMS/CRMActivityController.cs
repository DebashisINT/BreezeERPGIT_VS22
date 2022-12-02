using CRM.Models;
using CRM.Models.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace CRM.Controllers.CRMS
{
    public class CRMActivityController : Controller
    {
        //
        // GET: /CRMActivity/
        public ActionResult DoActivity(String Module_Name, String Module_id)
        {
            crmActivity crmAct = new crmActivity();
            DataSet ddVal = crmAct.GetAllDropdown();

            List<Contact_Type> Contact_Type = APIHelperMethods.ToModelList<Contact_Type>(ddVal.Tables[5]);
            List<Activity> Activity = APIHelperMethods.ToModelList<Activity>(ddVal.Tables[0]);

            Lead_ActivityType act = new Lead_ActivityType();
            act.Lead_ActivityTypeName = "Select";
            act.Id = 0;
            List<Lead_ActivityType> ActivityType = new List<Lead_ActivityType>();
            ActivityType.Add(act);
            List<AssignTo> AssignTo = APIHelperMethods.ToModelList<AssignTo>(ddVal.Tables[4]);
            List<Duration> Duration = APIHelperMethods.ToModelList<Duration>(ddVal.Tables[3]);
            List<Prioritys> Priority = APIHelperMethods.ToModelList<Prioritys>(ddVal.Tables[2]);
            // Mantis Issue 24175
            List<Lead_ActivityTypeNew> ActivityTypeNew = APIHelperMethods.ToModelList<Lead_ActivityTypeNew>(ddVal.Tables[6]);
            // End of Mantis Issue 24175




            crmAct.Contact_Type = Contact_Type;
            crmAct.Activity = Activity;
            crmAct.ActivityType = ActivityType;
            crmAct.AssignTo = AssignTo;
            crmAct.Duration = Duration;
            crmAct.Priority = Priority;
            // Mantis Issue 24175
            crmAct.ActivityTypeNew = ActivityTypeNew;
            // End of Mantis Issue 24175




            ViewBag.cActivity_Date = null;
            ViewBag.ddlContactType = "";
            ViewBag.cbtnEntity = "";
            ViewBag.cddlActivity = "";
            ViewBag.cddlActivityType = "";
            ViewBag.ctxt_Subject = "";
            ViewBag.cmemo_Details = "";
            ViewBag.AssignTo = Convert.ToString(Session["userid"]);
            ViewBag.Duration = "";
            ViewBag.ddlPriority = "";
            ViewBag.cDue_dt = null;
            // Rev Mantis Issue 22801
            ViewBag.timepicker1 = "00:00:00";
            // End of Rev Mantis Issue 22801
            // Mantis Issue 24175
            ViewBag.cddlActivityTypeNew = "";
            // End of Mantis Issue 24175

            DataSet output = crmAct.EditCRMActivity(Module_Name, Module_id);

            if (output != null && output.Tables[0] != null && output.Tables[0].Rows.Count > 0)
            {
                ViewBag.cActivity_Date = Convert.ToDateTime(output.Tables[0].Rows[0]["ActivityDate"]);
                ViewBag.ddlContactType = Convert.ToString(output.Tables[0].Rows[0]["ContactType"]);
                ViewBag.cbtnEntity = Convert.ToString(output.Tables[0].Rows[0]["ContactType"]);
                ViewBag.cddlActivity = Convert.ToString(output.Tables[0].Rows[0]["Lead_activityid"]);
                ViewBag.cddlActivityType = Convert.ToString(output.Tables[0].Rows[0]["Typeid"]);
                ViewBag.ctxt_Subject = Convert.ToString(output.Tables[0].Rows[0]["Leadsubject"]);
                ViewBag.cmemo_Details = Convert.ToString(output.Tables[0].Rows[0]["Leaddetails"]);
                ViewBag.AssignTo = Convert.ToString(output.Tables[0].Rows[0]["Assignto"]);
                ViewBag.Duration = Convert.ToString(output.Tables[0].Rows[0]["Durationid"]);
                ViewBag.ddlPriority = Convert.ToString(output.Tables[0].Rows[0]["Priorityid"]);
                // Rev Mantis Issue 22801
                ViewBag.timepicker1 = Convert.ToString(output.Tables[0].Rows[0]["Duration_New"]);
                // End of Rev Mantis Issue 22801
                // Mantis Issue 24175
                ViewBag.cddlActivityTypeNew = Convert.ToString(output.Tables[0].Rows[0]["TypeNewid"]);
                // End of Mantis Issue 24175
                if (!string.IsNullOrEmpty(Convert.ToString(output.Tables[0].Rows[0]["Duedate"])))
                {
                    ViewBag.cDue_dt = Convert.ToDateTime(output.Tables[0].Rows[0]["Duedate"]);
                }


                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

                var Lead_ActivityTypes = from c in dcon.Lead_ActivityTypes
                                         where c.LeadActivityId == Convert.ToInt32(output.Tables[0].Rows[0]["Lead_activityid"])
                                         select c;
                crmAct.ActivityType = Lead_ActivityTypes.ToList(); 


            }

            return PartialView(@"~/Views/CRMS/UserControl/PartialActivity.cshtml", crmAct);
        }

        public ActionResult ActivityChange(int ActivityId )
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            var Lead_ActivityTypes = from c in dcon.Lead_ActivityTypes
                                     where c.LeadActivityId == ActivityId
                                     select c;

            return Json(Lead_ActivityTypes);
        }

        public string SaveActivityProductDetails(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CRMActivityProductDetails> finalResult = jsSerializer.Deserialize<List<CRMActivityProductDetails>>(list);
            Session["SessionActivityProduct"] = finalResult;

            return null;
        }

        // Rev Mantis Issue 22801 [control Duration_New added in parameter]
        // Mantis Issue 24175 (parameter ActivityTypeNew  added)
        public ActionResult SaveActivity(string activity, string activity_type, string subject, string details, string assignto, string duration, string priority, DateTime? DtxtDue, DateTime? dtActivityDate, string CRMactivityid, string contacttype, string Module_Name, TimeSpan Duration_New, string ActivityTypeNew)
        {
            crmActivity crmAct = new crmActivity();
            DataTable dt_activityproducts = new DataTable();
            dt_activityproducts.Columns.Add("Id", typeof(string));
            dt_activityproducts.Columns.Add("ProdId", typeof(Int32));
            dt_activityproducts.Columns.Add("Act_Prod_Qty", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Rate", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Remarks", typeof(String));
            if (Session["SessionActivityProduct"] != null)
            {

                List<CRMActivityProductDetails> obj = new List<CRMActivityProductDetails>();
                obj = (List<CRMActivityProductDetails>)Session["SessionActivityProduct"];
                foreach (var item in obj)
                {
                    dt_activityproducts.Rows.Add(item.guid, item.ProductId, item.Quantity, item.Rate, item.Remarks);
                }
            }

            // Rev Mantis Issue 22801 [Duration_New control passed in parameter]
            // Mantis Issue 24175 [parameter ActivityTypeNew added ]
            string output = crmAct.SaveActivity(activity, activity_type, subject, details, assignto, duration, priority, DtxtDue, dtActivityDate, CRMactivityid, contacttype, dt_activityproducts, Module_Name, Duration_New,ActivityTypeNew);

            return Json(output,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetcrmProducts(string SearchKey)
        {
            List<crmPosProductModel> listCust = new List<crmPosProductModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = oDBEngine.GetDataTable("select top 10  sProductsID as Products_ID,Products_Name ,Products_Description ,HSNSAC  from v_Product_MargeDetailsPOS where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");


                listCust = (from DataRow dr in cust.Rows
                            select new crmPosProductModel()
                            {
                                id = dr["Products_ID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"]),
                                HSN = Convert.ToString(dr["HSNSAC"])
                            }).ToList();
            }

           return Json(listCust);
        }

        public ActionResult GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                // Rev 0019246 Subhra 26-12-2018 
                //DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_pos_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                DataTable cust = oDBEngine.GetDataTable("select top 10 cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing   from v_group_customerDetails  where  Type='Customer' and  uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");
                //End of Rev

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return Json(listCust);
        }


      


        public ActionResult CRMShowHistory()
        {
            return PartialView(@"~/Views/CRMS/UserControl/PartialActivityShowHistory.cshtml");
        }

        public ActionResult PopulateActivityShowHistoryGrid()
        {
            return PartialView(@"~/Views/CRMS/UserControl/PartialActivityShowHistoryGridview.cshtml", GetShowHistoryData("1"));
        }
        public ActionResult PopulateActivityAllShowHistoryGrid(string frmdate, string action_type, string entity_id, string Module)
        {
            TempData["IsShowHistorybttnClicked"]="Yes";
            crmActivity crmAct = new crmActivity();
            crmAct.GetShowHistoryData(action_type, entity_id, Module);
            return PartialView(@"~/Views/CRMS/UserControl/PartialActivityShowHistory.cshtml", GetShowHistoryData(frmdate));
        }
        public ActionResult PopulateActivityToptenShowHistoryGrid(string frmdate, string action_type, string entity_id, string Module)
        {
            TempData["IsShowHistorybttnClicked"] = "Yes";
            crmActivity crmAct = new crmActivity();
            action_type = "ActivityShowTop10";
            crmAct.GetShowHistoryData(action_type, entity_id, Module);
            return PartialView(@"~/Views/CRMS/UserControl/PartialActivityShowHistory.cshtml", GetShowHistoryData(frmdate));
        }
        public IEnumerable GetShowHistoryData(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);
            if (TempData["IsShowHistorybttnClicked"]!=null && TempData["IsShowHistorybttnClicked"].ToString() == "Yes")
            {
                if (frmdate != "Ispageload")
                {
                    CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                    var q = from d in dc.SHOWACTIVITYHISTORies
                            orderby d.SHOWHISTORY_SALESACTIVITYID
                            select d;
                    return q;
                }
                else
                {
                    CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                    var q = from d in dc.SHOWACTIVITYHISTORies
                            where d.SHOWHISTORY_SALESACTIVITYID == 99999999999
                            orderby d.SHOWHISTORY_SALESACTIVITYID
                            select d;
                    return q;
                }
                
               

            }
            else {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.SHOWACTIVITYHISTORies
                        where d.SHOWHISTORY_SALESACTIVITYID == 99999999999
                        orderby d.SHOWHISTORY_SALESACTIVITYID
                        select d;
                return q;
            }


          

        }
    }
}