using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using BusinessLogicLayer;
using System.Web.Services;
using DataAccessLayer;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Linq;

namespace ServiceManagement.ServiceManagement.Management
{
    public partial class management_ProjectMainPage : System.Web.UI.Page
    {

        MasterSettings masterbl = new MasterSettings();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                //For service management settings wise popup hide/show Start Tanmoy
                string mastersettings = masterbl.GetSettings("isServiceManagementRequred");

                DataSet dsUserDetail = new DataSet();
                dsUserDetail = oDBEngine.PopulateData("ISNULL(DefaultServiceDashboard,0) AS DefaultServiceDashboard ", "tbl_master_user", "user_id='" + Convert.ToString(Session["userid"]) + "'");

                if (mastersettings == "0")
                {
                    divPopHead.Style.Add("display", "none");
                }
                else
                {
                    if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
                    {
                        hdnDefaultDashboardService.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]);
                        if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]) == "True")
                        {
                            divPopHead.Style.Add("display", "!inline-block");
                        }
                        else
                        {
                            divPopHead.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        divPopHead.Style.Add("display", "none");
                    }
                }

                ProcedureExecute procd = new ProcedureExecute("prc_AnnouncementAddEdit");
                procd.AddPara("@Action", "FetchDetailsforDashboard");
                procd.AddPara("@userid", HttpContext.Current.Session["userid"]);
                DataTable DT = procd.GetTable();

                if (DT!=null && DT.Rows.Count > 0)
                {
                    h1heading.InnerText = DT.Rows[0]["title"].ToString();
                    pParagraph.InnerText = DT.Rows[0]["annoucement"].ToString();
                }
                else
                {
                    h1heading.InnerText = "";
                    pParagraph.InnerText = "No notification for today.";
                }
                //For service management settings wise popup hide/show End Tanmoy

                ProcedureExecute proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Sale");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();
                //                DataTable dt = oDBEngine.GetDataTable(@"select null from tbl_master_dashboard_setting_details
                //                                where (TotSale=1 or TotAdvRcv =1 or TotOrder=1 or TopNSalesman=1 or TopNCustomer=1 or TotSaleDue=1) 
                //                                and user_id=" + Convert.ToString(Session["userid"]));
                if (dt.Rows.Count > 0)
                {
                    SalesDbButton.Visible = true;

                }

                //                DataTable dt2 = oDBEngine.GetDataTable(@"select null from tbl_master_dashboard_setting_details
                //                where (TotPurchase=1 or TotDue =1 or TotPayment=1 or TotReturn=1 or TopNItemByPurchase=1 or TopNVendor=1)
                //                and user_id=" + Convert.ToString(Session["userid"]));
                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Purchase");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                DataTable dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    PurchaseDbButton.Visible = true;

                }

                //                dt2 = oDBEngine.GetDataTable(@"select null from tbl_master_dashboard_setting_details
                //                where (totPhCall=1 or totSv=1 or TotEnt=1 or PendAct=1 or OrdCnt=1 or QuoteCount=1 or efficiencyrat=1 or ActHistory=1 or newVsreapeat=1)
                //                and user_id=" + Convert.ToString(Session["userid"]));
                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "CRM");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    CRMButton.Visible = true;

                }


                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Attendance");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    Attbtn.Visible = true;

                }


                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Followup");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    followupBtn.Visible = true;

                }



                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Accounts");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    AccountsBtn.Visible = true;

                }

                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "Tasklist");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2.Rows.Count > 0)
                {
                    tasklistbtn.Visible = true;

                }


                if (Session["hasShowWeekPass"] == null)
                {

                    proc = new ProcedureExecute("prc_dashboardRights");
                    proc.AddPara("@moduleName", "GetPassStrength");
                    proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                    dt2 = proc.GetTable();
                    if (Convert.ToInt32(dt2.Rows[0][0]) == 1)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showWeekFunctionAlert()", true);
                    }

                    Session["hasShowWeekPass"] = "Done";
                }


            }
        }




        [WebMethod]
        public static object GetMyAnnouncement()
        {
            List<announcement> anc = new List<announcement>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "MyAnnouncement");
            proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
            DataTable Dt = proc.GetTable();

            anc = (from DataRow dr in Dt.Rows
                   select new announcement()
                  {
                      title = Convert.ToString(dr["title"]),
                      msg = Convert.ToString(dr["annoucement"]),
                      AncId = Convert.ToInt32(dr["AncId"]),
                      allowCmnt = Convert.ToBoolean(dr["AllowComment"])
                  }).ToList();

            return anc;

        }

        [WebMethod]
        public static bool SaveComment(string comment, string AncId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "AddComment");
            proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
            proc.AddPara("@comment", comment);
            proc.AddPara("@id", AncId);
            proc.RunActionQuery();

            return true;
        }



        [WebMethod]
        public static object Gettop5Comment(string AncId)
        {
            commentfor5box commentbox = new commentfor5box();

            List<commentList> anc = new List<commentList>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "GetLast5Comment");
            proc.AddPara("@id", AncId);
            DataSet Dt = proc.GetDataSet();

            anc = (from DataRow dr in Dt.Tables[0].Rows
                   select new commentList()
                   {
                       user_name = Convert.ToString(dr["user_name"]),
                       Comment = Convert.ToString(dr["Comment"]),
                       CommentOn = Convert.ToString(dr["CommentOn"])
                   }).ToList();

            commentbox.cmt = anc;
            commentbox.totalCount = Convert.ToInt32(Dt.Tables[1].Rows[0][0]);

            return commentbox;

        }


        [WebMethod]
        public static object GetAllComment(string AncId)
        {

            List<commentList> anc = new List<commentList>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "GetAllComment");
            proc.AddPara("@id", AncId);
            DataSet Dt = proc.GetDataSet();

            anc = (from DataRow dr in Dt.Tables[0].Rows
                   select new commentList()
                   {
                       user_name = Convert.ToString(dr["user_name"]),
                       Comment = Convert.ToString(dr["Comment"]),
                       CommentOn = Convert.ToString(dr["CommentOn"])
                   }).ToList();


            return anc;

        }

        //For service management data fetch Start Tanmoy
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
        //For service management data fetch End Tanmoy

        public class announcement
        {
            public string title { get; set; }
            public string msg { get; set; }
            public int AncId { get; set; }
            public bool allowCmnt { get; set; }
        }

        public class commentList
        {
            public string user_name { get; set; }
            public string Comment { get; set; }
            public string CommentOn { get; set; }
        }

        public class commentfor5box
        {
            public int totalCount { get; set; }
            public List<commentList> cmt { get; set; }
        }
    }
}