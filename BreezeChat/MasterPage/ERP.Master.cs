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
//using ClsDropDownlistNameSpace;
using System.Net.NetworkInformation;
using DevExpress.Web;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Web.Services;
using System.Collections.Generic;
using UtilityLayer;


namespace ERP.OMS.MasterPage
{
    public partial class ERP : System.Web.UI.MasterPage
    {
        public static string PageFooterTag3 = string.Empty;
        public string styleMenuCloseOpen = string.Empty;
        Management_BL mng_bl = new Management_BL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine("");
        BusinessLogicLayer.GenericMethod oGenericMethod = null;
       // clsDropDownList OclsDropDownList = new clsDropDownList();
        MasterPageBL objMasterPageBL = new MasterPageBL();

        #region IMP Note Need to check Segment List It may be more than 1 value but right now i assume this is a single value

        #endregion IMP Note Need to check Segment List It may be more than 1 value but right now i assume this is a single value
        public void CompanyMasterDetail(string Companyid, string exchangeSegmentID, string userid, string userlastsegment, string usergrp)
        {
            DataSet dst = new DataSet();
            dst = objMasterPageBL.CompanyMasterDetail(Companyid, exchangeSegmentID, userid, userlastsegment, usergrp);
            string urlName = Convert.ToString(Request.UrlReferrer);

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                SetCompanyLogo(dst.Tables[0]);
            }

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                string strVersionNumber = String.Empty;
                string strVersionDateTime = String.Empty;

                strVersionNumber = Convert.ToString(dst.Tables[1].Rows[0]["VersionNumber"]);
                strVersionDateTime = Convert.ToString("2020-10-01");
                hiddnversion.Value = strVersionNumber.ToString();
            }


            if (Session["LastCompany"] != null && Session["UserSegID"] != null)
            {

                /* For 3 tier
                oDBEngine = new DBEngine("");
                 */
                oDBEngine = new BusinessLogicLayer.DBEngine("");

                #region 2nd table Start
                if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
                {
                    //if (DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "NSDL" && DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "CDSL")
                    //{
                    if (Session["ExchangeSegmentID"] != null)
                    {
                        if (Session["LastCompany"].ToString() != "n" && Session["ExchangeSegmentID"].ToString() != "n" && dst.Tables[2].Rows.Count > 0)
                        {


                            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)  //
                            {
                                Session["TradeCurrency"] = Convert.ToString(dst.Tables[3].Rows[0]["TradeCurrency"]);
                                Session["LocalCurrency"] = Convert.ToString(dst.Tables[3].Rows[1]["TradeCurrency"]);
                                //First Time Active Currency Will Be Trade Currency ByDefault....
                                Session["ActiveCurrency"] = Convert.ToString(dst.Tables[3].Rows[0]["TradeCurrency"]);

                                //Session["TradeCurrency"] = DTCurrency.Rows[0][0].ToString();
                                //Session["LocalCurrency"] = DTCurrency.Rows[1][0].ToString();
                                ////First Time Active Currency Will Be Trade Currency ByDefault....
                                //Session["ActiveCurrency"] = DTCurrency.Rows[0][0].ToString();


                                //if (Session["TradeCurrency"].ToString() != Session["LocalCurrency"].ToString())
                                //{
                                //lblCurrencyNameSymbol.Text = DTCurrency.Rows[0][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[0][0].ToString().Split('~')[2].Trim() + "]";
                                //AChangeCurrency.InnerHtml = "Switch To " + DTCurrency.Rows[1][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[1][0].ToString().Split('~')[2].Trim() + "]";
                                //}
                                //else
                                //{
                                //lblCurrencyNameSymbol.Text = DTCurrency.Rows[0][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[0][0].ToString().Split('~')[2].Trim() + "]";
                                //AChangeCurrency.InnerHtml = String.Empty;
                                //}
                            }
                            //DTCurrency.Dispose();
                            //DTCompanyDetail.Dispose();
                        }
                        else
                            CurrencySetting_Null();
                        // }
                        //else
                        //{
                        //    if (DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "Accounts") CurrencySetting_NonExchangeSegment();
                        //    else CurrencySetting_Null();

                        //}
                    }
                    else
                    {
                        CurrencySetting_NonExchangeSegment();
                    }
                }
                else
                {
                    CurrencySetting_Null();
                }
            }
            else
            {
                CurrencySetting_Null();
            }
                #endregion 2nd Table End

            if (Session["userlastsegment"] != null)
            {
                #region 4th Table
                if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
                {


                    //dtFavourite = oDBEngine.GetDataTable("tbl_trans_menu,Config_FavouriteMenu", "mnu_menuname,mnu_menuLink,dbo.fnSplit(dbo.fnSplit(FavouriteMenu_Image,'/',3),'.',1) AS ImageType,FavouriteMenu_ID", " FavouriteMenu_MenuID=mnu_id and FavouriteMenu_Segment=mnu_segmentID and FavouriteMenu_UserID=" + Session["userid"].ToString() + " and FavouriteMenu_Segment=" + Session["userlastsegment"].ToString() + "", "FavouriteMenu_Order"); ;


                    int FavItem = 0;
                    int TotalItem = 0;
                    TotalItem = dst.Tables[4].Rows.Count;
                    if (dst.Tables[4].Rows.Count >= 10)
                        FavItem = 10;
                    else
                        FavItem = dst.Tables[4].Rows.Count;
                    string MainLink = null;
                    string SubLink = null;
                    MainLink = "<ul id=\"social\">";
                    for (int i = 0; i < FavItem; i++)
                    {
                        MainLink += "<li><a class=\"tiplink tip" + Convert.ToString(dst.Tables[4].Rows[i]["ImageType"]) + "\" href=\"javascript:CallMenuPage('" + Convert.ToString(dst.Tables[4].Rows[i]["mnu_menuLink"]) + "');\"></a>";
                        MainLink += "<div id=\"Div" + i + "\" class=\"tip\">";
                        MainLink += "<ul>";
                        MainLink += "<li><a href=\"javascript:CallMenuPage('" + Convert.ToString(dst.Tables[4].Rows[i]["mnu_menuLink"]) + "');\">" + Convert.ToString(dst.Tables[4].Rows[i]["mnu_menuname"]) + "</a></br><a href=\"javascript:PicChange(" + Convert.ToString(dst.Tables[4].Rows[i]["FavouriteMenu_ID"]) + ");\">Change Picture</a></li>";
                        //MainLink += "<li><a href=\"javascript:PicChange();\">Change Picture</a></li>";
                        MainLink += "</ul>";
                        MainLink += "</div>";
                        MainLink += "</li>";
                    }
                    MainLink += "</ul>";
                }
                #endregion  4th Table
                #region Table 5
                if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
                {
                    HttpContext.Current.Session["StartdateFundsPayindate"] = Convert.ToString(dst.Tables[5].Rows[0]["StartdateFundsPayindate"]);
                }
                #endregion Table 5


            }
            if (Session["username"] != null)
            {
                if (Session["userAllowAccessIP"] != null)
                {
                    string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;
                    string userAllowAccessIP = Session["userAllowAccessIP"].ToString().Trim();
                    if (!string.IsNullOrEmpty(userAllowAccessIP))
                    {
                        string[] LoggedInIP = IPNAme.Trim().Split('.');
                        string[] AllowedIP = userAllowAccessIP.Trim().Split('.');
                        int j = 0;
                        for (j = 0; j < AllowedIP.Length; j++)
                        {
                            if (LoggedInIP[j].ToString() != AllowedIP[j].ToString())
                            {

                                Session["IPnotallowed"] = 1;

                                Response.Redirect("../login.aspx");
                            }
                        }
                    }
                }
                #region Table 6
                if (dst.Tables[6] != null && dst.Tables[6].Rows.Count > 0)
                {
                    Session["userallsegmentnotonlyLast"] = Convert.ToString(dst.Tables[6].Rows[0]["grp_segmentid"]);
                }
                #endregion Table 6
            }

            #region Table 7 no use

            #endregion Table 7

            #region Table 8
            if (Session["LastFinYear"] == null || Session["FinYearStart"] == null || Session["FinYearEnd"] == null || Session["lablelCompanyName"] == null)
            {
                if (dst.Tables[8] != null && dst.Tables[8].Rows.Count > 0)
                {

                    lblSCompName.Text = Convert.ToString(dst.Tables[8].Rows[0]["comp"]) + " (" + Convert.ToString(dst.Tables[8].Rows[0]["Branch"]) + ")";
                    // lblFinYear.Text = Convert.ToString(dst.Tables[8].Rows[0]["ls_lastFinYear"]);
                    lblFinYear.Text = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("dd-MM-yyyy") + " - " + Convert.ToDateTime(HttpContext.Current.Session["FinYearEnd"]).ToString("dd-MM-yyyy");
                    HttpContext.Current.Session["lablelCompanyName"] = Convert.ToString(dst.Tables[8].Rows[0]["comp"]) + " (" + Convert.ToString(dst.Tables[8].Rows[0]["Branch"]) + ")";
                    HttpContext.Current.Session["LastFinYear"] = Convert.ToString(dst.Tables[8].Rows[0]["ls_lastFinYear"]);
                    HttpContext.Current.Session["FinYearStart"] = Convert.ToString(dst.Tables[8].Rows[0]["FinYearStart"]);
                    HttpContext.Current.Session["FinYearEnd"] = Convert.ToString(dst.Tables[8].Rows[0]["FinYearEnd"]);
                }
            }
            else
            {

                // lblFinYear.Text = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
               // lblFinYear.Text = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("dd-MM-yyyy") + " - " + Convert.ToDateTime(HttpContext.Current.Session["FinYearEnd"]).ToString("dd-MM-yyyy");
                lblSCompName.Text = Convert.ToString(HttpContext.Current.Session["lablelCompanyName"]);
            }

            if (dst.Tables[8] != null && dst.Tables[8].Rows.Count > 0)
            {
                lblSCompName.Text = Convert.ToString(dst.Tables[8].Rows[0]["comp"]) + " (" + Convert.ToString(dst.Tables[8].Rows[0]["Branch"]) + ")";
                //lblFinYear.Text = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("dd-MM-yyyy") + " - " + Convert.ToDateTime(HttpContext.Current.Session["FinYearEnd"]).ToString("dd-MM-yyyy");
            }

            //lblSCompName.Text = data[0, 0] + " (" + data[0, 8] + ")";
            //lblFinYear.Text = data[0, 4];
            //HttpContext.Current.Session["LastFinYear"] = data[0, 4];

            ////lblStartDate.Text = data[0, 2];
            ////lblfundPayeeDate.Text = data[0, 3];
            ////lblFinYear.Text = data[0, 4];
            //HttpContext.Current.Session["LastFinYear"] = data[0, 4];

            //HttpContext.Current.Session["FinYearStart"] = data[0, 6];
            //HttpContext.Current.Session["FinYearEnd"] = data[0, 7];
            #endregion Table 8
        }
        protected void Page_Load(object sender, EventArgs e)
        {



            PageFooterTag3 = System.Configuration.ConfigurationManager.AppSettings["PageFooterTag3"].ToString();
            try
            {
                //Purpose: Chrcking that the user coming from redirect url or direct url. If the user wants access any page via its url then the user will be redirect to alert page
                //Name: Debjyoti Dhar. 21-11-2016

                if (!HttpContext.Current.Request.Url.AbsoluteUri.ToString().Contains("http://localhost"))
                {
                    if (!HttpContext.Current.Request.Url.AbsoluteUri.ToString().Contains("ProjectMainPage"))
                    {
                        if (HttpContext.Current.Session["DeveloperRedirect"] == null)
                        {
                            if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] == null)
                            {
                                HttpContext.Current.Response.Redirect("/oms/AlertMessage.aspx");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Session.Remove("DeveloperRedirect");
                        }
                    }
                }


                if (Session["errSession"] != null)
                {
                    Session.Remove("errSession");
                    HttpContext.Current.Response.Write("<script> alert('You do not have permission to perform this operation');</script>");
                    return;
                }
                HttpCookie cookie = Request.Cookies["sKey"];
                hdnCurrentUserIDS.Value = HttpContext.Current.Session["userid"].ToString();
                DBEngine obj = new DBEngine();

                DataTable dt = obj.GetDataTable("select user_id,grp_name from tbl_master_user inner join tbl_master_userGroup on user_group=grp_id WHERE user_id='" + hdnCurrentUserIDS.Value + "'");
                string group_name = "";

                if (dt != null && dt.Rows.Count > 0)
                {
                    group_name = Convert.ToString(dt.Rows[0]["grp_name"]);
                }
                hdnCurrentGroup.Value = group_name;
                //if (cookie != null)
                //{
                //    LogedUser.Text = Convert.ToString(cookie.Value);
                //}


                lblVersion.InnerText = Convert.ToString(HttpContext.Current.Session["LastVersion"]);
                version2.InnerText = Convert.ToString(HttpContext.Current.Session["LastVersion"]);
                if (LogedUser.Text == "" && HttpContext.Current.Session["username"] != null)
                {
                    LogedUser.Text = Convert.ToString(HttpContext.Current.Session["username"]);
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
                    //Debjyoti 
                    //SetCompanyLogo company LogoutAction on top

                    #region Old Modification By Sam on 25092017
                    //SetCompanyLogo();  1111111111
                    #endregion
                    #region Sam Section For New Modification on 25092017
                    CompanyMasterDetail(Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["ExchangeSegmentID"]), Convert.ToString(Session["userid"]), Convert.ToString(Session["userlastsegment"]), Convert.ToString(HttpContext.Current.Session["usergoup"]));

                    #endregion Sam Section For New Modification on 25092017

                    #region No Use
                    //Purpose : Show Menu Name in Page Header / Title
                    //Name : Sudip 
                    // Dated : 18-01-2017
                    ////try
                    ////{
                    ////    string Originalpath = HttpContext.Current.Request.Url.AbsolutePath;
                    ////    string path = Originalpath.Replace("/OMS/", "/");
                    ////    DataTable dtPath = oDBEngine.GetDataTable("tbl_trans_menu", "ltrim(rtrim(mnu_menuName))", " mnu_menuLink='" + Convert.ToString(path).Trim() + "'");
                    ////    if (dtPath != null && dtPath.Rows.Count > 0)
                    ////    {
                    ////        Page.Title = Convert.ToString(dtPath.Rows[0][0]);
                    ////    }
                    ////    else if (Session["requesttype"] != null)
                    ////    {
                    ////        string ContType = Convert.ToString(Session["requesttype"]);

                    ////        if (ContType != "")
                    ////        {
                    ////            if (ContType == "Customer/Client")
                    ////            {
                    ////                Page.Title = "Customers / Clients";
                    ////            }
                    ////            else if (ContType == "Franchisee")
                    ////            {
                    ////                Page.Title = "Franchisees";
                    ////            }
                    ////            else if (ContType == "Salesman/Agents")
                    ////            {
                    ////                Page.Title = "Salesman/Agents";
                    ////            }
                    ////            else if (ContType == "Creditors")
                    ////            {
                    ////                Page.Title = "Creditors";
                    ////            }
                    ////            else if (ContType == "Business Partner")
                    ////            {
                    ////                Page.Title = "Business Partners";
                    ////            }
                    ////            else if (ContType == "Book Runners")
                    ////            {
                    ////                Page.Title = "Book Runners";
                    ////            }
                    ////            else if (ContType == "Relationship Partners")
                    ////            {
                    ////                Page.Title = "Relationship Partners";
                    ////            }
                    ////            else if (ContType == "Partner")
                    ////            {
                    ////                Page.Title = "Business Partners";
                    ////            }
                    ////            else if (ContType == "OtherEntity")
                    ////            {
                    ////                Page.Title = "Other Entity";
                    ////            }
                    ////            else
                    ////            {
                    ////                Page.Title = ContType + "s";
                    ////            }
                    ////        }
                    ////        else
                    ////        {
                    ////            Page.Title = "Welcome to BreezeERP";
                    ////        }
                    ////    }
                    ////    else
                    ////    {
                    ////        Page.Title = "Welcome to BreezeERP";
                    ////    }
                    ////}
                    ////catch { }
                    // End

                    //Find Out Last Compiled
                    //oDBEngine.PopulateAllMenu(Menumain);
                    #endregion No Use
                    DateTime GetLastCompiled = File.GetLastWriteTime(Server.MapPath("~/Login.aspx"));
                    string strLastCompiled = GetLastCompiled.ToString("dd MMM yyyy") + " " + GetLastCompiled.ToString("hh:mm tt");

                    //Find Out DB Version Detail

                    #region Sam Section Commented For New Modification on 25092017  geet

                    //                    DataTable GetVersionDetail = new DataTable();
                    //                    GetVersionDetail = oDBEngine.GetDataTable(@"Select LTRIM(Rtrim(CurrentDbVersion_Number)) VersionNumber,
                    //                Convert(Varchar,CurrentDBVersion_CreateDateTime,106)+' '+LTRIM(RIGHT(CONVERT(VARCHAR(20), CurrentDBVersion_CreateDateTime, 100), 7)) Date
                    //                from Master_CurrentDBVersion");
                    //string strVersionNumber = String.Empty;
                    //string strVersionDateTime = String.Empty;
                    //if (GetVersionDetail.Rows.Count > 0)
                    //{
                    //    strVersionNumber = GetVersionDetail.Rows[0][0].ToString();
                    //    strVersionDateTime = GetVersionDetail.Rows[0][1].ToString();
                    //}

                    #endregion Sam Section  Commented For New Modification on 25092017

                    #region No Need to optimise
                    //String[] PageFooterTags = ConfigurationManager.AppSettings["PageFooterTags"].ToString().Split('/');
                    //if (PageFooterTags != null)
                    //{
                    //tdCopyRight.InnerText = PageFooterTags[0].Trim() + " ( v " + strVersionNumber + " | " + strLastCompiled + " )";
                    //tdPowerBy.InnerText = PageFooterTags[1];
                    //tdTechnialBy.InnerText = PageFooterTags[2];
                    //}
                    //else
                    //{
                    //tdCopyRight.InnerText = "Copyright © 2009 Influx Technologies ( v " + strVersionNumber + " | " + strLastCompiled + " )";
                    //tdPowerBy.InnerText = "Powered By INFLUX";
                    //tdTechnialBy.InnerText = "Technicals by : Influx Technologies";
                    //}
                    #endregion No Need to optimise

                    #region Need to optimise Later by Sam on 25092017

                    if (Session["username"] != null)
                    {
                        string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;
                        string urlName = Convert.ToString(Request.UrlReferrer);
                        //if (urlName == "http://localhost:7665/OMS/login.aspx" || urlName == "login.aspx")
                        if (urlName.ToUpper().Contains("LOGIN.ASPX"))
                        {
                            string ipaddress;
                            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (ipaddress == "" || ipaddress == null)
                                ipaddress = Request.ServerVariables["REMOTE_ADDR"];

                            // string Mac = GetMAC();
                            //string Mac = Getmaccccaddress22();

                            //int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_UserLogin_Log", " User_Id,Datelogin,MacAddr,IpAddress", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Mac + "','" + ipaddress + "'");
                            int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_UserLogin_Log", " User_Id,Datelogin,IpAddress", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + ipaddress + "'");
                            //string ValueForUpdate = " user_status='1',Mac_Address='" + Mac + "',user_lastIP='" + ipaddress + "',last_login_date='" + oDBEngine.GetDate().ToString() + "'";
                            string ValueForUpdate = " user_status='1',user_lastIP='" + ipaddress + "',last_login_date='" + oDBEngine.GetDate().ToString() + "'";

                            int NoofRows = oDBEngine.SetFieldValue("tbl_master_user", ValueForUpdate, " user_id='" + Session["userid"].ToString() + "'");
                            //lblName.Text = oDBEngine.GetFieldValue("tbl_master_user", " (user_loginid +'[' + (user_name + '['+ (select cnt_shortName from tbl_master_contact where cnt_internalId=tbl_master_user.user_contactId )+']') + '(<b style=\"color:Red\">'+Case When User_EntryProfile='C' then 'Checker' When User_EntryProfile='M' then 'Maker' When User_EntryProfile='P' then 'Prov.' When User_EntryProfile='F' then 'Final' Else User_EntryProfile End +'</b>)'+']') as name ", "user_id = " + Session["userid"].ToString(), 1)[0, 0].ToString();
                            //lblLastTime.Text = oDBEngine.GetFieldValue(" tbl_master_user", "REPLACE(CONVERT(VARCHAR(11), last_login_date, 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,last_login_date,100 ) ,7), 6, 0, ' ')", "user_id = " + Session["userid"].ToString(), 1)[0, 0].ToString();
                        }

                        #region Segemnt and Group Setting of User By Sam on 03102017


                        #endregion Segemnt and Group Setting of User By Sam on 03102017
                        #region Code Commented By sam on 03102017
                        //string[,] segmnet = oDBEngine.GetFieldValue(" tbl_master_userGroup", " grp_segmentid", " grp_id in (" + HttpContext.Current.Session["usergoup"] + ")", 1);
                        string segmentList = "";
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
                        #endregion Code Commented By sam on 03102017
                        //-------------------


                        //segmentList = segmentList.Substring(0, segmentList.Length - 1);
                        //Session["userallsegmentnotonlyLast"] = segmentList;
                        #region Code Commented By sam on 03102017
                        //                        string[,] ValidUserSegment = oDBEngine.GetFieldValue(" tbl_master_segment",
                        //  " seg_id,seg_name",
                        //   " seg_id in(" + segmentList + ")", 2, @"Case 
                        //	  When Ltrim(Rtrim(Seg_name)) in ('NSDL','CDSL') Then 1
                        //	  When Ltrim(Rtrim(Seg_name)) Like ('BSE%') Then 2
                        //	  When Ltrim(Rtrim(Seg_name)) Like ('NSE%') Then 3
                        //	  When Ltrim(Rtrim(Seg_name)) Like ('MCX%') Then 4
                        //	  When Ltrim(Rtrim(Seg_name)) in ('Insurance','Accounts') Then 6
                        //	  When LTRIM(Rtrim(Seg_Name)) in ('HR','CRM','Root') Then 7
                        //	  Else 5
                        //    End");

                        #endregion Code Commented By sam on 03102017

                        //Calling a function to add values to a dropdownlist
                        //First argument is data array and secont is comboboxID
                        // oDBEngine.AddDataToDropDownList(ValidUserSegment, cmbSegment, int.Parse(HttpContext.Current.Session["userlastsegment"].ToString()));
                        // OclsDropDownList.AddDataToDropDownList(ValidUserSegment, cmbSegment, int.Parse(HttpContext.Current.Session["userlastsegment"].ToString()));

                        //if (cmbSegment.SelectedItem.Value != "")
                        //{
                        //    oDBEngine.PopulateMenu(Menumain, cmbSegment.SelectedItem.Value);
                        //}

                        //if (cmbSegment.SelectedItem.Value.Trim() == "7" || cmbSegment.SelectedItem.Value.Trim() == "8" || cmbSegment.SelectedItem.Value.Trim() == "9" || cmbSegment.SelectedItem.Value.Trim() == "10" ||
                        //    cmbSegment.SelectedItem.Value.Trim() == "5" || cmbSegment.SelectedItem.Value.Trim() == "11" || cmbSegment.SelectedItem.Value.Trim() == "12" ||
                        //    cmbSegment.SelectedItem.Value.Trim() == "15" || cmbSegment.SelectedItem.Value.Trim() == "14" || cmbSegment.SelectedItem.Value.Trim() == "23" || cmbSegment.SelectedItem.Value.Trim() == "18" ||
                        //    cmbSegment.SelectedItem.Value.Trim() == "16" || cmbSegment.SelectedItem.Value.Trim() == "17" || cmbSegment.SelectedItem.Value.Trim() == "24" || cmbSegment.SelectedItem.Value.Trim() == "25" ||
                        //    cmbSegment.SelectedItem.Value.Trim() == "26" || cmbSegment.SelectedItem.Value.Trim() == "28" || cmbSegment.SelectedItem.Value.Trim() == "29" ||
                        //    cmbSegment.SelectedItem.Value.Trim() == "30" || cmbSegment.SelectedItem.Value.Trim() == "31" || cmbSegment.SelectedItem.Value.Trim() == "32" || cmbSegment.SelectedItem.Value.Trim() == "33" || cmbSegment.SelectedItem.Value.Trim() == "34" || cmbSegment.SelectedItem.Value.Trim() == "35" || cmbSegment.SelectedItem.Value.Trim() == "36" || cmbSegment.SelectedItem.Value.Trim() == "37" || cmbSegment.SelectedItem.Value.Trim() == "38" || cmbSegment.SelectedItem.Value.Trim() == "39" || cmbSegment.SelectedItem.Value.Trim() == "40")
                        //{
                        //tblSegment.Visible = true;
                        //if (cmbSegment.SelectedItem.Value.Trim() == "5")
                        //    Session["usersegid"] = "0";
                        //if (cmbSegment.SelectedItem.Value.Trim() == "5" || cmbSegment.SelectedItem.Value.Trim() == "9" || cmbSegment.SelectedItem.Value.Trim() == "10")
                        //    SettBtnPart.Visible = false;
                        //else
                        //    SettBtnPart.Visible = true;

                        //string[,] data = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_Name ,(select convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))='" + HttpContext.Current.Session["LastSettNo"].ToString() + "') ,(select convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))='" + HttpContext.Current.Session["LastSettNo"].ToString() + "') ", " cmp_internalid ='" + HttpContext.Current.Session["LastCompany"].ToString() + "'", 3);
                        ////string[,] data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
                        ////    " ls_lastSettlementNo+ls_lastSettlementType as sett," +
                        ////    " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ," +
                        ////    " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ,ls_lastFinYear,ls_lastdpcoid as dpid,(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select FinYear_EndDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd ",
                        ////    " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 8);


                        #region Code Commented By sam on 03102017
                        //string[,] data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
                        //    " ls_lastSettlementNo+ls_lastSettlementType as sett," +
                        //    " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " ))) ," +
                        //    " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " ))) ,ls_lastFinYear,ls_lastdpcoid as dpid,(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select CONVERT(VARCHAR(30),FinYear_EndDate,101) from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd, "+
                        //    "(select branch_description from tbl_master_branch where branch_id=(select user_branchId from tbl_master_user where user_id=ls_userId)) Branch",
                        //    " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 9);

                        //if (data[0, 0] != "n")
                        //{
                        //    lblSCompName.Text = data[0, 0] + " (" + data[0, 8]+")";
                        //    lblFinYear.Text = data[0, 4];
                        //    HttpContext.Current.Session["LastFinYear"] = data[0, 4];

                        //    //lblStartDate.Text = data[0, 2];
                        //    //lblfundPayeeDate.Text = data[0, 3];
                        //    //lblFinYear.Text = data[0, 4];
                        //    HttpContext.Current.Session["LastFinYear"] = data[0, 4];

                        //    HttpContext.Current.Session["FinYearStart"] = data[0, 6];
                        //    HttpContext.Current.Session["FinYearEnd"] = data[0, 7];

                        //}

                        #endregion Code Commented By sam on 03102017

                    }

                    #endregion Need to optimise Later by Sam on 25092017

                    //Set Currrency On Page Load
                    //CurrencySetting();

                    //BindFavouriteMenu();
                    //ReAssign_Session();
                    #region Showlastdatetime
                    //DataTable lastlogindatetime=new DataTable();
                    //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    // con.Open();
                    //SqlCommand cmd = new SqlCommand("Prc_GetEmployeeDetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@emp_contactId", Session["userid"]);
                    //cmd.Parameters.AddWithValue("@Action", "Getlastlogindatetime");
                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(lastlogindatetime);
                    //if (lastlogindatetime.Rows.Count > 0)
                    //{
                    //    if (lastlogindatetime.Rows[0]["Lastlogindate"].ToString() != "" && lastlogindatetime.Rows[0]["Lastlogindate"].ToString() != null
                    //    && lastlogindatetime.Rows[0]["Lastlogindate"] != DBNull.Value)
                    //    {
                    //        lbllastlogindate.Text = lastlogindatetime.Rows[0]["Lastlogindate"].ToString();
                    //    }
                    //    if (lastlogindatetime.Rows[0]["Lastlogintime"].ToString() != "" && lastlogindatetime.Rows[0]["Lastlogintime"].ToString() != null
                    //    && lastlogindatetime.Rows[0]["Lastlogintime"] != DBNull.Value)
                    //    {
                    //        lbllastlogintime.Text= lastlogindatetime.Rows[0]["Lastlogintime"].ToString();
                    //    }
                    //}
                    //DataTable dtgetpi = new DataTable();
                    //SqlCommand cmdgetpi = new SqlCommand("Prc_GetEmployeeDetails", con);
                    //cmdgetpi.CommandType = CommandType.StoredProcedure;
                    //cmdgetpi.Parameters.AddWithValue("@emp_contactId", Session["userid"]);
                    //cmdgetpi.Parameters.AddWithValue("@Action", "Getprofileimage");
                    //cmdgetpi.CommandTimeout = 0;
                    //SqlDataAdapter dagetpi = new SqlDataAdapter();
                    //dagetpi.SelectCommand = cmdgetpi;
                    //dagetpi.Fill(dtgetpi);
                    //if (dtgetpi.Rows.Count > 0)
                    //{
                    //    if (dtgetpi.Rows[0]["emp_image"].ToString() != "" && dtgetpi.Rows[0]["emp_image"].ToString() != null
                    //    && dtgetpi.Rows[0]["emp_image"] != DBNull.Value)
                    //    {
                    //        string strsrc = dtgetpi.Rows[0]["emp_image"].ToString();
                    //        imgpp.Src = strsrc.Substring(8); ;
                    //    }
                    //   else
                    //     { 
                    //         imgpp.Src="/assests/images/profile1.png";
                    //     }
                    //}
                    //else
                    //{
                    //    imgpp.Src = "/assests/images/profile1.png";
                    //}
                    if (HttpContext.Current.Session["Lastlogindate"] != null && HttpContext.Current.Session["Lastlogindate"] != "")
                    {
                        lbllastlogindate.Text = HttpContext.Current.Session["Lastlogindate"].ToString();
                    }
                    if (HttpContext.Current.Session["Lastlogintime"] != null && HttpContext.Current.Session["Lastlogintime"] != "")
                    {
                        lbllastlogintime.Text = HttpContext.Current.Session["Lastlogintime"].ToString();
                    }
                    if (HttpContext.Current.Session["UserImage"] != "" && HttpContext.Current.Session["UserImage"] != null)
                    {
                        string UploadFileDirectory = System.Web.Configuration.WebConfigurationManager.AppSettings["SaveFile"].ToString();
                        imgpp.Src = "~/" + UploadFileDirectory + HttpContext.Current.Session["UserImage"].ToString();
                    }
                    else
                    {
                        imgpp.Src = "/assests/images/profile1.png";
                    }
                    #endregion Showlastdatetime
                }
                else
                {
                    if (Session["ActiveCurrency"] != null)
                    {
                        if (Convert.ToInt32(Session["LocalCurrency"].ToString().Split('~')[0]) != Convert.ToInt32(Session["TradeCurrency"].ToString().Split('~')[0]))
                        {
                            //lblCurrencyNameSymbol.Text = Session["ActiveCurrency"].ToString().Split('~')[1].Trim() + " [" + Session["ActiveCurrency"].ToString().Split('~')[2].Trim() + "]";
                            //if (Session["ActiveCurrency"] != Session["TradeCurrency"])
                            //{
                            //    AChangeCurrency.InnerHtml = "Switch To " + Session["TradeCurrency"].ToString().Split('~')[1].Trim() + " [" + Session["TradeCurrency"].ToString().Split('~')[2].Trim() + "]";
                            //}
                            //else
                            //{
                            //    AChangeCurrency.InnerHtml = "Switch To " + Session["LocalCurrency"].ToString().Split('~')[1].Trim() + " [" + Session["LocalCurrency"].ToString().Split('~')[2].Trim() + "]";
                            //}
                        }
                    }
                }


                HttpContext.Current.Session["ServerDate"] = oDBEngine.GetDate();


                if (!Page.IsPostBack)
                    Check_ExpiryStatus();
                #region Comment Check_ExpiryStatus() Code
                //=====Start Check Global Expiry Date Before 7 days===================
                /* For 3 tier structure 
                oGenericMethod = new GenericMethod();
                */
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string Licns_GlobalExpiry = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//GlobalExpiry~", System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
                if (Licns_GlobalExpiry.Length > 0)
                {
                    if (Convert.ToDateTime(Licns_GlobalExpiry) > oDBEngine.GetDate())
                    {
                        TimeSpan oGlobalTimeSpan = Convert.ToDateTime(Licns_GlobalExpiry) - oDBEngine.GetDate();
                        double GlobalExpireDayDiff = oGlobalTimeSpan.TotalDays;
                        int IsActiveGlobalExpiry = Convert.ToInt32(oDBEngine.GetDataTable("Select GlobalSettings_Value Value from Config_GlobalSettings where GlobalSettings_Name='GS_GEXPDT' and GlobalSettings_SegmentID=0").Rows[0]["Value"].ToString());
                        if ((GlobalExpireDayDiff <= 7) && (IsActiveGlobalExpiry == 1))
                        {
                            int globalDayDiff = Convert.ToDateTime(Licns_GlobalExpiry).Day - oDBEngine.GetDate().Day;
                            Page.ClientScript.RegisterStartupScript(GetType(), "GlobalExpire1", "<script>fn_ExpiryOverlay('Your Application Will Expired In <b>" + globalDayDiff.ToString() + "</b> Day(s).<br/>Please Upgrade Your License!!!');</script>");
                        }
                    }
                }
                else
                {
                    Licns_GlobalExpiry = "2099-12-31";
                    string CurrentCompanyName = string.Empty;
                    string CurrentSegmentName = string.Empty;
                    if (Convert.ToString(Session["LastCompany"]) != "")
                    {
                        //Session["LastCompany"] = "COR0000002"; // remove this when issue resolved
                        ///CurrentCompanyName = Convert.ToString(oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1]);

                        //DataTable dtorg = new DataTable();
                        //dtorg = oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString());
                        //if (dtorg.Rows.Count > 0)
                        //{
                        //    CurrentCompanyName = Convert.ToString(oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1]);
                        //}

                        if (Session["lablelCompanyName"] != null)
                        {
                            CurrentCompanyName = Convert.ToString(Session["lablelCompanyName"]);
                        }
                        else
                        {
                            DataTable dtorg = new DataTable();
                            dtorg = oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString());

                            if (dtorg.Rows.Count > 0)
                            {
                                CurrentCompanyName = Convert.ToString(dtorg.Rows[0][1]);
                            }
                        }
                    }
                    if (Convert.ToString(Session["userlastsegment"]) != "")
                    {
                        CurrentSegmentName = "Accounts";
                        //CurrentSegmentName = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1)[0];
                    }


                    oGenericMethod = new BusinessLogicLayer.GenericMethod();

                    string Licns_ExpiryByCompSeg = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//cName[@Value='" + CurrentCompanyName + "']/@Value,//Segments[@Value='" + CurrentSegmentName + "']/@Value,//cName[@Value='" + CurrentCompanyName + "']/Segments[@Value='" + CurrentSegmentName + "']/Expiry~" + CurrentCompanyName + "," + CurrentSegmentName, System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
                    if (Licns_ExpiryByCompSeg.Length > 0 && Licns_ExpiryByCompSeg != "Invalid License!!!")
                    {
                        if (Convert.ToDateTime(Licns_ExpiryByCompSeg) > oDBEngine.GetDate())
                        {
                            TimeSpan oSegmentTimeSpan = Convert.ToDateTime(Licns_ExpiryByCompSeg) - oDBEngine.GetDate();
                            double SegExpireDayDiff = oSegmentTimeSpan.TotalDays;
                            if (SegExpireDayDiff <= 7)
                            {
                                int segWiseDayDiff = Convert.ToDateTime(Licns_ExpiryByCompSeg).Day - oDBEngine.GetDate().Day;
                                Page.ClientScript.RegisterStartupScript(GetType(), "SegmentExpire1", "<script language='javascript'>fn_ExpiryOverlay('Your <b>" + CurrentSegmentName + "</b> Segment Of This Application Will Expire in <b>" + segWiseDayDiff.ToString() + "</b> Day(s).<br/>Please Upgrade Your License!!!');</script>");
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "SegmentExpire1", "<script language='javascript'>fn_ExpiryOverlay('Your <b>" + CurrentSegmentName + "</b> Segment Of This Application Was Already Expired on <b>" + Licns_ExpiryByCompSeg + "</b> Date.<br/>Some Features Are Blocked.<br/>Please Upgrade Your License!!!');</script>");
                        }
                    }
                }
                //=====End Check Global Expiry Date Before 7 days===================
                #endregion

                // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................

                //  lnkSelectCompanySettFinYear.HRef = "javascript:showpage('" + Convert.ToString(HttpContext.Current.Session["userlastsegment"]) + "')";


                //Currency Setting
                if (Session["LocalCurrency"] != null)
                {
                    lblCurrency.Text = "Base Currency : " + Session["LocalCurrency"].ToString().Split('~')[1].Trim() + "[" +
                           Session["LocalCurrency"].ToString().Split('~')[2].Trim() + "]";

                    //B_ChoosenCurrency.InnerText = "Trade Currency : " + Session["ActiveCurrency"].ToString().Split('~')[1].Trim() + "[" +
                    //       Session["ActiveCurrency"].ToString().Split('~')[2].Trim() + "]";
                    //if (!CbpChoosenCurrency.IsCallback)
                    //{
                    //    //if (Session["LocalCurrency"].ToString().Trim() != Session["TradeCurrency"].ToString().Trim())
                    //    //{
                    //        B_ChoosenCurrency.Attributes.Add("onclick", "ChangeCurrency()");
                    //        B_ChoosenCurrency.Style.Add("cursor", "hand");
                    //    //}
                    //}
                }



                // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). .....................................
                //----------------


                //string[,] data1 = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
                //          " ls_lastSettlementNo+ls_lastSettlementType as sett," +
                //          " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ," +
                //          " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ,ls_lastFinYear,ls_lastdpcoid as dpid ",
                //          " ls_cntId='" + HttpContext.Current.Session["usercontactID"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 6);
                //if (data1[0, 0] != "n")
                //{
                //    lblSCompName.Text = data1[0, 0];
                //    if (data1[0, 1] != "")
                //    {
                //        //lblSettNo.Text = data1[0, 1];
                //        //lblStartDate.Text = data1[0, 2];
                //        //lblfundPayeeDate.Text = data1[0, 3];
                //        lblFinYear.Text = data1[0, 4];
                //        HttpContext.Current.Session["LastFinYear"] = data1[0, 4];
                //        //HttpContext.Current.Session["LastSettNo"] = data1[0, 1];
                //    }             

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //--------------

        }
        #region
        //        protected void cmbSegment_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //            if (cmbSegment.SelectedItem.Value != "")
        //            {
        //                //-------Update tbl_master_user according to the segment selected------//
        //                oDBEngine.SetFieldValue("tbl_master_user", "user_lastsegement=" + cmbSegment.SelectedItem.Value, " user_id = '" + HttpContext.Current.Session["userid"] + "'");
        //                HttpContext.Current.Session["userlastsegment"] = cmbSegment.SelectedItem.Value;
        //                //-------End-----------------------------------------------------------//
        //                oDBEngine.PopulateMenu(Menumain, cmbSegment.SelectedItem.Value);

        //                string segmentname = cmbSegment.SelectedItem.Text.ToString();
        //                string[] sname = segmentname.Split('-');
        //                if (sname.Length > 1)
        //                {
        //                    string[] ExchangeSegmentID = oDBEngine.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='" + sname[1] + "'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
        //                    HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
        //                }
        //                else
        //                {
        //                    if (sname[0] == "Accounts")
        //                    {
        //                        string[] ExchangeSegmentID = oDBEngine.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='ACC'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
        //                        HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
        //                    }
        //                    else
        //                    {
        //                        HttpContext.Current.Session["ExchangeSegmentID"] = null;
        //                    }
        //                }

        //                string[,] data = new string[1, 1];
        //                if (cmbSegment.SelectedItem.Value.Trim() == "7" || cmbSegment.SelectedItem.Value.Trim() == "8" || cmbSegment.SelectedItem.Value.Trim() == "9" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "10" || cmbSegment.SelectedItem.Value.Trim() == "5" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "11" || cmbSegment.SelectedItem.Value.Trim() == "12" || cmbSegment.SelectedItem.Value.Trim() == "15" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "14" || cmbSegment.SelectedItem.Value.Trim() == "23" || cmbSegment.SelectedItem.Value.Trim() == "18" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "16" || cmbSegment.SelectedItem.Value.Trim() == "17" || cmbSegment.SelectedItem.Value.Trim() == "24" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "25" || cmbSegment.SelectedItem.Value.Trim() == "26" || cmbSegment.SelectedItem.Value.Trim() == "99" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "28" || cmbSegment.SelectedItem.Value.Trim() == "29" || cmbSegment.SelectedItem.Value.Trim() == "30" ||
        //                    cmbSegment.SelectedItem.Value.Trim() == "31" || cmbSegment.SelectedItem.Value.Trim() == "32" || cmbSegment.SelectedItem.Value.Trim() == "33" || cmbSegment.SelectedItem.Value.Trim() == "34" || cmbSegment.SelectedItem.Value.Trim() == "35" || cmbSegment.SelectedItem.Value.Trim() == "36" || cmbSegment.SelectedItem.Value.Trim() == "37" || cmbSegment.SelectedItem.Value.Trim() == "38" || cmbSegment.SelectedItem.Value.Trim() == "39" || cmbSegment.SelectedItem.Value.Trim() == "40")
        //                {
        //                    tblSegment.Visible = true;
        //                    if (cmbSegment.SelectedItem.Value.Trim() == "5" || cmbSegment.SelectedItem.Value.Trim() == "9" || cmbSegment.SelectedItem.Value.Trim() == "10" || cmbSegment.SelectedItem.Value.Trim() == "99")
        //                    {
        //                        SettBtnPart.Visible = false;
        //                        data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
        //                                            " ls_lastSettlementNo+ls_lastSettlementType as sett," +
        //                                            " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ," +
        //                                            " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ,ls_lastFinYear,ls_lastCompany,(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select FinYear_EndDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd",
        //                                            " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 8);
        //                    }
        //                    else
        //                    {
        //                        SettBtnPart.Visible = true;
        //                        data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
        //                                            " ls_lastSettlementNo+ls_lastSettlementType as sett," +
        //                                            " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") ," +
        //                                            " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType) and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") ,ls_lastFinYear,ls_lastCompany,(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select FinYear_EndDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd",
        //                                            " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 8);
        //                    }



        //                    string[,] segId = oDBEngine.GetFieldValue("tbl_trans_LastSegment", "ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo", "ls_lastSegment='" + cmbSegment.SelectedItem.Value + "' and ls_userId='" + HttpContext.Current.Session["userid"] + "'", 4);
        //                    if (segId[0, 0] != "n")
        //                    {
        //                        HttpContext.Current.Session["usersegid"] = segId[0, 0].ToString().Trim();
        //                        HttpContext.Current.Session["LastCompany"] = segId[0, 1].ToString();
        //                        HttpContext.Current.Session["LastFinYear"] = segId[0, 2].ToString();
        //                        HttpContext.Current.Session["LastSettNo"] = segId[0, 3].ToString();
        //                    }
        //                    //if (cmbSegment.SelectedItem.Value.Trim() == "5")
        //                    //    Session["usersegid"] = "0";


        //                    if (data[0, 0] != "n")
        //                    {
        //                        if (data[0, 1] != "")
        //                        {
        //                            lblSCompName.Text = data[0, 0];
        //                            if (cmbSegment.SelectedItem.Value.Trim() == "5")
        //                            {
        //                                lblSettNo.Text = "";
        //                            }
        //                            else
        //                            {
        //                                lblSettNo.Text = data[0, 1];
        //                            }

        //                            lblStartDate.Text = data[0, 2];
        //                            lblfundPayeeDate.Text = data[0, 3];
        //                            lblFinYear.Text = data[0, 4];
        //                            HttpContext.Current.Session["LastFinYear"] = data[0, 4];
        //                            HttpContext.Current.Session["LastSettNo"] = data[0, 1];
        //                            HttpContext.Current.Session["LastCompany"] = data[0, 5];
        //                        }
        //                        else
        //                        {
        //                            if (cmbSegment.SelectedItem.Value.Trim() == "5")
        //                            {
        //                                lblSettNo.Text = "";
        //                            }
        //                            else
        //                            {
        //                                lblSettNo.Text = segId[0, 0].ToString().Trim();
        //                            }
        //                            lblSCompName.Text = data[0, 0];
        //                            lblFinYear.Text = data[0, 4];
        //                            lblStartDate.Text = "";
        //                            lblfundPayeeDate.Text = "";
        //                        }
        //                        HttpContext.Current.Session["FinYearStart"] = data[0, 6];
        //                        HttpContext.Current.Session["FinYearEnd"] = data[0, 7];

        //                        ////////////////////////Entry Lock System Session Creation/////////////////////////////
        //                        string SegmentID = null;
        //                        DataTable DtLockEntrySystem = null;
        //                        if (cmbSegment.SelectedItem.Value.Trim() == "9" || cmbSegment.SelectedItem.Value.Trim() == "10")
        //                        {
        //                            DtLockEntrySystem = oDBEngine.GetDataTable("tbl_master_CompanyExchange", "Exch_InternalID", "Exch_CompID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Exch_TMCode='" + HttpContext.Current.Session["UserSegID"].ToString() + "'");
        //                            SegmentID = DtLockEntrySystem.Rows[0][0].ToString();
        //                            DtLockEntrySystem = null;

        //                        }
        //                        else
        //                        {
        //                            SegmentID = HttpContext.Current.Session["UserSegID"].ToString();
        //                        }

        //                        //======================================Expiry Module==========================
        //                        //Set Expiry From Encrypted File
        //                        /* For 3 tier structure 
        //                           oGenericMethod = new GenericMethod();
        //                        */
        //                        oGenericMethod = new BusinessLogicLayer.GenericMethod();
        //                        oGenericMethod.EncryptDecript(1, "SetExpiryDate", HttpContext.Current.Server.MapPath(@"../License.txt"));
        //                        //try
        //                        //{
        //                        Check_ExpiryStatus();
        //                        //}
        //                        //catch(Exception ex)
        //                        //{
        //                        //    throw new Exception("Unspecifed Error: Possible Error Due To Segment Not valid in License.");
        //                        //}
        //                        //======================================End Expiry Module==========================

        //                        // Variable For All "GS_LCKALL"
        //                        // 1.Session CashBank -->"GS_LCKBNK",
        //                        // 2.Session Demate -->"GS_LCKDEMAT",
        //                        // 3.Session Trade -->"GS_LCKTRADE",
        //                        // 4.Session JounalVoucher -->"GS_LCKJV",
        //                        DtLockEntrySystem = oDBEngine.GetDataTable("(Select GlobalSettings_Name," +
        //                                                                @"Case
        //                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
        //                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
        //                                                                End as LockDaysOrDate
        //                                                                From
        //                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKBNK'
        //                                                                union
        //                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
        //                                                            Union
        //                                                            Select 'GS_LCKDEMAT' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
        //                                                            Select GlobalSettings_Name," +
        //                                                                @"Case
        //                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
        //                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
        //                                                                End as LockDaysOrDate
        //                                                                From
        //                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKDEMAT'
        //                                                                union
        //                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
        //                                                            Union
        //                                                            Select 'GS_LCKTRADE' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
        //                                                            Select GlobalSettings_Name," +
        //                                                                @"Case
        //                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
        //                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
        //                                                                End as LockDaysOrDate
        //                                                                From
        //                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKTRADE'
        //                                                                union
        //                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
        //                                                            Union
        //                                                            Select 'GS_LCKJV' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
        //                                                            Select GlobalSettings_Name," +
        //                                                                @"Case
        //                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
        //                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
        //                                                                End as LockDaysOrDate
        //                                                                From
        //                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKJV'
        //                                                                union
        //                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
        //                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2", "'GS_LCKBNK' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate", null);

        //                        if (DtLockEntrySystem.Rows.Count > 0)
        //                        {
        //                            DataRow row;
        //                            DtLockEntrySystem.PrimaryKey = new DataColumn[] { DtLockEntrySystem.Columns["GlobalSettings_Name"] };
        //                            row = DtLockEntrySystem.Rows.Find("GS_LCKBNK");
        //                            if (row != null)
        //                            {
        //                                if (row[1].ToString() != string.Empty)
        //                                {
        //                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
        //                                        HttpContext.Current.Session["LCKBNK"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
        //                                    else
        //                                        HttpContext.Current.Session["LCKBNK"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["LCKBNK"] = null;
        //                                    row = null;
        //                                }

        //                            }
        //                            row = DtLockEntrySystem.Rows.Find("GS_LCKDEMAT");
        //                            if (row != null)
        //                            {
        //                                if (row[1].ToString() != string.Empty)
        //                                {
        //                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
        //                                        HttpContext.Current.Session["LCKDEMAT"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
        //                                    else
        //                                        HttpContext.Current.Session["LCKDEMAT"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["LCKDEMAT"] = null;
        //                                    row = null;
        //                                }
        //                            }
        //                            row = DtLockEntrySystem.Rows.Find("GS_LCKTRADE");
        //                            if (row != null)
        //                            {
        //                                if (row[1].ToString() != string.Empty)
        //                                {
        //                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
        //                                        HttpContext.Current.Session["LCKTRADE"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
        //                                    else
        //                                        HttpContext.Current.Session["LCKTRADE"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["LCKTRADE"] = null;
        //                                    row = null;
        //                                }
        //                            }
        //                            row = DtLockEntrySystem.Rows.Find("GS_LCKJV");
        //                            if (row != null)
        //                            {
        //                                if (row[1].ToString() != string.Empty)
        //                                {
        //                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
        //                                        HttpContext.Current.Session["LCKJV"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
        //                                    else
        //                                        HttpContext.Current.Session["LCKJV"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["LCKJV"] = null;
        //                                    row = null;
        //                                }

        //                            }
        //                        }
        //                        //End Session For CashBankEntry
        //                        //////////////////////End Entry Lock System Session Creation/////////////////////////////


        //                    }
        //                    else
        //                    {
        //                        lblSCompName.Text = "";
        //                        lblSettNo.Text = "";
        //                        lblStartDate.Text = "";
        //                        lblfundPayeeDate.Text = "";
        //                        lblFinYear.Text = "";
        //                        HttpContext.Current.Session["LastFinYear"] = "";
        //                        HttpContext.Current.Session["LastSettNo"] = "";
        //                        HttpContext.Current.Session["LastCompany"] = "";
        //                        HttpContext.Current.Session["ExpireDate"] = null;
        //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Segment", "OnSegmentChange('" + cmbSegment.SelectedItem.Value + "')", true);
        //                    }
        //                }
        //                else
        //                    tblSegment.Visible = false;


        //                BindFavouriteMenu();

        //            }
        //            //Page.ClientScript.RegisterStartupScript(GetType(), "pagecall1", "<script>CallPage('../management/testProjectMainPage_FirstPage.aspx');onpageload();</script>");



        //            lnkSelectCompanySettFinYear.HRef = "javascript:showpage('" + HttpContext.Current.Session["userlastsegment"].ToString() + "')";
        //            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall1", "<script>onpageload();</script>");

        //            HiddenField1.Value = "../management/frm_homePage.aspx";
        //            //HiddenField1.Value = "../management/welcome.aspx";
        //            btnActivity_Click(sender, e);

        //            //Set Currrency On Segment Change
        //            CurrencySetting();

        //            ReAssign_Session();

        //        }//settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " )
        //        protected void btnSettUP_Click(object sender, ImageClickEventArgs e)
        //        {
        //            string settNo = lblSettNo.Text.Substring(0, lblSettNo.Text.Trim().Length - 1);
        //            DataTable DT = oDBEngine.GetDataTable(" (select RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix) as sttno,settlements_Number from Master_Settlements where Settlements_ExchangeSegmentID =(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + " )) and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and cast(RTRIM(settlements_Number) as int)>cast('" + settNo + "' as int) and RTRIM(settlements_TypeSuffix) = '" + lblSettNo.Text.Substring(lblSettNo.Text.Trim().Length - 1, 1) + "' and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' ) as D", " top 1 *" +
        //                ",(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " ) as startDate,(select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ")  as EndDate",
        //                null,
        //                " cast(RTRIM(settlements_Number) as int)");
        //            if (DT.Rows.Count > 0)
        //            {
        //                lblSettNo.Text = DT.Rows[0]["sttno"].ToString();
        //                lblStartDate.Text = DT.Rows[0]["startDate"].ToString();
        //                lblfundPayeeDate.Text = DT.Rows[0]["EndDate"].ToString();
        //                HttpContext.Current.Session["LastSettNo"] = DT.Rows[0]["sttno"].ToString();
        //                UpdateLastSettTable(DT.Rows[0]["sttno"].ToString());
        //            }
        //            else
        //                ScriptManager.RegisterStartupScript(this, GetType(), "test", "alert('Settlement No. not found!')", true);

        //            ReAssign_Session();
        //        }
        //        protected void btnSettDown_Click(object sender, ImageClickEventArgs e)
        //        {
        //            string settNo = lblSettNo.Text.Substring(0, lblSettNo.Text.Trim().Length - 1);
        //            DataTable DT = oDBEngine.GetDataTable(" (select  RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix) as sttno,settlements_Number from Master_Settlements where Settlements_ExchangeSegmentID =(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + " )) and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and cast(RTRIM(settlements_Number) as int)<cast('" + settNo + "' as int) and RTRIM(settlements_TypeSuffix) = '" + lblSettNo.Text.Substring(lblSettNo.Text.Trim().Length - 1, 1) + "' and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "'  ) as D ", " top 1 *" +
        //                ",(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and settlements_ExchangeSegmentID=(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + " ))) as startDate,(select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ")  as EndDate",
        //                null,
        //                " settlements_Number desc");
        //            if (DT.Rows.Count > 0)
        //            {
        //                lblSettNo.Text = DT.Rows[0]["sttno"].ToString();
        //                lblStartDate.Text = DT.Rows[0]["startDate"].ToString();
        //                lblfundPayeeDate.Text = DT.Rows[0]["EndDate"].ToString();
        //                HttpContext.Current.Session["LastSettNo"] = DT.Rows[0]["sttno"].ToString();
        //                UpdateLastSettTable(DT.Rows[0]["sttno"].ToString());
        //            }
        //            else
        //                ScriptManager.RegisterStartupScript(this, GetType(), "test", "alert('Settlement No. not found!')", true);
        //            ReAssign_Session();
        //        }
        //        protected void btnsetNext_Click(object sender, ImageClickEventArgs e)
        //        {
        //            string settNo = lblSettNo.Text.Substring(0, lblSettNo.Text.Trim().Length - 1);
        //            //DataTable DT = oDBEngine.GetDataTable(" ( select top 1 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix) as sttno,settlements_TypeSuffix from Master_Settlements where  Settlements_ExchangeSegmentID =(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + " )) and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and cast(RTRIM(settlements_Number) as int)= cast('" + settNo + "' as int) and ascii(RTRIM(settlements_TypeSuffix)) > ascii('" + lblSettNo.Text.Substring(lblSettNo.Text.Trim().Length - 1, 1) + "')  and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' ) as D ", " *" +
        //            //    ",(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno) as startDate,(select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno)  as EndDate",
        //            //    null,
        //            //    " RTRIM(settlements_TypeSuffix) ");
        //            DataTable DT = oDBEngine.GetDataTable(" ( select top 1 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix) as sttno,settlements_TypeSuffix from Master_Settlements where  Settlements_ExchangeSegmentID =" + Session["ExchangeSegmentID"].ToString() + " and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and cast(RTRIM(settlements_Number) as int)= cast('" + settNo + "' as int) and ascii(RTRIM(settlements_TypeSuffix)) > ascii('" + lblSettNo.Text.Substring(lblSettNo.Text.Trim().Length - 1, 1) + "')  and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' ) as D ", " *" +
        //                ",(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") as startDate,(select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ")  as EndDate",
        //                null,
        //                " RTRIM(settlements_TypeSuffix) ");
        //            if (DT.Rows.Count > 0)
        //            {
        //                lblSettNo.Text = DT.Rows[0]["sttno"].ToString();
        //                lblStartDate.Text = DT.Rows[0]["startDate"].ToString();
        //                lblfundPayeeDate.Text = DT.Rows[0]["EndDate"].ToString();
        //                HttpContext.Current.Session["LastSettNo"] = DT.Rows[0]["sttno"].ToString();
        //                UpdateLastSettTable(DT.Rows[0]["sttno"].ToString());
        //            }
        //            else
        //            {
        //                DT = oDBEngine.GetDataTable(" (select top 1 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix) as sttno,settlements_TypeSuffix from  Master_Settlements where Settlements_ExchangeSegmentID =" + Session["ExchangeSegmentID"].ToString() + " and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and cast(RTRIM(settlements_Number) as int)= cast('" + settNo + "' as int)  and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "') as D ", " *" +
        //                    ",(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") as startDate,(select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=sttno and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ")  as EndDate",
        //                    null,
        //                    " RTRIM(settlements_TypeSuffix) ");
        //                if (DT.Rows.Count > 0)
        //                {
        //                    lblSettNo.Text = DT.Rows[0]["sttno"].ToString();
        //                    lblStartDate.Text = DT.Rows[0]["startDate"].ToString();
        //                    lblfundPayeeDate.Text = DT.Rows[0]["EndDate"].ToString();
        //                    HttpContext.Current.Session["LastSettNo"] = DT.Rows[0]["sttno"].ToString();
        //                    UpdateLastSettTable(DT.Rows[0]["sttno"].ToString());
        //                }
        //            }
        //            ReAssign_Session();
        //        }

        #endregion
        void ReAssign_Session()
        {
            //Create Session For StartdateFundsPayindateFundsPayinDate Where Settlement Change
            //This Session is Created Only At The Time OF Authentication User
            /* For 3 tier structure 
                oGenericMethod = new GenericMethod();
              */
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable UserLastSegmentInfo = oGenericMethod.GetUserLastSegmentDetail();
            if (UserLastSegmentInfo.Rows.Count > 0)
            {
                HttpContext.Current.Session["StartdateFundsPayindate"] = UserLastSegmentInfo.Rows[0][5].ToString();
            }
            // fetch startdate and FundsPayin from Master_Settlements
        }
        //protected void btnFinNext_Click(object sender, ImageClickEventArgs e)
        //{
        //    DataTable DT = oDBEngine.GetDataTable("   Master_FinYear ", " top 1  FinYear_Code,FinYear_ID", " FinYear_ID > (SELECT D.FinYear_ID from Master_FinYear D where  FinYear_Code='" + lblFinYear.Text.Trim() + "' )", " FinYear_ID ");
        //    if (DT.Rows.Count > 0)
        //    {
        //        lblFinYear.Text = DT.Rows[0]["FinYear_Code"].ToString();
        //        HttpContext.Current.Session["LastFinYear"] = DT.Rows[0]["FinYear_Code"].ToString();
        //    }
        //    else
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Fin1", "alert('Fin. Year not found!')", true);
        //}
        //protected void btnFinPrev_Click(object sender, ImageClickEventArgs e)
        //{
        //    DataTable DT = oDBEngine.GetDataTable("   Master_FinYear ", " top 1  FinYear_Code,FinYear_ID", " FinYear_ID < (SELECT D.FinYear_ID from Master_FinYear D where  FinYear_Code='" + lblFinYear.Text.Trim() + "' )", " FinYear_ID DESC ");
        //    if (DT.Rows.Count > 0)
        //    {
        //        lblFinYear.Text = DT.Rows[0]["FinYear_Code"].ToString();
        //        HttpContext.Current.Session["LastFinYear"] = DT.Rows[0]["FinYear_Code"].ToString();
        //    }
        //    else
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Fin1", "alert('Fin. Year not found!')", true);
        //}
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
        protected void BtnFavourite_Click(object sender, EventArgs e)
        {
            string LastActivity = "";//HiddenField1.Value;
            DataTable ChkFav = oDBEngine.GetDataTable("Config_FavouriteMenu", "FavouriteMenu_ID", " FavouriteMenu_UserID=" + Session["userid"].ToString() + " and FavouriteMenu_MenuID in(select mnu_id from tbl_trans_menu where mnu_menuLink='" + LastActivity + "' and mnu_segmentid=" + Session["userlastsegment"].ToString() + ")");
            if (ChkFav.Rows.Count == 0)
            {
                DataTable dtFav = oDBEngine.GetDataTable("tbl_trans_menu", "mnu_id", " mnu_menuLink='" + LastActivity + "' and mnu_segmentID=" + Session["userlastsegment"].ToString() + "");
                int NoofRows = oDBEngine.InsurtFieldValue("Config_FavouriteMenu", "FavouriteMenu_UserID,FavouriteMenu_MenuID,FavouriteMenu_Segment,FavouriteMenu_Order,FavouriteMenu_Image,FavouriteMenu_CreateUser,FavouriteMenu_CreateDateTime", "" + Session["userid"].ToString() + "," + dtFav.Rows[0][0].ToString() + "," + Session["userlastsegment"].ToString() + ",1,'../images/1.png'," + Session["userid"].ToString() + ",getdate()");
                if (NoofRows > 0)
                    ScriptManager.RegisterStartupScript(this, GetType(), "JSF", "alert('Added Successfully !!')", true);
                dtFav.Dispose();
            }
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "JSF", "alert('You Already Added This Page !!')", true);
        }

        //If There is No LastSegment In LastSegment Table of Specific User 
        //He/She Will Get PopUp To Choose Settlement Or Open and Then Choose
        //Settlement. At That Time Authentication Method  is not able
        //Initialize Some Session and Lock Dates So That This Method Used
        void CreateSession_LCK_When_NewlyOpenSettlement()
        {
            /* For 3 tier structure 
                oGenericMethod = new GenericMethod();
              */
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable UserLastSegmentInfo = oGenericMethod.GetUserLastSegmentDetail();
            if (UserLastSegmentInfo.Rows.Count > 0)
            {
                HttpContext.Current.Session["usersegid"] = UserLastSegmentInfo.Rows[0][0].ToString().Trim();
                HttpContext.Current.Session["LastCompany"] = UserLastSegmentInfo.Rows[0][1].ToString();
                HttpContext.Current.Session["LastFinYear"] = UserLastSegmentInfo.Rows[0][2].ToString();
                HttpContext.Current.Session["LastSettNo"] = UserLastSegmentInfo.Rows[0][3].ToString();
                HttpContext.Current.Session["StartdateFundsPayindate"] = UserLastSegmentInfo.Rows[0][5].ToString();// fetch startdate and FundsPayin from Master_Settlements
                ////////////////////////Entry Lock System Session Creation/////////////////////////////
                string SegmentID = null;
                DataTable DtLockEntrySystem = null;
                string UserLastSegment = HttpContext.Current.Session["userlastsegment"].ToString();
                if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6")
                {
                    if (UserLastSegment == "9" || UserLastSegment == "10")
                    {
                        DtLockEntrySystem = oDBEngine.GetDataTable("tbl_master_CompanyExchange", "Exch_InternalID", "Exch_CompID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Exch_TMCode='" + HttpContext.Current.Session["UserSegID"].ToString() + "'");
                        SegmentID = DtLockEntrySystem.Rows[0][0].ToString();
                        DtLockEntrySystem = null;

                    }
                    else
                    {
                        SegmentID = HttpContext.Current.Session["UserSegID"].ToString();
                    }

                    //======================================Expiry Module==========================
                    //Set Expiry From Encrypted File
                    /* For 3 tier structure 
                       oGenericMethod = new GenericMethod();
                     */
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();

                    //oGenericMethod.EncryptDecript(1, "SetExpiryDate", HttpContext.Current.Server.MapPath(@"../License.txt"));
                    oGenericMethod.EncryptDecript(1, "SetExpiryDate", System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
                    Check_ExpiryStatus();
                    //======================================End Expiry Module==========================

                    // Variable For All "GS_LCKALL"
                    // 1.Session CashBank -->"GS_LCKBNK",
                    // 2.Session Demate -->"GS_LCKDEMAT",
                    // 3.Session Trade -->"GS_LCKTRADE",
                    // 4.Session JounalVoucher -->"GS_LCKJV",
                    DtLockEntrySystem = oDBEngine.GetDataTable("(Select GlobalSettings_Name," +
                                                            @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKBNK'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKDEMAT' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                            @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKDEMAT'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKTRADE' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                            @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKTRADE'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKJV' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                            @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKJV'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2", "'GS_LCKBNK' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate", null);

                    if (DtLockEntrySystem.Rows.Count > 0)
                    {
                        DataRow row;
                        DtLockEntrySystem.PrimaryKey = new DataColumn[] { DtLockEntrySystem.Columns["GlobalSettings_Name"] };
                        row = DtLockEntrySystem.Rows.Find("GS_LCKBNK");
                        if (row != null)
                        {
                            if (row[1].ToString() != string.Empty)
                            {
                                if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                    HttpContext.Current.Session["LCKBNK"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                else
                                    HttpContext.Current.Session["LCKBNK"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                            }
                            else
                            {
                                HttpContext.Current.Session["LCKBNK"] = null;
                                row = null;
                            }

                        }
                        row = DtLockEntrySystem.Rows.Find("GS_LCKDEMAT");
                        if (row != null)
                        {
                            if (row[1].ToString() != string.Empty)
                            {
                                if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                    HttpContext.Current.Session["LCKDEMAT"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                else
                                    HttpContext.Current.Session["LCKDEMAT"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                            }
                            else
                            {
                                HttpContext.Current.Session["LCKDEMAT"] = null;
                                row = null;
                            }
                        }
                        row = DtLockEntrySystem.Rows.Find("GS_LCKTRADE");
                        if (row != null)
                        {
                            if (row[1].ToString() != string.Empty)
                            {
                                if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                    HttpContext.Current.Session["LCKTRADE"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                else
                                    HttpContext.Current.Session["LCKTRADE"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                            }
                            else
                            {
                                HttpContext.Current.Session["LCKTRADE"] = null;
                                row = null;
                            }
                        }
                        row = DtLockEntrySystem.Rows.Find("GS_LCKJV");
                        if (row != null)
                        {
                            if (row[1].ToString() != string.Empty)
                            {
                                if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                    HttpContext.Current.Session["LCKJV"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                else
                                    HttpContext.Current.Session["LCKJV"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                            }
                            else
                            {
                                HttpContext.Current.Session["LCKJV"] = null;
                                row = null;
                            }

                        }
                    }
                }
                //End Session For CashBankEntry
                //////////////////////End Entry Lock System Session Creation/////////////////////////////

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

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);
        protected string Getmaccccaddress22()
        {
            try
            {
                string userip = Request.UserHostAddress;
                string strClientIP = Request.UserHostAddress.ToString().Trim();
                Int32 ldest = inet_addr(strClientIP);
                Int32 lhost = inet_addr("");
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                string mac_src = macinfo.ToString("X");
                if (mac_src == "0")
                {
                    if (userip == "127.0.0.1")
                        //  Response.Write("visited Localhost!");
                        //  else

                        //    Response.Write("the IP from" + userip + "" + "");
                        return userip;
                }

                while (mac_src.Length < 12)
                {
                    mac_src = mac_src.Insert(0, "0");
                }

                string mac_dest = "";

                for (int i = 0; i < 11; i++)
                {
                    if (0 == (i % 2))
                    {
                        if (i == 10)
                        {
                            mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                        else
                        {
                            mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                    }
                }

                //  Response.Write("welcome" + userip + "" + ",the mac address is" + mac_dest + "." + "");


                return mac_dest;


            }
            catch (Exception err)
            {
                //  Response.Write(err.Message);

                return err.Message;
            }
        }


        //Currency Setting
        //protected void CbpChoosenCurrency_Callback(object source, CallbackEventArgsBase e)
        //{
        //   // CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = null;

        //    string WhichCall = e.Parameter.Split('~')[0];
        //    if (WhichCall == "ChangeCurrency")
        //    {
        //        string ActiveCurrencyID = String.Empty;
        //        string ActiveCurrencyName = String.Empty;
        //        string ActiveCurrencySymbol = String.Empty;
        //        string CurrencyInXmlFile = String.Empty;
        //        string ActiveCurrency = Session["ActiveCurrency"].ToString();
        //        string TradeCurrency = Session["TradeCurrency"].ToString();
        //        string LocalCurrency = Session["LocalCurrency"].ToString();

        //Change ChoosenCurrency In XmlFile if Exists
        //if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        //{
        //    DataSet DsChangeChooseCurrency = new DataSet();
        //    DsChangeChooseCurrency.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        //    CurrencyInXmlFile = DsChangeChooseCurrency.Tables[0].Rows[0]["ChoosenCurrency"].ToString();
        //    if (CurrencyInXmlFile != ActiveCurrency)
        //    {
        //        if (ActiveCurrency == TradeCurrency)
        //        {
        //            ActiveCurrencyID = TradeCurrency.Split('~')[0];
        //            ActiveCurrencyName = TradeCurrency.Split('~')[1];
        //            ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
        //            Session["ActiveCurrency"] = TradeCurrency;
        //        }
        //        else
        //        {
        //            ActiveCurrencyID = LocalCurrency.Split('~')[0];
        //            ActiveCurrencyName = LocalCurrency.Split('~')[1];
        //            ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
        //            Session["ActiveCurrency"] = LocalCurrency;
        //        }
        //        for (int i = 0; i < DsChangeChooseCurrency.Tables[0].Rows.Count; i++)
        //        {
        //            DsChangeChooseCurrency.Tables[0].Rows[i]["ChoosenCurrency"] = Session["ActiveCurrency"].ToString();
        //        }
        //        DsChangeChooseCurrency.AcceptChanges();
        //        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
        //        DsChangeChooseCurrency.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        //        DsChangeChooseCurrency.Dispose();
        //        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
        //    }
        //    else
        //    {
        //        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = CurrencyInXmlFile.Split('~')[1] + '~' + CurrencyInXmlFile.Split('~')[2];

        //    }
        //}
        //else
        //{
        //    if (ActiveCurrency == TradeCurrency)
        //    {
        //        ActiveCurrencyID = TradeCurrency.Split('~')[0];
        //        ActiveCurrencyName = TradeCurrency.Split('~')[1];
        //        ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
        //        Session["ActiveCurrency"] = TradeCurrency;
        //    }
        //    else
        //    {
        //        ActiveCurrencyID = LocalCurrency.Split('~')[0];
        //        ActiveCurrencyName = LocalCurrency.Split('~')[1];
        //        ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
        //        Session["ActiveCurrency"] = LocalCurrency;
        //    }
        //    CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
        //        //}
        //    }
        //}
        //protected void Cbp_Currency_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    //Set Js Variable Null On Calling
        //    //Cbp_Currency.JSProperties["cpCurrencyChanged"] = null;

        //    string[] strJsParam = e.Parameter.Split('~');
        //    string strWhichCall = strJsParam[0];
        //    string strLocalCurrencyName = String.Empty;
        //    string strLocalCurrencySymbol = String.Empty;
        //    string strTradeCurrencyName = String.Empty;
        //    string strTradeCurrencySymbol = String.Empty;

        //    if (Session["TradeCurrency"] != null && Session["LocalCurrency"] != null)
        //    {
        //        strLocalCurrencyName = Session["LocalCurrency"].ToString().Split('~')[1];
        //        strLocalCurrencySymbol = Session["LocalCurrency"].ToString().Split('~')[2];
        //        strTradeCurrencyName = Session["TradeCurrency"].ToString().Split('~')[1];
        //        strTradeCurrencySymbol = Session["TradeCurrency"].ToString().Split('~')[2];
        //    }
        //    else
        //        return;

        //    if (strWhichCall == "ChangeToLocalCurrency")
        //    {
        //        if (strLocalCurrencyName.Trim() != String.Empty && strLocalCurrencySymbol.Trim() != String.Empty)
        //        {
        //           // lblCurrencyNameSymbol.Text = strLocalCurrencyName.Trim() + " [" + strLocalCurrencySymbol.Trim() + "]";
        //            Session["ActiveCurrency"] = Session["LocalCurrency"];
        //            //Cbp_Currency.JSProperties["cpCurrencyChanged"] = "Trade~" + strTradeCurrencyName.Trim() + " [" + strTradeCurrencySymbol.Trim() + "]";
        //        }

        //    }
        //    else if (strWhichCall == "ChangeToTradeCurrency")
        //    {
        //        if (strTradeCurrencyName.Trim() != String.Empty && strTradeCurrencySymbol.Trim() != String.Empty)
        //        {
        //            //lblCurrencyNameSymbol.Text = strTradeCurrencyName.Trim() + " [" + strTradeCurrencySymbol.Trim() + "]";
        //            Session["ActiveCurrency"] = Session["TradeCurrency"];
        //          //  Cbp_Currency.JSProperties["cpCurrencyChanged"] = "Local~" + strLocalCurrencyName.Trim() + " [" + strLocalCurrencySymbol.Trim() + "]"; ;
        //        }

        //    }
        //}


        #region Code Commented By sam


        string CurrencySetting()
        {

            //Session Creation For Currency Identification For A Exchange And A Local Company (Created On '2012-06-15')
            //lblCurrencyNameSymbol.Text = String.Empty;
            //AChangeCurrency.InnerHtml = String.Empty;

            if (Session["LastCompany"] != null && Session["UserSegID"] != null)
            {
                /* For 3 tier
                oDBEngine = new DBEngine("");
                 */
                oDBEngine = new BusinessLogicLayer.DBEngine("");

                DataTable DTCurrency = new DataTable();
                DataTable DTCompanyDetail = new DataTable();
                //DTCompanyDetail = oDBEngine.GetCompanyDetail(Session["LastCompany"].ToString(), Session["UserSegID"].ToString());
                DTCompanyDetail = oDBEngine.GetCompanyDetailInformation(Session["LastCompany"].ToString());
                if (DTCompanyDetail.Rows.Count > 0)
                {
                    //if (DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "NSDL" && DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "CDSL")
                    //{
                    if (Session["ExchangeSegmentID"] != null)
                    {
                        if (Session["LastCompany"].ToString() != "n" && Session["ExchangeSegmentID"].ToString() != "n" && DTCompanyDetail.Rows.Count > 0)
                        {
                            DTCurrency = oDBEngine.GetDataTable("tbl_Master_Company,Master_Currency", @"Cast(Currency_ID as Varchar)+'~'+
                        Ltrim(Rtrim(Currency_AlphaCode))+'~'+Ltrim(Rtrim(Replace(Currency_Symbol,Char(160),''))) as TradeCurrency 
                        from Master_ExchangeSegments,Master_Currency Where Currency_ID=ExchangeSegment_TradeCurrencyID and ExchangeSegment_ID=" +
                                    Session["ExchangeSegmentID"].ToString() +
                                            @"Union ALL
                        Select Cast(Currency_ID as Varchar)+'~'+Ltrim(Rtrim(Currency_AlphaCode))+'~'+
                        Ltrim(Rtrim(Replace(Currency_Symbol,Char(160),''))) as LocalCurrency", @"Currency_ID=Cmp_CurrencyID
                        and cmp_InternalID='" + Session["LastCompany"].ToString() + "'");


                            if (DTCurrency.Rows.Count > 1)
                            {
                                Session["TradeCurrency"] = DTCurrency.Rows[0][0].ToString();
                                Session["LocalCurrency"] = DTCurrency.Rows[1][0].ToString();
                                //First Time Active Currency Will Be Trade Currency ByDefault....
                                Session["ActiveCurrency"] = DTCurrency.Rows[0][0].ToString();
                                if (Session["TradeCurrency"].ToString() != Session["LocalCurrency"].ToString())
                                {
                                    //lblCurrencyNameSymbol.Text = DTCurrency.Rows[0][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[0][0].ToString().Split('~')[2].Trim() + "]";
                                    //AChangeCurrency.InnerHtml = "Switch To " + DTCurrency.Rows[1][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[1][0].ToString().Split('~')[2].Trim() + "]";
                                }
                                else
                                {
                                    //lblCurrencyNameSymbol.Text = DTCurrency.Rows[0][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[0][0].ToString().Split('~')[2].Trim() + "]";
                                    //AChangeCurrency.InnerHtml = String.Empty;
                                }
                            }
                            DTCurrency.Dispose();
                            DTCompanyDetail.Dispose();
                        }
                        else
                            CurrencySetting_Null();
                        // }
                        //else
                        //{
                        //    if (DTCompanyDetail.Rows[0]["Exh_ShortName"].ToString() != "Accounts") CurrencySetting_NonExchangeSegment();
                        //    else CurrencySetting_Null();

                        //}
                    }
                    else
                    {
                        CurrencySetting_NonExchangeSegment();
                    }
                }
                else
                {
                    CurrencySetting_Null();
                }
            }
            else
            {
                CurrencySetting_Null();
            }
            return String.Empty;
        }
        #endregion Code Commented By sam
        void CurrencySetting_NonExchangeSegment()
        {
            DataTable DTCurrency = new DataTable();
            DTCurrency = oDBEngine.GetDataTable("Tbl_Master_Company,Master_Currency", @"Cast(Currency_ID as Varchar)+'~'+Ltrim(Rtrim(Currency_AlphaCode))+'~'+
                Ltrim(Rtrim(Replace(Currency_Symbol,Char(160),''))) as LocalCurrency", @"Currency_ID=Cmp_CurrencyID
                and cmp_InternalID='" + Session["LastCompany"].ToString() + "'");

            if (DTCurrency.Rows.Count > 0)
            {
                Session["TradeCurrency"] = DTCurrency.Rows[0][0].ToString();
                Session["LocalCurrency"] = DTCurrency.Rows[0][0].ToString();
                Session["ActiveCurrency"] = DTCurrency.Rows[0][0].ToString();

                //lblCurrencyNameSymbol.Text = DTCurrency.Rows[0][0].ToString().Split('~')[1].Trim() + " [" + DTCurrency.Rows[0][0].ToString().Split('~')[2].Trim() + "]";
                //AChangeCurrency.InnerHtml = String.Empty;

            }
            else
            {
                CurrencySetting_Null();
            }
            DTCurrency.Dispose();
        }
        void CurrencySetting_Null()
        {
            Session["TradeCurrency"] = null;
            Session["LocalCurrency"] = null;
            Session["ActiveCurrency"] = null;
            //For JS Purpose To Set Name and Symbol
            //hdn_ActiveCurrency.Value = string.Empty;
            //lblCurrencyNameSymbol.Text = String.Empty;
            //AChangeCurrency.InnerHtml = String.Empty;
        }

        protected void Check_ExpiryStatus()
        {
            //=====Start Check Global Expiry Date Before 7 days===================
            /* For 3 tier structure 
                oGenericMethod = new GenericMethod();
              */
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            //System.AppDomain.CurrentDomain.BaseDirectory + "License.txt"
            string Licns_GlobalExpiry = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//GlobalExpiry~", System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
            // string Licns_GlobalExpiry = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//GlobalExpiry~", HttpContext.Current.Server.MapPath(@"../License.txt"));
            string CurrentSegmentName = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1)[0];
            if (Licns_GlobalExpiry.Length > 0)
            {
                if (!Licns_GlobalExpiry.Contains("Corrupted"))
                {
                    if (Convert.ToDateTime(Licns_GlobalExpiry) > oDBEngine.GetDate())
                    {
                        TimeSpan oGlobalTimeSpan = Convert.ToDateTime(Licns_GlobalExpiry) - oDBEngine.GetDate();
                        double GlobalExpireDayDiff = oGlobalTimeSpan.TotalDays;

                        int IsActiveGlobalExpiry = 0;
                        DataTable dtExpiry_GSetting = oDBEngine.GetDataTable("Select GlobalSettings_Value GlobalExpiryValue from Config_GlobalSettings where GlobalSettings_Name='GS_GEXPDT' and GlobalSettings_SegmentID=0");
                        if (dtExpiry_GSetting != null)
                            if (dtExpiry_GSetting.Rows.Count > 0)
                                IsActiveGlobalExpiry = Convert.ToInt32(dtExpiry_GSetting.Rows[0]["GlobalExpiryValue"].ToString());
                        if ((GlobalExpireDayDiff <= 7) && (IsActiveGlobalExpiry == 1))
                        {
                            int globalDayDiff = Convert.ToDateTime(Licns_GlobalExpiry).Day - oDBEngine.GetDate().Day;
                            Page.ClientScript.RegisterStartupScript(GetType(), "GlobalExpire1", "<script>fn_ExpiryOverlay('Your Application Will Expired In <b>" + globalDayDiff.ToString() + "</b> Day(s).<br/>Please Upgrade Your License!!!');</script>");
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Corrupted1", "<script>fn_ExpiryOverlay('" + Licns_GlobalExpiry + "');</script>");
                }
            }
            else if (CurrentSegmentName == "HR")
                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
            else if (CurrentSegmentName.ToUpper() == "ACCOUNTS")
                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
            else if (CurrentSegmentName == "CRM")
                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
            else if (CurrentSegmentName == "Root")
                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
            else
            {
                //Licns_GlobalExpiry = "2300-12-31";
                //string Licns_ExpiryByCompSeg = null;
                //string CurrentCompanyName = null;
                //string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                //DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                //CurrentCompanyName = dtcmp.Rows[0]["cmp_Name"].ToString();
                //dtcmp = null;

                ////if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                ////    CurrentCompanyName = oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1].ToString();

                Licns_GlobalExpiry = "2300-12-31";
                string Licns_ExpiryByCompSeg = null;
                string CurrentCompanyName = null;

                if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                    CurrentCompanyName = oDBEngine.GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1].ToString();
                else
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    CurrentCompanyName = dtcmp.Rows[0]["cmp_Name"].ToString();
                    dtcmp = null;
                }

                /* For 3 tier structure 
                oGenericMethod = new GenericMethod();
               */
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                if (CurrentSegmentName == "HR")
                    Licns_ExpiryByCompSeg = "2300-12-31";
                else if (CurrentSegmentName == "CRM")
                    Licns_ExpiryByCompSeg = "2300-12-31";
                else if (CurrentSegmentName == "root")
                    Licns_ExpiryByCompSeg = "2300-12-31";
                else
                {
                    Licns_ExpiryByCompSeg = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//cName[@Value='" + CurrentCompanyName + "']/@Value,//Segments[@Value='" + CurrentSegmentName + "']/@Value,//cName[@Value='" + CurrentCompanyName + "']/Segments[@Value='" + CurrentSegmentName + "']/Expiry~" + CurrentCompanyName + "," + CurrentSegmentName, System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
                    // Licns_ExpiryByCompSeg = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//cName[@Value='" + CurrentCompanyName + "']/@Value,//Segments[@Value='" + CurrentSegmentName + "']/@Value,//cName[@Value='" + CurrentCompanyName + "']/Segments[@Value='" + CurrentSegmentName + "']/Expiry~" + CurrentCompanyName + "," + CurrentSegmentName, HttpContext.Current.Server.MapPath(@"../License.txt"));
                }

                if (Licns_ExpiryByCompSeg.Length > 0)
                {
                    if (!Licns_ExpiryByCompSeg.Contains("Corrupted"))
                    {
                        if (!Licns_ExpiryByCompSeg.Contains("Invalid"))
                        {
                            if (Convert.ToDateTime(Licns_ExpiryByCompSeg) > oDBEngine.GetDate())
                            {
                                TimeSpan oSegmentTimeSpan = Convert.ToDateTime(Licns_ExpiryByCompSeg) - oDBEngine.GetDate();
                                double SegExpireDayDiff = oSegmentTimeSpan.TotalDays;
                                if (SegExpireDayDiff <= 7)
                                {
                                    int segWiseDayDiff = Convert.ToDateTime(Licns_ExpiryByCompSeg).Day - oDBEngine.GetDate().Day;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "SegmentExpirable1", "<script language='javascript'>fn_ExpiryOverlay('Your <b>" + CurrentSegmentName + "</b> Segment Of This Application Will Expire in <b>" + segWiseDayDiff.ToString() + "</b> Day(s).<br/>Please Upgrade Your License!!!');</script>");
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "SegmentExpired1", "<script language='javascript'>fn_ExpiryOverlay('Your <b>" + CurrentSegmentName + "</b> Segment Of This Application Was Already Expired on <b>" + Licns_ExpiryByCompSeg + "</b> Date.<br/>Some Features Are Blocked.<br/>Please Upgrade Your License!!!');</script>");
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "Invalid1", "<script language='javascript'>fn_ExpiryOverlay('" + Licns_ExpiryByCompSeg + "');</script>");
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "Corrupted2", "<script>fn_ExpiryOverlay('" + Licns_ExpiryByCompSeg + "');</script>");
                    }
                }
            }
            //=====End Check Global Expiry Date Before 7 days===================
        }


        #region Sam Section on 25092017 for new modification
        protected void SetCompanyLogo(DataTable companyimgDt)
        {
            if (Session["CompanyBigLogoUrl"] == null || Session["CompanySmallLogoUrl"] == null)
            {
                bigLogo.ImageUrl = "/assests/images/logo.png";
                miniLogo.ImageUrl = "/assests/images/logo-mini.png";

                bigLogo.ImageAlign = ImageAlign.Middle;
                miniLogo.ImageAlign = ImageAlign.Middle;

                if (!string.IsNullOrEmpty(Convert.ToString(companyimgDt.Rows[0]["cmp_bigLogo"])))
                {
                    if (File.Exists(Server.MapPath(Convert.ToString(companyimgDt.Rows[0]["cmp_bigLogo"]))))
                    {
                        bigLogo.ImageUrl = Convert.ToString(companyimgDt.Rows[0]["cmp_bigLogo"]);
                        Session["CompanyBigLogoUrl"] = Convert.ToString(companyimgDt.Rows[0]["cmp_bigLogo"]);
                    }
                }
                if (!string.IsNullOrEmpty(Convert.ToString(companyimgDt.Rows[0]["cmp_smallLogo"])))
                {
                    if (File.Exists(Server.MapPath(Convert.ToString(companyimgDt.Rows[0]["cmp_smallLogo"]))))
                    {
                        miniLogo.ImageUrl = Convert.ToString(companyimgDt.Rows[0]["cmp_smallLogo"]);
                        Session["CompanySmallLogoUrl"] = Convert.ToString(companyimgDt.Rows[0]["cmp_smallLogo"]);
                    }
                }
            }
            else
            {
                bigLogo.ImageUrl = Convert.ToString(Session["CompanyBigLogoUrl"]);
                miniLogo.ImageUrl = Convert.ToString(Session["CompanySmallLogoUrl"]);
            }
            //Set height
            //cmp_bigLogo, cmp_smallLogo

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


        #endregion Sam Section on 25092017 for new modification
        protected void SetCompanyLogo()
        {
            bigLogo.ImageUrl = "/assests/images/logo.png";
            miniLogo.ImageUrl = "/assests/images/logo-mini.png";

            bigLogo.ImageAlign = ImageAlign.Middle;
            miniLogo.ImageAlign = ImageAlign.Middle;
            //Set height


            string[] logo = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo, cmp_smallLogo", "cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "'", 2);
            if (logo.Length > 0)
            {
                if (logo[0] != null && logo[0] != "")
                {

                    if (File.Exists(Server.MapPath(logo[0])))
                    {
                        bigLogo.ImageUrl = logo[0];
                    }

                }

                if (logo[1] != null && logo[1] != "")
                {
                    if (File.Exists(Server.MapPath(logo[1])))
                    {
                        miniLogo.ImageUrl = logo[1];
                    }
                }
            }
        }

    }
}






