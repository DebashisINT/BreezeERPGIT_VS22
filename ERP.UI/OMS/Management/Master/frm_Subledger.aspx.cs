using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
////using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer; 
using System.Linq; 
//using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor; 
using EntityLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_Subledger : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string strname = "";
        public string accountname = "";
        public string flname = "";
        public string subaccountcode = "";
        string Error = "a";
        string Segment = "";
        AssetDetailBL objAssetDetailbl = new AssetDetailBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Init(object sender, EventArgs e)
        {
            tdstcs.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frm_Subledger.aspx");

                if (!IsPostBack)
                {
                   

                    //BindListTdsType();
                    Session["requesttype"] = "Sub Ledger";
                    String querystring = Request.Url.Query;
                    Session["querystring"] = querystring;
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
                //------- For Read Only User in SQL Datasource Connection String   Start-----------------

                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {
                        //SubAccount.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                        //WithoutCustom.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                        SubAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        WithoutCustom.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    }
                    else
                    {
                        //SubAccount.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        //WithoutCustom.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        SubAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        WithoutCustom.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    }
                }

                //------- For Read Only User in SQL Datasource Connection String   End-----------------


                if (Session["userlastsegment"] == null)
                {
                    Response.Redirect("../../login.aspx");
                }
                else
                {
                    Segment = Convert.ToString(Session["userlastsegment"]);
                    //Segment = Session["userlastsegment"].ToString(); 
                }
                bindgrid();
            //}
            //catch (Exception ex) 
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Something went wrong.Please Try Again later!..');", true); 
            //} 
        }

        public void bindgrid()
        {
            //try
            //{


                string str = Convert.ToString(Request.QueryString["id"]).Trim();
                string subaccountcode = Convert.ToString(Request.QueryString["accountcode"]).Trim();
                //string str = Request.QueryString["id"].ToString().Trim();
                //string subaccountcode = Request.QueryString["accountcode"].ToString().Trim();
                ViewState["MainAccountCode"] = subaccountcode;
                strname = Convert.ToString(Request.QueryString["name"]).Trim();
                //strname = Request.QueryString["name"].ToString().Trim();
                if (strname.ToLower() != "custom" && strname != "Products-Equity" && strname != "Products-Commodity" && strname != "NSDL Clients" && strname != "CDSL Clients")
                {
                    SubAccountGrid.Visible = false;
                    SubAccountWithoutCustom.Visible = true;
                    drdExport.Visible = true;
                    trCustom.Style["display"] = "none";
                    // code commented by sam on 27122016 to avoid the following query due to error the proper query and filll the grid after update for employee
                    //if (strname.ToLower() == "employees")
                    //{
                    //    // code commented by sam on 22122016 to get the proper query and filll the grid after update
                    //    //WithoutCustom.SelectCommand = @"Select Cnt_InternalID Contact_ID,(cnt_firstname + ' '+ cnt_middlename+ ' ' + cnt_lastName) Contact_Name,Cnt_UCC Contact_Code,isnull((Select SubAccount_ReferenceID from Master_SubAccount Where SubAccount_MainAcReferenceID='ADBS001' and SubAccount_Code=Cnt_internalID),0) SubAccount_RefereneceID,Cnt_InternalID SubAccount_Code,(cnt_firstname + ' '+cnt_middlename+ ' ' + cnt_lastName) SubAccount_Name,0 SubAccount_IsTDS,0 SubAccount_TDSRate,Cast('0' as Bit) SubAccount_IsFBT,0 SubAccount_FBTRate,0 SubAccount_RateOfInterest,Cnt_UCC SubAccount_ContactID,0 SubAccount_Depreciation,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID From tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Employees') order by cnt_firstName";
                    //    WithoutCustom.SelectCommand = @"Select Cnt_InternalID Contact_ID,(cnt_firstname + ' '+ cnt_middlename+ ' ' + cnt_lastName) Contact_Name,Cnt_UCC Contact_Code,isnull((Select SubAccount_ReferenceID from Master_SubAccount Where SubAccount_MainAcReferenceID='ADBS001' and SubAccount_Code=Cnt_internalID),0) Subaccount_ReferenceID,Cnt_InternalID SubAccount_Code,(cnt_firstname + ' '+cnt_middlename+ ' ' + cnt_lastName) SubAccount_Name,'0' SubAccount_IsTDS,'0' SubAccount_TDSRate,Cast('0' as Bit) SubAccount_IsFBT,0 SubAccount_FBTRate,'0' SubAccount_RateOfInterest,Cnt_UCC SubAccount_ContactID,'0' SubAccount_Depreciation,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID From tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Employees') order by cnt_firstName";
                    //    // code above commented by sam on 22122016 to get the proper query and filll the grid after update
                    //}
                    //else
                    // code above commented by sam on 27122016 to get the proper query and filll the grid after update for employee
                    if (strname.ToLower() == "sub brokers")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Sub Broker')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Sub Broker')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Sub Broker')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Sub Broker')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "relationship partners")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Partner')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Partner')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Partner')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Partner')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "franchisees")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Franchisee')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Franchisee')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Franchisee')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Franchisee')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "data vendors")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Data Vendor')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Data Vendor')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Data Vendor')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Data Vendor')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "vendors")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Vendor')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Vendor')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Vendors')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Vendors')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "brokers")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Broker')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Broker')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Broker')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Broker')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "business partners")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Partner')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Partner')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Partner')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Partner')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "consultants")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Consultant')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Consultant')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Consultant')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Consultant')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "share holder")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Share Holder')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Share Holder')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Share Holder')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Share Holder')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "debtors")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Debtor')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Debtor')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Debtor')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Debtor')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "creditors")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Creditors')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Creditors')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Creditors')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Creditors')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";

                    }
                    else if (strname.ToLower() == "agents")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Manager')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Manager')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Manager')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Relationship Manager')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
                    }
                    else if (strname.ToLower() == "pro-clients")
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading' union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading'";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading' union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ") and b.cnt_clienttype='Pro Trading'";
                    }
                    else if (strname.ToLower() == "drivertransporter") 
                    {
                        //WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading' union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading'";
                        WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Driver')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and b.cnt_clienttype='Pro Trading' union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Driver')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
                    }
                    else if (strname.ToLower() == "employees") 
                    {
                        WithoutCustom.SelectCommand = "SELECT * FROM(SELECT distinct cnt_internalid as Contact_ID,(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name,  cnt_UCC as Contact_Code FROM  tbl_master_contact  WHERE cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ")  AND cnt_contactType=(SELECT cnt_prefix FROM    tbl_master_contactType  WHERE cnttpy_contactType='Employees') ) AS A INNER JOIN   ( SELECT  Master_SubAccount.Subaccount_ReferenceID, Master_SubAccount.SubAccount_Code, Master_SubAccount.SubAccount_Name, (select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where  ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS, Master_SubAccount.SubAccount_TDSRate, cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID, Master_SubAccount.SubAccount_FBTRate, Master_SubAccount.SubAccount_RateOfInterest, Master_SubAccount.SubAccount_ContactID, Master_SubAccount.SubAccount_Depreciation  FROM   Master_SubAccount WHERE  SubAccount_MainAcReferenceID='" + subaccountcode + "'  ) AS B  ON A.Contact_ID=B.SubAccount_Code   UNION ALL  select distinct  cnt_internalid as Contact_ID, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name, cnt_UCC as Contact_Code, null as Subaccount_ReferenceID, cnt_internalid as SubAccount_Code, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as SubAccount_Name, null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID, null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID, null as SubAccount_Depreciation   FROM tbl_master_contact WHERE  cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Employees') and  cnt_internalid  not in(select DISTINCT SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "'  AND SubAccount_Code LIKE  (SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Employees')+'%' )  and cnt_branchid in (" + Convert.ToString(Session["userbranchHierarchy"]) + ") ";
                    }
                    else
                    {
                        // WithoutCustom.SelectCommand = "SELECT distinct b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,Master_SubAccount.Subaccount_ReferenceID,Master_SubAccount.SubAccount_Code,Master_SubAccount.SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_ContactID,Master_SubAccount.SubAccount_Depreciation FROM (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid where SubAccount_MainAcReferenceID='" + subaccountcode + "' and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") union all select b.cnt_internalid as Contact_ID,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as Contact_Name,b.cnt_UCC as Contact_Code,null as Subaccount_ReferenceID,b.cnt_internalid as SubAccount_Code,(ISNULL(b.cnt_firstname,'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) as SubAccount_Name,null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID,null as SubAccount_Depreciation from (SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers')) AS b where b.cnt_internalid not in(select SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "') and b.cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") ";

                        //WithoutCustom.SelectCommand = "SELECT * FROM(SELECT distinct cnt_internalid as Contact_ID,(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name,  cnt_UCC as Contact_Code FROM  tbl_master_contact  WHERE cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ")  AND cnt_contactType=(SELECT cnt_prefix FROM    tbl_master_contactType  WHERE cnttpy_contactType='Customers') ) AS A INNER JOIN   ( SELECT  Master_SubAccount.Subaccount_ReferenceID, Master_SubAccount.SubAccount_Code, Master_SubAccount.SubAccount_Name, (select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where  ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS, Master_SubAccount.SubAccount_TDSRate, cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID, Master_SubAccount.SubAccount_FBTRate, Master_SubAccount.SubAccount_RateOfInterest, Master_SubAccount.SubAccount_ContactID, Master_SubAccount.SubAccount_Depreciation  FROM   Master_SubAccount WHERE  SubAccount_MainAcReferenceID='" + subaccountcode + "'  ) AS B  ON A.Contact_ID=B.SubAccount_Code   UNION ALL  select distinct  cnt_internalid as Contact_ID, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name, cnt_UCC as Contact_Code, null as Subaccount_ReferenceID, cnt_internalid as SubAccount_Code, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as SubAccount_Name, null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID, null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID, null as SubAccount_Depreciation   FROM tbl_master_contact WHERE  cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers') and  cnt_internalid  not in(select DISTINCT SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "'  AND SubAccount_Code LIKE  (SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers')+'%' )  and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") ";
                        //WithoutCustom.SelectCommand = "SELECT * FROM(SELECT distinct cnt_internalid as Contact_ID,(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name,  cnt_UCC as Contact_Code FROM  tbl_master_contact  WHERE cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ")  AND cnt_contactType=(SELECT cnt_prefix FROM    tbl_master_contactType  WHERE cnttpy_contactType='Customers') ) AS A INNER JOIN   ( SELECT  Master_SubAccount.Subaccount_ReferenceID, Master_SubAccount.SubAccount_Code, Master_SubAccount.SubAccount_Name, (select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where  ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS, Master_SubAccount.SubAccount_TDSRate, cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID, Master_SubAccount.SubAccount_FBTRate, Master_SubAccount.SubAccount_RateOfInterest, Master_SubAccount.SubAccount_ContactID, Master_SubAccount.SubAccount_Depreciation  FROM   Master_SubAccount WHERE  SubAccount_MainAcReferenceID='" + subaccountcode + "'  ) AS B  ON A.Contact_ID=B.SubAccount_Code   UNION ALL  select distinct  cnt_internalid as Contact_ID, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name, cnt_UCC as Contact_Code, null as Subaccount_ReferenceID, cnt_internalid as SubAccount_Code, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as SubAccount_Name, null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID, null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID, null as SubAccount_Depreciation   FROM tbl_master_contact WHERE  cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers') and  cnt_internalid  not in(select DISTINCT SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "'  AND SubAccount_Code LIKE  (SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers')+'%' )  and cnt_branchid in (" + Convert.ToString(Session["userbranchHierarchy"]) + ") ";
                        WithoutCustom.SelectCommand = "SELECT * FROM(SELECT distinct cnt_internalid as Contact_ID,(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name,  cnt_UCC as Contact_Code FROM  tbl_master_contact  WHERE cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ")  AND cnt_contactType=(SELECT cnt_prefix FROM    tbl_master_contactType  WHERE cnttpy_contactType='Customers') ) AS A INNER JOIN   ( SELECT  Master_SubAccount.Subaccount_ReferenceID, Master_SubAccount.SubAccount_Code, Master_SubAccount.SubAccount_Name, (select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where  ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS, Master_SubAccount.SubAccount_TDSRate, cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,Master_SubAccount.SubAccount_MainAcReferenceID, Master_SubAccount.SubAccount_FBTRate, Master_SubAccount.SubAccount_RateOfInterest, Master_SubAccount.SubAccount_ContactID, Master_SubAccount.SubAccount_Depreciation  FROM   Master_SubAccount WHERE  SubAccount_MainAcReferenceID='" + subaccountcode + "'  ) AS B  ON A.Contact_ID=B.SubAccount_Code   UNION ALL  select distinct  cnt_internalid as Contact_ID, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as Contact_Name, cnt_UCC as Contact_Code, null as Subaccount_ReferenceID, cnt_internalid as SubAccount_Code, (ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISNULL(cnt_lastName,'')) as SubAccount_Name, null as SubAccount_IsTDS,null as SubAccount_TDSRate,cast(0 as bit) as SubAccount_IsFBT,'Documents' as AssetCustom1,null as SubAccount_MainAcReferenceID, null as SubAccount_FBTRate,null as SubAccount_RateOfInterest,null as SubAccount_ContactID, null as SubAccount_Depreciation   FROM tbl_master_contact WHERE  cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers') and  cnt_internalid  not in(select DISTINCT SubAccount_Code from master_subaccount where SubAccount_MainAcReferenceID='" + subaccountcode + "'  AND SubAccount_Code LIKE  (SELECT cnt_prefix FROM tbl_master_contactType  WHERE cnttpy_contactType='Customers')+'%' )  and cnt_branchid in (" + Convert.ToString(Session["userbranchHierarchy"]) + ") ";
                    }
                }
                else if (strname == "Products-Equity")
                {
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    if (subaccountcode == "SYSTM00020" || subaccountcode == "SYSTM00021")
                        SubAccount.SelectCommand = "SELECT distinct b.Equity_SeriesID as SubAccount_ReferenceID,b.Equity_SeriesID as SubAccount_Code ,b.Equity_TickerSymbol+' -'+b.Equity_Series as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,Master_SubAccount.SubAccount_MainAcReferenceID as SubAccount_MainAcReferenceID,'Documents' as AssetCustom1 FROM master_equity AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code in (cast(b.Equity_SeriesID as varchar(50)))";
                    else
                        //SubAccount.SelectCommand = "SELECT distinct b.Equity_SeriesID as SubAccount_ReferenceID,b.Equity_SeriesID as SubAccount_Code ,b.Equity_TickerSymbol+' -'+b.Equity_Series as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,Master_SubAccount.SubAccount_MainAcReferenceID as SubAccount_MainAcReferenceID,'Documents' as AssetCustom1 FROM master_equity AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code in (cast(b.Equity_SeriesID as varchar(50))) where Equity_ExchSegmentID=" + Session["ExchangeSegmentID"].ToString() + "";
                        SubAccount.SelectCommand = "SELECT distinct b.Equity_SeriesID as SubAccount_ReferenceID,b.Equity_SeriesID as SubAccount_Code ,b.Equity_TickerSymbol+' -'+b.Equity_Series as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,Master_SubAccount.SubAccount_MainAcReferenceID as SubAccount_MainAcReferenceID,'Documents' as AssetCustom1 FROM master_equity AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code in (cast(b.Equity_SeriesID as varchar(50))) where Equity_ExchSegmentID=" + Convert.ToString(Session["ExchangeSegmentID"]) + "";
                    SubAccountGrid.Columns[7].Visible = false;
                }
                else if (strname == "Products-Commodity")
                {
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    SubAccount.SelectCommand = @"Select SubAccount_ReferenceID,SubAccount_Code,SubAccount_Name,SubAccount_IsTDS,SubAccount_TDSRate,SubAccount_IsFBT,SubAccount_FBTRate,
                                        SubAccount_RateOfInterest,SubAccount_Depreciation  from 
                                        (SELECT distinct Left(Commodity_Identifier,3) Identifier,b.commodity_ProductSeriesID as SubAccount_ReferenceID,b.commodity_ProductSeriesID as SubAccount_Code ,
                                        Cast(Ltrim(Rtrim(b.commodity_TickerSymbol)) AS Varchar)+''+CONVERT(VARCHAR(11),b.COMMODITY_EXPIRYDATE,106) as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' 
                                        from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,
                                        Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,
                                        Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,'Documents' as AssetCustom1 
                                        FROM master_commodity AS b Left outer JOIN Master_SubAccount 
                                        ON Master_SubAccount.subAccount_code in(cast(b.commodity_ProductSeriesID as varchar(50)))) as T1
                                        Where Identifier in ('FUT','OPT')";
                    SubAccountGrid.Columns[7].Visible = false;
                }
                else if (strname == "Products-Spot")
                {
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    SubAccount.SelectCommand = @"Select SubAccount_ReferenceID,SubAccount_Code,SubAccount_Name,SubAccount_IsTDS,SubAccount_TDSRate,SubAccount_IsFBT,SubAccount_FBTRate,
                                        SubAccount_RateOfInterest,SubAccount_Depreciation  from 
                                        (SELECT distinct Left(Commodity_Identifier,3) Identifier,b.commodity_ProductSeriesID as SubAccount_ReferenceID,b.commodity_ProductSeriesID as SubAccount_Code ,
                                        Ltrim(Rtrim(b.commodity_TickerSymbol)) as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' 
                                        from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,
                                        Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,
                                        Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,'Documents' as AssetCustom1
                                        FROM master_commodity AS b Left outer JOIN Master_SubAccount 
                                        ON Master_SubAccount.subAccount_code in(cast(b.commodity_ProductSeriesID as varchar(50)))) as T1
                                        Where Identifier in ('SPT')";
                    SubAccountGrid.Columns[7].Visible = false;
                }

                else if (strname == "NSDL Clients")
                {
                    string MainAccID = Request.QueryString["accountcode"].ToString();
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    SubAccount.SelectCommand = "SELECT distinct b.nsdlclients_benaccountid as SubAccount_ReferenceID,b.nsdlclients_benaccountid as SubAccount_Code ,b.nsdlclients_benfirstholdername as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,isnull(SubAccount_MainAcReferenceID ,'" + MainAccID + "') as SubAccount_MainAcReferenceID,'Documents' as AssetCustom1 FROM (select nsdlclients_contactid,nsdlclients_benaccountid,nsdlclients_benfirstholdername from master_nsdlclients) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code in(cast(b.nsdlclients_benaccountid as varchar(50))) and isnull(SubAccount_MainAcReferenceID,'')='" + MainAccID + "'";
                    SubAccountGrid.Columns[7].Visible = false;
                }
                else if (strname == "CDSL Clients")
                {
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    SubAccount.SelectCommand = "SELECT distinct b.cdslclients_benaccountnumber as SubAccount_ReferenceID,b.cdslclients_benaccountnumber as SubAccount_Code ,b.cdslclients_firstholdername as SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,Master_SubAccount.SubAccount_TDSRate,cast(isnull(Master_SubAccount.SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,Master_SubAccount.SubAccount_FBTRate,Master_SubAccount.SubAccount_RateOfInterest,Master_SubAccount.SubAccount_Depreciation,Master_SubAccount.SubAccount_MainAcReferenceID as SubAccount_MainAcReferenceID,'Documents' as AssetCustom1 FROM (select cdslclients_benaccountnumber,cdslclients_contactid,cdslclients_firstholdername from master_cdslclients ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code in(cast(b.cdslclients_benaccountnumber as varchar(50))) where b.cdslclients_benaccountnumber is not null";
                    SubAccountGrid.Columns[7].Visible = false;
                }
                else
                {
                    trWithoutCustom.Style["display"] = "none";
                    SubAccountGrid.Visible = true;
                    SubAccountWithoutCustom.Visible = false;
                    SubAccount.SelectCommand = "select distinct 'Documents' as AssetCustom1,SubAccount_MainAcReferenceID,SubAccount_Code as SubAccount_ReferenceID,SubAccount_Code,SubAccount_Name,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_SubAccount.SubAccount_TDSRate))) as SubAccount_IsTDS,SubAccount_TDSRate,cast(isnull(SubAccount_IsFBT,0) as bit) as SubAccount_IsFBT,SubAccount_FBTRate,SubAccount_RateOfInterest,SubAccount_Depreciation from Master_SubAccount where  SubAccount_MainAcReferenceID in(select mainaccount_accountcode from master_mainaccount where mainaccount_referenceID='" + str + "')  order by SubAccount_ReferenceID  desc";
                    // SubAccountGrid.Columns[7].Visible = false;
                }
                //string AssetVal = Request.QueryString["accountType"].ToString();
                string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
                if (Segment == "5")
                {
                    if (AssetVal == "Asset" && Segment == "5")
                    {
                        SubAccountWithoutCustom.Columns[14].Visible = true;
                        SubAccountGrid.Columns[10].Visible = true;
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load1();</script>");
                    }
                    else
                    {
                        //SubAccountWithoutCustom.Columns[15].Visible = false;
                        //SubAccountGrid.Columns[11].Visible = false;
                        SubAccountWithoutCustom.Columns[14].Visible = false;
                        SubAccountGrid.Columns[10].Visible = false;
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    }
                }
                else
                {
                    //SubAccountWithoutCustom.Columns[15].Visible = false;
                    //SubAccountGrid.Columns[11].Visible = false;
                    //SubAccountWithoutCustom.Columns[14].Visible = false;
                    //SubAccountGrid.Columns[10].Visible = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                }
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Something Went wrong.Please Try Again later!..');", true); 
            //}
        }
        //public void BindListTdsType()
        //{
        //    Dim MyComboBox As ASPxEditors.ASPxComboBox = Nothing
        //    MyComboBox = SubAccountWithoutCustom.FindEditFormTemplateControl("ASPxComboBox_ExpYear")
        //    DataTable tdstypedt = objAssetDetailbl.BindTdsTypeForSubLedger();
        //    if (tdstypedt != null && tdstypedt.Rows.Count > 0)
        //    {


        //        ListBox1.DataTextField = "description";
        //        ListBox1.DataValueField = "code";
        //        ListBox1.DataSource = tdstypedt;
        //        ListBox1.DataBind();
        //        ListBox1.Items.Insert(0, new ListItem("Select", "0"));
        //    }

        //}
        Dictionary<string, object> values = new Dictionary<string, object>();
        public void ShowList()
        {
            string str1 = Request.QueryString["id"].ToString();
            DataTable dt = oDbEngine.GetDataTable("Master_MainAccount", "MainAccount_Name", "MainAccount_ReferenceID='" + str1 + "' ");
            // string[] st = str.Split(',');
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //accountname = "Sub Account Of: " + dt.Rows[i]["MainAccount_Name"].ToString();
                    accountname = "Sub Account Of: " + Convert.ToString(dt.Rows[i]["MainAccount_Name"]);
                    flname = "SubAccountOf_" + Convert.ToString(dt.Rows[i]["MainAccount_Name"]);
                }
            }
            else
            {
                accountname = "No Sub Account Found.";
            }
            if(accountname!="")
            {
                Session["acname"] = accountname;
            }
            else
            {
                Session["acname"] = "Sub Account";
            }
            
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowList();
            string filename = "SubLedger";
            //if (Session["Contactrequesttype"] != null)
            //{
            //    filename = Session["Contactrequesttype"].ToString();
            //}
            //else
            //{
            //    filename = "Lead";
            //}

            //Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (flname!="")
            {
                exporter.FileName = filename;
                exporter.PageHeader.Left = Convert.ToString(Session["acname"]);
            }
            else
            {
                exporter.FileName = "SubAccount";
                exporter.PageHeader.Left = Convert.ToString(Session["acname"]);
            } 
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            SubAccountWithoutCustom.Columns[12].Visible = false;
            SubAccountWithoutCustom.Columns[13].Visible = false;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    //bindpdf();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #region SubAccountGrid 1
        protected void SubAccountGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            values.Clear();
            //ASPxCheckBox list = (ASPxCheckBox)SubAccountGrid.FindEditRowCellTemplateControl((GridViewDataCheckColumn)SubAccountGrid.Columns["SubAccount_IsFBT"], "SubAccount_IsFBT");
            //ASPxCheckBox chkBox = (ASPxCheckBox)SubAccountGrid.FindEditFormTemplateControl("SubAccount_IsTDS");
            //values.Add("SubAccount_IsTDS", chkBox.Checked);

            //var k = ((ASPxCheckBox)SubAccountGrid.FindEditFormTemplateControl("SubAccount_IsFBT")) as ASPxCheckBox;
            ASPxCheckBox chkBox1 = ((ASPxCheckBox)SubAccountGrid.FindEditFormTemplateControl("SubAccount_IsFBT")) as ASPxCheckBox; 
            values.Add("SubAccount_IsFBT", chkBox1.Checked);

            //ASPxCheckBox chkBox1 = (ASPxCheckBox)(SubAccountGrid.FindEditRowCellTemplateControl((GridViewDataCheckColumn)SubAccountGrid.Columns["SubAccount_IsFBT"], "SubAccount_IsFBT"));
            //values.Add("SubAccount_IsFBT", chkBox1.Checked);           

        }
        protected void SubAccountGrid_OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
           

            ASPxCheckBox chkfbtratecus = (ASPxCheckBox)SubAccountGrid.FindEditFormTemplateControl("SubAccount_IsFBT");
            ASPxComboBox tcscts = (ASPxComboBox)SubAccountGrid.FindEditFormTemplateControl("cmb_tdstcs");
            string istcsrate = Convert.ToString(tcscts.Value);

            // .............................Code Above Commented and Added by Sam on 09122016...................................... 
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("UpdateInSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Convert.ToString(Request.QueryString["accountcode"]).Trim();

            lcmd.Parameters.Add("@MainAcReferenceID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["id"]);

            lcmd.Parameters.Add("@SubAccount_ReferenceID", SqlDbType.VarChar).Value = Convert.ToString(e.Keys[0]);
            lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["SubAccount_Code"]);

            lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["SubAccount_Name"]);

           
            
            // .............................Code Commented and Added by Sam on 09122016. ..................................... 
            if (e.NewValues["SubAccount_TDSRate"] != null)
            {
                lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = istcsrate;
            }
            else { lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = ""; }
     
            if (chkfbtratecus.Checked == true)
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = true;
                if (e.NewValues["SubAccount_FBTRate"] != null)
                {
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_FBTRate"]));
                }
                else
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = false;
                lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
            }

            // .............................Code Above Commented and Added by Sam on 07122016...................................... 
            lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_RateOfInterest"]));
            lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_Depreciation"]));
            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();
            e.Cancel = true;
            bindgrid();
            SubAccountGrid.CancelEdit();
        }
        protected void SubAccountGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
           
          
            ASPxCheckBox chkfbtratecus = (ASPxCheckBox)SubAccountGrid.FindEditFormTemplateControl("SubAccount_IsFBT");
            ASPxComboBox tcscts = (ASPxComboBox)SubAccountGrid.FindEditFormTemplateControl("cmb_tdstcs");
            string istcsrate = Convert.ToString(tcscts.Value);
           
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("InsertInSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
          
            lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Convert.ToString(Request.QueryString["id"]);
            if (e.NewValues["SubAccount_Code"] != null)
            {
               
                lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["SubAccount_Code"]);
            }
            else
                lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = "";
            if (e.NewValues["SubAccount_Name"] != null)
            {
                
                lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["SubAccount_Name"]);
            }
            else
                lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = "";


          
            if (e.NewValues["SubAccount_TDSRate"] != null)
            {
                lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = istcsrate;
            }
            else { lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = ""; }

           
            if (chkfbtratecus.Checked == true)
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = true;
                if (e.NewValues["SubAccount_FBTRate"] != null)
                {
                   
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_FBTRate"]));
                }
                else
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
               
            }
            else
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = false;
                lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
            }
             
            // .............................Code Above Commented and Added by Sam on 07122016...................................... 

            if (e.NewValues["SubAccount_RateOfInterest"] != null)
            {
               
                lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_RateOfInterest"]));
            }
            else
                lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.VarChar).Value = "";
            if (e.NewValues["SubAccount_Depreciation"] != null)
            {
              
                lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_Depreciation"]));
            }
            else
                lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();
            e.Cancel = true;
            SubAccountGrid.CancelEdit();
            bindgrid();
        } 
        protected void SubAccountGrid_OnStartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            // Validates the edited row if it isn't a new row,.
            //if (!SubAccountGrid.IsNewRowEditing)
            //    SubAccountGrid.DoRowValidation();
            Error = "z";
        } 
        protected void SubAccountGrid_OnRowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (!SubAccountGrid.IsNewRowEditing)
            {


            }
            else
            {
                if (e.NewValues["SubAccount_Code"].ToString().Trim() == String.Empty) e.RowError = "SubAccount Code Required.";
                string strSubAccountCode = "";

                if (e.NewValues["SubAccount_Code"] != null)
                {
                   
                    strSubAccountCode = Convert.ToString(e.NewValues["SubAccount_Code"]);
                }

                DBEngine gg = new DBEngine("");
              
                SqlDataReader GetReader1 = gg.GetReader("SELECT  SubAccount_Code as SubAccount_Code FROM Master_SubAccount where SubAccount_Code='" + strSubAccountCode + "' and SubAccount_MainAcReferenceID='" + Convert.ToString(ViewState["MainAccountCode"]) + "'");

                if (GetReader1.HasRows == true)
                {

                    e.RowError = "Account Code  should be unique.";
                }
                gg.CloseConnection();

            }

        } 
        protected void SubAccountGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                SubAccountGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                SubAccountGrid.FilterExpression = string.Empty;
            }
        } 
        protected void SubAccountGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {



            if (e.DataColumn.FieldName == "Custom")
            {
                string kv = e.GetValue("SubAccount_Code").ToString();
                
                string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
               
                e.Cell.Attributes.Add("onclick", "ShowCustom('" + kv + "','" + Convert.ToString(Request.QueryString["id"]) + "')");
            }
            if (e.DataColumn.FieldName == "AssetCustom")
            {
               
                string kv = Convert.ToString(e.GetValue("SubAccount_Code"));
               
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
               
                e.Cell.Attributes.Add("onclick", "ShowAssetCustom('" + kv + "','" + Convert.ToString(Request.QueryString["accountcode"]) + "')");
            }

            if (e.DataColumn.FieldName == "AssetCustom1")
            {
               
                string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
              
                string kv1 = Convert.ToString(e.GetValue("SubAccount_Code"));
               
                string cellv1 = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
               
                e.Cell.Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv1 + "');");
                e.Cell.ToolTip = "ADD/VIEW";
                e.Cell.Style.Add("color", "Blue");
            }
        }

        protected void SubAccountGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                
                string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
                
                string kv = Convert.ToString(e.GetValue("SubAccount_Code"));
                
                string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));

                if (Segment == "5")
                {
                    if (AssetVal == "Asset" && Segment == "5")
                    {
                        e.Row.Cells[5].Style.Add("cursor", "hand");
                       
                        e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");

                        e.Row.Cells[5].ToolTip = "ADD/VIEW";
                        e.Row.Cells[5].Style.Add("color", "Blue");
                    }
                    else
                    {
                        e.Row.Cells[4].Style.Add("cursor", "hand");
                      
                        e.Row.Cells[4].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");

                        e.Row.Cells[4].ToolTip = "ADD/VIEW";
                        e.Row.Cells[4].Style.Add("color", "Blue");
                    }
                }
            }
        }
        protected void SubAccountGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
           
            string str = Convert.ToString(Request.QueryString["id"]);
           
            string Code = Convert.ToString(e.Values[0]);
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand lcmd = new SqlCommand("DeleteCustomAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@MainAcountCode", SqlDbType.VarChar, 15).Value = str;
            lcmd.Parameters.Add("@SubAcountCode", SqlDbType.VarChar, 15).Value = Code;
            lcon.Open();
            int NoofRowsAffected = lcmd.ExecuteNonQuery();
            lcon.Close();
            if (NoofRowsAffected <= 0)
            {
                Error = "b";
            }
            e.Cancel = true;
        }
        protected void SubAccountGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

            e.Properties["cpInsertError"] = Error;
        } 
       

        #endregion SubAccountGrid 1

        #region SubAccountWithoutCustom 2

        #region During Open the Page Following Events occur
        protected void SubAccountWithoutCustom_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                //string AssetVal = Request.QueryString["accountType"].ToString();
                string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
                //string kv = e.GetValue("SubAccount_Code").ToString();
                string kv = Convert.ToString(e.GetValue("SubAccount_Code"));
                //string cellv = e.GetValue("SubAccount_MainAcReferenceID").ToString();
                string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
                //string subaccountcode123 = Request.QueryString["accountcode"].ToString().Trim();
                string subaccountcode123 = Convert.ToString(Request.QueryString["accountcode"]).Trim();
                if (Segment == "5")
                {
                    if (AssetVal == "Asset" && Segment == "5")
                    {
                        e.Row.Cells[6].Style.Add("cursor", "hand");
                        // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
                        //e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
                        e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
                        e.Row.Cells[6].ToolTip = "ADD/VIEW";
                        e.Row.Cells[6].Style.Add("color", "Blue");
                    }
                    else
                    {
                        e.Row.Cells[5].Style.Add("cursor", "hand");
                        // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
                        //e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
                        e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
                        e.Row.Cells[5].ToolTip = "ADD/VIEW";
                        e.Row.Cells[5].Style.Add("color", "Blue");
                    }
                }
            }
        } 
        protected void SubAccountWithoutCustom_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

            e.Properties["cpInsertErrorWithoutCustom"] = 'a';

        }
        protected void SubAccountWithoutCustom_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "ShowWCustom")
            {
                //string kv = e.GetValue("Contact_ID").ToString();
                string kv = Convert.ToString(e.GetValue("Contact_ID"));
                //string cellv = e.CellValue.ToString();
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                //e.Cell.Attributes.Add("onclick", "ShowWCustomType('" + kv + "','" + Request.QueryString["id"].ToString() + "')");
                e.Cell.Attributes.Add("onclick", "ShowWCustomType('" + kv + "','" + Convert.ToString(Request.QueryString["id"]) + "')");
            }
            if (e.DataColumn.FieldName == "AssetShowWCustom")
            {
                //string kv = e.GetValue("Contact_ID").ToString();
                string kv = Convert.ToString(e.GetValue("Contact_ID"));
                //string cellv = e.CellValue.ToString();
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                e.Cell.ToolTip = "Asset Detail";
                // .............................Code Commented and Added by Sam on 05122016.To pass the parameter  .....................................  

                //e.Cell.Attributes.Add("onclick", "ShowAssetCustom()");
                //e.Cell.Attributes.Add("onclick", "ShowAssetCustom('" + kv + "','" + Request.QueryString["id"].ToString() + "')");
                e.Cell.Attributes.Add("onclick", "ShowAssetCustom('" + kv + "','" + Convert.ToString(Request.QueryString["id"]) + "')");
                // .............................Code Above Commented and Added by Sam on 05122016...................................... 

            }
            if (e.DataColumn.FieldName == "AssetCustom1")
            {
                //string kv = e.GetValue("Contact_ID").ToString();
                string kv = Convert.ToString(e.GetValue("Contact_ID"));
                e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
                //e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
                e.Cell.Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
                e.Cell.ToolTip = "Document";

            }

        }  
        #endregion 

        protected void SubAccountWithoutCustom_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            if (Session["Code"] != null)
            {
                //e.NewValues["Contact_ID"] = Session["Code"].ToString();
                e.NewValues["Contact_ID"] = Convert.ToString(Session["Code"]);
                Session["Code"] = null;
            }
            if (Session["Name"] != null)
            {
                //e.NewValues["Contact_Name"] = Session["Name"].ToString();
                e.NewValues["Contact_Name"] = Convert.ToString(Session["Name"]);
                Session["Name"] = null;
            }

        }
        protected void SubAccountWithoutCustom_OnHtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 29112016 to unuse of this section. ...................................
            //values.Clear();
            // .............................Code Above Commented and Added by Sam on 29112016...................................... 

            //ASPxCheckBox chkBox = (ASPxCheckBox)SubAccountWithoutCustom.FindEditFormTemplateControl("SubAccount_IsTDS");
            //values.Add("SubAccount_IsTDS", chkBox.Checked);

            //ASPxCheckBox chkBox1 = (ASPxCheckBox)SubAccountWithoutCustom.FindEditFormTemplateControl("SubAccount_IsFBT1");
            //values.Add("SubAccount_IsFBT", chkBox1.Checked);

            if (SubAccountWithoutCustom.IsNewRowEditing)
            {

            }
            else
            {

            }

        }
        protected void SubAccountWithoutCustom_OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxCheckBox chkfbtrate = (ASPxCheckBox)SubAccountWithoutCustom.FindEditFormTemplateControl("SubAccount_IsFBT12");
            ASPxComboBox tcscts = (ASPxComboBox)SubAccountWithoutCustom.FindEditFormTemplateControl("cmd_tdstcs");
            string istcsrate = Convert.ToString(tcscts.Value);
            //HiddenField hdnfbtrate = (HiddenField)SubAccountWithoutCustom.FindEditFormTemplateControl("hdnisfbtrate");
            //HiddenField hdnfbtrate = (HiddenField)SubAccountWithoutCustom.FindEditFormTemplateControl("hdnisfbtrate");
            //string fbtrate = hdnfbtrate.Value;
            ASPxGridView grid = sender as ASPxGridView;
            //e.NewValues["SubAccount_IsTDS"] = values["SubAccount_IsTDS"];

            // .............................Code Commented and Added by Sam on 29112016. ...................................
            //e.NewValues["SubAccount_IsFBT"] = values["SubAccount_IsFBT"];
            string isfbt = Convert.ToString(e.NewValues["SubAccount_IsFBT"]);
            // .............................Code Above Commented and Added by Sam on 29112016...................................... 

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("UpdateInSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Request.QueryString["accountcode"].ToString().Trim();
            lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Convert.ToString(Request.QueryString["accountcode"]).Trim();
            if (Session["SubAccount_ReferenceID"] != null)
            {
                //lcmd.Parameters.Add("@Subaccount_ReferenceID", SqlDbType.VarChar).Value = Session["SubAccount_ReferenceID"].ToString();
                lcmd.Parameters.Add("@Subaccount_ReferenceID", SqlDbType.VarChar).Value = Convert.ToString(Session["SubAccount_ReferenceID"]);
            }
            //lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = e.NewValues["Contact_ID"].ToString();
            lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["Contact_ID"]);
            //lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = e.NewValues["Contact_Name"].ToString();
            lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["Contact_Name"]);
            //lcmd.Parameters.Add("@SubAccount_IsTDS", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["SubAccount_IsTDS"]);
            // .............................Code Commented and Added by Sam on 07122016. ..................................... 
            //lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["SubAccount_IsFBT"]);
            if (chkfbtrate.Checked == true)
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = true;
                //lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_FBTRate"].ToString());
                lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_FBTRate"]));
            }
            else
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = false;
                lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
            }

            // .............................Code Above Commented and Added by Sam on 07122016...................................... 

            

            //lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = e.NewValues["SubAccount_TDSRate"].ToString();
            lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = istcsrate;
            
            //lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_RateOfInterest"].ToString());
            lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_RateOfInterest"]));
            //lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_Depreciation"].ToString());
            lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_Depreciation"]));
            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();
            e.Cancel = true;
            bindgrid();
            SubAccountWithoutCustom.CancelEdit(); 
        }
        protected void SubAccountWithoutCustom_OnStartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //Session["Code"] = e.EditingKeyValue.ToString();
            Session["Code"] = Convert.ToString(e.EditingKeyValue);
            //DataTable dtWC = oDbEngine.GetDataTable("Master_SubAccount", "SubAccount_Code,SubAccount_ReferenceID", "SubAccount_Code='" + e.EditingKeyValue.ToString() + "' ");
            DataTable dtWC = oDbEngine.GetDataTable("Master_SubAccount", "SubAccount_Code,SubAccount_ReferenceID", "SubAccount_Code='" + Convert.ToString(e.EditingKeyValue) + "' ");
            if (dtWC.Rows.Count != 0)
            {
                //Records are exist , Editing previous record.
                //Session["SubAccount_ReferenceID"] = dtWC.Rows[0]["SubAccount_ReferenceID"].ToString();
                Session["SubAccount_ReferenceID"] = Convert.ToString(dtWC.Rows[0]["SubAccount_ReferenceID"]);
                //SubAccountWithoutCustom.DoRowValidation();
            }
            else
            {
                //DataTable dt = oDbEngine.GetDataTable("tbl_master_contact", "(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISnull(cnt_lastName,'')) as Fullname", "cnt_internalid='" + e.EditingKeyValue.ToString() + "' ");
                DataTable dt = oDbEngine.GetDataTable("tbl_master_contact", "(ISNULL(cnt_firstname,'') + ' '+ ISNULL(cnt_middlename,'')+ ' ' + ISnull(cnt_lastName,'')) as Fullname", "cnt_internalid='" + Convert.ToString(e.EditingKeyValue) + "' ");

                if (dt.Rows.Count != 0)
                {

                    //Session["Name"] = dt.Rows[0]["Fullname"].ToString();
                    Session["Name"] = Convert.ToString(dt.Rows[0]["Fullname"]);
                }
                //Record Does not Exist, Have to Insert as New Record
                SubAccountWithoutCustom.AddNewRow();
            }
        }
        protected void SubAccountWithoutCustom_OnRowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //ASPxCheckBox chkfbtrate = (ASPxCheckBox)SubAccountWithoutCustom.FindEditFormTemplateControl("SubAccount_IsFBT12");
            //e.NewValues["SubAccount_IsTDS"] = values["SubAccount_IsTDS"];
            //e.NewValues["SubAccount_IsFBT"] = values["SubAccount_IsFBT"];
            

            // .............................Code Commented and Added by Sam on 09122016. ..................................... 
            ASPxCheckBox chkfbtrate = (ASPxCheckBox)SubAccountWithoutCustom.FindEditFormTemplateControl("SubAccount_IsFBT12");
            ASPxComboBox tcscts = (ASPxComboBox)SubAccountWithoutCustom.FindEditFormTemplateControl("cmd_tdstcs");
            string istcsrate = Convert.ToString(tcscts.Value);
            //string isfbt = Convert.ToString(e.NewValues["SubAccount_IsFBT"]);

            // .............................Code Above Commented and Added by Sam on 09122016...................................... 
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("InsertInSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Request.QueryString["id"].ToString();
            lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.VarChar).Value = Convert.ToString(Request.QueryString["id"]);
            if (e.NewValues["Contact_ID"] != null)
            {
                //lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = e.NewValues["Contact_ID"].ToString();
                lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["Contact_ID"]);
            }
            else
                lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = "";
            if (e.NewValues["Contact_Name"] != null)
            {
                //lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = e.NewValues["Contact_Name"].ToString();
                lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["Contact_Name"]);
            }
            else
                lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = "";


            //lcmd.Parameters.Add("@SubAccount_IsTDS", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["SubAccount_IsTDS"]);
            // .............................Code Commented and Added by Sam on 09122016. ..................................... 
            //if (e.NewValues["SubAccount_TDSRate"] != null)
            //{
            lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = istcsrate;
            //}

            //if (e.NewValues["SubAccount_TDSRate"] != null)
            //{
            //    lcmd.Parameters.Add("@SubAccount_TDSRate", SqlDbType.VarChar).Value = e.NewValues["SubAccount_TDSRate"].ToString();
            //}
            // .............................Code Above Commented and Added by Sam on 09122016...................................... 


            // .............................Code Commented and Added by Sam on 07122016. ..................................... 
            //lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["SubAccount_IsFBT"]);
            if (chkfbtrate.Checked == true)
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = true;
                if (e.NewValues["SubAccount_FBTRate"] != null)
                {
                    //lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_FBTRate"].ToString());
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_FBTRate"]));
                }
                else
                    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
                //lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_FBTRate"].ToString());
            }
            else
            {
                lcmd.Parameters.Add("@SubAccount_IsFBT", SqlDbType.Bit).Value = false;
                lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
            }
            //if (e.NewValues["SubAccount_FBTRate"] != null)
            //{
            //    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_FBTRate"].ToString());
            //}
            //else
            //    lcmd.Parameters.Add("@SubAccount_FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            // .............................Code Above Commented and Added by Sam on 07122016...................................... 
            

            if (e.NewValues["SubAccount_RateOfInterest"] != null)
            {
                //lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_RateOfInterest"].ToString());
                lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_RateOfInterest"]));
            }
            else
                lcmd.Parameters.Add("@SubAccount_RateOfInterest", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            if (e.NewValues["SubAccount_Depreciation"] != null)
            {
                //lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["SubAccount_Depreciation"].ToString());
                lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(Convert.ToString(e.NewValues["SubAccount_Depreciation"]));
            }
            else
                lcmd.Parameters.Add("@SubAccount_Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");

            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();
            e.Cancel = true;
            bindgrid();
            SubAccountWithoutCustom.CancelEdit(); 

        }
        protected void SubAccountWithoutCustom_OnRowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            if (SubAccountWithoutCustom.IsNewRowEditing)
            {

            }
            else
            {

            }

        }
        
       
       
        protected void SubAccountWithoutCustom_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                SubAccountWithoutCustom.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                SubAccountWithoutCustom.FilterExpression = string.Empty;
            }
        } 

        #endregion SubAccountWithoutCustom 2

        protected void SubAccountWithoutCustom_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (SubAccountWithoutCustom.IsNewRowEditing && e.Column.FieldName == "SubAccount_IsFBT")
                (e.Editor as ASPxCheckBox).Checked = false;
        }

        
    }
}