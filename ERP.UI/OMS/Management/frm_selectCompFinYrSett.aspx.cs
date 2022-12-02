using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management{
    public partial class management_frm_selectCompFinYrSett : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        Management_BL oManagement_BL = new Management_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList clsdropdown = new clsDropDownList();
        public string SegmentId = "1";
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

          
            if (!IsPostBack)
            {
                if (Session["userlastsegment"].ToString() == "1" || Session["userlastsegment"].ToString() == "4" || Session["userlastsegment"].ToString() == "6" || Session["userlastsegment"].ToString() == "5" || Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "ButtonVisible", "<script language='javascript'> PageLoadForButtonV();</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "ButtonVisible", "<script language='javascript'> PageLoadForButton();</script>");
                }

                DataTable dtUCmp = oDBEngine.GetDataTable("Master_UserCompany", "*", "UserCompany_UserID='" + Session["userid"].ToString() + "'");
                if (dtUCmp.Rows.Count > 0)
                {

                    string ID = SegmentId; //SegmentId     //Request.QueryString["id"];
                    string[,] data = oDBEngine.GetFieldValue(" Master_FinYear ", " FinYear_Code as ID,FinYear_Code as Name ", null, 2, " FinYear_Code desc ");
                    if (data[0, 0] != "n")
                        clsdropdown.AddDataToDropDownList(data, cmbFinYear);



                    //Added by Jitendra
                    if (SegmentId == "1")
                    {
                        data = oDBEngine.GetFieldValue(" tbl_master_company", "  cmp_internalId,cmp_name", "cmp_internalid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "')", 2);
                        if (data[0, 0] != "n")
                        {
                            clsdropdown.AddDataToDropDownList(data, cmbCompany);
                        }
                    }



                    //if (ID == "9" || ID == "10")
                    //{
                    //    data = oDBEngine.GetFieldValue(" tbl_master_company tmc,tbl_master_companyExchange tmd,tbl_master_segment tms", " exch_compId, cmp_name+' '+'['+exch_TMCode+']' ", " (tmc.cmp_internalid=tmd.exch_compId and tms.seg_name=tmd.exch_membershipType and tms.seg_id=" + ID + ")  and exch_compid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "') ", 2);
                    //    if (data[0, 0] != "n")
                    //        clsdropdown.AddDataToDropDownList(data, cmbCompany);

                    //}
                    //else if (ID == "5")
                    //{
                    //    data = oDBEngine.GetFieldValue(" tbl_master_companyExchange", "  exch_compid ,(select cmp_Name from tbl_master_company where cmp_internalId=exch_compid) as name", " (select LTRIM(RTRIM(E.exh_shortName)) from tbl_master_exchange E where E.exh_cntId=tbl_master_companyExchange.exch_exchId)= (select LTRIM(RTRIM(seg_name)) from tbl_master_segment where seg_id=" + ID + ")  and exch_compid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "') ", 2);
                    //    if (data[0, 0] != "n")
                    //        clsdropdown.AddDataToDropDownList(data, cmbCompany);
                    //    setid.Style["display"] = "none";

                    //}
                    //else if (ID == "5" || ID == "99")
                    //{
                    //    data = oDBEngine.GetFieldValue(" tbl_master_company", "  cmp_internalId,cmp_name", "cmp_internalid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "')", 2);
                    //    if (data[0, 0] != "n")
                    //        oDBEngine.AddDataToDropDownList(data, cmbCompany);
                    //    setid.Style["display"] = "none";
                    //}
                    //else if (ID == "3")
                    //{
                    //    data = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalid,cmp_Name", " cmp_internalid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "')", 2);
                    //    if (data[0, 0] != "n")
                    //    {
                    //        clsdropdown.AddDataToDropDownList(data, cmbCompany);
                    //        if (HttpContext.Current.Session["LastCompany"] != null)
                    //            cmbCompany.SelectedValue = HttpContext.Current.Session["LastCompany"].ToString();
                    //        if (HttpContext.Current.Session["LastFinYear"] != null)
                    //            cmbFinYear.SelectedValue = HttpContext.Current.Session["LastFinYear"].ToString();
                    //    }
                    //    Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'> visibility();</script>");

                    //}
                    //else
                    //{
                    //data = oDBEngine.GetFieldValue(" tbl_master_companyExchange", "  exch_compid ,(select cmp_Name from tbl_master_company where cmp_internalId=exch_compid) as name", " ((select LTRIM(RTRIM(E.exh_shortName)) from tbl_master_exchange E where E.exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+LTRIM(RTRIM(exch_segmentId)))= (select LTRIM(RTRIM(seg_name)) from tbl_master_segment where seg_id=" + ID + ")  and exch_compid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Session["userid"].ToString() + "') ", 2);
                    //if (data[0, 0] != "n")
                    //    clsdropdown.AddDataToDropDownList(data, cmbCompany);

                    //txtSettNo.Attributes.Add("onkeyup", "showlist(this,'settNo',event," + ID + ")");
                    // }
                    if (HttpContext.Current.Session["LastCompany"] != null)
                        cmbCompany.SelectedValue = HttpContext.Current.Session["LastCompany"].ToString();
                    if (HttpContext.Current.Session["LastFinYear"] != null)
                        cmbFinYear.SelectedValue = HttpContext.Current.Session["LastFinYear"].ToString();
                }
                else
                {
                   // string ID = Request.QueryString["id"];
                    string[,] data = oDBEngine.GetFieldValue(" Master_FinYear ", " FinYear_Code as ID,FinYear_Code as Name ", null, 2, " FinYear_Code desc ");
                    if (data[0, 0] != "n")
                        clsdropdown.AddDataToDropDownList(data, cmbFinYear);


                    if (SegmentId == "1")
                    {
                        data = oDBEngine.GetFieldValue(" tbl_master_company", "  cmp_internalId,cmp_name", null, 2);
                        if (data[0, 0] != "n")
                            clsdropdown.AddDataToDropDownList(data, cmbCompany);
                        setid.Style["display"] = "none";
                    }
                   
                    if (HttpContext.Current.Session["LastCompany"] != null)
                        cmbCompany.SelectedValue = HttpContext.Current.Session["LastCompany"].ToString();
                    if (HttpContext.Current.Session["LastFinYear"] != null)
                        cmbFinYear.SelectedValue = HttpContext.Current.Session["LastFinYear"].ToString();

                }
                DBEngine objDBEngine = new DBEngine();
                string parentcompany = "'"+HttpContext.Current.Session["LastCompany"].ToString()+"'";
                // Code Added By Sam to get Child Company Detail after Compant Change Section Start
                //HttpContext.Current.Session["userCompanyHierarchy"] = objDBEngine.GetAllCompanyInHierarchy(Convert.ToString(HttpContext.Current.Session["LastCompany"] ));
                objDBEngine.GetChildCompany(parentcompany);

                // Code Added By Sam to get Child Company Detail after Compant Change Section End

                //_____For performing operation without refreshing page___//
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //___________-end here___//
            }

            //ExchangeSegmentID Session is Created Forcely For Showing Open Settlement Pop

            //string[] sname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id='" + Request.QueryString["id"] + "'", 1);
          string[] sname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id='" + SegmentId + "'", 1);
            if (sname[0] == "Accounts")
            {
                string[] ExchangeSegmentID = oDBEngine.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='ACC'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
            }
            else
            {
//                    string[,] ExchangeSegmentID = oDBEngine.GetFieldValue(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID and exch_compId='" + cmbCompany.SelectedValue + @"'
//                    and exh_shortName+'-'+exch_segmentId=(Select Seg_Name CompName from tbl_master_segment Where seg_id='" + Request.QueryString["id"] + @"')) as T1,Master_Exchange
//                    Where Exchange_ShortName=Exh_ShortName", "(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID  and ExchangeSegment_Code=Exch_SegmentID) as ExchangeSegmentID", null, 1);
//                    Session["ExchangeSegmentID"] = ExchangeSegmentID[0, 0];
                string[,] ExchangeSegmentID = oDBEngine.GetFieldValue(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID and exch_compId='" + cmbCompany.SelectedValue + @"'
                    and exh_shortName+'-'+exch_segmentId=(Select Seg_Name CompName from tbl_master_segment Where seg_id=1)) as T1,Master_Exchange
                    Where Exchange_ShortName=Exh_ShortName", "(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID  and ExchangeSegment_Code=Exch_SegmentID) as ExchangeSegmentID", null, 1);
                Session["ExchangeSegmentID"] = SegmentId;
            }
            Session["CmbSegmentValue"] = SegmentId;
            //Session["CmbSegmentValue"] = Request.QueryString["id"].ToString();
        }


        protected void btnDone_Click(object sender, EventArgs e)
        {
            string id = hdncompany.Value;
            string[] idlist = id.Split('~');
            string[] idlistdpid = id.Split('[');


            if (idlist[0] == "Save")
            {
                try
                {
                    string feilds = " ls_lastCompany='" + idlist[1] + "'";
                    feilds += ",ls_lastFinYear='" + idlist[2] + "' ";
                    if (Session["userlastsegment"].ToString() != "3")
                    {
                        if (idlist[4] == "")
                        {
                            int NoOfRowsEffected = 0;
                            //string ID1 = Request.QueryString["id"];
                            if (SegmentId == "99")
                            {
                                NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userid='" + HttpContext.Current.Session["userID"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                if (NoOfRowsEffected == 0)
                                    oDBEngine.InsurtFieldValue(" tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_userId ", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + HttpContext.Current.Session["userid"].ToString() + "'");

                            }
                            else
                            {
                                if (Session["userlastsegment"].ToString() == "5" || Session["userlastsegment"].ToString() == "1")
                                {
                                    try
                                    {
                                        int exch_internalid = 1;
                                        // exch_internalid= oManagement_BL.select_exchinternalid(Convert.ToString(idlist[1]), Convert.ToString(Session["userlastsegment"]), out exch_internalid);
                                        int InternalID = exch_internalid;
                                        feilds += ",ls_lastdpcoid='" + InternalID + "'";
                                        int RowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                        if (RowsEffected == 0)
                                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastdpcoid,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + InternalID + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                        HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                                        HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                                        HttpContext.Current.Session["usersegid"] = InternalID;

                                        HttpContext.Current.Session["LastSettNo"] = idlist[4].ToString();
                                        data = "Save~Y";
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userid='" + HttpContext.Current.Session["userID"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                    if (NoOfRowsEffected == 0)
                                        oDBEngine.InsurtFieldValue(" tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastdpcoid,ls_userid ", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + idlistdpid[1].Substring(0, idlistdpid[1].Trim().Length - 2) + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                    HttpContext.Current.Session["usersegid"] = 1;// idlistdpid[1].Substring(0, idlistdpid[1].Trim().Length - 2);
                                }
                            }



                            HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                            HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                            HttpContext.Current.Session["LastSettNo"] = idlist[3].ToString();
                            data = "Save~Y";
                        }
                        else
                        {

                            try
                            {

                                int exch_internalid = 0;
                                oManagement_BL.select_exchinternalid(Convert.ToString(idlist[1]), Convert.ToString(Session["userlastsegment"]), out exch_internalid);
                                int InternalID = exch_internalid;

                                feilds += ",ls_lastSettlementNo='" + idlist[4].Substring(0, idlist[4].Trim().Length - 1) + "'";

                                feilds += ",ls_lastSettlementType='" + idlist[4].Substring(idlist[4].Trim().Length - 1, 1) + "'";

                                feilds += ",ls_lastdpcoid='" + InternalID + "'";

                                int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                if (NoOfRowsEffected == 0)
                                    oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType ,ls_lastdpcoid,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + idlist[4].Substring(0, idlist[4].Trim().Length - 1) + "','" + idlist[4].Substring(idlist[4].Trim().Length - 1, 1) + "','" + InternalID + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                                HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                                HttpContext.Current.Session["usersegid"] = InternalID;

                                HttpContext.Current.Session["LastSettNo"] = idlist[4].ToString();
                                data = "Save~Y";
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                            if (NoOfRowsEffected == 0)
                                oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                            HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                            HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();
                            data = "Save~Y";
                        }
                        catch
                        {
                        }

                    }
                 
                   //  Response.Redirect("../login.aspx", false);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Changed Successfully !'); window.location='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "/login.aspx';", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Changed Successfully. You need to Re-login.'); window.location.href='/oms/signoff.aspx';", true);
                    //Response.Redirect("frm_selectCompFinYrSett.aspx", false);


                }
                catch (Exception excp)
                {
                    data = "Save~" + excp;
                }
            }

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] idlistdpid = id.Split('[');
            #region Save
            if (idlist[0] == "Save")
            {
                try
                {
                    string feilds = " ls_lastCompany='" + idlist[1] + "'";
                    feilds += ",ls_lastFinYear='" + idlist[2] + "' ";
                    if (Session["userlastsegment"].ToString() != "3")
                    {
                        if (idlist[4] == "")
                        {
                            int NoOfRowsEffected = 0;
                            //string ID1 = Request.QueryString["id"];
                            if (SegmentId == "99")
                            {
                                NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userid='" + HttpContext.Current.Session["userID"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                if (NoOfRowsEffected == 0)
                                    oDBEngine.InsurtFieldValue(" tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_userId ", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + HttpContext.Current.Session["userid"].ToString() + "'");

                            }
                            else
                            {
                                if (Session["userlastsegment"].ToString() == "5" || Session["userlastsegment"].ToString() == "1")
                                {                                    
                                    try
                                    {                                      
                                        int exch_internalid = 1;
                                       // exch_internalid= oManagement_BL.select_exchinternalid(Convert.ToString(idlist[1]), Convert.ToString(Session["userlastsegment"]), out exch_internalid);
                                        int InternalID = exch_internalid;
                                        feilds += ",ls_lastdpcoid='" + InternalID + "'";
                                        int RowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                        if (RowsEffected == 0)
                                            oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastdpcoid,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + InternalID + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                        HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                                        HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                                        HttpContext.Current.Session["usersegid"] = InternalID;

                                        HttpContext.Current.Session["LastSettNo"] = idlist[4].ToString();
                                        data = "Save~Y";
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userid='" + HttpContext.Current.Session["userID"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                    if (NoOfRowsEffected == 0)
                                        oDBEngine.InsurtFieldValue(" tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastdpcoid,ls_userid ", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + idlistdpid[1].Substring(0, idlistdpid[1].Trim().Length - 2) + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                    HttpContext.Current.Session["usersegid"] = 1;// idlistdpid[1].Substring(0, idlistdpid[1].Trim().Length - 2);
                                }
                            }



                            HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                            HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                            HttpContext.Current.Session["LastSettNo"] = idlist[3].ToString();
                            data = "Save~Y";
                        }
                        else
                        {
                           
                            try
                            {

                                int exch_internalid = 0;
                                oManagement_BL.select_exchinternalid(Convert.ToString(idlist[1]), Convert.ToString(Session["userlastsegment"]), out exch_internalid);
                                int InternalID = exch_internalid;

                                feilds += ",ls_lastSettlementNo='" + idlist[4].Substring(0, idlist[4].Trim().Length - 1) + "'";

                                feilds += ",ls_lastSettlementType='" + idlist[4].Substring(idlist[4].Trim().Length - 1, 1) + "'";

                                feilds += ",ls_lastdpcoid='" + InternalID + "'";

                                int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                                if (NoOfRowsEffected == 0)
                                    oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType ,ls_lastdpcoid,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + idlist[4].Substring(0, idlist[4].Trim().Length - 1) + "','" + idlist[4].Substring(idlist[4].Trim().Length - 1, 1) + "','" + InternalID + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                                HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                                HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();

                                HttpContext.Current.Session["usersegid"] = InternalID;

                                HttpContext.Current.Session["LastSettNo"] = idlist[4].ToString();
                                data = "Save~Y";
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_LastSegment ", feilds, " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"].ToString());
                            if (NoOfRowsEffected == 0)
                                oDBEngine.InsurtFieldValue("tbl_trans_LastSegment ", " ls_cntId, ls_lastSegment,ls_lastCompany,ls_lastFinYear,ls_userid", " '" + HttpContext.Current.Session["usercontactID"].ToString() + "'," + HttpContext.Current.Session["userlastsegment"].ToString() + ",'" + idlist[1] + "','" + idlist[2] + "','" + HttpContext.Current.Session["userid"].ToString() + "'");
                            HttpContext.Current.Session["LastCompany"] = idlist[1].ToString();
                            HttpContext.Current.Session["LastFinYear"] = idlist[2].ToString();
                            data = "Save~Y";
                        }
                        catch
                        {
                        }

                    }
                }
                catch (Exception excp)
                {
                    data = "Save~" + excp;
                }
            }
            #endregion
            #region CreateSettlement
            if (idlist[0] == "CreateSettlement")
            {
                string[] DTFin = idlist[1].ToString().Split('-');
                string SettlementNo = DTFin[0].ToString() + "001";
                DataTable dtFin = oDBEngine.GetDataTable("master_FinYear", "FinYear_StartDate,FinYear_EndDate", " FinYear_Code='" + idlist[1].ToString() + "'");
                DataTable dtEx = oDBEngine.GetDataTable("Master_Settlements", "*", "Settlements_ExchangeSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'  and Settlements_FinYear='" + idlist[1].ToString() + "'");
                if (dtEx.Rows.Count == 0)
                {
                    Int32 rowsEffected = oDBEngine.InsurtFieldValue("Master_Settlements", "Settlements_ExchangeSegmentID,Settlements_FinYear,Settlements_Type,Settlements_Number,Settlements_TypeSuffix,Settlements_StartDateTime,Settlements_EndDateTime,Settlements_FundsPayin,Settlements_FundsPayout,Settlements_DeliveryPayin,Settlements_DeliveryPayout", "'" + Session["ExchangeSegmentID"].ToString() + "','" + idlist[1].ToString() + "','Futures','" + SettlementNo + "','F','" + dtFin.Rows[0][0].ToString() + "','" + dtFin.Rows[0][1].ToString() + "','" + dtFin.Rows[0][1].ToString() + "','" + dtFin.Rows[0][1].ToString() + "','" + dtFin.Rows[0][1].ToString() + "','" + dtFin.Rows[0][1].ToString() + "'");

                    data = "CreateSettlement~Settlement Successfully Created.";
                }
                else
                {
                    data = "There is Some Problem To Open Settlement. Please Try Again.";
                }
            }
            #endregion
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            var popupScript = "<script language='javascript'>window.parent.popup.Hide();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            ClientScript.RegisterStartupScript(GetType(), "CloseScript","opener.location.reload(true);self.close();", true);           
            return data;
        }
    }
}