
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

namespace Dashboard_React.ajax.STBMNG
{
    public partial class STB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static boxValues GetBoxsData()
        {
            string userbranchHierachy = null;
            CommonBL ComBL = new CommonBL();
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
            boxValues cls = new boxValues();

            ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
            proc.AddPara("@SETTINGS_NAME", "STB Management");
            proc.AddPara("@user_id", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                cls.DivReceipt = Convert.ToBoolean(dt.Rows[0]["StbReceipt"]);
                cls.DivWalletRecharge = Convert.ToBoolean(dt.Rows[0]["StbWalletRecharge"]);
                cls.DivSTBRequisition = Convert.ToBoolean(dt.Rows[0]["StbSTBRequisition"]);
                cls.DivSTBReqReturn = Convert.ToBoolean(dt.Rows[0]["StbSTBReqReturn"]);
                cls.DivApproval = Convert.ToBoolean(dt.Rows[0]["StbApproval"]);
                cls.DivInventory = Convert.ToBoolean(dt.Rows[0]["StbInventory"]);
                cls.DivSearch = Convert.ToBoolean(dt.Rows[0]["StbSearch"]);
                cls.DivReports = Convert.ToBoolean(dt.Rows[0]["StbReports"]);
                cls.DivReturnDispatch = Convert.ToBoolean(dt.Rows[0]["StbRetDispatch"]);

                cls.DivSchemeReceived = Convert.ToBoolean(dt.Rows[0]["StbSchemeReceived"]);
                cls.DivSchemeDirApproval = Convert.ToBoolean(dt.Rows[0]["StbSchemeDirApproval"]);
                cls.DivSchemeReqClose = Convert.ToBoolean(dt.Rows[0]["StbSchemeReqClose"]);
                cls.DivSchemeSearch = Convert.ToBoolean(dt.Rows[0]["StbSchemeSearch"]);
                cls.DivSchemeRegister = Convert.ToBoolean(dt.Rows[0]["StbSchemeRegister"]);
            }
            else
            {
                cls.DivReceipt = Convert.ToBoolean(0);
                cls.DivWalletRecharge = Convert.ToBoolean(0);
                cls.DivSTBRequisition = Convert.ToBoolean(0);
                cls.DivSTBReqReturn = Convert.ToBoolean(0);
                cls.DivApproval = Convert.ToBoolean(0);
                cls.DivInventory = Convert.ToBoolean(0);
                cls.DivSearch = Convert.ToBoolean(0);
                cls.DivReports = Convert.ToBoolean(0);
                cls.DivReturnDispatch = Convert.ToBoolean(0);

                cls.DivSchemeReceived = Convert.ToBoolean(0);
                cls.DivSchemeDirApproval = Convert.ToBoolean(0);
                cls.DivSchemeReqClose = Convert.ToBoolean(0);
                cls.DivSchemeSearch = Convert.ToBoolean(0);
                cls.DivSchemeRegister = Convert.ToBoolean(0);
            }
            

            ProcedureExecute procd = new ProcedureExecute("Prc_STBDashboardCount");
            procd.AddPara("@BranchID", userbranchHierachy);
            procd.AddPara("@user_id", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dtCount = procd.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                cls.DivSTBRequisitionFinPending = Convert.ToString(dtCount.Rows[0]["STBRequisitionFinPending"]);
                cls.DivSTBRequisitionDirPending = Convert.ToString(dtCount.Rows[0]["STBRequisitionDirPending"]);
                cls.DivSTBRequisitionOnHold = Convert.ToString(dtCount.Rows[0]["STBRequisitionOnHold"]);
                cls.DivInventoryPending = Convert.ToString(dtCount.Rows[0]["InventoryPending"]);
                cls.DivReturnReqPendingBranch = Convert.ToString(dtCount.Rows[0]["ReturnReqPendingBranch"]);
                cls.DivReturnReqPendingHO = Convert.ToString(dtCount.Rows[0]["ReturnReqPendingHO"]);
                cls.DivWalletRechargeOpenCash = Convert.ToString(dtCount.Rows[0]["WalletRechargeOpenCash"]);
                cls.DivWalletRechargeOpenCheque = Convert.ToString(dtCount.Rows[0]["WalletRechargeOpenCheque"]);
                cls.DivWalletRechargeCancelReq = Convert.ToString(dtCount.Rows[0]["WalletRechargeCancelReq"]);
                cls.DivReceiptCancelReq = Convert.ToString(dtCount.Rows[0]["ReceiptCancelReq"]);
            }
            else
            {
                cls.DivSTBRequisitionFinPending = "0";
                cls.DivSTBRequisitionDirPending = "0";
                cls.DivSTBRequisitionOnHold = "0";
                cls.DivInventoryPending = "0";
                cls.DivReturnReqPendingBranch = "0";
                cls.DivReturnReqPendingHO = "0";
                cls.DivWalletRechargeOpenCash = "0";
                cls.DivWalletRechargeOpenCheque = "0";
                cls.DivWalletRechargeCancelReq = "0";
                cls.DivReceiptCancelReq = "0";
            }
            return cls;
        }

        public static String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, System.Web.HttpContext.Current.Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }
        [WebMethod]
        public static List<DashboardDetails> ShowDashboardDetails(string report)
        {
            List<DashboardDetails> listStatues = new List<DashboardDetails>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_STBReqDashboardDetails");
            proc.AddVarcharPara("@ACTION", 500, report);
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new DashboardDetails
                    {
                        key = item["DocumentNumber"].ToString(),
                        ReqNo = item["DocumentNumber"].ToString(),
                        ReqDate = item["DocumentDate"].ToString(),
                        Location = item["branch_description"].ToString(),
                        EntityCode = item["EntityCode"].ToString(),
                        Qty = item["Details_Quantity"].ToString(),
                        HoldBy = item["HoldBy"].ToString(),
                        Director = item["Director"].ToString(),
                        Model = item["ModelDesc"].ToString()
                    });
                }
            }
            return listStatues;
        }


        // Classess
        public class boxValues
        {
            public bool DivReceipt { get; set; }
            public bool DivWalletRecharge { get; set; }
            public bool DivSTBRequisition { get; set; }
            public bool DivSTBReqReturn { get; set; }
            public bool DivApproval { get; set; }
            public bool DivInventory { get; set; }
            public bool DivSearch { get; set; }
            public bool DivReports { get; set; }
            public bool DivReturnDispatch { get; set; }
            public bool DivSchemeReceived { get; set; }
            public bool DivSchemeDirApproval { get; set; }
            public bool DivSchemeReqClose { get; set; }
            public bool DivSchemeSearch { get; set; }
            public bool DivSchemeRegister { get; set; }

            public string DivSTBRequisitionFinPending { get; set; }
            public string DivSTBRequisitionDirPending { get; set; }
            public string DivSTBRequisitionOnHold { get; set; }
            public string DivInventoryPending { get; set; }
            public string DivReturnReqPendingBranch { get; set; }
            public string DivReturnReqPendingHO { get; set; }

            public string DivWalletRechargeOpenCash { get; set; }
            public string DivWalletRechargeOpenCheque { get; set; }
            public string DivWalletRechargeCancelReq { get; set; }
            public string DivReceiptCancelReq { get; set; }

        }
        public class DashboardDetails
        { 
            public String key { get; set; }
            public String ReqNo { get; set; }
            public String ReqDate { get; set; }
            public String Location { get; set; }
            public String EntityCode { get; set; }
            public String Qty { get; set; }
            public String HoldBy { get; set; }
            public String Director { get; set; }
            public String Model { get; set; }

        }

    }
}