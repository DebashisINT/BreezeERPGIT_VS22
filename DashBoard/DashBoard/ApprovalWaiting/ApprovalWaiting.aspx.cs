using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.ApprovalWaiting
{
    public partial class ApprovalWaiting : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        CommonBL ComBL = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string DefaultBranchInLogin = ComBL.GetSystemSettingsResult("DefaultBranchInLogin");
                //if (!String.IsNullOrEmpty(DefaultBranchInLogin))
                //{
                //    if (DefaultBranchInLogin.ToUpper().Trim() == "YES")
                //    {
                //        userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //    }
                //    else
                //    {                       
                //        userbranchHierachy = EmployeeBranchMap();
                //    }
                //}
                //else
                //{
                //    userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //}

                userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                Session["EmployeeBranchMapping"] = userbranchHierachy;

                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "Approval Waiting");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    divBranchRequisiton.Visible = Convert.ToBoolean(dt.Rows[0]["AWBranchRequisition"]);
                    divPurchaseIndent.Visible = Convert.ToBoolean(dt.Rows[0]["AWPurchaseIndent"]);
                    divProjectIndent.Visible = Convert.ToBoolean(dt.Rows[0]["AWProjectIndent"]);
                    divPurchaseOrder.Visible = Convert.ToBoolean(dt.Rows[0]["AWPurchaseOrder"]);
                    divProjectPurchaseOrder.Visible = Convert.ToBoolean(dt.Rows[0]["AWProjectPurchaseOrder"]);
                    divSalesOrder.Visible = Convert.ToBoolean(dt.Rows[0]["AWSalesOrder"]);
                    divProjectSalesOrder.Visible = Convert.ToBoolean(dt.Rows[0]["AWProjectSalesOrder"]);
                }
                else
                {
                    divBranchRequisiton.Visible = Convert.ToBoolean(0);
                    divPurchaseIndent.Visible = Convert.ToBoolean(0);
                    divProjectIndent.Visible = Convert.ToBoolean(0);
                    divPurchaseOrder.Visible = Convert.ToBoolean(0);
                    divProjectPurchaseOrder.Visible = Convert.ToBoolean(0);
                    divSalesOrder.Visible = Convert.ToBoolean(0);
                    divProjectSalesOrder.Visible = Convert.ToBoolean(0);

                }
            }
        }

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        [WebMethod]
        public static object GetAllCountData(string action)
        {
            List<getCount> lEfficency = new List<getCount>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getCount()
                              {
                                  BranchRequisitonCount = Convert.ToString(dr["BranchRequisitonCount"]),
                                  PurchaseIndentCount = Convert.ToString(dr["PurchaseIndentCount"]),
                                  ProjectIndentCount = Convert.ToString(dr["ProjectIndentCount"]),
                                  PurchaseOrderCount = Convert.ToString(dr["PurchaseOrderCount"]),
                                  ProjectPurchaseOrderCount = Convert.ToString(dr["ProjectPurchaseOrderCount"]),
                                  SalesOrderCount = Convert.ToString(dr["SalesOrderCount"]),
                                  ProjectSalesOrderCount = Convert.ToString(dr["ProjectSalesOrderCount"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetBranchRequisiton(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "BR");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetPurchaseIndent(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "PI");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetProjectIndent(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "PI");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetPurchaseOrder(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "PO");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetProjectPurchaseOrder(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "PO");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetSalesOrder(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "SO");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object GetProjectSalesOrder(string action)
        {
            List<getDetailsList> lEfficency = new List<getDetailsList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@Action", 100, action);
            proc.AddVarcharPara("@DocType", 100, "SO");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getDetailsList()
                              {
                                  Branch = Convert.ToString(dr["branch_description"]),
                                  DocumentNo = Convert.ToString(dr["Number"]),
                                  Date = Convert.ToString(dr["CreateDate"]),
                                  RequestedBy = Convert.ToString(dr["craetedby"]),
                                  Approve = Convert.ToString(dr["Approved"]),
                                  Reject = Convert.ToString(dr["Rejected"]),
                                  DocumnetId = Convert.ToString(dr["ID"])
                              }).ToList();
            }
            return lEfficency;
        }

        public class getCount
        {
            public String BranchRequisitonCount { get; set; }
            public String PurchaseIndentCount { get; set; }
            public String ProjectIndentCount { get; set; }
            public String PurchaseOrderCount { get; set; }
            public String ProjectPurchaseOrderCount { get; set; }
            public String SalesOrderCount { get; set; }
            public String ProjectSalesOrderCount { get; set; }
        }

        public class getDetailsList
        {
            public String Branch { get; set; }
            public String DocumentNo { get; set; }
            public String Date { get; set; }
            public String RequestedBy { get; set; }
            public String Approve { get; set; }
            public String Reject { get; set; }
            public String DocumnetId { get; set; }
            public String BranchId { get; set; }
        }
    }
}