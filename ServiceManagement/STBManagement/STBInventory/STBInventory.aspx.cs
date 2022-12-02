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
using UtilityLayer;

namespace ServiceManagement.STBManagement.STBInventory
{
    public partial class STBInventory : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/STBInventory/STBInventory.aspx");
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
                ProcedureExecute procd = new ProcedureExecute("Prc_STBInventorySTBReqCount");
                procd.AddPara("@BranchID", userbranch);
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    divTotal.InnerHtml = Convert.ToString(dtCount.Rows[0]["Total"]);
                }
                else
                {
                    divTotal.InnerHtml = "0";
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
                string BranchList = userbranch;
                if (strBranchID == "0")
                {
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.V_STBReqInventoryLists
                            where branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.DocumentDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_STBReqInventoryLists
                            where branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.DocumentDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_STBReqInventoryLists
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
                ProcedureExecute procd = new ProcedureExecute("Prc_STBInventorySTBReqCount");
                procd.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {

                    Total = dtCount.Rows[0]["Total"].ToString();
                }
            }
            return Total;
        }

        [WebMethod]
        public static string CancelInventory(String Document_ID, String Remarks, String IsGatePass, String CancelGatepassNo, String GoodsReturnVoucherNumber)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
                    proc.AddPara("@STBRequisitionID", Document_ID);
                    proc.AddPara("@InventoryRemarks", Convert.ToString(Remarks));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@Action", "CancelInventory");
                    proc.AddPara("@GatepassNo", CancelGatepassNo);
                    proc.AddPara("@IsGatePass", IsGatePass);
                    proc.AddPara("@GoodsReturnVoucherNumber", GoodsReturnVoucherNumber);
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

        [WebMethod]
        public static string UpdateInventory(String Document_ID, String Remarks, String GatepassNo, String AttachDocument)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
                    proc.AddPara("@STBRequisitionID", Document_ID);
                    proc.AddPara("@InventoryRemarks", Convert.ToString(Remarks));
                    proc.AddPara("@GatepassNo", Convert.ToString(GatepassNo));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@Action", "ApproveInventory");
                    proc.AddPara("@AttachDocument", AttachDocument);
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
            string filename = "Pending Inventory";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Pending Inventory";
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

        [WebMethod]
        public static STB_STBRequisitionHeader STBRequisitionDetails(String STBRequisitionID)
        {
            STB_STBRequisitionHeader ret = new STB_STBRequisitionHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionDetails");
                proc.AddVarcharPara("@ACTION", 500, "ShowDetailsForHTMLView");
                proc.AddPara("@STBRequisition_id", STBRequisitionID);
                ds = proc.GetDataSet();

                List<STBRequisitionDetailsList> DetailsList = new List<STBRequisitionDetailsList>();
                List<STBRequisitionDetailsList> DetailsList2 = new List<STBRequisitionDetailsList>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.DocumentNumber = item["DocumentNumber"].ToString();
                        ret.Location = item["branch_description"].ToString();
                        ret.DocumentDate = item["DocumentDate"].ToString();
                        ret.RequisitionType = item["RequisitionType"].ToString();
                        ret.RequisitionFor = item["RequisitionFor"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.ContactNo = item["ContactNumber"].ToString();
                        ret.DAS = item["DAS"].ToString();
                        ret.PayUsingAcountBalance = item["PayUsingAcountBalance"].ToString();
                        ret.NoPayment = item["NoPayment"].ToString();
                        ret.PaymentRelatedRemarksNotes = item["PaymentRelatedRemarks"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = APIHelperMethods.ToModelList<STBRequisitionDetailsList>(ds.Tables[1]);
                    }

                    if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        DetailsList2 = APIHelperMethods.ToModelList<STBRequisitionDetailsList>(ds.Tables[2]);
                    }
                    ret.DetailsList = DetailsList;
                    ret.DetailsList2 = DetailsList2;
                }
            }
            return ret;
        }

        public class STB_STBRequisitionHeader
        {
            public string DocumentNumber { get; set; }
            public string Location { get; set; }
            public string DocumentDate { get; set; }
            public string RequisitionType { get; set; }
            public string RequisitionFor { get; set; }
            public string EntityCode { get; set; }
            public string NetworkName { get; set; }
            public string ContactPerson { get; set; }
            public string ContactNo { get; set; }
            public string DAS { get; set; }
            public string PayUsingAcountBalance { get; set; }
            public string NoPayment { get; set; }
            public string PaymentRelatedRemarksNotes { get; set; }

            public List<STBRequisitionDetailsList> DetailsList { get; set; }
            public List<STBRequisitionDetailsList> DetailsList2 { get; set; }
        }

        public class STBRequisitionDetailsList
        {
            public long SrlNo { get; set; }
            public String Model { get; set; }
            public Decimal UnitPrice { get; set; }
            public Decimal Quantity { get; set; }
            public Decimal Amount { get; set; }
            public String Remarks { get; set; }
        }
    }
}