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

namespace Dashboard_React.ajax.ServiceManagement
{
    public partial class ServiceMng : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
        public static bool SrvSession(string comment)
        {
            HttpContext.Current.Session["DashboardShow"] = comment;
            return true;
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
            proc.AddPara("@SETTINGS_NAME", "Service Management");
            proc.AddPara("@user_id", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                cls.DivReceiptChallan = Convert.ToBoolean(dt.Rows[0]["TokenChallan"]);
                cls.DivAssignJob = Convert.ToBoolean(dt.Rows[0]["AssignJob"]);
                cls.DivServiceDatas = Convert.ToBoolean(dt.Rows[0]["ServiceDataEntry"]);
                cls.DivDelivery = Convert.ToBoolean(dt.Rows[0]["Delivery"]);
                cls.DivSearch = Convert.ToBoolean(dt.Rows[0]["Search"]);
                cls.DivJobsheetEntry = Convert.ToBoolean(dt.Rows[0]["JobsheetEntry"]);
                cls.DivReport = Convert.ToBoolean(dt.Rows[0]["Reports"]);
                cls.warrantyDiv = Convert.ToBoolean(dt.Rows[0]["SrvWarrantyUpdate"]);
            }
            else
            {
                cls.DivReceiptChallan = Convert.ToBoolean(0);
                cls.DivAssignJob = Convert.ToBoolean(0);
                cls.DivServiceDatas = Convert.ToBoolean(0);
                cls.DivDelivery = Convert.ToBoolean(0);
                cls.DivSearch = Convert.ToBoolean(0);
                cls.DivJobsheetEntry = Convert.ToBoolean(0);
                cls.DivReport = Convert.ToBoolean(0);
                cls.warrantyDiv = Convert.ToBoolean(0);
            }

            ProcedureExecute procd = new ProcedureExecute("Prc_SrvDashboardCountModelWise");
            procd.AddPara("@BranchID", userbranchHierachy);
            procd.AddPara("@user_id", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dtCount = procd.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                cls.DivTotalChallan = Convert.ToString(dtCount.Rows[0]["Challan"]);
                cls.DivTotalToken = Convert.ToString(dtCount.Rows[0]["Token"]);
                cls.DivTotalReady = Convert.ToString(dtCount.Rows[0]["Ready"]);
                cls.DivTotalDelivered = Convert.ToString(dtCount.Rows[0]["Delivered"]);
                cls.DivToptalOpen = Convert.ToString(dtCount.Rows[0]["OpenCount"]);
                cls.DivTotalUndelivered = Convert.ToString(dtCount.Rows[0]["Undelivered"]);
            }
            else
            {
                cls.DivTotalChallan = "0";
                cls.DivTotalToken = "0";
                cls.DivTotalReady = "0";
                cls.DivTotalDelivered = "0";
                cls.DivToptalOpen = "0";
                cls.DivTotalUndelivered = "0";
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

        public class boxValues
        {
            public bool  DivReceiptChallan { get; set; }
            public bool DivAssignJob { get; set; }
            public bool DivServiceDatas { get; set; }
            public bool DivDelivery { get; set; }
            public bool DivSearch { get; set; }
            public bool DivJobsheetEntry { get; set; }
            public bool DivReport { get; set; }
            public bool warrantyDiv { get; set; }
            public string   DivTotalChallan { get; set; }
            public string   DivTotalToken   { get; set; }
            public string   DivTotalReady   { get; set; }
            public string   DivTotalDelivered { get; set; }
            public string   DivToptalOpen     { get; set; }
            public string   DivTotalUndelivered { get; set; }

        }
    }
}