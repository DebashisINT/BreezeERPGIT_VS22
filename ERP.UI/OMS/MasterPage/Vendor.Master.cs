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
using ClsDropDownlistNameSpace;
using System.Net.NetworkInformation;
using DevExpress.Web;

namespace ERP.OMS.MasterPage
{
    public partial class Vendor : System.Web.UI.MasterPage
    {
        public string styleMenuCloseOpen = string.Empty;
        Management_BL mng_bl = new Management_BL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine("");
        BusinessLogicLayer.GenericMethod oGenericMethod = null;
        clsDropDownList OclsDropDownList = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Purpose: Chrcking that the user coming from redirect url or direct url. If the user wants access any page via its url then the user will be redirect to alert page
                //Name: Debjyoti Dhar. 21-11-2016

                //if (!HttpContext.Current.Request.Url.AbsoluteUri.ToString().Contains("http://localhost"))
                //{
                //    if (!HttpContext.Current.Request.Url.AbsoluteUri.ToString().Contains("ProjectMainPage"))
                //    {
                //        if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] == null)
                //        {
                //            HttpContext.Current.Response.Redirect("/oms/AlertMessage.aspx");
                //        }
                //    }
                //}


                if (Session["errSession"] != null)
                {
                    Session.Remove("errSession");
                    HttpContext.Current.Response.Write("<script> alert('You do not have permission to perform this operation');</script>");
                    return;
                }
                HttpCookie cookie = Request.Cookies["sKey"];
                if (cookie != null)
                {
                    LogedUser.Text = Convert.ToString(cookie.Value);
                }
                if (Request.Cookies["MenuCloseOpen"] == null)
                {
                    styleMenuCloseOpen = "mini-navbar";
                }
                else
                {
                    styleMenuCloseOpen = "JK";
                }
                if (!Page.IsPostBack)
                {
                   
                   SetCompanyLogo();

                    
                    DateTime GetLastCompiled = File.GetLastWriteTime(Server.MapPath("~/VendorLogin.aspx"));
                    string strLastCompiled = GetLastCompiled.ToString("dd MMM yyyy") + " " + GetLastCompiled.ToString("hh:mm tt");

                    //Find Out DB Version Detail
//                    DataTable GetVersionDetail = new DataTable();
//                    GetVersionDetail = oDBEngine.GetDataTable(@"Select LTRIM(Rtrim(CurrentDbVersion_Number)) VersionNumber,
//                Convert(Varchar,CurrentDBVersion_CreateDateTime,106)+' '+LTRIM(RIGHT(CONVERT(VARCHAR(20), CurrentDBVersion_CreateDateTime, 100), 7)) Date
//                from Master_CurrentDBVersion");
//                    string strVersionNumber = String.Empty;
//                    string strVersionDateTime = String.Empty;
//                    if (GetVersionDetail.Rows.Count > 0)
//                    {
//                        strVersionNumber = GetVersionDetail.Rows[0][0].ToString();
//                        strVersionDateTime = GetVersionDetail.Rows[0][1].ToString();
//                    }
                    String[] PageFooterTags = ConfigurationManager.AppSettings["PageFooterTags"].ToString().Split('/');
                  
                    if (Session["username"] != null)
                    {
                        //string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;

                        //string ipaddress;
                        //ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        //if (ipaddress == "" || ipaddress == null)
                        //    ipaddress = Request.ServerVariables["REMOTE_ADDR"];

                        //string Mac = GetMAC();
                        //int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_UserLogin_Log", " User_Id,Datelogin,MacAddr,IpAddress", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Mac + "','" + ipaddress + "'");

                        //string ValueForUpdate = " user_status='1',Mac_Address='" + Mac + "',user_lastIP='" + ipaddress + "',last_login_date='" + oDBEngine.GetDate().ToString() + "'";


                        //int NoofRows = oDBEngine.SetFieldValue("tbl_master_user", ValueForUpdate, " user_id='" + Session["userid"].ToString() + "'");
                      
                        //string[,] segmnet = oDBEngine.GetFieldValue(" tbl_master_userGroup", " grp_segmentid", " grp_id in (" + HttpContext.Current.Session["usergoup"] + ")", 1);
                        //string segmentList = "";
                        //if (segmnet.Length > 0)
                        //{
                        //    for (int i = 0; i < segmnet.Length; i++)
                        //    {
                        //        segmentList += segmnet[i, 0].ToString() + ",";
                        //    }
                        //}

                        //-------------------
                        //if (Session["userAllowAccessIP"] != null)
                        //{
                        //    string userAllowAccessIP = Session["userAllowAccessIP"].ToString().Trim();
                        //    if (!string.IsNullOrEmpty(userAllowAccessIP))
                        //    {
                        //        string[] LoggedInIP = IPNAme.Trim().Split('.');
                        //        string[] AllowedIP = userAllowAccessIP.Trim().Split('.');
                        //        int j = 0;
                        //        for (j = 0; j < AllowedIP.Length; j++)
                        //        {
                        //            if (LoggedInIP[j].ToString() != AllowedIP[j].ToString())
                        //            {

                        //                Session["IPnotallowed"] = 1;

                        //                Response.Redirect("../login.aspx");
                        //            }
                        //        }
                        //    }
                        //}

                        //-------------------


//                        segmentList = segmentList.Substring(0, segmentList.Length - 1);
//                        Session["userallsegmentnotonlyLast"] = segmentList;
//                        string[,] ValidUserSegment = oDBEngine.GetFieldValue(" tbl_master_segment",
//                                                                       " seg_id,seg_name",
//                                                                       " seg_id in(" + segmentList + ")", 2, @"Case 
//	                                                                When Ltrim(Rtrim(Seg_name)) in ('NSDL','CDSL') Then 1
//	                                                                When Ltrim(Rtrim(Seg_name)) Like ('BSE%') Then 2
//	                                                                When Ltrim(Rtrim(Seg_name)) Like ('NSE%') Then 3
//	                                                                When Ltrim(Rtrim(Seg_name)) Like ('MCX%') Then 4
//	                                                                When Ltrim(Rtrim(Seg_name)) in ('Insurance','Accounts') Then 6
//	                                                                When LTRIM(Rtrim(Seg_Name)) in ('HR','CRM','Root') Then 7
//	                                                                Else 5
//                                                                End");

                       //string[,] data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
                       //     " ls_lastSettlementNo+ls_lastSettlementType as sett," +
                       //     " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " ))) ," +
                       //     " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " ))) ,ls_lastFinYear,ls_lastdpcoid as dpid,(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select CONVERT(VARCHAR(30),FinYear_EndDate,101) from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd ",
                       //     " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 8);

                        //if (data[0, 0] != "n")
                        //{
                        //  //  lblSCompName.Text = data[0, 0];
                        //   // lblFinYear.Text = data[0, 4];
                        //    HttpContext.Current.Session["LastFinYear"] = data[0, 4];

                        //    //lblStartDate.Text = data[0, 2];
                        //    //lblfundPayeeDate.Text = data[0, 3];
                        //    //lblFinYear.Text = data[0, 4];
                        //    HttpContext.Current.Session["LastFinYear"] = data[0, 4];

                        //    HttpContext.Current.Session["FinYearStart"] = data[0, 6];
                        //    HttpContext.Current.Session["FinYearEnd"] = data[0, 7];

                        //}

                    }



                   

                   // BindFavouriteMenu();
                   // ReAssign_Session();
                }
             


                HttpContext.Current.Session["ServerDate"] = oDBEngine.GetDate();


             


                //Currency Setting
                if (Session["LocalCurrency"] != null)
                {
                  //  lblCurrency.Text = "Base Currency : " + Session["LocalCurrency"].ToString().Split('~')[1].Trim() + "[" +
                   //        Session["LocalCurrency"].ToString().Split('~')[2].Trim() + "]";
                  
                }



               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //--------------
        }
        #region
        

        #endregion
        void ReAssign_Session()
        {
            
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable UserLastSegmentInfo = oGenericMethod.GetUserLastSegmentDetail();
            if (UserLastSegmentInfo.Rows.Count > 0)
            {
                HttpContext.Current.Session["StartdateFundsPayindate"] = UserLastSegmentInfo.Rows[0][5].ToString();
            }
          
        }
       
        private void UpdateLastSettTable(string SettNo)
        {
            string settNo = SettNo.Substring(0, SettNo.Trim().Length - 1);
            string type = SettNo.Substring(SettNo.Trim().Length - 1, 1);
            int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", " ls_lastSettlementNo='" + settNo + "', ls_lastSettlementType='" + type + "' ", " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString() + " and ls_lastFinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "'");
        }
        protected void btnBrowserClose_Click(object sender, EventArgs e)
        {
            int NoofRows = 0;
            HttpCookie cookie = Request.Cookies["sKey"];
            string getCookie = cookie.Value.ToString();

            string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;
            NoofRows = oDBEngine.SetFieldValue("tbl_master_user", "user_status='0',user_lastIP='" + IPNAme + "'", " user_loginid='" + getCookie + "'");

            HttpCookie myCookie = new HttpCookie("sKey");
            myCookie.Expires = oDBEngine.GetDate().AddDays(-1);
            Response.Cookies.Add(myCookie);

            Session.Abandon();
        }

        public void BindFavouriteMenu()
        {
            DataTable dtFavourite = new DataTable();
            if (Session["userlastsegment"] != null)
            {
                dtFavourite = oDBEngine.GetDataTable("tbl_trans_menu,Config_FavouriteMenu", "mnu_menuname,mnu_menuLink,dbo.fnSplit(dbo.fnSplit(FavouriteMenu_Image,'/',3),'.',1) AS ImageType,FavouriteMenu_ID", " FavouriteMenu_MenuID=mnu_id and FavouriteMenu_Segment=mnu_segmentID and FavouriteMenu_UserID=" + Session["userid"].ToString() + " and FavouriteMenu_Segment=" + Session["userlastsegment"].ToString() + "", "FavouriteMenu_Order"); ;
            }
            if (dtFavourite.Rows.Count > 0)
            {
                int FavItem = 0;
                int TotalItem = 0;
                TotalItem = dtFavourite.Rows.Count;
                if (dtFavourite.Rows.Count >= 10)
                    FavItem = 10;
                else
                    FavItem = dtFavourite.Rows.Count;
                string MainLink = null;
                string SubLink = null;
                MainLink = "<ul id=\"social\">";
                for (int i = 0; i < FavItem; i++)
                {
                    MainLink += "<li><a class=\"tiplink tip" + dtFavourite.Rows[i]["ImageType"].ToString() + "\" href=\"javascript:CallMenuPage('" + dtFavourite.Rows[i]["mnu_menuLink"].ToString() + "');\"></a>";
                    MainLink += "<div id=\"Div" + i + "\" class=\"tip\">";
                    MainLink += "<ul>";
                    MainLink += "<li><a href=\"javascript:CallMenuPage('" + dtFavourite.Rows[i]["mnu_menuLink"].ToString() + "');\">" + dtFavourite.Rows[i]["mnu_menuname"].ToString() + "</a></br><a href=\"javascript:PicChange(" + dtFavourite.Rows[i]["FavouriteMenu_ID"].ToString() + ");\">Change Picture</a></li>";
                    //MainLink += "<li><a href=\"javascript:PicChange();\">Change Picture</a></li>";
                    MainLink += "</ul>";
                    MainLink += "</div>";
                    MainLink += "</li>";
                }
                MainLink += "</ul>";

            }
            else
            {
                //Response.Redirect("~/AlertMessage.aspx",true);
                //toolbar.Visible = false;
            }
        }
       

      


        private string GetMAC()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }












        protected void SetCompanyLogo()
        {
            bigLogo.ImageUrl = "/assests/images/logo.png";
            miniLogo.ImageUrl = "/assests/images/logo-mini.png";

            bigLogo.ImageAlign = ImageAlign.Middle;
            miniLogo.ImageAlign = ImageAlign.Middle;
            //Set height


            //string[] logo = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo, cmp_smallLogo", "cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "'", 2);
            //if (logo.Length > 0)
            //{
            //    if (logo[0] != null && logo[0] != "")
            //    {

            //        if (File.Exists(Server.MapPath(logo[0])))
            //        {
            //            bigLogo.ImageUrl = logo[0];
            //        }

            //    }

            //    if (logo[1] != null && logo[1] != "")
            //    {
            //        if (File.Exists(Server.MapPath(logo[1])))
            //        {
            //            miniLogo.ImageUrl = logo[1];
            //        }
            //    }
            //}
        }



    }
}