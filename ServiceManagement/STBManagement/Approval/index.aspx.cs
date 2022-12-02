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

namespace ServiceManagement.STBManagement.Approval
{
    public partial class index : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Approval/index.aspx");
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
                ProcedureExecute procd = new ProcedureExecute("Prc_STBApproveCount");
                procd.AddPara("@BranchID", userbranch);
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    divTotal.InnerHtml = Convert.ToString(dtCount.Rows[0]["Total"]);
                    divMRCancelation.InnerHtml = Convert.ToString(dtCount.Rows[0]["MRCancelation"]);
                    divWRCancelation.InnerHtml = Convert.ToString(dtCount.Rows[0]["WRCancelation"]);
                    divHolds.InnerHtml = Convert.ToString(dtCount.Rows[0]["Holds"]);
                    divSTBRequisition.InnerHtml = Convert.ToString(dtCount.Rows[0]["STBRequisition"]);
                    divInventoryCancelation.InnerHtml = Convert.ToString(dtCount.Rows[0]["InventoryCancelation"]);
                    divDirectorApproval.InnerHtml = Convert.ToString(dtCount.Rows[0]["DirectorApproval"]);
                    divReturnRequisition.InnerHtml = Convert.ToString(dtCount.Rows[0]["ReturnRequisition"]);
                }
                else
                {
                    divTotal.InnerHtml = "0";
                    divMRCancelation.InnerHtml = "0";
                    divWRCancelation.InnerHtml = "0";
                    divHolds.InnerHtml = "0";
                    divSTBRequisition.InnerHtml = "0";
                    divInventoryCancelation.InnerHtml = "0";
                    divDirectorApproval.InnerHtml = "0";
                    divReturnRequisition.InnerHtml = "0";
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
                if (Filterby == "ALL")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else if (Filterby == "MRCancelation")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Money Receipt"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Money Receipt"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else if (Filterby == "WRCancelation")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Wallet Recharge"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Wallet Recharge"
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else if (Filterby == "STBRequisition")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "STB Requisition" && d.CancelStatus == hdnFilterByStatus.Value
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "STB Requisition" && d.CancelStatus == hdnFilterByStatus.Value
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else if (Filterby == "STBReturnRequisition")
                {
                    if (strBranchID == "0")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Return Requisition" 
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.V_STB_ApprovalLists
                                where branchidlist.Contains(Convert.ToInt32(d.Branch))
                                && d.DocType == "Return Requisition" 
                                orderby d.DocumentDate descending
                                select d;
                        e.QueryableSource = q;
                    }
                }
                else
                {
                    var q = from d in dc.V_STB_ApprovalLists
                            where d.Branch == -1
                            orderby d.DocumentDate descending
                            select d;
                    e.QueryableSource = q;
                }

            }
            else
            {
                var q = from d in dc.V_STB_ApprovalLists
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
            string filename = "Approve";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Approve";
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
                ProcedureExecute procd = new ProcedureExecute("Prc_STBApproveCount");
                procd.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                DataTable dtCount = procd.GetTable();

                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow item in dtCount.Rows)
                    {
                        ret.Total = item["Total"].ToString();
                        ret.MRCancelation = item["MRCancelation"].ToString();
                        ret.WRCancelation = item["WRCancelation"].ToString();
                        ret.Holds = item["Holds"].ToString();
                        ret.STBRequisition = item["STBRequisition"].ToString();
                        ret.InventoryCancelation = item["InventoryCancelation"].ToString();
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
        public static string ClickOnApproveM(string ID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    string DataBase = con.Database;

                    DataTable dt = oDBEngine.GetDataTable("Select DirectorApprovalRequiredEmployee from STB_STBRequisitionHeader where STBRequisition_id=" + ID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["DirectorApprovalRequiredEmployee"].ToString() + "~" + DataBase;
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
            public String MRCancelation { get; set; }
            public String WRCancelation { get; set; }
            public String Holds { get; set; }
            public String STBRequisition { get; set; }
            public String InventoryCancelation { get; set; }
            public String DirectorApproval { get; set; }
        }

        [WebMethod]
        public static STB_MoneyReceptHeader ReceptDetails(String ReceiptID, String module)
        {
            STB_MoneyReceptHeader ret = new STB_MoneyReceptHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBMoneyReceiptDetails");
                proc.AddVarcharPara("@ACTION", 500, "MoneyReceiptDetailsView");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<MoneyReceptDetailsList> DetailsList = new List<MoneyReceptDetailsList>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.EntryType = item["Type"].ToString();
                        ret.DocumentNumber = item["DocumentNumber"].ToString();
                        ret.DocumentDate = item["DocumentDate"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.Branch = item["Branch"].ToString();
                        ret.ContactNo = item["ContactNo"].ToString();

                        ret.CancelStatus = item["CancelStatus"].ToString();
                        ret.ReasonForCancel = item["ReasonForCancel"].ToString();
                        ret.Cancel_Create_by = item["Cancel_Create_by"].ToString();
                        ret.Cancel_Create_date = item["Cancel_Create_date"].ToString();
                        ret.IsCancel = item["IsCancel"].ToString();

                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = APIHelperMethods.ToModelList<MoneyReceptDetailsList>(ds.Tables[1]);


                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        public class STB_MoneyReceptHeader
        {
            public string EntryType { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentDate { get; set; }
            public string EntityCode { get; set; }
            public string NetworkName { get; set; }
            public string ContactPerson { get; set; }
            public string Branch { get; set; }
            public string ContactNo { get; set; }

            public string CancelStatus { get; set; }
            public string ReasonForCancel { get; set; }
            public string Cancel_Create_by { get; set; }
            public string Cancel_Create_date { get; set; }
            public string IsCancel { get; set; }

            public List<MoneyReceptDetailsList> DetailsList { get; set; }
        }

        public class MoneyReceptDetailsList
        {

            public Int64 Payment_Id { get; set; }
            public Int64 MoneyReceipt_ID { get; set; }
            public String Payment_Mode { get; set; }
            public Decimal Payment_Amount { get; set; }
            public String Cheque_No { get; set; }
            public String Cheque_date { get; set; }
            public String Ref_No { get; set; }
            public String PaymentDetails_BankName { get; set; }
            public String PaymentDetails_BranchName { get; set; }
            public String Remarks { get; set; }

        }

        [WebMethod]
        public static STB_WalletRechargeHeader WalletRechargeDetails(String WalletID, String module)
        {
            STB_WalletRechargeHeader ret = new STB_WalletRechargeHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
                proc.AddVarcharPara("@ACTION", 500, "WalletRechargeDetailsView");
                proc.AddPara("@WalletRecharge_ID", WalletID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<WalletRechargeDetailsList> DetailsList = new List<WalletRechargeDetailsList>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.EntryType = item["Type"].ToString();
                        ret.DocumentNumber = item["DocumentNumber"].ToString();
                        ret.DocumentDate = item["DocumentDate"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.Branch = item["Branch"].ToString();
                        ret.ContactNo = item["ContactNo"].ToString();
                        ret.DAS = item["DAS"].ToString();
                        ret.CancelStatus = item["CancelStatus"].ToString();
                        ret.ReasonForCancel = item["ReasonForCancel"].ToString();
                        ret.Cancel_Create_by = item["Cancel_Create_by"].ToString();
                        ret.Cancel_Create_date = item["Cancel_Create_date"].ToString();
                        ret.IsCancel = item["IsCancel"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = APIHelperMethods.ToModelList<WalletRechargeDetailsList>(ds.Tables[1]);


                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        public class STB_WalletRechargeHeader
        {
            public string EntryType { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentDate { get; set; }
            public string EntityCode { get; set; }
            public string NetworkName { get; set; }
            public string ContactPerson { get; set; }
            public string Branch { get; set; }
            public string ContactNo { get; set; }
            public string DAS { get; set; }
            public string CancelStatus { get; set; }
            public string ReasonForCancel { get; set; }
            public string Cancel_Create_by { get; set; }
            public string Cancel_Create_date { get; set; }
            public string IsCancel { get; set; }

            public List<WalletRechargeDetailsList> DetailsList { get; set; }
        }

        public class WalletRechargeDetailsList
        {

            public Int64 Payment_Id { get; set; }
            public Int64 WalletRecharge_ID { get; set; }
            public String Payment_Mode { get; set; }
            public Decimal Payment_Amount { get; set; }
            public String Cheque_No { get; set; }
            public String Cheque_date { get; set; }
            public String Ref_No { get; set; }
            public String Payment_BankName { get; set; }
            public String Payment_BranchName { get; set; }
            public String Remarks { get; set; }

        }

        [WebMethod]
        public static STB_STBRequisitionHeader STBRequisitionDetails(String STBRequisitionID, String module)
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
            public string NoPayment  { get; set; }
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


        [WebMethod]
        public static STB_STBRequisitionHeader STBReturnRequisitionDetails(String STBReturnRequisitionID, String module)
        {
            STB_STBRequisitionHeader ret = new STB_STBRequisitionHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
                proc.AddVarcharPara("@ACTION", 500, "ShowDetailsForHTMLView");
                proc.AddPara("@ReturnReq_id", STBReturnRequisitionID);
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

                    ret.DetailsList = DetailsList;
                }
            }
            return ret;
        }
    }
}