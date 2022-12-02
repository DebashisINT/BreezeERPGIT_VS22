using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class payrollStructureEngine
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ActionType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureHeaderName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureName { get; set; }

     
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadShortName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DataType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CalculationType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RoundOffType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_CalculateFormula { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_DisplayFormula { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_PayHeadID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> PayHeadTypeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> DataTypeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> CalculationTypeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> RoundOffTypeList { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PClassId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool IsProrataCalculated { get; set; }

        public String Comments { get; set; }
        public List<SelectListItem> _PClassName { get; set; }
        public List<PayHeadsDetails> AllowanceDetails { get; set; }
        public List<PayHeadIDList> PayHeadList { get; set; }
        public List<PayHeadDataTypeList> PayHeadDataTypeList { get; set; }
        public List<dynamic> EmployeeHOPList { get; set; }
        public List<SelectListItem> PayHeadDetails { get; set; }
        public List<SelectListItem> FunctionDetails { get; set; }
        public List<SelectListItem> TableDetails { get; set; }

        public List<Display_PayHeadList> AllowanceHeadDetails { get; set; }
        public List<Display_PayHeadList> DeductionHeadDetails { get; set; }
        public List<Display_PayHeadList> OthersHeadDetails { get; set; }

        public List<SelectListItem> PopulateClassName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_PayrollClass_Master ", " PayrollClassID,PayrollClassName ", "IsDeleted='N'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["PayrollClassName"].ToString(),
                        Value = row["PayrollClassID"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulatePayHeadType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='PT'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateDataType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='DT'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateCalculationType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='CT'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateRoundOffType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='RO'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulatePayHeadDetailsType(string StructureID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_PayHeadMaster ", " PayHeadCode,PayHeadName ", " StructureID='" + StructureID + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Value = row["PayHeadCode"].ToString(),
                        Text = row["PayHeadCode"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateFunctionList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_Function_Master ", " (Case When FunctionName='IIF' Then FunctionName+'({ }, , )' When FunctionName='TABLE' Then FunctionName+'( , ,yymm)' Else FunctionName+'( )' End)FunctionName,FunctionDesc ", "IsActive=1");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Value = row["FunctionName"].ToString(),
                        Text = row["FunctionDesc"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateTableList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_TableFormula_Master ", " TableCode,TableName ", "IsDeleted='N'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Value = row["TableName"].ToString(),
                        Text = row["TableCode"].ToString()
                    });
                }
            }

            return items;
        }
    }
    public class PayHeadsDetails
    {
        public string PayHead { get; set; }
        public string ShortName { get; set; }
        public string PayType { get; set; }
        public string DataType { get; set; }
        public string CalculationType { get; set; }
        public string RoundOffType { get; set; }
        public string PayHeadID { get; set; }
    }
    public class PayHeadIDList
    {
        public string PayHeadID { get; set; }
        public string PayHead { get; set; }
    }
    public class PayHeadDataTypeList
    {
        public string PayHeadID { get; set; }
        public string DataType { get; set; }
    }    
    public class classPayHead
    {
        public string Keys { get; set; }
        public string Amount { get; set; }
        public string Values { get; set; }
    }
    public class Hop
    {
        public List<classPayHead> classPayHead { get; set; }
    }
    public class HopHead
    {
        public List<Hop> MainArray { get; set; }
    }
    public class _Formula_PayHeadList
    {
        public string PayHeadCode { get; set; }
        public string PayHeadName { get; set; }
    }
    public class _Formula_FunctionList
    {
        public string FunctionCode { get; set; }
        public string FunctionDesc { get; set; }
    }
    public class DisplayPayHeadList
    {
        public string DisplayIndex { get; set; }
        public string PayHeadID { get; set; }
    }
    public class Display_PayHeadList
    {
        public string PayHeadID { get; set; }
        public string PayHeadName { get; set; }

    }

    public class ExcelMap
    {
        public String Column { get; set; }

        public String Map { get; set; }
    }
}