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

namespace Dashboard_React.ajax.permissions
{
    public partial class permissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, HttpContext.Current.Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        [WebMethod]
        public static object getDashboardRedirect()
        {
            string userbranchHierachy = null;
            CommonBL ComBL = new CommonBL();
            MasterSettings masterbl = new MasterSettings();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            homePermisissonCls mdl = new homePermisissonCls();

            string DefaultBranchInLogin = ComBL.GetSystemSettingsResult("DefaultBranchInLogin");
            if (!String.IsNullOrEmpty(DefaultBranchInLogin))
            {
                if (DefaultBranchInLogin.ToUpper().Trim() == "YES")
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

            System.Web.HttpContext.Current.Session["EmployeeBranchMapping"] = userbranchHierachy;
            //For Settings wise user barnch 

            //For service management settings wise popup hide/show Start Tanmoy
            DataTable DT = new DataTable();
            DataTable dt1 = new DataTable();
            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            //rev Pratik
            string masterSTBsettings = ComBL.GetSystemSettingsResult("IsSTBManagementRequired");

            DataSet dsUserDetail = new DataSet();
            dsUserDetail = oDBEngine.PopulateData("ISNULL(DefaultServiceDashboard,0) AS DefaultServiceDashboard,ISNULL(DefaultSTBDashboard,0) AS DefaultSTBDashboard ", "tbl_master_user", "user_id='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "'");

            if (masterSTBsettings == "Yes")
            {

                if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
                {
                    mdl.dhnDashBoardSession = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]);
                    mdl.hdnDefaultDashboardSTB = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]);
                    mdl.hdnDefaultDashboardService = "False";
                    if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]) == "True")
                    {
                        if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                        {
                            System.Web.HttpContext.Current.Session["DashboardShow"] = "STB";
                        }

                        mdl.dhnDashBoardSession = "STB";
                        //divPopHead.Style.Add("display", "!inline-block");
                        //divSwitchtoSTBManagement.Style.Add("display", "!inline-block");
                        mdl.divPopHead = "block";
                        ProcedureExecute procdS = new ProcedureExecute("prc_InsrtAnnouncementFlagUserWise");
                        procdS.AddPara("@ISSEEN", 0);
                        procdS.AddPara("@USER_ID", HttpContext.Current.Session["userid"]);
                        dt1 = procdS.GetTable();
                    }
                    else
                    {
                        mdl.divPopHead = "none";
                        if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                        {
                            System.Web.HttpContext.Current.Session["DashboardShow"] = "ERP";
                        }

                        if (System.Web.HttpContext.Current.Session["DashboardShow"].ToString() == "STB")
                        {
                            mdl.dhnDashBoardSession = "STB";
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SwitchtoSTBManagement()", true);
                        }
                        else
                        {
                            mdl.dhnDashBoardSession = "ERP";
                        }

                        // divSwitchtoSTBManagement.Style.Add("display", "!inline-block");
                        mdl.divPopHead = "block";
                    }
                }
                else
                {
                    //divPopHead.Style.Add("display", "none");

                    if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                    {
                        System.Web.HttpContext.Current.Session["DashboardShow"] = "ERP";
                    }
                    mdl.dhnDashBoardSession = "ERP";
                    mdl.divPopHead = "none";

                }
            }
            //End of rev Pratik
            else if (mastersettings == "0")
            {
                mdl.divPopHead = "none";//.Style.Add("display", "none");
                if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                {
                    System.Web.HttpContext.Current.Session["DashboardShow"] = "ERP";
                }
                mdl.dhnDashBoardSession = "ERP";
                // divSwitchtoServiceManagement.Style.Add("display", "none");
            }
            else
            {
                if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
                {
                    mdl.hdnDefaultDashboardService = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]);
                    mdl.hdnDefaultDashboardSTB = "False";
                    if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]) == "True")
                    {
                        if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                        {
                            System.Web.HttpContext.Current.Session["DashboardShow"] = "SRV";
                        }

                        mdl.dhnDashBoardSession = "SRV";
                        //divPopHead.Style.Add("display", "!inline-block");
                        //divSwitchtoServiceManagement.Style.Add("display", "!inline-block");

                        ProcedureExecute procdS = new ProcedureExecute("prc_InsrtAnnouncementFlagUserWise");
                        procdS.AddPara("@ISSEEN", 0);
                        procdS.AddPara("@USER_ID", HttpContext.Current.Session["userid"]);
                        dt1 = procdS.GetTable();
                    }
                    else
                    {
                        // divPopHead.Style.Add("display", "none");
                        if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                        {
                            System.Web.HttpContext.Current.Session["DashboardShow"] = "ERP";
                        }

                        if (System.Web.HttpContext.Current.Session["DashboardShow"].ToString() == "SRV")
                        {
                            mdl.dhnDashBoardSession = "SRV";
                            // Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SwitchtoServiceManagement()", true);
                        }
                        else
                        {
                            mdl.dhnDashBoardSession = "ERP";
                        }
                        // divSwitchtoServiceManagement.Style.Add("display", "none");
                        // mdl.divSwitchtoServiceManagement = "block";//.Style.Add("display", "!inline-block");

                    }
                }
                else
                {
                    mdl.divPopHead = "none";//.Style.Add("display", "none");
                    if (System.Web.HttpContext.Current.Session["DashboardShow"] == null)
                    {
                        System.Web.HttpContext.Current.Session["DashboardShow"] = "ERP";
                    }
                    mdl.dhnDashBoardSession = "ERP";
                    //divSwitchtoServiceManagement.Style.Add("display", "none");
                }
            }




            if (System.Web.HttpContext.Current.Session["DashboardShow"].ToString() == "ERP")
            {
                mdl.dhnDashBoardSession = "ERP";
            }
            //rev Pratik
            else if (System.Web.HttpContext.Current.Session["DashboardShow"].ToString() == "STB")
            {
                mdl.dhnDashBoardSession = "STB";
            }
            //End of rev Pratik
            else
            {
                mdl.dhnDashBoardSession = "SRV";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PopupOk()", true);
            }


            //ProcedureExecute procd = new ProcedureExecute("prc_AnnouncementAddEdit");
            //procd.AddPara("@Action", "FetchDetailsforDashboard");
            //procd.AddPara("@userid", HttpContext.Current.Session["userid"]);
            //DT = procd.GetTable();

            //if (DT != null && DT.Rows.Count > 0)
            //{
            //    h1heading.InnerText = DT.Rows[0]["title"].ToString();
            //    pParagraph.InnerText = DT.Rows[0]["annoucement"].ToString();
            //}
            //else
            //{
            //    h1heading.InnerText = "";
            //    pParagraph.InnerText = "No notification for today.";
            //}


            if (dt1 != null && dt1.Rows.Count > 0)
            {
                if (System.Web.HttpContext.Current.Session["ShowServicePopUp"] == null)
                {
                    System.Web.HttpContext.Current.Session["ShowServicePopUp"] = "NO";
                    mdl.divPopHead = "block";//.Style.Add("display", "!inline-block");
                }
                else
                {
                    if (dt1.Rows[0]["MSG"].ToString() == "YES")
                    {
                        mdl.divPopHead = "block";//.Style.Add("display", "!inline-block");
                    }
                    else
                    {
                        mdl.divPopHead = "none";//.Style.Add("display", "none");

                        //if (System.Web.HttpContext.Current.Session["DashboardShow"] == "SRV")
                        //{
                        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PopupOk()", true);
                        //}
                    }
                }
            }
            else
            {
                mdl.divPopHead = "none";//.Style.Add("display", "none");
            }

            //For service management settings wise popup hide/show End Tanmoy


            ProcedureExecute proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Sale");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                mdl.SalesDbButton = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Purchase");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.PurchaseDbButton = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "CRM");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.CRMButton = true;

            }


            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Attendance");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.Attbtn = true;

            }


            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Followup");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.followupBtn = true;

            }



            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Accounts");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.AccountsBtn = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "Tasklist");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.tasklistbtn = true;

            }

            //Rev Start Customer Relationship Management and Project Management add Tanmoy 
            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "CustRM");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.CustRMButton = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "PMS");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.PMSButton = true;

            }
            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "KPISummary");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.dvKPISummary = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "InventoryAnalytics");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.dvInveDashboard = true;

            }


            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "FinancialButton");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.FinancialButton = true;

            }


            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "ManagementNotification");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.dvManagementNotification = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "ApprovalWaiting");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                mdl.dvApprovalWaiting = true;

            }

            proc = new ProcedureExecute("prc_dashboardRights");
            proc.AddPara("@moduleName", "custProfile");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            dt2 = proc.GetTable();
            if (dt2 != null &&  dt2.Rows.Count > 0)
            {
                mdl.CUSTButton = true;

            }
            //Rev End Customer Relationship Management and Project Management add Tanmoy

            if (System.Web.HttpContext.Current.Session["hasShowWeekPass"] == null)
            {

                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "GetPassStrength");
                proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
                dt2 = proc.GetTable();
                if (Convert.ToInt32(dt2.Rows[0][0]) == 1)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showWeekFunctionAlert()", true);
                }

                System.Web.HttpContext.Current.Session["hasShowWeekPass"] = "Done";
            }

            return mdl;
        }
        public class homePermisissonCls
        {
            public bool SalesDbButton { get; set; }
            public bool PurchaseDbButton { get; set; }
            public bool CRMButton { get; set; }
            public bool Attbtn { get; set; }
            public bool followupBtn { get; set; }
            public bool AccountsBtn { get; set; }
            public bool tasklistbtn { get; set; }
            public bool CustRMButton { get; set; }
            public bool PMSButton { get; set; }
            public bool dvKPISummary { get; set; }
            public bool dvInveDashboard { get; set; }
            public bool FinancialButton { get; set; }
            public bool dvManagementNotification { get; set; }
            public bool dvApprovalWaiting { get; set; }
            public String dhnDashBoardSession { get; set; }
            public String divPopHead { get; set; }
            public String hdnDefaultDashboardService { get; set; }
            public String hdnDefaultDashboardSTB { get; set; }
            public bool CUSTButton { get; set; }
        }




        [WebMethod]
        public static object getPermissions()
        {
            PermissionsCls mdl = new PermissionsCls();
            ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
            proc.AddPara("@SETTINGS_NAME", "Approval Waiting");
            proc.AddPara("@user_id", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                mdl.divBranchRequisiton = Convert.ToBoolean(dt.Rows[0]["AWBranchRequisition"]);
                mdl.divPurchaseIndent = Convert.ToBoolean(dt.Rows[0]["AWPurchaseIndent"]);
                mdl.divProjectIndent = Convert.ToBoolean(dt.Rows[0]["AWProjectIndent"]);
                mdl.divPurchaseOrder = Convert.ToBoolean(dt.Rows[0]["AWPurchaseOrder"]);
                mdl.divProjectPurchaseOrder = Convert.ToBoolean(dt.Rows[0]["AWProjectPurchaseOrder"]);
                mdl.divSalesOrder = Convert.ToBoolean(dt.Rows[0]["AWSalesOrder"]);
                mdl.divProjectSalesOrder = Convert.ToBoolean(dt.Rows[0]["AWProjectSalesOrder"]);
            }
            else
            {
                mdl.divBranchRequisiton = Convert.ToBoolean(0);
                mdl.divPurchaseIndent = Convert.ToBoolean(0);
                mdl.divProjectIndent = Convert.ToBoolean(0);
                mdl.divPurchaseOrder = Convert.ToBoolean(0);
                mdl.divProjectPurchaseOrder = Convert.ToBoolean(0);
                mdl.divSalesOrder = Convert.ToBoolean(0);
                mdl.divProjectSalesOrder = Convert.ToBoolean(0);

            }
            return mdl;
        }
        public class PermissionsCls
        {
            public bool divBranchRequisiton { get; set; }
            public bool divPurchaseIndent { get; set; }
            public bool divProjectIndent { get; set; }
            public bool divPurchaseOrder { get; set; }
            public bool divProjectPurchaseOrder { get; set; }
            public bool divSalesOrder { get; set; }
            public bool divProjectSalesOrder { get; set; }
        }
    }
}