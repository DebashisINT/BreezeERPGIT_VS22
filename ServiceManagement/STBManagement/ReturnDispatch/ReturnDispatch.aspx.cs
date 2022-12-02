using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ServiceManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.ReturnDispatch
{
    public partial class ReturnDispatch : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ReturnDispatch/ReturnDispatch.aspx");
            DataTable dt = new DataTable();
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranch = EmployeeBranchMap();
                }
                else
                {
                    userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            if (!IsPostBack)
            {
                ProcedureExecute procd = new ProcedureExecute("Prc_STBReturnReqDispatchCount");
                procd.AddPara("@BranchID", userbranch);
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    divPendingDispatch.InnerHtml = Convert.ToString(dtCount.Rows[0]["PendingDispatch"]);
                    divDispatchAcknowledgment.InnerHtml = Convert.ToString(dtCount.Rows[0]["DispatchAcknowledgment"]);
                }
                else
                {
                    divPendingDispatch.InnerHtml = "0";
                    divDispatchAcknowledgment.InnerHtml = "0";
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

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string Filterby = hdnFilterby.Value;
            List<int> branchidlist;

            e.KeyExpression = "ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (hdnSearchType.Value == "Dispatch")
                {
                    string BranchList = userbranch;
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STBReturnReqDispatchLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DispatchStatus == "Dispatch"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STBReturnReqDispatchLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DispatchStatus == "Dispatch"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else if (hdnSearchType.Value == "DispatchAcknowledgment")
                {
                    string BranchList = userbranch;
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STBReturnReqDispatchLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DispatchStatus == "DispatchAcknowledgment"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STBReturnReqDispatchLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DispatchStatus == "DispatchAcknowledgment"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    var q = from d in dc.V_STBReturnReqDispatchLists
                            where d.Branch == -1
                            orderby d.DocumentDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_STBReturnReqDispatchLists
                        where d.Branch == -1
                        orderby d.DocumentDate descending
                        select d;
                e.QueryableSource = q;
            }
        }

        [WebMethod]
        public static string CountButton(String UserType)
        {
            String Total = "0";
            if (HttpContext.Current.Session["userid"] != null)
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                DataTable ds = new DataTable();
                ProcedureExecute procd = new ProcedureExecute("Prc_STBReturnReqDispatchCount");
                procd.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {

                    Total = dtCount.Rows[0]["PendingDispatch"].ToString() + "~" + dtCount.Rows[0]["DispatchAcknowledgment"].ToString();
                }
            }
            return Total;
        }

        [WebMethod]
        public static string UpdateDispatch(String Document_ID, String Remarks)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("Prc_STBReturnReqDispatchUpdate");
                    proc.AddPara("@Action", "UpdateDispatch");
                    proc.AddPara("@STBReturnReqID", Document_ID);
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddPara("@USER_ID", user_id);
                    int i = proc.RunActionQuery();
                    if (i > 0)
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static string DispatchAcknowledgmentUpdate(String Document_ID, String Remarks, String DispatchAcknowledgment)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("Prc_STBReturnReqDispatchUpdate");
                    proc.AddPara("@Action", "DispatchAcknowledgmentUpdate");
                    proc.AddPara("@STBReturnReqID", Document_ID);
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddPara("@DispatchAcknowledgmentType", Convert.ToString(DispatchAcknowledgment));
                    proc.AddPara("@USER_ID", user_id);
                    int i = proc.RunActionQuery();
                    if (i > 0)
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static string DeleteInventory(String Document_ID)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
                    proc.AddPara("@STBRequisitionID", Document_ID);
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@Action", "DeleteInventory");
                    DataTable dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["msg"].ToString();
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            exporter.GridViewID = "gridStatus";
            string filename = "Return Dispatch";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Return Dispatch";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        [WebMethod]
        public static string CheckWorkingRoster(string module_ID)
        {
            CommonBL ComBL = new CommonBL();
            string STBTransactionsRestrictBeyondTheWorkingDays = ComBL.GetSystemSettingsResult("STBTransactionsRestrictBeyondTheWorkingDays");
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    if (STBTransactionsRestrictBeyondTheWorkingDays.ToUpper() == "YES")
                    {
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBModuleRosterStatus");
                        proc.AddPara("@ModuleId", module_ID);
                        proc.AddPara("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
                        DataSet ds = proc.GetDataSet();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "true")
                            {
                                output = "true";
                            }
                            else if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "false")
                            {

                                output = "false~" + ds.Tables[1].Rows[0]["BeginTime"].ToString() + "~" + ds.Tables[1].Rows[0]["EndTime"].ToString();
                            }

                        }
                        else
                        {
                            output = "false";
                        }
                    }
                    else
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
    }
}