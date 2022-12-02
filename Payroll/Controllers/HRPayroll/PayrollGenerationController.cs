using BusinessLogicLayer;
using DataAccessLayer;
using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.PayrollGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class PayrollGenerationController : Controller
    {
        // GET: PayrollGeneration
        PGenerationEngine objModel = new PGenerationEngine();
        private IPGeneration _PGeneration;
        CommonBL cSOrder = new CommonBL();

        public ActionResult DashBoard()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            objModel._PClassName = objModel.PopulateClassName();
            objModel.NumberingSchemas = objModel.PopulateNumberingSchema();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ProjectMandatoryInEntry = cSOrder.GetSystemSettingsResult("ProjectMandatoryInEntry");
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.ProjectMandatoryInEntry = ProjectMandatoryInEntry;

            List<HierarchyList> objHierarchy = new List<HierarchyList>();
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                HierarchyList obj = new HierarchyList();
                foreach (DataRow item in hierarchydt.Rows)
                {
                    obj = new HierarchyList();
                    obj.Hierarchy_id = Convert.ToString(item["ID"]);
                    obj.Hierarchy_Name = Convert.ToString(item["H_Name"]);
                    objHierarchy.Add(obj);
                }
            }
            objModel.Hierarchy_List = objHierarchy;

            string HierarchySelectInEntryModule = cSOrder.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ViewBag.Hierarchy = "1";
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ViewBag.Hierarchy = "0";
                }
            }

            return View("~/Views/HRPayroll/PayrollGeneration/DashBoard.cshtml", objModel);

        }
        [HttpGet]
        public JsonResult GetPeriodName(string classId)
        {

            var jsontable = (String)null; ;
            Msg _msg = new Msg();
            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable(@"select proll_PeriodGeneration.Period,proll_PeriodGeneration.YYMM  from proll_PeriodGeneration where     proll_PeriodGeneration.PayrollClassID='" + classId + "' and IsActive=1");
                if (dt != null)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsontable = serializer.Serialize(rows);


                    //ViewData["PeriodFrm"] = dt.Rows[0]["PeriodFrom"].ToString();
                    //ViewData["PeriodTo"] = dt.Rows[0]["PeriodTo"].ToString();
                    //ViewData["Period"] = dt.Rows[0]["Period"].ToString();
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            var result = new { data = jsontable, data2 = _msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PopulateBranchByHierchy()
        {
            List<UnitList> list = new List<UnitList>();

            string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);

            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchHierachy);


            if (branchtable.Rows.Count > 0)
            {
                UnitList obj = new UnitList();
                foreach (DataRow item in branchtable.Rows)
                {
                    obj = new UnitList();
                    obj.ID = Convert.ToString(item["branch_id"]);
                    obj.Name = Convert.ToString(item["branch_description"]);
                    list.Add(obj);
                }
            }

            return Json(list);
        }
        public class UnitList
        {
            public string ID { get; set; }

            public string Name { get; set; }
        }

        public PartialViewResult PartialEmployeeGenerationGrid(string ClassCode, string yymm)
        {
            return PartialView("~/Views/HRPayroll/PayrollGeneration/PartialEmployeeGenerationGrid.cshtml", GetEmployeeList(ClassCode, yymm));
        }

        public PartialViewResult PartialSelectedEmployeeGenerationGrid(string ClassCode, string yymm)
        {
            return PartialView("~/Views/HRPayroll/PayrollGeneration/_PartialSelectedGeneration.cshtml", GetEmployeeList(ClassCode, yymm));
        }
        public IEnumerable GetEmployeeList(string ClassCode, string yymm)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_Dflt_Salaries
                    where d.ClassCode == ClassCode && Convert.ToInt32(d.FrmDt) <= Convert.ToInt32(yymm) && Convert.ToInt32(d.ToDt) >= Convert.ToInt32(yymm)
                    orderby d.EmployeeID descending
                    select d;
            return q;
        }




        public PartialViewResult PartialAllowanceGenerationGrid(string ClassCode, string EmployeeCode, string yymm)
        {
            return PartialView("~/Views/HRPayroll/PayrollGeneration/PartialAllowanceGenerationGrid.cshtml", GetAllowanceList(ClassCode, EmployeeCode, yymm));
        }
        public IEnumerable GetAllowanceList(string ClassCode, string EmployeeCode, string yymm)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_SalaryImage_AllowanceDeductions
                    where (d.EmployeeCode == EmployeeCode) && (d.ClassID == ClassCode) && (d.YYMM == yymm)
                    orderby d.serial descending
                    select d;
            return q;
        }

        [HttpPost]
        public JsonResult PGeneration(string ClassCode)
        {
            string output_msg = string.Empty;
            int ReturnCode = 0;
            _PGeneration = new PGeneration();
            Msg _msg = new Msg();
            try
            {
                output_msg = _PGeneration.PGenerate(ClassCode, ref ReturnCode);
                if (output_msg == "Payroll Generate successfully" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PGenerationEmployeeWise(string ClassCode,string emp)
        {
            string output_msg = string.Empty;
            int ReturnCode = 0;
            _PGeneration = new PGeneration();
            Msg _msg = new Msg();
            try
            {
                output_msg = _PGeneration.PGenerateEmployeeWise(ClassCode,emp, ref ReturnCode);
                if (output_msg == "Payroll Generate successfully" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UndoSalaryGeneration(String EmployeeCode, String ClassCode, String yymm)
        {
            string output_msg = string.Empty;
            Msg _msg = new Msg();
            int ReturnCode = 0;
            string ReturnMessage="";
            _PGeneration = new PGeneration();
            DataTable tableEMPCode = new DataTable();
            tableEMPCode.Columns.Add("EmployeeCode", typeof(string));
            if (EmployeeCode != "")
            {
                string[] EmployeeList = EmployeeCode.Split(',');
                for (int i = 0; i < EmployeeList.Length; i++)
                {
                    tableEMPCode.Rows.Add(new object[] { EmployeeList[i] });
                }
            }
            try
            {
               // output_msg = _PGeneration.UndoSalaryGeneration(EmployeeCode,ClassCode,yymm, ref ReturnCode);
                _PGeneration.UndoSalaryGeneration(tableEMPCode, ClassCode, yymm, ref ReturnCode, ref ReturnMessage);
                _msg.response_msg = ReturnMessage;

            }

            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            var result = new { data = _msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");

                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }


            strschemavalue = strschemavalue + "~" + "" + "~" + Valid_From + "~" + Valid_Upto;
            return Json(strschemavalue);
        }


        public JsonResult Validate(string employeeId)
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "ValidateEmployee");
            proc.AddVarcharPara("@employeeId", -1, employeeId);
            DT = proc.GetTable();
            return Json("");
        }


        public JsonResult PostToAccount(string ClassCode, string EmployeeCode, string yymm, string Numbering, string Doc_no, DateTime Date, string BranchId, string projId)
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLPOSTING");
            proc.AddVarcharPara("@NUMBERING", 100, Numbering);
            proc.AddVarcharPara("@DOC_NO", 100, Doc_no);
            proc.AddDateTimePara("@POSTING_DATE", Date);
            proc.AddVarcharPara("@CLASSID", 100, ClassCode);
            proc.AddVarcharPara("@YYMM", 100, yymm);
            proc.AddVarcharPara("@Branch", 100, BranchId);
            proc.AddVarcharPara("@userid", 100, Convert.ToString(Session["userid"]));
            proc.AddVarcharPara("@EMPLOYEES", -1, EmployeeCode);
            proc.AddVarcharPara("@Project_Id", 100, projId);

            DT = proc.GetTable();
            return Json("Jornal Generated Successfully.");
        }

        public JsonResult PostToAccountCheck(string ClassCode, string EmployeeCode, string yymm)
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "PostToJournalCheck");
            proc.AddVarcharPara("@CLASSID", 100, ClassCode);
            proc.AddVarcharPara("@YYMM", 100, yymm);           
           
            proc.AddVarcharPara("@EMPLOYEES", -1, EmployeeCode);
            DT = proc.GetTable();
            int count = 0;
            if (DT.Rows.Count > 0 && DT!=null)
            {
                 count = Convert.ToInt32(DT.Rows[0]["exist"]);
            }
            return Json(count);
        }

        public ActionResult GetProjectCode( string Project_ID, String Branchs, String Hierarchy)
        {
            try
            {
                String Branch = "";
                //if (model.Unit != null)
                //{
                //    Branch = model.Unit;
                //}
                //else
                //{
                //    Branch = Branchs;
                //}
                Branch = Branchs;
                DataTable dtProj = new DataTable();
                dtProj = DTGetProjectCode(Branch);
                
                List<ProjectList> modelProj = new List<ProjectList>();
                modelProj = APIHelperMethods.ToModelList<ProjectList>(dtProj);
                ViewBag.ProjectID = Project_ID;
                ViewBag.Hierarchy = Hierarchy;

                return PartialView("~/Views/HRPayroll/PayrollGeneration/_PartialProjectCode.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public class ProjectList
        {
            public long Proj_Id { get; set; }
            public String ProjectCode { get; set; }
            public String ProjectName { get; set; }
            public String CostomerName { get; set; }
            public String Hierarchy_ID { get; set; }
            public String Hierarchy_Name { get; set; }
        }
        public DataTable DTGetProjectCode(String BRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECTALL");
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            ds = proc.GetTable();
            return ds;
        }
        [WebMethod]
        public JsonResult getHierarchyID(string ProjID)
        {
            ReturnData obj = new ReturnData();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                obj.Success = true;
                obj.Message = Hierarchy_ID;
                // return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                //return Hierarchy_ID;
                obj.Success = true;
                obj.Message = Hierarchy_ID;
            }
            return Json(obj);
        }

    }
}