using System;
using System.Data;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ClsDropDownlistNameSpace;


using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Web.Services;


namespace ERP.OMS.Management.Master
{
    public partial class TransporterMasterList : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        int NoOfRow = 0;
        string AllUserCntId;
        string cBranchId = "";
        public string pageAccess = "";
        static string Checking = null;
        public static string IsLighterCustomePage = string.Empty;
        clsDropDownList oclsDropDownList = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ShowHistoryLeadDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            hdnUserId.Value = HttpContext.Current.Session["userid"].ToString();

            if (HttpContext.Current.Session["userid"] == null)
            {

            }

            if (Session["ExchangeSegmentID"] == null)
            {

                TxtSeg.Value = "N";
            }
            else
            {
                TxtSeg.Value = "Y";



            }


            string PageType = Convert.ToString(Request.QueryString["requesttype"]);
            if (PageType == "Transporter")
            {
                EmployeeGrid.Columns[27].Width = 180;
            }
            else
            {
                EmployeeGrid.Columns[27].Width = 0;
            }

            if (!Page.IsPostBack)
            {
                this.cmbActivity.Attributes.Add("onclick", "return ddlICRChange();");

                this.cmbPriority.Attributes.Add("onclick", "return ddlPriorityChange();");
                rights = new UserRightsForPage();
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/TransporterMasterList.aspx?requesttype=" + Convert.ToString(Request.QueryString["requesttype"]) + "");

                hidIsLigherContactPage.Value = "0";
                IsLighterCustomePage = "";

                Session["exportval"] = null;
                Session["SalesActivityid"] = null;
                Session["ShowHistorySalesActivity"] = null;
                Session["SessionLeadActivityProduct"] = null;
                string requesttype = Convert.ToString(Request.QueryString["requesttype"]);

                Session["Contactrequesttype"] = requesttype;

                string ContType = "";
                switch (requesttype)
                {
                    case "customer":
                        ContType = "Customer/Client";
                        break;
                    case "OtherEntity":
                        ContType = "OtherEntity";
                        break;

                    case "subbroker":
                        ContType = "Sub Broker";
                        break;
                    case "franchisee":
                        ContType = "Franchisee";
                        break;
                    case "referalagent":
                        ContType = "Relationship Partners";
                        break;
                    case "broker":
                        ContType = "Broker";
                        break;
                    case "agent":

                        ContType = "Salesman/Agents";
                        break;
                    case "datavendor":
                        ContType = "Data Vendor";
                        break;
                    case "vendor":
                        ContType = "Vendor";
                        break;
                    case "partner":
                        ContType = "Partner";
                        break;
                    case "consultant":
                        ContType = "Consultant";
                        break;
                    case "shareholder":
                        ContType = "Share Holder";
                        break;
                    case "creditor":
                        ContType = "Creditors";
                        break;
                    case "debtor":
                        ContType = "Debtor";
                        break;
                    case "leadmanagers":
                        ContType = "Lead managers";
                        break;
                    case "bookrunners":
                        ContType = "Book Runners";
                        break;
                    case "listedcompanies":
                        ContType = "Companies-Listed";
                        break;
                    case "recruitmentagent":
                        ContType = "Relationship Manager";
                        break;
                    case "businesspartner":
                        ContType = "Business Partner";
                        break;
                    case "Transporter":
                        ContType = "Transporter";
                        break;
                    //For Leads
                    case "Lead":
                        ContType = "Lead";
                        break;
                }



                if (ContType == "Broker")
                {
                    td_broker1.Visible = true;
                    td_contact1.Visible = false;
                }
                else
                {
                    td_broker1.Visible = false;
                    td_contact1.Visible = true;
                }



                if (ContType == "Customer/Client")
                {
                    lblHeadTitle.Text = "Customers / Clients";
                    this.Title = "Customers / Clients";

                    CommonBL cbl = new CommonBL();
                    string ISLigherpage = cbl.GetSystemSettingsResult("LighterCustomerEntryPage");
                    if (!String.IsNullOrEmpty(ISLigherpage))
                    {
                        if (ISLigherpage == "Yes")
                        {
                            hidIsLigherContactPage.Value = "1";
                            IsLighterCustomePage = "1";
                        }
                    }
                }
                else if (ContType == "Franchisee")
                {
                    lblHeadTitle.Text = "Franchisees";
                    this.Title = "Franchisees";
                }

                else if (ContType == "Salesman/Agents")
                {

                    lblHeadTitle.Text = "Salesman/Agents";
                    this.Title = "Salesman/Agents";
                }
                else if (ContType == "Creditors")
                {
                    lblHeadTitle.Text = "Creditors";
                    this.Title = "Creditors";
                }
                else if (ContType == "Business Partner")
                {
                    lblHeadTitle.Text = "Business Partners";
                    this.Title = "Business Partners";
                }
                else if (ContType == "Book Runners")
                {
                    lblHeadTitle.Text = "Book Runners";
                    this.Title = "Book Runners";
                }
                else if (ContType == "Relationship Partners")
                {
                    lblHeadTitle.Text = "Influencer";
                    this.Title = "Influencer";
                }
                else if (ContType == "Partner")
                {
                    lblHeadTitle.Text = "Business Partners";
                    this.Title = "Business Partners";
                }
                else if (ContType == "OtherEntity")
                {
                    lblHeadTitle.Text = "Other Entity";
                    this.Title = "Other Entity";
                }
                else if (ContType == "Transporter")
                {
                    lblHeadTitle.Text = "Transporter";
                    this.Title = "Transporter";
                }
                else
                {
                    lblHeadTitle.Text = ContType + "s";
                    this.Title = ContType + "s";
                }

                //........................ Code Commented By Sam on 01112016.....................................
                Session["requesttype"] = ContType;
                AssignQuery();

                //-----------------------Subhra 16-05-2019--------------------------------
                if (ContType == "Lead")
                {
                    //EmployeeGrid.Columns[11].Visible = true;
                    //EmployeeGrid.Columns[12].Visible = true;
                    EmployeeGrid.Columns[7].Visible = true;
                    EmployeeGrid.Columns[8].Visible = true;

                }
                else if (ContType == "Customer/Client")
                {
                    //EmployeeGrid.Columns[11].Visible = true;
                    //EmployeeGrid.Columns[12].Visible = false;
                    EmployeeGrid.Columns[7].Visible = true;
                    EmployeeGrid.Columns[8].Visible = false;
                }
                else
                {
                    //EmployeeGrid.Columns[11].Visible = false;
                    //EmployeeGrid.Columns[12].Visible = false;
                    EmployeeGrid.Columns[7].Visible = false;
                    EmployeeGrid.Columns[8].Visible = false;
                }
                string[,] DataAssignTo = oDBEngine.GetFieldValue("tbl_master_user", "user_id, user_name", "user_inactive='N'", 2, "user_name");

                oclsDropDownList.AddDataToDropDownList(DataAssignTo, cmbAssignTo);

                string[,] DataConvertTo = oDBEngine.GetFieldValue("LeadStatus", "Id, Convertto_Text", null, 2, "Convertto_Text");

                oclsDropDownList.AddDataToDropDownList(DataConvertTo, cmbLeadstatus);
                ListItem LST = new ListItem("--Select--", "0");
                cmbLeadstatus.Items.Insert(0, LST);
                cmbLeadstatus.SelectedIndex = 0;

                string[,] DataActivity = oDBEngine.GetFieldValue("Lead_Activity", "Id, Lead_ActivityName", null, 2, "Lead_ActivityName");
                oclsDropDownList.AddDataToDropDownList(DataActivity, cmbActivity);
                ListItem LST_ActivityName = new ListItem("--Select--", "0");
                cmbActivity.Items.Insert(0, LST_ActivityName);
                cmbActivity.SelectedIndex = 0;

                string[,] DataSalesActivityAssignTo = oDBEngine.GetFieldValue("tbl_master_user", "user_id, user_name", "user_inactive='N'", 2, "user_name");
                oclsDropDownList.AddDataToDropDownList(DataSalesActivityAssignTo, cmbSalesActivityAssignTo);
                ListItem LST_ActivityAssignTo = new ListItem("--Select--", "0");
                cmbSalesActivityAssignTo.Items.Insert(0, LST_ActivityAssignTo);
                cmbSalesActivityAssignTo.SelectedIndex = 0;

                string[,] DataDuration = oDBEngine.GetFieldValue("Lead_Duration", "Id, DurationName", null, 2, "Id");
                oclsDropDownList.AddDataToDropDownList(DataDuration, cmbDuration);
                ListItem LST_Duration = new ListItem("--Select--", "0");
                cmbDuration.Items.Insert(0, LST_Duration);
                cmbDuration.SelectedIndex = 0;

                string[,] DataPriority = oDBEngine.GetFieldValue("Lead_Priority", "Id, PriorityName", null, 2, "PriorityName");
                oclsDropDownList.AddDataToDropDownList(DataPriority, cmbPriority);
                ListItem LST_Priority = new ListItem("--Select--", "0");
                cmbPriority.Items.Insert(0, LST_Priority);
                cmbPriority.SelectedIndex = 0;

                //-------------------------------------------------------------------------------
            }
            else
            {
                AssignQuery1();
                if (Session["ShowHistorySalesActivity"] == "top10ShowHistory")
                {
                    SalesActivityGridBind();
                }
                else if (Session["ShowHistorySalesActivity"] == "AllShowHistory")
                {
                    AllSalesActivityGridBind();
                }

            }
            //-----Rev---------------Subhra 04/06/2019-----------------------------------------------------
            #region time
            DtxtDue.TimeSectionProperties.Visible = true;
            DtxtDue.UseMaskBehavior = true;
            DtxtDue.EditFormatString = "dd-MM-yyyy hh:mm tt";

            dtActivityDate.TimeSectionProperties.Visible = true;
            dtActivityDate.UseMaskBehavior = true;
            dtActivityDate.EditFormatString = "dd-MM-yyyy hh:mm tt";
            #endregion

            DtxtDue.MinDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            //Rev Subhra 22-05-2019
            MasterSettings objmasterposprint = new MasterSettings();
            hdnUserRestrictionForLeadAction.Value = objmasterposprint.GetSettings("UserRestrictionforLeadAction");
            //End of Rev Subhra 22-05-2019
            //----End of Rev---------Subhra 04/06/2019------------------------------------------------------
        }
        protected void AssignQuery()
        {
            string requesttype1 = Convert.ToString(Session["requesttype"]);

            if (requesttype1 == "Customer/Client")
            {
                DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AllUserCntId = Convert.ToString(CntId.Tables[0].Rows[i]["user_contactid"]);
                    }
                    else
                    {
                        AllUserCntId += "," + Convert.ToString(CntId.Tables[0].Rows[i]["user_contactid"]);
                    }

                }


                EmployeeDataSource.SelectCommand = "select * from (select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin, (select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,'' AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status, Case tbl_master_contact.Statustype when 'A' then 'Active' when 'D' then 'Dormant' END as Activetype,  tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (select branch_id from tbl_master_branch) or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_contactType= 'CL' ) as D order by CrDate desc ";


            }

            else if (requesttype1 == "Franchisee")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "Salesman/Agents")
            {
                EmployeeDataSource.SelectCommand = "select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'AG%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "OtherEntity")
            {
                EmployeeDataSource.SelectCommand = "select *  from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "Salesman/Agents")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 phf_phoneNumber from tbl_master_phonefax where ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Data Vendor")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DV%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Vendor")
            {
                EmployeeDataSource.SelectCommand = "select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'VR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Partner")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                         " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Consultant")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Share Holder")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Creditors")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Debtor")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Lead managers")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                         " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LM%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Book Runners")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card')and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'BS%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Companies-Listed")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                        " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";
            }

            else if (requesttype1 == "Relationship Partners")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                          " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Transporter")
            {
                //EmployeeDataSource.SelectCommand = "select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,case when tbl_master_branch.branch_description is null then 'ALL' else  tbl_master_branch.branch_description end AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                //        " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                //       " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                //       " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                //       " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,"+
                //       " CONVERT(VARCHAR(11),tbl_master_contact.CreateDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.CreateDate, 108) as EnteredDate " +
                //       " from tbl_master_contact left JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'TR%' and (tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_branchid =0)) as D order by CrDate desc";

                EmployeeDataSource.SelectCommand = "select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,case when tbl_master_branch.branch_description is null then 'ALL' else  tbl_master_branch.branch_description end AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                            " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                           " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                           " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                           " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating," +
                           " CONVERT(VARCHAR(11),tbl_master_contact.CreateDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.CreateDate, 108) as EnteredDate " +
                           " ,cnt_mainAccount Account,BillingContact,BillingAddress,BillingCity,BillingState,BillingCountry,BillingPIN " +
                           " ,ShippingContact,ShippingAddress1,ShippingCity,ShippingState,ShippingCountry,ShippingPIN " +
                           " ,case TDSRATE_TYPE when 1 then 'Individual/HUF' when 2 then 'Others'   end AS TDSRATE_TYPE" +
                            " ,case when tbl_master_contact.CNT_TAX_ENTITYTYPE='O' then 'Others' when tbl_master_contact.CNT_TAX_ENTITYTYPE='NG' then 'Non-government'  when tbl_master_contact.CNT_TAX_ENTITYTYPE='G' then 'Government' else '' end TaxEntityType" +
                           " from tbl_master_contact " +
                            " left JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id " +
                             " left outer join " +
                             " (select add_cntId,add_addressType,contactperson BillingContact,Isdefault ,add_address1 BillingAddress,city_name BillingCity,state BillingState " +
                             " ,cou_country BillingCountry,pin_code BillingPIN " +
                             " from tbl_master_address" +
                             " left outer join tbl_master_country on cou_id=add_country" +
                             " left outer join tbl_master_state on id=add_state" +
                             " left outer join tbl_master_city on city_id=add_city" +
                             " left outer join  tbl_master_pinzip on pin_id=add_pin" +
                             " )tblBilling on cnt_internalId=add_cntId and add_addressType='Billing' and Isdefault=1" +

                             " left outer join" +
                             " (select add_cntId ShippingID,add_addressType ShippingType,contactperson ShippingContact,Isdefault ShippingDefault,add_address1 ShippingAddress1,city_name ShippingCity,state ShippingState" +
                             " ,cou_country ShippingCountry,pin_code ShippingPIN" +
                             " from tbl_master_address" +
                             " left outer join tbl_master_country on cou_id=add_country" +
                             " left outer join tbl_master_state on id=add_state" +
                             " left outer join tbl_master_city on city_id=add_city" +
                             " left outer join  tbl_master_pinzip on pin_id=add_pin" +
                             " )tblShipping on cnt_internalId=ShippingID and ShippingType='Shipping' and ShippingDefault=1" +

                            " inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'TR%' " +
                            " and (tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_branchid =0)" +
                            " ) as D order by CrDate desc";





            }
            //For Leads
            else if (requesttype1 == "Lead")
            {
                string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);


                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable(@"select user_IsUserwise from tbl_master_user where user_id='" + Session["userid"] + "'");

                bool is_userwise = Convert.ToBoolean(dt.Rows[0]["user_IsUserwise"]);


                //---------Rev Subhra 17-05-2019
                //EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                //       " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,"+
                //        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime,"+
                //        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser"+
                //        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_contactType= 'LD' " + (is_userwise? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in (" + userbranchHierarchy + ")) as D order by CrDate desc";

                //EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                //     " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy," +
                //      " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                //      " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,u.user_name as Assign_To,ls.Convertto_Text as Convert_To,case when Correcttocust=1 then 1 else 0 end as IsConvert " +
                //      " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus "+
                //      " on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id left outer join tbl_master_user u on u.user_id=tbl_master_contact.Assign_To "+
                //      " left outer join LeadStatus ls on ls.Id=tbl_master_contact.LeadStatus " +
                //      "where  cnt_contactType= 'LD' " + (is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in (" + userbranchHierarchy + ")) as D order by CrDate desc";

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                     " ,isnull(entrd.user_name,'') as EnterBy,entrd.user_id as enteredby_id, " +
                      " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                      " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser," +
                      "u.user_name as Assign_To,ls.Convertto_Text as Convert_To,case when Correcttocust=1 then 1 else 0 end as IsConvert,u.user_id as Assign_Id," +
                      " src.cntsrc_sourceType as 'Source',lr.rat_LeadRating as 'Rating',CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as EnteredDate " +

                      " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus " +
                      " on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id left outer join tbl_master_user u on u.user_id=tbl_master_contact.Assign_To " +
                      " left outer join tbl_master_ContactSource src on src.cntsrc_id=tbl_master_contact.cnt_contactSource " +
                      " left outer join tbl_master_leadRating lr on lr.rat_id=tbl_master_contact.cnt_rating " +
                      " left outer join tbl_master_user entrd on entrd.user_id=tbl_master_contact.CreateUser" +
                      " left outer join LeadStatus ls on ls.Id=tbl_master_contact.LeadStatus " +
                    //-----------------Rev Subhra 0020380 12-06-2019-------------------
                    //" where  cnt_contactType= 'LD' " + (is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in (" + userbranchHierarchy + ")) as D order by CrDate desc";
                       " where  cnt_internalId like 'LD%' " + (is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in (" + userbranchHierarchy + ")) as D order by CrDate desc";
                //End of Rev Subhra 0020380 12-06-2019-------------------


                //---------Rev Subhra 17-05-2019
            }


            EmployeeGrid.DataBind();

        }

        protected void AssignQuery1()
        {

            string requesttype1 = Convert.ToString(Session["requesttype"]);

            if (requesttype1 == "Customer/Client")
            {
                DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AllUserCntId = Convert.ToString(CntId.Tables[0].Rows[i]["user_contactid"]);
                    }
                    else
                    {
                        AllUserCntId += "," + Convert.ToString(CntId.Tables[0].Rows[i]["user_contactid"]);
                    }

                }

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin, (select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,Case tbl_master_contact.Statustype when 'A' then 'Active' when 'D' then 'Dormant' END as Activetype,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (select branch_id from tbl_master_branch) or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "'))   and cnt_contactType= 'CL' ) as D order by CrDate desc ";

            }
            else if (requesttype1 == "Sub Broker")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SB%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Franchisee")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                            " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Relationship Partners")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "OtherEntity")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "Salesman/Agents")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'AG%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Data Vendor")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DV%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Vendor")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'VR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Partner")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Consultant")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Share Holder")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Creditors")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                            " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Debtor")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Lead managers")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LM%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Book Runners")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'BS%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Companies-Listed")
            {

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }

            else if (requesttype1 == "Relationship Partners")
            {
                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                        " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";

            }
            else if (requesttype1 == "Transporter")
            {
                //EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,case when tbl_master_branch.branch_description is null then 'ALL' else  tbl_master_branch.branch_description end AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                //           " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                //      " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                //      " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                //      " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating,null as EnteredDate " +
                //      " from tbl_master_contact left JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'TR%' and (tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_branchid =0)) as D order by CrDate desc";


                EmployeeDataSource.SelectCommand = "select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,case when tbl_master_branch.branch_description is null then 'ALL' else  tbl_master_branch.branch_description end AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate " +
                              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy,'' as enteredby_id," +
                             " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                             " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                             " ,'' as Assign_To,'' as Convert_To,0 as IsConvert,0 as Assign_Id,'' as Source,'' as Rating," +
                             " CONVERT(VARCHAR(11),tbl_master_contact.CreateDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.CreateDate, 108) as EnteredDate " +
                             " ,cnt_mainAccount Account,BillingContact,BillingAddress,BillingCity,BillingState,BillingCountry,BillingPIN " +
                             " ,ShippingContact,ShippingAddress1,ShippingCity,ShippingState,ShippingCountry,ShippingPIN " +
                             " ,case TDSRATE_TYPE when 1 then 'Individual/HUF' when 2 then 'Others'   end AS TDSRATE_TYPE" +
                            " ,case when tbl_master_contact.CNT_TAX_ENTITYTYPE='O' then 'Others' when tbl_master_contact.CNT_TAX_ENTITYTYPE='NG' then 'Non-government'  when tbl_master_contact.CNT_TAX_ENTITYTYPE='G' then 'Government' else '' end TaxEntityType" +

                             " from tbl_master_contact " +
                              " left JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id " +
                               " left outer join " +
                               " (select add_cntId,add_addressType,contactperson BillingContact,Isdefault ,add_address1 BillingAddress,city_name BillingCity,state BillingState " +
                               " ,cou_country BillingCountry,pin_code BillingPIN " +
                               " from tbl_master_address" +
                               " left outer join tbl_master_country on cou_id=add_country" +
                               " left outer join tbl_master_state on id=add_state" +
                               " left outer join tbl_master_city on city_id=add_city" +
                               " left outer join  tbl_master_pinzip on pin_id=add_pin" +
                               " )tblBilling on cnt_internalId=add_cntId and add_addressType='Billing' and Isdefault=1" +

                               " left outer join" +
                               " (select add_cntId ShippingID,add_addressType ShippingType,contactperson ShippingContact,Isdefault ShippingDefault,add_address1 ShippingAddress1,city_name ShippingCity,state ShippingState" +
                               " ,cou_country ShippingCountry,pin_code ShippingPIN" +
                               " from tbl_master_address" +
                               " left outer join tbl_master_country on cou_id=add_country" +
                               " left outer join tbl_master_state on id=add_state" +
                               " left outer join tbl_master_city on city_id=add_city" +
                               " left outer join  tbl_master_pinzip on pin_id=add_pin" +
                               " )tblShipping on cnt_internalId=ShippingID and ShippingType='Shipping' and ShippingDefault=1" +

                              " inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'TR%' " +
                              " and (tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_branchid =0)" +
                              " ) as D order by CrDate desc";



            }
            //For Leads
            else if (requesttype1 == "Lead")
            {
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable(@"select user_IsUserwise from tbl_master_user where user_id='" + Session["userid"] + "'");

                bool is_userwise = Convert.ToBoolean(dt.Rows[0]["user_IsUserwise"]);
                //---------Rev Subhra 20-05-2019
                //EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                //              " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy," +
                //        " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                //        " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser" +
                //         " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%'"+(is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in(select branch_id from tbl_master_branch)) as D order by CrDate desc";

                //EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                //             " ,(Select user_name from tbl_master_user where user_id=tbl_master_contact.CreateUser)as EnterBy," +
                //       " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                //       " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser,u.user_name as Assign_To,ls.Convertto_Text as Convert_To,case when Correcttocust=1 then 1 else 0 end as IsConvert " +
                //        " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id "+
                //        " inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id "+
                //        " left outer join tbl_master_user u on u.user_id=tbl_master_contact.Assign_To " +
                //        " left outer join LeadStatus ls on ls.Id=tbl_master_contact.LeadStatus " +
                //        " where  cnt_internalId like 'LD%'" + (is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in(select branch_id from tbl_master_branch)) as D order by CrDate desc";

                EmployeeDataSource.SelectCommand = "select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,cnt_gstin gstin,(select top 1 crg_number  from tbl_master_contactRegistration where (crg_type='Pancard' or crg_type='Pan card') and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate" +
                      " ,isnull(entrd.user_name,'') as EnterBy,entrd.user_id as enteredby_id, " +
                     " CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as ModifyDateTime," +
                     " (Select user_name from tbl_master_user where user_id=tbl_master_contact.LastModifyUser)as ModifyUser, " +
                      " u.user_name as Assign_To,ls.Convertto_Text as Convert_To,case when Correcttocust=1 then 1 else 0 end as IsConvert," +
                      " u.user_id as Assign_Id,src.cntsrc_sourceType as 'Source',lr.rat_LeadRating as 'Rating',CONVERT(VARCHAR(11),tbl_master_contact.LastModifyDate, 105) + ' ' + CONVERT(VARCHAR(8), tbl_master_contact.LastModifyDate, 108) as EnteredDate  " +
                      " from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id " +
                      " inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id " +
                      " left outer join tbl_master_ContactSource src on src.cntsrc_id=tbl_master_contact.cnt_contactSource " +
                      " left outer join tbl_master_leadRating lr on lr.rat_id=tbl_master_contact.cnt_rating " +
                      " left outer join tbl_master_user u on u.user_id=tbl_master_contact.Assign_To " +
                      " left outer join tbl_master_user entrd on entrd.user_id=tbl_master_contact.CreateUser " +
                      " left outer join LeadStatus ls on ls.Id=tbl_master_contact.LeadStatus " +
                      " where  cnt_internalId like 'LD%'" + (is_userwise ? "and tbl_master_contact.CreateUser=" + Session["userid"].ToString() : "") + " and tbl_master_contact.cnt_branchid in(select branch_id from tbl_master_branch)) as D order by CrDate desc";

                //---------Rev Subhra 20-05-2019





            }
            EmployeeGrid.DataBind();

        }

        public void bindexport(int Filter)
        {
            EmployeeGrid.Columns[7].Visible = false;
            //EmployeeGrid.Columns[12].Visible = false;

            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Lead"));
            exporter.FileName = filename;

            exporter.PageHeader.Left = Convert.ToString((Session["Contactrequesttype"] ?? "Lead"));
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
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
                case 5:
                    exporter.WriteXlsxToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }


        protected void addnew(object sender, EventArgs e)
        {
            EmployeeGrid.AddNewRow();
            ASPxPageControl pageControl = EmployeeGrid.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
            TabPage corres = pageControl.TabPages.FindByName("General");
            corres.Visible = false;

        }
        protected void EmployeeGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError"] = "ADD";
        }
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            EmployeeGrid.JSProperties["cpDelete"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

                }
                if (WhichCall == "Delete")
                {
                    MasterDataCheckingBL objMasterDataCheckingBL = new MasterDataCheckingBL();

                    deletecnt = objMasterDataCheckingBL.DeleteLeadOrContact(WhichType);
                    if (deletecnt > 0)
                    {

                        EmployeeGrid.JSProperties["cpDelete"] = "Success";
                        AssignQuery1();
                    }
                    else
                        EmployeeGrid.JSProperties["cpDelete"] = "Fail";
                }
            }
            EmployeeGrid.ClearSort();

            if (e.Parameters == "ssss")
            {

                EmployeeGrid.Settings.ShowFilterRow = true;



            }
            if (e.Parameters == "All")
            {
                EmployeeGrid.FilterExpression = string.Empty;
                Checking = "All";
                AssignQuery1();
            }
            if (e.Parameters == "")
            {

                AssignQuery1();
            }


        }


        protected void EmployeeGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
                //string ContactID = Convert.ToString(e.GetValue("Id"));
                //string CntName = Convert.ToString(e.GetValue("Name"));
                //e.Row.Cells[0].Style.Add("cursor", "hand");
                //e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "','" + CntName + "');");
                //e.Row.Cells[0].ToolTip = "Click to View Not Available Records!";
                // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 

                //if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                //{
                //    e.Row.Cells[2].Style.Add("display", "none");
                //}
                //else { e.Row.Cells[2].Style.Add("display", "block"); }

            }

        }

        protected void EmployeeGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (Convert.ToString(Session["Contactrequesttype"]) != "Lead")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((Convert.ToString(c.FieldName)).StartsWith("Status"))
                    {
                        c.Visible = false;
                    }
                    if ((Convert.ToString(c.FieldName)).StartsWith("PAN"))
                    {
                        c.Visible = false;
                    }
                }

            }
            if (Convert.ToString(Session["Contactrequesttype"]) != "customer")
            {
                foreach (GridViewDataColumn d in grid.Columns)
                {
                    if ((Convert.ToString(d.FieldName)).StartsWith("Activetype"))
                    {
                        d.Visible = false;
                    }
                    if ((Convert.ToString(d.FieldName)).StartsWith("gstin"))
                    {
                        d.Visible = false;
                    }
                    if ((Convert.ToString(d.FieldName)).StartsWith("PAN"))
                    {
                        d.Visible = false;
                    }
                }

            }
            if (Convert.ToString(Session["Contactrequesttype"]) == "Transporter")
            {
                foreach (GridViewDataColumn d in grid.Columns)
                {

                    if ((Convert.ToString(d.FieldName)).StartsWith("gstin"))
                    {
                        d.Visible = true;
                    }

                }

            }

        }

        //---------------Subhra 16-05-2019-------------------------------
        protected void PopupAssign_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Convert.ToString(Session["Contactrequesttype"]) == "Lead")
            {
                string strSplitCommand = e.Parameter.Split('~')[0];
                string entity_id = e.Parameter.Split('~')[1];


                if (strSplitCommand == "Load")
                {
                    string name = e.Parameter.Split('~')[2];
                    DataTable dtchkassign = new DataTable();
                    dtchkassign = oDBEngine.GetDataTable("select Assign_To,Assign_Remarks,CreateUser from tbl_master_contact where cnt_internalId='" + entity_id + "'");
                    if (dtchkassign.Rows.Count > 0)
                    {
                        if (dtchkassign.Rows[0][0].ToString() == "")
                        {
                            CallbackPanelAssign.JSProperties["cpName"] = name;
                            CallbackPanelAssign.JSProperties["cpAssignTo"] = "";
                            CallbackPanelAssign.JSProperties["cpRemarks"] = "";
                            CallbackPanelAssign.JSProperties["cpEnteredBy"] = Convert.ToInt32(dtchkassign.Rows[0][2]);
                            CallbackPanelAssign.JSProperties["cpAssignSave"] = "";
                        }
                        else
                        {
                            CallbackPanelAssign.JSProperties["cpName"] = name;
                            CallbackPanelAssign.JSProperties["cpAssignTo"] = dtchkassign.Rows[0][0].ToString();
                            CallbackPanelAssign.JSProperties["cpRemarks"] = dtchkassign.Rows[0][1].ToString();
                            CallbackPanelAssign.JSProperties["cpEnteredBy"] = 0;
                            CallbackPanelAssign.JSProperties["cpAssignSave"] = "";
                        }
                    }

                    //hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                }
                else if (strSplitCommand == "Assign")
                {
                    int OutputId = 0;
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_LEAD_ASSIGN", con);
                    DataTable dtReceipt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ACTION_TYPE", "ASSIGN");
                    cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", entity_id);
                    cmd.Parameters.AddWithValue("@LEAD_CONTACTTYPE", "LD");
                    cmd.Parameters.AddWithValue("@USERID", cmbAssignTo.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@ASSIGN_REMARKS", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));

                    SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    output.Direction = ParameterDirection.Output;

                    SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputText.Direction = ParameterDirection.Output;


                    cmd.Parameters.Add(output);
                    cmd.Parameters.Add(outputText);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();


                    OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                    string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                    CallbackPanelAssign.JSProperties["cpAssignSave"] = "Save";
                }
                else if (strSplitCommand == "Unassign")
                {
                    int OutputId = 0;
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_LEAD_ASSIGN", con);
                    DataTable dtReceipt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ACTION_TYPE", "UNASSIGN");
                    cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", entity_id);
                    cmd.Parameters.AddWithValue("@LEAD_CONTACTTYPE", "LD");
                    cmd.Parameters.AddWithValue("@USERID", cmbAssignTo.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@ASSIGN_REMARKS", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));


                    SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    output.Direction = ParameterDirection.Output;

                    SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputText.Direction = ParameterDirection.Output;



                    cmd.Parameters.Add(output);
                    cmd.Parameters.Add(outputText);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();


                    OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                    string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                    CallbackPanelAssign.JSProperties["cpAssignSave"] = "Save";
                }


            }
        }

        protected void CallbackPanelConvertto_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Convert.ToString(Session["Contactrequesttype"]) == "Lead")
            {
                string strSplitCommand = e.Parameter.Split('~')[0];
                string entity_id = e.Parameter.Split('~')[1];
                if (strSplitCommand == "Load")
                {
                    string name = e.Parameter.Split('~')[2];
                    DataTable dtchkconvert = new DataTable();
                    dtchkconvert = oDBEngine.GetDataTable("select LeadStatus,Convert_Remarks from tbl_master_contact where cnt_internalId='" + entity_id + "'");

                    if (dtchkconvert.Rows[0][0].ToString() == "")
                    {
                        CallbackPanelConvertto.JSProperties["cpName"] = name;
                        CallbackPanelConvertto.JSProperties["cpLeadStatus"] = "";
                        CallbackPanelConvertto.JSProperties["cpConverttoRemarks"] = "";
                        CallbackPanelConvertto.JSProperties["cpConverttoSave"] = "";

                    }
                    else
                    {
                        CallbackPanelConvertto.JSProperties["cpName"] = name;
                        CallbackPanelConvertto.JSProperties["cpLeadStatus"] = dtchkconvert.Rows[0][0].ToString();
                        CallbackPanelConvertto.JSProperties["cpConverttoRemarks"] = dtchkconvert.Rows[0][1].ToString();
                        CallbackPanelConvertto.JSProperties["cpConverttoSave"] = "";
                    }


                }
                else if (strSplitCommand == "Save")
                {
                    int OutputId = 0;
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_LEAD_CONVERT", con);
                    DataTable dtReceipt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ACTION_TYPE", "SAVE");
                    cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", entity_id);
                    cmd.Parameters.AddWithValue("@LEAD_STATUS", cmbLeadstatus.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@CONVERT_REMARKS", txtConvertToRemarks.Text);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));

                    SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    output.Direction = ParameterDirection.Output;

                    SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputText.Direction = ParameterDirection.Output;


                    cmd.Parameters.Add(output);
                    cmd.Parameters.Add(outputText);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();
                    CallbackPanelConvertto.JSProperties["cpConverttoSave"] = "Save";
                }
            }
        }

        protected void CallbackPanelLeadActivity_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];


            if (strSplitCommand == "Activity_Type")
            {
                int leadactivityid = Convert.ToInt32(e.Parameter.Split('~')[1]);
                string[,] DataActivityType = oDBEngine.GetFieldValue("Lead_ActivityType", "Id, Lead_ActivityTypeName", "LeadActivityId='" + leadactivityid + "'", 2, "Lead_ActivityTypeName");
                oclsDropDownList.AddDataToDropDownList(DataActivityType, cmbType);
                ListItem LST_ActivityType = new ListItem("--Select--", "0");
                cmbType.Items.Insert(0, LST_ActivityType);
                cmbType.SelectedIndex = 0;
            }
            else if (strSplitCommand == "Save")
            {
                string leadactivityid = Convert.ToString(e.Parameter.Split('~')[1]);
                int activity = Convert.ToInt32(e.Parameter.Split('~')[2]);
                int activity_type = Convert.ToInt32(e.Parameter.Split('~')[3]);
                string subject = Convert.ToString(e.Parameter.Split('~')[4]);
                string details = Convert.ToString(e.Parameter.Split('~')[5]);
                int assignto = Convert.ToInt32(e.Parameter.Split('~')[6]);
                int duration = Convert.ToInt32(e.Parameter.Split('~')[7]);
                int priority = Convert.ToInt32(e.Parameter.Split('~')[8]);


                DataTable dt_activityproducts = new DataTable();
                dt_activityproducts.Columns.Add("Id", typeof(string));
                dt_activityproducts.Columns.Add("ProdId", typeof(Int32));
                dt_activityproducts.Columns.Add("Act_Prod_Qty", typeof(Decimal));
                dt_activityproducts.Columns.Add("Act_Prod_Rate", typeof(Decimal));
                dt_activityproducts.Columns.Add("Act_Prod_Remarks", typeof(String));

                if (HttpContext.Current.Session["SessionLeadActivityProduct"] != null)
                {
                    List<LeadActivityProductDetailsBL> obj = new List<LeadActivityProductDetailsBL>();
                    obj = (List<LeadActivityProductDetailsBL>)HttpContext.Current.Session["SessionLeadActivityProduct"];
                    foreach (var item in obj)
                    {
                        dt_activityproducts.Rows.Add(item.guid, item.ProductId, item.Quantity, item.Rate, item.Remarks);
                    }
                }


                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);
                DataTable dtReceipt = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "SAVE");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", activity);
                cmd.Parameters.AddWithValue("@TYPEID", activity_type);
                cmd.Parameters.AddWithValue("@LEADSUBJECT", subject);
                cmd.Parameters.AddWithValue("@LEADDETAILS", details);
                cmd.Parameters.AddWithValue("@ASSIGNTO", assignto);
                cmd.Parameters.AddWithValue("@DURATIONID", duration);
                cmd.Parameters.AddWithValue("@PRIORITYID", priority);
                cmd.Parameters.AddWithValue("@DUEDATE", DtxtDue.Date);
                cmd.Parameters.AddWithValue("@ACTIVITYDATE", dtActivityDate.Date);
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", leadactivityid);
                cmd.Parameters.AddWithValue("@MODULENAME", "Lead Master");
                cmd.Parameters.AddWithValue("@CONTACTTYPE", "LD");
                cmd.Parameters.AddWithValue("@ActivityProducts", dt_activityproducts);

                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                CallbackPanelLeadActivity.JSProperties["cpStatusLeadActivity"] = "Save";
            }
            else if (strSplitCommand == "Load")
            {
                string entity_id = e.Parameter.Split('~')[1];
                string name = e.Parameter.Split('~')[2];
                DataTable dtchkassign = new DataTable();
                DataTable dtchkassignproduct = new DataTable();



                dtchkassign = oDBEngine.GetDataTable("select top 1 sa.Id as SalesActivityId,sa.Lead_Entity_id,isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') as Name," +
                    "la.Id as ActivityId,la.Lead_ActivityName as ActivityName,lat.Id as ActivityTypeId,lat.Lead_ActivityTypeName as ActivityTypeName," +
                "pr.Id as PriorityId,pr.PriorityName as PriorityName,ld.Id as DurationId,ld.DurationName as DurationName,cn.Assign_To,Convert(varchar,ISNULL(sa.Duedate,'') ,105) as Duedate" +
                " from Lead_Sales_Activity sa inner join Lead_Activity la on sa.Lead_activityid=la.Id" +
                " inner join Lead_ActivityType lat on lat.Id=sa.Typeid" +
                " inner join Lead_Priority pr on pr.Id=sa.Priorityid" +
                " inner join Lead_Duration ld on ld.Id=sa.Durationid" +
                " inner join tbl_master_contact cn on cn.cnt_internalId=sa.Lead_Entity_id" +
                " where Lead_Entity_id='" + entity_id + "' order by Created_date desc ");


                dtchkassignproduct = oDBEngine.GetDataTable("select  CAST(ap.Id as VARCHAR(100)) as [guid],ap.ActivityId as ActivityId,ap.Lead_Entity_id as Lead_Entity_id," +
                 " cast(ap.ProdId as int) as ProductId,p.sProducts_Description as ProductName,cast(ap.Act_Prod_Qty as decimal(18,2)) as Quantity," +
                 " cast(ap.Act_Prod_Rate as decimal(18,2)) as Rate,ap.Act_Prod_Remarks as Remarks " +
                 " from " +
                 " (select top 1 sa.Id from Lead_Sales_Activity sa where sa.Lead_Entity_id='" + entity_id + "' order by Created_date desc) as tsa " +
                 " inner join Activity_Products  ap on tsa.Id=ap.ActivityId " +
                 " inner join Master_sProducts p on p.sProducts_ID=ap.ProdId ");





                if (dtchkassign.Rows.Count > 0)
                {
                    CallbackPanelLeadActivity.JSProperties["cpLeadName"] = name;
                    CallbackPanelLeadActivity.JSProperties["cpDueDate"] = dtchkassign.Rows[0][12].ToString();
                    CallbackPanelLeadActivity.JSProperties["cpPriority"] = dtchkassign.Rows[0][8].ToString();
                    if (Convert.ToString(dtchkassign.Rows[0][11]) != "")
                    {
                        CallbackPanelLeadActivity.JSProperties["cpAssignto"] = dtchkassign.Rows[0][11].ToString();
                    }
                    if (dtchkassignproduct.Rows.Count > 0)
                    {
                        List<LeadActivityProductDetailsBL> finalResult = DbHelpers.ToModelList<LeadActivityProductDetailsBL>(dtchkassignproduct);
                        CallbackPanelLeadActivity.JSProperties["cpProductDetails"] = finalResult;
                    }
                }
                else
                {
                    CallbackPanelLeadActivity.JSProperties["cpLeadName"] = name;
                    CallbackPanelLeadActivity.JSProperties["cpDueDate"] = "";
                    CallbackPanelLeadActivity.JSProperties["cpPriority"] = "";
                    CallbackPanelLeadActivity.JSProperties["cpAssignto"] = HttpContext.Current.Session["userid"].ToString();
                    CallbackPanelLeadActivity.JSProperties["cpProductDetails"] = "";
                }
                CallbackPanelLeadActivity.JSProperties["cpStatusLeadActivity"] = "Load";

            }
        }


        protected void CallbackPanelShowHistory_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strcomnd = "";
            string strLeadEntityid = "";
            strcomnd = e.Parameter.Split('~')[0];
            strLeadEntityid = e.Parameter.Split('~')[1];
            Session["SalesActivityid"] = strLeadEntityid;
            Session["ShowHistorySalesActivity"] = strcomnd;
            if (strcomnd == "top10ShowHistory")
            {
                SalesActivityGridBind();
            }
            else if (strcomnd == "AllShowHistory")
            {
                AllSalesActivityGridBind();
            }
            else if (strcomnd == "Delete")
            {
                int leadid = Convert.ToInt32(e.Parameter.Split('~')[2]);
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);
                DataTable dtReceipt = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", "DELETE");
                cmd.Parameters.AddWithValue("@LEADID", leadid);

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                CallbackPanelShowHistory.JSProperties["cpStatusLeadActivity"] = "Delete";
            }
        }

        public void SalesActivityGridBind()
        {
            ShowHistoryLeadDataSource.SelectCommand = "select top 10 sa.Id as SalesActivityId,sa.Lead_Entity_id,isnull(cn.cnt_firstName,'')+' '+isnull(cn.cnt_middleName,'')+' '+isnull(cn.cnt_lastName,'') as Name," +
                "la.Id as ActivityId,la.Lead_ActivityName as ActivityName,lat.Id as ActivityTypeId,lat.Lead_ActivityTypeName as ActivityTypeName,sa.Leadsubject,sa.Leaddetails,pr.Id as PriorityId," +
                "pr.PriorityName as PriorityName,ld.Id as DurationId,ld.DurationName as DurationName,cn.Assign_To,u.user_name as AssignTo_Name," +
                "Convert(varchar,ISNULL(sa.Duedate,'') ,105) as Duedate,Created_date,sa.ModuleName,Convert(varchar,ISNULL(sa.ActivityDate,'') ,105) as ActivityDate,sa.ContactType " +
                " from Lead_Sales_Activity sa inner join Lead_Activity la on sa.Lead_activityid=la.Id " +
                " inner join Lead_ActivityType lat on lat.Id=sa.Typeid " +
                " inner join Lead_Priority pr on pr.Id=sa.Priorityid " +
                " inner join Lead_Duration ld on ld.Id=sa.Durationid " +
                " inner join tbl_master_contact cn on cn.cnt_internalId=sa.Lead_Entity_id " +
                " left outer join tbl_master_user u on u.user_id=cn.Assign_To " +
                " where Lead_Entity_id='" + Convert.ToString(Session["SalesActivityid"]) + "' " +
                " order by Created_date desc ";
            ShowHistoryLeadDataSource.DataBind();
            showhistorygrid.DataBind();
        }
        public void AllSalesActivityGridBind()
        {
            ShowHistoryLeadDataSource.SelectCommand = "select sa.Id as SalesActivityId,sa.Lead_Entity_id,isnull(cn.cnt_firstName,'')+' '+isnull(cn.cnt_middleName,'')+' '+isnull(cn.cnt_lastName,'') as Name," +
                "la.Id as ActivityId,la.Lead_ActivityName as ActivityName,lat.Id as ActivityTypeId,lat.Lead_ActivityTypeName as ActivityTypeName,sa.Leadsubject,sa.Leaddetails,pr.Id as PriorityId," +
                "pr.PriorityName as PriorityName,ld.Id as DurationId,ld.DurationName as DurationName,cn.Assign_To,u.user_name as AssignTo_Name," +
                "Convert(varchar,ISNULL(sa.Duedate,'') ,105) as Duedate,Created_date,sa.ModuleName,Convert(varchar,ISNULL(sa.ActivityDate,'') ,105) as ActivityDate,sa.ContactType " +
                " from Lead_Sales_Activity sa inner join Lead_Activity la on sa.Lead_activityid=la.Id " +
                " inner join Lead_ActivityType lat on lat.Id=sa.Typeid " +
                " inner join Lead_Priority pr on pr.Id=sa.Priorityid " +
                " inner join Lead_Duration ld on ld.Id=sa.Durationid " +
                " inner join tbl_master_contact cn on cn.cnt_internalId=sa.Lead_Entity_id " +
                " left outer join tbl_master_user u on u.user_id=cn.Assign_To " +
                " where Lead_Entity_id='" + Convert.ToString(Session["SalesActivityid"]) + "' " +
                " order by Created_date desc ";
            ShowHistoryLeadDataSource.DataBind();
            showhistorygrid.DataBind();
        }

        [WebMethod]
        public static string SaveLeadActivityProductDetails(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<LeadActivityProductDetailsBL> finalResult = jsSerializer.Deserialize<List<LeadActivityProductDetailsBL>>(list);
            HttpContext.Current.Session["SessionLeadActivityProduct"] = finalResult;

            return null;
        }
        public class LeadActivityProductDetailsBL
        {
            public string guid { get; set; }
            public int ActivityId { get; set; }
            public string Lead_Entity_id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public decimal Rate { get; set; }
            public string Remarks { get; set; }
        }

        //--------------------------------------------------------------------
    }
}
