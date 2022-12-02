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
using System.Xml;
using System.Text;
using UtilityLayer;

namespace DashBoard.DashBoard.ProjectMain
{
    public partial class ProjectMain : System.Web.UI.Page
    {
        MasterSettings masterbl = new MasterSettings();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                //For service management settings wise popup hide/show Start Tanmoy
                DataTable DT = new DataTable();
                DataTable dt1 = new DataTable();
                string mastersettings = masterbl.GetSettings("isServiceManagementRequred");

                DataSet dsUserDetail = new DataSet();
                dsUserDetail = oDBEngine.PopulateData("ISNULL(DefaultServiceDashboard,0) AS DefaultServiceDashboard ", "tbl_master_user", "user_id='" + Convert.ToString(Session["userid"]) + "'");

                if (mastersettings == "0")
                {
                    divPopHead.Style.Add("display", "none");
                    if (Session["DashboardShow"] == null)
                    {
                        Session["DashboardShow"] = "ERP";
                    }
                    dhnDashBoardSession.Value = "ERP";
                    divSwitchtoServiceManagement.Style.Add("display", "none");
                }
                else
                {
                    if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
                    {
                        hdnDefaultDashboardService.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]);
                        if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultServiceDashboard"]) == "True")
                        {
                            if (Session["DashboardShow"] == null)
                            {
                                Session["DashboardShow"] = "SRV";
                            }

                            dhnDashBoardSession.Value = "SRV";
                            divPopHead.Style.Add("display", "!inline-block");
                            divSwitchtoServiceManagement.Style.Add("display", "!inline-block");

                            ProcedureExecute procdS = new ProcedureExecute("prc_InsrtAnnouncementFlagUserWise");
                            procdS.AddPara("@ISSEEN", 0);
                            procdS.AddPara("@USER_ID", HttpContext.Current.Session["userid"]);
                            dt1 = procdS.GetTable();
                        }
                        else
                        {
                            divPopHead.Style.Add("display", "none");
                            if (Session["DashboardShow"] == null)
                            {
                                Session["DashboardShow"] = "ERP";
                            }

                            if (Session["DashboardShow"].ToString() == "SRV")
                            {
                                dhnDashBoardSession.Value = "SRV";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SwitchtoServiceManagement()", true);
                            }
                            else
                            {
                                dhnDashBoardSession.Value = "ERP";
                            }
                            // divSwitchtoServiceManagement.Style.Add("display", "none");
                            divSwitchtoServiceManagement.Style.Add("display", "!inline-block");

                        }
                    }
                    else
                    {
                        divPopHead.Style.Add("display", "none");
                        if (Session["DashboardShow"] == null)
                        {
                            Session["DashboardShow"] = "ERP";
                        }
                        dhnDashBoardSession.Value = "ERP";
                        divSwitchtoServiceManagement.Style.Add("display", "none");
                    }
                }

                if (Session["DashboardShow"].ToString() == "ERP")
                {
                    dhnDashBoardSession.Value = "ERP";
                }
                else
                {
                    dhnDashBoardSession.Value = "SRV";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PopupOk()", true);
                }


                ProcedureExecute procd = new ProcedureExecute("prc_AnnouncementAddEdit");
                procd.AddPara("@Action", "FetchDetailsforDashboard");
                procd.AddPara("@userid", HttpContext.Current.Session["userid"]);
                DT = procd.GetTable();

                if (DT != null && DT.Rows.Count > 0)
                {
                    h1heading.InnerText = DT.Rows[0]["title"].ToString();
                    //pParagraph.InnerHtml = DT.Rows[0]["anninHtml"].ToString();
                    pParagraph.InnerHtml = "Hello";
                }
                else
                {
                    h1heading.InnerText = "";
                    pParagraph.InnerText = "No notification for today.";
                }


                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    if (Session["ShowServicePopUp"] == null)
                    {
                        Session["ShowServicePopUp"] = "NO";
                        divPopHead.Style.Add("display", "!inline-block");
                    }
                    else
                    {
                        if (dt1.Rows[0]["MSG"].ToString() == "YES")
                        {
                            divPopHead.Style.Add("display", "!inline-block");
                        }
                        else
                        {
                            divPopHead.Style.Add("display", "none");

                            if (Session["DashboardShow"] == "SRV")
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PopupOk()", true);
                            }
                        }
                    }
                }
                else
                {
                    divPopHead.Style.Add("display", "none");
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

                //Rev Start Customer Relationship Management and Project Management add Tanmoy 
                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "CustRM");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    CustRMButton.Visible = true;

                }

                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "PMS");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    PMSButton.Visible = true;

                }
                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "KPISummary");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    dvKPISummary.Visible = true;

                }

                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "InventoryDashboard");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    dvInveDashboard.Visible = true;

                }


                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "FinancialButton");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    FinancialButton.Visible = true;

                }
                //Rev End Customer Relationship Management and Project Management add Tanmoy

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
            MasterSettings masterbl = new MasterSettings();
            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            List<announcement> anc = new List<announcement>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "MyAnnouncement");
            proc.AddPara("@isServiceManagement", mastersettings);
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
        public static string ParseRssFile(string links)
        {
            CommonBL objCommonBl = new CommonBL();
            string output = objCommonBl.GetSystemSettingsResult("ShowNewsFeed");

            if (output == "No")
            {
                return null;
            }
            else
            {
                XmlDocument rssXmlDoc = new XmlDocument();

                // Load the RSS file from the RSS URL
                rssXmlDoc.Load(links);

                // Parse the Items in the RSS file
                XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

                StringBuilder rssContent = new StringBuilder();

                // Iterate through the items in the RSS file
                foreach (XmlNode rssNode in rssNodes)
                {
                    XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                    string title = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("link");
                    string link = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("description");
                    string description = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssContent.Append("<div class='ti_news'><a href='" + link + "' target='_blank'>" + title + "</a></div>");
                }

                // Return the string that contain the RSS items
                return rssContent.ToString();
            }
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
        [WebMethod]
        public static bool SrvSession(string comment)
        {
            HttpContext.Current.Session["DashboardShow"] = comment;

            ProcedureExecute procdS = new ProcedureExecute("prc_InsrtAnnouncementFlagUserWise");
            procdS.AddPara("@ISSEEN", 1);
            procdS.AddPara("@USER_ID", HttpContext.Current.Session["userid"]);
            DataTable dt1 = procdS.GetTable();

            return true;
        }
        //For service management data fetch End Tanmoy
        [WebMethod]
        public static object GetAllNotificationData(string action)
        {
            List<allDataClass> lEfficency = new List<allDataClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new allDataClass()
                          {
                              TOTCUST = Convert.ToString(dr["TOTCUST"]),
                              TOTVEND = Convert.ToString(dr["TOTVEND"]),
                              TOTEMP = Convert.ToString(dr["TOTEMP"]),
                              CNTINF = Convert.ToString(dr["CNTINF"]),
                              CNTTRANS = Convert.ToString(dr["CNTTRANS"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object AddFav(string url, string title)
        {
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_FAVMENU", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "add");
            cmd.Parameters.AddWithValue("@Url", url);
            cmd.Parameters.AddWithValue("@Title", title);

            cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            List<GetfavClass> lstQuickMenuLink = APIHelperMethods.ToModelList<GetfavClass>(dsInst);

            return lstQuickMenuLink;
        }
        [WebMethod]
        public static object GetFav()
        {
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_FAVMENU", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "fetch");
            cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            List<GetfavClass> lstQuickMenuLink = APIHelperMethods.ToModelList<GetfavClass>(dsInst);

            return lstQuickMenuLink;
        }
        [WebMethod]
        public static object RemoveFav(string url, string title)
        {
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_FAVMENU", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "delete");
            cmd.Parameters.AddWithValue("@Url", url);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            List<GetfavClass> lstQuickMenuLink = APIHelperMethods.ToModelList<GetfavClass>(dsInst);

            return lstQuickMenuLink;
        }
    }
    public class GetfavClass
    {
        public string PAGE_URL { get; set; }
        public string PAGE_TITLE { get; set; }
    }
    public class allDataClass
    {
        public string TOTCUST { get; set; }
        public string TOTVEND { get; set; }
        public string TOTEMP { get; set; }
        public string CNTINF { get; set; }
        public string CNTTRANS { get; set; }

    }
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