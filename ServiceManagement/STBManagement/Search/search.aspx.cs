using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
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

namespace ServiceManagement.STBManagement.Search
{
    public partial class search : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        string userbranchHierachy = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvAssignJobBL obj = new SrvAssignJobBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranchHierachy = EmployeeBranchMap();
                }
                else
                {
                    userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            Session["UserBranchMapID"] = userbranchHierachy;
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            if (!IsPostBack)
            {
                //  string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                // ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                }
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            //PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            //DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            //ddlBranch.DataSource = branchtable;
            //ddlBranch.DataValueField = "branch_id";
            //ddlBranch.DataTextField = "branch_description";
            //ddlBranch.DataBind();
            //ddlBranch.SelectedIndex = 0;
        }

        [WebMethod]
        public static List<DocumentList> SearchList(String DocumentNo, String EntityCode, String Branch)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<DocumentList> listStatues = new List<DocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSearchModule");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@DocumentNo", DocumentNo);
            proc.AddPara("@EntityCode", EntityCode);
            if (Branch == "0")
            {
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@BranchID", Branch);
            }
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {

                listStatues = (from DataRow item in ds.Rows
                               select new DocumentList()
                               {
                                   MoneyReceipt_ID = item["MoneyReceipt_ID"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["CancelStatus"].ToString(),
                                   ReasonForCancel = item["ReasonForCancel"].ToString(),
                                   Action = item["Action"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Model = item["Model"].ToString(),
                                   Payment_Mode = item["Payment_Mode"].ToString(),
                                   Cheque_No = item["Cheque_No"].ToString(),
                                   Cheque_date = item["Cheque_date"].ToString(),
                                   Ref_No = item["Ref_No"].ToString(),
                                   PaymentDetails_BankName = item["PaymentDetails_BankName"].ToString(),
                                   PaymentDetails_BranchName = item["PaymentDetails_BranchName"].ToString(),
                                   branch_description = item["branch_description"].ToString(),
                                   Approve_bY = item["Approve_bY"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Approve_Amount = item["Approve_Amount"].ToString(),
                                   Approve_QTY = item["Approve_QTY"].ToString(),

                                   Dispatch = item["IsDispatch"].ToString(),
                                   DisptAck = item["IsDispatchAcknowledgment"].ToString(),
                                   AckRemarks = item["AckRemarks"].ToString(),
                                   Director = item["Director"].ToString(),
                                   InvUpdatedBy = item["InvUpdatedBy"].ToString(),
                                   InvUpdatedOn = item["InvUpdatedOn"].ToString(),
                                   RejectRemarks = item["RejectRemarks"].ToString(),
                                   HoldRemarks = item["HoldRemarks"].ToString(),
                                   CancelRemarks = item["CancelRemarks"].ToString()
                               }).ToList();

            }
            return listStatues;
        }


        [WebMethod]
        public static List<WalletRechargeDocumentList> WalletSearchList(String DocumentNo, String EntityCode, String Branch)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<WalletRechargeDocumentList> listStatues = new List<WalletRechargeDocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@DocumentNo", DocumentNo);
            proc.AddPara("@EntityCode", EntityCode);
            if (Branch == "0")
            {
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@BranchID", Branch);
            }
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {

                listStatues = (from DataRow item in ds.Rows
                               select new WalletRechargeDocumentList()
                               {
                                   WalletRecharge_ID = item["WalletRecharge_ID"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["CancelStatus"].ToString(),
                                   Action = item["Action"].ToString()
                               }).ToList();

            }
            return listStatues;
        }

        [WebMethod]
        public static List<DocumentList> MoneyReceiptSearchList(String DocumentNo, String EntityCode, String Branch)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<DocumentList> listStatues = new List<DocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSearchModule");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@DocumentNo", DocumentNo);
            proc.AddPara("@EntityCode", EntityCode);
            if (Branch == "0")
            {
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@BranchID", Branch);
            }
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = (from DataRow item in ds.Rows
                               select new DocumentList()
                               {
                                   MoneyReceipt_ID = item["MoneyReceipt_ID"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["CancelStatus"].ToString(),
                                   ReasonForCancel = item["ReasonForCancel"].ToString(),
                                   Action = item["Action"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   Amount = item["Amount"].ToString(),
                                   ReqFor = item["RequisitionFor"].ToString(),
                                   InvUpdated = item["InvUpdated"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Model = item["Model"].ToString(),
                                   Payment_Mode = item["Payment_Mode"].ToString(),
                                   Cheque_No = item["Cheque_No"].ToString(),
                                   Cheque_date = item["Cheque_date"].ToString(),
                                   Ref_No = item["Ref_No"].ToString(),
                                   PaymentDetails_BankName = item["PaymentDetails_BankName"].ToString(),
                                   PaymentDetails_BranchName = item["PaymentDetails_BranchName"].ToString(),
                                   branch_description = item["branch_description"].ToString(),
                                   Approve_bY = item["Approve_bY"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Approve_Amount = item["Approve_Amount"].ToString(),
                                   Approve_QTY = item["Approve_QTY"].ToString(),
                                   Dispatch = item["IsDispatch"].ToString(),
                                   DisptAck = item["IsDispatchAcknowledgment"].ToString(),
                                   AckRemarks = item["AckRemarks"].ToString(),
                                   Director = item["Director"].ToString(),
                                   InvUpdatedBy = item["InvUpdatedBy"].ToString(),
                                   InvUpdatedOn = item["InvUpdatedOn"].ToString(),
                                   RejectRemarks = item["RejectRemarks"].ToString(),
                                   HoldRemarks = item["HoldRemarks"].ToString(),
                                   CancelRemarks = item["CancelRemarks"].ToString()
                               }).ToList();

            }
            return listStatues;
        }

        [WebMethod]
        public static List<DocumentList> MoneyReceiptDateWiseSearchList(String hfFromDate, String hfToDate)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<DocumentList> listStatues = new List<DocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSearchModule");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@FromDate", hfFromDate);
            proc.AddPara("@ToDate", hfToDate);
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = (from DataRow item in ds.Rows
                               select new DocumentList()
                               {
                                   MoneyReceipt_ID = item["MoneyReceipt_ID"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["CancelStatus"].ToString(),
                                   ReasonForCancel = item["ReasonForCancel"].ToString(),
                                   Action = item["Action"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   Amount = item["Amount"].ToString(),
                                   ReqFor = item["RequisitionFor"].ToString(),
                                   InvUpdated = item["InvUpdated"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Model = item["Model"].ToString(),
                                   Payment_Mode = item["Payment_Mode"].ToString(),
                                   Cheque_No = item["Cheque_No"].ToString(),
                                   Cheque_date = item["Cheque_date"].ToString(),
                                   Ref_No = item["Ref_No"].ToString(),
                                   PaymentDetails_BankName = item["PaymentDetails_BankName"].ToString(),
                                   PaymentDetails_BranchName = item["PaymentDetails_BranchName"].ToString(),
                                   branch_description = item["branch_description"].ToString(),
                                   Approve_bY = item["Approve_bY"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Approve_Amount = item["Approve_Amount"].ToString(),
                                   Approve_QTY = item["Approve_QTY"].ToString(),
                                   Dispatch = item["IsDispatch"].ToString(),
                                   DisptAck = item["IsDispatchAcknowledgment"].ToString(),
                                   AckRemarks = item["AckRemarks"].ToString(),
                                   Director = item["Director"].ToString(),
                                   InvUpdatedBy = item["InvUpdatedBy"].ToString(),
                                   InvUpdatedOn = item["InvUpdatedOn"].ToString(),
                                   RejectRemarks = item["RejectRemarks"].ToString(),
                                   HoldRemarks = item["HoldRemarks"].ToString(),
                                   CancelRemarks = item["CancelRemarks"].ToString()
                               }).ToList();

            }
            return listStatues;
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

        public class DocumentList
        {
            public String MoneyReceipt_ID { get; set; }
            public String DocumentNumber { get; set; }
            public String DocumentDate { get; set; }
            public String Type { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNo { get; set; }
            public String Location { get; set; }
            public String DAS { get; set; }
            public String Enterby { get; set; }
            public String EnterOn { get; set; }

            public String Status { get; set; }
            public String ReasonForCancel { get; set; }
            public String Action { get; set; }
            public String ModuleType { get; set; }

            public String Amount { get; set; }
            public String ReqFor { get; set; }
            public String InvUpdated { get; set; }

            public String UnitPrice { get; set; }
            public String Quantity { get; set; }
            public String Model { get; set; }
            public String Payment_Mode { get; set; }
            public String Cheque_No { get; set; }
            public String Cheque_date { get; set; }
            public String Ref_No { get; set; }
            public String PaymentDetails_BankName { get; set; }
            public String PaymentDetails_BranchName { get; set; }
            public String branch_description { get; set; }

            public String Approve_bY { get; set; }
            public String Approve_On { get; set; }
            public String Approve_Amount { get; set; }
            public String Approve_QTY { get; set; }

            public String Dispatch { get; set; }
            public String DisptAck { get; set; }
            public String AckRemarks { get; set; }

            public String Director { get; set; }

            public String InvUpdatedBy { get; set; }
            public String InvUpdatedOn { get; set; }

            public String RejectRemarks { get; set; }
            public String HoldRemarks { get; set; }
            public String CancelRemarks { get; set; }
        }
        [WebMethod]
        public static List<WalletRechargeDocumentList> WalletRechargeSearchList(String DocumentNo)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<WalletRechargeDocumentList> listStatues = new List<WalletRechargeDocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@DocumentNo", DocumentNo);

            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            proc.AddPara("@BranchID", "");
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = (from DataRow item in ds.Rows
                               select new WalletRechargeDocumentList()
                               {
                                   WalletRecharge_ID = item["WalletRecharge_ID"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["CancelStatus"].ToString(),
                                   ReasonForCancel = item["ReasonForCancel"].ToString(),
                                   Action = item["Action"].ToString()
                               }).ToList();

            }
            return listStatues;
        }
        public class WalletRechargeDocumentList
        {
            public String WalletRecharge_ID { get; set; }
            public String DocumentNumber { get; set; }
            public String DocumentDate { get; set; }
            public String Type { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNo { get; set; }
            public String Location { get; set; }
            public String DAS { get; set; }
            public String Enterby { get; set; }
            public String EnterOn { get; set; }
            public String Status { get; set; }
            public String ReasonForCancel { get; set; }
            public String Action { get; set; }

        }
        

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    // ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();
                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

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

        #region Location Populate

        protected void Location_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindLocationGrid")
            {
                DataTable LocationTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Session["userbranchHierarchy"] != null)
                {
                    //LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }

                if (LocationTable.Rows.Count > 0)
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = LocationTable;
                    lookup_Location.DataBind();
                }
                else
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = null;
                    lookup_Location.DataBind();
                }
            }
        }

        protected void lookup_Location_DataBinding(object sender, EventArgs e)
        {
            if (Session["LocationData"] != null)
            {
                lookup_Location.DataSource = (DataTable)Session["LocationData"];
            }
        }

        #endregion

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
        public static EditInventory STBReqUpdateInventory(String STBRequisitionID)
        {
            EditInventory ret = new EditInventory();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionDetails");
                proc.AddVarcharPara("@ACTION", 500, "ShowDetailsForHTMLView");
                proc.AddPara("@STBRequisition_id", STBRequisitionID);
                ds = proc.GetDataSet();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.GatepassNo = item["GatepassNo"].ToString();
                        ret.Remarks = item["InventoryRemarks"].ToString();
                        break;
                    }
                }
            }
            return ret;
        }

        public class EditInventory
        {
            public string GatepassNo { get; set; }
            public string Remarks { get; set; }
        }
    }
}