using DataAccessLayer;
using ERP.OMS.Management.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Payroll.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Payroll_Master : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayStructureList(string SearchKey)
        {
            List<StructureList> list = new List<StructureList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" SELECT StructureID,StructureName From proll_PayStructureMaster Where IsDeleted<>'Y' AND StructureName like '%" + SearchKey + "%' or StructureCode like '%" + SearchKey + "%'   ORDER BY StructureName");


                list = (from DataRow dr in dt.Rows
                        select new StructureList()
                        {
                            StructureID = Convert.ToString(dr["StructureID"]),
                            StructureName = Convert.ToString(dr["StructureName"]),
                        }).ToList();
            }

            return list;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayrollEmployeeList(string SearchKey, string PayStructureID)
        {
            List<EmployeeList> list = new List<EmployeeList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(@"Select Employee_Code,EmployeeUniqueCode,Employee_Name From proll_EmployeeAttactchment
                                                        inner Join v_proll_EmployeeList On proll_EmployeeAttactchment.EmployeeCode=v_proll_EmployeeList.Employee_Code
                                                        Where (PayStructureCode='" + PayStructureID + "' AND EmployeeUniqueCode like '%" + SearchKey + "%') OR (PayStructureCode='" + PayStructureID + "' AND Employee_Name like '%" + SearchKey + "%')");


                list = (from DataRow dr in dt.Rows
                        select new EmployeeList()
                        {
                            EmployeeCode = Convert.ToString(dr["Employee_Code"]),
                            EmployeeUniqueCode = Convert.ToString(dr["EmployeeUniqueCode"]),
                            EmployeeName = Convert.ToString(dr["Employee_Name"])
                        }).ToList();
            }

            return list;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayrollSalaryPeriodList(string SearchKey, string PayStructureID)
        {
            List<PeriodList> list = new List<PeriodList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

                string ActiveYYMM = oDBEngine.ExeSclar("SELECT YYMM From proll_PayStructureMaster Inner Join proll_PeriodGeneration On PayrollClassID=ClassId Where IsActive=1 AND StructureID='" + PayStructureID + "'");
                DataTable dt = new DataTable();
                if (PayStructureID != "")
                    dt = oDBEngine.GetDataTable(@"select proll_PayStructureMaster.StructureID,proll_PayrollClass_Master.PayrollClassID,
                                                        Convert(nvarchar(10),proll_PayrollClass_Master.PeriodFrom,105) as PeriodFrom,
                                                        Convert(nvarchar(10),proll_PayrollClass_Master.PeriodTo,105) as PeriodTo,
                                                        a.YYMM,a.Period from proll_PayStructureMaster
                                                        inner join proll_PayrollClass_Master
                                                        on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
                                                        left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period 
                                                        from proll_PeriodGeneration)a
                                                        on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID
                                                        where proll_PayStructureMaster.StructureID='" + PayStructureID + "' AND YYMM<='" + ActiveYYMM + "'");
                else
                    dt = oDBEngine.GetDataTable(@"select distinct 
                                                        Convert(nvarchar(10),proll_PayrollClass_Master.PeriodFrom,105) as PeriodFrom,
                                                        Convert(nvarchar(10),proll_PayrollClass_Master.PeriodTo,105) as PeriodTo,
                                                        a.YYMM,a.Period from proll_PayStructureMaster
                                                        inner join proll_PayrollClass_Master
                                                        on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
                                                        left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period 
                                                        from proll_PeriodGeneration)a
                                                        on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID
                                                        ");

                list = (from DataRow dr in dt.Rows
                        select new PeriodList()
                        {
                            YYMM = Convert.ToString(dr["YYMM"]),
                            Period = Convert.ToString(dr["Period"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        // Rev Sanchita
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayrollEmployeeTable(string SearchKey, string PayStructureID)
        {
            DataTable dt = new DataTable();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                dt = oDBEngine.GetDataTable(@"Select Employee_Code,EmployeeUniqueCode,Employee_Name From proll_EmployeeAttactchment
                                                        inner Join v_proll_EmployeeList On proll_EmployeeAttactchment.EmployeeCode=v_proll_EmployeeList.Employee_Code
                                                        Where PayStructureCode='" + PayStructureID + "'");
            }
            Session["EmployeeDetails"] = dt;

            DataTable dtDesign = new DataTable();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                dtDesign = oDBEngine.GetDataTable(@"select DesignName as FullDefaultDesign,replace(DesignName,'~N','') as DesignName from proll_PayslipDesignMap PM inner join proll_PayslipCong P on P.payConfig_ID=PM.payConfig_ID and P.StructureId ='" + PayStructureID + "'");
            }
            Session["dtSelectedDesign"] = dtDesign;

            return "Success";
        }

        // End of Rev Sanchita
       // Rev Deep
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmpLeaveRegisterLists(string SearchKey)
        {
            List<EmpLeaveRegList> list = new List<EmpLeaveRegList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                string cntType = "EM";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(@"select distinct tmc.cnt_internalId ID,(tmc.cnt_firstName +' '+ tmc.cnt_lastName)  as Name from tbl_master_contact tmc
                                                        inner join tbl_master_employee tme on tmc.cnt_internalId = tme.emp_contactId
                                                        inner join proll_Leave_Transaction plt on tme.emp_contactId = plt.EmployeeCode
                                                        Where cnt_contactType='" + cntType + "' AND cnt_firstName like '%" + SearchKey + "%'");


                list = (from DataRow dr in dt.Rows
                        select new EmpLeaveRegList()
                        {
                            id = Convert.ToString(dr["ID"]),
                            Name = Convert.ToString(dr["Name"]),
                        }).ToList();
            }

            return list;
        }
       // End Rev Deep


    }
    // Rev Deep
 
    public class EmpLeaveRegList
    {
        public string id { get; set; }
        public string Name { get; set; }
    }
    // End Rev Deep
    public class StructureList
    {
        public string StructureID { get; set; }
        public string StructureName { get; set; }
    }
    public class PeriodList
    {
        public string YYMM { get; set; }
        public string Period { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
    }
    public class EmployeeList
    {
        public string EmployeeCode { get; set; }
        public string EmployeeUniqueCode { get; set; }
        public string EmployeeName { get; set; }
    }
}
