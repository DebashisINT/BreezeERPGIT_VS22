using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.STBManagement.STBSchemeSearch
{
    public partial class STBSchemeSearch : System.Web.UI.Page
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
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeSearchModule");
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
                                   STBRequisition_id = item["STBRequisition_id"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["ReqStatus"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   RecvModel = item["RecvModel"].ToString(),
                                   SerialNumber = item["DeviceNumber"].ToString(),
                                   Rate = item["Rate"].ToString(),
                                   Remarks = item["Remarks"].ToString(),
                                   Remote = item["Remote"].ToString(),
                                   CordAdaptor = item["CardAdaptor"].ToString(),
                                   ReqFor = item["RequisitionFor"].ToString(),
                                   ReqType = item["ReqType"].ToString(),
                                   ReqModel = item["ReqModel"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Amount = item["Amount"].ToString(),
                                   Approve_By = item["Approve_By"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Action = item["Action"].ToString()
                               }).ToList();

            }
            return listStatues;
        }

        [WebMethod]
        public static List<DocumentList> DateWiseSearchList(String hfFromDate, String hfToDate)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<DocumentList> listStatues = new List<DocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeSearchModule");
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
                                   STBRequisition_id = item["STBRequisition_id"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["ReqStatus"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   RecvModel = item["RecvModel"].ToString(),
                                   SerialNumber = item["DeviceNumber"].ToString(),
                                   Rate = item["Rate"].ToString(),
                                   Remarks = item["Remarks"].ToString(),
                                   Remote = item["Remote"].ToString(),
                                   CordAdaptor = item["CardAdaptor"].ToString(),
                                   ReqFor = item["RequisitionFor"].ToString(),
                                   ReqType = item["ReqType"].ToString(),
                                   ReqModel = item["ReqModel"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Amount = item["Amount"].ToString(),
                                   Approve_By = item["Approve_By"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Action = item["Action"].ToString()
                               }).ToList();

            }
            return listStatues;
        }

        [WebMethod]
        public static List<DocumentList> SchemeSearchList(String DocumentNo, String EntityCode, String Branch, String Serial)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Search/search.aspx");
            List<DocumentList> listStatues = new List<DocumentList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeSearchModule");
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
            proc.AddPara("@Serial", Serial);
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = (from DataRow item in ds.Rows
                               select new DocumentList()
                               {
                                   STBRequisition_id = item["STBRequisition_id"].ToString(),
                                   DocumentNumber = item["DocumentNumber"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   ContactNo = item["ContactNo"].ToString(),
                                   DAS = item["DAS"].ToString(),
                                   Enterby = item["Enterby"].ToString(),
                                   EnterOn = item["EnterOn"].ToString(),
                                   Status = item["ReqStatus"].ToString(),
                                   ModuleType = item["ModuleType"].ToString(),
                                   RecvModel = item["RecvModel"].ToString(),
                                   SerialNumber = item["DeviceNumber"].ToString(),
                                   Rate = item["Rate"].ToString(),
                                   Remarks = item["Remarks"].ToString(),
                                   Remote = item["Remote"].ToString(),
                                   CordAdaptor = item["CardAdaptor"].ToString(),
                                   ReqFor = item["RequisitionFor"].ToString(),
                                   ReqType = item["ReqType"].ToString(),
                                   ReqModel = item["ReqModel"].ToString(),
                                   UnitPrice = item["UnitPrice"].ToString(),
                                   Quantity = item["Quantity"].ToString(),
                                   Amount = item["Amount"].ToString(),
                                   Approve_By = item["Approve_By"].ToString(),
                                   Approve_On = item["Approve_On"].ToString(),
                                   Action = item["Action"].ToString()
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
            public String STBRequisition_id { get; set; }
            public String DocumentNumber { get; set; }
            public String DocumentDate { get; set; }
            public String Location { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNo { get; set; }
            public String DAS { get; set; }
            public String Enterby { get; set; }
            public String EnterOn { get; set; }
            public String Status { get; set; }
            public String ModuleType { get; set; }
            //Recv
            public String RecvModel { get; set; }
            public String SerialNumber { get; set; }
            public String Rate { get; set; }
            public String Remarks { get; set; }
            public String Remote { get; set; }
            public String CordAdaptor { get; set; }
            //Req
            public String ReqFor { get; set; }
            public String ReqType { get; set; }
            public String ReqModel { get; set; }
            public String UnitPrice { get; set; }
            public String Quantity { get; set; }
            public String Amount { get; set; }

            public String Approve_By { get; set; }//Only for Rcev
            public String Approve_On { get; set; }//Only for Rcev
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
        public static STB_STBRequisitionHeader STBRequisitionDetails(String STBRequisitionID, String Document)
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

        [WebMethod]
        public static string RequisitionNoUpdate(string Received_id, String ReqNo1, String ReqNo2, String ReqNo3, String ReqNo4, String ReqNo5, String Remarks)
        {
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedInsertUpdate");
                    proc.AddPara("@Action", "RequisitionNoInsert");
                    proc.AddPara("@SchemeReceived_ID", Received_id);
                    proc.AddPara("@RequisitionNo1", ReqNo1);
                    proc.AddPara("@RequisitionNo2", ReqNo2);
                    proc.AddPara("@RequisitionNo3", ReqNo3);
                    proc.AddPara("@RequisitionNo4", ReqNo4);
                    proc.AddPara("@RequisitionNo5", ReqNo5);
                    proc.AddPara("@Remarks", Remarks);
                    proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
                    int i = proc.RunActionQuery();
                    if (i > 0)
                    {
                        output = "true";
                    }
                    else
                    {
                        output = "false";
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
        public static STB_CloseRequisition STBRequisitionNoDetails(String STBSchemeReceivedID)
        {
            STB_CloseRequisition ret = new STB_CloseRequisition();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
                proc.AddVarcharPara("@ACTION", 500, "GetReqNoDetails");
                proc.AddPara("@SchemeReceived_ID", STBSchemeReceivedID);
                ds = proc.GetTable();

                if (ds != null && ds.Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Rows)
                    {
                        ret.ReqNo1 = item["RequisitionNo1"].ToString();
                        ret.ReqNo2 = item["RequisitionNo2"].ToString();
                        ret.ReqNo3 = item["RequisitionNo3"].ToString();
                        ret.ReqNo4 = item["RequisitionNo4"].ToString();
                        ret.ReqNo5 = item["RequisitionNo5"].ToString();
                        ret.Remarks = item["Remarks"].ToString();
                        break;
                    }
                }
            }
            return ret;
        }

        public class STB_CloseRequisition
        {
            public string ReqNo1 { get; set; }
            public string ReqNo2 { get; set; }
            public string ReqNo3 { get; set; }
            public string ReqNo4 { get; set; }
            public string ReqNo5 { get; set; }
            public string Remarks { get; set; }
        }
    }
}