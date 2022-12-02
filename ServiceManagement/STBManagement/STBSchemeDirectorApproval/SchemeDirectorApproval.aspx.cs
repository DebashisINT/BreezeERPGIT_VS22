using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ServiceManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.STBManagement.STBSchemeDirectorApproval
{
    public partial class SchemeDirectorApproval : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/STBSchemeDirectorApproval/SchemeDirectorApproval.aspx");
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
                ProcedureExecute procd = new ProcedureExecute("Prc_STBSchemeReceivedCount");
                procd.AddPara("@BranchID", userbranch);
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    divTotal.InnerHtml = Convert.ToString(dtCount.Rows[0]["Total"]);
                    divHolds.InnerHtml = Convert.ToString(dtCount.Rows[0]["Holds"]);
                    divCancel.InnerHtml = Convert.ToString(dtCount.Rows[0]["Cancelation"]);
                    divDirectorApproval.InnerHtml = Convert.ToString(dtCount.Rows[0]["DirectorApproval"]);
                }
                else
                {
                    divTotal.InnerHtml = "0";
                    divHolds.InnerHtml = "0";
                    divCancel.InnerHtml = "0";
                    divDirectorApproval.InnerHtml = "0";
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

            e.KeyExpression = "SchemeReceived_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                string BranchList = userbranch;
                if (Filterby == "ALL")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STBSchemeReceivedApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STBSchemeReceivedApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STBSchemeReceivedApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.ApprovalStatus == Filterby
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STBSchemeReceivedApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.ApprovalStatus == Filterby
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
            }
            else
            {
                var q = from d in dc.V_STBSchemeReceivedApprovalLists
                        where d.Branch == -1
                        orderby d.DocumentDate descending
                        select d;
                e.QueryableSource = q;
            }
        }

        [WebMethod]
        public static string CancelDocument(String Document_ID, String Remarks)
        {
            string output = string.Empty;
            try
            {
                string id = Convert.ToString(Document_ID.Split(',')[1]);
                string mode = Convert.ToString(Document_ID.Split(',')[0]);
                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("Prc_STBApproveCancelUpdate");
                    proc.AddPara("@Document_ID", Convert.ToString(id));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@ACTION", "CANCEL");
                    proc.AddPara("@Mode", Convert.ToString(mode));
                    DataTable dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["MSG"].ToString();
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
        public static string ApproveDocument(String Document_ID, String Remarks)
        {
            string output = string.Empty;
            try
            {
                string id = Convert.ToString(Document_ID.Split(',')[1]);
                string mode = Convert.ToString(Document_ID.Split(',')[0]);

                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("Prc_STBApproveCancelUpdate");
                    proc.AddPara("@Document_ID", Convert.ToString(id));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@ACTION", "APPROVE");
                    proc.AddPara("@Mode", Convert.ToString(mode));
                    DataTable dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["MSG"].ToString();
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
            string filename = "STB Scheme - Director Approval";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "STB Scheme - Director Approval";
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
        public static STB_ButtonCount CountButton(String UserType)
        {
            STB_ButtonCount ret = new STB_ButtonCount();
            if (HttpContext.Current.Session["userid"] != null)
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                DataTable ds = new DataTable();
                ProcedureExecute procd = new ProcedureExecute("Prc_STBSchemeReceivedCount");
                procd.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow item in dtCount.Rows)
                    {
                        ret.Total = item["Total"].ToString();
                        ret.Holds = item["Holds"].ToString();
                        ret.Cancel = item["Cancelation"].ToString();
                        ret.DirectorApproval = item["DirectorApproval"].ToString();
                        break;
                    }
                }
            }
            return ret;
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

        public class STB_ButtonCount
        {
            public String Total { get; set; }
            public String Holds { get; set; }
            public String Cancel { get; set; }
            public String DirectorApproval { get; set; }
        }

        [WebMethod]
        public static STB_SchemeReceivedHeader SchemeReceivedDetails(String SchemeReceivedID)
        {
            STB_SchemeReceivedHeader ret = new STB_SchemeReceivedHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
                proc.AddVarcharPara("@ACTION", 500, "GetDetailsHTMLView");
                proc.AddPara("@SchemeReceived_ID", SchemeReceivedID);
                ds = proc.GetDataSet();

                List<STB_SchemeReceivedDetails> DetailsList = new List<STB_SchemeReceivedDetails>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.DocumentNumber = item["DocumentNumber"].ToString();
                        ret.DocumentDate = item["DocumentDate"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.Branch = item["Branch"].ToString();
                        ret.ContactNo = item["ContactNo"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = APIHelperMethods.ToModelList<STB_SchemeReceivedDetails>(ds.Tables[1]);
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        public class STB_SchemeReceivedHeader
        {
            public string DocumentNumber { get; set; }
            public string DocumentDate { get; set; }
            public string EntityCode { get; set; }
            public string NetworkName { get; set; }
            public string ContactPerson { get; set; }
            public string Branch { get; set; }
            public string ContactNo { get; set; }
            public List<STB_SchemeReceivedDetails> DetailsList { get; set; }
        }

        public class STB_SchemeReceivedDetails
        {
            public string Model { get; set; }
            public string DeviceNumber { get; set; }
            public decimal Rate { get; set; }
            public string Remarks { get; set; }
            public string DeviceType { get; set; }
            public string Remotes { get; set; }
            public string CardAdaptor { get; set; }
        }
    }
}