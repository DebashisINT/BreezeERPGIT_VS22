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

namespace DashBoard.DashBoard.STBMng
{
    public partial class StbMangement : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        CommonBL ComBL = new CommonBL();

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
            //Session["STBUserBranchID"] = userbranchHierachy;
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "STB Management");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DivReceipt.Visible = Convert.ToBoolean(dt.Rows[0]["StbReceipt"]);
                    DivWalletRecharge.Visible = Convert.ToBoolean(dt.Rows[0]["StbWalletRecharge"]);
                    DivSTBRequisition.Visible = Convert.ToBoolean(dt.Rows[0]["StbSTBRequisition"]);
                    DivSTBReqReturn.Visible = Convert.ToBoolean(dt.Rows[0]["StbSTBReqReturn"]);
                    DivApproval.Visible = Convert.ToBoolean(dt.Rows[0]["StbApproval"]);
                    DivInventory.Visible = Convert.ToBoolean(dt.Rows[0]["StbInventory"]);
                    DivSearch.Visible = Convert.ToBoolean(dt.Rows[0]["StbSearch"]);
                    DivReports.Visible = Convert.ToBoolean(dt.Rows[0]["StbReports"]);
                    DivReturnDispatch.Visible = Convert.ToBoolean(dt.Rows[0]["StbRetDispatch"]);

                    DivSchemeReceived.Visible = Convert.ToBoolean(dt.Rows[0]["StbSchemeReceived"]);
                    DivSchemeDirApproval.Visible = Convert.ToBoolean(dt.Rows[0]["StbSchemeDirApproval"]);
                    DivSchemeReqClose.Visible = Convert.ToBoolean(dt.Rows[0]["StbSchemeReqClose"]);
                    DivSchemeSearch.Visible = Convert.ToBoolean(dt.Rows[0]["StbSchemeSearch"]);
                    DivSchemeRegister.Visible = Convert.ToBoolean(dt.Rows[0]["StbSchemeRegister"]);
                }
                else
                {
                    DivReceipt.Visible = Convert.ToBoolean(0);
                    DivWalletRecharge.Visible = Convert.ToBoolean(0);
                    DivSTBRequisition.Visible = Convert.ToBoolean(0);
                    DivSTBReqReturn.Visible = Convert.ToBoolean(0);
                    DivApproval.Visible = Convert.ToBoolean(0);
                    DivInventory.Visible = Convert.ToBoolean(0);
                    DivSearch.Visible = Convert.ToBoolean(0);
                    DivReports.Visible = Convert.ToBoolean(0);
                    DivReturnDispatch.Visible = Convert.ToBoolean(0);

                    DivSchemeReceived.Visible = Convert.ToBoolean(0);
                    DivSchemeDirApproval.Visible = Convert.ToBoolean(0);
                    DivSchemeReqClose.Visible = Convert.ToBoolean(0);
                    DivSchemeSearch.Visible = Convert.ToBoolean(0);
                    DivSchemeRegister.Visible = Convert.ToBoolean(0);
                }

                ProcedureExecute procd = new ProcedureExecute("Prc_STBDashboardCount");
                procd.AddPara("@BranchID", userbranchHierachy);
                procd.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dtCount = procd.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DivSTBRequisitionFinPending.InnerHtml = Convert.ToString(dtCount.Rows[0]["STBRequisitionFinPending"]);
                    DivSTBRequisitionDirPending.InnerHtml = Convert.ToString(dtCount.Rows[0]["STBRequisitionDirPending"]);
                    DivSTBRequisitionOnHold.InnerHtml = Convert.ToString(dtCount.Rows[0]["STBRequisitionOnHold"]);
                    DivInventoryPending.InnerHtml = Convert.ToString(dtCount.Rows[0]["InventoryPending"]);
                    DivReturnReqPendingBranch.InnerHtml = Convert.ToString(dtCount.Rows[0]["ReturnReqPendingBranch"]);
                    DivReturnReqPendingHO.InnerHtml = Convert.ToString(dtCount.Rows[0]["ReturnReqPendingHO"]);
                    DivWalletRechargeOpenCash.InnerHtml = Convert.ToString(dtCount.Rows[0]["WalletRechargeOpenCash"]);
                    DivWalletRechargeOpenCheque.InnerHtml = Convert.ToString(dtCount.Rows[0]["WalletRechargeOpenCheque"]);
                    DivWalletRechargeCancelReq.InnerHtml = Convert.ToString(dtCount.Rows[0]["WalletRechargeCancelReq"]);
                    DivReceiptCancelReq.InnerHtml = Convert.ToString(dtCount.Rows[0]["ReceiptCancelReq"]);
                }
                else
                {
                    DivSTBRequisitionFinPending.InnerHtml = "0";
                    DivSTBRequisitionDirPending.InnerHtml = "0";
                    DivSTBRequisitionOnHold.InnerHtml = "0";
                    DivInventoryPending.InnerHtml = "0";
                    DivReturnReqPendingBranch.InnerHtml = "0";
                    DivReturnReqPendingHO.InnerHtml = "0";
                    DivWalletRechargeOpenCash.InnerHtml = "0";
                    DivWalletRechargeOpenCheque.InnerHtml = "0";
                    DivWalletRechargeCancelReq.InnerHtml = "0";
                    DivReceiptCancelReq.InnerHtml = "0";
                }
            }
        }

        [WebMethod]
        public static List<string> AnnouncementDetails(string reqStr)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "FetchDetailsforDashboard");
            proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
            DataTable DT = proc.GetTable();

            if (DT.Rows.Count > 0)
            {
                List<string> obj = new List<string>();
                foreach (DataRow dr in DT.Rows)
                {
                    obj.Add(Convert.ToString(dr["title"]) + "|" + Convert.ToString(dr["annoucement"]));
                }
                return obj;
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static bool STBSession(string comment)
        {
            HttpContext.Current.Session["DashboardShow"] = comment;
            return true;
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

        public class DashboardDetails
        {
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
