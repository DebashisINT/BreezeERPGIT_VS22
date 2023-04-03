/******************************************************************************************************************
Rev 1.0     Sanchita        V2.0.38     Message will be fired from first tab when logged out from the 2nd tab.
******************************************************************************************************************/
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
using System.Net;
using Newtonsoft.Json;

namespace ERP.OMS.Management
{
    public partial class management_ProjectMainPage : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        CommonBL ComBL = new CommonBL();

        MasterSettings masterbl = new MasterSettings();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                //For Settings wise user barnch 

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

                string ShowNewsFeed = ComBL.GetSystemSettingsResult("ShowNewsFeed");
                if (!String.IsNullOrEmpty(ShowNewsFeed))
                {
                    if (ShowNewsFeed == "Yes")
                    {
                        hdnShowNewsFeed.Value = "Yes";
                    }
                    else if (ShowNewsFeed.ToUpper().Trim() == "NO")
                    {
                        hdnShowNewsFeed.Value = "NO";
                    }
                }

                Session["EmployeeBranchMapping"] = userbranchHierachy;
                //For Settings wise user barnch 

                //For service management settings wise popup hide/show Start Tanmoy
                DataTable DT = new DataTable();
                DataTable dt1 = new DataTable();
                string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
                string masterSTBsettings = ComBL.GetSystemSettingsResult("IsSTBManagementRequired");

                DataSet dsUserDetail = new DataSet();
                dsUserDetail = oDBEngine.PopulateData("ISNULL(DefaultServiceDashboard,0) AS DefaultServiceDashboard,ISNULL(DefaultSTBDashboard,0) AS DefaultSTBDashboard ", "tbl_master_user", "user_id='" + Convert.ToString(Session["userid"]) + "'");


                if (masterSTBsettings == "Yes")
                {

                    if (dsUserDetail.Tables["TableName"].Rows.Count > 0)
                    {
                        hdnDefaultDashboardSTB.Value = Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]);
                        if (Convert.ToString(dsUserDetail.Tables["TableName"].Rows[0]["DefaultSTBDashboard"]) == "True")
                        {
                            if (Session["DashboardShow"] == null)
                            {
                                Session["DashboardShow"] = "STB";
                            }

                            dhnDashBoardSession.Value = "STB";
                            divPopHead.Style.Add("display", "!inline-block");
                            divSwitchtoSTBManagement.Style.Add("display", "!inline-block");

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

                            if (Session["DashboardShow"].ToString() == "STB")
                            {
                                dhnDashBoardSession.Value = "STB";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SwitchtoSTBManagement()", true);
                            }
                            else
                            {
                                dhnDashBoardSession.Value = "ERP";
                            }
                            // divSwitchtoServiceManagement.Style.Add("display", "none");
                            divSwitchtoSTBManagement.Style.Add("display", "!inline-block");

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
                        divSwitchtoSTBManagement.Style.Add("display", "none");
                    }
                }
                else if (mastersettings == "1")
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
                else
                {
                    divPopHead.Style.Add("display", "none");
                    if (Session["DashboardShow"] == null)
                    {
                        Session["DashboardShow"] = "ERP";
                    }
                    dhnDashBoardSession.Value = "ERP";
                    divSwitchtoSTBManagement.Style.Add("display", "none");
                }


                //if (mastersettings == "0")
                //{
                //    divPopHead.Style.Add("display", "none");
                //    if (Session["DashboardShow"] == null)
                //    {
                //        Session["DashboardShow"] = "ERP";
                //    }
                //    dhnDashBoardSession.Value = "ERP";
                //    divSwitchtoServiceManagement.Style.Add("display", "none");
                //}
                //else


                if (Session["DashboardShow"].ToString() == "ERP")
                {
                    dhnDashBoardSession.Value = "ERP";
                }
                else
                {
                    if (masterSTBsettings == "Yes")
                    {
                        dhnDashBoardSession.Value = "STB";
                    }
                    else if (mastersettings == "1")
                    {
                        dhnDashBoardSession.Value = "SRV";
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PopupOk()", true);
                }


                ProcedureExecute procd = new ProcedureExecute("prc_AnnouncementAddEdit");
                procd.AddPara("@Action", "FetchDetailsforDashboard");
                procd.AddPara("@userid", HttpContext.Current.Session["userid"]);
                DT = procd.GetTable();

                if (DT != null && DT.Rows.Count > 0)
                {
                    h1heading.InnerText = DT.Rows[0]["title"].ToString();
                    pParagraph.InnerHtml = DT.Rows[0]["anninHtml"].ToString();
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

                            if (Session["DashboardShow"] == "SRV" || Session["DashboardShow"] == "STB")
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
                proc.AddPara("@moduleName", "InventoryAnalytics");
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


                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "ManagementNotification");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    dvManagementNotification.Visible = true;
                    ManagementNot.Value = "1";

                }
                else
                {
                    ManagementNot.Value = "1";
                }

                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "ApprovalWaiting");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    dvApprovalWaiting.Visible = true;

                }
                //Rev End Customer Relationship Management and Project Management add Tanmoy

                //susanta 
                proc = new ProcedureExecute("prc_dashboardRights");
                proc.AddPara("@moduleName", "custProfile");
                proc.AddPara("@userid", Convert.ToString(Session["userid"]));
                dt2 = proc.GetTable();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    CUSTButton.Visible = true;

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
                      anninHtml = Convert.ToString(dr["anninHtml"]),
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

            if (DT != null && DT.Rows.Count > 0)
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


        [WebMethod]
        public static object GetVersionDetails(string links)
        {
            List<ReturnMessageInfo> result = new List<ReturnMessageInfo>();
            try
            {
                InputVersion obj = new InputVersion();
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = ConfigurationManager.AppSettings["VersionBaseURL"];

                    obj.client_uniqaueID = ConfigurationManager.AppSettings["Client_UniqueID"];
                    obj.current_version = Convert.ToString(HttpContext.Current.Session["LastVersion"]);


                    var url = links;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string data = JsonConvert.SerializeObject(obj);
                    var response = webClient.UploadString(url, data);
                    result = JsonConvert.DeserializeObject<List<ReturnMessageInfo>>(response);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [WebMethod]
        public static object SaveVersionDetails(string links, string requestedversion)
        {
            string output = "";
            try
            {
                InputVersionSave obj = new InputVersionSave();
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = ConfigurationManager.AppSettings["VersionBaseURL"];

                    obj.requested_version = requestedversion;
                    obj.client_uniqaueID = ConfigurationManager.AppSettings["Client_UniqueID"];


                    var url = links;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string data = JsonConvert.SerializeObject(obj);
                    var response = webClient.UploadString(url, data);
                    output = JsonConvert.DeserializeObject<String>(response);

                }
                //string requested_version ,string client_uniqaueID
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }

        [WebMethod]
        public static object getAllApprovalWaitingData(string action)
        {
            List<getApprovalWaitingCount> lEfficency = new List<getApprovalWaitingCount>();
            ProcedureExecute proc = new ProcedureExecute("PRC_ApprovalWaitingDetails");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            proc.AddPara("@Branchid", Convert.ToString(System.Web.HttpContext.Current.Session["EmployeeBranchMapping"]));
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new getApprovalWaitingCount()
                              {
                                  BranchRequisitonCount = Convert.ToString(dr["BranchRequisitonCount"]),
                                  PurchaseIndentCount = Convert.ToString(dr["PurchaseIndentCount"]),
                                  ProjectIndentCount = Convert.ToString(dr["ProjectIndentCount"]),
                                  PurchaseOrderCount = Convert.ToString(dr["PurchaseOrderCount"]),
                                  ProjectPurchaseOrderCount = Convert.ToString(dr["ProjectPurchaseOrderCount"]),
                                  SalesOrderCount = Convert.ToString(dr["SalesOrderCount"]),
                                  ProjectSalesOrderCount = Convert.ToString(dr["ProjectSalesOrderCount"])
                              }).ToList();
            }
            return lEfficency;
        }

        [WebMethod]
        public static object getNotificationTaskReminder()
        {
            List<AlertNotificationForTaskClass> lEfficency = new List<AlertNotificationForTaskClass>();
            ProcedureExecute proc = new ProcedureExecute("AlertNotificationForTask");
            proc.AddPara("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            if (CallData != null && CallData.Rows.Count > 0)
            {
                lEfficency = (from DataRow dr in CallData.Rows
                              select new AlertNotificationForTaskClass()
                              {
                                  UserID = Convert.ToString(dr["UserID"]),
                                  username = Convert.ToString(dr["username"]),
                                  TASK_SUBJECT = Convert.ToString(dr["TASK_SUBJECT"]),
                                  TASK_DESCRIPTION = Convert.ToString(dr["TASK_DESCRIPTION"]),
                                  REMARKS = Convert.ToString(dr["REMARKS"]),
                                  TASK_DUEDATE = Convert.ToString(dr["TASK_DUEDATE"]),
                                  TASK_PRIORITY = Convert.ToString(dr["TASK_PRIORITY"]),
                                  TASK_STARTDATE = Convert.ToString(dr["TASK_STARTDATE"]),
                              }).ToList();
            }
            return lEfficency;
        }

        // Rev 1.0
        [WebMethod]
        public static int checkSessionLogout()
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // End of Rev 1.0
    }

    
    public class AlertNotificationForTaskClass
    {
        public string UserID { get; set; }	
        public string username{ get; set; }	
        public string TASK_SUBJECT{ get; set; }	
        public string TASK_DESCRIPTION	{ get; set; }
        public string REMARKS	{ get; set; }
        public string TASK_DUEDATE	{ get; set; }
        public string TASK_PRIORITY	{ get; set; }
        public string TASK_STARTDATE { get; set; }
    }

    public class InputVersionSave
    {
        public string requested_version { get; set; }
        public string client_uniqaueID { get; set; }
    }
    public class GetfavClass
    {
        public string PAGE_URL { get; set; }
        public string PAGE_TITLE { get; set; }
    }

    public class ReturnMessageInfo
    {
        public string version_name { get; set; }
        public string whatsnew { get; set; }
        public string version_date { get; set; }
    }

    public class InputVersion
    {
        public string current_version { get; set; }

        public string client_uniqaueID { get; set; }

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
        public string anninHtml { get; set; }
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

    public class getApprovalWaitingCount
    {
        public String BranchRequisitonCount { get; set; }
        public String PurchaseIndentCount { get; set; }
        public String ProjectIndentCount { get; set; }
        public String PurchaseOrderCount { get; set; }
        public String ProjectPurchaseOrderCount { get; set; }
        public String SalesOrderCount { get; set; }
        public String ProjectSalesOrderCount { get; set; }
    }
}
