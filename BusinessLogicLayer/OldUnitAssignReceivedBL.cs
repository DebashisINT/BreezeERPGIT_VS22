using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class oldunit
    {
        public int Id { get; set; }
        public int Invoice_Id { get; set; }
        public string financial_year { get; set; }
        public string assign_from_branch { get; set; }
        public string assignee_remark { get; set; }
        public int assigned_by { get; set; }
        public string assign_to_branch { get; set; }
        public string receiver_remark { get; set; }
        public int received_by { get; set; }
        public DateTime assigned_on { get; set; }
        public DateTime received_on { get; set; }
        public int current_status { get; set; }
        public string company_Id { get; set; }
    }
    public class tbl_trans_Oldunit_details
    {
        public int oldUnit_id { get; set; }
        public int doc_id { get; set; }
        public string ProductID { get; set; }
        public int oldUnit_qty { get; set; }
        public int oldUnit_Uom { get; set; }
        public DateTime createDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string SrlNo { get; set; }
        public string sProducts_Name { get; set; }
        public string sProducts_Description { get; set; }
        public decimal oldUnit_value { get; set; }
        public string UOM_Name { get; set; }
        public string Quantity_Received { get; set; }
        public string Balance_Quantity { get; set; }
    }
    public class OldUnitAssignReceivedBL
    {
        public DataTable GetBranch(int LoggedInBranchid, string BranchList)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranch");
            proc.AddIntegerPara("@LoggedInBranchid", LoggedInBranchid);
            proc.AddVarcharPara("@BranchList", 50, BranchList);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetOldUnitAssignData(string userbranchlist, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@userbranchlist", -1, userbranchlist);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetOldUnitReceivedData(string Branch, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@Branch", -1, Branch);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetOldUnitDetailsByInvoice(int Invoice_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetOldUnitDetails");
            proc.AddIntegerPara("@doc_id", Invoice_Id);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetOldUnitByInvoice(int Invoice_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetOldUnit");
            proc.AddIntegerPara("@Invoice_Id", Invoice_Id);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable getBranchListByHierchy(string userbranchhierchy)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("SP_OldUnit_CRUD");
           proc.AddVarcharPara("@Action", 100, "GetBranchListForOldUnit");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           ds = proc.GetTable();
           return ds;
       }
        public DataTable GetOldUnitReceivedByInvoice(int Invoice_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetOldUnitReceived");
            proc.AddIntegerPara("@Invoice_Id", Invoice_Id);
            dt = proc.GetTable();
            return dt;
        }
        public static string AssignedBranch(object _Obj1)
        {
            try
            {
                dynamic _Obj = _Obj1;
                //oldunit _Obj = (oldunit)_Obj1;
                //_Obj = (oldunit)_Obj1;
                string msg = "";
                ProcedureExecute proc = new ProcedureExecute("SP_OldUnit_CRUD");

                proc.AddIntegerPara("@Invoice_Id", _Obj.Invoice_Id);
                proc.AddVarcharPara("@financial_year", 100, _Obj.financial_year);
                proc.AddVarcharPara("@assign_from_branch", 100, _Obj.assign_from_branch);
                proc.AddVarcharPara("@assignee_remark", 100, _Obj.assignee_remark);
                proc.AddIntegerPara("@assigned_by", _Obj.assigned_by);
                proc.AddVarcharPara("@assign_to_branch", 100, _Obj.assign_to_branch);
                proc.AddDateTimePara("@assigned_on", DateTime.Now);
                proc.AddIntegerPara("@current_status", 1);
                proc.AddVarcharPara("@company_Id", 100, _Obj.company_Id);
                proc.AddVarcharPara("@Action", 100, "AssignBranch");

                int _Ret = proc.RunActionQuery() > 0 ? 1 : -1;

                if (_Ret == 1)
                {
                    msg = "Branch assigned successfully.";
                }
                else
                {
                    msg = "Error occured.";
                }
                return msg;
            }
            catch (Exception ex)
            {
                return "Error occured.";
            }
        }
        public static string ReceivedBranch(object _Obj1)
        {
            try
            {
                dynamic _Obj = _Obj1;
                //oldunit _Obj = new oldunit();
                //_Obj = (oldunit)_Obj1;
                string msg = "";
                ProcedureExecute proc = new ProcedureExecute("SP_OldUnit_CRUD");

                proc.AddIntegerPara("@Invoice_Id", _Obj.Invoice_Id);
                proc.AddVarcharPara("@receiver_remark", 100, _Obj.receiver_remark);
                proc.AddIntegerPara("@current_status", _Obj.current_status);
                proc.AddDateTimePara("@received_on", DateTime.Now);
                proc.AddIntegerPara("@received_by", _Obj.received_by);
                proc.AddVarcharPara("@Action", 100, "AssignBranch");

                int _Ret = proc.RunActionQuery() > 0 ? 1 : -1;

                if (_Ret == 1)
                {
                    msg = "old unit received successfully.";
                }
                else
                {
                    msg = "Error occured.";
                }
                return msg;
            }
            catch (Exception ex)
            {
                return "Error occured.";
            }
        }

        public static int SaveOldUnit(DataTable oldUnitTable, string Invoice_Id, string OldUnitAdvice_DocDate, string OldUnitAdvice_DocNo)
        {
            try
            {
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                SqlCommand cmd = new SqlCommand("SP_OldUnit_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "OldUnitSave");
                cmd.Parameters.AddWithValue("@Invoice_Id", Convert.ToInt32(Invoice_Id));
                cmd.Parameters.AddWithValue("@OldUnitDetails", oldUnitTable);
                cmd.Parameters.AddWithValue("@OldUnitAdvice_DocDate", OldUnitAdvice_DocDate);
                cmd.Parameters.AddWithValue("@OldUnitAdvice_DocNo", OldUnitAdvice_DocNo);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                return 1;
            }
            catch (Exception ex) { return 0; }
        }

        public static DataSet GetOldUnitDataByInvoiceId(string Invoice_Id)
        {
            DataSet oldUnitTable = null;
            ProcedureExecute proc = new ProcedureExecute("SP_OldUnit_CRUD");

            proc.AddIntegerPara("@Invoice_Id", Convert.ToInt32(Invoice_Id));
            proc.AddVarcharPara("@Action", 100, "GetOldUnitData");

            oldUnitTable = proc.GetDataSet();

            if (oldUnitTable != null && oldUnitTable.Tables.Count > 0)
                return oldUnitTable;
            else
                return null;
        }

        public DataTable GetOldUnitListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_OLDUNITDETAILS");
            proc.AddVarcharPara("@Action", 500, "OldUnitReceived");
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FromDate", 50, fromdate);
            proc.AddVarcharPara("@ToDate", 50, todate);
            proc.AddVarcharPara("@Branch", 3000, (branch == "0") ? userbranchlist : branch);
            dt = proc.GetTable();
            return dt;
        }

    }
}
