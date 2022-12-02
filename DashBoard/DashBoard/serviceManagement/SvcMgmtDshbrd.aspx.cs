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

namespace DashBoard.DashBoard.serviceManagement
{
    public partial class SvcMgmtDshbrd : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "Service Management");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DivReceiptChallan.Visible = Convert.ToBoolean(dt.Rows[0]["TokenChallan"]);
                    DivAssignJob.Visible = Convert.ToBoolean(dt.Rows[0]["AssignJob"]);
                    DivServiceDatas.Visible = Convert.ToBoolean(dt.Rows[0]["ServiceDataEntry"]);
                    DivDelivery.Visible = Convert.ToBoolean(dt.Rows[0]["Delivery"]);
                    DivSearch.Visible = Convert.ToBoolean(dt.Rows[0]["Search"]);
                    DivJobsheetEntry.Visible = Convert.ToBoolean(dt.Rows[0]["JobsheetEntry"]);
                    DivReport.Visible = Convert.ToBoolean(dt.Rows[0]["Reports"]);
                    warrantyDiv.Visible = Convert.ToBoolean(dt.Rows[0]["SrvWarrantyUpdate"]);
                    DivSTBReceiving.Visible = Convert.ToBoolean(dt.Rows[0]["SrvSTBReceiving"]);
                }
                else
                {
                    DivReceiptChallan.Visible = Convert.ToBoolean(0);
                    DivAssignJob.Visible = Convert.ToBoolean(0);
                    DivServiceDatas.Visible = Convert.ToBoolean(0);
                    DivDelivery.Visible = Convert.ToBoolean(0);
                    DivSearch.Visible = Convert.ToBoolean(0);
                    DivJobsheetEntry.Visible = Convert.ToBoolean(0);
                    DivReport.Visible = Convert.ToBoolean(0);
                    warrantyDiv.Visible = Convert.ToBoolean(0);
                    DivSTBReceiving.Visible = Convert.ToBoolean(0);
                }

               // string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                ProcedureExecute procd = new ProcedureExecute("Prc_SrvDashboardCountModelWise");
                procd.AddPara("@BranchID", userbranchHierachy);
                procd.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dtCount = procd.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DivTotalChallan.InnerHtml = Convert.ToString(dtCount.Rows[0]["Challan"]);
                    DivTotalToken.InnerHtml = Convert.ToString(dtCount.Rows[0]["Token"]);
                    DivTotalReady.InnerHtml = Convert.ToString(dtCount.Rows[0]["Ready"]);
                    DivTotalDelivered.InnerHtml = Convert.ToString(dtCount.Rows[0]["Delivered"]);
                    DivToptalOpen.InnerHtml = Convert.ToString(dtCount.Rows[0]["OpenCount"]);
                    DivTotalUndelivered.InnerHtml = Convert.ToString(dtCount.Rows[0]["Undelivered"]);
                }
                else
                {
                    DivTotalChallan.InnerHtml = "0";
                    DivTotalToken.InnerHtml = "0";
                    DivTotalReady.InnerHtml = "0";
                    DivTotalDelivered.InnerHtml = "0";
                    DivToptalOpen.InnerHtml = "0";
                    DivTotalUndelivered.InnerHtml = "0";
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
                    obj.Add(Convert.ToString(dr["title"]) + "|" + Convert.ToString(dr["annoucement"]+ "|" + Convert.ToString(dr["anninHtml"])));
                }
                return obj;
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static bool SrvSession(string comment)
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
    }
}