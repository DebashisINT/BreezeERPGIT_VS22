using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
   public class ERPDocPendingApprovalBL
   {

        //ConditionWiseShowApprovalStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");
       public int ConditionWiseShowApprovalStatusButton(int EntityId, string branchid, string userid,string doctype)
       {

           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "ConditionWiseShowApprovalStatusButton");
           proc.AddVarcharPara("@DocType", 5, doctype);
           proc.AddIntegerPara("@EntityId", EntityId);
           proc.AddVarcharPara("@userbranchHierarchy", 5000, branchid);
           if (userid != "")
           {
               proc.AddIntegerPara("@UserId", Convert.ToInt32(userid));
           }

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int ConditionWiseShowApprovalPendingButton(int EntityId, string branchid, string userid, string doctype)
       {

           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "ConditionWiseShowApprovalPendingButton");
           proc.AddVarcharPara("@DocType", 5, doctype);
           proc.AddIntegerPara("@EntityId", EntityId);
           proc.AddVarcharPara("@userbranchHierarchy", 500, branchid);
           if (userid != "")
           {
               proc.AddIntegerPara("@UserId", Convert.ToInt32(userid));
           }

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       

       public int ConditionWiseShowApprovalDtlStatusButton(int EntityId, int branchid, string userid,string doctype)
       {

           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "ConditionWiseShowApprovalDtlStatusButton");
           proc.AddVarcharPara("@DocType", 5, doctype);
           proc.AddIntegerPara("@EntityId", EntityId);
           proc.AddIntegerPara("@BranchId", branchid);
           if (userid != "")
           {
               proc.AddIntegerPara("@UserId", Convert.ToInt32(userid));
           }

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }
       public int ConditionWiseShowStatusButton(int EntityId, int branchid, string userid)
       {
           
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "ConditionWiseShowStatusButton");
           proc.AddIntegerPara("@EntityId", EntityId);
           proc.AddIntegerPara("@BranchId", branchid);
           if(userid!="")
           {
               proc.AddIntegerPara("@UserId", Convert.ToInt32(userid));
           }
          
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }
       #region Count Pending Approval Section Start    
       public DataTable PopulateERPDocApprovalPendingCountByUserLevel(int userid,string doctype)
       { 
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateERPDocApprovalPendingCountByUserLevel");
           proc.AddIntegerPara("@UserId", userid);
           proc.AddVarcharPara("@DocType", 5, doctype);           
           dt = proc.GetTable();
           return dt;
       }

       public DataTable ProjectPopulateERPDocApprovalPendingCountByUserLevel(int userid, string doctype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 500, "ProjectPopulateERPDocApprovalPendingCountByUserLevel");
           proc.AddIntegerPara("@UserId", userid);
           proc.AddVarcharPara("@DocType", 5, doctype);
           dt = proc.GetTable();
           return dt;
       }


       #endregion Count Pending Approval Section End


       #region List Pending Approval Section Start
       public DataTable PopulateERPDocApprovalPendingListByUserLevel(int userid, string doctype)
       { 
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateERPDocApprovalPendingListByUserLevel");
           proc.AddIntegerPara("@UserId", userid);           
           proc.AddVarcharPara("@DocType", 5, doctype); 
           dt = proc.GetTable();
           return dt;
       }


       public DataTable ProjectPopulateERPDocApprovalPendingListByUserLevel(int userid, string doctype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 500, "ProjectPopulateERPDocApprovalPendingListByUserLevel");
           proc.AddIntegerPara("@UserId", userid);
           proc.AddVarcharPara("@DocType", 5, doctype);
           dt = proc.GetTable();
           return dt;
       }

       #endregion List Pending Approval Section End

       #region User Wise Document Creation
       public DataTable PopulateUserWiseERPDocCreation(int userid, string doctype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateUserWiseERPDocCreation");
           proc.AddIntegerPara("@UserId", userid);
           proc.AddVarcharPara("@DocType", 5, doctype);  
           dt = proc.GetTable();
           return dt;
       }


       public DataTable ProjectPopulateUserWiseERPDocCreation(int userid, string doctype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddVarcharPara("@Action", 100, "ProjectPopulateUserWiseERPDocCreation");
           proc.AddIntegerPara("@UserId", userid);
           proc.AddVarcharPara("@DocType", 5, doctype);  
           dt = proc.GetTable();
           return dt;
       }

       #endregion User Wise Document Creation

       #region To Allow Edit or Not Edit Using Show/Hide of Save&New/Save&Exit Button
       public DataTable IsExistsDocumentInERPDocApproveStatus(int docid,int entityId)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
           proc.AddNVarcharPara("@action", 150, "IsExistsDocumentInERPDocApproveStatus");
           proc.AddIntegerPara("@DocId", docid);
           proc.AddIntegerPara("@EntityId", entityId);
           
           dt = proc.GetTable();
           return dt;
       }

       #endregion


   }
}
